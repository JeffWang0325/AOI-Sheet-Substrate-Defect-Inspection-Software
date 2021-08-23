namespace DeltaSubstrateInspector.src.Modules.TCPIP4Sorter
{
    partial class FormTCPIPSorter
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
            this.components = new System.ComponentModel.Container();
            this.button1_ClearSendMsg = new System.Windows.Forms.Button();
            this.richTextBox_SendMsg = new System.Windows.Forms.RichTextBox();
            this.button_Save = new System.Windows.Forms.Button();
            this.statusStripMain = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel_Step = new System.Windows.Forms.ToolStripStatusLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button_ClearMsg = new System.Windows.Forms.Button();
            this.richTextBox_Msg = new System.Windows.Forms.RichTextBox();
            this.textBox_Port = new System.Windows.Forms.TextBox();
            this.textBox_IP = new System.Windows.Forms.TextBox();
            this.button_Send = new System.Windows.Forms.Button();
            this.tbResult = new System.Windows.Forms.TextBox();
            this.button_StartListen = new System.Windows.Forms.Button();
            this.timer_MsgShow = new System.Windows.Forms.Timer(this.components);
            this.statusStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1_ClearSendMsg
            // 
            this.button1_ClearSendMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button1_ClearSendMsg.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button1_ClearSendMsg.Location = new System.Drawing.Point(380, 100);
            this.button1_ClearSendMsg.Name = "button1_ClearSendMsg";
            this.button1_ClearSendMsg.Size = new System.Drawing.Size(362, 39);
            this.button1_ClearSendMsg.TabIndex = 45;
            this.button1_ClearSendMsg.Text = "清除傳送訊息";
            this.button1_ClearSendMsg.UseVisualStyleBackColor = true;
            this.button1_ClearSendMsg.Click += new System.EventHandler(this.button1_ClearSendMsg_Click);
            // 
            // richTextBox_SendMsg
            // 
            this.richTextBox_SendMsg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox_SendMsg.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.richTextBox_SendMsg.Location = new System.Drawing.Point(380, 145);
            this.richTextBox_SendMsg.Name = "richTextBox_SendMsg";
            this.richTextBox_SendMsg.Size = new System.Drawing.Size(362, 252);
            this.richTextBox_SendMsg.TabIndex = 44;
            this.richTextBox_SendMsg.Text = "";
            // 
            // button_Save
            // 
            this.button_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Save.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button_Save.Location = new System.Drawing.Point(671, 6);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(75, 39);
            this.button_Save.TabIndex = 43;
            this.button_Save.Text = "儲存";
            this.button_Save.UseVisualStyleBackColor = true;
            this.button_Save.Click += new System.EventHandler(this.button_Save_Click);
            // 
            // statusStripMain
            // 
            this.statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel_Step});
            this.statusStripMain.Location = new System.Drawing.Point(0, 406);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Size = new System.Drawing.Size(754, 22);
            this.statusStripMain.TabIndex = 42;
            this.statusStripMain.Text = "statusStrip1";
            // 
            // toolStripStatusLabel_Step
            // 
            this.toolStripStatusLabel_Step.Name = "toolStripStatusLabel_Step";
            this.toolStripStatusLabel_Step.Size = new System.Drawing.Size(36, 17);
            this.toolStripStatusLabel_Step.Text = "Step:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(296, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 16);
            this.label2.TabIndex = 41;
            this.label2.Text = "Port";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(93, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 16);
            this.label1.TabIndex = 40;
            this.label1.Text = "Address";
            // 
            // button_ClearMsg
            // 
            this.button_ClearMsg.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button_ClearMsg.Location = new System.Drawing.Point(12, 100);
            this.button_ClearMsg.Name = "button_ClearMsg";
            this.button_ClearMsg.Size = new System.Drawing.Size(362, 39);
            this.button_ClearMsg.TabIndex = 39;
            this.button_ClearMsg.Text = "清除接收訊息";
            this.button_ClearMsg.UseVisualStyleBackColor = true;
            this.button_ClearMsg.Click += new System.EventHandler(this.button_ClearMsg_Click);
            // 
            // richTextBox_Msg
            // 
            this.richTextBox_Msg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.richTextBox_Msg.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.richTextBox_Msg.Location = new System.Drawing.Point(12, 145);
            this.richTextBox_Msg.Name = "richTextBox_Msg";
            this.richTextBox_Msg.Size = new System.Drawing.Size(362, 252);
            this.richTextBox_Msg.TabIndex = 38;
            this.richTextBox_Msg.Text = "";
            // 
            // textBox_Port
            // 
            this.textBox_Port.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox_Port.Location = new System.Drawing.Point(335, 16);
            this.textBox_Port.Name = "textBox_Port";
            this.textBox_Port.Size = new System.Drawing.Size(77, 27);
            this.textBox_Port.TabIndex = 37;
            this.textBox_Port.Text = "4001";
            this.textBox_Port.TextChanged += new System.EventHandler(this.textBox_IP_TextChanged);
            // 
            // textBox_IP
            // 
            this.textBox_IP.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox_IP.Location = new System.Drawing.Point(158, 16);
            this.textBox_IP.Name = "textBox_IP";
            this.textBox_IP.Size = new System.Drawing.Size(120, 27);
            this.textBox_IP.TabIndex = 36;
            this.textBox_IP.Text = "127.0.0.1";
            this.textBox_IP.TextChanged += new System.EventHandler(this.textBox_IP_TextChanged);
            // 
            // button_Send
            // 
            this.button_Send.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button_Send.Location = new System.Drawing.Point(12, 55);
            this.button_Send.Name = "button_Send";
            this.button_Send.Size = new System.Drawing.Size(75, 39);
            this.button_Send.TabIndex = 35;
            this.button_Send.Text = "傳送";
            this.button_Send.UseVisualStyleBackColor = true;
            this.button_Send.Click += new System.EventHandler(this.button_Send_Click);
            // 
            // tbResult
            // 
            this.tbResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbResult.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tbResult.Location = new System.Drawing.Point(93, 61);
            this.tbResult.Name = "tbResult";
            this.tbResult.Size = new System.Drawing.Size(653, 27);
            this.tbResult.TabIndex = 34;
            this.tbResult.Text = "test";
            // 
            // button_StartListen
            // 
            this.button_StartListen.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button_StartListen.Location = new System.Drawing.Point(12, 6);
            this.button_StartListen.Name = "button_StartListen";
            this.button_StartListen.Size = new System.Drawing.Size(75, 39);
            this.button_StartListen.TabIndex = 33;
            this.button_StartListen.Text = "開始";
            this.button_StartListen.UseVisualStyleBackColor = true;
            this.button_StartListen.Click += new System.EventHandler(this.button_StartListen_Click);
            // 
            // timer_MsgShow
            // 
            this.timer_MsgShow.Interval = 500;
            this.timer_MsgShow.Tick += new System.EventHandler(this.timer_MsgShow_Tick);
            // 
            // FormTCPIPSorter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 428);
            this.Controls.Add(this.button1_ClearSendMsg);
            this.Controls.Add(this.richTextBox_SendMsg);
            this.Controls.Add(this.button_Save);
            this.Controls.Add(this.statusStripMain);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_ClearMsg);
            this.Controls.Add(this.richTextBox_Msg);
            this.Controls.Add(this.textBox_Port);
            this.Controls.Add(this.textBox_IP);
            this.Controls.Add(this.button_Send);
            this.Controls.Add(this.tbResult);
            this.Controls.Add(this.button_StartListen);
            this.Name = "FormTCPIPSorter";
            this.Text = "FormTCPIPSorter";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormTCPIPSorter_FormClosing);
            this.Load += new System.EventHandler(this.FormTCPIPSorter_Load);
            this.VisibleChanged += new System.EventHandler(this.FormTCPIPSorter_VisibleChanged);
            this.statusStripMain.ResumeLayout(false);
            this.statusStripMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1_ClearSendMsg;
        private System.Windows.Forms.RichTextBox richTextBox_SendMsg;
        private System.Windows.Forms.Button button_Save;
        private System.Windows.Forms.StatusStrip statusStripMain;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_Step;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_ClearMsg;
        private System.Windows.Forms.RichTextBox richTextBox_Msg;
        private System.Windows.Forms.TextBox textBox_Port;
        private System.Windows.Forms.TextBox textBox_IP;
        private System.Windows.Forms.Button button_Send;
        private System.Windows.Forms.TextBox tbResult;
        private System.Windows.Forms.Button button_StartListen;
        private System.Windows.Forms.Timer timer_MsgShow;
    }
}