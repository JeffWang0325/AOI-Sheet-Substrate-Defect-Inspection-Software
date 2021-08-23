using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using DeltaTCP;

namespace DeltaSubstrateInspector.src.Modules.TCPIP4Sorter
{
    public partial class FormTCPIPSorter : Form
    {
        public MesServer dMes = new MesServer();

        public FormTCPIPSorter()
        {
            InitializeComponent();
        }

        public delegate void TextChangedHandler(object sender, System.EventArgs e); //declare the delegate
        public event TextChangedHandler TxtChanged; // declare the event
        protected virtual void OnTextChanged(object sender, System.EventArgs e)
        {
            if (TxtChanged != null)
            {
                TxtChanged(sender, e);
            }
        }

        private void FormTCPIPSorter_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void FormTCPIPSorter_Load(object sender, EventArgs e)
        {
            this.TopMost = true;

        }

        // Main form use
        public void SetIPandPort(string IP, string Port)
        {
            textBox_IP.Text = IP;
            textBox_Port.Text = Port;
        }

        private void FormTCPIPSorter_VisibleChanged(object sender, EventArgs e)
        {
            timer_MsgShow.Enabled = this.Visible;
        }

        private void textBox_IP_TextChanged(object sender, EventArgs e)
        {
            OnTextChanged(sender, e);
        }

        private void button_ClearMsg_Click(object sender, EventArgs e)
        {
            richTextBox_Msg.Clear();
        }

        private void button_StartListen_Click(object sender, EventArgs e)
        {

            dMes.Start(textBox_IP.Text, Int16.Parse(textBox_Port.Text));
        }

        private void button_Send_Click(object sender, EventArgs e)
        {
            dMes.DirectSendResult(tbResult.Text);
        }

        private void timer_MsgShow_Tick(object sender, EventArgs e)
        {
            // Server ReciveData Show            
            richTextBoxUpdateByList(ref richTextBox_Msg, ref dMes.MsgList);

            // Server SendData Show           
            richTextBoxUpdateByList(ref richTextBox_SendMsg, ref dMes.SendMsgList);

            // Step show
            string StepStr = "[Step " + dMes.ListenStep.ToString() + "] ";
            switch (dMes.ListenStep)
            {
                case 10:
                    toolStripStatusLabel_Step.BackColor = Color.Red;
                    toolStripStatusLabel_Step.Text = StepStr + "Ethrenet error.....";
                    break;

                case 20:
                    toolStripStatusLabel_Step.BackColor = Color.Gold;
                    toolStripStatusLabel_Step.Text = StepStr + "Waitting for a connection.....";
                    break;

                case 30:
                    toolStripStatusLabel_Step.BackColor = Color.Lime;
                    toolStripStatusLabel_Step.Text = StepStr + "Connection accepted from " + dMes.ClientIPInfo;
                    break;

                case 40:
                    toolStripStatusLabel_Step.BackColor = Color.SkyBlue;
                    toolStripStatusLabel_Step.Text = StepStr + "String analyzing.....";
                    break;

                default:
                    toolStripStatusLabel_Step.BackColor = SystemColors.Control;
                    toolStripStatusLabel_Step.Text = StepStr;
                    break;

            }
        }

        public delegate void SaveClickHandler(object sender, System.EventArgs e); //declare the delegate
        public event SaveClickHandler SaveClick; // declare the event
        protected virtual void OnSaveCliek(object sender, System.EventArgs e)
        {
            if (SaveClick != null)
            {
                SaveClick(sender, e);
            }
        }
        private void button_Save_Click(object sender, EventArgs e)
        {
            OnSaveCliek(sender, e);
        }

        private void button1_ClearSendMsg_Click(object sender, EventArgs e)
        {
            richTextBox_SendMsg.Clear();
        }

        private void richTextBoxUpdateByList(ref RichTextBox showTB, ref List<string> showList)
        {
            if (showList == null) return;

            // Data Show
            if (showList.Count > 0)
            {
                for (int i = 0; i < showList.Count; i++)
                {
                    string msg = showList[i];
                    showTB.AppendText(msg + "\n");
                }
                showTB.SelectionStart = showTB.Text.Length;
                showTB.ScrollToCaret();
                showList.Clear();
                if (showTB.Lines.Length > 1000) showTB.Clear();
            }
        }


    }
}
