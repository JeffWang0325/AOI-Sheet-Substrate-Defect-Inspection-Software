namespace DeltaSubstrateInspector.src.Modules.NewInspectModule.InductanceInsp
{
    partial class InductanceInspUC
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InductanceInspUC));
            this.tabControl_Info = new System.Windows.Forms.TabControl();
            this.tabPage_Result = new System.Windows.Forms.TabPage();
            this.DisplayWindows = new HalconDotNet.HSmartWindowControl();
            this.labImageFileName = new System.Windows.Forms.Label();
            this.tpSetupParam = new System.Windows.Forms.TabPage();
            this.gbx_BatchTest = new System.Windows.Forms.GroupBox();
            this.gbx_BatchTest_Info = new System.Windows.Forms.GroupBox();
            this.txb_BatchTest_ID = new System.Windows.Forms.TextBox();
            this.label99 = new System.Windows.Forms.Label();
            this.txb_BatchTest_Count = new System.Windows.Forms.TextBox();
            this.label98 = new System.Windows.Forms.Label();
            this.listView_BatchTest_Info = new System.Windows.Forms.ListView();
            this.columnHeader_Index = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_FileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_OK = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_NG = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btn_BatchTest_LoadImg = new System.Windows.Forms.Button();
            this.gbx_BatchTest_Result = new System.Windows.Forms.GroupBox();
            this.listView_BatchTest_Result = new System.Windows.Forms.ListView();
            this.columnHeader_result = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_count = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txb_BatchTest_Path = new System.Windows.Forms.TextBox();
            this.label97 = new System.Windows.Forms.Label();
            this.btnBatchTest = new System.Windows.Forms.Button();
            this.gbxSynchronizePath = new System.Windows.Forms.GroupBox();
            this.cbxAutoUpdate = new System.Windows.Forms.CheckBox();
            this.btnSynchronize = new System.Windows.Forms.Button();
            this.txbSynchronizePath = new System.Windows.Forms.TextBox();
            this.label86 = new System.Windows.Forms.Label();
            this.button_Add = new System.Windows.Forms.Button();
            this.labVersionNumber = new System.Windows.Forms.Label();
            this.labInspVersion = new System.Windows.Forms.Label();
            this.gbxMethod = new System.Windows.Forms.GroupBox();
            this.btnSettingMethod = new System.Windows.Forms.Button();
            this.cbxListMethod = new System.Windows.Forms.CheckedListBox();
            this.gbxOtherParam = new System.Windows.Forms.GroupBox();
            this.cbxLogEnabled = new System.Windows.Forms.CheckBox();
            this.txbResolution = new System.Windows.Forms.TextBox();
            this.labumpixel = new System.Windows.Forms.Label();
            this.labResolution = new System.Windows.Forms.Label();
            this.labImgCount = new System.Windows.Forms.Label();
            this.nudImgCount = new System.Windows.Forms.NumericUpDown();
            this.gbxAIType = new System.Windows.Forms.GroupBox();
            this.combxAIType = new System.Windows.Forms.ComboBox();
            this.btnDAVSSetting = new System.Windows.Forms.Button();
            this.gbxSaveAOIImgSetting = new System.Windows.Forms.GroupBox();
            this.comboBoxDAVSBand3 = new System.Windows.Forms.ComboBox();
            this.label88 = new System.Windows.Forms.Label();
            this.comboBoxDAVSBand2 = new System.Windows.Forms.ComboBox();
            this.nudDAVSImageId = new System.Windows.Forms.NumericUpDown();
            this.comboBoxDAVSBand1 = new System.Windows.Forms.ComboBox();
            this.combxSaveImgType = new System.Windows.Forms.ComboBox();
            this.nudDAVSBand3ImageIndex = new System.Windows.Forms.NumericUpDown();
            this.cbxSaveImgEnable = new System.Windows.Forms.CheckBox();
            this.nudDAVSBand2ImageIndex = new System.Windows.Forms.NumericUpDown();
            this.cbxDAVSMixBandEnabled = new System.Windows.Forms.CheckBox();
            this.nudDAVSBand1ImageIndex = new System.Windows.Forms.NumericUpDown();
            this.gbxSkipParam = new System.Windows.Forms.GroupBox();
            this.combxTestModeType = new System.Windows.Forms.ComboBox();
            this.cbxTestModeEnabled = new System.Windows.Forms.CheckBox();
            this.comboBoxDisplayType = new System.Windows.Forms.ComboBox();
            this.gbxMatchingParam = new System.Windows.Forms.GroupBox();
            this.cbxRotateImage = new System.Windows.Forms.CheckBox();
            this.nudScaleSize = new System.Windows.Forms.NumericUpDown();
            this.cbxMatchThEnabled = new System.Windows.Forms.CheckBox();
            this.panMatching = new System.Windows.Forms.Panel();
            this.nudMatchNumber = new System.Windows.Forms.NumericUpDown();
            this.label82 = new System.Windows.Forms.Label();
            this.labGreediness = new System.Windows.Forms.Label();
            this.labOverlap = new System.Windows.Forms.Label();
            this.nudGreediness = new System.Windows.Forms.NumericUpDown();
            this.nudOverlap = new System.Windows.Forms.NumericUpDown();
            this.cbxSubPixel = new System.Windows.Forms.ComboBox();
            this.labSubPixel = new System.Windows.Forms.Label();
            this.cbxMinContrastAuto = new System.Windows.Forms.CheckBox();
            this.label66 = new System.Windows.Forms.Label();
            this.cbxContrastAuto = new System.Windows.Forms.CheckBox();
            this.cbxAngleStepAuto = new System.Windows.Forms.CheckBox();
            this.cbxMetric = new System.Windows.Forms.ComboBox();
            this.labMetric = new System.Windows.Forms.Label();
            this.cbxOptimization = new System.Windows.Forms.ComboBox();
            this.labOptimization = new System.Windows.Forms.Label();
            this.nudScaleMax = new System.Windows.Forms.NumericUpDown();
            this.nudScaleMin = new System.Windows.Forms.NumericUpDown();
            this.labScaleMax = new System.Windows.Forms.Label();
            this.labScaleMin = new System.Windows.Forms.Label();
            this.nudAngleStep = new System.Windows.Forms.NumericUpDown();
            this.labAngleStep = new System.Windows.Forms.Label();
            this.nudMinContrast = new System.Windows.Forms.NumericUpDown();
            this.labMinContrast = new System.Windows.Forms.Label();
            this.nudAngleExtent = new System.Windows.Forms.NumericUpDown();
            this.nudMinObjSize = new System.Windows.Forms.NumericUpDown();
            this.nudContrastHigh = new System.Windows.Forms.NumericUpDown();
            this.nudContrast = new System.Windows.Forms.NumericUpDown();
            this.labMInObj = new System.Windows.Forms.Label();
            this.labAngleExtent = new System.Windows.Forms.Label();
            this.labContrastLow = new System.Windows.Forms.Label();
            this.nudAngleStart = new System.Windows.Forms.NumericUpDown();
            this.labAngleStart = new System.Windows.Forms.Label();
            this.nudNumLevels = new System.Windows.Forms.NumericUpDown();
            this.labNumLevels = new System.Windows.Forms.Label();
            this.combxDisplayImg = new System.Windows.Forms.ComboBox();
            this.btnLoadImg = new System.Windows.Forms.Button();
            this.btnSaveImg = new System.Windows.Forms.Button();
            this.btnAddImsge = new System.Windows.Forms.Button();
            this.tbInspParamSetting = new System.Windows.Forms.TabControl();
            this.tpPatternMatch = new System.Windows.Forms.TabPage();
            this.gbxParam = new System.Windows.Forms.GroupBox();
            this.gbx_PatternMatch_PreProcess = new System.Windows.Forms.GroupBox();
            this.btnTest_PatternMatch_PreProcess = new System.Windows.Forms.Button();
            this.cbxFillup = new System.Windows.Forms.CheckBox();
            this.cbxThEnabled = new System.Windows.Forms.CheckBox();
            this.label92 = new System.Windows.Forms.Label();
            this.nudOpeningNum = new System.Windows.Forms.NumericUpDown();
            this.labMorphologyNum = new System.Windows.Forms.Label();
            this.btnThSetup = new System.Windows.Forms.Button();
            this.nudFillupMin = new System.Windows.Forms.NumericUpDown();
            this.comboBoxFillupType = new System.Windows.Forms.ComboBox();
            this.nudClosingNum = new System.Windows.Forms.NumericUpDown();
            this.nudFillupMax = new System.Windows.Forms.NumericUpDown();
            this.label90 = new System.Windows.Forms.Label();
            this.radioButton_OrigImg_Match = new System.Windows.Forms.RadioButton();
            this.radioButton_ImgA_Match = new System.Windows.Forms.RadioButton();
            this.nudInspectImgID = new System.Windows.Forms.NumericUpDown();
            this.labDefectCount = new System.Windows.Forms.Label();
            this.labMatchCount = new System.Windows.Forms.Label();
            this.cbxOutBondary = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cbxHistoEq = new System.Windows.Forms.CheckBox();
            this.label22 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.cbxMaskEnabled = new System.Windows.Forms.CheckBox();
            this.nudMatchCloseHeight = new System.Windows.Forms.NumericUpDown();
            this.nudMatchCloseWidth = new System.Windows.Forms.NumericUpDown();
            this.nudMatchOpenHeight = new System.Windows.Forms.NumericUpDown();
            this.nudMatchOpenWidth = new System.Windows.Forms.NumericUpDown();
            this.btnMaskTH = new System.Windows.Forms.Button();
            this.cbxNCCMode = new System.Windows.Forms.CheckBox();
            this.btnSegCellTest = new System.Windows.Forms.Button();
            this.labMatchID = new System.Windows.Forms.Label();
            this.combxBand = new System.Windows.Forms.ComboBox();
            this.btnATestMatch = new System.Windows.Forms.Button();
            this.labBand = new System.Windows.Forms.Label();
            this.cbxCellRegion = new System.Windows.Forms.CheckBox();
            this.nudAMinScore = new System.Windows.Forms.NumericUpDown();
            this.cbxASetPattern = new System.Windows.Forms.CheckBox();
            this.labAMinScore = new System.Windows.Forms.Label();
            this.btnATeachPattern = new System.Windows.Forms.Button();
            this.tpAlgoImage = new System.Windows.Forms.TabPage();
            this.gbx_AlgoImage = new System.Windows.Forms.GroupBox();
            this.cbx_ResultRegID_A = new System.Windows.Forms.ComboBox();
            this.label101 = new System.Windows.Forms.Label();
            this.button_SaveAsResultImgA = new System.Windows.Forms.Button();
            this.cbx_ResultImgID_A = new System.Windows.Forms.ComboBox();
            this.label96 = new System.Windows.Forms.Label();
            this.btn_Execute_Algo = new System.Windows.Forms.Button();
            this.btn_RemoveAll_Algo = new System.Windows.Forms.Button();
            this.btn_Remove_Algo = new System.Windows.Forms.Button();
            this.btn_Edit_Algo = new System.Windows.Forms.Button();
            this.btn_Add_Algo = new System.Windows.Forms.Button();
            this.listView_EditAlgo = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gbx_AlgoImage_UsedRegion = new System.Windows.Forms.GroupBox();
            this.cbx_ResultRegID_B = new System.Windows.Forms.ComboBox();
            this.label100 = new System.Windows.Forms.Label();
            this.button_SaveAsResultImgB = new System.Windows.Forms.Button();
            this.cbx_ResultImgID_B = new System.Windows.Forms.ComboBox();
            this.label95 = new System.Windows.Forms.Label();
            this.btn_Execute_Algo_UsedRegion = new System.Windows.Forms.Button();
            this.btn_RemoveAll_Algo_UsedRegion = new System.Windows.Forms.Button();
            this.btn_Remove_Algo_UsedRegion = new System.Windows.Forms.Button();
            this.btn_Edit_Algo_UsedRegion = new System.Windows.Forms.Button();
            this.btn_Add_Algo_UsedRegion = new System.Windows.Forms.Button();
            this.listView_EditAlgo_UsedRegion = new System.Windows.Forms.ListView();
            this.columnHeader_ID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_Name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_Algo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btn_Compute_UsedRegion = new System.Windows.Forms.Button();
            this.tpRgionMethod = new System.Windows.Forms.TabPage();
            this.gbxAdjust = new System.Windows.Forms.GroupBox();
            this.btnAdjSet = new System.Windows.Forms.Button();
            this.labAdjY = new System.Windows.Forms.Label();
            this.labAdjX = new System.Windows.Forms.Label();
            this.txbAdjY = new System.Windows.Forms.TextBox();
            this.txbAdjX = new System.Windows.Forms.TextBox();
            this.btnLeft = new System.Windows.Forms.Button();
            this.btnRight = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnAddUsed = new System.Windows.Forms.Button();
            this.gbxUsedRegion = new System.Windows.Forms.GroupBox();
            this.btnClearUsedRegion = new System.Windows.Forms.Button();
            this.btnExportRegion = new System.Windows.Forms.Button();
            this.btnRemoveUseRegion = new System.Windows.Forms.Button();
            this.listViewUsedRegion = new System.Windows.Forms.ListView();
            this.columnHeaderUseRegion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderUseDisplay = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderUseImageId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderUseColor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gbxEditRegionList = new System.Windows.Forms.GroupBox();
            this.btnEditRegionRemoveAll = new System.Windows.Forms.Button();
            this.btnOpenEditRegionForm = new System.Windows.Forms.Button();
            this.btnEditRegionRemove = new System.Windows.Forms.Button();
            this.listViewEditRegion = new System.Windows.Forms.ListView();
            this.columnHeaderEditRegion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gbxMethodList = new System.Windows.Forms.GroupBox();
            this.btnClearMethodRegion = new System.Windows.Forms.Button();
            this.btnRemoveMethodRegion = new System.Windows.Forms.Button();
            this.listViewMethod = new System.Windows.Forms.ListView();
            this.columnHeaderMethodRegion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gbxMtehodThreshold = new System.Windows.Forms.GroupBox();
            this.nudMethodThImageID = new System.Windows.Forms.NumericUpDown();
            this.radioButton_OrigImg_RegMethod = new System.Windows.Forms.RadioButton();
            this.radioButton_ImgA_RegMethod = new System.Windows.Forms.RadioButton();
            this.labMethodThImageID = new System.Windows.Forms.Label();
            this.labMethThBand = new System.Windows.Forms.Label();
            this.tbThMethod = new System.Windows.Forms.TabControl();
            this.tpSingleTh = new System.Windows.Forms.TabPage();
            this.label72 = new System.Windows.Forms.Label();
            this.btnMethodThAdd = new System.Windows.Forms.Button();
            this.label73 = new System.Windows.Forms.Label();
            this.btnMethodThSet = new System.Windows.Forms.Button();
            this.nudMethodThMin = new System.Windows.Forms.NumericUpDown();
            this.nudMethodThMax = new System.Windows.Forms.NumericUpDown();
            this.btnMethodThTest = new System.Windows.Forms.Button();
            this.tpDualTh = new System.Windows.Forms.TabPage();
            this.label77 = new System.Windows.Forms.Label();
            this.label76 = new System.Windows.Forms.Label();
            this.label75 = new System.Windows.Forms.Label();
            this.btnDualThAdd = new System.Windows.Forms.Button();
            this.nudDualThThreshold = new System.Windows.Forms.NumericUpDown();
            this.nudDualThMinSize = new System.Windows.Forms.NumericUpDown();
            this.nudDualThMinGray = new System.Windows.Forms.NumericUpDown();
            this.btnDualThTest = new System.Windows.Forms.Button();
            this.tpAutoTh = new System.Windows.Forms.TabPage();
            this.btnAutoThAdd = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.comboBoxAutoThSelect = new System.Windows.Forms.ComboBox();
            this.label79 = new System.Windows.Forms.Label();
            this.nudAutoThSigma = new System.Windows.Forms.NumericUpDown();
            this.label78 = new System.Windows.Forms.Label();
            this.tpDynTh = new System.Windows.Forms.TabPage();
            this.comboBoxDynType = new System.Windows.Forms.ComboBox();
            this.btnDynThAdd = new System.Windows.Forms.Button();
            this.labOffset = new System.Windows.Forms.Label();
            this.labDynMeanImage = new System.Windows.Forms.Label();
            this.nudMeanHeight = new System.Windows.Forms.NumericUpDown();
            this.nudDynOffset = new System.Windows.Forms.NumericUpDown();
            this.nudMeanWidth = new System.Windows.Forms.NumericUpDown();
            this.btnDynTest = new System.Windows.Forms.Button();
            this.tpUserSet = new System.Windows.Forms.TabPage();
            this.cbxCustomizeAdd = new System.Windows.Forms.CheckBox();
            this.label74 = new System.Windows.Forms.Label();
            this.combxCustomizeType = new System.Windows.Forms.ComboBox();
            this.combxMethodThBand = new System.Windows.Forms.ComboBox();
            this.labAnisometry = new System.Windows.Forms.TabPage();
            this.radioButton_ImgB_AOI = new System.Windows.Forms.RadioButton();
            this.radioButton_OrigImg_AOI = new System.Windows.Forms.RadioButton();
            this.radioButton_ImgA_AOI = new System.Windows.Forms.RadioButton();
            this.btnImportParam = new System.Windows.Forms.Button();
            this.nudPriority = new System.Windows.Forms.NumericUpDown();
            this.label84 = new System.Windows.Forms.Label();
            this.gbxRegionFeature = new System.Windows.Forms.GroupBox();
            this.gbxSurfaceFeature = new System.Windows.Forms.GroupBox();
            this.label94 = new System.Windows.Forms.Label();
            this.labanisometryL = new System.Windows.Forms.Label();
            this.labinner_radius = new System.Windows.Forms.Label();
            this.labholes_num = new System.Windows.Forms.Label();
            this.label89 = new System.Windows.Forms.Label();
            this.labroundness = new System.Windows.Forms.Label();
            this.label91 = new System.Windows.Forms.Label();
            this.label93 = new System.Windows.Forms.Label();
            this.label87 = new System.Windows.Forms.Label();
            this.label83 = new System.Windows.Forms.Label();
            this.labrectangularity = new System.Windows.Forms.Label();
            this.labcircularity = new System.Windows.Forms.Label();
            this.label85 = new System.Windows.Forms.Label();
            this.labconvexity = new System.Windows.Forms.Label();
            this.gbxInfo = new System.Windows.Forms.GroupBox();
            this.labImageIDDisplay = new System.Windows.Forms.Label();
            this.labID1 = new System.Windows.Forms.Label();
            this.labID0 = new System.Windows.Forms.Label();
            this.label81 = new System.Windows.Forms.Label();
            this.label80 = new System.Windows.Forms.Label();
            this.labSecGrayMax = new System.Windows.Forms.Label();
            this.labInfoGrayMax = new System.Windows.Forms.Label();
            this.labSecGrayMin = new System.Windows.Forms.Label();
            this.labInfoGrayMin = new System.Windows.Forms.Label();
            this.labSecGray = new System.Windows.Forms.Label();
            this.labGrayMean = new System.Windows.Forms.Label();
            this.labGray = new System.Windows.Forms.Label();
            this.labA = new System.Windows.Forms.Label();
            this.labH = new System.Windows.Forms.Label();
            this.labW = new System.Windows.Forms.Label();
            this.labArea = new System.Windows.Forms.Label();
            this.labHeight = new System.Windows.Forms.Label();
            this.labWidth = new System.Windows.Forms.Label();
            this.cbxTestROI = new System.Windows.Forms.CheckBox();
            this.gbxHoleGrayFeature = new System.Windows.Forms.GroupBox();
            this.labDarkRatioValue = new System.Windows.Forms.Label();
            this.labGrayMinValue = new System.Windows.Forms.Label();
            this.labMeanValue = new System.Windows.Forms.Label();
            this.labRadiusValue = new System.Windows.Forms.Label();
            this.labRatio = new System.Windows.Forms.Label();
            this.labGrayMin = new System.Windows.Forms.Label();
            this.labMeanGray = new System.Windows.Forms.Label();
            this.labRadius = new System.Windows.Forms.Label();
            this.btnMethTest = new System.Windows.Forms.Button();
            this.labInspImgIndex = new System.Windows.Forms.Label();
            this.nudInspImageIndex = new System.Windows.Forms.NumericUpDown();
            this.comboBoxRegionSelect = new System.Windows.Forms.ComboBox();
            this.labRegionIndex = new System.Windows.Forms.Label();
            this.txbAlgorithmName = new System.Windows.Forms.TextBox();
            this.btnAddAlgorithm = new System.Windows.Forms.Button();
            this.gbxAlgorithmList = new System.Windows.Forms.GroupBox();
            this.btnAlgorithmClear = new System.Windows.Forms.Button();
            this.btnAlgorithmSetup = new System.Windows.Forms.Button();
            this.btnUpdateDefectName = new System.Windows.Forms.Button();
            this.btnAlgorithmRemove = new System.Windows.Forms.Button();
            this.listViewAlgorithm = new System.Windows.Forms.ListView();
            this.columnHeaderAlgorithm = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderPriority = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderColor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.labAlgorithmSelect = new System.Windows.Forms.Label();
            this.comboBoxAlgorithmSelect = new System.Windows.Forms.ComboBox();
            this.Inner = new System.Windows.Forms.TabPage();
            this.btnTestEro = new System.Windows.Forms.Button();
            this.comboBoxInnerBand = new System.Windows.Forms.ComboBox();
            this.labInnerBand = new System.Windows.Forms.Label();
            this.txbInnerName = new System.Windows.Forms.TextBox();
            this.btnInnerInsp = new System.Windows.Forms.Button();
            this.gbxInnerParam = new System.Windows.Forms.GroupBox();
            this.btnInnerMultiTh = new System.Windows.Forms.Button();
            this.nudInnerLTh = new System.Windows.Forms.NumericUpDown();
            this.txbInnerImgIndex = new System.Windows.Forms.TextBox();
            this.labInnerEdgeSkipSize = new System.Windows.Forms.Label();
            this.labInnerHighTH = new System.Windows.Forms.Label();
            this.labInnerImgIndex = new System.Windows.Forms.Label();
            this.btnInnerThSetup = new System.Windows.Forms.Button();
            this.nudInnerEdgeSkipHeight = new System.Windows.Forms.NumericUpDown();
            this.nudInnerEdgeSkipSize = new System.Windows.Forms.NumericUpDown();
            this.nudInnerHTH = new System.Windows.Forms.NumericUpDown();
            this.labInnerLowTh = new System.Windows.Forms.Label();
            this.cbxInnerEnabled = new System.Windows.Forms.CheckBox();
            this.gbxinnerDefectSpec = new System.Windows.Forms.GroupBox();
            this.cbxInnerHeightEnabled = new System.Windows.Forms.CheckBox();
            this.cbxInnerWidthEnabled = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbxInnerAreaEnabled = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labInnerMinA = new System.Windows.Forms.Label();
            this.txbInnerMinA = new System.Windows.Forms.TextBox();
            this.labInnerMinH = new System.Windows.Forms.Label();
            this.txbInnerMinH = new System.Windows.Forms.TextBox();
            this.labInnerMinW = new System.Windows.Forms.Label();
            this.txbInnerMinW = new System.Windows.Forms.TextBox();
            this.Outer = new System.Windows.Forms.TabPage();
            this.btnDilTest = new System.Windows.Forms.Button();
            this.comboBoxOuterBand = new System.Windows.Forms.ComboBox();
            this.labOuterBand = new System.Windows.Forms.Label();
            this.txbOuterName = new System.Windows.Forms.TextBox();
            this.btnOuterInsp = new System.Windows.Forms.Button();
            this.gbxOuterParam = new System.Windows.Forms.GroupBox();
            this.btnOuterMultiTh = new System.Windows.Forms.Button();
            this.nudOuterLTh = new System.Windows.Forms.NumericUpDown();
            this.txbOuterImgIndex = new System.Windows.Forms.TextBox();
            this.labOuterEdgeSkipSize = new System.Windows.Forms.Label();
            this.labOuterHighTH = new System.Windows.Forms.Label();
            this.labOuterImgIndex = new System.Windows.Forms.Label();
            this.btnOuterThSetup = new System.Windows.Forms.Button();
            this.nudOuterEdgeSkipHeight = new System.Windows.Forms.NumericUpDown();
            this.nudOuterEdgeSkipSize = new System.Windows.Forms.NumericUpDown();
            this.nudOuterHTH = new System.Windows.Forms.NumericUpDown();
            this.labOuterLowTh = new System.Windows.Forms.Label();
            this.cbxOuterEnabled = new System.Windows.Forms.CheckBox();
            this.gbxOuterDefectSpec = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cbxOuterHEnabled = new System.Windows.Forms.CheckBox();
            this.cbxOuterWEnabled = new System.Windows.Forms.CheckBox();
            this.cbxOuterAreaEnabled = new System.Windows.Forms.CheckBox();
            this.labOuterMinH = new System.Windows.Forms.Label();
            this.labOuterMinA = new System.Windows.Forms.Label();
            this.txbOuterMinH = new System.Windows.Forms.TextBox();
            this.txbOuterMinA = new System.Windows.Forms.TextBox();
            this.labOuterMinW = new System.Windows.Forms.Label();
            this.txbOuterMinW = new System.Windows.Forms.TextBox();
            this.Thin = new System.Windows.Forms.TabPage();
            this.gbxThinScratch = new System.Windows.Forms.GroupBox();
            this.label55 = new System.Windows.Forms.Label();
            this.nudSensitivity = new System.Windows.Forms.NumericUpDown();
            this.cbxTrainROI = new System.Windows.Forms.CheckBox();
            this.comboBoxSelectRegion = new System.Windows.Forms.ComboBox();
            this.btnSelectObj = new System.Windows.Forms.Button();
            this.btnTrain = new System.Windows.Forms.Button();
            this.btnTextTest = new System.Windows.Forms.Button();
            this.btnAddImage = new System.Windows.Forms.Button();
            this.btnCreateNewMode = new System.Windows.Forms.Button();
            this.cbxThinScratchEnabled = new System.Windows.Forms.CheckBox();
            this.label61 = new System.Windows.Forms.Label();
            this.cbxThinScratchHeightEnabled = new System.Windows.Forms.CheckBox();
            this.txbThinScratchHeight = new System.Windows.Forms.TextBox();
            this.txbThinScratchWidth = new System.Windows.Forms.TextBox();
            this.cbxThinScratchWidthEnabled = new System.Windows.Forms.CheckBox();
            this.nudThinScratchCloseH = new System.Windows.Forms.NumericUpDown();
            this.nudThinScratchCloseW = new System.Windows.Forms.NumericUpDown();
            this.label64 = new System.Windows.Forms.Label();
            this.txbThinScratchArea = new System.Windows.Forms.TextBox();
            this.label56 = new System.Windows.Forms.Label();
            this.cbxThinScratchAreaEnabled = new System.Windows.Forms.CheckBox();
            this.nudThinScratchEdgeExHeight = new System.Windows.Forms.NumericUpDown();
            this.nudThinScratchEdgeSkipH = new System.Windows.Forms.NumericUpDown();
            this.nudThinScratchOpenH = new System.Windows.Forms.NumericUpDown();
            this.nudThinScratchOpenW = new System.Windows.Forms.NumericUpDown();
            this.label71 = new System.Windows.Forms.Label();
            this.nudThinScratchEdgeExWidth = new System.Windows.Forms.NumericUpDown();
            this.label54 = new System.Windows.Forms.Label();
            this.nudThinScratchEdgeSkipW = new System.Windows.Forms.NumericUpDown();
            this.label63 = new System.Windows.Forms.Label();
            this.label57 = new System.Windows.Forms.Label();
            this.label58 = new System.Windows.Forms.Label();
            this.label60 = new System.Windows.Forms.Label();
            this.label59 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbxThinHatEnabled = new System.Windows.Forms.CheckBox();
            this.cbxThinHatHeightEnabled = new System.Windows.Forms.CheckBox();
            this.txbThinHatHeightMin = new System.Windows.Forms.TextBox();
            this.nudThinHatDarkTh = new System.Windows.Forms.NumericUpDown();
            this.btnThinHatRegionTest = new System.Windows.Forms.Button();
            this.txbThinHatWidthMin = new System.Windows.Forms.TextBox();
            this.cbxThinHatWidthEnabled = new System.Windows.Forms.CheckBox();
            this.label37 = new System.Windows.Forms.Label();
            this.txbThinHatAreaMin = new System.Windows.Forms.TextBox();
            this.label40 = new System.Windows.Forms.Label();
            this.cbxThinHatAreaEnabled = new System.Windows.Forms.CheckBox();
            this.label41 = new System.Windows.Forms.Label();
            this.nudThinHatBrightTh = new System.Windows.Forms.NumericUpDown();
            this.label42 = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.label44 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.nudThinHatOpenWidth = new System.Windows.Forms.NumericUpDown();
            this.label45 = new System.Windows.Forms.Label();
            this.nudThinHatCloseHeight = new System.Windows.Forms.NumericUpDown();
            this.nudThinHatSkipH = new System.Windows.Forms.NumericUpDown();
            this.nudThinHatCloseWidth = new System.Windows.Forms.NumericUpDown();
            this.label46 = new System.Windows.Forms.Label();
            this.nudThinHatSkipW = new System.Windows.Forms.NumericUpDown();
            this.nudThinHatOpenHeight = new System.Windows.Forms.NumericUpDown();
            this.label48 = new System.Windows.Forms.Label();
            this.comboxModeSelect = new System.Windows.Forms.ComboBox();
            this.gbxHistoEq = new System.Windows.Forms.GroupBox();
            this.btnSettingHistoEqTh = new System.Windows.Forms.Button();
            this.cbxHistoEqEnabled = new System.Windows.Forms.CheckBox();
            this.label26 = new System.Windows.Forms.Label();
            this.cbxHistoHeightEnabled = new System.Windows.Forms.CheckBox();
            this.label23 = new System.Windows.Forms.Label();
            this.nudHistoEdgeSkipTest = new System.Windows.Forms.Button();
            this.txbHistoHeightMin = new System.Windows.Forms.TextBox();
            this.nudHistoEqTh = new System.Windows.Forms.NumericUpDown();
            this.txbHistoWidthMin = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.cbxHistoWidthEnabled = new System.Windows.Forms.CheckBox();
            this.label27 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.txbHistoAreaMin = new System.Windows.Forms.TextBox();
            this.cbxHistoAreaEnabled = new System.Windows.Forms.CheckBox();
            this.label29 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.nudHistoOpenWidth = new System.Windows.Forms.NumericUpDown();
            this.nudHistoCloseHeight = new System.Windows.Forms.NumericUpDown();
            this.nudHistoCloseWidth = new System.Windows.Forms.NumericUpDown();
            this.nudHistoEdgeSkipH = new System.Windows.Forms.NumericUpDown();
            this.nudHistoOpenHeight = new System.Windows.Forms.NumericUpDown();
            this.label34 = new System.Windows.Forms.Label();
            this.nudHistoEdgeSkipW = new System.Windows.Forms.NumericUpDown();
            this.btnSaveMode = new System.Windows.Forms.Button();
            this.txbThinName = new System.Windows.Forms.TextBox();
            this.cbxThinEnabled = new System.Windows.Forms.CheckBox();
            this.gbxThinParam = new System.Windows.Forms.GroupBox();
            this.gbxAvgTest = new System.Windows.Forms.GroupBox();
            this.btnTestAverage = new System.Windows.Forms.Button();
            this.labMin = new System.Windows.Forms.Label();
            this.labMid = new System.Windows.Forms.Label();
            this.labMax = new System.Windows.Forms.Label();
            this.gbxSumArea = new System.Windows.Forms.GroupBox();
            this.txbSumArea = new System.Windows.Forms.TextBox();
            this.labSumArea = new System.Windows.Forms.Label();
            this.cbxSumAreaEnabled = new System.Windows.Forms.CheckBox();
            this.label19 = new System.Windows.Forms.Label();
            this.labThinID = new System.Windows.Forms.Label();
            this.labPartGrayValue = new System.Windows.Forms.Label();
            this.txbPartGrayValue = new System.Windows.Forms.TextBox();
            this.txbThinID = new System.Windows.Forms.TextBox();
            this.btnThinPartitionTest = new System.Windows.Forms.Button();
            this.gbxSmallArea = new System.Windows.Forms.GroupBox();
            this.cbxTopHatEnabled = new System.Windows.Forms.CheckBox();
            this.cbxTophatHEnabled = new System.Windows.Forms.CheckBox();
            this.txbtophatH = new System.Windows.Forms.TextBox();
            this.nudTophatDarkTh = new System.Windows.Forms.NumericUpDown();
            this.txbHoleName = new System.Windows.Forms.TextBox();
            this.btnBrightRegionTh = new System.Windows.Forms.Button();
            this.btnTopHatEdgeExpTest = new System.Windows.Forms.Button();
            this.txbTophatW = new System.Windows.Forms.TextBox();
            this.cbxTophatWEnabled = new System.Windows.Forms.CheckBox();
            this.labThinTopHatum2 = new System.Windows.Forms.Label();
            this.btnHoleInsp = new System.Windows.Forms.Button();
            this.txbTophatMinArea = new System.Windows.Forms.TextBox();
            this.label36 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.labTophatDarkTh = new System.Windows.Forms.Label();
            this.cbxTophatAreaEnabled = new System.Windows.Forms.CheckBox();
            this.labThinTopHatum1 = new System.Windows.Forms.Label();
            this.nudTophatBrightTh = new System.Windows.Forms.NumericUpDown();
            this.labTophatum2 = new System.Windows.Forms.Label();
            this.labTopHatH = new System.Windows.Forms.Label();
            this.labTophatBrightTh = new System.Windows.Forms.Label();
            this.labTophatMinArea = new System.Windows.Forms.Label();
            this.nudSESizeHeight = new System.Windows.Forms.NumericUpDown();
            this.nudTopHatEdgeExpHeight = new System.Windows.Forms.NumericUpDown();
            this.labTopHatW = new System.Windows.Forms.Label();
            this.nudExtHTh = new System.Windows.Forms.NumericUpDown();
            this.nudSESizeWidth = new System.Windows.Forms.NumericUpDown();
            this.nudTopHatEdgeExpWidth = new System.Windows.Forms.NumericUpDown();
            this.labSESize = new System.Windows.Forms.Label();
            this.nudExtH = new System.Windows.Forms.NumericUpDown();
            this.labTopHatEdgeExp = new System.Windows.Forms.Label();
            this.btnThinInsp = new System.Windows.Forms.Button();
            this.gbxLargeArea = new System.Windows.Forms.GroupBox();
            this.cbxMeanEnabled = new System.Windows.Forms.CheckBox();
            this.cbxThinHEnabled = new System.Windows.Forms.CheckBox();
            this.txbThinInspH = new System.Windows.Forms.TextBox();
            this.txbThinInspW = new System.Windows.Forms.TextBox();
            this.btnThinRegionTest = new System.Windows.Forms.Button();
            this.cbxThinWEnabled = new System.Windows.Forms.CheckBox();
            this.labThinum2 = new System.Windows.Forms.Label();
            this.txbThinMinArea = new System.Windows.Forms.TextBox();
            this.cbxThinAreaEnabled = new System.Windows.Forms.CheckBox();
            this.labThinum1 = new System.Windows.Forms.Label();
            this.labDarkTH = new System.Windows.Forms.Label();
            this.labThinInspH = new System.Windows.Forms.Label();
            this.labThinum = new System.Windows.Forms.Label();
            this.labThinInspW = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.nudThinDarkTh = new System.Windows.Forms.NumericUpDown();
            this.nudMeanOpenRad = new System.Windows.Forms.NumericUpDown();
            this.labMinArea = new System.Windows.Forms.Label();
            this.nudThinEdgeSkipH = new System.Windows.Forms.NumericUpDown();
            this.nudMeanCloseRad = new System.Windows.Forms.NumericUpDown();
            this.nudThinEdgeSkipW = new System.Windows.Forms.NumericUpDown();
            this.nudThinBrightTh = new System.Windows.Forms.NumericUpDown();
            this.labThinByPass = new System.Windows.Forms.Label();
            this.labBrightTH = new System.Windows.Forms.Label();
            this.Scratch = new System.Windows.Forms.TabPage();
            this.btnScratchRegion = new System.Windows.Forms.Button();
            this.gbxScratchOuter = new System.Windows.Forms.GroupBox();
            this.labScratchOutTh = new System.Windows.Forms.Label();
            this.cbxScratchOuterHeightEnabled = new System.Windows.Forms.CheckBox();
            this.nudScratchOutTH = new System.Windows.Forms.NumericUpDown();
            this.btnScratchOutTh = new System.Windows.Forms.Button();
            this.txbScratchOuterHeightMin = new System.Windows.Forms.TextBox();
            this.label50 = new System.Windows.Forms.Label();
            this.label51 = new System.Windows.Forms.Label();
            this.txbScratchOuterWidthMin = new System.Windows.Forms.TextBox();
            this.label49 = new System.Windows.Forms.Label();
            this.label52 = new System.Windows.Forms.Label();
            this.cbxScratchOuterWidthEnabled = new System.Windows.Forms.CheckBox();
            this.label47 = new System.Windows.Forms.Label();
            this.cbxScratchOuterAreaEnabled = new System.Windows.Forms.CheckBox();
            this.label53 = new System.Windows.Forms.Label();
            this.txbScratchOuterAreaMin = new System.Windows.Forms.TextBox();
            this.txbScratchName = new System.Windows.Forms.TextBox();
            this.cbxScratchEnabled = new System.Windows.Forms.CheckBox();
            this.gbxScratch = new System.Windows.Forms.GroupBox();
            this.cbxScratchHeightEnabled = new System.Windows.Forms.CheckBox();
            this.txbScratchHeightMin = new System.Windows.Forms.TextBox();
            this.txbScratchWidthMin = new System.Windows.Forms.TextBox();
            this.cbxScratchWidthEnabled = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cbxScratchAreaEnabled = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txbScratchMinArea = new System.Windows.Forms.TextBox();
            this.labScratchum2 = new System.Windows.Forms.Label();
            this.labScratchMinArea = new System.Windows.Forms.Label();
            this.labScratchID = new System.Windows.Forms.Label();
            this.txbScratchID = new System.Windows.Forms.TextBox();
            this.labScratchInTH = new System.Windows.Forms.Label();
            this.btnScratchInTH = new System.Windows.Forms.Button();
            this.nudScratchInTH = new System.Windows.Forms.NumericUpDown();
            this.btnScratchInsp = new System.Windows.Forms.Button();
            this.Stain = new System.Windows.Forms.TabPage();
            this.txbStainName = new System.Windows.Forms.TextBox();
            this.cbxStainEnabled = new System.Windows.Forms.CheckBox();
            this.gbxStain = new System.Windows.Forms.GroupBox();
            this.cbxStainHeightEnabled = new System.Windows.Forms.CheckBox();
            this.txbStainHeightMin = new System.Windows.Forms.TextBox();
            this.txbStainWidthMin = new System.Windows.Forms.TextBox();
            this.cbxStainWidthEnabled = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cbxStainAreaEnabled = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.txbStainMinArea = new System.Windows.Forms.TextBox();
            this.labStainum2 = new System.Windows.Forms.Label();
            this.labStainMinArea = new System.Windows.Forms.Label();
            this.labStainID = new System.Windows.Forms.Label();
            this.labStainBrightTh = new System.Windows.Forms.Label();
            this.txbStainID = new System.Windows.Forms.TextBox();
            this.labStainDarkTh = new System.Windows.Forms.Label();
            this.btnStainBrightTh = new System.Windows.Forms.Button();
            this.btnStainDarkTh = new System.Windows.Forms.Button();
            this.nudStainBrightTh = new System.Windows.Forms.NumericUpDown();
            this.nudStainDarkTh = new System.Windows.Forms.NumericUpDown();
            this.btnStainPartitionTest = new System.Windows.Forms.Button();
            this.btnStainTest = new System.Windows.Forms.Button();
            this.RDefect = new System.Windows.Forms.TabPage();
            this.txbRShiftName = new System.Windows.Forms.TextBox();
            this.txbRDefectName = new System.Windows.Forms.TextBox();
            this.cbxRShiftEnabled = new System.Windows.Forms.CheckBox();
            this.cbxRDefectEnabled = new System.Windows.Forms.CheckBox();
            this.gbxRShift = new System.Windows.Forms.GroupBox();
            this.nudRShiftClosingSize = new System.Windows.Forms.NumericUpDown();
            this.btnTargetRTest = new System.Windows.Forms.Button();
            this.btnAutoThTest = new System.Windows.Forms.Button();
            this.btnTargetETest = new System.Windows.Forms.Button();
            this.nudTargetRWidth = new System.Windows.Forms.NumericUpDown();
            this.nudTargetEArea = new System.Windows.Forms.NumericUpDown();
            this.label68 = new System.Windows.Forms.Label();
            this.label67 = new System.Windows.Forms.Label();
            this.nudAutoTthSigma = new System.Windows.Forms.NumericUpDown();
            this.label62 = new System.Windows.Forms.Label();
            this.label65 = new System.Windows.Forms.Label();
            this.nudRShiftTh = new System.Windows.Forms.NumericUpDown();
            this.labRSiftTh = new System.Windows.Forms.Label();
            this.btnRShiftInsp = new System.Windows.Forms.Button();
            this.gbxRDefect = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.cbxRDefectHeightEnabled = new System.Windows.Forms.CheckBox();
            this.nudRDefectSkipHeight = new System.Windows.Forms.NumericUpDown();
            this.nudRDefectExtHeight = new System.Windows.Forms.NumericUpDown();
            this.txbRDefectHeightMin = new System.Windows.Forms.TextBox();
            this.nudRDefectSkipWidth = new System.Windows.Forms.NumericUpDown();
            this.nudRDefectExtWidth = new System.Windows.Forms.NumericUpDown();
            this.txbRDefectWidthMin = new System.Windows.Forms.TextBox();
            this.label69 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.cbxRDefectWidthEnabled = new System.Windows.Forms.CheckBox();
            this.label15 = new System.Windows.Forms.Label();
            this.cbxRDefectAreaEnabled = new System.Windows.Forms.CheckBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.labRDefectBS = new System.Windows.Forms.Label();
            this.labRDefectDS = new System.Windows.Forms.Label();
            this.txbRDefectMinArea = new System.Windows.Forms.TextBox();
            this.labRDefectum2 = new System.Windows.Forms.Label();
            this.labRDefectMinArea = new System.Windows.Forms.Label();
            this.labRDefectID = new System.Windows.Forms.Label();
            this.labRDefectBrightTH = new System.Windows.Forms.Label();
            this.txbRDefectID = new System.Windows.Forms.TextBox();
            this.labRDefectDarkTh = new System.Windows.Forms.Label();
            this.btnRDefectBrightTh = new System.Windows.Forms.Button();
            this.btnRDefectDarkTh = new System.Windows.Forms.Button();
            this.nudRDefectBrightMaxTh = new System.Windows.Forms.NumericUpDown();
            this.nudRDefectBrightMinTh = new System.Windows.Forms.NumericUpDown();
            this.nudRDefectDarkMaxTh = new System.Windows.Forms.NumericUpDown();
            this.nudRDefectDarkMinTh = new System.Windows.Forms.NumericUpDown();
            this.btnRDefectPartitionTest = new System.Windows.Forms.Button();
            this.btnRDefectTest = new System.Windows.Forms.Button();
            this.button_SaveParam = new System.Windows.Forms.Button();
            this.button_Inspection = new System.Windows.Forms.Button();
            this.button_LoadParam = new System.Windows.Forms.Button();
            this.btnLoadGolden = new System.Windows.Forms.Button();
            this.txbGrayValue = new System.Windows.Forms.TextBox();
            this.txt_ColorValue = new System.Windows.Forms.TextBox();
            this.txt_CursorCoordinate = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.labXY = new System.Windows.Forms.Label();
            this.labColorValue = new System.Windows.Forms.Label();
            this.panelImageControl = new System.Windows.Forms.Panel();
            this.panelInspectionControl = new System.Windows.Forms.Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.tabControl_Info.SuspendLayout();
            this.tabPage_Result.SuspendLayout();
            this.tpSetupParam.SuspendLayout();
            this.gbx_BatchTest.SuspendLayout();
            this.gbx_BatchTest_Info.SuspendLayout();
            this.gbx_BatchTest_Result.SuspendLayout();
            this.gbxSynchronizePath.SuspendLayout();
            this.gbxMethod.SuspendLayout();
            this.gbxOtherParam.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudImgCount)).BeginInit();
            this.gbxAIType.SuspendLayout();
            this.gbxSaveAOIImgSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDAVSImageId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDAVSBand3ImageIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDAVSBand2ImageIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDAVSBand1ImageIndex)).BeginInit();
            this.gbxSkipParam.SuspendLayout();
            this.gbxMatchingParam.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudScaleSize)).BeginInit();
            this.panMatching.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMatchNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGreediness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOverlap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudScaleMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudScaleMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAngleStep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinContrast)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAngleExtent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinObjSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudContrastHigh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudContrast)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAngleStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumLevels)).BeginInit();
            this.tbInspParamSetting.SuspendLayout();
            this.tpPatternMatch.SuspendLayout();
            this.gbxParam.SuspendLayout();
            this.gbx_PatternMatch_PreProcess.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudOpeningNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFillupMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudClosingNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFillupMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInspectImgID)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMatchCloseHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMatchCloseWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMatchOpenHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMatchOpenWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAMinScore)).BeginInit();
            this.tpAlgoImage.SuspendLayout();
            this.gbx_AlgoImage.SuspendLayout();
            this.gbx_AlgoImage_UsedRegion.SuspendLayout();
            this.tpRgionMethod.SuspendLayout();
            this.gbxAdjust.SuspendLayout();
            this.gbxUsedRegion.SuspendLayout();
            this.gbxEditRegionList.SuspendLayout();
            this.gbxMethodList.SuspendLayout();
            this.gbxMtehodThreshold.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMethodThImageID)).BeginInit();
            this.tbThMethod.SuspendLayout();
            this.tpSingleTh.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMethodThMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMethodThMax)).BeginInit();
            this.tpDualTh.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDualThThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDualThMinSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDualThMinGray)).BeginInit();
            this.tpAutoTh.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAutoThSigma)).BeginInit();
            this.tpDynTh.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMeanHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDynOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMeanWidth)).BeginInit();
            this.tpUserSet.SuspendLayout();
            this.labAnisometry.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPriority)).BeginInit();
            this.gbxRegionFeature.SuspendLayout();
            this.gbxSurfaceFeature.SuspendLayout();
            this.gbxInfo.SuspendLayout();
            this.gbxHoleGrayFeature.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudInspImageIndex)).BeginInit();
            this.gbxAlgorithmList.SuspendLayout();
            this.Inner.SuspendLayout();
            this.gbxInnerParam.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudInnerLTh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInnerEdgeSkipHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInnerEdgeSkipSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInnerHTH)).BeginInit();
            this.gbxinnerDefectSpec.SuspendLayout();
            this.Outer.SuspendLayout();
            this.gbxOuterParam.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudOuterLTh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOuterEdgeSkipHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOuterEdgeSkipSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOuterHTH)).BeginInit();
            this.gbxOuterDefectSpec.SuspendLayout();
            this.Thin.SuspendLayout();
            this.gbxThinScratch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSensitivity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThinScratchCloseH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThinScratchCloseW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThinScratchEdgeExHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThinScratchEdgeSkipH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThinScratchOpenH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThinScratchOpenW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThinScratchEdgeExWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThinScratchEdgeSkipW)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudThinHatDarkTh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThinHatBrightTh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThinHatOpenWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThinHatCloseHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThinHatSkipH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThinHatCloseWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThinHatSkipW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThinHatOpenHeight)).BeginInit();
            this.gbxHistoEq.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudHistoEqTh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHistoOpenWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHistoCloseHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHistoCloseWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHistoEdgeSkipH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHistoOpenHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHistoEdgeSkipW)).BeginInit();
            this.gbxThinParam.SuspendLayout();
            this.gbxAvgTest.SuspendLayout();
            this.gbxSumArea.SuspendLayout();
            this.gbxSmallArea.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTophatDarkTh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTophatBrightTh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSESizeHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTopHatEdgeExpHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudExtHTh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSESizeWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTopHatEdgeExpWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudExtH)).BeginInit();
            this.gbxLargeArea.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudThinDarkTh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMeanOpenRad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThinEdgeSkipH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMeanCloseRad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThinEdgeSkipW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThinBrightTh)).BeginInit();
            this.Scratch.SuspendLayout();
            this.gbxScratchOuter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudScratchOutTH)).BeginInit();
            this.gbxScratch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudScratchInTH)).BeginInit();
            this.Stain.SuspendLayout();
            this.gbxStain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudStainBrightTh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStainDarkTh)).BeginInit();
            this.RDefect.SuspendLayout();
            this.gbxRShift.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRShiftClosingSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTargetRWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTargetEArea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAutoTthSigma)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRShiftTh)).BeginInit();
            this.gbxRDefect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRDefectSkipHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRDefectExtHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRDefectSkipWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRDefectExtWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRDefectBrightMaxTh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRDefectBrightMinTh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRDefectDarkMaxTh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRDefectDarkMinTh)).BeginInit();
            this.panelImageControl.SuspendLayout();
            this.panelInspectionControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl_Info
            // 
            this.tabControl_Info.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl_Info.Controls.Add(this.tabPage_Result);
            this.tabControl_Info.Controls.Add(this.tpSetupParam);
            this.tabControl_Info.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.tabControl_Info.Location = new System.Drawing.Point(2, 58);
            this.tabControl_Info.Name = "tabControl_Info";
            this.tabControl_Info.SelectedIndex = 0;
            this.tabControl_Info.Size = new System.Drawing.Size(1148, 791);
            this.tabControl_Info.TabIndex = 73;
            // 
            // tabPage_Result
            // 
            this.tabPage_Result.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabPage_Result.Controls.Add(this.DisplayWindows);
            this.tabPage_Result.Controls.Add(this.labImageFileName);
            this.tabPage_Result.Location = new System.Drawing.Point(4, 29);
            this.tabPage_Result.Name = "tabPage_Result";
            this.tabPage_Result.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Result.Size = new System.Drawing.Size(1140, 758);
            this.tabPage_Result.TabIndex = 0;
            this.tabPage_Result.Text = "測試影像";
            this.tabPage_Result.UseVisualStyleBackColor = true;
            // 
            // DisplayWindows
            // 
            this.DisplayWindows.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.DisplayWindows.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.DisplayWindows.HDoubleClickToFitContent = true;
            this.DisplayWindows.HDrawingObjectsModifier = HalconDotNet.HSmartWindowControl.DrawingObjectsModifier.None;
            this.DisplayWindows.HImagePart = new System.Drawing.Rectangle(0, 0, 4096, 3000);
            this.DisplayWindows.HKeepAspectRatio = false;
            this.DisplayWindows.HMoveContent = true;
            this.DisplayWindows.HZoomContent = HalconDotNet.HSmartWindowControl.ZoomContent.WheelBackwardZoomsIn;
            this.DisplayWindows.ImeMode = System.Windows.Forms.ImeMode.On;
            this.DisplayWindows.Location = new System.Drawing.Point(3, 3);
            this.DisplayWindows.Margin = new System.Windows.Forms.Padding(0);
            this.DisplayWindows.Name = "DisplayWindows";
            this.DisplayWindows.Size = new System.Drawing.Size(1132, 750);
            this.DisplayWindows.TabIndex = 69;
            this.DisplayWindows.WindowSize = new System.Drawing.Size(1132, 750);
            this.DisplayWindows.HMouseMove += new HalconDotNet.HMouseEventHandler(this.DisplayWindows_HMouseMove);
            this.DisplayWindows.HMouseDown += new HalconDotNet.HMouseEventHandler(this.DisplayWindows_HMouseDown);
            // 
            // labImageFileName
            // 
            this.labImageFileName.AutoSize = true;
            this.labImageFileName.Location = new System.Drawing.Point(6, 23);
            this.labImageFileName.Name = "labImageFileName";
            this.labImageFileName.Size = new System.Drawing.Size(114, 18);
            this.labImageFileName.TabIndex = 103;
            this.labImageFileName.Text = "Load File Name";
            // 
            // tpSetupParam
            // 
            this.tpSetupParam.AutoScroll = true;
            this.tpSetupParam.BackColor = System.Drawing.SystemColors.Control;
            this.tpSetupParam.Controls.Add(this.gbx_BatchTest);
            this.tpSetupParam.Controls.Add(this.gbxSynchronizePath);
            this.tpSetupParam.Controls.Add(this.button_Add);
            this.tpSetupParam.Controls.Add(this.labVersionNumber);
            this.tpSetupParam.Controls.Add(this.labInspVersion);
            this.tpSetupParam.Controls.Add(this.gbxMethod);
            this.tpSetupParam.Controls.Add(this.gbxOtherParam);
            this.tpSetupParam.Controls.Add(this.gbxAIType);
            this.tpSetupParam.Controls.Add(this.gbxSaveAOIImgSetting);
            this.tpSetupParam.Controls.Add(this.gbxSkipParam);
            this.tpSetupParam.Location = new System.Drawing.Point(4, 29);
            this.tpSetupParam.Name = "tpSetupParam";
            this.tpSetupParam.Size = new System.Drawing.Size(1140, 758);
            this.tpSetupParam.TabIndex = 1;
            this.tpSetupParam.Text = "進階設定";
            // 
            // gbx_BatchTest
            // 
            this.gbx_BatchTest.Controls.Add(this.gbx_BatchTest_Info);
            this.gbx_BatchTest.Controls.Add(this.gbx_BatchTest_Result);
            this.gbx_BatchTest.Controls.Add(this.txb_BatchTest_Path);
            this.gbx_BatchTest.Controls.Add(this.label97);
            this.gbx_BatchTest.Controls.Add(this.btnBatchTest);
            this.gbx_BatchTest.Location = new System.Drawing.Point(193, 182);
            this.gbx_BatchTest.Name = "gbx_BatchTest";
            this.gbx_BatchTest.Size = new System.Drawing.Size(350, 573);
            this.gbx_BatchTest.TabIndex = 124;
            this.gbx_BatchTest.TabStop = false;
            this.gbx_BatchTest.Text = "批量測試";
            // 
            // gbx_BatchTest_Info
            // 
            this.gbx_BatchTest_Info.Controls.Add(this.txb_BatchTest_ID);
            this.gbx_BatchTest_Info.Controls.Add(this.label99);
            this.gbx_BatchTest_Info.Controls.Add(this.txb_BatchTest_Count);
            this.gbx_BatchTest_Info.Controls.Add(this.label98);
            this.gbx_BatchTest_Info.Controls.Add(this.listView_BatchTest_Info);
            this.gbx_BatchTest_Info.Controls.Add(this.btn_BatchTest_LoadImg);
            this.gbx_BatchTest_Info.Location = new System.Drawing.Point(6, 150);
            this.gbx_BatchTest_Info.Name = "gbx_BatchTest_Info";
            this.gbx_BatchTest_Info.Size = new System.Drawing.Size(338, 415);
            this.gbx_BatchTest_Info.TabIndex = 218;
            this.gbx_BatchTest_Info.TabStop = false;
            this.gbx_BatchTest_Info.Text = "批量測試詳細資訊";
            // 
            // txb_BatchTest_ID
            // 
            this.txb_BatchTest_ID.Cursor = System.Windows.Forms.Cursors.Hand;
            this.txb_BatchTest_ID.ForeColor = System.Drawing.Color.DarkViolet;
            this.txb_BatchTest_ID.Location = new System.Drawing.Point(193, 18);
            this.txb_BatchTest_ID.Name = "txb_BatchTest_ID";
            this.txb_BatchTest_ID.ReadOnly = true;
            this.txb_BatchTest_ID.Size = new System.Drawing.Size(60, 25);
            this.txb_BatchTest_ID.TabIndex = 220;
            this.txb_BatchTest_ID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label99
            // 
            this.label99.AutoSize = true;
            this.label99.Location = new System.Drawing.Point(154, 21);
            this.label99.Name = "label99";
            this.label99.Size = new System.Drawing.Size(40, 18);
            this.label99.TabIndex = 219;
            this.label99.Text = "編號:";
            // 
            // txb_BatchTest_Count
            // 
            this.txb_BatchTest_Count.Cursor = System.Windows.Forms.Cursors.Hand;
            this.txb_BatchTest_Count.Location = new System.Drawing.Point(45, 18);
            this.txb_BatchTest_Count.Name = "txb_BatchTest_Count";
            this.txb_BatchTest_Count.ReadOnly = true;
            this.txb_BatchTest_Count.Size = new System.Drawing.Size(60, 25);
            this.txb_BatchTest_Count.TabIndex = 218;
            this.txb_BatchTest_Count.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label98
            // 
            this.label98.AutoSize = true;
            this.label98.Location = new System.Drawing.Point(6, 21);
            this.label98.Name = "label98";
            this.label98.Size = new System.Drawing.Size(40, 18);
            this.label98.TabIndex = 217;
            this.label98.Text = "總數:";
            // 
            // listView_BatchTest_Info
            // 
            this.listView_BatchTest_Info.BackColor = System.Drawing.Color.AliceBlue;
            this.listView_BatchTest_Info.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listView_BatchTest_Info.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader_Index,
            this.columnHeader_FileName,
            this.columnHeader_OK,
            this.columnHeader_NG});
            this.listView_BatchTest_Info.FullRowSelect = true;
            this.listView_BatchTest_Info.GridLines = true;
            this.listView_BatchTest_Info.Location = new System.Drawing.Point(5, 48);
            this.listView_BatchTest_Info.MultiSelect = false;
            this.listView_BatchTest_Info.Name = "listView_BatchTest_Info";
            this.listView_BatchTest_Info.Size = new System.Drawing.Size(326, 361);
            this.listView_BatchTest_Info.TabIndex = 216;
            this.listView_BatchTest_Info.UseCompatibleStateImageBehavior = false;
            this.listView_BatchTest_Info.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader_Index
            // 
            this.columnHeader_Index.Text = "編號";
            this.columnHeader_Index.Width = 39;
            // 
            // columnHeader_FileName
            // 
            this.columnHeader_FileName.Text = "檔名";
            this.columnHeader_FileName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader_FileName.Width = 195;
            // 
            // columnHeader_OK
            // 
            this.columnHeader_OK.Text = "OK";
            this.columnHeader_OK.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader_OK.Width = 40;
            // 
            // columnHeader_NG
            // 
            this.columnHeader_NG.Text = "NG";
            this.columnHeader_NG.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader_NG.Width = 40;
            // 
            // btn_BatchTest_LoadImg
            // 
            this.btn_BatchTest_LoadImg.BackColor = System.Drawing.Color.YellowGreen;
            this.btn_BatchTest_LoadImg.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_BatchTest_LoadImg.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_BatchTest_LoadImg.Location = new System.Drawing.Point(257, 14);
            this.btn_BatchTest_LoadImg.Name = "btn_BatchTest_LoadImg";
            this.btn_BatchTest_LoadImg.Size = new System.Drawing.Size(74, 30);
            this.btn_BatchTest_LoadImg.TabIndex = 131;
            this.btn_BatchTest_LoadImg.Text = "載入影像";
            this.btn_BatchTest_LoadImg.UseVisualStyleBackColor = false;
            this.btn_BatchTest_LoadImg.Click += new System.EventHandler(this.btn_BatchTest_LoadImg_Click);
            // 
            // gbx_BatchTest_Result
            // 
            this.gbx_BatchTest_Result.Controls.Add(this.listView_BatchTest_Result);
            this.gbx_BatchTest_Result.Location = new System.Drawing.Point(140, 52);
            this.gbx_BatchTest_Result.Name = "gbx_BatchTest_Result";
            this.gbx_BatchTest_Result.Size = new System.Drawing.Size(158, 98);
            this.gbx_BatchTest_Result.TabIndex = 217;
            this.gbx_BatchTest_Result.TabStop = false;
            this.gbx_BatchTest_Result.Text = "批量測試統計結果";
            // 
            // listView_BatchTest_Result
            // 
            this.listView_BatchTest_Result.BackColor = System.Drawing.Color.AliceBlue;
            this.listView_BatchTest_Result.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listView_BatchTest_Result.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader_result,
            this.columnHeader_count});
            this.listView_BatchTest_Result.FullRowSelect = true;
            this.listView_BatchTest_Result.GridLines = true;
            this.listView_BatchTest_Result.Location = new System.Drawing.Point(11, 20);
            this.listView_BatchTest_Result.MultiSelect = false;
            this.listView_BatchTest_Result.Name = "listView_BatchTest_Result";
            this.listView_BatchTest_Result.Size = new System.Drawing.Size(135, 73);
            this.listView_BatchTest_Result.TabIndex = 219;
            this.listView_BatchTest_Result.UseCompatibleStateImageBehavior = false;
            this.listView_BatchTest_Result.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader_result
            // 
            this.columnHeader_result.Text = "檢測結果";
            this.columnHeader_result.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader_result.Width = 70;
            // 
            // columnHeader_count
            // 
            this.columnHeader_count.Text = "數量";
            this.columnHeader_count.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader_count.Width = 62;
            // 
            // txb_BatchTest_Path
            // 
            this.txb_BatchTest_Path.Cursor = System.Windows.Forms.Cursors.Hand;
            this.txb_BatchTest_Path.Location = new System.Drawing.Point(42, 22);
            this.txb_BatchTest_Path.Name = "txb_BatchTest_Path";
            this.txb_BatchTest_Path.ReadOnly = true;
            this.txb_BatchTest_Path.Size = new System.Drawing.Size(300, 25);
            this.txb_BatchTest_Path.TabIndex = 129;
            this.txb_BatchTest_Path.Click += new System.EventHandler(this.txb_BatchTest_Path_Click);
            // 
            // label97
            // 
            this.label97.AutoSize = true;
            this.label97.Location = new System.Drawing.Point(3, 25);
            this.label97.Name = "label97";
            this.label97.Size = new System.Drawing.Size(40, 18);
            this.label97.TabIndex = 130;
            this.label97.Text = "路徑:";
            // 
            // btnBatchTest
            // 
            this.btnBatchTest.BackColor = System.Drawing.Color.Lime;
            this.btnBatchTest.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBatchTest.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnBatchTest.Location = new System.Drawing.Point(6, 52);
            this.btnBatchTest.Name = "btnBatchTest";
            this.btnBatchTest.Size = new System.Drawing.Size(74, 30);
            this.btnBatchTest.TabIndex = 123;
            this.btnBatchTest.Text = "批量測試";
            this.btnBatchTest.UseVisualStyleBackColor = false;
            this.btnBatchTest.Click += new System.EventHandler(this.btnBatchTest_Click);
            // 
            // gbxSynchronizePath
            // 
            this.gbxSynchronizePath.Controls.Add(this.cbxAutoUpdate);
            this.gbxSynchronizePath.Controls.Add(this.btnSynchronize);
            this.gbxSynchronizePath.Controls.Add(this.txbSynchronizePath);
            this.gbxSynchronizePath.Controls.Add(this.label86);
            this.gbxSynchronizePath.Location = new System.Drawing.Point(422, 51);
            this.gbxSynchronizePath.Name = "gbxSynchronizePath";
            this.gbxSynchronizePath.Size = new System.Drawing.Size(423, 125);
            this.gbxSynchronizePath.TabIndex = 123;
            this.gbxSynchronizePath.TabStop = false;
            this.gbxSynchronizePath.Text = "同步更新";
            // 
            // cbxAutoUpdate
            // 
            this.cbxAutoUpdate.AutoSize = true;
            this.cbxAutoUpdate.Location = new System.Drawing.Point(8, 34);
            this.cbxAutoUpdate.Name = "cbxAutoUpdate";
            this.cbxAutoUpdate.Size = new System.Drawing.Size(83, 22);
            this.cbxAutoUpdate.TabIndex = 3;
            this.cbxAutoUpdate.Text = "自動同步";
            this.cbxAutoUpdate.UseVisualStyleBackColor = true;
            // 
            // btnSynchronize
            // 
            this.btnSynchronize.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSynchronize.Location = new System.Drawing.Point(339, 69);
            this.btnSynchronize.Name = "btnSynchronize";
            this.btnSynchronize.Size = new System.Drawing.Size(75, 25);
            this.btnSynchronize.TabIndex = 2;
            this.btnSynchronize.Text = "更新";
            this.btnSynchronize.UseVisualStyleBackColor = true;
            this.btnSynchronize.Click += new System.EventHandler(this.btnSynchronize_Click);
            // 
            // txbSynchronizePath
            // 
            this.txbSynchronizePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txbSynchronizePath.Cursor = System.Windows.Forms.Cursors.Hand;
            this.txbSynchronizePath.Location = new System.Drawing.Point(51, 69);
            this.txbSynchronizePath.Name = "txbSynchronizePath";
            this.txbSynchronizePath.ReadOnly = true;
            this.txbSynchronizePath.Size = new System.Drawing.Size(282, 25);
            this.txbSynchronizePath.TabIndex = 1;
            this.txbSynchronizePath.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txbSynchronizePath_MouseClick);
            // 
            // label86
            // 
            this.label86.AutoSize = true;
            this.label86.Location = new System.Drawing.Point(5, 72);
            this.label86.Name = "label86";
            this.label86.Size = new System.Drawing.Size(40, 18);
            this.label86.TabIndex = 0;
            this.label86.Text = "路徑:";
            // 
            // button_Add
            // 
            this.button_Add.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(166)))), ((int)(((byte)(137)))));
            this.button_Add.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.button_Add.Location = new System.Drawing.Point(204, 13);
            this.button_Add.Name = "button_Add";
            this.button_Add.Size = new System.Drawing.Size(114, 28);
            this.button_Add.TabIndex = 110;
            this.button_Add.Text = "新增";
            this.button_Add.UseVisualStyleBackColor = false;
            this.button_Add.Click += new System.EventHandler(this.button_Add_Click_1);
            // 
            // labVersionNumber
            // 
            this.labVersionNumber.AutoSize = true;
            this.labVersionNumber.Location = new System.Drawing.Point(99, 19);
            this.labVersionNumber.Name = "labVersionNumber";
            this.labVersionNumber.Size = new System.Drawing.Size(28, 18);
            this.labVersionNumber.TabIndex = 121;
            this.labVersionNumber.Text = "4.0";
            // 
            // labInspVersion
            // 
            this.labInspVersion.AutoSize = true;
            this.labInspVersion.Location = new System.Drawing.Point(13, 18);
            this.labInspVersion.Name = "labInspVersion";
            this.labInspVersion.Size = new System.Drawing.Size(90, 18);
            this.labInspVersion.TabIndex = 120;
            this.labInspVersion.Text = "演算法版本 : ";
            // 
            // gbxMethod
            // 
            this.gbxMethod.Controls.Add(this.btnSettingMethod);
            this.gbxMethod.Controls.Add(this.cbxListMethod);
            this.gbxMethod.Location = new System.Drawing.Point(885, 17);
            this.gbxMethod.Name = "gbxMethod";
            this.gbxMethod.Size = new System.Drawing.Size(223, 236);
            this.gbxMethod.TabIndex = 120;
            this.gbxMethod.TabStop = false;
            this.gbxMethod.Text = "使用檢測";
            this.gbxMethod.Visible = false;
            // 
            // btnSettingMethod
            // 
            this.btnSettingMethod.Location = new System.Drawing.Point(9, 199);
            this.btnSettingMethod.Name = "btnSettingMethod";
            this.btnSettingMethod.Size = new System.Drawing.Size(198, 31);
            this.btnSettingMethod.TabIndex = 1;
            this.btnSettingMethod.Text = "確認";
            this.btnSettingMethod.UseVisualStyleBackColor = true;
            this.btnSettingMethod.Click += new System.EventHandler(this.btnSettingMethod_Click);
            // 
            // cbxListMethod
            // 
            this.cbxListMethod.BackColor = System.Drawing.SystemColors.Control;
            this.cbxListMethod.CheckOnClick = true;
            this.cbxListMethod.FormattingEnabled = true;
            this.cbxListMethod.Items.AddRange(new object[] {
            "Method Name1",
            "Method Name2",
            "Method Name3",
            "Method Name4",
            ".",
            ".",
            ".",
            "."});
            this.cbxListMethod.Location = new System.Drawing.Point(9, 27);
            this.cbxListMethod.Name = "cbxListMethod";
            this.cbxListMethod.Size = new System.Drawing.Size(198, 164);
            this.cbxListMethod.TabIndex = 0;
            // 
            // gbxOtherParam
            // 
            this.gbxOtherParam.Controls.Add(this.cbxLogEnabled);
            this.gbxOtherParam.Controls.Add(this.txbResolution);
            this.gbxOtherParam.Controls.Add(this.labumpixel);
            this.gbxOtherParam.Controls.Add(this.labResolution);
            this.gbxOtherParam.Controls.Add(this.labImgCount);
            this.gbxOtherParam.Controls.Add(this.nudImgCount);
            this.gbxOtherParam.Location = new System.Drawing.Point(193, 51);
            this.gbxOtherParam.Name = "gbxOtherParam";
            this.gbxOtherParam.Size = new System.Drawing.Size(223, 125);
            this.gbxOtherParam.TabIndex = 119;
            this.gbxOtherParam.TabStop = false;
            this.gbxOtherParam.Text = "參數設定";
            // 
            // cbxLogEnabled
            // 
            this.cbxLogEnabled.AutoSize = true;
            this.cbxLogEnabled.Location = new System.Drawing.Point(19, 94);
            this.cbxLogEnabled.Name = "cbxLogEnabled";
            this.cbxLogEnabled.Size = new System.Drawing.Size(80, 22);
            this.cbxLogEnabled.TabIndex = 122;
            this.cbxLogEnabled.Text = "Log啟用";
            this.cbxLogEnabled.UseVisualStyleBackColor = true;
            // 
            // txbResolution
            // 
            this.txbResolution.Enabled = false;
            this.txbResolution.Location = new System.Drawing.Point(97, 63);
            this.txbResolution.Name = "txbResolution";
            this.txbResolution.Size = new System.Drawing.Size(48, 25);
            this.txbResolution.TabIndex = 121;
            this.txbResolution.Text = "3.4";
            // 
            // labumpixel
            // 
            this.labumpixel.AutoSize = true;
            this.labumpixel.Location = new System.Drawing.Point(151, 66);
            this.labumpixel.Name = "labumpixel";
            this.labumpixel.Size = new System.Drawing.Size(68, 18);
            this.labumpixel.TabIndex = 120;
            this.labumpixel.Text = "um/pixel";
            // 
            // labResolution
            // 
            this.labResolution.AutoSize = true;
            this.labResolution.Location = new System.Drawing.Point(6, 66);
            this.labResolution.Name = "labResolution";
            this.labResolution.Size = new System.Drawing.Size(85, 18);
            this.labResolution.TabIndex = 120;
            this.labResolution.Text = "Resolution:";
            // 
            // labImgCount
            // 
            this.labImgCount.AutoSize = true;
            this.labImgCount.Location = new System.Drawing.Point(16, 26);
            this.labImgCount.Name = "labImgCount";
            this.labImgCount.Size = new System.Drawing.Size(76, 18);
            this.labImgCount.TabIndex = 119;
            this.labImgCount.Text = "影像數量 : ";
            // 
            // nudImgCount
            // 
            this.nudImgCount.Location = new System.Drawing.Point(98, 24);
            this.nudImgCount.Name = "nudImgCount";
            this.nudImgCount.Size = new System.Drawing.Size(47, 25);
            this.nudImgCount.TabIndex = 118;
            this.nudImgCount.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // gbxAIType
            // 
            this.gbxAIType.Controls.Add(this.combxAIType);
            this.gbxAIType.Controls.Add(this.btnDAVSSetting);
            this.gbxAIType.Location = new System.Drawing.Point(4, 442);
            this.gbxAIType.Name = "gbxAIType";
            this.gbxAIType.Size = new System.Drawing.Size(183, 125);
            this.gbxAIType.TabIndex = 117;
            this.gbxAIType.TabStop = false;
            this.gbxAIType.Text = "AI Type";
            // 
            // combxAIType
            // 
            this.combxAIType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combxAIType.FormattingEnabled = true;
            this.combxAIType.Items.AddRange(new object[] {
            "AI與AOI獨立檢測",
            "AI覆判NG",
            "AI覆判OK"});
            this.combxAIType.Location = new System.Drawing.Point(15, 28);
            this.combxAIType.Name = "combxAIType";
            this.combxAIType.Size = new System.Drawing.Size(159, 25);
            this.combxAIType.TabIndex = 7;
            this.combxAIType.SelectedIndexChanged += new System.EventHandler(this.combxAIType_SelectedIndexChanged);
            // 
            // btnDAVSSetting
            // 
            this.btnDAVSSetting.BackColor = System.Drawing.Color.LightGray;
            this.btnDAVSSetting.Location = new System.Drawing.Point(15, 59);
            this.btnDAVSSetting.Name = "btnDAVSSetting";
            this.btnDAVSSetting.Size = new System.Drawing.Size(159, 32);
            this.btnDAVSSetting.TabIndex = 2;
            this.btnDAVSSetting.Text = "AI 參數設定";
            this.btnDAVSSetting.UseVisualStyleBackColor = false;
            this.btnDAVSSetting.Click += new System.EventHandler(this.btnCellCut_Click);
            // 
            // gbxSaveAOIImgSetting
            // 
            this.gbxSaveAOIImgSetting.Controls.Add(this.comboBoxDAVSBand3);
            this.gbxSaveAOIImgSetting.Controls.Add(this.label88);
            this.gbxSaveAOIImgSetting.Controls.Add(this.comboBoxDAVSBand2);
            this.gbxSaveAOIImgSetting.Controls.Add(this.nudDAVSImageId);
            this.gbxSaveAOIImgSetting.Controls.Add(this.comboBoxDAVSBand1);
            this.gbxSaveAOIImgSetting.Controls.Add(this.combxSaveImgType);
            this.gbxSaveAOIImgSetting.Controls.Add(this.nudDAVSBand3ImageIndex);
            this.gbxSaveAOIImgSetting.Controls.Add(this.cbxSaveImgEnable);
            this.gbxSaveAOIImgSetting.Controls.Add(this.nudDAVSBand2ImageIndex);
            this.gbxSaveAOIImgSetting.Controls.Add(this.cbxDAVSMixBandEnabled);
            this.gbxSaveAOIImgSetting.Controls.Add(this.nudDAVSBand1ImageIndex);
            this.gbxSaveAOIImgSetting.Location = new System.Drawing.Point(4, 182);
            this.gbxSaveAOIImgSetting.Name = "gbxSaveAOIImgSetting";
            this.gbxSaveAOIImgSetting.Size = new System.Drawing.Size(183, 246);
            this.gbxSaveAOIImgSetting.TabIndex = 116;
            this.gbxSaveAOIImgSetting.TabStop = false;
            this.gbxSaveAOIImgSetting.Text = "AOI儲存影像設定";
            // 
            // comboBoxDAVSBand3
            // 
            this.comboBoxDAVSBand3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDAVSBand3.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.comboBoxDAVSBand3.FormattingEnabled = true;
            this.comboBoxDAVSBand3.Location = new System.Drawing.Point(6, 208);
            this.comboBoxDAVSBand3.Name = "comboBoxDAVSBand3";
            this.comboBoxDAVSBand3.Size = new System.Drawing.Size(100, 24);
            this.comboBoxDAVSBand3.TabIndex = 126;
            // 
            // label88
            // 
            this.label88.AutoSize = true;
            this.label88.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.label88.Location = new System.Drawing.Point(6, 88);
            this.label88.Name = "label88";
            this.label88.Size = new System.Drawing.Size(35, 16);
            this.label88.TabIndex = 124;
            this.label88.Text = "影像:";
            // 
            // comboBoxDAVSBand2
            // 
            this.comboBoxDAVSBand2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDAVSBand2.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.comboBoxDAVSBand2.FormattingEnabled = true;
            this.comboBoxDAVSBand2.Location = new System.Drawing.Point(6, 177);
            this.comboBoxDAVSBand2.Name = "comboBoxDAVSBand2";
            this.comboBoxDAVSBand2.Size = new System.Drawing.Size(100, 24);
            this.comboBoxDAVSBand2.TabIndex = 126;
            // 
            // nudDAVSImageId
            // 
            this.nudDAVSImageId.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.nudDAVSImageId.Location = new System.Drawing.Point(47, 85);
            this.nudDAVSImageId.Name = "nudDAVSImageId";
            this.nudDAVSImageId.Size = new System.Drawing.Size(51, 23);
            this.nudDAVSImageId.TabIndex = 124;
            // 
            // comboBoxDAVSBand1
            // 
            this.comboBoxDAVSBand1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDAVSBand1.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.comboBoxDAVSBand1.FormattingEnabled = true;
            this.comboBoxDAVSBand1.Location = new System.Drawing.Point(6, 145);
            this.comboBoxDAVSBand1.Name = "comboBoxDAVSBand1";
            this.comboBoxDAVSBand1.Size = new System.Drawing.Size(100, 24);
            this.comboBoxDAVSBand1.TabIndex = 126;
            // 
            // combxSaveImgType
            // 
            this.combxSaveImgType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combxSaveImgType.FormattingEnabled = true;
            this.combxSaveImgType.Items.AddRange(new object[] {
            "儲存所有影像",
            "儲存NG影像",
            "儲存OK影像"});
            this.combxSaveImgType.Location = new System.Drawing.Point(9, 49);
            this.combxSaveImgType.Name = "combxSaveImgType";
            this.combxSaveImgType.Size = new System.Drawing.Size(159, 25);
            this.combxSaveImgType.TabIndex = 1;
            // 
            // nudDAVSBand3ImageIndex
            // 
            this.nudDAVSBand3ImageIndex.Location = new System.Drawing.Point(112, 208);
            this.nudDAVSBand3ImageIndex.Name = "nudDAVSBand3ImageIndex";
            this.nudDAVSBand3ImageIndex.Size = new System.Drawing.Size(64, 25);
            this.nudDAVSBand3ImageIndex.TabIndex = 125;
            // 
            // cbxSaveImgEnable
            // 
            this.cbxSaveImgEnable.AutoSize = true;
            this.cbxSaveImgEnable.Location = new System.Drawing.Point(9, 28);
            this.cbxSaveImgEnable.Name = "cbxSaveImgEnable";
            this.cbxSaveImgEnable.Size = new System.Drawing.Size(55, 22);
            this.cbxSaveImgEnable.TabIndex = 0;
            this.cbxSaveImgEnable.Text = "啟用";
            this.cbxSaveImgEnable.UseVisualStyleBackColor = true;
            this.cbxSaveImgEnable.CheckedChanged += new System.EventHandler(this.cbxSaveImgEnable_CheckedChanged);
            // 
            // nudDAVSBand2ImageIndex
            // 
            this.nudDAVSBand2ImageIndex.Location = new System.Drawing.Point(112, 177);
            this.nudDAVSBand2ImageIndex.Name = "nudDAVSBand2ImageIndex";
            this.nudDAVSBand2ImageIndex.Size = new System.Drawing.Size(64, 25);
            this.nudDAVSBand2ImageIndex.TabIndex = 125;
            // 
            // cbxDAVSMixBandEnabled
            // 
            this.cbxDAVSMixBandEnabled.AutoSize = true;
            this.cbxDAVSMixBandEnabled.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.cbxDAVSMixBandEnabled.Location = new System.Drawing.Point(9, 119);
            this.cbxDAVSMixBandEnabled.Name = "cbxDAVSMixBandEnabled";
            this.cbxDAVSMixBandEnabled.Size = new System.Drawing.Size(82, 20);
            this.cbxDAVSMixBandEnabled.TabIndex = 124;
            this.cbxDAVSMixBandEnabled.Text = "Mix Band";
            this.cbxDAVSMixBandEnabled.UseVisualStyleBackColor = true;
            // 
            // nudDAVSBand1ImageIndex
            // 
            this.nudDAVSBand1ImageIndex.Location = new System.Drawing.Point(112, 146);
            this.nudDAVSBand1ImageIndex.Name = "nudDAVSBand1ImageIndex";
            this.nudDAVSBand1ImageIndex.Size = new System.Drawing.Size(64, 25);
            this.nudDAVSBand1ImageIndex.TabIndex = 125;
            // 
            // gbxSkipParam
            // 
            this.gbxSkipParam.Controls.Add(this.combxTestModeType);
            this.gbxSkipParam.Controls.Add(this.cbxTestModeEnabled);
            this.gbxSkipParam.Location = new System.Drawing.Point(4, 51);
            this.gbxSkipParam.Name = "gbxSkipParam";
            this.gbxSkipParam.Size = new System.Drawing.Size(183, 125);
            this.gbxSkipParam.TabIndex = 115;
            this.gbxSkipParam.TabStop = false;
            this.gbxSkipParam.Text = "測試模式設定";
            // 
            // combxTestModeType
            // 
            this.combxTestModeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combxTestModeType.FormattingEnabled = true;
            this.combxTestModeType.Items.AddRange(new object[] {
            "忽略所有檢測",
            "只回傳對位中心",
            "只回傳切割範圍"});
            this.combxTestModeType.Location = new System.Drawing.Point(12, 53);
            this.combxTestModeType.Name = "combxTestModeType";
            this.combxTestModeType.Size = new System.Drawing.Size(159, 25);
            this.combxTestModeType.TabIndex = 1;
            // 
            // cbxTestModeEnabled
            // 
            this.cbxTestModeEnabled.AutoSize = true;
            this.cbxTestModeEnabled.Location = new System.Drawing.Point(12, 24);
            this.cbxTestModeEnabled.Name = "cbxTestModeEnabled";
            this.cbxTestModeEnabled.Size = new System.Drawing.Size(55, 22);
            this.cbxTestModeEnabled.TabIndex = 0;
            this.cbxTestModeEnabled.Text = "啟用";
            this.cbxTestModeEnabled.UseVisualStyleBackColor = true;
            this.cbxTestModeEnabled.CheckedChanged += new System.EventHandler(this.cbxTestModeEnabled_CheckedChanged);
            // 
            // comboBoxDisplayType
            // 
            this.comboBoxDisplayType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDisplayType.FormattingEnabled = true;
            this.comboBoxDisplayType.Items.AddRange(new object[] {
            "填滿",
            "輪廓"});
            this.comboBoxDisplayType.Location = new System.Drawing.Point(308, 13);
            this.comboBoxDisplayType.Name = "comboBoxDisplayType";
            this.comboBoxDisplayType.Size = new System.Drawing.Size(55, 24);
            this.comboBoxDisplayType.TabIndex = 121;
            this.comboBoxDisplayType.SelectedIndexChanged += new System.EventHandler(this.comboBoxDisplayType_SelectedIndexChanged);
            // 
            // gbxMatchingParam
            // 
            this.gbxMatchingParam.Controls.Add(this.cbxRotateImage);
            this.gbxMatchingParam.Controls.Add(this.nudScaleSize);
            this.gbxMatchingParam.Controls.Add(this.cbxMatchThEnabled);
            this.gbxMatchingParam.Controls.Add(this.panMatching);
            this.gbxMatchingParam.Controls.Add(this.cbxMinContrastAuto);
            this.gbxMatchingParam.Controls.Add(this.label66);
            this.gbxMatchingParam.Controls.Add(this.cbxContrastAuto);
            this.gbxMatchingParam.Controls.Add(this.cbxAngleStepAuto);
            this.gbxMatchingParam.Controls.Add(this.cbxMetric);
            this.gbxMatchingParam.Controls.Add(this.labMetric);
            this.gbxMatchingParam.Controls.Add(this.cbxOptimization);
            this.gbxMatchingParam.Controls.Add(this.labOptimization);
            this.gbxMatchingParam.Controls.Add(this.nudScaleMax);
            this.gbxMatchingParam.Controls.Add(this.nudScaleMin);
            this.gbxMatchingParam.Controls.Add(this.labScaleMax);
            this.gbxMatchingParam.Controls.Add(this.labScaleMin);
            this.gbxMatchingParam.Controls.Add(this.nudAngleStep);
            this.gbxMatchingParam.Controls.Add(this.labAngleStep);
            this.gbxMatchingParam.Controls.Add(this.nudMinContrast);
            this.gbxMatchingParam.Controls.Add(this.labMinContrast);
            this.gbxMatchingParam.Controls.Add(this.nudAngleExtent);
            this.gbxMatchingParam.Controls.Add(this.nudMinObjSize);
            this.gbxMatchingParam.Controls.Add(this.nudContrastHigh);
            this.gbxMatchingParam.Controls.Add(this.nudContrast);
            this.gbxMatchingParam.Controls.Add(this.labMInObj);
            this.gbxMatchingParam.Controls.Add(this.labAngleExtent);
            this.gbxMatchingParam.Controls.Add(this.labContrastLow);
            this.gbxMatchingParam.Controls.Add(this.nudAngleStart);
            this.gbxMatchingParam.Controls.Add(this.labAngleStart);
            this.gbxMatchingParam.Controls.Add(this.nudNumLevels);
            this.gbxMatchingParam.Controls.Add(this.labNumLevels);
            this.gbxMatchingParam.Location = new System.Drawing.Point(6, 321);
            this.gbxMatchingParam.Name = "gbxMatchingParam";
            this.gbxMatchingParam.Size = new System.Drawing.Size(342, 471);
            this.gbxMatchingParam.TabIndex = 6;
            this.gbxMatchingParam.TabStop = false;
            this.gbxMatchingParam.Text = "圖樣進階參數設定";
            // 
            // cbxRotateImage
            // 
            this.cbxRotateImage.AutoSize = true;
            this.cbxRotateImage.Location = new System.Drawing.Point(195, 39);
            this.cbxRotateImage.Name = "cbxRotateImage";
            this.cbxRotateImage.Size = new System.Drawing.Size(75, 20);
            this.cbxRotateImage.TabIndex = 87;
            this.cbxRotateImage.Text = "影像旋轉";
            this.cbxRotateImage.UseVisualStyleBackColor = true;
            // 
            // nudScaleSize
            // 
            this.nudScaleSize.DecimalPlaces = 4;
            this.nudScaleSize.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudScaleSize.Location = new System.Drawing.Point(90, 405);
            this.nudScaleSize.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudScaleSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.nudScaleSize.Name = "nudScaleSize";
            this.nudScaleSize.Size = new System.Drawing.Size(64, 23);
            this.nudScaleSize.TabIndex = 11;
            this.nudScaleSize.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cbxMatchThEnabled
            // 
            this.cbxMatchThEnabled.AutoSize = true;
            this.cbxMatchThEnabled.Location = new System.Drawing.Point(195, 20);
            this.cbxMatchThEnabled.Name = "cbxMatchThEnabled";
            this.cbxMatchThEnabled.Size = new System.Drawing.Size(123, 20);
            this.cbxMatchThEnabled.TabIndex = 84;
            this.cbxMatchThEnabled.Text = "對位時是否前處理";
            this.cbxMatchThEnabled.UseVisualStyleBackColor = true;
            // 
            // panMatching
            // 
            this.panMatching.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panMatching.Controls.Add(this.nudMatchNumber);
            this.panMatching.Controls.Add(this.label82);
            this.panMatching.Controls.Add(this.labGreediness);
            this.panMatching.Controls.Add(this.labOverlap);
            this.panMatching.Controls.Add(this.nudGreediness);
            this.panMatching.Controls.Add(this.nudOverlap);
            this.panMatching.Controls.Add(this.cbxSubPixel);
            this.panMatching.Controls.Add(this.labSubPixel);
            this.panMatching.Location = new System.Drawing.Point(17, 298);
            this.panMatching.Name = "panMatching";
            this.panMatching.Size = new System.Drawing.Size(323, 95);
            this.panMatching.TabIndex = 8;
            // 
            // nudMatchNumber
            // 
            this.nudMatchNumber.Location = new System.Drawing.Point(252, 35);
            this.nudMatchNumber.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.nudMatchNumber.Name = "nudMatchNumber";
            this.nudMatchNumber.Size = new System.Drawing.Size(63, 23);
            this.nudMatchNumber.TabIndex = 8;
            // 
            // label82
            // 
            this.label82.AutoSize = true;
            this.label82.Location = new System.Drawing.Point(187, 39);
            this.label82.Name = "label82";
            this.label82.Size = new System.Drawing.Size(59, 16);
            this.label82.TabIndex = 7;
            this.label82.Text = "匹配個數:";
            // 
            // labGreediness
            // 
            this.labGreediness.AutoSize = true;
            this.labGreediness.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.labGreediness.Location = new System.Drawing.Point(12, 67);
            this.labGreediness.Name = "labGreediness";
            this.labGreediness.Size = new System.Drawing.Size(54, 18);
            this.labGreediness.TabIndex = 3;
            this.labGreediness.Text = "貪婪度:";
            // 
            // labOverlap
            // 
            this.labOverlap.AutoSize = true;
            this.labOverlap.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.labOverlap.Location = new System.Drawing.Point(12, 36);
            this.labOverlap.Name = "labOverlap";
            this.labOverlap.Size = new System.Drawing.Size(68, 18);
            this.labOverlap.TabIndex = 3;
            this.labOverlap.Text = "交疊比例:";
            // 
            // nudGreediness
            // 
            this.nudGreediness.DecimalPlaces = 1;
            this.nudGreediness.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudGreediness.Location = new System.Drawing.Point(115, 65);
            this.nudGreediness.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudGreediness.Name = "nudGreediness";
            this.nudGreediness.Size = new System.Drawing.Size(64, 23);
            this.nudGreediness.TabIndex = 4;
            this.nudGreediness.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            // 
            // nudOverlap
            // 
            this.nudOverlap.DecimalPlaces = 1;
            this.nudOverlap.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudOverlap.Location = new System.Drawing.Point(115, 34);
            this.nudOverlap.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudOverlap.Name = "nudOverlap";
            this.nudOverlap.Size = new System.Drawing.Size(64, 23);
            this.nudOverlap.TabIndex = 4;
            this.nudOverlap.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            // 
            // cbxSubPixel
            // 
            this.cbxSubPixel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxSubPixel.FormattingEnabled = true;
            this.cbxSubPixel.Items.AddRange(new object[] {
            "none",
            "interpolation",
            "least_squares",
            "least_squares_high",
            "least_squares_very_high"});
            this.cbxSubPixel.Location = new System.Drawing.Point(115, 3);
            this.cbxSubPixel.Name = "cbxSubPixel";
            this.cbxSubPixel.Size = new System.Drawing.Size(197, 24);
            this.cbxSubPixel.TabIndex = 6;
            // 
            // labSubPixel
            // 
            this.labSubPixel.AutoSize = true;
            this.labSubPixel.Location = new System.Drawing.Point(12, 7);
            this.labSubPixel.Name = "labSubPixel";
            this.labSubPixel.Size = new System.Drawing.Size(59, 16);
            this.labSubPixel.TabIndex = 5;
            this.labSubPixel.Text = "像素精度:";
            // 
            // cbxMinContrastAuto
            // 
            this.cbxMinContrastAuto.AutoSize = true;
            this.cbxMinContrastAuto.Location = new System.Drawing.Point(187, 205);
            this.cbxMinContrastAuto.Name = "cbxMinContrastAuto";
            this.cbxMinContrastAuto.Size = new System.Drawing.Size(51, 20);
            this.cbxMinContrastAuto.TabIndex = 7;
            this.cbxMinContrastAuto.Text = "自動";
            this.cbxMinContrastAuto.UseVisualStyleBackColor = true;
            // 
            // label66
            // 
            this.label66.AutoSize = true;
            this.label66.Location = new System.Drawing.Point(14, 409);
            this.label66.Name = "label66";
            this.label66.Size = new System.Drawing.Size(65, 16);
            this.label66.TabIndex = 10;
            this.label66.Text = "縮圖比例 : ";
            // 
            // cbxContrastAuto
            // 
            this.cbxContrastAuto.AutoSize = true;
            this.cbxContrastAuto.Location = new System.Drawing.Point(230, 146);
            this.cbxContrastAuto.Name = "cbxContrastAuto";
            this.cbxContrastAuto.Size = new System.Drawing.Size(51, 20);
            this.cbxContrastAuto.TabIndex = 7;
            this.cbxContrastAuto.Text = "自動";
            this.cbxContrastAuto.UseVisualStyleBackColor = true;
            // 
            // cbxAngleStepAuto
            // 
            this.cbxAngleStepAuto.AutoSize = true;
            this.cbxAngleStepAuto.Location = new System.Drawing.Point(187, 117);
            this.cbxAngleStepAuto.Name = "cbxAngleStepAuto";
            this.cbxAngleStepAuto.Size = new System.Drawing.Size(51, 20);
            this.cbxAngleStepAuto.TabIndex = 7;
            this.cbxAngleStepAuto.Text = "自動";
            this.cbxAngleStepAuto.UseVisualStyleBackColor = true;
            // 
            // cbxMetric
            // 
            this.cbxMetric.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxMetric.FormattingEnabled = true;
            this.cbxMetric.Items.AddRange(new object[] {
            "ignore_color_polarity",
            "ignore_global_polarity",
            "ignore_local_polarity",
            "use_polarity"});
            this.cbxMetric.Location = new System.Drawing.Point(108, 268);
            this.cbxMetric.Name = "cbxMetric";
            this.cbxMetric.Size = new System.Drawing.Size(197, 24);
            this.cbxMetric.TabIndex = 6;
            // 
            // labMetric
            // 
            this.labMetric.AutoSize = true;
            this.labMetric.Location = new System.Drawing.Point(14, 271);
            this.labMetric.Name = "labMetric";
            this.labMetric.Size = new System.Drawing.Size(59, 16);
            this.labMetric.TabIndex = 5;
            this.labMetric.Text = "極性模式:";
            // 
            // cbxOptimization
            // 
            this.cbxOptimization.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxOptimization.FormattingEnabled = true;
            this.cbxOptimization.Items.AddRange(new object[] {
            "auto",
            "no_pregeneration",
            "none",
            "point_reduction_high",
            "point_reduction_low",
            "point_reduction_medium",
            "pregeneration"});
            this.cbxOptimization.Location = new System.Drawing.Point(108, 232);
            this.cbxOptimization.Name = "cbxOptimization";
            this.cbxOptimization.Size = new System.Drawing.Size(197, 24);
            this.cbxOptimization.TabIndex = 6;
            // 
            // labOptimization
            // 
            this.labOptimization.AutoSize = true;
            this.labOptimization.Location = new System.Drawing.Point(14, 235);
            this.labOptimization.Name = "labOptimization";
            this.labOptimization.Size = new System.Drawing.Size(59, 16);
            this.labOptimization.TabIndex = 5;
            this.labOptimization.Text = "優化算法:";
            // 
            // nudScaleMax
            // 
            this.nudScaleMax.DecimalPlaces = 1;
            this.nudScaleMax.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudScaleMax.Location = new System.Drawing.Point(272, 86);
            this.nudScaleMax.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nudScaleMax.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudScaleMax.Name = "nudScaleMax";
            this.nudScaleMax.Size = new System.Drawing.Size(64, 23);
            this.nudScaleMax.TabIndex = 4;
            this.nudScaleMax.Value = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.nudScaleMax.Visible = false;
            this.nudScaleMax.ValueChanged += new System.EventHandler(this.nudScaleMax_ValueChanged);
            // 
            // nudScaleMin
            // 
            this.nudScaleMin.DecimalPlaces = 1;
            this.nudScaleMin.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudScaleMin.Location = new System.Drawing.Point(272, 57);
            this.nudScaleMin.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nudScaleMin.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudScaleMin.Name = "nudScaleMin";
            this.nudScaleMin.Size = new System.Drawing.Size(64, 23);
            this.nudScaleMin.TabIndex = 4;
            this.nudScaleMin.Value = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.nudScaleMin.Visible = false;
            this.nudScaleMin.ValueChanged += new System.EventHandler(this.nudScaleMin_ValueChanged);
            // 
            // labScaleMax
            // 
            this.labScaleMax.AutoSize = true;
            this.labScaleMax.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.labScaleMax.Location = new System.Drawing.Point(192, 89);
            this.labScaleMax.Name = "labScaleMax";
            this.labScaleMax.Size = new System.Drawing.Size(66, 16);
            this.labScaleMax.TabIndex = 3;
            this.labScaleMax.Text = "ScaleMax:";
            this.labScaleMax.Visible = false;
            // 
            // labScaleMin
            // 
            this.labScaleMin.AutoSize = true;
            this.labScaleMin.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.labScaleMin.Location = new System.Drawing.Point(192, 62);
            this.labScaleMin.Name = "labScaleMin";
            this.labScaleMin.Size = new System.Drawing.Size(64, 16);
            this.labScaleMin.TabIndex = 3;
            this.labScaleMin.Text = "ScaleMin:";
            this.labScaleMin.Visible = false;
            // 
            // nudAngleStep
            // 
            this.nudAngleStep.DecimalPlaces = 3;
            this.nudAngleStep.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudAngleStep.Location = new System.Drawing.Point(117, 115);
            this.nudAngleStep.Maximum = new decimal(new int[] {
            19,
            0,
            0,
            131072});
            this.nudAngleStep.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            327680});
            this.nudAngleStep.Name = "nudAngleStep";
            this.nudAngleStep.Size = new System.Drawing.Size(64, 23);
            this.nudAngleStep.TabIndex = 4;
            this.nudAngleStep.Value = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            // 
            // labAngleStep
            // 
            this.labAngleStep.AutoSize = true;
            this.labAngleStep.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.labAngleStep.Location = new System.Drawing.Point(14, 117);
            this.labAngleStep.Name = "labAngleStep";
            this.labAngleStep.Size = new System.Drawing.Size(83, 16);
            this.labAngleStep.TabIndex = 3;
            this.labAngleStep.Text = "角度搜尋步長:";
            // 
            // nudMinContrast
            // 
            this.nudMinContrast.Location = new System.Drawing.Point(117, 205);
            this.nudMinContrast.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudMinContrast.Name = "nudMinContrast";
            this.nudMinContrast.Size = new System.Drawing.Size(64, 23);
            this.nudMinContrast.TabIndex = 4;
            this.nudMinContrast.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // labMinContrast
            // 
            this.labMinContrast.AutoSize = true;
            this.labMinContrast.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.labMinContrast.Location = new System.Drawing.Point(14, 207);
            this.labMinContrast.Name = "labMinContrast";
            this.labMinContrast.Size = new System.Drawing.Size(59, 16);
            this.labMinContrast.TabIndex = 3;
            this.labMinContrast.Text = "最小對比:";
            // 
            // nudAngleExtent
            // 
            this.nudAngleExtent.Location = new System.Drawing.Point(117, 84);
            this.nudAngleExtent.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.nudAngleExtent.Name = "nudAngleExtent";
            this.nudAngleExtent.Size = new System.Drawing.Size(64, 23);
            this.nudAngleExtent.TabIndex = 4;
            this.nudAngleExtent.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // nudMinObjSize
            // 
            this.nudMinObjSize.Location = new System.Drawing.Point(117, 177);
            this.nudMinObjSize.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.nudMinObjSize.Name = "nudMinObjSize";
            this.nudMinObjSize.Size = new System.Drawing.Size(64, 23);
            this.nudMinObjSize.TabIndex = 4;
            this.nudMinObjSize.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            // 
            // nudContrastHigh
            // 
            this.nudContrastHigh.Location = new System.Drawing.Point(160, 146);
            this.nudContrastHigh.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudContrastHigh.Name = "nudContrastHigh";
            this.nudContrastHigh.Size = new System.Drawing.Size(64, 23);
            this.nudContrastHigh.TabIndex = 4;
            this.nudContrastHigh.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            // 
            // nudContrast
            // 
            this.nudContrast.Location = new System.Drawing.Point(90, 146);
            this.nudContrast.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudContrast.Name = "nudContrast";
            this.nudContrast.Size = new System.Drawing.Size(64, 23);
            this.nudContrast.TabIndex = 4;
            this.nudContrast.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // labMInObj
            // 
            this.labMInObj.AutoSize = true;
            this.labMInObj.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.labMInObj.Location = new System.Drawing.Point(14, 180);
            this.labMInObj.Name = "labMInObj";
            this.labMInObj.Size = new System.Drawing.Size(59, 16);
            this.labMInObj.TabIndex = 3;
            this.labMInObj.Text = "最小面積:";
            // 
            // labAngleExtent
            // 
            this.labAngleExtent.AutoSize = true;
            this.labAngleExtent.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.labAngleExtent.Location = new System.Drawing.Point(14, 86);
            this.labAngleExtent.Name = "labAngleExtent";
            this.labAngleExtent.Size = new System.Drawing.Size(59, 16);
            this.labAngleExtent.TabIndex = 3;
            this.labAngleExtent.Text = "延伸角度:";
            // 
            // labContrastLow
            // 
            this.labContrastLow.AutoSize = true;
            this.labContrastLow.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.labContrastLow.Location = new System.Drawing.Point(14, 148);
            this.labContrastLow.Name = "labContrastLow";
            this.labContrastLow.Size = new System.Drawing.Size(47, 16);
            this.labContrastLow.TabIndex = 3;
            this.labContrastLow.Text = "對比度:";
            // 
            // nudAngleStart
            // 
            this.nudAngleStart.Location = new System.Drawing.Point(117, 53);
            this.nudAngleStart.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.nudAngleStart.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.nudAngleStart.Name = "nudAngleStart";
            this.nudAngleStart.Size = new System.Drawing.Size(64, 23);
            this.nudAngleStart.TabIndex = 4;
            // 
            // labAngleStart
            // 
            this.labAngleStart.AutoSize = true;
            this.labAngleStart.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.labAngleStart.Location = new System.Drawing.Point(14, 55);
            this.labAngleStart.Name = "labAngleStart";
            this.labAngleStart.Size = new System.Drawing.Size(59, 16);
            this.labAngleStart.TabIndex = 3;
            this.labAngleStart.Text = "起始角度:";
            // 
            // nudNumLevels
            // 
            this.nudNumLevels.Location = new System.Drawing.Point(117, 22);
            this.nudNumLevels.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudNumLevels.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudNumLevels.Name = "nudNumLevels";
            this.nudNumLevels.Size = new System.Drawing.Size(64, 23);
            this.nudNumLevels.TabIndex = 4;
            this.nudNumLevels.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // labNumLevels
            // 
            this.labNumLevels.AutoSize = true;
            this.labNumLevels.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.labNumLevels.Location = new System.Drawing.Point(14, 27);
            this.labNumLevels.Name = "labNumLevels";
            this.labNumLevels.Size = new System.Drawing.Size(71, 16);
            this.labNumLevels.TabIndex = 3;
            this.labNumLevels.Text = "金字塔階層:";
            // 
            // combxDisplayImg
            // 
            this.combxDisplayImg.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combxDisplayImg.FormattingEnabled = true;
            this.combxDisplayImg.Location = new System.Drawing.Point(243, 13);
            this.combxDisplayImg.Name = "combxDisplayImg";
            this.combxDisplayImg.Size = new System.Drawing.Size(59, 24);
            this.combxDisplayImg.TabIndex = 117;
            this.combxDisplayImg.SelectedIndexChanged += new System.EventHandler(this.combxDisplayImg_SelectedIndexChanged);
            // 
            // btnLoadImg
            // 
            this.btnLoadImg.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnLoadImg.Font = new System.Drawing.Font("微軟正黑體", 8F, System.Drawing.FontStyle.Bold);
            this.btnLoadImg.Image = ((System.Drawing.Image)(resources.GetObject("btnLoadImg.Image")));
            this.btnLoadImg.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnLoadImg.Location = new System.Drawing.Point(3, 3);
            this.btnLoadImg.Name = "btnLoadImg";
            this.btnLoadImg.Size = new System.Drawing.Size(74, 43);
            this.btnLoadImg.TabIndex = 74;
            this.btnLoadImg.Text = "載入影像";
            this.btnLoadImg.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnLoadImg.UseVisualStyleBackColor = true;
            this.btnLoadImg.Click += new System.EventHandler(this.btnLoadImg_Click);
            // 
            // btnSaveImg
            // 
            this.btnSaveImg.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSaveImg.Font = new System.Drawing.Font("微軟正黑體", 8F, System.Drawing.FontStyle.Bold);
            this.btnSaveImg.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveImg.Image")));
            this.btnSaveImg.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSaveImg.Location = new System.Drawing.Point(163, 3);
            this.btnSaveImg.Name = "btnSaveImg";
            this.btnSaveImg.Size = new System.Drawing.Size(74, 43);
            this.btnSaveImg.TabIndex = 121;
            this.btnSaveImg.Text = "儲存結果";
            this.btnSaveImg.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSaveImg.UseVisualStyleBackColor = true;
            this.btnSaveImg.Click += new System.EventHandler(this.btnSaveImg_Click);
            // 
            // btnAddImsge
            // 
            this.btnAddImsge.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAddImsge.Font = new System.Drawing.Font("微軟正黑體", 8F, System.Drawing.FontStyle.Bold);
            this.btnAddImsge.Image = ((System.Drawing.Image)(resources.GetObject("btnAddImsge.Image")));
            this.btnAddImsge.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnAddImsge.Location = new System.Drawing.Point(83, 3);
            this.btnAddImsge.Name = "btnAddImsge";
            this.btnAddImsge.Size = new System.Drawing.Size(74, 43);
            this.btnAddImsge.TabIndex = 118;
            this.btnAddImsge.Text = "新增影像";
            this.btnAddImsge.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAddImsge.UseVisualStyleBackColor = true;
            this.btnAddImsge.Click += new System.EventHandler(this.btnAddImsge_Click);
            // 
            // tbInspParamSetting
            // 
            this.tbInspParamSetting.Controls.Add(this.tpPatternMatch);
            this.tbInspParamSetting.Controls.Add(this.tpAlgoImage);
            this.tbInspParamSetting.Controls.Add(this.tpRgionMethod);
            this.tbInspParamSetting.Controls.Add(this.labAnisometry);
            this.tbInspParamSetting.Controls.Add(this.Inner);
            this.tbInspParamSetting.Controls.Add(this.Outer);
            this.tbInspParamSetting.Controls.Add(this.Thin);
            this.tbInspParamSetting.Controls.Add(this.Scratch);
            this.tbInspParamSetting.Controls.Add(this.Stain);
            this.tbInspParamSetting.Controls.Add(this.RDefect);
            this.tbInspParamSetting.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.tbInspParamSetting.Location = new System.Drawing.Point(1152, 2);
            this.tbInspParamSetting.Name = "tbInspParamSetting";
            this.tbInspParamSetting.SelectedIndex = 0;
            this.tbInspParamSetting.Size = new System.Drawing.Size(390, 846);
            this.tbInspParamSetting.TabIndex = 112;
            // 
            // tpPatternMatch
            // 
            this.tpPatternMatch.AutoScroll = true;
            this.tpPatternMatch.BackColor = System.Drawing.SystemColors.Control;
            this.tpPatternMatch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tpPatternMatch.Controls.Add(this.gbxMatchingParam);
            this.tpPatternMatch.Controls.Add(this.gbxParam);
            this.tpPatternMatch.ForeColor = System.Drawing.SystemColors.Desktop;
            this.tpPatternMatch.Location = new System.Drawing.Point(4, 25);
            this.tpPatternMatch.Name = "tpPatternMatch";
            this.tpPatternMatch.Padding = new System.Windows.Forms.Padding(3);
            this.tpPatternMatch.Size = new System.Drawing.Size(382, 817);
            this.tpPatternMatch.TabIndex = 0;
            this.tpPatternMatch.Text = "圖像教導";
            // 
            // gbxParam
            // 
            this.gbxParam.Controls.Add(this.gbx_PatternMatch_PreProcess);
            this.gbxParam.Controls.Add(this.radioButton_OrigImg_Match);
            this.gbxParam.Controls.Add(this.radioButton_ImgA_Match);
            this.gbxParam.Controls.Add(this.nudInspectImgID);
            this.gbxParam.Controls.Add(this.labDefectCount);
            this.gbxParam.Controls.Add(this.labMatchCount);
            this.gbxParam.Controls.Add(this.cbxOutBondary);
            this.gbxParam.Controls.Add(this.panel2);
            this.gbxParam.Controls.Add(this.cbxNCCMode);
            this.gbxParam.Controls.Add(this.btnSegCellTest);
            this.gbxParam.Controls.Add(this.labMatchID);
            this.gbxParam.Controls.Add(this.combxBand);
            this.gbxParam.Controls.Add(this.btnATestMatch);
            this.gbxParam.Controls.Add(this.labBand);
            this.gbxParam.Controls.Add(this.cbxCellRegion);
            this.gbxParam.Controls.Add(this.nudAMinScore);
            this.gbxParam.Controls.Add(this.cbxASetPattern);
            this.gbxParam.Controls.Add(this.labAMinScore);
            this.gbxParam.Controls.Add(this.btnATeachPattern);
            this.gbxParam.Location = new System.Drawing.Point(6, 6);
            this.gbxParam.Name = "gbxParam";
            this.gbxParam.Size = new System.Drawing.Size(365, 309);
            this.gbxParam.TabIndex = 5;
            this.gbxParam.TabStop = false;
            this.gbxParam.Text = "參數設定";
            // 
            // gbx_PatternMatch_PreProcess
            // 
            this.gbx_PatternMatch_PreProcess.Controls.Add(this.btnTest_PatternMatch_PreProcess);
            this.gbx_PatternMatch_PreProcess.Controls.Add(this.cbxFillup);
            this.gbx_PatternMatch_PreProcess.Controls.Add(this.cbxThEnabled);
            this.gbx_PatternMatch_PreProcess.Controls.Add(this.label92);
            this.gbx_PatternMatch_PreProcess.Controls.Add(this.nudOpeningNum);
            this.gbx_PatternMatch_PreProcess.Controls.Add(this.labMorphologyNum);
            this.gbx_PatternMatch_PreProcess.Controls.Add(this.btnThSetup);
            this.gbx_PatternMatch_PreProcess.Controls.Add(this.nudFillupMin);
            this.gbx_PatternMatch_PreProcess.Controls.Add(this.comboBoxFillupType);
            this.gbx_PatternMatch_PreProcess.Controls.Add(this.nudClosingNum);
            this.gbx_PatternMatch_PreProcess.Controls.Add(this.nudFillupMax);
            this.gbx_PatternMatch_PreProcess.Controls.Add(this.label90);
            this.gbx_PatternMatch_PreProcess.Location = new System.Drawing.Point(168, 45);
            this.gbx_PatternMatch_PreProcess.Name = "gbx_PatternMatch_PreProcess";
            this.gbx_PatternMatch_PreProcess.Size = new System.Drawing.Size(186, 237);
            this.gbx_PatternMatch_PreProcess.TabIndex = 124;
            this.gbx_PatternMatch_PreProcess.TabStop = false;
            this.gbx_PatternMatch_PreProcess.Text = "影像前處理";
            // 
            // btnTest_PatternMatch_PreProcess
            // 
            this.btnTest_PatternMatch_PreProcess.Enabled = false;
            this.btnTest_PatternMatch_PreProcess.Location = new System.Drawing.Point(99, 19);
            this.btnTest_PatternMatch_PreProcess.Name = "btnTest_PatternMatch_PreProcess";
            this.btnTest_PatternMatch_PreProcess.Size = new System.Drawing.Size(75, 23);
            this.btnTest_PatternMatch_PreProcess.TabIndex = 134;
            this.btnTest_PatternMatch_PreProcess.Tag = "測試";
            this.btnTest_PatternMatch_PreProcess.Text = "測試";
            this.btnTest_PatternMatch_PreProcess.UseVisualStyleBackColor = true;
            this.btnTest_PatternMatch_PreProcess.Click += new System.EventHandler(this.PatternMatch_PreProcess_ValueChanged);
            // 
            // cbxFillup
            // 
            this.cbxFillup.AutoSize = true;
            this.cbxFillup.Location = new System.Drawing.Point(111, 158);
            this.cbxFillup.Name = "cbxFillup";
            this.cbxFillup.Size = new System.Drawing.Size(51, 20);
            this.cbxFillup.TabIndex = 133;
            this.cbxFillup.Tag = "填滿";
            this.cbxFillup.Text = "填滿";
            this.cbxFillup.UseVisualStyleBackColor = true;
            this.cbxFillup.CheckedChanged += new System.EventHandler(this.PatternMatch_PreProcess_ValueChanged);
            // 
            // cbxThEnabled
            // 
            this.cbxThEnabled.AutoSize = true;
            this.cbxThEnabled.Location = new System.Drawing.Point(6, 24);
            this.cbxThEnabled.Name = "cbxThEnabled";
            this.cbxThEnabled.Size = new System.Drawing.Size(63, 20);
            this.cbxThEnabled.TabIndex = 74;
            this.cbxThEnabled.Text = "二值化";
            this.cbxThEnabled.UseVisualStyleBackColor = true;
            this.cbxThEnabled.CheckedChanged += new System.EventHandler(this.cbxThEnabled_CheckedChanged);
            // 
            // label92
            // 
            this.label92.AutoSize = true;
            this.label92.Location = new System.Drawing.Point(68, 191);
            this.label92.Name = "label92";
            this.label92.Size = new System.Drawing.Size(17, 16);
            this.label92.TabIndex = 131;
            this.label92.Text = "~";
            // 
            // nudOpeningNum
            // 
            this.nudOpeningNum.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudOpeningNum.Location = new System.Drawing.Point(57, 117);
            this.nudOpeningNum.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudOpeningNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudOpeningNum.Name = "nudOpeningNum";
            this.nudOpeningNum.Size = new System.Drawing.Size(46, 25);
            this.nudOpeningNum.TabIndex = 79;
            this.nudOpeningNum.Tag = "開運算";
            this.nudOpeningNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudOpeningNum.ValueChanged += new System.EventHandler(this.PatternMatch_PreProcess_ValueChanged);
            // 
            // labMorphologyNum
            // 
            this.labMorphologyNum.AutoSize = true;
            this.labMorphologyNum.Location = new System.Drawing.Point(3, 120);
            this.labMorphologyNum.Name = "labMorphologyNum";
            this.labMorphologyNum.Size = new System.Drawing.Size(47, 16);
            this.labMorphologyNum.TabIndex = 81;
            this.labMorphologyNum.Text = "開運算:";
            // 
            // btnThSetup
            // 
            this.btnThSetup.Location = new System.Drawing.Point(6, 50);
            this.btnThSetup.Name = "btnThSetup";
            this.btnThSetup.Size = new System.Drawing.Size(98, 32);
            this.btnThSetup.TabIndex = 75;
            this.btnThSetup.Text = "二值化設定";
            this.btnThSetup.UseVisualStyleBackColor = true;
            this.btnThSetup.Click += new System.EventHandler(this.btnThSetup_Click);
            // 
            // nudFillupMin
            // 
            this.nudFillupMin.Location = new System.Drawing.Point(9, 188);
            this.nudFillupMin.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.nudFillupMin.Name = "nudFillupMin";
            this.nudFillupMin.Size = new System.Drawing.Size(53, 23);
            this.nudFillupMin.TabIndex = 130;
            this.nudFillupMin.Tag = "填滿";
            this.nudFillupMin.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudFillupMin.ValueChanged += new System.EventHandler(this.PatternMatch_PreProcess_ValueChanged);
            // 
            // comboBoxFillupType
            // 
            this.comboBoxFillupType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFillupType.FormattingEnabled = true;
            this.comboBoxFillupType.Items.AddRange(new object[] {
            "anisometry",
            "area",
            "compactness",
            "convexity",
            "inner_circle",
            "outer_circle",
            "phi",
            "ra",
            "rb"});
            this.comboBoxFillupType.Location = new System.Drawing.Point(9, 156);
            this.comboBoxFillupType.Name = "comboBoxFillupType";
            this.comboBoxFillupType.Size = new System.Drawing.Size(97, 24);
            this.comboBoxFillupType.TabIndex = 132;
            this.comboBoxFillupType.Tag = "填滿";
            this.comboBoxFillupType.SelectedIndexChanged += new System.EventHandler(this.PatternMatch_PreProcess_ValueChanged);
            // 
            // nudClosingNum
            // 
            this.nudClosingNum.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudClosingNum.Location = new System.Drawing.Point(57, 84);
            this.nudClosingNum.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudClosingNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudClosingNum.Name = "nudClosingNum";
            this.nudClosingNum.Size = new System.Drawing.Size(46, 25);
            this.nudClosingNum.TabIndex = 79;
            this.nudClosingNum.Tag = "閉運算";
            this.nudClosingNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudClosingNum.ValueChanged += new System.EventHandler(this.PatternMatch_PreProcess_ValueChanged);
            // 
            // nudFillupMax
            // 
            this.nudFillupMax.Location = new System.Drawing.Point(91, 188);
            this.nudFillupMax.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.nudFillupMax.Name = "nudFillupMax";
            this.nudFillupMax.Size = new System.Drawing.Size(53, 23);
            this.nudFillupMax.TabIndex = 129;
            this.nudFillupMax.Tag = "填滿";
            this.nudFillupMax.Value = new decimal(new int[] {
            6000,
            0,
            0,
            0});
            this.nudFillupMax.ValueChanged += new System.EventHandler(this.PatternMatch_PreProcess_ValueChanged);
            // 
            // label90
            // 
            this.label90.AutoSize = true;
            this.label90.Location = new System.Drawing.Point(3, 89);
            this.label90.Name = "label90";
            this.label90.Size = new System.Drawing.Size(47, 16);
            this.label90.TabIndex = 81;
            this.label90.Text = "閉運算:";
            // 
            // radioButton_OrigImg_Match
            // 
            this.radioButton_OrigImg_Match.AutoSize = true;
            this.radioButton_OrigImg_Match.Checked = true;
            this.radioButton_OrigImg_Match.Font = new System.Drawing.Font("微軟正黑體", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.radioButton_OrigImg_Match.Location = new System.Drawing.Point(5, 16);
            this.radioButton_OrigImg_Match.Name = "radioButton_OrigImg_Match";
            this.radioButton_OrigImg_Match.Size = new System.Drawing.Size(47, 19);
            this.radioButton_OrigImg_Match.TabIndex = 113;
            this.radioButton_OrigImg_Match.TabStop = true;
            this.radioButton_OrigImg_Match.Tag = "原始";
            this.radioButton_OrigImg_Match.Text = "原始";
            this.radioButton_OrigImg_Match.UseVisualStyleBackColor = true;
            this.radioButton_OrigImg_Match.CheckedChanged += new System.EventHandler(this.radioButton_Img_Match_CheckedChanged);
            // 
            // radioButton_ImgA_Match
            // 
            this.radioButton_ImgA_Match.AutoSize = true;
            this.radioButton_ImgA_Match.Font = new System.Drawing.Font("微軟正黑體", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.radioButton_ImgA_Match.Location = new System.Drawing.Point(75, 16);
            this.radioButton_ImgA_Match.Name = "radioButton_ImgA_Match";
            this.radioButton_ImgA_Match.Size = new System.Drawing.Size(55, 19);
            this.radioButton_ImgA_Match.TabIndex = 114;
            this.radioButton_ImgA_Match.TabStop = true;
            this.radioButton_ImgA_Match.Tag = "影像A";
            this.radioButton_ImgA_Match.Text = "影像A";
            this.radioButton_ImgA_Match.UseVisualStyleBackColor = true;
            this.radioButton_ImgA_Match.CheckedChanged += new System.EventHandler(this.radioButton_Img_Match_CheckedChanged);
            // 
            // nudInspectImgID
            // 
            this.nudInspectImgID.Font = new System.Drawing.Font("Verdana", 9.75F);
            this.nudInspectImgID.Location = new System.Drawing.Point(205, 15);
            this.nudInspectImgID.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.nudInspectImgID.Name = "nudInspectImgID";
            this.nudInspectImgID.Size = new System.Drawing.Size(47, 23);
            this.nudInspectImgID.TabIndex = 112;
            // 
            // labDefectCount
            // 
            this.labDefectCount.AutoSize = true;
            this.labDefectCount.Location = new System.Drawing.Point(257, 287);
            this.labDefectCount.Name = "labDefectCount";
            this.labDefectCount.Size = new System.Drawing.Size(56, 16);
            this.labDefectCount.TabIndex = 88;
            this.labDefectCount.Text = "瑕疵個數";
            // 
            // labMatchCount
            // 
            this.labMatchCount.AutoSize = true;
            this.labMatchCount.Location = new System.Drawing.Point(167, 287);
            this.labMatchCount.Name = "labMatchCount";
            this.labMatchCount.Size = new System.Drawing.Size(56, 16);
            this.labMatchCount.TabIndex = 88;
            this.labMatchCount.Text = "對位個數";
            // 
            // cbxOutBondary
            // 
            this.cbxOutBondary.AutoSize = true;
            this.cbxOutBondary.Location = new System.Drawing.Point(5, 283);
            this.cbxOutBondary.Name = "cbxOutBondary";
            this.cbxOutBondary.Size = new System.Drawing.Size(87, 20);
            this.cbxOutBondary.TabIndex = 87;
            this.cbxOutBondary.Text = "邊界外搜尋";
            this.cbxOutBondary.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.cbxHistoEq);
            this.panel2.Controls.Add(this.label22);
            this.panel2.Controls.Add(this.label21);
            this.panel2.Controls.Add(this.cbxMaskEnabled);
            this.panel2.Controls.Add(this.nudMatchCloseHeight);
            this.panel2.Controls.Add(this.nudMatchCloseWidth);
            this.panel2.Controls.Add(this.nudMatchOpenHeight);
            this.panel2.Controls.Add(this.nudMatchOpenWidth);
            this.panel2.Controls.Add(this.btnMaskTH);
            this.panel2.Location = new System.Drawing.Point(168, 164);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(168, 118);
            this.panel2.TabIndex = 86;
            // 
            // cbxHistoEq
            // 
            this.cbxHistoEq.AutoSize = true;
            this.cbxHistoEq.Location = new System.Drawing.Point(3, 86);
            this.cbxHistoEq.Name = "cbxHistoEq";
            this.cbxHistoEq.Size = new System.Drawing.Size(75, 20);
            this.cbxHistoEq.TabIndex = 88;
            this.cbxHistoEq.Text = "加強對比";
            this.cbxHistoEq.UseVisualStyleBackColor = true;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(0, 61);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(41, 16);
            this.label22.TabIndex = 76;
            this.label22.Text = "閉合 : ";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(0, 32);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(41, 16);
            this.label21.TabIndex = 76;
            this.label21.Text = "斷開 : ";
            // 
            // cbxMaskEnabled
            // 
            this.cbxMaskEnabled.AutoSize = true;
            this.cbxMaskEnabled.Location = new System.Drawing.Point(4, 5);
            this.cbxMaskEnabled.Name = "cbxMaskEnabled";
            this.cbxMaskEnabled.Size = new System.Drawing.Size(51, 20);
            this.cbxMaskEnabled.TabIndex = 8;
            this.cbxMaskEnabled.Text = "遮罩";
            this.cbxMaskEnabled.UseVisualStyleBackColor = true;
            // 
            // nudMatchCloseHeight
            // 
            this.nudMatchCloseHeight.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.nudMatchCloseHeight.Location = new System.Drawing.Point(104, 57);
            this.nudMatchCloseHeight.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.nudMatchCloseHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMatchCloseHeight.Name = "nudMatchCloseHeight";
            this.nudMatchCloseHeight.Size = new System.Drawing.Size(53, 23);
            this.nudMatchCloseHeight.TabIndex = 79;
            this.nudMatchCloseHeight.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudMatchCloseWidth
            // 
            this.nudMatchCloseWidth.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.nudMatchCloseWidth.Location = new System.Drawing.Point(41, 57);
            this.nudMatchCloseWidth.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.nudMatchCloseWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMatchCloseWidth.Name = "nudMatchCloseWidth";
            this.nudMatchCloseWidth.Size = new System.Drawing.Size(53, 23);
            this.nudMatchCloseWidth.TabIndex = 79;
            this.nudMatchCloseWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudMatchOpenHeight
            // 
            this.nudMatchOpenHeight.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.nudMatchOpenHeight.Location = new System.Drawing.Point(104, 30);
            this.nudMatchOpenHeight.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.nudMatchOpenHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMatchOpenHeight.Name = "nudMatchOpenHeight";
            this.nudMatchOpenHeight.Size = new System.Drawing.Size(53, 23);
            this.nudMatchOpenHeight.TabIndex = 79;
            this.nudMatchOpenHeight.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudMatchOpenWidth
            // 
            this.nudMatchOpenWidth.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.nudMatchOpenWidth.Location = new System.Drawing.Point(41, 30);
            this.nudMatchOpenWidth.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.nudMatchOpenWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMatchOpenWidth.Name = "nudMatchOpenWidth";
            this.nudMatchOpenWidth.Size = new System.Drawing.Size(53, 23);
            this.nudMatchOpenWidth.TabIndex = 79;
            this.nudMatchOpenWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnMaskTH
            // 
            this.btnMaskTH.Location = new System.Drawing.Point(75, 3);
            this.btnMaskTH.Name = "btnMaskTH";
            this.btnMaskTH.Size = new System.Drawing.Size(65, 23);
            this.btnMaskTH.TabIndex = 75;
            this.btnMaskTH.Text = "二值化";
            this.btnMaskTH.UseVisualStyleBackColor = true;
            this.btnMaskTH.Click += new System.EventHandler(this.btnMaskTH_Click);
            // 
            // cbxNCCMode
            // 
            this.cbxNCCMode.AutoSize = true;
            this.cbxNCCMode.Location = new System.Drawing.Point(6, 43);
            this.cbxNCCMode.Name = "cbxNCCMode";
            this.cbxNCCMode.Size = new System.Drawing.Size(75, 20);
            this.cbxNCCMode.TabIndex = 9;
            this.cbxNCCMode.Text = "灰階對位";
            this.cbxNCCMode.UseVisualStyleBackColor = true;
            this.cbxNCCMode.CheckedChanged += new System.EventHandler(this.cbxNCCMode_CheckedChanged);
            // 
            // btnSegCellTest
            // 
            this.btnSegCellTest.BackColor = System.Drawing.Color.LightGray;
            this.btnSegCellTest.Location = new System.Drawing.Point(6, 246);
            this.btnSegCellTest.Name = "btnSegCellTest";
            this.btnSegCellTest.Size = new System.Drawing.Size(124, 32);
            this.btnSegCellTest.TabIndex = 2;
            this.btnSegCellTest.Text = "單顆範圍測試";
            this.btnSegCellTest.UseVisualStyleBackColor = false;
            this.btnSegCellTest.Click += new System.EventHandler(this.btnSegCellTest_Click);
            // 
            // labMatchID
            // 
            this.labMatchID.AutoSize = true;
            this.labMatchID.Location = new System.Drawing.Point(145, 18);
            this.labMatchID.Name = "labMatchID";
            this.labMatchID.Size = new System.Drawing.Size(48, 16);
            this.labMatchID.TabIndex = 8;
            this.labMatchID.Text = "影像ID:";
            // 
            // combxBand
            // 
            this.combxBand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combxBand.FormattingEnabled = true;
            this.combxBand.Items.AddRange(new object[] {
            "R",
            "G",
            "B",
            "Gray"});
            this.combxBand.Location = new System.Drawing.Point(300, 14);
            this.combxBand.Name = "combxBand";
            this.combxBand.Size = new System.Drawing.Size(54, 24);
            this.combxBand.TabIndex = 83;
            this.combxBand.SelectedIndexChanged += new System.EventHandler(this.combxBand_SelectedIndexChanged);
            // 
            // btnATestMatch
            // 
            this.btnATestMatch.BackColor = System.Drawing.Color.LightGray;
            this.btnATestMatch.Location = new System.Drawing.Point(6, 208);
            this.btnATestMatch.Name = "btnATestMatch";
            this.btnATestMatch.Size = new System.Drawing.Size(124, 32);
            this.btnATestMatch.TabIndex = 2;
            this.btnATestMatch.Text = "定位測試";
            this.btnATestMatch.UseVisualStyleBackColor = false;
            this.btnATestMatch.Click += new System.EventHandler(this.btnATestMatch_Click);
            // 
            // labBand
            // 
            this.labBand.AutoSize = true;
            this.labBand.Location = new System.Drawing.Point(258, 18);
            this.labBand.Name = "labBand";
            this.labBand.Size = new System.Drawing.Size(35, 16);
            this.labBand.TabIndex = 82;
            this.labBand.Text = "頻帶:";
            // 
            // cbxCellRegion
            // 
            this.cbxCellRegion.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbxCellRegion.BackColor = System.Drawing.Color.LightGray;
            this.cbxCellRegion.Location = new System.Drawing.Point(6, 170);
            this.cbxCellRegion.MaximumSize = new System.Drawing.Size(124, 32);
            this.cbxCellRegion.Name = "cbxCellRegion";
            this.cbxCellRegion.Size = new System.Drawing.Size(124, 32);
            this.cbxCellRegion.TabIndex = 1;
            this.cbxCellRegion.Text = "單顆切割範圍";
            this.cbxCellRegion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbxCellRegion.UseVisualStyleBackColor = false;
            this.cbxCellRegion.CheckedChanged += new System.EventHandler(this.cbxCellRegion_CheckedChanged);
            // 
            // nudAMinScore
            // 
            this.nudAMinScore.DecimalPlaces = 2;
            this.nudAMinScore.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudAMinScore.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudAMinScore.Location = new System.Drawing.Point(81, 64);
            this.nudAMinScore.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudAMinScore.Name = "nudAMinScore";
            this.nudAMinScore.Size = new System.Drawing.Size(53, 25);
            this.nudAMinScore.TabIndex = 4;
            this.nudAMinScore.Value = new decimal(new int[] {
            9,
            0,
            0,
            65536});
            // 
            // cbxASetPattern
            // 
            this.cbxASetPattern.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbxASetPattern.BackColor = System.Drawing.Color.LightGray;
            this.cbxASetPattern.Location = new System.Drawing.Point(6, 94);
            this.cbxASetPattern.MaximumSize = new System.Drawing.Size(9999, 9999);
            this.cbxASetPattern.Name = "cbxASetPattern";
            this.cbxASetPattern.Size = new System.Drawing.Size(124, 32);
            this.cbxASetPattern.TabIndex = 1;
            this.cbxASetPattern.Text = "圖像範圍";
            this.cbxASetPattern.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbxASetPattern.UseVisualStyleBackColor = false;
            this.cbxASetPattern.CheckedChanged += new System.EventHandler(this.cbxSetPattern_CheckedChanged);
            // 
            // labAMinScore
            // 
            this.labAMinScore.AutoSize = true;
            this.labAMinScore.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.labAMinScore.Location = new System.Drawing.Point(6, 69);
            this.labAMinScore.Name = "labAMinScore";
            this.labAMinScore.Size = new System.Drawing.Size(56, 16);
            this.labAMinScore.TabIndex = 3;
            this.labAMinScore.Text = "最小分數";
            // 
            // btnATeachPattern
            // 
            this.btnATeachPattern.BackColor = System.Drawing.Color.LightGray;
            this.btnATeachPattern.Location = new System.Drawing.Point(6, 132);
            this.btnATeachPattern.Name = "btnATeachPattern";
            this.btnATeachPattern.Size = new System.Drawing.Size(124, 32);
            this.btnATeachPattern.TabIndex = 2;
            this.btnATeachPattern.Text = "教導圖像";
            this.btnATeachPattern.UseVisualStyleBackColor = false;
            this.btnATeachPattern.Click += new System.EventHandler(this.btnTeachPattern_Click);
            // 
            // tpAlgoImage
            // 
            this.tpAlgoImage.Controls.Add(this.gbx_AlgoImage);
            this.tpAlgoImage.Controls.Add(this.gbx_AlgoImage_UsedRegion);
            this.tpAlgoImage.Controls.Add(this.btn_Compute_UsedRegion);
            this.tpAlgoImage.Location = new System.Drawing.Point(4, 25);
            this.tpAlgoImage.Name = "tpAlgoImage";
            this.tpAlgoImage.Size = new System.Drawing.Size(382, 817);
            this.tpAlgoImage.TabIndex = 9;
            this.tpAlgoImage.Text = "影像演算法";
            this.tpAlgoImage.UseVisualStyleBackColor = true;
            // 
            // gbx_AlgoImage
            // 
            this.gbx_AlgoImage.Controls.Add(this.cbx_ResultRegID_A);
            this.gbx_AlgoImage.Controls.Add(this.label101);
            this.gbx_AlgoImage.Controls.Add(this.button_SaveAsResultImgA);
            this.gbx_AlgoImage.Controls.Add(this.cbx_ResultImgID_A);
            this.gbx_AlgoImage.Controls.Add(this.label96);
            this.gbx_AlgoImage.Controls.Add(this.btn_Execute_Algo);
            this.gbx_AlgoImage.Controls.Add(this.btn_RemoveAll_Algo);
            this.gbx_AlgoImage.Controls.Add(this.btn_Remove_Algo);
            this.gbx_AlgoImage.Controls.Add(this.btn_Edit_Algo);
            this.gbx_AlgoImage.Controls.Add(this.btn_Add_Algo);
            this.gbx_AlgoImage.Controls.Add(this.listView_EditAlgo);
            this.gbx_AlgoImage.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.gbx_AlgoImage.Location = new System.Drawing.Point(8, 8);
            this.gbx_AlgoImage.Name = "gbx_AlgoImage";
            this.gbx_AlgoImage.Size = new System.Drawing.Size(340, 330);
            this.gbx_AlgoImage.TabIndex = 8;
            this.gbx_AlgoImage.TabStop = false;
            this.gbx_AlgoImage.Text = "影像區域演算法A (不使用範圍列表)";
            // 
            // cbx_ResultRegID_A
            // 
            this.cbx_ResultRegID_A.Font = new System.Drawing.Font("微軟正黑體", 11.25F);
            this.cbx_ResultRegID_A.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cbx_ResultRegID_A.FormattingEnabled = true;
            this.cbx_ResultRegID_A.Location = new System.Drawing.Point(140, 59);
            this.cbx_ResultRegID_A.Margin = new System.Windows.Forms.Padding(2);
            this.cbx_ResultRegID_A.Name = "cbx_ResultRegID_A";
            this.cbx_ResultRegID_A.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cbx_ResultRegID_A.Size = new System.Drawing.Size(47, 27);
            this.cbx_ResultRegID_A.TabIndex = 226;
            this.cbx_ResultRegID_A.SelectedIndexChanged += new System.EventHandler(this.cbx_ResultRegID_A_SelectedIndexChanged);
            // 
            // label101
            // 
            this.label101.AutoSize = true;
            this.label101.Font = new System.Drawing.Font("微軟正黑體", 11.25F);
            this.label101.Location = new System.Drawing.Point(18, 62);
            this.label101.Name = "label101";
            this.label101.Size = new System.Drawing.Size(122, 19);
            this.label101.TabIndex = 225;
            this.label101.Text = "結果區域A(編號):";
            // 
            // button_SaveAsResultImgA
            // 
            this.button_SaveAsResultImgA.Font = new System.Drawing.Font("微軟正黑體", 11.25F);
            this.button_SaveAsResultImgA.Location = new System.Drawing.Point(202, 25);
            this.button_SaveAsResultImgA.Name = "button_SaveAsResultImgA";
            this.button_SaveAsResultImgA.Size = new System.Drawing.Size(128, 28);
            this.button_SaveAsResultImgA.TabIndex = 224;
            this.button_SaveAsResultImgA.Tag = "";
            this.button_SaveAsResultImgA.Text = "另存結果影像A";
            this.button_SaveAsResultImgA.UseVisualStyleBackColor = true;
            this.button_SaveAsResultImgA.Click += new System.EventHandler(this.button_SaveAsResultImgA_Click);
            // 
            // cbx_ResultImgID_A
            // 
            this.cbx_ResultImgID_A.Font = new System.Drawing.Font("微軟正黑體", 11.25F);
            this.cbx_ResultImgID_A.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cbx_ResultImgID_A.FormattingEnabled = true;
            this.cbx_ResultImgID_A.Location = new System.Drawing.Point(140, 27);
            this.cbx_ResultImgID_A.Margin = new System.Windows.Forms.Padding(2);
            this.cbx_ResultImgID_A.Name = "cbx_ResultImgID_A";
            this.cbx_ResultImgID_A.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cbx_ResultImgID_A.Size = new System.Drawing.Size(47, 27);
            this.cbx_ResultImgID_A.TabIndex = 222;
            this.cbx_ResultImgID_A.SelectedIndexChanged += new System.EventHandler(this.cbx_ResultImgID_A_SelectedIndexChanged);
            // 
            // label96
            // 
            this.label96.AutoSize = true;
            this.label96.Font = new System.Drawing.Font("微軟正黑體", 11.25F);
            this.label96.Location = new System.Drawing.Point(18, 30);
            this.label96.Name = "label96";
            this.label96.Size = new System.Drawing.Size(122, 19);
            this.label96.TabIndex = 221;
            this.label96.Text = "結果影像A(編號):";
            // 
            // btn_Execute_Algo
            // 
            this.btn_Execute_Algo.BackColor = System.Drawing.Color.Lime;
            this.btn_Execute_Algo.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.btn_Execute_Algo.Location = new System.Drawing.Point(10, 90);
            this.btn_Execute_Algo.Name = "btn_Execute_Algo";
            this.btn_Execute_Algo.Size = new System.Drawing.Size(55, 29);
            this.btn_Execute_Algo.TabIndex = 220;
            this.btn_Execute_Algo.Tag = "";
            this.btn_Execute_Algo.Text = "執行";
            this.btn_Execute_Algo.UseVisualStyleBackColor = false;
            this.btn_Execute_Algo.Click += new System.EventHandler(this.btn_Execute_Algo_Click);
            // 
            // btn_RemoveAll_Algo
            // 
            this.btn_RemoveAll_Algo.BackColor = System.Drawing.Color.Red;
            this.btn_RemoveAll_Algo.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_RemoveAll_Algo.ForeColor = System.Drawing.Color.White;
            this.btn_RemoveAll_Algo.Location = new System.Drawing.Point(266, 90);
            this.btn_RemoveAll_Algo.Name = "btn_RemoveAll_Algo";
            this.btn_RemoveAll_Algo.Size = new System.Drawing.Size(64, 29);
            this.btn_RemoveAll_Algo.TabIndex = 219;
            this.btn_RemoveAll_Algo.Text = "清空";
            this.btn_RemoveAll_Algo.UseVisualStyleBackColor = false;
            this.btn_RemoveAll_Algo.Click += new System.EventHandler(this.btn_RemoveAll_Algo_Click);
            // 
            // btn_Remove_Algo
            // 
            this.btn_Remove_Algo.BackColor = System.Drawing.Color.Red;
            this.btn_Remove_Algo.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Remove_Algo.ForeColor = System.Drawing.Color.White;
            this.btn_Remove_Algo.Location = new System.Drawing.Point(191, 90);
            this.btn_Remove_Algo.Name = "btn_Remove_Algo";
            this.btn_Remove_Algo.Size = new System.Drawing.Size(72, 29);
            this.btn_Remove_Algo.TabIndex = 218;
            this.btn_Remove_Algo.Text = "移除";
            this.btn_Remove_Algo.UseVisualStyleBackColor = false;
            this.btn_Remove_Algo.Click += new System.EventHandler(this.btn_Remove_Algo_Click);
            // 
            // btn_Edit_Algo
            // 
            this.btn_Edit_Algo.BackColor = System.Drawing.Color.YellowGreen;
            this.btn_Edit_Algo.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Edit_Algo.ForeColor = System.Drawing.Color.Black;
            this.btn_Edit_Algo.Location = new System.Drawing.Point(130, 90);
            this.btn_Edit_Algo.Name = "btn_Edit_Algo";
            this.btn_Edit_Algo.Size = new System.Drawing.Size(55, 29);
            this.btn_Edit_Algo.TabIndex = 217;
            this.btn_Edit_Algo.Text = "編輯";
            this.btn_Edit_Algo.UseVisualStyleBackColor = false;
            this.btn_Edit_Algo.Click += new System.EventHandler(this.btn_Edit_Algo_Click);
            // 
            // btn_Add_Algo
            // 
            this.btn_Add_Algo.BackColor = System.Drawing.Color.YellowGreen;
            this.btn_Add_Algo.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Add_Algo.ForeColor = System.Drawing.Color.Black;
            this.btn_Add_Algo.Location = new System.Drawing.Point(70, 90);
            this.btn_Add_Algo.Name = "btn_Add_Algo";
            this.btn_Add_Algo.Size = new System.Drawing.Size(55, 29);
            this.btn_Add_Algo.TabIndex = 216;
            this.btn_Add_Algo.Text = "新增";
            this.btn_Add_Algo.UseVisualStyleBackColor = false;
            this.btn_Add_Algo.Click += new System.EventHandler(this.btn_Add_Algo_Click);
            // 
            // listView_EditAlgo
            // 
            this.listView_EditAlgo.BackColor = System.Drawing.Color.AliceBlue;
            this.listView_EditAlgo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listView_EditAlgo.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listView_EditAlgo.FullRowSelect = true;
            this.listView_EditAlgo.GridLines = true;
            this.listView_EditAlgo.Location = new System.Drawing.Point(10, 122);
            this.listView_EditAlgo.MultiSelect = false;
            this.listView_EditAlgo.Name = "listView_EditAlgo";
            this.listView_EditAlgo.Size = new System.Drawing.Size(320, 200);
            this.listView_EditAlgo.TabIndex = 215;
            this.listView_EditAlgo.UseCompatibleStateImageBehavior = false;
            this.listView_EditAlgo.View = System.Windows.Forms.View.Details;
            this.listView_EditAlgo.SelectedIndexChanged += new System.EventHandler(this.listView_EditAlgo_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "編號";
            this.columnHeader1.Width = 50;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "名稱";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader2.Width = 80;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "演算法";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader3.Width = 150;
            // 
            // gbx_AlgoImage_UsedRegion
            // 
            this.gbx_AlgoImage_UsedRegion.Controls.Add(this.cbx_ResultRegID_B);
            this.gbx_AlgoImage_UsedRegion.Controls.Add(this.label100);
            this.gbx_AlgoImage_UsedRegion.Controls.Add(this.button_SaveAsResultImgB);
            this.gbx_AlgoImage_UsedRegion.Controls.Add(this.cbx_ResultImgID_B);
            this.gbx_AlgoImage_UsedRegion.Controls.Add(this.label95);
            this.gbx_AlgoImage_UsedRegion.Controls.Add(this.btn_Execute_Algo_UsedRegion);
            this.gbx_AlgoImage_UsedRegion.Controls.Add(this.btn_RemoveAll_Algo_UsedRegion);
            this.gbx_AlgoImage_UsedRegion.Controls.Add(this.btn_Remove_Algo_UsedRegion);
            this.gbx_AlgoImage_UsedRegion.Controls.Add(this.btn_Edit_Algo_UsedRegion);
            this.gbx_AlgoImage_UsedRegion.Controls.Add(this.btn_Add_Algo_UsedRegion);
            this.gbx_AlgoImage_UsedRegion.Controls.Add(this.listView_EditAlgo_UsedRegion);
            this.gbx_AlgoImage_UsedRegion.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.gbx_AlgoImage_UsedRegion.Location = new System.Drawing.Point(8, 350);
            this.gbx_AlgoImage_UsedRegion.Name = "gbx_AlgoImage_UsedRegion";
            this.gbx_AlgoImage_UsedRegion.Size = new System.Drawing.Size(340, 330);
            this.gbx_AlgoImage_UsedRegion.TabIndex = 7;
            this.gbx_AlgoImage_UsedRegion.TabStop = false;
            this.gbx_AlgoImage_UsedRegion.Text = "影像區域演算法B (使用範圍列表)";
            // 
            // cbx_ResultRegID_B
            // 
            this.cbx_ResultRegID_B.Font = new System.Drawing.Font("微軟正黑體", 11.25F);
            this.cbx_ResultRegID_B.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cbx_ResultRegID_B.FormattingEnabled = true;
            this.cbx_ResultRegID_B.Location = new System.Drawing.Point(140, 59);
            this.cbx_ResultRegID_B.Margin = new System.Windows.Forms.Padding(2);
            this.cbx_ResultRegID_B.Name = "cbx_ResultRegID_B";
            this.cbx_ResultRegID_B.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cbx_ResultRegID_B.Size = new System.Drawing.Size(47, 27);
            this.cbx_ResultRegID_B.TabIndex = 225;
            this.cbx_ResultRegID_B.SelectedIndexChanged += new System.EventHandler(this.cbx_ResultRegID_B_SelectedIndexChanged);
            // 
            // label100
            // 
            this.label100.AutoSize = true;
            this.label100.Font = new System.Drawing.Font("微軟正黑體", 11.25F);
            this.label100.Location = new System.Drawing.Point(18, 62);
            this.label100.Name = "label100";
            this.label100.Size = new System.Drawing.Size(121, 19);
            this.label100.TabIndex = 224;
            this.label100.Text = "結果區域B(編號):";
            // 
            // button_SaveAsResultImgB
            // 
            this.button_SaveAsResultImgB.Font = new System.Drawing.Font("微軟正黑體", 11.25F);
            this.button_SaveAsResultImgB.Location = new System.Drawing.Point(202, 25);
            this.button_SaveAsResultImgB.Name = "button_SaveAsResultImgB";
            this.button_SaveAsResultImgB.Size = new System.Drawing.Size(128, 28);
            this.button_SaveAsResultImgB.TabIndex = 223;
            this.button_SaveAsResultImgB.Tag = "";
            this.button_SaveAsResultImgB.Text = "另存結果影像B";
            this.button_SaveAsResultImgB.UseVisualStyleBackColor = true;
            this.button_SaveAsResultImgB.Click += new System.EventHandler(this.button_SaveAsResultImgB_Click);
            // 
            // cbx_ResultImgID_B
            // 
            this.cbx_ResultImgID_B.Font = new System.Drawing.Font("微軟正黑體", 11.25F);
            this.cbx_ResultImgID_B.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cbx_ResultImgID_B.FormattingEnabled = true;
            this.cbx_ResultImgID_B.Location = new System.Drawing.Point(140, 27);
            this.cbx_ResultImgID_B.Margin = new System.Windows.Forms.Padding(2);
            this.cbx_ResultImgID_B.Name = "cbx_ResultImgID_B";
            this.cbx_ResultImgID_B.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cbx_ResultImgID_B.Size = new System.Drawing.Size(47, 27);
            this.cbx_ResultImgID_B.TabIndex = 222;
            this.cbx_ResultImgID_B.SelectedIndexChanged += new System.EventHandler(this.cbx_ResultImgID_B_SelectedIndexChanged);
            // 
            // label95
            // 
            this.label95.AutoSize = true;
            this.label95.Font = new System.Drawing.Font("微軟正黑體", 11.25F);
            this.label95.Location = new System.Drawing.Point(18, 30);
            this.label95.Name = "label95";
            this.label95.Size = new System.Drawing.Size(121, 19);
            this.label95.TabIndex = 221;
            this.label95.Text = "結果影像B(編號):";
            // 
            // btn_Execute_Algo_UsedRegion
            // 
            this.btn_Execute_Algo_UsedRegion.BackColor = System.Drawing.Color.Lime;
            this.btn_Execute_Algo_UsedRegion.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.btn_Execute_Algo_UsedRegion.Location = new System.Drawing.Point(10, 90);
            this.btn_Execute_Algo_UsedRegion.Name = "btn_Execute_Algo_UsedRegion";
            this.btn_Execute_Algo_UsedRegion.Size = new System.Drawing.Size(55, 29);
            this.btn_Execute_Algo_UsedRegion.TabIndex = 220;
            this.btn_Execute_Algo_UsedRegion.Tag = "";
            this.btn_Execute_Algo_UsedRegion.Text = "執行";
            this.btn_Execute_Algo_UsedRegion.UseVisualStyleBackColor = false;
            this.btn_Execute_Algo_UsedRegion.Click += new System.EventHandler(this.btn_Execute_Algo_UsedRegion_Click);
            // 
            // btn_RemoveAll_Algo_UsedRegion
            // 
            this.btn_RemoveAll_Algo_UsedRegion.BackColor = System.Drawing.Color.Red;
            this.btn_RemoveAll_Algo_UsedRegion.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_RemoveAll_Algo_UsedRegion.ForeColor = System.Drawing.Color.White;
            this.btn_RemoveAll_Algo_UsedRegion.Location = new System.Drawing.Point(266, 90);
            this.btn_RemoveAll_Algo_UsedRegion.Name = "btn_RemoveAll_Algo_UsedRegion";
            this.btn_RemoveAll_Algo_UsedRegion.Size = new System.Drawing.Size(64, 29);
            this.btn_RemoveAll_Algo_UsedRegion.TabIndex = 219;
            this.btn_RemoveAll_Algo_UsedRegion.Text = "清空";
            this.btn_RemoveAll_Algo_UsedRegion.UseVisualStyleBackColor = false;
            this.btn_RemoveAll_Algo_UsedRegion.Click += new System.EventHandler(this.btn_RemoveAll_Algo_UsedRegion_Click);
            // 
            // btn_Remove_Algo_UsedRegion
            // 
            this.btn_Remove_Algo_UsedRegion.BackColor = System.Drawing.Color.Red;
            this.btn_Remove_Algo_UsedRegion.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Remove_Algo_UsedRegion.ForeColor = System.Drawing.Color.White;
            this.btn_Remove_Algo_UsedRegion.Location = new System.Drawing.Point(191, 90);
            this.btn_Remove_Algo_UsedRegion.Name = "btn_Remove_Algo_UsedRegion";
            this.btn_Remove_Algo_UsedRegion.Size = new System.Drawing.Size(72, 29);
            this.btn_Remove_Algo_UsedRegion.TabIndex = 218;
            this.btn_Remove_Algo_UsedRegion.Text = "移除";
            this.btn_Remove_Algo_UsedRegion.UseVisualStyleBackColor = false;
            this.btn_Remove_Algo_UsedRegion.Click += new System.EventHandler(this.btn_Remove_Algo_UsedRegion_Click);
            // 
            // btn_Edit_Algo_UsedRegion
            // 
            this.btn_Edit_Algo_UsedRegion.BackColor = System.Drawing.Color.YellowGreen;
            this.btn_Edit_Algo_UsedRegion.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Edit_Algo_UsedRegion.ForeColor = System.Drawing.Color.Black;
            this.btn_Edit_Algo_UsedRegion.Location = new System.Drawing.Point(130, 90);
            this.btn_Edit_Algo_UsedRegion.Name = "btn_Edit_Algo_UsedRegion";
            this.btn_Edit_Algo_UsedRegion.Size = new System.Drawing.Size(55, 29);
            this.btn_Edit_Algo_UsedRegion.TabIndex = 217;
            this.btn_Edit_Algo_UsedRegion.Text = "編輯";
            this.btn_Edit_Algo_UsedRegion.UseVisualStyleBackColor = false;
            this.btn_Edit_Algo_UsedRegion.Click += new System.EventHandler(this.btn_Edit_Algo_UsedRegion_Click);
            // 
            // btn_Add_Algo_UsedRegion
            // 
            this.btn_Add_Algo_UsedRegion.BackColor = System.Drawing.Color.YellowGreen;
            this.btn_Add_Algo_UsedRegion.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Add_Algo_UsedRegion.ForeColor = System.Drawing.Color.Black;
            this.btn_Add_Algo_UsedRegion.Location = new System.Drawing.Point(70, 90);
            this.btn_Add_Algo_UsedRegion.Name = "btn_Add_Algo_UsedRegion";
            this.btn_Add_Algo_UsedRegion.Size = new System.Drawing.Size(55, 29);
            this.btn_Add_Algo_UsedRegion.TabIndex = 216;
            this.btn_Add_Algo_UsedRegion.Text = "新增";
            this.btn_Add_Algo_UsedRegion.UseVisualStyleBackColor = false;
            this.btn_Add_Algo_UsedRegion.Click += new System.EventHandler(this.btn_Add_Algo_UsedRegion_Click);
            // 
            // listView_EditAlgo_UsedRegion
            // 
            this.listView_EditAlgo_UsedRegion.BackColor = System.Drawing.Color.AliceBlue;
            this.listView_EditAlgo_UsedRegion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listView_EditAlgo_UsedRegion.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader_ID,
            this.columnHeader_Name,
            this.columnHeader_Algo});
            this.listView_EditAlgo_UsedRegion.FullRowSelect = true;
            this.listView_EditAlgo_UsedRegion.GridLines = true;
            this.listView_EditAlgo_UsedRegion.Location = new System.Drawing.Point(10, 122);
            this.listView_EditAlgo_UsedRegion.MultiSelect = false;
            this.listView_EditAlgo_UsedRegion.Name = "listView_EditAlgo_UsedRegion";
            this.listView_EditAlgo_UsedRegion.Size = new System.Drawing.Size(320, 200);
            this.listView_EditAlgo_UsedRegion.TabIndex = 215;
            this.listView_EditAlgo_UsedRegion.UseCompatibleStateImageBehavior = false;
            this.listView_EditAlgo_UsedRegion.View = System.Windows.Forms.View.Details;
            this.listView_EditAlgo_UsedRegion.SelectedIndexChanged += new System.EventHandler(this.listView_EditAlgo_UsedRegion_SelectedIndexChanged);
            // 
            // columnHeader_ID
            // 
            this.columnHeader_ID.Text = "編號";
            this.columnHeader_ID.Width = 50;
            // 
            // columnHeader_Name
            // 
            this.columnHeader_Name.Text = "名稱";
            this.columnHeader_Name.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader_Name.Width = 80;
            // 
            // columnHeader_Algo
            // 
            this.columnHeader_Algo.Text = "演算法";
            this.columnHeader_Algo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader_Algo.Width = 150;
            // 
            // btn_Compute_UsedRegion
            // 
            this.btn_Compute_UsedRegion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(166)))), ((int)(((byte)(137)))));
            this.btn_Compute_UsedRegion.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Compute_UsedRegion.ForeColor = System.Drawing.Color.White;
            this.btn_Compute_UsedRegion.Location = new System.Drawing.Point(229, 763);
            this.btn_Compute_UsedRegion.Name = "btn_Compute_UsedRegion";
            this.btn_Compute_UsedRegion.Size = new System.Drawing.Size(109, 39);
            this.btn_Compute_UsedRegion.TabIndex = 6;
            this.btn_Compute_UsedRegion.Text = "計算使用範圍";
            this.btn_Compute_UsedRegion.UseVisualStyleBackColor = false;
            this.btn_Compute_UsedRegion.Visible = false;
            this.btn_Compute_UsedRegion.Click += new System.EventHandler(this.btn_Compute_UsedRegion_Click);
            // 
            // tpRgionMethod
            // 
            this.tpRgionMethod.AutoScroll = true;
            this.tpRgionMethod.BackColor = System.Drawing.SystemColors.Control;
            this.tpRgionMethod.Controls.Add(this.gbxAdjust);
            this.tpRgionMethod.Controls.Add(this.btnAddUsed);
            this.tpRgionMethod.Controls.Add(this.gbxUsedRegion);
            this.tpRgionMethod.Controls.Add(this.gbxEditRegionList);
            this.tpRgionMethod.Controls.Add(this.gbxMethodList);
            this.tpRgionMethod.Controls.Add(this.gbxMtehodThreshold);
            this.tpRgionMethod.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.tpRgionMethod.Location = new System.Drawing.Point(4, 25);
            this.tpRgionMethod.Name = "tpRgionMethod";
            this.tpRgionMethod.Size = new System.Drawing.Size(382, 817);
            this.tpRgionMethod.TabIndex = 7;
            this.tpRgionMethod.Text = "檢測範圍設定";
            // 
            // gbxAdjust
            // 
            this.gbxAdjust.Controls.Add(this.btnAdjSet);
            this.gbxAdjust.Controls.Add(this.labAdjY);
            this.gbxAdjust.Controls.Add(this.labAdjX);
            this.gbxAdjust.Controls.Add(this.txbAdjY);
            this.gbxAdjust.Controls.Add(this.txbAdjX);
            this.gbxAdjust.Controls.Add(this.btnLeft);
            this.gbxAdjust.Controls.Add(this.btnRight);
            this.gbxAdjust.Controls.Add(this.btnDown);
            this.gbxAdjust.Controls.Add(this.btnUp);
            this.gbxAdjust.Location = new System.Drawing.Point(373, 9);
            this.gbxAdjust.Name = "gbxAdjust";
            this.gbxAdjust.Size = new System.Drawing.Size(200, 138);
            this.gbxAdjust.TabIndex = 6;
            this.gbxAdjust.TabStop = false;
            this.gbxAdjust.Text = "調整";
            this.gbxAdjust.Visible = false;
            // 
            // btnAdjSet
            // 
            this.btnAdjSet.Location = new System.Drawing.Point(134, 109);
            this.btnAdjSet.Name = "btnAdjSet";
            this.btnAdjSet.Size = new System.Drawing.Size(60, 23);
            this.btnAdjSet.TabIndex = 3;
            this.btnAdjSet.Text = "Set";
            this.btnAdjSet.UseVisualStyleBackColor = true;
            this.btnAdjSet.Click += new System.EventHandler(this.btnAdjSet_Click);
            // 
            // labAdjY
            // 
            this.labAdjY.AutoSize = true;
            this.labAdjY.Location = new System.Drawing.Point(6, 54);
            this.labAdjY.Name = "labAdjY";
            this.labAdjY.Size = new System.Drawing.Size(24, 16);
            this.labAdjY.TabIndex = 2;
            this.labAdjY.Text = "Y : ";
            // 
            // labAdjX
            // 
            this.labAdjX.AutoSize = true;
            this.labAdjX.Location = new System.Drawing.Point(6, 25);
            this.labAdjX.Name = "labAdjX";
            this.labAdjX.Size = new System.Drawing.Size(25, 16);
            this.labAdjX.TabIndex = 2;
            this.labAdjX.Text = "X : ";
            // 
            // txbAdjY
            // 
            this.txbAdjY.Location = new System.Drawing.Point(37, 51);
            this.txbAdjY.Name = "txbAdjY";
            this.txbAdjY.Size = new System.Drawing.Size(69, 23);
            this.txbAdjY.TabIndex = 1;
            this.txbAdjY.Text = "0";
            // 
            // txbAdjX
            // 
            this.txbAdjX.Location = new System.Drawing.Point(37, 22);
            this.txbAdjX.Name = "txbAdjX";
            this.txbAdjX.Size = new System.Drawing.Size(69, 23);
            this.txbAdjX.TabIndex = 1;
            this.txbAdjX.Text = "0";
            // 
            // btnLeft
            // 
            this.btnLeft.Location = new System.Drawing.Point(13, 109);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size(27, 23);
            this.btnLeft.TabIndex = 0;
            this.btnLeft.Text = "←";
            this.btnLeft.UseVisualStyleBackColor = true;
            this.btnLeft.Click += new System.EventHandler(this.btnLeft_Click);
            // 
            // btnRight
            // 
            this.btnRight.Location = new System.Drawing.Point(79, 109);
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size(27, 23);
            this.btnRight.TabIndex = 0;
            this.btnRight.Text = "→";
            this.btnRight.UseVisualStyleBackColor = true;
            this.btnRight.Click += new System.EventHandler(this.btnRight_Click);
            // 
            // btnDown
            // 
            this.btnDown.Location = new System.Drawing.Point(46, 109);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(27, 23);
            this.btnDown.TabIndex = 0;
            this.btnDown.Text = "↓";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnUp
            // 
            this.btnUp.Location = new System.Drawing.Point(46, 84);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(27, 23);
            this.btnUp.TabIndex = 0;
            this.btnUp.Text = "↑";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnAddUsed
            // 
            this.btnAddUsed.BackColor = System.Drawing.SystemColors.Control;
            this.btnAddUsed.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAddUsed.Image = ((System.Drawing.Image)(resources.GetObject("btnAddUsed.Image")));
            this.btnAddUsed.Location = new System.Drawing.Point(141, 545);
            this.btnAddUsed.Name = "btnAddUsed";
            this.btnAddUsed.Size = new System.Drawing.Size(39, 28);
            this.btnAddUsed.TabIndex = 5;
            this.btnAddUsed.UseVisualStyleBackColor = false;
            this.btnAddUsed.Click += new System.EventHandler(this.btnAddUsed_Click);
            // 
            // gbxUsedRegion
            // 
            this.gbxUsedRegion.Controls.Add(this.btnClearUsedRegion);
            this.gbxUsedRegion.Controls.Add(this.btnExportRegion);
            this.gbxUsedRegion.Controls.Add(this.btnRemoveUseRegion);
            this.gbxUsedRegion.Controls.Add(this.listViewUsedRegion);
            this.gbxUsedRegion.Location = new System.Drawing.Point(6, 575);
            this.gbxUsedRegion.Name = "gbxUsedRegion";
            this.gbxUsedRegion.Size = new System.Drawing.Size(324, 234);
            this.gbxUsedRegion.TabIndex = 1;
            this.gbxUsedRegion.TabStop = false;
            this.gbxUsedRegion.Text = "使用範圍列表";
            // 
            // btnClearUsedRegion
            // 
            this.btnClearUsedRegion.BackColor = System.Drawing.Color.Red;
            this.btnClearUsedRegion.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClearUsedRegion.Location = new System.Drawing.Point(248, 201);
            this.btnClearUsedRegion.Name = "btnClearUsedRegion";
            this.btnClearUsedRegion.Size = new System.Drawing.Size(67, 23);
            this.btnClearUsedRegion.TabIndex = 4;
            this.btnClearUsedRegion.Text = "Clear";
            this.btnClearUsedRegion.UseVisualStyleBackColor = false;
            this.btnClearUsedRegion.Click += new System.EventHandler(this.btnClearUsedRegion_Click);
            // 
            // btnExportRegion
            // 
            this.btnExportRegion.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnExportRegion.Location = new System.Drawing.Point(6, 201);
            this.btnExportRegion.Name = "btnExportRegion";
            this.btnExportRegion.Size = new System.Drawing.Size(67, 23);
            this.btnExportRegion.TabIndex = 4;
            this.btnExportRegion.Text = "匯出";
            this.btnExportRegion.UseVisualStyleBackColor = true;
            this.btnExportRegion.Click += new System.EventHandler(this.btnExportRegion_Click);
            // 
            // btnRemoveUseRegion
            // 
            this.btnRemoveUseRegion.BackColor = System.Drawing.Color.Red;
            this.btnRemoveUseRegion.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRemoveUseRegion.Location = new System.Drawing.Point(121, 201);
            this.btnRemoveUseRegion.Name = "btnRemoveUseRegion";
            this.btnRemoveUseRegion.Size = new System.Drawing.Size(67, 23);
            this.btnRemoveUseRegion.TabIndex = 3;
            this.btnRemoveUseRegion.Text = "移除";
            this.btnRemoveUseRegion.UseVisualStyleBackColor = false;
            this.btnRemoveUseRegion.Click += new System.EventHandler(this.btnRemoveUseRegion_Click);
            // 
            // listViewUsedRegion
            // 
            this.listViewUsedRegion.BackColor = System.Drawing.Color.AliceBlue;
            this.listViewUsedRegion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listViewUsedRegion.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderUseRegion,
            this.columnHeaderUseDisplay,
            this.columnHeaderUseImageId,
            this.columnHeaderUseColor});
            this.listViewUsedRegion.FullRowSelect = true;
            this.listViewUsedRegion.GridLines = true;
            this.listViewUsedRegion.Location = new System.Drawing.Point(10, 22);
            this.listViewUsedRegion.MultiSelect = false;
            this.listViewUsedRegion.Name = "listViewUsedRegion";
            this.listViewUsedRegion.Size = new System.Drawing.Size(305, 173);
            this.listViewUsedRegion.TabIndex = 0;
            this.listViewUsedRegion.UseCompatibleStateImageBehavior = false;
            this.listViewUsedRegion.View = System.Windows.Forms.View.Details;
            this.listViewUsedRegion.SelectedIndexChanged += new System.EventHandler(this.listViewUsedRegion_SelectedIndexChanged);
            this.listViewUsedRegion.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.listUsedRegion_KeyPress);
            // 
            // columnHeaderUseRegion
            // 
            this.columnHeaderUseRegion.Text = "使用範圍";
            this.columnHeaderUseRegion.Width = 120;
            // 
            // columnHeaderUseDisplay
            // 
            this.columnHeaderUseDisplay.Text = "顯示";
            // 
            // columnHeaderUseImageId
            // 
            this.columnHeaderUseImageId.Text = "影像ID";
            this.columnHeaderUseImageId.Width = 50;
            // 
            // columnHeaderUseColor
            // 
            this.columnHeaderUseColor.Text = "顏色";
            this.columnHeaderUseColor.Width = 70;
            // 
            // gbxEditRegionList
            // 
            this.gbxEditRegionList.Controls.Add(this.btnEditRegionRemoveAll);
            this.gbxEditRegionList.Controls.Add(this.btnOpenEditRegionForm);
            this.gbxEditRegionList.Controls.Add(this.btnEditRegionRemove);
            this.gbxEditRegionList.Controls.Add(this.listViewEditRegion);
            this.gbxEditRegionList.Location = new System.Drawing.Point(6, 359);
            this.gbxEditRegionList.Name = "gbxEditRegionList";
            this.gbxEditRegionList.Size = new System.Drawing.Size(324, 180);
            this.gbxEditRegionList.TabIndex = 1;
            this.gbxEditRegionList.TabStop = false;
            this.gbxEditRegionList.Text = "編輯範圍列表";
            // 
            // btnEditRegionRemoveAll
            // 
            this.btnEditRegionRemoveAll.BackColor = System.Drawing.Color.Red;
            this.btnEditRegionRemoveAll.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnEditRegionRemoveAll.Location = new System.Drawing.Point(248, 148);
            this.btnEditRegionRemoveAll.Name = "btnEditRegionRemoveAll";
            this.btnEditRegionRemoveAll.Size = new System.Drawing.Size(67, 23);
            this.btnEditRegionRemoveAll.TabIndex = 4;
            this.btnEditRegionRemoveAll.Text = "清空";
            this.btnEditRegionRemoveAll.UseVisualStyleBackColor = false;
            this.btnEditRegionRemoveAll.Click += new System.EventHandler(this.btnEditRegionRemoveAll_Click);
            // 
            // btnOpenEditRegionForm
            // 
            this.btnOpenEditRegionForm.BackColor = System.Drawing.Color.Yellow;
            this.btnOpenEditRegionForm.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnOpenEditRegionForm.Location = new System.Drawing.Point(6, 148);
            this.btnOpenEditRegionForm.Name = "btnOpenEditRegionForm";
            this.btnOpenEditRegionForm.Size = new System.Drawing.Size(67, 23);
            this.btnOpenEditRegionForm.TabIndex = 3;
            this.btnOpenEditRegionForm.Text = "編輯";
            this.btnOpenEditRegionForm.UseVisualStyleBackColor = false;
            this.btnOpenEditRegionForm.Click += new System.EventHandler(this.btnOpenEditRegionForm_Click);
            // 
            // btnEditRegionRemove
            // 
            this.btnEditRegionRemove.BackColor = System.Drawing.Color.Red;
            this.btnEditRegionRemove.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnEditRegionRemove.Location = new System.Drawing.Point(121, 148);
            this.btnEditRegionRemove.Name = "btnEditRegionRemove";
            this.btnEditRegionRemove.Size = new System.Drawing.Size(67, 23);
            this.btnEditRegionRemove.TabIndex = 3;
            this.btnEditRegionRemove.Text = "移除";
            this.btnEditRegionRemove.UseVisualStyleBackColor = false;
            this.btnEditRegionRemove.Click += new System.EventHandler(this.btnEditRegionRemove_Click);
            // 
            // listViewEditRegion
            // 
            this.listViewEditRegion.BackColor = System.Drawing.Color.AliceBlue;
            this.listViewEditRegion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listViewEditRegion.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderEditRegion});
            this.listViewEditRegion.FullRowSelect = true;
            this.listViewEditRegion.GridLines = true;
            this.listViewEditRegion.Location = new System.Drawing.Point(6, 22);
            this.listViewEditRegion.MultiSelect = false;
            this.listViewEditRegion.Name = "listViewEditRegion";
            this.listViewEditRegion.Size = new System.Drawing.Size(309, 120);
            this.listViewEditRegion.TabIndex = 0;
            this.listViewEditRegion.UseCompatibleStateImageBehavior = false;
            this.listViewEditRegion.View = System.Windows.Forms.View.Details;
            this.listViewEditRegion.SelectedIndexChanged += new System.EventHandler(this.listViewEditRegion_SelectedIndexChanged);
            this.listViewEditRegion.Click += new System.EventHandler(this.listViewEditRegion_SelectedIndexChanged);
            // 
            // columnHeaderEditRegion
            // 
            this.columnHeaderEditRegion.Text = "編輯範圍";
            this.columnHeaderEditRegion.Width = 290;
            // 
            // gbxMethodList
            // 
            this.gbxMethodList.Controls.Add(this.btnClearMethodRegion);
            this.gbxMethodList.Controls.Add(this.btnRemoveMethodRegion);
            this.gbxMethodList.Controls.Add(this.listViewMethod);
            this.gbxMethodList.Location = new System.Drawing.Point(6, 184);
            this.gbxMethodList.Name = "gbxMethodList";
            this.gbxMethodList.Size = new System.Drawing.Size(324, 169);
            this.gbxMethodList.TabIndex = 1;
            this.gbxMethodList.TabStop = false;
            this.gbxMethodList.Text = "對位範圍列表";
            // 
            // btnClearMethodRegion
            // 
            this.btnClearMethodRegion.BackColor = System.Drawing.Color.Red;
            this.btnClearMethodRegion.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClearMethodRegion.Location = new System.Drawing.Point(248, 139);
            this.btnClearMethodRegion.Name = "btnClearMethodRegion";
            this.btnClearMethodRegion.Size = new System.Drawing.Size(67, 23);
            this.btnClearMethodRegion.TabIndex = 4;
            this.btnClearMethodRegion.Text = "清空";
            this.btnClearMethodRegion.UseVisualStyleBackColor = false;
            this.btnClearMethodRegion.Click += new System.EventHandler(this.btnClearMethodRegion_Click);
            // 
            // btnRemoveMethodRegion
            // 
            this.btnRemoveMethodRegion.BackColor = System.Drawing.Color.Red;
            this.btnRemoveMethodRegion.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRemoveMethodRegion.Location = new System.Drawing.Point(6, 139);
            this.btnRemoveMethodRegion.Name = "btnRemoveMethodRegion";
            this.btnRemoveMethodRegion.Size = new System.Drawing.Size(67, 23);
            this.btnRemoveMethodRegion.TabIndex = 3;
            this.btnRemoveMethodRegion.Text = "移除";
            this.btnRemoveMethodRegion.UseVisualStyleBackColor = false;
            this.btnRemoveMethodRegion.Click += new System.EventHandler(this.btnRemoveMethodRegion_Click);
            // 
            // listViewMethod
            // 
            this.listViewMethod.BackColor = System.Drawing.Color.AliceBlue;
            this.listViewMethod.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listViewMethod.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderMethodRegion});
            this.listViewMethod.FullRowSelect = true;
            this.listViewMethod.GridLines = true;
            this.listViewMethod.Location = new System.Drawing.Point(6, 22);
            this.listViewMethod.MultiSelect = false;
            this.listViewMethod.Name = "listViewMethod";
            this.listViewMethod.Size = new System.Drawing.Size(309, 111);
            this.listViewMethod.TabIndex = 0;
            this.listViewMethod.UseCompatibleStateImageBehavior = false;
            this.listViewMethod.View = System.Windows.Forms.View.Details;
            this.listViewMethod.SelectedIndexChanged += new System.EventHandler(this.listViewMethod_SelectedIndexChanged);
            this.listViewMethod.Click += new System.EventHandler(this.listViewMethod_SelectedIndexChanged);
            // 
            // columnHeaderMethodRegion
            // 
            this.columnHeaderMethodRegion.Text = "對位範圍";
            this.columnHeaderMethodRegion.Width = 290;
            // 
            // gbxMtehodThreshold
            // 
            this.gbxMtehodThreshold.Controls.Add(this.nudMethodThImageID);
            this.gbxMtehodThreshold.Controls.Add(this.radioButton_OrigImg_RegMethod);
            this.gbxMtehodThreshold.Controls.Add(this.radioButton_ImgA_RegMethod);
            this.gbxMtehodThreshold.Controls.Add(this.labMethodThImageID);
            this.gbxMtehodThreshold.Controls.Add(this.labMethThBand);
            this.gbxMtehodThreshold.Controls.Add(this.tbThMethod);
            this.gbxMtehodThreshold.Controls.Add(this.combxMethodThBand);
            this.gbxMtehodThreshold.Location = new System.Drawing.Point(6, 3);
            this.gbxMtehodThreshold.Name = "gbxMtehodThreshold";
            this.gbxMtehodThreshold.Size = new System.Drawing.Size(365, 175);
            this.gbxMtehodThreshold.TabIndex = 0;
            this.gbxMtehodThreshold.TabStop = false;
            this.gbxMtehodThreshold.Text = "方法";
            // 
            // nudMethodThImageID
            // 
            this.nudMethodThImageID.Font = new System.Drawing.Font("Verdana", 9.75F);
            this.nudMethodThImageID.Location = new System.Drawing.Point(205, 15);
            this.nudMethodThImageID.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.nudMethodThImageID.Name = "nudMethodThImageID";
            this.nudMethodThImageID.Size = new System.Drawing.Size(47, 23);
            this.nudMethodThImageID.TabIndex = 111;
            // 
            // radioButton_OrigImg_RegMethod
            // 
            this.radioButton_OrigImg_RegMethod.AutoSize = true;
            this.radioButton_OrigImg_RegMethod.Checked = true;
            this.radioButton_OrigImg_RegMethod.Font = new System.Drawing.Font("微軟正黑體", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.radioButton_OrigImg_RegMethod.Location = new System.Drawing.Point(5, 16);
            this.radioButton_OrigImg_RegMethod.Name = "radioButton_OrigImg_RegMethod";
            this.radioButton_OrigImg_RegMethod.Size = new System.Drawing.Size(47, 19);
            this.radioButton_OrigImg_RegMethod.TabIndex = 87;
            this.radioButton_OrigImg_RegMethod.TabStop = true;
            this.radioButton_OrigImg_RegMethod.Tag = "原始";
            this.radioButton_OrigImg_RegMethod.Text = "原始";
            this.radioButton_OrigImg_RegMethod.UseVisualStyleBackColor = true;
            this.radioButton_OrigImg_RegMethod.CheckedChanged += new System.EventHandler(this.radioButton_Img_RegMethod_CheckedChanged);
            // 
            // radioButton_ImgA_RegMethod
            // 
            this.radioButton_ImgA_RegMethod.AutoSize = true;
            this.radioButton_ImgA_RegMethod.Font = new System.Drawing.Font("微軟正黑體", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.radioButton_ImgA_RegMethod.Location = new System.Drawing.Point(75, 16);
            this.radioButton_ImgA_RegMethod.Name = "radioButton_ImgA_RegMethod";
            this.radioButton_ImgA_RegMethod.Size = new System.Drawing.Size(55, 19);
            this.radioButton_ImgA_RegMethod.TabIndex = 88;
            this.radioButton_ImgA_RegMethod.TabStop = true;
            this.radioButton_ImgA_RegMethod.Tag = "影像A";
            this.radioButton_ImgA_RegMethod.Text = "影像A";
            this.radioButton_ImgA_RegMethod.UseVisualStyleBackColor = true;
            this.radioButton_ImgA_RegMethod.CheckedChanged += new System.EventHandler(this.radioButton_Img_RegMethod_CheckedChanged);
            // 
            // labMethodThImageID
            // 
            this.labMethodThImageID.AutoSize = true;
            this.labMethodThImageID.Location = new System.Drawing.Point(145, 18);
            this.labMethodThImageID.Name = "labMethodThImageID";
            this.labMethodThImageID.Size = new System.Drawing.Size(48, 16);
            this.labMethodThImageID.TabIndex = 86;
            this.labMethodThImageID.Text = "影像ID:";
            // 
            // labMethThBand
            // 
            this.labMethThBand.AutoSize = true;
            this.labMethThBand.Location = new System.Drawing.Point(258, 18);
            this.labMethThBand.Name = "labMethThBand";
            this.labMethThBand.Size = new System.Drawing.Size(35, 16);
            this.labMethThBand.TabIndex = 86;
            this.labMethThBand.Text = "頻帶:";
            // 
            // tbThMethod
            // 
            this.tbThMethod.Controls.Add(this.tpSingleTh);
            this.tbThMethod.Controls.Add(this.tpDualTh);
            this.tbThMethod.Controls.Add(this.tpAutoTh);
            this.tbThMethod.Controls.Add(this.tpDynTh);
            this.tbThMethod.Controls.Add(this.tpUserSet);
            this.tbThMethod.Location = new System.Drawing.Point(6, 43);
            this.tbThMethod.Name = "tbThMethod";
            this.tbThMethod.SelectedIndex = 0;
            this.tbThMethod.Size = new System.Drawing.Size(313, 122);
            this.tbThMethod.TabIndex = 4;
            // 
            // tpSingleTh
            // 
            this.tpSingleTh.BackColor = System.Drawing.SystemColors.Control;
            this.tpSingleTh.Controls.Add(this.label72);
            this.tpSingleTh.Controls.Add(this.btnMethodThAdd);
            this.tpSingleTh.Controls.Add(this.label73);
            this.tpSingleTh.Controls.Add(this.btnMethodThSet);
            this.tpSingleTh.Controls.Add(this.nudMethodThMin);
            this.tpSingleTh.Controls.Add(this.nudMethodThMax);
            this.tpSingleTh.Controls.Add(this.btnMethodThTest);
            this.tpSingleTh.Location = new System.Drawing.Point(4, 25);
            this.tpSingleTh.Name = "tpSingleTh";
            this.tpSingleTh.Padding = new System.Windows.Forms.Padding(3);
            this.tpSingleTh.Size = new System.Drawing.Size(305, 93);
            this.tpSingleTh.TabIndex = 0;
            this.tpSingleTh.Text = "二值化";
            // 
            // label72
            // 
            this.label72.AutoSize = true;
            this.label72.Location = new System.Drawing.Point(10, 18);
            this.label72.Name = "label72";
            this.label72.Size = new System.Drawing.Size(41, 16);
            this.label72.TabIndex = 86;
            this.label72.Text = "閥值 : ";
            // 
            // btnMethodThAdd
            // 
            this.btnMethodThAdd.BackColor = System.Drawing.Color.Aqua;
            this.btnMethodThAdd.Location = new System.Drawing.Point(198, 53);
            this.btnMethodThAdd.Name = "btnMethodThAdd";
            this.btnMethodThAdd.Size = new System.Drawing.Size(75, 23);
            this.btnMethodThAdd.TabIndex = 102;
            this.btnMethodThAdd.Text = "新增";
            this.btnMethodThAdd.UseVisualStyleBackColor = false;
            this.btnMethodThAdd.Click += new System.EventHandler(this.btnMethodThAdd_Click);
            // 
            // label73
            // 
            this.label73.AutoSize = true;
            this.label73.Location = new System.Drawing.Point(141, 18);
            this.label73.Name = "label73";
            this.label73.Size = new System.Drawing.Size(20, 16);
            this.label73.TabIndex = 100;
            this.label73.Text = "～";
            // 
            // btnMethodThSet
            // 
            this.btnMethodThSet.Location = new System.Drawing.Point(226, 15);
            this.btnMethodThSet.Name = "btnMethodThSet";
            this.btnMethodThSet.Size = new System.Drawing.Size(47, 23);
            this.btnMethodThSet.TabIndex = 101;
            this.btnMethodThSet.Text = "設定";
            this.btnMethodThSet.UseVisualStyleBackColor = true;
            this.btnMethodThSet.Click += new System.EventHandler(this.btnMethodThSet_Click);
            // 
            // nudMethodThMin
            // 
            this.nudMethodThMin.Location = new System.Drawing.Point(80, 15);
            this.nudMethodThMin.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudMethodThMin.Name = "nudMethodThMin";
            this.nudMethodThMin.Size = new System.Drawing.Size(55, 23);
            this.nudMethodThMin.TabIndex = 87;
            this.nudMethodThMin.Value = new decimal(new int[] {
            128,
            0,
            0,
            0});
            // 
            // nudMethodThMax
            // 
            this.nudMethodThMax.Location = new System.Drawing.Point(165, 15);
            this.nudMethodThMax.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudMethodThMax.Name = "nudMethodThMax";
            this.nudMethodThMax.Size = new System.Drawing.Size(55, 23);
            this.nudMethodThMax.TabIndex = 87;
            this.nudMethodThMax.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            // 
            // btnMethodThTest
            // 
            this.btnMethodThTest.Location = new System.Drawing.Point(60, 53);
            this.btnMethodThTest.Name = "btnMethodThTest";
            this.btnMethodThTest.Size = new System.Drawing.Size(75, 23);
            this.btnMethodThTest.TabIndex = 102;
            this.btnMethodThTest.Text = "測試";
            this.btnMethodThTest.UseVisualStyleBackColor = true;
            this.btnMethodThTest.Click += new System.EventHandler(this.btnMethodThTest_Click);
            // 
            // tpDualTh
            // 
            this.tpDualTh.BackColor = System.Drawing.SystemColors.Control;
            this.tpDualTh.Controls.Add(this.label77);
            this.tpDualTh.Controls.Add(this.label76);
            this.tpDualTh.Controls.Add(this.label75);
            this.tpDualTh.Controls.Add(this.btnDualThAdd);
            this.tpDualTh.Controls.Add(this.nudDualThThreshold);
            this.tpDualTh.Controls.Add(this.nudDualThMinSize);
            this.tpDualTh.Controls.Add(this.nudDualThMinGray);
            this.tpDualTh.Controls.Add(this.btnDualThTest);
            this.tpDualTh.Location = new System.Drawing.Point(4, 25);
            this.tpDualTh.Name = "tpDualTh";
            this.tpDualTh.Padding = new System.Windows.Forms.Padding(3);
            this.tpDualTh.Size = new System.Drawing.Size(305, 93);
            this.tpDualTh.TabIndex = 1;
            this.tpDualTh.Text = "Dual 二值化";
            // 
            // label77
            // 
            this.label77.AutoSize = true;
            this.label77.Location = new System.Drawing.Point(17, 66);
            this.label77.Name = "label77";
            this.label77.Size = new System.Drawing.Size(41, 16);
            this.label77.TabIndex = 103;
            this.label77.Text = "閥值 : ";
            // 
            // label76
            // 
            this.label76.AutoSize = true;
            this.label76.Location = new System.Drawing.Point(17, 38);
            this.label76.Name = "label76";
            this.label76.Size = new System.Drawing.Size(65, 16);
            this.label76.TabIndex = 103;
            this.label76.Text = "最小灰階 : ";
            // 
            // label75
            // 
            this.label75.AutoSize = true;
            this.label75.Location = new System.Drawing.Point(17, 8);
            this.label75.Name = "label75";
            this.label75.Size = new System.Drawing.Size(65, 16);
            this.label75.TabIndex = 103;
            this.label75.Text = "最小大小 : ";
            // 
            // btnDualThAdd
            // 
            this.btnDualThAdd.BackColor = System.Drawing.Color.Aqua;
            this.btnDualThAdd.Location = new System.Drawing.Point(197, 64);
            this.btnDualThAdd.Name = "btnDualThAdd";
            this.btnDualThAdd.Size = new System.Drawing.Size(75, 23);
            this.btnDualThAdd.TabIndex = 107;
            this.btnDualThAdd.Text = "新增";
            this.btnDualThAdd.UseVisualStyleBackColor = false;
            this.btnDualThAdd.Click += new System.EventHandler(this.btnDualThAdd_Click);
            // 
            // nudDualThThreshold
            // 
            this.nudDualThThreshold.Location = new System.Drawing.Point(109, 64);
            this.nudDualThThreshold.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudDualThThreshold.Name = "nudDualThThreshold";
            this.nudDualThThreshold.Size = new System.Drawing.Size(55, 23);
            this.nudDualThThreshold.TabIndex = 105;
            this.nudDualThThreshold.Value = new decimal(new int[] {
            95,
            0,
            0,
            0});
            // 
            // nudDualThMinSize
            // 
            this.nudDualThMinSize.Location = new System.Drawing.Point(109, 6);
            this.nudDualThMinSize.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudDualThMinSize.Name = "nudDualThMinSize";
            this.nudDualThMinSize.Size = new System.Drawing.Size(55, 23);
            this.nudDualThMinSize.TabIndex = 104;
            this.nudDualThMinSize.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // nudDualThMinGray
            // 
            this.nudDualThMinGray.Location = new System.Drawing.Point(109, 35);
            this.nudDualThMinGray.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudDualThMinGray.Name = "nudDualThMinGray";
            this.nudDualThMinGray.Size = new System.Drawing.Size(55, 23);
            this.nudDualThMinGray.TabIndex = 105;
            this.nudDualThMinGray.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // btnDualThTest
            // 
            this.btnDualThTest.Location = new System.Drawing.Point(197, 8);
            this.btnDualThTest.Name = "btnDualThTest";
            this.btnDualThTest.Size = new System.Drawing.Size(75, 23);
            this.btnDualThTest.TabIndex = 108;
            this.btnDualThTest.Text = "測試";
            this.btnDualThTest.UseVisualStyleBackColor = true;
            this.btnDualThTest.Click += new System.EventHandler(this.btnDualThTest_Click);
            // 
            // tpAutoTh
            // 
            this.tpAutoTh.BackColor = System.Drawing.SystemColors.Control;
            this.tpAutoTh.Controls.Add(this.btnAutoThAdd);
            this.tpAutoTh.Controls.Add(this.button2);
            this.tpAutoTh.Controls.Add(this.comboBoxAutoThSelect);
            this.tpAutoTh.Controls.Add(this.label79);
            this.tpAutoTh.Controls.Add(this.nudAutoThSigma);
            this.tpAutoTh.Controls.Add(this.label78);
            this.tpAutoTh.Location = new System.Drawing.Point(4, 25);
            this.tpAutoTh.Name = "tpAutoTh";
            this.tpAutoTh.Size = new System.Drawing.Size(305, 93);
            this.tpAutoTh.TabIndex = 2;
            this.tpAutoTh.Text = "自動二值化";
            // 
            // btnAutoThAdd
            // 
            this.btnAutoThAdd.BackColor = System.Drawing.Color.Aqua;
            this.btnAutoThAdd.Location = new System.Drawing.Point(201, 56);
            this.btnAutoThAdd.Name = "btnAutoThAdd";
            this.btnAutoThAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAutoThAdd.TabIndex = 110;
            this.btnAutoThAdd.Text = "新增";
            this.btnAutoThAdd.UseVisualStyleBackColor = false;
            this.btnAutoThAdd.Click += new System.EventHandler(this.btnAutoThAdd_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(201, 23);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 109;
            this.button2.Text = "測試";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // comboBoxAutoThSelect
            // 
            this.comboBoxAutoThSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAutoThSelect.FormattingEnabled = true;
            this.comboBoxAutoThSelect.Location = new System.Drawing.Point(95, 51);
            this.comboBoxAutoThSelect.Name = "comboBoxAutoThSelect";
            this.comboBoxAutoThSelect.Size = new System.Drawing.Size(69, 24);
            this.comboBoxAutoThSelect.TabIndex = 3;
            this.comboBoxAutoThSelect.SelectedIndexChanged += new System.EventHandler(this.comboBoxAutoThSelect_SelectedIndexChanged);
            // 
            // label79
            // 
            this.label79.AutoSize = true;
            this.label79.Location = new System.Drawing.Point(26, 56);
            this.label79.Name = "label79";
            this.label79.Size = new System.Drawing.Size(65, 16);
            this.label79.TabIndex = 2;
            this.label79.Text = "選取範圍 : ";
            // 
            // nudAutoThSigma
            // 
            this.nudAutoThSigma.DecimalPlaces = 1;
            this.nudAutoThSigma.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.nudAutoThSigma.Location = new System.Drawing.Point(85, 18);
            this.nudAutoThSigma.Name = "nudAutoThSigma";
            this.nudAutoThSigma.Size = new System.Drawing.Size(53, 23);
            this.nudAutoThSigma.TabIndex = 1;
            this.nudAutoThSigma.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label78
            // 
            this.label78.AutoSize = true;
            this.label78.Location = new System.Drawing.Point(26, 20);
            this.label78.Name = "label78";
            this.label78.Size = new System.Drawing.Size(53, 16);
            this.label78.TabIndex = 0;
            this.label78.Text = "Sigma : ";
            // 
            // tpDynTh
            // 
            this.tpDynTh.BackColor = System.Drawing.SystemColors.Control;
            this.tpDynTh.Controls.Add(this.comboBoxDynType);
            this.tpDynTh.Controls.Add(this.btnDynThAdd);
            this.tpDynTh.Controls.Add(this.labOffset);
            this.tpDynTh.Controls.Add(this.labDynMeanImage);
            this.tpDynTh.Controls.Add(this.nudMeanHeight);
            this.tpDynTh.Controls.Add(this.nudDynOffset);
            this.tpDynTh.Controls.Add(this.nudMeanWidth);
            this.tpDynTh.Controls.Add(this.btnDynTest);
            this.tpDynTh.Location = new System.Drawing.Point(4, 25);
            this.tpDynTh.Name = "tpDynTh";
            this.tpDynTh.Size = new System.Drawing.Size(305, 93);
            this.tpDynTh.TabIndex = 3;
            this.tpDynTh.Text = "動態二值化";
            // 
            // comboBoxDynType
            // 
            this.comboBoxDynType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDynType.FormattingEnabled = true;
            this.comboBoxDynType.Items.AddRange(new object[] {
            "dark",
            "light",
            "equal",
            "not_equal"});
            this.comboBoxDynType.Location = new System.Drawing.Point(15, 66);
            this.comboBoxDynType.Name = "comboBoxDynType";
            this.comboBoxDynType.Size = new System.Drawing.Size(121, 24);
            this.comboBoxDynType.TabIndex = 129;
            // 
            // btnDynThAdd
            // 
            this.btnDynThAdd.BackColor = System.Drawing.Color.Aqua;
            this.btnDynThAdd.Location = new System.Drawing.Point(213, 49);
            this.btnDynThAdd.Name = "btnDynThAdd";
            this.btnDynThAdd.Size = new System.Drawing.Size(75, 23);
            this.btnDynThAdd.TabIndex = 128;
            this.btnDynThAdd.Text = "新增";
            this.btnDynThAdd.UseVisualStyleBackColor = false;
            this.btnDynThAdd.Click += new System.EventHandler(this.btnDynThAdd_Click);
            // 
            // labOffset
            // 
            this.labOffset.AutoSize = true;
            this.labOffset.Location = new System.Drawing.Point(11, 39);
            this.labOffset.Name = "labOffset";
            this.labOffset.Size = new System.Drawing.Size(41, 16);
            this.labOffset.TabIndex = 126;
            this.labOffset.Text = "平移 : ";
            // 
            // labDynMeanImage
            // 
            this.labDynMeanImage.AutoSize = true;
            this.labDynMeanImage.Location = new System.Drawing.Point(12, 14);
            this.labDynMeanImage.Name = "labDynMeanImage";
            this.labDynMeanImage.Size = new System.Drawing.Size(65, 16);
            this.labDynMeanImage.TabIndex = 127;
            this.labDynMeanImage.Text = "影像平均 : ";
            // 
            // nudMeanHeight
            // 
            this.nudMeanHeight.Location = new System.Drawing.Point(140, 8);
            this.nudMeanHeight.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudMeanHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMeanHeight.Name = "nudMeanHeight";
            this.nudMeanHeight.Size = new System.Drawing.Size(51, 23);
            this.nudMeanHeight.TabIndex = 123;
            this.nudMeanHeight.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // nudDynOffset
            // 
            this.nudDynOffset.Location = new System.Drawing.Point(83, 37);
            this.nudDynOffset.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudDynOffset.Minimum = new decimal(new int[] {
            255,
            0,
            0,
            -2147483648});
            this.nudDynOffset.Name = "nudDynOffset";
            this.nudDynOffset.Size = new System.Drawing.Size(51, 23);
            this.nudDynOffset.TabIndex = 124;
            this.nudDynOffset.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // nudMeanWidth
            // 
            this.nudMeanWidth.Location = new System.Drawing.Point(83, 8);
            this.nudMeanWidth.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudMeanWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMeanWidth.Name = "nudMeanWidth";
            this.nudMeanWidth.Size = new System.Drawing.Size(51, 23);
            this.nudMeanWidth.TabIndex = 125;
            this.nudMeanWidth.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // btnDynTest
            // 
            this.btnDynTest.Location = new System.Drawing.Point(213, 7);
            this.btnDynTest.Name = "btnDynTest";
            this.btnDynTest.Size = new System.Drawing.Size(75, 23);
            this.btnDynTest.TabIndex = 122;
            this.btnDynTest.Text = "測試";
            this.btnDynTest.UseVisualStyleBackColor = true;
            this.btnDynTest.Click += new System.EventHandler(this.btnDynTest_Click);
            // 
            // tpUserSet
            // 
            this.tpUserSet.BackColor = System.Drawing.SystemColors.Control;
            this.tpUserSet.Controls.Add(this.cbxCustomizeAdd);
            this.tpUserSet.Controls.Add(this.label74);
            this.tpUserSet.Controls.Add(this.combxCustomizeType);
            this.tpUserSet.Location = new System.Drawing.Point(4, 25);
            this.tpUserSet.Name = "tpUserSet";
            this.tpUserSet.Size = new System.Drawing.Size(305, 93);
            this.tpUserSet.TabIndex = 4;
            this.tpUserSet.Text = "自訂範圍";
            // 
            // cbxCustomizeAdd
            // 
            this.cbxCustomizeAdd.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbxCustomizeAdd.BackColor = System.Drawing.Color.Aqua;
            this.cbxCustomizeAdd.Location = new System.Drawing.Point(227, 25);
            this.cbxCustomizeAdd.Name = "cbxCustomizeAdd";
            this.cbxCustomizeAdd.Size = new System.Drawing.Size(72, 24);
            this.cbxCustomizeAdd.TabIndex = 86;
            this.cbxCustomizeAdd.Text = "新增";
            this.cbxCustomizeAdd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbxCustomizeAdd.UseVisualStyleBackColor = false;
            this.cbxCustomizeAdd.CheckedChanged += new System.EventHandler(this.cbxCustomizeAdd_CheckedChanged);
            // 
            // label74
            // 
            this.label74.AutoSize = true;
            this.label74.Location = new System.Drawing.Point(5, 28);
            this.label74.Name = "label74";
            this.label74.Size = new System.Drawing.Size(46, 16);
            this.label74.TabIndex = 84;
            this.label74.Text = "Type : ";
            // 
            // combxCustomizeType
            // 
            this.combxCustomizeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combxCustomizeType.FormattingEnabled = true;
            this.combxCustomizeType.Items.AddRange(new object[] {
            "RECTANGLE1",
            "RECTANGLE2",
            "CIRCLE",
            "ELLIPSE"});
            this.combxCustomizeType.Location = new System.Drawing.Point(52, 25);
            this.combxCustomizeType.Name = "combxCustomizeType";
            this.combxCustomizeType.Size = new System.Drawing.Size(169, 24);
            this.combxCustomizeType.TabIndex = 85;
            // 
            // combxMethodThBand
            // 
            this.combxMethodThBand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combxMethodThBand.FormattingEnabled = true;
            this.combxMethodThBand.Items.AddRange(new object[] {
            "R",
            "G",
            "B",
            "Gray"});
            this.combxMethodThBand.Location = new System.Drawing.Point(300, 14);
            this.combxMethodThBand.Name = "combxMethodThBand";
            this.combxMethodThBand.Size = new System.Drawing.Size(54, 24);
            this.combxMethodThBand.TabIndex = 85;
            this.combxMethodThBand.SelectedIndexChanged += new System.EventHandler(this.combxMethodThBand_SelectedIndexChanged);
            // 
            // labAnisometry
            // 
            this.labAnisometry.BackColor = System.Drawing.SystemColors.Control;
            this.labAnisometry.Controls.Add(this.radioButton_ImgB_AOI);
            this.labAnisometry.Controls.Add(this.radioButton_OrigImg_AOI);
            this.labAnisometry.Controls.Add(this.radioButton_ImgA_AOI);
            this.labAnisometry.Controls.Add(this.btnImportParam);
            this.labAnisometry.Controls.Add(this.nudPriority);
            this.labAnisometry.Controls.Add(this.label84);
            this.labAnisometry.Controls.Add(this.gbxRegionFeature);
            this.labAnisometry.Controls.Add(this.btnMethTest);
            this.labAnisometry.Controls.Add(this.labInspImgIndex);
            this.labAnisometry.Controls.Add(this.nudInspImageIndex);
            this.labAnisometry.Controls.Add(this.comboBoxRegionSelect);
            this.labAnisometry.Controls.Add(this.labRegionIndex);
            this.labAnisometry.Controls.Add(this.txbAlgorithmName);
            this.labAnisometry.Controls.Add(this.btnAddAlgorithm);
            this.labAnisometry.Controls.Add(this.gbxAlgorithmList);
            this.labAnisometry.Controls.Add(this.labAlgorithmSelect);
            this.labAnisometry.Controls.Add(this.comboBoxAlgorithmSelect);
            this.labAnisometry.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.labAnisometry.Location = new System.Drawing.Point(4, 25);
            this.labAnisometry.Name = "labAnisometry";
            this.labAnisometry.Size = new System.Drawing.Size(382, 817);
            this.labAnisometry.TabIndex = 8;
            this.labAnisometry.Text = "演算法參數設定";
            // 
            // radioButton_ImgB_AOI
            // 
            this.radioButton_ImgB_AOI.AutoSize = true;
            this.radioButton_ImgB_AOI.Font = new System.Drawing.Font("微軟正黑體", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.radioButton_ImgB_AOI.Location = new System.Drawing.Point(160, 63);
            this.radioButton_ImgB_AOI.Name = "radioButton_ImgB_AOI";
            this.radioButton_ImgB_AOI.Size = new System.Drawing.Size(54, 19);
            this.radioButton_ImgB_AOI.TabIndex = 130;
            this.radioButton_ImgB_AOI.TabStop = true;
            this.radioButton_ImgB_AOI.Tag = "影像B";
            this.radioButton_ImgB_AOI.Text = "影像B";
            this.radioButton_ImgB_AOI.UseVisualStyleBackColor = true;
            this.radioButton_ImgB_AOI.CheckedChanged += new System.EventHandler(this.radioButton_Img_AOI_CheckedChanged);
            // 
            // radioButton_OrigImg_AOI
            // 
            this.radioButton_OrigImg_AOI.AutoSize = true;
            this.radioButton_OrigImg_AOI.Checked = true;
            this.radioButton_OrigImg_AOI.Font = new System.Drawing.Font("微軟正黑體", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.radioButton_OrigImg_AOI.Location = new System.Drawing.Point(20, 63);
            this.radioButton_OrigImg_AOI.Name = "radioButton_OrigImg_AOI";
            this.radioButton_OrigImg_AOI.Size = new System.Drawing.Size(47, 19);
            this.radioButton_OrigImg_AOI.TabIndex = 128;
            this.radioButton_OrigImg_AOI.TabStop = true;
            this.radioButton_OrigImg_AOI.Tag = "原始";
            this.radioButton_OrigImg_AOI.Text = "原始";
            this.radioButton_OrigImg_AOI.UseVisualStyleBackColor = true;
            this.radioButton_OrigImg_AOI.CheckedChanged += new System.EventHandler(this.radioButton_Img_AOI_CheckedChanged);
            // 
            // radioButton_ImgA_AOI
            // 
            this.radioButton_ImgA_AOI.AutoSize = true;
            this.radioButton_ImgA_AOI.Font = new System.Drawing.Font("微軟正黑體", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.radioButton_ImgA_AOI.Location = new System.Drawing.Point(90, 63);
            this.radioButton_ImgA_AOI.Name = "radioButton_ImgA_AOI";
            this.radioButton_ImgA_AOI.Size = new System.Drawing.Size(55, 19);
            this.radioButton_ImgA_AOI.TabIndex = 129;
            this.radioButton_ImgA_AOI.TabStop = true;
            this.radioButton_ImgA_AOI.Tag = "影像A";
            this.radioButton_ImgA_AOI.Text = "影像A";
            this.radioButton_ImgA_AOI.UseVisualStyleBackColor = true;
            this.radioButton_ImgA_AOI.CheckedChanged += new System.EventHandler(this.radioButton_Img_AOI_CheckedChanged);
            // 
            // btnImportParam
            // 
            this.btnImportParam.Location = new System.Drawing.Point(145, 389);
            this.btnImportParam.Name = "btnImportParam";
            this.btnImportParam.Size = new System.Drawing.Size(75, 23);
            this.btnImportParam.TabIndex = 127;
            this.btnImportParam.Text = "批量匯入";
            this.btnImportParam.UseVisualStyleBackColor = true;
            this.btnImportParam.Click += new System.EventHandler(this.btnImportParam_Click);
            // 
            // nudPriority
            // 
            this.nudPriority.Location = new System.Drawing.Point(296, 6);
            this.nudPriority.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nudPriority.Name = "nudPriority";
            this.nudPriority.Size = new System.Drawing.Size(47, 23);
            this.nudPriority.TabIndex = 126;
            // 
            // label84
            // 
            this.label84.AutoSize = true;
            this.label84.Location = new System.Drawing.Point(235, 9);
            this.label84.Name = "label84";
            this.label84.Size = new System.Drawing.Size(47, 16);
            this.label84.TabIndex = 125;
            this.label84.Text = "優先權:";
            // 
            // gbxRegionFeature
            // 
            this.gbxRegionFeature.Controls.Add(this.gbxSurfaceFeature);
            this.gbxRegionFeature.Controls.Add(this.gbxInfo);
            this.gbxRegionFeature.Controls.Add(this.cbxTestROI);
            this.gbxRegionFeature.Controls.Add(this.gbxHoleGrayFeature);
            this.gbxRegionFeature.Location = new System.Drawing.Point(18, 418);
            this.gbxRegionFeature.Name = "gbxRegionFeature";
            this.gbxRegionFeature.Size = new System.Drawing.Size(328, 396);
            this.gbxRegionFeature.TabIndex = 123;
            this.gbxRegionFeature.TabStop = false;
            this.gbxRegionFeature.Text = "特徵";
            // 
            // gbxSurfaceFeature
            // 
            this.gbxSurfaceFeature.Controls.Add(this.label94);
            this.gbxSurfaceFeature.Controls.Add(this.labanisometryL);
            this.gbxSurfaceFeature.Controls.Add(this.labinner_radius);
            this.gbxSurfaceFeature.Controls.Add(this.labholes_num);
            this.gbxSurfaceFeature.Controls.Add(this.label89);
            this.gbxSurfaceFeature.Controls.Add(this.labroundness);
            this.gbxSurfaceFeature.Controls.Add(this.label91);
            this.gbxSurfaceFeature.Controls.Add(this.label93);
            this.gbxSurfaceFeature.Controls.Add(this.label87);
            this.gbxSurfaceFeature.Controls.Add(this.label83);
            this.gbxSurfaceFeature.Controls.Add(this.labrectangularity);
            this.gbxSurfaceFeature.Controls.Add(this.labcircularity);
            this.gbxSurfaceFeature.Controls.Add(this.label85);
            this.gbxSurfaceFeature.Controls.Add(this.labconvexity);
            this.gbxSurfaceFeature.Location = new System.Drawing.Point(21, 186);
            this.gbxSurfaceFeature.Name = "gbxSurfaceFeature";
            this.gbxSurfaceFeature.Size = new System.Drawing.Size(300, 178);
            this.gbxSurfaceFeature.TabIndex = 123;
            this.gbxSurfaceFeature.TabStop = false;
            this.gbxSurfaceFeature.Text = "形狀特徵";
            // 
            // label94
            // 
            this.label94.AutoSize = true;
            this.label94.Location = new System.Drawing.Point(244, 25);
            this.label94.Name = "label94";
            this.label94.Size = new System.Drawing.Size(15, 16);
            this.label94.TabIndex = 2;
            this.label94.Text = "0";
            // 
            // labanisometryL
            // 
            this.labanisometryL.AutoSize = true;
            this.labanisometryL.Location = new System.Drawing.Point(160, 25);
            this.labanisometryL.Name = "labanisometryL";
            this.labanisometryL.Size = new System.Drawing.Size(77, 16);
            this.labanisometryL.TabIndex = 1;
            this.labanisometryL.Text = "Anisometry";
            // 
            // labinner_radius
            // 
            this.labinner_radius.AutoSize = true;
            this.labinner_radius.Location = new System.Drawing.Point(104, 157);
            this.labinner_radius.Name = "labinner_radius";
            this.labinner_radius.Size = new System.Drawing.Size(15, 16);
            this.labinner_radius.TabIndex = 0;
            this.labinner_radius.Text = "0";
            // 
            // labholes_num
            // 
            this.labholes_num.AutoSize = true;
            this.labholes_num.Location = new System.Drawing.Point(104, 132);
            this.labholes_num.Name = "labholes_num";
            this.labholes_num.Size = new System.Drawing.Size(15, 16);
            this.labholes_num.TabIndex = 0;
            this.labholes_num.Text = "0";
            // 
            // label89
            // 
            this.label89.AutoSize = true;
            this.label89.Location = new System.Drawing.Point(6, 25);
            this.label89.Name = "label89";
            this.label89.Size = new System.Drawing.Size(66, 16);
            this.label89.TabIndex = 0;
            this.label89.Text = "circularity";
            // 
            // labroundness
            // 
            this.labroundness.AutoSize = true;
            this.labroundness.Location = new System.Drawing.Point(104, 50);
            this.labroundness.Name = "labroundness";
            this.labroundness.Size = new System.Drawing.Size(15, 16);
            this.labroundness.TabIndex = 0;
            this.labroundness.Text = "0";
            // 
            // label91
            // 
            this.label91.AutoSize = true;
            this.label91.Location = new System.Drawing.Point(6, 50);
            this.label91.Name = "label91";
            this.label91.Size = new System.Drawing.Size(72, 16);
            this.label91.TabIndex = 0;
            this.label91.Text = "roundness";
            // 
            // label93
            // 
            this.label93.AutoSize = true;
            this.label93.Location = new System.Drawing.Point(6, 157);
            this.label93.Name = "label93";
            this.label93.Size = new System.Drawing.Size(81, 16);
            this.label93.TabIndex = 0;
            this.label93.Text = "inner_radius";
            // 
            // label87
            // 
            this.label87.AutoSize = true;
            this.label87.Location = new System.Drawing.Point(6, 132);
            this.label87.Name = "label87";
            this.label87.Size = new System.Drawing.Size(72, 16);
            this.label87.TabIndex = 0;
            this.label87.Text = "holes_num";
            // 
            // label83
            // 
            this.label83.AutoSize = true;
            this.label83.Location = new System.Drawing.Point(6, 80);
            this.label83.Name = "label83";
            this.label83.Size = new System.Drawing.Size(68, 16);
            this.label83.TabIndex = 0;
            this.label83.Text = "convexity:";
            // 
            // labrectangularity
            // 
            this.labrectangularity.AutoSize = true;
            this.labrectangularity.Location = new System.Drawing.Point(104, 106);
            this.labrectangularity.Name = "labrectangularity";
            this.labrectangularity.Size = new System.Drawing.Size(15, 16);
            this.labrectangularity.TabIndex = 0;
            this.labrectangularity.Text = "0";
            // 
            // labcircularity
            // 
            this.labcircularity.AutoSize = true;
            this.labcircularity.Location = new System.Drawing.Point(104, 25);
            this.labcircularity.Name = "labcircularity";
            this.labcircularity.Size = new System.Drawing.Size(15, 16);
            this.labcircularity.TabIndex = 0;
            this.labcircularity.Text = "0";
            // 
            // label85
            // 
            this.label85.AutoSize = true;
            this.label85.Location = new System.Drawing.Point(6, 106);
            this.label85.Name = "label85";
            this.label85.Size = new System.Drawing.Size(92, 16);
            this.label85.TabIndex = 0;
            this.label85.Text = "rectangularity";
            // 
            // labconvexity
            // 
            this.labconvexity.AutoSize = true;
            this.labconvexity.Location = new System.Drawing.Point(104, 80);
            this.labconvexity.Name = "labconvexity";
            this.labconvexity.Size = new System.Drawing.Size(15, 16);
            this.labconvexity.TabIndex = 0;
            this.labconvexity.Text = "0";
            // 
            // gbxInfo
            // 
            this.gbxInfo.Controls.Add(this.labImageIDDisplay);
            this.gbxInfo.Controls.Add(this.labID1);
            this.gbxInfo.Controls.Add(this.labID0);
            this.gbxInfo.Controls.Add(this.label81);
            this.gbxInfo.Controls.Add(this.label80);
            this.gbxInfo.Controls.Add(this.labSecGrayMax);
            this.gbxInfo.Controls.Add(this.labInfoGrayMax);
            this.gbxInfo.Controls.Add(this.labSecGrayMin);
            this.gbxInfo.Controls.Add(this.labInfoGrayMin);
            this.gbxInfo.Controls.Add(this.labSecGray);
            this.gbxInfo.Controls.Add(this.labGrayMean);
            this.gbxInfo.Controls.Add(this.labGray);
            this.gbxInfo.Controls.Add(this.labA);
            this.gbxInfo.Controls.Add(this.labH);
            this.gbxInfo.Controls.Add(this.labW);
            this.gbxInfo.Controls.Add(this.labArea);
            this.gbxInfo.Controls.Add(this.labHeight);
            this.gbxInfo.Controls.Add(this.labWidth);
            this.gbxInfo.Location = new System.Drawing.Point(21, 22);
            this.gbxInfo.Name = "gbxInfo";
            this.gbxInfo.Size = new System.Drawing.Size(181, 158);
            this.gbxInfo.TabIndex = 118;
            this.gbxInfo.TabStop = false;
            this.gbxInfo.Text = "特徵資訊(um)";
            // 
            // labImageIDDisplay
            // 
            this.labImageIDDisplay.AutoSize = true;
            this.labImageIDDisplay.Location = new System.Drawing.Point(6, 83);
            this.labImageIDDisplay.Name = "labImageIDDisplay";
            this.labImageIDDisplay.Size = new System.Drawing.Size(53, 16);
            this.labImageIDDisplay.TabIndex = 6;
            this.labImageIDDisplay.Text = "ImgID : ";
            // 
            // labID1
            // 
            this.labID1.AutoSize = true;
            this.labID1.Location = new System.Drawing.Point(127, 83);
            this.labID1.Name = "labID1";
            this.labID1.Size = new System.Drawing.Size(15, 16);
            this.labID1.TabIndex = 5;
            this.labID1.Text = "0";
            // 
            // labID0
            // 
            this.labID0.AutoSize = true;
            this.labID0.Location = new System.Drawing.Point(70, 83);
            this.labID0.Name = "labID0";
            this.labID0.Size = new System.Drawing.Size(15, 16);
            this.labID0.TabIndex = 5;
            this.labID0.Text = "0";
            // 
            // label81
            // 
            this.label81.AutoSize = true;
            this.label81.Location = new System.Drawing.Point(6, 133);
            this.label81.Name = "label81";
            this.label81.Size = new System.Drawing.Size(59, 16);
            this.label81.TabIndex = 4;
            this.label81.Text = "灰階最大:";
            // 
            // label80
            // 
            this.label80.AutoSize = true;
            this.label80.Location = new System.Drawing.Point(6, 116);
            this.label80.Name = "label80";
            this.label80.Size = new System.Drawing.Size(59, 16);
            this.label80.TabIndex = 4;
            this.label80.Text = "灰階最小:";
            // 
            // labSecGrayMax
            // 
            this.labSecGrayMax.AutoSize = true;
            this.labSecGrayMax.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.labSecGrayMax.Location = new System.Drawing.Point(127, 133);
            this.labSecGrayMax.Name = "labSecGrayMax";
            this.labSecGrayMax.Size = new System.Drawing.Size(15, 16);
            this.labSecGrayMax.TabIndex = 3;
            this.labSecGrayMax.Text = "0";
            // 
            // labInfoGrayMax
            // 
            this.labInfoGrayMax.AutoSize = true;
            this.labInfoGrayMax.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.labInfoGrayMax.Location = new System.Drawing.Point(71, 133);
            this.labInfoGrayMax.Name = "labInfoGrayMax";
            this.labInfoGrayMax.Size = new System.Drawing.Size(15, 16);
            this.labInfoGrayMax.TabIndex = 3;
            this.labInfoGrayMax.Text = "0";
            // 
            // labSecGrayMin
            // 
            this.labSecGrayMin.AutoSize = true;
            this.labSecGrayMin.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.labSecGrayMin.Location = new System.Drawing.Point(127, 116);
            this.labSecGrayMin.Name = "labSecGrayMin";
            this.labSecGrayMin.Size = new System.Drawing.Size(15, 16);
            this.labSecGrayMin.TabIndex = 3;
            this.labSecGrayMin.Text = "0";
            // 
            // labInfoGrayMin
            // 
            this.labInfoGrayMin.AutoSize = true;
            this.labInfoGrayMin.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.labInfoGrayMin.Location = new System.Drawing.Point(71, 116);
            this.labInfoGrayMin.Name = "labInfoGrayMin";
            this.labInfoGrayMin.Size = new System.Drawing.Size(15, 16);
            this.labInfoGrayMin.TabIndex = 3;
            this.labInfoGrayMin.Text = "0";
            // 
            // labSecGray
            // 
            this.labSecGray.AutoSize = true;
            this.labSecGray.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.labSecGray.Location = new System.Drawing.Point(127, 100);
            this.labSecGray.Name = "labSecGray";
            this.labSecGray.Size = new System.Drawing.Size(15, 16);
            this.labSecGray.TabIndex = 3;
            this.labSecGray.Text = "0";
            // 
            // labGrayMean
            // 
            this.labGrayMean.AutoSize = true;
            this.labGrayMean.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.labGrayMean.Location = new System.Drawing.Point(71, 100);
            this.labGrayMean.Name = "labGrayMean";
            this.labGrayMean.Size = new System.Drawing.Size(15, 16);
            this.labGrayMean.TabIndex = 3;
            this.labGrayMean.Text = "0";
            // 
            // labGray
            // 
            this.labGray.AutoSize = true;
            this.labGray.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.labGray.Location = new System.Drawing.Point(6, 100);
            this.labGray.Name = "labGray";
            this.labGray.Size = new System.Drawing.Size(41, 16);
            this.labGray.TabIndex = 2;
            this.labGray.Text = "灰階 : ";
            // 
            // labA
            // 
            this.labA.AutoSize = true;
            this.labA.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.labA.Location = new System.Drawing.Point(48, 53);
            this.labA.Name = "labA";
            this.labA.Size = new System.Drawing.Size(15, 16);
            this.labA.TabIndex = 1;
            this.labA.Text = "0";
            // 
            // labH
            // 
            this.labH.AutoSize = true;
            this.labH.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.labH.Location = new System.Drawing.Point(48, 36);
            this.labH.Name = "labH";
            this.labH.Size = new System.Drawing.Size(15, 16);
            this.labH.TabIndex = 1;
            this.labH.Text = "0";
            // 
            // labW
            // 
            this.labW.AutoSize = true;
            this.labW.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.labW.Location = new System.Drawing.Point(48, 18);
            this.labW.Name = "labW";
            this.labW.Size = new System.Drawing.Size(15, 16);
            this.labW.TabIndex = 1;
            this.labW.Text = "0";
            // 
            // labArea
            // 
            this.labArea.AutoSize = true;
            this.labArea.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.labArea.Location = new System.Drawing.Point(6, 53);
            this.labArea.Name = "labArea";
            this.labArea.Size = new System.Drawing.Size(35, 16);
            this.labArea.TabIndex = 0;
            this.labArea.Text = "面積:";
            // 
            // labHeight
            // 
            this.labHeight.AutoSize = true;
            this.labHeight.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.labHeight.Location = new System.Drawing.Point(6, 36);
            this.labHeight.Name = "labHeight";
            this.labHeight.Size = new System.Drawing.Size(23, 16);
            this.labHeight.TabIndex = 0;
            this.labHeight.Text = "高:";
            // 
            // labWidth
            // 
            this.labWidth.AutoSize = true;
            this.labWidth.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.labWidth.Location = new System.Drawing.Point(6, 18);
            this.labWidth.Name = "labWidth";
            this.labWidth.Size = new System.Drawing.Size(23, 16);
            this.labWidth.TabIndex = 0;
            this.labWidth.Text = "寬:";
            // 
            // cbxTestROI
            // 
            this.cbxTestROI.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbxTestROI.BackColor = System.Drawing.Color.LightGray;
            this.cbxTestROI.Location = new System.Drawing.Point(126, 367);
            this.cbxTestROI.MaximumSize = new System.Drawing.Size(9999, 9999);
            this.cbxTestROI.Name = "cbxTestROI";
            this.cbxTestROI.Size = new System.Drawing.Size(84, 23);
            this.cbxTestROI.TabIndex = 120;
            this.cbxTestROI.Text = "區域測試";
            this.cbxTestROI.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbxTestROI.UseVisualStyleBackColor = false;
            this.cbxTestROI.CheckedChanged += new System.EventHandler(this.cbxTestROI_CheckedChanged);
            // 
            // gbxHoleGrayFeature
            // 
            this.gbxHoleGrayFeature.Controls.Add(this.labDarkRatioValue);
            this.gbxHoleGrayFeature.Controls.Add(this.labGrayMinValue);
            this.gbxHoleGrayFeature.Controls.Add(this.labMeanValue);
            this.gbxHoleGrayFeature.Controls.Add(this.labRadiusValue);
            this.gbxHoleGrayFeature.Controls.Add(this.labRatio);
            this.gbxHoleGrayFeature.Controls.Add(this.labGrayMin);
            this.gbxHoleGrayFeature.Controls.Add(this.labMeanGray);
            this.gbxHoleGrayFeature.Controls.Add(this.labRadius);
            this.gbxHoleGrayFeature.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.gbxHoleGrayFeature.Location = new System.Drawing.Point(216, 22);
            this.gbxHoleGrayFeature.Name = "gbxHoleGrayFeature";
            this.gbxHoleGrayFeature.Size = new System.Drawing.Size(105, 158);
            this.gbxHoleGrayFeature.TabIndex = 122;
            this.gbxHoleGrayFeature.TabStop = false;
            this.gbxHoleGrayFeature.Text = "孔洞特徵";
            // 
            // labDarkRatioValue
            // 
            this.labDarkRatioValue.AutoSize = true;
            this.labDarkRatioValue.Location = new System.Drawing.Point(67, 109);
            this.labDarkRatioValue.Name = "labDarkRatioValue";
            this.labDarkRatioValue.Size = new System.Drawing.Size(15, 16);
            this.labDarkRatioValue.TabIndex = 1;
            this.labDarkRatioValue.Text = "0";
            // 
            // labGrayMinValue
            // 
            this.labGrayMinValue.AutoSize = true;
            this.labGrayMinValue.Location = new System.Drawing.Point(67, 83);
            this.labGrayMinValue.Name = "labGrayMinValue";
            this.labGrayMinValue.Size = new System.Drawing.Size(15, 16);
            this.labGrayMinValue.TabIndex = 1;
            this.labGrayMinValue.Text = "0";
            // 
            // labMeanValue
            // 
            this.labMeanValue.AutoSize = true;
            this.labMeanValue.Location = new System.Drawing.Point(66, 55);
            this.labMeanValue.Name = "labMeanValue";
            this.labMeanValue.Size = new System.Drawing.Size(15, 16);
            this.labMeanValue.TabIndex = 1;
            this.labMeanValue.Text = "0";
            // 
            // labRadiusValue
            // 
            this.labRadiusValue.AutoSize = true;
            this.labRadiusValue.Location = new System.Drawing.Point(66, 25);
            this.labRadiusValue.Name = "labRadiusValue";
            this.labRadiusValue.Size = new System.Drawing.Size(15, 16);
            this.labRadiusValue.TabIndex = 1;
            this.labRadiusValue.Text = "0";
            // 
            // labRatio
            // 
            this.labRatio.AutoSize = true;
            this.labRatio.Location = new System.Drawing.Point(7, 109);
            this.labRatio.Name = "labRatio";
            this.labRatio.Size = new System.Drawing.Size(59, 16);
            this.labRatio.TabIndex = 0;
            this.labRatio.Text = "黑白比例:";
            // 
            // labGrayMin
            // 
            this.labGrayMin.AutoSize = true;
            this.labGrayMin.Location = new System.Drawing.Point(7, 83);
            this.labGrayMin.Name = "labGrayMin";
            this.labGrayMin.Size = new System.Drawing.Size(59, 16);
            this.labGrayMin.TabIndex = 0;
            this.labGrayMin.Text = "最小灰階:";
            // 
            // labMeanGray
            // 
            this.labMeanGray.AutoSize = true;
            this.labMeanGray.Location = new System.Drawing.Point(7, 55);
            this.labMeanGray.Name = "labMeanGray";
            this.labMeanGray.Size = new System.Drawing.Size(35, 16);
            this.labMeanGray.TabIndex = 0;
            this.labMeanGray.Text = "平均:";
            // 
            // labRadius
            // 
            this.labRadius.AutoSize = true;
            this.labRadius.Location = new System.Drawing.Point(7, 25);
            this.labRadius.Name = "labRadius";
            this.labRadius.Size = new System.Drawing.Size(35, 16);
            this.labRadius.TabIndex = 0;
            this.labRadius.Text = "直徑:";
            // 
            // btnMethTest
            // 
            this.btnMethTest.BackColor = System.Drawing.Color.MediumBlue;
            this.btnMethTest.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnMethTest.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnMethTest.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnMethTest.Location = new System.Drawing.Point(124, 337);
            this.btnMethTest.Name = "btnMethTest";
            this.btnMethTest.Size = new System.Drawing.Size(116, 41);
            this.btnMethTest.TabIndex = 104;
            this.btnMethTest.Text = "單項檢測";
            this.btnMethTest.UseVisualStyleBackColor = false;
            this.btnMethTest.Click += new System.EventHandler(this.btnMethTest_Click);
            // 
            // labInspImgIndex
            // 
            this.labInspImgIndex.AutoSize = true;
            this.labInspImgIndex.Location = new System.Drawing.Point(230, 64);
            this.labInspImgIndex.Name = "labInspImgIndex";
            this.labInspImgIndex.Size = new System.Drawing.Size(48, 16);
            this.labInspImgIndex.TabIndex = 109;
            this.labInspImgIndex.Text = "影像ID:";
            // 
            // nudInspImageIndex
            // 
            this.nudInspImageIndex.Font = new System.Drawing.Font("Verdana", 9.75F);
            this.nudInspImageIndex.Location = new System.Drawing.Point(290, 61);
            this.nudInspImageIndex.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.nudInspImageIndex.Name = "nudInspImageIndex";
            this.nudInspImageIndex.Size = new System.Drawing.Size(47, 23);
            this.nudInspImageIndex.TabIndex = 110;
            // 
            // comboBoxRegionSelect
            // 
            this.comboBoxRegionSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRegionSelect.FormattingEnabled = true;
            this.comboBoxRegionSelect.Location = new System.Drawing.Point(99, 34);
            this.comboBoxRegionSelect.Name = "comboBoxRegionSelect";
            this.comboBoxRegionSelect.Size = new System.Drawing.Size(171, 24);
            this.comboBoxRegionSelect.TabIndex = 108;
            this.comboBoxRegionSelect.SelectedIndexChanged += new System.EventHandler(this.comboBoxRegionSelect_SelectedIndexChanged);
            // 
            // labRegionIndex
            // 
            this.labRegionIndex.AutoSize = true;
            this.labRegionIndex.Location = new System.Drawing.Point(15, 38);
            this.labRegionIndex.Name = "labRegionIndex";
            this.labRegionIndex.Size = new System.Drawing.Size(65, 16);
            this.labRegionIndex.TabIndex = 107;
            this.labRegionIndex.Text = "使用範圍 : ";
            // 
            // txbAlgorithmName
            // 
            this.txbAlgorithmName.Location = new System.Drawing.Point(18, 89);
            this.txbAlgorithmName.Name = "txbAlgorithmName";
            this.txbAlgorithmName.Size = new System.Drawing.Size(210, 23);
            this.txbAlgorithmName.TabIndex = 106;
            // 
            // btnAddAlgorithm
            // 
            this.btnAddAlgorithm.BackColor = System.Drawing.Color.Aqua;
            this.btnAddAlgorithm.Location = new System.Drawing.Point(234, 90);
            this.btnAddAlgorithm.Name = "btnAddAlgorithm";
            this.btnAddAlgorithm.Size = new System.Drawing.Size(58, 23);
            this.btnAddAlgorithm.TabIndex = 103;
            this.btnAddAlgorithm.Text = "新增";
            this.btnAddAlgorithm.UseVisualStyleBackColor = false;
            this.btnAddAlgorithm.Click += new System.EventHandler(this.btnAddAlgorithm_Click);
            // 
            // gbxAlgorithmList
            // 
            this.gbxAlgorithmList.Controls.Add(this.btnAlgorithmClear);
            this.gbxAlgorithmList.Controls.Add(this.btnAlgorithmSetup);
            this.gbxAlgorithmList.Controls.Add(this.btnUpdateDefectName);
            this.gbxAlgorithmList.Controls.Add(this.btnAlgorithmRemove);
            this.gbxAlgorithmList.Controls.Add(this.listViewAlgorithm);
            this.gbxAlgorithmList.Location = new System.Drawing.Point(18, 118);
            this.gbxAlgorithmList.Name = "gbxAlgorithmList";
            this.gbxAlgorithmList.Size = new System.Drawing.Size(328, 213);
            this.gbxAlgorithmList.TabIndex = 3;
            this.gbxAlgorithmList.TabStop = false;
            this.gbxAlgorithmList.Text = "演算法列表";
            // 
            // btnAlgorithmClear
            // 
            this.btnAlgorithmClear.BackColor = System.Drawing.Color.Red;
            this.btnAlgorithmClear.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAlgorithmClear.Location = new System.Drawing.Point(254, 178);
            this.btnAlgorithmClear.Name = "btnAlgorithmClear";
            this.btnAlgorithmClear.Size = new System.Drawing.Size(67, 23);
            this.btnAlgorithmClear.TabIndex = 4;
            this.btnAlgorithmClear.Text = "Clear";
            this.btnAlgorithmClear.UseVisualStyleBackColor = false;
            this.btnAlgorithmClear.Click += new System.EventHandler(this.btnAlgorithmClear_Click);
            // 
            // btnAlgorithmSetup
            // 
            this.btnAlgorithmSetup.BackColor = System.Drawing.Color.Yellow;
            this.btnAlgorithmSetup.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAlgorithmSetup.Location = new System.Drawing.Point(6, 178);
            this.btnAlgorithmSetup.Name = "btnAlgorithmSetup";
            this.btnAlgorithmSetup.Size = new System.Drawing.Size(67, 23);
            this.btnAlgorithmSetup.TabIndex = 3;
            this.btnAlgorithmSetup.Text = "參數設定";
            this.btnAlgorithmSetup.UseVisualStyleBackColor = false;
            this.btnAlgorithmSetup.Click += new System.EventHandler(this.btnAlgorithmSetup_Click);
            // 
            // btnUpdateDefectName
            // 
            this.btnUpdateDefectName.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnUpdateDefectName.Location = new System.Drawing.Point(88, 178);
            this.btnUpdateDefectName.Name = "btnUpdateDefectName";
            this.btnUpdateDefectName.Size = new System.Drawing.Size(67, 23);
            this.btnUpdateDefectName.TabIndex = 124;
            this.btnUpdateDefectName.Text = "重整名稱";
            this.btnUpdateDefectName.UseVisualStyleBackColor = true;
            this.btnUpdateDefectName.Click += new System.EventHandler(this.btnUpdateDefectName_Click);
            // 
            // btnAlgorithmRemove
            // 
            this.btnAlgorithmRemove.BackColor = System.Drawing.Color.Red;
            this.btnAlgorithmRemove.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAlgorithmRemove.Location = new System.Drawing.Point(171, 178);
            this.btnAlgorithmRemove.Name = "btnAlgorithmRemove";
            this.btnAlgorithmRemove.Size = new System.Drawing.Size(67, 23);
            this.btnAlgorithmRemove.TabIndex = 3;
            this.btnAlgorithmRemove.Text = "移除";
            this.btnAlgorithmRemove.UseVisualStyleBackColor = false;
            this.btnAlgorithmRemove.Click += new System.EventHandler(this.btnAlgorithmRemove_Click);
            // 
            // listViewAlgorithm
            // 
            this.listViewAlgorithm.BackColor = System.Drawing.Color.AliceBlue;
            this.listViewAlgorithm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listViewAlgorithm.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderAlgorithm,
            this.columnHeaderPriority,
            this.columnHeaderColor});
            this.listViewAlgorithm.FullRowSelect = true;
            this.listViewAlgorithm.GridLines = true;
            this.listViewAlgorithm.Location = new System.Drawing.Point(6, 22);
            this.listViewAlgorithm.Name = "listViewAlgorithm";
            this.listViewAlgorithm.Size = new System.Drawing.Size(315, 150);
            this.listViewAlgorithm.TabIndex = 0;
            this.listViewAlgorithm.UseCompatibleStateImageBehavior = false;
            this.listViewAlgorithm.View = System.Windows.Forms.View.Details;
            this.listViewAlgorithm.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.listViewAlgorithm_KeyPress);
            // 
            // columnHeaderAlgorithm
            // 
            this.columnHeaderAlgorithm.Text = "演算法";
            this.columnHeaderAlgorithm.Width = 160;
            // 
            // columnHeaderPriority
            // 
            this.columnHeaderPriority.Text = "優先權";
            // 
            // columnHeaderColor
            // 
            this.columnHeaderColor.Text = "顏色";
            this.columnHeaderColor.Width = 80;
            // 
            // labAlgorithmSelect
            // 
            this.labAlgorithmSelect.AutoSize = true;
            this.labAlgorithmSelect.Location = new System.Drawing.Point(15, 9);
            this.labAlgorithmSelect.Name = "labAlgorithmSelect";
            this.labAlgorithmSelect.Size = new System.Drawing.Size(65, 16);
            this.labAlgorithmSelect.TabIndex = 1;
            this.labAlgorithmSelect.Text = "方法選擇 : ";
            // 
            // comboBoxAlgorithmSelect
            // 
            this.comboBoxAlgorithmSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAlgorithmSelect.FormattingEnabled = true;
            this.comboBoxAlgorithmSelect.Location = new System.Drawing.Point(99, 6);
            this.comboBoxAlgorithmSelect.Name = "comboBoxAlgorithmSelect";
            this.comboBoxAlgorithmSelect.Size = new System.Drawing.Size(121, 24);
            this.comboBoxAlgorithmSelect.TabIndex = 0;
            // 
            // Inner
            // 
            this.Inner.BackColor = System.Drawing.SystemColors.Control;
            this.Inner.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Inner.Controls.Add(this.btnTestEro);
            this.Inner.Controls.Add(this.comboBoxInnerBand);
            this.Inner.Controls.Add(this.labInnerBand);
            this.Inner.Controls.Add(this.txbInnerName);
            this.Inner.Controls.Add(this.btnInnerInsp);
            this.Inner.Controls.Add(this.gbxInnerParam);
            this.Inner.Controls.Add(this.cbxInnerEnabled);
            this.Inner.Controls.Add(this.gbxinnerDefectSpec);
            this.Inner.Location = new System.Drawing.Point(4, 25);
            this.Inner.Name = "Inner";
            this.Inner.Size = new System.Drawing.Size(382, 817);
            this.Inner.TabIndex = 1;
            this.Inner.Text = "圖樣內部瑕疵";
            // 
            // btnTestEro
            // 
            this.btnTestEro.Location = new System.Drawing.Point(609, 219);
            this.btnTestEro.Name = "btnTestEro";
            this.btnTestEro.Size = new System.Drawing.Size(124, 32);
            this.btnTestEro.TabIndex = 92;
            this.btnTestEro.Text = "檢測區域";
            this.btnTestEro.UseVisualStyleBackColor = true;
            this.btnTestEro.Click += new System.EventHandler(this.btnTestEro_Click);
            // 
            // comboBoxInnerBand
            // 
            this.comboBoxInnerBand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxInnerBand.FormattingEnabled = true;
            this.comboBoxInnerBand.Items.AddRange(new object[] {
            "R",
            "G",
            "B",
            "Gray"});
            this.comboBoxInnerBand.Location = new System.Drawing.Point(278, 11);
            this.comboBoxInnerBand.Name = "comboBoxInnerBand";
            this.comboBoxInnerBand.Size = new System.Drawing.Size(54, 24);
            this.comboBoxInnerBand.TabIndex = 91;
            this.comboBoxInnerBand.SelectedIndexChanged += new System.EventHandler(this.comboBoxInnerBand_SelectedIndexChanged);
            // 
            // labInnerBand
            // 
            this.labInnerBand.AutoSize = true;
            this.labInnerBand.Location = new System.Drawing.Point(209, 14);
            this.labInnerBand.Name = "labInnerBand";
            this.labInnerBand.Size = new System.Drawing.Size(48, 16);
            this.labInnerBand.TabIndex = 90;
            this.labInnerBand.Text = "Band : ";
            // 
            // txbInnerName
            // 
            this.txbInnerName.Location = new System.Drawing.Point(103, 11);
            this.txbInnerName.Name = "txbInnerName";
            this.txbInnerName.Size = new System.Drawing.Size(100, 23);
            this.txbInnerName.TabIndex = 89;
            this.txbInnerName.Text = "Defect";
            // 
            // btnInnerInsp
            // 
            this.btnInnerInsp.BackColor = System.Drawing.Color.LightGray;
            this.btnInnerInsp.Location = new System.Drawing.Point(609, 257);
            this.btnInnerInsp.Name = "btnInnerInsp";
            this.btnInnerInsp.Size = new System.Drawing.Size(124, 32);
            this.btnInnerInsp.TabIndex = 88;
            this.btnInnerInsp.Text = "檢測";
            this.btnInnerInsp.UseVisualStyleBackColor = false;
            this.btnInnerInsp.Click += new System.EventHandler(this.btnInnerInsp_Click);
            // 
            // gbxInnerParam
            // 
            this.gbxInnerParam.Controls.Add(this.btnInnerMultiTh);
            this.gbxInnerParam.Controls.Add(this.nudInnerLTh);
            this.gbxInnerParam.Controls.Add(this.txbInnerImgIndex);
            this.gbxInnerParam.Controls.Add(this.labInnerEdgeSkipSize);
            this.gbxInnerParam.Controls.Add(this.labInnerHighTH);
            this.gbxInnerParam.Controls.Add(this.labInnerImgIndex);
            this.gbxInnerParam.Controls.Add(this.btnInnerThSetup);
            this.gbxInnerParam.Controls.Add(this.nudInnerEdgeSkipHeight);
            this.gbxInnerParam.Controls.Add(this.nudInnerEdgeSkipSize);
            this.gbxInnerParam.Controls.Add(this.nudInnerHTH);
            this.gbxInnerParam.Controls.Add(this.labInnerLowTh);
            this.gbxInnerParam.Location = new System.Drawing.Point(12, 40);
            this.gbxInnerParam.Name = "gbxInnerParam";
            this.gbxInnerParam.Size = new System.Drawing.Size(262, 249);
            this.gbxInnerParam.TabIndex = 87;
            this.gbxInnerParam.TabStop = false;
            this.gbxInnerParam.Text = "基本參數設定";
            // 
            // btnInnerMultiTh
            // 
            this.btnInnerMultiTh.Location = new System.Drawing.Point(25, 210);
            this.btnInnerMultiTh.Name = "btnInnerMultiTh";
            this.btnInnerMultiTh.Size = new System.Drawing.Size(170, 33);
            this.btnInnerMultiTh.TabIndex = 89;
            this.btnInnerMultiTh.Text = "多組閥值設定";
            this.btnInnerMultiTh.UseVisualStyleBackColor = true;
            this.btnInnerMultiTh.Click += new System.EventHandler(this.btnInnerMultiTh_Click);
            // 
            // nudInnerLTh
            // 
            this.nudInnerLTh.Font = new System.Drawing.Font("Verdana", 9.75F);
            this.nudInnerLTh.Location = new System.Drawing.Point(137, 116);
            this.nudInnerLTh.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudInnerLTh.Name = "nudInnerLTh";
            this.nudInnerLTh.Size = new System.Drawing.Size(58, 23);
            this.nudInnerLTh.TabIndex = 84;
            // 
            // txbInnerImgIndex
            // 
            this.txbInnerImgIndex.Location = new System.Drawing.Point(91, 38);
            this.txbInnerImgIndex.Name = "txbInnerImgIndex";
            this.txbInnerImgIndex.Size = new System.Drawing.Size(104, 23);
            this.txbInnerImgIndex.TabIndex = 9;
            this.txbInnerImgIndex.Text = "0";
            // 
            // labInnerEdgeSkipSize
            // 
            this.labInnerEdgeSkipSize.AutoSize = true;
            this.labInnerEdgeSkipSize.Location = new System.Drawing.Point(22, 187);
            this.labInnerEdgeSkipSize.Name = "labInnerEdgeSkipSize";
            this.labInnerEdgeSkipSize.Size = new System.Drawing.Size(83, 16);
            this.labInnerEdgeSkipSize.TabIndex = 85;
            this.labInnerEdgeSkipSize.Text = "邊緣忽略大小:";
            // 
            // labInnerHighTH
            // 
            this.labInnerHighTH.AutoSize = true;
            this.labInnerHighTH.Location = new System.Drawing.Point(22, 151);
            this.labInnerHighTH.Name = "labInnerHighTH";
            this.labInnerHighTH.Size = new System.Drawing.Size(96, 16);
            this.labInnerHighTH.TabIndex = 85;
            this.labInnerHighTH.Text = "MaxThreshold:";
            // 
            // labInnerImgIndex
            // 
            this.labInnerImgIndex.AutoSize = true;
            this.labInnerImgIndex.Location = new System.Drawing.Point(22, 41);
            this.labInnerImgIndex.Name = "labInnerImgIndex";
            this.labInnerImgIndex.Size = new System.Drawing.Size(54, 16);
            this.labInnerImgIndex.TabIndex = 10;
            this.labInnerImgIndex.Text = "影像ID : ";
            // 
            // btnInnerThSetup
            // 
            this.btnInnerThSetup.Location = new System.Drawing.Point(25, 76);
            this.btnInnerThSetup.Name = "btnInnerThSetup";
            this.btnInnerThSetup.Size = new System.Drawing.Size(170, 32);
            this.btnInnerThSetup.TabIndex = 82;
            this.btnInnerThSetup.Text = "二值化設定";
            this.btnInnerThSetup.UseVisualStyleBackColor = true;
            this.btnInnerThSetup.Click += new System.EventHandler(this.btnInnerThSetup_Click);
            // 
            // nudInnerEdgeSkipHeight
            // 
            this.nudInnerEdgeSkipHeight.Font = new System.Drawing.Font("Verdana", 9.75F);
            this.nudInnerEdgeSkipHeight.Location = new System.Drawing.Point(198, 185);
            this.nudInnerEdgeSkipHeight.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudInnerEdgeSkipHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudInnerEdgeSkipHeight.Name = "nudInnerEdgeSkipHeight";
            this.nudInnerEdgeSkipHeight.Size = new System.Drawing.Size(58, 23);
            this.nudInnerEdgeSkipHeight.TabIndex = 83;
            this.nudInnerEdgeSkipHeight.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudInnerEdgeSkipSize
            // 
            this.nudInnerEdgeSkipSize.Font = new System.Drawing.Font("Verdana", 9.75F);
            this.nudInnerEdgeSkipSize.Location = new System.Drawing.Point(137, 185);
            this.nudInnerEdgeSkipSize.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudInnerEdgeSkipSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudInnerEdgeSkipSize.Name = "nudInnerEdgeSkipSize";
            this.nudInnerEdgeSkipSize.Size = new System.Drawing.Size(58, 23);
            this.nudInnerEdgeSkipSize.TabIndex = 83;
            this.nudInnerEdgeSkipSize.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudInnerHTH
            // 
            this.nudInnerHTH.Font = new System.Drawing.Font("Verdana", 9.75F);
            this.nudInnerHTH.Location = new System.Drawing.Point(137, 149);
            this.nudInnerHTH.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudInnerHTH.Name = "nudInnerHTH";
            this.nudInnerHTH.Size = new System.Drawing.Size(58, 23);
            this.nudInnerHTH.TabIndex = 83;
            // 
            // labInnerLowTh
            // 
            this.labInnerLowTh.AutoSize = true;
            this.labInnerLowTh.Location = new System.Drawing.Point(22, 118);
            this.labInnerLowTh.Name = "labInnerLowTh";
            this.labInnerLowTh.Size = new System.Drawing.Size(94, 16);
            this.labInnerLowTh.TabIndex = 86;
            this.labInnerLowTh.Text = "MinThreshold:";
            // 
            // cbxInnerEnabled
            // 
            this.cbxInnerEnabled.AutoSize = true;
            this.cbxInnerEnabled.Location = new System.Drawing.Point(12, 12);
            this.cbxInnerEnabled.Name = "cbxInnerEnabled";
            this.cbxInnerEnabled.Size = new System.Drawing.Size(75, 20);
            this.cbxInnerEnabled.TabIndex = 0;
            this.cbxInnerEnabled.Text = "檢測啟用";
            this.cbxInnerEnabled.UseVisualStyleBackColor = true;
            // 
            // gbxinnerDefectSpec
            // 
            this.gbxinnerDefectSpec.Controls.Add(this.cbxInnerHeightEnabled);
            this.gbxinnerDefectSpec.Controls.Add(this.cbxInnerWidthEnabled);
            this.gbxinnerDefectSpec.Controls.Add(this.label1);
            this.gbxinnerDefectSpec.Controls.Add(this.cbxInnerAreaEnabled);
            this.gbxinnerDefectSpec.Controls.Add(this.label2);
            this.gbxinnerDefectSpec.Controls.Add(this.label3);
            this.gbxinnerDefectSpec.Controls.Add(this.labInnerMinA);
            this.gbxinnerDefectSpec.Controls.Add(this.txbInnerMinA);
            this.gbxinnerDefectSpec.Controls.Add(this.labInnerMinH);
            this.gbxinnerDefectSpec.Controls.Add(this.txbInnerMinH);
            this.gbxinnerDefectSpec.Controls.Add(this.labInnerMinW);
            this.gbxinnerDefectSpec.Controls.Add(this.txbInnerMinW);
            this.gbxinnerDefectSpec.Location = new System.Drawing.Point(280, 40);
            this.gbxinnerDefectSpec.Name = "gbxinnerDefectSpec";
            this.gbxinnerDefectSpec.Size = new System.Drawing.Size(323, 249);
            this.gbxinnerDefectSpec.TabIndex = 87;
            this.gbxinnerDefectSpec.TabStop = false;
            this.gbxinnerDefectSpec.Text = "檢出規格設定";
            // 
            // cbxInnerHeightEnabled
            // 
            this.cbxInnerHeightEnabled.AutoSize = true;
            this.cbxInnerHeightEnabled.Location = new System.Drawing.Point(261, 161);
            this.cbxInnerHeightEnabled.Name = "cbxInnerHeightEnabled";
            this.cbxInnerHeightEnabled.Size = new System.Drawing.Size(51, 20);
            this.cbxInnerHeightEnabled.TabIndex = 103;
            this.cbxInnerHeightEnabled.Text = "啟用";
            this.cbxInnerHeightEnabled.UseVisualStyleBackColor = true;
            // 
            // cbxInnerWidthEnabled
            // 
            this.cbxInnerWidthEnabled.AutoSize = true;
            this.cbxInnerWidthEnabled.Location = new System.Drawing.Point(261, 99);
            this.cbxInnerWidthEnabled.Name = "cbxInnerWidthEnabled";
            this.cbxInnerWidthEnabled.Size = new System.Drawing.Size(51, 20);
            this.cbxInnerWidthEnabled.TabIndex = 104;
            this.cbxInnerWidthEnabled.Text = "啟用";
            this.cbxInnerWidthEnabled.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(197, 162);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 16);
            this.label1.TabIndex = 100;
            this.label1.Text = "(um)";
            // 
            // cbxInnerAreaEnabled
            // 
            this.cbxInnerAreaEnabled.AutoSize = true;
            this.cbxInnerAreaEnabled.Location = new System.Drawing.Point(261, 37);
            this.cbxInnerAreaEnabled.Name = "cbxInnerAreaEnabled";
            this.cbxInnerAreaEnabled.Size = new System.Drawing.Size(51, 20);
            this.cbxInnerAreaEnabled.TabIndex = 105;
            this.cbxInnerAreaEnabled.Text = "啟用";
            this.cbxInnerAreaEnabled.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(197, 99);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 16);
            this.label2.TabIndex = 101;
            this.label2.Text = "(um)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(197, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 16);
            this.label3.TabIndex = 102;
            this.label3.Text = "(um^2)";
            // 
            // labInnerMinA
            // 
            this.labInnerMinA.AutoSize = true;
            this.labInnerMinA.Location = new System.Drawing.Point(6, 37);
            this.labInnerMinA.Name = "labInnerMinA";
            this.labInnerMinA.Size = new System.Drawing.Size(62, 16);
            this.labInnerMinA.TabIndex = 86;
            this.labInnerMinA.Text = "檢測面積 :";
            // 
            // txbInnerMinA
            // 
            this.txbInnerMinA.Location = new System.Drawing.Point(91, 34);
            this.txbInnerMinA.Name = "txbInnerMinA";
            this.txbInnerMinA.Size = new System.Drawing.Size(100, 23);
            this.txbInnerMinA.TabIndex = 87;
            this.txbInnerMinA.Text = "10";
            // 
            // labInnerMinH
            // 
            this.labInnerMinH.AutoSize = true;
            this.labInnerMinH.Location = new System.Drawing.Point(6, 161);
            this.labInnerMinH.Name = "labInnerMinH";
            this.labInnerMinH.Size = new System.Drawing.Size(62, 16);
            this.labInnerMinH.TabIndex = 86;
            this.labInnerMinH.Text = "檢測高度 :";
            // 
            // txbInnerMinH
            // 
            this.txbInnerMinH.Location = new System.Drawing.Point(91, 157);
            this.txbInnerMinH.Name = "txbInnerMinH";
            this.txbInnerMinH.Size = new System.Drawing.Size(100, 23);
            this.txbInnerMinH.TabIndex = 87;
            this.txbInnerMinH.Text = "10";
            // 
            // labInnerMinW
            // 
            this.labInnerMinW.AutoSize = true;
            this.labInnerMinW.Location = new System.Drawing.Point(6, 99);
            this.labInnerMinW.Name = "labInnerMinW";
            this.labInnerMinW.Size = new System.Drawing.Size(62, 16);
            this.labInnerMinW.TabIndex = 86;
            this.labInnerMinW.Text = "檢測寬度 :";
            // 
            // txbInnerMinW
            // 
            this.txbInnerMinW.Location = new System.Drawing.Point(91, 95);
            this.txbInnerMinW.Name = "txbInnerMinW";
            this.txbInnerMinW.Size = new System.Drawing.Size(100, 23);
            this.txbInnerMinW.TabIndex = 87;
            this.txbInnerMinW.Text = "10";
            // 
            // Outer
            // 
            this.Outer.BackColor = System.Drawing.SystemColors.Control;
            this.Outer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Outer.Controls.Add(this.btnDilTest);
            this.Outer.Controls.Add(this.comboBoxOuterBand);
            this.Outer.Controls.Add(this.labOuterBand);
            this.Outer.Controls.Add(this.txbOuterName);
            this.Outer.Controls.Add(this.btnOuterInsp);
            this.Outer.Controls.Add(this.gbxOuterParam);
            this.Outer.Controls.Add(this.cbxOuterEnabled);
            this.Outer.Controls.Add(this.gbxOuterDefectSpec);
            this.Outer.Location = new System.Drawing.Point(4, 25);
            this.Outer.Name = "Outer";
            this.Outer.Size = new System.Drawing.Size(382, 817);
            this.Outer.TabIndex = 2;
            this.Outer.Text = "圖樣外部瑕疵";
            // 
            // btnDilTest
            // 
            this.btnDilTest.Location = new System.Drawing.Point(609, 219);
            this.btnDilTest.Name = "btnDilTest";
            this.btnDilTest.Size = new System.Drawing.Size(124, 32);
            this.btnDilTest.TabIndex = 95;
            this.btnDilTest.Text = "檢測區域";
            this.btnDilTest.UseVisualStyleBackColor = true;
            this.btnDilTest.Click += new System.EventHandler(this.btnDilTest_Click);
            // 
            // comboBoxOuterBand
            // 
            this.comboBoxOuterBand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOuterBand.FormattingEnabled = true;
            this.comboBoxOuterBand.Items.AddRange(new object[] {
            "R",
            "G",
            "B",
            "Gray"});
            this.comboBoxOuterBand.Location = new System.Drawing.Point(278, 11);
            this.comboBoxOuterBand.Name = "comboBoxOuterBand";
            this.comboBoxOuterBand.Size = new System.Drawing.Size(54, 24);
            this.comboBoxOuterBand.TabIndex = 94;
            this.comboBoxOuterBand.SelectedIndexChanged += new System.EventHandler(this.comboBoxOuterBand_SelectedIndexChanged);
            // 
            // labOuterBand
            // 
            this.labOuterBand.AutoSize = true;
            this.labOuterBand.Location = new System.Drawing.Point(209, 14);
            this.labOuterBand.Name = "labOuterBand";
            this.labOuterBand.Size = new System.Drawing.Size(48, 16);
            this.labOuterBand.TabIndex = 93;
            this.labOuterBand.Text = "Band : ";
            // 
            // txbOuterName
            // 
            this.txbOuterName.Location = new System.Drawing.Point(103, 11);
            this.txbOuterName.Name = "txbOuterName";
            this.txbOuterName.Size = new System.Drawing.Size(100, 23);
            this.txbOuterName.TabIndex = 92;
            this.txbOuterName.Text = "Defect";
            // 
            // btnOuterInsp
            // 
            this.btnOuterInsp.BackColor = System.Drawing.Color.LightGray;
            this.btnOuterInsp.Location = new System.Drawing.Point(609, 257);
            this.btnOuterInsp.Name = "btnOuterInsp";
            this.btnOuterInsp.Size = new System.Drawing.Size(124, 32);
            this.btnOuterInsp.TabIndex = 91;
            this.btnOuterInsp.Text = "檢測";
            this.btnOuterInsp.UseVisualStyleBackColor = false;
            this.btnOuterInsp.Click += new System.EventHandler(this.btnOuterInsp_Click);
            // 
            // gbxOuterParam
            // 
            this.gbxOuterParam.Controls.Add(this.btnOuterMultiTh);
            this.gbxOuterParam.Controls.Add(this.nudOuterLTh);
            this.gbxOuterParam.Controls.Add(this.txbOuterImgIndex);
            this.gbxOuterParam.Controls.Add(this.labOuterEdgeSkipSize);
            this.gbxOuterParam.Controls.Add(this.labOuterHighTH);
            this.gbxOuterParam.Controls.Add(this.labOuterImgIndex);
            this.gbxOuterParam.Controls.Add(this.btnOuterThSetup);
            this.gbxOuterParam.Controls.Add(this.nudOuterEdgeSkipHeight);
            this.gbxOuterParam.Controls.Add(this.nudOuterEdgeSkipSize);
            this.gbxOuterParam.Controls.Add(this.nudOuterHTH);
            this.gbxOuterParam.Controls.Add(this.labOuterLowTh);
            this.gbxOuterParam.Location = new System.Drawing.Point(12, 40);
            this.gbxOuterParam.Name = "gbxOuterParam";
            this.gbxOuterParam.Size = new System.Drawing.Size(262, 249);
            this.gbxOuterParam.TabIndex = 89;
            this.gbxOuterParam.TabStop = false;
            this.gbxOuterParam.Text = "基本參數設定";
            // 
            // btnOuterMultiTh
            // 
            this.btnOuterMultiTh.Location = new System.Drawing.Point(25, 210);
            this.btnOuterMultiTh.Name = "btnOuterMultiTh";
            this.btnOuterMultiTh.Size = new System.Drawing.Size(170, 33);
            this.btnOuterMultiTh.TabIndex = 90;
            this.btnOuterMultiTh.Text = "多組閥值設定";
            this.btnOuterMultiTh.UseVisualStyleBackColor = true;
            this.btnOuterMultiTh.Click += new System.EventHandler(this.button1_Click);
            // 
            // nudOuterLTh
            // 
            this.nudOuterLTh.Font = new System.Drawing.Font("Verdana", 9.75F);
            this.nudOuterLTh.Location = new System.Drawing.Point(137, 116);
            this.nudOuterLTh.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudOuterLTh.Name = "nudOuterLTh";
            this.nudOuterLTh.Size = new System.Drawing.Size(58, 23);
            this.nudOuterLTh.TabIndex = 84;
            // 
            // txbOuterImgIndex
            // 
            this.txbOuterImgIndex.Location = new System.Drawing.Point(91, 38);
            this.txbOuterImgIndex.Name = "txbOuterImgIndex";
            this.txbOuterImgIndex.Size = new System.Drawing.Size(104, 23);
            this.txbOuterImgIndex.TabIndex = 9;
            this.txbOuterImgIndex.Text = "0";
            // 
            // labOuterEdgeSkipSize
            // 
            this.labOuterEdgeSkipSize.AutoSize = true;
            this.labOuterEdgeSkipSize.Location = new System.Drawing.Point(22, 187);
            this.labOuterEdgeSkipSize.Name = "labOuterEdgeSkipSize";
            this.labOuterEdgeSkipSize.Size = new System.Drawing.Size(83, 16);
            this.labOuterEdgeSkipSize.TabIndex = 85;
            this.labOuterEdgeSkipSize.Text = "邊緣忽略大小:";
            // 
            // labOuterHighTH
            // 
            this.labOuterHighTH.AutoSize = true;
            this.labOuterHighTH.Location = new System.Drawing.Point(22, 151);
            this.labOuterHighTH.Name = "labOuterHighTH";
            this.labOuterHighTH.Size = new System.Drawing.Size(96, 16);
            this.labOuterHighTH.TabIndex = 85;
            this.labOuterHighTH.Text = "MaxThreshold:";
            // 
            // labOuterImgIndex
            // 
            this.labOuterImgIndex.AutoSize = true;
            this.labOuterImgIndex.Location = new System.Drawing.Point(22, 41);
            this.labOuterImgIndex.Name = "labOuterImgIndex";
            this.labOuterImgIndex.Size = new System.Drawing.Size(54, 16);
            this.labOuterImgIndex.TabIndex = 10;
            this.labOuterImgIndex.Text = "影像ID : ";
            // 
            // btnOuterThSetup
            // 
            this.btnOuterThSetup.Location = new System.Drawing.Point(25, 76);
            this.btnOuterThSetup.Name = "btnOuterThSetup";
            this.btnOuterThSetup.Size = new System.Drawing.Size(170, 32);
            this.btnOuterThSetup.TabIndex = 82;
            this.btnOuterThSetup.Text = "二值化設定";
            this.btnOuterThSetup.UseVisualStyleBackColor = true;
            this.btnOuterThSetup.Click += new System.EventHandler(this.btnOuterThSetup_Click);
            // 
            // nudOuterEdgeSkipHeight
            // 
            this.nudOuterEdgeSkipHeight.Font = new System.Drawing.Font("Verdana", 9.75F);
            this.nudOuterEdgeSkipHeight.Location = new System.Drawing.Point(198, 185);
            this.nudOuterEdgeSkipHeight.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudOuterEdgeSkipHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudOuterEdgeSkipHeight.Name = "nudOuterEdgeSkipHeight";
            this.nudOuterEdgeSkipHeight.Size = new System.Drawing.Size(58, 23);
            this.nudOuterEdgeSkipHeight.TabIndex = 83;
            this.nudOuterEdgeSkipHeight.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudOuterEdgeSkipSize
            // 
            this.nudOuterEdgeSkipSize.Font = new System.Drawing.Font("Verdana", 9.75F);
            this.nudOuterEdgeSkipSize.Location = new System.Drawing.Point(137, 185);
            this.nudOuterEdgeSkipSize.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudOuterEdgeSkipSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudOuterEdgeSkipSize.Name = "nudOuterEdgeSkipSize";
            this.nudOuterEdgeSkipSize.Size = new System.Drawing.Size(58, 23);
            this.nudOuterEdgeSkipSize.TabIndex = 83;
            this.nudOuterEdgeSkipSize.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudOuterHTH
            // 
            this.nudOuterHTH.Font = new System.Drawing.Font("Verdana", 9.75F);
            this.nudOuterHTH.Location = new System.Drawing.Point(137, 149);
            this.nudOuterHTH.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudOuterHTH.Name = "nudOuterHTH";
            this.nudOuterHTH.Size = new System.Drawing.Size(58, 23);
            this.nudOuterHTH.TabIndex = 83;
            // 
            // labOuterLowTh
            // 
            this.labOuterLowTh.AutoSize = true;
            this.labOuterLowTh.Location = new System.Drawing.Point(22, 118);
            this.labOuterLowTh.Name = "labOuterLowTh";
            this.labOuterLowTh.Size = new System.Drawing.Size(94, 16);
            this.labOuterLowTh.TabIndex = 86;
            this.labOuterLowTh.Text = "MinThreshold:";
            // 
            // cbxOuterEnabled
            // 
            this.cbxOuterEnabled.AutoSize = true;
            this.cbxOuterEnabled.Location = new System.Drawing.Point(12, 12);
            this.cbxOuterEnabled.Name = "cbxOuterEnabled";
            this.cbxOuterEnabled.Size = new System.Drawing.Size(75, 20);
            this.cbxOuterEnabled.TabIndex = 0;
            this.cbxOuterEnabled.Text = "檢測啟用";
            this.cbxOuterEnabled.UseVisualStyleBackColor = true;
            // 
            // gbxOuterDefectSpec
            // 
            this.gbxOuterDefectSpec.Controls.Add(this.label4);
            this.gbxOuterDefectSpec.Controls.Add(this.label5);
            this.gbxOuterDefectSpec.Controls.Add(this.label6);
            this.gbxOuterDefectSpec.Controls.Add(this.cbxOuterHEnabled);
            this.gbxOuterDefectSpec.Controls.Add(this.cbxOuterWEnabled);
            this.gbxOuterDefectSpec.Controls.Add(this.cbxOuterAreaEnabled);
            this.gbxOuterDefectSpec.Controls.Add(this.labOuterMinH);
            this.gbxOuterDefectSpec.Controls.Add(this.labOuterMinA);
            this.gbxOuterDefectSpec.Controls.Add(this.txbOuterMinH);
            this.gbxOuterDefectSpec.Controls.Add(this.txbOuterMinA);
            this.gbxOuterDefectSpec.Controls.Add(this.labOuterMinW);
            this.gbxOuterDefectSpec.Controls.Add(this.txbOuterMinW);
            this.gbxOuterDefectSpec.Location = new System.Drawing.Point(280, 40);
            this.gbxOuterDefectSpec.Name = "gbxOuterDefectSpec";
            this.gbxOuterDefectSpec.Size = new System.Drawing.Size(323, 249);
            this.gbxOuterDefectSpec.TabIndex = 90;
            this.gbxOuterDefectSpec.TabStop = false;
            this.gbxOuterDefectSpec.Text = "檢出規格設定";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(197, 162);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 16);
            this.label4.TabIndex = 109;
            this.label4.Text = "(um)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(197, 99);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 16);
            this.label5.TabIndex = 110;
            this.label5.Text = "(um)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(197, 37);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(51, 16);
            this.label6.TabIndex = 111;
            this.label6.Text = "(um^2)";
            // 
            // cbxOuterHEnabled
            // 
            this.cbxOuterHEnabled.AutoSize = true;
            this.cbxOuterHEnabled.Location = new System.Drawing.Point(261, 161);
            this.cbxOuterHEnabled.Name = "cbxOuterHEnabled";
            this.cbxOuterHEnabled.Size = new System.Drawing.Size(51, 20);
            this.cbxOuterHEnabled.TabIndex = 106;
            this.cbxOuterHEnabled.Text = "啟用";
            this.cbxOuterHEnabled.UseVisualStyleBackColor = true;
            // 
            // cbxOuterWEnabled
            // 
            this.cbxOuterWEnabled.AutoSize = true;
            this.cbxOuterWEnabled.Location = new System.Drawing.Point(261, 99);
            this.cbxOuterWEnabled.Name = "cbxOuterWEnabled";
            this.cbxOuterWEnabled.Size = new System.Drawing.Size(51, 20);
            this.cbxOuterWEnabled.TabIndex = 107;
            this.cbxOuterWEnabled.Text = "啟用";
            this.cbxOuterWEnabled.UseVisualStyleBackColor = true;
            // 
            // cbxOuterAreaEnabled
            // 
            this.cbxOuterAreaEnabled.AutoSize = true;
            this.cbxOuterAreaEnabled.Location = new System.Drawing.Point(261, 37);
            this.cbxOuterAreaEnabled.Name = "cbxOuterAreaEnabled";
            this.cbxOuterAreaEnabled.Size = new System.Drawing.Size(51, 20);
            this.cbxOuterAreaEnabled.TabIndex = 108;
            this.cbxOuterAreaEnabled.Text = "啟用";
            this.cbxOuterAreaEnabled.UseVisualStyleBackColor = true;
            // 
            // labOuterMinH
            // 
            this.labOuterMinH.AutoSize = true;
            this.labOuterMinH.Location = new System.Drawing.Point(6, 161);
            this.labOuterMinH.Name = "labOuterMinH";
            this.labOuterMinH.Size = new System.Drawing.Size(62, 16);
            this.labOuterMinH.TabIndex = 86;
            this.labOuterMinH.Text = "檢測高度 :";
            // 
            // labOuterMinA
            // 
            this.labOuterMinA.AutoSize = true;
            this.labOuterMinA.Location = new System.Drawing.Point(6, 37);
            this.labOuterMinA.Name = "labOuterMinA";
            this.labOuterMinA.Size = new System.Drawing.Size(65, 16);
            this.labOuterMinA.TabIndex = 86;
            this.labOuterMinA.Text = "檢測面積 : ";
            // 
            // txbOuterMinH
            // 
            this.txbOuterMinH.Location = new System.Drawing.Point(91, 157);
            this.txbOuterMinH.Name = "txbOuterMinH";
            this.txbOuterMinH.Size = new System.Drawing.Size(100, 23);
            this.txbOuterMinH.TabIndex = 87;
            this.txbOuterMinH.Text = "10";
            // 
            // txbOuterMinA
            // 
            this.txbOuterMinA.Location = new System.Drawing.Point(91, 34);
            this.txbOuterMinA.Name = "txbOuterMinA";
            this.txbOuterMinA.Size = new System.Drawing.Size(100, 23);
            this.txbOuterMinA.TabIndex = 87;
            this.txbOuterMinA.Text = "10";
            // 
            // labOuterMinW
            // 
            this.labOuterMinW.AutoSize = true;
            this.labOuterMinW.Location = new System.Drawing.Point(6, 99);
            this.labOuterMinW.Name = "labOuterMinW";
            this.labOuterMinW.Size = new System.Drawing.Size(62, 16);
            this.labOuterMinW.TabIndex = 86;
            this.labOuterMinW.Text = "檢測寬度 :";
            // 
            // txbOuterMinW
            // 
            this.txbOuterMinW.Location = new System.Drawing.Point(91, 95);
            this.txbOuterMinW.Name = "txbOuterMinW";
            this.txbOuterMinW.Size = new System.Drawing.Size(100, 23);
            this.txbOuterMinW.TabIndex = 87;
            this.txbOuterMinW.Text = "10";
            // 
            // Thin
            // 
            this.Thin.AutoScroll = true;
            this.Thin.BackColor = System.Drawing.SystemColors.Control;
            this.Thin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Thin.Controls.Add(this.gbxThinScratch);
            this.Thin.Controls.Add(this.groupBox1);
            this.Thin.Controls.Add(this.comboxModeSelect);
            this.Thin.Controls.Add(this.gbxHistoEq);
            this.Thin.Controls.Add(this.btnSaveMode);
            this.Thin.Controls.Add(this.txbThinName);
            this.Thin.Controls.Add(this.cbxThinEnabled);
            this.Thin.Controls.Add(this.gbxThinParam);
            this.Thin.Controls.Add(this.btnThinPartitionTest);
            this.Thin.Controls.Add(this.gbxSmallArea);
            this.Thin.Controls.Add(this.btnThinInsp);
            this.Thin.Controls.Add(this.gbxLargeArea);
            this.Thin.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.Thin.Location = new System.Drawing.Point(4, 25);
            this.Thin.Name = "Thin";
            this.Thin.Size = new System.Drawing.Size(382, 817);
            this.Thin.TabIndex = 3;
            this.Thin.Text = "薄化檢測";
            // 
            // gbxThinScratch
            // 
            this.gbxThinScratch.Controls.Add(this.label55);
            this.gbxThinScratch.Controls.Add(this.nudSensitivity);
            this.gbxThinScratch.Controls.Add(this.cbxTrainROI);
            this.gbxThinScratch.Controls.Add(this.comboBoxSelectRegion);
            this.gbxThinScratch.Controls.Add(this.btnSelectObj);
            this.gbxThinScratch.Controls.Add(this.btnTrain);
            this.gbxThinScratch.Controls.Add(this.btnTextTest);
            this.gbxThinScratch.Controls.Add(this.btnAddImage);
            this.gbxThinScratch.Controls.Add(this.btnCreateNewMode);
            this.gbxThinScratch.Controls.Add(this.cbxThinScratchEnabled);
            this.gbxThinScratch.Controls.Add(this.label61);
            this.gbxThinScratch.Controls.Add(this.cbxThinScratchHeightEnabled);
            this.gbxThinScratch.Controls.Add(this.txbThinScratchHeight);
            this.gbxThinScratch.Controls.Add(this.txbThinScratchWidth);
            this.gbxThinScratch.Controls.Add(this.cbxThinScratchWidthEnabled);
            this.gbxThinScratch.Controls.Add(this.nudThinScratchCloseH);
            this.gbxThinScratch.Controls.Add(this.nudThinScratchCloseW);
            this.gbxThinScratch.Controls.Add(this.label64);
            this.gbxThinScratch.Controls.Add(this.txbThinScratchArea);
            this.gbxThinScratch.Controls.Add(this.label56);
            this.gbxThinScratch.Controls.Add(this.cbxThinScratchAreaEnabled);
            this.gbxThinScratch.Controls.Add(this.nudThinScratchEdgeExHeight);
            this.gbxThinScratch.Controls.Add(this.nudThinScratchEdgeSkipH);
            this.gbxThinScratch.Controls.Add(this.nudThinScratchOpenH);
            this.gbxThinScratch.Controls.Add(this.nudThinScratchOpenW);
            this.gbxThinScratch.Controls.Add(this.label71);
            this.gbxThinScratch.Controls.Add(this.nudThinScratchEdgeExWidth);
            this.gbxThinScratch.Controls.Add(this.label54);
            this.gbxThinScratch.Controls.Add(this.nudThinScratchEdgeSkipW);
            this.gbxThinScratch.Controls.Add(this.label63);
            this.gbxThinScratch.Controls.Add(this.label57);
            this.gbxThinScratch.Controls.Add(this.label58);
            this.gbxThinScratch.Controls.Add(this.label60);
            this.gbxThinScratch.Controls.Add(this.label59);
            this.gbxThinScratch.Location = new System.Drawing.Point(1497, 40);
            this.gbxThinScratch.Name = "gbxThinScratch";
            this.gbxThinScratch.Size = new System.Drawing.Size(394, 247);
            this.gbxThinScratch.TabIndex = 104;
            this.gbxThinScratch.TabStop = false;
            this.gbxThinScratch.Text = "劃痕";
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.Location = new System.Drawing.Point(10, 135);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(53, 16);
            this.label55.TabIndex = 125;
            this.label55.Text = "靈敏度 : ";
            // 
            // nudSensitivity
            // 
            this.nudSensitivity.Location = new System.Drawing.Point(112, 133);
            this.nudSensitivity.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudSensitivity.Name = "nudSensitivity";
            this.nudSensitivity.Size = new System.Drawing.Size(40, 23);
            this.nudSensitivity.TabIndex = 124;
            // 
            // cbxTrainROI
            // 
            this.cbxTrainROI.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbxTrainROI.BackColor = System.Drawing.Color.LightGray;
            this.cbxTrainROI.Location = new System.Drawing.Point(205, 73);
            this.cbxTrainROI.MaximumSize = new System.Drawing.Size(9999, 9999);
            this.cbxTrainROI.Name = "cbxTrainROI";
            this.cbxTrainROI.Size = new System.Drawing.Size(89, 23);
            this.cbxTrainROI.TabIndex = 121;
            this.cbxTrainROI.Text = "自訂區域";
            this.cbxTrainROI.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbxTrainROI.UseVisualStyleBackColor = false;
            this.cbxTrainROI.CheckedChanged += new System.EventHandler(this.cbxTrainROI_CheckedChanged);
            // 
            // comboBoxSelectRegion
            // 
            this.comboBoxSelectRegion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSelectRegion.FormattingEnabled = true;
            this.comboBoxSelectRegion.Location = new System.Drawing.Point(110, 43);
            this.comboBoxSelectRegion.Name = "comboBoxSelectRegion";
            this.comboBoxSelectRegion.Size = new System.Drawing.Size(89, 24);
            this.comboBoxSelectRegion.TabIndex = 106;
            this.comboBoxSelectRegion.SelectedIndexChanged += new System.EventHandler(this.comboBoxSelectRegion_SelectedIndexChanged);
            // 
            // btnSelectObj
            // 
            this.btnSelectObj.Location = new System.Drawing.Point(13, 44);
            this.btnSelectObj.Name = "btnSelectObj";
            this.btnSelectObj.Size = new System.Drawing.Size(91, 23);
            this.btnSelectObj.TabIndex = 105;
            this.btnSelectObj.Text = "切割訓練影像";
            this.btnSelectObj.UseVisualStyleBackColor = true;
            this.btnSelectObj.Click += new System.EventHandler(this.btnSelectObj_Click);
            // 
            // btnTrain
            // 
            this.btnTrain.Location = new System.Drawing.Point(302, 44);
            this.btnTrain.Name = "btnTrain";
            this.btnTrain.Size = new System.Drawing.Size(75, 23);
            this.btnTrain.TabIndex = 104;
            this.btnTrain.Text = "訓練";
            this.btnTrain.UseVisualStyleBackColor = true;
            this.btnTrain.Click += new System.EventHandler(this.btnTrain_Click);
            // 
            // btnTextTest
            // 
            this.btnTextTest.Location = new System.Drawing.Point(302, 73);
            this.btnTextTest.Name = "btnTextTest";
            this.btnTextTest.Size = new System.Drawing.Size(75, 23);
            this.btnTextTest.TabIndex = 103;
            this.btnTextTest.Text = "測試";
            this.btnTextTest.UseVisualStyleBackColor = true;
            this.btnTextTest.Click += new System.EventHandler(this.btnTextTest_Click);
            // 
            // btnAddImage
            // 
            this.btnAddImage.Location = new System.Drawing.Point(205, 44);
            this.btnAddImage.Name = "btnAddImage";
            this.btnAddImage.Size = new System.Drawing.Size(89, 23);
            this.btnAddImage.TabIndex = 102;
            this.btnAddImage.Text = "加入影像";
            this.btnAddImage.UseVisualStyleBackColor = true;
            this.btnAddImage.Click += new System.EventHandler(this.btnAddImage_Click);
            // 
            // btnCreateNewMode
            // 
            this.btnCreateNewMode.Location = new System.Drawing.Point(70, 16);
            this.btnCreateNewMode.Name = "btnCreateNewMode";
            this.btnCreateNewMode.Size = new System.Drawing.Size(89, 23);
            this.btnCreateNewMode.TabIndex = 101;
            this.btnCreateNewMode.Text = "建立新Mode";
            this.btnCreateNewMode.UseVisualStyleBackColor = true;
            this.btnCreateNewMode.Click += new System.EventHandler(this.btnCreateNewMode_Click);
            // 
            // cbxThinScratchEnabled
            // 
            this.cbxThinScratchEnabled.AutoSize = true;
            this.cbxThinScratchEnabled.Location = new System.Drawing.Point(13, 19);
            this.cbxThinScratchEnabled.Name = "cbxThinScratchEnabled";
            this.cbxThinScratchEnabled.Size = new System.Drawing.Size(51, 20);
            this.cbxThinScratchEnabled.TabIndex = 100;
            this.cbxThinScratchEnabled.Text = "啟用";
            this.cbxThinScratchEnabled.UseVisualStyleBackColor = true;
            // 
            // label61
            // 
            this.label61.AutoSize = true;
            this.label61.Location = new System.Drawing.Point(10, 216);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(65, 16);
            this.label61.TabIndex = 97;
            this.label61.Text = "檢測高度 : ";
            // 
            // cbxThinScratchHeightEnabled
            // 
            this.cbxThinScratchHeightEnabled.AutoSize = true;
            this.cbxThinScratchHeightEnabled.Location = new System.Drawing.Point(212, 216);
            this.cbxThinScratchHeightEnabled.Name = "cbxThinScratchHeightEnabled";
            this.cbxThinScratchHeightEnabled.Size = new System.Drawing.Size(51, 20);
            this.cbxThinScratchHeightEnabled.TabIndex = 99;
            this.cbxThinScratchHeightEnabled.Text = "啟用";
            this.cbxThinScratchHeightEnabled.UseVisualStyleBackColor = true;
            // 
            // txbThinScratchHeight
            // 
            this.txbThinScratchHeight.Location = new System.Drawing.Point(89, 213);
            this.txbThinScratchHeight.Name = "txbThinScratchHeight";
            this.txbThinScratchHeight.Size = new System.Drawing.Size(53, 23);
            this.txbThinScratchHeight.TabIndex = 98;
            this.txbThinScratchHeight.Text = "20";
            // 
            // txbThinScratchWidth
            // 
            this.txbThinScratchWidth.Location = new System.Drawing.Point(89, 188);
            this.txbThinScratchWidth.Name = "txbThinScratchWidth";
            this.txbThinScratchWidth.Size = new System.Drawing.Size(53, 23);
            this.txbThinScratchWidth.TabIndex = 98;
            this.txbThinScratchWidth.Text = "20";
            // 
            // cbxThinScratchWidthEnabled
            // 
            this.cbxThinScratchWidthEnabled.AutoSize = true;
            this.cbxThinScratchWidthEnabled.Location = new System.Drawing.Point(212, 191);
            this.cbxThinScratchWidthEnabled.Name = "cbxThinScratchWidthEnabled";
            this.cbxThinScratchWidthEnabled.Size = new System.Drawing.Size(51, 20);
            this.cbxThinScratchWidthEnabled.TabIndex = 99;
            this.cbxThinScratchWidthEnabled.Text = "啟用";
            this.cbxThinScratchWidthEnabled.UseVisualStyleBackColor = true;
            // 
            // nudThinScratchCloseH
            // 
            this.nudThinScratchCloseH.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudThinScratchCloseH.Location = new System.Drawing.Point(285, 127);
            this.nudThinScratchCloseH.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudThinScratchCloseH.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudThinScratchCloseH.Name = "nudThinScratchCloseH";
            this.nudThinScratchCloseH.Size = new System.Drawing.Size(37, 25);
            this.nudThinScratchCloseH.TabIndex = 93;
            this.nudThinScratchCloseH.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudThinScratchCloseW
            // 
            this.nudThinScratchCloseW.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudThinScratchCloseW.Location = new System.Drawing.Point(241, 128);
            this.nudThinScratchCloseW.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudThinScratchCloseW.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudThinScratchCloseW.Name = "nudThinScratchCloseW";
            this.nudThinScratchCloseW.Size = new System.Drawing.Size(38, 25);
            this.nudThinScratchCloseW.TabIndex = 93;
            this.nudThinScratchCloseW.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label64
            // 
            this.label64.AutoSize = true;
            this.label64.Location = new System.Drawing.Point(148, 216);
            this.label64.Name = "label64";
            this.label64.Size = new System.Drawing.Size(35, 16);
            this.label64.TabIndex = 97;
            this.label64.Text = "(um)";
            // 
            // txbThinScratchArea
            // 
            this.txbThinScratchArea.Location = new System.Drawing.Point(89, 163);
            this.txbThinScratchArea.Name = "txbThinScratchArea";
            this.txbThinScratchArea.Size = new System.Drawing.Size(53, 23);
            this.txbThinScratchArea.TabIndex = 98;
            this.txbThinScratchArea.Text = "20";
            // 
            // label56
            // 
            this.label56.AutoSize = true;
            this.label56.Location = new System.Drawing.Point(10, 166);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(65, 16);
            this.label56.TabIndex = 97;
            this.label56.Text = "檢測面積 : ";
            // 
            // cbxThinScratchAreaEnabled
            // 
            this.cbxThinScratchAreaEnabled.AutoSize = true;
            this.cbxThinScratchAreaEnabled.Location = new System.Drawing.Point(212, 166);
            this.cbxThinScratchAreaEnabled.Name = "cbxThinScratchAreaEnabled";
            this.cbxThinScratchAreaEnabled.Size = new System.Drawing.Size(51, 20);
            this.cbxThinScratchAreaEnabled.TabIndex = 99;
            this.cbxThinScratchAreaEnabled.Text = "啟用";
            this.cbxThinScratchAreaEnabled.UseVisualStyleBackColor = true;
            // 
            // nudThinScratchEdgeExHeight
            // 
            this.nudThinScratchEdgeExHeight.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudThinScratchEdgeExHeight.Location = new System.Drawing.Point(159, 76);
            this.nudThinScratchEdgeExHeight.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudThinScratchEdgeExHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudThinScratchEdgeExHeight.Name = "nudThinScratchEdgeExHeight";
            this.nudThinScratchEdgeExHeight.Size = new System.Drawing.Size(33, 25);
            this.nudThinScratchEdgeExHeight.TabIndex = 93;
            this.nudThinScratchEdgeExHeight.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudThinScratchEdgeSkipH
            // 
            this.nudThinScratchEdgeSkipH.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudThinScratchEdgeSkipH.Location = new System.Drawing.Point(159, 104);
            this.nudThinScratchEdgeSkipH.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudThinScratchEdgeSkipH.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudThinScratchEdgeSkipH.Name = "nudThinScratchEdgeSkipH";
            this.nudThinScratchEdgeSkipH.Size = new System.Drawing.Size(33, 25);
            this.nudThinScratchEdgeSkipH.TabIndex = 93;
            this.nudThinScratchEdgeSkipH.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudThinScratchOpenH
            // 
            this.nudThinScratchOpenH.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudThinScratchOpenH.Location = new System.Drawing.Point(285, 100);
            this.nudThinScratchOpenH.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudThinScratchOpenH.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudThinScratchOpenH.Name = "nudThinScratchOpenH";
            this.nudThinScratchOpenH.Size = new System.Drawing.Size(37, 25);
            this.nudThinScratchOpenH.TabIndex = 93;
            this.nudThinScratchOpenH.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudThinScratchOpenW
            // 
            this.nudThinScratchOpenW.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudThinScratchOpenW.Location = new System.Drawing.Point(241, 99);
            this.nudThinScratchOpenW.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudThinScratchOpenW.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudThinScratchOpenW.Name = "nudThinScratchOpenW";
            this.nudThinScratchOpenW.Size = new System.Drawing.Size(38, 25);
            this.nudThinScratchOpenW.TabIndex = 93;
            this.nudThinScratchOpenW.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label71
            // 
            this.label71.AutoSize = true;
            this.label71.Location = new System.Drawing.Point(126, 21);
            this.label71.Name = "label71";
            this.label71.Size = new System.Drawing.Size(48, 16);
            this.label71.TabIndex = 84;
            this.label71.Text = "Band : ";
            // 
            // nudThinScratchEdgeExWidth
            // 
            this.nudThinScratchEdgeExWidth.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudThinScratchEdgeExWidth.Location = new System.Drawing.Point(112, 76);
            this.nudThinScratchEdgeExWidth.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudThinScratchEdgeExWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudThinScratchEdgeExWidth.Name = "nudThinScratchEdgeExWidth";
            this.nudThinScratchEdgeExWidth.Size = new System.Drawing.Size(41, 25);
            this.nudThinScratchEdgeExWidth.TabIndex = 93;
            this.nudThinScratchEdgeExWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label54
            // 
            this.label54.AutoSize = true;
            this.label54.Location = new System.Drawing.Point(10, 107);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(83, 16);
            this.label54.TabIndex = 95;
            this.label54.Text = "邊緣忽略大小:";
            // 
            // nudThinScratchEdgeSkipW
            // 
            this.nudThinScratchEdgeSkipW.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudThinScratchEdgeSkipW.Location = new System.Drawing.Point(112, 104);
            this.nudThinScratchEdgeSkipW.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudThinScratchEdgeSkipW.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudThinScratchEdgeSkipW.Name = "nudThinScratchEdgeSkipW";
            this.nudThinScratchEdgeSkipW.Size = new System.Drawing.Size(41, 25);
            this.nudThinScratchEdgeSkipW.TabIndex = 93;
            this.nudThinScratchEdgeSkipW.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label63
            // 
            this.label63.AutoSize = true;
            this.label63.Location = new System.Drawing.Point(148, 191);
            this.label63.Name = "label63";
            this.label63.Size = new System.Drawing.Size(35, 16);
            this.label63.TabIndex = 97;
            this.label63.Text = "(um)";
            // 
            // label57
            // 
            this.label57.AutoSize = true;
            this.label57.Location = new System.Drawing.Point(202, 132);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(38, 16);
            this.label57.TabIndex = 95;
            this.label57.Text = "閉合 :";
            // 
            // label58
            // 
            this.label58.AutoSize = true;
            this.label58.Location = new System.Drawing.Point(202, 104);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(38, 16);
            this.label58.TabIndex = 95;
            this.label58.Text = "斷開 :";
            // 
            // label60
            // 
            this.label60.AutoSize = true;
            this.label60.Location = new System.Drawing.Point(148, 166);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(51, 16);
            this.label60.TabIndex = 97;
            this.label60.Text = "(um^2)";
            // 
            // label59
            // 
            this.label59.AutoSize = true;
            this.label59.Location = new System.Drawing.Point(10, 191);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(65, 16);
            this.label59.TabIndex = 97;
            this.label59.Text = "檢測寬度 : ";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbxThinHatEnabled);
            this.groupBox1.Controls.Add(this.cbxThinHatHeightEnabled);
            this.groupBox1.Controls.Add(this.txbThinHatHeightMin);
            this.groupBox1.Controls.Add(this.nudThinHatDarkTh);
            this.groupBox1.Controls.Add(this.btnThinHatRegionTest);
            this.groupBox1.Controls.Add(this.txbThinHatWidthMin);
            this.groupBox1.Controls.Add(this.cbxThinHatWidthEnabled);
            this.groupBox1.Controls.Add(this.label37);
            this.groupBox1.Controls.Add(this.txbThinHatAreaMin);
            this.groupBox1.Controls.Add(this.label40);
            this.groupBox1.Controls.Add(this.cbxThinHatAreaEnabled);
            this.groupBox1.Controls.Add(this.label41);
            this.groupBox1.Controls.Add(this.nudThinHatBrightTh);
            this.groupBox1.Controls.Add(this.label42);
            this.groupBox1.Controls.Add(this.label43);
            this.groupBox1.Controls.Add(this.label39);
            this.groupBox1.Controls.Add(this.label44);
            this.groupBox1.Controls.Add(this.label38);
            this.groupBox1.Controls.Add(this.nudThinHatOpenWidth);
            this.groupBox1.Controls.Add(this.label45);
            this.groupBox1.Controls.Add(this.nudThinHatCloseHeight);
            this.groupBox1.Controls.Add(this.nudThinHatSkipH);
            this.groupBox1.Controls.Add(this.nudThinHatCloseWidth);
            this.groupBox1.Controls.Add(this.label46);
            this.groupBox1.Controls.Add(this.nudThinHatSkipW);
            this.groupBox1.Controls.Add(this.nudThinHatOpenHeight);
            this.groupBox1.Controls.Add(this.label48);
            this.groupBox1.Location = new System.Drawing.Point(1176, 40);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(315, 247);
            this.groupBox1.TabIndex = 103;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "薄化偵測";
            // 
            // cbxThinHatEnabled
            // 
            this.cbxThinHatEnabled.AutoSize = true;
            this.cbxThinHatEnabled.Location = new System.Drawing.Point(13, 27);
            this.cbxThinHatEnabled.Name = "cbxThinHatEnabled";
            this.cbxThinHatEnabled.Size = new System.Drawing.Size(51, 20);
            this.cbxThinHatEnabled.TabIndex = 100;
            this.cbxThinHatEnabled.Text = "啟用";
            this.cbxThinHatEnabled.UseVisualStyleBackColor = true;
            // 
            // cbxThinHatHeightEnabled
            // 
            this.cbxThinHatHeightEnabled.AutoSize = true;
            this.cbxThinHatHeightEnabled.Location = new System.Drawing.Point(203, 205);
            this.cbxThinHatHeightEnabled.Name = "cbxThinHatHeightEnabled";
            this.cbxThinHatHeightEnabled.Size = new System.Drawing.Size(51, 20);
            this.cbxThinHatHeightEnabled.TabIndex = 99;
            this.cbxThinHatHeightEnabled.Text = "啟用";
            this.cbxThinHatHeightEnabled.UseVisualStyleBackColor = true;
            // 
            // txbThinHatHeightMin
            // 
            this.txbThinHatHeightMin.Location = new System.Drawing.Point(80, 201);
            this.txbThinHatHeightMin.Name = "txbThinHatHeightMin";
            this.txbThinHatHeightMin.Size = new System.Drawing.Size(53, 23);
            this.txbThinHatHeightMin.TabIndex = 98;
            this.txbThinHatHeightMin.Text = "20";
            // 
            // nudThinHatDarkTh
            // 
            this.nudThinHatDarkTh.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudThinHatDarkTh.Location = new System.Drawing.Point(81, 50);
            this.nudThinHatDarkTh.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudThinHatDarkTh.Name = "nudThinHatDarkTh";
            this.nudThinHatDarkTh.Size = new System.Drawing.Size(54, 25);
            this.nudThinHatDarkTh.TabIndex = 94;
            // 
            // btnThinHatRegionTest
            // 
            this.btnThinHatRegionTest.BackColor = System.Drawing.Color.LightGray;
            this.btnThinHatRegionTest.Location = new System.Drawing.Point(230, 112);
            this.btnThinHatRegionTest.Name = "btnThinHatRegionTest";
            this.btnThinHatRegionTest.Size = new System.Drawing.Size(77, 26);
            this.btnThinHatRegionTest.TabIndex = 89;
            this.btnThinHatRegionTest.Text = "範圍測試";
            this.btnThinHatRegionTest.UseVisualStyleBackColor = false;
            this.btnThinHatRegionTest.Click += new System.EventHandler(this.btnThinHatRegionTest_Click);
            // 
            // txbThinHatWidthMin
            // 
            this.txbThinHatWidthMin.Location = new System.Drawing.Point(80, 171);
            this.txbThinHatWidthMin.Name = "txbThinHatWidthMin";
            this.txbThinHatWidthMin.Size = new System.Drawing.Size(53, 23);
            this.txbThinHatWidthMin.TabIndex = 98;
            this.txbThinHatWidthMin.Text = "20";
            // 
            // cbxThinHatWidthEnabled
            // 
            this.cbxThinHatWidthEnabled.AutoSize = true;
            this.cbxThinHatWidthEnabled.Location = new System.Drawing.Point(203, 175);
            this.cbxThinHatWidthEnabled.Name = "cbxThinHatWidthEnabled";
            this.cbxThinHatWidthEnabled.Size = new System.Drawing.Size(51, 20);
            this.cbxThinHatWidthEnabled.TabIndex = 99;
            this.cbxThinHatWidthEnabled.Text = "啟用";
            this.cbxThinHatWidthEnabled.UseVisualStyleBackColor = true;
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(139, 204);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(35, 16);
            this.label37.TabIndex = 97;
            this.label37.Text = "(um)";
            // 
            // txbThinHatAreaMin
            // 
            this.txbThinHatAreaMin.Location = new System.Drawing.Point(80, 143);
            this.txbThinHatAreaMin.Name = "txbThinHatAreaMin";
            this.txbThinHatAreaMin.Size = new System.Drawing.Size(53, 23);
            this.txbThinHatAreaMin.TabIndex = 98;
            this.txbThinHatAreaMin.Text = "20";
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(10, 53);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(59, 16);
            this.label40.TabIndex = 96;
            this.label40.Text = "暗區閥值:";
            // 
            // cbxThinHatAreaEnabled
            // 
            this.cbxThinHatAreaEnabled.AutoSize = true;
            this.cbxThinHatAreaEnabled.Location = new System.Drawing.Point(203, 144);
            this.cbxThinHatAreaEnabled.Name = "cbxThinHatAreaEnabled";
            this.cbxThinHatAreaEnabled.Size = new System.Drawing.Size(51, 20);
            this.cbxThinHatAreaEnabled.TabIndex = 99;
            this.cbxThinHatAreaEnabled.Text = "啟用";
            this.cbxThinHatAreaEnabled.UseVisualStyleBackColor = true;
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(139, 174);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(35, 16);
            this.label41.TabIndex = 97;
            this.label41.Text = "(um)";
            // 
            // nudThinHatBrightTh
            // 
            this.nudThinHatBrightTh.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudThinHatBrightTh.Location = new System.Drawing.Point(82, 84);
            this.nudThinHatBrightTh.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudThinHatBrightTh.Name = "nudThinHatBrightTh";
            this.nudThinHatBrightTh.Size = new System.Drawing.Size(53, 25);
            this.nudThinHatBrightTh.TabIndex = 93;
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(139, 146);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(51, 16);
            this.label42.TabIndex = 97;
            this.label42.Text = "(um^2)";
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(10, 204);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(65, 16);
            this.label43.TabIndex = 97;
            this.label43.Text = "檢測高度 : ";
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(141, 54);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(38, 16);
            this.label39.TabIndex = 95;
            this.label39.Text = "侵蝕 :";
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Location = new System.Drawing.Point(10, 86);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(59, 16);
            this.label44.TabIndex = 95;
            this.label44.Text = "亮區閥值:";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(141, 84);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(38, 16);
            this.label38.TabIndex = 95;
            this.label38.Text = "膨脹 :";
            // 
            // nudThinHatOpenWidth
            // 
            this.nudThinHatOpenWidth.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudThinHatOpenWidth.Location = new System.Drawing.Point(185, 52);
            this.nudThinHatOpenWidth.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudThinHatOpenWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudThinHatOpenWidth.Name = "nudThinHatOpenWidth";
            this.nudThinHatOpenWidth.Size = new System.Drawing.Size(53, 25);
            this.nudThinHatOpenWidth.TabIndex = 93;
            this.nudThinHatOpenWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Location = new System.Drawing.Point(10, 146);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(65, 16);
            this.label45.TabIndex = 97;
            this.label45.Text = "檢測面積 : ";
            // 
            // nudThinHatCloseHeight
            // 
            this.nudThinHatCloseHeight.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudThinHatCloseHeight.Location = new System.Drawing.Point(244, 82);
            this.nudThinHatCloseHeight.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudThinHatCloseHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudThinHatCloseHeight.Name = "nudThinHatCloseHeight";
            this.nudThinHatCloseHeight.Size = new System.Drawing.Size(53, 25);
            this.nudThinHatCloseHeight.TabIndex = 93;
            this.nudThinHatCloseHeight.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudThinHatSkipH
            // 
            this.nudThinHatSkipH.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudThinHatSkipH.Location = new System.Drawing.Point(171, 112);
            this.nudThinHatSkipH.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudThinHatSkipH.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudThinHatSkipH.Name = "nudThinHatSkipH";
            this.nudThinHatSkipH.Size = new System.Drawing.Size(53, 25);
            this.nudThinHatSkipH.TabIndex = 93;
            this.nudThinHatSkipH.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudThinHatCloseWidth
            // 
            this.nudThinHatCloseWidth.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudThinHatCloseWidth.Location = new System.Drawing.Point(185, 82);
            this.nudThinHatCloseWidth.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudThinHatCloseWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudThinHatCloseWidth.Name = "nudThinHatCloseWidth";
            this.nudThinHatCloseWidth.Size = new System.Drawing.Size(53, 25);
            this.nudThinHatCloseWidth.TabIndex = 93;
            this.nudThinHatCloseWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Location = new System.Drawing.Point(10, 174);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(65, 16);
            this.label46.TabIndex = 97;
            this.label46.Text = "檢測寬度 : ";
            // 
            // nudThinHatSkipW
            // 
            this.nudThinHatSkipW.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudThinHatSkipW.Location = new System.Drawing.Point(112, 112);
            this.nudThinHatSkipW.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudThinHatSkipW.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudThinHatSkipW.Name = "nudThinHatSkipW";
            this.nudThinHatSkipW.Size = new System.Drawing.Size(53, 25);
            this.nudThinHatSkipW.TabIndex = 93;
            this.nudThinHatSkipW.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudThinHatOpenHeight
            // 
            this.nudThinHatOpenHeight.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudThinHatOpenHeight.Location = new System.Drawing.Point(244, 52);
            this.nudThinHatOpenHeight.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudThinHatOpenHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudThinHatOpenHeight.Name = "nudThinHatOpenHeight";
            this.nudThinHatOpenHeight.Size = new System.Drawing.Size(53, 25);
            this.nudThinHatOpenHeight.TabIndex = 93;
            this.nudThinHatOpenHeight.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Location = new System.Drawing.Point(10, 115);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(83, 16);
            this.label48.TabIndex = 95;
            this.label48.Text = "邊緣縮減大小:";
            // 
            // comboxModeSelect
            // 
            this.comboxModeSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxModeSelect.FormattingEnabled = true;
            this.comboxModeSelect.Items.AddRange(new object[] {
            "Dark",
            "Bright"});
            this.comboxModeSelect.Location = new System.Drawing.Point(1594, 7);
            this.comboxModeSelect.Name = "comboxModeSelect";
            this.comboxModeSelect.Size = new System.Drawing.Size(65, 24);
            this.comboxModeSelect.TabIndex = 123;
            this.comboxModeSelect.SelectedIndexChanged += new System.EventHandler(this.comboxModeSelect_SelectedIndexChanged);
            // 
            // gbxHistoEq
            // 
            this.gbxHistoEq.Controls.Add(this.btnSettingHistoEqTh);
            this.gbxHistoEq.Controls.Add(this.cbxHistoEqEnabled);
            this.gbxHistoEq.Controls.Add(this.label26);
            this.gbxHistoEq.Controls.Add(this.cbxHistoHeightEnabled);
            this.gbxHistoEq.Controls.Add(this.label23);
            this.gbxHistoEq.Controls.Add(this.nudHistoEdgeSkipTest);
            this.gbxHistoEq.Controls.Add(this.txbHistoHeightMin);
            this.gbxHistoEq.Controls.Add(this.nudHistoEqTh);
            this.gbxHistoEq.Controls.Add(this.txbHistoWidthMin);
            this.gbxHistoEq.Controls.Add(this.label24);
            this.gbxHistoEq.Controls.Add(this.label25);
            this.gbxHistoEq.Controls.Add(this.cbxHistoWidthEnabled);
            this.gbxHistoEq.Controls.Add(this.label27);
            this.gbxHistoEq.Controls.Add(this.label28);
            this.gbxHistoEq.Controls.Add(this.txbHistoAreaMin);
            this.gbxHistoEq.Controls.Add(this.cbxHistoAreaEnabled);
            this.gbxHistoEq.Controls.Add(this.label29);
            this.gbxHistoEq.Controls.Add(this.label30);
            this.gbxHistoEq.Controls.Add(this.nudHistoOpenWidth);
            this.gbxHistoEq.Controls.Add(this.nudHistoCloseHeight);
            this.gbxHistoEq.Controls.Add(this.nudHistoCloseWidth);
            this.gbxHistoEq.Controls.Add(this.nudHistoEdgeSkipH);
            this.gbxHistoEq.Controls.Add(this.nudHistoOpenHeight);
            this.gbxHistoEq.Controls.Add(this.label34);
            this.gbxHistoEq.Controls.Add(this.nudHistoEdgeSkipW);
            this.gbxHistoEq.Location = new System.Drawing.Point(914, 40);
            this.gbxHistoEq.Name = "gbxHistoEq";
            this.gbxHistoEq.Size = new System.Drawing.Size(256, 247);
            this.gbxHistoEq.TabIndex = 102;
            this.gbxHistoEq.TabStop = false;
            this.gbxHistoEq.Text = "對比強化";
            // 
            // btnSettingHistoEqTh
            // 
            this.btnSettingHistoEqTh.Location = new System.Drawing.Point(111, 44);
            this.btnSettingHistoEqTh.Name = "btnSettingHistoEqTh";
            this.btnSettingHistoEqTh.Size = new System.Drawing.Size(75, 28);
            this.btnSettingHistoEqTh.TabIndex = 101;
            this.btnSettingHistoEqTh.Text = "設定";
            this.btnSettingHistoEqTh.UseVisualStyleBackColor = true;
            this.btnSettingHistoEqTh.Visible = false;
            this.btnSettingHistoEqTh.Click += new System.EventHandler(this.btnSettingHistoEqTh_Click);
            // 
            // cbxHistoEqEnabled
            // 
            this.cbxHistoEqEnabled.AutoSize = true;
            this.cbxHistoEqEnabled.Location = new System.Drawing.Point(9, 22);
            this.cbxHistoEqEnabled.Name = "cbxHistoEqEnabled";
            this.cbxHistoEqEnabled.Size = new System.Drawing.Size(51, 20);
            this.cbxHistoEqEnabled.TabIndex = 100;
            this.cbxHistoEqEnabled.Text = "啟用";
            this.cbxHistoEqEnabled.UseVisualStyleBackColor = true;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(7, 224);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(65, 16);
            this.label26.TabIndex = 97;
            this.label26.Text = "檢測高度 : ";
            // 
            // cbxHistoHeightEnabled
            // 
            this.cbxHistoHeightEnabled.AutoSize = true;
            this.cbxHistoHeightEnabled.Location = new System.Drawing.Point(203, 224);
            this.cbxHistoHeightEnabled.Name = "cbxHistoHeightEnabled";
            this.cbxHistoHeightEnabled.Size = new System.Drawing.Size(51, 20);
            this.cbxHistoHeightEnabled.TabIndex = 99;
            this.cbxHistoHeightEnabled.Text = "啟用";
            this.cbxHistoHeightEnabled.UseVisualStyleBackColor = true;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(7, 163);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(65, 16);
            this.label23.TabIndex = 97;
            this.label23.Text = "檢測面積 : ";
            // 
            // nudHistoEdgeSkipTest
            // 
            this.nudHistoEdgeSkipTest.BackColor = System.Drawing.Color.LightGray;
            this.nudHistoEdgeSkipTest.Location = new System.Drawing.Point(179, 102);
            this.nudHistoEdgeSkipTest.Name = "nudHistoEdgeSkipTest";
            this.nudHistoEdgeSkipTest.Size = new System.Drawing.Size(77, 26);
            this.nudHistoEdgeSkipTest.TabIndex = 89;
            this.nudHistoEdgeSkipTest.Text = "範圍測試";
            this.nudHistoEdgeSkipTest.UseVisualStyleBackColor = false;
            this.nudHistoEdgeSkipTest.Click += new System.EventHandler(this.nudHistoEdgeSkipTest_Click);
            // 
            // txbHistoHeightMin
            // 
            this.txbHistoHeightMin.Location = new System.Drawing.Point(86, 221);
            this.txbHistoHeightMin.Name = "txbHistoHeightMin";
            this.txbHistoHeightMin.Size = new System.Drawing.Size(53, 23);
            this.txbHistoHeightMin.TabIndex = 98;
            this.txbHistoHeightMin.Text = "20";
            // 
            // nudHistoEqTh
            // 
            this.nudHistoEqTh.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudHistoEqTh.Location = new System.Drawing.Point(51, 47);
            this.nudHistoEqTh.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudHistoEqTh.Name = "nudHistoEqTh";
            this.nudHistoEqTh.Size = new System.Drawing.Size(54, 25);
            this.nudHistoEqTh.TabIndex = 94;
            // 
            // txbHistoWidthMin
            // 
            this.txbHistoWidthMin.Location = new System.Drawing.Point(86, 191);
            this.txbHistoWidthMin.Name = "txbHistoWidthMin";
            this.txbHistoWidthMin.Size = new System.Drawing.Size(53, 23);
            this.txbHistoWidthMin.TabIndex = 98;
            this.txbHistoWidthMin.Text = "20";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(7, 194);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(65, 16);
            this.label24.TabIndex = 97;
            this.label24.Text = "檢測寬度 : ";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(145, 163);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(51, 16);
            this.label25.TabIndex = 97;
            this.label25.Text = "(um^2)";
            // 
            // cbxHistoWidthEnabled
            // 
            this.cbxHistoWidthEnabled.AutoSize = true;
            this.cbxHistoWidthEnabled.Location = new System.Drawing.Point(203, 194);
            this.cbxHistoWidthEnabled.Name = "cbxHistoWidthEnabled";
            this.cbxHistoWidthEnabled.Size = new System.Drawing.Size(51, 20);
            this.cbxHistoWidthEnabled.TabIndex = 99;
            this.cbxHistoWidthEnabled.Text = "啟用";
            this.cbxHistoWidthEnabled.UseVisualStyleBackColor = true;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(6, 50);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(35, 16);
            this.label27.TabIndex = 96;
            this.label27.Text = "閥值:";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(145, 194);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(35, 16);
            this.label28.TabIndex = 97;
            this.label28.Text = "(um)";
            // 
            // txbHistoAreaMin
            // 
            this.txbHistoAreaMin.Location = new System.Drawing.Point(86, 160);
            this.txbHistoAreaMin.Name = "txbHistoAreaMin";
            this.txbHistoAreaMin.Size = new System.Drawing.Size(53, 23);
            this.txbHistoAreaMin.TabIndex = 98;
            this.txbHistoAreaMin.Text = "20";
            // 
            // cbxHistoAreaEnabled
            // 
            this.cbxHistoAreaEnabled.AutoSize = true;
            this.cbxHistoAreaEnabled.Location = new System.Drawing.Point(203, 163);
            this.cbxHistoAreaEnabled.Name = "cbxHistoAreaEnabled";
            this.cbxHistoAreaEnabled.Size = new System.Drawing.Size(51, 20);
            this.cbxHistoAreaEnabled.TabIndex = 99;
            this.cbxHistoAreaEnabled.Text = "啟用";
            this.cbxHistoAreaEnabled.UseVisualStyleBackColor = true;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(7, 104);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(38, 16);
            this.label29.TabIndex = 95;
            this.label29.Text = "斷開 :";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(7, 134);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(38, 16);
            this.label30.TabIndex = 95;
            this.label30.Text = "閉合 :";
            // 
            // nudHistoOpenWidth
            // 
            this.nudHistoOpenWidth.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudHistoOpenWidth.Location = new System.Drawing.Point(65, 102);
            this.nudHistoOpenWidth.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudHistoOpenWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudHistoOpenWidth.Name = "nudHistoOpenWidth";
            this.nudHistoOpenWidth.Size = new System.Drawing.Size(53, 25);
            this.nudHistoOpenWidth.TabIndex = 93;
            this.nudHistoOpenWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudHistoCloseHeight
            // 
            this.nudHistoCloseHeight.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudHistoCloseHeight.Location = new System.Drawing.Point(124, 132);
            this.nudHistoCloseHeight.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudHistoCloseHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudHistoCloseHeight.Name = "nudHistoCloseHeight";
            this.nudHistoCloseHeight.Size = new System.Drawing.Size(53, 25);
            this.nudHistoCloseHeight.TabIndex = 93;
            this.nudHistoCloseHeight.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudHistoCloseWidth
            // 
            this.nudHistoCloseWidth.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudHistoCloseWidth.Location = new System.Drawing.Point(65, 132);
            this.nudHistoCloseWidth.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudHistoCloseWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudHistoCloseWidth.Name = "nudHistoCloseWidth";
            this.nudHistoCloseWidth.Size = new System.Drawing.Size(53, 25);
            this.nudHistoCloseWidth.TabIndex = 93;
            this.nudHistoCloseWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudHistoEdgeSkipH
            // 
            this.nudHistoEdgeSkipH.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudHistoEdgeSkipH.Location = new System.Drawing.Point(168, 75);
            this.nudHistoEdgeSkipH.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudHistoEdgeSkipH.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudHistoEdgeSkipH.Name = "nudHistoEdgeSkipH";
            this.nudHistoEdgeSkipH.Size = new System.Drawing.Size(53, 25);
            this.nudHistoEdgeSkipH.TabIndex = 93;
            this.nudHistoEdgeSkipH.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudHistoOpenHeight
            // 
            this.nudHistoOpenHeight.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudHistoOpenHeight.Location = new System.Drawing.Point(124, 102);
            this.nudHistoOpenHeight.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudHistoOpenHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudHistoOpenHeight.Name = "nudHistoOpenHeight";
            this.nudHistoOpenHeight.Size = new System.Drawing.Size(53, 25);
            this.nudHistoOpenHeight.TabIndex = 93;
            this.nudHistoOpenHeight.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(7, 78);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(83, 16);
            this.label34.TabIndex = 95;
            this.label34.Text = "邊緣忽略大小:";
            // 
            // nudHistoEdgeSkipW
            // 
            this.nudHistoEdgeSkipW.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudHistoEdgeSkipW.Location = new System.Drawing.Point(109, 75);
            this.nudHistoEdgeSkipW.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudHistoEdgeSkipW.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudHistoEdgeSkipW.Name = "nudHistoEdgeSkipW";
            this.nudHistoEdgeSkipW.Size = new System.Drawing.Size(53, 25);
            this.nudHistoEdgeSkipW.TabIndex = 93;
            this.nudHistoEdgeSkipW.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnSaveMode
            // 
            this.btnSaveMode.Enabled = false;
            this.btnSaveMode.Location = new System.Drawing.Point(1497, 8);
            this.btnSaveMode.Name = "btnSaveMode";
            this.btnSaveMode.Size = new System.Drawing.Size(91, 23);
            this.btnSaveMode.TabIndex = 122;
            this.btnSaveMode.Text = "儲存Mode";
            this.btnSaveMode.UseVisualStyleBackColor = true;
            this.btnSaveMode.Click += new System.EventHandler(this.btnSaveMode_Click);
            // 
            // txbThinName
            // 
            this.txbThinName.Location = new System.Drawing.Point(103, 11);
            this.txbThinName.Name = "txbThinName";
            this.txbThinName.Size = new System.Drawing.Size(100, 23);
            this.txbThinName.TabIndex = 99;
            this.txbThinName.Text = "Defect";
            // 
            // cbxThinEnabled
            // 
            this.cbxThinEnabled.AutoSize = true;
            this.cbxThinEnabled.Location = new System.Drawing.Point(12, 12);
            this.cbxThinEnabled.Name = "cbxThinEnabled";
            this.cbxThinEnabled.Size = new System.Drawing.Size(75, 20);
            this.cbxThinEnabled.TabIndex = 98;
            this.cbxThinEnabled.Text = "檢測啟用";
            this.cbxThinEnabled.UseVisualStyleBackColor = true;
            // 
            // gbxThinParam
            // 
            this.gbxThinParam.Controls.Add(this.gbxAvgTest);
            this.gbxThinParam.Controls.Add(this.gbxSumArea);
            this.gbxThinParam.Controls.Add(this.labThinID);
            this.gbxThinParam.Controls.Add(this.labPartGrayValue);
            this.gbxThinParam.Controls.Add(this.txbPartGrayValue);
            this.gbxThinParam.Controls.Add(this.txbThinID);
            this.gbxThinParam.Location = new System.Drawing.Point(12, 40);
            this.gbxThinParam.Name = "gbxThinParam";
            this.gbxThinParam.Size = new System.Drawing.Size(255, 247);
            this.gbxThinParam.TabIndex = 97;
            this.gbxThinParam.TabStop = false;
            this.gbxThinParam.Text = "薄化參數設定";
            // 
            // gbxAvgTest
            // 
            this.gbxAvgTest.Controls.Add(this.btnTestAverage);
            this.gbxAvgTest.Controls.Add(this.labMin);
            this.gbxAvgTest.Controls.Add(this.labMid);
            this.gbxAvgTest.Controls.Add(this.labMax);
            this.gbxAvgTest.Location = new System.Drawing.Point(6, 84);
            this.gbxAvgTest.Name = "gbxAvgTest";
            this.gbxAvgTest.Size = new System.Drawing.Size(244, 91);
            this.gbxAvgTest.TabIndex = 103;
            this.gbxAvgTest.TabStop = false;
            this.gbxAvgTest.Text = "灰階測試";
            // 
            // btnTestAverage
            // 
            this.btnTestAverage.Location = new System.Drawing.Point(50, 23);
            this.btnTestAverage.Name = "btnTestAverage";
            this.btnTestAverage.Size = new System.Drawing.Size(124, 32);
            this.btnTestAverage.TabIndex = 1;
            this.btnTestAverage.Text = "灰階測試";
            this.btnTestAverage.UseVisualStyleBackColor = true;
            this.btnTestAverage.Click += new System.EventHandler(this.btnTestAverage_Click);
            // 
            // labMin
            // 
            this.labMin.AutoSize = true;
            this.labMin.Location = new System.Drawing.Point(181, 64);
            this.labMin.Name = "labMin";
            this.labMin.Size = new System.Drawing.Size(31, 16);
            this.labMin.TabIndex = 0;
            this.labMin.Text = "Min";
            // 
            // labMid
            // 
            this.labMid.AutoSize = true;
            this.labMid.Location = new System.Drawing.Point(94, 65);
            this.labMid.Name = "labMid";
            this.labMid.Size = new System.Drawing.Size(31, 16);
            this.labMid.TabIndex = 0;
            this.labMid.Text = "Mid";
            // 
            // labMax
            // 
            this.labMax.AutoSize = true;
            this.labMax.Location = new System.Drawing.Point(6, 64);
            this.labMax.Name = "labMax";
            this.labMax.Size = new System.Drawing.Size(33, 16);
            this.labMax.TabIndex = 0;
            this.labMax.Text = "Max";
            // 
            // gbxSumArea
            // 
            this.gbxSumArea.Controls.Add(this.txbSumArea);
            this.gbxSumArea.Controls.Add(this.labSumArea);
            this.gbxSumArea.Controls.Add(this.cbxSumAreaEnabled);
            this.gbxSumArea.Controls.Add(this.label19);
            this.gbxSumArea.Location = new System.Drawing.Point(6, 177);
            this.gbxSumArea.Name = "gbxSumArea";
            this.gbxSumArea.Size = new System.Drawing.Size(244, 63);
            this.gbxSumArea.TabIndex = 102;
            this.gbxSumArea.TabStop = false;
            this.gbxSumArea.Text = "檢測規格";
            // 
            // txbSumArea
            // 
            this.txbSumArea.Location = new System.Drawing.Point(67, 23);
            this.txbSumArea.Name = "txbSumArea";
            this.txbSumArea.Size = new System.Drawing.Size(53, 23);
            this.txbSumArea.TabIndex = 104;
            this.txbSumArea.Text = "20";
            // 
            // labSumArea
            // 
            this.labSumArea.AutoSize = true;
            this.labSumArea.Location = new System.Drawing.Point(7, 26);
            this.labSumArea.Name = "labSumArea";
            this.labSumArea.Size = new System.Drawing.Size(53, 16);
            this.labSumArea.TabIndex = 103;
            this.labSumArea.Text = "總面積 : ";
            // 
            // cbxSumAreaEnabled
            // 
            this.cbxSumAreaEnabled.AutoSize = true;
            this.cbxSumAreaEnabled.Location = new System.Drawing.Point(184, 26);
            this.cbxSumAreaEnabled.Name = "cbxSumAreaEnabled";
            this.cbxSumAreaEnabled.Size = new System.Drawing.Size(51, 20);
            this.cbxSumAreaEnabled.TabIndex = 105;
            this.cbxSumAreaEnabled.Text = "啟用";
            this.cbxSumAreaEnabled.UseVisualStyleBackColor = true;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(125, 26);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(51, 16);
            this.label19.TabIndex = 102;
            this.label19.Text = "(um^2)";
            // 
            // labThinID
            // 
            this.labThinID.AutoSize = true;
            this.labThinID.Location = new System.Drawing.Point(6, 32);
            this.labThinID.Name = "labThinID";
            this.labThinID.Size = new System.Drawing.Size(54, 16);
            this.labThinID.TabIndex = 91;
            this.labThinID.Text = "影像ID : ";
            // 
            // labPartGrayValue
            // 
            this.labPartGrayValue.AutoSize = true;
            this.labPartGrayValue.Location = new System.Drawing.Point(6, 60);
            this.labPartGrayValue.Name = "labPartGrayValue";
            this.labPartGrayValue.Size = new System.Drawing.Size(77, 16);
            this.labPartGrayValue.TabIndex = 8;
            this.labPartGrayValue.Text = "分區灰階值 : ";
            // 
            // txbPartGrayValue
            // 
            this.txbPartGrayValue.Location = new System.Drawing.Point(103, 57);
            this.txbPartGrayValue.Name = "txbPartGrayValue";
            this.txbPartGrayValue.Size = new System.Drawing.Size(54, 23);
            this.txbPartGrayValue.TabIndex = 7;
            this.txbPartGrayValue.Text = "100";
            // 
            // txbThinID
            // 
            this.txbThinID.Location = new System.Drawing.Point(75, 29);
            this.txbThinID.Name = "txbThinID";
            this.txbThinID.Size = new System.Drawing.Size(54, 23);
            this.txbThinID.TabIndex = 90;
            this.txbThinID.Text = "0";
            // 
            // btnThinPartitionTest
            // 
            this.btnThinPartitionTest.BackColor = System.Drawing.Color.LightGray;
            this.btnThinPartitionTest.Location = new System.Drawing.Point(573, 3);
            this.btnThinPartitionTest.Name = "btnThinPartitionTest";
            this.btnThinPartitionTest.Size = new System.Drawing.Size(196, 32);
            this.btnThinPartitionTest.TabIndex = 89;
            this.btnThinPartitionTest.Text = "分區測試";
            this.btnThinPartitionTest.UseVisualStyleBackColor = false;
            this.btnThinPartitionTest.Click += new System.EventHandler(this.btnPartitionTest_Click);
            // 
            // gbxSmallArea
            // 
            this.gbxSmallArea.Controls.Add(this.cbxTopHatEnabled);
            this.gbxSmallArea.Controls.Add(this.cbxTophatHEnabled);
            this.gbxSmallArea.Controls.Add(this.txbtophatH);
            this.gbxSmallArea.Controls.Add(this.nudTophatDarkTh);
            this.gbxSmallArea.Controls.Add(this.txbHoleName);
            this.gbxSmallArea.Controls.Add(this.btnBrightRegionTh);
            this.gbxSmallArea.Controls.Add(this.btnTopHatEdgeExpTest);
            this.gbxSmallArea.Controls.Add(this.txbTophatW);
            this.gbxSmallArea.Controls.Add(this.cbxTophatWEnabled);
            this.gbxSmallArea.Controls.Add(this.labThinTopHatum2);
            this.gbxSmallArea.Controls.Add(this.btnHoleInsp);
            this.gbxSmallArea.Controls.Add(this.txbTophatMinArea);
            this.gbxSmallArea.Controls.Add(this.label36);
            this.gbxSmallArea.Controls.Add(this.label35);
            this.gbxSmallArea.Controls.Add(this.labTophatDarkTh);
            this.gbxSmallArea.Controls.Add(this.cbxTophatAreaEnabled);
            this.gbxSmallArea.Controls.Add(this.labThinTopHatum1);
            this.gbxSmallArea.Controls.Add(this.nudTophatBrightTh);
            this.gbxSmallArea.Controls.Add(this.labTophatum2);
            this.gbxSmallArea.Controls.Add(this.labTopHatH);
            this.gbxSmallArea.Controls.Add(this.labTophatBrightTh);
            this.gbxSmallArea.Controls.Add(this.labTophatMinArea);
            this.gbxSmallArea.Controls.Add(this.nudSESizeHeight);
            this.gbxSmallArea.Controls.Add(this.nudTopHatEdgeExpHeight);
            this.gbxSmallArea.Controls.Add(this.labTopHatW);
            this.gbxSmallArea.Controls.Add(this.nudExtHTh);
            this.gbxSmallArea.Controls.Add(this.nudSESizeWidth);
            this.gbxSmallArea.Controls.Add(this.nudTopHatEdgeExpWidth);
            this.gbxSmallArea.Controls.Add(this.labSESize);
            this.gbxSmallArea.Controls.Add(this.nudExtH);
            this.gbxSmallArea.Controls.Add(this.labTopHatEdgeExp);
            this.gbxSmallArea.Location = new System.Drawing.Point(273, 40);
            this.gbxSmallArea.Name = "gbxSmallArea";
            this.gbxSmallArea.Size = new System.Drawing.Size(335, 247);
            this.gbxSmallArea.TabIndex = 100;
            this.gbxSmallArea.TabStop = false;
            this.gbxSmallArea.Text = "孔洞偵測";
            // 
            // cbxTopHatEnabled
            // 
            this.cbxTopHatEnabled.AutoSize = true;
            this.cbxTopHatEnabled.Location = new System.Drawing.Point(19, 27);
            this.cbxTopHatEnabled.Name = "cbxTopHatEnabled";
            this.cbxTopHatEnabled.Size = new System.Drawing.Size(51, 20);
            this.cbxTopHatEnabled.TabIndex = 100;
            this.cbxTopHatEnabled.Text = "啟用";
            this.cbxTopHatEnabled.UseVisualStyleBackColor = true;
            // 
            // cbxTophatHEnabled
            // 
            this.cbxTophatHEnabled.AutoSize = true;
            this.cbxTophatHEnabled.Location = new System.Drawing.Point(209, 225);
            this.cbxTophatHEnabled.Name = "cbxTophatHEnabled";
            this.cbxTophatHEnabled.Size = new System.Drawing.Size(51, 20);
            this.cbxTophatHEnabled.TabIndex = 99;
            this.cbxTophatHEnabled.Text = "啟用";
            this.cbxTophatHEnabled.UseVisualStyleBackColor = true;
            // 
            // txbtophatH
            // 
            this.txbtophatH.Location = new System.Drawing.Point(86, 221);
            this.txbtophatH.Name = "txbtophatH";
            this.txbtophatH.Size = new System.Drawing.Size(53, 23);
            this.txbtophatH.TabIndex = 98;
            this.txbtophatH.Text = "20";
            // 
            // nudTophatDarkTh
            // 
            this.nudTophatDarkTh.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudTophatDarkTh.Location = new System.Drawing.Point(87, 50);
            this.nudTophatDarkTh.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudTophatDarkTh.Name = "nudTophatDarkTh";
            this.nudTophatDarkTh.Size = new System.Drawing.Size(54, 25);
            this.nudTophatDarkTh.TabIndex = 94;
            // 
            // txbHoleName
            // 
            this.txbHoleName.Location = new System.Drawing.Point(73, 24);
            this.txbHoleName.Name = "txbHoleName";
            this.txbHoleName.Size = new System.Drawing.Size(100, 23);
            this.txbHoleName.TabIndex = 99;
            this.txbHoleName.Text = "Defect";
            // 
            // btnBrightRegionTh
            // 
            this.btnBrightRegionTh.BackColor = System.Drawing.Color.LightGray;
            this.btnBrightRegionTh.Location = new System.Drawing.Point(180, 14);
            this.btnBrightRegionTh.Name = "btnBrightRegionTh";
            this.btnBrightRegionTh.Size = new System.Drawing.Size(133, 30);
            this.btnBrightRegionTh.TabIndex = 89;
            this.btnBrightRegionTh.Text = "缺角閥值設定";
            this.btnBrightRegionTh.UseVisualStyleBackColor = false;
            this.btnBrightRegionTh.Click += new System.EventHandler(this.btnBrightRegionTh_Click);
            // 
            // btnTopHatEdgeExpTest
            // 
            this.btnTopHatEdgeExpTest.BackColor = System.Drawing.Color.LightGray;
            this.btnTopHatEdgeExpTest.Location = new System.Drawing.Point(236, 106);
            this.btnTopHatEdgeExpTest.Name = "btnTopHatEdgeExpTest";
            this.btnTopHatEdgeExpTest.Size = new System.Drawing.Size(77, 26);
            this.btnTopHatEdgeExpTest.TabIndex = 89;
            this.btnTopHatEdgeExpTest.Text = "範圍測試";
            this.btnTopHatEdgeExpTest.UseVisualStyleBackColor = false;
            this.btnTopHatEdgeExpTest.Click += new System.EventHandler(this.btnTopHatEdgeExpTest_Click);
            // 
            // txbTophatW
            // 
            this.txbTophatW.Location = new System.Drawing.Point(86, 191);
            this.txbTophatW.Name = "txbTophatW";
            this.txbTophatW.Size = new System.Drawing.Size(53, 23);
            this.txbTophatW.TabIndex = 98;
            this.txbTophatW.Text = "20";
            // 
            // cbxTophatWEnabled
            // 
            this.cbxTophatWEnabled.AutoSize = true;
            this.cbxTophatWEnabled.Location = new System.Drawing.Point(209, 195);
            this.cbxTophatWEnabled.Name = "cbxTophatWEnabled";
            this.cbxTophatWEnabled.Size = new System.Drawing.Size(51, 20);
            this.cbxTophatWEnabled.TabIndex = 99;
            this.cbxTophatWEnabled.Text = "啟用";
            this.cbxTophatWEnabled.UseVisualStyleBackColor = true;
            // 
            // labThinTopHatum2
            // 
            this.labThinTopHatum2.AutoSize = true;
            this.labThinTopHatum2.Location = new System.Drawing.Point(145, 224);
            this.labThinTopHatum2.Name = "labThinTopHatum2";
            this.labThinTopHatum2.Size = new System.Drawing.Size(35, 16);
            this.labThinTopHatum2.TabIndex = 97;
            this.labThinTopHatum2.Text = "(um)";
            // 
            // btnHoleInsp
            // 
            this.btnHoleInsp.BackColor = System.Drawing.Color.LightGray;
            this.btnHoleInsp.Location = new System.Drawing.Point(268, 209);
            this.btnHoleInsp.Name = "btnHoleInsp";
            this.btnHoleInsp.Size = new System.Drawing.Size(61, 32);
            this.btnHoleInsp.TabIndex = 89;
            this.btnHoleInsp.Text = "檢測";
            this.btnHoleInsp.UseVisualStyleBackColor = false;
            this.btnHoleInsp.Click += new System.EventHandler(this.btnHoleInsp_Click);
            // 
            // txbTophatMinArea
            // 
            this.txbTophatMinArea.Location = new System.Drawing.Point(86, 163);
            this.txbTophatMinArea.Name = "txbTophatMinArea";
            this.txbTophatMinArea.Size = new System.Drawing.Size(53, 23);
            this.txbTophatMinArea.TabIndex = 98;
            this.txbTophatMinArea.Text = "20";
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(158, 81);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(83, 16);
            this.label36.TabIndex = 96;
            this.label36.Text = "擴張區域閥值:";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(158, 51);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(62, 16);
            this.label35.TabIndex = 96;
            this.label35.Text = "上下擴張 :";
            // 
            // labTophatDarkTh
            // 
            this.labTophatDarkTh.AutoSize = true;
            this.labTophatDarkTh.Location = new System.Drawing.Point(16, 53);
            this.labTophatDarkTh.Name = "labTophatDarkTh";
            this.labTophatDarkTh.Size = new System.Drawing.Size(59, 16);
            this.labTophatDarkTh.TabIndex = 96;
            this.labTophatDarkTh.Text = "暗區閥值:";
            // 
            // cbxTophatAreaEnabled
            // 
            this.cbxTophatAreaEnabled.AutoSize = true;
            this.cbxTophatAreaEnabled.Location = new System.Drawing.Point(209, 164);
            this.cbxTophatAreaEnabled.Name = "cbxTophatAreaEnabled";
            this.cbxTophatAreaEnabled.Size = new System.Drawing.Size(51, 20);
            this.cbxTophatAreaEnabled.TabIndex = 99;
            this.cbxTophatAreaEnabled.Text = "啟用";
            this.cbxTophatAreaEnabled.UseVisualStyleBackColor = true;
            // 
            // labThinTopHatum1
            // 
            this.labThinTopHatum1.AutoSize = true;
            this.labThinTopHatum1.Location = new System.Drawing.Point(145, 194);
            this.labThinTopHatum1.Name = "labThinTopHatum1";
            this.labThinTopHatum1.Size = new System.Drawing.Size(35, 16);
            this.labThinTopHatum1.TabIndex = 97;
            this.labThinTopHatum1.Text = "(um)";
            // 
            // nudTophatBrightTh
            // 
            this.nudTophatBrightTh.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudTophatBrightTh.Location = new System.Drawing.Point(88, 79);
            this.nudTophatBrightTh.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudTophatBrightTh.Name = "nudTophatBrightTh";
            this.nudTophatBrightTh.Size = new System.Drawing.Size(53, 25);
            this.nudTophatBrightTh.TabIndex = 93;
            // 
            // labTophatum2
            // 
            this.labTophatum2.AutoSize = true;
            this.labTophatum2.Location = new System.Drawing.Point(145, 166);
            this.labTophatum2.Name = "labTophatum2";
            this.labTophatum2.Size = new System.Drawing.Size(51, 16);
            this.labTophatum2.TabIndex = 97;
            this.labTophatum2.Text = "(um^2)";
            // 
            // labTopHatH
            // 
            this.labTopHatH.AutoSize = true;
            this.labTopHatH.Location = new System.Drawing.Point(16, 224);
            this.labTopHatH.Name = "labTopHatH";
            this.labTopHatH.Size = new System.Drawing.Size(65, 16);
            this.labTopHatH.TabIndex = 97;
            this.labTopHatH.Text = "檢測高度 : ";
            // 
            // labTophatBrightTh
            // 
            this.labTophatBrightTh.AutoSize = true;
            this.labTophatBrightTh.Location = new System.Drawing.Point(16, 81);
            this.labTophatBrightTh.Name = "labTophatBrightTh";
            this.labTophatBrightTh.Size = new System.Drawing.Size(59, 16);
            this.labTophatBrightTh.TabIndex = 95;
            this.labTophatBrightTh.Text = "亮區閥值:";
            // 
            // labTophatMinArea
            // 
            this.labTophatMinArea.AutoSize = true;
            this.labTophatMinArea.Location = new System.Drawing.Point(16, 166);
            this.labTophatMinArea.Name = "labTophatMinArea";
            this.labTophatMinArea.Size = new System.Drawing.Size(65, 16);
            this.labTophatMinArea.TabIndex = 97;
            this.labTophatMinArea.Text = "檢測面積 : ";
            // 
            // nudSESizeHeight
            // 
            this.nudSESizeHeight.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudSESizeHeight.Location = new System.Drawing.Point(177, 136);
            this.nudSESizeHeight.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudSESizeHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSESizeHeight.Name = "nudSESizeHeight";
            this.nudSESizeHeight.Size = new System.Drawing.Size(53, 25);
            this.nudSESizeHeight.TabIndex = 93;
            this.nudSESizeHeight.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudTopHatEdgeExpHeight
            // 
            this.nudTopHatEdgeExpHeight.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudTopHatEdgeExpHeight.Location = new System.Drawing.Point(177, 106);
            this.nudTopHatEdgeExpHeight.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudTopHatEdgeExpHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudTopHatEdgeExpHeight.Name = "nudTopHatEdgeExpHeight";
            this.nudTopHatEdgeExpHeight.Size = new System.Drawing.Size(53, 25);
            this.nudTopHatEdgeExpHeight.TabIndex = 93;
            this.nudTopHatEdgeExpHeight.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // labTopHatW
            // 
            this.labTopHatW.AutoSize = true;
            this.labTopHatW.Location = new System.Drawing.Point(16, 194);
            this.labTopHatW.Name = "labTopHatW";
            this.labTopHatW.Size = new System.Drawing.Size(65, 16);
            this.labTopHatW.TabIndex = 97;
            this.labTopHatW.Text = "檢測寬度 : ";
            // 
            // nudExtHTh
            // 
            this.nudExtHTh.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudExtHTh.Location = new System.Drawing.Point(260, 76);
            this.nudExtHTh.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudExtHTh.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudExtHTh.Name = "nudExtHTh";
            this.nudExtHTh.Size = new System.Drawing.Size(53, 25);
            this.nudExtHTh.TabIndex = 93;
            this.nudExtHTh.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudSESizeWidth
            // 
            this.nudSESizeWidth.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudSESizeWidth.Location = new System.Drawing.Point(118, 136);
            this.nudSESizeWidth.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudSESizeWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSESizeWidth.Name = "nudSESizeWidth";
            this.nudSESizeWidth.Size = new System.Drawing.Size(53, 25);
            this.nudSESizeWidth.TabIndex = 93;
            this.nudSESizeWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudTopHatEdgeExpWidth
            // 
            this.nudTopHatEdgeExpWidth.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudTopHatEdgeExpWidth.Location = new System.Drawing.Point(118, 106);
            this.nudTopHatEdgeExpWidth.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudTopHatEdgeExpWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudTopHatEdgeExpWidth.Name = "nudTopHatEdgeExpWidth";
            this.nudTopHatEdgeExpWidth.Size = new System.Drawing.Size(53, 25);
            this.nudTopHatEdgeExpWidth.TabIndex = 93;
            this.nudTopHatEdgeExpWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // labSESize
            // 
            this.labSESize.AutoSize = true;
            this.labSESize.Location = new System.Drawing.Point(16, 139);
            this.labSESize.Name = "labSESize";
            this.labSESize.Size = new System.Drawing.Size(83, 16);
            this.labSESize.TabIndex = 95;
            this.labSESize.Text = "檢測元素大小:";
            // 
            // nudExtH
            // 
            this.nudExtH.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudExtH.Location = new System.Drawing.Point(260, 47);
            this.nudExtH.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudExtH.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudExtH.Name = "nudExtH";
            this.nudExtH.Size = new System.Drawing.Size(53, 25);
            this.nudExtH.TabIndex = 93;
            this.nudExtH.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // labTopHatEdgeExp
            // 
            this.labTopHatEdgeExp.AutoSize = true;
            this.labTopHatEdgeExp.Location = new System.Drawing.Point(16, 109);
            this.labTopHatEdgeExp.Name = "labTopHatEdgeExp";
            this.labTopHatEdgeExp.Size = new System.Drawing.Size(83, 16);
            this.labTopHatEdgeExp.TabIndex = 95;
            this.labTopHatEdgeExp.Text = "邊緣擴張大小:";
            // 
            // btnThinInsp
            // 
            this.btnThinInsp.BackColor = System.Drawing.Color.LightGray;
            this.btnThinInsp.Location = new System.Drawing.Point(775, 3);
            this.btnThinInsp.Name = "btnThinInsp";
            this.btnThinInsp.Size = new System.Drawing.Size(196, 32);
            this.btnThinInsp.TabIndex = 89;
            this.btnThinInsp.Text = "檢測";
            this.btnThinInsp.UseVisualStyleBackColor = false;
            this.btnThinInsp.Click += new System.EventHandler(this.btnThinInsp_Click);
            // 
            // gbxLargeArea
            // 
            this.gbxLargeArea.Controls.Add(this.cbxMeanEnabled);
            this.gbxLargeArea.Controls.Add(this.cbxThinHEnabled);
            this.gbxLargeArea.Controls.Add(this.txbThinInspH);
            this.gbxLargeArea.Controls.Add(this.txbThinInspW);
            this.gbxLargeArea.Controls.Add(this.btnThinRegionTest);
            this.gbxLargeArea.Controls.Add(this.cbxThinWEnabled);
            this.gbxLargeArea.Controls.Add(this.labThinum2);
            this.gbxLargeArea.Controls.Add(this.txbThinMinArea);
            this.gbxLargeArea.Controls.Add(this.cbxThinAreaEnabled);
            this.gbxLargeArea.Controls.Add(this.labThinum1);
            this.gbxLargeArea.Controls.Add(this.labDarkTH);
            this.gbxLargeArea.Controls.Add(this.labThinInspH);
            this.gbxLargeArea.Controls.Add(this.labThinum);
            this.gbxLargeArea.Controls.Add(this.labThinInspW);
            this.gbxLargeArea.Controls.Add(this.label33);
            this.gbxLargeArea.Controls.Add(this.label32);
            this.gbxLargeArea.Controls.Add(this.nudThinDarkTh);
            this.gbxLargeArea.Controls.Add(this.nudMeanOpenRad);
            this.gbxLargeArea.Controls.Add(this.labMinArea);
            this.gbxLargeArea.Controls.Add(this.nudThinEdgeSkipH);
            this.gbxLargeArea.Controls.Add(this.nudMeanCloseRad);
            this.gbxLargeArea.Controls.Add(this.nudThinEdgeSkipW);
            this.gbxLargeArea.Controls.Add(this.nudThinBrightTh);
            this.gbxLargeArea.Controls.Add(this.labThinByPass);
            this.gbxLargeArea.Controls.Add(this.labBrightTH);
            this.gbxLargeArea.Location = new System.Drawing.Point(614, 40);
            this.gbxLargeArea.Name = "gbxLargeArea";
            this.gbxLargeArea.Size = new System.Drawing.Size(295, 247);
            this.gbxLargeArea.TabIndex = 101;
            this.gbxLargeArea.TabStop = false;
            this.gbxLargeArea.Text = "平均值";
            // 
            // cbxMeanEnabled
            // 
            this.cbxMeanEnabled.AutoSize = true;
            this.cbxMeanEnabled.Location = new System.Drawing.Point(9, 27);
            this.cbxMeanEnabled.Name = "cbxMeanEnabled";
            this.cbxMeanEnabled.Size = new System.Drawing.Size(51, 20);
            this.cbxMeanEnabled.TabIndex = 100;
            this.cbxMeanEnabled.Text = "啟用";
            this.cbxMeanEnabled.UseVisualStyleBackColor = true;
            // 
            // cbxThinHEnabled
            // 
            this.cbxThinHEnabled.AutoSize = true;
            this.cbxThinHEnabled.Location = new System.Drawing.Point(199, 219);
            this.cbxThinHEnabled.Name = "cbxThinHEnabled";
            this.cbxThinHEnabled.Size = new System.Drawing.Size(51, 20);
            this.cbxThinHEnabled.TabIndex = 99;
            this.cbxThinHEnabled.Text = "啟用";
            this.cbxThinHEnabled.UseVisualStyleBackColor = true;
            // 
            // txbThinInspH
            // 
            this.txbThinInspH.Location = new System.Drawing.Point(76, 216);
            this.txbThinInspH.Name = "txbThinInspH";
            this.txbThinInspH.Size = new System.Drawing.Size(53, 23);
            this.txbThinInspH.TabIndex = 98;
            this.txbThinInspH.Text = "20";
            // 
            // txbThinInspW
            // 
            this.txbThinInspW.Location = new System.Drawing.Point(76, 186);
            this.txbThinInspW.Name = "txbThinInspW";
            this.txbThinInspW.Size = new System.Drawing.Size(53, 23);
            this.txbThinInspW.TabIndex = 98;
            this.txbThinInspW.Text = "20";
            // 
            // btnThinRegionTest
            // 
            this.btnThinRegionTest.BackColor = System.Drawing.Color.LightGray;
            this.btnThinRegionTest.Location = new System.Drawing.Point(226, 122);
            this.btnThinRegionTest.Name = "btnThinRegionTest";
            this.btnThinRegionTest.Size = new System.Drawing.Size(64, 26);
            this.btnThinRegionTest.TabIndex = 89;
            this.btnThinRegionTest.Text = "範圍測試";
            this.btnThinRegionTest.UseVisualStyleBackColor = false;
            this.btnThinRegionTest.Click += new System.EventHandler(this.btnThinRegionTest_Click);
            // 
            // cbxThinWEnabled
            // 
            this.cbxThinWEnabled.AutoSize = true;
            this.cbxThinWEnabled.Location = new System.Drawing.Point(199, 189);
            this.cbxThinWEnabled.Name = "cbxThinWEnabled";
            this.cbxThinWEnabled.Size = new System.Drawing.Size(51, 20);
            this.cbxThinWEnabled.TabIndex = 99;
            this.cbxThinWEnabled.Text = "啟用";
            this.cbxThinWEnabled.UseVisualStyleBackColor = true;
            // 
            // labThinum2
            // 
            this.labThinum2.AutoSize = true;
            this.labThinum2.Location = new System.Drawing.Point(135, 219);
            this.labThinum2.Name = "labThinum2";
            this.labThinum2.Size = new System.Drawing.Size(35, 16);
            this.labThinum2.TabIndex = 97;
            this.labThinum2.Text = "(um)";
            // 
            // txbThinMinArea
            // 
            this.txbThinMinArea.Location = new System.Drawing.Point(76, 155);
            this.txbThinMinArea.Name = "txbThinMinArea";
            this.txbThinMinArea.Size = new System.Drawing.Size(53, 23);
            this.txbThinMinArea.TabIndex = 98;
            this.txbThinMinArea.Text = "20";
            // 
            // cbxThinAreaEnabled
            // 
            this.cbxThinAreaEnabled.AutoSize = true;
            this.cbxThinAreaEnabled.Location = new System.Drawing.Point(199, 158);
            this.cbxThinAreaEnabled.Name = "cbxThinAreaEnabled";
            this.cbxThinAreaEnabled.Size = new System.Drawing.Size(51, 20);
            this.cbxThinAreaEnabled.TabIndex = 99;
            this.cbxThinAreaEnabled.Text = "啟用";
            this.cbxThinAreaEnabled.UseVisualStyleBackColor = true;
            // 
            // labThinum1
            // 
            this.labThinum1.AutoSize = true;
            this.labThinum1.Location = new System.Drawing.Point(135, 189);
            this.labThinum1.Name = "labThinum1";
            this.labThinum1.Size = new System.Drawing.Size(35, 16);
            this.labThinum1.TabIndex = 97;
            this.labThinum1.Text = "(um)";
            // 
            // labDarkTH
            // 
            this.labDarkTH.AutoSize = true;
            this.labDarkTH.Location = new System.Drawing.Point(6, 60);
            this.labDarkTH.Name = "labDarkTH";
            this.labDarkTH.Size = new System.Drawing.Size(59, 16);
            this.labDarkTH.TabIndex = 96;
            this.labDarkTH.Text = "暗區閥值:";
            // 
            // labThinInspH
            // 
            this.labThinInspH.AutoSize = true;
            this.labThinInspH.Location = new System.Drawing.Point(6, 219);
            this.labThinInspH.Name = "labThinInspH";
            this.labThinInspH.Size = new System.Drawing.Size(65, 16);
            this.labThinInspH.TabIndex = 97;
            this.labThinInspH.Text = "檢測高度 : ";
            // 
            // labThinum
            // 
            this.labThinum.AutoSize = true;
            this.labThinum.Location = new System.Drawing.Point(135, 158);
            this.labThinum.Name = "labThinum";
            this.labThinum.Size = new System.Drawing.Size(51, 16);
            this.labThinum.TabIndex = 97;
            this.labThinum.Text = "(um^2)";
            // 
            // labThinInspW
            // 
            this.labThinInspW.AutoSize = true;
            this.labThinInspW.Location = new System.Drawing.Point(6, 189);
            this.labThinInspW.Name = "labThinInspW";
            this.labThinInspW.Size = new System.Drawing.Size(65, 16);
            this.labThinInspW.TabIndex = 97;
            this.labThinInspW.Text = "檢測寬度 : ";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(164, 57);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(38, 16);
            this.label33.TabIndex = 95;
            this.label33.Text = "斷開 :";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(164, 87);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(38, 16);
            this.label32.TabIndex = 95;
            this.label32.Text = "閉合 :";
            // 
            // nudThinDarkTh
            // 
            this.nudThinDarkTh.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudThinDarkTh.Location = new System.Drawing.Point(77, 55);
            this.nudThinDarkTh.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudThinDarkTh.Name = "nudThinDarkTh";
            this.nudThinDarkTh.Size = new System.Drawing.Size(54, 25);
            this.nudThinDarkTh.TabIndex = 94;
            // 
            // nudMeanOpenRad
            // 
            this.nudMeanOpenRad.DecimalPlaces = 1;
            this.nudMeanOpenRad.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudMeanOpenRad.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.nudMeanOpenRad.Location = new System.Drawing.Point(222, 55);
            this.nudMeanOpenRad.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudMeanOpenRad.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMeanOpenRad.Name = "nudMeanOpenRad";
            this.nudMeanOpenRad.Size = new System.Drawing.Size(53, 25);
            this.nudMeanOpenRad.TabIndex = 93;
            this.nudMeanOpenRad.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // labMinArea
            // 
            this.labMinArea.AutoSize = true;
            this.labMinArea.Location = new System.Drawing.Point(6, 158);
            this.labMinArea.Name = "labMinArea";
            this.labMinArea.Size = new System.Drawing.Size(65, 16);
            this.labMinArea.TabIndex = 97;
            this.labMinArea.Text = "檢測面積 : ";
            // 
            // nudThinEdgeSkipH
            // 
            this.nudThinEdgeSkipH.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudThinEdgeSkipH.Location = new System.Drawing.Point(167, 122);
            this.nudThinEdgeSkipH.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudThinEdgeSkipH.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudThinEdgeSkipH.Name = "nudThinEdgeSkipH";
            this.nudThinEdgeSkipH.Size = new System.Drawing.Size(53, 25);
            this.nudThinEdgeSkipH.TabIndex = 93;
            this.nudThinEdgeSkipH.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudMeanCloseRad
            // 
            this.nudMeanCloseRad.DecimalPlaces = 1;
            this.nudMeanCloseRad.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudMeanCloseRad.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.nudMeanCloseRad.Location = new System.Drawing.Point(222, 85);
            this.nudMeanCloseRad.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudMeanCloseRad.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMeanCloseRad.Name = "nudMeanCloseRad";
            this.nudMeanCloseRad.Size = new System.Drawing.Size(53, 25);
            this.nudMeanCloseRad.TabIndex = 93;
            this.nudMeanCloseRad.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudThinEdgeSkipW
            // 
            this.nudThinEdgeSkipW.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudThinEdgeSkipW.Location = new System.Drawing.Point(108, 122);
            this.nudThinEdgeSkipW.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudThinEdgeSkipW.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudThinEdgeSkipW.Name = "nudThinEdgeSkipW";
            this.nudThinEdgeSkipW.Size = new System.Drawing.Size(53, 25);
            this.nudThinEdgeSkipW.TabIndex = 93;
            this.nudThinEdgeSkipW.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudThinBrightTh
            // 
            this.nudThinBrightTh.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudThinBrightTh.Location = new System.Drawing.Point(78, 91);
            this.nudThinBrightTh.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudThinBrightTh.Name = "nudThinBrightTh";
            this.nudThinBrightTh.Size = new System.Drawing.Size(53, 25);
            this.nudThinBrightTh.TabIndex = 93;
            // 
            // labThinByPass
            // 
            this.labThinByPass.AutoSize = true;
            this.labThinByPass.Location = new System.Drawing.Point(6, 125);
            this.labThinByPass.Name = "labThinByPass";
            this.labThinByPass.Size = new System.Drawing.Size(83, 16);
            this.labThinByPass.TabIndex = 95;
            this.labThinByPass.Text = "邊緣忽略大小:";
            // 
            // labBrightTH
            // 
            this.labBrightTH.AutoSize = true;
            this.labBrightTH.Location = new System.Drawing.Point(6, 93);
            this.labBrightTH.Name = "labBrightTH";
            this.labBrightTH.Size = new System.Drawing.Size(59, 16);
            this.labBrightTH.TabIndex = 95;
            this.labBrightTH.Text = "亮區閥值:";
            // 
            // Scratch
            // 
            this.Scratch.BackColor = System.Drawing.SystemColors.Control;
            this.Scratch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Scratch.Controls.Add(this.btnScratchRegion);
            this.Scratch.Controls.Add(this.gbxScratchOuter);
            this.Scratch.Controls.Add(this.txbScratchName);
            this.Scratch.Controls.Add(this.cbxScratchEnabled);
            this.Scratch.Controls.Add(this.gbxScratch);
            this.Scratch.Controls.Add(this.btnScratchInsp);
            this.Scratch.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Scratch.Location = new System.Drawing.Point(4, 25);
            this.Scratch.Name = "Scratch";
            this.Scratch.Size = new System.Drawing.Size(382, 817);
            this.Scratch.TabIndex = 4;
            this.Scratch.Text = "刮傷檢測";
            // 
            // btnScratchRegion
            // 
            this.btnScratchRegion.BackColor = System.Drawing.Color.LightGray;
            this.btnScratchRegion.Location = new System.Drawing.Point(556, 175);
            this.btnScratchRegion.Name = "btnScratchRegion";
            this.btnScratchRegion.Size = new System.Drawing.Size(124, 32);
            this.btnScratchRegion.TabIndex = 110;
            this.btnScratchRegion.Text = "範圍測試";
            this.btnScratchRegion.UseVisualStyleBackColor = false;
            this.btnScratchRegion.Click += new System.EventHandler(this.btnScratchRegion_Click);
            // 
            // gbxScratchOuter
            // 
            this.gbxScratchOuter.Controls.Add(this.labScratchOutTh);
            this.gbxScratchOuter.Controls.Add(this.cbxScratchOuterHeightEnabled);
            this.gbxScratchOuter.Controls.Add(this.nudScratchOutTH);
            this.gbxScratchOuter.Controls.Add(this.btnScratchOutTh);
            this.gbxScratchOuter.Controls.Add(this.txbScratchOuterHeightMin);
            this.gbxScratchOuter.Controls.Add(this.label50);
            this.gbxScratchOuter.Controls.Add(this.label51);
            this.gbxScratchOuter.Controls.Add(this.txbScratchOuterWidthMin);
            this.gbxScratchOuter.Controls.Add(this.label49);
            this.gbxScratchOuter.Controls.Add(this.label52);
            this.gbxScratchOuter.Controls.Add(this.cbxScratchOuterWidthEnabled);
            this.gbxScratchOuter.Controls.Add(this.label47);
            this.gbxScratchOuter.Controls.Add(this.cbxScratchOuterAreaEnabled);
            this.gbxScratchOuter.Controls.Add(this.label53);
            this.gbxScratchOuter.Controls.Add(this.txbScratchOuterAreaMin);
            this.gbxScratchOuter.Location = new System.Drawing.Point(284, 40);
            this.gbxScratchOuter.Name = "gbxScratchOuter";
            this.gbxScratchOuter.Size = new System.Drawing.Size(266, 205);
            this.gbxScratchOuter.TabIndex = 109;
            this.gbxScratchOuter.TabStop = false;
            this.gbxScratchOuter.Text = "電極刮傷參數設定";
            // 
            // labScratchOutTh
            // 
            this.labScratchOutTh.AutoSize = true;
            this.labScratchOutTh.Location = new System.Drawing.Point(6, 72);
            this.labScratchOutTh.Name = "labScratchOutTh";
            this.labScratchOutTh.Size = new System.Drawing.Size(59, 16);
            this.labScratchOutTh.TabIndex = 95;
            this.labScratchOutTh.Text = "外部閥值:";
            // 
            // cbxScratchOuterHeightEnabled
            // 
            this.cbxScratchOuterHeightEnabled.AutoSize = true;
            this.cbxScratchOuterHeightEnabled.Location = new System.Drawing.Point(201, 164);
            this.cbxScratchOuterHeightEnabled.Name = "cbxScratchOuterHeightEnabled";
            this.cbxScratchOuterHeightEnabled.Size = new System.Drawing.Size(51, 20);
            this.cbxScratchOuterHeightEnabled.TabIndex = 106;
            this.cbxScratchOuterHeightEnabled.Text = "啟用";
            this.cbxScratchOuterHeightEnabled.UseVisualStyleBackColor = true;
            // 
            // nudScratchOutTH
            // 
            this.nudScratchOutTH.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudScratchOutTH.Location = new System.Drawing.Point(82, 70);
            this.nudScratchOutTH.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudScratchOutTH.Name = "nudScratchOutTH";
            this.nudScratchOutTH.Size = new System.Drawing.Size(53, 25);
            this.nudScratchOutTH.TabIndex = 93;
            // 
            // btnScratchOutTh
            // 
            this.btnScratchOutTh.Location = new System.Drawing.Point(141, 64);
            this.btnScratchOutTh.Name = "btnScratchOutTh";
            this.btnScratchOutTh.Size = new System.Drawing.Size(54, 32);
            this.btnScratchOutTh.TabIndex = 92;
            this.btnScratchOutTh.Text = "設定";
            this.btnScratchOutTh.UseVisualStyleBackColor = true;
            this.btnScratchOutTh.Click += new System.EventHandler(this.btnScratchOutTh_Click);
            // 
            // txbScratchOuterHeightMin
            // 
            this.txbScratchOuterHeightMin.Location = new System.Drawing.Point(82, 161);
            this.txbScratchOuterHeightMin.Name = "txbScratchOuterHeightMin";
            this.txbScratchOuterHeightMin.Size = new System.Drawing.Size(53, 23);
            this.txbScratchOuterHeightMin.TabIndex = 104;
            this.txbScratchOuterHeightMin.Text = "20";
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.Location = new System.Drawing.Point(6, 133);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(65, 16);
            this.label50.TabIndex = 103;
            this.label50.Text = "檢測寬度 : ";
            // 
            // label51
            // 
            this.label51.AutoSize = true;
            this.label51.Location = new System.Drawing.Point(6, 165);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(65, 16);
            this.label51.TabIndex = 102;
            this.label51.Text = "檢測高度 : ";
            // 
            // txbScratchOuterWidthMin
            // 
            this.txbScratchOuterWidthMin.Location = new System.Drawing.Point(82, 130);
            this.txbScratchOuterWidthMin.Name = "txbScratchOuterWidthMin";
            this.txbScratchOuterWidthMin.Size = new System.Drawing.Size(53, 23);
            this.txbScratchOuterWidthMin.TabIndex = 105;
            this.txbScratchOuterWidthMin.Text = "20";
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Location = new System.Drawing.Point(141, 102);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(51, 16);
            this.label49.TabIndex = 97;
            this.label49.Text = "(um^2)";
            // 
            // label52
            // 
            this.label52.AutoSize = true;
            this.label52.Location = new System.Drawing.Point(141, 133);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(35, 16);
            this.label52.TabIndex = 101;
            this.label52.Text = "(um)";
            // 
            // cbxScratchOuterWidthEnabled
            // 
            this.cbxScratchOuterWidthEnabled.AutoSize = true;
            this.cbxScratchOuterWidthEnabled.Location = new System.Drawing.Point(201, 134);
            this.cbxScratchOuterWidthEnabled.Name = "cbxScratchOuterWidthEnabled";
            this.cbxScratchOuterWidthEnabled.Size = new System.Drawing.Size(51, 20);
            this.cbxScratchOuterWidthEnabled.TabIndex = 107;
            this.cbxScratchOuterWidthEnabled.Text = "啟用";
            this.cbxScratchOuterWidthEnabled.UseVisualStyleBackColor = true;
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Location = new System.Drawing.Point(6, 102);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(62, 16);
            this.label47.TabIndex = 97;
            this.label47.Text = "檢測面積 :";
            // 
            // cbxScratchOuterAreaEnabled
            // 
            this.cbxScratchOuterAreaEnabled.AutoSize = true;
            this.cbxScratchOuterAreaEnabled.Location = new System.Drawing.Point(201, 102);
            this.cbxScratchOuterAreaEnabled.Name = "cbxScratchOuterAreaEnabled";
            this.cbxScratchOuterAreaEnabled.Size = new System.Drawing.Size(51, 20);
            this.cbxScratchOuterAreaEnabled.TabIndex = 108;
            this.cbxScratchOuterAreaEnabled.Text = "啟用";
            this.cbxScratchOuterAreaEnabled.UseVisualStyleBackColor = true;
            // 
            // label53
            // 
            this.label53.AutoSize = true;
            this.label53.Location = new System.Drawing.Point(141, 163);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(35, 16);
            this.label53.TabIndex = 100;
            this.label53.Text = "(um)";
            // 
            // txbScratchOuterAreaMin
            // 
            this.txbScratchOuterAreaMin.Location = new System.Drawing.Point(82, 99);
            this.txbScratchOuterAreaMin.Name = "txbScratchOuterAreaMin";
            this.txbScratchOuterAreaMin.Size = new System.Drawing.Size(53, 23);
            this.txbScratchOuterAreaMin.TabIndex = 98;
            this.txbScratchOuterAreaMin.Text = "20";
            // 
            // txbScratchName
            // 
            this.txbScratchName.Location = new System.Drawing.Point(103, 11);
            this.txbScratchName.Name = "txbScratchName";
            this.txbScratchName.Size = new System.Drawing.Size(100, 23);
            this.txbScratchName.TabIndex = 102;
            this.txbScratchName.Text = "Defect";
            // 
            // cbxScratchEnabled
            // 
            this.cbxScratchEnabled.AutoSize = true;
            this.cbxScratchEnabled.Location = new System.Drawing.Point(12, 12);
            this.cbxScratchEnabled.Name = "cbxScratchEnabled";
            this.cbxScratchEnabled.Size = new System.Drawing.Size(75, 20);
            this.cbxScratchEnabled.TabIndex = 101;
            this.cbxScratchEnabled.Text = "檢測啟用";
            this.cbxScratchEnabled.UseVisualStyleBackColor = true;
            // 
            // gbxScratch
            // 
            this.gbxScratch.Controls.Add(this.cbxScratchHeightEnabled);
            this.gbxScratch.Controls.Add(this.txbScratchHeightMin);
            this.gbxScratch.Controls.Add(this.txbScratchWidthMin);
            this.gbxScratch.Controls.Add(this.cbxScratchWidthEnabled);
            this.gbxScratch.Controls.Add(this.label7);
            this.gbxScratch.Controls.Add(this.cbxScratchAreaEnabled);
            this.gbxScratch.Controls.Add(this.label8);
            this.gbxScratch.Controls.Add(this.label9);
            this.gbxScratch.Controls.Add(this.label10);
            this.gbxScratch.Controls.Add(this.txbScratchMinArea);
            this.gbxScratch.Controls.Add(this.labScratchum2);
            this.gbxScratch.Controls.Add(this.labScratchMinArea);
            this.gbxScratch.Controls.Add(this.labScratchID);
            this.gbxScratch.Controls.Add(this.txbScratchID);
            this.gbxScratch.Controls.Add(this.labScratchInTH);
            this.gbxScratch.Controls.Add(this.btnScratchInTH);
            this.gbxScratch.Controls.Add(this.nudScratchInTH);
            this.gbxScratch.Location = new System.Drawing.Point(12, 40);
            this.gbxScratch.Name = "gbxScratch";
            this.gbxScratch.Size = new System.Drawing.Size(266, 205);
            this.gbxScratch.TabIndex = 100;
            this.gbxScratch.TabStop = false;
            this.gbxScratch.Text = "刮傷參數設定";
            // 
            // cbxScratchHeightEnabled
            // 
            this.cbxScratchHeightEnabled.AutoSize = true;
            this.cbxScratchHeightEnabled.Location = new System.Drawing.Point(199, 169);
            this.cbxScratchHeightEnabled.Name = "cbxScratchHeightEnabled";
            this.cbxScratchHeightEnabled.Size = new System.Drawing.Size(51, 20);
            this.cbxScratchHeightEnabled.TabIndex = 106;
            this.cbxScratchHeightEnabled.Text = "啟用";
            this.cbxScratchHeightEnabled.UseVisualStyleBackColor = true;
            // 
            // txbScratchHeightMin
            // 
            this.txbScratchHeightMin.Location = new System.Drawing.Point(76, 169);
            this.txbScratchHeightMin.Name = "txbScratchHeightMin";
            this.txbScratchHeightMin.Size = new System.Drawing.Size(53, 23);
            this.txbScratchHeightMin.TabIndex = 104;
            this.txbScratchHeightMin.Text = "20";
            // 
            // txbScratchWidthMin
            // 
            this.txbScratchWidthMin.Location = new System.Drawing.Point(76, 138);
            this.txbScratchWidthMin.Name = "txbScratchWidthMin";
            this.txbScratchWidthMin.Size = new System.Drawing.Size(53, 23);
            this.txbScratchWidthMin.TabIndex = 105;
            this.txbScratchWidthMin.Text = "20";
            // 
            // cbxScratchWidthEnabled
            // 
            this.cbxScratchWidthEnabled.AutoSize = true;
            this.cbxScratchWidthEnabled.Location = new System.Drawing.Point(199, 139);
            this.cbxScratchWidthEnabled.Name = "cbxScratchWidthEnabled";
            this.cbxScratchWidthEnabled.Size = new System.Drawing.Size(51, 20);
            this.cbxScratchWidthEnabled.TabIndex = 107;
            this.cbxScratchWidthEnabled.Text = "啟用";
            this.cbxScratchWidthEnabled.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(135, 171);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 16);
            this.label7.TabIndex = 100;
            this.label7.Text = "(um)";
            // 
            // cbxScratchAreaEnabled
            // 
            this.cbxScratchAreaEnabled.AutoSize = true;
            this.cbxScratchAreaEnabled.Location = new System.Drawing.Point(199, 107);
            this.cbxScratchAreaEnabled.Name = "cbxScratchAreaEnabled";
            this.cbxScratchAreaEnabled.Size = new System.Drawing.Size(51, 20);
            this.cbxScratchAreaEnabled.TabIndex = 108;
            this.cbxScratchAreaEnabled.Text = "啟用";
            this.cbxScratchAreaEnabled.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(135, 141);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 16);
            this.label8.TabIndex = 101;
            this.label8.Text = "(um)";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(4, 173);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 16);
            this.label9.TabIndex = 102;
            this.label9.Text = "檢測高度 : ";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(4, 141);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 16);
            this.label10.TabIndex = 103;
            this.label10.Text = "檢測寬度 : ";
            // 
            // txbScratchMinArea
            // 
            this.txbScratchMinArea.Location = new System.Drawing.Point(76, 107);
            this.txbScratchMinArea.Name = "txbScratchMinArea";
            this.txbScratchMinArea.Size = new System.Drawing.Size(53, 23);
            this.txbScratchMinArea.TabIndex = 98;
            this.txbScratchMinArea.Text = "20";
            // 
            // labScratchum2
            // 
            this.labScratchum2.AutoSize = true;
            this.labScratchum2.Location = new System.Drawing.Point(135, 110);
            this.labScratchum2.Name = "labScratchum2";
            this.labScratchum2.Size = new System.Drawing.Size(51, 16);
            this.labScratchum2.TabIndex = 97;
            this.labScratchum2.Text = "(um^2)";
            // 
            // labScratchMinArea
            // 
            this.labScratchMinArea.AutoSize = true;
            this.labScratchMinArea.Location = new System.Drawing.Point(4, 110);
            this.labScratchMinArea.Name = "labScratchMinArea";
            this.labScratchMinArea.Size = new System.Drawing.Size(62, 16);
            this.labScratchMinArea.TabIndex = 97;
            this.labScratchMinArea.Text = "檢測面積 :";
            // 
            // labScratchID
            // 
            this.labScratchID.AutoSize = true;
            this.labScratchID.Location = new System.Drawing.Point(6, 32);
            this.labScratchID.Name = "labScratchID";
            this.labScratchID.Size = new System.Drawing.Size(54, 16);
            this.labScratchID.TabIndex = 91;
            this.labScratchID.Text = "影像ID : ";
            // 
            // txbScratchID
            // 
            this.txbScratchID.Location = new System.Drawing.Point(75, 29);
            this.txbScratchID.Name = "txbScratchID";
            this.txbScratchID.Size = new System.Drawing.Size(54, 23);
            this.txbScratchID.TabIndex = 90;
            this.txbScratchID.Text = "0";
            // 
            // labScratchInTH
            // 
            this.labScratchInTH.AutoSize = true;
            this.labScratchInTH.Location = new System.Drawing.Point(4, 75);
            this.labScratchInTH.Name = "labScratchInTH";
            this.labScratchInTH.Size = new System.Drawing.Size(59, 16);
            this.labScratchInTH.TabIndex = 96;
            this.labScratchInTH.Text = "內部閥值:";
            // 
            // btnScratchInTH
            // 
            this.btnScratchInTH.Location = new System.Drawing.Point(135, 67);
            this.btnScratchInTH.Name = "btnScratchInTH";
            this.btnScratchInTH.Size = new System.Drawing.Size(54, 32);
            this.btnScratchInTH.TabIndex = 92;
            this.btnScratchInTH.Text = "設定";
            this.btnScratchInTH.UseVisualStyleBackColor = true;
            this.btnScratchInTH.Click += new System.EventHandler(this.btnScratchInTH_Click);
            // 
            // nudScratchInTH
            // 
            this.nudScratchInTH.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudScratchInTH.Location = new System.Drawing.Point(75, 70);
            this.nudScratchInTH.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudScratchInTH.Name = "nudScratchInTH";
            this.nudScratchInTH.Size = new System.Drawing.Size(54, 25);
            this.nudScratchInTH.TabIndex = 94;
            // 
            // btnScratchInsp
            // 
            this.btnScratchInsp.BackColor = System.Drawing.Color.LightGray;
            this.btnScratchInsp.Location = new System.Drawing.Point(556, 213);
            this.btnScratchInsp.Name = "btnScratchInsp";
            this.btnScratchInsp.Size = new System.Drawing.Size(124, 32);
            this.btnScratchInsp.TabIndex = 90;
            this.btnScratchInsp.Text = "檢測";
            this.btnScratchInsp.UseVisualStyleBackColor = false;
            this.btnScratchInsp.Click += new System.EventHandler(this.btnScratchInsp_Click);
            // 
            // Stain
            // 
            this.Stain.BackColor = System.Drawing.SystemColors.Control;
            this.Stain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Stain.Controls.Add(this.txbStainName);
            this.Stain.Controls.Add(this.cbxStainEnabled);
            this.Stain.Controls.Add(this.gbxStain);
            this.Stain.Controls.Add(this.btnStainPartitionTest);
            this.Stain.Controls.Add(this.btnStainTest);
            this.Stain.Location = new System.Drawing.Point(4, 25);
            this.Stain.Name = "Stain";
            this.Stain.Size = new System.Drawing.Size(382, 817);
            this.Stain.TabIndex = 6;
            this.Stain.Text = "髒污檢測";
            // 
            // txbStainName
            // 
            this.txbStainName.Location = new System.Drawing.Point(103, 11);
            this.txbStainName.Name = "txbStainName";
            this.txbStainName.Size = new System.Drawing.Size(100, 23);
            this.txbStainName.TabIndex = 102;
            this.txbStainName.Text = "Defect";
            // 
            // cbxStainEnabled
            // 
            this.cbxStainEnabled.AutoSize = true;
            this.cbxStainEnabled.Location = new System.Drawing.Point(12, 12);
            this.cbxStainEnabled.Name = "cbxStainEnabled";
            this.cbxStainEnabled.Size = new System.Drawing.Size(75, 20);
            this.cbxStainEnabled.TabIndex = 101;
            this.cbxStainEnabled.Text = "檢測啟用";
            this.cbxStainEnabled.UseVisualStyleBackColor = true;
            // 
            // gbxStain
            // 
            this.gbxStain.Controls.Add(this.cbxStainHeightEnabled);
            this.gbxStain.Controls.Add(this.txbStainHeightMin);
            this.gbxStain.Controls.Add(this.txbStainWidthMin);
            this.gbxStain.Controls.Add(this.cbxStainWidthEnabled);
            this.gbxStain.Controls.Add(this.label11);
            this.gbxStain.Controls.Add(this.cbxStainAreaEnabled);
            this.gbxStain.Controls.Add(this.label12);
            this.gbxStain.Controls.Add(this.label13);
            this.gbxStain.Controls.Add(this.label14);
            this.gbxStain.Controls.Add(this.txbStainMinArea);
            this.gbxStain.Controls.Add(this.labStainum2);
            this.gbxStain.Controls.Add(this.labStainMinArea);
            this.gbxStain.Controls.Add(this.labStainID);
            this.gbxStain.Controls.Add(this.labStainBrightTh);
            this.gbxStain.Controls.Add(this.txbStainID);
            this.gbxStain.Controls.Add(this.labStainDarkTh);
            this.gbxStain.Controls.Add(this.btnStainBrightTh);
            this.gbxStain.Controls.Add(this.btnStainDarkTh);
            this.gbxStain.Controls.Add(this.nudStainBrightTh);
            this.gbxStain.Controls.Add(this.nudStainDarkTh);
            this.gbxStain.Location = new System.Drawing.Point(12, 40);
            this.gbxStain.Name = "gbxStain";
            this.gbxStain.Size = new System.Drawing.Size(266, 260);
            this.gbxStain.TabIndex = 100;
            this.gbxStain.TabStop = false;
            this.gbxStain.Text = "髒污參數設定";
            // 
            // cbxStainHeightEnabled
            // 
            this.cbxStainHeightEnabled.AutoSize = true;
            this.cbxStainHeightEnabled.Location = new System.Drawing.Point(199, 212);
            this.cbxStainHeightEnabled.Name = "cbxStainHeightEnabled";
            this.cbxStainHeightEnabled.Size = new System.Drawing.Size(51, 20);
            this.cbxStainHeightEnabled.TabIndex = 115;
            this.cbxStainHeightEnabled.Text = "啟用";
            this.cbxStainHeightEnabled.UseVisualStyleBackColor = true;
            // 
            // txbStainHeightMin
            // 
            this.txbStainHeightMin.Location = new System.Drawing.Point(76, 212);
            this.txbStainHeightMin.Name = "txbStainHeightMin";
            this.txbStainHeightMin.Size = new System.Drawing.Size(53, 23);
            this.txbStainHeightMin.TabIndex = 113;
            this.txbStainHeightMin.Text = "20";
            // 
            // txbStainWidthMin
            // 
            this.txbStainWidthMin.Location = new System.Drawing.Point(76, 181);
            this.txbStainWidthMin.Name = "txbStainWidthMin";
            this.txbStainWidthMin.Size = new System.Drawing.Size(53, 23);
            this.txbStainWidthMin.TabIndex = 114;
            this.txbStainWidthMin.Text = "20";
            // 
            // cbxStainWidthEnabled
            // 
            this.cbxStainWidthEnabled.AutoSize = true;
            this.cbxStainWidthEnabled.Location = new System.Drawing.Point(199, 182);
            this.cbxStainWidthEnabled.Name = "cbxStainWidthEnabled";
            this.cbxStainWidthEnabled.Size = new System.Drawing.Size(51, 20);
            this.cbxStainWidthEnabled.TabIndex = 116;
            this.cbxStainWidthEnabled.Text = "啟用";
            this.cbxStainWidthEnabled.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(135, 214);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(35, 16);
            this.label11.TabIndex = 109;
            this.label11.Text = "(um)";
            // 
            // cbxStainAreaEnabled
            // 
            this.cbxStainAreaEnabled.AutoSize = true;
            this.cbxStainAreaEnabled.Location = new System.Drawing.Point(199, 150);
            this.cbxStainAreaEnabled.Name = "cbxStainAreaEnabled";
            this.cbxStainAreaEnabled.Size = new System.Drawing.Size(51, 20);
            this.cbxStainAreaEnabled.TabIndex = 117;
            this.cbxStainAreaEnabled.Text = "啟用";
            this.cbxStainAreaEnabled.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(135, 184);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(35, 16);
            this.label12.TabIndex = 110;
            this.label12.Text = "(um)";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(4, 216);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(65, 16);
            this.label13.TabIndex = 111;
            this.label13.Text = "檢測高度 : ";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(4, 184);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(65, 16);
            this.label14.TabIndex = 112;
            this.label14.Text = "檢測寬度 : ";
            // 
            // txbStainMinArea
            // 
            this.txbStainMinArea.Location = new System.Drawing.Point(76, 150);
            this.txbStainMinArea.Name = "txbStainMinArea";
            this.txbStainMinArea.Size = new System.Drawing.Size(53, 23);
            this.txbStainMinArea.TabIndex = 98;
            this.txbStainMinArea.Text = "20";
            // 
            // labStainum2
            // 
            this.labStainum2.AutoSize = true;
            this.labStainum2.Location = new System.Drawing.Point(135, 153);
            this.labStainum2.Name = "labStainum2";
            this.labStainum2.Size = new System.Drawing.Size(51, 16);
            this.labStainum2.TabIndex = 97;
            this.labStainum2.Text = "(um^2)";
            // 
            // labStainMinArea
            // 
            this.labStainMinArea.AutoSize = true;
            this.labStainMinArea.Location = new System.Drawing.Point(4, 153);
            this.labStainMinArea.Name = "labStainMinArea";
            this.labStainMinArea.Size = new System.Drawing.Size(65, 16);
            this.labStainMinArea.TabIndex = 97;
            this.labStainMinArea.Text = "檢測面積 : ";
            // 
            // labStainID
            // 
            this.labStainID.AutoSize = true;
            this.labStainID.Location = new System.Drawing.Point(6, 32);
            this.labStainID.Name = "labStainID";
            this.labStainID.Size = new System.Drawing.Size(54, 16);
            this.labStainID.TabIndex = 91;
            this.labStainID.Text = "影像ID : ";
            // 
            // labStainBrightTh
            // 
            this.labStainBrightTh.AutoSize = true;
            this.labStainBrightTh.Location = new System.Drawing.Point(4, 108);
            this.labStainBrightTh.Name = "labStainBrightTh";
            this.labStainBrightTh.Size = new System.Drawing.Size(59, 16);
            this.labStainBrightTh.TabIndex = 95;
            this.labStainBrightTh.Text = "亮區閥值:";
            // 
            // txbStainID
            // 
            this.txbStainID.Location = new System.Drawing.Point(75, 29);
            this.txbStainID.Name = "txbStainID";
            this.txbStainID.Size = new System.Drawing.Size(54, 23);
            this.txbStainID.TabIndex = 90;
            this.txbStainID.Text = "0";
            // 
            // labStainDarkTh
            // 
            this.labStainDarkTh.AutoSize = true;
            this.labStainDarkTh.Location = new System.Drawing.Point(4, 75);
            this.labStainDarkTh.Name = "labStainDarkTh";
            this.labStainDarkTh.Size = new System.Drawing.Size(59, 16);
            this.labStainDarkTh.TabIndex = 96;
            this.labStainDarkTh.Text = "暗區閥值:";
            // 
            // btnStainBrightTh
            // 
            this.btnStainBrightTh.Location = new System.Drawing.Point(135, 100);
            this.btnStainBrightTh.Name = "btnStainBrightTh";
            this.btnStainBrightTh.Size = new System.Drawing.Size(54, 32);
            this.btnStainBrightTh.TabIndex = 92;
            this.btnStainBrightTh.Text = "設定";
            this.btnStainBrightTh.UseVisualStyleBackColor = true;
            this.btnStainBrightTh.Click += new System.EventHandler(this.btnStainBrightTh_Click);
            // 
            // btnStainDarkTh
            // 
            this.btnStainDarkTh.Location = new System.Drawing.Point(135, 67);
            this.btnStainDarkTh.Name = "btnStainDarkTh";
            this.btnStainDarkTh.Size = new System.Drawing.Size(54, 32);
            this.btnStainDarkTh.TabIndex = 92;
            this.btnStainDarkTh.Text = "設定";
            this.btnStainDarkTh.UseVisualStyleBackColor = true;
            this.btnStainDarkTh.Click += new System.EventHandler(this.btnStainDarkTh_Click);
            // 
            // nudStainBrightTh
            // 
            this.nudStainBrightTh.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudStainBrightTh.Location = new System.Drawing.Point(76, 106);
            this.nudStainBrightTh.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudStainBrightTh.Name = "nudStainBrightTh";
            this.nudStainBrightTh.Size = new System.Drawing.Size(53, 25);
            this.nudStainBrightTh.TabIndex = 93;
            // 
            // nudStainDarkTh
            // 
            this.nudStainDarkTh.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudStainDarkTh.Location = new System.Drawing.Point(75, 70);
            this.nudStainDarkTh.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudStainDarkTh.Name = "nudStainDarkTh";
            this.nudStainDarkTh.Size = new System.Drawing.Size(54, 25);
            this.nudStainDarkTh.TabIndex = 94;
            // 
            // btnStainPartitionTest
            // 
            this.btnStainPartitionTest.BackColor = System.Drawing.Color.LightGray;
            this.btnStainPartitionTest.Location = new System.Drawing.Point(284, 230);
            this.btnStainPartitionTest.Name = "btnStainPartitionTest";
            this.btnStainPartitionTest.Size = new System.Drawing.Size(124, 32);
            this.btnStainPartitionTest.TabIndex = 94;
            this.btnStainPartitionTest.Text = "分區測試";
            this.btnStainPartitionTest.UseVisualStyleBackColor = false;
            this.btnStainPartitionTest.Click += new System.EventHandler(this.btnStainPartitionTest_Click);
            // 
            // btnStainTest
            // 
            this.btnStainTest.BackColor = System.Drawing.Color.LightGray;
            this.btnStainTest.Location = new System.Drawing.Point(284, 268);
            this.btnStainTest.Name = "btnStainTest";
            this.btnStainTest.Size = new System.Drawing.Size(124, 32);
            this.btnStainTest.TabIndex = 93;
            this.btnStainTest.Text = "檢測";
            this.btnStainTest.UseVisualStyleBackColor = false;
            this.btnStainTest.Click += new System.EventHandler(this.btnStainTest_Click);
            // 
            // RDefect
            // 
            this.RDefect.BackColor = System.Drawing.SystemColors.Control;
            this.RDefect.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RDefect.Controls.Add(this.txbRShiftName);
            this.RDefect.Controls.Add(this.txbRDefectName);
            this.RDefect.Controls.Add(this.cbxRShiftEnabled);
            this.RDefect.Controls.Add(this.cbxRDefectEnabled);
            this.RDefect.Controls.Add(this.gbxRShift);
            this.RDefect.Controls.Add(this.gbxRDefect);
            this.RDefect.Controls.Add(this.btnRDefectPartitionTest);
            this.RDefect.Controls.Add(this.btnRDefectTest);
            this.RDefect.Font = new System.Drawing.Font("微軟正黑體", 8F, System.Drawing.FontStyle.Bold);
            this.RDefect.Location = new System.Drawing.Point(4, 25);
            this.RDefect.Name = "RDefect";
            this.RDefect.Size = new System.Drawing.Size(382, 817);
            this.RDefect.TabIndex = 5;
            this.RDefect.Text = "R層異常";
            // 
            // txbRShiftName
            // 
            this.txbRShiftName.Location = new System.Drawing.Point(589, 11);
            this.txbRShiftName.Name = "txbRShiftName";
            this.txbRShiftName.Size = new System.Drawing.Size(100, 22);
            this.txbRShiftName.TabIndex = 105;
            this.txbRShiftName.Text = "Defect";
            // 
            // txbRDefectName
            // 
            this.txbRDefectName.Location = new System.Drawing.Point(103, 11);
            this.txbRDefectName.Name = "txbRDefectName";
            this.txbRDefectName.Size = new System.Drawing.Size(100, 22);
            this.txbRDefectName.TabIndex = 105;
            this.txbRDefectName.Text = "Defect";
            // 
            // cbxRShiftEnabled
            // 
            this.cbxRShiftEnabled.AutoSize = true;
            this.cbxRShiftEnabled.Location = new System.Drawing.Point(500, 12);
            this.cbxRShiftEnabled.Name = "cbxRShiftEnabled";
            this.cbxRShiftEnabled.Size = new System.Drawing.Size(70, 19);
            this.cbxRShiftEnabled.TabIndex = 104;
            this.cbxRShiftEnabled.Text = "檢測啟用";
            this.cbxRShiftEnabled.UseVisualStyleBackColor = true;
            // 
            // cbxRDefectEnabled
            // 
            this.cbxRDefectEnabled.AutoSize = true;
            this.cbxRDefectEnabled.Location = new System.Drawing.Point(12, 12);
            this.cbxRDefectEnabled.Name = "cbxRDefectEnabled";
            this.cbxRDefectEnabled.Size = new System.Drawing.Size(70, 19);
            this.cbxRDefectEnabled.TabIndex = 104;
            this.cbxRDefectEnabled.Text = "檢測啟用";
            this.cbxRDefectEnabled.UseVisualStyleBackColor = true;
            // 
            // gbxRShift
            // 
            this.gbxRShift.Controls.Add(this.nudRShiftClosingSize);
            this.gbxRShift.Controls.Add(this.btnTargetRTest);
            this.gbxRShift.Controls.Add(this.btnAutoThTest);
            this.gbxRShift.Controls.Add(this.btnTargetETest);
            this.gbxRShift.Controls.Add(this.nudTargetRWidth);
            this.gbxRShift.Controls.Add(this.nudTargetEArea);
            this.gbxRShift.Controls.Add(this.label68);
            this.gbxRShift.Controls.Add(this.label67);
            this.gbxRShift.Controls.Add(this.nudAutoTthSigma);
            this.gbxRShift.Controls.Add(this.label62);
            this.gbxRShift.Controls.Add(this.label65);
            this.gbxRShift.Controls.Add(this.nudRShiftTh);
            this.gbxRShift.Controls.Add(this.labRSiftTh);
            this.gbxRShift.Controls.Add(this.btnRShiftInsp);
            this.gbxRShift.Location = new System.Drawing.Point(500, 42);
            this.gbxRShift.Name = "gbxRShift";
            this.gbxRShift.Size = new System.Drawing.Size(407, 258);
            this.gbxRShift.TabIndex = 103;
            this.gbxRShift.TabStop = false;
            this.gbxRShift.Text = "R層異偏移";
            // 
            // nudRShiftClosingSize
            // 
            this.nudRShiftClosingSize.Location = new System.Drawing.Point(298, 61);
            this.nudRShiftClosingSize.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudRShiftClosingSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudRShiftClosingSize.Name = "nudRShiftClosingSize";
            this.nudRShiftClosingSize.Size = new System.Drawing.Size(40, 22);
            this.nudRShiftClosingSize.TabIndex = 98;
            this.nudRShiftClosingSize.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // btnTargetRTest
            // 
            this.btnTargetRTest.Location = new System.Drawing.Point(162, 91);
            this.btnTargetRTest.Name = "btnTargetRTest";
            this.btnTargetRTest.Size = new System.Drawing.Size(75, 23);
            this.btnTargetRTest.TabIndex = 96;
            this.btnTargetRTest.Text = "測試";
            this.btnTargetRTest.UseVisualStyleBackColor = true;
            this.btnTargetRTest.Click += new System.EventHandler(this.btnTargetRTest_Click);
            // 
            // btnAutoThTest
            // 
            this.btnAutoThTest.Location = new System.Drawing.Point(162, 31);
            this.btnAutoThTest.Name = "btnAutoThTest";
            this.btnAutoThTest.Size = new System.Drawing.Size(75, 23);
            this.btnAutoThTest.TabIndex = 96;
            this.btnAutoThTest.Text = "測試";
            this.btnAutoThTest.UseVisualStyleBackColor = true;
            this.btnAutoThTest.Click += new System.EventHandler(this.btnAutoThTest_Click);
            // 
            // btnTargetETest
            // 
            this.btnTargetETest.Location = new System.Drawing.Point(162, 61);
            this.btnTargetETest.Name = "btnTargetETest";
            this.btnTargetETest.Size = new System.Drawing.Size(75, 23);
            this.btnTargetETest.TabIndex = 96;
            this.btnTargetETest.Text = "測試";
            this.btnTargetETest.UseVisualStyleBackColor = true;
            this.btnTargetETest.Click += new System.EventHandler(this.btnTargetETest_Click);
            // 
            // nudTargetRWidth
            // 
            this.nudTargetRWidth.Location = new System.Drawing.Point(102, 91);
            this.nudTargetRWidth.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.nudTargetRWidth.Name = "nudTargetRWidth";
            this.nudTargetRWidth.Size = new System.Drawing.Size(51, 22);
            this.nudTargetRWidth.TabIndex = 95;
            // 
            // nudTargetEArea
            // 
            this.nudTargetEArea.Location = new System.Drawing.Point(102, 62);
            this.nudTargetEArea.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.nudTargetEArea.Name = "nudTargetEArea";
            this.nudTargetEArea.Size = new System.Drawing.Size(51, 22);
            this.nudTargetEArea.TabIndex = 95;
            // 
            // label68
            // 
            this.label68.AutoSize = true;
            this.label68.Location = new System.Drawing.Point(6, 93);
            this.label68.Name = "label68";
            this.label68.Size = new System.Drawing.Size(56, 15);
            this.label68.TabIndex = 94;
            this.label68.Text = "R層寬度 : ";
            // 
            // label67
            // 
            this.label67.AutoSize = true;
            this.label67.Location = new System.Drawing.Point(6, 64);
            this.label67.Name = "label67";
            this.label67.Size = new System.Drawing.Size(60, 15);
            this.label67.TabIndex = 94;
            this.label67.Text = "電極面積 : ";
            // 
            // nudAutoTthSigma
            // 
            this.nudAutoTthSigma.DecimalPlaces = 1;
            this.nudAutoTthSigma.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudAutoTthSigma.Location = new System.Drawing.Point(102, 32);
            this.nudAutoTthSigma.Name = "nudAutoTthSigma";
            this.nudAutoTthSigma.Size = new System.Drawing.Size(51, 22);
            this.nudAutoTthSigma.TabIndex = 93;
            // 
            // label62
            // 
            this.label62.AutoSize = true;
            this.label62.Location = new System.Drawing.Point(6, 34);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(58, 15);
            this.label62.TabIndex = 92;
            this.label62.Text = "Auto Th : ";
            // 
            // label65
            // 
            this.label65.AutoSize = true;
            this.label65.Location = new System.Drawing.Point(159, 126);
            this.label65.Name = "label65";
            this.label65.Size = new System.Drawing.Size(32, 15);
            this.label65.TabIndex = 10;
            this.label65.Text = "Pixel";
            // 
            // nudRShiftTh
            // 
            this.nudRShiftTh.Location = new System.Drawing.Point(102, 121);
            this.nudRShiftTh.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudRShiftTh.Name = "nudRShiftTh";
            this.nudRShiftTh.Size = new System.Drawing.Size(51, 22);
            this.nudRShiftTh.TabIndex = 1;
            // 
            // labRSiftTh
            // 
            this.labRSiftTh.AutoSize = true;
            this.labRSiftTh.Location = new System.Drawing.Point(6, 123);
            this.labRSiftTh.Name = "labRSiftTh";
            this.labRSiftTh.Size = new System.Drawing.Size(49, 15);
            this.labRSiftTh.TabIndex = 0;
            this.labRSiftTh.Text = "偏移量 : ";
            // 
            // btnRShiftInsp
            // 
            this.btnRShiftInsp.BackColor = System.Drawing.Color.LightGray;
            this.btnRShiftInsp.Location = new System.Drawing.Point(307, 220);
            this.btnRShiftInsp.Name = "btnRShiftInsp";
            this.btnRShiftInsp.Size = new System.Drawing.Size(94, 32);
            this.btnRShiftInsp.TabIndex = 91;
            this.btnRShiftInsp.Text = "檢測";
            this.btnRShiftInsp.UseVisualStyleBackColor = false;
            this.btnRShiftInsp.Click += new System.EventHandler(this.btnRShiftInsp_Click);
            // 
            // gbxRDefect
            // 
            this.gbxRDefect.Controls.Add(this.button1);
            this.gbxRDefect.Controls.Add(this.cbxRDefectHeightEnabled);
            this.gbxRDefect.Controls.Add(this.nudRDefectSkipHeight);
            this.gbxRDefect.Controls.Add(this.nudRDefectExtHeight);
            this.gbxRDefect.Controls.Add(this.txbRDefectHeightMin);
            this.gbxRDefect.Controls.Add(this.nudRDefectSkipWidth);
            this.gbxRDefect.Controls.Add(this.nudRDefectExtWidth);
            this.gbxRDefect.Controls.Add(this.txbRDefectWidthMin);
            this.gbxRDefect.Controls.Add(this.label69);
            this.gbxRDefect.Controls.Add(this.label31);
            this.gbxRDefect.Controls.Add(this.cbxRDefectWidthEnabled);
            this.gbxRDefect.Controls.Add(this.label15);
            this.gbxRDefect.Controls.Add(this.cbxRDefectAreaEnabled);
            this.gbxRDefect.Controls.Add(this.label16);
            this.gbxRDefect.Controls.Add(this.label17);
            this.gbxRDefect.Controls.Add(this.label18);
            this.gbxRDefect.Controls.Add(this.labRDefectBS);
            this.gbxRDefect.Controls.Add(this.labRDefectDS);
            this.gbxRDefect.Controls.Add(this.txbRDefectMinArea);
            this.gbxRDefect.Controls.Add(this.labRDefectum2);
            this.gbxRDefect.Controls.Add(this.labRDefectMinArea);
            this.gbxRDefect.Controls.Add(this.labRDefectID);
            this.gbxRDefect.Controls.Add(this.labRDefectBrightTH);
            this.gbxRDefect.Controls.Add(this.txbRDefectID);
            this.gbxRDefect.Controls.Add(this.labRDefectDarkTh);
            this.gbxRDefect.Controls.Add(this.btnRDefectBrightTh);
            this.gbxRDefect.Controls.Add(this.btnRDefectDarkTh);
            this.gbxRDefect.Controls.Add(this.nudRDefectBrightMaxTh);
            this.gbxRDefect.Controls.Add(this.nudRDefectBrightMinTh);
            this.gbxRDefect.Controls.Add(this.nudRDefectDarkMaxTh);
            this.gbxRDefect.Controls.Add(this.nudRDefectDarkMinTh);
            this.gbxRDefect.Location = new System.Drawing.Point(12, 34);
            this.gbxRDefect.Name = "gbxRDefect";
            this.gbxRDefect.Size = new System.Drawing.Size(338, 268);
            this.gbxRDefect.TabIndex = 103;
            this.gbxRDefect.TabStop = false;
            this.gbxRDefect.Text = "R層異常參數設定";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.LightGray;
            this.button1.Location = new System.Drawing.Point(209, 54);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(96, 32);
            this.button1.TabIndex = 106;
            this.button1.Text = "範圍測試";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // cbxRDefectHeightEnabled
            // 
            this.cbxRDefectHeightEnabled.AutoSize = true;
            this.cbxRDefectHeightEnabled.Location = new System.Drawing.Point(199, 246);
            this.cbxRDefectHeightEnabled.Name = "cbxRDefectHeightEnabled";
            this.cbxRDefectHeightEnabled.Size = new System.Drawing.Size(48, 19);
            this.cbxRDefectHeightEnabled.TabIndex = 124;
            this.cbxRDefectHeightEnabled.Text = "啟用";
            this.cbxRDefectHeightEnabled.UseVisualStyleBackColor = true;
            // 
            // nudRDefectSkipHeight
            // 
            this.nudRDefectSkipHeight.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudRDefectSkipHeight.Location = new System.Drawing.Point(150, 77);
            this.nudRDefectSkipHeight.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudRDefectSkipHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudRDefectSkipHeight.Name = "nudRDefectSkipHeight";
            this.nudRDefectSkipHeight.Size = new System.Drawing.Size(53, 25);
            this.nudRDefectSkipHeight.TabIndex = 107;
            this.nudRDefectSkipHeight.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudRDefectExtHeight
            // 
            this.nudRDefectExtHeight.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudRDefectExtHeight.Location = new System.Drawing.Point(150, 41);
            this.nudRDefectExtHeight.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudRDefectExtHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudRDefectExtHeight.Name = "nudRDefectExtHeight";
            this.nudRDefectExtHeight.Size = new System.Drawing.Size(53, 25);
            this.nudRDefectExtHeight.TabIndex = 107;
            this.nudRDefectExtHeight.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // txbRDefectHeightMin
            // 
            this.txbRDefectHeightMin.Location = new System.Drawing.Point(76, 244);
            this.txbRDefectHeightMin.Name = "txbRDefectHeightMin";
            this.txbRDefectHeightMin.Size = new System.Drawing.Size(53, 22);
            this.txbRDefectHeightMin.TabIndex = 122;
            this.txbRDefectHeightMin.Text = "20";
            // 
            // nudRDefectSkipWidth
            // 
            this.nudRDefectSkipWidth.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudRDefectSkipWidth.Location = new System.Drawing.Point(91, 77);
            this.nudRDefectSkipWidth.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudRDefectSkipWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudRDefectSkipWidth.Name = "nudRDefectSkipWidth";
            this.nudRDefectSkipWidth.Size = new System.Drawing.Size(53, 25);
            this.nudRDefectSkipWidth.TabIndex = 108;
            this.nudRDefectSkipWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudRDefectExtWidth
            // 
            this.nudRDefectExtWidth.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudRDefectExtWidth.Location = new System.Drawing.Point(91, 41);
            this.nudRDefectExtWidth.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudRDefectExtWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudRDefectExtWidth.Name = "nudRDefectExtWidth";
            this.nudRDefectExtWidth.Size = new System.Drawing.Size(53, 25);
            this.nudRDefectExtWidth.TabIndex = 108;
            this.nudRDefectExtWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // txbRDefectWidthMin
            // 
            this.txbRDefectWidthMin.Location = new System.Drawing.Point(76, 213);
            this.txbRDefectWidthMin.Name = "txbRDefectWidthMin";
            this.txbRDefectWidthMin.Size = new System.Drawing.Size(53, 22);
            this.txbRDefectWidthMin.TabIndex = 123;
            this.txbRDefectWidthMin.Text = "20";
            // 
            // label69
            // 
            this.label69.AutoSize = true;
            this.label69.Location = new System.Drawing.Point(6, 81);
            this.label69.Name = "label69";
            this.label69.Size = new System.Drawing.Size(76, 15);
            this.label69.TabIndex = 109;
            this.label69.Text = "邊緣縮減大小:";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(6, 45);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(76, 15);
            this.label31.TabIndex = 109;
            this.label31.Text = "邊緣擴張大小:";
            // 
            // cbxRDefectWidthEnabled
            // 
            this.cbxRDefectWidthEnabled.AutoSize = true;
            this.cbxRDefectWidthEnabled.Location = new System.Drawing.Point(199, 216);
            this.cbxRDefectWidthEnabled.Name = "cbxRDefectWidthEnabled";
            this.cbxRDefectWidthEnabled.Size = new System.Drawing.Size(48, 19);
            this.cbxRDefectWidthEnabled.TabIndex = 125;
            this.cbxRDefectWidthEnabled.Text = "啟用";
            this.cbxRDefectWidthEnabled.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(135, 246);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(32, 15);
            this.label15.TabIndex = 118;
            this.label15.Text = "(um)";
            // 
            // cbxRDefectAreaEnabled
            // 
            this.cbxRDefectAreaEnabled.AutoSize = true;
            this.cbxRDefectAreaEnabled.Location = new System.Drawing.Point(199, 184);
            this.cbxRDefectAreaEnabled.Name = "cbxRDefectAreaEnabled";
            this.cbxRDefectAreaEnabled.Size = new System.Drawing.Size(48, 19);
            this.cbxRDefectAreaEnabled.TabIndex = 126;
            this.cbxRDefectAreaEnabled.Text = "啟用";
            this.cbxRDefectAreaEnabled.UseVisualStyleBackColor = true;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(135, 216);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(32, 15);
            this.label16.TabIndex = 119;
            this.label16.Text = "(um)";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(4, 248);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(60, 15);
            this.label17.TabIndex = 120;
            this.label17.Text = "檢測高度 : ";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(4, 216);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(60, 15);
            this.label18.TabIndex = 121;
            this.label18.Text = "檢測寬度 : ";
            // 
            // labRDefectBS
            // 
            this.labRDefectBS.AutoSize = true;
            this.labRDefectBS.Location = new System.Drawing.Point(149, 156);
            this.labRDefectBS.Name = "labRDefectBS";
            this.labRDefectBS.Size = new System.Drawing.Size(18, 15);
            this.labRDefectBS.TabIndex = 99;
            this.labRDefectBS.Text = "～";
            // 
            // labRDefectDS
            // 
            this.labRDefectDS.AutoSize = true;
            this.labRDefectDS.Location = new System.Drawing.Point(149, 123);
            this.labRDefectDS.Name = "labRDefectDS";
            this.labRDefectDS.Size = new System.Drawing.Size(18, 15);
            this.labRDefectDS.TabIndex = 99;
            this.labRDefectDS.Text = "～";
            // 
            // txbRDefectMinArea
            // 
            this.txbRDefectMinArea.Location = new System.Drawing.Point(76, 182);
            this.txbRDefectMinArea.Name = "txbRDefectMinArea";
            this.txbRDefectMinArea.Size = new System.Drawing.Size(53, 22);
            this.txbRDefectMinArea.TabIndex = 98;
            this.txbRDefectMinArea.Text = "20";
            // 
            // labRDefectum2
            // 
            this.labRDefectum2.AutoSize = true;
            this.labRDefectum2.Location = new System.Drawing.Point(135, 185);
            this.labRDefectum2.Name = "labRDefectum2";
            this.labRDefectum2.Size = new System.Drawing.Size(47, 15);
            this.labRDefectum2.TabIndex = 97;
            this.labRDefectum2.Text = "(um^2)";
            // 
            // labRDefectMinArea
            // 
            this.labRDefectMinArea.AutoSize = true;
            this.labRDefectMinArea.Location = new System.Drawing.Point(4, 185);
            this.labRDefectMinArea.Name = "labRDefectMinArea";
            this.labRDefectMinArea.Size = new System.Drawing.Size(60, 15);
            this.labRDefectMinArea.TabIndex = 97;
            this.labRDefectMinArea.Text = "檢測面積 : ";
            // 
            // labRDefectID
            // 
            this.labRDefectID.AutoSize = true;
            this.labRDefectID.Location = new System.Drawing.Point(6, 18);
            this.labRDefectID.Name = "labRDefectID";
            this.labRDefectID.Size = new System.Drawing.Size(49, 15);
            this.labRDefectID.TabIndex = 91;
            this.labRDefectID.Text = "影像ID : ";
            // 
            // labRDefectBrightTH
            // 
            this.labRDefectBrightTH.AutoSize = true;
            this.labRDefectBrightTH.Location = new System.Drawing.Point(6, 156);
            this.labRDefectBrightTH.Name = "labRDefectBrightTH";
            this.labRDefectBrightTH.Size = new System.Drawing.Size(54, 15);
            this.labRDefectBrightTH.TabIndex = 95;
            this.labRDefectBrightTH.Text = "亮區閥值:";
            // 
            // txbRDefectID
            // 
            this.txbRDefectID.Location = new System.Drawing.Point(61, 15);
            this.txbRDefectID.Name = "txbRDefectID";
            this.txbRDefectID.Size = new System.Drawing.Size(54, 22);
            this.txbRDefectID.TabIndex = 90;
            this.txbRDefectID.Text = "0";
            // 
            // labRDefectDarkTh
            // 
            this.labRDefectDarkTh.AutoSize = true;
            this.labRDefectDarkTh.Location = new System.Drawing.Point(6, 123);
            this.labRDefectDarkTh.Name = "labRDefectDarkTh";
            this.labRDefectDarkTh.Size = new System.Drawing.Size(54, 15);
            this.labRDefectDarkTh.TabIndex = 96;
            this.labRDefectDarkTh.Text = "暗區閥值:";
            // 
            // btnRDefectBrightTh
            // 
            this.btnRDefectBrightTh.Location = new System.Drawing.Point(244, 153);
            this.btnRDefectBrightTh.Name = "btnRDefectBrightTh";
            this.btnRDefectBrightTh.Size = new System.Drawing.Size(54, 32);
            this.btnRDefectBrightTh.TabIndex = 92;
            this.btnRDefectBrightTh.Text = "設定";
            this.btnRDefectBrightTh.UseVisualStyleBackColor = true;
            this.btnRDefectBrightTh.Click += new System.EventHandler(this.btnRDefectBrightTh_Click);
            // 
            // btnRDefectDarkTh
            // 
            this.btnRDefectDarkTh.Location = new System.Drawing.Point(244, 115);
            this.btnRDefectDarkTh.Name = "btnRDefectDarkTh";
            this.btnRDefectDarkTh.Size = new System.Drawing.Size(54, 32);
            this.btnRDefectDarkTh.TabIndex = 92;
            this.btnRDefectDarkTh.Text = "設定";
            this.btnRDefectDarkTh.UseVisualStyleBackColor = true;
            this.btnRDefectDarkTh.Click += new System.EventHandler(this.btnRDefectDarkTh_Click);
            // 
            // nudRDefectBrightMaxTh
            // 
            this.nudRDefectBrightMaxTh.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudRDefectBrightMaxTh.Location = new System.Drawing.Point(185, 154);
            this.nudRDefectBrightMaxTh.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudRDefectBrightMaxTh.Name = "nudRDefectBrightMaxTh";
            this.nudRDefectBrightMaxTh.Size = new System.Drawing.Size(53, 25);
            this.nudRDefectBrightMaxTh.TabIndex = 93;
            // 
            // nudRDefectBrightMinTh
            // 
            this.nudRDefectBrightMinTh.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudRDefectBrightMinTh.Location = new System.Drawing.Point(78, 154);
            this.nudRDefectBrightMinTh.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudRDefectBrightMinTh.Name = "nudRDefectBrightMinTh";
            this.nudRDefectBrightMinTh.Size = new System.Drawing.Size(53, 25);
            this.nudRDefectBrightMinTh.TabIndex = 93;
            // 
            // nudRDefectDarkMaxTh
            // 
            this.nudRDefectDarkMaxTh.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudRDefectDarkMaxTh.Location = new System.Drawing.Point(184, 118);
            this.nudRDefectDarkMaxTh.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudRDefectDarkMaxTh.Name = "nudRDefectDarkMaxTh";
            this.nudRDefectDarkMaxTh.Size = new System.Drawing.Size(54, 25);
            this.nudRDefectDarkMaxTh.TabIndex = 94;
            // 
            // nudRDefectDarkMinTh
            // 
            this.nudRDefectDarkMinTh.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.nudRDefectDarkMinTh.Location = new System.Drawing.Point(77, 118);
            this.nudRDefectDarkMinTh.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudRDefectDarkMinTh.Name = "nudRDefectDarkMinTh";
            this.nudRDefectDarkMinTh.Size = new System.Drawing.Size(54, 25);
            this.nudRDefectDarkMinTh.TabIndex = 94;
            // 
            // btnRDefectPartitionTest
            // 
            this.btnRDefectPartitionTest.BackColor = System.Drawing.Color.LightGray;
            this.btnRDefectPartitionTest.Location = new System.Drawing.Point(356, 230);
            this.btnRDefectPartitionTest.Name = "btnRDefectPartitionTest";
            this.btnRDefectPartitionTest.Size = new System.Drawing.Size(124, 32);
            this.btnRDefectPartitionTest.TabIndex = 92;
            this.btnRDefectPartitionTest.Text = "分區測試";
            this.btnRDefectPartitionTest.UseVisualStyleBackColor = false;
            this.btnRDefectPartitionTest.Click += new System.EventHandler(this.btnRDefectPartitionTest_Click);
            // 
            // btnRDefectTest
            // 
            this.btnRDefectTest.BackColor = System.Drawing.Color.LightGray;
            this.btnRDefectTest.Location = new System.Drawing.Point(356, 268);
            this.btnRDefectTest.Name = "btnRDefectTest";
            this.btnRDefectTest.Size = new System.Drawing.Size(124, 32);
            this.btnRDefectTest.TabIndex = 91;
            this.btnRDefectTest.Text = "檢測";
            this.btnRDefectTest.UseVisualStyleBackColor = false;
            this.btnRDefectTest.Click += new System.EventHandler(this.btnRShiftTest_Click);
            // 
            // button_SaveParam
            // 
            this.button_SaveParam.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_SaveParam.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.button_SaveParam.Location = new System.Drawing.Point(302, 7);
            this.button_SaveParam.Name = "button_SaveParam";
            this.button_SaveParam.Size = new System.Drawing.Size(92, 34);
            this.button_SaveParam.TabIndex = 5;
            this.button_SaveParam.Text = "參數儲存";
            this.button_SaveParam.UseVisualStyleBackColor = true;
            this.button_SaveParam.Click += new System.EventHandler(this.button_SaveParam_Click);
            // 
            // button_Inspection
            // 
            this.button_Inspection.BackColor = System.Drawing.Color.Lime;
            this.button_Inspection.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_Inspection.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.button_Inspection.Location = new System.Drawing.Point(3, 7);
            this.button_Inspection.Name = "button_Inspection";
            this.button_Inspection.Size = new System.Drawing.Size(92, 34);
            this.button_Inspection.TabIndex = 4;
            this.button_Inspection.Text = "檢測";
            this.button_Inspection.UseVisualStyleBackColor = false;
            this.button_Inspection.Click += new System.EventHandler(this.button_Inspection_Click);
            // 
            // button_LoadParam
            // 
            this.button_LoadParam.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_LoadParam.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.button_LoadParam.Location = new System.Drawing.Point(204, 7);
            this.button_LoadParam.Name = "button_LoadParam";
            this.button_LoadParam.Size = new System.Drawing.Size(92, 34);
            this.button_LoadParam.TabIndex = 6;
            this.button_LoadParam.Text = "參數載入";
            this.button_LoadParam.UseVisualStyleBackColor = true;
            this.button_LoadParam.Click += new System.EventHandler(this.button_LoadParam_Click);
            // 
            // btnLoadGolden
            // 
            this.btnLoadGolden.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnLoadGolden.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.btnLoadGolden.Location = new System.Drawing.Point(106, 7);
            this.btnLoadGolden.Name = "btnLoadGolden";
            this.btnLoadGolden.Size = new System.Drawing.Size(92, 34);
            this.btnLoadGolden.TabIndex = 122;
            this.btnLoadGolden.Text = "讀取教導影像";
            this.btnLoadGolden.UseVisualStyleBackColor = true;
            this.btnLoadGolden.Click += new System.EventHandler(this.btnLoadGolden_Click);
            // 
            // txbGrayValue
            // 
            this.txbGrayValue.Font = new System.Drawing.Font("微軟正黑體", 8F, System.Drawing.FontStyle.Bold);
            this.txbGrayValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txbGrayValue.Location = new System.Drawing.Point(899, 58);
            this.txbGrayValue.Name = "txbGrayValue";
            this.txbGrayValue.ReadOnly = true;
            this.txbGrayValue.Size = new System.Drawing.Size(231, 22);
            this.txbGrayValue.TabIndex = 78;
            this.txbGrayValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txt_ColorValue
            // 
            this.txt_ColorValue.Font = new System.Drawing.Font("微軟正黑體", 8F, System.Drawing.FontStyle.Bold);
            this.txt_ColorValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txt_ColorValue.Location = new System.Drawing.Point(899, 33);
            this.txt_ColorValue.Name = "txt_ColorValue";
            this.txt_ColorValue.ReadOnly = true;
            this.txt_ColorValue.Size = new System.Drawing.Size(231, 22);
            this.txt_ColorValue.TabIndex = 78;
            this.txt_ColorValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txt_CursorCoordinate
            // 
            this.txt_CursorCoordinate.Font = new System.Drawing.Font("微軟正黑體", 8F, System.Drawing.FontStyle.Bold);
            this.txt_CursorCoordinate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txt_CursorCoordinate.Location = new System.Drawing.Point(899, 9);
            this.txt_CursorCoordinate.Name = "txt_CursorCoordinate";
            this.txt_CursorCoordinate.ReadOnly = true;
            this.txt_CursorCoordinate.Size = new System.Drawing.Size(231, 22);
            this.txt_CursorCoordinate.TabIndex = 76;
            this.txt_CursorCoordinate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("微軟正黑體", 8F, System.Drawing.FontStyle.Bold);
            this.label20.ForeColor = System.Drawing.Color.Black;
            this.label20.Location = new System.Drawing.Point(797, 61);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(46, 15);
            this.label20.TabIndex = 77;
            this.label20.Text = "灰階值 :";
            // 
            // labXY
            // 
            this.labXY.AutoSize = true;
            this.labXY.Font = new System.Drawing.Font("微軟正黑體", 8F, System.Drawing.FontStyle.Bold);
            this.labXY.ForeColor = System.Drawing.Color.Black;
            this.labXY.Location = new System.Drawing.Point(797, 12);
            this.labXY.Name = "labXY";
            this.labXY.Size = new System.Drawing.Size(61, 15);
            this.labXY.TabIndex = 75;
            this.labXY.Text = "座標 (x, y):";
            // 
            // labColorValue
            // 
            this.labColorValue.AutoSize = true;
            this.labColorValue.Font = new System.Drawing.Font("微軟正黑體", 8F, System.Drawing.FontStyle.Bold);
            this.labColorValue.ForeColor = System.Drawing.Color.Black;
            this.labColorValue.Location = new System.Drawing.Point(797, 37);
            this.labColorValue.Name = "labColorValue";
            this.labColorValue.Size = new System.Drawing.Size(57, 15);
            this.labColorValue.TabIndex = 77;
            this.labColorValue.Text = "色彩資訊 :";
            // 
            // panelImageControl
            // 
            this.panelImageControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelImageControl.Controls.Add(this.combxDisplayImg);
            this.panelImageControl.Controls.Add(this.comboBoxDisplayType);
            this.panelImageControl.Controls.Add(this.btnLoadImg);
            this.panelImageControl.Controls.Add(this.btnAddImsge);
            this.panelImageControl.Controls.Add(this.btnSaveImg);
            this.panelImageControl.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.panelImageControl.Location = new System.Drawing.Point(6, 5);
            this.panelImageControl.Name = "panelImageControl";
            this.panelImageControl.Size = new System.Drawing.Size(368, 50);
            this.panelImageControl.TabIndex = 124;
            // 
            // panelInspectionControl
            // 
            this.panelInspectionControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelInspectionControl.Controls.Add(this.button_SaveParam);
            this.panelInspectionControl.Controls.Add(this.button_Inspection);
            this.panelInspectionControl.Controls.Add(this.btnLoadGolden);
            this.panelInspectionControl.Controls.Add(this.button_LoadParam);
            this.panelInspectionControl.Location = new System.Drawing.Point(376, 5);
            this.panelInspectionControl.Name = "panelInspectionControl";
            this.panelInspectionControl.Size = new System.Drawing.Size(414, 50);
            this.panelInspectionControl.TabIndex = 124;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Interval = 20;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // InductanceInspUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.txbGrayValue);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.panelInspectionControl);
            this.Controls.Add(this.panelImageControl);
            this.Controls.Add(this.tbInspParamSetting);
            this.Controls.Add(this.txt_ColorValue);
            this.Controls.Add(this.tabControl_Info);
            this.Controls.Add(this.txt_CursorCoordinate);
            this.Controls.Add(this.labXY);
            this.Controls.Add(this.labColorValue);
            this.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "InductanceInspUC";
            this.Size = new System.Drawing.Size(1545, 856);
            this.tabControl_Info.ResumeLayout(false);
            this.tabPage_Result.ResumeLayout(false);
            this.tabPage_Result.PerformLayout();
            this.tpSetupParam.ResumeLayout(false);
            this.tpSetupParam.PerformLayout();
            this.gbx_BatchTest.ResumeLayout(false);
            this.gbx_BatchTest.PerformLayout();
            this.gbx_BatchTest_Info.ResumeLayout(false);
            this.gbx_BatchTest_Info.PerformLayout();
            this.gbx_BatchTest_Result.ResumeLayout(false);
            this.gbxSynchronizePath.ResumeLayout(false);
            this.gbxSynchronizePath.PerformLayout();
            this.gbxMethod.ResumeLayout(false);
            this.gbxOtherParam.ResumeLayout(false);
            this.gbxOtherParam.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudImgCount)).EndInit();
            this.gbxAIType.ResumeLayout(false);
            this.gbxSaveAOIImgSetting.ResumeLayout(false);
            this.gbxSaveAOIImgSetting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDAVSImageId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDAVSBand3ImageIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDAVSBand2ImageIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDAVSBand1ImageIndex)).EndInit();
            this.gbxSkipParam.ResumeLayout(false);
            this.gbxSkipParam.PerformLayout();
            this.gbxMatchingParam.ResumeLayout(false);
            this.gbxMatchingParam.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudScaleSize)).EndInit();
            this.panMatching.ResumeLayout(false);
            this.panMatching.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMatchNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGreediness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOverlap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudScaleMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudScaleMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAngleStep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinContrast)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAngleExtent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinObjSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudContrastHigh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudContrast)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAngleStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumLevels)).EndInit();
            this.tbInspParamSetting.ResumeLayout(false);
            this.tpPatternMatch.ResumeLayout(false);
            this.gbxParam.ResumeLayout(false);
            this.gbxParam.PerformLayout();
            this.gbx_PatternMatch_PreProcess.ResumeLayout(false);
            this.gbx_PatternMatch_PreProcess.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudOpeningNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFillupMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudClosingNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFillupMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInspectImgID)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMatchCloseHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMatchCloseWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMatchOpenHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMatchOpenWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAMinScore)).EndInit();
            this.tpAlgoImage.ResumeLayout(false);
            this.gbx_AlgoImage.ResumeLayout(false);
            this.gbx_AlgoImage.PerformLayout();
            this.gbx_AlgoImage_UsedRegion.ResumeLayout(false);
            this.gbx_AlgoImage_UsedRegion.PerformLayout();
            this.tpRgionMethod.ResumeLayout(false);
            this.gbxAdjust.ResumeLayout(false);
            this.gbxAdjust.PerformLayout();
            this.gbxUsedRegion.ResumeLayout(false);
            this.gbxEditRegionList.ResumeLayout(false);
            this.gbxMethodList.ResumeLayout(false);
            this.gbxMtehodThreshold.ResumeLayout(false);
            this.gbxMtehodThreshold.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMethodThImageID)).EndInit();
            this.tbThMethod.ResumeLayout(false);
            this.tpSingleTh.ResumeLayout(false);
            this.tpSingleTh.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMethodThMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMethodThMax)).EndInit();
            this.tpDualTh.ResumeLayout(false);
            this.tpDualTh.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDualThThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDualThMinSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDualThMinGray)).EndInit();
            this.tpAutoTh.ResumeLayout(false);
            this.tpAutoTh.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAutoThSigma)).EndInit();
            this.tpDynTh.ResumeLayout(false);
            this.tpDynTh.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMeanHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDynOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMeanWidth)).EndInit();
            this.tpUserSet.ResumeLayout(false);
            this.tpUserSet.PerformLayout();
            this.labAnisometry.ResumeLayout(false);
            this.labAnisometry.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPriority)).EndInit();
            this.gbxRegionFeature.ResumeLayout(false);
            this.gbxSurfaceFeature.ResumeLayout(false);
            this.gbxSurfaceFeature.PerformLayout();
            this.gbxInfo.ResumeLayout(false);
            this.gbxInfo.PerformLayout();
            this.gbxHoleGrayFeature.ResumeLayout(false);
            this.gbxHoleGrayFeature.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudInspImageIndex)).EndInit();
            this.gbxAlgorithmList.ResumeLayout(false);
            this.Inner.ResumeLayout(false);
            this.Inner.PerformLayout();
            this.gbxInnerParam.ResumeLayout(false);
            this.gbxInnerParam.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudInnerLTh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInnerEdgeSkipHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInnerEdgeSkipSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInnerHTH)).EndInit();
            this.gbxinnerDefectSpec.ResumeLayout(false);
            this.gbxinnerDefectSpec.PerformLayout();
            this.Outer.ResumeLayout(false);
            this.Outer.PerformLayout();
            this.gbxOuterParam.ResumeLayout(false);
            this.gbxOuterParam.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudOuterLTh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOuterEdgeSkipHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOuterEdgeSkipSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOuterHTH)).EndInit();
            this.gbxOuterDefectSpec.ResumeLayout(false);
            this.gbxOuterDefectSpec.PerformLayout();
            this.Thin.ResumeLayout(false);
            this.Thin.PerformLayout();
            this.gbxThinScratch.ResumeLayout(false);
            this.gbxThinScratch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSensitivity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThinScratchCloseH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThinScratchCloseW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThinScratchEdgeExHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThinScratchEdgeSkipH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThinScratchOpenH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThinScratchOpenW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThinScratchEdgeExWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThinScratchEdgeSkipW)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudThinHatDarkTh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThinHatBrightTh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThinHatOpenWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThinHatCloseHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThinHatSkipH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThinHatCloseWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThinHatSkipW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThinHatOpenHeight)).EndInit();
            this.gbxHistoEq.ResumeLayout(false);
            this.gbxHistoEq.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudHistoEqTh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHistoOpenWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHistoCloseHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHistoCloseWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHistoEdgeSkipH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHistoOpenHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHistoEdgeSkipW)).EndInit();
            this.gbxThinParam.ResumeLayout(false);
            this.gbxThinParam.PerformLayout();
            this.gbxAvgTest.ResumeLayout(false);
            this.gbxAvgTest.PerformLayout();
            this.gbxSumArea.ResumeLayout(false);
            this.gbxSumArea.PerformLayout();
            this.gbxSmallArea.ResumeLayout(false);
            this.gbxSmallArea.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTophatDarkTh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTophatBrightTh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSESizeHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTopHatEdgeExpHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudExtHTh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSESizeWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTopHatEdgeExpWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudExtH)).EndInit();
            this.gbxLargeArea.ResumeLayout(false);
            this.gbxLargeArea.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudThinDarkTh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMeanOpenRad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThinEdgeSkipH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMeanCloseRad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThinEdgeSkipW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThinBrightTh)).EndInit();
            this.Scratch.ResumeLayout(false);
            this.Scratch.PerformLayout();
            this.gbxScratchOuter.ResumeLayout(false);
            this.gbxScratchOuter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudScratchOutTH)).EndInit();
            this.gbxScratch.ResumeLayout(false);
            this.gbxScratch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudScratchInTH)).EndInit();
            this.Stain.ResumeLayout(false);
            this.Stain.PerformLayout();
            this.gbxStain.ResumeLayout(false);
            this.gbxStain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudStainBrightTh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStainDarkTh)).EndInit();
            this.RDefect.ResumeLayout(false);
            this.RDefect.PerformLayout();
            this.gbxRShift.ResumeLayout(false);
            this.gbxRShift.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRShiftClosingSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTargetRWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTargetEArea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAutoTthSigma)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRShiftTh)).EndInit();
            this.gbxRDefect.ResumeLayout(false);
            this.gbxRDefect.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRDefectSkipHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRDefectExtHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRDefectSkipWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRDefectExtWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRDefectBrightMaxTh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRDefectBrightMinTh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRDefectDarkMaxTh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRDefectDarkMinTh)).EndInit();
            this.panelImageControl.ResumeLayout(false);
            this.panelInspectionControl.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl_Info;
        private System.Windows.Forms.TabPage tabPage_Result;
        private HalconDotNet.HSmartWindowControl DisplayWindows;
        private System.Windows.Forms.Button btnLoadImg;
        private System.Windows.Forms.Button button_Add;
        private System.Windows.Forms.TabControl tbInspParamSetting;
        private System.Windows.Forms.TabPage tpPatternMatch;
        private System.Windows.Forms.Button button_LoadParam;
        private System.Windows.Forms.Button button_SaveParam;
        private System.Windows.Forms.Button button_Inspection;
        private System.Windows.Forms.CheckBox cbxASetPattern;
        private System.Windows.Forms.Button btnATeachPattern;
        private System.Windows.Forms.Button btnATestMatch;
        private System.Windows.Forms.NumericUpDown nudAMinScore;
        private System.Windows.Forms.Label labAMinScore;
        private System.Windows.Forms.CheckBox cbxCellRegion;
        private System.Windows.Forms.Button btnDAVSSetting;
        private System.Windows.Forms.GroupBox gbxParam;
        private System.Windows.Forms.Button btnThSetup;
        private System.Windows.Forms.CheckBox cbxThEnabled;
        private System.Windows.Forms.Label labMatchID;
        private System.Windows.Forms.TabPage Inner;
        private System.Windows.Forms.TabPage Outer;
        private System.Windows.Forms.Button btnSegCellTest;
        private System.Windows.Forms.GroupBox gbxinnerDefectSpec;
        private System.Windows.Forms.Label labInnerHighTH;
        private System.Windows.Forms.Button btnInnerThSetup;
        private System.Windows.Forms.Label labInnerLowTh;
        private System.Windows.Forms.NumericUpDown nudInnerHTH;
        private System.Windows.Forms.NumericUpDown nudInnerLTh;
        private System.Windows.Forms.Label labInnerImgIndex;
        private System.Windows.Forms.TextBox txbInnerImgIndex;
        private System.Windows.Forms.GroupBox gbxInnerParam;
        private System.Windows.Forms.CheckBox cbxInnerEnabled;
        private System.Windows.Forms.TextBox txbInnerMinA;
        private System.Windows.Forms.TextBox txbInnerMinH;
        private System.Windows.Forms.TextBox txbInnerMinW;
        private System.Windows.Forms.Label labInnerMinA;
        private System.Windows.Forms.Label labInnerMinH;
        private System.Windows.Forms.Label labInnerMinW;
        private System.Windows.Forms.Button btnInnerInsp;
        private System.Windows.Forms.Label labInnerEdgeSkipSize;
        private System.Windows.Forms.NumericUpDown nudInnerEdgeSkipSize;
        private System.Windows.Forms.Button btnOuterInsp;
        private System.Windows.Forms.GroupBox gbxOuterParam;
        private System.Windows.Forms.NumericUpDown nudOuterLTh;
        private System.Windows.Forms.TextBox txbOuterImgIndex;
        private System.Windows.Forms.CheckBox cbxOuterEnabled;
        private System.Windows.Forms.Label labOuterEdgeSkipSize;
        private System.Windows.Forms.Label labOuterHighTH;
        private System.Windows.Forms.Label labOuterImgIndex;
        private System.Windows.Forms.Button btnOuterThSetup;
        private System.Windows.Forms.NumericUpDown nudOuterEdgeSkipSize;
        private System.Windows.Forms.NumericUpDown nudOuterHTH;
        private System.Windows.Forms.Label labOuterLowTh;
        private System.Windows.Forms.GroupBox gbxOuterDefectSpec;
        private System.Windows.Forms.TextBox txbOuterMinA;
        private System.Windows.Forms.Label labOuterMinA;
        private System.Windows.Forms.Label labOuterMinW;
        private System.Windows.Forms.TextBox txbOuterMinW;
        private System.Windows.Forms.Label labOuterMinH;
        private System.Windows.Forms.TextBox txbOuterMinH;
        private System.Windows.Forms.GroupBox gbxMatchingParam;
        private System.Windows.Forms.NumericUpDown nudNumLevels;
        private System.Windows.Forms.Label labNumLevels;
        private System.Windows.Forms.NumericUpDown nudAngleStep;
        private System.Windows.Forms.Label labAngleStep;
        private System.Windows.Forms.NumericUpDown nudAngleExtent;
        private System.Windows.Forms.Label labAngleExtent;
        private System.Windows.Forms.NumericUpDown nudAngleStart;
        private System.Windows.Forms.Label labAngleStart;
        private System.Windows.Forms.ComboBox cbxMetric;
        private System.Windows.Forms.Label labMetric;
        private System.Windows.Forms.ComboBox cbxOptimization;
        private System.Windows.Forms.Label labOptimization;
        private System.Windows.Forms.NumericUpDown nudMinContrast;
        private System.Windows.Forms.Label labMinContrast;
        private System.Windows.Forms.NumericUpDown nudContrast;
        private System.Windows.Forms.Label labContrastLow;
        private System.Windows.Forms.CheckBox cbxMinContrastAuto;
        private System.Windows.Forms.CheckBox cbxContrastAuto;
        private System.Windows.Forms.CheckBox cbxAngleStepAuto;
        private System.Windows.Forms.Panel panMatching;
        private System.Windows.Forms.Label labGreediness;
        private System.Windows.Forms.Label labOverlap;
        private System.Windows.Forms.NumericUpDown nudGreediness;
        private System.Windows.Forms.NumericUpDown nudOverlap;
        private System.Windows.Forms.ComboBox cbxSubPixel;
        private System.Windows.Forms.Label labSubPixel;
        private System.Windows.Forms.Button btnInnerMultiTh;
        private System.Windows.Forms.Button btnOuterMultiTh;
        private System.Windows.Forms.ComboBox combxAIType;
        private System.Windows.Forms.TabPage tpSetupParam;
        private System.Windows.Forms.GroupBox gbxSkipParam;
        private System.Windows.Forms.ComboBox combxTestModeType;
        private System.Windows.Forms.CheckBox cbxTestModeEnabled;
        private System.Windows.Forms.GroupBox gbxSaveAOIImgSetting;
        private System.Windows.Forms.ComboBox combxSaveImgType;
        private System.Windows.Forms.CheckBox cbxSaveImgEnable;
        private System.Windows.Forms.ComboBox combxDisplayImg;
        private System.Windows.Forms.GroupBox gbxAIType;
        private System.Windows.Forms.GroupBox gbxOtherParam;
        private System.Windows.Forms.Label labImgCount;
        private System.Windows.Forms.NumericUpDown nudImgCount;
        private System.Windows.Forms.GroupBox gbxInfo;
        private System.Windows.Forms.Label labA;
        private System.Windows.Forms.Label labH;
        private System.Windows.Forms.Label labW;
        private System.Windows.Forms.Label labArea;
        private System.Windows.Forms.Label labHeight;
        private System.Windows.Forms.Label labWidth;
        private System.Windows.Forms.TextBox txbResolution;
        private System.Windows.Forms.Label labResolution;
        private System.Windows.Forms.Label labumpixel;
        private System.Windows.Forms.ComboBox combxBand;
        private System.Windows.Forms.Label labBand;
        private System.Windows.Forms.CheckBox cbxMatchThEnabled;
        private System.Windows.Forms.NumericUpDown nudScaleMax;
        private System.Windows.Forms.NumericUpDown nudScaleMin;
        private System.Windows.Forms.Label labScaleMax;
        private System.Windows.Forms.Label labScaleMin;
        private System.Windows.Forms.Label labMorphologyNum;
        private System.Windows.Forms.NumericUpDown nudOpeningNum;
        private System.Windows.Forms.TextBox txbInnerName;
        private System.Windows.Forms.TextBox txbOuterName;
        private System.Windows.Forms.TextBox txt_ColorValue;
        private System.Windows.Forms.TextBox txt_CursorCoordinate;
        private System.Windows.Forms.Label labXY;
        private System.Windows.Forms.Label labColorValue;
        private System.Windows.Forms.TabPage Thin;
        private System.Windows.Forms.Button btnThinInsp;
        private System.Windows.Forms.TabPage Scratch;
        private System.Windows.Forms.Button btnScratchInsp;
        private System.Windows.Forms.TabPage RDefect;
        private System.Windows.Forms.Button btnRDefectTest;
        private System.Windows.Forms.Button btnThinPartitionTest;
        private System.Windows.Forms.Button btnRDefectPartitionTest;
        private System.Windows.Forms.TabPage Stain;
        private System.Windows.Forms.Button btnStainPartitionTest;
        private System.Windows.Forms.Button btnStainTest;
        private System.Windows.Forms.TextBox txbThinName;
        private System.Windows.Forms.CheckBox cbxThinEnabled;
        private System.Windows.Forms.GroupBox gbxThinParam;
        private System.Windows.Forms.Label labThinID;
        private System.Windows.Forms.Label labBrightTH;
        private System.Windows.Forms.TextBox txbThinID;
        private System.Windows.Forms.Label labDarkTH;
        private System.Windows.Forms.NumericUpDown nudThinBrightTh;
        private System.Windows.Forms.NumericUpDown nudThinDarkTh;
        private System.Windows.Forms.Label labMinArea;
        private System.Windows.Forms.TextBox txbThinMinArea;
        private System.Windows.Forms.Label labThinum;
        private System.Windows.Forms.Label labPartGrayValue;
        private System.Windows.Forms.TextBox txbPartGrayValue;
        private System.Windows.Forms.TextBox txbScratchName;
        private System.Windows.Forms.CheckBox cbxScratchEnabled;
        private System.Windows.Forms.GroupBox gbxScratch;
        private System.Windows.Forms.TextBox txbScratchMinArea;
        private System.Windows.Forms.Label labScratchum2;
        private System.Windows.Forms.Label labScratchMinArea;
        private System.Windows.Forms.Label labScratchID;
        private System.Windows.Forms.Label labScratchOutTh;
        private System.Windows.Forms.TextBox txbScratchID;
        private System.Windows.Forms.Label labScratchInTH;
        private System.Windows.Forms.Button btnScratchOutTh;
        private System.Windows.Forms.Button btnScratchInTH;
        private System.Windows.Forms.NumericUpDown nudScratchOutTH;
        private System.Windows.Forms.NumericUpDown nudScratchInTH;
        private System.Windows.Forms.TextBox txbStainName;
        private System.Windows.Forms.CheckBox cbxStainEnabled;
        private System.Windows.Forms.GroupBox gbxStain;
        private System.Windows.Forms.TextBox txbStainMinArea;
        private System.Windows.Forms.Label labStainum2;
        private System.Windows.Forms.Label labStainMinArea;
        private System.Windows.Forms.Label labStainID;
        private System.Windows.Forms.Label labStainBrightTh;
        private System.Windows.Forms.TextBox txbStainID;
        private System.Windows.Forms.Label labStainDarkTh;
        private System.Windows.Forms.Button btnStainBrightTh;
        private System.Windows.Forms.Button btnStainDarkTh;
        private System.Windows.Forms.NumericUpDown nudStainBrightTh;
        private System.Windows.Forms.NumericUpDown nudStainDarkTh;
        private System.Windows.Forms.TextBox txbRDefectName;
        private System.Windows.Forms.CheckBox cbxRDefectEnabled;
        private System.Windows.Forms.GroupBox gbxRDefect;
        private System.Windows.Forms.TextBox txbRDefectMinArea;
        private System.Windows.Forms.Label labRDefectum2;
        private System.Windows.Forms.Label labRDefectMinArea;
        private System.Windows.Forms.Label labRDefectID;
        private System.Windows.Forms.Label labRDefectBrightTH;
        private System.Windows.Forms.TextBox txbRDefectID;
        private System.Windows.Forms.Label labRDefectDarkTh;
        private System.Windows.Forms.Button btnRDefectBrightTh;
        private System.Windows.Forms.Button btnRDefectDarkTh;
        private System.Windows.Forms.NumericUpDown nudRDefectBrightMinTh;
        private System.Windows.Forms.NumericUpDown nudRDefectDarkMinTh;
        private System.Windows.Forms.NumericUpDown nudRDefectDarkMaxTh;
        private System.Windows.Forms.NumericUpDown nudRDefectBrightMaxTh;
        private System.Windows.Forms.Label labRDefectBS;
        private System.Windows.Forms.Label labRDefectDS;
        private System.Windows.Forms.GroupBox gbxSmallArea;
        private System.Windows.Forms.NumericUpDown nudTophatDarkTh;
        private System.Windows.Forms.TextBox txbTophatMinArea;
        private System.Windows.Forms.Label labTophatDarkTh;
        private System.Windows.Forms.NumericUpDown nudTophatBrightTh;
        private System.Windows.Forms.Label labTophatum2;
        private System.Windows.Forms.Label labTophatBrightTh;
        private System.Windows.Forms.Label labTophatMinArea;
        private System.Windows.Forms.GroupBox gbxLargeArea;
        private System.Windows.Forms.CheckBox cbxMaskEnabled;
        private System.Windows.Forms.Label labImageFileName;
        private System.Windows.Forms.CheckBox cbxTophatHEnabled;
        private System.Windows.Forms.TextBox txbtophatH;
        private System.Windows.Forms.TextBox txbTophatW;
        private System.Windows.Forms.CheckBox cbxTophatWEnabled;
        private System.Windows.Forms.Label labThinTopHatum2;
        private System.Windows.Forms.CheckBox cbxTophatAreaEnabled;
        private System.Windows.Forms.Label labThinTopHatum1;
        private System.Windows.Forms.Label labTopHatH;
        private System.Windows.Forms.Label labTopHatW;
        private System.Windows.Forms.CheckBox cbxThinHEnabled;
        private System.Windows.Forms.TextBox txbThinInspH;
        private System.Windows.Forms.TextBox txbThinInspW;
        private System.Windows.Forms.CheckBox cbxThinWEnabled;
        private System.Windows.Forms.Label labThinum2;
        private System.Windows.Forms.CheckBox cbxThinAreaEnabled;
        private System.Windows.Forms.Label labThinum1;
        private System.Windows.Forms.Label labThinInspH;
        private System.Windows.Forms.Label labThinInspW;
        private System.Windows.Forms.CheckBox cbxInnerHeightEnabled;
        private System.Windows.Forms.CheckBox cbxInnerWidthEnabled;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbxInnerAreaEnabled;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox cbxOuterHEnabled;
        private System.Windows.Forms.CheckBox cbxOuterWEnabled;
        private System.Windows.Forms.CheckBox cbxOuterAreaEnabled;
        private System.Windows.Forms.CheckBox cbxScratchHeightEnabled;
        private System.Windows.Forms.TextBox txbScratchHeightMin;
        private System.Windows.Forms.TextBox txbScratchWidthMin;
        private System.Windows.Forms.CheckBox cbxScratchWidthEnabled;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox cbxScratchAreaEnabled;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox cbxStainHeightEnabled;
        private System.Windows.Forms.TextBox txbStainHeightMin;
        private System.Windows.Forms.TextBox txbStainWidthMin;
        private System.Windows.Forms.CheckBox cbxStainWidthEnabled;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox cbxStainAreaEnabled;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.CheckBox cbxRDefectHeightEnabled;
        private System.Windows.Forms.TextBox txbRDefectHeightMin;
        private System.Windows.Forms.TextBox txbRDefectWidthMin;
        private System.Windows.Forms.CheckBox cbxRDefectWidthEnabled;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.CheckBox cbxRDefectAreaEnabled;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Button btnMaskTH;
        private System.Windows.Forms.GroupBox gbxMethod;
        private System.Windows.Forms.CheckedListBox cbxListMethod;
        private System.Windows.Forms.Button btnSettingMethod;
        private System.Windows.Forms.TextBox txbSumArea;
        private System.Windows.Forms.CheckBox cbxSumAreaEnabled;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label labSumArea;
        private System.Windows.Forms.GroupBox gbxSumArea;
        private System.Windows.Forms.Label labVersionNumber;
        private System.Windows.Forms.Label labInspVersion;
        private System.Windows.Forms.TextBox txbGrayValue;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.NumericUpDown nudMatchCloseHeight;
        private System.Windows.Forms.NumericUpDown nudMatchCloseWidth;
        private System.Windows.Forms.NumericUpDown nudMatchOpenHeight;
        private System.Windows.Forms.NumericUpDown nudMatchOpenWidth;
        private System.Windows.Forms.CheckBox cbxRotateImage;
        private System.Windows.Forms.ComboBox comboBoxInnerBand;
        private System.Windows.Forms.Label labInnerBand;
        private System.Windows.Forms.ComboBox comboBoxOuterBand;
        private System.Windows.Forms.Label labOuterBand;
        private System.Windows.Forms.Button btnTestEro;
        private System.Windows.Forms.Button btnDilTest;
        private System.Windows.Forms.CheckBox cbxTestROI;
        private System.Windows.Forms.NumericUpDown nudThinEdgeSkipH;
        private System.Windows.Forms.NumericUpDown nudThinEdgeSkipW;
        private System.Windows.Forms.Label labThinByPass;
        private System.Windows.Forms.Button btnThinRegionTest;
        private System.Windows.Forms.Button btnTopHatEdgeExpTest;
        private System.Windows.Forms.NumericUpDown nudTopHatEdgeExpHeight;
        private System.Windows.Forms.NumericUpDown nudTopHatEdgeExpWidth;
        private System.Windows.Forms.Label labTopHatEdgeExp;
        private System.Windows.Forms.NumericUpDown nudSESizeHeight;
        private System.Windows.Forms.NumericUpDown nudSESizeWidth;
        private System.Windows.Forms.Label labSESize;
        private System.Windows.Forms.GroupBox gbxAvgTest;
        private System.Windows.Forms.Button btnTestAverage;
        private System.Windows.Forms.Label labMin;
        private System.Windows.Forms.Label labMid;
        private System.Windows.Forms.Label labMax;
        private System.Windows.Forms.NumericUpDown nudContrastHigh;
        private System.Windows.Forms.Label labMInObj;
        private System.Windows.Forms.NumericUpDown nudMinObjSize;
        private System.Windows.Forms.NumericUpDown nudInnerEdgeSkipHeight;
        private System.Windows.Forms.NumericUpDown nudOuterEdgeSkipHeight;
        private System.Windows.Forms.CheckBox cbxHistoEq;
        private System.Windows.Forms.CheckBox cbxTopHatEnabled;
        private System.Windows.Forms.CheckBox cbxMeanEnabled;
        private System.Windows.Forms.GroupBox gbxHistoEq;
        private System.Windows.Forms.CheckBox cbxHistoEqEnabled;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.CheckBox cbxHistoHeightEnabled;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox txbHistoHeightMin;
        private System.Windows.Forms.NumericUpDown nudHistoEqTh;
        private System.Windows.Forms.TextBox txbHistoWidthMin;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.CheckBox cbxHistoWidthEnabled;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.TextBox txbHistoAreaMin;
        private System.Windows.Forms.CheckBox cbxHistoAreaEnabled;
        private System.Windows.Forms.Button btnSaveImg;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.NumericUpDown nudHistoOpenWidth;
        private System.Windows.Forms.NumericUpDown nudHistoCloseHeight;
        private System.Windows.Forms.NumericUpDown nudHistoCloseWidth;
        private System.Windows.Forms.NumericUpDown nudHistoOpenHeight;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.NumericUpDown nudRDefectExtHeight;
        private System.Windows.Forms.NumericUpDown nudRDefectExtWidth;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.NumericUpDown nudMeanOpenRad;
        private System.Windows.Forms.NumericUpDown nudMeanCloseRad;
        private System.Windows.Forms.Button nudHistoEdgeSkipTest;
        private System.Windows.Forms.NumericUpDown nudHistoEdgeSkipH;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.NumericUpDown nudHistoEdgeSkipW;
        private System.Windows.Forms.NumericUpDown nudExtHTh;
        private System.Windows.Forms.NumericUpDown nudExtH;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cbxThinHatEnabled;
        private System.Windows.Forms.CheckBox cbxThinHatHeightEnabled;
        private System.Windows.Forms.TextBox txbThinHatHeightMin;
        private System.Windows.Forms.NumericUpDown nudThinHatDarkTh;
        private System.Windows.Forms.Button btnThinHatRegionTest;
        private System.Windows.Forms.TextBox txbThinHatWidthMin;
        private System.Windows.Forms.CheckBox cbxThinHatWidthEnabled;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.TextBox txbThinHatAreaMin;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.CheckBox cbxThinHatAreaEnabled;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.NumericUpDown nudThinHatBrightTh;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.NumericUpDown nudThinHatOpenWidth;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.NumericUpDown nudThinHatCloseHeight;
        private System.Windows.Forms.NumericUpDown nudThinHatSkipH;
        private System.Windows.Forms.NumericUpDown nudThinHatCloseWidth;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.NumericUpDown nudThinHatSkipW;
        private System.Windows.Forms.NumericUpDown nudThinHatOpenHeight;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.GroupBox gbxScratchOuter;
        private System.Windows.Forms.CheckBox cbxScratchOuterHeightEnabled;
        private System.Windows.Forms.TextBox txbScratchOuterHeightMin;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.TextBox txbScratchOuterWidthMin;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.Label label52;
        private System.Windows.Forms.CheckBox cbxScratchOuterWidthEnabled;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.CheckBox cbxScratchOuterAreaEnabled;
        private System.Windows.Forms.Label label53;
        private System.Windows.Forms.TextBox txbScratchOuterAreaMin;
        private System.Windows.Forms.Button btnScratchRegion;
        private System.Windows.Forms.Button btnSettingHistoEqTh;
        private System.Windows.Forms.Button btnBrightRegionTh;
        private System.Windows.Forms.CheckBox cbxNCCMode;
        private System.Windows.Forms.Label labGrayMean;
        private System.Windows.Forms.Label labGray;
        private System.Windows.Forms.CheckBox cbxLogEnabled;
        private System.Windows.Forms.GroupBox gbxThinScratch;
        private System.Windows.Forms.CheckBox cbxThinScratchEnabled;
        private System.Windows.Forms.Label label61;
        private System.Windows.Forms.CheckBox cbxThinScratchHeightEnabled;
        private System.Windows.Forms.TextBox txbThinScratchHeight;
        private System.Windows.Forms.TextBox txbThinScratchWidth;
        private System.Windows.Forms.CheckBox cbxThinScratchWidthEnabled;
        private System.Windows.Forms.NumericUpDown nudThinScratchCloseW;
        private System.Windows.Forms.Label label64;
        private System.Windows.Forms.TextBox txbThinScratchArea;
        private System.Windows.Forms.Label label56;
        private System.Windows.Forms.CheckBox cbxThinScratchAreaEnabled;
        private System.Windows.Forms.NumericUpDown nudThinScratchOpenW;
        private System.Windows.Forms.Label label63;
        private System.Windows.Forms.Label label57;
        private System.Windows.Forms.Label label58;
        private System.Windows.Forms.Label label60;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.NumericUpDown nudThinScratchCloseH;
        private System.Windows.Forms.NumericUpDown nudThinScratchOpenH;
        private System.Windows.Forms.Button btnCreateNewMode;
        private System.Windows.Forms.Button btnAddImage;
        private System.Windows.Forms.Button btnTextTest;
        private System.Windows.Forms.Button btnTrain;
        private System.Windows.Forms.ComboBox comboBoxSelectRegion;
        private System.Windows.Forms.Button btnSelectObj;
        private System.Windows.Forms.CheckBox cbxTrainROI;
        private System.Windows.Forms.Button btnSaveMode;
        private System.Windows.Forms.ComboBox comboxModeSelect;
        private System.Windows.Forms.NumericUpDown nudSensitivity;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.NumericUpDown nudThinScratchEdgeSkipH;
        private System.Windows.Forms.Label label54;
        private System.Windows.Forms.NumericUpDown nudThinScratchEdgeSkipW;
        private System.Windows.Forms.TextBox txbRShiftName;
        private System.Windows.Forms.CheckBox cbxRShiftEnabled;
        private System.Windows.Forms.GroupBox gbxRShift;
        private System.Windows.Forms.Button btnRShiftInsp;
        private System.Windows.Forms.NumericUpDown nudRShiftTh;
        private System.Windows.Forms.Label labRSiftTh;
        private System.Windows.Forms.Label label65;
        private System.Windows.Forms.NumericUpDown nudScaleSize;
        private System.Windows.Forms.Label label66;
        private System.Windows.Forms.NumericUpDown nudAutoTthSigma;
        private System.Windows.Forms.Label label62;
        private System.Windows.Forms.NumericUpDown nudTargetEArea;
        private System.Windows.Forms.Label label67;
        private System.Windows.Forms.NumericUpDown nudTargetRWidth;
        private System.Windows.Forms.Label label68;
        private System.Windows.Forms.Button btnTargetETest;
        private System.Windows.Forms.Button btnTargetRTest;
        private System.Windows.Forms.Label label69;
        private System.Windows.Forms.NumericUpDown nudRDefectSkipHeight;
        private System.Windows.Forms.NumericUpDown nudRDefectSkipWidth;
        private System.Windows.Forms.Button btnAutoThTest;
        private System.Windows.Forms.NumericUpDown nudRShiftClosingSize;
	    private System.Windows.Forms.NumericUpDown nudThinScratchEdgeExHeight;
        private System.Windows.Forms.Label label71;
        private System.Windows.Forms.NumericUpDown nudThinScratchEdgeExWidth;
        private System.Windows.Forms.TextBox txbHoleName;
        private System.Windows.Forms.Button btnHoleInsp;
        private System.Windows.Forms.TabPage tpRgionMethod;
        private System.Windows.Forms.GroupBox gbxMtehodThreshold;
        private System.Windows.Forms.Label label73;
        private System.Windows.Forms.NumericUpDown nudMethodThMin;
        private System.Windows.Forms.Label label72;
        private System.Windows.Forms.ComboBox combxMethodThBand;
        private System.Windows.Forms.Label label70;
        private System.Windows.Forms.NumericUpDown nudMethodThMax;
        private System.Windows.Forms.Button btnMethodThSet;
        private System.Windows.Forms.Button btnMethodThAdd;
        private System.Windows.Forms.Button btnMethodThTest;
        private System.Windows.Forms.GroupBox gbxMethodList;
        private System.Windows.Forms.ListView listViewMethod;
        private System.Windows.Forms.ColumnHeader columnHeaderMethodRegion;
        private System.Windows.Forms.Label label74;
        private System.Windows.Forms.ComboBox combxCustomizeType;
        private System.Windows.Forms.CheckBox cbxCustomizeAdd;
        private System.Windows.Forms.Button btnRemoveMethodRegion;
        private System.Windows.Forms.Button btnOpenEditRegionForm;
        private System.Windows.Forms.TabControl tbThMethod;
        private System.Windows.Forms.TabPage tpSingleTh;
        private System.Windows.Forms.TabPage tpDualTh;
        private System.Windows.Forms.Label label75;
        private System.Windows.Forms.Button btnDualThAdd;
        private System.Windows.Forms.NumericUpDown nudDualThMinSize;
        private System.Windows.Forms.NumericUpDown nudDualThMinGray;
        private System.Windows.Forms.Button btnDualThTest;
        private System.Windows.Forms.Label label77;
        private System.Windows.Forms.Label label76;
        private System.Windows.Forms.NumericUpDown nudDualThThreshold;
        private System.Windows.Forms.GroupBox gbxEditRegionList;
        private System.Windows.Forms.Button btnEditRegionRemove;
        private System.Windows.Forms.ListView listViewEditRegion;
        private System.Windows.Forms.ColumnHeader columnHeaderEditRegion;
        private System.Windows.Forms.ComboBox comboBoxDisplayType;
        private System.Windows.Forms.TabPage tpAutoTh;
        private System.Windows.Forms.Button btnAutoThAdd;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox comboBoxAutoThSelect;
        private System.Windows.Forms.Label label79;
        private System.Windows.Forms.NumericUpDown nudAutoThSigma;
        private System.Windows.Forms.Label label78;
        private System.Windows.Forms.Button btnLoadGolden;
        private System.Windows.Forms.Button btnExportRegion;
        private System.Windows.Forms.Button btnEditRegionRemoveAll;
        private System.Windows.Forms.Button btnClearMethodRegion;
        private System.Windows.Forms.TabPage labAnisometry;
        private System.Windows.Forms.Button btnAddAlgorithm;
        private System.Windows.Forms.GroupBox gbxAlgorithmList;
        private System.Windows.Forms.Button btnAlgorithmClear;
        private System.Windows.Forms.Button btnAlgorithmSetup;
        private System.Windows.Forms.Button btnAlgorithmRemove;
        private System.Windows.Forms.ListView listViewAlgorithm;
        private System.Windows.Forms.ColumnHeader columnHeaderAlgorithm;
        private System.Windows.Forms.Label labAlgorithmSelect;
        private System.Windows.Forms.ComboBox comboBoxAlgorithmSelect;
        private System.Windows.Forms.Button btnMethTest;
        private System.Windows.Forms.TextBox txbAlgorithmName;
        private System.Windows.Forms.ComboBox comboBoxRegionSelect;
        private System.Windows.Forms.Label labRegionIndex;
        private System.Windows.Forms.Label labInspImgIndex;
        private System.Windows.Forms.NumericUpDown nudInspImageIndex;
        private System.Windows.Forms.Label labMethThBand;
        private System.Windows.Forms.Label labMethodThImageID;
        private System.Windows.Forms.Button btnAddImsge;
        private System.Windows.Forms.TabPage tpDynTh;
        private System.Windows.Forms.Button btnDynThAdd;
        private System.Windows.Forms.Label labOffset;
        private System.Windows.Forms.Label labDynMeanImage;
        private System.Windows.Forms.NumericUpDown nudMeanHeight;
        private System.Windows.Forms.NumericUpDown nudDynOffset;
        private System.Windows.Forms.NumericUpDown nudMeanWidth;
        private System.Windows.Forms.Button btnDynTest;
        private System.Windows.Forms.ComboBox comboBoxDynType;
        private System.Windows.Forms.GroupBox gbxHoleGrayFeature;
        private System.Windows.Forms.Label labDarkRatioValue;
        private System.Windows.Forms.Label labGrayMinValue;
        private System.Windows.Forms.Label labMeanValue;
        private System.Windows.Forms.Label labRadiusValue;
        private System.Windows.Forms.Label labRatio;
        private System.Windows.Forms.Label labGrayMin;
        private System.Windows.Forms.Label labMeanGray;
        private System.Windows.Forms.Label labRadius;
        private System.Windows.Forms.GroupBox gbxRegionFeature;
        private System.Windows.Forms.Label label81;
        private System.Windows.Forms.Label label80;
        private System.Windows.Forms.Label labInfoGrayMax;
        private System.Windows.Forms.Label labInfoGrayMin;
        private System.Windows.Forms.NumericUpDown nudMatchNumber;
        private System.Windows.Forms.Label label82;
        private System.Windows.Forms.GroupBox gbxSurfaceFeature;
        private System.Windows.Forms.Label labroundness;
        private System.Windows.Forms.Label labcircularity;
        private System.Windows.Forms.Label labholes_num;
        private System.Windows.Forms.Label labrectangularity;
        private System.Windows.Forms.Label labconvexity;
        private System.Windows.Forms.Label label91;
        private System.Windows.Forms.Label label89;
        private System.Windows.Forms.Label label87;
        private System.Windows.Forms.Label label85;
        private System.Windows.Forms.Label label83;
        private System.Windows.Forms.ColumnHeader columnHeaderPriority;
        private System.Windows.Forms.GroupBox gbxUsedRegion;
        private System.Windows.Forms.Button btnClearUsedRegion;
        private System.Windows.Forms.Button btnRemoveUseRegion;
        private System.Windows.Forms.ListView listViewUsedRegion;
        private System.Windows.Forms.ColumnHeader columnHeaderUseRegion;
        private System.Windows.Forms.ColumnHeader columnHeaderUseDisplay;
        private System.Windows.Forms.ColumnHeader columnHeaderUseImageId;
        private System.Windows.Forms.Button btnAddUsed;
        private System.Windows.Forms.Button btnUpdateDefectName;
        private System.Windows.Forms.NumericUpDown nudPriority;
        private System.Windows.Forms.Label label84;
        private System.Windows.Forms.GroupBox gbxSynchronizePath;
        private System.Windows.Forms.Button btnSynchronize;
        private System.Windows.Forms.TextBox txbSynchronizePath;
        private System.Windows.Forms.Label label86;
        private System.Windows.Forms.CheckBox cbxAutoUpdate;
        private System.Windows.Forms.TabPage tpUserSet;
        private System.Windows.Forms.Panel panelImageControl;
        private System.Windows.Forms.Panel panelInspectionControl;
        private System.Windows.Forms.Label label88;
        private System.Windows.Forms.NumericUpDown nudDAVSImageId;
        private System.Windows.Forms.Label label90;
        private System.Windows.Forms.NumericUpDown nudClosingNum;
        private System.Windows.Forms.CheckBox cbxFillup;
        private System.Windows.Forms.Label label92;
        private System.Windows.Forms.ComboBox comboBoxFillupType;
        private System.Windows.Forms.NumericUpDown nudFillupMax;
        private System.Windows.Forms.NumericUpDown nudFillupMin;
        private System.Windows.Forms.ColumnHeader columnHeaderColor;
        private System.Windows.Forms.Button btnImportParam;
        private System.Windows.Forms.ColumnHeader columnHeaderUseColor;
        private System.Windows.Forms.Label labinner_radius;
        private System.Windows.Forms.Label label93;
        private System.Windows.Forms.Label labSecGrayMax;
        private System.Windows.Forms.Label labSecGrayMin;
        private System.Windows.Forms.Label labSecGray;
        private System.Windows.Forms.CheckBox cbxOutBondary;
        private System.Windows.Forms.NumericUpDown nudDAVSBand1ImageIndex;
        private System.Windows.Forms.CheckBox cbxDAVSMixBandEnabled;
        private System.Windows.Forms.ComboBox comboBoxDAVSBand3;
        private System.Windows.Forms.ComboBox comboBoxDAVSBand2;
        private System.Windows.Forms.ComboBox comboBoxDAVSBand1;
        private System.Windows.Forms.NumericUpDown nudDAVSBand3ImageIndex;
        private System.Windows.Forms.NumericUpDown nudDAVSBand2ImageIndex;
        private System.Windows.Forms.Label labImageIDDisplay;
        private System.Windows.Forms.Label labID1;
        private System.Windows.Forms.Label labID0;
        private System.Windows.Forms.Label label94;
        private System.Windows.Forms.Label labanisometryL;
        private System.Windows.Forms.Label labMatchCount;
        private System.Windows.Forms.Label labDefectCount;
        private System.Windows.Forms.GroupBox gbxAdjust;
        private System.Windows.Forms.Button btnAdjSet;
        private System.Windows.Forms.Label labAdjY;
        private System.Windows.Forms.Label labAdjX;
        private System.Windows.Forms.TextBox txbAdjY;
        private System.Windows.Forms.TextBox txbAdjX;
        private System.Windows.Forms.Button btnLeft;
        private System.Windows.Forms.Button btnRight;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnBatchTest;
        private System.Windows.Forms.TabPage tpAlgoImage;
        private System.Windows.Forms.Button btn_Compute_UsedRegion;
        private System.Windows.Forms.GroupBox gbx_AlgoImage_UsedRegion;
        private System.Windows.Forms.Button btn_Execute_Algo_UsedRegion;
        private System.Windows.Forms.Button btn_RemoveAll_Algo_UsedRegion;
        private System.Windows.Forms.Button btn_Remove_Algo_UsedRegion;
        private System.Windows.Forms.Button btn_Edit_Algo_UsedRegion;
        private System.Windows.Forms.Button btn_Add_Algo_UsedRegion;
        private System.Windows.Forms.ListView listView_EditAlgo_UsedRegion;
        private System.Windows.Forms.ColumnHeader columnHeader_ID;
        private System.Windows.Forms.ColumnHeader columnHeader_Name;
        private System.Windows.Forms.ColumnHeader columnHeader_Algo;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.ComboBox cbx_ResultImgID_B;
        private System.Windows.Forms.Label label95;
        private System.Windows.Forms.GroupBox gbx_AlgoImage;
        private System.Windows.Forms.ComboBox cbx_ResultImgID_A;
        private System.Windows.Forms.Label label96;
        private System.Windows.Forms.Button btn_Execute_Algo;
        private System.Windows.Forms.Button btn_RemoveAll_Algo;
        private System.Windows.Forms.Button btn_Remove_Algo;
        private System.Windows.Forms.Button btn_Edit_Algo;
        private System.Windows.Forms.Button btn_Add_Algo;
        private System.Windows.Forms.ListView listView_EditAlgo;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.RadioButton radioButton_OrigImg_RegMethod;
        private System.Windows.Forms.RadioButton radioButton_ImgA_RegMethod;
        private System.Windows.Forms.RadioButton radioButton_ImgB_AOI;
        private System.Windows.Forms.RadioButton radioButton_OrigImg_AOI;
        private System.Windows.Forms.RadioButton radioButton_ImgA_AOI;
        private System.Windows.Forms.NumericUpDown nudMethodThImageID;
        private System.Windows.Forms.NumericUpDown nudInspectImgID;
        private System.Windows.Forms.RadioButton radioButton_OrigImg_Match;
        private System.Windows.Forms.RadioButton radioButton_ImgA_Match;
        private System.Windows.Forms.GroupBox gbx_PatternMatch_PreProcess;
        private System.Windows.Forms.Button btnTest_PatternMatch_PreProcess;
        private System.Windows.Forms.Button button_SaveAsResultImgA;
        private System.Windows.Forms.Button button_SaveAsResultImgB;
        private System.Windows.Forms.GroupBox gbx_BatchTest;
        private System.Windows.Forms.ListView listView_BatchTest_Info;
        private System.Windows.Forms.ColumnHeader columnHeader_Index;
        private System.Windows.Forms.ColumnHeader columnHeader_FileName;
        private System.Windows.Forms.ColumnHeader columnHeader_OK;
        private System.Windows.Forms.Button btn_BatchTest_LoadImg;
        private System.Windows.Forms.TextBox txb_BatchTest_Path;
        private System.Windows.Forms.Label label97;
        private System.Windows.Forms.ListView listView_BatchTest_Result;
        private System.Windows.Forms.ColumnHeader columnHeader_result;
        private System.Windows.Forms.GroupBox gbx_BatchTest_Info;
        private System.Windows.Forms.GroupBox gbx_BatchTest_Result;
        private System.Windows.Forms.ColumnHeader columnHeader_count;
        private System.Windows.Forms.TextBox txb_BatchTest_Count;
        private System.Windows.Forms.Label label98;
        private System.Windows.Forms.ColumnHeader columnHeader_NG;
        private System.Windows.Forms.TextBox txb_BatchTest_ID;
        private System.Windows.Forms.Label label99;
        private System.Windows.Forms.ComboBox cbx_ResultRegID_A;
        private System.Windows.Forms.Label label101;
        private System.Windows.Forms.ComboBox cbx_ResultRegID_B;
        private System.Windows.Forms.Label label100;
    }
}
