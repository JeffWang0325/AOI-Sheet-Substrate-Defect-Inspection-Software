namespace DeltaSubstrateInspector.src.PositionMethod.LocateMethod.Location
{
    partial class DispWindow_AICellImg
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
            this.hSmartWindowControl_AICellImg = new HalconDotNet.HSmartWindowControl();
            this.trackBar_Width = new System.Windows.Forms.TrackBar();
            this.trackBar_Height = new System.Windows.Forms.TrackBar();
            this.label_Width = new System.Windows.Forms.Label();
            this.label_Height = new System.Windows.Forms.Label();
            this.nud_Width = new System.Windows.Forms.NumericUpDown();
            this.nud_Height = new System.Windows.Forms.NumericUpDown();
            this.txt_LabelType_AICellImg = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Width)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Height)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Width)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Height)).BeginInit();
            this.SuspendLayout();
            // 
            // hSmartWindowControl_AICellImg
            // 
            this.hSmartWindowControl_AICellImg.AllowDrop = true;
            this.hSmartWindowControl_AICellImg.AutoScroll = true;
            this.hSmartWindowControl_AICellImg.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.hSmartWindowControl_AICellImg.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.hSmartWindowControl_AICellImg.BackColor = System.Drawing.SystemColors.HotTrack;
            this.hSmartWindowControl_AICellImg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hSmartWindowControl_AICellImg.HDoubleClickToFitContent = true;
            this.hSmartWindowControl_AICellImg.HDrawingObjectsModifier = HalconDotNet.HSmartWindowControl.DrawingObjectsModifier.None;
            this.hSmartWindowControl_AICellImg.HImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hSmartWindowControl_AICellImg.HKeepAspectRatio = false;
            this.hSmartWindowControl_AICellImg.HMoveContent = true;
            this.hSmartWindowControl_AICellImg.HZoomContent = HalconDotNet.HSmartWindowControl.ZoomContent.WheelBackwardZoomsIn;
            this.hSmartWindowControl_AICellImg.Location = new System.Drawing.Point(55, 40);
            this.hSmartWindowControl_AICellImg.Margin = new System.Windows.Forms.Padding(0);
            this.hSmartWindowControl_AICellImg.Name = "hSmartWindowControl_AICellImg";
            this.hSmartWindowControl_AICellImg.Size = new System.Drawing.Size(670, 600);
            this.hSmartWindowControl_AICellImg.TabIndex = 131;
            this.hSmartWindowControl_AICellImg.WindowSize = new System.Drawing.Size(670, 600);
            // 
            // trackBar_Width
            // 
            this.trackBar_Width.BackColor = System.Drawing.Color.White;
            this.trackBar_Width.Location = new System.Drawing.Point(169, 11);
            this.trackBar_Width.Margin = new System.Windows.Forms.Padding(2);
            this.trackBar_Width.Maximum = 800;
            this.trackBar_Width.Minimum = 100;
            this.trackBar_Width.Name = "trackBar_Width";
            this.trackBar_Width.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.trackBar_Width.Size = new System.Drawing.Size(300, 45);
            this.trackBar_Width.TabIndex = 132;
            this.trackBar_Width.Tag = "Width";
            this.trackBar_Width.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar_Width.Value = 100;
            this.trackBar_Width.Scroll += new System.EventHandler(this.trackBar_Scroll);
            // 
            // trackBar_Height
            // 
            this.trackBar_Height.BackColor = System.Drawing.Color.White;
            this.trackBar_Height.Location = new System.Drawing.Point(9, 115);
            this.trackBar_Height.Margin = new System.Windows.Forms.Padding(2);
            this.trackBar_Height.Maximum = 800;
            this.trackBar_Height.Minimum = 100;
            this.trackBar_Height.Name = "trackBar_Height";
            this.trackBar_Height.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar_Height.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.trackBar_Height.Size = new System.Drawing.Size(45, 300);
            this.trackBar_Height.TabIndex = 133;
            this.trackBar_Height.Tag = "Height";
            this.trackBar_Height.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar_Height.Value = 800;
            this.trackBar_Height.Scroll += new System.EventHandler(this.trackBar_Scroll);
            // 
            // label_Width
            // 
            this.label_Width.AutoSize = true;
            this.label_Width.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_Width.ForeColor = System.Drawing.Color.Black;
            this.label_Width.Location = new System.Drawing.Point(75, 11);
            this.label_Width.Name = "label_Width";
            this.label_Width.Size = new System.Drawing.Size(33, 24);
            this.label_Width.TabIndex = 134;
            this.label_Width.Text = "寬:";
            // 
            // label_Height
            // 
            this.label_Height.AutoSize = true;
            this.label_Height.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_Height.ForeColor = System.Drawing.Color.Black;
            this.label_Height.Location = new System.Drawing.Point(4, 50);
            this.label_Height.Name = "label_Height";
            this.label_Height.Size = new System.Drawing.Size(33, 24);
            this.label_Height.TabIndex = 135;
            this.label_Height.Text = "高:";
            // 
            // nud_Width
            // 
            this.nud_Width.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.nud_Width.Location = new System.Drawing.Point(112, 12);
            this.nud_Width.Maximum = new decimal(new int[] {
            800,
            0,
            0,
            0});
            this.nud_Width.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nud_Width.Name = "nud_Width";
            this.nud_Width.Size = new System.Drawing.Size(48, 26);
            this.nud_Width.TabIndex = 166;
            this.nud_Width.Tag = "Width";
            this.nud_Width.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nud_Width.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nud_Width.ValueChanged += new System.EventHandler(this.nud_ValueChanged);
            // 
            // nud_Height
            // 
            this.nud_Height.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.nud_Height.Location = new System.Drawing.Point(3, 79);
            this.nud_Height.Maximum = new decimal(new int[] {
            800,
            0,
            0,
            0});
            this.nud_Height.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nud_Height.Name = "nud_Height";
            this.nud_Height.Size = new System.Drawing.Size(48, 26);
            this.nud_Height.TabIndex = 167;
            this.nud_Height.Tag = "Height";
            this.nud_Height.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nud_Height.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nud_Height.ValueChanged += new System.EventHandler(this.nud_ValueChanged);
            // 
            // txt_LabelType_AICellImg
            // 
            this.txt_LabelType_AICellImg.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txt_LabelType_AICellImg.ForeColor = System.Drawing.Color.White;
            this.txt_LabelType_AICellImg.Location = new System.Drawing.Point(3, 9);
            this.txt_LabelType_AICellImg.Name = "txt_LabelType_AICellImg";
            this.txt_LabelType_AICellImg.ReadOnly = true;
            this.txt_LabelType_AICellImg.Size = new System.Drawing.Size(65, 29);
            this.txt_LabelType_AICellImg.TabIndex = 168;
            this.txt_LabelType_AICellImg.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_LabelType_AICellImg.Visible = false;
            // 
            // DispWindow_AICellImg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 655);
            this.Controls.Add(this.txt_LabelType_AICellImg);
            this.Controls.Add(this.nud_Height);
            this.Controls.Add(this.nud_Width);
            this.Controls.Add(this.label_Height);
            this.Controls.Add(this.label_Width);
            this.Controls.Add(this.hSmartWindowControl_AICellImg);
            this.Controls.Add(this.trackBar_Height);
            this.Controls.Add(this.trackBar_Width);
            this.Name = "DispWindow_AICellImg";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DispWindow_AICellImg_FormClosed);
            this.Load += new System.EventHandler(this.DispWindow_AICellImg_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Width)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Height)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Width)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Height)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private HalconDotNet.HSmartWindowControl hSmartWindowControl_AICellImg;
        private System.Windows.Forms.TrackBar trackBar_Width;
        private System.Windows.Forms.TrackBar trackBar_Height;
        private System.Windows.Forms.Label label_Width;
        private System.Windows.Forms.Label label_Height;
        private System.Windows.Forms.NumericUpDown nud_Width;
        private System.Windows.Forms.NumericUpDown nud_Height;
        private System.Windows.Forms.TextBox txt_LabelType_AICellImg;
    }
}