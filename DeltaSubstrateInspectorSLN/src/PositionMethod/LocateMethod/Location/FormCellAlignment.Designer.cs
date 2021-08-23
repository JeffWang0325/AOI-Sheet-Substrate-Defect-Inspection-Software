namespace DeltaSubstrateInspector.src.PositionMethod.LocateMethod.Location
{
    partial class FormCellAlignment
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
            this.hSmartWindowControl_map = new HalconDotNet.HSmartWindowControl();
            this.panelDisplay = new System.Windows.Forms.Panel();
            this.cbxLeftTopRegion = new System.Windows.Forms.CheckBox();
            this.gbxSettingRegions = new System.Windows.Forms.GroupBox();
            this.panelProduct = new System.Windows.Forms.Panel();
            this.btnDisplayRegions = new System.Windows.Forms.Button();
            this.cbxRightBot = new System.Windows.Forms.CheckBox();
            this.cbxRightTop = new System.Windows.Forms.CheckBox();
            this.cbxLeftBot = new System.Windows.Forms.CheckBox();
            this.cbxEnabled = new System.Windows.Forms.CheckBox();
            this.gbxMatchSetting = new System.Windows.Forms.GroupBox();
            this.cbxTeachRegionSetting = new System.Windows.Forms.CheckBox();
            this.btnMatchingTest = new System.Windows.Forms.Button();
            this.panelDisplay.SuspendLayout();
            this.gbxSettingRegions.SuspendLayout();
            this.gbxMatchSetting.SuspendLayout();
            this.SuspendLayout();
            // 
            // hSmartWindowControl_map
            // 
            this.hSmartWindowControl_map.AllowDrop = true;
            this.hSmartWindowControl_map.AutoScroll = true;
            this.hSmartWindowControl_map.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.hSmartWindowControl_map.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.hSmartWindowControl_map.BackColor = System.Drawing.SystemColors.HotTrack;
            this.hSmartWindowControl_map.HDoubleClickToFitContent = true;
            this.hSmartWindowControl_map.HDrawingObjectsModifier = HalconDotNet.HSmartWindowControl.DrawingObjectsModifier.None;
            this.hSmartWindowControl_map.HImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hSmartWindowControl_map.HKeepAspectRatio = false;
            this.hSmartWindowControl_map.HMoveContent = true;
            this.hSmartWindowControl_map.HZoomContent = HalconDotNet.HSmartWindowControl.ZoomContent.WheelBackwardZoomsIn;
            this.hSmartWindowControl_map.Location = new System.Drawing.Point(3, 3);
            this.hSmartWindowControl_map.Margin = new System.Windows.Forms.Padding(0);
            this.hSmartWindowControl_map.Name = "hSmartWindowControl_map";
            this.hSmartWindowControl_map.Size = new System.Drawing.Size(942, 882);
            this.hSmartWindowControl_map.TabIndex = 2;
            this.hSmartWindowControl_map.WindowSize = new System.Drawing.Size(942, 882);
            // 
            // panelDisplay
            // 
            this.panelDisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelDisplay.Controls.Add(this.hSmartWindowControl_map);
            this.panelDisplay.Location = new System.Drawing.Point(12, 12);
            this.panelDisplay.Name = "panelDisplay";
            this.panelDisplay.Padding = new System.Windows.Forms.Padding(3);
            this.panelDisplay.Size = new System.Drawing.Size(950, 890);
            this.panelDisplay.TabIndex = 3;
            // 
            // cbxLeftTopRegion
            // 
            this.cbxLeftTopRegion.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbxLeftTopRegion.BackColor = System.Drawing.Color.LightGray;
            this.cbxLeftTopRegion.Location = new System.Drawing.Point(6, 24);
            this.cbxLeftTopRegion.MaximumSize = new System.Drawing.Size(124, 32);
            this.cbxLeftTopRegion.Name = "cbxLeftTopRegion";
            this.cbxLeftTopRegion.Size = new System.Drawing.Size(124, 32);
            this.cbxLeftTopRegion.TabIndex = 4;
            this.cbxLeftTopRegion.Text = "左上";
            this.cbxLeftTopRegion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbxLeftTopRegion.UseVisualStyleBackColor = false;
            this.cbxLeftTopRegion.CheckedChanged += new System.EventHandler(this.cbxLeftTopRegion_CheckedChanged);
            // 
            // gbxSettingRegions
            // 
            this.gbxSettingRegions.Controls.Add(this.panelProduct);
            this.gbxSettingRegions.Controls.Add(this.btnDisplayRegions);
            this.gbxSettingRegions.Controls.Add(this.cbxRightBot);
            this.gbxSettingRegions.Controls.Add(this.cbxRightTop);
            this.gbxSettingRegions.Controls.Add(this.cbxLeftBot);
            this.gbxSettingRegions.Controls.Add(this.cbxLeftTopRegion);
            this.gbxSettingRegions.Location = new System.Drawing.Point(968, 45);
            this.gbxSettingRegions.Name = "gbxSettingRegions";
            this.gbxSettingRegions.Size = new System.Drawing.Size(314, 221);
            this.gbxSettingRegions.TabIndex = 5;
            this.gbxSettingRegions.TabStop = false;
            this.gbxSettingRegions.Text = "範圍設定";
            // 
            // panelProduct
            // 
            this.panelProduct.BackColor = System.Drawing.Color.Yellow;
            this.panelProduct.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelProduct.Location = new System.Drawing.Point(98, 62);
            this.panelProduct.Name = "panelProduct";
            this.panelProduct.Size = new System.Drawing.Size(119, 78);
            this.panelProduct.TabIndex = 5;
            // 
            // btnDisplayRegions
            // 
            this.btnDisplayRegions.Location = new System.Drawing.Point(6, 184);
            this.btnDisplayRegions.Name = "btnDisplayRegions";
            this.btnDisplayRegions.Size = new System.Drawing.Size(302, 30);
            this.btnDisplayRegions.TabIndex = 7;
            this.btnDisplayRegions.Text = "顯示範圍";
            this.btnDisplayRegions.UseVisualStyleBackColor = true;
            this.btnDisplayRegions.Click += new System.EventHandler(this.btnDisplayRegions_Click);
            // 
            // cbxRightBot
            // 
            this.cbxRightBot.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbxRightBot.BackColor = System.Drawing.Color.LightGray;
            this.cbxRightBot.Location = new System.Drawing.Point(184, 146);
            this.cbxRightBot.MaximumSize = new System.Drawing.Size(124, 32);
            this.cbxRightBot.Name = "cbxRightBot";
            this.cbxRightBot.Size = new System.Drawing.Size(124, 32);
            this.cbxRightBot.TabIndex = 4;
            this.cbxRightBot.Text = "右下";
            this.cbxRightBot.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbxRightBot.UseVisualStyleBackColor = false;
            this.cbxRightBot.CheckedChanged += new System.EventHandler(this.cbxRightBot_CheckedChanged);
            // 
            // cbxRightTop
            // 
            this.cbxRightTop.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbxRightTop.BackColor = System.Drawing.Color.LightGray;
            this.cbxRightTop.Location = new System.Drawing.Point(184, 24);
            this.cbxRightTop.MaximumSize = new System.Drawing.Size(124, 32);
            this.cbxRightTop.Name = "cbxRightTop";
            this.cbxRightTop.Size = new System.Drawing.Size(124, 32);
            this.cbxRightTop.TabIndex = 4;
            this.cbxRightTop.Text = "右上";
            this.cbxRightTop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbxRightTop.UseVisualStyleBackColor = false;
            this.cbxRightTop.CheckedChanged += new System.EventHandler(this.cbxRightTop_CheckedChanged);
            // 
            // cbxLeftBot
            // 
            this.cbxLeftBot.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbxLeftBot.BackColor = System.Drawing.Color.LightGray;
            this.cbxLeftBot.Location = new System.Drawing.Point(6, 146);
            this.cbxLeftBot.MaximumSize = new System.Drawing.Size(124, 32);
            this.cbxLeftBot.Name = "cbxLeftBot";
            this.cbxLeftBot.Size = new System.Drawing.Size(124, 32);
            this.cbxLeftBot.TabIndex = 4;
            this.cbxLeftBot.Text = "左下";
            this.cbxLeftBot.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbxLeftBot.UseVisualStyleBackColor = false;
            this.cbxLeftBot.CheckedChanged += new System.EventHandler(this.cbxLeftBot_CheckedChanged);
            // 
            // cbxEnabled
            // 
            this.cbxEnabled.AutoSize = true;
            this.cbxEnabled.Location = new System.Drawing.Point(969, 12);
            this.cbxEnabled.Name = "cbxEnabled";
            this.cbxEnabled.Size = new System.Drawing.Size(55, 22);
            this.cbxEnabled.TabIndex = 6;
            this.cbxEnabled.Text = "啟用";
            this.cbxEnabled.UseVisualStyleBackColor = true;
            this.cbxEnabled.CheckedChanged += new System.EventHandler(this.cbxEnabled_CheckedChanged);
            // 
            // gbxMatchSetting
            // 
            this.gbxMatchSetting.Controls.Add(this.btnMatchingTest);
            this.gbxMatchSetting.Controls.Add(this.cbxTeachRegionSetting);
            this.gbxMatchSetting.Location = new System.Drawing.Point(969, 272);
            this.gbxMatchSetting.Name = "gbxMatchSetting";
            this.gbxMatchSetting.Size = new System.Drawing.Size(313, 174);
            this.gbxMatchSetting.TabIndex = 8;
            this.gbxMatchSetting.TabStop = false;
            this.gbxMatchSetting.Text = "圖樣教導";
            // 
            // cbxTeachRegionSetting
            // 
            this.cbxTeachRegionSetting.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbxTeachRegionSetting.BackColor = System.Drawing.Color.LightGray;
            this.cbxTeachRegionSetting.Location = new System.Drawing.Point(6, 24);
            this.cbxTeachRegionSetting.MaximumSize = new System.Drawing.Size(124, 32);
            this.cbxTeachRegionSetting.Name = "cbxTeachRegionSetting";
            this.cbxTeachRegionSetting.Size = new System.Drawing.Size(124, 32);
            this.cbxTeachRegionSetting.TabIndex = 4;
            this.cbxTeachRegionSetting.Text = "範圍設定";
            this.cbxTeachRegionSetting.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbxTeachRegionSetting.UseVisualStyleBackColor = false;
            this.cbxTeachRegionSetting.CheckedChanged += new System.EventHandler(this.cbxTeachRegionSetting_CheckedChanged);
            // 
            // btnMatchingTest
            // 
            this.btnMatchingTest.Location = new System.Drawing.Point(136, 24);
            this.btnMatchingTest.Name = "btnMatchingTest";
            this.btnMatchingTest.Size = new System.Drawing.Size(124, 32);
            this.btnMatchingTest.TabIndex = 5;
            this.btnMatchingTest.Text = "定位測試";
            this.btnMatchingTest.UseVisualStyleBackColor = true;
            this.btnMatchingTest.Click += new System.EventHandler(this.btnMatchingTest_Click);
            // 
            // FormCellAlignment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1687, 915);
            this.Controls.Add(this.gbxMatchSetting);
            this.Controls.Add(this.cbxEnabled);
            this.Controls.Add(this.gbxSettingRegions);
            this.Controls.Add(this.panelDisplay);
            this.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormCellAlignment";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormCellAlignment";
            this.panelDisplay.ResumeLayout(false);
            this.gbxSettingRegions.ResumeLayout(false);
            this.gbxMatchSetting.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private HalconDotNet.HSmartWindowControl hSmartWindowControl_map;
        private System.Windows.Forms.Panel panelDisplay;
        private System.Windows.Forms.CheckBox cbxLeftTopRegion;
        private System.Windows.Forms.GroupBox gbxSettingRegions;
        private System.Windows.Forms.Panel panelProduct;
        private System.Windows.Forms.CheckBox cbxRightBot;
        private System.Windows.Forms.CheckBox cbxRightTop;
        private System.Windows.Forms.CheckBox cbxLeftBot;
        private System.Windows.Forms.CheckBox cbxEnabled;
        private System.Windows.Forms.Button btnDisplayRegions;
        private System.Windows.Forms.GroupBox gbxMatchSetting;
        private System.Windows.Forms.CheckBox cbxTeachRegionSetting;
        private System.Windows.Forms.Button btnMatchingTest;
    }
}