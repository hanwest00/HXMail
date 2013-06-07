namespace HXMail
{
    partial class DownLoadMails
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label1_StartDown = new System.Windows.Forms.Label();
            this.button1_Pause = new System.Windows.Forms.Button();
            this.button2_Cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 37);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(336, 23);
            this.progressBar1.TabIndex = 0;
            // 
            // label1_StartDown
            // 
            this.label1_StartDown.AutoSize = true;
            this.label1_StartDown.Location = new System.Drawing.Point(12, 12);
            this.label1_StartDown.Name = "label1_StartDown";
            this.label1_StartDown.Size = new System.Drawing.Size(33, 12);
            this.label1_StartDown.TabIndex = 1;
            this.label1_StartDown.Text = "label1";
            // 
            // button1_Pause
            // 
            this.button1_Pause.Location = new System.Drawing.Point(192, 77);
            this.button1_Pause.Name = "button1_Pause";
            this.button1_Pause.Size = new System.Drawing.Size(75, 23);
            this.button1_Pause.TabIndex = 2;
            this.button1_Pause.Text = "button1_Pause";
            this.button1_Pause.UseVisualStyleBackColor = true;
            this.button1_Pause.Click += new System.EventHandler(this.button1_Pause_Click);
            // 
            // button2_Cancel
            // 
            this.button2_Cancel.Location = new System.Drawing.Point(273, 77);
            this.button2_Cancel.Name = "button2_Cancel";
            this.button2_Cancel.Size = new System.Drawing.Size(75, 23);
            this.button2_Cancel.TabIndex = 3;
            this.button2_Cancel.Text = "button2_Cancel";
            this.button2_Cancel.UseVisualStyleBackColor = true;
            this.button2_Cancel.Click += new System.EventHandler(this.button2_Cancel_Click);
            // 
            // DownLoadMails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(360, 110);
            this.Controls.Add(this.button2_Cancel);
            this.Controls.Add(this.button1_Pause);
            this.Controls.Add(this.label1_StartDown);
            this.Controls.Add(this.progressBar1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(368, 139);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(368, 139);
            this.Name = "DownLoadMails";
            this.Text = "DownLoadMails";
            this.Load += new System.EventHandler(this.DownLoadMails_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label1_StartDown;
        private System.Windows.Forms.Button button1_Pause;
        private System.Windows.Forms.Button button2_Cancel;
    }
}