using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using DeltaSubstrateInspector.src.Modules.NewInspectModule.InductanceInsp; // For enuBand
using HalconDotNet;

namespace DeltaSubstrateInspector.src.Modules.Algorithm // (20191226) Jeff Revised!
{
    public partial class Algo_Image_Form : Form
    {
        #region 參數

        /// <summary>
        /// Form內目前 Algo_Image類別物件
        /// </summary>
        private Algo_Image AlgoImg { get; set; } = new Algo_Image();

        /// <summary>
        /// 點擊【確定】後更新呼叫端之 Algo_Image類別物件
        /// </summary>
        public Algo_Image AlgoImg_yes { get; set; } = new Algo_Image();

        /// <summary>
        /// 演算法編號 (0, 1, 2, ...)
        /// </summary>
        private int ID_ResultImg { get; set; }

        /// <summary>
        /// 是否正在載入參數
        /// </summary>
        private bool b_LoadParams { get; set; } = false;

        /// <summary>
        /// 影像【來源】控制項集合
        /// </summary>
        private List<ComboBox> ListCtrl_ImageSource { get; set; } = new List<ComboBox>();
        /// <summary>
        /// 影像【ID】控制項集合
        /// </summary>
        private List<ComboBox> ListCtrl_ImageID { get; set; } = new List<ComboBox>();
        /// <summary>
        /// 影像【Band】控制項集合
        /// </summary>
        private List<ComboBox> ListCtrl_ImageBand { get; set; } = new List<ComboBox>();

        /// <summary>
        /// 區域【來源】控制項集合
        /// </summary>
        private List<ComboBox> ListCtrl_RegionSource { get; set; } = new List<ComboBox>(); // (20200729) Jeff Revised!
        /// <summary>
        /// 區域【ID】控制項集合
        /// </summary>
        private List<ComboBox> ListCtrl_RegionID { get; set; } = new List<ComboBox>(); // (20200729) Jeff Revised!

        /// <summary>
        /// 區域【ID】Label控制項集合
        /// </summary>
        private List<Label> ListLabel_RegionID { get; set; } = new List<Label>(); // (20200729) Jeff Revised!

        /// <summary>
        /// 結果影像集合 (Form內)
        /// </summary>
        private List<HObject> ResultImages = new List<HObject>();

        /// <summary>
        /// 結果區域集合 (Form內)
        /// </summary>
        private List<HObject> ResultRegions = new List<HObject>(); // (20200729) Jeff Revised!

        /// <summary>
        /// 結果影像集合 (點擊【確定】後更新呼叫端之 結果影像集合)
        /// </summary>
        private List<HObject> ResultImages_yes = new List<HObject>();

        /// <summary>
        /// 結果區域集合 (點擊【確定】後更新呼叫端之 結果區域集合)
        /// </summary>
        private List<HObject> ResultRegions_yes = new List<HObject>(); // (20200729) Jeff Revised!

        #endregion

        public Algo_Image_Form(Algo_Image algoImg, int id_ResultImg, ref List<HObject> ResultImages_, ref List<HObject> ResultRegions_) // (20200729) Jeff Revised!
        {
            InitializeComponent();
            this.hWindowControl_DispImage.MouseWheel += hWindowControl_DispImage.HSmartWindowControl_MouseWheel;
            this.Initialize_GUI(algoImg);

            //this.AlgoImg = algoImg.Clone(); // 會有問題!
            //this.AlgoImg = algoImg.DeepClone();
            //this.AlgoImg = algoImg.Clone_Manual(true);
            this.AlgoImg = algoImg.Clone_Manual();

            this.AlgoImg_yes = algoImg;
            this.ID_ResultImg = id_ResultImg;

            //this.ResultImages = ResultImages_.ToList(); // 不會複製到新記憶體位址!
            for (int i = 0; i < ResultImages_.Count; i++)
                this.ResultImages.Add(ResultImages_[i].Clone());
            this.ResultImages_yes = ResultImages_;

            for (int i = 0; i < ResultRegions_.Count; i++)
            {
                if (ResultRegions_[i] != null)
                    this.ResultRegions.Add(ResultRegions_[i].Clone());
                else
                    this.ResultRegions.Add(null);
            }
            this.ResultRegions_yes = ResultRegions_;
        }

        private void Algo_Image_Form_Load(object sender, EventArgs e)
        {
            // 判斷是【新增】或【編輯】演算法
            if (this.ID_ResultImg > (this.AlgoImg.ListAlgoImage.Count - 1)) //【新增】
            {
                Single_AlgoImage single_algoImg = new Single_AlgoImage();
                single_algoImg.Single_AlgoImage_Constructor(this.ID_ResultImg, "", 0, this.AlgoImg.B_UsedRegion); // (20200729) Jeff Revised!
                this.AlgoImg.ListAlgoImage.Add(single_algoImg);
            }

            // 載入參數
            this.ui_parameters(false);

            // 顯示計算結果影像
            this.button_Execute_Click(null, null);

            HOperatorSet.SetPart(hWindowControl_DispImage.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part
        }
        
        /// <summary>
        /// 初始化GUI
        /// </summary>
        /// <param name="algoImg"></param>
        private void Initialize_GUI(Algo_Image algoImg) // (20200729) Jeff Revised!
        {
            this.comboBox_DispRegType.SelectedIndex = 0;
            // 【演算法】
            this.cbx_Algo.Items.Clear();
            string[] ArrayAlgo = Enum.GetNames(typeof(enu_Algo_Image));
            foreach (string Algo in ArrayAlgo)
                this.cbx_Algo.Items.Add(Algo);
            
            #region 【輸入影像設定】

            this.ListCtrl_ImageSource.Add(this.cbx_ImageSource1);
            this.ListCtrl_ImageSource.Add(this.cbx_ImageSource2);
            this.ListCtrl_ImageSource.Add(this.cbx_ImageSource3);
            this.ListCtrl_ImageID.Add(this.cbx_Img1_ID);
            this.ListCtrl_ImageID.Add(this.cbx_Img2_ID);
            this.ListCtrl_ImageID.Add(this.cbx_Img3_ID);
            this.ListCtrl_ImageBand.Add(this.cbx_Img1_Band);
            this.ListCtrl_ImageBand.Add(this.cbx_Img2_Band);
            this.ListCtrl_ImageBand.Add(this.cbx_Img3_Band);

            // 【Band】
            string[] ArrayBand = Enum.GetNames(typeof(enuBand));
            this.cbx_Img1_Band.Items.Clear();
            this.cbx_Img2_Band.Items.Clear();
            this.cbx_Img3_Band.Items.Clear();
            foreach (string Band in ArrayBand)
            {
                this.cbx_Img1_Band.Items.Add(Band);
                this.cbx_Img2_Band.Items.Add(Band);
                this.cbx_Img3_Band.Items.Add(Band);
            }

            #endregion

            #region 【影像參數設定】

            //【NewType】
            this.cbx_newType.Items.Clear();
            string[] ArrayNewType = Enum.GetNames(typeof(enu_ImageType));
            foreach (string newType in ArrayNewType)
                this.cbx_newType.Items.Add(newType.Substring(1));

            //【Type_ROI】-> 【使用範圍】 (20200119) Jeff Revised!
            if (algoImg.B_UsedRegion) // 是否使用編輯範圍 (只對有ROI之影像演算法 or 區域演算法B)
            {
                this.label_Type_ROI.Text = "使用範圍:";
                this.label_Type_ROI.Tag = "SelectRegionIndex";
                this.cbx_Type_ROI.Tag = "SelectRegionIndex";
                this.cbx_Type_ROI.Items.Clear();
                foreach (string name in algoImg.Name_UsedRegionList)
                    this.cbx_Type_ROI.Items.Add(name);
            }

            //【PaintType】
            this.cbx_PaintType.Items.Clear();
            string[] ArrayPaintType = Enum.GetNames(typeof(enu_PaintType));
            foreach (string paintType in ArrayPaintType)
                this.cbx_PaintType.Items.Add(paintType);

            //【Interpolation】
            this.cbx_interpolation.Items.Clear();
            string[] ArrayInterpolation = Enum.GetNames(typeof(enu_interpolation));
            foreach (string interpolation in ArrayInterpolation)
                this.cbx_interpolation.Items.Add(interpolation);

            #endregion

            #region 【輸入區域設定】

            this.ListCtrl_RegionSource.Add(this.cbx_RegionSource1);
            this.ListCtrl_RegionSource.Add(this.cbx_RegionSource2);
            this.ListCtrl_RegionID.Add(this.cbx_Reg1_ID);
            this.ListCtrl_RegionID.Add(this.cbx_Reg2_ID);
            this.ListLabel_RegionID.Add(this.label_Reg1_ID);
            this.ListLabel_RegionID.Add(this.label_Reg2_ID);

            // 【來源】
            if (algoImg.B_UsedRegion == false) // 不使用編輯範圍
            {
                for (int i = 0; i < this.ListCtrl_RegionSource.Count; i++)
                    this.ListCtrl_RegionSource[i].Items.RemoveAt(1);
            }

            #endregion

            #region 【區域參數設定】

            // 【LightDark】
            this.cbx_LightDark.Items.Clear();
            string[] ArrayLightDark = Enum.GetNames(typeof(enu_LightDark));
            foreach (string lightDark in ArrayLightDark)
                this.cbx_LightDark.Items.Add(lightDark);

            // 【Type_ShapeTrans】
            this.cbx_ShapeTrans_Type.Items.Clear();
            string[] Type_ShapeTrans = Enum.GetNames(typeof(enu_ShapeTrans_Type));
            foreach (string type_ShapeTrans in Type_ShapeTrans)
                this.cbx_ShapeTrans_Type.Items.Add(type_ShapeTrans);

            // 【Fill Up Feature】
            this.cbx_FillUp_Feature.Items.Clear();
            string[] FillUp_Feature = Enum.GetNames(typeof(enu_FillUp_Feature));
            foreach (string fillUp_Feature in FillUp_Feature)
                this.cbx_FillUp_Feature.Items.Add(fillUp_Feature);

            // 【使用範圍】
            this.checkBox_b_ROI_UsedRegion_Reg.Enabled = algoImg.B_UsedRegion;
            this.checkBox_b_ROI_UsedRegion_Reg.Visible = algoImg.B_UsedRegion;

            #endregion
        }

        /// <summary>
        /// 將UI內容與各Class參數互傳
        /// </summary>
        /// <param name="ui_2_parameters_">True=UI傳至Class ; False=Class傳至UI</param>
        private void ui_parameters(bool ui_2_parameters_) // (20200729) Jeff Revised!
        {
            try
            {
                Single_AlgoImage single_algoImg = this.AlgoImg.ListAlgoImage[this.ID_ResultImg];

                if (ui_2_parameters_)
                {
                    #region 將UI內容回傳至Class

                    single_algoImg.ID = int.Parse(this.nud_Algo_ID.Value.ToString());
                    single_algoImg.Name = this.textBox_Name.Text;
                    single_algoImg.B_ImageAlgo = this.radioButton_Algo_Image.Checked; // (20200729) Jeff Revised!
                    // (20200729) Jeff Revised!
                    if (single_algoImg.B_ImageAlgo) // 影像演算法
                        single_algoImg.Algo = (enu_Algo_Image)(this.cbx_Algo.SelectedIndex);
                    else // 區域演算法
                    {
                        single_algoImg.Type_Algo = (enuTools)(this.cbx_Type_Algo.SelectedIndex);
                        single_algoImg.Index_Algo_Region = this.cbx_Algo.SelectedIndex;
                    }

                    // 【輸入影像設定】
                    for (int i = 0; i < single_algoImg.CountImg; i++)
                    {
                        single_algoImg.B_ImageSource[i] = (this.ListCtrl_ImageSource[i].SelectedIndex == 0) ? true : false;
                        single_algoImg.ID_InputImage[i] = this.ListCtrl_ImageID[i].SelectedIndex;
                        single_algoImg.Bands[i] = (enuBand)(this.ListCtrl_ImageBand[i].SelectedIndex);
                    }

                    // 【輸入區域設定】
                    for (int i = 0; i < single_algoImg.CountReg; i++) // (20200729) Jeff Revised!
                    {
                        single_algoImg.B_RegionSource[i] = (this.ListCtrl_RegionSource[i].SelectedIndex == 0) ? true : false;
                        single_algoImg.ID_InputRegion[i] = this.ListCtrl_RegionID[i].SelectedIndex;
                    }

                    if (single_algoImg.B_ImageAlgo) // 影像演算法
                    {
                        #region 【影像參數設定】

                        foreach (Control ctrl in this.tabPage_ImageParamSetting.Controls)
                        {
                            if (ctrl is Label)
                                continue;

                            string tag = ctrl.Tag.ToString();
                            if (single_algoImg.Parameters.ContainsKey(tag))
                            {
                                if (ctrl is NumericUpDown)
                                    single_algoImg.Parameters[tag] = (ctrl as NumericUpDown).Value;
                                else if (ctrl is ComboBox)
                                {
                                    /*
                                    // Hashtable之value存成各別enum型態
                                    if (single_algoImg.Parameters[tag] is enu_BandpassImage_filterType)
                                        single_algoImg.Parameters[tag] = (enu_BandpassImage_filterType)((ctrl as ComboBox).SelectedIndex);
                                    else if (single_algoImg.Parameters[tag] is enu_DotsImage_filterType)
                                        single_algoImg.Parameters[tag] = (enu_DotsImage_filterType)((ctrl as ComboBox).SelectedIndex);
                                    */

                                    if (tag == "SelectRegionIndex") // (20200119) Jeff Revised!
                                    {
                                        // Hashtable之value存成int型態
                                        single_algoImg.Parameters[tag] = (ctrl as ComboBox).SelectedIndex;
                                    }
                                    else
                                    {
                                        // Hashtable之value存成string型態
                                        single_algoImg.Parameters[tag] = (ctrl as ComboBox).Text;
                                    }
                                }
                                else if (ctrl is CheckBox)
                                {
                                    if (tag != "region")
                                        single_algoImg.Parameters[tag] = (ctrl as CheckBox).Checked;
                                    else // 【Set】
                                        ;
                                }

                            }
                        }

                        #endregion
                    }
                    else // 區域演算法
                    {
                        #region 【區域參數設定】

                        foreach (Control ctrl in this.tabPage_RegionParamSetting.Controls)
                        {
                            if (ctrl is Label)
                                continue;

                            string tag = ctrl.Tag.ToString();
                            if (single_algoImg.Parameters.ContainsKey(tag))
                            {
                                if (ctrl is NumericUpDown)
                                    single_algoImg.Parameters[tag] = (ctrl as NumericUpDown).Value;
                                else if (ctrl is ComboBox)
                                {
                                    if (tag == "SelectRegionIndex")
                                    {
                                        // Hashtable之value存成int型態
                                        single_algoImg.Parameters[tag] = (ctrl as ComboBox).SelectedIndex;
                                    }
                                    else
                                    {
                                        // Hashtable之value存成string型態
                                        single_algoImg.Parameters[tag] = (ctrl as ComboBox).Text;
                                    }
                                }
                                else if (ctrl is CheckBox)
                                {
                                    if (tag != "region")
                                        single_algoImg.Parameters[tag] = (ctrl as CheckBox).Checked;
                                    else // 【Set】
                                        ;
                                }

                            }
                        }

                        #endregion
                    }

                    #endregion
                }
                else
                {
                    #region 將Class內容回傳至UI

                    this.nud_Algo_ID.Value = this.ID_ResultImg;
                    this.textBox_Name.Text = single_algoImg.Name;
                    // (20200729) Jeff Revised!
                    if (single_algoImg.B_ImageAlgo) // 影像演算法
                    {
                        this.radioButton_Algo_Image.Checked = true;
                        this.cbx_Algo.SelectedIndex = -1;
                        this.cbx_Algo.SelectedIndex = (int)(single_algoImg.Algo); // 會執行 cbx_Algo_SelectedIndexChanged()
                    }
                    else // 區域演算法
                    {
                        this.radioButton_Algo_Region.Checked = true; // 會執行 radioButton_Algo_CheckedChanged()
                        this.cbx_Type_Algo.SelectedIndex = (int)single_algoImg.Type_Algo; // 會執行 cbx_Type_Algo_SelectedIndexChanged()
                        this.cbx_Algo.SelectedIndex = single_algoImg.Index_Algo_Region; // 會執行 cbx_Algo_SelectedIndexChanged()
                    }
                    
                    #endregion
                }
            }
            catch (Exception ex)
            {
                string message = "Error: " + ex.Message;
                MessageBox.Show(message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 【影像】/【區域】切換
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_Algo_CheckedChanged(object sender, EventArgs e) // (20200729) Jeff Revised!
        {
            RadioButton rbtn = sender as RadioButton;
            if (rbtn.Checked == false)
                return;

            this.cbx_Type_Algo.Items.Clear();
            this.cbx_Type_Algo.Text = "";
            this.cbx_Algo.Items.Clear();
            this.cbx_Algo.Text = "";
            this.tabPage_ImageParamSetting.Enabled = false;
            this.tabPage_RegionParamSetting.Enabled = false;
            if (this.radioButton_Algo_Image.Checked) // 影像演算法
            {
                string[] ArrayAlgo = Enum.GetNames(typeof(enu_Algo_Image));
                foreach (string Algo in ArrayAlgo)
                    this.cbx_Algo.Items.Add(Algo);
                //this.cbx_Algo.SelectedIndex = 0;
                this.tabControl_ParamSetting.SelectedIndex = 0;
            }
            else // 區域演算法
            {
                string[] ArrayTools = Enum.GetNames(typeof(enuTools));
                foreach (string Tool in ArrayTools)
                    this.cbx_Type_Algo.Items.Add(Tool);
                //this.cbx_Type_Algo.SelectedIndex = 0;
                this.tabControl_ParamSetting.SelectedIndex = 1;
            }
        }

        /// <summary>
        /// 【類型】切換
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbx_Type_Algo_SelectedIndexChanged(object sender, EventArgs e) // (20200729) Jeff Revised!
        {
            if (this.cbx_Type_Algo.SelectedIndex < 0)
                return;

            if (this.radioButton_Algo_Region.Checked) // 區域演算法
            {
                #region 設定【演算法】下拉式選單項目

                this.cbx_Algo.Items.Clear();
                this.cbx_Algo.Text = "";

                enuTools type_Algo = (enuTools)(this.cbx_Type_Algo.SelectedIndex);
                string[] ArrayAlgo;
                if (type_Algo == enuTools.Threshold)
                    ArrayAlgo = Enum.GetNames(typeof(enuThreshold));
                else if (type_Algo == enuTools.RegionSets)
                    ArrayAlgo = Enum.GetNames(typeof(enuRegionSets));
                else if (type_Algo == enuTools.Morphology)
                    ArrayAlgo = Enum.GetNames(typeof(enuMorphology));
                else
                    ArrayAlgo = Enum.GetNames(typeof(enuSelect));
                foreach (string Algo in ArrayAlgo)
                    this.cbx_Algo.Items.Add(Algo);
                //this.cbx_Algo.SelectedIndex = 0;

                #endregion
            }
        }

        /// <summary>
        /// 【演算法】切換，更新控制項啟用狀態、相關屬性及參數
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbx_Algo_SelectedIndexChanged(object sender, EventArgs e) // (20200729) Jeff Revised!
        {
            if (this.cbx_Algo.SelectedIndex < 0)
                return;

            /* 更新 Single_AlgoImage 類別物件 */
            Single_AlgoImage single_AlgoImage = this.AlgoImg.ListAlgoImage[this.ID_ResultImg];
            bool b_Initialize_Algo = false;
            if (this.radioButton_Algo_Image.Checked != single_AlgoImage.B_ImageAlgo)
                b_Initialize_Algo = true;
            else
            {
                if (this.radioButton_Algo_Image.Checked) // 影像演算法
                {
                    if (this.cbx_Algo.SelectedIndex != (int)(single_AlgoImage.Algo)) // 非原始演算法類型
                        b_Initialize_Algo = true;
                }
                else // 區域演算法
                {
                    if ((this.cbx_Type_Algo.SelectedIndex != (int)(single_AlgoImage.Type_Algo)) || (this.cbx_Algo.SelectedIndex != single_AlgoImage.Index_Algo_Region)) // 非原始演算法類型
                        b_Initialize_Algo = true;
                }
            }
            if (b_Initialize_Algo)
                single_AlgoImage.Initialize_Algo(this.radioButton_Algo_Image.Checked, this.cbx_Algo.SelectedIndex, this.cbx_Type_Algo.SelectedIndex);

            #region 更新控制項啟用狀態、相關屬性及參數

            this.b_LoadParams = true;

            #region 【輸入影像設定】

            // 更新控制項啟用狀態
            switch (single_AlgoImage.CountImg)
            {
                case 0:
                    {
                        this.gbx_Image1.Enabled = false;
                        this.gbx_Image2.Enabled = false;
                        this.gbx_Image3.Enabled = false;
                    }
                    break;
                case 1:
                    {
                        this.gbx_Image1.Enabled = true;
                        this.gbx_Image2.Enabled = false;
                        this.gbx_Image3.Enabled = false;
                    }
                    break;
                case 2:
                    {
                        this.gbx_Image1.Enabled = true;
                        this.gbx_Image2.Enabled = true;
                        this.gbx_Image3.Enabled = false;
                    }
                    break;
                case 3:
                    {
                        this.gbx_Image1.Enabled = true;
                        this.gbx_Image2.Enabled = true;
                        this.gbx_Image3.Enabled = true;
                    }
                    break;
                default:
                    break;
            }

            // 更新控制項參數
            for (int i = 0; i < single_AlgoImage.CountImg; i++)
            {
                this.ListCtrl_ImageSource[i].SelectedIndex = (single_AlgoImage.B_ImageSource[i]) ? 0 : 1;
                this.ListCtrl_ImageID[i].SelectedIndex = single_AlgoImage.ID_InputImage[i];
                this.ListCtrl_ImageBand[i].SelectedIndex = (int)(single_AlgoImage.Bands[i]);
            }

            #endregion

            #region 【輸入區域設定】 // (20200729) Jeff Revised!

            // 更新控制項啟用狀態
            switch (single_AlgoImage.CountReg)
            {
                case 0:
                    {
                        this.gbx_Region1.Enabled = false;
                        this.gbx_Region2.Enabled = false;
                    }
                    break;
                case 1:
                    {
                        this.gbx_Region1.Enabled = true;
                        this.gbx_Region2.Enabled = false;
                    }
                    break;
                case 2:
                    {
                        this.gbx_Region1.Enabled = true;
                        this.gbx_Region2.Enabled = true;
                    }
                    break;
                default:
                    break;
            }

            // 更新控制項參數
            for (int i = 0; i < single_AlgoImage.CountReg; i++)
            {
                this.ListCtrl_RegionSource[i].SelectedIndex = (single_AlgoImage.B_RegionSource[i]) ? 0 : 1;
                if (this.ListCtrl_RegionID[i].Items.Count > single_AlgoImage.ID_InputRegion[i])
                    this.ListCtrl_RegionID[i].SelectedIndex = single_AlgoImage.ID_InputRegion[i];
            }

            #endregion

            #region 【參數設定】 // (20200729) Jeff Revised!

            if (single_AlgoImage.Parameters.Count == 0) // 無需設定參數
            {
                this.tabPage_ImageParamSetting.Enabled = false;
                this.tabPage_RegionParamSetting.Enabled = false;
            }
            else // 需設定參數
            {
                if (single_AlgoImage.B_ImageAlgo) // 影像演算法
                {
                    #region 【影像參數設定】

                    this.tabPage_ImageParamSetting.Enabled = true;
                    this.tabPage_RegionParamSetting.Enabled = false;
                    
                    #region 多種演算法共用之字串類型控制項更新

                    if (single_AlgoImage.Parameters.ContainsKey("filterType")) // 【FilterType】
                    {
                        this.cbx_filterType.Items.Clear();
                        string[] Array_filterType = new string[] { };
                        if (single_AlgoImage.Algo == enu_Algo_Image.帶通濾波)
                            Array_filterType = Enum.GetNames(typeof(enu_BandpassImage_filterType));
                        else if (single_AlgoImage.Algo == enu_Algo_Image.Dots)
                            Array_filterType = Enum.GetNames(typeof(enu_DotsImage_filterType));

                        foreach (string filterType in Array_filterType)
                            this.cbx_filterType.Items.Add(filterType);
                    }

                    #endregion
                    
                    #region 更新控制項啟用狀態 & 參數

                    foreach (Control ctrl in this.tabPage_ImageParamSetting.Controls)
                    {
                        string tag = ctrl.Tag.ToString();
                        if (single_AlgoImage.Parameters.ContainsKey(tag))
                        {
                            ctrl.Enabled = true;
                            // 更新控制項參數
                            if (ctrl is NumericUpDown)
                                (ctrl as NumericUpDown).Value = decimal.Parse(single_AlgoImage.Parameters[tag].ToString());
                            else if (ctrl is ComboBox)
                            {
                                if (tag == "SelectRegionIndex") // (20200119) Jeff Revised!
                                {
                                    // Hashtable之value存成int型態
                                    (ctrl as ComboBox).SelectedIndex = int.Parse(single_AlgoImage.Parameters[tag].ToString());
                                }
                                else
                                {
                                    //(ctrl as ComboBox).SelectedIndex = (int)(single_AlgoImage.Parameters[tag]); // Hashtable之value存成各別enum型態
                                    (ctrl as ComboBox).SelectedItem = single_AlgoImage.Parameters[tag]; // Hashtable之value存成string型態
                                }
                            }
                            else if (ctrl is CheckBox)
                            {
                                if (tag != "region")
                                    (ctrl as CheckBox).Checked = bool.Parse(single_AlgoImage.Parameters[tag].ToString());
                            }
                        }
                        else
                            ctrl.Enabled = false;
                    }

                    #endregion
                    
                    #region 特殊控制項

                    if (this.AlgoImg.B_UsedRegion == false) // (20200119) Jeff Revised!
                    {
                        // 【Invert】
                        this.checkBox_b_ROI_Invert.Checked = false;
                        if (single_AlgoImage.Parameters.ContainsKey("region"))
                            this.checkBox_b_ROI_Invert.Enabled = true;
                        else
                            this.checkBox_b_ROI_Invert.Enabled = false;
                    }

                    #endregion

                    #endregion
                }
                else // 區域演算法
                {
                    #region 【區域參數設定】

                    this.tabPage_ImageParamSetting.Enabled = false;
                    this.tabPage_RegionParamSetting.Enabled = true;
                    
                    #region 更新控制項啟用狀態 & 參數

                    if (single_AlgoImage.Parameters.ContainsKey("b_ROI_UsedRegion"))
                        this.checkBox_b_ROI_UsedRegion_Reg.Checked = bool.Parse(single_AlgoImage.Parameters["b_ROI_UsedRegion"].ToString());

                    foreach (Control ctrl in this.tabPage_RegionParamSetting.Controls)
                    {
                        string tag = ctrl.Tag.ToString();
                        if (single_AlgoImage.Parameters.ContainsKey(tag))
                        {
                            ctrl.Enabled = true;
                            // 更新控制項參數
                            if (ctrl is NumericUpDown)
                                (ctrl as NumericUpDown).Value = decimal.Parse(single_AlgoImage.Parameters[tag].ToString());
                            else if (ctrl is ComboBox)
                            {
                                if (tag == "SelectRegionIndex")
                                {
                                    // Hashtable之value存成int型態
                                    (ctrl as ComboBox).SelectedIndex = int.Parse(single_AlgoImage.Parameters[tag].ToString());
                                }
                                else
                                {
                                    (ctrl as ComboBox).SelectedItem = single_AlgoImage.Parameters[tag]; // Hashtable之value存成string型態
                                }
                            }
                            else if (ctrl is CheckBox)
                            {
                                if (tag != "region")
                                    (ctrl as CheckBox).Checked = bool.Parse(single_AlgoImage.Parameters[tag].ToString());
                            }
                        }
                        else
                            ctrl.Enabled = false;
                    }

                    #endregion

                    #region 特殊控制項

                    // 【Invert】
                    if (single_AlgoImage.Parameters.ContainsKey("region"))
                    {
                        this.checkBox_b_ROI_Invert_Reg.Enabled = true;
                        this.checkBox_b_ROI_Invert_Reg.Checked = false;
                    }

                    #endregion

                    #endregion
                }
            }

            #endregion

            this.b_LoadParams = false;

            #endregion

            // 顯示計算結果影像 & 區域
            //this.button_Execute_Click(null, null);
        }
        
        /// <summary>
        /// 【輸入影像設定】: 【來源】、【ID】或【Band】切換
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbx_Image_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cbx = sender as ComboBox;
            if (cbx.SelectedIndex < 0)
                return;
            string tag = cbx.Tag.ToString(); // tag: ImageSource, ID, Band

            if (this.b_LoadParams && tag != "ImageSource")
                return;
            
            #region 判斷是第幾張輸入影像

            int index = 0;
            if (tag == "ImageSource")
                index = this.ListCtrl_ImageSource.IndexOf(cbx);
            else if (tag == "ID")
                index = this.ListCtrl_ImageID.IndexOf(cbx);
            else if (tag == "Band")
                index = this.ListCtrl_ImageBand.IndexOf(cbx);
            if (index == -1)
            {
                MessageBox.Show("程式內部設定錯誤", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            #endregion
            
            #region 【來源】切換則更新【ID】items

            if (tag == "ImageSource")
            {
                this.ListCtrl_ImageID[index].Items.Clear();
                this.ListCtrl_ImageID[index].Text = ""; // (20200729) Jeff Revised!
                int maxCount = 0;
                if (cbx.SelectedIndex == 0) // 原始
                    maxCount = this.AlgoImg.ImageSource.Count;
                else // 結果
                    maxCount = this.ID_ResultImg;

                for (int i = 0; i < maxCount; i++)
                    this.ListCtrl_ImageID[index].Items.Add(i.ToString());

                if (this.ListCtrl_ImageID[index].Items.Count > 0 && this.b_LoadParams == false)
                {
                    this.ListCtrl_ImageID[index].SelectedIndexChanged -= new System.EventHandler(this.cbx_Image_SelectedIndexChanged);
                    this.ListCtrl_ImageID[index].SelectedIndex = 0;
                    this.ListCtrl_ImageID[index].SelectedIndexChanged += new System.EventHandler(this.cbx_Image_SelectedIndexChanged);
                }
            }

            #endregion

            #region 更新顯示影像

            if (this.b_LoadParams == false)
            {
                if ((this.ListCtrl_ImageSource[index].SelectedIndex < 0) ||
                    (this.ListCtrl_ImageID[index].SelectedIndex < 0) ||
                    (this.ListCtrl_ImageBand[index].SelectedIndex < 0))
                    return;

                // 更新參數
                this.ui_parameters(true);

                /* 影像顯示 */
                HObject imgDisp = this.Get_InputImage(index);
                HOperatorSet.ClearWindow(hWindowControl_DispImage.HalconWindow);
                HOperatorSet.DispObj(imgDisp, hWindowControl_DispImage.HalconWindow);
                this.Update_DispImageInfo(imgDisp);
                imgDisp.Dispose();
            }

            #endregion
        }

        /// <summary>
        /// 取得輸入影像
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private HObject Get_InputImage(int index) // (20200729) Jeff Revised!
        {
            HObject InputImage;
            HOperatorSet.GenEmptyObj(out InputImage);
            if ((this.ListCtrl_ImageSource[index].SelectedIndex < 0) ||
                (this.ListCtrl_ImageID[index].SelectedIndex < 0) ||
                (this.ListCtrl_ImageBand[index].SelectedIndex < 0))
                return InputImage;

            int idDisp = this.ListCtrl_ImageID[index].SelectedIndex;
            enuBand bandDisp = (enuBand)(this.ListCtrl_ImageBand[index].SelectedIndex);
            InputImage.Dispose();
            if (this.ListCtrl_ImageSource[index].SelectedIndex == 0) // 原始
                InputImage = clsStaticTool.GetChannelImage(this.AlgoImg.ImageSource[idDisp], bandDisp);
            else // 結果
                InputImage = clsStaticTool.GetChannelImage(this.ResultImages[idDisp], bandDisp);
            
            return InputImage;
        }

        /// <summary>
        /// 【輸入區域設定】: 【來源】或【ID】切換
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbx_Region_SelectedIndexChanged(object sender, EventArgs e) // (20200729) Jeff Revised!
        {
            ComboBox cbx = sender as ComboBox;
            if (cbx.SelectedIndex < 0)
                return;
            string tag = cbx.Tag.ToString(); // tag: RegionSource, ID

            if (this.b_LoadParams && tag != "RegionSource")
                return;

            #region 判斷是第幾張輸入區域

            int index = 0;
            if (tag == "RegionSource")
                index = this.ListCtrl_RegionSource.IndexOf(cbx);
            else if (tag == "ID")
                index = this.ListCtrl_RegionID.IndexOf(cbx);
            if (index == -1)
            {
                MessageBox.Show("程式內部設定錯誤", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            #endregion

            #region 【來源】切換則更新【ID】items

            if (tag == "RegionSource")
            {
                this.ListCtrl_RegionID[index].Items.Clear();
                this.ListCtrl_RegionID[index].Text = "";
                if (cbx.SelectedIndex == 0) // 結果
                {
                    this.ListLabel_RegionID[index].Text = "ID:";
                    for (int i = 0; i < this.ID_ResultImg; i++)
                        this.ListCtrl_RegionID[index].Items.Add(i.ToString());
                }
                else // 使用範圍
                {
                    this.ListLabel_RegionID[index].Text = "名稱:";
                    foreach (string s in this.AlgoImg.Name_UsedRegionList)
                        this.ListCtrl_RegionID[index].Items.Add(s);
                }
                
                if (this.ListCtrl_RegionID[index].Items.Count > 0 && this.b_LoadParams == false)
                {
                    this.ListCtrl_RegionID[index].SelectedIndexChanged -= new System.EventHandler(this.cbx_Region_SelectedIndexChanged);
                    this.ListCtrl_RegionID[index].SelectedIndex = 0;
                    this.ListCtrl_RegionID[index].SelectedIndexChanged += new System.EventHandler(this.cbx_Region_SelectedIndexChanged);
                }
            }

            #endregion

            #region 更新顯示影像 & 區域

            if (this.b_LoadParams == false)
            {
                if ((this.ListCtrl_RegionSource[index].SelectedIndex < 0) ||
                    (this.ListCtrl_RegionID[index].SelectedIndex < 0))
                    return;

                // 更新參數
                this.ui_parameters(true);

                /* 影像顯示 */
                Single_AlgoImage single_algoImg = this.AlgoImg.ListAlgoImage[this.ID_ResultImg];
                HOperatorSet.ClearWindow(hWindowControl_DispImage.HalconWindow);
                if (single_algoImg.CountImg > 0)
                {
                    int index_img = 0;
                    HObject imgDisp = this.Get_InputImage(index_img);
                    HOperatorSet.DispObj(imgDisp, hWindowControl_DispImage.HalconWindow);
                    this.Update_DispImageInfo(imgDisp);
                    imgDisp.Dispose();
                }

                /* 區域顯示 */
                {
                    int idDisp = this.ListCtrl_RegionID[index].SelectedIndex;
                    HObject regDisp;
                    if (this.ListCtrl_RegionSource[index].SelectedIndex == 0) // 結果
                        regDisp = this.ResultRegions[idDisp];
                    else // 使用範圍
                        regDisp = this.AlgoImg.UsedRegionList[idDisp];

                    HOperatorSet.SetColor(hWindowControl_DispImage.HalconWindow, "red");
                    if (this.comboBox_DispRegType.SelectedIndex == 1) // 輪廓
                        HOperatorSet.SetDraw(hWindowControl_DispImage.HalconWindow, "margin");
                    else // 填滿
                        HOperatorSet.SetDraw(hWindowControl_DispImage.HalconWindow, "fill");
                    HOperatorSet.DispObj(regDisp, hWindowControl_DispImage.HalconWindow);

                    Extension.HObjectMedthods.ReleaseHObject(ref this.Current_disp_regions);
                    this.Current_disp_regions = regDisp.Clone();
                }
            }

            #endregion
        }
        
        private List<Control> ListCtrl_Bypass { get; set; } = new List<Control>();

        private HDrawingObject drawing_Rect { get; set; } = new HDrawingObject(100, 100, 199, 199);

        /// <summary>
        /// 計算及設定 區域反向 (i.e. 更新 single_algoImg.Parameters["region"])
        /// </summary>
        /// <param name="index"></param>
        private bool Compute_Region_Invert(int index = 0)
        {
            bool b_status_ = false;
            try
            {
                Single_AlgoImage single_algoImg = this.AlgoImg.ListAlgoImage[this.ID_ResultImg];
                if (single_algoImg.Parameters.ContainsKey("region"))
                {
                    // 取得輸入影像
                    HObject InputImg = this.Get_InputImage(index);

                    // 計算
                    HObject ResultRegion;
                    Algo_Image.Region_Invert(InputImg, (HObject)single_algoImg.Parameters["region"], out ResultRegion);
                    InputImg.Dispose();
                    single_algoImg.Dispose();
                    single_algoImg.Parameters["region"] = ResultRegion;

                    b_status_ = true;
                }
            }
            catch (Exception ex)
            { }

            return b_status_;
        }

        #region 【影像參數設定】

        /// <summary>
        /// 【影像參數設定】參數更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void param_ValueChanged(object sender, EventArgs e)
        {
            if (this.b_LoadParams)
                return;

            Control ctrl = sender as Control;
            string tag = ctrl.Tag.ToString();
            
            #region 特殊控制項

            if (this.AlgoImg.B_UsedRegion == false) // (20200119) Jeff Revised!
            {
                if (tag == "b_ROI_Invert") // 【Invert】
                {
                    Single_AlgoImage single_algoImg = this.AlgoImg.ListAlgoImage[this.ID_ResultImg];
                    if (single_algoImg.Parameters.ContainsKey("region"))
                    {
                        if (this.Compute_Region_Invert(0) == false)
                        {
                            MessageBox.Show("計算區域反向失敗", "設定失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }
            }
            else // 使用編輯範圍 (只對有ROI之影像演算法 or 區域演算法B)
            {
                if (tag == "b_ROI_Invert" || tag == "SelectRegionIndex") // 【Invert】 or 【使用範圍】
                {
                    try
                    {
                        // 更新參數
                        this.ui_parameters(true);

                        // 顯示Region
                        this.button_LoadROI_Click(null, null);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "設定失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    return;
                }
            }

            #endregion

            // 顯示計算結果影像
            this.button_Execute_Click(null, null);
        }

        /// <summary>
        /// 【Load】: 載入ROI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_LoadROI_Click(object sender, EventArgs e)
        {
            Single_AlgoImage single_algoImg = this.AlgoImg.ListAlgoImage[this.ID_ResultImg];
            // ROI
            HObject ROI;
            if (single_algoImg.Parameters.ContainsKey("region"))
                ROI = ((HObject)single_algoImg.Parameters["region"]).Clone();
            else if (single_algoImg.Parameters.ContainsKey("SelectRegionIndex"))
            {
                HObject SelectRegion = this.AlgoImg.UsedRegionList[int.Parse(single_algoImg.Parameters["SelectRegionIndex"].ToString())];
                if (bool.Parse(single_algoImg.Parameters["b_ROI_Invert"].ToString()))
                {
                    // 取得輸入影像
                    HObject InputImg = this.Get_InputImage(0);

                    // 計算
                    Algo_Image.Region_Invert(InputImg, SelectRegion, out ROI);
                    InputImg.Dispose();
                }
                else
                    ROI = SelectRegion.Clone();
            }
            else
                return;

            // 顯示輸入影像
            this.cbx_Image_SelectedIndexChanged(this.cbx_Img1_ID, null);

            // 顯示ROI
            HOperatorSet.SetDraw(hWindowControl_DispImage.HalconWindow, "margin");
            HOperatorSet.SetColor(hWindowControl_DispImage.HalconWindow, clsStaticTool.GetHalconColor(Color.DarkViolet)); // DarkViolet: #9400D3FF
            HTuple line_w;
            HOperatorSet.GetLineWidth(hWindowControl_DispImage.HalconWindow, out line_w);
            HOperatorSet.SetLineWidth(hWindowControl_DispImage.HalconWindow, 3);
            HOperatorSet.DispObj(ROI, hWindowControl_DispImage.HalconWindow);
            HOperatorSet.SetLineWidth(hWindowControl_DispImage.HalconWindow, line_w);
            ROI.Dispose();
        }

        /// <summary>
        /// 【Set】: 設定ROI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbx_SetROI_CheckedChanged(object sender, EventArgs e) // (20200729) Jeff Revised!
        {
            Single_AlgoImage single_algoImg = this.AlgoImg.ListAlgoImage[this.ID_ResultImg];
            if (single_algoImg.Parameters.ContainsKey("region") == false)
                return;

            ComboBox CBX_Type_ROI;
            CheckBox CBX_b_ROI_Invert;
            TabPage TabPage_ParamSetting;
            if (this.radioButton_Algo_Image.Checked) // 影像演算法
            {
                CBX_Type_ROI = this.cbx_Type_ROI;
                CBX_b_ROI_Invert = this.checkBox_b_ROI_Invert;
                TabPage_ParamSetting = this.tabPage_ImageParamSetting;
            }
            else // 區域演算法
            {
                CBX_Type_ROI = this.cbx_Type_ROI_Reg;
                CBX_b_ROI_Invert = this.checkBox_b_ROI_Invert_Reg;
                TabPage_ParamSetting = this.tabPage_RegionParamSetting;
            }

            CheckBox Obj = (CheckBox)sender;
            if (Obj.Checked) // 【Set】
            {
                #region 【Set】

                try
                {
                    // 顯示輸入影像
                    this.cbx_Image_SelectedIndexChanged(this.cbx_Img1_ID, null);

                    // 創建繪製物件
                    if (this.drawing_Rect != null)
                    {
                        this.drawing_Rect.Dispose();
                        this.drawing_Rect = null;
                    }
                    HDrawingObject.HDrawingObjectType Type = (HDrawingObject.HDrawingObjectType)(CBX_Type_ROI.SelectedIndex);
                    HTuple ImgWidth, ImgHeight;
                    HOperatorSet.GetImageSize(this.DispImg, out ImgWidth, out ImgHeight);
                    this.drawing_Rect = clsStaticTool.GetDrawObj(Type, ImgWidth, ImgHeight);
                    this.drawing_Rect.SetDrawingObjectParams("color", "green");
                    this.hWindowControl_DispImage.HalconWindow.AttachDrawingObjectToWindow(this.drawing_Rect);

                    this.ListCtrl_Bypass = clsStaticTool.EnableAllControls_Bypass(TabPage_ParamSetting, Obj, false);
                    Obj.ForeColor = Color.GreenYellow;
                    Obj.Text = "Done";
                    this.ctrl_timer1 = Obj;
                    this.BackColor_ctrl_timer1_1 = Color.DarkViolet;
                    this.BackColor_ctrl_timer1_2 = Color.Lime;
                    this.timer1.Enabled = true;

                    this.tabPage_InputImage.Enabled = false;
                }
                catch (Exception ex)
                {
                    #region CheckBox狀態復原

                    Obj.CheckedChanged -= this.cbx_SetROI_CheckedChanged;
                    Obj.Checked = false;
                    Obj.CheckedChanged += this.cbx_SetROI_CheckedChanged;

                    #endregion

                    this.drawing_Rect.Dispose();
                    this.drawing_Rect = null;

                    MessageBox.Show(ex.ToString(), "設定ROI失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                #endregion
            }
            else // 【Done】
            {
                #region 【Done】

                this.timer1.Enabled = false;
                this.ctrl_timer1.BackColor = this.BackColor_ctrl_timer1_1;
                clsStaticTool.EnableAllControls_Bypass(TabPage_ParamSetting, Obj, true, this.ListCtrl_Bypass);
                Obj.ForeColor = Color.White;
                Obj.Text = "Set";

                this.tabPage_InputImage.Enabled = true;

                try
                {
                    // 取得使用者繪製region
                    HDrawingObject.HDrawingObjectType Type = (HDrawingObject.HDrawingObjectType)(CBX_Type_ROI.SelectedIndex);
                    HObject Region;
                    HTuple CenterX, CenterY;
                    clsStaticTool.GetDrawRegionRes(this.drawing_Rect.ID, Type, out Region, out CenterX, out CenterY);
                    this.AlgoImg.ListAlgoImage[this.ID_ResultImg].Dispose();
                    this.AlgoImg.ListAlgoImage[this.ID_ResultImg].Parameters["region"] = Region;

                    // 【Invert】
                    if (CBX_b_ROI_Invert.Checked)
                        this.Compute_Region_Invert();

                    // 顯示計算結果影像 & 區域
                    this.button_Execute_Click(null, null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "設定ROI失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                finally
                {
                    this.drawing_Rect.Dispose();
                    this.drawing_Rect = null;
                }

                #endregion
            }
        }

        #endregion

        #region 【區域參數設定】

        /// <summary>
        /// 【區域參數設定】參數更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void param_Reg_ValueChanged(object sender, EventArgs e) // (20200729) Jeff Revised!
        {
            if (this.b_LoadParams)
                return;

            Control ctrl = sender as Control;
            string tag = ctrl.Tag.ToString();

            #region 特殊控制項

            if (tag == "b_ROI_Invert") // 【Invert】
            {
                Single_AlgoImage single_algoImg = this.AlgoImg.ListAlgoImage[this.ID_ResultImg];
                if (single_algoImg.Parameters.ContainsKey("region"))
                {
                    if (this.Compute_Region_Invert(0) == false)
                    {
                        MessageBox.Show("計算區域反向失敗", "設定失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }

            #endregion

            // 顯示計算結果影像 & 區域
            this.button_Execute_Click(null, null);
        }
        
        /// <summary>
        /// 【設定】(灰階閥值)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_SetTh_Reg_Click(object sender, EventArgs e) // (20200729) Jeff Revised!
        {
            if (this.b_LoadParams)
                return;

            HObject img = this.Get_InputImage(0);
            FormThresholdAdjust MyForm = new FormThresholdAdjust(img, int.Parse(this.nud_ThMin_Reg.Value.ToString()), int.Parse(this.nud_ThMax_Reg.Value.ToString()), FormThresholdAdjust.enType.Dual);
            img.Dispose();
            if (MyForm.ShowDialog() == DialogResult.OK)
            {
                int ThMin, ThMax;
                MyForm.GetThreshold(out ThMin, out ThMax);

                this.b_LoadParams = true;
                this.nud_ThMin_Reg.Value = ThMin;
                this.nud_ThMax_Reg.Value = ThMax;
                this.b_LoadParams = false;
                this.param_Reg_ValueChanged(this.nud_ThMin_Reg, null);
            }
        }

        /// <summary>
        /// 【使用範圍】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_b_ROI_UsedRegion_Reg_CheckedChanged(object sender, EventArgs e) // (20200729) Jeff Revised!
        {
            this.cbx_SetROI_Reg.Enabled = this.button_LoadROI_Reg.Enabled = !this.checkBox_b_ROI_UsedRegion_Reg.Checked;
            this.cbx_Type_ROI_Reg.Items.Clear();
            this.cbx_Type_ROI_Reg.Text = "";
            if (this.checkBox_b_ROI_UsedRegion_Reg.Checked)
            {
                this.label_Type_ROI_Reg.Text = "使用範圍:";
                this.label_Type_ROI_Reg.Tag = "SelectRegionIndex";
                this.cbx_Type_ROI_Reg.Tag = "SelectRegionIndex";
                
                foreach (string name in this.AlgoImg.Name_UsedRegionList)
                    this.cbx_Type_ROI_Reg.Items.Add(name);
            }
            else
            {
                this.label_Type_ROI_Reg.Text = "Type_ROI:";
                this.label_Type_ROI_Reg.Tag = "Type_ROI";
                this.cbx_Type_ROI_Reg.Tag = "Type_ROI";

                this.cbx_Type_ROI_Reg.Items.Add("RECTANGLE1");
                this.cbx_Type_ROI_Reg.Items.Add("RECTANGLE2");
                this.cbx_Type_ROI_Reg.Items.Add("CIRCLE");
                this.cbx_Type_ROI_Reg.Items.Add("ELLIPSE");
            }
            
            #region Class參數更新

            if (this.b_LoadParams == false)
            {
                Single_AlgoImage single_algoImg = this.AlgoImg.ListAlgoImage[this.ID_ResultImg];
                if (single_algoImg.Parameters.ContainsKey("b_ROI_UsedRegion"))
                {
                    single_algoImg.Parameters["b_ROI_UsedRegion"] = this.checkBox_b_ROI_UsedRegion_Reg.Checked;
                    if (this.checkBox_b_ROI_UsedRegion_Reg.Checked)
                    {
                        single_algoImg.Parameters.Remove("Type_ROI");
                        single_algoImg.Dispose();
                        single_algoImg.Parameters.Remove("region");
                        single_algoImg.Parameters.Add("SelectRegionIndex", -1);
                        single_algoImg.Parameters.Add("b_ROI_Invert", false);
                    }
                    else
                    {
                        this.checkBox_b_ROI_Invert_Reg.CheckedChanged -= new System.EventHandler(this.param_Reg_ValueChanged);
                        this.checkBox_b_ROI_Invert_Reg.Checked = false;
                        this.checkBox_b_ROI_Invert_Reg.CheckedChanged += new System.EventHandler(this.param_Reg_ValueChanged);

                        single_algoImg.Parameters.Remove("SelectRegionIndex");
                        single_algoImg.Parameters.Remove("b_ROI_Invert");
                        single_algoImg.Parameters.Add("Type_ROI", HDrawingObject.HDrawingObjectType.RECTANGLE1.ToString());
                        HObject reg;
                        HOperatorSet.GenRectangle1(out reg, 0, 0, 299, 299); // 預設region
                        single_algoImg.Parameters.Add("region", reg);
                        this.cbx_Type_ROI_Reg.SelectedIndex = 0;
                    }
                }
            }

            #endregion
        }
        
        #endregion

        #region Timer

        private Control ctrl_timer1 { get; set; } = null;
        private Color BackColor_ctrl_timer1_1 = Color.Transparent, BackColor_ctrl_timer1_2 = Color.Green; // (20190610) Jeff Revised!
        private void timer1_Tick(object sender, EventArgs e) // (20190509) Jeff Revised!
        {
            if (this.ctrl_timer1 == null) return;

            if (this.ctrl_timer1.BackColor == this.BackColor_ctrl_timer1_1)
                this.ctrl_timer1.BackColor = this.BackColor_ctrl_timer1_2;
            else
                this.ctrl_timer1.BackColor = this.BackColor_ctrl_timer1_1;
        }

        #endregion

        private HObject DispImg, DispGrayImg;
        private HTuple width, height;
        /// <summary>
        /// 更新目前視窗顯示影像相關資訊 (【影像型態:】, 【影像頻道:】, 【影像大小:】)
        /// </summary>
        /// <param name="img"></param>
        private void Update_DispImageInfo(HObject img)
        {
            try
            {
                Extension.HObjectMedthods.ReleaseHObject(ref this.DispImg);
                Extension.HObjectMedthods.ReleaseHObject(ref this.DispGrayImg);
                HOperatorSet.CopyImage(img, out this.DispImg);
                HOperatorSet.Rgb1ToGray(this.DispImg, out this.DispGrayImg);

                // 【影像型態:】
                HTuple type;
                HOperatorSet.GetImageType(this.DispImg, out type);
                this.txt_ImageType.Text = type.S;

                // 【影像頻道:】
                HTuple ch;
                HOperatorSet.CountChannels(this.DispImg, out ch);
                this.txt_BandCount.Text = ch.I.ToString();

                // 【影像大小:】
                HOperatorSet.GetImageSize(this.DispImg, out this.width, out this.height);
                this.txt_ImageSize.Text = this.width.I.ToString() + " x " + this.height.I.ToString();
            }
            catch (Exception ex)
            {
                this.txt_ImageType.Text = "";
                this.txt_BandCount.Text = "";
                this.txt_ImageSize.Text = "";
            }
            finally
            {
                Extension.HObjectMedthods.ReleaseHObject(ref this.Current_disp_regions);
            }
        }

        /// <summary>
        /// 更新 【滑鼠影像座標 (x, y):】, 【[R, G, B]:】, 【灰階值:】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hWindowControl_DispImage_HMouseMove(object sender, HMouseEventArgs e)
        {
            this.txt_CursorCoordinate.Text = "(" + String.Format("{0:0}", e.X) + ", " + String.Format("{0:0}", e.Y) + ")";
            try
            {
                if (e.X >= 0 && e.X < this.width && e.Y >= 0 && e.Y < this.height)
                {
                    HTuple grayval;
                    HOperatorSet.GetGrayval(this.DispImg, (int)(e.Y + 0.5), (int)(e.X + 0.5), out grayval);
                    this.txt_RGBValue.Text = (grayval.TupleInt()).ToString();
                    HOperatorSet.GetGrayval(this.DispGrayImg, (int)(e.Y + 0.5), (int)(e.X + 0.5), out grayval);
                    this.txt_GrayValue.Text = (grayval.TupleInt()).ToString();
                }
                else
                {
                    this.txt_RGBValue.Text = "";
                    this.txt_GrayValue.Text = "";
                }
            }
            catch
            {
                this.txt_RGBValue.Text = "";
                this.txt_GrayValue.Text = "";
            }
        }

        /// <summary>
        /// 當前顯示之 Region
        /// </summary>
        private HObject Current_disp_regions;
        /// <summary>
        /// 更新 【區域資訊】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hWindowControl_DispImage_HMouseDown(object sender, HMouseEventArgs e) // (20200729) Jeff Revised!
        {
            if (this.DispImg == null || this.DispGrayImg == null || this.Current_disp_regions == null)
                return;
            
            HTuple RegionCount;
            HOperatorSet.CountObj(this.Current_disp_regions, out RegionCount);
            if (RegionCount > 0)
            {
                HTuple hv_Index = new HTuple();
                HOperatorSet.GetRegionIndex(this.Current_disp_regions, (int)(e.Y + 0.5), (int)(e.X + 0.5), out hv_Index);
                if (hv_Index.Length > 0)
                {
                    HOperatorSet.SetDraw(hWindowControl_DispImage.HalconWindow, (this.comboBox_DispRegType.SelectedIndex == 1) ? "margin" : "fill");
                    HOperatorSet.SetColor(hWindowControl_DispImage.HalconWindow, "red");
                    HOperatorSet.DispObj(this.DispImg, hWindowControl_DispImage.HalconWindow);
                    HOperatorSet.DispObj(this.Current_disp_regions, hWindowControl_DispImage.HalconWindow);

                    HOperatorSet.SetColor(hWindowControl_DispImage.HalconWindow, "blue");
                    HOperatorSet.DispObj(this.Current_disp_regions[hv_Index], hWindowControl_DispImage.HalconWindow);

                    HTuple Width, Height, Area;
                    HOperatorSet.RegionFeatures(this.Current_disp_regions[hv_Index], "width", out Width);
                    HOperatorSet.RegionFeatures(this.Current_disp_regions[hv_Index], "height", out Height);
                    HOperatorSet.RegionFeatures(this.Current_disp_regions[hv_Index], "area", out Area);

                    this.labW.Text = ((double)Width).ToString("#0.000");
                    this.labH.Text = ((double)Height).ToString("#0.000");
                    this.labA.Text = ((double)Area).ToString("#0.000");
                    
                    // 顯示: 灰階範圍 & 平均灰階
                    HTuple min, max, range, mean;
                    HOperatorSet.MinMaxGray(this.Current_disp_regions[hv_Index], this.DispGrayImg, 0, out min, out max, out range); // 法1
                    //HOperatorSet.GrayFeatures(this.Current_disp_regions[hv_Index], this.DispGrayImg, "min", out min); // 法2
                    //HOperatorSet.GrayFeatures(this.Current_disp_regions[hv_Index], this.DispGrayImg, "max", out max); // 法2
                    HOperatorSet.GrayFeatures(this.Current_disp_regions[hv_Index], this.DispGrayImg, "mean", out mean);

                    this.labR.Text = min.ToString() + " ~ " + max.ToString();
                    this.labM.Text = mean.D.ToString("#0.000");
                }
            }
        }

        /// <summary>
        /// 【執行】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Execute_Click(object sender, EventArgs e) // (20200729) Jeff Revised!
        {
            // 更新參數
            this.ui_parameters(true);

            // 執行所有演算法計算
            clsStaticTool.DisposeAll(this.ResultImages);
            clsStaticTool.DisposeAll(this.ResultRegions);
            if (this.AlgoImg.Execute(out this.ResultImages, out this.ResultRegions))
            {
                // 顯示
                HOperatorSet.ClearWindow(hWindowControl_DispImage.HalconWindow);
                HObject dispImg;
                if (this.AlgoImg.ListAlgoImage[this.ID_ResultImg].B_ImageAlgo) // 影像演算法
                    dispImg = this.ResultImages[this.ID_ResultImg];
                else // 區域演算法
                    dispImg = this.Get_InputImage(0);
                HOperatorSet.DispObj(dispImg, hWindowControl_DispImage.HalconWindow);

                HOperatorSet.SetColor(hWindowControl_DispImage.HalconWindow, "red");
                HOperatorSet.SetDraw(hWindowControl_DispImage.HalconWindow, (this.comboBox_DispRegType.SelectedIndex == 1) ? "margin" : "fill");
                HOperatorSet.DispObj(this.ResultRegions[this.ID_ResultImg], hWindowControl_DispImage.HalconWindow);

                this.Update_DispImageInfo(dispImg);
                if (this.AlgoImg.ListAlgoImage[this.ID_ResultImg].B_ImageAlgo == false) // 區域演算法
                    dispImg.Dispose();

                Extension.HObjectMedthods.ReleaseHObject(ref this.Current_disp_regions);
                this.Current_disp_regions = this.ResultRegions[this.ID_ResultImg].Clone();
            }
        }

        /// <summary>
        /// 【確定】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_yes_Click(object sender, EventArgs e) // (20200729) Jeff Revised!
        {
            // 顯示計算結果影像 & 區域
            this.button_Execute_Click(null, null);

            // 更新呼叫端之 Algo_Image類別物件
            //this.AlgoImg_yes.Dispose(); // For: AlgoImg = algoImg.Clone_Manual(true); 和 AlgoImg = algoImg.DeepClone();
            this.AlgoImg_yes = this.AlgoImg;
            clsStaticTool.DisposeAll(this.ResultImages_yes);
            this.ResultImages_yes = this.ResultImages;
            clsStaticTool.DisposeAll(this.ResultRegions_yes); // (20200729) Jeff Revised!
            this.ResultRegions_yes = this.ResultRegions; // (20200729) Jeff Revised!

            DialogResult = DialogResult.Yes; // 會自動關閉表單
        }
        
        /// <summary>
        /// 【取消】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel; // 會自動關閉表單
        }

        /// <summary>
        /// 關閉表單
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Algo_Image_Form_FormClosing(object sender, FormClosingEventArgs e) // (20200729) Jeff Revised!
        {
            if (DialogResult != DialogResult.Yes)
            {
                // 釋放記憶體
                //this.AlgoImg.Dispose(); // For: AlgoImg = algoImg.Clone_Manual(true); 和 AlgoImg = algoImg.DeepClone();
                clsStaticTool.DisposeAll(this.ResultImages);
                clsStaticTool.DisposeAll(this.ResultRegions); // (20200729) Jeff Revised!
            }
        }
    }
}
