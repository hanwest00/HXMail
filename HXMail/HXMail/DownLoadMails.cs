using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HXMail.Model;
using HXMail.Common;
using HXMail.BLL;
using HXMail.IBLL;
using HMail;
using HMail.MailEntity;
using System.Threading;

namespace HXMail
{
    public partial class DownLoadMails : FormBase
    {
        private Thread thread;
        private IList<HMailInfo> allMail;
        private HMail.HMail hmail;
        private IMailManage MailMng;
        private int currMailCount;
        //for test
        private Label TestLaber;

        public DownLoadMails()
        {
            InitializeComponent();
            base._SetLanguage(this);

            #region control setting
            //todo: 
            this.button1_Pause.Hide();
            this.button2_Cancel.Hide();

            this.progressBar1.Style = ProgressBarStyle.Continuous;

            //for test
            TestLaber = new Label();
            TestLaber.Text = "0";
            TestLaber.Name = "TestLaber";
            TestLaber.Location = new Point(20,0);
            this.Controls.Add(TestLaber);
            //test end

            #endregion
            MailMng = new MailManage();
            currMailCount = MailMng.GetUserMailCount((int)AppMain.currUser.Id);
            hmail = new HMail.HMail(HMConvert.ConvertMailEntity.ConvertToMailEntity(AppMain.currUser));
            if (hmail.MailCount <= currMailCount)
                this.Close();
            this.progressBar1.Maximum = hmail.MailCount - currMailCount;
        }

        private void DownLoadMails_Load(object sender, EventArgs e)
        {
            int count = 0;
            hmail.GetOneMailEvent += new HMail.HMail.GetOneMail(() =>
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    TestLaber.Text = count.ToString();
                    this.progressBar1.Increment(1);
                    count++;
                }));
            });

            thread = new Thread(new ThreadStart(() =>
            {
                StartDown();
            }));

            thread.Start();
        }

        private void StartDown()
        {
            
            if (hmail.PopAccessed)
            {
                try
                {
                    allMail = hmail.GetMailInfo(currMailCount + 1, hmail.MailCount, HMail.HMail.ShortOrWhole.Whole);
                    this.Invoke(new MethodInvoker(() => {
                        this.label1_StartDown.Text = App_Code.LanguageSelect.getResc("StartSave");
                        this.progressBar1.Step = 1;
                        this.progressBar1.PerformStep();
                    }));
                    
                    SaveToDB();
                }
                catch (Exception e)
                {
                    //AppLog.CreateLog("c:\\mailerr.txt", hmail.CurrentMailString);
                    AppLog.SysLog(e.ToString());
                }
            }

            this.Invoke(new MethodInvoker(() =>
                {
                    this.Close();
                }));
        }

        private void SaveToDB()
        {
            if (allMail.Count <= 0)
                return;
            try
            {
                IContentManage MailContentMng = new ContentManage();
                IAttachmentManage MailAttachMng = new AttachmentManage();
                int mailId;
                int mailCount = hmail.MailCount - currMailCount;
                allMail.ToList().ForEach(s =>
                {
                    mailId = MailMng.CreateMail(HMConvert.ConvertMailEntity.ConvertToDbMailEntity((int)AppMain.currUser.Id, s));
                    HMConvert.ConvertMailEntity.ConvertToDbContentEntity(mailId, s).ToList().ForEach(v =>
                    {
                        MailContentMng.CreateContent(v);
                    });
                    HMConvert.ConvertMailEntity.ConvertToDbAttachEntity(mailId, s).ToList().ForEach(n =>
                    {
                        MailAttachMng.CreateAttactment(n);
                    });
                    this.Invoke(new MethodInvoker(() =>
                    {
                        this.progressBar1.Value--;
                        TestLaber.Text = mailCount.ToString();
                        mailCount--;
                    }));
                });

            }
            catch (Exception e)
            {
                AppLog.SysLog(e.ToString());
            }
        }

        private void DownLoadMails_Close(object sender, EventArgs e)
        {
            //todo:
        }

        private void button1_Pause_Click(object sender, EventArgs e)
        {
            //todo:
        }

        private void button2_Cancel_Click(object sender, EventArgs e)
        {
            //todo:
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            thread.Abort();
        }

    }
}
