using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HXMail.App_Code;

namespace HXMail
{
    public class FormBase : Form
    {
    
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected void SetLanguage()
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form.Controls.Count > 0)
                {
                    _SetLanguage(form);
                }
            }
        }

        protected void _SetLanguage(Control controls)
        {
            foreach (Control c in controls.Controls)
            {
                if (!c.Name.Contains('_'))
                    continue;
                switch (c.GetType().Name)
                {
                    case "TabControl":
                        TabControl tbc = c as TabControl;
                        for (int j = 0; j < tbc.TabCount; j++)
                        {
                            tbc.TabPages[j].Text = LanguageSelect.getResc(tbc.TabPages[j].Name.Split('_')[1]);
                            _SetLanguage(tbc.TabPages[j]);
                        };
                        break;
                    default:
                        if (c.Controls.Count > 0)
                            _SetLanguage(c);
                        c.Text = LanguageSelect.getResc(c.Name.Split('_')[1]);
                        break;
                }
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FormBase
            // 
            this.ClientSize = new System.Drawing.Size(292, 271);
            this.Name = "FormBase";
            this.ResumeLayout(false);

        }
    }
}
