namespace DeltaSubstrateInspector.src.PositionMethod.AffineMethod
{
    partial class UserControl_AffineTrans_2
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
            this.btn_rotate = new System.Windows.Forms.Button();
            this.hWindowControl_preview = new HalconDotNet.HWindowControl();
            this.btn_load_image = new System.Windows.Forms.Button();
            this.openFileDialog_file = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.tabPage_matching = new System.Windows.Forms.TabPage();
            this.btn_save = new System.Windows.Forms.Button();
            this.lab_model_status = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btn_rotate_images = new System.Windows.Forms.Button();
            this.btn_load_markimg_2 = new System.Windows.Forms.Button();
            this.hWindowControl_mark_2 = new HalconDotNet.HWindowControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_find_mark = new System.Windows.Forms.Button();
            this.btn_load_markimg_1 = new System.Windows.Forms.Button();
            this.hWindowControl_mark_1 = new HalconDotNet.HWindowControl();
            this.btn_create_model = new System.Windows.Forms.Button();
            this.txt_matching_score = new System.Windows.Forms.TextBox();
            this.txt_motion_shift_dist = new System.Windows.Forms.TextBox();
            this.txt_pixel_resolution = new System.Windows.Forms.TextBox();
            this.txt_save_path = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_read_model = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl_matching = new System.Windows.Forms.TabControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.tabPage_matching.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabControl_matching.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_rotate
            // 
            this.btn_rotate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(166)))), ((int)(((byte)(137)))));
            this.btn_rotate.Enabled = false;
            this.btn_rotate.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(166)))), ((int)(((byte)(137)))));
            this.btn_rotate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_rotate.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_rotate.Location = new System.Drawing.Point(505, 548);
            this.btn_rotate.Name = "btn_rotate";
            this.btn_rotate.Size = new System.Drawing.Size(101, 32);
            this.btn_rotate.TabIndex = 1;
            this.btn_rotate.Text = "Rotate";
            this.btn_rotate.UseVisualStyleBackColor = false;
            this.btn_rotate.Click += new System.EventHandler(this.btn_rotate_Click);
            // 
            // hWindowControl_preview
            // 
            this.hWindowControl_preview.BackColor = System.Drawing.Color.Black;
            this.hWindowControl_preview.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl_preview.ImagePart = new System.Drawing.Rectangle(0, 0, 2448, 2048);
            this.hWindowControl_preview.Location = new System.Drawing.Point(7, 9);
            this.hWindowControl_preview.Name = "hWindowControl_preview";
            this.hWindowControl_preview.Size = new System.Drawing.Size(599, 533);
            this.hWindowControl_preview.TabIndex = 0;
            this.hWindowControl_preview.WindowSize = new System.Drawing.Size(599, 533);
            // 
            // btn_load_image
            // 
            this.btn_load_image.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(166)))), ((int)(((byte)(137)))));
            this.btn_load_image.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(166)))), ((int)(((byte)(137)))));
            this.btn_load_image.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_load_image.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_load_image.Location = new System.Drawing.Point(398, 548);
            this.btn_load_image.Name = "btn_load_image";
            this.btn_load_image.Size = new System.Drawing.Size(101, 32);
            this.btn_load_image.TabIndex = 0;
            this.btn_load_image.Text = "Load Image";
            this.btn_load_image.UseVisualStyleBackColor = false;
            this.btn_load_image.Click += new System.EventHandler(this.btn_load_image_Click);
            // 
            // openFileDialog_file
            // 
            this.openFileDialog_file.FileName = "openFileDialog1";
            // 
            // tabPage_matching
            // 
            this.tabPage_matching.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabPage_matching.Controls.Add(this.btn_save);
            this.tabPage_matching.Controls.Add(this.lab_model_status);
            this.tabPage_matching.Controls.Add(this.label5);
            this.tabPage_matching.Controls.Add(this.groupBox3);
            this.tabPage_matching.Controls.Add(this.groupBox2);
            this.tabPage_matching.Controls.Add(this.txt_matching_score);
            this.tabPage_matching.Controls.Add(this.txt_motion_shift_dist);
            this.tabPage_matching.Controls.Add(this.txt_pixel_resolution);
            this.tabPage_matching.Controls.Add(this.txt_save_path);
            this.tabPage_matching.Controls.Add(this.label4);
            this.tabPage_matching.Controls.Add(this.label3);
            this.tabPage_matching.Controls.Add(this.label2);
            this.tabPage_matching.Controls.Add(this.btn_read_model);
            this.tabPage_matching.Controls.Add(this.label1);
            this.tabPage_matching.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tabPage_matching.Location = new System.Drawing.Point(4, 29);
            this.tabPage_matching.Name = "tabPage_matching";
            this.tabPage_matching.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_matching.Size = new System.Drawing.Size(652, 593);
            this.tabPage_matching.TabIndex = 0;
            this.tabPage_matching.Text = "Teach";
            this.tabPage_matching.UseVisualStyleBackColor = true;
            // 
            // btn_save
            // 
            this.btn_save.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(166)))), ((int)(((byte)(137)))));
            this.btn_save.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(166)))), ((int)(((byte)(137)))));
            this.btn_save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_save.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_save.Location = new System.Drawing.Point(439, 52);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(144, 32);
            this.btn_save.TabIndex = 2;
            this.btn_save.Text = "Save";
            this.btn_save.UseVisualStyleBackColor = false;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // lab_model_status
            // 
            this.lab_model_status.BackColor = System.Drawing.Color.Black;
            this.lab_model_status.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lab_model_status.ForeColor = System.Drawing.Color.Red;
            this.lab_model_status.Location = new System.Drawing.Point(243, 9);
            this.lab_model_status.Name = "lab_model_status";
            this.lab_model_status.Size = new System.Drawing.Size(158, 36);
            this.lab_model_status.TabIndex = 15;
            this.lab_model_status.Text = "Offline";
            this.lab_model_status.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label5.Location = new System.Drawing.Point(112, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(124, 20);
            this.label5.TabIndex = 14;
            this.label5.Text = "Model Status：";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btn_rotate_images);
            this.groupBox3.Controls.Add(this.btn_load_markimg_2);
            this.groupBox3.Controls.Add(this.hWindowControl_mark_2);
            this.groupBox3.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox3.Location = new System.Drawing.Point(330, 208);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(316, 377);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Mark Image 2";
            // 
            // btn_rotate_images
            // 
            this.btn_rotate_images.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(166)))), ((int)(((byte)(137)))));
            this.btn_rotate_images.Enabled = false;
            this.btn_rotate_images.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(166)))), ((int)(((byte)(137)))));
            this.btn_rotate_images.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_rotate_images.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_rotate_images.Location = new System.Drawing.Point(128, 327);
            this.btn_rotate_images.Name = "btn_rotate_images";
            this.btn_rotate_images.Size = new System.Drawing.Size(179, 39);
            this.btn_rotate_images.TabIndex = 3;
            this.btn_rotate_images.Text = "Rotate Mark Images";
            this.btn_rotate_images.UseVisualStyleBackColor = false;
            this.btn_rotate_images.Click += new System.EventHandler(this.btn_rotate_images_Click);
            // 
            // btn_load_markimg_2
            // 
            this.btn_load_markimg_2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(166)))), ((int)(((byte)(137)))));
            this.btn_load_markimg_2.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(166)))), ((int)(((byte)(137)))));
            this.btn_load_markimg_2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_load_markimg_2.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_load_markimg_2.Location = new System.Drawing.Point(7, 327);
            this.btn_load_markimg_2.Name = "btn_load_markimg_2";
            this.btn_load_markimg_2.Size = new System.Drawing.Size(115, 39);
            this.btn_load_markimg_2.TabIndex = 2;
            this.btn_load_markimg_2.Text = "Load Image 2";
            this.btn_load_markimg_2.UseVisualStyleBackColor = false;
            this.btn_load_markimg_2.Click += new System.EventHandler(this.btn_load_markimg_2_Click);
            // 
            // hWindowControl_mark_2
            // 
            this.hWindowControl_mark_2.BackColor = System.Drawing.Color.Black;
            this.hWindowControl_mark_2.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl_mark_2.ImagePart = new System.Drawing.Rectangle(0, 0, 2448, 2048);
            this.hWindowControl_mark_2.Location = new System.Drawing.Point(7, 21);
            this.hWindowControl_mark_2.Name = "hWindowControl_mark_2";
            this.hWindowControl_mark_2.Size = new System.Drawing.Size(300, 300);
            this.hWindowControl_mark_2.TabIndex = 1;
            this.hWindowControl_mark_2.WindowSize = new System.Drawing.Size(300, 300);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btn_find_mark);
            this.groupBox2.Controls.Add(this.btn_load_markimg_1);
            this.groupBox2.Controls.Add(this.hWindowControl_mark_1);
            this.groupBox2.Controls.Add(this.btn_create_model);
            this.groupBox2.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox2.Location = new System.Drawing.Point(6, 208);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(323, 377);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Mark Image 1";
            // 
            // btn_find_mark
            // 
            this.btn_find_mark.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(166)))), ((int)(((byte)(137)))));
            this.btn_find_mark.Enabled = false;
            this.btn_find_mark.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(166)))), ((int)(((byte)(137)))));
            this.btn_find_mark.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_find_mark.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_find_mark.Location = new System.Drawing.Point(232, 327);
            this.btn_find_mark.Name = "btn_find_mark";
            this.btn_find_mark.Size = new System.Drawing.Size(81, 39);
            this.btn_find_mark.TabIndex = 5;
            this.btn_find_mark.Text = "Find Mark";
            this.btn_find_mark.UseVisualStyleBackColor = false;
            this.btn_find_mark.Click += new System.EventHandler(this.btn_find_mark_Click);
            // 
            // btn_load_markimg_1
            // 
            this.btn_load_markimg_1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(166)))), ((int)(((byte)(137)))));
            this.btn_load_markimg_1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(166)))), ((int)(((byte)(137)))));
            this.btn_load_markimg_1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_load_markimg_1.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_load_markimg_1.Location = new System.Drawing.Point(7, 327);
            this.btn_load_markimg_1.Name = "btn_load_markimg_1";
            this.btn_load_markimg_1.Size = new System.Drawing.Size(106, 39);
            this.btn_load_markimg_1.TabIndex = 1;
            this.btn_load_markimg_1.Text = "Load Image 1";
            this.btn_load_markimg_1.UseVisualStyleBackColor = false;
            this.btn_load_markimg_1.Click += new System.EventHandler(this.btn_load_markimg_1_Click);
            // 
            // hWindowControl_mark_1
            // 
            this.hWindowControl_mark_1.BackColor = System.Drawing.Color.Black;
            this.hWindowControl_mark_1.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl_mark_1.ImagePart = new System.Drawing.Rectangle(0, 0, 2448, 2048);
            this.hWindowControl_mark_1.Location = new System.Drawing.Point(7, 21);
            this.hWindowControl_mark_1.Name = "hWindowControl_mark_1";
            this.hWindowControl_mark_1.Size = new System.Drawing.Size(306, 300);
            this.hWindowControl_mark_1.TabIndex = 1;
            this.hWindowControl_mark_1.WindowSize = new System.Drawing.Size(306, 300);
            // 
            // btn_create_model
            // 
            this.btn_create_model.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(166)))), ((int)(((byte)(137)))));
            this.btn_create_model.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(166)))), ((int)(((byte)(137)))));
            this.btn_create_model.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_create_model.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_create_model.Location = new System.Drawing.Point(119, 327);
            this.btn_create_model.Name = "btn_create_model";
            this.btn_create_model.Size = new System.Drawing.Size(107, 39);
            this.btn_create_model.TabIndex = 4;
            this.btn_create_model.Text = "Create Model";
            this.btn_create_model.UseVisualStyleBackColor = false;
            this.btn_create_model.Click += new System.EventHandler(this.btn_create_model_Click);
            // 
            // txt_matching_score
            // 
            this.txt_matching_score.Location = new System.Drawing.Point(243, 166);
            this.txt_matching_score.Name = "txt_matching_score";
            this.txt_matching_score.Size = new System.Drawing.Size(158, 29);
            this.txt_matching_score.TabIndex = 11;
            this.txt_matching_score.Text = "0.7";
            this.txt_matching_score.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_matching_score.TextChanged += new System.EventHandler(this.txt_matching_score_TextChanged);
            // 
            // txt_motion_shift_dist
            // 
            this.txt_motion_shift_dist.Location = new System.Drawing.Point(243, 131);
            this.txt_motion_shift_dist.Name = "txt_motion_shift_dist";
            this.txt_motion_shift_dist.Size = new System.Drawing.Size(158, 29);
            this.txt_motion_shift_dist.TabIndex = 9;
            this.txt_motion_shift_dist.Text = "51.83";
            this.txt_motion_shift_dist.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_motion_shift_dist.TextChanged += new System.EventHandler(this.txt_motion_shift_dist_TextChanged);
            // 
            // txt_pixel_resolution
            // 
            this.txt_pixel_resolution.Location = new System.Drawing.Point(243, 93);
            this.txt_pixel_resolution.Name = "txt_pixel_resolution";
            this.txt_pixel_resolution.Size = new System.Drawing.Size(158, 29);
            this.txt_pixel_resolution.TabIndex = 7;
            this.txt_pixel_resolution.Text = "3.45";
            this.txt_pixel_resolution.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_pixel_resolution.TextChanged += new System.EventHandler(this.txt_pixel_resolution_TextChanged);
            // 
            // txt_save_path
            // 
            this.txt_save_path.Location = new System.Drawing.Point(243, 58);
            this.txt_save_path.Name = "txt_save_path";
            this.txt_save_path.Size = new System.Drawing.Size(158, 29);
            this.txt_save_path.TabIndex = 3;
            this.txt_save_path.Text = "temp\\";
            this.txt_save_path.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_save_path.TextChanged += new System.EventHandler(this.txt_save_path_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.Location = new System.Drawing.Point(17, 169);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(220, 20);
            this.label4.TabIndex = 10;
            this.label4.Text = "Minimum Matching Score：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.Location = new System.Drawing.Point(6, 134);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(231, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "Motion Shift Distance (mm)：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(53, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(183, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "Pixel Resolution (um)：";
            // 
            // btn_read_model
            // 
            this.btn_read_model.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(166)))), ((int)(((byte)(137)))));
            this.btn_read_model.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(166)))), ((int)(((byte)(137)))));
            this.btn_read_model.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_read_model.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_read_model.Location = new System.Drawing.Point(439, 11);
            this.btn_read_model.Name = "btn_read_model";
            this.btn_read_model.Size = new System.Drawing.Size(144, 32);
            this.btn_read_model.TabIndex = 5;
            this.btn_read_model.Text = "Read Model";
            this.btn_read_model.UseVisualStyleBackColor = false;
            this.btn_read_model.Click += new System.EventHandler(this.btn_read_model_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(141, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "SavePath：";
            // 
            // tabControl_matching
            // 
            this.tabControl_matching.Controls.Add(this.tabPage_matching);
            this.tabControl_matching.Location = new System.Drawing.Point(10, 17);
            this.tabControl_matching.Name = "tabControl_matching";
            this.tabControl_matching.SelectedIndex = 0;
            this.tabControl_matching.Size = new System.Drawing.Size(660, 626);
            this.tabControl_matching.TabIndex = 20;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.hWindowControl_preview);
            this.panel1.Controls.Add(this.btn_rotate);
            this.panel1.Controls.Add(this.btn_load_image);
            this.panel1.Location = new System.Drawing.Point(672, 46);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(613, 593);
            this.panel1.TabIndex = 21;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label6.Location = new System.Drawing.Point(668, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 20);
            this.label6.TabIndex = 15;
            this.label6.Text = "預覽影像";
            // 
            // UserControl_AffineTrans_2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.label6);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tabControl_matching);
            this.Font = new System.Drawing.Font("微軟正黑體 Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Name = "UserControl_AffineTrans_2";
            this.Size = new System.Drawing.Size(1300, 800);
            this.tabPage_matching.ResumeLayout(false);
            this.tabPage_matching.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tabControl_matching.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private HalconDotNet.HWindowControl hWindowControl_preview;
        private System.Windows.Forms.Button btn_load_image;
        private System.Windows.Forms.OpenFileDialog openFileDialog_file;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btn_rotate;
        private System.Windows.Forms.TabPage tabPage_matching;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btn_rotate_images;
        private System.Windows.Forms.Button btn_load_markimg_2;
        private HalconDotNet.HWindowControl hWindowControl_mark_2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_load_markimg_1;
        private HalconDotNet.HWindowControl hWindowControl_mark_1;
        private System.Windows.Forms.Button btn_create_model;
        private System.Windows.Forms.TextBox txt_matching_score;
        private System.Windows.Forms.TextBox txt_motion_shift_dist;
        private System.Windows.Forms.TextBox txt_pixel_resolution;
        private System.Windows.Forms.TextBox txt_save_path;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_read_model;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl_matching;
        private System.Windows.Forms.Button btn_find_mark;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lab_model_status;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label6;
    }
}
