namespace DeltaSubstrateInspector.src.SystemSource.WorkSheet
{
    partial class WorkSheetForm
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
            this.btn_new = new System.Windows.Forms.Button();
            this.textBox_NewRecipeName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_new
            // 
            this.btn_new.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(166)))), ((int)(((byte)(137)))));
            this.btn_new.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(166)))), ((int)(((byte)(137)))));
            this.btn_new.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_new.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_new.Location = new System.Drawing.Point(510, 18);
            this.btn_new.Margin = new System.Windows.Forms.Padding(2);
            this.btn_new.Name = "btn_new";
            this.btn_new.Size = new System.Drawing.Size(69, 28);
            this.btn_new.TabIndex = 9;
            this.btn_new.Text = "確定";
            this.btn_new.UseVisualStyleBackColor = false;
            this.btn_new.Click += new System.EventHandler(this.btn_new_Click);
            // 
            // textBox_NewRecipeName
            // 
            this.textBox_NewRecipeName.Font = new System.Drawing.Font("新細明體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox_NewRecipeName.Location = new System.Drawing.Point(92, 16);
            this.textBox_NewRecipeName.Name = "textBox_NewRecipeName";
            this.textBox_NewRecipeName.Size = new System.Drawing.Size(404, 30);
            this.textBox_NewRecipeName.TabIndex = 10;
            this.textBox_NewRecipeName.TextChanged += new System.EventHandler(this.textBox_NewRecipeName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 21);
            this.label1.TabIndex = 11;
            this.label1.Text = "程式名稱";
            // 
            // WorkSheetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 64);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_NewRecipeName);
            this.Controls.Add(this.btn_new);
            this.Name = "WorkSheetForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "設定程式名稱";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_new;
        private System.Windows.Forms.TextBox textBox_NewRecipeName;
        private System.Windows.Forms.Label label1;
    }
}