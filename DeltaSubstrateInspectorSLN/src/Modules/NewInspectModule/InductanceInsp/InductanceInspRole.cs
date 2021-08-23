using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeltaSubstrateInspector.src.Roles;
using System.Xml;
using static DeltaSubstrateInspector.FileSystem.FileSystem;
using System.IO;
using HalconDotNet;
using System.Diagnostics;
using System.Drawing;
using DAVS;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Reflection;
using System.Runtime.InteropServices;

using DeltaSubstrateInspector.src.Modules.Algorithm; // (20200130) Jeff Revised!

namespace DeltaSubstrateInspector.src.Modules.NewInspectModule.InductanceInsp
{

    #region Enum

    public enum enuMLPFeatureType
    {
        texture_Laws
    }

    public enum enuMLPTextureLaws
    {
        ll,
        le,
        ls,
        lw,
        lr,
        lu,
        lo,
        el,
        ee,
        es,
        ew,
        er,
        eu,
        eo,
        sl,
        se,
        ss,
        sw,
        sr,
        su,
        so,
        wl,
        we,
        ws,
        ww,
        wr,
        wu,
        wo,
        rl,
        re,
        rs,
        rw,
        rr,
        ru,
        ro,
        ul,
        ue,
        us,
        uw,
        ur,
        uu,
        uo,
        ol,
        oe,
        os,
        ow,
        or,
        ou,
        oo
    }
    

    public enum enuMethod
    {
        Inner,
        Outer,
        Thin,
        Scratch,
        Stain,
        RDefect,
        AI,
    }

    /// <summary>
    /// 影像來源
    /// </summary>
    public enum enu_ImageSource // (20200130) Jeff Revised!
    {
        原始,
        影像A,
        影像B,
    }

    public enum enuBand
    {
        R,
        G,
        B,
        Gray,
        y,
        u,
        v,
        v_sub_u,
        R_sub_B,
        R_sub_G,
        B_sub_G,
        h,
        s,
        i,
        /// <summary>
        /// 不選
        /// </summary>
        None // (20191226) MIL Jeff Revised!
    }

    public enum enuSearchTransition
    {
        negative,
        positive,
        all,
        uniform
    }

    public enum enuBinaryThType
    {
        max_separability,
        smooth_histo
    }

    public enum enuBinaryThSearchType
    {
        dark,
        light
    }

    public enum enuFillupType
    {
        anisometry,
        area,
        compactness,
        convexity,
        inner_circle,
        outer_circle,
        phi,
        ra,
        rb
    }
    public enum enuOperation
    {
        and,
        or
    }

    public enum enuGraySelectType
    {
        mean,
        min,
        max,
        deviation
    }

    public enum enuTransType
    {
        convex,
        ellipse,
        outer_circle,
        inner_circle,
        rectangle1,
        rectangle2,
        inner_rectangle1,
        inner_center
    }

    public enum enuDAVSMode
    {
        Online,
        Offline
    }

    public enum enuPredictionMode
    {
        BGR,
        RGB
    }

    public enum enuLineModel
    {
        gaussian,
        parabolic,
        barshaped,
        none,
    }

    /// <summary>
    /// set_texture_inspection_model_param 之參數 patch_normalization
    /// </summary>
    public enum enu_patch_normalization // (20200319) Jeff Revised!
    {
        weber,
        none
    }

    #endregion

    [Serializable]
    public class clsRecipe
    {
        //System Type
        //測試模式Type
        public bool TestModeEnabled = false;
        public int TestModeType = 0;
        public bool LogEnabled = true;

        //AOI儲存影像參數
        public bool SaveAOIImgEnabled = false;
        public int SaveAOIImgType = 0;

        //AI Param
        public bool bRotateCellImage = false;
        public bool bInspOutboundary = false;
        public int DAVSInspType = 0;// 0:全部檢測, 1:只檢測NG , 2:只檢測OK
        public int DAVSImgID = 0;
        public bool bDAVSMixImgBand = false;
        public int DAVSBand1ImgIndex = 0;
        public int DAVSBand2ImgIndex = 0;
        public int DAVSBand3ImgIndex = 0;
        public enuBand DAVSBand1 = enuBand.R;
        public enuBand DAVSBand2 = enuBand.G;
        public enuBand DAVSBand3 = enuBand.B;

        //自動同步功能參數
        public string SynchronizePath = "";
        public bool bAutoSynchronizeRecipe = false;


        //Pattern Match
        /// <summary>
        /// Pattern Match 教導參數 參數
        /// </summary>
        public clsAdvParam AdvParam = new clsAdvParam();
        /// <summary>
        /// 切割影像參數
        /// </summary>
        public clsSegParam SegParam = new clsSegParam();
        /// <summary>
        /// 影像旋轉: 原始影像是否先轉正
        /// </summary>
        public bool RotateImageEnabled { get; set; } = false;

        //AOI Insp
        public clsAOIParam InnerInspParam = new clsAOIParam();//Pattern內部檢測參數
        public clsAOIParam OuterInspParam = new clsAOIParam();//Pattern外部檢測參數
        public clsRisistAOIParam RisistAOIParam = new clsRisistAOIParam();//電阻檢測參數

        public int ImageWidth = 4096;
        public int ImageHeight = 3000;

        /// <summary>
        /// 所有AOI演算法之優先權中之最大值
        /// </summary>
        public int PriorityLayerCount { get; set; } = 1;

        // System

        /// <summary>
        /// 拍攝影像數量
        /// </summary>
        public int ImgCount = 2;

        /// <summary>
        /// 顯示方法
        /// </summary>
        public List<clsMethod> MethodList { get; set; } = new List<clsMethod>();

        /// <summary>
        /// Region 工具
        /// </summary>
        public List<clsMethodRegion> MethodRegionList { get; set; } = new List<clsMethodRegion>();

        /// <summary>
        /// 編輯Region
        /// </summary>
        public List<clsMethodRegion> EditRegionList { get; set; } = new List<clsMethodRegion>();
        
        /// <summary>
        /// 實際使用之Region
        /// </summary>
        public List<clsMethodRegion> UsedRegionList { get; set; } = new List<clsMethodRegion>();

        /// <summary>
        /// 影像演算法物件 (不使用範圍列表)
        /// </summary>
        public Algo_Image AlgoImg { get; set; } = new Algo_Image(); // (20200130) Jeff Revised!

        /// <summary>
        /// 影像演算法物件 (使用範圍列表)
        /// </summary>
        public Algo_Image AlgoImg_UsedRegion { get; set; } = new Algo_Image(); // (20200130) Jeff Revised!

        /// <summary>
        /// 演算法List
        /// </summary>
        public List<clsAlgorithm> AlgorithmList { get; set; } = new List<clsAlgorithm>();


        public clsRecipe()
        {

        }

        #region  ================ XmlInclude =================

        [XmlInclude(typeof(clsThresholdInsp))]
        [XmlInclude(typeof(clsLaserHoleInsp))]
        [XmlInclude(typeof(clsTextureInsp))]
        [XmlInclude(typeof(clsFeatureInsp))]
        [XmlInclude(typeof(clsDAVSInsp))]
        [XmlInclude(typeof(clsLineSearchInsp))]
        [XmlInclude(typeof(clsFeatureLaws))]

        #endregion

        [Serializable]
        public class clsAlgorithm // (20200130) Jeff Revised!
        {
            /// <summary>
            /// AOI演算法類型
            /// </summary>
            public InductanceInsp.enuAlgorithm Type { get; set; } // (20200409) Jeff Revised!
            /// <summary>
            /// 該AOI演算法物件 (檢測參數Recipe)
            /// </summary>
            public object Param { get; set; } = new object();
            public string Name = "Insp";
            /// <summary>
            /// Region ID
            /// </summary>
            public int SelectRegionIndex { get; set; } = 0;
            public enu_ImageSource ImageSource { get; set; } = enu_ImageSource.原始; // (20200130) Jeff Revised!
            public int InspImageIndex { get; set; } = 0;
            public bool bUsedDAVS = false;
            public enuBand Band = enuBand.R;
            public string DefectName = "DefectName";
            /// <summary>
            /// 優先權
            /// </summary>
            public int Priority { get; set; } = 0;
            public string Color { get; set; } = "#0000ff40"; // (20200409) Jeff Revised!
            public int SecondSrcImgID = 0;
            public enuBand SecondImgBand = enuBand.R;

            public clsAlgorithm() { }
        }

        [Serializable]
        public class clsLineSearchInsp
        {
            public bool Enabled = true;

            //邊緣忽略、擴張大小
            public int EdgeSkipSizeWidth = 1;
            public int EdgeSkipSizeHeight = 1;
            public int EdgeExtSizeWidth = 1;
            public int EdgeExtSizeHeight = 1;

            //Line Gauss
            public double Sigma = 2;
            public int Low = 4;
            public int High = 8;

            public enuBinaryThSearchType LightDark = enuBinaryThSearchType.dark;
            public enuLineModel LineModel = enuLineModel.gaussian;

            //Line union
            public int MaxDistAbs = 20;
            public int MaxDistRel = 1;
            public int MaxShift = 2;
            public double MaxAngle = 0.1;

            //Select Length
            public int LengthMin = 10;
            public int LengthMax = 99999;

            //檢出規格
            public bool WEnabled = true;
            public int WidthMin = 17;
            public int WidthMax = 99999;

            public bool HEnabled = true;
            public int HeightMin = 17;
            public int HeightMax = 99999;

            public bool AEnabled = true;
            public int AreaMin = 300;
            public int AreaMax = 999999;

            public bool anisometryEnabled = false;
            public double anisometryMin = 3.0;
        }

        [Serializable]
        public class clsDAVSInsp
        {
            public bool Enabled = true;

            //邊緣忽略、擴張大小
            public int EdgeSkipSizeWidth = 1;
            public int EdgeSkipSizeHeight = 1;
            public int EdgeExtSizeWidth = 1;
            public int EdgeExtSizeHeight = 1;
            
            public string ModelPath = "";
            public string SaveImagePath = "D://DSI//Image//DAVSInspImage//";

            public bool bFixSize = false;
            public int FixSizeWidth = 50;
            public int FixSizeHeight = 50;
            
            public enuDAVSMode DAVS_InspMode = enuDAVSMode.Offline;
            public enuPredictionMode DAVS_PredictMode = enuPredictionMode.RGB; // Predict模式, 0:bgr , 1: rgb
            public List<bool> DAVS_SaveImgEnabledList = new List<bool>();//分類是否儲存影像
            public string DAVS_ImgHSDir = "C:\\AI_Img";// AOI與AI交握影像資料夾

            public clsDAVSInsp() { }
        }

        [Serializable]
        public class clsFeatureLaws
        {
            public enuMLPTextureLaws FeatureLaws = enuMLPTextureLaws.ee;
            public int Size = 3;
            public int Shift = 0;

            public clsFeatureLaws() { }
        }

        [Serializable]
        public class clsFeatureInsp
        {
            public bool Enabled = true;
            
            //邊緣忽略、擴張大小
            public int EdgeSkipSizeWidth = 1;
            public int EdgeSkipSizeHeight = 1;
            public int EdgeExtSizeWidth = 1;
            public int EdgeExtSizeHeight = 1;
            public string ModelPath = "";

            public List<clsFeatureLaws> FeatureLaws = new List<clsFeatureLaws>();

            public int NumHidden = 10;

            //檢出規格
            public bool WEnabled = true;
            public int WidthMin = 17;

            public bool HEnabled = true;
            public int HeightMin = 17;

            public bool AEnabled = true;
            public int AreaMin = 300;

            public clsFeatureInsp() { }
        }

        [Serializable]
        public class clsTextureInsp
        {
            public bool Enabled = true;

            // 邊緣忽略、擴張大小
            public int EdgeSkipSizeWidth { get; set; } = 1;
            public int EdgeSkipSizeHeight = 1;
            public int EdgeExtSizeWidth = 1;
            public int EdgeExtSizeHeight = 1;

            public int Sensitivity = 5;
            public int LevelStart = 2;
            public int LevelEnd = 5;
            public int max_Iter = 100;
            public string ModelPath = "";
            public enu_patch_normalization patch_normalization { get; set; } = enu_patch_normalization.weber; // (20200319) Jeff Revised!
            public bool patch_rotational_robustness { get; set; } = false; // (20200319) Jeff Revised!

            // 檢測ROI (20200319) Jeff Revised!
            public bool ROI_Enabled { get; set; } = false;
            public int ROI_SkipWidth { get; set; } = 1;
            public int ROI_SkipHeight { get; set; } = 1;

            // 檢出規格
            public bool WEnabled { get; set; } = true;
            public int LowWidth { get; set; } = 17;

            public bool HEnabled { get; set; } = true;
            public int LowHeight { get; set; } = 17;

            public bool AEnabled { get; set; } = true;
            public int LowArea { get; set; } = 300;

            public bool anisometryEnabled { get; set; } = false;
            public double anisometryMin { get; set; } = 3.0;

            public clsTextureInsp() { }
        }

        [Serializable]
        public class clsLaserHoleInsp
        {
            //必要變數
            public bool Enabled = true;
            public int Acceptable_Value = 0;
            public bool bAcceptable_ValueEnabled = false;

            //邊緣忽略、擴張大小
            public int EdgeSkipSizeWidth = 1;
            public int EdgeSkipSizeHeight = 1;
            public int EdgeExtSizeWidth = 1;
            public int EdgeExtSizeHeight = 1;

            public bool bSearchFailPass = false;

            public bool bSearchSizePass = false;
            public double SearchRadiusMin = 0.0;
            public double SearchRadiusMax = 9999.0;

            #region 搜尋圓孔參數

            public int SearchCircleRadius = 15;
            public int SearchRadiusTolerance = 20;
            public int measureLength2 = 10;
            public double MeasureSigma = 5;
            public int MeasureThreshold = 20;
            public int num_instances = 1;
            public enuSearchTransition Trans = enuSearchTransition.negative;
            public double min_score = 0.9;

            public double ErosionSize = 2.0;

            #endregion

            #region 特徵參數

            //平均值
            public int MeanID = 0;
            public enuBand MeanBand = enuBand.R;
            public bool bMeanEnabled = false;
            public double MeanMin = 100;

            //半徑
            public bool bRadius = false;
            public double RadiusMin = 10;
            public double RadiusMax = 14;

            //灰階最小值
            public int MinID = 0;
            public enuBand GrayMinBand = enuBand.R;
            public bool bGrayMin = false;
            public int GrayMin = 75;

            //黑色面積比例
            public int RatioID = 0;
            public enuBand DarkRatioBand = enuBand.R;
            public bool bDarkRatio = false;
            public double DarkRatioMin = 10;

            public bool bDynThreshold = false;
            public int MeanImgWidth = 5;
            public int MeanImgHeight = 5;
            public int DynOffset = 10;

            public bool ThresholdEnabled = true;
            public int ThresholdMin = 0;
            public int ThresholdMax = 95;


            #endregion

            //檢出規格
            public bool WEnabled = true;
            public int LowWidth = 10;

            public bool HEnabled = true;
            public int LowHeight = 10;

            public bool AEnabled = true;
            public int LowArea = 10;

            public clsLaserHoleInsp() { }
        }

        /// <summary>
        /// 各種二值化之參數
        /// </summary>
        [Serializable]
        public class clsThresholdInsp
        {
            //必要變數
            public bool Enabled = true;
            public InductanceInsp.enuThresholdType ThType = InductanceInsp.enuThresholdType.GeneralThreshold;
            public enuOperation Operation = enuOperation.and;

            public int Acceptable_Value = 0;
            public bool bAcceptable_ValueEnabled = false;

            public bool bEdgeEdit = true;

            #region 一般二值化

            public int LTH = 128;
            public int HTH = 255;

            #endregion

            #region 自動二值化

            public double Sigma = 2.0;
            public int SelectMean = 0;//0 : Dark   ,    1 : Bright

            #endregion

            #region 動態二值化

            public int GrayMeanWidth = 5;
            public int GrayMeanHeight = 5;
            public int Offset = 5;
            public InductanceInsp.enuDynThresholdType DynType = InductanceInsp.enuDynThresholdType.dark;

            #endregion

            #region  Binary Threshold

            public enuBinaryThType BinaryThType = enuBinaryThType.smooth_histo;
            public enuBinaryThSearchType BinaryThSearchType = enuBinaryThSearchType.dark;

            #endregion
            
            #region 特徵篩選

            //邊緣忽略大小
            public int EdgeSkipSizeWidth = 1;
            public int EdgeSkipSizeHeight = 1;

            public double ExtSizeCircle = 0.5;

            public bool CircularityEnabled = false;
            public double CircularityMin = 0.0;
            public double CircularityMax = 1.0;

            public bool RoundnessEnabled = false;
            public double RoundnessMin = 0.0;
            public double RoundnessMax = 1.0;

            public bool ConvexityEnabled = false;
            public double ConvexityMin = 0.0;
            public double ConvexityMax = 1.0;

            public bool RectangularityEnabled = false;
            public double RectangularityMin = 0.0;
            public double RectangularityMax = 1.0;

            public bool Holes_numEnabled = false;
            public int Holes_numMin = 0;
            public int Holes_numMax = 100;

            public bool bInnerCircleEnabled = false;
            public double Inner_radiusMin = 5;
            public double Inner_radiusMax = 999999;

            public bool banisometryEnabled = false;
            public double anisometryMin = 0.0;
            public double anisometryMax = 3.0;

            //灰階篩選
            public bool GrayEnabled = false;
            public int GrayMin = 0;
            public int GrayMax = 255;
            public enuGraySelectType GraySelectType = enuGraySelectType.mean;

            //第二張影像判斷灰階值
            public bool bSecondSrcEnabled = false;
            public int SecondSrcGrayMin = 0;
            public int SecondSrcGrayMax = 255;
            public enuGraySelectType SecondSrcGraySelectType = enuGraySelectType.mean;

            #endregion


            //檢出規格
            public bool WEnabled = true;
            public int WidthMin = 10;
            public int WidthMax = 99999;
            public enuOperation WidthOperation = enuOperation.and;

            public bool HEnabled = true;
            public int HeightMin = 10;
            public int HeightMax = 99999;
            public enuOperation HeightOperation = enuOperation.and;

            public bool AEnabled = true;
            public int AreaMin = 10;
            public int AreaMax = 99999;
            public enuOperation AreaOperation = enuOperation.and;

            public clsThresholdInsp() { }
        }
        
        [Serializable]
        public class clsMethod
        {
            public string Name = "";
            public bool Enabled = false;
            public clsMethod() { }
        }


        [Serializable]
        public class clsMultiTh
        {
            public int LTH = 128;
            public int HTH = 255;
            public int ImageID = 0;
            public enuBand Band = enuBand.R;
            public clsMultiTh() { }
        }

        [Serializable]
        public class clsRisistAOIParam
        {
            public clsRisistAOIParam() { }

            public clsThinParam ThinParam = new clsThinParam();
            public clsStainParam StainParam = new clsStainParam();
            public clsRDefectParam RDefectParam = new clsRDefectParam();
            public clsScratchParam ScratchParam = new clsScratchParam();
            public clsRShiftParam RShiftParam = new clsRShiftParam();
            public clsRShiftNewParam RShiftNewParam = new clsRShiftNewParam();

            [Serializable]
            public class clsRShiftNewParam
            {
                public bool Enabled = false;
                public string Name = "Defect";
                public int ShiftStandard = 12;
                public int SelectArea = 10000;
                public int SelectWidth = 50;
                public double AutoThSigma = 6;
                public int CloseSize = 2;


                public clsRShiftNewParam() { }
            }

                [Serializable]
            public class clsRShiftParam
            {
                public bool Enabled = false;
                public string Name = "R層偏移";
                public int FixDiffValue = 0;
                public int ShiftStandard = 12;
                public int GrayValue = 0;

                public clsRShiftParam() { }
            }

            [Serializable]
            public class clsThinParam
            {
                public bool Enabled = true;

                //劃痕偵測
                public bool ThinScratchEnabled = false;
                public int OpenScratchSizeW = 1;
                public int CloseScratchSizeW = 1;
                public int OpenScratchSizeH = 1;
                public int CloseScratchSizeH = 1;
                public bool ThinScratchAreaEnabled = true;
                public int ThinScratchAreaMin = 25;
                public bool ThinScratchWidthEnabled = true;
                public int ThinScratchWidthMin = 5;
                public bool ThinScratchHeightEnabled = true;
                public int ThinScratchHeightMin = 5;
                public int ThinScratchSensitivity = 15;
                public int ThinScratchEdgeSkipWidth = 1;
                public int ThinScratchEdgeSkipHeight = 1;
                public int ThinScratchEdgeExWidth = 1;
                public int ThinScratchEdgeExHeight = 1;


                //TopHat 大面積
                public bool ThinHatEnabled = false;
                public int ThinHatDarkTh = 20;
                public int ThinHatBrightTh = 20;
                public int ThinHatOpenWidth = 3;
                public int ThinHatOpenHeight = 3;
                public int ThinHatCloseWidth = 3;
                public int ThinHatCloseHeight = 3;
                public int ThinHatEdgeSkipWidth = 5;
                public int ThinHatEdgeSkipHeight = 5;
                public bool ThinHatAreaEnabled = true;
                public int ThinHatAreaMin = 25;
                public bool ThinHatWidthEnabled = true;
                public int ThinHatWidthMin = 5;
                public bool ThinHatHeightEnabled = true;
                public int ThinHatHeightMin = 5;



                //平均值法
                public bool MeanEnabled = false;
                public int InspID = 0;
                public int Dark_Th = 20;
                public int Bright_Th = 20;
                public bool AreaEnabled = true;
                public int AreaMin = 25;
                public bool WidthEnabled = true;
                public int WidthMin = 5;
                public bool HeightEnabled = true;
                public int HeightMin = 5;
                public int EdgeSkipWidth = 1;
                public int EdgeSkipHeight = 1;
                public double MeanOpenRad = 3;
                public double MeanCloseRad = 20;

                //孔洞偵測
                public string HoleName = "孔洞";
                public bool TopHatEnabled = false;
                public int TopHatDark_Th = 20;
                public int TopHatBright_Th = 20;
                public bool TopHatAreaEnabled = true;
                public int TopHatAreaMin = 25;
                public bool TopHatWidthEnabled = true;
                public int TopHatWidthMin = 5;
                public bool TopHatHeightEnabled = true;
                public int TopHatHeightMin = 5;
                public int TopHatEdgeExpandWidth = 1;
                public int TopHatEdgeExpandHeight = 1;
                public int SESizeWidth = 50;
                public int SESizeHeight = 50;
                public int ExtH = 45;
                public int ExtThAdd = 5;
                public int BrightDetectTh = 200;
                
                //對比強化
                public bool HistoEqEnabled = false;
                public int HistoEqTh = 140;
                public int HistoEqOpeningWidth = 11;
                public int HistoEqOpeningHeight = 11;
                public int HistoEqClosingWidth = 11;
                public int HistoEqClosingHeight = 11;
                public bool HistoEqWidth = true;
                public int HistoEqWidthMin = 30;
                public bool HistoEqHeightEnabled = true;
                public int HistoEqHeightMin = 30;
                public bool HistoEqAreaEnabled = true;
                public int HistoEqAreaMin = 30;
                public int HistoEdgeSkipWidth = 1;
                public int HistoEdgeSkipHeight = 1;


                public bool DefectSumAreaEnabled = true;
                public int DefectSumArea = 50;
                public string Name = "薄化";
                public clsThinParam() { }
            }

            [Serializable]
            public class clsStainParam
            {
                public bool Enabled = true;
                public int InspID = 0;
                public int Dark_Th = 20;
                public int Bright_Th = 20;
                public bool AreaEnabled = true;
                public int AreaMin = 25;
                public bool WidthEnabled = true;
                public int WidthMin = 5;
                public bool HeightEnabled = true;
                public int HeightMin = 5;
                public string Name = "髒汙";
                public clsStainParam() { }
            }

            [Serializable]
            public class clsRDefectParam
            {
                public bool Enabled = true;
                public int InspID = 0;
                public int Dark_ThMin = 20;
                public int Dark_ThMax = 200;
                public int Bright_ThMin = 20;
                public int Bright_ThMax = 200;
                public bool AreaEnabled = true;
                public int AreaMin = 25;
                public bool WidthEnabled = true;
                public int WidthMin = 5;
                public bool HeightEnabled = true;
                public int HeightMin = 5;
                public int ExtWidth = 1;
                public int ExtHeight = 1;
                public int SkipWidth = 1;
                public int SkipHeight = 1;
                public string Name = "R層異常";
                public clsRDefectParam() { }
            }

            [Serializable]
            public class clsScratchParam
            {
                public bool Enabled = true;
                public int InspID = 0;
                public int In_Th = 20;
                public int Out_Th = 20;
                public bool AreaEnabled = true;
                public int AreaMin = 25;
                public bool WidthEnabled = true;
                public int WidthMin = 5;
                public bool HeightEnabled = true;
                public int HeightMin = 5;

                public bool OuterAreaEnabled = true;
                public int OuterAreaMin = 25;
                public bool OuterWidthEnabled = true;
                public int OuterWidthMin = 5;
                public bool OuterHeightEnabled = true;
                public int OuterHeightMin = 5;


                public string Name = "刮傷";
                public clsScratchParam() { }
            }
        }

        [Serializable]
        public class clsAOIParam
        {
            public string InspName = "DefectName";
            public enuBand Band = enuBand.R;
            //二值化
            public int LowThreshold = 0;
            public int HighThreshold = 255;
            //檢出規格
            public bool WEnabled = true;
            public int LowWidth = 10;

            public bool HEnabled = true;
            public int LowHeight = 10;

            public bool AEnabled = true;
            public int LowArea = 10;
            //形態學
            public int EdgeSkipSizeWidth = 5;
            public int EdgeSkipSizeHeight = 5;

            //其他
            public int ImageIndex = 0;
            public bool Enabled = true;

            public List<clsMultiTh> MultiTHList = new List<clsMultiTh>();

            public clsAOIParam() { }
        }

        [Serializable]
        public class clsAdvParam
        {
            public int NumLevels = 5;
            public int AngleStart = -3;
            public int AngleExtent = 7;
            public double AngleStep = 0.05;
            public bool AngleStepAuto = true;
            public int Optimization = 0;
            public int Metric = 3;

            public bool ContrastAuto = true;
            public int MinContrast = 10;
            public bool MinContrastAuto = true;

            public double Overlap = 0.0;
            public int SubPixel = 0;
            public double Greediness = 0.9;

            public double ScaleMin = 1.0;
            public double ScaleMax = 1.0;

            public int OpeningNum = 3;
            public int ClosingNum = 3;

            /// <summary>
            /// 填滿
            /// </summary>
            public bool bFillupEnabled { get; set; } = false;
            public enuFillupType FillupType = enuFillupType.area;
            public int FillupMin = 10;
            public int FillupMax = 999999;

            public int MinObjSize = 30;
            public int ContrastSmall = 30;
            public int ContrastLarge = 40;

            /// <summary>
            /// 匹配個數
            /// </summary>
            public int MatchNumber { get; set; } = 0;

            public double MatchSacleSize = 1.0;

            public clsAdvParam() { }
        }

        [Serializable]
        public class clsMethodRegion
        {
            public double HotspotX = 9999999;//檢測位置與Pattern中心相對距離X
            public double HotspotY = 9999999;//檢測位置與Pattern中心相對距離Y
            public double Row = 9999999;
            public double Column = 9999999;
            public string RegionPath { get; set; } = "";
            public string Color = "#0000ff40";
            public string Name = "";
            public HObject Region;
            public bool bShowRegion { get; set; } = false;
            public int ShowImageID = 0;

            public clsMethodRegion() { }
        }

        [Serializable]
        public class clsSegParam
        {
            public double MinScore = 0.6; // 樣板比對分數A

            public bool IsNCCMode { get; set; } = false;

            //Cell 區域
            public double HotspotX = 9999999;//檢測位置與Pattern中心相對距離X
            public double HotspotY = 9999999;//檢測位置與Pattern中心相對距離Y

            public double CellMatchHotspotX = 9999999;
            public double CellMatchHotspotY = 9999999;

            public int InspRectWidth = 0;//檢測大小寬
            public int InspRectHeight = 0;//檢測大小高

            public int CellRow1 = 0;
            public int CellRow2 = 0;
            public int CellColumn1 = 0;
            public int CellColumn2 = 0;

            public int Patternrow1 = 0;
            public int Patternrow2 = 0;
            public int Patterncolumn1 = 0;
            public int Patterncolumn2 = 0;

            //Mask
            public bool TeachMask = false;
            public int MaskThValueMax = 70;
            public int MaskThValueMin = 0;
            public int OpeningWidth = 5;
            public int OpeningHeight = 5;
            public int ClosingingWidth = 5;
            public int ClosingingHeight = 5;
            public bool MaskHistoeq = false;

            //Pre 
            public int BandIndex = 0; // R, G, B, Gray
            /// <summary>
            /// 對位時是否前處理
            /// </summary>
            public bool MatchPreProcessEnabled { get; set; } = false;

            public double GoldenCenterX = 9999999;//教導之Pattern Region中心X
            public double GoldenCenterY = 9999999;//教導之Pattern Region中心Y
            public double GoldenAngle = 0.0;

            //public double GoldenMatchCenterX = 9999999;//教導之Pattern中心X
            //public double GoldenMatchCenterY = 9999999;//教導之Pattern中心Y
            //public double GoldenMatchAngle = 0.0;

            /// <summary>
            /// 影像前處理 (圖像教導) 是否啟用
            /// </summary>
            public bool ThEnabled { get; set; } = true;
            public int ThMax = 255;
            public int THMin = 128;

            /// <summary>
            /// 切割時使用的影像來源
            /// </summary>
            public enu_ImageSource ImageSource_SegImg { get; set; } = enu_ImageSource.原始; // (20200130) Jeff Revised!
            /// <summary>
            /// 切割時使用的影像id
            /// </summary>
            public int SegImgIndex { get; set; } = 0;

            //薄化檢測區域
            public int ThinInspWidrh = 0;
            public int ThinInspHeight = 0;
            public double ThinHotspotX = 9999999;//檢測位置與Pattern中心相對距離X
            public double ThinHotspotY = 9999999;//檢測位置與Pattern中心相對距離Y

            //刮傷檢測區域
            public int ScratchInspWidrh = 0;
            public int ScratchInspHeight = 0;
            public double ScratchHotspotX = 9999999;//檢測位置與Pattern中心相對距離X
            public double ScratchHotspotY = 9999999;//檢測位置與Pattern中心相對距離Y

            //髒污檢測區域
            public int StainInspWidrh = 0;
            public int StainInspHeight = 0;
            public double StainHotspotX = 9999999;//檢測位置與Pattern中心相對距離X
            public double StainHotspotY = 9999999;//檢測位置與Pattern中心相對距離Y

            public int PartitionGrayValue = 100; // 區分亮區暗區灰階值

            public clsSegParam() { }
        }
    }

    public class InductanceInspRole : InspectRole
    {
        #region 暫存變數

        static InductanceInspRole m_Singleton;
        public string Path = ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\InductanceInsp\\";

        /// <summary>
        /// 對位範圍列表 儲存路徑
        /// </summary>
        public string MethodRegionPath { get; set; } = "";

        /// <summary>
        /// 編輯範圍列表 儲存路徑
        /// </summary>
        public string EditRegionPath { get; set; } = "";

        /// <summary>
        /// 影像演算法A 儲存路徑
        /// </summary>
        public string ImgAlgoA_Path { get; set; } = ""; // (20200130) Jeff Revised!

        /// <summary>
        /// 影像演算法B 儲存路徑
        /// </summary>
        public string ImgAlgoB_Path { get; set; } = ""; // (20200130) Jeff Revised!

        public HObject Pattern_Rect = null;
        public HObject InspRect = null;
        public HTuple ModelID = null;
        public HTuple ModelID_NCC = null;
        public clsDAVS DAVS = null;
        public clsDAVSParam DAVSParam = new clsDAVSParam();
        public HTuple TextureInspectionModel = null;
        public HTuple TextureInspectionModelDark = null;
        public clsDAVSInspMethod[] DAVSInspArray;

        #endregion

        #region 參數儲存區

        public clsRecipe Param = new clsRecipe();//總參數

        #endregion

        public static InductanceInspRole GetSingleton()
        {
            if (m_Singleton == null)
            {
                m_Singleton = new InductanceInspRole();
            }
            return m_Singleton;
        }

        public InductanceInspRole()
        {


        }

        public void Init(bool Enabled)
        {

        }

        public void Dispose()
        {
            if (DAVS != null)
            {
                DAVS.Dispose();
                DAVS = null;
            }
        }

        /// <summary>
        /// 儲存 Halcon 相關變數
        /// </summary>
        private void SaveHalconFile() // (20200130) Jeff Revised!
        {
            #region Save Model

            if (!Directory.Exists(this.Path))
            {
                Directory.CreateDirectory(this.Path);
            }

            if (ModelID != null)
            {
                if (ModelID.Length > 0)
                    HOperatorSet.WriteShapeModel(ModelID, Path + "ModelID");
            }

            if (ModelID_NCC != null)
            {
                if (ModelID_NCC.Length > 0)
                    HOperatorSet.WriteNccModel(ModelID_NCC, Path + "ModelID_NCC");
            }

            #endregion

            #region 編輯Region Tool 

            // 對位範圍列表
            if (!Directory.Exists(this.MethodRegionPath))
            {
                Directory.CreateDirectory(this.MethodRegionPath);
            }
            foreach (clsRecipe.clsMethodRegion RegionInfo in this.Param.MethodRegionList)
            {
                HOperatorSet.WriteRegion(RegionInfo.Region, this.MethodRegionPath + RegionInfo.Name + ".reg");
            }

            // 編輯範圍列表
            if (!Directory.Exists(this.EditRegionPath))
            {
                Directory.CreateDirectory(this.EditRegionPath);
            }
            foreach (clsRecipe.clsMethodRegion RegionInfo in this.Param.EditRegionList)
            {
                HOperatorSet.WriteRegion(RegionInfo.Region, this.EditRegionPath + RegionInfo.Name + ".reg");
            }

            #endregion

            #region 影像演算法 A & B // (20200130) Jeff Revised!

            // 影像演算法A
            if (!Directory.Exists(this.ImgAlgoA_Path))
            {
                Directory.CreateDirectory(this.ImgAlgoA_Path);
            }
            this.Param.AlgoImg.SaveHalconFile(this.ImgAlgoA_Path, false);

            // 影像演算法B
            if (!Directory.Exists(this.ImgAlgoB_Path))
            {
                Directory.CreateDirectory(this.ImgAlgoB_Path);
            }
            this.Param.AlgoImg_UsedRegion.SaveHalconFile(this.ImgAlgoB_Path, false);

            #endregion
        }

        /// <summary>
        /// 載入 Halcon 相關變數
        /// </summary>
        private void ReadHalconFile() // (20200130) Jeff Revised!
        {
            if (ModelID != null && ModelID.Length > 0)
            {
                HOperatorSet.ClearShapeModel(ModelID);
                ModelID = null;
            }
            if (File.Exists(Path + "ModelID"))
            {
                HOperatorSet.ReadShapeModel(Path + "ModelID", out ModelID);
            }

            if (ModelID_NCC != null && ModelID_NCC.Length > 0)
            {
                HOperatorSet.ClearNccModel(ModelID_NCC);
                ModelID_NCC = null;
            }
            if (File.Exists(Path + "ModelID_NCC"))
            {
                HOperatorSet.ReadNccModel(Path + "ModelID_NCC", out ModelID_NCC);
            }

            if (TextureInspectionModel != null && TextureInspectionModel.Length > 0)
            {
                HOperatorSet.ClearTextureInspectionModel(TextureInspectionModel);
                TextureInspectionModel = null;
            }
            if (File.Exists(Path + "TextureModel.htim"))
            {
                HOperatorSet.ReadTextureInspectionModel(Path + "TextureModel.htim", out TextureInspectionModel);
            }

            if (TextureInspectionModelDark != null && TextureInspectionModelDark.Length > 0)
            {
                HOperatorSet.ClearTextureInspectionModel(TextureInspectionModelDark);
                TextureInspectionModelDark = null;
            }
            if (File.Exists(Path + "TextureModelDark.htim"))
            {
                HOperatorSet.ReadTextureInspectionModel(Path + "TextureModelDark.htim", out TextureInspectionModelDark);
            }

            #region 編輯Region Tool

            // 對位範圍列表
            for (int i = 0; i < this.Param.MethodRegionList.Count; i++)
            {
                if (Param.MethodRegionList[i].Region != null)
                {
                    Param.MethodRegionList[i].Region.Dispose();
                    Param.MethodRegionList[i].Region = null;
                }
                //if (File.Exists(Param.MethodRegionList[i].RegionPath + ".reg"))
                //{
                //    HOperatorSet.ReadRegion(out this.Param.MethodRegionList[i].Region, this.Param.MethodRegionList[i].RegionPath + ".reg");
                //}
                if (File.Exists(this.MethodRegionPath + Param.MethodRegionList[i].Name + ".reg"))
                    HOperatorSet.ReadRegion(out this.Param.MethodRegionList[i].Region, this.MethodRegionPath + Param.MethodRegionList[i].Name + ".reg");
                else
                    HOperatorSet.GenEmptyObj(out Param.MethodRegionList[i].Region);
            }

            // 編輯範圍列表
            for (int i = 0; i < this.Param.EditRegionList.Count; i++)
            {
                //if (File.Exists(Param.EditRegionList[i].RegionPath + ".reg"))
                //{
                //    HOperatorSet.ReadRegion(out this.Param.EditRegionList[i].Region, this.Param.EditRegionList[i].RegionPath + ".reg");
                //}
                if (File.Exists(this.EditRegionPath + Param.EditRegionList[i].Name + ".reg"))
                    HOperatorSet.ReadRegion(out this.Param.EditRegionList[i].Region, this.EditRegionPath + Param.EditRegionList[i].Name + ".reg");
                else
                    HOperatorSet.GenEmptyObj(out Param.EditRegionList[i].Region);
            }

            // 使用範圍列表
            for (int i = 0; i < this.Param.UsedRegionList.Count; i++)
            {
                //if (File.Exists(Param.UsedRegionList[i].RegionPath + ".reg"))
                //{
                //    HOperatorSet.ReadRegion(out this.Param.UsedRegionList[i].Region, this.Param.UsedRegionList[i].RegionPath + ".reg");
                //}
                if (File.Exists(this.EditRegionPath + Param.UsedRegionList[i].Name + ".reg"))
                    HOperatorSet.ReadRegion(out this.Param.UsedRegionList[i].Region, this.EditRegionPath + Param.UsedRegionList[i].Name + ".reg");
                else
                    HOperatorSet.GenEmptyObj(out Param.UsedRegionList[i].Region);
            }

            #endregion

            #region 影像演算法 A & B // (20200130) Jeff Revised!
            
            // 影像演算法A
            this.Param.AlgoImg.ReadHalconFile(this.ImgAlgoA_Path, false);

            // 影像演算法B
            this.Param.AlgoImg_UsedRegion.ReadHalconFile(this.ImgAlgoB_Path, false);
            
            #endregion

        }

        public override object Clone()
        {
            InductanceInspRole obj = InductanceInspRole.GetSingleton();

            // Copy the content of original object and setup a new one
            obj.inspect_name_ = this.inspect_name_;
            obj.method_ = this.method_;

            return obj;
        }

        public string GetRecipePath()
        {
            return ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\InductanceInsp\\";
        }

        public static clsRecipe LoadXML(string PathFile)
        {
            clsRecipe Res = new clsRecipe();
            try
            {
                XmlSerializer XmlS = new XmlSerializer(Res.GetType());
                Stream S = File.Open(PathFile, FileMode.Open);
                Res = (clsRecipe)XmlS.Deserialize(S);
                S.Close();
            }
            catch (Exception e)
            { }

            return Res;
        }

        public static void SaveXML(clsRecipe SrcProduct, string PathFile)
        {
            clsRecipe Product = clsStaticTool.Clone<clsRecipe>(SrcProduct);
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(PathFile));

            try
            {
                XmlSerializer XmlS = new XmlSerializer(Product.GetType());
                Stream S = File.Open(PathFile, FileMode.Create);
                XmlS.Serialize(S, Product);
                S.Close();
            }
            catch (Exception e)
            {

            }
        }

        public override void save()
        {
            if (ModuleName == "") return; // 181031, andy

            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }

            XmlDocument doc = new XmlDocument();
            if (System.IO.File.Exists(ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\InductanceInsp\\InductanceInsp.xml"))
            {
                XmlElement element = doc.CreateElement("Parameter"); //181031, andy
                doc.AppendChild(element); //181031, andy
            }
            else
            {
                XmlElement element = doc.CreateElement("Parameter");
                doc.AppendChild(element);
            }
            XmlNode param = doc.SelectSingleNode("Parameter");
            XmlElement eleInspect = doc.CreateElement("Inspect");
            eleInspect.SetAttribute("Name", inspect_name_);
            param.AppendChild(eleInspect);


            #region 演算法參數儲存

            clsDAVSParam.SaveDAVSParam(DAVSParam, Path + "DAVS.xml");

            SaveHalconFile();

            //SaveXML(this.Param, ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\InductanceInsp\\InductanceInspParam.xml");
            clsStaticTool.SaveXML(this.Param, ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\InductanceInsp\\InductanceInspParam.xml"); // (20200130) Jeff Revised!

            #endregion


            if (!Directory.Exists(ModuleParamDirectory + RecipeFileDirectory + ModuleName))
            {
                Directory.CreateDirectory(ModuleParamDirectory + RecipeFileDirectory + ModuleName);
            }
            if (!Directory.Exists(ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\InductanceInsp"))
            {
                Directory.CreateDirectory(ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\InductanceInsp");
            }


            doc.Save(ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\InductanceInsp\\InductanceInsp.xml");
        }

        public override void load() // (20200130) Jeff Revised!
        {
            HOperatorSet.SetSystem("border_shape_models", "true");
            HOperatorSet.SetSystem("clip_region", "false");
            Path = ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\InductanceInsp\\";
            MethodRegionPath = Path + "MethodRegion\\";
            EditRegionPath = Path + "EditRegion\\";
            ImgAlgoA_Path = Path + "ImgAlgoA\\"; // (20200130) Jeff Revised!
            ImgAlgoB_Path = Path + "ImgAlgoB\\"; // (20200130) Jeff Revised!

            if (!System.IO.File.Exists(ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\InductanceInsp\\InductanceInsp.xml"))
            {
                if (!Directory.Exists(Path))
                {
                    Directory.CreateDirectory(Path);
                }
                DAVSParam = new clsDAVSParam();
                this.Param = new clsRecipe();
                string[] ArrayMethod = Enum.GetNames(typeof(enuMethod));
                if (ArrayMethod.Length != this.Param.MethodList.Count)
                {
                    int StartIndex = ArrayMethod.Length - this.Param.MethodList.Count;
                    for (int i = this.Param.MethodList.Count; i < ArrayMethod.Length; i++)
                    {
                        clsRecipe.clsMethod Method = new clsRecipe.clsMethod();
                        Method.Name = ArrayMethod[i].ToString();
                        this.Param.MethodList.Add(Method);
                    }
                }
                return;
            }

            XmlDocument doc = new XmlDocument();

            try
            {
                doc.Load(ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\InductanceInsp\\InductanceInsp.xml");
                string andyMagicStr = "";
                string index = "//Inspect[@Name='" + andyMagicStr + "']";

                XmlNode node = doc.SelectSingleNode(index);

                #region 演算法參數載入

                DAVSParam = clsDAVSParam.LoadDAVSParam(Path + "DAVS.xml"); // AI參數
                this.Param = LoadXML(ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\InductanceInsp\\InductanceInspParam.xml"); // 切割與檢測參數
                ReadHalconFile(); // Pattern 模型參數
                if (DAVS != null)
                {
                    DAVS.Dispose();
                    DAVS = null;
                }

                this.DAVS = new clsDAVS(DAVSParam, GetRecipePath());
                string[] ArrayMethod = Enum.GetNames(typeof(enuMethod));
                if (ArrayMethod.Length != this.Param.MethodList.Count)
                {
                    int StartIndex = ArrayMethod.Length - this.Param.MethodList.Count;
                    for (int i = this.Param.MethodList.Count; i < ArrayMethod.Length; i++)
                    {
                        clsRecipe.clsMethod Method = new clsRecipe.clsMethod();
                        Method.Name = ArrayMethod[i].ToString();
                        this.Param.MethodList.Add(Method);
                    }
                }

                UpdateDAVSArray(Param, ref DAVSInspArray);

                #endregion
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }
        }

        public static void UpdateDAVSArray(clsRecipe Param, ref clsDAVSInspMethod[] Array)
        {
            Array = null;
            Array = new clsDAVSInspMethod[Param.AlgorithmList.Count];
            for (int i = 0; i < Param.AlgorithmList.Count; i++)
            {
                if (Param.AlgorithmList[i].Type == InductanceInsp.enuAlgorithm.DAVSInsp)
                {
                    Array[i] = new clsDAVSInspMethod((clsRecipe.clsDAVSInsp)Param.AlgorithmList[i].Param, "",Param.bInspOutboundary);
                }
            }
        }
    }

}
