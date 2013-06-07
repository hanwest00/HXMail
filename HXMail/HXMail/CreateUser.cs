using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using HXMail.IBLL;
using HXMail.BLL;
using HXMail.Model;
using HXMail.App_Code;
using System.Runtime.InteropServices;

namespace HXMail
{
    public partial class CreateUser : FormBase
    {
        public CreateUser()
        {
            InitializeComponent();
            base._SetLanguage(this);

            #region controller setting
            textBox1.ForeColor = Color.Gray;
            textBox1.Text = LanguageSelect.getResc("EnterUser");
            textBox1.GotFocus += new EventHandler((object obj, EventArgs e) =>
            {
                if (string.IsNullOrEmpty(textBox1.Text) || textBox1.Text == LanguageSelect.getResc("EnterUserError") || textBox1.Text == LanguageSelect.getResc("EnterUser"))
                {
                    textBox1.ForeColor = Color.Black;
                    textBox1.Text = "";
                }
            });
            textBox1.LostFocus += new EventHandler((object obj, EventArgs e) =>
            {
                if (!Regex.IsMatch(textBox1.Text, "^.*@.*.((.[a-zA-Z])|(.[a-zA-Z].[a-zA-Z]))$") && textBox1.Text != LanguageSelect.getResc("EnterUserError"))
                {
                    textBox1.ForeColor = Color.Red;
                    textBox1.Text = LanguageSelect.getResc("EnterUserError");
                }
            });

            textBox2.GotFocus += new EventHandler((object obj, EventArgs e) =>
            {
                textBox2.ForeColor = Color.Black;
                textBox2.PasswordChar = '❤';
                textBox2.Text = "";
            });
            textBox2.LostFocus += new EventHandler((object obj, EventArgs e) =>
            {
                if (string.IsNullOrEmpty(textBox2.Text))
                {
                    textBox2.ForeColor = Color.Red;
                    textBox2.UseSystemPasswordChar = false;
                    textBox2.Text = LanguageSelect.getResc("EmptyPassError");
                }
            });
            textBox7.Text = "110";
            textBox7.LostFocus += new EventHandler((object obj, EventArgs e) =>
            {
                if (!Regex.IsMatch(textBox7.Text, "^[0-9]{1,5}$"))
                    textBox7.Text = "";
            });

            textBox6.Text = "25";
            textBox6.LostFocus += new EventHandler((object obj, EventArgs e) =>
            {
                if (!Regex.IsMatch(textBox6.Text, "^[0-9]{1,5}$"))
                    textBox6.Text = "";
            });
            #endregion
        }

        private void button1_Create_Click(object sender, EventArgs e)
        {
            if (!ValiInput())
                return;
            UserInfo userInfo = new UserInfo();
            userInfo.EmailAddress = textBox1.Text;
            userInfo.UserName = textBox5.Text;
            userInfo.Password = textBox2.Text;
            userInfo.PopAddress = textBox4.Text;
            userInfo.SmtpAddress = textBox3.Text;
            userInfo.PopPort = int.Parse(textBox7.Text);
            userInfo.SmtpPort = int.Parse(textBox6.Text);
            userInfo.CreateDate = DateTime.Now;

            UserManage userMng = new UserManage();

            userMng.OnCreateUserErrorEvent += new UserManage.CreateUserErrorHandler((Exception ex) =>
            {
                Common.AppLog.SysLog(ex.ToString());
                MessageBox.Show(LanguageSelect.getResc("CreateUserError"));
            });

            userMng.OnCreatedUserEvent += new UserManage.CreatedUserHandler(() =>
            {
                Common.AppLog.SysLog(string.Format(LanguageSelect.getResc("SysUserCreate"),userInfo.EmailAddress));
            });

            switch (userMng.CreateUser(userInfo))
            {
                case 1: MessageBox.Show(LanguageSelect.getResc("CreateUserSuccess"));
                        AppMain.currUser = userInfo;
                        DownLoadMails f = new DownLoadMails();
                        f.Show();
                        f.Activate();
                        this.Close();
                        break;
                case 2: MessageBox.Show(LanguageSelect.getResc("RepeatUser"));
                        break;
            }
        }

        private bool ValiInput()
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox5.Text) || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox4.Text) || string.IsNullOrEmpty(textBox3.Text))
                return false;
            return true;
        }

    }
}
