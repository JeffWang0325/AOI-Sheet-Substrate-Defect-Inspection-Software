namespace Risitanse_AOI.src.PositionMethod.GridSegmentation
{
    partial class GridSegment
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
            this.controlPanel = new System.Windows.Forms.Panel();
            this.loadImgB = new System.Windows.Forms.Button();
            this.exeB = new System.Windows.Forms.Button();
            this.erL_V = new System.Windows.Forms.Label();
            this.erH_V = new System.Windows.Forms.Label();
            this.thL_V = new System.Windows.Forms.Label();
            this.thH_V = new System.Windows.Forms.Label();
            this.erL_T = new System.Windows.Forms.TrackBar();
            this.erH_T = new System.Windows.Forms.TrackBar();
            this.thL_T = new System.Windows.Forms.TrackBar();
            this.thH_T = new System.Windows.Forms.TrackBar();
            this.erL_L = new System.Windows.Forms.Label();
            this.erH_L = new System.Windows.Forms.Label();
            this.thH_L = new System.Windows.Forms.Label();
            this.thL_L = new System.Windows.Forms.Label();
            this.downScalar_V = new System.Windows.Forms.TextBox();
            this.downScalar_L = new System.Windows.Forms.Label();
            this.showResult = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.controlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.erL_T)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.erH_T)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.thL_T)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.thH_T)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.showResult)).BeginInit();
            this.SuspendLayout();
            // 
            // controlPanel
            // 
            this.controlPanel.BackColor = System.Drawing.Color.White;
            this.controlPanel.Controls.Add(this.button1);
            this.controlPanel.Controls.Add(this.loadImgB);
            this.controlPanel.Controls.Add(this.exeB);
            this.controlPanel.Controls.Add(this.erL_V);
            this.controlPanel.Controls.Add(this.erH_V);
            this.controlPanel.Controls.Add(this.thL_V);
            this.controlPanel.Controls.Add(this.thH_V);
            this.controlPanel.Controls.Add(this.erL_T);
            this.controlPanel.Controls.Add(this.erH_T);
            this.controlPanel.Controls.Add(this.thL_T);
            this.controlPanel.Controls.Add(this.thH_T);
            this.controlPanel.Controls.Add(this.erL_L);
            this.controlPanel.Controls.Add(this.erH_L);
            this.controlPanel.Controls.Add(this.thH_L);
            this.controlPanel.Controls.Add(this.thL_L);
            this.controlPanel.Controls.Add(this.downScalar_V);
            this.controlPanel.Controls.Add(this.downScalar_L);
            this.controlPanel.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.controlPanel.Location = new System.Drawing.Point(21, 13);
            this.controlPanel.Margin = new System.Windows.Forms.Padding(4);
            this.controlPanel.Name = "controlPanel";
            this.controlPanel.Size = new System.Drawing.Size(412, 730);
            this.controlPanel.TabIndex = 3;
            // 
            // loadImgB
            // 
            this.loadImgB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(176)))), ((int)(((byte)(242)))));
            this.loadImgB.Font = new System.Drawing.Font("微軟正黑體", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.loadImgB.ForeColor = System.Drawing.Color.White;
            this.loadImgB.Location = new System.Drawing.Point(15, 16);
            this.loadImgB.Margin = new System.Windows.Forms.Padding(4);
            this.loadImgB.Name = "loadImgB";
            this.loadImgB.Size = new System.Drawing.Size(380, 54);
            this.loadImgB.TabIndex = 15;
            this.loadImgB.Text = "Load Image";
            this.loadImgB.UseVisualStyleBackColor = false;
            this.loadImgB.Click += new System.EventHandler(this.loadImgB_Click);
            // 
            // exeB
            // 
            this.exeB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(176)))), ((int)(((byte)(242)))));
            this.exeB.Font = new System.Drawing.Font("微軟正黑體", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.exeB.ForeColor = System.Drawing.Color.White;
            this.exeB.Location = new System.Drawing.Point(97, 662);
            this.exeB.Margin = new System.Windows.Forms.Padding(4);
            this.exeB.Name = "exeB";
            this.exeB.Size = new System.Drawing.Size(145, 54);
            this.exeB.TabIndex = 14;
            this.exeB.Text = "執行預覽";
            this.exeB.UseVisualStyleBackColor = false;
            this.exeB.Click += new System.EventHandler(this.exeB_Click);
            // 
            // erL_V
            // 
            this.erL_V.AutoSize = true;
            this.erL_V.Font = new System.Drawing.Font("微軟正黑體", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.erL_V.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.erL_V.Location = new System.Drawing.Point(327, 532);
            this.erL_V.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.erL_V.Name = "erL_V";
            this.erL_V.Size = new System.Drawing.Size(41, 30);
            this.erL_V.TabIndex = 13;
            this.erL_V.Text = "15";
            // 
            // erH_V
            // 
            this.erH_V.AutoSize = true;
            this.erH_V.Font = new System.Drawing.Font("微軟正黑體", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.erH_V.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.erH_V.Location = new System.Drawing.Point(327, 413);
            this.erH_V.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.erH_V.Name = "erH_V";
            this.erH_V.Size = new System.Drawing.Size(41, 30);
            this.erH_V.TabIndex = 12;
            this.erH_V.Text = "14";
            // 
            // thL_V
            // 
            this.thL_V.AutoSize = true;
            this.thL_V.Font = new System.Drawing.Font("微軟正黑體", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.thL_V.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.thL_V.Location = new System.Drawing.Point(327, 305);
            this.thL_V.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.thL_V.Name = "thL_V";
            this.thL_V.Size = new System.Drawing.Size(41, 30);
            this.thL_V.TabIndex = 11;
            this.thL_V.Text = "80";
            // 
            // thH_V
            // 
            this.thH_V.AutoSize = true;
            this.thH_V.Font = new System.Drawing.Font("微軟正黑體", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.thH_V.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.thH_V.Location = new System.Drawing.Point(327, 211);
            this.thH_V.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.thH_V.Name = "thH_V";
            this.thH_V.Size = new System.Drawing.Size(55, 30);
            this.thH_V.TabIndex = 10;
            this.thH_V.Text = "172";
            // 
            // erL_T
            // 
            this.erL_T.Location = new System.Drawing.Point(29, 532);
            this.erL_T.Margin = new System.Windows.Forms.Padding(4);
            this.erL_T.Maximum = 25;
            this.erL_T.Minimum = 3;
            this.erL_T.Name = "erL_T";
            this.erL_T.Size = new System.Drawing.Size(289, 56);
            this.erL_T.TabIndex = 9;
            this.erL_T.TickFrequency = 2;
            this.erL_T.Value = 15;
            this.erL_T.Scroll += new System.EventHandler(this.erL_T_Scroll);
            // 
            // erH_T
            // 
            this.erH_T.Location = new System.Drawing.Point(29, 413);
            this.erH_T.Margin = new System.Windows.Forms.Padding(4);
            this.erH_T.Maximum = 25;
            this.erH_T.Minimum = 3;
            this.erH_T.Name = "erH_T";
            this.erH_T.Size = new System.Drawing.Size(289, 56);
            this.erH_T.TabIndex = 8;
            this.erH_T.TickFrequency = 2;
            this.erH_T.Value = 14;
            this.erH_T.Scroll += new System.EventHandler(this.erH_T_Scroll);
            // 
            // thL_T
            // 
            this.thL_T.Location = new System.Drawing.Point(30, 305);
            this.thL_T.Margin = new System.Windows.Forms.Padding(4);
            this.thL_T.Maximum = 255;
            this.thL_T.Name = "thL_T";
            this.thL_T.Size = new System.Drawing.Size(289, 56);
            this.thL_T.TabIndex = 7;
            this.thL_T.Value = 80;
            this.thL_T.Scroll += new System.EventHandler(this.thL_T_Scroll);
            // 
            // thH_T
            // 
            this.thH_T.Location = new System.Drawing.Point(15, 211);
            this.thH_T.Margin = new System.Windows.Forms.Padding(4);
            this.thH_T.Maximum = 255;
            this.thH_T.Name = "thH_T";
            this.thH_T.Size = new System.Drawing.Size(289, 56);
            this.thH_T.TabIndex = 6;
            this.thH_T.Value = 172;
            this.thH_T.Scroll += new System.EventHandler(this.thH_T_Scroll);
            // 
            // erL_L
            // 
            this.erL_L.AutoSize = true;
            this.erL_L.Font = new System.Drawing.Font("微軟正黑體", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.erL_L.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.erL_L.Location = new System.Drawing.Point(25, 498);
            this.erL_L.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.erL_L.Name = "erL_L";
            this.erL_L.Size = new System.Drawing.Size(163, 30);
            this.erL_L.TabIndex = 5;
            this.erL_L.Text = "電阻侵蝕大小:";
            // 
            // erH_L
            // 
            this.erH_L.AutoSize = true;
            this.erH_L.Font = new System.Drawing.Font("微軟正黑體", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.erH_L.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.erH_L.Location = new System.Drawing.Point(25, 379);
            this.erH_L.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.erH_L.Name = "erH_L";
            this.erH_L.Size = new System.Drawing.Size(163, 30);
            this.erH_L.TabIndex = 4;
            this.erH_L.Text = "電極侵蝕大小:";
            // 
            // thH_L
            // 
            this.thH_L.AutoSize = true;
            this.thH_L.Font = new System.Drawing.Font("微軟正黑體", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.thH_L.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.thH_L.Location = new System.Drawing.Point(24, 177);
            this.thH_L.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.thH_L.Name = "thH_L";
            this.thH_L.Size = new System.Drawing.Size(163, 30);
            this.thH_L.TabIndex = 3;
            this.thH_L.Text = "電極二值閥值:";
            // 
            // thL_L
            // 
            this.thL_L.AutoSize = true;
            this.thL_L.Font = new System.Drawing.Font("微軟正黑體", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.thL_L.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.thL_L.Location = new System.Drawing.Point(25, 271);
            this.thL_L.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.thL_L.Name = "thL_L";
            this.thL_L.Size = new System.Drawing.Size(163, 30);
            this.thL_L.TabIndex = 2;
            this.thL_L.Text = "電阻二值閥值:";
            // 
            // downScalar_V
            // 
            this.downScalar_V.Font = new System.Drawing.Font("微軟正黑體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.downScalar_V.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.downScalar_V.Location = new System.Drawing.Point(212, 100);
            this.downScalar_V.Margin = new System.Windows.Forms.Padding(4);
            this.downScalar_V.Name = "downScalar_V";
            this.downScalar_V.Size = new System.Drawing.Size(170, 39);
            this.downScalar_V.TabIndex = 1;
            this.downScalar_V.Text = "4";
            // 
            // downScalar_L
            // 
            this.downScalar_L.AutoSize = true;
            this.downScalar_L.Font = new System.Drawing.Font("微軟正黑體", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.downScalar_L.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.downScalar_L.Location = new System.Drawing.Point(25, 103);
            this.downScalar_L.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.downScalar_L.Name = "downScalar_L";
            this.downScalar_L.Size = new System.Drawing.Size(179, 30);
            this.downScalar_L.TabIndex = 0;
            this.downScalar_L.Text = "縮小比例(計算):";
            // 
            // showResult
            // 
            this.showResult.BackColor = System.Drawing.Color.White;
            this.showResult.Location = new System.Drawing.Point(447, 13);
            this.showResult.Margin = new System.Windows.Forms.Padding(4);
            this.showResult.Name = "showResult";
            this.showResult.Size = new System.Drawing.Size(900, 730);
            this.showResult.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.showResult.TabIndex = 2;
            this.showResult.TabStop = false;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(176)))), ((int)(((byte)(242)))));
            this.button1.Font = new System.Drawing.Font("微軟正黑體", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(250, 662);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(145, 54);
            this.button1.TabIndex = 16;
            this.button1.Text = "儲存";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // GridSegment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1361, 752);
            this.Controls.Add(this.controlPanel);
            this.Controls.Add(this.showResult);
            this.Name = "GridSegment";
            this.Text = "GridSegmentation";
            this.controlPanel.ResumeLayout(false);
            this.controlPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.erL_T)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.erH_T)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.thL_T)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.thH_T)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.showResult)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel controlPanel;
        private System.Windows.Forms.Button loadImgB;
        private System.Windows.Forms.Button exeB;
        private System.Windows.Forms.Label erL_V;
        private System.Windows.Forms.Label erH_V;
        private System.Windows.Forms.Label thL_V;
        private System.Windows.Forms.Label thH_V;
        private System.Windows.Forms.TrackBar erL_T;
        private System.Windows.Forms.TrackBar erH_T;
        private System.Windows.Forms.TrackBar thL_T;
        private System.Windows.Forms.TrackBar thH_T;
        private System.Windows.Forms.Label erL_L;
        private System.Windows.Forms.Label erH_L;
        private System.Windows.Forms.Label thH_L;
        private System.Windows.Forms.Label thL_L;
        private System.Windows.Forms.TextBox downScalar_V;
        private System.Windows.Forms.Label downScalar_L;
        private System.Windows.Forms.PictureBox showResult;
        private System.Windows.Forms.Button button1;
    }
}