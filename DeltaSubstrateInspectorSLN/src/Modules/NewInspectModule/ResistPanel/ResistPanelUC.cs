using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using DeltaSubstrateInspector.src.Modules.NewInspectModule;
using DeltaSubstrateInspector.src.Roles;

using HalconDotNet;
using System.IO;
using System.Diagnostics;

using DAVS;
using static DeltaSubstrateInspector.FileSystem.FileSystem;

using System.Media; // For SystemSounds

//190221, andy
using static DeltaSubstrateInspector.src.Modules.ResultModule.ImageShowForm;

namespace DeltaSubstrateInspector.src.Modules.NewInspectModule.ResistPanel
{
    public partial class ResistPanelUC : UserControl
    {

        #region //=======================  變數設置 =======================

        public event EventHandler OnUserControlButtonClicked;

        //private HObject Display_Image = null;
        private HDrawingObject drawing_Rect = new HDrawingObject(100, 100, 210, 210);

        private ResistPanelRole Recipe = ResistPanelRole.GetSingleton();
        private ResistPanel Method = ResistPanel.GetSingleton(true); // (20191112) Jeff Revised!

        string Path = ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\ResistPanel\\";

        private HObject SegRegions = null;
        private HObject PatternRegions = null;

        List<HObject> Input_ImgList = new List<HObject>();

        int hv_Button = 0;
        int hv_Row = 0, hv_Column = 0;
        private HObject Current_disp_regions = null; // 點擊當前 Window 顯示之某一個 region ，並且顯示其資訊
        private HObject Current_disp_image = null;

        /// <summary>
        /// 是否正在載入Recipe
        /// </summary>
        private bool b_LoadRecipe { get; set; } = false;

        /// <summary>
        /// 是否尚未執行檢測 (For 各單一分頁項目) (Note: 每切換一次分頁，皆會初始化此變數)
        /// </summary>
        private bool b_Initial_State = true;

        #endregion


        #region //=======================  Function =======================

        public ResistPanelUC()
        {
            InitializeComponent();

            this.DisplayWindows.MouseWheel += DisplayWindows.HSmartWindowControl_MouseWheel;
            HOperatorSet.GenEmptyRegion(out Current_disp_regions);
            UpdateParameter();
            //Affine_angle_degree = 0.0; // (20190219) Jeff Revised!

            // (20190225) Jeff Revised!
            combx_Algo.Items.Clear();
            foreach (TabPage TPs in tbInspParamSetting.TabPages)
            {
                combx_Algo.Items.Add(TPs.Text);
            }
            //combx_Algo.SelectedIndex = 0;

            // 初始化【AI只覆判模稜兩可NG】之GUI控制項
            Initialize_dict_GUIControls_notSureNG(); // (20190807) Jeff Revised!
            
            clsLanguage.clsLanguage.SetLanguateToControls(this, true, "false"); // (20200214) Jeff Revised!
        }

        /// <summary>
        /// 更新影像ID
        /// </summary>
        public void UpdateDisplayList()
        {
            // 讀取影像並且轉正
            if (!(Method.Read_AllImages_Insp(Recipe, Input_ImgList)))
                MessageBox.Show("載入影像失敗", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            combxDisplayImg.Items.Clear();
            for (int i = 0; i < Input_ImgList.Count; i++)
            {
                combxDisplayImg.Items.Add(i.ToString());
            }
            if (Input_ImgList.Count > 0)
                combxDisplayImg.SelectedIndex = 0;

            #region 設定各檢測瑕疵之影像ID的最大值
            nud_ImageID_WhiteBlob_FL.Maximum = Input_ImgList.Count - 1;
            nud_ImageID_WhiteBlob_CL.Maximum = Input_ImgList.Count - 1;
            nud_ImageID_BlackCrack_FL.Maximum = Input_ImgList.Count - 1;
            nud_ImageID_BlackBlob_CL.Maximum = Input_ImgList.Count - 1;
            nud_ImageID_BrightBlob_BL.Maximum = Input_ImgList.Count - 1;
            #endregion
        }

        public InspectRole get_role()
        {
            return create_role();
        }

        private ResistPanelRole create_role()
        {
            ResistPanelRole role = new ResistPanelRole();
            role.Method = "ResistPanel";
            return role;
        }

        /// <summary>
        /// 鍵盤事件
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.E)) // 按下 "Ctrl"+"E"
            {
                
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        
        private void ChangeColor(Color C,CheckBox cbx)
        {
            cbx.BackColor = C;
        }

        public void GetRectData(HTuple Rect,out HTuple column1, out HTuple row1, out HTuple column2, out HTuple row2)
        {
            HOperatorSet.GetDrawingObjectParams(Rect, new HTuple("column1"), out column1);
            HOperatorSet.GetDrawingObjectParams(Rect, new HTuple("row1"), out row1);
            HOperatorSet.GetDrawingObjectParams(Rect, new HTuple("column2"), out column2);
            HOperatorSet.GetDrawingObjectParams(Rect, new HTuple("row2"), out row2);
        }

        public void GetRect2Data(HTuple Rect, out HTuple column, out HTuple row, out HTuple phi, out HTuple length1,out HTuple length2)
        {
            HOperatorSet.GetDrawingObjectParams(Rect, new HTuple("column"), out column);
            HOperatorSet.GetDrawingObjectParams(Rect, new HTuple("row"), out row);
            HOperatorSet.GetDrawingObjectParams(Rect, new HTuple("phi"), out phi);
            HOperatorSet.GetDrawingObjectParams(Rect, new HTuple("length1"), out length1);
            HOperatorSet.GetDrawingObjectParams(Rect, new HTuple("length2"), out length2);
        }
        
        public void UpdateParameter() // (20190808) Jeff Revised!
        {
            // Load parameter from XML file
            Recipe.load();

            b_LoadRecipe = true;

            #region 設定各檢測瑕疵之影像ID的最大值

            nud_ImageID_CellSeg_CL.Maximum = Recipe.Param.ImgCount - 1;
            nud_ImageID_CellSeg_BL.Maximum = Recipe.Param.ImgCount - 1;
            nud_ImageID_WhiteBlob_FL.Maximum = Recipe.Param.ImgCount - 1;
            nud_ImageID_WhiteBlob_CL.Maximum = Recipe.Param.ImgCount - 1;
            nud_ImageID_BlackCrack_FL.Maximum = Recipe.Param.ImgCount - 1;
            nud_ImageID_BlackBlob_CL.Maximum = Recipe.Param.ImgCount - 1;
            nud_ImageID_BlackBlob2_CL.Maximum = Recipe.Param.ImgCount - 1;
            nud_ImageID1_Dirty_FLCL.Maximum = Recipe.Param.ImgCount - 1;
            nud_ImageID2_Dirty_FLCL.Maximum = Recipe.Param.ImgCount - 1;
            nud_ImageID_BrightBlob_BL.Maximum = Recipe.Param.ImgCount - 1;
            nud_ImageID_LackAngle_BL.Maximum = Recipe.Param.ImgCount - 1;

            #endregion

            /* Set parameters to UI */
            #region 進階設定內參數

            // 測試模式設定
            cbxTestModeEnabled.Checked = Recipe.Param.TestModeEnabled;
            combxTestModeType.SelectedIndex = Recipe.Param.TestModeType;
            combxTestModeType.Enabled = cbxTestModeEnabled.Checked;
            // AOI儲存影像設定
            cbxSaveImgEnable.Checked = Recipe.Param.SaveAOIImgEnabled;
            combxSaveImgType.SelectedIndex = Recipe.Param.SaveAOIImgType;
            combxSaveImgType.Enabled = cbxSaveImgEnable.Checked;
            // AI Type
            combxAIType.SelectedIndex = Recipe.Param.DAVSInspType;
            // 參數設定
            nudImgCount.Value = Recipe.Param.ImgCount;
            //txbResolution.Text = Recipe.Param.Resolution.ToString();
            txbResolution.Text = Locate_Method_FS.pixel_resolution_.ToString(); // (20190131) Jeff Revised!
            // 黑條方向
            combx_Type_BlackStripe.Text = Recipe.Param.Type_BlackStripe; // (20191112) Jeff Revised!
            // 計算每種瑕疵在Cell的中心位置
            cbx_Compute_df_CellCenter.Checked = Recipe.Param.Enabled_Compute_df_CellCenter;
            // 扭正影像
            cbx_rotate.Checked = Recipe.Param.Enabled_rotate;
            //AI 影像ID
            nudAIImgID.Value = decimal.Parse(Recipe.Param.AIImageID.ToString());
            // 是否啟用AOI判斷絕對NG
            cbx_notSureNG.Checked = Recipe.Param.B_AOI_absolNG; // (20190807) Jeff Revised!

            #endregion

            #region AOI參數
            
            #region 初定位

            cbx_Enabled_NonInspectCell_InitialPosition.Checked = Recipe.Param.PositionParam_Initial.Enabled_NonInspectCell;
            nud_L_NonInspect_InitialPosition.Value = Recipe.Param.PositionParam_Initial.L_NonInspect;
            nud_L_NonInspect_Wsmall_InitialPosition.Value = Recipe.Param.PositionParam_Initial.L_NonInspect_Wsmall;
            nud_L_NonInspect_Wsmall3_InitialPosition.Value = Recipe.Param.PositionParam_Initial.L_NonInspect_Wsmall3;

            // 【分割電阻條】 (20190816) Jeff Revised!
            nud_ImageIndex_SegBlackStripe.Value = Recipe.Param.PositionParam_Initial.ImageIndex_SegBlackStripe;
            nud_MinGray_SegBlackStripe.Value = Recipe.Param.PositionParam_Initial.MinGray_SegBlackStripe;
            nud_MaxGray_SegBlackStripe.Value = Recipe.Param.PositionParam_Initial.MaxGray_SegBlackStripe;
            nud_W_closing_SegBlackStripe.Value = Recipe.Param.PositionParam_Initial.W_closing_SegBlackStripe;
            nud_H_closing_SegBlackStripe.Value = Recipe.Param.PositionParam_Initial.H_closing_SegBlackStripe;
            nud_MaxWidth_BlackCrack.Value = Recipe.Param.PositionParam_Initial.MaxWidth_BlackCrack;
            nud_H_opening_BlackCrack.Value = Recipe.Param.PositionParam_Initial.H_opening_BlackCrack;
            nud_MinW_BlackStripe.Value = Recipe.Param.PositionParam_Initial.MinW_BlackStripe;
            nud_MaxW_BlackStripe.Value = Recipe.Param.PositionParam_Initial.MaxW_BlackStripe;
            nud_MinH_BlackStripe.Value = Recipe.Param.PositionParam_Initial.MinH_BlackStripe;
            nud_MaxH_BlackStripe.Value = Recipe.Param.PositionParam_Initial.MaxH_BlackStripe;
            nud_W_dila_BlackStripe.Value = Recipe.Param.PositionParam_Initial.W_dila_BlackStripe;
            nud_H_dila_BlackStripe.Value = Recipe.Param.PositionParam_Initial.H_dila_BlackStripe;
            nud_H_ero_BlackStripe_small.Value = Recipe.Param.PositionParam_Initial.H_ero_BlackStripe_small;

            #endregion

            #region Cell分割 (同軸光) 之參數

            cbx_CellSeg_CL.Checked = Recipe.Param.InspParam_CellSeg_CL.Enabled;
            nud_ImageID_CellSeg_CL.Value = Recipe.Param.InspParam_CellSeg_CL.ImageIndex;
            cbx_Enabled_removeDef_CellSeg_CL.Checked = Recipe.Param.InspParam_CellSeg_CL.Enabled_removeDef;
            nud_MinGray_removeDef_CellSeg_CL.Value = Recipe.Param.InspParam_CellSeg_CL.MinGray_removeDef;
            nud_MaxGray_removeDef_CellSeg_CL.Value = Recipe.Param.InspParam_CellSeg_CL.MaxGray_removeDef;
            cbx_Enabled_equHisto_CellSeg_CL.Checked = Recipe.Param.InspParam_CellSeg_CL.Enabled_equHisto; // (20191112) Jeff Revised!
            cbx_Enabled_equHisto_part_CellSeg_CL.Checked = Recipe.Param.InspParam_CellSeg_CL.Enabled_equHisto_part; // (20191112) Jeff Revised!
            nud_count_col_CellSeg_CL.Value = Recipe.Param.InspParam_CellSeg_CL.count_col;
            combx_str_BinaryImg_CellSeg_CL.Text = Recipe.Param.InspParam_CellSeg_CL.str_BinaryImg;
            nud_MaskWidth_CellSeg_CL.Value = Recipe.Param.InspParam_CellSeg_CL.MaskWidth;
            nud_MaskHeight_CellSeg_CL.Value = Recipe.Param.InspParam_CellSeg_CL.MaskHeight;
            combx_str_LightDark_CellSeg_CL.Text = Recipe.Param.InspParam_CellSeg_CL.str_LightDark;
            nud_Offset_CellSeg_CL.Value = Recipe.Param.InspParam_CellSeg_CL.Offset;
            nud_MinGray_CellSeg_CL.Value = Recipe.Param.InspParam_CellSeg_CL.MinGray_CellSeg;
            nud_MaxGray_CellSeg_CL.Value = Recipe.Param.InspParam_CellSeg_CL.MaxGray_CellSeg;
            nud_W_opening1_CellSeg_CL.Value = Recipe.Param.InspParam_CellSeg_CL.W_opening1_CellSeg;
            nud_H_opening1_CellSeg_CL.Value = Recipe.Param.InspParam_CellSeg_CL.H_opening1_CellSeg;
            nud_W_closing_CellSeg_CL.Value = Recipe.Param.InspParam_CellSeg_CL.W_closing_CellSeg;
            nud_H_closing_CellSeg_CL.Value = Recipe.Param.InspParam_CellSeg_CL.H_closing_CellSeg;
            nud_W_opening2_CellSeg_CL.Value = Recipe.Param.InspParam_CellSeg_CL.W_opening2_CellSeg;
            nud_H_opening2_CellSeg_CL.Value = Recipe.Param.InspParam_CellSeg_CL.H_opening2_CellSeg;
            nud_W_dila_CellSeg_CL.Value = Recipe.Param.InspParam_CellSeg_CL.W_dila_CellSeg;
            nud_H_dila_CellSeg_CL.Value = Recipe.Param.InspParam_CellSeg_CL.H_dila_CellSeg;
            nud_MinHeight_CellSeg_CL.Value = Recipe.Param.InspParam_CellSeg_CL.MinHeight_CellSeg;
            nud_MaxHeight_CellSeg_CL.Value = Recipe.Param.InspParam_CellSeg_CL.MaxHeight_CellSeg;
            nud_MinWidth_CellSeg_CL.Value = Recipe.Param.InspParam_CellSeg_CL.MinWidth_CellSeg;
            nud_MaxWidth_CellSeg_CL.Value = Recipe.Param.InspParam_CellSeg_CL.MaxWidth_CellSeg;
            combx_str_ModeCellSeg_CellSeg_CL.Text = Recipe.Param.InspParam_CellSeg_CL.str_ModeCellSeg;
            cbx_Enabled_RemainValidCells_CellSeg_CL.Checked = Recipe.Param.InspParam_CellSeg_CL.Enabled_RemainValidCells;

            #endregion
            
            #region Cell分割 (背光) 之參數

            cbx_CellSeg_BL.Checked = Recipe.Param.InspParam_CellSeg_BL.Enabled;
            nud_ImageID_CellSeg_BL.Value = Recipe.Param.InspParam_CellSeg_BL.ImageIndex;
            cbx_Enabled_removeDef_CellSeg_BL.Checked = Recipe.Param.InspParam_CellSeg_BL.Enabled_removeDef;
            nud_MinGray_removeDef_CellSeg_BL.Value = Recipe.Param.InspParam_CellSeg_BL.MinGray_removeDef;
            nud_MaxGray_removeDef_CellSeg_BL.Value = Recipe.Param.InspParam_CellSeg_BL.MaxGray_removeDef;
            cbx_Enabled_equHisto_CellSeg_BL.Checked = Recipe.Param.InspParam_CellSeg_BL.Enabled_equHisto; // (20191112) Jeff Revised!
            cbx_Enabled_equHisto_part_CellSeg_BL.Checked = Recipe.Param.InspParam_CellSeg_BL.Enabled_equHisto_part; // (20191112) Jeff Revised!
            nud_count_col_CellSeg_BL.Value = Recipe.Param.InspParam_CellSeg_BL.count_col;
            combx_str_BinaryImg_CellSeg_BL.Text = Recipe.Param.InspParam_CellSeg_BL.str_BinaryImg;
            nud_MaskWidth_CellSeg_BL.Value = Recipe.Param.InspParam_CellSeg_BL.MaskWidth;
            nud_MaskHeight_CellSeg_BL.Value = Recipe.Param.InspParam_CellSeg_BL.MaskHeight;
            combx_str_LightDark_CellSeg_BL.Text = Recipe.Param.InspParam_CellSeg_BL.str_LightDark;
            nud_Offset_CellSeg_BL.Value = Recipe.Param.InspParam_CellSeg_BL.Offset;
            nud_MinGray_CellSeg_BL.Value = Recipe.Param.InspParam_CellSeg_BL.MinGray_CellSeg;
            nud_MaxGray_CellSeg_BL.Value = Recipe.Param.InspParam_CellSeg_BL.MaxGray_CellSeg;
            nud_W_opening1_CellSeg_BL.Value = Recipe.Param.InspParam_CellSeg_BL.W_opening1_CellSeg;
            nud_H_opening1_CellSeg_BL.Value = Recipe.Param.InspParam_CellSeg_BL.H_opening1_CellSeg;
            nud_W_closing_CellSeg_BL.Value = Recipe.Param.InspParam_CellSeg_BL.W_closing_CellSeg;
            nud_H_closing_CellSeg_BL.Value = Recipe.Param.InspParam_CellSeg_BL.H_closing_CellSeg;
            nud_W_opening2_CellSeg_BL.Value = Recipe.Param.InspParam_CellSeg_BL.W_opening2_CellSeg;
            nud_H_opening2_CellSeg_BL.Value = Recipe.Param.InspParam_CellSeg_BL.H_opening2_CellSeg;
            nud_W_dila_CellSeg_BL.Value = Recipe.Param.InspParam_CellSeg_BL.W_dila_CellSeg;
            nud_H_dila_CellSeg_BL.Value = Recipe.Param.InspParam_CellSeg_BL.H_dila_CellSeg;
            nud_MinHeight_CellSeg_BL.Value = Recipe.Param.InspParam_CellSeg_BL.MinHeight_CellSeg;
            nud_MaxHeight_CellSeg_BL.Value = Recipe.Param.InspParam_CellSeg_BL.MaxHeight_CellSeg;
            nud_MinWidth_CellSeg_BL.Value = Recipe.Param.InspParam_CellSeg_BL.MinWidth_CellSeg;
            nud_MaxWidth_CellSeg_BL.Value = Recipe.Param.InspParam_CellSeg_BL.MaxWidth_CellSeg;
            combx_str_ModeCellSeg_CellSeg_BL.Text = Recipe.Param.InspParam_CellSeg_BL.str_ModeCellSeg;
            cbx_Enabled_RemainValidCells_CellSeg_BL.Checked = Recipe.Param.InspParam_CellSeg_BL.Enabled_RemainValidCells;

            #endregion
            
            #region 瑕疵Cell判斷範圍 之參數

            cbx_Enabled_InspReg_Df_CellReg.Checked = Recipe.Param.InspParam_Df_CellReg.Enabled_InspReg;
            nud_W_InspReg_Df_CellReg.Value = Recipe.Param.InspParam_Df_CellReg.W_InspReg;
            nud_H_InspReg_Df_CellReg.Value = Recipe.Param.InspParam_Df_CellReg.H_InspReg;
            nud_W_Cell_Df_CellReg.Value = Recipe.Param.InspParam_Df_CellReg.W_Cell;
            nud_H_Cell_Df_CellReg.Value = Recipe.Param.InspParam_Df_CellReg.H_Cell;

            #endregion

            #region 檢測黑條內白點 (正光) (Frontal light) 之參數

            cbx_WhiteBlob_FL.Checked = Recipe.Param.InspParam_WhiteBlob_FL.Enabled;
            nud_ImageID_WhiteBlob_FL.Value = Recipe.Param.InspParam_WhiteBlob_FL.ImageIndex;
            cbx_Enabled_Wsmall_WhiteBlob_FL.Checked = Recipe.Param.InspParam_WhiteBlob_FL.Enabled_Wsmall;
            combx_str_L_NonInspect_WhiteBlob_FL.Text = Recipe.Param.InspParam_WhiteBlob_FL.str_L_NonInspect;
            cbx_Enabled_Cell_Step1_WhiteBlob_FL.Checked = Recipe.Param.InspParam_WhiteBlob_FL.Enabled_Cell;
            cbx_Enabled_Cell_Step2_WhiteBlob_FL.Checked = Recipe.Param.InspParam_WhiteBlob_FL.Enabled_Cell;
            nud_W_Cell_WhiteBlob_FL.Value = Recipe.Param.InspParam_WhiteBlob_FL.W_Cell;
            nud_H_Cell_WhiteBlob_FL.Value = Recipe.Param.InspParam_WhiteBlob_FL.H_Cell;
            cbx_Enabled_BypassReg_WhiteBlob_FL.Checked = Recipe.Param.InspParam_WhiteBlob_FL.Enabled_BypassReg;
            nud_x_BypassReg_WhiteBlob_FL.Value = Recipe.Param.InspParam_WhiteBlob_FL.x_BypassReg;
            nud_y_BypassReg_WhiteBlob_FL.Value = Recipe.Param.InspParam_WhiteBlob_FL.y_BypassReg;
            nud_W_BypassReg_WhiteBlob_FL.Value = Recipe.Param.InspParam_WhiteBlob_FL.W_BypassReg;
            nud_H_BypassReg_WhiteBlob_FL.Value = Recipe.Param.InspParam_WhiteBlob_FL.H_BypassReg;
            cbx_Enabled_BlackStripe_Step1_WhiteBlob_FL.Checked = Recipe.Param.InspParam_WhiteBlob_FL.Enabled_BlackStripe;
            cbx_Enabled_BlackStripe_Step2_WhiteBlob_FL.Checked = Recipe.Param.InspParam_WhiteBlob_FL.Enabled_BlackStripe;
            combx_str_ModeBlackStripe_WhiteBlob_FL.Text = Recipe.Param.InspParam_WhiteBlob_FL.str_ModeBlackStripe;
            nud_H_BlackStripe_WhiteBlob_FL.Value = Recipe.Param.InspParam_WhiteBlob_FL.H_BlackStripe;
            nud_H_BlackStripe_mode2_WhiteBlob_FL.Value = Recipe.Param.InspParam_WhiteBlob_FL.H_BlackStripe_mode2;
            combx_str_BinaryImg_Cell_WhiteBlob_FL.Text = Recipe.Param.InspParam_WhiteBlob_FL.str_BinaryImg_Cell;
            nud_MaskWidth_1_Cell_WhiteBlob_FL.Value = Recipe.Param.InspParam_WhiteBlob_FL.MaskWidth_Cell;
            nud_MaskHeight_1_Cell_WhiteBlob_FL.Value = Recipe.Param.InspParam_WhiteBlob_FL.MaskHeight_Cell;
            combx_str_LightDark_1_Cell_WhiteBlob_FL.Text = Recipe.Param.InspParam_WhiteBlob_FL.str_LightDark_Cell;
            nud_Offset_1_Cell_WhiteBlob_FL.Value = Recipe.Param.InspParam_WhiteBlob_FL.Offset_Cell;
            nud_MinGray_1_Cell_WhiteBlob_FL.Value = Recipe.Param.InspParam_WhiteBlob_FL.MinGray_Cell;
            nud_MaxGray_1_Cell_WhiteBlob_FL.Value = Recipe.Param.InspParam_WhiteBlob_FL.MaxGray_Cell;
            combx_str_BinaryImg_Resist_WhiteBlob_FL.Text = Recipe.Param.InspParam_WhiteBlob_FL.str_BinaryImg_Resist;
            nud_MaskWidth_1_Resist_WhiteBlob_FL.Value = Recipe.Param.InspParam_WhiteBlob_FL.MaskWidth_Resist;
            nud_MaskHeight_1_Resist_WhiteBlob_FL.Value = Recipe.Param.InspParam_WhiteBlob_FL.MaskHeight_Resist;
            combx_str_LightDark_1_Resist_WhiteBlob_FL.Text = Recipe.Param.InspParam_WhiteBlob_FL.str_LightDark_Resist;
            nud_Offset_1_Resist_WhiteBlob_FL.Value = Recipe.Param.InspParam_WhiteBlob_FL.Offset_Resist;
            nud_MinGray_1_Resist_WhiteBlob_FL.Value = Recipe.Param.InspParam_WhiteBlob_FL.MinGray_Resist;
            nud_MaxGray_1_Resist_WhiteBlob_FL.Value = Recipe.Param.InspParam_WhiteBlob_FL.MaxGray_Resist;

            // 規格2 (20190411) Jeff Revised!
            nud_count_BinImg_Cell_WhiteBlob_FL.Value = Recipe.Param.InspParam_WhiteBlob_FL.count_BinImg_Cell;
            nud_MinGray_2_Cell_WhiteBlob_FL.Value = Recipe.Param.InspParam_WhiteBlob_FL.MinGray_2_Cell;
            nud_MaxGray_2_Cell_WhiteBlob_FL.Value = Recipe.Param.InspParam_WhiteBlob_FL.MaxGray_2_Cell;
            nud_MaskWidth_2_Cell_WhiteBlob_FL.Value = Recipe.Param.InspParam_WhiteBlob_FL.MaskWidth_2_Cell;
            nud_MaskHeight_2_Cell_WhiteBlob_FL.Value = Recipe.Param.InspParam_WhiteBlob_FL.MaskHeight_2_Cell;
            combx_str_LightDark_2_Cell_WhiteBlob_FL.Text = Recipe.Param.InspParam_WhiteBlob_FL.str_LightDark_2_Cell;
            nud_Offset_2_Cell_WhiteBlob_FL.Value = Recipe.Param.InspParam_WhiteBlob_FL.Offset_2_Cell;
            nud_count_BinImg_Resist_WhiteBlob_FL.Value = Recipe.Param.InspParam_WhiteBlob_FL.count_BinImg_Resist;
            nud_MinGray_2_Resist_WhiteBlob_FL.Value = Recipe.Param.InspParam_WhiteBlob_FL.MinGray_2_Resist;
            nud_MaxGray_2_Resist_WhiteBlob_FL.Value = Recipe.Param.InspParam_WhiteBlob_FL.MaxGray_2_Resist;
            nud_MaskWidth_2_Resist_WhiteBlob_FL.Value = Recipe.Param.InspParam_WhiteBlob_FL.MaskWidth_2_Resist;
            nud_MaskHeight_2_Resist_WhiteBlob_FL.Value = Recipe.Param.InspParam_WhiteBlob_FL.MaskHeight_2_Resist;
            combx_str_LightDark_2_Resist_WhiteBlob_FL.Text = Recipe.Param.InspParam_WhiteBlob_FL.str_LightDark_2_Resist;
            nud_Offset_2_Resist_WhiteBlob_FL.Value = Recipe.Param.InspParam_WhiteBlob_FL.Offset_2_Resist;

            cbx_Enabled_Gray_select_Cell_WhiteBlob_FL.Checked = Recipe.Param.InspParam_WhiteBlob_FL.Enabled_Gray_select_Cell;
            combx_str_Gray_select_Cell_WhiteBlob_FL.Text = Recipe.Param.InspParam_WhiteBlob_FL.str_Gray_select_Cell;
            nud_Gray_select_Cell_WhiteBlob_FL.Value = Recipe.Param.InspParam_WhiteBlob_FL.Gray_select_Cell;
            cbx_Enabled_Gray_select_Resist_WhiteBlob_FL.Checked = Recipe.Param.InspParam_WhiteBlob_FL.Enabled_Gray_select_Resist;
            combx_str_Gray_select_Resist_WhiteBlob_FL.Text = Recipe.Param.InspParam_WhiteBlob_FL.str_Gray_select_Resist;
            nud_Gray_select_Resist_WhiteBlob_FL.Value = Recipe.Param.InspParam_WhiteBlob_FL.Gray_select_Resist;
            cbx_Enabled_Cell2_Step4_WhiteBlob_FL.Checked = Recipe.Param.InspParam_WhiteBlob_FL.Enabled_Cell2;
            nud_W_Cell2_WhiteBlob_FL.Value = Recipe.Param.InspParam_WhiteBlob_FL.W_Cell2;
            nud_H_Cell2_WhiteBlob_FL.Value = Recipe.Param.InspParam_WhiteBlob_FL.H_Cell2;
            nud_count_op1_WhiteBlob_FL.Value = Recipe.Param.InspParam_WhiteBlob_FL.count_op1;
            cbx_Enabled_Height_WhiteBlob_FL.Checked = Recipe.Param.InspParam_WhiteBlob_FL.Enabled_Height;
            cbx_Enabled_Width_WhiteBlob_FL.Checked = Recipe.Param.InspParam_WhiteBlob_FL.Enabled_Width;
            nud_MinH_df_WhiteBlob_FL.Value = Recipe.Param.InspParam_WhiteBlob_FL.MinHeight_defect;
            nud_MaxH_df_WhiteBlob_FL.Value = Recipe.Param.InspParam_WhiteBlob_FL.MaxHeight_defect;
            nud_MinW_df_WhiteBlob_FL.Value = Recipe.Param.InspParam_WhiteBlob_FL.MinWidth_defect;
            nud_MaxW_df_WhiteBlob_FL.Value = Recipe.Param.InspParam_WhiteBlob_FL.MaxWidth_defect;
            combx_str_op1_WhiteBlob_FL.Text = Recipe.Param.InspParam_WhiteBlob_FL.str_op1;
            cbx_Enabled_Height_op1_2_WhiteBlob_FL.Checked = Recipe.Param.InspParam_WhiteBlob_FL.Enabled_Height_op1_2;
            cbx_Enabled_Width_op1_2_WhiteBlob_FL.Checked = Recipe.Param.InspParam_WhiteBlob_FL.Enabled_Width_op1_2;
            nud_MinH_df_op1_2_WhiteBlob_FL.Value = Recipe.Param.InspParam_WhiteBlob_FL.MinHeight_defect_op1_2;
            nud_MaxH_df_op1_2_WhiteBlob_FL.Value = Recipe.Param.InspParam_WhiteBlob_FL.MaxHeight_defect_op1_2;
            nud_MinW_df_op1_2_WhiteBlob_FL.Value = Recipe.Param.InspParam_WhiteBlob_FL.MinWidth_defect_op1_2;
            nud_MaxW_df_op1_2_WhiteBlob_FL.Value = Recipe.Param.InspParam_WhiteBlob_FL.MaxWidth_defect_op1_2;
            combx_str_op1_2_WhiteBlob_FL.Text = Recipe.Param.InspParam_WhiteBlob_FL.str_op1_2;
            cbx_Enabled_Area_WhiteBlob_FL.Checked = Recipe.Param.InspParam_WhiteBlob_FL.Enabled_Area;
            nud_MinA_df_WhiteBlob_FL.Value = Recipe.Param.InspParam_WhiteBlob_FL.MinArea_defect;
            nud_MaxA_df_WhiteBlob_FL.Value = Recipe.Param.InspParam_WhiteBlob_FL.MaxArea_defect;
            combx_str_op2_WhiteBlob_FL.Text = Recipe.Param.InspParam_WhiteBlob_FL.str_op2;

            // 【AI只覆判模稜兩可NG】
            cbx_notSureNG_WhiteBlob_FL.Checked = Recipe.Param.InspParam_WhiteBlob_FL.cls_absolNG.Enabled;
            cbx_Enabled_Gray_select_WhiteBlob_FL_AbsolNG.Checked = Recipe.Param.InspParam_WhiteBlob_FL.cls_absolNG.Enabled_Gray_select;
            combx_str_Gray_select_WhiteBlob_FL_AbsolNG.Text = Recipe.Param.InspParam_WhiteBlob_FL.cls_absolNG.str_Gray_select;
            nud_Gray_select_WhiteBlob_FL_AbsolNG.Value = Recipe.Param.InspParam_WhiteBlob_FL.cls_absolNG.Gray_select;
            combx_str_op_WhiteBlob_FL_AbsolNG.Text = Recipe.Param.InspParam_WhiteBlob_FL.cls_absolNG.str_op;
            cbx_Enabled_Area_WhiteBlob_FL_AbsolNG.Checked = Recipe.Param.InspParam_WhiteBlob_FL.cls_absolNG.Enabled_Area;
            nud_MinA_df_WhiteBlob_FL_AbsolNG.Value = Recipe.Param.InspParam_WhiteBlob_FL.cls_absolNG.MinArea_defect;
            nud_MaxA_df_WhiteBlob_FL_AbsolNG.Value = Recipe.Param.InspParam_WhiteBlob_FL.cls_absolNG.MaxArea_defect;

            #endregion

            #region 檢測黑條內白點 (同軸光) (Coaxial light) 之參數

            cbx_WhiteBlob_CL.Checked = Recipe.Param.InspParam_WhiteBlob_CL.Enabled;
            nud_ImageID_WhiteBlob_CL.Value = Recipe.Param.InspParam_WhiteBlob_CL.ImageIndex;
            cbx_Enabled_Wsmall_WhiteBlob_CL.Checked = Recipe.Param.InspParam_WhiteBlob_CL.Enabled_Wsmall;
            combx_str_L_NonInspect_WhiteBlob_CL.Text = Recipe.Param.InspParam_WhiteBlob_CL.str_L_NonInspect;
            cbx_Enabled_Cell_Step1_WhiteBlob_CL.Checked = Recipe.Param.InspParam_WhiteBlob_CL.Enabled_Cell;
            cbx_Enabled_Cell_Step2_WhiteBlob_CL.Checked = Recipe.Param.InspParam_WhiteBlob_CL.Enabled_Cell;
            nud_W_Cell_WhiteBlob_CL.Value = Recipe.Param.InspParam_WhiteBlob_CL.W_Cell;
            nud_H_Cell_WhiteBlob_CL.Value = Recipe.Param.InspParam_WhiteBlob_CL.H_Cell;
            cbx_Enabled_BypassReg_WhiteBlob_CL.Checked = Recipe.Param.InspParam_WhiteBlob_CL.Enabled_BypassReg;
            nud_x_BypassReg_WhiteBlob_CL.Value = Recipe.Param.InspParam_WhiteBlob_CL.x_BypassReg;
            nud_y_BypassReg_WhiteBlob_CL.Value = Recipe.Param.InspParam_WhiteBlob_CL.y_BypassReg;
            nud_W_BypassReg_WhiteBlob_CL.Value = Recipe.Param.InspParam_WhiteBlob_CL.W_BypassReg;
            nud_H_BypassReg_WhiteBlob_CL.Value = Recipe.Param.InspParam_WhiteBlob_CL.H_BypassReg;
            cbx_Enabled_BlackStripe_Step1_WhiteBlob_CL.Checked = Recipe.Param.InspParam_WhiteBlob_CL.Enabled_BlackStripe;
            cbx_Enabled_BlackStripe_Step2_WhiteBlob_CL.Checked = Recipe.Param.InspParam_WhiteBlob_CL.Enabled_BlackStripe;
            combx_str_ModeBlackStripe_WhiteBlob_CL.Text = Recipe.Param.InspParam_WhiteBlob_CL.str_ModeBlackStripe;
            nud_H_BlackStripe_WhiteBlob_CL.Value = Recipe.Param.InspParam_WhiteBlob_CL.H_BlackStripe;
            nud_H_BlackStripe_mode2_WhiteBlob_CL.Value = Recipe.Param.InspParam_WhiteBlob_CL.H_BlackStripe_mode2;
            combx_str_BinaryImg_Cell_WhiteBlob_CL.Text = Recipe.Param.InspParam_WhiteBlob_CL.str_BinaryImg_Cell;
            nud_MaskWidth_1_Cell_WhiteBlob_CL.Value = Recipe.Param.InspParam_WhiteBlob_CL.MaskWidth_Cell;
            nud_MaskHeight_1_Cell_WhiteBlob_CL.Value = Recipe.Param.InspParam_WhiteBlob_CL.MaskHeight_Cell;
            combx_str_LightDark_1_Cell_WhiteBlob_CL.Text = Recipe.Param.InspParam_WhiteBlob_CL.str_LightDark_Cell;
            nud_Offset_1_Cell_WhiteBlob_CL.Value = Recipe.Param.InspParam_WhiteBlob_CL.Offset_Cell;
            nud_MinGray_1_Cell_WhiteBlob_CL.Value = Recipe.Param.InspParam_WhiteBlob_CL.MinGray_Cell;
            nud_MaxGray_1_Cell_WhiteBlob_CL.Value = Recipe.Param.InspParam_WhiteBlob_CL.MaxGray_Cell;
            combx_str_BinaryImg_Resist_WhiteBlob_CL.Text = Recipe.Param.InspParam_WhiteBlob_CL.str_BinaryImg_Resist;
            nud_MaskWidth_1_Resist_WhiteBlob_CL.Value = Recipe.Param.InspParam_WhiteBlob_CL.MaskWidth_Resist;
            nud_MaskHeight_1_Resist_WhiteBlob_CL.Value = Recipe.Param.InspParam_WhiteBlob_CL.MaskHeight_Resist;
            combx_str_LightDark_1_Resist_WhiteBlob_CL.Text = Recipe.Param.InspParam_WhiteBlob_CL.str_LightDark_Resist;
            nud_Offset_1_Resist_WhiteBlob_CL.Value = Recipe.Param.InspParam_WhiteBlob_CL.Offset_Resist;
            nud_MinGray_1_Resist_WhiteBlob_CL.Value = Recipe.Param.InspParam_WhiteBlob_CL.MinGray_Resist;
            nud_MaxGray_1_Resist_WhiteBlob_CL.Value = Recipe.Param.InspParam_WhiteBlob_CL.MaxGray_Resist;

            // 規格2 (20190411) Jeff Revised!
            nud_count_BinImg_Cell_WhiteBlob_CL.Value = Recipe.Param.InspParam_WhiteBlob_CL.count_BinImg_Cell;
            nud_MinGray_2_Cell_WhiteBlob_CL.Value = Recipe.Param.InspParam_WhiteBlob_CL.MinGray_2_Cell;
            nud_MaxGray_2_Cell_WhiteBlob_CL.Value = Recipe.Param.InspParam_WhiteBlob_CL.MaxGray_2_Cell;
            nud_MaskWidth_2_Cell_WhiteBlob_CL.Value = Recipe.Param.InspParam_WhiteBlob_CL.MaskWidth_2_Cell;
            nud_MaskHeight_2_Cell_WhiteBlob_CL.Value = Recipe.Param.InspParam_WhiteBlob_CL.MaskHeight_2_Cell;
            combx_str_LightDark_2_Cell_WhiteBlob_CL.Text = Recipe.Param.InspParam_WhiteBlob_CL.str_LightDark_2_Cell;
            nud_Offset_2_Cell_WhiteBlob_CL.Value = Recipe.Param.InspParam_WhiteBlob_CL.Offset_2_Cell;
            nud_count_BinImg_Resist_WhiteBlob_CL.Value = Recipe.Param.InspParam_WhiteBlob_CL.count_BinImg_Resist;
            nud_MinGray_2_Resist_WhiteBlob_CL.Value = Recipe.Param.InspParam_WhiteBlob_CL.MinGray_2_Resist;
            nud_MaxGray_2_Resist_WhiteBlob_CL.Value = Recipe.Param.InspParam_WhiteBlob_CL.MaxGray_2_Resist;
            nud_MaskWidth_2_Resist_WhiteBlob_CL.Value = Recipe.Param.InspParam_WhiteBlob_CL.MaskWidth_2_Resist;
            nud_MaskHeight_2_Resist_WhiteBlob_CL.Value = Recipe.Param.InspParam_WhiteBlob_CL.MaskHeight_2_Resist;
            combx_str_LightDark_2_Resist_WhiteBlob_CL.Text = Recipe.Param.InspParam_WhiteBlob_CL.str_LightDark_2_Resist;
            nud_Offset_2_Resist_WhiteBlob_CL.Value = Recipe.Param.InspParam_WhiteBlob_CL.Offset_2_Resist;

            cbx_Enabled_Gray_select_Cell_WhiteBlob_CL.Checked = Recipe.Param.InspParam_WhiteBlob_CL.Enabled_Gray_select_Cell;
            combx_str_Gray_select_Cell_WhiteBlob_CL.Text = Recipe.Param.InspParam_WhiteBlob_CL.str_Gray_select_Cell;
            nud_Gray_select_Cell_WhiteBlob_CL.Value = Recipe.Param.InspParam_WhiteBlob_CL.Gray_select_Cell;
            cbx_Enabled_Gray_select_Resist_WhiteBlob_CL.Checked = Recipe.Param.InspParam_WhiteBlob_CL.Enabled_Gray_select_Resist;
            combx_str_Gray_select_Resist_WhiteBlob_CL.Text = Recipe.Param.InspParam_WhiteBlob_CL.str_Gray_select_Resist;
            nud_Gray_select_Resist_WhiteBlob_CL.Value = Recipe.Param.InspParam_WhiteBlob_CL.Gray_select_Resist;
            cbx_Enabled_Cell2_Step4_WhiteBlob_CL.Checked = Recipe.Param.InspParam_WhiteBlob_CL.Enabled_Cell2;
            nud_W_Cell2_WhiteBlob_CL.Value = Recipe.Param.InspParam_WhiteBlob_CL.W_Cell2;
            nud_H_Cell2_WhiteBlob_CL.Value = Recipe.Param.InspParam_WhiteBlob_CL.H_Cell2;
            nud_count_op1_WhiteBlob_CL.Value = Recipe.Param.InspParam_WhiteBlob_CL.count_op1;
            cbx_Enabled_Height_WhiteBlob_CL.Checked = Recipe.Param.InspParam_WhiteBlob_CL.Enabled_Height;
            cbx_Enabled_Width_WhiteBlob_CL.Checked = Recipe.Param.InspParam_WhiteBlob_CL.Enabled_Width;
            nud_MinH_df_WhiteBlob_CL.Value = Recipe.Param.InspParam_WhiteBlob_CL.MinHeight_defect;
            nud_MaxH_df_WhiteBlob_CL.Value = Recipe.Param.InspParam_WhiteBlob_CL.MaxHeight_defect;
            nud_MinW_df_WhiteBlob_CL.Value = Recipe.Param.InspParam_WhiteBlob_CL.MinWidth_defect;
            nud_MaxW_df_WhiteBlob_CL.Value = Recipe.Param.InspParam_WhiteBlob_CL.MaxWidth_defect;
            combx_str_op1_WhiteBlob_CL.Text = Recipe.Param.InspParam_WhiteBlob_CL.str_op1;
            cbx_Enabled_Height_op1_2_WhiteBlob_CL.Checked = Recipe.Param.InspParam_WhiteBlob_CL.Enabled_Height_op1_2;
            cbx_Enabled_Width_op1_2_WhiteBlob_CL.Checked = Recipe.Param.InspParam_WhiteBlob_CL.Enabled_Width_op1_2;
            nud_MinH_df_op1_2_WhiteBlob_CL.Value = Recipe.Param.InspParam_WhiteBlob_CL.MinHeight_defect_op1_2;
            nud_MaxH_df_op1_2_WhiteBlob_CL.Value = Recipe.Param.InspParam_WhiteBlob_CL.MaxHeight_defect_op1_2;
            nud_MinW_df_op1_2_WhiteBlob_CL.Value = Recipe.Param.InspParam_WhiteBlob_CL.MinWidth_defect_op1_2;
            nud_MaxW_df_op1_2_WhiteBlob_CL.Value = Recipe.Param.InspParam_WhiteBlob_CL.MaxWidth_defect_op1_2;
            combx_str_op1_2_WhiteBlob_CL.Text = Recipe.Param.InspParam_WhiteBlob_CL.str_op1_2;
            cbx_Enabled_Area_WhiteBlob_CL.Checked = Recipe.Param.InspParam_WhiteBlob_CL.Enabled_Area;
            nud_MinA_df_WhiteBlob_CL.Value = Recipe.Param.InspParam_WhiteBlob_CL.MinArea_defect;
            nud_MaxA_df_WhiteBlob_CL.Value = Recipe.Param.InspParam_WhiteBlob_CL.MaxArea_defect;
            combx_str_op2_WhiteBlob_CL.Text = Recipe.Param.InspParam_WhiteBlob_CL.str_op2;

            // 【AI只覆判模稜兩可NG】
            cbx_notSureNG_WhiteBlob_CL.Checked = Recipe.Param.InspParam_WhiteBlob_CL.cls_absolNG.Enabled;
            cbx_Enabled_Gray_select_WhiteBlob_CL_AbsolNG.Checked = Recipe.Param.InspParam_WhiteBlob_CL.cls_absolNG.Enabled_Gray_select;
            combx_str_Gray_select_WhiteBlob_CL_AbsolNG.Text = Recipe.Param.InspParam_WhiteBlob_CL.cls_absolNG.str_Gray_select;
            nud_Gray_select_WhiteBlob_CL_AbsolNG.Value = Recipe.Param.InspParam_WhiteBlob_CL.cls_absolNG.Gray_select;
            combx_str_op_WhiteBlob_CL_AbsolNG.Text = Recipe.Param.InspParam_WhiteBlob_CL.cls_absolNG.str_op;
            cbx_Enabled_Area_WhiteBlob_CL_AbsolNG.Checked = Recipe.Param.InspParam_WhiteBlob_CL.cls_absolNG.Enabled_Area;
            nud_MinA_df_WhiteBlob_CL_AbsolNG.Value = Recipe.Param.InspParam_WhiteBlob_CL.cls_absolNG.MinArea_defect;
            nud_MaxA_df_WhiteBlob_CL_AbsolNG.Value = Recipe.Param.InspParam_WhiteBlob_CL.cls_absolNG.MaxArea_defect;

            #endregion

            #region 檢測絲狀異物 (正光) (Frontal Light) 之參數

            cbx_Line_FL.Checked = Recipe.Param.InspParam_Line_FL.Enabled;
            nud_ImageID_Line_FL.Value = Recipe.Param.InspParam_Line_FL.ImageIndex;
            cbx_Enabled_Wsmall_Line_FL.Checked = Recipe.Param.InspParam_Line_FL.Enabled_Wsmall;
            combx_str_L_NonInspect_Line_FL.Text = Recipe.Param.InspParam_Line_FL.str_L_NonInspect;
            cbx_Enabled_Cell_Step1_Line_FL.Checked = Recipe.Param.InspParam_Line_FL.Enabled_Cell;
            cbx_Enabled_Cell_Step2_Line_FL.Checked = Recipe.Param.InspParam_Line_FL.Enabled_Cell;
            nud_W_Cell_Line_FL.Value = Recipe.Param.InspParam_Line_FL.W_Cell;
            nud_H_Cell_Line_FL.Value = Recipe.Param.InspParam_Line_FL.H_Cell;
            cbx_Enabled_BypassReg_Line_FL.Checked = Recipe.Param.InspParam_Line_FL.Enabled_BypassReg;
            nud_x_BypassReg_Line_FL.Value = Recipe.Param.InspParam_Line_FL.x_BypassReg;
            nud_y_BypassReg_Line_FL.Value = Recipe.Param.InspParam_Line_FL.y_BypassReg;
            nud_W_BypassReg_Line_FL.Value = Recipe.Param.InspParam_Line_FL.W_BypassReg;
            nud_H_BypassReg_Line_FL.Value = Recipe.Param.InspParam_Line_FL.H_BypassReg;
            cbx_Enabled_BlackStripe_Step1_Line_FL.Checked = Recipe.Param.InspParam_Line_FL.Enabled_BlackStripe;
            cbx_Enabled_BlackStripe_Step2_Line_FL.Checked = Recipe.Param.InspParam_Line_FL.Enabled_BlackStripe;
            combx_str_ModeBlackStripe_Line_FL.Text = Recipe.Param.InspParam_Line_FL.str_ModeBlackStripe;
            nud_H_BlackStripe_Line_FL.Value = Recipe.Param.InspParam_Line_FL.H_BlackStripe;
            nud_H_BlackStripe_mode2_Line_FL.Value = Recipe.Param.InspParam_Line_FL.H_BlackStripe_mode2;
            combx_str_BinaryImg_Cell_Line_FL.Text = Recipe.Param.InspParam_Line_FL.str_BinaryImg_Cell;
            nud_MaskWidth_1_Cell_Line_FL.Value = Recipe.Param.InspParam_Line_FL.MaskWidth_Cell;
            nud_MaskHeight_1_Cell_Line_FL.Value = Recipe.Param.InspParam_Line_FL.MaskHeight_Cell;
            combx_str_LightDark_1_Cell_Line_FL.Text = Recipe.Param.InspParam_Line_FL.str_LightDark_Cell;
            nud_Offset_1_Cell_Line_FL.Value = Recipe.Param.InspParam_Line_FL.Offset_Cell;
            nud_MinGray_1_Cell_Line_FL.Value = Recipe.Param.InspParam_Line_FL.MinGray_Cell;
            nud_MaxGray_1_Cell_Line_FL.Value = Recipe.Param.InspParam_Line_FL.MaxGray_Cell;
            combx_str_BinaryImg_Resist_Line_FL.Text = Recipe.Param.InspParam_Line_FL.str_BinaryImg_Resist;
            nud_MaskWidth_1_Resist_Line_FL.Value = Recipe.Param.InspParam_Line_FL.MaskWidth_Resist;
            nud_MaskHeight_1_Resist_Line_FL.Value = Recipe.Param.InspParam_Line_FL.MaskHeight_Resist;
            combx_str_LightDark_1_Resist_Line_FL.Text = Recipe.Param.InspParam_Line_FL.str_LightDark_Resist;
            nud_Offset_1_Resist_Line_FL.Value = Recipe.Param.InspParam_Line_FL.Offset_Resist;
            nud_MinGray_1_Resist_Line_FL.Value = Recipe.Param.InspParam_Line_FL.MinGray_Resist;
            nud_MaxGray_1_Resist_Line_FL.Value = Recipe.Param.InspParam_Line_FL.MaxGray_Resist;

            // 規格2 (20190411) Jeff Revised!
            nud_count_BinImg_Cell_Line_FL.Value = Recipe.Param.InspParam_Line_FL.count_BinImg_Cell;
            nud_MinGray_2_Cell_Line_FL.Value = Recipe.Param.InspParam_Line_FL.MinGray_2_Cell;
            nud_MaxGray_2_Cell_Line_FL.Value = Recipe.Param.InspParam_Line_FL.MaxGray_2_Cell;
            nud_MaskWidth_2_Cell_Line_FL.Value = Recipe.Param.InspParam_Line_FL.MaskWidth_2_Cell;
            nud_MaskHeight_2_Cell_Line_FL.Value = Recipe.Param.InspParam_Line_FL.MaskHeight_2_Cell;
            combx_str_LightDark_2_Cell_Line_FL.Text = Recipe.Param.InspParam_Line_FL.str_LightDark_2_Cell;
            nud_Offset_2_Cell_Line_FL.Value = Recipe.Param.InspParam_Line_FL.Offset_2_Cell;
            nud_count_BinImg_Resist_Line_FL.Value = Recipe.Param.InspParam_Line_FL.count_BinImg_Resist;
            nud_MinGray_2_Resist_Line_FL.Value = Recipe.Param.InspParam_Line_FL.MinGray_2_Resist;
            nud_MaxGray_2_Resist_Line_FL.Value = Recipe.Param.InspParam_Line_FL.MaxGray_2_Resist;
            nud_MaskWidth_2_Resist_Line_FL.Value = Recipe.Param.InspParam_Line_FL.MaskWidth_2_Resist;
            nud_MaskHeight_2_Resist_Line_FL.Value = Recipe.Param.InspParam_Line_FL.MaskHeight_2_Resist;
            combx_str_LightDark_2_Resist_Line_FL.Text = Recipe.Param.InspParam_Line_FL.str_LightDark_2_Resist;
            nud_Offset_2_Resist_Line_FL.Value = Recipe.Param.InspParam_Line_FL.Offset_2_Resist;

            cbx_Enabled_Gray_select_Cell_Line_FL.Checked = Recipe.Param.InspParam_Line_FL.Enabled_Gray_select_Cell;
            combx_str_Gray_select_Cell_Line_FL.Text = Recipe.Param.InspParam_Line_FL.str_Gray_select_Cell;
            nud_Gray_select_Cell_Line_FL.Value = Recipe.Param.InspParam_Line_FL.Gray_select_Cell;
            cbx_Enabled_Gray_select_Resist_Line_FL.Checked = Recipe.Param.InspParam_Line_FL.Enabled_Gray_select_Resist;
            combx_str_Gray_select_Resist_Line_FL.Text = Recipe.Param.InspParam_Line_FL.str_Gray_select_Resist;
            nud_Gray_select_Resist_Line_FL.Value = Recipe.Param.InspParam_Line_FL.Gray_select_Resist;
            cbx_Enabled_Cell2_Step4_Line_FL.Checked = Recipe.Param.InspParam_Line_FL.Enabled_Cell2;
            nud_W_Cell2_Line_FL.Value = Recipe.Param.InspParam_Line_FL.W_Cell2;
            nud_H_Cell2_Line_FL.Value = Recipe.Param.InspParam_Line_FL.H_Cell2;
            nud_count_op1_Line_FL.Value = Recipe.Param.InspParam_Line_FL.count_op1;
            cbx_Enabled_Height_Line_FL.Checked = Recipe.Param.InspParam_Line_FL.Enabled_Height;
            cbx_Enabled_Width_Line_FL.Checked = Recipe.Param.InspParam_Line_FL.Enabled_Width;
            nud_MinH_df_Line_FL.Value = Recipe.Param.InspParam_Line_FL.MinHeight_defect;
            nud_MaxH_df_Line_FL.Value = Recipe.Param.InspParam_Line_FL.MaxHeight_defect;
            nud_MinW_df_Line_FL.Value = Recipe.Param.InspParam_Line_FL.MinWidth_defect;
            nud_MaxW_df_Line_FL.Value = Recipe.Param.InspParam_Line_FL.MaxWidth_defect;
            combx_str_op1_Line_FL.Text = Recipe.Param.InspParam_Line_FL.str_op1;
            cbx_Enabled_Height_op1_2_Line_FL.Checked = Recipe.Param.InspParam_Line_FL.Enabled_Height_op1_2;
            cbx_Enabled_Width_op1_2_Line_FL.Checked = Recipe.Param.InspParam_Line_FL.Enabled_Width_op1_2;
            nud_MinH_df_op1_2_Line_FL.Value = Recipe.Param.InspParam_Line_FL.MinHeight_defect_op1_2;
            nud_MaxH_df_op1_2_Line_FL.Value = Recipe.Param.InspParam_Line_FL.MaxHeight_defect_op1_2;
            nud_MinW_df_op1_2_Line_FL.Value = Recipe.Param.InspParam_Line_FL.MinWidth_defect_op1_2;
            nud_MaxW_df_op1_2_Line_FL.Value = Recipe.Param.InspParam_Line_FL.MaxWidth_defect_op1_2;
            combx_str_op1_2_Line_FL.Text = Recipe.Param.InspParam_Line_FL.str_op1_2;
            cbx_Enabled_Area_Line_FL.Checked = Recipe.Param.InspParam_Line_FL.Enabled_Area;
            nud_MinA_df_Line_FL.Value = Recipe.Param.InspParam_Line_FL.MinArea_defect;
            nud_MaxA_df_Line_FL.Value = Recipe.Param.InspParam_Line_FL.MaxArea_defect;
            combx_str_op2_Line_FL.Text = Recipe.Param.InspParam_Line_FL.str_op2;

            // 【AI只覆判模稜兩可NG】
            cbx_notSureNG_Line_FL.Checked = Recipe.Param.InspParam_Line_FL.cls_absolNG.Enabled;
            cbx_Enabled_Gray_select_Line_FL_AbsolNG.Checked = Recipe.Param.InspParam_Line_FL.cls_absolNG.Enabled_Gray_select;
            combx_str_Gray_select_Line_FL_AbsolNG.Text = Recipe.Param.InspParam_Line_FL.cls_absolNG.str_Gray_select;
            nud_Gray_select_Line_FL_AbsolNG.Value = Recipe.Param.InspParam_Line_FL.cls_absolNG.Gray_select;
            combx_str_op_Line_FL_AbsolNG.Text = Recipe.Param.InspParam_Line_FL.cls_absolNG.str_op;
            cbx_Enabled_Area_Line_FL_AbsolNG.Checked = Recipe.Param.InspParam_Line_FL.cls_absolNG.Enabled_Area;
            nud_MinA_df_Line_FL_AbsolNG.Value = Recipe.Param.InspParam_Line_FL.cls_absolNG.MinArea_defect;
            nud_MaxA_df_Line_FL_AbsolNG.Value = Recipe.Param.InspParam_Line_FL.cls_absolNG.MaxArea_defect;

            #endregion

            #region 檢測絲狀異物 (正光&同軸光) 之參數

            cbx_Line_FLCL.Checked = Recipe.Param.InspParam_Line_FLCL.Enabled;
            combx_str_op1_2Images_Line_FLCL.Text = Recipe.Param.InspParam_Line_FLCL.str_op1_2Images;
            nud_count_op1_Line_FLCL.Value = Recipe.Param.InspParam_Line_FLCL.count_op1;
            cbx_Enabled_Height_Line_FLCL.Checked = Recipe.Param.InspParam_Line_FLCL.Enabled_Height;
            cbx_Enabled_Width_Line_FLCL.Checked = Recipe.Param.InspParam_Line_FLCL.Enabled_Width;
            nud_MinH_df_Line_FLCL.Value = Recipe.Param.InspParam_Line_FLCL.MinHeight_defect;
            nud_MaxH_df_Line_FLCL.Value = Recipe.Param.InspParam_Line_FLCL.MaxHeight_defect;
            nud_MinW_df_Line_FLCL.Value = Recipe.Param.InspParam_Line_FLCL.MinWidth_defect;
            nud_MaxW_df_Line_FLCL.Value = Recipe.Param.InspParam_Line_FLCL.MaxWidth_defect;
            combx_str_op1_Line_FLCL.Text = Recipe.Param.InspParam_Line_FLCL.str_op1;
            cbx_Enabled_Height_op1_2_Line_FLCL.Checked = Recipe.Param.InspParam_Line_FLCL.Enabled_Height_op1_2;
            cbx_Enabled_Width_op1_2_Line_FLCL.Checked = Recipe.Param.InspParam_Line_FLCL.Enabled_Width_op1_2;
            nud_MinH_df_op1_2_Line_FLCL.Value = Recipe.Param.InspParam_Line_FLCL.MinHeight_defect_op1_2;
            nud_MaxH_df_op1_2_Line_FLCL.Value = Recipe.Param.InspParam_Line_FLCL.MaxHeight_defect_op1_2;
            nud_MinW_df_op1_2_Line_FLCL.Value = Recipe.Param.InspParam_Line_FLCL.MinWidth_defect_op1_2;
            nud_MaxW_df_op1_2_Line_FLCL.Value = Recipe.Param.InspParam_Line_FLCL.MaxWidth_defect_op1_2;
            combx_str_op1_2_Line_FLCL.Text = Recipe.Param.InspParam_Line_FLCL.str_op1_2;
            cbx_Enabled_Area_Line_FLCL.Checked = Recipe.Param.InspParam_Line_FLCL.Enabled_Area;
            nud_MinA_df_Line_FLCL.Value = Recipe.Param.InspParam_Line_FLCL.MinArea_defect;
            nud_MaxA_df_Line_FLCL.Value = Recipe.Param.InspParam_Line_FLCL.MaxArea_defect;
            combx_str_op2_Line_FLCL.Text = Recipe.Param.InspParam_Line_FLCL.str_op2;

            cbx_Enabled_BypassReg_Line_FLCL.Checked = Recipe.Param.InspParam_Line_FLCL.Enabled_BypassReg;
            nud_x_BypassReg_Line_FLCL.Value = Recipe.Param.InspParam_Line_FLCL.x_BypassReg;
            nud_y_BypassReg_Line_FLCL.Value = Recipe.Param.InspParam_Line_FLCL.y_BypassReg;
            nud_W_BypassReg_Line_FLCL.Value = Recipe.Param.InspParam_Line_FLCL.W_BypassReg;
            nud_H_BypassReg_Line_FLCL.Value = Recipe.Param.InspParam_Line_FLCL.H_BypassReg;

            nud_count_op3_Line_FLCL.Value = Recipe.Param.InspParam_Line_FLCL.count_op3;
            cbx_Enabled_Height_op3_1_Line_FLCL.Checked = Recipe.Param.InspParam_Line_FLCL.Enabled_Height_op3_1;
            cbx_Enabled_Width_op3_1_Line_FLCL.Checked = Recipe.Param.InspParam_Line_FLCL.Enabled_Width_op3_1;
            nud_MinH_df_op3_1_Line_FLCL.Value = Recipe.Param.InspParam_Line_FLCL.MinHeight_defect_op3_1;
            nud_MaxH_df_op3_1_Line_FLCL.Value = Recipe.Param.InspParam_Line_FLCL.MaxHeight_defect_op3_1;
            nud_MinW_df_op3_1_Line_FLCL.Value = Recipe.Param.InspParam_Line_FLCL.MinWidth_defect_op3_1;
            nud_MaxW_df_op3_1_Line_FLCL.Value = Recipe.Param.InspParam_Line_FLCL.MaxWidth_defect_op3_1;
            combx_str_op3_1_Line_FLCL.Text = Recipe.Param.InspParam_Line_FLCL.str_op3_1;
            cbx_Enabled_Height_op3_2_Line_FLCL.Checked = Recipe.Param.InspParam_Line_FLCL.Enabled_Height_op3_2;
            cbx_Enabled_Width_op3_2_Line_FLCL.Checked = Recipe.Param.InspParam_Line_FLCL.Enabled_Width_op3_2;
            nud_MinH_df_op3_2_Line_FLCL.Value = Recipe.Param.InspParam_Line_FLCL.MinHeight_defect_op3_2;
            nud_MaxH_df_op3_2_Line_FLCL.Value = Recipe.Param.InspParam_Line_FLCL.MaxHeight_defect_op3_2;
            nud_MinW_df_op3_2_Line_FLCL.Value = Recipe.Param.InspParam_Line_FLCL.MinWidth_defect_op3_2;
            nud_MaxW_df_op3_2_Line_FLCL.Value = Recipe.Param.InspParam_Line_FLCL.MaxWidth_defect_op3_2;
            combx_str_op3_2_Line_FLCL.Text = Recipe.Param.InspParam_Line_FLCL.str_op3_2;

            // 【AI只覆判模稜兩可NG】
            cbx_notSureNG_Line_FLCL.Checked = Recipe.Param.InspParam_Line_FLCL.cls_absolNG.Enabled;
            cbx_Enabled_Gray_select_Line_FLCL_AbsolNG.Checked = Recipe.Param.InspParam_Line_FLCL.cls_absolNG.Enabled_Gray_select;
            combx_str_Gray_select_Line_FLCL_AbsolNG.Text = Recipe.Param.InspParam_Line_FLCL.cls_absolNG.str_Gray_select;
            nud_Gray_select_Line_FLCL_AbsolNG.Value = Recipe.Param.InspParam_Line_FLCL.cls_absolNG.Gray_select;
            combx_str_op_Line_FLCL_AbsolNG.Text = Recipe.Param.InspParam_Line_FLCL.cls_absolNG.str_op;
            cbx_Enabled_Area_Line_FLCL_AbsolNG.Checked = Recipe.Param.InspParam_Line_FLCL.cls_absolNG.Enabled_Area;
            nud_MinA_df_Line_FLCL_AbsolNG.Value = Recipe.Param.InspParam_Line_FLCL.cls_absolNG.MinArea_defect;
            nud_MaxA_df_Line_FLCL_AbsolNG.Value = Recipe.Param.InspParam_Line_FLCL.cls_absolNG.MaxArea_defect;

            #endregion

            #region 檢測白條內黑點 (Black Crack) (Frontal light) 之參數

            cbx_BlackCrack_FL.Checked = Recipe.Param.InspParam_BlackCrack_FL.Enabled;
            nud_ImageID_BlackCrack_FL.Value = Recipe.Param.InspParam_BlackCrack_FL.ImageIndex;
            cbx_Enabled_Wsmall_BlackCrack_FL.Checked = Recipe.Param.InspParam_BlackCrack_FL.Enabled_Wsmall;
            combx_str_L_NonInspect_BlackCrack_FL.Text = Recipe.Param.InspParam_BlackCrack_FL.str_L_NonInspect;
            cbx_Enabled_BlackStripe_Step1_BlackCrack_FL.Checked = Recipe.Param.InspParam_BlackCrack_FL.Enabled_BlackStripe;
            combx_str_ModeBlackStripe_BlackCrack_FL.Text = Recipe.Param.InspParam_BlackCrack_FL.str_ModeBlackStripe;
            nud_H_BlackStripe_BlackCrack_FL.Value = Recipe.Param.InspParam_BlackCrack_FL.H_BlackStripe;
            nud_H_BlackStripe_mode2_BlackCrack_FL.Value = Recipe.Param.InspParam_BlackCrack_FL.H_BlackStripe_mode2;
            nud_MinGray_Resist_BlackCrack_FL.Value = Recipe.Param.InspParam_BlackCrack_FL.MinGray_BlackCrack;
            nud_MaxGray_Resist_BlackCrack_FL.Value = Recipe.Param.InspParam_BlackCrack_FL.MaxGray_BlackCrack;
            cbx_Enabled_Cell_Step3_BlackCrack_FL.Checked = Recipe.Param.InspParam_BlackCrack_FL.Enabled_Cell;
            nud_W_Cell_BlackCrack_FL.Value = Recipe.Param.InspParam_BlackCrack_FL.W_Cell;
            nud_H_Cell_BlackCrack_FL.Value = Recipe.Param.InspParam_BlackCrack_FL.H_Cell;
            combx_str_segMode_BlackCrack_FL.Text = Recipe.Param.InspParam_BlackCrack_FL.str_segMode;// (20190529) Jeff Revised!
            cbx_Enabled_Height_BlackCrack_FL.Checked = Recipe.Param.InspParam_BlackCrack_FL.Enabled_Height;
            cbx_Enabled_Width_BlackCrack_FL.Checked = Recipe.Param.InspParam_BlackCrack_FL.Enabled_Width;
            nud_MinH_df_BlackCrack_FL.Value = Recipe.Param.InspParam_BlackCrack_FL.MinHeight_defect;
            nud_MaxH_df_BlackCrack_FL.Value = Recipe.Param.InspParam_BlackCrack_FL.MaxHeight_defect;
            nud_MinW_df_BlackCrack_FL.Value = Recipe.Param.InspParam_BlackCrack_FL.MinWidth_defect;
            nud_MaxW_df_BlackCrack_FL.Value = Recipe.Param.InspParam_BlackCrack_FL.MaxWidth_defect;
            combx_str_op1_BlackCrack_FL.Text = Recipe.Param.InspParam_BlackCrack_FL.str_op1;
            cbx_Enabled_Area_BlackCrack_FL.Checked = Recipe.Param.InspParam_BlackCrack_FL.Enabled_Area;
            nud_MinA_df_BlackCrack_FL.Value = Recipe.Param.InspParam_BlackCrack_FL.MinArea_defect;
            nud_MaxA_df_BlackCrack_FL.Value = Recipe.Param.InspParam_BlackCrack_FL.MaxArea_defect;
            combx_str_op2_BlackCrack_FL.Text = Recipe.Param.InspParam_BlackCrack_FL.str_op2;

            // 【AI只覆判模稜兩可NG】
            cbx_notSureNG_BlackCrack_FL.Checked = Recipe.Param.InspParam_BlackCrack_FL.cls_absolNG.Enabled;
            cbx_Enabled_Gray_select_BlackCrack_FL_AbsolNG.Checked = Recipe.Param.InspParam_BlackCrack_FL.cls_absolNG.Enabled_Gray_select;
            combx_str_Gray_select_BlackCrack_FL_AbsolNG.Text = Recipe.Param.InspParam_BlackCrack_FL.cls_absolNG.str_Gray_select;
            nud_Gray_select_BlackCrack_FL_AbsolNG.Value = Recipe.Param.InspParam_BlackCrack_FL.cls_absolNG.Gray_select;
            combx_str_op_BlackCrack_FL_AbsolNG.Text = Recipe.Param.InspParam_BlackCrack_FL.cls_absolNG.str_op;
            cbx_Enabled_Area_BlackCrack_FL_AbsolNG.Checked = Recipe.Param.InspParam_BlackCrack_FL.cls_absolNG.Enabled_Area;
            nud_MinA_df_BlackCrack_FL_AbsolNG.Value = Recipe.Param.InspParam_BlackCrack_FL.cls_absolNG.MinArea_defect;
            nud_MaxA_df_BlackCrack_FL_AbsolNG.Value = Recipe.Param.InspParam_BlackCrack_FL.cls_absolNG.MaxArea_defect;

            #endregion

            #region 檢測白條內黑點 (Black Crack) (Coaxial light) 之參數

            cbx_BlackCrack_CL.Checked = Recipe.Param.InspParam_BlackCrack_CL.Enabled;
            nud_ImageID_BlackCrack_CL.Value = Recipe.Param.InspParam_BlackCrack_CL.ImageIndex;
            cbx_Enabled_Wsmall_BlackCrack_CL.Checked = Recipe.Param.InspParam_BlackCrack_CL.Enabled_Wsmall;
            combx_str_L_NonInspect_BlackCrack_CL.Text = Recipe.Param.InspParam_BlackCrack_CL.str_L_NonInspect;
            cbx_Enabled_BlackStripe_Step1_BlackCrack_CL.Checked = Recipe.Param.InspParam_BlackCrack_CL.Enabled_BlackStripe;
            combx_str_ModeBlackStripe_BlackCrack_CL.Text = Recipe.Param.InspParam_BlackCrack_CL.str_ModeBlackStripe;
            nud_H_BlackStripe_BlackCrack_CL.Value = Recipe.Param.InspParam_BlackCrack_CL.H_BlackStripe;
            nud_H_BlackStripe_mode2_BlackCrack_CL.Value = Recipe.Param.InspParam_BlackCrack_CL.H_BlackStripe_mode2;
            nud_MinGray_Resist_BlackCrack_CL.Value = Recipe.Param.InspParam_BlackCrack_CL.MinGray_BlackCrack;
            nud_MaxGray_Resist_BlackCrack_CL.Value = Recipe.Param.InspParam_BlackCrack_CL.MaxGray_BlackCrack;
            cbx_Enabled_Cell_Step3_BlackCrack_CL.Checked = Recipe.Param.InspParam_BlackCrack_CL.Enabled_Cell;
            nud_W_Cell_BlackCrack_CL.Value = Recipe.Param.InspParam_BlackCrack_CL.W_Cell;
            nud_H_Cell_BlackCrack_CL.Value = Recipe.Param.InspParam_BlackCrack_CL.H_Cell;
            combx_str_segMode_BlackCrack_CL.Text = Recipe.Param.InspParam_BlackCrack_CL.str_segMode;// (20190529) Jeff Revised!
            cbx_Enabled_Height_BlackCrack_CL.Checked = Recipe.Param.InspParam_BlackCrack_CL.Enabled_Height;
            cbx_Enabled_Width_BlackCrack_CL.Checked = Recipe.Param.InspParam_BlackCrack_CL.Enabled_Width;
            nud_MinH_df_BlackCrack_CL.Value = Recipe.Param.InspParam_BlackCrack_CL.MinHeight_defect;
            nud_MaxH_df_BlackCrack_CL.Value = Recipe.Param.InspParam_BlackCrack_CL.MaxHeight_defect;
            nud_MinW_df_BlackCrack_CL.Value = Recipe.Param.InspParam_BlackCrack_CL.MinWidth_defect;
            nud_MaxW_df_BlackCrack_CL.Value = Recipe.Param.InspParam_BlackCrack_CL.MaxWidth_defect;
            combx_str_op1_BlackCrack_CL.Text = Recipe.Param.InspParam_BlackCrack_CL.str_op1;
            cbx_Enabled_Area_BlackCrack_CL.Checked = Recipe.Param.InspParam_BlackCrack_CL.Enabled_Area;
            nud_MinA_df_BlackCrack_CL.Value = Recipe.Param.InspParam_BlackCrack_CL.MinArea_defect;
            nud_MaxA_df_BlackCrack_CL.Value = Recipe.Param.InspParam_BlackCrack_CL.MaxArea_defect;
            combx_str_op2_BlackCrack_CL.Text = Recipe.Param.InspParam_BlackCrack_CL.str_op2;

            // 【AI只覆判模稜兩可NG】
            cbx_notSureNG_BlackCrack_CL.Checked = Recipe.Param.InspParam_BlackCrack_CL.cls_absolNG.Enabled;
            cbx_Enabled_Gray_select_BlackCrack_CL_AbsolNG.Checked = Recipe.Param.InspParam_BlackCrack_CL.cls_absolNG.Enabled_Gray_select;
            combx_str_Gray_select_BlackCrack_CL_AbsolNG.Text = Recipe.Param.InspParam_BlackCrack_CL.cls_absolNG.str_Gray_select;
            nud_Gray_select_BlackCrack_CL_AbsolNG.Value = Recipe.Param.InspParam_BlackCrack_CL.cls_absolNG.Gray_select;
            combx_str_op_BlackCrack_CL_AbsolNG.Text = Recipe.Param.InspParam_BlackCrack_CL.cls_absolNG.str_op;
            cbx_Enabled_Area_BlackCrack_CL_AbsolNG.Checked = Recipe.Param.InspParam_BlackCrack_CL.cls_absolNG.Enabled_Area;
            nud_MinA_df_BlackCrack_CL_AbsolNG.Value = Recipe.Param.InspParam_BlackCrack_CL.cls_absolNG.MinArea_defect;
            nud_MaxA_df_BlackCrack_CL_AbsolNG.Value = Recipe.Param.InspParam_BlackCrack_CL.cls_absolNG.MaxArea_defect;

            #endregion

            #region 檢測 Black Blob (Coaxial Light) 之參數

            cbx_BlackBlob_CL.Checked = Recipe.Param.InspParam_BlackBlob_CL.Enabled;
            nud_ImageID_BlackBlob_CL.Value = Recipe.Param.InspParam_BlackBlob_CL.ImageIndex;
            txb_MinH_df_BlackBlob_CL.Text = Recipe.Param.InspParam_BlackBlob_CL.MinHeight_defect.ToString();
            txb_MaxH_df_BlackBlob_CL.Text = Recipe.Param.InspParam_BlackBlob_CL.MaxHeight_defect.ToString();
            txb_MinW_df_BlackBlob_CL.Text = Recipe.Param.InspParam_BlackBlob_CL.MinWidth_defect.ToString();
            txb_MaxW_df_BlackBlob_CL.Text = Recipe.Param.InspParam_BlackBlob_CL.MaxWidth_defect.ToString();

            // 【AI只覆判模稜兩可NG】
            cbx_notSureNG_BlackBlob_CL.Checked = Recipe.Param.InspParam_BlackBlob_CL.cls_absolNG.Enabled;
            cbx_Enabled_Gray_select_BlackBlob_CL_AbsolNG.Checked = Recipe.Param.InspParam_BlackBlob_CL.cls_absolNG.Enabled_Gray_select;
            combx_str_Gray_select_BlackBlob_CL_AbsolNG.Text = Recipe.Param.InspParam_BlackBlob_CL.cls_absolNG.str_Gray_select;
            nud_Gray_select_BlackBlob_CL_AbsolNG.Value = Recipe.Param.InspParam_BlackBlob_CL.cls_absolNG.Gray_select;
            combx_str_op_BlackBlob_CL_AbsolNG.Text = Recipe.Param.InspParam_BlackBlob_CL.cls_absolNG.str_op;
            cbx_Enabled_Area_BlackBlob_CL_AbsolNG.Checked = Recipe.Param.InspParam_BlackBlob_CL.cls_absolNG.Enabled_Area;
            nud_MinA_df_BlackBlob_CL_AbsolNG.Value = Recipe.Param.InspParam_BlackBlob_CL.cls_absolNG.MinArea_defect;
            nud_MaxA_df_BlackBlob_CL_AbsolNG.Value = Recipe.Param.InspParam_BlackBlob_CL.cls_absolNG.MaxArea_defect;

            #endregion

            #region 檢測 Black Blob2 (Coaxial Light) 之參數

            cbx_BlackBlob2_CL.Checked = Recipe.Param.InspParam_BlackBlob2_CL.Enabled;
            nud_ImageID_BlackBlob2_CL.Value = Recipe.Param.InspParam_BlackBlob2_CL.ImageIndex;
            cbx_Enabled_Wsmall_BlackBlob2_CL.Checked = Recipe.Param.InspParam_BlackBlob2_CL.Enabled_Wsmall;
            combx_str_L_NonInspect_BlackBlob2_CL.Text = Recipe.Param.InspParam_BlackBlob2_CL.str_L_NonInspect;
            cbx_Enabled_Cell_Step1_BlackBlob2_CL.Checked = Recipe.Param.InspParam_BlackBlob2_CL.Enabled_Cell;
            cbx_Enabled_Cell_Step2_BlackBlob2_CL.Checked = Recipe.Param.InspParam_BlackBlob2_CL.Enabled_Cell;
            nud_W_Cell_BlackBlob2_CL.Value = Recipe.Param.InspParam_BlackBlob2_CL.W_Cell;
            nud_H_Cell_BlackBlob2_CL.Value = Recipe.Param.InspParam_BlackBlob2_CL.H_Cell;
            cbx_Enabled_BypassReg_BlackBlob2_CL.Checked = Recipe.Param.InspParam_BlackBlob2_CL.Enabled_BypassReg;
            nud_x_BypassReg_BlackBlob2_CL.Value = Recipe.Param.InspParam_BlackBlob2_CL.x_BypassReg;
            nud_y_BypassReg_BlackBlob2_CL.Value = Recipe.Param.InspParam_BlackBlob2_CL.y_BypassReg;
            nud_W_BypassReg_BlackBlob2_CL.Value = Recipe.Param.InspParam_BlackBlob2_CL.W_BypassReg;
            nud_H_BypassReg_BlackBlob2_CL.Value = Recipe.Param.InspParam_BlackBlob2_CL.H_BypassReg;
            cbx_Enabled_BlackStripe_Step1_BlackBlob2_CL.Checked = Recipe.Param.InspParam_BlackBlob2_CL.Enabled_BlackStripe;
            cbx_Enabled_BlackStripe_Step2_BlackBlob2_CL.Checked = Recipe.Param.InspParam_BlackBlob2_CL.Enabled_BlackStripe;
            combx_str_ModeBlackStripe_BlackBlob2_CL.Text = Recipe.Param.InspParam_BlackBlob2_CL.str_ModeBlackStripe;
            nud_H_BlackStripe_BlackBlob2_CL.Value = Recipe.Param.InspParam_BlackBlob2_CL.H_BlackStripe;
            nud_H_BlackStripe_mode2_BlackBlob2_CL.Value = Recipe.Param.InspParam_BlackBlob2_CL.H_BlackStripe_mode2;
            combx_str_BinaryImg_Cell_BlackBlob2_CL.Text = Recipe.Param.InspParam_BlackBlob2_CL.str_BinaryImg_Cell;
            nud_MaskWidth_1_Cell_BlackBlob2_CL.Value = Recipe.Param.InspParam_BlackBlob2_CL.MaskWidth_Cell;
            nud_MaskHeight_1_Cell_BlackBlob2_CL.Value = Recipe.Param.InspParam_BlackBlob2_CL.MaskHeight_Cell;
            combx_str_LightDark_1_Cell_BlackBlob2_CL.Text = Recipe.Param.InspParam_BlackBlob2_CL.str_LightDark_Cell;
            nud_Offset_1_Cell_BlackBlob2_CL.Value = Recipe.Param.InspParam_BlackBlob2_CL.Offset_Cell;
            nud_MinGray_1_Cell_BlackBlob2_CL.Value = Recipe.Param.InspParam_BlackBlob2_CL.MinGray_Cell;
            nud_MaxGray_1_Cell_BlackBlob2_CL.Value = Recipe.Param.InspParam_BlackBlob2_CL.MaxGray_Cell;
            combx_str_BinaryImg_Resist_BlackBlob2_CL.Text = Recipe.Param.InspParam_BlackBlob2_CL.str_BinaryImg_Resist;
            nud_MaskWidth_1_Resist_BlackBlob2_CL.Value = Recipe.Param.InspParam_BlackBlob2_CL.MaskWidth_Resist;
            nud_MaskHeight_1_Resist_BlackBlob2_CL.Value = Recipe.Param.InspParam_BlackBlob2_CL.MaskHeight_Resist;
            combx_str_LightDark_1_Resist_BlackBlob2_CL.Text = Recipe.Param.InspParam_BlackBlob2_CL.str_LightDark_Resist;
            nud_Offset_1_Resist_BlackBlob2_CL.Value = Recipe.Param.InspParam_BlackBlob2_CL.Offset_Resist;
            nud_MinGray_1_Resist_BlackBlob2_CL.Value = Recipe.Param.InspParam_BlackBlob2_CL.MinGray_Resist;
            nud_MaxGray_1_Resist_BlackBlob2_CL.Value = Recipe.Param.InspParam_BlackBlob2_CL.MaxGray_Resist;

            // 規格2 (20190411) Jeff Revised!
            nud_count_BinImg_Cell_BlackBlob2_CL.Value = Recipe.Param.InspParam_BlackBlob2_CL.count_BinImg_Cell;
            nud_MinGray_2_Cell_BlackBlob2_CL.Value = Recipe.Param.InspParam_BlackBlob2_CL.MinGray_2_Cell;
            nud_MaxGray_2_Cell_BlackBlob2_CL.Value = Recipe.Param.InspParam_BlackBlob2_CL.MaxGray_2_Cell;
            nud_MaskWidth_2_Cell_BlackBlob2_CL.Value = Recipe.Param.InspParam_BlackBlob2_CL.MaskWidth_2_Cell;
            nud_MaskHeight_2_Cell_BlackBlob2_CL.Value = Recipe.Param.InspParam_BlackBlob2_CL.MaskHeight_2_Cell;
            combx_str_LightDark_2_Cell_BlackBlob2_CL.Text = Recipe.Param.InspParam_BlackBlob2_CL.str_LightDark_2_Cell;
            nud_Offset_2_Cell_BlackBlob2_CL.Value = Recipe.Param.InspParam_BlackBlob2_CL.Offset_2_Cell;
            nud_count_BinImg_Resist_BlackBlob2_CL.Value = Recipe.Param.InspParam_BlackBlob2_CL.count_BinImg_Resist;
            nud_MinGray_2_Resist_BlackBlob2_CL.Value = Recipe.Param.InspParam_BlackBlob2_CL.MinGray_2_Resist;
            nud_MaxGray_2_Resist_BlackBlob2_CL.Value = Recipe.Param.InspParam_BlackBlob2_CL.MaxGray_2_Resist;
            nud_MaskWidth_2_Resist_BlackBlob2_CL.Value = Recipe.Param.InspParam_BlackBlob2_CL.MaskWidth_2_Resist;
            nud_MaskHeight_2_Resist_BlackBlob2_CL.Value = Recipe.Param.InspParam_BlackBlob2_CL.MaskHeight_2_Resist;
            combx_str_LightDark_2_Resist_BlackBlob2_CL.Text = Recipe.Param.InspParam_BlackBlob2_CL.str_LightDark_2_Resist;
            nud_Offset_2_Resist_BlackBlob2_CL.Value = Recipe.Param.InspParam_BlackBlob2_CL.Offset_2_Resist;

            cbx_Enabled_Gray_select_Cell_BlackBlob2_CL.Checked = Recipe.Param.InspParam_BlackBlob2_CL.Enabled_Gray_select_Cell;
            combx_str_Gray_select_Cell_BlackBlob2_CL.Text = Recipe.Param.InspParam_BlackBlob2_CL.str_Gray_select_Cell;
            nud_Gray_select_Cell_BlackBlob2_CL.Value = Recipe.Param.InspParam_BlackBlob2_CL.Gray_select_Cell;
            cbx_Enabled_Gray_select_Resist_BlackBlob2_CL.Checked = Recipe.Param.InspParam_BlackBlob2_CL.Enabled_Gray_select_Resist;
            combx_str_Gray_select_Resist_BlackBlob2_CL.Text = Recipe.Param.InspParam_BlackBlob2_CL.str_Gray_select_Resist;
            nud_Gray_select_Resist_BlackBlob2_CL.Value = Recipe.Param.InspParam_BlackBlob2_CL.Gray_select_Resist;
            cbx_Enabled_Cell2_Step4_BlackBlob2_CL.Checked = Recipe.Param.InspParam_BlackBlob2_CL.Enabled_Cell2;
            nud_W_Cell2_BlackBlob2_CL.Value = Recipe.Param.InspParam_BlackBlob2_CL.W_Cell2;
            nud_H_Cell2_BlackBlob2_CL.Value = Recipe.Param.InspParam_BlackBlob2_CL.H_Cell2;
            nud_count_op1_BlackBlob2_CL.Value = Recipe.Param.InspParam_BlackBlob2_CL.count_op1;
            cbx_Enabled_Height_BlackBlob2_CL.Checked = Recipe.Param.InspParam_BlackBlob2_CL.Enabled_Height;
            cbx_Enabled_Width_BlackBlob2_CL.Checked = Recipe.Param.InspParam_BlackBlob2_CL.Enabled_Width;
            nud_MinH_df_BlackBlob2_CL.Value = Recipe.Param.InspParam_BlackBlob2_CL.MinHeight_defect;
            nud_MaxH_df_BlackBlob2_CL.Value = Recipe.Param.InspParam_BlackBlob2_CL.MaxHeight_defect;
            nud_MinW_df_BlackBlob2_CL.Value = Recipe.Param.InspParam_BlackBlob2_CL.MinWidth_defect;
            nud_MaxW_df_BlackBlob2_CL.Value = Recipe.Param.InspParam_BlackBlob2_CL.MaxWidth_defect;
            combx_str_op1_BlackBlob2_CL.Text = Recipe.Param.InspParam_BlackBlob2_CL.str_op1;
            cbx_Enabled_Height_op1_2_BlackBlob2_CL.Checked = Recipe.Param.InspParam_BlackBlob2_CL.Enabled_Height_op1_2;
            cbx_Enabled_Width_op1_2_BlackBlob2_CL.Checked = Recipe.Param.InspParam_BlackBlob2_CL.Enabled_Width_op1_2;
            nud_MinH_df_op1_2_BlackBlob2_CL.Value = Recipe.Param.InspParam_BlackBlob2_CL.MinHeight_defect_op1_2;
            nud_MaxH_df_op1_2_BlackBlob2_CL.Value = Recipe.Param.InspParam_BlackBlob2_CL.MaxHeight_defect_op1_2;
            nud_MinW_df_op1_2_BlackBlob2_CL.Value = Recipe.Param.InspParam_BlackBlob2_CL.MinWidth_defect_op1_2;
            nud_MaxW_df_op1_2_BlackBlob2_CL.Value = Recipe.Param.InspParam_BlackBlob2_CL.MaxWidth_defect_op1_2;
            combx_str_op1_2_BlackBlob2_CL.Text = Recipe.Param.InspParam_BlackBlob2_CL.str_op1_2;
            cbx_Enabled_Area_BlackBlob2_CL.Checked = Recipe.Param.InspParam_BlackBlob2_CL.Enabled_Area;
            nud_MinA_df_BlackBlob2_CL.Value = Recipe.Param.InspParam_BlackBlob2_CL.MinArea_defect;
            nud_MaxA_df_BlackBlob2_CL.Value = Recipe.Param.InspParam_BlackBlob2_CL.MaxArea_defect;
            combx_str_op2_BlackBlob2_CL.Text = Recipe.Param.InspParam_BlackBlob2_CL.str_op2;

            // 【AI只覆判模稜兩可NG】
            cbx_notSureNG_BlackBlob2_CL.Checked = Recipe.Param.InspParam_BlackBlob2_CL.cls_absolNG.Enabled;
            cbx_Enabled_Gray_select_BlackBlob2_CL_AbsolNG.Checked = Recipe.Param.InspParam_BlackBlob2_CL.cls_absolNG.Enabled_Gray_select;
            combx_str_Gray_select_BlackBlob2_CL_AbsolNG.Text = Recipe.Param.InspParam_BlackBlob2_CL.cls_absolNG.str_Gray_select;
            nud_Gray_select_BlackBlob2_CL_AbsolNG.Value = Recipe.Param.InspParam_BlackBlob2_CL.cls_absolNG.Gray_select;
            combx_str_op_BlackBlob2_CL_AbsolNG.Text = Recipe.Param.InspParam_BlackBlob2_CL.cls_absolNG.str_op;
            cbx_Enabled_Area_BlackBlob2_CL_AbsolNG.Checked = Recipe.Param.InspParam_BlackBlob2_CL.cls_absolNG.Enabled_Area;
            nud_MinA_df_BlackBlob2_CL_AbsolNG.Value = Recipe.Param.InspParam_BlackBlob2_CL.cls_absolNG.MinArea_defect;
            nud_MaxA_df_BlackBlob2_CL_AbsolNG.Value = Recipe.Param.InspParam_BlackBlob2_CL.cls_absolNG.MaxArea_defect;

            #endregion

            #region 檢測汙染 (Coaxial Light) 之參數

            cbx_Dirty_CL.Checked = Recipe.Param.InspParam_Dirty_CL.Enabled;
            nud_ImageID_Dirty_CL.Value = Recipe.Param.InspParam_Dirty_CL.ImageIndex;
            cbx_Enabled_Cell_Step1_Dirty_CL.Checked = Recipe.Param.InspParam_Dirty_CL.Enabled_Cell;
            cbx_Enabled_Cell_Step2_Dirty_CL.Checked = Recipe.Param.InspParam_Dirty_CL.Enabled_Cell;
            nud_W_Cell_Dirty_CL.Value = Recipe.Param.InspParam_Dirty_CL.W_Cell;
            nud_H_Cell_Dirty_CL.Value = Recipe.Param.InspParam_Dirty_CL.H_Cell;
            cbx_Enabled_BypassReg_Dirty_CL.Checked = Recipe.Param.InspParam_Dirty_CL.Enabled_BypassReg;
            nud_x_BypassReg_Dirty_CL.Value = Recipe.Param.InspParam_Dirty_CL.x_BypassReg;
            nud_y_BypassReg_Dirty_CL.Value = Recipe.Param.InspParam_Dirty_CL.y_BypassReg;
            nud_W_BypassReg_Dirty_CL.Value = Recipe.Param.InspParam_Dirty_CL.W_BypassReg;
            nud_H_BypassReg_Dirty_CL.Value = Recipe.Param.InspParam_Dirty_CL.H_BypassReg;
            cbx_Enabled_BlackStripe_Step1_Dirty_CL.Checked = Recipe.Param.InspParam_Dirty_CL.Enabled_BlackStripe;
            cbx_Enabled_BlackStripe_Step2_Dirty_CL.Checked = Recipe.Param.InspParam_Dirty_CL.Enabled_BlackStripe;
            combx_str_ModeBlackStripe_Dirty_CL.Text = Recipe.Param.InspParam_Dirty_CL.str_ModeBlackStripe;
            nud_H_BlackStripe_Dirty_CL.Value = Recipe.Param.InspParam_Dirty_CL.H_BlackStripe;
            nud_H_BlackStripe_mode2_Dirty_CL.Value = Recipe.Param.InspParam_Dirty_CL.H_BlackStripe_mode2;
            nud_MinGray_Cell_Dirty_CL.Value = Recipe.Param.InspParam_Dirty_CL.MinGray_Cell;
            nud_MaxGray_Cell_Dirty_CL.Value = Recipe.Param.InspParam_Dirty_CL.MaxGray_Cell;
            nud_MinGray_Resist_Dirty_CL.Value = Recipe.Param.InspParam_Dirty_CL.MinGray_Resist;
            nud_MaxGray_Resist_Dirty_CL.Value = Recipe.Param.InspParam_Dirty_CL.MaxGray_Resist;
            cbx_Enabled_Gray_select_Dirty_CL.Checked = Recipe.Param.InspParam_Dirty_CL.Enabled_Gray_select;
            combx_str_Gray_select_Dirty_CL.Text = Recipe.Param.InspParam_Dirty_CL.str_Gray_select;
            nud_Gray_select_Dirty_CL.Value = Recipe.Param.InspParam_Dirty_CL.Gray_select;
            cbx_Enabled_Height_Dirty_CL.Checked = Recipe.Param.InspParam_Dirty_CL.Enabled_Height;
            cbx_Enabled_Width_Dirty_CL.Checked = Recipe.Param.InspParam_Dirty_CL.Enabled_Width;
            nud_MinH_df_Dirty_CL.Value = Recipe.Param.InspParam_Dirty_CL.MinHeight_defect;
            nud_MaxH_df_Dirty_CL.Value = Recipe.Param.InspParam_Dirty_CL.MaxHeight_defect;
            nud_MinW_df_Dirty_CL.Value = Recipe.Param.InspParam_Dirty_CL.MinWidth_defect;
            nud_MaxW_df_Dirty_CL.Value = Recipe.Param.InspParam_Dirty_CL.MaxWidth_defect;
            combx_str_op1_Dirty_CL.Text = Recipe.Param.InspParam_Dirty_CL.str_op1;
            cbx_Enabled_Area_Dirty_CL.Checked = Recipe.Param.InspParam_Dirty_CL.Enabled_Area;
            nud_MinA_df_Dirty_CL.Value = Recipe.Param.InspParam_Dirty_CL.MinArea_defect;
            nud_MaxA_df_Dirty_CL.Value = Recipe.Param.InspParam_Dirty_CL.MaxArea_defect;
            combx_str_op2_Dirty_CL.Text = Recipe.Param.InspParam_Dirty_CL.str_op2;

            // 【AI只覆判模稜兩可NG】
            cbx_notSureNG_Dirty_CL.Checked = Recipe.Param.InspParam_Dirty_CL.cls_absolNG.Enabled;
            cbx_Enabled_Gray_select_Dirty_CL_AbsolNG.Checked = Recipe.Param.InspParam_Dirty_CL.cls_absolNG.Enabled_Gray_select;
            combx_str_Gray_select_Dirty_CL_AbsolNG.Text = Recipe.Param.InspParam_Dirty_CL.cls_absolNG.str_Gray_select;
            nud_Gray_select_Dirty_CL_AbsolNG.Value = Recipe.Param.InspParam_Dirty_CL.cls_absolNG.Gray_select;
            combx_str_op_Dirty_CL_AbsolNG.Text = Recipe.Param.InspParam_Dirty_CL.cls_absolNG.str_op;
            cbx_Enabled_Area_Dirty_CL_AbsolNG.Checked = Recipe.Param.InspParam_Dirty_CL.cls_absolNG.Enabled_Area;
            nud_MinA_df_Dirty_CL_AbsolNG.Value = Recipe.Param.InspParam_Dirty_CL.cls_absolNG.MinArea_defect;
            nud_MaxA_df_Dirty_CL_AbsolNG.Value = Recipe.Param.InspParam_Dirty_CL.cls_absolNG.MaxArea_defect;

            #endregion

            #region 檢測汙染 (Dirty) (Frontal light & Coaxial Light) 之參數

            cbx_Dirty_FLCL.Checked = Recipe.Param.InspParam_Dirty_FLCL.Enabled;
            nud_ImageID1_Dirty_FLCL.Value = Recipe.Param.InspParam_Dirty_FLCL.ImageIndex1;
            nud_ImageID2_Dirty_FLCL.Value = Recipe.Param.InspParam_Dirty_FLCL.ImageIndex2;
            nud_MaxGray_Dirty_FL.Value = Recipe.Param.InspParam_Dirty_FLCL.MaxGray_Dirty_FL;
            nud_Gray_Dark_FL.Value = Recipe.Param.InspParam_Dirty_FLCL.Gray_Dark_FL;
            nud_MinGray_Dirty_CL.Value = Recipe.Param.InspParam_Dirty_FLCL.MinGray_Dirty_CL;
            nud_Gray_Bright_CL.Value = Recipe.Param.InspParam_Dirty_FLCL.Gray_Bright_CL;
            txb_MinH_df_Dirty_FLCL.Text = Recipe.Param.InspParam_Dirty_FLCL.MinHeight_defect.ToString();
            txb_MaxH_df_Dirty_FLCL.Text = Recipe.Param.InspParam_Dirty_FLCL.MaxHeight_defect.ToString();
            txb_MinW_df_Dirty_FLCL.Text = Recipe.Param.InspParam_Dirty_FLCL.MinWidth_defect.ToString();
            txb_MaxW_df_Dirty_FLCL.Text = Recipe.Param.InspParam_Dirty_FLCL.MaxWidth_defect.ToString();

            // 【AI只覆判模稜兩可NG】
            cbx_notSureNG_Dirty_FLCL.Checked = Recipe.Param.InspParam_Dirty_FLCL.cls_absolNG.Enabled;
            cbx_Enabled_Gray_select_Dirty_FLCL_AbsolNG.Checked = Recipe.Param.InspParam_Dirty_FLCL.cls_absolNG.Enabled_Gray_select;
            combx_str_Gray_select_Dirty_FLCL_AbsolNG.Text = Recipe.Param.InspParam_Dirty_FLCL.cls_absolNG.str_Gray_select;
            nud_Gray_select_Dirty_FLCL_AbsolNG.Value = Recipe.Param.InspParam_Dirty_FLCL.cls_absolNG.Gray_select;
            combx_str_op_Dirty_FLCL_AbsolNG.Text = Recipe.Param.InspParam_Dirty_FLCL.cls_absolNG.str_op;
            cbx_Enabled_Area_Dirty_FLCL_AbsolNG.Checked = Recipe.Param.InspParam_Dirty_FLCL.cls_absolNG.Enabled_Area;
            nud_MinA_df_Dirty_FLCL_AbsolNG.Value = Recipe.Param.InspParam_Dirty_FLCL.cls_absolNG.MinArea_defect;
            nud_MaxA_df_Dirty_FLCL_AbsolNG.Value = Recipe.Param.InspParam_Dirty_FLCL.cls_absolNG.MaxArea_defect;

            #endregion

            #region 檢測電極區白點 (Back Light) 之參數

            cbx_BrightBlob_BL.Checked = Recipe.Param.InspParam_BrightBlob_BL.Enabled;
            nud_ImageID_BrightBlob_BL.Value = Recipe.Param.InspParam_BrightBlob_BL.ImageIndex;
            nud_MinGray_Blob_Elect_BL.Value = Recipe.Param.InspParam_BrightBlob_BL.MinGray_Blob_Elect_BL;
            nud_MinH_df_BrightBlob_BL.Value = Recipe.Param.InspParam_BrightBlob_BL.MinHeight_defect; // (20190215) Jeff Revised!
            nud_MaxH_df_BrightBlob_BL.Value = Recipe.Param.InspParam_BrightBlob_BL.MaxHeight_defect; // (20190215) Jeff Revised!
            nud_MinW_df_BrightBlob_BL.Value = Recipe.Param.InspParam_BrightBlob_BL.MinWidth_defect; // (20190215) Jeff Revised!
            nud_MaxW_df_BrightBlob_BL.Value = Recipe.Param.InspParam_BrightBlob_BL.MaxWidth_defect; // (20190215) Jeff Revised!

            // 【AI只覆判模稜兩可NG】
            cbx_notSureNG_BrightBlob_BL.Checked = Recipe.Param.InspParam_BrightBlob_BL.cls_absolNG.Enabled;
            cbx_Enabled_Gray_select_BrightBlob_BL_AbsolNG.Checked = Recipe.Param.InspParam_BrightBlob_BL.cls_absolNG.Enabled_Gray_select;
            combx_str_Gray_select_BrightBlob_BL_AbsolNG.Text = Recipe.Param.InspParam_BrightBlob_BL.cls_absolNG.str_Gray_select;
            nud_Gray_select_BrightBlob_BL_AbsolNG.Value = Recipe.Param.InspParam_BrightBlob_BL.cls_absolNG.Gray_select;
            combx_str_op_BrightBlob_BL_AbsolNG.Text = Recipe.Param.InspParam_BrightBlob_BL.cls_absolNG.str_op;
            cbx_Enabled_Area_BrightBlob_BL_AbsolNG.Checked = Recipe.Param.InspParam_BrightBlob_BL.cls_absolNG.Enabled_Area;
            nud_MinA_df_BrightBlob_BL_AbsolNG.Value = Recipe.Param.InspParam_BrightBlob_BL.cls_absolNG.MinArea_defect;
            nud_MaxA_df_BrightBlob_BL_AbsolNG.Value = Recipe.Param.InspParam_BrightBlob_BL.cls_absolNG.MaxArea_defect;

            #endregion

            #region 檢測保缺角 (LackAngle) (Back Light) 之參數

            cbx_LackAngle_BL.Checked = Recipe.Param.InspParam_LackAngle_BL.Enabled;
            nud_ImageID_LackAngle_BL.Value = Recipe.Param.InspParam_LackAngle_BL.ImageIndex;
            cbx_Enabled_Cell_Step1_LackAngle_BL.Checked = Recipe.Param.InspParam_LackAngle_BL.Enabled_Cell;
            cbx_Enabled_Cell_Step4_LackAngle_BL.Checked = Recipe.Param.InspParam_LackAngle_BL.Enabled_Cell;
            nud_W_Cell_LackAngle_BL.Value = Recipe.Param.InspParam_LackAngle_BL.W_Cell;
            nud_H_Cell_LackAngle_BL.Value = Recipe.Param.InspParam_LackAngle_BL.H_Cell;
            cbx_Enabled_BypassReg_LackAngle_BL.Checked = Recipe.Param.InspParam_LackAngle_BL.Enabled_BypassReg;
            nud_x_BypassReg_LackAngle_BL.Value = Recipe.Param.InspParam_LackAngle_BL.x_BypassReg;
            nud_y_BypassReg_LackAngle_BL.Value = Recipe.Param.InspParam_LackAngle_BL.y_BypassReg;
            nud_W_BypassReg_LackAngle_BL.Value = Recipe.Param.InspParam_LackAngle_BL.W_BypassReg;
            nud_H_BypassReg_LackAngle_BL.Value = Recipe.Param.InspParam_LackAngle_BL.H_BypassReg;
            cbx_Enabled_BlackStripe_Step1_LackAngle_BL.Checked = Recipe.Param.InspParam_LackAngle_BL.Enabled_BlackStripe;
            cbx_Enabled_BlackStripe_Step4_LackAngle_BL.Checked = Recipe.Param.InspParam_LackAngle_BL.Enabled_BlackStripe;
            nud_H_BlackStripe_LackAngle_BL.Value = Recipe.Param.InspParam_LackAngle_BL.H_BlackStripe;
            cbx_Enabled_EquHisto_Step2_LackAngle_BL.Checked = Recipe.Param.InspParam_LackAngle_BL.Enabled_EquHisto;
            cbx_Enabled_EquHisto_Step3_LackAngle_BL.Checked = Recipe.Param.InspParam_LackAngle_BL.Enabled_EquHisto;
            nud_MinGray_Bright_BL_LackAngle_BL.Value = Recipe.Param.InspParam_LackAngle_BL.MinGray_Bright_BL;
            nud_MaxGray_Bright_BL_LackAngle_BL.Value = Recipe.Param.InspParam_LackAngle_BL.MaxGray_Bright_BL;
            nud_w_Part_LackAngle_BL.Value = Recipe.Param.InspParam_LackAngle_BL.w_Part;
            nud_h_Part_LackAngle_BL.Value = Recipe.Param.InspParam_LackAngle_BL.h_Part;
            nud_MinGray_Cell_LackAngle_BL.Value = Recipe.Param.InspParam_LackAngle_BL.MinGray_Cell;
            nud_MinGray_Resist_LackAngle_BL.Value = Recipe.Param.InspParam_LackAngle_BL.MinGray_Resist;
            nud_Gray_select_LackAngle_BL.Value = Recipe.Param.InspParam_LackAngle_BL.Gray_select;
            cbx_Enabled_Height_LackAngle_BL.Checked = Recipe.Param.InspParam_LackAngle_BL.Enabled_Height;
            cbx_Enabled_Width_LackAngle_BL.Checked = Recipe.Param.InspParam_LackAngle_BL.Enabled_Width;
            nud_MinH_df_LackAngle_BL.Value = Recipe.Param.InspParam_LackAngle_BL.MinHeight_defect;
            nud_MaxH_df_LackAngle_BL.Value = Recipe.Param.InspParam_LackAngle_BL.MaxHeight_defect;
            nud_MinW_df_LackAngle_BL.Value = Recipe.Param.InspParam_LackAngle_BL.MinWidth_defect;
            nud_MaxW_df_LackAngle_BL.Value = Recipe.Param.InspParam_LackAngle_BL.MaxWidth_defect;
            combx_str_op1_LackAngle_BL.Text = Recipe.Param.InspParam_LackAngle_BL.str_op1;
            cbx_Enabled_Area_LackAngle_BL.Checked = Recipe.Param.InspParam_LackAngle_BL.Enabled_Area;
            nud_MinA_df_LackAngle_BL.Value = Recipe.Param.InspParam_LackAngle_BL.MinArea_defect;
            nud_MaxA_df_LackAngle_BL.Value = Recipe.Param.InspParam_LackAngle_BL.MaxArea_defect;
            combx_str_op2_LackAngle_BL.Text = Recipe.Param.InspParam_LackAngle_BL.str_op2;

            // 【AI只覆判模稜兩可NG】
            cbx_notSureNG_LackAngle_BL.Checked = Recipe.Param.InspParam_LackAngle_BL.cls_absolNG.Enabled;
            cbx_Enabled_Gray_select_LackAngle_BL_AbsolNG.Checked = Recipe.Param.InspParam_LackAngle_BL.cls_absolNG.Enabled_Gray_select;
            combx_str_Gray_select_LackAngle_BL_AbsolNG.Text = Recipe.Param.InspParam_LackAngle_BL.cls_absolNG.str_Gray_select;
            nud_Gray_select_LackAngle_BL_AbsolNG.Value = Recipe.Param.InspParam_LackAngle_BL.cls_absolNG.Gray_select;
            combx_str_op_LackAngle_BL_AbsolNG.Text = Recipe.Param.InspParam_LackAngle_BL.cls_absolNG.str_op;
            cbx_Enabled_Area_LackAngle_BL_AbsolNG.Checked = Recipe.Param.InspParam_LackAngle_BL.cls_absolNG.Enabled_Area;
            nud_MinA_df_LackAngle_BL_AbsolNG.Value = Recipe.Param.InspParam_LackAngle_BL.cls_absolNG.MinArea_defect;
            nud_MaxA_df_LackAngle_BL_AbsolNG.Value = Recipe.Param.InspParam_LackAngle_BL.cls_absolNG.MaxArea_defect;

            #endregion

            #region 檢測保缺角 (LackAngle) (Side Light) 之參數

            cbx_LackAngle_SL.Checked = Recipe.Param.InspParam_LackAngle_SL.Enabled;
            nud_ImageID_LackAngle_SL.Value = Recipe.Param.InspParam_LackAngle_SL.ImageIndex;
            cbx_Enabled_Wsmall_LackAngle_SL.Checked = Recipe.Param.InspParam_LackAngle_SL.Enabled_Wsmall;
            combx_str_L_NonInspect_LackAngle_SL.Text = Recipe.Param.InspParam_LackAngle_SL.str_L_NonInspect;
            cbx_Enabled_Cell_Step1_LackAngle_SL.Checked = Recipe.Param.InspParam_LackAngle_SL.Enabled_Cell;
            cbx_Enabled_Cell_Step2_LackAngle_SL.Checked = Recipe.Param.InspParam_LackAngle_SL.Enabled_Cell;
            nud_W_Cell_LackAngle_SL.Value = Recipe.Param.InspParam_LackAngle_SL.W_Cell;
            nud_H_Cell_LackAngle_SL.Value = Recipe.Param.InspParam_LackAngle_SL.H_Cell;
            cbx_Enabled_BypassReg_LackAngle_SL.Checked = Recipe.Param.InspParam_LackAngle_SL.Enabled_BypassReg;
            nud_x_BypassReg_LackAngle_SL.Value = Recipe.Param.InspParam_LackAngle_SL.x_BypassReg;
            nud_y_BypassReg_LackAngle_SL.Value = Recipe.Param.InspParam_LackAngle_SL.y_BypassReg;
            nud_W_BypassReg_LackAngle_SL.Value = Recipe.Param.InspParam_LackAngle_SL.W_BypassReg;
            nud_H_BypassReg_LackAngle_SL.Value = Recipe.Param.InspParam_LackAngle_SL.H_BypassReg;
            cbx_Enabled_BlackStripe_Step1_LackAngle_SL.Checked = Recipe.Param.InspParam_LackAngle_SL.Enabled_BlackStripe;
            cbx_Enabled_BlackStripe_Step2_LackAngle_SL.Checked = Recipe.Param.InspParam_LackAngle_SL.Enabled_BlackStripe;
            combx_str_ModeBlackStripe_LackAngle_SL.Text = Recipe.Param.InspParam_LackAngle_SL.str_ModeBlackStripe;
            nud_H_BlackStripe_LackAngle_SL.Value = Recipe.Param.InspParam_LackAngle_SL.H_BlackStripe;
            nud_H_BlackStripe_mode2_LackAngle_SL.Value = Recipe.Param.InspParam_LackAngle_SL.H_BlackStripe_mode2;
            combx_str_BinaryImg_Cell_LackAngle_SL.Text = Recipe.Param.InspParam_LackAngle_SL.str_BinaryImg_Cell;
            nud_MaskWidth_1_Cell_LackAngle_SL.Value = Recipe.Param.InspParam_LackAngle_SL.MaskWidth_Cell;
            nud_MaskHeight_1_Cell_LackAngle_SL.Value = Recipe.Param.InspParam_LackAngle_SL.MaskHeight_Cell;
            combx_str_LightDark_1_Cell_LackAngle_SL.Text = Recipe.Param.InspParam_LackAngle_SL.str_LightDark_Cell;
            nud_Offset_1_Cell_LackAngle_SL.Value = Recipe.Param.InspParam_LackAngle_SL.Offset_Cell;
            nud_MinGray_1_Cell_LackAngle_SL.Value = Recipe.Param.InspParam_LackAngle_SL.MinGray_Cell;
            nud_MaxGray_1_Cell_LackAngle_SL.Value = Recipe.Param.InspParam_LackAngle_SL.MaxGray_Cell;
            combx_str_BinaryImg_Resist_LackAngle_SL.Text = Recipe.Param.InspParam_LackAngle_SL.str_BinaryImg_Resist;
            nud_MaskWidth_1_Resist_LackAngle_SL.Value = Recipe.Param.InspParam_LackAngle_SL.MaskWidth_Resist;
            nud_MaskHeight_1_Resist_LackAngle_SL.Value = Recipe.Param.InspParam_LackAngle_SL.MaskHeight_Resist;
            combx_str_LightDark_1_Resist_LackAngle_SL.Text = Recipe.Param.InspParam_LackAngle_SL.str_LightDark_Resist;
            nud_Offset_1_Resist_LackAngle_SL.Value = Recipe.Param.InspParam_LackAngle_SL.Offset_Resist;
            nud_MinGray_1_Resist_LackAngle_SL.Value = Recipe.Param.InspParam_LackAngle_SL.MinGray_Resist;
            nud_MaxGray_1_Resist_LackAngle_SL.Value = Recipe.Param.InspParam_LackAngle_SL.MaxGray_Resist;

            // 規格2 (20190411) Jeff Revised!
            nud_count_BinImg_Cell_LackAngle_SL.Value = Recipe.Param.InspParam_LackAngle_SL.count_BinImg_Cell;
            nud_MinGray_2_Cell_LackAngle_SL.Value = Recipe.Param.InspParam_LackAngle_SL.MinGray_2_Cell;
            nud_MaxGray_2_Cell_LackAngle_SL.Value = Recipe.Param.InspParam_LackAngle_SL.MaxGray_2_Cell;
            nud_MaskWidth_2_Cell_LackAngle_SL.Value = Recipe.Param.InspParam_LackAngle_SL.MaskWidth_2_Cell;
            nud_MaskHeight_2_Cell_LackAngle_SL.Value = Recipe.Param.InspParam_LackAngle_SL.MaskHeight_2_Cell;
            combx_str_LightDark_2_Cell_LackAngle_SL.Text = Recipe.Param.InspParam_LackAngle_SL.str_LightDark_2_Cell;
            nud_Offset_2_Cell_LackAngle_SL.Value = Recipe.Param.InspParam_LackAngle_SL.Offset_2_Cell;
            nud_count_BinImg_Resist_LackAngle_SL.Value = Recipe.Param.InspParam_LackAngle_SL.count_BinImg_Resist;
            nud_MinGray_2_Resist_LackAngle_SL.Value = Recipe.Param.InspParam_LackAngle_SL.MinGray_2_Resist;
            nud_MaxGray_2_Resist_LackAngle_SL.Value = Recipe.Param.InspParam_LackAngle_SL.MaxGray_2_Resist;
            nud_MaskWidth_2_Resist_LackAngle_SL.Value = Recipe.Param.InspParam_LackAngle_SL.MaskWidth_2_Resist;
            nud_MaskHeight_2_Resist_LackAngle_SL.Value = Recipe.Param.InspParam_LackAngle_SL.MaskHeight_2_Resist;
            combx_str_LightDark_2_Resist_LackAngle_SL.Text = Recipe.Param.InspParam_LackAngle_SL.str_LightDark_2_Resist;
            nud_Offset_2_Resist_LackAngle_SL.Value = Recipe.Param.InspParam_LackAngle_SL.Offset_2_Resist;

            cbx_Enabled_Gray_select_Cell_LackAngle_SL.Checked = Recipe.Param.InspParam_LackAngle_SL.Enabled_Gray_select_Cell;
            combx_str_Gray_select_Cell_LackAngle_SL.Text = Recipe.Param.InspParam_LackAngle_SL.str_Gray_select_Cell;
            nud_Gray_select_Cell_LackAngle_SL.Value = Recipe.Param.InspParam_LackAngle_SL.Gray_select_Cell;
            cbx_Enabled_Gray_select_Resist_LackAngle_SL.Checked = Recipe.Param.InspParam_LackAngle_SL.Enabled_Gray_select_Resist;
            combx_str_Gray_select_Resist_LackAngle_SL.Text = Recipe.Param.InspParam_LackAngle_SL.str_Gray_select_Resist;
            nud_Gray_select_Resist_LackAngle_SL.Value = Recipe.Param.InspParam_LackAngle_SL.Gray_select_Resist;
            cbx_Enabled_Cell2_Step4_LackAngle_SL.Checked = Recipe.Param.InspParam_LackAngle_SL.Enabled_Cell2;
            nud_W_Cell2_LackAngle_SL.Value = Recipe.Param.InspParam_LackAngle_SL.W_Cell2;
            nud_H_Cell2_LackAngle_SL.Value = Recipe.Param.InspParam_LackAngle_SL.H_Cell2;
            nud_count_op1_LackAngle_SL.Value = Recipe.Param.InspParam_LackAngle_SL.count_op1;
            cbx_Enabled_Height_LackAngle_SL.Checked = Recipe.Param.InspParam_LackAngle_SL.Enabled_Height;
            cbx_Enabled_Width_LackAngle_SL.Checked = Recipe.Param.InspParam_LackAngle_SL.Enabled_Width;
            nud_MinH_df_LackAngle_SL.Value = Recipe.Param.InspParam_LackAngle_SL.MinHeight_defect;
            nud_MaxH_df_LackAngle_SL.Value = Recipe.Param.InspParam_LackAngle_SL.MaxHeight_defect;
            nud_MinW_df_LackAngle_SL.Value = Recipe.Param.InspParam_LackAngle_SL.MinWidth_defect;
            nud_MaxW_df_LackAngle_SL.Value = Recipe.Param.InspParam_LackAngle_SL.MaxWidth_defect;
            combx_str_op1_LackAngle_SL.Text = Recipe.Param.InspParam_LackAngle_SL.str_op1;
            cbx_Enabled_Height_op1_2_LackAngle_SL.Checked = Recipe.Param.InspParam_LackAngle_SL.Enabled_Height_op1_2;
            cbx_Enabled_Width_op1_2_LackAngle_SL.Checked = Recipe.Param.InspParam_LackAngle_SL.Enabled_Width_op1_2;
            nud_MinH_df_op1_2_LackAngle_SL.Value = Recipe.Param.InspParam_LackAngle_SL.MinHeight_defect_op1_2;
            nud_MaxH_df_op1_2_LackAngle_SL.Value = Recipe.Param.InspParam_LackAngle_SL.MaxHeight_defect_op1_2;
            nud_MinW_df_op1_2_LackAngle_SL.Value = Recipe.Param.InspParam_LackAngle_SL.MinWidth_defect_op1_2;
            nud_MaxW_df_op1_2_LackAngle_SL.Value = Recipe.Param.InspParam_LackAngle_SL.MaxWidth_defect_op1_2;
            combx_str_op1_2_LackAngle_SL.Text = Recipe.Param.InspParam_LackAngle_SL.str_op1_2;
            cbx_Enabled_Area_LackAngle_SL.Checked = Recipe.Param.InspParam_LackAngle_SL.Enabled_Area;
            nud_MinA_df_LackAngle_SL.Value = Recipe.Param.InspParam_LackAngle_SL.MinArea_defect;
            nud_MaxA_df_LackAngle_SL.Value = Recipe.Param.InspParam_LackAngle_SL.MaxArea_defect;
            combx_str_op2_LackAngle_SL.Text = Recipe.Param.InspParam_LackAngle_SL.str_op2;

            // 【AI只覆判模稜兩可NG】
            cbx_notSureNG_LackAngle_SL.Checked = Recipe.Param.InspParam_LackAngle_SL.cls_absolNG.Enabled;
            cbx_Enabled_Gray_select_LackAngle_SL_AbsolNG.Checked = Recipe.Param.InspParam_LackAngle_SL.cls_absolNG.Enabled_Gray_select;
            combx_str_Gray_select_LackAngle_SL_AbsolNG.Text = Recipe.Param.InspParam_LackAngle_SL.cls_absolNG.str_Gray_select;
            nud_Gray_select_LackAngle_SL_AbsolNG.Value = Recipe.Param.InspParam_LackAngle_SL.cls_absolNG.Gray_select;
            combx_str_op_LackAngle_SL_AbsolNG.Text = Recipe.Param.InspParam_LackAngle_SL.cls_absolNG.str_op;
            cbx_Enabled_Area_LackAngle_SL_AbsolNG.Checked = Recipe.Param.InspParam_LackAngle_SL.cls_absolNG.Enabled_Area;
            nud_MinA_df_LackAngle_SL_AbsolNG.Value = Recipe.Param.InspParam_LackAngle_SL.cls_absolNG.MinArea_defect;
            nud_MaxA_df_LackAngle_SL_AbsolNG.Value = Recipe.Param.InspParam_LackAngle_SL.cls_absolNG.MaxArea_defect;

            #endregion

            #endregion

            b_LoadRecipe = false;
            b_Initial_State = true;

            Method.Init_Dict_AOI_NG_absolNG(Recipe); // (20190805) Jeff Revised!
        }

        // 190221, andy
        public void UpdateNowInput_ImgList()
        {
            HObject TmpImg;
            HOperatorSet.GenEmptyObj(out TmpImg);

            // Reset
            for (int i = 0; i < Input_ImgList.Count; i++)
            {
                if (Input_ImgList[i] != null)
                {
                    Input_ImgList[i].Dispose();
                    Input_ImgList[i] = null;
                }
            }
            Input_ImgList.Clear();

            // Upate now image list from mapItem
            if (NowMapItem == null) return;
            for (int i = 0; i < NowMapItem.ImgObj.Source.Count; i++)
            {
                Input_ImgList.Add(NowMapItem.ImgObj.Source[i].Clone());

            }

            if (Input_ImgList.Count <= 0)
            {
                HOperatorSet.GenEmptyObj(out TmpImg);
                for (int i = 0; i < Input_ImgList.Count; i++)
                {
                    if (Input_ImgList[i] != null)
                    {
                        Input_ImgList[i].Dispose();
                        Input_ImgList[i] = null;
                    }
                }

                Input_ImgList.Clear();
                Input_ImgList.Add(TmpImg.Clone());
                TmpImg.Dispose();
                TmpImg = null;
            }

            UpdateDisplayList();
            // 顯示影像
            if (Input_ImgList.Count > 0)
            {
                Current_disp_image = Method.Insp_ImgList[0].Clone();
                HOperatorSet.DispObj(Current_disp_image, DisplayWindows.HalconWindow);
                HOperatorSet.SetPart(DisplayWindows.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Window">顯示之Halcon Window</param>
        /// <param name="Image">顯示之影像</param>
        /// <param name="Regions">顯示之區域</param>
        private void DisplayRegions(HWindow Window,HObject Image,HObject Regions,bool IsClear,string Color)
        {
            if (IsClear)
            {
                HOperatorSet.ClearWindow(Window);
                HOperatorSet.DispObj(Image, Window);
            }

            HOperatorSet.SetColor(Window, Color);
            HOperatorSet.SetDraw(Window, "margin");
            HOperatorSet.DispObj(Regions, Window);
        }

        public void ParamGetFromUI() // (20190807) Jeff Revised!
        {
            #region 進階設定內參數

            // 測試模式設定
            Recipe.Param.TestModeEnabled = cbxTestModeEnabled.Checked;
            Recipe.Param.TestModeType = combxTestModeType.SelectedIndex;
            // AOI儲存影像設定
            Recipe.Param.SaveAOIImgEnabled = cbxSaveImgEnable.Checked;
            Recipe.Param.SaveAOIImgType = combxSaveImgType.SelectedIndex;
            // AI Type
            Recipe.Param.DAVSInspType = combxAIType.SelectedIndex;
            // 參數設定
            Recipe.Param.ImgCount = int.Parse(nudImgCount.Value.ToString());
            //Recipe.Param.Resolution = double.Parse(txbResolution.Text.ToString()); // (20190131) Jeff Revised!
            // 黑條方向
            Recipe.Param.Type_BlackStripe = combx_Type_BlackStripe.Text; // (20191112) Jeff Revised!
            // 計算每種瑕疵在Cell的中心位置
            Recipe.Param.Enabled_Compute_df_CellCenter = cbx_Compute_df_CellCenter.Checked;
            // 扭正影像
            Recipe.Param.Enabled_rotate = cbx_rotate.Checked;
            //AI 影像ID
            Recipe.Param.AIImageID = int.Parse(nudAIImgID.Value.ToString());
            // 是否啟用AOI判斷絕對NG
            Recipe.Param.B_AOI_absolNG = cbx_notSureNG.Checked; // (20190807) Jeff Revised!

            #endregion

            #region AOI參數

            #region 初定位

            Recipe.Param.PositionParam_Initial.Enabled_NonInspectCell = cbx_Enabled_NonInspectCell_InitialPosition.Checked;
            Recipe.Param.PositionParam_Initial.L_NonInspect = int.Parse(nud_L_NonInspect_InitialPosition.Value.ToString());
            Recipe.Param.PositionParam_Initial.L_NonInspect_Wsmall = int.Parse(nud_L_NonInspect_Wsmall_InitialPosition.Value.ToString());
            Recipe.Param.PositionParam_Initial.L_NonInspect_Wsmall3 = int.Parse(nud_L_NonInspect_Wsmall3_InitialPosition.Value.ToString());

            // 【分割電阻條】 (20190816) Jeff Revised!
            Recipe.Param.PositionParam_Initial.ImageIndex_SegBlackStripe = int.Parse(nud_ImageIndex_SegBlackStripe.Value.ToString());
            Recipe.Param.PositionParam_Initial.MinGray_SegBlackStripe = int.Parse(nud_MinGray_SegBlackStripe.Value.ToString());
            Recipe.Param.PositionParam_Initial.MaxGray_SegBlackStripe = int.Parse(nud_MaxGray_SegBlackStripe.Value.ToString());
            Recipe.Param.PositionParam_Initial.W_closing_SegBlackStripe = int.Parse(nud_W_closing_SegBlackStripe.Value.ToString());
            Recipe.Param.PositionParam_Initial.H_closing_SegBlackStripe = int.Parse(nud_H_closing_SegBlackStripe.Value.ToString());
            Recipe.Param.PositionParam_Initial.MaxWidth_BlackCrack = int.Parse(nud_MaxWidth_BlackCrack.Value.ToString());
            Recipe.Param.PositionParam_Initial.H_opening_BlackCrack = int.Parse(nud_H_opening_BlackCrack.Value.ToString());
            Recipe.Param.PositionParam_Initial.MinW_BlackStripe = int.Parse(nud_MinW_BlackStripe.Value.ToString());
            Recipe.Param.PositionParam_Initial.MaxW_BlackStripe = int.Parse(nud_MaxW_BlackStripe.Value.ToString());
            Recipe.Param.PositionParam_Initial.MinH_BlackStripe = int.Parse(nud_MinH_BlackStripe.Value.ToString());
            Recipe.Param.PositionParam_Initial.MaxH_BlackStripe = int.Parse(nud_MaxH_BlackStripe.Value.ToString());
            Recipe.Param.PositionParam_Initial.W_dila_BlackStripe = int.Parse(nud_W_dila_BlackStripe.Value.ToString());
            Recipe.Param.PositionParam_Initial.H_dila_BlackStripe = int.Parse(nud_H_dila_BlackStripe.Value.ToString());
            Recipe.Param.PositionParam_Initial.H_ero_BlackStripe_small = int.Parse(nud_H_ero_BlackStripe_small.Value.ToString());

            #endregion

            #region Cell分割 (同軸光) 之參數

            Recipe.Param.InspParam_CellSeg_CL.Enabled = cbx_CellSeg_CL.Checked;
            Recipe.Param.InspParam_CellSeg_CL.ImageIndex = int.Parse(nud_ImageID_CellSeg_CL.Value.ToString());
            Recipe.Param.InspParam_CellSeg_CL.Enabled_removeDef = cbx_Enabled_removeDef_CellSeg_CL.Checked;
            Recipe.Param.InspParam_CellSeg_CL.MinGray_removeDef = int.Parse(nud_MinGray_removeDef_CellSeg_CL.Value.ToString());
            Recipe.Param.InspParam_CellSeg_CL.MaxGray_removeDef = int.Parse(nud_MaxGray_removeDef_CellSeg_CL.Value.ToString());
            Recipe.Param.InspParam_CellSeg_CL.Enabled_equHisto = cbx_Enabled_equHisto_CellSeg_CL.Checked; // (20191112) Jeff Revised!
            Recipe.Param.InspParam_CellSeg_CL.Enabled_equHisto_part = cbx_Enabled_equHisto_part_CellSeg_CL.Checked; // (20191112) Jeff Revised!
            Recipe.Param.InspParam_CellSeg_CL.count_col = int.Parse(nud_count_col_CellSeg_CL.Value.ToString());
            Recipe.Param.InspParam_CellSeg_CL.str_BinaryImg = combx_str_BinaryImg_CellSeg_CL.Text;
            Recipe.Param.InspParam_CellSeg_CL.MaskWidth = int.Parse(nud_MaskWidth_CellSeg_CL.Value.ToString());
            Recipe.Param.InspParam_CellSeg_CL.MaskHeight = int.Parse(nud_MaskHeight_CellSeg_CL.Value.ToString());
            Recipe.Param.InspParam_CellSeg_CL.str_LightDark = combx_str_LightDark_CellSeg_CL.Text;
            Recipe.Param.InspParam_CellSeg_CL.Offset = int.Parse(nud_Offset_CellSeg_CL.Value.ToString());
            Recipe.Param.InspParam_CellSeg_CL.MinGray_CellSeg = int.Parse(nud_MinGray_CellSeg_CL.Value.ToString());
            Recipe.Param.InspParam_CellSeg_CL.MaxGray_CellSeg = int.Parse(nud_MaxGray_CellSeg_CL.Value.ToString());
            Recipe.Param.InspParam_CellSeg_CL.W_opening1_CellSeg = int.Parse(nud_W_opening1_CellSeg_CL.Value.ToString());
            Recipe.Param.InspParam_CellSeg_CL.H_opening1_CellSeg = int.Parse(nud_H_opening1_CellSeg_CL.Value.ToString());
            Recipe.Param.InspParam_CellSeg_CL.W_closing_CellSeg = int.Parse(nud_W_closing_CellSeg_CL.Value.ToString());
            Recipe.Param.InspParam_CellSeg_CL.H_closing_CellSeg = int.Parse(nud_H_closing_CellSeg_CL.Value.ToString());
            Recipe.Param.InspParam_CellSeg_CL.W_opening2_CellSeg = int.Parse(nud_W_opening2_CellSeg_CL.Value.ToString());
            Recipe.Param.InspParam_CellSeg_CL.H_opening2_CellSeg = int.Parse(nud_H_opening2_CellSeg_CL.Value.ToString());
            Recipe.Param.InspParam_CellSeg_CL.W_dila_CellSeg = int.Parse(nud_W_dila_CellSeg_CL.Value.ToString());
            Recipe.Param.InspParam_CellSeg_CL.H_dila_CellSeg = int.Parse(nud_H_dila_CellSeg_CL.Value.ToString());
            Recipe.Param.InspParam_CellSeg_CL.MinHeight_CellSeg = int.Parse(nud_MinHeight_CellSeg_CL.Value.ToString());
            Recipe.Param.InspParam_CellSeg_CL.MaxHeight_CellSeg = int.Parse(nud_MaxHeight_CellSeg_CL.Value.ToString());
            Recipe.Param.InspParam_CellSeg_CL.MinWidth_CellSeg = int.Parse(nud_MinWidth_CellSeg_CL.Value.ToString());
            Recipe.Param.InspParam_CellSeg_CL.MaxWidth_CellSeg = int.Parse(nud_MaxWidth_CellSeg_CL.Value.ToString());
            Recipe.Param.InspParam_CellSeg_CL.str_ModeCellSeg = combx_str_ModeCellSeg_CellSeg_CL.Text;
            Recipe.Param.InspParam_CellSeg_CL.Enabled_RemainValidCells = cbx_Enabled_RemainValidCells_CellSeg_CL.Checked;

            #endregion

            #region Cell分割 (背光) 之參數

            Recipe.Param.InspParam_CellSeg_BL.Enabled = cbx_CellSeg_BL.Checked;
            Recipe.Param.InspParam_CellSeg_BL.ImageIndex = int.Parse(nud_ImageID_CellSeg_BL.Value.ToString());
            Recipe.Param.InspParam_CellSeg_BL.Enabled_removeDef = cbx_Enabled_removeDef_CellSeg_BL.Checked;
            Recipe.Param.InspParam_CellSeg_BL.MinGray_removeDef = int.Parse(nud_MinGray_removeDef_CellSeg_BL.Value.ToString());
            Recipe.Param.InspParam_CellSeg_BL.MaxGray_removeDef = int.Parse(nud_MaxGray_removeDef_CellSeg_BL.Value.ToString());
            Recipe.Param.InspParam_CellSeg_BL.Enabled_equHisto = cbx_Enabled_equHisto_CellSeg_BL.Checked; // (20191112) Jeff Revised!
            Recipe.Param.InspParam_CellSeg_BL.Enabled_equHisto_part = cbx_Enabled_equHisto_part_CellSeg_BL.Checked; // (20191112) Jeff Revised!
            Recipe.Param.InspParam_CellSeg_BL.count_col = int.Parse(nud_count_col_CellSeg_BL.Value.ToString());
            Recipe.Param.InspParam_CellSeg_BL.str_BinaryImg = combx_str_BinaryImg_CellSeg_BL.Text;
            Recipe.Param.InspParam_CellSeg_BL.MaskWidth = int.Parse(nud_MaskWidth_CellSeg_BL.Value.ToString());
            Recipe.Param.InspParam_CellSeg_BL.MaskHeight = int.Parse(nud_MaskHeight_CellSeg_BL.Value.ToString());
            Recipe.Param.InspParam_CellSeg_BL.str_LightDark = combx_str_LightDark_CellSeg_BL.Text;
            Recipe.Param.InspParam_CellSeg_BL.Offset = int.Parse(nud_Offset_CellSeg_BL.Value.ToString());
            Recipe.Param.InspParam_CellSeg_BL.MinGray_CellSeg = int.Parse(nud_MinGray_CellSeg_BL.Value.ToString());
            Recipe.Param.InspParam_CellSeg_BL.MaxGray_CellSeg = int.Parse(nud_MaxGray_CellSeg_BL.Value.ToString());
            Recipe.Param.InspParam_CellSeg_BL.W_opening1_CellSeg = int.Parse(nud_W_opening1_CellSeg_BL.Value.ToString());
            Recipe.Param.InspParam_CellSeg_BL.H_opening1_CellSeg = int.Parse(nud_H_opening1_CellSeg_BL.Value.ToString());
            Recipe.Param.InspParam_CellSeg_BL.W_closing_CellSeg = int.Parse(nud_W_closing_CellSeg_BL.Value.ToString());
            Recipe.Param.InspParam_CellSeg_BL.H_closing_CellSeg = int.Parse(nud_H_closing_CellSeg_BL.Value.ToString());
            Recipe.Param.InspParam_CellSeg_BL.W_opening2_CellSeg = int.Parse(nud_W_opening2_CellSeg_BL.Value.ToString());
            Recipe.Param.InspParam_CellSeg_BL.H_opening2_CellSeg = int.Parse(nud_H_opening2_CellSeg_BL.Value.ToString());
            Recipe.Param.InspParam_CellSeg_BL.W_dila_CellSeg = int.Parse(nud_W_dila_CellSeg_BL.Value.ToString());
            Recipe.Param.InspParam_CellSeg_BL.H_dila_CellSeg = int.Parse(nud_H_dila_CellSeg_BL.Value.ToString());
            Recipe.Param.InspParam_CellSeg_BL.MinHeight_CellSeg = int.Parse(nud_MinHeight_CellSeg_BL.Value.ToString());
            Recipe.Param.InspParam_CellSeg_BL.MaxHeight_CellSeg = int.Parse(nud_MaxHeight_CellSeg_BL.Value.ToString());
            Recipe.Param.InspParam_CellSeg_BL.MinWidth_CellSeg = int.Parse(nud_MinWidth_CellSeg_BL.Value.ToString());
            Recipe.Param.InspParam_CellSeg_BL.MaxWidth_CellSeg = int.Parse(nud_MaxWidth_CellSeg_BL.Value.ToString());
            Recipe.Param.InspParam_CellSeg_BL.str_ModeCellSeg = combx_str_ModeCellSeg_CellSeg_BL.Text;
            Recipe.Param.InspParam_CellSeg_BL.Enabled_RemainValidCells = cbx_Enabled_RemainValidCells_CellSeg_BL.Checked;

            #endregion

            #region 瑕疵Cell判斷範圍 之參數

            Recipe.Param.InspParam_Df_CellReg.Enabled_InspReg = cbx_Enabled_InspReg_Df_CellReg.Checked;
            Recipe.Param.InspParam_Df_CellReg.W_InspReg = int.Parse(nud_W_InspReg_Df_CellReg.Value.ToString());
            Recipe.Param.InspParam_Df_CellReg.H_InspReg = int.Parse(nud_H_InspReg_Df_CellReg.Value.ToString());
            Recipe.Param.InspParam_Df_CellReg.W_Cell = int.Parse(nud_W_Cell_Df_CellReg.Value.ToString());
            Recipe.Param.InspParam_Df_CellReg.H_Cell = int.Parse(nud_H_Cell_Df_CellReg.Value.ToString());

            #endregion

            #region 檢測黑條內白點 (正光) (Frontal light) 之參數

            Recipe.Param.InspParam_WhiteBlob_FL.Enabled = cbx_WhiteBlob_FL.Checked;
            Recipe.Param.InspParam_WhiteBlob_FL.ImageIndex = int.Parse(nud_ImageID_WhiteBlob_FL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_FL.Enabled_Wsmall = cbx_Enabled_Wsmall_WhiteBlob_FL.Checked;
            Recipe.Param.InspParam_WhiteBlob_FL.str_L_NonInspect = combx_str_L_NonInspect_WhiteBlob_FL.Text;
            Recipe.Param.InspParam_WhiteBlob_FL.Enabled_Cell = cbx_Enabled_Cell_Step1_WhiteBlob_FL.Checked;
            Recipe.Param.InspParam_WhiteBlob_FL.Enabled_Cell = cbx_Enabled_Cell_Step2_WhiteBlob_FL.Checked;
            Recipe.Param.InspParam_WhiteBlob_FL.W_Cell = int.Parse(nud_W_Cell_WhiteBlob_FL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_FL.H_Cell = int.Parse(nud_H_Cell_WhiteBlob_FL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_FL.Enabled_BypassReg = cbx_Enabled_BypassReg_WhiteBlob_FL.Checked;
            Recipe.Param.InspParam_WhiteBlob_FL.x_BypassReg = int.Parse(nud_x_BypassReg_WhiteBlob_FL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_FL.y_BypassReg = int.Parse(nud_y_BypassReg_WhiteBlob_FL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_FL.W_BypassReg = int.Parse(nud_W_BypassReg_WhiteBlob_FL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_FL.H_BypassReg = int.Parse(nud_H_BypassReg_WhiteBlob_FL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_FL.Enabled_BlackStripe = cbx_Enabled_BlackStripe_Step1_WhiteBlob_FL.Checked;
            Recipe.Param.InspParam_WhiteBlob_FL.Enabled_BlackStripe = cbx_Enabled_BlackStripe_Step2_WhiteBlob_FL.Checked;
            Recipe.Param.InspParam_WhiteBlob_FL.str_ModeBlackStripe = combx_str_ModeBlackStripe_WhiteBlob_FL.Text;
            Recipe.Param.InspParam_WhiteBlob_FL.H_BlackStripe = int.Parse(nud_H_BlackStripe_WhiteBlob_FL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_FL.H_BlackStripe_mode2 = int.Parse(nud_H_BlackStripe_mode2_WhiteBlob_FL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_FL.str_BinaryImg_Cell = combx_str_BinaryImg_Cell_WhiteBlob_FL.Text;
            Recipe.Param.InspParam_WhiteBlob_FL.MaskWidth_Cell = int.Parse(nud_MaskWidth_1_Cell_WhiteBlob_FL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_FL.MaskHeight_Cell = int.Parse(nud_MaskHeight_1_Cell_WhiteBlob_FL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_FL.str_LightDark_Cell = combx_str_LightDark_1_Cell_WhiteBlob_FL.Text;
            Recipe.Param.InspParam_WhiteBlob_FL.Offset_Cell = int.Parse(nud_Offset_1_Cell_WhiteBlob_FL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_FL.MinGray_Cell = int.Parse(nud_MinGray_1_Cell_WhiteBlob_FL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_FL.MaxGray_Cell = int.Parse(nud_MaxGray_1_Cell_WhiteBlob_FL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_FL.str_BinaryImg_Resist = combx_str_BinaryImg_Resist_WhiteBlob_FL.Text;
            Recipe.Param.InspParam_WhiteBlob_FL.MaskWidth_Resist = int.Parse(nud_MaskWidth_1_Resist_WhiteBlob_FL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_FL.MaskHeight_Resist = int.Parse(nud_MaskHeight_1_Resist_WhiteBlob_FL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_FL.str_LightDark_Resist = combx_str_LightDark_1_Resist_WhiteBlob_FL.Text;
            Recipe.Param.InspParam_WhiteBlob_FL.Offset_Resist = int.Parse(nud_Offset_1_Resist_WhiteBlob_FL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_FL.MinGray_Resist = int.Parse(nud_MinGray_1_Resist_WhiteBlob_FL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_FL.MaxGray_Resist = int.Parse(nud_MaxGray_1_Resist_WhiteBlob_FL.Value.ToString());

            // 規格2 (20190411) Jeff Revised!
            Recipe.Param.InspParam_WhiteBlob_FL.count_BinImg_Cell = int.Parse(nud_count_BinImg_Cell_WhiteBlob_FL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_FL.MinGray_2_Cell = int.Parse(nud_MinGray_2_Cell_WhiteBlob_FL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_FL.MaxGray_2_Cell = int.Parse(nud_MaxGray_2_Cell_WhiteBlob_FL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_FL.MaskWidth_2_Cell = int.Parse(nud_MaskWidth_2_Cell_WhiteBlob_FL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_FL.MaskHeight_2_Cell = int.Parse(nud_MaskHeight_2_Cell_WhiteBlob_FL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_FL.str_LightDark_2_Cell = combx_str_LightDark_2_Cell_WhiteBlob_FL.Text;
            Recipe.Param.InspParam_WhiteBlob_FL.Offset_2_Cell = int.Parse(nud_Offset_2_Cell_WhiteBlob_FL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_FL.count_BinImg_Resist = int.Parse(nud_count_BinImg_Resist_WhiteBlob_FL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_FL.MinGray_2_Resist = int.Parse(nud_MinGray_2_Resist_WhiteBlob_FL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_FL.MaxGray_2_Resist = int.Parse(nud_MaxGray_2_Resist_WhiteBlob_FL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_FL.MaskWidth_2_Resist = int.Parse(nud_MaskWidth_2_Resist_WhiteBlob_FL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_FL.MaskHeight_2_Resist = int.Parse(nud_MaskHeight_2_Resist_WhiteBlob_FL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_FL.str_LightDark_2_Resist = combx_str_LightDark_2_Resist_WhiteBlob_FL.Text;
            Recipe.Param.InspParam_WhiteBlob_FL.Offset_2_Resist = int.Parse(nud_Offset_2_Resist_WhiteBlob_FL.Value.ToString());

            Recipe.Param.InspParam_WhiteBlob_FL.Enabled_Gray_select_Cell = cbx_Enabled_Gray_select_Cell_WhiteBlob_FL.Checked;
            Recipe.Param.InspParam_WhiteBlob_FL.str_Gray_select_Cell = combx_str_Gray_select_Cell_WhiteBlob_FL.Text;
            Recipe.Param.InspParam_WhiteBlob_FL.Gray_select_Cell = int.Parse(nud_Gray_select_Cell_WhiteBlob_FL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_FL.Enabled_Gray_select_Resist = cbx_Enabled_Gray_select_Resist_WhiteBlob_FL.Checked;
            Recipe.Param.InspParam_WhiteBlob_FL.str_Gray_select_Resist = combx_str_Gray_select_Resist_WhiteBlob_FL.Text;
            Recipe.Param.InspParam_WhiteBlob_FL.Gray_select_Resist = int.Parse(nud_Gray_select_Resist_WhiteBlob_FL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_FL.Enabled_Cell2 = cbx_Enabled_Cell2_Step4_WhiteBlob_FL.Checked;
            Recipe.Param.InspParam_WhiteBlob_FL.W_Cell2 = int.Parse(nud_W_Cell2_WhiteBlob_FL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_FL.H_Cell2 = int.Parse(nud_H_Cell2_WhiteBlob_FL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_FL.count_op1 = int.Parse(nud_count_op1_WhiteBlob_FL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_FL.Enabled_Height = cbx_Enabled_Height_WhiteBlob_FL.Checked;
            Recipe.Param.InspParam_WhiteBlob_FL.Enabled_Width = cbx_Enabled_Width_WhiteBlob_FL.Checked;
            Recipe.Param.InspParam_WhiteBlob_FL.MinHeight_defect = int.Parse(nud_MinH_df_WhiteBlob_FL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_FL.MaxHeight_defect = int.Parse(nud_MaxH_df_WhiteBlob_FL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_FL.MinWidth_defect = int.Parse(nud_MinW_df_WhiteBlob_FL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_FL.MaxWidth_defect = int.Parse(nud_MaxW_df_WhiteBlob_FL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_FL.str_op1 = combx_str_op1_WhiteBlob_FL.Text;
            Recipe.Param.InspParam_WhiteBlob_FL.Enabled_Height_op1_2 = cbx_Enabled_Height_op1_2_WhiteBlob_FL.Checked;
            Recipe.Param.InspParam_WhiteBlob_FL.Enabled_Width_op1_2 = cbx_Enabled_Width_op1_2_WhiteBlob_FL.Checked;
            Recipe.Param.InspParam_WhiteBlob_FL.MinHeight_defect_op1_2 = int.Parse(nud_MinH_df_op1_2_WhiteBlob_FL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_FL.MaxHeight_defect_op1_2 = int.Parse(nud_MaxH_df_op1_2_WhiteBlob_FL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_FL.MinWidth_defect_op1_2 = int.Parse(nud_MinW_df_op1_2_WhiteBlob_FL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_FL.MaxWidth_defect_op1_2 = int.Parse(nud_MaxW_df_op1_2_WhiteBlob_FL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_FL.str_op1_2 = combx_str_op1_2_WhiteBlob_FL.Text;
            Recipe.Param.InspParam_WhiteBlob_FL.Enabled_Area = cbx_Enabled_Area_WhiteBlob_FL.Checked;
            Recipe.Param.InspParam_WhiteBlob_FL.MinArea_defect = int.Parse(nud_MinA_df_WhiteBlob_FL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_FL.MaxArea_defect = int.Parse(nud_MaxA_df_WhiteBlob_FL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_FL.str_op2 = combx_str_op2_WhiteBlob_FL.Text;

            // 【AI只覆判模稜兩可NG】
            Recipe.Param.InspParam_WhiteBlob_FL.cls_absolNG.Enabled = cbx_notSureNG_WhiteBlob_FL.Checked;
            Recipe.Param.InspParam_WhiteBlob_FL.cls_absolNG.Enabled_Gray_select = cbx_Enabled_Gray_select_WhiteBlob_FL_AbsolNG.Checked;
            Recipe.Param.InspParam_WhiteBlob_FL.cls_absolNG.str_Gray_select = combx_str_Gray_select_WhiteBlob_FL_AbsolNG.Text;
            Recipe.Param.InspParam_WhiteBlob_FL.cls_absolNG.Gray_select = int.Parse(nud_Gray_select_WhiteBlob_FL_AbsolNG.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_FL.cls_absolNG.str_op = combx_str_op_WhiteBlob_FL_AbsolNG.Text;
            Recipe.Param.InspParam_WhiteBlob_FL.cls_absolNG.Enabled_Area = cbx_Enabled_Area_WhiteBlob_FL_AbsolNG.Checked;
            Recipe.Param.InspParam_WhiteBlob_FL.cls_absolNG.MinArea_defect = int.Parse(nud_MinA_df_WhiteBlob_FL_AbsolNG.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_FL.cls_absolNG.MaxArea_defect = int.Parse(nud_MaxA_df_WhiteBlob_FL_AbsolNG.Value.ToString());

            #endregion

            #region 檢測黑條內白點 (同軸光) (Coaxial light) 之參數

            Recipe.Param.InspParam_WhiteBlob_CL.Enabled = cbx_WhiteBlob_CL.Checked;
            Recipe.Param.InspParam_WhiteBlob_CL.ImageIndex = int.Parse(nud_ImageID_WhiteBlob_CL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_CL.Enabled_Wsmall = cbx_Enabled_Wsmall_WhiteBlob_CL.Checked;
            Recipe.Param.InspParam_WhiteBlob_CL.str_L_NonInspect = combx_str_L_NonInspect_WhiteBlob_CL.Text;
            Recipe.Param.InspParam_WhiteBlob_CL.Enabled_Cell = cbx_Enabled_Cell_Step1_WhiteBlob_CL.Checked;
            Recipe.Param.InspParam_WhiteBlob_CL.Enabled_Cell = cbx_Enabled_Cell_Step2_WhiteBlob_CL.Checked;
            Recipe.Param.InspParam_WhiteBlob_CL.W_Cell = int.Parse(nud_W_Cell_WhiteBlob_CL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_CL.H_Cell = int.Parse(nud_H_Cell_WhiteBlob_CL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_CL.Enabled_BypassReg = cbx_Enabled_BypassReg_WhiteBlob_CL.Checked;
            Recipe.Param.InspParam_WhiteBlob_CL.x_BypassReg = int.Parse(nud_x_BypassReg_WhiteBlob_CL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_CL.y_BypassReg = int.Parse(nud_y_BypassReg_WhiteBlob_CL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_CL.W_BypassReg = int.Parse(nud_W_BypassReg_WhiteBlob_CL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_CL.H_BypassReg = int.Parse(nud_H_BypassReg_WhiteBlob_CL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_CL.Enabled_BlackStripe = cbx_Enabled_BlackStripe_Step1_WhiteBlob_CL.Checked;
            Recipe.Param.InspParam_WhiteBlob_CL.Enabled_BlackStripe = cbx_Enabled_BlackStripe_Step2_WhiteBlob_CL.Checked;
            Recipe.Param.InspParam_WhiteBlob_CL.str_ModeBlackStripe = combx_str_ModeBlackStripe_WhiteBlob_CL.Text;
            Recipe.Param.InspParam_WhiteBlob_CL.H_BlackStripe = int.Parse(nud_H_BlackStripe_WhiteBlob_CL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_CL.H_BlackStripe_mode2 = int.Parse(nud_H_BlackStripe_mode2_WhiteBlob_CL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_CL.str_BinaryImg_Cell = combx_str_BinaryImg_Cell_WhiteBlob_CL.Text;
            Recipe.Param.InspParam_WhiteBlob_CL.MaskWidth_Cell = int.Parse(nud_MaskWidth_1_Cell_WhiteBlob_CL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_CL.MaskHeight_Cell = int.Parse(nud_MaskHeight_1_Cell_WhiteBlob_CL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_CL.str_LightDark_Cell = combx_str_LightDark_1_Cell_WhiteBlob_CL.Text;
            Recipe.Param.InspParam_WhiteBlob_CL.Offset_Cell = int.Parse(nud_Offset_1_Cell_WhiteBlob_CL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_CL.MinGray_Cell = int.Parse(nud_MinGray_1_Cell_WhiteBlob_CL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_CL.MaxGray_Cell = int.Parse(nud_MaxGray_1_Cell_WhiteBlob_CL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_CL.str_BinaryImg_Resist = combx_str_BinaryImg_Resist_WhiteBlob_CL.Text;
            Recipe.Param.InspParam_WhiteBlob_CL.MaskWidth_Resist = int.Parse(nud_MaskWidth_1_Resist_WhiteBlob_CL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_CL.MaskHeight_Resist = int.Parse(nud_MaskHeight_1_Resist_WhiteBlob_CL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_CL.str_LightDark_Resist = combx_str_LightDark_1_Resist_WhiteBlob_CL.Text;
            Recipe.Param.InspParam_WhiteBlob_CL.Offset_Resist = int.Parse(nud_Offset_1_Resist_WhiteBlob_CL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_CL.MinGray_Resist = int.Parse(nud_MinGray_1_Resist_WhiteBlob_CL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_CL.MaxGray_Resist = int.Parse(nud_MaxGray_1_Resist_WhiteBlob_CL.Value.ToString());

            // 規格2 (20190411) Jeff Revised!
            Recipe.Param.InspParam_WhiteBlob_CL.count_BinImg_Cell = int.Parse(nud_count_BinImg_Cell_WhiteBlob_CL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_CL.MinGray_2_Cell = int.Parse(nud_MinGray_2_Cell_WhiteBlob_CL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_CL.MaxGray_2_Cell = int.Parse(nud_MaxGray_2_Cell_WhiteBlob_CL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_CL.MaskWidth_2_Cell = int.Parse(nud_MaskWidth_2_Cell_WhiteBlob_CL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_CL.MaskHeight_2_Cell = int.Parse(nud_MaskHeight_2_Cell_WhiteBlob_CL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_CL.str_LightDark_2_Cell = combx_str_LightDark_2_Cell_WhiteBlob_CL.Text;
            Recipe.Param.InspParam_WhiteBlob_CL.Offset_2_Cell = int.Parse(nud_Offset_2_Cell_WhiteBlob_CL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_CL.count_BinImg_Resist = int.Parse(nud_count_BinImg_Resist_WhiteBlob_CL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_CL.MinGray_2_Resist = int.Parse(nud_MinGray_2_Resist_WhiteBlob_CL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_CL.MaxGray_2_Resist = int.Parse(nud_MaxGray_2_Resist_WhiteBlob_CL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_CL.MaskWidth_2_Resist = int.Parse(nud_MaskWidth_2_Resist_WhiteBlob_CL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_CL.MaskHeight_2_Resist = int.Parse(nud_MaskHeight_2_Resist_WhiteBlob_CL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_CL.str_LightDark_2_Resist = combx_str_LightDark_2_Resist_WhiteBlob_CL.Text;
            Recipe.Param.InspParam_WhiteBlob_CL.Offset_2_Resist = int.Parse(nud_Offset_2_Resist_WhiteBlob_CL.Value.ToString());

            Recipe.Param.InspParam_WhiteBlob_CL.Enabled_Gray_select_Cell = cbx_Enabled_Gray_select_Cell_WhiteBlob_CL.Checked;
            Recipe.Param.InspParam_WhiteBlob_CL.str_Gray_select_Cell = combx_str_Gray_select_Cell_WhiteBlob_CL.Text;
            Recipe.Param.InspParam_WhiteBlob_CL.Gray_select_Cell = int.Parse(nud_Gray_select_Cell_WhiteBlob_CL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_CL.Enabled_Gray_select_Resist = cbx_Enabled_Gray_select_Resist_WhiteBlob_CL.Checked;
            Recipe.Param.InspParam_WhiteBlob_CL.str_Gray_select_Resist = combx_str_Gray_select_Resist_WhiteBlob_CL.Text;
            Recipe.Param.InspParam_WhiteBlob_CL.Gray_select_Resist = int.Parse(nud_Gray_select_Resist_WhiteBlob_CL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_CL.Enabled_Cell2 = cbx_Enabled_Cell2_Step4_WhiteBlob_CL.Checked;
            Recipe.Param.InspParam_WhiteBlob_CL.W_Cell2 = int.Parse(nud_W_Cell2_WhiteBlob_CL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_CL.H_Cell2 = int.Parse(nud_H_Cell2_WhiteBlob_CL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_CL.count_op1 = int.Parse(nud_count_op1_WhiteBlob_CL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_CL.Enabled_Height = cbx_Enabled_Height_WhiteBlob_CL.Checked;
            Recipe.Param.InspParam_WhiteBlob_CL.Enabled_Width = cbx_Enabled_Width_WhiteBlob_CL.Checked;
            Recipe.Param.InspParam_WhiteBlob_CL.MinHeight_defect = int.Parse(nud_MinH_df_WhiteBlob_CL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_CL.MaxHeight_defect = int.Parse(nud_MaxH_df_WhiteBlob_CL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_CL.MinWidth_defect = int.Parse(nud_MinW_df_WhiteBlob_CL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_CL.MaxWidth_defect = int.Parse(nud_MaxW_df_WhiteBlob_CL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_CL.str_op1 = combx_str_op1_WhiteBlob_CL.Text;
            Recipe.Param.InspParam_WhiteBlob_CL.Enabled_Height_op1_2 = cbx_Enabled_Height_op1_2_WhiteBlob_CL.Checked;
            Recipe.Param.InspParam_WhiteBlob_CL.Enabled_Width_op1_2 = cbx_Enabled_Width_op1_2_WhiteBlob_CL.Checked;
            Recipe.Param.InspParam_WhiteBlob_CL.MinHeight_defect_op1_2 = int.Parse(nud_MinH_df_op1_2_WhiteBlob_CL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_CL.MaxHeight_defect_op1_2 = int.Parse(nud_MaxH_df_op1_2_WhiteBlob_CL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_CL.MinWidth_defect_op1_2 = int.Parse(nud_MinW_df_op1_2_WhiteBlob_CL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_CL.MaxWidth_defect_op1_2 = int.Parse(nud_MaxW_df_op1_2_WhiteBlob_CL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_CL.str_op1_2 = combx_str_op1_2_WhiteBlob_CL.Text;
            Recipe.Param.InspParam_WhiteBlob_CL.Enabled_Area = cbx_Enabled_Area_WhiteBlob_CL.Checked;
            Recipe.Param.InspParam_WhiteBlob_CL.MinArea_defect = int.Parse(nud_MinA_df_WhiteBlob_CL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_CL.MaxArea_defect = int.Parse(nud_MaxA_df_WhiteBlob_CL.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_CL.str_op2 = combx_str_op2_WhiteBlob_CL.Text;

            // 【AI只覆判模稜兩可NG】
            Recipe.Param.InspParam_WhiteBlob_CL.cls_absolNG.Enabled = cbx_notSureNG_WhiteBlob_CL.Checked;
            Recipe.Param.InspParam_WhiteBlob_CL.cls_absolNG.Enabled_Gray_select = cbx_Enabled_Gray_select_WhiteBlob_CL_AbsolNG.Checked;
            Recipe.Param.InspParam_WhiteBlob_CL.cls_absolNG.str_Gray_select = combx_str_Gray_select_WhiteBlob_CL_AbsolNG.Text;
            Recipe.Param.InspParam_WhiteBlob_CL.cls_absolNG.Gray_select = int.Parse(nud_Gray_select_WhiteBlob_CL_AbsolNG.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_CL.cls_absolNG.str_op = combx_str_op_WhiteBlob_CL_AbsolNG.Text;
            Recipe.Param.InspParam_WhiteBlob_CL.cls_absolNG.Enabled_Area = cbx_Enabled_Area_WhiteBlob_CL_AbsolNG.Checked;
            Recipe.Param.InspParam_WhiteBlob_CL.cls_absolNG.MinArea_defect = int.Parse(nud_MinA_df_WhiteBlob_CL_AbsolNG.Value.ToString());
            Recipe.Param.InspParam_WhiteBlob_CL.cls_absolNG.MaxArea_defect = int.Parse(nud_MaxA_df_WhiteBlob_CL_AbsolNG.Value.ToString());

            #endregion

            #region 檢測絲狀異物 (正光) (Frontal Light) 之參數

            Recipe.Param.InspParam_Line_FL.Enabled = cbx_Line_FL.Checked;
            Recipe.Param.InspParam_Line_FL.ImageIndex = int.Parse(nud_ImageID_Line_FL.Value.ToString());
            Recipe.Param.InspParam_Line_FL.Enabled_Wsmall = cbx_Enabled_Wsmall_Line_FL.Checked;
            Recipe.Param.InspParam_Line_FL.str_L_NonInspect = combx_str_L_NonInspect_Line_FL.Text;
            Recipe.Param.InspParam_Line_FL.Enabled_Cell = cbx_Enabled_Cell_Step1_Line_FL.Checked;
            Recipe.Param.InspParam_Line_FL.Enabled_Cell = cbx_Enabled_Cell_Step2_Line_FL.Checked;
            Recipe.Param.InspParam_Line_FL.W_Cell = int.Parse(nud_W_Cell_Line_FL.Value.ToString());
            Recipe.Param.InspParam_Line_FL.H_Cell = int.Parse(nud_H_Cell_Line_FL.Value.ToString());
            Recipe.Param.InspParam_Line_FL.Enabled_BypassReg = cbx_Enabled_BypassReg_Line_FL.Checked;
            Recipe.Param.InspParam_Line_FL.x_BypassReg = int.Parse(nud_x_BypassReg_Line_FL.Value.ToString());
            Recipe.Param.InspParam_Line_FL.y_BypassReg = int.Parse(nud_y_BypassReg_Line_FL.Value.ToString());
            Recipe.Param.InspParam_Line_FL.W_BypassReg = int.Parse(nud_W_BypassReg_Line_FL.Value.ToString());
            Recipe.Param.InspParam_Line_FL.H_BypassReg = int.Parse(nud_H_BypassReg_Line_FL.Value.ToString());
            Recipe.Param.InspParam_Line_FL.Enabled_BlackStripe = cbx_Enabled_BlackStripe_Step1_Line_FL.Checked;
            Recipe.Param.InspParam_Line_FL.Enabled_BlackStripe = cbx_Enabled_BlackStripe_Step2_Line_FL.Checked;
            Recipe.Param.InspParam_Line_FL.str_ModeBlackStripe = combx_str_ModeBlackStripe_Line_FL.Text;
            Recipe.Param.InspParam_Line_FL.H_BlackStripe = int.Parse(nud_H_BlackStripe_Line_FL.Value.ToString());
            Recipe.Param.InspParam_Line_FL.H_BlackStripe_mode2 = int.Parse(nud_H_BlackStripe_mode2_Line_FL.Value.ToString());
            Recipe.Param.InspParam_Line_FL.str_BinaryImg_Cell = combx_str_BinaryImg_Cell_Line_FL.Text;
            Recipe.Param.InspParam_Line_FL.MaskWidth_Cell = int.Parse(nud_MaskWidth_1_Cell_Line_FL.Value.ToString());
            Recipe.Param.InspParam_Line_FL.MaskHeight_Cell = int.Parse(nud_MaskHeight_1_Cell_Line_FL.Value.ToString());
            Recipe.Param.InspParam_Line_FL.str_LightDark_Cell = combx_str_LightDark_1_Cell_Line_FL.Text;
            Recipe.Param.InspParam_Line_FL.Offset_Cell = int.Parse(nud_Offset_1_Cell_Line_FL.Value.ToString());
            Recipe.Param.InspParam_Line_FL.MinGray_Cell = int.Parse(nud_MinGray_1_Cell_Line_FL.Value.ToString());
            Recipe.Param.InspParam_Line_FL.MaxGray_Cell = int.Parse(nud_MaxGray_1_Cell_Line_FL.Value.ToString());
            Recipe.Param.InspParam_Line_FL.str_BinaryImg_Resist = combx_str_BinaryImg_Resist_Line_FL.Text;
            Recipe.Param.InspParam_Line_FL.MaskWidth_Resist = int.Parse(nud_MaskWidth_1_Resist_Line_FL.Value.ToString());
            Recipe.Param.InspParam_Line_FL.MaskHeight_Resist = int.Parse(nud_MaskHeight_1_Resist_Line_FL.Value.ToString());
            Recipe.Param.InspParam_Line_FL.str_LightDark_Resist = combx_str_LightDark_1_Resist_Line_FL.Text;
            Recipe.Param.InspParam_Line_FL.Offset_Resist = int.Parse(nud_Offset_1_Resist_Line_FL.Value.ToString());
            Recipe.Param.InspParam_Line_FL.MinGray_Resist = int.Parse(nud_MinGray_1_Resist_Line_FL.Value.ToString());
            Recipe.Param.InspParam_Line_FL.MaxGray_Resist = int.Parse(nud_MaxGray_1_Resist_Line_FL.Value.ToString());

            // 規格2 (20190411) Jeff Revised!
            Recipe.Param.InspParam_Line_FL.count_BinImg_Cell = int.Parse(nud_count_BinImg_Cell_Line_FL.Value.ToString());
            Recipe.Param.InspParam_Line_FL.MinGray_2_Cell = int.Parse(nud_MinGray_2_Cell_Line_FL.Value.ToString());
            Recipe.Param.InspParam_Line_FL.MaxGray_2_Cell = int.Parse(nud_MaxGray_2_Cell_Line_FL.Value.ToString());
            Recipe.Param.InspParam_Line_FL.MaskWidth_2_Cell = int.Parse(nud_MaskWidth_2_Cell_Line_FL.Value.ToString());
            Recipe.Param.InspParam_Line_FL.MaskHeight_2_Cell = int.Parse(nud_MaskHeight_2_Cell_Line_FL.Value.ToString());
            Recipe.Param.InspParam_Line_FL.str_LightDark_2_Cell = combx_str_LightDark_2_Cell_Line_FL.Text;
            Recipe.Param.InspParam_Line_FL.Offset_2_Cell = int.Parse(nud_Offset_2_Cell_Line_FL.Value.ToString());
            Recipe.Param.InspParam_Line_FL.count_BinImg_Resist = int.Parse(nud_count_BinImg_Resist_Line_FL.Value.ToString());
            Recipe.Param.InspParam_Line_FL.MinGray_2_Resist = int.Parse(nud_MinGray_2_Resist_Line_FL.Value.ToString());
            Recipe.Param.InspParam_Line_FL.MaxGray_2_Resist = int.Parse(nud_MaxGray_2_Resist_Line_FL.Value.ToString());
            Recipe.Param.InspParam_Line_FL.MaskWidth_2_Resist = int.Parse(nud_MaskWidth_2_Resist_Line_FL.Value.ToString());
            Recipe.Param.InspParam_Line_FL.MaskHeight_2_Resist = int.Parse(nud_MaskHeight_2_Resist_Line_FL.Value.ToString());
            Recipe.Param.InspParam_Line_FL.str_LightDark_2_Resist = combx_str_LightDark_2_Resist_Line_FL.Text;
            Recipe.Param.InspParam_Line_FL.Offset_2_Resist = int.Parse(nud_Offset_2_Resist_Line_FL.Value.ToString());

            Recipe.Param.InspParam_Line_FL.Enabled_Gray_select_Cell = cbx_Enabled_Gray_select_Cell_Line_FL.Checked;
            Recipe.Param.InspParam_Line_FL.str_Gray_select_Cell = combx_str_Gray_select_Cell_Line_FL.Text;
            Recipe.Param.InspParam_Line_FL.Gray_select_Cell = int.Parse(nud_Gray_select_Cell_Line_FL.Value.ToString());
            Recipe.Param.InspParam_Line_FL.Enabled_Gray_select_Resist = cbx_Enabled_Gray_select_Resist_Line_FL.Checked;
            Recipe.Param.InspParam_Line_FL.str_Gray_select_Resist = combx_str_Gray_select_Resist_Line_FL.Text;
            Recipe.Param.InspParam_Line_FL.Gray_select_Resist = int.Parse(nud_Gray_select_Resist_Line_FL.Value.ToString());
            Recipe.Param.InspParam_Line_FL.Enabled_Cell2 = cbx_Enabled_Cell2_Step4_Line_FL.Checked;
            Recipe.Param.InspParam_Line_FL.W_Cell2 = int.Parse(nud_W_Cell2_Line_FL.Value.ToString());
            Recipe.Param.InspParam_Line_FL.H_Cell2 = int.Parse(nud_H_Cell2_Line_FL.Value.ToString());
            Recipe.Param.InspParam_Line_FL.count_op1 = int.Parse(nud_count_op1_Line_FL.Value.ToString());
            Recipe.Param.InspParam_Line_FL.Enabled_Height = cbx_Enabled_Height_Line_FL.Checked;
            Recipe.Param.InspParam_Line_FL.Enabled_Width = cbx_Enabled_Width_Line_FL.Checked;
            Recipe.Param.InspParam_Line_FL.MinHeight_defect = int.Parse(nud_MinH_df_Line_FL.Value.ToString());
            Recipe.Param.InspParam_Line_FL.MaxHeight_defect = int.Parse(nud_MaxH_df_Line_FL.Value.ToString());
            Recipe.Param.InspParam_Line_FL.MinWidth_defect = int.Parse(nud_MinW_df_Line_FL.Value.ToString());
            Recipe.Param.InspParam_Line_FL.MaxWidth_defect = int.Parse(nud_MaxW_df_Line_FL.Value.ToString());
            Recipe.Param.InspParam_Line_FL.str_op1 = combx_str_op1_Line_FL.Text;
            Recipe.Param.InspParam_Line_FL.Enabled_Height_op1_2 = cbx_Enabled_Height_op1_2_Line_FL.Checked;
            Recipe.Param.InspParam_Line_FL.Enabled_Width_op1_2 = cbx_Enabled_Width_op1_2_Line_FL.Checked;
            Recipe.Param.InspParam_Line_FL.MinHeight_defect_op1_2 = int.Parse(nud_MinH_df_op1_2_Line_FL.Value.ToString());
            Recipe.Param.InspParam_Line_FL.MaxHeight_defect_op1_2 = int.Parse(nud_MaxH_df_op1_2_Line_FL.Value.ToString());
            Recipe.Param.InspParam_Line_FL.MinWidth_defect_op1_2 = int.Parse(nud_MinW_df_op1_2_Line_FL.Value.ToString());
            Recipe.Param.InspParam_Line_FL.MaxWidth_defect_op1_2 = int.Parse(nud_MaxW_df_op1_2_Line_FL.Value.ToString());
            Recipe.Param.InspParam_Line_FL.str_op1_2 = combx_str_op1_2_Line_FL.Text;
            Recipe.Param.InspParam_Line_FL.Enabled_Area = cbx_Enabled_Area_Line_FL.Checked;
            Recipe.Param.InspParam_Line_FL.MinArea_defect = int.Parse(nud_MinA_df_Line_FL.Value.ToString());
            Recipe.Param.InspParam_Line_FL.MaxArea_defect = int.Parse(nud_MaxA_df_Line_FL.Value.ToString());
            Recipe.Param.InspParam_Line_FL.str_op2 = combx_str_op2_Line_FL.Text;

            // 【AI只覆判模稜兩可NG】
            Recipe.Param.InspParam_Line_FL.cls_absolNG.Enabled = cbx_notSureNG_Line_FL.Checked;
            Recipe.Param.InspParam_Line_FL.cls_absolNG.Enabled_Gray_select = cbx_Enabled_Gray_select_Line_FL_AbsolNG.Checked;
            Recipe.Param.InspParam_Line_FL.cls_absolNG.str_Gray_select = combx_str_Gray_select_Line_FL_AbsolNG.Text;
            Recipe.Param.InspParam_Line_FL.cls_absolNG.Gray_select = int.Parse(nud_Gray_select_Line_FL_AbsolNG.Value.ToString());
            Recipe.Param.InspParam_Line_FL.cls_absolNG.str_op = combx_str_op_Line_FL_AbsolNG.Text;
            Recipe.Param.InspParam_Line_FL.cls_absolNG.Enabled_Area = cbx_Enabled_Area_Line_FL_AbsolNG.Checked;
            Recipe.Param.InspParam_Line_FL.cls_absolNG.MinArea_defect = int.Parse(nud_MinA_df_Line_FL_AbsolNG.Value.ToString());
            Recipe.Param.InspParam_Line_FL.cls_absolNG.MaxArea_defect = int.Parse(nud_MaxA_df_Line_FL_AbsolNG.Value.ToString());

            #endregion

            #region 檢測絲狀異物 (正光&同軸光) 之參數

            Recipe.Param.InspParam_Line_FLCL.Enabled = cbx_Line_FLCL.Checked;
            Recipe.Param.InspParam_Line_FLCL.str_op1_2Images = combx_str_op1_2Images_Line_FLCL.Text;
            Recipe.Param.InspParam_Line_FLCL.count_op1 = int.Parse(nud_count_op1_Line_FLCL.Value.ToString());
            Recipe.Param.InspParam_Line_FLCL.Enabled_Height = cbx_Enabled_Height_Line_FLCL.Checked;
            Recipe.Param.InspParam_Line_FLCL.Enabled_Width = cbx_Enabled_Width_Line_FLCL.Checked;
            Recipe.Param.InspParam_Line_FLCL.MinHeight_defect = int.Parse(nud_MinH_df_Line_FLCL.Value.ToString());
            Recipe.Param.InspParam_Line_FLCL.MaxHeight_defect = int.Parse(nud_MaxH_df_Line_FLCL.Value.ToString());
            Recipe.Param.InspParam_Line_FLCL.MinWidth_defect = int.Parse(nud_MinW_df_Line_FLCL.Value.ToString());
            Recipe.Param.InspParam_Line_FLCL.MaxWidth_defect = int.Parse(nud_MaxW_df_Line_FLCL.Value.ToString());
            Recipe.Param.InspParam_Line_FLCL.str_op1 = combx_str_op1_Line_FLCL.Text;
            Recipe.Param.InspParam_Line_FLCL.Enabled_Height_op1_2 = cbx_Enabled_Height_op1_2_Line_FLCL.Checked;
            Recipe.Param.InspParam_Line_FLCL.Enabled_Width_op1_2 = cbx_Enabled_Width_op1_2_Line_FLCL.Checked;
            Recipe.Param.InspParam_Line_FLCL.MinHeight_defect_op1_2 = int.Parse(nud_MinH_df_op1_2_Line_FLCL.Value.ToString());
            Recipe.Param.InspParam_Line_FLCL.MaxHeight_defect_op1_2 = int.Parse(nud_MaxH_df_op1_2_Line_FLCL.Value.ToString());
            Recipe.Param.InspParam_Line_FLCL.MinWidth_defect_op1_2 = int.Parse(nud_MinW_df_op1_2_Line_FLCL.Value.ToString());
            Recipe.Param.InspParam_Line_FLCL.MaxWidth_defect_op1_2 = int.Parse(nud_MaxW_df_op1_2_Line_FLCL.Value.ToString());
            Recipe.Param.InspParam_Line_FLCL.str_op1_2 = combx_str_op1_2_Line_FLCL.Text;
            Recipe.Param.InspParam_Line_FLCL.Enabled_Area = cbx_Enabled_Area_Line_FLCL.Checked;
            Recipe.Param.InspParam_Line_FLCL.MinArea_defect = int.Parse(nud_MinA_df_Line_FLCL.Value.ToString());
            Recipe.Param.InspParam_Line_FLCL.MaxArea_defect = int.Parse(nud_MaxA_df_Line_FLCL.Value.ToString());
            Recipe.Param.InspParam_Line_FLCL.str_op2 = combx_str_op2_Line_FLCL.Text;

            Recipe.Param.InspParam_Line_FLCL.Enabled_BypassReg = cbx_Enabled_BypassReg_Line_FLCL.Checked;
            Recipe.Param.InspParam_Line_FLCL.x_BypassReg = int.Parse(nud_x_BypassReg_Line_FLCL.Value.ToString());
            Recipe.Param.InspParam_Line_FLCL.y_BypassReg = int.Parse(nud_y_BypassReg_Line_FLCL.Value.ToString());
            Recipe.Param.InspParam_Line_FLCL.W_BypassReg = int.Parse(nud_W_BypassReg_Line_FLCL.Value.ToString());
            Recipe.Param.InspParam_Line_FLCL.H_BypassReg = int.Parse(nud_H_BypassReg_Line_FLCL.Value.ToString());
            
            Recipe.Param.InspParam_Line_FLCL.count_op3 = int.Parse(nud_count_op3_Line_FLCL.Value.ToString());
            Recipe.Param.InspParam_Line_FLCL.Enabled_Height_op3_1 = cbx_Enabled_Height_op3_1_Line_FLCL.Checked;
            Recipe.Param.InspParam_Line_FLCL.Enabled_Width_op3_1 = cbx_Enabled_Width_op3_1_Line_FLCL.Checked;
            Recipe.Param.InspParam_Line_FLCL.MinHeight_defect_op3_1 = int.Parse(nud_MinH_df_op3_1_Line_FLCL.Value.ToString());
            Recipe.Param.InspParam_Line_FLCL.MaxHeight_defect_op3_1 = int.Parse(nud_MaxH_df_op3_1_Line_FLCL.Value.ToString());
            Recipe.Param.InspParam_Line_FLCL.MinWidth_defect_op3_1 = int.Parse(nud_MinW_df_op3_1_Line_FLCL.Value.ToString());
            Recipe.Param.InspParam_Line_FLCL.MaxWidth_defect_op3_1 = int.Parse(nud_MaxW_df_op3_1_Line_FLCL.Value.ToString());
            Recipe.Param.InspParam_Line_FLCL.str_op3_1 = combx_str_op3_1_Line_FLCL.Text;
            Recipe.Param.InspParam_Line_FLCL.Enabled_Height_op3_2 = cbx_Enabled_Height_op3_2_Line_FLCL.Checked;
            Recipe.Param.InspParam_Line_FLCL.Enabled_Width_op3_2 = cbx_Enabled_Width_op3_2_Line_FLCL.Checked;
            Recipe.Param.InspParam_Line_FLCL.MinHeight_defect_op3_2 = int.Parse(nud_MinH_df_op3_2_Line_FLCL.Value.ToString());
            Recipe.Param.InspParam_Line_FLCL.MaxHeight_defect_op3_2 = int.Parse(nud_MaxH_df_op3_2_Line_FLCL.Value.ToString());
            Recipe.Param.InspParam_Line_FLCL.MinWidth_defect_op3_2 = int.Parse(nud_MinW_df_op3_2_Line_FLCL.Value.ToString());
            Recipe.Param.InspParam_Line_FLCL.MaxWidth_defect_op3_2 = int.Parse(nud_MaxW_df_op3_2_Line_FLCL.Value.ToString());
            Recipe.Param.InspParam_Line_FLCL.str_op3_2 = combx_str_op3_2_Line_FLCL.Text;

            // 【AI只覆判模稜兩可NG】
            Recipe.Param.InspParam_Line_FLCL.cls_absolNG.Enabled = cbx_notSureNG_Line_FLCL.Checked;
            Recipe.Param.InspParam_Line_FLCL.cls_absolNG.Enabled_Gray_select = cbx_Enabled_Gray_select_Line_FLCL_AbsolNG.Checked;
            Recipe.Param.InspParam_Line_FLCL.cls_absolNG.str_Gray_select = combx_str_Gray_select_Line_FLCL_AbsolNG.Text;
            Recipe.Param.InspParam_Line_FLCL.cls_absolNG.Gray_select = int.Parse(nud_Gray_select_Line_FLCL_AbsolNG.Value.ToString());
            Recipe.Param.InspParam_Line_FLCL.cls_absolNG.str_op = combx_str_op_Line_FLCL_AbsolNG.Text;
            Recipe.Param.InspParam_Line_FLCL.cls_absolNG.Enabled_Area = cbx_Enabled_Area_Line_FLCL_AbsolNG.Checked;
            Recipe.Param.InspParam_Line_FLCL.cls_absolNG.MinArea_defect = int.Parse(nud_MinA_df_Line_FLCL_AbsolNG.Value.ToString());
            Recipe.Param.InspParam_Line_FLCL.cls_absolNG.MaxArea_defect = int.Parse(nud_MaxA_df_Line_FLCL_AbsolNG.Value.ToString());

            #endregion

            #region 檢測白條內黑點 (Black Crack) (Frontal light) 之參數

            Recipe.Param.InspParam_BlackCrack_FL.Enabled = cbx_BlackCrack_FL.Checked;
            Recipe.Param.InspParam_BlackCrack_FL.ImageIndex = int.Parse(nud_ImageID_BlackCrack_FL.Value.ToString());
            Recipe.Param.InspParam_BlackCrack_FL.Enabled_Wsmall = cbx_Enabled_Wsmall_BlackCrack_FL.Checked;
            Recipe.Param.InspParam_BlackCrack_FL.str_L_NonInspect = combx_str_L_NonInspect_BlackCrack_FL.Text;
            Recipe.Param.InspParam_BlackCrack_FL.Enabled_BlackStripe = cbx_Enabled_BlackStripe_Step1_BlackCrack_FL.Checked;
            Recipe.Param.InspParam_BlackCrack_FL.str_ModeBlackStripe = combx_str_ModeBlackStripe_BlackCrack_FL.Text;
            Recipe.Param.InspParam_BlackCrack_FL.H_BlackStripe = int.Parse(nud_H_BlackStripe_BlackCrack_FL.Value.ToString());
            Recipe.Param.InspParam_BlackCrack_FL.H_BlackStripe_mode2 = int.Parse(nud_H_BlackStripe_mode2_BlackCrack_FL.Value.ToString());
            Recipe.Param.InspParam_BlackCrack_FL.MinGray_BlackCrack = int.Parse(nud_MinGray_Resist_BlackCrack_FL.Value.ToString());
            Recipe.Param.InspParam_BlackCrack_FL.MaxGray_BlackCrack = int.Parse(nud_MaxGray_Resist_BlackCrack_FL.Value.ToString());
            Recipe.Param.InspParam_BlackCrack_FL.Enabled_Cell = cbx_Enabled_Cell_Step3_BlackCrack_FL.Checked;
            Recipe.Param.InspParam_BlackCrack_FL.W_Cell = int.Parse(nud_W_Cell_BlackCrack_FL.Value.ToString());
            Recipe.Param.InspParam_BlackCrack_FL.H_Cell = int.Parse(nud_H_Cell_BlackCrack_FL.Value.ToString());
            Recipe.Param.InspParam_BlackCrack_FL.str_segMode = combx_str_segMode_BlackCrack_FL.Text; // (20190529) Jeff Revised!
            Recipe.Param.InspParam_BlackCrack_FL.Enabled_Height = cbx_Enabled_Height_BlackCrack_FL.Checked;
            Recipe.Param.InspParam_BlackCrack_FL.Enabled_Width = cbx_Enabled_Width_BlackCrack_FL.Checked;
            Recipe.Param.InspParam_BlackCrack_FL.MinHeight_defect = int.Parse(nud_MinH_df_BlackCrack_FL.Value.ToString());
            Recipe.Param.InspParam_BlackCrack_FL.MaxHeight_defect = int.Parse(nud_MaxH_df_BlackCrack_FL.Value.ToString());
            Recipe.Param.InspParam_BlackCrack_FL.MinWidth_defect = int.Parse(nud_MinW_df_BlackCrack_FL.Value.ToString());
            Recipe.Param.InspParam_BlackCrack_FL.MaxWidth_defect = int.Parse(nud_MaxW_df_BlackCrack_FL.Value.ToString());
            Recipe.Param.InspParam_BlackCrack_FL.str_op1 = combx_str_op1_BlackCrack_FL.Text;
            Recipe.Param.InspParam_BlackCrack_FL.Enabled_Area = cbx_Enabled_Area_BlackCrack_FL.Checked;
            Recipe.Param.InspParam_BlackCrack_FL.MinArea_defect = int.Parse(nud_MinA_df_BlackCrack_FL.Value.ToString());
            Recipe.Param.InspParam_BlackCrack_FL.MaxArea_defect = int.Parse(nud_MaxA_df_BlackCrack_FL.Value.ToString());
            Recipe.Param.InspParam_BlackCrack_FL.str_op2 = combx_str_op2_BlackCrack_FL.Text;

            // 【AI只覆判模稜兩可NG】
            Recipe.Param.InspParam_BlackCrack_FL.cls_absolNG.Enabled = cbx_notSureNG_BlackCrack_FL.Checked;
            Recipe.Param.InspParam_BlackCrack_FL.cls_absolNG.Enabled_Gray_select = cbx_Enabled_Gray_select_BlackCrack_FL_AbsolNG.Checked;
            Recipe.Param.InspParam_BlackCrack_FL.cls_absolNG.str_Gray_select = combx_str_Gray_select_BlackCrack_FL_AbsolNG.Text;
            Recipe.Param.InspParam_BlackCrack_FL.cls_absolNG.Gray_select = int.Parse(nud_Gray_select_BlackCrack_FL_AbsolNG.Value.ToString());
            Recipe.Param.InspParam_BlackCrack_FL.cls_absolNG.str_op = combx_str_op_BlackCrack_FL_AbsolNG.Text;
            Recipe.Param.InspParam_BlackCrack_FL.cls_absolNG.Enabled_Area = cbx_Enabled_Area_BlackCrack_FL_AbsolNG.Checked;
            Recipe.Param.InspParam_BlackCrack_FL.cls_absolNG.MinArea_defect = int.Parse(nud_MinA_df_BlackCrack_FL_AbsolNG.Value.ToString());
            Recipe.Param.InspParam_BlackCrack_FL.cls_absolNG.MaxArea_defect = int.Parse(nud_MaxA_df_BlackCrack_FL_AbsolNG.Value.ToString());

            #endregion

            #region 檢測白條內黑點 (Black Crack) (Coaxial light) 之參數

            Recipe.Param.InspParam_BlackCrack_CL.Enabled = cbx_BlackCrack_CL.Checked;
            Recipe.Param.InspParam_BlackCrack_CL.ImageIndex = int.Parse(nud_ImageID_BlackCrack_CL.Value.ToString());
            Recipe.Param.InspParam_BlackCrack_CL.Enabled_Wsmall = cbx_Enabled_Wsmall_BlackCrack_CL.Checked;
            Recipe.Param.InspParam_BlackCrack_CL.str_L_NonInspect = combx_str_L_NonInspect_BlackCrack_CL.Text;
            Recipe.Param.InspParam_BlackCrack_CL.Enabled_BlackStripe = cbx_Enabled_BlackStripe_Step1_BlackCrack_CL.Checked;
            Recipe.Param.InspParam_BlackCrack_CL.str_ModeBlackStripe = combx_str_ModeBlackStripe_BlackCrack_CL.Text;
            Recipe.Param.InspParam_BlackCrack_CL.H_BlackStripe = int.Parse(nud_H_BlackStripe_BlackCrack_CL.Value.ToString());
            Recipe.Param.InspParam_BlackCrack_CL.H_BlackStripe_mode2 = int.Parse(nud_H_BlackStripe_mode2_BlackCrack_CL.Value.ToString());
            Recipe.Param.InspParam_BlackCrack_CL.MinGray_BlackCrack = int.Parse(nud_MinGray_Resist_BlackCrack_CL.Value.ToString());
            Recipe.Param.InspParam_BlackCrack_CL.MaxGray_BlackCrack = int.Parse(nud_MaxGray_Resist_BlackCrack_CL.Value.ToString());
            Recipe.Param.InspParam_BlackCrack_CL.Enabled_Cell = cbx_Enabled_Cell_Step3_BlackCrack_CL.Checked;
            Recipe.Param.InspParam_BlackCrack_CL.W_Cell = int.Parse(nud_W_Cell_BlackCrack_CL.Value.ToString());
            Recipe.Param.InspParam_BlackCrack_CL.H_Cell = int.Parse(nud_H_Cell_BlackCrack_CL.Value.ToString());
            Recipe.Param.InspParam_BlackCrack_CL.str_segMode = combx_str_segMode_BlackCrack_CL.Text; // (20190529) Jeff Revised!
            Recipe.Param.InspParam_BlackCrack_CL.Enabled_Height = cbx_Enabled_Height_BlackCrack_CL.Checked;
            Recipe.Param.InspParam_BlackCrack_CL.Enabled_Width = cbx_Enabled_Width_BlackCrack_CL.Checked;
            Recipe.Param.InspParam_BlackCrack_CL.MinHeight_defect = int.Parse(nud_MinH_df_BlackCrack_CL.Value.ToString());
            Recipe.Param.InspParam_BlackCrack_CL.MaxHeight_defect = int.Parse(nud_MaxH_df_BlackCrack_CL.Value.ToString());
            Recipe.Param.InspParam_BlackCrack_CL.MinWidth_defect = int.Parse(nud_MinW_df_BlackCrack_CL.Value.ToString());
            Recipe.Param.InspParam_BlackCrack_CL.MaxWidth_defect = int.Parse(nud_MaxW_df_BlackCrack_CL.Value.ToString());
            Recipe.Param.InspParam_BlackCrack_CL.str_op1 = combx_str_op1_BlackCrack_CL.Text;
            Recipe.Param.InspParam_BlackCrack_CL.Enabled_Area = cbx_Enabled_Area_BlackCrack_CL.Checked;
            Recipe.Param.InspParam_BlackCrack_CL.MinArea_defect = int.Parse(nud_MinA_df_BlackCrack_CL.Value.ToString());
            Recipe.Param.InspParam_BlackCrack_CL.MaxArea_defect = int.Parse(nud_MaxA_df_BlackCrack_CL.Value.ToString());
            Recipe.Param.InspParam_BlackCrack_CL.str_op2 = combx_str_op2_BlackCrack_CL.Text;

            // 【AI只覆判模稜兩可NG】
            Recipe.Param.InspParam_BlackCrack_CL.cls_absolNG.Enabled = cbx_notSureNG_BlackCrack_CL.Checked;
            Recipe.Param.InspParam_BlackCrack_CL.cls_absolNG.Enabled_Gray_select = cbx_Enabled_Gray_select_BlackCrack_CL_AbsolNG.Checked;
            Recipe.Param.InspParam_BlackCrack_CL.cls_absolNG.str_Gray_select = combx_str_Gray_select_BlackCrack_CL_AbsolNG.Text;
            Recipe.Param.InspParam_BlackCrack_CL.cls_absolNG.Gray_select = int.Parse(nud_Gray_select_BlackCrack_CL_AbsolNG.Value.ToString());
            Recipe.Param.InspParam_BlackCrack_CL.cls_absolNG.str_op = combx_str_op_BlackCrack_CL_AbsolNG.Text;
            Recipe.Param.InspParam_BlackCrack_CL.cls_absolNG.Enabled_Area = cbx_Enabled_Area_BlackCrack_CL_AbsolNG.Checked;
            Recipe.Param.InspParam_BlackCrack_CL.cls_absolNG.MinArea_defect = int.Parse(nud_MinA_df_BlackCrack_CL_AbsolNG.Value.ToString());
            Recipe.Param.InspParam_BlackCrack_CL.cls_absolNG.MaxArea_defect = int.Parse(nud_MaxA_df_BlackCrack_CL_AbsolNG.Value.ToString());

            #endregion

            #region 檢測 Black Blob (Coaxial Light) 之參數

            Recipe.Param.InspParam_BlackBlob_CL.Enabled = cbx_BlackBlob_CL.Checked;
            Recipe.Param.InspParam_BlackBlob_CL.ImageIndex = int.Parse(nud_ImageID_BlackBlob_CL.Value.ToString());
            Recipe.Param.InspParam_BlackBlob_CL.MinHeight_defect = double.Parse(txb_MinH_df_BlackBlob_CL.Text);
            Recipe.Param.InspParam_BlackBlob_CL.MaxHeight_defect = double.Parse(txb_MaxH_df_BlackBlob_CL.Text);
            Recipe.Param.InspParam_BlackBlob_CL.MinWidth_defect = double.Parse(txb_MinW_df_BlackBlob_CL.Text);
            Recipe.Param.InspParam_BlackBlob_CL.MaxWidth_defect = double.Parse(txb_MaxW_df_BlackBlob_CL.Text);

            // 【AI只覆判模稜兩可NG】
            Recipe.Param.InspParam_BlackBlob_CL.cls_absolNG.Enabled = cbx_notSureNG_BlackBlob_CL.Checked;
            Recipe.Param.InspParam_BlackBlob_CL.cls_absolNG.Enabled_Gray_select = cbx_Enabled_Gray_select_BlackBlob_CL_AbsolNG.Checked;
            Recipe.Param.InspParam_BlackBlob_CL.cls_absolNG.str_Gray_select = combx_str_Gray_select_BlackBlob_CL_AbsolNG.Text;
            Recipe.Param.InspParam_BlackBlob_CL.cls_absolNG.Gray_select = int.Parse(nud_Gray_select_BlackBlob_CL_AbsolNG.Value.ToString());
            Recipe.Param.InspParam_BlackBlob_CL.cls_absolNG.str_op = combx_str_op_BlackBlob_CL_AbsolNG.Text;
            Recipe.Param.InspParam_BlackBlob_CL.cls_absolNG.Enabled_Area = cbx_Enabled_Area_BlackBlob_CL_AbsolNG.Checked;
            Recipe.Param.InspParam_BlackBlob_CL.cls_absolNG.MinArea_defect = int.Parse(nud_MinA_df_BlackBlob_CL_AbsolNG.Value.ToString());
            Recipe.Param.InspParam_BlackBlob_CL.cls_absolNG.MaxArea_defect = int.Parse(nud_MaxA_df_BlackBlob_CL_AbsolNG.Value.ToString());

            #endregion

            #region 檢測 Black Blob2 (Coaxial Light) 之參數

            Recipe.Param.InspParam_BlackBlob2_CL.Enabled = cbx_BlackBlob2_CL.Checked;
            Recipe.Param.InspParam_BlackBlob2_CL.ImageIndex = int.Parse(nud_ImageID_BlackBlob2_CL.Value.ToString());
            Recipe.Param.InspParam_BlackBlob2_CL.Enabled_Wsmall = cbx_Enabled_Wsmall_BlackBlob2_CL.Checked;
            Recipe.Param.InspParam_BlackBlob2_CL.str_L_NonInspect = combx_str_L_NonInspect_BlackBlob2_CL.Text;
            Recipe.Param.InspParam_BlackBlob2_CL.Enabled_Cell = cbx_Enabled_Cell_Step1_BlackBlob2_CL.Checked;
            Recipe.Param.InspParam_BlackBlob2_CL.Enabled_Cell = cbx_Enabled_Cell_Step2_BlackBlob2_CL.Checked;
            Recipe.Param.InspParam_BlackBlob2_CL.W_Cell = int.Parse(nud_W_Cell_BlackBlob2_CL.Value.ToString());
            Recipe.Param.InspParam_BlackBlob2_CL.H_Cell = int.Parse(nud_H_Cell_BlackBlob2_CL.Value.ToString());
            Recipe.Param.InspParam_BlackBlob2_CL.Enabled_BypassReg = cbx_Enabled_BypassReg_BlackBlob2_CL.Checked;
            Recipe.Param.InspParam_BlackBlob2_CL.x_BypassReg = int.Parse(nud_x_BypassReg_BlackBlob2_CL.Value.ToString());
            Recipe.Param.InspParam_BlackBlob2_CL.y_BypassReg = int.Parse(nud_y_BypassReg_BlackBlob2_CL.Value.ToString());
            Recipe.Param.InspParam_BlackBlob2_CL.W_BypassReg = int.Parse(nud_W_BypassReg_BlackBlob2_CL.Value.ToString());
            Recipe.Param.InspParam_BlackBlob2_CL.H_BypassReg = int.Parse(nud_H_BypassReg_BlackBlob2_CL.Value.ToString());
            Recipe.Param.InspParam_BlackBlob2_CL.Enabled_BlackStripe = cbx_Enabled_BlackStripe_Step1_BlackBlob2_CL.Checked;
            Recipe.Param.InspParam_BlackBlob2_CL.Enabled_BlackStripe = cbx_Enabled_BlackStripe_Step2_BlackBlob2_CL.Checked;
            Recipe.Param.InspParam_BlackBlob2_CL.str_ModeBlackStripe = combx_str_ModeBlackStripe_BlackBlob2_CL.Text;
            Recipe.Param.InspParam_BlackBlob2_CL.H_BlackStripe = int.Parse(nud_H_BlackStripe_BlackBlob2_CL.Value.ToString());
            Recipe.Param.InspParam_BlackBlob2_CL.H_BlackStripe_mode2 = int.Parse(nud_H_BlackStripe_mode2_BlackBlob2_CL.Value.ToString());
            Recipe.Param.InspParam_BlackBlob2_CL.str_BinaryImg_Cell = combx_str_BinaryImg_Cell_BlackBlob2_CL.Text;
            Recipe.Param.InspParam_BlackBlob2_CL.MaskWidth_Cell = int.Parse(nud_MaskWidth_1_Cell_BlackBlob2_CL.Value.ToString());
            Recipe.Param.InspParam_BlackBlob2_CL.MaskHeight_Cell = int.Parse(nud_MaskHeight_1_Cell_BlackBlob2_CL.Value.ToString());
            Recipe.Param.InspParam_BlackBlob2_CL.str_LightDark_Cell = combx_str_LightDark_1_Cell_BlackBlob2_CL.Text;
            Recipe.Param.InspParam_BlackBlob2_CL.Offset_Cell = int.Parse(nud_Offset_1_Cell_BlackBlob2_CL.Value.ToString());
            Recipe.Param.InspParam_BlackBlob2_CL.MinGray_Cell = int.Parse(nud_MinGray_1_Cell_BlackBlob2_CL.Value.ToString());
            Recipe.Param.InspParam_BlackBlob2_CL.MaxGray_Cell = int.Parse(nud_MaxGray_1_Cell_BlackBlob2_CL.Value.ToString());
            Recipe.Param.InspParam_BlackBlob2_CL.str_BinaryImg_Resist = combx_str_BinaryImg_Resist_BlackBlob2_CL.Text;
            Recipe.Param.InspParam_BlackBlob2_CL.MaskWidth_Resist = int.Parse(nud_MaskWidth_1_Resist_BlackBlob2_CL.Value.ToString());
            Recipe.Param.InspParam_BlackBlob2_CL.MaskHeight_Resist = int.Parse(nud_MaskHeight_1_Resist_BlackBlob2_CL.Value.ToString());
            Recipe.Param.InspParam_BlackBlob2_CL.str_LightDark_Resist = combx_str_LightDark_1_Resist_BlackBlob2_CL.Text;
            Recipe.Param.InspParam_BlackBlob2_CL.Offset_Resist = int.Parse(nud_Offset_1_Resist_BlackBlob2_CL.Value.ToString());
            Recipe.Param.InspParam_BlackBlob2_CL.MinGray_Resist = int.Parse(nud_MinGray_1_Resist_BlackBlob2_CL.Value.ToString());
            Recipe.Param.InspParam_BlackBlob2_CL.MaxGray_Resist = int.Parse(nud_MaxGray_1_Resist_BlackBlob2_CL.Value.ToString());

            // 規格2 (20190411) Jeff Revised!
            Recipe.Param.InspParam_BlackBlob2_CL.count_BinImg_Cell = int.Parse(nud_count_BinImg_Cell_BlackBlob2_CL.Value.ToString());
            Recipe.Param.InspParam_BlackBlob2_CL.MinGray_2_Cell = int.Parse(nud_MinGray_2_Cell_BlackBlob2_CL.Value.ToString());
            Recipe.Param.InspParam_BlackBlob2_CL.MaxGray_2_Cell = int.Parse(nud_MaxGray_2_Cell_BlackBlob2_CL.Value.ToString());
            Recipe.Param.InspParam_BlackBlob2_CL.MaskWidth_2_Cell = int.Parse(nud_MaskWidth_2_Cell_BlackBlob2_CL.Value.ToString());
            Recipe.Param.InspParam_BlackBlob2_CL.MaskHeight_2_Cell = int.Parse(nud_MaskHeight_2_Cell_BlackBlob2_CL.Value.ToString());
            Recipe.Param.InspParam_BlackBlob2_CL.str_LightDark_2_Cell = combx_str_LightDark_2_Cell_BlackBlob2_CL.Text;
            Recipe.Param.InspParam_BlackBlob2_CL.Offset_2_Cell = int.Parse(nud_Offset_2_Cell_BlackBlob2_CL.Value.ToString());
            Recipe.Param.InspParam_BlackBlob2_CL.count_BinImg_Resist = int.Parse(nud_count_BinImg_Resist_BlackBlob2_CL.Value.ToString());
            Recipe.Param.InspParam_BlackBlob2_CL.MinGray_2_Resist = int.Parse(nud_MinGray_2_Resist_BlackBlob2_CL.Value.ToString());
            Recipe.Param.InspParam_BlackBlob2_CL.MaxGray_2_Resist = int.Parse(nud_MaxGray_2_Resist_BlackBlob2_CL.Value.ToString());
            Recipe.Param.InspParam_BlackBlob2_CL.MaskWidth_2_Resist = int.Parse(nud_MaskWidth_2_Resist_BlackBlob2_CL.Value.ToString());
            Recipe.Param.InspParam_BlackBlob2_CL.MaskHeight_2_Resist = int.Parse(nud_MaskHeight_2_Resist_BlackBlob2_CL.Value.ToString());
            Recipe.Param.InspParam_BlackBlob2_CL.str_LightDark_2_Resist = combx_str_LightDark_2_Resist_BlackBlob2_CL.Text;
            Recipe.Param.InspParam_BlackBlob2_CL.Offset_2_Resist = int.Parse(nud_Offset_2_Resist_BlackBlob2_CL.Value.ToString());

            Recipe.Param.InspParam_BlackBlob2_CL.Enabled_Gray_select_Cell = cbx_Enabled_Gray_select_Cell_BlackBlob2_CL.Checked;
            Recipe.Param.InspParam_BlackBlob2_CL.str_Gray_select_Cell = combx_str_Gray_select_Cell_BlackBlob2_CL.Text;
            Recipe.Param.InspParam_BlackBlob2_CL.Gray_select_Cell = int.Parse(nud_Gray_select_Cell_BlackBlob2_CL.Value.ToString());
            Recipe.Param.InspParam_BlackBlob2_CL.Enabled_Gray_select_Resist = cbx_Enabled_Gray_select_Resist_BlackBlob2_CL.Checked;
            Recipe.Param.InspParam_BlackBlob2_CL.str_Gray_select_Resist = combx_str_Gray_select_Resist_BlackBlob2_CL.Text;
            Recipe.Param.InspParam_BlackBlob2_CL.Gray_select_Resist = int.Parse(nud_Gray_select_Resist_BlackBlob2_CL.Value.ToString());
            Recipe.Param.InspParam_BlackBlob2_CL.Enabled_Cell2 = cbx_Enabled_Cell2_Step4_BlackBlob2_CL.Checked;
            Recipe.Param.InspParam_BlackBlob2_CL.W_Cell2 = int.Parse(nud_W_Cell2_BlackBlob2_CL.Value.ToString());
            Recipe.Param.InspParam_BlackBlob2_CL.H_Cell2 = int.Parse(nud_H_Cell2_BlackBlob2_CL.Value.ToString());
            Recipe.Param.InspParam_BlackBlob2_CL.count_op1 = int.Parse(nud_count_op1_BlackBlob2_CL.Value.ToString());
            Recipe.Param.InspParam_BlackBlob2_CL.Enabled_Height = cbx_Enabled_Height_BlackBlob2_CL.Checked;
            Recipe.Param.InspParam_BlackBlob2_CL.Enabled_Width = cbx_Enabled_Width_BlackBlob2_CL.Checked;
            Recipe.Param.InspParam_BlackBlob2_CL.MinHeight_defect = int.Parse(nud_MinH_df_BlackBlob2_CL.Value.ToString());
            Recipe.Param.InspParam_BlackBlob2_CL.MaxHeight_defect = int.Parse(nud_MaxH_df_BlackBlob2_CL.Value.ToString());
            Recipe.Param.InspParam_BlackBlob2_CL.MinWidth_defect = int.Parse(nud_MinW_df_BlackBlob2_CL.Value.ToString());
            Recipe.Param.InspParam_BlackBlob2_CL.MaxWidth_defect = int.Parse(nud_MaxW_df_BlackBlob2_CL.Value.ToString());
            Recipe.Param.InspParam_BlackBlob2_CL.str_op1 = combx_str_op1_BlackBlob2_CL.Text;
            Recipe.Param.InspParam_BlackBlob2_CL.Enabled_Height_op1_2 = cbx_Enabled_Height_op1_2_BlackBlob2_CL.Checked;
            Recipe.Param.InspParam_BlackBlob2_CL.Enabled_Width_op1_2 = cbx_Enabled_Width_op1_2_BlackBlob2_CL.Checked;
            Recipe.Param.InspParam_BlackBlob2_CL.MinHeight_defect_op1_2 = int.Parse(nud_MinH_df_op1_2_BlackBlob2_CL.Value.ToString());
            Recipe.Param.InspParam_BlackBlob2_CL.MaxHeight_defect_op1_2 = int.Parse(nud_MaxH_df_op1_2_BlackBlob2_CL.Value.ToString());
            Recipe.Param.InspParam_BlackBlob2_CL.MinWidth_defect_op1_2 = int.Parse(nud_MinW_df_op1_2_BlackBlob2_CL.Value.ToString());
            Recipe.Param.InspParam_BlackBlob2_CL.MaxWidth_defect_op1_2 = int.Parse(nud_MaxW_df_op1_2_BlackBlob2_CL.Value.ToString());
            Recipe.Param.InspParam_BlackBlob2_CL.str_op1_2 = combx_str_op1_2_BlackBlob2_CL.Text;
            Recipe.Param.InspParam_BlackBlob2_CL.Enabled_Area = cbx_Enabled_Area_BlackBlob2_CL.Checked;
            Recipe.Param.InspParam_BlackBlob2_CL.MinArea_defect = int.Parse(nud_MinA_df_BlackBlob2_CL.Value.ToString());
            Recipe.Param.InspParam_BlackBlob2_CL.MaxArea_defect = int.Parse(nud_MaxA_df_BlackBlob2_CL.Value.ToString());
            Recipe.Param.InspParam_BlackBlob2_CL.str_op2 = combx_str_op2_BlackBlob2_CL.Text;

            // 【AI只覆判模稜兩可NG】
            Recipe.Param.InspParam_BlackBlob2_CL.cls_absolNG.Enabled = cbx_notSureNG_BlackBlob2_CL.Checked;
            Recipe.Param.InspParam_BlackBlob2_CL.cls_absolNG.Enabled_Gray_select = cbx_Enabled_Gray_select_BlackBlob2_CL_AbsolNG.Checked;
            Recipe.Param.InspParam_BlackBlob2_CL.cls_absolNG.str_Gray_select = combx_str_Gray_select_BlackBlob2_CL_AbsolNG.Text;
            Recipe.Param.InspParam_BlackBlob2_CL.cls_absolNG.Gray_select = int.Parse(nud_Gray_select_BlackBlob2_CL_AbsolNG.Value.ToString());
            Recipe.Param.InspParam_BlackBlob2_CL.cls_absolNG.str_op = combx_str_op_BlackBlob2_CL_AbsolNG.Text;
            Recipe.Param.InspParam_BlackBlob2_CL.cls_absolNG.Enabled_Area = cbx_Enabled_Area_BlackBlob2_CL_AbsolNG.Checked;
            Recipe.Param.InspParam_BlackBlob2_CL.cls_absolNG.MinArea_defect = int.Parse(nud_MinA_df_BlackBlob2_CL_AbsolNG.Value.ToString());
            Recipe.Param.InspParam_BlackBlob2_CL.cls_absolNG.MaxArea_defect = int.Parse(nud_MaxA_df_BlackBlob2_CL_AbsolNG.Value.ToString());

            #endregion

            #region 檢測汙染 (Coaxial Light) 之參數

            Recipe.Param.InspParam_Dirty_CL.Enabled = cbx_Dirty_CL.Checked;
            Recipe.Param.InspParam_Dirty_CL.ImageIndex = int.Parse(nud_ImageID_Dirty_CL.Value.ToString());
            Recipe.Param.InspParam_Dirty_CL.Enabled_Cell = cbx_Enabled_Cell_Step1_Dirty_CL.Checked;
            Recipe.Param.InspParam_Dirty_CL.Enabled_Cell = cbx_Enabled_Cell_Step2_Dirty_CL.Checked;
            Recipe.Param.InspParam_Dirty_CL.W_Cell = int.Parse(nud_W_Cell_Dirty_CL.Value.ToString());
            Recipe.Param.InspParam_Dirty_CL.H_Cell = int.Parse(nud_H_Cell_Dirty_CL.Value.ToString());
            Recipe.Param.InspParam_Dirty_CL.Enabled_BypassReg = cbx_Enabled_BypassReg_Dirty_CL.Checked;
            Recipe.Param.InspParam_Dirty_CL.x_BypassReg = int.Parse(nud_x_BypassReg_Dirty_CL.Value.ToString());
            Recipe.Param.InspParam_Dirty_CL.y_BypassReg = int.Parse(nud_y_BypassReg_Dirty_CL.Value.ToString());
            Recipe.Param.InspParam_Dirty_CL.W_BypassReg = int.Parse(nud_W_BypassReg_Dirty_CL.Value.ToString());
            Recipe.Param.InspParam_Dirty_CL.H_BypassReg = int.Parse(nud_H_BypassReg_Dirty_CL.Value.ToString());
            Recipe.Param.InspParam_Dirty_CL.Enabled_BlackStripe = cbx_Enabled_BlackStripe_Step1_Dirty_CL.Checked;
            Recipe.Param.InspParam_Dirty_CL.Enabled_BlackStripe = cbx_Enabled_BlackStripe_Step2_Dirty_CL.Checked;
            Recipe.Param.InspParam_Dirty_CL.str_ModeBlackStripe = combx_str_ModeBlackStripe_Dirty_CL.Text;
            Recipe.Param.InspParam_Dirty_CL.H_BlackStripe = int.Parse(nud_H_BlackStripe_Dirty_CL.Value.ToString());
            Recipe.Param.InspParam_Dirty_CL.H_BlackStripe_mode2 = int.Parse(nud_H_BlackStripe_mode2_Dirty_CL.Value.ToString());
            Recipe.Param.InspParam_Dirty_CL.MinGray_Cell = int.Parse(nud_MinGray_Cell_Dirty_CL.Value.ToString());
            Recipe.Param.InspParam_Dirty_CL.MaxGray_Cell = int.Parse(nud_MaxGray_Cell_Dirty_CL.Value.ToString());
            Recipe.Param.InspParam_Dirty_CL.MinGray_Resist = int.Parse(nud_MinGray_Resist_Dirty_CL.Value.ToString());
            Recipe.Param.InspParam_Dirty_CL.MaxGray_Resist = int.Parse(nud_MaxGray_Resist_Dirty_CL.Value.ToString());
            Recipe.Param.InspParam_Dirty_CL.Enabled_Gray_select = cbx_Enabled_Gray_select_Dirty_CL.Checked;
            Recipe.Param.InspParam_Dirty_CL.str_Gray_select = combx_str_Gray_select_Dirty_CL.Text;
            Recipe.Param.InspParam_Dirty_CL.Gray_select = int.Parse(nud_Gray_select_Dirty_CL.Value.ToString());
            Recipe.Param.InspParam_Dirty_CL.Enabled_Height = cbx_Enabled_Height_Dirty_CL.Checked;
            Recipe.Param.InspParam_Dirty_CL.Enabled_Width = cbx_Enabled_Width_Dirty_CL.Checked;
            Recipe.Param.InspParam_Dirty_CL.MinHeight_defect = int.Parse(nud_MinH_df_Dirty_CL.Value.ToString());
            Recipe.Param.InspParam_Dirty_CL.MaxHeight_defect = int.Parse(nud_MaxH_df_Dirty_CL.Value.ToString());
            Recipe.Param.InspParam_Dirty_CL.MinWidth_defect = int.Parse(nud_MinW_df_Dirty_CL.Value.ToString());
            Recipe.Param.InspParam_Dirty_CL.MaxWidth_defect = int.Parse(nud_MaxW_df_Dirty_CL.Value.ToString());
            Recipe.Param.InspParam_Dirty_CL.str_op1 = combx_str_op1_Dirty_CL.Text;
            Recipe.Param.InspParam_Dirty_CL.Enabled_Area = cbx_Enabled_Area_Dirty_CL.Checked;
            Recipe.Param.InspParam_Dirty_CL.MinArea_defect = int.Parse(nud_MinA_df_Dirty_CL.Value.ToString());
            Recipe.Param.InspParam_Dirty_CL.MaxArea_defect = int.Parse(nud_MaxA_df_Dirty_CL.Value.ToString());
            Recipe.Param.InspParam_Dirty_CL.str_op2 = combx_str_op2_Dirty_CL.Text;

            // 【AI只覆判模稜兩可NG】
            Recipe.Param.InspParam_Dirty_CL.cls_absolNG.Enabled = cbx_notSureNG_Dirty_CL.Checked;
            Recipe.Param.InspParam_Dirty_CL.cls_absolNG.Enabled_Gray_select = cbx_Enabled_Gray_select_Dirty_CL_AbsolNG.Checked;
            Recipe.Param.InspParam_Dirty_CL.cls_absolNG.str_Gray_select = combx_str_Gray_select_Dirty_CL_AbsolNG.Text;
            Recipe.Param.InspParam_Dirty_CL.cls_absolNG.Gray_select = int.Parse(nud_Gray_select_Dirty_CL_AbsolNG.Value.ToString());
            Recipe.Param.InspParam_Dirty_CL.cls_absolNG.str_op = combx_str_op_Dirty_CL_AbsolNG.Text;
            Recipe.Param.InspParam_Dirty_CL.cls_absolNG.Enabled_Area = cbx_Enabled_Area_Dirty_CL_AbsolNG.Checked;
            Recipe.Param.InspParam_Dirty_CL.cls_absolNG.MinArea_defect = int.Parse(nud_MinA_df_Dirty_CL_AbsolNG.Value.ToString());
            Recipe.Param.InspParam_Dirty_CL.cls_absolNG.MaxArea_defect = int.Parse(nud_MaxA_df_Dirty_CL_AbsolNG.Value.ToString());

            #endregion

            #region 檢測汙染 (Dirty) (Frontal light & Coaxial Light) 之參數

            Recipe.Param.InspParam_Dirty_FLCL.Enabled = cbx_Dirty_FLCL.Checked;
            Recipe.Param.InspParam_Dirty_FLCL.ImageIndex1 = int.Parse(nud_ImageID1_Dirty_FLCL.Value.ToString());
            Recipe.Param.InspParam_Dirty_FLCL.ImageIndex2 = int.Parse(nud_ImageID2_Dirty_FLCL.Value.ToString());
            Recipe.Param.InspParam_Dirty_FLCL.MaxGray_Dirty_FL = int.Parse(nud_MaxGray_Dirty_FL.Value.ToString());
            Recipe.Param.InspParam_Dirty_FLCL.Gray_Dark_FL = int.Parse(nud_Gray_Dark_FL.Value.ToString());
            Recipe.Param.InspParam_Dirty_FLCL.MinGray_Dirty_CL = int.Parse(nud_MinGray_Dirty_CL.Value.ToString());
            Recipe.Param.InspParam_Dirty_FLCL.Gray_Bright_CL = int.Parse(nud_Gray_Bright_CL.Value.ToString());
            Recipe.Param.InspParam_Dirty_FLCL.MinHeight_defect = double.Parse(txb_MinH_df_Dirty_FLCL.Text);
            Recipe.Param.InspParam_Dirty_FLCL.MaxHeight_defect = double.Parse(txb_MaxH_df_Dirty_FLCL.Text);
            Recipe.Param.InspParam_Dirty_FLCL.MinWidth_defect = double.Parse(txb_MinW_df_Dirty_FLCL.Text);
            Recipe.Param.InspParam_Dirty_FLCL.MaxWidth_defect = double.Parse(txb_MaxW_df_Dirty_FLCL.Text);

            // 【AI只覆判模稜兩可NG】
            Recipe.Param.InspParam_Dirty_FLCL.cls_absolNG.Enabled = cbx_notSureNG_Dirty_FLCL.Checked;
            Recipe.Param.InspParam_Dirty_FLCL.cls_absolNG.Enabled_Gray_select = cbx_Enabled_Gray_select_Dirty_FLCL_AbsolNG.Checked;
            Recipe.Param.InspParam_Dirty_FLCL.cls_absolNG.str_Gray_select = combx_str_Gray_select_Dirty_FLCL_AbsolNG.Text;
            Recipe.Param.InspParam_Dirty_FLCL.cls_absolNG.Gray_select = int.Parse(nud_Gray_select_Dirty_FLCL_AbsolNG.Value.ToString());
            Recipe.Param.InspParam_Dirty_FLCL.cls_absolNG.str_op = combx_str_op_Dirty_FLCL_AbsolNG.Text;
            Recipe.Param.InspParam_Dirty_FLCL.cls_absolNG.Enabled_Area = cbx_Enabled_Area_Dirty_FLCL_AbsolNG.Checked;
            Recipe.Param.InspParam_Dirty_FLCL.cls_absolNG.MinArea_defect = int.Parse(nud_MinA_df_Dirty_FLCL_AbsolNG.Value.ToString());
            Recipe.Param.InspParam_Dirty_FLCL.cls_absolNG.MaxArea_defect = int.Parse(nud_MaxA_df_Dirty_FLCL_AbsolNG.Value.ToString());

            #endregion

            #region 檢測電極區白點 (Back Light) 之參數

            Recipe.Param.InspParam_BrightBlob_BL.Enabled = cbx_BrightBlob_BL.Checked;
            Recipe.Param.InspParam_BrightBlob_BL.ImageIndex = int.Parse(nud_ImageID_BrightBlob_BL.Value.ToString());
            Recipe.Param.InspParam_BrightBlob_BL.MinGray_Blob_Elect_BL = int.Parse(nud_MinGray_Blob_Elect_BL.Value.ToString());
            Recipe.Param.InspParam_BrightBlob_BL.MinHeight_defect = int.Parse(nud_MinH_df_BrightBlob_BL.Value.ToString()); // (20190215) Jeff Revised!
            Recipe.Param.InspParam_BrightBlob_BL.MaxHeight_defect = int.Parse(nud_MaxH_df_BrightBlob_BL.Value.ToString()); // (20190215) Jeff Revised!
            Recipe.Param.InspParam_BrightBlob_BL.MinWidth_defect = int.Parse(nud_MinW_df_BrightBlob_BL.Value.ToString()); // (20190215) Jeff Revised!
            Recipe.Param.InspParam_BrightBlob_BL.MaxWidth_defect = int.Parse(nud_MaxW_df_BrightBlob_BL.Value.ToString()); // (20190215) Jeff Revised!

            // 【AI只覆判模稜兩可NG】
            Recipe.Param.InspParam_BrightBlob_BL.cls_absolNG.Enabled = cbx_notSureNG_BrightBlob_BL.Checked;
            Recipe.Param.InspParam_BrightBlob_BL.cls_absolNG.Enabled_Gray_select = cbx_Enabled_Gray_select_BrightBlob_BL_AbsolNG.Checked;
            Recipe.Param.InspParam_BrightBlob_BL.cls_absolNG.str_Gray_select = combx_str_Gray_select_BrightBlob_BL_AbsolNG.Text;
            Recipe.Param.InspParam_BrightBlob_BL.cls_absolNG.Gray_select = int.Parse(nud_Gray_select_BrightBlob_BL_AbsolNG.Value.ToString());
            Recipe.Param.InspParam_BrightBlob_BL.cls_absolNG.str_op = combx_str_op_BrightBlob_BL_AbsolNG.Text;
            Recipe.Param.InspParam_BrightBlob_BL.cls_absolNG.Enabled_Area = cbx_Enabled_Area_BrightBlob_BL_AbsolNG.Checked;
            Recipe.Param.InspParam_BrightBlob_BL.cls_absolNG.MinArea_defect = int.Parse(nud_MinA_df_BrightBlob_BL_AbsolNG.Value.ToString());
            Recipe.Param.InspParam_BrightBlob_BL.cls_absolNG.MaxArea_defect = int.Parse(nud_MaxA_df_BrightBlob_BL_AbsolNG.Value.ToString());

            #endregion

            #region 檢測保缺角 (LackAngle) (Back Light) 之參數

            Recipe.Param.InspParam_LackAngle_BL.Enabled = cbx_LackAngle_BL.Checked;
            Recipe.Param.InspParam_LackAngle_BL.ImageIndex = int.Parse(nud_ImageID_LackAngle_BL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_BL.Enabled_Cell = cbx_Enabled_Cell_Step1_LackAngle_BL.Checked;
            Recipe.Param.InspParam_LackAngle_BL.Enabled_Cell = cbx_Enabled_Cell_Step4_LackAngle_BL.Checked;
            Recipe.Param.InspParam_LackAngle_BL.W_Cell = int.Parse(nud_W_Cell_LackAngle_BL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_BL.H_Cell = int.Parse(nud_H_Cell_LackAngle_BL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_BL.Enabled_BypassReg = cbx_Enabled_BypassReg_LackAngle_BL.Checked;
            Recipe.Param.InspParam_LackAngle_BL.x_BypassReg = int.Parse(nud_x_BypassReg_LackAngle_BL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_BL.y_BypassReg = int.Parse(nud_y_BypassReg_LackAngle_BL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_BL.W_BypassReg = int.Parse(nud_W_BypassReg_LackAngle_BL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_BL.H_BypassReg = int.Parse(nud_H_BypassReg_LackAngle_BL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_BL.Enabled_BlackStripe = cbx_Enabled_BlackStripe_Step1_LackAngle_BL.Checked;
            Recipe.Param.InspParam_LackAngle_BL.Enabled_BlackStripe = cbx_Enabled_BlackStripe_Step4_LackAngle_BL.Checked;
            Recipe.Param.InspParam_LackAngle_BL.H_BlackStripe = int.Parse(nud_H_BlackStripe_LackAngle_BL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_BL.Enabled_EquHisto = cbx_Enabled_EquHisto_Step2_LackAngle_BL.Checked;
            Recipe.Param.InspParam_LackAngle_BL.Enabled_EquHisto = cbx_Enabled_EquHisto_Step3_LackAngle_BL.Checked;
            Recipe.Param.InspParam_LackAngle_BL.MinGray_Bright_BL = int.Parse(nud_MinGray_Bright_BL_LackAngle_BL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_BL.MaxGray_Bright_BL = int.Parse(nud_MaxGray_Bright_BL_LackAngle_BL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_BL.w_Part = int.Parse(nud_w_Part_LackAngle_BL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_BL.h_Part = int.Parse(nud_h_Part_LackAngle_BL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_BL.MinGray_Cell = int.Parse(nud_MinGray_Cell_LackAngle_BL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_BL.MinGray_Resist = int.Parse(nud_MinGray_Resist_LackAngle_BL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_BL.Gray_select = int.Parse(nud_Gray_select_LackAngle_BL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_BL.Enabled_Height = cbx_Enabled_Height_LackAngle_BL.Checked;
            Recipe.Param.InspParam_LackAngle_BL.Enabled_Width = cbx_Enabled_Width_LackAngle_BL.Checked;
            Recipe.Param.InspParam_LackAngle_BL.MinHeight_defect = int.Parse(nud_MinH_df_LackAngle_BL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_BL.MaxHeight_defect = int.Parse(nud_MaxH_df_LackAngle_BL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_BL.MinWidth_defect = int.Parse(nud_MinW_df_LackAngle_BL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_BL.MaxWidth_defect = int.Parse(nud_MaxW_df_LackAngle_BL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_BL.str_op1 = combx_str_op1_LackAngle_BL.Text;
            Recipe.Param.InspParam_LackAngle_BL.Enabled_Area = cbx_Enabled_Area_LackAngle_BL.Checked;
            Recipe.Param.InspParam_LackAngle_BL.MinArea_defect = int.Parse(nud_MinA_df_LackAngle_BL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_BL.MaxArea_defect = int.Parse(nud_MaxA_df_LackAngle_BL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_BL.str_op2 = combx_str_op2_LackAngle_BL.Text;

            // 【AI只覆判模稜兩可NG】
            Recipe.Param.InspParam_LackAngle_BL.cls_absolNG.Enabled = cbx_notSureNG_LackAngle_BL.Checked;
            Recipe.Param.InspParam_LackAngle_BL.cls_absolNG.Enabled_Gray_select = cbx_Enabled_Gray_select_LackAngle_BL_AbsolNG.Checked;
            Recipe.Param.InspParam_LackAngle_BL.cls_absolNG.str_Gray_select = combx_str_Gray_select_LackAngle_BL_AbsolNG.Text;
            Recipe.Param.InspParam_LackAngle_BL.cls_absolNG.Gray_select = int.Parse(nud_Gray_select_LackAngle_BL_AbsolNG.Value.ToString());
            Recipe.Param.InspParam_LackAngle_BL.cls_absolNG.str_op = combx_str_op_LackAngle_BL_AbsolNG.Text;
            Recipe.Param.InspParam_LackAngle_BL.cls_absolNG.Enabled_Area = cbx_Enabled_Area_LackAngle_BL_AbsolNG.Checked;
            Recipe.Param.InspParam_LackAngle_BL.cls_absolNG.MinArea_defect = int.Parse(nud_MinA_df_LackAngle_BL_AbsolNG.Value.ToString());
            Recipe.Param.InspParam_LackAngle_BL.cls_absolNG.MaxArea_defect = int.Parse(nud_MaxA_df_LackAngle_BL_AbsolNG.Value.ToString());

            #endregion

            #region 檢測保缺角 (LackAngle) (Side Light) 之參數

            Recipe.Param.InspParam_LackAngle_SL.Enabled = cbx_LackAngle_SL.Checked;
            Recipe.Param.InspParam_LackAngle_SL.ImageIndex = int.Parse(nud_ImageID_LackAngle_SL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_SL.Enabled_Wsmall = cbx_Enabled_Wsmall_LackAngle_SL.Checked;
            Recipe.Param.InspParam_LackAngle_SL.str_L_NonInspect = combx_str_L_NonInspect_LackAngle_SL.Text;
            Recipe.Param.InspParam_LackAngle_SL.Enabled_Cell = cbx_Enabled_Cell_Step1_LackAngle_SL.Checked;
            Recipe.Param.InspParam_LackAngle_SL.Enabled_Cell = cbx_Enabled_Cell_Step2_LackAngle_SL.Checked;
            Recipe.Param.InspParam_LackAngle_SL.W_Cell = int.Parse(nud_W_Cell_LackAngle_SL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_SL.H_Cell = int.Parse(nud_H_Cell_LackAngle_SL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_SL.Enabled_BypassReg = cbx_Enabled_BypassReg_LackAngle_SL.Checked;
            Recipe.Param.InspParam_LackAngle_SL.x_BypassReg = int.Parse(nud_x_BypassReg_LackAngle_SL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_SL.y_BypassReg = int.Parse(nud_y_BypassReg_LackAngle_SL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_SL.W_BypassReg = int.Parse(nud_W_BypassReg_LackAngle_SL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_SL.H_BypassReg = int.Parse(nud_H_BypassReg_LackAngle_SL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_SL.Enabled_BlackStripe = cbx_Enabled_BlackStripe_Step1_LackAngle_SL.Checked;
            Recipe.Param.InspParam_LackAngle_SL.Enabled_BlackStripe = cbx_Enabled_BlackStripe_Step2_LackAngle_SL.Checked;
            Recipe.Param.InspParam_LackAngle_SL.str_ModeBlackStripe = combx_str_ModeBlackStripe_LackAngle_SL.Text;
            Recipe.Param.InspParam_LackAngle_SL.H_BlackStripe = int.Parse(nud_H_BlackStripe_LackAngle_SL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_SL.H_BlackStripe_mode2 = int.Parse(nud_H_BlackStripe_mode2_LackAngle_SL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_SL.str_BinaryImg_Cell = combx_str_BinaryImg_Cell_LackAngle_SL.Text;
            Recipe.Param.InspParam_LackAngle_SL.MaskWidth_Cell = int.Parse(nud_MaskWidth_1_Cell_LackAngle_SL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_SL.MaskHeight_Cell = int.Parse(nud_MaskHeight_1_Cell_LackAngle_SL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_SL.str_LightDark_Cell = combx_str_LightDark_1_Cell_LackAngle_SL.Text;
            Recipe.Param.InspParam_LackAngle_SL.Offset_Cell = int.Parse(nud_Offset_1_Cell_LackAngle_SL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_SL.MinGray_Cell = int.Parse(nud_MinGray_1_Cell_LackAngle_SL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_SL.MaxGray_Cell = int.Parse(nud_MaxGray_1_Cell_LackAngle_SL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_SL.str_BinaryImg_Resist = combx_str_BinaryImg_Resist_LackAngle_SL.Text;
            Recipe.Param.InspParam_LackAngle_SL.MaskWidth_Resist = int.Parse(nud_MaskWidth_1_Resist_LackAngle_SL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_SL.MaskHeight_Resist = int.Parse(nud_MaskHeight_1_Resist_LackAngle_SL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_SL.str_LightDark_Resist = combx_str_LightDark_1_Resist_LackAngle_SL.Text;
            Recipe.Param.InspParam_LackAngle_SL.Offset_Resist = int.Parse(nud_Offset_1_Resist_LackAngle_SL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_SL.MinGray_Resist = int.Parse(nud_MinGray_1_Resist_LackAngle_SL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_SL.MaxGray_Resist = int.Parse(nud_MaxGray_1_Resist_LackAngle_SL.Value.ToString());

            // 規格2 (20190411) Jeff Revised!
            Recipe.Param.InspParam_LackAngle_SL.count_BinImg_Cell = int.Parse(nud_count_BinImg_Cell_LackAngle_SL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_SL.MinGray_2_Cell = int.Parse(nud_MinGray_2_Cell_LackAngle_SL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_SL.MaxGray_2_Cell = int.Parse(nud_MaxGray_2_Cell_LackAngle_SL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_SL.MaskWidth_2_Cell = int.Parse(nud_MaskWidth_2_Cell_LackAngle_SL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_SL.MaskHeight_2_Cell = int.Parse(nud_MaskHeight_2_Cell_LackAngle_SL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_SL.str_LightDark_2_Cell = combx_str_LightDark_2_Cell_LackAngle_SL.Text;
            Recipe.Param.InspParam_LackAngle_SL.Offset_2_Cell = int.Parse(nud_Offset_2_Cell_LackAngle_SL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_SL.count_BinImg_Resist = int.Parse(nud_count_BinImg_Resist_LackAngle_SL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_SL.MinGray_2_Resist = int.Parse(nud_MinGray_2_Resist_LackAngle_SL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_SL.MaxGray_2_Resist = int.Parse(nud_MaxGray_2_Resist_LackAngle_SL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_SL.MaskWidth_2_Resist = int.Parse(nud_MaskWidth_2_Resist_LackAngle_SL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_SL.MaskHeight_2_Resist = int.Parse(nud_MaskHeight_2_Resist_LackAngle_SL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_SL.str_LightDark_2_Resist = combx_str_LightDark_2_Resist_LackAngle_SL.Text;
            Recipe.Param.InspParam_LackAngle_SL.Offset_2_Resist = int.Parse(nud_Offset_2_Resist_LackAngle_SL.Value.ToString());

            Recipe.Param.InspParam_LackAngle_SL.Enabled_Gray_select_Cell = cbx_Enabled_Gray_select_Cell_LackAngle_SL.Checked;
            Recipe.Param.InspParam_LackAngle_SL.str_Gray_select_Cell = combx_str_Gray_select_Cell_LackAngle_SL.Text;
            Recipe.Param.InspParam_LackAngle_SL.Gray_select_Cell = int.Parse(nud_Gray_select_Cell_LackAngle_SL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_SL.Enabled_Gray_select_Resist = cbx_Enabled_Gray_select_Resist_LackAngle_SL.Checked;
            Recipe.Param.InspParam_LackAngle_SL.str_Gray_select_Resist = combx_str_Gray_select_Resist_LackAngle_SL.Text;
            Recipe.Param.InspParam_LackAngle_SL.Gray_select_Resist = int.Parse(nud_Gray_select_Resist_LackAngle_SL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_SL.Enabled_Cell2 = cbx_Enabled_Cell2_Step4_LackAngle_SL.Checked;
            Recipe.Param.InspParam_LackAngle_SL.W_Cell2 = int.Parse(nud_W_Cell2_LackAngle_SL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_SL.H_Cell2 = int.Parse(nud_H_Cell2_LackAngle_SL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_SL.count_op1 = int.Parse(nud_count_op1_LackAngle_SL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_SL.Enabled_Height = cbx_Enabled_Height_LackAngle_SL.Checked;
            Recipe.Param.InspParam_LackAngle_SL.Enabled_Width = cbx_Enabled_Width_LackAngle_SL.Checked;
            Recipe.Param.InspParam_LackAngle_SL.MinHeight_defect = int.Parse(nud_MinH_df_LackAngle_SL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_SL.MaxHeight_defect = int.Parse(nud_MaxH_df_LackAngle_SL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_SL.MinWidth_defect = int.Parse(nud_MinW_df_LackAngle_SL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_SL.MaxWidth_defect = int.Parse(nud_MaxW_df_LackAngle_SL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_SL.str_op1 = combx_str_op1_LackAngle_SL.Text;
            Recipe.Param.InspParam_LackAngle_SL.Enabled_Height_op1_2 = cbx_Enabled_Height_op1_2_LackAngle_SL.Checked;
            Recipe.Param.InspParam_LackAngle_SL.Enabled_Width_op1_2 = cbx_Enabled_Width_op1_2_LackAngle_SL.Checked;
            Recipe.Param.InspParam_LackAngle_SL.MinHeight_defect_op1_2 = int.Parse(nud_MinH_df_op1_2_LackAngle_SL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_SL.MaxHeight_defect_op1_2 = int.Parse(nud_MaxH_df_op1_2_LackAngle_SL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_SL.MinWidth_defect_op1_2 = int.Parse(nud_MinW_df_op1_2_LackAngle_SL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_SL.MaxWidth_defect_op1_2 = int.Parse(nud_MaxW_df_op1_2_LackAngle_SL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_SL.str_op1_2 = combx_str_op1_2_LackAngle_SL.Text;
            Recipe.Param.InspParam_LackAngle_SL.Enabled_Area = cbx_Enabled_Area_LackAngle_SL.Checked;
            Recipe.Param.InspParam_LackAngle_SL.MinArea_defect = int.Parse(nud_MinA_df_LackAngle_SL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_SL.MaxArea_defect = int.Parse(nud_MaxA_df_LackAngle_SL.Value.ToString());
            Recipe.Param.InspParam_LackAngle_SL.str_op2 = combx_str_op2_LackAngle_SL.Text;

            // 【AI只覆判模稜兩可NG】
            Recipe.Param.InspParam_LackAngle_SL.cls_absolNG.Enabled = cbx_notSureNG_LackAngle_SL.Checked;
            Recipe.Param.InspParam_LackAngle_SL.cls_absolNG.Enabled_Gray_select = cbx_Enabled_Gray_select_LackAngle_SL_AbsolNG.Checked;
            Recipe.Param.InspParam_LackAngle_SL.cls_absolNG.str_Gray_select = combx_str_Gray_select_LackAngle_SL_AbsolNG.Text;
            Recipe.Param.InspParam_LackAngle_SL.cls_absolNG.Gray_select = int.Parse(nud_Gray_select_LackAngle_SL_AbsolNG.Value.ToString());
            Recipe.Param.InspParam_LackAngle_SL.cls_absolNG.str_op = combx_str_op_LackAngle_SL_AbsolNG.Text;
            Recipe.Param.InspParam_LackAngle_SL.cls_absolNG.Enabled_Area = cbx_Enabled_Area_LackAngle_SL_AbsolNG.Checked;
            Recipe.Param.InspParam_LackAngle_SL.cls_absolNG.MinArea_defect = int.Parse(nud_MinA_df_LackAngle_SL_AbsolNG.Value.ToString());
            Recipe.Param.InspParam_LackAngle_SL.cls_absolNG.MaxArea_defect = int.Parse(nud_MaxA_df_LackAngle_SL_AbsolNG.Value.ToString());

            #endregion

            #endregion
        }

        public void ClearDisplay(HObject OrgImage)
        {
            HOperatorSet.ClearWindow(DisplayWindows.HalconWindow);
            if (OrgImage != null)
                HOperatorSet.DispObj(OrgImage, DisplayWindows.HalconWindow);
            Current_disp_regions.Dispose();
            HOperatorSet.GenEmptyRegion(out Current_disp_regions);
        }

        #endregion


        #region //=========================  Event ========================

        private void btnLoadImg_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog OpenImgDilg = new OpenFileDialog();
                //OpenImgDilg.Filter = "(*.tif)|*.tif|(*.bmp)|*.bmp|(*.jpg)|*.jpg";
                OpenImgDilg.Filter = "All Files|*.*";

                if (OpenImgDilg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return;
                string path = OpenImgDilg.FileName;

                string FileType = path.Substring(path.LastIndexOf(".") + 1);
                List<string> FileList = new List<string>();

                for (int i = 1; i <= Recipe.Param.ImgCount; i++)
                {
                    string TmpPath = path.Substring(0, path.LastIndexOf("_") + 1);
                    string FilePathName = TmpPath + "F" + i.ToString("000") + "." + FileType;
                    FileList.Add(FilePathName);

                }

                HObject TmpImg;
                HOperatorSet.GenEmptyObj(out TmpImg);

                for (int i = 0; i < Input_ImgList.Count; i++)
                {
                    if (Input_ImgList[i] != null)
                    {
                        Input_ImgList[i].Dispose();
                        Input_ImgList[i] = null;
                    }
                }
                Input_ImgList.Clear();

                for (int i = 0; i < FileList.Count; i++)
                {
                    if (!File.Exists(FileList[i]))
                    {
                        continue;
                    }
                    HOperatorSet.ReadImage(out TmpImg, FileList[i]);
                    Input_ImgList.Add(TmpImg.Clone());
                    TmpImg.Dispose();
                }

                TmpImg.Dispose();
                TmpImg = null;

                if (Input_ImgList.Count <= 0)
                {
                    HOperatorSet.GenEmptyObj(out TmpImg);
                    for (int i = 0; i < Input_ImgList.Count; i++)
                    {
                        if (Input_ImgList[i] != null)
                        {
                            Input_ImgList[i].Dispose();
                            Input_ImgList[i] = null;
                        }
                    }

                    Input_ImgList.Clear();

                    HOperatorSet.ReadImage(out TmpImg, path);
                    Input_ImgList.Add(TmpImg.Clone());
                    TmpImg.Dispose();
                    TmpImg = null;
                }

                UpdateDisplayList();
                // 顯示影像
                if (Input_ImgList.Count > 0)
                {
                    Current_disp_image = Method.Insp_ImgList[0].Clone();
                    HOperatorSet.DispObj(Current_disp_image, DisplayWindows.HalconWindow);
                    HOperatorSet.SetPart(DisplayWindows.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part
                }

                clear_rbtn_Step(); // (20190218) Jeff Revised!
                b_Initial_State = true;
            }
            catch (Exception ex)
            {
                string message = "Error: " + ex.Message;
                MessageBox.Show(message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button_Add_Click_1(object sender, EventArgs e)
        {
            OnUserControlButtonClicked(this, e);
        }

        private void btnSaveImg_Click(object sender, EventArgs e)
        {
            // 檢測結果影像 (20190403) Jeff Revised!
            button_Inspection_Click(button_Inspection, e);
            // 轉成RGB image
            HObject img_result = null;
            HTuple Channels;
            HOperatorSet.CountChannels(Method.Insp_ImgList[combxDisplayImg.SelectedIndex], out Channels);
            if (Channels == 1)
            {
                HOperatorSet.Compose3(Method.Insp_ImgList[combxDisplayImg.SelectedIndex].Clone(), Method.Insp_ImgList[combxDisplayImg.SelectedIndex].Clone(), Method.Insp_ImgList[combxDisplayImg.SelectedIndex].Clone(), out img_result);
            }
            else
                HOperatorSet.CopyImage(Method.Insp_ImgList[combxDisplayImg.SelectedIndex], out img_result);

            if (Method.ho_Region_InspReg != null)
                HOperatorSet.OverpaintRegion(img_result, Method.ho_Region_InspReg, ((new HTuple(0)).TupleConcat(255)).TupleConcat(0), "margin");
            if (Method.ho_Orig_DefectReg != null)
                HOperatorSet.OverpaintRegion(img_result, Method.ho_Orig_DefectReg, ((new HTuple(255)).TupleConcat(0)).TupleConcat(0), "fill");

            SaveFileDialog saveImgDialog = new SaveFileDialog();
            //saveImgDialog.Filter = "JPeg Image|*.jpg|PNG Image|*.png|Bitmap Image|*.bmp|TIFF Image|*.tif";
            saveImgDialog.Filter = "TIFF Image|*.tif|JPeg Image|*.jpg|PNG Image|*.png|Bitmap Image|*.bmp";
            saveImgDialog.Title = "另存結果影像";
            if (saveImgDialog.ShowDialog() != DialogResult.OK)
                return;

            string path = saveImgDialog.FileName;
            try
            {
                /*
                switch (saveImgDialog.FilterIndex)
                {
                    case 1:
                        HOperatorSet.WriteImage(img_result, "tiff", 0, path);
                        break;
                    case 2:
                        HOperatorSet.WriteImage(img_result, "jpeg", 0, path);
                        break;
                    case 3:
                        HOperatorSet.WriteImage(img_result, "png", 0, path);
                        break;
                    case 4:
                        HOperatorSet.WriteImage(img_result, "bmp", 0, path);
                        break;
                }
                */
                string ImageFormat = ((enu_ImageFormat)(saveImgDialog.FilterIndex - 1)).ToString();
                HOperatorSet.WriteImage(img_result, ImageFormat, 0, path); // (20191226) MIL Jeff Revised!
            }
            catch
            {
                SystemSounds.Exclamation.Play();
                MessageBox.Show("Save result image fails...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            img_result.Dispose();
        }        

        private void button_LoadParam_Click(object sender, EventArgs e)
        {
            UpdateParameter();
            clear_rbtn_Step();
        }

        private void button_SaveParam_Click(object sender, EventArgs e) // (20190807) Jeff Revised!
        {
            //DialogResult dr = MessageBox.Show("Are you sure you want to save parameters to .xml file?", "Warning", MessageBoxButtons.YesNo);
            BtnLog.WriteLog("[Inspection UI] Save Param Click");
            DialogResult dr = MessageBox.Show("確定要儲存參數到xml檔嗎?", "警告", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    /* Get parameters from UI */
                    ParamGetFromUI(); // (20190807) Jeff Revised!

                    // Set parameter to method
                    Method.set_parameter(Recipe);

                    // Save parameter to XML file !!!! 一定要加
                    Recipe.save();
                    BtnLog.WriteLog("[Inspection UI] Save Param Done");
                    MessageBox.Show("Save parameters succeeds!");
                }
                catch
                {
                    SystemSounds.Exclamation.Play();
                    MessageBox.Show("Save parameters fails...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                BtnLog.WriteLog("[Inspection UI] Save Param Cancel");
            }
        }
        
        private void button_Inspection_Click(object sender, EventArgs e)
        {
            if (Input_ImgList.Count <= 0)
            {
                MessageBox.Show("請先載入影像");
                return;
            }
            //combxDisplayImg.SelectedIndex = 0;
            ParamGetFromUI();
            clear_rbtn_Step(); // (20190216) Jeff Revised!
            
            Button bt = sender as Button;
            string tag = bt.Tag.ToString();
            if (tag == "總檢測") // 【參數管理】
            {
                ClearDisplay(Method.Insp_ImgList[combxDisplayImg.SelectedIndex]);
                if (bt.Text == "原始影像 (ID)") return; // 【參數管理】 => 【原始影像 (ID)】
            }
            else // tbInspParamSetting
            {
                switch (tbInspParamSetting.SelectedTab.Text)
                {
                    case "初定位":
                        {
                            //ClearDisplay(Method.Insp_ImgList[combxDisplayImg.SelectedIndex]);
                        }
                        break;
                    case "Cell分割 (同軸光)":
                        {
                            combxDisplayImg.SelectedIndex = int.Parse(nud_ImageID_CellSeg_CL.Value.ToString());
                        }
                        break;
                    case "Cell分割 (背光)":
                        {
                            combxDisplayImg.SelectedIndex = int.Parse(nud_ImageID_CellSeg_BL.Value.ToString());
                        }
                        break;
                    case "瑕疵Cell判斷範圍":
                        {
                            //ClearDisplay(Method.Insp_ImgList[combxDisplayImg.SelectedIndex]);
                        }
                        break;
                    case "檢測黑條內白點 (正光)":
                        {
                            combxDisplayImg.SelectedIndex = int.Parse(nud_ImageID_WhiteBlob_FL.Value.ToString());
                        }
                        break;
                    case "檢測黑條內白點 (同軸光)":
                        {
                            combxDisplayImg.SelectedIndex = int.Parse(nud_ImageID_WhiteBlob_CL.Value.ToString());
                        }
                        break;
                    case "檢測絲狀異物 (正光)":
                        {
                            combxDisplayImg.SelectedIndex = int.Parse(nud_ImageID_Line_FL.Value.ToString());
                        }
                        break;
                    case "檢測白條內黑點 (正光)":
                        {
                            combxDisplayImg.SelectedIndex = int.Parse(nud_ImageID_BlackCrack_FL.Value.ToString());
                        }
                        break;
                    case "檢測白條內黑點 (同軸光)":
                        {
                            combxDisplayImg.SelectedIndex = int.Parse(nud_ImageID_BlackCrack_CL.Value.ToString());
                        }
                        break;
                    case "檢測黑條內黑點1 (同軸光)":
                        {
                            combxDisplayImg.SelectedIndex = int.Parse(nud_ImageID_BlackBlob_CL.Value.ToString());
                        }
                        break;
                    case "檢測黑條內黑點2 (同軸光)":
                        {
                            combxDisplayImg.SelectedIndex = int.Parse(nud_ImageID_BlackBlob2_CL.Value.ToString());
                        }
                        break;
                    case "檢測絲狀異物 (正光&同軸光)":
                        {
                            combxDisplayImg.SelectedIndex = int.Parse(nud_ImageID_BlackBlob2_CL.Value.ToString());
                        }
                        break;
                    case "檢測汙染 (同軸光)":
                        {
                            combxDisplayImg.SelectedIndex = int.Parse(nud_ImageID_Dirty_CL.Value.ToString());
                        }
                        break;
                    case "檢測汙染 (正光&同軸光)":
                        {
                            if (bt.Text == "原始影像 (ID1)" || bt.Text == "檢測 (影像ID1)")
                                combxDisplayImg.SelectedIndex = int.Parse(nud_ImageID1_Dirty_FLCL.Value.ToString());
                            else
                                combxDisplayImg.SelectedIndex = int.Parse(nud_ImageID2_Dirty_FLCL.Value.ToString());
                        }
                        break;
                    case "檢測電極區白點 (背光)":
                        {
                            combxDisplayImg.SelectedIndex = int.Parse(nud_ImageID_BrightBlob_BL.Value.ToString());
                        }
                        break;
                    case "檢測保缺角 (背光)":
                        {
                            combxDisplayImg.SelectedIndex = int.Parse(nud_ImageID_LackAngle_BL.Value.ToString());
                        }
                        break;
                    case "檢測保缺角 (側光背光)":
                        {
                            combxDisplayImg.SelectedIndex = int.Parse(nud_ImageID_LackAngle_SL.Value.ToString());
                        }
                        break;
                }

                combxDisplayImg_SelectedIndexChanged(combxDisplayImg, e);
                if (tag == "原始影像")
                    return;
            }
            
            Method.set_parameter(Recipe);

            #region Insp AI & AOI

            if (Recipe.DAVS == null)
                Recipe.DAVS = new clsDAVS(Recipe.DAVSParam, Recipe.GetRecipePath());

            Recipe.DAVS.SetPathParam(ModuleName, SB_ID, MOVE_ID, PartNumber);
            List<Defect> ResList;
            HObject ResRegion = null;
            try
            {
                bool Res = false;
                if (tag == "總檢測") // 【參數管理】 => 【檢測】
                {
                    if (Method.execute(Recipe, Input_ImgList, out ResRegion, out ResList))
                    {
                        if (Method.ho_Region_InspReg != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "green");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Method.ho_Region_InspReg, DisplayWindows.HalconWindow);
                        }

                        HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                        ResRegion = Method.ho_Orig_DefectReg.Clone();
                        Res = true;
                    }
                }
                else // tbInspParamSetting
                {
                    switch (tbInspParamSetting.SelectedTab.Text)
                    {
                        case "初定位":
                            {
                                if (Inspection_InitialPosition())
                                {
                                    if (Method.ho_Regions_BlackStripe_Wsmall != null)
                                    {
                                        HOperatorSet.SetColor(DisplayWindows.HalconWindow, "green");
                                        HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                                        HOperatorSet.DispObj(Method.ho_Regions_BlackStripe_Wsmall, DisplayWindows.HalconWindow);
                                    }
                                    if (Method.ho_Regions_BlackStripe_Wsmall3 != null)
                                    {
                                        HOperatorSet.SetColor(DisplayWindows.HalconWindow, "blue");
                                        HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                                        HOperatorSet.DispObj(Method.ho_Regions_BlackStripe_Wsmall3, DisplayWindows.HalconWindow);
                                    }
                                    ResRegion = Method.ho_Regions_BlackStripe.Clone();
                                    HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                                    Res = true;
                                }
                            }
                            break;
                        case "Cell分割 (同軸光)":
                            {
                                if (Inspection_CellSeg_CL())
                                {
                                    ResRegion = Method.ho_AllCells_Rect.Clone();
                                    HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                                    Res = true;
                                }
                            }
                            break;
                        case "Cell分割 (背光)":
                            {
                                if (Inspection_CellSeg_BL())
                                {
                                    ResRegion = Method.ho_AllCells_Rect.Clone();
                                    HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                                    Res = true;
                                }
                            }
                            break;
                        case "瑕疵Cell判斷範圍":
                            {
                                if (Method.execute(Recipe, Input_ImgList, out ResRegion, out ResList))
                                {
                                    //if (Method.ho_AllCells_Rect != null)
                                    //{
                                    //    HOperatorSet.SetColor(DisplayWindows.HalconWindow, "green");
                                    //    HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                                    //    HOperatorSet.DispObj(Method.ho_AllCells_Rect, DisplayWindows.HalconWindow);
                                    //}
                                    if (Method.ho_Region_InspReg != null)
                                    {
                                        HOperatorSet.SetColor(DisplayWindows.HalconWindow, "green");
                                        HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                                        HOperatorSet.DispObj(Method.ho_Region_InspReg, DisplayWindows.HalconWindow);
                                    }
                                    if (Method.DefectReg_CellCenter != null)
                                    {
                                        HOperatorSet.SetColor(DisplayWindows.HalconWindow, "pink");
                                        HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                                        HObject CircleRegions = null;
                                        Method.Convert2Circle(Method.DefectReg_CellCenter, out CircleRegions, 3);
                                        HOperatorSet.DispObj(CircleRegions, DisplayWindows.HalconWindow);
                                    }
                                    if (Method.ho_InspRects_AllCells != null)
                                    {
                                        //HOperatorSet.SetColored(DisplayWindows.HalconWindow, 12);
                                        HOperatorSet.SetColor(DisplayWindows.HalconWindow, "blue");
                                        HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                                        HOperatorSet.DispObj(Method.ho_InspRects_AllCells, DisplayWindows.HalconWindow);
                                    }

                                    HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                                    ResRegion = Method.ho_Orig_DefectReg.Clone();
                                    Res = true;
                                }
                            }
                            break;
                        case "檢測黑條內白點 (正光)":
                            {
                                if (Inspection_WhiteBlob_FL())
                                {
                                    if (tag == "檢測AOI絕對NG")
                                        ResRegion = Method.Dict_AOI_absolNG["黑條內白點 (正光)"].DefectReg.Clone();
                                    else
                                        ResRegion = Method.ho_DefectReg_WhiteBlob_FL.Clone();
                                    HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                                    Res = true;
                                }
                            }
                            break;
                        case "檢測黑條內白點 (同軸光)":
                            {
                                if (Inspection_WhiteBlob_CL())
                                {
                                    if (tag == "檢測AOI絕對NG")
                                        ResRegion = Method.Dict_AOI_absolNG["黑條內白點 (同軸光)"].DefectReg.Clone();
                                    else
                                        ResRegion = Method.ho_DefectReg_WhiteBlob_CL.Clone();
                                    HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                                    Res = true;
                                }
                            }
                            break;
                        case "檢測絲狀異物 (正光)":
                            {
                                if (Inspection_Line_FL())
                                {
                                    if (tag == "檢測AOI絕對NG")
                                        ResRegion = Method.Dict_AOI_absolNG["絲狀異物 (正光)"].DefectReg.Clone();
                                    else
                                        ResRegion = Method.ho_DefectReg_Line.Clone();
                                    HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                                    Res = true;
                                }
                            }
                            break;
                        case "檢測白條內黑點 (正光)":
                            {
                                if (Inspection_BlackCrack_FL())
                                {
                                    if (tag == "檢測AOI絕對NG")
                                        ResRegion = Method.Dict_AOI_absolNG["白條內黑點 (正光)"].DefectReg.Clone();
                                    else
                                        ResRegion = Method.ho_DefectReg_BlackCrack_FL.Clone();
                                    HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                                    Res = true;
                                }
                            }
                            break;
                        case "檢測白條內黑點 (同軸光)":
                            {
                                if (Inspection_BlackCrack_CL())
                                {
                                    if (tag == "檢測AOI絕對NG")
                                        ResRegion = Method.Dict_AOI_absolNG["白條內黑點 (同軸光)"].DefectReg.Clone();
                                    else
                                        ResRegion = Method.ho_DefectReg_BlackCrack_CL.Clone();
                                    HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                                    Res = true;
                                }
                            }
                            break;
                        case "檢測黑條內黑點1 (同軸光)":
                            {
                                if (Method.Read_AllImages_Insp(Recipe, Input_ImgList))
                                {
                                    if (Method.InitialPosition_Insp(Recipe))
                                    {
                                        if (cbx_CellSeg_CL.Checked) // (20190810) Jeff Revised!
                                        {
                                            if (!(Method.CellSeg_CL_Insp(Recipe))) break;
                                        }
                                        else
                                        {
                                            if (!(Method.CellSeg_BL_Insp(Recipe))) break;
                                        }
                                        if (!(Method.Df_CellReg(Recipe))) break; // (20190810) Jeff Revised!
                                        if (Method.BlackBlob_CL_Insp(Recipe))
                                        {
                                            if (tag == "檢測AOI絕對NG")
                                                ResRegion = Method.Dict_AOI_absolNG["BlackBlob (同軸光)"].DefectReg.Clone();
                                            else
                                                ResRegion = Method.ho_DefectReg_BlackBlob.Clone();
                                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                                            Res = true;
                                        }
                                    }
                                }
                            }
                            break;
                        case "檢測黑條內黑點2 (同軸光)":
                            {
                                if (Inspection_BlackBlob2_CL())
                                {
                                    if (tag == "檢測AOI絕對NG")
                                        ResRegion = Method.Dict_AOI_absolNG["BlackBlob2 (同軸光)"].DefectReg.Clone();
                                    else
                                        ResRegion = Method.ho_DefectReg_BlackBlob2.Clone();
                                    HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                                    Res = true;
                                }
                            }
                            break;
                        case "檢測絲狀異物 (正光&同軸光)":
                            {
                                if (Inspection_Line_FLCL())
                                {
                                    if (tag == "檢測AOI絕對NG")
                                        ResRegion = Method.Dict_AOI_absolNG["絲狀異物 (正光&同軸光)"].DefectReg.Clone();
                                    else
                                        ResRegion = Method.ho_DefectReg_Line_FLCL.Clone();
                                    HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                                    Res = true;
                                }
                            }
                            break;
                        case "檢測汙染 (同軸光)":
                            {
                                if (Inspection_Dirty_CL())
                                {
                                    if (tag == "檢測AOI絕對NG")
                                        ResRegion = Method.Dict_AOI_absolNG["汙染 (同軸光)"].DefectReg.Clone();
                                    else
                                        ResRegion = Method.ho_DefectReg_Dirty.Clone();
                                    HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                                    Res = true;
                                }
                            }
                            break;
                        case "檢測汙染 (正光&同軸光)":
                            {
                                if (Method.Read_AllImages_Insp(Recipe, Input_ImgList))
                                {
                                    if (Method.InitialPosition_Insp(Recipe))
                                    {
                                        if (cbx_CellSeg_CL.Checked) // (20190810) Jeff Revised!
                                        {
                                            if (!(Method.CellSeg_CL_Insp(Recipe))) break;
                                        }
                                        else
                                        {
                                            if (!(Method.CellSeg_BL_Insp(Recipe))) break;
                                        }
                                        if (!(Method.Df_CellReg(Recipe))) break; // (20190810) Jeff Revised!
                                        if (Method.Dirty_FLCL_Insp(Recipe))
                                        {
                                            if (tag == "檢測AOI絕對NG")
                                                ResRegion = Method.Dict_AOI_absolNG["汙染 (正光&同軸光)"].DefectReg.Clone();
                                            else
                                                ResRegion = Method.ho_DefectReg_Dirty_FLCL.Clone();
                                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                                            Res = true;
                                        }
                                    }
                                }
                            }
                            break;
                        case "檢測電極區白點 (背光)":
                            {
                                if (Inspection_BrightBlob_BL())
                                {
                                    if (tag == "檢測AOI絕對NG")
                                        ResRegion = Method.Dict_AOI_absolNG["電極區白點 (背光)"].DefectReg.Clone();
                                    else
                                        ResRegion = Method.ho_DefectReg_BrightBlob.Clone();
                                    HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                                    Res = true;
                                }
                            }
                            break;
                        case "檢測保缺角 (背光)":
                            {
                                if (Inspection_LackAngle_BL())
                                {
                                    if (tag == "檢測AOI絕對NG")
                                        ResRegion = Method.Dict_AOI_absolNG["保缺角 (背光)"].DefectReg.Clone();
                                    else
                                        ResRegion = Method.ho_DefectReg_LackAngle_BL.Clone();
                                    HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                                    Res = true;
                                }
                            }
                            break;
                        case "檢測保缺角 (側光背光)":
                            {
                                if (Inspection_LackAngle_SL())
                                {
                                    if (tag == "檢測AOI絕對NG")
                                        ResRegion = Method.Dict_AOI_absolNG["保缺角 (側光)"].DefectReg.Clone();
                                    else
                                        ResRegion = Method.ho_DefectReg_LackAngle_SL.Clone();
                                    HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                                    Res = true;
                                }
                            }
                            break;
                    }
                }

                if (!Res)
                {
                    MessageBox.Show("檢測失敗", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 點擊 region 顯示其資訊
                if (tag == "檢測AOI絕對NG")
                    HOperatorSet.SetColor(DisplayWindows.HalconWindow, "blue");
                else
                    HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                HOperatorSet.DispObj(ResRegion, DisplayWindows.HalconWindow);
                Current_disp_regions.Dispose();
                HOperatorSet.Connection(ResRegion, out Current_disp_regions);

                if (tag != "總檢測")
                    b_Initial_State = false;
            }
            catch (Exception ex)
            { 
                string message = "Error: " + ex.Message;
                MessageBox.Show(message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            #endregion
        }

        private void combxAIType_SelectedIndexChanged(object sender, EventArgs e) // (20190806) Jeff Revised!
        {
            ComboBox Obj = (ComboBox)sender;
            if (Obj.Text == "AI覆判NG" && Recipe.DAVSParam.DAVS_Mode != 0) // AI覆判NG & AI啟用 (20190806) Jeff Revised!
                cbx_notSureNG.Enabled = true;
            else
            {
                cbx_notSureNG.Enabled = false;
                cbx_notSureNG.Checked = false;
            }
            ParamGetFromUI();
            if (Obj.SelectedIndex == 1 || Obj.SelectedIndex == 2)
            {
                //if (!Recipe.Param.InnerInspParam.Enabled && !Recipe.Param.OuterInspParam.Enabled)
                //{
                //    MessageBox.Show("AOI不啟用,無法啟用覆判機制");
                //    Obj.SelectedIndex = 0;
                //    return;
                //}
            }
        }

        /// <summary>
        /// 【AI 參數設定】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCellCut_Click(object sender, EventArgs e) // (20190806) Jeff Revised!
        {
            //Chris 20190305
            HObject InputImage;

            if (Input_ImgList.Count <= Recipe.Param.AIImageID || Input_ImgList.Count <= 0)
            {
                HObject H = new HObject();
                HOperatorSet.GenEmptyObj(out H);
                InputImage = H.Clone();
                H.Dispose();
            }
            else
            {
                ParamGetFromUI();
                clear_rbtn_Step(); // (20190216) Jeff Revised!
                Method.set_parameter(Recipe);
                try
                {
                    //Inspection_CellSeg_CL();
                    Inspection_Df_CellReg();
                }
                catch (Exception ex)
                {
                    string message = "Error: " + ex.Message;
                    MessageBox.Show(message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                //InputImage = Input_ImgList[Recipe.Param.AIImageID].Clone(); //Chris 20190305 
                InputImage = Method.Insp_ImgList[Recipe.Param.AIImageID].Clone(); // (20190307) Jeff Revised!
            }

            if (Recipe.DAVS == null)
            {
                Recipe.DAVS = new clsDAVS(Recipe.DAVSParam, Recipe.GetRecipePath());
            }
            FormDAVS MyForm = new FormDAVS(InputImage, Recipe.DAVSParam, Recipe.DAVS, Recipe.GetRecipePath(), Method.ho_InspRects_AllCells, ModuleName, SB_ID, MOVE_ID, PartNumber);
            MyForm.ShowDialog();

            if (MyForm.DialogResult == DialogResult.OK)
            {
                Recipe.DAVSParam = MyForm.GetParam();

                if (Recipe.Param.DAVSInspType == 1 && Recipe.DAVSParam.DAVS_Mode != 0) // AI覆判NG & AI啟用 (20190806) Jeff Revised!
                    cbx_notSureNG.Enabled = true;
                else
                {
                    cbx_notSureNG.Enabled = false;
                    cbx_notSureNG.Checked = false;
                }
                ParamGetFromUI();
            }

            #region Dispose
            InputImage.Dispose();
            #endregion
        }

        #region 【AI只覆判模稜兩可NG】

        private Dictionary<string, cls_GUIControls_notSureNG> dict_GUIControls_notSureNG = new Dictionary<string, cls_GUIControls_notSureNG>();

        /// <summary>
        /// 【AI只覆判模稜兩可NG】之GUI控制項
        /// </summary>
        private class cls_GUIControls_notSureNG // (20190807) Jeff Revised!
        {
            public GroupBox gbx_notSureNG = new GroupBox(); // 【AI只覆判模稜兩可NG】
            public CheckBox cbx_Enabled = new CheckBox(); // 【啟用】
            public TextBox textBox_Info_notSureNG = new TextBox(); // 顯示資訊
            public GroupBox gbx_AbsolNG = new GroupBox(); // 【AOI絕對NG】
            public RichTextBox richTextBox_log = new RichTextBox(); // 【說明】欄位顯示
            public Button btn_instruction_Step1 = new Button(); // 【說明】Step1
            public Button btn_instruction_Step2 = new Button(); // 【說明】Step2
            public RadioButton rbtn_Step1 = new RadioButton();
            public RadioButton rbtn_Step2 = new RadioButton();

            public cls_GUIControls_notSureNG(GroupBox gbx_notSureNG_, CheckBox cbx_Enabled_, TextBox textBox_Info_notSureNG_, GroupBox gbx_AbsolNG_,
                                             RichTextBox richTextBox_log_, Button btn_instruction_Step1_, Button btn_instruction_Step2_, RadioButton rbtn_Step1_, RadioButton rbtn_Step2_)
            {
                #region 設定控制項

                this.gbx_notSureNG = gbx_notSureNG_;
                this.cbx_Enabled = cbx_Enabled_;
                this.textBox_Info_notSureNG = textBox_Info_notSureNG_;
                this.gbx_AbsolNG = gbx_AbsolNG_;
                this.richTextBox_log = richTextBox_log_;
                this.btn_instruction_Step1 = btn_instruction_Step1_;
                this.btn_instruction_Step2 = btn_instruction_Step2_;
                this.rbtn_Step1 = rbtn_Step1_;
                this.rbtn_Step2 = rbtn_Step2_;

                #endregion

                Initialize();
            }

            /// <summary>
            /// 初始化
            /// </summary>
            private void Initialize()
            {
                #region 設定控制項之觸發事件

                this.cbx_Enabled.CheckedChanged += new System.EventHandler(this.cbx_AbsolNG_CheckedChanged);
                this.btn_instruction_Step1.Click += new System.EventHandler(this.btn_instruction_Click);
                this.btn_instruction_Step2.Click += new System.EventHandler(this.btn_instruction_Click);

                #endregion
            }

            /// <summary>
            /// 【啟用】狀態變更事件
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void cbx_AbsolNG_CheckedChanged(object sender, EventArgs e) // (20190807) Jeff Revised!
            {
                if (cbx_Enabled.Checked)
                {
                    this.gbx_AbsolNG.Enabled = true;
                    this.textBox_Info_notSureNG.Text = "AI只覆判模稜兩可NG";
                }
                else
                {
                    this.gbx_AbsolNG.Enabled = false;
                    this.textBox_Info_notSureNG.Text = "此種瑕疵NG皆給AI做覆判";
                }
            }

            /// <summary>
            /// 點擊【說明】按鈕
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void btn_instruction_Click(object sender, EventArgs e) // (20190807) Jeff Revised!
            {
                Button btn = sender as Button;
                if (btn == btn_instruction_Step1)
                {
                    this.rbtn_Step1.Checked = true;
                    richTextBox_log.AppendText("※AOI絕對NG Step1:" + "\n");
                    richTextBox_log.AppendText("min: 瑕疵Region內至少有一個pixel之灰階值低於一個數值" + "\n");
                    richTextBox_log.AppendText("max: 瑕疵Region內至少有一個pixel之灰階值高於一個數值" + "\n\n");
                }
                else if (btn == btn_instruction_Step2)
                {
                    this.rbtn_Step2.Checked = true;
                    richTextBox_log.AppendText("※AOI絕對NG Step2:" + "\n");
                    richTextBox_log.AppendText("瑕疵尺寸篩選" + "\n\n");
                }

                richTextBox_log.ScrollToCaret();
            }

        }

        /// <summary>
        /// 初始化【AI只覆判模稜兩可NG】之GUI控制項 (dict_GUIControls_notSureNG)
        /// </summary>
        private void Initialize_dict_GUIControls_notSureNG() // (20190807) Jeff Revised!
        {
            dict_GUIControls_notSureNG.Add(tp_Line_FL.Tag.ToString(), new cls_GUIControls_notSureNG(gbx_notSureNG_Line_FL, cbx_notSureNG_Line_FL, textBox_Info_notSureNG_Line_FL, gbx_AbsolNG_Line_FL, 
                                                                                                richTextBox_log, btn_Line_FL_Step1_AbsolNG, btn_Line_FL_Step2_AbsolNG, rbtn_Step1_AbsolNG_Line_FL, rbtn_Step2_AbsolNG_Line_FL));
            dict_GUIControls_notSureNG.Add(tp_BlackBlob2_CL.Tag.ToString(), new cls_GUIControls_notSureNG(gbx_notSureNG_BlackBlob2_CL, cbx_notSureNG_BlackBlob2_CL, textBox_Info_notSureNG_BlackBlob2_CL, gbx_AbsolNG_BlackBlob2_CL,
                                                                                                richTextBox_log, btn_BlackBlob2_CL_Step1_AbsolNG, btn_BlackBlob2_CL_Step2_AbsolNG, rbtn_Step1_AbsolNG_BlackBlob2_CL, rbtn_Step2_AbsolNG_BlackBlob2_CL));
            dict_GUIControls_notSureNG.Add(tp_Line_FLCL.Tag.ToString(), new cls_GUIControls_notSureNG(gbx_notSureNG_Line_FLCL, cbx_notSureNG_Line_FLCL, textBox_Info_notSureNG_Line_FLCL, gbx_AbsolNG_Line_FLCL,
                                                                                                richTextBox_log, btn_Line_FLCL_Step1_AbsolNG, btn_Line_FLCL_Step2_AbsolNG, rbtn_Step1_AbsolNG_Line_FLCL, rbtn_Step2_AbsolNG_Line_FLCL));
            dict_GUIControls_notSureNG.Add(tp_WhiteBlob_FL.Tag.ToString(), new cls_GUIControls_notSureNG(gbx_notSureNG_WhiteBlob_FL, cbx_notSureNG_WhiteBlob_FL, textBox_Info_notSureNG_WhiteBlob_FL, gbx_AbsolNG_WhiteBlob_FL,
                                                                                                richTextBox_log, btn_WhiteBlob_FL_Step1_AbsolNG, btn_WhiteBlob_FL_Step2_AbsolNG, rbtn_Step1_AbsolNG_WhiteBlob_FL, rbtn_Step2_AbsolNG_WhiteBlob_FL));
            dict_GUIControls_notSureNG.Add(tp_WhiteBlob_CL.Tag.ToString(), new cls_GUIControls_notSureNG(gbx_notSureNG_WhiteBlob_CL, cbx_notSureNG_WhiteBlob_CL, textBox_Info_notSureNG_WhiteBlob_CL, gbx_AbsolNG_WhiteBlob_CL,
                                                                                                richTextBox_log, btn_WhiteBlob_CL_Step1_AbsolNG, btn_WhiteBlob_CL_Step2_AbsolNG, rbtn_Step1_AbsolNG_WhiteBlob_CL, rbtn_Step2_AbsolNG_WhiteBlob_CL));
            dict_GUIControls_notSureNG.Add(tp_BlackCrack_FL.Tag.ToString(), new cls_GUIControls_notSureNG(gbx_notSureNG_BlackCrack_FL, cbx_notSureNG_BlackCrack_FL, textBox_Info_notSureNG_BlackCrack_FL, gbx_AbsolNG_BlackCrack_FL,
                                                                                                richTextBox_log, btn_BlackCrack_FL_Step1_AbsolNG, btn_BlackCrack_FL_Step2_AbsolNG, rbtn_Step1_AbsolNG_BlackCrack_FL, rbtn_Step2_AbsolNG_BlackCrack_FL));
            dict_GUIControls_notSureNG.Add(tp_BlackCrack_CL.Tag.ToString(), new cls_GUIControls_notSureNG(gbx_notSureNG_BlackCrack_CL, cbx_notSureNG_BlackCrack_CL, textBox_Info_notSureNG_BlackCrack_CL, gbx_AbsolNG_BlackCrack_CL,
                                                                                                richTextBox_log, btn_BlackCrack_CL_Step1_AbsolNG, btn_BlackCrack_CL_Step2_AbsolNG, rbtn_Step1_AbsolNG_BlackCrack_CL, rbtn_Step2_AbsolNG_BlackCrack_CL));
            dict_GUIControls_notSureNG.Add(tp_BlackBlob_CL.Tag.ToString(), new cls_GUIControls_notSureNG(gbx_notSureNG_BlackBlob_CL, cbx_notSureNG_BlackBlob_CL, textBox_Info_notSureNG_BlackBlob_CL, gbx_AbsolNG_BlackBlob_CL,
                                                                                                richTextBox_log, btn_BlackBlob_CL_Step1_AbsolNG, btn_BlackBlob_CL_Step2_AbsolNG, rbtn_Step1_AbsolNG_BlackBlob_CL, rbtn_Step2_AbsolNG_BlackBlob_CL));
            dict_GUIControls_notSureNG.Add(tp_Dirty_CL.Tag.ToString(), new cls_GUIControls_notSureNG(gbx_notSureNG_Dirty_CL, cbx_notSureNG_Dirty_CL, textBox_Info_notSureNG_Dirty_CL, gbx_AbsolNG_Dirty_CL,
                                                                                                richTextBox_log, btn_Dirty_CL_Step1_AbsolNG, btn_Dirty_CL_Step2_AbsolNG, rbtn_Step1_AbsolNG_Dirty_CL, rbtn_Step2_AbsolNG_Dirty_CL));
            dict_GUIControls_notSureNG.Add(tp_Dirty_FLCL.Tag.ToString(), new cls_GUIControls_notSureNG(gbx_notSureNG_Dirty_FLCL, cbx_notSureNG_Dirty_FLCL, textBox_Info_notSureNG_Dirty_FLCL, gbx_AbsolNG_Dirty_FLCL,
                                                                                                richTextBox_log, btn_Dirty_FLCL_Step1_AbsolNG, btn_Dirty_FLCL_Step2_AbsolNG, rbtn_Step1_AbsolNG_Dirty_FLCL, rbtn_Step2_AbsolNG_Dirty_FLCL));
            dict_GUIControls_notSureNG.Add(tp_BrightBlob_BL.Tag.ToString(), new cls_GUIControls_notSureNG(gbx_notSureNG_BrightBlob_BL, cbx_notSureNG_BrightBlob_BL, textBox_Info_notSureNG_BrightBlob_BL, gbx_AbsolNG_BrightBlob_BL,
                                                                                                richTextBox_log, btn_BrightBlob_BL_Step1_AbsolNG, btn_BrightBlob_BL_Step2_AbsolNG, rbtn_Step1_AbsolNG_BrightBlob_BL, rbtn_Step2_AbsolNG_BrightBlob_BL));
            dict_GUIControls_notSureNG.Add(tp_LackAngle_BL.Tag.ToString(), new cls_GUIControls_notSureNG(gbx_notSureNG_LackAngle_BL, cbx_notSureNG_LackAngle_BL, textBox_Info_notSureNG_LackAngle_BL, gbx_AbsolNG_LackAngle_BL,
                                                                                                richTextBox_log, btn_LackAngle_BL_Step1_AbsolNG, btn_LackAngle_BL_Step2_AbsolNG, rbtn_Step1_AbsolNG_LackAngle_BL, rbtn_Step2_AbsolNG_LackAngle_BL));
            dict_GUIControls_notSureNG.Add(tp_LackAngle_SL.Tag.ToString(), new cls_GUIControls_notSureNG(gbx_notSureNG_LackAngle_SL, cbx_notSureNG_LackAngle_SL, textBox_Info_notSureNG_LackAngle_SL, gbx_AbsolNG_LackAngle_SL,
                                                                                                richTextBox_log, btn_LackAngle_SL_Step1_AbsolNG, btn_LackAngle_SL_Step2_AbsolNG, rbtn_Step1_AbsolNG_LackAngle_SL, rbtn_Step2_AbsolNG_LackAngle_SL));
        }

        /// <summary>
        /// 【AI只覆判模稜兩可NG】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbx_notSureNG_CheckedChanged(object sender, EventArgs e) // (20190807) Jeff Revised!
        {
            if (cbx_notSureNG.Checked)
            {
                foreach (cls_GUIControls_notSureNG cls in dict_GUIControls_notSureNG.Values)
                    cls.gbx_notSureNG.Enabled = true;
            }
            else
            {
                foreach (cls_GUIControls_notSureNG cls in dict_GUIControls_notSureNG.Values)
                    cls.gbx_notSureNG.Enabled = false;
            }
        }

        private void btn_notSureNG_Click(object sender, EventArgs e) // (20190806) Jeff Revised!
        {
            richTextBox_log.AppendText("※AI只覆判模稜兩可NG:" + "\n");
            richTextBox_log.AppendText("AOI判斷之絕對NG直接輸出NG，模稜兩可NG給AI做覆判" + "\n\n");
            richTextBox_log.ScrollToCaret();
        }

        /// <summary>
        /// 參數變更 (【AOI絕對NG】)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void param_AbsolNG_ValueChanged(object sender, EventArgs e) // (20190810) Jeff Revised!
        {
            Control c = sender as Control;
            string tag = c.Tag.ToString();

            if (b_LoadRecipe) return;

            if (Input_ImgList.Count <= 0)
            {
                MessageBox.Show("請先載入影像");
                return;
            }

            ParamGetFromUI();
            string str_TabPage = tbInspParamSetting.SelectedTab.Tag.ToString();
            bool result_insp = false;
            switch (str_TabPage)
            {
                case "絲狀異物 (正光)":
                    {
                        result_insp = Inspection_Line_FL();
                    }
                    break;
                case "BlackBlob2 (同軸光)":
                    {
                        result_insp = Inspection_BlackBlob2_CL();
                    }
                    break;
                case "絲狀異物 (正光&同軸光)":
                    {
                        result_insp = Inspection_Line_FLCL();
                    }
                    break;
                case "黑條內白點 (正光)":
                    {
                        result_insp = Inspection_WhiteBlob_FL();
                    }
                    break;
                case "黑條內白點 (同軸光)":
                    {
                        result_insp = Inspection_WhiteBlob_CL();
                    }
                    break;
                case "白條內黑點 (正光)":
                    {
                        result_insp = Inspection_BlackCrack_FL();
                    }
                    break;
                case "白條內黑點 (同軸光)":
                    {
                        result_insp = Inspection_BlackCrack_CL();
                    }
                    break;
                case "BlackBlob (同軸光)":
                    {
                        if (Method.Read_AllImages_Insp(Recipe, Input_ImgList))
                        {
                            if (Method.InitialPosition_Insp(Recipe))
                            {
                                if (cbx_CellSeg_CL.Checked) // (20190810) Jeff Revised!
                                {
                                    if (!(Method.CellSeg_CL_Insp(Recipe))) break;
                                }
                                else
                                {
                                    if (!(Method.CellSeg_BL_Insp(Recipe))) break;
                                }
                                if (!(Method.Df_CellReg(Recipe))) break; // (20190810) Jeff Revised!
                                if (Method.BlackBlob_CL_Insp(Recipe))
                                    result_insp = true;
                            }
                        }
                    }
                    break;
                case "汙染 (同軸光)":
                    {
                        result_insp = Inspection_Dirty_CL();
                    }
                    break;
                case "汙染 (正光&同軸光)":
                    {
                        if (Method.Read_AllImages_Insp(Recipe, Input_ImgList))
                        {
                            if (Method.InitialPosition_Insp(Recipe))
                            {
                                if (cbx_CellSeg_CL.Checked) // (20190810) Jeff Revised!
                                {
                                    if (!(Method.CellSeg_CL_Insp(Recipe))) break;
                                }
                                else
                                {
                                    if (!(Method.CellSeg_BL_Insp(Recipe))) break;
                                }
                                if (!(Method.Df_CellReg(Recipe))) break; // (20190810) Jeff Revised!
                                if (Method.Dirty_FLCL_Insp(Recipe))
                                    result_insp = true;
                            }
                        }
                    }
                    break;
                case "電極區白點 (背光)":
                    {
                        result_insp = Inspection_BrightBlob_BL();
                    }
                    break;
                case "保缺角 (背光)":
                    {
                        result_insp = Inspection_LackAngle_BL();
                    }
                    break;
                case "保缺角 (側光)":
                    {
                        result_insp = Inspection_LackAngle_SL();
                    }
                    break;
            }
            if (result_insp)
                b_Initial_State = false;
            else
            {
                MessageBox.Show("檢測失敗", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            switch (tag)
            {
                case "Step1_AbsolNG":
                    {
                        if (dict_GUIControls_notSureNG[str_TabPage].rbtn_Step1.Checked) rbtn_AbsolNG_CheckedChanged(dict_GUIControls_notSureNG[str_TabPage].rbtn_Step1, e);
                        else dict_GUIControls_notSureNG[str_TabPage].rbtn_Step1.Checked = true;
                    }
                    break;
                case "Step2_AbsolNG":
                    {
                        if (dict_GUIControls_notSureNG[str_TabPage].rbtn_Step2.Checked) rbtn_AbsolNG_CheckedChanged(dict_GUIControls_notSureNG[str_TabPage].rbtn_Step2, e);
                        else dict_GUIControls_notSureNG[str_TabPage].rbtn_Step2.Checked = true;
                    }
                    break;
            }
        }

        private void rbtn_AbsolNG_CheckedChanged(object sender, EventArgs e) // (20190810) Jeff Revised!
        {
            RadioButton rbtn = sender as RadioButton;
            if (rbtn.Checked == false) return;

            if (Input_ImgList.Count <= 0)
            {
                rbtn.Checked = false;
                MessageBox.Show("請先載入影像");
                return;
            }

            string str_TabPage = tbInspParamSetting.SelectedTab.Tag.ToString();
            int dispImageIndex = 0;
            bool result_insp = false;
            switch (str_TabPage)
            {
                case "絲狀異物 (正光)":
                    {
                        if (b_Initial_State)
                            result_insp = Inspection_Line_FL();
                        dispImageIndex = int.Parse(nud_ImageID_Line_FL.Value.ToString());
                    }
                    break;
                case "BlackBlob2 (同軸光)":
                    {
                        if (b_Initial_State)
                            result_insp = Inspection_BlackBlob2_CL();
                        dispImageIndex = int.Parse(nud_ImageID_BlackBlob2_CL.Value.ToString());
                    }
                    break;
                case "絲狀異物 (正光&同軸光)":
                    {
                        if (b_Initial_State)
                            result_insp = Inspection_Line_FLCL();
                        dispImageIndex = int.Parse(nud_ImageID_Line_FL.Value.ToString());
                    }
                    break;
                case "黑條內白點 (正光)":
                    {
                        if (b_Initial_State)
                            result_insp = Inspection_WhiteBlob_FL();
                        dispImageIndex = int.Parse(nud_ImageID_WhiteBlob_FL.Value.ToString());
                    }
                    break;
                case "黑條內白點 (同軸光)":
                    {
                        if (b_Initial_State)
                            result_insp = Inspection_WhiteBlob_CL();
                        dispImageIndex = int.Parse(nud_ImageID_WhiteBlob_CL.Value.ToString());
                    }
                    break;
                case "白條內黑點 (正光)":
                    {
                        if (b_Initial_State)
                            result_insp = Inspection_BlackCrack_FL();
                        dispImageIndex = int.Parse(nud_ImageID_BlackCrack_FL.Value.ToString());
                    }
                    break;
                case "白條內黑點 (同軸光)":
                    {
                        if (b_Initial_State)
                            result_insp = Inspection_BlackCrack_CL();
                        dispImageIndex = int.Parse(nud_ImageID_BlackCrack_CL.Value.ToString());
                    }
                    break;
                case "BlackBlob (同軸光)":
                    {
                        if (b_Initial_State)
                        {
                            if (Method.Read_AllImages_Insp(Recipe, Input_ImgList))
                            {
                                if (Method.InitialPosition_Insp(Recipe))
                                {
                                    if (cbx_CellSeg_CL.Checked) // (20190810) Jeff Revised!
                                    {
                                        if (!(Method.CellSeg_CL_Insp(Recipe))) break;
                                    }
                                    else
                                    {
                                        if (!(Method.CellSeg_BL_Insp(Recipe))) break;
                                    }
                                    if (!(Method.Df_CellReg(Recipe))) break; // (20190810) Jeff Revised!
                                    if (Method.BlackBlob_CL_Insp(Recipe))
                                        result_insp = true;
                                }
                            }
                        }
                        dispImageIndex = int.Parse(nud_ImageID_BlackBlob_CL.Value.ToString());
                    }
                    break;
                case "汙染 (同軸光)":
                    {
                        if (b_Initial_State)
                            result_insp = Inspection_Dirty_CL();
                        dispImageIndex = int.Parse(nud_ImageID_Dirty_CL.Value.ToString());
                    }
                    break;
                case "汙染 (正光&同軸光)":
                    {
                        if (b_Initial_State)
                        {
                            if (Method.Read_AllImages_Insp(Recipe, Input_ImgList))
                            {
                                if (Method.InitialPosition_Insp(Recipe))
                                {
                                    if (cbx_CellSeg_CL.Checked) // (20190810) Jeff Revised!
                                    {
                                        if (!(Method.CellSeg_CL_Insp(Recipe))) break;
                                    }
                                    else
                                    {
                                        if (!(Method.CellSeg_BL_Insp(Recipe))) break;
                                    }
                                    if (!(Method.Df_CellReg(Recipe))) break; // (20190810) Jeff Revised!
                                    if (Method.Dirty_FLCL_Insp(Recipe))
                                        result_insp = true;
                                }
                            }
                        }
                        dispImageIndex = int.Parse(nud_ImageID1_Dirty_FLCL.Value.ToString());
                    }
                    break;
                case "電極區白點 (背光)":
                    {
                        if (b_Initial_State)
                            result_insp = Inspection_BrightBlob_BL();
                        dispImageIndex = int.Parse(nud_ImageID_BrightBlob_BL.Value.ToString());
                    }
                    break;
                case "保缺角 (背光)":
                    {
                        if (b_Initial_State)
                            result_insp = Inspection_LackAngle_BL();
                        dispImageIndex = int.Parse(nud_ImageID_LackAngle_BL.Value.ToString());
                    }
                    break;
                case "保缺角 (側光)":
                    {
                        if (b_Initial_State)
                            result_insp = Inspection_LackAngle_SL();
                        dispImageIndex = int.Parse(nud_ImageID_LackAngle_SL.Value.ToString());
                    }
                    break;
            }

            if (b_Initial_State)
            {
                if (result_insp)
                    b_Initial_State = false;
                else
                {
                    rbtn.Checked = false;
                    MessageBox.Show("檢測失敗", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

            }
            
            combxDisplayImg.SelectedIndex = dispImageIndex;
            Current_disp_regions.Dispose();
            HOperatorSet.GenEmptyObj(out Current_disp_regions);
            Current_disp_image = Method.Insp_ImgList[dispImageIndex].Clone();
            ClearDisplay(Current_disp_image);
            string tag = rbtn.Tag.ToString();
            switch (tag)
            {
                case "Step1_AbsolNG":
                    {
                        if (Method.Reg_SelectGray_AbsolNG != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.Connection(Method.Reg_SelectGray_AbsolNG, out Current_disp_regions);
                        }
                    }
                    break;
                case "Step2_AbsolNG":
                    {
                        if (Method.Dict_AOI_absolNG[str_TabPage].DefectReg != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.Connection(Method.Dict_AOI_absolNG[str_TabPage].DefectReg, out Current_disp_regions);
                        }
                    }
                    break;
            }
            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "blue");
            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
        }

        /// <summary>
        /// 清除 【AOI絕對NG】 內所有 rbtn_Step
        /// </summary>
        private void clear_rbtn_Step_AbsolNG() // (20190810) Jeff Revised!
        {
            foreach (cls_GUIControls_notSureNG cls in dict_GUIControls_notSureNG.Values)
            {
                cls.rbtn_Step1.Checked = false;
                cls.rbtn_Step2.Checked = false;
            }
        }

        #endregion

        private void cbxTestModeEnabled_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox Obj = (CheckBox)sender;
            combxTestModeType.Enabled = Obj.Checked;
        }

        private void cbxSaveImgEnable_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox Obj = (CheckBox)sender;
            combxSaveImgType.Enabled = Obj.Checked;
        }

        private void DisplayWindows_HMouseDown(object sender, HMouseEventArgs e) // (20200729) Jeff Revised!
        {
            if (this.Current_disp_regions == null || this.Current_disp_image == null)
                return;
            
            HTuple RegionCount;
            HOperatorSet.CountObj(this.Current_disp_regions, out RegionCount);
            if (RegionCount > 0)
            {
                HTuple hv_Index = new HTuple(); // (20200729) Jeff Revised!
                HOperatorSet.GetRegionIndex(this.Current_disp_regions, (int)(e.Y + 0.5), (int)(e.X + 0.5), out hv_Index);
                if (hv_Index.Length > 0)
                {
                    HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                    HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                    HOperatorSet.DispObj(this.Current_disp_image, DisplayWindows.HalconWindow);
                    HOperatorSet.DispObj(this.Current_disp_regions, DisplayWindows.HalconWindow);

                    HOperatorSet.SetColor(DisplayWindows.HalconWindow, "blue");
                    HOperatorSet.DispObj(this.Current_disp_regions[hv_Index], DisplayWindows.HalconWindow);

                    HTuple Area, Width, Height;
                    HOperatorSet.RegionFeatures(this.Current_disp_regions[hv_Index], "area", out Area);
                    HOperatorSet.RegionFeatures(this.Current_disp_regions[hv_Index], "width", out Width);
                    HOperatorSet.RegionFeatures(this.Current_disp_regions[hv_Index], "height", out Height);

                    this.labW.Text = ((double)(Width * Locate_Method_FS.pixel_resolution_)).ToString("#0.000");
                    this.labH.Text = ((double)(Height * Locate_Method_FS.pixel_resolution_)).ToString("#0.000");
                    this.labA.Text = ((double)(Area * Locate_Method_FS.pixel_resolution_ * Locate_Method_FS.pixel_resolution_)).ToString("#0.000");
                    
                    // 顯示灰階範圍 (20190220) Jeff Revised!
                    HTuple min, max, range;
                    //HOperatorSet.MinMaxGray(Current_disp_regions[hv_Index], Current_disp_image, 0, out min, out max, out range);
                    // (20190529) Jeff Revised!
                    HObject GrayImg;
                    HOperatorSet.Rgb1ToGray(this.Current_disp_image, out GrayImg);
                    HOperatorSet.MinMaxGray(Current_disp_regions[hv_Index], GrayImg, 0, out min, out max, out range);
                    GrayImg.Dispose();

                    this.labR.Text = min.ToString() + " ~ " + max.ToString();
                }
            }
        }

        private void DisplayWindows_HMouseMove(object sender, HMouseEventArgs e) // (20190218) Jeff Revised!
        {
            if (Current_disp_image == null) return;
            try
            {
                HTuple width = null, height = null;
                HOperatorSet.GetImageSize(Current_disp_image, out width, out height);
                //txt_CursorCoordinate.Text = "(" + String.Format("{0:#}", e.X) + ", " + String.Format("{0:#}", e.Y) + ")";
                txt_CursorCoordinate.Text = "(" + String.Format("{0:0}", e.X) + ", " + String.Format("{0:0}", e.Y) + ")"; // (20190815) Jeff Revised!
                try
                {
                    if (e.X >= 0 && e.X < width && e.Y >= 0 && e.Y < height)
                    {
                        HTuple grayval;
                        HOperatorSet.GetGrayval(Current_disp_image, (int)(e.Y + 0.5), (int)(e.X + 0.5), out grayval);
                        txt_ColorValue.Text = (grayval.TupleInt()).ToString();
                        HObject GrayImg;
                        HOperatorSet.Rgb1ToGray(Current_disp_image, out GrayImg);
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

        #region 初定位

        private void btn_instruction_SegBlackStripe_Click(object sender, EventArgs e) // (20190816) Jeff Revised!
        {
            Button btn = sender as Button;
            string tag = btn.Tag.ToString();
            switch (tag)
            {
                case "Step1":
                    {
                        rbtn_Step1_SegBlackStripe.Checked = true;
                        richTextBox_log.AppendText("※分割電阻條 Step1:" + "\n");
                        richTextBox_log.AppendText("二值化處理 (固定閥值)" + "\n\n");
                    }
                    break;
                case "Step2":
                    {
                        rbtn_Step2_SegBlackStripe.Checked = true;
                        richTextBox_log.AppendText("※分割電阻條 Step2:" + "\n");
                        richTextBox_log.AppendText("閉運算處理，用以填補 縫隙 & 孔洞" + "\n\n");
                    }
                    break;
                case "Step3":
                    {
                        rbtn_Step3_SegBlackStripe.Checked = true;
                        richTextBox_log.AppendText("※分割電阻條 Step3:" + "\n");
                        richTextBox_log.AppendText("開運算處理，用以消除誤判區域" + "\n\n");
                    }
                    break;
                case "Step4":
                    {
                        rbtn_Step4_SegBlackStripe.Checked = true;
                        richTextBox_log.AppendText("※分割電阻條 Step4:" + "\n");
                        richTextBox_log.AppendText("電阻條尺寸篩選" + "\n\n");
                    }
                    break;
                case "Step5":
                    {
                        rbtn_Step5_SegBlackStripe.Checked = true;
                        richTextBox_log.AppendText("※分割電阻條 Step5:" + "\n");
                        richTextBox_log.AppendText("膨脹處理，用以填滿電阻條區域" + "\n\n");
                    }
                    break;
                case "Step6":
                    {
                        rbtn_Step6_SegBlackStripe.Checked = true;
                        richTextBox_log.AppendText("※分割電阻條 Step6:" + "\n");
                        richTextBox_log.AppendText("電阻條寬度縮減，用於Cell分割參考區域" + "\n\n");
                    }
                    break;
                case "Step7":
                    {
                        rbtn_Step7_SegBlackStripe.Checked = true;
                        richTextBox_log.AppendText("※分割電阻條 Step7:" + "\n");
                        richTextBox_log.AppendText("電阻條忽略長度，用於設定檢測ROI" + "\n\n");
                    }
                    break;
            }

            richTextBox_log.ScrollToCaret();
        }

        private void rbtn_Step_SegBlackStripe_CheckedChanged(object sender, EventArgs e) // (20190816) Jeff Revised!
        {
            RadioButton rbtn = sender as RadioButton;
            if (rbtn.Checked == false) return;

            if (Input_ImgList.Count <= 0)
            {
                rbtn.Checked = false;
                MessageBox.Show("請先載入影像");
                return;
            }

            if (b_Initial_State)
            {
                if (Inspection_InitialPosition()) b_Initial_State = false;
                else
                {
                    rbtn.Checked = false;
                    MessageBox.Show("檢測失敗", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            combxDisplayImg.SelectedIndex = int.Parse(nud_ImageIndex_SegBlackStripe.Value.ToString());
            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
            Extension.HObjectMedthods.ReleaseHObject(ref Current_disp_image); // (20190816) Jeff Revised!
            Current_disp_regions.Dispose();
            HOperatorSet.GenEmptyObj(out Current_disp_regions);
            string tag = rbtn.Tag.ToString();
            switch (tag)
            {
                case "Step1":
                    {
                        Current_disp_image = Method.ho_ImageReduced_FL.Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_Regions_BlackStripe_thresh_FL != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.Connection(Method.ho_Regions_BlackStripe_thresh_FL, out Current_disp_regions);
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step2":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageIndex_SegBlackStripe.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_Regions_BlackStripe_closing != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.Connection(Method.ho_Regions_BlackStripe_closing, out Current_disp_regions);
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step3":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageIndex_SegBlackStripe.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_Regions_BlackStripe_opening != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.Connection(Method.ho_Regions_BlackStripe_opening, out Current_disp_regions);
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step4":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageIndex_SegBlackStripe.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_Regions_BlackStripe_select != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.Connection(Method.ho_Regions_BlackStripe_select, out Current_disp_regions);
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step5":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageIndex_SegBlackStripe.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_Regions_BlackStripe != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.Connection(Method.ho_Regions_BlackStripe, out Current_disp_regions);
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step6":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageIndex_SegBlackStripe.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_Regions_BlackStripe_small != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.Connection(Method.ho_Regions_BlackStripe_small, out Current_disp_regions);
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step7":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageIndex_SegBlackStripe.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_Regions_BlackStripe != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.Connection(Method.ho_Regions_BlackStripe, out Current_disp_regions);
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Regions_BlackStripe_Wsmall != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.Connection(Method.ho_Regions_BlackStripe_Wsmall, out Current_disp_regions);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "green");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Regions_BlackStripe_Wsmall3 != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.Connection(Method.ho_Regions_BlackStripe_Wsmall3, out Current_disp_regions);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "blue");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
            }
        }

        private bool Inspection_InitialPosition()
        {
            if (!(Method.Read_AllImages_Insp(Recipe, Input_ImgList))) return false;
            if (!(Method.InitialPosition_Insp(Recipe))) return false;
            return true;
        }

        private void param_SegBlackStripe_ValueChanged(object sender, EventArgs e) // (20190816) Jeff Revised!
        {
            Control c = sender as Control;
            string tag = c.Tag.ToString();

            if (b_LoadRecipe) return;

            if (Input_ImgList.Count <= 0)
            {
                MessageBox.Show("請先載入影像");
                return;
            }

            ParamGetFromUI();
            if (!Inspection_InitialPosition())
            {
                clear_rbtn_Step_SegBlackStripe();
                MessageBox.Show("檢測失敗", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            b_Initial_State = false;

            switch (tag)
            {
                case "ImageID":
                    {

                    }
                    break;
                case "Step1":
                    {
                        if (rbtn_Step1_SegBlackStripe.Checked) rbtn_Step_SegBlackStripe_CheckedChanged(rbtn_Step1_SegBlackStripe, e);
                        else rbtn_Step1_SegBlackStripe.Checked = true;
                    }
                    break;
                case "Step2":
                    {
                        if (rbtn_Step2_SegBlackStripe.Checked) rbtn_Step_SegBlackStripe_CheckedChanged(rbtn_Step2_SegBlackStripe, e);
                        else rbtn_Step2_SegBlackStripe.Checked = true;
                    }
                    break;
                case "Step3":
                    {
                        if (rbtn_Step3_SegBlackStripe.Checked) rbtn_Step_SegBlackStripe_CheckedChanged(rbtn_Step3_SegBlackStripe, e);
                        else rbtn_Step3_SegBlackStripe.Checked = true;
                    }
                    break;
                case "Step4":
                    {
                        if (rbtn_Step4_SegBlackStripe.Checked) rbtn_Step_SegBlackStripe_CheckedChanged(rbtn_Step4_SegBlackStripe, e);
                        else rbtn_Step4_SegBlackStripe.Checked = true;
                    }
                    break;
                case "Step5":
                    {
                        if (rbtn_Step5_SegBlackStripe.Checked) rbtn_Step_SegBlackStripe_CheckedChanged(rbtn_Step5_SegBlackStripe, e);
                        else rbtn_Step5_SegBlackStripe.Checked = true;
                    }
                    break;
                case "Step6":
                    {
                        if (rbtn_Step6_SegBlackStripe.Checked) rbtn_Step_SegBlackStripe_CheckedChanged(rbtn_Step6_SegBlackStripe, e);
                        else rbtn_Step6_SegBlackStripe.Checked = true;
                    }
                    break;
                case "Step7":
                    {
                        if (rbtn_Step7_SegBlackStripe.Checked) rbtn_Step_SegBlackStripe_CheckedChanged(rbtn_Step7_SegBlackStripe, e);
                        else rbtn_Step7_SegBlackStripe.Checked = true;
                    }
                    break;
            }
        }

        /// <summary>
        /// 清除 【分割電阻條】 內所有 rbtn_Step
        /// </summary>
        private void clear_rbtn_Step_SegBlackStripe() // (20190816) Jeff Revised!
        {
            rbtn_Step1_SegBlackStripe.Checked = false;
            rbtn_Step2_SegBlackStripe.Checked = false;
            rbtn_Step3_SegBlackStripe.Checked = false;
            rbtn_Step4_SegBlackStripe.Checked = false;
            rbtn_Step5_SegBlackStripe.Checked = false;
            rbtn_Step6_SegBlackStripe.Checked = false;
            rbtn_Step7_SegBlackStripe.Checked = false;
        }

        #endregion

        #region Cell分割 (同軸光)

        private void btn_instruction_CellSeg_CL_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string tag = btn.Tag.ToString();
            switch (tag)
            {
                case "Step1":
                    {
                        rbtn_Step1_CellSeg_CL.Checked = true;
                        richTextBox_log.AppendText("※Cell分割 (同軸光) Step1:" + "\n");
                        richTextBox_log.AppendText("將瑕疵區域(亮區)先消除" + "\n\n");
                    }
                    break;
                case "Step2":
                    {
                        rbtn_Step2_CellSeg_CL.Checked = true;
                        richTextBox_log.AppendText("※Cell分割 (同軸光) Step2:" + "\n");
                        richTextBox_log.AppendText("分區做直方圖等化 (Note: 黑條方向之分區數量為黑條之條數)" + "\n\n");
                    }
                    break;
                case "Step3":
                    {
                        rbtn_Step3_CellSeg_CL.Checked = true;
                        richTextBox_log.AppendText("※Cell分割 (同軸光) Step3:" + "\n");
                        richTextBox_log.AppendText("找出Cell (對分區做直方圖等化之結果做二值化處理)" + "\n\n");
                    }
                    break;
                case "Step4":
                    {
                        rbtn_Step4_CellSeg_CL.Checked = true;
                        richTextBox_log.AppendText("※Cell分割 (同軸光) Step4:" + "\n");
                        richTextBox_log.AppendText("開運算處理，用以消除Cell間相連之誤判" + "\n\n");
                    }
                    break;
                case "Step5":
                    {
                        rbtn_Step5_CellSeg_CL.Checked = true;
                        richTextBox_log.AppendText("※Cell分割 (同軸光) Step5:" + "\n");
                        richTextBox_log.AppendText("閉運算處理，用以連接同一Cell內區域" + "\n\n");
                    }
                    break;
                case "Step6":
                    {
                        rbtn_Step6_CellSeg_CL.Checked = true;
                        richTextBox_log.AppendText("※Cell分割 (同軸光) Step6:" + "\n");
                        richTextBox_log.AppendText("開運算處理，用以消除Cell間相連之誤判" + "\n\n");
                    }
                    break;
                case "Step7":
                    {
                        rbtn_Step7_CellSeg_CL.Checked = true;
                        richTextBox_log.AppendText("※Cell分割 (同軸光) Step7:" + "\n");
                        richTextBox_log.AppendText("膨脹處理，用以將各顆Cell的高，填滿電阻區域" + "\n\n");
                    }
                    break;
                case "Step8":
                    {
                        rbtn_Step8_CellSeg_CL.Checked = true;
                        richTextBox_log.AppendText("※Cell分割 (同軸光) Step8:" + "\n");
                        richTextBox_log.AppendText("Cell尺寸篩選" + "\n\n");
                    }
                    break;
                case "Step9":
                    {
                        rbtn_Step9_CellSeg_CL.Checked = true;
                        richTextBox_log.AppendText("※Cell分割 (同軸光) Step9:" + "\n");
                        richTextBox_log.AppendText("顯示檢測出之Cell" + "\n\n");
                    }
                    break;
                case "Step10":
                    {
                        rbtn_Step10_CellSeg_CL.Checked = true;
                        richTextBox_log.AppendText("※Cell分割 (同軸光) Step10:" + "\n");
                        richTextBox_log.AppendText("計算所有Cell區域 (Note: 綠色矩形代表檢測出之Cell位置)" + "\n\n");
                    }
                    break;
            }

            richTextBox_log.ScrollToCaret();
        }

        private void rbtn_Step_CellSeg_CL_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbtn = sender as RadioButton;
            if (rbtn.Checked == false) return;

            if (Input_ImgList.Count <= 0)
            {
                rbtn.Checked = false;
                MessageBox.Show("請先載入影像");
                return;
            }

            if (b_Initial_State)
            {
                if (Inspection_CellSeg_CL()) b_Initial_State = false;
                else
                {
                    rbtn.Checked = false;
                    MessageBox.Show("檢測失敗", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            combxDisplayImg.SelectedIndex = int.Parse(nud_ImageID_CellSeg_CL.Value.ToString());
            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
            Extension.HObjectMedthods.ReleaseHObject(ref Current_disp_image); // (20190816) Jeff Revised!
            Current_disp_regions.Dispose();
            HOperatorSet.GenEmptyObj(out Current_disp_regions);
            string tag = rbtn.Tag.ToString();
            switch (tag)
            {
                case "Step1":
                    {
                        Current_disp_image = Method.ho_ImageReduced_BlackStripe_CL.Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_Regions_removeDef != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.Connection(Method.ho_Regions_removeDef, out Current_disp_regions);
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step2":
                    {
                        Current_disp_image = Method.ho_TiledImage_ALL.Clone();
                        ClearDisplay(Current_disp_image);
                    }
                    break;
                case "Step3":
                    {
                        Current_disp_image = Method.ho_TiledImage_ALL.Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_CellReg_Resist_ALL != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.Connection(Method.ho_CellReg_Resist_ALL, out Current_disp_regions);
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step4":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_CellSeg_CL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_RegionOpening_ALL != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.Connection(Method.ho_RegionOpening_ALL, out Current_disp_regions);
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step5":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_CellSeg_CL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_RegionClosing_ALL != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.Connection(Method.ho_RegionClosing_ALL, out Current_disp_regions);
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step6":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_CellSeg_CL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_RegionOpening1_ALL != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.Connection(Method.ho_RegionOpening1_ALL, out Current_disp_regions);
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step7":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_CellSeg_CL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_RegionIntersection_ALL != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.Connection(Method.ho_RegionIntersection_ALL, out Current_disp_regions);
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step8":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_CellSeg_CL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_SelectedRegions_ALL != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.CopyObj(Method.ho_SelectedRegions_ALL, out Current_disp_regions, 1, -1);
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step9":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_CellSeg_CL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_All_ValidCells_rect != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.Connection(Method.ho_All_ValidCells_rect, out Current_disp_regions);
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_FirstCell_pt != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "blue");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispCircle(DisplayWindows.HalconWindow, Method.hv_y_FirstCell, Method.hv_x_FirstCell, 3);
                        }
                    }
                    break;
                case "Step10":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_CellSeg_CL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_AllCells_Rect != null && Method.ho_AllCells_Pt != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.Connection(Method.ho_AllCells_Rect, out Current_disp_regions);
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);

                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "blue");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Method.ho_AllCells_Pt, DisplayWindows.HalconWindow);
                        }

                        if (cbx_Enabled_RemainValidCells_CellSeg_CL.Checked)
                        {
                            if (Method.ho_All_ValidCells_rect != null)
                            {
                                HOperatorSet.SetColor(DisplayWindows.HalconWindow, "green");
                                HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                                HOperatorSet.DispObj(Method.ho_All_ValidCells_rect, DisplayWindows.HalconWindow);
                            }
                        }
                    }
                    break;
            }
        }

        private bool Inspection_CellSeg_CL()
        {
            if (!(Method.Read_AllImages_Insp(Recipe, Input_ImgList))) return false;
            if (!(Method.InitialPosition_Insp(Recipe))) return false;
            Method.CellSeg_CL_Insp(Recipe);
            //if (!(Method.CellSeg_CL_Insp(Recipe))) return false;
            return true;
        }

        private void param_CellSeg_CL_ValueChanged(object sender, EventArgs e)
        {
            Control c = sender as Control;
            string tag = c.Tag.ToString();
            // Visible狀態切換
            if (tag == "str_BinaryImg")
            {
                string str_BinaryImg = c.Text;
                if (str_BinaryImg == "固定閥值")
                {
                    gbx_ConstThresh_CellSeg_CL.Visible = true;
                    gbx_DynThresh_CellSeg_CL.Visible = false;
                }
                else if (str_BinaryImg == "動態閥值")
                {
                    gbx_ConstThresh_CellSeg_CL.Visible = false;
                    gbx_DynThresh_CellSeg_CL.Visible = true;
                }
                tag = "Step3"; // (20190412) Jeff Revised!
            }

            // Enabled狀態切換
            else if (tag == "Enabled_equHisto") // (20191112) Jeff Revised!
            {
                gbx_equHisto_part_CellSeg_CL.Enabled = (sender as CheckBox).Checked;
                tag = "Step2";
            }
            else if (tag == "Enabled_equHisto_part") // (20191112) Jeff Revised!
            {
                nud_count_col_CellSeg_CL.Enabled = (sender as CheckBox).Checked;
                tag = "Step2";
            }

            if (b_LoadRecipe) return;

            if (Input_ImgList.Count <= 0)
            {
                MessageBox.Show("請先載入影像");
                return;
            }

            //string tag = "";
            //int value;
            //bool cb_checked = false;
            //if (sender is NumericUpDown)
            //{
            //    NumericUpDown nud = sender as NumericUpDown;

            //    // 非數字，則直接return
            //    if (!(int.TryParse(nud.Value.ToString(), out value)))
            //        return;
            //    tag = nud.Tag.ToString();
            //}
            //else if (sender is CheckBox)
            //{
            //    CheckBox cb = sender as CheckBox;
            //    cb_checked = cb.Checked;
            //    tag = cb.Tag.ToString();
            //}

            ParamGetFromUI();
            if (!Inspection_CellSeg_CL())
            {
                clear_rbtn_Step_CellSeg_CL();
                MessageBox.Show("檢測失敗", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            b_Initial_State = false;
            
            switch (tag)
            {
                case "ImageID":
                    {

                    }
                    break;
                case "Step1":
                    {
                        if (rbtn_Step1_CellSeg_CL.Checked) rbtn_Step_CellSeg_CL_CheckedChanged(rbtn_Step1_CellSeg_CL, e);
                        else rbtn_Step1_CellSeg_CL.Checked = true;
                    }
                    break;
                case "Step2":
                    {
                        if (rbtn_Step2_CellSeg_CL.Checked) rbtn_Step_CellSeg_CL_CheckedChanged(rbtn_Step2_CellSeg_CL, e);
                        else rbtn_Step2_CellSeg_CL.Checked = true;
                    }
                    break;
                case "Step3":
                    {
                        if (rbtn_Step3_CellSeg_CL.Checked) rbtn_Step_CellSeg_CL_CheckedChanged(rbtn_Step3_CellSeg_CL, e);
                        else rbtn_Step3_CellSeg_CL.Checked = true;
                    }
                    break;
                case "Step4":
                    {
                        if (rbtn_Step4_CellSeg_CL.Checked) rbtn_Step_CellSeg_CL_CheckedChanged(rbtn_Step4_CellSeg_CL, e);
                        else rbtn_Step4_CellSeg_CL.Checked = true;
                    }
                    break;
                case "Step5":
                    {
                        if (rbtn_Step5_CellSeg_CL.Checked) rbtn_Step_CellSeg_CL_CheckedChanged(rbtn_Step5_CellSeg_CL, e);
                        else rbtn_Step5_CellSeg_CL.Checked = true;
                    }
                    break;
                case "Step6":
                    {
                        if (rbtn_Step6_CellSeg_CL.Checked) rbtn_Step_CellSeg_CL_CheckedChanged(rbtn_Step6_CellSeg_CL, e);
                        else rbtn_Step6_CellSeg_CL.Checked = true;
                    }
                    break;
                case "Step7":
                    {
                        if (rbtn_Step7_CellSeg_CL.Checked) rbtn_Step_CellSeg_CL_CheckedChanged(rbtn_Step7_CellSeg_CL, e);
                        else rbtn_Step7_CellSeg_CL.Checked = true;
                    }
                    break;
                case "Step8":
                    {
                        if (rbtn_Step8_CellSeg_CL.Checked) rbtn_Step_CellSeg_CL_CheckedChanged(rbtn_Step8_CellSeg_CL, e);
                        else rbtn_Step8_CellSeg_CL.Checked = true;
                    }
                    break;
                case "Step9":
                    {
                        if (rbtn_Step9_CellSeg_CL.Checked) rbtn_Step_CellSeg_CL_CheckedChanged(rbtn_Step9_CellSeg_CL, e);
                        else rbtn_Step9_CellSeg_CL.Checked = true;
                    }
                    break;
                case "Step10":
                    {
                        if (rbtn_Step10_CellSeg_CL.Checked) rbtn_Step_CellSeg_CL_CheckedChanged(rbtn_Step10_CellSeg_CL, e);
                        else rbtn_Step10_CellSeg_CL.Checked = true;
                    }
                    break;
            }
        }

        /// <summary>
        /// 清除 Cell分割 (同軸光) 內所有 rbtn_Step
        /// </summary>
        private void clear_rbtn_Step_CellSeg_CL()
        {
            rbtn_Step1_CellSeg_CL.Checked = false;
            rbtn_Step2_CellSeg_CL.Checked = false;
            rbtn_Step3_CellSeg_CL.Checked = false;
            rbtn_Step4_CellSeg_CL.Checked = false;
            rbtn_Step5_CellSeg_CL.Checked = false;
            rbtn_Step6_CellSeg_CL.Checked = false;
            rbtn_Step7_CellSeg_CL.Checked = false;
            rbtn_Step8_CellSeg_CL.Checked = false;
            rbtn_Step9_CellSeg_CL.Checked = false;
            rbtn_Step10_CellSeg_CL.Checked = false;
        }

        #endregion

        #region Cell分割 (背光)
        private void btn_instruction_CellSeg_BL_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string tag = btn.Tag.ToString();
            switch (tag)
            {
                case "Step1":
                    {
                        rbtn_Step1_CellSeg_BL.Checked = true;
                        richTextBox_log.AppendText("※Cell分割 (背光) Step1:" + "\n");
                        richTextBox_log.AppendText("將瑕疵區域(亮區)先消除" + "\n\n");
                    }
                    break;
                case "Step2":
                    {
                        rbtn_Step2_CellSeg_BL.Checked = true;
                        richTextBox_log.AppendText("※Cell分割 (背光) Step2:" + "\n");
                        richTextBox_log.AppendText("分區做直方圖等化 (Note: 黑條方向之分區數量為黑條之條數)" + "\n\n");
                    }
                    break;
                case "Step3":
                    {
                        rbtn_Step3_CellSeg_BL.Checked = true;
                        richTextBox_log.AppendText("※Cell分割 (背光) Step3:" + "\n");
                        richTextBox_log.AppendText("找出Cell (對分區做直方圖等化之結果做二值化處理)" + "\n\n");
                    }
                    break;
                case "Step4":
                    {
                        rbtn_Step4_CellSeg_BL.Checked = true;
                        richTextBox_log.AppendText("※Cell分割 (背光) Step4:" + "\n");
                        richTextBox_log.AppendText("開運算處理，用以消除Cell間相連之誤判" + "\n\n");
                    }
                    break;
                case "Step5":
                    {
                        rbtn_Step5_CellSeg_BL.Checked = true;
                        richTextBox_log.AppendText("※Cell分割 (背光) Step5:" + "\n");
                        richTextBox_log.AppendText("閉運算處理，用以連接同一Cell內區域" + "\n\n");
                    }
                    break;
                case "Step6":
                    {
                        rbtn_Step6_CellSeg_BL.Checked = true;
                        richTextBox_log.AppendText("※Cell分割 (背光) Step6:" + "\n");
                        richTextBox_log.AppendText("開運算處理，用以消除Cell間相連之誤判" + "\n\n");
                    }
                    break;
                case "Step7":
                    {
                        rbtn_Step7_CellSeg_BL.Checked = true;
                        richTextBox_log.AppendText("※Cell分割 (背光) Step7:" + "\n");
                        richTextBox_log.AppendText("膨脹處理，用以將各顆Cell的高，填滿電阻區域" + "\n\n");
                    }
                    break;
                case "Step8":
                    {
                        rbtn_Step8_CellSeg_BL.Checked = true;
                        richTextBox_log.AppendText("※Cell分割 (背光) Step8:" + "\n");
                        richTextBox_log.AppendText("Cell尺寸篩選" + "\n\n");
                    }
                    break;
                case "Step9":
                    {
                        rbtn_Step9_CellSeg_BL.Checked = true;
                        richTextBox_log.AppendText("※Cell分割 (背光) Step9:" + "\n");
                        richTextBox_log.AppendText("顯示檢測出之Cell" + "\n\n");
                    }
                    break;
                case "Step10":
                    {
                        rbtn_Step10_CellSeg_BL.Checked = true;
                        richTextBox_log.AppendText("※Cell分割 (背光) Step10:" + "\n");
                        richTextBox_log.AppendText("計算所有Cell區域 (Note: 綠色矩形代表檢測出之Cell位置)" + "\n\n");
                    }
                    break;
            }

            richTextBox_log.ScrollToCaret();
        }

        private void rbtn_Step_CellSeg_BL_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbtn = sender as RadioButton;
            if (rbtn.Checked == false) return;

            if (Input_ImgList.Count <= 0)
            {
                rbtn.Checked = false;
                MessageBox.Show("請先載入影像");
                return;
            }

            if (b_Initial_State)
            {
                if (Inspection_CellSeg_BL()) b_Initial_State = false;
                else
                {
                    rbtn.Checked = false;
                    MessageBox.Show("檢測失敗", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            combxDisplayImg.SelectedIndex = int.Parse(nud_ImageID_CellSeg_BL.Value.ToString());
            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
            Extension.HObjectMedthods.ReleaseHObject(ref Current_disp_image); // (20190816) Jeff Revised!
            Current_disp_regions.Dispose();
            HOperatorSet.GenEmptyObj(out Current_disp_regions);
            string tag = rbtn.Tag.ToString();
            switch (tag)
            {
                case "Step1":
                    {
                        Current_disp_image = Method.ho_ImageReduced_Inspect_BL.Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_Regions_removeDef != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.Connection(Method.ho_Regions_removeDef, out Current_disp_regions);
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step2":
                    {
                        Current_disp_image = Method.ho_TiledImage_ALL.Clone();
                        ClearDisplay(Current_disp_image);
                    }
                    break;
                case "Step3":
                    {
                        Current_disp_image = Method.ho_TiledImage_ALL.Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_CellReg_Resist_ALL != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.Connection(Method.ho_CellReg_Resist_ALL, out Current_disp_regions);
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step4":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_CellSeg_BL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_RegionOpening_ALL != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.Connection(Method.ho_RegionOpening_ALL, out Current_disp_regions);
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step5":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_CellSeg_BL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_RegionClosing_ALL != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.Connection(Method.ho_RegionClosing_ALL, out Current_disp_regions);
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step6":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_CellSeg_BL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_RegionOpening1_ALL != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.Connection(Method.ho_RegionOpening1_ALL, out Current_disp_regions);
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step7":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_CellSeg_BL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_RegionIntersection_ALL != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.Connection(Method.ho_RegionIntersection_ALL, out Current_disp_regions);
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step8":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_CellSeg_BL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_SelectedRegions_ALL != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.CopyObj(Method.ho_SelectedRegions_ALL, out Current_disp_regions, 1, -1);
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step9":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_CellSeg_BL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_All_ValidCells_rect != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.Connection(Method.ho_All_ValidCells_rect, out Current_disp_regions);
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_FirstCell_pt != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "blue");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispCircle(DisplayWindows.HalconWindow, Method.hv_y_FirstCell, Method.hv_x_FirstCell, 3);
                        }
                    }
                    break;
                case "Step10":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_CellSeg_BL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_AllCells_Rect != null && Method.ho_AllCells_Pt != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.Connection(Method.ho_AllCells_Rect, out Current_disp_regions);
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);

                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "blue");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Method.ho_AllCells_Pt, DisplayWindows.HalconWindow);
                        }

                        if (cbx_Enabled_RemainValidCells_CellSeg_BL.Checked)
                        {
                            if (Method.ho_All_ValidCells_rect != null)
                            {
                                HOperatorSet.SetColor(DisplayWindows.HalconWindow, "green");
                                HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                                HOperatorSet.DispObj(Method.ho_All_ValidCells_rect, DisplayWindows.HalconWindow);
                            }
                        }
                    }
                    break;
            }
        }

        private bool Inspection_CellSeg_BL()
        {
            if (!(Method.Read_AllImages_Insp(Recipe, Input_ImgList))) return false;
            if (!(Method.InitialPosition_Insp(Recipe))) return false;
            Method.CellSeg_BL_Insp(Recipe);
            //if (!(Method.CellSeg_BL_Insp(Recipe))) return false;
            return true;
        }

        private void param_CellSeg_BL_ValueChanged(object sender, EventArgs e) // (20191112) Jeff Revised!
        {
            Control c = sender as Control;
            string tag = c.Tag.ToString();
            // Visible狀態切換
            if (tag == "str_BinaryImg")
            {
                string str_BinaryImg = c.Text;
                if (str_BinaryImg == "固定閥值")
                {
                    gbx_ConstThresh_CellSeg_BL.Visible = true;
                    gbx_DynThresh_CellSeg_BL.Visible = false;
                }
                else if (str_BinaryImg == "動態閥值")
                {
                    gbx_ConstThresh_CellSeg_BL.Visible = false;
                    gbx_DynThresh_CellSeg_BL.Visible = true;
                }
                tag = "Step3"; // (20190412) Jeff Revised!
            }

            // Enabled狀態切換
            else if (tag == "Enabled_equHisto") // (20191112) Jeff Revised!
            {
                gbx_equHisto_part_CellSeg_BL.Enabled = (sender as CheckBox).Checked;
                tag = "Step2";
            }
            else if (tag == "Enabled_equHisto_part") // (20191112) Jeff Revised!
            {
                nud_count_col_CellSeg_BL.Enabled = (sender as CheckBox).Checked;
                tag = "Step2";
            }

            if (b_LoadRecipe) return;

            if (Input_ImgList.Count <= 0)
            {
                MessageBox.Show("請先載入影像");
                return;
            }

            ParamGetFromUI();
            if (!Inspection_CellSeg_BL())
            {
                clear_rbtn_Step_CellSeg_BL();
                MessageBox.Show("檢測失敗", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            b_Initial_State = false;

            switch (tag)
            {
                case "ImageID":
                    {

                    }
                    break;
                case "Step1":
                    {
                        if (rbtn_Step1_CellSeg_BL.Checked) rbtn_Step_CellSeg_BL_CheckedChanged(rbtn_Step1_CellSeg_BL, e);
                        else rbtn_Step1_CellSeg_BL.Checked = true;
                    }
                    break;
                case "Step2":
                    {
                        if (rbtn_Step2_CellSeg_BL.Checked) rbtn_Step_CellSeg_BL_CheckedChanged(rbtn_Step2_CellSeg_BL, e);
                        else rbtn_Step2_CellSeg_BL.Checked = true;
                    }
                    break;
                case "Step3":
                    {
                        if (rbtn_Step3_CellSeg_BL.Checked) rbtn_Step_CellSeg_BL_CheckedChanged(rbtn_Step3_CellSeg_BL, e);
                        else rbtn_Step3_CellSeg_BL.Checked = true;
                    }
                    break;
                case "Step4":
                    {
                        if (rbtn_Step4_CellSeg_BL.Checked) rbtn_Step_CellSeg_BL_CheckedChanged(rbtn_Step4_CellSeg_BL, e);
                        else rbtn_Step4_CellSeg_BL.Checked = true;
                    }
                    break;
                case "Step5":
                    {
                        if (rbtn_Step5_CellSeg_BL.Checked) rbtn_Step_CellSeg_BL_CheckedChanged(rbtn_Step5_CellSeg_BL, e);
                        else rbtn_Step5_CellSeg_BL.Checked = true;
                    }
                    break;
                case "Step6":
                    {
                        if (rbtn_Step6_CellSeg_BL.Checked) rbtn_Step_CellSeg_BL_CheckedChanged(rbtn_Step6_CellSeg_BL, e);
                        else rbtn_Step6_CellSeg_BL.Checked = true;
                    }
                    break;
                case "Step7":
                    {
                        if (rbtn_Step7_CellSeg_BL.Checked) rbtn_Step_CellSeg_BL_CheckedChanged(rbtn_Step7_CellSeg_BL, e);
                        else rbtn_Step7_CellSeg_BL.Checked = true;
                    }
                    break;
                case "Step8":
                    {
                        if (rbtn_Step8_CellSeg_BL.Checked) rbtn_Step_CellSeg_BL_CheckedChanged(rbtn_Step8_CellSeg_BL, e);
                        else rbtn_Step8_CellSeg_BL.Checked = true;
                    }
                    break;
                case "Step9":
                    {
                        if (rbtn_Step9_CellSeg_BL.Checked) rbtn_Step_CellSeg_BL_CheckedChanged(rbtn_Step9_CellSeg_BL, e);
                        else rbtn_Step9_CellSeg_BL.Checked = true;
                    }
                    break;
                case "Step10":
                    {
                        if (rbtn_Step10_CellSeg_BL.Checked) rbtn_Step_CellSeg_BL_CheckedChanged(rbtn_Step10_CellSeg_BL, e);
                        else rbtn_Step10_CellSeg_BL.Checked = true;
                    }
                    break;
            }
        }

        /// <summary>
        /// 清除 Cell分割 (背光) 內所有 rbtn_Step
        /// </summary>
        private void clear_rbtn_Step_CellSeg_BL()
        {
            rbtn_Step1_CellSeg_BL.Checked = false;
            rbtn_Step2_CellSeg_BL.Checked = false;
            rbtn_Step3_CellSeg_BL.Checked = false;
            rbtn_Step4_CellSeg_BL.Checked = false;
            rbtn_Step5_CellSeg_BL.Checked = false;
            rbtn_Step6_CellSeg_BL.Checked = false;
            rbtn_Step7_CellSeg_BL.Checked = false;
            rbtn_Step8_CellSeg_BL.Checked = false;
            rbtn_Step9_CellSeg_BL.Checked = false;
            rbtn_Step10_CellSeg_BL.Checked = false;
        }
        #endregion

        #region 瑕疵Cell判斷範圍
        private void btn_instruction_Df_CellReg_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string tag = btn.Tag.ToString();
            switch (tag)
            {
                case "Step1":
                    {
                        rbtn_Step1_Df_CellReg.Checked = true;
                        richTextBox_log.AppendText("※瑕疵Cell判斷範圍 Step1:" + "\n");
                        richTextBox_log.AppendText("設定影像檢測範圍，未落於範圍內之瑕疵將被消除。" + "\n\n");
                    }
                    break;
                case "Step2":
                    {
                        rbtn_Step2_Df_CellReg.Checked = true;
                        richTextBox_log.AppendText("※瑕疵Cell判斷範圍 Step2:" + "\n");
                        richTextBox_log.AppendText("設定瑕疵Cell判斷範圍 (Note: 如果瑕疵同時落於多顆Cell範圍內，則這些Cell皆判定為NG。)" + "\n\n");
                    }
                    break;
            }

            richTextBox_log.ScrollToCaret();
        }

        private void rbtn_Step_Df_CellReg_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbtn = sender as RadioButton;
            if (rbtn.Checked == false) return;

            if (Input_ImgList.Count <= 0)
            {
                rbtn.Checked = false;
                MessageBox.Show("請先載入影像");
                return;
            }

            if (b_Initial_State)
            {
                if (Inspection_Df_CellReg()) b_Initial_State = false;
                else
                {
                    rbtn.Checked = false;
                    MessageBox.Show("檢測失敗", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            Current_disp_regions.Dispose();
            HOperatorSet.GenEmptyObj(out Current_disp_regions);
            string tag = rbtn.Tag.ToString();
            switch (tag)
            {
                case "Step1":
                    {
                        ClearDisplay(Method.Insp_ImgList[combxDisplayImg.SelectedIndex]);
                        if (Method.ho_Region_InspReg != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.Connection(Method.ho_Region_InspReg, out Current_disp_regions);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step2":
                    {
                        ClearDisplay(Method.Insp_ImgList[combxDisplayImg.SelectedIndex]);
                        //if (Method.ho_AllCells_Rect != null)
                        //{
                        //    HOperatorSet.SetColor(DisplayWindows.HalconWindow, "green");
                        //    HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                        //    HOperatorSet.DispObj(Method.ho_AllCells_Rect, DisplayWindows.HalconWindow);
                        //}
                        if (Method.ho_Region_InspReg != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Method.ho_Region_InspReg, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_InspRects_AllCells != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.Connection(Method.ho_InspRects_AllCells, out Current_disp_regions);
                            //HOperatorSet.SetColored(DisplayWindows.HalconWindow, 12);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "blue");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
            }
        }

        private bool Inspection_Df_CellReg()
        {
            if (!(Method.Read_AllImages_Insp(Recipe, Input_ImgList))) return false;
            if (!(Method.InitialPosition_Insp(Recipe))) return false;
            if (cbx_CellSeg_CL.Checked)
            {
                if (!(Method.CellSeg_CL_Insp(Recipe))) return false;
            }
            else
            {
                if (!(Method.CellSeg_BL_Insp(Recipe))) return false;
            }
            if (!(Method.Df_CellReg(Recipe))) return false;
            return true;
        }

        private void param_Df_CellReg_ValueChanged(object sender, EventArgs e)
        {
            if (b_LoadRecipe) return;

            if (Input_ImgList.Count <= 0)
            {
                MessageBox.Show("請先載入影像");
                return;
            }

            ParamGetFromUI();
            if (!Inspection_Df_CellReg())
            {
                clear_rbtn_Step_Df_CellReg();
                MessageBox.Show("檢測失敗", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            b_Initial_State = false;

            Control c = sender as Control;
            string tag = c.Tag.ToString();

            switch (tag)
            {
                case "Step1":
                    {
                        if (rbtn_Step1_Df_CellReg.Checked) rbtn_Step_Df_CellReg_CheckedChanged(rbtn_Step1_Df_CellReg, e);
                        else rbtn_Step1_Df_CellReg.Checked = true;
                    }
                    break;
                case "Step2":
                    {
                        if (rbtn_Step2_Df_CellReg.Checked) rbtn_Step_Df_CellReg_CheckedChanged(rbtn_Step2_Df_CellReg, e);
                        else rbtn_Step2_Df_CellReg.Checked = true;
                    }
                    break;
            }
        }

        /// <summary>
        /// 清除 瑕疵Cell判斷範圍 內所有 rbtn_Step
        /// </summary>
        private void clear_rbtn_Step_Df_CellReg()
        {
            rbtn_Step1_Df_CellReg.Checked = false;
            rbtn_Step2_Df_CellReg.Checked = false;
        }
        #endregion
        
        #region 檢測黑條內白點 (正光)
        private void btn_instruction_WhiteBlob_FL_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string tag = btn.Tag.ToString();
            switch (tag)
            {
                case "Step1":
                    {
                        rbtn_Step1_WhiteBlob_FL.Checked = true;
                        richTextBox_log.AppendText("※檢測黑條內白點 (正光) Step1:" + "\n");
                        richTextBox_log.AppendText("設定檢測ROI (針對 各顆Cell, Cell內忽略區域 & 黑色長條電阻區)" + "\n\n");
                    }
                    break;
                case "Step2":
                    {
                        rbtn_Step2_WhiteBlob_FL.Checked = true;
                        richTextBox_log.AppendText("※檢測黑條內白點 (正光) Step2:" + "\n");
                        richTextBox_log.AppendText("二值化處理 (針對 各顆Cell不含忽略區域 & 黑色長條電阻區不含各顆Cell,及Cell內忽略區域)" + "\n");
                        richTextBox_log.AppendText("Note: " + "\n\n");
                    }
                    break;
                case "Step3":
                    {
                        rbtn_Step3_WhiteBlob_FL.Checked = true;
                        richTextBox_log.AppendText("※檢測黑條內白點 (正光) Step3:" + "\n");
                        richTextBox_log.AppendText("瑕疵Region內(同軸光)至少有一個pixel之灰階值低於一個數值 (瑕疵Region內最低灰階標準)" + "\n\n");
                    }
                    break;
                case "Step4":
                    {
                        rbtn_Step4_WhiteBlob_FL.Checked = true;
                        richTextBox_log.AppendText("※檢測黑條內白點 (正光) Step4:" + "\n");
                        richTextBox_log.AppendText("設定檢測ROI 2 (針對 各顆Cell)" + "\n\n");
                    }
                    break;
                case "Step5":
                    {
                        rbtn_Step5_WhiteBlob_FL.Checked = true;
                        richTextBox_log.AppendText("※檢測黑條內白點 (正光) Step5:" + "\n");
                        richTextBox_log.AppendText("瑕疵尺寸篩選" + "\n\n");
                    }
                    break;
                case "Step6":
                    {
                        rbtn_Step6_WhiteBlob_FL.Checked = true;
                        richTextBox_log.AppendText("※檢測黑條內白點 (正光)) Step6:" + "\n");
                        richTextBox_log.AppendText("瑕疵尺寸篩選" + "\n\n");
                    }
                    break;
            }

            richTextBox_log.ScrollToCaret();
        }

        private void rbtn_Step_WhiteBlob_FL_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbtn = sender as RadioButton;
            if (rbtn.Checked == false) return;

            if (Input_ImgList.Count <= 0)
            {
                rbtn.Checked = false;
                MessageBox.Show("請先載入影像");
                return;
            }

            if (b_Initial_State)
            {
                if (Inspection_WhiteBlob_FL()) b_Initial_State = false;
                else
                {
                    rbtn.Checked = false;
                    MessageBox.Show("檢測失敗", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            combxDisplayImg.SelectedIndex = int.Parse(nud_ImageID_WhiteBlob_FL.Value.ToString());
            Extension.HObjectMedthods.ReleaseHObject(ref Current_disp_image); // (20190816) Jeff Revised!
            Current_disp_regions.Dispose();
            HOperatorSet.GenEmptyObj(out Current_disp_regions);
            string tag = rbtn.Tag.ToString();
            switch (tag)
            {
                case "Step1":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_WhiteBlob_FL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_Rect_AllCells != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "green");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Method.ho_Rect_AllCells, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Rect_BypassReg != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "blue");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Method.ho_Rect_BypassReg, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Rect_BlackStripe != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.CopyObj(Method.ho_Rect_BlackStripe, out Current_disp_regions, 1, -1);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step2":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_WhiteBlob_FL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_Reg_Cell_Thresh != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Method.ho_Reg_Cell_Thresh, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Reg_Resist_Thresh != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "pink");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Method.ho_Reg_Resist_Thresh, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Rect_AllCells != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "green");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Method.ho_Rect_AllCells, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Rect_BypassReg != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "blue");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Method.ho_Rect_BypassReg, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Rect_BlackStripe != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Method.ho_Rect_BlackStripe, DisplayWindows.HalconWindow);
                        }

                        // 設定使用者點擊region
                        if (Method.ho_Reg_Cell_Thresh != null || Method.ho_Reg_Resist_Thresh != null)
                        {
                            HObject ho_Reg_Cell_Thresh, ho_Reg_Resist_Thresh;
                            HOperatorSet.Connection(Method.ho_Reg_Cell_Thresh, out ho_Reg_Cell_Thresh);
                            HOperatorSet.Connection(Method.ho_Reg_Resist_Thresh, out ho_Reg_Resist_Thresh);
                            Current_disp_regions.Dispose();
                            HOperatorSet.ConcatObj(ho_Reg_Cell_Thresh, ho_Reg_Resist_Thresh, out Current_disp_regions);
                            ho_Reg_Cell_Thresh.Dispose();
                            ho_Reg_Resist_Thresh.Dispose();
                        }
                    }
                    break;
                case "Step3":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_WhiteBlob_FL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_Reg_select != null)
                        {
                            HObject ho_Reg_select = null;
                            HOperatorSet.Connection(Method.ho_Reg_select, out ho_Reg_select);
                            Current_disp_regions.Dispose();
                            HOperatorSet.CopyObj(ho_Reg_select, out Current_disp_regions, 1, -1);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                            ho_Reg_select.Dispose();
                        }
                    }
                    break;
                case "Step4":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_WhiteBlob_FL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_Rect_AllCells2 != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "green");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Method.ho_Rect_AllCells2, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Reg_select_CellReg != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.CopyObj(Method.ho_Reg_select_CellReg, out Current_disp_regions, 1, -1);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step5":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_WhiteBlob_FL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_DefectReg_op1 != null)
                        {
                            Current_disp_regions.Dispose();
                            //HOperatorSet.CopyObj(Method.ho_DefectReg_op1, out Current_disp_regions, 1, -1);
                            HObject DefectReg_op1_union = null;
                            HOperatorSet.Union1(Method.ho_DefectReg_op1, out DefectReg_op1_union);
                            HOperatorSet.Connection(DefectReg_op1_union, out Current_disp_regions);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step6":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_WhiteBlob_FL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_DefectReg_WhiteBlob_FL != null)
                        {
                            Current_disp_regions.Dispose();
                            //HOperatorSet.CopyObj(Method.ho_DefectReg_WhiteBlob, out Current_disp_regions, 1, -1);
                            HOperatorSet.Connection(Method.ho_DefectReg_WhiteBlob_FL, out Current_disp_regions); // (20190331) Jeff Revised!
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
            }
        }

        private bool Inspection_WhiteBlob_FL()
        {
            if (!(Method.Read_AllImages_Insp(Recipe, Input_ImgList))) return false;
            if (!(Method.InitialPosition_Insp(Recipe))) return false;
            if (cbx_CellSeg_CL.Checked)
            {
                if (!(Method.CellSeg_CL_Insp(Recipe))) return false;
            }
            else
            {
                if (!(Method.CellSeg_BL_Insp(Recipe))) return false;
            }
            if (!(Method.Df_CellReg(Recipe))) return false; // (20190810) Jeff Revised!
            if (!(Method.WhiteBlob_FL_Insp(Recipe))) return false;
            return true;
        }

        private void param_WhiteBlob_FL_ValueChanged(object sender, EventArgs e)
        {
            Control c = sender as Control;
            string tag = c.Tag.ToString();
            // Visible狀態切換
            if (tag == "str_ModeBlackStripe")
            {
                string str_ModeBlackStripe = c.Text;
                if (str_ModeBlackStripe == "mode1")
                {
                    nud_H_BlackStripe_WhiteBlob_FL.Visible = true;
                    nud_H_BlackStripe_mode2_WhiteBlob_FL.Visible = false;
                    lab_unit_WhiteBlob_FL.Text = "(μm)";
                }
                else if (str_ModeBlackStripe == "mode2")
                {
                    nud_H_BlackStripe_WhiteBlob_FL.Visible = false;
                    nud_H_BlackStripe_mode2_WhiteBlob_FL.Visible = true;
                    lab_unit_WhiteBlob_FL.Text = "(pixel)";
                }
                tag = "Step1";
            }
            else if (tag == "str_BinaryImg_Cell") // (20190410) Jeff Revised!
            {
                if (combx_num_BinImg_Cell_WhiteBlob_FL.Text != "1")
                    combx_num_BinImg_Cell_WhiteBlob_FL.Text = "1";
                else
                    param_WhiteBlob_FL_ValueChanged(combx_num_BinImg_Cell_WhiteBlob_FL, e);
                tag = "Step2";
            }
            else if (tag == "num_BinImg_Cell") // (20190410) Jeff Revised!
            {
                string str_BinaryImg = combx_str_BinaryImg_Cell_WhiteBlob_FL.Text;
                string str_num = c.Text;
                if (str_BinaryImg == "固定閥值")
                {
                    gbx_DynThresh_1_Cell_WhiteBlob_FL.Visible = false;
                    gbx_DynThresh_2_Cell_WhiteBlob_FL.Visible = false;
                    if (str_num == "1")
                    {
                        gbx_ConstThresh_1_Cell_WhiteBlob_FL.Visible = true;
                        gbx_ConstThresh_2_Cell_WhiteBlob_FL.Visible = false;
                    }
                    else if (str_num == "2")
                    {
                        gbx_ConstThresh_1_Cell_WhiteBlob_FL.Visible = false;
                        gbx_ConstThresh_2_Cell_WhiteBlob_FL.Visible = true;
                    }
                }
                else if (str_BinaryImg == "動態閥值")
                {
                    gbx_ConstThresh_1_Cell_WhiteBlob_FL.Visible = false;
                    gbx_ConstThresh_2_Cell_WhiteBlob_FL.Visible = false;
                    if (str_num == "1")
                    {
                        gbx_DynThresh_1_Cell_WhiteBlob_FL.Visible = true;
                        gbx_DynThresh_2_Cell_WhiteBlob_FL.Visible = false;
                    }
                    else if (str_num == "2")
                    {
                        gbx_DynThresh_1_Cell_WhiteBlob_FL.Visible = false;
                        gbx_DynThresh_2_Cell_WhiteBlob_FL.Visible = true;
                    }
                }

                return;
            }
            else if (tag == "str_BinaryImg_Resist") // (20190410) Jeff Revised!
            {
                if (combx_num_BinImg_Resist_WhiteBlob_FL.Text != "1")
                    combx_num_BinImg_Resist_WhiteBlob_FL.Text = "1";
                else
                    param_WhiteBlob_FL_ValueChanged(combx_num_BinImg_Resist_WhiteBlob_FL, e);
                tag = "Step2";
            }
            else if (tag == "num_BinImg_Resist") // (20190410) Jeff Revised!
            {
                string str_BinaryImg = combx_str_BinaryImg_Resist_WhiteBlob_FL.Text;
                string str_num = c.Text;
                if (str_BinaryImg == "固定閥值")
                {
                    gbx_DynThresh_1_Resist_WhiteBlob_FL.Visible = false;
                    gbx_DynThresh_2_Resist_WhiteBlob_FL.Visible = false;
                    if (str_num == "1")
                    {
                        gbx_ConstThresh_1_Resist_WhiteBlob_FL.Visible = true;
                        gbx_ConstThresh_2_Resist_WhiteBlob_FL.Visible = false;
                    }
                    else if (str_num == "2")
                    {
                        gbx_ConstThresh_1_Resist_WhiteBlob_FL.Visible = false;
                        gbx_ConstThresh_2_Resist_WhiteBlob_FL.Visible = true;
                    }
                }
                else if (str_BinaryImg == "動態閥值")
                {
                    gbx_ConstThresh_1_Resist_WhiteBlob_FL.Visible = false;
                    gbx_ConstThresh_2_Resist_WhiteBlob_FL.Visible = false;
                    if (str_num == "1")
                    {
                        gbx_DynThresh_1_Resist_WhiteBlob_FL.Visible = true;
                        gbx_DynThresh_2_Resist_WhiteBlob_FL.Visible = false;
                    }
                    else if (str_num == "2")
                    {
                        gbx_DynThresh_1_Resist_WhiteBlob_FL.Visible = false;
                        gbx_DynThresh_2_Resist_WhiteBlob_FL.Visible = true;
                    }
                }

                return;
            }
            else if (tag == "num_op1")
            {
                string str_num_op1 = combx_num_op1_WhiteBlob_FL.Text;
                if (str_num_op1 == "1")
                {
                    gbx_op1_1_WhiteBlob_FL.Visible = true;
                    gbx_op1_2_WhiteBlob_FL.Visible = false;
                }
                else if (str_num_op1 == "2")
                {
                    gbx_op1_1_WhiteBlob_FL.Visible = false;
                    gbx_op1_2_WhiteBlob_FL.Visible = true;
                }

                return;
            }

            // Enabled狀態切換
            else if (tag == "count_BinImg_Cell") // (20190410) Jeff Revised!
            {
                if (nud_count_BinImg_Cell_WhiteBlob_FL.Value > 1)
                    combx_num_BinImg_Cell_WhiteBlob_FL.Enabled = true;
                else
                {
                    combx_num_BinImg_Cell_WhiteBlob_FL.Text = "1";
                    combx_num_BinImg_Cell_WhiteBlob_FL.Enabled = false;
                }
                tag = "Step2";
            }
            else if (tag == "count_BinImg_Resist") // (20190410) Jeff Revised!
            {
                if (nud_count_BinImg_Resist_WhiteBlob_FL.Value > 1)
                    combx_num_BinImg_Resist_WhiteBlob_FL.Enabled = true;
                else
                {
                    combx_num_BinImg_Resist_WhiteBlob_FL.Text = "1";
                    combx_num_BinImg_Resist_WhiteBlob_FL.Enabled = false;
                }
                tag = "Step2";
            }
            else if (tag == "count_op1")
            {
                if (nud_count_op1_WhiteBlob_FL.Value > 1)
                    combx_num_op1_WhiteBlob_FL.Enabled = true;
                else
                {
                    combx_num_op1_WhiteBlob_FL.Text = "1";
                    combx_num_op1_WhiteBlob_FL.Enabled = false;
                }
                tag = "Step5";
            }

            if (b_LoadRecipe) return;

            // 相同啟用項目同步更新
            switch (tag)
            {
                case "Enabled_Cell_Step1":
                    {
                        b_LoadRecipe = true;
                        cbx_Enabled_Cell_Step2_WhiteBlob_FL.Checked = cbx_Enabled_Cell_Step1_WhiteBlob_FL.Checked;
                        b_LoadRecipe = false;
                        tag = "Step1";
                    }
                    break;
                case "Enabled_Cell_Step2":
                    {
                        b_LoadRecipe = true;
                        cbx_Enabled_Cell_Step1_WhiteBlob_FL.Checked = cbx_Enabled_Cell_Step2_WhiteBlob_FL.Checked;
                        b_LoadRecipe = false;
                        tag = "Step2";
                    }
                    break;
                case "Enabled_BlackStripe_Step1":
                    {
                        b_LoadRecipe = true;
                        cbx_Enabled_BlackStripe_Step2_WhiteBlob_FL.Checked = cbx_Enabled_BlackStripe_Step1_WhiteBlob_FL.Checked;
                        b_LoadRecipe = false;
                        tag = "Step1";
                    }
                    break;
                case "Enabled_BlackStripe_Step2":
                    {
                        b_LoadRecipe = true;
                        cbx_Enabled_BlackStripe_Step1_WhiteBlob_FL.Checked = cbx_Enabled_BlackStripe_Step2_WhiteBlob_FL.Checked;
                        b_LoadRecipe = false;
                        tag = "Step2";
                    }
                    break;
            }

            if (Input_ImgList.Count <= 0)
            {
                MessageBox.Show("請先載入影像");
                return;
            }

            ParamGetFromUI();
            if (!Inspection_WhiteBlob_FL())
            {
                clear_rbtn_Step_WhiteBlob_FL();
                MessageBox.Show("檢測失敗", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            b_Initial_State = false;

            switch (tag)
            {
                case "ImageID":
                    {

                    }
                    break;
                case "Step1":
                    {
                        if (rbtn_Step1_WhiteBlob_FL.Checked) rbtn_Step_WhiteBlob_FL_CheckedChanged(rbtn_Step1_WhiteBlob_FL, e);
                        else rbtn_Step1_WhiteBlob_FL.Checked = true;
                    }
                    break;
                case "Step2":
                    {
                        if (rbtn_Step2_WhiteBlob_FL.Checked) rbtn_Step_WhiteBlob_FL_CheckedChanged(rbtn_Step2_WhiteBlob_FL, e);
                        else rbtn_Step2_WhiteBlob_FL.Checked = true;
                    }
                    break;
                case "Step3":
                    {
                        if (rbtn_Step3_WhiteBlob_FL.Checked) rbtn_Step_WhiteBlob_FL_CheckedChanged(rbtn_Step3_WhiteBlob_FL, e);
                        else rbtn_Step3_WhiteBlob_FL.Checked = true;
                    }
                    break;
                case "Step4":
                    {
                        if (rbtn_Step4_WhiteBlob_FL.Checked) rbtn_Step_WhiteBlob_FL_CheckedChanged(rbtn_Step4_WhiteBlob_FL, e);
                        else rbtn_Step4_WhiteBlob_FL.Checked = true;
                    }
                    break;
                case "Step5":
                    {
                        if (rbtn_Step5_WhiteBlob_FL.Checked) rbtn_Step_WhiteBlob_FL_CheckedChanged(rbtn_Step5_WhiteBlob_FL, e);
                        else rbtn_Step5_WhiteBlob_FL.Checked = true;
                    }
                    break;
                case "Step6":
                    {
                        if (rbtn_Step6_WhiteBlob_FL.Checked) rbtn_Step_WhiteBlob_FL_CheckedChanged(rbtn_Step6_WhiteBlob_FL, e);
                        else rbtn_Step6_WhiteBlob_FL.Checked = true;
                    }
                    break;
            }
        }

        /// <summary>
        /// 清除 檢測黑條內白點 (正光) 內所有 rbtn_Step
        /// </summary>
        private void clear_rbtn_Step_WhiteBlob_FL()
        {
            rbtn_Step1_WhiteBlob_FL.Checked = false;
            rbtn_Step2_WhiteBlob_FL.Checked = false;
            rbtn_Step3_WhiteBlob_FL.Checked = false;
            rbtn_Step4_WhiteBlob_FL.Checked = false;
            rbtn_Step5_WhiteBlob_FL.Checked = false;
            rbtn_Step6_WhiteBlob_FL.Checked = false;
        }
        #endregion

        #region 檢測黑條內白點 (同軸光)
        private void btn_instruction_WhiteBlob_CL_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string tag = btn.Tag.ToString();
            switch (tag)
            {
                case "Step1":
                    {
                        rbtn_Step1_WhiteBlob_CL.Checked = true;
                        richTextBox_log.AppendText("※檢測黑條內白點 (同軸光) Step1:" + "\n");
                        richTextBox_log.AppendText("設定檢測ROI (針對 各顆Cell, Cell內忽略區域 & 黑色長條電阻區)" + "\n\n");
                    }
                    break;
                case "Step2":
                    {
                        rbtn_Step2_WhiteBlob_CL.Checked = true;
                        richTextBox_log.AppendText("※檢測黑條內白點 (同軸光) Step2:" + "\n");
                        richTextBox_log.AppendText("二值化處理 (針對 各顆Cell不含忽略區域 & 黑色長條電阻區不含各顆Cell,及Cell內忽略區域)" + "\n");
                        richTextBox_log.AppendText("Note: " + "\n\n");
                    }
                    break;
                case "Step3":
                    {
                        rbtn_Step3_WhiteBlob_CL.Checked = true;
                        richTextBox_log.AppendText("※檢測黑條內白點 (同軸光) Step3:" + "\n");
                        richTextBox_log.AppendText("瑕疵Region內(同軸光)至少有一個pixel之灰階值低於一個數值 (瑕疵Region內最低灰階標準)" + "\n\n");
                    }
                    break;
                case "Step4":
                    {
                        rbtn_Step4_WhiteBlob_CL.Checked = true;
                        richTextBox_log.AppendText("※檢測黑條內白點 (同軸光) Step4:" + "\n");
                        richTextBox_log.AppendText("設定檢測ROI 2 (針對 各顆Cell)" + "\n\n");
                    }
                    break;
                case "Step5":
                    {
                        rbtn_Step5_WhiteBlob_CL.Checked = true;
                        richTextBox_log.AppendText("※檢測黑條內白點 (同軸光) Step5:" + "\n");
                        richTextBox_log.AppendText("瑕疵尺寸篩選" + "\n\n");
                    }
                    break;
                case "Step6":
                    {
                        rbtn_Step6_WhiteBlob_CL.Checked = true;
                        richTextBox_log.AppendText("※檢測黑條內白點 (同軸光)) Step6:" + "\n");
                        richTextBox_log.AppendText("瑕疵尺寸篩選" + "\n\n");
                    }
                    break;
            }

            richTextBox_log.ScrollToCaret();
        }

        private void rbtn_Step_WhiteBlob_CL_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbtn = sender as RadioButton;
            if (rbtn.Checked == false) return;

            if (Input_ImgList.Count <= 0)
            {
                rbtn.Checked = false;
                MessageBox.Show("請先載入影像");
                return;
            }

            if (b_Initial_State)
            {
                if (Inspection_WhiteBlob_CL()) b_Initial_State = false;
                else
                {
                    rbtn.Checked = false;
                    MessageBox.Show("檢測失敗", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            combxDisplayImg.SelectedIndex = int.Parse(nud_ImageID_WhiteBlob_CL.Value.ToString());
            Extension.HObjectMedthods.ReleaseHObject(ref Current_disp_image); // (20190816) Jeff Revised!
            Current_disp_regions.Dispose();
            HOperatorSet.GenEmptyObj(out Current_disp_regions);
            string tag = rbtn.Tag.ToString();
            switch (tag)
            {
                case "Step1":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_WhiteBlob_CL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_Rect_AllCells != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "green");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Method.ho_Rect_AllCells, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Rect_BypassReg != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "blue");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Method.ho_Rect_BypassReg, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Rect_BlackStripe != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.CopyObj(Method.ho_Rect_BlackStripe, out Current_disp_regions, 1, -1);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step2":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_WhiteBlob_CL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_Reg_Cell_Thresh != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Method.ho_Reg_Cell_Thresh, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Reg_Resist_Thresh != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "pink");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Method.ho_Reg_Resist_Thresh, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Rect_AllCells != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "green");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Method.ho_Rect_AllCells, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Rect_BypassReg != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "blue");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Method.ho_Rect_BypassReg, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Rect_BlackStripe != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Method.ho_Rect_BlackStripe, DisplayWindows.HalconWindow);
                        }

                        // 設定使用者點擊region
                        if (Method.ho_Reg_Cell_Thresh != null || Method.ho_Reg_Resist_Thresh != null)
                        {
                            HObject ho_Reg_Cell_Thresh, ho_Reg_Resist_Thresh;
                            HOperatorSet.Connection(Method.ho_Reg_Cell_Thresh, out ho_Reg_Cell_Thresh);
                            HOperatorSet.Connection(Method.ho_Reg_Resist_Thresh, out ho_Reg_Resist_Thresh);
                            Current_disp_regions.Dispose();
                            HOperatorSet.ConcatObj(ho_Reg_Cell_Thresh, ho_Reg_Resist_Thresh, out Current_disp_regions);
                            ho_Reg_Cell_Thresh.Dispose();
                            ho_Reg_Resist_Thresh.Dispose();
                        }
                    }
                    break;
                case "Step3":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_WhiteBlob_CL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_Reg_select != null)
                        {
                            HObject ho_Reg_select = null;
                            HOperatorSet.Connection(Method.ho_Reg_select, out ho_Reg_select);
                            Current_disp_regions.Dispose();
                            HOperatorSet.CopyObj(ho_Reg_select, out Current_disp_regions, 1, -1);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                            ho_Reg_select.Dispose();
                        }
                    }
                    break;
                case "Step4":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_WhiteBlob_CL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_Rect_AllCells2 != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "green");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Method.ho_Rect_AllCells2, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Reg_select_CellReg != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.CopyObj(Method.ho_Reg_select_CellReg, out Current_disp_regions, 1, -1);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step5":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_WhiteBlob_CL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_DefectReg_op1 != null)
                        {
                            Current_disp_regions.Dispose();
                            //HOperatorSet.CopyObj(Method.ho_DefectReg_op1, out Current_disp_regions, 1, -1);
                            HObject DefectReg_op1_union = null;
                            HOperatorSet.Union1(Method.ho_DefectReg_op1, out DefectReg_op1_union);
                            HOperatorSet.Connection(DefectReg_op1_union, out Current_disp_regions);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step6":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_WhiteBlob_CL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_DefectReg_WhiteBlob_CL != null)
                        {
                            Current_disp_regions.Dispose();
                            //HOperatorSet.CopyObj(Method.ho_DefectReg_WhiteBlob, out Current_disp_regions, 1, -1);
                            HOperatorSet.Connection(Method.ho_DefectReg_WhiteBlob_CL, out Current_disp_regions);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
            }
        }

        private bool Inspection_WhiteBlob_CL()
        {
            if (!(Method.Read_AllImages_Insp(Recipe, Input_ImgList))) return false;
            if (!(Method.InitialPosition_Insp(Recipe))) return false;
            if (cbx_CellSeg_CL.Checked)
            {
                if (!(Method.CellSeg_CL_Insp(Recipe))) return false;
            }
            else
            {
                if (!(Method.CellSeg_BL_Insp(Recipe))) return false;
            }
            if (!(Method.Df_CellReg(Recipe))) return false; // (20190810) Jeff Revised!
            if (!(Method.WhiteBlob_CL_Insp(Recipe))) return false;
            return true;
        }

        private void param_WhiteBlob_CL_ValueChanged(object sender, EventArgs e)
        {
            Control c = sender as Control;
            string tag = c.Tag.ToString();
            // Visible狀態切換
            if (tag == "str_ModeBlackStripe")
            {
                string str_ModeBlackStripe = c.Text;
                if (str_ModeBlackStripe == "mode1")
                {
                    nud_H_BlackStripe_WhiteBlob_CL.Visible = true;
                    nud_H_BlackStripe_mode2_WhiteBlob_CL.Visible = false;
                    lab_unit_WhiteBlob_CL.Text = "(μm)";
                }
                else if (str_ModeBlackStripe == "mode2")
                {
                    nud_H_BlackStripe_WhiteBlob_CL.Visible = false;
                    nud_H_BlackStripe_mode2_WhiteBlob_CL.Visible = true;
                    lab_unit_WhiteBlob_CL.Text = "(pixel)";
                }
                tag = "Step1";
            }
            else if (tag == "str_BinaryImg_Cell") // (20190410) Jeff Revised!
            {
                if (combx_num_BinImg_Cell_WhiteBlob_CL.Text != "1")
                    combx_num_BinImg_Cell_WhiteBlob_CL.Text = "1";
                else
                    param_WhiteBlob_CL_ValueChanged(combx_num_BinImg_Cell_WhiteBlob_CL, e);
                tag = "Step2";
            }
            else if (tag == "num_BinImg_Cell") // (20190410) Jeff Revised!
            {
                string str_BinaryImg = combx_str_BinaryImg_Cell_WhiteBlob_CL.Text;
                string str_num = c.Text;
                if (str_BinaryImg == "固定閥值")
                {
                    gbx_DynThresh_1_Cell_WhiteBlob_CL.Visible = false;
                    gbx_DynThresh_2_Cell_WhiteBlob_CL.Visible = false;
                    if (str_num == "1")
                    {
                        gbx_ConstThresh_1_Cell_WhiteBlob_CL.Visible = true;
                        gbx_ConstThresh_2_Cell_WhiteBlob_CL.Visible = false;
                    }
                    else if (str_num == "2")
                    {
                        gbx_ConstThresh_1_Cell_WhiteBlob_CL.Visible = false;
                        gbx_ConstThresh_2_Cell_WhiteBlob_CL.Visible = true;
                    }
                }
                else if (str_BinaryImg == "動態閥值")
                {
                    gbx_ConstThresh_1_Cell_WhiteBlob_CL.Visible = false;
                    gbx_ConstThresh_2_Cell_WhiteBlob_CL.Visible = false;
                    if (str_num == "1")
                    {
                        gbx_DynThresh_1_Cell_WhiteBlob_CL.Visible = true;
                        gbx_DynThresh_2_Cell_WhiteBlob_CL.Visible = false;
                    }
                    else if (str_num == "2")
                    {
                        gbx_DynThresh_1_Cell_WhiteBlob_CL.Visible = false;
                        gbx_DynThresh_2_Cell_WhiteBlob_CL.Visible = true;
                    }
                }

                return;
            }
            else if (tag == "str_BinaryImg_Resist") // (20190410) Jeff Revised!
            {
                if (combx_num_BinImg_Resist_WhiteBlob_CL.Text != "1")
                    combx_num_BinImg_Resist_WhiteBlob_CL.Text = "1";
                else
                    param_WhiteBlob_CL_ValueChanged(combx_num_BinImg_Resist_WhiteBlob_CL, e);
                tag = "Step2";
            }
            else if (tag == "num_BinImg_Resist") // (20190410) Jeff Revised!
            {
                string str_BinaryImg = combx_str_BinaryImg_Resist_WhiteBlob_CL.Text;
                string str_num = c.Text;
                if (str_BinaryImg == "固定閥值")
                {
                    gbx_DynThresh_1_Resist_WhiteBlob_CL.Visible = false;
                    gbx_DynThresh_2_Resist_WhiteBlob_CL.Visible = false;
                    if (str_num == "1")
                    {
                        gbx_ConstThresh_1_Resist_WhiteBlob_CL.Visible = true;
                        gbx_ConstThresh_2_Resist_WhiteBlob_CL.Visible = false;
                    }
                    else if (str_num == "2")
                    {
                        gbx_ConstThresh_1_Resist_WhiteBlob_CL.Visible = false;
                        gbx_ConstThresh_2_Resist_WhiteBlob_CL.Visible = true;
                    }
                }
                else if (str_BinaryImg == "動態閥值")
                {
                    gbx_ConstThresh_1_Resist_WhiteBlob_CL.Visible = false;
                    gbx_ConstThresh_2_Resist_WhiteBlob_CL.Visible = false;
                    if (str_num == "1")
                    {
                        gbx_DynThresh_1_Resist_WhiteBlob_CL.Visible = true;
                        gbx_DynThresh_2_Resist_WhiteBlob_CL.Visible = false;
                    }
                    else if (str_num == "2")
                    {
                        gbx_DynThresh_1_Resist_WhiteBlob_CL.Visible = false;
                        gbx_DynThresh_2_Resist_WhiteBlob_CL.Visible = true;
                    }
                }

                return;
            }
            else if (tag == "num_op1")
            {
                string str_num_op1 = combx_num_op1_WhiteBlob_CL.Text;
                if (str_num_op1 == "1")
                {
                    gbx_op1_1_WhiteBlob_CL.Visible = true;
                    gbx_op1_2_WhiteBlob_CL.Visible = false;
                }
                else if (str_num_op1 == "2")
                {
                    gbx_op1_1_WhiteBlob_CL.Visible = false;
                    gbx_op1_2_WhiteBlob_CL.Visible = true;
                }

                return;
            }

            // Enabled狀態切換
            else if (tag == "count_BinImg_Cell") // (20190410) Jeff Revised!
            {
                if (nud_count_BinImg_Cell_WhiteBlob_CL.Value > 1)
                    combx_num_BinImg_Cell_WhiteBlob_CL.Enabled = true;
                else
                {
                    combx_num_BinImg_Cell_WhiteBlob_CL.Text = "1";
                    combx_num_BinImg_Cell_WhiteBlob_CL.Enabled = false;
                }
                tag = "Step2";
            }
            else if (tag == "count_BinImg_Resist") // (20190410) Jeff Revised!
            {
                if (nud_count_BinImg_Resist_WhiteBlob_CL.Value > 1)
                    combx_num_BinImg_Resist_WhiteBlob_CL.Enabled = true;
                else
                {
                    combx_num_BinImg_Resist_WhiteBlob_CL.Text = "1";
                    combx_num_BinImg_Resist_WhiteBlob_CL.Enabled = false;
                }
                tag = "Step2";
            }
            else if (tag == "count_op1")
            {
                if (nud_count_op1_WhiteBlob_CL.Value > 1)
                    combx_num_op1_WhiteBlob_CL.Enabled = true;
                else
                {
                    combx_num_op1_WhiteBlob_CL.Text = "1";
                    combx_num_op1_WhiteBlob_CL.Enabled = false;
                }
                tag = "Step5";
            }

            if (b_LoadRecipe) return;

            // 相同啟用項目同步更新
            switch (tag)
            {
                case "Enabled_Cell_Step1":
                    {
                        b_LoadRecipe = true;
                        cbx_Enabled_Cell_Step2_WhiteBlob_CL.Checked = cbx_Enabled_Cell_Step1_WhiteBlob_CL.Checked;
                        b_LoadRecipe = false;
                        tag = "Step1";
                    }
                    break;
                case "Enabled_Cell_Step2":
                    {
                        b_LoadRecipe = true;
                        cbx_Enabled_Cell_Step1_WhiteBlob_CL.Checked = cbx_Enabled_Cell_Step2_WhiteBlob_CL.Checked;
                        b_LoadRecipe = false;
                        tag = "Step2";
                    }
                    break;
                case "Enabled_BlackStripe_Step1":
                    {
                        b_LoadRecipe = true;
                        cbx_Enabled_BlackStripe_Step2_WhiteBlob_CL.Checked = cbx_Enabled_BlackStripe_Step1_WhiteBlob_CL.Checked;
                        b_LoadRecipe = false;
                        tag = "Step1";
                    }
                    break;
                case "Enabled_BlackStripe_Step2":
                    {
                        b_LoadRecipe = true;
                        cbx_Enabled_BlackStripe_Step1_WhiteBlob_CL.Checked = cbx_Enabled_BlackStripe_Step2_WhiteBlob_CL.Checked;
                        b_LoadRecipe = false;
                        tag = "Step2";
                    }
                    break;
            }

            if (Input_ImgList.Count <= 0)
            {
                MessageBox.Show("請先載入影像");
                return;
            }

            ParamGetFromUI();
            if (!Inspection_WhiteBlob_CL())
            {
                clear_rbtn_Step_WhiteBlob_CL();
                MessageBox.Show("檢測失敗", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            b_Initial_State = false;

            switch (tag)
            {
                case "ImageID":
                    {

                    }
                    break;
                case "Step1":
                    {
                        if (rbtn_Step1_WhiteBlob_CL.Checked) rbtn_Step_WhiteBlob_CL_CheckedChanged(rbtn_Step1_WhiteBlob_CL, e);
                        else rbtn_Step1_WhiteBlob_CL.Checked = true;
                    }
                    break;
                case "Step2":
                    {
                        if (rbtn_Step2_WhiteBlob_CL.Checked) rbtn_Step_WhiteBlob_CL_CheckedChanged(rbtn_Step2_WhiteBlob_CL, e);
                        else rbtn_Step2_WhiteBlob_CL.Checked = true;
                    }
                    break;
                case "Step3":
                    {
                        if (rbtn_Step3_WhiteBlob_CL.Checked) rbtn_Step_WhiteBlob_CL_CheckedChanged(rbtn_Step3_WhiteBlob_CL, e);
                        else rbtn_Step3_WhiteBlob_CL.Checked = true;
                    }
                    break;
                case "Step4":
                    {
                        if (rbtn_Step4_WhiteBlob_CL.Checked) rbtn_Step_WhiteBlob_CL_CheckedChanged(rbtn_Step4_WhiteBlob_CL, e);
                        else rbtn_Step4_WhiteBlob_CL.Checked = true;
                    }
                    break;
                case "Step5":
                    {
                        if (rbtn_Step5_WhiteBlob_CL.Checked) rbtn_Step_WhiteBlob_CL_CheckedChanged(rbtn_Step5_WhiteBlob_CL, e);
                        else rbtn_Step5_WhiteBlob_CL.Checked = true;
                    }
                    break;
                case "Step6":
                    {
                        if (rbtn_Step6_WhiteBlob_CL.Checked) rbtn_Step_WhiteBlob_CL_CheckedChanged(rbtn_Step6_WhiteBlob_CL, e);
                        else rbtn_Step6_WhiteBlob_CL.Checked = true;
                    }
                    break;
            }
        }

        /// <summary>
        /// 清除 檢測黑條內白點 (同軸光) 內所有 rbtn_Step
        /// </summary>
        private void clear_rbtn_Step_WhiteBlob_CL()
        {
            rbtn_Step1_WhiteBlob_CL.Checked = false;
            rbtn_Step2_WhiteBlob_CL.Checked = false;
            rbtn_Step3_WhiteBlob_CL.Checked = false;
            rbtn_Step4_WhiteBlob_CL.Checked = false;
            rbtn_Step5_WhiteBlob_CL.Checked = false;
            rbtn_Step6_WhiteBlob_CL.Checked = false;
        }
        #endregion

        #region 檢測絲狀異物 (正光)
        private void btn_instruction_Line_FL_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string tag = btn.Tag.ToString();
            switch (tag)
            {
                case "Step1":
                    {
                        rbtn_Step1_Line_FL.Checked = true;
                        richTextBox_log.AppendText("※檢測絲狀異物 (黑條內白點) (正光) Step1:" + "\n");
                        richTextBox_log.AppendText("設定檢測ROI (針對 各顆Cell, Cell內忽略區域 & 黑色長條電阻區)" + "\n\n");
                    }
                    break;
                case "Step2":
                    {
                        rbtn_Step2_Line_FL.Checked = true;
                        richTextBox_log.AppendText("※檢測絲狀異物 (黑條內白點) (正光) Step2:" + "\n");
                        richTextBox_log.AppendText("二值化處理 (針對 各顆Cell不含忽略區域 & 黑色長條電阻區不含各顆Cell,及Cell內忽略區域)" + "\n");
                        richTextBox_log.AppendText("Note: " + "\n\n");
                    }
                    break;
                case "Step3":
                    {
                        rbtn_Step3_Line_FL.Checked = true;
                        richTextBox_log.AppendText("※檢測絲狀異物 (黑條內白點) (正光) Step3:" + "\n");
                        richTextBox_log.AppendText("瑕疵Region內(正光)至少有一個pixel之灰階值高於一個數值 (瑕疵Region內最高灰階標準)" + "\n\n");
                    }
                    break;
                case "Step4":
                    {
                        rbtn_Step4_Line_FL.Checked = true;
                        richTextBox_log.AppendText("※檢測絲狀異物 (黑條內白點) (正光) Step4:" + "\n");
                        richTextBox_log.AppendText("設定檢測ROI 2 (針對 各顆Cell)" + "\n\n");
                    }
                    break;
                case "Step5":
                    {
                        rbtn_Step5_Line_FL.Checked = true;
                        richTextBox_log.AppendText("※檢測絲狀異物 (黑條內白點) (正光) Step5:" + "\n");
                        richTextBox_log.AppendText("瑕疵尺寸篩選" + "\n\n");
                    }
                    break;
                case "Step6":
                    {
                        rbtn_Step6_Line_FL.Checked = true;
                        richTextBox_log.AppendText("※檢測絲狀異物 (黑條內白點) (正光) Step6:" + "\n");
                        richTextBox_log.AppendText("瑕疵尺寸篩選" + "\n\n");
                    }
                    break;
            }

            richTextBox_log.ScrollToCaret();
        }

        private void rbtn_Step_Line_FL_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbtn = sender as RadioButton;
            if (rbtn.Checked == false) return;

            if (Input_ImgList.Count <= 0)
            {
                rbtn.Checked = false;
                MessageBox.Show("請先載入影像");
                return;
            }

            if (b_Initial_State)
            {
                if (Inspection_Line_FL()) b_Initial_State = false;
                else
                {
                    rbtn.Checked = false;
                    MessageBox.Show("檢測失敗", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            combxDisplayImg.SelectedIndex = int.Parse(nud_ImageID_Line_FL.Value.ToString());
            Extension.HObjectMedthods.ReleaseHObject(ref Current_disp_image); // (20190816) Jeff Revised!
            Current_disp_regions.Dispose();
            HOperatorSet.GenEmptyObj(out Current_disp_regions);
            string tag = rbtn.Tag.ToString();
            switch (tag)
            {
                case "Step1":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_Line_FL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_Rect_AllCells != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "green");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Method.ho_Rect_AllCells, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Rect_BypassReg != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "blue");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Method.ho_Rect_BypassReg, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Rect_BlackStripe != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.CopyObj(Method.ho_Rect_BlackStripe, out Current_disp_regions, 1, -1);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step2":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_Line_FL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_Reg_Cell_Thresh != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Method.ho_Reg_Cell_Thresh, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Reg_Resist_Thresh != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "pink");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Method.ho_Reg_Resist_Thresh, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Rect_AllCells != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "green");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Method.ho_Rect_AllCells, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Rect_BypassReg != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "blue");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Method.ho_Rect_BypassReg, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Rect_BlackStripe != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Method.ho_Rect_BlackStripe, DisplayWindows.HalconWindow);
                        }

                        // 設定使用者點擊region
                        if (Method.ho_Reg_Cell_Thresh != null || Method.ho_Reg_Resist_Thresh != null)
                        {
                            HObject ho_Reg_Cell_Thresh, ho_Reg_Resist_Thresh;
                            HOperatorSet.Connection(Method.ho_Reg_Cell_Thresh, out ho_Reg_Cell_Thresh);
                            HOperatorSet.Connection(Method.ho_Reg_Resist_Thresh, out ho_Reg_Resist_Thresh);
                            Current_disp_regions.Dispose();
                            HOperatorSet.ConcatObj(ho_Reg_Cell_Thresh, ho_Reg_Resist_Thresh, out Current_disp_regions);
                            ho_Reg_Cell_Thresh.Dispose();
                            ho_Reg_Resist_Thresh.Dispose();
                        }
                    }
                    break;
                case "Step3":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_Line_FL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_Reg_select != null)
                        {
                            HObject ho_Reg_select = null;
                            HOperatorSet.Connection(Method.ho_Reg_select, out ho_Reg_select);
                            Current_disp_regions.Dispose();
                            HOperatorSet.CopyObj(ho_Reg_select, out Current_disp_regions, 1, -1);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                            ho_Reg_select.Dispose();
                        }
                    }
                    break;
                case "Step4":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_Line_FL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_Rect_AllCells2 != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "green");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Method.ho_Rect_AllCells2, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Reg_select_CellReg != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.CopyObj(Method.ho_Reg_select_CellReg, out Current_disp_regions, 1, -1);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step5":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_Line_FL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_DefectReg_op1 != null)
                        {
                            Current_disp_regions.Dispose();
                            //HOperatorSet.CopyObj(Method.ho_DefectReg_op1, out Current_disp_regions, 1, -1);
                            HObject DefectReg_op1_union = null;
                            HOperatorSet.Union1(Method.ho_DefectReg_op1, out DefectReg_op1_union);
                            HOperatorSet.Connection(DefectReg_op1_union, out Current_disp_regions);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step6":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_Line_FL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_DefectReg_Line != null)
                        {
                            Current_disp_regions.Dispose();
                            //HOperatorSet.CopyObj(Method.ho_DefectReg_Line, out Current_disp_regions, 1, -1);
                            HOperatorSet.Connection(Method.ho_DefectReg_Line, out Current_disp_regions); // (20190331) Jeff Revised!
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
            }
        }

        private bool Inspection_Line_FL()
        {
            if (!(Method.Read_AllImages_Insp(Recipe, Input_ImgList))) return false;
            if (!(Method.InitialPosition_Insp(Recipe))) return false;
            if (cbx_CellSeg_CL.Checked)
            {
                if (!(Method.CellSeg_CL_Insp(Recipe))) return false;
            }
            else
            {
                if (!(Method.CellSeg_BL_Insp(Recipe))) return false;
            }
            if (!(Method.Df_CellReg(Recipe))) return false; // (20190810) Jeff Revised!
            if (!(Method.Line_FL_Insp(Recipe))) return false;
            return true;
        }

        private void param_Line_FL_ValueChanged(object sender, EventArgs e)
        {
            Control c = sender as Control;
            string tag = c.Tag.ToString();
            // Visible狀態切換
            if (tag == "str_ModeBlackStripe")
            {
                string str_ModeBlackStripe = c.Text;
                if (str_ModeBlackStripe == "mode1")
                {
                    nud_H_BlackStripe_Line_FL.Visible = true;
                    nud_H_BlackStripe_mode2_Line_FL.Visible = false;
                    lab_unit_Line_FL.Text = "(μm)";
                }
                else if (str_ModeBlackStripe == "mode2")
                {
                    nud_H_BlackStripe_Line_FL.Visible = false;
                    nud_H_BlackStripe_mode2_Line_FL.Visible = true;
                    lab_unit_Line_FL.Text = "(pixel)";
                }
                tag = "Step1"; // (20190411) Jeff Revised!
            }
            else if (tag == "str_BinaryImg_Cell") // (20190410) Jeff Revised!
            {
                if (combx_num_BinImg_Cell_Line_FL.Text != "1")
                    combx_num_BinImg_Cell_Line_FL.Text = "1";
                else
                    param_Line_FL_ValueChanged(combx_num_BinImg_Cell_Line_FL, e);
                tag = "Step2";
            }
            else if (tag == "num_BinImg_Cell") // (20190410) Jeff Revised!
            {
                string str_BinaryImg = combx_str_BinaryImg_Cell_Line_FL.Text;
                string str_num = c.Text;
                if (str_BinaryImg == "固定閥值")
                {
                    gbx_DynThresh_1_Cell_Line_FL.Visible = false;
                    gbx_DynThresh_2_Cell_Line_FL.Visible = false;
                    if (str_num == "1")
                    {
                        gbx_ConstThresh_1_Cell_Line_FL.Visible = true;
                        gbx_ConstThresh_2_Cell_Line_FL.Visible = false;
                    }
                    else if (str_num == "2")
                    {
                        gbx_ConstThresh_1_Cell_Line_FL.Visible = false;
                        gbx_ConstThresh_2_Cell_Line_FL.Visible = true;
                    }
                }
                else if (str_BinaryImg == "動態閥值")
                {
                    gbx_ConstThresh_1_Cell_Line_FL.Visible = false;
                    gbx_ConstThresh_2_Cell_Line_FL.Visible = false;
                    if (str_num == "1")
                    {
                        gbx_DynThresh_1_Cell_Line_FL.Visible = true;
                        gbx_DynThresh_2_Cell_Line_FL.Visible = false;
                    }
                    else if (str_num == "2")
                    {
                        gbx_DynThresh_1_Cell_Line_FL.Visible = false;
                        gbx_DynThresh_2_Cell_Line_FL.Visible = true;
                    }
                }

                return;
            }
            else if (tag == "str_BinaryImg_Resist") // (20190410) Jeff Revised!
            {
                if (combx_num_BinImg_Resist_Line_FL.Text != "1")
                    combx_num_BinImg_Resist_Line_FL.Text = "1";
                else
                    param_Line_FL_ValueChanged(combx_num_BinImg_Resist_Line_FL, e);
                tag = "Step2";
            }
            else if (tag == "num_BinImg_Resist") // (20190410) Jeff Revised!
            {
                string str_BinaryImg = combx_str_BinaryImg_Resist_Line_FL.Text;
                string str_num = c.Text;
                if (str_BinaryImg == "固定閥值")
                {
                    gbx_DynThresh_1_Resist_Line_FL.Visible = false;
                    gbx_DynThresh_2_Resist_Line_FL.Visible = false;
                    if (str_num == "1")
                    {
                        gbx_ConstThresh_1_Resist_Line_FL.Visible = true;
                        gbx_ConstThresh_2_Resist_Line_FL.Visible = false;
                    }
                    else if (str_num == "2")
                    {
                        gbx_ConstThresh_1_Resist_Line_FL.Visible = false;
                        gbx_ConstThresh_2_Resist_Line_FL.Visible = true;
                    }
                }
                else if (str_BinaryImg == "動態閥值")
                {
                    gbx_ConstThresh_1_Resist_Line_FL.Visible = false;
                    gbx_ConstThresh_2_Resist_Line_FL.Visible = false;
                    if (str_num == "1")
                    {
                        gbx_DynThresh_1_Resist_Line_FL.Visible = true;
                        gbx_DynThresh_2_Resist_Line_FL.Visible = false;
                    }
                    else if (str_num == "2")
                    {
                        gbx_DynThresh_1_Resist_Line_FL.Visible = false;
                        gbx_DynThresh_2_Resist_Line_FL.Visible = true;
                    }
                }

                return;
            }
            else if (tag == "num_op1")
            {
                string str_num_op1 = combx_num_op1_Line_FL.Text;
                if (str_num_op1 == "1")
                {
                    gbx_op1_1_Line_FL.Visible = true;
                    gbx_op1_2_Line_FL.Visible = false;
                }
                else if (str_num_op1 == "2")
                {
                    gbx_op1_1_Line_FL.Visible = false;
                    gbx_op1_2_Line_FL.Visible = true;
                }

                return;
            }

            // Enabled狀態切換
            else if (tag == "count_BinImg_Cell") // (20190410) Jeff Revised!
            {
                if (nud_count_BinImg_Cell_Line_FL.Value > 1)
                    combx_num_BinImg_Cell_Line_FL.Enabled = true;
                else
                {
                    combx_num_BinImg_Cell_Line_FL.Text = "1";
                    combx_num_BinImg_Cell_Line_FL.Enabled = false;
                }
                tag = "Step2";
            }
            else if (tag == "count_BinImg_Resist") // (20190410) Jeff Revised!
            {
                if (nud_count_BinImg_Resist_Line_FL.Value > 1)
                    combx_num_BinImg_Resist_Line_FL.Enabled = true;
                else
                {
                    combx_num_BinImg_Resist_Line_FL.Text = "1";
                    combx_num_BinImg_Resist_Line_FL.Enabled = false;
                }
                tag = "Step2";
            }
            else if (tag == "count_op1")
            {
                if (nud_count_op1_Line_FL.Value > 1)
                    combx_num_op1_Line_FL.Enabled = true;
                else
                {
                    combx_num_op1_Line_FL.Text = "1";
                    combx_num_op1_Line_FL.Enabled = false;
                }
                tag = "Step5";
            }

            if (b_LoadRecipe) return;

            // 相同啟用項目同步更新
            switch (tag)
            {
                case "Enabled_Cell_Step1":
                    {
                        b_LoadRecipe = true;
                        cbx_Enabled_Cell_Step2_Line_FL.Checked = cbx_Enabled_Cell_Step1_Line_FL.Checked;
                        b_LoadRecipe = false;
                        tag = "Step1"; // (20190411) Jeff Revised!
                    }
                    break;
                case "Enabled_Cell_Step2":
                    {
                        b_LoadRecipe = true;
                        cbx_Enabled_Cell_Step1_Line_FL.Checked = cbx_Enabled_Cell_Step2_Line_FL.Checked;
                        b_LoadRecipe = false;
                        tag = "Step2"; // (20190411) Jeff Revised!
                    }
                    break;
                case "Enabled_BlackStripe_Step1":
                    {
                        b_LoadRecipe = true;
                        cbx_Enabled_BlackStripe_Step2_Line_FL.Checked = cbx_Enabled_BlackStripe_Step1_Line_FL.Checked;
                        b_LoadRecipe = false;
                        tag = "Step1"; // (20190411) Jeff Revised!
                    }
                    break;
                case "Enabled_BlackStripe_Step2":
                    {
                        b_LoadRecipe = true;
                        cbx_Enabled_BlackStripe_Step1_Line_FL.Checked = cbx_Enabled_BlackStripe_Step2_Line_FL.Checked;
                        b_LoadRecipe = false;
                        tag = "Step2"; // (20190411) Jeff Revised!
                    }
                    break;
            }

            if (Input_ImgList.Count <= 0)
            {
                MessageBox.Show("請先載入影像");
                return;
            }

            ParamGetFromUI();
            if (!Inspection_Line_FL())
            {
                clear_rbtn_Step_Line_FL();
                MessageBox.Show("檢測失敗", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            b_Initial_State = false;

            switch (tag)
            {
                case "ImageID":
                    {

                    }
                    break;
                case "Step1":
                    {
                        if (rbtn_Step1_Line_FL.Checked) rbtn_Step_Line_FL_CheckedChanged(rbtn_Step1_Line_FL, e);
                        else rbtn_Step1_Line_FL.Checked = true;
                    }
                    break;
                case "Step2":
                    {
                        if (rbtn_Step2_Line_FL.Checked) rbtn_Step_Line_FL_CheckedChanged(rbtn_Step2_Line_FL, e);
                        else rbtn_Step2_Line_FL.Checked = true;
                    }
                    break;
                case "Step3":
                    {
                        if (rbtn_Step3_Line_FL.Checked) rbtn_Step_Line_FL_CheckedChanged(rbtn_Step3_Line_FL, e);
                        else rbtn_Step3_Line_FL.Checked = true;
                    }
                    break;
                case "Step4":
                    {
                        if (rbtn_Step4_Line_FL.Checked) rbtn_Step_Line_FL_CheckedChanged(rbtn_Step4_Line_FL, e);
                        else rbtn_Step4_Line_FL.Checked = true;
                    }
                    break;
                case "Step5":
                    {
                        if (rbtn_Step5_Line_FL.Checked) rbtn_Step_Line_FL_CheckedChanged(rbtn_Step5_Line_FL, e);
                        else rbtn_Step5_Line_FL.Checked = true;
                    }
                    break;
                case "Step6":
                    {
                        if (rbtn_Step6_Line_FL.Checked) rbtn_Step_Line_FL_CheckedChanged(rbtn_Step6_Line_FL, e);
                        else rbtn_Step6_Line_FL.Checked = true;
                    }
                    break;
            }
        }

        /// <summary>
        /// 清除 檢測絲狀異物 (正光) 內所有 rbtn_Step
        /// </summary>
        private void clear_rbtn_Step_Line_FL()
        {
            rbtn_Step1_Line_FL.Checked = false;
            rbtn_Step2_Line_FL.Checked = false;
            rbtn_Step3_Line_FL.Checked = false;
            rbtn_Step4_Line_FL.Checked = false;
            rbtn_Step5_Line_FL.Checked = false;
            rbtn_Step6_Line_FL.Checked = false;
        }
        #endregion

        #region 檢測絲狀異物 (正光&同軸光)
        private void btn_instruction_Line_FLCL_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string tag = btn.Tag.ToString();
            switch (tag)
            {
                case "Step1":
                    {
                        rbtn_Step1_Line_FLCL.Checked = true;
                        richTextBox_log.AppendText("※檢測絲狀異物 (黑條內白點) (正光&同軸光) Step1:" + "\n");
                        richTextBox_log.AppendText("對 檢測絲狀異物 (正光) & 檢測黑條內黑點2 (同軸光) 兩瑕疵區域做邏輯運算" + "\n\n");
                    }
                    break;
                case "Step2":
                    {
                        rbtn_Step2_Line_FLCL.Checked = true;
                        richTextBox_log.AppendText("※檢測絲狀異物 (黑條內白點) (正光&同軸光) Step2:" + "\n");
                        richTextBox_log.AppendText("瑕疵尺寸篩選" + "\n\n");
                    }
                    break;
                case "Step3":
                    {
                        rbtn_Step3_Line_FLCL.Checked = true;
                        richTextBox_log.AppendText("※檢測絲狀異物 (黑條內白點) (正光&同軸光) Step3:" + "\n");
                        richTextBox_log.AppendText("瑕疵尺寸篩選" + "\n\n");
                    }
                    break;
                case "Step4":
                    {
                        rbtn_Step4_Line_FLCL.Checked = true;
                        richTextBox_log.AppendText("※檢測絲狀異物 (黑條內白點) (正光&同軸光) Step4:" + "\n");
                        richTextBox_log.AppendText("消除誤判區域內，符合檢出規格設定3之瑕疵區域" + "\n");
                        richTextBox_log.AppendText("Note: 綠色區域為符合檢出規格設定3之瑕疵區域，最終瑕疵以紅色標示" + "\n\n");
                    }
                    break;
            }

            richTextBox_log.ScrollToCaret();
        }

        private void rbtn_Step_Line_FLCL_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbtn = sender as RadioButton;
            if (rbtn.Checked == false) return;

            if (Input_ImgList.Count <= 0)
            {
                rbtn.Checked = false;
                MessageBox.Show("請先載入影像");
                return;
            }

            if (b_Initial_State)
            {
                if (Inspection_Line_FLCL()) b_Initial_State = false;
                else
                {
                    rbtn.Checked = false;
                    MessageBox.Show("檢測失敗", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            combxDisplayImg.SelectedIndex = int.Parse(nud_ImageID_Line_FL.Value.ToString());
            Extension.HObjectMedthods.ReleaseHObject(ref Current_disp_image); // (20190816) Jeff Revised!
            Current_disp_regions.Dispose();
            HOperatorSet.GenEmptyObj(out Current_disp_regions);
            string tag = rbtn.Tag.ToString();
            switch (tag)
            {
                case "Step1":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_BlackBlob2_CL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_DefectReg_op1_2Images != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.CopyObj(Method.ho_DefectReg_op1_2Images, out Current_disp_regions, 1, -1);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step2":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_BlackBlob2_CL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_DefectReg_op1 != null)
                        {
                            Current_disp_regions.Dispose();
                            //HOperatorSet.CopyObj(Method.ho_DefectReg_op1, out Current_disp_regions, 1, -1);
                            HObject DefectReg_op1_union = null;
                            HOperatorSet.Union1(Method.ho_DefectReg_op1, out DefectReg_op1_union);
                            HOperatorSet.Connection(DefectReg_op1_union, out Current_disp_regions);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step3":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_BlackBlob2_CL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_DefectReg_op2_2Images != null)
                        {
                            Current_disp_regions.Dispose();
                            //HOperatorSet.CopyObj(Method.ho_DefectReg_op2_2Images, out Current_disp_regions, 1, -1);
                            HOperatorSet.Connection(Method.ho_DefectReg_op2_2Images, out Current_disp_regions); // (20190331) Jeff Revised!
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step4":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_BlackBlob2_CL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_DefectReg_Line_FLCL != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Method.ho_DefectReg_Line_FLCL, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Rect_BypassReg != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "blue");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Method.ho_Rect_BypassReg, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_DefectReg_op3_2Images != null)
                        {
                            Current_disp_regions.Dispose();
                            //HOperatorSet.CopyObj(Method.ho_DefectReg_op3_2Images, out Current_disp_regions, 1, -1);
                            HOperatorSet.Connection(Method.ho_DefectReg_op3_2Images, out Current_disp_regions);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "green");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
            }
        }

        private bool Inspection_Line_FLCL()
        {
            if (!(Method.Read_AllImages_Insp(Recipe, Input_ImgList))) return false;
            if (!(Method.InitialPosition_Insp(Recipe))) return false;
            if (cbx_CellSeg_CL.Checked)
            {
                if (!(Method.CellSeg_CL_Insp(Recipe))) return false;
            }
            else
            {
                if (!(Method.CellSeg_BL_Insp(Recipe))) return false;
            }
            if (!(Method.Df_CellReg(Recipe))) return false; // (20190810) Jeff Revised!
            //if (!(Method.Line_FL_Insp(Recipe))) return false;
            //if (!(Method.BlackBlob2_CL_Insp(Recipe))) return false;
            if (!(Method.Line_FLCL_Insp(Recipe))) return false;
            return true;
        }

        private void param_Line_FLCL_ValueChanged(object sender, EventArgs e)
        {
            Control c = sender as Control;
            string tag = c.Tag.ToString();
            // Visible狀態切換
            if (tag == "num_op1")
            {
                string str_num_op1 = combx_num_op1_Line_FLCL.Text;
                if (str_num_op1 == "1")
                {
                    gbx_op1_1_Line_FLCL.Visible = true;
                    gbx_op1_2_Line_FLCL.Visible = false;
                }
                else if (str_num_op1 == "2")
                {
                    gbx_op1_1_Line_FLCL.Visible = false;
                    gbx_op1_2_Line_FLCL.Visible = true;
                }

                return;
            }
            else if(tag == "num_op3") // (20190417) Jeff Revised!
            {
                string str_num_op3 = combx_num_op3_Line_FLCL.Text;
                if (str_num_op3 == "1")
                {
                    gbx_op3_1_Line_FLCL.Visible = true;
                    gbx_op3_2_Line_FLCL.Visible = false;
                }
                else if (str_num_op3 == "2")
                {
                    gbx_op3_1_Line_FLCL.Visible = false;
                    gbx_op3_2_Line_FLCL.Visible = true;
                }

                return;
            }

            // Enabled狀態切換
            else if (tag == "count_op1")
            {
                if (nud_count_op1_Line_FLCL.Value > 1)
                    combx_num_op1_Line_FLCL.Enabled = true;
                else
                {
                    combx_num_op1_Line_FLCL.Text = "1";
                    combx_num_op1_Line_FLCL.Enabled = false;
                }
                tag = "Step2";
            }
            else if (tag == "count_op3") // (20190417) Jeff Revised!
            {
                if (nud_count_op3_Line_FLCL.Value > 1)
                    combx_num_op3_Line_FLCL.Enabled = true;
                else
                {
                    combx_num_op3_Line_FLCL.Text = "1";
                    combx_num_op3_Line_FLCL.Enabled = false;
                }
                tag = "Step4";
            }

            if (b_LoadRecipe) return;
            
            if (Input_ImgList.Count <= 0)
            {
                MessageBox.Show("請先載入影像");
                return;
            }

            ParamGetFromUI();
            if (!Inspection_Line_FLCL())
            {
                clear_rbtn_Step_Line_FLCL();
                MessageBox.Show("檢測失敗", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            b_Initial_State = false;

            switch (tag)
            {
                case "ImageID":
                    {

                    }
                    break;
                case "Step1":
                    {
                        if (rbtn_Step1_Line_FLCL.Checked) rbtn_Step_Line_FLCL_CheckedChanged(rbtn_Step1_Line_FLCL, e);
                        else rbtn_Step1_Line_FLCL.Checked = true;
                    }
                    break;
                case "Step2":
                    {
                        if (rbtn_Step2_Line_FLCL.Checked) rbtn_Step_Line_FLCL_CheckedChanged(rbtn_Step2_Line_FLCL, e);
                        else rbtn_Step2_Line_FLCL.Checked = true;
                    }
                    break;
                case "Step3":
                    {
                        if (rbtn_Step3_Line_FLCL.Checked) rbtn_Step_Line_FLCL_CheckedChanged(rbtn_Step3_Line_FLCL, e);
                        else rbtn_Step3_Line_FLCL.Checked = true;
                    }
                    break;
                case "Step4":
                    {
                        if (rbtn_Step4_Line_FLCL.Checked) rbtn_Step_Line_FLCL_CheckedChanged(rbtn_Step4_Line_FLCL, e);
                        else rbtn_Step4_Line_FLCL.Checked = true;
                    }
                    break;
            }
        }

        /// <summary>
        /// 清除 檢測絲狀異物 (正光&同軸光) 內所有 rbtn_Step
        /// </summary>
        private void clear_rbtn_Step_Line_FLCL()
        {
            rbtn_Step1_Line_FLCL.Checked = false;
            rbtn_Step2_Line_FLCL.Checked = false;
            rbtn_Step3_Line_FLCL.Checked = false;
            rbtn_Step4_Line_FLCL.Checked = false;
        }
        #endregion

        #region 檢測白條內黑點 (正光)
        private void btn_instruction_FL2_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string tag = btn.Tag.ToString();
            switch (tag)
            {
                case "Step1":
                    {
                        rbtn_Step1_BlackCrack_FL.Checked = true;
                        richTextBox_log.AppendText("※檢測白條內黑點 (正光) Step1:" + "\n");
                        richTextBox_log.AppendText("設定檢測ROI (針對 黑色長條電阻區)" + "\n\n");
                    }
                    break;
                case "Step2":
                    {
                        rbtn_Step2_BlackCrack_FL.Checked = true;
                        richTextBox_log.AppendText("※檢測白條內黑點 (正光) Step2:" + "\n");
                        richTextBox_log.AppendText("二值化處理 (針對 黑條區域的補集合)" + "\n\n");
                    }
                    break;
                case "Step3":
                    {
                        rbtn_Step3_BlackCrack_FL.Checked = true;
                        richTextBox_log.AppendText("※檢測白條內黑點 (正光) Step3:" + "\n");
                        richTextBox_log.AppendText("設定檢測ROI 2 (針對 各顆Cell)" + "\n\n");
                    }
                    break;
                case "Step4":
                    {
                        rbtn_Step4_BlackCrack_FL.Checked = true;
                        richTextBox_log.AppendText("※檢測白條內黑點 (正光) Step4:" + "\n");
                        richTextBox_log.AppendText("瑕疵尺寸篩選" + "\n\n");
                    }
                    break;
                case "Step5":
                    {
                        rbtn_Step5_BlackCrack_FL.Checked = true;
                        richTextBox_log.AppendText("※檢測白條內黑點 (正光) Step5:" + "\n");
                        richTextBox_log.AppendText("瑕疵尺寸篩選" + "\n\n");
                    }
                    break;
            }

            richTextBox_log.ScrollToCaret();
        }

        private void rbtn_Step_BlackCrack_FL_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbtn = sender as RadioButton;
            if (rbtn.Checked == false) return;

            if (Input_ImgList.Count <= 0)
            {
                rbtn.Checked = false;
                MessageBox.Show("請先載入影像");
                return;
            }

            if (b_Initial_State)
            {
                if (Inspection_BlackCrack_FL()) b_Initial_State = false;
                else
                {
                    rbtn.Checked = false;
                    MessageBox.Show("檢測失敗", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            combxDisplayImg.SelectedIndex = int.Parse(nud_ImageID_BlackCrack_FL.Value.ToString());
            Extension.HObjectMedthods.ReleaseHObject(ref Current_disp_image); // (20190816) Jeff Revised!
            Current_disp_regions.Dispose();
            HOperatorSet.GenEmptyObj(out Current_disp_regions);
            string tag = rbtn.Tag.ToString();
            switch (tag)
            {
                case "Step1":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_BlackCrack_FL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_Rect_BlackStripe != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.CopyObj(Method.ho_Rect_BlackStripe, out Current_disp_regions, 1, -1);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step2":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_BlackCrack_FL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_Rect_BlackStripe != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "blue");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Method.ho_Rect_BlackStripe, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_DefectReg_BlackCrack_thresh != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.CopyObj(Method.ho_DefectReg_BlackCrack_thresh, out Current_disp_regions, 1, -1);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step3":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_BlackCrack_FL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_Rect_AllCells != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "green");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Method.ho_Rect_AllCells, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_DefectReg_BlackCrack_CellReg != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.CopyObj(Method.ho_DefectReg_BlackCrack_CellReg, out Current_disp_regions, 1, -1);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step4":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_BlackCrack_FL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_DefectReg_op1 != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.CopyObj(Method.ho_DefectReg_op1, out Current_disp_regions, 1, -1);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step5":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_BlackCrack_FL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_DefectReg_BlackCrack_FL != null)
                        {
                            Current_disp_regions.Dispose();
                            //HOperatorSet.CopyObj(Method.ho_DefectReg_BlackCrack_FL, out Current_disp_regions, 1, -1);
                            HOperatorSet.Connection(Method.ho_DefectReg_BlackCrack_FL, out Current_disp_regions); // (20190331) Jeff Revised!
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
            }
        }
        
        private bool Inspection_BlackCrack_FL()
        {
            if (!(Method.Read_AllImages_Insp(Recipe, Input_ImgList))) return false;
            if (!(Method.InitialPosition_Insp(Recipe))) return false;
            if (cbx_CellSeg_CL.Checked)
            {
                if (!(Method.CellSeg_CL_Insp(Recipe))) return false;
            }
            else
            {
                if (!(Method.CellSeg_BL_Insp(Recipe))) return false;
            }
            if (!(Method.Df_CellReg(Recipe))) return false; // (20190810) Jeff Revised!
            if (!(Method.BlackCrack_FL_Insp(Recipe))) return false;
            return true;
        }

        private void param_BlackCrack_FL_ValueChanged(object sender, EventArgs e)
        {
            Control c = sender as Control;
            string tag = c.Tag.ToString();
            // Visible狀態切換
            if (tag == "str_ModeBlackStripe")
            {
                string str_ModeBlackStripe = c.Text;
                if (str_ModeBlackStripe == "mode1")
                {
                    nud_H_BlackStripe_BlackCrack_FL.Visible = true;
                    nud_H_BlackStripe_mode2_BlackCrack_FL.Visible = false;
                    lab_unit_BlackCrack_FL.Text = "(μm)";
                }
                else if (str_ModeBlackStripe == "mode2")
                {
                    nud_H_BlackStripe_BlackCrack_FL.Visible = false;
                    nud_H_BlackStripe_mode2_BlackCrack_FL.Visible = true;
                    lab_unit_BlackCrack_FL.Text = "(pixel)";
                }
                tag = "Step1"; // (20190412) Jeff Revised!
            }

            if (b_LoadRecipe) return;

            if (Input_ImgList.Count <= 0)
            {
                MessageBox.Show("請先載入影像");
                return;
            }

            ParamGetFromUI();
            if (!Inspection_BlackCrack_FL())
            {
                clear_rbtn_Step_BlackCrack_FL();
                MessageBox.Show("檢測失敗", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            b_Initial_State = false;

            switch (tag)
            {
                case "ImageID":
                    {

                    }
                    break;
                case "Step1":
                    {
                        if (rbtn_Step1_BlackCrack_FL.Checked) rbtn_Step_BlackCrack_FL_CheckedChanged(rbtn_Step1_BlackCrack_FL, e);
                        else rbtn_Step1_BlackCrack_FL.Checked = true;
                    }
                    break;
                case "Step2":
                    {
                        if (rbtn_Step2_BlackCrack_FL.Checked) rbtn_Step_BlackCrack_FL_CheckedChanged(rbtn_Step2_BlackCrack_FL, e);
                        else rbtn_Step2_BlackCrack_FL.Checked = true;
                    }
                    break;
                case "Step3":
                    {
                        if (rbtn_Step3_BlackCrack_FL.Checked) rbtn_Step_BlackCrack_FL_CheckedChanged(rbtn_Step3_BlackCrack_FL, e);
                        else rbtn_Step3_BlackCrack_FL.Checked = true;
                    }
                    break;
                case "Step4":
                    {
                        if (rbtn_Step4_BlackCrack_FL.Checked) rbtn_Step_BlackCrack_FL_CheckedChanged(rbtn_Step4_BlackCrack_FL, e);
                        else rbtn_Step4_BlackCrack_FL.Checked = true;
                    }
                    break;
                case "Step5":
                    {
                        if (rbtn_Step5_BlackCrack_FL.Checked) rbtn_Step_BlackCrack_FL_CheckedChanged(rbtn_Step5_BlackCrack_FL, e);
                        else rbtn_Step5_BlackCrack_FL.Checked = true;
                    }
                    break;
            }
        }

        /// <summary>
        /// 清除 檢測白條內黑點 (正光) 內所有 rbtn_Step
        /// </summary>
        private void clear_rbtn_Step_BlackCrack_FL() // (20190216) Jeff Revised!
        {
            rbtn_Step1_BlackCrack_FL.Checked = false;
            rbtn_Step2_BlackCrack_FL.Checked = false;
            rbtn_Step3_BlackCrack_FL.Checked = false;
            rbtn_Step4_BlackCrack_FL.Checked = false;
            rbtn_Step5_BlackCrack_FL.Checked = false;
        }
        #endregion

        #region 檢測白條內黑點 (同軸光)
        private void btn_instruction_CL_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string tag = btn.Tag.ToString();
            switch (tag)
            {
                case "Step1":
                    {
                        rbtn_Step1_BlackCrack_CL.Checked = true;
                        richTextBox_log.AppendText("※檢測白條內黑點 (同軸光) Step1:" + "\n");
                        richTextBox_log.AppendText("設定檢測ROI (針對 黑色長條電阻區)" + "\n\n");
                    }
                    break;
                case "Step2":
                    {
                        rbtn_Step2_BlackCrack_CL.Checked = true;
                        richTextBox_log.AppendText("※檢測白條內黑點 (同軸光) Step2:" + "\n");
                        richTextBox_log.AppendText("二值化處理 (針對 黑條區域的補集合)" + "\n\n");
                    }
                    break;
                case "Step3":
                    {
                        rbtn_Step3_BlackCrack_CL.Checked = true;
                        richTextBox_log.AppendText("※檢測白條內黑點 (同軸光) Step3:" + "\n");
                        richTextBox_log.AppendText("設定檢測ROI 2 (針對 各顆Cell)" + "\n\n");
                    }
                    break;
                case "Step4":
                    {
                        rbtn_Step4_BlackCrack_CL.Checked = true;
                        richTextBox_log.AppendText("※檢測白條內黑點 (同軸光) Step4:" + "\n");
                        richTextBox_log.AppendText("瑕疵尺寸篩選" + "\n\n");
                    }
                    break;
                case "Step5":
                    {
                        rbtn_Step5_BlackCrack_CL.Checked = true;
                        richTextBox_log.AppendText("※檢測白條內黑點 (同軸光) Step5:" + "\n");
                        richTextBox_log.AppendText("瑕疵尺寸篩選" + "\n\n");
                    }
                    break;
            }

            richTextBox_log.ScrollToCaret();
        }

        private void rbtn_Step_BlackCrack_CL_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbtn = sender as RadioButton;
            if (rbtn.Checked == false) return;

            if (Input_ImgList.Count <= 0)
            {
                rbtn.Checked = false;
                MessageBox.Show("請先載入影像");
                return;
            }

            if (b_Initial_State)
            {
                if (Inspection_BlackCrack_CL()) b_Initial_State = false;
                else
                {
                    rbtn.Checked = false;
                    MessageBox.Show("檢測失敗", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            combxDisplayImg.SelectedIndex = int.Parse(nud_ImageID_BlackCrack_CL.Value.ToString());
            Extension.HObjectMedthods.ReleaseHObject(ref Current_disp_image); // (20190816) Jeff Revised!
            Current_disp_regions.Dispose();
            HOperatorSet.GenEmptyObj(out Current_disp_regions);
            string tag = rbtn.Tag.ToString();
            switch (tag)
            {
                case "Step1":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_BlackCrack_CL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_Rect_BlackStripe != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.CopyObj(Method.ho_Rect_BlackStripe, out Current_disp_regions, 1, -1);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step2":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_BlackCrack_CL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_Rect_BlackStripe != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "blue");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Method.ho_Rect_BlackStripe, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_DefectReg_BlackCrack_thresh != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.CopyObj(Method.ho_DefectReg_BlackCrack_thresh, out Current_disp_regions, 1, -1);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step3":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_BlackCrack_CL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_Rect_AllCells != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "green");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Method.ho_Rect_AllCells, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_DefectReg_BlackCrack_CellReg != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.CopyObj(Method.ho_DefectReg_BlackCrack_CellReg, out Current_disp_regions, 1, -1);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step4":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_BlackCrack_CL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_DefectReg_op1 != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.CopyObj(Method.ho_DefectReg_op1, out Current_disp_regions, 1, -1);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step5":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_BlackCrack_CL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_DefectReg_BlackCrack_CL != null)
                        {
                            Current_disp_regions.Dispose();
                            //HOperatorSet.CopyObj(Method.ho_DefectReg_BlackCrack_CL, out Current_disp_regions, 1, -1);
                            HOperatorSet.Connection(Method.ho_DefectReg_BlackCrack_CL, out Current_disp_regions); // (20190331) Jeff Revised!
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
            }
        }

        private bool Inspection_BlackCrack_CL()
        {
            if (!(Method.Read_AllImages_Insp(Recipe, Input_ImgList))) return false;
            if (!(Method.InitialPosition_Insp(Recipe))) return false;
            if (cbx_CellSeg_CL.Checked)
            {
                if (!(Method.CellSeg_CL_Insp(Recipe))) return false;
            }
            else
            {
                if (!(Method.CellSeg_BL_Insp(Recipe))) return false;
            }
            if (!(Method.Df_CellReg(Recipe))) return false; // (20190810) Jeff Revised!
            if (!(Method.BlackCrack_CL_Insp(Recipe))) return false;
            return true;
        }

        private void param_BlackCrack_CL_ValueChanged(object sender, EventArgs e)
        {
            Control c = sender as Control;
            string tag = c.Tag.ToString();
            // Visible狀態切換
            if (tag == "str_ModeBlackStripe")
            {
                string str_ModeBlackStripe = c.Text;
                if (str_ModeBlackStripe == "mode1")
                {
                    nud_H_BlackStripe_BlackCrack_CL.Visible = true;
                    nud_H_BlackStripe_mode2_BlackCrack_CL.Visible = false;
                    lab_unit_BlackCrack_CL.Text = "(μm)";
                }
                else if (str_ModeBlackStripe == "mode2")
                {
                    nud_H_BlackStripe_BlackCrack_CL.Visible = false;
                    nud_H_BlackStripe_mode2_BlackCrack_CL.Visible = true;
                    lab_unit_BlackCrack_CL.Text = "(pixel)";
                }
                tag = "Step1"; // (20190412) Jeff Revised!
            }

            if (b_LoadRecipe) return;

            if (Input_ImgList.Count <= 0)
            {
                MessageBox.Show("請先載入影像");
                return;
            }

            ParamGetFromUI();
            if (!Inspection_BlackCrack_CL())
            {
                clear_rbtn_Step_BlackCrack_CL();
                MessageBox.Show("檢測失敗", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            b_Initial_State = false;

            switch (tag)
            {
                case "ImageID":
                    {

                    }
                    break;
                case "Step1":
                    {
                        if (rbtn_Step1_BlackCrack_CL.Checked) rbtn_Step_BlackCrack_CL_CheckedChanged(rbtn_Step1_BlackCrack_CL, e);
                        else rbtn_Step1_BlackCrack_CL.Checked = true;
                    }
                    break;
                case "Step2":
                    {
                        if (rbtn_Step2_BlackCrack_CL.Checked) rbtn_Step_BlackCrack_CL_CheckedChanged(rbtn_Step2_BlackCrack_CL, e);
                        else rbtn_Step2_BlackCrack_CL.Checked = true;
                    }
                    break;
                case "Step3":
                    {
                        if (rbtn_Step3_BlackCrack_CL.Checked) rbtn_Step_BlackCrack_CL_CheckedChanged(rbtn_Step3_BlackCrack_CL, e);
                        else rbtn_Step3_BlackCrack_CL.Checked = true;
                    }
                    break;
                case "Step4":
                    {
                        if (rbtn_Step4_BlackCrack_CL.Checked) rbtn_Step_BlackCrack_CL_CheckedChanged(rbtn_Step4_BlackCrack_CL, e);
                        else rbtn_Step4_BlackCrack_CL.Checked = true;
                    }
                    break;
                case "Step5":
                    {
                        if (rbtn_Step5_BlackCrack_CL.Checked) rbtn_Step_BlackCrack_CL_CheckedChanged(rbtn_Step5_BlackCrack_CL, e);
                        else rbtn_Step5_BlackCrack_CL.Checked = true;
                    }
                    break;
            }
        }

        /// <summary>
        /// 清除 檢測白條內黑點 (同軸光) 內所有 rbtn_Step
        /// </summary>
        private void clear_rbtn_Step_BlackCrack_CL()
        {
            rbtn_Step1_BlackCrack_CL.Checked = false;
            rbtn_Step2_BlackCrack_CL.Checked = false;
            rbtn_Step3_BlackCrack_CL.Checked = false;
            rbtn_Step4_BlackCrack_CL.Checked = false;
            rbtn_Step5_BlackCrack_CL.Checked = false;
        }
        #endregion

        #region 檢測黑條內黑點2 (同軸光)
        private void btn_instruction_BlackBlob2_CL_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string tag = btn.Tag.ToString();
            switch (tag)
            {
                case "Step1":
                    {
                        rbtn_Step1_BlackBlob2_CL.Checked = true;
                        richTextBox_log.AppendText("※檢測黑條內黑點2 (同軸光) Step1:" + "\n");
                        richTextBox_log.AppendText("設定檢測ROI (針對 各顆Cell, Cell內忽略區域 & 黑色長條電阻區)" + "\n\n");
                    }
                    break;
                case "Step2":
                    {
                        rbtn_Step2_BlackBlob2_CL.Checked = true;
                        richTextBox_log.AppendText("※檢測黑條內黑點2 (同軸光) Step2:" + "\n");
                        richTextBox_log.AppendText("二值化處理 (針對 各顆Cell不含忽略區域 & 黑色長條電阻區不含各顆Cell,及Cell內忽略區域)" + "\n");
                        richTextBox_log.AppendText("Note: " + "\n\n");
                    }
                    break;
                case "Step3":
                    {
                        rbtn_Step3_BlackBlob2_CL.Checked = true;
                        richTextBox_log.AppendText("※檢測黑條內黑點2 (同軸光) Step3:" + "\n");
                        richTextBox_log.AppendText("瑕疵Region內(同軸光)至少有一個pixel之灰階值低於一個數值 (瑕疵Region內最低灰階標準)" + "\n\n");
                    }
                    break;
                case "Step4":
                    {
                        rbtn_Step4_BlackBlob2_CL.Checked = true;
                        richTextBox_log.AppendText("※檢測黑條內黑點2 (同軸光) Step4:" + "\n");
                        richTextBox_log.AppendText("設定檢測ROI 2 (針對 各顆Cell)" + "\n\n");
                    }
                    break;
                case "Step5":
                    {
                        rbtn_Step5_BlackBlob2_CL.Checked = true;
                        richTextBox_log.AppendText("※檢測黑條內黑點2 (同軸光) Step5:" + "\n");
                        richTextBox_log.AppendText("瑕疵尺寸篩選" + "\n\n");
                    }
                    break;
                case "Step6":
                    {
                        rbtn_Step6_BlackBlob2_CL.Checked = true;
                        richTextBox_log.AppendText("※檢測黑條內黑點2 (同軸光) Step6:" + "\n");
                        richTextBox_log.AppendText("瑕疵尺寸篩選" + "\n\n");
                    }
                    break;
            }

            richTextBox_log.ScrollToCaret();
        }

        private void rbtn_Step_BlackBlob2_CL_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbtn = sender as RadioButton;
            if (rbtn.Checked == false) return;

            if (Input_ImgList.Count <= 0)
            {
                rbtn.Checked = false;
                MessageBox.Show("請先載入影像");
                return;
            }

            if (b_Initial_State)
            {
                if (Inspection_BlackBlob2_CL()) b_Initial_State = false;
                else
                {
                    rbtn.Checked = false;
                    MessageBox.Show("檢測失敗", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            combxDisplayImg.SelectedIndex = int.Parse(nud_ImageID_BlackBlob2_CL.Value.ToString());
            Extension.HObjectMedthods.ReleaseHObject(ref Current_disp_image); // (20190816) Jeff Revised!
            Current_disp_regions.Dispose();
            HOperatorSet.GenEmptyObj(out Current_disp_regions);
            string tag = rbtn.Tag.ToString();
            switch (tag)
            {
                case "Step1":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_BlackBlob2_CL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_Rect_AllCells != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "green");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Method.ho_Rect_AllCells, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Rect_BypassReg != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "blue");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Method.ho_Rect_BypassReg, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Rect_BlackStripe != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.CopyObj(Method.ho_Rect_BlackStripe, out Current_disp_regions, 1, -1);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step2":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_BlackBlob2_CL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_Reg_Cell_Thresh != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Method.ho_Reg_Cell_Thresh, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Reg_Resist_Thresh != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "pink");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Method.ho_Reg_Resist_Thresh, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Rect_AllCells != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "green");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Method.ho_Rect_AllCells, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Rect_BypassReg != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "blue");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Method.ho_Rect_BypassReg, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Rect_BlackStripe != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Method.ho_Rect_BlackStripe, DisplayWindows.HalconWindow);
                        }

                        // 設定使用者點擊region
                        if (Method.ho_Reg_Cell_Thresh != null || Method.ho_Reg_Resist_Thresh != null)
                        {
                            HObject ho_Reg_Cell_Thresh, ho_Reg_Resist_Thresh;
                            HOperatorSet.Connection(Method.ho_Reg_Cell_Thresh, out ho_Reg_Cell_Thresh);
                            HOperatorSet.Connection(Method.ho_Reg_Resist_Thresh, out ho_Reg_Resist_Thresh);
                            Current_disp_regions.Dispose();
                            HOperatorSet.ConcatObj(ho_Reg_Cell_Thresh, ho_Reg_Resist_Thresh, out Current_disp_regions);
                            ho_Reg_Cell_Thresh.Dispose();
                            ho_Reg_Resist_Thresh.Dispose();
                        }
                    }
                    break;
                case "Step3":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_BlackBlob2_CL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_Reg_select != null)
                        {
                            HObject ho_Reg_select = null;
                            HOperatorSet.Connection(Method.ho_Reg_select, out ho_Reg_select);
                            Current_disp_regions.Dispose();
                            HOperatorSet.CopyObj(ho_Reg_select, out Current_disp_regions, 1, -1);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                            ho_Reg_select.Dispose();
                        }
                    }
                    break;
                case "Step4":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_BlackBlob2_CL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_Rect_AllCells2 != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "green");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Method.ho_Rect_AllCells2, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Reg_select_CellReg != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.CopyObj(Method.ho_Reg_select_CellReg, out Current_disp_regions, 1, -1);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step5":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_BlackBlob2_CL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_DefectReg_op1 != null)
                        {
                            Current_disp_regions.Dispose();
                            //HOperatorSet.CopyObj(Method.ho_DefectReg_op1, out Current_disp_regions, 1, -1);
                            HObject DefectReg_op1_union = null;
                            HOperatorSet.Union1(Method.ho_DefectReg_op1, out DefectReg_op1_union);
                            HOperatorSet.Connection(DefectReg_op1_union, out Current_disp_regions);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step6":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_BlackBlob2_CL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_DefectReg_BlackBlob2 != null)
                        {
                            Current_disp_regions.Dispose();
                            //HOperatorSet.CopyObj(Method.ho_DefectReg_BlackBlob2, out Current_disp_regions, 1, -1);
                            HOperatorSet.Connection(Method.ho_DefectReg_BlackBlob2, out Current_disp_regions); // (20190331) Jeff Revised!
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
            }
        }

        private bool Inspection_BlackBlob2_CL()
        {
            if (!(Method.Read_AllImages_Insp(Recipe, Input_ImgList))) return false;
            if (!(Method.InitialPosition_Insp(Recipe))) return false;
            if (cbx_CellSeg_CL.Checked)
            {
                if (!(Method.CellSeg_CL_Insp(Recipe))) return false;
            }
            else
            {
                if (!(Method.CellSeg_BL_Insp(Recipe))) return false;
            }
            if (!(Method.Df_CellReg(Recipe))) return false; // (20190810) Jeff Revised!
            if (!(Method.BlackBlob2_CL_Insp(Recipe))) return false;
            return true;
        }

        private void param_BlackBlob2_CL_ValueChanged(object sender, EventArgs e)
        {
            Control c = sender as Control;
            string tag = c.Tag.ToString();
            // Visible狀態切換
            if (tag == "str_ModeBlackStripe")
            {
                string str_ModeBlackStripe = c.Text;
                if (str_ModeBlackStripe == "mode1")
                {
                    nud_H_BlackStripe_BlackBlob2_CL.Visible = true;
                    nud_H_BlackStripe_mode2_BlackBlob2_CL.Visible = false;
                    lab_unit_BlackBlob2_CL.Text = "(μm)";
                }
                else if (str_ModeBlackStripe == "mode2")
                {
                    nud_H_BlackStripe_BlackBlob2_CL.Visible = false;
                    nud_H_BlackStripe_mode2_BlackBlob2_CL.Visible = true;
                    lab_unit_BlackBlob2_CL.Text = "(pixel)";
                }
                tag = "Step1";
            }
            else if (tag == "str_BinaryImg_Cell") // (20190410) Jeff Revised!
            {
                if (combx_num_BinImg_Cell_BlackBlob2_CL.Text != "1")
                    combx_num_BinImg_Cell_BlackBlob2_CL.Text = "1";
                else
                    param_BlackBlob2_CL_ValueChanged(combx_num_BinImg_Cell_BlackBlob2_CL, e);
                tag = "Step2";
            }
            else if (tag == "num_BinImg_Cell") // (20190410) Jeff Revised!
            {
                string str_BinaryImg = combx_str_BinaryImg_Cell_BlackBlob2_CL.Text;
                string str_num = c.Text;
                if (str_BinaryImg == "固定閥值")
                {
                    gbx_DynThresh_1_Cell_BlackBlob2_CL.Visible = false;
                    gbx_DynThresh_2_Cell_BlackBlob2_CL.Visible = false;
                    if (str_num == "1")
                    {
                        gbx_ConstThresh_1_Cell_BlackBlob2_CL.Visible = true;
                        gbx_ConstThresh_2_Cell_BlackBlob2_CL.Visible = false;
                    }
                    else if (str_num == "2")
                    {
                        gbx_ConstThresh_1_Cell_BlackBlob2_CL.Visible = false;
                        gbx_ConstThresh_2_Cell_BlackBlob2_CL.Visible = true;
                    }
                }
                else if (str_BinaryImg == "動態閥值")
                {
                    gbx_ConstThresh_1_Cell_BlackBlob2_CL.Visible = false;
                    gbx_ConstThresh_2_Cell_BlackBlob2_CL.Visible = false;
                    if (str_num == "1")
                    {
                        gbx_DynThresh_1_Cell_BlackBlob2_CL.Visible = true;
                        gbx_DynThresh_2_Cell_BlackBlob2_CL.Visible = false;
                    }
                    else if (str_num == "2")
                    {
                        gbx_DynThresh_1_Cell_BlackBlob2_CL.Visible = false;
                        gbx_DynThresh_2_Cell_BlackBlob2_CL.Visible = true;
                    }
                }

                return;
            }
            else if (tag == "str_BinaryImg_Resist") // (20190410) Jeff Revised!
            {
                if (combx_num_BinImg_Resist_BlackBlob2_CL.Text != "1")
                    combx_num_BinImg_Resist_BlackBlob2_CL.Text = "1";
                else
                    param_BlackBlob2_CL_ValueChanged(combx_num_BinImg_Resist_BlackBlob2_CL, e);
                tag = "Step2";
            }
            else if (tag == "num_BinImg_Resist") // (20190410) Jeff Revised!
            {
                string str_BinaryImg = combx_str_BinaryImg_Resist_BlackBlob2_CL.Text;
                string str_num = c.Text;
                if (str_BinaryImg == "固定閥值")
                {
                    gbx_DynThresh_1_Resist_BlackBlob2_CL.Visible = false;
                    gbx_DynThresh_2_Resist_BlackBlob2_CL.Visible = false;
                    if (str_num == "1")
                    {
                        gbx_ConstThresh_1_Resist_BlackBlob2_CL.Visible = true;
                        gbx_ConstThresh_2_Resist_BlackBlob2_CL.Visible = false;
                    }
                    else if (str_num == "2")
                    {
                        gbx_ConstThresh_1_Resist_BlackBlob2_CL.Visible = false;
                        gbx_ConstThresh_2_Resist_BlackBlob2_CL.Visible = true;
                    }
                }
                else if (str_BinaryImg == "動態閥值")
                {
                    gbx_ConstThresh_1_Resist_BlackBlob2_CL.Visible = false;
                    gbx_ConstThresh_2_Resist_BlackBlob2_CL.Visible = false;
                    if (str_num == "1")
                    {
                        gbx_DynThresh_1_Resist_BlackBlob2_CL.Visible = true;
                        gbx_DynThresh_2_Resist_BlackBlob2_CL.Visible = false;
                    }
                    else if (str_num == "2")
                    {
                        gbx_DynThresh_1_Resist_BlackBlob2_CL.Visible = false;
                        gbx_DynThresh_2_Resist_BlackBlob2_CL.Visible = true;
                    }
                }

                return;
            }
            else if (tag == "num_op1")
            {
                string str_num_op1 = combx_num_op1_BlackBlob2_CL.Text;
                if (str_num_op1 == "1")
                {
                    gbx_op1_1_BlackBlob2_CL.Visible = true;
                    gbx_op1_2_BlackBlob2_CL.Visible = false;
                }
                else if (str_num_op1 == "2")
                {
                    gbx_op1_1_BlackBlob2_CL.Visible = false;
                    gbx_op1_2_BlackBlob2_CL.Visible = true;
                }

                return;
            }

            // Enabled狀態切換
            else if (tag == "count_BinImg_Cell") // (20190410) Jeff Revised!
            {
                if (nud_count_BinImg_Cell_BlackBlob2_CL.Value > 1)
                    combx_num_BinImg_Cell_BlackBlob2_CL.Enabled = true;
                else
                {
                    combx_num_BinImg_Cell_BlackBlob2_CL.Text = "1";
                    combx_num_BinImg_Cell_BlackBlob2_CL.Enabled = false;
                }
                tag = "Step2";
            }
            else if (tag == "count_BinImg_Resist") // (20190410) Jeff Revised!
            {
                if (nud_count_BinImg_Resist_BlackBlob2_CL.Value > 1)
                    combx_num_BinImg_Resist_BlackBlob2_CL.Enabled = true;
                else
                {
                    combx_num_BinImg_Resist_BlackBlob2_CL.Text = "1";
                    combx_num_BinImg_Resist_BlackBlob2_CL.Enabled = false;
                }
                tag = "Step2";
            }
            else if (tag == "count_op1")
            {
                if (nud_count_op1_BlackBlob2_CL.Value > 1)
                    combx_num_op1_BlackBlob2_CL.Enabled = true;
                else
                {
                    combx_num_op1_BlackBlob2_CL.Text = "1";
                    combx_num_op1_BlackBlob2_CL.Enabled = false;
                }
                tag = "Step5";
            }

            if (b_LoadRecipe) return;

            // 相同啟用項目同步更新
            switch (tag)
            {
                case "Enabled_Cell_Step1":
                    {
                        b_LoadRecipe = true;
                        cbx_Enabled_Cell_Step2_BlackBlob2_CL.Checked = cbx_Enabled_Cell_Step1_BlackBlob2_CL.Checked;
                        b_LoadRecipe = false;
                        tag = "Step1";
                    }
                    break;
                case "Enabled_Cell_Step2":
                    {
                        b_LoadRecipe = true;
                        cbx_Enabled_Cell_Step1_BlackBlob2_CL.Checked = cbx_Enabled_Cell_Step2_BlackBlob2_CL.Checked;
                        b_LoadRecipe = false;
                        tag = "Step2";
                    }
                    break;
                case "Enabled_BlackStripe_Step1":
                    {
                        b_LoadRecipe = true;
                        cbx_Enabled_BlackStripe_Step2_BlackBlob2_CL.Checked = cbx_Enabled_BlackStripe_Step1_BlackBlob2_CL.Checked;
                        b_LoadRecipe = false;
                        tag = "Step1";
                    }
                    break;
                case "Enabled_BlackStripe_Step2":
                    {
                        b_LoadRecipe = true;
                        cbx_Enabled_BlackStripe_Step1_BlackBlob2_CL.Checked = cbx_Enabled_BlackStripe_Step2_BlackBlob2_CL.Checked;
                        b_LoadRecipe = false;
                        tag = "Step2";
                    }
                    break;
            }
            
            if (Input_ImgList.Count <= 0)
            {
                MessageBox.Show("請先載入影像");
                return;
            }
            
            ParamGetFromUI();
            if (!Inspection_BlackBlob2_CL())
            {
                clear_rbtn_Step_BlackBlob2_CL();
                MessageBox.Show("檢測失敗", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            b_Initial_State = false;
            
            switch (tag)
            {
                case "ImageID":
                    {
                        
                    }
                    break;
                case "Step1":
                    {
                        if (rbtn_Step1_BlackBlob2_CL.Checked) rbtn_Step_BlackBlob2_CL_CheckedChanged(rbtn_Step1_BlackBlob2_CL, e);
                        else rbtn_Step1_BlackBlob2_CL.Checked = true;
                    }
                    break;
                case "Step2":
                    {
                        if (rbtn_Step2_BlackBlob2_CL.Checked) rbtn_Step_BlackBlob2_CL_CheckedChanged(rbtn_Step2_BlackBlob2_CL, e);
                        else rbtn_Step2_BlackBlob2_CL.Checked = true;
                    }
                    break;
                case "Step3":
                    {
                        if (rbtn_Step3_BlackBlob2_CL.Checked) rbtn_Step_BlackBlob2_CL_CheckedChanged(rbtn_Step3_BlackBlob2_CL, e);
                        else rbtn_Step3_BlackBlob2_CL.Checked = true;
                    }
                    break;
                case "Step4":
                    {
                        if (rbtn_Step4_BlackBlob2_CL.Checked) rbtn_Step_BlackBlob2_CL_CheckedChanged(rbtn_Step4_BlackBlob2_CL, e);
                        else rbtn_Step4_BlackBlob2_CL.Checked = true;
                    }
                    break;
                case "Step5":
                    {
                        if (rbtn_Step5_BlackBlob2_CL.Checked) rbtn_Step_BlackBlob2_CL_CheckedChanged(rbtn_Step5_BlackBlob2_CL, e);
                        else rbtn_Step5_BlackBlob2_CL.Checked = true;
                    }
                    break;
                case "Step6":
                    {
                        if (rbtn_Step6_BlackBlob2_CL.Checked) rbtn_Step_BlackBlob2_CL_CheckedChanged(rbtn_Step6_BlackBlob2_CL, e);
                        else rbtn_Step6_BlackBlob2_CL.Checked = true;
                    }
                    break;
            }
        }

        /// <summary>
        /// 清除 檢測黑條內黑點2 (同軸光) 內所有 rbtn_Step
        /// </summary>
        private void clear_rbtn_Step_BlackBlob2_CL()
        {
            rbtn_Step1_BlackBlob2_CL.Checked = false;
            rbtn_Step2_BlackBlob2_CL.Checked = false;
            rbtn_Step3_BlackBlob2_CL.Checked = false;
            rbtn_Step4_BlackBlob2_CL.Checked = false;
            rbtn_Step5_BlackBlob2_CL.Checked = false;
            rbtn_Step6_BlackBlob2_CL.Checked = false;
        }
        #endregion

        #region 檢測汙染 (同軸光)
        private void btn_instruction_Dirty_CL_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string tag = btn.Tag.ToString();
            switch (tag)
            {
                case "Step1":
                    {
                        rbtn_Step1_Dirty_CL.Checked = true;
                        richTextBox_log.AppendText("※檢測汙染 (同軸光) Step1:" + "\n");
                        richTextBox_log.AppendText("設定檢測ROI (針對 各顆Cell, Cell內忽略區域 & 黑色長條電阻區)" + "\n\n");
                    }
                    break;
                case "Step2":
                    {
                        rbtn_Step2_Dirty_CL.Checked = true;
                        richTextBox_log.AppendText("※檢測汙染 (同軸光) Step2:" + "\n");
                        richTextBox_log.AppendText("二值化處理 (針對 各顆Cell不含忽略區域 & 黑色長條電阻區不含各顆Cell,及Cell內忽略區域)，並將啟用區域取聯集" + "\n");
                        richTextBox_log.AppendText("Note: " + "\n\n");
                    }
                    break;
                case "Step3":
                    {
                        rbtn_Step3_Dirty_CL.Checked = true;
                        richTextBox_log.AppendText("※檢測汙染 (同軸光) Step3:" + "\n");
                        richTextBox_log.AppendText("瑕疵Region內(同軸光)至少有一個pixel之灰階值高於一個數值 (瑕疵Region內最高灰階標準)" + "\n\n");
                    }
                    break;
                case "Step4":
                    {
                        rbtn_Step4_Dirty_CL.Checked = true;
                        richTextBox_log.AppendText("※檢測汙染 (同軸光) Step4:" + "\n");
                        richTextBox_log.AppendText("瑕疵尺寸篩選" + "\n\n");
                    }
                    break;
                case "Step5":
                    {
                        rbtn_Step5_Dirty_CL.Checked = true;
                        richTextBox_log.AppendText("※檢測汙染 (同軸光) Step5:" + "\n");
                        richTextBox_log.AppendText("瑕疵尺寸篩選" + "\n\n");
                    }
                    break;
            }

            richTextBox_log.ScrollToCaret();
        }

        private void rbtn_Step_Dirty_CL_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbtn = sender as RadioButton;
            if (rbtn.Checked == false) return;

            if (Input_ImgList.Count <= 0)
            {
                rbtn.Checked = false;
                MessageBox.Show("請先載入影像");
                return;
            }

            if (b_Initial_State)
            {
                if (Inspection_Dirty_CL()) b_Initial_State = false;
                else
                {
                    rbtn.Checked = false;
                    MessageBox.Show("檢測失敗", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            combxDisplayImg.SelectedIndex = int.Parse(nud_ImageID_Dirty_CL.Value.ToString());
            Extension.HObjectMedthods.ReleaseHObject(ref Current_disp_image); // (20190816) Jeff Revised!
            Current_disp_regions.Dispose();
            HOperatorSet.GenEmptyObj(out Current_disp_regions);
            string tag = rbtn.Tag.ToString();
            switch (tag)
            {
                case "Step1":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_Dirty_CL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_Rect_AllCells != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.CopyObj(Method.ho_Rect_AllCells, out Current_disp_regions, 1, -1);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "green");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Rect_BypassReg != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.CopyObj(Method.ho_Rect_BypassReg, out Current_disp_regions, 1, -1);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "blue");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Rect_BlackStripe != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.CopyObj(Method.ho_Rect_BlackStripe, out Current_disp_regions, 1, -1);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step2":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_Dirty_CL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_Reg_Cell_Dirty_CL != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Method.ho_Reg_Cell_Dirty_CL, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Reg_Resist_Dirty_CL != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "pink");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Method.ho_Reg_Resist_Dirty_CL, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Rect_AllCells != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "green");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Method.ho_Rect_AllCells, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Rect_BypassReg != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "blue");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Method.ho_Rect_BypassReg, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Rect_BlackStripe != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Method.ho_Rect_BlackStripe, DisplayWindows.HalconWindow);
                        }

                        // 設定使用者點擊region
                        if (Method.ho_Reg_Dirty_CL != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.CopyObj(Method.ho_Reg_Dirty_CL, out Current_disp_regions, 1, -1);
                        }
                    }
                    break;
                case "Step3":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_Dirty_CL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_Reg_Dirty_CL_select != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.CopyObj(Method.ho_Reg_Dirty_CL_select, out Current_disp_regions, 1, -1);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step4":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_Dirty_CL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_DefectReg_op1 != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.CopyObj(Method.ho_DefectReg_op1, out Current_disp_regions, 1, -1);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step5":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_Dirty_CL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_DefectReg_Dirty != null)
                        {
                            Current_disp_regions.Dispose();
                            //HOperatorSet.CopyObj(Method.ho_DefectReg_Dirty, out Current_disp_regions, 1, -1);
                            HOperatorSet.Connection(Method.ho_DefectReg_Dirty, out Current_disp_regions); // (20190331) Jeff Revised!
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
            }
        }

        private bool Inspection_Dirty_CL()
        {
            if (!(Method.Read_AllImages_Insp(Recipe, Input_ImgList))) return false;
            if (!(Method.InitialPosition_Insp(Recipe))) return false;
            if (cbx_CellSeg_CL.Checked)
            {
                if (!(Method.CellSeg_CL_Insp(Recipe))) return false;
            }
            else
            {
                if (!(Method.CellSeg_BL_Insp(Recipe))) return false;
            }
            if (!(Method.Df_CellReg(Recipe))) return false; // (20190810) Jeff Revised!
            if (!(Method.Dirty_CL_Insp(Recipe))) return false;
            return true;
        }

        private void param_Dirty_CL_ValueChanged(object sender, EventArgs e)
        {
            Control c = sender as Control;
            string tag = c.Tag.ToString();
            // Visible狀態切換
            if (tag == "str_ModeBlackStripe")
            {
                string str_ModeBlackStripe = c.Text;
                if (str_ModeBlackStripe == "mode1")
                {
                    nud_H_BlackStripe_Dirty_CL.Visible = true;
                    nud_H_BlackStripe_mode2_Dirty_CL.Visible = false;
                    lab_unit_Dirty_CL.Text = "(μm)";
                }
                else if (str_ModeBlackStripe == "mode2")
                {
                    nud_H_BlackStripe_Dirty_CL.Visible = false;
                    nud_H_BlackStripe_mode2_Dirty_CL.Visible = true;
                    lab_unit_Dirty_CL.Text = "(pixel)";
                }
                tag = "Step1"; // (20190412) Jeff Revised!
            }

            if (b_LoadRecipe) return;

            // 相同啟用項目同步更新
            switch (tag)
            {
                case "Enabled_Cell_Step1":
                    {
                        b_LoadRecipe = true;
                        cbx_Enabled_Cell_Step2_Dirty_CL.Checked = cbx_Enabled_Cell_Step1_Dirty_CL.Checked;
                        b_LoadRecipe = false;
                        tag = "Step1"; // (20190412) Jeff Revised!
                    }
                    break;
                case "Enabled_Cell_Step2":
                    {
                        b_LoadRecipe = true;
                        cbx_Enabled_Cell_Step1_Dirty_CL.Checked = cbx_Enabled_Cell_Step2_Dirty_CL.Checked;
                        b_LoadRecipe = false;
                        tag = "Step2"; // (20190412) Jeff Revised!
                    }
                    break;
                case "Enabled_BlackStripe_Step1":
                    {
                        b_LoadRecipe = true;
                        cbx_Enabled_BlackStripe_Step2_Dirty_CL.Checked = cbx_Enabled_BlackStripe_Step1_Dirty_CL.Checked;
                        b_LoadRecipe = false;
                        tag = "Step1"; // (20190412) Jeff Revised!
                    }
                    break;
                case "Enabled_BlackStripe_Step2":
                    {
                        b_LoadRecipe = true;
                        cbx_Enabled_BlackStripe_Step1_Dirty_CL.Checked = cbx_Enabled_BlackStripe_Step2_Dirty_CL.Checked;
                        b_LoadRecipe = false;
                        tag = "Step2"; // (20190412) Jeff Revised!
                    }
                    break;
            }

            if (Input_ImgList.Count <= 0)
            {
                MessageBox.Show("請先載入影像");
                return;
            }

            ParamGetFromUI();
            if (!Inspection_Dirty_CL())
            {
                clear_rbtn_Step_Dirty_CL();
                MessageBox.Show("檢測失敗", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            b_Initial_State = false;

            switch (tag)
            {
                case "ImageID":
                    {

                    }
                    break;
                case "Step1":
                    {
                        if (rbtn_Step1_Dirty_CL.Checked) rbtn_Step_Dirty_CL_CheckedChanged(rbtn_Step1_Dirty_CL, e);
                        else rbtn_Step1_Dirty_CL.Checked = true;
                    }
                    break;
                case "Step2":
                    {
                        if (rbtn_Step2_Dirty_CL.Checked) rbtn_Step_Dirty_CL_CheckedChanged(rbtn_Step2_Dirty_CL, e);
                        else rbtn_Step2_Dirty_CL.Checked = true;
                    }
                    break;
                case "Step3":
                    {
                        if (rbtn_Step3_Dirty_CL.Checked) rbtn_Step_Dirty_CL_CheckedChanged(rbtn_Step3_Dirty_CL, e);
                        else rbtn_Step3_Dirty_CL.Checked = true;
                    }
                    break;
                case "Step4":
                    {
                        if (rbtn_Step4_Dirty_CL.Checked) rbtn_Step_Dirty_CL_CheckedChanged(rbtn_Step4_Dirty_CL, e);
                        else rbtn_Step4_Dirty_CL.Checked = true;
                    }
                    break;
                case "Step5":
                    {
                        if (rbtn_Step5_Dirty_CL.Checked) rbtn_Step_Dirty_CL_CheckedChanged(rbtn_Step5_Dirty_CL, e);
                        else rbtn_Step5_Dirty_CL.Checked = true;
                    }
                    break;
            }
        }

        /// <summary>
        /// 清除 檢測汙染 (同軸光) 內所有 rbtn_Step
        /// </summary>
        private void clear_rbtn_Step_Dirty_CL()
        {
            rbtn_Step1_Dirty_CL.Checked = false;
            rbtn_Step2_Dirty_CL.Checked = false;
            rbtn_Step3_Dirty_CL.Checked = false;
            rbtn_Step4_Dirty_CL.Checked = false;
            rbtn_Step5_Dirty_CL.Checked = false;
        }
        #endregion

        private void btn_instruction_FLCL_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string tag = btn.Tag.ToString();
            switch (tag)
            {
                case "Step1":
                    {
                        richTextBox_log.AppendText("※檢測汙染 (正光&同軸光) Step1:" + "\n");
                        richTextBox_log.AppendText("檢測汙染 (正光下黑條區域內的黑點)" + "\n\n");
                    }
                    break;
                case "Step2":
                    {
                        richTextBox_log.AppendText("※檢測汙染 (正光&同軸光) Step2:" + "\n");
                        richTextBox_log.AppendText("瑕疵Region內(正光)至少有一個pixel之灰階值低於一個數值 (瑕疵Region內最低灰階標準)" + "\n\n");
                    }
                    break;
                case "Step3":
                    {
                        richTextBox_log.AppendText("※檢測汙染 (正光&同軸光) Step3:" + "\n");
                        richTextBox_log.AppendText("檢測汙染 (同軸光下黑條區域內的白點)" + "\n\n");
                    }
                    break;
                case "Step4":
                    {
                        richTextBox_log.AppendText("※檢測汙染 (正光&同軸光) Step4:" + "\n");
                        richTextBox_log.AppendText("瑕疵Region內(同軸光)至少有一個pixel之灰階值高於一個數值 (瑕疵Region內最高灰階標準)" + "\n\n");
                    }
                    break;
                case "Step5":
                    {
                        richTextBox_log.AppendText("※檢測汙染 (正光&同軸光) Step5:" + "\n");
                        richTextBox_log.AppendText("結合正光&同軸光之汙染瑕疵" + "\n\n");
                    }
                    break;
                case "Step6":
                    {
                        richTextBox_log.AppendText("※檢測汙染 (正光&同軸光) Step6:" + "\n");
                        richTextBox_log.AppendText("瑕疵尺寸篩選" + "\n\n");
                    }
                    break;
            }

            richTextBox_log.ScrollToCaret();
        }
        
        #region 檢測電極區白點 (背光)
        private void btn_instruction_BL_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string tag = btn.Tag.ToString();
            switch (tag)
            {
                case "Step1":
                    {
                        rbtn_Step1_BrightBlob_BL.Checked = true;
                        richTextBox_log.AppendText("※檢測電極區白點 (背光) Step1:" + "\n");
                        richTextBox_log.AppendText("檢測電極區白點 (灰條內白點)" + "\n\n");
                    }
                    break;
                case "Step2":
                    {
                        rbtn_Step2_BrightBlob_BL.Checked = true;
                        richTextBox_log.AppendText("※檢測電極區白點 (背光) Step2:" + "\n");
                        richTextBox_log.AppendText("瑕疵尺寸篩選" + "\n\n");
                    }
                    break;
            }

            richTextBox_log.ScrollToCaret();
        }

        private void rbtn_Step_BrightBlob_BL_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbtn = sender as RadioButton;
            if (rbtn.Checked == false) return;

            if (Input_ImgList.Count <= 0)
            {
                rbtn.Checked = false;
                MessageBox.Show("請先載入影像");
                return;
            }

            if (b_Initial_State)
            {
                if (Inspection_BrightBlob_BL()) b_Initial_State = false;
                else
                {
                    rbtn.Checked = false;
                    MessageBox.Show("檢測失敗", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            combxDisplayImg.SelectedIndex = int.Parse(nud_ImageID_BrightBlob_BL.Value.ToString());
            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
            Extension.HObjectMedthods.ReleaseHObject(ref Current_disp_image); // (20190816) Jeff Revised!
            Current_disp_regions.Dispose();
            HOperatorSet.GenEmptyObj(out Current_disp_regions);
            string tag = rbtn.Tag.ToString();
            switch (tag)
            {
                case "Step1":
                    {
                        Current_disp_image = Method.ho_ImageReduced_WhiteStripe_BL.Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_Region_Blob_Elect_BL != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.CopyObj(Method.ho_Region_Blob_Elect_BL, out Current_disp_regions, 1, -1);
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step2":
                    {
                        Current_disp_image = Method.ho_ImageReduced_WhiteStripe_BL.Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_DefectReg_BrightBlob != null)
                        {
                            Current_disp_regions.Dispose();
                            //HOperatorSet.CopyObj(Method.ho_DefectReg_BrightBlob, out Current_disp_regions, 1, -1);
                            HOperatorSet.Connection(Method.ho_DefectReg_BrightBlob, out Current_disp_regions); // (20190331) Jeff Revised!
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
            }
        }

        private bool Inspection_BrightBlob_BL()
        {
            if (!(Method.Read_AllImages_Insp(Recipe, Input_ImgList))) return false;
            if (!(Method.InitialPosition_Insp(Recipe))) return false;
            if (cbx_CellSeg_CL.Checked)
            {
                if (!(Method.CellSeg_CL_Insp(Recipe))) return false;
            }
            else
            {
                if (!(Method.CellSeg_BL_Insp(Recipe))) return false;
            }
            if (!(Method.Df_CellReg(Recipe))) return false; // (20190810) Jeff Revised!
            if (!(Method.BrightBlob_BL_Insp(Recipe))) return false;
            return true;
        }

        private void param_BrightBlob_BL_ValueChanged(object sender, EventArgs e)
        {
            if (b_LoadRecipe) return;

            if (Input_ImgList.Count <= 0)
            {
                MessageBox.Show("請先載入影像");
                return;
            }

            NumericUpDown nud = sender as NumericUpDown;

            // 非數字，則直接return
            int value;
            if (!(int.TryParse(nud.Value.ToString(), out value)))
                return;

            bool Res = false;
            string tag = nud.Tag.ToString();
            switch (tag)
            {
                case "ImageID":
                    {
                        Recipe.Param.InspParam_BrightBlob_BL.ImageIndex = int.Parse(nud_ImageID_BrightBlob_BL.Value.ToString());
                        if (Inspection_BrightBlob_BL()) Res = true;
                    }
                    break;
                case "MinGray_Blob_Elect_BL":
                    {
                        Recipe.Param.InspParam_BrightBlob_BL.MinGray_Blob_Elect_BL = int.Parse(nud_MinGray_Blob_Elect_BL.Value.ToString());
                        if (Inspection_BrightBlob_BL())
                        {
                            Res = true;
                            if (rbtn_Step1_BrightBlob_BL.Checked) rbtn_Step_BrightBlob_BL_CheckedChanged(rbtn_Step1_BrightBlob_BL, e);
                            else rbtn_Step1_BrightBlob_BL.Checked = true;
                        }
                    }
                    break;
                case "MinH_df_BrightBlob_BL":
                    {
                        Recipe.Param.InspParam_BrightBlob_BL.MinHeight_defect = int.Parse(nud_MinH_df_BrightBlob_BL.Value.ToString());
                        if (Inspection_BrightBlob_BL())
                        {
                            Res = true;
                            if (rbtn_Step2_BrightBlob_BL.Checked) rbtn_Step_BrightBlob_BL_CheckedChanged(rbtn_Step2_BrightBlob_BL, e);
                            else rbtn_Step2_BrightBlob_BL.Checked = true;
                        }
                    }
                    break;
                case "MaxH_df_BrightBlob_BL":
                    {
                        Recipe.Param.InspParam_BrightBlob_BL.MaxHeight_defect = int.Parse(nud_MaxH_df_BrightBlob_BL.Value.ToString());
                        if (Inspection_BrightBlob_BL())
                        {
                            Res = true;
                            if (rbtn_Step2_BrightBlob_BL.Checked) rbtn_Step_BrightBlob_BL_CheckedChanged(rbtn_Step2_BrightBlob_BL, e);
                            else rbtn_Step2_BrightBlob_BL.Checked = true;
                        }
                    }
                    break;
                case "MinW_df_BrightBlob_BL":
                    {
                        Recipe.Param.InspParam_BrightBlob_BL.MinWidth_defect = int.Parse(nud_MinW_df_BrightBlob_BL.Value.ToString());
                        if (Inspection_BrightBlob_BL())
                        {
                            Res = true;
                            if (rbtn_Step2_BrightBlob_BL.Checked) rbtn_Step_BrightBlob_BL_CheckedChanged(rbtn_Step2_BrightBlob_BL, e);
                            else rbtn_Step2_BrightBlob_BL.Checked = true;
                        }
                    }
                    break;
                case "MaxW_df_BrightBlob_BL":
                    {
                        Recipe.Param.InspParam_BrightBlob_BL.MaxWidth_defect = int.Parse(nud_MaxW_df_BrightBlob_BL.Value.ToString());
                        if (Inspection_BrightBlob_BL())
                        {
                            Res = true;
                            if (rbtn_Step2_BrightBlob_BL.Checked) rbtn_Step_BrightBlob_BL_CheckedChanged(rbtn_Step2_BrightBlob_BL, e);
                            else rbtn_Step2_BrightBlob_BL.Checked = true;
                        }
                    }
                    break;
            }

            if (Res) b_Initial_State = false;
            else
            {
                clear_rbtn_Step_BrightBlob_BL();
                MessageBox.Show("檢測失敗", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 清除 檢測電極區白點 (背光) 內所有 rbtn_Step
        /// </summary>
        private void clear_rbtn_Step_BrightBlob_BL() // (20190216) Jeff Revised!
        {
            rbtn_Step1_BrightBlob_BL.Checked = false;
            rbtn_Step2_BrightBlob_BL.Checked = false;
        }
        #endregion

        #region 檢測保缺角 (背光)
        private void btn_instruction_LackAngle_BL_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string tag = btn.Tag.ToString();
            switch (tag)
            {
                case "Step1":
                    {
                        rbtn_Step1_LackAngle_BL.Checked = true;
                        richTextBox_log.AppendText("※檢測保缺角 (背光) Step1:" + "\n");
                        richTextBox_log.AppendText("設定檢測ROI (針對 各顆Cell, Cell內忽略區域 & 黑色長條電阻區)" + "\n\n");
                    }
                    break;
                case "Step2":
                    {
                        rbtn_Step2_LackAngle_BL.Checked = true;
                        richTextBox_log.AppendText("※檢測保缺角 (背光) Step2:" + "\n");
                        richTextBox_log.AppendText("分區做直方圖等化: 消除亮區，以避免影響直方圖等化效果" + "\n\n");
                    }
                    break;
                case "Step3":
                    {
                        rbtn_Step3_LackAngle_BL.Checked = true;
                        richTextBox_log.AppendText("※檢測保缺角 (背光) Step2:" + "\n");
                        richTextBox_log.AppendText("分區做直方圖等化: 設定分區尺寸的寬和高" + "\n\n");
                    }
                    break;
                case "Step4":
                    {
                        rbtn_Step4_LackAngle_BL.Checked = true;
                        richTextBox_log.AppendText("※檢測保缺角 (背光) Step4:" + "\n");
                        richTextBox_log.AppendText("二值化處理 (針對 各顆Cell不含忽略區域 & 黑色長條電阻區不含各顆Cell,及Cell內忽略區域)，並將啟用區域取聯集" + "\n");
                        richTextBox_log.AppendText("Note: " + "\n\n");
                    }
                    break;
                case "Step5":
                    {
                        rbtn_Step5_LackAngle_BL.Checked = true;
                        richTextBox_log.AppendText("※檢測保缺角 (背光) Step5:" + "\n");
                        richTextBox_log.AppendText("瑕疵Region內(背光)至少有一個pixel之灰階值高於一個數值 (瑕疵Region內最高灰階標準)" + "\n\n");
                    }
                    break;
                case "Step6":
                    {
                        rbtn_Step6_LackAngle_BL.Checked = true;
                        richTextBox_log.AppendText("※檢測保缺角 (背光) Step6:" + "\n");
                        richTextBox_log.AppendText("瑕疵尺寸篩選" + "\n\n");
                    }
                    break;
                case "Step7":
                    {
                        rbtn_Step7_LackAngle_BL.Checked = true;
                        richTextBox_log.AppendText("※檢測保缺角 (背光) Step7:" + "\n");
                        richTextBox_log.AppendText("瑕疵尺寸篩選" + "\n\n");
                    }
                    break;
            }

            richTextBox_log.ScrollToCaret();
        }

        private void rbtn_Step_LackAngle_BL_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbtn = sender as RadioButton;
            if (rbtn.Checked == false) return;

            if (Input_ImgList.Count <= 0)
            {
                rbtn.Checked = false;
                MessageBox.Show("請先載入影像");
                return;
            }

            if (b_Initial_State)
            {
                if (Inspection_LackAngle_BL()) b_Initial_State = false;
                else
                {
                    rbtn.Checked = false;
                    MessageBox.Show("檢測失敗", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            combxDisplayImg.SelectedIndex = int.Parse(nud_ImageID_LackAngle_BL.Value.ToString());
            Extension.HObjectMedthods.ReleaseHObject(ref Current_disp_image); // (20190816) Jeff Revised!
            Current_disp_regions.Dispose();
            HOperatorSet.GenEmptyObj(out Current_disp_regions);
            string tag = rbtn.Tag.ToString();
            switch (tag)
            {
                case "Step1":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_LackAngle_BL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_Rect_AllCells != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.CopyObj(Method.ho_Rect_AllCells, out Current_disp_regions, 1, -1);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "green");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Rect_BypassReg != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.CopyObj(Method.ho_Rect_BypassReg, out Current_disp_regions, 1, -1);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "blue");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Rect_BlackStripe != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.CopyObj(Method.ho_Rect_BlackStripe, out Current_disp_regions, 1, -1);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step2":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_LackAngle_BL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_Regions_Bright_BL != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.CopyObj(Method.ho_Regions_Bright_BL, out Current_disp_regions, 1, -1);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step3":
                    {
                        Current_disp_image = Method.ho_Image_select.Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_Partitioned != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.CopyObj(Method.ho_Partitioned, out Current_disp_regions, 1, -1);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "blue");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step4":
                    {
                        Current_disp_image = Method.ho_Image_select.Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_Reg_Cell_LackAngle_BL != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Method.ho_Reg_Cell_LackAngle_BL, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Reg_Resist_LackAngle_BL != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "pink");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Method.ho_Reg_Resist_LackAngle_BL, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Rect_AllCells != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "green");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Method.ho_Rect_AllCells, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Rect_BypassReg != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "blue");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Method.ho_Rect_BypassReg, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Rect_BlackStripe != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Method.ho_Rect_BlackStripe, DisplayWindows.HalconWindow);
                        }

                        // 設定使用者點擊region
                        if (Method.ho_Reg_LackAngle_BL != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.CopyObj(Method.ho_Reg_LackAngle_BL, out Current_disp_regions, 1, -1);
                        }
                    }
                    break;
                case "Step5":
                    {
                        Current_disp_image = Method.ho_Image_select.Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_Reg_LackAngle_BL_select != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.CopyObj(Method.ho_Reg_LackAngle_BL_select, out Current_disp_regions, 1, -1);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step6":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_LackAngle_BL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_DefectReg_op1 != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.CopyObj(Method.ho_DefectReg_op1, out Current_disp_regions, 1, -1);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step7":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_LackAngle_BL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_DefectReg_LackAngle_BL != null)
                        {
                            Current_disp_regions.Dispose();
                            //HOperatorSet.CopyObj(Method.ho_DefectReg_LackAngle_BL, out Current_disp_regions, 1, -1);
                            HOperatorSet.Connection(Method.ho_DefectReg_LackAngle_BL, out Current_disp_regions); // (20190331) Jeff Revised!
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
            }
        }

        private bool Inspection_LackAngle_BL()
        {
            if (!(Method.Read_AllImages_Insp(Recipe, Input_ImgList))) return false;
            if (!(Method.InitialPosition_Insp(Recipe))) return false;
            if (cbx_CellSeg_CL.Checked)
            {
                if (!(Method.CellSeg_CL_Insp(Recipe))) return false;
            }
            else
            {
                if (!(Method.CellSeg_BL_Insp(Recipe))) return false;
            }
            if (!(Method.Df_CellReg(Recipe))) return false; // (20190810) Jeff Revised!
            if (!(Method.LackAngle_BL_Insp(Recipe))) return false;
            return true;
        }

        private void param_LackAngle_BL_ValueChanged(object sender, EventArgs e)
        {
            if (b_LoadRecipe) return;

            // 相同啟用項目同步更新
            Control c = sender as Control;
            string tag = c.Tag.ToString();
            switch (tag)
            {
                case "Enabled_Cell_Step1":
                    {
                        b_LoadRecipe = true;
                        cbx_Enabled_Cell_Step4_LackAngle_BL.Checked = cbx_Enabled_Cell_Step1_LackAngle_BL.Checked;
                        b_LoadRecipe = false;
                        tag = "Step1"; // (20190412) Jeff Revised!
                    }
                    break;
                case "Enabled_Cell_Step4":
                    {
                        b_LoadRecipe = true;
                        cbx_Enabled_Cell_Step1_LackAngle_BL.Checked = cbx_Enabled_Cell_Step4_LackAngle_BL.Checked;
                        b_LoadRecipe = false;
                        tag = "Step4"; // (20190412) Jeff Revised!
                    }
                    break;
                case "Enabled_BlackStripe_Step1":
                    {
                        b_LoadRecipe = true;
                        cbx_Enabled_BlackStripe_Step4_LackAngle_BL.Checked = cbx_Enabled_BlackStripe_Step1_LackAngle_BL.Checked;
                        b_LoadRecipe = false;
                        tag = "Step1"; // (20190412) Jeff Revised!
                    }
                    break;
                case "Enabled_BlackStripe_Step4":
                    {
                        b_LoadRecipe = true;
                        cbx_Enabled_BlackStripe_Step1_LackAngle_BL.Checked = cbx_Enabled_BlackStripe_Step4_LackAngle_BL.Checked;
                        b_LoadRecipe = false;
                        tag = "Step4"; // (20190412) Jeff Revised!
                    }
                    break;
                case "Enabled_EquHisto_Step2":
                    {
                        b_LoadRecipe = true;
                        cbx_Enabled_EquHisto_Step3_LackAngle_BL.Checked = cbx_Enabled_EquHisto_Step2_LackAngle_BL.Checked;
                        b_LoadRecipe = false;
                        tag = "Step2"; // (20190412) Jeff Revised!
                    }
                    break;
                case "Enabled_EquHisto_Step3":
                    {
                        b_LoadRecipe = true;
                        cbx_Enabled_EquHisto_Step2_LackAngle_BL.Checked = cbx_Enabled_EquHisto_Step3_LackAngle_BL.Checked;
                        b_LoadRecipe = false;
                        tag = "Step3"; // (20190412) Jeff Revised!
                    }
                    break;
            }

            if (Input_ImgList.Count <= 0)
            {
                MessageBox.Show("請先載入影像");
                return;
            }

            ParamGetFromUI();
            if (!Inspection_LackAngle_BL())
            {
                clear_rbtn_Step_LackAngle_BL();
                MessageBox.Show("檢測失敗", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            b_Initial_State = false;

            switch (tag)
            {
                case "ImageID":
                    {

                    }
                    break;
                case "Step1":
                    {
                        if (rbtn_Step1_LackAngle_BL.Checked) rbtn_Step_LackAngle_BL_CheckedChanged(rbtn_Step1_LackAngle_BL, e);
                        else rbtn_Step1_LackAngle_BL.Checked = true;
                    }
                    break;
                case "Step2":
                    {
                        if (rbtn_Step2_LackAngle_BL.Checked) rbtn_Step_LackAngle_BL_CheckedChanged(rbtn_Step2_LackAngle_BL, e);
                        else rbtn_Step2_LackAngle_BL.Checked = true;
                    }
                    break;
                case "Step3":
                    {
                        if (rbtn_Step3_LackAngle_BL.Checked) rbtn_Step_LackAngle_BL_CheckedChanged(rbtn_Step3_LackAngle_BL, e);
                        else rbtn_Step3_LackAngle_BL.Checked = true;
                    }
                    break;
                case "Step4":
                    {
                        if (rbtn_Step4_LackAngle_BL.Checked) rbtn_Step_LackAngle_BL_CheckedChanged(rbtn_Step4_LackAngle_BL, e);
                        else rbtn_Step4_LackAngle_BL.Checked = true;
                    }
                    break;
                case "Step5":
                    {
                        if (rbtn_Step5_LackAngle_BL.Checked) rbtn_Step_LackAngle_BL_CheckedChanged(rbtn_Step5_LackAngle_BL, e);
                        else rbtn_Step5_LackAngle_BL.Checked = true;
                    }
                    break;
                case "Step6":
                    {
                        if (rbtn_Step6_LackAngle_BL.Checked) rbtn_Step_LackAngle_BL_CheckedChanged(rbtn_Step6_LackAngle_BL, e);
                        else rbtn_Step6_LackAngle_BL.Checked = true;
                    }
                    break;
                case "Step7":
                    {
                        if (rbtn_Step7_LackAngle_BL.Checked) rbtn_Step_LackAngle_BL_CheckedChanged(rbtn_Step7_LackAngle_BL, e);
                        else rbtn_Step7_LackAngle_BL.Checked = true;
                    }
                    break;
            }
        }

        /// <summary>
        /// 清除 檢測保缺角 (背光) 內所有 rbtn_Step
        /// </summary>
        private void clear_rbtn_Step_LackAngle_BL()
        {
            rbtn_Step1_LackAngle_BL.Checked = false;
            rbtn_Step2_LackAngle_BL.Checked = false;
            rbtn_Step3_LackAngle_BL.Checked = false;
            rbtn_Step4_LackAngle_BL.Checked = false;
            rbtn_Step5_LackAngle_BL.Checked = false;
            rbtn_Step6_LackAngle_BL.Checked = false;
            rbtn_Step7_LackAngle_BL.Checked = false;
        }
        #endregion

        #region 檢測保缺角 (側光)
        private void btn_instruction_LackAngle_SL_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string tag = btn.Tag.ToString();
            switch (tag)
            {
                case "Step1":
                    {
                        rbtn_Step1_LackAngle_SL.Checked = true;
                        richTextBox_log.AppendText("※檢測保缺角 (側光) Step1:" + "\n");
                        richTextBox_log.AppendText("設定檢測ROI (針對 各顆Cell, Cell內忽略區域 & 黑色長條電阻區)" + "\n\n");
                    }
                    break;
                case "Step2":
                    {
                        rbtn_Step2_LackAngle_SL.Checked = true;
                        richTextBox_log.AppendText("※檢測保缺角 (側光) Step2:" + "\n");
                        richTextBox_log.AppendText("二值化處理 (針對 各顆Cell不含忽略區域 & 黑色長條電阻區不含各顆Cell,及Cell內忽略區域)" + "\n");
                        richTextBox_log.AppendText("Note: " + "\n\n");
                    }
                    break;
                case "Step3":
                    {
                        rbtn_Step3_LackAngle_SL.Checked = true;
                        richTextBox_log.AppendText("※檢測保缺角 (側光) Step3:" + "\n");
                        richTextBox_log.AppendText("瑕疵Region內(同軸光)至少有一個pixel之灰階值高於一個數值 (瑕疵Region內最高灰階標準)" + "\n\n");
                    }
                    break;
                case "Step4":
                    {
                        rbtn_Step4_LackAngle_SL.Checked = true;
                        richTextBox_log.AppendText("※檢測保缺角 (側光) Step4:" + "\n");
                        richTextBox_log.AppendText("設定檢測ROI 2 (針對 各顆Cell)" + "\n\n");
                    }
                    break;
                case "Step5":
                    {
                        rbtn_Step5_LackAngle_SL.Checked = true;
                        richTextBox_log.AppendText("※檢測保缺角 (側光) Step5:" + "\n");
                        richTextBox_log.AppendText("瑕疵尺寸篩選" + "\n\n");
                    }
                    break;
                case "Step6":
                    {
                        rbtn_Step6_LackAngle_SL.Checked = true;
                        richTextBox_log.AppendText("※檢測保缺角 (側光) Step6:" + "\n");
                        richTextBox_log.AppendText("瑕疵尺寸篩選" + "\n\n");
                    }
                    break;
            }

            richTextBox_log.ScrollToCaret();
        }

        private void rbtn_Step_LackAngle_SL_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbtn = sender as RadioButton;
            if (rbtn.Checked == false) return;

            if (Input_ImgList.Count <= 0)
            {
                rbtn.Checked = false;
                MessageBox.Show("請先載入影像");
                return;
            }

            if (b_Initial_State)
            {
                if (Inspection_LackAngle_SL()) b_Initial_State = false;
                else
                {
                    rbtn.Checked = false;
                    MessageBox.Show("檢測失敗", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            combxDisplayImg.SelectedIndex = int.Parse(nud_ImageID_LackAngle_SL.Value.ToString());
            Extension.HObjectMedthods.ReleaseHObject(ref Current_disp_image); // (20190816) Jeff Revised!
            Current_disp_regions.Dispose();
            HOperatorSet.GenEmptyObj(out Current_disp_regions);
            string tag = rbtn.Tag.ToString();
            switch (tag)
            {
                case "Step1":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_LackAngle_SL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_Rect_AllCells != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.CopyObj(Method.ho_Rect_AllCells, out Current_disp_regions, 1, -1);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "green");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Rect_BypassReg != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.CopyObj(Method.ho_Rect_BypassReg, out Current_disp_regions, 1, -1);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "blue");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Rect_BlackStripe != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.CopyObj(Method.ho_Rect_BlackStripe, out Current_disp_regions, 1, -1);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step2":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_LackAngle_SL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_Reg_Cell_Thresh != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Method.ho_Reg_Cell_Thresh, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Reg_Resist_Thresh != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "pink");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Method.ho_Reg_Resist_Thresh, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Rect_AllCells != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "green");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Method.ho_Rect_AllCells, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Rect_BypassReg != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "blue");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Method.ho_Rect_BypassReg, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Rect_BlackStripe != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Method.ho_Rect_BlackStripe, DisplayWindows.HalconWindow);
                        }

                        // 設定使用者點擊region
                        if (Method.ho_Reg_Cell_Thresh != null || Method.ho_Reg_Resist_Thresh != null)
                        {
                            HObject ho_Reg_Cell_Thresh, ho_Reg_Resist_Thresh;
                            HOperatorSet.Connection(Method.ho_Reg_Cell_Thresh, out ho_Reg_Cell_Thresh);
                            HOperatorSet.Connection(Method.ho_Reg_Resist_Thresh, out ho_Reg_Resist_Thresh);
                            Current_disp_regions.Dispose();
                            HOperatorSet.ConcatObj(ho_Reg_Cell_Thresh, ho_Reg_Resist_Thresh, out Current_disp_regions);
                            ho_Reg_Cell_Thresh.Dispose();
                            ho_Reg_Resist_Thresh.Dispose();
                        }
                    }
                    break;
                case "Step3":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_LackAngle_SL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_Reg_select != null)
                        {
                            HObject ho_Reg_select = null;
                            HOperatorSet.Connection(Method.ho_Reg_select, out ho_Reg_select);
                            Current_disp_regions.Dispose();
                            HOperatorSet.CopyObj(ho_Reg_select, out Current_disp_regions, 1, -1);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                            ho_Reg_select.Dispose();
                        }
                    }
                    break;
                case "Step4":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_LackAngle_SL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_Rect_AllCells2 != null)
                        {
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "green");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                            HOperatorSet.DispObj(Method.ho_Rect_AllCells2, DisplayWindows.HalconWindow);
                        }
                        if (Method.ho_Reg_select_CellReg != null)
                        {
                            Current_disp_regions.Dispose();
                            HOperatorSet.CopyObj(Method.ho_Reg_select_CellReg, out Current_disp_regions, 1, -1);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step5":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_LackAngle_SL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_DefectReg_op1 != null)
                        {
                            Current_disp_regions.Dispose();
                            //HOperatorSet.CopyObj(Method.ho_DefectReg_op1, out Current_disp_regions, 1, -1);
                            HObject DefectReg_op1_union = null;
                            HOperatorSet.Union1(Method.ho_DefectReg_op1, out DefectReg_op1_union);
                            HOperatorSet.Connection(DefectReg_op1_union, out Current_disp_regions);
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
                case "Step6":
                    {
                        Current_disp_image = Method.Insp_ImgList[int.Parse(nud_ImageID_LackAngle_SL.Value.ToString())].Clone();
                        ClearDisplay(Current_disp_image);
                        if (Method.ho_DefectReg_LackAngle_SL != null)
                        {
                            Current_disp_regions.Dispose();
                            //HOperatorSet.CopyObj(Method.ho_DefectReg_LackAngle_SL, out Current_disp_regions, 1, -1);
                            HOperatorSet.Connection(Method.ho_DefectReg_LackAngle_SL, out Current_disp_regions); // (20190331) Jeff Revised!
                            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                            HOperatorSet.DispObj(Current_disp_regions, DisplayWindows.HalconWindow);
                        }
                    }
                    break;
            }
        }

        private bool Inspection_LackAngle_SL()
        {
            if (!(Method.Read_AllImages_Insp(Recipe, Input_ImgList))) return false;
            if (!(Method.InitialPosition_Insp(Recipe))) return false;
            if (cbx_CellSeg_CL.Checked)
            {
                if (!(Method.CellSeg_CL_Insp(Recipe))) return false;
            }
            else
            {
                if (!(Method.CellSeg_BL_Insp(Recipe))) return false;
            }
            if (!(Method.Df_CellReg(Recipe))) return false; // (20190810) Jeff Revised!
            if (!(Method.LackAngle_SL_Insp(Recipe))) return false;
            return true;
        }

        private void param_LackAngle_SL_ValueChanged(object sender, EventArgs e)
        {
            Control c = sender as Control;
            string tag = c.Tag.ToString();
            // Visible狀態切換
            if (tag == "str_ModeBlackStripe")
            {
                string str_ModeBlackStripe = c.Text;
                if (str_ModeBlackStripe == "mode1")
                {
                    nud_H_BlackStripe_LackAngle_SL.Visible = true;
                    nud_H_BlackStripe_mode2_LackAngle_SL.Visible = false;
                    lab_unit_LackAngle_SL.Text = "(μm)";
                }
                else if (str_ModeBlackStripe == "mode2")
                {
                    nud_H_BlackStripe_LackAngle_SL.Visible = false;
                    nud_H_BlackStripe_mode2_LackAngle_SL.Visible = true;
                    lab_unit_LackAngle_SL.Text = "(pixel)";
                }
                tag = "Step1";
            }
            else if (tag == "str_BinaryImg_Cell") // (20190410) Jeff Revised!
            {
                if (combx_num_BinImg_Cell_LackAngle_SL.Text != "1")
                    combx_num_BinImg_Cell_LackAngle_SL.Text = "1";
                else
                    param_LackAngle_SL_ValueChanged(combx_num_BinImg_Cell_LackAngle_SL, e);
                tag = "Step2";
            }
            else if (tag == "num_BinImg_Cell") // (20190410) Jeff Revised!
            {
                string str_BinaryImg = combx_str_BinaryImg_Cell_LackAngle_SL.Text;
                string str_num = c.Text;
                if (str_BinaryImg == "固定閥值")
                {
                    gbx_DynThresh_1_Cell_LackAngle_SL.Visible = false;
                    gbx_DynThresh_2_Cell_LackAngle_SL.Visible = false;
                    if (str_num == "1")
                    {
                        gbx_ConstThresh_1_Cell_LackAngle_SL.Visible = true;
                        gbx_ConstThresh_2_Cell_LackAngle_SL.Visible = false;
                    }
                    else if (str_num == "2")
                    {
                        gbx_ConstThresh_1_Cell_LackAngle_SL.Visible = false;
                        gbx_ConstThresh_2_Cell_LackAngle_SL.Visible = true;
                    }
                }
                else if (str_BinaryImg == "動態閥值")
                {
                    gbx_ConstThresh_1_Cell_LackAngle_SL.Visible = false;
                    gbx_ConstThresh_2_Cell_LackAngle_SL.Visible = false;
                    if (str_num == "1")
                    {
                        gbx_DynThresh_1_Cell_LackAngle_SL.Visible = true;
                        gbx_DynThresh_2_Cell_LackAngle_SL.Visible = false;
                    }
                    else if (str_num == "2")
                    {
                        gbx_DynThresh_1_Cell_LackAngle_SL.Visible = false;
                        gbx_DynThresh_2_Cell_LackAngle_SL.Visible = true;
                    }
                }

                return;
            }
            else if (tag == "str_BinaryImg_Resist") // (20190410) Jeff Revised!
            {
                if (combx_num_BinImg_Resist_LackAngle_SL.Text != "1")
                    combx_num_BinImg_Resist_LackAngle_SL.Text = "1";
                else
                    param_LackAngle_SL_ValueChanged(combx_num_BinImg_Resist_LackAngle_SL, e);
                tag = "Step2";
            }
            else if (tag == "num_BinImg_Resist") // (20190410) Jeff Revised!
            {
                string str_BinaryImg = combx_str_BinaryImg_Resist_LackAngle_SL.Text;
                string str_num = c.Text;
                if (str_BinaryImg == "固定閥值")
                {
                    gbx_DynThresh_1_Resist_LackAngle_SL.Visible = false;
                    gbx_DynThresh_2_Resist_LackAngle_SL.Visible = false;
                    if (str_num == "1")
                    {
                        gbx_ConstThresh_1_Resist_LackAngle_SL.Visible = true;
                        gbx_ConstThresh_2_Resist_LackAngle_SL.Visible = false;
                    }
                    else if (str_num == "2")
                    {
                        gbx_ConstThresh_1_Resist_LackAngle_SL.Visible = false;
                        gbx_ConstThresh_2_Resist_LackAngle_SL.Visible = true;
                    }
                }
                else if (str_BinaryImg == "動態閥值")
                {
                    gbx_ConstThresh_1_Resist_LackAngle_SL.Visible = false;
                    gbx_ConstThresh_2_Resist_LackAngle_SL.Visible = false;
                    if (str_num == "1")
                    {
                        gbx_DynThresh_1_Resist_LackAngle_SL.Visible = true;
                        gbx_DynThresh_2_Resist_LackAngle_SL.Visible = false;
                    }
                    else if (str_num == "2")
                    {
                        gbx_DynThresh_1_Resist_LackAngle_SL.Visible = false;
                        gbx_DynThresh_2_Resist_LackAngle_SL.Visible = true;
                    }
                }

                return;
            }
            else if (tag == "num_op1")
            {
                string str_num_op1 = combx_num_op1_LackAngle_SL.Text;
                if (str_num_op1 == "1")
                {
                    gbx_op1_1_LackAngle_SL.Visible = true;
                    gbx_op1_2_LackAngle_SL.Visible = false;
                }
                else if (str_num_op1 == "2")
                {
                    gbx_op1_1_LackAngle_SL.Visible = false;
                    gbx_op1_2_LackAngle_SL.Visible = true;
                }

                return;
            }

            // Enabled狀態切換
            else if (tag == "count_BinImg_Cell") // (20190410) Jeff Revised!
            {
                if (nud_count_BinImg_Cell_LackAngle_SL.Value > 1)
                    combx_num_BinImg_Cell_LackAngle_SL.Enabled = true;
                else
                {
                    combx_num_BinImg_Cell_LackAngle_SL.Text = "1";
                    combx_num_BinImg_Cell_LackAngle_SL.Enabled = false;
                }
                tag = "Step2";
            }
            else if (tag == "count_BinImg_Resist") // (20190410) Jeff Revised!
            {
                if (nud_count_BinImg_Resist_LackAngle_SL.Value > 1)
                    combx_num_BinImg_Resist_LackAngle_SL.Enabled = true;
                else
                {
                    combx_num_BinImg_Resist_LackAngle_SL.Text = "1";
                    combx_num_BinImg_Resist_LackAngle_SL.Enabled = false;
                }
                tag = "Step2";
            }
            else if (tag == "count_op1")
            {
                if (nud_count_op1_LackAngle_SL.Value > 1)
                    combx_num_op1_LackAngle_SL.Enabled = true;
                else
                {
                    combx_num_op1_LackAngle_SL.Text = "1";
                    combx_num_op1_LackAngle_SL.Enabled = false;
                }
                tag = "Step5";
            }

            if (b_LoadRecipe) return;

            // 相同啟用項目同步更新
            switch (tag)
            {
                case "Enabled_Cell_Step1":
                    {
                        b_LoadRecipe = true;
                        cbx_Enabled_Cell_Step2_LackAngle_SL.Checked = cbx_Enabled_Cell_Step1_LackAngle_SL.Checked;
                        b_LoadRecipe = false;
                        tag = "Step1";
                    }
                    break;
                case "Enabled_Cell_Step2":
                    {
                        b_LoadRecipe = true;
                        cbx_Enabled_Cell_Step1_LackAngle_SL.Checked = cbx_Enabled_Cell_Step2_LackAngle_SL.Checked;
                        b_LoadRecipe = false;
                        tag = "Step2";
                    }
                    break;
                case "Enabled_BlackStripe_Step1":
                    {
                        b_LoadRecipe = true;
                        cbx_Enabled_BlackStripe_Step2_LackAngle_SL.Checked = cbx_Enabled_BlackStripe_Step1_LackAngle_SL.Checked;
                        b_LoadRecipe = false;
                        tag = "Step1";
                    }
                    break;
                case "Enabled_BlackStripe_Step2":
                    {
                        b_LoadRecipe = true;
                        cbx_Enabled_BlackStripe_Step1_LackAngle_SL.Checked = cbx_Enabled_BlackStripe_Step2_LackAngle_SL.Checked;
                        b_LoadRecipe = false;
                        tag = "Step2";
                    }
                    break;
            }

            if (Input_ImgList.Count <= 0)
            {
                MessageBox.Show("請先載入影像");
                return;
            }

            ParamGetFromUI();
            if (!Inspection_LackAngle_SL())
            {
                clear_rbtn_Step_LackAngle_SL();
                MessageBox.Show("檢測失敗", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            b_Initial_State = false;

            switch (tag)
            {
                case "ImageID":
                    {

                    }
                    break;
                case "Step1":
                    {
                        if (rbtn_Step1_LackAngle_SL.Checked) rbtn_Step_LackAngle_SL_CheckedChanged(rbtn_Step1_LackAngle_SL, e);
                        else rbtn_Step1_LackAngle_SL.Checked = true;
                    }
                    break;
                case "Step2":
                    {
                        if (rbtn_Step2_LackAngle_SL.Checked) rbtn_Step_LackAngle_SL_CheckedChanged(rbtn_Step2_LackAngle_SL, e);
                        else rbtn_Step2_LackAngle_SL.Checked = true;
                    }
                    break;
                case "Step3":
                    {
                        if (rbtn_Step3_LackAngle_SL.Checked) rbtn_Step_LackAngle_SL_CheckedChanged(rbtn_Step3_LackAngle_SL, e);
                        else rbtn_Step3_LackAngle_SL.Checked = true;
                    }
                    break;
                case "Step4":
                    {
                        if (rbtn_Step4_LackAngle_SL.Checked) rbtn_Step_LackAngle_SL_CheckedChanged(rbtn_Step4_LackAngle_SL, e);
                        else rbtn_Step4_LackAngle_SL.Checked = true;
                    }
                    break;
                case "Step5":
                    {
                        if (rbtn_Step5_LackAngle_SL.Checked) rbtn_Step_LackAngle_SL_CheckedChanged(rbtn_Step5_LackAngle_SL, e);
                        else rbtn_Step5_LackAngle_SL.Checked = true;
                    }
                    break;
                case "Step6":
                    {
                        if (rbtn_Step6_LackAngle_SL.Checked) rbtn_Step_LackAngle_SL_CheckedChanged(rbtn_Step6_LackAngle_SL, e);
                        else rbtn_Step6_LackAngle_SL.Checked = true;
                    }
                    break;
            }
        }

        /// <summary>
        /// 清除 檢測保缺角 (側光) 內所有 rbtn_Step
        /// </summary>
        private void clear_rbtn_Step_LackAngle_SL()
        {
            rbtn_Step1_LackAngle_SL.Checked = false;
            rbtn_Step2_LackAngle_SL.Checked = false;
            rbtn_Step3_LackAngle_SL.Checked = false;
            rbtn_Step4_LackAngle_SL.Checked = false;
            rbtn_Step5_LackAngle_SL.Checked = false;
            rbtn_Step6_LackAngle_SL.Checked = false;
        }
        #endregion

        /// <summary>
        /// 清除所有 rbtn_Step
        /// </summary>
        private void clear_rbtn_Step() // (20190816) Jeff Revised!
        {
            clear_rbtn_Step_SegBlackStripe(); // (20190816) Jeff Revised!
            clear_rbtn_Step_CellSeg_CL();
            clear_rbtn_Step_CellSeg_BL();
            clear_rbtn_Step_Df_CellReg();
            clear_rbtn_Step_WhiteBlob_FL();
            clear_rbtn_Step_WhiteBlob_CL();
            clear_rbtn_Step_Line_FL();
            clear_rbtn_Step_Line_FLCL();
            clear_rbtn_Step_BlackCrack_FL();
            clear_rbtn_Step_BlackCrack_CL();
            clear_rbtn_Step_BlackBlob2_CL();
            clear_rbtn_Step_Dirty_CL();
            clear_rbtn_Step_BrightBlob_BL();
            clear_rbtn_Step_LackAngle_BL();
            clear_rbtn_Step_LackAngle_SL();

            clear_rbtn_Step_AbsolNG(); // (20190810) Jeff Revised!
        }

        private void combxDisplayImg_SelectedIndexChanged(object sender, EventArgs e) // (20190216) Jeff Revised!
        {
            Current_disp_image = Method.Insp_ImgList[combxDisplayImg.SelectedIndex].Clone();
            ClearDisplay(Current_disp_image);
            //clear_rbtn_Step(); // (20190218) Jeff Revised!
        }        

        /// <summary>
        /// Cell分割二選一: Cell分割 (同軸光) or Cell分割 (背光)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbx_CellSeg_CheckedChanged(object sender, EventArgs e)
        {
            // Cell分割 (同軸光) & Cell分割 (背光) 互相切換
            CheckBox cbx = sender as CheckBox;
            //string tag = cbx.Tag.ToString();
            //switch (tag)
            //{
            //    case "CellSeg_CL":
            //        {
            //            if (cbx.Checked)
            //                cbx_CellSeg_BL.Checked = false;
            //            else
            //                cbx_CellSeg_BL.Checked = true;
            //        }
            //        break;
            //    case "CellSeg_BL":
            //        {
            //            if (cbx.Checked)
            //                cbx_CellSeg_CL.Checked = false;
            //            else
            //                cbx_CellSeg_CL.Checked = true;
            //        }
            //        break;
            //}

            // (20190403) Jeff Revised!
            if (cbx == cbx_CellSeg_CL)
            {
                if (cbx.Checked)
                    cbx_CellSeg_BL.Checked = false;
                else
                    cbx_CellSeg_BL.Checked = true;
            }
            else if (cbx == cbx_CellSeg_BL)
            {
                if (cbx.Checked)
                    cbx_CellSeg_CL.Checked = false;
                else
                    cbx_CellSeg_CL.Checked = true;
            }
        }

        private void nudImgCount_ValueChanged(object sender, EventArgs e)
        {
            if (b_LoadRecipe) return;

            Recipe.Param.ImgCount = int.Parse(nudImgCount.Value.ToString());
            
            b_LoadRecipe = true;

            #region 設定各檢測瑕疵之影像ID的最大值
            int id_max = Recipe.Param.ImgCount - 1;
            nud_ImageID_CellSeg_CL.Maximum = id_max;
            nud_ImageID_CellSeg_BL.Maximum = id_max;
            nud_ImageID_WhiteBlob_FL.Maximum = id_max;
            nud_ImageID_WhiteBlob_CL.Maximum = id_max;
            nud_ImageID_BlackCrack_FL.Maximum = id_max;
            nud_ImageID_BlackBlob_CL.Maximum = id_max;
            nud_ImageID_BlackBlob2_CL.Maximum = id_max;
            nud_ImageID1_Dirty_FLCL.Maximum = id_max;
            nud_ImageID2_Dirty_FLCL.Maximum = id_max;
            nud_ImageID_BrightBlob_BL.Maximum = id_max;
            nud_ImageID_LackAngle_BL.Maximum = id_max;
            #endregion

            b_Initial_State = true;
            combxDisplayImg.Items.Clear();
            HOperatorSet.ClearWindow(DisplayWindows.HalconWindow);
            Current_disp_image = null;
            Current_disp_regions.Dispose();
            HOperatorSet.GenEmptyRegion(out Current_disp_regions);
            for (int i = 0; i < Input_ImgList.Count; i++)
            {
                if (Input_ImgList[i] != null)
                {
                    Input_ImgList[i].Dispose();
                    Input_ImgList[i] = null;
                }
            }
            Input_ImgList.Clear();

            /* 更新各檢測瑕疵之影像ID */
            // Load parameter from XML file
            Recipe.load();

            Recipe.Param.ImgCount = int.Parse(nudImgCount.Value.ToString());

            #region AOI參數
            int id;
            // Cell分割 (同軸光) 之參數
            id = (Recipe.Param.InspParam_CellSeg_CL.ImageIndex > id_max) ? id_max : Recipe.Param.InspParam_CellSeg_CL.ImageIndex;
            nud_ImageID_CellSeg_CL.Value = id;

            // Cell分割 (背光) 之參數
            id = (Recipe.Param.InspParam_CellSeg_BL.ImageIndex > id_max) ? id_max : Recipe.Param.InspParam_CellSeg_BL.ImageIndex;
            nud_ImageID_CellSeg_BL.Value = id;

            // 檢測黑條內白點 (正光) (Frontal light) 之參數
            id = (Recipe.Param.InspParam_WhiteBlob_FL.ImageIndex > id_max) ? id_max : Recipe.Param.InspParam_WhiteBlob_FL.ImageIndex;
            nud_ImageID_WhiteBlob_FL.Value = id;

            // 檢測黑條內白點 (同軸光) (Coaxial light) 之參數
            id = (Recipe.Param.InspParam_WhiteBlob_CL.ImageIndex > id_max) ? id_max : Recipe.Param.InspParam_WhiteBlob_CL.ImageIndex;
            nud_ImageID_WhiteBlob_CL.Value = id;

            // 檢測白條內黑點 (Black Crack) (Frontal light) 之參數
            id = (Recipe.Param.InspParam_BlackCrack_FL.ImageIndex > id_max) ? id_max : Recipe.Param.InspParam_BlackCrack_FL.ImageIndex;
            nud_ImageID_BlackCrack_FL.Value = id;

            // 檢測 Black Blob (Coaxial Light) 之參數
            id = (Recipe.Param.InspParam_BlackBlob_CL.ImageIndex > id_max) ? id_max : Recipe.Param.InspParam_BlackBlob_CL.ImageIndex;
            nud_ImageID_BlackBlob_CL.Value = id;

            // 檢測汙染 (Dirty) (Frontal light & Coaxial Light) 之參數
            id = (Recipe.Param.InspParam_Dirty_FLCL.ImageIndex1 > id_max) ? id_max : Recipe.Param.InspParam_Dirty_FLCL.ImageIndex1;
            nud_ImageID1_Dirty_FLCL.Value = id;

            id = (Recipe.Param.InspParam_Dirty_FLCL.ImageIndex2 > id_max) ? id_max : Recipe.Param.InspParam_Dirty_FLCL.ImageIndex2;
            nud_ImageID2_Dirty_FLCL.Value = id;

            // 檢測電極區白點 (Back Light) 之參數
            id = (Recipe.Param.InspParam_BrightBlob_BL.ImageIndex > id_max) ? id_max : Recipe.Param.InspParam_BrightBlob_BL.ImageIndex;
            nud_ImageID_BrightBlob_BL.Value = id;

            // 檢測保缺角 (LackAngle) (Back Light) 之參數
            id = (Recipe.Param.InspParam_LackAngle_BL.ImageIndex > id_max) ? id_max : Recipe.Param.InspParam_LackAngle_BL.ImageIndex;
            nud_ImageID_LackAngle_BL.Value = id;
            #endregion

            b_LoadRecipe = false;
        }

        private void cbx_rotate_CheckedChanged(object sender, EventArgs e)
        {
            b_Initial_State = true;
            clear_rbtn_Step();

            Recipe.Param.Enabled_rotate = cbx_rotate.Checked;
            if (cbx_rotate.Checked)
            {
                nud_rotate.Value = (Decimal)Affine_angle_degree.TupleReal().D;
                nud_rotate.Enabled = true;
            }
            else
            {
                nud_rotate.Enabled = false;
                nud_rotate.Value = 0;
            }

            if (Input_ImgList.Count <= 0)
            {
                //MessageBox.Show("請先載入影像");
                return;
            }

            // 讀取影像並且轉正
            if (!(Method.Read_AllImages_Insp(Recipe, Input_ImgList)))
                MessageBox.Show("載入影像失敗", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void nud_rotate_ValueChanged(object sender, EventArgs e)
        {
            if (!nud_rotate.Enabled) return;

            b_Initial_State = true;
            clear_rbtn_Step();

            Affine_angle_degree = double.Parse(nud_rotate.Value.ToString());

            if (Input_ImgList.Count <= 0)
            {
                //MessageBox.Show("請先載入影像");
                return;
            }

            // 讀取影像並且轉正
            if (!(Method.Read_AllImages_Insp(Recipe, Input_ImgList)))
                MessageBox.Show("載入影像失敗", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void tbInspParamSetting_SelectedIndexChanged(object sender, EventArgs e)
        {
            b_Initial_State = true;
            clear_rbtn_Step();

            TabControl tb = sender as TabControl;
            combx_Algo.SelectedIndex = tb.SelectedIndex;
        }
        #endregion

        #region 檢測程序
        private void button_Info_Algo_Click(object sender, EventArgs e) // (20190403) Jeff Revised!
        {
            List<string> str_algo_Enabled = new List<string>(), str_algo_NotEnabled = new List<string>();
            foreach (TabPage TP in tbInspParamSetting.TabPages)
            {
                foreach (Control C in TP.Controls)
                {
                    if (C is CheckBox)
                    {
                        if (C.Tag.ToString() == "檢測啟用")
                        {
                            CheckBox ch = (CheckBox)C;
                            if (ch.Checked)
                                str_algo_Enabled.Add(TP.Text);
                            else
                                str_algo_NotEnabled.Add(TP.Text);
                            break;
                        }
                    }
                }
            }
            
            richTextBox_log.AppendText("※檢測程序已啟用項目:" + "\n");
            for (int i = 0; i < str_algo_Enabled.Count; i++)
                richTextBox_log.AppendText("    " + str_algo_Enabled[i] + "\n");

            richTextBox_log.AppendText("※檢測程序未啟用項目:" + "\n");
            for (int i = 0; i < str_algo_NotEnabled.Count; i++)
                richTextBox_log.AppendText("    " + str_algo_NotEnabled[i] + "\n");

            richTextBox_log.ScrollToCaret();
        }

        /// <summary>
        /// 清除說明欄位顯示內容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_clear_Info_Click(object sender, EventArgs e) // (20190403) Jeff Revised!
        {
            richTextBox_log.Clear();
        }

        private void combx_Algo_TextChanged(object sender, EventArgs e)
        {
            tbInspParamSetting.SelectedIndex = combx_Algo.SelectedIndex;

            richTextBox_log.AppendText("\n" + "※" + combx_Algo.Text + ":" + "\n");
            richTextBox_log.AppendText(algo_instruction(combx_Algo.Text) + "\n");
            richTextBox_log.ScrollToCaret();
        }

        private string algo_instruction(string str_algo)
        {
            string str_instruction = "";
            switch (str_algo)
            {
                case "初定位":
                    {
                        str_instruction = "定位檢測區&黑條區域";
                    }
                    break;
                case "Cell分割 (同軸光)":
                    {
                        str_instruction = "定位各顆Cell (利用同軸光影像)";
                    }
                    break;
                case "Cell分割 (背光)":
                    {
                        str_instruction = "定位各顆Cell (利用背光影像)";
                    }
                    break;
                case "瑕疵Cell判斷範圍":
                    {
                        str_instruction = "執行AOI檢測，並且標示出瑕疵Cell位置";
                    }
                    break;
            }

            return str_instruction;
        }
        
        //Chris 20190305
        private void nudAIImgID_ValueChanged(object sender, EventArgs e)
        {
            if (nudAIImgID.Value >= Recipe.Param.ImgCount)
            {
                nudAIImgID.Value = Recipe.Param.ImgCount - 1;
            }
        }
        #endregion
    }
}