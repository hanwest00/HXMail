using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Mime;
using System.Net.Mail;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Reflection;
using HMail.MailEntity;

namespace HMail
{
    public class HMail : IDisposable
    {
        private UserInfo userInfo;
        private int mailCount;
        private TcpClient mailClient = new TcpClient();
        private static SmtpClient smtpClient = new SmtpClient();
        private StreamWriter popStreamWriter;
        private StreamReader popStreamReader;
        private NetworkStream ns;
        private bool popAccessed;
        private static string mailEncoding;
        private string result;
        private Regex reg;
        private int currentMailId;
        private IList<HMailInfo> listMsg = new List<HMailInfo>();

        /// <summary>
        /// popserver链接状态
        /// </summary>
        public bool PopAccessed
        {
            get
            {
                return popAccessed;
            }
        }

        public UserInfo UserInfo
        {
            get { return userInfo; }
            set { userInfo = value; }
        }

        /// <summary>
        /// 获取到的邮件总数
        /// </summary>
        public int MailCount
        {
            get { return mailCount; }
        }

        public string CurrentMailString
        {
            get { return result; }
        }

        public int CurrentMailId
        {
            get { return currentMailId; }
        }

        /// <summary>
        /// 连接popserver失败时事件
        /// </summary>
        /// <param name="ex">catch到的Exception</param>
        public delegate void AccessPopFailed(Exception ex);
        public event AccessPopFailed AccessPopFailedEvent;

        /// <summary>
        /// 发送邮件失败时事件
        /// </summary>
        /// <param name="ex">catch到的Exception</param>
        public delegate void SendMailFAailed(Exception ex);
        public static event SendMailFAailed SendMailFAailedEvent;

        /// <summary>
        /// 收到单条邮件时发生事件
        /// </summary>
        public delegate void GetOneMail();
        public event GetOneMail GetOneMailEvent;


        public HMail(UserInfo Info)
        {
            if (Info == null)
                throw new ArgumentNullException("Info");
            if (string.IsNullOrEmpty(Info.UserName) || string.IsNullOrEmpty(Info.Password))
                throw new ArgumentException("Info.UserName,Info.Password不能为空");
            userInfo = Info;
            smtpClient.Host = Info.SmtpAddress;
            smtpClient.Port = Info.SmtpPort;
            smtpClient.Credentials = new System.Net.NetworkCredential(Info.UserName, Info.Password);
            AccessPop();
        }

        //连接popserver并登陆
        private void AccessPop()
        {
            try
            {
                mailClient.Connect(userInfo.PopAddress, userInfo.PopPort);
                ns = mailClient.GetStream();
                popStreamWriter = new StreamWriter(ns);
                popStreamReader = new StreamReader(ns);
                popStreamWriter.WriteLine("user " + userInfo.UserName);
                popStreamWriter.Flush();
                if (popStreamReader.ReadLine().StartsWith("-ERR"))
                    return;
                popStreamWriter.WriteLine("pass " + userInfo.Password);
                popStreamWriter.Flush();
                if (popStreamReader.ReadLine().StartsWith("-ERR"))
                    return;
                string paaStr = popStreamReader.ReadLine();
                if (!string.IsNullOrEmpty(paaStr))
                    mailCount = int.Parse(paaStr.Split(' ')[1]);
                popAccessed = true;
            }
            catch (Exception ex)
            {
                AccessPopFailedEvent(ex);
                popAccessed = false;
            }
        }

        //發送郵件
        public static bool SendHMail(HMailInfo hMailInfo)
        {
            MailMessage msg = new MailMessage();
            msg.From = hMailInfo.From;
            foreach (MailAddress mail in hMailInfo.To)
                msg.To.Add(mail);
            foreach (MailAddress mail in hMailInfo.Cc)
                msg.CC.Add(mail);
            foreach (MailAddress mail in hMailInfo.Bcc)
                msg.Bcc.Add(mail);
            foreach (HMailAttachmentInfo mail in hMailInfo.Attachments)
                msg.Attachments.Add(Attachment.CreateAttachmentFromString(Encoding.GetEncoding(mail.Encoding).GetString(mail.FileBuffer), mail.ContentType));
            StringBuilder strBud = new StringBuilder();
            foreach (HMailContentInfo mail in hMailInfo.ContentInfo)
                strBud.Append(mail.Content);
            msg.Body = strBud.ToString();
            msg.IsBodyHtml = true;
            msg.BodyEncoding = msg.SubjectEncoding = Encoding.UTF8;
            try
            {
                smtpClient.Send(msg);
                return true;
            }
            catch (Exception ex)
            {
                SendMailFAailedEvent(ex);
                return false;
            }
        }

        /// <summary>
        /// 返回获取到的邮件条目
        /// </summary>
        /// <param name="start">起始号</param>
        /// <param name="end">结束号</param>
        /// <returns>编号,标题,发件人,收件人</returns>
        public IList<HMailInfo> GetMailInfo(int start, int end, ShortOrWhole type)
        {
            if (PopAccessed)
            {

                if (mailCount > 0)
                {
                    //for (int i = mailCount - start; i >= mailCount - end; i--)
                    //for (int i = 26; i >= mailCount - end; i--)
                    for (int i = start; i <= end; i++)
                    {
                        //i = 183;
                        try
                        {
                            currentMailId = i;
                            if (i == mailCount || i == 0)
                            {
                                // GetMailInfoById(i);
                                continue;
                            }
                            listMsg.Add(GetMailInfoById(i, type));
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(string.Format("{0}{1}{2}", ex.ToString(), "\r\n error message id:", i));
                        }
                    }
                }

            }

            return listMsg;
        }

        //執行命令,retr|top|stat|dele等
        public string ExcuteMailCommand(string command, Encoding encoding)
        {
            popStreamWriter.WriteLine(command);
            popStreamWriter.Flush();
            char[] buffer = new char[120000];
            Thread.Sleep(1000);
            popStreamReader = new StreamReader(ns, encoding);
            int readint = popStreamReader.Read(buffer, 0, buffer.Length);
            return encoding.GetString(encoding.GetBytes(buffer));
        }

        //获得单条邮件
        public HMailInfo GetMailInfoById(int id, ShortOrWhole type)
        {
            try
            {
                //for test
                id = 2;

                HMailInfo mailInfo = new HMailInfo();
                string cmd = "top " + id.ToString() + " 50";
                if (type == ShortOrWhole.Whole)
                    cmd = "retr " + id.ToString();
                result = ExcuteMailCommand(cmd, Encoding.UTF8);
                mailEncoding = "UTF-8";

                //if (!(result.Contains("charset=\"UTF-8\"") || result.Contains("charset=\"utf-8\"") || result.Contains("charset=utf-8") || result.Contains("charset=UTF-8")))
                    //GetMailEncoding(cmd);
                mailInfo.MailId = id;
                string _partten = "((?=References:)|(?=MIME-Version:)|(?=Mime-version:)|(?=Mime-Version:)|(?=Message-ID:)|(?=Message-Id:)|(?=Date:)|(?=date:)|(?=Content-Type:)|(?=content-type:)|(?=--)|(?=To:)|(?=to:)|(?=TO:)|(?=Subject:)|(?=subject:)|(?=SUBJECT:)|(?=Cc:)|(?=CC:)|(?=cc:)|(?=From:)|(?=from:)|(?=FROM:)|(?=X-Originating-IP:)|(?=X-Coremail-Antispam:)|(?=Sender:)|(?=Reply-To:)|(?=Reply-to)|(?=X-Mailer:)|(?=X-Priority:)|(?=\r\n\r\n))";
                mailInfo.To = GetAddress(MatchAndDecodeMail(result, string.Format(@"((?<=\r\nTo:)|(?<=\r\nTO:)|(?<=\r\nto:))[\s\S]*?{0}", _partten)));
                mailInfo.From = GetAddress(MatchAndDecodeMail(result, string.Format(@"((?<=\r\nFrom:)|(?<=\r\nfrom:)|(?<=\r\nFROM:))[\s\S]*?{0}", _partten)))[0];

                mailInfo.Cc = GetAddress(MatchAndDecodeMail(result, string.Format(@"((?<=\r\nCc:)|(?<=\r\nCC:)|(?<=\r\ncc:))[\s\S]*?{0}", _partten)));
                mailInfo.SubJect = MatchAndDecodeMail(result, string.Format(@"((?<=\r\nSubject:)|(?<=\r\nsubject:)|(?<=\r\nSUBJECT:))[\s\S]*?{0}", _partten));
                mailInfo.Date = MatchAndDecodeMail(result, string.Format(@"((?<=\r\nDate:)|(?<=\r\ndate:))[\s\S]*?{0}", _partten));
                if (type == ShortOrWhole.Whole)
                {
                    IList<HMailAttachmentInfo> mailMentInfo = new List<HMailAttachmentInfo>();
                    mailInfo.ContentInfo = GetMailContent(result, ref mailMentInfo);
                    mailInfo.Attachments = mailMentInfo;
                }
                GetOneMailEvent();
                return mailInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private IList<MailAddress> GetAddress(string mailString)
        {
            if (string.IsNullOrEmpty(mailString) || mailString == " ")
                return null;
            mailString = mailString.Trim();
            IList<MailAddress> mailAddressList = new List<MailAddress>();
            char splitChar = mailString.Contains(";") ? ';' : ',';
            if (mailString.Split(splitChar).Length > 1)
            {
                foreach (string str in mailString.Split(splitChar))
                {
                    if (string.IsNullOrWhiteSpace(str))
                        continue;
                    if (str.Contains("<"))
                        mailAddressList.Add(new MailAddress(str.Split('<')[1].Replace(">", "").Replace("\"", ""), str.Split('<')[0]));
                    else
                        mailAddressList.Add(new MailAddress(str.Replace("\"", "")));
                }
            }
            else
            {
                if (mailString.Contains("<"))
                {
                    string ss = mailString.Split('<')[0];
                    string sss = mailString.Split('<')[1].Replace(">", "");
                    mailAddressList.Add(new MailAddress(mailString.Split('<')[1].Replace(">", "").Replace("\"", ""), mailString.Split('<')[0]));
                }
                else
                    mailAddressList.Add(new MailAddress(mailString.Replace("\"", "")));
            }
            return mailAddressList;
        }

        private string MatchAndDecodeMail(string input, string partten)
        {
            return Regex.Match(input, partten).Success ? DecodingMailString(Regex.Match(input, partten).Value).Trim() : "";
        }

        //解析邮件
        public string DecodingMailString(string mailString)
        {
            try
            {
                string retString = mailString.Replace("\r\n", "");
                StringBuilder strBud = new StringBuilder();
                foreach (string str in retString.Split(','))
                {
                    if (string.IsNullOrEmpty(str))
                        continue;

                    if (str.Contains("?Q?") || str.Contains("?q?"))
                    {
                        reg = new Regex(@"(?<==\?)[\s\S]*?((?=\?q\?)|(?=\?Q\?))");
                        strBud.Append(string.Format("{0},", DecodeQP(Regex.Replace(str, @"=\?.*?\?[qQ]\?", "").Replace("=?", "").Replace("?=", "").Replace("\"", "").Trim(), reg.Match(str).Value.Contains("8859-1") ? "ISO-8859-1" : reg.Match(str).Value)));
                    }

                    else if (str.Contains("?B?") || str.Contains("?b?"))
                        strBud.Append(string.Format("{0},", _DecodingMailString(str)));
                    else
                        strBud.Append(string.Format("{0},", str));
                }
                return strBud.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string _DecodingMailString(string mailString)
        {
            StringBuilder strBud = new StringBuilder();
            reg = new Regex(@"(?<==\?)[\s\S]*?((?=\?b\?)|(?=\?B\?))");

            if (string.IsNullOrEmpty(mailString) || mailString == " ")
                return "";
            string retString = mailString.Replace("=?", "").Replace("?=", "").Replace("\"", "").Replace("\t", "").Trim();
            string mailAddressTemp = string.Empty;

            if (retString.Contains("<"))
            {
                mailAddressTemp = retString.Substring(retString.IndexOf("<"));
                retString = retString.Replace(mailAddressTemp, "");
            }

            string getCharset = reg.Match(mailString).Value.Contains("=") ? reg.Match(mailString).Value.Trim('=').Split('=')[1].Replace("\"", "") : reg.Match(mailString).Value;
            if (getCharset.Contains("8859-1"))
                getCharset = "ISO-8859-1";
            retString = Encoding.GetEncoding(getCharset).GetString(Convert.FromBase64String(retString.Replace(getCharset, "").Replace("?b?", "").Replace("?B?", "")));

            strBud.Append(retString);
            strBud.Append(" ");
            strBud.Append(mailAddressTemp);

            return strBud.ToString();
        }

        //分析邮件编码,統一編碼
        private void GetMailEncoding(string comm)
        {

            if (result.Contains("charset=\"gb2312\"") || result.Contains("charset=\"GB2312\"") || result.Contains("charset=gb2312") || result.Contains("charset=GB2312"))
                mailEncoding = "GB2312";
            if (result.Contains("charset=\"gbk\"") || result.Contains("charset=\"GBK\"") || result.Contains("charset=gbk") || result.Contains("charset=GBK"))
                mailEncoding = "GBK";
            if (result.Contains("charset=\"BIG5\"") || result.Contains("charset=\"big5\"") || result.Contains("charset=\"Big5\"") || result.Contains("charset=BIG5") || result.Contains("charset=big5") || result.Contains("charset=Big5"))
                mailEncoding = "BIG5";
            if (result.Contains("8859-1"))
                mailEncoding = "ISO-8859-1";

            result = ExcuteMailCommand(comm, Encoding.GetEncoding(mailEncoding));
        }

        //解析邮件正文,附件
        private IList<HMailContentInfo> GetMailContent(string mailString, ref IList<HMailAttachmentInfo> mentInfo)
        {
            IList<HMailContentInfo> mailContents = new List<HMailContentInfo>();
            string mixedMark = GetMailStringMark(mailString, "Content-Type: multipart/mixed;");
            string matchString = string.Empty;
            string[] strArr = null;
            if (!string.IsNullOrEmpty(mixedMark))
            {
                matchString = Regex.Match(mailString, string.Format("(?<=\r\n{0}[^\"])[\\s\\S]*(?=[^\"]{0}{1}{1})", mixedMark, "-")).Value;
                string[] matchStrArr = Regex.Split(matchString, mixedMark);

                foreach (string str in matchStrArr)
                {
                    matchString = Regex.Match(str, "[\\s\\S]*(?=\n)").Value;
                    mixedMark = GetMailStringMark(matchString, "Content-Type: multipart/alternative;");
                    if (string.IsNullOrEmpty(mixedMark))
                        mixedMark = GetMailStringMark(matchString, "");
                    if (!string.IsNullOrEmpty(mixedMark))
                    {
                        strArr = Regex.Split(matchString, mixedMark);
                        HMailContentInfo tmpContent = null;
                        foreach (string tmpStr in strArr)
                        {
                            tmpContent = DecodeContent(tmpStr);
                            if (string.IsNullOrEmpty(tmpContent.ContentType) || string.IsNullOrEmpty(tmpContent.Encoding))
                                continue;
                            mailContents.Add(DecodeContent(tmpStr));
                        }
                    }
                    else if (matchString.Contains("Content-Disposition: attachment;"))
                        mentInfo.Add(GetAttachment(matchString));
                    else
                        mailContents.Add(DecodeContent(matchString));
                }
            }
            else
            {
                RecursiveContent(mailContents, mentInfo, mailString);
            }
            return mailContents;
        }

        private void RecursiveContent(IList<HMailContentInfo> mailContents, IList<HMailAttachmentInfo> mentInfo, string mailString)
        {
            string matchString = string.Empty;
            string mixedMark = string.Empty;
            string[] strArr = null;
            matchString = Regex.Match(mailString, "[\\s\\S]*(?=\n)").Value;
            mixedMark = GetMailStringMark(matchString, "Content-Type: multipart/alternative;");
            if (string.IsNullOrEmpty(mixedMark))
                mixedMark = GetMailStringMark(matchString, "");
            if (!string.IsNullOrEmpty(mixedMark))
            {
                strArr = Regex.Split(matchString, mixedMark);
                HMailContentInfo tmpContent = null;
                foreach (string tmpStr in strArr)
                {
                    tmpContent = DecodeContent(tmpStr);
                    if (string.IsNullOrEmpty(tmpContent.ContentType) || string.IsNullOrEmpty(tmpContent.Encoding))
                        continue;
                    mixedMark = GetMailStringMark(tmpStr, "Content-Type: multipart/alternative;");
                    if (string.IsNullOrEmpty(mixedMark))
                        mixedMark = GetMailStringMark(tmpStr, "");
                    if (!string.IsNullOrEmpty(mixedMark))
                        RecursiveContent(mailContents, mentInfo, tmpStr);
                    else
                        mailContents.Add(DecodeContent(tmpStr));
                }
            }
            else if (matchString.Contains("Content-Disposition: attachment;"))
                mentInfo.Add(GetAttachment(matchString));
            else
            {
                mailContents.Add(DecodeContent(matchString));
            }
        }

        private HMailContentInfo DecodeContent(string mailString)
        {
            HMailContentInfo mailContentInfo = new HMailContentInfo();
            try
            {
                if (mailString.Contains("Content-Type: Message/Rfc822"))
                {
                    mailContentInfo.Content = "";
                    return mailContentInfo;
                }
                mailContentInfo.ContentType = Regex.Match(mailString, "(?<=Content-Type:)[\\s\\S]*?(?=\r\n)").Value.Trim().Trim(new char[] { ';' }).ToLower();
                mailContentInfo.Charset = Regex.Match(mailString, "(?<=charset)[\\s\\S]*?(?=\r\n)").Value.Replace('=', ' ').Trim().Trim(new char[] { '\"' }).ToLower();
                if (mailContentInfo.Charset.Contains("charset"))
                    mailContentInfo.Charset = Regex.Split(mailContentInfo.Charset, "charset")[1].Trim().Trim(new char[] { '\"' }).ToLower();
                if (mailContentInfo.Charset.ToUpper() == "SO-8859-1")
                    mailContentInfo.Charset = "ISO-8859-1";
                mailContentInfo.Encoding = Regex.Match(mailString, "(?<=Content-Transfer-Encoding:)[\\s\\S]*?(?=\r\n)").Value.Trim().ToLower();
                if (!string.IsNullOrEmpty(mailContentInfo.ContentType) && !string.IsNullOrEmpty(mailContentInfo.Charset) && (mailContentInfo.ContentType.Contains("text/plain") || mailContentInfo.ContentType.Contains("text/html")) && (mailContentInfo.Encoding == "quoted-printable" || mailContentInfo.Encoding == "base64"))
                {
                    //string sssss = Regex.Replace(mailString, "[\\s\\S]*(?=<html>)", "");
                    if (mailContentInfo.Encoding == "quoted-printable")
                    {
                        mailString = Regex.Replace(mailString, "Content-Type:[\\s\\S]*?\r\n", "");
                        mailString = Regex.Replace(mailString, "charset=[\\s\\S]*?\r\n", "");
                        mailString = Regex.Replace(mailString, "Content-Transfer-Encoding:[\\s\\S]*?\r\n", "");
                        if (Regex.IsMatch(mailString, "[\\s\\S]+\r\n\r\n[\\s\\S]*"))
                            mailString = Regex.Match(mailString, "\r\n\r\n[\\s\\S]*").Value;
                        mailContentInfo.Content = DecodeQP(Regex.Replace(mailString, "[\\s\\S]*(?=<html>)", ""), mailContentInfo.Charset);
                    }
                    else
                    {
                        string _partten = "((?=X-CM-TRANSID)|(?=X-Coremail-Antispam)|(?=Message-Id)|(?=Date))";
                        string _tmpString = Regex.Match(mailString, "(?<=base64)[\\s\\S]*(?=\n)").Value;
                        if (_tmpString.Contains("X-CM-TRANSID"))
                            _tmpString = Regex.Replace(_tmpString, string.Format("X-CM-TRANSID[\\s\\S]*{0}", _partten), "");
                        if (_tmpString.Contains("X-Coremail-Antispam"))
                            _tmpString = Regex.Replace(_tmpString, string.Format("X-Coremail-Antispam[\\s\\S]*{0}", _partten), "");
                        if (_tmpString.Contains("Message-Id") || _tmpString.Contains("Message-ID"))
                            _tmpString = Regex.Replace(_tmpString, string.Format("((Message-Id)|(Message-ID))[\\s\\S]*{0}", _partten), "");
                        if (_tmpString.Contains("Date") || _tmpString.Contains("date"))
                            _tmpString = Regex.Replace(_tmpString, "((Date)|(date))[\\s\\S]*?\r\n", "");
                        if (_tmpString.Contains("X-CM-SenderInfo"))
                            _tmpString = Regex.Replace(_tmpString, "X-CM-SenderInfo[\\s\\S]*?\r\n", "");
                        if (_tmpString.Contains("Content-Description"))
                            _tmpString = Regex.Replace(_tmpString, "Content-Description[\\s\\S]*?\r\n", "");
                        if (_tmpString.Contains("X-Mailer"))
                            _tmpString = Regex.Replace(_tmpString, "X-Mailer[\\s\\S]*?\r\n", "");
                        if (_tmpString.Contains("X-IM-id"))
                            _tmpString = Regex.Replace(_tmpString, "X-IM-id[\\s\\S]*?\r\n", "");

                        if (Regex.IsMatch(_tmpString, "[\\s\\S]*\r\n\r\n[\\s\\S]*"))
                            _tmpString = Regex.Match(_tmpString, "\r\n\r\n[\\s\\S]*").Value;

                        //if (Regex.IsMatch(_tmpString, "[\\s\\S]+\r\n\r\n[\\s\\S]*"))
                        //_tmpString = Regex.Match(_tmpString, "\r\n\r\n[\\s\\S]*").Value;
                        mailContentInfo.Content = Encoding.GetEncoding(mailContentInfo.Charset).GetString(Convert.FromBase64String(_tmpString));
                    }
                    //mailContentInfo.Content = mailContentInfo.Encoding == "quoted-printable" ? DecodeQP(Regex.Replace(mailString, "[\\s\\S]*(?=<html>)", ""), mailContentInfo.Charset) : Encoding.GetEncoding(mailContentInfo.Charset).GetString(Convert.FromBase64String(Regex.Match(mailString, "(?<=base64)[\\s\\S]*(?=\n)").Value));
                }
                else
                {
                    mailString = Regex.Replace(mailString, "Content-Type[\\s\\S]*?\r\n", "");
                    mailString = Regex.Replace(mailString, "Content-Transfer-Encoding[\\s\\S]*?\r\n", "");
                    if (Regex.IsMatch(mailString, "[\\s\\S]+\r\n\r\n[\\s\\S]*"))
                        mailString = Regex.Match(mailString, "(?<=\r\n\r\n)[\\s\\S]*").Value;
                    mailContentInfo.Content = Regex.Match(mailString, "[\\s\\S]*(?=\n)").Value;
                }
                //if (mailContentInfo.Content.Contains("<body") && mailContentInfo.Content.Contains("</body>"))
                // mailContentInfo.Content = Regex.Match(mailContentInfo.Content, "(?<=<body [\\s\\S]*?>)[\\s\\S]*(?=</body>)").Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return mailContentInfo;
        }

        private HMailAttachmentInfo GetAttachment(string mailString)
        {
            HMailAttachmentInfo attachInfo = new HMailAttachmentInfo();
            attachInfo.FileName = Regex.Match(mailString, "(?<=filename=\")[\\s\\S]*(?=\"\r\n)").Value.Trim();
            attachInfo.Encoding = Regex.Match(mailString, "(?<=Content-Transfer-Encoding:)[\\s\\S]*(?=\r\n)").Value.Trim();
            if (!string.IsNullOrEmpty(attachInfo.FileName) && !string.IsNullOrEmpty(attachInfo.Encoding))
                attachInfo.FileBuffer = attachInfo.Encoding == "base64" ? Convert.FromBase64String(Regex.Match(mailString, string.Format("(?<={0}{1})[\\s\\S]*(?=\n)", attachInfo.FileName, "\"")).Value) : Encoding.UTF8.GetBytes(DecodeQP(Regex.Match(mailString, string.Format("(?<={0}{1})[\\s\\S]*(?=\n)", attachInfo.FileName, "\"")).Value, attachInfo.Encoding));

            return attachInfo;
        }

        private string GetMailStringMark(string mailString, string contentType)
        {
            string retString = string.Empty;
            if (mailString.Contains(contentType))
                retString = Regex.Match(mailString, "(?<=" + contentType + "[\t\\s\\S]{0,4}boundary=).*(?=\r\n)").Value.Replace("-", "");
            if (contentType == "")
                retString = Regex.Match(mailString, "(?<=boundary=).*(?=\r\n)").Value;
            return retString.Trim(new char[] { '"' });
        }

        //Quoted-Printable 解码
        public static string DecodeQP(string codeString, string mailEncoding)
        {
            codeString = Regex.Replace(codeString, "=3D", "=").Replace("=\r\n", "");
            StringBuilder strBud = new StringBuilder();
            int i = 0;
            try
            {
                for (i = 0; i < codeString.Length; i++)
                {
                    if (codeString[i] == '=')
                    {
                        if (Convert.ToInt32((codeString[i + 1] + codeString[i + 2]).ToString(), 16) < 127)
                        {
                            strBud.Append(Encoding.GetEncoding(mailEncoding).GetString(new byte[] { Convert.ToByte((codeString[i + 1] + codeString[i + 2]).ToString(), 16) }));
                            i += 2;
                            continue;
                        }

                        if (codeString[i + 3] == '=')
                        {
                            strBud.Append(Encoding.GetEncoding(mailEncoding).GetString(new byte[] { Convert.ToByte((codeString[i + 1].ToString() + codeString[i + 2].ToString()), 16), Convert.ToByte((codeString[i + 4].ToString() + codeString[i + 5].ToString()), 16) }));
                            i += 5;
                            continue;
                        }

                    }
                    else
                    {
                        strBud.Append(codeString[i]);
                    }
                }

                return strBud.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 读取邮件的长度，简短|整条
        /// </summary>
        public enum ShortOrWhole
        {
            Short,
            Whole
        }

        ~HMail()
        {
            Dispose(false);
        }

        private bool _alReadyDisposed = false;

        protected virtual void Dispose(bool isDisposing)
        {
            if (_alReadyDisposed)
                return;
            if (isDisposing)
            { }
            popStreamReader.Close();
            popStreamWriter.Close();
            ns.Close();
            mailClient.Close();
            _alReadyDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
    }

}
