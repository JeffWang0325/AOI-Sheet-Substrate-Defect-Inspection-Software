namespace DeltaSubstrateInspector.src.PositionMethod.LocateMethod.Rotation
{
    partial class AngleAffineUC
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
            this.openFileDialog_file = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.btn_save = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_rotate = new System.Windows.Forms.Button();
            this.hWindowControl_preview = new HalconDotNet.HWindowControl();
            this.btn_load_image = new System.Windows.Forms.Button();
            this.lab_model_status = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btn_rotate_images = new System.Windows.Forms.Button();
            this.btn_load_markig_2 = new System.Windows.Forms.Button();
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
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog_file
            // 
            this.openFileDialog_file.FileName = "openFileDialog1";
            // 
            // btn_save
            // 
            this.btn_save.Location = new System.Drawing.Point(332, 59);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(98, 39);
            this.btn_save.TabIndex = 20;
            this.btn_save.Text = "Save";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_rotate);
            this.groupBox1.Controls.Add(this.hWindowControl_preview);
            this.groupBox1.Controls.Add(this.btn_load_image);
            this.groupBox1.Location = new System.Drawing.Point(654, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(510, 570);
            this.groupBox1.TabIndex = 34;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Preview";
            // 
            // btn_rotate
            // 
            this.btn_rotate.Enabled = false;
            this.btn_rotate.Location = new System.Drawing.Point(110, 520);
            this.btn_rotate.Name = "btn_rotate";
            this.btn_rotate.Size = new System.Drawing.Size(98, 39);
            this.btn_rotate.TabIndex = 1;
            this.btn_rotate.Text = "Rotate";
            this.btn_rotate.UseVisualStyleBackColor = true;
            this.btn_rotate.Click += new System.EventHandler(this.btn_rotate_Click);
            // 
            // hWindowControl_preview
            // 
            this.hWindowControl_preview.BackColor = System.Drawing.Color.Black;
            this.hWindowControl_preview.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl_preview.ImagePart = new System.Drawing.Rectangle(0, 0, 2448, 2048);
            this.hWindowControl_preview.Location = new System.Drawing.Point(6, 21);
            this.hWindowControl_preview.Name = "hWindowControl_preview";
            this.hWindowControl_preview.Size = new System.Drawing.Size(498, 493);
            this.hWindowControl_preview.TabIndex = 0;
            this.hWindowControl_preview.WindowSize = new System.Drawing.Size(498, 493);
            // 
            // btn_load_image
            // 
            this.btn_load_image.Location = new System.Drawing.Point(6, 520);
            this.btn_load_image.Name = "btn_load_image";
            this.btn_load_image.Size = new System.Drawing.Size(98, 39);
            this.btn_load_image.TabIndex = 0;
            this.btn_load_image.Text = "Load Image";
            this.btn_load_image.UseVisualStyleBackColor = true;
            this.btn_load_image.Click += new System.EventHandler(this.btn_load_image_Click);
            // 
            // lab_model_status
            // 
            this.lab_model_status.BackColor = System.Drawing.Color.Black;
            this.lab_model_status.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lab_model_status.ForeColor = System.Drawing.Color.Red;
            this.lab_model_status.Location = new System.Drawing.Point(168, 15);
            this.lab_model_status.Name = "lab_model_status";
            this.lab_model_status.Size = new System.Drawing.Size(158, 36);
            this.lab_model_status.TabIndex = 33;
            this.lab_model_status.Text = "Offline";
            this.lab_model_status.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label5.Location = new System.Drawing.Point(57, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(105, 16);
            this.label5.TabIndex = 32;
            this.label5.Text = "Model Status：";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btn_rotate_images);
            this.groupBox3.Controls.Add(this.btn_load_markig_2);
            this.groupBox3.Controls.Add(this.hWindowControl_mark_2);
            this.groupBox3.Location = new System.Drawing.Point(332, 207);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(316, 377);
            this.groupBox3.TabIndex = 31;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Mark Image 2";
            // 
            // btn_rotate_images
            // 
            this.btn_rotate_images.Enabled = false;
            this.btn_rotate_images.Location = new System.Drawing.Point(127, 327);
            this.btn_rotate_images.Name = "btn_rotate_images";
            this.btn_rotate_images.Size = new System.Drawing.Size(180, 39);
            this.btn_rotate_images.TabIndex = 3;
            this.btn_rotate_images.Text = "Rotate Mark Images";
            this.btn_rotate_images.UseVisualStyleBackColor = true;
            this.btn_rotate_images.Click += new System.EventHandler(this.btn_rotate_mark_images_Click);
            // 
            // btn_load_markig_2
            // 
            this.btn_load_markig_2.Location = new System.Drawing.Point(7, 327);
            this.btn_load_markig_2.Name = "btn_load_markig_2";
            this.btn_load_markig_2.Size = new System.Drawing.Size(114, 39);
            this.btn_load_markig_2.TabIndex = 2;
            this.btn_load_markig_2.Text = "Load Mark Image 2";
            this.btn_load_markig_2.UseVisualStyleBackColor = true;
            this.btn_load_markig_2.Click += new System.EventHandler(this.btn_load_markimg_2_Click);
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
            this.groupBox2.Location = new System.Drawing.Point(10, 207);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(316, 377);
            this.groupBox2.TabIndex = 30;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Mark Image 1";
            // 
            // btn_find_mark
            // 
            this.btn_find_mark.Enabled = false;
            this.btn_find_mark.Location = new System.Drawing.Point(226, 327);
            this.btn_find_mark.Name = "btn_find_mark";
            this.btn_find_mark.Size = new System.Drawing.Size(81, 39);
            this.btn_find_mark.TabIndex = 5;
            this.btn_find_mark.Text = "Find Mark";
            this.btn_find_mark.UseVisualStyleBackColor = true;
            this.btn_find_mark.Click += new System.EventHandler(this.btn_find_mark_Click);
            // 
            // btn_load_markimg_1
            // 
            this.btn_load_markimg_1.Location = new System.Drawing.Point(7, 327);
            this.btn_load_markimg_1.Name = "btn_load_markimg_1";
            this.btn_load_markimg_1.Size = new System.Drawing.Size(114, 39);
            this.btn_load_markimg_1.TabIndex = 1;
            this.btn_load_markimg_1.Text = "Load Mark Image 1";
            this.btn_load_markimg_1.UseVisualStyleBackColor = true;
            this.btn_load_markimg_1.Click += new System.EventHandler(this.btn_load_markimg_1_Click);
            // 
            // hWindowControl_mark_1
            // 
            this.hWindowControl_mark_1.BackColor = System.Drawing.Color.Black;
            this.hWindowControl_mark_1.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl_mark_1.ImagePart = new System.Drawing.Rectangle(0, 0, 2448, 2048);
            this.hWindowControl_mark_1.Location = new System.Drawing.Point(7, 21);
            this.hWindowControl_mark_1.Name = "hWindowControl_mark_1";
            this.hWindowControl_mark_1.Size = new System.Drawing.Size(300, 300);
            this.hWindowControl_mark_1.TabIndex = 1;
            this.hWindowControl_mark_1.WindowSize = new System.Drawing.Size(300, 300);
            // 
            // btn_create_model
            // 
            this.btn_create_model.Location = new System.Drawing.Point(133, 327);
            this.btn_create_model.Name = "btn_create_model";
            this.btn_create_model.Size = new System.Drawing.Size(81, 39);
            this.btn_create_model.TabIndex = 4;
            this.btn_create_model.Text = "Create Model";
            this.btn_create_model.UseVisualStyleBackColor = true;
            this.btn_create_model.Click += new System.EventHandler(this.btn_create_model_Click);
            // 
            // txt_matching_score
            // 
            this.txt_matching_score.Location = new System.Drawing.Point(168, 172);
            this.txt_matching_score.Name = "txt_matching_score";
            this.txt_matching_score.Size = new System.Drawing.Size(158, 22);
            this.txt_matching_score.TabIndex = 29;
            this.txt_matching_score.Text = "0.7";
            this.txt_matching_score.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_matching_score.TextChanged += new System.EventHandler(this.txt_matching_score_TextChanged);
            // 
            // txt_motion_shift_dist
            // 
            this.txt_motion_shift_dist.Location = new System.Drawing.Point(168, 136);
            this.txt_motion_shift_dist.Name = "txt_motion_shift_dist";
            this.txt_motion_shift_dist.Size = new System.Drawing.Size(158, 22);
            this.txt_motion_shift_dist.TabIndex = 27;
            this.txt_motion_shift_dist.Text = "51.83";
            this.txt_motion_shift_dist.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_motion_shift_dist.TextChanged += new System.EventHandler(this.txt_motion_shift_dist_TextChanged);
            // 
            // txt_pixel_resolution
            // 
            this.txt_pixel_resolution.Location = new System.Drawing.Point(168, 100);
            this.txt_pixel_resolution.Name = "txt_pixel_resolution";
            this.txt_pixel_resolution.Size = new System.Drawing.Size(158, 22);
            this.txt_pixel_resolution.TabIndex = 25;
            this.txt_pixel_resolution.Text = "3.45";
            this.txt_pixel_resolution.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_pixel_resolution.TextChanged += new System.EventHandler(this.txt_pixel_resolution_TextChanged);
            // 
            // txt_save_path
            // 
            this.txt_save_path.Location = new System.Drawing.Point(168, 64);
            this.txt_save_path.Name = "txt_save_path";
            this.txt_save_path.Size = new System.Drawing.Size(158, 22);
            this.txt_save_path.TabIndex = 22;
            this.txt_save_path.Text = "temp\\";
            this.txt_save_path.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_save_path.TextChanged += new System.EventHandler(this.txt_save_path_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.Location = new System.Drawing.Point(23, 175);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(139, 12);
            this.label4.TabIndex = 28;
            this.label4.Text = "Minimum Matching Score：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.Location = new System.Drawing.Point(15, 139);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(147, 12);
            this.label3.TabIndex = 26;
            this.label3.Text = "Motion Shift Distance (mm)：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(43, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 12);
            this.label2.TabIndex = 24;
            this.label2.Text = "Pixel Resolution (um)：";
            // 
            // btn_read_model
            // 
            this.btn_read_model.Location = new System.Drawing.Point(332, 14);
            this.btn_read_model.Name = "btn_read_model";
            this.btn_read_model.Size = new System.Drawing.Size(98, 39);
            this.btn_read_model.TabIndex = 23;
            this.btn_read_model.Text = "Read Model";
            this.btn_read_model.UseVisualStyleBackColor = true;
            this.btn_read_model.Click += new System.EventHandler(this.btn_read_model_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(103, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 21;
            this.label1.Text = "SavePath：";
            // 
            // AngleAffineUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lab_model_status);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.txt_matching_score);
            this.Controls.Add(this.txt_motion_shift_dist);
            this.Controls.Add(this.txt_pixel_resolution);
            this.Controls.Add(this.txt_save_path);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn_read_model);
            this.Controls.Add(this.label1);
            this.Name = "AngleAffineUC";
            this.Size = new System.Drawing.Size(1174, 598);
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFileDialog_file;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_rotate;
        private HalconDotNet.HWindowControl hWindowControl_preview;
        private System.Windows.Forms.Button btn_load_image;
        private System.Windows.Forms.Label lab_model_status;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btn_rotate_images;
        private System.Windows.Forms.Button btn_load_markig_2;
        private HalconDotNet.HWindowControl hWindowControl_mark_2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_find_mark;
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
    }
}
