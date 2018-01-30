namespace CurApp
{
    partial class FormRegistartion
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRegistartion));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textLogin = new System.Windows.Forms.TextBox();
            this.textPwd = new System.Windows.Forms.TextBox();
            this.textPwdRepeat = new System.Windows.Forms.TextBox();
            this.buttonReg = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textName = new System.Windows.Forms.TextBox();
            this.textSurname = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textGroup = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(66, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Логин";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(59, 123);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Пароль";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 149);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Повторить пароль";
            // 
            // textLogin
            // 
            this.textLogin.Location = new System.Drawing.Point(110, 15);
            this.textLogin.Name = "textLogin";
            this.textLogin.Size = new System.Drawing.Size(190, 20);
            this.textLogin.TabIndex = 1;
            this.textLogin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textLogin_KeyPress);
            // 
            // textPwd
            // 
            this.textPwd.Location = new System.Drawing.Point(110, 120);
            this.textPwd.Name = "textPwd";
            this.textPwd.PasswordChar = '●';
            this.textPwd.Size = new System.Drawing.Size(190, 20);
            this.textPwd.TabIndex = 5;
            this.textPwd.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textPwd_KeyPress);
            // 
            // textPwdRepeat
            // 
            this.textPwdRepeat.Location = new System.Drawing.Point(110, 146);
            this.textPwdRepeat.Name = "textPwdRepeat";
            this.textPwdRepeat.PasswordChar = '●';
            this.textPwdRepeat.Size = new System.Drawing.Size(190, 20);
            this.textPwdRepeat.TabIndex = 6;
            this.textPwdRepeat.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textPwdRepeat_KeyPress);
            // 
            // buttonReg
            // 
            this.buttonReg.Location = new System.Drawing.Point(110, 177);
            this.buttonReg.Name = "buttonReg";
            this.buttonReg.Size = new System.Drawing.Size(190, 23);
            this.buttonReg.TabIndex = 7;
            this.buttonReg.Text = "Зарегистрироваться";
            this.buttonReg.UseVisualStyleBackColor = true;
            this.buttonReg.Click += new System.EventHandler(this.button1_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(75, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Имя";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(48, 71);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Фамилия";
            // 
            // textName
            // 
            this.textName.Location = new System.Drawing.Point(110, 41);
            this.textName.Name = "textName";
            this.textName.Size = new System.Drawing.Size(190, 20);
            this.textName.TabIndex = 2;
            // 
            // textSurname
            // 
            this.textSurname.Location = new System.Drawing.Point(110, 68);
            this.textSurname.Name = "textSurname";
            this.textSurname.Size = new System.Drawing.Size(190, 20);
            this.textSurname.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(59, 97);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Группа";
            // 
            // textGroup
            // 
            this.textGroup.Location = new System.Drawing.Point(110, 94);
            this.textGroup.Name = "textGroup";
            this.textGroup.Size = new System.Drawing.Size(190, 20);
            this.textGroup.TabIndex = 4;
            // 
            // FormRegistartion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(315, 212);
            this.Controls.Add(this.textGroup);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textSurname);
            this.Controls.Add(this.textName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.buttonReg);
            this.Controls.Add(this.textPwdRepeat);
            this.Controls.Add(this.textPwd);
            this.Controls.Add(this.textLogin);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormRegistartion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Регистрация";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormRegistartion_FormClosed);
            this.Load += new System.EventHandler(this.FormRegistartion_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textLogin;
        private System.Windows.Forms.TextBox textPwd;
        private System.Windows.Forms.TextBox textPwdRepeat;
        private System.Windows.Forms.Button buttonReg;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textName;
        private System.Windows.Forms.TextBox textSurname;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textGroup;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}