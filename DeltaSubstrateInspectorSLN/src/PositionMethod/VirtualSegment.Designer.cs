namespace DeltaSubstrateInspector.src.PositionMethod
{
    partial class VirtualSeg1Form
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.pic_src_img = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pic_preview = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.tkb_dilation = new System.Windows.Forms.TrackBar();
            this.label9 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.tkb_erosion = new System.Windows.Forms.TrackBar();
            this.label6 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.tkb_max_thr = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.tkb_min_thr = new System.Windows.Forms.TrackBar();
            this.BottomToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.TopToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.RightToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.LeftToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.ContentPanel = new System.Windows.Forms.ToolStripContentPanel();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.file_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_load_video = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_save_file = new System.Windows.Forms.ToolStripMenuItem();
            this.lines_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rGBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lCHToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.btn_operate = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_src_img)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_preview)).BeginInit();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tkb_dilation)).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tkb_erosion)).BeginInit();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tkb_max_thr)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tkb_min_thr)).BeginInit();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.pic_src_img);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(18, 345);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(625, 596);
            this.panel1.TabIndex = 17;
            // 
            // pic_src_img
            // 
            this.pic_src_img.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pic_src_img.Location = new System.Drawing.Point(24, 41);
            this.pic_src_img.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pic_src_img.Name = "pic_src_img";
            this.pic_src_img.Size = new System.Drawing.Size(579, 542);
            this.pic_src_img.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic_src_img.TabIndex = 1;
            this.pic_src_img.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label1.Location = new System.Drawing.Point(19, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = "原始影像";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.btn_operate);
            this.panel2.Controls.Add(this.pic_preview);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Location = new System.Drawing.Point(666, 59);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(819, 892);
            this.panel2.TabIndex = 18;
            // 
            // pic_preview
            // 
            this.pic_preview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pic_preview.Location = new System.Drawing.Point(23, 45);
            this.pic_preview.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pic_preview.Name = "pic_preview";
            this.pic_preview.Size = new System.Drawing.Size(776, 824);
            this.pic_preview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic_preview.TabIndex = 1;
            this.pic_preview.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label2.Location = new System.Drawing.Point(18, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 30);
            this.label2.TabIndex = 0;
            this.label2.Text = "預覽影像";
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.White;
            this.panel6.Controls.Add(this.label8);
            this.panel6.Controls.Add(this.tkb_dilation);
            this.panel6.Location = new System.Drawing.Point(353, 233);
            this.panel6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(293, 106);
            this.panel6.TabIndex = 32;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label8.ForeColor = System.Drawing.Color.Gray;
            this.label8.Location = new System.Drawing.Point(7, 12);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(110, 31);
            this.label8.TabIndex = 20;
            this.label8.Text = "膨脹等級";
            // 
            // tkb_dilation
            // 
            this.tkb_dilation.Enabled = false;
            this.tkb_dilation.LargeChange = 1;
            this.tkb_dilation.Location = new System.Drawing.Point(0, 55);
            this.tkb_dilation.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tkb_dilation.Maximum = 21;
            this.tkb_dilation.Minimum = 1;
            this.tkb_dilation.Name = "tkb_dilation";
            this.tkb_dilation.Size = new System.Drawing.Size(290, 69);
            this.tkb_dilation.TabIndex = 0;
            this.tkb_dilation.Value = 1;
            this.tkb_dilation.Scroll += new System.EventHandler(this.tkb_dilation_Scroll);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("微軟正黑體", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.label9.Location = new System.Drawing.Point(346, 199);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(145, 40);
            this.label9.TabIndex = 31;
            this.label9.Text = "膨脹區域";
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.White;
            this.panel5.Controls.Add(this.label7);
            this.panel5.Controls.Add(this.tkb_erosion);
            this.panel5.Location = new System.Drawing.Point(19, 233);
            this.panel5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(312, 106);
            this.panel5.TabIndex = 30;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label7.ForeColor = System.Drawing.Color.Gray;
            this.label7.Location = new System.Drawing.Point(7, 12);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(110, 31);
            this.label7.TabIndex = 20;
            this.label7.Text = "侵蝕等級";
            // 
            // tkb_erosion
            // 
            this.tkb_erosion.Enabled = false;
            this.tkb_erosion.LargeChange = 1;
            this.tkb_erosion.Location = new System.Drawing.Point(0, 55);
            this.tkb_erosion.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tkb_erosion.Maximum = 21;
            this.tkb_erosion.Minimum = 1;
            this.tkb_erosion.Name = "tkb_erosion";
            this.tkb_erosion.Size = new System.Drawing.Size(309, 69);
            this.tkb_erosion.TabIndex = 0;
            this.tkb_erosion.Value = 1;
            this.tkb_erosion.Scroll += new System.EventHandler(this.tkb_erosion_Scroll);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微軟正黑體", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.label6.Location = new System.Drawing.Point(12, 199);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(145, 40);
            this.label6.TabIndex = 29;
            this.label6.Text = "消除雜訊";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.White;
            this.panel4.Controls.Add(this.label5);
            this.panel4.Controls.Add(this.tkb_max_thr);
            this.panel4.Location = new System.Drawing.Point(335, 90);
            this.panel4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(311, 106);
            this.panel4.TabIndex = 28;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label5.ForeColor = System.Drawing.Color.Gray;
            this.label5.Location = new System.Drawing.Point(7, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(192, 31);
            this.label5.TabIndex = 20;
            this.label5.Text = "最大允許(0-255)";
            // 
            // tkb_max_thr
            // 
            this.tkb_max_thr.Enabled = false;
            this.tkb_max_thr.LargeChange = 1;
            this.tkb_max_thr.Location = new System.Drawing.Point(0, 55);
            this.tkb_max_thr.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tkb_max_thr.Maximum = 255;
            this.tkb_max_thr.Name = "tkb_max_thr";
            this.tkb_max_thr.Size = new System.Drawing.Size(308, 69);
            this.tkb_max_thr.TabIndex = 0;
            this.tkb_max_thr.Scroll += new System.EventHandler(this.tkb_max_thr_Scroll);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微軟正黑體", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.label3.Location = new System.Drawing.Point(12, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 40);
            this.label3.TabIndex = 26;
            this.label3.Text = "閥值";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.tkb_min_thr);
            this.panel3.Location = new System.Drawing.Point(18, 90);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(311, 106);
            this.panel3.TabIndex = 27;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.ForeColor = System.Drawing.Color.Gray;
            this.label4.Location = new System.Drawing.Point(7, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(192, 31);
            this.label4.TabIndex = 20;
            this.label4.Text = "最小允許(0-255)";
            // 
            // tkb_min_thr
            // 
            this.tkb_min_thr.Enabled = false;
            this.tkb_min_thr.LargeChange = 1;
            this.tkb_min_thr.Location = new System.Drawing.Point(0, 55);
            this.tkb_min_thr.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tkb_min_thr.Maximum = 255;
            this.tkb_min_thr.Name = "tkb_min_thr";
            this.tkb_min_thr.Size = new System.Drawing.Size(308, 69);
            this.tkb_min_thr.TabIndex = 0;
            this.tkb_min_thr.Scroll += new System.EventHandler(this.tkb_min_thr_Scroll);
            // 
            // BottomToolStripPanel
            // 
            this.BottomToolStripPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BottomToolStripPanel.Location = new System.Drawing.Point(0, 187);
            this.BottomToolStripPanel.Name = "BottomToolStripPanel";
            this.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.BottomToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.BottomToolStripPanel.Size = new System.Drawing.Size(150, 0);
            // 
            // TopToolStripPanel
            // 
            this.TopToolStripPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TopToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.TopToolStripPanel.Name = "TopToolStripPanel";
            this.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.TopToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.TopToolStripPanel.Size = new System.Drawing.Size(150, 25);
            // 
            // RightToolStripPanel
            // 
            this.RightToolStripPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.RightToolStripPanel.Location = new System.Drawing.Point(150, 25);
            this.RightToolStripPanel.Name = "RightToolStripPanel";
            this.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.RightToolStripPanel.RowMargin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.RightToolStripPanel.Size = new System.Drawing.Size(0, 162);
            // 
            // LeftToolStripPanel
            // 
            this.LeftToolStripPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.LeftToolStripPanel.Location = new System.Drawing.Point(0, 25);
            this.LeftToolStripPanel.Name = "LeftToolStripPanel";
            this.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.LeftToolStripPanel.RowMargin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.LeftToolStripPanel.Size = new System.Drawing.Size(0, 162);
            // 
            // ContentPanel
            // 
            this.ContentPanel.Size = new System.Drawing.Size(150, 162);
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.menuStrip.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.file_ToolStripMenuItem,
            this.lines_ToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(9, 3, 0, 3);
            this.menuStrip.Size = new System.Drawing.Size(1497, 55);
            this.menuStrip.TabIndex = 35;
            this.menuStrip.Text = "menuStrip1";
            // 
            // file_ToolStripMenuItem
            // 
            this.file_ToolStripMenuItem.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.file_ToolStripMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.file_ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItem_load_video,
            this.MenuItem_save_file});
            this.file_ToolStripMenuItem.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F, System.Drawing.FontStyle.Bold);
            this.file_ToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.file_ToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Maroon;
            this.file_ToolStripMenuItem.Name = "file_ToolStripMenuItem";
            this.file_ToolStripMenuItem.Padding = new System.Windows.Forms.Padding(25, 15, 4, 0);
            this.file_ToolStripMenuItem.Size = new System.Drawing.Size(94, 49);
            this.file_ToolStripMenuItem.Text = "檔案";
            // 
            // MenuItem_load_video
            // 
            this.MenuItem_load_video.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.MenuItem_load_video.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.MenuItem_load_video.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.MenuItem_load_video.ForeColor = System.Drawing.Color.SteelBlue;
            this.MenuItem_load_video.Name = "MenuItem_load_video";
            this.MenuItem_load_video.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.MenuItem_load_video.Size = new System.Drawing.Size(194, 42);
            this.MenuItem_load_video.Text = "載入圖片";
            this.MenuItem_load_video.Click += new System.EventHandler(this.btn_loadImg_Click);
            // 
            // MenuItem_save_file
            // 
            this.MenuItem_save_file.ForeColor = System.Drawing.Color.SteelBlue;
            this.MenuItem_save_file.Name = "MenuItem_save_file";
            this.MenuItem_save_file.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.MenuItem_save_file.Size = new System.Drawing.Size(194, 42);
            this.MenuItem_save_file.Text = "儲存檔案";
            this.MenuItem_save_file.Click += new System.EventHandler(this.MenuItem_save_file_Click_1);
            // 
            // lines_ToolStripMenuItem
            // 
            this.lines_ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rGBToolStripMenuItem,
            this.hSVToolStripMenuItem,
            this.lCHToolStripMenuItem});
            this.lines_ToolStripMenuItem.Enabled = false;
            this.lines_ToolStripMenuItem.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F, System.Drawing.FontStyle.Bold);
            this.lines_ToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lines_ToolStripMenuItem.Name = "lines_ToolStripMenuItem";
            this.lines_ToolStripMenuItem.Padding = new System.Windows.Forms.Padding(20, 0, 4, 0);
            this.lines_ToolStripMenuItem.Size = new System.Drawing.Size(137, 49);
            this.lines_ToolStripMenuItem.Text = "色域切換";
            // 
            // rGBToolStripMenuItem
            // 
            this.rGBToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rToolStripMenuItem,
            this.gToolStripMenuItem,
            this.bToolStripMenuItem});
            this.rGBToolStripMenuItem.ForeColor = System.Drawing.Color.SteelBlue;
            this.rGBToolStripMenuItem.Name = "rGBToolStripMenuItem";
            this.rGBToolStripMenuItem.Padding = new System.Windows.Forms.Padding(0, 5, 0, 1);
            this.rGBToolStripMenuItem.Size = new System.Drawing.Size(210, 38);
            this.rGBToolStripMenuItem.Text = "RGB";
            this.rGBToolStripMenuItem.Click += new System.EventHandler(this.set_color_method);
            // 
            // rToolStripMenuItem
            // 
            this.rToolStripMenuItem.ForeColor = System.Drawing.Color.SteelBlue;
            this.rToolStripMenuItem.Name = "rToolStripMenuItem";
            this.rToolStripMenuItem.Size = new System.Drawing.Size(116, 34);
            this.rToolStripMenuItem.Text = "R";
            this.rToolStripMenuItem.Click += new System.EventHandler(this.setup_color_space);
            // 
            // gToolStripMenuItem
            // 
            this.gToolStripMenuItem.ForeColor = System.Drawing.Color.SteelBlue;
            this.gToolStripMenuItem.Name = "gToolStripMenuItem";
            this.gToolStripMenuItem.Size = new System.Drawing.Size(116, 34);
            this.gToolStripMenuItem.Text = "G";
            this.gToolStripMenuItem.Click += new System.EventHandler(this.setup_color_space);
            // 
            // bToolStripMenuItem
            // 
            this.bToolStripMenuItem.ForeColor = System.Drawing.Color.SteelBlue;
            this.bToolStripMenuItem.Name = "bToolStripMenuItem";
            this.bToolStripMenuItem.Size = new System.Drawing.Size(116, 34);
            this.bToolStripMenuItem.Text = "B";
            this.bToolStripMenuItem.Click += new System.EventHandler(this.setup_color_space);
            // 
            // hSVToolStripMenuItem
            // 
            this.hSVToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hToolStripMenuItem,
            this.sToolStripMenuItem,
            this.vToolStripMenuItem});
            this.hSVToolStripMenuItem.ForeColor = System.Drawing.Color.SteelBlue;
            this.hSVToolStripMenuItem.Name = "hSVToolStripMenuItem";
            this.hSVToolStripMenuItem.Padding = new System.Windows.Forms.Padding(0, 5, 0, 1);
            this.hSVToolStripMenuItem.Size = new System.Drawing.Size(210, 38);
            this.hSVToolStripMenuItem.Text = "HSV";
            this.hSVToolStripMenuItem.Click += new System.EventHandler(this.set_color_method);
            // 
            // hToolStripMenuItem
            // 
            this.hToolStripMenuItem.ForeColor = System.Drawing.Color.SteelBlue;
            this.hToolStripMenuItem.Name = "hToolStripMenuItem";
            this.hToolStripMenuItem.Size = new System.Drawing.Size(117, 34);
            this.hToolStripMenuItem.Text = "H";
            this.hToolStripMenuItem.Click += new System.EventHandler(this.setup_color_space);
            // 
            // sToolStripMenuItem
            // 
            this.sToolStripMenuItem.ForeColor = System.Drawing.Color.SteelBlue;
            this.sToolStripMenuItem.Name = "sToolStripMenuItem";
            this.sToolStripMenuItem.Size = new System.Drawing.Size(117, 34);
            this.sToolStripMenuItem.Text = "S";
            this.sToolStripMenuItem.Click += new System.EventHandler(this.setup_color_space);
            // 
            // vToolStripMenuItem
            // 
            this.vToolStripMenuItem.ForeColor = System.Drawing.Color.SteelBlue;
            this.vToolStripMenuItem.Name = "vToolStripMenuItem";
            this.vToolStripMenuItem.Size = new System.Drawing.Size(117, 34);
            this.vToolStripMenuItem.Text = "V";
            this.vToolStripMenuItem.Click += new System.EventHandler(this.setup_color_space);
            // 
            // lCHToolStripMenuItem
            // 
            this.lCHToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lToolStripMenuItem,
            this.cToolStripMenuItem,
            this.hToolStripMenuItem1});
            this.lCHToolStripMenuItem.ForeColor = System.Drawing.Color.SteelBlue;
            this.lCHToolStripMenuItem.Name = "lCHToolStripMenuItem";
            this.lCHToolStripMenuItem.Padding = new System.Windows.Forms.Padding(0, 5, 0, 1);
            this.lCHToolStripMenuItem.Size = new System.Drawing.Size(210, 38);
            this.lCHToolStripMenuItem.Text = "LCH";
            this.lCHToolStripMenuItem.Click += new System.EventHandler(this.set_color_method);
            // 
            // lToolStripMenuItem
            // 
            this.lToolStripMenuItem.ForeColor = System.Drawing.Color.SteelBlue;
            this.lToolStripMenuItem.Name = "lToolStripMenuItem";
            this.lToolStripMenuItem.Size = new System.Drawing.Size(117, 34);
            this.lToolStripMenuItem.Text = "L";
            this.lToolStripMenuItem.Click += new System.EventHandler(this.setup_color_space);
            // 
            // cToolStripMenuItem
            // 
            this.cToolStripMenuItem.ForeColor = System.Drawing.Color.SteelBlue;
            this.cToolStripMenuItem.Name = "cToolStripMenuItem";
            this.cToolStripMenuItem.Size = new System.Drawing.Size(117, 34);
            this.cToolStripMenuItem.Text = "C";
            this.cToolStripMenuItem.Click += new System.EventHandler(this.setup_color_space);
            // 
            // hToolStripMenuItem1
            // 
            this.hToolStripMenuItem1.ForeColor = System.Drawing.Color.SteelBlue;
            this.hToolStripMenuItem1.Name = "hToolStripMenuItem1";
            this.hToolStripMenuItem1.Size = new System.Drawing.Size(117, 34);
            this.hToolStripMenuItem1.Text = "H";
            this.hToolStripMenuItem1.Click += new System.EventHandler(this.setup_color_space);
            // 
            // btn_operate
            // 
            this.btn_operate.BackColor = System.Drawing.Color.SteelBlue;
            this.btn_operate.FlatAppearance.BorderSize = 0;
            this.btn_operate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_operate.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_operate.ForeColor = System.Drawing.Color.White;
            this.btn_operate.Location = new System.Drawing.Point(691, 3);
            this.btn_operate.Name = "btn_operate";
            this.btn_operate.Size = new System.Drawing.Size(108, 35);
            this.btn_operate.TabIndex = 36;
            this.btn_operate.Text = "執行";
            this.btn_operate.UseVisualStyleBackColor = false;
            this.btn_operate.Click += new System.EventHandler(this.btn_operate_Click);
            // 
            // VirtualSeg1Form
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1497, 954);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "VirtualSeg1Form";
            this.Text = "VirtualSegmentation1";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_src_img)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_preview)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tkb_dilation)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tkb_erosion)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tkb_max_thr)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tkb_min_thr)).EndInit();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pic_src_img;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pic_preview;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TrackBar tkb_dilation;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TrackBar tkb_erosion;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TrackBar tkb_max_thr;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TrackBar tkb_min_thr;
        private System.Windows.Forms.ToolStripPanel BottomToolStripPanel;
        private System.Windows.Forms.ToolStripPanel TopToolStripPanel;
        private System.Windows.Forms.ToolStripPanel RightToolStripPanel;
        private System.Windows.Forms.ToolStripPanel LeftToolStripPanel;
        private System.Windows.Forms.ToolStripContentPanel ContentPanel;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem file_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_load_video;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_save_file;
        private System.Windows.Forms.ToolStripMenuItem lines_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rGBToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hSVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lCHToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hToolStripMenuItem1;
        private System.Windows.Forms.Button btn_operate;
    }
}