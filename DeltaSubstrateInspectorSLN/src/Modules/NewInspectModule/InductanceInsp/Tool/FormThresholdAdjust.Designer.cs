namespace DeltaSubstrateInspector.src.Modules.NewInspectModule.InductanceInsp
{
    partial class FormThresholdAdjust
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint1 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(0D, 2000000D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint2 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(0D, 3D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint3 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(0D, 4D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint4 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(0D, 5D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint5 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(0D, 3D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint6 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(0D, 2D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint7 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(0D, 9D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint8 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(0D, 8D);
            this.panelDisplay = new System.Windows.Forms.Panel();
            this.DisplayWindow = new HalconDotNet.HSmartWindowControl();
            this.NumUpDownThresholdMin = new System.Windows.Forms.NumericUpDown();
            this.chtHist = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.labMinThreshold = new System.Windows.Forms.Label();
            this.labMaxThreshold = new System.Windows.Forms.Label();
            this.NumUpDownThresholdMax = new System.Windows.Forms.NumericUpDown();
            this.rangeSliderTh = new MyLibrary.Controls.RangeSlider();
            this.panelDisplay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDownThresholdMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chtHist)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDownThresholdMax)).BeginInit();
            this.SuspendLayout();
            // 
            // panelDisplay
            // 
            this.panelDisplay.AutoScroll = true;
            this.panelDisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelDisplay.Controls.Add(this.DisplayWindow);
            this.panelDisplay.Location = new System.Drawing.Point(2, 2);
            this.panelDisplay.Name = "panelDisplay";
            this.panelDisplay.Size = new System.Drawing.Size(745, 669);
            this.panelDisplay.TabIndex = 71;
            // 
            // DisplayWindow
            // 
            this.DisplayWindow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.DisplayWindow.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.DisplayWindow.HDoubleClickToFitContent = true;
            this.DisplayWindow.HDrawingObjectsModifier = HalconDotNet.HSmartWindowControl.DrawingObjectsModifier.None;
            this.DisplayWindow.HImagePart = new System.Drawing.Rectangle(0, 0, 4096, 2160);
            this.DisplayWindow.HKeepAspectRatio = false;
            this.DisplayWindow.HMoveContent = true;
            this.DisplayWindow.HZoomContent = HalconDotNet.HSmartWindowControl.ZoomContent.WheelBackwardZoomsIn;
            this.DisplayWindow.ImeMode = System.Windows.Forms.ImeMode.On;
            this.DisplayWindow.Location = new System.Drawing.Point(0, 0);
            this.DisplayWindow.Margin = new System.Windows.Forms.Padding(0);
            this.DisplayWindow.Name = "DisplayWindow";
            this.DisplayWindow.Size = new System.Drawing.Size(743, 667);
            this.DisplayWindow.TabIndex = 70;
            this.DisplayWindow.WindowSize = new System.Drawing.Size(743, 667);
            // 
            // NumUpDownThresholdMin
            // 
            this.NumUpDownThresholdMin.Font = new System.Drawing.Font("Verdana", 9.75F);
            this.NumUpDownThresholdMin.Location = new System.Drawing.Point(865, 676);
            this.NumUpDownThresholdMin.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.NumUpDownThresholdMin.Name = "NumUpDownThresholdMin";
            this.NumUpDownThresholdMin.Size = new System.Drawing.Size(58, 23);
            this.NumUpDownThresholdMin.TabIndex = 73;
            this.NumUpDownThresholdMin.ValueChanged += new System.EventHandler(this.NumUpDownThreshold_ValueChanged);
            // 
            // chtHist
            // 
            this.chtHist.BackColor = System.Drawing.Color.Silver;
            this.chtHist.BorderlineColor = System.Drawing.Color.Black;
            this.chtHist.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            chartArea1.Name = "ChartArea1";
            this.chtHist.ChartAreas.Add(chartArea1);
            legend1.Enabled = false;
            legend1.Name = "Legend1";
            this.chtHist.Legends.Add(legend1);
            this.chtHist.Location = new System.Drawing.Point(754, 6);
            this.chtHist.Margin = new System.Windows.Forms.Padding(4);
            this.chtHist.Name = "chtHist";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.SplineArea;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            series1.Points.Add(dataPoint1);
            series1.Points.Add(dataPoint2);
            series1.Points.Add(dataPoint3);
            series1.Points.Add(dataPoint4);
            series1.Points.Add(dataPoint5);
            series1.Points.Add(dataPoint6);
            series1.Points.Add(dataPoint7);
            series1.Points.Add(dataPoint8);
            series1.ShadowColor = System.Drawing.Color.Silver;
            this.chtHist.Series.Add(series1);
            this.chtHist.Size = new System.Drawing.Size(577, 665);
            this.chtHist.TabIndex = 74;
            this.chtHist.Text = "Histogram";
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnCancel.Location = new System.Drawing.Point(1246, 689);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(85, 34);
            this.btnCancel.TabIndex = 75;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.Green;
            this.btnOk.Location = new System.Drawing.Point(1155, 689);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(85, 34);
            this.btnOk.TabIndex = 76;
            this.btnOk.Text = "確定";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // labMinThreshold
            // 
            this.labMinThreshold.AutoSize = true;
            this.labMinThreshold.Location = new System.Drawing.Point(751, 678);
            this.labMinThreshold.Name = "labMinThreshold";
            this.labMinThreshold.Size = new System.Drawing.Size(108, 18);
            this.labMinThreshold.TabIndex = 77;
            this.labMinThreshold.Text = "MinThreshold:";
            // 
            // labMaxThreshold
            // 
            this.labMaxThreshold.AutoSize = true;
            this.labMaxThreshold.Location = new System.Drawing.Point(968, 678);
            this.labMaxThreshold.Name = "labMaxThreshold";
            this.labMaxThreshold.Size = new System.Drawing.Size(110, 18);
            this.labMaxThreshold.TabIndex = 77;
            this.labMaxThreshold.Text = "MaxThreshold:";
            // 
            // NumUpDownThresholdMax
            // 
            this.NumUpDownThresholdMax.Font = new System.Drawing.Font("Verdana", 9.75F);
            this.NumUpDownThresholdMax.Location = new System.Drawing.Point(1084, 676);
            this.NumUpDownThresholdMax.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.NumUpDownThresholdMax.Name = "NumUpDownThresholdMax";
            this.NumUpDownThresholdMax.Size = new System.Drawing.Size(58, 23);
            this.NumUpDownThresholdMax.TabIndex = 73;
            this.NumUpDownThresholdMax.ValueChanged += new System.EventHandler(this.NumUpDownThresholdMax_ValueChanged);
            // 
            // rangeSliderTh
            // 
            this.rangeSliderTh.BackColor = System.Drawing.Color.Transparent;
            this.rangeSliderTh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.rangeSliderTh.BarMax = 255;
            this.rangeSliderTh.BarMin = 0;
            this.rangeSliderTh.Color = MyLibrary.Classes.EMyColors.Blue;
            this.rangeSliderTh.CustomBackground = false;
            this.rangeSliderTh.Font = new System.Drawing.Font("Arimo", 1F);
            this.rangeSliderTh.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.rangeSliderTh.Location = new System.Drawing.Point(841, 644);
            this.rangeSliderTh.Margin = new System.Windows.Forms.Padding(0);
            this.rangeSliderTh.Name = "rangeSliderTh";
            this.rangeSliderTh.Orientation = MyLibrary.Classes.HVOrientation.Horizontal;
            this.rangeSliderTh.RangeMax = 255;
            this.rangeSliderTh.RangeMin = 0;
            this.rangeSliderTh.Reverse = false;
            this.rangeSliderTh.Size = new System.Drawing.Size(471, 16);
            this.rangeSliderTh.TabIndex = 78;
            this.rangeSliderTh.TabStop = false;
            this.rangeSliderTh.Scroll += new System.EventHandler(this.rangeSliderTh_Scroll);
            // 
            // FormThresholdAdjust
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1343, 735);
            this.Controls.Add(this.rangeSliderTh);
            this.Controls.Add(this.labMaxThreshold);
            this.Controls.Add(this.labMinThreshold);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.chtHist);
            this.Controls.Add(this.NumUpDownThresholdMax);
            this.Controls.Add(this.NumUpDownThresholdMin);
            this.Controls.Add(this.panelDisplay);
            this.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormThresholdAdjust";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "調整二值化閥值";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormThresholdAdjust_FormClosed);
            this.panelDisplay.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDownThresholdMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chtHist)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDownThresholdMax)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panelDisplay;
        private System.Windows.Forms.NumericUpDown NumUpDownThresholdMin;
        private System.Windows.Forms.DataVisualization.Charting.Chart chtHist;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label labMinThreshold;
        private System.Windows.Forms.Label labMaxThreshold;
        private System.Windows.Forms.NumericUpDown NumUpDownThresholdMax;
        private HalconDotNet.HSmartWindowControl DisplayWindow;
        private MyLibrary.Controls.RangeSlider rangeSliderTh;
    }
}