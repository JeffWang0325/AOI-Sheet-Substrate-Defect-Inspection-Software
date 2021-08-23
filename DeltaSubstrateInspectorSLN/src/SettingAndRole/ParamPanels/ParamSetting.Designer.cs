namespace DeltaSubstrateInspector.src.InspectionForms.ParamPanels
{
    partial class ParamSetting
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

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_save = new System.Windows.Forms.Button();
            this.button_ResistPanel = new System.Windows.Forms.Button();
            this.InductanceInsp = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button_DefaultInspect = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.pnl_oper_roles = new System.Windows.Forms.Panel();
            this.button_PatternCheck = new System.Windows.Forms.Button();
            this.txt_name = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.method_UC_pnl = new System.Windows.Forms.Panel();
            this.panelInspMethod = new System.Windows.Forms.Panel();
            this.groupBox1.SuspendLayout();
            this.panelInspMethod.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panelInspMethod);
            this.groupBox1.Controls.Add(this.btn_save);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.pnl_oper_roles);
            this.groupBox1.Controls.Add(this.txt_name);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox1.ForeColor = System.Drawing.Color.Gray;
            this.groupBox1.Location = new System.Drawing.Point(14, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(237, 865);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "檢測設定";
            // 
            // btn_save
            // 
            this.btn_save.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(76)))), ((int)(((byte)(61)))));
            this.btn_save.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btn_save.FlatAppearance.BorderSize = 0;
            this.btn_save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_save.Font = new System.Drawing.Font("微軟正黑體", 14F, System.Drawing.FontStyle.Bold);
            this.btn_save.ForeColor = System.Drawing.SystemColors.Control;
            this.btn_save.Location = new System.Drawing.Point(11, 822);
            this.btn_save.Margin = new System.Windows.Forms.Padding(2);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(217, 29);
            this.btn_save.TabIndex = 8;
            this.btn_save.Text = "儲存";
            this.btn_save.UseVisualStyleBackColor = false;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // button_ResistPanel
            // 
            this.button_ResistPanel.Location = new System.Drawing.Point(3, 135);
            this.button_ResistPanel.Name = "button_ResistPanel";
            this.button_ResistPanel.Size = new System.Drawing.Size(217, 38);
            this.button_ResistPanel.TabIndex = 18;
            this.button_ResistPanel.Tag = "ResistPanel";
            this.button_ResistPanel.Text = "電阻基板檢測";
            this.button_ResistPanel.UseVisualStyleBackColor = true;
            this.button_ResistPanel.Click += new System.EventHandler(this.method_btn_Click);
            // 
            // InductanceInsp
            // 
            this.InductanceInsp.Location = new System.Drawing.Point(3, 91);
            this.InductanceInsp.Name = "InductanceInsp";
            this.InductanceInsp.Size = new System.Drawing.Size(217, 38);
            this.InductanceInsp.TabIndex = 17;
            this.InductanceInsp.Tag = "InductanceInsp";
            this.InductanceInsp.Text = "基板檢測";
            this.InductanceInsp.UseVisualStyleBackColor = true;
            this.InductanceInsp.Click += new System.EventHandler(this.method_btn_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button3.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("微軟正黑體", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button3.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.button3.Location = new System.Drawing.Point(179, 301);
            this.button3.Margin = new System.Windows.Forms.Padding(2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(49, 26);
            this.button3.TabIndex = 9;
            this.button3.Text = "刪除";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button_DefaultInspect
            // 
            this.button_DefaultInspect.Location = new System.Drawing.Point(3, 3);
            this.button_DefaultInspect.Name = "button_DefaultInspect";
            this.button_DefaultInspect.Size = new System.Drawing.Size(217, 38);
            this.button_DefaultInspect.TabIndex = 13;
            this.button_DefaultInspect.Tag = "DefaultInspect";
            this.button_DefaultInspect.Text = "預設檢查";
            this.button_DefaultInspect.UseVisualStyleBackColor = true;
            this.button_DefaultInspect.Click += new System.EventHandler(this.method_btn_Click);
            // 
            // button1
            // 
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.button1.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.button1.Location = new System.Drawing.Point(11, 302);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(160, 25);
            this.button1.TabIndex = 4;
            this.button1.Text = "Inspection Overview";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.UseVisualStyleBackColor = true;
            // 
            // pnl_oper_roles
            // 
            this.pnl_oper_roles.BackColor = System.Drawing.Color.White;
            this.pnl_oper_roles.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnl_oper_roles.Location = new System.Drawing.Point(11, 333);
            this.pnl_oper_roles.Name = "pnl_oper_roles";
            this.pnl_oper_roles.Size = new System.Drawing.Size(217, 484);
            this.pnl_oper_roles.TabIndex = 3;
            // 
            // button_PatternCheck
            // 
            this.button_PatternCheck.Location = new System.Drawing.Point(3, 47);
            this.button_PatternCheck.Name = "button_PatternCheck";
            this.button_PatternCheck.Size = new System.Drawing.Size(217, 38);
            this.button_PatternCheck.TabIndex = 12;
            this.button_PatternCheck.Tag = "PatternCheck";
            this.button_PatternCheck.Text = "圖樣檢查";
            this.button_PatternCheck.UseVisualStyleBackColor = true;
            this.button_PatternCheck.Click += new System.EventHandler(this.method_btn_Click);
            // 
            // txt_name
            // 
            this.txt_name.Location = new System.Drawing.Point(11, 54);
            this.txt_name.Name = "txt_name";
            this.txt_name.Size = new System.Drawing.Size(217, 29);
            this.txt_name.TabIndex = 3;
            this.txt_name.Text = "DefectName_default";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "檢測方法";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "瑕疵名稱";
            // 
            // method_UC_pnl
            // 
            this.method_UC_pnl.AutoScroll = true;
            this.method_UC_pnl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.method_UC_pnl.Location = new System.Drawing.Point(257, 13);
            this.method_UC_pnl.Name = "method_UC_pnl";
            this.method_UC_pnl.Size = new System.Drawing.Size(1550, 865);
            this.method_UC_pnl.TabIndex = 10;
            // 
            // panelInspMethod
            // 
            this.panelInspMethod.AutoScroll = true;
            this.panelInspMethod.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelInspMethod.Controls.Add(this.button_DefaultInspect);
            this.panelInspMethod.Controls.Add(this.button_ResistPanel);
            this.panelInspMethod.Controls.Add(this.button_PatternCheck);
            this.panelInspMethod.Controls.Add(this.InductanceInsp);
            this.panelInspMethod.Location = new System.Drawing.Point(11, 113);
            this.panelInspMethod.Name = "panelInspMethod";
            this.panelInspMethod.Size = new System.Drawing.Size(226, 183);
            this.panelInspMethod.TabIndex = 0;
            // 
            // ParamSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.Controls.Add(this.method_UC_pnl);
            this.Controls.Add(this.groupBox1);
            this.Name = "ParamSetting";
            this.Size = new System.Drawing.Size(1830, 890);
            this.Load += new System.EventHandler(this.ParamSetting_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panelInspMethod.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pnl_oper_roles;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txt_name;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Panel method_UC_pnl;
        private System.Windows.Forms.Button button_PatternCheck;
        private System.Windows.Forms.Button button_DefaultInspect;
        private System.Windows.Forms.Button InductanceInsp;
        private System.Windows.Forms.Button button_ResistPanel;
        private System.Windows.Forms.Panel panelInspMethod;
    }
}
