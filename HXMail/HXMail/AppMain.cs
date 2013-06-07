using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using HXMail.Common;
using HXMail.BLL;
using HXMail.IBLL;
using HXMail.Model;

namespace HXMail
{
    public partial class AppMain : FormBase
    {
        private IUserManage userMng = new UserManage();
        private IMailManage mailMng = new MailManage();
        private IList<UserInfo> allUser;
        private IList<MailInfo> allMail;
        private System.Threading.Thread thread1;

        public static UserInfo currUser;

        public AppMain()
        {
            InitializeComponent();
            webBrowser1.Navigate(AppDomain.CurrentDomain.BaseDirectory + "kindeditor\\examples\\simple.html");
            webBrowser1.SizeChanged += new EventHandler(webBrowser1_SizeChanged);
            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser1_DocumentCompleted);
        }

        void ClipScreen_ClipCompleteEvent(Image retImage)
        {
            retImage.Save("c:\\hhh.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        void webBrowser1_SizeChanged(object sender, EventArgs e)
        {
            webBrowser1.Document.InvokeScript("removeEditor");
            webBrowser1.Document.Body.Style = string.Format("height:{1};width:{0}", webBrowser1.Width, webBrowser1.Height);
            webBrowser1.Document.GetElementById("content").Style = string.Format("height:{1};width:{0}", webBrowser1.Width, webBrowser1.Height + 10);
            webBrowser1.Document.InvokeScript("showEditor");
        }

        void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            webBrowser1.Document.Body.Style = string.Format("height:{1};width:{0}", webBrowser1.Width, webBrowser1.Height);
            webBrowser1.Document.GetElementById("content").Style = string.Format("height:{1};width:{0}", webBrowser1.Width, webBrowser1.Height + 10);
            webBrowser1.Document.InvokeScript("showEditor");

            // MessageBox.Show(webBrowser1.Document.GetElementById("content").Style);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            base._SetLanguage(this);
            HXMail.App_Code.HotKey.RegisterHotKey(this.Handle, 100100, App_Code.HotKey.KeyModifiers.Ctrl, Keys.X);
            this.listBox1.ItemHeight = 30;
            this.listBox1.Font = new Font(new FontFamily("宋体"), 10);
            IContentManage contentMng = new ContentManage();
            StringBuilder sb = new StringBuilder();
            this.listBox1.Click += new EventHandler((object obj, EventArgs ev) =>
            {
                IList<ContentInfo> all = contentMng.GetByMailId((int)allMail[this.listBox1.SelectedIndex].Id);
                foreach (ContentInfo v in all)
                {
                    sb.Append(v.Content);
                }

                try
                {
                    webBrowser1.Document.InvokeScript("removeEditor");
                    webBrowser1.Document.Body.Style = string.Format("height:{1};width:{0}", webBrowser1.Width, webBrowser1.Height);
                    webBrowser1.Document.GetElementById("content").Style = string.Format("height:{1};width:{0}", webBrowser1.Width, webBrowser1.Height);
                    webBrowser1.Document.GetElementById("content").SetAttribute("value", sb.ToString());
                    webBrowser1.Document.InvokeScript("showEditor");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                sb.Clear();
            });
            this.listView1.DoubleClick += new EventHandler((object obj, EventArgs ex) =>
            {
                currUser = allUser[((ListView)obj).SelectedItems[0].Index];

                this.listBox1.Items.Clear();
                thread1 = new System.Threading.Thread(new System.Threading.ThreadStart(() =>
                {
                    LoadMail();
                }));

                thread1.Start();
            });
            LoadUser();
        }

        private void LoadUser()
        {
            allUser = userMng.GetUserList();
            foreach (UserInfo user in allUser)
            {
                this.listView1.Items.Add(string.Format("{0} ({1})", user.UserName, user.EmailAddress));
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_HOTKEY = 0x0312;
            switch (m.Msg)
            {
                case WM_HOTKEY:
                    switch (m.WParam.ToInt32())
                    {
                        case 100100:
                            HXMail.App_Code.ClipScreen.DoClipScreen();
                            HXMail.App_Code.ClipScreen.ClipCompleteEvent += new App_Code.ClipScreen.OnClipComplete(ClipScreen_ClipCompleteEvent);
                            break;
                    }
                    break;
            }
            base.WndProc(ref m);
        }

        private void CreateUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateUser form = new CreateUser();
            this.Enabled = false;
            form.FormClosed += new FormClosedEventHandler(delegate(object obj, FormClosedEventArgs ex)
            {
                this.Enabled = true;
                LoadUser();
            });
            form.Show();
            form.Activate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (currUser == null)
                currUser = allUser[0];
            DownLoadMails f = new DownLoadMails();
            f.FormClosed += new FormClosedEventHandler((object o, FormClosedEventArgs exc) =>
            {
                this.Enabled = true;
            });
            this.Enabled = false;
            f.Show();
            f.Activate();
        }

        private void LoadMail()
        {
            allMail = mailMng.LoadMails(0, 0, (int)currUser.Id).OrderByDescending(o => o.ReceiveDate).ToList();
            allMail.ToList().ForEach(v =>
            {
                this.Invoke(new MethodInvoker(() =>
                    {
                        this.listBox1.Items.Add(string.Format("{0} {1} {2}", v.FromAddress, v.Subject.Length > 14 ? string.Format("{0}...", v.Subject.Substring(0, 14)) : v.Subject, v.ReceiveDate));
                    }));
            });

        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (thread1 != null)
                thread1.Abort();
        }
    }
}
