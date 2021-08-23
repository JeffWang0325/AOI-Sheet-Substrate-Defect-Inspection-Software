namespace DeltaSubstrateInspector.src.Modules.NewInspectModule.HTResistor0201
{
    partial class HTResistor0201UC
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
            this.button_Add = new System.Windows.Forms.Button();
            this.tabControlHTResistor = new System.Windows.Forms.TabControl();
            this.tabBrightBlobBL = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtboxThresholdOffset_BrightBlobBL = new System.Windows.Forms.TextBox();
            this.txtboxAreaUpper_BrightBlobBL = new System.Windows.Forms.TextBox();
            this.txtboxAreaLower_BrightBlobBL = new System.Windows.Forms.TextBox();
            this.btnBrightBlobBL = new System.Windows.Forms.Button();
            this.tabBlackCrackFL = new System.Windows.Forms.TabPage();
            this.chkboxVertClose_BlackCrackFL = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtboxCrackHeight_BlackCrackFL = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtboxAreaLower_BlackCrackFL = new System.Windows.Forms.TextBox();
            this.txtboxAreaUpper_BlackCrackFL = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtboxThresholdScale_BlackCrackFL = new System.Windows.Forms.TextBox();
            this.btnBlackCrackFL = new System.Windows.Forms.Button();
            this.tabWhiteCrackFL = new System.Windows.Forms.TabPage();
            this.chkboxVertClose_WhiteCrackFL = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtboxCrackWidth_WhiteCrackFL = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtboxAreaLower_WhiteCrackFL = new System.Windows.Forms.TextBox();
            this.txtboxAreaUpper_WhiteCrackFL = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtboxThresholdScale_WhiteCrackFL = new System.Windows.Forms.TextBox();
            this.btnWhiteCrackFL = new System.Windows.Forms.Button();
            this.tabWhiteBlobFL = new System.Windows.Forms.TabPage();
            this.chkboxVertClose_WhiteBlobFL = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtboxBlobHeight_WhiteBlobFL = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtboxAreaLower_WhiteBlobFL = new System.Windows.Forms.TextBox();
            this.txtboxAreaUpper_WhiteBlobFL = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtboxThresholdScale_WhiteBlobFL = new System.Windows.Forms.TextBox();
            this.btnWhiteBlobFL = new System.Windows.Forms.Button();
            this.tabBlackBlobCL = new System.Windows.Forms.TabPage();
            this.label15 = new System.Windows.Forms.Label();
            this.txtboxMaskSize_BlackBlobCL = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtboxAreaLower_BlackBlobCL = new System.Windows.Forms.TextBox();
            this.txtboxAreaUpper_BlackBlobCL = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtboxThresholdScale_BlackBlobCL = new System.Windows.Forms.TextBox();
            this.btnBlackBlobCL = new System.Windows.Forms.Button();
            this.hSmartWindowControl1 = new HalconDotNet.HSmartWindowControl();
            this.btnLoadImage = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.labFileName = new System.Windows.Forms.Label();
            this.btnLoadParameters = new System.Windows.Forms.Button();
            this.btnSaveParameters = new System.Windows.Forms.Button();
            this.chkboxCombinedTest = new System.Windows.Forms.CheckBox();
            this.tabControlHTResistor.SuspendLayout();
            this.tabBrightBlobBL.SuspendLayout();
            this.tabBlackCrackFL.SuspendLayout();
            this.tabWhiteCrackFL.SuspendLayout();
            this.tabWhiteBlobFL.SuspendLayout();
            this.tabBlackBlobCL.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_Add
            // 
            this.button_Add.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(166)))), ((int)(((byte)(137)))));
            this.button_Add.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button_Add.Location = new System.Drawing.Point(1045, 743);
            this.button_Add.Name = "button_Add";
            this.button_Add.Size = new System.Drawing.Size(98, 40);
            this.button_Add.TabIndex = 0;
            this.button_Add.Text = "新增";
            this.button_Add.UseVisualStyleBackColor = false;
            this.button_Add.Click += new System.EventHandler(this.button_Add_Click);
            // 
            // tabControlHTResistor
            // 
            this.tabControlHTResistor.Controls.Add(this.tabBrightBlobBL);
            this.tabControlHTResistor.Controls.Add(this.tabBlackCrackFL);
            this.tabControlHTResistor.Controls.Add(this.tabWhiteCrackFL);
            this.tabControlHTResistor.Controls.Add(this.tabWhiteBlobFL);
            this.tabControlHTResistor.Controls.Add(this.tabBlackBlobCL);
            this.tabControlHTResistor.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tabControlHTResistor.Location = new System.Drawing.Point(3, 3);
            this.tabControlHTResistor.Name = "tabControlHTResistor";
            this.tabControlHTResistor.SelectedIndex = 0;
            this.tabControlHTResistor.Size = new System.Drawing.Size(638, 100);
            this.tabControlHTResistor.TabIndex = 1;
            // 
            // tabBrightBlobBL
            // 
            this.tabBrightBlobBL.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tabBrightBlobBL.Controls.Add(this.label3);
            this.tabBrightBlobBL.Controls.Add(this.label2);
            this.tabBrightBlobBL.Controls.Add(this.txtboxThresholdOffset_BrightBlobBL);
            this.tabBrightBlobBL.Controls.Add(this.txtboxAreaUpper_BrightBlobBL);
            this.tabBrightBlobBL.Controls.Add(this.txtboxAreaLower_BrightBlobBL);
            this.tabBrightBlobBL.Controls.Add(this.btnBrightBlobBL);
            this.tabBrightBlobBL.Location = new System.Drawing.Point(4, 26);
            this.tabBrightBlobBL.Name = "tabBrightBlobBL";
            this.tabBrightBlobBL.Padding = new System.Windows.Forms.Padding(3);
            this.tabBrightBlobBL.Size = new System.Drawing.Size(630, 70);
            this.tabBrightBlobBL.TabIndex = 0;
            this.tabBrightBlobBL.Text = "亮點(背光)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(6, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(162, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "閾值偏移量 (-255 ~ 255): ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "面積 (像素):";
            // 
            // txtboxThresholdOffset_BrightBlobBL
            // 
            this.txtboxThresholdOffset_BrightBlobBL.Location = new System.Drawing.Point(174, 4);
            this.txtboxThresholdOffset_BrightBlobBL.Name = "txtboxThresholdOffset_BrightBlobBL";
            this.txtboxThresholdOffset_BrightBlobBL.Size = new System.Drawing.Size(81, 25);
            this.txtboxThresholdOffset_BrightBlobBL.TabIndex = 4;
            this.txtboxThresholdOffset_BrightBlobBL.Text = "30";
            this.txtboxThresholdOffset_BrightBlobBL.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtboxThresholdOffset_BrightBlobBL.Validated += new System.EventHandler(this.txtboxThresholdOffset_BrightBlobBL_Validated);
            // 
            // txtboxAreaUpper_BrightBlobBL
            // 
            this.txtboxAreaUpper_BrightBlobBL.Location = new System.Drawing.Point(174, 35);
            this.txtboxAreaUpper_BrightBlobBL.Name = "txtboxAreaUpper_BrightBlobBL";
            this.txtboxAreaUpper_BrightBlobBL.Size = new System.Drawing.Size(81, 25);
            this.txtboxAreaUpper_BrightBlobBL.TabIndex = 4;
            this.txtboxAreaUpper_BrightBlobBL.Text = "2000";
            this.txtboxAreaUpper_BrightBlobBL.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtboxAreaLower_BrightBlobBL
            // 
            this.txtboxAreaLower_BrightBlobBL.Location = new System.Drawing.Point(88, 35);
            this.txtboxAreaLower_BrightBlobBL.Name = "txtboxAreaLower_BrightBlobBL";
            this.txtboxAreaLower_BrightBlobBL.Size = new System.Drawing.Size(81, 25);
            this.txtboxAreaLower_BrightBlobBL.TabIndex = 4;
            this.txtboxAreaLower_BrightBlobBL.Text = "100";
            this.txtboxAreaLower_BrightBlobBL.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnBrightBlobBL
            // 
            this.btnBrightBlobBL.BackColor = System.Drawing.SystemColors.Control;
            this.btnBrightBlobBL.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnBrightBlobBL.Location = new System.Drawing.Point(525, 6);
            this.btnBrightBlobBL.Name = "btnBrightBlobBL";
            this.btnBrightBlobBL.Size = new System.Drawing.Size(98, 40);
            this.btnBrightBlobBL.TabIndex = 3;
            this.btnBrightBlobBL.Text = "檢測";
            this.btnBrightBlobBL.UseVisualStyleBackColor = false;
            this.btnBrightBlobBL.Click += new System.EventHandler(this.btnBrightBlobBL_Click);
            // 
            // tabBlackCrackFL
            // 
            this.tabBlackCrackFL.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tabBlackCrackFL.Controls.Add(this.chkboxVertClose_BlackCrackFL);
            this.tabBlackCrackFL.Controls.Add(this.label6);
            this.tabBlackCrackFL.Controls.Add(this.txtboxCrackHeight_BlackCrackFL);
            this.tabBlackCrackFL.Controls.Add(this.label5);
            this.tabBlackCrackFL.Controls.Add(this.txtboxAreaLower_BlackCrackFL);
            this.tabBlackCrackFL.Controls.Add(this.txtboxAreaUpper_BlackCrackFL);
            this.tabBlackCrackFL.Controls.Add(this.label4);
            this.tabBlackCrackFL.Controls.Add(this.txtboxThresholdScale_BlackCrackFL);
            this.tabBlackCrackFL.Controls.Add(this.btnBlackCrackFL);
            this.tabBlackCrackFL.Location = new System.Drawing.Point(4, 26);
            this.tabBlackCrackFL.Name = "tabBlackCrackFL";
            this.tabBlackCrackFL.Padding = new System.Windows.Forms.Padding(3);
            this.tabBlackCrackFL.Size = new System.Drawing.Size(630, 70);
            this.tabBlackCrackFL.TabIndex = 1;
            this.tabBlackCrackFL.Text = "白條黑間隙(正光)";
            // 
            // chkboxVertClose_BlackCrackFL
            // 
            this.chkboxVertClose_BlackCrackFL.AutoSize = true;
            this.chkboxVertClose_BlackCrackFL.Checked = true;
            this.chkboxVertClose_BlackCrackFL.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkboxVertClose_BlackCrackFL.Location = new System.Drawing.Point(303, 39);
            this.chkboxVertClose_BlackCrackFL.Name = "chkboxVertClose_BlackCrackFL";
            this.chkboxVertClose_BlackCrackFL.Size = new System.Drawing.Size(79, 21);
            this.chkboxVertClose_BlackCrackFL.TabIndex = 28;
            this.chkboxVertClose_BlackCrackFL.Text = "垂直閉合";
            this.chkboxVertClose_BlackCrackFL.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(169, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(137, 17);
            this.label6.TabIndex = 27;
            this.label6.Text = "間隙寬度/高度 (像素): ";
            // 
            // txtboxCrackHeight_BlackCrackFL
            // 
            this.txtboxCrackHeight_BlackCrackFL.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtboxCrackHeight_BlackCrackFL.Location = new System.Drawing.Point(309, 6);
            this.txtboxCrackHeight_BlackCrackFL.Name = "txtboxCrackHeight_BlackCrackFL";
            this.txtboxCrackHeight_BlackCrackFL.Size = new System.Drawing.Size(73, 25);
            this.txtboxCrackHeight_BlackCrackFL.TabIndex = 26;
            this.txtboxCrackHeight_BlackCrackFL.Text = "39";
            this.txtboxCrackHeight_BlackCrackFL.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtboxCrackHeight_BlackCrackFL.Validated += new System.EventHandler(this.txtboxCrackHeight_BlackCrackFL_Validated);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(6, 40);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 17);
            this.label5.TabIndex = 25;
            this.label5.Text = "面積 (像素): ";
            // 
            // txtboxAreaLower_BlackCrackFL
            // 
            this.txtboxAreaLower_BlackCrackFL.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtboxAreaLower_BlackCrackFL.Location = new System.Drawing.Point(90, 37);
            this.txtboxAreaLower_BlackCrackFL.Name = "txtboxAreaLower_BlackCrackFL";
            this.txtboxAreaLower_BlackCrackFL.Size = new System.Drawing.Size(73, 25);
            this.txtboxAreaLower_BlackCrackFL.TabIndex = 23;
            this.txtboxAreaLower_BlackCrackFL.Text = "300";
            this.txtboxAreaLower_BlackCrackFL.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtboxAreaUpper_BlackCrackFL
            // 
            this.txtboxAreaUpper_BlackCrackFL.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtboxAreaUpper_BlackCrackFL.Location = new System.Drawing.Point(169, 37);
            this.txtboxAreaUpper_BlackCrackFL.Name = "txtboxAreaUpper_BlackCrackFL";
            this.txtboxAreaUpper_BlackCrackFL.Size = new System.Drawing.Size(73, 25);
            this.txtboxAreaUpper_BlackCrackFL.TabIndex = 24;
            this.txtboxAreaUpper_BlackCrackFL.Text = "700";
            this.txtboxAreaUpper_BlackCrackFL.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(45, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 17);
            this.label4.TabIndex = 22;
            this.label4.Text = "閾值: ";
            // 
            // txtboxThresholdScale_BlackCrackFL
            // 
            this.txtboxThresholdScale_BlackCrackFL.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtboxThresholdScale_BlackCrackFL.Location = new System.Drawing.Point(90, 6);
            this.txtboxThresholdScale_BlackCrackFL.Name = "txtboxThresholdScale_BlackCrackFL";
            this.txtboxThresholdScale_BlackCrackFL.Size = new System.Drawing.Size(73, 25);
            this.txtboxThresholdScale_BlackCrackFL.TabIndex = 21;
            this.txtboxThresholdScale_BlackCrackFL.Text = "1";
            this.txtboxThresholdScale_BlackCrackFL.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtboxThresholdScale_BlackCrackFL.Validated += new System.EventHandler(this.txtboxThresholdScale_BlackCrackFL_Validated);
            // 
            // btnBlackCrackFL
            // 
            this.btnBlackCrackFL.BackColor = System.Drawing.SystemColors.Control;
            this.btnBlackCrackFL.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnBlackCrackFL.Location = new System.Drawing.Point(525, 6);
            this.btnBlackCrackFL.Name = "btnBlackCrackFL";
            this.btnBlackCrackFL.Size = new System.Drawing.Size(98, 40);
            this.btnBlackCrackFL.TabIndex = 4;
            this.btnBlackCrackFL.Text = "檢測";
            this.btnBlackCrackFL.UseVisualStyleBackColor = false;
            this.btnBlackCrackFL.Click += new System.EventHandler(this.btnBlackCrackFL_Click);
            // 
            // tabWhiteCrackFL
            // 
            this.tabWhiteCrackFL.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tabWhiteCrackFL.Controls.Add(this.chkboxVertClose_WhiteCrackFL);
            this.tabWhiteCrackFL.Controls.Add(this.label12);
            this.tabWhiteCrackFL.Controls.Add(this.txtboxCrackWidth_WhiteCrackFL);
            this.tabWhiteCrackFL.Controls.Add(this.label13);
            this.tabWhiteCrackFL.Controls.Add(this.txtboxAreaLower_WhiteCrackFL);
            this.tabWhiteCrackFL.Controls.Add(this.txtboxAreaUpper_WhiteCrackFL);
            this.tabWhiteCrackFL.Controls.Add(this.label14);
            this.tabWhiteCrackFL.Controls.Add(this.txtboxThresholdScale_WhiteCrackFL);
            this.tabWhiteCrackFL.Controls.Add(this.btnWhiteCrackFL);
            this.tabWhiteCrackFL.Location = new System.Drawing.Point(4, 26);
            this.tabWhiteCrackFL.Name = "tabWhiteCrackFL";
            this.tabWhiteCrackFL.Size = new System.Drawing.Size(630, 70);
            this.tabWhiteCrackFL.TabIndex = 2;
            this.tabWhiteCrackFL.Text = "黑條白裂隙(正光)";
            // 
            // chkboxVertClose_WhiteCrackFL
            // 
            this.chkboxVertClose_WhiteCrackFL.AutoSize = true;
            this.chkboxVertClose_WhiteCrackFL.Location = new System.Drawing.Point(303, 39);
            this.chkboxVertClose_WhiteCrackFL.Name = "chkboxVertClose_WhiteCrackFL";
            this.chkboxVertClose_WhiteCrackFL.Size = new System.Drawing.Size(79, 21);
            this.chkboxVertClose_WhiteCrackFL.TabIndex = 36;
            this.chkboxVertClose_WhiteCrackFL.Text = "垂直閉合";
            this.chkboxVertClose_WhiteCrackFL.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(169, 9);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(134, 17);
            this.label12.TabIndex = 35;
            this.label12.Text = "間隙寬度/高度 (像素):";
            // 
            // txtboxCrackWidth_WhiteCrackFL
            // 
            this.txtboxCrackWidth_WhiteCrackFL.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtboxCrackWidth_WhiteCrackFL.Location = new System.Drawing.Point(309, 6);
            this.txtboxCrackWidth_WhiteCrackFL.Name = "txtboxCrackWidth_WhiteCrackFL";
            this.txtboxCrackWidth_WhiteCrackFL.Size = new System.Drawing.Size(73, 25);
            this.txtboxCrackWidth_WhiteCrackFL.TabIndex = 34;
            this.txtboxCrackWidth_WhiteCrackFL.Text = "21";
            this.txtboxCrackWidth_WhiteCrackFL.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtboxCrackWidth_WhiteCrackFL.Validated += new System.EventHandler(this.txtboxCrackWidth_WhiteCrackFL_Validated);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(6, 40);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(79, 17);
            this.label13.TabIndex = 33;
            this.label13.Text = "面積 (像素): ";
            // 
            // txtboxAreaLower_WhiteCrackFL
            // 
            this.txtboxAreaLower_WhiteCrackFL.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtboxAreaLower_WhiteCrackFL.Location = new System.Drawing.Point(90, 37);
            this.txtboxAreaLower_WhiteCrackFL.Name = "txtboxAreaLower_WhiteCrackFL";
            this.txtboxAreaLower_WhiteCrackFL.Size = new System.Drawing.Size(73, 25);
            this.txtboxAreaLower_WhiteCrackFL.TabIndex = 31;
            this.txtboxAreaLower_WhiteCrackFL.Text = "30";
            this.txtboxAreaLower_WhiteCrackFL.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtboxAreaUpper_WhiteCrackFL
            // 
            this.txtboxAreaUpper_WhiteCrackFL.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtboxAreaUpper_WhiteCrackFL.Location = new System.Drawing.Point(169, 37);
            this.txtboxAreaUpper_WhiteCrackFL.Name = "txtboxAreaUpper_WhiteCrackFL";
            this.txtboxAreaUpper_WhiteCrackFL.Size = new System.Drawing.Size(73, 25);
            this.txtboxAreaUpper_WhiteCrackFL.TabIndex = 32;
            this.txtboxAreaUpper_WhiteCrackFL.Text = "200";
            this.txtboxAreaUpper_WhiteCrackFL.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(45, 9);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(40, 17);
            this.label14.TabIndex = 30;
            this.label14.Text = "閾值: ";
            // 
            // txtboxThresholdScale_WhiteCrackFL
            // 
            this.txtboxThresholdScale_WhiteCrackFL.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtboxThresholdScale_WhiteCrackFL.Location = new System.Drawing.Point(90, 6);
            this.txtboxThresholdScale_WhiteCrackFL.Name = "txtboxThresholdScale_WhiteCrackFL";
            this.txtboxThresholdScale_WhiteCrackFL.Size = new System.Drawing.Size(73, 25);
            this.txtboxThresholdScale_WhiteCrackFL.TabIndex = 29;
            this.txtboxThresholdScale_WhiteCrackFL.Text = "0.5";
            this.txtboxThresholdScale_WhiteCrackFL.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtboxThresholdScale_WhiteCrackFL.Validated += new System.EventHandler(this.txtboxThresholdScale_WhiteCrackFL_Validated);
            // 
            // btnWhiteCrackFL
            // 
            this.btnWhiteCrackFL.BackColor = System.Drawing.SystemColors.Control;
            this.btnWhiteCrackFL.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnWhiteCrackFL.Location = new System.Drawing.Point(525, 6);
            this.btnWhiteCrackFL.Name = "btnWhiteCrackFL";
            this.btnWhiteCrackFL.Size = new System.Drawing.Size(98, 40);
            this.btnWhiteCrackFL.TabIndex = 4;
            this.btnWhiteCrackFL.Text = "檢測";
            this.btnWhiteCrackFL.UseVisualStyleBackColor = false;
            this.btnWhiteCrackFL.Click += new System.EventHandler(this.btnWhiteCrackFL_Click);
            // 
            // tabWhiteBlobFL
            // 
            this.tabWhiteBlobFL.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tabWhiteBlobFL.Controls.Add(this.chkboxVertClose_WhiteBlobFL);
            this.tabWhiteBlobFL.Controls.Add(this.label9);
            this.tabWhiteBlobFL.Controls.Add(this.txtboxBlobHeight_WhiteBlobFL);
            this.tabWhiteBlobFL.Controls.Add(this.label8);
            this.tabWhiteBlobFL.Controls.Add(this.txtboxAreaLower_WhiteBlobFL);
            this.tabWhiteBlobFL.Controls.Add(this.txtboxAreaUpper_WhiteBlobFL);
            this.tabWhiteBlobFL.Controls.Add(this.label7);
            this.tabWhiteBlobFL.Controls.Add(this.txtboxThresholdScale_WhiteBlobFL);
            this.tabWhiteBlobFL.Controls.Add(this.btnWhiteBlobFL);
            this.tabWhiteBlobFL.Location = new System.Drawing.Point(4, 26);
            this.tabWhiteBlobFL.Name = "tabWhiteBlobFL";
            this.tabWhiteBlobFL.Size = new System.Drawing.Size(630, 70);
            this.tabWhiteBlobFL.TabIndex = 3;
            this.tabWhiteBlobFL.Text = "黑條白斑點(正光)";
            // 
            // chkboxVertClose_WhiteBlobFL
            // 
            this.chkboxVertClose_WhiteBlobFL.AutoSize = true;
            this.chkboxVertClose_WhiteBlobFL.Location = new System.Drawing.Point(303, 39);
            this.chkboxVertClose_WhiteBlobFL.Name = "chkboxVertClose_WhiteBlobFL";
            this.chkboxVertClose_WhiteBlobFL.Size = new System.Drawing.Size(79, 21);
            this.chkboxVertClose_WhiteBlobFL.TabIndex = 37;
            this.chkboxVertClose_WhiteBlobFL.Text = "垂直閉合";
            this.chkboxVertClose_WhiteBlobFL.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(169, 9);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(134, 17);
            this.label9.TabIndex = 29;
            this.label9.Text = "斑點寬度/高度 (像素):";
            // 
            // txtboxBlobHeight_WhiteBlobFL
            // 
            this.txtboxBlobHeight_WhiteBlobFL.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtboxBlobHeight_WhiteBlobFL.Location = new System.Drawing.Point(309, 6);
            this.txtboxBlobHeight_WhiteBlobFL.Name = "txtboxBlobHeight_WhiteBlobFL";
            this.txtboxBlobHeight_WhiteBlobFL.Size = new System.Drawing.Size(73, 25);
            this.txtboxBlobHeight_WhiteBlobFL.TabIndex = 28;
            this.txtboxBlobHeight_WhiteBlobFL.Text = "93";
            this.txtboxBlobHeight_WhiteBlobFL.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtboxBlobHeight_WhiteBlobFL.Validated += new System.EventHandler(this.txtboxBlobHeight_WhiteBlobFL_Validated);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(6, 40);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(76, 17);
            this.label8.TabIndex = 27;
            this.label8.Text = "面積 (像素):";
            // 
            // txtboxAreaLower_WhiteBlobFL
            // 
            this.txtboxAreaLower_WhiteBlobFL.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtboxAreaLower_WhiteBlobFL.Location = new System.Drawing.Point(90, 37);
            this.txtboxAreaLower_WhiteBlobFL.Name = "txtboxAreaLower_WhiteBlobFL";
            this.txtboxAreaLower_WhiteBlobFL.Size = new System.Drawing.Size(73, 25);
            this.txtboxAreaLower_WhiteBlobFL.TabIndex = 25;
            this.txtboxAreaLower_WhiteBlobFL.Text = "500";
            this.txtboxAreaLower_WhiteBlobFL.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtboxAreaUpper_WhiteBlobFL
            // 
            this.txtboxAreaUpper_WhiteBlobFL.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtboxAreaUpper_WhiteBlobFL.Location = new System.Drawing.Point(169, 37);
            this.txtboxAreaUpper_WhiteBlobFL.Name = "txtboxAreaUpper_WhiteBlobFL";
            this.txtboxAreaUpper_WhiteBlobFL.Size = new System.Drawing.Size(73, 25);
            this.txtboxAreaUpper_WhiteBlobFL.TabIndex = 26;
            this.txtboxAreaUpper_WhiteBlobFL.Text = "4000";
            this.txtboxAreaUpper_WhiteBlobFL.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(45, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(37, 17);
            this.label7.TabIndex = 24;
            this.label7.Text = "閾值:";
            // 
            // txtboxThresholdScale_WhiteBlobFL
            // 
            this.txtboxThresholdScale_WhiteBlobFL.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtboxThresholdScale_WhiteBlobFL.Location = new System.Drawing.Point(90, 6);
            this.txtboxThresholdScale_WhiteBlobFL.Name = "txtboxThresholdScale_WhiteBlobFL";
            this.txtboxThresholdScale_WhiteBlobFL.Size = new System.Drawing.Size(73, 25);
            this.txtboxThresholdScale_WhiteBlobFL.TabIndex = 23;
            this.txtboxThresholdScale_WhiteBlobFL.Text = "1";
            this.txtboxThresholdScale_WhiteBlobFL.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtboxThresholdScale_WhiteBlobFL.Validated += new System.EventHandler(this.txtboxThresholdScale_WhiteBlobFL_Validated);
            // 
            // btnWhiteBlobFL
            // 
            this.btnWhiteBlobFL.BackColor = System.Drawing.SystemColors.Control;
            this.btnWhiteBlobFL.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnWhiteBlobFL.Location = new System.Drawing.Point(525, 6);
            this.btnWhiteBlobFL.Name = "btnWhiteBlobFL";
            this.btnWhiteBlobFL.Size = new System.Drawing.Size(98, 40);
            this.btnWhiteBlobFL.TabIndex = 4;
            this.btnWhiteBlobFL.Text = "檢測";
            this.btnWhiteBlobFL.UseVisualStyleBackColor = false;
            this.btnWhiteBlobFL.Click += new System.EventHandler(this.btnWhiteBlobFL_Click);
            // 
            // tabBlackBlobCL
            // 
            this.tabBlackBlobCL.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tabBlackBlobCL.Controls.Add(this.label15);
            this.tabBlackBlobCL.Controls.Add(this.txtboxMaskSize_BlackBlobCL);
            this.tabBlackBlobCL.Controls.Add(this.label10);
            this.tabBlackBlobCL.Controls.Add(this.txtboxAreaLower_BlackBlobCL);
            this.tabBlackBlobCL.Controls.Add(this.txtboxAreaUpper_BlackBlobCL);
            this.tabBlackBlobCL.Controls.Add(this.label11);
            this.tabBlackBlobCL.Controls.Add(this.txtboxThresholdScale_BlackBlobCL);
            this.tabBlackBlobCL.Controls.Add(this.btnBlackBlobCL);
            this.tabBlackBlobCL.Location = new System.Drawing.Point(4, 26);
            this.tabBlackBlobCL.Name = "tabBlackBlobCL";
            this.tabBlackBlobCL.Size = new System.Drawing.Size(630, 70);
            this.tabBlackBlobCL.TabIndex = 4;
            this.tabBlackBlobCL.Text = "黑色斑點(同軸光)";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(167, 9);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(234, 17);
            this.label15.TabIndex = 36;
            this.label15.Text = "遮罩大小 (Width / Mask Size) * 2 + 1: ";
            // 
            // txtboxMaskSize_BlackBlobCL
            // 
            this.txtboxMaskSize_BlackBlobCL.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtboxMaskSize_BlackBlobCL.Location = new System.Drawing.Point(407, 6);
            this.txtboxMaskSize_BlackBlobCL.Name = "txtboxMaskSize_BlackBlobCL";
            this.txtboxMaskSize_BlackBlobCL.Size = new System.Drawing.Size(73, 25);
            this.txtboxMaskSize_BlackBlobCL.TabIndex = 35;
            this.txtboxMaskSize_BlackBlobCL.Text = "8";
            this.txtboxMaskSize_BlackBlobCL.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtboxMaskSize_BlackBlobCL.Validated += new System.EventHandler(this.txtboxMaskSize_BlackBlobCL_Validated);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(6, 40);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(76, 17);
            this.label10.TabIndex = 34;
            this.label10.Text = "面積 (像素):";
            // 
            // txtboxAreaLower_BlackBlobCL
            // 
            this.txtboxAreaLower_BlackBlobCL.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtboxAreaLower_BlackBlobCL.Location = new System.Drawing.Point(88, 37);
            this.txtboxAreaLower_BlackBlobCL.Name = "txtboxAreaLower_BlackBlobCL";
            this.txtboxAreaLower_BlackBlobCL.Size = new System.Drawing.Size(73, 25);
            this.txtboxAreaLower_BlackBlobCL.TabIndex = 32;
            this.txtboxAreaLower_BlackBlobCL.Text = "500";
            this.txtboxAreaLower_BlackBlobCL.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtboxAreaUpper_BlackBlobCL
            // 
            this.txtboxAreaUpper_BlackBlobCL.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtboxAreaUpper_BlackBlobCL.Location = new System.Drawing.Point(167, 37);
            this.txtboxAreaUpper_BlackBlobCL.Name = "txtboxAreaUpper_BlackBlobCL";
            this.txtboxAreaUpper_BlackBlobCL.Size = new System.Drawing.Size(73, 25);
            this.txtboxAreaUpper_BlackBlobCL.TabIndex = 33;
            this.txtboxAreaUpper_BlackBlobCL.Text = "4000";
            this.txtboxAreaUpper_BlackBlobCL.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(45, 9);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(37, 17);
            this.label11.TabIndex = 31;
            this.label11.Text = "閾值:";
            // 
            // txtboxThresholdScale_BlackBlobCL
            // 
            this.txtboxThresholdScale_BlackBlobCL.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtboxThresholdScale_BlackBlobCL.Location = new System.Drawing.Point(88, 6);
            this.txtboxThresholdScale_BlackBlobCL.Name = "txtboxThresholdScale_BlackBlobCL";
            this.txtboxThresholdScale_BlackBlobCL.Size = new System.Drawing.Size(73, 25);
            this.txtboxThresholdScale_BlackBlobCL.TabIndex = 30;
            this.txtboxThresholdScale_BlackBlobCL.Text = "0.9";
            this.txtboxThresholdScale_BlackBlobCL.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtboxThresholdScale_BlackBlobCL.Validated += new System.EventHandler(this.txtboxThresholdScale_BlackBlobCL_Validated);
            // 
            // btnBlackBlobCL
            // 
            this.btnBlackBlobCL.BackColor = System.Drawing.SystemColors.Control;
            this.btnBlackBlobCL.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnBlackBlobCL.Location = new System.Drawing.Point(525, 6);
            this.btnBlackBlobCL.Name = "btnBlackBlobCL";
            this.btnBlackBlobCL.Size = new System.Drawing.Size(98, 40);
            this.btnBlackBlobCL.TabIndex = 4;
            this.btnBlackBlobCL.Text = "檢測";
            this.btnBlackBlobCL.UseVisualStyleBackColor = false;
            this.btnBlackBlobCL.Click += new System.EventHandler(this.btnBlackBlobCL_Click);
            // 
            // hSmartWindowControl1
            // 
            this.hSmartWindowControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.hSmartWindowControl1.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.hSmartWindowControl1.HDoubleClickToFitContent = true;
            this.hSmartWindowControl1.HDrawingObjectsModifier = HalconDotNet.HSmartWindowControl.DrawingObjectsModifier.None;
            this.hSmartWindowControl1.HImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hSmartWindowControl1.HKeepAspectRatio = true;
            this.hSmartWindowControl1.HMoveContent = true;
            this.hSmartWindowControl1.HZoomContent = HalconDotNet.HSmartWindowControl.ZoomContent.WheelForwardZoomsIn;
            this.hSmartWindowControl1.Location = new System.Drawing.Point(3, 106);
            this.hSmartWindowControl1.Margin = new System.Windows.Forms.Padding(0);
            this.hSmartWindowControl1.Name = "hSmartWindowControl1";
            this.hSmartWindowControl1.Size = new System.Drawing.Size(1140, 607);
            this.hSmartWindowControl1.TabIndex = 0;
            this.hSmartWindowControl1.WindowSize = new System.Drawing.Size(1140, 607);
            // 
            // btnLoadImage
            // 
            this.btnLoadImage.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btnLoadImage.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnLoadImage.Location = new System.Drawing.Point(941, 3);
            this.btnLoadImage.Name = "btnLoadImage";
            this.btnLoadImage.Size = new System.Drawing.Size(98, 40);
            this.btnLoadImage.TabIndex = 2;
            this.btnLoadImage.Text = "載入影像";
            this.btnLoadImage.UseVisualStyleBackColor = false;
            this.btnLoadImage.Click += new System.EventHandler(this.btnLoadImage_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // labFileName
            // 
            this.labFileName.AutoSize = true;
            this.labFileName.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labFileName.Location = new System.Drawing.Point(647, 15);
            this.labFileName.Name = "labFileName";
            this.labFileName.Size = new System.Drawing.Size(76, 17);
            this.labFileName.TabIndex = 3;
            this.labFileName.Text = "File Name: ";
            // 
            // btnLoadParameters
            // 
            this.btnLoadParameters.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btnLoadParameters.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnLoadParameters.Location = new System.Drawing.Point(1045, 3);
            this.btnLoadParameters.Name = "btnLoadParameters";
            this.btnLoadParameters.Size = new System.Drawing.Size(98, 40);
            this.btnLoadParameters.TabIndex = 4;
            this.btnLoadParameters.Text = "載入參數";
            this.btnLoadParameters.UseVisualStyleBackColor = false;
            this.btnLoadParameters.Click += new System.EventHandler(this.btnLoadParameters_Click);
            // 
            // btnSaveParameters
            // 
            this.btnSaveParameters.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btnSaveParameters.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnSaveParameters.Location = new System.Drawing.Point(1045, 49);
            this.btnSaveParameters.Name = "btnSaveParameters";
            this.btnSaveParameters.Size = new System.Drawing.Size(98, 40);
            this.btnSaveParameters.TabIndex = 5;
            this.btnSaveParameters.Text = "儲存參數";
            this.btnSaveParameters.UseVisualStyleBackColor = false;
            this.btnSaveParameters.Click += new System.EventHandler(this.btnSaveParameters_Click);
            // 
            // chkboxCombinedTest
            // 
            this.chkboxCombinedTest.AutoSize = true;
            this.chkboxCombinedTest.Location = new System.Drawing.Point(651, 59);
            this.chkboxCombinedTest.Name = "chkboxCombinedTest";
            this.chkboxCombinedTest.Size = new System.Drawing.Size(72, 16);
            this.chkboxCombinedTest.TabIndex = 38;
            this.chkboxCombinedTest.Text = "整合檢測";
            this.chkboxCombinedTest.UseVisualStyleBackColor = true;
            // 
            // HTResistor0201UC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkboxCombinedTest);
            this.Controls.Add(this.btnSaveParameters);
            this.Controls.Add(this.btnLoadParameters);
            this.Controls.Add(this.labFileName);
            this.Controls.Add(this.btnLoadImage);
            this.Controls.Add(this.hSmartWindowControl1);
            this.Controls.Add(this.tabControlHTResistor);
            this.Controls.Add(this.button_Add);
            this.Name = "HTResistor0201UC";
            this.Size = new System.Drawing.Size(1150, 860);
            this.Load += new System.EventHandler(this.HTResistor0201UC_Load);
            this.tabControlHTResistor.ResumeLayout(false);
            this.tabBrightBlobBL.ResumeLayout(false);
            this.tabBrightBlobBL.PerformLayout();
            this.tabBlackCrackFL.ResumeLayout(false);
            this.tabBlackCrackFL.PerformLayout();
            this.tabWhiteCrackFL.ResumeLayout(false);
            this.tabWhiteCrackFL.PerformLayout();
            this.tabWhiteBlobFL.ResumeLayout(false);
            this.tabWhiteBlobFL.PerformLayout();
            this.tabBlackBlobCL.ResumeLayout(false);
            this.tabBlackBlobCL.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_Add;
        private System.Windows.Forms.TabControl tabControlHTResistor;
        private System.Windows.Forms.TabPage tabBrightBlobBL;
        private System.Windows.Forms.TabPage tabBlackCrackFL;
        private System.Windows.Forms.TabPage tabWhiteCrackFL;
        private System.Windows.Forms.TabPage tabWhiteBlobFL;
        private System.Windows.Forms.TabPage tabBlackBlobCL;
        private HalconDotNet.HSmartWindowControl hSmartWindowControl1;
        private System.Windows.Forms.Button btnLoadImage;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnBrightBlobBL;
        private System.Windows.Forms.TextBox txtboxAreaLower_BrightBlobBL;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtboxAreaUpper_BrightBlobBL;
        private System.Windows.Forms.TextBox txtboxThresholdOffset_BrightBlobBL;
        private System.Windows.Forms.Button btnBlackCrackFL;
        private System.Windows.Forms.Button btnWhiteCrackFL;
        private System.Windows.Forms.Button btnWhiteBlobFL;
        private System.Windows.Forms.Button btnBlackBlobCL;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtboxCrackHeight_BlackCrackFL;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtboxAreaLower_BlackCrackFL;
        private System.Windows.Forms.TextBox txtboxAreaUpper_BlackCrackFL;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtboxThresholdScale_BlackCrackFL;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtboxCrackWidth_WhiteCrackFL;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtboxAreaLower_WhiteCrackFL;
        private System.Windows.Forms.TextBox txtboxAreaUpper_WhiteCrackFL;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtboxThresholdScale_WhiteCrackFL;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtboxBlobHeight_WhiteBlobFL;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtboxAreaLower_WhiteBlobFL;
        private System.Windows.Forms.TextBox txtboxAreaUpper_WhiteBlobFL;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtboxThresholdScale_WhiteBlobFL;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtboxMaskSize_BlackBlobCL;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtboxAreaLower_BlackBlobCL;
        private System.Windows.Forms.TextBox txtboxAreaUpper_BlackBlobCL;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtboxThresholdScale_BlackBlobCL;
        private System.Windows.Forms.Label labFileName;
        private System.Windows.Forms.Button btnLoadParameters;
        private System.Windows.Forms.Button btnSaveParameters;
        private System.Windows.Forms.CheckBox chkboxVertClose_BlackCrackFL;
        private System.Windows.Forms.CheckBox chkboxVertClose_WhiteCrackFL;
        private System.Windows.Forms.CheckBox chkboxVertClose_WhiteBlobFL;
        private System.Windows.Forms.CheckBox chkboxCombinedTest;
    }
}
