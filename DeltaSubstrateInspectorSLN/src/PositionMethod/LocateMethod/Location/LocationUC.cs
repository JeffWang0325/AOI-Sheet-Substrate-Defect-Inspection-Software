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
using DeltaSubstrateInspector.src.PositionMethod.LocateMethod.Rotation;

using System.Media; // For SystemSounds (20181217) Jeff Revised!
using System.Threading; // (20190611) Jeff Revised!
using System.Collections.Concurrent; // For ConcurrentStack<T> (20190612) Jeff Revised!
using DeltaSubstrateInspector.src.Modules; // (20190701) Jeff Revised!
using DeltaSubstrateInspector.FileSystem;

namespace DeltaSubstrateInspector.src.PositionMethod.LocateMethod.Location
{
    public partial class LocationUC : UserControl
    {
        private LocateMethod locate_method_ = new LocateMethod();
        private AngleAffineMethod affine_method_ = new AngleAffineMethod();
        HObject load_image_, load_image_rotate_, mark_image_1, mark_image_2;
        HObject mark_1_rotate_image_, first_cell_roi_,first_cell_region_;
        HObject cell_dx_roi_, cell_dy_roi_;
        private string info_ = "";
        /// <summary>
        /// 待測基板儲存檔案位置
        /// </summary>
        private string file_dirctory_;
        
        #region 利用BackgroundWorker做需耗時之工作
        
        private BackgroundWorker bw_tileImgs; // 影像大圖拼接 (20190610) Jeff Revised!
        private BackgroundWorker bw_tileImgs_parallel; // 影像大圖拼接 Using Parallel.For (20190612) Jeff Revised!
        private BackgroundWorker bw_Load_DefectTest; //【載入瑕疵檔】 (20190804) Jeff Revised!
        private BackgroundWorker bw_SaveAs_DefectTest; //【另存瑕疵檔】 (20190804) Jeff Revised!
        private BackgroundWorker bw_Load_Recipe_AB; //【載入】(【雙面統計結果合併】) (20200429) Jeff Revised!

        #endregion

        //*************************************************
        public LocationUC()
        {
            InitializeComponent();

            this.hSmartWindowControl_mark_1.MouseWheel += hSmartWindowControl_mark_1.HSmartWindowControl_MouseWheel;
            this.hSmartWindowControl_mark_2.MouseWheel += hSmartWindowControl_mark_2.HSmartWindowControl_MouseWheel;
            this.hSmartWindowControl_map.MouseWheel += hSmartWindowControl_map.HSmartWindowControl_MouseWheel;
            this.hSmartWindowControl_defComb.MouseWheel += hSmartWindowControl_defComb.HSmartWindowControl_MouseWheel;

            update_groupBox_cell_pos(); // (20180903) Jeff Revised!

            affine_method_.BoardType = txt_type.Text;
            //affine_method_.Motion_shift_dist_y_hat = Math.Abs(Convert.ToDouble(txt_mark1_my.Text) - Convert.ToDouble(txt_mark2_my.Text));
            if (!checkBox_Y_dir_inv.Checked) // (20190516) Jeff Revised!
            {
                affine_method_.Motion_shift_dist_y_hat = Convert.ToDouble(txt_mark2_my.Text) - Convert.ToDouble(txt_mark1_my.Text);
            }
            else
                affine_method_.Motion_shift_dist_y_hat = Convert.ToDouble(txt_mark1_my.Text) - Convert.ToDouble(txt_mark2_my.Text);
            //this.locate_method_.LocateMethod_Constructor(); // (20200429) Jeff Revised!
            this.locate_method_.board_type_ = txt_type.Text;

            // (20180828) Jeff Revised!
            this.locate_method_.mark_rdx_ = Convert.ToDouble(txt_mark_rdx.Text);
            this.locate_method_.mark_rdy_ = Convert.ToDouble(txt_mark_rdy.Text);
            this.locate_method_.cell_rwidth_ = Convert.ToDouble(txt_cell_rwidth.Text);
            this.locate_method_.cell_rheight_ = Convert.ToDouble(txt_cell_rheight.Text);
            this.locate_method_.cell_rdx_ = Convert.ToDouble(txt_cell_rdx.Text);
            this.locate_method_.cell_rdy_ = Convert.ToDouble(txt_cell_rdy.Text);

            // 單位轉換 (mm → pixel) (20180828) Jeff Revised!
            this.locate_method_.unitTrans_mm2pixel();
            // 更新顯示參數
            txt_mark_pdx.Text = (this.locate_method_.mark_pdx_).ToString();
            txt_mark_pdy.Text = (this.locate_method_.mark_pdy_).ToString();
            txt_cell_pwidth.Text = (this.locate_method_.cell_pwidth_).ToString();
            txt_cell_pheight.Text = (this.locate_method_.cell_pheight_).ToString();
            txt_cell_pdx.Text = (this.locate_method_.cell_pdx_).ToString();
            txt_cell_pdy.Text = (this.locate_method_.cell_pdy_).ToString();

            // 更新按鈕開關狀態 (20181115) Jeff Revised!
            if (this.locate_method_.b_BackDefect)
            {
                lbl_b_BackDefect.Text = "ON";
                this.btn_b_BackDefect.BackgroundImage = global::DeltaSubstrateInspector.Properties.Resources.ON;
            }
            else
            {
                lbl_b_BackDefect.Text = "OFF";
                this.btn_b_BackDefect.BackgroundImage = global::DeltaSubstrateInspector.Properties.Resources.OFF_edited;
            }

            initBackgroundWorker(); // (20190610) Jeff Revised!

            // 更新【輸出格式】下拉式選單項目
            this.OutputType_defComb_CheckedChanged(null, null); // (20200429) Jeff Revised!

            clsLanguage.clsLanguage.SetLanguateToControls(this, true); // (20200214) Jeff Revised!
        }

        private void initBackgroundWorker() // (20190804) Jeff Revised!
        {
            bw_tileImgs = new BackgroundWorker();
            bw_tileImgs.WorkerReportsProgress = false;
            bw_tileImgs.WorkerSupportsCancellation = true;
            bw_tileImgs.DoWork += new DoWorkEventHandler(bw_DoWork_tileImgs);
            bw_tileImgs.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted_tileImgs);

            bw_tileImgs_parallel = new BackgroundWorker();
            bw_tileImgs_parallel.WorkerReportsProgress = false;
            bw_tileImgs_parallel.WorkerSupportsCancellation = true;
            bw_tileImgs_parallel.DoWork += new DoWorkEventHandler(bw_DoWork_tileImgs_parallel);
            bw_tileImgs_parallel.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted_tileImgs);

            bw_Load_DefectTest = new BackgroundWorker();
            bw_Load_DefectTest.WorkerReportsProgress = false;
            bw_Load_DefectTest.WorkerSupportsCancellation = true;
            bw_Load_DefectTest.DoWork += new DoWorkEventHandler(bw_DoWork_Load_DefectTest);
            bw_Load_DefectTest.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted_Load_DefectTest);

            bw_SaveAs_DefectTest = new BackgroundWorker();
            bw_SaveAs_DefectTest.WorkerReportsProgress = false;
            bw_SaveAs_DefectTest.WorkerSupportsCancellation = true;
            bw_SaveAs_DefectTest.DoWork += new DoWorkEventHandler(bw_DoWork_SaveAs_DefectTest);
            bw_SaveAs_DefectTest.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted_SaveAs_DefectTest);

            bw_Load_Recipe_AB = new BackgroundWorker();
            bw_Load_Recipe_AB.WorkerReportsProgress = false;
            bw_Load_Recipe_AB.WorkerSupportsCancellation = true;
            bw_Load_Recipe_AB.DoWork += new DoWorkEventHandler(bw_DoWork_Load_Recipe_AB);
            bw_Load_Recipe_AB.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted_Load_Recipe_AB);
        }

        #region Button of HWindow


        private string Pre_Name_Img = "abc-1", Index_Light = "001", Index_Move = "003"; // (20191216) MIL Jeff Revised!
        private int index_Move = 3; // (20191216) MIL Jeff Revised!
        /// <summary>
        /// 載入第一張檢測影像，並將影像轉正
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_load_image_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog_file.ShowDialog() == DialogResult.OK)
                {
                    ui_parameters(true); // (20191216) MIL Jeff Revised!

                    // (20181217) Jeff Revised!
                    string SafeFileName = openFileDialog_file.SafeFileName;
                    int index = SafeFileName.IndexOf('_');
                    Pre_Name_Img = SafeFileName.Substring(0, index);

                    // (20191216) MIL Jeff Revised!
                    index = SafeFileName.IndexOf("_M");
                    Index_Move = SafeFileName.Substring(index + 2, 3);
                    index_Move = int.Parse(Index_Move);

                    index = SafeFileName.IndexOf("_F");
                    Index_Light = SafeFileName.Substring(index + 2, 3);

                    string path_ = openFileDialog_file.FileName;                    
                    HOperatorSet.ReadImage(out load_image_, path_);
                    // 轉成RGB image (20180823) Jeff Revised!
                    HTuple Channels;
                    HOperatorSet.CountChannels(load_image_, out Channels);
                    if (Channels == 1)
                    {
                        HOperatorSet.Compose3(load_image_.Clone(), load_image_.Clone(), load_image_.Clone(), out load_image_); // (20181224) Jeff Revised!
                    }
                    
                    if (!affine_method_.rotate_image_(hSmartWindowControl_map.HalconWindow, load_image_, out load_image_rotate_))
                    {
                        MessageBox.Show("Rotating Image Fail !", "Rotating Image Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        HOperatorSet.ClearWindow(hSmartWindowControl_map.HalconWindow);
                        HOperatorSet.DispObj(load_image_, hSmartWindowControl_map.HalconWindow);
                    }
                    HOperatorSet.SetPart(hSmartWindowControl_map.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part
                    file_dirctory_ = Path.GetDirectoryName(path_); // path_的路徑刪掉最後的影像名稱 

                    Release_All_concat_images_(); // (20190610) Jeff Revised!
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Loading Image Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            btn_save_map.Enabled = false; // (20190624) Jeff Revised!
        }

        private void btn_create_map_Click(object sender, EventArgs e)
        {
            try
            {
                ui_parameters(true); // (20190603) Jeff Revised!

                if (this.locate_method_.Create_cell_map(hSmartWindowControl_map, load_image_, checkBox_parallel_map.Checked)) // (20190624) Jeff Revised!
                {
                    richTextBox_log.AppendText("Create Map succeeds!" + "\n");
                    if (this.locate_method_.ElapseTime != -1)
                        richTextBox_log.AppendText("Elapsed Time: " + this.locate_method_.ElapseTime + " ms" + "\n");
                    richTextBox_log.ScrollToCaret();

                    HOperatorSet.SetColored(hSmartWindowControl_map.HalconWindow, 12);
                    HObject load_image_rotate_resize; // (20180829) Jeff Revised!
                    HOperatorSet.ZoomImageFactor(load_image_rotate_, out load_image_rotate_resize, this.locate_method_.resize_, this.locate_method_.resize_, "bilinear");
                    HOperatorSet.ClearWindow(hSmartWindowControl_map.HalconWindow);
                    HOperatorSet.DispObj(load_image_rotate_resize, hSmartWindowControl_map.HalconWindow);
                    HOperatorSet.SetDraw(hSmartWindowControl_map.HalconWindow, "margin");
                    HOperatorSet.DispRegion(this.locate_method_.cellmap_sortregion_, hSmartWindowControl_map.HalconWindow);
                    HOperatorSet.SetPart(hSmartWindowControl_map.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part
                    load_image_rotate_resize.Dispose();

                    btn_save_map.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                btn_save_map.Enabled = false;
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_save_map_Click(object sender, EventArgs e)
        {
            try
            {
                ui_parameters(true); // (20190603) Jeff Revised!
                //string file_name_ = txt_array_x_count.Text + "x" + txt_array_y_count.Text; // (20180824) Jeff Revised!
                string file_name_ = this.locate_method_.array_x_count_.ToString() + "x" + this.locate_method_.array_y_count_.ToString(); // (20190603) Jeff Revised!
                if (this.locate_method_.cellmap_sortregion_ != null)
                {
                    HOperatorSet.WriteRegion(this.locate_method_.cellmap_sortregion_, this.locate_method_.File_directory_ + "\\" + file_name_ + "_cellmap.hobj");
                    MessageBox.Show("Save cellmap succeeds!"); // (20180824) Jeff Revised!
                }
                else
                {
                    richTextBox_log.AppendText("==========" + "\n");
                    richTextBox_log.AppendText("Cell Map Missing !");
                    richTextBox_log.ScrollToCaret();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_create_report_Click(object sender, EventArgs e)
        {
            try
            {
                this.locate_method_.Cell_region_mapping(); // (20180829) Jeff Revised!

                this.locate_method_.clear_all_defects(); // (20190713) Jeff Revised!

                HOperatorSet.SetDraw(hSmartWindowControl_map.HalconWindow, "margin");
                HOperatorSet.SetColored(hSmartWindowControl_map.HalconWindow, 12);
#if DEBUG
                HOperatorSet.ClearWindow(hSmartWindowControl_map.HalconWindow);
                HOperatorSet.DispObj(this.locate_method_.TileImage, hSmartWindowControl_map.HalconWindow); // (20200219) Jeff Revised!
                HOperatorSet.DispRegion(this.locate_method_.cellmap_affine_, hSmartWindowControl_map.HalconWindow);
                //MessageBox.Show("Please Check Cell Mapping Location", "Program Stop");
#endif
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 【顯示報告】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_display_report_Click(object sender, EventArgs e) // (20200429) Jeff Revised!
        {
            try
            {
                // 儲存原先工單
                if (this.radioButton_Recipe_A.Checked) // A面
                    this.locate_method_A = this.locate_method_;
                else if (this.radioButton_Recipe_B.Checked) // B面
                    this.locate_method_B = this.locate_method_;
                
                //if (this.locate_method_A.TileImage == null)
                //{
                //    SystemSounds.Exclamation.Play();
                //    MessageBox.Show("TileImage is null!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    return;
                //}
                
                using (CellMapForm form_cellmap_ = new CellMapForm())
                {
                    form_cellmap_.Set_All_Recipe(this.locate_method_A, this.locate_method_B, this.Recipe_AB); // (20200429) Jeff Revised!
                    
                    form_cellmap_.ShowDialog();

                    #region 更新 // (20200429) Jeff Revised!

                    // 更新類別物件
                    form_cellmap_.Update_locate_method_(ref this.locate_method_A);
                    form_cellmap_.Update_locate_method_B(ref this.locate_method_B);
                    form_cellmap_.Update_Recipe_AB(ref this.Recipe_AB);
                    if (this.radioButton_Recipe_A.Checked) // A面
                        this.locate_method_ = this.locate_method_A;
                    else if (this.radioButton_Recipe_B.Checked) // B面
                        this.locate_method_ = this.locate_method_B;

                    // 更新啟用狀態
                    if (this.radioButton_Recipe_B.Enabled == false)
                    {
                        if (this.locate_method_A.Path_DefectTest != "" || this.locate_method_B.Path_DefectTest != "")
                        {
                            this.radioButton_Recipe_B.Enabled = true;
                            this.groupBox_NGClassify_Statistics.Enabled = true;
                        }
                    }
                    
                    #endregion
                }

                // 預設顯示A面，並且強制更新資訊
                if (this.radioButton_Recipe_A.Checked) // A面
                    this.radioButton_Recipe_CheckedChanged(this.radioButton_Recipe_A, null);
                else
                    this.radioButton_Recipe_A.Checked = true;

            }
            catch (Exception ex)
            {
                SystemSounds.Exclamation.Play();
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_save_parameters_Click(object sender, EventArgs e) // (20180829) Jeff Revised!
        {
            BtnLog.WriteLog(" [Location UC] Type : " + this.locate_method_.board_type_);
            BtnLog.WriteLog(" [Location UC] Save_Parameters Click");
            SystemSounds.Exclamation.Play(); // (20190509) Jeff Revised!
            DialogResult dr = MessageBox.Show("Are you sure you want to save parameters to .xml file?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question); // (20180901) Jeff Revised!
            if (dr == DialogResult.Yes) // (20180901) Jeff Revised!
            {
                ui_parameters(true);
                if (this.affine_method_.save() && this.locate_method_.save())
                {
                    BtnLog.WriteLog(" [Location UC] Save_Parameters Done");
                    MessageBox.Show("Save parameters succeeds!");
                }
                else // (20181217) Jeff Revised!
                {
                    BtnLog.WriteLog(" [Location UC] Save_Parameters Error");
                    SystemSounds.Exclamation.Play();
                    MessageBox.Show("Save parameters fails...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        /// <summary>
        /// 計算每個影像位置包含之完整Cell顆數
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_CountCell_zone_Click(object sender, EventArgs e)
        {
            //Test
            //FormCellAlignment MYForm = new FormCellAlignment(this.locate_method_.TileImage, "D://Tmp");
            //MYForm.ShowDialog();
            //return;

            if (this.locate_method_.CountCell_zone())
                MessageBox.Show("Save CellCount.txt succeeds!");
        }

        private void btn_b_BackDefect_Click(object sender, EventArgs e) // (20181115) Jeff Revised!
        {
            this.locate_method_.b_BackDefect ^= true; // XOR
            if (this.locate_method_.b_BackDefect)
            {
                lbl_b_BackDefect.Text = "ON";
                this.btn_b_BackDefect.BackgroundImage = global::DeltaSubstrateInspector.Properties.Resources.ON;
                lbl_b_BackDefect.ForeColor = Color.DeepSkyBlue;
            }
            else
            {
                lbl_b_BackDefect.Text = "OFF";
                this.btn_b_BackDefect.BackgroundImage = global::DeltaSubstrateInspector.Properties.Resources.OFF_edited;
                lbl_b_BackDefect.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            }
        }

        
        #endregion

        #region Timer

        private Control ctrl_timer1 { get; set; } = null;
        private Color BackColor_ctrl_timer1_1 = Color.Transparent, BackColor_ctrl_timer1_2 = Color.Green; // (20190610) Jeff Revised!
        private void timer1_Tick(object sender, EventArgs e) // (20190509) Jeff Revised!
        {
            if (ctrl_timer1 == null) return;

            if (ctrl_timer1.BackColor == BackColor_ctrl_timer1_1)
                ctrl_timer1.BackColor = BackColor_ctrl_timer1_2;
            else
                ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
        }

        private Control ctrl_timer2 { get; set; } = null;
        private int Count_timer2_Tick = 0, MaxCount_timer2_Tick = 15;
        private Color BackColor_ctrl_timer2 = System.Drawing.SystemColors.Control; // (20190603) Jeff Revised!
        private void timer2_Tick(object sender, EventArgs e) // (20190517) Jeff Revised!
        {
            if (ctrl_timer2 == null)
            {
                Count_timer2_Tick = 0;
                timer2.Enabled = false;
                return;
            }

            Count_timer2_Tick++;
            if (Count_timer2_Tick == MaxCount_timer2_Tick)
            {
                timer2.Enabled = false;
                ctrl_timer2.BackColor = BackColor_ctrl_timer2;
                Count_timer2_Tick = 0;
                return;
            }

            if (ctrl_timer2.BackColor == BackColor_ctrl_timer2)
                ctrl_timer2.BackColor = Color.Green;
            else
                ctrl_timer2.BackColor = BackColor_ctrl_timer2;
        }

        #endregion

        /// <summary>
        /// 【視覺定位】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbx_CheckedChanged(object sender, EventArgs e) // (20200429) Jeff Revised!
        {
            if (this.cbx_VisualPosEnable.Checked) // ON
            {
                this.cbx_VisualPosEnable.BackgroundImage = global::DeltaSubstrateInspector.Properties.Resources.ON;
                this.lbl_VisualPosEnable.Text = "ON";
                this.lbl_VisualPosEnable.ForeColor = Color.DeepSkyBlue;
            }
            else // OFF
            {
                this.cbx_VisualPosEnable.BackgroundImage = global::DeltaSubstrateInspector.Properties.Resources.OFF_edited;
                this.lbl_VisualPosEnable.Text = "OFF";
                this.lbl_VisualPosEnable.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            }

            // Enabled狀態切換
            this.tbControlAlignment.Enabled = this.cbx_VisualPosEnable.Checked;
            this.groupBox_coordinateInfo.Enabled = this.cbx_VisualPosEnable.Checked;
        }

        /// <summary>
        /// 【強制指定預設位置】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbx_DefaultMarksPosEnable_CheckedChanged(object sender, EventArgs e) // (20200429) Jeff Revised!
        {
            if (this.cbx_DefaultMarksPosEnable.Checked) // ON
            {
                this.cbx_DefaultMarksPosEnable.BackgroundImage = global::DeltaSubstrateInspector.Properties.Resources.ON;
                this.lbl_DefaultMarksPosEnable.Text = "ON";
                this.lbl_DefaultMarksPosEnable.ForeColor = Color.DeepSkyBlue;
            }
            else // OFF
            {
                this.cbx_DefaultMarksPosEnable.BackgroundImage = global::DeltaSubstrateInspector.Properties.Resources.OFF_edited;
                this.lbl_DefaultMarksPosEnable.Text = "OFF";
                this.lbl_DefaultMarksPosEnable.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            }

            // Enabled狀態切換
            this.panel_DefaultMarksPos.Enabled = this.cbx_DefaultMarksPosEnable.Checked;
        }

        #region TabControl - Rotate
        private void btn_load_markimg_1_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog_file.ShowDialog() == DialogResult.OK)
                {
                    string path_ = openFileDialog_file.FileName;
                    HOperatorSet.ReadImage(out mark_image_1, path_);
                    // 轉成RGB image (20180822) Jeff Revised!
                    HTuple Channels;
                    HOperatorSet.CountChannels(mark_image_1, out Channels);
                    if (Channels == 1)
                    {
                        HOperatorSet.Compose3(mark_image_1.Clone(), mark_image_1.Clone(), mark_image_1.Clone(), out mark_image_1); // (20181224) Jeff Revised!
                    }

                    //hWindowControl_mark_1.SetFullImagePart(new HImage(mark_image_1)); // (20180822) Jeff Revised!
                    //HOperatorSet.DispObj(mark_image_1, hWindowControl_mark_1.HalconWindow);
                    HOperatorSet.DispObj(mark_image_1, hSmartWindowControl_mark_1.HalconWindow);
                    HOperatorSet.SetPart(hSmartWindowControl_mark_1.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part
                    file_dirctory_ = Path.GetDirectoryName(path_); // path_的路徑刪掉最後的影像名稱         

                    // Display cross marks (20190517) Jeff Revised!
                    if (checkBox_ShowCross.Checked)
                    {
                        if (CrossRegion1 != null)
                        {
                            CrossRegion1.Dispose();
                            CrossRegion1 = null;
                        }

                        DeltaSubstrateInspector.src.Models.CaptureModel.Camera.GenCrossMark(mark_image_1, out CrossRegion1);
                        HOperatorSet.SetColor(hSmartWindowControl_mark_1.HalconWindow, "red");
                        HOperatorSet.SetDraw(hSmartWindowControl_mark_1.HalconWindow, "fill");
                        HOperatorSet.DispObj(CrossRegion1, hSmartWindowControl_mark_1.HalconWindow);
                    }

                    // 顯示扭正參考框 (20190517) Jeff Revised!
                    if (checkBox_posRect.Checked && drawing_ID_posRect1 == null)
                    {
                        HTuple ImgWidth, ImgHeight;
                        HOperatorSet.GetImageSize(mark_image_1, out ImgWidth, out ImgHeight);
                        HOperatorSet.CreateDrawingObjectRectangle1(ImgHeight / 2 - 100, ImgWidth / 2 - 100, ImgHeight / 2 + 100, ImgWidth / 2 + 100, out drawing_ID_posRect1);
                        HOperatorSet.SetDrawingObjectParams(drawing_ID_posRect1, "color", "blue");
                        HOperatorSet.AttachDrawingObjectToWindow(hSmartWindowControl_mark_1.HalconWindow, drawing_ID_posRect1);
                    }

                    // 讀取XML檔是否有此基板Type
                    //if (!affine_method_.load() || !locate_method_.load())
                    //{
                    //    lab_xml_status.ForeColor = Color.Red;
                    //    lab_xml_status.Text = "Offline";
                    //}
                    //else
                    //{
                    //    ui_parameters(false);
                    //    lab_xml_status.ForeColor = Color.LightGreen;
                    //    lab_xml_status.Text = "Online";
                    //}

                    // 讀取根目錄下是否有Mark Model
                    //if (!affine_method_.B_model_exist_)
                    //{
                    //    lab_model_status.ForeColor = Color.Red;
                    //    lab_model_status.Text = "Offline";
                    //    richTextBox_log.AppendText("==========" + "\n");
                    //    richTextBox_log.AppendText("Please Create Shape Model First !" + "\n");
                    //    richTextBox_log.ScrollToCaret();
                    //}
                    //else
                    //{
                    //    lab_model_status.ForeColor = Color.LightGreen;
                    //    lab_model_status.Text = "Online";
                    //}

                    // 讀取根目錄下是否有Mark ROI
                    //if (!affine_method_.B_mark_ROI_exist)
                    //{
                    //    richTextBox_log.AppendText("==========" + "\n");
                    //    richTextBox_log.AppendText("Please Create Mark ROI if needed~" + "\n");
                    //    richTextBox_log.ScrollToCaret();
                    //}

                    // 讀取根目錄下是否有Cell Golden Map
                    //string golden_map_path_ = ModuleParamDirectory + PositionParam + @"\" + locate_method_.board_type_ + @"\" // (20180822) Jeff Revised!
                    //                       + locate_method_.array_x_count_.ToString() + "x" + locate_method_.array_y_count_.ToString() + "_cellmap.hobj";
                    //HTuple file_exist_ = 0;
                    //HOperatorSet.FileExists(golden_map_path_, out file_exist_);
                    //if (file_exist_ == 1)
                    //{
                    //    lab_golden_map.ForeColor = Color.LightGreen;
                    //    lab_golden_map.Text = "Online";
                    //}
                    //else
                    //{
                    //    lab_golden_map.ForeColor = Color.Red;
                    //    lab_golden_map.Text = "Offline";
                    //}
                }
            }
            catch (Exception ex)
            {
                SystemSounds.Exclamation.Play(); // (20190509) Jeff Revised!
                MessageBox.Show(ex.ToString(), "Load Mark Image_1 fails", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (mark_image_1 != null && mark_image_2 != null) // (20190509) Jeff Revised!
                btn_find_mark.Enabled = true;
        }

        private HObject CrossRegion1 = null, CrossRegion2 = null; // (20190517) Jeff Revised!
        private HTuple drawing_ID_posRect1 = null, drawing_ID_posRect2 = null;
        private void checkBox_display_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            string tag = cb.Tag.ToString();

            switch (tag)
            {
                case "中心十字":
                    {
                        if (cb.Checked)
                        {
                            cb.ForeColor = Color.Green;
                            if (mark_image_1 != null && CrossRegion1 == null)
                            {
                                DeltaSubstrateInspector.src.Models.CaptureModel.Camera.GenCrossMark(mark_image_1, out CrossRegion1);
                                HOperatorSet.SetColor(hSmartWindowControl_mark_1.HalconWindow, "red");
                                HOperatorSet.SetDraw(hSmartWindowControl_mark_1.HalconWindow, "fill");
                                HOperatorSet.DispObj(CrossRegion1, hSmartWindowControl_mark_1.HalconWindow);
                            }
                            if (mark_image_2 != null && CrossRegion2 == null)
                            {
                                DeltaSubstrateInspector.src.Models.CaptureModel.Camera.GenCrossMark(mark_image_2, out CrossRegion2);
                                HOperatorSet.SetColor(hSmartWindowControl_mark_2.HalconWindow, "red");
                                HOperatorSet.SetDraw(hSmartWindowControl_mark_2.HalconWindow, "fill");
                                HOperatorSet.DispObj(CrossRegion2, hSmartWindowControl_mark_2.HalconWindow);
                            }
                        }
                        else
                        {
                            cb.ForeColor = System.Drawing.SystemColors.ControlText;
                            if (mark_image_1 != null)
                                HOperatorSet.DispObj(mark_image_1, hSmartWindowControl_mark_1.HalconWindow);
                            if (mark_image_2 != null)
                                HOperatorSet.DispObj(mark_image_2, hSmartWindowControl_mark_2.HalconWindow);
                            if (CrossRegion1 != null)
                            {
                                CrossRegion1.Dispose();
                                CrossRegion1 = null;
                            }
                            if (CrossRegion2 != null)
                            {
                                CrossRegion2.Dispose();
                                CrossRegion2 = null;
                            }
                        }
                    }
                    break;
                case "扭正參考框":
                    {
                        if (cb.Checked)
                        {
                            cb.ForeColor = Color.Green;
                            if (mark_image_1 != null)
                            {
                                HTuple ImgWidth, ImgHeight;
                                HOperatorSet.GetImageSize(mark_image_1, out ImgWidth, out ImgHeight);
                                HOperatorSet.CreateDrawingObjectRectangle1(ImgHeight / 2 - 100, ImgWidth / 2 - 100, ImgHeight / 2 + 100, ImgWidth / 2 + 100, out drawing_ID_posRect1);
                                HOperatorSet.SetDrawingObjectParams(drawing_ID_posRect1, "color", "blue");
                                HOperatorSet.AttachDrawingObjectToWindow(hSmartWindowControl_mark_1.HalconWindow, drawing_ID_posRect1);
                            }
                            if (mark_image_2 != null)
                            {
                                HTuple ImgWidth, ImgHeight;
                                HOperatorSet.GetImageSize(mark_image_2, out ImgWidth, out ImgHeight);
                                HOperatorSet.CreateDrawingObjectRectangle1(ImgHeight / 2 - 100, ImgWidth / 2 - 100, ImgHeight / 2 + 100, ImgWidth / 2 + 100, out drawing_ID_posRect2);
                                HOperatorSet.SetDrawingObjectParams(drawing_ID_posRect2, "color", "blue");
                                HOperatorSet.AttachDrawingObjectToWindow(hSmartWindowControl_mark_2.HalconWindow, drawing_ID_posRect2);
                            }
                        }
                        else
                        {
                            cb.ForeColor = System.Drawing.SystemColors.ControlText;
                            if (drawing_ID_posRect1 != null)
                            {
                                HOperatorSet.DetachDrawingObjectFromWindow(hSmartWindowControl_mark_1.HalconWindow, drawing_ID_posRect1);
                                HOperatorSet.ClearDrawingObject(drawing_ID_posRect1);
                                drawing_ID_posRect1 = null;
                            }
                            if (drawing_ID_posRect2 != null)
                            {
                                HOperatorSet.DetachDrawingObjectFromWindow(hSmartWindowControl_mark_2.HalconWindow, drawing_ID_posRect2);
                                HOperatorSet.ClearDrawingObject(drawing_ID_posRect2);
                                drawing_ID_posRect2 = null;
                            }
                        }
                    }
                    break;
            }
        }

        public void update_disp_status(bool b_xml_exist) // (20190509) Jeff Revised!
        {
            if (!b_xml_exist)
            {
                lab_xml_status.ForeColor = Color.Red;
                lab_xml_status.Text = "Offline";
            }
            else
            {
                ui_parameters(false);
                lab_xml_status.ForeColor = Color.LightGreen;
                lab_xml_status.Text = "Online";
            }

            // 讀取根目錄下是否有Mark Model
            if (!affine_method_.B_model_exist_)
            {
                lab_model_status.ForeColor = Color.Red;
                lab_model_status.Text = "Offline";
                richTextBox_log.AppendText("==========" + "\n");
                richTextBox_log.AppendText("Please Create Shape Model First !" + "\n");
                richTextBox_log.ScrollToCaret();
            }
            else
            {
                lab_model_status.ForeColor = Color.LightGreen;
                lab_model_status.Text = "Online";
            }

            // 讀取根目錄下是否有Mark ROI
            if (!affine_method_.B_mark_ROI_exist)
            {
                richTextBox_log.AppendText("==========" + "\n");
                richTextBox_log.AppendText("Please Create Mark ROI if needed~" + "\n");
                richTextBox_log.ScrollToCaret();
            }

            // 讀取根目錄下是否有Cell Golden Map
            string golden_map_path_ = ModuleParamDirectory + PositionParam + @"\" + this.locate_method_.board_type_ + @"\" // (20180822) Jeff Revised!
                                   + this.locate_method_.array_x_count_.ToString() + "x" + this.locate_method_.array_y_count_.ToString() + "_cellmap.hobj";
            HTuple file_exist_ = 0;
            HOperatorSet.FileExists(golden_map_path_, out file_exist_);
            if (file_exist_ == 1)
            {
                lab_golden_map.ForeColor = Color.LightGreen;
                lab_golden_map.Text = "Online";
            }
            else
            {
                lab_golden_map.ForeColor = Color.Red;
                lab_golden_map.Text = "Offline";
            }
        }

        private void btn_load_mark_ROI_Click(object sender, EventArgs e) // (20190509) Jeff Revised!
        {
            if (mark_image_1 == null)
            {
                ctrl_timer1 = btn_load_markimg_1;
                BackColor_ctrl_timer1_1 = Color.Transparent; // (20190610) Jeff Revised!
                BackColor_ctrl_timer1_2 = Color.Green; // (20190610) Jeff Revised!
                timer1.Enabled = true;
                SystemSounds.Exclamation.Play();
                MessageBox.Show("尚未載入影像!", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                timer1.Enabled = false;
                ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
                return;
            }

            if (affine_method_.B_mark_ROI_exist)
            {
                if (mark_image_1 != null)
                {
                    //affine_method_.disp_mark_ROI(hWindowControl_mark_1.HalconWindow);
                    affine_method_.disp_mark_ROI(hSmartWindowControl_mark_1.HalconWindow, mark_image_1); // (20190509) Jeff Revised!

                    // Display cross marks (20190517) Jeff Revised!
                    if (checkBox_ShowCross.Checked && CrossRegion1 != null)
                    {
                        HOperatorSet.SetColor(hSmartWindowControl_mark_1.HalconWindow, "red");
                        HOperatorSet.SetDraw(hSmartWindowControl_mark_1.HalconWindow, "fill");
                        HOperatorSet.DispObj(CrossRegion1, hSmartWindowControl_mark_1.HalconWindow);
                    }
                }
                if (mark_image_2 != null)
                {
                    //affine_method_.disp_mark_ROI(hWindowControl_mark_2.HalconWindow);
                    affine_method_.disp_mark_ROI(hSmartWindowControl_mark_2.HalconWindow, mark_image_2); // (20190509) Jeff Revised!

                    // Display cross marks (20190517) Jeff Revised!
                    if (checkBox_ShowCross.Checked && CrossRegion2 != null)
                    {
                        HOperatorSet.SetColor(hSmartWindowControl_mark_2.HalconWindow, "red");
                        HOperatorSet.SetDraw(hSmartWindowControl_mark_2.HalconWindow, "fill");
                        HOperatorSet.DispObj(CrossRegion2, hSmartWindowControl_mark_2.HalconWindow);
                    }
                }
            }
            else
            {
                ctrl_timer1 = btn_create_mark_ROI;
                BackColor_ctrl_timer1_1 = Color.Transparent; // (20190610) Jeff Revised!
                BackColor_ctrl_timer1_2 = Color.Green; // (20190610) Jeff Revised!
                timer1.Enabled = true;
                SystemSounds.Exclamation.Play(); // (20190509) Jeff Revised!
                MessageBox.Show("Load Mark ROI fails. Please Create Mark ROI First !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                timer1.Enabled = false;
                ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
            }
        }

        private void btn_create_mark_ROI_Click(object sender, EventArgs e) // (20190509) Jeff Revised!
        {
            if (mark_image_1 == null)
            {
                ctrl_timer1 = btn_load_markimg_1;
                BackColor_ctrl_timer1_1 = Color.Transparent; // (20190610) Jeff Revised!
                BackColor_ctrl_timer1_2 = Color.Green; // (20190610) Jeff Revised!
                timer1.Enabled = true;
                SystemSounds.Exclamation.Play();
                MessageBox.Show("尚未載入影像!", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                timer1.Enabled = false;
                ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
                return;
            }
            BtnLog.WriteLog(" [Location UC] Type : " + this.locate_method_.board_type_);
            BtnLog.WriteLog(" [Location UC] Create Mark_ROI Click");
            // 新增使用者保護機制 (20190408) Jeff Revised!
            SystemSounds.Exclamation.Play(); // (20190509) Jeff Revised!
            DialogResult dr = MessageBox.Show("確定要開啟Create Mark ROI功能嗎?", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                if (affine_method_.create_mark_ROI(hWindowControl_mark_1.HalconWindow, mark_image_1))
                {
                    BtnLog.WriteLog(" [Location UC] Create Mark_ROI Done");
                }
                else
                {
                    BtnLog.WriteLog(" [Location UC] Create Mark_ROI Error");
                    SystemSounds.Exclamation.Play(); // (20190509) Jeff Revised!
                    MessageBox.Show("Create Mark ROI Fails!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                BtnLog.WriteLog(" [Location UC] Create Mark_ROI Cancel");
            }
        }

        private HTuple drawing_ID = null;
        private void cbx_create_mark_ROI_CheckedChanged(object sender, EventArgs e) // (20200214) Jeff Revised!
        {
            CheckBox Obj = (CheckBox)sender;

            if (mark_image_1 == null)
            {
                #region CheckBox狀態復原
                cbx_create_mark_ROI.CheckedChanged -= cbx_create_mark_ROI_CheckedChanged;
                Obj.Checked = false;
                cbx_create_mark_ROI.CheckedChanged += cbx_create_mark_ROI_CheckedChanged;
                #endregion

                ctrl_timer1 = btn_load_markimg_1;
                BackColor_ctrl_timer1_1 = Color.Transparent; // (20190610) Jeff Revised!
                BackColor_ctrl_timer1_2 = Color.Green; // (20190610) Jeff Revised!
                timer1.Enabled = true;
                SystemSounds.Exclamation.Play();
                MessageBox.Show("尚未載入影像!", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                timer1.Enabled = false;
                ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
                return;
            }

            if (Obj.Checked) // Create Mark ROI
            {
                BtnLog.WriteLog(" [Location UC] Type : " + this.locate_method_.board_type_);
                BtnLog.WriteLog(" [Location UC] Create Mark_ROI Click");
                // 新增使用者保護機制
                SystemSounds.Exclamation.Play();
                DialogResult dr = MessageBox.Show("確定要開啟Create Mark ROI功能嗎?", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    clsStaticTool.EnableAllControls(this.PatternMatch, Obj, false);
                    HTuple ImgWidth, ImgHeight;
                    HOperatorSet.GetImageSize(mark_image_1, out ImgWidth, out ImgHeight);
                    HOperatorSet.CreateDrawingObjectRectangle2(ImgHeight / 2, ImgWidth / 2, 0, 100, 100, out drawing_ID);
                    HOperatorSet.SetDrawingObjectParams(drawing_ID, "color", "green");
                    HOperatorSet.DispObj(mark_image_1, hSmartWindowControl_mark_1.HalconWindow);
                    HOperatorSet.AttachDrawingObjectToWindow(hSmartWindowControl_mark_1.HalconWindow, drawing_ID);

                    Obj.ForeColor = Color.GreenYellow;
                    if (Language == "Chinese") // (20200214) Jeff Revised!
                        Obj.Text = "創建完成";
                    else
                        Obj.Text = "Create Done";
                    ctrl_timer1 = Obj;
                    BackColor_ctrl_timer1_1 = Color.Transparent; // (20190610) Jeff Revised!
                    BackColor_ctrl_timer1_2 = Color.Green; // (20190610) Jeff Revised!
                    timer1.Enabled = true;

                    // Display cross marks (20190517) Jeff Revised!
                    if (checkBox_ShowCross.Checked && CrossRegion1 != null)
                    {
                        HOperatorSet.SetColor(hSmartWindowControl_mark_1.HalconWindow, "red");
                        HOperatorSet.SetDraw(hSmartWindowControl_mark_1.HalconWindow, "fill");
                        HOperatorSet.DispObj(CrossRegion1, hSmartWindowControl_mark_1.HalconWindow);
                    }
                }
                else
                {
                    #region CheckBox狀態復原
                    cbx_create_mark_ROI.CheckedChanged -= cbx_create_mark_ROI_CheckedChanged;
                    Obj.Checked = false;
                    cbx_create_mark_ROI.CheckedChanged += cbx_create_mark_ROI_CheckedChanged;
                    #endregion

                    BtnLog.WriteLog(" [Location UC] Create Mark_ROI Cancel");
                }
            }
            else // 設定完成
            {
                timer1.Enabled = false;
                ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
                clsStaticTool.EnableAllControls(this.PatternMatch, Obj, true);
                Obj.ForeColor = Color.Black;
                if (Language == "Chinese") // (20200214) Jeff Revised!
                    Obj.Text = "創建標記ROI";
                else
                    Obj.Text = "Create Mark ROI";

                HTuple row, column, phi, length1, length2;
                clsStaticTool.GetRect2Data(drawing_ID, out row, out column, out phi, out length1, out length2);
                HOperatorSet.DetachDrawingObjectFromWindow(hSmartWindowControl_mark_1.HalconWindow, drawing_ID);
                HOperatorSet.ClearDrawingObject(drawing_ID);
                drawing_ID = null;
                HObject Rect2;
                HOperatorSet.GenRectangle2(out Rect2, row, column, phi, length1, length2);
                if (affine_method_.create_mark_ROI(hSmartWindowControl_mark_1.HalconWindow, mark_image_1, Rect2))
                {
                    BtnLog.WriteLog(" [Location UC] Create Mark_ROI Done");

                    // Display cross marks (20190517) Jeff Revised!
                    if (checkBox_ShowCross.Checked && CrossRegion1 != null)
                    {
                        HOperatorSet.SetColor(hSmartWindowControl_mark_1.HalconWindow, "red");
                        HOperatorSet.SetDraw(hSmartWindowControl_mark_1.HalconWindow, "fill");
                        HOperatorSet.DispObj(CrossRegion1, hSmartWindowControl_mark_1.HalconWindow);
                    }

                    ctrl_timer1 = btn_save_parameters;
                    BackColor_ctrl_timer1_1 = Color.Transparent; // (20190610) Jeff Revised!
                    BackColor_ctrl_timer1_2 = Color.Green; // (20190610) Jeff Revised!
                    timer1.Enabled = true;
                    MessageBox.Show("Mark ROI設定成功，如欲儲存設定，請點擊【Save Parameters】; 如欲返回原先設定，請重新載入工單。", "溫馨提醒", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    timer1.Enabled = false;
                    ctrl_timer1.BackColor = Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(166)))), ((int)(((byte)(137)))));
                }
                else
                {
                    BtnLog.WriteLog(" [Location UC] Create Mark_ROI Error");
                    SystemSounds.Exclamation.Play();
                    MessageBox.Show("Create Mark ROI Fails!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                Rect2.Dispose();
            }
        }
        
        private List<Control> ListCtrl_Bypass { get; set; } = new List<Control>();
        
        private void btn_create_model_Click(object sender, EventArgs e)
        {
            if (mark_image_1 == null)
            {
                ctrl_timer1 = btn_load_markimg_1;
                BackColor_ctrl_timer1_1 = Color.Transparent; // (20190610) Jeff Revised!
                BackColor_ctrl_timer1_2 = Color.Green; // (20190610) Jeff Revised!
                timer1.Enabled = true;
                SystemSounds.Exclamation.Play();
                MessageBox.Show("尚未載入影像!", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                timer1.Enabled = false;
                ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
                return;
            }
            BtnLog.WriteLog(" [Location UC] Type : " + this.locate_method_.board_type_);
            BtnLog.WriteLog(" [Location UC] Create Model Click");
            // 新增使用者保護機制 
            SystemSounds.Exclamation.Play(); // (20190509) Jeff Revised!
            DialogResult dr = MessageBox.Show("確定要開啟模板教導功能嗎?", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);    
            if (dr == DialogResult.Yes)
            {
                HObject InputImg;
                if (cbxThEnabled.Checked)
                {
                    // 前處理: 影像二值化
                    HObject ThRegion;
                    HOperatorSet.Threshold(mark_image_1, out ThRegion, MinTh, MaxTh);
                    HOperatorSet.RegionToBin(ThRegion, out InputImg, 255, 0, int.Parse(textBox_frame_pwidth.Text), int.Parse(textBox_frame_pheight.Text));
                    ThRegion.Dispose();
                }
                else
                    InputImg = mark_image_1.Clone();

                if (affine_method_.create_shape_model(hWindowControl_mark_1.HalconWindow, InputImg))
                {
                    BtnLog.WriteLog(" [Location UC] Create Model Done");
                    lab_model_status.ForeColor = Color.LightGreen;
                    lab_model_status.Text = "Online";
                }
                else
                {
                    BtnLog.WriteLog(" [Location UC] Create Model Error");
                    SystemSounds.Exclamation.Play(); // (20190509) Jeff Revised!
                    MessageBox.Show("Create Model Fails!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                InputImg.Dispose();
            }
            else
            {
                BtnLog.WriteLog(" [Location UC] Create Model Cancel");
            }
        }

        private void cbx_create_model_CheckedChanged(object sender, EventArgs e) // (20200214) Jeff Revised!
        {
            CheckBox Obj = (CheckBox)sender;

            if (mark_image_1 == null)
            {
                #region CheckBox狀態復原
                cbx_create_model.CheckedChanged -= cbx_create_model_CheckedChanged;
                Obj.Checked = false;
                cbx_create_model.CheckedChanged += cbx_create_model_CheckedChanged;
                #endregion

                ctrl_timer1 = btn_load_markimg_1;
                BackColor_ctrl_timer1_1 = Color.Transparent; // (20190610) Jeff Revised!
                BackColor_ctrl_timer1_2 = Color.Green; // (20190610) Jeff Revised!
                timer1.Enabled = true;
                SystemSounds.Exclamation.Play();
                MessageBox.Show("尚未載入影像!", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                timer1.Enabled = false;
                ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
                return;
            }

            if (Obj.Checked) // 模板教導
            {
                BtnLog.WriteLog(" [Location UC] Type : " + this.locate_method_.board_type_);
                BtnLog.WriteLog(" [Location UC] Create Model Click");
                // 新增使用者保護機制
                SystemSounds.Exclamation.Play();
                DialogResult dr = MessageBox.Show("確定要開啟模板教導功能嗎?", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    clsStaticTool.EnableAllControls(this.PatternMatch, Obj, false);
                    HTuple ImgWidth, ImgHeight;
                    HOperatorSet.GetImageSize(mark_image_1, out ImgWidth, out ImgHeight);
                    HOperatorSet.CreateDrawingObjectRectangle2(ImgHeight / 2, ImgWidth / 2, 0, 100, 100, out drawing_ID);
                    HOperatorSet.SetDrawingObjectParams(drawing_ID, "color", "red");
                    HOperatorSet.DispObj(mark_image_1, hSmartWindowControl_mark_1.HalconWindow);
                    HOperatorSet.AttachDrawingObjectToWindow(hSmartWindowControl_mark_1.HalconWindow, drawing_ID);

                    Obj.ForeColor = Color.GreenYellow;
                    if (Language == "Chinese") // (20200214) Jeff Revised!
                        Obj.Text = "教導完成";
                    else
                        Obj.Text = "Teach Done";
                    ctrl_timer1 = Obj;
                    BackColor_ctrl_timer1_1 = Color.Transparent; // (20190610) Jeff Revised!
                    BackColor_ctrl_timer1_2 = Color.Green; // (20190610) Jeff Revised!
                    timer1.Enabled = true;

                    // Display cross marks (20190517) Jeff Revised!
                    if (checkBox_ShowCross.Checked && CrossRegion1 != null)
                    {
                        HOperatorSet.SetColor(hSmartWindowControl_mark_1.HalconWindow, "red");
                        HOperatorSet.SetDraw(hSmartWindowControl_mark_1.HalconWindow, "fill");
                        HOperatorSet.DispObj(CrossRegion1, hSmartWindowControl_mark_1.HalconWindow);
                    }
                }
                else
                {
                    #region CheckBox狀態復原
                    cbx_create_model.CheckedChanged -= cbx_create_model_CheckedChanged;
                    Obj.Checked = false;
                    cbx_create_model.CheckedChanged += cbx_create_model_CheckedChanged;
                    #endregion

                    BtnLog.WriteLog(" [Location UC] Create Model Cancel");
                }
            }
            else // 教導完成
            {
                timer1.Enabled = false;
                ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
                clsStaticTool.EnableAllControls(this.PatternMatch, Obj, true);
                Obj.ForeColor = Color.Black;
                if (Language == "Chinese") // (20200214) Jeff Revised!
                    Obj.Text = "模板教導";
                else
                    Obj.Text = "Pattern Teach";

                HTuple row, column, phi, length1, length2;
                clsStaticTool.GetRect2Data(drawing_ID, out row, out column, out phi, out length1, out length2);
                HOperatorSet.DetachDrawingObjectFromWindow(hSmartWindowControl_mark_1.HalconWindow, drawing_ID);
                HOperatorSet.ClearDrawingObject(drawing_ID);
                drawing_ID = null;
                HObject Rect2;
                HOperatorSet.GenRectangle2(out Rect2, row, column, phi, length1, length2);

                HObject InputImg;
                if (cbxThEnabled.Checked)
                {
                    // 前處理: 影像二值化
                    HObject ThRegion;
                    HOperatorSet.Threshold(mark_image_1, out ThRegion, MinTh, MaxTh);
                    HOperatorSet.RegionToBin(ThRegion, out InputImg, 255, 0, int.Parse(textBox_frame_pwidth.Text), int.Parse(textBox_frame_pheight.Text));
                    //HOperatorSet.Compose3(InputImg.Clone(), InputImg.Clone(), InputImg.Clone(), out InputImg); // (20190514) Jeff Revised!
                    ThRegion.Dispose();
                }
                else
                    InputImg = mark_image_1.Clone();

                if (affine_method_.create_shape_model(hSmartWindowControl_mark_1.HalconWindow, InputImg, Rect2))
                {
                    BtnLog.WriteLog(" [Location UC] Create Model Done");
                    lab_model_status.ForeColor = Color.LightGreen;
                    lab_model_status.Text = "Online";

                    // Display cross marks (20190517) Jeff Revised!
                    if (checkBox_ShowCross.Checked && CrossRegion1 != null)
                    {
                        HOperatorSet.SetColor(hSmartWindowControl_mark_1.HalconWindow, "red");
                        HOperatorSet.SetDraw(hSmartWindowControl_mark_1.HalconWindow, "fill");
                        HOperatorSet.DispObj(CrossRegion1, hSmartWindowControl_mark_1.HalconWindow);
                    }

                    ctrl_timer1 = btn_save_parameters;
                    BackColor_ctrl_timer1_1 = Color.Transparent; // (20190610) Jeff Revised!
                    BackColor_ctrl_timer1_2 = Color.Green; // (20190610) Jeff Revised!
                    timer1.Enabled = true;
                    MessageBox.Show("模板教導成功，如欲儲存模板，請點擊【Save Parameters】; 如欲返回原先模板，請重新載入工單。", "溫馨提醒", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    timer1.Enabled = false;
                    ctrl_timer1.BackColor = Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(166)))), ((int)(((byte)(137)))));
                }
                else
                {
                    BtnLog.WriteLog(" [Location UC] Create Model Error");
                    SystemSounds.Exclamation.Play();
                    MessageBox.Show("Create Model Fails!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                Rect2.Dispose();
                InputImg.Dispose();
            }
        }

        //private void btn_tile_images_Click(object sender, EventArgs e)
        //{
        //    ui_parameters(true); // (20190603) Jeff Revised!

        //    richTextBox_log.AppendText("Wait for tiling images ..." + "\n");
        //    richTextBox_log.ScrollToCaret();

        //    int array_x_ = Convert.ToInt16(txt_array_x_count.Text);
        //    int array_y_ = Convert.ToInt16(txt_array_y_count.Text);
        //    List<HObject> all_capture_images_ = new List<HObject>(); // 法1 + 法2
        //    // 法3 (20190606) Jeff Revised!
        //    HObject all_concat_images_ = null;
        //    HOperatorSet.GenEmptyObj(out all_concat_images_);
        //    bool b_resize_first = false;

        //    #region 讀取所有檢測影像
        //    int index_ = 3; // 第一張檢測影像
        //    for (int i = 1; i <= array_x_ * array_y_; i++)
        //    {
        //        //GC.Collect();
        //        //GC.WaitForPendingFinalizers();
        //        HObject read_image_ = null;

        //        try // (20190603) Jeff Revised!
        //        {
        //            if (index_ < 10)
        //            {
        //                //HOperatorSet.ReadImage(out read_image_, file_dirctory_ + "\\abc-1_M00" + index_ + "_F001.tif");
        //                HOperatorSet.ReadImage(out read_image_, file_dirctory_ + "\\" + Pre_Name_Img + "_M00" + index_ + "_F" + Index_Light + ".tif"); // (20181217) Jeff Revised!
        //            }
        //            else if (index_ < 100) // (20180824) Jeff Revised!
        //            {
        //                //HOperatorSet.ReadImage(out read_image_, file_dirctory_ + "\\abc-1_M0" + index_ + "_F001.tif");
        //                HOperatorSet.ReadImage(out read_image_, file_dirctory_ + "\\" + Pre_Name_Img + "_M0" + index_ + "_F" + Index_Light + ".tif"); // (20181217) Jeff Revised!
        //            }
        //            else // (20180824) Jeff Revised!
        //            {
        //                //HOperatorSet.ReadImage(out read_image_, file_dirctory_ + "\\abc-1_M" + index_ + "_F001.tif");
        //                HOperatorSet.ReadImage(out read_image_, file_dirctory_ + "\\" + Pre_Name_Img + "_M" + index_ + "_F" + Index_Light + ".tif"); // (20181217) Jeff Revised!
        //            }
        //        }
        //        catch
        //        {
        //            richTextBox_log.AppendText("Error: Read images fails" + "\n");
        //            richTextBox_log.ScrollToCaret();
        //            SystemSounds.Exclamation.Play();
        //            MessageBox.Show("讀取影像錯誤，請確認影像數量是否完整!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        //            #region Release memories
        //            // 法1 + 法2
        //            //foreach (HObject img in all_capture_images_)
        //            //{
        //            //    img.Dispose();
        //            //}
        //            //all_capture_images_.Clear();
        //            // 法3 (20190606) Jeff Revised!
        //            all_concat_images_.Dispose();

        //            //GC.Collect();
        //            //GC.WaitForPendingFinalizers();
        //            #endregion

        //            return;
        //        }

        //        // 法1
        //        //all_capture_images_.Add(read_image_.Clone());
        //        //read_image_.Dispose();
        //        // 法2
        //        //all_capture_images_.Add(read_image_);
        //        // 法3 (20190606) Jeff Revised!
        //        if (locate_method_.resize_ != 1.0 && b_resize_first) // (20190606) Jeff Revised!
        //        {
        //            // Resize
        //            {
        //                HObject ExpTmpOutVar_0;
        //                HOperatorSet.ZoomImageFactor(read_image_, out ExpTmpOutVar_0, locate_method_.resize_, locate_method_.resize_, "bilinear");
        //                read_image_.Dispose();
        //                read_image_ = ExpTmpOutVar_0;
        //            }
        //        }
        //        {
        //            HObject ExpTmpOutVar_0;
        //            HOperatorSet.ConcatObj(all_concat_images_, read_image_, out ExpTmpOutVar_0);
        //            all_concat_images_.Dispose();
        //            all_concat_images_ = ExpTmpOutVar_0;
        //        }
        //        read_image_.Dispose();

        //        index_++;
        //    }
        //    #endregion

        //    #region 所有觸發影像拼接
        //    bool b_tile_images = false; // (20190603) Jeff Revised!
        //    // 法1 + 法2
        //    //b_tile_images = locate_method_.tile_images(hSmartWindowControl_map, all_capture_images_, out this.locate_method_.TileImage);
        //    // 法3: 速度快很多! (20190606) Jeff Revised!
        //    b_tile_images = locate_method_.tile_images(hSmartWindowControl_map, all_concat_images_, out this.locate_method_.TileImage, b_resize_first);

        //    #region Release memories
        //    // 法1 + 法2
        //    //foreach (HObject i in all_capture_images_)
        //    //{
        //    //    i.Dispose();
        //    //}
        //    //all_capture_images_.Clear();
        //    // 法3 (20190606) Jeff Revised!
        //    all_concat_images_.Dispose();

        //    //GC.Collect();
        //    //GC.WaitForPendingFinalizers();
        //    #endregion

        //    if (b_tile_images)
        //    {
        //        richTextBox_log.AppendText("Tile images succeeds!" + "\n");
        //        if (locate_method_.ElapseTime != -1) // (20181116) Jeff Revised!
        //            richTextBox_log.AppendText("Elapsed Time: " + locate_method_.ElapseTime + " ms" + "\n");
        //        richTextBox_log.ScrollToCaret();
        //    }
        //    else
        //    {
        //        richTextBox_log.AppendText("Tile images fails!" + "\n");
        //        richTextBox_log.ScrollToCaret();

        //        tabControl1.SelectedIndex = 1;
        //        ctrl_timer2 = textBox_resize;
        //        //BackColor_ctrl_timer2 = System.Drawing.SystemColors.Window;
        //        BackColor_ctrl_timer2 = ctrl_timer2.BackColor;
        //        timer2.Enabled = true;

        //        SystemSounds.Exclamation.Play();
        //        MessageBox.Show("影像拼接失敗，請確認Resize數值是否符合規範!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //    }
        //    #endregion
        //}

        /// <summary>
        /// 利用BackgroundWorker做影像大圖拼接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_tile_images_Click(object sender, EventArgs e) // (20190610) Jeff Revised!
        {
            if (bw_tileImgs.IsBusy != true && bw_tileImgs_parallel.IsBusy != true) // (20190613) Jeff Revised!
            {
                clsStaticTool.EnableAllControls(this.groupBox_BigMapTest, btn_tile_images, false);
                btn_tile_images.Enabled = false;
                ui_parameters(true); // (20190603) Jeff Revised!

                richTextBox_log.AppendText("Wait for tiling images ..." + "\n");
                richTextBox_log.ScrollToCaret();

                //Release_All_concat_images_();
                b_OnlyReadAllImgs = false;
                if (checkBox_parallel_tile.Checked) // Using Parallel.For to read all images (20190613) Jeff Revised!
                    bw_tileImgs_parallel.RunWorkerAsync(); // (20190612) Jeff Revised!
                else
                    bw_tileImgs.RunWorkerAsync();

                ctrl_timer1 = btn_tile_images;
                BackColor_ctrl_timer1_1 = Color.Transparent; // (20190610) Jeff Revised!
                BackColor_ctrl_timer1_2 = Color.Green; // (20190610) Jeff Revised!
                timer1.Enabled = true;
            }
        }

        private bool b_OnlyReadAllImgs = false; // 只載入所有拼接影像 (20190610) Jeff Revised!
        private HObject All_concat_images_ = null; // 所有拼接影像 (20190610) Jeff Revised!
        private void Release_All_concat_images_() // (20190610) Jeff Revised!
        {
            if (All_concat_images_ != null)
            {
                All_concat_images_.Dispose();
                All_concat_images_ = null;
            }
        }

        private void bw_DoWork_tileImgs(object sender, DoWorkEventArgs e) // (20190610) Jeff Revised!
        {
            BackgroundWorker Worker = (BackgroundWorker)sender;
            clsProgressbar m_ProgressBar = new clsProgressbar();

            if (!b_OnlyReadAllImgs)
            {
                m_ProgressBar.FormClosedEvent2 += new clsProgressbar.FormClosedHandler2(SetFormClosed2);

                m_ProgressBar.SetShowText("請等待載入影像......");
                m_ProgressBar.SetShowCaption("執行中......");
                m_ProgressBar.ShowRunProgress_thenWait();
            }

            bool b_resize_first = true; // true 比 false 快很多!
            if (All_concat_images_ == null)
            {
                #region 載入所有拼接影像
                int array_x_ = Convert.ToInt16(txt_array_x_count.Text);
                int array_y_ = Convert.ToInt16(txt_array_y_count.Text);
                // 法3 (20190606) Jeff Revised!
                HOperatorSet.GenEmptyObj(out All_concat_images_);

                //int index_ = 3; // 第一張檢測影像 (Method 1)
                //HTuple Path_allImgs = new HTuple(); // Method 3 & Method 4
                //HOperatorSet.TupleGenConst(array_x_ * array_y_, file_dirctory_ + "\\" + Pre_Name_Img + "_", out Path_allImgs); // Method 3 & Method 4
                //HTuple Move_format = new HTuple(), Move_format_temp = new HTuple(); // Method 3 & Method 4
                //HOperatorSet.TupleGenConst(array_x_ * array_y_, "", out Move_format); // Method 3
                //HOperatorSet.TupleGenSequence(3, array_x_ * array_y_ + 2, 1, out Move_format_temp); // Method 4
                //HOperatorSet.TupleString(Move_format_temp, ".3d", out Move_format); // Method 4

                for (int i = index_Move; i <= index_Move + array_x_ * array_y_ - 1; i++) // (20191216) MIL Jeff Revised!
                //for (int i = 1; i <= array_x_ * array_y_; i++)
                //for (int i = 1; false;) // Method 4
                {
                    #region Method 1 & Method 2
                    //GC.Collect();
                    //GC.WaitForPendingFinalizers();
                    HObject read_image_ = null;

                    try // (20190603) Jeff Revised!
                    {
                        // Method 1
                        /*
                        if (index_ < 10)
                        {
                            //HOperatorSet.ReadImage(out read_image_, file_dirctory_ + "\\abc-1_M00" + index_ + "_F001.tif");
                            HOperatorSet.ReadImage(out read_image_, file_dirctory_ + "\\" + Pre_Name_Img + "_M00" + index_ + "_F" + Index_Light + ".tif"); // (20181217) Jeff Revised!
                        }
                        else if (index_ < 100) // (20180824) Jeff Revised!
                        {
                            //HOperatorSet.ReadImage(out read_image_, file_dirctory_ + "\\abc-1_M0" + index_ + "_F001.tif");
                            HOperatorSet.ReadImage(out read_image_, file_dirctory_ + "\\" + Pre_Name_Img + "_M0" + index_ + "_F" + Index_Light + ".tif"); // (20181217) Jeff Revised!
                        }
                        else // (20180824) Jeff Revised!
                        {
                            //HOperatorSet.ReadImage(out read_image_, file_dirctory_ + "\\abc-1_M" + index_ + "_F001.tif");
                            HOperatorSet.ReadImage(out read_image_, file_dirctory_ + "\\" + Pre_Name_Img + "_M" + index_ + "_F" + Index_Light + ".tif"); // (20181217) Jeff Revised!
                        }
                        */
                        // Method 2
                        //string move_format = string.Format("M{0:d3}", i + 2);
                        //HOperatorSet.ReadImage(out read_image_, file_dirctory_ + "\\" + Pre_Name_Img + "_" + move_format + "_F" + Index_Light + ".tif");
                        //HOperatorSet.ReadImage(out read_image_, file_dirctory_ + "\\" + Pre_Name_Img + "_M" + (i + 2).ToString("000") + "_F" + Index_Light + ".tif");
                        HOperatorSet.ReadImage(out read_image_, file_dirctory_ + "\\" + Pre_Name_Img + "_M" + (i).ToString("000") + "_F" + Index_Light + ".tif");
                    }
                    catch
                    {
                        #region Release memories
                        if (read_image_ != null)
                            read_image_.Dispose();
                        // 法3 (20190606) Jeff Revised!
                        Release_All_concat_images_();

                        //GC.Collect();
                        //GC.WaitForPendingFinalizers();
                        #endregion

                        e.Result = "Exception: ReadAllImgs";
                        m_ProgressBar.CloseProgress();
                        return;
                    }
                    
                    // 法3 (20190606) Jeff Revised!
                    if (this.locate_method_.resize_ != 1.0 && b_resize_first) // (20190606) Jeff Revised!
                    {
                        // Resize
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ZoomImageFactor(read_image_, out ExpTmpOutVar_0, this.locate_method_.resize_, this.locate_method_.resize_, "bilinear");
                            //HOperatorSet.ZoomImageFactor(read_image_, out ExpTmpOutVar_0, this.locate_method_.resize_, this.locate_method_.resize_, "constant");
                            //HOperatorSet.ZoomImageFactor(read_image_, out ExpTmpOutVar_0, this.locate_method_.resize_, this.locate_method_.resize_, "nearest_neighbor");
                            read_image_.Dispose();
                            read_image_ = ExpTmpOutVar_0;
                        }
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(All_concat_images_, read_image_, out ExpTmpOutVar_0);
                        All_concat_images_.Dispose();
                        All_concat_images_ = ExpTmpOutVar_0;
                    }
                    read_image_.Dispose();

                    //index_++; // Method 1
                    #endregion



                    #region Method 3
                    //string move_format = string.Format("M{0:d3}", i + 2);
                    //Move_format[i - 1] = move_format;
                    #endregion



                    if (bw_tileImgs.CancellationPending == true) // 判斷是否停止BackgroundWorker執行
                    {
                        e.Cancel = true;
                        m_ProgressBar.CloseProgress();
                        Release_All_concat_images_();
                        return;
                    }

                    // 更新目前進度條的位置
                    m_ProgressBar.SetStep(100 * i / (array_x_ * array_y_));
                }

                #region Method 3 & Method 4
                /*
                Path_allImgs += (Move_format + "_F" + Index_Light + ".tif"); // Method 3
                Path_allImgs += ("M" + Move_format + "_F" + Index_Light + ".tif"); // Method 4

                try
                {
                    HOperatorSet.ReadImage(out All_concat_images_, Path_allImgs);

                    if (locate_method_.resize_ != 1.0 && b_resize_first)
                    {
                        // Resize
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ZoomImageFactor(All_concat_images_, out ExpTmpOutVar_0, locate_method_.resize_, locate_method_.resize_, "bilinear");
                            All_concat_images_.Dispose();
                            All_concat_images_ = ExpTmpOutVar_0;
                        }
                    }
                }
                catch
                {
                    Release_All_concat_images_();

                    e.Result = "Exception: ReadAllImgs";
                    m_ProgressBar.CloseProgress();
                    return;
                }
                */
                #endregion

                #endregion
            }

            if (bw_tileImgs.CancellationPending == true) // 判斷是否停止BackgroundWorker執行
            {
                e.Cancel = true;
                m_ProgressBar.CloseProgress();
                Release_All_concat_images_();
                return;
            }

            if (b_OnlyReadAllImgs)
            {
                e.Result = "Success: ReadAllImgs";
                return;
            }
            else
            {
                // 轉換到WaitProgress (20190611) Jeff Revised!
                m_ProgressBar.SetShowText("請等待大圖拼接......");
                m_ProgressBar.trans2WaitProgress();
                //Thread.Sleep(5000); // For debug!

                #region 所有觸發影像拼接
                bool b_tile_images = false; // (20190603) Jeff Revised!
                // 法3: 速度快很多! (20190606) Jeff Revised!
                b_tile_images = this.locate_method_.tile_images(hSmartWindowControl_map, All_concat_images_, b_resize_first);
                m_ProgressBar.CloseProgress();
                if (b_tile_images)
                {
                    e.Result = "Success";
                }
                else
                {
                    e.Result = "Exception";
                    
                    return;
                }
                #endregion
            }

            if (bw_tileImgs.CancellationPending == true) // 判斷是否停止BackgroundWorker執行
            {
                e.Cancel = true;
                Release_All_concat_images_();
                return;
            }
        }
        
        private void bw_DoWork_tileImgs_parallel(object sender, DoWorkEventArgs e) // (20190612) Jeff Revised!
        {
            BackgroundWorker Worker = (BackgroundWorker)sender;
            clsProgressbar m_ProgressBar = new clsProgressbar();

            if (!b_OnlyReadAllImgs)
            {
                m_ProgressBar.FormClosedEvent2 += new clsProgressbar.FormClosedHandler2(SetFormClosed2);

                m_ProgressBar.SetShowText("請等待載入影像......");
                m_ProgressBar.SetShowCaption("執行中......");
                m_ProgressBar.ShowRunProgress_thenWait();
            }

            bool b_resize_first = true; // true 比 false 快很多!
            if (All_concat_images_ == null)
            {
                #region 載入所有拼接影像
                int array_x_ = Convert.ToInt16(txt_array_x_count.Text);
                int array_y_ = Convert.ToInt16(txt_array_y_count.Text);
                // 法3 (20190606) Jeff Revised!
                HOperatorSet.GenEmptyObj(out All_concat_images_);
                
                try
                {
                    /* Parallel Programming */
                    HObject[] All_images = new HObject[array_x_ * array_y_];
                    int count_imgRead = 0; // 平行運算目前已完成載入幾張影像
                    bool b_error_Parallel = false; // 平行運算中，是否遇到例外狀況
                    Parallel.For(index_Move, index_Move + array_x_ * array_y_, (i, loopState) =>
                    //Parallel.For(0, array_x_ * array_y_, (i, loopState) =>
                    {
                        try
                        {
                            //HOperatorSet.ReadImage(out All_images[i], file_dirctory_ + "\\" + Pre_Name_Img + "_M" + (i + 3).ToString("000") + "_F" + Index_Light + ".tif");
                            HOperatorSet.ReadImage(out All_images[i - index_Move], file_dirctory_ + "\\" + Pre_Name_Img + "_M" + (i).ToString("000") + "_F" + Index_Light + ".tif");
                        }
                        catch (Exception ex)
                        {
                            // 停止並退出Parallel.For
                            loopState.Stop();
                            b_error_Parallel = true;
                            return;
                        }

                        // Resize
                        if (this.locate_method_.resize_ != 1.0 && b_resize_first)
                        {
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.ZoomImageFactor(All_images[i - index_Move], out ExpTmpOutVar_0, this.locate_method_.resize_, this.locate_method_.resize_, "bilinear");
                                //HOperatorSet.ZoomImageFactor(All_images[i], out ExpTmpOutVar_0, this.locate_method_.resize_, this.locate_method_.resize_, "constant");
                                //HOperatorSet.ZoomImageFactor(All_images[i], out ExpTmpOutVar_0, this.locate_method_.resize_, this.locate_method_.resize_, "nearest_neighbor");
                                All_images[i - index_Move].Dispose();
                                All_images[i - index_Move] = ExpTmpOutVar_0;
                            }
                        }
                        
                        if (bw_tileImgs_parallel.CancellationPending == true) // 判斷是否停止BackgroundWorker執行
                        {
                            // 停止並退出Parallel.For
                            loopState.Stop();

                            //e.Cancel = true;
                            //m_ProgressBar.CloseProgress();
                            //Release_All_concat_images_();

                            //foreach (var img in All_images) // (20190612) Jeff Revised!
                            //{
                            //    if (img != null)
                            //        img.Dispose();
                            //}

                            return;
                        }

                        // 更新目前進度條的位置
                        //m_ProgressBar.SetStep(100 * i / (array_x_ * array_y_));
                        //count_imgRead++;
                        //m_ProgressBar.SetStep(100 * count_imgRead / (array_x_ * array_y_));
                        m_ProgressBar.SetStep(100 * (++count_imgRead) / (array_x_ * array_y_));
                    });

                    if (bw_tileImgs_parallel.CancellationPending == true) // 判斷是否停止BackgroundWorker執行
                    {
                        e.Cancel = true;
                        m_ProgressBar.CloseProgress();
                        Release_All_concat_images_();

                        foreach (var img in All_images) // (20190612) Jeff Revised!
                        {
                            if (img != null)
                                img.Dispose();
                        }

                        return;
                    }

                    if (b_error_Parallel) // 判斷平行運算中，是否遇到例外狀況
                    {
                        #region Release memories
                        // 法3 (20190606) Jeff Revised!
                        Release_All_concat_images_();

                        // (20190612) Jeff Revised!
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        #endregion

                        e.Result = "Exception: ReadAllImgs";
                        m_ProgressBar.CloseProgress();
                        return;
                    }

                    /* Transform HObject[] into HObject (All_concat_images_) */
                    foreach (var img in All_images) // (20190612) Jeff Revised!
                    {
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(All_concat_images_, img, out ExpTmpOutVar_0);
                            All_concat_images_.Dispose();
                            All_concat_images_ = ExpTmpOutVar_0;
                        }

                        if (img != null)
                            img.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    #region Release memories
                    // 法3 (20190606) Jeff Revised!
                    Release_All_concat_images_();

                    // (20190612) Jeff Revised!
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    #endregion

                    e.Result = "Exception: ReadAllImgs";
                    m_ProgressBar.CloseProgress();
                    return;
                }
                #endregion
            }

            if (bw_tileImgs_parallel.CancellationPending == true) // 判斷是否停止BackgroundWorker執行
            {
                e.Cancel = true;
                m_ProgressBar.CloseProgress();
                Release_All_concat_images_();
                return;
            }

            if (b_OnlyReadAllImgs)
            {
                e.Result = "Success: ReadAllImgs";
                m_ProgressBar.CloseProgress();
                return;
            }
            else
            {
                // 轉換到WaitProgress (20190611) Jeff Revised!
                m_ProgressBar.SetShowText("請等待大圖拼接......");
                m_ProgressBar.trans2WaitProgress();
                //Thread.Sleep(5000); // For debug!

                #region 所有觸發影像拼接
                bool b_tile_images = false; // (20190603) Jeff Revised!
                // 法3: 速度快很多! (20190606) Jeff Revised!
                b_tile_images = this.locate_method_.tile_images(hSmartWindowControl_map, All_concat_images_, b_resize_first);
                m_ProgressBar.CloseProgress();
                if (b_tile_images)
                {
                    e.Result = "Success";
                }
                else
                {
                    e.Result = "Exception";

                    return;
                }
                #endregion
            }

            if (bw_tileImgs_parallel.CancellationPending == true) // 判斷是否停止BackgroundWorker執行
            {
                e.Cancel = true;
                Release_All_concat_images_();
                return;
            }
        }
        
        /// <summary>
        /// 停止BackgroundWorker執行
        /// </summary>
        public void SetFormClosed2() // (20200429) Jeff Revised!
        {
            if (bw_tileImgs.WorkerSupportsCancellation == true)
            {
                if (bw_tileImgs.IsBusy)
                    bw_tileImgs.CancelAsync();
            }

            if (bw_tileImgs_parallel.WorkerSupportsCancellation == true)
            {
                if (bw_tileImgs_parallel.IsBusy)
                    bw_tileImgs_parallel.CancelAsync();
            }

            if (bw_Load_DefectTest.WorkerSupportsCancellation == true)
            {
                if (bw_Load_DefectTest.IsBusy)
                    bw_Load_DefectTest.CancelAsync();
            }

            if (bw_SaveAs_DefectTest.WorkerSupportsCancellation == true)
            {
                if (bw_SaveAs_DefectTest.IsBusy)
                    bw_SaveAs_DefectTest.CancelAsync();
            }

            if (bw_Load_Recipe_AB.WorkerSupportsCancellation == true)
            {
                if (bw_Load_Recipe_AB.IsBusy)
                    bw_Load_Recipe_AB.CancelAsync();
            }
        }

        /// <summary>
        /// BackgroundWorker執行完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bw_RunWorkerCompleted_tileImgs(object sender, RunWorkerCompletedEventArgs e) // (20190610) Jeff Revised!
        {
            if (e.Cancelled == true)
            {
                //MessageBox.Show("取消!");
            }
            else if (e.Error != null)
            {
                MessageBox.Show("Error: " + e.Error.Message);
            }
            else if (e.Result != null && e.Result.ToString() == "Exception: ReadAllImgs")
            {
                richTextBox_log.AppendText("Error: Read images fails" + "\n");
                richTextBox_log.ScrollToCaret();
                SystemSounds.Exclamation.Play();
                MessageBox.Show("讀取影像錯誤，請確認影像數量是否完整!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (e.Result != null && e.Result.ToString() == "Exception")
            {
                richTextBox_log.AppendText("Tile images fails!" + "\n");
                richTextBox_log.ScrollToCaret();

                tabControl1.SelectedIndex = 1;
                ctrl_timer2 = textBox_resize;
                BackColor_ctrl_timer2 = ctrl_timer2.BackColor;
                timer2.Enabled = true;

                SystemSounds.Exclamation.Play();
                MessageBox.Show("影像拼接失敗，請確認Resize數值是否符合規範!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (e.Result != null && e.Result.ToString() == "Success")
            {
                richTextBox_log.AppendText("Tile images succeeds!" + "\n");
                if (this.locate_method_.ElapseTime != -1) // (20181116) Jeff Revised!
                    richTextBox_log.AppendText("Elapsed Time: " + this.locate_method_.ElapseTime + " ms" + "\n");
                richTextBox_log.ScrollToCaret();
            }
            else
                return;

            clsStaticTool.EnableAllControls(this.groupBox_BigMapTest, btn_tile_images, true);
            btn_tile_images.Enabled = true;
            timer1.Enabled = false;
            ctrl_timer1.BackColor = Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(166)))), ((int)(((byte)(137)))));
        }
        
        private void btn_find_mark_Click(object sender, EventArgs e)
        {
            bool b_status_ = false;
            if (mark_image_1 != null && mark_image_2 != null)
            {
                this.ui_parameters(true); // (20200429) Jeff Revised!
                //b_status_ = affine_method_.find_shpe_model(hWindowControl_mark_1.HalconWindow, mark_image_1.Clone(), hWindowControl_mark_2.HalconWindow, mark_image_2.Clone(), checkBox_save.Checked); // (20181116) Jeff Revised!
                b_status_ = affine_method_.find_shpe_model(hSmartWindowControl_mark_1.HalconWindow, mark_image_1.Clone(), hSmartWindowControl_mark_2.HalconWindow, mark_image_2.Clone(), checkBox_save.Checked); // (20190510) Jeff Revised!
            }
            
            try
            {
                if (b_status_)
                {
                    this.locate_method_.affine_degree_ = affine_method_.Affine_degree_;
                    this.locate_method_.origin_mark_pos_x_ = affine_method_.mark_x_[0];
                    this.locate_method_.origin_mark_pos_y_ = affine_method_.mark_y_[0];

                    // Display cross marks (20190517) Jeff Revised!
                    if (checkBox_ShowCross.Checked && CrossRegion1 != null)
                    {
                        HOperatorSet.SetColor(hSmartWindowControl_mark_1.HalconWindow, "red");
                        HOperatorSet.SetDraw(hSmartWindowControl_mark_1.HalconWindow, "fill");
                        HOperatorSet.DispObj(CrossRegion1, hSmartWindowControl_mark_1.HalconWindow);
                    }
                    if (checkBox_ShowCross.Checked && CrossRegion2 != null)
                    {
                        HOperatorSet.SetColor(hSmartWindowControl_mark_2.HalconWindow, "red");
                        HOperatorSet.SetDraw(hSmartWindowControl_mark_2.HalconWindow, "fill");
                        HOperatorSet.DispObj(CrossRegion2, hSmartWindowControl_mark_2.HalconWindow);
                    }

                    // Display center points of two mark images (20180907) Jeff Revised!
                    HOperatorSet.SetColor(hSmartWindowControl_mark_1.HalconWindow, "blue");
                    HOperatorSet.SetDraw(hSmartWindowControl_mark_1.HalconWindow, "fill");
                    HOperatorSet.DispCircle(hSmartWindowControl_mark_1.HalconWindow, affine_method_.mark_y_[0], affine_method_.mark_x_[0], 10);
                    HOperatorSet.SetColor(hSmartWindowControl_mark_2.HalconWindow, "blue");
                    HOperatorSet.SetDraw(hSmartWindowControl_mark_2.HalconWindow, "fill");
                    HOperatorSet.DispCircle(hSmartWindowControl_mark_2.HalconWindow, affine_method_.mark_y_[1], affine_method_.mark_x_[1], 10);
                    
                    btn_rotate_images.Enabled = true;
                    richTextBox_log.AppendText("============" + "\n");
                    richTextBox_log.AppendText("Affine Degree = " + affine_method_.Affine_degree_ + " (degree)" + "\n");
                    richTextBox_log.AppendText("Mark 1 Points = (" + affine_method_.mark_x_[0].D.ToString() + ", " + affine_method_.mark_y_[0].D.ToString() + ")" + "\n");
                    richTextBox_log.AppendText("Mark 2 Points = (" + affine_method_.mark_x_[1].D.ToString() + ", " + affine_method_.mark_y_[1].D.ToString() + ")" + "\n");
                    richTextBox_log.ScrollToCaret();

                    // Display Computed 2 Marks Distance (運動座標) (mm) (20190516) Jeff Revised!
                    if (affine_method_.B_compute_marks_dist)
                    {
                        tabControl_image_pos_Info.SelectedIndex = 1;
                        txt_computeMarksDist.Text = affine_method_.Marks_dist_motion_compute.ToString();
                        ctrl_timer2 = txt_computeMarksDist;
                        BackColor_ctrl_timer2 = System.Drawing.SystemColors.Control; // (20190603) Jeff Revised!
                        timer2.Enabled = true;
                    }

                    // Display Computed Resolution (μm/pixel) (20190516) Jeff Revised!
                    if (affine_method_.B_compute_resolution)
                    {
                        tabControl_image_pos_Info.SelectedIndex = 1;
                        txt_computeResol.Text = affine_method_.Computed_resolution_.ToString();
                        ctrl_timer2 = txt_computeResol;
                        BackColor_ctrl_timer2 = System.Drawing.SystemColors.Control; // (20190603) Jeff Revised!
                        timer2.Enabled = true;
                    }

                    // 判斷Marks間距誤差是否符合設定範圍
                    if (!(affine_method_.B_marks_dist_valid()))
                    {
                        SystemSounds.Exclamation.Play();
                        MessageBox.Show("模板匹配成功，但兩Marks間距與理論值誤差不符合設定範圍!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    btn_rotate_images.Enabled = false; // (20181107) Jeff Revised!
                    SystemSounds.Exclamation.Play(); // (20190509) Jeff Revised!
                    MessageBox.Show("模板匹配失敗!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                SystemSounds.Exclamation.Play(); // (20190509) Jeff Revised!
                MessageBox.Show(ex.ToString(), "Find marks fail~", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_load_markimg_2_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog_file.ShowDialog() == DialogResult.OK)
                {
                    string path_ = openFileDialog_file.FileName;
                    HOperatorSet.ReadImage(out mark_image_2, path_);
                    // 轉成RGB image (20180822) Jeff Revised!
                    HTuple Channels;
                    HOperatorSet.CountChannels(mark_image_2, out Channels);
                    if (Channels == 1)
                    {
                        HOperatorSet.Compose3(mark_image_2.Clone(), mark_image_2.Clone(), mark_image_2.Clone(), out mark_image_2); // (20181224) Jeff Revised!
                    }

                    //hWindowControl_mark_2.SetFullImagePart(new HImage(mark_image_2)); // (20180822) Jeff Revised!
                    //HOperatorSet.DispObj(mark_image_2, hWindowControl_mark_2.HalconWindow);
                    HOperatorSet.DispObj(mark_image_2, hSmartWindowControl_mark_2.HalconWindow);
                    HOperatorSet.SetPart(hSmartWindowControl_mark_2.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part

                    // Display cross marks (20190517) Jeff Revised!
                    if (checkBox_ShowCross.Checked)
                    {
                        if (CrossRegion2 != null)
                        {
                            CrossRegion2.Dispose();
                            CrossRegion2 = null;
                        }

                        DeltaSubstrateInspector.src.Models.CaptureModel.Camera.GenCrossMark(mark_image_2, out CrossRegion2);
                        HOperatorSet.SetColor(hSmartWindowControl_mark_2.HalconWindow, "red");
                        HOperatorSet.SetDraw(hSmartWindowControl_mark_2.HalconWindow, "fill");
                        HOperatorSet.DispObj(CrossRegion2, hSmartWindowControl_mark_2.HalconWindow);
                    }
                }
            }
            catch (Exception ex)
            {
                SystemSounds.Exclamation.Play(); // (20190509) Jeff Revised!
                MessageBox.Show(ex.ToString(), "Loading Mark Image_2  Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (mark_image_1 != null && mark_image_2 != null)
                btn_find_mark.Enabled = true;
        }

        private void btn_rotate_mark_images_Click(object sender, EventArgs e)
        {
            if (affine_method_.Affine_degree_ != null) // (20181107) Jeff Revised!
            {
                HObject rotate_image_1, rotate_image_2;
                affine_method_.rotate_image_(hSmartWindowControl_mark_1.HalconWindow, mark_image_1, out rotate_image_1, 
                                             checkBox_saveRotateImg.Checked, "RotateImage1.tiff"); // (20190510) Jeff Revised!
                affine_method_.rotate_image_(hSmartWindowControl_mark_2.HalconWindow, mark_image_2, out rotate_image_2, 
                                             checkBox_saveRotateImg.Checked, "RotateImage2.tiff"); // (20190510) Jeff Revised!

                // Display cross marks (20190517) Jeff Revised!
                if (checkBox_ShowCross.Checked && CrossRegion1 != null)
                {
                    HOperatorSet.SetColor(hSmartWindowControl_mark_1.HalconWindow, "red");
                    HOperatorSet.SetDraw(hSmartWindowControl_mark_1.HalconWindow, "fill");
                    HOperatorSet.DispObj(CrossRegion1, hSmartWindowControl_mark_1.HalconWindow);
                }
                if (checkBox_ShowCross.Checked && CrossRegion2 != null)
                {
                    HOperatorSet.SetColor(hSmartWindowControl_mark_2.HalconWindow, "red");
                    HOperatorSet.SetDraw(hSmartWindowControl_mark_2.HalconWindow, "fill");
                    HOperatorSet.DispObj(CrossRegion2, hSmartWindowControl_mark_2.HalconWindow);
                }

                richTextBox_log.AppendText("Mark 1 AffinePoints = (" + affine_method_.affine_mark1_x_.D.ToString() + ", " + affine_method_.affine_mark1_y_.D.ToString() + ")" + "\n");
                richTextBox_log.ScrollToCaret();
            }
            btn_rotate_images.Enabled = false;
        }

        private void checkBox_save_CheckedChanged(object sender, EventArgs e)
        {
            if (!(checkBox_save.Checked))
                checkBox_saveRotateImg.Checked = false;
        }

        private void checkBox_saveRotateImg_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_saveRotateImg.Checked)
                checkBox_save.Checked = true;
        }

        #region TextBox Changed
        private void txt_pixel_resolution_TextChanged(object sender, EventArgs e)
        {
            affine_method_.pixel_resolution_ = Convert.ToDouble(txt_pixel_resolution.Text);
            this.locate_method_.pixel_resolution_ = Convert.ToDouble(txt_pixel_resolution.Text); // (20181018) Jeff Revised!
            this.locate_method_.unitTrans_mm2pixel(); // (20181018) Jeff Revised!
        }
        private void txt_type_TextChanged(object sender, EventArgs e)
        {
            affine_method_.BoardType = txt_type.Text;
            this.locate_method_.board_type_ = txt_type.Text;      
        }

        private void checkBox_X_dir_inv_CheckedChanged(object sender, EventArgs e)
        {
            this.locate_method_.X_dir_inv = Convert.ToInt32(checkBox_X_dir_inv.Checked); // (20180901) Jeff Revised!

            // (20190516) Jeff Revised!
            double mark1_x_hat, mark2_x_hat;
            if (double.TryParse(txt_mark1_mx.Text, out mark1_x_hat) && double.TryParse(txt_mark2_mx.Text, out mark2_x_hat))
            {
                if (this.locate_method_.X_dir_inv == 0)
                    affine_method_.Motion_shift_dist_x_hat = mark2_x_hat - mark1_x_hat;
                else
                    affine_method_.Motion_shift_dist_x_hat = mark1_x_hat - mark2_x_hat;
            }
        }

        private void checkBox_Y_dir_inv_CheckedChanged(object sender, EventArgs e)
        {
            this.locate_method_.Y_dir_inv = Convert.ToInt32(checkBox_Y_dir_inv.Checked); // (20180901) Jeff Revised!

            // (20190516) Jeff Revised!
            double mark1_y_hat, mark2_y_hat;
            if (double.TryParse(txt_mark1_my.Text, out mark1_y_hat) && double.TryParse(txt_mark2_my.Text, out mark2_y_hat))
            {
                if (this.locate_method_.Y_dir_inv == 0)
                    affine_method_.Motion_shift_dist_y_hat = mark2_y_hat - mark1_y_hat;
                else
                    affine_method_.Motion_shift_dist_y_hat = mark1_y_hat - mark2_y_hat;
            }
        }

        /// <summary>
        /// 運動座標 內Text更新事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_motion_TextChanged(object sender, EventArgs e) // (20190515) Jeff Revised!
        {
            TextBox tb = sender as TextBox;
            string tag = tb.Tag.ToString();
            double D;
            if (!double.TryParse(tb.Text, out D))
                return;

            switch (tag)
            {
                case "mark1_rpos_x_":
                    {
                        this.locate_method_.mark1_rpos_x_ = D;
                        double mark2_x_hat;
                        if (double.TryParse(txt_mark2_mx.Text, out mark2_x_hat))
                        {
                            if (this.locate_method_.X_dir_inv == 0)
                            {
                                //affine_method_.Motion_shift_dist_x_hat = Math.Abs(D - mark2_x_hat);
                                affine_method_.Motion_shift_dist_x_hat = mark2_x_hat - D;
                            }
                            else
                                affine_method_.Motion_shift_dist_x_hat = D - mark2_x_hat;
                        }
                    }
                    break;
                case "mark1_rpos_y_":
                    {
                        this.locate_method_.mark1_rpos_y_ = D;
                        double mark2_y_hat;
                        if (double.TryParse(txt_mark2_my.Text, out mark2_y_hat))
                        {
                            if (this.locate_method_.Y_dir_inv == 0)
                            {
                                //affine_method_.Motion_shift_dist_y_hat = Math.Abs(D - mark2_y_hat);
                                affine_method_.Motion_shift_dist_y_hat = mark2_y_hat - D;
                            }
                            else
                                affine_method_.Motion_shift_dist_y_hat = D - mark2_y_hat;
                        }
                    }
                    break;
                case "mark2_rpos_x_":
                    {
                        this.locate_method_.mark2_rpos_x_ = D;
                        double mark1_x_hat;
                        if (double.TryParse(txt_mark1_mx.Text, out mark1_x_hat))
                        {
                            if (this.locate_method_.X_dir_inv == 0)
                            {
                                //affine_method_.Motion_shift_dist_x_hat = Math.Abs(mark1_x_hat - D);
                                affine_method_.Motion_shift_dist_x_hat = D - mark1_x_hat;
                            }
                            else
                                affine_method_.Motion_shift_dist_x_hat = mark1_x_hat - D;
                        }
                    }
                    break;
                case "mark2_rpos_y_":
                    {
                        this.locate_method_.mark2_rpos_y_ = D;
                        double mark1_y_hat;
                        if (double.TryParse(txt_mark1_my.Text, out mark1_y_hat))
                        {
                            if (this.locate_method_.Y_dir_inv == 0)
                            {
                                //affine_method_.Motion_shift_dist_y_hat = Math.Abs(mark1_y_hat - D);
                                affine_method_.Motion_shift_dist_y_hat = D - mark1_y_hat;
                            }
                            else
                                affine_method_.Motion_shift_dist_y_hat = mark1_y_hat - D;
                        }
                    }
                    break;
                case "start_rpos_x_":
                    {
                        this.locate_method_.start_rpos_x_ = D;
                    }
                    break;
                case "start_rpos_y_":
                    {
                        this.locate_method_.start_rpos_y_ = D;
                    }
                    break;
            }
        }

        /// <summary>
        /// 基板座標 內Text更新事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_sample_TextChanged(object sender, EventArgs e) // (20190516) Jeff Revised!
        {
            TextBox tb = sender as TextBox;
            string tag = tb.Tag.ToString();
            double D;
            if (!double.TryParse(tb.Text, out D))
            {
                txt_marks_dist.Text = "";
                txt_marks_angle.Text = "";
                return;
            }

            switch (tag)
            {
                case "Mark1_rpos_sample_X":
                    {
                        affine_method_.Mark1_rpos_sample_X = D;
                    }
                    break;
                case "Mark1_rpos_sample_Y":
                    {
                        affine_method_.Mark1_rpos_sample_Y = D;
                    }
                    break;
                case "Mark2_rpos_sample_X":
                    {
                        affine_method_.Mark2_rpos_sample_X = D;
                    }
                    break;
                case "Mark2_rpos_sample_Y":
                    {
                        affine_method_.Mark2_rpos_sample_Y = D;
                    }
                    break;
            }

            // Compute 2 Marks Distance (基板座標) (mm)
            affine_method_.Marks_dist_sample = Math.Sqrt(Math.Pow(affine_method_.Mark2_rpos_sample_X - affine_method_.Mark1_rpos_sample_X, 2) + 
                                                          Math.Pow(affine_method_.Mark2_rpos_sample_Y - affine_method_.Mark1_rpos_sample_Y, 2) );
            txt_marks_dist.Text = affine_method_.Marks_dist_sample.ToString();

            // 計算基板座標上兩marks夾角 (degree)
            double numerator = affine_method_.Mark2_rpos_sample_Y - affine_method_.Mark1_rpos_sample_Y;
            double denominator = affine_method_.Mark2_rpos_sample_X - affine_method_.Mark1_rpos_sample_X;
            affine_method_.Angle_degree_marks_sample = (((HTuple)(numerator / denominator)).TupleAtan()).TupleDeg().D;
            //HTuple dev_rad;
            //HOperatorSet.TupleAtan(numerator / denominator, out dev_rad);
            //affine_method_.Angle_degree_marks_sample = dev_rad.TupleDeg().D;
            txt_marks_angle.Text = affine_method_.Angle_degree_marks_sample.ToString();
        }

        private void txt_matching_score_TextChanged(object sender, EventArgs e)
        {
            affine_method_.match_score_ = Convert.ToDouble(txt_matching_score.Text);
        }
        private void txt_AngleShift_TextChanged(object sender, EventArgs e) // (20190112) Jeff Revised!
        {
            double D;
            if (double.TryParse(txt_AngleShift.Text, out D))
                affine_method_.Affine_degree_shift = D;
        }
        #endregion
        #endregion

        private void cbxCompute_CheckedChanged(object sender, EventArgs e) // (20190516) Jeff Revised!
        {
            CheckBox cb = sender as CheckBox;
            string tag = cb.Tag.ToString();

            if (tag == "計算解析度")
            {
                affine_method_.B_compute_resolution = cb.Checked;
                if (cb.Checked)
                {
                    //cbx_computeMarksDist.CheckedChanged -= cbxCompute_CheckedChanged;
                    cbx_computeMarksDist.Checked = false;
                    //cbx_computeMarksDist.CheckedChanged += cbxCompute_CheckedChanged;
                    cbx_controlMarksDist.Checked = false;

                    cbx_computeResol.ForeColor = Color.Green;
                }
                else
                {
                    cbx_computeResol.ForeColor = Color.Black;
                    txt_computeResol.Text = "";
                }
            }
            else if (tag == "計算Marks間距")
            {
                affine_method_.B_compute_marks_dist = cb.Checked;
                if (cb.Checked)
                {
                    //cbx_computeResol.CheckedChanged -= cbxCompute_CheckedChanged;
                    cbx_computeResol.Checked = false;
                    //cbx_computeResol.CheckedChanged += cbxCompute_CheckedChanged;

                    cbx_computeMarksDist.ForeColor = Color.Green;
                }
                else
                {
                    cbx_controlMarksDist.Checked = false;

                    cbx_computeMarksDist.ForeColor = Color.Black;
                    txt_computeMarksDist.Text = "";
                }
            }
            else if (tag == "卡控Marks間距誤差")
            {
                affine_method_.B_control_marks_dist = cb.Checked;
                if (cb.Checked)
                {
                    cbx_computeResol.Checked = false;
                    cbx_computeMarksDist.Checked = true;

                    cbx_controlMarksDist.ForeColor = Color.Green;
                    txt_diff_controlMarksDist.Enabled = true;
                }
                else
                {
                    cbx_controlMarksDist.ForeColor = Color.Black;
                    txt_diff_controlMarksDist.Enabled = false;
                }
            }
        }

        private void txt_diff_controlMarksDist_TextChanged(object sender, EventArgs e) // (20190516) Jeff Revised!
        {
            double D;
            if (double.TryParse(txt_diff_controlMarksDist.Text, out D))
            {
                affine_method_.Diff_controlMarksDist = D;
            }
        }

        #region TabControl - Virtual Segmentation + Report
        private void btn_cs_roi_Click(object sender, EventArgs e)
        {
            HObject thred_region_, mark_region_;
            HTuple roi_r1_, roi_r2_, roi_c1_, roi_c2_;
            HTuple area_fc_, row_fc_, col_fc_;
            HTuple cs_thred_min_ = new HTuple(Convert.ToInt32(txt_cs_thred_min.Text));
            HTuple cs_thred_max_ = new HTuple(Convert.ToInt32(txt_cs_thred_max.Text));
            HTuple cs_area_min_ = new HTuple(Convert.ToInt32(txt_cs_area_min.Text));
            HTuple cs_area_max_ = new HTuple(Convert.ToInt32(txt_cs_area_max.Text));


            HOperatorSet.SetColor(hSmartWindowControl_map.HalconWindow, "red");
            richTextBox_log.AppendText("==================" + "\n");
            richTextBox_log.AppendText("Please Drawing Region Contains FirstCell" + "\n");
            if (mark_1_rotate_image_ != null)
            {
                HOperatorSet.Rgb1ToGray(mark_1_rotate_image_, out mark_1_rotate_image_);
                HOperatorSet.ClearWindow(hSmartWindowControl_map.HalconWindow);
                HOperatorSet.DispImage(mark_1_rotate_image_, hSmartWindowControl_map.HalconWindow);
                HOperatorSet.DrawRectangle1(hSmartWindowControl_map.HalconWindow, out roi_r1_, out roi_c1_, out roi_r2_, out roi_c2_);
                HOperatorSet.GenRectangle1(out first_cell_roi_, roi_r1_, roi_c1_, roi_r2_, roi_c2_);
                HOperatorSet.ReduceDomain(mark_1_rotate_image_, first_cell_roi_, out first_cell_roi_);

                double cell_width_ = Math.Abs(roi_c2_.D - roi_c1_.D);
                double cell_height_ = Math.Abs(roi_r2_.D - roi_r1_.D);
                txt_cell_pwidth.Text = cell_width_.ToString();
                txt_cell_pheight.Text = cell_height_.ToString();
                this.locate_method_.cell_pwidth_ = cell_width_;
                this.locate_method_.cell_pheight_ = cell_height_;
                richTextBox_log.AppendText("Cell ROI Success" + "\n");
                richTextBox_log.AppendText("Cell Width = " + cell_width_.ToString() + " pixel"+"\n");
                richTextBox_log.AppendText("Cell Height = " + cell_height_.ToString() + " pixel" + "\n");
                btn_cs_filter.Enabled = true;  
            }
            else
                richTextBox_log.AppendText("Cell ROI Fail" + "\n");

            richTextBox_log.ScrollToCaret();
        }

        private void btn_cs_filter_Click(object sender, EventArgs e)
        {
            HObject thred_region_;
            HTuple cs_thred_min_ = new HTuple(Convert.ToInt32(txt_cs_thred_min.Text));
            HTuple cs_thred_max_ = new HTuple(Convert.ToInt32(txt_cs_thred_max.Text));
            HTuple cs_area_min_ = new HTuple(Convert.ToInt32(txt_cs_area_min.Text));
            HTuple cs_area_max_ = new HTuple(Convert.ToInt32(txt_cs_area_max.Text));

            if (first_cell_roi_ != null)
            {
                HOperatorSet.Threshold(first_cell_roi_, out thred_region_, cs_thred_min_, cs_thred_max_);
                HOperatorSet.Connection(thred_region_, out thred_region_);
                HOperatorSet.SelectShape(thred_region_, out first_cell_region_, "area", "and", cs_area_min_, cs_area_max_);
                HOperatorSet.DispImage(mark_1_rotate_image_, hSmartWindowControl_map.HalconWindow);
                HOperatorSet.DispRegion(first_cell_region_, hSmartWindowControl_map.HalconWindow);
                btn_cs_calculate.Enabled = true;
            }
            else
                richTextBox_log.AppendText("Calculate Cell Size Fail" + "\n");

            richTextBox_log.ScrollToCaret();
        }


        private void btn_cs_calculate_Click(object sender, EventArgs e)
        {
            try
            {
                HTuple area_fc_, row_fc_, col_fc_;
                HOperatorSet.AreaCenter(first_cell_region_, out area_fc_, out row_fc_, out col_fc_);
                double mark_pdx_ = Math.Abs(affine_method_.affine_mark1_x_.D - col_fc_.D);
                double mark_pdy_ = Math.Abs(affine_method_.affine_mark1_y_.D - row_fc_.D);
                txt_mark_pdx.Text = mark_pdx_.ToString();
                txt_mark_pdy.Text = mark_pdy_.ToString();

                this.locate_method_.mark_pdx_ = mark_pdx_;
                this.locate_method_.mark_pdy_ = mark_pdy_;
                richTextBox_log.AppendText("==================" + "\n");
                richTextBox_log.AppendText("Cell ROI Success" + "\n");
                richTextBox_log.AppendText("Mark to FirstCell dx = " + mark_pdx_.ToString() + " pixel" + "\n");
                richTextBox_log.AppendText("Mark to FirstCell dy = " + mark_pdy_.ToString() + " pixel" + "\n");

                richTextBox_log.ScrollToCaret();
            }
            catch(Exception ex)
            { MessageBox.Show(ex.ToString(), "Error"); }
        }

        private void btn_dxdy_roi_Click(object sender, EventArgs e)
        {            
            // Reture 【cell_dx_roi_、cell_dy_roi_】: Already ReduceDomain.
            HObject dx_roi_rect_,  dy_roi_rect_;
            HObject dy_1_roi_rect_, dy_2_roi_rect_;
            HTuple dx_r1_, dx_r2_, dx_c1_, dx_c2_;
            HTuple dy_1_r1_, dy_1_r2_, dy_1_c1_, dy_1_c2_;
            HTuple dy_2_r1_, dy_2_r2_, dy_2_c1_, dy_2_c2_;
            HTuple cd_thred_min_ = new HTuple(Convert.ToInt32(txt_dx_thred_min.Text));
            HTuple cd_thred_max_ = new HTuple(Convert.ToInt32(txt_dx_thred_max.Text));
            HTuple cd_area_min_ = new HTuple(Convert.ToInt32(txt_dx_area_min.Text));
            HTuple cd_area_max_ = new HTuple(Convert.ToInt32(txt_dx_area_max.Text));

            HOperatorSet.SetColor(hSmartWindowControl_map.HalconWindow, "red");
            richTextBox_log.AppendText("==================" + "\n");
            richTextBox_log.AppendText("1. Please Drawing Region Contains Two Horizontal Cells" + "\n");
            richTextBox_log.AppendText("2. Please Drawing Region Contains Two Vertical Cells" + "\n");
            if (load_image_rotate_ != null)
            {
                HOperatorSet.Rgb1ToGray(load_image_rotate_, out load_image_rotate_);
                HOperatorSet.ClearWindow(hSmartWindowControl_map.HalconWindow);
                HOperatorSet.DispImage(load_image_rotate_, hSmartWindowControl_map.HalconWindow);
                // 計算Cell之dx
                HOperatorSet.DrawRectangle1(hSmartWindowControl_map.HalconWindow, out dx_r1_, out dx_c1_, out dx_r2_, out dx_c2_);
                HOperatorSet.GenRectangle1(out dx_roi_rect_, dx_r1_, dx_c1_, dx_r2_, dx_c2_);
                HOperatorSet.ReduceDomain(load_image_rotate_, dx_roi_rect_, out cell_dx_roi_);

                // 計算Cell之dy
                HOperatorSet.DrawRectangle1(hSmartWindowControl_map.HalconWindow, out dy_1_r1_, out dy_1_c1_, out dy_1_r2_, out dy_1_c2_);
                HOperatorSet.GenRectangle1(out dy_1_roi_rect_, dy_1_r1_, dy_1_c1_, dy_1_r2_, dy_1_c2_);
                HOperatorSet.DrawRectangle1(hSmartWindowControl_map.HalconWindow, out dy_2_r1_, out dy_2_c1_, out dy_2_r2_, out dy_2_c2_);
                HOperatorSet.GenRectangle1(out dy_2_roi_rect_, dy_2_r1_, dy_2_c1_, dy_2_r2_, dy_2_c2_);
                HOperatorSet.Union2(dy_1_roi_rect_, dy_2_roi_rect_, out dy_roi_rect_);
                HOperatorSet.ReduceDomain(load_image_rotate_, dy_roi_rect_, out cell_dy_roi_);

                HOperatorSet.DispImage(load_image_rotate_, hSmartWindowControl_map.HalconWindow);
                HOperatorSet.DispRegion(dx_roi_rect_, hSmartWindowControl_map.HalconWindow);
                HOperatorSet.DispRegion(dy_roi_rect_, hSmartWindowControl_map.HalconWindow);
                richTextBox_log.AppendText("ROI Cell dxdy Success" + "\n");
                btn_dxdy_filter.Enabled = true;
            }
            else
                richTextBox_log.AppendText("ROI Cell dxdy Fail" + "\n");

            richTextBox_log.ScrollToCaret();
        }

        private void btn_execute_Click(object sender, EventArgs e)
        {
            int array_x_ = Convert.ToInt16(txt_array_x_count.Text);
            int array_y_ = Convert.ToInt16(txt_array_y_count.Text);
            List<HObject> all_capture_images_ = new List<HObject>();

            // 讀取所有檢測影像
            int index_ = 3; // 第一張檢測影像
            for (int i = 1; i <= array_x_ * array_y_; i++)
            {
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                HObject read_image_;

                if (index_ < 10)
                {
                    HOperatorSet.ReadImage(out read_image_, file_dirctory_ + "\\abc-1_M00" + index_ + "_F001.tif");
                }
                else if (index_ < 100) // (20180824) Jeff Revised!
                    HOperatorSet.ReadImage(out read_image_, file_dirctory_ + "\\abc-1_M0" + index_ + "_F001.tif");
                else // (20180824) Jeff Revised!
                    HOperatorSet.ReadImage(out read_image_, file_dirctory_ + "\\abc-1_M" + index_ + "_F001.tif");

                all_capture_images_.Add(read_image_.Clone());
                index_++;
            }
            //GC.Collect();
            //GC.WaitForPendingFinalizers();

            //locate_method_.execute(hSmartWindowControl_map, txt_save_path.Text, mark_image_1, mark_image_2, all_capture_images_);
        }

        private void btn_dxdy_filter_Click(object sender, EventArgs e)
        {
            HObject dx_thred_region_, dy_thred_region_;
            HTuple cd_thred_min_ = new HTuple(Convert.ToInt32(txt_dx_thred_min.Text));
            HTuple cd_thred_max_ = new HTuple(Convert.ToInt32(txt_dx_thred_max.Text));
            HTuple cd_area_min_ = new HTuple(Convert.ToInt32(txt_dx_area_min.Text));
            HTuple cd_area_max_ = new HTuple(Convert.ToInt32(txt_dx_area_max.Text));

            if (cell_dx_roi_ != null && cell_dy_roi_ != null)
            {
                HOperatorSet.Threshold(cell_dx_roi_, out dx_thred_region_, cd_thred_min_, cd_thred_max_);
                HOperatorSet.Connection(dx_thred_region_, out dx_thred_region_);
                HOperatorSet.SelectShape(dx_thred_region_, out dx_thred_region_, "area", "and", cd_area_min_, cd_area_max_);

                HOperatorSet.Threshold(cell_dy_roi_, out dy_thred_region_, cd_thred_min_, cd_thred_max_);
                HOperatorSet.Connection(dy_thred_region_, out dy_thred_region_);
                HOperatorSet.SelectShape(dy_thred_region_, out dy_thred_region_, "area", "and", cd_area_min_, cd_area_max_);

                HOperatorSet.DispImage(load_image_rotate_, hSmartWindowControl_map.HalconWindow);
                HOperatorSet.DispRegion(dx_thred_region_, hSmartWindowControl_map.HalconWindow);
                HOperatorSet.DispRegion(dy_thred_region_, hSmartWindowControl_map.HalconWindow);
                richTextBox_log.AppendText("Filter Cell dxdy Success" + "\n");
                btn_dx_calculate.Enabled = true;
            }
            else
                richTextBox_log.AppendText("Filter Cell dxdy Fail" + "\n");

            richTextBox_log.ScrollToCaret();
        }

        private void btn_dx_calculate_Click(object sender, EventArgs e)
        {
            this.locate_method_.cd_thred_min_ = new HTuple(Convert.ToInt32(txt_dx_thred_min.Text));
            this.locate_method_.cd_thred_max_ = new HTuple(Convert.ToInt32(txt_dx_thred_max.Text));
            this.locate_method_.cd_area_min_ = new HTuple(Convert.ToInt32(txt_dx_area_min.Text));
            this.locate_method_.cd_area_max_ = new HTuple(Convert.ToInt32(txt_dx_area_max.Text));


            double cell_pdx_, cell_pdy_;
            HOperatorSet.SetColor(hSmartWindowControl_map.HalconWindow, "red");
            if (!this.locate_method_.cal_cell_dxdy(cell_dx_roi_, cell_dy_roi_, out cell_pdx_, out cell_pdy_))
            {
                richTextBox_log.AppendText("==================" + "\n");
                richTextBox_log.AppendText("Calculate Cell dxdy Fail" + "\n");
            }
            else
            {
                this.locate_method_.cell_pdx_ = cell_pdx_;
                this.locate_method_.cell_pdy_ = cell_pdy_;
                txt_cell_pdx.Text = cell_pdx_.ToString();
                txt_cell_pdy.Text = cell_pdy_.ToString();
                richTextBox_log.AppendText("==================" + "\n");
                richTextBox_log.AppendText("Calculate Cell dxdy Success" + "\n");
                richTextBox_log.AppendText("Cell dx = " + cell_pdx_.ToString() + " pixel "+ "\n");
                richTextBox_log.AppendText("Cell dy = " + cell_pdy_.ToString() + " pixel " + "\n");
            }
            richTextBox_log.ScrollToCaret();
        }



        #region TextChanged Events

        #region Parameters of 【Motion】

        private void txt_array_x_count_TextChanged(object sender, EventArgs e)
        {
            this.locate_method_.array_x_count_ = Convert.ToInt32(txt_array_x_count.Text);
        }
        private void txt_array_x_pitch_TextChanged(object sender, EventArgs e)
        {
            this.locate_method_.array_x_rpitch_ = Convert.ToDouble(txt_array_x_pitch.Text);
        }
        private void txt_array_y_count_TextChanged(object sender, EventArgs e)
        {
            this.locate_method_.array_y_count_ = Convert.ToInt32(txt_array_y_count.Text);
        }
        private void txt_array_y_pitch_TextChanged(object sender, EventArgs e)
        {
            this.locate_method_.array_y_rpitch_ = Convert.ToDouble(txt_array_y_pitch.Text);
        }

        #endregion

        #region Parameters of 【Cell Info (Pixel)】

        private void txt_mark_pdx_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.locate_method_.mark_pdx_ = Convert.ToDouble(txt_mark_pdx.Text);
                this.locate_method_.mark_rdx_ = Convert.ToDouble(txt_mark_pdx.Text) * this.locate_method_.pixel_resolution_ / 1000;
            }
            catch
            { }
        }
        private void txt_mark_pdy_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.locate_method_.mark_pdy_ = Convert.ToDouble(txt_mark_pdy.Text);
                this.locate_method_.mark_rdy_ = Convert.ToDouble(txt_mark_pdy.Text) * this.locate_method_.pixel_resolution_ / 1000;
            }
            catch
            { }
        }
        private void txt_cell_pwidth_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.locate_method_.cell_pwidth_ = Convert.ToDouble(txt_cell_pwidth.Text);
            }
            catch
            { }
        }
        private void txt_cell_pheight_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.locate_method_.cell_pheight_ = Convert.ToDouble(txt_cell_pheight.Text);
            }
            catch
            { }
        }
        private void txt_cell_pdx_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.locate_method_.cell_pdx_ = Convert.ToDouble(txt_cell_pdx.Text);
            }
            catch
            { }
        }
        private void txt_cell_pdy_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.locate_method_.cell_pdy_ = Convert.ToDouble(txt_cell_pdy.Text);
            }
            catch
            { }
        }

        #endregion

        #region Parameters of 【Cell Info (mm)】

        private void textBox_mark_rdx_shift_TextChanged(object sender, EventArgs e) // (20181112) Jeff Revised!
        {
            //locate_method_.mark_rdx_shift = Convert.ToDouble(textBox_mark_rdx_shift.Text) * locate_method_.resize_;
            //locate_method_.unitTrans_mm2pixel();

            //double result;
            //if (double.TryParse(textBox_mark_rdx_shift.Text, out result))
            //{
            //    locate_method_.mark_rdx_shift = result * locate_method_.resize_;
            //    locate_method_.unitTrans_mm2pixel();
            //}
        }

        #endregion

        #region Parameters of 【基板Cell位置】

        /// <summary>
        /// 更新 groupBox 基板Cell位置 內之所有參數
        /// </summary>
        private void update_groupBox_cell_pos() // (20180903) Jeff Revised!
        {
            /* X方向 */
            update_groupBox_cell_pos_X();

            /* Y方向 */
            update_groupBox_cell_pos_Y();
        }

        /// <summary>
        /// comboBox_region_x 初始化 & 更新 groupBox 基板Cell位置 內之X方向參數
        /// </summary>
        private void update_groupBox_cell_pos_X() // (20191108) Jeff Revised!
        {
            /* X方向 */
            // comboBox_region_x 初始化
            comboBox_region_x.Items.Clear();
            for (int i = 1; i <= int.Parse(nud_num_region_x.Value.ToString()); i++)
                comboBox_region_x.Items.Add(i.ToString());
            comboBox_region_x.SelectedIndex = 0;

            // 更新參數
            //update_cell_pos_params_X();
        }

        /// <summary>
        /// 更新 groupBox 基板Cell位置 內之X方向參數
        /// </summary>
        private void update_cell_pos_params_X() // (20191108) Jeff Revised!
        {
            int index = Convert.ToInt32(comboBox_region_x.Text) - 1;
            textBox_reg_cell_x_count.Text = (this.locate_method_.cell_x_count_HTuple[index].I).ToString();
            textBox_dist_reg_rdx.Text = (this.locate_method_.dist_region_rdx_HTuple[index].D).ToString();
        }

        /// <summary>
        /// comboBox_region_y 初始化 & 更新 groupBox 基板Cell位置 內之Y方向參數
        /// </summary>
        private void update_groupBox_cell_pos_Y() // (20191108) Jeff Revised!
        {
            /* Y方向 */
            // comboBox_region_y 初始化
            comboBox_region_y.Items.Clear();
            for (int i = 1; i <= int.Parse(nud_num_region_y.Value.ToString()); i++)
                comboBox_region_y.Items.Add(i.ToString());
            comboBox_region_y.SelectedIndex = 0;

            // 更新參數
            //update_cell_pos_params_Y();
        }

        /// <summary>
        /// 更新 groupBox 基板Cell位置 內之Y方向參數
        /// </summary>
        private void update_cell_pos_params_Y() // (20191108) Jeff Revised!
        {
            int index = Convert.ToInt32(comboBox_region_y.Text) - 1;
            textBox_reg_cell_y_count.Text = (this.locate_method_.cell_y_count_HTuple[index].I).ToString();
            textBox_dist_reg_rdy.Text = (this.locate_method_.dist_region_rdy_HTuple[index].D).ToString();
        }
        
        private void nud_num_region_x_ValueChanged(object sender, EventArgs e) // (20191108) Jeff Revised!
        {
            if (b_loadParameters)
                return;
            this.locate_method_.Update_count_region_x(int.Parse(nud_num_region_x.Value.ToString()));
            this.update_groupBox_cell_pos_X();
        }

        private bool b_comboBox_region_xy_SelectedIndexChanged = false;
        private void comboBox_region_x_SelectedIndexChanged(object sender, EventArgs e) // (20180903) Jeff Revised!
        {
            b_comboBox_region_xy_SelectedIndexChanged = true;
            // 更新X方向參數
            update_cell_pos_params_X();
            b_comboBox_region_xy_SelectedIndexChanged = false;
        }
        
        private void nud_num_region_y_ValueChanged(object sender, EventArgs e) // (20191108) Jeff Revised!
        {
            if (b_loadParameters)
                return;
            this.locate_method_.Update_count_region_y(int.Parse(nud_num_region_y.Value.ToString()));
            update_groupBox_cell_pos_Y();
        }

        private void comboBox_region_y_SelectedIndexChanged(object sender, EventArgs e)
        {
            b_comboBox_region_xy_SelectedIndexChanged = true;
            // 更新Y方向參數
            update_cell_pos_params_Y();
            b_comboBox_region_xy_SelectedIndexChanged = false;
        }

        private void textBox_reg_cell_x_count_TextChanged(object sender, EventArgs e)
        {
            if (!b_comboBox_region_xy_SelectedIndexChanged)
            {
                int index = Convert.ToInt32(comboBox_region_x.Text) - 1;
                this.locate_method_.cell_x_count_HTuple[index] = Convert.ToInt32(textBox_reg_cell_x_count.Text);
            }
        }

        private void textBox_dist_reg_rdx_TextChanged(object sender, EventArgs e)
        {
            if (!b_comboBox_region_xy_SelectedIndexChanged)
            {
                int index = Convert.ToInt32(comboBox_region_x.Text) - 1;
                this.locate_method_.dist_region_rdx_HTuple[index] = Convert.ToDouble(textBox_dist_reg_rdx.Text);
            }
        }

        private void textBox_reg_cell_y_count_TextChanged(object sender, EventArgs e)
        {
            if (!b_comboBox_region_xy_SelectedIndexChanged)
            {
                int index = Convert.ToInt32(comboBox_region_y.Text) - 1;
                this.locate_method_.cell_y_count_HTuple[index] = Convert.ToInt32(textBox_reg_cell_y_count.Text);
            }
        }

        private void textBox_dist_reg_rdy_TextChanged(object sender, EventArgs e)
        {
            if (!b_comboBox_region_xy_SelectedIndexChanged)
            {
                int index = Convert.ToInt32(comboBox_region_y.Text) - 1;
                this.locate_method_.dist_region_rdy_HTuple[index] = Convert.ToDouble(textBox_dist_reg_rdy.Text);
            }
        }

        /// <summary>
        /// 【X方向總Cell顆數】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_cell_col_count_TextChanged(object sender, EventArgs e) // (20190719) Jeff Revised!
        {
            // 非數字，則直接return
            int value;
            if (!(int.TryParse(textBox_cell_col_count.Text, out value)))
                return;

            // 更新【Bypass Cell】
            if (value > 0)
            {
                if (int.Parse(nud_X_BypassCell.Value.ToString()) > value)
                    nud_X_BypassCell.Value = value;
                nud_X_BypassCell.Maximum = value;
            }
        }

        /// <summary>
        /// 【Y方向總Cell顆數】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_cell_row_count_TextChanged(object sender, EventArgs e) // (20190719) Jeff Revised!
        {
            // 非數字，則直接return
            int value;
            if (!(int.TryParse(textBox_cell_row_count.Text, out value)))
                return;

            // 更新【Bypass Cell】
            if (value > 0)
            {
                if (int.Parse(nud_Y_BypassCell.Value.ToString()) > value)
                    nud_Y_BypassCell.Value = value;
                nud_Y_BypassCell.Maximum = value;
            }
        }

        private List<Point> ListPt_BypassCell { get; set; } = new List<Point>(); // (20190719) Jeff Revised!
        /// <summary>
        /// 【新增】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Add_BypassCell_Click(object sender, EventArgs e) // (20190719) Jeff Revised!
        {
            Point pt = new Point(int.Parse(nud_X_BypassCell.Value.ToString()), int.Parse(nud_Y_BypassCell.Value.ToString()));
            if (ListPt_BypassCell.Contains(pt))
            {
                SystemSounds.Exclamation.Play();
                MessageBox.Show("已存在相同座標!", "溫馨提醒", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                ListPt_BypassCell.Add(pt);
                Update_listView_BypassCell();
            }
        }

        /// <summary>
        /// 更新 listView_BypassCell
        /// </summary>
        private void Update_listView_BypassCell() // (20190722) Jeff Revised!
        {
            listView_BypassCell.BeginUpdate();
            listView_BypassCell.Items.Clear();
            
            foreach (Point pt in ListPt_BypassCell)
            {
                ListViewItem lvi = new ListViewItem(pt.X.ToString());
                lvi.SubItems.Add(pt.Y.ToString());

                listView_BypassCell.Items.Add(lvi);
            }

            listView_BypassCell.EndUpdate();
        }

        /// <summary>
        /// 【清空】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_RemoveAll_BypassCell_Click(object sender, EventArgs e)
        {
            ListPt_BypassCell.Clear();
            Update_listView_BypassCell();
        }

        /// <summary>
        /// 【刪除】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Remove_BypassCell_Click(object sender, EventArgs e)
        {
            if (listView_BypassCell.SelectedIndices.Count <= 0)
            {
                ctrl_timer1 = listView_BypassCell;
                BackColor_ctrl_timer1_1 = Color.AliceBlue;
                BackColor_ctrl_timer1_2 = Color.Green;
                timer1.Enabled = true;
                SystemSounds.Exclamation.Play();
                MessageBox.Show("請選擇座標", "溫馨提醒", MessageBoxButtons.OK, MessageBoxIcon.Information);
                timer1.Enabled = false;
                ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
                return;
            }

            int index = listView_BypassCell.SelectedIndices[0];
            ListPt_BypassCell.RemoveAt(index);
            Update_listView_BypassCell();
        }

        #endregion

        #region Parameters of 【Calculate Cell Size】

        private void txt_cs_thred_min_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.locate_method_.cs_thred_min_ = Convert.ToInt32(txt_cs_thred_min.Text);
            }
            catch
            { }
        }
        private void txt_cs_thred_max_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.locate_method_.cs_thred_max_ = Convert.ToInt32(txt_cs_thred_max.Text);
            }
            catch
            { }
        }
        private void txt_cs_area_min_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.locate_method_.cs_area_min_ = Convert.ToInt32(txt_cs_area_min.Text);
            }
            catch
            { }
        }
        private void txt_cs_area_max_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.locate_method_.cs_area_max_ = Convert.ToInt32(txt_cs_area_max.Text);
            }
            catch
            { }
        }

        #endregion

        #region Parameters of 【Calculate Cell dxdy】

        private void txt_dx_thred_min_TextChanged(object sender, EventArgs e)
        {
            this.locate_method_.cd_thred_min_ = Convert.ToInt32(txt_dx_thred_min.Text);
        }
        private void txt_dx_thred_max_TextChanged(object sender, EventArgs e)
        {
            this.locate_method_.cd_thred_max_ = Convert.ToInt32(txt_dx_thred_max.Text);
        }

        private void btnEdgeSearchLoadImage1_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog_file.ShowDialog() == DialogResult.OK)
                {
                    string path_ = openFileDialog_file.FileName;
                    HOperatorSet.ReadImage(out mark_image_1, path_);
                    // 轉成RGB image (20180822) Jeff Revised!
                    HTuple Channels;
                    HOperatorSet.CountChannels(mark_image_1, out Channels);
                    if (Channels == 1)
                    {
                        HOperatorSet.Compose3(mark_image_1.Clone(), mark_image_1.Clone(), mark_image_1.Clone(), out mark_image_1); // (20181224) Jeff Revised!
                    }

                    hEdgeSearchWindow1.SetFullImagePart(new HImage(mark_image_1)); // (20180822) Jeff Revised!
                    HOperatorSet.DispColor(mark_image_1, hEdgeSearchWindow1.HalconWindow);
                    file_dirctory_ = Path.GetDirectoryName(path_); // path_的路徑刪掉最後的影像名稱         

                    /* // (20200219) Jeff Revised!
                    // 讀取XML檔是否有此基板Type
                    if (!affine_method_.load() || !locate_method_.load())
                    {
                        lab_xml_status.ForeColor = Color.Red;
                        lab_xml_status.Text = "Offline";
                    }
                    else
                    {
                        ui_parameters(false);
                        lab_xml_status.ForeColor = Color.LightGreen;
                        lab_xml_status.Text = "Online";
                    }

                    // 讀取根目錄下是否有Mark Model
                    lab_model_status.ForeColor = Color.LightGreen;
                    lab_model_status.Text = "Online";
                    

                    // 讀取根目錄下是否有Cell Golden Map
                    string golden_map_path_ = ModuleParamDirectory + PositionParam + @"\" + locate_method_.board_type_ + @"\" // (20180822) Jeff Revised!
                                           + locate_method_.array_x_count_.ToString() + "x" + locate_method_.array_y_count_.ToString() + "_cellmap.hobj";
                    HTuple file_exist_ = 0;
                    HOperatorSet.FileExists(golden_map_path_, out file_exist_);
                    if (file_exist_ == 1)
                    {
                        lab_golden_map.ForeColor = Color.LightGreen;
                        lab_golden_map.Text = "Online";
                    }
                    else
                    {
                        lab_golden_map.ForeColor = Color.Red;
                        lab_golden_map.Text = "Offline";
                    }
                    */
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Load Mark Image_1 fails", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSearchROI_Click(object sender, EventArgs e)
        {
            if (mark_image_1 != null)
            {
                if (affine_method_.create_Edge_ROI(hEdgeSearchWindow1.HalconWindow, mark_image_1))
                {
                    lab_model_status.ForeColor = Color.LightGreen;
                    lab_model_status.Text = "Online";
                }
                else
                    MessageBox.Show("Create Model Fails!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEdgeSearchLoadImage2_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog_file.ShowDialog() == DialogResult.OK)
                {
                    string path_ = openFileDialog_file.FileName;
                    HOperatorSet.ReadImage(out mark_image_2, path_);
                    // 轉成RGB image (20180822) Jeff Revised!
                    HTuple Channels;
                    HOperatorSet.CountChannels(mark_image_2, out Channels);
                    if (Channels == 1)
                    {
                        HOperatorSet.Compose3(mark_image_2.Clone(), mark_image_2.Clone(), mark_image_2.Clone(), out mark_image_2); // (20181224) Jeff Revised!
                    }

                    hEdgeSearchWindow2.SetFullImagePart(new HImage(mark_image_2)); // (20180822) Jeff Revised!
                    HOperatorSet.DispColor(mark_image_2, hEdgeSearchWindow2.HalconWindow);
                    //HOperatorSet.DispImage(mark_image_2, hWindowControl_mark_2.HalconWindow); // (20180822) Jeff Revised!
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Loading Mark Image_2  Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (mark_image_1 != null && mark_image_2 != null)
                btnEdgeSearchPoints.Enabled = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (affine_method_.Affine_degree_ != null) // (20181107) Jeff Revised!
            {
                HObject rotate_image_1, rotate_image_2;
                affine_method_.rotate_image_(hEdgeSearchWindow1.HalconWindow, mark_image_1, out rotate_image_1,
                                             checkBox_saveRotateImg.Checked, "RotateImage1.tiff"); // (20190112) Jeff Revised!
                affine_method_.rotate_image_(hEdgeSearchWindow2.HalconWindow, mark_image_2, out rotate_image_2,
                                             checkBox_saveRotateImg.Checked, "RotateImage2.tiff"); // (20190112) Jeff Revised!
                richTextBox_log.AppendText("Mark 1 AffinePoints = (" + affine_method_.affine_mark1_x_.D.ToString() + ", " + affine_method_.affine_mark1_y_.D.ToString() + ")" + "\n");
                richTextBox_log.ScrollToCaret();
            }
        }

        private void cbxSaveFindImage_CheckedChanged(object sender, EventArgs e)
        {
            if (!(cbxSaveFindImage.Checked))
                cbxSaveRotateImage.Checked = false;
        }

        private void cbxSaveRotateImage_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxSaveRotateImage.Checked)
                cbxSaveFindImage.Checked = true;
        }

        private void txbAngleShift_TextChanged(object sender, EventArgs e)
        {
            double Res;
            if (double.TryParse(txbAngleShift.Text, out Res))
                affine_method_.Affine_degree_shift = Res;
        }

        /// <summary>
        /// 【對位類型】切換
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboxAlignmentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox Obj = (ComboBox)sender;
            if (Obj.SelectedIndex >= 0)
            {
                affine_method_.EdgeSearchParam.AlignmentType = (AngleAffineMethod.enuAlignmentType)Obj.SelectedIndex;

                UpdateUIDisplay();
            }
        }

        TabControl HidePage { get; set; } = new TabControl();
        public void UpdateUIDisplay()
        {
            foreach (TabPage P in tbControlAlignment.TabPages)
            {
                HidePage.TabPages.Add(P);
            }
            string Name = ((AngleAffineMethod.enuAlignmentType)comboxAlignmentType.SelectedIndex).ToString();
            tbControlAlignment.TabPages.Add(HidePage.TabPages[Name]);

            clsLanguage.clsLanguage.SetLanguateToControls(this.tbControlAlignment, true); // (20200214) Jeff Revised!
        }

        private void btnEdgeSearchPoints_Click(object sender, EventArgs e)
        {
            bool b_status_ = false;
            if (mark_image_1 != null && mark_image_2 != null)
            {
                b_status_ = affine_method_.FindEdgePoins(mark_image_1.Clone(), mark_image_2.Clone(), cbxSaveFindImage.Checked);
            }
            this.locate_method_.affine_degree_ = affine_method_.Affine_degree_;
            this.locate_method_.origin_mark_pos_x_ = affine_method_.mark_x_[0];
            this.locate_method_.origin_mark_pos_y_ = affine_method_.mark_y_[0];

            try
            {
                if (affine_method_.GetEdgeResRegion1 != null)
                {
                    HOperatorSet.SetColor(hEdgeSearchWindow1.HalconWindow, "green");
                    HOperatorSet.SetDraw(hEdgeSearchWindow1.HalconWindow, "margin");
                    HOperatorSet.DispObj(affine_method_.GetEdgeResRegion1, hEdgeSearchWindow1.HalconWindow);
                }
                if (affine_method_.GetEdgeResRegion2 != null)
                {
                    HOperatorSet.SetColor(hEdgeSearchWindow2.HalconWindow, "green");
                    HOperatorSet.SetDraw(hEdgeSearchWindow2.HalconWindow, "margin");
                    HOperatorSet.DispObj(affine_method_.GetEdgeResRegion2, hEdgeSearchWindow2.HalconWindow);
                }
                if (affine_method_.Affine_degree_ != null)
                {
                    HOperatorSet.SetColor(hEdgeSearchWindow1.HalconWindow, "blue");
                    HOperatorSet.SetDraw(hEdgeSearchWindow1.HalconWindow, "fill");
                    HOperatorSet.DispCircle(hEdgeSearchWindow1.HalconWindow, affine_method_.mark_y_[0], affine_method_.mark_x_[0], 10);
                    HOperatorSet.SetColor(hEdgeSearchWindow2.HalconWindow, "blue");
                    HOperatorSet.SetDraw(hEdgeSearchWindow2.HalconWindow, "fill");
                    HOperatorSet.DispCircle(hEdgeSearchWindow2.HalconWindow, affine_method_.mark_y_[1], affine_method_.mark_x_[1], 10);

                    btnEdgeSearchRotate.Enabled = true;
                    richTextBox_log.AppendText("============" + "\n");
                    richTextBox_log.AppendText("Affine Degree = " + affine_method_.Affine_degree_ + "\n");
                    richTextBox_log.AppendText("Mark 1 Points = (" + affine_method_.mark_x_[0].D.ToString() + ", " + affine_method_.mark_y_[0].D.ToString() + ")" + "\n");
                    richTextBox_log.AppendText("Mark 2 Points = (" + affine_method_.mark_x_[1].D.ToString() + ", " + affine_method_.mark_y_[1].D.ToString() + ")" + "\n");
                    richTextBox_log.ScrollToCaret();
                }
                else
                {
                    btnEdgeSearchRotate.Enabled = false;
                    MessageBox.Show("Find marks fail~", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Find marks fail~", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void textBox_MinContrast_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt_dx_area_min_TextChanged(object sender, EventArgs e)
        {
            this.locate_method_.cd_area_min_ = Convert.ToInt32(txt_dx_area_min.Text);
        }

        private void nudThresholdMin_ValueChanged(object sender, EventArgs e)
        {
            affine_method_.EdgeSearchParam.Th_Low = Convert.ToInt32(nudThresholdMin.Value.ToString());
        }

        private void nudnudThresholdMax_ValueChanged(object sender, EventArgs e)
        {
            affine_method_.EdgeSearchParam.Th_High = Convert.ToInt32(nudnudThresholdMax.Value.ToString());
        }

        private void nudErosion_ValueChanged(object sender, EventArgs e)
        {
            affine_method_.EdgeSearchParam.DeNoiseSize = Convert.ToInt32(nudErosion.Value.ToString());
        }

        private void nudOpening_ValueChanged(object sender, EventArgs e)
        {
            affine_method_.EdgeSearchParam.OpeningSize = Convert.ToInt32(nudOpening.Value.ToString());
        }

        private void nudClosing_ValueChanged(object sender, EventArgs e)
        {
            affine_method_.EdgeSearchParam.ClosingSize = Convert.ToInt32(nudClosing.Value.ToString());
        }

        private void comboBoxPointType1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox Obj = (ComboBox)sender;
            if (Obj.SelectedIndex >= 0)
            {
                affine_method_.EdgeSearchParam.SearchType1 = (AngleAffineMethod.enuSearchType)Obj.SelectedIndex;
            }
        }

        private void comboBoxPointType2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox Obj = (ComboBox)sender;
            if (Obj.SelectedIndex >= 0)
            {
                affine_method_.EdgeSearchParam.SearchType2 = (AngleAffineMethod.enuSearchType)Obj.SelectedIndex;
            }
        }

        private void comboBoxTransType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox Obj = (ComboBox)sender;
            if (Obj.SelectedIndex >= 0)
            {
                affine_method_.EdgeSearchParam.TransType = Obj.SelectedItem.ToString();
            }
        }

        private void txt_dx_area_max_TextChanged(object sender, EventArgs e)
        {
            this.locate_method_.cd_area_max_ = Convert.ToInt32(txt_dx_area_max.Text);
        }
        
        #endregion

        #endregion

        #endregion

        public LocateMethod LocateMethodObj
        {
            get { return this.locate_method_; }
        }

        private void btnSetTh_Click(object sender, EventArgs e)
        {
            if (mark_image_1 == null)
            {
                MessageBox.Show("請先讀取影像");
                return;
            }

            HObject SrcImg;
            HObject TeachImg;
            HOperatorSet.GenEmptyObj(out TeachImg);
            
            HOperatorSet.CopyImage(mark_image_1, out SrcImg);

            HTuple Ch;
            HOperatorSet.CountChannels(SrcImg, out Ch);
            if (Ch == 1)
                TeachImg = SrcImg.Clone();
            else
            {
                HOperatorSet.Rgb1ToGray(SrcImg, out TeachImg);
            }

            FormThresholdAdjust MyForm = new FormThresholdAdjust(TeachImg,
                                                                 affine_method_.EdgeSearchParam.Th_Low,
                                                                 affine_method_.EdgeSearchParam.Th_High, FormThresholdAdjust.enType.Dual);

            MyForm.ShowDialog();

            if (MyForm.DialogResult == DialogResult.OK)
            {
                int ThMin, ThMax;
                MyForm.GetThreshold(out ThMin, out ThMax);

                nudnudThresholdMax.Value = Convert.ToDecimal(ThMax.ToString());
                nudThresholdMin.Value = Convert.ToDecimal(ThMin.ToString());
            }
            TeachImg.Dispose();
            SrcImg.Dispose();
        }
        
        private void cbxAutoEnabled_CheckedChanged(object sender, EventArgs e)
        {
            affine_method_.AutoParam = cbxAutoEnabled.Checked;
        }
        public int MaxTh = 255;
        public int MinTh = 128;
        
        private void btnThSetting_Click(object sender, EventArgs e)
        {
            if (mark_image_1 == null)
            {
                MessageBox.Show("請先讀取影像");
                return;
            }

            HObject SrcImg;
            HObject TeachImg;
            HOperatorSet.GenEmptyObj(out TeachImg);

            HOperatorSet.CopyImage(mark_image_1, out SrcImg);

            HTuple Ch;
            HOperatorSet.CountChannels(SrcImg, out Ch);
            if (Ch == 1)
                TeachImg = SrcImg.Clone();
            else
            {
                HOperatorSet.Rgb1ToGray(SrcImg, out TeachImg);
            }

            FormThresholdAdjust MyForm = new FormThresholdAdjust(TeachImg,
                                                                 MinTh,
                                                                 MaxTh, FormThresholdAdjust.enType.Dual);

            MyForm.ShowDialog();

            if (MyForm.DialogResult == DialogResult.OK)
            {
                int ThMin, ThMax;
                MyForm.GetThreshold(out ThMin, out ThMax);

                MaxTh = Convert.ToInt32(ThMax.ToString());
                MinTh = Convert.ToInt32(ThMin.ToString());
            }
            TeachImg.Dispose();
            SrcImg.Dispose();
        }
        
        public AngleAffineMethod AffineMethodObj
        {
            get { return this.affine_method_; }
        }

        /// <summary>
        /// 是否正在載入參數至GUI
        /// </summary>
        private bool b_loadParameters { get; set; } = false; // (20190727) Jeff Revised!
        /// <summary>
        /// 將UI內容與各Class參數互傳
        /// </summary>
        /// <param name="ui_2_parameters_">True=UI傳至Class ; False=Class傳至UI</param>
        private void ui_parameters(bool ui_2_parameters_) // (20200429) Jeff Revised!
        {
            try
            {
                if (ui_2_parameters_)
                {
                    #region 將UI內容回傳至Class

                    /* Class of 【AngleAffineMethod】 */
                    this.affine_method_.pixel_resolution_ = Convert.ToDouble(txt_pixel_resolution.Text);
                    //affine_method_.Motion_shift_dist_y_hat = Convert.ToDouble(txt_mark2_my.Text) - Convert.ToDouble(txt_mark1_my.Text);
                    if (!checkBox_Y_dir_inv.Checked) // (20190516) Jeff Revised!
                    {
                        this.affine_method_.Motion_shift_dist_y_hat = Convert.ToDouble(txt_mark2_my.Text) - Convert.ToDouble(txt_mark1_my.Text);
                    }
                    else
                        affine_method_.Motion_shift_dist_y_hat = Convert.ToDouble(txt_mark1_my.Text) - Convert.ToDouble(txt_mark2_my.Text);
                    this.affine_method_.Contrast_string = textBox_Contrast.Text; // (20181113) Jeff Revised!
                    this.affine_method_.string_2_HTuple(textBox_Contrast.Text, out this.affine_method_.Contrast_HTuple); // (20181113) Jeff Revised!                   
                    this.affine_method_.MinContrast = Convert.ToInt32(textBox_MinContrast.Text);
                    this.affine_method_.match_score_ = Convert.ToDouble(txt_matching_score.Text);
                    this.affine_method_.Affine_degree_shift = Convert.ToDouble(txt_AngleShift.Text); // (20190112) Jeff Revised!

                    // (20200429) Jeff Revised!
                    this.affine_method_.angleAffineMethod_New.DefaultMarksPosEnable = this.cbx_DefaultMarksPosEnable.Checked;
                    this.affine_method_.angleAffineMethod_New.defMark1_x = Convert.ToDouble(this.textBox_defMark1_x.Text);
                    this.affine_method_.angleAffineMethod_New.defMark1_y = Convert.ToDouble(this.textBox_defMark1_y.Text);
                    this.affine_method_.angleAffineMethod_New.defMark2_x = Convert.ToDouble(this.textBox_defMark2_x.Text);
                    this.affine_method_.angleAffineMethod_New.defMark2_y = Convert.ToDouble(this.textBox_defMark2_y.Text);
                    this.affine_method_.angleAffineMethod_New.defAffineAngle = Convert.ToDouble(this.textBox_defAffineAngle.Text);

                    /* Class of 【LocateMethod】 */
                    this.locate_method_.pixel_resolution_ = Convert.ToDouble(txt_pixel_resolution.Text);
                    this.locate_method_.frame_pwidth_ = Convert.ToInt32(textBox_frame_pwidth.Text);
                    this.locate_method_.frame_pheight_ = Convert.ToInt32(textBox_frame_pheight.Text);

                    this.locate_method_.LocateMethodRecipe.VisualPosEnable = cbx_VisualPosEnable.Checked; // (20191216) MIL Jeff Revised!
                    if (!(cbx_VisualPosEnable.Checked))
                        affine_method_.Affine_degree_ = this.locate_method_.affine_degree_ = 0.0;

                    this.locate_method_.size_sample_rx = Convert.ToDouble(textBox_size_sample_rx.Text);
                    this.locate_method_.size_sample_ry = Convert.ToDouble(textBox_size_sample_ry.Text);
                    this.locate_method_.resize_ = Convert.ToDouble(textBox_resize.Text);
                    this.locate_method_.X_dir_inv = Convert.ToInt32(checkBox_X_dir_inv.Checked); // (20180901) Jeff Revised!
                    this.locate_method_.Y_dir_inv = Convert.ToInt32(checkBox_Y_dir_inv.Checked); // (20180901) Jeff Revised!
                    this.locate_method_.mark1_rpos_x_ = Convert.ToDouble(txt_mark1_mx.Text);
                    this.locate_method_.mark1_rpos_y_ = Convert.ToDouble(txt_mark1_my.Text);
                    this.locate_method_.mark2_rpos_x_ = Convert.ToDouble(txt_mark2_mx.Text);
                    this.locate_method_.mark2_rpos_y_ = Convert.ToDouble(txt_mark2_my.Text);
                    this.locate_method_.start_rpos_x_ = Convert.ToDouble(txt_start_mx.Text);
                    this.locate_method_.start_rpos_y_ = Convert.ToDouble(txt_start_my.Text);
                    this.locate_method_.array_x_count_ = Convert.ToInt32(txt_array_x_count.Text);
                    this.locate_method_.array_x_rpitch_ = Convert.ToDouble(txt_array_x_pitch.Text);
                    this.locate_method_.array_y_count_ = Convert.ToInt32(txt_array_y_count.Text);
                    this.locate_method_.array_y_rpitch_ = Convert.ToDouble(txt_array_y_pitch.Text);
                    this.locate_method_.cell_col_count_ = Convert.ToInt32(textBox_cell_col_count.Text); // (20180901) Jeff Revised!
                    this.locate_method_.cell_row_count_ = Convert.ToInt32(textBox_cell_row_count.Text); // (20180901) Jeff Revised!
                    this.locate_method_.mark_rdx_ = Convert.ToDouble(txt_mark_rdx.Text);
                    this.locate_method_.mark_rdy_ = Convert.ToDouble(txt_mark_rdy.Text);
                    this.locate_method_.mark_rdx_shift = Convert.ToDouble(textBox_mark_rdx_shift.Text);
                    this.locate_method_.mark_rdy_shift = Convert.ToDouble(textBox_mark_rdy_shift.Text);
                    this.locate_method_.cell_rwidth_ = Convert.ToDouble(txt_cell_rwidth.Text);
                    this.locate_method_.cell_rheight_ = Convert.ToDouble(txt_cell_rheight.Text);
                    this.locate_method_.cell_rdx_ = Convert.ToDouble(txt_cell_rdx.Text);
                    this.locate_method_.cell_rdy_ = Convert.ToDouble(txt_cell_rdy.Text);
                    this.locate_method_.mark_pdx_ = Convert.ToDouble(txt_mark_pdx.Text);
                    this.locate_method_.mark_pdy_ = Convert.ToDouble(txt_mark_pdy.Text);
                    this.locate_method_.cell_pwidth_ = Convert.ToDouble(txt_cell_pwidth.Text);
                    this.locate_method_.cell_pheight_ = Convert.ToDouble(txt_cell_pheight.Text);
                    this.locate_method_.cell_pdx_ = Convert.ToDouble(txt_cell_pdx.Text);
                    this.locate_method_.cell_pdy_ = Convert.ToDouble(txt_cell_pdy.Text);
                    this.locate_method_.num_region_x_ = int.Parse(nud_num_region_x.Value.ToString()); // (20191108) Jeff Revised!
                    this.locate_method_.num_region_y_ = int.Parse(nud_num_region_y.Value.ToString()); // (20191108) Jeff Revised!
                    this.locate_method_.center_x_pix_shift = Convert.ToInt32(textBox_center_x_pix_shift.Text); // (20181022) Jeff Revised!
                    this.locate_method_.center_y_pix_shift = Convert.ToInt32(textBox_center_y_pix_shift.Text); // (20181022) Jeff Revised!

                    this.locate_method_.LocateMethodRecipe.ListPt_BypassCell = ListPt_BypassCell.ToList(); // (20190722) Jeff Revised!

                    this.locate_method_.cs_thred_min_ = Convert.ToInt32(txt_cs_thred_min.Text);
                    this.locate_method_.cs_thred_max_ = Convert.ToInt32(txt_cs_thred_max.Text);
                    this.locate_method_.cs_area_min_ = Convert.ToInt32(txt_cs_area_min.Text);
                    this.locate_method_.cs_area_max_ = Convert.ToInt32(txt_cs_area_max.Text);
                    this.locate_method_.cd_thred_min_ = Convert.ToInt32(txt_dx_thred_min.Text);
                    this.locate_method_.cd_thred_max_ = Convert.ToInt32(txt_dx_thred_max.Text);
                    this.locate_method_.cd_area_min_ = Convert.ToInt32(txt_dx_area_min.Text);
                    this.locate_method_.cd_area_max_ = Convert.ToInt32(txt_dx_area_max.Text);
                    this.UpdateParamFormUI();
                    // Resize parameters
                    this.locate_method_.b_resizeParam = false;
                    this.locate_method_.resize_parameters(); // (20180829) Jeff Revised!
                    // 單位轉換 (mm → pixel) (20190603) Jeff Revised!
                    this.locate_method_.unitTrans_mm2pixel();

                    // (20190516) Jeff Revised!
                    try
                    {
                        // 定位測試資訊
                        affine_method_.B_control_marks_dist = cbx_controlMarksDist.Checked;
                        affine_method_.Diff_controlMarksDist = Convert.ToDouble(txt_diff_controlMarksDist.Text);

                        // 運動座標
                        if (!checkBox_X_dir_inv.Checked)
                        {
                            affine_method_.Motion_shift_dist_x_hat = Convert.ToDouble(txt_mark2_mx.Text) - Convert.ToDouble(txt_mark1_mx.Text);
                        }
                        else
                            affine_method_.Motion_shift_dist_x_hat = Convert.ToDouble(txt_mark1_mx.Text) - Convert.ToDouble(txt_mark2_mx.Text);

                        // 基板座標
                        affine_method_.Mark1_rpos_sample_X = Convert.ToDouble(txt_mark1_sample_X.Text);
                        affine_method_.Mark1_rpos_sample_Y = Convert.ToDouble(txt_mark1_sample_Y.Text);
                        affine_method_.Mark2_rpos_sample_X = Convert.ToDouble(txt_mark2_sample_X.Text);
                        affine_method_.Mark2_rpos_sample_Y = Convert.ToDouble(txt_mark2_sample_Y.Text);
                        affine_method_.Marks_dist_sample = Convert.ToDouble(txt_marks_dist.Text);
                        affine_method_.Angle_degree_marks_sample = Convert.ToDouble(txt_marks_angle.Text);
                    }
                    catch
                    { }

                    #endregion

                }
                else
                {
                    #region 將Class內容回傳至UI

                    this.b_loadParameters = true; // (20190727) Jeff Revised!

                    txt_type.Text = this.locate_method_.BoardType; // (20180830) Jeff Revised!

                    /* Class of 【AngleAffineMethod】 */
                    txt_pixel_resolution.Text = affine_method_.pixel_resolution_.ToString();
                    textBox_Contrast.Text = affine_method_.Contrast_string;
                    textBox_MinContrast.Text = affine_method_.MinContrast.ToString();
                    txt_matching_score.Text = affine_method_.match_score_.ToString();
                    txt_AngleShift.Text = affine_method_.Affine_degree_shift.ToString(); // (20190112) Jeff Revised!

                    // (20200429) Jeff Revised!
                    this.cbx_DefaultMarksPosEnable.Checked = this.affine_method_.angleAffineMethod_New.DefaultMarksPosEnable;
                    this.textBox_defMark1_x.Text = this.affine_method_.angleAffineMethod_New.defMark1_x.ToString();
                    this.textBox_defMark1_y.Text = this.affine_method_.angleAffineMethod_New.defMark1_y.ToString();
                    this.textBox_defMark2_x.Text = this.affine_method_.angleAffineMethod_New.defMark2_x.ToString();
                    this.textBox_defMark2_y.Text = this.affine_method_.angleAffineMethod_New.defMark2_y.ToString();
                    this.textBox_defAffineAngle.Text = this.affine_method_.angleAffineMethod_New.defAffineAngle.ToString();

                    /* Class of 【LocateMethod】 */
                    // Resize parameters back to the original size
                    this.locate_method_.resize_parameters_back(); // (20180829) Jeff Revised!

                    txt_pixel_resolution.Text = this.locate_method_.pixel_resolution_.ToString();
                    textBox_frame_pwidth.Text = this.locate_method_.frame_pwidth_.ToString();
                    textBox_frame_pheight.Text = this.locate_method_.frame_pheight_.ToString();
                    cbx_VisualPosEnable.Checked = this.locate_method_.LocateMethodRecipe.VisualPosEnable; // (20191216) MIL Jeff Revised!
                    textBox_size_sample_rx.Text = this.locate_method_.size_sample_rx.ToString();
                    textBox_size_sample_ry.Text = this.locate_method_.size_sample_ry.ToString();
                    textBox_resize.Text = this.locate_method_.resize_.ToString();
                    checkBox_X_dir_inv.Checked = Convert.ToBoolean(this.locate_method_.X_dir_inv); // (20180901) Jeff Revised!
                    checkBox_Y_dir_inv.Checked = Convert.ToBoolean(this.locate_method_.Y_dir_inv); // (20180901) Jeff Revised!
                    txt_mark1_mx.Text = this.locate_method_.mark1_rpos_x_.ToString();
                    txt_mark1_my.Text = this.locate_method_.mark1_rpos_y_.ToString();
                    txt_mark2_mx.Text = this.locate_method_.mark2_rpos_x_.ToString();
                    txt_mark2_my.Text = this.locate_method_.mark2_rpos_y_.ToString();
                    txt_start_mx.Text = this.locate_method_.start_rpos_x_.ToString();
                    txt_start_my.Text = this.locate_method_.start_rpos_y_.ToString();
                    txt_array_x_count.Text = this.locate_method_.array_x_count_.ToString();
                    txt_array_x_pitch.Text = this.locate_method_.array_x_rpitch_.ToString();
                    txt_array_y_count.Text = this.locate_method_.array_y_count_.ToString();
                    txt_array_y_pitch.Text = this.locate_method_.array_y_rpitch_.ToString();
                    textBox_cell_col_count.Text = this.locate_method_.cell_col_count_.ToString(); // (20180901) Jeff Revised!
                    textBox_cell_row_count.Text = this.locate_method_.cell_row_count_.ToString(); // (20180901) Jeff Revised!
                    txt_mark_rdx.Text = this.locate_method_.mark_rdx_.ToString();
                    txt_mark_rdy.Text = this.locate_method_.mark_rdy_.ToString();
                    textBox_mark_rdx_shift.Text = this.locate_method_.mark_rdx_shift.ToString();
                    textBox_mark_rdy_shift.Text = this.locate_method_.mark_rdy_shift.ToString();
                    txt_cell_rwidth.Text = this.locate_method_.cell_rwidth_.ToString();
                    txt_cell_rheight.Text = this.locate_method_.cell_rheight_.ToString();
                    txt_cell_rdx.Text = this.locate_method_.cell_rdx_.ToString();
                    txt_cell_rdy.Text = this.locate_method_.cell_rdy_.ToString();
                    txt_mark_pdx.Text = this.locate_method_.mark_pdx_.ToString();
                    txt_mark_pdy.Text = this.locate_method_.mark_pdy_.ToString();
                    txt_cell_pwidth.Text = this.locate_method_.cell_pwidth_.ToString();
                    txt_cell_pheight.Text = this.locate_method_.cell_pheight_.ToString();
                    txt_cell_pdx.Text = this.locate_method_.cell_pdx_.ToString();
                    txt_cell_pdy.Text = this.locate_method_.cell_pdy_.ToString();
                    nud_num_region_x.Value = this.locate_method_.num_region_x_; // (20191108) Jeff Revised!
                    nud_num_region_y.Value = this.locate_method_.num_region_y_; // (20191108) Jeff Revised!
                    update_groupBox_cell_pos(); // (20180903) Jeff Revised!
                    textBox_center_x_pix_shift.Text = this.locate_method_.center_x_pix_shift.ToString(); // (20181022) Jeff Revised!
                    textBox_center_y_pix_shift.Text = this.locate_method_.center_y_pix_shift.ToString(); // (20181022) Jeff Revised!

                    ListPt_BypassCell = this.locate_method_.LocateMethodRecipe.ListPt_BypassCell.ToList(); // (20190722) Jeff Revised!
                    Update_listView_BypassCell();

                    txt_cs_thred_min.Text = this.locate_method_.cs_thred_min_.ToString();
                    txt_cs_thred_max.Text = this.locate_method_.cs_thred_max_.ToString();
                    txt_cs_area_min.Text = this.locate_method_.cs_area_min_.ToString();
                    txt_cs_area_max.Text = this.locate_method_.cs_area_max_.ToString();
                    txt_dx_thred_min.Text = this.locate_method_.cd_thred_min_.ToString();
                    txt_dx_thred_max.Text = this.locate_method_.cd_thred_max_.ToString();
                    txt_dx_area_min.Text = this.locate_method_.cd_area_min_.ToString();
                    txt_dx_area_max.Text = this.locate_method_.cd_area_max_.ToString();
                    UpdateControl();
                    // Resize parameters
                    this.locate_method_.resize_parameters(); // (20180829) Jeff Revised!

                    // (20190516) Jeff Revised!
                    try
                    {
                        // 定位測試資訊
                        cbx_controlMarksDist.Checked = affine_method_.B_control_marks_dist;
                        txt_diff_controlMarksDist.Text = affine_method_.Diff_controlMarksDist.ToString();

                        // 基板座標
                        txt_mark1_sample_X.Text = affine_method_.Mark1_rpos_sample_X.ToString();
                        txt_mark1_sample_Y.Text = affine_method_.Mark1_rpos_sample_Y.ToString();
                        txt_mark2_sample_X.Text = affine_method_.Mark2_rpos_sample_X.ToString();
                        txt_mark2_sample_Y.Text = affine_method_.Mark2_rpos_sample_Y.ToString();
                        txt_marks_dist.Text = affine_method_.Marks_dist_sample.ToString();
                        txt_marks_angle.Text = affine_method_.Angle_degree_marks_sample.ToString();
                    }
                    catch
                    { }

                    // Initialize all variables (20190611) Jeff Revised!
                    Release_All_concat_images_();
                    Pre_Name_Img = "abc-1";
                    Index_Light = "001";

                    #region 【計算瑕疵座標測試】

                    // (20190727) Jeff Revised!
                    cbx_NG_classification.Checked = this.locate_method_.b_Defect_Classify;
                    cbx_Priority.Checked = this.locate_method_.b_Defect_priority;
                    cbx_Recheck.Checked = this.locate_method_.b_Defect_Recheck;

                    this.textBox_Path_DefectTest.Text = this.locate_method_.Path_DefectTest; // (20200429) Jeff Revised!

                    #endregion

                    this.b_loadParameters = false; // (20190727) Jeff Revised!

                    #endregion

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "UI&Parameters Translate Fail !");
            }
        }
        
        public void UpdateControl()
        {
            comboxAlignmentType.SelectedIndex = (int)affine_method_.EdgeSearchParam.AlignmentType;
            nudThresholdMin.Value = Convert.ToDecimal(affine_method_.EdgeSearchParam.Th_Low);
            nudnudThresholdMax.Value = Convert.ToDecimal(affine_method_.EdgeSearchParam.Th_High);

            nudErosion.Value = Convert.ToDecimal(affine_method_.EdgeSearchParam.DeNoiseSize);
            nudOpening.Value = Convert.ToDecimal(affine_method_.EdgeSearchParam.OpeningSize);
            nudClosing.Value = Convert.ToDecimal(affine_method_.EdgeSearchParam.ClosingSize);

            comboBoxPointType1.SelectedIndex = (int)affine_method_.EdgeSearchParam.SearchType1;
            comboBoxPointType2.SelectedIndex = (int)affine_method_.EdgeSearchParam.SearchType2;
            comboBoxTransType.SelectedIndex = Convert2Index(affine_method_.EdgeSearchParam.TransType);
        }

        public void UpdateParamFormUI()
        {
            affine_method_.EdgeSearchParam.Th_Low = Convert.ToInt32(nudThresholdMin.Value.ToString());
            affine_method_.EdgeSearchParam.Th_High = Convert.ToInt32(nudnudThresholdMax.Value.ToString());
            affine_method_.EdgeSearchParam.DeNoiseSize = Convert.ToInt32(nudErosion.Value.ToString());
            affine_method_.EdgeSearchParam.OpeningSize = Convert.ToInt32(nudOpening.Value.ToString());
            affine_method_.EdgeSearchParam.ClosingSize = Convert.ToInt32(nudClosing.Value.ToString());
            if (comboBoxPointType1.SelectedIndex >= 0)
                affine_method_.EdgeSearchParam.SearchType1 = (AngleAffineMethod.enuSearchType)comboBoxPointType1.SelectedIndex;
            if (comboBoxPointType2.SelectedIndex >= 0)
                affine_method_.EdgeSearchParam.SearchType2 = (AngleAffineMethod.enuSearchType)comboBoxPointType2.SelectedIndex;
            if (comboBoxTransType.SelectedIndex >= 0)
                affine_method_.EdgeSearchParam.TransType = comboBoxTransType.SelectedItem.ToString();
        }
        
        public int Convert2Index(string Name)
        {
            int Res = 0;
            switch(Name)
            {
                case "rectangle1":
                    {
                        Res = 0;
                    }
                    break;
                case "rectangle2":
                    {
                        Res = 1;
                    }
                    break;
                case "convex":
                    {
                        Res = 2;
                    }
                    break;
                default:
                    Res = 0;
                    break;
            }
            return Res;
        }
        
        public string Info
        {
            get { return txt_type.Text; }
        }



        #region 【計算瑕疵座標測試】

        /// <summary>
        /// A面
        /// </summary>
        private LocateMethod locate_method_A = new LocateMethod(); // (20200429) Jeff Revised!

        /// <summary>
        /// B面
        /// </summary>
        private LocateMethod locate_method_B = new LocateMethod(); // (20200429) Jeff Revised!
        
        /// <summary>
        /// 雙面
        /// </summary>
        private DefectTest_AB_Recipe Recipe_AB = new DefectTest_AB_Recipe(); // (20200429) Jeff Revised!

        /// <summary>
        /// 工單切換
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_Recipe_CheckedChanged(object sender, EventArgs e) // (20200429) Jeff Revised!
        {
            RadioButton rbtn = sender as RadioButton;
            string tag = rbtn.Tag.ToString();

            if (rbtn.Checked == false)
            {
                // 儲存原先工單
                switch (tag)
                {
                    case "A面":
                            this.locate_method_A = this.locate_method_;
                        break;
                    case "B面":
                            this.locate_method_B = this.locate_method_;
                        break;
                    case "雙面":
                        {
                            // 更新啟用狀態
                            this.cbx_NG_classification.Enabled = true;
                            this.cbx_Priority.Enabled = true;
                            this.cbx_fake_defect.Enabled = true;
                            this.button_clear_defect.Enabled = true;
                            this.groupBox_Recheck.Enabled = true;
                            this.groupBox_DefectsClassify.Enabled = this.cbx_NG_classification.Checked;
                            this.groupBox_NGClassify_Statistics.Visible = false;
                        }
                        break;
                }

                return;
            }
            
            // 載入工單
            switch (tag)
            {
                case "A面":
                        this.locate_method_ = this.locate_method_A;
                    break;
                case "B面":
                        this.locate_method_ = this.locate_method_B;
                    break;
                case "雙面":
                    {
                        // 更新雙面工單
                        //this.Recipe_AB.Set_LocateMethod(this.locate_method_A, this.locate_method_B);

                        // 更新啟用狀態
                        this.cbx_NG_classification.Enabled = false;
                        this.cbx_Priority.Enabled = false;
                        this.cbx_fake_defect.Enabled = false;
                        this.button_clear_defect.Enabled = false;
                        this.groupBox_Recheck.Enabled = false;
                        this.groupBox_DefectsClassify.Enabled = false;

                        // 瑕疵分類統計 (人工覆判結果) // (20200429) Jeff Revised!
                        this.groupBox_NGClassify_Statistics.Visible = true;
                        this.cbx_B_NG_Classify.CheckedChanged -= new System.EventHandler(this.cbx_B_NG_Classify_CheckedChanged);
                        this.cbx_B_NG_Classify.Checked = this.Recipe_AB.B_NG_Classify;
                        this.listView_NGClassify_Statistics.Enabled = this.cbx_B_NG_Classify.Checked;
                        this.cbx_B_NG_Classify.CheckedChanged += new System.EventHandler(this.cbx_B_NG_Classify_CheckedChanged);
                    }
                    break;
            }

            if (tag == "雙面")
            {
                this.textBox_Path_DefectTest.Text = this.Recipe_AB.Directory_AB_Recipe;

                // 清除 listView_Edit
                this.listView_Edit.Items.Clear();

                // 更新 listView_Result
                this.Recipe_AB.Update_listView_Result_AB(this.listView_Result, this.radioButton_Result, this.radioButton_Recheck);

                // 更新 listView_NGClassify_Statistics
                this.Recipe_AB.Update_listView_NGClassify_Statistics(this.listView_NGClassify_Statistics); // (20200429) Jeff Revised!

                // 更新 hSmartWindowControl_map
                this.listView_Result_SelectedIndexChanged(null, null);
            }
            else // A面 or B面
            {
                this.ui_parameters(false);

                // 更新 hSmartWindowControl_map
                this.locate_method_.Disp_DefectTest(this.hSmartWindowControl_map, this.radioButton_Result, this.radioButton_Recheck, this.radioButton_fill, this.radioButton_margin);

                // 更新 listView_Edit
                this.locate_method_.Update_listView_Edit(this.listView_Edit, this.radioButton_Result, this.radioButton_Recheck);

                // 更新 listView_Result
                this.locate_method_.Update_listView_Result(this.listView_Result, this.radioButton_Result, this.radioButton_Recheck);
            }
        }

        /// <summary>
        /// 批量測試 (人工覆判 & 瑕疵整合輸出)
        /// </summary>
        private cls_BatchTest BatchTest { get; set; } = new cls_BatchTest(); // (20200429) Jeff Revised!

        /// <summary>
        /// 【批量載入】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_BatchLoad_DefectTest_Click(object sender, EventArgs e) // (20200429) Jeff Revised!
        {
            using (Form_editBatchTest form = new Form_editBatchTest("【批量載入】雙面合併工單", this.BatchTest))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    this.ComboBox_BatchSBID_DefectTest.Text = "";
                    this.ComboBox_BatchRecipes_DefectTest.Text = "";
                    this.ComboBox_BatchSBID_DefectTest.Items.Clear();
                    this.ComboBox_BatchRecipes_DefectTest.Items.Clear();

                    if (this.BatchTest.B_All_SBID) // 只輸入生產料號
                    {
                        List<string> list_SBID = new List<string>();
                        if (this.BatchTest.Get_AllSBID_from_partNumber(out list_SBID) == false)
                        {
                            this.ComboBox_BatchRecipes_DefectTest.Items.Clear();
                            return;
                        }

                        foreach (string s in list_SBID)
                            this.ComboBox_BatchSBID_DefectTest.Items.Add(s);
                        if (list_SBID.Count > 0)
                            this.ComboBox_BatchSBID_DefectTest.SelectedIndex = 0;
                    }
                    else
                    {
                        this.ComboBox_BatchSBID_DefectTest.Items.Add(this.BatchTest.SBID);
                        this.ComboBox_BatchSBID_DefectTest.SelectedIndex = 0;
                    }
                }
            }
        }

        /// <summary>
        /// 【序號列表】選擇
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_BatchSBID_DefectTest_SelectedIndexChanged(object sender, EventArgs e) // (20200429) Jeff Revised!
        {
            if (this.ComboBox_BatchSBID_DefectTest.SelectedIndex < 0)
                return;

            // 更新序號
            this.BatchTest.SBID = this.ComboBox_BatchSBID_DefectTest.Text;

            this.ComboBox_BatchRecipes_DefectTest.Items.Clear();
            if (this.BatchTest.Update_Directories())
            {
                //for (int i = 1; i <= this.BatchTest.Directories.Count; i++)
                //    this.ComboBox_BatchRecipes_DefectTest.Items.Add(i.ToString());
                for (int i = 0; i < this.BatchTest.Directories.Count; i++)
                    this.ComboBox_BatchRecipes_DefectTest.Items.Add(this.BatchTest.productNames[i]);
            }
        }

        /// <summary>
        /// 【工單列表】選擇
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_BatchRecipes_DefectTest_SelectedIndexChanged(object sender, EventArgs e) // (20200429) Jeff Revised!
        {
            if (this.ComboBox_BatchRecipes_DefectTest.SelectedIndex < 0)
                return;

            this.radioButton_Recipe_AB.Checked = true; // 雙面
            this.textBox_Path_DefectTest.Text = this.BatchTest.Directories[this.ComboBox_BatchRecipes_DefectTest.SelectedIndex]; // 更新路徑
            this.button_Load_DefectTest_Click(null, null); //【載入瑕疵檔】
        }

        /// <summary>
        /// 【載入瑕疵檔】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Load_DefectTest_Click(object sender, EventArgs e) // (20200429) Jeff Revised!
        {
            if (sender != null) // (20200429) Jeff Revised!
            {
                FolderBrowserDialog Dilg = new FolderBrowserDialog();
                Dilg.Description = "【載入瑕疵檔】"; // (20200429) Jeff Revised!
                Dilg.SelectedPath = this.textBox_Path_DefectTest.Text; // 初始路徑
                if (Dilg.ShowDialog() != DialogResult.OK)
                    return;

                this.textBox_Path_DefectTest.Text = Dilg.SelectedPath;
            }
            if (string.IsNullOrEmpty(this.textBox_Path_DefectTest.Text))
            {
                SystemSounds.Exclamation.Play();
                MessageBox.Show("路徑無效!", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.textBox_Path_DefectTest.Text = "";
                return;
            }

            if (this.radioButton_Recipe_AB.Checked) // 雙面
            {
                if (this.bw_Load_Recipe_AB.IsBusy != true)
                {
                    this.groupBox_DefectTest.Enabled = false;

                    this.bw_Load_Recipe_AB.RunWorkerAsync();

                    ctrl_timer1 = this.button_Load_DefectTest;
                    BackColor_ctrl_timer1_1 = Color.Blue;
                    BackColor_ctrl_timer1_2 = Color.Green;
                    timer1.Enabled = true;
                }
                else
                    MessageBox.Show("背景執行緖忙碌中，請稍後再試", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else // A面 or B面
            {
                if (this.bw_Load_DefectTest.IsBusy != true) // (20190804) Jeff Revised!
                {
                    this.groupBox_DefectTest.Enabled = false;

                    richTextBox_log.AppendText("等待【載入瑕疵檔】 ..." + "\n");
                    richTextBox_log.ScrollToCaret();

                    this.bw_Load_DefectTest.RunWorkerAsync();

                    ctrl_timer1 = this.button_Load_DefectTest;
                    BackColor_ctrl_timer1_1 = Color.Blue;
                    BackColor_ctrl_timer1_2 = Color.Green;
                    timer1.Enabled = true;
                }
                else
                    MessageBox.Show("背景執行緖忙碌中，請稍後再試", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        clsProgressbar m_ProgressBar { get; set; } = null; // (20200429) Jeff Revised!
        private void bw_DoWork_Load_DefectTest(object sender, DoWorkEventArgs e) // (20190804) Jeff Revised!
        {
            BackgroundWorker Worker = (BackgroundWorker)sender;
            //clsProgressbar m_ProgressBar = new clsProgressbar();
            if (this.m_ProgressBar == null) // (20200429) Jeff Revised!
            {
                m_ProgressBar = new clsProgressbar();

                m_ProgressBar.FormClosedEvent2 += new clsProgressbar.FormClosedHandler2(SetFormClosed2);

                m_ProgressBar.SetShowText("請等待【載入瑕疵檔】......");
                m_ProgressBar.SetShowCaption("執行中......");
                m_ProgressBar.ShowWaitProgress();
            }
            
            if (LocateMethod.Load_DefectTest_Recipe(ref this.locate_method_, this.textBox_Path_DefectTest.Text + "\\")) // (20200429) Jeff Revised!
                e.Result = "Success";
            else
                e.Result = "Exception";

            //m_ProgressBar.CloseProgress();

            if (this.bw_Load_DefectTest.CancellationPending == true) // 判斷是否停止BackgroundWorker執行
            {
                e.Cancel = true;
                return;
            }
        }

        private void bw_RunWorkerCompleted_Load_DefectTest(object sender, RunWorkerCompletedEventArgs e) // (20190804) Jeff Revised!
        {
            if (e.Cancelled == true)
            {
                //MessageBox.Show("取消!");
            }
            else if (e.Error != null)
            {
                MessageBox.Show("Error: " + e.Error.Message);
            }
            else if (e.Result != null && e.Result.ToString() == "Exception")
            {
                richTextBox_log.AppendText("【載入瑕疵檔】失敗!" + "\n");
                richTextBox_log.ScrollToCaret();
                
                SystemSounds.Exclamation.Play();
                MessageBox.Show("【載入瑕疵檔】失敗!", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (e.Result != null && e.Result.ToString() == "Success")
            {
                this.m_ProgressBar.SetShowText("【載入】完成，執行GUI更新......");

                this.ui_parameters(false);

                // 更新 hSmartWindowControl_map
                this.locate_method_.Disp_DefectTest(this.hSmartWindowControl_map, this.radioButton_Result, this.radioButton_Recheck, this.radioButton_fill, this.radioButton_margin);

                // 更新 listView_Edit
                this.locate_method_.Update_listView_Edit(this.listView_Edit, this.radioButton_Result, this.radioButton_Recheck);

                // 更新 listView_Result
                this.locate_method_.Update_listView_Result(this.listView_Result, this.radioButton_Result, this.radioButton_Recheck);

                richTextBox_log.AppendText("【載入瑕疵檔】成功!" + "\n");
                richTextBox_log.ScrollToCaret();

                //MessageBox.Show("【載入瑕疵檔】成功!");

                this.radioButton_Recipe_B.Enabled = true; // (20200429) Jeff Revised!
                this.groupBox_NGClassify_Statistics.Enabled = true; // (20200429) Jeff Revised!
            }
            else
                return;

            #region 更新雙面工單 // (20200429) Jeff Revised!

            // 更新 Recipe_AB
            this.Set_LocateMethod_AB(); // (20200429) Jeff Revised!

            #endregion

            timer1.Enabled = false;
            ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
            this.groupBox_DefectTest.Enabled = true;

            this.m_ProgressBar.CloseProgress(); // (20200429) Jeff Revised!
            this.m_ProgressBar = null; // (20200429) Jeff Revised!
        }

        private void bw_DoWork_Load_Recipe_AB(object sender, DoWorkEventArgs e) // (20200429) Jeff Revised!
        {
            BackgroundWorker Worker = (BackgroundWorker)sender;
            if (this.m_ProgressBar == null)
            {
                m_ProgressBar = new clsProgressbar();

                m_ProgressBar.FormClosedEvent2 += new clsProgressbar.FormClosedHandler2(SetFormClosed2);

                m_ProgressBar.SetShowText("請等待雙面【載入】......");
                m_ProgressBar.SetShowCaption("執行中......");
                m_ProgressBar.ShowWaitProgress();
            }

            if (DefectTest_AB_Recipe.Load_DefectTest_AB_Recipe(this.textBox_Path_DefectTest.Text + "\\", out this.Recipe_AB)) // 雙面
            {
                m_ProgressBar.SetShowText("請等待A面【載入】......");
                if (LocateMethod.Load_DefectTest_Recipe(ref this.locate_method_A, this.Recipe_AB.Path_DefectTest_A + "\\")) // A面
                {
                    m_ProgressBar.SetShowText("請等待B面【載入】......");
                    if (LocateMethod.Load_DefectTest_Recipe(ref this.locate_method_B, this.Recipe_AB.Path_DefectTest_B + "\\")) // B面
                        e.Result = "Success";
                    else
                        e.Result = "Exception: B面";
                }
                else
                    e.Result = "Exception: A面";
            }
            else
                e.Result = "Exception: 雙面";

            if (this.bw_Load_Recipe_AB.CancellationPending == true) // 判斷是否停止BackgroundWorker執行
            {
                e.Cancel = true;
                return;
            }
        }

        private void bw_RunWorkerCompleted_Load_Recipe_AB(object sender, RunWorkerCompletedEventArgs e) // (20200429) Jeff Revised!
        {
            if (e.Cancelled == true)
            {
                //MessageBox.Show("取消!");
            }
            else if (e.Error != null)
            {
                MessageBox.Show("Error: " + e.Error.Message);
            }
            else if (e.Result != null && e.Result.ToString() != "Success")
            {
                SystemSounds.Exclamation.Play();
                if (e.Result.ToString() == "Exception: 雙面")
                    MessageBox.Show("雙面【載入】失敗!", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else if (e.Result.ToString() == "Exception: A面")
                    MessageBox.Show("A面【載入】失敗!", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else if (e.Result.ToString() == "Exception: B面")
                {
                    MessageBox.Show("B面【載入】失敗!", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.radioButton_Recipe_B.Enabled = true;
                    this.locate_method_ = this.locate_method_A;
                    this.ui_parameters(false);
                }
            }
            else if (e.Result != null && e.Result.ToString() == "Success")
            {
                //MessageBox.Show("【載入瑕疵檔】成功!");
                this.m_ProgressBar.SetShowText("【載入】完成，執行GUI更新......");

                richTextBox_log.AppendText("雙面【載入】成功!" + "\n");
                richTextBox_log.ScrollToCaret();
                this.radioButton_Recipe_B.Enabled = true;
                this.groupBox_NGClassify_Statistics.Enabled = true;
                this.locate_method_ = this.locate_method_A;
                this.ui_parameters(false);

                // 更新 Recipe_AB
                this.Set_LocateMethod_AB(); // (20200429) Jeff Revised!

                this.radioButton_Recipe_CheckedChanged(this.radioButton_Recipe_AB, null);
                HOperatorSet.SetPart(hSmartWindowControl_map.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part
            }
            else
                return;

            timer1.Enabled = false;
            ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
            this.groupBox_DefectTest.Enabled = true;
            this.m_ProgressBar.CloseProgress();
            this.m_ProgressBar = null;
        }

        /// <summary>
        /// 【儲存/另存】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Save_DefectTest_Click(object sender, EventArgs e) // (20200429) Jeff Revised!
        {
            SystemSounds.Exclamation.Play();
            if (MessageBox.Show("確定要【儲存/另存】?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) // (20200429) Jeff Revised!
                return;

            Button bt = sender as Button; // (20200429) Jeff Revised!
            if (this.textBox_Path_DefectTest.Text == "" || bt == this.button_SaveAs_DefectTest) // (20200429) Jeff Revised!
            {
                FolderBrowserDialog Dilg = new FolderBrowserDialog();
                Dilg.Description = "【儲存/另存】"; // (20200429) Jeff Revised!
                Dilg.SelectedPath = this.textBox_Path_DefectTest.Text; // 初始路徑
                if (Dilg.ShowDialog() != DialogResult.OK)
                    return;
                
                this.textBox_Path_DefectTest.Text = Dilg.SelectedPath;
            }
            
            if (string.IsNullOrEmpty(this.textBox_Path_DefectTest.Text))
            {
                SystemSounds.Exclamation.Play();
                MessageBox.Show("路徑無效!", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.textBox_Path_DefectTest.Text = "";
                return;
            }

            if (this.radioButton_Recipe_AB.Checked) // 雙面
            {
                // 儲存【雙面統計結果合併】Recipe路徑 // (20200429) Jeff Revised!
                this.Recipe_AB.Directory_AB_Recipe = this.textBox_Path_DefectTest.Text;

                this.groupBox_DefectTest.Enabled = false;

                richTextBox_log.AppendText("等待雙面【儲存/另存】 ..." + "\n");
                richTextBox_log.ScrollToCaret();

                ctrl_timer1 = bt;
                BackColor_ctrl_timer1_1 = Color.Blue;
                BackColor_ctrl_timer1_2 = Color.Green;
                timer1.Enabled = true;

                #region 儲存A面

                this.locate_method_ = this.locate_method_A;
                bool b_saveA = true;
                if (this.locate_method_.Path_DefectTest != "")
                {
                    DialogResult dialogResult = MessageBox.Show("是否儲存A面資訊?", "A面", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult != DialogResult.Yes)
                        b_saveA = false;
                }
                else
                {
                    FolderBrowserDialog Dilg = new FolderBrowserDialog();
                    Dilg.Description = "【儲存/另存】A面";
                    if (Dilg.ShowDialog() != DialogResult.OK)
                        return;
                    this.locate_method_.Path_DefectTest = Dilg.SelectedPath;
                }

                if (b_saveA)
                {
                    object[] parameters = new object[] { "A面", this.locate_method_.Path_DefectTest + "\\", false };
                    this.bw_SaveAs_DefectTest.RunWorkerAsync(parameters);

                    // 等待儲存A面
                    while (true)
                    {
                        // UI才會更新 (Timer持續作用) !
                        Application.DoEvents(); // 處理當前在消息隊列中的所有 Windows 消息，可以防止界面停止響應
                        Thread.Sleep(1); // Sleep一下，避免一直跑DoEvents()導致計算效率低
                        if (this.bw_SaveAs_DefectTest.IsBusy != true)
                            break;
                    }
                }

                #endregion
                
                #region 儲存B面

                this.locate_method_ = this.locate_method_B;
                bool b_saveB = true;
                if (this.locate_method_.Path_DefectTest != "")
                {
                    DialogResult dialogResult = MessageBox.Show("是否儲存B面資訊?", "B面", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult != DialogResult.Yes)
                        b_saveB = false;
                }
                else
                {
                    FolderBrowserDialog Dilg = new FolderBrowserDialog();
                    Dilg.Description = "【儲存/另存】B面";
                    if (Dilg.ShowDialog() != DialogResult.OK)
                        return;
                    this.locate_method_.Path_DefectTest = Dilg.SelectedPath;
                }

                if (b_saveB)
                {
                    object[] parameters = new object[] { "B面", this.locate_method_.Path_DefectTest + "\\", false };
                    this.bw_SaveAs_DefectTest.RunWorkerAsync(parameters);

                    // 等待儲存B面
                    while (true)
                    {
                        // UI才會更新 (Timer持續作用) !
                        Application.DoEvents(); // 處理當前在消息隊列中的所有 Windows 消息，可以防止界面停止響應
                        Thread.Sleep(1); // Sleep一下，避免一直跑DoEvents()導致計算效率低
                        if (this.bw_SaveAs_DefectTest.IsBusy != true)
                            break;
                    }
                }

                #endregion

                #region 儲存雙面

                this.Recipe_AB.Path_DefectTest_A = this.locate_method_A.Path_DefectTest;
                this.Recipe_AB.Path_DefectTest_B = this.locate_method_B.Path_DefectTest;
                object[] param = new object[] { "雙面", this.Recipe_AB.Directory_AB_Recipe + "\\", true };
                this.bw_SaveAs_DefectTest.RunWorkerAsync(param);

                #endregion

            }
            else // A面 or B面
            {
                // 儲存 Path_DefectTest // (20200429) Jeff Revised!
                this.locate_method_.Path_DefectTest = this.textBox_Path_DefectTest.Text;
                
                if (this.bw_SaveAs_DefectTest.IsBusy != true) // (20190804) Jeff Revised!
                {
                    this.groupBox_DefectTest.Enabled = false;

                    richTextBox_log.AppendText("等待【儲存/另存瑕疵檔】 ..." + "\n");
                    richTextBox_log.ScrollToCaret();

                    string nowRecipe_DefectTest = ""; // (20200429) Jeff Revised!
                    if (this.radioButton_Recipe_A.Checked)
                        nowRecipe_DefectTest = "A面";
                    else
                        nowRecipe_DefectTest = "B面";

                    object[] parameters = new object[] { nowRecipe_DefectTest, this.locate_method_.Path_DefectTest + "\\", true }; // (20200429) Jeff Revised!
                    this.bw_SaveAs_DefectTest.RunWorkerAsync(parameters);

                    ctrl_timer1 = bt;
                    BackColor_ctrl_timer1_1 = Color.Blue;
                    BackColor_ctrl_timer1_2 = Color.Green;
                    timer1.Enabled = true;
                }
                else
                    MessageBox.Show("背景執行緖忙碌中，請稍後再試", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }

        }
        
        private void bw_DoWork_SaveAs_DefectTest(object sender, DoWorkEventArgs e) // (20200429) Jeff Revised!
        {
            BackgroundWorker Worker = (BackgroundWorker)sender;
            // 取得 input arguments // (20200429) Jeff Revised!
            object[] parameters = e.Argument as object[];
            string nowRecipe_DefectTest = parameters[0].ToString();
            string directory_ = parameters[1].ToString();
            bool b_closeTimer = (bool)parameters[2];

            // 設定 e.Result
            e.Result = parameters;

            clsProgressbar m_ProgressBar = new clsProgressbar();

            m_ProgressBar.FormClosedEvent2 += new clsProgressbar.FormClosedHandler2(SetFormClosed2);

            m_ProgressBar.SetShowText("請等待" + nowRecipe_DefectTest + "【儲存/另存】......");
            m_ProgressBar.SetShowCaption("執行中......");
            m_ProgressBar.ShowWaitProgress();

            /*
            try
            {
                if (Directory.Exists(directory_)) // 如果檔案已存在，則先刪除再儲存!
                    Directory.Delete(directory_, true); // true表示指定目錄與其子目錄一起刪除
            }
            catch (Exception ex)
            { }
            */
            List<object> param = parameters.ToList(); // (20200429) Jeff Revised!
            if (nowRecipe_DefectTest == "雙面") // 雙面
            {
                if (this.Recipe_AB.Save_DefectTest_AB_Recipe())
                    param.Add("Success");
                else
                    param.Add("Exception");
            }
            else // A面 or B面
            {
                if (this.locate_method_.Save_DefectTest_Recipe(directory_))
                    param.Add("Success");
                else
                    param.Add("Exception");
            }
            e.Result = param.ToArray();

            m_ProgressBar.CloseProgress();

            if (this.bw_SaveAs_DefectTest.CancellationPending == true) // 判斷是否停止BackgroundWorker執行
            {
                e.Cancel = true;
                return;
            }
        }

        private void bw_RunWorkerCompleted_SaveAs_DefectTest(object sender, RunWorkerCompletedEventArgs e) // (20200429) Jeff Revised!
        {
            // 取得 input arguments // (20200429) Jeff Revised!
            object[] parameters = e.Result as object[];
            string nowRecipe_DefectTest = parameters[0].ToString();
            string directory_ = parameters[1].ToString();
            bool b_closeTimer = (bool)parameters[2];

            if (e.Cancelled == true)
            {
                //MessageBox.Show("取消!");
            }
            else if (e.Error != null)
            {
                MessageBox.Show("Error: " + e.Error.Message);
            }
            else if (e.Result != null && parameters[3].ToString() == "Exception")
            {
                richTextBox_log.AppendText(nowRecipe_DefectTest + "【儲存/另存】失敗!" + "\n");
                richTextBox_log.ScrollToCaret();

                SystemSounds.Exclamation.Play();
                MessageBox.Show(nowRecipe_DefectTest + "【儲存/另存】失敗!", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // 重建已刪除之檔案路徑
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(directory_));
            }
            else if (e.Result != null && parameters[3].ToString() == "Success")
            {
                richTextBox_log.AppendText(nowRecipe_DefectTest + "【儲存/另存】成功!" + "\n");
                richTextBox_log.ScrollToCaret();

                //MessageBox.Show("【儲存/另存瑕疵檔】成功!");
                if (this.radioButton_Recipe_B.Enabled == false)
                {
                    this.radioButton_Recipe_B.Enabled = true; // (20200429) Jeff Revised!
                    this.groupBox_NGClassify_Statistics.Enabled = true; // (20200429) Jeff Revised!
                }
            }
            else
                return;

            if (b_closeTimer)
            {
                timer1.Enabled = false;
                ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
                this.groupBox_DefectTest.Enabled = true;
            }
        }

        /// <summary>
        /// 瑕疵模式選擇 (瑕疵分類, 瑕疵優先權, 人工覆判)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbx_DefectTest_CheckedChanged(object sender, EventArgs e) // (20200429) Jeff Revised!
        {
            CheckBox cbx = sender as CheckBox;
            string tag = cbx.Tag.ToString();

            switch (tag)
            {
                case "瑕疵分類":
                    {
                        if (!this.b_loadParameters) // (20190727) Jeff Revised!
                            this.locate_method_.b_Defect_Classify = cbx.Checked;

                        this.groupBox_DefectsClassify.Enabled = cbx.Checked;
                    }
                    break;
                case "瑕疵優先權":
                    {
                        if (!this.b_loadParameters) // (20190727) Jeff Revised!
                        {
                            this.locate_method_.b_Defect_priority = cbx.Checked;
                            this.locate_method_.Update_DefectsResult_Priority(); // (20190702) Jeff Revised!
                        }
                    }
                    break;
                case "人工覆判":
                    {
                        if (!this.b_loadParameters) // (20190727) Jeff Revised!
                            this.locate_method_.b_Defect_Recheck = cbx.Checked;

                        clsStaticTool.EnableAllControls(this.groupBox_Recheck, cbx, cbx.Checked);
                        this.radioButton_Recheck.Enabled = cbx.Checked;
                        if (cbx.Checked == false)
                        {
                            this.radioButton_Recheck.Checked = false;
                            this.radioButton_Result.Checked = true;
                        }
                    }
                    break;
            }

            if (this.b_loadParameters) // (20190727) Jeff Revised!
                return;

            // 初始化人工覆判
            this.locate_method_.Initialize_Recheck(); // (20190704) Jeff Revised!

            // 更新 hSmartWindowControl_map
            HOperatorSet.ClearWindow(hSmartWindowControl_map.HalconWindow);
            if (this.locate_method_.TileImage != null)
                HOperatorSet.DispObj(this.locate_method_.TileImage, hSmartWindowControl_map.HalconWindow);
            HOperatorSet.SetPart(hSmartWindowControl_map.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part

            // 更新 listView_Edit
            this.locate_method_.Update_listView_Edit(this.listView_Edit, this.radioButton_Result, this.radioButton_Recheck);

            // 更新 listView_Result
            this.locate_method_.Update_listView_Result(this.listView_Result, this.radioButton_Result, this.radioButton_Recheck);

            // 更新 Recipe_AB
            this.Set_LocateMethod_AB(); // (20200429) Jeff Revised!
        }

        /// <summary>
        /// 【顯示資訊】 切換
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_defTest_CheckedChanged(object sender, EventArgs e) // (20200429) Jeff Revised!
        {
            RadioButton rbtn = sender as RadioButton;
            if (rbtn.Checked == false) return;

            if (this.radioButton_Recipe_AB.Checked) // 雙面
            {
                // 更新 listView_Result
                this.Recipe_AB.Update_listView_Result_AB(this.listView_Result, this.radioButton_Result, this.radioButton_Recheck);

                // 更新 hSmartWindowControl_map
                this.listView_Result_SelectedIndexChanged(null, null);
            }
            else // A面 or B面
            {
                // 更新 hSmartWindowControl_map
                this.radioButton_DefDispType_CheckedChanged(sender, e); // (20200429) Jeff Revised!

                // 更新 listView_Edit
                this.locate_method_.Update_listView_Edit(this.listView_Edit, this.radioButton_Result, this.radioButton_Recheck);

                // 更新 listView_Result
                this.locate_method_.Update_listView_Result(this.listView_Result, this.radioButton_Result, this.radioButton_Recheck);
            }
        }

        /// <summary>
        /// 【瑕疵顯示形式】 切換
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_DefDispType_CheckedChanged(object sender, EventArgs e) // (20200429) Jeff Revised!
        {
            RadioButton rbtn = sender as RadioButton;
            if (rbtn.Checked == false) return;

            // 更新 hSmartWindowControl_map
            if (this.listView_Edit.SelectedIndices.Count > 0) // (20200429) Jeff Revised!
            {
                List<string> ListName = new List<string>();
                ListName.Add(this.listView_Edit.SelectedItems[0].Text);
                this.locate_method_.Disp_DefectTest(this.hSmartWindowControl_map, this.radioButton_Result, this.radioButton_Recheck, this.radioButton_fill, this.radioButton_margin, ListName, false);
            }
            else
                this.locate_method_.Disp_DefectTest(this.hSmartWindowControl_map, this.radioButton_Result, this.radioButton_Recheck, this.radioButton_fill, this.radioButton_margin, null, false);
        }

        /// <summary>
        /// 更新顯示瑕疵 (hSmartWindowControl_map)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView_Edit_SelectedIndexChanged(object sender, EventArgs e) // (20200429) Jeff Revised!
        {
            if (this.listView_Edit.SelectedIndices.Count <= 0)
                return;

            List<string> ListName = new List<string>();
            ListName.Add(this.listView_Edit.SelectedItems[0].Text);
            this.locate_method_.Disp_DefectTest(this.hSmartWindowControl_map, this.radioButton_Result, this.radioButton_Recheck, this.radioButton_fill, this.radioButton_margin, ListName, false);

            // 同時選取 listView_Result 中相同瑕疵名稱 (20200429) Jeff Revised!
            this.listView_Result.SelectedIndexChanged -= new System.EventHandler(this.listView_Result_SelectedIndexChanged);
            if (this.listView_Result.SelectedIndices.Count > 0) // 消除listView_Result目前選取狀態
                this.listView_Result.SelectedItems[0].Selected = false;
            this.listView_Result.Items[this.listView_Edit.SelectedIndices[0]].Focused = true;
            this.listView_Result.Items[this.listView_Edit.SelectedIndices[0]].Selected = true;
            this.listView_Result.Items[this.listView_Edit.SelectedIndices[0]].EnsureVisible();
            this.listView_Result.SelectedIndexChanged += new System.EventHandler(this.listView_Result_SelectedIndexChanged);
        }

        /// <summary>
        /// 更新顯示瑕疵 (hSmartWindowControl_map)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView_Result_SelectedIndexChanged(object sender, EventArgs e) // (20200429) Jeff Revised!
        {
            if (this.radioButton_Recipe_AB.Checked) // 雙面
            {
                if (this.listView_Result.SelectedIndices.Count <= 0 || this.listView_Result.SelectedIndices[0] == 2) // 顯示雙面
                {
                    LocateMethod.Disp_DefectTest(hSmartWindowControl_map, this.locate_method_A, null, this.radioButton_Result, this.radioButton_Recheck, this.radioButton_fill, this.radioButton_margin);
                    LocateMethod.Disp_DefectTest(hSmartWindowControl_map, this.Recipe_AB.Locate_method_B_2_A, null, this.radioButton_Result, this.radioButton_Recheck, this.radioButton_fill, this.radioButton_margin, false);
                    return;
                }
                else if (this.listView_Result.SelectedIndices[0] == 0) // 顯示A面
                {
                    LocateMethod.Disp_DefectTest(hSmartWindowControl_map, this.locate_method_A, null, this.radioButton_Result, this.radioButton_Recheck, this.radioButton_fill, this.radioButton_margin);
                }
                else if (this.listView_Result.SelectedIndices[0] == 1) // 顯示B面
                {
                    LocateMethod.Disp_DefectTest(hSmartWindowControl_map, this.Recipe_AB.Locate_method_B_2_A, null, this.radioButton_Result, this.radioButton_Recheck, this.radioButton_fill, this.radioButton_margin);
                }

            }
            else // A面 or B面
            {
                if (this.listView_Result.SelectedIndices.Count <= 0)
                    return;

                // 同時選取 listView_Edit 中相同瑕疵名稱
                if (this.listView_Edit.SelectedIndices.Count > 0) // 消除listView_Edit目前選取狀態
                    this.listView_Edit.SelectedItems[0].Selected = false;
                this.listView_Edit.Items[this.listView_Result.SelectedIndices[0]].Focused = true;
                this.listView_Edit.Items[this.listView_Result.SelectedIndices[0]].Selected = true;
                this.listView_Edit.Items[this.listView_Result.SelectedIndices[0]].EnsureVisible();
            }
        }

        /// <summary>
        /// 【瑕疵分類統計】啟用狀態切換
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbx_B_NG_Classify_CheckedChanged(object sender, EventArgs e) // (20200429) Jeff Revised!
        {
            // 更新Enabled狀態
            this.listView_NGClassify_Statistics.Enabled = this.cbx_B_NG_Classify.Checked;

            // 設定 B_NG_Classify & 更新 NGClassify_Statistics
            this.Recipe_AB.Set_B_NG_Classify(this.cbx_B_NG_Classify.Checked);

            // 更新 listView_NGClassify_Statistics
            this.Recipe_AB.Update_listView_NGClassify_Statistics(this.listView_NGClassify_Statistics);
        }

        /// <summary>
        /// 更新 locate_method_B_2_A 之人工覆判
        /// </summary>
        private void Update_Locate_method_B_2_A_Recheck() // (20200429) Jeff Revised!
        {
            if (this.locate_method_B.Path_DefectTest == "")
                return;
            
            if (this.radioButton_Recipe_A.Checked) // A面
                this.locate_method_A = this.locate_method_;
            else if (this.radioButton_Recipe_B.Checked) // B面
                this.locate_method_B = this.locate_method_;

            if (!this.Recipe_AB.Update_Locate_method_B_2_A_Recheck(true, this.locate_method_A, this.locate_method_B))
            {
                //return;
            }
        }

        /// <summary>
        /// 更新 Locate_method_B_2_A 之 DefectsClassify 顏色
        /// </summary>
        private void Update_Color_Locate_method_B_2_A() // (20200429) Jeff Revised!
        {
            if (this.locate_method_B.Path_DefectTest == "")
                return;
            
            if (this.radioButton_Recipe_A.Checked) // A面
                this.locate_method_A = this.locate_method_;
            else if (this.radioButton_Recipe_B.Checked) // B面
                this.locate_method_B = this.locate_method_;

            if (!this.Recipe_AB.Update_Color_Locate_method_B_2_A(this.locate_method_A, this.locate_method_B))
            {
                //return;
            }
        }

        /// <summary>
        ///  更新 Recipe_AB
        /// </summary>
        private void Set_LocateMethod_AB() // (20200429) Jeff Revised!
        {
            //if (this.locate_method_B.Path_DefectTest == "")
            //    return;

            try
            {
                if (this.radioButton_Recipe_A.Checked) // A面
                    this.locate_method_A = this.locate_method_;
                else if (this.radioButton_Recipe_B.Checked) // B面
                    this.locate_method_B = this.locate_method_;

                if (this.locate_method_B.Path_DefectTest == "")
                    return;

                this.Recipe_AB.Set_LocateMethod(this.locate_method_A, this.locate_method_B);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 【新增】瑕疵
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Add_DefectsClassify_Click(object sender, EventArgs e) // (20200429) Jeff Revised!
        {
            using (Form_editDefect form = new Form_editDefect(this.locate_method_))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    // 更新 listView_Edit
                    this.locate_method_.Update_listView_Edit(this.listView_Edit, this.radioButton_Result, this.radioButton_Recheck);

                    // 更新 listView_Result
                    this.locate_method_.Update_listView_Result(this.listView_Result, this.radioButton_Result, this.radioButton_Recheck);

                    // 更新 Recipe_AB
                    this.Set_LocateMethod_AB(); // (20200429) Jeff Revised!
                }
            }
        }

        /// <summary>
        /// 【編輯】瑕疵
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Edit_DefectsClassify_Click(object sender, EventArgs e) // (20200429) Jeff Revised!
        {
            if (this.listView_Edit.SelectedIndices.Count <= 0)
            {
                ctrl_timer1 = this.listView_Edit;
                BackColor_ctrl_timer1_1 = Color.AliceBlue;
                BackColor_ctrl_timer1_2 = Color.Green;
                timer1.Enabled = true;
                SystemSounds.Exclamation.Play();
                MessageBox.Show("請選擇瑕疵名稱", "溫馨提醒", MessageBoxButtons.OK, MessageBoxIcon.Information);
                timer1.Enabled = false;
                ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
                return;
            }

            using (Form_editDefect form = new Form_editDefect(this.locate_method_, true, this.listView_Edit.SelectedItems[0].Text))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    // 更新 hSmartWindowControl_map
                    this.listView_Edit_SelectedIndexChanged(sender, e);

                    // 更新 listView_Edit
                    this.locate_method_.Update_listView_Edit(this.listView_Edit, this.radioButton_Result, this.radioButton_Recheck);

                    // 更新 listView_Result
                    this.locate_method_.Update_listView_Result(this.listView_Result, this.radioButton_Result, this.radioButton_Recheck);

                    // 更新 Locate_method_B_2_A 之 DefectsClassify 顏色
                    //this.Update_Color_Locate_method_B_2_A(); // (20200429) Jeff Revised!

                    // 更新 Recipe_AB
                    this.Set_LocateMethod_AB(); // (20200429) Jeff Revised!
                }
            }
        }

        /// <summary>
        /// 【刪除】瑕疵
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Remove_DefectsClassify_Click(object sender, EventArgs e) // (20200429) Jeff Revised!
        {
            if (this.listView_Edit.SelectedIndices.Count <= 0)
            {
                ctrl_timer1 = this.listView_Edit;
                BackColor_ctrl_timer1_1 = Color.AliceBlue;
                BackColor_ctrl_timer1_2 = Color.Green;
                timer1.Enabled = true;
                SystemSounds.Exclamation.Play();
                MessageBox.Show("請選擇瑕疵名稱", "溫馨提醒", MessageBoxButtons.OK, MessageBoxIcon.Information);
                timer1.Enabled = false;
                ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
                return;
            }

            DialogResult dr = MessageBox.Show("確定要刪除瑕疵名稱【" + this.listView_Edit.SelectedItems[0].Text + "】?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No)
                return;

            if (!(this.locate_method_.Remove_1_DefectsClassify(this.listView_Edit.SelectedItems[0].Text, int.Parse(this.listView_Edit.SelectedItems[0].SubItems[1].Text))))
            {
                SystemSounds.Exclamation.Play();
                MessageBox.Show("刪除失敗!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 更新 hSmartWindowControl_map
            HOperatorSet.ClearWindow(hSmartWindowControl_map.HalconWindow);
            HOperatorSet.DispObj(this.locate_method_.TileImage, hSmartWindowControl_map.HalconWindow);
            HOperatorSet.SetPart(hSmartWindowControl_map.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part

            // 更新 listView_Edit
            this.locate_method_.Update_listView_Edit(this.listView_Edit, this.radioButton_Result, this.radioButton_Recheck);

            // 更新 listView_Result
            this.locate_method_.Update_listView_Result(this.listView_Result, this.radioButton_Result, this.radioButton_Recheck);

            // 更新 Recipe_AB
            this.Set_LocateMethod_AB(); // (20200429) Jeff Revised!
        }

        /// <summary>
        /// 【清空】瑕疵
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_RemoveAll_DefectsClassify_Click(object sender, EventArgs e) // (20200429) Jeff Revised!
        {
            DialogResult dr = MessageBox.Show("確定要清空所有瑕疵名稱?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No)
                return;

            this.locate_method_.Release_DefectsClassify();

            // 更新 hSmartWindowControl_map
            HOperatorSet.ClearWindow(hSmartWindowControl_map.HalconWindow);
            HOperatorSet.DispObj(this.locate_method_.TileImage, hSmartWindowControl_map.HalconWindow);
            HOperatorSet.SetPart(hSmartWindowControl_map.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part

            // 更新 listView_Edit
            this.locate_method_.Update_listView_Edit(this.listView_Edit, this.radioButton_Result, this.radioButton_Recheck);

            // 更新 listView_Result
            this.locate_method_.Update_listView_Result(this.listView_Result, this.radioButton_Result, this.radioButton_Recheck);

            // 更新 Recipe_AB
            this.Set_LocateMethod_AB(); // (20200429) Jeff Revised!
        }
        
        /// <summary>
        /// 手動製造defects region
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_fake_defect_Click(object sender, EventArgs e) // (20190627) Jeff Revised!
        {
            if (this.locate_method_.b_Defect_Classify)
            {
                if (listView_Edit.SelectedIndices.Count <= 0)
                {
                    ctrl_timer1 = listView_Edit;
                    BackColor_ctrl_timer1_1 = Color.AliceBlue;
                    BackColor_ctrl_timer1_2 = Color.Green;
                    timer1.Enabled = true;
                    SystemSounds.Exclamation.Play();
                    MessageBox.Show("請選擇瑕疵名稱", "溫馨提醒", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    timer1.Enabled = false;
                    ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
                    return;
                }
            }

            try
            {
                if (openFileDialog_file.ShowDialog() == DialogResult.OK)
                {
                    //int capture_index_ = Convert.ToInt32(openFileDialog_file.SafeFileName.Substring(7, 3).ToString()) - 2;
                    // (20181217) Jeff Revised!
                    int index = openFileDialog_file.SafeFileName.IndexOf("_M");
                    int capture_index_ = Convert.ToInt32(openFileDialog_file.SafeFileName.Substring(index + 2, 3).ToString()) - 2;

                    if (capture_index_ >= 1 && capture_index_ <= this.locate_method_.array_x_count_ * this.locate_method_.array_y_count_)
                    {
                        HObject test_image_, fake_region_;
                        HTuple row_, col_, phi_, leng1_, leng2_;
                        HObject defect_region_;
                        HObject BackDefect; // (20181115) Jeff Revised!
                        List<Point> defect_id_ = new List<Point>();

                        string file_name_ = openFileDialog_file.FileName;
                        HOperatorSet.ReadImage(out test_image_, file_name_);
                        // 轉成RGB image (20180829) Jeff Revised!
                        //HTuple Channels;
                        //HOperatorSet.CountChannels(test_image_, out Channels);
                        //if (Channels == 1)
                        //{
                        //    HOperatorSet.Compose3(test_image_.Clone(), test_image_.Clone(), test_image_.Clone(), out test_image_);
                        //}
                        //HOperatorSet.ZoomImageFactor(test_image_, out test_image_, locate_method_.resize_, locate_method_.resize_, "bilinear"); // (20190306) Jeff Revised!
                        HOperatorSet.ClearWindow(hSmartWindowControl_map.HalconWindow);
                        HOperatorSet.DispObj(test_image_, hSmartWindowControl_map.HalconWindow);
                        HOperatorSet.SetColor(hSmartWindowControl_map.HalconWindow, "red");
                        HOperatorSet.DrawRectangle2(hSmartWindowControl_map.HalconWindow, out row_, out col_, out phi_, out leng1_, out leng2_);
                        HOperatorSet.GenRectangle2(out fake_region_, row_, col_, phi_, leng1_, leng2_);

                        this.locate_method_.label_defect(capture_index_, fake_region_, out defect_region_, out defect_id_, out BackDefect); // (20181115) Jeff Revised!
                        if (this.locate_method_.b_Defect_Classify)
                        {
                            string Name = listView_Edit.SelectedItems[0].Text;
                            Dictionary<string, HObject> defects_with_names = new Dictionary<string, HObject>();
                            defects_with_names.Add(Name, fake_region_);
                            this.locate_method_.label_defect_DefectsClassify(capture_index_, defects_with_names, true, true);
                        }

                        // 更新 hSmartWindowControl_map
                        HOperatorSet.ClearWindow(hSmartWindowControl_map.HalconWindow);
                        HOperatorSet.DispObj(this.locate_method_.TileImage, hSmartWindowControl_map.HalconWindow);
                        HOperatorSet.SetDraw(hSmartWindowControl_map.HalconWindow, "margin");
                        if (!this.locate_method_.b_Defect_Classify) // 單一瑕疵
                        {
                            HOperatorSet.DispObj(this.locate_method_.all_intersection_defect_, hSmartWindowControl_map.HalconWindow);
                        }
                        else // 多瑕疵
                        {
                            string Name = listView_Edit.SelectedItems[0].Text;
                            HOperatorSet.SetColor(hSmartWindowControl_map.HalconWindow, this.locate_method_.DefectsClassify[Name].Str_Color_Halcon);
                            if (this.locate_method_.b_Defect_priority)
                                HOperatorSet.DispObj(this.locate_method_.DefectsClassify[Name].all_intersection_defect_Priority, hSmartWindowControl_map.HalconWindow);
                            else
                                HOperatorSet.DispObj(this.locate_method_.DefectsClassify[Name].all_intersection_defect_Origin, hSmartWindowControl_map.HalconWindow);
                        }

                        richTextBox_log.AppendText("============" + "\n");
                        richTextBox_log.AppendText("Defect Number : " + this.locate_method_.all_defect_id_.Count + "\n");

                        foreach (Point i in this.locate_method_.all_defect_id_)
                        {
                            richTextBox_log.AppendText("X = " + i.X.ToString() + ", Y = " + i.Y.ToString() + "\n");
                        }
                        richTextBox_log.ScrollToCaret();

                        // 更新 listView_Result
                        this.locate_method_.Update_listView_Result(this.listView_Result, this.radioButton_Result, this.radioButton_Recheck);
                        
                        #region Release memories

                        test_image_.Dispose();
                        fake_region_.Dispose();
                        defect_region_.Dispose();
                        BackDefect.Dispose();

                        #endregion
                    }
                    else
                        MessageBox.Show("Please Load Right Capture Index Image", "Load Wrong Index of Image", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Make Fake Defect Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 【Clear All Defects】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_clear_defect_Click(object sender, EventArgs e) // (20200429) Jeff Revised!
        {
            try
            {
                // 新增使用者保護機制
                SystemSounds.Exclamation.Play();
                DialogResult dr = MessageBox.Show("確定要清空所有瑕疵Region ? \n Note: 連同【人工覆判】也會一併清空", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr != DialogResult.Yes)
                    return;

                if (this.locate_method_.clear_all_defects())
                {
                    // 更新 richTextBox_log
                    richTextBox_log.AppendText("============" + "\n");
                    richTextBox_log.AppendText("Defect Number : " + this.locate_method_.all_defect_id_.Count + "\n");

                    foreach (Point i in this.locate_method_.all_defect_id_)
                    {
                        richTextBox_log.AppendText("X = " + i.X.ToString() + ", Y = " + i.Y.ToString() + "\n");
                    }
                    richTextBox_log.ScrollToCaret();

                    // 更新 hSmartWindowControl_map
                    this.locate_method_.Disp_DefectTest(this.hSmartWindowControl_map, this.radioButton_Result, this.radioButton_Recheck, this.radioButton_fill, this.radioButton_margin);

                    // 更新 listView_Edit
                    this.locate_method_.Update_listView_Edit(this.listView_Edit, this.radioButton_Result, this.radioButton_Recheck);

                    // 更新 listView_Result
                    this.locate_method_.Update_listView_Result(this.listView_Result, this.radioButton_Result, this.radioButton_Recheck);

                    // 更新 Recipe_AB
                    this.Set_LocateMethod_AB(); // (20200429) Jeff Revised!

                    //MessageBox.Show("Clear all defects succeeds!");
                }
            }
            catch
            { }
        }

        /// <summary>
        /// 第一個檢測位置為1
        /// </summary>
        private int index_capture { get; set; } = 1;
        /// <summary>
        /// 【Make Fake Defect】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbx_fake_defect_CheckedChanged(object sender, EventArgs e) // (20200429) Jeff Revised!
        {
            CheckBox Obj = (CheckBox)sender;

            if (this.locate_method_.b_Defect_Classify)
            {
                if (listView_Edit.SelectedIndices.Count <= 0)
                {
                    #region CheckBox狀態復原
                    cbx_fake_defect.CheckedChanged -= cbx_fake_defect_CheckedChanged;
                    Obj.Checked = false;
                    cbx_fake_defect.CheckedChanged += cbx_fake_defect_CheckedChanged;
                    #endregion

                    ctrl_timer1 = listView_Edit;
                    BackColor_ctrl_timer1_1 = Color.AliceBlue;
                    BackColor_ctrl_timer1_2 = Color.Green;
                    timer1.Enabled = true;
                    SystemSounds.Exclamation.Play();
                    MessageBox.Show("請選擇瑕疵名稱", "溫馨提醒", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    timer1.Enabled = false;
                    ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
                    return;
                }
            }

            if (Obj.Checked) // Make Fake Defect
            {
                try
                {
                    if (openFileDialog_file.ShowDialog() == DialogResult.OK)
                    {
                        int index = openFileDialog_file.SafeFileName.IndexOf("_M");
                        //int capture_index_ = Convert.ToInt32(openFileDialog_file.SafeFileName.Substring(index + 2, 3).ToString()) - 2;
                        int capture_index_ = Convert.ToInt32(openFileDialog_file.SafeFileName.Substring(index + 2, 3).ToString()) - index_Move + 1; // (20191216) MIL Jeff Revised!

                        if (capture_index_ >= 1 && capture_index_ <= this.locate_method_.array_x_count_ * this.locate_method_.array_y_count_)
                        {
                            index_capture = capture_index_;
                            HObject test_image_;
                            string file_name_ = openFileDialog_file.FileName;
                            HOperatorSet.ReadImage(out test_image_, file_name_);
                            // 轉成RGB image (20180829) Jeff Revised!
                            //HTuple Channels;
                            //HOperatorSet.CountChannels(test_image_, out Channels);
                            //if (Channels == 1)
                            //{
                            //    HOperatorSet.Compose3(test_image_.Clone(), test_image_.Clone(), test_image_.Clone(), out test_image_);
                            //}
                            //HOperatorSet.ZoomImageFactor(test_image_, out test_image_, this.locate_method_.resize_, this.locate_method_.resize_, "bilinear"); // (20190306) Jeff Revised!
                            HOperatorSet.ClearWindow(hSmartWindowControl_map.HalconWindow);
                            HOperatorSet.DispObj(test_image_, hSmartWindowControl_map.HalconWindow);
                            HOperatorSet.SetPart(hSmartWindowControl_map.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part

                            HTuple ImgWidth, ImgHeight;
                            HOperatorSet.GetImageSize(test_image_, out ImgWidth, out ImgHeight);
                            HOperatorSet.CreateDrawingObjectRectangle2(ImgHeight / 2, ImgWidth / 2, 0, 100, 100, out drawing_ID);
                            HOperatorSet.SetDrawingObjectParams(drawing_ID, "color", "red");
                            HOperatorSet.AttachDrawingObjectToWindow(hSmartWindowControl_map.HalconWindow, drawing_ID);
                            test_image_.Dispose();

                            //ListCtrl_Bypass = EnableAllControls(this.tabControl1, Obj, false);
                            ListCtrl_Bypass = clsStaticTool.EnableAllControls_Bypass(this.tabControl1, Obj, false);
                            Obj.ForeColor = Color.GreenYellow;
                            Obj.Text = "Done";
                            ctrl_timer1 = Obj;
                            BackColor_ctrl_timer1_1 = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(166)))), ((int)(((byte)(137)))));
                            BackColor_ctrl_timer1_2 = Color.Lime;
                            timer1.Enabled = true;
                        }
                        else
                        {
                            #region CheckBox狀態復原
                            cbx_fake_defect.CheckedChanged -= cbx_fake_defect_CheckedChanged;
                            Obj.Checked = false;
                            cbx_fake_defect.CheckedChanged += cbx_fake_defect_CheckedChanged;
                            #endregion

                            MessageBox.Show("Please Load Right Capture Index Image", "Load Wrong Index of Image", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                    }
                    else
                    {
                        #region CheckBox狀態復原
                        cbx_fake_defect.CheckedChanged -= cbx_fake_defect_CheckedChanged;
                        Obj.Checked = false;
                        cbx_fake_defect.CheckedChanged += cbx_fake_defect_CheckedChanged;
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    #region CheckBox狀態復原
                    cbx_fake_defect.CheckedChanged -= cbx_fake_defect_CheckedChanged;
                    Obj.Checked = false;
                    cbx_fake_defect.CheckedChanged += cbx_fake_defect_CheckedChanged;
                    #endregion

                    MessageBox.Show(ex.ToString(), "Make Fake Defect Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
            else // Done
            {
                timer1.Enabled = false;
                ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
                //EnableAllControls(this.tabControl1, Obj, true, ListCtrl_Bypass);
                clsStaticTool.EnableAllControls_Bypass(this.tabControl1, Obj, true, ListCtrl_Bypass);
                Obj.ForeColor = Color.White;
                Obj.Text = "Make Fake Defect";

                try
                {
                    HTuple row, column, phi, length1, length2;
                    clsStaticTool.GetRect2Data(drawing_ID, out row, out column, out phi, out length1, out length2);
                    HOperatorSet.DetachDrawingObjectFromWindow(hSmartWindowControl_map.HalconWindow, drawing_ID);
                    HOperatorSet.ClearDrawingObject(drawing_ID);
                    drawing_ID = null;
                    HObject fake_region_;
                    HOperatorSet.GenRectangle2(out fake_region_, row, column, phi, length1, length2);

                    HObject defect_region_, BackDefect;
                    List<Point> defect_id_ = new List<Point>();
                    this.locate_method_.label_defect(index_capture, fake_region_, out defect_region_, out defect_id_, out BackDefect); // (20181115) Jeff Revised!
                    if (this.locate_method_.b_Defect_Classify) // 多瑕疵
                    {
                        string Name = listView_Edit.SelectedItems[0].Text;
                        Dictionary<string, HObject> defects_with_names = new Dictionary<string, HObject>();
                        defects_with_names.Add(Name, fake_region_);
                        this.locate_method_.label_defect_DefectsClassify(index_capture, defects_with_names, true, true);

                        // 初始化人工覆判
                        this.locate_method_.Initialize_Recheck();

                        // 更新 hSmartWindowControl_map
                        List<string> ListName = new List<string>();
                        ListName.Add(Name);
                        this.locate_method_.Disp_DefectTest(this.hSmartWindowControl_map, radioButton_Result, radioButton_Recheck, radioButton_fill, radioButton_margin, ListName);
                    }
                    else // 單一瑕疵
                    {
                        // 初始化人工覆判
                        locate_method_.Initialize_Recheck();

                        // 更新 hSmartWindowControl_map
                        locate_method_.Disp_DefectTest(this.hSmartWindowControl_map, radioButton_Result, radioButton_Recheck, radioButton_fill, radioButton_margin);
                    }

                    // 更新 richTextBox_log
                    richTextBox_log.AppendText("============" + "\n");
                    richTextBox_log.AppendText("Defect Number : " + this.locate_method_.all_defect_id_.Count + "\n");

                    foreach (Point i in this.locate_method_.all_defect_id_)
                    {
                        richTextBox_log.AppendText("X = " + i.X.ToString() + ", Y = " + i.Y.ToString() + "\n");
                    }
                    richTextBox_log.ScrollToCaret();

                    // 更新 listView_Result
                    this.locate_method_.Update_listView_Result(this.listView_Result, this.radioButton_Result, this.radioButton_Recheck);

                    // 更新 Recipe_AB
                    this.Set_LocateMethod_AB(); // (20200429) Jeff Revised!

                    #region Release memories

                    fake_region_.Dispose();
                    defect_region_.Dispose();
                    BackDefect.Dispose();

                    #endregion

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Make Fake Defect Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
        }

        /// <summary>
        /// 某一取像位置之原始影像
        /// </summary>
        private HObject Image_MoveIndex = null; // (20190706) Jeff Revised!
        /// <summary>
        /// 某一取像位置之完整 Cell regions (平移到原點)
        /// </summary>
        private HObject CellReg_MoveIndex = null; // (20190706) Jeff Revised!
        
        /// <summary>
        /// CellReg_MoveIndex經過Dilation運算
        /// </summary>
        private HObject CellReg_MoveIndex_Dila = null; // (20190708) Jeff Revised!
        
        /// <summary>
        /// 將大圖瑕疵Cell區域 轉到 某一取像位置 (平移到原點)
        /// </summary>
        private List<HObject> DefectReg_MoveIndex = new List<HObject>(); // (20190709) Jeff Revised!
        /// <summary>
        /// 使用者點選之瑕疵名稱
        /// </summary>
        private List<string> DefectName_select = new List<string>(); // (20190709) Jeff Revised!
        
        /// <summary>
        /// 小圖上標註 or 大圖上標註
        /// </summary>
        private string BigSmallMap_label = "小圖上標註"; // (20190708) Jeff Revised!
        
        /// <summary>
        /// 已標註Cell
        /// </summary>
        private HObject cellLabelled_MoveIndex = null; // (20190708) Jeff Revised!
        
        /// <summary>
        /// 單顆標註模式之瑕疵座標
        /// </summary>
        private List<Point> DefectId_SingleCell = new List<Point>(); // (20190710) Jeff Revised!

        /// <summary>
        /// 【人工覆判】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbx_Do_Recheck_CheckedChanged(object sender, EventArgs e) // (20200429) Jeff Revised!
        {
            CheckBox Obj = (CheckBox)sender;
            
            if (Obj.Checked) // 人工覆判
            {
                try
                {
                    DefectName_select.Clear();
                    contextMenuStrip_Recheck.Items.Clear();
                    if (this.locate_method_.b_Defect_Classify && radioButton_MultiCells.Checked) // 多瑕疵 & 多顆標註模式
                    {
                        if (listView_Edit.SelectedIndices.Count <= 0)
                        {
                            #region CheckBox狀態復原
                            cbx_Do_Recheck.CheckedChanged -= cbx_Do_Recheck_CheckedChanged;
                            Obj.Checked = false;
                            cbx_Do_Recheck.CheckedChanged += cbx_Do_Recheck_CheckedChanged;
                            #endregion

                            ctrl_timer1 = listView_Edit;
                            BackColor_ctrl_timer1_1 = Color.AliceBlue;
                            BackColor_ctrl_timer1_2 = Color.Green;
                            timer1.Enabled = true;
                            SystemSounds.Exclamation.Play();
                            MessageBox.Show("請選擇瑕疵名稱", "溫馨提醒", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            timer1.Enabled = false;
                            ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
                            return;
                        }
                        else
                        {
                            DefectName_select.Add(listView_Edit.SelectedItems[0].Text);
                        }
                    }
                    else if (this.locate_method_.b_Defect_Classify && radioButton_SingleCell.Checked) // 多瑕疵 & 單顆標註模式
                    {
                        contextMenuStrip_Recheck.Items.Add(GetMenuItem("OK", Color.Green));
                        foreach (string name in this.locate_method_.DefectsClassify.Keys)
                        {
                            DefectName_select.Add(name);
                            contextMenuStrip_Recheck.Items.Add(GetMenuItem(name, this.locate_method_.DefectsClassify[name].GetColor()));
                        }
                    }
                    else // 單一瑕疵
                    {
                        DefectName_select.Add("");
                        if (radioButton_SingleCell.Checked) // 單顆標註模式
                        {
                            contextMenuStrip_Recheck.Items.Add(GetMenuItem("OK", Color.Green));
                            contextMenuStrip_Recheck.Items.Add(GetMenuItem("NG", Color.Red));
                        }
                    }

                    // 強制勾選【顯示資訊】為【人工覆判結果】
                    radioButton_Recheck.Checked = true;

                    Form_DynamicButtons form = new Form_DynamicButtons("請選擇欲標註之位置", "人工覆判");
                    form.Add_Button("小圖上標註", new Point(10, 100));
                    form.Add_Button("大圖上標註", new Point(130, 100));
                    if (form.ShowDialog() != DialogResult.OK)
                    {
                        #region CheckBox狀態復原
                        cbx_Do_Recheck.CheckedChanged -= cbx_Do_Recheck_CheckedChanged;
                        Obj.Checked = false;
                        cbx_Do_Recheck.CheckedChanged += cbx_Do_Recheck_CheckedChanged;
                        #endregion
                        return;
                    }
                    else
                        BigSmallMap_label = form.GetResult();

                    /* 讀取影像 */
                    if (BigSmallMap_label == "小圖上標註")
                    {
                        if (openFileDialog_file.ShowDialog() != DialogResult.OK)
                        {
                            #region CheckBox狀態復原
                            cbx_Do_Recheck.CheckedChanged -= cbx_Do_Recheck_CheckedChanged;
                            Obj.Checked = false;
                            cbx_Do_Recheck.CheckedChanged += cbx_Do_Recheck_CheckedChanged;
                            #endregion
                            return;
                        }
                        
                        int index = openFileDialog_file.SafeFileName.IndexOf("_M");
                        //int capture_index_ = Convert.ToInt32(openFileDialog_file.SafeFileName.Substring(index + 2, 3).ToString()) - 2;
                        int capture_index_ = Convert.ToInt32(openFileDialog_file.SafeFileName.Substring(index + 2, 3).ToString()) - index_Move + 1; // (20191216) MIL Jeff Revised!

                        if (!(capture_index_ >= 1 && capture_index_ <= this.locate_method_.array_x_count_ * this.locate_method_.array_y_count_))
                        {
                            #region CheckBox狀態復原
                            cbx_Do_Recheck.CheckedChanged -= cbx_Do_Recheck_CheckedChanged;
                            Obj.Checked = false;
                            cbx_Do_Recheck.CheckedChanged += cbx_Do_Recheck_CheckedChanged;
                            #endregion

                            MessageBox.Show("Please Load Right Capture Index Image", "Load Wrong Index of Image", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        
                        index_capture = capture_index_;
                        string file_name_ = openFileDialog_file.FileName;
                        Extension.HObjectMedthods.ReleaseHObject(ref Image_MoveIndex);
                        HOperatorSet.ReadImage(out Image_MoveIndex, file_name_);
                    }
                    else if (BigSmallMap_label == "大圖上標註")
                    {
                        index_capture = -1;
                        Extension.HObjectMedthods.ReleaseHObject(ref Image_MoveIndex);
                        HOperatorSet.CopyObj(this.locate_method_.TileImage, out Image_MoveIndex, 1, -1);
                    }

                    /* 計算此位置Cell */
                    HOperatorSet.SetSystem("clip_region", "false");
                    Extension.HObjectMedthods.ReleaseHObject(ref CellReg_MoveIndex);
                    this.locate_method_.Compute_CellReg_MoveIndex(index_capture, out CellReg_MoveIndex);
                    Extension.HObjectMedthods.ReleaseHObject(ref CellReg_MoveIndex_Dila);
                    HOperatorSet.DilationCircle(CellReg_MoveIndex, out CellReg_MoveIndex_Dila, 5);
                    HOperatorSet.SetSystem("clip_region", "true");

                    /* 計算此位置瑕疵 + 計算此位置已標註Cell */
                    Update_DefectReg_cellLabelled_MoveIndex();

                    // 更新顯示
                    this.locate_method_.Disp_Recheck(hSmartWindowControl_map, Image_MoveIndex, CellReg_MoveIndex,
                                                DefectReg_MoveIndex,  DefectName_select, cellLabelled_MoveIndex, true);
                    
                    //ListCtrl_Bypass = EnableAllControls(this.tabControl1, Obj, false);
                    ListCtrl_Bypass = clsStaticTool.EnableAllControls_Bypass(this.tabControl1, Obj, false);
                    Obj.ForeColor = Color.GreenYellow;
                    Obj.Text = "設定完成";
                    ctrl_timer1 = Obj;
                    BackColor_ctrl_timer1_1 = Color.DarkViolet;
                    BackColor_ctrl_timer1_2 = Color.Lime;
                    timer1.Enabled = true;

                    label_cellID.Enabled = true;
                    txt_cellID.Enabled = true;

                    // 致能滑鼠點擊事件
                    this.hSmartWindowControl_map.HMouseDown += new HalconDotNet.HMouseEventHandler(this.hSmartWindowControl_map_HMouseDown_Recheck);
                }
                catch (Exception ex)
                {
                    #region CheckBox狀態復原
                    cbx_Do_Recheck.CheckedChanged -= cbx_Do_Recheck_CheckedChanged;
                    Obj.Checked = false;
                    cbx_Do_Recheck.CheckedChanged += cbx_Do_Recheck_CheckedChanged;
                    #endregion

                    MessageBox.Show(ex.ToString(), "人工覆判失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

            }
            else // 設定完成
            {
                // 更新 locate_method_B_2_A 之人工覆判
                this.Update_Locate_method_B_2_A_Recheck(); // (20200429) Jeff Revised!

                timer1.Enabled = false;
                ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
                //EnableAllControls(this.tabControl1, Obj, true, ListCtrl_Bypass);
                clsStaticTool.EnableAllControls_Bypass(this.tabControl1, Obj, true, ListCtrl_Bypass);
                Obj.ForeColor = Color.White;
                Obj.Text = "人工覆判";

                // 禁能滑鼠點擊事件
                this.hSmartWindowControl_map.HMouseDown -= new HalconDotNet.HMouseEventHandler(this.hSmartWindowControl_map_HMouseDown_Recheck);

                try
                {
                    // 更新 hSmartWindowControl_map
                    if (!this.locate_method_.b_Defect_Classify) // 單一瑕疵
                    {
                        this.locate_method_.Disp_DefectTest(this.hSmartWindowControl_map, radioButton_Result, radioButton_Recheck, radioButton_fill, radioButton_margin);
                    }
                    else // 多瑕疵
                    {
                        List<string> ListName = new List<string>();
                        foreach (string name in DefectName_select)
                            ListName.Add(name);
                        this.locate_method_.Disp_DefectTest(this.hSmartWindowControl_map, radioButton_Result, radioButton_Recheck, radioButton_fill, radioButton_margin, ListName);
                    }

                    // 更新 listView_Result
                    this.locate_method_.Update_listView_Result(this.listView_Result, this.radioButton_Result, this.radioButton_Recheck);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "人工覆判失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
        }
        
        /// <summary>
        /// 計算此位置瑕疵 + 計算此位置已標註Cell
        /// </summary>
        private void Update_DefectReg_cellLabelled_MoveIndex() // (20190710) Jeff Revised!
        {
            /* 計算此位置瑕疵 */
            clsStaticTool.DisposeAll(DefectReg_MoveIndex);
            DefectReg_MoveIndex.Clear();
            if (!this.locate_method_.b_Defect_Classify) // 單一瑕疵
            {
                HObject reg;
                this.locate_method_.Compute_DefectReg_MoveIndex(index_capture, this.locate_method_.all_intersection_defect_Recheck, out reg);
                DefectReg_MoveIndex.Add(reg);
                //reg.Dispose();
            }
            else // 多瑕疵
            {
                foreach (string name in DefectName_select)
                {
                    HObject reg;
                    this.locate_method_.Compute_DefectReg_MoveIndex(index_capture, this.locate_method_.DefectsClassify[name].all_intersection_defect_Recheck, out reg);
                    DefectReg_MoveIndex.Add(reg);
                    //reg.Dispose();
                }
            }

            /* 計算此位置已標註Cell */
            Extension.HObjectMedthods.ReleaseHObject(ref cellLabelled_MoveIndex);
            this.locate_method_.Compute_DefectReg_MoveIndex(index_capture, this.locate_method_.all_intersection_cellLabelled_Recheck, out cellLabelled_MoveIndex);
        }
        
        /// <summary>
        /// 滑鼠點擊事件 (人工覆判)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hSmartWindowControl_map_HMouseDown_Recheck(object sender, HMouseEventArgs e) // (20190710) Jeff Revised!
        {
            // 游標是否於 Cell region 內
            HTuple hv_Index = new HTuple();
            HOperatorSet.GetRegionIndex(CellReg_MoveIndex, (int)(e.Y + 0.5), (int)(e.X + 0.5), out hv_Index);
            if (hv_Index.Length <= 0)
            {
                txt_cellID.Text = "";
                return;
            }

            HTuple cell_ids = new HTuple();
            List<Point> ListCellID = new List<Point>();
            // 計算此顆Cell座標
            if (!this.locate_method_.pos_2_cellID_MoveIndex(index_capture, e.Y, e.X, out cell_ids, out ListCellID))
                return;

            if (e.Button == System.Windows.Forms.MouseButtons.Left) // 滑鼠是否點擊左鍵
            {
                #region 滑鼠點擊左鍵Cell座標 (X, Y)

                // 更新顯示
                string str = "";
                foreach (Point pt in ListCellID)
                    str += "(" + pt.X + ", " + pt.Y + ")";
                txt_cellID.Text = str;
                this.locate_method_.Disp_Recheck(hSmartWindowControl_map, Image_MoveIndex, CellReg_MoveIndex,
                                            DefectReg_MoveIndex, DefectName_select, cellLabelled_MoveIndex, false);

                // 顯示上下文選單
                toolStripMenuItem_ID.Text = str;
                HTuple rowWindow, columnWindow;
                HOperatorSet.ConvertCoordinatesImageToWindow(hSmartWindowControl_map.HalconWindow, e.Y, e.X, out rowWindow, out columnWindow);
                contextMenuStrip_DispID.Show(hSmartWindowControl_map, (int)(columnWindow.D + 0.5), (int)(rowWindow.D + 0.5));
                
                // 顯示此位置Cell
                HOperatorSet.SetDraw(hSmartWindowControl_map.HalconWindow, "margin");
                HOperatorSet.SetColor(hSmartWindowControl_map.HalconWindow, "#FF00FFFF"); // Magenta
                HTuple line_w;
                HOperatorSet.GetLineWidth(hSmartWindowControl_map.HalconWindow, out line_w);
                HOperatorSet.SetLineWidth(hSmartWindowControl_map.HalconWindow, 3);
                HObject reg;
                HOperatorSet.SelectObj(CellReg_MoveIndex_Dila, out reg, hv_Index);
                HOperatorSet.DispObj(reg, hSmartWindowControl_map.HalconWindow);
                reg.Dispose();
                HOperatorSet.SetLineWidth(hSmartWindowControl_map.HalconWindow, line_w);

                #endregion

                return;
            }

            if (e.Button != System.Windows.Forms.MouseButtons.Right) // 滑鼠是否點擊右鍵
                return;

            if (radioButton_MultiCells.Checked) // 多顆標註模式 (小圖上標註 & 大圖上標註)
            {
                #region 多顆標註模式

                // 判斷是要判定此顆Cell為NG or OK: 利用游標是否於 DefectReg_MoveIndex 內
                bool b_NG = false;
                HOperatorSet.GetRegionIndex(DefectReg_MoveIndex[0], (int)(e.Y + 0.5), (int)(e.X + 0.5), out hv_Index);
                if (hv_Index.Length <= 0)
                    b_NG = true;
                
                /* 更新人工覆判結果 */
                if (b_NG) // 標註此顆Cell NG
                    this.locate_method_.Update_Recheck(null, ListCellID, DefectName_select[0]);
                else // 標註此顆Cell OK
                    this.locate_method_.Update_Recheck(ListCellID, null, DefectName_select[0]);

                /* 計算此位置瑕疵 + 計算此位置已標註Cell */
                Update_DefectReg_cellLabelled_MoveIndex();

                // 更新顯示
                this.locate_method_.Disp_Recheck(hSmartWindowControl_map, Image_MoveIndex, CellReg_MoveIndex,
                                            DefectReg_MoveIndex, DefectName_select, cellLabelled_MoveIndex, false);

                // 更新 listView_Result
                this.locate_method_.Update_listView_Result(this.listView_Result, this.radioButton_Result, this.radioButton_Recheck);

                #endregion

            }
            else if (radioButton_SingleCell.Checked) // 單顆標註模式
            {
                #region 單顆標註模式

                if (ListCellID.Count != 1)
                {
                    SystemSounds.Exclamation.Play();
                    MessageBox.Show("【單顆標註模式】一次僅能選擇一顆Cell做標註", "溫馨提醒", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    DefectId_SingleCell.Clear();
                    DefectId_SingleCell.Add(ListCellID[0]);
                }

                /* 更新上下文選單 */
                if (!this.locate_method_.b_Defect_Classify) // 單一瑕疵
                {
                    if (this.locate_method_.all_defect_id_Recheck.Contains(DefectId_SingleCell[0])) // NG Cell
                    {
                        ((ToolStripMenuItem)contextMenuStrip_Recheck.Items[0]).Checked = false;
                        ((ToolStripMenuItem)contextMenuStrip_Recheck.Items[1]).Checked = true;
                    }
                    else // OK Cell
                    {
                        ((ToolStripMenuItem)contextMenuStrip_Recheck.Items[0]).Checked = true;
                        ((ToolStripMenuItem)contextMenuStrip_Recheck.Items[1]).Checked = false;
                    }
                }
                else // 多瑕疵
                {
                    // 判斷各瑕疵類型
                    bool b_OKCell = true;
                    for (int i = 1; i <= DefectName_select.Count; i++)
                    {
                        if (this.locate_method_.DefectsClassify[DefectName_select[i - 1]].all_defect_id_Recheck.Contains(DefectId_SingleCell[0])) // 包含此瑕疵類型
                        {
                            ((ToolStripMenuItem)contextMenuStrip_Recheck.Items[i]).Checked = true;
                            b_OKCell = false;
                        }
                        else
                            ((ToolStripMenuItem)contextMenuStrip_Recheck.Items[i]).Checked = false;
                    }
                    if (b_OKCell) // OK Cell
                        ((ToolStripMenuItem)contextMenuStrip_Recheck.Items[0]).Checked = true;
                    else // NG Cell
                        ((ToolStripMenuItem)contextMenuStrip_Recheck.Items[0]).Checked = false;

                }

                /* 顯示上下文選單 */
                HTuple rowWindow, columnWindow;
                HOperatorSet.ConvertCoordinatesImageToWindow(hSmartWindowControl_map.HalconWindow, e.Y, e.X, out rowWindow, out columnWindow);
                contextMenuStrip_Recheck.Show(hSmartWindowControl_map, (int)(columnWindow.D + 0.5), (int)(rowWindow.D + 0.5));
                
                #endregion

            }
            
        }
        
        /// <summary>
        /// 生成選單項
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="ForeColor"></param>
        /// <param name="img"></param>
        /// <param name="b_ClickEvent"></param>
        /// <returns></returns>
        private ToolStripMenuItem GetMenuItem(string txt, object ForeColor = null, Image img = null, bool b_ClickEvent = true) // (20190710) Jeff Revised!
        {
            ToolStripMenuItem menuItem = new ToolStripMenuItem();

            // 屬性設定
            menuItem.Text = txt;
            if (ForeColor is Color)
                menuItem.ForeColor = (Color)ForeColor;
            if (img != null)
                menuItem.Image = img;

            // 點擊觸發事件設定
            if (b_ClickEvent)
                menuItem.Click += new EventHandler(toolStripMenuItem_Click);

            return menuItem;
        }
        
        /// <summary>
        /// 選單項事件響應
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem_Click(object sender, EventArgs e) // (20190710) Jeff Revised!
        {
            ToolStripMenuItem menu = sender as ToolStripMenuItem;
            //MessageBox.Show(menu.Text);
            string name = menu.Text;

            // 如果原先狀態為OK啟用，則不改變狀態! (因為不知道要判定哪一種瑕疵類型)
            if (name == "OK" && menu.Checked)
                return;

            // Checked 狀態改變
            menu.Checked = !(menu.Checked);

            /* 更新人工覆判結果 */
            if (name == "OK") // 點擊【OK】
                this.locate_method_.Update_All_OK_Recheck(DefectId_SingleCell);
            else // 點擊其他瑕疵類型
            {
                if (menu.Checked) // 標註此種瑕疵
                    this.locate_method_.Update_Recheck(null, DefectId_SingleCell, name);
                else // 解除標註此種瑕疵
                    this.locate_method_.Update_Recheck(DefectId_SingleCell, null, name);
            }

            /* 計算此位置瑕疵 + 計算此位置已標註Cell */
            this.Update_DefectReg_cellLabelled_MoveIndex();

            // 更新顯示
            this.locate_method_.Disp_Recheck(hSmartWindowControl_map, Image_MoveIndex, CellReg_MoveIndex,
                                        DefectReg_MoveIndex, DefectName_select, cellLabelled_MoveIndex, false);

            // 更新 listView_Result
            this.locate_method_.Update_listView_Result(this.listView_Result, this.radioButton_Result, this.radioButton_Recheck);

        }
        
        /// <summary>
        /// 【清空人工覆判】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Clear_Recheck_Click(object sender, EventArgs e) // (20200429) Jeff Revised!
        {
            // 新增使用者保護機制
            SystemSounds.Exclamation.Play();
            DialogResult dr = MessageBox.Show("確定要清空人工覆判 ?", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr != DialogResult.Yes)
                return;

            this.locate_method_.Initialize_Recheck();

            // 更新 hSmartWindowControl_map
            this.locate_method_.Disp_DefectTest(this.hSmartWindowControl_map, radioButton_Result, radioButton_Recheck, radioButton_fill, radioButton_margin);

            // 更新 listView_Edit
            this.locate_method_.Update_listView_Edit(this.listView_Edit, this.radioButton_Result, this.radioButton_Recheck);

            // 更新 listView_Result
            this.locate_method_.Update_listView_Result(this.listView_Result, this.radioButton_Result, this.radioButton_Recheck);

            // 更新 locate_method_B_2_A 之人工覆判
            this.Update_Locate_method_B_2_A_Recheck(); // (20200429) Jeff Revised!
        }

        #endregion



        #region 【瑕疵分類統計】 // (20200429) Jeff Revised!

        private cls_NGClassify_Statistics_Multi Cls_NGClassify_Statistics_Multi { get; set; } = new cls_NGClassify_Statistics_Multi();

        /// <summary>
        /// 【批量載入】 (【瑕疵分類統計】)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_BatchLoad_NG_Classify_Multi_Click(object sender, EventArgs e)
        {
            using (Form_editBatchTest form = new Form_editBatchTest("【瑕疵分類統計】", this.BatchTest, false, true))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    this.ComboBox_BatchRecipes_NG_Classify_Multi.Items.Clear();
                    this.ComboBox_BatchRecipes_NG_Classify_Multi.Text = "";
                    this.txt_TotalChip.Text = "";
                    this.listView_NGClassify_Statistics_Multi.Items.Clear();

                    // 搜尋所有序號 (i.e. 片號)
                    List<string> list_SBID = new List<string>();
                    if (this.BatchTest.Get_AllSBID_from_partNumber(out list_SBID) == false)
                        return;

                    Dictionary<string, List<string>> Recipes_ListPath = new Dictionary<string, List<string>>();
                    #region 搜尋該LotNum之所有檢測站點 & 各自對應之所有 雙面合併工單 資料夾路徑

                    foreach (string sbid in list_SBID) // For 各序號 (i.e. 片號)
                    {
                        // 更新序號
                        this.BatchTest.SBID = sbid;

                        // 搜尋該序號之所有檢測站點
                        if (this.BatchTest.Update_Directories())
                        {
                            for (int i = 0; i < this.BatchTest.productNames.Count; i++) // For 各檢測站點
                            {
                                string productName = this.BatchTest.productNames[i];
                                string dir_productName = this.BatchTest.Directory_AB + productName;
                                if (Recipes_ListPath.ContainsKey(productName) == false)
                                    Recipes_ListPath.Add(productName, new List<string>() { dir_productName });
                                else
                                    Recipes_ListPath[productName].Add(dir_productName);
                            }
                        }
                    }
                    this.Cls_NGClassify_Statistics_Multi.Initialize(Recipes_ListPath);

                    #endregion

                    // 更新GUI
                    foreach (string productName in Recipes_ListPath.Keys)
                        this.ComboBox_BatchRecipes_NG_Classify_Multi.Items.Add(productName);
                }
            }
        }

        /// <summary>
        /// 【檢測站點】選擇
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_BatchRecipes_NG_Classify_Multi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ComboBox_BatchRecipes_NG_Classify_Multi.SelectedIndex < 0)
                return;

            string productName = this.ComboBox_BatchRecipes_NG_Classify_Multi.SelectedItem.ToString();
            int totalChip = this.Cls_NGClassify_Statistics_Multi.Recipes_ListPath[productName].Count;
            this.txt_TotalChip.Text = totalChip.ToString();

            this.Cls_NGClassify_Statistics_Multi.Compute_NGClassify_Statistics_Multi(productName, this.listView_NGClassify_Statistics_Multi, this.locate_method_.get_total_cell_count());
        }
        
        #endregion



        #region 【瑕疵整合輸出】 // (20200429) Jeff Revised!

        private cls_DefectCombined Recipe_defComb = new cls_DefectCombined();
        
        /// <summary>
        /// 是否正在載入參數至GUI (【瑕疵整合輸出】)
        /// </summary>
        private bool b_loadParams_defComb { get; set; } = false;

        /// <summary>
        /// 將UI內容與Class參數互傳
        /// </summary>
        /// <param name="ui_2_parameters_">True=UI傳至Class ; False=Class傳至UI</param>
        private void UI_Params_defComb(bool ui_2_parameters_)
        {
            try
            {
                if (ui_2_parameters_)
                {
                    #region 將UI內容回傳至Class

                    #region 【瑕疵標注卡設定】

                    // Cell資訊
                    this.Recipe_defComb.cell_pWidth = double.Parse(this.nud_cell_pWidth_defComb.Value.ToString());
                    this.Recipe_defComb.cell_pHeight = double.Parse(this.nud_cell_pHeight_defComb.Value.ToString());
                    this.Recipe_defComb.cell_pdx = double.Parse(this.nud_cell_pdx_defComb.Value.ToString());
                    this.Recipe_defComb.cell_pdy = double.Parse(this.nud_cell_pdy_defComb.Value.ToString());
                    this.Recipe_defComb.Str_CellColor_Halcon = clsStaticTool.GetHalconColor(this.btn_SetCellColor_defComb.BackColor);
                    this.Recipe_defComb.Str_NGColor_Halcon = clsStaticTool.GetHalconColor(this.btn_SetNGColor_defComb.BackColor);

                    // 輸出設定
                    if (this.rbt_OutputImg_defComb.Checked)
                    {
                        this.Recipe_defComb.OutputType = enu_OutputType.Image;
                        this.Recipe_defComb.ImageFormat = (enu_DispImageFormat)this.cbx_OutputFormat_defComb.SelectedIndex;
                    }
                    else if (this.rbt_OutputWord_defComb.Checked)
                    {
                        this.Recipe_defComb.OutputType = enu_OutputType.Word;
                        this.Recipe_defComb.WordFormat = (enu_WordFormat)this.cbx_OutputFormat_defComb.SelectedIndex;
                    }
                    else
                        this.Recipe_defComb.OutputType = enu_OutputType.PDF;
                    this.Recipe_defComb.ImageWidth_cm = float.Parse(this.nud_ImageWidth_cm_defComb.Value.ToString());
                    this.Recipe_defComb.ImageHeight_cm = float.Parse(this.nud_ImageHeight_cm_defComb.Value.ToString());

                    #endregion

                    this.Recipe_defComb.Path_Image = this.tbx_PathImage_defComb.Text;
                    this.Recipe_defComb.B_dispCell = this.cbx_dispCell_defComb.Checked;

                    #endregion

                }
                else
                {
                    #region 將Class內容回傳至UI

                    this.b_loadParams_defComb = true;

                    #region 各單/雙面瑕疵結果工單

                    this.cbx_SelectRecipe_defComb.Items.Clear();
                    this.UI_Params_Recipe_defComb(false);

                    #endregion

                    #region 【瑕疵標注卡設定】

                    // Cell資訊
                    this.nud_cell_pWidth_defComb.Value = (decimal)this.Recipe_defComb.cell_pWidth;
                    this.nud_cell_pHeight_defComb.Value = (decimal)this.Recipe_defComb.cell_pHeight;
                    this.nud_cell_pdx_defComb.Value = (decimal)this.Recipe_defComb.cell_pdx;
                    this.nud_cell_pdy_defComb.Value = (decimal)this.Recipe_defComb.cell_pdy;
                    this.btn_SetCellColor_defComb.BackColor = clsStaticTool.GetSystemColor(this.Recipe_defComb.Str_CellColor_Halcon);
                    this.btn_SetNGColor_defComb.BackColor = clsStaticTool.GetSystemColor(this.Recipe_defComb.Str_NGColor_Halcon);

                    // X方向
                    if (int.Parse(this.nud_num_region_x_defComb.Value.ToString()) == this.Recipe_defComb.num_region_x)
                        this.nud_num_region_x_defComb_ValueChanged(null, null);
                    else
                        this.nud_num_region_x_defComb.Value = this.Recipe_defComb.num_region_x;

                    // Y方向
                    if (int.Parse(this.nud_num_region_y_defComb.Value.ToString()) == this.Recipe_defComb.num_region_y)
                        this.nud_num_region_y_defComb_ValueChanged(null, null);
                    else
                        this.nud_num_region_y_defComb.Value = this.Recipe_defComb.num_region_y;

                    // Bypass Cell
                    this.Update_listView_BypassCell_defComb();

                    /* 輸出設定 */
                    if (this.Recipe_defComb.OutputType == enu_OutputType.Image)
                        this.rbt_OutputImg_defComb.Checked = true;
                    else if (this.Recipe_defComb.OutputType == enu_OutputType.Word)
                        this.rbt_OutputWord_defComb.Checked = true;
                    else
                        this.rbt_OutputPDF_defComb.Checked = true;

                    // 更新【輸出格式】下拉式選單項目
                    this.b_loadParams_defComb = false;
                    this.OutputType_defComb_CheckedChanged(null, null);
                    this.b_loadParams_defComb = true;

                    this.nud_ImageWidth_cm_defComb.Value = (decimal)this.Recipe_defComb.ImageWidth_cm;
                    this.nud_ImageHeight_cm_defComb.Value = (decimal)this.Recipe_defComb.ImageHeight_cm;

                    #endregion

                    this.tbx_PathImage_defComb.Text = this.Recipe_defComb.Path_Image;
                    this.cbx_dispCell_defComb.Checked = this.Recipe_defComb.B_dispCell;

                    this.b_loadParams_defComb = false;

                    #endregion

                }
            }
            catch (Exception ex)
            { }
        }

        /// <summary>
        /// 【批量載入儲存】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_BatchLoadSave_defComb_Click(object sender, EventArgs e) // (20200429) Jeff Revised!
        {
            using (Form_editBatchTest form = new Form_editBatchTest("【批量載入儲存】瑕疵整合輸出", this.BatchTest, true))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    List<string> list_SBID = new List<string>();
                    if (this.BatchTest.B_All_SBID) // 只輸入生產料號
                    {
                        if (this.BatchTest.Get_AllSBID_from_partNumber(out list_SBID) == false)
                            return;
                    }
                    else
                        list_SBID.Add(this.BatchTest.SBID);

                    this.tabPage_defectCombined.Enabled = false;
                    
                    ctrl_timer1 = this.button_BatchLoadSave_defComb;
                    BackColor_ctrl_timer1_1 = Color.Magenta;
                    BackColor_ctrl_timer1_2 = Color.Green;
                    timer1.Enabled = true;

                    if (this.m_ProgressBar == null)
                    {
                        m_ProgressBar = new clsProgressbar();

                        m_ProgressBar.FormClosedEvent2 += new clsProgressbar.FormClosedHandler2(SetFormClosed2);

                        m_ProgressBar.SetShowText("請等待【批量載入儲存】......");
                        m_ProgressBar.SetShowCaption("執行中......");
                        m_ProgressBar.SetShowText_location(new Point(11, 1), new Size(387, 48));
                        m_ProgressBar.ShowWaitProgress();
                    }

                    bool b_success = true;
                    for (int i_id = 0; i_id < list_SBID.Count; i_id++)
                    {
                        this.BatchTest.SBID = list_SBID[i_id];

                        #region 每一序號做計算及輸出結果

                        if (this.BatchTest.B_combine_1File) // 整合成一個檔案輸出
                            m_ProgressBar.SetShowText("請等待【批量載入儲存】\n共要產出" + list_SBID.Count.ToString() + "個報表\n已完成" + i_id.ToString() + "個");
                        else
                            m_ProgressBar.SetShowText("請等待【批量載入儲存】\n共要輸出" + list_SBID.Count.ToString() + "個檔案\n已完成" + i_id.ToString() + "個");

                        if (this.BatchTest.Update_Directories())
                        {
                            // 載入瑕疵整合工單
                            cls_DefectCombined defComb = new cls_DefectCombined();
                            if (cls_DefectCombined.Load_Recipe(out defComb, FinalInspectParam.Directory_defComb, false))
                            {
                                defComb.RemoveAll(); // 先清空瑕疵檔
                                //【新增瑕疵檔】
                                for (int i = 0; i < this.BatchTest.Directories.Count; i++)
                                    defComb.Add(true, false, false, this.BatchTest.Directories[i], false);

                                // 執行計算
                                defComb.Execute();

                                // 更新工單類別物件
                                this.Recipe_defComb.Release();
                                if (this.BatchTest.B_combine_1File) // 整合成一個檔案輸出
                                    defComb.Assign_clsWordPDF(this.Recipe_defComb.Get_clsWordPDF());
                                this.Recipe_defComb = defComb;

                                // 輸出結果檔案
                                if (Directory.Exists(FinalInspectParam.OutputDirectory_defComb) == false)
                                    Directory.CreateDirectory(FinalInspectParam.OutputDirectory_defComb);

                                if (this.BatchTest.B_combine_1File) // 整合成一個檔案輸出
                                {
                                    bool b_Initial = false, b_End = false;
                                    if (i_id == 0)
                                        b_Initial = true;
                                    if (i_id == list_SBID.Count - 1)
                                        b_End = true;
                                    this.Recipe_defComb.Save_Result(false, FinalInspectParam.OutputDirectory_defComb + "\\" + this.BatchTest.partNumber,
                                                                    true, b_Initial, b_End);
                                }
                                else
                                    this.Recipe_defComb.Save_Result(false, FinalInspectParam.OutputDirectory_defComb + "\\" + this.BatchTest.partNumber + "_" + this.BatchTest.SBID);
                            }
                            else
                                b_success = false;
                        }
                        else
                            b_success = false;

                        #endregion
                    }

                    if (b_success)
                    {
                        this.m_ProgressBar.SetShowText("【批量載入儲存】完成，執行GUI更新......");

                        // 更新GUI顯示參數
                        this.tbx_Directory_defComb.Text = this.Recipe_defComb.Directory;
                        this.UI_Params_defComb(false);

                        // 顯示結果影像
                        if (this.rbt_ResultImg_defComb.Checked)
                            this.Display_defComb_CheckedChanged(null, null);
                        else
                            this.rbt_ResultImg_defComb.Checked = true;
                        HOperatorSet.SetPart(this.hSmartWindowControl_defComb.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part
                    }

                    timer1.Enabled = false;
                    ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
                    this.tabPage_defectCombined.Enabled = true;
                    this.m_ProgressBar.CloseProgress();
                    this.m_ProgressBar = null;

                    this.Focus();
                    if (b_success)
                        MessageBox.Show("儲存成功!");
                    
                }
            }
        }
        
        /// <summary>
        /// 【載入工單】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_LoadRecipe_defComb_Click(object sender, EventArgs e)
        {
            cls_DefectCombined defComb = new cls_DefectCombined();
            if (cls_DefectCombined.Load_Recipe(out defComb, this.tbx_Directory_defComb.Text))
            {
                // 更新工單類別物件
                this.Recipe_defComb.Release();
                this.Recipe_defComb = defComb;

                // 更新GUI顯示參數
                this.tbx_Directory_defComb.Text = this.Recipe_defComb.Directory;
                this.UI_Params_defComb(false);

                // 顯示結果影像
                if (this.rbt_ResultImg_defComb.Checked)
                    this.Display_defComb_CheckedChanged(null, null);
                else
                    this.rbt_ResultImg_defComb.Checked = true;
                HOperatorSet.SetPart(this.hSmartWindowControl_defComb.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part
            }
        }

        /// <summary>
        /// 【儲存工單】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SaveRecipe_defComb_Click(object sender, EventArgs e)
        {
            // 新增使用者保護機制
            SystemSounds.Exclamation.Play();
            DialogResult dr = MessageBox.Show("確定要【儲存工單】?", "溫馨提醒", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr != DialogResult.Yes)
                return;

            this.btn_Save_defComb_Click(null, null); // 【儲存】使用者選擇之工單
            this.UI_Params_defComb(true); // 更新參數
            if (this.Recipe_defComb.Save_Recipe())
                this.tbx_Directory_defComb.Text = this.Recipe_defComb.Directory;
        }

        #region 各單/雙面瑕疵結果工單

        /// <summary>
        /// 【新增瑕疵檔】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Add_defComb_Click(object sender, EventArgs e)
        {
            if (this.Recipe_defComb.Add(this.rbt_Double_defComb.Checked, this.cbx_B_UpDown_invert.Checked, this.cbx_B_LeftRight_invert.Checked, this.tbx_Path_defComb.Text))
            {
                this.cbx_SelectRecipe_defComb.Items.Clear();
                this.UI_Params_Recipe_defComb(false);
            }
        }

        /// <summary>
        /// 【儲存】使用者選擇之工單
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Save_defComb_Click(object sender, EventArgs e)
        {
            this.UI_Params_Recipe_defComb(true);
        }

        /// <summary>
        /// 【刪除】使用者選擇之工單
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Remove_defComb_Click(object sender, EventArgs e)
        {
            // 新增使用者保護機制
            SystemSounds.Exclamation.Play();
            DialogResult dr = MessageBox.Show("確定要【刪除】使用者選擇之工單?", "溫馨提醒", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr != DialogResult.Yes)
                return;

            this.Recipe_defComb.Remove(this.cbx_SelectRecipe_defComb.SelectedIndex);
            this.cbx_SelectRecipe_defComb.Items.Clear();
            this.UI_Params_Recipe_defComb(false);
        }

        /// <summary>
        /// 【清空】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_RemoveAll_defComb_Click(object sender, EventArgs e)
        {
            // 新增使用者保護機制
            SystemSounds.Exclamation.Play();
            DialogResult dr = MessageBox.Show("確定要【清空】所有工單?", "溫馨提醒", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr != DialogResult.Yes)
                return;

            this.Recipe_defComb.RemoveAll();
            this.cbx_SelectRecipe_defComb.Items.Clear();
            this.cbx_SelectRecipe_defComb.Text = "";
            this.UI_Params_Recipe_defComb(false);
        }
        
        /// <summary>
        /// 工單選擇切換
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbx_SelectRecipe_defComb_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.UI_Params_Recipe_defComb(false);
        }
        
        /// <summary>
        /// 將UI內容與Class參數互傳 (各單/雙面瑕疵結果工單)
        /// </summary>
        /// <param name="ui_2_parameters_">True=UI傳至Class ; False=Class傳至UI</param>
        private void UI_Params_Recipe_defComb(bool ui_2_parameters_)
        {
            try
            {
                int index = this.cbx_SelectRecipe_defComb.SelectedIndex;

                if (ui_2_parameters_)
                {
                    #region 將UI內容回傳至Class

                    if (index >= 0 && index < this.Recipe_defComb.Recipes_defComb.Count)
                    {
                        cls_DefectCombined.DefectResult_1pcs dr = this.Recipe_defComb.Recipes_defComb[index];
                        dr.Directory = this.tbx_Path_defComb.Text;
                        dr.B_doubleSide = this.rbt_Double_defComb.Checked;
                        dr.B_UpDown_invert = this.cbx_B_UpDown_invert.Checked;
                        dr.B_LeftRight_invert = this.cbx_B_LeftRight_invert.Checked;
                    }

                    #endregion

                }
                else
                {
                    #region 將Class內容回傳至UI
                    
                    if (index < 0 || index >= this.Recipe_defComb.Recipes_defComb.Count) // 更新所有工單
                    {
                        this.tbx_Path_defComb.Text = "";
                        this.rbt_Double_defComb.Checked = true;
                        this.cbx_B_UpDown_invert.Checked = false;
                        this.cbx_B_LeftRight_invert.Checked = false;

                        this.cbx_SelectRecipe_defComb.Items.Clear();
                        for (int i = 0; i < this.Recipe_defComb.Recipes_defComb.Count; i++)
                            this.cbx_SelectRecipe_defComb.Items.Add((i + 1).ToString());
                        if (this.cbx_SelectRecipe_defComb.Items.Count > 0)
                            this.cbx_SelectRecipe_defComb.SelectedIndex = this.cbx_SelectRecipe_defComb.Items.Count - 1;
                    }
                    else // 更新顯示使用者選擇之工單
                    {
                        cls_DefectCombined.DefectResult_1pcs dr = this.Recipe_defComb.Recipes_defComb[index];
                        this.tbx_Path_defComb.Text = dr.Directory;
                        if (dr.B_doubleSide)
                            this.rbt_Double_defComb.Checked = true;
                        else
                            this.rbt_Single_defComb.Checked = true;
                        this.cbx_B_UpDown_invert.Checked = dr.B_UpDown_invert;
                        this.cbx_B_LeftRight_invert.Checked = dr.B_LeftRight_invert;
                    }
                    
                    #endregion

                }
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region 【瑕疵標注卡設定】

        /// <summary>
        /// 設定顏色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_SetColor_Click(object sender, EventArgs e)
        {
            Button bt = sender as Button;
            this.colorDialog_SetColor.Color = bt.BackColor; // 初始顏色
            if (this.colorDialog_SetColor.ShowDialog() != DialogResult.Cancel)
                bt.BackColor = this.colorDialog_SetColor.Color;
        }

        /// <summary>
        /// 各分區資訊 (X/Y方向) 參數更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void param_regInfo_defComb_ValueChanged(object sender, EventArgs e)
        {
            if (this.b_loadParams_defComb)
                return;

            #region GUI參數更新到 Recipe_defComb

            // X方向
            int index = this.cbx_region_x_defComb.SelectedIndex;
            this.Recipe_defComb.reg_cell_x_count[index] = int.Parse(this.nud_reg_cell_x_count_defComb.Value.ToString());
            this.Recipe_defComb.dist_reg_pdx[index] = double.Parse(this.nud_dist_reg_pdx_defComb.Value.ToString());

            // Y方向
            index = this.cbx_region_y_defComb.SelectedIndex;
            this.Recipe_defComb.reg_cell_y_count[index] = int.Parse(this.nud_reg_cell_y_count_defComb.Value.ToString());
            this.Recipe_defComb.dist_reg_pdy[index] = double.Parse(this.nud_dist_reg_pdy_defComb.Value.ToString());

            #endregion

            /* 更新GUI顯示參數 */
            // X方向總Cell顆數
            this.tbx_cell_col_count_defComb.Text = this.Recipe_defComb.cell_col_count.ToString();
            // Y方向總Cell顆數
            this.tbx_cell_row_count_defComb.Text = this.Recipe_defComb.cell_row_count.ToString();
        }

        /// <summary>
        /// 【分區數量】(X方向) 數值更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nud_num_region_x_defComb_ValueChanged(object sender, EventArgs e)
        {
            // num_region_x 數值更新
            if (this.b_loadParams_defComb == false) // 手動更新【分區數量】
                this.Recipe_defComb.Update_num_region_x(int.Parse(this.nud_num_region_x_defComb.Value.ToString()));

            /* 更新GUI顯示參數 */
            // X方向總Cell顆數
            this.tbx_cell_col_count_defComb.Text = this.Recipe_defComb.cell_col_count.ToString();
            // 各分區資訊 (X方向)
            this.cbx_region_x_defComb.Items.Clear();
            for (int i = 0; i < this.Recipe_defComb.num_region_x; i++)
                this.cbx_region_x_defComb.Items.Add((i + 1).ToString());
            if (this.cbx_region_x_defComb.Items.Count > 0)
                this.cbx_region_x_defComb.SelectedIndex = 0;
        }
        
        /// <summary>
        /// 區域選擇切換 (X方向)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbx_region_x_defComb_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = this.cbx_region_x_defComb.SelectedIndex;
            if (index < 0)
                return;

            bool b_loadParams_defComb_orig = this.b_loadParams_defComb;
            this.b_loadParams_defComb = true;

            /* 更新GUI顯示參數 */
            this.nud_reg_cell_x_count_defComb.Value = this.Recipe_defComb.reg_cell_x_count[index]; // Cell顆數
            this.nud_dist_reg_pdx_defComb.Value = (decimal)this.Recipe_defComb.dist_reg_pdx[index]; // 與原點距離 (Pixel)

            this.b_loadParams_defComb = b_loadParams_defComb_orig;
        }
        
        /// <summary>
        /// X方向總Cell顆數更新時，要更新【Bypass Cell】X座標之最大值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbx_cell_col_count_defComb_TextChanged(object sender, EventArgs e)
        {
            // 非數字，則直接return
            int value;
            if (!(int.TryParse(this.tbx_cell_col_count_defComb.Text, out value)))
                return;

            // 更新【Bypass Cell】
            if (value > 0)
            {
                if (int.Parse(this.nud_X_BypassCell_defComb.Value.ToString()) > value)
                    this.nud_X_BypassCell_defComb.Value = value;
                this.nud_X_BypassCell_defComb.Maximum = value;
            }
        }
        
        /// <summary>
        /// 【分區數量】(Y方向) 數值更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nud_num_region_y_defComb_ValueChanged(object sender, EventArgs e)
        {
            // num_region_y 數值更新
            if (this.b_loadParams_defComb == false) // 手動更新【分區數量】
                this.Recipe_defComb.Update_num_region_y(int.Parse(this.nud_num_region_y_defComb.Value.ToString()));

            /* 更新GUI顯示參數 */
            // Y方向總Cell顆數
            this.tbx_cell_row_count_defComb.Text = this.Recipe_defComb.cell_row_count.ToString();
            // 各分區資訊 (Y方向)
            this.cbx_region_y_defComb.Items.Clear();
            for (int i = 0; i < this.Recipe_defComb.num_region_y; i++)
                this.cbx_region_y_defComb.Items.Add((i + 1).ToString());
            if (this.cbx_region_y_defComb.Items.Count > 0)
                this.cbx_region_y_defComb.SelectedIndex = 0;
        }

        /// <summary>
        /// 區域選擇切換 (Y方向)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbx_region_y_defComb_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = this.cbx_region_y_defComb.SelectedIndex;
            if (index < 0)
                return;

            bool b_loadParams_defComb_orig = this.b_loadParams_defComb;
            this.b_loadParams_defComb = true;

            /* 更新GUI顯示參數 */
            this.nud_reg_cell_y_count_defComb.Value = this.Recipe_defComb.reg_cell_y_count[index]; // Cell顆數
            this.nud_dist_reg_pdy_defComb.Value = (decimal)this.Recipe_defComb.dist_reg_pdy[index]; // 與原點距離 (Pixel)

            this.b_loadParams_defComb = b_loadParams_defComb_orig;
        }

        /// <summary>
        /// Y方向總Cell顆數更新時，要更新【Bypass Cell】Y座標之最大值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbx_cell_row_count_defComb_TextChanged(object sender, EventArgs e)
        {
            // 非數字，則直接return
            int value;
            if (!(int.TryParse(this.tbx_cell_row_count_defComb.Text, out value)))
                return;

            // 更新【Bypass Cell】
            if (value > 0)
            {
                if (int.Parse(this.nud_Y_BypassCell_defComb.Value.ToString()) > value)
                    this.nud_Y_BypassCell_defComb.Value = value;
                this.nud_Y_BypassCell_defComb.Maximum = value;
            }
        }

        /// <summary>
        /// 【輸出類型】切換，更新【輸出格式】下拉式選單項目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OutputType_defComb_CheckedChanged(object sender, EventArgs e)
        {
            if (this.b_loadParams_defComb)
                return;

            if (sender != null)
            {
                RadioButton rbtn = sender as RadioButton;
                if (rbtn.Checked == false) return;
            }

            // 更新【輸出格式】下拉式選單項目
            this.cbx_OutputFormat_defComb.Items.Clear();
            if (this.rbt_OutputImg_defComb.Checked)
            {
                string[] EnumArray = Enum.GetNames(typeof(enu_DispImageFormat));
                foreach (string s in EnumArray)
                    this.cbx_OutputFormat_defComb.Items.Add("." + s);
                this.cbx_OutputFormat_defComb.SelectedIndex = (int)this.Recipe_defComb.ImageFormat;
            }
            else if (this.rbt_OutputWord_defComb.Checked)
            {
                string[] EnumArray = Enum.GetNames(typeof(enu_WordFormat));
                foreach (string s in EnumArray)
                    this.cbx_OutputFormat_defComb.Items.Add("." + s);
                this.cbx_OutputFormat_defComb.SelectedIndex = (int)this.Recipe_defComb.WordFormat;
            }
            else
            {
                this.cbx_OutputFormat_defComb.Items.Add(".pdf");
                this.cbx_OutputFormat_defComb.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 【新增】(Bypass Cell)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Add_BypassCell_defComb_Click(object sender, EventArgs e)
        {
            Point pt = new Point(int.Parse(nud_X_BypassCell_defComb.Value.ToString()), int.Parse(nud_Y_BypassCell_defComb.Value.ToString()));
            if (this.Recipe_defComb.ListPt_BypassCell.Contains(pt))
            {
                SystemSounds.Exclamation.Play();
                MessageBox.Show("已存在相同座標!", "溫馨提醒", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.Recipe_defComb.ListPt_BypassCell.Add(pt);
                this.Update_listView_BypassCell_defComb();
            }
        }

        /// <summary>
        /// 更新 listView_BypassCell_defComb (Bypass Cell)
        /// </summary>
        private void Update_listView_BypassCell_defComb()
        {
            this.listView_BypassCell_defComb.BeginUpdate();
            this.listView_BypassCell_defComb.Items.Clear();

            foreach (Point pt in this.Recipe_defComb.ListPt_BypassCell)
            {
                ListViewItem lvi = new ListViewItem(pt.X.ToString());
                lvi.SubItems.Add(pt.Y.ToString());

                this.listView_BypassCell_defComb.Items.Add(lvi);
            }

            this.listView_BypassCell_defComb.EndUpdate();
        }

        /// <summary>
        /// 【刪除】(Bypass Cell)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Remove_BypassCell_defComb_Click(object sender, EventArgs e)
        {
            if (this.listView_BypassCell_defComb.SelectedIndices.Count <= 0)
            {
                ctrl_timer1 = this.listView_BypassCell_defComb;
                BackColor_ctrl_timer1_1 = Color.AliceBlue;
                BackColor_ctrl_timer1_2 = Color.Green;
                timer1.Enabled = true;
                SystemSounds.Exclamation.Play();
                MessageBox.Show("請選擇座標", "溫馨提醒", MessageBoxButtons.OK, MessageBoxIcon.Information);
                timer1.Enabled = false;
                ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
                return;
            }

            int index = this.listView_BypassCell_defComb.SelectedIndices[0];
            this.Recipe_defComb.ListPt_BypassCell.RemoveAt(index);
            this.Update_listView_BypassCell_defComb();
        }

        /// <summary>
        /// 【清空】(Bypass Cell)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_RemoveAll_BypassCell_defComb_Click(object sender, EventArgs e)
        {
            this.Recipe_defComb.ListPt_BypassCell.Clear();
            this.Update_listView_BypassCell_defComb();
        }
        
        #endregion


        /// <summary>
        /// 【載入影像】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_LoadImg_defComb_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenImgDilg = new OpenFileDialog();
            if (OpenImgDilg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;
            this.Recipe_defComb.Path_Image = OpenImgDilg.FileName;
            this.tbx_PathImage_defComb.Text = this.Recipe_defComb.Path_Image;
            if (this.Recipe_defComb.Load_Image())
            {
                // 顯示原始影像
                if (this.rbt_OrigImg_defComb.Checked)
                    this.Display_defComb_CheckedChanged(null, null);
                else
                    this.rbt_OrigImg_defComb.Checked = true;
                HOperatorSet.SetPart(this.hSmartWindowControl_defComb.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part
            }
        }

        /// <summary>
        /// 【計算】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Calculate_defComb_Click(object sender, EventArgs e)
        {
            this.btn_Save_defComb_Click(null, null); // 【儲存】使用者選擇之工單
            this.UI_Params_defComb(true); // 更新參數
            if (this.Recipe_defComb.Execute()) // 執行計算
            {
                // 顯示結果影像
                if (this.rbt_ResultImg_defComb.Checked)
                    this.Display_defComb_CheckedChanged(null, null);
                else
                    this.rbt_ResultImg_defComb.Checked = true;
            }
        }
        
        /// <summary>
        /// 【儲存】(根據輸出類型及格式)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SaveResult_defComb_Click(object sender, EventArgs e)
        {
            this.UI_Params_defComb(true); // 更新參數
            this.Recipe_defComb.Save_Result();
        }

        /// <summary>
        /// 改變顯示模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Display_defComb_CheckedChanged(object sender, EventArgs e)
        {
            if (this.b_loadParams_defComb)
                return;

            if (sender is RadioButton)
            {
                RadioButton rbtn = sender as RadioButton;
                if (rbtn.Checked == false) return;
            }

            this.Recipe_defComb.Display(this.hSmartWindowControl_defComb, this.cbx_dispCell_defComb, this.rbt_OrigImg_defComb, this.rbt_ResultImg_defComb);
        }
        
        #endregion



    }
}
