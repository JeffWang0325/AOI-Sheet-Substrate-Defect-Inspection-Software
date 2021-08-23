namespace DeltaSubstrateInspector.src.InspectionForms.ParamPanels
{
    partial class ParamMaskSetting
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
            this.colorSpaceSetup1 = new DeltaSubstrateInspector.UIControl.ColorSpaceSetup();
            this.SuspendLayout();
            // 
            // colorSpaceSetup1
            // 
            this.colorSpaceSetup1.BackColor = System.Drawing.Color.Transparent;
            this.colorSpaceSetup1.Location = new System.Drawing.Point(3, 2);
            this.colorSpaceSetup1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.colorSpaceSetup1.Name = "colorSpaceSetup1";
            this.colorSpaceSetup1.Size = new System.Drawing.Size(246, 888);
            this.colorSpaceSetup1.TabIndex = 0;
            // 
            // ParamMaskSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Pink;
            this.Controls.Add(this.colorSpaceSetup1);
            this.Name = "ParamMaskSetting";
            this.Size = new System.Drawing.Size(870, 891);
            this.ResumeLayout(false);

        }

        #endregion

        private UIControl.ColorSpaceSetup colorSpaceSetup1;
    }
}
