namespace DeltaSubstrateInspector.src.Preference.UC
{
    partial class MotionControl
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.hWindowControl_Cam = new HalconDotNet.HWindowControl();
            this.btn_up = new System.Windows.Forms.Button();
            this.btn_down = new System.Windows.Forms.Button();
            this.btn_left = new System.Windows.Forms.Button();
            this.btn_right = new System.Windows.Forms.Button();
            this.label_v = new System.Windows.Forms.Label();
            this.label_h = new System.Windows.Forms.Label();
            this.tabControl_motion = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btn_z_negitive = new System.Windows.Forms.Button();
            this.btn_z_positive = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_p2p_step_z = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_p2p_step_y = new System.Windows.Forms.TextBox();
            this.btn_p2p_run = new System.Windows.Forms.Button();
            this.label43 = new System.Windows.Forms.Label();
            this.label44 = new System.Windows.Forms.Label();
            this.txt_p2p_step_x = new System.Windows.Forms.TextBox();
            this.label39 = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.txt_p2p_vel = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.btn_array_add = new System.Windows.Forms.Button();
            this.btn_array_run = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.txt_array_vel = new System.Windows.Forms.TextBox();
            this.checkBox_N = new System.Windows.Forms.CheckBox();
            this.checkBox_Z = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txt_y_count = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txt_x_count = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txt_y_shift = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txt_x_shift = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txt_start_index = new System.Windows.Forms.TextBox();
            this.dataGridView_step = new System.Windows.Forms.DataGridView();
            this.Column_Index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_X = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Y = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Z = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Velocity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label_x = new System.Windows.Forms.Label();
            this.txt_cp_x = new System.Windows.Forms.TextBox();
            this.txt_cp_y = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_cp_z = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_add = new System.Windows.Forms.Button();
            this.label_title = new System.Windows.Forms.Label();
            this.btn_set = new System.Windows.Forms.Button();
            this.backgroundWorker_motion = new System.ComponentModel.BackgroundWorker();
            this.btn_stop = new System.Windows.Forms.Button();
            this.tabControl_motion.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_step)).BeginInit();
            this.SuspendLayout();
            // 
            // hWindowControl_Cam
            // 
            this.hWindowControl_Cam.BackColor = System.Drawing.Color.Black;
            this.hWindowControl_Cam.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl_Cam.ImagePart = new System.Drawing.Rectangle(0, 0, 4096, 2160);
            this.hWindowControl_Cam.Location = new System.Drawing.Point(3, 3);
            this.hWindowControl_Cam.Name = "hWindowControl_Cam";
            this.hWindowControl_Cam.Size = new System.Drawing.Size(398, 383);
            this.hWindowControl_Cam.TabIndex = 0;
            this.hWindowControl_Cam.WindowSize = new System.Drawing.Size(398, 383);
            // 
            // btn_up
            // 
            this.btn_up.Location = new System.Drawing.Point(75, 6);
            this.btn_up.Name = "btn_up";
            this.btn_up.Size = new System.Drawing.Size(60, 60);
            this.btn_up.TabIndex = 1;
            this.btn_up.Text = "Y +";
            this.btn_up.UseVisualStyleBackColor = true;
            this.btn_up.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_up_MouseDown);
            this.btn_up.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_up_MouseUp);
            // 
            // btn_down
            // 
            this.btn_down.Location = new System.Drawing.Point(75, 72);
            this.btn_down.Name = "btn_down";
            this.btn_down.Size = new System.Drawing.Size(60, 60);
            this.btn_down.TabIndex = 2;
            this.btn_down.Text = "Y -";
            this.btn_down.UseVisualStyleBackColor = true;
            this.btn_down.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_down_MouseDown);
            this.btn_down.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_down_MouseUp);
            // 
            // btn_left
            // 
            this.btn_left.Location = new System.Drawing.Point(9, 41);
            this.btn_left.Name = "btn_left";
            this.btn_left.Size = new System.Drawing.Size(60, 60);
            this.btn_left.TabIndex = 3;
            this.btn_left.Text = "X -";
            this.btn_left.UseVisualStyleBackColor = true;
            this.btn_left.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_left_MouseDown);
            this.btn_left.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_left_MouseUp);
            // 
            // btn_right
            // 
            this.btn_right.Location = new System.Drawing.Point(141, 41);
            this.btn_right.Name = "btn_right";
            this.btn_right.Size = new System.Drawing.Size(60, 60);
            this.btn_right.TabIndex = 4;
            this.btn_right.Text = "X +";
            this.btn_right.UseVisualStyleBackColor = true;
            this.btn_right.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_right_MouseDown);
            this.btn_right.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_right_MouseUp);
            // 
            // label_v
            // 
            this.label_v.BackColor = System.Drawing.Color.Red;
            this.label_v.Location = new System.Drawing.Point(193, 3);
            this.label_v.Name = "label_v";
            this.label_v.Size = new System.Drawing.Size(1, 383);
            this.label_v.TabIndex = 5;
            // 
            // label_h
            // 
            this.label_h.BackColor = System.Drawing.Color.Red;
            this.label_h.Location = new System.Drawing.Point(3, 182);
            this.label_h.Name = "label_h";
            this.label_h.Size = new System.Drawing.Size(396, 1);
            this.label_h.TabIndex = 6;
            // 
            // tabControl_motion
            // 
            this.tabControl_motion.Controls.Add(this.tabPage1);
            this.tabControl_motion.Controls.Add(this.tabPage2);
            this.tabControl_motion.Controls.Add(this.tabPage3);
            this.tabControl_motion.Location = new System.Drawing.Point(407, 3);
            this.tabControl_motion.Name = "tabControl_motion";
            this.tabControl_motion.SelectedIndex = 0;
            this.tabControl_motion.Size = new System.Drawing.Size(352, 165);
            this.tabControl_motion.TabIndex = 7;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btn_z_negitive);
            this.tabPage1.Controls.Add(this.btn_z_positive);
            this.tabPage1.Controls.Add(this.btn_up);
            this.tabPage1.Controls.Add(this.btn_down);
            this.tabPage1.Controls.Add(this.btn_left);
            this.tabPage1.Controls.Add(this.btn_right);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(344, 139);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Jog";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btn_z_negitive
            // 
            this.btn_z_negitive.Location = new System.Drawing.Point(207, 72);
            this.btn_z_negitive.Name = "btn_z_negitive";
            this.btn_z_negitive.Size = new System.Drawing.Size(131, 60);
            this.btn_z_negitive.TabIndex = 6;
            this.btn_z_negitive.Text = "Z -";
            this.btn_z_negitive.UseVisualStyleBackColor = true;
            this.btn_z_negitive.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_z_negitive_MouseDown);
            this.btn_z_negitive.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_z_negitive_MouseUp);
            // 
            // btn_z_positive
            // 
            this.btn_z_positive.Location = new System.Drawing.Point(207, 6);
            this.btn_z_positive.Name = "btn_z_positive";
            this.btn_z_positive.Size = new System.Drawing.Size(131, 60);
            this.btn_z_positive.TabIndex = 5;
            this.btn_z_positive.Text = "Z +";
            this.btn_z_positive.UseVisualStyleBackColor = true;
            this.btn_z_positive.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_z_positive_MouseDown);
            this.btn_z_positive.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_z_positive_MouseUp);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.txt_p2p_step_z);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.txt_p2p_step_y);
            this.tabPage2.Controls.Add(this.btn_p2p_run);
            this.tabPage2.Controls.Add(this.label43);
            this.tabPage2.Controls.Add(this.label44);
            this.tabPage2.Controls.Add(this.txt_p2p_step_x);
            this.tabPage2.Controls.Add(this.label39);
            this.tabPage2.Controls.Add(this.label42);
            this.tabPage2.Controls.Add(this.txt_p2p_vel);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(344, 139);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "P2P";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(195, 101);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 62;
            this.label5.Text = "pusle";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(23, 101);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 12);
            this.label6.TabIndex = 61;
            this.label6.Text = "Z Step：";
            // 
            // txt_p2p_step_z
            // 
            this.txt_p2p_step_z.Location = new System.Drawing.Point(84, 98);
            this.txt_p2p_step_z.Name = "txt_p2p_step_z";
            this.txt_p2p_step_z.Size = new System.Drawing.Size(100, 22);
            this.txt_p2p_step_z.TabIndex = 60;
            this.txt_p2p_step_z.Text = "100";
            this.txt_p2p_step_z.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(195, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 59;
            this.label3.Text = "pusle";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 12);
            this.label4.TabIndex = 58;
            this.label4.Text = "Y Step：";
            // 
            // txt_p2p_step_y
            // 
            this.txt_p2p_step_y.Location = new System.Drawing.Point(84, 70);
            this.txt_p2p_step_y.Name = "txt_p2p_step_y";
            this.txt_p2p_step_y.Size = new System.Drawing.Size(100, 22);
            this.txt_p2p_step_y.TabIndex = 57;
            this.txt_p2p_step_y.Text = "331.36";
            this.txt_p2p_step_y.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btn_p2p_run
            // 
            this.btn_p2p_run.Location = new System.Drawing.Point(246, 6);
            this.btn_p2p_run.Name = "btn_p2p_run";
            this.btn_p2p_run.Size = new System.Drawing.Size(92, 127);
            this.btn_p2p_run.TabIndex = 56;
            this.btn_p2p_run.Text = "Run";
            this.btn_p2p_run.UseVisualStyleBackColor = true;
            this.btn_p2p_run.Click += new System.EventHandler(this.btn_p2p_run_Click);
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(195, 45);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(29, 12);
            this.label43.TabIndex = 55;
            this.label43.Text = "pusle";
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Location = new System.Drawing.Point(22, 45);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(48, 12);
            this.label44.TabIndex = 54;
            this.label44.Text = "X Step：";
            // 
            // txt_p2p_step_x
            // 
            this.txt_p2p_step_x.Location = new System.Drawing.Point(84, 42);
            this.txt_p2p_step_x.Name = "txt_p2p_step_x";
            this.txt_p2p_step_x.Size = new System.Drawing.Size(100, 22);
            this.txt_p2p_step_x.TabIndex = 53;
            this.txt_p2p_step_x.Text = "251.63";
            this.txt_p2p_step_x.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(195, 17);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(45, 12);
            this.label39.TabIndex = 50;
            this.label39.Text = "pusle/ms";
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(13, 17);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(56, 12);
            this.label42.TabIndex = 44;
            this.label42.Text = "Velocity：";
            // 
            // txt_p2p_vel
            // 
            this.txt_p2p_vel.Location = new System.Drawing.Point(84, 14);
            this.txt_p2p_vel.Name = "txt_p2p_vel";
            this.txt_p2p_vel.Size = new System.Drawing.Size(100, 22);
            this.txt_p2p_vel.TabIndex = 45;
            this.txt_p2p_vel.Text = "50";
            this.txt_p2p_vel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.btn_array_add);
            this.tabPage3.Controls.Add(this.btn_array_run);
            this.tabPage3.Controls.Add(this.label12);
            this.tabPage3.Controls.Add(this.label13);
            this.tabPage3.Controls.Add(this.txt_array_vel);
            this.tabPage3.Controls.Add(this.checkBox_N);
            this.tabPage3.Controls.Add(this.checkBox_Z);
            this.tabPage3.Controls.Add(this.label10);
            this.tabPage3.Controls.Add(this.txt_y_count);
            this.tabPage3.Controls.Add(this.label11);
            this.tabPage3.Controls.Add(this.txt_x_count);
            this.tabPage3.Controls.Add(this.label9);
            this.tabPage3.Controls.Add(this.txt_y_shift);
            this.tabPage3.Controls.Add(this.label8);
            this.tabPage3.Controls.Add(this.txt_x_shift);
            this.tabPage3.Controls.Add(this.label7);
            this.tabPage3.Controls.Add(this.txt_start_index);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(344, 139);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Array";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // btn_array_add
            // 
            this.btn_array_add.Location = new System.Drawing.Point(75, 91);
            this.btn_array_add.Name = "btn_array_add";
            this.btn_array_add.Size = new System.Drawing.Size(120, 42);
            this.btn_array_add.TabIndex = 62;
            this.btn_array_add.Text = "Add";
            this.btn_array_add.UseVisualStyleBackColor = true;
            this.btn_array_add.Click += new System.EventHandler(this.btn_array_add_Click);
            // 
            // btn_array_run
            // 
            this.btn_array_run.Location = new System.Drawing.Point(208, 91);
            this.btn_array_run.Name = "btn_array_run";
            this.btn_array_run.Size = new System.Drawing.Size(120, 42);
            this.btn_array_run.TabIndex = 61;
            this.btn_array_run.Text = "Run";
            this.btn_array_run.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(283, 10);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(45, 12);
            this.label12.TabIndex = 60;
            this.label12.Text = "pusle/ms";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(123, 10);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(56, 12);
            this.label13.TabIndex = 58;
            this.label13.Text = "Velocity：";
            // 
            // txt_array_vel
            // 
            this.txt_array_vel.Location = new System.Drawing.Point(185, 7);
            this.txt_array_vel.Name = "txt_array_vel";
            this.txt_array_vel.Size = new System.Drawing.Size(90, 22);
            this.txt_array_vel.TabIndex = 59;
            this.txt_array_vel.Text = "50";
            this.txt_array_vel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // checkBox_N
            // 
            this.checkBox_N.AutoSize = true;
            this.checkBox_N.Checked = true;
            this.checkBox_N.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_N.Location = new System.Drawing.Point(8, 91);
            this.checkBox_N.Name = "checkBox_N";
            this.checkBox_N.Size = new System.Drawing.Size(62, 16);
            this.checkBox_N.TabIndex = 57;
            this.checkBox_N.Text = "N Mode";
            this.checkBox_N.UseVisualStyleBackColor = true;
            this.checkBox_N.CheckStateChanged += new System.EventHandler(this.checkBox_N_CheckStateChanged);
            // 
            // checkBox_Z
            // 
            this.checkBox_Z.AutoSize = true;
            this.checkBox_Z.Location = new System.Drawing.Point(8, 113);
            this.checkBox_Z.Name = "checkBox_Z";
            this.checkBox_Z.Size = new System.Drawing.Size(61, 16);
            this.checkBox_Z.TabIndex = 56;
            this.checkBox_Z.Text = "Z Mode";
            this.checkBox_Z.UseVisualStyleBackColor = true;
            this.checkBox_Z.CheckStateChanged += new System.EventHandler(this.checkBox_Z_CheckStateChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(172, 66);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(57, 12);
            this.label10.TabIndex = 54;
            this.label10.Text = "Y Count：";
            // 
            // txt_y_count
            // 
            this.txt_y_count.Location = new System.Drawing.Point(237, 63);
            this.txt_y_count.Name = "txt_y_count";
            this.txt_y_count.Size = new System.Drawing.Size(90, 22);
            this.txt_y_count.TabIndex = 55;
            this.txt_y_count.Text = "0";
            this.txt_y_count.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(172, 38);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(57, 12);
            this.label11.TabIndex = 52;
            this.label11.Text = "X Count：";
            // 
            // txt_x_count
            // 
            this.txt_x_count.Location = new System.Drawing.Point(237, 35);
            this.txt_x_count.Name = "txt_x_count";
            this.txt_x_count.Size = new System.Drawing.Size(90, 22);
            this.txt_x_count.TabIndex = 53;
            this.txt_x_count.Text = "0";
            this.txt_x_count.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(20, 66);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(50, 12);
            this.label9.TabIndex = 50;
            this.label9.Text = "Y Shift：";
            // 
            // txt_y_shift
            // 
            this.txt_y_shift.Location = new System.Drawing.Point(76, 63);
            this.txt_y_shift.Name = "txt_y_shift";
            this.txt_y_shift.Size = new System.Drawing.Size(90, 22);
            this.txt_y_shift.TabIndex = 51;
            this.txt_y_shift.Text = "0";
            this.txt_y_shift.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(20, 38);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(50, 12);
            this.label8.TabIndex = 48;
            this.label8.Text = "X Shift：";
            // 
            // txt_x_shift
            // 
            this.txt_x_shift.Location = new System.Drawing.Point(76, 35);
            this.txt_x_shift.Name = "txt_x_shift";
            this.txt_x_shift.Size = new System.Drawing.Size(90, 22);
            this.txt_x_shift.TabIndex = 49;
            this.txt_x_shift.Text = "0";
            this.txt_x_shift.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(2, 10);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(68, 12);
            this.label7.TabIndex = 46;
            this.label7.Text = "Start Index：";
            // 
            // txt_start_index
            // 
            this.txt_start_index.Location = new System.Drawing.Point(76, 7);
            this.txt_start_index.Name = "txt_start_index";
            this.txt_start_index.Size = new System.Drawing.Size(35, 22);
            this.txt_start_index.TabIndex = 47;
            this.txt_start_index.Text = "0";
            this.txt_start_index.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // dataGridView_step
            // 
            this.dataGridView_step.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView_step.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_step.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridView_step.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_step.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_Index,
            this.Column_X,
            this.Column_Y,
            this.Column_Z,
            this.Column_Velocity});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView_step.DefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridView_step.Location = new System.Drawing.Point(407, 267);
            this.dataGridView_step.Name = "dataGridView_step";
            this.dataGridView_step.RowTemplate.Height = 24;
            this.dataGridView_step.Size = new System.Drawing.Size(352, 119);
            this.dataGridView_step.TabIndex = 8;
            this.dataGridView_step.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_step_CellClick);
            this.dataGridView_step.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dataGridView_step_MouseClick);
            // 
            // Column_Index
            // 
            this.Column_Index.HeaderText = "Index";
            this.Column_Index.Name = "Column_Index";
            // 
            // Column_X
            // 
            this.Column_X.HeaderText = "X";
            this.Column_X.Name = "Column_X";
            // 
            // Column_Y
            // 
            this.Column_Y.HeaderText = "Y";
            this.Column_Y.Name = "Column_Y";
            // 
            // Column_Z
            // 
            this.Column_Z.HeaderText = "Z";
            this.Column_Z.Name = "Column_Z";
            // 
            // Column_Velocity
            // 
            this.Column_Velocity.HeaderText = "Velocity";
            this.Column_Velocity.Name = "Column_Velocity";
            // 
            // label_x
            // 
            this.label_x.AutoSize = true;
            this.label_x.Location = new System.Drawing.Point(411, 175);
            this.label_x.Name = "label_x";
            this.label_x.Size = new System.Drawing.Size(22, 12);
            this.label_x.TabIndex = 9;
            this.label_x.Text = "X =";
            // 
            // txt_cp_x
            // 
            this.txt_cp_x.Location = new System.Drawing.Point(437, 170);
            this.txt_cp_x.Name = "txt_cp_x";
            this.txt_cp_x.Size = new System.Drawing.Size(57, 22);
            this.txt_cp_x.TabIndex = 10;
            this.txt_cp_x.Text = "0";
            this.txt_cp_x.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txt_cp_y
            // 
            this.txt_cp_y.Location = new System.Drawing.Point(525, 170);
            this.txt_cp_y.Name = "txt_cp_y";
            this.txt_cp_y.Size = new System.Drawing.Size(57, 22);
            this.txt_cp_y.TabIndex = 12;
            this.txt_cp_y.Text = "0";
            this.txt_cp_y.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(500, 175);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 12);
            this.label1.TabIndex = 11;
            this.label1.Text = "Y=";
            // 
            // txt_cp_z
            // 
            this.txt_cp_z.Location = new System.Drawing.Point(621, 170);
            this.txt_cp_z.Name = "txt_cp_z";
            this.txt_cp_z.Size = new System.Drawing.Size(57, 22);
            this.txt_cp_z.TabIndex = 14;
            this.txt_cp_z.Text = "0";
            this.txt_cp_z.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(594, 175);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 12);
            this.label2.TabIndex = 13;
            this.label2.Text = "Z =";
            // 
            // btn_add
            // 
            this.btn_add.Location = new System.Drawing.Point(413, 198);
            this.btn_add.Name = "btn_add";
            this.btn_add.Size = new System.Drawing.Size(110, 34);
            this.btn_add.TabIndex = 15;
            this.btn_add.Text = "Current Pos. Add";
            this.btn_add.UseVisualStyleBackColor = true;
            this.btn_add.Click += new System.EventHandler(this.btn_add_Click);
            // 
            // label_title
            // 
            this.label_title.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(176)))), ((int)(((byte)(242)))));
            this.label_title.Font = new System.Drawing.Font("新細明體", 12F);
            this.label_title.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label_title.Location = new System.Drawing.Point(407, 235);
            this.label_title.Name = "label_title";
            this.label_title.Size = new System.Drawing.Size(352, 29);
            this.label_title.TabIndex = 0;
            this.label_title.Text = "位置紀錄 (x,y,z)";
            this.label_title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_set
            // 
            this.btn_set.Location = new System.Drawing.Point(529, 198);
            this.btn_set.Name = "btn_set";
            this.btn_set.Size = new System.Drawing.Size(149, 34);
            this.btn_set.TabIndex = 16;
            this.btn_set.Text = "Setting";
            this.btn_set.UseVisualStyleBackColor = true;
            this.btn_set.Click += new System.EventHandler(this.btn_set_Click);
            // 
            // backgroundWorker_motion
            // 
            this.backgroundWorker_motion.WorkerReportsProgress = true;
            this.backgroundWorker_motion.WorkerSupportsCancellation = true;
            this.backgroundWorker_motion.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_motion_DoWork);
            this.backgroundWorker_motion.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_motion_ProgressChanged);
            // 
            // btn_stop
            // 
            this.btn_stop.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_stop.ForeColor = System.Drawing.Color.Red;
            this.btn_stop.Location = new System.Drawing.Point(684, 170);
            this.btn_stop.Name = "btn_stop";
            this.btn_stop.Size = new System.Drawing.Size(75, 62);
            this.btn_stop.TabIndex = 17;
            this.btn_stop.Text = "Stop";
            this.btn_stop.UseVisualStyleBackColor = true;
            this.btn_stop.Click += new System.EventHandler(this.btn_stop_Click);
            // 
            // MotionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btn_stop);
            this.Controls.Add(this.btn_set);
            this.Controls.Add(this.label_title);
            this.Controls.Add(this.btn_add);
            this.Controls.Add(this.txt_cp_z);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txt_cp_y);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_cp_x);
            this.Controls.Add(this.label_x);
            this.Controls.Add(this.dataGridView_step);
            this.Controls.Add(this.tabControl_motion);
            this.Controls.Add(this.label_h);
            this.Controls.Add(this.label_v);
            this.Controls.Add(this.hWindowControl_Cam);
            this.Name = "MotionControl";
            this.Size = new System.Drawing.Size(762, 389);
            this.tabControl_motion.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_step)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btn_up;
        private System.Windows.Forms.Button btn_down;
        private System.Windows.Forms.Button btn_left;
        private System.Windows.Forms.Button btn_right;
        private System.Windows.Forms.Label label_v;
        private System.Windows.Forms.Label label_h;
        private System.Windows.Forms.TabControl tabControl_motion;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dataGridView_step;
        private System.Windows.Forms.Label label_x;
        private System.Windows.Forms.TextBox txt_cp_x;
        private System.Windows.Forms.TextBox txt_cp_y;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_cp_z;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_add;
        private System.Windows.Forms.Label label_title;
        public HalconDotNet.HWindowControl hWindowControl_Cam;
        private System.Windows.Forms.Button btn_set;
        private System.Windows.Forms.Button btn_p2p_run;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.TextBox txt_p2p_step_x;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.TextBox txt_p2p_vel;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Index;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_X;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Y;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Z;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Velocity;
        private System.ComponentModel.BackgroundWorker backgroundWorker_motion;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txt_p2p_step_z;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_p2p_step_y;
        private System.Windows.Forms.Button btn_z_negitive;
        private System.Windows.Forms.Button btn_z_positive;
        private System.Windows.Forms.Button btn_stop;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txt_y_count;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txt_x_count;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txt_y_shift;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txt_x_shift;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txt_start_index;
        private System.Windows.Forms.CheckBox checkBox_N;
        private System.Windows.Forms.CheckBox checkBox_Z;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txt_array_vel;
        private System.Windows.Forms.Button btn_array_run;
        private System.Windows.Forms.Button btn_array_add;
    }
}
