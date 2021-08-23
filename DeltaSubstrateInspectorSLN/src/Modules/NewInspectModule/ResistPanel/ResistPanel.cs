using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeltaSubstrateInspector.src.Modules.OperateMethods.InspectModule;
using DeltaSubstrateInspector.src.Roles;

using HalconDotNet;

using System.Diagnostics;
using System.IO;
using static DeltaSubstrateInspector.FileSystem.FileSystem;
using DAVS;

using static DeltaSubstrateInspector.FileSystem.FileSystem; // (20190125) Jeff Revised!
using DeltaSubstrateInspector.FileSystem;
using System.Windows.Forms;
using System.Media; // For SystemSounds

namespace DeltaSubstrateInspector.src.Modules.NewInspectModule.ResistPanel
{
    public class ResistPanel : OperateMethod
    {
        static ResistPanel m_Singleton;
        public ResistPanelRole methRole = ResistPanelRole.GetSingleton();
        HObject PatternRegion = null; // Cell Region Centers
        public HObject DefectReg_CellCenter = null; // 所有瑕疵Cell中心
        List<Defect> defectResult = new List<Defect>();
        clsLog Log = new clsLog();
        private List<HObject> insp_ImgList = new List<HObject>();
        public List<HObject> Insp_ImgList
        {
            get { return this.insp_ImgList; }
            //set { this.insp_ImgList = value; }
        }
        private List<HObject> insp_GrayImgList = new List<HObject>();
        public List<HObject> Insp_GrayImgList
        {
            get { return this.insp_GrayImgList; }
            //set { this.insp_GrayImgList = value; }
        }
        object LockObj = new object(); // (20191214) Jeff Revised!


        #region 檢測演算法相關 HObject 變數

        public HObject ho_image_FL = null, ho_image_CL = null, ho_image_BL = null, ho_image_SL = null;
        public HObject ho_GrayImage_FL = null, ho_GrayImage_CL = null, ho_GrayImage_BL = null, ho_GrayImage_SL = null;
        public HObject ho_Region_ImageBorder = null, ho_Regions_White_thresh_FL = null;
        public HObject ho_Regions_White_select = null, ho_Regions_White_opening = null;
        public HObject ho_Regions_White = null, ho_Regions_Inspect_thresh_BL = null;
        public HObject ho_Regions_Inspect_fill = null, ho_Regions_Inspect_opening = null;
        public HObject ho_Regions_Inspect_select = null, ho_Regions_Inspect_union = null;
        public HObject ho_Regions_Inspect_IntSec = null, ho_Regions_Inspect_orig = null;
        public HObject ho_Regions_Inspect0 = null, ho_Regions_Inspect0_Wsmall = null, ho_Regions_Inspect0_Wsmall3 = null, ho_ObjectSelected = null;
        public HObject ho_Regions_temp = null, ho_Regions_WhiteReg_thresh_BL = null;
        public HObject ho_Regions_WhiteReg_select_BL = null, ho_Regions_WhiteReg_BL = null;
        public HObject ho_Regions_Inspect1 = null, ho_ImageReduced_FL = null;
        public HObject ho_Regions_BlackStripe_thresh_FL = null, ho_Regions_BlackStripe_closing = null;
        public HObject ho_Regions_BlackStripe_opening = null, ho_Regions_BlackStripe_select = null;
        public HObject ho_Regions_BlackStripe_union = null, ho_Regions_BlackStripe_FL = null;
        public HObject ho_Regions_BlackStripe_dila = null, ho_Regions_BlackStripe = null;
        public HObject ho_ImageReduced_BlackStripe_BL = null, ho_Region_Blob_Resist_BL = null;
        public HObject ho_Regions_WhiteStripe_BL = null, ho_ImageReduced_WhiteStripe_BL = null;
        public HObject ho_Regions_BlackStripe_small = null;
        public HObject ho_Regions_Inspect1_thresh_FL = null;
        public HObject ho_Regions_Inspect_closing = null, ho_Regions_Inspect = null;
        public HObject ho_RegionClosing_WhiteBlob_FL = null, ho_DefectReg_WhiteBlob0 = null;
        public HObject ho_ImageReduced_BlackStripe_FL = null;
        public HObject ho_Regions_WhiteStripe = null;
        public HObject ho_ImageReduced_WhiteStripe = null;
        public HObject ho_ImageReduced_Inspect_CL = null;
        public HObject ho_Region_BigBlob_thresh_CL = null, ho_DefectReg_BigBlackBlob = null;
        public HObject ho_GrayImage_CL_Zoomed = null, ho_ImageFFT = null, ho_ImageMask = null;
        public HObject ho_RegionUnion_y = null, ho_Rectangle1 = null, ho_Rectangle2 = null;
        public HObject ho_RegionUnion_x = null, ho_Rectangle3 = null, ho_Rectangle4 = null;
        public HObject ho_RegionUnion = null, ho_RegionComplement = null, ho_Filtered = null;
        public HObject ho_ImageFFTInv = null, ho_Regions_Inspect_Zoomed = null, ho_Regions_BlackStripe_Zoomed = null;
        public HObject ho_ImageReduced_FFTInv_CL = null, ho_Region_Blob_thresh_CL = null;
        public HObject ho_DefectReg_BlackBlob_select = null, ho_DefectReg_BlackBlob_Zoomed = null, ho_DefectReg_SmallBlackBlob = null;
        public HObject ho_DefectReg_BlackBlob = null, ho_ImageReduced_BL = null;
        public HObject ho_Region_Blob_thresh_BL = null, ho_ImageReduced_Inspect_BL = null;
        public HObject ho_Region_Blob_Elect_BL = null, ho_Region_Blob_BL = null;
        public HObject ho_DefectReg_BrightBlob = null;

        // 初定位
        public HObject ho_Regions_BlackStripe_Connect = null, ho_CenterLine_BlackStripe = null, ho_Regions_temp_Wsmall = null, ho_Regions_temp_Wsmall3 = null;
        public HObject ho_Regions_Inspect1_Wsmall = null, ho_Regions_Inspect1_Wsmall3 = null, ho_Regions_BlackStripe_Wsmall = null, ho_Regions_BlackStripe_Wsmall3 = null;

        // Cell分割 (同軸光) & (背光)
        public HObject ho_Inspect_ConnectReg = null, ho_AllCells_Rect = null, ho_AllCells_Pt = null, ho_All_ValidCells_rect = null;
        public HObject ho_OneInspectReg = null;
        public HObject ho_Partitioned = null, ho_ImagePart = null, ho_ImageEquHisto2 = null;
        public HObject ho_TiledImage = null, ho_RegionOpening = null;
        public HObject ho_RegionClosing = null, ho_RegionOpening1 = null, ho_RegionDilation = null;
        public HObject ho_RegionIntersection = null, ho_SelectedRegions = null;
        public HObject ho_ValidCells_pt = null, ho_FirstCell_pt = null, ho_ValidCells_rect = null;
        public HObject ho_AllCells_ext_pt = null;
        public HObject ho_OneCell_pt = null, ho_moved = null, ho_Cells_ext_pt = null;
        //public HObject ho_allCells_pt = null, ho_allCells_rect = null, ho_AllCells_pt = null;
        public HObject ho_Regions_removeDef = null, ho_RegionDifference = null, ho_CellReg_Resist;
        public HObject ho_Regions_BlackStripe_1InspReg = null; // (20190529) Jeff Revised!
        // (同軸光)
        public HObject ho_ImageReduced_BlackStripe2_CL = null;
        // (背光)
        public HObject ho_ImageReduced_Inspect2_BL = null, ho_ConnectReg = null;
        // 執行 ResistPanelUC 相關顯示物件 (20191112) Jeff Revised!
        public HObject ho_TiledImage_ALL = null, ho_CellReg_Resist_ALL = null, ho_RegionOpening_ALL = null, ho_RegionClosing_ALL = null, ho_RegionOpening1_ALL = null;
        public HObject ho_RegionIntersection_ALL = null, ho_SelectedRegions_ALL = null;

        // 瑕疵Cell判斷範圍
        public HObject ho_Region_InspReg = null, ho_InspRects_AllCells = null;

        // 檢測相關共用變數
        public HObject ho_Rect_AllCells = null, ho_Rect_AllCells2 = null, ho_Rect_BypassReg = null, ho_RegDif_Cell = null, ho_Rect_AllCells_union = null, ho_Rect_BlackStripe = null, ho_Rect_BlackStripe_union = null;
        public HObject ho_RegDif_BlackStripe = null, ho_DefectReg_op1 = null, ho_DefectReg_op2 = null, ho_Rect_AllCells_union1 = null, ho_Rect_BypassReg_union = null, ho_RegDif_BlackStripe_temp = null;
        public HObject ho_DefectReg_op1_union = null, ho_DefectReg_op2_union = null;
        public HObject ho_DefectReg1_union = null, ho_DefectReg2_union = null;
        public HObject ho_Orig_DefectReg = null; // 完整瑕疵區域
        /// <summary>
        /// 檢測到之所有Cell中心 (旋轉回原始方向)
        /// </summary>
        public HObject ho_AllCellsPt_origin; // (20191122) Jeff Revised!

        private HObject orig_DefectReg_notSureNG = null;
        /// <summary>
        /// 完整瑕疵區域 (模稜兩可之NG)
        /// </summary>
        public HObject Orig_DefectReg_notSureNG // (20190806) Jeff Revised!
        {
            get { return orig_DefectReg_notSureNG; }
            set { orig_DefectReg_notSureNG = value; }
        }

        private HObject defectReg_notSureNG_CellCenter = null;
        /// <summary>
        /// 完整瑕疵區域之Cell中心 (模稜兩可之NG)
        /// </summary>
        public HObject DefectReg_notSureNG_CellCenter // (20190806) Jeff Revised!
        {
            get { return defectReg_notSureNG_CellCenter; }
            set { defectReg_notSureNG_CellCenter = value; }
        }

        private HObject reg_SelectGray_AbsolNG = null;
        /// <summary>
        /// AOI判斷絕對NG之瑕疵灰階標準檢測結果
        /// </summary>
        public HObject Reg_SelectGray_AbsolNG // (20190805) Jeff Revised!
        {
            get { return reg_SelectGray_AbsolNG; }
            set { reg_SelectGray_AbsolNG = value; }
        }

        /* AOI判斷絕對NG (AI覆判NG，只覆判模稜兩可之NG) (20190805) Jeff Revised! */
        private Dictionary<string, Cls_AOI_absolNG> dict_AOI_absolNG = new Dictionary<string, Cls_AOI_absolNG>();
        /// <summary>
        /// 各類型瑕疵之AOI判斷絕對NG (如果不開啟AOI判斷絕對NG，則AOI之NG一律視為絕對NG)
        /// </summary
        public Dictionary<string, Cls_AOI_absolNG> Dict_AOI_absolNG
        {
            get { return dict_AOI_absolNG; }
            set { dict_AOI_absolNG = value; }
        }
        private Dictionary<string, Cls_AOI_absolNG> dict_AOI_NG = new Dictionary<string, Cls_AOI_absolNG>();
        /// <summary>
        /// 各類型瑕疵之AOI判斷NG (不含絕對NG，i.e. 模稜兩可之NG)
        /// </summary>
        public Dictionary<string, Cls_AOI_absolNG> Dict_AOI_NG
        {
            get { return dict_AOI_NG; }
            set { dict_AOI_NG = value; }
        }

        // 檢測絲狀異物 (正光) (Frontal Light)
        public HObject ho_DefectReg_Line = null;

        // 檢測黑條內白點 (正光) (Frontal Light)
        public HObject ho_DefectReg_WhiteBlob_FL = null;

        // 檢測黑條內白點 (同軸光) (Coaxial Light)
        public HObject ho_DefectReg_WhiteBlob_CL = null;

        // 檢測白條內黑點 (正光) & (同軸光)
        public HObject ho_DefectReg_BlackCrack_thresh = null, ho_DefectReg_BlackCrack_CellReg = null, ho_DefectReg_BlackCrack_FL = null, ho_DefectReg_BlackCrack_CL = null;

        // 檢測絲狀異物 (正光&同軸光)
        public HObject ho_DefectReg_Line_FLCL = null, ho_DefectReg_op1_2Images = null, ho_DefectReg_op2_2Images = null, ho_DefectReg_op3_2Images = null;

        // Cell_BlackStripe
        public HObject ho_ImageReduced_AllCells = null, ho_Reg_Cell_Thresh = null, ho_ImageReduced_BlackStripe = null, ho_Reg_Resist_Thresh = null;
        public HObject ho_Reg_select_Cell = null, ho_Reg_select_Resist = null, ho_Reg_select = null, ho_Reg_select_CellReg = null;

        // 檢測黑條內黑點2 (Coaxial Light)
        public HObject ho_DefectReg_BlackBlob2 = null;

        // 檢測汙染 (同軸光)
        public HObject ho_DefectReg_Dirty = null, ho_ImageReduced_AllCells_Dirty_CL = null, ho_Reg_Cell_Dirty_CL = null, ho_ImageReduced_BlackStripe_Dirty_CL = null;
        public HObject ho_Reg_Resist_Dirty_CL = null, ho_Reg_Dirty_CL = null, ho_Reg_Dirty_CL_select = null;

        // 檢測汙染 (正光&同軸光)
        public HObject ho_DefectReg_Dirty_thresh_FL = null, ho_DefectReg_Dirty_FL = null;
        public HObject ho_ImageReduced_BlackStripe_CL = null, ho_DefectReg_Dirty_thresh_CL = null, ho_DefectReg_Dirty_CL = null;
        public HObject ho_DefectReg_Dirty_union = null, ho_DefectReg_Dirty_FLCL = null;

        // 檢測保缺角 (背光)
        public HObject ho_DefectReg_LackAngle_BL = null, ho_Regions_Bright_BL = null;
        public HObject ho_ImageReduced_AllCells_LackAngle_BL = null, ho_Reg_Cell_LackAngle_BL = null, ho_ImageReduced_BlackStripe_LackAngle_BL = null;
        public HObject ho_Reg_Resist_LackAngle_BL = null, ho_Reg_LackAngle_BL = null, ho_Reg_LackAngle_BL_select = null;
        public HObject ho_Image_select = null, ho_Image_Bright = null, ho_ImageReduced_Bright_BL = null;

        // 檢測保缺角 (側光)
        public HObject ho_DefectReg_LackAngle_SL = null;

        #endregion

        #region 檢測演算法相關 HTuple 變數

        // Local control variables
        HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
        HTuple hv_Num_WhiteRegion = new HTuple();
        HTuple hv_Num_InspectRegions = new HTuple();
        HTuple hv_Value_R12C12 = new HTuple();
        HTuple hv_MinW_BlackStripe = new HTuple();
        HTuple hv_MaxW_BlackStripe = new HTuple(), hv_MinH_BlackStripe = new HTuple();
        HTuple hv_MaxH_BlackStripe = new HTuple();
        HTuple hv_MinHeight_defect = new HTuple(), hv_MaxHeight_defect = new HTuple();
        HTuple hv_MinWidth_defect = new HTuple(), hv_MaxWidth_defect = new HTuple();
        HTuple hv_Width_Zoomed = new HTuple();
        HTuple hv_Height_Zoomed = new HTuple(), hv_center_col = new HTuple();
        HTuple hv_center_row = new HTuple();
        HTuple hommat2d_origin_ = new HTuple(); // 旋轉 Cell & 瑕疵region 回原本方向

        // 初定位
        HTuple hv_r_BlackStripe = new HTuple(), hv_c_BlackStripe = new HTuple(), hv_L1_BlackStripe = new HTuple(), hv_Phi_BlackStripe = new HTuple();
        HTuple hv_L1_BlackStripe_Wsmall = new HTuple(), hv_L1_BlackStripe_Wsmall3 = new HTuple();
        HTuple hv_count_BlackStripe = new HTuple();
        HTuple hv_L_NonInspect = new HTuple(), hv_L_NonInspect_Wsmall = new HTuple(), hv_L_NonInspect_Wsmall3 = new HTuple();

        // Cell分割 (同軸光) & (背光)
        HTuple hv_Value_R1 = new HTuple(), hv_Value_C1 = new HTuple(), hv_Value_R2 = new HTuple(), hv_Value_C2 = new HTuple();
        HTuple hv_tile_rect = new HTuple();
        HTuple hv_MinHeight_CellSeg = new HTuple(), hv_MaxHeight_CellSeg = new HTuple();
        HTuple hv_MinWidth_CellSeg = new HTuple(), hv_MaxWidth_CellSeg = new HTuple();      
        HTuple hv_r1_InspectReg = new HTuple(), hv_c1_InspectReg = new HTuple();
        HTuple hv_r2_InspectReg = new HTuple(), hv_c2_InspectReg = new HTuple();
        HTuple hv_r_ValidCells = new HTuple(), hv_c_ValidCells = new HTuple();
        HTuple hv_n_Cell_pdy = new HTuple(), hv_n_Cell_pdx = new HTuple();
        public HTuple hv_y_FirstCell = new HTuple(), hv_x_FirstCell = new HTuple();
        HTuple hv_N_ValidCell = new HTuple(), hv_Phi = new HTuple();
        HTuple hv_r_allCells = new HTuple(), hv_c_allCells = new HTuple();
        HTuple hv_N_allCells = new HTuple();
        HTuple hv_r_AllCells = new HTuple(), hv_c_AllCells = new HTuple(), hv_phi_AllCells = new HTuple(), hv_count_AllCells = new HTuple(); // Cell相關資訊
        HTuple r_BlackStripe_1InspReg = new HTuple(), c_BlackStripe_1InspReg = new HTuple(); // (20191112) Jeff Revised!

        // 瑕疵Cell判斷範圍
        HTuple hv_MinArea_cell = new HTuple(), hv_MaxArea_cell = new HTuple();

        // Cell_BlackStripe_Insp
        HTuple hv_L1_AllCells = new HTuple(), hv_L2_AllCells = new HTuple(), hv_L2_BlackStripe = new HTuple();
        HTuple hv_L1_AllCells2 = new HTuple(), hv_L2_AllCells2 = new HTuple();
        HTuple hv_MinArea_defect = new HTuple(), hv_MaxArea_defect = new HTuple();

        #endregion



        public ResistPanel()
        {

        }

        public static ResistPanel GetSingleton(bool b_ResistPanelUC_ = false) // (20191112) Jeff Revised!
        {
            if (m_Singleton == null)
            {
                m_Singleton = new ResistPanel();
                m_Singleton.b_ResistPanelUC = b_ResistPanelUC_;
            }
            return m_Singleton;
        }

        /// <summary>
        /// 是否是 ResistPanelUC 在執行
        /// </summary>
        private bool b_ResistPanelUC { get; set; } = false; // (20191112) Jeff Revised!

        /// <summary>
        /// 與主軟體對接的視覺演算法
        /// </summary>
        /// <param name="role">演算法參數</param>
        /// <param name="src_imgs">每個取像位置的影像集合</param>
        /// <returns></returns>
        public override HObject get_defect_rgn(InspectRole role, List<HObject> src_imgs)
        {
            //List<clsDAVS.clsResultRegion> ResRegion;
            // 設定參數
            set_parameter((ResistPanelRole)role);

            HObject DefectRegions;
            HOperatorSet.GenEmptyRegion(out DefectRegions);

            if (methRole.DAVS == null)
            {
                methRole.DAVS = new clsDAVS(methRole.DAVSParam, methRole.GetRecipePath());
            }
            string SBID = "";
            if (B_SB_InerCountID)
                SBID = SB_InerCountID;
            else
                SBID = SB_ID;
            methRole.DAVS.SetPathParam(ModuleName, SBID, MOVE_ID, PartNumber);
            // 執行演算法
            if (!execute(methRole, src_imgs, out DefectRegions, out defectResult))
            {               
                Log.WriteLog("Error: execute() 執行失敗!");
                return DefectRegions;
            }

            // For debug!
            //try
            //{
            //    HObject img = src_imgs[1].Clone();
            //    // 轉成RGB image
            //    HTuple Channels;
            //    HOperatorSet.CountChannels(img, out Channels);
            //    if (Channels == 1)
            //    {
            //        HOperatorSet.Compose3(img.Clone(), img.Clone(), img.Clone(), out img);
            //    }
            //    HOperatorSet.OverpaintRegion(img, DefectRegions, ((new HTuple(255)).TupleConcat(0)).TupleConcat(0), "fill");
            //    HOperatorSet.WriteImage(img, "tiff", 0, "img_defect");
            //}
            //catch (Exception e)
            //{ }

            // 回傳檢測結果
            if (DefectRegions == null)
                HOperatorSet.GenEmptyObj(out DefectRegions);
            //HOperatorSet.GenEmptyObj(out DefectRegions); // For debug!
            //HOperatorSet.GenEmptyRegion(out DefectRegions); // For debug!
            //HOperatorSet.GenCircle(out DefectRegions, 100, 100, 40); // For debug!
            return DefectRegions;
        }

        /// <summary>
        /// 演算法回傳每個cell的 region
        /// </summary>
        /// <param name="role"></param>
        /// <param name="src_imgs"></param>
        /// <returns></returns>
        public override HObject get_cell_rgn(InspectRole role, List<HObject> src_imgs) // (20191122) Jeff Revised!
        {
            //HObject CellRegion;
            //CellRegion = null;
            //HOperatorSet.GenEmptyObj(out CellRegion);

            //// Return
            //if (CellRegion == null)
            //    HOperatorSet.GenEmptyObj(out CellRegion);
            //return CellRegion;

            // (20190426) Jeff Revised!
            //if (ho_AllCells_Pt == null)
            //    HOperatorSet.GenEmptyObj(out ho_AllCells_Pt);
            //return ho_AllCells_Pt;

            // (20191122) Jeff Revised!
            if (ho_AllCellsPt_origin == null)
                HOperatorSet.GenEmptyObj(out ho_AllCellsPt_origin);
            return ho_AllCellsPt_origin;
        }

        public override List<Defect> get_defect_result(InspectRole role, List<HObject> src_imgs)
        {
            return defectResult;
        }

        public override Dictionary<string, DefectsResult> Initialize_DefectsClassify(InspectRole role) // (20190806) Jeff Revised!
        {
            Dictionary<string, DefectsResult> defectsClassify = new Dictionary<string, DefectsResult>();

            // 設定參數
            set_parameter((ResistPanelRole)role);
            
            if (FinalInspectParam.b_NG_classification) // 多瑕疵
            {
                try
                {
                    int priority = 0;
                    if (methRole.DAVSParam.DAVS_Mode == 1) // AI啟用
                    {
                        defectsClassify.Add("AI_NG", DefectsResult.Get_DefectsResult("AI_NG", priority++, "#ff00ffff"));
                    }
                    if (methRole.Param.InspParam_WhiteBlob_FL.Enabled)
                    {
                        //defectsClassify.Add("黑條內白點 (正光)", new DefectsResult("黑條內白點 (正光)", priority++, "#0000ff40"));
                        defectsClassify.Add("黑條內白點 (正光)", DefectsResult.Get_DefectsResult("黑條內白點 (正光)", priority++, "#0000ff40"));
                    }
                    if (methRole.Param.InspParam_WhiteBlob_CL.Enabled)
                    {
                        //defectsClassify.Add("黑條內白點 (同軸光)", new DefectsResult("黑條內白點 (同軸光)", priority++, "#00ffff40"));
                        defectsClassify.Add("黑條內白點 (同軸光)", DefectsResult.Get_DefectsResult("黑條內白點 (同軸光)", priority++, "#00ffff40"));
                    }
                    if (methRole.Param.InspParam_Line_FL.Enabled)
                    {
                        //defectsClassify.Add("絲狀異物 (正光)", new DefectsResult("絲狀異物 (正光)", priority++, "#ff00ff40"));
                        defectsClassify.Add("絲狀異物 (正光)", DefectsResult.Get_DefectsResult("絲狀異物 (正光)", priority++, "#ff00ff40"));
                    }
                    if (methRole.Param.InspParam_BlackCrack_FL.Enabled)
                    {
                        //defectsClassify.Add("白條內黑點 (正光)", new DefectsResult("白條內黑點 (正光)", priority++, "#ffff0040"));
                        defectsClassify.Add("白條內黑點 (正光)", DefectsResult.Get_DefectsResult("白條內黑點 (正光)", priority++, "#ffff0040"));
                    }
                    if (methRole.Param.InspParam_BlackCrack_CL.Enabled)
                    {
                        //defectsClassify.Add("白條內黑點 (同軸光)", new DefectsResult("白條內黑點 (同軸光)", priority++, "#7b68ee40"));
                        defectsClassify.Add("白條內黑點 (同軸光)", DefectsResult.Get_DefectsResult("白條內黑點 (同軸光)", priority++, "#7b68ee40"));
                    }
                    if (methRole.Param.InspParam_BlackBlob_CL.Enabled)
                    {
                        //defectsClassify.Add("BlackBlob (同軸光)", new DefectsResult("BlackBlob (同軸光)", priority++, "#ff7f5040"));
                        defectsClassify.Add("BlackBlob (同軸光)", DefectsResult.Get_DefectsResult("BlackBlob (同軸光)", priority++, "#ff7f5040"));
                    }
                    if (methRole.Param.InspParam_BlackBlob2_CL.Enabled)
                    {
                        //defectsClassify.Add("BlackBlob2 (同軸光)", new DefectsResult("BlackBlob2 (同軸光)", priority++, "#00ff7f40"));
                        defectsClassify.Add("BlackBlob2 (同軸光)", DefectsResult.Get_DefectsResult("BlackBlob2 (同軸光)", priority++, "#00ff7f40"));
                    }
                    if (methRole.Param.InspParam_Line_FLCL.Enabled)
                    {
                        //defectsClassify.Add("絲狀異物 (正光&同軸光)", new DefectsResult("絲狀異物 (正光&同軸光)", priority++, "#ff450040"));
                        defectsClassify.Add("絲狀異物 (正光&同軸光)", DefectsResult.Get_DefectsResult("絲狀異物 (正光&同軸光)", priority++, "#ff450040"));
                    }
                    if (methRole.Param.InspParam_Dirty_CL.Enabled)
                    {
                        //defectsClassify.Add("汙染 (同軸光)", new DefectsResult("汙染 (同軸光)", priority++, "#556b2f40"));
                        defectsClassify.Add("汙染 (同軸光)", DefectsResult.Get_DefectsResult("汙染 (同軸光)", priority++, "#556b2f40"));
                    }
                    if (methRole.Param.InspParam_Dirty_FLCL.Enabled)
                    {
                        //defectsClassify.Add("汙染 (正光&同軸光)", new DefectsResult("汙染 (正光&同軸光)", priority++, "#ffc0cb40"));
                        defectsClassify.Add("汙染 (正光&同軸光)", DefectsResult.Get_DefectsResult("汙染 (正光&同軸光)", priority++, "#ffc0cb40"));
                    }
                    if (methRole.Param.InspParam_BrightBlob_BL.Enabled)
                    {
                        //defectsClassify.Add("電極區白點 (背光)", new DefectsResult("電極區白點 (背光)", priority++, "#5f9ea040"));
                        defectsClassify.Add("電極區白點 (背光)", DefectsResult.Get_DefectsResult("電極區白點 (背光)", priority++, "#5f9ea040"));
                    }
                    if (methRole.Param.InspParam_LackAngle_BL.Enabled)
                    {
                        //defectsClassify.Add("保缺角 (背光)", new DefectsResult("保缺角 (背光)", priority++, "#daa52040"));
                        defectsClassify.Add("保缺角 (背光)", DefectsResult.Get_DefectsResult("保缺角 (背光)", priority++, "#daa52040"));
                    }
                    if (methRole.Param.InspParam_LackAngle_SL.Enabled)
                    {
                        //defectsClassify.Add("保缺角 (側光)", new DefectsResult("保缺角 (側光)", priority++, "#40e0d040"));
                        defectsClassify.Add("保缺角 (側光)", DefectsResult.Get_DefectsResult("保缺角 (側光)", priority++, "#40e0d040"));
                    }
                }
                catch
                {
                    SystemSounds.Exclamation.Play();
                    MessageBox.Show("新增瑕疵類型失敗，確請認: \n 1. 瑕疵名稱是否已存在 \n 2. 瑕疵名稱不能為【OK】及【Cell瑕疵】 \n 3. 優先權是否已存在", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            return defectsClassify;
        }

        private string sb_InerCountID = "";
        /// <summary>
        /// 演算法實作
        /// </summary>
        /// <param name="Recipe"></param>
        /// <param name="ho_ImgList"></param>
        /// <param name="MergeDefectRegions"></param>
        /// <param name="DefectResultList"></param>
        /// <returns></returns>
        public bool execute(ResistPanelRole Recipe, List<HObject> ho_ImgList, out HObject MergeDefectRegions ,out List<Defect> DefectResultList) // (20200429) Jeff Revised!
        {
            bool Res = false;
            Recipe.Init();
            HOperatorSet.GenEmptyObj(out MergeDefectRegions);
            DefectResultList = null;
            DefectResultList = new List<Defect>();

            if (sb_InerCountID != SB_InerCountID)
            {
                sb_InerCountID = SB_InerCountID;
                Log.WriteLog("");
                Log.WriteLog("※基板序號: " + SB_InerCountID);
                Log.WriteLog("");
            }
            Log.WriteLog("");
            Log.WriteLog("檢測位置: " + MOVE_ID);
            Log.WriteLog("execute() 執行開始");
            try
            {
                //action_InspectAllDefects(Recipe, ho_ImgList, out MergeDefectRegions, out DefectResultList);
                //Res = true;
                if (action_InspectAllDefects(Recipe, ho_ImgList, out MergeDefectRegions, out DefectResultList))
                    Res = true;
            }
            catch (Exception ex)
            {
                Log.WriteLog("Error: execute() 執行發生例外狀況 -> " + ex.ToString());
                return false;
            }
            finally
            {
                #region 人工覆判 或 AI分圖

                if (FinalInspectParam.b_Recheck || FinalInspectParam.b_AICellImg_Enable) // (20200429) Jeff Revised!
                {
                    if (!b_ResistPanelUC)
                    {
                        // 瑕疵 Region
                        HObject RealDefectRegion = MergeDefectRegions.Clone();
                        for (int i = 0; i < DefectResultList.Count; i++)
                        {
                            if (!DefectResultList[i].B_defect)
                                continue;

                            HOperatorSet.Union2(RealDefectRegion, DefectResultList[i].DefectRegion, out RealDefectRegion);
                        }
                        lock (LockObj)
                            DefReg_MoveIndex_FS.Add(RealDefectRegion);
                    }
                }

                #endregion
            }
            Log.WriteLog("execute() 執行結束");
            
            return Res;
        }

        public bool action_InspectAllDefects(ResistPanelRole Recipe, List<HObject> ho_ImgList, out HObject MergeDefectRegions, out List<Defect> DefectResultList)
        {
            bool Res = false;
            HOperatorSet.GenEmptyObj(out MergeDefectRegions);
            DefectResultList = null;
            DefectResultList = new List<Defect>();
            HObject CellRegion; // (20200429) Jeff Revised!
            HOperatorSet.GenEmptyRegion(out CellRegion); // (20200429) Jeff Revised!

            try
            {
                // Initialize variables
                Init_HObject();
                Init_Dict_AOI_NG_absolNG(Recipe); // (20190805) Jeff Revised!

                #region 讀取影像 Light1: FL (Frontal Light), Light2: CL (Coaxial Light), Light3: BL (Back Light) & SL (Side Light)
                if (!Read_AllImages_Insp(Recipe, ho_ImgList))
                {
                    Log.WriteLog("Error: 讀取影像 失敗!");
                    return false;
                }
                #endregion

                #region 忽略所有檢測
                if (Recipe.Param.TestModeEnabled && Recipe.Param.TestModeType == 0)
                {
                    Log.WriteLog("忽略所有檢測");
                    return true;
                }
                #endregion

                #region 初定位
                if (!InitialPosition_Insp(Recipe))
                {
                    /* 初定位失敗 */
                    // Initialize variables
                    Init_HObject();

                    // 新增結果或瑕疵
                    Add_DefectResultList(Recipe, out DefectResultList, false);

                    Log.WriteLog("Error: 初定位 失敗!");
                    return false;
                }
                #endregion

                #region Cell分割 (同軸光)
                //** Cell Segmentation (0402 同軸光)
                if (Recipe.Param.InspParam_CellSeg_CL.Enabled)
                {
                    if (!CellSeg_CL_Insp(Recipe))
                    {
                        // 新增結果或瑕疵
                        Add_DefectResultList(Recipe, out DefectResultList, false);

                        Log.WriteLog("Error: Cell分割 (同軸光) 失敗!");
                        return false;
                    }
                }
                #endregion

                #region Cell分割 (背光)
                //** Cell Segmentation (0201 背光)
                if (Recipe.Param.InspParam_CellSeg_BL.Enabled)
                {
                    if (!CellSeg_BL_Insp(Recipe))
                    {
                        // 新增結果或瑕疵
                        Add_DefectResultList(Recipe, out DefectResultList, false);

                        Log.WriteLog("Error: Cell分割 (背光) 失敗!");
                        return false;
                    }
                }
                #endregion

                #region 只回傳對位中心 or 只回傳切割範圍 (20190426) Jeff Revised!
                if (Recipe.Param.TestModeEnabled)
                {
                    HObject CellReg = null;
                    if (Recipe.Param.TestModeType == 1) // 只回傳對位中心
                    {
                        Log.WriteLog("只回傳對位中心");
                        HOperatorSet.CopyObj(ho_AllCells_Pt, out CellReg, 1, -1);
                    }
                    else if (Recipe.Param.TestModeType == 2) // 只回傳切割範圍
                    {
                        Log.WriteLog("只回傳切割範圍");
                        HOperatorSet.CopyObj(ho_AllCells_Rect, out CellReg, 1, -1);
                    }

                    // 旋轉瑕疵region回原本方向
                    if (Recipe.Param.Enabled_rotate)
                    {
                        HOperatorSet.AffineTransRegion(CellReg, out MergeDefectRegions, hommat2d_origin_, "nearest_neighbor");
                    }
                    else
                        HOperatorSet.CopyObj(CellReg, out MergeDefectRegions, 1, -1);
                    CellReg.Dispose();
                    return true;
                }
                #endregion

                #region 瑕疵Cell判斷範圍
                if (!Df_CellReg(Recipe))
                {
                    // 新增結果或瑕疵
                    Add_DefectResultList(Recipe, out DefectResultList, false);

                    Log.WriteLog("Error: 瑕疵Cell判斷範圍 失敗!");
                    return false;
                }
                #endregion

                #region 檢測黑條內白點 (正光)
                if (Recipe.Param.InspParam_WhiteBlob_FL.Enabled)
                {
                    if (!WhiteBlob_FL_Insp(Recipe))
                    {
                        // 新增結果或瑕疵
                        Add_DefectResultList(Recipe, out DefectResultList, false);

                        Log.WriteLog("Error: 檢測黑條內白點 (正光) 失敗!");
                        return false;
                    }
                }
                #endregion

                #region 檢測黑條內白點 (同軸光)
                if (Recipe.Param.InspParam_WhiteBlob_CL.Enabled)
                {
                    if (!WhiteBlob_CL_Insp(Recipe))
                    {
                        // 新增結果或瑕疵
                        Add_DefectResultList(Recipe, out DefectResultList, false);

                        Log.WriteLog("Error: 檢測黑條內白點 (同軸光) 失敗!");
                        return false;
                    }
                }
                #endregion

                #region 檢測絲狀異物 (正光)
                if (Recipe.Param.InspParam_Line_FL.Enabled)
                {
                    if (!Line_FL_Insp(Recipe))
                    {
                        // 新增結果或瑕疵
                        Add_DefectResultList(Recipe, out DefectResultList, false);

                        Log.WriteLog("Error: 檢測絲狀異物 (正光) 失敗!");
                        return false;
                    }
                }
                #endregion

                #region 檢測白條內黑點 (正光)
                if (Recipe.Param.InspParam_BlackCrack_FL.Enabled)
                {
                    if (!BlackCrack_FL_Insp(Recipe))
                    {
                        // 新增結果或瑕疵
                        Add_DefectResultList(Recipe, out DefectResultList, false);

                        Log.WriteLog("Error: 檢測白條內黑點 (正光) 失敗!");
                        return false;
                    }
                }
                #endregion

                #region 檢測白條內黑點 (同軸光)
                if (Recipe.Param.InspParam_BlackCrack_CL.Enabled)
                {
                    if (!BlackCrack_CL_Insp(Recipe))
                    {
                        // 新增結果或瑕疵
                        Add_DefectResultList(Recipe, out DefectResultList, false);

                        Log.WriteLog("Error: 檢測白條內黑點 (同軸光) 失敗!");
                        return false;
                    }
                }
                #endregion

                #region 檢測黑條內黑點1 (同軸光)
                if (Recipe.Param.InspParam_BlackBlob_CL.Enabled)
                {
                    if (!BlackBlob_CL_Insp(Recipe))
                    {
                        // 新增結果或瑕疵
                        Add_DefectResultList(Recipe, out DefectResultList, false);

                        Log.WriteLog("Error: 檢測黑條內黑點1 (同軸光) 失敗!");
                        return false;
                    }
                }
                #endregion

                #region 檢測黑條內黑點2 (同軸光)
                if (Recipe.Param.InspParam_BlackBlob2_CL.Enabled)
                {
                    if (!BlackBlob2_CL_Insp(Recipe))
                    {
                        // 新增結果或瑕疵
                        Add_DefectResultList(Recipe, out DefectResultList, false);

                        Log.WriteLog("Error: 檢測黑條內黑點2 (同軸光) 失敗!");
                        return false;
                    }
                }
                #endregion

                #region 檢測絲狀異物 (正光&同軸光)
                if (Recipe.Param.InspParam_Line_FLCL.Enabled)
                {
                    if (!Line_FLCL_Insp(Recipe))
                    {
                        // 新增結果或瑕疵
                        Add_DefectResultList(Recipe, out DefectResultList, false);

                        Log.WriteLog("Error: 檢測絲狀異物 (正光&同軸光) 失敗!");
                        return false;
                    }
                }
                #endregion

                #region 檢測汙染 (同軸光)
                if (Recipe.Param.InspParam_Dirty_CL.Enabled)
                {
                    if (!Dirty_CL_Insp(Recipe))
                    {
                        // 新增結果或瑕疵
                        Add_DefectResultList(Recipe, out DefectResultList, false);

                        Log.WriteLog("Error: 檢測汙染 (同軸光) 失敗!");
                        return false;
                    }
                }
                #endregion

                #region 檢測汙染 (正光&同軸光)
                if (Recipe.Param.InspParam_Dirty_FLCL.Enabled)
                {
                    if (!Dirty_FLCL_Insp(Recipe))
                    {
                        // 新增結果或瑕疵
                        Add_DefectResultList(Recipe, out DefectResultList, false);

                        Log.WriteLog("Error: 檢測汙染 (正光&同軸光) 失敗!");
                        return false;
                    }
                }
                #endregion

                #region 檢測電極區白點 (背光)
                if (Recipe.Param.InspParam_BrightBlob_BL.Enabled)
                {
                    if (!BrightBlob_BL_Insp(Recipe))
                    {
                        // 新增結果或瑕疵
                        Add_DefectResultList(Recipe, out DefectResultList, false);

                        Log.WriteLog("Error: 檢測電極區白點 (背光) 失敗!");
                        return false;
                    }
                }
                #endregion

                #region 檢測保缺角 (背光)
                if (Recipe.Param.InspParam_LackAngle_BL.Enabled)
                {
                    if (!LackAngle_BL_Insp(Recipe))
                    {
                        // 新增結果或瑕疵
                        Add_DefectResultList(Recipe, out DefectResultList, false);

                        Log.WriteLog("Error: 檢測保缺角 (背光) 失敗!");
                        return false;
                    }
                }
                #endregion

                #region 檢測保缺角 (側光)
                if (Recipe.Param.InspParam_LackAngle_SL.Enabled)
                {
                    if (!LackAngle_SL_Insp(Recipe))
                    {
                        // 新增結果或瑕疵
                        Add_DefectResultList(Recipe, out DefectResultList, false);

                        Log.WriteLog("Error: 檢測保缺角 (側光) 失敗!");
                        return false;
                    }
                }
                #endregion



                #region AOI: 結合所有瑕疵 & 新增各類型瑕疵 & 計算瑕疵Cell座標

                // 新增結果或瑕疵
                Add_DefectResultList(Recipe, out DefectResultList, true);

                // 影像檢測範圍
                if (Recipe.Param.InspParam_Df_CellReg.Enabled_InspReg)
                {
                    // 完整瑕疵區域 (NG)
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.Intersection(ho_Region_InspReg, ho_Orig_DefectReg, out ExpTmpOutVar_0);
                        ho_Orig_DefectReg.Dispose();
                        ho_Orig_DefectReg = ExpTmpOutVar_0;
                    }
                    // 完整瑕疵區域 (模稜兩可之NG)
                    if (Recipe.Param.B_AOI_absolNG) // 是否啟用AOI判斷絕對NG
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.Intersection(ho_Region_InspReg, orig_DefectReg_notSureNG, out ExpTmpOutVar_0);
                        orig_DefectReg_notSureNG.Dispose();
                        orig_DefectReg_notSureNG = ExpTmpOutVar_0;
                    }
                }

                // 判定瑕疵cell
                Compute_df_CellCenter(ho_Orig_DefectReg, out DefectReg_CellCenter);
                if (Recipe.Param.B_AOI_absolNG) // 是否啟用AOI判斷絕對NG
                {
                    Compute_df_CellCenter(orig_DefectReg_notSureNG, out defectReg_notSureNG_CellCenter);
                }

                // 新增 AOI瑕疵 結果
                DefectResultList.Add(new Defect("AOI瑕疵", insp_ImgList[0].Clone(), ho_Orig_DefectReg.Clone(), DefectReg_CellCenter.Clone(), ho_AllCells_Pt, false, "#ff0000ff")); // (20190806) Jeff Revised!
                if (Recipe.Param.B_AOI_absolNG) // 是否啟用AOI判斷絕對NG
                {
                    DefectResultList.Add(new Defect("AOI模稜兩可瑕疵", insp_ImgList[0].Clone(), orig_DefectReg_notSureNG.Clone(), defectReg_notSureNG_CellCenter.Clone(), ho_AllCells_Pt, false, "#0000ffff")); // (20190806) Jeff Revised!
                }

                // 旋轉瑕疵region回原本方向
                if (Recipe.Param.Enabled_rotate)
                    HOperatorSet.AffineTransRegion(DefectReg_CellCenter, out MergeDefectRegions, hommat2d_origin_, "nearest_neighbor");
                else
                    HOperatorSet.CopyObj(DefectReg_CellCenter, out MergeDefectRegions, 1, -1);

                #endregion

                #region 判斷 是否儲存AOI影像 及 AI是否啟用

                // (20191214) Jeff Revised!
                if (!(Recipe.Param.SaveAOIImgEnabled) && Recipe.DAVSParam.DAVS_Mode == 0 && !(FinalInspectParam.b_Recheck) && !(FinalInspectParam.b_AICellImg_Enable)) // Recipe.DAVSParam.DAVS_Mode: 0 (不啟用), 1 (線上檢測), 2 (離線學習)
                    return true;

                #endregion

                #region 取得 NG & OK Cell region

                // 影像檢測範圍 (20190401) Jeff Revised!
                //HObject CellRegion; // (20200429) Jeff Revised!
                CellRegion.Dispose(); // (20200429) Jeff Revised!
                if (Recipe.Param.InspParam_Df_CellReg.Enabled_InspReg)
                    LocalDefectMappping(ho_Region_InspReg, ho_InspRects_AllCells, out CellRegion);
                else
                    CellRegion = ho_InspRects_AllCells.Clone();

                // AI分圖 (20191214) Jeff Revised!
                bool b_Add_CellRegion = false;
                if (FinalInspectParam.b_Recheck || FinalInspectParam.b_AICellImg_Enable) // 人工覆判 或 AI分圖 (20200429) Jeff Revised!
                {
                    if (!b_ResistPanelUC)
                    {
                        //lock (LockObj) // 在一個時刻內只允許一個線程進入執行，而其他線程必須等待，確保 CellReg_MoveIndex_FS 內之順序正確!
                        //{
                        //    CellReg_MoveIndex_FS.Add(CellRegion);
                        b_Add_CellRegion = true;
                        //}
                    }
                    if (!(Recipe.Param.SaveAOIImgEnabled) && Recipe.DAVSParam.DAVS_Mode == 0)
                    {
                        if (!b_Add_CellRegion)
                            Extension.HObjectMedthods.ReleaseHObject(ref CellRegion);
                        return true;
                    }
                }

                HObject MappingDefectRegion, MappingPassRegion, MappingAbsolNGRegion = null; // NG, OK, AbsolNG Cell region
                                                                                             // NG Cell region (餵給AI之NG)
                if (Recipe.Param.B_AOI_absolNG) // 是否啟用AOI判斷絕對NG
                {
                    LocalDefectMappping(DefectReg_notSureNG_CellCenter, CellRegion, out MappingDefectRegion);
                    // AbsolNG Cell region
                    LocalDefectMappping(DefectReg_CellCenter, CellRegion, out MappingAbsolNGRegion);
                }
                else
                    LocalDefectMappping(DefectReg_CellCenter, CellRegion, out MappingDefectRegion);
                // OK Cell region
                HOperatorSet.SetSystem("store_empty_region", "false");
                if (Recipe.Param.B_AOI_absolNG) // 是否啟用AOI判斷絕對NG
                {
                    HObject temp;
                    HOperatorSet.Difference(CellRegion, MappingDefectRegion, out temp);
                    HOperatorSet.Difference(temp, MappingAbsolNGRegion, out MappingPassRegion);
                    temp.Dispose();
                }
                else
                    HOperatorSet.Difference(CellRegion, MappingDefectRegion, out MappingPassRegion);
                HOperatorSet.SetSystem("store_empty_region", "true");

                #endregion

                #region 儲存AOI影像

                if (Recipe.Param.SaveAOIImgEnabled)
                {
                    DateTime Time = DateTime.Now;
                    string ImgPath = ModuleParamDirectory + ImageFileDirectory + "AOI_Cell_Image" + "\\" + Time.ToString("yyyyMMdd") + "\\"; // D disk
                                                                                                                                             //string ImgPath = "C:\\DSI" + ImageFileDirectory + "AOI_Cell_Image" + "\\" + Time.ToString("yyyyMMdd") + "\\"; // C disk
                                                                                                                                             //string ImgPath = "Z:\\DSI" + ImageFileDirectory + "AOI_Cell_Image" + "\\" + Time.ToString("yyyyMMdd") + "\\"; // Z disk
                                                                                                                                             //clsSaveImg.clsPacket SavePacket = new clsSaveImg.clsPacket(ho_ImgList[methRole.Param.AIImageID],
                                                                                                                                             //                                                           Recipe, MappingDefectRegion, MappingPassRegion,
                                                                                                                                             //                                                           ImgPath, ModuleName, SB_ID, MOVE_ID, PartNumber);
                                                                                                                                             //clsSaveImg.clsPacket SavePacket = new clsSaveImg.clsPacket(insp_ImgList[Recipe.Param.AIImageID],
                                                                                                                                             //                                                           Recipe, MappingDefectRegion, MappingPassRegion,
                                                                                                                                             //                                                           ImgPath, ModuleName, SB_ID, MOVE_ID, PartNumber); // (20190307) Jeff Revised!
                                                                                                                                             //clsSaveImg.clsPacket SavePacket = new clsSaveImg.clsPacket(insp_ImgList[Recipe.Param.AIImageID],
                                                                                                                                             //                                                           Recipe, MappingDefectRegion, MappingPassRegion,
                                                                                                                                             //                                                           ImgPath, ModuleName, SB_InerCountID, MOVE_ID, PartNumber); // (20190423) Jeff Revised!

                    // (20190424) Jeff Revised!
                    string SBID = "";
                    if (B_SB_InerCountID)
                        SBID = SB_InerCountID;
                    else
                        SBID = SB_ID;
                    clsSaveImg.clsPacket SavePacket = new clsSaveImg.clsPacket(insp_ImgList[Recipe.Param.AIImageID],
                                                                               Recipe, MappingDefectRegion, MappingPassRegion, MappingAbsolNGRegion, // (20190808) Jeff Revised!
                                                                               ImgPath, ModuleName, SBID, MOVE_ID, PartNumber);

                    Recipe.SaveAOIImg.PushImg(SavePacket);
                }

                #endregion

                #region DAVS檢測方式

                // 若AI不啟用
                if (Recipe.DAVSParam.DAVS_Mode == 0)
                {
                    #region Release memories

                    if (!b_Add_CellRegion) // (20191214) Jeff Revised!
                        Extension.HObjectMedthods.ReleaseHObject(ref CellRegion);
                    Extension.HObjectMedthods.ReleaseHObject(ref MappingDefectRegion);
                    Extension.HObjectMedthods.ReleaseHObject(ref MappingPassRegion);
                    Extension.HObjectMedthods.ReleaseHObject(ref MappingAbsolNGRegion);

                    #endregion

                    return true;
                }

                List<clsDAVS.clsResultRegion> ResList = new List<clsDAVS.clsResultRegion>();
                switch (Recipe.Param.DAVSInspType)
                {
                    case 0:
                        {
                            #region 單獨執行AI

                            Recipe.DAVS.InsertPaternMatchSegRegions(CellRegion);

                            if (!Recipe.DAVS.DAVS_execute(Recipe.DAVSParam, insp_ImgList[Recipe.Param.AIImageID]))
                            {
                                Log.WriteLog("Error: DAVS.DAVS_execute() 執行失敗!");
                                return false;
                            }

                            ResList = Recipe.DAVS.GetPredictionRegionList();
                            HObject TmpAIDefectRegions;
                            HOperatorSet.GenEmptyObj(out TmpAIDefectRegions);

                            for (int i = 0; i < ResList.Count; i++)
                            {
                                if (ResList[i].Name != "OK")
                                {
                                    HObject ExpTmpOutVar_0;
                                    HOperatorSet.ConcatObj(TmpAIDefectRegions, ResList[i].ClassResultRegion, out ExpTmpOutVar_0);
                                    TmpAIDefectRegions.Dispose();
                                    TmpAIDefectRegions = ExpTmpOutVar_0;
                                }
                            }

                            #region 加入檢測結果
                            if (Recipe.DAVSParam.DAVS_Mode == 1)
                            {
                                Defect AIRes = new Defect("AI_NG", insp_ImgList[Recipe.Param.AIImageID].Clone(), TmpAIDefectRegions, TmpAIDefectRegions, ho_AllCells_Pt, true, "#ff00ffff");
                                DefectResultList.Add(AIRes);
                            }
                            #endregion

                            #region 合併 AOI & AI 檢測結果 (20190401) Jeff Revised!
                            // Chris 20190305
                            //HObject TempRegion, AIConnectionRegion;
                            //HOperatorSet.Connection(TmpAIDefectRegions, out AIConnectionRegion);
                            //HOperatorSet.ConcatObj(AIConnectionRegion, MergeDefectRegions, out TempRegion);
                            //MergeDefectRegions.Dispose();
                            //MergeDefectRegions = TempRegion.Clone();

                            // (20190423) Jeff Revised!
                            // 轉成一點
                            HObject TmpAIDefectRegions_pt = null;
                            HOperatorSet.ShapeTrans(TmpAIDefectRegions, out TmpAIDefectRegions_pt, "inner_center");
                            HObject Total_DefReg;
                            HOperatorSet.ConcatObj(TmpAIDefectRegions_pt, DefectReg_CellCenter, out Total_DefReg);
                            TmpAIDefectRegions_pt.Dispose();
                            // 旋轉瑕疵region回原本方向
                            MergeDefectRegions.Dispose();
                            if (Recipe.Param.Enabled_rotate)
                                HOperatorSet.AffineTransRegion(Total_DefReg, out MergeDefectRegions, hommat2d_origin_, "nearest_neighbor");
                            else
                                HOperatorSet.CopyObj(Total_DefReg, out MergeDefectRegions, 1, -1);
                            Total_DefReg.Dispose();
                            #endregion

                            #endregion
                        }
                        break;
                    case 1:
                        {
                            #region 覆判NG

                            // (20190401) Jeff Revised!
                            HTuple a_AI, r_AI, c_AI;
                            HOperatorSet.AreaCenter(MappingDefectRegion, out a_AI, out r_AI, out c_AI);
                            if (a_AI.Length == 0)
                            {
                                DefectResultList.Add(new Defect("AI_NG", insp_ImgList[Recipe.Param.AIImageID].Clone(), MappingDefectRegion.Clone(), MappingDefectRegion.Clone(), ho_AllCells_Pt, true, "#ff00ffff"));
                                return true;
                            }

                            Recipe.DAVS.InsertPaternMatchSegRegions(MappingDefectRegion);

                            if (!Recipe.DAVS.DAVS_execute(Recipe.DAVSParam, insp_ImgList[Recipe.Param.AIImageID]))
                            {
                                Log.WriteLog("Error: DAVS.DAVS_execute() 執行失敗!");
                                return false;
                            }

                            ResList = Recipe.DAVS.GetPredictionRegionList();
                            HObject TmpAIDefectRegions;
                            HOperatorSet.GenEmptyObj(out TmpAIDefectRegions);

                            for (int i = 0; i < ResList.Count; i++)
                            {
                                if (ResList[i].Name != "OK")
                                {
                                    HObject ExpTmpOutVar_0;
                                    HOperatorSet.ConcatObj(TmpAIDefectRegions, ResList[i].ClassResultRegion, out ExpTmpOutVar_0);
                                    TmpAIDefectRegions.Dispose();
                                    TmpAIDefectRegions = ExpTmpOutVar_0;
                                }
                            }

                            #region 加入檢測結果
                            Defect AIRes = new Defect("AI_NG", insp_ImgList[Recipe.Param.AIImageID].Clone(), TmpAIDefectRegions, TmpAIDefectRegions, ho_AllCells_Pt, true, "#ff00ffff");
                            DefectResultList.Add(AIRes);
                            #endregion

                            #region 合併 AOI & AI 檢測結果

                            // 瑕疵Region轉成小圓
                            HObject Total_DefReg;
                            Convert2Circle(TmpAIDefectRegions, out Total_DefReg, 1); // (20190423) Jeff Revised!

                            // 旋轉瑕疵Region回原本方向
                            if (Recipe.Param.Enabled_rotate)
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.AffineTransRegion(Total_DefReg, out ExpTmpOutVar_0, hommat2d_origin_, "nearest_neighbor");
                                Total_DefReg.Dispose();
                                Total_DefReg = ExpTmpOutVar_0;
                            }

                            if (Recipe.Param.B_AOI_absolNG) // 是否啟用AOI判斷絕對NG
                            {
                                {
                                    HObject ExpTmpOutVar_0;
                                    HOperatorSet.Union2(MergeDefectRegions, Total_DefReg, out ExpTmpOutVar_0);
                                    MergeDefectRegions.Dispose();
                                    MergeDefectRegions = ExpTmpOutVar_0;
                                }
                                Total_DefReg.Dispose();
                            }
                            else
                            {
                                MergeDefectRegions.Dispose();
                                MergeDefectRegions = Total_DefReg;
                            }

                            #endregion

                            #endregion
                        }
                        break;
                    case 2:
                        {
                            #region 覆判OK

                            // (20190401) Jeff Revised!
                            HTuple a_AI, r_AI, c_AI;
                            HOperatorSet.AreaCenter(MappingPassRegion, out a_AI, out r_AI, out c_AI);
                            if (a_AI.Length == 0)
                            {
                                DefectResultList.Add(new Defect("AI_NG", insp_ImgList[Recipe.Param.AIImageID].Clone(), MappingPassRegion.Clone(), MappingPassRegion.Clone(), ho_AllCells_Pt, true, "#ff00ffff"));
                                return true;
                            }

                            Recipe.DAVS.InsertPaternMatchSegRegions(MappingPassRegion);

                            if (!Recipe.DAVS.DAVS_execute(Recipe.DAVSParam, insp_ImgList[Recipe.Param.AIImageID]))
                            {
                                Log.WriteLog("Error: DAVS.DAVS_execute() 執行失敗!");
                                return false;
                            }

                            ResList = Recipe.DAVS.GetPredictionRegionList();
                            HObject TmpAIDefectRegions;
                            HOperatorSet.GenEmptyObj(out TmpAIDefectRegions);

                            for (int i = 0; i < ResList.Count; i++)
                            {
                                if (ResList[i].Name != "OK")
                                {
                                    HObject ExpTmpOutVar_0;
                                    HOperatorSet.ConcatObj(TmpAIDefectRegions, ResList[i].ClassResultRegion, out ExpTmpOutVar_0);
                                    TmpAIDefectRegions.Dispose();
                                    TmpAIDefectRegions = ExpTmpOutVar_0;
                                }
                            }

                            #region 加入檢測結果
                            Defect AIRes = new Defect("AI_NG", insp_ImgList[Recipe.Param.AIImageID].Clone(), TmpAIDefectRegions, TmpAIDefectRegions, ho_AllCells_Pt, true, "#ff00ffff");
                            DefectResultList.Add(AIRes);
                            #endregion

                            #region 合併 AOI & AI 檢測結果 (20190401) Jeff Revised!
                            // Chris 20190305
                            //HObject TempRegion, AIConnectionRegion;
                            //HOperatorSet.Connection(TmpAIDefectRegions, out AIConnectionRegion);
                            //HOperatorSet.ConcatObj(AIConnectionRegion, MergeDefectRegions, out TempRegion);
                            //MergeDefectRegions.Dispose();
                            //MergeDefectRegions = TempRegion.Clone();

                            // (20190423) Jeff Revised!
                            // 轉成一點
                            HObject TmpAIDefectRegions_pt = null;
                            HOperatorSet.ShapeTrans(TmpAIDefectRegions, out TmpAIDefectRegions_pt, "inner_center");
                            HObject Total_DefReg;
                            HOperatorSet.ConcatObj(TmpAIDefectRegions_pt, DefectReg_CellCenter, out Total_DefReg);
                            TmpAIDefectRegions_pt.Dispose();

                            // 旋轉瑕疵region回原本方向
                            MergeDefectRegions.Dispose();
                            if (Recipe.Param.Enabled_rotate)
                                HOperatorSet.AffineTransRegion(Total_DefReg, out MergeDefectRegions, hommat2d_origin_, "nearest_neighbor");
                            else
                                HOperatorSet.CopyObj(Total_DefReg, out MergeDefectRegions, 1, -1);
                            Total_DefReg.Dispose();
                            #endregion

                            #endregion
                        }
                        break;

                    default:
                        break;
                }

                #endregion

                #region Release memories

                if (!b_Add_CellRegion) // (20191214) Jeff Revised!
                    Extension.HObjectMedthods.ReleaseHObject(ref CellRegion);
                Extension.HObjectMedthods.ReleaseHObject(ref MappingDefectRegion);
                Extension.HObjectMedthods.ReleaseHObject(ref MappingPassRegion);
                Extension.HObjectMedthods.ReleaseHObject(ref MappingAbsolNGRegion);

                #endregion

                Res = true;

            }
            catch
            { }
            finally // (20200429) Jeff Revised!
            {
                if (FinalInspectParam.b_Recheck || FinalInspectParam.b_AICellImg_Enable) // 人工覆判 或 AI分圖 (20200429) Jeff Revised!
                {
                    if (!b_ResistPanelUC)
                    {
                        lock (LockObj) // 在一個時刻內只允許一個線程進入執行，而其他線程必須等待，確保 CellReg_MoveIndex_FS 內之順序正確!
                        {
                            CellReg_MoveIndex_FS.Add(CellRegion);
                        }
                    }
                }
            }

            return Res;
        }

        /// <summary>
        /// 釋放記憶體
        /// </summary>
        private void Release_HObject() // (20190806) Jeff Revised!
        {
            Extension.HObjectMedthods.ReleaseHObject(ref ho_Orig_DefectReg);
            Extension.HObjectMedthods.ReleaseHObject(ref orig_DefectReg_notSureNG); // (20190806) Jeff Revised!
            Extension.HObjectMedthods.ReleaseHObject(ref defectReg_notSureNG_CellCenter); // (20190806) Jeff Revised!
            Extension.HObjectMedthods.ReleaseHObject(ref reg_SelectGray_AbsolNG); // (20190806) Jeff Revised!
            Extension.HObjectMedthods.ReleaseHObject(ref DefectReg_CellCenter);
            //Extension.HObjectMedthods.ReleaseHObject(ref ho_AllCells_Pt);
            //Extension.HObjectMedthods.ReleaseHObject(ref ho_Regions_BlackStripe);
            //Extension.HObjectMedthods.ReleaseHObject(ref ho_AllCells_Rect);
            Extension.HObjectMedthods.ReleaseHObject(ref ho_DefectReg_WhiteBlob_FL);
            Extension.HObjectMedthods.ReleaseHObject(ref ho_DefectReg_WhiteBlob_CL);
            Extension.HObjectMedthods.ReleaseHObject(ref ho_DefectReg_Line);
            Extension.HObjectMedthods.ReleaseHObject(ref ho_DefectReg_BlackCrack_FL);
            Extension.HObjectMedthods.ReleaseHObject(ref ho_DefectReg_BlackCrack_CL);
            Extension.HObjectMedthods.ReleaseHObject(ref ho_DefectReg_BlackBlob);
            Extension.HObjectMedthods.ReleaseHObject(ref ho_DefectReg_BlackBlob2);
            Extension.HObjectMedthods.ReleaseHObject(ref ho_DefectReg_Line_FLCL);
            Extension.HObjectMedthods.ReleaseHObject(ref ho_DefectReg_Dirty);
            Extension.HObjectMedthods.ReleaseHObject(ref ho_DefectReg_Dirty_FLCL);
            Extension.HObjectMedthods.ReleaseHObject(ref ho_DefectReg_BrightBlob);
            Extension.HObjectMedthods.ReleaseHObject(ref ho_DefectReg_LackAngle_BL);
            Extension.HObjectMedthods.ReleaseHObject(ref ho_DefectReg_LackAngle_SL);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init_HObject() // (20190806) Jeff Revised!
        {
            // 不能釋放記憶體，否則外層儲存Region之記憶體會消失!
            Release_HObject(); // (20190806) Jeff Revised!

            HOperatorSet.GenEmptyObj(out ho_Orig_DefectReg);
            HOperatorSet.GenEmptyObj(out orig_DefectReg_notSureNG); // (20190806) Jeff Revised!
            HOperatorSet.GenEmptyObj(out defectReg_notSureNG_CellCenter); // (20190806) Jeff Revised!
            HOperatorSet.GenEmptyObj(out reg_SelectGray_AbsolNG); // (20190806) Jeff Revised!
            HOperatorSet.GenEmptyObj(out DefectReg_CellCenter);
            HOperatorSet.GenEmptyObj(out ho_AllCells_Pt);
            HOperatorSet.GenEmptyObj(out ho_AllCellsPt_origin); // (20191122) Jeff Revised!
            HOperatorSet.GenEmptyObj(out ho_Regions_BlackStripe);
            HOperatorSet.GenEmptyObj(out ho_AllCells_Rect);
            HOperatorSet.GenEmptyObj(out ho_DefectReg_WhiteBlob_FL);
            HOperatorSet.GenEmptyObj(out ho_DefectReg_WhiteBlob_CL);
            HOperatorSet.GenEmptyObj(out ho_DefectReg_Line);
            HOperatorSet.GenEmptyObj(out ho_DefectReg_BlackCrack_FL);
            HOperatorSet.GenEmptyObj(out ho_DefectReg_BlackCrack_CL);
            HOperatorSet.GenEmptyObj(out ho_DefectReg_BlackBlob);
            HOperatorSet.GenEmptyObj(out ho_DefectReg_BlackBlob2);
            HOperatorSet.GenEmptyObj(out ho_DefectReg_Line_FLCL);
            HOperatorSet.GenEmptyObj(out ho_DefectReg_Dirty);
            HOperatorSet.GenEmptyObj(out ho_DefectReg_Dirty_FLCL);
            HOperatorSet.GenEmptyObj(out ho_DefectReg_BrightBlob);
            HOperatorSet.GenEmptyObj(out ho_DefectReg_LackAngle_BL);
            HOperatorSet.GenEmptyObj(out ho_DefectReg_LackAngle_SL);
        }

        /// <summary>
        /// 釋放記憶體 (Dict_AOI_absolNG)
        /// </summary>
        private void Release_Dict_AOI_absolNG() // (20190805) Jeff Revised!
        {
            foreach (Cls_AOI_absolNG cls in Dict_AOI_absolNG.Values)
                cls.Release();
        }

        /// <summary>
        /// 釋放記憶體 (Dict_AOI_NG)
        /// </summary>
        private void Release_Dict_AOI_NG() // (20190805) Jeff Revised!
        {
            foreach (Cls_AOI_absolNG cls in Dict_AOI_NG.Values)
                cls.Release();
        }

        /// <summary>
        /// 初始化 Dict_AOI_NG & Dict_AOI_absolNG
        /// </summary>
        /// <param name="Recipe"></param>
        public void Init_Dict_AOI_NG_absolNG(ResistPanelRole Recipe) // (20190806) Jeff Revised!
        {
            Release_Dict_AOI_absolNG();
            Dict_AOI_absolNG.Clear();

            Release_Dict_AOI_NG();
            Dict_AOI_NG.Clear();
            
            #region 加入瑕疵
            
            //if (Recipe.Param.InspParam_WhiteBlob_FL.Enabled)
            {
                Dict_AOI_NG.Add("黑條內白點 (正光)", Cls_AOI_absolNG.Get_Cls_AOI_absolNG("黑條內白點 (正光)", Recipe.Param.InspParam_WhiteBlob_FL.Enabled));
                //if (Recipe.Param.B_AOI_absolNG) // 是否啟用AOI判斷絕對NG
                    Dict_AOI_absolNG.Add("黑條內白點 (正光)", Cls_AOI_absolNG.Get_Cls_AOI_absolNG("黑條內白點 (正光)", Recipe.Param.InspParam_WhiteBlob_FL.cls_absolNG.Enabled));
            }
            //if (Recipe.Param.InspParam_WhiteBlob_CL.Enabled)
            {
                Dict_AOI_NG.Add("黑條內白點 (同軸光)", Cls_AOI_absolNG.Get_Cls_AOI_absolNG("黑條內白點 (同軸光)", Recipe.Param.InspParam_WhiteBlob_CL.Enabled));
                //if (Recipe.Param.B_AOI_absolNG) // 是否啟用AOI判斷絕對NG
                    Dict_AOI_absolNG.Add("黑條內白點 (同軸光)", Cls_AOI_absolNG.Get_Cls_AOI_absolNG("黑條內白點 (同軸光)", Recipe.Param.InspParam_WhiteBlob_CL.cls_absolNG.Enabled));
            }
            //if (Recipe.Param.InspParam_Line_FL.Enabled)
            {
                Dict_AOI_NG.Add("絲狀異物 (正光)", Cls_AOI_absolNG.Get_Cls_AOI_absolNG("絲狀異物 (正光)", Recipe.Param.InspParam_Line_FL.Enabled));
                //if (Recipe.Param.B_AOI_absolNG) // 是否啟用AOI判斷絕對NG
                    Dict_AOI_absolNG.Add("絲狀異物 (正光)", Cls_AOI_absolNG.Get_Cls_AOI_absolNG("絲狀異物 (正光)", Recipe.Param.InspParam_Line_FL.cls_absolNG.Enabled));
            }
            //if (Recipe.Param.InspParam_BlackCrack_FL.Enabled)
            {
                Dict_AOI_NG.Add("白條內黑點 (正光)", Cls_AOI_absolNG.Get_Cls_AOI_absolNG("白條內黑點 (正光)", Recipe.Param.InspParam_BlackCrack_FL.Enabled));
                //if (Recipe.Param.B_AOI_absolNG) // 是否啟用AOI判斷絕對NG
                    Dict_AOI_absolNG.Add("白條內黑點 (正光)", Cls_AOI_absolNG.Get_Cls_AOI_absolNG("白條內黑點 (正光)", Recipe.Param.InspParam_BlackCrack_FL.cls_absolNG.Enabled));
            }
            //if (Recipe.Param.InspParam_BlackCrack_CL.Enabled)
            {
                Dict_AOI_NG.Add("白條內黑點 (同軸光)", Cls_AOI_absolNG.Get_Cls_AOI_absolNG("白條內黑點 (同軸光)", Recipe.Param.InspParam_BlackCrack_CL.Enabled));
                //if (Recipe.Param.B_AOI_absolNG) // 是否啟用AOI判斷絕對NG
                    Dict_AOI_absolNG.Add("白條內黑點 (同軸光)", Cls_AOI_absolNG.Get_Cls_AOI_absolNG("白條內黑點 (同軸光)", Recipe.Param.InspParam_BlackCrack_CL.cls_absolNG.Enabled));
            }
            //if (Recipe.Param.InspParam_BlackBlob_CL.Enabled)
            {
                Dict_AOI_NG.Add("BlackBlob (同軸光)", Cls_AOI_absolNG.Get_Cls_AOI_absolNG("BlackBlob (同軸光)", Recipe.Param.InspParam_BlackBlob_CL.Enabled));
                //if (Recipe.Param.B_AOI_absolNG) // 是否啟用AOI判斷絕對NG
                    Dict_AOI_absolNG.Add("BlackBlob (同軸光)", Cls_AOI_absolNG.Get_Cls_AOI_absolNG("BlackBlob (同軸光)", Recipe.Param.InspParam_BlackBlob_CL.cls_absolNG.Enabled));
            }
            //if (Recipe.Param.InspParam_BlackBlob2_CL.Enabled)
            {
                Dict_AOI_NG.Add("BlackBlob2 (同軸光)", Cls_AOI_absolNG.Get_Cls_AOI_absolNG("BlackBlob2 (同軸光)", Recipe.Param.InspParam_BlackBlob2_CL.Enabled));
                //if (Recipe.Param.B_AOI_absolNG) // 是否啟用AOI判斷絕對NG
                    Dict_AOI_absolNG.Add("BlackBlob2 (同軸光)", Cls_AOI_absolNG.Get_Cls_AOI_absolNG("BlackBlob2 (同軸光)", Recipe.Param.InspParam_BlackBlob2_CL.cls_absolNG.Enabled));
            }
            //if (Recipe.Param.InspParam_Line_FLCL.Enabled)
            {
                Dict_AOI_NG.Add("絲狀異物 (正光&同軸光)", Cls_AOI_absolNG.Get_Cls_AOI_absolNG("絲狀異物 (正光&同軸光)", Recipe.Param.InspParam_Line_FLCL.Enabled));
                //if (Recipe.Param.B_AOI_absolNG) // 是否啟用AOI判斷絕對NG
                    Dict_AOI_absolNG.Add("絲狀異物 (正光&同軸光)", Cls_AOI_absolNG.Get_Cls_AOI_absolNG("絲狀異物 (正光&同軸光)", Recipe.Param.InspParam_Line_FLCL.cls_absolNG.Enabled));
            }
            //if (Recipe.Param.InspParam_Dirty_CL.Enabled)
            {
                Dict_AOI_NG.Add("汙染 (同軸光)", Cls_AOI_absolNG.Get_Cls_AOI_absolNG("汙染 (同軸光)", Recipe.Param.InspParam_Dirty_CL.Enabled));
                //if (Recipe.Param.B_AOI_absolNG) // 是否啟用AOI判斷絕對NG
                    Dict_AOI_absolNG.Add("汙染 (同軸光)", Cls_AOI_absolNG.Get_Cls_AOI_absolNG("汙染 (同軸光)", Recipe.Param.InspParam_Dirty_CL.cls_absolNG.Enabled));
            }
            //if (Recipe.Param.InspParam_Dirty_FLCL.Enabled)
            {
                Dict_AOI_NG.Add("汙染 (正光&同軸光)", Cls_AOI_absolNG.Get_Cls_AOI_absolNG("汙染 (正光&同軸光)", Recipe.Param.InspParam_Dirty_FLCL.Enabled));
                //if (Recipe.Param.B_AOI_absolNG) // 是否啟用AOI判斷絕對NG
                    Dict_AOI_absolNG.Add("汙染 (正光&同軸光)", Cls_AOI_absolNG.Get_Cls_AOI_absolNG("汙染 (正光&同軸光)", Recipe.Param.InspParam_Dirty_FLCL.cls_absolNG.Enabled));
            }
            //if (Recipe.Param.InspParam_BrightBlob_BL.Enabled)
            {
                Dict_AOI_NG.Add("電極區白點 (背光)", Cls_AOI_absolNG.Get_Cls_AOI_absolNG("電極區白點 (背光)", Recipe.Param.InspParam_BrightBlob_BL.Enabled));
                //if (Recipe.Param.B_AOI_absolNG) // 是否啟用AOI判斷絕對NG
                    Dict_AOI_absolNG.Add("電極區白點 (背光)", Cls_AOI_absolNG.Get_Cls_AOI_absolNG("電極區白點 (背光)", Recipe.Param.InspParam_BrightBlob_BL.cls_absolNG.Enabled));
            }
            //if (Recipe.Param.InspParam_LackAngle_BL.Enabled)
            {
                Dict_AOI_NG.Add("保缺角 (背光)", Cls_AOI_absolNG.Get_Cls_AOI_absolNG("保缺角 (背光)", Recipe.Param.InspParam_LackAngle_BL.Enabled));
                //if (Recipe.Param.B_AOI_absolNG) // 是否啟用AOI判斷絕對NG
                    Dict_AOI_absolNG.Add("保缺角 (背光)", Cls_AOI_absolNG.Get_Cls_AOI_absolNG("保缺角 (背光)", Recipe.Param.InspParam_LackAngle_BL.cls_absolNG.Enabled));
            }
            //if (Recipe.Param.InspParam_LackAngle_SL.Enabled)
            {
                Dict_AOI_NG.Add("保缺角 (側光)", Cls_AOI_absolNG.Get_Cls_AOI_absolNG("保缺角 (側光)", Recipe.Param.InspParam_LackAngle_SL.Enabled));
                //if (Recipe.Param.B_AOI_absolNG) // 是否啟用AOI判斷絕對NG
                    Dict_AOI_absolNG.Add("保缺角 (側光)", Cls_AOI_absolNG.Get_Cls_AOI_absolNG("保缺角 (側光)", Recipe.Param.InspParam_LackAngle_SL.cls_absolNG.Enabled));
            }
            
            #endregion

        }

        /// <summary>
        /// 新增結果或瑕疵
        /// </summary>
        /// <param name="Recipe"></param>
        /// <param name="DefectResultList"></param>
        /// <param name="b_defect"></param>
        /// <returns></returns>
        private bool Add_DefectResultList(ResistPanelRole Recipe, out List<Defect> DefectResultList, bool b_defect = false) // (20190807) Jeff Revised!
        {
            bool Res = false;
            DefectResultList = null;
            DefectResultList = new List<Defect>();

            try
            {
                #region 新增結果或瑕疵

                HObject df_cellcenter_rgn;
                HOperatorSet.GenEmptyRegion(out df_cellcenter_rgn);
                // 新增 初定位 結果
                DefectResultList.Add(new Defect("初定位", insp_ImgList[0].Clone(), ho_Regions_BlackStripe, df_cellcenter_rgn.Clone(), ho_AllCells_Pt, false, "#00ff7fc0")); // (20190624) Jeff Revised!
                if (Recipe.Param.InspParam_CellSeg_CL.Enabled)
                {
                    if (b_defect)
                    {
                        //HOperatorSet.Connection(ho_AllCells_Rect.Clone(), out ho_AllCells_Rect);
                        if (Recipe.Param.Enabled_Compute_df_CellCenter)
                        {
                            Extension.HObjectMedthods.ReleaseHObject(ref df_cellcenter_rgn);
                            Compute_df_CellCenter(ho_AllCells_Rect, out df_cellcenter_rgn);
                        }
                    }

                    // 新增 Cell分割 結果
                    DefectResultList.Add(new Defect("Cell分割 (同軸光)", insp_ImgList[Recipe.Param.InspParam_CellSeg_CL.ImageIndex].Clone(), ho_AllCells_Rect, df_cellcenter_rgn.Clone(), ho_AllCells_Pt, false, "#00ff00ff")); // (20190624) Jeff Revised!
                }
                if (Recipe.Param.InspParam_CellSeg_BL.Enabled)
                {
                    if (b_defect)
                    {
                        //HOperatorSet.Connection(ho_AllCells_Rect.Clone(), out ho_AllCells_Rect);
                        if (Recipe.Param.Enabled_Compute_df_CellCenter)
                        {
                            Extension.HObjectMedthods.ReleaseHObject(ref df_cellcenter_rgn);
                            Compute_df_CellCenter(ho_AllCells_Rect, out df_cellcenter_rgn);
                        }
                    }

                    // 新增 Cell分割 結果
                    DefectResultList.Add(new Defect("Cell分割 (背光)", insp_ImgList[Recipe.Param.InspParam_CellSeg_BL.ImageIndex].Clone(), ho_AllCells_Rect, df_cellcenter_rgn.Clone(), ho_AllCells_Pt, false, "#00ff00ff")); // (20190624) Jeff Revised!
                }
                if (Recipe.Param.InspParam_WhiteBlob_FL.Enabled)
                {
                    if (b_defect)
                    {
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.Union2(ho_Orig_DefectReg, Dict_AOI_absolNG["黑條內白點 (正光)"].DefectReg, out ExpTmpOutVar_0);
                            ho_Orig_DefectReg.Dispose();
                            ho_Orig_DefectReg = ExpTmpOutVar_0;
                        }
                        
                        if (FinalInspectParam.b_NG_classification) // 多瑕疵
                        {
                            Extension.HObjectMedthods.ReleaseHObject(ref df_cellcenter_rgn);
                            Compute_DefectsClassify_CellCenter(Recipe, Dict_AOI_absolNG["黑條內白點 (正光)"].DefectReg, out df_cellcenter_rgn);
                        }
                        else if (Recipe.Param.Enabled_Compute_df_CellCenter)
                        {
                            Extension.HObjectMedthods.ReleaseHObject(ref df_cellcenter_rgn);
                            Compute_df_CellCenter(Dict_AOI_absolNG["黑條內白點 (正光)"].DefectReg, out df_cellcenter_rgn);
                        }

                        /* 計算模稜兩可之NG */
                        if (Recipe.Param.B_AOI_absolNG) // 是否啟用AOI判斷絕對NG
                        {
                            //if (Recipe.Param.InspParam_WhiteBlob_FL.cls_absolNG.Enabled)
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.Union2(orig_DefectReg_notSureNG, Dict_AOI_NG["黑條內白點 (正光)"].DefectReg, out ExpTmpOutVar_0);
                                orig_DefectReg_notSureNG.Dispose();
                                orig_DefectReg_notSureNG = ExpTmpOutVar_0;
                            }
                        }

                    }

                    // 新增瑕疵
                    DefectResultList.Add(new Defect("黑條內白點 (正光)", insp_ImgList[Recipe.Param.InspParam_WhiteBlob_FL.ImageIndex].Clone(), Dict_AOI_absolNG["黑條內白點 (正光)"].DefectReg.Clone(), df_cellcenter_rgn.Clone(), ho_AllCells_Pt, true, "#0000ff40")); // (20190624) Jeff Revised!
                }
                if (Recipe.Param.InspParam_WhiteBlob_CL.Enabled)
                {
                    if (b_defect)
                    {
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.Union2(ho_Orig_DefectReg, Dict_AOI_absolNG["黑條內白點 (同軸光)"].DefectReg, out ExpTmpOutVar_0);
                            ho_Orig_DefectReg.Dispose();
                            ho_Orig_DefectReg = ExpTmpOutVar_0;
                        }

                        if (FinalInspectParam.b_NG_classification) // 多瑕疵
                        {
                            Extension.HObjectMedthods.ReleaseHObject(ref df_cellcenter_rgn);
                            Compute_DefectsClassify_CellCenter(Recipe, Dict_AOI_absolNG["黑條內白點 (同軸光)"].DefectReg, out df_cellcenter_rgn);
                        }
                        else if (Recipe.Param.Enabled_Compute_df_CellCenter)
                        {
                            Extension.HObjectMedthods.ReleaseHObject(ref df_cellcenter_rgn);
                            Compute_df_CellCenter(Dict_AOI_absolNG["黑條內白點 (同軸光)"].DefectReg, out df_cellcenter_rgn);
                        }

                        /* 計算模稜兩可之NG */
                        if (Recipe.Param.B_AOI_absolNG) // 是否啟用AOI判斷絕對NG
                        {
                            //if (Recipe.Param.InspParam_WhiteBlob_CL.cls_absolNG.Enabled)
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.Union2(orig_DefectReg_notSureNG, Dict_AOI_NG["黑條內白點 (同軸光)"].DefectReg, out ExpTmpOutVar_0);
                                orig_DefectReg_notSureNG.Dispose();
                                orig_DefectReg_notSureNG = ExpTmpOutVar_0;
                            }
                        }

                    }

                    // 新增瑕疵
                    DefectResultList.Add(new Defect("黑條內白點 (同軸光)", insp_ImgList[Recipe.Param.InspParam_WhiteBlob_CL.ImageIndex].Clone(), Dict_AOI_absolNG["黑條內白點 (同軸光)"].DefectReg.Clone(), df_cellcenter_rgn.Clone(), ho_AllCells_Pt, true, "#00ffff40")); // (20190624) Jeff Revised!
                }
                if (Recipe.Param.InspParam_Line_FL.Enabled)
                {
                    if (b_defect)
                    {
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.Union2(ho_Orig_DefectReg, Dict_AOI_absolNG["絲狀異物 (正光)"].DefectReg, out ExpTmpOutVar_0);
                            ho_Orig_DefectReg.Dispose();
                            ho_Orig_DefectReg = ExpTmpOutVar_0;
                        }

                        if (FinalInspectParam.b_NG_classification) // 多瑕疵
                        {
                            Extension.HObjectMedthods.ReleaseHObject(ref df_cellcenter_rgn);
                            Compute_DefectsClassify_CellCenter(Recipe, Dict_AOI_absolNG["絲狀異物 (正光)"].DefectReg, out df_cellcenter_rgn);
                        }
                        else if (Recipe.Param.Enabled_Compute_df_CellCenter)
                        {
                            Extension.HObjectMedthods.ReleaseHObject(ref df_cellcenter_rgn);
                            Compute_df_CellCenter(Dict_AOI_absolNG["絲狀異物 (正光)"].DefectReg, out df_cellcenter_rgn);
                        }

                        /* 計算模稜兩可之NG */
                        if (Recipe.Param.B_AOI_absolNG) // 是否啟用AOI判斷絕對NG
                        {
                            //if (Recipe.Param.InspParam_Line_FL.cls_absolNG.Enabled)
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.Union2(orig_DefectReg_notSureNG, Dict_AOI_NG["絲狀異物 (正光)"].DefectReg, out ExpTmpOutVar_0);
                                orig_DefectReg_notSureNG.Dispose();
                                orig_DefectReg_notSureNG = ExpTmpOutVar_0;
                            }
                        }

                    }

                    // 新增瑕疵
                    DefectResultList.Add(new Defect("絲狀異物 (正光)", insp_ImgList[Recipe.Param.InspParam_BlackBlob2_CL.ImageIndex].Clone(), Dict_AOI_absolNG["絲狀異物 (正光)"].DefectReg.Clone(), df_cellcenter_rgn.Clone(), ho_AllCells_Pt, true, "#ff00ff40")); // (20190624) Jeff Revised!
                }
                if (Recipe.Param.InspParam_BlackCrack_FL.Enabled)
                {
                    if (b_defect)
                    {
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.Union2(ho_Orig_DefectReg, Dict_AOI_absolNG["白條內黑點 (正光)"].DefectReg, out ExpTmpOutVar_0);
                            ho_Orig_DefectReg.Dispose();
                            ho_Orig_DefectReg = ExpTmpOutVar_0;
                        }

                        if (FinalInspectParam.b_NG_classification) // 多瑕疵
                        {
                            Extension.HObjectMedthods.ReleaseHObject(ref df_cellcenter_rgn);
                            Compute_DefectsClassify_CellCenter(Recipe, Dict_AOI_absolNG["白條內黑點 (正光)"].DefectReg, out df_cellcenter_rgn);
                        }
                        else if (Recipe.Param.Enabled_Compute_df_CellCenter)
                        {
                            Extension.HObjectMedthods.ReleaseHObject(ref df_cellcenter_rgn);
                            Compute_df_CellCenter(Dict_AOI_absolNG["白條內黑點 (正光)"].DefectReg, out df_cellcenter_rgn);
                        }

                        /* 計算模稜兩可之NG */
                        if (Recipe.Param.B_AOI_absolNG) // 是否啟用AOI判斷絕對NG
                        {
                            //if (Recipe.Param.InspParam_BlackCrack_FL.cls_absolNG.Enabled)
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.Union2(orig_DefectReg_notSureNG, Dict_AOI_NG["白條內黑點 (正光)"].DefectReg, out ExpTmpOutVar_0);
                                orig_DefectReg_notSureNG.Dispose();
                                orig_DefectReg_notSureNG = ExpTmpOutVar_0;
                            }
                        }

                    }

                    // 新增瑕疵
                    DefectResultList.Add(new Defect("白條內黑點 (正光)", insp_ImgList[Recipe.Param.InspParam_BlackCrack_FL.ImageIndex].Clone(), Dict_AOI_absolNG["白條內黑點 (正光)"].DefectReg.Clone(), df_cellcenter_rgn.Clone(), ho_AllCells_Pt, true, "#ffff0040")); // (20190624) Jeff Revised!
                }
                if (Recipe.Param.InspParam_BlackCrack_CL.Enabled)
                {
                    if (b_defect)
                    {
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.Union2(ho_Orig_DefectReg, Dict_AOI_absolNG["白條內黑點 (同軸光)"].DefectReg, out ExpTmpOutVar_0);
                            ho_Orig_DefectReg.Dispose();
                            ho_Orig_DefectReg = ExpTmpOutVar_0;
                        }

                        if (FinalInspectParam.b_NG_classification) // 多瑕疵
                        {
                            Extension.HObjectMedthods.ReleaseHObject(ref df_cellcenter_rgn);
                            Compute_DefectsClassify_CellCenter(Recipe, Dict_AOI_absolNG["白條內黑點 (同軸光)"].DefectReg, out df_cellcenter_rgn);
                        }
                        else if (Recipe.Param.Enabled_Compute_df_CellCenter)
                        {
                            Extension.HObjectMedthods.ReleaseHObject(ref df_cellcenter_rgn);
                            Compute_df_CellCenter(Dict_AOI_absolNG["白條內黑點 (同軸光)"].DefectReg, out df_cellcenter_rgn);
                        }

                        /* 計算模稜兩可之NG */
                        if (Recipe.Param.B_AOI_absolNG) // 是否啟用AOI判斷絕對NG
                        {
                            //if (Recipe.Param.InspParam_BlackCrack_CL.cls_absolNG.Enabled)
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.Union2(orig_DefectReg_notSureNG, Dict_AOI_NG["白條內黑點 (同軸光)"].DefectReg, out ExpTmpOutVar_0);
                                orig_DefectReg_notSureNG.Dispose();
                                orig_DefectReg_notSureNG = ExpTmpOutVar_0;
                            }
                        }

                    }

                    // 新增瑕疵
                    DefectResultList.Add(new Defect("白條內黑點 (同軸光)", insp_ImgList[Recipe.Param.InspParam_BlackCrack_CL.ImageIndex].Clone(), Dict_AOI_absolNG["白條內黑點 (同軸光)"].DefectReg.Clone(), df_cellcenter_rgn.Clone(), ho_AllCells_Pt, true, "#7b68ee40")); // (20190624) Jeff Revised!
                }
                if (Recipe.Param.InspParam_BlackBlob_CL.Enabled)
                {
                    if (b_defect)
                    {
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.Union2(ho_Orig_DefectReg, Dict_AOI_absolNG["BlackBlob (同軸光)"].DefectReg, out ExpTmpOutVar_0);
                            ho_Orig_DefectReg.Dispose();
                            ho_Orig_DefectReg = ExpTmpOutVar_0;
                        }

                        if (FinalInspectParam.b_NG_classification) // 多瑕疵
                        {
                            Extension.HObjectMedthods.ReleaseHObject(ref df_cellcenter_rgn);
                            Compute_DefectsClassify_CellCenter(Recipe, Dict_AOI_absolNG["BlackBlob (同軸光)"].DefectReg, out df_cellcenter_rgn);
                        }
                        else if (Recipe.Param.Enabled_Compute_df_CellCenter)
                        {
                            Extension.HObjectMedthods.ReleaseHObject(ref df_cellcenter_rgn);
                            Compute_df_CellCenter(Dict_AOI_absolNG["BlackBlob (同軸光)"].DefectReg, out df_cellcenter_rgn);
                        }

                        /* 計算模稜兩可之NG */
                        if (Recipe.Param.B_AOI_absolNG) // 是否啟用AOI判斷絕對NG
                        {
                            //if (Recipe.Param.InspParam_BlackBlob_CL.cls_absolNG.Enabled)
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.Union2(orig_DefectReg_notSureNG, Dict_AOI_NG["BlackBlob (同軸光)"].DefectReg, out ExpTmpOutVar_0);
                                orig_DefectReg_notSureNG.Dispose();
                                orig_DefectReg_notSureNG = ExpTmpOutVar_0;
                            }
                        }

                    }

                    // 新增瑕疵
                    DefectResultList.Add(new Defect("BlackBlob (同軸光)", insp_ImgList[Recipe.Param.InspParam_BlackBlob_CL.ImageIndex].Clone(), Dict_AOI_absolNG["BlackBlob (同軸光)"].DefectReg.Clone(), df_cellcenter_rgn.Clone(), ho_AllCells_Pt, true, "#ff7f5040")); // (20190624) Jeff Revised!
                }
                if (Recipe.Param.InspParam_BlackBlob2_CL.Enabled)
                {
                    if (b_defect)
                    {
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.Union2(ho_Orig_DefectReg, Dict_AOI_absolNG["BlackBlob2 (同軸光)"].DefectReg, out ExpTmpOutVar_0);
                            ho_Orig_DefectReg.Dispose();
                            ho_Orig_DefectReg = ExpTmpOutVar_0;
                        }

                        if (FinalInspectParam.b_NG_classification) // 多瑕疵
                        {
                            Extension.HObjectMedthods.ReleaseHObject(ref df_cellcenter_rgn);
                            Compute_DefectsClassify_CellCenter(Recipe, Dict_AOI_absolNG["BlackBlob2 (同軸光)"].DefectReg, out df_cellcenter_rgn);
                        }
                        else if (Recipe.Param.Enabled_Compute_df_CellCenter)
                        {
                            Extension.HObjectMedthods.ReleaseHObject(ref df_cellcenter_rgn);
                            Compute_df_CellCenter(Dict_AOI_absolNG["BlackBlob2 (同軸光)"].DefectReg, out df_cellcenter_rgn);
                        }

                        /* 計算模稜兩可之NG */
                        if (Recipe.Param.B_AOI_absolNG) // 是否啟用AOI判斷絕對NG
                        {
                            //if (Recipe.Param.InspParam_BlackBlob2_CL.cls_absolNG.Enabled)
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.Union2(orig_DefectReg_notSureNG, Dict_AOI_NG["BlackBlob2 (同軸光)"].DefectReg, out ExpTmpOutVar_0);
                                orig_DefectReg_notSureNG.Dispose();
                                orig_DefectReg_notSureNG = ExpTmpOutVar_0;
                            }
                        }

                    }

                    // 新增瑕疵
                    DefectResultList.Add(new Defect("BlackBlob2 (同軸光)", insp_ImgList[Recipe.Param.InspParam_BlackBlob2_CL.ImageIndex].Clone(), Dict_AOI_absolNG["BlackBlob2 (同軸光)"].DefectReg.Clone(), df_cellcenter_rgn.Clone(), ho_AllCells_Pt, true, "#00ff7f40")); // (20190624) Jeff Revised!
                }
                if (Recipe.Param.InspParam_Line_FLCL.Enabled)
                {
                    if (b_defect)
                    {
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.Union2(ho_Orig_DefectReg, Dict_AOI_absolNG["絲狀異物 (正光&同軸光)"].DefectReg, out ExpTmpOutVar_0);
                            ho_Orig_DefectReg.Dispose();
                            ho_Orig_DefectReg = ExpTmpOutVar_0;
                        }

                        if (FinalInspectParam.b_NG_classification) // 多瑕疵
                        {
                            Extension.HObjectMedthods.ReleaseHObject(ref df_cellcenter_rgn);
                            Compute_DefectsClassify_CellCenter(Recipe, Dict_AOI_absolNG["絲狀異物 (正光&同軸光)"].DefectReg, out df_cellcenter_rgn);
                        }
                        else if (Recipe.Param.Enabled_Compute_df_CellCenter)
                        {
                            Extension.HObjectMedthods.ReleaseHObject(ref df_cellcenter_rgn);
                            Compute_df_CellCenter(Dict_AOI_absolNG["絲狀異物 (正光&同軸光)"].DefectReg, out df_cellcenter_rgn);
                        }

                        /* 計算模稜兩可之NG */
                        if (Recipe.Param.B_AOI_absolNG) // 是否啟用AOI判斷絕對NG
                        {
                            //if (Recipe.Param.InspParam_Line_FLCL.cls_absolNG.Enabled)
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.Union2(orig_DefectReg_notSureNG, Dict_AOI_NG["絲狀異物 (正光&同軸光)"].DefectReg, out ExpTmpOutVar_0);
                                orig_DefectReg_notSureNG.Dispose();
                                orig_DefectReg_notSureNG = ExpTmpOutVar_0;
                            }
                        }

                    }

                    // 新增瑕疵
                    DefectResultList.Add(new Defect("絲狀異物 (正光&同軸光)", insp_ImgList[Recipe.Param.InspParam_BlackBlob2_CL.ImageIndex].Clone(), Dict_AOI_absolNG["絲狀異物 (正光&同軸光)"].DefectReg.Clone(), df_cellcenter_rgn.Clone(), ho_AllCells_Pt, true, "#ff450040")); // (20190624) Jeff Revised!
                }
                if (Recipe.Param.InspParam_Dirty_CL.Enabled)
                {
                    if (b_defect)
                    {
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.Union2(ho_Orig_DefectReg, Dict_AOI_absolNG["汙染 (同軸光)"].DefectReg, out ExpTmpOutVar_0);
                            ho_Orig_DefectReg.Dispose();
                            ho_Orig_DefectReg = ExpTmpOutVar_0;
                        }

                        if (FinalInspectParam.b_NG_classification) // 多瑕疵
                        {
                            Extension.HObjectMedthods.ReleaseHObject(ref df_cellcenter_rgn);
                            Compute_DefectsClassify_CellCenter(Recipe, Dict_AOI_absolNG["汙染 (同軸光)"].DefectReg, out df_cellcenter_rgn);
                        }
                        else if (Recipe.Param.Enabled_Compute_df_CellCenter)
                        {
                            Extension.HObjectMedthods.ReleaseHObject(ref df_cellcenter_rgn);
                            Compute_df_CellCenter(Dict_AOI_absolNG["汙染 (同軸光)"].DefectReg, out df_cellcenter_rgn);
                        }

                        /* 計算模稜兩可之NG */
                        if (Recipe.Param.B_AOI_absolNG) // 是否啟用AOI判斷絕對NG
                        {
                            //if (Recipe.Param.InspParam_Dirty_CL.cls_absolNG.Enabled)
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.Union2(orig_DefectReg_notSureNG, Dict_AOI_NG["汙染 (同軸光)"].DefectReg, out ExpTmpOutVar_0);
                                orig_DefectReg_notSureNG.Dispose();
                                orig_DefectReg_notSureNG = ExpTmpOutVar_0;
                            }
                        }

                    }

                    // 新增瑕疵
                    DefectResultList.Add(new Defect("汙染 (同軸光)", insp_ImgList[Recipe.Param.InspParam_Dirty_CL.ImageIndex].Clone(), Dict_AOI_absolNG["汙染 (同軸光)"].DefectReg.Clone(), df_cellcenter_rgn.Clone(), ho_AllCells_Pt, true, "#556b2f40")); // (20190624) Jeff Revised!
                }
                if (Recipe.Param.InspParam_Dirty_FLCL.Enabled)
                {
                    if (b_defect)
                    {
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.Union2(ho_Orig_DefectReg, Dict_AOI_absolNG["汙染 (正光&同軸光)"].DefectReg, out ExpTmpOutVar_0);
                            ho_Orig_DefectReg.Dispose();
                            ho_Orig_DefectReg = ExpTmpOutVar_0;
                        }

                        if (FinalInspectParam.b_NG_classification) // 多瑕疵
                        {
                            Extension.HObjectMedthods.ReleaseHObject(ref df_cellcenter_rgn);
                            Compute_DefectsClassify_CellCenter(Recipe, Dict_AOI_absolNG["汙染 (正光&同軸光)"].DefectReg, out df_cellcenter_rgn);
                        }
                        else if (Recipe.Param.Enabled_Compute_df_CellCenter)
                        {
                            Extension.HObjectMedthods.ReleaseHObject(ref df_cellcenter_rgn);
                            Compute_df_CellCenter(Dict_AOI_absolNG["汙染 (正光&同軸光)"].DefectReg, out df_cellcenter_rgn);
                        }

                        /* 計算模稜兩可之NG */
                        if (Recipe.Param.B_AOI_absolNG) // 是否啟用AOI判斷絕對NG
                        {
                            //if (Recipe.Param.InspParam_Dirty_FLCL.cls_absolNG.Enabled)
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.Union2(orig_DefectReg_notSureNG, Dict_AOI_NG["汙染 (正光&同軸光)"].DefectReg, out ExpTmpOutVar_0);
                                orig_DefectReg_notSureNG.Dispose();
                                orig_DefectReg_notSureNG = ExpTmpOutVar_0;
                            }
                        }

                    }

                    // 新增瑕疵
                    DefectResultList.Add(new Defect("汙染 (正光&同軸光)", insp_ImgList[Recipe.Param.InspParam_Dirty_FLCL.ImageIndex1].Clone(), Dict_AOI_absolNG["汙染 (正光&同軸光)"].DefectReg.Clone(), df_cellcenter_rgn.Clone(), ho_AllCells_Pt, true, "#ffc0cb40")); // (20190624) Jeff Revised!
                }
                if (Recipe.Param.InspParam_BrightBlob_BL.Enabled)
                {
                    if (b_defect)
                    {
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.Union2(ho_Orig_DefectReg, Dict_AOI_absolNG["電極區白點 (背光)"].DefectReg, out ExpTmpOutVar_0);
                            ho_Orig_DefectReg.Dispose();
                            ho_Orig_DefectReg = ExpTmpOutVar_0;
                        }

                        if (FinalInspectParam.b_NG_classification) // 多瑕疵
                        {
                            Extension.HObjectMedthods.ReleaseHObject(ref df_cellcenter_rgn);
                            Compute_DefectsClassify_CellCenter(Recipe, Dict_AOI_absolNG["電極區白點 (背光)"].DefectReg, out df_cellcenter_rgn);
                        }
                        else if (Recipe.Param.Enabled_Compute_df_CellCenter)
                        {
                            Extension.HObjectMedthods.ReleaseHObject(ref df_cellcenter_rgn);
                            Compute_df_CellCenter(Dict_AOI_absolNG["電極區白點 (背光)"].DefectReg, out df_cellcenter_rgn);
                        }

                        /* 計算模稜兩可之NG */
                        if (Recipe.Param.B_AOI_absolNG) // 是否啟用AOI判斷絕對NG
                        {
                            //if (Recipe.Param.InspParam_BrightBlob_BL.cls_absolNG.Enabled)
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.Union2(orig_DefectReg_notSureNG, Dict_AOI_NG["電極區白點 (背光)"].DefectReg, out ExpTmpOutVar_0);
                                orig_DefectReg_notSureNG.Dispose();
                                orig_DefectReg_notSureNG = ExpTmpOutVar_0;
                            }
                        }

                    }

                    // 新增瑕疵
                    DefectResultList.Add(new Defect("電極區白點 (背光)", insp_ImgList[Recipe.Param.InspParam_BrightBlob_BL.ImageIndex].Clone(), Dict_AOI_absolNG["電極區白點 (背光)"].DefectReg.Clone(), df_cellcenter_rgn.Clone(), ho_AllCells_Pt, true, "#5f9ea040")); // (20190624) Jeff Revised!
                }
                if (Recipe.Param.InspParam_LackAngle_BL.Enabled)
                {
                    if (b_defect)
                    {
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.Union2(ho_Orig_DefectReg, Dict_AOI_absolNG["保缺角 (背光)"].DefectReg, out ExpTmpOutVar_0);
                            ho_Orig_DefectReg.Dispose();
                            ho_Orig_DefectReg = ExpTmpOutVar_0;
                        }

                        if (FinalInspectParam.b_NG_classification) // 多瑕疵
                        {
                            Extension.HObjectMedthods.ReleaseHObject(ref df_cellcenter_rgn);
                            Compute_DefectsClassify_CellCenter(Recipe, Dict_AOI_absolNG["保缺角 (背光)"].DefectReg, out df_cellcenter_rgn);
                        }
                        else if (Recipe.Param.Enabled_Compute_df_CellCenter)
                        {
                            Extension.HObjectMedthods.ReleaseHObject(ref df_cellcenter_rgn);
                            Compute_df_CellCenter(Dict_AOI_absolNG["保缺角 (背光)"].DefectReg, out df_cellcenter_rgn);
                        }

                        /* 計算模稜兩可之NG */
                        if (Recipe.Param.B_AOI_absolNG) // 是否啟用AOI判斷絕對NG
                        {
                            //if (Recipe.Param.InspParam_LackAngle_BL.cls_absolNG.Enabled)
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.Union2(orig_DefectReg_notSureNG, Dict_AOI_NG["保缺角 (背光)"].DefectReg, out ExpTmpOutVar_0);
                                orig_DefectReg_notSureNG.Dispose();
                                orig_DefectReg_notSureNG = ExpTmpOutVar_0;
                            }
                        }

                    }

                    // 新增瑕疵
                    DefectResultList.Add(new Defect("保缺角 (背光)", insp_ImgList[Recipe.Param.InspParam_LackAngle_BL.ImageIndex].Clone(), Dict_AOI_absolNG["保缺角 (背光)"].DefectReg.Clone(), df_cellcenter_rgn.Clone(), ho_AllCells_Pt, true, "#daa52040")); // (20190624) Jeff Revised!
                }
                if (Recipe.Param.InspParam_LackAngle_SL.Enabled)
                {
                    if (b_defect)
                    {
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.Union2(ho_Orig_DefectReg, Dict_AOI_absolNG["保缺角 (側光)"].DefectReg, out ExpTmpOutVar_0);
                            ho_Orig_DefectReg.Dispose();
                            ho_Orig_DefectReg = ExpTmpOutVar_0;
                        }

                        if (FinalInspectParam.b_NG_classification) // 多瑕疵
                        {
                            Extension.HObjectMedthods.ReleaseHObject(ref df_cellcenter_rgn);
                            Compute_DefectsClassify_CellCenter(Recipe, Dict_AOI_absolNG["保缺角 (側光)"].DefectReg, out df_cellcenter_rgn);
                        }
                        else if (Recipe.Param.Enabled_Compute_df_CellCenter)
                        {
                            Extension.HObjectMedthods.ReleaseHObject(ref df_cellcenter_rgn);
                            Compute_df_CellCenter(Dict_AOI_absolNG["保缺角 (側光)"].DefectReg, out df_cellcenter_rgn);
                        }

                        /* 計算模稜兩可之NG */
                        if (Recipe.Param.B_AOI_absolNG) // 是否啟用AOI判斷絕對NG
                        {
                            //if (Recipe.Param.InspParam_LackAngle_SL.cls_absolNG.Enabled)
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.Union2(orig_DefectReg_notSureNG, Dict_AOI_NG["保缺角 (側光)"].DefectReg, out ExpTmpOutVar_0);
                                orig_DefectReg_notSureNG.Dispose();
                                orig_DefectReg_notSureNG = ExpTmpOutVar_0;
                            }
                        }

                    }

                    // 新增瑕疵
                    DefectResultList.Add(new Defect("保缺角 (側光)", insp_ImgList[Recipe.Param.InspParam_LackAngle_SL.ImageIndex].Clone(), Dict_AOI_absolNG["保缺角 (側光)"].DefectReg.Clone(), df_cellcenter_rgn.Clone(), ho_AllCells_Pt, true, "#40e0d040")); // (20190624) Jeff Revised!
                }

                #endregion

                Res = true;
            }
            catch (Exception ex)
            {
                Log.WriteLog("Error: Add_DefectResultList() 執行發生例外狀況 -> " + ex.ToString());
                return false;
            }

            return Res;
        }

        /// <summary>
        /// 計算單一類型瑕疵所在Cell中心 (旋轉瑕疵region回原本方向)
        /// </summary>
        /// <param name="DefectReg"></param>
        /// <param name="Defect_CellCenter"></param>
        public void Compute_DefectsClassify_CellCenter(ResistPanelRole Recipe, HObject DefectReg, out HObject Defect_CellCenter) // (20190717) Jeff Revised!
        {
            HObject DefectReg_InspReg;

            // 影像檢測範圍
            if (Recipe.Param.InspParam_Df_CellReg.Enabled_InspReg)
                HOperatorSet.Intersection(ho_Region_InspReg, DefectReg, out DefectReg_InspReg);
            else
                DefectReg_InspReg = DefectReg.Clone();

            // 判定瑕疵cell
            Compute_df_CellCenter(DefectReg_InspReg, out Defect_CellCenter);

            // 旋轉瑕疵region回原本方向
            if (Recipe.Param.Enabled_rotate)
            {
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.AffineTransRegion(Defect_CellCenter, out ExpTmpOutVar_0, hommat2d_origin_, "nearest_neighbor");
                    Defect_CellCenter.Dispose();
                    Defect_CellCenter = ExpTmpOutVar_0;

                }
            }

            DefectReg_InspReg.Dispose();
        }

        public void LocalDefectMappping(HObject ho_DefectRegions, HObject SelectRegions, out HObject ho_DefectRegionMarks)
        {
            // Local iconic variables
            HObject ho_SegmentRegions_rec1_dl;
            HObject ho_RegionIntersection;

            // Local control variables 
            HTuple hv_aValue = null, hv_Greater = null, hv_Indices = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_DefectRegionMarks);
            HOperatorSet.GenEmptyObj(out ho_SegmentRegions_rec1_dl);
            HOperatorSet.GenEmptyObj(out ho_RegionIntersection);
            try
            {
                //*******************************************************************************************************************
                //計算瑕疵位於的Region範圍
                //*******************************************************************************************************************
                ho_SegmentRegions_rec1_dl.Dispose();
                ho_RegionIntersection.Dispose();
                HOperatorSet.Intersection(SelectRegions, ho_DefectRegions, out ho_RegionIntersection);
                HOperatorSet.RegionFeatures(ho_RegionIntersection, "area", out hv_aValue);
                HOperatorSet.TupleGreaterEqualElem(hv_aValue, 1, out hv_Greater);
                HOperatorSet.TupleFind(hv_Greater, 1, out hv_Indices);

                ho_DefectRegionMarks.Dispose();
                HOperatorSet.GenEmptyObj(out ho_DefectRegionMarks);
                if ((int)(new HTuple(hv_Indices.TupleNotEqual(-1))) != 0)
                {
                    ho_DefectRegionMarks.Dispose();
                    HOperatorSet.SelectObj(SelectRegions, out ho_DefectRegionMarks, hv_Indices + 1);
                }

                //*******************************************************************************************************************
                ho_SegmentRegions_rec1_dl.Dispose();
                ho_RegionIntersection.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_SegmentRegions_rec1_dl.Dispose();
                ho_RegionIntersection.Dispose();

                throw HDevExpDefaultException;
            }
        }

        /// <summary>
        /// 計算瑕疵Cell座標
        /// </summary>
        /// <param name="DefectReg"></param>
        /// <param name="Defect_CellCenter"></param>
        /// <returns></returns>
        public bool Compute_df_CellCenter(HObject DefectReg, out HObject Defect_CellCenter)
        {
            bool Res = false;
            HOperatorSet.GenEmptyObj(out Defect_CellCenter);

            try
            {
                HTuple hv_Index_defect = new HTuple();
                int Method = 3;
                if (Method == 1) // Method 1
                {
                    HObject ho_RegDif = null, ho_SelectRegions = null, ho_RegTrans = null, ho_RegTrans_Union = null;
                    HObject ho_RegDif2 = null, ho_SelectRegions2 = null, ho_RegTrans2 = null, ho_RegTrans2_Union = null;
                    HOperatorSet.Difference(ho_InspRects_AllCells, DefectReg, out ho_RegDif);
                    //HOperatorSet.SelectShape(ho_RegDif, out ho_SelectRegions, "area", "and", hv_MinArea_cell, hv_MaxArea_cell);
                    HOperatorSet.SelectShape(ho_RegDif, out ho_SelectRegions, "area", "and", hv_MaxArea_cell, hv_MaxArea_cell);
                    HOperatorSet.ShapeTrans(ho_SelectRegions, out ho_RegTrans, "inner_center");
                    HOperatorSet.Union1(ho_RegTrans, out ho_RegTrans_Union);
                    HOperatorSet.Difference(ho_InspRects_AllCells, ho_RegTrans_Union, out ho_RegDif2);
                    //HOperatorSet.SelectShape(ho_RegDif2, out ho_SelectRegions2, "area", "and", hv_MinArea_cell, hv_MaxArea_cell);
                    HOperatorSet.SelectShape(ho_RegDif2, out ho_SelectRegions2, "area", "and", hv_MaxArea_cell, hv_MaxArea_cell);
                    HOperatorSet.ShapeTrans(ho_SelectRegions2, out ho_RegTrans2, "inner_center");
                    HOperatorSet.Union1(ho_RegTrans2, out ho_RegTrans2_Union);
                    HTuple hv_Rows = new HTuple(), hv_Columns = new HTuple();
                    HOperatorSet.GetRegionPoints(ho_RegTrans2_Union, out hv_Rows, out hv_Columns);
                    HTuple hv_i = new HTuple(), hv_Index = new HTuple();
                    for (hv_i = 0; (int)hv_i <= (int)((new HTuple(hv_Rows.TupleLength())) - 1); hv_i = (int)hv_i + 1)
                    {
                        HOperatorSet.GetRegionIndex(ho_AllCells_Rect, hv_Rows.TupleSelect(hv_i), hv_Columns.TupleSelect(hv_i), out hv_Index);
                        HOperatorSet.TupleConcat(hv_Index_defect, hv_Index, out hv_Index_defect);
                    }
                }
                else if (Method == 2) // Method 2
                {
                    HObject ho_NotDefectReg_all = null, ho_InspRects_Defect = null, ho_InspRects_AllCells_ImageBorder = null;
                    HTuple hv_A_CellDefect = new HTuple(), hv_R_CellDefect = new HTuple(), hv_C_CellDefect = new HTuple(), hv_Sequence = new HTuple();
                    HOperatorSet.Difference(ho_Region_ImageBorder, DefectReg, out ho_NotDefectReg_all);
                    HOperatorSet.Intersection(ho_InspRects_AllCells, ho_Region_ImageBorder, out ho_InspRects_AllCells_ImageBorder);
                    HOperatorSet.Difference(ho_InspRects_AllCells_ImageBorder, ho_NotDefectReg_all, out ho_InspRects_Defect);
                    HOperatorSet.AreaCenter(ho_InspRects_Defect, out hv_A_CellDefect, out hv_R_CellDefect, out hv_C_CellDefect);
                    HOperatorSet.TupleGenSequence(1, new HTuple(hv_A_CellDefect.TupleLength()), 1, out hv_Sequence);
                    HOperatorSet.TupleSelectMask(hv_Sequence, hv_A_CellDefect, out hv_Index_defect);
                }
                else if (Method == 3) // Method 3 (20190325) Jeff Revised!
                {
                    HObject ho_InspRects_Defect = null;
                    HTuple hv_area_InspRects_Defect = new HTuple(), hv_Greater = new HTuple();
                    HOperatorSet.Intersection(ho_InspRects_AllCells, DefectReg, out ho_InspRects_Defect);
                    HOperatorSet.RegionFeatures(ho_InspRects_Defect, "area", out hv_area_InspRects_Defect);
                    HOperatorSet.TupleGreaterElem(hv_area_InspRects_Defect, 0.0, out hv_Greater);
                    HOperatorSet.TupleFind(hv_Greater, 1, out hv_Index_defect);

                    if ((int)(new HTuple(hv_Index_defect.TupleNotEqual(-1))) == 0)
                        return true;

                    hv_Index_defect = hv_Index_defect + 1;
                }

                HOperatorSet.SelectObj(ho_AllCells_Pt, out Defect_CellCenter, hv_Index_defect);
                
                Res = true;
            }
            catch
            {
                Log.WriteLog("Error: 計算瑕疵Cell座標 失敗!");
            }

            return Res;
        }

        public void Convert2Circle(HObject Regions, out HObject CircleRegions, int radius = 3) // (20190806) Jeff Revised!
        {
            HOperatorSet.GenEmptyObj(out CircleRegions);
            HTuple area_, row_, col_, hv_Number;
            HTuple hv_Newtuple = null;

            HOperatorSet.AreaCenter(Regions, out area_, out row_, out col_);
            HOperatorSet.CountObj(Regions, out hv_Number);
            if (hv_Number > 0)
            {
                HOperatorSet.TupleGenConst(hv_Number, radius, out hv_Newtuple);
                CircleRegions.Dispose();
                HOperatorSet.GenCircle(out CircleRegions, row_, col_, hv_Newtuple);
            }
        }

        /// <summary>
        /// 讀取影像
        /// </summary>
        /// <param name="Recipe"></param>
        /// <param name="ho_ImgList"></param>
        /// <returns></returns>
        public bool Read_AllImages_Insp(ResistPanelRole Recipe, List<HObject> ho_ImgList)
        {
            bool Res = false;

            try
            {
                /* 讀取影像 Light1: FL (Frontal Light), Light2: CL (Coaxial Light), Light3: BL (Back Light) & SL (Side Light) */
                //HOperatorSet.CopyImage(ho_ImgList[0], out ho_image_FL);
                //HOperatorSet.CopyImage(ho_ImgList[1], out ho_image_CL);
                //HOperatorSet.CopyImage(ho_ImgList[2], out ho_image_BL);
                //if (ho_ImgList.Count == 4)
                //    HOperatorSet.CopyImage(ho_ImgList[3], out ho_image_SL);

                // 影像轉正
                if (Recipe.Param.Enabled_rotate)
                {
                    HTuple hommat2d_affine_;
                    HOperatorSet.HomMat2dIdentity(out hommat2d_affine_);
                    HOperatorSet.HomMat2dRotate(hommat2d_affine_, Affine_angle_degree.TupleRad(), 0, 0, out hommat2d_affine_);
                    //HOperatorSet.AffineTransImage(ho_image_FL.Clone(), out ho_image_FL, hommat2d_affine_, "constant", "false");
                    //HOperatorSet.AffineTransImage(ho_image_CL.Clone(), out ho_image_CL, hommat2d_affine_, "constant", "false");
                    //HOperatorSet.AffineTransImage(ho_image_BL.Clone(), out ho_image_BL, hommat2d_affine_, "constant", "false");
                    //if (ho_ImgList.Count == 4)
                    //    HOperatorSet.AffineTransImage(ho_image_SL.Clone(), out ho_image_SL, hommat2d_affine_, "constant", "false");
                    HOperatorSet.AffineTransImage(ho_ImgList[0], out ho_image_FL, hommat2d_affine_, "constant", "false");
                    HOperatorSet.AffineTransImage(ho_ImgList[1], out ho_image_CL, hommat2d_affine_, "constant", "false");
                    HOperatorSet.AffineTransImage(ho_ImgList[2], out ho_image_BL, hommat2d_affine_, "constant", "false");
                    //HOperatorSet.AffineTransImage(ho_ImgList[0], out ho_image_FL, hommat2d_affine_, "weighted", "false");
                    //HOperatorSet.AffineTransImage(ho_ImgList[1], out ho_image_CL, hommat2d_affine_, "weighted", "false");
                    //HOperatorSet.AffineTransImage(ho_ImgList[2], out ho_image_BL, hommat2d_affine_, "weighted", "false");
                    //HOperatorSet.RotateImage(ho_ImgList[0], out ho_image_FL, Affine_angle_degree, "constant");
                    //HOperatorSet.RotateImage(ho_ImgList[1], out ho_image_CL, Affine_angle_degree, "constant");
                    //HOperatorSet.RotateImage(ho_ImgList[2], out ho_image_BL, Affine_angle_degree, "constant");
                    if (ho_ImgList.Count == 4)
                        HOperatorSet.AffineTransImage(ho_ImgList[3], out ho_image_SL, hommat2d_affine_, "constant", "false");
                }
                else
                {
                    HOperatorSet.CopyImage(ho_ImgList[0], out ho_image_FL);
                    HOperatorSet.CopyImage(ho_ImgList[1], out ho_image_CL);
                    HOperatorSet.CopyImage(ho_ImgList[2], out ho_image_BL);
                    if (ho_ImgList.Count == 4)
                        HOperatorSet.CopyImage(ho_ImgList[3], out ho_image_SL);
                }

                HOperatorSet.Rgb1ToGray(ho_image_FL, out ho_GrayImage_FL);
                HOperatorSet.Rgb1ToGray(ho_image_CL, out ho_GrayImage_CL);
                HOperatorSet.Rgb1ToGray(ho_image_BL, out ho_GrayImage_BL);
                if (ho_ImgList.Count == 4)
                    HOperatorSet.Rgb1ToGray(ho_image_SL, out ho_GrayImage_SL);
                HOperatorSet.GetImageSize(ho_GrayImage_FL, out hv_Width, out hv_Height);

                // Copy to insp_ImgList & insp_GrayImgList
                for (int i = 0; i < insp_ImgList.Count; i++)
                {
                    if (insp_ImgList[i] != null)
                    {
                        insp_ImgList[i].Dispose();
                        insp_ImgList[i] = null;
                    }
                }
                insp_ImgList.Clear();
                //insp_ImgList.Add(ho_image_FL.Clone());
                //insp_ImgList.Add(ho_image_CL.Clone());
                //insp_ImgList.Add(ho_image_BL.Clone());
                //if (ho_ImgList.Count == 4)
                //    insp_ImgList.Add(ho_image_SL.Clone());
                insp_ImgList.Add(ho_image_FL);
                insp_ImgList.Add(ho_image_CL);
                insp_ImgList.Add(ho_image_BL);
                if (ho_ImgList.Count == 4)
                    insp_ImgList.Add(ho_image_SL);

                for (int i = 0; i < insp_GrayImgList.Count; i++)
                {
                    if (insp_GrayImgList[i] != null)
                    {
                        insp_GrayImgList[i].Dispose();
                        insp_GrayImgList[i] = null;
                    }
                }
                insp_GrayImgList.Clear();
                //insp_GrayImgList.Add(ho_GrayImage_FL.Clone());
                //insp_GrayImgList.Add(ho_GrayImage_CL.Clone());
                //insp_GrayImgList.Add(ho_GrayImage_BL.Clone());
                //if (ho_ImgList.Count == 4)
                //    insp_GrayImgList.Add(ho_GrayImage_SL.Clone());
                insp_GrayImgList.Add(ho_GrayImage_FL);
                insp_GrayImgList.Add(ho_GrayImage_CL);
                insp_GrayImgList.Add(ho_GrayImage_BL);
                if (ho_ImgList.Count == 4)
                    insp_GrayImgList.Add(ho_GrayImage_SL);

                // 取得 影像邊界區域
                HOperatorSet.GenRectangle1(out ho_Region_ImageBorder, 0, 0, hv_Height - 1, hv_Width - 1);

                Res = true;
            }
            catch (Exception ex)
            {
                Log.WriteLog("Error: Read_AllImages_Insp() 執行發生例外狀況 -> " + ex.ToString());
                return false;
            }

            return Res;
        }

        /// <summary>
        /// 初定位
        /// </summary>
        /// <param name="Recipe"></param>
        /// <returns></returns>
        public bool InitialPosition_Insp(ResistPanelRole Recipe) // (20191112) Jeff Revised!
        {
            bool Res = false;

            try
            {
                /* 擷取有效檢測區域 (For Frontal Light Image & Back Light Image) */
                // 白色區域之外接矩形 (For Frontal Light Image)
                HOperatorSet.Threshold(ho_GrayImage_FL, out ho_Regions_White_thresh_FL, Recipe.Param.PositionParam_Initial.MinGray_WhiteReg_FL, 255);
                if (Recipe.Param.PositionParam_Initial.Enabled_closing_ValidReg_FL) // (20191112) Jeff Revised!
                {
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ClosingRectangle1(ho_Regions_White_thresh_FL, out ExpTmpOutVar_0, Recipe.Param.PositionParam_Initial.W_closing_ValidReg_FL, Recipe.Param.PositionParam_Initial.H_closing_ValidReg_FL);
                        ho_Regions_White_thresh_FL.Dispose();
                        ho_Regions_White_thresh_FL = ExpTmpOutVar_0;
                    }
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Connection(ho_Regions_White_thresh_FL, out ExpTmpOutVar_0);
                    ho_Regions_White_thresh_FL.Dispose();
                    ho_Regions_White_thresh_FL = ExpTmpOutVar_0;
                }
                HOperatorSet.SelectShape(ho_Regions_White_thresh_FL, out ho_Regions_White_select, "area", "and", Recipe.Param.PositionParam_Initial.MinArea_Inspect, hv_Width * hv_Height);
                HOperatorSet.CountObj(ho_Regions_White_select, out hv_Num_WhiteRegion);
                if ((int)(new HTuple(hv_Num_WhiteRegion.TupleNotEqual(0))) != 0)
                {
                    HOperatorSet.OpeningRectangle1(ho_Regions_White_select, out ho_Regions_White_opening, Recipe.Param.PositionParam_Initial.W_opening_ValidReg_FL, Recipe.Param.PositionParam_Initial.H_opening_ValidReg_FL); // (20191112) Jeff Revised!
                    HOperatorSet.ShapeTrans(ho_Regions_White_opening, out ho_Regions_White, "rectangle1");
                }
                else
                    HOperatorSet.Threshold(ho_GrayImage_FL, out ho_Regions_White, 0, 255);

                #region 擷取有效檢測區域 (20191112) Jeff Revised!

                HObject ho_ValidReg_Type = null;
                if (Recipe.Param.PositionParam_Initial.str_Type_ValidReg == "BL")
                {
                    // 擷取有效檢測區域 (For Back Light Image)
                    HOperatorSet.Threshold(ho_GrayImage_BL, out ho_Regions_Inspect_thresh_BL, 0, Recipe.Param.PositionParam_Initial.MaxGray_InspectReg_BL);
                    HOperatorSet.FillUp(ho_Regions_Inspect_thresh_BL, out ho_Regions_Inspect_fill);
                    // 消除三角形誤判
                    HOperatorSet.OpeningRectangle1(ho_Regions_Inspect_fill, out ho_ValidReg_Type, Recipe.Param.PositionParam_Initial.W_opening_ValidReg_BL, Recipe.Param.PositionParam_Initial.H_opening_ValidReg_BL); // (20191112) Jeff Revised!
                }
                else if (Recipe.Param.PositionParam_Initial.str_Type_ValidReg == "FL")
                {
                    // 擷取有效檢測區域 (For Frontal Light Image)
                    HOperatorSet.Threshold(ho_GrayImage_FL, out ho_Regions_Inspect_thresh_BL, 0, Recipe.Param.PositionParam_Initial.MaxGray_InspectReg_BL);
                    // 消除誤判
                    HOperatorSet.OpeningRectangle1(ho_Regions_Inspect_thresh_BL, out ho_Regions_Inspect_opening, Recipe.Param.PositionParam_Initial.W_opening_ValidReg_BL, Recipe.Param.PositionParam_Initial.H_opening_ValidReg_BL);
                    //  尺寸篩選
                    HObject ho_Regions_Inspect_opening_con, ho_ValidReg_select, ho_ValidReg_union;
                    HOperatorSet.Connection(ho_Regions_Inspect_opening, out ho_Regions_Inspect_opening_con);
                    // int 轉 HTuple
                    hv_MinW_BlackStripe = Recipe.Param.PositionParam_Initial.MinW_BlackStripe_ValidReg;
                    hv_MaxW_BlackStripe = Recipe.Param.PositionParam_Initial.MaxW_BlackStripe_ValidReg;
                    hv_MinH_BlackStripe = Recipe.Param.PositionParam_Initial.MinH_BlackStripe_ValidReg;
                    hv_MaxH_BlackStripe = Recipe.Param.PositionParam_Initial.MaxH_BlackStripe_ValidReg;
                    HOperatorSet.SelectShape(ho_Regions_Inspect_opening_con, out ho_ValidReg_select,
                                             (new HTuple("width")).TupleConcat("height"), "and",
                                             hv_MinW_BlackStripe.TupleConcat(hv_MinH_BlackStripe), hv_MaxW_BlackStripe.TupleConcat(hv_MaxH_BlackStripe));
                    HOperatorSet.Union1(ho_ValidReg_select, out ho_ValidReg_union);
                    // Morphology
                    HOperatorSet.DilationRectangle1(ho_ValidReg_union, out ho_ValidReg_Type, Recipe.Param.PositionParam_Initial.W_dila_ValidReg, Recipe.Param.PositionParam_Initial.H_dila_ValidReg);
                    ho_Regions_Inspect_opening_con.Dispose();
                    ho_ValidReg_select.Dispose();
                    ho_ValidReg_union.Dispose();
                }

                HOperatorSet.Intersection(ho_ValidReg_Type, ho_Regions_White, out ho_Regions_Inspect_IntSec);
                ho_ValidReg_Type.Dispose();
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Connection(ho_Regions_Inspect_IntSec, out ExpTmpOutVar_0);
                    ho_Regions_Inspect_IntSec.Dispose();
                    ho_Regions_Inspect_IntSec = ExpTmpOutVar_0;
                }
                HOperatorSet.SelectShape(ho_Regions_Inspect_IntSec, out ho_Regions_Inspect_select, "area", "and", Recipe.Param.PositionParam_Initial.MinArea_Inspect, hv_Width * hv_Height);
                HOperatorSet.ShapeTrans(ho_Regions_Inspect_select, out ho_Regions_Inspect_orig, "rectangle1");

                #endregion

                //消除掉非檢測Cell (黑色長條邊緣)
                if (Recipe.Param.PositionParam_Initial.Enabled_NonInspectCell)
                {
                    HOperatorSet.CountObj(ho_Regions_Inspect_orig, out hv_Num_InspectRegions);
                    HOperatorSet.GenEmptyRegion(out ho_Regions_Inspect0);
                    HOperatorSet.GenEmptyRegion(out ho_Regions_Inspect0_Wsmall);
                    HOperatorSet.GenEmptyRegion(out ho_Regions_Inspect0_Wsmall3);
                    for (int i_InspectRegions = 1; i_InspectRegions <= hv_Num_InspectRegions; i_InspectRegions++)
                    {
                        HOperatorSet.SelectObj(ho_Regions_Inspect_orig, out ho_ObjectSelected, i_InspectRegions);
                        HOperatorSet.RegionFeatures(ho_ObjectSelected, (((new HTuple("row1")).TupleConcat("row2")).TupleConcat("column1")).TupleConcat("column2"), out hv_Value_R12C12);

                        // double 轉 HTuple
                        hv_L_NonInspect = Recipe.Param.PositionParam_Initial.L_NonInspect / Locate_Method_FS.pixel_resolution_;
                        hv_L_NonInspect_Wsmall = Recipe.Param.PositionParam_Initial.L_NonInspect_Wsmall / Locate_Method_FS.pixel_resolution_;
                        hv_L_NonInspect_Wsmall3 = Recipe.Param.PositionParam_Initial.L_NonInspect_Wsmall3 / Locate_Method_FS.pixel_resolution_;

                        // (20191112) Jeff Revised!
                        if (Recipe.Param.Type_BlackStripe == "Horizontal") // 黑條方向水平
                        {
                            if ((int)((new HTuple((new HTuple((((hv_Value_R12C12.TupleSelect(3)) - hv_L_NonInspect)).TupleLessEqual(hv_Value_R12C12.TupleSelect(2)))).TupleOr(new HTuple((((hv_Value_R12C12.TupleSelect(3)) - hv_L_NonInspect_Wsmall)).TupleLessEqual(hv_Value_R12C12.TupleSelect(2)))))).TupleOr(new HTuple((((hv_Value_R12C12.TupleSelect(3)) - hv_L_NonInspect_Wsmall3)).TupleLessEqual(hv_Value_R12C12.TupleSelect(2))))) != 0)
                            {
                                /* 初定位失敗 */
                                return false;
                            }

                            double gap = 2.0; // 影像旋轉後，和原先影像邊界之誤差 (pixel) (20190815) Jeff Revised!

                            //if ((int)((new HTuple(((hv_Value_R12C12.TupleSelect(2))).TupleNotEqual(0))).TupleAnd(new HTuple(((hv_Value_R12C12.TupleSelect(3))).TupleNotEqual(hv_Width - 1)))) != 0)
                            //if (hv_Value_R12C12.DArr[2] != 0.0 && hv_Value_R12C12.DArr[3] != (hv_Width - 1))
                            if (Math.Abs(hv_Value_R12C12.DArr[2] - 0) > gap && Math.Abs(hv_Value_R12C12.DArr[3] - (hv_Width.D - 1)) > gap) // (20190815) Jeff Revised!
                            {
                                HOperatorSet.GenRectangle1(out ho_Regions_temp, hv_Value_R12C12.TupleSelect(0), (hv_Value_R12C12.TupleSelect(2)) + hv_L_NonInspect, hv_Value_R12C12.TupleSelect(1), (hv_Value_R12C12.TupleSelect(3)) - hv_L_NonInspect);
                                HOperatorSet.GenRectangle1(out ho_Regions_temp_Wsmall, hv_Value_R12C12.TupleSelect(0), (hv_Value_R12C12.TupleSelect(2)) + hv_L_NonInspect_Wsmall, hv_Value_R12C12.TupleSelect(1), (hv_Value_R12C12.TupleSelect(3)) - hv_L_NonInspect_Wsmall);
                                HOperatorSet.GenRectangle1(out ho_Regions_temp_Wsmall3, hv_Value_R12C12.TupleSelect(0), (hv_Value_R12C12.TupleSelect(2)) + hv_L_NonInspect_Wsmall3, hv_Value_R12C12.TupleSelect(1), (hv_Value_R12C12.TupleSelect(3)) - hv_L_NonInspect_Wsmall3);
                            }
                            //else if ((int)(new HTuple(((hv_Value_R12C12.TupleSelect(2))).TupleNotEqual(0))) != 0)
                            //else if (hv_Value_R12C12.DArr[2] != 0.0)
                            else if (Math.Abs(hv_Value_R12C12.DArr[2] - 0) > gap) // (20190815) Jeff Revised!
                            {
                                HOperatorSet.GenRectangle1(out ho_Regions_temp, hv_Value_R12C12.TupleSelect(0), (hv_Value_R12C12.TupleSelect(2)) + hv_L_NonInspect, hv_Value_R12C12.TupleSelect(1), hv_Value_R12C12.TupleSelect(3));
                                HOperatorSet.GenRectangle1(out ho_Regions_temp_Wsmall, hv_Value_R12C12.TupleSelect(0), (hv_Value_R12C12.TupleSelect(2)) + hv_L_NonInspect_Wsmall, hv_Value_R12C12.TupleSelect(1), hv_Value_R12C12.TupleSelect(3));
                                HOperatorSet.GenRectangle1(out ho_Regions_temp_Wsmall3, hv_Value_R12C12.TupleSelect(0), (hv_Value_R12C12.TupleSelect(2)) + hv_L_NonInspect_Wsmall3, hv_Value_R12C12.TupleSelect(1), hv_Value_R12C12.TupleSelect(3));
                            }
                            //else if ((int)(new HTuple(((hv_Value_R12C12.TupleSelect(3))).TupleNotEqual(hv_Width - 1))) != 0)
                            //else if (hv_Value_R12C12.DArr[3] != (hv_Width - 1))
                            else if (Math.Abs(hv_Value_R12C12.DArr[3] - (hv_Width.D - 1)) > gap) // (20190815) Jeff Revised!
                            {
                                HOperatorSet.GenRectangle1(out ho_Regions_temp, hv_Value_R12C12.TupleSelect(0), hv_Value_R12C12.TupleSelect(2), hv_Value_R12C12.TupleSelect(1), (hv_Value_R12C12.TupleSelect(3)) - hv_L_NonInspect);
                                HOperatorSet.GenRectangle1(out ho_Regions_temp_Wsmall, hv_Value_R12C12.TupleSelect(0), hv_Value_R12C12.TupleSelect(2), hv_Value_R12C12.TupleSelect(1), (hv_Value_R12C12.TupleSelect(3)) - hv_L_NonInspect_Wsmall);
                                HOperatorSet.GenRectangle1(out ho_Regions_temp_Wsmall3, hv_Value_R12C12.TupleSelect(0), hv_Value_R12C12.TupleSelect(2), hv_Value_R12C12.TupleSelect(1), (hv_Value_R12C12.TupleSelect(3)) - hv_L_NonInspect_Wsmall3);
                            }
                            else
                            {
                                HOperatorSet.GenRectangle1(out ho_Regions_temp, hv_Value_R12C12.TupleSelect(0), hv_Value_R12C12.TupleSelect(2), hv_Value_R12C12.TupleSelect(1), hv_Value_R12C12.TupleSelect(3));
                                HOperatorSet.GenRectangle1(out ho_Regions_temp_Wsmall, hv_Value_R12C12.TupleSelect(0), hv_Value_R12C12.TupleSelect(2), hv_Value_R12C12.TupleSelect(1), hv_Value_R12C12.TupleSelect(3));
                                HOperatorSet.GenRectangle1(out ho_Regions_temp_Wsmall3, hv_Value_R12C12.TupleSelect(0), hv_Value_R12C12.TupleSelect(2), hv_Value_R12C12.TupleSelect(1), hv_Value_R12C12.TupleSelect(3));
                            }


                        }
                        else if (Recipe.Param.Type_BlackStripe == "Vertical") // 黑條方向垂直
                        {
                            if ((int)((new HTuple((new HTuple((((hv_Value_R12C12.TupleSelect(1)) - hv_L_NonInspect)).TupleLessEqual(hv_Value_R12C12.TupleSelect(0)))).TupleOr(new HTuple((((hv_Value_R12C12.TupleSelect(1)) - hv_L_NonInspect_Wsmall)).TupleLessEqual(hv_Value_R12C12.TupleSelect(0)))))).TupleOr(new HTuple((((hv_Value_R12C12.TupleSelect(1)) - hv_L_NonInspect_Wsmall3)).TupleLessEqual(hv_Value_R12C12.TupleSelect(0))))) != 0)
                            {
                                /* 初定位失敗 */
                                return false;
                            }

                            double gap = 5.0; // 影像旋轉後，和原先影像邊界之誤差 (pixel) (20191122) Jeff Revised!

                            if (Math.Abs(hv_Value_R12C12.DArr[0] - 0) > gap && Math.Abs(hv_Value_R12C12.DArr[1] - (hv_Height.D - 1)) > gap)
                            {
                                HOperatorSet.GenRectangle1(out ho_Regions_temp, (hv_Value_R12C12.TupleSelect(0)) + hv_L_NonInspect, hv_Value_R12C12.TupleSelect(2), (hv_Value_R12C12.TupleSelect(1)) - hv_L_NonInspect, hv_Value_R12C12.TupleSelect(3));
                                HOperatorSet.GenRectangle1(out ho_Regions_temp_Wsmall, (hv_Value_R12C12.TupleSelect(0)) + hv_L_NonInspect_Wsmall, hv_Value_R12C12.TupleSelect(2), (hv_Value_R12C12.TupleSelect(1)) - hv_L_NonInspect_Wsmall, hv_Value_R12C12.TupleSelect(3));
                                HOperatorSet.GenRectangle1(out ho_Regions_temp_Wsmall3, (hv_Value_R12C12.TupleSelect(0)) + hv_L_NonInspect_Wsmall3, hv_Value_R12C12.TupleSelect(2), (hv_Value_R12C12.TupleSelect(1)) - hv_L_NonInspect_Wsmall3, hv_Value_R12C12.TupleSelect(3));
                            }
                            else if (Math.Abs(hv_Value_R12C12.DArr[0] - 0) > gap)
                            {
                                HOperatorSet.GenRectangle1(out ho_Regions_temp, (hv_Value_R12C12.TupleSelect(0)) + hv_L_NonInspect, hv_Value_R12C12.TupleSelect(2), hv_Value_R12C12.TupleSelect(1), hv_Value_R12C12.TupleSelect(3));
                                HOperatorSet.GenRectangle1(out ho_Regions_temp_Wsmall, (hv_Value_R12C12.TupleSelect(0)) + hv_L_NonInspect_Wsmall, hv_Value_R12C12.TupleSelect(2), hv_Value_R12C12.TupleSelect(1), hv_Value_R12C12.TupleSelect(3));
                                HOperatorSet.GenRectangle1(out ho_Regions_temp_Wsmall3, (hv_Value_R12C12.TupleSelect(0)) + hv_L_NonInspect_Wsmall3, hv_Value_R12C12.TupleSelect(2), hv_Value_R12C12.TupleSelect(1), hv_Value_R12C12.TupleSelect(3));
                            }
                            else if (Math.Abs(hv_Value_R12C12.DArr[1] - (hv_Height.D - 1)) > gap)
                            {
                                HOperatorSet.GenRectangle1(out ho_Regions_temp, hv_Value_R12C12.TupleSelect(0), hv_Value_R12C12.TupleSelect(2), (hv_Value_R12C12.TupleSelect(1)) - hv_L_NonInspect, hv_Value_R12C12.TupleSelect(3));
                                HOperatorSet.GenRectangle1(out ho_Regions_temp_Wsmall, hv_Value_R12C12.TupleSelect(0), hv_Value_R12C12.TupleSelect(2), (hv_Value_R12C12.TupleSelect(1)) - hv_L_NonInspect_Wsmall, hv_Value_R12C12.TupleSelect(3));
                                HOperatorSet.GenRectangle1(out ho_Regions_temp_Wsmall3, hv_Value_R12C12.TupleSelect(0), hv_Value_R12C12.TupleSelect(2), (hv_Value_R12C12.TupleSelect(1)) - hv_L_NonInspect_Wsmall3, hv_Value_R12C12.TupleSelect(3));
                            }
                            else
                            {
                                HOperatorSet.GenRectangle1(out ho_Regions_temp, hv_Value_R12C12.TupleSelect(0), hv_Value_R12C12.TupleSelect(2), hv_Value_R12C12.TupleSelect(1), hv_Value_R12C12.TupleSelect(3));
                                HOperatorSet.GenRectangle1(out ho_Regions_temp_Wsmall, hv_Value_R12C12.TupleSelect(0), hv_Value_R12C12.TupleSelect(2), hv_Value_R12C12.TupleSelect(1), hv_Value_R12C12.TupleSelect(3));
                                HOperatorSet.GenRectangle1(out ho_Regions_temp_Wsmall3, hv_Value_R12C12.TupleSelect(0), hv_Value_R12C12.TupleSelect(2), hv_Value_R12C12.TupleSelect(1), hv_Value_R12C12.TupleSelect(3));
                            }


                        }
                        
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.Union2(ho_Regions_Inspect0, ho_Regions_temp, out ExpTmpOutVar_0);
                            ho_Regions_Inspect0.Dispose();
                            ho_Regions_Inspect0 = ExpTmpOutVar_0;
                        }
                        
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.Union2(ho_Regions_Inspect0_Wsmall, ho_Regions_temp_Wsmall, out ExpTmpOutVar_0);
                            ho_Regions_Inspect0_Wsmall.Dispose();
                            ho_Regions_Inspect0_Wsmall = ExpTmpOutVar_0;
                        }
                        
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.Union2(ho_Regions_Inspect0_Wsmall3, ho_Regions_temp_Wsmall3, out ExpTmpOutVar_0);
                            ho_Regions_Inspect0_Wsmall3.Dispose();
                            ho_Regions_Inspect0_Wsmall3 = ExpTmpOutVar_0;
                        }


                    }
                }
                else
                {
                    HOperatorSet.CopyObj(ho_Regions_Inspect_orig, out ho_Regions_Inspect0, 1, -1);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.Union1(ho_Regions_Inspect0, out ExpTmpOutVar_0);
                        ho_Regions_Inspect0.Dispose();
                        ho_Regions_Inspect0 = ExpTmpOutVar_0;
                    }

                    HOperatorSet.CopyObj(ho_Regions_Inspect_orig, out ho_Regions_Inspect0_Wsmall, 1, -1);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.Union1(ho_Regions_Inspect0_Wsmall, out ExpTmpOutVar_0);
                        ho_Regions_Inspect0_Wsmall.Dispose();
                        ho_Regions_Inspect0_Wsmall = ExpTmpOutVar_0;
                    }

                    HOperatorSet.CopyObj(ho_Regions_Inspect_orig, out ho_Regions_Inspect0_Wsmall3, 1, -1);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.Union1(ho_Regions_Inspect0_Wsmall3, out ExpTmpOutVar_0);
                        ho_Regions_Inspect0_Wsmall3.Dispose();
                        ho_Regions_Inspect0_Wsmall3 = ExpTmpOutVar_0;
                    }
                }

                // 消除掉背光白色背景區域 (For Back Light Image)
                HOperatorSet.Threshold(ho_GrayImage_BL, out ho_Regions_WhiteReg_thresh_BL, Recipe.Param.PositionParam_Initial.MinGray_WhiteReg_BL, 255);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Connection(ho_Regions_WhiteReg_thresh_BL, out ExpTmpOutVar_0);
                    ho_Regions_WhiteReg_thresh_BL.Dispose();
                    ho_Regions_WhiteReg_thresh_BL = ExpTmpOutVar_0;
                }
                HOperatorSet.SelectShape(ho_Regions_WhiteReg_thresh_BL, out ho_Regions_WhiteReg_select_BL, "area", "and", Recipe.Param.PositionParam_Initial.MinArea_WhiteReg_BL, hv_Width * hv_Height);
                HOperatorSet.DilationCircle(ho_Regions_WhiteReg_select_BL, out ho_Regions_WhiteReg_BL, Recipe.Param.PositionParam_Initial.Radius_dila_WhiteReg_BL);

                HOperatorSet.Difference(ho_Regions_Inspect0, ho_Regions_WhiteReg_BL, out ho_Regions_Inspect1);
                HOperatorSet.Difference(ho_Regions_Inspect0_Wsmall, ho_Regions_WhiteReg_BL, out ho_Regions_Inspect1_Wsmall);
                HOperatorSet.Difference(ho_Regions_Inspect0_Wsmall3, ho_Regions_WhiteReg_BL, out ho_Regions_Inspect1_Wsmall3);


                // 擷取 黑色長條區域 (For Frontal Light Image) & 黑色長條區域之中心線 & 寬度縮小/放大 黑色長條區域
                HOperatorSet.ReduceDomain(insp_GrayImgList[Recipe.Param.PositionParam_Initial.ImageIndex_SegBlackStripe], ho_Regions_Inspect1, out ho_ImageReduced_FL);
                HOperatorSet.Threshold(ho_ImageReduced_FL, out ho_Regions_BlackStripe_thresh_FL, Recipe.Param.PositionParam_Initial.MinGray_SegBlackStripe, Recipe.Param.PositionParam_Initial.MaxGray_SegBlackStripe);
                //填補 縫隙 & 孔洞
                HOperatorSet.ClosingRectangle1(ho_Regions_BlackStripe_thresh_FL, out ho_Regions_BlackStripe_closing, Recipe.Param.PositionParam_Initial.W_closing_SegBlackStripe, Recipe.Param.PositionParam_Initial.H_closing_SegBlackStripe);
                //消除掉 Black Crack
                HOperatorSet.OpeningRectangle1(ho_Regions_BlackStripe_closing, out ho_Regions_BlackStripe_opening, Recipe.Param.PositionParam_Initial.MaxWidth_BlackCrack, Recipe.Param.PositionParam_Initial.H_opening_BlackCrack);
                //長寬篩選
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Connection(ho_Regions_BlackStripe_opening, out ExpTmpOutVar_0);
                    ho_Regions_BlackStripe_opening.Dispose();
                    ho_Regions_BlackStripe_opening = ExpTmpOutVar_0;
                }
                // double 轉 HTuple
                hv_MinW_BlackStripe = Recipe.Param.PositionParam_Initial.MinW_BlackStripe;
                hv_MaxW_BlackStripe = Recipe.Param.PositionParam_Initial.MaxW_BlackStripe;
                hv_MinH_BlackStripe = Recipe.Param.PositionParam_Initial.MinH_BlackStripe;
                hv_MaxH_BlackStripe = Recipe.Param.PositionParam_Initial.MaxH_BlackStripe;
                HOperatorSet.SelectShape(ho_Regions_BlackStripe_opening, out ho_Regions_BlackStripe_select,
                                         (new HTuple("width")).TupleConcat("height"), "and",
                                         hv_MinW_BlackStripe.TupleConcat(hv_MinH_BlackStripe), hv_MaxW_BlackStripe.TupleConcat(hv_MaxH_BlackStripe));
                HOperatorSet.Union1(ho_Regions_BlackStripe_select, out ho_Regions_BlackStripe_union);
                HOperatorSet.FillUp(ho_Regions_BlackStripe_union, out ho_Regions_BlackStripe_FL);
                HOperatorSet.DilationRectangle1(ho_Regions_BlackStripe_FL, out ho_Regions_BlackStripe_dila, Recipe.Param.PositionParam_Initial.W_dila_BlackStripe, Recipe.Param.PositionParam_Initial.H_dila_BlackStripe);
                HOperatorSet.Intersection(ho_Regions_BlackStripe_dila, ho_Regions_Inspect1, out ho_Regions_BlackStripe);

                HOperatorSet.Intersection(ho_Regions_BlackStripe_dila, ho_Regions_Inspect1_Wsmall, out ho_Regions_BlackStripe_Wsmall);
                HOperatorSet.Intersection(ho_Regions_BlackStripe_dila, ho_Regions_Inspect1_Wsmall3, out ho_Regions_BlackStripe_Wsmall3);

                // 黑色長條區域之中心線
                HOperatorSet.Connection(ho_Regions_BlackStripe, out ho_Regions_BlackStripe_Connect);
                // Method 1
                //HOperatorSet.RegionFeatures(ho_Regions_BlackStripe_Connect, "row", out hv_r_BlackStripe);
                //HOperatorSet.RegionFeatures(ho_Regions_BlackStripe_Connect, "column", out hv_c_BlackStripe);
                //HOperatorSet.RegionFeatures(ho_Regions_BlackStripe_Connect, "rect2_len1", out hv_L1_BlackStripe);
                //HOperatorSet.RegionFeatures(ho_Regions_BlackStripe_Connect, "rect2_phi", out hv_Phi_BlackStripe);
                // Method 2 (20190529) Jeff Revised!
                HTuple Length2 = new HTuple();
                HOperatorSet.SmallestRectangle2(ho_Regions_BlackStripe_Connect, out hv_r_BlackStripe, out hv_c_BlackStripe, out hv_Phi_BlackStripe, out hv_L1_BlackStripe, out Length2);
                // Method 3: 最大內接矩形 (20190529) Jeff Revised!
                //HTuple Length2 = new HTuple();
                //HOperatorSet.SmallestRectangle2(ho_Regions_BlackStripe_Connect, out hv_r_BlackStripe, out hv_c_BlackStripe, out hv_Phi_BlackStripe, out hv_L1_BlackStripe, out Length2);
                //HTuple R1_innerRect = new HTuple(), C1_innerRect = new HTuple(), R2_innerRect = new HTuple(), C2_innerRect = new HTuple();
                //HOperatorSet.InnerRectangle1(ho_Regions_BlackStripe_Connect, out R1_innerRect, out C1_innerRect, out R2_innerRect, out C2_innerRect);
                //hv_r_BlackStripe = (R1_innerRect + R2_innerRect) / 2.0;

                hv_count_BlackStripe = new HTuple(hv_r_BlackStripe.TupleLength());
                HTuple hv_L2_BlackStripe = new HTuple();
                HOperatorSet.TupleGenConst(hv_count_BlackStripe, 0.5, out hv_L2_BlackStripe);
                HOperatorSet.GenRectangle2(out ho_CenterLine_BlackStripe, hv_r_BlackStripe, hv_c_BlackStripe, hv_Phi_BlackStripe, hv_L1_BlackStripe, hv_L2_BlackStripe);

                //HOperatorSet.Connection(ho_Regions_BlackStripe_Wsmall, out ho_Regions_BlackStripe_Connect);
                //HOperatorSet.RegionFeatures(ho_Regions_BlackStripe_Connect, "rect2_len1", out hv_L1_BlackStripe_Wsmall);
                //HOperatorSet.Connection(ho_Regions_BlackStripe_Wsmall3, out ho_Regions_BlackStripe_Connect);
                //HOperatorSet.RegionFeatures(ho_Regions_BlackStripe_Connect, "rect2_len1", out hv_L1_BlackStripe_Wsmall3);
                // (20191112) Jeff Revised!
                HObject reg_BlackStripe_con_temp;
                HOperatorSet.Connection(ho_Regions_BlackStripe_Wsmall, out reg_BlackStripe_con_temp);
                HOperatorSet.RegionFeatures(reg_BlackStripe_con_temp, "rect2_len1", out hv_L1_BlackStripe_Wsmall);
                HOperatorSet.Connection(ho_Regions_BlackStripe_Wsmall3, out reg_BlackStripe_con_temp);
                HOperatorSet.RegionFeatures(reg_BlackStripe_con_temp, "rect2_len1", out hv_L1_BlackStripe_Wsmall3);

                // 寬度縮小 黑色長條區域
                if (Recipe.Param.Type_BlackStripe == "Horizontal") // 黑條方向水平 (20191112) Jeff Revised!
                    HOperatorSet.ErosionRectangle1(ho_Regions_BlackStripe, out ho_Regions_BlackStripe_small, 1, Recipe.Param.PositionParam_Initial.H_ero_BlackStripe_small);
                else if (Recipe.Param.Type_BlackStripe == "Vertical") // 黑條方向垂直 (20191112) Jeff Revised!
                    HOperatorSet.ErosionRectangle1(ho_Regions_BlackStripe, out ho_Regions_BlackStripe_small, Recipe.Param.PositionParam_Initial.H_ero_BlackStripe_small, 1);

                // 寬度放大 黑色長條區域
                //HOperatorSet.DilationRectangle1(ho_Regions_BlackStripe, out ho_Regions_BlackStripe_large, 1, Recipe.Param.PositionParam_Initial.H_dila_BlackStripe_large);


                /* 修正有效檢測區域 */
                if (Recipe.Param.PositionParam_Initial.Enabled_ValidReg_Revised) // (20191112) Jeff Revised!
                {
                    // 消除掉上下邊緣誤判 (For Frontal Light Image)
                    HOperatorSet.Threshold(ho_ImageReduced_FL, out ho_Regions_Inspect1_thresh_FL, Recipe.Param.PositionParam_Initial.MinGray_WhiteReg_FL, 255);
                    HOperatorSet.Union2(ho_Regions_Inspect1_thresh_FL, ho_Regions_BlackStripe, out ho_Regions_Inspect_union);
                    // 填補 邊緣縫隙 & 內部孔洞
                    HOperatorSet.ClosingRectangle1(ho_Regions_Inspect_union, out ho_Regions_Inspect_closing, Recipe.Param.PositionParam_Initial.W_closing_Inspect, Recipe.Param.PositionParam_Initial.H_closing_Inspect); // (20191112) Jeff Revised!
                                                                                                                                                                                                                          //ho_Regions_Inspect.Dispose();
                    HOperatorSet.FillUp(ho_Regions_Inspect_closing, out ho_Regions_Inspect);
                }
                else
                    ho_Regions_Inspect = ho_Regions_Inspect1.Clone();

                // 旋轉 Cell & 瑕疵region 回原本方向
                if (Recipe.Param.Enabled_rotate)
                {
                    HOperatorSet.HomMat2dIdentity(out hommat2d_origin_);
                    HOperatorSet.HomMat2dRotate(hommat2d_origin_, new HTuple(-Affine_angle_degree).TupleRad(), 0, 0, out hommat2d_origin_);
                }

                Res = true;
            }
            catch (Exception ex)
            {
                Log.WriteLog("Error: InitialPosition_Insp() 執行發生例外狀況 -> " + ex.ToString());
                return false;
            }

            return Res;
        }

        /// <summary>
        /// Cell分割 (同軸光)
        /// </summary>
        /// <returns></returns>
        public bool CellSeg_CL_Insp(ResistPanelRole Recipe) // (20190217) Jeff Revised!
        {
            bool Res = false;

            try
            {
                #region Execute CellSeg_CL_Insp()

                // Initialize local and output iconic variables
                HOperatorSet.GenEmptyObj(out ho_AllCells_Rect);
                HOperatorSet.GenEmptyObj(out ho_AllCells_Pt);
                HOperatorSet.GenEmptyObj(out ho_AllCellsPt_origin); // (20191122) Jeff Revised!
                HOperatorSet.GenEmptyObj(out ho_All_ValidCells_rect);

                HOperatorSet.ReduceDomain(insp_GrayImgList[Recipe.Param.InspParam_CellSeg_CL.ImageIndex], ho_Regions_BlackStripe_small, out ho_ImageReduced_BlackStripe_CL);
                // 將瑕疵區域(亮區)先消除
                if (Recipe.Param.InspParam_CellSeg_CL.Enabled_removeDef)
                    HOperatorSet.Threshold(ho_ImageReduced_BlackStripe_CL, out ho_Regions_removeDef, Recipe.Param.InspParam_CellSeg_CL.MinGray_removeDef, Recipe.Param.InspParam_CellSeg_CL.MaxGray_removeDef);
                else
                    HOperatorSet.GenEmptyObj(out ho_Regions_removeDef);
                //HOperatorSet.Connection(ho_Regions_Inspect, out ho_Inspect_ConnectReg);
                //HOperatorSet.Connection(ho_Regions_Inspect1, out ho_Inspect_ConnectReg); // (20190214) Jeff Revised!
                HOperatorSet.Connection(ho_Regions_Inspect0, out ho_Inspect_ConnectReg); // (20190215) Jeff Revised!
                //HOperatorSet.CountObj(ho_Inspect_ConnectReg, out hv_Num_InspectRegions);

                #region 執行 ResistPanelUC 相關顯示物件

                if (b_ResistPanelUC) // (20191112) Jeff Revised!
                {
                    // Release memory
                    Extension.HObjectMedthods.ReleaseHObject(ref ho_TiledImage_ALL);
                    Extension.HObjectMedthods.ReleaseHObject(ref ho_CellReg_Resist_ALL);
                    Extension.HObjectMedthods.ReleaseHObject(ref ho_RegionOpening_ALL);
                    Extension.HObjectMedthods.ReleaseHObject(ref ho_RegionClosing_ALL);
                    Extension.HObjectMedthods.ReleaseHObject(ref ho_RegionOpening1_ALL);
                    Extension.HObjectMedthods.ReleaseHObject(ref ho_RegionIntersection_ALL);
                    Extension.HObjectMedthods.ReleaseHObject(ref ho_SelectedRegions_ALL);

                    // Initialize
                    HOperatorSet.GenEmptyObj(out ho_TiledImage_ALL);
                    HOperatorSet.GenEmptyObj(out ho_CellReg_Resist_ALL);
                    HOperatorSet.GenEmptyObj(out ho_RegionOpening_ALL);
                    HOperatorSet.GenEmptyObj(out ho_RegionClosing_ALL);
                    HOperatorSet.GenEmptyObj(out ho_RegionOpening1_ALL);
                    HOperatorSet.GenEmptyObj(out ho_RegionIntersection_ALL);
                    HOperatorSet.GenEmptyObj(out ho_SelectedRegions_ALL);
                }

                #endregion

                for (int i_InspectRegions = 1; i_InspectRegions <= hv_Num_InspectRegions; i_InspectRegions++)
                {
                    #region For 每一檢測區域

                    //ho_OneInspectReg.Dispose();
                    HOperatorSet.SelectObj(ho_Inspect_ConnectReg, out ho_OneInspectReg, i_InspectRegions);
                    HOperatorSet.Difference(ho_OneInspectReg, ho_Regions_removeDef, out ho_RegionDifference);
                    HOperatorSet.ReduceDomain(ho_ImageReduced_BlackStripe_CL, ho_RegionDifference, out ho_ImageReduced_BlackStripe2_CL);

                    // (20190529) Jeff Revised!
                    HOperatorSet.Intersection(ho_OneInspectReg, ho_Regions_BlackStripe_Connect, out ho_Regions_BlackStripe_1InspReg);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.Connection(ho_Regions_BlackStripe_1InspReg, out ExpTmpOutVar_0);
                        ho_Regions_BlackStripe_1InspReg.Dispose();
                        ho_Regions_BlackStripe_1InspReg = ExpTmpOutVar_0;
                    }
                    HTuple Phi_BlackStripe_1InspReg = new HTuple(), L1_BlackStripe_1InspReg = new HTuple(), L2_BlackStripe_1InspReg = new HTuple();
                    HOperatorSet.SmallestRectangle2(ho_Regions_BlackStripe_1InspReg, out r_BlackStripe_1InspReg, out c_BlackStripe_1InspReg, out Phi_BlackStripe_1InspReg, out L1_BlackStripe_1InspReg, out L2_BlackStripe_1InspReg);

                    #region 影像增強 (20191112) Jeff Revised!

                    if (Recipe.Param.InspParam_CellSeg_CL.Enabled_equHisto == false) // 不做直方圖等化
                        ho_TiledImage = ho_ImageReduced_BlackStripe2_CL.Clone();
                    else if (Recipe.Param.InspParam_CellSeg_CL.Enabled_equHisto_part) // 分區做直方圖等化
                    {
                        HTuple hv_Value_WH = new HTuple();
                        HOperatorSet.RegionFeatures(ho_RegionDifference, (new HTuple("width")).TupleConcat("height"), out hv_Value_WH);
                        HTuple hv_W = new HTuple(), hv_H = new HTuple();
                        hv_W = hv_Value_WH.TupleSelect(0);
                        hv_H = hv_Value_WH.TupleSelect(1);

                        //分區數量
                        HObject ho_regions_blackStripe;
                        HOperatorSet.Intersection(ho_Regions_BlackStripe_small, ho_OneInspectReg, out ho_regions_blackStripe);
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.Connection(ho_regions_blackStripe, out ExpTmpOutVar_0);
                            ho_regions_blackStripe.Dispose();
                            ho_regions_blackStripe = ExpTmpOutVar_0;
                        }
                        HTuple hv_N_BlackStripe = new HTuple();
                        HOperatorSet.CountObj(ho_regions_blackStripe, out hv_N_BlackStripe);
                        ho_regions_blackStripe.Dispose();
                        HTuple hv_count_row = new HTuple(), hv_count_col = new HTuple(); // (20191112) Jeff Revised!
                        if (Recipe.Param.Type_BlackStripe == "Horizontal") // 黑條方向水平 (20191112) Jeff Revised!
                        {
                            hv_count_row = hv_N_BlackStripe;
                            hv_count_col = Recipe.Param.InspParam_CellSeg_CL.count_col;
                        }
                        else if (Recipe.Param.Type_BlackStripe == "Vertical") // 黑條方向垂直
                        {
                            hv_count_row = Recipe.Param.InspParam_CellSeg_CL.count_col;
                            hv_count_col = hv_N_BlackStripe;
                        }
                        HTuple hv_w = new HTuple(), hv_h = new HTuple();
                        hv_w = hv_W / hv_count_col;
                        hv_h = hv_H / hv_count_row;

                        HOperatorSet.PartitionRectangle(ho_RegionDifference, out ho_Partitioned, hv_w, hv_h);
                        HOperatorSet.RegionFeatures(ho_Partitioned, "row1", out hv_Value_R1);
                        HOperatorSet.RegionFeatures(ho_Partitioned, "column1", out hv_Value_C1);
                        HOperatorSet.RegionFeatures(ho_Partitioned, "row2", out hv_Value_R2);
                        HOperatorSet.RegionFeatures(ho_Partitioned, "column2", out hv_Value_C2);
                        HOperatorSet.CropRectangle1(ho_ImageReduced_BlackStripe2_CL, out ho_ImagePart, hv_Value_R1, hv_Value_C1, hv_Value_R2, hv_Value_C2);
                        HOperatorSet.EquHistoImage(ho_ImagePart, out ho_ImageEquHisto2);
                        //get_image_size (ImageReduced_Inspect2_BL, Width, Height)
                        HOperatorSet.TupleGenConst(hv_count_row * hv_count_col, -1, out hv_tile_rect);
                        HOperatorSet.TileImagesOffset(ho_ImageEquHisto2, out ho_TiledImage, hv_Value_R1, hv_Value_C1,
                                                      hv_tile_rect, hv_tile_rect, hv_tile_rect, hv_tile_rect, hv_Width, hv_Height);
                    }
                    else // 整個區域做直方圖等化
                        HOperatorSet.EquHistoImage(ho_ImageReduced_BlackStripe2_CL, out ho_TiledImage);

                    if (b_ResistPanelUC) // (20191112) Jeff Revised!
                    {
                        HOperatorSet.ConcatObj(ho_TiledImage_ALL, ho_TiledImage, out ho_TiledImage_ALL);
                    }

                    #endregion

                    /* 二值化 */
                    switch (Recipe.Param.InspParam_CellSeg_CL.str_BinaryImg)
                    {
                        case "固定閥值":
                            {
                                HOperatorSet.Threshold(ho_TiledImage, out ho_CellReg_Resist, Recipe.Param.InspParam_CellSeg_CL.MinGray_CellSeg, Recipe.Param.InspParam_CellSeg_CL.MaxGray_CellSeg);
                            }
                            break;
                        case "動態閥值":
                            {
                                HObject ImageMean = null;
                                HOperatorSet.MeanImage(ho_TiledImage, out ImageMean, Recipe.Param.InspParam_CellSeg_CL.MaskWidth, Recipe.Param.InspParam_CellSeg_CL.MaskHeight);
                                HOperatorSet.DynThreshold(ho_TiledImage, ImageMean, out ho_CellReg_Resist, Recipe.Param.InspParam_CellSeg_CL.Offset, Recipe.Param.InspParam_CellSeg_CL.str_LightDark);
                                ImageMean.Dispose();
                            }
                            break;
                    }

                    //Morphology + transform to rectangle
                    HOperatorSet.OpeningRectangle1(ho_CellReg_Resist, out ho_RegionOpening, Recipe.Param.InspParam_CellSeg_CL.W_opening1_CellSeg, Recipe.Param.InspParam_CellSeg_CL.H_opening1_CellSeg);
                    HOperatorSet.ClosingRectangle1(ho_RegionOpening, out ho_RegionClosing, Recipe.Param.InspParam_CellSeg_CL.W_closing_CellSeg, Recipe.Param.InspParam_CellSeg_CL.H_closing_CellSeg);
                    HOperatorSet.OpeningRectangle1(ho_RegionClosing, out ho_RegionOpening1, Recipe.Param.InspParam_CellSeg_CL.W_opening2_CellSeg, Recipe.Param.InspParam_CellSeg_CL.H_opening2_CellSeg);
                    HOperatorSet.DilationRectangle1(ho_RegionOpening1, out ho_RegionDilation, Recipe.Param.InspParam_CellSeg_CL.W_dila_CellSeg, Recipe.Param.InspParam_CellSeg_CL.H_dila_CellSeg);
                    HOperatorSet.Intersection(ho_RegionDilation, ho_Regions_BlackStripe_small, out ho_RegionIntersection);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.Connection(ho_RegionIntersection, out ExpTmpOutVar_0);
                        ho_RegionIntersection.Dispose();
                        ho_RegionIntersection = ExpTmpOutVar_0;
                    }

                    if (b_ResistPanelUC) // (20191112) Jeff Revised!
                    {
                        HOperatorSet.ConcatObj(ho_CellReg_Resist_ALL, ho_CellReg_Resist, out ho_CellReg_Resist_ALL);
                        HOperatorSet.ConcatObj(ho_RegionOpening_ALL, ho_RegionOpening, out ho_RegionOpening_ALL);
                        HOperatorSet.ConcatObj(ho_RegionClosing_ALL, ho_RegionClosing, out ho_RegionClosing_ALL);
                        HOperatorSet.ConcatObj(ho_RegionOpening1_ALL, ho_RegionOpening1, out ho_RegionOpening1_ALL);
                    }

                    /* 尺寸篩選 */
                    // double 轉 HTuple
                    hv_MinHeight_CellSeg = Recipe.Param.InspParam_CellSeg_CL.MinHeight_CellSeg / Locate_Method_FS.pixel_resolution_;
                    hv_MaxHeight_CellSeg = Recipe.Param.InspParam_CellSeg_CL.MaxHeight_CellSeg / Locate_Method_FS.pixel_resolution_;
                    hv_MinWidth_CellSeg = Recipe.Param.InspParam_CellSeg_CL.MinWidth_CellSeg / Locate_Method_FS.pixel_resolution_;
                    hv_MaxWidth_CellSeg = Recipe.Param.InspParam_CellSeg_CL.MaxWidth_CellSeg / Locate_Method_FS.pixel_resolution_;
                    HOperatorSet.SelectShape(ho_RegionIntersection, out ho_SelectedRegions, (new HTuple("width")).TupleConcat("height"), "and",
                                             (new HTuple(hv_MinWidth_CellSeg)).TupleConcat(hv_MinHeight_CellSeg),
                                             (new HTuple(hv_MaxWidth_CellSeg)).TupleConcat(hv_MaxHeight_CellSeg));

                    if (b_ResistPanelUC) // (20191112) Jeff Revised!
                    {
                        HOperatorSet.ConcatObj(ho_RegionIntersection_ALL, ho_RegionIntersection, out ho_RegionIntersection_ALL);
                        HOperatorSet.ConcatObj(ho_SelectedRegions_ALL, ho_SelectedRegions, out ho_SelectedRegions_ALL);
                    }

                    HTuple count_ValidCells; // (20190214) Jeff Revised!
                    HOperatorSet.CountObj(ho_SelectedRegions, out count_ValidCells);
                    //HOperatorSet.CountObj(ho_RegionIntersection, out count_ValidCells); // For debug!
                    if (count_ValidCells == 0) //  Cell Segmentation (0402 同軸光) 失敗
                    {
                        //HOperatorSet.WriteRegion(ho_RegionIntersection, "ho_RegionIntersection.hobj");
                        return false;
                    }

                    HOperatorSet.ShapeTrans(ho_SelectedRegions, out ho_ValidCells_pt, "inner_center");
                    
                    if (!(Compute_CellSeg(Recipe.Param.Type_BlackStripe, Recipe.Param.InspParam_CellSeg_CL.Enabled_RemainValidCells, Recipe.Param.InspParam_CellSeg_CL.str_ModeCellSeg)))
                        return false;

                    #endregion
                }

                if (b_ResistPanelUC) // (20191112) Jeff Revised!
                {
                    HTuple hv_offset_ = new HTuple(), hv_tile_rect_ = new HTuple();
                    HOperatorSet.TupleGenConst(hv_Num_InspectRegions, 0, out hv_offset_);
                    HOperatorSet.TupleGenConst(hv_Num_InspectRegions, -1, out hv_tile_rect_);
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.TileImagesOffset(ho_TiledImage_ALL, out ExpTmpOutVar_0, hv_offset_, hv_offset_, hv_tile_rect_, hv_tile_rect_, hv_tile_rect_, hv_tile_rect_, hv_Width, hv_Height);
                    ho_TiledImage_ALL.Dispose();
                    ho_TiledImage_ALL = ExpTmpOutVar_0;
                }

                // 得到Cell相關資訊
                HOperatorSet.RegionFeatures(ho_AllCells_Pt, "row", out hv_r_AllCells);
                HOperatorSet.RegionFeatures(ho_AllCells_Pt, "column", out hv_c_AllCells);
                HOperatorSet.RegionFeatures(ho_AllCells_Pt, "rect2_phi", out hv_phi_AllCells);
                hv_count_AllCells = new HTuple(hv_r_AllCells.TupleLength());

                // 旋轉瑕疵region回原本方向
                //if (Recipe.Param.Enabled_rotate)
                //{
                //    HOperatorSet.AffineTransRegion(ho_AllCells_Rect.Clone(), out ho_AllCells_Rect, hommat2d_origin_, "nearest_neighbor");
                //    HOperatorSet.AffineTransRegion(ho_AllCells_Pt.Clone(), out ho_AllCells_Pt, hommat2d_origin_, "nearest_neighbor");
                //    HOperatorSet.AffineTransRegion(ho_All_ValidCells_rect.Clone(), out ho_All_ValidCells_rect, hommat2d_origin_, "nearest_neighbor");
                //}

                Res = true;

                #endregion
            }
            catch (Exception ex)
            {
                Log.WriteLog("Error: CellSeg_CL_Insp() 執行發生例外狀況 -> " + ex.ToString());
                return false;
            }

            return Res;
        }

        /// <summary>
        /// Cell分割 (背光)
        /// </summary>
        /// <returns></returns>
        public bool CellSeg_BL_Insp(ResistPanelRole Recipe) // (20190408) Jeff Revised!
        {
            bool Res = false;

            try
            {
                #region Execute CellSeg_BL_Insp()

                // Initialize local and output iconic variables
                HOperatorSet.GenEmptyObj(out ho_AllCells_Rect);
                HOperatorSet.GenEmptyObj(out ho_AllCells_Pt);
                HOperatorSet.GenEmptyObj(out ho_AllCellsPt_origin); // (20191122) Jeff Revised!
                HOperatorSet.GenEmptyObj(out ho_All_ValidCells_rect);

                HOperatorSet.ReduceDomain(insp_GrayImgList[Recipe.Param.InspParam_CellSeg_BL.ImageIndex], ho_Regions_Inspect0, out ho_ImageReduced_Inspect_BL);
                // 將瑕疵區域(亮區)先消除
                if (Recipe.Param.InspParam_CellSeg_BL.Enabled_removeDef)
                    HOperatorSet.Threshold(ho_ImageReduced_Inspect_BL, out ho_Regions_removeDef, Recipe.Param.InspParam_CellSeg_BL.MinGray_removeDef, Recipe.Param.InspParam_CellSeg_BL.MaxGray_removeDef);
                else
                    HOperatorSet.GenEmptyObj(out ho_Regions_removeDef);
                HOperatorSet.Connection(ho_Regions_Inspect0, out ho_Inspect_ConnectReg);

                #region 執行 ResistPanelUC 相關顯示物件

                if (b_ResistPanelUC) // (20191112) Jeff Revised!
                {
                    // Release memory
                    Extension.HObjectMedthods.ReleaseHObject(ref ho_TiledImage_ALL);
                    Extension.HObjectMedthods.ReleaseHObject(ref ho_CellReg_Resist_ALL);
                    Extension.HObjectMedthods.ReleaseHObject(ref ho_RegionOpening_ALL);
                    Extension.HObjectMedthods.ReleaseHObject(ref ho_RegionClosing_ALL);
                    Extension.HObjectMedthods.ReleaseHObject(ref ho_RegionOpening1_ALL);
                    Extension.HObjectMedthods.ReleaseHObject(ref ho_RegionIntersection_ALL);
                    Extension.HObjectMedthods.ReleaseHObject(ref ho_SelectedRegions_ALL);

                    // Initialize
                    HOperatorSet.GenEmptyObj(out ho_TiledImage_ALL);
                    HOperatorSet.GenEmptyObj(out ho_CellReg_Resist_ALL);
                    HOperatorSet.GenEmptyObj(out ho_RegionOpening_ALL);
                    HOperatorSet.GenEmptyObj(out ho_RegionClosing_ALL);
                    HOperatorSet.GenEmptyObj(out ho_RegionOpening1_ALL);
                    HOperatorSet.GenEmptyObj(out ho_RegionIntersection_ALL);
                    HOperatorSet.GenEmptyObj(out ho_SelectedRegions_ALL);
                }

                #endregion

                for (int i_InspectRegions = 1; i_InspectRegions <= hv_Num_InspectRegions; i_InspectRegions++)
                {
                    #region For 每一檢測區域

                    //ho_OneInspectReg.Dispose();
                    HOperatorSet.SelectObj(ho_Inspect_ConnectReg, out ho_OneInspectReg, i_InspectRegions);
                    HOperatorSet.Difference(ho_OneInspectReg, ho_Regions_removeDef, out ho_RegionDifference);
                    HOperatorSet.ReduceDomain(ho_ImageReduced_Inspect_BL, ho_RegionDifference, out ho_ImageReduced_Inspect2_BL);

                    // (20190529) Jeff Revised!
                    HOperatorSet.Intersection(ho_OneInspectReg, ho_Regions_BlackStripe_Connect, out ho_Regions_BlackStripe_1InspReg);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.Connection(ho_Regions_BlackStripe_1InspReg, out ExpTmpOutVar_0);
                        ho_Regions_BlackStripe_1InspReg.Dispose();
                        ho_Regions_BlackStripe_1InspReg = ExpTmpOutVar_0;
                    }
                    HTuple Phi_BlackStripe_1InspReg = new HTuple(), L1_BlackStripe_1InspReg = new HTuple(), L2_BlackStripe_1InspReg = new HTuple();
                    HOperatorSet.SmallestRectangle2(ho_Regions_BlackStripe_1InspReg, out r_BlackStripe_1InspReg, out c_BlackStripe_1InspReg, out Phi_BlackStripe_1InspReg, out L1_BlackStripe_1InspReg, out L2_BlackStripe_1InspReg);

                    #region 影像增強 (20191112) Jeff Revised!

                    if (Recipe.Param.InspParam_CellSeg_BL.Enabled_equHisto == false) // 不做直方圖等化
                        ho_TiledImage = ho_ImageReduced_Inspect2_BL.Clone();
                    else if (Recipe.Param.InspParam_CellSeg_BL.Enabled_equHisto_part) // 分區做直方圖等化
                    {
                        HTuple hv_Value_WH = new HTuple();
                        HOperatorSet.RegionFeatures(ho_RegionDifference, (new HTuple("width")).TupleConcat("height"), out hv_Value_WH);
                        HTuple hv_W = new HTuple(), hv_H = new HTuple();
                        hv_W = hv_Value_WH.TupleSelect(0);
                        hv_H = hv_Value_WH.TupleSelect(1);

                        // 分區數量
                        HObject ho_regions_blackStripe;
                        HOperatorSet.Intersection(ho_Regions_BlackStripe, ho_OneInspectReg, out ho_regions_blackStripe);
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.Connection(ho_regions_blackStripe, out ExpTmpOutVar_0);
                            ho_regions_blackStripe.Dispose();
                            ho_regions_blackStripe = ExpTmpOutVar_0;
                        }
                        HTuple hv_N_BlackStripe = new HTuple();
                        HOperatorSet.CountObj(ho_regions_blackStripe, out hv_N_BlackStripe);
                        ho_regions_blackStripe.Dispose();
                        HTuple hv_count_row = new HTuple(), hv_count_col = new HTuple(); // (20191112) Jeff Revised!
                        if (Recipe.Param.Type_BlackStripe == "Horizontal") // 黑條方向水平 (20191112) Jeff Revised!
                        {
                            hv_count_row = hv_N_BlackStripe;
                            hv_count_col = Recipe.Param.InspParam_CellSeg_BL.count_col;
                        }
                        else if (Recipe.Param.Type_BlackStripe == "Vertical") // 黑條方向垂直
                        {
                            hv_count_row = Recipe.Param.InspParam_CellSeg_BL.count_col;
                            hv_count_col = hv_N_BlackStripe;
                        }
                        HTuple hv_w = new HTuple(), hv_h = new HTuple();
                        hv_w = hv_W / hv_count_col;
                        hv_h = hv_H / hv_count_row;

                        HOperatorSet.PartitionRectangle(ho_RegionDifference, out ho_Partitioned, hv_w, hv_h);
                        HOperatorSet.RegionFeatures(ho_Partitioned, "row1", out hv_Value_R1);
                        HOperatorSet.RegionFeatures(ho_Partitioned, "column1", out hv_Value_C1);
                        HOperatorSet.RegionFeatures(ho_Partitioned, "row2", out hv_Value_R2);
                        HOperatorSet.RegionFeatures(ho_Partitioned, "column2", out hv_Value_C2);
                        HOperatorSet.CropRectangle1(ho_ImageReduced_Inspect2_BL, out ho_ImagePart, hv_Value_R1, hv_Value_C1, hv_Value_R2, hv_Value_C2);
                        HOperatorSet.EquHistoImage(ho_ImagePart, out ho_ImageEquHisto2);
                        //get_image_size (ImageReduced_Inspect2_BL, Width, Height)
                        HOperatorSet.TupleGenConst(hv_count_row * hv_count_col, -1, out hv_tile_rect);
                        HOperatorSet.TileImagesOffset(ho_ImageEquHisto2, out ho_TiledImage, hv_Value_R1, hv_Value_C1,
                                                      hv_tile_rect, hv_tile_rect, hv_tile_rect, hv_tile_rect, hv_Width, hv_Height);
                    }
                    else // 整個區域做直方圖等化
                        HOperatorSet.EquHistoImage(ho_ImageReduced_Inspect2_BL, out ho_TiledImage);

                    if (b_ResistPanelUC) // (20191112) Jeff Revised!
                    {
                        HOperatorSet.ConcatObj(ho_TiledImage_ALL, ho_TiledImage, out ho_TiledImage_ALL);
                    }

                    #endregion

                    /* 二值化 */
                    switch (Recipe.Param.InspParam_CellSeg_BL.str_BinaryImg)
                    {
                        case "固定閥值":
                            {
                                HOperatorSet.Threshold(ho_TiledImage, out ho_CellReg_Resist, Recipe.Param.InspParam_CellSeg_BL.MinGray_CellSeg, Recipe.Param.InspParam_CellSeg_BL.MaxGray_CellSeg);
                            }
                            break;
                        case "動態閥值":
                            {
                                HObject ImageMean = null;
                                HOperatorSet.MeanImage(ho_TiledImage, out ImageMean, Recipe.Param.InspParam_CellSeg_BL.MaskWidth, Recipe.Param.InspParam_CellSeg_BL.MaskHeight);
                                HOperatorSet.DynThreshold(ho_TiledImage, ImageMean, out ho_CellReg_Resist, Recipe.Param.InspParam_CellSeg_BL.Offset, Recipe.Param.InspParam_CellSeg_BL.str_LightDark);
                                ImageMean.Dispose();
                            }
                            break;
                    }

                    //Morphology + transform to rectangle
                    //分成兩種情形之 Morphology
                    HOperatorSet.OpeningRectangle1(ho_CellReg_Resist, out ho_RegionOpening, Recipe.Param.InspParam_CellSeg_BL.W_opening1_CellSeg, Recipe.Param.InspParam_CellSeg_BL.H_opening1_CellSeg);
                    HOperatorSet.ClosingRectangle1(ho_RegionOpening, out ho_RegionClosing, Recipe.Param.InspParam_CellSeg_BL.W_closing_CellSeg, Recipe.Param.InspParam_CellSeg_BL.H_closing_CellSeg);
                    HOperatorSet.OpeningRectangle1(ho_RegionClosing, out ho_RegionOpening1, Recipe.Param.InspParam_CellSeg_BL.W_opening2_CellSeg, Recipe.Param.InspParam_CellSeg_BL.H_opening2_CellSeg);

                    if (b_ResistPanelUC) // (20191112) Jeff Revised!
                    {
                        HOperatorSet.ConcatObj(ho_CellReg_Resist_ALL, ho_CellReg_Resist, out ho_CellReg_Resist_ALL);
                        HOperatorSet.ConcatObj(ho_RegionOpening_ALL, ho_RegionOpening, out ho_RegionOpening_ALL);
                        HOperatorSet.ConcatObj(ho_RegionClosing_ALL, ho_RegionClosing, out ho_RegionClosing_ALL);
                        HOperatorSet.ConcatObj(ho_RegionOpening1_ALL, ho_RegionOpening1, out ho_RegionOpening1_ALL);
                    }

                    int Method = 2;
                    if (Method == 1) // Method 1
                    {
                        HOperatorSet.Connection(ho_RegionOpening1, out ho_ConnectReg);

                        /* 尺寸篩選 */
                        // double 轉 HTuple
                        hv_MinHeight_CellSeg = Recipe.Param.InspParam_CellSeg_BL.MinHeight_CellSeg / Locate_Method_FS.pixel_resolution_;
                        hv_MaxHeight_CellSeg = Recipe.Param.InspParam_CellSeg_BL.MaxHeight_CellSeg / Locate_Method_FS.pixel_resolution_;
                        hv_MinWidth_CellSeg = Recipe.Param.InspParam_CellSeg_BL.MinWidth_CellSeg / Locate_Method_FS.pixel_resolution_;
                        hv_MaxWidth_CellSeg = Recipe.Param.InspParam_CellSeg_BL.MaxWidth_CellSeg / Locate_Method_FS.pixel_resolution_;
                        HOperatorSet.SelectShape(ho_ConnectReg, out ho_SelectedRegions, (new HTuple("width")).TupleConcat("height"), "and",
                                                 (new HTuple(hv_MinWidth_CellSeg)).TupleConcat(hv_MinHeight_CellSeg),
                                                 (new HTuple(hv_MaxWidth_CellSeg)).TupleConcat(hv_MaxHeight_CellSeg));

                        if (b_ResistPanelUC) // (20191112) Jeff Revised!
                        {
                            HOperatorSet.ConcatObj(ho_SelectedRegions_ALL, ho_SelectedRegions, out ho_SelectedRegions_ALL);
                        }

                        HTuple count_ValidCells; // (20190214) Jeff Revised!
                        HOperatorSet.CountObj(ho_SelectedRegions, out count_ValidCells);
                        //HOperatorSet.CountObj(ho_RegionIntersection, out count_ValidCells); // For debug!
                        if (count_ValidCells == 0) //  Cell Segmentation (0201 背光) 失敗
                        {
                            HOperatorSet.WriteRegion(ho_ConnectReg, "ho_ConnectReg.hobj");
                            return false;
                        }

                        HOperatorSet.DilationRectangle1(ho_SelectedRegions, out ho_RegionDilation, Recipe.Param.InspParam_CellSeg_BL.W_dila_CellSeg, Recipe.Param.InspParam_CellSeg_BL.H_dila_CellSeg);
                        HOperatorSet.Intersection(ho_RegionDilation, ho_Regions_BlackStripe, out ho_RegionIntersection);
                        HOperatorSet.ShapeTrans(ho_RegionIntersection, out ho_ValidCells_pt, "inner_center");

                        if (b_ResistPanelUC) // (20191112) Jeff Revised!
                        {
                            HOperatorSet.ConcatObj(ho_RegionIntersection_ALL, ho_RegionIntersection, out ho_RegionIntersection_ALL);
                        }
                    }
                    else if (Method == 2) // Method 2 (20190408) Jeff Revised!
                    {
                        HOperatorSet.DilationRectangle1(ho_RegionOpening1, out ho_RegionDilation, Recipe.Param.InspParam_CellSeg_BL.W_dila_CellSeg, Recipe.Param.InspParam_CellSeg_BL.H_dila_CellSeg);
                        HOperatorSet.Intersection(ho_RegionDilation, ho_Regions_BlackStripe_small, out ho_RegionIntersection);
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.Connection(ho_RegionIntersection, out ExpTmpOutVar_0);
                            ho_RegionIntersection.Dispose();
                            ho_RegionIntersection = ExpTmpOutVar_0;
                        }

                        /* 尺寸篩選 */
                        // double 轉 HTuple
                        hv_MinHeight_CellSeg = Recipe.Param.InspParam_CellSeg_BL.MinHeight_CellSeg / Locate_Method_FS.pixel_resolution_;
                        hv_MaxHeight_CellSeg = Recipe.Param.InspParam_CellSeg_BL.MaxHeight_CellSeg / Locate_Method_FS.pixel_resolution_;
                        hv_MinWidth_CellSeg = Recipe.Param.InspParam_CellSeg_BL.MinWidth_CellSeg / Locate_Method_FS.pixel_resolution_;
                        hv_MaxWidth_CellSeg = Recipe.Param.InspParam_CellSeg_BL.MaxWidth_CellSeg / Locate_Method_FS.pixel_resolution_;
                        HOperatorSet.SelectShape(ho_RegionIntersection, out ho_SelectedRegions, (new HTuple("width")).TupleConcat("height"), "and",
                                                 (new HTuple(hv_MinWidth_CellSeg)).TupleConcat(hv_MinHeight_CellSeg),
                                                 (new HTuple(hv_MaxWidth_CellSeg)).TupleConcat(hv_MaxHeight_CellSeg));

                        if (b_ResistPanelUC) // (20191112) Jeff Revised!
                        {
                            HOperatorSet.ConcatObj(ho_RegionIntersection_ALL, ho_RegionIntersection, out ho_RegionIntersection_ALL);
                            HOperatorSet.ConcatObj(ho_SelectedRegions_ALL, ho_SelectedRegions, out ho_SelectedRegions_ALL);
                        }

                        HTuple count_ValidCells;
                        HOperatorSet.CountObj(ho_SelectedRegions, out count_ValidCells);
                        //HOperatorSet.CountObj(ho_RegionIntersection, out count_ValidCells); // For debug!
                        if (count_ValidCells == 0) //  Cell Segmentation (0201 背光) 失敗
                        {
                            //HOperatorSet.WriteRegion(ho_RegionIntersection, "ho_RegionIntersection.hobj");
                            return false;
                        }
                        HOperatorSet.ShapeTrans(ho_SelectedRegions, out ho_ValidCells_pt, "inner_center");
                    }
                    
                    if (!(Compute_CellSeg(Recipe.Param.Type_BlackStripe, Recipe.Param.InspParam_CellSeg_BL.Enabled_RemainValidCells, Recipe.Param.InspParam_CellSeg_BL.str_ModeCellSeg)))
                        return false;
                    
                    #endregion
                }

                if (b_ResistPanelUC) // (20191112) Jeff Revised!
                {
                    HTuple hv_offset_ = new HTuple(), hv_tile_rect_ = new HTuple();
                    HOperatorSet.TupleGenConst(hv_Num_InspectRegions, 0, out hv_offset_);
                    HOperatorSet.TupleGenConst(hv_Num_InspectRegions, -1, out hv_tile_rect_);
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.TileImagesOffset(ho_TiledImage_ALL, out ExpTmpOutVar_0, hv_offset_, hv_offset_, hv_tile_rect_, hv_tile_rect_, hv_tile_rect_, hv_tile_rect_, hv_Width, hv_Height);
                    ho_TiledImage_ALL.Dispose();
                    ho_TiledImage_ALL = ExpTmpOutVar_0;
                }

                // 得到Cell相關資訊
                HOperatorSet.RegionFeatures(ho_AllCells_Pt, "row", out hv_r_AllCells);
                HOperatorSet.RegionFeatures(ho_AllCells_Pt, "column", out hv_c_AllCells);
                HOperatorSet.RegionFeatures(ho_AllCells_Pt, "rect2_phi", out hv_phi_AllCells);
                hv_count_AllCells = new HTuple(hv_r_AllCells.TupleLength());

                // 旋轉瑕疵region回原本方向
                //if (Recipe.Param.Enabled_rotate)
                //{
                //    HOperatorSet.AffineTransRegion(ho_AllCells_Rect.Clone(), out ho_AllCells_Rect, hommat2d_origin_, "nearest_neighbor");
                //    HOperatorSet.AffineTransRegion(ho_AllCells_Pt.Clone(), out ho_AllCells_Pt, hommat2d_origin_, "nearest_neighbor");
                //    HOperatorSet.AffineTransRegion(ho_All_ValidCells_rect.Clone(), out ho_All_ValidCells_rect, hommat2d_origin_, "nearest_neighbor");
                //}
                
                Res = true;

                #endregion
            }
            catch (Exception ex)
            {
                Log.WriteLog("Error: CellSeg_BL_Insp() 執行發生例外狀況 -> " + ex.ToString());
                return false;
            }

            return Res;
        }
        
        public bool Compute_CellSeg(string type_BlackStripe = "Horizontal", bool b_RemainValidCells = false, string str_ModeCellSeg = "mode2") // (20191112) Jeff Revised!
        {
            bool Res = false;

            try
            {
                #region Execute Compute_CellSeg()

                HTuple hv_L1 = new HTuple(), hv_L2 = new HTuple(), hv_cell_y_count = new HTuple(), hv_cell_x_count = new HTuple();

                // 計算左上角Cell座標
                HOperatorSet.RegionFeatures(ho_OneInspectReg, "row1", out hv_r1_InspectReg);
                HOperatorSet.RegionFeatures(ho_OneInspectReg, "column1", out hv_c1_InspectReg);
                HOperatorSet.RegionFeatures(ho_OneInspectReg, "row2", out hv_r2_InspectReg);
                HOperatorSet.RegionFeatures(ho_OneInspectReg, "column2", out hv_c2_InspectReg);

                HOperatorSet.RegionFeatures(ho_ValidCells_pt, "row", out hv_r_ValidCells);
                HOperatorSet.RegionFeatures(ho_ValidCells_pt, "column", out hv_c_ValidCells);

                hv_n_Cell_pdy = (((hv_r_ValidCells - hv_r1_InspectReg) / (Locate_Method_FS.cell_pdy_ / Locate_Method_FS.resize_))).TupleInt();
                hv_n_Cell_pdx = (((hv_c_ValidCells - hv_c1_InspectReg) / (Locate_Method_FS.cell_pdx_ / Locate_Method_FS.resize_))).TupleInt();
                if (type_BlackStripe == "Horizontal") // 黑條方向水平 (20191112) Jeff Revised!
                {
                    if (str_ModeCellSeg == "mode1")
                        hv_y_FirstCell = ((hv_r_ValidCells - ((Locate_Method_FS.cell_pdy_ / Locate_Method_FS.resize_) * hv_n_Cell_pdy))).TupleMedian();
                    else if (str_ModeCellSeg == "mode2")
                        hv_y_FirstCell = r_BlackStripe_1InspReg.TupleSelect(0); // (20190529) Jeff Revised!
                    hv_x_FirstCell = ((hv_c_ValidCells - ((Locate_Method_FS.cell_pdx_ / Locate_Method_FS.resize_) * hv_n_Cell_pdx))).TupleMedian();
                }
                else if (type_BlackStripe == "Vertical") // 黑條方向垂直
                {
                    if (str_ModeCellSeg == "mode1")
                        hv_x_FirstCell = ((hv_c_ValidCells - ((Locate_Method_FS.cell_pdx_ / Locate_Method_FS.resize_) * hv_n_Cell_pdx))).TupleMedian();
                    else if (str_ModeCellSeg == "mode2")
                        hv_x_FirstCell = c_BlackStripe_1InspReg.TupleSelect(0);
                    hv_y_FirstCell = ((hv_r_ValidCells - ((Locate_Method_FS.cell_pdy_ / Locate_Method_FS.resize_) * hv_n_Cell_pdy))).TupleMedian();
                }
                HOperatorSet.GenRegionPoints(out ho_FirstCell_pt, hv_y_FirstCell, hv_x_FirstCell);

                HOperatorSet.CountObj(ho_ValidCells_pt, out hv_N_ValidCell);
                HOperatorSet.TupleGenConst(hv_N_ValidCell, 0, out hv_Phi);
                HOperatorSet.TupleGenConst(hv_N_ValidCell, 0.5 * Locate_Method_FS.cell_pwidth_ / Locate_Method_FS.resize_, out hv_L1);
                HOperatorSet.TupleGenConst(hv_N_ValidCell, 0.5 * Locate_Method_FS.cell_pheight_ / Locate_Method_FS.resize_, out hv_L2);
                HOperatorSet.GenRectangle2(out ho_ValidCells_rect, hv_r_ValidCells, hv_c_ValidCells, hv_Phi, hv_L1, hv_L2);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_All_ValidCells_rect, ho_ValidCells_rect, out ExpTmpOutVar_0);
                    ho_All_ValidCells_rect.Dispose();
                    ho_All_ValidCells_rect = ExpTmpOutVar_0;
                }

                //由左上角Cell座標展開所有Cell座標
                if (type_BlackStripe == "Horizontal") // 黑條方向水平 (20191112) Jeff Revised!
                {
                    if (str_ModeCellSeg == "mode1")
                        hv_cell_y_count = ((((hv_r2_InspectReg - hv_y_FirstCell) / (Locate_Method_FS.cell_pdy_ / Locate_Method_FS.resize_))).TupleInt()) + 1;
                    else if (str_ModeCellSeg == "mode2")
                        hv_cell_y_count = new HTuple(r_BlackStripe_1InspReg.TupleLength()); // (20190529) Jeff Revised!
                    hv_cell_x_count = ((((hv_c2_InspectReg - hv_x_FirstCell) / (Locate_Method_FS.cell_pdx_ / Locate_Method_FS.resize_))).TupleInt()) + 1;
                }
                else if (type_BlackStripe == "Vertical") // 黑條方向垂直
                {
                    if (str_ModeCellSeg == "mode1")
                        hv_cell_x_count = ((((hv_c2_InspectReg - hv_x_FirstCell) / (Locate_Method_FS.cell_pdx_ / Locate_Method_FS.resize_))).TupleInt()) + 1;
                    else if (str_ModeCellSeg == "mode2")
                        hv_cell_x_count = new HTuple(c_BlackStripe_1InspReg.TupleLength());
                    hv_cell_y_count = ((((hv_r2_InspectReg - hv_y_FirstCell) / (Locate_Method_FS.cell_pdy_ / Locate_Method_FS.resize_))).TupleInt()) + 1;
                }

                HOperatorSet.GenEmptyRegion(out ho_AllCells_ext_pt);
                if (type_BlackStripe == "Horizontal") // 黑條方向水平 (20191112) Jeff Revised!
                {
                    HObject ho_AllCells_extY_pt;
                    HOperatorSet.GenEmptyRegion(out ho_AllCells_extY_pt);
                    // y方向
                    if (str_ModeCellSeg == "mode1")
                    {
                        for (int y = 0; y <= hv_cell_y_count - 1; y++)
                        {
                            //ho_OneCell_pt.Dispose();
                            HOperatorSet.GenRegionPoints(out ho_OneCell_pt, hv_y_FirstCell + ((Locate_Method_FS.cell_pdy_ / Locate_Method_FS.resize_) * y), hv_x_FirstCell);
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.Union2(ho_AllCells_extY_pt, ho_OneCell_pt, out ExpTmpOutVar_0);
                                ho_AllCells_extY_pt.Dispose();
                                ho_AllCells_extY_pt = ExpTmpOutVar_0;
                            }
                        }
                    }
                    else if (str_ModeCellSeg == "mode2")
                    {
                        HTuple hv_X_FirstCell = new HTuple();
                        HOperatorSet.TupleGenConst(new HTuple(r_BlackStripe_1InspReg.TupleLength()), hv_x_FirstCell, out hv_X_FirstCell);
                        HOperatorSet.GenRegionPoints(out ho_AllCells_extY_pt, r_BlackStripe_1InspReg, hv_X_FirstCell);
                    }
                    // x方向
                    for (int x = 0; x <= hv_cell_x_count - 1; x++)
                    {
                        //ho_moved.Dispose();
                        HOperatorSet.MoveRegion(ho_AllCells_extY_pt, out ho_moved, 0, (Locate_Method_FS.cell_pdx_ / Locate_Method_FS.resize_) * x);
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.Union2(ho_AllCells_ext_pt, ho_moved, out ExpTmpOutVar_0);
                            ho_AllCells_ext_pt.Dispose();
                            ho_AllCells_ext_pt = ExpTmpOutVar_0;
                        }
                    }

                    ho_AllCells_extY_pt.Dispose();
                }
                else if (type_BlackStripe == "Vertical") // 黑條方向垂直
                {
                    HObject ho_AllCells_extX_pt;
                    HOperatorSet.GenEmptyRegion(out ho_AllCells_extX_pt);
                    // x方向
                    if (str_ModeCellSeg == "mode1")
                    {
                        for (int x = 0; x <= hv_cell_x_count - 1; x++)
                        {
                            //ho_OneCell_pt.Dispose();
                            HOperatorSet.GenRegionPoints(out ho_OneCell_pt, hv_y_FirstCell, hv_x_FirstCell + ((Locate_Method_FS.cell_pdx_ / Locate_Method_FS.resize_) * x));
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.Union2(ho_AllCells_extX_pt, ho_OneCell_pt, out ExpTmpOutVar_0);
                                ho_AllCells_extX_pt.Dispose();
                                ho_AllCells_extX_pt = ExpTmpOutVar_0;
                            }
                        }
                    }
                    else if (str_ModeCellSeg == "mode2")
                    {
                        HTuple hv_Y_FirstCell = new HTuple();
                        HOperatorSet.TupleGenConst(new HTuple(c_BlackStripe_1InspReg.TupleLength()), hv_y_FirstCell, out hv_Y_FirstCell);
                        HOperatorSet.GenRegionPoints(out ho_AllCells_extX_pt, hv_Y_FirstCell, c_BlackStripe_1InspReg);
                    }
                    // y方向
                    for (int y = 0; y <= hv_cell_y_count - 1; y++)
                    {
                        //ho_moved.Dispose();
                        HOperatorSet.MoveRegion(ho_AllCells_extX_pt, out ho_moved, (Locate_Method_FS.cell_pdy_ / Locate_Method_FS.resize_) * y, 0);
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.Union2(ho_AllCells_ext_pt, ho_moved, out ExpTmpOutVar_0);
                            ho_AllCells_ext_pt.Dispose();
                            ho_AllCells_ext_pt = ExpTmpOutVar_0;
                        }
                    }

                    ho_AllCells_extX_pt.Dispose();
                }

                // (20190426) Jeff Revised!
                HObject allCells_pt, allCells_rect, AllCells_pt, allCells_rect_ImageBorder, allCells_rect_select = null;
                if (b_RemainValidCells)
                {
                    HOperatorSet.Difference(ho_AllCells_ext_pt, ho_ValidCells_rect, out ho_Cells_ext_pt);
                    HOperatorSet.Union2(ho_Cells_ext_pt, ho_ValidCells_pt, out allCells_pt);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.Connection(allCells_pt, out ExpTmpOutVar_0);
                        allCells_pt.Dispose();
                        allCells_pt = ExpTmpOutVar_0;
                    }
                }
                else
                    HOperatorSet.Connection(ho_AllCells_ext_pt, out allCells_pt);

                HOperatorSet.RegionFeatures(allCells_pt, "row", out hv_r_allCells);
                HOperatorSet.RegionFeatures(allCells_pt, "column", out hv_c_allCells);
                HOperatorSet.CountObj(allCells_pt, out hv_N_allCells);
                HOperatorSet.TupleGenConst(hv_N_allCells, 0, out hv_Phi);
                HOperatorSet.TupleGenConst(hv_N_allCells, 0.5 * Locate_Method_FS.cell_pwidth_ / Locate_Method_FS.resize_, out hv_L1);
                HOperatorSet.TupleGenConst(hv_N_allCells, 0.5 * Locate_Method_FS.cell_pheight_ / Locate_Method_FS.resize_, out hv_L2);
                HOperatorSet.SetSystem("clip_region", "false"); // (20190529) Jeff Revised!
                HOperatorSet.GenRectangle2(out allCells_rect, hv_r_allCells, hv_c_allCells, hv_Phi, hv_L1, hv_L2);
                HOperatorSet.SetSystem("clip_region", "true"); // (20190529) Jeff Revised!

                // 切掉超出影像邊界之區域
                HOperatorSet.Intersection(allCells_rect, ho_Region_ImageBorder, out allCells_rect_ImageBorder);

                // 消除不完整Cell
                int method = 2;
                if (method == 1)
                {
                    HTuple hv_area_cells = new HTuple();
                    HOperatorSet.RegionFeatures(allCells_rect, "area", out hv_area_cells);
                    HTuple MinArea_cell = hv_area_cells.TupleMin();
                    HTuple MaxArea_cell = hv_area_cells.TupleMax();
                    HTuple hv_height_cells = new HTuple();
                    HOperatorSet.RegionFeatures(allCells_rect, "height", out hv_height_cells);
                    HTuple Maxheight_cell = hv_height_cells.TupleMax();
                    HOperatorSet.SelectShape(allCells_rect_ImageBorder, out allCells_rect_select, (new HTuple("area")).TupleConcat("height"), "and",
                                             (MaxArea_cell - 1).TupleConcat(Maxheight_cell - 1), ((MaxArea_cell)).TupleConcat(Maxheight_cell));
                }
                else if (method == 2) // (20190529) Jeff Revised!
                {
                    HTuple hv_Area_allCells1 = new HTuple(), hv_Area_allCells2 = new HTuple(), hv_Equal = new HTuple(), hv_Indices = new HTuple();
                    HOperatorSet.RegionFeatures(allCells_rect, "area", out hv_Area_allCells1);
                    HOperatorSet.RegionFeatures(allCells_rect_ImageBorder, "area", out hv_Area_allCells2);
                    HOperatorSet.TupleEqualElem(hv_Area_allCells1, hv_Area_allCells2, out hv_Equal);
                    HOperatorSet.TupleFind(hv_Equal, 1, out hv_Indices);
                    if ((int)(new HTuple(hv_Indices.TupleNotEqual(-1))) == 1)
                    {
                        hv_Indices = hv_Indices + 1;
                        HOperatorSet.SelectObj(allCells_rect, out allCells_rect_select, hv_Indices);
                    }
                    else
                        HOperatorSet.GenEmptyObj(out allCells_rect_select);
                }

                // 轉成各Cell中心點
                HOperatorSet.ShapeTrans(allCells_rect_select, out AllCells_pt, "inner_center");

                // 結合所有有效檢測區之Cell
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_AllCells_Rect, allCells_rect_select, out ExpTmpOutVar_0);
                    ho_AllCells_Rect.Dispose();
                    ho_AllCells_Rect = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_AllCells_Pt, AllCells_pt, out ExpTmpOutVar_0);
                    ho_AllCells_Pt.Dispose();
                    ho_AllCells_Pt = ExpTmpOutVar_0;
                }

                #region Release memories

                allCells_pt.Dispose();
                allCells_rect.Dispose();
                AllCells_pt.Dispose();
                allCells_rect_ImageBorder.Dispose();
                allCells_rect_select.Dispose();

                #endregion

                Res = true;

                #endregion
            }
            catch
            { }

            return Res;
        }

        public bool Df_CellReg(ResistPanelRole Recipe) // (20191122) Jeff Revised!
        {
            bool Res = false;

            try
            {
                #region Execute Df_CellReg()

                // Initialize local and output iconic variables
                HOperatorSet.GenEmptyObj(out ho_Region_InspReg);
                HOperatorSet.GenEmptyObj(out ho_InspRects_AllCells);

                /* 影像檢測範圍 */
                if (Recipe.Param.InspParam_Df_CellReg.Enabled_InspReg)
                    HOperatorSet.GenRectangle2(out ho_Region_InspReg, hv_Height / 2.0, hv_Width / 2.0, 0, Recipe.Param.InspParam_Df_CellReg.W_InspReg / 2.0, Recipe.Param.InspParam_Df_CellReg.H_InspReg / 2.0);

                /* 設定瑕疵Cell判斷範圍 */
                if (hv_count_AllCells <= 0) return false;
                // μm 轉 pixel
                HTuple hv_W_Cell = Recipe.Param.InspParam_Df_CellReg.W_Cell / Locate_Method_FS.pixel_resolution_;
                HTuple hv_H_Cell = Recipe.Param.InspParam_Df_CellReg.H_Cell / Locate_Method_FS.pixel_resolution_;
                HOperatorSet.TupleGenConst(hv_count_AllCells, 0.5 * hv_W_Cell, out hv_L1_AllCells);
                HOperatorSet.TupleGenConst(hv_count_AllCells, 0.5 * hv_H_Cell, out hv_L2_AllCells);
                HOperatorSet.GenRectangle2(out ho_InspRects_AllCells, hv_r_AllCells, hv_c_AllCells, hv_phi_AllCells, hv_L1_AllCells, hv_L2_AllCells);
                HTuple hv_area_cells = new HTuple();
                HOperatorSet.RegionFeatures(ho_InspRects_AllCells, "area", out hv_area_cells);
                hv_MinArea_cell = hv_area_cells.TupleMin();
                hv_MaxArea_cell = hv_area_cells.TupleMax();

                // 旋轉瑕疵region回原本方向
                //if (Recipe.Param.Enabled_rotate)
                //{
                //    HOperatorSet.AffineTransRegion(ho_InspRects_AllCells.Clone(), out ho_InspRects_AllCells, hommat2d_origin_, "nearest_neighbor");
                //}

                // 旋轉檢測到之所有Cell中心回原本方向
                if (Recipe.Param.Enabled_rotate) // (20191122) Jeff Revised!
                {
                    HObject CellCenter, CellCenter_rect, CellCenter_rect_origin;
                    // 影像檢測範圍
                    if (Recipe.Param.InspParam_Df_CellReg.Enabled_InspReg)
                    {
                        HOperatorSet.SetSystem("store_empty_region", "false");
                        HOperatorSet.Intersection(ho_AllCells_Pt, ho_Region_InspReg, out CellCenter);
                        HOperatorSet.SetSystem("store_empty_region", "true");
                    }
                    else
                        CellCenter = ho_AllCells_Pt.Clone();

                    // 轉成矩形 (避免Point經旋轉後可能消失)
                    LocalDefectMappping(CellCenter, ho_AllCells_Rect, out CellCenter_rect);
                    
                    HOperatorSet.AffineTransRegion(CellCenter_rect, out CellCenter_rect_origin, hommat2d_origin_, "nearest_neighbor");

                    int method = 2;
                    if (method == 1) // 圓
                    {
                        HTuple row_AllCellsPt = new HTuple(), column_AllCellsPt = new HTuple(), radius_AllCellsPt = new HTuple();
                        HOperatorSet.RegionFeatures(CellCenter_rect_origin, "row", out row_AllCellsPt);
                        HOperatorSet.RegionFeatures(CellCenter_rect_origin, "column", out column_AllCellsPt);
                        HOperatorSet.TupleGenConst(row_AllCellsPt.TupleLength(), 1, out radius_AllCellsPt);
                        Extension.HObjectMedthods.ReleaseHObject(ref ho_AllCellsPt_origin);
                        HOperatorSet.GenCircle(out ho_AllCellsPt_origin, row_AllCellsPt, column_AllCellsPt, radius_AllCellsPt);
                    }
                    else if (method == 2) // 中心
                    {
                        Extension.HObjectMedthods.ReleaseHObject(ref ho_AllCellsPt_origin);
                        HOperatorSet.ShapeTrans(CellCenter_rect_origin, out ho_AllCellsPt_origin, "inner_center");
                    }

                    CellCenter.Dispose();
                    CellCenter_rect.Dispose();
                    CellCenter_rect_origin.Dispose();
                }

                Res = true;

                #endregion
            }
            catch (Exception ex)
            {
                Log.WriteLog("Error: Df_CellReg() 執行發生例外狀況 -> " + ex.ToString());
                return false;
            }

            return Res;
        }

        /// <summary>
        /// 檢測黑條內白點 (正光)
        /// </summary>
        /// <param name="Recipe"></param>
        /// <returns></returns>
        public bool WhiteBlob_FL_Insp(ResistPanelRole Recipe)
        {
            bool Res = false;

            try
            {
                #region Execute WhiteBlob_FL_Insp()

                // Initialize local and output iconic variables
                HOperatorSet.GenEmptyObj(out ho_DefectReg_WhiteBlob_FL);

                if (!Cell_BlackStripe_Insp(Recipe, Recipe.Param.InspParam_WhiteBlob_FL, out ho_DefectReg_WhiteBlob_FL))
                    return false;

                // 旋轉瑕疵region回原本方向
                //if (Recipe.Param.Enabled_rotate)
                //{
                //    HOperatorSet.AffineTransRegion(ho_DefectReg_WhiteBlob_FL.Clone(), out ho_DefectReg_WhiteBlob_FL, hommat2d_origin_, "nearest_neighbor");
                //}

                // (20190807) Jeff Revised!
                if (Recipe.Param.B_AOI_absolNG) // 是否啟用AOI判斷絕對NG
                {
                    HObject Reg_absolNG;
                    Extension.HObjectMedthods.ReleaseHObject(ref reg_SelectGray_AbsolNG);
                    if (!AOI_absolNG_Insp(Recipe.Param.InspParam_WhiteBlob_FL.cls_absolNG, Recipe.Param.InspParam_WhiteBlob_FL.ImageIndex, ho_DefectReg_WhiteBlob_FL, out Reg_absolNG, out reg_SelectGray_AbsolNG))
                    {
                        Extension.HObjectMedthods.ReleaseHObject(ref Reg_absolNG);
                        Dict_AOI_absolNG["黑條內白點 (正光)"].DefectReg = ho_DefectReg_WhiteBlob_FL.Clone();
                        return false;
                    }
                    else
                    {
                        Dict_AOI_absolNG["黑條內白點 (正光)"].DefectReg = Reg_absolNG;

                        #region 加入絕對NG Region所在Cell之其他瑕疵Region

                        HObject Reg_absolNG_Cell;
                        if (Recipe.Param.InspParam_WhiteBlob_FL.cls_absolNG.Enabled)
                        {
                            HObject temp;
                            HTuple hv_area_InspRects_Defect = new HTuple(), hv_Greater = new HTuple(), hv_Index_defect = new HTuple();
                            HOperatorSet.Intersection(ho_InspRects_AllCells, Reg_absolNG, out temp);
                            HOperatorSet.RegionFeatures(temp, "area", out hv_area_InspRects_Defect);
                            HOperatorSet.TupleGreaterElem(hv_area_InspRects_Defect, 0.0, out hv_Greater);
                            HOperatorSet.TupleFind(hv_Greater, 1, out hv_Index_defect);

                            if ((int)(new HTuple(hv_Index_defect.TupleNotEqual(-1))) == 1)
                            {
                                hv_Index_defect = hv_Index_defect + 1;
                                HOperatorSet.SelectObj(ho_InspRects_AllCells, out Reg_absolNG_Cell, hv_Index_defect);
                            }
                            else
                                Reg_absolNG_Cell = Reg_absolNG.Clone();
                            temp.Dispose();
                        }
                        else
                            Reg_absolNG_Cell = Reg_absolNG.Clone();

                        #endregion

                        {
                            HObject AOI_NG;
                            HOperatorSet.Difference(ho_DefectReg_WhiteBlob_FL, Reg_absolNG_Cell, out AOI_NG);
                            Dict_AOI_NG["黑條內白點 (正光)"].DefectReg = AOI_NG;
                        }
                        Reg_absolNG_Cell.Dispose();
                    }
                }
                else
                {
                    Dict_AOI_absolNG["黑條內白點 (正光)"].DefectReg = ho_DefectReg_WhiteBlob_FL.Clone();
                }

                #endregion

                Res = true;
            }
            catch (Exception ex)
            {
                Log.WriteLog("Error: WhiteBlob_FL_Insp() 執行發生例外狀況 -> " + ex.ToString());
                return false;
            }

            return Res;
        }

        /// <summary>
        /// 檢測黑條內白點 (同軸光)
        /// </summary>
        /// <param name="Recipe"></param>
        /// <returns></returns>
        public bool WhiteBlob_CL_Insp(ResistPanelRole Recipe)
        {
            bool Res = false;

            try
            {
                #region Execute WhiteBlob_CL_Insp()

                // Initialize local and output iconic variables
                HOperatorSet.GenEmptyObj(out ho_DefectReg_WhiteBlob_CL);

                if (!Cell_BlackStripe_Insp(Recipe, Recipe.Param.InspParam_WhiteBlob_CL, out ho_DefectReg_WhiteBlob_CL))
                    return false;

                // 旋轉瑕疵region回原本方向
                //if (Recipe.Param.Enabled_rotate)
                //{
                //    HOperatorSet.AffineTransRegion(ho_DefectReg_WhiteBlob_CL.Clone(), out ho_DefectReg_WhiteBlob_CL, hommat2d_origin_, "nearest_neighbor");
                //}

                // (20190807) Jeff Revised!
                if (Recipe.Param.B_AOI_absolNG) // 是否啟用AOI判斷絕對NG
                {
                    HObject Reg_absolNG;
                    Extension.HObjectMedthods.ReleaseHObject(ref reg_SelectGray_AbsolNG);
                    if (!AOI_absolNG_Insp(Recipe.Param.InspParam_WhiteBlob_CL.cls_absolNG, Recipe.Param.InspParam_WhiteBlob_CL.ImageIndex, ho_DefectReg_WhiteBlob_CL, out Reg_absolNG, out reg_SelectGray_AbsolNG))
                    {
                        Extension.HObjectMedthods.ReleaseHObject(ref Reg_absolNG);
                        Dict_AOI_absolNG["黑條內白點 (同軸光)"].DefectReg = ho_DefectReg_WhiteBlob_CL.Clone();
                        return false;
                    }
                    else
                    {
                        Dict_AOI_absolNG["黑條內白點 (同軸光)"].DefectReg = Reg_absolNG;

                        #region 加入絕對NG Region所在Cell之其他瑕疵Region

                        HObject Reg_absolNG_Cell;
                        if (Recipe.Param.InspParam_WhiteBlob_CL.cls_absolNG.Enabled)
                        {
                            HObject temp;
                            HTuple hv_area_InspRects_Defect = new HTuple(), hv_Greater = new HTuple(), hv_Index_defect = new HTuple();
                            HOperatorSet.Intersection(ho_InspRects_AllCells, Reg_absolNG, out temp);
                            HOperatorSet.RegionFeatures(temp, "area", out hv_area_InspRects_Defect);
                            HOperatorSet.TupleGreaterElem(hv_area_InspRects_Defect, 0.0, out hv_Greater);
                            HOperatorSet.TupleFind(hv_Greater, 1, out hv_Index_defect);

                            if ((int)(new HTuple(hv_Index_defect.TupleNotEqual(-1))) == 1)
                            {
                                hv_Index_defect = hv_Index_defect + 1;
                                HOperatorSet.SelectObj(ho_InspRects_AllCells, out Reg_absolNG_Cell, hv_Index_defect);
                            }
                            else
                                Reg_absolNG_Cell = Reg_absolNG.Clone();
                            temp.Dispose();
                        }
                        else
                            Reg_absolNG_Cell = Reg_absolNG.Clone();

                        #endregion
                        
                        {
                            HObject AOI_NG;
                            HOperatorSet.Difference(ho_DefectReg_WhiteBlob_CL, Reg_absolNG_Cell, out AOI_NG);
                            Dict_AOI_NG["黑條內白點 (同軸光)"].DefectReg = AOI_NG;
                        }
                        Reg_absolNG_Cell.Dispose();
                    }
                }
                else
                {
                    Dict_AOI_absolNG["黑條內白點 (同軸光)"].DefectReg = ho_DefectReg_WhiteBlob_CL.Clone();
                }

                #endregion

                Res = true;
            }
            catch (Exception ex)
            {
                Log.WriteLog("Error: WhiteBlob_CL_Insp() 執行發生例外狀況 -> " + ex.ToString());
                return false;
            }

            return Res;
        }

        /// <summary>
        /// 檢測絲狀異物 (正光)
        /// </summary>
        /// <param name="Recipe"></param>
        /// <returns></returns>
        public bool Line_FL_Insp(ResistPanelRole Recipe)
        {
            bool Res = false;

            try
            {
                #region Execute Line_FL_Insp()

                // Initialize local and output iconic variables
                HOperatorSet.GenEmptyObj(out ho_DefectReg_Line);

                if (!Cell_BlackStripe_Insp(Recipe, Recipe.Param.InspParam_Line_FL, out ho_DefectReg_Line))
                    return false;

                // 旋轉瑕疵region回原本方向
                //if (Recipe.Param.Enabled_rotate)
                //{
                //    HOperatorSet.AffineTransRegion(ho_DefectReg_Line.Clone(), out ho_DefectReg_Line, hommat2d_origin_, "nearest_neighbor");
                //}

                // (20190807) Jeff Revised!
                if (Recipe.Param.B_AOI_absolNG) // 是否啟用AOI判斷絕對NG
                {
                    HObject Reg_absolNG;
                    Extension.HObjectMedthods.ReleaseHObject(ref reg_SelectGray_AbsolNG);
                    if (!AOI_absolNG_Insp(Recipe.Param.InspParam_Line_FL.cls_absolNG, Recipe.Param.InspParam_Line_FL.ImageIndex, ho_DefectReg_Line, out Reg_absolNG, out reg_SelectGray_AbsolNG))
                    {
                        Extension.HObjectMedthods.ReleaseHObject(ref Reg_absolNG);
                        Dict_AOI_absolNG["絲狀異物 (正光)"].DefectReg = ho_DefectReg_Line.Clone();
                        return false;
                    }
                    else
                    {
                        Dict_AOI_absolNG["絲狀異物 (正光)"].DefectReg = Reg_absolNG;

                        #region 加入絕對NG Region所在Cell之其他瑕疵Region

                        HObject Reg_absolNG_Cell;
                        if (Recipe.Param.InspParam_Line_FL.cls_absolNG.Enabled)
                        {
                            HObject temp;
                            HTuple hv_area_InspRects_Defect = new HTuple(), hv_Greater = new HTuple(), hv_Index_defect = new HTuple();
                            HOperatorSet.Intersection(ho_InspRects_AllCells, Reg_absolNG, out temp);
                            HOperatorSet.RegionFeatures(temp, "area", out hv_area_InspRects_Defect);
                            HOperatorSet.TupleGreaterElem(hv_area_InspRects_Defect, 0.0, out hv_Greater);
                            HOperatorSet.TupleFind(hv_Greater, 1, out hv_Index_defect);

                            if ((int)(new HTuple(hv_Index_defect.TupleNotEqual(-1))) == 1)
                            {
                                hv_Index_defect = hv_Index_defect + 1;
                                HOperatorSet.SelectObj(ho_InspRects_AllCells, out Reg_absolNG_Cell, hv_Index_defect);
                            }
                            else
                                Reg_absolNG_Cell = Reg_absolNG.Clone();
                            temp.Dispose();
                        }
                        else
                            Reg_absolNG_Cell = Reg_absolNG.Clone();

                        #endregion
                        
                        {
                            HObject AOI_NG;
                            HOperatorSet.Difference(ho_DefectReg_Line, Reg_absolNG_Cell, out AOI_NG);
                            Dict_AOI_NG["絲狀異物 (正光)"].DefectReg = AOI_NG;
                        }
                        Reg_absolNG_Cell.Dispose();
                    }
                }
                else
                {
                    Dict_AOI_absolNG["絲狀異物 (正光)"].DefectReg = ho_DefectReg_Line.Clone();
                }

                #endregion

                Res = true;
            }
            catch (Exception ex)
            {
                Log.WriteLog("Error: Line_FL_Insp() 執行發生例外狀況 -> " + ex.ToString());
                return false;
            }

            return Res;
        }

        /// <summary>
        /// 檢測絲狀異物 (正光&同軸光): 考慮 檢測絲狀異物 (正光) 及 檢測黑條內黑點2 (同軸光) 之結果
        /// </summary>
        /// <param name="Recipe"></param>
        /// <returns></returns>
        public bool Line_FLCL_Insp(ResistPanelRole Recipe)
        {
            bool Res = false;

            try
            {
                #region Execute Line_FLCL_Insp()

                if (!(Line_FL_Insp(Recipe)) || !(BlackBlob2_CL_Insp(Recipe))) return false;

                if (ho_DefectReg_Line == null || ho_DefectReg_BlackBlob2 == null) return false;
                
                HOperatorSet.Union1(ho_DefectReg_Line, out ho_DefectReg1_union);
                HOperatorSet.Union1(ho_DefectReg_BlackBlob2, out ho_DefectReg2_union);

                // Initialize local and output iconic variables
                HOperatorSet.GenEmptyObj(out ho_DefectReg_Line_FLCL);

                // 邏輯操作
                string str_op1 = Recipe.Param.InspParam_Line_FLCL.str_op1_2Images;
                switch (str_op1)
                {
                    case "and":
                        {
                            HOperatorSet.Intersection(ho_DefectReg1_union, ho_DefectReg2_union, out ho_DefectReg_op1_2Images);
                        }
                        break;
                    case "or":
                        {
                            HOperatorSet.Union2(ho_DefectReg1_union, ho_DefectReg2_union, out ho_DefectReg_op1_2Images);
                        }
                        break;
                    case "not1":
                        {
                            HOperatorSet.Difference(ho_DefectReg2_union, ho_DefectReg1_union, out ho_DefectReg_op1_2Images);
                        }
                        break;
                    case "not2":
                        {
                            HOperatorSet.Difference(ho_DefectReg1_union, ho_DefectReg2_union, out ho_DefectReg_op1_2Images);
                        }
                        break;
                }

                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Connection(ho_DefectReg_op1_2Images, out ExpTmpOutVar_0);
                    ho_DefectReg_op1_2Images.Dispose();
                    ho_DefectReg_op1_2Images = ExpTmpOutVar_0;
                }
                


                /* 瑕疵尺寸篩選 */
                // μm 轉 pixel
                hv_MinHeight_defect = Recipe.Param.InspParam_Line_FLCL.MinHeight_defect / Locate_Method_FS.pixel_resolution_;
                hv_MaxHeight_defect = Recipe.Param.InspParam_Line_FLCL.MaxHeight_defect / Locate_Method_FS.pixel_resolution_;
                hv_MinWidth_defect = Recipe.Param.InspParam_Line_FLCL.MinWidth_defect / Locate_Method_FS.pixel_resolution_;
                hv_MaxWidth_defect = Recipe.Param.InspParam_Line_FLCL.MaxWidth_defect / Locate_Method_FS.pixel_resolution_;
                hv_MinArea_defect = Recipe.Param.InspParam_Line_FLCL.MinArea_defect / (Locate_Method_FS.pixel_resolution_ * Locate_Method_FS.pixel_resolution_);
                hv_MaxArea_defect = Recipe.Param.InspParam_Line_FLCL.MaxArea_defect / (Locate_Method_FS.pixel_resolution_ * Locate_Method_FS.pixel_resolution_);

                // 操作1
                if (Recipe.Param.InspParam_Line_FLCL.count_op1 == 0)
                    HOperatorSet.CopyObj(ho_DefectReg_op1_2Images, out ho_DefectReg_op1, 1, -1);
                else
                {
                    // 規格1
                    if (Recipe.Param.InspParam_Line_FLCL.Enabled_Height && Recipe.Param.InspParam_Line_FLCL.Enabled_Width)
                    {
                        HOperatorSet.SelectShape(ho_DefectReg_op1_2Images, out ho_DefectReg_op1,
                                                 (new HTuple("height")).TupleConcat("width"), Recipe.Param.InspParam_Line_FLCL.str_op1,
                                                 hv_MinHeight_defect.TupleConcat(hv_MinWidth_defect),
                                                 hv_MaxHeight_defect.TupleConcat(hv_MaxWidth_defect));
                    }
                    else if (Recipe.Param.InspParam_Line_FLCL.Enabled_Height)
                    {
                        HOperatorSet.SelectShape(ho_DefectReg_op1_2Images, out ho_DefectReg_op1, "height",
                                                 Recipe.Param.InspParam_Line_FLCL.str_op1, hv_MinHeight_defect, hv_MaxHeight_defect);
                    }
                    else if (Recipe.Param.InspParam_Line_FLCL.Enabled_Width)
                    {
                        HOperatorSet.SelectShape(ho_DefectReg_op1_2Images, out ho_DefectReg_op1, "width",
                                                 Recipe.Param.InspParam_Line_FLCL.str_op1, hv_MinWidth_defect, hv_MaxWidth_defect);
                    }
                    else
                        HOperatorSet.CopyObj(ho_DefectReg_op1_2Images, out ho_DefectReg_op1, 1, -1);

                    // 規格2
                    if (Recipe.Param.InspParam_Line_FLCL.count_op1 == 2)
                    {
                        // μm 轉 pixel
                        HTuple hv_MinHeight_defect_op1_2 = Recipe.Param.InspParam_Line_FLCL.MinHeight_defect_op1_2 / Locate_Method_FS.pixel_resolution_;
                        HTuple hv_MaxHeight_defect_op1_2 = Recipe.Param.InspParam_Line_FLCL.MaxHeight_defect_op1_2 / Locate_Method_FS.pixel_resolution_;
                        HTuple hv_MinWidth_defect_op1_2 = Recipe.Param.InspParam_Line_FLCL.MinWidth_defect_op1_2 / Locate_Method_FS.pixel_resolution_;
                        HTuple hv_MaxWidth_defect_op1_2 = Recipe.Param.InspParam_Line_FLCL.MaxWidth_defect_op1_2 / Locate_Method_FS.pixel_resolution_;

                        HObject ho_DefectReg_op1_2 = null;
                        if (Recipe.Param.InspParam_Line_FLCL.Enabled_Height_op1_2 && Recipe.Param.InspParam_Line_FLCL.Enabled_Width_op1_2)
                        {
                            HOperatorSet.SelectShape(ho_DefectReg_op1_2Images, out ho_DefectReg_op1_2,
                                                     (new HTuple("height")).TupleConcat("width"), Recipe.Param.InspParam_Line_FLCL.str_op1_2,
                                                     hv_MinHeight_defect_op1_2.TupleConcat(hv_MinWidth_defect_op1_2),
                                                     hv_MaxHeight_defect_op1_2.TupleConcat(hv_MaxWidth_defect_op1_2));
                        }
                        else if (Recipe.Param.InspParam_Line_FLCL.Enabled_Height_op1_2)
                        {
                            HOperatorSet.SelectShape(ho_DefectReg_op1_2Images, out ho_DefectReg_op1_2, "height",
                                                     Recipe.Param.InspParam_Line_FLCL.str_op1_2, hv_MinHeight_defect_op1_2, hv_MaxHeight_defect_op1_2);
                        }
                        else if (Recipe.Param.InspParam_Line_FLCL.Enabled_Width_op1_2)
                        {
                            HOperatorSet.SelectShape(ho_DefectReg_op1_2Images, out ho_DefectReg_op1_2, "width",
                                                     Recipe.Param.InspParam_Line_FLCL.str_op1_2, hv_MinWidth_defect_op1_2, hv_MaxWidth_defect_op1_2);
                        }
                        else
                            HOperatorSet.CopyObj(ho_DefectReg_op1_2Images, out ho_DefectReg_op1_2, 1, -1);

                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_DefectReg_op1, ho_DefectReg_op1_2, out ExpTmpOutVar_0);
                            ho_DefectReg_op1.Dispose();
                            ho_DefectReg_op1 = ExpTmpOutVar_0;
                        }

                        ho_DefectReg_op1_2.Dispose();
                    }
                }

                // 操作2
                if (Recipe.Param.InspParam_Line_FLCL.Enabled_Area)
                {
                    HOperatorSet.SelectShape(ho_DefectReg_op1_2Images, out ho_DefectReg_op2, "area",
                                             Recipe.Param.InspParam_Line_FLCL.str_op2, hv_MinArea_defect, hv_MaxArea_defect);
                    HOperatorSet.Union1(ho_DefectReg_op1, out ho_DefectReg_op1_union);
                    HOperatorSet.Union1(ho_DefectReg_op2, out ho_DefectReg_op2_union);
                    if (Recipe.Param.InspParam_Line_FLCL.str_op2 == "and")
                        HOperatorSet.Intersection(ho_DefectReg_op1_union, ho_DefectReg_op2_union, out ho_DefectReg_op2_2Images);
                    else if (Recipe.Param.InspParam_Line_FLCL.str_op2 == "or")
                        HOperatorSet.Union2(ho_DefectReg_op1_union, ho_DefectReg_op2_union, out ho_DefectReg_op2_2Images);

                    // (20190331) Jeff Revised!
                    //{
                    //    HObject ExpTmpOutVar_0;
                    //    HOperatorSet.Connection(ho_DefectReg_op2_2Images, out ExpTmpOutVar_0);
                    //    ho_DefectReg_op2_2Images.Dispose();
                    //    ho_DefectReg_op2_2Images = ExpTmpOutVar_0;
                    //}
                }
                else
                    HOperatorSet.CopyObj(ho_DefectReg_op1, out ho_DefectReg_op2_2Images, 1, -1);

                /* 消除誤判區域 (20190417) Jeff Revised! */
                if (Recipe.Param.InspParam_Line_FLCL.Enabled_BypassReg)
                {
                    /* 設定區域 */
                    // μm 轉 pixel
                    HTuple hv_x_BypassReg = Recipe.Param.InspParam_Line_FLCL.x_BypassReg / Locate_Method_FS.pixel_resolution_;
                    HTuple hv_y_BypassReg = Recipe.Param.InspParam_Line_FLCL.y_BypassReg / Locate_Method_FS.pixel_resolution_;
                    HTuple hv_W_BypassReg = Recipe.Param.InspParam_Line_FLCL.W_BypassReg / Locate_Method_FS.pixel_resolution_;
                    HTuple hv_H_BypassReg = Recipe.Param.InspParam_Line_FLCL.H_BypassReg / Locate_Method_FS.pixel_resolution_;

                    HTuple hv_r_BypassReg = hv_r_AllCells + hv_y_BypassReg;
                    HTuple hv_c_BypassReg = hv_c_AllCells + hv_x_BypassReg;
                    HTuple hv_L1_BypassReg = new HTuple(), hv_L2_BypassReg = new HTuple();
                    HOperatorSet.TupleGenConst(hv_count_AllCells, 0.5 * hv_W_BypassReg, out hv_L1_BypassReg);
                    HOperatorSet.TupleGenConst(hv_count_AllCells, 0.5 * hv_H_BypassReg, out hv_L2_BypassReg);
                    HOperatorSet.GenRectangle2(out ho_Rect_BypassReg, hv_r_BypassReg, hv_c_BypassReg, hv_phi_AllCells, hv_L1_BypassReg, hv_L2_BypassReg);

                    /* 檢出規格設定3: 欲消除之瑕疵區域 */
                    HObject ho_DefectReg_BypassReg = null;
                    HOperatorSet.Intersection(ho_DefectReg_op2_2Images, ho_Rect_BypassReg, out ho_DefectReg_BypassReg);
                    {
                        HObject temp;
                        HOperatorSet.Connection(ho_DefectReg_BypassReg, out temp);
                        ho_DefectReg_BypassReg.Dispose();
                        ho_DefectReg_BypassReg = temp;
                    }

                    // 操作3
                    if (Recipe.Param.InspParam_Line_FLCL.count_op3 == 0)
                        HOperatorSet.CopyObj(ho_DefectReg_BypassReg, out ho_DefectReg_op3_2Images, 1, -1);
                    else
                    {
                        // μm 轉 pixel
                        hv_MinHeight_defect = Recipe.Param.InspParam_Line_FLCL.MinHeight_defect_op3_1 / Locate_Method_FS.pixel_resolution_;
                        hv_MaxHeight_defect = Recipe.Param.InspParam_Line_FLCL.MaxHeight_defect_op3_1 / Locate_Method_FS.pixel_resolution_;
                        hv_MinWidth_defect = Recipe.Param.InspParam_Line_FLCL.MinWidth_defect_op3_1 / Locate_Method_FS.pixel_resolution_;
                        hv_MaxWidth_defect = Recipe.Param.InspParam_Line_FLCL.MaxWidth_defect_op3_1 / Locate_Method_FS.pixel_resolution_;

                        // 規格1
                        if (Recipe.Param.InspParam_Line_FLCL.Enabled_Height_op3_1 && Recipe.Param.InspParam_Line_FLCL.Enabled_Width_op3_1)
                        {
                            HOperatorSet.SelectShape(ho_DefectReg_BypassReg, out ho_DefectReg_op3_2Images,
                                                     (new HTuple("height")).TupleConcat("width"), Recipe.Param.InspParam_Line_FLCL.str_op3_1,
                                                     hv_MinHeight_defect.TupleConcat(hv_MinWidth_defect),
                                                     hv_MaxHeight_defect.TupleConcat(hv_MaxWidth_defect));
                        }
                        else if (Recipe.Param.InspParam_Line_FLCL.Enabled_Height_op3_1)
                        {
                            HOperatorSet.SelectShape(ho_DefectReg_BypassReg, out ho_DefectReg_op3_2Images, "height",
                                                     Recipe.Param.InspParam_Line_FLCL.str_op3_1, hv_MinHeight_defect, hv_MaxHeight_defect);
                        }
                        else if (Recipe.Param.InspParam_Line_FLCL.Enabled_Width_op3_1)
                        {
                            HOperatorSet.SelectShape(ho_DefectReg_BypassReg, out ho_DefectReg_op3_2Images, "width",
                                                     Recipe.Param.InspParam_Line_FLCL.str_op3_1, hv_MinWidth_defect, hv_MaxWidth_defect);
                        }
                        else
                            HOperatorSet.CopyObj(ho_DefectReg_BypassReg, out ho_DefectReg_op3_2Images, 1, -1);

                        // 規格2
                        if (Recipe.Param.InspParam_Line_FLCL.count_op3 == 2)
                        {
                            // μm 轉 pixel
                            HTuple hv_MinHeight_defect_op3_2 = Recipe.Param.InspParam_Line_FLCL.MinHeight_defect_op3_2 / Locate_Method_FS.pixel_resolution_;
                            HTuple hv_MaxHeight_defect_op3_2 = Recipe.Param.InspParam_Line_FLCL.MaxHeight_defect_op3_2 / Locate_Method_FS.pixel_resolution_;
                            HTuple hv_MinWidth_defect_op3_2 = Recipe.Param.InspParam_Line_FLCL.MinWidth_defect_op3_2 / Locate_Method_FS.pixel_resolution_;
                            HTuple hv_MaxWidth_defect_op3_2 = Recipe.Param.InspParam_Line_FLCL.MaxWidth_defect_op3_2 / Locate_Method_FS.pixel_resolution_;

                            HObject ho_DefectReg_op3_2 = null;
                            if (Recipe.Param.InspParam_Line_FLCL.Enabled_Height_op3_2 && Recipe.Param.InspParam_Line_FLCL.Enabled_Width_op3_2)
                            {
                                HOperatorSet.SelectShape(ho_DefectReg_BypassReg, out ho_DefectReg_op3_2,
                                                         (new HTuple("height")).TupleConcat("width"), Recipe.Param.InspParam_Line_FLCL.str_op3_2,
                                                         hv_MinHeight_defect_op3_2.TupleConcat(hv_MinWidth_defect_op3_2),
                                                         hv_MaxHeight_defect_op3_2.TupleConcat(hv_MaxWidth_defect_op3_2));
                            }
                            else if (Recipe.Param.InspParam_Line_FLCL.Enabled_Height_op3_2)
                            {
                                HOperatorSet.SelectShape(ho_DefectReg_BypassReg, out ho_DefectReg_op3_2, "height",
                                                         Recipe.Param.InspParam_Line_FLCL.str_op3_2, hv_MinHeight_defect_op3_2, hv_MaxHeight_defect_op3_2);
                            }
                            else if (Recipe.Param.InspParam_Line_FLCL.Enabled_Width_op3_2)
                            {
                                HOperatorSet.SelectShape(ho_DefectReg_BypassReg, out ho_DefectReg_op3_2, "width",
                                                         Recipe.Param.InspParam_Line_FLCL.str_op3_2, hv_MinWidth_defect_op3_2, hv_MaxWidth_defect_op3_2);
                            }
                            else
                                HOperatorSet.CopyObj(ho_DefectReg_BypassReg, out ho_DefectReg_op3_2, 1, -1);

                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.ConcatObj(ho_DefectReg_op3_2Images, ho_DefectReg_op3_2, out ExpTmpOutVar_0);
                                ho_DefectReg_op3_2Images.Dispose();
                                ho_DefectReg_op3_2Images = ExpTmpOutVar_0;
                            }

                            ho_DefectReg_op3_2.Dispose();
                        }
                    }
                    ho_DefectReg_BypassReg.Dispose();

                    /* 得到最終瑕疵 */
                    HOperatorSet.Difference(ho_DefectReg_op2_2Images, ho_DefectReg_op3_2Images, out ho_DefectReg_Line_FLCL);

                }
                else
                {
                    HOperatorSet.CopyObj(ho_DefectReg_op2_2Images, out ho_DefectReg_Line_FLCL, 1, -1);
                    // Initialize local and output iconic variables
                    HOperatorSet.GenEmptyObj(out ho_Rect_BypassReg);
                    HOperatorSet.GenEmptyObj(out ho_DefectReg_op3_2Images);
                }

                // 旋轉瑕疵region回原本方向
                //if (Recipe.Param.Enabled_rotate)
                //{
                //    HOperatorSet.AffineTransRegion(ho_DefectReg_Line_FLCL.Clone(), out ho_DefectReg_Line_FLCL, hommat2d_origin_, "nearest_neighbor");
                //}

                // (20190807) Jeff Revised!
                if (Recipe.Param.B_AOI_absolNG) // 是否啟用AOI判斷絕對NG
                {
                    HObject Reg_absolNG;
                    Extension.HObjectMedthods.ReleaseHObject(ref reg_SelectGray_AbsolNG);
                    if (!AOI_absolNG_Insp(Recipe.Param.InspParam_Line_FLCL.cls_absolNG, -1, ho_DefectReg_Line_FLCL, out Reg_absolNG, out reg_SelectGray_AbsolNG))
                    {
                        Extension.HObjectMedthods.ReleaseHObject(ref Reg_absolNG);
                        Dict_AOI_absolNG["絲狀異物 (正光&同軸光)"].DefectReg = ho_DefectReg_Line_FLCL.Clone();
                        return false;
                    }
                    else
                    {
                        Dict_AOI_absolNG["絲狀異物 (正光&同軸光)"].DefectReg = Reg_absolNG;

                        #region 加入絕對NG Region所在Cell之其他瑕疵Region

                        HObject Reg_absolNG_Cell;
                        if (Recipe.Param.InspParam_Line_FLCL.cls_absolNG.Enabled)
                        {
                            HObject temp;
                            HTuple hv_area_InspRects_Defect = new HTuple(), hv_Greater = new HTuple(), hv_Index_defect = new HTuple();
                            HOperatorSet.Intersection(ho_InspRects_AllCells, Reg_absolNG, out temp);
                            HOperatorSet.RegionFeatures(temp, "area", out hv_area_InspRects_Defect);
                            HOperatorSet.TupleGreaterElem(hv_area_InspRects_Defect, 0.0, out hv_Greater);
                            HOperatorSet.TupleFind(hv_Greater, 1, out hv_Index_defect);

                            if ((int)(new HTuple(hv_Index_defect.TupleNotEqual(-1))) == 1)
                            {
                                hv_Index_defect = hv_Index_defect + 1;
                                HOperatorSet.SelectObj(ho_InspRects_AllCells, out Reg_absolNG_Cell, hv_Index_defect);
                            }
                            else
                                Reg_absolNG_Cell = Reg_absolNG.Clone();
                            temp.Dispose();
                        }
                        else
                            Reg_absolNG_Cell = Reg_absolNG.Clone();

                        #endregion
                        
                        {
                            HObject AOI_NG;
                            HOperatorSet.Difference(ho_DefectReg_Line_FLCL, Reg_absolNG_Cell, out AOI_NG);
                            Dict_AOI_NG["絲狀異物 (正光&同軸光)"].DefectReg = AOI_NG;
                        }
                        Reg_absolNG_Cell.Dispose();
                    }
                }
                else
                {
                    Dict_AOI_absolNG["絲狀異物 (正光&同軸光)"].DefectReg = ho_DefectReg_Line_FLCL.Clone();
                }

                #endregion

                Res = true;
            }
            catch (Exception ex)
            {
                Log.WriteLog("Error: Line_FLCL_Insp() 執行發生例外狀況 -> " + ex.ToString());
                return false;
            }

            return Res;
        }

        /// <summary>
        /// 檢測白條內黑點
        /// </summary>
        /// <param name="Recipe"></param>
        /// <param name="clsAOIParam_BlackCrack"></param>
        /// <param name="DefectReg_BlackCrack"></param>
        /// <returns></returns>
        public bool BlackCrack_Insp(ResistPanelRole Recipe, clsRecipe.clsAOIParam_BlackCrack clsAOIParam_BlackCrack, out HObject DefectReg_BlackCrack) // (20191122) Jeff Revised!
        {
            bool Res = false;
            // Initialize local and output iconic variables
            HOperatorSet.GenEmptyObj(out ho_Rect_AllCells);
            HOperatorSet.GenEmptyObj(out DefectReg_BlackCrack);

            try
            {
                #region Execute BlackCrack_Insp()

                /* 設定檢測ROI */
                // 電阻ROI
                if (clsAOIParam_BlackCrack.Enabled_BlackStripe)
                {
                    string str_ModeBlackStripe = clsAOIParam_BlackCrack.str_ModeBlackStripe;
                    if (str_ModeBlackStripe == "mode1")
                    {
                        // μm 轉 pixel
                        HTuple hv_H_BlackStripe = clsAOIParam_BlackCrack.H_BlackStripe / Locate_Method_FS.pixel_resolution_;

                        HOperatorSet.TupleGenConst(hv_count_BlackStripe, 0.5 * hv_H_BlackStripe, out hv_L2_BlackStripe);
                        if (!clsAOIParam_BlackCrack.Enabled_Wsmall) // (20190225) Jeff Revised!
                            HOperatorSet.GenRectangle2(out ho_Rect_BlackStripe, hv_r_BlackStripe, hv_c_BlackStripe, hv_Phi_BlackStripe, hv_L1_BlackStripe, hv_L2_BlackStripe);
                        else // (20190312) Jeff Revised!
                        {
                            HTuple L1 = new HTuple();
                            switch (clsAOIParam_BlackCrack.str_L_NonInspect)
                            {
                                case "1":
                                    {
                                        L1 = hv_L1_BlackStripe;
                                    }
                                    break;
                                case "2":
                                    {
                                        L1 = hv_L1_BlackStripe_Wsmall;
                                    }
                                    break;
                                case "3":
                                    {
                                        L1 = hv_L1_BlackStripe_Wsmall3;
                                    }
                                    break;
                            }
                            HOperatorSet.GenRectangle2(out ho_Rect_BlackStripe, hv_r_BlackStripe, hv_c_BlackStripe, hv_Phi_BlackStripe, L1, hv_L2_BlackStripe);
                        }
                    }
                    else if (str_ModeBlackStripe == "mode2") // (20191122) Jeff Revised!
                    {
                        int W_mor = 0, H_mor = 0, Type_mor = 0;
                        if (Recipe.Param.Type_BlackStripe == "Horizontal") // 黑條方向水平
                        {
                            W_mor = 1;
                            H_mor = Math.Abs(clsAOIParam_BlackCrack.H_BlackStripe_mode2);
                        }
                        else if (Recipe.Param.Type_BlackStripe == "Vertical") // 黑條方向垂直
                        {
                            H_mor = 1;
                            W_mor = Math.Abs(clsAOIParam_BlackCrack.H_BlackStripe_mode2);
                        }

                        if (clsAOIParam_BlackCrack.H_BlackStripe_mode2 > 0)
                            Type_mor = 1;
                        else if (clsAOIParam_BlackCrack.H_BlackStripe_mode2 < 0)
                            Type_mor = -1;

                        if (!clsAOIParam_BlackCrack.Enabled_Wsmall)
                        {
                            if (Type_mor > 0)
                                HOperatorSet.DilationRectangle1(ho_Regions_BlackStripe, out ho_Rect_BlackStripe, W_mor, H_mor);
                            else if (Type_mor < 0)
                                HOperatorSet.ErosionRectangle1(ho_Regions_BlackStripe, out ho_Rect_BlackStripe, W_mor, H_mor);
                            else
                                HOperatorSet.CopyObj(ho_Regions_BlackStripe, out ho_Rect_BlackStripe, 1, -1);
                        }
                        else
                        {
                            HObject Regions_BlackStripe = null;
                            switch (clsAOIParam_BlackCrack.str_L_NonInspect)
                            {
                                case "1":
                                    {
                                        HOperatorSet.CopyObj(ho_Regions_BlackStripe, out Regions_BlackStripe, 1, -1);
                                    }
                                    break;
                                case "2":
                                    {
                                        HOperatorSet.CopyObj(ho_Regions_BlackStripe_Wsmall, out Regions_BlackStripe, 1, -1);
                                    }
                                    break;
                                case "3":
                                    {
                                        HOperatorSet.CopyObj(ho_Regions_BlackStripe_Wsmall3, out Regions_BlackStripe, 1, -1);
                                    }
                                    break;
                            }

                            if (Type_mor > 0)
                                HOperatorSet.DilationRectangle1(Regions_BlackStripe, out ho_Rect_BlackStripe, W_mor, H_mor);
                            else if (Type_mor < 0)
                                HOperatorSet.ErosionRectangle1(Regions_BlackStripe, out ho_Rect_BlackStripe, W_mor, H_mor);
                            else
                                HOperatorSet.CopyObj(Regions_BlackStripe, out ho_Rect_BlackStripe, 1, -1);

                            Regions_BlackStripe.Dispose();
                        }
                    }
                }
                else
                    HOperatorSet.GenEmptyRegion(out ho_Rect_BlackStripe);



                /* 二值化灰階 */
                HObject Regions_Inspect = null;
                switch (clsAOIParam_BlackCrack.str_L_NonInspect)
                {
                    case "1":
                        {
                            HOperatorSet.CopyObj(ho_Regions_Inspect, out Regions_Inspect, 1, -1);
                        }
                        break;
                    case "2":
                        {
                            HOperatorSet.CopyObj(ho_Regions_Inspect1_Wsmall, out Regions_Inspect, 1, -1);
                        }
                        break;
                    case "3":
                        {
                            HOperatorSet.CopyObj(ho_Regions_Inspect1_Wsmall3, out Regions_Inspect, 1, -1);
                        }
                        break;
                }
                HOperatorSet.Difference(Regions_Inspect, ho_Rect_BlackStripe, out ho_Regions_WhiteStripe);
                Regions_Inspect.Dispose();
                HOperatorSet.ReduceDomain(insp_GrayImgList[clsAOIParam_BlackCrack.ImageIndex], ho_Regions_WhiteStripe, out ho_ImageReduced_WhiteStripe);
                HOperatorSet.Threshold(ho_ImageReduced_WhiteStripe, out ho_DefectReg_BlackCrack_thresh, clsAOIParam_BlackCrack.MinGray_BlackCrack, clsAOIParam_BlackCrack.MaxGray_BlackCrack);
                //{
                //    HObject ExpTmpOutVar_0;
                //    HOperatorSet.Connection(ho_DefectReg_BlackCrack_thresh, out ExpTmpOutVar_0);
                //    ho_DefectReg_BlackCrack_thresh.Dispose();
                //    ho_DefectReg_BlackCrack_thresh = ExpTmpOutVar_0;
                //}



                /* 設定檢測ROI 2 */
                // μm 轉 pixel
                HTuple hv_W_Cell = clsAOIParam_BlackCrack.W_Cell / Locate_Method_FS.pixel_resolution_;
                HTuple hv_H_Cell = clsAOIParam_BlackCrack.H_Cell / Locate_Method_FS.pixel_resolution_;
                // Cell ROI
                if (clsAOIParam_BlackCrack.Enabled_Cell)
                {
                    HOperatorSet.TupleGenConst(hv_count_AllCells, 0.5 * hv_W_Cell, out hv_L1_AllCells);
                    HOperatorSet.TupleGenConst(hv_count_AllCells, 0.5 * hv_H_Cell, out hv_L2_AllCells);
                    HOperatorSet.GenRectangle2(out ho_Rect_AllCells, hv_r_AllCells, hv_c_AllCells, hv_phi_AllCells, hv_L1_AllCells, hv_L2_AllCells);

                    if (clsAOIParam_BlackCrack.str_segMode == "ROI2")
                        HOperatorSet.Intersection(ho_Rect_AllCells, ho_DefectReg_BlackCrack_thresh, out ho_DefectReg_BlackCrack_CellReg);
                    else if (clsAOIParam_BlackCrack.str_segMode == "連通區域") // (20190529) Jeff Revised!
                        HOperatorSet.Intersection(ho_DefectReg_BlackCrack_thresh, ho_Rect_AllCells, out ho_DefectReg_BlackCrack_CellReg);

                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.Connection(ho_DefectReg_BlackCrack_CellReg, out ExpTmpOutVar_0);
                        ho_DefectReg_BlackCrack_CellReg.Dispose();
                        ho_DefectReg_BlackCrack_CellReg = ExpTmpOutVar_0;
                    }
                }
                else
                    HOperatorSet.Connection(ho_DefectReg_BlackCrack_thresh, out ho_DefectReg_BlackCrack_CellReg);


                /* 尺寸篩選 */
                // μm 轉 pixel
                hv_MinHeight_defect = clsAOIParam_BlackCrack.MinHeight_defect / Locate_Method_FS.pixel_resolution_;
                hv_MaxHeight_defect = clsAOIParam_BlackCrack.MaxHeight_defect / Locate_Method_FS.pixel_resolution_;
                hv_MinWidth_defect = clsAOIParam_BlackCrack.MinWidth_defect / Locate_Method_FS.pixel_resolution_;
                hv_MaxWidth_defect = clsAOIParam_BlackCrack.MaxWidth_defect / Locate_Method_FS.pixel_resolution_;
                hv_MinArea_defect = clsAOIParam_BlackCrack.MinArea_defect / (Locate_Method_FS.pixel_resolution_ * Locate_Method_FS.pixel_resolution_);
                hv_MaxArea_defect = clsAOIParam_BlackCrack.MaxArea_defect / (Locate_Method_FS.pixel_resolution_ * Locate_Method_FS.pixel_resolution_);

                //操作1
                if (clsAOIParam_BlackCrack.Enabled_Height && clsAOIParam_BlackCrack.Enabled_Width)
                {
                    HOperatorSet.SelectShape(ho_DefectReg_BlackCrack_CellReg, out ho_DefectReg_op1,
                                             (new HTuple("height")).TupleConcat("width"), clsAOIParam_BlackCrack.str_op1,
                                             hv_MinHeight_defect.TupleConcat(hv_MinWidth_defect),
                                             hv_MaxHeight_defect.TupleConcat(hv_MaxWidth_defect));
                }
                else if (clsAOIParam_BlackCrack.Enabled_Height)
                {
                    HOperatorSet.SelectShape(ho_DefectReg_BlackCrack_CellReg, out ho_DefectReg_op1, "height",
                                             clsAOIParam_BlackCrack.str_op1, hv_MinHeight_defect, hv_MaxHeight_defect);
                }
                else if (clsAOIParam_BlackCrack.Enabled_Width)
                {
                    HOperatorSet.SelectShape(ho_DefectReg_BlackCrack_CellReg, out ho_DefectReg_op1, "width",
                                             clsAOIParam_BlackCrack.str_op1, hv_MinWidth_defect, hv_MaxWidth_defect);
                }
                else
                    HOperatorSet.CopyObj(ho_DefectReg_BlackCrack_CellReg, out ho_DefectReg_op1, 1, -1);
                //操作2
                if (clsAOIParam_BlackCrack.Enabled_Area)
                {
                    HOperatorSet.SelectShape(ho_DefectReg_BlackCrack_CellReg, out ho_DefectReg_op2, "area",
                                             clsAOIParam_BlackCrack.str_op2, hv_MinArea_defect, hv_MaxArea_defect);
                    HOperatorSet.Union1(ho_DefectReg_op1, out ho_DefectReg_op1_union);
                    HOperatorSet.Union1(ho_DefectReg_op2, out ho_DefectReg_op2_union);
                    if (clsAOIParam_BlackCrack.str_op2 == "and")
                        HOperatorSet.Intersection(ho_DefectReg_op1_union, ho_DefectReg_op2_union, out DefectReg_BlackCrack);
                    else if (clsAOIParam_BlackCrack.str_op2 == "or")
                        HOperatorSet.Union2(ho_DefectReg_op1_union, ho_DefectReg_op2_union, out DefectReg_BlackCrack);

                    // (20190331) Jeff Revised!
                    //{
                    //    HObject ExpTmpOutVar_0;
                    //    HOperatorSet.Connection(DefectReg_BlackCrack, out ExpTmpOutVar_0);
                    //    DefectReg_BlackCrack.Dispose();
                    //    DefectReg_BlackCrack = ExpTmpOutVar_0;
                    //}
                }
                else
                    HOperatorSet.CopyObj(ho_DefectReg_op1, out DefectReg_BlackCrack, 1, -1);

                Res = true;

                #endregion
            }
            catch (Exception ex)
            {
                Log.WriteLog("Error: BlackCrack_Insp() 執行發生例外狀況 -> " + ex.ToString());
                return false;
            }

            return Res;
        }

        /// <summary>
        /// 檢測白條內黑點 (正光)
        /// </summary>
        /// <param name="Recipe"></param>
        /// <returns></returns>
        public bool BlackCrack_FL_Insp(ResistPanelRole Recipe) // (20190215) Jeff Revised!
        {
            bool Res = false;

            try
            {
                #region Execute BlackCrack_FL_Insp()

                // Initialize local and output iconic variables
                HOperatorSet.GenEmptyObj(out ho_DefectReg_BlackCrack_FL);

                if (!BlackCrack_Insp(Recipe, Recipe.Param.InspParam_BlackCrack_FL, out ho_DefectReg_BlackCrack_FL))
                    return false;

                // 旋轉瑕疵region回原本方向
                //if (Recipe.Param.Enabled_rotate)
                //{
                //    HOperatorSet.AffineTransRegion(ho_DefectReg_BlackCrack_FL.Clone(), out ho_DefectReg_BlackCrack_FL, hommat2d_origin_, "nearest_neighbor");
                //}

                // (20190807) Jeff Revised!
                if (Recipe.Param.B_AOI_absolNG) // 是否啟用AOI判斷絕對NG
                {
                    HObject Reg_absolNG;
                    Extension.HObjectMedthods.ReleaseHObject(ref reg_SelectGray_AbsolNG);
                    if (!AOI_absolNG_Insp(Recipe.Param.InspParam_BlackCrack_FL.cls_absolNG, Recipe.Param.InspParam_BlackCrack_FL.ImageIndex, ho_DefectReg_BlackCrack_FL, out Reg_absolNG, out reg_SelectGray_AbsolNG))
                    {
                        Extension.HObjectMedthods.ReleaseHObject(ref Reg_absolNG);
                        Dict_AOI_absolNG["白條內黑點 (正光)"].DefectReg = ho_DefectReg_BlackCrack_FL.Clone();
                        return false;
                    }
                    else
                    {
                        Dict_AOI_absolNG["白條內黑點 (正光)"].DefectReg = Reg_absolNG;

                        #region 加入絕對NG Region所在Cell之其他瑕疵Region

                        HObject Reg_absolNG_Cell;
                        if (Recipe.Param.InspParam_BlackCrack_FL.cls_absolNG.Enabled)
                        {
                            HObject temp;
                            HTuple hv_area_InspRects_Defect = new HTuple(), hv_Greater = new HTuple(), hv_Index_defect = new HTuple();
                            HOperatorSet.Intersection(ho_InspRects_AllCells, Reg_absolNG, out temp);
                            HOperatorSet.RegionFeatures(temp, "area", out hv_area_InspRects_Defect);
                            HOperatorSet.TupleGreaterElem(hv_area_InspRects_Defect, 0.0, out hv_Greater);
                            HOperatorSet.TupleFind(hv_Greater, 1, out hv_Index_defect);

                            if ((int)(new HTuple(hv_Index_defect.TupleNotEqual(-1))) == 1)
                            {
                                hv_Index_defect = hv_Index_defect + 1;
                                HOperatorSet.SelectObj(ho_InspRects_AllCells, out Reg_absolNG_Cell, hv_Index_defect);
                            }
                            else
                                Reg_absolNG_Cell = Reg_absolNG.Clone();
                            temp.Dispose();
                        }
                        else
                            Reg_absolNG_Cell = Reg_absolNG.Clone();

                        #endregion
                        
                        {
                            HObject AOI_NG;
                            HOperatorSet.Difference(ho_DefectReg_BlackCrack_FL, Reg_absolNG_Cell, out AOI_NG);
                            Dict_AOI_NG["白條內黑點 (正光)"].DefectReg = AOI_NG;
                        }
                        Reg_absolNG_Cell.Dispose();
                    }
                }
                else
                {
                    Dict_AOI_absolNG["白條內黑點 (正光)"].DefectReg = ho_DefectReg_BlackCrack_FL.Clone();
                }

                #endregion

                Res = true;
            }
            catch (Exception ex)
            {
                Log.WriteLog("Error: BlackCrack_FL_Insp() 執行發生例外狀況 -> " + ex.ToString());
                return false;
            }

            return Res;
        }

        /// <summary>
        /// 檢測白條內黑點 (同軸光)
        /// </summary>
        /// <param name="Recipe"></param>
        /// <returns></returns>
        public bool BlackCrack_CL_Insp(ResistPanelRole Recipe) // (20190308) Jeff Revised!
        {
            bool Res = false;

            try
            {
                #region Execute BlackCrack_CL_Insp()

                // Initialize local and output iconic variables
                HOperatorSet.GenEmptyObj(out ho_DefectReg_BlackCrack_CL);

                if (!BlackCrack_Insp(Recipe, Recipe.Param.InspParam_BlackCrack_CL, out ho_DefectReg_BlackCrack_CL))
                    return false;

                // 旋轉瑕疵region回原本方向
                //if (Recipe.Param.Enabled_rotate)
                //{
                //    HOperatorSet.AffineTransRegion(ho_DefectReg_BlackCrack_CL.Clone(), out ho_DefectReg_BlackCrack_CL, hommat2d_origin_, "nearest_neighbor");
                //}

                // (20190807) Jeff Revised!
                if (Recipe.Param.B_AOI_absolNG) // 是否啟用AOI判斷絕對NG
                {
                    HObject Reg_absolNG;
                    Extension.HObjectMedthods.ReleaseHObject(ref reg_SelectGray_AbsolNG);
                    if (!AOI_absolNG_Insp(Recipe.Param.InspParam_BlackCrack_CL.cls_absolNG, Recipe.Param.InspParam_BlackCrack_CL.ImageIndex, ho_DefectReg_BlackCrack_CL, out Reg_absolNG, out reg_SelectGray_AbsolNG))
                    {
                        Extension.HObjectMedthods.ReleaseHObject(ref Reg_absolNG);
                        Dict_AOI_absolNG["白條內黑點 (同軸光)"].DefectReg = ho_DefectReg_BlackCrack_CL.Clone();
                        return false;
                    }
                    else
                    {
                        Dict_AOI_absolNG["白條內黑點 (同軸光)"].DefectReg = Reg_absolNG;

                        #region 加入絕對NG Region所在Cell之其他瑕疵Region

                        HObject Reg_absolNG_Cell;
                        if (Recipe.Param.InspParam_BlackCrack_CL.cls_absolNG.Enabled)
                        {
                            HObject temp;
                            HTuple hv_area_InspRects_Defect = new HTuple(), hv_Greater = new HTuple(), hv_Index_defect = new HTuple();
                            HOperatorSet.Intersection(ho_InspRects_AllCells, Reg_absolNG, out temp);
                            HOperatorSet.RegionFeatures(temp, "area", out hv_area_InspRects_Defect);
                            HOperatorSet.TupleGreaterElem(hv_area_InspRects_Defect, 0.0, out hv_Greater);
                            HOperatorSet.TupleFind(hv_Greater, 1, out hv_Index_defect);

                            if ((int)(new HTuple(hv_Index_defect.TupleNotEqual(-1))) == 1)
                            {
                                hv_Index_defect = hv_Index_defect + 1;
                                HOperatorSet.SelectObj(ho_InspRects_AllCells, out Reg_absolNG_Cell, hv_Index_defect);
                            }
                            else
                                Reg_absolNG_Cell = Reg_absolNG.Clone();
                            temp.Dispose();
                        }
                        else
                            Reg_absolNG_Cell = Reg_absolNG.Clone();

                        #endregion
                        
                        {
                            HObject AOI_NG;
                            HOperatorSet.Difference(ho_DefectReg_BlackCrack_CL, Reg_absolNG_Cell, out AOI_NG);
                            Dict_AOI_NG["白條內黑點 (同軸光)"].DefectReg = AOI_NG;
                        }
                        Reg_absolNG_Cell.Dispose();
                    }
                }
                else
                {
                    Dict_AOI_absolNG["白條內黑點 (同軸光)"].DefectReg = ho_DefectReg_BlackCrack_CL.Clone();
                }

                #endregion

                Res = true;
            }
            catch (Exception ex)
            {
                Log.WriteLog("Error: BlackCrack_CL_Insp() 執行發生例外狀況 -> " + ex.ToString());
                return false;
            }

            return Res;
        }

        /// <summary>
        /// 檢測黑條內黑點1 (同軸光)
        /// </summary>
        /// <param name="Recipe"></param>
        /// <returns></returns>
        public bool BlackBlob_CL_Insp(ResistPanelRole Recipe) // (20190215) Jeff Revised!
        {
            bool Res = false;

            try
            {
                #region Execute BlackBlob_CL_Insp()

                // Initialize local and output iconic variables
                HOperatorSet.GenEmptyObj(out ho_DefectReg_BlackBlob);

                //* 大顆 Black Blob: 速度快，但容易誤判黑條邊緣
                HOperatorSet.ReduceDomain(ho_GrayImage_CL, ho_Regions_Inspect, out ho_ImageReduced_Inspect_CL);
                //檢測 Black Blob
                HOperatorSet.Threshold(ho_ImageReduced_Inspect_CL, out ho_Region_BigBlob_thresh_CL, 0, Recipe.Param.InspParam_BlackBlob_CL.MaxGray_BigBlackBlob_CL);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Connection(ho_Region_BigBlob_thresh_CL, out ExpTmpOutVar_0);
                    ho_Region_BigBlob_thresh_CL.Dispose();
                    ho_Region_BigBlob_thresh_CL = ExpTmpOutVar_0;
                }
                //ho_DefectReg_BigBlackBlob.Dispose();
                //HOperatorSet.SelectShape(ho_Region_BigBlob_thresh_CL, out ho_DefectReg_BigBlackBlob, "area", "and", Recipe.Param.InspParam_BlackBlob_CL.MinArea_BigBlackBlob_CL, Recipe.Param.InspParam_BlackBlob_CL.MaxArea_BigBlackBlob_CL);
                // 尺寸篩選 (20190126) Jeff Revised!
                // double 轉 HTuple
                hv_MinHeight_defect = Recipe.Param.InspParam_BlackBlob_CL.MinHeight_defect / Locate_Method_FS.pixel_resolution_;
                hv_MaxHeight_defect = Recipe.Param.InspParam_BlackBlob_CL.MaxHeight_defect / Locate_Method_FS.pixel_resolution_;
                hv_MinWidth_defect = Recipe.Param.InspParam_BlackBlob_CL.MinWidth_defect / Locate_Method_FS.pixel_resolution_;
                hv_MaxWidth_defect = Recipe.Param.InspParam_BlackBlob_CL.MaxWidth_defect / Locate_Method_FS.pixel_resolution_;
                HOperatorSet.SelectShape(ho_Region_BigBlob_thresh_CL, out ho_DefectReg_BigBlackBlob,
                                         (new HTuple("height")).TupleConcat("width"), "and",
                                         ((hv_MinHeight_defect)).TupleConcat(hv_MinWidth_defect),
                                         ((hv_MaxHeight_defect)).TupleConcat(hv_MaxWidth_defect));



                //* 小顆 Black Blob: 速度慢，但大大降低誤判黑條邊緣機率
                //縮放影像大小，可以大大降低 FFT 和 IFFT 運算時間
                //ho_GrayImage_CL_Zoomed.Dispose();
                HOperatorSet.ZoomImageFactor(ho_GrayImage_CL, out ho_GrayImage_CL_Zoomed, 1.0 / Recipe.Param.InspParam_BlackBlob_CL.Zoom_DownScale, 1.0 / Recipe.Param.InspParam_BlackBlob_CL.Zoom_DownScale, "bilinear");

                //FFT
                //ho_ImageFFT.Dispose();
                HOperatorSet.FftImage(ho_GrayImage_CL_Zoomed, out ho_ImageFFT);

                //Create filter
                hv_Width_Zoomed = hv_Width / Recipe.Param.InspParam_BlackBlob_CL.Zoom_DownScale;
                hv_Height_Zoomed = hv_Height / Recipe.Param.InspParam_BlackBlob_CL.Zoom_DownScale;
                hv_center_col = hv_Width_Zoomed / 2;
                hv_center_row = hv_Height_Zoomed / 2;
                //ho_ImageMask.Dispose();
                HOperatorSet.GenImageConst(out ho_ImageMask, "real", hv_Width_Zoomed, hv_Height_Zoomed);
                //去除水平線
                //ho_RegionUnion_y.Dispose();
                HOperatorSet.GenEmptyRegion(out ho_RegionUnion_y);
                if (Recipe.Param.InspParam_BlackBlob_CL.Enabled_Reduce_Hor)
                {
                    //ho_Rectangle1.Dispose();
                    HOperatorSet.GenRectangle1(out ho_Rectangle1, 0, hv_center_col - (0.5 * Recipe.Param.InspParam_BlackBlob_CL.W_Filter_FFT_y), hv_center_row - Recipe.Param.InspParam_BlackBlob_CL.Shift_FFT_y, hv_center_col + (0.5 * Recipe.Param.InspParam_BlackBlob_CL.W_Filter_FFT_y));
                    //ho_Rectangle2.Dispose();
                    HOperatorSet.GenRectangle1(out ho_Rectangle2, hv_center_row + Recipe.Param.InspParam_BlackBlob_CL.Shift_FFT_y, hv_center_col - (0.5 * Recipe.Param.InspParam_BlackBlob_CL.W_Filter_FFT_y), hv_Height_Zoomed - 1, hv_center_col + (0.5 * Recipe.Param.InspParam_BlackBlob_CL.W_Filter_FFT_y));
                    ho_RegionUnion_y.Dispose();
                    HOperatorSet.Union2(ho_Rectangle1, ho_Rectangle2, out ho_RegionUnion_y);
                }
                //去除垂直線
                //ho_RegionUnion_x.Dispose();
                HOperatorSet.GenEmptyRegion(out ho_RegionUnion_x);
                if (Recipe.Param.InspParam_BlackBlob_CL.Enabled_Reduce_Ver)
                {
                    //ho_Rectangle3.Dispose();
                    HOperatorSet.GenRectangle1(out ho_Rectangle3, hv_center_row - (0.5 * Recipe.Param.InspParam_BlackBlob_CL.W_Filter_FFT_x), 0, hv_center_row + (0.5 * Recipe.Param.InspParam_BlackBlob_CL.W_Filter_FFT_x), hv_center_col - Recipe.Param.InspParam_BlackBlob_CL.Shift_FFT_x);
                    //ho_Rectangle4.Dispose();
                    HOperatorSet.GenRectangle1(out ho_Rectangle4, hv_center_row - (0.5 * Recipe.Param.InspParam_BlackBlob_CL.W_Filter_FFT_x), hv_center_col + Recipe.Param.InspParam_BlackBlob_CL.Shift_FFT_x, hv_center_row + (0.5 * Recipe.Param.InspParam_BlackBlob_CL.W_Filter_FFT_x), hv_Width_Zoomed - 1);
                    ho_RegionUnion_x.Dispose();
                    HOperatorSet.Union2(ho_Rectangle3, ho_Rectangle4, out ho_RegionUnion_x);
                }
                //結合兩者
                //ho_RegionUnion.Dispose();
                HOperatorSet.Union2(ho_RegionUnion_y, ho_RegionUnion_x, out ho_RegionUnion);

                //ho_RegionComplement.Dispose();
                HOperatorSet.Complement(ho_RegionUnion, out ho_RegionComplement);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.PaintRegion(ho_RegionComplement, ho_ImageMask, out ExpTmpOutVar_0, 1.0, "fill");
                    ho_ImageMask.Dispose();
                    ho_ImageMask = ExpTmpOutVar_0;
                }

                //IFFT
                //ho_Filtered.Dispose();
                HOperatorSet.ConvolFft(ho_ImageFFT, ho_ImageMask, out ho_Filtered);
                //ho_ImageFFTInv.Dispose();
                HOperatorSet.FftImageInv(ho_Filtered, out ho_ImageFFTInv);

                //檢測 Black Blob
                //HOperatorSet.ZoomRegion(ho_Regions_Inspect, out ho_Regions_Inspect_Zoomed, 1.0 / Recipe.Param.InspParam_BlackBlob_CL.Zoom_DownScale, 1.0 / Recipe.Param.InspParam_BlackBlob_CL.Zoom_DownScale);               
                //HOperatorSet.ReduceDomain(ho_ImageFFTInv, ho_Regions_Inspect_Zoomed, out ho_ImageReduced_FFTInv_CL);
                HOperatorSet.ZoomRegion(ho_Regions_BlackStripe_small, out ho_Regions_BlackStripe_Zoomed, 1.0 / Recipe.Param.InspParam_BlackBlob_CL.Zoom_DownScale, 1.0 / Recipe.Param.InspParam_BlackBlob_CL.Zoom_DownScale); // (20190126) Jeff Revised!
                HOperatorSet.ReduceDomain(ho_ImageFFTInv, ho_Regions_BlackStripe_Zoomed, out ho_ImageReduced_FFTInv_CL); // (20190126) Jeff Revised!

                //ho_Region_Blob_thresh_CL.Dispose();
                HOperatorSet.Threshold(ho_ImageReduced_FFTInv_CL, out ho_Region_Blob_thresh_CL, 0, Recipe.Param.InspParam_BlackBlob_CL.MaxGray_BlackBlob_CL);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Connection(ho_Region_Blob_thresh_CL, out ExpTmpOutVar_0);
                    ho_Region_Blob_thresh_CL.Dispose();
                    ho_Region_Blob_thresh_CL = ExpTmpOutVar_0;
                }

                /* 尺寸篩選 */
                // double 轉 HTuple
                hv_MinHeight_defect = Recipe.Param.InspParam_BlackBlob_CL.MinHeight_defect / Locate_Method_FS.pixel_resolution_;
                hv_MaxHeight_defect = Recipe.Param.InspParam_BlackBlob_CL.MaxHeight_defect / Locate_Method_FS.pixel_resolution_;
                hv_MinWidth_defect = Recipe.Param.InspParam_BlackBlob_CL.MinWidth_defect / Locate_Method_FS.pixel_resolution_;
                hv_MaxWidth_defect = Recipe.Param.InspParam_BlackBlob_CL.MaxWidth_defect / Locate_Method_FS.pixel_resolution_;
                //ho_DefectReg_BlackBlob_Zoomed.Dispose();
                HOperatorSet.SelectShape(ho_Region_Blob_thresh_CL, out ho_DefectReg_BlackBlob_select,
                                         (new HTuple("height")).TupleConcat("width"), "and",
                                         ((hv_MinHeight_defect / Recipe.Param.InspParam_BlackBlob_CL.Zoom_DownScale)).TupleConcat(hv_MinWidth_defect / Recipe.Param.InspParam_BlackBlob_CL.Zoom_DownScale),
                                         ((hv_MaxHeight_defect / Recipe.Param.InspParam_BlackBlob_CL.Zoom_DownScale)).TupleConcat(hv_MaxWidth_defect / Recipe.Param.InspParam_BlackBlob_CL.Zoom_DownScale));

                // 瑕疵Region內至少有一個pixel之灰階值低於一個數值 (20190126) Jeff Revised!
                HOperatorSet.SelectGray(ho_DefectReg_BlackBlob_select, ho_ImageReduced_FFTInv_CL, out ho_DefectReg_BlackBlob_Zoomed, "min", "and", 0, Recipe.Param.InspParam_BlackBlob_CL.Gray_Dark_CL);

                // Convert defect regions to the original image size
                //ho_DefectReg_SmallBlackBlob.Dispose();
                HOperatorSet.ZoomRegion(ho_DefectReg_BlackBlob_Zoomed, out ho_DefectReg_SmallBlackBlob, Recipe.Param.InspParam_BlackBlob_CL.Zoom_DownScale, Recipe.Param.InspParam_BlackBlob_CL.Zoom_DownScale);

                //* 結合 大/小顆 Black Blob
                //ho_DefectReg_BlackBlob.Dispose();
                HOperatorSet.Union2(ho_DefectReg_BigBlackBlob, ho_DefectReg_SmallBlackBlob, out ho_DefectReg_BlackBlob);

                // 旋轉瑕疵region回原本方向
                //if (Recipe.Param.Enabled_rotate)
                //{
                //    HOperatorSet.AffineTransRegion(ho_DefectReg_BlackBlob.Clone(), out ho_DefectReg_BlackBlob, hommat2d_origin_, "nearest_neighbor");
                //}

                //if (MOVE_ID == "3") // debug (20190126) Jeff Revised!
                //{
                //    HOperatorSet.WriteRegion(ho_DefectReg_BlackBlob, "DefectReg_BlackBlob.hobj");
                //}

                // (20190807) Jeff Revised!
                if (Recipe.Param.B_AOI_absolNG) // 是否啟用AOI判斷絕對NG
                {
                    HObject Reg_absolNG;
                    Extension.HObjectMedthods.ReleaseHObject(ref reg_SelectGray_AbsolNG);
                    if (!AOI_absolNG_Insp(Recipe.Param.InspParam_BlackBlob_CL.cls_absolNG, Recipe.Param.InspParam_BlackBlob_CL.ImageIndex, ho_DefectReg_BlackBlob, out Reg_absolNG, out reg_SelectGray_AbsolNG))
                    {
                        Extension.HObjectMedthods.ReleaseHObject(ref Reg_absolNG);
                        Dict_AOI_absolNG["BlackBlob (同軸光)"].DefectReg = ho_DefectReg_BlackBlob.Clone();
                        return false;
                    }
                    else
                    {
                        Dict_AOI_absolNG["BlackBlob (同軸光)"].DefectReg = Reg_absolNG;

                        #region 加入絕對NG Region所在Cell之其他瑕疵Region

                        HObject Reg_absolNG_Cell;
                        if (Recipe.Param.InspParam_BlackBlob_CL.cls_absolNG.Enabled)
                        {
                            HObject temp;
                            HTuple hv_area_InspRects_Defect = new HTuple(), hv_Greater = new HTuple(), hv_Index_defect = new HTuple();
                            HOperatorSet.Intersection(ho_InspRects_AllCells, Reg_absolNG, out temp);
                            HOperatorSet.RegionFeatures(temp, "area", out hv_area_InspRects_Defect);
                            HOperatorSet.TupleGreaterElem(hv_area_InspRects_Defect, 0.0, out hv_Greater);
                            HOperatorSet.TupleFind(hv_Greater, 1, out hv_Index_defect);

                            if ((int)(new HTuple(hv_Index_defect.TupleNotEqual(-1))) == 1)
                            {
                                hv_Index_defect = hv_Index_defect + 1;
                                HOperatorSet.SelectObj(ho_InspRects_AllCells, out Reg_absolNG_Cell, hv_Index_defect);
                            }
                            else
                                Reg_absolNG_Cell = Reg_absolNG.Clone();
                            temp.Dispose();
                        }
                        else
                            Reg_absolNG_Cell = Reg_absolNG.Clone();

                        #endregion
                        
                        {
                            HObject AOI_NG;
                            HOperatorSet.Difference(ho_DefectReg_BlackBlob, Reg_absolNG_Cell, out AOI_NG);
                            Dict_AOI_NG["BlackBlob (同軸光)"].DefectReg = AOI_NG;
                        }
                        Reg_absolNG_Cell.Dispose();
                    }
                }
                else
                {
                    Dict_AOI_absolNG["BlackBlob (同軸光)"].DefectReg = ho_DefectReg_BlackBlob.Clone();
                }

                #endregion

                Res = true;
            }
            catch (Exception ex)
            {
                Log.WriteLog("Error: BlackBlob_CL_Insp() 執行發生例外狀況 -> " + ex.ToString());
                return false;
            }

            return Res;
        }

        /// <summary>
        /// 計算該瑕疵之絕對NG Region
        /// </summary>
        /// <param name="cls_absolNG"></param>
        /// <param name="imageIndex"></param>
        /// <param name="Reg_defect"></param>
        /// <param name="Reg_absolNG"></param>
        /// <param name="Reg_SelectGray"></param>
        /// <returns></returns>
        public bool AOI_absolNG_Insp(clsRecipe.clsAOIParam_absolNG cls_absolNG, int imageIndex, HObject Reg_defect, out HObject Reg_absolNG, out HObject Reg_SelectGray) // (20190806) Jeff Revised!
        {
            bool Res = false;

            try
            {
                if (!cls_absolNG.Enabled) // 不啟用
                {
                    //Reg_absolNG = Reg_defect.Clone();
                    //Reg_SelectGray = Reg_defect.Clone();
                    HOperatorSet.GenEmptyObj(out Reg_absolNG);
                    HOperatorSet.GenEmptyObj(out Reg_SelectGray);
                    return true;
                }

                #region 瑕疵灰階標準

                if (cls_absolNG.Enabled_Gray_select && imageIndex >= 0)
                {
                    HOperatorSet.Connection(Reg_defect, out Reg_SelectGray);
                    {
                        HObject ExpTmpOutVar_0;
                        if (cls_absolNG.str_Gray_select == "min")
                            HOperatorSet.SelectGray(Reg_SelectGray, insp_GrayImgList[imageIndex], out ExpTmpOutVar_0, "min", "and", 0, cls_absolNG.Gray_select);
                        else
                            HOperatorSet.SelectGray(Reg_SelectGray, insp_GrayImgList[imageIndex], out ExpTmpOutVar_0, "max", "and", cls_absolNG.Gray_select, 255);

                        Reg_SelectGray.Dispose();
                        Reg_SelectGray = ExpTmpOutVar_0;
                    }
                }
                else
                    Reg_SelectGray = Reg_defect.Clone();

                #endregion

                #region 瑕疵尺寸篩選

                if (cls_absolNG.Enabled_Area)
                {
                    // μm 轉 pixel
                    HTuple MinArea_defect = cls_absolNG.MinArea_defect / (Locate_Method_FS.pixel_resolution_ * Locate_Method_FS.pixel_resolution_);
                    HTuple MaxArea_defect = cls_absolNG.MaxArea_defect / (Locate_Method_FS.pixel_resolution_ * Locate_Method_FS.pixel_resolution_);

                    if (cls_absolNG.str_op == "and")
                    {
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.Connection(Reg_SelectGray, out ExpTmpOutVar_0);
                            Reg_SelectGray.Dispose();
                            Reg_SelectGray = ExpTmpOutVar_0;
                        }
                        HOperatorSet.SelectShape(Reg_SelectGray, out Reg_absolNG, "area", cls_absolNG.str_op, MinArea_defect, MaxArea_defect);
                    }
                    else // or
                    {
                        HObject Reg_SelectArea;
                        HOperatorSet.Connection(Reg_defect, out Reg_SelectArea);
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.SelectShape(Reg_SelectArea, out ExpTmpOutVar_0, "area", cls_absolNG.str_op, MinArea_defect, MaxArea_defect);
                            Reg_SelectArea.Dispose();
                            Reg_SelectArea = ExpTmpOutVar_0;
                        }

                        HObject Reg_SelectGray_union, Reg_SelectArea_union;
                        HOperatorSet.Union1(Reg_SelectGray, out Reg_SelectGray_union);
                        HOperatorSet.Union1(Reg_SelectArea, out Reg_SelectArea_union);
                        HOperatorSet.Union2(Reg_SelectGray_union, Reg_SelectArea_union, out Reg_absolNG);

                        Reg_SelectGray_union.Dispose();
                        Reg_SelectArea_union.Dispose();
                        Reg_SelectArea.Dispose();
                    }

                }
                else
                    Reg_absolNG = Reg_SelectGray.Clone();

                #endregion
                
                Res = true;
            }
            catch (Exception ex)
            {
                //Reg_absolNG = Reg_defect.Clone();
                //Reg_SelectGray = Reg_defect.Clone();
                HOperatorSet.GenEmptyObj(out Reg_absolNG);
                HOperatorSet.GenEmptyObj(out Reg_SelectGray);
                Log.WriteLog("Error: AOI_absolNG_Insp() 執行發生例外狀況 -> " + ex.ToString());
                return false;
            }

            return Res;
        }

        public bool Cell_BlackStripe_Insp(ResistPanelRole Recipe, clsRecipe.clsAOIParam_Cell_BlackStripe clsAOIParam_Cell_BlackStripe, out HObject DefectReg_Cell_BlackStripe) // (20190805) Jeff Revised!
        {
            bool Res = false;
            // Initialize local and output iconic variables
            HOperatorSet.GenEmptyObj(out ho_Rect_AllCells);
            HOperatorSet.GenEmptyObj(out DefectReg_Cell_BlackStripe);

            try
            {
                #region Execute Cell_BlackStripe_Insp()
                
                #region 設定檢測ROI

                // μm 轉 pixel
                HTuple hv_W_Cell = clsAOIParam_Cell_BlackStripe.W_Cell / Locate_Method_FS.pixel_resolution_;
                HTuple hv_H_Cell = clsAOIParam_Cell_BlackStripe.H_Cell / Locate_Method_FS.pixel_resolution_;
                HTuple hv_x_BypassReg = clsAOIParam_Cell_BlackStripe.x_BypassReg / Locate_Method_FS.pixel_resolution_;
                HTuple hv_y_BypassReg = clsAOIParam_Cell_BlackStripe.y_BypassReg / Locate_Method_FS.pixel_resolution_;
                HTuple hv_W_BypassReg = clsAOIParam_Cell_BlackStripe.W_BypassReg / Locate_Method_FS.pixel_resolution_;
                HTuple hv_H_BypassReg = clsAOIParam_Cell_BlackStripe.H_BypassReg / Locate_Method_FS.pixel_resolution_;
                HTuple hv_H_BlackStripe = clsAOIParam_Cell_BlackStripe.H_BlackStripe / Locate_Method_FS.pixel_resolution_;
                // Cell ROI
                if (clsAOIParam_Cell_BlackStripe.Enabled_Cell)
                {
                    HOperatorSet.TupleGenConst(hv_count_AllCells, 0.5 * hv_W_Cell, out hv_L1_AllCells);
                    HOperatorSet.TupleGenConst(hv_count_AllCells, 0.5 * hv_H_Cell, out hv_L2_AllCells);
                    HOperatorSet.GenRectangle2(out ho_Rect_AllCells, hv_r_AllCells, hv_c_AllCells, hv_phi_AllCells, hv_L1_AllCells, hv_L2_AllCells);
                }
                else
                    HOperatorSet.GenEmptyRegion(out ho_Rect_AllCells);
                // Cell內忽略區域
                if (clsAOIParam_Cell_BlackStripe.Enabled_BypassReg)
                {
                    HTuple hv_r_BypassReg = hv_r_AllCells + hv_y_BypassReg;
                    HTuple hv_c_BypassReg = hv_c_AllCells + hv_x_BypassReg;
                    HTuple hv_L1_BypassReg = new HTuple(), hv_L2_BypassReg = new HTuple();
                    HOperatorSet.TupleGenConst(hv_count_AllCells, 0.5 * hv_W_BypassReg, out hv_L1_BypassReg);
                    HOperatorSet.TupleGenConst(hv_count_AllCells, 0.5 * hv_H_BypassReg, out hv_L2_BypassReg);
                    HOperatorSet.GenRectangle2(out ho_Rect_BypassReg, hv_r_BypassReg, hv_c_BypassReg, hv_phi_AllCells, hv_L1_BypassReg, hv_L2_BypassReg);
                }
                else
                    HOperatorSet.GenEmptyRegion(out ho_Rect_BypassReg);
                // 電阻ROI
                if (clsAOIParam_Cell_BlackStripe.Enabled_BlackStripe)
                {
                    string str_ModeBlackStripe = clsAOIParam_Cell_BlackStripe.str_ModeBlackStripe;
                    if (str_ModeBlackStripe == "mode1")
                    {
                        HOperatorSet.TupleGenConst(hv_count_BlackStripe, 0.5 * hv_H_BlackStripe, out hv_L2_BlackStripe);
                        if (!clsAOIParam_Cell_BlackStripe.Enabled_Wsmall) // (20190225) Jeff Revised!
                            HOperatorSet.GenRectangle2(out ho_Rect_BlackStripe, hv_r_BlackStripe, hv_c_BlackStripe, hv_Phi_BlackStripe, hv_L1_BlackStripe, hv_L2_BlackStripe);
                        else // (20190312) Jeff Revised!
                        {
                            HTuple L1 = new HTuple();
                            switch (clsAOIParam_Cell_BlackStripe.str_L_NonInspect)
                            {
                                case "1":
                                    {
                                        L1 = hv_L1_BlackStripe;
                                    }
                                    break;
                                case "2":
                                    {
                                        L1 = hv_L1_BlackStripe_Wsmall;
                                    }
                                    break;
                                case "3":
                                    {
                                        L1 = hv_L1_BlackStripe_Wsmall3;
                                    }
                                    break;
                            }
                            HOperatorSet.GenRectangle2(out ho_Rect_BlackStripe, hv_r_BlackStripe, hv_c_BlackStripe, hv_Phi_BlackStripe, L1, hv_L2_BlackStripe);
                        }
                    }
                    else if (str_ModeBlackStripe == "mode2") // (20191122) Jeff Revised!
                    {
                        int W_mor = 0, H_mor = 0, Type_mor = 0;
                        if (Recipe.Param.Type_BlackStripe == "Horizontal") // 黑條方向水平
                        {
                            W_mor = 1;
                            H_mor = Math.Abs(clsAOIParam_Cell_BlackStripe.H_BlackStripe_mode2);
                        }
                        else if (Recipe.Param.Type_BlackStripe == "Vertical") // 黑條方向垂直
                        {
                            H_mor = 1;
                            W_mor = Math.Abs(clsAOIParam_Cell_BlackStripe.H_BlackStripe_mode2);
                        }

                        if (clsAOIParam_Cell_BlackStripe.H_BlackStripe_mode2 > 0)
                            Type_mor = 1;
                        else if (clsAOIParam_Cell_BlackStripe.H_BlackStripe_mode2 < 0)
                            Type_mor = -1;

                        if (!clsAOIParam_Cell_BlackStripe.Enabled_Wsmall)
                        {
                            if (Type_mor > 0)
                                HOperatorSet.DilationRectangle1(ho_Regions_BlackStripe, out ho_Rect_BlackStripe, W_mor, H_mor);
                            else if (Type_mor < 0)
                                HOperatorSet.ErosionRectangle1(ho_Regions_BlackStripe, out ho_Rect_BlackStripe, W_mor, H_mor);
                            else
                                HOperatorSet.CopyObj(ho_Regions_BlackStripe, out ho_Rect_BlackStripe, 1, -1);
                        }
                        else
                        {
                            HObject Regions_BlackStripe = null;
                            switch (clsAOIParam_Cell_BlackStripe.str_L_NonInspect)
                            {
                                case "1":
                                    {
                                        HOperatorSet.CopyObj(ho_Regions_BlackStripe, out Regions_BlackStripe, 1, -1);
                                    }
                                    break;
                                case "2":
                                    {
                                        HOperatorSet.CopyObj(ho_Regions_BlackStripe_Wsmall, out Regions_BlackStripe, 1, -1);
                                    }
                                    break;
                                case "3":
                                    {
                                        HOperatorSet.CopyObj(ho_Regions_BlackStripe_Wsmall3, out Regions_BlackStripe, 1, -1);
                                    }
                                    break;
                            }

                            if (Type_mor > 0)
                                HOperatorSet.DilationRectangle1(Regions_BlackStripe, out ho_Rect_BlackStripe, W_mor, H_mor);
                            else if (Type_mor < 0)
                                HOperatorSet.ErosionRectangle1(Regions_BlackStripe, out ho_Rect_BlackStripe, W_mor, H_mor);
                            else
                                HOperatorSet.CopyObj(Regions_BlackStripe, out ho_Rect_BlackStripe, 1, -1);

                            Regions_BlackStripe.Dispose();
                        }
                    }
                }
                else
                    HOperatorSet.GenEmptyRegion(out ho_Rect_BlackStripe);

                #endregion
                
                #region 二值化處理

                // Cell ROI
                if (clsAOIParam_Cell_BlackStripe.Enabled_BypassReg)
                {
                    HOperatorSet.Difference(ho_Rect_AllCells, ho_Rect_BypassReg, out ho_RegDif_Cell);
                    HOperatorSet.Union1(ho_RegDif_Cell, out ho_Rect_AllCells_union);
                }
                else
                {
                    HOperatorSet.Union1(ho_Rect_AllCells, out ho_Rect_AllCells_union);
                }
                if (clsAOIParam_Cell_BlackStripe.Enabled_Wsmall) // (20190225) Jeff Revised!
                {
                    HObject Regions_Inspect0 = null;
                    switch (clsAOIParam_Cell_BlackStripe.str_L_NonInspect)
                    {
                        case "1":
                            {
                                HOperatorSet.CopyObj(ho_Regions_Inspect0, out Regions_Inspect0, 1, -1);
                            }
                            break;
                        case "2":
                            {
                                HOperatorSet.CopyObj(ho_Regions_Inspect0_Wsmall, out Regions_Inspect0, 1, -1);
                            }
                            break;
                        case "3":
                            {
                                HOperatorSet.CopyObj(ho_Regions_Inspect0_Wsmall3, out Regions_Inspect0, 1, -1);
                            }
                            break;
                    }

                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.Intersection(ho_Rect_AllCells_union, Regions_Inspect0, out ExpTmpOutVar_0);
                        ho_Rect_AllCells_union.Dispose();
                        ho_Rect_AllCells_union = ExpTmpOutVar_0;
                    }
                }
                HOperatorSet.ReduceDomain(insp_GrayImgList[clsAOIParam_Cell_BlackStripe.ImageIndex], ho_Rect_AllCells_union, out ho_ImageReduced_AllCells);
                switch (clsAOIParam_Cell_BlackStripe.str_BinaryImg_Cell)
                {
                    case "固定閥值":
                        {
                            HOperatorSet.Threshold(ho_ImageReduced_AllCells, out ho_Reg_Cell_Thresh, clsAOIParam_Cell_BlackStripe.MinGray_Cell, clsAOIParam_Cell_BlackStripe.MaxGray_Cell);
                            // 規格2 (20190411) Jeff Revised!
                            if (clsAOIParam_Cell_BlackStripe.count_BinImg_Cell == 2)
                            {
                                HObject ho_Reg_2_Cell_Thresh;
                                HOperatorSet.Threshold(ho_ImageReduced_AllCells, out ho_Reg_2_Cell_Thresh, clsAOIParam_Cell_BlackStripe.MinGray_2_Cell, clsAOIParam_Cell_BlackStripe.MaxGray_2_Cell);
                                {
                                    HObject ExpTmpOutVar_0;
                                    HOperatorSet.Union2(ho_Reg_Cell_Thresh, ho_Reg_2_Cell_Thresh, out ExpTmpOutVar_0);
                                    ho_Reg_Cell_Thresh.Dispose();
                                    ho_Reg_Cell_Thresh = ExpTmpOutVar_0;
                                }
                                ho_Reg_2_Cell_Thresh.Dispose();
                            }
                        }
                        break;
                    case "動態閥值":
                        {
                            HObject ImageMean = null;
                            HOperatorSet.MeanImage(ho_ImageReduced_AllCells, out ImageMean, clsAOIParam_Cell_BlackStripe.MaskWidth_Cell, clsAOIParam_Cell_BlackStripe.MaskHeight_Cell);
                            HOperatorSet.DynThreshold(ho_ImageReduced_AllCells, ImageMean, out ho_Reg_Cell_Thresh, clsAOIParam_Cell_BlackStripe.Offset_Cell, clsAOIParam_Cell_BlackStripe.str_LightDark_Cell);
                            ImageMean.Dispose();
                            // 規格2 (20190411) Jeff Revised!
                            if (clsAOIParam_Cell_BlackStripe.count_BinImg_Cell == 2)
                            {
                                HObject ho_Reg_2_Cell_Thresh;
                                HOperatorSet.MeanImage(ho_ImageReduced_AllCells, out ImageMean, clsAOIParam_Cell_BlackStripe.MaskWidth_2_Cell, clsAOIParam_Cell_BlackStripe.MaskHeight_2_Cell);
                                HOperatorSet.DynThreshold(ho_ImageReduced_AllCells, ImageMean, out ho_Reg_2_Cell_Thresh, clsAOIParam_Cell_BlackStripe.Offset_2_Cell, clsAOIParam_Cell_BlackStripe.str_LightDark_2_Cell);
                                ImageMean.Dispose();
                                {
                                    HObject ExpTmpOutVar_0;
                                    HOperatorSet.Union2(ho_Reg_Cell_Thresh, ho_Reg_2_Cell_Thresh, out ExpTmpOutVar_0);
                                    ho_Reg_Cell_Thresh.Dispose();
                                    ho_Reg_Cell_Thresh = ExpTmpOutVar_0;
                                }
                                ho_Reg_2_Cell_Thresh.Dispose();
                            }
                        }
                        break;
                }

                // 電阻ROI
                HOperatorSet.Union1(ho_Rect_BlackStripe, out ho_Rect_BlackStripe_union);
                HOperatorSet.Union1(ho_Rect_AllCells, out ho_Rect_AllCells_union1);
                HOperatorSet.Union1(ho_Rect_BypassReg, out ho_Rect_BypassReg_union);
                HOperatorSet.Difference(ho_Rect_BlackStripe_union, ho_Rect_AllCells_union1, out ho_RegDif_BlackStripe_temp);
                HOperatorSet.Difference(ho_RegDif_BlackStripe_temp, ho_Rect_BypassReg_union, out ho_RegDif_BlackStripe);
                HOperatorSet.ReduceDomain(insp_GrayImgList[clsAOIParam_Cell_BlackStripe.ImageIndex], ho_RegDif_BlackStripe, out ho_ImageReduced_BlackStripe);
                switch (clsAOIParam_Cell_BlackStripe.str_BinaryImg_Resist)
                {
                    case "固定閥值":
                        {
                            HOperatorSet.Threshold(ho_ImageReduced_BlackStripe, out ho_Reg_Resist_Thresh, clsAOIParam_Cell_BlackStripe.MinGray_Resist, clsAOIParam_Cell_BlackStripe.MaxGray_Resist);
                            // 規格2 (20190411) Jeff Revised!
                            if (clsAOIParam_Cell_BlackStripe.count_BinImg_Resist == 2)
                            {
                                HObject ho_Reg_2_Resist_Thresh;
                                HOperatorSet.Threshold(ho_ImageReduced_BlackStripe, out ho_Reg_2_Resist_Thresh, clsAOIParam_Cell_BlackStripe.MinGray_2_Resist, clsAOIParam_Cell_BlackStripe.MaxGray_2_Resist);
                                {
                                    HObject ExpTmpOutVar_0;
                                    HOperatorSet.Union2(ho_Reg_Resist_Thresh, ho_Reg_2_Resist_Thresh, out ExpTmpOutVar_0);
                                    ho_Reg_Resist_Thresh.Dispose();
                                    ho_Reg_Resist_Thresh = ExpTmpOutVar_0;
                                }
                                ho_Reg_2_Resist_Thresh.Dispose();
                            }
                        }
                        break;
                    case "動態閥值":
                        {
                            HObject ImageMean = null;
                            HOperatorSet.MeanImage(ho_ImageReduced_BlackStripe, out ImageMean, clsAOIParam_Cell_BlackStripe.MaskWidth_Resist, clsAOIParam_Cell_BlackStripe.MaskHeight_Resist);
                            HOperatorSet.DynThreshold(ho_ImageReduced_BlackStripe, ImageMean, out ho_Reg_Resist_Thresh, clsAOIParam_Cell_BlackStripe.Offset_Resist, clsAOIParam_Cell_BlackStripe.str_LightDark_Resist);
                            ImageMean.Dispose();
                            // 規格2 (20190411) Jeff Revised!
                            if (clsAOIParam_Cell_BlackStripe.count_BinImg_Resist == 2)
                            {
                                HObject ho_Reg_2_Resist_Thresh;
                                HOperatorSet.MeanImage(ho_ImageReduced_BlackStripe, out ImageMean, clsAOIParam_Cell_BlackStripe.MaskWidth_2_Resist, clsAOIParam_Cell_BlackStripe.MaskHeight_2_Resist);
                                HOperatorSet.DynThreshold(ho_ImageReduced_BlackStripe, ImageMean, out ho_Reg_2_Resist_Thresh, clsAOIParam_Cell_BlackStripe.Offset_2_Resist, clsAOIParam_Cell_BlackStripe.str_LightDark_2_Resist);
                                ImageMean.Dispose();
                                {
                                    HObject ExpTmpOutVar_0;
                                    HOperatorSet.Union2(ho_Reg_Resist_Thresh, ho_Reg_2_Resist_Thresh, out ExpTmpOutVar_0);
                                    ho_Reg_Resist_Thresh.Dispose();
                                    ho_Reg_Resist_Thresh = ExpTmpOutVar_0;
                                }
                                ho_Reg_2_Resist_Thresh.Dispose();
                            }
                        }
                        break;
                }

                #endregion
                
                #region  瑕疵Region內至少有一個pixel之灰階值低於一個數值 (瑕疵Region內最低灰階標準)

                // Cell ROI
                if (clsAOIParam_Cell_BlackStripe.Enabled_Gray_select_Cell && clsAOIParam_Cell_BlackStripe.Enabled_Cell)
                {
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.Connection(ho_Reg_Cell_Thresh, out ExpTmpOutVar_0);
                        ho_Reg_Cell_Thresh.Dispose();
                        ho_Reg_Cell_Thresh = ExpTmpOutVar_0;
                    }
                    if (clsAOIParam_Cell_BlackStripe.str_Gray_select_Cell == "min")
                        HOperatorSet.SelectGray(ho_Reg_Cell_Thresh, insp_GrayImgList[clsAOIParam_Cell_BlackStripe.ImageIndex], out ho_Reg_select_Cell, "min", "and", 0, clsAOIParam_Cell_BlackStripe.Gray_select_Cell);
                    else if (clsAOIParam_Cell_BlackStripe.str_Gray_select_Cell == "max")
                        HOperatorSet.SelectGray(ho_Reg_Cell_Thresh, insp_GrayImgList[clsAOIParam_Cell_BlackStripe.ImageIndex], out ho_Reg_select_Cell, "max", "and", clsAOIParam_Cell_BlackStripe.Gray_select_Cell, 255);
                    else
                        HOperatorSet.Union1(ho_Reg_Cell_Thresh, out ho_Reg_select_Cell);

                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.Union1(ho_Reg_select_Cell, out ExpTmpOutVar_0);
                        ho_Reg_select_Cell.Dispose();
                        ho_Reg_select_Cell = ExpTmpOutVar_0;
                    }
                }
                else
                    HOperatorSet.Union1(ho_Reg_Cell_Thresh, out ho_Reg_select_Cell);
                // 電阻ROI
                if (clsAOIParam_Cell_BlackStripe.Enabled_Gray_select_Resist && clsAOIParam_Cell_BlackStripe.Enabled_BlackStripe)
                {
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.Connection(ho_Reg_Resist_Thresh, out ExpTmpOutVar_0);
                        ho_Reg_Resist_Thresh.Dispose();
                        ho_Reg_Resist_Thresh = ExpTmpOutVar_0;
                    }
                    if (clsAOIParam_Cell_BlackStripe.str_Gray_select_Resist == "min")
                        HOperatorSet.SelectGray(ho_Reg_Resist_Thresh, insp_GrayImgList[clsAOIParam_Cell_BlackStripe.ImageIndex], out ho_Reg_select_Resist, "min", "and", 0, clsAOIParam_Cell_BlackStripe.Gray_select_Resist);
                    else if (clsAOIParam_Cell_BlackStripe.str_Gray_select_Resist == "max")
                        HOperatorSet.SelectGray(ho_Reg_Resist_Thresh, insp_GrayImgList[clsAOIParam_Cell_BlackStripe.ImageIndex], out ho_Reg_select_Resist, "max", "and", clsAOIParam_Cell_BlackStripe.Gray_select_Resist, 255);
                    else
                        HOperatorSet.Union1(ho_Reg_Resist_Thresh, out ho_Reg_select_Resist);

                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.Union1(ho_Reg_select_Resist, out ExpTmpOutVar_0);
                        ho_Reg_select_Resist.Dispose();
                        ho_Reg_select_Resist = ExpTmpOutVar_0;
                    }
                }
                else
                    HOperatorSet.Union1(ho_Reg_Resist_Thresh, out ho_Reg_select_Resist);

                // 結合兩區域瑕疵
                HOperatorSet.Union2(ho_Reg_select_Cell, ho_Reg_select_Resist, out ho_Reg_select);
                //{
                //    HObject ExpTmpOutVar_0;
                //    HOperatorSet.Connection(ho_Reg_select, out ExpTmpOutVar_0);
                //    ho_Reg_select.Dispose();
                //    ho_Reg_select = ExpTmpOutVar_0;
                //}

                #endregion
                
                #region 設定檢測ROI 2

                // μm 轉 pixel
                HTuple hv_W_Cell2 = clsAOIParam_Cell_BlackStripe.W_Cell2 / Locate_Method_FS.pixel_resolution_;
                HTuple hv_H_Cell2 = clsAOIParam_Cell_BlackStripe.H_Cell2 / Locate_Method_FS.pixel_resolution_;
                // Cell ROI
                if (clsAOIParam_Cell_BlackStripe.Enabled_Cell2)
                {
                    HOperatorSet.TupleGenConst(hv_count_AllCells, 0.5 * hv_W_Cell2, out hv_L1_AllCells2);
                    HOperatorSet.TupleGenConst(hv_count_AllCells, 0.5 * hv_H_Cell2, out hv_L2_AllCells2);
                    HOperatorSet.GenRectangle2(out ho_Rect_AllCells2, hv_r_AllCells, hv_c_AllCells, hv_phi_AllCells, hv_L1_AllCells2, hv_L2_AllCells2);

                    HOperatorSet.Intersection(ho_Rect_AllCells2, ho_Reg_select, out ho_Reg_select_CellReg);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.Connection(ho_Reg_select_CellReg, out ExpTmpOutVar_0);
                        ho_Reg_select_CellReg.Dispose();
                        ho_Reg_select_CellReg = ExpTmpOutVar_0;
                    }
                }
                else
                {
                    HOperatorSet.GenEmptyObj(out ho_Rect_AllCells2);
                    HOperatorSet.Connection(ho_Reg_select, out ho_Reg_select_CellReg);
                }

                #endregion
                
                #region  瑕疵尺寸篩選

                // μm 轉 pixel
                hv_MinHeight_defect = clsAOIParam_Cell_BlackStripe.MinHeight_defect / Locate_Method_FS.pixel_resolution_;
                hv_MaxHeight_defect = clsAOIParam_Cell_BlackStripe.MaxHeight_defect / Locate_Method_FS.pixel_resolution_;
                hv_MinWidth_defect = clsAOIParam_Cell_BlackStripe.MinWidth_defect / Locate_Method_FS.pixel_resolution_;
                hv_MaxWidth_defect = clsAOIParam_Cell_BlackStripe.MaxWidth_defect / Locate_Method_FS.pixel_resolution_;
                hv_MinArea_defect = clsAOIParam_Cell_BlackStripe.MinArea_defect / (Locate_Method_FS.pixel_resolution_ * Locate_Method_FS.pixel_resolution_);
                hv_MaxArea_defect = clsAOIParam_Cell_BlackStripe.MaxArea_defect / (Locate_Method_FS.pixel_resolution_ * Locate_Method_FS.pixel_resolution_);

                // 操作1
                if (clsAOIParam_Cell_BlackStripe.count_op1 == 0)
                    HOperatorSet.CopyObj(ho_Reg_select_CellReg, out ho_DefectReg_op1, 1, -1);
                else
                {
                    // 規格1
                    if (clsAOIParam_Cell_BlackStripe.Enabled_Height && clsAOIParam_Cell_BlackStripe.Enabled_Width)
                    {
                        HOperatorSet.SelectShape(ho_Reg_select_CellReg, out ho_DefectReg_op1,
                                                 (new HTuple("height")).TupleConcat("width"), clsAOIParam_Cell_BlackStripe.str_op1,
                                                 hv_MinHeight_defect.TupleConcat(hv_MinWidth_defect),
                                                 hv_MaxHeight_defect.TupleConcat(hv_MaxWidth_defect));
                    }
                    else if (clsAOIParam_Cell_BlackStripe.Enabled_Height)
                    {
                        HOperatorSet.SelectShape(ho_Reg_select_CellReg, out ho_DefectReg_op1, "height",
                                                 clsAOIParam_Cell_BlackStripe.str_op1, hv_MinHeight_defect, hv_MaxHeight_defect);
                    }
                    else if (clsAOIParam_Cell_BlackStripe.Enabled_Width)
                    {
                        HOperatorSet.SelectShape(ho_Reg_select_CellReg, out ho_DefectReg_op1, "width",
                                                 clsAOIParam_Cell_BlackStripe.str_op1, hv_MinWidth_defect, hv_MaxWidth_defect);
                    }
                    else
                        HOperatorSet.CopyObj(ho_Reg_select_CellReg, out ho_DefectReg_op1, 1, -1);

                    // 規格2
                    if (clsAOIParam_Cell_BlackStripe.count_op1 == 2)
                    {
                        // μm 轉 pixel
                        HTuple hv_MinHeight_defect_op1_2 = clsAOIParam_Cell_BlackStripe.MinHeight_defect_op1_2 / Locate_Method_FS.pixel_resolution_;
                        HTuple hv_MaxHeight_defect_op1_2 = clsAOIParam_Cell_BlackStripe.MaxHeight_defect_op1_2 / Locate_Method_FS.pixel_resolution_;
                        HTuple hv_MinWidth_defect_op1_2 = clsAOIParam_Cell_BlackStripe.MinWidth_defect_op1_2 / Locate_Method_FS.pixel_resolution_;
                        HTuple hv_MaxWidth_defect_op1_2 = clsAOIParam_Cell_BlackStripe.MaxWidth_defect_op1_2 / Locate_Method_FS.pixel_resolution_;

                        HObject ho_DefectReg_op1_2 = null;
                        if (clsAOIParam_Cell_BlackStripe.Enabled_Height_op1_2 && clsAOIParam_Cell_BlackStripe.Enabled_Width_op1_2)
                        {
                            HOperatorSet.SelectShape(ho_Reg_select_CellReg, out ho_DefectReg_op1_2,
                                                     (new HTuple("height")).TupleConcat("width"), clsAOIParam_Cell_BlackStripe.str_op1_2,
                                                     hv_MinHeight_defect_op1_2.TupleConcat(hv_MinWidth_defect_op1_2),
                                                     hv_MaxHeight_defect_op1_2.TupleConcat(hv_MaxWidth_defect_op1_2));
                        }
                        else if (clsAOIParam_Cell_BlackStripe.Enabled_Height_op1_2)
                        {
                            HOperatorSet.SelectShape(ho_Reg_select_CellReg, out ho_DefectReg_op1_2, "height",
                                                     clsAOIParam_Cell_BlackStripe.str_op1_2, hv_MinHeight_defect_op1_2, hv_MaxHeight_defect_op1_2);
                        }
                        else if (clsAOIParam_Cell_BlackStripe.Enabled_Width_op1_2)
                        {
                            HOperatorSet.SelectShape(ho_Reg_select_CellReg, out ho_DefectReg_op1_2, "width",
                                                     clsAOIParam_Cell_BlackStripe.str_op1_2, hv_MinWidth_defect_op1_2, hv_MaxWidth_defect_op1_2);
                        }
                        else
                            HOperatorSet.CopyObj(ho_Reg_select_CellReg, out ho_DefectReg_op1_2, 1, -1);

                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_DefectReg_op1, ho_DefectReg_op1_2, out ExpTmpOutVar_0);
                            ho_DefectReg_op1.Dispose();
                            ho_DefectReg_op1 = ExpTmpOutVar_0;
                        }

                        ho_DefectReg_op1_2.Dispose();
                    }
                }

                // 操作2
                if (clsAOIParam_Cell_BlackStripe.Enabled_Area)
                {
                    HOperatorSet.SelectShape(ho_Reg_select_CellReg, out ho_DefectReg_op2, "area",
                                             clsAOIParam_Cell_BlackStripe.str_op2, hv_MinArea_defect, hv_MaxArea_defect);
                    HOperatorSet.Union1(ho_DefectReg_op1, out ho_DefectReg_op1_union);
                    HOperatorSet.Union1(ho_DefectReg_op2, out ho_DefectReg_op2_union);
                    if (clsAOIParam_Cell_BlackStripe.str_op2 == "and")
                        HOperatorSet.Intersection(ho_DefectReg_op1_union, ho_DefectReg_op2_union, out DefectReg_Cell_BlackStripe);
                    else if (clsAOIParam_Cell_BlackStripe.str_op2 == "or")
                        HOperatorSet.Union2(ho_DefectReg_op1_union, ho_DefectReg_op2_union, out DefectReg_Cell_BlackStripe);

                    // (20190331) Jeff Revised!
                    //{
                    //    HObject ExpTmpOutVar_0;
                    //    HOperatorSet.Connection(DefectReg_Cell_BlackStripe, out ExpTmpOutVar_0);
                    //    DefectReg_Cell_BlackStripe.Dispose();
                    //    DefectReg_Cell_BlackStripe = ExpTmpOutVar_0;
                    //}
                }
                else
                    HOperatorSet.CopyObj(ho_DefectReg_op1, out DefectReg_Cell_BlackStripe, 1, -1);

                #endregion

                #endregion

                Res = true;
            }
            catch (Exception ex)
            {
                Log.WriteLog("Error: Cell_BlackStripe_Insp() 執行發生例外狀況 -> " + ex.ToString());
                return false;
            }

            return Res;
        }

        /// <summary>
        /// 檢測黑條內黑點2 (同軸光)
        /// </summary>
        /// <param name="Recipe"></param>
        /// <returns></returns>
        public bool BlackBlob2_CL_Insp(ResistPanelRole Recipe) // (20190219) Jeff Revised!
        {
            bool Res = false;

            try
            {
                #region Execute BlackBlob2_CL_Insp()

                // Initialize local and output iconic variables
                HOperatorSet.GenEmptyObj(out ho_DefectReg_BlackBlob2);

                if (!Cell_BlackStripe_Insp(Recipe, Recipe.Param.InspParam_BlackBlob2_CL, out ho_DefectReg_BlackBlob2))
                    return false;

                // 旋轉瑕疵region回原本方向
                //if (Recipe.Param.Enabled_rotate)
                //{
                //    HOperatorSet.AffineTransRegion(ho_DefectReg_BlackBlob2.Clone(), out ho_DefectReg_BlackBlob2, hommat2d_origin_, "nearest_neighbor");
                //}

                // (20190807) Jeff Revised!
                if (Recipe.Param.B_AOI_absolNG) // 是否啟用AOI判斷絕對NG
                {
                    HObject Reg_absolNG;
                    Extension.HObjectMedthods.ReleaseHObject(ref reg_SelectGray_AbsolNG);
                    if (!AOI_absolNG_Insp(Recipe.Param.InspParam_BlackBlob2_CL.cls_absolNG, Recipe.Param.InspParam_BlackBlob2_CL.ImageIndex, ho_DefectReg_BlackBlob2, out Reg_absolNG, out reg_SelectGray_AbsolNG))
                    {
                        Extension.HObjectMedthods.ReleaseHObject(ref Reg_absolNG);
                        Dict_AOI_absolNG["BlackBlob2 (同軸光)"].DefectReg = ho_DefectReg_BlackBlob2.Clone();
                        return false;
                    }
                    else
                    {
                        Dict_AOI_absolNG["BlackBlob2 (同軸光)"].DefectReg = Reg_absolNG;

                        #region 加入絕對NG Region所在Cell之其他瑕疵Region

                        HObject Reg_absolNG_Cell;
                        if (Recipe.Param.InspParam_BlackBlob2_CL.cls_absolNG.Enabled)
                        {
                            HObject temp;
                            HTuple hv_area_InspRects_Defect = new HTuple(), hv_Greater = new HTuple(), hv_Index_defect = new HTuple();
                            HOperatorSet.Intersection(ho_InspRects_AllCells, Reg_absolNG, out temp);
                            HOperatorSet.RegionFeatures(temp, "area", out hv_area_InspRects_Defect);
                            HOperatorSet.TupleGreaterElem(hv_area_InspRects_Defect, 0.0, out hv_Greater);
                            HOperatorSet.TupleFind(hv_Greater, 1, out hv_Index_defect);

                            if ((int)(new HTuple(hv_Index_defect.TupleNotEqual(-1))) == 1)
                            {
                                hv_Index_defect = hv_Index_defect + 1;
                                HOperatorSet.SelectObj(ho_InspRects_AllCells, out Reg_absolNG_Cell, hv_Index_defect);
                            }
                            else
                                Reg_absolNG_Cell = Reg_absolNG.Clone();
                            temp.Dispose();
                        }
                        else
                            Reg_absolNG_Cell = Reg_absolNG.Clone();

                        #endregion
                        
                        {
                            HObject AOI_NG;
                            HOperatorSet.Difference(ho_DefectReg_BlackBlob2, Reg_absolNG_Cell, out AOI_NG);
                            Dict_AOI_NG["BlackBlob2 (同軸光)"].DefectReg = AOI_NG;
                        }
                        Reg_absolNG_Cell.Dispose();
                    }
                }
                else
                {
                    Dict_AOI_absolNG["BlackBlob2 (同軸光)"].DefectReg = ho_DefectReg_BlackBlob2.Clone();
                }

                #endregion

                Res = true;
            }
            catch (Exception ex)
            {
                Log.WriteLog("Error: BlackBlob2_CL_Insp() 執行發生例外狀況 -> " + ex.ToString());
                return false;
            }

            return Res;
        }

        /// <summary>
        /// 檢測汙染 (同軸光)
        /// </summary>
        /// <param name="Recipe"></param>
        /// <returns></returns>
        public bool Dirty_CL_Insp(ResistPanelRole Recipe) // (20191122) Jeff Revised!
        {
            bool Res = false;

            try
            {
                #region Execute Dirty_CL_Insp()

                // Initialize local and output iconic variables
                HOperatorSet.GenEmptyObj(out ho_DefectReg_Dirty);

                /* 設定檢測ROI */
                // μm 轉 pixel
                HTuple hv_W_Cell = Recipe.Param.InspParam_Dirty_CL.W_Cell / Locate_Method_FS.pixel_resolution_;
                HTuple hv_H_Cell = Recipe.Param.InspParam_Dirty_CL.H_Cell / Locate_Method_FS.pixel_resolution_;
                HTuple hv_x_BypassReg = Recipe.Param.InspParam_Dirty_CL.x_BypassReg / Locate_Method_FS.pixel_resolution_;
                HTuple hv_y_BypassReg = Recipe.Param.InspParam_Dirty_CL.y_BypassReg / Locate_Method_FS.pixel_resolution_;
                HTuple hv_W_BypassReg = Recipe.Param.InspParam_Dirty_CL.W_BypassReg / Locate_Method_FS.pixel_resolution_;
                HTuple hv_H_BypassReg = Recipe.Param.InspParam_Dirty_CL.H_BypassReg / Locate_Method_FS.pixel_resolution_;
                HTuple hv_H_BlackStripe = Recipe.Param.InspParam_Dirty_CL.H_BlackStripe / Locate_Method_FS.pixel_resolution_;
                // Cell ROI
                if (Recipe.Param.InspParam_Dirty_CL.Enabled_Cell)
                {
                    HOperatorSet.TupleGenConst(hv_count_AllCells, 0.5 * hv_W_Cell, out hv_L1_AllCells);
                    HOperatorSet.TupleGenConst(hv_count_AllCells, 0.5 * hv_H_Cell, out hv_L2_AllCells);
                    HOperatorSet.GenRectangle2(out ho_Rect_AllCells, hv_r_AllCells, hv_c_AllCells, hv_phi_AllCells, hv_L1_AllCells, hv_L2_AllCells);
                }
                else
                    HOperatorSet.GenEmptyRegion(out ho_Rect_AllCells);
                // Cell內忽略區域
                if (Recipe.Param.InspParam_Dirty_CL.Enabled_BypassReg)
                {
                    HTuple hv_r_BypassReg = hv_r_AllCells + hv_y_BypassReg;
                    HTuple hv_c_BypassReg = hv_c_AllCells + hv_x_BypassReg;
                    HTuple hv_L1_BypassReg = new HTuple(), hv_L2_BypassReg = new HTuple();
                    HOperatorSet.TupleGenConst(hv_count_AllCells, 0.5 * hv_W_BypassReg, out hv_L1_BypassReg);
                    HOperatorSet.TupleGenConst(hv_count_AllCells, 0.5 * hv_H_BypassReg, out hv_L2_BypassReg);
                    HOperatorSet.GenRectangle2(out ho_Rect_BypassReg, hv_r_BypassReg, hv_c_BypassReg, hv_phi_AllCells, hv_L1_BypassReg, hv_L2_BypassReg);
                }
                else
                    HOperatorSet.GenEmptyRegion(out ho_Rect_BypassReg);
                // 電阻ROI
                if (Recipe.Param.InspParam_Dirty_CL.Enabled_BlackStripe)
                {
                    string str_ModeBlackStripe = Recipe.Param.InspParam_Dirty_CL.str_ModeBlackStripe;
                    if (str_ModeBlackStripe == "mode1")
                    {
                        HOperatorSet.TupleGenConst(hv_count_BlackStripe, 0.5 * hv_H_BlackStripe, out hv_L2_BlackStripe);
                        HOperatorSet.GenRectangle2(out ho_Rect_BlackStripe, hv_r_BlackStripe, hv_c_BlackStripe, hv_Phi_BlackStripe, hv_L1_BlackStripe, hv_L2_BlackStripe);
                    }
                    else if (str_ModeBlackStripe == "mode2") // (20191122) Jeff Revised!
                    {
                        int W_mor = 0, H_mor = 0, Type_mor = 0;
                        if (Recipe.Param.Type_BlackStripe == "Horizontal") // 黑條方向水平
                        {
                            W_mor = 1;
                            H_mor = Math.Abs(Recipe.Param.InspParam_Dirty_CL.H_BlackStripe_mode2);
                        }
                        else if (Recipe.Param.Type_BlackStripe == "Vertical") // 黑條方向垂直
                        {
                            H_mor = 1;
                            W_mor = Math.Abs(Recipe.Param.InspParam_Dirty_CL.H_BlackStripe_mode2);
                        }

                        if (Recipe.Param.InspParam_Dirty_CL.H_BlackStripe_mode2 > 0)
                            Type_mor = 1;
                        else if (Recipe.Param.InspParam_Dirty_CL.H_BlackStripe_mode2 < 0)
                            Type_mor = -1;
                        
                        if (Type_mor > 0)
                            HOperatorSet.DilationRectangle1(ho_Regions_BlackStripe, out ho_Rect_BlackStripe, W_mor, H_mor);
                        else if (Type_mor < 0)
                            HOperatorSet.ErosionRectangle1(ho_Regions_BlackStripe, out ho_Rect_BlackStripe, W_mor, H_mor);
                        else
                            HOperatorSet.CopyObj(ho_Regions_BlackStripe, out ho_Rect_BlackStripe, 1, -1);
                    }
                }
                else
                    HOperatorSet.GenEmptyRegion(out ho_Rect_BlackStripe);


                /* 二值化處理 */
                // Cell ROI
                if (Recipe.Param.InspParam_Dirty_CL.Enabled_BypassReg)
                {
                    HOperatorSet.Difference(ho_Rect_AllCells, ho_Rect_BypassReg, out ho_RegDif_Cell);
                    HOperatorSet.Union1(ho_RegDif_Cell, out ho_Rect_AllCells_union);
                }
                else
                {
                    HOperatorSet.Union1(ho_Rect_AllCells, out ho_Rect_AllCells_union);
                }
                HOperatorSet.ReduceDomain(ho_GrayImage_CL, ho_Rect_AllCells_union, out ho_ImageReduced_AllCells_Dirty_CL);
                HOperatorSet.Threshold(ho_ImageReduced_AllCells_Dirty_CL, out ho_Reg_Cell_Dirty_CL, Recipe.Param.InspParam_Dirty_CL.MinGray_Cell, Recipe.Param.InspParam_Dirty_CL.MaxGray_Cell);
                // 電阻ROI
                HOperatorSet.Union1(ho_Rect_BlackStripe, out ho_Rect_BlackStripe_union);
                HOperatorSet.Union1(ho_Rect_AllCells, out ho_Rect_AllCells_union1);
                HOperatorSet.Union1(ho_Rect_BypassReg, out ho_Rect_BypassReg_union);
                HOperatorSet.Difference(ho_Rect_BlackStripe_union, ho_Rect_AllCells_union1, out ho_RegDif_BlackStripe_temp);
                HOperatorSet.Difference(ho_RegDif_BlackStripe_temp, ho_Rect_BypassReg_union, out ho_RegDif_BlackStripe);
                HOperatorSet.ReduceDomain(ho_GrayImage_CL, ho_RegDif_BlackStripe, out ho_ImageReduced_BlackStripe_Dirty_CL);
                HOperatorSet.Threshold(ho_ImageReduced_BlackStripe_Dirty_CL, out ho_Reg_Resist_Dirty_CL, Recipe.Param.InspParam_Dirty_CL.MinGray_Resist, Recipe.Param.InspParam_Dirty_CL.MaxGray_Resist);

                // 結合兩區域瑕疵
                HOperatorSet.Union2(ho_Reg_Cell_Dirty_CL, ho_Reg_Resist_Dirty_CL, out ho_Reg_Dirty_CL);

                // 瑕疵Region內至少有一個pixel之灰階值低於一個數值 (瑕疵Region內最低灰階標準)
                if (Recipe.Param.InspParam_Dirty_CL.Enabled_Gray_select)
                {
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.Connection(ho_Reg_Dirty_CL, out ExpTmpOutVar_0);
                        ho_Reg_Dirty_CL.Dispose();
                        ho_Reg_Dirty_CL = ExpTmpOutVar_0;
                    }
                    if (Recipe.Param.InspParam_Dirty_CL.str_Gray_select == "min")
                        HOperatorSet.SelectGray(ho_Reg_Dirty_CL, ho_GrayImage_CL, out ho_Reg_Dirty_CL_select, "min", "and", 0, Recipe.Param.InspParam_Dirty_CL.Gray_select);
                    else if (Recipe.Param.InspParam_Dirty_CL.str_Gray_select == "max")
                        HOperatorSet.SelectGray(ho_Reg_Dirty_CL, ho_GrayImage_CL, out ho_Reg_Dirty_CL_select, "max", "and", Recipe.Param.InspParam_Dirty_CL.Gray_select, 255);
                    else
                        HOperatorSet.Connection(ho_Reg_Dirty_CL, out ho_Reg_Dirty_CL_select);
                }
                else
                    HOperatorSet.Connection(ho_Reg_Dirty_CL, out ho_Reg_Dirty_CL_select);



                /* 瑕疵尺寸篩選 */
                // μm 轉 pixel
                hv_MinHeight_defect = Recipe.Param.InspParam_Dirty_CL.MinHeight_defect / Locate_Method_FS.pixel_resolution_;
                hv_MaxHeight_defect = Recipe.Param.InspParam_Dirty_CL.MaxHeight_defect / Locate_Method_FS.pixel_resolution_;
                hv_MinWidth_defect = Recipe.Param.InspParam_Dirty_CL.MinWidth_defect / Locate_Method_FS.pixel_resolution_;
                hv_MaxWidth_defect = Recipe.Param.InspParam_Dirty_CL.MaxWidth_defect / Locate_Method_FS.pixel_resolution_;
                hv_MinArea_defect = Recipe.Param.InspParam_Dirty_CL.MinArea_defect / (Locate_Method_FS.pixel_resolution_ * Locate_Method_FS.pixel_resolution_);
                hv_MaxArea_defect = Recipe.Param.InspParam_Dirty_CL.MaxArea_defect / (Locate_Method_FS.pixel_resolution_ * Locate_Method_FS.pixel_resolution_);

                //操作1
                if (Recipe.Param.InspParam_Dirty_CL.Enabled_Height && Recipe.Param.InspParam_Dirty_CL.Enabled_Width)
                {
                    HOperatorSet.SelectShape(ho_Reg_Dirty_CL_select, out ho_DefectReg_op1,
                                             (new HTuple("height")).TupleConcat("width"), Recipe.Param.InspParam_Dirty_CL.str_op1,
                                             hv_MinHeight_defect.TupleConcat(hv_MinWidth_defect),
                                             hv_MaxHeight_defect.TupleConcat(hv_MaxWidth_defect));
                }
                else if (Recipe.Param.InspParam_Dirty_CL.Enabled_Height)
                {
                    HOperatorSet.SelectShape(ho_Reg_Dirty_CL_select, out ho_DefectReg_op1, "height",
                                             Recipe.Param.InspParam_Dirty_CL.str_op1, hv_MinHeight_defect, hv_MaxHeight_defect);
                }
                else if (Recipe.Param.InspParam_Dirty_CL.Enabled_Width)
                {
                    HOperatorSet.SelectShape(ho_Reg_Dirty_CL_select, out ho_DefectReg_op1, "width",
                                             Recipe.Param.InspParam_Dirty_CL.str_op1, hv_MinWidth_defect, hv_MaxWidth_defect);
                }
                else
                    HOperatorSet.CopyObj(ho_Reg_Dirty_CL_select, out ho_DefectReg_op1, 1, -1);
                //操作2
                if (Recipe.Param.InspParam_Dirty_CL.Enabled_Area)
                {
                    HOperatorSet.SelectShape(ho_Reg_Dirty_CL_select, out ho_DefectReg_op2, "area",
                                             Recipe.Param.InspParam_Dirty_CL.str_op2, hv_MinArea_defect, hv_MaxArea_defect);
                    HOperatorSet.Union1(ho_DefectReg_op1, out ho_DefectReg_op1_union);
                    HOperatorSet.Union1(ho_DefectReg_op2, out ho_DefectReg_op2_union);
                    if (Recipe.Param.InspParam_Dirty_CL.str_op2 == "and")
                        HOperatorSet.Intersection(ho_DefectReg_op1_union, ho_DefectReg_op2_union, out ho_DefectReg_Dirty);
                    else if (Recipe.Param.InspParam_Dirty_CL.str_op2 == "or")
                        HOperatorSet.Union2(ho_DefectReg_op1_union, ho_DefectReg_op2_union, out ho_DefectReg_Dirty);

                    // (20190331) Jeff Revised!
                    //{
                    //    HObject ExpTmpOutVar_0;
                    //    HOperatorSet.Connection(ho_DefectReg_Dirty, out ExpTmpOutVar_0);
                    //    ho_DefectReg_Dirty.Dispose();
                    //    ho_DefectReg_Dirty = ExpTmpOutVar_0;
                    //}
                }
                else
                    HOperatorSet.CopyObj(ho_DefectReg_op1, out ho_DefectReg_Dirty, 1, -1);

                // 旋轉瑕疵region回原本方向
                //if (Recipe.Param.Enabled_rotate)
                //{
                //    HOperatorSet.AffineTransRegion(ho_DefectReg_Dirty.Clone(), out ho_DefectReg_Dirty, hommat2d_origin_, "nearest_neighbor");
                //}

                // (20190807) Jeff Revised!
                if (Recipe.Param.B_AOI_absolNG) // 是否啟用AOI判斷絕對NG
                {
                    HObject Reg_absolNG;
                    Extension.HObjectMedthods.ReleaseHObject(ref reg_SelectGray_AbsolNG);
                    if (!AOI_absolNG_Insp(Recipe.Param.InspParam_Dirty_CL.cls_absolNG, Recipe.Param.InspParam_Dirty_CL.ImageIndex, ho_DefectReg_Dirty, out Reg_absolNG, out reg_SelectGray_AbsolNG))
                    {
                        Extension.HObjectMedthods.ReleaseHObject(ref Reg_absolNG);
                        Dict_AOI_absolNG["汙染 (同軸光)"].DefectReg = ho_DefectReg_Dirty.Clone();
                        return false;
                    }
                    else
                    {
                        Dict_AOI_absolNG["汙染 (同軸光)"].DefectReg = Reg_absolNG;

                        #region 加入絕對NG Region所在Cell之其他瑕疵Region

                        HObject Reg_absolNG_Cell;
                        if (Recipe.Param.InspParam_Dirty_CL.cls_absolNG.Enabled)
                        {
                            HObject temp;
                            HTuple hv_area_InspRects_Defect = new HTuple(), hv_Greater = new HTuple(), hv_Index_defect = new HTuple();
                            HOperatorSet.Intersection(ho_InspRects_AllCells, Reg_absolNG, out temp);
                            HOperatorSet.RegionFeatures(temp, "area", out hv_area_InspRects_Defect);
                            HOperatorSet.TupleGreaterElem(hv_area_InspRects_Defect, 0.0, out hv_Greater);
                            HOperatorSet.TupleFind(hv_Greater, 1, out hv_Index_defect);

                            if ((int)(new HTuple(hv_Index_defect.TupleNotEqual(-1))) == 1)
                            {
                                hv_Index_defect = hv_Index_defect + 1;
                                HOperatorSet.SelectObj(ho_InspRects_AllCells, out Reg_absolNG_Cell, hv_Index_defect);
                            }
                            else
                                Reg_absolNG_Cell = Reg_absolNG.Clone();
                            temp.Dispose();
                        }
                        else
                            Reg_absolNG_Cell = Reg_absolNG.Clone();

                        #endregion
                        
                        {
                            HObject AOI_NG;
                            HOperatorSet.Difference(ho_DefectReg_Dirty, Reg_absolNG_Cell, out AOI_NG);
                            Dict_AOI_NG["汙染 (同軸光)"].DefectReg = AOI_NG;
                        }
                        Reg_absolNG_Cell.Dispose();
                    }
                }
                else
                {
                    Dict_AOI_absolNG["汙染 (同軸光)"].DefectReg = ho_DefectReg_Dirty.Clone();
                }

                #endregion

                Res = true;
            }
            catch (Exception ex)
            {
                Log.WriteLog("Error: Dirty_CL_Insp() 執行發生例外狀況 -> " + ex.ToString());
                return false;
            }

            return Res;
        }

        /// <summary>
        /// 檢測汙染 (正光&同軸光)
        /// </summary>
        /// <param name="Recipe"></param>
        /// <returns></returns>
        public bool Dirty_FLCL_Insp(ResistPanelRole Recipe) // (20190215) Jeff Revised!
        {
            bool Res = false;

            try
            {
                #region Execute Dirty_FLCL_Insp()

                // Initialize local and output iconic variables
                HOperatorSet.GenEmptyObj(out ho_DefectReg_Dirty_FLCL);

                /* Dirty (汙染 or 異物): 黑色長條區域內的黑點 (Frontal light) */
                HOperatorSet.ReduceDomain(ho_ImageReduced_FL, ho_Regions_BlackStripe_small, out ho_ImageReduced_BlackStripe_FL);
                HOperatorSet.Threshold(ho_ImageReduced_BlackStripe_FL, out ho_DefectReg_Dirty_thresh_FL, 0, Recipe.Param.InspParam_Dirty_FLCL.MaxGray_Dirty_FL);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Connection(ho_DefectReg_Dirty_thresh_FL, out ExpTmpOutVar_0);
                    ho_DefectReg_Dirty_thresh_FL.Dispose();
                    ho_DefectReg_Dirty_thresh_FL = ExpTmpOutVar_0;
                }
                //瑕疵Region內至少有一個pixel之灰階值低於一個數值 (瑕疵Region內最低灰階標準)
                HOperatorSet.SelectGray(ho_DefectReg_Dirty_thresh_FL, ho_ImageReduced_BlackStripe_FL, out ho_DefectReg_Dirty_FL,
                                        "min", "and", 0, Recipe.Param.InspParam_Dirty_FLCL.Gray_Dark_FL);

                /* Dirty (汙染 or 異物): 黑色長條區域內的白點 (Coaxial light) */
                HOperatorSet.ReduceDomain(ho_GrayImage_CL, ho_Regions_BlackStripe_small, out ho_ImageReduced_BlackStripe_CL);
                HOperatorSet.Threshold(ho_ImageReduced_BlackStripe_CL, out ho_DefectReg_Dirty_thresh_CL, Recipe.Param.InspParam_Dirty_FLCL.MinGray_Dirty_CL, 255);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Connection(ho_DefectReg_Dirty_thresh_CL, out ExpTmpOutVar_0);
                    ho_DefectReg_Dirty_thresh_CL.Dispose();
                    ho_DefectReg_Dirty_thresh_CL = ExpTmpOutVar_0;
                }
                //瑕疵Region內至少有一個pixel之灰階值高於一個數值 (瑕疵Region內最高灰階標準)
                HOperatorSet.SelectGray(ho_DefectReg_Dirty_thresh_CL, ho_ImageReduced_BlackStripe_CL, out ho_DefectReg_Dirty_CL,
                                        "max", "and", Recipe.Param.InspParam_Dirty_FLCL.Gray_Bright_CL, 255);

                /* 結合兩者 */
                HOperatorSet.Union2(ho_DefectReg_Dirty_FL, ho_DefectReg_Dirty_CL, out ho_DefectReg_Dirty_union);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Connection(ho_DefectReg_Dirty_union, out ExpTmpOutVar_0);
                    ho_DefectReg_Dirty_union.Dispose();
                    ho_DefectReg_Dirty_union = ExpTmpOutVar_0;
                }

                /* 尺寸篩選 */
                // double 轉 HTuple
                hv_MinHeight_defect = Recipe.Param.InspParam_Dirty_FLCL.MinHeight_defect / Locate_Method_FS.pixel_resolution_;
                hv_MaxHeight_defect = Recipe.Param.InspParam_Dirty_FLCL.MaxHeight_defect / Locate_Method_FS.pixel_resolution_;
                hv_MinWidth_defect = Recipe.Param.InspParam_Dirty_FLCL.MinWidth_defect / Locate_Method_FS.pixel_resolution_;
                hv_MaxWidth_defect = Recipe.Param.InspParam_Dirty_FLCL.MaxWidth_defect / Locate_Method_FS.pixel_resolution_;
                HOperatorSet.SelectShape(ho_DefectReg_Dirty_union, out ho_DefectReg_Dirty_FLCL,
                                         (new HTuple("height")).TupleConcat("width"), "and",
                                         hv_MinHeight_defect.TupleConcat(hv_MinWidth_defect),
                                         hv_MaxHeight_defect.TupleConcat(hv_MaxWidth_defect));

                // 旋轉瑕疵region回原本方向
                //if (Recipe.Param.Enabled_rotate)
                //{
                //    HOperatorSet.AffineTransRegion(ho_DefectReg_Dirty_FLCL.Clone(), out ho_DefectReg_Dirty_FLCL, hommat2d_origin_, "nearest_neighbor");
                //}

                // (20190807) Jeff Revised!
                if (Recipe.Param.B_AOI_absolNG) // 是否啟用AOI判斷絕對NG
                {
                    HObject Reg_absolNG;
                    Extension.HObjectMedthods.ReleaseHObject(ref reg_SelectGray_AbsolNG);
                    if (!AOI_absolNG_Insp(Recipe.Param.InspParam_Dirty_FLCL.cls_absolNG, -1, ho_DefectReg_Dirty_FLCL, out Reg_absolNG, out reg_SelectGray_AbsolNG))
                    {
                        Extension.HObjectMedthods.ReleaseHObject(ref Reg_absolNG);
                        Dict_AOI_absolNG["汙染 (正光&同軸光)"].DefectReg = ho_DefectReg_Dirty_FLCL.Clone();
                        return false;
                    }
                    else
                    {
                        Dict_AOI_absolNG["汙染 (正光&同軸光)"].DefectReg = Reg_absolNG;

                        #region 加入絕對NG Region所在Cell之其他瑕疵Region

                        HObject Reg_absolNG_Cell;
                        if (Recipe.Param.InspParam_Dirty_FLCL.cls_absolNG.Enabled)
                        {
                            HObject temp;
                            HTuple hv_area_InspRects_Defect = new HTuple(), hv_Greater = new HTuple(), hv_Index_defect = new HTuple();
                            HOperatorSet.Intersection(ho_InspRects_AllCells, Reg_absolNG, out temp);
                            HOperatorSet.RegionFeatures(temp, "area", out hv_area_InspRects_Defect);
                            HOperatorSet.TupleGreaterElem(hv_area_InspRects_Defect, 0.0, out hv_Greater);
                            HOperatorSet.TupleFind(hv_Greater, 1, out hv_Index_defect);

                            if ((int)(new HTuple(hv_Index_defect.TupleNotEqual(-1))) == 1)
                            {
                                hv_Index_defect = hv_Index_defect + 1;
                                HOperatorSet.SelectObj(ho_InspRects_AllCells, out Reg_absolNG_Cell, hv_Index_defect);
                            }
                            else
                                Reg_absolNG_Cell = Reg_absolNG.Clone();
                            temp.Dispose();
                        }
                        else
                            Reg_absolNG_Cell = Reg_absolNG.Clone();

                        #endregion
                        
                        {
                            HObject AOI_NG;
                            HOperatorSet.Difference(ho_DefectReg_Dirty_FLCL, Reg_absolNG_Cell, out AOI_NG);
                            Dict_AOI_NG["汙染 (正光&同軸光)"].DefectReg = AOI_NG;
                        }
                        Reg_absolNG_Cell.Dispose();
                    }
                }
                else
                {
                    Dict_AOI_absolNG["汙染 (正光&同軸光)"].DefectReg = ho_DefectReg_Dirty_FLCL.Clone();
                }

                #endregion

                Res = true;
            }
            catch (Exception ex)
            {
                Log.WriteLog("Error: Dirty_FLCL_Insp() 執行發生例外狀況 -> " + ex.ToString());
                return false;
            }

            return Res;
        }

        /// <summary>
        /// 檢測電極區白點 (背光)
        /// </summary>
        /// <param name="Recipe"></param>
        /// <returns></returns>
        public bool BrightBlob_BL_Insp(ResistPanelRole Recipe) // (20190215) Jeff Revised!
        {
            bool Res = false;

            try
            {
                #region Execute BrightBlob_BL_Insp()

                // Initialize local and output iconic variables
                HOperatorSet.GenEmptyObj(out ho_DefectReg_BrightBlob);

                //電極上 Bright Blob
                HOperatorSet.Difference(ho_Regions_Inspect, ho_Regions_BlackStripe, out ho_Regions_WhiteStripe_BL);
                HOperatorSet.ReduceDomain(ho_GrayImage_BL, ho_Regions_WhiteStripe_BL, out ho_ImageReduced_WhiteStripe_BL);
                HOperatorSet.Threshold(ho_ImageReduced_WhiteStripe_BL, out ho_Region_Blob_Elect_BL, Recipe.Param.InspParam_BrightBlob_BL.MinGray_Blob_Elect_BL, 255);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Connection(ho_Region_Blob_Elect_BL, out ExpTmpOutVar_0);
                    ho_Region_Blob_Elect_BL.Dispose();
                    ho_Region_Blob_Elect_BL = ExpTmpOutVar_0;
                }

                /* 尺寸篩選 */
                // double 轉 HTuple
                hv_MinHeight_defect = Recipe.Param.InspParam_BrightBlob_BL.MinHeight_defect / Locate_Method_FS.pixel_resolution_;
                hv_MaxHeight_defect = Recipe.Param.InspParam_BrightBlob_BL.MaxHeight_defect / Locate_Method_FS.pixel_resolution_;
                hv_MinWidth_defect = Recipe.Param.InspParam_BrightBlob_BL.MinWidth_defect / Locate_Method_FS.pixel_resolution_;
                hv_MaxWidth_defect = Recipe.Param.InspParam_BrightBlob_BL.MaxWidth_defect / Locate_Method_FS.pixel_resolution_;
                HOperatorSet.SelectShape(ho_Region_Blob_Elect_BL, out ho_DefectReg_BrightBlob,
                                         (new HTuple("height")).TupleConcat("width"), "and",
                                         hv_MinHeight_defect.TupleConcat(hv_MinWidth_defect),
                                         hv_MaxHeight_defect.TupleConcat(hv_MaxWidth_defect));

                // 旋轉瑕疵region回原本方向
                //if (Recipe.Param.Enabled_rotate)
                //{
                //    HOperatorSet.AffineTransRegion(ho_DefectReg_BrightBlob.Clone(), out ho_DefectReg_BrightBlob, hommat2d_origin_, "nearest_neighbor");
                //}

                // (20190807) Jeff Revised!
                if (Recipe.Param.B_AOI_absolNG) // 是否啟用AOI判斷絕對NG
                {
                    HObject Reg_absolNG;
                    Extension.HObjectMedthods.ReleaseHObject(ref reg_SelectGray_AbsolNG);
                    if (!AOI_absolNG_Insp(Recipe.Param.InspParam_BrightBlob_BL.cls_absolNG, Recipe.Param.InspParam_BrightBlob_BL.ImageIndex, ho_DefectReg_BrightBlob, out Reg_absolNG, out reg_SelectGray_AbsolNG))
                    {
                        Extension.HObjectMedthods.ReleaseHObject(ref Reg_absolNG);
                        Dict_AOI_absolNG["電極區白點 (背光)"].DefectReg = ho_DefectReg_BrightBlob.Clone();
                        return false;
                    }
                    else
                    {
                        Dict_AOI_absolNG["電極區白點 (背光)"].DefectReg = Reg_absolNG;

                        #region 加入絕對NG Region所在Cell之其他瑕疵Region

                        HObject Reg_absolNG_Cell;
                        if (Recipe.Param.InspParam_BrightBlob_BL.cls_absolNG.Enabled)
                        {
                            HObject temp;
                            HTuple hv_area_InspRects_Defect = new HTuple(), hv_Greater = new HTuple(), hv_Index_defect = new HTuple();
                            HOperatorSet.Intersection(ho_InspRects_AllCells, Reg_absolNG, out temp);
                            HOperatorSet.RegionFeatures(temp, "area", out hv_area_InspRects_Defect);
                            HOperatorSet.TupleGreaterElem(hv_area_InspRects_Defect, 0.0, out hv_Greater);
                            HOperatorSet.TupleFind(hv_Greater, 1, out hv_Index_defect);

                            if ((int)(new HTuple(hv_Index_defect.TupleNotEqual(-1))) == 1)
                            {
                                hv_Index_defect = hv_Index_defect + 1;
                                HOperatorSet.SelectObj(ho_InspRects_AllCells, out Reg_absolNG_Cell, hv_Index_defect);
                            }
                            else
                                Reg_absolNG_Cell = Reg_absolNG.Clone();
                            temp.Dispose();
                        }
                        else
                            Reg_absolNG_Cell = Reg_absolNG.Clone();

                        #endregion
                        
                        {
                            HObject AOI_NG;
                            HOperatorSet.Difference(ho_DefectReg_BrightBlob, Reg_absolNG_Cell, out AOI_NG);
                            Dict_AOI_NG["電極區白點 (背光)"].DefectReg = AOI_NG;
                        }
                        Reg_absolNG_Cell.Dispose();
                    }
                }
                else
                {
                    Dict_AOI_absolNG["電極區白點 (背光)"].DefectReg = ho_DefectReg_BrightBlob.Clone();
                }

                #endregion

                Res = true;
            }
            catch (Exception ex)
            {
                Log.WriteLog("Error: BrightBlob_BL_Insp() 執行發生例外狀況 -> " + ex.ToString());
                return false;
            }

            return Res;
        }

        /// <summary>
        /// 檢測保缺角 (背光)
        /// </summary>
        /// <param name="Recipe"></param>
        /// <returns></returns>
        public bool LackAngle_BL_Insp(ResistPanelRole Recipe) // (20190226) Jeff Revised!
        {
            bool Res = false;

            try
            {
                #region Execute LackAngle_BL_Insp()

                // Initialize local and output iconic variables
                HOperatorSet.GenEmptyObj(out ho_DefectReg_LackAngle_BL);

                /* 設定檢測ROI */
                // μm 轉 pixel
                HTuple hv_W_Cell = Recipe.Param.InspParam_LackAngle_BL.W_Cell / Locate_Method_FS.pixel_resolution_;
                HTuple hv_H_Cell = Recipe.Param.InspParam_LackAngle_BL.H_Cell / Locate_Method_FS.pixel_resolution_;
                HTuple hv_x_BypassReg = Recipe.Param.InspParam_LackAngle_BL.x_BypassReg / Locate_Method_FS.pixel_resolution_;
                HTuple hv_y_BypassReg = Recipe.Param.InspParam_LackAngle_BL.y_BypassReg / Locate_Method_FS.pixel_resolution_;
                HTuple hv_W_BypassReg = Recipe.Param.InspParam_LackAngle_BL.W_BypassReg / Locate_Method_FS.pixel_resolution_;
                HTuple hv_H_BypassReg = Recipe.Param.InspParam_LackAngle_BL.H_BypassReg / Locate_Method_FS.pixel_resolution_;
                HTuple hv_H_BlackStripe = Recipe.Param.InspParam_LackAngle_BL.H_BlackStripe / Locate_Method_FS.pixel_resolution_;
                // Cell ROI
                if (Recipe.Param.InspParam_LackAngle_BL.Enabled_Cell)
                {
                    HOperatorSet.TupleGenConst(hv_count_AllCells, 0.5 * hv_W_Cell, out hv_L1_AllCells);
                    HOperatorSet.TupleGenConst(hv_count_AllCells, 0.5 * hv_H_Cell, out hv_L2_AllCells);
                    HOperatorSet.GenRectangle2(out ho_Rect_AllCells, hv_r_AllCells, hv_c_AllCells, hv_phi_AllCells, hv_L1_AllCells, hv_L2_AllCells);
                }
                else
                    HOperatorSet.GenEmptyRegion(out ho_Rect_AllCells);
                // Cell內忽略區域
                if (Recipe.Param.InspParam_LackAngle_BL.Enabled_BypassReg)
                {
                    HTuple hv_r_BypassReg = hv_r_AllCells + hv_y_BypassReg;
                    HTuple hv_c_BypassReg = hv_c_AllCells + hv_x_BypassReg;
                    HTuple hv_L1_BypassReg = new HTuple(), hv_L2_BypassReg = new HTuple();
                    HOperatorSet.TupleGenConst(hv_count_AllCells, 0.5 * hv_W_BypassReg, out hv_L1_BypassReg);
                    HOperatorSet.TupleGenConst(hv_count_AllCells, 0.5 * hv_H_BypassReg, out hv_L2_BypassReg);
                    HOperatorSet.GenRectangle2(out ho_Rect_BypassReg, hv_r_BypassReg, hv_c_BypassReg, hv_phi_AllCells, hv_L1_BypassReg, hv_L2_BypassReg);
                }
                else
                    HOperatorSet.GenEmptyRegion(out ho_Rect_BypassReg);
                // 電阻ROI
                if (Recipe.Param.InspParam_LackAngle_BL.Enabled_BlackStripe)
                {
                    HOperatorSet.TupleGenConst(hv_count_BlackStripe, 0.5 * hv_H_BlackStripe, out hv_L2_BlackStripe);
                    HOperatorSet.GenRectangle2(out ho_Rect_BlackStripe, hv_r_BlackStripe, hv_c_BlackStripe, hv_Phi_BlackStripe, hv_L1_BlackStripe, hv_L2_BlackStripe);
                }
                else
                    HOperatorSet.GenEmptyRegion(out ho_Rect_BlackStripe);


                /* 分區做直方圖等化 */
                if (Recipe.Param.InspParam_LackAngle_BL.Enabled_EquHisto)
                {
                    //消除亮區，以避免影響直方圖等化效果
                    HOperatorSet.ReduceDomain(ho_GrayImage_BL, ho_Regions_Inspect1, out ho_ImageReduced_Inspect_BL);
                    HOperatorSet.Threshold(ho_ImageReduced_Inspect_BL, out ho_Regions_Bright_BL, Recipe.Param.InspParam_LackAngle_BL.MinGray_Bright_BL, Recipe.Param.InspParam_LackAngle_BL.MaxGray_Bright_BL);

                    HOperatorSet.GenImageConst(out ho_Image_select, "byte", hv_Width, hv_Height);
                    HOperatorSet.GenImageConst(out ho_Image_Bright, "byte", hv_Width, hv_Height);
                    for (int i_InspectRegions = 1; i_InspectRegions <= hv_Num_InspectRegions; i_InspectRegions++)
                    {
                        HOperatorSet.SelectObj(ho_Inspect_ConnectReg, out ho_OneInspectReg, i_InspectRegions);
                        HOperatorSet.Difference(ho_OneInspectReg, ho_Regions_Bright_BL, out ho_RegionDifference);
                        HOperatorSet.ReduceDomain(ho_ImageReduced_Inspect_BL, ho_RegionDifference, out ho_ImageReduced_Inspect2_BL);

                        //分區做直方圖等化
                        // μm 轉 pixel
                        HTuple hv_w_Part = Recipe.Param.InspParam_LackAngle_BL.w_Part / Locate_Method_FS.pixel_resolution_;
                        HTuple hv_h_Part = Recipe.Param.InspParam_LackAngle_BL.h_Part / Locate_Method_FS.pixel_resolution_;
                        HOperatorSet.PartitionRectangle(ho_RegionDifference, out ho_Partitioned, hv_w_Part, hv_h_Part);
                        HOperatorSet.RegionFeatures(ho_Partitioned, "row1", out hv_Value_R1);
                        HOperatorSet.RegionFeatures(ho_Partitioned, "column1", out hv_Value_C1);
                        HOperatorSet.RegionFeatures(ho_Partitioned, "row2", out hv_Value_R2);
                        HOperatorSet.RegionFeatures(ho_Partitioned, "column2", out hv_Value_C2);
                        HOperatorSet.CropRectangle1(ho_ImageReduced_Inspect2_BL, out ho_ImagePart, hv_Value_R1, hv_Value_C1, hv_Value_R2, hv_Value_C2);
                        HOperatorSet.EquHistoImage(ho_ImagePart, out ho_ImageEquHisto2);
                        HOperatorSet.TupleGenConst(new HTuple(hv_Value_R1.TupleLength()), -1, out hv_tile_rect);
                        HOperatorSet.TileImagesOffset(ho_ImageEquHisto2, out ho_TiledImage, hv_Value_R1, hv_Value_C1, hv_tile_rect, hv_tile_rect, hv_tile_rect, hv_tile_rect, hv_Width, hv_Height);

                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.PaintGray(ho_TiledImage, ho_Image_select, out ExpTmpOutVar_0);
                            ho_Image_select.Dispose();
                            ho_Image_select = ExpTmpOutVar_0;
                        }
                    }

                    // 加入被消除之亮區
                    //HOperatorSet.ReduceDomain(ho_GrayImage_BL, ho_Regions_Bright_BL, out ho_ImageReduced_Bright_BL);
                    //{
                    //    HObject ExpTmpOutVar_0;
                    //    HOperatorSet.PaintGray(ho_ImageReduced_Bright_BL, ho_Image_select, out ExpTmpOutVar_0);
                    //    ho_Image_select.Dispose();
                    //    ho_Image_select = ExpTmpOutVar_0;
                    //}
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.PaintRegion(ho_Regions_Bright_BL, ho_Image_select, out ExpTmpOutVar_0, 255, "fill");
                        ho_Image_select.Dispose();
                        ho_Image_select = ExpTmpOutVar_0;
                    }

                }
                else
                {
                    HOperatorSet.GenEmptyRegion(out ho_Regions_Bright_BL);
                    HOperatorSet.CopyObj(ho_GrayImage_BL, out ho_Image_select, 1, -1);
                }


                /* 二值化處理 */
                // Cell ROI
                if (Recipe.Param.InspParam_LackAngle_BL.Enabled_BypassReg)
                {
                    HOperatorSet.Difference(ho_Rect_AllCells, ho_Rect_BypassReg, out ho_RegDif_Cell);
                    HOperatorSet.Union1(ho_RegDif_Cell, out ho_Rect_AllCells_union);
                }
                else
                {
                    HOperatorSet.Union1(ho_Rect_AllCells, out ho_Rect_AllCells_union);
                }
                HOperatorSet.ReduceDomain(ho_Image_select, ho_Rect_AllCells_union, out ho_ImageReduced_AllCells_LackAngle_BL);
                HOperatorSet.Threshold(ho_ImageReduced_AllCells_LackAngle_BL, out ho_Reg_Cell_LackAngle_BL, Recipe.Param.InspParam_LackAngle_BL.MinGray_Cell, 255);
                // 電阻ROI
                HOperatorSet.Union1(ho_Rect_BlackStripe, out ho_Rect_BlackStripe_union);
                HOperatorSet.Union1(ho_Rect_AllCells, out ho_Rect_AllCells_union1);
                HOperatorSet.Union1(ho_Rect_BypassReg, out ho_Rect_BypassReg_union);
                HOperatorSet.Difference(ho_Rect_BlackStripe_union, ho_Rect_AllCells_union1, out ho_RegDif_BlackStripe_temp);
                HOperatorSet.Difference(ho_RegDif_BlackStripe_temp, ho_Rect_BypassReg_union, out ho_RegDif_BlackStripe);
                HOperatorSet.ReduceDomain(ho_Image_select, ho_RegDif_BlackStripe, out ho_ImageReduced_BlackStripe_LackAngle_BL);
                HOperatorSet.Threshold(ho_ImageReduced_BlackStripe_LackAngle_BL, out ho_Reg_Resist_LackAngle_BL, Recipe.Param.InspParam_LackAngle_BL.MinGray_Resist, 255);

                // 結合兩區域瑕疵
                HOperatorSet.Union2(ho_Reg_Cell_LackAngle_BL, ho_Reg_Resist_LackAngle_BL, out ho_Reg_LackAngle_BL);

                // 瑕疵Region內至少有一個pixel之灰階值低於一個數值 (瑕疵Region內最低灰階標準)
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Connection(ho_Reg_LackAngle_BL, out ExpTmpOutVar_0);
                    ho_Reg_LackAngle_BL.Dispose();
                    ho_Reg_LackAngle_BL = ExpTmpOutVar_0;
                }
                HOperatorSet.SelectGray(ho_Reg_LackAngle_BL, ho_Image_select, out ho_Reg_LackAngle_BL_select, "max", "and", Recipe.Param.InspParam_LackAngle_BL.Gray_select, 255);



                /* 瑕疵尺寸篩選 */
                // μm 轉 pixel
                hv_MinHeight_defect = Recipe.Param.InspParam_LackAngle_BL.MinHeight_defect / Locate_Method_FS.pixel_resolution_;
                hv_MaxHeight_defect = Recipe.Param.InspParam_LackAngle_BL.MaxHeight_defect / Locate_Method_FS.pixel_resolution_;
                hv_MinWidth_defect = Recipe.Param.InspParam_LackAngle_BL.MinWidth_defect / Locate_Method_FS.pixel_resolution_;
                hv_MaxWidth_defect = Recipe.Param.InspParam_LackAngle_BL.MaxWidth_defect / Locate_Method_FS.pixel_resolution_;
                hv_MinArea_defect = Recipe.Param.InspParam_LackAngle_BL.MinArea_defect / (Locate_Method_FS.pixel_resolution_ * Locate_Method_FS.pixel_resolution_);
                hv_MaxArea_defect = Recipe.Param.InspParam_LackAngle_BL.MaxArea_defect / (Locate_Method_FS.pixel_resolution_ * Locate_Method_FS.pixel_resolution_);

                //操作1
                if (Recipe.Param.InspParam_LackAngle_BL.Enabled_Height && Recipe.Param.InspParam_LackAngle_BL.Enabled_Width)
                {
                    HOperatorSet.SelectShape(ho_Reg_LackAngle_BL_select, out ho_DefectReg_op1,
                                             (new HTuple("height")).TupleConcat("width"), Recipe.Param.InspParam_LackAngle_BL.str_op1,
                                             hv_MinHeight_defect.TupleConcat(hv_MinWidth_defect),
                                             hv_MaxHeight_defect.TupleConcat(hv_MaxWidth_defect));
                }
                else if (Recipe.Param.InspParam_LackAngle_BL.Enabled_Height)
                {
                    HOperatorSet.SelectShape(ho_Reg_LackAngle_BL_select, out ho_DefectReg_op1, "height",
                                             Recipe.Param.InspParam_LackAngle_BL.str_op1, hv_MinHeight_defect, hv_MaxHeight_defect);
                }
                else if (Recipe.Param.InspParam_LackAngle_BL.Enabled_Width)
                {
                    HOperatorSet.SelectShape(ho_Reg_LackAngle_BL_select, out ho_DefectReg_op1, "width",
                                             Recipe.Param.InspParam_LackAngle_BL.str_op1, hv_MinWidth_defect, hv_MaxWidth_defect);
                }
                else
                    HOperatorSet.CopyObj(ho_Reg_LackAngle_BL_select, out ho_DefectReg_op1, 1, -1);
                //操作2
                if (Recipe.Param.InspParam_LackAngle_BL.Enabled_Area)
                {
                    HOperatorSet.SelectShape(ho_Reg_LackAngle_BL_select, out ho_DefectReg_op2, "area",
                                             Recipe.Param.InspParam_LackAngle_BL.str_op2, hv_MinArea_defect, hv_MaxArea_defect);
                    HOperatorSet.Union1(ho_DefectReg_op1, out ho_DefectReg_op1_union);
                    HOperatorSet.Union1(ho_DefectReg_op2, out ho_DefectReg_op2_union);
                    if (Recipe.Param.InspParam_LackAngle_BL.str_op2 == "and")
                        HOperatorSet.Intersection(ho_DefectReg_op1_union, ho_DefectReg_op2_union, out ho_DefectReg_LackAngle_BL);
                    else if (Recipe.Param.InspParam_LackAngle_BL.str_op2 == "or")
                        HOperatorSet.Union2(ho_DefectReg_op1_union, ho_DefectReg_op2_union, out ho_DefectReg_LackAngle_BL);

                    // (20190331) Jeff Revised!
                    //{
                    //    HObject ExpTmpOutVar_0;
                    //    HOperatorSet.Connection(ho_DefectReg_LackAngle_BL, out ExpTmpOutVar_0);
                    //    ho_DefectReg_LackAngle_BL.Dispose();
                    //    ho_DefectReg_LackAngle_BL = ExpTmpOutVar_0;
                    //}
                }
                else
                    HOperatorSet.CopyObj(ho_DefectReg_op1, out ho_DefectReg_LackAngle_BL, 1, -1);

                // 旋轉瑕疵region回原本方向
                //if (Recipe.Param.Enabled_rotate)
                //{
                //    HOperatorSet.AffineTransRegion(ho_DefectReg_LackAngle.Clone(), out ho_DefectReg_LackAngle, hommat2d_origin_, "nearest_neighbor");
                //}

                // (20190807) Jeff Revised!
                if (Recipe.Param.B_AOI_absolNG) // 是否啟用AOI判斷絕對NG
                {
                    HObject Reg_absolNG;
                    Extension.HObjectMedthods.ReleaseHObject(ref reg_SelectGray_AbsolNG);
                    if (!AOI_absolNG_Insp(Recipe.Param.InspParam_LackAngle_BL.cls_absolNG, Recipe.Param.InspParam_LackAngle_BL.ImageIndex, ho_DefectReg_LackAngle_BL, out Reg_absolNG, out reg_SelectGray_AbsolNG))
                    {
                        Extension.HObjectMedthods.ReleaseHObject(ref Reg_absolNG);
                        Dict_AOI_absolNG["保缺角 (背光)"].DefectReg = ho_DefectReg_LackAngle_BL.Clone();
                        return false;
                    }
                    else
                    {
                        Dict_AOI_absolNG["保缺角 (背光)"].DefectReg = Reg_absolNG;

                        #region 加入絕對NG Region所在Cell之其他瑕疵Region

                        HObject Reg_absolNG_Cell;
                        if (Recipe.Param.InspParam_LackAngle_BL.cls_absolNG.Enabled)
                        {
                            HObject temp;
                            HTuple hv_area_InspRects_Defect = new HTuple(), hv_Greater = new HTuple(), hv_Index_defect = new HTuple();
                            HOperatorSet.Intersection(ho_InspRects_AllCells, Reg_absolNG, out temp);
                            HOperatorSet.RegionFeatures(temp, "area", out hv_area_InspRects_Defect);
                            HOperatorSet.TupleGreaterElem(hv_area_InspRects_Defect, 0.0, out hv_Greater);
                            HOperatorSet.TupleFind(hv_Greater, 1, out hv_Index_defect);

                            if ((int)(new HTuple(hv_Index_defect.TupleNotEqual(-1))) == 1)
                            {
                                hv_Index_defect = hv_Index_defect + 1;
                                HOperatorSet.SelectObj(ho_InspRects_AllCells, out Reg_absolNG_Cell, hv_Index_defect);
                            }
                            else
                                Reg_absolNG_Cell = Reg_absolNG.Clone();
                            temp.Dispose();
                        }
                        else
                            Reg_absolNG_Cell = Reg_absolNG.Clone();

                        #endregion
                        
                        {
                            HObject AOI_NG;
                            HOperatorSet.Difference(ho_DefectReg_LackAngle_BL, Reg_absolNG_Cell, out AOI_NG);
                            Dict_AOI_NG["保缺角 (背光)"].DefectReg = AOI_NG;
                        }
                        Reg_absolNG_Cell.Dispose();
                    }
                }
                else
                {
                    Dict_AOI_absolNG["保缺角 (背光)"].DefectReg = ho_DefectReg_LackAngle_BL.Clone();
                }

                #endregion

                Res = true;
            }
            catch (Exception ex)
            {
                Log.WriteLog("Error: LackAngle_BL_Insp() 執行發生例外狀況 -> " + ex.ToString());
                return false;
            }

            return Res;
        }

        /// <summary>
        /// 檢測保缺角 (側光)
        /// </summary>
        /// <param name="Recipe"></param>
        /// <returns></returns>
        public bool LackAngle_SL_Insp(ResistPanelRole Recipe) // (20190313) Jeff Revised!
        {
            bool Res = false;

            try
            {
                #region Execute LackAngle_SL_Insp()

                // Initialize local and output iconic variables
                HOperatorSet.GenEmptyObj(out ho_DefectReg_LackAngle_SL);

                if (!Cell_BlackStripe_Insp(Recipe, Recipe.Param.InspParam_LackAngle_SL, out ho_DefectReg_LackAngle_SL))
                    return false;

                // 旋轉瑕疵region回原本方向
                //if (Recipe.Param.Enabled_rotate)
                //{
                //    HOperatorSet.AffineTransRegion(ho_DefectReg_LackAngle_SL.Clone(), out ho_DefectReg_LackAngle_SL, hommat2d_origin_, "nearest_neighbor");
                //}

                // (20190807) Jeff Revised!
                if (Recipe.Param.B_AOI_absolNG) // 是否啟用AOI判斷絕對NG
                {
                    HObject Reg_absolNG;
                    Extension.HObjectMedthods.ReleaseHObject(ref reg_SelectGray_AbsolNG);
                    if (!AOI_absolNG_Insp(Recipe.Param.InspParam_LackAngle_SL.cls_absolNG, Recipe.Param.InspParam_LackAngle_SL.ImageIndex, ho_DefectReg_LackAngle_SL, out Reg_absolNG, out reg_SelectGray_AbsolNG))
                    {
                        Extension.HObjectMedthods.ReleaseHObject(ref Reg_absolNG);
                        Dict_AOI_absolNG["保缺角 (側光)"].DefectReg = ho_DefectReg_LackAngle_SL.Clone();
                        return false;
                    }
                    else
                    {
                        Dict_AOI_absolNG["保缺角 (側光)"].DefectReg = Reg_absolNG;

                        #region 加入絕對NG Region所在Cell之其他瑕疵Region

                        HObject Reg_absolNG_Cell;
                        if (Recipe.Param.InspParam_LackAngle_SL.cls_absolNG.Enabled)
                        {
                            HObject temp;
                            HTuple hv_area_InspRects_Defect = new HTuple(), hv_Greater = new HTuple(), hv_Index_defect = new HTuple();
                            HOperatorSet.Intersection(ho_InspRects_AllCells, Reg_absolNG, out temp);
                            HOperatorSet.RegionFeatures(temp, "area", out hv_area_InspRects_Defect);
                            HOperatorSet.TupleGreaterElem(hv_area_InspRects_Defect, 0.0, out hv_Greater);
                            HOperatorSet.TupleFind(hv_Greater, 1, out hv_Index_defect);

                            if ((int)(new HTuple(hv_Index_defect.TupleNotEqual(-1))) == 1)
                            {
                                hv_Index_defect = hv_Index_defect + 1;
                                HOperatorSet.SelectObj(ho_InspRects_AllCells, out Reg_absolNG_Cell, hv_Index_defect);
                            }
                            else
                                Reg_absolNG_Cell = Reg_absolNG.Clone();
                            temp.Dispose();
                        }
                        else
                            Reg_absolNG_Cell = Reg_absolNG.Clone();

                        #endregion
                        
                        {
                            HObject AOI_NG;
                            HOperatorSet.Difference(ho_DefectReg_LackAngle_SL, Reg_absolNG_Cell, out AOI_NG);
                            Dict_AOI_NG["保缺角 (側光)"].DefectReg = AOI_NG;
                        }
                        Reg_absolNG_Cell.Dispose();
                    }
                }
                else
                {
                    Dict_AOI_absolNG["保缺角 (側光)"].DefectReg = ho_DefectReg_LackAngle_SL.Clone();
                }

                #endregion

                Res = true;
            }
            catch (Exception ex)
            {
                Log.WriteLog("Error: LackAngle_SL_Insp() 執行發生例外狀況 -> " + ex.ToString());
                return false;
            }

            return Res;
        }

        public void set_parameter(ResistPanelRole role)
        {
            methRole = role;
        }
        
        #region //=====================  AOI Function =====================
        
        #endregion
    }
}
