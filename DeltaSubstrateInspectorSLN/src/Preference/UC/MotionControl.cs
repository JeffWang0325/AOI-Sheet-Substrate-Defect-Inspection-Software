using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Threading;
using DeltaSubstrateInspector.src.Modules.MotionModule;

namespace DeltaSubstrateInspector.src.Preference.UC
{
    public partial class MotionControl : UserControl
    {
        string actual_p_1_, profile_p_1_;
        string actual_p_2_, profile_p_2_;
        string actual_p_3_, profile_p_3_;

        string xyz_x_, xyz_y_, xyz_z_;
        //*************************************************************************************************

        public MotionControl()
        {
            InitializeComponent();

            #region 紅色中心線(動態)
            label_v.Location = new Point(hWindowControl_Cam.Size.Width /2 - hWindowControl_Cam.Location.X, hWindowControl_Cam.Location.Y);
            label_v.Size = new System.Drawing.Size(1, hWindowControl_Cam.Size.Height);
            label_h.Location = new Point(hWindowControl_Cam.Location.X, hWindowControl_Cam.Size.Height / 2 -  hWindowControl_Cam.Location.Y);
            label_h.Size = new System.Drawing.Size(hWindowControl_Cam.Size.Width, 1);
            #endregion

            backgroundWorker_motion.RunWorkerAsync();
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            int step_ = dataGridView_step.Rows.Count-1;
            dataGridView_step.Rows.Add(step_, txt_cp_x.Text, txt_cp_y.Text, txt_cp_z.Text, Motion.vel_);
        }

        private void btn_set_Click(object sender, EventArgs e)
        {
            List<Motion.Position> list_path_xyz_ = new List<Motion.Position>();

            if (dataGridView_step.RowCount > 1)
            {
                for (int i = 0; i < dataGridView_step.RowCount - 1; i++)
                {
                    float x_ = Convert.ToSingle(dataGridView_step.Rows[i].Cells[1].Value.ToString());
                    float y_ = Convert.ToSingle(dataGridView_step.Rows[i].Cells[2].Value.ToString());
                    float z_ = Convert.ToSingle(dataGridView_step.Rows[i].Cells[3].Value.ToString());
                    list_path_xyz_.Add(new Motion.Position(x_, y_, z_));
                }
            }

            using (GoogolMotionForm googolmotionform = new GoogolMotionForm())
            {
                googolmotionform.set_list_path_xyz = list_path_xyz_;
                googolmotionform.ShowDialog();                
            }
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            Motion.stop_all_motion();
        }

        #region TabControl - Jog
        private void btn_right_MouseDown(object sender, MouseEventArgs e)
        {
            if (Motion.b_googol_connecting_)
                Motion.jogMotion(1, true);
            else if (Motion.b_xyz_table_connecting_)
                Motion.xyz_jog_move(1, 0);
        }
        private void btn_left_MouseDown(object sender, MouseEventArgs e)
        {           
            if (Motion.b_googol_connecting_)
                Motion.jogMotion(1, false);
            else if (Motion.b_xyz_table_connecting_)
                Motion.xyz_jog_move(1,1);
        }
        private void btn_up_MouseDown(object sender, MouseEventArgs e)
        {
            if (Motion.b_googol_connecting_)
                Motion.jogMotion(2, true);
            else if (Motion.b_xyz_table_connecting_)
                Motion.xyz_jog_move(2, 1);            
        }
        private void btn_down_MouseDown(object sender, MouseEventArgs e)
        {
            if (Motion.b_googol_connecting_)
                Motion.jogMotion(2, false);
            else if (Motion.b_xyz_table_connecting_)
                Motion.xyz_jog_move(2, 0);            
        }
        private void btn_z_positive_MouseDown(object sender, MouseEventArgs e)
        {
            if (Motion.b_googol_connecting_)
                Motion.jogMotion(3, true);
            else if (Motion.b_xyz_table_connecting_)
                Motion.xyz_jog_move(3, 1);
        }
        private void btn_z_negitive_MouseDown(object sender, MouseEventArgs e)
        {
            if (Motion.b_googol_connecting_)
                Motion.jogMotion(3, false);
            else if (Motion.b_xyz_table_connecting_)
                Motion.xyz_jog_move(3, 0);
        }


        private void btn_up_MouseUp(object sender, MouseEventArgs e)
        {
            if (Motion.b_googol_connecting_)
                Motion.stop_all_motion();
            else if (Motion.b_xyz_table_connecting_)
                Motion.xyz_motion_stop();

            Motion.xyz_get_xyz_position();
        }
        private void btn_down_MouseUp(object sender, MouseEventArgs e)
        {
            if (Motion.b_googol_connecting_)
                Motion.stop_all_motion();
            else if (Motion.b_xyz_table_connecting_)
                Motion.xyz_motion_stop();

            Motion.xyz_get_xyz_position();
        }
        private void btn_right_MouseUp(object sender, MouseEventArgs e)
        {
            if (Motion.b_googol_connecting_)
                Motion.stop_all_motion();
            else if (Motion.b_xyz_table_connecting_)
                Motion.xyz_motion_stop();

            Motion.xyz_get_xyz_position();
        }
        private void btn_left_MouseUp(object sender, MouseEventArgs e)
        {
            if (Motion.b_googol_connecting_)
                Motion.stop_all_motion();
            else if (Motion.b_xyz_table_connecting_)
                Motion.xyz_motion_stop();

            Motion.xyz_get_xyz_position();
        }
        private void btn_z_positive_MouseUp(object sender, MouseEventArgs e)
        {
            if (Motion.b_googol_connecting_)
                Motion.stop_all_motion();
            else if (Motion.b_xyz_table_connecting_)
                Motion.xyz_motion_stop();

            Motion.xyz_get_xyz_position();
        }
        private void btn_z_negitive_MouseUp(object sender, MouseEventArgs e)
        {
            if (Motion.b_googol_connecting_)
                Motion.stop_all_motion();
            else if (Motion.b_xyz_table_connecting_)
                Motion.xyz_motion_stop();

            Motion.xyz_get_xyz_position();
        }
        #endregion

        private void btn_p2p_run_Click(object sender, EventArgs e)
        {
            if (Motion.b_googol_connecting_)
            {

            }
            else if (Motion.b_xyz_table_connecting_)
            {
                double x_ = Convert.ToDouble(txt_p2p_step_x.Text);
                double y_ = Convert.ToDouble(txt_p2p_step_y.Text);
                double z_ = Convert.ToDouble(txt_p2p_step_z.Text);
                Motion.xyz_abs_move(x_, y_, z_);
                Motion.xyz_get_xyz_position();
            }
        }

        #region TabControl - Array
        private void checkBox_N_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBox_N.Checked)
                checkBox_Z.Checked = false;
            else if (!checkBox_N.Checked)
                checkBox_Z.Checked = true;
        }

        private void checkBox_Z_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBox_Z.Checked)
                checkBox_N.Checked = false;
            else if (!checkBox_Z.Checked)
                checkBox_N.Checked = true;
        }

        private void btn_array_add_Click(object sender, EventArgs e)
        {
            int start_index_ = Convert.ToInt16(txt_start_index.Text);
            int x_count_ = Convert.ToInt16(txt_x_count.Text);
            double x_shift_ = Convert.ToDouble(txt_x_shift.Text);
            int y_count_ = Convert.ToInt16(txt_y_count.Text);
            double y_shift_ = Convert.ToDouble(txt_y_shift.Text);

            if (x_count_ > 0 && x_shift_ != 0 && y_count_ > 0 && y_shift_ !=0 && dataGridView_step.RowCount>=start_index_)
            {                
                double start_index_x_ = Convert.ToDouble(dataGridView_step.Rows[start_index_].Cells[1].Value.ToString());
                double start_index_y_ = Convert.ToDouble(dataGridView_step.Rows[start_index_].Cells[2].Value.ToString());
                double start_index_z_ = Convert.ToDouble(dataGridView_step.Rows[start_index_].Cells[3].Value.ToString());
                double array_vel_ = Convert.ToDouble(txt_array_vel.Text);

                int step_ = start_index_ + 1;
                if (checkBox_N.Checked)  // N字形
                {
                    for (int x = 1; x <= x_count_; x++)
                    {
                        for (int y = 1; y <= y_count_; y++)
                        {
                            double x_ = start_index_x_ + x_shift_ * (x - 1);
                            double y_;
                            if (x % 2 == 0)
                                y_ = start_index_y_ + y_shift_ * (y_count_ - y);
                            else
                                y_ = start_index_y_ + y_shift_ * (y - 1);

                            if (x != 1 || y != 1)
                            {
                                dataGridView_step.Rows.Add(step_, x_.ToString(), y_.ToString(), start_index_z_.ToString(), array_vel_);
                                step_++;
                            }
                        }
                    }
                }
                else if (checkBox_Z.Checked)  // Z字形
                {
                    for (int y = 1; y <= y_count_; y++)
                    {
                        for (int x = 1; x <= x_count_; x++)
                        {
                            double x_ = start_index_x_ + x_shift_ * (x - 1);
                            if (y % 2 == 0)
                                x_ = start_index_x_ + x_shift_ * (x_count_ - x);
                            else
                                x_ = start_index_x_ + x_shift_ * (x - 1);

                            double y_ = start_index_y_ + y_shift_ * (y - 1);

                            if (x != 1 || y != 1)
                            {
                                dataGridView_step.Rows.Add(step_, x_.ToString(), y_.ToString(), start_index_z_.ToString(), array_vel_);
                                step_++;
                            }
                        }
                    }
                }
            }
            else
                MessageBox.Show("Please Check Parameters First!", "Error Parameters", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void dataGridView_step_MouseClick(object sender, MouseEventArgs e)
        {
            //if(e.Button == MouseButtons.Left)

            //if(e.Button==MouseButtons.Right)
            //{
            //    int row_index_ = dataGridView_step.
            //}
        }
        #endregion

        // Reading Googol Card & Motion's  Encoder Status(pos, vel, acc, dec...etc.)
        private void backgroundWorker_motion_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!backgroundWorker_motion.CancellationPending)
            {
                while (Motion.b_googol_connecting_)
                {
                    Motion.axis_status_update(1, out actual_p_1_, out profile_p_1_);
                    backgroundWorker_motion.ReportProgress(30);
                    Motion.axis_status_update(2, out actual_p_2_, out profile_p_2_);
                    backgroundWorker_motion.ReportProgress(60);
                    Motion.axis_status_update(3, out actual_p_3_, out profile_p_3_);
                    backgroundWorker_motion.ReportProgress(90);
                    Thread.Sleep(10);
                }
                while (Motion.b_xyz_table_connecting_)
                {                    
                    backgroundWorker_motion.ReportProgress(100);
                    Thread.Sleep(10);
                }
            }           
        }
        private void backgroundWorker_motion_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 30)
                txt_cp_x.Text = profile_p_1_;
            else if (e.ProgressPercentage == 60)
                txt_cp_y.Text = profile_p_2_;
            else if (e.ProgressPercentage == 90)
                txt_cp_z.Text = profile_p_3_;
            else if (e.ProgressPercentage == 100)
            {
                txt_cp_x.Text = Motion.xyz_cur_pos_.set_x.ToString();
                txt_cp_y.Text = Motion.xyz_cur_pos_.set_y.ToString();
                txt_cp_z.Text = Motion.xyz_cur_pos_.set_z.ToString();
            }


        }
        private void dataGridView_step_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index_ = dataGridView_step.CurrentCell.RowIndex;
            txt_p2p_step_x.Text = dataGridView_step.Rows[index_].Cells[1].Value.ToString();
            txt_p2p_step_y.Text = dataGridView_step.Rows[index_].Cells[2].Value.ToString();
            txt_p2p_step_z.Text = dataGridView_step.Rows[index_].Cells[3].Value.ToString();
            txt_p2p_vel.Text = dataGridView_step.Rows[index_].Cells[4].Value.ToString();
        }


    }
}
