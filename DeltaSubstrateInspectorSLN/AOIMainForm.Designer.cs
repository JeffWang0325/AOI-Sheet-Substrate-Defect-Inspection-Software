namespace DeltaSubstrateInspector
{
    partial class AOIMainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AOIMainForm));
            this.timer_InitCheck = new System.Windows.Forms.Timer(this.components);
            this.timer_InfoUpdate = new System.Windows.Forms.Timer(this.components);
            this.TimerCycleTest = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.labUserLevel = new System.Windows.Forms.Label();
            this.label_PartNumber = new System.Windows.Forms.Label();
            this.label_RecipeName = new System.Windows.Forms.Label();
            this.label_Version = new System.Windows.Forms.Label();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.file_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.load_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exit_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xu6vu04ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wholeFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cycleTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.多組測試ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.run_StripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.StopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SysModuletoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.config模組ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iO模組ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.光源模組ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tCPIP模組ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.訊息模組ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reviewer模組ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.帳號密碼模組ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.登入管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.登出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.權限設定ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.語言設定ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.中文ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.英文ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox_DeltaIcon = new System.Windows.Forms.PictureBox();
            this.btnMinForm = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btn_create_model = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_DeltaIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // timer_InitCheck
            // 
            this.timer_InitCheck.Tick += new System.EventHandler(this.timer_InitCheck_Tick);
            // 
            // timer_InfoUpdate
            // 
            this.timer_InfoUpdate.Enabled = true;
            this.timer_InfoUpdate.Interval = 1000;
            this.timer_InfoUpdate.Tick += new System.EventHandler(this.timer_InfoUpdate_Tick);
            // 
            // TimerCycleTest
            // 
            this.TimerCycleTest.Interval = 3000;
            this.TimerCycleTest.Tick += new System.EventHandler(this.TimerCycleTest_Tick);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.panel1.BackgroundImage = global::DeltaSubstrateInspector.Properties.Resources.bar;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.labUserLevel);
            this.panel1.Controls.Add(this.label_PartNumber);
            this.panel1.Controls.Add(this.label_RecipeName);
            this.panel1.Controls.Add(this.label_Version);
            this.panel1.Controls.Add(this.menuStrip);
            this.panel1.Controls.Add(this.pictureBox_DeltaIcon);
            this.panel1.Controls.Add(this.btnMinForm);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1920, 47);
            this.panel1.TabIndex = 10;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // labUserLevel
            // 
            this.labUserLevel.AutoSize = true;
            this.labUserLevel.BackColor = System.Drawing.Color.Transparent;
            this.labUserLevel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labUserLevel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labUserLevel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            this.labUserLevel.ForeColor = System.Drawing.Color.Gold;
            this.labUserLevel.Location = new System.Drawing.Point(44, 31);
            this.labUserLevel.Name = "labUserLevel";
            this.labUserLevel.Size = new System.Drawing.Size(48, 15);
            this.labUserLevel.TabIndex = 47;
            this.labUserLevel.Text = "使用者";
            this.labUserLevel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labUserLevel.Click += new System.EventHandler(this.labUserLevel_Click);
            // 
            // label_PartNumber
            // 
            this.label_PartNumber.AutoSize = true;
            this.label_PartNumber.BackColor = System.Drawing.Color.Transparent;
            this.label_PartNumber.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label_PartNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_PartNumber.ForeColor = System.Drawing.Color.Gold;
            this.label_PartNumber.Location = new System.Drawing.Point(819, 15);
            this.label_PartNumber.Name = "label_PartNumber";
            this.label_PartNumber.Size = new System.Drawing.Size(186, 20);
            this.label_PartNumber.TabIndex = 46;
            this.label_PartNumber.Text = "生產料號: Default1234";
            this.label_PartNumber.Click += new System.EventHandler(this.label_PartNumber_Click);
            // 
            // label_RecipeName
            // 
            this.label_RecipeName.AutoSize = true;
            this.label_RecipeName.BackColor = System.Drawing.Color.Transparent;
            this.label_RecipeName.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label_RecipeName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_RecipeName.ForeColor = System.Drawing.Color.Gold;
            this.label_RecipeName.Location = new System.Drawing.Point(1246, 15);
            this.label_RecipeName.Name = "label_RecipeName";
            this.label_RecipeName.Size = new System.Drawing.Size(82, 20);
            this.label_RecipeName.TabIndex = 45;
            this.label_RecipeName.Text = "程式名稱:";
            this.label_RecipeName.Click += new System.EventHandler(this.label_RecipeName_Click);
            // 
            // label_Version
            // 
            this.label_Version.AutoSize = true;
            this.label_Version.BackColor = System.Drawing.Color.Transparent;
            this.label_Version.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Version.ForeColor = System.Drawing.Color.Gold;
            this.label_Version.Location = new System.Drawing.Point(1663, 15);
            this.label_Version.Name = "label_Version";
            this.label_Version.Size = new System.Drawing.Size(53, 20);
            this.label_Version.TabIndex = 44;
            this.label_Version.Text = " 版本:";
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.Color.Transparent;
            this.menuStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.menuStrip.GripMargin = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.file_ToolStripMenuItem,
            this.xu6vu04ToolStripMenuItem,
            this.run_StripMenuItem1,
            this.StopToolStripMenuItem,
            this.SysModuletoolStripMenuItem});
            this.menuStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.menuStrip.Location = new System.Drawing.Point(125, 9);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(9, 0, 0, 0);
            this.menuStrip.Size = new System.Drawing.Size(582, 28);
            this.menuStrip.TabIndex = 36;
            this.menuStrip.Text = "menuStrip1";
            // 
            // file_ToolStripMenuItem
            // 
            this.file_ToolStripMenuItem.BackColor = System.Drawing.Color.Transparent;
            this.file_ToolStripMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.file_ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.load_ToolStripMenuItem,
            this.exit_ToolStripMenuItem});
            this.file_ToolStripMenuItem.Font = new System.Drawing.Font("微軟正黑體", 14F);
            this.file_ToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.file_ToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Maroon;
            this.file_ToolStripMenuItem.Name = "file_ToolStripMenuItem";
            this.file_ToolStripMenuItem.Padding = new System.Windows.Forms.Padding(25, 0, 25, 0);
            this.file_ToolStripMenuItem.Size = new System.Drawing.Size(102, 28);
            this.file_ToolStripMenuItem.Text = "檔案";
            this.file_ToolStripMenuItem.DropDownClosed += new System.EventHandler(this.file_ToolStripMenuItem_DropDownClosed);
            this.file_ToolStripMenuItem.DropDownOpening += new System.EventHandler(this.file_ToolStripMenuItem_DropDownOpening);
            this.file_ToolStripMenuItem.Click += new System.EventHandler(this.file_ToolStripMenuItem_Click);
            this.file_ToolStripMenuItem.MouseDown += new System.Windows.Forms.MouseEventHandler(this.file_ToolStripMenuItem_MouseDown);
            this.file_ToolStripMenuItem.MouseEnter += new System.EventHandler(this.file_ToolStripMenuItem_MouseEnter);
            this.file_ToolStripMenuItem.MouseLeave += new System.EventHandler(this.file_ToolStripMenuItem_MouseLeave);
            this.file_ToolStripMenuItem.MouseHover += new System.EventHandler(this.file_ToolStripMenuItem_MouseHover);
            this.file_ToolStripMenuItem.MouseMove += new System.Windows.Forms.MouseEventHandler(this.file_ToolStripMenuItem_MouseMove);
            // 
            // load_ToolStripMenuItem
            // 
            this.load_ToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.load_ToolStripMenuItem.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.load_ToolStripMenuItem.Name = "load_ToolStripMenuItem";
            this.load_ToolStripMenuItem.Padding = new System.Windows.Forms.Padding(0);
            this.load_ToolStripMenuItem.Size = new System.Drawing.Size(156, 26);
            this.load_ToolStripMenuItem.Text = "程式設定";
            this.load_ToolStripMenuItem.Click += new System.EventHandler(this.load_ToolStripMenuItem_Click);
            // 
            // exit_ToolStripMenuItem
            // 
            this.exit_ToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.exit_ToolStripMenuItem.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.exit_ToolStripMenuItem.Name = "exit_ToolStripMenuItem";
            this.exit_ToolStripMenuItem.Padding = new System.Windows.Forms.Padding(0);
            this.exit_ToolStripMenuItem.Size = new System.Drawing.Size(156, 26);
            this.exit_ToolStripMenuItem.Text = "關閉";
            this.exit_ToolStripMenuItem.Click += new System.EventHandler(this.exit_ToolStripMenuItem_Click_1);
            // 
            // xu6vu04ToolStripMenuItem
            // 
            this.xu6vu04ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.wholeFolderToolStripMenuItem,
            this.cycleTestToolStripMenuItem,
            this.多組測試ToolStripMenuItem});
            this.xu6vu04ToolStripMenuItem.Font = new System.Drawing.Font("微軟正黑體", 14F);
            this.xu6vu04ToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.xu6vu04ToolStripMenuItem.Name = "xu6vu04ToolStripMenuItem";
            this.xu6vu04ToolStripMenuItem.Size = new System.Drawing.Size(98, 28);
            this.xu6vu04ToolStripMenuItem.Text = "離線檢測";
            this.xu6vu04ToolStripMenuItem.DropDownClosed += new System.EventHandler(this.xu6vu04ToolStripMenuItem_DropDownClosed);
            this.xu6vu04ToolStripMenuItem.DropDownOpening += new System.EventHandler(this.xu6vu04ToolStripMenuItem_DropDownOpening);
            // 
            // wholeFolderToolStripMenuItem
            // 
            this.wholeFolderToolStripMenuItem.Font = new System.Drawing.Font("微軟正黑體", 14F);
            this.wholeFolderToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ControlText;
            this.wholeFolderToolStripMenuItem.Name = "wholeFolderToolStripMenuItem";
            this.wholeFolderToolStripMenuItem.Size = new System.Drawing.Size(194, 28);
            this.wholeFolderToolStripMenuItem.Text = "單片檢測";
            this.wholeFolderToolStripMenuItem.Click += new System.EventHandler(this.simulation_ToolStripMenuItem_Click);
            // 
            // cycleTestToolStripMenuItem
            // 
            this.cycleTestToolStripMenuItem.Name = "cycleTestToolStripMenuItem";
            this.cycleTestToolStripMenuItem.Size = new System.Drawing.Size(194, 28);
            this.cycleTestToolStripMenuItem.Text = "單片循環檢測";
            this.cycleTestToolStripMenuItem.Click += new System.EventHandler(this.cycleTestToolStripMenuItem_Click);
            // 
            // 多組測試ToolStripMenuItem
            // 
            this.多組測試ToolStripMenuItem.Name = "多組測試ToolStripMenuItem";
            this.多組測試ToolStripMenuItem.Size = new System.Drawing.Size(194, 28);
            this.多組測試ToolStripMenuItem.Text = "多片檢測";
            this.多組測試ToolStripMenuItem.Click += new System.EventHandler(this.多組測試ToolStripMenuItem_Click);
            // 
            // run_StripMenuItem1
            // 
            this.run_StripMenuItem1.Font = new System.Drawing.Font("微軟正黑體", 14F);
            this.run_StripMenuItem1.ForeColor = System.Drawing.Color.White;
            this.run_StripMenuItem1.Name = "run_StripMenuItem1";
            this.run_StripMenuItem1.Padding = new System.Windows.Forms.Padding(25, 0, 25, 0);
            this.run_StripMenuItem1.Size = new System.Drawing.Size(120, 28);
            this.run_StripMenuItem1.Text = "執行 ▶";
            this.run_StripMenuItem1.Click += new System.EventHandler(this.run_StripMenuItem1_Click);
            // 
            // StopToolStripMenuItem
            // 
            this.StopToolStripMenuItem.Enabled = false;
            this.StopToolStripMenuItem.Font = new System.Drawing.Font("微軟正黑體", 14F);
            this.StopToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.StopToolStripMenuItem.Name = "StopToolStripMenuItem";
            this.StopToolStripMenuItem.Size = new System.Drawing.Size(76, 28);
            this.StopToolStripMenuItem.Text = "停止 ■";
            this.StopToolStripMenuItem.Click += new System.EventHandler(this.StopToolStripMenuItem_Click);
            // 
            // SysModuletoolStripMenuItem
            // 
            this.SysModuletoolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.config模組ToolStripMenuItem,
            this.iO模組ToolStripMenuItem,
            this.光源模組ToolStripMenuItem,
            this.tCPIP模組ToolStripMenuItem,
            this.訊息模組ToolStripMenuItem,
            this.reviewer模組ToolStripMenuItem,
            this.帳號密碼模組ToolStripMenuItem,
            this.權限設定ToolStripMenuItem,
            this.語言設定ToolStripMenuItem});
            this.SysModuletoolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.SysModuletoolStripMenuItem.Name = "SysModuletoolStripMenuItem";
            this.SysModuletoolStripMenuItem.Size = new System.Drawing.Size(85, 24);
            this.SysModuletoolStripMenuItem.Text = "模組維護";
            this.SysModuletoolStripMenuItem.DropDownClosed += new System.EventHandler(this.SysModuletoolStripMenuItem_DropDownClosed);
            this.SysModuletoolStripMenuItem.DropDownOpening += new System.EventHandler(this.SysModuletoolStripMenuItem_DropDownOpening);
            this.SysModuletoolStripMenuItem.Click += new System.EventHandler(this.SysModuletoolStripMenuItem_Click);
            // 
            // config模組ToolStripMenuItem
            // 
            this.config模組ToolStripMenuItem.Name = "config模組ToolStripMenuItem";
            this.config模組ToolStripMenuItem.Size = new System.Drawing.Size(175, 24);
            this.config模組ToolStripMenuItem.Text = "Config模組";
            this.config模組ToolStripMenuItem.Click += new System.EventHandler(this.config模組ToolStripMenuItem_Click);
            // 
            // iO模組ToolStripMenuItem
            // 
            this.iO模組ToolStripMenuItem.Name = "iO模組ToolStripMenuItem";
            this.iO模組ToolStripMenuItem.Size = new System.Drawing.Size(175, 24);
            this.iO模組ToolStripMenuItem.Text = "IO模組";
            this.iO模組ToolStripMenuItem.Click += new System.EventHandler(this.iO模組ToolStripMenuItem_Click);
            // 
            // 光源模組ToolStripMenuItem
            // 
            this.光源模組ToolStripMenuItem.Name = "光源模組ToolStripMenuItem";
            this.光源模組ToolStripMenuItem.Size = new System.Drawing.Size(175, 24);
            this.光源模組ToolStripMenuItem.Text = "光源模組";
            this.光源模組ToolStripMenuItem.Click += new System.EventHandler(this.光源模組ToolStripMenuItem_Click);
            // 
            // tCPIP模組ToolStripMenuItem
            // 
            this.tCPIP模組ToolStripMenuItem.Name = "tCPIP模組ToolStripMenuItem";
            this.tCPIP模組ToolStripMenuItem.Size = new System.Drawing.Size(175, 24);
            this.tCPIP模組ToolStripMenuItem.Text = "TCP/IP模組";
            this.tCPIP模組ToolStripMenuItem.Click += new System.EventHandler(this.tCPIP模組ToolStripMenuItem_Click);
            // 
            // 訊息模組ToolStripMenuItem
            // 
            this.訊息模組ToolStripMenuItem.Name = "訊息模組ToolStripMenuItem";
            this.訊息模組ToolStripMenuItem.Size = new System.Drawing.Size(175, 24);
            this.訊息模組ToolStripMenuItem.Text = "訊息模組";
            this.訊息模組ToolStripMenuItem.Click += new System.EventHandler(this.訊息模組ToolStripMenuItem_Click);
            // 
            // reviewer模組ToolStripMenuItem
            // 
            this.reviewer模組ToolStripMenuItem.Name = "reviewer模組ToolStripMenuItem";
            this.reviewer模組ToolStripMenuItem.Size = new System.Drawing.Size(175, 24);
            this.reviewer模組ToolStripMenuItem.Text = "Reviewer模組";
            this.reviewer模組ToolStripMenuItem.Click += new System.EventHandler(this.reviewer模組ToolStripMenuItem_Click);
            // 
            // 帳號密碼模組ToolStripMenuItem
            // 
            this.帳號密碼模組ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.登入管理ToolStripMenuItem,
            this.登出ToolStripMenuItem});
            this.帳號密碼模組ToolStripMenuItem.Name = "帳號密碼模組ToolStripMenuItem";
            this.帳號密碼模組ToolStripMenuItem.Size = new System.Drawing.Size(175, 24);
            this.帳號密碼模組ToolStripMenuItem.Text = "帳號密碼模組";
            // 
            // 登入管理ToolStripMenuItem
            // 
            this.登入管理ToolStripMenuItem.Name = "登入管理ToolStripMenuItem";
            this.登入管理ToolStripMenuItem.Size = new System.Drawing.Size(146, 24);
            this.登入管理ToolStripMenuItem.Text = "登入/管理";
            this.登入管理ToolStripMenuItem.Click += new System.EventHandler(this.登入管理ToolStripMenuItem_Click);
            // 
            // 登出ToolStripMenuItem
            // 
            this.登出ToolStripMenuItem.Name = "登出ToolStripMenuItem";
            this.登出ToolStripMenuItem.Size = new System.Drawing.Size(146, 24);
            this.登出ToolStripMenuItem.Text = "登出";
            this.登出ToolStripMenuItem.Click += new System.EventHandler(this.登出ToolStripMenuItem_Click);
            // 
            // 權限設定ToolStripMenuItem
            // 
            this.權限設定ToolStripMenuItem.Name = "權限設定ToolStripMenuItem";
            this.權限設定ToolStripMenuItem.Size = new System.Drawing.Size(175, 24);
            this.權限設定ToolStripMenuItem.Text = "權限設定";
            this.權限設定ToolStripMenuItem.Click += new System.EventHandler(this.權限設定ToolStripMenuItem_Click);
            // 
            // 語言設定ToolStripMenuItem
            // 
            this.語言設定ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.中文ToolStripMenuItem,
            this.英文ToolStripMenuItem});
            this.語言設定ToolStripMenuItem.Image = global::DeltaSubstrateInspector.Properties.Resources.language_64;
            this.語言設定ToolStripMenuItem.Name = "語言設定ToolStripMenuItem";
            this.語言設定ToolStripMenuItem.Size = new System.Drawing.Size(179, 26);
            this.語言設定ToolStripMenuItem.Text = "語言設定";
            // 
            // 中文ToolStripMenuItem
            // 
            this.中文ToolStripMenuItem.Name = "中文ToolStripMenuItem";
            this.中文ToolStripMenuItem.Size = new System.Drawing.Size(152, 24);
            this.中文ToolStripMenuItem.Text = "中文";
            this.中文ToolStripMenuItem.Click += new System.EventHandler(this.中文ToolStripMenuItem_Click_1);
            // 
            // 英文ToolStripMenuItem
            // 
            this.英文ToolStripMenuItem.Name = "英文ToolStripMenuItem";
            this.英文ToolStripMenuItem.Size = new System.Drawing.Size(152, 24);
            this.英文ToolStripMenuItem.Text = "English";
            this.英文ToolStripMenuItem.Click += new System.EventHandler(this.英文ToolStripMenuItem_Click_1);
            // 
            // pictureBox_DeltaIcon
            // 
            this.pictureBox_DeltaIcon.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_DeltaIcon.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox_DeltaIcon.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_DeltaIcon.Image")));
            this.pictureBox_DeltaIcon.Location = new System.Drawing.Point(20, 5);
            this.pictureBox_DeltaIcon.Name = "pictureBox_DeltaIcon";
            this.pictureBox_DeltaIcon.Size = new System.Drawing.Size(94, 24);
            this.pictureBox_DeltaIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_DeltaIcon.TabIndex = 0;
            this.pictureBox_DeltaIcon.TabStop = false;
            this.pictureBox_DeltaIcon.MouseHover += new System.EventHandler(this.pictureBox_DeltaIcon_MouseHover);
            // 
            // btnMinForm
            // 
            this.btnMinForm.BackColor = System.Drawing.Color.Transparent;
            this.btnMinForm.FlatAppearance.BorderSize = 0;
            this.btnMinForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinForm.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnMinForm.ForeColor = System.Drawing.Color.White;
            this.btnMinForm.Location = new System.Drawing.Point(1826, 7);
            this.btnMinForm.Name = "btnMinForm";
            this.btnMinForm.Size = new System.Drawing.Size(38, 32);
            this.btnMinForm.TabIndex = 22;
            this.btnMinForm.Text = "-";
            this.btnMinForm.UseVisualStyleBackColor = false;
            this.btnMinForm.Click += new System.EventHandler(this.btnMinForm_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(1870, 7);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(38, 32);
            this.button1.TabIndex = 22;
            this.button1.Text = "X";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btn_create_model
            // 
            this.btn_create_model.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_create_model.BackgroundImage")));
            this.btn_create_model.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_create_model.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btn_create_model.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_create_model.Location = new System.Drawing.Point(140, 560);
            this.btn_create_model.Name = "btn_create_model";
            this.btn_create_model.Size = new System.Drawing.Size(40, 40);
            this.btn_create_model.TabIndex = 11;
            this.btn_create_model.UseVisualStyleBackColor = true;
            // 
            // AOIMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(1920, 1040);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AOIMainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AOIMainForm_FormClosing);
            this.Load += new System.EventHandler(this.AOIMainForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AOIMainForm_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_DeltaIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox_DeltaIcon;
        private System.Windows.Forms.Button btn_create_model;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem file_ToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolStripMenuItem run_StripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem xu6vu04ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wholeFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem StopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SysModuletoolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iO模組ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tCPIP模組ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 訊息模組ToolStripMenuItem;
        private System.Windows.Forms.Label label_Version;
        private System.Windows.Forms.Label label_RecipeName;
        private System.Windows.Forms.ToolStripMenuItem 光源模組ToolStripMenuItem;
        private System.Windows.Forms.Timer timer_InitCheck;
        private System.Windows.Forms.Timer timer_InfoUpdate;
        private System.Windows.Forms.ToolStripMenuItem reviewer模組ToolStripMenuItem;
        private System.Windows.Forms.Label label_PartNumber;
        private System.Windows.Forms.ToolStripMenuItem 帳號密碼模組ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 登入管理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 登出ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem load_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exit_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cycleTestToolStripMenuItem;
        private System.Windows.Forms.Timer TimerCycleTest;
        private System.Windows.Forms.ToolStripMenuItem 多組測試ToolStripMenuItem;
        private System.Windows.Forms.Label labUserLevel;
        private System.Windows.Forms.ToolStripMenuItem 權限設定ToolStripMenuItem;
        private System.Windows.Forms.Button btnMinForm;
        private System.Windows.Forms.ToolStripMenuItem config模組ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 語言設定ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 中文ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 英文ToolStripMenuItem;
    }
}

