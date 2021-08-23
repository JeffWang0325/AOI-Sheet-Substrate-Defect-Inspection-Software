using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using static DeltaSubstrateInspector.FileSystem.FileSystem;
using HalconDotNet;


namespace DeltaSubstrateInspector.src.PositionMethod.LocateMethod.Location
{
    public partial class LaserLocationUC : UserControl // (20181031) Jeff Revised!
    {
        #region 模板建立
        private HObject load_image_template = null;
        /// <summary>
        /// 是否尚未執行模板匹配
        /// </summary>
        private bool b_Initial_State_template = true;

        private void btn_load_image_template_Click(object sender, EventArgs e) // (20190214) Jeff Revised!
        {
            try
            {
                if (openFileDialog_file.ShowDialog() == DialogResult.OK)
                {
                    clear_rbtn_Step_template();
                    string path_ = openFileDialog_file.FileName;
                    HOperatorSet.ReadImage(out load_image_template, path_);              
                    HOperatorSet.DispObj(load_image_template, hSmartWindowControl_template.HalconWindow);
                    HOperatorSet.SetPart(hSmartWindowControl_template.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part (20181108) Jeff Revised!

                    // Initialize
                    b_Initial_State_template = true;
                    laser_locate_method_.initialize_params();
                    clear_rbtn_Step_template();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Load Image fails!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_match_template_Click(object sender, EventArgs e) // (20190214) Jeff Revised!
        {
            if (load_image_template == null) return;

            b_Initial_State_template = false;
            laser_locate_method_.find_marks_center(load_image_template.Clone(), checkBox_sort.Checked);

            clear_rbtn_Step_template();
            HOperatorSet.ClearWindow(hSmartWindowControl_template.HalconWindow);
            HOperatorSet.SetColor(hSmartWindowControl_template.HalconWindow, "green");
            HOperatorSet.DispObj(load_image_template, hSmartWindowControl_template.HalconWindow);
            HOperatorSet.SetDraw(hSmartWindowControl_template.HalconWindow, "margin");
            HOperatorSet.DispObj(laser_locate_method_.ho_TransContours_all, hSmartWindowControl_template.HalconWindow);
        }

        private void btn_teach_template_Click(object sender, EventArgs e)
        {
            
        }

        private void checkBox_PartialResult_template_CheckedChanged(object sender, EventArgs e)
        {
            clear_rbtn_Step_template();
        }

        private void btn_instruction_template_Click(object sender, EventArgs e)
        {

        }

        private void rbtn_Step_template_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbtn = sender as RadioButton;
            if (rbtn.Checked == false) return;
            else
            {
                if (checkBox_PartialResult_template.Checked == false)
                {
                    rbtn.Checked = false;
                    return;
                }
            }

            if (load_image_template == null) return;
            else if (b_Initial_State_template)
            {
                b_Initial_State_template = false;
                ;
            }


        }

        private void param_changed(object sender, EventArgs e)
        {
            if (b_LoadRecipe) return;

            Control Ctrl = sender as Control;
            int value = -1;
            string str = "";
            if (Ctrl is NumericUpDown)
            {
                //NumericUpDown nud = (NumericUpDown)Ctrl;
                NumericUpDown nud = sender as NumericUpDown;
                if (!(int.TryParse(nud.Value.ToString(), out value)))
                    return;
            }
            else if (Ctrl is CheckBox)
            {
                CheckBox chb = sender as CheckBox;
                value = Convert.ToInt32(chb.Checked);
            }
            else if (Ctrl is TextBox)
            {
                TextBox tb = sender as TextBox;
                str = tb.Text;
            }

            string tag = Ctrl.Tag.ToString();
            switch (tag)
            {
                case "b_Pretreatment":
                    {
                        laser_locate_method_.b_Pretreatment = value;
                        if (checkBox_PartialResult_template.Checked)
                        {
                            if (load_image_template != null)
                            {
                                b_Initial_State_template = false;
                                ;
                            }
                            if (rbtn_Step1_template.Checked) rbtn_Step_template_CheckedChanged(rbtn_Step1_template, e);
                            else rbtn_Step1_template.Checked = true;
                        }
                    }
                    break;
                case "MinGray_Pretreatment":
                    {
                        laser_locate_method_.MinGray_Pretreatment = value;
                        if (checkBox_PartialResult_template.Checked)
                        {
                            if (load_image_template != null)
                            {
                                b_Initial_State_template = false;
                                ;
                            }
                            if (rbtn_Step1_template.Checked) rbtn_Step_template_CheckedChanged(rbtn_Step1_template, e);
                            else rbtn_Step1_template.Checked = true;
                        }
                    }
                    break;
                case "MaxGray_Pretreatment":
                    {
                        laser_locate_method_.MaxGray_Pretreatment = value;
                        if (checkBox_PartialResult_template.Checked)
                        {
                            if (load_image_template != null)
                            {
                                b_Initial_State_template = false;
                                ;
                            }
                            if (rbtn_Step1_template.Checked) rbtn_Step_template_CheckedChanged(rbtn_Step1_template, e);
                            else rbtn_Step1_template.Checked = true;
                        }
                    }
                    break;
                case "MinContrast":
                    {
                        laser_locate_method_.MinContrast = value;
                        if (checkBox_PartialResult_template.Checked)
                        {
                            if (load_image_template != null)
                            {
                                b_Initial_State_template = false;
                                ;
                            }
                            if (rbtn_Step2_template.Checked) rbtn_Step_template_CheckedChanged(rbtn_Step2_template, e);
                            else rbtn_Step2_template.Checked = true;
                        }
                    }
                    break;
                case "Contrast":
                    {
                        laser_locate_method_.Contrast_string = str;
                        laser_locate_method_.string_2_HTuple(str, out laser_locate_method_.Contrast_HTuple, true);
                        if (checkBox_PartialResult_template.Checked)
                        {
                            if (load_image_template != null)
                            {
                                b_Initial_State_template = false;
                                ;
                            }
                            if (rbtn_Step2_template.Checked) rbtn_Step_template_CheckedChanged(rbtn_Step2_template, e);
                            else rbtn_Step2_template.Checked = true;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// 清除所有 rbtn_Step (For template)
        /// </summary>
        private void clear_rbtn_Step_template()
        {
            rbtn_Step1_template.Checked = false;
            rbtn_Step2_template.Checked = false;
        }
        #endregion



        private LaserLocateMethod laser_locate_method_ = new LaserLocateMethod();
        private HObject load_image_ = null;
        private string info_ = "";
        private HObject Current_disp_regions = null; // 點擊當前 Window 顯示之某一個 region ，並且顯示其資訊
        /// <summary>
        /// 是否尚未執行Marks中心點計算 (20190212) Jeff Revised!
        /// </summary>
        private bool b_Initial_State = true;
        /// <summary>
        /// 是否正在載入Recipe
        /// </summary>
        private bool b_LoadRecipe = false;

        //*************************************************
        public LaserLocationUC()
        {
            InitializeComponent();
            this.hSmartWindowControl_image.MouseWheel += hSmartWindowControl_image.HSmartWindowControl_MouseWheel;
            this.hSmartWindowControl_template.MouseWheel += hSmartWindowControl_template.HSmartWindowControl_MouseWheel;
            
            laser_locate_method_.BoardType = txt_type.Text;

            HOperatorSet.GenEmptyRegion(out Current_disp_regions);

            // 更新顯示參數
            update_groupBox_rowInfo(); // (20181102) Jeff Revised!

            // For debug! (20190212) Jeff Revised!
            //foreach (TabPage P in tabControl1.TabPages)
            //{
            //    if (P.Tag.ToString() == "Template")
            //    {
            //        //P.Hide();
            //        P.Parent = null; // 隱藏

            //    }
            //}
            //tabPage_Template.Parent = null; // 隱藏
            //tabPage_Template.Parent = this.tabControl1; // 顯示
            //tabPage_FindMarksCenter.Parent = null; // 隱藏
            //tabPage_FindMarksCenter.Parent = this.tabControl1; // 顯示
            //foreach (Control c in tabPage_FindMarksCenter.Controls) // 禁能tabPage_FindMarksCenter內所有元件
            //    c.Enabled = false;
            //foreach (Control c in tabPage_FindMarksCenter.Controls) // 致能tabPage_FindMarksCenter內所有元件
            //    c.Enabled = true;
        }

        #region Button & RadioButton
        private void btn_load_image_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog_file.ShowDialog() == DialogResult.OK)
                {
                    string path_ = openFileDialog_file.FileName;
                    HOperatorSet.ReadImage(out load_image_, path_);

                    // 轉成RGB image
                    HTuple Channels;
                    HOperatorSet.CountChannels(load_image_, out Channels);
                    if (Channels == 1)
                    {
                        HOperatorSet.Compose3(load_image_.Clone(), load_image_.Clone(), load_image_.Clone(), out load_image_); // (20181224) Jeff Revised!
                    }

                    //hSmartWindowControl_image.SetFullImagePart(new HImage(load_image_));                  
                    HOperatorSet.DispColor(load_image_, hSmartWindowControl_image.HalconWindow);
                    //HOperatorSet.DispImage(load_image_, hSmartWindowControl_image.HalconWindow);
                    //HOperatorSet.SetPart(hSmartWindowControl_image.HalconWindow, 0, 0, 2160 - 1, 4096 - 1); // (20181108) Jeff Revised!
                    HOperatorSet.SetPart(hSmartWindowControl_image.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part (20181108) Jeff Revised!

                    // Initialize
                    b_Initial_State = true; // (20190212) Jeff Revised!
                    laser_locate_method_.initialize_params();
                    rbtn_origin.Checked = true;
                    //rbtn_result.Checked = false;
                    clear_rbtn_Step(); // (20190212) Jeff Revised!

                    // 讀取XML檔是否有此基板Type
                    //if (!(laser_locate_method_.load()))
                    //    MessageBox.Show("XML檔沒有此基板Type", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Load Mark Image fails!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_FindMarksCenter_Click(object sender, EventArgs e)
        {
            if (load_image_ == null) return;
            
            b_Initial_State = false; // (20190212) Jeff Revised!
            //laser_locate_method_.find_marks_center(load_image_, checkBox_sort.Checked, checkBox_save.Checked); // (20181107) Jeff Revised!
            laser_locate_method_.find_marks_center(load_image_.Clone(), checkBox_sort.Checked, checkBox_save.Checked); // (20181107) Jeff Revised!
            clear_rbtn_Step(); // (20190212) Jeff Revised!
            laser_locate_method_.disp_marks_center(hSmartWindowControl_image, load_image_);

            // 顯示檢測到 Marks 之數量及各自之中心座標位置
            richTextBox_log.AppendText("============" + "\n");
            richTextBox_log.AppendText("Marks' Number : " + laser_locate_method_.all_marks_id_.Count + "\n");

            foreach (PointF i in laser_locate_method_.all_marks_id_) // (20181119) Jeff Revised!
            {
                richTextBox_log.AppendText("X = " + i.X.ToString() + ", Y = " + i.Y.ToString() + "\n");
            }
            richTextBox_log.AppendText("\n");
            richTextBox_log.ScrollToCaret();

            rbtn_origin.Checked = false;
            rbtn_result.Checked = true;
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to save parameters to .xml file?", "Warning", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                ui_parameters(true);
                if (laser_locate_method_.save())
                    MessageBox.Show("Save parameters succeeds!");
            }
        }

        private void Change_image(object sender, EventArgs e)
        {
            Current_disp_regions.Dispose();
            HOperatorSet.GenEmptyRegion(out Current_disp_regions);

            RadioButton rbtn = sender as RadioButton;
            if (rbtn.Checked == false) return;

            if (load_image_ == null)
            {
                rbtn_origin.Checked = true;
                return;
            }
            else if (b_Initial_State) // (20190212) Jeff Revised!
            {
                rbtn_origin.Checked = true;
                HOperatorSet.DispObj(load_image_, hSmartWindowControl_image.HalconWindow);
                return;
            }

            string tag = rbtn.Tag.ToString();
            switch (tag)
            {
                case "origin":
                    {
                        HOperatorSet.DispObj(load_image_, hSmartWindowControl_image.HalconWindow);
                    }
                    break;
                case "result":
                    {
                        laser_locate_method_.disp_marks_center(hSmartWindowControl_image, load_image_);
                    }
                    break;
                case "PartialResult":
                    {
                        if (rbtn_Step1.Checked) rbtn_Step_CheckedChanged(rbtn_Step1, e);
                        else if (rbtn_Step2.Checked) rbtn_Step_CheckedChanged(rbtn_Step2, e);
                        else if (rbtn_Step3.Checked) rbtn_Step_CheckedChanged(rbtn_Step3, e);
                        else if (rbtn_Step4.Checked) rbtn_Step_CheckedChanged(rbtn_Step4, e);
                        else if (rbtn_Step5.Checked) rbtn_Step_CheckedChanged(rbtn_Step5, e);
                        else if (rbtn_Step6.Checked) rbtn_Step_CheckedChanged(rbtn_Step6, e);
                        else if (rbtn_Step7.Checked) rbtn_Step_CheckedChanged(rbtn_Step7, e);
                    }
                    break;
            }
        }
        #endregion

        #region 參數更新 & 顯示片段檢測結果
        private void txt_type_TextChanged(object sender, EventArgs e)
        {
            if (b_LoadRecipe) return; // (20190212) Jeff Revised!

            laser_locate_method_.BoardType = txt_type.Text;

            // 更新即時檢測結果
            if (checkBox_update.Checked)
                btn_FindMarksCenter_Click(sender, e);
        }

        private void nud_ValueChanged(object sender, EventArgs e)
        {
            if (b_LoadRecipe) return; // (20190212) Jeff Revised!

            NumericUpDown nud = sender as NumericUpDown;

            // 非數字，則直接return
            int value;
            if (!(int.TryParse(nud.Value.ToString(), out value)))
                return;

            string tag = nud.Tag.ToString();
            switch (tag)
            {
                case "MinScore":
                    {
                        laser_locate_method_.MinScore = value;
                        if (checkBox_PartialResult.Checked)
                        {
                            if (load_image_ != null)
                            {
                                b_Initial_State = false; // (20190212) Jeff Revised!
                                laser_locate_method_.find_marks_center(load_image_.Clone(), checkBox_sort.Checked);
                            }
                            if (rbtn_Step1.Checked) rbtn_Step_CheckedChanged(rbtn_Step1, e);
                            else rbtn_Step1.Checked = true;
                        }
                    }
                    break;
                case "count_marks":
                    {
                        txt_count_marks.Text = value.ToString();
                        laser_locate_method_.count_marks = value;
                        if (checkBox_PartialResult.Checked)
                        {
                            if (load_image_ != null)
                            {
                                b_Initial_State = false; // (20190212) Jeff Revised!
                                laser_locate_method_.find_marks_center(load_image_.Clone(), checkBox_sort.Checked);
                            }
                            if (rbtn_Step1.Checked) rbtn_Step_CheckedChanged(rbtn_Step1, e);
                            else rbtn_Step1.Checked = true;
                        }
                    }
                    break;
                case "Width_ROI":
                    {
                        laser_locate_method_.Width_ROI = value;
                        if (checkBox_PartialResult.Checked)
                        {
                            if (load_image_ != null)
                            {
                                b_Initial_State = false; // (20190212) Jeff Revised!
                                laser_locate_method_.find_marks_center(load_image_.Clone(), checkBox_sort.Checked);
                            }
                            if (rbtn_Step2.Checked) rbtn_Step_CheckedChanged(rbtn_Step2, e);
                            else rbtn_Step2.Checked = true;
                        }
                    }
                    break;
                case "Height_ROI":
                    {
                        laser_locate_method_.Height_ROI = value;
                        if (checkBox_PartialResult.Checked)
                        {
                            if (load_image_ != null)
                            {
                                b_Initial_State = false; // (20190212) Jeff Revised!
                                laser_locate_method_.find_marks_center(load_image_.Clone(), checkBox_sort.Checked);
                            }
                            if (rbtn_Step2.Checked) rbtn_Step_CheckedChanged(rbtn_Step2, e);
                            else rbtn_Step2.Checked = true;
                        }
                    }
                    break;
                case "MinGray":
                    {
                        laser_locate_method_.MinGray = value;
                        if (checkBox_PartialResult.Checked)
                        {
                            if (load_image_ != null)
                            {
                                b_Initial_State = false; // (20190212) Jeff Revised!
                                laser_locate_method_.find_marks_center(load_image_.Clone(), checkBox_sort.Checked);
                            }
                            if (rbtn_Step3.Checked) rbtn_Step_CheckedChanged(rbtn_Step3, e);
                            else rbtn_Step3.Checked = true;
                        }
                    }
                    break;
                case "MaxGray":
                    {
                        laser_locate_method_.MaxGray = value;
                        if (checkBox_PartialResult.Checked)
                        {
                            if (load_image_ != null)
                            {
                                b_Initial_State = false; // (20190212) Jeff Revised!
                                laser_locate_method_.find_marks_center(load_image_.Clone(), checkBox_sort.Checked);
                            }
                            if (rbtn_Step3.Checked) rbtn_Step_CheckedChanged(rbtn_Step3, e);
                            else rbtn_Step3.Checked = true;
                        }
                    }
                    break;
                case "MinArea":
                    {
                        laser_locate_method_.MinArea = value;
                        if (checkBox_PartialResult.Checked)
                        {
                            if (load_image_ != null)
                            {
                                b_Initial_State = false; // (20190212) Jeff Revised!
                                laser_locate_method_.find_marks_center(load_image_.Clone(), checkBox_sort.Checked);
                            }
                            if (rbtn_Step4.Checked) rbtn_Step_CheckedChanged(rbtn_Step4, e);
                            else rbtn_Step4.Checked = true;
                        }
                    }
                    break;
                case "MaxArea":
                    {
                        laser_locate_method_.MaxArea = value;
                        if (checkBox_PartialResult.Checked)
                        {
                            if (load_image_ != null)
                            {
                                b_Initial_State = false; // (20190212) Jeff Revised!
                                laser_locate_method_.find_marks_center(load_image_.Clone(), checkBox_sort.Checked);
                            }
                            if (rbtn_Step4.Checked) rbtn_Step_CheckedChanged(rbtn_Step4, e);
                            else rbtn_Step4.Checked = true;
                        }
                    }
                    break;
                case "MinHeight":
                    {
                        laser_locate_method_.MinHeight = value;
                        if (checkBox_PartialResult.Checked)
                        {
                            if (load_image_ != null)
                            {
                                b_Initial_State = false; // (20190212) Jeff Revised!
                                laser_locate_method_.find_marks_center(load_image_.Clone(), checkBox_sort.Checked);
                            }
                            if (rbtn_Step4.Checked) rbtn_Step_CheckedChanged(rbtn_Step4, e);
                            else rbtn_Step4.Checked = true;
                        }
                    }
                    break;
                case "MaxHeight":
                    {
                        laser_locate_method_.MaxHeight = value;
                        if (checkBox_PartialResult.Checked)
                        {
                            if (load_image_ != null)
                            {
                                b_Initial_State = false; // (20190212) Jeff Revised!
                                laser_locate_method_.find_marks_center(load_image_.Clone(), checkBox_sort.Checked);
                            }
                            if (rbtn_Step4.Checked) rbtn_Step_CheckedChanged(rbtn_Step4, e);
                            else rbtn_Step4.Checked = true;
                        }
                    }
                    break;
                case "MinWidth":
                    {
                        laser_locate_method_.MinWidth = value;
                        if (checkBox_PartialResult.Checked)
                        {
                            if (load_image_ != null)
                            {
                                b_Initial_State = false; // (20190212) Jeff Revised!
                                laser_locate_method_.find_marks_center(load_image_.Clone(), checkBox_sort.Checked);
                            }
                            if (rbtn_Step4.Checked) rbtn_Step_CheckedChanged(rbtn_Step4, e);
                            else rbtn_Step4.Checked = true;
                        }
                    }
                    break;
                case "MaxWidth":
                    {
                        laser_locate_method_.MaxWidth = value;
                        if (checkBox_PartialResult.Checked)
                        {
                            if (load_image_ != null)
                            {
                                b_Initial_State = false; // (20190212) Jeff Revised!
                                laser_locate_method_.find_marks_center(load_image_.Clone(), checkBox_sort.Checked);
                            }
                            if (rbtn_Step4.Checked) rbtn_Step_CheckedChanged(rbtn_Step4, e);
                            else rbtn_Step4.Checked = true;
                        }
                    }
                    break;
                case "Radius_opening":
                    {
                        laser_locate_method_.Radius_opening = value;
                        if (checkBox_PartialResult.Checked)
                        {
                            if (load_image_ != null)
                            {
                                b_Initial_State = false; // (20190212) Jeff Revised!
                                laser_locate_method_.find_marks_center(load_image_.Clone(), checkBox_sort.Checked);
                            }
                            if (rbtn_Step5.Checked) rbtn_Step_CheckedChanged(rbtn_Step5, e);
                            else rbtn_Step5.Checked = true;
                        }
                    }
                    break;
                case "Radius_closing":
                    {
                        laser_locate_method_.Radius_closing = value;
                        if (checkBox_PartialResult.Checked)
                        {
                            if (load_image_ != null)
                            {
                                b_Initial_State = false; // (20190212) Jeff Revised!
                                laser_locate_method_.find_marks_center(load_image_.Clone(), checkBox_sort.Checked);
                            }
                            if (rbtn_Step6.Checked) rbtn_Step_CheckedChanged(rbtn_Step6, e);
                            else rbtn_Step6.Checked = true;
                        }
                    }
                    break;
            }

            // 更新即時檢測結果
            if (checkBox_update.Checked)
                btn_FindMarksCenter_Click(sender, e);
        }

        private void rbtn_Step_CheckedChanged(object sender, EventArgs e)
        {
            Current_disp_regions.Dispose();
            HOperatorSet.GenEmptyRegion(out Current_disp_regions);

            RadioButton rbtn = sender as RadioButton;
            if (rbtn.Checked == false) return;
            else
            {
                if (checkBox_PartialResult.Checked == false)
                {
                    rbtn.Checked = false;
                    return;
                }
            }

            if (load_image_ == null) return;
            else if (b_Initial_State) // (20190212) Jeff Revised!
            {
                b_Initial_State = false;
                laser_locate_method_.find_marks_center(load_image_.Clone(), checkBox_sort.Checked);
            }

            rbtn_PartialResult.Checked = true;
            HOperatorSet.ClearWindow(hSmartWindowControl_image.HalconWindow);
            HOperatorSet.SetColor(hSmartWindowControl_image.HalconWindow, "green");
            string tag = rbtn.Tag.ToString();
            switch (tag)
            {
                case "Step1":
                    {
                        HOperatorSet.DispObj(load_image_, hSmartWindowControl_image.HalconWindow);
                        HOperatorSet.SetDraw(hSmartWindowControl_image.HalconWindow, "margin");
                        HOperatorSet.DispObj(laser_locate_method_.ho_TransContours_all, hSmartWindowControl_image.HalconWindow);
                    }
                    break;
                case "Step2":
                    {
                        HOperatorSet.DispObj(load_image_, hSmartWindowControl_image.HalconWindow);
                        HOperatorSet.SetDraw(hSmartWindowControl_image.HalconWindow, "margin");
                        HOperatorSet.DispObj(laser_locate_method_.ho_Rect_marks, hSmartWindowControl_image.HalconWindow);
                    }
                    break;
                case "Step3":
                    {
                        HOperatorSet.DispObj(laser_locate_method_.ho_ImageReduced_marks, hSmartWindowControl_image.HalconWindow);
                        HOperatorSet.SetDraw(hSmartWindowControl_image.HalconWindow, "fill");
                        HOperatorSet.DispObj(laser_locate_method_.ho_ConnectedRegions, hSmartWindowControl_image.HalconWindow);
                        Current_disp_regions.Dispose();
                        HOperatorSet.CopyObj(laser_locate_method_.ho_ConnectedRegions, out Current_disp_regions, 1, -1);
                    }
                    break;
                case "Step4":
                    {
                        HOperatorSet.DispObj(laser_locate_method_.ho_ImageReduced_marks, hSmartWindowControl_image.HalconWindow);
                        HOperatorSet.SetDraw(hSmartWindowControl_image.HalconWindow, "fill");
                        HOperatorSet.DispObj(laser_locate_method_.ho_SelectedRegions, hSmartWindowControl_image.HalconWindow);
                        Current_disp_regions.Dispose();
                        HOperatorSet.CopyObj(laser_locate_method_.ho_SelectedRegions, out Current_disp_regions, 1, -1);
                    }
                    break;
                case "Step5":
                    {
                        HOperatorSet.DispObj(laser_locate_method_.ho_ImageReduced_marks, hSmartWindowControl_image.HalconWindow);
                        HOperatorSet.SetDraw(hSmartWindowControl_image.HalconWindow, "fill");
                        HOperatorSet.DispObj(laser_locate_method_.ho_RegionOpening, hSmartWindowControl_image.HalconWindow);
                    }
                    break;
                case "Step6":
                    {
                        HOperatorSet.DispObj(laser_locate_method_.ho_ImageReduced_marks, hSmartWindowControl_image.HalconWindow);
                        HOperatorSet.SetDraw(hSmartWindowControl_image.HalconWindow, "fill");
                        HOperatorSet.DispObj(laser_locate_method_.ho_RegionClosing, hSmartWindowControl_image.HalconWindow);
                    }
                    break;
                case "Step7":
                    {
                        HOperatorSet.DispObj(laser_locate_method_.ho_ImageReduced_marks, hSmartWindowControl_image.HalconWindow);
                        HOperatorSet.SetDraw(hSmartWindowControl_image.HalconWindow, "fill");
                        HOperatorSet.DispObj(laser_locate_method_.ho_Skeleton, hSmartWindowControl_image.HalconWindow);
                        HOperatorSet.SetColor(hSmartWindowControl_image.HalconWindow, "blue");
                        HOperatorSet.DispObj(laser_locate_method_.ho_JuncPoints, hSmartWindowControl_image.HalconWindow);
                    }
                    break;
            }
        }

        /// <summary>
        /// 清除所有 rbtn_Step
        /// </summary>
        private void clear_rbtn_Step() // (20190212) Jeff Revised!
        {
            rbtn_Step1.Checked = false;
            rbtn_Step2.Checked = false;
            rbtn_Step3.Checked = false;
            rbtn_Step4.Checked = false;
            rbtn_Step5.Checked = false;
            rbtn_Step6.Checked = false;
            rbtn_Step7.Checked = false;
        }

        private void btn_instruction_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string tag = btn.Tag.ToString();
            switch (tag)
            {
                case "Step1":
                    {
                        if (checkBox_PartialResult.Checked) rbtn_Step1.Checked = true;
                        richTextBox_log.AppendText("※Step1:" + "\n");
                        richTextBox_log.AppendText("初定位: 利用模板匹配，找出Mark的粗略位置" + "\n\n");
                    }
                    break;
                case "Step2":
                    {
                        if (checkBox_PartialResult.Checked) rbtn_Step2.Checked = true;
                        richTextBox_log.AppendText("※Step2:" + "\n");
                        richTextBox_log.AppendText("初定位: 模板匹配完成，建立 Mark ROI" + "\n\n");
                    }
                    break;
                case "Step3":
                    {
                        if (checkBox_PartialResult.Checked) rbtn_Step3.Checked = true;
                        richTextBox_log.AppendText("※Step3:" + "\n");
                        richTextBox_log.AppendText("針對初定位區域，利用二值化找出Mark區域" + "\n\n");
                    }
                    break;
                case "Step4":
                    {
                        if (checkBox_PartialResult.Checked) rbtn_Step4.Checked = true;
                        richTextBox_log.AppendText("※Step4:" + "\n");
                        richTextBox_log.AppendText("Mark尺寸篩選 (Note: 可點擊任一區域，於區域資訊檢視其對應尺寸)" + "\n\n");
                    }
                    break;
                case "Step5":
                    {
                        if (checkBox_PartialResult.Checked) rbtn_Step5.Checked = true;
                        richTextBox_log.AppendText("※Step5:" + "\n");
                        richTextBox_log.AppendText("消除突出雜點 (Note: 半徑過大可能會消除Mark區域)" + "\n\n");
                    }
                    break;
                case "Step6":
                    {
                        if (checkBox_PartialResult.Checked) rbtn_Step6.Checked = true;
                        richTextBox_log.AppendText("※Step6:" + "\n");
                        richTextBox_log.AppendText("填滿內部孔洞 & 平滑化輪廓" + "\n\n");
                    }
                    break;
                case "Step7":
                    {
                        if (checkBox_PartialResult.Checked) rbtn_Step7.Checked = true;
                        richTextBox_log.AppendText("※Step7:" + "\n");
                        richTextBox_log.AppendText("利用Marks的骨架，找交接點" + "\n\n");
                    }
                    break;
            }

            richTextBox_log.ScrollToCaret();
        }

        private void checkBox_sort_CheckedChanged(object sender, EventArgs e)
        {


            // 更新即時檢測結果
            if (checkBox_update.Checked)
                btn_FindMarksCenter_Click(sender, e);
        }

        private void txt_count_row_TextChanged(object sender, EventArgs e)
        {
            if (b_LoadRecipe) return; // (20190212) Jeff Revised!

            laser_locate_method_.count_row = Convert.ToInt32(txt_count_row.Text);

            // 更新即時檢測結果
            if (checkBox_update.Checked)
                btn_FindMarksCenter_Click(sender, e);
        }

        private void txt_count_marks_TextChanged(object sender, EventArgs e)
        {
            if (b_LoadRecipe) return; // (20190212) Jeff Revised!

            nud_count_marks.Value = Convert.ToInt32(txt_count_marks.Text);
            //laser_locate_method_.count_marks = Convert.ToInt32(txt_count_marks.Text);

            //// 更新即時檢測結果
            //if (checkBox_update.Checked)
            //    btn_FindMarksCenter_Click(sender, e);
        }
        #endregion

        public LaserLocateMethod LaserLocateMethodObj
        {
            get { return this.laser_locate_method_; }
        }

        /// <summary>
        /// 將UI內容與各Class參數互傳
        /// </summary>
        /// <param name="ui_2_parameters_">True=UI傳至Class ; False=Class傳至UI</param>
        public void ui_parameters(bool ui_2_parameters_) // private 改成 public (20180830) Jeff Revised!
        {
            try
            {
                if (ui_2_parameters_) // 將UI內容回傳至Class
                {
                    /* Class of 【LaserLocateMethod】 */
                    // 模板建立
                    laser_locate_method_.b_Pretreatment = Convert.ToInt32(checkBox_Step1_enable.Checked);
                    laser_locate_method_.MinGray_Pretreatment = int.Parse(nud_MinGray_Pretreatment.Value.ToString());
                    laser_locate_method_.MaxGray_Pretreatment = int.Parse(nud_MaxGray_Pretreatment.Value.ToString());
                    laser_locate_method_.MinContrast = int.Parse(nud_MinContrast.Value.ToString());
                    laser_locate_method_.Contrast_string = textBox_Contrast.Text;
                    laser_locate_method_.string_2_HTuple(textBox_Contrast.Text, out laser_locate_method_.Contrast_HTuple, true);

                    // Find Marks Center
                    laser_locate_method_.MinScore = int.Parse(nud_MinScore.Value.ToString()); // (20190211) Jeff Revised!
                    laser_locate_method_.count_marks = int.Parse(nud_count_marks.Value.ToString()); // (20190211) Jeff Revised!
                    laser_locate_method_.Width_ROI = int.Parse(nud_Width_ROI.Value.ToString()); // (20190211) Jeff Revised!
                    laser_locate_method_.Height_ROI = int.Parse(nud_Height_ROI.Value.ToString()); // (20190211) Jeff Revised!
                    laser_locate_method_.MinGray = int.Parse(nud_MinGray.Value.ToString()); // (20190210) Jeff Revised!
                    laser_locate_method_.MaxGray = int.Parse(nud_MaxGray.Value.ToString()); // (20190210) Jeff Revised!
                    laser_locate_method_.MinArea = int.Parse(nud_MinArea.Value.ToString()); // (20190210) Jeff Revised!
                    laser_locate_method_.MaxArea = int.Parse(nud_MaxArea.Value.ToString()); // (20190210) Jeff Revised!
                    laser_locate_method_.MinWidth = int.Parse(nud_MinWidth.Value.ToString()); // (20190210) Jeff Revised!
                    laser_locate_method_.MaxWidth = int.Parse(nud_MaxWidth.Value.ToString()); // (20190210) Jeff Revised!
                    laser_locate_method_.MinHeight = int.Parse(nud_MinHeight.Value.ToString()); // (20190210) Jeff Revised!
                    laser_locate_method_.MaxHeight = int.Parse(nud_MaxHeight.Value.ToString()); // (20190210) Jeff Revised!
                    laser_locate_method_.Radius_opening = int.Parse(nud_Radius_opening.Value.ToString()); // (20190210) Jeff Revised!
                    laser_locate_method_.Radius_closing = int.Parse(nud_Radius_closing.Value.ToString()); // (20190210) Jeff Revised!
                    // (20181102) Jeff Revised!
                    laser_locate_method_.count_marks = Convert.ToInt32(txt_count_marks.Text);
                    laser_locate_method_.count_row = Convert.ToInt32(txt_count_row.Text);

                }
                else // 將Class內容回傳至UI
                {
                    clear_rbtn_Step(); // (20190212) Jeff Revised!
                    b_Initial_State = true; // (20190212) Jeff Revised!
                    b_LoadRecipe = true; // (20190212) Jeff Revised!
                    rbtn_origin.Checked = true;

                    txt_type.Text = laser_locate_method_.BoardType;

                    /* Class of 【LaserLocateMethod】 */
                    // 模板建立
                    checkBox_Step1_enable.Checked = Convert.ToBoolean(laser_locate_method_.b_Pretreatment);
                    nud_MinGray_Pretreatment.Value = laser_locate_method_.MinGray_Pretreatment;
                    nud_MaxGray_Pretreatment.Value = laser_locate_method_.MaxGray_Pretreatment;
                    nud_MinContrast.Value = laser_locate_method_.MinContrast;
                    textBox_Contrast.Text= laser_locate_method_.Contrast_string;

                    // Find Marks Center
                    nud_MinScore.Value = laser_locate_method_.MinScore; // (20190211) Jeff Revised!
                    nud_count_marks.Value = laser_locate_method_.count_marks; // (20190211) Jeff Revised!
                    nud_Width_ROI.Value = laser_locate_method_.Width_ROI; // (20190211) Jeff Revised!
                    nud_Height_ROI.Value = laser_locate_method_.Height_ROI; // (20190211) Jeff Revised!
                    nud_MinGray.Value = laser_locate_method_.MinGray; // (20190210) Jeff Revised!
                    nud_MaxGray.Value = laser_locate_method_.MaxGray; // (20190210) Jeff Revised!
                    nud_MinArea.Value = laser_locate_method_.MinArea; // (20190210) Jeff Revised!
                    nud_MaxArea.Value = laser_locate_method_.MaxArea; // (20190210) Jeff Revised!
                    nud_MinWidth.Value = laser_locate_method_.MinWidth; // (20190210) Jeff Revised!
                    nud_MaxWidth.Value = laser_locate_method_.MaxWidth; // (20190210) Jeff Revised!
                    nud_MinHeight.Value = laser_locate_method_.MinHeight; // (20190210) Jeff Revised!
                    nud_MaxHeight.Value = laser_locate_method_.MaxHeight; // (20190210) Jeff Revised!
                    nud_Radius_opening.Value = laser_locate_method_.Radius_opening; // (20190210) Jeff Revised!
                    nud_Radius_closing.Value = laser_locate_method_.Radius_closing; // (20190210) Jeff Revised!
                    // (20181102) Jeff Revised!
                    txt_count_marks.Text = laser_locate_method_.count_marks.ToString();
                    txt_count_row.Text = laser_locate_method_.count_row.ToString();
                    update_groupBox_rowInfo();

                    b_LoadRecipe = false; // (20190212) Jeff Revised!
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "UI&Parameters Translate Fails !");
            }
        }

        /// <summary>
        /// 更新 groupBox 各列資訊 內之所有參數
        /// </summary>
        private void update_groupBox_rowInfo() // (20181102) Jeff Revised!
        {
            // comboBox_row 初始化
            comboBox_row.Items.Clear();
            comboBox_row.Text = "1";
            for (int i = 1; i <= Convert.ToInt32(txt_count_row.Text); i++)
                comboBox_row.Items.Add(i.ToString());

            // 更新參數
            update_rowInfo_params();
        }

        private void update_rowInfo_params() // (20181102) Jeff Revised!
        {
            int index = Convert.ToInt32(comboBox_row.Text) - 1;
            string temp = "";
            temp += laser_locate_method_.count_rowElement_HTuple[index];
            txt_row_count_rowElement.Text = temp;
        }

        private bool b_comboBox_row_SelectedIndexChanged = false; // (20181102) Jeff Revised!
        private void comboBox_row_SelectedIndexChanged(object sender, EventArgs e) // (20181102) Jeff Revised!
        {
            if (b_LoadRecipe) return; // (20190212) Jeff Revised!

            b_comboBox_row_SelectedIndexChanged = true;
            // 更新參數
            update_rowInfo_params();
            b_comboBox_row_SelectedIndexChanged = false;
        }

        private void txt_row_count_rowElement_TextChanged(object sender, EventArgs e) // (20181102) Jeff Revised!
        {
            if (b_LoadRecipe) return; // (20190212) Jeff Revised!

            if (!b_comboBox_row_SelectedIndexChanged)
            {
                int index = Convert.ToInt32(comboBox_row.Text) - 1;
                laser_locate_method_.count_rowElement_HTuple[index] = Convert.ToInt32(txt_row_count_rowElement.Text);
            }
        }

        private void checkBox_Disp_CheckedChanged(object sender, EventArgs e)
        {
            clear_rbtn_Step(); // (20190212) Jeff Revised!

            CheckBox cb = sender as CheckBox;
            if (cb.Checked == false) return;

            string tag = cb.Tag.ToString();
            switch (tag)
            {
                case "update": // 即時更新計算結果
                    {
                        if (checkBox_PartialResult.Checked)
                            checkBox_PartialResult.Checked = false;
                        rbtn_PartialResult.Enabled = false;
                    }
                    break;
                case "PartialResult": // 顯示片段檢測結果
                    {
                        if (checkBox_update.Checked)
                            checkBox_update.Checked = false;
                        rbtn_PartialResult.Enabled = true;
                    }
                    break;
            }
        }
        
        private void DisplayWindows_HMouseDown(object sender, HMouseEventArgs e)
        {
            HTuple Area, Width, Height;

            if (Current_disp_regions == null) return;

            HTuple RegionCount, hv_Index;
            HOperatorSet.CountObj(Current_disp_regions, out RegionCount);
            if (RegionCount > 0)
            {
                HOperatorSet.GetRegionIndex(Current_disp_regions, (int)(e.Y + 0.5), (int)(e.X + 0.5), out hv_Index); // (20190215) Jeff Revised!
                if (hv_Index.Length > 0)
                {
                    HOperatorSet.SetDraw(hSmartWindowControl_image.HalconWindow, "fill");
                    HOperatorSet.SetColor(hSmartWindowControl_image.HalconWindow, "green");
                    HOperatorSet.DispObj(laser_locate_method_.ho_ImageReduced_marks, hSmartWindowControl_image.HalconWindow);
                    HOperatorSet.DispObj(Current_disp_regions, hSmartWindowControl_image.HalconWindow);

                    HOperatorSet.RegionFeatures(Current_disp_regions[hv_Index], "area", out Area);
                    HOperatorSet.RegionFeatures(Current_disp_regions[hv_Index], "width", out Width);
                    HOperatorSet.RegionFeatures(Current_disp_regions[hv_Index], "height", out Height);

                    labW.Text = Width.ToString();
                    labH.Text = Height.ToString();
                    labA.Text = Area.ToString();

                    HOperatorSet.SetDraw(hSmartWindowControl_image.HalconWindow, "fill");
                    HOperatorSet.SetColor(hSmartWindowControl_image.HalconWindow, "blue");
                    HOperatorSet.DispObj(Current_disp_regions[hv_Index], hSmartWindowControl_image.HalconWindow);
                }
            }
        }

        private void DisplayWindows_HMouseMove(object sender, HMouseEventArgs e) // (20190218) Jeff Revised!
        {
            try
            {
                if (load_image_ == null) return;

                HObject nowImage = load_image_;
                //HObject nowImage = load_image_.Clone(); // 速度慢!
                //HOperatorSet.CopyImage(load_image_, out nowImage); // 速度慢!

                if (rbtn_Step3.Checked || rbtn_Step4.Checked || rbtn_Step5.Checked || rbtn_Step6.Checked || rbtn_Step7.Checked)
                    nowImage = laser_locate_method_.ho_ImageReduced_marks;

                if (nowImage == null) return;
                HTuple width = null, height = null;
                HOperatorSet.GetImageSize(nowImage, out width, out height);
                txt_CursorCoordinate.Text = "(" + String.Format("{0:#}", e.X) + ", " + String.Format("{0:#}", e.Y) + ")";
                try
                {
                    if (e.X >= 0 && e.X < width && e.Y >= 0 && e.Y < height)
                    {
                        HTuple grayval;
                        HOperatorSet.GetGrayval(nowImage, (int)(e.Y + 0.5), (int)(e.X + 0.5), out grayval);
                        txt_ColorValue.Text = (grayval.TupleInt()).ToString();
                        HObject GrayImg;
                        HOperatorSet.Rgb1ToGray(nowImage, out GrayImg);
                        HOperatorSet.GetGrayval(GrayImg, (int)(e.Y + 0.5), (int)(e.X + 0.5), out grayval);
                        txbGrayValue.Text = (grayval.TupleInt()).ToString();
                        GrayImg.Dispose();
                    }
                    else
                    {
                        txt_ColorValue.Text = "";
                        txbGrayValue.Text = "";
                    }
                }
                catch
                { }
            }
            catch
            { }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e) // (20190213) Jeff Revised!
        {
            TabControl tb = sender as TabControl;
            //tb.SelectedIndex = 0;
            if (tb.SelectedTab.Tag.ToString() == "Template")
                groupBox_ImageDisplay.Enabled = false;
            else
                groupBox_ImageDisplay.Enabled = true;
        }

        public string Info
        {
            get { return txt_type.Text; }
        }
    }
}
