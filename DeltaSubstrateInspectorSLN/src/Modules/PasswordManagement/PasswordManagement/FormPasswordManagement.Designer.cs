namespace PasswordManagement
{
    partial class Form_PWInput
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button_OK = new System.Windows.Forms.Button();
            this.label_PW = new System.Windows.Forms.Label();
            this.textBox_PW = new System.Windows.Forms.TextBox();
            this.label_Acc = new System.Windows.Forms.Label();
            this.comboBox_AccList = new System.Windows.Forms.ComboBox();
            this.button_Close = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage_Login = new System.Windows.Forms.TabPage();
            this.label_Level = new System.Windows.Forms.Label();
            this.textBox_Level = new System.Windows.Forms.TextBox();
            this.tabPage_PWManagement = new System.Windows.Forms.TabPage();
            this.label_ = new System.Windows.Forms.Label();
            this.comboBox_NewLevel = new System.Windows.Forms.ComboBox();
            this.button_Delete = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox_NowAllAcc = new System.Windows.Forms.ComboBox();
            this.button_Save = new System.Windows.Forms.Button();
            this.textBox_NewPW = new System.Windows.Forms.TextBox();
            this.textBox_NewAcc = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage_Login.SuspendLayout();
            this.tabPage_PWManagement.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_OK
            // 
            this.button_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_OK.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button_OK.Location = new System.Drawing.Point(18, 220);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(132, 40);
            this.button_OK.TabIndex = 6;
            this.button_OK.Text = "確定";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // label_PW
            // 
            this.label_PW.AutoSize = true;
            this.label_PW.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_PW.Location = new System.Drawing.Point(6, 64);
            this.label_PW.Name = "label_PW";
            this.label_PW.Size = new System.Drawing.Size(41, 20);
            this.label_PW.TabIndex = 4;
            this.label_PW.Text = "密碼";
            // 
            // textBox_PW
            // 
            this.textBox_PW.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox_PW.Location = new System.Drawing.Point(89, 61);
            this.textBox_PW.Name = "textBox_PW";
            this.textBox_PW.PasswordChar = '*';
            this.textBox_PW.Size = new System.Drawing.Size(201, 29);
            this.textBox_PW.TabIndex = 3;
            this.textBox_PW.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_PW_KeyDown);
            // 
            // label_Acc
            // 
            this.label_Acc.AutoSize = true;
            this.label_Acc.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_Acc.Location = new System.Drawing.Point(6, 19);
            this.label_Acc.Name = "label_Acc";
            this.label_Acc.Size = new System.Drawing.Size(41, 20);
            this.label_Acc.TabIndex = 7;
            this.label_Acc.Text = "帳號";
            // 
            // comboBox_AccList
            // 
            this.comboBox_AccList.AutoCompleteCustomSource.AddRange(new string[] {
            "Delta",
            "Engineer",
            "Op"});
            this.comboBox_AccList.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.comboBox_AccList.FormattingEnabled = true;
            this.comboBox_AccList.Location = new System.Drawing.Point(89, 16);
            this.comboBox_AccList.Name = "comboBox_AccList";
            this.comboBox_AccList.Size = new System.Drawing.Size(201, 28);
            this.comboBox_AccList.TabIndex = 8;
            this.comboBox_AccList.SelectedIndexChanged += new System.EventHandler(this.comboBox_AccList_SelectedIndexChanged);
            // 
            // button_Close
            // 
            this.button_Close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Close.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button_Close.Location = new System.Drawing.Point(158, 220);
            this.button_Close.Name = "button_Close";
            this.button_Close.Size = new System.Drawing.Size(132, 40);
            this.button_Close.TabIndex = 9;
            this.button_Close.Text = "關閉";
            this.button_Close.UseVisualStyleBackColor = true;
            this.button_Close.Click += new System.EventHandler(this.button_Close_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage_Login);
            this.tabControl1.Controls.Add(this.tabPage_PWManagement);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(322, 298);
            this.tabControl1.TabIndex = 10;
            // 
            // tabPage_Login
            // 
            this.tabPage_Login.Controls.Add(this.label_Level);
            this.tabPage_Login.Controls.Add(this.textBox_Level);
            this.tabPage_Login.Controls.Add(this.comboBox_AccList);
            this.tabPage_Login.Controls.Add(this.button_Close);
            this.tabPage_Login.Controls.Add(this.textBox_PW);
            this.tabPage_Login.Controls.Add(this.label_PW);
            this.tabPage_Login.Controls.Add(this.label_Acc);
            this.tabPage_Login.Controls.Add(this.button_OK);
            this.tabPage_Login.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Login.Name = "tabPage_Login";
            this.tabPage_Login.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Login.Size = new System.Drawing.Size(314, 272);
            this.tabPage_Login.TabIndex = 0;
            this.tabPage_Login.Text = "登入";
            this.tabPage_Login.UseVisualStyleBackColor = true;
            this.tabPage_Login.Click += new System.EventHandler(this.tabPage_Login_Click);
            // 
            // label_Level
            // 
            this.label_Level.AutoSize = true;
            this.label_Level.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_Level.Location = new System.Drawing.Point(6, 112);
            this.label_Level.Name = "label_Level";
            this.label_Level.Size = new System.Drawing.Size(41, 20);
            this.label_Level.TabIndex = 11;
            this.label_Level.Text = "權限";
            // 
            // textBox_Level
            // 
            this.textBox_Level.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox_Level.Location = new System.Drawing.Point(89, 109);
            this.textBox_Level.Name = "textBox_Level";
            this.textBox_Level.ReadOnly = true;
            this.textBox_Level.Size = new System.Drawing.Size(201, 29);
            this.textBox_Level.TabIndex = 10;
            // 
            // tabPage_PWManagement
            // 
            this.tabPage_PWManagement.Controls.Add(this.label_);
            this.tabPage_PWManagement.Controls.Add(this.comboBox_NewLevel);
            this.tabPage_PWManagement.Controls.Add(this.button_Delete);
            this.tabPage_PWManagement.Controls.Add(this.label4);
            this.tabPage_PWManagement.Controls.Add(this.comboBox_NowAllAcc);
            this.tabPage_PWManagement.Controls.Add(this.button_Save);
            this.tabPage_PWManagement.Controls.Add(this.textBox_NewPW);
            this.tabPage_PWManagement.Controls.Add(this.textBox_NewAcc);
            this.tabPage_PWManagement.Controls.Add(this.label2);
            this.tabPage_PWManagement.Controls.Add(this.label3);
            this.tabPage_PWManagement.Location = new System.Drawing.Point(4, 22);
            this.tabPage_PWManagement.Name = "tabPage_PWManagement";
            this.tabPage_PWManagement.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_PWManagement.Size = new System.Drawing.Size(314, 272);
            this.tabPage_PWManagement.TabIndex = 1;
            this.tabPage_PWManagement.Text = "管理";
            this.tabPage_PWManagement.UseVisualStyleBackColor = true;
            // 
            // label_
            // 
            this.label_.AutoSize = true;
            this.label_.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_.Location = new System.Drawing.Point(6, 159);
            this.label_.Name = "label_";
            this.label_.Size = new System.Drawing.Size(41, 20);
            this.label_.TabIndex = 18;
            this.label_.Text = "權限";
            // 
            // comboBox_NewLevel
            // 
            this.comboBox_NewLevel.AutoCompleteCustomSource.AddRange(new string[] {
            "Delta",
            "Engineer",
            "Op"});
            this.comboBox_NewLevel.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.comboBox_NewLevel.FormattingEnabled = true;
            this.comboBox_NewLevel.Location = new System.Drawing.Point(89, 156);
            this.comboBox_NewLevel.Name = "comboBox_NewLevel";
            this.comboBox_NewLevel.Size = new System.Drawing.Size(201, 28);
            this.comboBox_NewLevel.TabIndex = 17;
            this.comboBox_NewLevel.SelectedIndexChanged += new System.EventHandler(this.comboBox_NewLevel_SelectedIndexChanged);
            // 
            // button_Delete
            // 
            this.button_Delete.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_Delete.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button_Delete.Location = new System.Drawing.Point(158, 220);
            this.button_Delete.Name = "button_Delete";
            this.button_Delete.Size = new System.Drawing.Size(132, 40);
            this.button_Delete.TabIndex = 16;
            this.button_Delete.Text = "刪除";
            this.button_Delete.UseVisualStyleBackColor = true;
            this.button_Delete.Click += new System.EventHandler(this.button_Delete_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.Location = new System.Drawing.Point(6, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 20);
            this.label4.TabIndex = 15;
            this.label4.Text = "所有帳號";
            // 
            // comboBox_NowAllAcc
            // 
            this.comboBox_NowAllAcc.AutoCompleteCustomSource.AddRange(new string[] {
            "Delta",
            "Engineer",
            "Op"});
            this.comboBox_NowAllAcc.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.comboBox_NowAllAcc.FormattingEnabled = true;
            this.comboBox_NowAllAcc.Location = new System.Drawing.Point(89, 16);
            this.comboBox_NowAllAcc.Name = "comboBox_NowAllAcc";
            this.comboBox_NowAllAcc.Size = new System.Drawing.Size(201, 28);
            this.comboBox_NowAllAcc.TabIndex = 14;
            this.comboBox_NowAllAcc.SelectedIndexChanged += new System.EventHandler(this.comboBox_NowAllAcc_SelectedIndexChanged);
            // 
            // button_Save
            // 
            this.button_Save.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_Save.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button_Save.Location = new System.Drawing.Point(18, 220);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(132, 40);
            this.button_Save.TabIndex = 12;
            this.button_Save.Text = "更新 / 新增";
            this.button_Save.UseVisualStyleBackColor = true;
            this.button_Save.Click += new System.EventHandler(this.button_Save_Click);
            // 
            // textBox_NewPW
            // 
            this.textBox_NewPW.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox_NewPW.Location = new System.Drawing.Point(89, 109);
            this.textBox_NewPW.Name = "textBox_NewPW";
            this.textBox_NewPW.Size = new System.Drawing.Size(201, 29);
            this.textBox_NewPW.TabIndex = 11;
            // 
            // textBox_NewAcc
            // 
            this.textBox_NewAcc.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox_NewAcc.Location = new System.Drawing.Point(89, 61);
            this.textBox_NewAcc.Name = "textBox_NewAcc";
            this.textBox_NewAcc.Size = new System.Drawing.Size(201, 29);
            this.textBox_NewAcc.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(6, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 20);
            this.label2.TabIndex = 8;
            this.label2.Text = "密碼";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.Location = new System.Drawing.Point(6, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 20);
            this.label3.TabIndex = 9;
            this.label3.Text = "帳號";
            // 
            // Form_PWInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(349, 322);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form_PWInput";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "帳號密碼模組";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_PWInput_FormClosing);
            this.Load += new System.EventHandler(this.Form_PWInput_Load);
            this.VisibleChanged += new System.EventHandler(this.Form_PWInput_VisibleChanged);
            this.tabControl1.ResumeLayout(false);
            this.tabPage_Login.ResumeLayout(false);
            this.tabPage_Login.PerformLayout();
            this.tabPage_PWManagement.ResumeLayout(false);
            this.tabPage_PWManagement.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.Label label_PW;
        private System.Windows.Forms.TextBox textBox_PW;
        private System.Windows.Forms.Label label_Acc;
        private System.Windows.Forms.ComboBox comboBox_AccList;
        private System.Windows.Forms.Button button_Close;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage_Login;
        private System.Windows.Forms.TabPage tabPage_PWManagement;
        private System.Windows.Forms.TextBox textBox_NewPW;
        private System.Windows.Forms.TextBox textBox_NewAcc;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button_Save;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBox_NowAllAcc;
        private System.Windows.Forms.Button button_Delete;
        private System.Windows.Forms.Label label_;
        private System.Windows.Forms.ComboBox comboBox_NewLevel;
        private System.Windows.Forms.Label label_Level;
        private System.Windows.Forms.TextBox textBox_Level;
    }
}

