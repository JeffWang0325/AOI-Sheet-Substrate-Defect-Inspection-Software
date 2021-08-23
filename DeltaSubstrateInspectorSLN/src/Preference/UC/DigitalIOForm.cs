using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Threading;
using gts;

namespace TestingGoogolTech
{
    public partial class Form_Digital_IO : Form
    {
        List<CheckBox> list_di_ = new List<CheckBox>();

        class DigitalOutput
        {
            private int id_;
            private bool b_ischecked_;

            public int get_id()
            {
                return id_;
            }

            public int ID
            {
                get { return id_; }
                set { this.id_ = value; }
            }

            public bool IsChecked
            {
                get { return b_ischecked_; }
                set { this.b_ischecked_ = value; }
            }

            public void check_cheange()
            {
                if (b_ischecked_)
                    mc.GT_SetDoBit(12, (short)id_, 1);
                else
                    mc.GT_SetDoBit(12, (short)id_, 0);
            }
        }
        //*********************************************
        public Form_Digital_IO()
        {
            InitializeComponent();                 
        }

        private void Form_Digital_IO_Load(object sender, EventArgs e)
        {           
            foreach(CheckBox chkBox in this.groupBox_DI.Controls)
            {
                list_di_.Add(chkBox);
            }

            timer1.Start();
        }

        private void digit_output_click(object sender, EventArgs e)
        {
            CheckBox chk = sender as CheckBox;

            string tag = chk.Tag.ToString();
            int bound = tag.Length - 3;
            tag = tag.Substring(3, bound);

            if (chk.Checked)
                mc.GT_SetDoBit(12, (short)Convert.ToInt16(tag), 1);
            else
                mc.GT_SetDoBit(12, (short)Convert.ToInt16(tag), 0);
        }

        /// <summary>
        /// Check Di Signal
        /// </summary>
        private void check_general_di()
        {
            int hex_;
            short status_ = mc.GT_GetDi(4, out hex_);

            if (status_ != 0)
            {
                timer1.Stop();
                MessageBox.Show("指令執行失敗!");                
            }
            else
            {
                string binary_ = Convert.ToString(hex_, 2).PadLeft(16, '0');

                for(int i=1;i<=16;i++)
                {                    
                    string check_status_ = binary_[16-i].ToString();
                    var chkBox = list_di_.Where(x => x.Tag.ToString() == "di_" + i).Single();                    

                    if (check_status_ == "1")
                        chkBox.Checked = true;
                    else if (check_status_ == "0")
                        chkBox.Checked = false;
                    else
                    {
                        timer1.Stop();
                        MessageBox.Show("Status of Digital Input is Error !");                        
                        break;
                    }
                }        
            }
        } 
      

        #region CheckButton of Digital Output - Alarm Clear
        private void chk_alarmclear_1_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_general_do_1.Checked)
                mc.GT_SetDoBit(11, 1, 1);
            else
                mc.GT_SetDoBit(11, 1, 0);
        }

        private void chk_alarmclear_2_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_general_do_2.Checked)
                mc.GT_SetDoBit(11, 2, 1);
            else
                mc.GT_SetDoBit(11, 2, 0);
        }

        private void chk_alarmclear_3_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_general_do_1.Checked)
                mc.GT_SetDoBit(11, 3, 1);
            else
                mc.GT_SetDoBit(11, 3, 0);
        }
        private void chk_alarmclear_4_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_general_do_4.Checked)
                mc.GT_SetDoBit(11, 4, 1);
            else
                mc.GT_SetDoBit(11, 4, 0);
        }
        private void chk_alarmclear_5_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_general_do_5.Checked)
                mc.GT_SetDoBit(11, 5, 1);
            else
                mc.GT_SetDoBit(11, 5, 0);
        }
        private void chk_alarmclear_6_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_general_do_6.Checked)
                mc.GT_SetDoBit(11, 6, 1);
            else
                mc.GT_SetDoBit(11, 6, 0);
        }
        private void chk_alarmclear_7_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_general_do_7.Checked)
                mc.GT_SetDoBit(11, 7, 1);
            else
                mc.GT_SetDoBit(11, 7, 0);
        }
        private void chk_alarmclear_8_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_general_do_8.Checked)
                mc.GT_SetDoBit(11, 8, 1);
            else
                mc.GT_SetDoBit(11, 8, 0);
        }

        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            check_general_di();
        }




        

    }
}
