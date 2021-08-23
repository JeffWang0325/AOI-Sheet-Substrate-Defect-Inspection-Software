using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Threading;
using System.IO.Ports;
using System.Diagnostics;
using HalconDotNet;
using Extension;
using DeltaSubstrateInspector.src.Modules.MotionModule;

namespace DeltaSubstrateInspector.src.Preference.UC
{
    public partial class GoogolMotionForm : Form
    {
        List<Motion.Position> list_path_xyz_ = new List<Motion.Position>();
        public List<Motion.Position> set_list_path_xyz
        { set { list_path_xyz_ = value; } }

        HTuple circle_row_, circle_col_;

        /// <summary>
        /// 點膠機通訊SerialPort
        /// </summary>
        SerialPort SerialPort_XYZTable = new SerialPort();
        //************************************************************
        public GoogolMotionForm()
        {
            InitializeComponent();

            // 顯示所有ComPort
            foreach (string s in SerialPort.GetPortNames())
                comboBox_comport.Items.Add(s);

            backgroundWorker_display.RunWorkerAsync();

            if(Motion.b_xyz_table_connecting_)
            {
                lab_xyz_connect.BackColor = Color.Green;
                lab_xyz_connect.Text = "Connecting";
            }
        }

        private void GoogolMotionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (backgroundWorker_display.IsBusy)
                backgroundWorker_display.CancelAsync();
        }

        private void backgroundWorker_display_DoWork(object sender, DoWorkEventArgs e)
        {
            if (Motion.b_googol_connecting_)
                backgroundWorker_display.ReportProgress(0);  // Display UI about Googol Motion Status

            HOperatorSet.SetColor(hWindowControl_move.HalconWindow, "red");
            Thread.Sleep(100);
            foreach (Motion.Position i in list_path_xyz_)
            {
                circle_col_ = (int)i.set_x;
                circle_row_ = (int)i.set_y;
                backgroundWorker_display.ReportProgress(50);
                Thread.Sleep(100);
            }

        }
        private void backgroundWorker_display_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 0)
            {
                #region Display UI about Googol Motion Status
                bool b_connect_ = false;
                foreach (Label label_ in this.groupBox_status.Controls.OfType<Label>())
                {
                    bool b_label_ = false;
                    if (label_.Name == "lab_googol_conn")
                    {
                        b_connect_ = Motion.b_googol_connecting_;
                        b_label_ = true;
                    }
                    else if (label_.Name == "lab_axis1_conn")
                    {
                        b_connect_ = Motion.b_axis_connect_[0];
                        b_label_ = true;
                    }
                    else if (label_.Name == "lab_axis2_conn")
                    {
                        b_connect_ = Motion.b_axis_connect_[1];
                        b_label_ = true;
                    }
                    else if (label_.Name == "lab_axis3_conn")
                    {
                        b_connect_ = Motion.b_axis_connect_[2];
                        b_label_ = true;
                    }

                    if (b_label_)
                    {
                        if (b_connect_)
                        {
                            label_.BackColor = Color.Green;
                            label_.Text = "Connecting";
                        }
                        else
                        {
                            label_.BackColor = Color.Red;
                            label_.Text = "Disconnecting";
                        }
                    }
                }
                #endregion
            }

            if (e.ProgressPercentage == 50)
                HOperatorSet.DispCircle(hWindowControl_move.HalconWindow, circle_row_, circle_col_, 3);
        }

        #region TabControl - Googol Motion
        private void btn_connecting_Click(object sender, EventArgs e)
        {
            Motion.googol_connecting();
        }

        private void btn_digitalIO_form_Click(object sender, EventArgs e)
        {
            using (TestingGoogolTech.Form_Digital_IO form_dio_ = new TestingGoogolTech.Form_Digital_IO())
            {
                form_dio_.ShowDialog();
            }
        }

        private void btn_closing_Click(object sender, EventArgs e)
        {
            Motion.googol_closing();
        }

        #region 【Button】設定非軸DAC(ch9~12)
        private void btn_model1_ch_1_Click(object sender, EventArgs e)
        {
            if (Motion.set_unaxis_dac(9, txt_model1_ch_1.Text))
                MessageBox.Show("Setting Value Error!", "Error Value", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        private void btn_model1_ch_2_Click(object sender, EventArgs e)
        {
            if (Motion.set_unaxis_dac(10, txt_model1_ch_2.Text))
                MessageBox.Show("Setting Value Error!", "Error Value", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        private void btn_model1_ch_3_Click(object sender, EventArgs e)
        {
            if (Motion.set_unaxis_dac(11, txt_model1_ch_3.Text))
                MessageBox.Show("Setting Value Error!", "Error Value", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        private void btn_model1_ch_4_Click(object sender, EventArgs e)
        {
            if (Motion.set_unaxis_dac(12, txt_model1_ch_4.Text))
                MessageBox.Show("Setting Value Error!", "Error Value", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        private void btn_setting_sync_voltage_Click(object sender, EventArgs e)
        {
            Motion.set_unaxis_dac(9, txt_sync_voltage.Text);
            Motion.set_unaxis_dac(10, txt_sync_voltage.Text);
            Motion.set_unaxis_dac(11, txt_sync_voltage.Text);
            Motion.set_unaxis_dac(12, txt_sync_voltage.Text);
            txt_model1_ch_1.Text = txt_sync_voltage.Text;
            txt_model1_ch_2.Text = txt_sync_voltage.Text;
            txt_model1_ch_3.Text = txt_sync_voltage.Text;
            txt_model1_ch_4.Text = txt_sync_voltage.Text;
        }
        private void btn_adda_model_initial_Click(object sender, EventArgs e)
        {
            Motion.set_unaxis_dac(9, "0");
            Motion.set_unaxis_dac(10, "0");
            Motion.set_unaxis_dac(11, "0");
            Motion.set_unaxis_dac(12, "0");
            txt_model1_ch_1.Text = "0";
            txt_model1_ch_2.Text = "0";
            txt_model1_ch_3.Text = "0";
            txt_model1_ch_4.Text = "0";
        }
        #endregion

        #endregion

        #region TabControl - XYZ Table
        private void btn_connect_Click(object sender, EventArgs e)
        {
            if(Motion.xyz_connect(comboBox_comport.Text))
            {
                lab_xyz_connect.BackColor = Color.Green;
                lab_xyz_connect.Text = "Connecting";
            }
            else
            {
                lab_xyz_connect.BackColor = Color.Red;
                lab_xyz_connect.Text = "Disconnecting";
            }            
        }

        private void btn_home_Click(object sender, EventArgs e)
        {
            Motion.xyz_home();
        }

        private void btn_close_Click(object sender, EventArgs e)
        {            
            Motion.SerialPort_XYZTable.Close();
            Motion.b_xyz_table_connecting_ = false;
            lab_xyz_connect.BackColor = Color.Red;
            lab_xyz_connect.Text = "Disconnecting";
        }

        private void btn_go_Click(object sender, EventArgs e)
        {
            Motion.xyz_go();
        }

        private void btn_set_Click(object sender, EventArgs e)
        {
            if (Convert.ToDouble(txt_speed.Text) > 0)
                Motion.xyz_set_speed(Convert.ToDouble(txt_speed.Text));
        }

        private void btn_trigger_signal_Click(object sender, EventArgs e)
        {
            if (Convert.ToDouble(txt_signal.Text) > 0)
            {
                int port_ = Convert.ToInt16(txt_port.Text);
                Motion.xyz_io(port_, 1);
                Motion.xyz_wait_time(txt_signal.Text);
                Motion.xyz_io(port_, 0);
            }
        }

        private void btn_run_Click(object sender, EventArgs e)
        {            
            Motion.xyz_home();

            Stopwatch sw_ = new Stopwatch();
            sw_.Start();
            foreach(Motion.Position i in list_path_xyz_)
            {
                Motion.b_xyz_motion_copleting_ = false;
                Motion.xyz_abs_move(i.set_x, i.set_y, i.set_z);

                //Motion.xyz_wait_move_completing();
                while(!Motion.b_xyz_motion_copleting_)
                {
                    Motion.xyz_get_motion_status();
                    Thread.Sleep(10);
                }


                if(Convert.ToDouble(txt_signal.Text)>0)
                {
                    Motion.xyz_io(12, 1);
                    Motion.xyz_wait_time(txt_signal.Text);
                    Motion.xyz_io(12, 0);
                }
            }
            sw_.Stop();
            string tt = sw_.ElapsedMilliseconds.ToString();
        }        
        #endregion



    }
}
