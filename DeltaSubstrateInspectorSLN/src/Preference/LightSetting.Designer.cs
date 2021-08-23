namespace DeltaSubstrateInspector.src.MainSetting
{
    partial class LightSetting
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbl_title = new System.Windows.Forms.Label();
            this.pnl_light_param = new System.Windows.Forms.Panel();
            this.btn_new = new System.Windows.Forms.Button();
            this.btn_remove = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.panel1.Controls.Add(this.lbl_title);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1190, 64);
            this.panel1.TabIndex = 1;
            // 
            // lbl_title
            // 
            this.lbl_title.AutoSize = true;
            this.lbl_title.Font = new System.Drawing.Font("微軟正黑體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_title.ForeColor = System.Drawing.Color.White;
            this.lbl_title.Location = new System.Drawing.Point(18, 13);
            this.lbl_title.Name = "lbl_title";
            this.lbl_title.Size = new System.Drawing.Size(301, 35);
            this.lbl_title.TabIndex = 1;
            this.lbl_title.Text = "光源設定 Light Setting";
            // 
            // pnl_light_param
            // 
            this.pnl_light_param.AutoScroll = true;
            this.pnl_light_param.BackColor = System.Drawing.Color.White;
            this.pnl_light_param.Location = new System.Drawing.Point(23, 79);
            this.pnl_light_param.Name = "pnl_light_param";
            this.pnl_light_param.Size = new System.Drawing.Size(944, 585);
            this.pnl_light_param.TabIndex = 2;
            // 
            // btn_new
            // 
            this.btn_new.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(166)))), ((int)(((byte)(137)))));
            this.btn_new.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(166)))), ((int)(((byte)(137)))));
            this.btn_new.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_new.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_new.Location = new System.Drawing.Point(984, 79);
            this.btn_new.Name = "btn_new";
            this.btn_new.Size = new System.Drawing.Size(186, 42);
            this.btn_new.TabIndex = 3;
            this.btn_new.Text = "+ Add Light";
            this.btn_new.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_new.UseVisualStyleBackColor = false;
            this.btn_new.Click += new System.EventHandler(this.btn_new_Click);
            // 
            // btn_remove
            // 
            this.btn_remove.BackColor = System.Drawing.Color.DarkGray;
            this.btn_remove.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btn_remove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_remove.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_remove.Location = new System.Drawing.Point(984, 140);
            this.btn_remove.Name = "btn_remove";
            this.btn_remove.Size = new System.Drawing.Size(186, 42);
            this.btn_remove.TabIndex = 4;
            this.btn_remove.Text = "- Remove Light";
            this.btn_remove.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_remove.UseVisualStyleBackColor = false;
            // 
            // LightSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Controls.Add(this.btn_remove);
            this.Controls.Add(this.btn_new);
            this.Controls.Add(this.pnl_light_param);
            this.Controls.Add(this.panel1);
            this.Name = "LightSetting";
            this.Size = new System.Drawing.Size(1190, 682);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbl_title;
        private System.Windows.Forms.Panel pnl_light_param;
        private System.Windows.Forms.Button btn_new;
        private System.Windows.Forms.Button btn_remove;
    }
}
