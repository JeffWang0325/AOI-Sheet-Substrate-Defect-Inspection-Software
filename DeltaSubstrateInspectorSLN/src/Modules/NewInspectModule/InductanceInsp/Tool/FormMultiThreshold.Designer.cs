namespace DeltaSubstrateInspector.src.Modules.NewInspectModule.InductanceInsp
{
    partial class FormMultiThreshold
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
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("Index");
            this.gbxThParam = new System.Windows.Forms.GroupBox();
            this.nudLTh = new System.Windows.Forms.NumericUpDown();
            this.txbImgIndex = new System.Windows.Forms.TextBox();
            this.labHighTH = new System.Windows.Forms.Label();
            this.labImgIndex = new System.Windows.Forms.Label();
            this.btnThSetup = new System.Windows.Forms.Button();
            this.nudHTH = new System.Windows.Forms.NumericUpDown();
            this.labLowTh = new System.Windows.Forms.Label();
            this.bthAdd = new System.Windows.Forms.Button();
            this.bthRemove = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.gbxThList = new System.Windows.Forms.GroupBox();
            this.cbxListMultiTh = new System.Windows.Forms.ListView();
            this.Index = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.comboBoxMultiTHBand = new System.Windows.Forms.ComboBox();
            this.labMultiTHBand = new System.Windows.Forms.Label();
            this.gbxThParam.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLTh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHTH)).BeginInit();
            this.gbxThList.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbxThParam
            // 
            this.gbxThParam.Controls.Add(this.comboBoxMultiTHBand);
            this.gbxThParam.Controls.Add(this.labMultiTHBand);
            this.gbxThParam.Controls.Add(this.nudLTh);
            this.gbxThParam.Controls.Add(this.txbImgIndex);
            this.gbxThParam.Controls.Add(this.labHighTH);
            this.gbxThParam.Controls.Add(this.labImgIndex);
            this.gbxThParam.Controls.Add(this.btnThSetup);
            this.gbxThParam.Controls.Add(this.nudHTH);
            this.gbxThParam.Controls.Add(this.labLowTh);
            this.gbxThParam.Location = new System.Drawing.Point(272, 20);
            this.gbxThParam.Name = "gbxThParam";
            this.gbxThParam.Size = new System.Drawing.Size(231, 277);
            this.gbxThParam.TabIndex = 88;
            this.gbxThParam.TabStop = false;
            this.gbxThParam.Text = "二值化參數設定";
            // 
            // nudLTh
            // 
            this.nudLTh.Font = new System.Drawing.Font("Verdana", 9.75F);
            this.nudLTh.Location = new System.Drawing.Point(145, 156);
            this.nudLTh.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudLTh.Name = "nudLTh";
            this.nudLTh.Size = new System.Drawing.Size(58, 23);
            this.nudLTh.TabIndex = 84;
            this.nudLTh.ValueChanged += new System.EventHandler(this.nudLTh_ValueChanged);
            // 
            // txbImgIndex
            // 
            this.txbImgIndex.Location = new System.Drawing.Point(99, 78);
            this.txbImgIndex.Name = "txbImgIndex";
            this.txbImgIndex.Size = new System.Drawing.Size(104, 25);
            this.txbImgIndex.TabIndex = 9;
            this.txbImgIndex.Text = "0";
            this.txbImgIndex.TextChanged += new System.EventHandler(this.txbImgIndex_TextChanged);
            // 
            // labHighTH
            // 
            this.labHighTH.AutoSize = true;
            this.labHighTH.Location = new System.Drawing.Point(30, 191);
            this.labHighTH.Name = "labHighTH";
            this.labHighTH.Size = new System.Drawing.Size(110, 18);
            this.labHighTH.TabIndex = 85;
            this.labHighTH.Text = "MaxThreshold:";
            // 
            // labImgIndex
            // 
            this.labImgIndex.AutoSize = true;
            this.labImgIndex.Location = new System.Drawing.Point(30, 81);
            this.labImgIndex.Name = "labImgIndex";
            this.labImgIndex.Size = new System.Drawing.Size(63, 18);
            this.labImgIndex.TabIndex = 10;
            this.labImgIndex.Text = "影像ID : ";
            // 
            // btnThSetup
            // 
            this.btnThSetup.Location = new System.Drawing.Point(33, 116);
            this.btnThSetup.Name = "btnThSetup";
            this.btnThSetup.Size = new System.Drawing.Size(170, 32);
            this.btnThSetup.TabIndex = 82;
            this.btnThSetup.Text = "二值化設定";
            this.btnThSetup.UseVisualStyleBackColor = true;
            this.btnThSetup.Click += new System.EventHandler(this.btnThSetup_Click);
            // 
            // nudHTH
            // 
            this.nudHTH.Font = new System.Drawing.Font("Verdana", 9.75F);
            this.nudHTH.Location = new System.Drawing.Point(145, 189);
            this.nudHTH.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudHTH.Name = "nudHTH";
            this.nudHTH.Size = new System.Drawing.Size(58, 23);
            this.nudHTH.TabIndex = 83;
            this.nudHTH.ValueChanged += new System.EventHandler(this.nudHTH_ValueChanged);
            // 
            // labLowTh
            // 
            this.labLowTh.AutoSize = true;
            this.labLowTh.Location = new System.Drawing.Point(30, 158);
            this.labLowTh.Name = "labLowTh";
            this.labLowTh.Size = new System.Drawing.Size(108, 18);
            this.labLowTh.TabIndex = 86;
            this.labLowTh.Text = "MinThreshold:";
            // 
            // bthAdd
            // 
            this.bthAdd.Location = new System.Drawing.Point(24, 261);
            this.bthAdd.Name = "bthAdd";
            this.bthAdd.Size = new System.Drawing.Size(86, 38);
            this.bthAdd.TabIndex = 90;
            this.bthAdd.Text = "新增";
            this.bthAdd.UseVisualStyleBackColor = true;
            this.bthAdd.Click += new System.EventHandler(this.bthAdd_Click);
            // 
            // bthRemove
            // 
            this.bthRemove.Location = new System.Drawing.Point(132, 261);
            this.bthRemove.Name = "bthRemove";
            this.bthRemove.Size = new System.Drawing.Size(86, 38);
            this.bthRemove.TabIndex = 90;
            this.bthRemove.Text = "移除";
            this.bthRemove.UseVisualStyleBackColor = true;
            this.bthRemove.Click += new System.EventHandler(this.bthRemove_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Green;
            this.button1.Location = new System.Drawing.Point(324, 303);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(86, 38);
            this.button1.TabIndex = 90;
            this.button1.Text = "確定";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.button2.Location = new System.Drawing.Point(416, 303);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(86, 38);
            this.button2.TabIndex = 90;
            this.button2.Text = "取消";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // gbxThList
            // 
            this.gbxThList.Controls.Add(this.cbxListMultiTh);
            this.gbxThList.Controls.Add(this.bthRemove);
            this.gbxThList.Controls.Add(this.bthAdd);
            this.gbxThList.Location = new System.Drawing.Point(29, 20);
            this.gbxThList.Name = "gbxThList";
            this.gbxThList.Size = new System.Drawing.Size(237, 321);
            this.gbxThList.TabIndex = 91;
            this.gbxThList.TabStop = false;
            this.gbxThList.Text = "列表";
            // 
            // cbxListMultiTh
            // 
            this.cbxListMultiTh.BackColor = System.Drawing.SystemColors.Control;
            this.cbxListMultiTh.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Index});
            this.cbxListMultiTh.FullRowSelect = true;
            this.cbxListMultiTh.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem2});
            this.cbxListMultiTh.Location = new System.Drawing.Point(6, 22);
            this.cbxListMultiTh.Name = "cbxListMultiTh";
            this.cbxListMultiTh.Size = new System.Drawing.Size(224, 233);
            this.cbxListMultiTh.TabIndex = 91;
            this.cbxListMultiTh.UseCompatibleStateImageBehavior = false;
            this.cbxListMultiTh.View = System.Windows.Forms.View.Details;
            this.cbxListMultiTh.SelectedIndexChanged += new System.EventHandler(this.cbxListMultiTh_SelectedIndexChanged_1);
            // 
            // Index
            // 
            this.Index.Text = "項目";
            this.Index.Width = 200;
            // 
            // comboBoxMultiTHBand
            // 
            this.comboBoxMultiTHBand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMultiTHBand.FormattingEnabled = true;
            this.comboBoxMultiTHBand.Items.AddRange(new object[] {
            "R",
            "G",
            "B",
            "Gray"});
            this.comboBoxMultiTHBand.Location = new System.Drawing.Point(99, 222);
            this.comboBoxMultiTHBand.Name = "comboBoxMultiTHBand";
            this.comboBoxMultiTHBand.Size = new System.Drawing.Size(54, 25);
            this.comboBoxMultiTHBand.TabIndex = 93;
            this.comboBoxMultiTHBand.SelectedIndexChanged += new System.EventHandler(this.comboBoxMultiTHBand_SelectedIndexChanged);
            // 
            // labMultiTHBand
            // 
            this.labMultiTHBand.AutoSize = true;
            this.labMultiTHBand.Location = new System.Drawing.Point(30, 225);
            this.labMultiTHBand.Name = "labMultiTHBand";
            this.labMultiTHBand.Size = new System.Drawing.Size(55, 18);
            this.labMultiTHBand.TabIndex = 92;
            this.labMultiTHBand.Text = "Band : ";
            // 
            // FormMultiThreshold
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(521, 356);
            this.Controls.Add(this.gbxThList);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.gbxThParam);
            this.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormMultiThreshold";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MultiThreshold";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMultiThreshold_FormClosing);
            this.Load += new System.EventHandler(this.FormMultiThreshold_Load);
            this.gbxThParam.ResumeLayout(false);
            this.gbxThParam.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLTh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHTH)).EndInit();
            this.gbxThList.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxThParam;
        private System.Windows.Forms.NumericUpDown nudLTh;
        private System.Windows.Forms.TextBox txbImgIndex;
        private System.Windows.Forms.Label labHighTH;
        private System.Windows.Forms.Label labImgIndex;
        private System.Windows.Forms.Button btnThSetup;
        private System.Windows.Forms.NumericUpDown nudHTH;
        private System.Windows.Forms.Label labLowTh;
        private System.Windows.Forms.Button bthAdd;
        private System.Windows.Forms.Button bthRemove;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox gbxThList;
        private System.Windows.Forms.ListView cbxListMultiTh;
        private System.Windows.Forms.ColumnHeader Index;
        private System.Windows.Forms.ComboBox comboBoxMultiTHBand;
        private System.Windows.Forms.Label labMultiTHBand;
    }
}