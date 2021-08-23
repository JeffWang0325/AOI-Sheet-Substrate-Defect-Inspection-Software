namespace DeltaSubstrateInspector.src.Modules.NewInspectModule.DefaultInspect
{
    partial class DefaultInspectUC
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
            this.hWindowControl_InputImage = new HalconDotNet.HSmartWindowControl();
            this.checkBox_InspectBypass = new System.Windows.Forms.CheckBox();
            this.button_SaveParam = new System.Windows.Forms.Button();
            this.button_Inspection = new System.Windows.Forms.Button();
            this.button_LoadSingleImage = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_Add
            // 
            this.button_Add.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(166)))), ((int)(((byte)(137)))));
            this.button_Add.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.button_Add.Location = new System.Drawing.Point(3, 3);
            this.button_Add.Name = "button_Add";
            this.button_Add.Size = new System.Drawing.Size(251, 47);
            this.button_Add.TabIndex = 64;
            this.button_Add.Text = "新增";
            this.button_Add.UseVisualStyleBackColor = false;
            this.button_Add.Click += new System.EventHandler(this.button_Add_Click);
            // 
            // hWindowControl_InputImage
            // 
            this.hWindowControl_InputImage.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.hWindowControl_InputImage.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.hWindowControl_InputImage.HDoubleClickToFitContent = true;
            this.hWindowControl_InputImage.HDrawingObjectsModifier = HalconDotNet.HSmartWindowControl.DrawingObjectsModifier.None;
            this.hWindowControl_InputImage.HImagePart = new System.Drawing.Rectangle(0, 0, 4096, 2160);
            this.hWindowControl_InputImage.HKeepAspectRatio = false;
            this.hWindowControl_InputImage.HMoveContent = true;
            this.hWindowControl_InputImage.HZoomContent = HalconDotNet.HSmartWindowControl.ZoomContent.WheelBackwardZoomsIn;
            this.hWindowControl_InputImage.Location = new System.Drawing.Point(3, 53);
            this.hWindowControl_InputImage.Margin = new System.Windows.Forms.Padding(0);
            this.hWindowControl_InputImage.Name = "hWindowControl_InputImage";
            this.hWindowControl_InputImage.Size = new System.Drawing.Size(859, 653);
            this.hWindowControl_InputImage.TabIndex = 70;
            this.hWindowControl_InputImage.WindowSize = new System.Drawing.Size(859, 653);
            // 
            // checkBox_InspectBypass
            // 
            this.checkBox_InspectBypass.AutoSize = true;
            this.checkBox_InspectBypass.Checked = true;
            this.checkBox_InspectBypass.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_InspectBypass.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.checkBox_InspectBypass.Location = new System.Drawing.Point(960, 139);
            this.checkBox_InspectBypass.Name = "checkBox_InspectBypass";
            this.checkBox_InspectBypass.Size = new System.Drawing.Size(142, 23);
            this.checkBox_InspectBypass.TabIndex = 71;
            this.checkBox_InspectBypass.Text = "忽略所有檢測";
            this.checkBox_InspectBypass.UseVisualStyleBackColor = true;
            // 
            // button_SaveParam
            // 
            this.button_SaveParam.Location = new System.Drawing.Point(876, 53);
            this.button_SaveParam.Name = "button_SaveParam";
            this.button_SaveParam.Size = new System.Drawing.Size(110, 47);
            this.button_SaveParam.TabIndex = 72;
            this.button_SaveParam.Text = "參數儲存";
            this.button_SaveParam.UseVisualStyleBackColor = true;
            this.button_SaveParam.Click += new System.EventHandler(this.button_SaveParam_Click);
            // 
            // button_Inspection
            // 
            this.button_Inspection.BackColor = System.Drawing.Color.SandyBrown;
            this.button_Inspection.Location = new System.Drawing.Point(992, 53);
            this.button_Inspection.Name = "button_Inspection";
            this.button_Inspection.Size = new System.Drawing.Size(110, 47);
            this.button_Inspection.TabIndex = 73;
            this.button_Inspection.Text = "檢測";
            this.button_Inspection.UseVisualStyleBackColor = false;
            this.button_Inspection.Click += new System.EventHandler(this.button_Inspection_Click);
            // 
            // button_LoadSingleImage
            // 
            this.button_LoadSingleImage.Location = new System.Drawing.Point(752, 3);
            this.button_LoadSingleImage.Name = "button_LoadSingleImage";
            this.button_LoadSingleImage.Size = new System.Drawing.Size(110, 47);
            this.button_LoadSingleImage.TabIndex = 74;
            this.button_LoadSingleImage.Text = "載入影像";
            this.button_LoadSingleImage.UseVisualStyleBackColor = true;
            this.button_LoadSingleImage.Click += new System.EventHandler(this.button_LoadSingleImage_Click);
            // 
            // DefaultInspectUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.button_LoadSingleImage);
            this.Controls.Add(this.button_Inspection);
            this.Controls.Add(this.button_SaveParam);
            this.Controls.Add(this.checkBox_InspectBypass);
            this.Controls.Add(this.hWindowControl_InputImage);
            this.Controls.Add(this.button_Add);
            this.Name = "DefaultInspectUC";
            this.Size = new System.Drawing.Size(1150, 860);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_Add;
        private HalconDotNet.HSmartWindowControl hWindowControl_InputImage;
        private System.Windows.Forms.CheckBox checkBox_InspectBypass;
        private System.Windows.Forms.Button button_SaveParam;
        private System.Windows.Forms.Button button_Inspection;
        private System.Windows.Forms.Button button_LoadSingleImage;
    }
}
