namespace DeltaSubstrateInspector.src.Modules.EnginerrModule
{
    partial class EnginerForm
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
            this.button2 = new System.Windows.Forms.Button();
            this.checkBox_RefMainPCRecipe = new System.Windows.Forms.CheckBox();
            this.checkBox_IgnorePosAndIns = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 56);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 42;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // checkBox_RefMainPCRecipe
            // 
            this.checkBox_RefMainPCRecipe.AutoSize = true;
            this.checkBox_RefMainPCRecipe.Location = new System.Drawing.Point(12, 34);
            this.checkBox_RefMainPCRecipe.Name = "checkBox_RefMainPCRecipe";
            this.checkBox_RefMainPCRecipe.Size = new System.Drawing.Size(104, 16);
            this.checkBox_RefMainPCRecipe.TabIndex = 41;
            this.checkBox_RefMainPCRecipe.Text = "參考主機Recipe";
            this.checkBox_RefMainPCRecipe.UseVisualStyleBackColor = true;
            // 
            // checkBox_IgnorePosAndIns
            // 
            this.checkBox_IgnorePosAndIns.AutoSize = true;
            this.checkBox_IgnorePosAndIns.Location = new System.Drawing.Point(12, 12);
            this.checkBox_IgnorePosAndIns.Name = "checkBox_IgnorePosAndIns";
            this.checkBox_IgnorePosAndIns.Size = new System.Drawing.Size(105, 16);
            this.checkBox_IgnorePosAndIns.TabIndex = 40;
            this.checkBox_IgnorePosAndIns.Text = "IgnorePosAndIns";
            this.checkBox_IgnorePosAndIns.UseVisualStyleBackColor = true;
            // 
            // EnginerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 462);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.checkBox_RefMainPCRecipe);
            this.Controls.Add(this.checkBox_IgnorePosAndIns);
            this.Name = "EnginerForm";
            this.Text = "工程設定";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox checkBox_RefMainPCRecipe;
        private System.Windows.Forms.CheckBox checkBox_IgnorePosAndIns;
    }
}