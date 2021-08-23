namespace Risitanse_AOI.Inspection
{
    partial class InspectionSettingPanel
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
            this.grid_control_btns = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.grid_control_btns.SuspendLayout();
            this.SuspendLayout();
            // 
            // grid_control_btns
            // 
            this.grid_control_btns.Controls.Add(this.button3);
            this.grid_control_btns.Controls.Add(this.button2);
            this.grid_control_btns.Controls.Add(this.button1);
            this.grid_control_btns.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.grid_control_btns.Location = new System.Drawing.Point(3, 3);
            this.grid_control_btns.Name = "grid_control_btns";
            this.grid_control_btns.Size = new System.Drawing.Size(313, 186);
            this.grid_control_btns.TabIndex = 0;
            this.grid_control_btns.TabStop = false;
            this.grid_control_btns.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(42, 22);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(225, 46);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(42, 74);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(225, 46);
            this.button2.TabIndex = 1;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(42, 126);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(225, 46);
            this.button3.TabIndex = 2;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // InspectionSettingPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grid_control_btns);
            this.Name = "InspectionSettingPanel";
            this.Size = new System.Drawing.Size(319, 728);
            this.grid_control_btns.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grid_control_btns;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
    }
}
