using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using HalconDotNet;
using System.Media; // For SystemSounds
using System.IO;
using DeltaSubstrateInspector.src.Modules.ResultModule;
using static DeltaSubstrateInspector.FileSystem.FileSystem;
using DeltaSubstrateInspector.FileSystem;
using DeltaSubstrateInspector.src.Modules.NewInspectModule.InductanceInsp;
using DeltaSubstrateInspector.src.Modules.MotionModule;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Threading;

namespace DeltaSubstrateInspector.src.PositionMethod.LocateMethod.Location
{
    public partial class CellMapForm : Form
    {
        /// <summary>
        /// 更新各種類別物件
        /// </summary>
        /// <param name="rec_A"></param>
        /// <param name="rec_B"></param>
        /// <param name="rec_AB"></param>
        public void Set_All_Recipe(LocateMethod rec_A = null, LocateMethod rec_B = null, DefectTest_AB_Recipe rec_AB = null) // (20200429) Jeff Revised!
        {
            if (rec_A != null)
                this.set_locate_method(rec_A);
            if (rec_AB != null)
                this.Set_Recipe_AB(rec_AB);
            if (rec_B != null) // Note: 順序必須在 rec_AB 之後，因為載入B面工單時會一併更新 rec_AB相關資訊
                this.set_locate_method_B(rec_B);
        }

        /// <summary>
        /// A面
        /// </summary>
        private LocateMethod locate_method_ = new LocateMethod(); // (20181116) Jeff Revised!

        /// <summary>
        /// 設定 locate_method_ (A面)
        /// </summary>
        /// <param name="method"></param>
        public void set_locate_method(LocateMethod method) // (20200429) Jeff Revised!
        {
            this.locate_method_ = method;
            this.textBox_Path_DefectTest.Text = this.locate_method_.Path_DefectTest;
        }

        /// <summary>
        /// 更新外部 locate_method_
        /// </summary>
        /// <param name="method"></param>
        public void Update_locate_method_(ref LocateMethod method) // (20200429) Jeff Revised!
        {
            method = this.locate_method_;
        }

        /// <summary>
        /// B面
        /// </summary>
        private LocateMethod locate_method_B = new LocateMethod(); // (20190817) Jeff Revised!

        /// <summary>
        /// 設定 locate_method_B (B面)
        /// </summary>
        /// <param name="method"></param>
        public void set_locate_method_B(LocateMethod method) // (20200429) Jeff Revised!
        {
            this.locate_method_B = method;
            if (this.locate_method_B.Path_DefectTest != "")
            {
                this.textBox_Path_DefectTest_B.Text = this.locate_method_B.Path_DefectTest;

                // 載入B面工單
                RunWorkerCompletedEventArgs e = new RunWorkerCompletedEventArgs("Success", null, false);
                this.bw_RunWorkerCompleted_Load_DefectTest_B(null, e);
            }
        }

        /// <summary>
        /// 更新外部 locate_method_B
        /// </summary>
        /// <param name="method"></param>
        public void Update_locate_method_B(ref LocateMethod method) // (20200429) Jeff Revised!
        {
            method = this.locate_method_B;
        }

        /// <summary>
        /// 瑕疵地圖各位置相關資訊
        /// </summary>
        public List<MapItem> ListMapItem { get; set; } = new List<MapItem>();
        
        private HTuple width, height; // (20181108) Jeff Revised!
        private HTuple allDefect_id = new HTuple(), allCellDefect_id = new HTuple(); // (20181207) Jeff Revised! 
        private HTuple allDefect_id_Recheck = new HTuple(), allCellDefect_id_Recheck = new HTuple(); // (20190716) Jeff Revised! 

        private BackgroundWorker bw_Load_DefectTest; //【載入】 (20200429) Jeff Revised!
        private BackgroundWorker bw_Load_DefectTest_B; //【載入】 (20190817) Jeff Revised!
        private BackgroundWorker bw_B_invert; //B面翻轉 (20190826) Jeff Revised!
        private BackgroundWorker bw_SaveAs_DefectTest; //【另存瑕疵檔】 (20200429) Jeff Revised!
        private BackgroundWorker bw_Load_Recipe_AB; //【載入】(【雙面統計結果合併】) (20200429) Jeff Revised!

        /// <summary>
        /// LocateMethod (B面轉到A面)
        /// </summary>
        private LocateMethod locate_method_B_2_A = null; // (20190822) Jeff Revised!

        /// <summary>
        /// Recipe (【雙面統計結果合併】)
        /// </summary>
        private DefectTest_AB_Recipe Recipe_AB = new DefectTest_AB_Recipe(); // (20190822) Jeff Revised!

        /// <summary>
        /// 設定 Recipe_AB (雙面)
        /// </summary>
        /// <param name="recipe_AB_"></param>
        public void Set_Recipe_AB(DefectTest_AB_Recipe recipe_AB_) // (20200429) Jeff Revised!
        {
            this.Recipe_AB = recipe_AB_;
        }

        /// <summary>
        /// 更新外部 Recipe_AB
        /// </summary>
        /// <param name="recipe_AB_"></param>
        public void Update_Recipe_AB(ref DefectTest_AB_Recipe recipe_AB_) // (20200429) Jeff Revised!
        {
            recipe_AB_ = this.Recipe_AB;
        }

        private List<Control> ListCtrl_Bypass { get; set; } = new List<Control>();

        #region 【AI分圖】

        /// <summary>
        /// AI分圖儲存路徑
        /// </summary>
        private string Path_AICellImage = ""; // (20190902) Jeff Revised!

        private string SBID = "";

        /// <summary>
        /// Recipe (AI分圖)
        /// </summary>
        private cls_AICellImg_Recipe AICellImg_Recipe = null; // (20190902) Jeff Revised!

        /// <summary>
        /// 是否正在載入Recipe (AICellImg_Recipe)
        /// </summary>
        private bool b_LoadRecipe_AICellImg = false;

        /// <summary>
        /// Cell中心座標(For 大圖)
        /// </summary>
        private PointF posBigMapCellCenter_AICellImg = new PointF(-1, -1);
        /// <summary>
        /// 所有走停拍位置(MoveIndex)
        /// </summary>
        private List<int> ListMoveIndex_AICellImg = new List<int>();
        /// <summary>
        /// 對應該走停拍位置之Cell region
        /// </summary>
        private List<HObject> ListCellReg_MoveIndex_AICellImg = new List<HObject>();

        /// <summary>
        /// 【視窗顯示】(AI分圖) (20191023) Jeff Revised!
        /// </summary>
        private DispWindow_AICellImg Form_DispWindow_AICellImg { get; set; } = new DispWindow_AICellImg();

        #endregion

        public CellMapForm()
        {
            InitializeComponent();
            this.hSmartWindowControl_map.MouseWheel += hSmartWindowControl_map.HSmartWindowControl_MouseWheel; // (20180825) Jeff Revised!   
            this.hSmartWindowControl_CellImage.MouseWheel += hSmartWindowControl_CellImage.HSmartWindowControl_MouseWheel; // (20190729) Jeff Revised!
            this.hSmartWindowControl_AICellImg.MouseWheel += hSmartWindowControl_AICellImg.HSmartWindowControl_MouseWheel; // (20190902) Jeff Revised!
            //this.Width = 1700; // (20190201) Jeff Revised! 
            //this.height = 1142; // (20190201) Jeff Revised!

            initBackgroundWorker(); // (20190817) Jeff Revised!

            //locate_method_.LocateMethod_Constructor(); // (20200429) Jeff Revised!
            //locate_method_B.LocateMethod_Constructor(); // (20200429) Jeff Revised!
        }

        private void initBackgroundWorker() // (20200429) Jeff Revised!
        {
            bw_Load_DefectTest = new BackgroundWorker();
            bw_Load_DefectTest.WorkerReportsProgress = false;
            bw_Load_DefectTest.WorkerSupportsCancellation = true;
            bw_Load_DefectTest.DoWork += new DoWorkEventHandler(bw_DoWork_Load_DefectTest);
            bw_Load_DefectTest.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted_Load_DefectTest);

            bw_Load_DefectTest_B = new BackgroundWorker();
            bw_Load_DefectTest_B.WorkerReportsProgress = false;
            bw_Load_DefectTest_B.WorkerSupportsCancellation = true;
            bw_Load_DefectTest_B.DoWork += new DoWorkEventHandler(bw_DoWork_Load_DefectTest_B);
            bw_Load_DefectTest_B.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted_Load_DefectTest_B);

            bw_B_invert = new BackgroundWorker();
            bw_B_invert.WorkerReportsProgress = false;
            bw_B_invert.WorkerSupportsCancellation = true;
            bw_B_invert.DoWork += new DoWorkEventHandler(bw_DoWork_B_invert);
            bw_B_invert.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted_B_invert);

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
        
        public HObject Cell_map_img_ // (20200429) Jeff Revised! 
        {
            set
            {
                this.locate_method_.TileImage = value;
            }
        }

        private void CellMapForm_Load(object sender, EventArgs e) // (20190711) Jeff Revised!
        {
            #region 結果顯示

            /* 更新參數顯示 */

            // 更新【總瑕疵】
            this.Update_groupBox_TotalDefect(locate_method_);

            // 【瑕疵類型:】
            comboBox_DefectType.Items.Clear();
            comboBox_DefectType.Text = "";
            foreach (string name in locate_method_.DefectsClassify.Keys) // (20190702) Jeff Revised!
                comboBox_DefectType.Items.Add(name);
            button_SetColor.BackColor = Color.Red;

            #endregion

            #region 人工覆判與統計結果

            if (locate_method_.b_Defect_Recheck) // 啟用人工覆判
            {
                radioButton_Recheck.Enabled = true;
                cbx_Recheck.Checked = true;
                groupBox_Recheck.Enabled = true;
            }

            if (locate_method_.b_Defect_Classify) // 瑕疵分類是否啟用
            {
                cbx_NG_classification.Checked = true;
                groupBox_DefectsClassify.Enabled = true;
            }

            cbx_Priority.Checked = locate_method_.b_Defect_priority; // 瑕疵優先權是否啟用
            
            #endregion

            #region 更新

            // 更新 hSmartWindowControl_map
            if (locate_method_.b_Defect_Recheck) // 啟用人工覆判
            {
                radioButton_Recheck.Checked = true; // (20190820) Jeff Revised!
                tabControl_BigMap.SelectedIndex = 1; // (20190820) Jeff Revised!
                HOperatorSet.SetPart(hSmartWindowControl_map.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part
            }
            else
            {
                Display(locate_method_, true); // (20190817) Jeff Revised!
            }
            Update_DispImageInfo(locate_method_.TileImage);

            // 更新 listView_Edit
            this.locate_method_.Update_listView_Edit(this.listView_Edit, this.radioButton_Result, this.radioButton_Recheck);

            // 更新 listView_Result
            this.locate_method_.Update_listView_Result(this.listView_Result, this.radioButton_Result, this.radioButton_Recheck);

            this.Initialize_comboBox_LightImage(locate_method_); // (20190730) Jeff Revised!

            #endregion

            #region 【AI分圖】

            if (FinalInspectParam.b_AICellImg_Enable && CellReg_MoveIndex_FS.Count == locate_method_.array_y_count_ * locate_method_.array_x_count_) // (20190902) Jeff Revised!
            {
                AICellImg_Recipe = new cls_AICellImg_Recipe();
                AICellImg_Recipe.Constructor(locate_method_);
                groupBox_SaveAICellImg.Enabled = true;
                groupBox_AICellImg_Info.Enabled = true;
                
                #region 初始化UI參數

                b_LoadRecipe_AICellImg = true;

                // 儲存路徑
                if (B_SB_InerCountID) // 啟用內部計數ID模式
                    SBID = SB_InerCountID;
                else
                    SBID = SB_ID;
                DateTime Time = DateTime.Now;
                Path_AICellImage = ModuleParamDirectory + ImageFileDirectory + "AI Cell Image" + "\\" + Time.ToString("yyyyMMdd") + "\\" + PartNumber + "\\" + ModuleName + "\\" + SBID + "\\";
                textBox_Path_AICellImg.Text = Path_AICellImage;

                string[] ArrayBand = Enum.GetNames(typeof(enuBand));
                comboBoxDAVSBand1.Items.Clear();
                comboBoxDAVSBand2.Items.Clear();
                comboBoxDAVSBand3.Items.Clear();
                foreach (string Band in ArrayBand)
                {
                    comboBoxDAVSBand1.Items.Add(Band);
                    comboBoxDAVSBand2.Items.Add(Band);
                    comboBoxDAVSBand3.Items.Add(Band);
                }

                nudDAVSImageId.Maximum = MovementPos.triggerCount - 1;
                nudDAVSBand1ImageIndex.Maximum = MovementPos.triggerCount - 1;
                nudDAVSBand2ImageIndex.Maximum = MovementPos.triggerCount - 1;
                nudDAVSBand3ImageIndex.Maximum = MovementPos.triggerCount - 1;

                nudDAVSImageId.Value = decimal.Parse(FinalInspectParam.DAVSImgID.ToString());
                cbxDAVSImgAlignEnabled.Checked = FinalInspectParam.b_DAVSImgAlign;
                cbxDAVSMixBandEnabled.Checked = FinalInspectParam.b_DAVSMixImgBand;
                nudDAVSBand1ImageIndex.Value = decimal.Parse(FinalInspectParam.DAVSBand1ImgIndex.ToString());
                nudDAVSBand2ImageIndex.Value = decimal.Parse(FinalInspectParam.DAVSBand2ImgIndex.ToString());
                nudDAVSBand3ImageIndex.Value = decimal.Parse(FinalInspectParam.DAVSBand3ImgIndex.ToString());
                comboBoxDAVSBand1.SelectedIndex = (int)FinalInspectParam.DAVSBand1;
                comboBoxDAVSBand2.SelectedIndex = (int)FinalInspectParam.DAVSBand2;
                comboBoxDAVSBand3.SelectedIndex = (int)FinalInspectParam.DAVSBand3;

                b_LoadRecipe_AICellImg = false;

                #endregion

                tabControl_BigMap.SelectedIndex = 4;
            }

            #endregion
        }

        /// <summary>
        /// 關閉表單前，確保所有此表單開啟之子視窗也跟著關閉
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CellMapForm_FormClosing(object sender, FormClosingEventArgs e) // (20200429) Jeff Revised!
        {
            if (this.cbx_DispWindow_AICellImg.Checked) //【視窗顯示】(AI分圖) 
            {
                try
                {
                    this.Form_DispWindow_AICellImg.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //return;
                }
            }

            if (this.cbx_DispWindow_CellImage.Checked) //【視窗顯示】(【單顆Cell影像】) 
            {
                try
                {
                    this.Form_DispWindow_CellImage.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //return;
                }
            }
        }

        /// <summary>
        /// 更新目前視窗顯示影像相關資訊
        /// </summary>
        /// <param name="img"></param>
        private void Update_DispImageInfo(HObject img) // (20200429) Jeff Revised!
        {
            Extension.HObjectMedthods.ReleaseHObject(ref DispImg);
            Extension.HObjectMedthods.ReleaseHObject(ref DispGrayImg);
            if (img != null) // (20200429) Jeff Revised!
            {
                HOperatorSet.CopyImage(img, out DispImg);
                HOperatorSet.Rgb1ToGray(DispImg, out DispGrayImg);
                HOperatorSet.GetImageSize(DispImg, out width, out height);
            }
        }

        /// <summary>
        /// 更新【總瑕疵】
        /// </summary>
        private void Update_groupBox_TotalDefect(LocateMethod locate_method, DefectTest_AB_Recipe recipe_AB = null) // (20190826) Jeff Revised!
        {
            if (recipe_AB == null)
            {
                // 瑕疵座標總數:
                if (radioButton_Result.Checked) // 顯示檢測結果
                    txt_TotalDefectCell.Text = locate_method.all_defect_id_.Count.ToString();
                else if (radioButton_Recheck.Checked) // 顯示人工覆判結果
                    txt_TotalDefectCell.Text = locate_method.all_defect_id_Recheck.Count.ToString();

                // 基板內所有未被檢測到之Cell座標 (20190817) Jeff Revised!
                allCellDefect_id = new HTuple();
                allCellDefect_id = locate_method.ListCellID_2_HTuple(locate_method.all_CellDefect_id_);

                // 瑕疵座標列表 (X, Y):
                defect_id = -1;
                comboBox_DefectCells.Items.Clear();
                comboBox_DefectCells.Text = "";
                if (radioButton_Result.Checked) // 顯示檢測結果
                {
                    allDefect_id = new HTuple();
                    foreach (Point pt in locate_method.all_defect_id_)
                    {
                        comboBox_DefectCells.Items.Add("(" + pt.X + ", " + pt.Y + ")");
                        HOperatorSet.TupleConcat(allDefect_id, locate_method.cellID_2_int(pt), out allDefect_id);
                    }
                }
                else if (radioButton_Recheck.Checked) // 顯示人工覆判結果
                {
                    allDefect_id_Recheck = new HTuple();
                    foreach (Point pt in locate_method.all_defect_id_Recheck)
                    {
                        comboBox_DefectCells.Items.Add("(" + pt.X + ", " + pt.Y + ")");
                        HOperatorSet.TupleConcat(allDefect_id_Recheck, locate_method.cellID_2_int(pt), out allDefect_id_Recheck);
                    }

                    // 【顯示所有Cell瑕疵座標】 (20190716) Jeff Revised!
                    allCellDefect_id_Recheck = new HTuple();
                    if (locate_method.DefectsClassify.ContainsKey("Cell瑕疵"))
                    {
                        allCellDefect_id_Recheck = locate_method.ListCellID_2_HTuple(locate_method.DefectsClassify["Cell瑕疵"].all_defect_id_Recheck);
                    }
                    else
                        allCellDefect_id_Recheck = allCellDefect_id.Clone();
                }
            }
            else if (recipe_AB != null) // 雙面合併 (20190826) Jeff Revised!
            {
                // 瑕疵座標總數:
                if (radioButton_Result.Checked) // 顯示檢測結果
                    txt_TotalDefectCell.Text = recipe_AB.All_AB_defect_id.Count.ToString();
                else if (radioButton_Recheck.Checked) // 顯示人工覆判結果
                    txt_TotalDefectCell.Text = recipe_AB.All_AB_defect_id_Recheck.Count.ToString();

                // 基板內所有未被檢測到之Cell座標
                allCellDefect_id = new HTuple();
                allCellDefect_id = locate_method.ListCellID_2_HTuple(recipe_AB.All_AB_CellDefect_id);

                // 瑕疵座標列表 (X, Y):
                defect_id = -1;
                comboBox_DefectCells.Items.Clear();
                comboBox_DefectCells.Text = "";
                if (radioButton_Result.Checked) // 顯示檢測結果
                {
                    allDefect_id = new HTuple();
                    foreach (Point pt in recipe_AB.All_AB_defect_id)
                    {
                        comboBox_DefectCells.Items.Add("(" + pt.X + ", " + pt.Y + ")");
                        HOperatorSet.TupleConcat(allDefect_id, locate_method.cellID_2_int(pt), out allDefect_id);
                    }
                }
                else if (radioButton_Recheck.Checked) // 顯示人工覆判結果
                {
                    allDefect_id_Recheck = new HTuple();
                    foreach (Point pt in recipe_AB.All_AB_defect_id_Recheck)
                    {
                        comboBox_DefectCells.Items.Add("(" + pt.X + ", " + pt.Y + ")");
                        HOperatorSet.TupleConcat(allDefect_id_Recheck, locate_method.cellID_2_int(pt), out allDefect_id_Recheck);
                    }

                    // 【顯示所有Cell瑕疵座標】 (20190716) Jeff Revised!
                    allCellDefect_id_Recheck = allCellDefect_id.Clone();
                }
            }
        }

        /// <summary>
        /// 更新顯示 (hSmartWindowControl_map) 【結果顯示】
        /// </summary>
        /// <param name="b_SetPart"></param>
        private void Display(LocateMethod locate_method, bool b_SetPart = false) // (20191122) Jeff Revised!
        {
            if (locate_method.TileImage == null)
                return;

            HOperatorSet.ClearWindow(hSmartWindowControl_map.HalconWindow);
            HOperatorSet.DispObj(locate_method.TileImage, hSmartWindowControl_map.HalconWindow);
            if (b_SetPart)
                HOperatorSet.SetPart(hSmartWindowControl_map.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part

            // 【顯示定位Marks中心點】
            if (checkBox_MarkCenter.Checked) // (20181226) Jeff Revised!
            {
                if (locate_method.MarkCenter_BigMap_orig != null)
                {
                    HOperatorSet.SetDraw(hSmartWindowControl_map.HalconWindow, "fill");
                    HOperatorSet.SetColor(hSmartWindowControl_map.HalconWindow, "blue");
                    HOperatorSet.DispObj(locate_method.MarkCenter_BigMap_orig, hSmartWindowControl_map.HalconWindow);
                }
            }

            // 【顯示每顆Cell區域】
            if (checkBox_cell.Checked)
            {
                if (locate_method.cellmap_affine_ != null)
                {
                    HOperatorSet.SetDraw(hSmartWindowControl_map.HalconWindow, "margin");
                    HOperatorSet.SetColor(hSmartWindowControl_map.HalconWindow, "green");
                    HOperatorSet.DispObj(locate_method.cellmap_affine_, hSmartWindowControl_map.HalconWindow);
                }
            }

            // 【顯示所有檢測到之Cell中心】
            if (checkBox_AllInspCell.Checked) // (20191122) Jeff Revised!
            {
                if (locate_method.all_intersection_Cell_ != null)
                {
                    HOperatorSet.SetDraw(hSmartWindowControl_map.HalconWindow, "fill");
                    HOperatorSet.SetColor(hSmartWindowControl_map.HalconWindow, "red");
                    HOperatorSet.DispObj(locate_method.all_intersection_Cell_, hSmartWindowControl_map.HalconWindow);
                }
            }

            // 【顯示瑕疵區域】
            if (checkBox_defects.Checked) // (20190711) Jeff Revised!
            {
                if (radioButton_fill_Result.Checked) // 填滿
                    HOperatorSet.SetDraw(hSmartWindowControl_map.HalconWindow, "fill");
                else // 邊緣
                    HOperatorSet.SetDraw(hSmartWindowControl_map.HalconWindow, "margin");

                if (!locate_method.b_Defect_Classify) // 單一瑕疵
                {
                    HOperatorSet.SetColor(hSmartWindowControl_map.HalconWindow, "red");
                    if (radioButton_Result.Checked) // 顯示檢測結果
                        HOperatorSet.DispObj(locate_method.all_intersection_defect_, hSmartWindowControl_map.HalconWindow);
                    else if (radioButton_Recheck.Checked) // 顯示人工覆判結果
                        HOperatorSet.DispObj(locate_method.all_intersection_defect_Recheck, hSmartWindowControl_map.HalconWindow);
                }
                else // 多瑕疵
                {
                    if (comboBox_DefectType.SelectedIndex >= 0)
                    {
                        string name = comboBox_DefectType.Text;
                        HOperatorSet.SetColor(hSmartWindowControl_map.HalconWindow, locate_method.DefectsClassify[name].Str_Color_Halcon);
                        if (radioButton_Result.Checked) // 顯示檢測結果
                        {
                            if (locate_method.b_Defect_priority)
                                HOperatorSet.DispObj(locate_method.DefectsClassify[name].all_intersection_defect_Priority, hSmartWindowControl_map.HalconWindow);
                            else
                                HOperatorSet.DispObj(locate_method.DefectsClassify[name].all_intersection_defect_Origin, hSmartWindowControl_map.HalconWindow);
                        }
                        else if (radioButton_Recheck.Checked) // 顯示人工覆判結果
                            HOperatorSet.DispObj(locate_method.DefectsClassify[name].all_intersection_defect_Recheck, hSmartWindowControl_map.HalconWindow);
                    }
                }
            }

            // 【顯示瑕疵座標】
            if (checkBox_DefectCell.Checked) // (20190711) Jeff Revised!
            {
                if (!locate_method.b_Defect_Classify) // 單一瑕疵
                {
                    //【顯示所有瑕疵座標】
                    this.checkBox_AllDefectCell.CheckedChanged -= new System.EventHandler(this.checkBox_CheckedChanged);
                    checkBox_AllDefectCell.Checked = true;
                    this.checkBox_AllDefectCell.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
                }
                else // 多瑕疵
                {
                    if (comboBox_DefectType.SelectedIndex >= 0)
                    {
                        string name = comboBox_DefectType.Text;
                        HOperatorSet.SetDraw(hSmartWindowControl_map.HalconWindow, "fill");
                        HOperatorSet.SetColor(hSmartWindowControl_map.HalconWindow, locate_method.DefectsClassify[name].Str_Color_Halcon);

                        List<Point> ListCellID = new List<Point>();
                        if (radioButton_Result.Checked) // 顯示檢測結果
                        {
                            if (locate_method.b_Defect_priority)
                                ListCellID = locate_method.DefectsClassify[name].all_defect_id_Priority;
                            else
                                ListCellID = locate_method.DefectsClassify[name].all_defect_id_Origin;
                        }
                        else if (radioButton_Recheck.Checked) // 顯示人工覆判結果
                            ListCellID = locate_method.DefectsClassify[name].all_defect_id_Recheck;

                        HObject reg_DefectCell = locate_method.Compute_CellReg_ListCellID(ListCellID);
                        HOperatorSet.DispObj(reg_DefectCell, hSmartWindowControl_map.HalconWindow);
                        reg_DefectCell.Dispose();
                    }
                }
            }

            // 【顯示背景瑕疵區域】
            if (checkBox_backDefect.Checked) // (20181115) Jeff Revised!
            {
                if (locate_method.all_BackDefect_ != null)
                {
                    if (radioButton_fill_Result.Checked) // 填滿
                        HOperatorSet.SetDraw(hSmartWindowControl_map.HalconWindow, "fill");
                    else // 邊緣
                        HOperatorSet.SetDraw(hSmartWindowControl_map.HalconWindow, "margin");
                    HOperatorSet.SetColor(hSmartWindowControl_map.HalconWindow, "magenta");
                    HOperatorSet.DispObj(locate_method.all_BackDefect_, hSmartWindowControl_map.HalconWindow);
                }
            }

            // 【顯示各位置取像FOV】
            if (checkBox_capture_map.Checked)
            {
                if (locate_method.capture_map_ != null)
                {
                    HOperatorSet.SetDraw(hSmartWindowControl_map.HalconWindow, "margin");
                    HOperatorSet.SetColored(hSmartWindowControl_map.HalconWindow, 12);
                    HOperatorSet.DispObj(locate_method.capture_map_, hSmartWindowControl_map.HalconWindow);
                }
            }

            // 【顯示所有瑕疵座標】
            if (checkBox_AllDefectCell.Checked) // (20190711) Jeff Revised!
            {
                if (locate_method.cellmap_affine_ != null)
                {
                    HTuple index = new HTuple();
                    if (radioButton_Result.Checked) // 顯示檢測結果
                        index = allDefect_id;
                    else if (radioButton_Recheck.Checked) // 顯示人工覆判結果
                        index = allDefect_id_Recheck;

                    HObject reg_All_DefectCell;
                    HOperatorSet.SelectObj(locate_method.cellmap_affine_, out reg_All_DefectCell, index);
                    HOperatorSet.SetDraw(hSmartWindowControl_map.HalconWindow, "fill");
                    HOperatorSet.SetColor(hSmartWindowControl_map.HalconWindow, "#ff000040"); // 25% alpha red
                    HOperatorSet.DispObj(reg_All_DefectCell, hSmartWindowControl_map.HalconWindow);
                    reg_All_DefectCell.Dispose();
                }
            }

            // 【顯示所有Cell瑕疵座標】
            if (checkBox_AllCellCenterDefect.Checked) // (20190716) Jeff Revised!
            {
                if (locate_method.cellmap_affine_ != null)
                {
                    HTuple index = new HTuple();
                    if (radioButton_Result.Checked) // 顯示檢測結果
                        index = allCellDefect_id;
                    else if (radioButton_Recheck.Checked) // 顯示人工覆判結果
                        index = allCellDefect_id_Recheck;

                    HObject reg;
                    HOperatorSet.SelectObj(locate_method.cellmap_affine_, out reg, index);
                    HOperatorSet.SetDraw(hSmartWindowControl_map.HalconWindow, "fill");
                    HOperatorSet.SetColor(hSmartWindowControl_map.HalconWindow, "#ff4500c0"); // 75% alpha orange red
                    HOperatorSet.DispObj(reg, hSmartWindowControl_map.HalconWindow);
                    reg.Dispose(); // (20190422) Jeff Revised!
                }
            }

            // 【顯示單顆瑕疵座標】
            if (checkBox_1DefectCell.Checked) // (20181116) Jeff Revised!
            {
                if (locate_method.cellmap_affine_ != null)
                {
                    if (defect_id != -1)
                    {
                        HObject reg;
                        HOperatorSet.SelectObj(locate_method.cellmap_affine_, out reg, defect_id);
                        HOperatorSet.SetDraw(hSmartWindowControl_map.HalconWindow, "fill");
                        HOperatorSet.SetColor(hSmartWindowControl_map.HalconWindow, "#ffd700c0"); // 75% alpha gold
                        HOperatorSet.DispObj(reg, hSmartWindowControl_map.HalconWindow);
                        reg.Dispose(); // (20190422) Jeff Revised!
                    }
                }
            }

            // 【滑鼠點擊右鍵Cell座標 (X, Y):】
            if (cell_id.Length > 0) // (20190706) Jeff Revised!
            {
                if (locate_method.cellmap_affine_ != null)
                {
                    try // (20190817) Jeff Revised!
                    {
                        HObject reg;
                        HOperatorSet.SelectObj(locate_method.cellmap_affine_, out reg, cell_id);
                        HOperatorSet.SetDraw(hSmartWindowControl_map.HalconWindow, "fill");
                        HOperatorSet.SetColor(hSmartWindowControl_map.HalconWindow, "#00ff0080"); // 50% alpha green
                        HOperatorSet.DispObj(reg, hSmartWindowControl_map.HalconWindow);
                        reg.Dispose(); // (20190422) Jeff Revised!
                    }
                    catch (Exception ex)
                    { }
                }
            }

        }

        /// <summary>
        /// 適用於: 【顯示資訊】 切換, 【結果顯示】,【人工覆判與統計結果】與【B面資訊】 切換
        /// </summary>
        private void Update_radioButton_defTest_tabControl() // (20190817) Jeff Revised!
        {
            if (tabControl_BigMap.SelectedTab.Text == "結果顯示")
            {
                // 更新【總瑕疵】
                Update_groupBox_TotalDefect(locate_method_);

                // 更新 hSmartWindowControl_map
                Display(locate_method_); // (20190817) Jeff Revised!

            }
            else if (tabControl_BigMap.SelectedTab.Text == "人工覆判與統計結果")
            {
                // 更新【總瑕疵】
                Update_groupBox_TotalDefect(locate_method_);

                // 更新 hSmartWindowControl_map
                LocateMethod.Disp_DefectTest(hSmartWindowControl_map, locate_method_, listView_Edit, radioButton_Result, radioButton_Recheck, radioButton_fill, radioButton_margin);

                // 更新 listView_Edit
                locate_method_.Update_listView_Edit(this.listView_Edit, this.radioButton_Result, this.radioButton_Recheck);

                // 更新 listView_Result
                locate_method_.Update_listView_Result(this.listView_Result, this.radioButton_Result, this.radioButton_Recheck);
            }
            else if (tabControl_BigMap.SelectedTab.Text == "B面資訊") // (20190817) Jeff Revised!
            {
                if (this.locate_method_B.TileImage == null)
                    return;

                // 更新【總瑕疵】
                Update_groupBox_TotalDefect(locate_method_B);

                // 更新 hSmartWindowControl_map
                LocateMethod.Disp_DefectTest(hSmartWindowControl_map, locate_method_B, listView_Edit_B, radioButton_Result, radioButton_Recheck, radioButton_fill_B, radioButton_margin_B);

                // 更新 listView_Edit
                locate_method_B.Update_listView_Edit(this.listView_Edit_B, this.radioButton_Result, this.radioButton_Recheck);

                // 更新 listView_Result
                locate_method_B.Update_listView_Result(this.listView_Result_B, this.radioButton_Result, this.radioButton_Recheck);
            }
            else if (tabControl_BigMap.SelectedTab.Text == "雙面統計結果合併") // (20190826) Jeff Revised!
            {
                if (this.locate_method_B.TileImage == null)
                    return;

                // 更新【總瑕疵】
                //this.Update_groupBox_TotalDefect(this.locate_method_, this.Recipe_AB); // (20200429) Jeff Revised!

                // 更新 hSmartWindowControl_map
                this.radioButton_DefDispAB_CheckedChanged(null, null);

                // 更新 listView_Result_AB
                this.Recipe_AB.Update_listView_Result_AB(this.listView_Result_AB, this.radioButton_Result, this.radioButton_Recheck); // (20200429) Jeff Revised!

                // 更新 listView_NGClassify_Statistics
                this.Recipe_AB.Update_listView_NGClassify_Statistics(this.listView_NGClassify_Statistics); // (20200429) Jeff Revised!
            }
            else if (tabControl_BigMap.SelectedTab.Text == "AI分圖") // (20190902) Jeff Revised!
            {
                if (AICellImg_Recipe == null)
                    return;

                // 更新 hSmartWindowControl_map
                Display_AICellImg(locate_method_, AICellImg_Recipe);

                // 更新 listView_AICellImg
                Update_listView_AICellImg();
            }

        }
        
        /// <summary>
        /// 【顯示資訊】 切換
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_defTest_CheckedChanged(object sender, EventArgs e) // (20190711) Jeff Revised!
        {
            RadioButton rbtn = sender as RadioButton;
            if (rbtn.Checked == false) return;

            Update_radioButton_defTest_tabControl();
        }

        /// <summary>
        /// 【結果顯示】, 【人工覆判與統計結果】, 【B面資訊】與【雙面統計結果合併】 切換
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl_BigMap_SelectedIndexChanged(object sender, EventArgs e) // (20190826) Jeff Revised!
        {
            cell_id = new HTuple();
            if (tabControl_BigMap.SelectedTab.Text == "B面資訊" && this.locate_method_B.TileImage != null)
            {
                Update_DispImageInfo(this.locate_method_B.TileImage);
                radioButton_DispB_CellImage.Checked = true;
            }
            else
            {
                Update_DispImageInfo(locate_method_.TileImage);
                if (tabControl_BigMap.SelectedTab.Text == "結果顯示" || tabControl_BigMap.SelectedTab.Text == "人工覆判與統計結果" || tabControl_BigMap.SelectedTab.Text == "AI分圖")
                    radioButton_DispA_CellImage.Checked = true;
                else if (tabControl_BigMap.SelectedTab.Text == "雙面統計結果合併")
                {
                    if (textBox_Path_DefectTest.Text != "" && textBox_Path_DefectTest_B.Text != "") // A, B面Recipe皆存在路徑
                    {
                        button_Save_DefectTest_AB.Enabled = true;
                        button_SaveAs_DefectTest_AB.Enabled = true;
                    }
                    else
                    {
                        button_Save_DefectTest_AB.Enabled = false;
                        button_SaveAs_DefectTest_AB.Enabled = false;
                    }
                }
            }

            if (tabControl_BigMap.SelectedTab.Text == "AI分圖") // (20190916) Jeff Revised!
            {
                if (FinalInspectParam.b_AICellImg_Enable && CellReg_MoveIndex_FS.Count == locate_method_.array_y_count_ * locate_method_.array_x_count_)
                    radioButton_Recheck.Enabled = true;
                else
                    radioButton_Recheck.Enabled = false;
            }
            else
            {
                if (locate_method_.b_Defect_Recheck) // 啟用人工覆判
                    radioButton_Recheck.Enabled = true;
                else
                    radioButton_Recheck.Enabled = false;
            }

            if (tabControl_BigMap.SelectedTab.Text == "AI分圖")
            {
                radioButton_Result.Text = "原始影像";
                radioButton_Recheck.Text = "AI分圖結果";

                HOperatorSet.SetSystem("clip_region", "false");
                Extension.HObjectMedthods.ReleaseHObject(ref CellReg_MoveIndex_Dila);
                HOperatorSet.DilationCircle(locate_method_.cellmap_affine_, out CellReg_MoveIndex_Dila, 5);
                HOperatorSet.SetSystem("clip_region", "true");
            }
            else
            {
                radioButton_Result.Text = "檢測結果";
                radioButton_Recheck.Text = "人工覆判結果";
            }

            this.Update_radioButton_defTest_tabControl();
            HOperatorSet.SetPart(hSmartWindowControl_map.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part
        }

        private bool b_originalImg_pressed = false; // (20181112) Jeff Revised!
        /// <summary>
        /// 【只顯示原始大圖影像】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_originalImg_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_originalImg.Checked)
            {
                b_originalImg_pressed = true;
                checkBox_MarkCenter.Checked = false; // (20181226) Jeff Revised!
                checkBox_cell.Checked = false;
                checkBox_AllInspCell.Checked = false; // (20191122) Jeff Revised!
                checkBox_defects.Checked = false;
                checkBox_DefectCell.Checked = false; // (20190711) Jeff Revised!
                checkBox_backDefect.Checked = false;
                checkBox_capture_map.Checked = false;
                checkBox_AllDefectCell.Checked = false; // (20181207) Jeff Revised!
                checkBox_AllCellCenterDefect.Checked = false; // (20181207) Jeff Revised!
                checkBox_1DefectCell.Checked = false;
                cell_id = new HTuple(); // (20181116) Jeff Revised!
                Display(locate_method_); // (20190817) Jeff Revised!
                b_originalImg_pressed = false;
            }
        }

        private void checkBox_CheckedChanged(object sender, EventArgs e) // (20181025) Jeff Revised!
        {
            if (!b_originalImg_pressed)
            {
                checkBox_originalImg.Checked = false;
                Display(locate_method_); // (20190817) Jeff Revised!
            }
        }

        /// <summary>
        /// 【填滿】與【邊緣】 切換
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_CheckedChanged(object sender, EventArgs e) // (20181115) Jeff Revised!
        {
            RadioButton rbtn = sender as RadioButton;
            if (rbtn.Checked == false) return;

            if (checkBox_defects.Checked || checkBox_backDefect.Checked)
                Display(locate_method_); // (20190817) Jeff Revised!
        }

        /// <summary>
        /// 【瑕疵類型:】選擇
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_DefectType_SelectedIndexChanged(object sender, EventArgs e) // (20190702) Jeff Revised!
        {
            if (comboBox_DefectType.SelectedIndex < 0)
                return;

            // 更新 顏色設定
            button_SetColor.BackColor = locate_method_.DefectsClassify[comboBox_DefectType.Text].GetColor();

            // 更新顯示
            if (checkBox_defects.Checked || checkBox_DefectCell.Checked)
                Display(locate_method_); // (20190817) Jeff Revised!
        }

        /// <summary>
        /// 顏色設定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_SetColor_Click(object sender, EventArgs e) // (20190711) Jeff Revised!
        {
            if (comboBox_DefectType.SelectedIndex < 0)
            {
                ctrl_timer1 = comboBox_DefectType;
                BackColor_ctrl_timer1_1 = Color.White;
                BackColor_ctrl_timer1_2 = Color.Green;
                timer1.Enabled = true;
                SystemSounds.Exclamation.Play();
                MessageBox.Show("請選擇【瑕疵類型】!", "溫馨提醒", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                timer1.Enabled = false;
                ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
                return;
            }

            using (Form_editDefect form = new Form_editDefect(locate_method_, true, comboBox_DefectType.Text, false))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    button_SetColor.BackColor = locate_method_.DefectsClassify[comboBox_DefectType.Text].GetColor();

                    // 更新 hSmartWindowControl_map
                    Display(locate_method_); // (20190817) Jeff Revised!
                }
            }
        }

        #region Timer

        private Control ctrl_timer1 = null;
        private Color BackColor_ctrl_timer1_1 = Color.Transparent, BackColor_ctrl_timer1_2 = Color.Green;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (ctrl_timer1 == null) return;

            if (ctrl_timer1.BackColor == BackColor_ctrl_timer1_1)
                ctrl_timer1.BackColor = BackColor_ctrl_timer1_2;
            else
                ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
        }

        private Control ctrl_timer2 = null;
        private Color BackColor_ctrl_timer2_1 = Color.Transparent, BackColor_ctrl_timer2_2 = Color.Green;
        private void timer2_Tick(object sender, EventArgs e) // (20200429) Jeff Revised!
        {
            if (ctrl_timer2 == null) return;

            if (ctrl_timer2.BackColor == BackColor_ctrl_timer2_1)
                ctrl_timer2.BackColor = BackColor_ctrl_timer2_2;
            else
                ctrl_timer2.BackColor = BackColor_ctrl_timer2_1;
        }

        #endregion

        private int defect_id = -1; // (20181116) Jeff Revised!
        /// <summary>
        /// 【瑕疵座標列表 (X, Y):】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_DefectCells_SelectedIndexChanged(object sender, EventArgs e) // (20190711) Jeff Revised! 
        {
            int index = comboBox_DefectCells.SelectedIndex;
            if (radioButton_Result.Checked) // 顯示檢測結果
                defect_id = locate_method_.cellID_2_int(locate_method_.all_defect_id_[index]);
            else if (radioButton_Recheck.Checked) // 顯示人工覆判結果
                defect_id = locate_method_.cellID_2_int(locate_method_.all_defect_id_Recheck[index]);

            // 【顯示單顆瑕疵座標】
            if (checkBox_1DefectCell.Checked)
                Display(locate_method_); // (20190817) Jeff Revised!
        }


        private HObject DispImg, DispGrayImg; // (20190711) Jeff Revised!
        /// <summary>
        /// 更新 【滑鼠影像座標 (x, y):】, 【[R, G, B]:】, 【灰階值:】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hSmartWindowControl_map_HMouseMove(object sender, HMouseEventArgs e) // (20190817) Jeff Revised!
        {
            //txt_CursorCoordinate.Text = "(" + String.Format("{0:#.##}", e.X) + ", " + String.Format("{0:#.##}", e.Y) + ")";
            txt_CursorCoordinate.Text = "(" + String.Format("{0:0}", e.X) + ", " + String.Format("{0:0}", e.Y) + ")"; // (20190817) Jeff Revised!
            try
            {
                if (e.X >= 0 && e.X < width && e.Y >= 0 && e.Y < height)
                {
                    HTuple grayval;
                    HOperatorSet.GetGrayval(DispImg, (int)(e.Y + 0.5), (int)(e.X + 0.5), out grayval);
                    txt_RGBValue.Text = (grayval.TupleInt()).ToString();
                    HOperatorSet.GetGrayval(DispGrayImg, (int)(e.Y + 0.5), (int)(e.X + 0.5), out grayval);
                    txt_GrayValue.Text = (grayval.TupleInt()).ToString();
                }
                else
                {
                    txt_RGBValue.Text = "";
                    txt_GrayValue.Text = "";
                }
            }
            catch
            {
                txt_RGBValue.Text = "";
                txt_GrayValue.Text = "";
            }
        }

        private HTuple cell_id = new HTuple(); // (20190706) Jeff Revised!                   
        /// <summary>
        /// 更新 【滑鼠點擊右鍵Cell座標 (X, Y):】 及 【距離量測】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hSmartWindowControl_map_HMouseDown(object sender, HMouseEventArgs e) // (20190817) Jeff Revised! 
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right) // 滑鼠點擊右鍵
            {
                if (tabControl_BigMap.SelectedTab.Text == "結果顯示" || tabControl_BigMap.SelectedTab.Text == "人工覆判與統計結果") // (20190817) Jeff Revised! 
                {
                    #region 【結果顯示】 or 【人工覆判與統計結果】

                    List<Point> ListCellID = new List<Point>();
                    if (locate_method_.pos_2_cellID((int)(e.Y + 0.5), (int)(e.X + 0.5), out cell_id, out ListCellID))
                    {
                        checkBox_originalImg.Checked = false;
                        string str = "";
                        foreach (Point pt in ListCellID)
                            str += "(" + pt.X + ", " + pt.Y + ")";
                        txt_cellID_Result.Text = str;

                        // 【單顆Cell影像】 (20190729) Jeff Revised!
                        Point_CellImage = ListCellID[0];
                        txt_CellImage_cellID.Text = "(" + Point_CellImage.X + ", " + Point_CellImage.Y + ")";
                        if (radioButton_DispA_CellImage.Checked) // 【A面】
                            Display_CellImage(locate_method_);
                        else
                            HOperatorSet.ClearWindow(hSmartWindowControl_CellImage.HalconWindow);
                    }
                    else
                    {
                        txt_cellID_Result.Text = "";
                        cell_id = new HTuple();

                        // 【單顆Cell影像】 (20190729) Jeff Revised!
                        Point_CellImage = new Point(-1, -1);
                        txt_CellImage_cellID.Text = "";
                        if (radioButton_DispA_CellImage.Checked) // 【A面】
                            Display_CellImage(locate_method_);
                        else
                            HOperatorSet.ClearWindow(hSmartWindowControl_CellImage.HalconWindow);
                    }

                    Display(locate_method_); // (20190817) Jeff Revised!

                    #endregion
                }
                else if (tabControl_BigMap.SelectedTab.Text == "B面資訊") // (20190817) Jeff Revised!
                {
                    #region 【B面資訊】

                    if (this.locate_method_B.TileImage == null)
                        return;

                    List<Point> ListCellID = new List<Point>();
                    if (locate_method_B.pos_2_cellID((int)(e.Y + 0.5), (int)(e.X + 0.5), out cell_id, out ListCellID))
                    {
                        checkBox_originalImg.Checked = false;
                        string str = "";
                        foreach (Point pt in ListCellID)
                            str += "(" + pt.X + ", " + pt.Y + ")";
                        txt_cellID_Result.Text = str;

                        // 【單顆Cell影像】
                        Point_CellImage = ListCellID[0];
                        txt_CellImage_cellID.Text = "(" + Point_CellImage.X + ", " + Point_CellImage.Y + ")";
                        if (radioButton_DispB_CellImage.Checked) // 【B面】
                            Display_CellImage(locate_method_B);
                        else
                            HOperatorSet.ClearWindow(hSmartWindowControl_CellImage.HalconWindow);
                    }
                    else
                    {
                        txt_cellID_Result.Text = "";
                        cell_id = new HTuple();

                        // 【單顆Cell影像】
                        Point_CellImage = new Point(-1, -1);
                        txt_CellImage_cellID.Text = "";
                        if (radioButton_DispB_CellImage.Checked) // 【B面】
                            Display_CellImage(locate_method_B);
                        else
                            HOperatorSet.ClearWindow(hSmartWindowControl_CellImage.HalconWindow);
                    }

                    Display(locate_method_B);

                    #endregion
                }
                else if (tabControl_BigMap.SelectedTab.Text == "雙面統計結果合併") // (20190826) Jeff Revised!
                {
                    #region 【雙面統計結果合併】

                    if (radioButton_DispB_CellImage.Checked && this.locate_method_B.TileImage == null)
                        return;

                    List<Point> ListCellID = new List<Point>();
                    if (locate_method_.pos_2_cellID((int)(e.Y + 0.5), (int)(e.X + 0.5), out cell_id, out ListCellID))
                    {
                        checkBox_originalImg.Checked = false;
                        string str = "";
                        foreach (Point pt in ListCellID)
                            str += "(" + pt.X + ", " + pt.Y + ")";
                        txt_cellID_Result.Text = str;

                        // 【單顆Cell影像】
                        Point_CellImage = ListCellID[0];
                        txt_CellImage_cellID.Text = "(" + Point_CellImage.X + ", " + Point_CellImage.Y + ")";
                        this.Recipe_AB.Convert_coordinate_A_2_B(Point_CellImage, out Point_CellImage_B);
                        if (radioButton_DispA_CellImage.Checked) // 【A面】
                            Display_CellImage(locate_method_);
                        else // 【B面】
                            Display_CellImage(locate_method_B);
                    }
                    else
                    {
                        txt_cellID_Result.Text = "";
                        cell_id = new HTuple();

                        // 【單顆Cell影像】
                        Point_CellImage = new Point(-1, -1);
                        Point_CellImage_B = new Point(-1, -1);
                        txt_CellImage_cellID.Text = "";
                        HOperatorSet.ClearWindow(hSmartWindowControl_CellImage.HalconWindow);
                    }

                    if (radioButton_DispA_CellImage.Checked) // 【A面】
                        Display(locate_method_);
                    else // 【B面】
                        Display(locate_method_B_2_A);

                    #endregion
                }
                else if (tabControl_BigMap.SelectedTab.Text == "AI分圖") // (20190902) Jeff Revised!
                {
                    #region 【AI分圖】

                    if (AICellImg_Recipe == null)
                        return;

                    List<Point> ListCellID = new List<Point>();
                    if (locate_method_.pos_2_cellID((int)(e.Y + 0.5), (int)(e.X + 0.5), out cell_id, out ListCellID))
                    {
                        Point_CellImage = ListCellID[0];
                        txt_cellID_AICellImg.Text = "(" + Point_CellImage.X + ", " + Point_CellImage.Y + ")";

                        // 【單顆Cell影像】
                        txt_CellImage_cellID.Text = "(" + Point_CellImage.X + ", " + Point_CellImage.Y + ")";
                        if (radioButton_DispA_CellImage.Checked) // 【A面】
                            Display_CellImage(locate_method_);
                        else
                            HOperatorSet.ClearWindow(hSmartWindowControl_CellImage.HalconWindow);

                        // 更新顯示 (hSmartWindowControl_AICellImg) 【AI分圖】
                        HObject cell;
                        HOperatorSet.SelectObj(this.locate_method_.cellmap_affine_, out cell, cell_id[0]);
                        HTuple x = new HTuple(), y = new HTuple();
                        HOperatorSet.RegionFeatures(cell, "column", out x);
                        HOperatorSet.RegionFeatures(cell, "row", out y);
                        cell.Dispose();
                        posBigMapCellCenter_AICellImg = new PointF((float)(x.D), (float)(y.D));
                        clsStaticTool.DisposeAll(this.ListCellReg_MoveIndex_AICellImg);
                        this.ListCellReg_MoveIndex_AICellImg.Clear();
                        this.locate_method_.posBigMapCellCenter_2_MoveIndex(posBigMapCellCenter_AICellImg, out this.ListMoveIndex_AICellImg, out this.ListCellReg_MoveIndex_AICellImg);
                        b_LoadRecipe_AICellImg = true;
                        comboBox_MoveIndex.Items.Clear();
                        foreach (int i in ListMoveIndex_AICellImg)
                            comboBox_MoveIndex.Items.Add(i);
                        if (comboBox_MoveIndex.Items.Count > 0) // (20191214) Jeff Revised!
                            comboBox_MoveIndex.SelectedIndex = 0;
                        b_LoadRecipe_AICellImg = false;
                        this.Display_AICellImg_1Cell();
                    }
                    else
                    {
                        cell_id = new HTuple();
                        txt_cellID_AICellImg.Text = "";
                        // 【單顆Cell影像】
                        Point_CellImage = new Point(-1, -1);
                        txt_CellImage_cellID.Text = "";
                        if (radioButton_DispA_CellImage.Checked) // 【A面】
                            Display_CellImage(locate_method_);
                        else
                            HOperatorSet.ClearWindow(hSmartWindowControl_CellImage.HalconWindow);
                        
                    }

                    // 更新 hSmartWindowControl_map
                    Display_AICellImg(locate_method_, AICellImg_Recipe);

                    // 顯示上下文選單
                    if (txt_cellID_AICellImg.Text != "")
                    {
                        toolStripMenuItem_ID.Text = txt_cellID_AICellImg.Text;
                        HTuple rowWindow, columnWindow;
                        HOperatorSet.ConvertCoordinatesImageToWindow(hSmartWindowControl_map.HalconWindow, e.Y, e.X, out rowWindow, out columnWindow);
                        contextMenuStrip_DispID.Show(hSmartWindowControl_map, (int)(columnWindow.D + 0.5), (int)(rowWindow.D + 0.5));
                    }

                    #endregion
                }
            }
            else
                cell_id = new HTuple();

            #region 190131, Andy: Distance measure

            if (checkBox_Tool_DistanceMeasure.Checked == false) return;

            Display(locate_method_); // (20190817) Jeff Revised!
            HOperatorSet.GenEmptyObj(out ho_Line);
            HOperatorSet.GenEmptyObj(out hoPoint1);
            HOperatorSet.GenEmptyObj(out hoPoint2);

            if (Point1_Set == true)
            {

                point1.X = (int)(e.X + 0.5); // (20190817) Jeff Revised! 
                point1.Y = (int)(e.Y + 0.5);

                //label_Point1.Text = "X: " + point1.X.ToString() + " , Y: " + point1.Y.ToString();

                HOperatorSet.GenCrossContourXld(out hoPoint1, point1.Y, point1.X, 10, 0);
                HOperatorSet.SetColor(hSmartWindowControl_map.HalconWindow, "blue");
                HOperatorSet.DispObj(hoPoint1, hSmartWindowControl_map.HalconWindow);

            }

            if (Point2_Set == true)
            {

                point2.X = (int)(e.X + 0.5); // (20190817) Jeff Revised! 
                point2.Y = (int)(e.Y + 0.5);

                //label_Point2.Text = "X: " + point2.X.ToString() + " , Y: " + point2.Y.ToString();

                double ShiftX = (point2.X - point1.X) / locate_method_.resize_;
                double ShiftY = (point2.Y - point1.Y) / locate_method_.resize_;
                label_Shift.Text = "X 偏移: " + ShiftX.ToString("#0") + " (pixel), " + (ShiftX * locate_method_.pixel_resolution_).ToString("#0.00") + " (μm)" +
                                 "\nY 偏移: " + ShiftY.ToString("#0") + " (pixel), " + (ShiftY * locate_method_.pixel_resolution_).ToString("#0.00") + " (μm)"; // (20190705) Jeff Revised!

                HTuple hv_Row1 = null, hv_Column1 = null, hv_Row2 = null, hv_Column2 = null;
                hv_Row1 = point1.Y;
                hv_Column1 = point1.X;
                hv_Row2 = point2.Y;
                hv_Column2 = point2.X;

                HOperatorSet.GenCrossContourXld(out hoPoint1, point1.Y, point1.X, 10, 0);
                HOperatorSet.GenCrossContourXld(out hoPoint2, point2.Y, point2.X, 10, 0);
                HOperatorSet.GenContourPolygonXld(out ho_Line, hv_Row1.TupleConcat(hv_Row2), hv_Column1.TupleConcat(hv_Column2));

                // Display
                HOperatorSet.SetColor(hSmartWindowControl_map.HalconWindow, "blue");
                HOperatorSet.DispObj(hoPoint1, hSmartWindowControl_map.HalconWindow);
                HOperatorSet.SetColor(hSmartWindowControl_map.HalconWindow, "red");
                HOperatorSet.DispObj(hoPoint2, hSmartWindowControl_map.HalconWindow);
                HOperatorSet.SetColor(hSmartWindowControl_map.HalconWindow, "green");
                HOperatorSet.DispObj(ho_Line, hSmartWindowControl_map.HalconWindow);

                /*
                HTuple hv_ShiftX_Str, hv_ShiftY_Str;
                HOperatorSet.TupleString(ShiftX, "10.1f", out hv_ShiftX_Str);
                HOperatorSet.TupleString(ShiftY, "10.1f", out hv_ShiftY_Str);
                HTuple hv_Row1SW = null, hv_Column1SW = null;
                hv_Row1SW = point1.Y + ((ShiftY > 0) ? (-1) : (1)) * 5;
                hv_Column1SW = point1.X + ((ShiftX > 0) ? (-1) : (1)) * 5;s
                disp_message(hSmartWindowControl_map.HalconWindow, "ShiftX:" + hv_ShiftX_Str, "window", hv_Row1SW+2, hv_Column1SW+2, "black", "true");
                disp_message(hSmartWindowControl_map.HalconWindow, "ShiftY:" + hv_ShiftY_Str, "window", hv_Row1SW+22, hv_Column1SW+2, "black", "true");
                */

            }

            #endregion

        }

        #region 190131, Andy: Distance measure

        HObject ho_Line = null;
        HObject hoPoint1 = null;
        HObject hoPoint2 = null;
        Point point1 = new Point();
        Point point2 = new Point();

        HTuple hv_WindowHandle = null;
        private void checkBox_Tool_DistanceMeasure_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Tool_DistanceMeasure.Checked)
            {
                hSmartWindowControl_map.HMoveContent = false;
                hSmartWindowControl_map.Cursor = Cursors.AppStarting;
            }
            else
            {
                hSmartWindowControl_map.HMoveContent = true;
                hSmartWindowControl_map.Cursor = Cursors.Default;

                Point1_Set = false;
                Point2_Set = false;
                button_ToolDistanceMeasure_Point1.ForeColor = Color.Black;
                button_ToolDistanceMeasure_Point2.ForeColor = Color.Black;
                button_ToolDistanceMeasure_Point2.Text = "第二點";
                button_ToolDistanceMeasure_Point1.Text = "第一點";

                Display(locate_method_); // (20190817) Jeff Revised!
            }

        }

        private void Tool_DistanceMeasure()
        {


            // Local iconic variables 

            HObject ho_Laser01, ho_Line;

            // Local control variables 

            HTuple hv_Row1 = null, hv_Column1 = null, hv_Row2 = null, hv_Column2 = null;
            HTuple hv_ShiftX = null, hv_ShiftY = null;
            HTuple hv_ShiftX_Str = null, hv_ShiftY_Str = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Laser01);
            HOperatorSet.GenEmptyObj(out ho_Line);
            ho_Laser01.Dispose();
            //HOperatorSet.ReadImage(out ho_Laser01, "Laser01.tif");       

            int w = hSmartWindowControl_map.HImagePart.Width;
            int h = hSmartWindowControl_map.HImagePart.Height;
            HTuple hv_Row1SW = null, hv_Column1SW = null, hv_Row2SW = null, hv_Column2SW = null;
            HOperatorSet.GetPart(hSmartWindowControl_map.HalconWindow, out hv_Row1SW, out hv_Column1SW, out hv_Row2SW, out hv_Column2SW);
            HOperatorSet.SetPart(hv_WindowHandle, hv_Row1SW, hv_Column1SW, hv_Row2SW, hv_Column2SW);

            HOperatorSet.DispObj(this.locate_method_.TileImage, hv_WindowHandle);

            HOperatorSet.SetColor(hv_WindowHandle, "red");
            HOperatorSet.DrawLine(hv_WindowHandle, out hv_Row1, out hv_Column1, out hv_Row2, out hv_Column2);
            ho_Line.Dispose();


            HOperatorSet.GenContourPolygonXld(out ho_Line, hv_Row1.TupleConcat(hv_Row2), hv_Column1.TupleConcat(hv_Column2));
            hv_ShiftX = hv_Column2 - hv_Column1;
            hv_ShiftY = hv_Row2 - hv_Row1;

            if (HDevWindowStack.IsOpen())
            {
                //HOperatorSet.ClearWindow(HDevWindowStack.GetActive());
            }
            if (HDevWindowStack.IsOpen())
            {
                //HOperatorSet.DispObj(ho_Laser01, HDevWindowStack.GetActive());
            }
            if (HDevWindowStack.IsOpen())
            {
                HOperatorSet.DispObj(ho_Line, HDevWindowStack.GetActive());
            }
            HOperatorSet.TupleString(hv_ShiftX, "10.1f", out hv_ShiftX_Str);
            HOperatorSet.TupleString(hv_ShiftY, "10.1f", out hv_ShiftY_Str);
            disp_message(hv_WindowHandle, "ShiftX:" + hv_ShiftX_Str + " (pixel), " + hv_ShiftX * locate_method_.pixel_resolution_ + " (μm)", "window", 2, 2, "black", "true"); // (20190201) Jeff Revised!
            disp_message(hv_WindowHandle, "ShiftY:" + hv_ShiftY_Str, "window", 22, 2, "black", "true");


            ho_Laser01.Dispose();
            ho_Line.Dispose();

        }

        public void disp_message(HTuple hv_WindowHandle, HTuple hv_String, HTuple hv_CoordSystem, HTuple hv_Row, HTuple hv_Column, HTuple hv_Color, HTuple hv_Box)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_GenParamName = null, hv_GenParamValue = null;
            HTuple hv_Color_COPY_INP_TMP = hv_Color.Clone();
            HTuple hv_Column_COPY_INP_TMP = hv_Column.Clone();
            HTuple hv_CoordSystem_COPY_INP_TMP = hv_CoordSystem.Clone();
            HTuple hv_Row_COPY_INP_TMP = hv_Row.Clone();

            // Initialize local and output iconic variables 
            //This procedure displays text in a graphics window.
            //
            //Input parameters:
            //WindowHandle: The WindowHandle of the graphics window, where
            //   the message should be displayed
            //String: A tuple of strings containing the text message to be displayed
            //CoordSystem: If set to 'window', the text position is given
            //   with respect to the window coordinate system.
            //   If set to 'image', image coordinates are used.
            //   (This may be useful in zoomed images.)
            //Row: The row coordinate of the desired text position
            //   A tuple of values is allowed to display text at different
            //   positions.
            //Column: The column coordinate of the desired text position
            //   A tuple of values is allowed to display text at different
            //   positions.
            //Color: defines the color of the text as string.
            //   If set to [], '' or 'auto' the currently set color is used.
            //   If a tuple of strings is passed, the colors are used cyclically...
            //   - if |Row| == |Column| == 1: for each new textline
            //   = else for each text position.
            //Box: If Box[0] is set to 'true', the text is written within an orange box.
            //     If set to' false', no box is displayed.
            //     If set to a color string (e.g. 'white', '#FF00CC', etc.),
            //       the text is written in a box of that color.
            //     An optional second value for Box (Box[1]) controls if a shadow is displayed:
            //       'true' -> display a shadow in a default color
            //       'false' -> display no shadow
            //       otherwise -> use given string as color string for the shadow color
            //
            //It is possible to display multiple text strings in a single call.
            //In this case, some restrictions apply:
            //- Multiple text positions can be defined by specifying a tuple
            //  with multiple Row and/or Column coordinates, i.e.:
            //  - |Row| == n, |Column| == n
            //  - |Row| == n, |Column| == 1
            //  - |Row| == 1, |Column| == n
            //- If |Row| == |Column| == 1,
            //  each element of String is display in a new textline.
            //- If multiple positions or specified, the number of Strings
            //  must match the number of positions, i.e.:
            //  - Either |String| == n (each string is displayed at the
            //                          corresponding position),
            //  - or     |String| == 1 (The string is displayed n times).
            //
            //
            //Convert the parameters for disp_text.
            if ((int)((new HTuple(hv_Row_COPY_INP_TMP.TupleEqual(new HTuple()))).TupleOr(
                new HTuple(hv_Column_COPY_INP_TMP.TupleEqual(new HTuple())))) != 0)
            {

                return;
            }
            if ((int)(new HTuple(hv_Row_COPY_INP_TMP.TupleEqual(-1))) != 0)
            {
                hv_Row_COPY_INP_TMP = 12;
            }
            if ((int)(new HTuple(hv_Column_COPY_INP_TMP.TupleEqual(-1))) != 0)
            {
                hv_Column_COPY_INP_TMP = 12;
            }
            //
            //Convert the parameter Box to generic parameters.
            hv_GenParamName = new HTuple();
            hv_GenParamValue = new HTuple();
            if ((int)(new HTuple((new HTuple(hv_Box.TupleLength())).TupleGreater(0))) != 0)
            {
                if ((int)(new HTuple(((hv_Box.TupleSelect(0))).TupleEqual("false"))) != 0)
                {
                    //Display no box
                    hv_GenParamName = hv_GenParamName.TupleConcat("box");
                    hv_GenParamValue = hv_GenParamValue.TupleConcat("false");
                }
                else if ((int)(new HTuple(((hv_Box.TupleSelect(0))).TupleNotEqual("true"))) != 0)
                {
                    //Set a color other than the default.
                    hv_GenParamName = hv_GenParamName.TupleConcat("box_color");
                    hv_GenParamValue = hv_GenParamValue.TupleConcat(hv_Box.TupleSelect(0));
                }
            }
            if ((int)(new HTuple((new HTuple(hv_Box.TupleLength())).TupleGreater(1))) != 0)
            {
                if ((int)(new HTuple(((hv_Box.TupleSelect(1))).TupleEqual("false"))) != 0)
                {
                    //Display no shadow.
                    hv_GenParamName = hv_GenParamName.TupleConcat("shadow");
                    hv_GenParamValue = hv_GenParamValue.TupleConcat("false");
                }
                else if ((int)(new HTuple(((hv_Box.TupleSelect(1))).TupleNotEqual("true"))) != 0)
                {
                    //Set a shadow color other than the default.
                    hv_GenParamName = hv_GenParamName.TupleConcat("shadow_color");
                    hv_GenParamValue = hv_GenParamValue.TupleConcat(hv_Box.TupleSelect(1));
                }
            }
            //Restore default CoordSystem behavior.
            if ((int)(new HTuple(hv_CoordSystem_COPY_INP_TMP.TupleNotEqual("window"))) != 0)
            {
                hv_CoordSystem_COPY_INP_TMP = "image";
            }
            //
            if ((int)(new HTuple(hv_Color_COPY_INP_TMP.TupleEqual(""))) != 0)
            {
                //disp_text does not accept an empty string for Color.
                hv_Color_COPY_INP_TMP = new HTuple();
            }
            //
            HOperatorSet.DispText(hv_WindowHandle, hv_String, hv_CoordSystem_COPY_INP_TMP,
                hv_Row_COPY_INP_TMP, hv_Column_COPY_INP_TMP, hv_Color_COPY_INP_TMP, hv_GenParamName,
                hv_GenParamValue);

            return;
        }

        private bool Point1_Set = false;
        private bool Point2_Set = false;
        private void button_ToolDistanceMeasure_Point1_Click(object sender, EventArgs e)
        {
            if (checkBox_Tool_DistanceMeasure.Checked == false) return;

            if (Point1_Set == false)
            {
                Point1_Set = true;
                Point2_Set = false;

                button_ToolDistanceMeasure_Point1.Text = "第一點 (作用中)";
                button_ToolDistanceMeasure_Point1.ForeColor = Color.Blue;
                button_ToolDistanceMeasure_Point2.Text = "第二點";
                button_ToolDistanceMeasure_Point2.ForeColor = Color.Black;
            }

        }

        private void button_ToolDistanceMeasure_Point2_Click(object sender, EventArgs e)
        {
            if (checkBox_Tool_DistanceMeasure.Checked == false) return;

            if (Point2_Set == false)
            {
                Point2_Set = true;
                Point1_Set = false;

                button_ToolDistanceMeasure_Point2.ForeColor = Color.Red;
                button_ToolDistanceMeasure_Point2.Text = "第二點 (作用中)";
                button_ToolDistanceMeasure_Point1.ForeColor = Color.Black;
                button_ToolDistanceMeasure_Point1.Text = "第一點";

            }

        }

        #endregion



        #region 【人工覆判與統計結果】

        /// <summary>
        /// 【儲存/另存】(A面)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Save_DefectTest_Click(object sender, EventArgs e) // (20200429) Jeff Revised!
        {
            if (e != null) // (20200429) Jeff Revised!
            {
                SystemSounds.Exclamation.Play();
                if (MessageBox.Show("確定要【儲存/另存】?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) // (20200429) Jeff Revised!
                    return;
            }

            Button bt = sender as Button; // (20200429) Jeff Revised!
            if (this.textBox_Path_DefectTest.Text == "" || bt == this.button_SaveAs_DefectTest) // (20200429) Jeff Revised!
            {
                FolderBrowserDialog Dilg = new FolderBrowserDialog();
                Dilg.Description = "【儲存/另存】(A面)"; // (20200429) Jeff Revised!
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
            // 儲存 Path_DefectTest (20200429) Jeff Revised!
            this.locate_method_.Path_DefectTest = this.textBox_Path_DefectTest.Text;

            if (this.bw_SaveAs_DefectTest.IsBusy != true)
            {
                this.tabPage_Recheck.Enabled = false;

                object[] parameters = new object[] { "A面", this.locate_method_, this.textBox_Path_DefectTest, this.tabPage_Recheck }; // (20200429) Jeff Revised!
                this.bw_SaveAs_DefectTest.RunWorkerAsync(parameters); // (20200429) Jeff Revised!

                ctrl_timer1 = bt; // (20200429) Jeff Revised!
                BackColor_ctrl_timer1_1 = Color.Blue;
                BackColor_ctrl_timer1_2 = Color.Green;
                timer1.Enabled = true;
            }
            else
                MessageBox.Show("背景執行緖忙碌中，請稍後再試", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }
        
        private void bw_DoWork_SaveAs_DefectTest(object sender, DoWorkEventArgs e) // (20200429) Jeff Revised!
        {
            BackgroundWorker Worker = (BackgroundWorker)sender;
            // 取得 input arguments (20200429) Jeff Revised!
            object[] parameters = e.Argument as object[];
            string nowRecipe_DefectTest = parameters[0].ToString();
            LocateMethod LocateMethodRecipe = null;
            DefectTest_AB_Recipe recipe_AB_ = null;
            if (nowRecipe_DefectTest != "雙面")
                LocateMethodRecipe = (LocateMethod)parameters[1];
            else
                recipe_AB_ = (DefectTest_AB_Recipe)parameters[1];
            TextBox tb = (TextBox)parameters[2];
            TabPage tp = (TabPage)parameters[3];

            // 設定 e.Result (20200429) Jeff Revised!
            e.Result = parameters;
            
            clsProgressbar m_ProgressBar = new clsProgressbar();
            m_ProgressBar.FormClosedEvent2 += new clsProgressbar.FormClosedHandler2(SetFormClosed2);

            m_ProgressBar.SetShowText("請等待" + nowRecipe_DefectTest + "【儲存/另存瑕疵檔】......");
            m_ProgressBar.SetShowCaption("執行中......");
            m_ProgressBar.ShowWaitProgress();

            string directory_ = tb.Text + "\\";
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
            if (nowRecipe_DefectTest != "雙面")
            {
                if (LocateMethodRecipe.Save_DefectTest_Recipe(directory_))
                    param.Add("Success");
                else
                    param.Add("Exception");
            }
            else
            {
                if (recipe_AB_.Save_DefectTest_AB_Recipe())
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
            // 取得 input arguments (20200429) Jeff Revised!
            object[] parameters = e.Result as object[];
            string nowRecipe_DefectTest = parameters[0].ToString();
            //LocateMethod LocateMethodRecipe = (LocateMethod)parameters[1];
            //TextBox tb = (TextBox)parameters[2];
            TabPage tp = (TabPage)parameters[3];

            if (e.Cancelled == true)
            {
                //MessageBox.Show("取消!");
            }
            else if (e.Error != null)
            {
                MessageBox.Show("Error: " + e.Error.Message);
            }
            else if (e.Result != null && parameters[4].ToString() == "Exception")
            {
                SystemSounds.Exclamation.Play();
                MessageBox.Show(nowRecipe_DefectTest + "【儲存/另存瑕疵檔】失敗!", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning); // (20200429) Jeff Revised!
                // 重建已刪除之檔案路徑
                //Directory.CreateDirectory(System.IO.Path.GetDirectoryName(tb.Text + "\\"));
            }
            else if (e.Result != null && parameters[4].ToString() == "Success")
            {
                //MessageBox.Show("【儲存/另存瑕疵檔】成功!");
            }
            else
                return;

            if (nowRecipe_DefectTest != "雙面")
            {
                timer1.Enabled = false;
                ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
            }
            else
            {
                timer2.Enabled = false;
                ctrl_timer2.BackColor = BackColor_ctrl_timer2_1;
            }
            tp.Enabled = true;
        }

        /// <summary>
        /// 【編輯顏色】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Edit_DefectsClassify_Click(object sender, EventArgs e) // (20190711) Jeff Revised!
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

            using (Form_editDefect form = new Form_editDefect(locate_method_, true, listView_Edit.SelectedItems[0].Text, false))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    // 更新 hSmartWindowControl_map
                    listView_Edit_SelectedIndexChanged(sender, e);

                    // 更新 listView_Edit
                    locate_method_.Update_listView_Edit(this.listView_Edit, this.radioButton_Result, this.radioButton_Recheck);

                    // 更新 listView_Result
                    locate_method_.Update_listView_Result(this.listView_Result, this.radioButton_Result, this.radioButton_Recheck);
                }
            }
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
            locate_method_.Disp_DefectTest(this.hSmartWindowControl_map, radioButton_Result, radioButton_Recheck, radioButton_fill, radioButton_margin, ListName, false);

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
            if (this.listView_Result.SelectedIndices.Count <= 0)
                return;

            // 同時選取 listView_Edit 中相同瑕疵名稱
            if (this.listView_Edit.SelectedIndices.Count > 0) // 消除listView_Edit目前選取狀態
                this.listView_Edit.SelectedItems[0].Selected = false;
            this.listView_Edit.Items[this.listView_Result.SelectedIndices[0]].Focused = true;
            this.listView_Edit.Items[this.listView_Result.SelectedIndices[0]].Selected = true;
            this.listView_Edit.Items[this.listView_Result.SelectedIndices[0]].EnsureVisible();
        }

        private int index_capture = 1;
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
        private void cbx_Do_Recheck_CheckedChanged(object sender, EventArgs e) // (20190711) Jeff Revised!
        {
            CheckBox Obj = (CheckBox)sender;

            if (Obj.Checked) // 人工覆判
            {
                try
                {
                    DefectName_select.Clear();
                    contextMenuStrip_Recheck.Items.Clear();
                    if (locate_method_.b_Defect_Classify && radioButton_MultiCells.Checked) // 多瑕疵 & 多顆標註模式
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
                    else if (locate_method_.b_Defect_Classify && radioButton_SingleCell.Checked) // 多瑕疵 & 單顆標註模式
                    {
                        contextMenuStrip_Recheck.Items.Add(GetMenuItem(locate_method_, "A", "OK", Color.Green));
                        foreach (string name in locate_method_.DefectsClassify.Keys)
                        {
                            DefectName_select.Add(name);
                            contextMenuStrip_Recheck.Items.Add(GetMenuItem(locate_method_, "A", name, locate_method_.DefectsClassify[name].GetColor()));
                        }
                    }
                    else // 單一瑕疵
                    {
                        DefectName_select.Add("");
                        if (radioButton_SingleCell.Checked) // 單顆標註模式
                        {
                            contextMenuStrip_Recheck.Items.Add(GetMenuItem(locate_method_, "A", "OK", Color.Green));
                            contextMenuStrip_Recheck.Items.Add(GetMenuItem(locate_method_, "A", "NG", Color.Red));
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
                        int capture_index_ = Convert.ToInt32(openFileDialog_file.SafeFileName.Substring(index + 2, 3).ToString()) - 2;

                        if (!(capture_index_ >= 1 && capture_index_ <= locate_method_.array_x_count_ * locate_method_.array_y_count_))
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
                    locate_method_.Compute_CellReg_MoveIndex(index_capture, out CellReg_MoveIndex);
                    Extension.HObjectMedthods.ReleaseHObject(ref CellReg_MoveIndex_Dila);
                    HOperatorSet.DilationCircle(CellReg_MoveIndex, out CellReg_MoveIndex_Dila, 5);
                    HOperatorSet.SetSystem("clip_region", "true");

                    /* 計算此位置瑕疵 + 計算此位置已標註Cell */
                    Update_DefectReg_cellLabelled_MoveIndex(locate_method_);

                    // 更新顯示
                    locate_method_.Disp_Recheck(hSmartWindowControl_map, Image_MoveIndex, CellReg_MoveIndex,
                                                DefectReg_MoveIndex, DefectName_select, cellLabelled_MoveIndex, true);

                    ListCtrl_Bypass = clsStaticTool.EnableAllControls_Bypass(this.tabControl_BigMap, Obj, false);
                    Obj.ForeColor = Color.GreenYellow;
                    Obj.Text = "設定完成";
                    ctrl_timer1 = Obj;
                    BackColor_ctrl_timer1_1 = Color.DarkViolet;
                    BackColor_ctrl_timer1_2 = Color.Lime;
                    timer1.Enabled = true;

                    label_cellID.Enabled = true;
                    txt_cellID.Enabled = true;
                    button_LoadCellImage.Enabled = false; // (20190730) Jeff Revised!
                    radioButton_Result.Enabled = false; // (20190817) Jeff Revised!
                    radioButton_Recheck.Enabled = false; // (20190817) Jeff Revised!

                    // 禁能滑鼠點擊事件 (原本)
                    this.hSmartWindowControl_map.HMouseDown -= new HalconDotNet.HMouseEventHandler(this.hSmartWindowControl_map_HMouseDown);
                    // 致能滑鼠點擊事件 (人工覆判)
                    this.hSmartWindowControl_map.HMouseDown += new HalconDotNet.HMouseEventHandler(this.hSmartWindowControl_map_HMouseDown_Recheck);

                    Update_DispImageInfo(Image_MoveIndex); // (20190712) Jeff Revised!
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
                // 更新 locate_method_B_2_A
                if (this.locate_method_B.Path_DefectTest != "")
                {
                    if (!this.Recipe_AB.Update_Locate_method_B_2_A_Recheck(true, this.locate_method_, this.locate_method_B)) // (20200429) Jeff Revised!
                    {
                        //return;
                    }
                }

                timer1.Enabled = false;
                ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
                clsStaticTool.EnableAllControls_Bypass(this.tabControl_BigMap, Obj, true, ListCtrl_Bypass);
                Obj.ForeColor = Color.White;
                Obj.Text = "人工覆判";

                button_LoadCellImage.Enabled = true; // (20190730) Jeff Revised!
                radioButton_Result.Enabled = true; // (20190817) Jeff Revised!
                radioButton_Recheck.Enabled = true; // (20190817) Jeff Revised!

                // 禁能滑鼠點擊事件 (人工覆判)
                this.hSmartWindowControl_map.HMouseDown -= new HalconDotNet.HMouseEventHandler(this.hSmartWindowControl_map_HMouseDown_Recheck);
                // 致能滑鼠點擊事件 (原本)
                this.hSmartWindowControl_map.HMouseDown += new HalconDotNet.HMouseEventHandler(this.hSmartWindowControl_map_HMouseDown);

                Update_DispImageInfo(locate_method_.TileImage); // (20190712) Jeff Revised!

                try
                {
                    // 更新 hSmartWindowControl_map
                    if (!locate_method_.b_Defect_Classify) // 單一瑕疵
                    {
                        locate_method_.Disp_DefectTest(this.hSmartWindowControl_map, radioButton_Result, radioButton_Recheck, radioButton_fill, radioButton_margin);
                    }
                    else // 多瑕疵
                    {
                        List<string> ListName = new List<string>();
                        foreach (string name in DefectName_select)
                            ListName.Add(name);
                        locate_method_.Disp_DefectTest(this.hSmartWindowControl_map, radioButton_Result, radioButton_Recheck, radioButton_fill, radioButton_margin, ListName);
                    }

                    // 更新 listView_Result
                    locate_method_.Update_listView_Result(this.listView_Result, this.radioButton_Result, this.radioButton_Recheck);
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
        private void Update_DefectReg_cellLabelled_MoveIndex(LocateMethod locate_method) // (20190817) Jeff Revised!
        {
            /* 計算此位置瑕疵 */
            clsStaticTool.DisposeAll(DefectReg_MoveIndex);
            DefectReg_MoveIndex.Clear();
            if (!locate_method.b_Defect_Classify) // 單一瑕疵
            {
                HObject reg;
                locate_method.Compute_DefectReg_MoveIndex(index_capture, locate_method.all_intersection_defect_Recheck, out reg);
                DefectReg_MoveIndex.Add(reg);
                //reg.Dispose();
            }
            else // 多瑕疵
            {
                foreach (string name in DefectName_select)
                {
                    HObject reg;
                    locate_method.Compute_DefectReg_MoveIndex(index_capture, locate_method.DefectsClassify[name].all_intersection_defect_Recheck, out reg);
                    DefectReg_MoveIndex.Add(reg);
                    //reg.Dispose();
                }
            }

            /* 計算此位置已標註Cell */
            Extension.HObjectMedthods.ReleaseHObject(ref cellLabelled_MoveIndex);
            locate_method.Compute_DefectReg_MoveIndex(index_capture, locate_method.all_intersection_cellLabelled_Recheck, out cellLabelled_MoveIndex);
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
            if (!locate_method_.pos_2_cellID_MoveIndex(index_capture, e.Y, e.X, out cell_ids, out ListCellID))
                return;

            if (e.Button == System.Windows.Forms.MouseButtons.Left) // 滑鼠是否點擊左鍵
            {
                #region 滑鼠點擊左鍵Cell座標 (X, Y)

                // 更新顯示
                string str = "";
                foreach (Point pt in ListCellID)
                    str += "(" + pt.X + ", " + pt.Y + ")";
                txt_cellID.Text = str;
                locate_method_.Disp_Recheck(hSmartWindowControl_map, Image_MoveIndex, CellReg_MoveIndex,
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

                // 【單顆Cell影像】 (20190729) Jeff Revised!
                Point_CellImage = ListCellID[0];
                txt_CellImage_cellID.Text = "(" + Point_CellImage.X + ", " + Point_CellImage.Y + ")";
                Display_CellImage(locate_method_);

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
                    locate_method_.Update_Recheck(null, ListCellID, DefectName_select[0]);
                else // 標註此顆Cell OK
                    locate_method_.Update_Recheck(ListCellID, null, DefectName_select[0]);

                /* 計算此位置瑕疵 + 計算此位置已標註Cell */
                Update_DefectReg_cellLabelled_MoveIndex(locate_method_);

                // 更新顯示
                locate_method_.Disp_Recheck(hSmartWindowControl_map, Image_MoveIndex, CellReg_MoveIndex,
                                            DefectReg_MoveIndex, DefectName_select, cellLabelled_MoveIndex, false);

                // 更新 listView_Result
                locate_method_.Update_listView_Result(this.listView_Result, this.radioButton_Result, this.radioButton_Recheck);

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
                if (!locate_method_.b_Defect_Classify) // 單一瑕疵
                {
                    if (locate_method_.all_defect_id_Recheck.Contains(DefectId_SingleCell[0])) // NG Cell
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
                        if (locate_method_.DefectsClassify[DefectName_select[i - 1]].all_defect_id_Recheck.Contains(DefectId_SingleCell[0])) // 包含此瑕疵類型
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
        /// <param name="locate_method"></param>
        /// <param name="str_side">A面 or B面</param>
        /// <param name="txt"></param>
        /// <param name="ForeColor"></param>
        /// <param name="img"></param>
        /// <param name="b_ClickEvent"></param>
        /// <returns></returns>
        private ToolStripMenuItem GetMenuItem(LocateMethod locate_method, string str_side, string txt, object ForeColor = null, Image img = null, bool b_ClickEvent = true) // (20190820) Jeff Revised!
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
            {
                //menuItem.Click += new EventHandler(toolStripMenuItem_Click);
                menuItem.Click += (sender, e) => toolStripMenuItem_Click(sender, locate_method, str_side);
            }

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
                locate_method_.Update_All_OK_Recheck(DefectId_SingleCell);
            else // 點擊其他瑕疵類型
            {
                if (menu.Checked) // 標註此種瑕疵
                    locate_method_.Update_Recheck(null, DefectId_SingleCell, name);
                else // 解除標註此種瑕疵
                    locate_method_.Update_Recheck(DefectId_SingleCell, null, name);
            }

            /* 計算此位置瑕疵 + 計算此位置已標註Cell */
            Update_DefectReg_cellLabelled_MoveIndex(locate_method_);

            // 更新顯示
            locate_method_.Disp_Recheck(hSmartWindowControl_map, Image_MoveIndex, CellReg_MoveIndex,
                                        DefectReg_MoveIndex, DefectName_select, cellLabelled_MoveIndex, false);

            // 更新 listView_Result
            //Update_listView_Result(this.listView_Result, this.radioButton_Result, this.radioButton_Recheck);
            locate_method_.Update_listView_Result(this.listView_Result, this.radioButton_Result, this.radioButton_Recheck);

        }

        /// <summary>
        /// 選單項事件響應 (有參數傳入)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="locate_method"></param>
        /// <param name="str_side">A面 or B面</param>
        private void toolStripMenuItem_Click(object sender, LocateMethod locate_method, string str_side = "A") // (20190820) Jeff Revised!
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
                locate_method.Update_All_OK_Recheck(DefectId_SingleCell);
            else // 點擊其他瑕疵類型
            {
                if (menu.Checked) // 標註此種瑕疵
                    locate_method.Update_Recheck(null, DefectId_SingleCell, name);
                else // 解除標註此種瑕疵
                    locate_method.Update_Recheck(DefectId_SingleCell, null, name);
            }

            /* 計算此位置瑕疵 + 計算此位置已標註Cell */
            this.Update_DefectReg_cellLabelled_MoveIndex(locate_method);

            // 更新顯示
            locate_method.Disp_Recheck(hSmartWindowControl_map, Image_MoveIndex, CellReg_MoveIndex,
                                       DefectReg_MoveIndex, DefectName_select, cellLabelled_MoveIndex, false);

            // 更新 listView_Result
            if (str_side == "A")
                locate_method.Update_listView_Result(this.listView_Result, this.radioButton_Result, this.radioButton_Recheck);
            else if (str_side == "B")
                locate_method.Update_listView_Result(this.listView_Result_B, this.radioButton_Result, this.radioButton_Recheck);
        }

        /// <summary>
        /// 【清空人工覆判】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Clear_Recheck_Click(object sender, EventArgs e)
        {
            // 新增使用者保護機制
            SystemSounds.Exclamation.Play();
            DialogResult dr = MessageBox.Show("確定要清空人工覆判 ?", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr != DialogResult.Yes)
                return;

            locate_method_.Initialize_Recheck();

            // 更新 hSmartWindowControl_map
            List<string> ListName = new List<string>();
            foreach (string name in locate_method_.DefectsClassify.Keys)
                ListName.Add(name);
            locate_method_.Disp_DefectTest(this.hSmartWindowControl_map, radioButton_Result, radioButton_Recheck, radioButton_fill, radioButton_margin, ListName);

            // 更新 listView_Edit
            locate_method_.Update_listView_Edit(this.listView_Edit, this.radioButton_Result, this.radioButton_Recheck);

            // 更新 listView_Result
            locate_method_.Update_listView_Result(this.listView_Result, this.radioButton_Result, this.radioButton_Recheck);
        }

        /// <summary>
        /// 【瑕疵顯示形式】 切換
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_DefDispType_CheckedChanged(object sender, EventArgs e) // (20190705) Jeff Revised!
        {
            RadioButton rbtn = sender as RadioButton;
            if (rbtn.Checked == false) return;

            // 更新 hSmartWindowControl_map
            if (locate_method_.b_Defect_Classify) // 多瑕疵
            {
                if (listView_Edit.SelectedIndices.Count > 0)
                {
                    List<string> ListName = new List<string>();
                    ListName.Add(listView_Edit.SelectedItems[0].Text);
                    locate_method_.Disp_DefectTest(this.hSmartWindowControl_map, radioButton_Result, radioButton_Recheck,
                                                   radioButton_fill, radioButton_margin, ListName, false);
                }
            }
            else // 單一瑕疵
            {
                locate_method_.Disp_DefectTest(this.hSmartWindowControl_map, radioButton_Result, radioButton_Recheck,
                                                   radioButton_fill, radioButton_margin, null, false);
            }
        }

        #endregion



        #region 【B面資訊】

        /// <summary>
        /// 【載入】(B面)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Load_DefectTest_B_Click(object sender, EventArgs e) // (20190817) Jeff Revised!
        {
            // locate_method_B 初始化: 從.xml檔讀取變數
            locate_method_B.board_type_ = locate_method_.board_type_;
            if (!locate_method_B.load())
            {
                SystemSounds.Exclamation.Play();
                MessageBox.Show("從.xml檔讀取變數失敗!", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.textBox_Path_DefectTest_B.Text = "";
                return;
            }

            // 載入瑕疵檔
            FolderBrowserDialog Dilg = new FolderBrowserDialog();
            Dilg.Description = "【載入】(B面)"; // (20200429) Jeff Revised!
            Dilg.SelectedPath = this.textBox_Path_DefectTest_B.Text; // 初始路徑
            if (Dilg.ShowDialog() != DialogResult.OK)
                return;

            this.textBox_Path_DefectTest_B.Text = Dilg.SelectedPath;
            if (string.IsNullOrEmpty(this.textBox_Path_DefectTest_B.Text))
            {
                SystemSounds.Exclamation.Play();
                MessageBox.Show("路徑無效!", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.textBox_Path_DefectTest_B.Text = "";
                return;
            }

            if (this.bw_Load_DefectTest_B.IsBusy != true)
            {
                this.tabControl_BigMap.Enabled = false;

                this.bw_Load_DefectTest_B.RunWorkerAsync();

                ctrl_timer1 = button_Load_DefectTest_B;
                BackColor_ctrl_timer1_1 = Color.Blue;
                BackColor_ctrl_timer1_2 = Color.Green;
                timer1.Enabled = true;
            }
        }

        clsProgressbar m_ProgressBar { get; set; } = null; // (20200429) Jeff Revised!
        private void bw_DoWork_Load_DefectTest_B(object sender, DoWorkEventArgs e) // (20200429) Jeff Revised!
        {
            BackgroundWorker Worker = (BackgroundWorker)sender;
            //clsProgressbar m_ProgressBar = new clsProgressbar();
            if (this.m_ProgressBar == null) // (20200429) Jeff Revised!
            {
                this.m_ProgressBar = new clsProgressbar();

                this.m_ProgressBar.FormClosedEvent2 += new clsProgressbar.FormClosedHandler2(SetFormClosed2);

                this.m_ProgressBar.SetShowText("請等待B面【載入】......");
                this.m_ProgressBar.SetShowCaption("執行中......");
                this.m_ProgressBar.ShowWaitProgress();
            }
            
            if (LocateMethod.Load_DefectTest_Recipe(ref this.locate_method_B, this.textBox_Path_DefectTest_B.Text + "\\")) // (20200429) Jeff Revised!
                e.Result = "Success";
            else
                e.Result = "Exception";

            //m_ProgressBar.CloseProgress();

            if (bw_Load_DefectTest_B.CancellationPending == true) // 判斷是否停止BackgroundWorker執行
            {
                e.Cancel = true;
                return;
            }
        }

        private void bw_RunWorkerCompleted_Load_DefectTest_B(object sender, RunWorkerCompletedEventArgs e) // (20200429) Jeff Revised!
        {
            if (e.Cancelled == true)
            {
                //MessageBox.Show("取消!");
                this.textBox_Path_DefectTest_B.Text = "";
            }
            else if (e.Error != null)
            {
                MessageBox.Show("Error: " + e.Error.Message);
                this.textBox_Path_DefectTest_B.Text = "";
            }
            else if (e.Result != null && e.Result.ToString() == "Exception")
            {
                SystemSounds.Exclamation.Play();
                MessageBox.Show("【載入瑕疵檔】失敗!", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.textBox_Path_DefectTest_B.Text = "";
            }
            else if (e.Result != null && e.Result.ToString() == "Success")
            {
                if (timer1.Enabled && sender != null) // (20200429) Jeff Revised!
                    this.m_ProgressBar.SetShowText("【載入】完成，執行GUI更新......");

                #region 更新GUI

                if (this.locate_method_B.b_Defect_Recheck) // 啟用人工覆判
                {
                    radioButton_Recheck.Enabled = true;
                    cbx_Recheck_B.Checked = true;
                    groupBox_Recheck_B.Enabled = true;
                }

                if (this.locate_method_B.b_Defect_Classify) // 瑕疵分類是否啟用
                {
                    cbx_NG_classification_B.Checked = true;
                    groupBox_DefectsClassify_B.Enabled = true;
                }

                this.cbx_Priority_B.Checked = this.locate_method_B.b_Defect_priority; // 瑕疵優先權是否啟用
                this.button_Save_DefectTest_B.Enabled = true;
                this.button_SaveAs_DefectTest_B.Enabled = true;

                #endregion

                if (this.tabControl_BigMap.SelectedTab.Text == "B面資訊") // (20200429) Jeff Revised!
                {
                    // 更新【總瑕疵】
                    this.Update_groupBox_TotalDefect(this.locate_method_B);

                    // 更新 hSmartWindowControl_map
                    List<string> ListName = new List<string>();
                    foreach (string name in this.locate_method_B.DefectsClassify.Keys)
                        ListName.Add(name);
                    this.locate_method_B.Disp_DefectTest(this.hSmartWindowControl_map, this.radioButton_Result, this.radioButton_Recheck, radioButton_fill_B, radioButton_margin_B, ListName);

                    // 更新 listView_Edit
                    this.locate_method_B.Update_listView_Edit(this.listView_Edit_B, this.radioButton_Result, this.radioButton_Recheck);

                    // 更新 listView_Result
                    this.locate_method_B.Update_listView_Result(this.listView_Result_B, this.radioButton_Result, this.radioButton_Recheck);

                    this.Update_DispImageInfo(this.locate_method_B.TileImage);
                }

                radioButton_DispB_CellImage.Enabled = true;
                radioButton_DispB_CellImage.Checked = true;

                //MessageBox.Show("【載入瑕疵檔】成功!");

                #region 【雙面統計結果合併】

                if (this.locate_method_.b_Defect_Recheck || this.locate_method_B.b_Defect_Recheck) // 啟用人工覆判
                {
                    this.cbx_Recheck_AB.Checked = true;
                    this.groupBox_Recheck_AB.Enabled = true;
                }
                else
                {
                    this.cbx_Recheck_AB.Checked = false;
                    this.groupBox_Recheck_AB.Enabled = false;
                }

                this.cbx_B_UpDown_invert.Enabled = true;
                this.cbx_B_LeftRight_invert.Enabled = true;
                this.groupBox_DefectsResult_AB.Enabled = true;
                this.groupBox_NGClassify_Statistics.Enabled = true; // (20200429) Jeff Revised!

                /* 更新GUI */
                // (B面翻轉方式)
                this.textBox_Path_DefectTest_AB.Text = this.Recipe_AB.Directory_AB_Recipe;
                this.cbx_B_UpDown_invert.CheckedChanged -= new System.EventHandler(this.cbx_B_invert_CheckedChanged);
                this.cbx_B_LeftRight_invert.CheckedChanged -= new System.EventHandler(this.cbx_B_invert_CheckedChanged);
                this.cbx_B_UpDown_invert.Checked = this.Recipe_AB.B_UpDown_invert;
                this.cbx_B_LeftRight_invert.Checked = this.Recipe_AB.B_LeftRight_invert;
                this.cbx_B_UpDown_invert.CheckedChanged += new System.EventHandler(this.cbx_B_invert_CheckedChanged);
                this.cbx_B_LeftRight_invert.CheckedChanged += new System.EventHandler(this.cbx_B_invert_CheckedChanged);
                // 瑕疵分類統計 (人工覆判結果) // (20200429) Jeff Revised!
                this.cbx_B_NG_Classify.CheckedChanged -= new System.EventHandler(this.cbx_B_NG_Classify_CheckedChanged);
                this.cbx_B_NG_Classify.Checked = this.Recipe_AB.B_NG_Classify;
                this.listView_NGClassify_Statistics.Enabled = this.cbx_B_NG_Classify.Checked;
                this.cbx_B_NG_Classify.CheckedChanged += new System.EventHandler(this.cbx_B_NG_Classify_CheckedChanged);

                //this.Recipe_AB.Set_B_invert(this.cbx_B_UpDown_invert.Checked, this.cbx_B_LeftRight_invert.Checked);
                this.Recipe_AB.Set_LocateMethod(this.locate_method_, this.locate_method_B);
                this.locate_method_B_2_A = this.Recipe_AB.Locate_method_B_2_A;

                if (this.tabControl_BigMap.SelectedTab.Text == "雙面統計結果合併")
                    this.tabControl_BigMap_SelectedIndexChanged(null, null);

                #endregion

            }
            else
                return;

            if (timer1.Enabled && sender != null) // (20200429) Jeff Revised!
            {
                timer1.Enabled = false;
                ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
                this.tabControl_BigMap.Enabled = true;

                this.m_ProgressBar.CloseProgress(); // (20190824) Jeff Revised!
                this.m_ProgressBar = null; // (20200429) Jeff Revised!
            }
        }

        /// <summary>
        /// 停止BackgroundWorker執行
        /// </summary>
        public void SetFormClosed2() // (20200429) Jeff Revised!
        {
            if (bw_Load_DefectTest.WorkerSupportsCancellation == true)
            {
                if (bw_Load_DefectTest.IsBusy)
                    bw_Load_DefectTest.CancelAsync();
            }

            if (bw_Load_DefectTest_B.WorkerSupportsCancellation == true)
            {
                if (bw_Load_DefectTest_B.IsBusy)
                    bw_Load_DefectTest_B.CancelAsync();
            }

            if (bw_B_invert.WorkerSupportsCancellation == true)
            {
                if (bw_B_invert.IsBusy)
                    bw_B_invert.CancelAsync();
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
        /// 【儲存/另存】(B面)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Save_DefectTest_B_Click(object sender, EventArgs e) // (20200429) Jeff Revised!
        {
            if (e != null) // (20200429) Jeff Revised!
            {
                SystemSounds.Exclamation.Play();
                if (MessageBox.Show("確定要【儲存/另存】?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) // (20200429) Jeff Revised!
                    return;
            }

            Button bt = sender as Button; // (20200429) Jeff Revised!
            if (this.textBox_Path_DefectTest_B.Text == "" || bt == this.button_SaveAs_DefectTest_B) // (20200429) Jeff Revised!
            {
                FolderBrowserDialog Dilg = new FolderBrowserDialog();
                Dilg.Description = "【儲存/另存】(B面)"; // (20200429) Jeff Revised!
                Dilg.SelectedPath = this.textBox_Path_DefectTest_B.Text; // 初始路徑
                if (Dilg.ShowDialog() != DialogResult.OK)
                    return;

                this.textBox_Path_DefectTest_B.Text = Dilg.SelectedPath;
            }

            if (string.IsNullOrEmpty(this.textBox_Path_DefectTest_B.Text))
            {
                SystemSounds.Exclamation.Play();
                MessageBox.Show("路徑無效!", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.textBox_Path_DefectTest_B.Text = "";
                return;
            }
            // 儲存 Path_DefectTest (20200429) Jeff Revised!
            this.locate_method_B.Path_DefectTest = this.textBox_Path_DefectTest_B.Text;

            if (this.bw_SaveAs_DefectTest.IsBusy != true)
            {
                this.tabPage_BSide.Enabled = false;

                object[] parameters = new object[] { "B面", this.locate_method_B, this.textBox_Path_DefectTest_B, this.tabPage_BSide }; // (20200429) Jeff Revised!
                this.bw_SaveAs_DefectTest.RunWorkerAsync(parameters); // (20200429) Jeff Revised!

                ctrl_timer1 = bt; // (20200429) Jeff Revised!
                BackColor_ctrl_timer1_1 = Color.Blue;
                BackColor_ctrl_timer1_2 = Color.Green;
                timer1.Enabled = true;
            }
            else
                MessageBox.Show("背景執行緖忙碌中，請稍後再試", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            
        }
        
        /// <summary>
        /// 【編輯顏色】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Edit_DefectsClassify_B_Click(object sender, EventArgs e) // (20190820) Jeff Revised!
        {
            if (listView_Edit_B.SelectedIndices.Count <= 0)
            {
                ctrl_timer1 = listView_Edit_B;
                BackColor_ctrl_timer1_1 = Color.AliceBlue;
                BackColor_ctrl_timer1_2 = Color.Green;
                timer1.Enabled = true;
                SystemSounds.Exclamation.Play();
                MessageBox.Show("請選擇瑕疵名稱", "溫馨提醒", MessageBoxButtons.OK, MessageBoxIcon.Information);
                timer1.Enabled = false;
                ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
                return;
            }

            using (Form_editDefect form = new Form_editDefect(locate_method_B, true, listView_Edit_B.SelectedItems[0].Text, false))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    // 更新 hSmartWindowControl_map
                    this.listView_Edit_B_SelectedIndexChanged(sender, e);

                    // 更新 listView_Edit
                    this.locate_method_B.Update_listView_Edit(this.listView_Edit_B, this.radioButton_Result, this.radioButton_Recheck);

                    // 更新 listView_Result
                    this.locate_method_B.Update_listView_Result(this.listView_Result_B, this.radioButton_Result, this.radioButton_Recheck);

                    // 更新 Locate_method_B_2_A 之 DefectsClassify 顏色
                    if (!this.Recipe_AB.Update_Color_Locate_method_B_2_A()) // (20190826) Jeff Revised!
                        return;
                }
            }
        }

        /// <summary>
        /// 更新顯示瑕疵 (hSmartWindowControl_map)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView_Edit_B_SelectedIndexChanged(object sender, EventArgs e) // (20200429) Jeff Revised!
        {
            if (listView_Edit_B.SelectedIndices.Count <= 0)
                return;

            List<string> ListName = new List<string>();
            ListName.Add(listView_Edit_B.SelectedItems[0].Text);
            locate_method_B.Disp_DefectTest(this.hSmartWindowControl_map, radioButton_Result, radioButton_Recheck, radioButton_fill_B, radioButton_margin_B, ListName, false);

            // 同時選取 listView_Result_B 中相同瑕疵名稱 (20200429) Jeff Revised!
            this.listView_Result_B.SelectedIndexChanged -= new System.EventHandler(this.listView_Result_B_SelectedIndexChanged);
            if (this.listView_Result_B.SelectedIndices.Count > 0) // 消除listView_Result_B目前選取狀態
                this.listView_Result_B.SelectedItems[0].Selected = false;
            this.listView_Result_B.Items[this.listView_Edit_B.SelectedIndices[0]].Focused = true;
            this.listView_Result_B.Items[this.listView_Edit_B.SelectedIndices[0]].Selected = true;
            this.listView_Result_B.Items[this.listView_Edit_B.SelectedIndices[0]].EnsureVisible();
            this.listView_Result_B.SelectedIndexChanged += new System.EventHandler(this.listView_Result_B_SelectedIndexChanged);
        }

        /// <summary>
        ///  更新顯示瑕疵 (hSmartWindowControl_map)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView_Result_B_SelectedIndexChanged(object sender, EventArgs e) // (20200429) Jeff Revised!
        {
            if (this.listView_Result_B.SelectedIndices.Count <= 0)
                return;

            // 同時選取 listView_Edit_B 中相同瑕疵名稱
            if (this.listView_Edit_B.SelectedIndices.Count > 0) // 消除listView_Edit_B目前選取狀態
                this.listView_Edit_B.SelectedItems[0].Selected = false;
            this.listView_Edit_B.Items[this.listView_Result_B.SelectedIndices[0]].Focused = true;
            this.listView_Edit_B.Items[this.listView_Result_B.SelectedIndices[0]].Selected = true;
            this.listView_Edit_B.Items[this.listView_Result_B.SelectedIndices[0]].EnsureVisible();
        }

        /// <summary>
        /// 【人工覆判】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbx_Do_Recheck_B_CheckedChanged(object sender, EventArgs e) // (20190820) Jeff Revised!
        {
            CheckBox Obj = (CheckBox)sender;

            if (Obj.Checked) // 人工覆判
            {
                try
                {
                    DefectName_select.Clear();
                    contextMenuStrip_Recheck.Items.Clear();
                    if (locate_method_B.b_Defect_Classify && radioButton_MultiCells_B.Checked) // 多瑕疵 & 多顆標註模式
                    {
                        if (listView_Edit_B.SelectedIndices.Count <= 0)
                        {
                            #region CheckBox狀態復原
                            cbx_Do_Recheck_B.CheckedChanged -= cbx_Do_Recheck_B_CheckedChanged;
                            Obj.Checked = false;
                            cbx_Do_Recheck_B.CheckedChanged += cbx_Do_Recheck_B_CheckedChanged;
                            #endregion

                            ctrl_timer1 = listView_Edit_B;
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
                            DefectName_select.Add(listView_Edit_B.SelectedItems[0].Text);
                        }
                    }
                    else if (locate_method_B.b_Defect_Classify && radioButton_SingleCell_B.Checked) // 多瑕疵 & 單顆標註模式
                    {
                        contextMenuStrip_Recheck.Items.Add(this.GetMenuItem(locate_method_B, "B", "OK", Color.Green));
                        foreach (string name in locate_method_B.DefectsClassify.Keys)
                        {
                            DefectName_select.Add(name);
                            contextMenuStrip_Recheck.Items.Add(this.GetMenuItem(locate_method_B, "B", name, locate_method_B.DefectsClassify[name].GetColor()));
                        }
                    }
                    else // 單一瑕疵
                    {
                        DefectName_select.Add("");
                        if (radioButton_SingleCell_B.Checked) // 單顆標註模式
                        {
                            contextMenuStrip_Recheck.Items.Add(this.GetMenuItem(locate_method_B, "B", "OK", Color.Green));
                            contextMenuStrip_Recheck.Items.Add(this.GetMenuItem(locate_method_B, "B", "NG", Color.Red));
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
                        cbx_Do_Recheck_B.CheckedChanged -= cbx_Do_Recheck_B_CheckedChanged;
                        Obj.Checked = false;
                        cbx_Do_Recheck_B.CheckedChanged += cbx_Do_Recheck_B_CheckedChanged;
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
                            cbx_Do_Recheck_B.CheckedChanged -= cbx_Do_Recheck_B_CheckedChanged;
                            Obj.Checked = false;
                            cbx_Do_Recheck_B.CheckedChanged += cbx_Do_Recheck_B_CheckedChanged;
                            #endregion
                            return;
                        }

                        int index = openFileDialog_file.SafeFileName.IndexOf("_M");
                        int capture_index_ = Convert.ToInt32(openFileDialog_file.SafeFileName.Substring(index + 2, 3).ToString()) - 2;

                        if (!(capture_index_ >= 1 && capture_index_ <= locate_method_B.array_x_count_ * locate_method_B.array_y_count_))
                        {
                            #region CheckBox狀態復原
                            cbx_Do_Recheck_B.CheckedChanged -= cbx_Do_Recheck_B_CheckedChanged;
                            Obj.Checked = false;
                            cbx_Do_Recheck_B.CheckedChanged += cbx_Do_Recheck_B_CheckedChanged;
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
                        HOperatorSet.CopyObj(this.locate_method_B.TileImage, out Image_MoveIndex, 1, -1);
                    }

                    /* 計算此位置Cell */
                    HOperatorSet.SetSystem("clip_region", "false");
                    Extension.HObjectMedthods.ReleaseHObject(ref CellReg_MoveIndex);
                    locate_method_B.Compute_CellReg_MoveIndex(index_capture, out CellReg_MoveIndex);
                    Extension.HObjectMedthods.ReleaseHObject(ref CellReg_MoveIndex_Dila);
                    HOperatorSet.DilationCircle(CellReg_MoveIndex, out CellReg_MoveIndex_Dila, 5);
                    HOperatorSet.SetSystem("clip_region", "true");

                    /* 計算此位置瑕疵 + 計算此位置已標註Cell */
                    Update_DefectReg_cellLabelled_MoveIndex(locate_method_B);

                    // 更新顯示
                    locate_method_B.Disp_Recheck(hSmartWindowControl_map, Image_MoveIndex, CellReg_MoveIndex,
                                                 DefectReg_MoveIndex, DefectName_select, cellLabelled_MoveIndex, true);

                    ListCtrl_Bypass = clsStaticTool.EnableAllControls_Bypass(this.tabControl_BigMap, Obj, false);
                    Obj.ForeColor = Color.GreenYellow;
                    Obj.Text = "設定完成";
                    ctrl_timer1 = Obj;
                    BackColor_ctrl_timer1_1 = Color.DarkViolet;
                    BackColor_ctrl_timer1_2 = Color.Lime;
                    timer1.Enabled = true;

                    label_cellID_B.Enabled = true;
                    txt_cellID_B.Enabled = true;
                    button_LoadCellImage.Enabled = false;
                    radioButton_Result.Enabled = false;
                    radioButton_Recheck.Enabled = false;

                    // 禁能滑鼠點擊事件 (原本)
                    this.hSmartWindowControl_map.HMouseDown -= new HalconDotNet.HMouseEventHandler(this.hSmartWindowControl_map_HMouseDown);
                    // 致能滑鼠點擊事件 (人工覆判)
                    this.hSmartWindowControl_map.HMouseDown += new HalconDotNet.HMouseEventHandler(this.hSmartWindowControl_map_HMouseDown_Recheck_B);

                    Update_DispImageInfo(Image_MoveIndex);
                }
                catch (Exception ex)
                {
                    #region CheckBox狀態復原
                    cbx_Do_Recheck_B.CheckedChanged -= cbx_Do_Recheck_B_CheckedChanged;
                    Obj.Checked = false;
                    cbx_Do_Recheck_B.CheckedChanged += cbx_Do_Recheck_B_CheckedChanged;
                    #endregion

                    MessageBox.Show(ex.ToString(), "人工覆判失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

            }
            else // 設定完成
            {
                // 更新 locate_method_B_2_A
                if (this.locate_method_B.Path_DefectTest != "")
                {
                    if (!this.Recipe_AB.Update_Locate_method_B_2_A_Recheck(true, this.locate_method_, this.locate_method_B)) // (20200429) Jeff Revised!
                    {
                        //return;
                    }
                }

                timer1.Enabled = false;
                ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
                clsStaticTool.EnableAllControls_Bypass(this.tabControl_BigMap, Obj, true, ListCtrl_Bypass);
                Obj.ForeColor = Color.White;
                Obj.Text = "人工覆判";

                button_LoadCellImage.Enabled = true;
                radioButton_Result.Enabled = true;
                radioButton_Recheck.Enabled = true;

                // 禁能滑鼠點擊事件 (人工覆判)
                this.hSmartWindowControl_map.HMouseDown -= new HalconDotNet.HMouseEventHandler(this.hSmartWindowControl_map_HMouseDown_Recheck_B);
                // 致能滑鼠點擊事件 (原本)
                this.hSmartWindowControl_map.HMouseDown += new HalconDotNet.HMouseEventHandler(this.hSmartWindowControl_map_HMouseDown);

                Update_DispImageInfo(this.locate_method_B.TileImage);

                try
                {
                    // 更新 hSmartWindowControl_map
                    if (!locate_method_B.b_Defect_Classify) // 單一瑕疵
                    {
                        locate_method_B.Disp_DefectTest(this.hSmartWindowControl_map, radioButton_Result, radioButton_Recheck, radioButton_fill_B, radioButton_margin_B);
                    }
                    else // 多瑕疵
                    {
                        List<string> ListName = new List<string>();
                        foreach (string name in DefectName_select)
                            ListName.Add(name);
                        locate_method_B.Disp_DefectTest(this.hSmartWindowControl_map, radioButton_Result, radioButton_Recheck, radioButton_fill_B, radioButton_margin_B, ListName);
                    }

                    // 更新 listView_Result
                    locate_method_B.Update_listView_Result(this.listView_Result_B, this.radioButton_Result, this.radioButton_Recheck);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "人工覆判失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
        }

        /// <summary>
        /// 滑鼠點擊事件 (人工覆判)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hSmartWindowControl_map_HMouseDown_Recheck_B(object sender, HMouseEventArgs e) // (20190820) Jeff Revised!
        {
            // 游標是否於 Cell region 內
            HTuple hv_Index = new HTuple();
            HOperatorSet.GetRegionIndex(CellReg_MoveIndex, (int)(e.Y + 0.5), (int)(e.X + 0.5), out hv_Index);
            if (hv_Index.Length <= 0)
            {
                txt_cellID_B.Text = "";
                return;
            }

            HTuple cell_ids = new HTuple();
            List<Point> ListCellID = new List<Point>();
            // 計算此顆Cell座標
            if (!locate_method_B.pos_2_cellID_MoveIndex(index_capture, e.Y, e.X, out cell_ids, out ListCellID))
                return;

            if (e.Button == System.Windows.Forms.MouseButtons.Left) // 滑鼠是否點擊左鍵
            {
                #region 滑鼠點擊左鍵Cell座標 (X, Y)

                // 更新顯示
                string str = "";
                foreach (Point pt in ListCellID)
                    str += "(" + pt.X + ", " + pt.Y + ")";
                txt_cellID_B.Text = str;
                locate_method_B.Disp_Recheck(hSmartWindowControl_map, Image_MoveIndex, CellReg_MoveIndex,
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

                // 【單顆Cell影像】
                Point_CellImage = ListCellID[0];
                txt_CellImage_cellID.Text = "(" + Point_CellImage.X + ", " + Point_CellImage.Y + ")";
                Display_CellImage(locate_method_B);

                #endregion

                return;
            }

            if (e.Button != System.Windows.Forms.MouseButtons.Right) // 滑鼠是否點擊右鍵
                return;

            if (radioButton_MultiCells_B.Checked) // 多顆標註模式 (小圖上標註 & 大圖上標註)
            {
                #region 多顆標註模式

                // 判斷是要判定此顆Cell為NG or OK: 利用游標是否於 DefectReg_MoveIndex 內
                bool b_NG = false;
                HOperatorSet.GetRegionIndex(DefectReg_MoveIndex[0], (int)(e.Y + 0.5), (int)(e.X + 0.5), out hv_Index);
                if (hv_Index.Length <= 0)
                    b_NG = true;

                /* 更新人工覆判結果 */
                if (b_NG) // 標註此顆Cell NG
                    locate_method_B.Update_Recheck(null, ListCellID, DefectName_select[0]);
                else // 標註此顆Cell OK
                    locate_method_B.Update_Recheck(ListCellID, null, DefectName_select[0]);

                /* 計算此位置瑕疵 + 計算此位置已標註Cell */
                Update_DefectReg_cellLabelled_MoveIndex(locate_method_B);

                // 更新顯示
                locate_method_B.Disp_Recheck(hSmartWindowControl_map, Image_MoveIndex, CellReg_MoveIndex,
                                             DefectReg_MoveIndex, DefectName_select, cellLabelled_MoveIndex, false);

                // 更新 listView_Result
                locate_method_B.Update_listView_Result(this.listView_Result_B, this.radioButton_Result, this.radioButton_Recheck);

                #endregion

            }
            else if (radioButton_SingleCell_B.Checked) // 單顆標註模式
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
                if (!locate_method_B.b_Defect_Classify) // 單一瑕疵
                {
                    if (locate_method_B.all_defect_id_Recheck.Contains(DefectId_SingleCell[0])) // NG Cell
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
                        if (locate_method_B.DefectsClassify[DefectName_select[i - 1]].all_defect_id_Recheck.Contains(DefectId_SingleCell[0])) // 包含此瑕疵類型
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
        /// 【清空人工覆判】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Clear_Recheck_B_Click(object sender, EventArgs e) // (20190820) Jeff Revised!
        {
            // 新增使用者保護機制
            SystemSounds.Exclamation.Play();
            DialogResult dr = MessageBox.Show("確定要清空人工覆判 ?", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr != DialogResult.Yes)
                return;

            locate_method_B.Initialize_Recheck();

            // 更新 hSmartWindowControl_map
            List<string> ListName = new List<string>();
            foreach (string name in locate_method_B.DefectsClassify.Keys)
                ListName.Add(name);
            locate_method_B.Disp_DefectTest(this.hSmartWindowControl_map, radioButton_Result, radioButton_Recheck, radioButton_fill_B, radioButton_margin_B, ListName);

            // 更新 listView_Edit
            locate_method_B.Update_listView_Edit(this.listView_Edit_B, this.radioButton_Result, this.radioButton_Recheck);

            // 更新 listView_Result
            locate_method_B.Update_listView_Result(this.listView_Result_B, this.radioButton_Result, this.radioButton_Recheck);
        }

        /// <summary>
        /// 【瑕疵顯示形式】 切換
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_DefDispType_B_CheckedChanged(object sender, EventArgs e) // (20190820) Jeff Revised!
        {
            RadioButton rbtn = sender as RadioButton;
            if (rbtn.Checked == false) return;

            // 更新 hSmartWindowControl_map
            if (locate_method_B.b_Defect_Classify) // 多瑕疵
            {
                if (listView_Edit_B.SelectedIndices.Count > 0)
                {
                    List<string> ListName = new List<string>();
                    ListName.Add(listView_Edit_B.SelectedItems[0].Text);
                    locate_method_B.Disp_DefectTest(this.hSmartWindowControl_map, radioButton_Result, radioButton_Recheck, radioButton_fill_B, radioButton_margin_B, ListName, false);
                }
            }
            else // 單一瑕疵
            {
                locate_method_B.Disp_DefectTest(this.hSmartWindowControl_map, radioButton_Result, radioButton_Recheck, radioButton_fill_B, radioButton_margin_B, null, false);
            }
        }

        #endregion



        #region 【雙面統計結果合併】

        /// <summary>
        /// 【載入】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Load_DefectTest_AB_Click(object sender, EventArgs e) // (20200429) Jeff Revised!
        {
            FolderBrowserDialog Dilg = new FolderBrowserDialog();
            Dilg.Description = "【載入】雙面工單";
            Dilg.SelectedPath = this.textBox_Path_DefectTest_AB.Text; // 初始路徑
            if (Dilg.ShowDialog() != DialogResult.OK)
                return;

            this.textBox_Path_DefectTest_AB.Text = Dilg.SelectedPath;
            if (string.IsNullOrEmpty(this.textBox_Path_DefectTest_AB.Text))
            {
                SystemSounds.Exclamation.Play();
                MessageBox.Show("路徑無效!", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.textBox_Path_DefectTest_AB.Text = "";
                return;
            }

            int Method = 2;
            if (Method == 1) // bw_Load_DefectTest
            {
                #region 雙面

                this.b_Load_success = false;

                if (this.bw_Load_DefectTest.IsBusy != true)
                {
                    this.tabPage_2Sides.Enabled = false;

                    object[] parameters = new object[] { "雙面", this.textBox_Path_DefectTest_AB.Text + "\\", this.tabPage_2Sides };
                    this.bw_Load_DefectTest.RunWorkerAsync(parameters);

                    ctrl_timer1 = this.button_Load_DefectTest_AB;
                    BackColor_ctrl_timer1_1 = Color.Blue;
                    BackColor_ctrl_timer1_2 = Color.Green;
                    //timer1.Enabled = true; // 不穩定，有時成功有時失敗!!!
                }
                else
                {
                    MessageBox.Show("背景執行緖忙碌中，請稍後再試", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 等待載入雙面
                while (true)
                {
                    // UI才會更新 (Timer持續作用) !
                    Application.DoEvents(); // 處理當前在消息隊列中的所有 Windows 消息，可以防止界面停止響應
                    Thread.Sleep(1); // Sleep一下，避免一直跑DoEvents()導致計算效率低
                    if (this.bw_Load_DefectTest.IsBusy != true)
                        break;
                }

                // 【載入】失敗，GUI還原
                if (this.b_Load_success == false)
                {
                    MessageBox.Show("【載入】失敗!", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    timer1.Enabled = false;
                    ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
                    this.tabPage_2Sides.Enabled = true;
                    this.m_ProgressBar.CloseProgress();
                    this.m_ProgressBar = null;
                    return;
                }

                #endregion

                //Thread.Sleep(5); // 必須大於1，否則 this.bw_Load_DefectTest.CancellationPending = true

                #region A面

                this.b_Load_success = false;

                this.textBox_Path_DefectTest.Text = this.Recipe_AB.Path_DefectTest_A;
                if (this.bw_Load_DefectTest.IsBusy != true)
                {
                    object[] parameters = new object[] { "A面", this.textBox_Path_DefectTest.Text + "\\", this.tabPage_2Sides };
                    this.bw_Load_DefectTest.RunWorkerAsync(parameters);
                }
                else
                    MessageBox.Show("背景執行緖忙碌中，請稍後再試", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // 等待載入A面
                while (true)
                {
                    // UI才會更新 (Timer持續作用) !
                    Application.DoEvents(); // 處理當前在消息隊列中的所有 Windows 消息，可以防止界面停止響應
                    Thread.Sleep(1); // Sleep一下，避免一直跑DoEvents()導致計算效率低
                    if (this.bw_Load_DefectTest.IsBusy != true)
                        break;
                }

                // 【載入】失敗，GUI還原
                if (this.b_Load_success == false)
                {
                    MessageBox.Show("【載入】失敗!", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    timer1.Enabled = false;
                    ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
                    this.tabPage_2Sides.Enabled = true;
                    this.m_ProgressBar.CloseProgress();
                    this.m_ProgressBar = null;
                    return;
                }

                #endregion

                //Thread.Sleep(5); // 必須大於1，否則 this.bw_Load_DefectTest.CancellationPending = true

                #region B面

                this.b_Load_success = false;

                this.textBox_Path_DefectTest_B.Text = this.Recipe_AB.Path_DefectTest_B;
                if (this.bw_Load_DefectTest.IsBusy != true)
                {
                    object[] parameters = new object[] { "B面", this.textBox_Path_DefectTest_B.Text + "\\", this.tabPage_2Sides };
                    this.bw_Load_DefectTest.RunWorkerAsync(parameters);
                }
                else
                    MessageBox.Show("背景執行緖忙碌中，請稍後再試", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // 等待載入B面
                while (true)
                {
                    // UI才會更新 (Timer持續作用) !
                    Application.DoEvents(); // 處理當前在消息隊列中的所有 Windows 消息，可以防止界面停止響應
                    Thread.Sleep(1); // Sleep一下，避免一直跑DoEvents()導致計算效率低
                    if (this.bw_Load_DefectTest.IsBusy != true)
                        break;
                }

                // 【載入】失敗，GUI還原
                if (this.b_Load_success == false)
                {
                    MessageBox.Show("【載入】失敗!", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    timer1.Enabled = false;
                    ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
                    this.tabPage_2Sides.Enabled = true;
                    this.m_ProgressBar.CloseProgress();
                    this.m_ProgressBar = null;
                    return;
                }

                #endregion

            }
            else if (Method == 2) // bw_Load_Recipe_AB
            {
                if (this.bw_Load_Recipe_AB.IsBusy != true)
                {
                    this.tabPage_2Sides.Enabled = false;
                    
                    this.bw_Load_Recipe_AB.RunWorkerAsync();

                    ctrl_timer1 = this.button_Load_DefectTest_AB;
                    BackColor_ctrl_timer1_1 = Color.Blue;
                    BackColor_ctrl_timer1_2 = Color.Green;
                    timer1.Enabled = true;
                }
                else
                {
                    MessageBox.Show("背景執行緖忙碌中，請稍後再試", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
        }

        private bool b_Load_success { get; set; } = false; // (20200429) Jeff Revised!
        private void bw_DoWork_Load_DefectTest(object sender, DoWorkEventArgs e) // (20200429) Jeff Revised!
        {
            BackgroundWorker Worker = (BackgroundWorker)sender;
            // 取得 input arguments
            object[] parameters = e.Argument as object[];
            string nowRecipe_DefectTest = parameters[0].ToString();
            string directory_ = parameters[1].ToString();
            TabPage tp = (TabPage)parameters[2];

            // 設定 e.Result
            e.Result = parameters;

            if (this.m_ProgressBar == null) // (20200429) Jeff Revised!
            {
                this.m_ProgressBar = new clsProgressbar();
                this.m_ProgressBar.FormClosedEvent2 += new clsProgressbar.FormClosedHandler2(SetFormClosed2);

                this.m_ProgressBar.SetShowText("請等待" + nowRecipe_DefectTest + "【載入】......");
                this.m_ProgressBar.SetShowCaption("執行中......");
                this.m_ProgressBar.ShowWaitProgress();
            }
            else
                this.m_ProgressBar.SetShowText("請等待" + nowRecipe_DefectTest + "【載入】......");

            List<object> param = parameters.ToList();
            if (nowRecipe_DefectTest == "雙面")
            {
                if (DefectTest_AB_Recipe.Load_DefectTest_AB_Recipe(directory_, out this.Recipe_AB))
                    param.Add("Success");
                else
                    param.Add("Exception");
            }
            else if (nowRecipe_DefectTest == "A面")
            {
                if (LocateMethod.Load_DefectTest_Recipe(ref this.locate_method_, directory_))
                    param.Add("Success");
                else
                    param.Add("Exception");
            }
            else if (nowRecipe_DefectTest == "B面")
            {
                if (LocateMethod.Load_DefectTest_Recipe(ref this.locate_method_B, directory_))
                    param.Add("Success");
                else
                    param.Add("Exception");
            }
            e.Result = param.ToArray();
            
            if (this.bw_Load_DefectTest.CancellationPending == true) // 判斷是否停止BackgroundWorker執行
            {
                e.Cancel = true;
                return;
            }
        }
        
        private void bw_RunWorkerCompleted_Load_DefectTest(object sender, RunWorkerCompletedEventArgs e) // (20200429) Jeff Revised!
        {
            // 取得 input arguments
            object[] parameters = e.Result as object[];
            string nowRecipe_DefectTest = parameters[0].ToString();
            TabPage tp = (TabPage)parameters[2];

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
                SystemSounds.Exclamation.Play();
                MessageBox.Show(nowRecipe_DefectTest + "【載入】失敗!", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (e.Result != null && parameters[3].ToString() == "Success")
            {
                //MessageBox.Show("【載入】成功!");
                this.b_Load_success = true;
                this.m_ProgressBar.SetShowText(nowRecipe_DefectTest + "【載入】完成，執行GUI更新......");
                if (nowRecipe_DefectTest == "雙面")
                    ;
                else if (nowRecipe_DefectTest == "A面")
                    this.CellMapForm_Load(null, null);
                else if (nowRecipe_DefectTest == "B面")
                    this.set_locate_method_B(this.locate_method_B);
            }
            else
                return;

            if (nowRecipe_DefectTest == "B面")
            {
                timer1.Enabled = false;
                ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
                tp.Enabled = true;
                this.m_ProgressBar.CloseProgress();
                this.m_ProgressBar = null;
            }

            //this.m_ProgressBar.CloseProgress();
            //this.m_ProgressBar = null;
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

            if (DefectTest_AB_Recipe.Load_DefectTest_AB_Recipe(this.textBox_Path_DefectTest_AB.Text + "\\", out this.Recipe_AB)) // 雙面
            {
                m_ProgressBar.SetShowText("請等待A面【載入】......");
                if (LocateMethod.Load_DefectTest_Recipe(ref this.locate_method_, this.Recipe_AB.Path_DefectTest_A + "\\")) // A面
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
                    this.textBox_Path_DefectTest.Text = this.Recipe_AB.Path_DefectTest_A;
                    this.CellMapForm_Load(null, null);
                }
            }
            else if (e.Result != null && e.Result.ToString() == "Success")
            {
                //MessageBox.Show("【載入瑕疵檔】成功!");
                this.m_ProgressBar.SetShowText("【載入】完成，執行GUI更新......");

                this.textBox_Path_DefectTest.Text = this.Recipe_AB.Path_DefectTest_A;
                //this.textBox_Path_DefectTest_B.Text = this.Recipe_AB.Path_DefectTest_B;
                this.CellMapForm_Load(null, null);
                this.set_locate_method_B(this.locate_method_B);
            }
            else
                return;
            
            timer1.Enabled = false;
            ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
            this.tabPage_2Sides.Enabled = true;
            this.m_ProgressBar.CloseProgress();
            this.m_ProgressBar = null;
        }

        /// <summary>
        /// 【儲存/另存】(雙面合併)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Save_DefectTest_AB_Click(object sender, EventArgs e) // (20200429) Jeff Revised!
        {
            SystemSounds.Exclamation.Play();
            if (MessageBox.Show("確定要【儲存/另存】?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) // (20200429) Jeff Revised!
                return;

            Button bt = sender as Button; // (20200429) Jeff Revised!
            if (this.textBox_Path_DefectTest_AB.Text == "" || bt == this.button_SaveAs_DefectTest_AB) // (20200429) Jeff Revised!
            {
                FolderBrowserDialog Dilg = new FolderBrowserDialog();
                Dilg.Description = "【儲存/另存】(雙面合併)"; // (20200429) Jeff Revised!
                Dilg.SelectedPath = this.textBox_Path_DefectTest_AB.Text; // 初始路徑
                if (Dilg.ShowDialog() != DialogResult.OK)
                    return;

                this.textBox_Path_DefectTest_AB.Text = Dilg.SelectedPath;
            }

            if (string.IsNullOrEmpty(this.textBox_Path_DefectTest_AB.Text))
            {
                SystemSounds.Exclamation.Play();
                MessageBox.Show("路徑無效!", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.textBox_Path_DefectTest_AB.Text = "";
                return;
            }
            // 儲存【雙面統計結果合併】Recipe路徑 // (20200429) Jeff Revised!
            this.Recipe_AB.Directory_AB_Recipe = this.textBox_Path_DefectTest_AB.Text;

            this.tabPage_2Sides.Enabled = false;
            ctrl_timer2 = bt; // (20200429) Jeff Revised!
            BackColor_ctrl_timer2_1 = Color.Blue;
            BackColor_ctrl_timer2_2 = Color.Green;
            timer2.Enabled = true;

            #region 儲存A面

            bool b_saveA = true;
            if (this.textBox_Path_DefectTest.Text != "")
            {
                DialogResult dialogResult = MessageBox.Show("是否儲存A面資訊?", "A面", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult != DialogResult.Yes)
                    b_saveA = false;
            }

            if (b_saveA)
            {
                this.button_Save_DefectTest_Click(this.button_Save_DefectTest, null);

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

            bool b_saveB = true;
            if (this.textBox_Path_DefectTest_B.Text != "")
            {
                DialogResult dialogResult = MessageBox.Show("是否儲存B面資訊?", "B面", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult != DialogResult.Yes)
                    b_saveB = false;
            }

            if (b_saveB)
            {
                this.button_Save_DefectTest_B_Click(this.button_Save_DefectTest_B, null);

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

            this.Recipe_AB.Path_DefectTest_A = this.textBox_Path_DefectTest.Text;
            this.Recipe_AB.Path_DefectTest_B = this.textBox_Path_DefectTest_B.Text;
            if (this.bw_SaveAs_DefectTest.IsBusy != true)
            {
                this.tabPage_2Sides.Enabled = false;

                object[] parameters = new object[] { "雙面", this.Recipe_AB, this.textBox_Path_DefectTest_AB, this.tabPage_2Sides };
                this.bw_SaveAs_DefectTest.RunWorkerAsync(parameters);
            }
            else
                MessageBox.Show("背景執行緖忙碌中，請稍後再試", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            #endregion

        }

        /// <summary>
        /// 【人工覆判】(【雙面統計結果合併】)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbx_Do_Recheck_AB_CheckedChanged(object sender, EventArgs e) // (20200429) Jeff Revised!
        {
            CheckBox Obj = (CheckBox)sender;

            if (Obj.Checked) // 人工覆判
            {
                try
                {
                    this.radioButton_AB_AB.Checked = true; // 【合併】 (20200429) Jeff Revised!

                    // A面
                    toolStripMenuItem_A.DropDownItems.Clear();
                    toolStripMenuItem_A.DropDownItems.Add(this.GetMenuItem_AB(locate_method_, "A", "OK", Color.Green));
                    if (locate_method_.b_Defect_Classify) // 多瑕疵 & 單顆標註模式
                    {
                        foreach (string name in locate_method_.DefectsClassify.Keys)
                            toolStripMenuItem_A.DropDownItems.Add(this.GetMenuItem_AB(locate_method_, "A", name, locate_method_.DefectsClassify[name].GetColor()));
                    }
                    else // 單一瑕疵 & 單顆標註模式
                        toolStripMenuItem_A.DropDownItems.Add(this.GetMenuItem_AB(locate_method_, "A", "NG", Color.Red));

                    // B面
                    toolStripMenuItem_B.DropDownItems.Clear();
                    toolStripMenuItem_B.DropDownItems.Add(this.GetMenuItem_AB(locate_method_B_2_A, "B", "OK", Color.Green));
                    if (locate_method_B_2_A.b_Defect_Classify) // 多瑕疵 & 單顆標註模式
                    {
                        foreach (string name in locate_method_B_2_A.DefectsClassify.Keys)
                            toolStripMenuItem_B.DropDownItems.Add(this.GetMenuItem_AB(locate_method_B_2_A, "B", name, locate_method_B_2_A.DefectsClassify[name].GetColor()));
                    }
                    else // 單一瑕疵 & 單顆標註模式
                        toolStripMenuItem_B.DropDownItems.Add(this.GetMenuItem_AB(locate_method_B_2_A, "B", "NG", Color.Red));

                    // 強制勾選【顯示資訊】為【人工覆判結果】
                    radioButton_Recheck.Checked = true;

                    // 大圖上標註
                    index_capture = -1;
                    Extension.HObjectMedthods.ReleaseHObject(ref Image_MoveIndex);
                    HOperatorSet.CopyObj(this.locate_method_.TileImage, out Image_MoveIndex, 1, -1);

                    /* 計算此位置Cell */
                    Extension.HObjectMedthods.ReleaseHObject(ref CellReg_MoveIndex);
                    CellReg_MoveIndex = locate_method_.cellmap_affine_.Clone();
                    Extension.HObjectMedthods.ReleaseHObject(ref CellReg_MoveIndex_Dila);
                    HOperatorSet.DilationCircle(CellReg_MoveIndex, out CellReg_MoveIndex_Dila, 5);

                    // 更新 hSmartWindowControl_map
                    Update_radioButton_defTest_tabControl();
                    HOperatorSet.SetPart(hSmartWindowControl_map.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part

                    ListCtrl_Bypass = clsStaticTool.EnableAllControls_Bypass(this.tabControl_BigMap, Obj, false);
                    Obj.ForeColor = Color.GreenYellow;
                    Obj.Text = "設定完成";
                    ctrl_timer1 = Obj;
                    BackColor_ctrl_timer1_1 = Color.DarkViolet;
                    BackColor_ctrl_timer1_2 = Color.Lime;
                    timer1.Enabled = true;

                    label_cellID_AB.Enabled = true;
                    txt_cellID_AB.Enabled = true;
                    button_LoadCellImage.Enabled = false;
                    radioButton_Result.Enabled = false;
                    radioButton_Recheck.Enabled = false;

                    // 禁能滑鼠點擊事件 (原本)
                    this.hSmartWindowControl_map.HMouseDown -= new HalconDotNet.HMouseEventHandler(this.hSmartWindowControl_map_HMouseDown);
                    // 致能滑鼠點擊事件 (人工覆判)
                    this.hSmartWindowControl_map.HMouseDown += new HalconDotNet.HMouseEventHandler(this.hSmartWindowControl_map_HMouseDown_Recheck_AB);

                }
                catch (Exception ex)
                {
                    #region CheckBox狀態復原
                    cbx_Do_Recheck_AB.CheckedChanged -= cbx_Do_Recheck_AB_CheckedChanged;
                    Obj.Checked = false;
                    cbx_Do_Recheck_AB.CheckedChanged += cbx_Do_Recheck_AB_CheckedChanged;
                    #endregion

                    MessageBox.Show(ex.ToString(), "人工覆判失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

            }
            else // 設定完成
            {
                // 更新 locate_method_B_2_A
                if (this.locate_method_B.Path_DefectTest != "")
                {
                    if (!this.Recipe_AB.Update_Locate_method_B_2_A_Recheck(true, this.locate_method_, this.locate_method_B)) // (20200429) Jeff Revised!
                    {
                        //return;
                    }
                }

                timer1.Enabled = false;
                ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
                clsStaticTool.EnableAllControls_Bypass(this.tabControl_BigMap, Obj, true, ListCtrl_Bypass);
                Obj.ForeColor = Color.White;
                Obj.Text = "人工覆判";

                button_LoadCellImage.Enabled = true;
                radioButton_Result.Enabled = true;
                radioButton_Recheck.Enabled = true;

                // 禁能滑鼠點擊事件 (人工覆判)
                this.hSmartWindowControl_map.HMouseDown -= new HalconDotNet.HMouseEventHandler(this.hSmartWindowControl_map_HMouseDown_Recheck_AB);
                // 致能滑鼠點擊事件 (原本)
                this.hSmartWindowControl_map.HMouseDown += new HalconDotNet.HMouseEventHandler(this.hSmartWindowControl_map_HMouseDown);

                try
                {
                    // 更新 hSmartWindowControl_map
                    Update_radioButton_defTest_tabControl();
                    HOperatorSet.SetPart(hSmartWindowControl_map.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "人工覆判失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }

        }

        /// <summary>
        /// 生成選單項 (【雙面統計結果合併】)
        /// </summary>
        /// <param name="locate_method"></param>
        /// <param name="str_side">A面 or B面</param>
        /// <param name="txt"></param>
        /// <param name="ForeColor"></param>
        /// <param name="img"></param>
        /// <param name="b_ClickEvent"></param>
        /// <returns></returns>
        private ToolStripMenuItem GetMenuItem_AB(LocateMethod locate_method, string str_side, string txt, object ForeColor = null, Image img = null, bool b_ClickEvent = true) // (20190826) Jeff Revised!
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
            {
                menuItem.Click += (sender, e) => toolStripMenuItem_AB_Click(sender, locate_method, str_side);
            }

            return menuItem;
        }

        /// <summary>
        /// 選單項事件響應 (有參數傳入) (【雙面統計結果合併】)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="locate_method"></param>
        /// <param name="str_side">A面 or B面</param>
        private void toolStripMenuItem_AB_Click(object sender, LocateMethod locate_method, string str_side = "A") // (20190827) Jeff Revised!
        {
            ToolStripMenuItem menu = sender as ToolStripMenuItem;
            string name = menu.Text;

            // 如果原先狀態為OK啟用，則不改變狀態! (因為不知道要判定哪一種瑕疵類型)
            if (name == "OK" && menu.Checked)
                return;

            // Checked 狀態改變
            menu.Checked = !(menu.Checked);

            /* 更新人工覆判結果 */
            List<Point> DefectId_SingleCell_B = new List<Point>();
            if (str_side == "B") // 更新 locate_method_B
            {
                Point pt_B;
                Recipe_AB.Convert_coordinate_A_2_B(DefectId_SingleCell[0], out pt_B);
                DefectId_SingleCell_B.Add(pt_B);
            }

            if (name == "OK") // 點擊【OK】
            {
                locate_method.Update_All_OK_Recheck(DefectId_SingleCell);
                if (str_side == "B") // 更新 locate_method_B
                    this.locate_method_B.Update_All_OK_Recheck(DefectId_SingleCell_B);
            }
            else // 點擊其他瑕疵類型
            {
                if (menu.Checked) // 標註此種瑕疵
                {
                    locate_method.Update_Recheck(null, DefectId_SingleCell, name);
                    if (str_side == "B") // 更新 locate_method_B
                        this.locate_method_B.Update_Recheck(null, DefectId_SingleCell_B, name);
                }
                else // 解除標註此種瑕疵
                {
                    locate_method.Update_Recheck(DefectId_SingleCell, null, name);
                    if (str_side == "B") // 更新 locate_method_B
                        this.locate_method_B.Update_Recheck(DefectId_SingleCell_B, null, name);
                }
            }

            // 更新 Recipe_AB
            //this.Recipe_AB.Update_Locate_method_B_2_A_Recheck(true, this.locate_method_, this.locate_method_B); // 時間太久了!!!
            this.Recipe_AB.Update(); // 時間太久了!!!
            //this.Recipe_AB.Locate_method_A = this.locate_method_;
            //this.Recipe_AB.Locate_method_B = this.locate_method_B;

            // 設定 B_NG_Classify & 更新 NGClassify_Statistics
            this.Recipe_AB.Set_B_NG_Classify(this.cbx_B_NG_Classify.Checked); // (20200429) Jeff Revised!

            // 更新顯示
            this.Update_radioButton_defTest_tabControl();
        }

        /// <summary>
        /// 滑鼠點擊事件 (人工覆判)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hSmartWindowControl_map_HMouseDown_Recheck_AB(object sender, HMouseEventArgs e) // (20190827) Jeff Revised!
        {
            // 游標是否於 Cell region 內
            HTuple hv_Index = new HTuple();
            HOperatorSet.GetRegionIndex(CellReg_MoveIndex, (int)(e.Y + 0.5), (int)(e.X + 0.5), out hv_Index);
            if (hv_Index.Length <= 0)
            {
                txt_cellID_AB.Text = "";
                return;
            }

            HTuple cell_ids = new HTuple();
            List<Point> ListCellID = new List<Point>();
            // 計算此顆Cell座標
            if (!locate_method_.pos_2_cellID_MoveIndex(index_capture, e.Y, e.X, out cell_ids, out ListCellID))
                return;

            if (e.Button == System.Windows.Forms.MouseButtons.Left) // 滑鼠是否點擊左鍵
            {
                #region 滑鼠點擊左鍵Cell座標 (X, Y)

                // 更新顯示
                string str = "";
                foreach (Point pt in ListCellID)
                    str += "(" + pt.X + ", " + pt.Y + ")";
                txt_cellID_AB.Text = str;
                Update_radioButton_defTest_tabControl();

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

                // 【單顆Cell影像】
                Point_CellImage = ListCellID[0];
                txt_CellImage_cellID.Text = "(" + Point_CellImage.X + ", " + Point_CellImage.Y + ")";
                Recipe_AB.Convert_coordinate_A_2_B(Point_CellImage, out Point_CellImage_B);
                if (radioButton_DispA_CellImage.Checked) // 【A面】
                    Display_CellImage(locate_method_);
                else // 【B面】
                    Display_CellImage(locate_method_B);

                #endregion

                return;
            }

            if (e.Button != System.Windows.Forms.MouseButtons.Right) // 滑鼠是否點擊右鍵
                return;

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
            // A面
            Update_toolStripMenuItem(toolStripMenuItem_A, this.locate_method_);
            // B面
            Update_toolStripMenuItem(toolStripMenuItem_B, this.locate_method_B_2_A);

            /* 顯示上下文選單 */
            {
                HTuple rowWindow, columnWindow;
                HOperatorSet.ConvertCoordinatesImageToWindow(hSmartWindowControl_map.HalconWindow, e.Y, e.X, out rowWindow, out columnWindow);
                contextMenuStrip_Recheck_AB.Show(hSmartWindowControl_map, (int)(columnWindow.D + 0.5), (int)(rowWindow.D + 0.5));
            }

            #endregion

        }

        private void Update_toolStripMenuItem(ToolStripMenuItem toolStripMenuItem, LocateMethod locate_method) // (20190827) Jeff Revised!
        {
            if (!locate_method.b_Defect_Classify) // 單一瑕疵
            {
                if (locate_method.all_defect_id_Recheck.Contains(DefectId_SingleCell[0])) // NG Cell
                {
                    ((ToolStripMenuItem)toolStripMenuItem.DropDownItems[0]).Checked = false;
                    ((ToolStripMenuItem)toolStripMenuItem.DropDownItems[0]).Checked = true;
                }
                else // OK Cell
                {
                    ((ToolStripMenuItem)toolStripMenuItem.DropDownItems[0]).Checked = true;
                    ((ToolStripMenuItem)toolStripMenuItem.DropDownItems[0]).Checked = false;
                }
            }
            else // 多瑕疵
            {
                // 判斷各瑕疵類型
                bool b_OKCell = true;
                int i = 0;
                foreach (string name in locate_method.DefectsClassify.Keys)
                {
                    i++;
                    if (locate_method.DefectsClassify[name].all_defect_id_Recheck.Contains(DefectId_SingleCell[0])) // 包含此瑕疵類型
                    {
                        ((ToolStripMenuItem)toolStripMenuItem.DropDownItems[i]).Checked = true;
                        b_OKCell = false;
                    }
                    else
                        ((ToolStripMenuItem)toolStripMenuItem.DropDownItems[i]).Checked = false;
                }

                if (b_OKCell) // OK Cell
                    ((ToolStripMenuItem)toolStripMenuItem.DropDownItems[0]).Checked = true;
                else // NG Cell
                    ((ToolStripMenuItem)toolStripMenuItem.DropDownItems[0]).Checked = false;
            }
        }
        
        /// <summary>
        /// B面翻轉方式切換
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbx_B_invert_CheckedChanged(object sender, EventArgs e) // (20190826) Jeff Revised!
        {
            if (this.locate_method_B.TileImage == null)
                return;

            //Recipe_AB.Set_B_invert(cbx_B_UpDown_invert.Checked, cbx_B_LeftRight_invert.Checked);
            //Recipe_AB.Initialize_Locate_method_B_2_A(false);
            //Update_radioButton_defTest_tabControl();
            //HOperatorSet.SetPart(hSmartWindowControl_map.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part

            if (bw_B_invert.IsBusy != true)
            {
                this.tabControl_BigMap.Enabled = false;

                bw_B_invert.RunWorkerAsync();

                ctrl_timer1 = sender as Control;
                BackColor_ctrl_timer1_1 = ctrl_timer1.BackColor;
                BackColor_ctrl_timer1_2 = Color.Green;
                timer1.Enabled = true;
            }
        }

        private void bw_DoWork_B_invert(object sender, DoWorkEventArgs e) // (20200429) Jeff Revised!
        {
            BackgroundWorker Worker = (BackgroundWorker)sender;
            //clsProgressbar m_ProgressBar = new clsProgressbar();
            if (this.m_ProgressBar == null) // (20200429) Jeff Revised!
            {
                this.m_ProgressBar = new clsProgressbar();

                this.m_ProgressBar.FormClosedEvent2 += new clsProgressbar.FormClosedHandler2(SetFormClosed2);

                this.m_ProgressBar.SetShowText("請等待B面翻轉......");
                this.m_ProgressBar.SetShowCaption("執行中......");
                this.m_ProgressBar.ShowWaitProgress();
            }

            Recipe_AB.Set_B_invert(cbx_B_UpDown_invert.Checked, cbx_B_LeftRight_invert.Checked);
            Recipe_AB.Initialize_Locate_method_B_2_A(false);
            e.Result = "Success";

            //m_ProgressBar.CloseProgress();

            if (bw_B_invert.CancellationPending == true) // 判斷是否停止BackgroundWorker執行
            {
                e.Cancel = true;
                return;
            }
        }

        private void bw_RunWorkerCompleted_B_invert(object sender, RunWorkerCompletedEventArgs e) // (20190826) Jeff Revised!
        {
            if (e.Cancelled == true)
            {

            }
            else if (e.Error != null)
            {

            }
            else if (e.Result != null && e.Result.ToString() == "Exception")
            {

            }
            else if (e.Result != null && e.Result.ToString() == "Success")
            {
                this.Update_radioButton_defTest_tabControl();
                HOperatorSet.SetPart(hSmartWindowControl_map.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part
            }
            else
                return;

            timer1.Enabled = false;
            ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
            this.tabControl_BigMap.Enabled = true;

            this.m_ProgressBar.CloseProgress();
            this.m_ProgressBar = null;
        }

        /// <summary>
        /// 更新 hSmartWindowControl_map (【雙面統計結果合併】)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_DefDispAB_CheckedChanged(object sender, EventArgs e) // (20200429) Jeff Revised!
        {
            if (sender != null)
            {
                RadioButton rbtn = sender as RadioButton;
                if (rbtn.Checked == false) return;
            }

            if (this.radioButton_A_AB.Checked) // 【A面】
                LocateMethod.Disp_DefectTest(hSmartWindowControl_map, this.locate_method_, null, this.radioButton_Result, this.radioButton_Recheck, this.radioButton_fill, this.radioButton_margin); // (20200429) Jeff Revised!
            else if (this.radioButton_B_AB.Checked) // 【B面】
                LocateMethod.Disp_DefectTest(hSmartWindowControl_map, this.locate_method_B_2_A, null, this.radioButton_Result, this.radioButton_Recheck, this.radioButton_fill_B, this.radioButton_margin_B); // (20200429) Jeff Revised!
            else if (this.radioButton_AB_AB.Checked) // 【合併】
            {
                LocateMethod.Disp_DefectTest(hSmartWindowControl_map, this.locate_method_, null, this.radioButton_Result, this.radioButton_Recheck, this.radioButton_fill, this.radioButton_margin); // (20200429) Jeff Revised!
                LocateMethod.Disp_DefectTest(hSmartWindowControl_map, this.locate_method_B_2_A, null, this.radioButton_Result, this.radioButton_Recheck, this.radioButton_fill_B, this.radioButton_margin_B, false); // (20200429) Jeff Revised!
            }
            else if (this.radioButton_orig_AB.Checked) // 【原圖】
            {
                HOperatorSet.ClearWindow(hSmartWindowControl_map.HalconWindow);
                HOperatorSet.DispObj(this.locate_method_.TileImage, hSmartWindowControl_map.HalconWindow);
                // 【顯示每顆Cell區域】
                if (this.locate_method_.cellmap_affine_ != null)
                {
                    HOperatorSet.SetDraw(hSmartWindowControl_map.HalconWindow, "margin");
                    HOperatorSet.SetColor(hSmartWindowControl_map.HalconWindow, "green");
                    HOperatorSet.DispObj(this.locate_method_.cellmap_affine_, hSmartWindowControl_map.HalconWindow);
                }
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

        private void listView_Result_AB_SelectedIndexChanged(object sender, EventArgs e) // (20190826) Jeff Revised!
        {
            if (listView_Result_AB.SelectedIndices.Count <= 0)
                return;

            switch (listView_Result_AB.SelectedItems[0].Text)
            {
                case "A面":
                    radioButton_A_AB.Checked = true;
                    break;
                case "B面":
                    radioButton_B_AB.Checked = true;
                    break;
                case "雙面合併":
                    radioButton_AB_AB.Checked = true;
                    break;
            }
        }

        #endregion



        #region 【AI分圖】

        /// <summary>
        /// 【儲存】(AI分圖)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Save_AICellImg_Click(object sender, EventArgs e) // (20190902) Jeff Revised!
        {
            if (string.IsNullOrEmpty(this.textBox_Path_AICellImg.Text))
            {
                SystemSounds.Exclamation.Play();
                MessageBox.Show("路徑無效!", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.textBox_Path_AICellImg.Text = "";
                return;
            }

            //string titleName_image = SBID;
            string titleName_image = PartNumber;

            if (this.radioButton_Multi_AICellImg.Checked) //【多顆】
            {
                if (this.AICellImg_Recipe.Save_1LabelType_AICellImg(this.locate_method_, enu_Label_AICellImg.OK, this.Path_AICellImage, titleName_image, ListMapItem) &&
                    this.AICellImg_Recipe.Save_1LabelType_AICellImg(this.locate_method_, enu_Label_AICellImg.NG, this.Path_AICellImage, titleName_image, ListMapItem) &&
                    clsStaticTool.SaveXML(this.AICellImg_Recipe, this.Path_AICellImage + "AICellImg_Recipe.xml")) // (20190906) Jeff Revised!
                {

                    MessageBox.Show("儲存成功!");
                }
                else
                {
                    SystemSounds.Exclamation.Play();
                    MessageBox.Show("儲存失敗!", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else //【單顆】
            {
                if (Point_CellImage.X < 0 || Point_CellImage.Y < 0)
                {
                    SystemSounds.Exclamation.Play();
                    MessageBox.Show("請選擇Cell!", "溫馨提醒", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 判斷單顆Cell AI分圖類型
                enu_Label_AICellImg labelType;
                if (AICellImg_Recipe.CellId_OK.Contains(Point_CellImage))
                    labelType = enu_Label_AICellImg.OK;
                else if (AICellImg_Recipe.CellId_NG.Contains(Point_CellImage))
                    labelType = enu_Label_AICellImg.NG;
                else
                    labelType = enu_Label_AICellImg.NONE;

                // 儲存
                List<Point> ListPt_specificCell = new List<Point>();
                ListPt_specificCell.Add(Point_CellImage);
                if (AICellImg_Recipe.Save_1LabelType_AICellImg(this.locate_method_, labelType, Path_AICellImage, titleName_image, ListMapItem, true, ListPt_specificCell))
                {
                    MessageBox.Show("儲存成功!");
                }
                else
                {
                    SystemSounds.Exclamation.Play();
                    MessageBox.Show("儲存失敗!", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        /// <summary>
        /// 【另存】(AI分圖)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_SaveAs_AICellImg_Click(object sender, EventArgs e) // (20190902) Jeff Revised!
        {
            FolderBrowserDialog Dilg = new FolderBrowserDialog();
            Dilg.Description = "【另存】(AI分圖)"; // (20200429) Jeff Revised!
            if (Dilg.ShowDialog() != DialogResult.OK)
                return;

            Path_AICellImage = Dilg.SelectedPath + "\\";
            this.textBox_Path_AICellImg.Text = Path_AICellImage;

            button_Save_AICellImg_Click(sender, e);
        }

        /// <summary>
        /// 【載入】(AI分圖)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Load_AICellImg_Click(object sender, EventArgs e) // (20190906) Jeff Revised!
        {
            FolderBrowserDialog Dilg = new FolderBrowserDialog();
            Dilg.Description = "【載入】(AI分圖)"; // (20200429) Jeff Revised!
            if (Dilg.ShowDialog() != DialogResult.OK)
                return;

            Path_AICellImage = Dilg.SelectedPath + "\\";
            this.textBox_Path_AICellImg.Text = Path_AICellImage;

            if (string.IsNullOrEmpty(Path_AICellImage))
            {
                SystemSounds.Exclamation.Play();
                MessageBox.Show("路徑無效!", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.textBox_Path_AICellImg.Text = "";
                Path_AICellImage = "";
                return;
            }

            AICellImg_Recipe.Release();
            if (cls_AICellImg_Recipe.Load(Path_AICellImage, locate_method_, out AICellImg_Recipe))
            {
                #region 更新GUI

                b_LoadRecipe_AICellImg = true;

                nudDAVSImageId.Value = decimal.Parse(AICellImg_Recipe.DAVSImgID.ToString());
                cbxDAVSImgAlignEnabled.Checked = AICellImg_Recipe.b_DAVSImgAlign;
                cbxDAVSMixBandEnabled.Checked = AICellImg_Recipe.b_DAVSMixImgBand;
                nudDAVSBand1ImageIndex.Value = decimal.Parse(AICellImg_Recipe.DAVSBand1ImgIndex.ToString());
                nudDAVSBand2ImageIndex.Value = decimal.Parse(AICellImg_Recipe.DAVSBand2ImgIndex.ToString());
                nudDAVSBand3ImageIndex.Value = decimal.Parse(AICellImg_Recipe.DAVSBand3ImgIndex.ToString());
                comboBoxDAVSBand1.SelectedIndex = (int)AICellImg_Recipe.DAVSBand1;
                comboBoxDAVSBand2.SelectedIndex = (int)AICellImg_Recipe.DAVSBand2;
                comboBoxDAVSBand3.SelectedIndex = (int)AICellImg_Recipe.DAVSBand3;

                b_LoadRecipe_AICellImg = false;

                #endregion

                // 更新 hSmartWindowControl_map
                Display_AICellImg(locate_method_, AICellImg_Recipe);

                // 更新 listView_AICellImg
                Update_listView_AICellImg();

            }
            else
            {
                SystemSounds.Exclamation.Play();
                MessageBox.Show("【載入】失敗!", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 【人員分圖】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbx_Label_AICellImg_CheckedChanged(object sender, EventArgs e) // (20190902) Jeff Revised!
        {
            CheckBox Obj = (CheckBox)sender;

            if (Obj.Checked) // 人員分圖
            {
                try
                {
                    contextMenuStrip_Recheck.Items.Clear();
                    contextMenuStrip_Recheck.Items.Add(GetMenuItem_AICellImg(locate_method_, enu_Label_AICellImg.OK, "OK", Color.Green));
                    contextMenuStrip_Recheck.Items.Add(GetMenuItem_AICellImg(locate_method_, enu_Label_AICellImg.NG, "NG", Color.Red));
                    contextMenuStrip_Recheck.Items.Add(GetMenuItem_AICellImg(locate_method_, enu_Label_AICellImg.NONE, "NONE", Color.Blue));

                    // 強制勾選【顯示資訊】為【AI分圖結果】
                    radioButton_Recheck.Checked = true;

                    // 大圖上標註
                    index_capture = -1;
                    Extension.HObjectMedthods.ReleaseHObject(ref Image_MoveIndex);
                    HOperatorSet.CopyObj(this.locate_method_.TileImage, out Image_MoveIndex, 1, -1);

                    /* 計算此位置Cell */
                    Extension.HObjectMedthods.ReleaseHObject(ref CellReg_MoveIndex);
                    CellReg_MoveIndex = locate_method_.cellmap_affine_.Clone();
                    Extension.HObjectMedthods.ReleaseHObject(ref CellReg_MoveIndex_Dila);
                    HOperatorSet.DilationCircle(CellReg_MoveIndex, out CellReg_MoveIndex_Dila, 5);

                    // 更新 hSmartWindowControl_map
                    Display_AICellImg(locate_method_, AICellImg_Recipe, true);
                    
                    Obj.ForeColor = Color.GreenYellow;
                    Obj.Text = "設定完成";
                    ctrl_timer1 = Obj;
                    BackColor_ctrl_timer1_1 = Color.DarkViolet;
                    BackColor_ctrl_timer1_2 = Color.Lime;
                    timer1.Enabled = true;
                    
                    button_LoadCellImage.Enabled = false;

                    // 禁能滑鼠點擊事件 (原本)
                    this.hSmartWindowControl_map.HMouseDown -= new HalconDotNet.HMouseEventHandler(this.hSmartWindowControl_map_HMouseDown);
                    // 致能滑鼠點擊事件 【AI分圖】
                    this.hSmartWindowControl_map.HMouseDown += new HalconDotNet.HMouseEventHandler(this.hSmartWindowControl_map_HMouseDown_AICellImg);

                    // 致能按鍵事件 【AI分圖】 (contextMenuStrip_Recheck)
                    this.contextMenuStrip_Recheck.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.contextMenuStrip_Recheck_PreviewKeyDown);
                }
                catch (Exception ex)
                {
                    #region CheckBox狀態復原
                    cbx_Label_AICellImg.CheckedChanged -= cbx_Label_AICellImg_CheckedChanged;
                    Obj.Checked = false;
                    cbx_Label_AICellImg.CheckedChanged += cbx_Label_AICellImg_CheckedChanged;
                    #endregion

                    MessageBox.Show(ex.ToString(), "AI分圖標註失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

            }
            else // 設定完成
            {
                timer1.Enabled = false;
                ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
                Obj.ForeColor = Color.White;
                Obj.Text = "人員分圖";

                button_LoadCellImage.Enabled = true;

                // 禁能滑鼠點擊事件 【AI分圖】
                this.hSmartWindowControl_map.HMouseDown -= new HalconDotNet.HMouseEventHandler(this.hSmartWindowControl_map_HMouseDown_AICellImg);
                // 致能滑鼠點擊事件 (原本)
                this.hSmartWindowControl_map.HMouseDown += new HalconDotNet.HMouseEventHandler(this.hSmartWindowControl_map_HMouseDown);

                // 禁能按鍵事件 【AI分圖】 (contextMenuStrip_Recheck)
                this.contextMenuStrip_Recheck.PreviewKeyDown -= new System.Windows.Forms.PreviewKeyDownEventHandler(this.contextMenuStrip_Recheck_PreviewKeyDown);

                try
                {
                    // 更新 hSmartWindowControl_map
                    Display_AICellImg(locate_method_, AICellImg_Recipe, true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "AI分圖標註失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }

        }
        
        /// <summary>
        /// 按鍵(上下左右)控制Cell位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenuStrip_Recheck_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) // (20190902) Jeff Revised!
        {
            if (DefectId_SingleCell.Count == 0)
                return;
            
            Point pt = DefectId_SingleCell[0];
            bool b_Label = false;
            switch (e.KeyCode)
            {
                case Keys.Up:
                    pt.Y -= 1;
                    break;
                case Keys.Down:
                    pt.Y += 1;
                    break;
                case Keys.Left:
                    pt.X -= 1;
                    break;
                case Keys.Right:
                    pt.X += 1;
                    break;
                case Keys.O: // 點擊"O": 標註OK
                    {
                        b_Label = true;
                        AICellImg_Recipe.Label_cell(locate_method_, pt, enu_Label_AICellImg.OK);
                        // 更新 listView_AICellImg
                        Update_listView_AICellImg();
                    }
                    break;
                case Keys.N: // 點擊"N": 標註NG
                    {
                        b_Label = true;
                        AICellImg_Recipe.Label_cell(locate_method_, pt, enu_Label_AICellImg.NG);
                        // 更新 listView_AICellImg
                        Update_listView_AICellImg();
                    }
                    break;
                case Keys.Space: // 點擊"空白鍵": 標註NONE
                    {
                        b_Label = true;
                        AICellImg_Recipe.Label_cell(locate_method_, pt, enu_Label_AICellImg.NONE);
                        // 更新 listView_AICellImg
                        Update_listView_AICellImg();
                    }
                    break;
                default:
                    return;
            }
            
            if (pt.X >= 1 && pt.X <= locate_method_.cell_col_count_ && pt.Y >= 1 && pt.Y <= locate_method_.cell_row_count_ && !b_Label)
            {
                if (!locate_method_.LocateMethodRecipe.ListPt_BypassCell.Contains(pt))
                    ;
                else
                    pt = DefectId_SingleCell[0];
            }
            else
                pt = DefectId_SingleCell[0];

            // 計算各Cell中心座標
            int cell_id_ = locate_method_.cellID_2_int(pt);
            HObject cell;
            HOperatorSet.SelectObj(locate_method_.cellmap_affine_, out cell, cell_id_);
            HTuple x = new HTuple(), y = new HTuple();
            HOperatorSet.RegionFeatures(cell, "column", out x);
            HOperatorSet.RegionFeatures(cell, "row", out y);
            cell.Dispose();

            // 法1
            //hSmartWindowControl_map_HMouseDown_AICellImg(y.D, x.D, e_Button);

            // 法2
            y_timer_AICellImg = y.D;
            x_timer_AICellImg = x.D;
            MouseButtons e_Button = MouseButtons.Right;
            e_Button_timer_AICellImg = e_Button;
            timer_AICellImg.Enabled = true;

        }

        double y_timer_AICellImg, x_timer_AICellImg;
        MouseButtons e_Button_timer_AICellImg;
        private void timer_AICellImg_Tick(object sender, EventArgs e) // (20190906) Jeff Revised!
        {
            timer_AICellImg.Enabled = false;
            hSmartWindowControl_map_HMouseDown_AICellImg(y_timer_AICellImg, x_timer_AICellImg, e_Button_timer_AICellImg);
        }

        /// <summary>
        /// 滑鼠點擊事件 【AI分圖】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hSmartWindowControl_map_HMouseDown_AICellImg(object sender, HMouseEventArgs e) // (20190902) Jeff Revised!
        {
            int Method = 2;
            if (Method == 1)
            {
                #region Method 1

                // 游標是否於 Cell region 內
                HTuple hv_Index = new HTuple();
                HOperatorSet.GetRegionIndex(locate_method_.cellmap_affine_, (int)(e.Y + 0.5), (int)(e.X + 0.5), out hv_Index);
                if (hv_Index.Length <= 0)
                {
                    txt_cellID_AICellImg.Text = "";
                    return;
                }

                List<Point> ListCellID = new List<Point>();
                // 計算此顆Cell座標
                if (!locate_method_.pos_2_cellID_MoveIndex(-1, e.Y, e.X, out cell_id, out ListCellID))
                    return;

                if (e.Button == System.Windows.Forms.MouseButtons.Left || e.Button == System.Windows.Forms.MouseButtons.Right) // 滑鼠是否點擊左鍵 or 右鍵
                {
                    Point_CellImage = ListCellID[0];
                    txt_cellID_AICellImg.Text = "(" + Point_CellImage.X + ", " + Point_CellImage.Y + ")";

                    // 【單顆Cell影像】
                    txt_CellImage_cellID.Text = "(" + Point_CellImage.X + ", " + Point_CellImage.Y + ")";
                    if (radioButton_DispA_CellImage.Checked) // 【A面】
                        Display_CellImage(locate_method_);
                    else
                        HOperatorSet.ClearWindow(hSmartWindowControl_CellImage.HalconWindow);

                    // 更新顯示 (hSmartWindowControl_AICellImg) 【AI分圖】
                    HObject cell;
                    HOperatorSet.SelectObj(locate_method_.cellmap_affine_, out cell, cell_id[0]);
                    HTuple x = new HTuple(), y = new HTuple();
                    HOperatorSet.RegionFeatures(cell, "column", out x);
                    HOperatorSet.RegionFeatures(cell, "row", out y);
                    cell.Dispose();
                    posBigMapCellCenter_AICellImg = new PointF((float)(x.D), (float)(y.D));
                    clsStaticTool.DisposeAll(ListCellReg_MoveIndex_AICellImg);
                    ListCellReg_MoveIndex_AICellImg.Clear();
                    locate_method_.posBigMapCellCenter_2_MoveIndex(posBigMapCellCenter_AICellImg, out ListMoveIndex_AICellImg, out ListCellReg_MoveIndex_AICellImg);
                    b_LoadRecipe_AICellImg = true;
                    comboBox_MoveIndex.Items.Clear();
                    foreach (int i in ListMoveIndex_AICellImg)
                        comboBox_MoveIndex.Items.Add(i);
                    if (comboBox_MoveIndex.Items.Count > 0) // (20191214) Jeff Revised!
                        comboBox_MoveIndex.SelectedIndex = 0;
                    b_LoadRecipe_AICellImg = false;
                    this.Display_AICellImg_1Cell();

                    // 更新 hSmartWindowControl_map
                    Display_AICellImg(locate_method_, AICellImg_Recipe);

                    #region 滑鼠點擊左鍵Cell座標 (X, Y)

                    if (e.Button == System.Windows.Forms.MouseButtons.Left) // 滑鼠是否點擊左鍵
                    {
                        // 顯示上下文選單
                        toolStripMenuItem_ID.Text = txt_cellID_AICellImg.Text;
                        HTuple rowWindow, columnWindow;
                        HOperatorSet.ConvertCoordinatesImageToWindow(hSmartWindowControl_map.HalconWindow, e.Y, e.X, out rowWindow, out columnWindow);
                        contextMenuStrip_DispID.Show(hSmartWindowControl_map, (int)(columnWindow.D + 0.5), (int)(rowWindow.D + 0.5));

                        return;
                    }

                    #endregion

                }

                if (e.Button != System.Windows.Forms.MouseButtons.Right) // 滑鼠是否點擊右鍵
                    return;

                #region 單顆標註模式

                DefectId_SingleCell.Clear();
                DefectId_SingleCell.Add(ListCellID[0]);

                /* 更新上下文選單 */
                if (AICellImg_Recipe.CellId_OK.Contains(ListCellID[0]))
                    ((ToolStripMenuItem)contextMenuStrip_Recheck.Items[0]).Checked = true;
                else
                    ((ToolStripMenuItem)contextMenuStrip_Recheck.Items[0]).Checked = false;
                if (AICellImg_Recipe.CellId_NG.Contains(ListCellID[0]))
                    ((ToolStripMenuItem)contextMenuStrip_Recheck.Items[1]).Checked = true;
                else
                    ((ToolStripMenuItem)contextMenuStrip_Recheck.Items[1]).Checked = false;
                if (AICellImg_Recipe.CellId_NONE.Contains(ListCellID[0]))
                    ((ToolStripMenuItem)contextMenuStrip_Recheck.Items[2]).Checked = true;
                else
                    ((ToolStripMenuItem)contextMenuStrip_Recheck.Items[2]).Checked = false;

                /* 顯示上下文選單 */
                {
                    HTuple rowWindow, columnWindow;
                    HOperatorSet.ConvertCoordinatesImageToWindow(hSmartWindowControl_map.HalconWindow, e.Y, e.X, out rowWindow, out columnWindow);
                    contextMenuStrip_Recheck.Show(hSmartWindowControl_map, (int)(columnWindow.D + 0.5), (int)(rowWindow.D + 0.5));
                }

                #endregion

                #endregion
            }
            else if (Method == 2)
            {
                hSmartWindowControl_map_HMouseDown_AICellImg(e.Y, e.X, e.Button);
            }

        }

        private void hSmartWindowControl_map_HMouseDown_AICellImg(double e_y, double e_x, MouseButtons e_Button) // (20190902) Jeff Revised!
        {
            // 游標是否於 Cell region 內
            HTuple hv_Index = new HTuple();
            HOperatorSet.GetRegionIndex(locate_method_.cellmap_affine_, (int)(e_y + 0.5), (int)(e_x + 0.5), out hv_Index);
            if (hv_Index.Length <= 0)
            {
                txt_cellID_AICellImg.Text = "";
                return;
            }

            List<Point> ListCellID = new List<Point>();
            // 計算此顆Cell座標
            if (!locate_method_.pos_2_cellID_MoveIndex(-1, e_y, e_x, out cell_id, out ListCellID))
                return;

            if (e_Button == System.Windows.Forms.MouseButtons.Left || e_Button == System.Windows.Forms.MouseButtons.Right) // 滑鼠是否點擊左鍵 or 右鍵
            {
                Point_CellImage = ListCellID[0];
                txt_cellID_AICellImg.Text = "(" + Point_CellImage.X + ", " + Point_CellImage.Y + ")";

                // 【單顆Cell影像】
                txt_CellImage_cellID.Text = "(" + Point_CellImage.X + ", " + Point_CellImage.Y + ")";
                if (radioButton_DispA_CellImage.Checked) // 【A面】
                    Display_CellImage(locate_method_);
                else
                    HOperatorSet.ClearWindow(hSmartWindowControl_CellImage.HalconWindow);

                // 更新顯示 (hSmartWindowControl_AICellImg) 【AI分圖】
                HObject cell;
                HOperatorSet.SelectObj(locate_method_.cellmap_affine_, out cell, cell_id[0]);
                HTuple x = new HTuple(), y = new HTuple();
                HOperatorSet.RegionFeatures(cell, "column", out x);
                HOperatorSet.RegionFeatures(cell, "row", out y);
                cell.Dispose();
                posBigMapCellCenter_AICellImg = new PointF((float)(x.D), (float)(y.D));
                clsStaticTool.DisposeAll(ListCellReg_MoveIndex_AICellImg);
                ListCellReg_MoveIndex_AICellImg.Clear();
                locate_method_.posBigMapCellCenter_2_MoveIndex(posBigMapCellCenter_AICellImg, out ListMoveIndex_AICellImg, out ListCellReg_MoveIndex_AICellImg);
                b_LoadRecipe_AICellImg = true;
                comboBox_MoveIndex.Items.Clear();
                foreach (int i in ListMoveIndex_AICellImg)
                    comboBox_MoveIndex.Items.Add(i);
                if (comboBox_MoveIndex.Items.Count > 0) // (20191214) Jeff Revised!
                    comboBox_MoveIndex.SelectedIndex = 0;
                b_LoadRecipe_AICellImg = false;
                this.Display_AICellImg_1Cell();

                // 更新 hSmartWindowControl_map
                Display_AICellImg(locate_method_, AICellImg_Recipe);

                #region 滑鼠點擊左鍵Cell座標 (X, Y)

                if (e_Button == System.Windows.Forms.MouseButtons.Left) // 滑鼠是否點擊左鍵
                {
                    // 顯示上下文選單
                    toolStripMenuItem_ID.Text = txt_cellID_AICellImg.Text;
                    HTuple rowWindow, columnWindow;
                    HOperatorSet.ConvertCoordinatesImageToWindow(hSmartWindowControl_map.HalconWindow, e_y, e_x, out rowWindow, out columnWindow);
                    contextMenuStrip_DispID.Show(hSmartWindowControl_map, (int)(columnWindow.D + 0.5), (int)(rowWindow.D + 0.5));

                    return;
                }

                #endregion

            }

            if (e_Button != System.Windows.Forms.MouseButtons.Right) // 滑鼠是否點擊右鍵
                return;

            #region 單顆標註模式

            DefectId_SingleCell.Clear();
            DefectId_SingleCell.Add(ListCellID[0]);

            /* 更新上下文選單 */
            if (AICellImg_Recipe.CellId_OK.Contains(ListCellID[0]))
                ((ToolStripMenuItem)contextMenuStrip_Recheck.Items[0]).Checked = true;
            else
                ((ToolStripMenuItem)contextMenuStrip_Recheck.Items[0]).Checked = false;
            if (AICellImg_Recipe.CellId_NG.Contains(ListCellID[0]))
                ((ToolStripMenuItem)contextMenuStrip_Recheck.Items[1]).Checked = true;
            else
                ((ToolStripMenuItem)contextMenuStrip_Recheck.Items[1]).Checked = false;
            if (AICellImg_Recipe.CellId_NONE.Contains(ListCellID[0]))
                ((ToolStripMenuItem)contextMenuStrip_Recheck.Items[2]).Checked = true;
            else
                ((ToolStripMenuItem)contextMenuStrip_Recheck.Items[2]).Checked = false;

            /* 顯示上下文選單 */
            {
                HTuple rowWindow, columnWindow;
                HOperatorSet.ConvertCoordinatesImageToWindow(hSmartWindowControl_map.HalconWindow, e_y, e_x, out rowWindow, out columnWindow);
                contextMenuStrip_Recheck.Show(hSmartWindowControl_map, (int)(columnWindow.D + 0.5), (int)(rowWindow.D + 0.5));
            }

            #endregion
        }

        /// <summary>
        /// 生成選單項
        /// </summary>
        /// <param name="locate_method"></param>
        /// <param name="labelType"></param>
        /// <param name="txt"></param>
        /// <param name="ForeColor"></param>
        /// <param name="img"></param>
        /// <returns></returns>
        private ToolStripMenuItem GetMenuItem_AICellImg(LocateMethod locate_method, enu_Label_AICellImg labelType, string txt, object ForeColor = null, Image img = null) // (20190902) Jeff Revised!
        {
            ToolStripMenuItem menuItem = new ToolStripMenuItem();

            // 屬性設定
            menuItem.Text = txt;
            if (ForeColor is Color)
                menuItem.ForeColor = (Color)ForeColor;
            if (img != null)
                menuItem.Image = img;

            // 點擊觸發事件設定
            menuItem.Click += (sender, e) => toolStripMenuItem_AICellImg_Click(sender, locate_method, labelType);

            return menuItem;
        }

        /// <summary>
        /// 選單項事件響應 (有參數傳入)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="locate_method"></param>
        /// <param name="str_side"></param>
        private void toolStripMenuItem_AICellImg_Click(object sender, LocateMethod locate_method, enu_Label_AICellImg labelType) // (20190902) Jeff Revised!
        {
            ToolStripMenuItem menu = sender as ToolStripMenuItem;
            string name = menu.Text;

            // 如果原先狀態為啟用，則不改變狀態! (因為不知道要判定哪一種AI分圖標註種類)
            if (menu.Checked)
                return;

            // Checked 狀態改變為true
            menu.Checked = true;

            /* 更新AI分圖標註結果 */
            AICellImg_Recipe.Label_cell(locate_method, DefectId_SingleCell[0], labelType);

            // 更新 hSmartWindowControl_map
            //cell_id = new HTuple();
            Display_AICellImg(locate_method, AICellImg_Recipe);

            // 更新 listView_AICellImg
            Update_listView_AICellImg();
        }

        /// <summary>
        /// 更新 listView_AICellImg
        /// </summary>
        private void Update_listView_AICellImg() // (20190902) Jeff Revised!
        {
            if (AICellImg_Recipe == null)
                return;

            listView_AICellImg.BeginUpdate();
            listView_AICellImg.Items.Clear();

            /* 資料建立 */
            Dictionary<string, int> Datas = new Dictionary<string, int>();
            Dictionary<string, string> Datas_Key = new Dictionary<string, string>(); // (20190916) Jeff Revised!
            // OK
            Datas.Add("OK", AICellImg_Recipe.CellId_OK.Count);
            Datas_Key.Add("OK", "O");
            // NG
            Datas.Add("NG", AICellImg_Recipe.CellId_NG.Count);
            Datas_Key.Add("NG", "N");
            // NONE
            Datas.Add("NONE", AICellImg_Recipe.CellId_NONE.Count);
            Datas_Key.Add("NONE", "Space");
            
            foreach (KeyValuePair<string, int> item in Datas)
            {
                ListViewItem lvi = new ListViewItem(item.Key);
                lvi.SubItems.Add(item.Value.ToString());
                lvi.SubItems.Add(Datas_Key[item.Key]);
                listView_AICellImg.Items.Add(lvi);
            }

            listView_AICellImg.EndUpdate();

        }

        /// <summary>
        /// 更新顯示 (hSmartWindowControl_AICellImg) 【AI分圖】
        /// </summary>
        /// <param name="locate_method"></param>
        /// <param name="aICellImg_Recipe"></param>
        /// <param name="b_SetPart"></param>
        private void Display_AICellImg(LocateMethod locate_method, cls_AICellImg_Recipe aICellImg_Recipe, bool b_SetPart = false) // (20190902) Jeff Revised!
        {
            if (locate_method.TileImage == null || aICellImg_Recipe == null)
                return;

            HOperatorSet.ClearWindow(hSmartWindowControl_map.HalconWindow);
            HOperatorSet.DispObj(locate_method.TileImage, hSmartWindowControl_map.HalconWindow);
            if (b_SetPart)
                HOperatorSet.SetPart(hSmartWindowControl_map.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part
            
            // 【顯示每顆Cell區域】
            if (locate_method.cellmap_affine_ != null)
            {
                HOperatorSet.SetDraw(hSmartWindowControl_map.HalconWindow, "margin");
                HOperatorSet.SetColor(hSmartWindowControl_map.HalconWindow, "green");
                HOperatorSet.DispObj(locate_method.cellmap_affine_, hSmartWindowControl_map.HalconWindow);
            }

            // 顯示【AI分圖結果】
            if (radioButton_Recheck.Checked)
            {
                if (aICellImg_Recipe.OK_CellReg != null)
                {
                    HOperatorSet.SetDraw(hSmartWindowControl_map.HalconWindow, "fill");
                    HOperatorSet.SetColor(hSmartWindowControl_map.HalconWindow, "#00ff00c0"); // 75% alpha green
                    HOperatorSet.DispObj(aICellImg_Recipe.OK_CellReg, hSmartWindowControl_map.HalconWindow);
                }
                if (aICellImg_Recipe.NG_CellReg != null)
                {
                    HOperatorSet.SetDraw(hSmartWindowControl_map.HalconWindow, "fill");
                    HOperatorSet.SetColor(hSmartWindowControl_map.HalconWindow, "#ff0000c0"); // 75% alpha red
                    HOperatorSet.DispObj(aICellImg_Recipe.NG_CellReg, hSmartWindowControl_map.HalconWindow);
                }
                if (aICellImg_Recipe.NONE_CellReg != null)
                {
                    HOperatorSet.SetDraw(hSmartWindowControl_map.HalconWindow, "fill");
                    HOperatorSet.SetColor(hSmartWindowControl_map.HalconWindow, "#0000ffc0"); // 75% alpha blue
                    HOperatorSet.DispObj(aICellImg_Recipe.NONE_CellReg, hSmartWindowControl_map.HalconWindow);
                }
            }
            
            // 【滑鼠點擊Cell座標 (X, Y):】
            if (cell_id.Length > 0)
            {
                if (locate_method.cellmap_affine_ != null)
                {
                    try
                    {
                        // 顯示此位置Cell
                        HOperatorSet.SetDraw(hSmartWindowControl_map.HalconWindow, "margin");
                        HOperatorSet.SetColor(hSmartWindowControl_map.HalconWindow, "#FF00FFFF"); // Magenta
                        HTuple line_w;
                        HOperatorSet.GetLineWidth(hSmartWindowControl_map.HalconWindow, out line_w);
                        HOperatorSet.SetLineWidth(hSmartWindowControl_map.HalconWindow, 3);
                        HObject reg;
                        HOperatorSet.SelectObj(CellReg_MoveIndex_Dila, out reg, cell_id);
                        HOperatorSet.DispObj(reg, hSmartWindowControl_map.HalconWindow);
                        reg.Dispose();
                        HOperatorSet.SetLineWidth(hSmartWindowControl_map.HalconWindow, line_w);
                    }
                    catch (Exception ex)
                    { }
                }
            }

        }

        /// <summary>
        /// 參數變更，更新顯示 (hSmartWindowControl_AICellImg) 【AI分圖資訊】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateParam_AICellImg(object sender, EventArgs e) // (20190902) Jeff Revised!
        {
            if (b_LoadRecipe_AICellImg)
                return;

            // 更新參數
            AICellImg_Recipe.DAVSImgID = int.Parse(nudDAVSImageId.Value.ToString());
            AICellImg_Recipe.b_DAVSImgAlign = cbxDAVSImgAlignEnabled.Checked;
            AICellImg_Recipe.b_DAVSMixImgBand = cbxDAVSMixBandEnabled.Checked;
            AICellImg_Recipe.DAVSBand1 = (enuBand)comboBoxDAVSBand1.SelectedIndex;
            AICellImg_Recipe.DAVSBand2 = (enuBand)comboBoxDAVSBand2.SelectedIndex;
            AICellImg_Recipe.DAVSBand3 = (enuBand)comboBoxDAVSBand3.SelectedIndex;
            AICellImg_Recipe.DAVSBand1ImgIndex = int.Parse(nudDAVSBand1ImageIndex.Value.ToString());
            AICellImg_Recipe.DAVSBand2ImgIndex = int.Parse(nudDAVSBand2ImageIndex.Value.ToString());
            AICellImg_Recipe.DAVSBand3ImgIndex = int.Parse(nudDAVSBand3ImageIndex.Value.ToString());

            this.Display_AICellImg_1Cell(false, false);
        }

        /// <summary>
        /// 更新顯示 (hSmartWindowControl_AICellImg) 【AI分圖資訊】
        /// </summary>
        /// <param name="b_SetPart">是否做 SetPart </param>
        /// <param name="b_update_LabelType">是否更新顯示該顆Cell分類</param>
        /// <param name="b_SetPart_NewWindow">是否做 SetPart (【視窗顯示】)</param>
        private void Display_AICellImg_1Cell(bool b_SetPart = true, bool b_update_LabelType = true, bool b_SetPart_NewWindow = false) // (20191023) Jeff Revised!
        {
            HOperatorSet.ClearWindow(hSmartWindowControl_AICellImg.HalconWindow);
            if (this.comboBox_MoveIndex.SelectedIndex < 0)
            {
                this.txt_LabelType_AICellImg.Text = "";
                return;
            }

            if (this.ListMapItem.Count == this.locate_method_.array_x_count_ * this.locate_method_.array_y_count_)
            {
                HObject dispImg = clsStaticTool.MixImageBand(this.ListMapItem[this.ListMoveIndex_AICellImg[this.comboBox_MoveIndex.SelectedIndex] - 1].ImgObj.Source,
                                                         this.AICellImg_Recipe.DAVSBand1, this.AICellImg_Recipe.DAVSBand2, this.AICellImg_Recipe.DAVSBand3,
                                                         this.AICellImg_Recipe.DAVSBand1ImgIndex, this.AICellImg_Recipe.DAVSBand2ImgIndex, this.AICellImg_Recipe.DAVSBand3ImgIndex,
                                                         this.AICellImg_Recipe.b_DAVSMixImgBand, this.AICellImg_Recipe.DAVSImgID, this.ListCellReg_MoveIndex_AICellImg[this.comboBox_MoveIndex.SelectedIndex]);

                HOperatorSet.DispObj(dispImg, hSmartWindowControl_AICellImg.HalconWindow);
                if (this.cbx_DispWindow_AICellImg.Checked) //【視窗顯示】(20191023) Jeff Revised!
                {
                    try
                    {
                        this.Form_DispWindow_AICellImg.Update_DispImg(dispImg, b_SetPart_NewWindow);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                dispImg.Dispose();

                if (b_SetPart)
                    HOperatorSet.SetPart(hSmartWindowControl_AICellImg.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part
            }

            // 更新顯示該顆Cell分類
            if (b_update_LabelType)
            {
                if (this.AICellImg_Recipe.CellId_OK.Contains(this.Point_CellImage))
                {
                    this.txt_LabelType_AICellImg.BackColor = Color.Green;
                    this.txt_LabelType_AICellImg.Text = "OK";
                }
                else if (this.AICellImg_Recipe.CellId_NG.Contains(this.Point_CellImage))
                {
                    this.txt_LabelType_AICellImg.BackColor = Color.Red;
                    this.txt_LabelType_AICellImg.Text = "NG";
                }
                else if (this.AICellImg_Recipe.CellId_NONE.Contains(this.Point_CellImage))
                {
                    this.txt_LabelType_AICellImg.BackColor = Color.Blue;
                    this.txt_LabelType_AICellImg.Text = "NONE";
                }
                else
                {
                    this.txt_LabelType_AICellImg.BackColor = Color.Black;
                    this.txt_LabelType_AICellImg.Text = "";
                }

                if (this.cbx_DispWindow_AICellImg.Checked) //【視窗顯示】(20191023) Jeff Revised!
                    this.Form_DispWindow_AICellImg.Update_LabelType(this.txt_LabelType_AICellImg.Text, this.txt_LabelType_AICellImg.BackColor);
            }
        }
        
        /// <summary>
        /// 【視窗顯示】(AI分圖)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbx_DispWindow_AICellImg_CheckedChanged(object sender, EventArgs e) // (20191023) Jeff Revised!
        {
            CheckBox Obj = (CheckBox)sender;

            if (Obj.Checked) // 視窗顯示
            {
                #region 視窗顯示

                try
                {
                    this.Form_DispWindow_AICellImg = new DispWindow_AICellImg();
                    this.Form_DispWindow_AICellImg.Set_Title("單顆Cell合成影像 (AI分圖資訊)"); // (20200429) Jeff Revised!
                    this.Form_DispWindow_AICellImg.Set_txt_LabelType_Visible(true); // (20200429) Jeff Revised!
                    this.Form_DispWindow_AICellImg.FormClosedEvent += new DispWindow_AICellImg.FormClosedHandler(SetFormClosed);
                    this.Form_DispWindow_AICellImg.Show();

                    this.Display_AICellImg_1Cell(false, true, true);

                    Obj.Text = "關閉視窗";
                    Obj.BackColor = Color.Red;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "視窗顯示失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                #endregion
            }
            else // 關閉視窗
            {
                #region 關閉視窗

                try
                {
                    this.Form_DispWindow_AICellImg.Close();

                    Obj.Text = "視窗顯示";
                    Obj.BackColor = Color.Green;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "關閉視窗失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                #endregion
            }
        }

        private void SetFormClosed(bool b_FormClosed) // (20200429) Jeff Revised!
        {
            if (b_FormClosed)
            {
                #region CheckBox狀態復原
                cbx_DispWindow_AICellImg.CheckedChanged -= cbx_DispWindow_AICellImg_CheckedChanged;
                cbx_DispWindow_AICellImg.Checked = false;
                cbx_DispWindow_AICellImg.CheckedChanged += cbx_DispWindow_AICellImg_CheckedChanged;
                #endregion

                cbx_DispWindow_AICellImg.Text = "視窗顯示";
                cbx_DispWindow_AICellImg.BackColor = Color.Green;
            }
        }

        /// <summary>
        /// 更新影像顯示 (hSmartWindowControl_AICellImg) 【AI分圖資訊】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hSmartWindowControl_AICellImg_Paint(object sender, PaintEventArgs e) // (20191023) Jeff Revised!
        {
            //if (cbx_DispWindow_AICellImg.Checked) //【視窗顯示】
            //{
            //    try
            //    {
            //        Form_DispWindow_AICellImg.Update_DispImg();
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        return;
            //    }
            //}
        }

        #endregion



        #region 【單顆Cell影像】

        private Point Point_CellImage { get; set; } = new Point(-1, -1);
        private Point Point_CellImage_B = new Point(-1, -1); // (20190826) Jeff Revised!

        /// <summary>
        /// 【視窗顯示】(單顆Cell影像) (20200429) Jeff Revised!
        /// </summary>
        private DispWindow_AICellImg Form_DispWindow_CellImage = new DispWindow_AICellImg();

        /// <summary>
        /// 【載入】所有/瑕疵 Cell影像
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_LoadCellImage_Click(object sender, EventArgs e) // (20200429) Jeff Revised!
        {
            if (this.radioButton_DispB_CellImage.Checked) // (20190817) Jeff Revised!
                return;

            if (this.ListMapItem.Count == this.locate_method_.array_x_count_ * this.locate_method_.array_y_count_)
            {
                bool b_status_ = false;
                int Method = 2; // Method: 1 (利用Cell Map抓取Cell影像，容易受大圖拼接偏移影響，但速度快!), 2 (利用檢測演算法抓到的Cell Region抓取Cell影像，速度慢，但較準確!)
                
                if (this.radioButton_LoadAllCellImage.Checked) // 【所有Cell影像】
                {
                    this.locate_method_.Cls_CellImage.Type_CellImage = enu_Type_CellImage.AllCellImage;
                    if (Method == 1)
                        b_status_ = this.locate_method_.Compute_Dictionary_AllCellImage(this.ListMapItem);
                    else
                        b_status_ = this.locate_method_.Compute_Dictionary_CellImage_From_CellReg_MoveIndex(this.ListMapItem);
                }
                else if (this.radioButton_LoadDefectCellImage.Checked) // 【瑕疵Cell影像】
                {
                    this.locate_method_.Cls_CellImage.Type_CellImage = enu_Type_CellImage.DefectCellImage;
                    if (Method == 1)
                    {
                        Dictionary<Point, clsHalconRegion> dictionary_CellImage;
                        if (this.locate_method_.compute_dictionary_cellImage_From_ConcatCellIndex(ListMapItem, this.locate_method_.ListCellID_2_HTuple(this.locate_method_.all_defect_id_), out dictionary_CellImage))
                        {
                            this.locate_method_.Dictionary_CellImage = dictionary_CellImage;
                            this.locate_method_.Cls_CellImage.B_CellImage_Done = true;
                            this.locate_method_.Cls_CellImage.CountImg_1Cell = ListMapItem[0].ImgObj.Source.Count;
                            b_status_ = true;
                        }
                        else
                        {
                            foreach (clsHalconRegion cls in dictionary_CellImage.Values)
                            {
                                if (cls != null)
                                    cls.Dispose();
                            }
                            dictionary_CellImage.Clear();
                        }
                    }
                    else
                        b_status_ = this.locate_method_.Compute_Dictionary_CellImage_From_CellReg_MoveIndex(this.ListMapItem, false, this.locate_method_.all_defect_id_);
                }

                if (b_status_)
                {
                    MessageBox.Show("載入成功!");
                    Initialize_comboBox_LightImage(locate_method_);
                }
                else
                {
                    SystemSounds.Exclamation.Play();
                    MessageBox.Show("載入失敗!", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

        }
        
        /// <summary>
        /// 初始化 comboBox_LightImage
        /// </summary>
        private void Initialize_comboBox_LightImage(LocateMethod locate_method) // (20190822) Jeff Revised!
        {
            if (ListMapItem.Count == locate_method.array_x_count_ * locate_method.array_y_count_ || locate_method.Cls_CellImage.B_CellImage_Done)
            {
                if (ListMapItem.Count != locate_method.array_x_count_ * locate_method.array_y_count_ && locate_method.Cls_CellImage.B_CellImage_Done)
                    button_LoadCellImage.Enabled = false;

                if (locate_method.Cls_CellImage.B_CellImage_Done)
                {
                    if (locate_method.Cls_CellImage.Type_CellImage == enu_Type_CellImage.AllCellImage) //【所有Cell影像】
                        radioButton_LoadAllCellImage.Checked = true;
                    else if (locate_method.Cls_CellImage.Type_CellImage == enu_Type_CellImage.DefectCellImage) //【瑕疵Cell影像】
                        radioButton_LoadDefectCellImage.Checked = true;

                    comboBox_LightImage.Enabled = true;
                    //int nowLightCount = ListMapItem[0].PicList.Count;
                    int nowLightCount = locate_method.Cls_CellImage.CountImg_1Cell;
                    comboBox_LightImage.Items.Clear();
                    for (int i = 0; i < nowLightCount; i++)
                    {
                        string lightName = "Light " + (i + 1).ToString();
                        comboBox_LightImage.Items.Add(lightName);
                    }
                    if (comboBox_LightImage.Items.Count > 0)
                        comboBox_LightImage.SelectedIndex = 0;
                }
                else
                    comboBox_LightImage.Enabled = false;
            }
            else
            {
                button_LoadCellImage.Enabled = false;
                comboBox_LightImage.Enabled = false;
            }
        }
        
        /// <summary>
        /// 【A面】, 【B面】顯示切換 (【單顆Cell影像】)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_DispCellImage_CheckedChanged(object sender, EventArgs e) // (20200429) Jeff Revised!
        {
            RadioButton rbtn = sender as RadioButton;
            if (rbtn.Checked == false) return;

            if (this.radioButton_DispA_CellImage.Checked) //【A面】
                this.Initialize_comboBox_LightImage(locate_method_);
            else if (this.radioButton_DispB_CellImage.Checked) //【B面】
                this.Initialize_comboBox_LightImage(locate_method_B);

            this.comboBox_LightImage_SelectedIndexChanged(sender, e);
        }

        /// <summary>
        /// 【原圖】, 【瑕疵圖】顯示切換 (【單顆Cell影像】)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_DispOrigDef_CheckedChanged(object sender, EventArgs e) // (20200429) Jeff Revised!
        {
            RadioButton rbtn = sender as RadioButton;
            if (rbtn.Checked == false) return;
            this.comboBox_LightImage_SelectedIndexChanged(sender, e);
        }

        /// <summary>
        /// 切換【單顆Cell影像】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_LightImage_SelectedIndexChanged(object sender, EventArgs e) // (20200429) Jeff Revised!
        {
            bool b_SetPart_NewWindow = false; // (20200429) Jeff Revised!
            if (sender == null) // 第一次按下【視窗顯示】
                b_SetPart_NewWindow = true;

            if (radioButton_DispA_CellImage.Checked && (tabControl_BigMap.SelectedTab.Text == "結果顯示" || tabControl_BigMap.SelectedTab.Text == "人工覆判與統計結果" ||
                                                        tabControl_BigMap.SelectedTab.Text == "AI分圖")) // 【A面】
                this.Display_CellImage(this.locate_method_, false, b_SetPart_NewWindow);
            else if (radioButton_DispB_CellImage.Checked && tabControl_BigMap.SelectedTab.Text == "B面資訊") // 【B面】
                this.Display_CellImage(locate_method_B, false, b_SetPart_NewWindow);
            else if (tabControl_BigMap.SelectedTab.Text == "雙面統計結果合併")
            {
                if (this.radioButton_DispA_CellImage.Checked) // 【A面】
                    this.Display_CellImage(locate_method_, false, b_SetPart_NewWindow);
                else // 【B面】
                    this.Display_CellImage(locate_method_B, false, b_SetPart_NewWindow);
            }
            else
                HOperatorSet.ClearWindow(hSmartWindowControl_CellImage.HalconWindow);
        }

        /// <summary>
        /// 【視窗顯示】(【單顆Cell影像】)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbx_DispWindow_CellImage_CheckedChanged(object sender, EventArgs e) // (20200429) Jeff Revised!
        {
            CheckBox Obj = (CheckBox)sender;

            if (Obj.Checked) // 視窗顯示
            {
                #region 視窗顯示

                try
                {
                    this.Form_DispWindow_CellImage = new DispWindow_AICellImg();
                    this.Form_DispWindow_CellImage.Set_Title("單顆Cell影像");
                    this.Form_DispWindow_CellImage.FormClosedEvent += new DispWindow_AICellImg.FormClosedHandler(SetFormClosed_CellImage);
                    this.Form_DispWindow_CellImage.Show();

                    this.comboBox_LightImage_SelectedIndexChanged(null, null);

                    Obj.Text = "關閉視窗";
                    Obj.BackColor = Color.Red;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "視窗顯示失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                #endregion
            }
            else // 關閉視窗
            {
                #region 關閉視窗

                try
                {
                    this.Form_DispWindow_CellImage.Close();

                    Obj.Text = "視窗顯示";
                    Obj.BackColor = Color.Green;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "關閉視窗失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                #endregion
            }
        }

        public void SetFormClosed_CellImage(bool b_FormClosed) // (20200429) Jeff Revised!
        {
            if (b_FormClosed)
            {
                #region CheckBox狀態復原
                this.cbx_DispWindow_CellImage.CheckedChanged -= cbx_DispWindow_CellImage_CheckedChanged;
                this.cbx_DispWindow_CellImage.Checked = false;
                this.cbx_DispWindow_CellImage.CheckedChanged += cbx_DispWindow_CellImage_CheckedChanged;
                #endregion

                this.cbx_DispWindow_CellImage.Text = "視窗顯示";
                this.cbx_DispWindow_CellImage.BackColor = Color.Green;
            }
        }

        /// <summary>
        /// 更新顯示 (hSmartWindowControl_CellImage) 【單顆Cell影像】
        /// </summary>
        /// <param name="locate_method"></param>
        /// <param name="b_SetPart">是否做 SetPart </param>
        /// <param name="b_SetPart_NewWindow">是否做 SetPart (【視窗顯示】)</param>
        private void Display_CellImage(LocateMethod locate_method, bool b_SetPart = true, bool b_SetPart_NewWindow = false) // (20200429) Jeff Revised!
        {
            HOperatorSet.ClearWindow(hSmartWindowControl_CellImage.HalconWindow);
            if (this.comboBox_LightImage.SelectedIndex < 0)
                return;

            Point point_CellImage;
            if (tabControl_BigMap.SelectedTab.Text == "雙面統計結果合併" && radioButton_DispB_CellImage.Checked) // 【B面】
                point_CellImage = Point_CellImage_B;
            else
                point_CellImage = Point_CellImage;

            HObject dispImg = null;
            if (locate_method.Dictionary_CellImage.ContainsKey(point_CellImage))
            {
                dispImg = locate_method.Dictionary_CellImage[point_CellImage].ListRegion[comboBox_LightImage.SelectedIndex];
                HOperatorSet.DispObj(dispImg, hSmartWindowControl_CellImage.HalconWindow);
            }

            if (b_SetPart)
                HOperatorSet.SetPart(hSmartWindowControl_CellImage.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part

            HObject DefectReg = null; // (20200429) Jeff Revised!
            if (locate_method.Dictionary_CellImage.ContainsKey(point_CellImage))
            {
                if (this.rbtn_defect.Checked) //【瑕疵圖】
                {
                    if (locate_method.Dictionary_CellImage[point_CellImage].B_Defect)
                    {
                        DefectReg = locate_method.Dictionary_CellImage[point_CellImage].DefectReg;
                        if (DefectReg != null)
                        {
                            HOperatorSet.SetDraw(hSmartWindowControl_CellImage.HalconWindow, "fill");
                            HOperatorSet.SetColor(hSmartWindowControl_CellImage.HalconWindow, "red");
                            HOperatorSet.DispObj(DefectReg, hSmartWindowControl_CellImage.HalconWindow);
                        }
                    }
                }
            }

            //【視窗顯示】
            if (this.cbx_DispWindow_CellImage.Checked) // (20200429) Jeff Revised!
            {
                try
                {
                    this.Form_DispWindow_CellImage.Update_DispImg(dispImg, b_SetPart_NewWindow, DefectReg);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
        }

        #endregion



        private void bt_Save_Click(object sender, EventArgs e) // (20200429) Jeff Revised!
        {
            try
            {
                // For debug!
                //HOperatorSet.WriteImage(hSmartWindowControl_map.HalconWindow.DumpWindowImage(), "tiff", 0, save_path_); // (20181018) Jeff Revised!
                //HOperatorSet.WriteImage(cell_map_img_, "tiff", 0, save_path_); // (20181018) Jeff Revised!
                
                HObject cell_map_img_color;
                // 轉成RGB image (20200429) Jeff Revised!
                HTuple Channels;
                HOperatorSet.CountChannels(this.locate_method_.TileImage, out Channels);
                if (Channels == 1)
                    HOperatorSet.Compose3(this.locate_method_.TileImage.Clone(), this.locate_method_.TileImage.Clone(), this.locate_method_.TileImage.Clone(), out cell_map_img_color);
                else
                    HOperatorSet.CopyImage(this.locate_method_.TileImage, out cell_map_img_color);

                if (checkBox_MarkCenter.Checked) // (20181226) Jeff Revised!
                {
                    if (locate_method_.MarkCenter_BigMap_orig != null)
                        HOperatorSet.OverpaintRegion(cell_map_img_color, locate_method_.MarkCenter_BigMap_orig, ((new HTuple(0)).TupleConcat(0)).TupleConcat(255), "fill");
                }

                if (checkBox_cell.Checked)
                {
                    if (locate_method_.cellmap_affine_ != null)
                        HOperatorSet.OverpaintRegion(cell_map_img_color, locate_method_.cellmap_affine_, ((new HTuple(0)).TupleConcat(255)).TupleConcat(0), "margin");
                }

                if (checkBox_AllInspCell.Checked) // (20191122) Jeff Revised!
                {
                    if (locate_method_.all_intersection_Cell_ != null)
                        HOperatorSet.OverpaintRegion(cell_map_img_color, locate_method_.all_intersection_Cell_, ((new HTuple(255)).TupleConcat(0)).TupleConcat(0), "fill");
                }

                if (checkBox_defects.Checked)
                {
                    if (locate_method_.all_intersection_defect_ != null)
                    {
                        if (radioButton_fill_Result.Checked) // (20181112) Jeff Revised!
                            HOperatorSet.OverpaintRegion(cell_map_img_color, locate_method_.all_intersection_defect_, ((new HTuple(255)).TupleConcat(0)).TupleConcat(0), "fill");
                        else
                            HOperatorSet.OverpaintRegion(cell_map_img_color, locate_method_.all_intersection_defect_, ((new HTuple(255)).TupleConcat(0)).TupleConcat(0), "margin");
                    }
                }

                if (checkBox_backDefect.Checked)
                {
                    if (locate_method_.all_BackDefect_ != null)
                    {
                        if (radioButton_fill_Result.Checked)
                            HOperatorSet.OverpaintRegion(cell_map_img_color, locate_method_.all_BackDefect_, ((new HTuple(255)).TupleConcat(0)).TupleConcat(255), "fill");
                        else
                            HOperatorSet.OverpaintRegion(cell_map_img_color, locate_method_.all_BackDefect_, ((new HTuple(255)).TupleConcat(0)).TupleConcat(255), "margin");
                    }
                }

                if (checkBox_capture_map.Checked)
                {
                    if (locate_method_.capture_map_ != null)
                        HOperatorSet.OverpaintRegion(cell_map_img_color, locate_method_.capture_map_, ((new HTuple(0)).TupleConcat(0)).TupleConcat(255), "margin");
                }

                HOperatorSet.WriteImage(cell_map_img_color, "tiff", 0, Application.StartupPath + "\\大圖.tif");
                MessageBox.Show("儲存大圖成功!");

                cell_map_img_color.Dispose(); // (20190403) Jeff Revised!
            }
            catch (Exception exception)
            {
                string message = "儲存大圖失敗，錯誤訊息如下:" + Environment.NewLine;
                message += exception.ToString();
                MessageBox.Show(message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

    }
    
}
