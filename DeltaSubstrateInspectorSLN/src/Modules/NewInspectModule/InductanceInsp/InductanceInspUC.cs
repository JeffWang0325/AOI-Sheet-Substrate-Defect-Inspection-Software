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
using static DeltaSubstrateInspector.src.Modules.ResultModule.ImageShowForm;
using System.Collections;
using System.Threading;
using System.Collections.Concurrent;
using DeltaSubstrateInspector.src.SystemSource.WorkSheet;
using System.Xml.Serialization;

using KellermanSoftware.CompareNetObjects;
using DeltaSubstrateInspector.src.Modules.Algorithm; // (20200119) Jeff Revised!
using System.Media; // For SystemSounds

namespace DeltaSubstrateInspector.src.Modules.NewInspectModule.InductanceInsp
{
    public partial class InductanceInspUC : UserControl
    {
        public enum enuMode
        {
            Dark,
            Bright
        }

        public enum enuDisplayRegionType
        {
            fill,
            margin
        }

        #region //=======================  變數設置 =======================

        public event EventHandler OnUserControlButtonClicked;

        //private HObject Display_Image = null;
        private HDrawingObject drawing_Rect = new HDrawingObject(100, 100, 210, 210);

        private InductanceInspRole Recipe = InductanceInspRole.GetSingleton();
        private InductanceInsp Method = InductanceInsp.GetSingleton(true); // (20191214) Jeff Revised!

        string Path = ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\InductanceInsp\\";

        private HObject PatternRegions = null;

        /// <summary>
        /// 原始影像集合
        /// </summary>
        List<HObject> Input_ImgList = new List<HObject>();

        HTuple hv_Index = new HTuple();
        HObject SelectRegions = null;
        TabControl HidePage = new TabControl();
        HObject TrainRegion = new HObject();
        HObject SaveImage = null;
        private static enuMode Mode = enuMode.Dark;
        enuDisplayRegionType DisplayType = enuDisplayRegionType.fill;

        HObject AutoRegion;
        HTuple LineWidth = 1;

        private clsRecipe TmpParam { get; set; }

        #endregion

        #region 利用BackgroundWorker做需耗時之工作

        private BackgroundWorker bw_BatchTest; // 批量測試 (20200319) Jeff Revised!

        private void initBackgroundWorker() // (20200319) Jeff Revised!
        {
            bw_BatchTest = new BackgroundWorker();
            bw_BatchTest.WorkerReportsProgress = false;
            bw_BatchTest.WorkerSupportsCancellation = true;
            bw_BatchTest.DoWork += new DoWorkEventHandler(bw_DoWork_BatchTest);
            bw_BatchTest.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted_BatchTest);
        }

        #endregion

        #region //=======================  Function =======================

        public InductanceInspUC() // (20200130) Jeff Revised!
        {
            InitializeComponent();
            DisableMouseWeel(this);
            this.DisplayWindows.MouseWheel += DisplayWindows.HSmartWindowControl_MouseWheel;
            HOperatorSet.GenEmptyRegion(out SelectRegions);
            UpdateParameter();
            UpdateUIDisplay();
            //nudDAVSBand1ImageIndex.Maximum = Recipe.Param.ImgCount - 1;
            //nudDAVSBand2ImageIndex.Maximum = Recipe.Param.ImgCount - 1;
            //nudDAVSBand3ImageIndex.Maximum = Recipe.Param.ImgCount - 1;
            //nudMethodThImageID.Maximum = Recipe.Param.ImgCount - 1; // (20200130) Jeff Revised!
            //nudInspImageIndex.Maximum = Recipe.Param.ImgCount - 1; // (20200130) Jeff Revised!
            
            clsLanguage.clsLanguage.SetLanguateToControls(this);

            initBackgroundWorker(); // (20200319) Jeff Revised!
        }

        private void Num_DiscountAmount_MouseWheel(object sender, MouseEventArgs e)
        {
            HandledMouseEventArgs h = e as HandledMouseEventArgs;
            if (h != null)
            {
                h.Handled = true;
            }
        }
        private void DisableMouseWeel(Control Parent)
        {
            List<Control> AllControls = GetAllControls(this);
            foreach (Control control in AllControls)
            {
                if (control.Name.Contains("nud") || control.Name.Contains("comb") || control.Name.Contains("cbx"))
                    control.MouseWheel += new MouseEventHandler(Num_DiscountAmount_MouseWheel);
            }

        }
        public static List<Control> GetAllControls(UserControl form)
        {
            return GetAllControls(ToList(form.Controls));
        }
        
        public static List<Control> ToList(Control.ControlCollection controls)
        {
            List<Control> controlList = new List<Control>();
            foreach (Control control in controls)
                controlList.Add(control);
            return controlList;
        }
        public static List<Control> GetAllControls(List<Control> inputList)
        {
            //複製inputList到outputList
            List<Control> outputList = new List<Control>(inputList);

            //取出inputList中的容器
            IEnumerable containers = from control in inputList
                                     where
                    control is GroupBox |
                    control is TabControl |
                    control is Panel |
                    control is FlowLayoutPanel |
                    control is TableLayoutPanel |
                    control is ContainerControl
                                     select control;

            foreach (Control container in containers)
            {
                //遞迴加入容器內的容器與控制項
                outputList.AddRange(GetAllControls(ToList(container.Controls)));
            }
            return outputList;
        }

        /// <summary>
        /// 畫瑕疵Region於影像上，供【儲存結果】
        /// </summary>
        /// <param name="DefectRegion"></param>
        /// <param name="DefectMap"></param>
        /// <param name="ImageIndex"></param>
        /// <param name="ImageSource"></param>
        public void DrawImg(HObject DefectRegion, HObject DefectMap = null, int ImageIndex = 0, enu_ImageSource ImageSource = enu_ImageSource.原始) // (20200409) Jeff Revised!
        {
            try
            {
                HObject Img;
                if (DefectRegion == null) // (20200409) Jeff Revised!
                    return;

                HObject SrcImg = Get_SourceImage(ImageSource, ImageIndex); // (20200130) Jeff Revised!

                if (SaveImage == null)
                    Img = SrcImg.Clone();
                else
                {
                    SaveImage.Dispose();
                    SaveImage = null;
                    Img = SrcImg.Clone();
                }
                HTuple Channel;
                HOperatorSet.CountChannels(Img, out Channel);
                if (Channel == 1)
                    HOperatorSet.Compose3(Img.Clone(), Img.Clone(), Img.Clone(), out SaveImage);
                else
                    SaveImage = Img.Clone();
                HOperatorSet.OverpaintRegion(SaveImage, DefectRegion, ((new HTuple(255)).TupleConcat(0)).TupleConcat(0), DisplayType.ToString());
                if (DefectMap != null) // (20200409) Jeff Revised!
                    HOperatorSet.OverpaintRegion(SaveImage, DefectMap, ((new HTuple(0)).TupleConcat(255)).TupleConcat(0), "margin");
                Img.Dispose();
            }
            catch (Exception E)
            {
                MessageBox.Show(E.ToString());
            }
        }

        public void UpdateUIDisplay()
        {
            foreach (TabPage P in tbInspParamSetting.TabPages)
            {
                if (P.Text == "圖像教導" || P.Text == "影像演算法" || P.Text == "檢測範圍設定" || P.Text == "演算法參數設定") // (20200119) Jeff Revised!
                    continue;
                HidePage.TabPages.Add(P);
            }
            //tbInspParamSetting.TabPages.Add(HidePage.TabPages["tpPatternMatch"]);
            //tbInspParamSetting.TabPages.Add(HidePage.TabPages["tpRgionMethod"]);
            //tbInspParamSetting.TabPages.Add(HidePage.TabPages["tpAlgorithm"]);
            
            for (int i = 0; i < Recipe.Param.MethodList.Count; i++)
            {
                if (Recipe.Param.MethodList[i].Name == "AI")
                {
                    if (Recipe.Param.MethodList[i].Enabled)
                    {
                        btnDAVSSetting.Visible = true;
                        gbxAIType.Visible = true;
                    }
                    else
                    {
                        btnDAVSSetting.Visible = false;
                        gbxAIType.Visible = false;
                        Recipe.DAVSParam.DAVS_Mode = 0;
                    }
                    continue;
                }
                //if (Recipe.Param.MethodList[i].Enabled)
                //{
                //    tbInspParamSetting.TabPages.Add(HidePage.TabPages[Recipe.Param.MethodList[i].Name]);
                //}
            }

            #region 例外
            //if (Recipe.Param.MethodList.Count > 0)
            //{
            //    if (!Recipe.Param.MethodList[2].Enabled && !Recipe.Param.MethodList[3].Enabled && !Recipe.Param.MethodList[4].Enabled && !Recipe.Param.MethodList[5].Enabled)
            //    {
            //        gbxInspRegionsSetting.Visible = false;
            //    }
            //    else
            //    {
            //        gbxInspRegionsSetting.Visible = true;
            //    }
            //}
            #endregion
        }

        public void UpdateDisplayList()
        {
            combxDisplayImg.Items.Clear();
            for (int i = 0; i < Input_ImgList.Count; i++)
            {
                combxDisplayImg.Items.Add(i.ToString());
            }
            if (Input_ImgList.Count > 0)
                combxDisplayImg.SelectedIndex = 0;
        }

        /// <summary>
        /// 點擊小圖>>【參數調整】<<設定輸入影像
        /// </summary>
        public void UpdateNowInput_ImgList() // (20200310) Jeff Revised!
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
                HOperatorSet.DispObj(Input_ImgList[0], DisplayWindows.HalconWindow);
                HOperatorSet.SetPart(DisplayWindows.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part
            }

            // (20200310) Jeff Revised!
            if (Input_ImgList != null && Input_ImgList.Count > 0)
            {
                btn_Execute_Algo_Click(null, null); //【執行】
                btn_Execute_Algo_UsedRegion_Click(null, null); //【執行】 (使用範圍列表)
            }
        }

        public InspectRole get_role()
        {
            return create_role();
        }

        private InductanceInspRole create_role()
        {
            InductanceInspRole role = new InductanceInspRole();
            role.Method = "InductanceInsp";
            return role;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.E))
            {
                //gbxMatchingParam.Visible = !gbxMatchingParam.Visible;
                gbxMethod.Visible = !gbxMethod.Visible;
                gbxAdjust.Visible = !gbxAdjust.Visible;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// 設定CheckBox控制項之BackColor
        /// </summary>
        /// <param name="C"></param>
        /// <param name="cbx"></param>
        public static void ChangeColor(Color C, CheckBox cbx)
        {
            cbx.BackColor = C;
        }

        public static void GetRectData(HTuple Rect, out HTuple column1, out HTuple row1, out HTuple column2, out HTuple row2)
        {
            HOperatorSet.GetDrawingObjectParams(Rect, new HTuple("column1"), out column1);
            HOperatorSet.GetDrawingObjectParams(Rect, new HTuple("row1"), out row1);
            HOperatorSet.GetDrawingObjectParams(Rect, new HTuple("column2"), out column2);
            HOperatorSet.GetDrawingObjectParams(Rect, new HTuple("row2"), out row2);
        }

        public static void GetRect2Data(HTuple Rect, out HTuple column, out HTuple row, out HTuple phi, out HTuple length1, out HTuple length2)
        {
            HOperatorSet.GetDrawingObjectParams(Rect, new HTuple("column"), out column);
            HOperatorSet.GetDrawingObjectParams(Rect, new HTuple("row"), out row);
            HOperatorSet.GetDrawingObjectParams(Rect, new HTuple("phi"), out phi);
            HOperatorSet.GetDrawingObjectParams(Rect, new HTuple("length1"), out length1);
            HOperatorSet.GetDrawingObjectParams(Rect, new HTuple("length2"), out length2);
        }
        
        /// <summary>
        /// 是否正在載入Recipe
        /// </summary>
        private bool b_LoadRecipe { get; set; } = false; // (20200130) Jeff Revised!

        /// <summary>
        /// 更新各控制項參數
        /// </summary>
        public void UpdateParameter() // (20200729) Jeff Revised!
        {
            // Load parameter from XML file
            Recipe.load();

            b_LoadRecipe = true; // (20200130) Jeff Revised!
            
            try
            {
                Recipe.Param.AlgoImg.ImageSource = Input_ImgList;
                Recipe.Param.AlgoImg_UsedRegion.ImageSource = Input_ImgList;
                this.TmpParam = clsStaticTool.Clone<clsRecipe>(Recipe.Param); // (20200130) Jeff Revised!
                //this.TmpParam = Recipe.Param.DeepClone(); // (20200729) Jeff Revised!
            }
            catch (Exception ex)
            { }
            
            #region UI

            cbxListMethod.Items.Clear();
            for (int i = 0; i < Recipe.Param.MethodList.Count; i++)
            {
                cbxListMethod.Items.Add(Recipe.Param.MethodList[i].Name);
                cbxListMethod.SetItemChecked(i, Recipe.Param.MethodList[i].Enabled);
            }

            string[] ArrayBand = Enum.GetNames(typeof(enuBand));
            combxBand.Items.Clear();
            combxMethodThBand.Items.Clear();
            comboBoxDAVSBand1.Items.Clear();
            comboBoxDAVSBand2.Items.Clear();
            comboBoxDAVSBand3.Items.Clear();
            foreach (string Band in ArrayBand)
            {
                combxBand.Items.Add(Band);
                combxMethodThBand.Items.Add(Band);
                comboBoxDAVSBand1.Items.Add(Band);
                comboBoxDAVSBand2.Items.Add(Band);
                comboBoxDAVSBand3.Items.Add(Band);
            }

            combxMethodThBand.SelectedIndex = 0;
            comboxModeSelect.SelectedIndex = 0;
            comboBoxDisplayType.SelectedIndex = 0;

            #region 設定 NumericUpDown 的最大值 // (20200130) Jeff Revised!

            nudDAVSBand1ImageIndex.Maximum = Recipe.Param.ImgCount - 1;
            nudDAVSBand2ImageIndex.Maximum = Recipe.Param.ImgCount - 1;
            nudDAVSBand3ImageIndex.Maximum = Recipe.Param.ImgCount - 1;

            radioButton_OrigImg_Match.Checked = true; // (20200130) Jeff Revised!
            nudInspectImgID.Maximum = Recipe.Param.ImgCount - 1; // (20200130) Jeff Revised!
            radioButton_OrigImg_RegMethod.Checked = true; // (20200130) Jeff Revised!
            nudMethodThImageID.Maximum = Recipe.Param.ImgCount - 1; // (20200130) Jeff Revised!
            radioButton_OrigImg_AOI.Checked = true; // (20200130) Jeff Revised!
            nudInspImageIndex.Maximum = Recipe.Param.ImgCount - 1; // (20200130) Jeff Revised!

            #endregion

            #endregion

            #region 影像演算法

            // 釋放原物件記憶體
            this.AlgoImg.Dispose(false);
            this.AlgoImg_UsedRegion.Dispose(false);

            this.AlgoImg = Recipe.Param.AlgoImg;
            this.AlgoImg.ImageSource = Input_ImgList;
            this.AlgoImg.Update_listView_Edit(listView_EditAlgo); // 更新 listView_EditAlgo
            // 更新【結果影像A(編號)】&【結果區域A(編號)】
            this.cbx_ResultImgID_A.Items.Clear();
            this.cbx_ResultRegID_A.Items.Clear();
            for (int i = 0; i < this.AlgoImg.ListAlgoImage.Count; i++)
            {
                this.cbx_ResultImgID_A.Items.Add(i.ToString());
                this.cbx_ResultRegID_A.Items.Add(i.ToString());
            }

            this.AlgoImg_UsedRegion = Recipe.Param.AlgoImg_UsedRegion;
            this.AlgoImg_UsedRegion.ImageSource = Input_ImgList;
            this.AlgoImg_UsedRegion.Update_listView_Edit(listView_EditAlgo_UsedRegion); // 更新 listView_EditAlgo_UsedRegion
            // 更新【結果影像B(編號)】&【結果區域B(編號)】
            this.cbx_ResultImgID_B.Items.Clear();
            this.cbx_ResultRegID_B.Items.Clear();
            for (int i = 0; i < this.AlgoImg_UsedRegion.ListAlgoImage.Count; i++)
            {
                this.cbx_ResultImgID_B.Items.Add(i.ToString());
                this.cbx_ResultRegID_B.Items.Add(i.ToString());
            }
            
            #endregion

            #region 演算法編輯介面

            string[] ArrayMethod = Enum.GetNames(typeof(InductanceInsp.enuAlgorithm));
            comboBoxAlgorithmSelect.Items.Clear();
            foreach (string S in ArrayMethod)
            {
                comboBoxAlgorithmSelect.Items.Add(S);
            }

            UpdateAlgorithmList(); // 更新演算法列表(listViewAlgorithm)

            #endregion

            #region 原始演算法參數

            #region SegParam

            combxBand.SelectedIndex = Recipe.Param.SegParam.BandIndex;
            nudAMinScore.Value = Convert.ToDecimal(Recipe.Param.SegParam.MinScore.ToString());
            cbxThEnabled.Checked = Recipe.Param.SegParam.ThEnabled;

            // (20200130) Jeff Revised!
            if (Recipe.Param.SegParam.ImageSource_SegImg == enu_ImageSource.原始)
                radioButton_OrigImg_Match.Checked = true;
            else
                radioButton_ImgA_Match.Checked = true;

            if (this.nudInspectImgID.Maximum >= Recipe.Param.SegParam.SegImgIndex) // (20200717) Jeff Revised!
                nudInspectImgID.Value = Recipe.Param.SegParam.SegImgIndex; // (20200130) Jeff Revised!
            cbxNCCMode.CheckedChanged -= cbxNCCMode_CheckedChanged;
            cbxNCCMode.Checked = Recipe.Param.SegParam.IsNCCMode;
            cbxNCCMode.CheckedChanged += cbxNCCMode_CheckedChanged;
            cbxMaskEnabled.Checked = Recipe.Param.SegParam.TeachMask;
            nudMatchOpenWidth.Value = Recipe.Param.SegParam.OpeningWidth;
            nudMatchOpenHeight.Value = Recipe.Param.SegParam.OpeningHeight;
            nudMatchCloseWidth.Value = Recipe.Param.SegParam.ClosingingWidth;
            nudMatchCloseHeight.Value = Recipe.Param.SegParam.ClosingingHeight;
            cbxHistoEq.Checked = Recipe.Param.SegParam.MaskHistoeq;
            txbPartGrayValue.Text = Recipe.Param.SegParam.PartitionGrayValue.ToString();
            cbxMatchThEnabled.Checked = Recipe.Param.SegParam.MatchPreProcessEnabled;

            #endregion

            #region AdvParam

            nudAngleStart.Value = decimal.Parse(Recipe.Param.AdvParam.AngleStart.ToString());
            nudAngleExtent.Value = decimal.Parse(Recipe.Param.AdvParam.AngleExtent.ToString());
            nudAngleStep.Value = decimal.Parse(Recipe.Param.AdvParam.AngleStep.ToString());
            nudNumLevels.Value = decimal.Parse(Recipe.Param.AdvParam.NumLevels.ToString());
            nudMinContrast.Value = decimal.Parse(Recipe.Param.AdvParam.MinContrast.ToString());
            cbxOptimization.SelectedIndex = Recipe.Param.AdvParam.Optimization;
            cbxMetric.SelectedIndex = Recipe.Param.AdvParam.Metric;
            cbxAngleStepAuto.Checked = Recipe.Param.AdvParam.AngleStepAuto;
            cbxContrastAuto.Checked = Recipe.Param.AdvParam.ContrastAuto;
            cbxMinContrastAuto.Checked = Recipe.Param.AdvParam.MinContrastAuto;
            nudGreediness.Value = decimal.Parse(Recipe.Param.AdvParam.Greediness.ToString());
            nudOverlap.Value = decimal.Parse(Recipe.Param.AdvParam.Overlap.ToString());
            cbxSubPixel.SelectedIndex = Recipe.Param.AdvParam.SubPixel;
            nudScaleMin.Value = decimal.Parse(Recipe.Param.AdvParam.ScaleMin.ToString());
            nudScaleMax.Value = decimal.Parse(Recipe.Param.AdvParam.ScaleMax.ToString());
            nudOpeningNum.Value = decimal.Parse(Recipe.Param.AdvParam.OpeningNum.ToString());
            nudContrast.Value = decimal.Parse(Recipe.Param.AdvParam.ContrastSmall.ToString());
            nudContrastHigh.Value = decimal.Parse(Recipe.Param.AdvParam.ContrastLarge.ToString());
            nudMinObjSize.Value = decimal.Parse(Recipe.Param.AdvParam.MinObjSize.ToString());
            nudClosingNum.Value = decimal.Parse(Recipe.Param.AdvParam.ClosingNum.ToString());
            nudFillupMin.Value = decimal.Parse(Recipe.Param.AdvParam.FillupMin.ToString());
            nudFillupMax.Value = decimal.Parse(Recipe.Param.AdvParam.FillupMax.ToString());
            cbxFillup.Checked = Recipe.Param.AdvParam.bFillupEnabled;
            comboBoxFillupType.SelectedIndex = (int)Recipe.Param.AdvParam.FillupType;
            nudScaleSize.Value = decimal.Parse(Recipe.Param.AdvParam.MatchSacleSize.ToString());
            nudMatchNumber.Value = decimal.Parse(Recipe.Param.AdvParam.MatchNumber.ToString());

            #endregion

            #region DAVS Param

            combxAIType.SelectedIndexChanged -= combxAIType_SelectedIndexChanged;
            combxAIType.SelectedIndex = Recipe.Param.DAVSInspType;
            combxAIType.SelectedIndexChanged += combxAIType_SelectedIndexChanged;
            nudDAVSImageId.Value = decimal.Parse(Recipe.Param.DAVSImgID.ToString());
            cbxSaveImgEnable.Checked = Recipe.Param.SaveAOIImgEnabled;
            combxSaveImgType.SelectedIndex = Recipe.Param.SaveAOIImgType;
            combxSaveImgType.Enabled = cbxSaveImgEnable.Checked;
            cbxDAVSMixBandEnabled.Checked = Recipe.Param.bDAVSMixImgBand;
            nudDAVSBand1ImageIndex.Value = decimal.Parse(Recipe.Param.DAVSBand1ImgIndex.ToString());
            nudDAVSBand2ImageIndex.Value = decimal.Parse(Recipe.Param.DAVSBand2ImgIndex.ToString());
            nudDAVSBand3ImageIndex.Value = decimal.Parse(Recipe.Param.DAVSBand3ImgIndex.ToString());
            comboBoxDAVSBand1.SelectedIndex = (int)Recipe.Param.DAVSBand1;
            comboBoxDAVSBand2.SelectedIndex = (int)Recipe.Param.DAVSBand2;
            comboBoxDAVSBand3.SelectedIndex = (int)Recipe.Param.DAVSBand3;

            #endregion

            #region System Param

            cbxOutBondary.Checked = Recipe.Param.bInspOutboundary;
            txbSynchronizePath.Text = Recipe.Param.SynchronizePath;
            cbxAutoUpdate.Checked = Recipe.Param.bAutoSynchronizeRecipe;
            cbxRotateImage.Checked = Recipe.Param.RotateImageEnabled;
            cbxLogEnabled.Checked = Recipe.Param.LogEnabled;
            cbxTestModeEnabled.Checked = Recipe.Param.TestModeEnabled;
            combxTestModeType.SelectedIndex = Recipe.Param.TestModeType;
            combxTestModeType.Enabled = cbxTestModeEnabled.Checked;
            nudImgCount.Value = Recipe.Param.ImgCount;

            txbResolution.Text = Locate_Method_FS.pixel_resolution_.ToString();

            #endregion

            #region AOI InnerInsp

            txbInnerName.Text = Recipe.Param.InnerInspParam.InspName;
            cbxInnerEnabled.Checked = Recipe.Param.InnerInspParam.Enabled;
            nudInnerHTH.Value = Recipe.Param.InnerInspParam.HighThreshold;
            nudInnerLTh.Value = Recipe.Param.InnerInspParam.LowThreshold;
            nudInnerEdgeSkipSize.Value = Recipe.Param.InnerInspParam.EdgeSkipSizeWidth;
            nudInnerEdgeSkipHeight.Value = Recipe.Param.InnerInspParam.EdgeSkipSizeHeight;
            txbInnerImgIndex.Text = Recipe.Param.InnerInspParam.ImageIndex.ToString();
            txbInnerMinA.Text = Recipe.Param.InnerInspParam.LowArea.ToString();
            txbInnerMinH.Text = Recipe.Param.InnerInspParam.LowHeight.ToString();
            txbInnerMinW.Text = Recipe.Param.InnerInspParam.LowWidth.ToString();
            cbxInnerAreaEnabled.Checked = Recipe.Param.InnerInspParam.AEnabled;
            cbxInnerWidthEnabled.Checked = Recipe.Param.InnerInspParam.WEnabled;
            cbxInnerHeightEnabled.Checked = Recipe.Param.InnerInspParam.HEnabled;
            comboBoxInnerBand.SelectedIndex = (int)Recipe.Param.InnerInspParam.Band;

            #endregion

            #region AOI OuterInsp

            txbOuterName.Text = Recipe.Param.OuterInspParam.InspName;
            cbxOuterEnabled.Checked = Recipe.Param.OuterInspParam.Enabled;
            nudOuterHTH.Value = Recipe.Param.OuterInspParam.HighThreshold;
            nudOuterLTh.Value = Recipe.Param.OuterInspParam.LowThreshold;
            nudOuterEdgeSkipSize.Value = decimal.Parse(Recipe.Param.OuterInspParam.EdgeSkipSizeWidth.ToString());
            nudOuterEdgeSkipHeight.Value = decimal.Parse(Recipe.Param.OuterInspParam.EdgeSkipSizeHeight.ToString());
            txbOuterImgIndex.Text = Recipe.Param.OuterInspParam.ImageIndex.ToString();
            txbOuterMinA.Text = Recipe.Param.OuterInspParam.LowArea.ToString();
            txbOuterMinH.Text = Recipe.Param.OuterInspParam.LowHeight.ToString();
            txbOuterMinW.Text = Recipe.Param.OuterInspParam.LowWidth.ToString();
            cbxOuterAreaEnabled.Checked = Recipe.Param.OuterInspParam.AEnabled;
            cbxOuterWEnabled.Checked = Recipe.Param.OuterInspParam.WEnabled;
            cbxOuterHEnabled.Checked = Recipe.Param.OuterInspParam.HEnabled;
            comboBoxOuterBand.SelectedIndex = (int)Recipe.Param.OuterInspParam.Band;

            #endregion

            #region 電阻檢測參數

            #region Thin

            txbHoleName.Text = Recipe.Param.RisistAOIParam.ThinParam.HoleName;
            nudThinScratchEdgeSkipW.Value = decimal.Parse(Recipe.Param.RisistAOIParam.ThinParam.ThinScratchEdgeSkipWidth.ToString());
            nudThinScratchEdgeSkipH.Value = decimal.Parse(Recipe.Param.RisistAOIParam.ThinParam.ThinScratchEdgeSkipHeight.ToString());
            nudSensitivity.Value = decimal.Parse(Recipe.Param.RisistAOIParam.ThinParam.ThinScratchSensitivity.ToString());
            cbxThinEnabled.Checked = Recipe.Param.RisistAOIParam.ThinParam.Enabled;
            txbThinID.Text = Recipe.Param.RisistAOIParam.ThinParam.InspID.ToString();
            txbThinName.Text = Recipe.Param.RisistAOIParam.ThinParam.Name.ToString();
            nudThinDarkTh.Value = decimal.Parse(Recipe.Param.RisistAOIParam.ThinParam.Dark_Th.ToString());
            nudThinBrightTh.Value = decimal.Parse(Recipe.Param.RisistAOIParam.ThinParam.Bright_Th.ToString());
            txbThinMinArea.Text = Recipe.Param.RisistAOIParam.ThinParam.AreaMin.ToString();
            nudTophatDarkTh.Value = decimal.Parse(Recipe.Param.RisistAOIParam.ThinParam.TopHatDark_Th.ToString());
            nudTophatBrightTh.Value = decimal.Parse(Recipe.Param.RisistAOIParam.ThinParam.TopHatBright_Th.ToString());
            txbTophatMinArea.Text = Recipe.Param.RisistAOIParam.ThinParam.TopHatAreaMin.ToString();
            cbxThinAreaEnabled.Checked = Recipe.Param.RisistAOIParam.ThinParam.AreaEnabled;
            cbxThinWEnabled.Checked = Recipe.Param.RisistAOIParam.ThinParam.WidthEnabled;
            cbxThinHEnabled.Checked = Recipe.Param.RisistAOIParam.ThinParam.HeightEnabled;
            cbxTophatAreaEnabled.Checked = Recipe.Param.RisistAOIParam.ThinParam.TopHatAreaEnabled;
            cbxTophatWEnabled.Checked = Recipe.Param.RisistAOIParam.ThinParam.TopHatWidthEnabled;
            cbxTophatHEnabled.Checked = Recipe.Param.RisistAOIParam.ThinParam.TopHatHeightEnabled;
            txbThinInspW.Text = Recipe.Param.RisistAOIParam.ThinParam.WidthMin.ToString();
            txbThinInspH.Text = Recipe.Param.RisistAOIParam.ThinParam.HeightMin.ToString();
            txbTophatW.Text = Recipe.Param.RisistAOIParam.ThinParam.TopHatWidthMin.ToString();
            txbtophatH.Text = Recipe.Param.RisistAOIParam.ThinParam.TopHatHeightMin.ToString();
            txbSumArea.Text = Recipe.Param.RisistAOIParam.ThinParam.DefectSumArea.ToString();
            cbxSumAreaEnabled.Checked = Recipe.Param.RisistAOIParam.ThinParam.DefectSumAreaEnabled;
            nudThinEdgeSkipW.Value = decimal.Parse(Recipe.Param.RisistAOIParam.ThinParam.EdgeSkipWidth.ToString());
            nudThinEdgeSkipH.Value = decimal.Parse(Recipe.Param.RisistAOIParam.ThinParam.EdgeSkipHeight.ToString());
            nudTopHatEdgeExpWidth.Value = decimal.Parse(Recipe.Param.RisistAOIParam.ThinParam.TopHatEdgeExpandWidth.ToString());
            nudTopHatEdgeExpHeight.Value = decimal.Parse(Recipe.Param.RisistAOIParam.ThinParam.TopHatEdgeExpandHeight.ToString());
            nudSESizeWidth.Value = decimal.Parse(Recipe.Param.RisistAOIParam.ThinParam.SESizeWidth.ToString());
            nudSESizeHeight.Value = decimal.Parse(Recipe.Param.RisistAOIParam.ThinParam.SESizeHeight.ToString());
            cbxTopHatEnabled.Checked = Recipe.Param.RisistAOIParam.ThinParam.TopHatEnabled;
            cbxMeanEnabled.Checked = Recipe.Param.RisistAOIParam.ThinParam.MeanEnabled;
            cbxHistoAreaEnabled.Checked = Recipe.Param.RisistAOIParam.ThinParam.HistoEqAreaEnabled;
            txbHistoAreaMin.Text = Recipe.Param.RisistAOIParam.ThinParam.HistoEqAreaMin.ToString();
            cbxHistoEqEnabled.Checked = Recipe.Param.RisistAOIParam.ThinParam.HistoEqEnabled;
            cbxHistoHeightEnabled.Checked = Recipe.Param.RisistAOIParam.ThinParam.HistoEqHeightEnabled;
            txbHistoHeightMin.Text = Recipe.Param.RisistAOIParam.ThinParam.HistoEqHeightMin.ToString();
            nudHistoEqTh.Value = decimal.Parse(Recipe.Param.RisistAOIParam.ThinParam.HistoEqTh.ToString());
            cbxHistoWidthEnabled.Checked = Recipe.Param.RisistAOIParam.ThinParam.HistoEqWidth;
            txbHistoWidthMin.Text = Recipe.Param.RisistAOIParam.ThinParam.HistoEqWidthMin.ToString();
            nudHistoCloseHeight.Value = decimal.Parse(Recipe.Param.RisistAOIParam.ThinParam.HistoEqClosingHeight.ToString());
            nudHistoCloseWidth.Value = decimal.Parse(Recipe.Param.RisistAOIParam.ThinParam.HistoEqClosingWidth.ToString());
            nudHistoOpenHeight.Value = decimal.Parse(Recipe.Param.RisistAOIParam.ThinParam.HistoEqOpeningHeight.ToString());
            nudHistoOpenWidth.Value = decimal.Parse(Recipe.Param.RisistAOIParam.ThinParam.HistoEqOpeningWidth.ToString());
            nudMeanOpenRad.Value = decimal.Parse(Recipe.Param.RisistAOIParam.ThinParam.MeanOpenRad.ToString());
            nudMeanCloseRad.Value = decimal.Parse(Recipe.Param.RisistAOIParam.ThinParam.MeanCloseRad.ToString());
            nudHistoEdgeSkipW.Value = decimal.Parse(Recipe.Param.RisistAOIParam.ThinParam.HistoEdgeSkipWidth.ToString());
            nudHistoEdgeSkipH.Value = decimal.Parse(Recipe.Param.RisistAOIParam.ThinParam.HistoEdgeSkipHeight.ToString());
            nudExtH.Value = decimal.Parse(Recipe.Param.RisistAOIParam.ThinParam.ExtH.ToString());
            nudExtHTh.Value = decimal.Parse(Recipe.Param.RisistAOIParam.ThinParam.ExtThAdd.ToString());

            cbxThinHatAreaEnabled.Checked = Recipe.Param.RisistAOIParam.ThinParam.ThinHatAreaEnabled;
            cbxThinHatEnabled.Checked = Recipe.Param.RisistAOIParam.ThinParam.ThinHatEnabled;
            cbxThinHatHeightEnabled.Checked = Recipe.Param.RisistAOIParam.ThinParam.ThinHatHeightEnabled;
            cbxThinHatWidthEnabled.Checked = Recipe.Param.RisistAOIParam.ThinParam.ThinHatWidthEnabled;
            txbThinHatAreaMin.Text = Recipe.Param.RisistAOIParam.ThinParam.ThinHatAreaMin.ToString();
            nudThinHatBrightTh.Value = decimal.Parse(Recipe.Param.RisistAOIParam.ThinParam.ThinHatBrightTh.ToString());
            nudThinHatCloseHeight.Value = decimal.Parse(Recipe.Param.RisistAOIParam.ThinParam.ThinHatCloseHeight.ToString());
            nudThinHatCloseWidth.Value = decimal.Parse(Recipe.Param.RisistAOIParam.ThinParam.ThinHatCloseWidth.ToString());
            nudThinHatDarkTh.Value = decimal.Parse(Recipe.Param.RisistAOIParam.ThinParam.ThinHatDarkTh.ToString());
            nudThinHatSkipH.Value = decimal.Parse(Recipe.Param.RisistAOIParam.ThinParam.ThinHatEdgeSkipHeight.ToString());
            nudThinHatSkipW.Value = decimal.Parse(Recipe.Param.RisistAOIParam.ThinParam.ThinHatEdgeSkipWidth.ToString());
            txbThinHatHeightMin.Text = Recipe.Param.RisistAOIParam.ThinParam.ThinHatHeightMin.ToString();
            nudThinHatOpenHeight.Value = decimal.Parse(Recipe.Param.RisistAOIParam.ThinParam.ThinHatOpenHeight.ToString());
            nudThinHatOpenWidth.Value = decimal.Parse(Recipe.Param.RisistAOIParam.ThinParam.ThinHatOpenWidth.ToString());
            txbThinHatWidthMin.Text = Recipe.Param.RisistAOIParam.ThinParam.ThinHatWidthMin.ToString();

            //Thin劃痕
            cbxThinScratchEnabled.Checked = Recipe.Param.RisistAOIParam.ThinParam.ThinScratchEnabled;
            cbxThinScratchAreaEnabled.Checked = Recipe.Param.RisistAOIParam.ThinParam.ThinScratchAreaEnabled;
            cbxThinScratchWidthEnabled.Checked = Recipe.Param.RisistAOIParam.ThinParam.ThinScratchWidthEnabled;
            cbxThinScratchHeightEnabled.Checked = Recipe.Param.RisistAOIParam.ThinParam.ThinScratchHeightEnabled;
            txbThinScratchArea.Text = Recipe.Param.RisistAOIParam.ThinParam.ThinScratchAreaMin.ToString();
            txbThinScratchHeight.Text = Recipe.Param.RisistAOIParam.ThinParam.ThinScratchHeightMin.ToString();
            txbThinScratchWidth.Text = Recipe.Param.RisistAOIParam.ThinParam.ThinScratchWidthMin.ToString();
            nudThinScratchOpenW.Value = decimal.Parse(Recipe.Param.RisistAOIParam.ThinParam.OpenScratchSizeW.ToString());
            nudThinScratchCloseW.Value = decimal.Parse(Recipe.Param.RisistAOIParam.ThinParam.CloseScratchSizeW.ToString());
            nudThinScratchOpenH.Value = decimal.Parse(Recipe.Param.RisistAOIParam.ThinParam.OpenScratchSizeH.ToString());
            nudThinScratchCloseH.Value = decimal.Parse(Recipe.Param.RisistAOIParam.ThinParam.CloseScratchSizeH.ToString());
            nudThinScratchEdgeExWidth.Value = decimal.Parse(Recipe.Param.RisistAOIParam.ThinParam.ThinScratchEdgeExWidth.ToString());
            nudThinScratchEdgeExHeight.Value = decimal.Parse(Recipe.Param.RisistAOIParam.ThinParam.ThinScratchEdgeExHeight.ToString());

            #endregion

            #region Scratch

            cbxScratchEnabled.Checked = Recipe.Param.RisistAOIParam.ScratchParam.Enabled;
            txbScratchID.Text = Recipe.Param.RisistAOIParam.ScratchParam.InspID.ToString();
            txbScratchName.Text = Recipe.Param.RisistAOIParam.ScratchParam.Name.ToString();
            nudScratchInTH.Value = decimal.Parse(Recipe.Param.RisistAOIParam.ScratchParam.In_Th.ToString());
            nudScratchOutTH.Value = decimal.Parse(Recipe.Param.RisistAOIParam.ScratchParam.Out_Th.ToString());
            cbxScratchAreaEnabled.Checked = Recipe.Param.RisistAOIParam.ScratchParam.AreaEnabled;
            cbxScratchWidthEnabled.Checked = Recipe.Param.RisistAOIParam.ScratchParam.WidthEnabled;
            cbxScratchHeightEnabled.Checked = Recipe.Param.RisistAOIParam.ScratchParam.HeightEnabled;
            txbScratchWidthMin.Text = Recipe.Param.RisistAOIParam.ScratchParam.WidthMin.ToString();
            txbScratchHeightMin.Text = Recipe.Param.RisistAOIParam.ScratchParam.HeightMin.ToString();
            txbScratchMinArea.Text = Recipe.Param.RisistAOIParam.ScratchParam.AreaMin.ToString();
            cbxScratchOuterAreaEnabled.Checked = Recipe.Param.RisistAOIParam.ScratchParam.OuterAreaEnabled;
            cbxScratchOuterWidthEnabled.Checked = Recipe.Param.RisistAOIParam.ScratchParam.OuterWidthEnabled;
            cbxScratchOuterHeightEnabled.Checked = Recipe.Param.RisistAOIParam.ScratchParam.OuterHeightEnabled;
            txbScratchOuterWidthMin.Text = Recipe.Param.RisistAOIParam.ScratchParam.OuterWidthMin.ToString();
            txbScratchOuterHeightMin.Text = Recipe.Param.RisistAOIParam.ScratchParam.OuterHeightMin.ToString();
            txbScratchOuterAreaMin.Text = Recipe.Param.RisistAOIParam.ScratchParam.OuterAreaMin.ToString();

            #endregion

            #region Stain

            cbxStainEnabled.Checked = Recipe.Param.RisistAOIParam.StainParam.Enabled;
            txbStainID.Text = Recipe.Param.RisistAOIParam.StainParam.InspID.ToString();
            txbStainName.Text = Recipe.Param.RisistAOIParam.StainParam.Name.ToString();
            nudStainDarkTh.Value = decimal.Parse(Recipe.Param.RisistAOIParam.StainParam.Dark_Th.ToString());
            nudStainBrightTh.Value = decimal.Parse(Recipe.Param.RisistAOIParam.StainParam.Bright_Th.ToString());
            txbStainMinArea.Text = Recipe.Param.RisistAOIParam.StainParam.AreaMin.ToString();
            cbxStainAreaEnabled.Checked = Recipe.Param.RisistAOIParam.StainParam.AreaEnabled;
            cbxStainWidthEnabled.Checked = Recipe.Param.RisistAOIParam.StainParam.WidthEnabled;
            cbxStainHeightEnabled.Checked = Recipe.Param.RisistAOIParam.StainParam.HeightEnabled;
            txbStainWidthMin.Text = Recipe.Param.RisistAOIParam.StainParam.WidthMin.ToString();
            txbStainHeightMin.Text = Recipe.Param.RisistAOIParam.StainParam.HeightMin.ToString();
            #endregion

            #region R-Defect

            cbxRDefectEnabled.Checked = Recipe.Param.RisistAOIParam.RDefectParam.Enabled;
            txbRDefectID.Text = Recipe.Param.RisistAOIParam.RDefectParam.InspID.ToString();
            txbRDefectName.Text = Recipe.Param.RisistAOIParam.RDefectParam.Name.ToString();
            nudRDefectBrightMaxTh.Value = decimal.Parse(Recipe.Param.RisistAOIParam.RDefectParam.Bright_ThMax.ToString());
            nudRDefectBrightMinTh.Value = decimal.Parse(Recipe.Param.RisistAOIParam.RDefectParam.Bright_ThMin.ToString());
            nudRDefectDarkMaxTh.Value = decimal.Parse(Recipe.Param.RisistAOIParam.RDefectParam.Dark_ThMax.ToString());
            nudRDefectDarkMinTh.Value = decimal.Parse(Recipe.Param.RisistAOIParam.RDefectParam.Dark_ThMin.ToString());
            txbRDefectMinArea.Text = Recipe.Param.RisistAOIParam.RDefectParam.AreaMin.ToString();
            cbxRDefectAreaEnabled.Checked = Recipe.Param.RisistAOIParam.RDefectParam.AreaEnabled;
            cbxRDefectWidthEnabled.Checked = Recipe.Param.RisistAOIParam.RDefectParam.WidthEnabled;
            cbxRDefectHeightEnabled.Checked = Recipe.Param.RisistAOIParam.RDefectParam.HeightEnabled;
            txbRDefectWidthMin.Text = Recipe.Param.RisistAOIParam.RDefectParam.WidthMin.ToString();
            txbRDefectHeightMin.Text = Recipe.Param.RisistAOIParam.RDefectParam.HeightMin.ToString();
            nudRDefectExtWidth.Value = decimal.Parse(Recipe.Param.RisistAOIParam.RDefectParam.ExtWidth.ToString());
            nudRDefectExtHeight.Value = decimal.Parse(Recipe.Param.RisistAOIParam.RDefectParam.ExtHeight.ToString());
            nudRDefectSkipWidth.Value = decimal.Parse(Recipe.Param.RisistAOIParam.RDefectParam.SkipWidth.ToString());
            nudRDefectSkipHeight.Value = decimal.Parse(Recipe.Param.RisistAOIParam.RDefectParam.SkipHeight.ToString());

            #endregion

            #region R-Shift

            cbxRShiftEnabled.Checked = Recipe.Param.RisistAOIParam.RShiftNewParam.Enabled;
            nudRShiftTh.Value = decimal.Parse(Recipe.Param.RisistAOIParam.RShiftNewParam.ShiftStandard.ToString());
            txbRShiftName.Text = Recipe.Param.RisistAOIParam.RShiftNewParam.Name;
            nudTargetRWidth.Value = decimal.Parse(Recipe.Param.RisistAOIParam.RShiftNewParam.SelectWidth.ToString());
            nudTargetEArea.Value = decimal.Parse(Recipe.Param.RisistAOIParam.RShiftNewParam.SelectArea.ToString());
            nudAutoTthSigma.Value = decimal.Parse(Recipe.Param.RisistAOIParam.RShiftNewParam.AutoThSigma.ToString());
            nudRShiftClosingSize.Value = decimal.Parse(Recipe.Param.RisistAOIParam.RShiftNewParam.CloseSize.ToString());

            #endregion

            #endregion
            

            #endregion

            #region Region 編輯介面

            UpdateMethodListView(Recipe.Param.MethodRegionList);
            UpdateEditListView(Recipe.Param.EditRegionList);
            UpdateUsedListView(Recipe.Param.UsedRegionList);

            #endregion

            b_LoadRecipe = false; // (20200130) Jeff Revised!
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Window">顯示之Halcon Window</param>
        /// <param name="Image">顯示之影像</param>
        /// <param name="Regions">顯示之區域</param>
        /// <param name="IsClear"></param>
        /// <param name="Color"></param>
        /// <param name="displayType"></param>
        /// <param name="b_SetPart"></param>
        private void DisplayRegions(HWindow Window, HObject Image, HObject Regions, bool IsClear, string Color, enuDisplayRegionType displayType, bool b_SetPart = false) // (20200130) Jeff Revised!
        {
            if (IsClear)
            {
                HOperatorSet.ClearWindow(Window);
                HOperatorSet.DispObj(Image, Window);
            }

            HOperatorSet.SetColor(Window, Color);
            HOperatorSet.SetDraw(Window, displayType.ToString());
            HOperatorSet.DispObj(Regions, Window);
            if (b_SetPart) // (20200130) Jeff Revised!
                HOperatorSet.SetPart(Window, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part
        }

        private void UpdateMethodListView(List<clsRecipe.clsMethodRegion> RegionList)
        {
            this.listViewMethod.BeginUpdate();
            listViewMethod.Items.Clear();
            for (int i = 0; i < RegionList.Count; i++)
            {
                listViewMethod.Items.Add(RegionList[i].Name);
            }
            this.listViewMethod.EndUpdate();
        }

        private void UpdateUsedListView(List<clsRecipe.clsMethodRegion> RegionList)
        {
            this.listViewUsedRegion.BeginUpdate();
            listViewUsedRegion.Items.Clear();
            comboBoxRegionSelect.Items.Clear();
            for (int i = 0; i < RegionList.Count; i++)
            {
                ListViewItem lvi = new ListViewItem(RegionList[i].Name);
                lvi.SubItems.Add(RegionList[i].bShowRegion.ToString());
                lvi.SubItems.Add(RegionList[i].ShowImageID.ToString());

                lvi.UseItemStyleForSubItems = false;
                ListViewItem.ListViewSubItem Itm = new ListViewItem.ListViewSubItem();
                Itm.BackColor = clsStaticTool.GetSystemColor(RegionList[i].Color);
                Itm.ForeColor = Color.Black;
                Itm.Text = RegionList[i].Color.ToString();
                lvi.SubItems.Add(Itm);


                listViewUsedRegion.Items.Add(lvi);
                comboBoxRegionSelect.Items.Add(RegionList[i].Name);
            }
            this.listViewUsedRegion.EndUpdate();
        }

        private void UpdateEditListView(List<clsRecipe.clsMethodRegion> RegionList)
        {
            this.listViewEditRegion.BeginUpdate();
            listViewEditRegion.Items.Clear();
            for (int i = 0; i < RegionList.Count; i++)
            {
                ListViewItem lvi = new ListViewItem(RegionList[i].Name);
                listViewEditRegion.Items.Add(lvi);
            }
            this.listViewEditRegion.EndUpdate();
        }

        public void ParamGetFromUI()
        {
            #region SegParam

            Recipe.Param.SegParam.MinScore = double.Parse(nudAMinScore.Value.ToString());
            Recipe.Param.SegParam.ThEnabled = cbxThEnabled.Checked;

            // (20200130) Jeff Revised!
            if (radioButton_OrigImg_Match.Checked)
                Recipe.Param.SegParam.ImageSource_SegImg = enu_ImageSource.原始;
            else
                Recipe.Param.SegParam.ImageSource_SegImg = enu_ImageSource.影像A;

            Recipe.Param.SegParam.SegImgIndex = int.Parse(nudInspectImgID.Value.ToString());
            Recipe.Param.SegParam.TeachMask = cbxMaskEnabled.Checked;
            Recipe.Param.SegParam.OpeningWidth = int.Parse(nudMatchOpenWidth.Value.ToString());
            Recipe.Param.SegParam.OpeningHeight = int.Parse(nudMatchOpenHeight.Value.ToString());
            Recipe.Param.SegParam.ClosingingWidth = int.Parse(nudMatchCloseWidth.Value.ToString());
            Recipe.Param.SegParam.ClosingingHeight = int.Parse(nudMatchCloseHeight.Value.ToString());
            Recipe.Param.SegParam.MaskHistoeq = cbxHistoEq.Checked;
            Recipe.Param.SegParam.BandIndex = combxBand.SelectedIndex;
            Recipe.Param.SegParam.MatchPreProcessEnabled = cbxMatchThEnabled.Checked;
            Recipe.Param.SegParam.IsNCCMode = cbxNCCMode.Checked;
            Recipe.Param.SegParam.PartitionGrayValue = int.Parse(txbPartGrayValue.Text);

            #endregion

            #region AdvParam

            Recipe.Param.AdvParam.MatchSacleSize = double.Parse(nudScaleSize.Value.ToString());
            Recipe.Param.AdvParam.MatchNumber = int.Parse(nudMatchNumber.Value.ToString());
            Recipe.Param.AdvParam.AngleStart = int.Parse(nudAngleStart.Value.ToString());
            Recipe.Param.AdvParam.AngleExtent = int.Parse(nudAngleExtent.Value.ToString());
            Recipe.Param.AdvParam.AngleStep = float.Parse(nudAngleStep.Value.ToString());
            Recipe.Param.AdvParam.NumLevels = int.Parse(nudNumLevels.Value.ToString());
            Recipe.Param.AdvParam.MinContrast = int.Parse(nudMinContrast.Value.ToString());
            Recipe.Param.AdvParam.Optimization = cbxOptimization.SelectedIndex;
            Recipe.Param.AdvParam.Metric = cbxMetric.SelectedIndex;
            Recipe.Param.AdvParam.AngleStepAuto = cbxAngleStepAuto.Checked;
            Recipe.Param.AdvParam.ContrastAuto = cbxContrastAuto.Checked;
            Recipe.Param.AdvParam.MinContrastAuto = cbxMinContrastAuto.Checked;
            Recipe.Param.AdvParam.Greediness = double.Parse(nudGreediness.Value.ToString());
            Recipe.Param.AdvParam.Overlap = double.Parse(nudOverlap.Value.ToString());
            Recipe.Param.AdvParam.SubPixel = cbxSubPixel.SelectedIndex;
            Recipe.Param.AdvParam.OpeningNum = int.Parse(nudOpeningNum.Value.ToString());
            Recipe.Param.AdvParam.ScaleMin = double.Parse(nudScaleMin.Value.ToString());
            Recipe.Param.AdvParam.ScaleMax = double.Parse(nudScaleMax.Value.ToString());
            Recipe.Param.AdvParam.ContrastSmall = int.Parse(nudContrast.Value.ToString());
            Recipe.Param.AdvParam.ContrastLarge = int.Parse(nudContrastHigh.Value.ToString());
            Recipe.Param.AdvParam.MinObjSize = int.Parse(nudMinObjSize.Value.ToString());
            Recipe.Param.AdvParam.bFillupEnabled = cbxFillup.Checked;
            Recipe.Param.AdvParam.ClosingNum = int.Parse(nudClosingNum.Value.ToString());
            Recipe.Param.AdvParam.FillupMin = int.Parse(nudFillupMin.Value.ToString());
            Recipe.Param.AdvParam.FillupMax = int.Parse(nudFillupMax.Value.ToString());
            Recipe.Param.AdvParam.FillupType = (enuFillupType)comboBoxFillupType.SelectedIndex;

            #endregion

            #region AOI InnerInsp

            Recipe.Param.InnerInspParam.Enabled = cbxInnerEnabled.Checked;
            Recipe.Param.InnerInspParam.HighThreshold = int.Parse(nudInnerHTH.Value.ToString());
            Recipe.Param.InnerInspParam.ImageIndex = int.Parse(txbInnerImgIndex.Text);
            Recipe.Param.InnerInspParam.LowArea = int.Parse(txbInnerMinA.Text);
            Recipe.Param.InnerInspParam.LowHeight = int.Parse(txbInnerMinH.Text);
            Recipe.Param.InnerInspParam.LowThreshold = int.Parse(nudInnerLTh.Value.ToString());
            Recipe.Param.InnerInspParam.LowWidth = int.Parse(txbInnerMinW.Text);
            Recipe.Param.InnerInspParam.EdgeSkipSizeWidth = int.Parse(nudInnerEdgeSkipSize.Value.ToString());
            Recipe.Param.InnerInspParam.EdgeSkipSizeHeight = int.Parse(nudInnerEdgeSkipHeight.Value.ToString());
            Recipe.Param.InnerInspParam.AEnabled = cbxInnerAreaEnabled.Checked;
            Recipe.Param.InnerInspParam.WEnabled = cbxInnerWidthEnabled.Checked;
            Recipe.Param.InnerInspParam.HEnabled = cbxInnerHeightEnabled.Checked;
            Recipe.Param.InnerInspParam.Band = (enuBand)comboBoxInnerBand.SelectedIndex;
            Recipe.Param.InnerInspParam.InspName = txbInnerName.Text;

            #endregion

            #region AOI OuterInsp

            Recipe.Param.OuterInspParam.Enabled = cbxOuterEnabled.Checked;
            Recipe.Param.OuterInspParam.HighThreshold = int.Parse(nudOuterHTH.Value.ToString());
            Recipe.Param.OuterInspParam.ImageIndex = int.Parse(txbOuterImgIndex.Text);
            Recipe.Param.OuterInspParam.LowArea = int.Parse(txbOuterMinA.Text);
            Recipe.Param.OuterInspParam.LowHeight = int.Parse(txbOuterMinH.Text);
            Recipe.Param.OuterInspParam.LowThreshold = int.Parse(nudOuterLTh.Value.ToString());
            Recipe.Param.OuterInspParam.LowWidth = int.Parse(txbOuterMinW.Text);
            Recipe.Param.OuterInspParam.EdgeSkipSizeWidth = int.Parse(nudOuterEdgeSkipSize.Value.ToString());
            Recipe.Param.OuterInspParam.EdgeSkipSizeHeight = int.Parse(nudOuterEdgeSkipHeight.Value.ToString());
            Recipe.Param.OuterInspParam.AEnabled = cbxOuterAreaEnabled.Checked;
            Recipe.Param.OuterInspParam.WEnabled = cbxOuterWEnabled.Checked;
            Recipe.Param.OuterInspParam.HEnabled = cbxOuterHEnabled.Checked;
            Recipe.Param.OuterInspParam.Band = (enuBand)comboBoxOuterBand.SelectedIndex;
            Recipe.Param.OuterInspParam.InspName = txbOuterName.Text;

            #endregion

            #region DAVS Param

            Recipe.Param.DAVSInspType = combxAIType.SelectedIndex;
            Recipe.Param.DAVSImgID = int.Parse(nudDAVSImageId.Value.ToString());
            Recipe.Param.SaveAOIImgEnabled = cbxSaveImgEnable.Checked;
            Recipe.Param.SaveAOIImgType = combxSaveImgType.SelectedIndex;
            Recipe.Param.bDAVSMixImgBand = cbxDAVSMixBandEnabled.Checked;
            Recipe.Param.DAVSBand1 = (enuBand)comboBoxDAVSBand1.SelectedIndex;
            Recipe.Param.DAVSBand2 = (enuBand)comboBoxDAVSBand2.SelectedIndex;
            Recipe.Param.DAVSBand3 = (enuBand)comboBoxDAVSBand3.SelectedIndex;
            Recipe.Param.DAVSBand1ImgIndex = int.Parse(nudDAVSBand1ImageIndex.Value.ToString());
            Recipe.Param.DAVSBand2ImgIndex = int.Parse(nudDAVSBand2ImageIndex.Value.ToString());
            Recipe.Param.DAVSBand3ImgIndex = int.Parse(nudDAVSBand3ImageIndex.Value.ToString());

            #endregion

            #region System Param

            Recipe.Param.SynchronizePath = txbSynchronizePath.Text;
            Recipe.Param.bAutoSynchronizeRecipe = cbxAutoUpdate.Checked;
            Recipe.Param.RotateImageEnabled = cbxRotateImage.Checked;
            Recipe.Param.LogEnabled = cbxLogEnabled.Checked;
            Recipe.Param.bInspOutboundary = cbxOutBondary.Checked;
            Recipe.Param.TestModeEnabled = cbxTestModeEnabled.Checked;
            Recipe.Param.TestModeType = combxTestModeType.SelectedIndex;
            Recipe.Param.ImgCount = int.Parse(nudImgCount.Value.ToString());

            #endregion

            #region 電阻檢測參數

            #region Thin

            Recipe.Param.RisistAOIParam.ThinParam.ThinScratchSensitivity = int.Parse(nudSensitivity.Value.ToString());
            Recipe.Param.RisistAOIParam.ThinParam.ThinScratchEdgeSkipWidth = int.Parse(nudThinScratchEdgeSkipW.Value.ToString());
            Recipe.Param.RisistAOIParam.ThinParam.ThinScratchEdgeSkipHeight = int.Parse(nudThinScratchEdgeSkipH.Value.ToString());
            Recipe.Param.RisistAOIParam.ThinParam.Enabled = cbxThinEnabled.Checked;
            Recipe.Param.RisistAOIParam.ThinParam.InspID = int.Parse(txbThinID.Text);
            Recipe.Param.RisistAOIParam.ThinParam.Name = txbThinName.Text;
            Recipe.Param.RisistAOIParam.ThinParam.Dark_Th = int.Parse(nudThinDarkTh.Value.ToString());
            Recipe.Param.RisistAOIParam.ThinParam.Bright_Th = int.Parse(nudThinBrightTh.Value.ToString());
            Recipe.Param.RisistAOIParam.ThinParam.AreaMin = int.Parse(txbThinMinArea.Text);
            Recipe.Param.RisistAOIParam.ThinParam.TopHatDark_Th = int.Parse(nudTophatDarkTh.Value.ToString());
            Recipe.Param.RisistAOIParam.ThinParam.TopHatBright_Th = int.Parse(nudTophatBrightTh.Value.ToString());
            Recipe.Param.RisistAOIParam.ThinParam.TopHatAreaMin = int.Parse(txbTophatMinArea.Text);
            Recipe.Param.RisistAOIParam.ThinParam.AreaEnabled = cbxThinAreaEnabled.Checked;
            Recipe.Param.RisistAOIParam.ThinParam.WidthEnabled = cbxThinWEnabled.Checked;
            Recipe.Param.RisistAOIParam.ThinParam.HeightEnabled = cbxThinHEnabled.Checked;
            Recipe.Param.RisistAOIParam.ThinParam.TopHatAreaEnabled = cbxTophatAreaEnabled.Checked;
            Recipe.Param.RisistAOIParam.ThinParam.TopHatWidthEnabled = cbxTophatWEnabled.Checked;
            Recipe.Param.RisistAOIParam.ThinParam.TopHatHeightEnabled = cbxTophatHEnabled.Checked;
            Recipe.Param.RisistAOIParam.ThinParam.WidthMin = int.Parse(txbThinInspW.Text);
            Recipe.Param.RisistAOIParam.ThinParam.HeightMin = int.Parse(txbThinInspH.Text);
            Recipe.Param.RisistAOIParam.ThinParam.TopHatWidthMin = int.Parse(txbTophatW.Text);
            Recipe.Param.RisistAOIParam.ThinParam.TopHatHeightMin = int.Parse(txbtophatH.Text);
            Recipe.Param.RisistAOIParam.ThinParam.DefectSumAreaEnabled = cbxSumAreaEnabled.Checked;
            Recipe.Param.RisistAOIParam.ThinParam.DefectSumArea = int.Parse(txbSumArea.Text);
            Recipe.Param.RisistAOIParam.ThinParam.EdgeSkipWidth = int.Parse(nudThinEdgeSkipW.Value.ToString());
            Recipe.Param.RisistAOIParam.ThinParam.EdgeSkipHeight = int.Parse(nudThinEdgeSkipH.Value.ToString());
            Recipe.Param.RisistAOIParam.ThinParam.TopHatEdgeExpandWidth = int.Parse(nudTopHatEdgeExpWidth.Value.ToString());
            Recipe.Param.RisistAOIParam.ThinParam.TopHatEdgeExpandHeight = int.Parse(nudTopHatEdgeExpHeight.Value.ToString());
            Recipe.Param.RisistAOIParam.ThinParam.SESizeWidth = int.Parse(nudSESizeWidth.Value.ToString());
            Recipe.Param.RisistAOIParam.ThinParam.SESizeHeight = int.Parse(nudSESizeHeight.Value.ToString());
            Recipe.Param.RisistAOIParam.ThinParam.MeanEnabled = cbxMeanEnabled.Checked;
            Recipe.Param.RisistAOIParam.ThinParam.TopHatEnabled = cbxTopHatEnabled.Checked;
            Recipe.Param.RisistAOIParam.ThinParam.HoleName = txbHoleName.Text;

            Recipe.Param.RisistAOIParam.ThinParam.HistoEqAreaEnabled = cbxHistoAreaEnabled.Checked;
            Recipe.Param.RisistAOIParam.ThinParam.HistoEqAreaMin = int.Parse(txbHistoAreaMin.Text);
            Recipe.Param.RisistAOIParam.ThinParam.HistoEqEnabled = cbxHistoEqEnabled.Checked;
            Recipe.Param.RisistAOIParam.ThinParam.HistoEqHeightEnabled = cbxHistoHeightEnabled.Checked;
            Recipe.Param.RisistAOIParam.ThinParam.HistoEqHeightMin = int.Parse(txbHistoHeightMin.Text);
            Recipe.Param.RisistAOIParam.ThinParam.HistoEqTh = int.Parse(nudHistoEqTh.Value.ToString());
            Recipe.Param.RisistAOIParam.ThinParam.HistoEqWidth = cbxHistoWidthEnabled.Checked;
            Recipe.Param.RisistAOIParam.ThinParam.HistoEqWidthMin = int.Parse(txbHistoWidthMin.Text);
            Recipe.Param.RisistAOIParam.ThinParam.HistoEqClosingHeight = int.Parse(nudHistoCloseHeight.Value.ToString());
            Recipe.Param.RisistAOIParam.ThinParam.HistoEqClosingWidth = int.Parse(nudHistoCloseWidth.Value.ToString());
            Recipe.Param.RisistAOIParam.ThinParam.HistoEqOpeningHeight = int.Parse(nudHistoOpenHeight.Value.ToString());
            Recipe.Param.RisistAOIParam.ThinParam.HistoEqOpeningWidth = int.Parse(nudHistoOpenWidth.Value.ToString());
            Recipe.Param.RisistAOIParam.ThinParam.MeanCloseRad = double.Parse(nudMeanCloseRad.Value.ToString());
            Recipe.Param.RisistAOIParam.ThinParam.MeanOpenRad = double.Parse(nudMeanOpenRad.Value.ToString());
            Recipe.Param.RisistAOIParam.ThinParam.HistoEdgeSkipWidth = int.Parse(nudHistoEdgeSkipW.Value.ToString());
            Recipe.Param.RisistAOIParam.ThinParam.HistoEdgeSkipHeight = int.Parse(nudHistoEdgeSkipH.Value.ToString());
            Recipe.Param.RisistAOIParam.ThinParam.ExtH = int.Parse(nudExtH.Value.ToString());
            Recipe.Param.RisistAOIParam.ThinParam.ExtThAdd = int.Parse(nudExtHTh.Value.ToString());

            //Thin劃痕
            Recipe.Param.RisistAOIParam.ThinParam.ThinScratchEnabled = cbxThinScratchEnabled.Checked;
            Recipe.Param.RisistAOIParam.ThinParam.ThinScratchAreaEnabled = cbxThinScratchAreaEnabled.Checked;
            Recipe.Param.RisistAOIParam.ThinParam.ThinScratchWidthEnabled = cbxThinScratchWidthEnabled.Checked;
            Recipe.Param.RisistAOIParam.ThinParam.ThinScratchHeightEnabled = cbxThinScratchHeightEnabled.Checked;
            Recipe.Param.RisistAOIParam.ThinParam.ThinScratchAreaMin = int.Parse(txbThinScratchArea.Text);
            Recipe.Param.RisistAOIParam.ThinParam.ThinScratchHeightMin = int.Parse(txbThinScratchHeight.Text);
            Recipe.Param.RisistAOIParam.ThinParam.ThinScratchWidthMin = int.Parse(txbThinScratchWidth.Text);
            Recipe.Param.RisistAOIParam.ThinParam.OpenScratchSizeW = int.Parse(nudThinScratchOpenW.Value.ToString());
            Recipe.Param.RisistAOIParam.ThinParam.CloseScratchSizeW = int.Parse(nudThinScratchCloseW.Value.ToString());
            Recipe.Param.RisistAOIParam.ThinParam.OpenScratchSizeH = int.Parse(nudThinScratchOpenH.Value.ToString());
            Recipe.Param.RisistAOIParam.ThinParam.CloseScratchSizeH = int.Parse(nudThinScratchCloseH.Value.ToString());
            Recipe.Param.RisistAOIParam.ThinParam.ThinScratchEdgeExWidth = int.Parse(nudThinScratchEdgeExWidth.Value.ToString());
            Recipe.Param.RisistAOIParam.ThinParam.ThinScratchEdgeExHeight = int.Parse(nudThinScratchEdgeExHeight.Value.ToString());

            //Thin TopHat 大面積
            Recipe.Param.RisistAOIParam.ThinParam.ThinHatAreaEnabled = cbxThinHatAreaEnabled.Checked;
            Recipe.Param.RisistAOIParam.ThinParam.ThinHatEnabled = cbxThinHatEnabled.Checked;
            Recipe.Param.RisistAOIParam.ThinParam.ThinHatHeightEnabled = cbxThinHatHeightEnabled.Checked;
            Recipe.Param.RisistAOIParam.ThinParam.ThinHatWidthEnabled = cbxThinHatWidthEnabled.Checked;
            Recipe.Param.RisistAOIParam.ThinParam.ThinHatAreaMin = int.Parse(txbThinHatAreaMin.Text);
            Recipe.Param.RisistAOIParam.ThinParam.ThinHatBrightTh = int.Parse(nudThinHatBrightTh.Value.ToString());
            Recipe.Param.RisistAOIParam.ThinParam.ThinHatCloseHeight = int.Parse(nudThinHatCloseHeight.Value.ToString());
            Recipe.Param.RisistAOIParam.ThinParam.ThinHatCloseWidth = int.Parse(nudThinHatCloseWidth.Value.ToString());
            Recipe.Param.RisistAOIParam.ThinParam.ThinHatDarkTh = int.Parse(nudThinHatDarkTh.Value.ToString());
            Recipe.Param.RisistAOIParam.ThinParam.ThinHatEdgeSkipHeight = int.Parse(nudThinHatSkipH.Value.ToString());
            Recipe.Param.RisistAOIParam.ThinParam.ThinHatEdgeSkipWidth = int.Parse(nudThinHatSkipW.Value.ToString());
            Recipe.Param.RisistAOIParam.ThinParam.ThinHatHeightMin = int.Parse(txbThinHatHeightMin.Text);
            Recipe.Param.RisistAOIParam.ThinParam.ThinHatOpenHeight = int.Parse(nudThinHatOpenHeight.Value.ToString());
            Recipe.Param.RisistAOIParam.ThinParam.ThinHatOpenWidth = int.Parse(nudThinHatOpenWidth.Value.ToString());
            Recipe.Param.RisistAOIParam.ThinParam.ThinHatWidthMin = int.Parse(txbThinHatWidthMin.Text);

            #endregion

            #region Scratch

            Recipe.Param.RisistAOIParam.ScratchParam.Enabled = cbxScratchEnabled.Checked;
            Recipe.Param.RisistAOIParam.ScratchParam.InspID = int.Parse(txbScratchID.Text);
            Recipe.Param.RisistAOIParam.ScratchParam.Name = txbScratchName.Text;
            Recipe.Param.RisistAOIParam.ScratchParam.In_Th = int.Parse(nudScratchInTH.Value.ToString());
            Recipe.Param.RisistAOIParam.ScratchParam.Out_Th = int.Parse(nudScratchOutTH.Value.ToString());
            Recipe.Param.RisistAOIParam.ScratchParam.AreaEnabled = cbxScratchAreaEnabled.Checked;
            Recipe.Param.RisistAOIParam.ScratchParam.WidthEnabled = cbxScratchWidthEnabled.Checked;
            Recipe.Param.RisistAOIParam.ScratchParam.HeightEnabled = cbxScratchHeightEnabled.Checked;
            Recipe.Param.RisistAOIParam.ScratchParam.WidthMin = int.Parse(txbScratchWidthMin.Text);
            Recipe.Param.RisistAOIParam.ScratchParam.HeightMin = int.Parse(txbScratchHeightMin.Text);
            Recipe.Param.RisistAOIParam.ScratchParam.AreaMin = int.Parse(txbScratchMinArea.Text);
            Recipe.Param.RisistAOIParam.ScratchParam.OuterAreaEnabled = cbxScratchOuterAreaEnabled.Checked;
            Recipe.Param.RisistAOIParam.ScratchParam.OuterWidthEnabled = cbxScratchOuterWidthEnabled.Checked;
            Recipe.Param.RisistAOIParam.ScratchParam.OuterHeightEnabled = cbxScratchOuterHeightEnabled.Checked;
            Recipe.Param.RisistAOIParam.ScratchParam.OuterWidthMin = int.Parse(txbScratchOuterWidthMin.Text);
            Recipe.Param.RisistAOIParam.ScratchParam.OuterHeightMin = int.Parse(txbScratchOuterHeightMin.Text);
            Recipe.Param.RisistAOIParam.ScratchParam.OuterAreaMin = int.Parse(txbScratchOuterAreaMin.Text);

            #endregion

            #region Stain

            Recipe.Param.RisistAOIParam.StainParam.Enabled = cbxStainEnabled.Checked;
            Recipe.Param.RisistAOIParam.StainParam.InspID = int.Parse(txbStainID.Text);
            Recipe.Param.RisistAOIParam.StainParam.Name = txbStainName.Text;
            Recipe.Param.RisistAOIParam.StainParam.Dark_Th = int.Parse(nudStainDarkTh.Value.ToString());
            Recipe.Param.RisistAOIParam.StainParam.Bright_Th = int.Parse(nudStainBrightTh.Value.ToString());
            Recipe.Param.RisistAOIParam.StainParam.AreaMin = int.Parse(txbStainMinArea.Text);
            Recipe.Param.RisistAOIParam.StainParam.AreaEnabled = cbxStainAreaEnabled.Checked;
            Recipe.Param.RisistAOIParam.StainParam.WidthEnabled = cbxStainWidthEnabled.Checked;
            Recipe.Param.RisistAOIParam.StainParam.HeightEnabled = cbxStainHeightEnabled.Checked;
            Recipe.Param.RisistAOIParam.StainParam.WidthMin = int.Parse(txbStainWidthMin.Text);
            Recipe.Param.RisistAOIParam.StainParam.HeightMin = int.Parse(txbStainHeightMin.Text);

            #endregion

            #region R-Defect

            Recipe.Param.RisistAOIParam.RDefectParam.Enabled = cbxRDefectEnabled.Checked;
            Recipe.Param.RisistAOIParam.RDefectParam.InspID = int.Parse(txbRDefectID.Text);
            Recipe.Param.RisistAOIParam.RDefectParam.Name = txbRDefectName.Text;
            Recipe.Param.RisistAOIParam.RDefectParam.Dark_ThMin = int.Parse(nudRDefectDarkMinTh.Value.ToString());
            Recipe.Param.RisistAOIParam.RDefectParam.Bright_ThMin = int.Parse(nudRDefectBrightMinTh.Value.ToString());
            Recipe.Param.RisistAOIParam.RDefectParam.Dark_ThMax = int.Parse(nudRDefectDarkMaxTh.Value.ToString());
            Recipe.Param.RisistAOIParam.RDefectParam.Bright_ThMax = int.Parse(nudRDefectBrightMaxTh.Value.ToString());
            Recipe.Param.RisistAOIParam.RDefectParam.AreaMin = int.Parse(txbRDefectMinArea.Text);
            Recipe.Param.RisistAOIParam.RDefectParam.AreaEnabled = cbxRDefectAreaEnabled.Checked;
            Recipe.Param.RisistAOIParam.RDefectParam.WidthEnabled = cbxRDefectWidthEnabled.Checked;
            Recipe.Param.RisistAOIParam.RDefectParam.HeightEnabled = cbxRDefectHeightEnabled.Checked;
            Recipe.Param.RisistAOIParam.RDefectParam.WidthMin = int.Parse(txbRDefectWidthMin.Text);
            Recipe.Param.RisistAOIParam.RDefectParam.HeightMin = int.Parse(txbRDefectHeightMin.Text);
            Recipe.Param.RisistAOIParam.RDefectParam.ExtWidth = int.Parse(nudRDefectExtWidth.Value.ToString());
            Recipe.Param.RisistAOIParam.RDefectParam.ExtHeight = int.Parse(nudRDefectExtHeight.Value.ToString());
            Recipe.Param.RisistAOIParam.RDefectParam.SkipWidth = int.Parse(nudRDefectSkipWidth.Value.ToString());
            Recipe.Param.RisistAOIParam.RDefectParam.SkipHeight = int.Parse(nudRDefectSkipHeight.Value.ToString());

            #endregion

            #region R-Shift

            Recipe.Param.RisistAOIParam.RShiftNewParam.Enabled = cbxRShiftEnabled.Checked;
            Recipe.Param.RisistAOIParam.RShiftNewParam.ShiftStandard = int.Parse(nudRShiftTh.Value.ToString());
            Recipe.Param.RisistAOIParam.RShiftNewParam.Name = txbRShiftName.Text;
            Recipe.Param.RisistAOIParam.RShiftNewParam.AutoThSigma = double.Parse(nudAutoTthSigma.Value.ToString());
            Recipe.Param.RisistAOIParam.RShiftNewParam.SelectArea = int.Parse(nudTargetEArea.Value.ToString());
            Recipe.Param.RisistAOIParam.RShiftNewParam.SelectWidth = int.Parse(nudTargetRWidth.Value.ToString());
            Recipe.Param.RisistAOIParam.RShiftNewParam.CloseSize = int.Parse(nudRShiftClosingSize.Value.ToString());

            #endregion

            #endregion

            #region UI

            for (int i = 0; i < Recipe.Param.MethodList.Count; i++)
            {
                Recipe.Param.MethodList[i].Enabled = cbxListMethod.GetItemChecked(i);
            }

            #endregion

            #region Merge Defect Info

            // 取得所有AOI演算法之優先權中之最大值
            if (Recipe.Param.AlgorithmList.Count > 0)
                Recipe.Param.PriorityLayerCount = Recipe.Param.AlgorithmList.Max(t => t.Priority);
            
            #endregion
        }

        public void ClearDisplay(HObject OrgImage, bool b_SetPart = false) // (20200130) Jeff Revised!
        {
            HOperatorSet.ClearWindow(DisplayWindows.HalconWindow);
            HOperatorSet.DispObj(OrgImage, DisplayWindows.HalconWindow);
            SelectRegions.Dispose();
            HOperatorSet.GenEmptyRegion(out SelectRegions);
            Mean = new HTuple();
            Min = new HTuple();
            Ratio = new HTuple();
            Radius = new HTuple();

            if (b_SetPart) // (20200130) Jeff Revised!
                HOperatorSet.SetPart(DisplayWindows.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part
        }

        public static HDrawingObject GetDrawObj(HDrawingObject.HDrawingObjectType Type, InductanceInspRole Recipe)
        {
            HDrawingObject drawing_Rect = new HDrawingObject(0, 0, 100, 100);

            if (Type == HDrawingObject.HDrawingObjectType.RECTANGLE1)
            {
                drawing_Rect = HDrawingObject.CreateDrawingObject(Type,
                                                                  Recipe.Param.SegParam.GoldenCenterY - 100,
                                                                  Recipe.Param.SegParam.GoldenCenterX - 100,
                                                                  Recipe.Param.SegParam.GoldenCenterY + 100,
                                                                  Recipe.Param.SegParam.GoldenCenterX + 100);
            }
            else if (Type == HDrawingObject.HDrawingObjectType.RECTANGLE2)
            {
                drawing_Rect = HDrawingObject.CreateDrawingObject(Type,
                                                                  Recipe.Param.SegParam.GoldenCenterY,
                                                                  Recipe.Param.SegParam.GoldenCenterX,
                                                                  0, 100, 100);
            }
            else if (Type == HDrawingObject.HDrawingObjectType.CIRCLE)
            {
                drawing_Rect = HDrawingObject.CreateDrawingObject(Type,
                                                                  Recipe.Param.SegParam.GoldenCenterY,
                                                                  Recipe.Param.SegParam.GoldenCenterX,
                                                                  100);
            }
            else
            {
                drawing_Rect = HDrawingObject.CreateDrawingObject(Type,
                                                                  Recipe.Param.SegParam.GoldenCenterY,
                                                                  Recipe.Param.SegParam.GoldenCenterX,
                                                                  0, 100, 100);
            }

            return drawing_Rect;
        }

        /// <summary>
        /// 更新演算法列表 (listViewAlgorithm)
        /// </summary>
        public void UpdateAlgorithmList()
        {
            this.listViewAlgorithm.BeginUpdate();
            listViewAlgorithm.Items.Clear();
            foreach (clsRecipe.clsAlgorithm Insp in Recipe.Param.AlgorithmList)
            {
                ListViewItem lvi = new ListViewItem(Insp.Name);
                lvi.SubItems.Add(Insp.Priority.ToString());
                
                lvi.UseItemStyleForSubItems = false;
                ListViewItem.ListViewSubItem Itm = new ListViewItem.ListViewSubItem();
                Itm.BackColor = clsStaticTool.GetSystemColor(Insp.Color);
                Itm.ForeColor = Color.Black;
                Itm.Text = Insp.Color.ToString();
                lvi.SubItems.Add(Itm);

                listViewAlgorithm.Items.Add(lvi);
            }
            this.listViewAlgorithm.EndUpdate();
        }

        /// <summary>
        /// 取得該AOI演算法物件 (檢測參數Recipe)
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        public object ConvertParam(InductanceInsp.enuAlgorithm Type)
        {
            object Obj;
            switch (Type)
            {
                case InductanceInsp.enuAlgorithm.ThresholdInsp:
                    {
                        Obj = new clsRecipe.clsThresholdInsp();
                    }
                    break;
                case InductanceInsp.enuAlgorithm.LaserHoleInsp:
                    {
                        Obj = new clsRecipe.clsLaserHoleInsp();
                    }
                    break;
                case InductanceInsp.enuAlgorithm.TextureInsp:
                    {
                        Obj = new clsRecipe.clsTextureInsp();
                    }
                    break;
                case InductanceInsp.enuAlgorithm.DAVSInsp:
                    {
                        Obj = new clsRecipe.clsDAVSInsp();
                    }
                    break;
                case InductanceInsp.enuAlgorithm.LineInsp:
                    {
                        Obj = new clsRecipe.clsLineSearchInsp();
                    }
                    break;
                case InductanceInsp.enuAlgorithm.FeatureInsp:
                    {
                        Obj = new clsRecipe.clsFeatureInsp();
                    }
                    break;
                default:
                    Obj = new object();
                    MessageBox.Show("Error");
                    break;
            }
            return Obj;
        }

        /// <summary>
        /// 取得該AOI演算法類型之Form
        /// </summary>
        /// <param name="Type">AOI演算法類型</param>
        /// <param name="Display">顯示影像視窗</param>
        /// <param name="Algorithm"></param>
        /// <param name="UsingIndex"></param>
        /// <returns></returns>
        public Form GetAlgorithmForm(InductanceInsp.enuAlgorithm Type, HSmartWindowControl Display, clsRecipe.clsAlgorithm Algorithm,int UsingIndex) // (20200130) Jeff Revised!
        {
            Form Output = new Form();
            switch (Type)
            {
                case InductanceInsp.enuAlgorithm.ThresholdInsp:
                    {
                        FormThresholdInsp MyForm = new FormThresholdInsp(Input_ImgList, DisplayWindows, Recipe, Algorithm, ResultImages, ResultImages_UsedRegion); // (20200130) Jeff Revised!
                        MyForm.ChangeRegion += new FormThresholdInsp.ChangeRegionHandler(ExportThRegion);
                        MyForm.FormClosedEvent += new FormThresholdInsp.FormClosedHandler(SetFormClosed);
                        Output = MyForm;
                    }
                    break;
                case InductanceInsp.enuAlgorithm.LaserHoleInsp:
                    {
                        FormLaserHoleInsp MyForm = new FormLaserHoleInsp(Input_ImgList, DisplayWindows, Recipe, Algorithm, ResultImages, ResultImages_UsedRegion); // (20200130) Jeff Revised!
                        MyForm.ChangeRegion += new FormLaserHoleInsp.ChangeRegionHandler(SetSelectRegion);
                        MyForm.FormClosedEvent += new FormLaserHoleInsp.FormClosedHandler(SetFormClosed);
                        Output = MyForm;
                    }
                    break;
                case InductanceInsp.enuAlgorithm.TextureInsp:
                    {
                        FormTextureInsp MyForm = new FormTextureInsp(Input_ImgList, DisplayWindows, Recipe, Algorithm, Locate_Method_FS.pixel_resolution_, ResultImages, ResultImages_UsedRegion); // (20200130) Jeff Revised!
                        MyForm.FormClosedEvent += new FormTextureInsp.FormClosedHandler(SetFormClosed);
                        Output = MyForm;
                    }
                    break;
                case InductanceInsp.enuAlgorithm.DAVSInsp:
                    {
                        FormDAVSInsp MyForm = new FormDAVSInsp(Input_ImgList, Recipe, Algorithm, Locate_Method_FS.pixel_resolution_, Recipe.DAVSInspArray[UsingIndex], ResultImages, ResultImages_UsedRegion); // (20200130) Jeff Revised!
                        MyForm.FormClosedEvent += new FormDAVSInsp.FormClosedHandler(SetFormClosed);
                        Output = MyForm;
                    }
                    break;
                case InductanceInsp.enuAlgorithm.LineInsp:
                    {
                        FormLineInsp MyForm = new FormLineInsp(Input_ImgList, DisplayWindows, Recipe, Algorithm, ResultImages, ResultImages_UsedRegion); // (20200130) Jeff Revised!
                        MyForm.FormClosedEvent += new FormLineInsp.FormClosedHandler(SetFormClosed);
                        Output = MyForm;
                    }
                    break;
                case InductanceInsp.enuAlgorithm.FeatureInsp:
                    {
                        FormFeatureInsp MyForm = new FormFeatureInsp(Input_ImgList, DisplayWindows, Recipe, Algorithm, Locate_Method_FS.pixel_resolution_, ResultImages, ResultImages_UsedRegion); // (20200130) Jeff Revised!
                        MyForm.FormClosedEvent += new FormFeatureInsp.FormClosedHandler(SetFormClosed);
                        Output = MyForm;
                    }
                    break;
            }

            return Output;
        }

        HTuple Mean, Min, Ratio, Radius;

        public void SetFormClosed(bool pmbFormClosed)
        {
            this.bFormClosed = pmbFormClosed;
            this.Focus();
        }

        public void ExportThRegion(HObject ExportRegion)
        {
            if (SelectRegions != null)
            {
                SelectRegions.Dispose();
            }
            HOperatorSet.Connection(ExportRegion, out SelectRegions);
        }

        public void SetSelectRegion(HObject Region, HTuple pmMean, HTuple pmMin, HTuple pmRatio,HTuple pmRadius)
        {
            SelectRegions.Dispose();
            HOperatorSet.GenEmptyObj(out SelectRegions);
            this.SelectRegions = Region;
            Mean = pmMean;
            Min = pmMin;
            Ratio = pmRatio;
            Radius = pmRadius;
        }

        #endregion


        #region //=========================  Event ========================

        /// <summary>
        /// 【載入影像】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoadImg_Click(object sender, EventArgs e) // (20200319) Jeff Revised!
        {
            OpenFileDialog OpenImgDilg = new OpenFileDialog();
            //OpenImgDilg.Filter = "(*.tif)|*.tif|(*.bmp)|*.bmp|(*.jpg)|*.jpg|(*.tiff)|*.tiff";


            if (OpenImgDilg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;
            string path = OpenImgDilg.FileName;
            this.BeginInvoke(new Action(() =>
            {
                this.labImageFileName.Text = path;
            }));
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
            
            clsStaticTool.DisposeAll(Input_ImgList); // (20200319) Jeff Revised!
            Input_ImgList.Clear();
            try
            {
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

                if (Input_ImgList.Count <= 0) // 如果影像檔名不符合命名規則，則直接載入所選影像
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
                    HOperatorSet.DispObj(Input_ImgList[0], DisplayWindows.HalconWindow);
                    HOperatorSet.SetPart(DisplayWindows.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part
                }

                // (20200310) Jeff Revised!
                if (Input_ImgList != null && Input_ImgList.Count > 0)
                {
                    btn_Execute_Algo_Click(null, null); //【執行】
                    btn_Execute_Algo_UsedRegion_Click(null, null); //【執行】 (使用範圍列表)
                }
            }
            catch
            {
                MessageBox.Show("讀取檔案錯誤");
            }
        }

        private void button_Add_Click_1(object sender, EventArgs e)
        {
            OnUserControlButtonClicked(this, e);
        }

        /// <summary>
        /// 【圖像範圍】 (圖像教導)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSetPattern_CheckedChanged(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            CheckBox Obj = (CheckBox)sender;

            #region 確認是否可執行

            if (Input_ImgList == null)
            {
                cbxASetPattern.CheckedChanged -= cbxSetPattern_CheckedChanged;
                cbxASetPattern.Checked = false;
                cbxASetPattern.CheckedChanged += cbxSetPattern_CheckedChanged;
                MessageBox.Show("請先讀取影像");
                return;
            }

            if (Input_ImgList.Count <= 0)
            {
                cbxASetPattern.CheckedChanged -= cbxSetPattern_CheckedChanged;
                cbxASetPattern.Checked = false;
                cbxASetPattern.CheckedChanged += cbxSetPattern_CheckedChanged;
                MessageBox.Show("請先讀取影像");
                return;
            }

            /*
            if (Input_ImgList.Count <= int.Parse(nudInspectImgID.Value.ToString()))
            {
                cbxASetPattern.CheckedChanged -= cbxSetPattern_CheckedChanged;
                cbxASetPattern.Checked = false;
                cbxASetPattern.CheckedChanged += cbxSetPattern_CheckedChanged;
                MessageBox.Show("影像個數小於Index");
                return;
            }
            */

            HObject SrcImg_Match = Get_SourceImage_Match(); // (20200130) Jeff Revised!
            ClearDisplay(SrcImg_Match);
            ParamGetFromUI();
            #endregion

            if (Obj.Checked)
            {
                if (Recipe.DAVSParam.DAVS_Mode != 0)
                {
                    DialogResult dialogResult = MessageBox.Show("若重新設定對位可能會造成AI需要重新訓練,請確認是否繼續?", "警告!!!", MessageBoxButtons.YesNo);

                    if (dialogResult != DialogResult.Yes)
                    {
                        cbxASetPattern.CheckedChanged -= cbxSetPattern_CheckedChanged;
                        cbxASetPattern.Checked = false;
                        cbxASetPattern.CheckedChanged += cbxSetPattern_CheckedChanged;
                        return;
                    }
                }

                #region 設定範圍

                try
                {
                    clsStaticTool.EnableAllControls(this.tpPatternMatch, Obj, false);
                    ChangeColor(Color.Green, Obj);
                    if (drawing_Rect != null)
                    {
                        drawing_Rect.Dispose();
                        drawing_Rect = null;
                    }
                    HTuple ImgWidth, ImgHeight;
                    HOperatorSet.GetImageSize(SrcImg_Match, out ImgWidth, out ImgHeight);
                    if (Recipe.Param.SegParam.Patterncolumn1 != 0 && Recipe.Param.SegParam.Patterncolumn2 != 0 && Recipe.Param.SegParam.Patternrow1 != 0 && Recipe.Param.SegParam.Patternrow2 != 0)
                    {
                        drawing_Rect = HDrawingObject.CreateDrawingObject(HDrawingObject.HDrawingObjectType.RECTANGLE1, Recipe.Param.SegParam.Patternrow1, Recipe.Param.SegParam.Patterncolumn1, Recipe.Param.SegParam.Patternrow2, Recipe.Param.SegParam.Patterncolumn2);
                    }
                    else
                    {
                        drawing_Rect = HDrawingObject.CreateDrawingObject(HDrawingObject.HDrawingObjectType.RECTANGLE1,
                                                                         ImgHeight / 2 - 100,
                                                                         ImgWidth / 2 - 100,
                                                                         ImgHeight / 2 + 100,
                                                                         ImgWidth / 2 + 100);
                    }
                    //drawing_Rect = HDrawingObject.CreateDrawingObject(HDrawingObject.HDrawingObjectType.RECTANGLE2, 
                    //                                                  ImgHeight / 2,
                    //                                                  ImgWidth / 2,
                    //                                                  0, 100, 100);

                    drawing_Rect.SetDrawingObjectParams("color", "green");
                    DisplayWindows.HalconWindow.AttachDrawingObjectToWindow(drawing_Rect);
                }
                catch (Exception ex)
                {
                    cbxASetPattern.CheckedChanged -= cbxSetPattern_CheckedChanged;
                    cbxASetPattern.Checked = false;
                    cbxASetPattern.CheckedChanged += cbxSetPattern_CheckedChanged;
                    drawing_Rect.Dispose();
                    drawing_Rect = null;
                    clsStaticTool.EnableAllControls(this.tpPatternMatch, Obj, true);
                    ChangeColor(Color.LightGray, Obj);
                    MessageBox.Show(ex.ToString());
                }

                #endregion
            }
            else
            {
                #region 寫入設定

                try
                {
                    if (Recipe.Pattern_Rect != null)
                    {
                        Recipe.Pattern_Rect.Dispose();
                        Recipe.Pattern_Rect = null;
                    }

                    HTuple row1, column1, row2, column2;
                    GetRectData(drawing_Rect.ID, out column1, out row1, out column2, out row2);


                    Recipe.Param.SegParam.Patternrow1 = (int)(row1.D * Recipe.Param.AdvParam.MatchSacleSize);
                    Recipe.Param.SegParam.Patternrow2 = (int)(row2.D * Recipe.Param.AdvParam.MatchSacleSize);
                    Recipe.Param.SegParam.Patterncolumn1 = (int)(column1.D * Recipe.Param.AdvParam.MatchSacleSize);
                    Recipe.Param.SegParam.Patterncolumn2 = (int)(column2.D * Recipe.Param.AdvParam.MatchSacleSize);

                    HOperatorSet.GenRectangle1(out Recipe.Pattern_Rect, Recipe.Param.SegParam.Patternrow1, Recipe.Param.SegParam.Patterncolumn1, Recipe.Param.SegParam.Patternrow2, Recipe.Param.SegParam.Patterncolumn2);

                    drawing_Rect.Dispose();
                    drawing_Rect = null;
                    ChangeColor(Color.LightGray, Obj);
                    clsStaticTool.EnableAllControls(this.tpPatternMatch, Obj, true);
                }
                catch (Exception ex)
                {
                    cbxASetPattern.CheckedChanged -= cbxSetPattern_CheckedChanged;
                    cbxASetPattern.Checked = false;
                    cbxASetPattern.CheckedChanged += cbxSetPattern_CheckedChanged;
                    drawing_Rect.Dispose();
                    drawing_Rect = null;
                    clsStaticTool.EnableAllControls(this.tpPatternMatch, Obj, true);
                    ChangeColor(Color.LightGray, Obj);
                    MessageBox.Show(ex.ToString());
                }

                #endregion
            }
        }

        /// <summary>
        /// 【教導圖像】 (圖像教導)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTeachPattern_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            // 檢查是否已讀取影像
            if (!Check_Input_ImgList())
                return;
            
            if (Recipe.Param.SegParam.Patterncolumn1 == 0 && Recipe.Param.SegParam.Patterncolumn2 == 0 && Recipe.Param.SegParam.Patternrow1 == 0 && Recipe.Param.SegParam.Patternrow2 == 0)
            {
                MessageBox.Show("請先設定圖樣範圍");
                return;
            }

            if (Recipe.DAVSParam.DAVS_Mode != 0)
            {
                DialogResult dialogResult = MessageBox.Show("若重新設定對位可能會造成AI需要重新訓練,請確認是否繼續?", "警告!!!", MessageBoxButtons.YesNo);

                if (dialogResult != DialogResult.Yes)
                    return;
            }

            ParamGetFromUI();

            #region 變數宣告

            HObject SrcImg = null;
            HObject ho_TemplateImage;
            HObject TeachImg;

            HOperatorSet.GenEmptyObj(out TeachImg);

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_TemplateImage);

            #endregion

            HObject SrcImg_Match = Get_SourceImage_Match(); // (20200130) Jeff Revised!
            HOperatorSet.ZoomImageFactor(SrcImg_Match, out SrcImg, Recipe.Param.AdvParam.MatchSacleSize, Recipe.Param.AdvParam.MatchSacleSize, "nearest_neighbor");
         
            TeachImg = clsStaticTool.GetChannelImage(SrcImg, (enuBand)combxBand.SelectedIndex);

            ho_TemplateImage.Dispose();

            HObject InputImg = null;

            if (cbxThEnabled.Checked) // 二值化
            {
                if (!(PreProcess_PatternMatch(TeachImg, out InputImg))) // (20200130) Jeff Revised!
                {
                    MessageBox.Show("二值化異常,請檢查閥值");

                    #region Dispose

                    ho_TemplateImage.Dispose();
                    SrcImg.Dispose();
                    TeachImg.Dispose();

                    #endregion

                    return;
                }
            }
            else
                InputImg = TeachImg;

            try
            {
                HObject MaskImg;
                if (Recipe.Param.SegParam.TeachMask)
                    InductanceInsp.PreMatchMask(InputImg.Clone(), out MaskImg, Recipe.Param.SegParam.MaskThValueMax, Recipe.Param.SegParam.MaskThValueMin, Recipe);
                else
                    MaskImg = InputImg.Clone();

                HObject ReadImg;
                HOperatorSet.WriteImage(MaskImg, "bmp", 0, "MaskImg");
                HOperatorSet.ReadImage(out ReadImg, "MaskImg");
                HOperatorSet.ReduceDomain(ReadImg, Recipe.Pattern_Rect, out ho_TemplateImage);

                if (Recipe.ModelID != null && Recipe.ModelID.Length > 0 && !Recipe.Param.SegParam.IsNCCMode)
                {
                    HOperatorSet.ClearShapeModel(Recipe.ModelID);
                    Recipe.ModelID = null;
                }
                if (Recipe.ModelID_NCC != null && Recipe.ModelID_NCC.Length > 0 && Recipe.Param.SegParam.IsNCCMode)
                {
                    HOperatorSet.ClearNccModel(Recipe.ModelID_NCC);
                    Recipe.ModelID_NCC = null;
                }



                if (Recipe.Param.SegParam.IsNCCMode)
                {
                    HOperatorSet.CreateNccModel(ho_TemplateImage, Recipe.Param.AdvParam.NumLevels,
                                                (new HTuple(Recipe.Param.AdvParam.AngleStart)).TupleRad(),
                                                (new HTuple(Recipe.Param.AdvParam.AngleExtent)).TupleRad(),
                                                InductanceInsp.GetAngleStep(Recipe),
                                                InductanceInsp.GetMetric(Recipe),
                                                out Recipe.ModelID_NCC);
                }
                else
                {
                    HOperatorSet.CreateShapeModel(ho_TemplateImage, Recipe.Param.AdvParam.NumLevels,
                                                 (new HTuple(Recipe.Param.AdvParam.AngleStart)).TupleRad(),
                                                (new HTuple(Recipe.Param.AdvParam.AngleExtent)).TupleRad(),
                                                InductanceInsp.GetAngleStep(Recipe),
                                                InductanceInsp.GetOptimization(Recipe),
                                                InductanceInsp.GetMetric(Recipe),
                                                InductanceInsp.GetContrast(Recipe),
                                                InductanceInsp.GetMinContrast(Recipe),
                                                out Recipe.ModelID);
                }



                ho_TemplateImage.Dispose();
                HOperatorSet.ReduceDomain(MaskImg, Recipe.Pattern_Rect, out ho_TemplateImage);

                HObject PatternRegion;
                HTuple GAngle;
                HTuple Row, Col;
                Method.TeachGolden(ho_TemplateImage, Recipe, out PatternRegion, out GAngle, out Row, out Col);
                
                if (GAngle.Length <= 0)
                {
                    MaskImg.Dispose();
                    InputImg.Dispose();
                    ReadImg.Dispose();

                    #region Dispose
                    TeachImg.Dispose();
                    ho_TemplateImage.Dispose();
                    SrcImg.Dispose();
                    InputImg.Dispose();
                    if (File.Exists("MaskImg.bmp"))
                    {
                        File.Delete("MaskImg.bmp");
                    }
                    #endregion
                    MessageBox.Show("搜尋不到對位資訊");
                    return;
                }

                //Col /= Recipe.Param.AdvParam.MatchSacleSize;
                //Row /= Recipe.Param.AdvParam.MatchSacleSize;

                DisplayRegions(DisplayWindows.HalconWindow, SrcImg_Match, PatternRegion, true, "red", enuDisplayRegionType.margin);
                HTuple Area, CenterX, CenterY;
                HOperatorSet.AreaCenter(PatternRegion, out Area, out CenterY, out CenterX);
                
                Recipe.Param.SegParam.GoldenCenterX = CenterX;//region center
                Recipe.Param.SegParam.GoldenCenterY = CenterY;
                Recipe.Param.SegParam.GoldenAngle = GAngle;

                //Recipe.Param.SegParam.GoldenMatchCenterX = Col;//pattern Info
                //Recipe.Param.SegParam.GoldenMatchCenterY = Row;
                //Recipe.Param.SegParam.GoldenMatchAngle = GAngle;

                MaskImg.Dispose();
                InputImg.Dispose();
                ReadImg.Dispose();


                #region 新增List

                if (!Recipe.Param.SegParam.IsNCCMode)
                {
                    DialogResult dialogResult = MessageBox.Show("確認是否新增?", "提示!", MessageBoxButtons.YesNo);

                    if (dialogResult != DialogResult.Yes)
                    {
                        return;
                    }
                    string RegionName = "PatternRegion";
                    WorkSheetForm MyForm = new WorkSheetForm("Name", 1);

                    MyForm.ShowDialog();

                    if (!MyForm.Saved)
                        RegionName = "PatternRegion";
                    else
                        RegionName = MyForm.NewRecipeName;


                    bool IsMemberExists = Recipe.Param.MethodRegionList.Exists(x => x.Name == RegionName);

                    if (IsMemberExists)
                    {
                        MessageBox.Show("存在相同名稱,請重新命名");
                        return;
                    }

                    clsRecipe.clsMethodRegion MethodRegion = new clsRecipe.clsMethodRegion();
                    //MethodRegion.HotspotX = Col - CenterX;
                    //MethodRegion.HotspotY = Row - CenterY;

                    MethodRegion.HotspotX = 0;
                    MethodRegion.HotspotY = 0;
                    MethodRegion.Row = CenterY;
                    MethodRegion.Column = CenterX;
                    MethodRegion.Name = RegionName;
                    MethodRegion.RegionPath = Recipe.MethodRegionPath + RegionName;
                    MethodRegion.Region = PatternRegion;
                    Recipe.Param.MethodRegionList.Add(MethodRegion);
                    UpdateMethodListView(Recipe.Param.MethodRegionList);
                }
                #endregion

                MessageBox.Show("教導完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show("建立板模失敗");
                MessageBox.Show(ex.ToString());
            }
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }

            HTuple Width, Height;
            HOperatorSet.GetImageSize(SrcImg_Match, out Width, out Height);

            for (int i = 0; i < Input_ImgList.Count; i++)
            {
                HOperatorSet.WriteImage(Input_ImgList[i], "tiff", 0, Path + "GoldenImg" + i);
            }

            Recipe.Param.ImageWidth = Width;
            Recipe.Param.ImageHeight = Height;

            #region Dispose

            TeachImg.Dispose();
            ho_TemplateImage.Dispose();
            SrcImg.Dispose();
            InputImg.Dispose();
            if (File.Exists("MaskImg.bmp"))
            {
                File.Delete("MaskImg.bmp");
            }

            #endregion
            
        }

        /// <summary>
        /// (圖像教導→影像前處理)
        /// </summary>
        /// <param name="TeachImg">1 channel image</param>
        /// <param name="MatchTH"></param>
        /// <param name="Tag"></param>
        private bool PreProcess_PatternMatch(HObject TeachImg, out HObject BinaryImg, string Tag = "") // (20200130) Jeff Revised!
        {
            bool b_status_ = false;
            HOperatorSet.GenEmptyObj(out BinaryImg);

            HObject tmp = null;
            HObject Closing = null, Opening = null;
            HObject ProcessRegion = null;
            
            try
            {
                HTuple W, H;
                HOperatorSet.GetImageSize(TeachImg, out W, out H);
                HOperatorSet.Threshold(TeachImg, out tmp, Recipe.Param.SegParam.THMin, Recipe.Param.SegParam.ThMax);
                
                HOperatorSet.ClosingRectangle1(tmp, out Closing, Recipe.Param.AdvParam.ClosingNum, Recipe.Param.AdvParam.ClosingNum);
                HOperatorSet.OpeningRectangle1(Closing, out Opening, Recipe.Param.AdvParam.OpeningNum, Recipe.Param.AdvParam.OpeningNum);
                
                if (Recipe.Param.AdvParam.bFillupEnabled) // 填滿
                {
                    HTuple TransType = Recipe.Param.AdvParam.FillupType.ToString();
                    HOperatorSet.FillUpShape(Opening, out ProcessRegion, TransType, Recipe.Param.AdvParam.FillupMin, Recipe.Param.AdvParam.FillupMax);
                }
                else
                    ProcessRegion = Opening;

                HOperatorSet.RegionToBin(ProcessRegion, out BinaryImg, 255, 0, W, H);

                // 顯示
                if (Tag == "測試")
                    ClearDisplay(BinaryImg, false);
                else if (Tag == "二值化")
                    DisplayRegions(DisplayWindows.HalconWindow, TeachImg, tmp, true, "red", DisplayType);
                else if (Tag == "閉運算")
                    DisplayRegions(DisplayWindows.HalconWindow, TeachImg, Closing, true, "red", DisplayType);
                else if (Tag == "開運算")
                    DisplayRegions(DisplayWindows.HalconWindow, TeachImg, Opening, true, "red", DisplayType);
                else if (Tag == "填滿")
                    DisplayRegions(DisplayWindows.HalconWindow, TeachImg, ProcessRegion, true, "red", DisplayType);

                b_status_ = true;
            }
            catch
            {
                MessageBox.Show("二值化異常,請檢查閥值");
            }

            // Release
            Extension.HObjectMedthods.ReleaseHObject(ref ProcessRegion);
            Extension.HObjectMedthods.ReleaseHObject(ref Closing);
            Extension.HObjectMedthods.ReleaseHObject(ref Opening);
            Extension.HObjectMedthods.ReleaseHObject(ref tmp);

            return b_status_;
        }

        /// <summary>
        /// 【定位測試】 (圖像教導)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnATestMatch_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            // 檢查是否已讀取影像
            if (!Check_Input_ImgList())
                return;

            if (Recipe.ModelID == null && !Recipe.Param.SegParam.IsNCCMode)
            {
                MessageBox.Show("請先設定板模");
                return;
            }
            if (Recipe.ModelID_NCC == null && Recipe.Param.SegParam.IsNCCMode)
            {
                MessageBox.Show("請先設定板模");
                return;
            }

            /*
            if (int.Parse(nudInspectImgID.Value.ToString())>= Input_ImgList.Count)
            {
                MessageBox.Show("影像ID錯誤");
                return;
            }
            */

            combxDisplayImg.SelectedIndex = 0;
            ParamGetFromUI();

            HObject CellRegion;

            HOperatorSet.GenEmptyObj(out CellRegion);

            HObject SrcImg_Match = Get_SourceImage_Match(); // (20200130) Jeff Revised!
            if (SrcImg_Match != null)
            {
                ClearDisplay(SrcImg_Match);

                HTuple Angle, Row, Column;
                if (!InductanceInsp.PtternMatchAndSegRegion_New(SrcImg_Match, Recipe, out PatternRegions, out Angle,out Row,out Column, out CellRegion))
                {
                    MessageBox.Show("對位失敗");
                    return;
                }
            }
            ClearDisplay(SrcImg_Match);

            HTuple Count;
            HOperatorSet.CountObj(PatternRegions, out Count);

            labMatchCount.Text = Count.D.ToString();

            if (Count <= 0)
            {
                MessageBox.Show("未搜尋到此特徵,可能為分數、角度、亮暗差異過大");
                return;
            }

            HOperatorSet.SetLineWidth(DisplayWindows.HalconWindow, LineWidth);
            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, DisplayType.ToString());
            HOperatorSet.DispObj(PatternRegions, DisplayWindows.HalconWindow);
        }

        /// <summary>
        /// 【單顆範圍測試】 (圖像教導)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSegCellTest_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            // 檢查是否已讀取影像
            if (!this.Check_Input_ImgList())
                return;

            if (Recipe.Param.SegParam.HotspotX == 9999999 || Recipe.Param.SegParam.HotspotY == 9999999)
            {
                MessageBox.Show("請先完成教導步驟");
                return;
            }

            /*
            if (int.Parse(nudInspectImgID.Value.ToString()) >= Input_ImgList.Count)
            {
                MessageBox.Show("影像ID錯誤");
                return;
            }
            */

            combxDisplayImg.SelectedIndex = 0;
            ParamGetFromUI();
            HObject CellRegion;

            HOperatorSet.GenEmptyObj(out CellRegion);

            HObject SrcImg_Match = Get_SourceImage_Match(); // (20200130) Jeff Revised!
            if (SrcImg_Match != null)
            {
                ClearDisplay(SrcImg_Match);

                HTuple Angle, Row, Column;

                InductanceInsp.PtternMatchAndSegRegion_New(SrcImg_Match, Recipe, out PatternRegions, out Angle, out Row, out Column, out CellRegion);
            }
            ClearDisplay(SrcImg_Match);

            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, DisplayType.ToString());
            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "blue");
            HOperatorSet.DispObj(CellRegion, DisplayWindows.HalconWindow);

        }

        /// <summary>
        /// 【參數載入】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_LoadParam_Click(object sender, EventArgs e)
        {
            UpdateParameter();

            // (20200130) Jeff Revised!
            if (Input_ImgList != null && Input_ImgList.Count > 0)
            {
                btn_Execute_Algo_Click(null, null); //【執行】
                btn_Execute_Algo_UsedRegion_Click(null, null); //【執行】 (使用範圍列表)
            }
        }

        /// <summary>
        /// 【參數儲存】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_SaveParam_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            BtnLog.WriteLog("[Inspection UI] Save Param Click");
            DialogResult dialogResult = MessageBox.Show("是否儲存檔案?", "Yes / NO", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2, (MessageBoxOptions)0x40000);
            //dialogResult = MessageBox.Show("是否儲存檔案?", "Yes / NO", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2, MessageBoxOptions.ServiceNotification);

            if (dialogResult != DialogResult.Yes)
            {

                BtnLog.WriteLog("[Inspection UI] Save Param Cancel");
                return;
            }

            try
            {
                ParamGetFromUI();

                // Set parameter to method
                Method.set_parameter(Recipe);

                #region 紀錄修改參數

                ComparisonConfig config = new ComparisonConfig();
                config.CompareChildren = true;
                config.MaxDifferences = 999999;
                IntPtr A = new IntPtr();

                config.TypesToIgnore.Add(A.GetType());
                config.IgnoreObjectTypes = true;
                config.SkipInvalidIndexers = true;

                if (TmpParam != null) // (20200130) Jeff Revised!
                {
                    CompareLogic C = new CompareLogic(config);
                    var result = C.Compare(TmpParam, Recipe.Param);
                    if (result.Differences.Count > 0)
                    {
                        BtnLog.WriteLog(ModuleName + " : ======================== Param Fix =========================");
                        foreach (Difference D in result.Differences)
                        {
                            BtnLog.WriteLog(D.PropertyName + " : " + D.Object1Value + " = > " + D.Object2Value);
                        }
                        BtnLog.WriteLog("============================================================");
                    }
                }

                #endregion

                // Save parameter to XML file !!!! 一定要加
                Recipe.save();

                if (TmpParam != null) // (20200130) Jeff Revised!
                {
                    InductanceInspRole.SaveXML(TmpParam, ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\InductanceInsp\\BK_Param.xml");
                    TmpParam = clsStaticTool.Clone<clsRecipe>(Recipe.Param);
                }

                #region 自動同步Recipe

                if (Recipe.Param.bAutoSynchronizeRecipe)
                {
                    DialogResult AutoUpdateDilog = MessageBox.Show("自動同步功能已開啟,確認是否儲存&自動同步參數?", "Yes / NO", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (AutoUpdateDilog != DialogResult.Yes)
                    {
                        BtnLog.WriteLog("[Inspection UI] Save Param Cancel");
                        return;
                    }
                    BtnLog.WriteLog("[Inspection UI] Auto Update Recipe");
                    if (!SynchronizeRecipe(Recipe.Param.SynchronizePath))
                    {
                        MessageBox.Show("自動同步功能失敗,請確認路徑是否設定");
                        return;
                    }
                }

                #endregion

                BtnLog.WriteLog("[Inspection UI] Save Param Done");
                MessageBox.Show("儲存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
                this.ParentForm.Focus();
                //MessageBox.Show("儲存成功");
            }
            catch(Exception ex)
            {
                BtnLog.WriteLog("[Inspection UI] Save Param Fail");
                BtnLog.WriteLog("[Inspection UI] Error Message : " + ex.ToString());
                MessageBox.Show("參數儲存失敗", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
            }
        }

        /// <summary>
        /// 【單顆切割範圍】 (圖像教導)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxCellRegion_CheckedChanged(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            CheckBox Obj = (CheckBox)sender;

            #region 確認是否可執行
            if (Input_ImgList == null)
            {
                cbxCellRegion.CheckedChanged -= cbxCellRegion_CheckedChanged;
                cbxCellRegion.Checked = false;
                cbxCellRegion.CheckedChanged += cbxCellRegion_CheckedChanged;
                MessageBox.Show("請先讀取影像");
                return;
            }

            if (Input_ImgList.Count <= 0)
            {
                cbxCellRegion.CheckedChanged -= cbxCellRegion_CheckedChanged;
                cbxCellRegion.Checked = false;
                cbxCellRegion.CheckedChanged += cbxCellRegion_CheckedChanged;
                MessageBox.Show("請先讀取影像");
                return;
            }
            
            if (Recipe.Param.SegParam.GoldenCenterX == 9999999 || Recipe.Param.SegParam.GoldenCenterY == 9999999)
            {
                cbxCellRegion.CheckedChanged -= cbxCellRegion_CheckedChanged;
                cbxCellRegion.Checked = false;
                cbxCellRegion.CheckedChanged += cbxCellRegion_CheckedChanged;
                MessageBox.Show("請先教導圖樣");
                return;
            }

            #endregion

            if (Obj.Checked)
            {
                #region 設定範圍

                if (Recipe.DAVSParam.DAVS_Mode != 0)
                {
                    DialogResult dialogResult = MessageBox.Show("若重新設定對位可能會造成AI需要重新訓練,請確認是否繼續?", "警告!!!", MessageBoxButtons.YesNo);

                    if (dialogResult != DialogResult.Yes)
                    {
                        cbxCellRegion.CheckedChanged -= cbxCellRegion_CheckedChanged;
                        cbxCellRegion.Checked = false;
                        cbxCellRegion.CheckedChanged += cbxCellRegion_CheckedChanged;
                        return;
                    }
                }
                try
                {
                    clsStaticTool.EnableAllControls(this.tpPatternMatch, Obj, false);
                    ChangeColor(Color.Green, Obj);
                    if (drawing_Rect != null)
                    {
                        drawing_Rect.Dispose();
                        drawing_Rect = null;
                    }
                    if (Recipe.Param.SegParam.HotspotY != 9999999 && Recipe.Param.SegParam.HotspotX != 9999999)
                    {
                        drawing_Rect = HDrawingObject.CreateDrawingObject(HDrawingObject.HDrawingObjectType.RECTANGLE1,
                                                                      Recipe.Param.SegParam.GoldenCenterY - (Recipe.Param.SegParam.InspRectHeight / 2),
                                                                      Recipe.Param.SegParam.GoldenCenterX - (Recipe.Param.SegParam.InspRectWidth / 2),
                                                                      Recipe.Param.SegParam.GoldenCenterY + (Recipe.Param.SegParam.InspRectHeight / 2),
                                                                      Recipe.Param.SegParam.GoldenCenterX + (Recipe.Param.SegParam.InspRectWidth / 2));
                    }
                    else
                    {
                        drawing_Rect = HDrawingObject.CreateDrawingObject(HDrawingObject.HDrawingObjectType.RECTANGLE1,
                                                                      Recipe.Param.SegParam.GoldenCenterY - 100,
                                                                      Recipe.Param.SegParam.GoldenCenterX - 100,
                                                                      Recipe.Param.SegParam.GoldenCenterY + 100,
                                                                      Recipe.Param.SegParam.GoldenCenterX + 100);
                    }


                    drawing_Rect.SetDrawingObjectParams("color", "green");
                    DisplayWindows.HalconWindow.AttachDrawingObjectToWindow(drawing_Rect);
                }
                catch (Exception ex)
                {
                    cbxCellRegion.CheckedChanged -= cbxCellRegion_CheckedChanged;
                    cbxCellRegion.Checked = false;
                    cbxCellRegion.CheckedChanged += cbxCellRegion_CheckedChanged;
                    drawing_Rect.Dispose();
                    drawing_Rect = null;
                    clsStaticTool.EnableAllControls(this.tpPatternMatch, Obj, true);
                    ChangeColor(Color.LightGray, Obj);
                    MessageBox.Show(ex.ToString());
                }
                #endregion
            }
            else
            {
                #region 寫入設定
                try
                {
                    HObject Hotspot_Rect = null;
                    HTuple row1, column1, row2, column2;
                    GetRectData(drawing_Rect.ID, out column1, out row1, out column2, out row2);

                    HOperatorSet.GenRectangle1(out Hotspot_Rect, (int)row1.D, (int)column1.D, (int)row2.D, (int)column2.D);

                    HTuple A, C, R;
                    HOperatorSet.AreaCenter(Hotspot_Rect, out A, out R, out C);

                    double HotspotCenterX = (double)C.D;
                    double HotspotCenterY = (double)R.D;

                    //double HotspotCenterX = (double)(column1 + column2) / 2;
                    //double HotspotCenterY = (double)(row1 + row2) / 2;
                    //Recipe.Param.SegParam.InspRectWidth = (int)(column2.D - column1.D);
                    //Recipe.Param.SegParam.InspRectHeight = (int)(row2.D - row1.D);

                    Recipe.Param.SegParam.HotspotY = Recipe.Param.SegParam.GoldenCenterY - HotspotCenterY;
                    Recipe.Param.SegParam.HotspotX = Recipe.Param.SegParam.GoldenCenterX - HotspotCenterX;

                    HTuple Width, Height;
                    HOperatorSet.RegionFeatures(Hotspot_Rect, "width", out Width);
                    HOperatorSet.RegionFeatures(Hotspot_Rect, "height", out Height);

                    Recipe.Param.SegParam.InspRectWidth = (int)(Width.D);
                    Recipe.Param.SegParam.InspRectHeight = (int)(Height.D);

                    //Recipe.Param.SegParam.HotspotY = Recipe.Param.SegParam.GoldenMatchCenterY - HotspotCenterY * Recipe.Param.AdvParam.MatchSacleSize;
                    //Recipe.Param.SegParam.HotspotX = Recipe.Param.SegParam.GoldenMatchCenterX - HotspotCenterX * Recipe.Param.AdvParam.MatchSacleSize;

                    //Recipe.Param.SegParam.CellMatchHotspotY = Recipe.Param.SegParam.GoldenMatchCenterY - (HotspotCenterY * Recipe.Param.AdvParam.MatchSacleSize);
                    //Recipe.Param.SegParam.CellMatchHotspotX = Recipe.Param.SegParam.GoldenMatchCenterX - (HotspotCenterX * Recipe.Param.AdvParam.MatchSacleSize);

                    Recipe.Param.SegParam.CellColumn1 = column1;
                    Recipe.Param.SegParam.CellColumn2 = column2;
                    Recipe.Param.SegParam.CellRow1 = row1;
                    Recipe.Param.SegParam.CellRow2 = row2;

                    #region 新增List

                    //if (!Recipe.Param.SegParam.IsNCCMode)
                    {
                        DialogResult dialogResult = MessageBox.Show("確認是否新增?", "提示!", MessageBoxButtons.YesNo);

                        if (dialogResult != DialogResult.Yes)
                        {
                            cbxCellRegion.CheckedChanged -= cbxCellRegion_CheckedChanged;
                            cbxCellRegion.Checked = false;
                            cbxCellRegion.CheckedChanged += cbxCellRegion_CheckedChanged;
                            drawing_Rect.Dispose();
                            drawing_Rect = null;
                            clsStaticTool.EnableAllControls(this.tpPatternMatch, Obj, true);
                            ChangeColor(Color.LightGray, Obj);
                            return;
                        }
                        string RegionName = "CellRegion";
                        WorkSheetForm MyForm = new WorkSheetForm("Name", 1);

                        MyForm.ShowDialog();

                        if (!MyForm.Saved)
                            RegionName = "CellRegion";
                        else
                            RegionName = MyForm.NewRecipeName;
                        clsRecipe.clsMethodRegion MethodRegion = new clsRecipe.clsMethodRegion();
                        MethodRegion.HotspotX = Recipe.Param.SegParam.HotspotX;
                        MethodRegion.HotspotY = Recipe.Param.SegParam.HotspotY;

                        //MethodRegion.HotspotX = Recipe.Param.SegParam.GoldenMatchCenterX - HotspotCenterX;
                        //MethodRegion.HotspotY = Recipe.Param.SegParam.GoldenMatchCenterY - HotspotCenterY;
                        MethodRegion.Name = RegionName;
                        MethodRegion.RegionPath = Recipe.MethodRegionPath + RegionName;
                        MethodRegion.Region = Hotspot_Rect;
                        Recipe.Param.MethodRegionList.Add(MethodRegion);
                        UpdateMethodListView(Recipe.Param.MethodRegionList);
                    }
                    #endregion

                    drawing_Rect.Dispose();
                    drawing_Rect = null;
                    ChangeColor(Color.LightGray, Obj);
                    clsStaticTool.EnableAllControls(this.tpPatternMatch, Obj, true);
                }
                catch (Exception ex)
                {
                    cbxCellRegion.CheckedChanged -= cbxCellRegion_CheckedChanged;
                    cbxCellRegion.Checked = false;
                    cbxCellRegion.CheckedChanged += cbxCellRegion_CheckedChanged;
                    drawing_Rect.Dispose();
                    drawing_Rect = null;
                    clsStaticTool.EnableAllControls(this.tpPatternMatch, Obj, true);
                    ChangeColor(Color.LightGray, Obj);
                    MessageBox.Show(ex.ToString());
                }
                #endregion
            }
        }

        /// <summary>
        /// 【AI 參數設定】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCellCut_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            bool IsEmpty = false;

            ParamGetFromUI();

            HObject CellRegion, ThinRegion, RDefectRegion, ScratchRegion;

            HOperatorSet.GenEmptyObj(out CellRegion);
            HOperatorSet.GenEmptyObj(out ThinRegion);
            HOperatorSet.GenEmptyObj(out RDefectRegion);
            HOperatorSet.GenEmptyObj(out ScratchRegion);
            if (Input_ImgList.Count != 0)
            {
                HObject SrcImg_Match = Get_SourceImage_Match(); // (20200130) Jeff Revised!
                if (SrcImg_Match != null)
                {

                    ClearDisplay(SrcImg_Match);

                    HTuple Angle;
                   
                    Method.PtternMatchAndSegRegion(SrcImg_Match, Recipe, out PatternRegions, out Angle, out CellRegion, out ThinRegion, out RDefectRegion, out ScratchRegion);
                }
                ClearDisplay(SrcImg_Match);
            }

            if (Input_ImgList.Count <= 0)
            {
                IsEmpty = true;
                HObject H = new HObject();
                HOperatorSet.GenEmptyObj(out H);
                Input_ImgList.Add(H);
            }
            if (Recipe.DAVS == null)
            {
                Recipe.DAVS = new clsDAVS(Recipe.DAVSParam, Recipe.GetRecipePath());
            }

            HObject SaveImage;
            if (!Recipe.Param.bDAVSMixImgBand)
                SaveImage = Input_ImgList[Recipe.Param.DAVSImgID];
            else
                SaveImage = clsStaticTool.MixImageBand(Input_ImgList, Recipe.Param.DAVSBand1, Recipe.Param.DAVSBand2, Recipe.Param.DAVSBand3, Recipe.Param.DAVSBand1ImgIndex, Recipe.Param.DAVSBand2ImgIndex, Recipe.Param.DAVSBand3ImgIndex);


            FormDAVS MyForm = new FormDAVS(SaveImage, Recipe.DAVSParam, Recipe.DAVS, Recipe.GetRecipePath(), CellRegion, ModuleName, SB_ID, MOVE_ID, PartNumber);
            MyForm.ShowDialog();

            if (MyForm.DialogResult == DialogResult.OK)
            {
                Recipe.DAVSParam = MyForm.GetParam();
            }

            #region Dispose

            if (IsEmpty)
            {
                Input_ImgList[0].Dispose();
                Input_ImgList[0] = null;
                Input_ImgList.Clear();
            }

            #endregion
        }

        /// <summary>
        /// 【檢測】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Inspection_Click(object sender, EventArgs e) // (20200409) Jeff Revised!
        {
            // 檢查是否已讀取影像
            if (!Check_Input_ImgList())
                return;

            button_Inspection.Enabled = false;
            //combxDisplayImg.SelectedIndex = 0; // (20200409) Jeff Revised!
            ParamGetFromUI();

            ClearDisplay(Input_ImgList[combxDisplayImg.SelectedIndex]); // (20200409) Jeff Revised!

            Method.set_parameter(Recipe);

            #region Insp AI & AOI

            if (Recipe.DAVS == null)
                Recipe.DAVS = new clsDAVS(Recipe.DAVSParam, Recipe.GetRecipePath());

            Recipe.DAVS.SetPathParam(ModuleName, SB_ID, MOVE_ID, PartNumber);
            List<Defect> ResList;
            HObject ResRegion;

            try
            {
                List<int> IndexList;
                if (!Method.executeNew(Recipe, Input_ImgList, true, out ResRegion, out ResList, out IndexList))
                {
                    //button_Inspection.Enabled = true; // (20200319) Jeff Revised!
                    MessageBox.Show("檢測失敗", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // 一樣會執行 finally {} 內程式
                }

                #region 修改演算法顏色

                for (int i = 0; i < Recipe.Param.AlgorithmList.Count; i++)
                {
                    listViewAlgorithm.Items[i].ForeColor = Color.Black;
                }
                foreach (int Index in IndexList)
                {
                    listViewAlgorithm.Items[Index].ForeColor = Color.Red;
                }

                #endregion

                Method.Convert2Circle(ResRegion, out ResRegion);

                HOperatorSet.SetDraw(DisplayWindows.HalconWindow, DisplayType.ToString());
                HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                HOperatorSet.DispObj(ResRegion, DisplayWindows.HalconWindow);

                // 顯示瑕疵(NG)顆數
                HTuple DefectCount;
                HOperatorSet.CountObj(ResRegion, out DefectCount);
                labDefectCount.Text = DefectCount.D.ToString();
                // 顯示Cell顆數 (20200319) Jeff Revised!
                HOperatorSet.CountObj(Method.PatternRegion, out DefectCount);
                labMatchCount.Text = DefectCount.D.ToString();

                #region 顯示Defect演算法

                HObject RealDefectRegion;
                HOperatorSet.GenEmptyObj(out RealDefectRegion);

                for (int i = 0; i < ResList.Count; i++)
                {
                    if (!ResList[i].B_defect)
                        continue;

                    HOperatorSet.Union2(RealDefectRegion, ResList[i].DefectRegion, out RealDefectRegion);
                }

                HOperatorSet.SetDraw(DisplayWindows.HalconWindow, DisplayType.ToString());
                HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                HOperatorSet.DispObj(RealDefectRegion, DisplayWindows.HalconWindow);

                DrawImg(RealDefectRegion, null, combxDisplayImg.SelectedIndex); // (20200409) Jeff Revised!

                RealDefectRegion.Dispose();

                #endregion

                clsStaticTool.DisposeAll(ResList);
                ResRegion.Dispose(); // (20200409) Jeff Revised!
            }
            catch (Exception ex)
            {
                string message = "Error: " + ex.Message;
                MessageBox.Show(message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally // (20200319) Jeff Revised!
            {
                button_Inspection.Enabled = true;
            }

            #endregion
        }

        /// <summary>
        /// 【二值化】 (圖像教導→影像前處理)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxThEnabled_CheckedChanged(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            clsStaticTool.EnableAllControls(this.gbx_PatternMatch_PreProcess, cbxThEnabled, cbxThEnabled.Checked);
        }

        /// <summary>
        /// 【二值化設定】 (圖像教導→影像前處理)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnThSetup_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            // 檢查是否已讀取影像
            if (!Check_Input_ImgList())
                return;

            ParamGetFromUI(); // (20200130) Jeff Revised!

            HObject SrcImg_Match = Get_SourceImage_Match(); // (20200130) Jeff Revised!
            HObject ZoomImg, TeachImg;
            HOperatorSet.ZoomImageFactor(SrcImg_Match, out ZoomImg, Recipe.Param.AdvParam.MatchSacleSize, Recipe.Param.AdvParam.MatchSacleSize, "nearest_neighbor");

            TeachImg = clsStaticTool.GetChannelImage(ZoomImg, (enuBand)combxBand.SelectedIndex);
            
            FormThresholdAdjust MyForm = new FormThresholdAdjust(TeachImg,
                                                                 Recipe.Param.SegParam.THMin,
                                                                 Recipe.Param.SegParam.ThMax, FormThresholdAdjust.enType.Dual);

            MyForm.ShowDialog();

            if (MyForm.DialogResult == DialogResult.OK)
            {
                int ThMin, ThMax;
                MyForm.GetThreshold(out ThMin, out ThMax);

                Recipe.Param.SegParam.ThMax = ThMax;
                Recipe.Param.SegParam.THMin = ThMin;

                // 顯示
                HObject InputImg;
                PreProcess_PatternMatch(TeachImg, out InputImg, "二值化");
                InputImg.Dispose();
            }
            
            ZoomImg.Dispose();
            TeachImg.Dispose();
        }

        /// <summary>
        /// 控制項參數變更 (圖像教導→影像前處理)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PatternMatch_PreProcess_ValueChanged(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            if (b_LoadRecipe)
                return;

            // 檢查是否已讀取影像
            if (!Check_Input_ImgList())
                return;

            ParamGetFromUI();

            /* 計算 & 顯示 */
            HObject SrcImg_Match = Get_SourceImage_Match();
            HObject ZoomImg, TeachImg;
            HOperatorSet.ZoomImageFactor(SrcImg_Match, out ZoomImg, Recipe.Param.AdvParam.MatchSacleSize, Recipe.Param.AdvParam.MatchSacleSize, "nearest_neighbor");

            TeachImg = clsStaticTool.GetChannelImage(ZoomImg, (enuBand)combxBand.SelectedIndex);

            HObject InputImg;
            PreProcess_PatternMatch(TeachImg, out InputImg, (sender as Control).Tag.ToString());

            InputImg.Dispose();
            ZoomImg.Dispose();
            TeachImg.Dispose();
        }
        
        private void btnInnerThSetup_Click(object sender, EventArgs e)
        {
            // 檢查是否已讀取影像
            if (!Check_Input_ImgList())
                return;

            HObject SrcImg_R, SrcImg_G, SrcImg_B;

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out SrcImg_R);
            HOperatorSet.GenEmptyObj(out SrcImg_G);
            HOperatorSet.GenEmptyObj(out SrcImg_B);

            HObject SrcImg;
            HObject InspImage;

            HOperatorSet.CopyImage(Input_ImgList[int.Parse(txbInnerImgIndex.Text)], out SrcImg);

            HTuple Ch;
            HOperatorSet.CountChannels(SrcImg, out Ch);

            if (Ch == 1)
                InspImage = SrcImg.Clone();
            else
            {
                HOperatorSet.Decompose3(SrcImg, out SrcImg_R, out SrcImg_G, out SrcImg_B);
                if (comboBoxInnerBand.SelectedIndex == 0)
                {
                    InspImage = SrcImg_R;
                }
                else if (comboBoxInnerBand.SelectedIndex == 1)
                {
                    InspImage = SrcImg_G;
                }
                else if (comboBoxInnerBand.SelectedIndex == 2)
                {
                    InspImage = SrcImg_B;
                }
                else
                {
                    HObject GrayImg;
                    HOperatorSet.Rgb1ToGray(SrcImg, out GrayImg);
                    InspImage = GrayImg.Clone();
                    GrayImg.Dispose();
                }
            }

            FormThresholdAdjust MyForm = new FormThresholdAdjust(InspImage,
                                                                 int.Parse(nudInnerLTh.Value.ToString()),
                                                                 int.Parse(nudInnerHTH.Value.ToString()), FormThresholdAdjust.enType.Dual);
            MyForm.ShowDialog();

            if (MyForm.DialogResult == DialogResult.OK)
            {
                int ThMin, ThMax;
                MyForm.GetThreshold(out ThMin, out ThMax);

                nudInnerHTH.Value = ThMax;
                nudInnerLTh.Value = ThMin;
            }

            SrcImg.Dispose();
            SrcImg_R.Dispose();
            SrcImg_G.Dispose();
            SrcImg_B.Dispose();
            InspImage.Dispose();
        }
        
        private void btnInnerInsp_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            ParamGetFromUI();
            if (!Recipe.Param.InnerInspParam.Enabled)
            {
                MessageBox.Show("未啟用");
                return;
            }

            // 檢查是否已讀取影像
            if (!Check_Input_ImgList())
                return;

            if (int.Parse(txbInnerImgIndex.Text)>= Input_ImgList.Count)
            {
                MessageBox.Show("影像ID錯誤");
                return;
            }

            HObject DefectRegions, MapRegions;

            combxDisplayImg.SelectedIndex = 0;

            HObject CellRegion, ThinRegion, RDefectRegion, ScratchRegion;

            HOperatorSet.GenEmptyObj(out CellRegion);
            HOperatorSet.GenEmptyObj(out ThinRegion);
            HOperatorSet.GenEmptyObj(out RDefectRegion);
            HOperatorSet.GenEmptyObj(out ScratchRegion);

            HObject SrcImg_Match = Get_SourceImage_Match(); // (20200130) Jeff Revised!
            if (SrcImg_Match != null)
            {
                ClearDisplay(SrcImg_Match);

                HTuple Angle;
               
                Method.PtternMatchAndSegRegion(SrcImg_Match, Recipe, out PatternRegions, out Angle, out CellRegion, out ThinRegion, out RDefectRegion, out ScratchRegion);
            }

            InductanceInsp.InnerInspParam InnerParam = new InductanceInsp.InnerInspParam(Input_ImgList[Recipe.Param.InnerInspParam.ImageIndex], Input_ImgList, PatternRegions, CellRegion, Recipe);
            var InnerTask = new Task<HObject>(InductanceInsp.InnerInspTask, InnerParam);
            InnerTask.Start();


            DefectRegions = InnerTask.Result;
            Method.LocalDefectMappping(DefectRegions, CellRegion, out MapRegions);
            InnerTask.Dispose();

            ClearDisplay(Input_ImgList[int.Parse(txbInnerImgIndex.Text)]);

            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
            HOperatorSet.DispObj(DefectRegions, DisplayWindows.HalconWindow);

            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "green");
            HOperatorSet.DispObj(MapRegions, DisplayWindows.HalconWindow);

            SelectRegions.Dispose();

            HOperatorSet.Connection(DefectRegions, out SelectRegions);

            DrawImg(DefectRegions, MapRegions, int.Parse(txbInnerImgIndex.Text));
        }

        private void btnOuterInsp_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            ParamGetFromUI();
            if (!Recipe.Param.OuterInspParam.Enabled)
            {
                MessageBox.Show("未啟用");
                return;
            }

            // 檢查是否已讀取影像
            if (!Check_Input_ImgList())
                return;

            if (int.Parse(txbOuterImgIndex.Text) >= Input_ImgList.Count)
            {
                MessageBox.Show("影像ID錯誤");
                return;
            }

            combxDisplayImg.SelectedIndex = 0;
            HObject DefectRegions, MapRegions;

            HObject CellRegion, ThinRegion, RDefectRegion, ScratchRegion;

            HOperatorSet.GenEmptyObj(out CellRegion);
            HOperatorSet.GenEmptyObj(out ThinRegion);
            HOperatorSet.GenEmptyObj(out RDefectRegion);
            HOperatorSet.GenEmptyObj(out ScratchRegion);

            HObject SrcImg_Match = Get_SourceImage_Match(); // (20200130) Jeff Revised!
            if (SrcImg_Match != null)
            {
                ClearDisplay(SrcImg_Match);

                HTuple Angle;
             
                Method.PtternMatchAndSegRegion(SrcImg_Match, Recipe, out PatternRegions, out Angle, out CellRegion, out ThinRegion, out RDefectRegion, out ScratchRegion);
            }

            InductanceInsp.OuterInspParam OuterParam = new InductanceInsp.OuterInspParam(Input_ImgList[Recipe.Param.OuterInspParam.ImageIndex], Input_ImgList, PatternRegions, CellRegion, Recipe);
            var OuterTask = new Task<HObject>(InductanceInsp.OuterInspTask, OuterParam);
            OuterTask.Start();

            DefectRegions = OuterTask.Result;
            Method.LocalDefectMappping(DefectRegions, CellRegion, out MapRegions);

            OuterTask.Dispose();

            ClearDisplay(Input_ImgList[int.Parse(txbOuterImgIndex.Text)]);

            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
            HOperatorSet.DispObj(DefectRegions, DisplayWindows.HalconWindow);

            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
            HOperatorSet.DispObj(MapRegions, DisplayWindows.HalconWindow);

            SelectRegions.Dispose();

            HOperatorSet.Connection(DefectRegions, out SelectRegions);

            DrawImg(DefectRegions, MapRegions, int.Parse(txbOuterImgIndex.Text));
        }

        private void btnOuterThSetup_Click(object sender, EventArgs e)
        {
            // 檢查是否已讀取影像
            if (!Check_Input_ImgList())
                return;

            HObject SrcImg_R, SrcImg_G, SrcImg_B;

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out SrcImg_R);
            HOperatorSet.GenEmptyObj(out SrcImg_G);
            HOperatorSet.GenEmptyObj(out SrcImg_B);

            HObject SrcImg, InspImage;

            HOperatorSet.CopyImage(Input_ImgList[int.Parse(txbOuterImgIndex.Text)], out SrcImg);

            HTuple Ch;
            HOperatorSet.CountChannels(SrcImg, out Ch);
            if (Ch == 1)
                InspImage = SrcImg.Clone();
            else
            {
                HOperatorSet.Decompose3(SrcImg, out SrcImg_R, out SrcImg_G, out SrcImg_B);
                if (comboBoxOuterBand.SelectedIndex == 0)
                {
                    InspImage = SrcImg_R;
                }
                else if (comboBoxOuterBand.SelectedIndex == 1)
                {
                    InspImage = SrcImg_G;
                }
                else if (comboBoxOuterBand.SelectedIndex == 2)
                {
                    InspImage = SrcImg_B;
                }
                else
                {
                    HObject GrayImg;
                    HOperatorSet.Rgb1ToGray(SrcImg, out GrayImg);
                    InspImage = GrayImg.Clone();
                    GrayImg.Dispose();
                }
            }

            FormThresholdAdjust MyForm = new FormThresholdAdjust(InspImage,
                                                                 int.Parse(nudOuterLTh.Value.ToString()),
                                                                 int.Parse(nudOuterHTH.Value.ToString()), FormThresholdAdjust.enType.Dual);
            MyForm.ShowDialog();

            if (MyForm.DialogResult == DialogResult.OK)
            {
                int ThMin, ThMax;
                MyForm.GetThreshold(out ThMin, out ThMax);

                nudOuterHTH.Value = ThMax;
                nudOuterLTh.Value = ThMin;
            }

            SrcImg.Dispose();
            SrcImg_R.Dispose();
            SrcImg_G.Dispose();
            SrcImg_B.Dispose();
        }

        private void combxAIType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox Obj = (ComboBox)sender;
            ParamGetFromUI();
            if (Obj.SelectedIndex == 1 || Obj.SelectedIndex == 2)
            {
                bool bAOIEnabled = false;
                for (int i = 0; i < Recipe.Param.AlgorithmList.Count; i++)
                {
                    if (Recipe.Param.AlgorithmList[i].bUsedDAVS == true)
                        bAOIEnabled = true;
                }

                if (!bAOIEnabled)
                {
                    MessageBox.Show("AOI未啟用DAVS檢測");
                    Obj.SelectedIndex = 0;
                    return;
                }
            }
        }

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

        private void btnInnerMultiTh_Click(object sender, EventArgs e)
        {
            // 檢查是否已讀取影像
            if (!Check_Input_ImgList())
                return;

            HObject SrcImg_R = null, SrcImg_G = null, SrcImg_B = null;
            HObject SrcImg = null;
            HObject InspImage;
            HOperatorSet.GenEmptyObj(out InspImage);
            if (Input_ImgList[int.Parse(txbInnerImgIndex.Text)] != null)
            {
                HOperatorSet.CopyImage(Input_ImgList[int.Parse(txbInnerImgIndex.Text)], out SrcImg);
                HTuple Ch;
                HOperatorSet.CountChannels(SrcImg, out Ch);

                if (Ch == 1)
                    InspImage = SrcImg.Clone();
                else
                {
                    HOperatorSet.Decompose3(SrcImg, out SrcImg_R, out SrcImg_G, out SrcImg_B);
                    if (combxBand.SelectedIndex == 0)
                    {
                        InspImage = SrcImg_R;
                    }
                    else if (combxBand.SelectedIndex == 1)
                    {
                        InspImage = SrcImg_G;
                    }
                    else if (combxBand.SelectedIndex == 2)
                    {
                        InspImage = SrcImg_B;
                    }
                    else
                    {
                        HObject GrayImg;
                        HOperatorSet.Rgb1ToGray(SrcImg, out GrayImg);
                        InspImage = GrayImg.Clone();
                        GrayImg.Dispose();
                    }
                }
            }
            FormMultiThreshold MyForm = new FormMultiThreshold(InspImage, Recipe.Param.InnerInspParam, Input_ImgList, Recipe);

            MyForm.ShowDialog();

            if (MyForm.DialogResult == DialogResult.OK)
            {
                clsRecipe.clsAOIParam Temp = clsStaticTool.Clone<clsRecipe.clsAOIParam>(MyForm.GetList());

                Recipe.Param.InnerInspParam.MultiTHList = Temp.MultiTHList;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 檢查是否已讀取影像
            if (!Check_Input_ImgList())
                return;

            HObject InspImage;
            HObject SrcImg_R = null, SrcImg_G = null, SrcImg_B = null;
            HObject SrcImg = null;
            HOperatorSet.GenEmptyObj(out InspImage);
            if (Input_ImgList[int.Parse(txbOuterImgIndex.Text)] != null)
            {
                HOperatorSet.CopyImage(Input_ImgList[int.Parse(txbOuterImgIndex.Text)], out SrcImg);
                HTuple Ch;
                HOperatorSet.CountChannels(SrcImg, out Ch);
                if (Ch == 1)
                    InspImage = SrcImg.Clone();
                else
                {
                    HOperatorSet.Decompose3(SrcImg, out SrcImg_R, out SrcImg_G, out SrcImg_B);
                    if (combxBand.SelectedIndex == 0)
                    {
                        InspImage = SrcImg_R;
                    }
                    else if (combxBand.SelectedIndex == 1)
                    {
                        InspImage = SrcImg_G;
                    }
                    else if (combxBand.SelectedIndex == 2)
                    {
                        InspImage = SrcImg_B;
                    }
                    else
                    {
                        HObject GrayImg;
                        HOperatorSet.Rgb1ToGray(SrcImg, out GrayImg);
                        InspImage = GrayImg.Clone();
                        GrayImg.Dispose();
                    }
                }
            }
            FormMultiThreshold MyForm = new FormMultiThreshold(InspImage, Recipe.Param.OuterInspParam, Input_ImgList, Recipe);

            MyForm.ShowDialog();

            if (MyForm.DialogResult == DialogResult.OK)
            {
                clsRecipe.clsAOIParam Temp = clsStaticTool.Clone<clsRecipe.clsAOIParam>(MyForm.GetList());

                Recipe.Param.OuterInspParam.MultiTHList = Temp.MultiTHList;
            }
        }

        private void btnAddImg_Click(object sender, EventArgs e)
        {

        }

        private void DisplayWindows_HMouseDown(object sender, HMouseEventArgs e)
        {
            #region 切換Display Type

            if (e.Button == MouseButtons.Right)
            {
                if (DisplayType == enuDisplayRegionType.fill)
                    DisplayType = enuDisplayRegionType.margin;
                else
                    DisplayType = enuDisplayRegionType.fill;

                comboBoxDisplayType.SelectedIndex = (int)DisplayType;
                return;
            }

            #endregion

            #region  顯示特徵資訊
            HTuple Area, Width, Height;

            if (SelectRegions != null)
            {
                HTuple RegionCount;

                HOperatorSet.Connection(SelectRegions, out SelectRegions);

                HOperatorSet.CountObj(SelectRegions, out RegionCount);
                if (RegionCount > 0)
                {
                    HOperatorSet.GetRegionIndex(SelectRegions, (int)(e.Y + 0.5), (int)(e.X + 0.5), out hv_Index); // (20200729) Jeff Revised!
                    if (hv_Index.Length > 0)
                    {
                        HOperatorSet.SetDraw(DisplayWindows.HalconWindow, DisplayType.ToString());
                        HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");

                        int ImgIndex = Recipe.Param.AlgorithmList[listViewAlgorithm.SelectedIndices[0]].InspImageIndex;
                        HObject Image = clsStaticTool.GetChannelImage(Input_ImgList[ImgIndex], Recipe.Param.AlgorithmList[listViewAlgorithm.SelectedIndices[0]].Band);
                        HOperatorSet.DispObj(Image, DisplayWindows.HalconWindow);
                        HOperatorSet.DispObj(SelectRegions, DisplayWindows.HalconWindow);
                        HTuple Avg, Dev;
                        HOperatorSet.Intensity(SelectRegions[hv_Index], Image, out Avg, out Dev);
                        HTuple GrayMin, GrayMax;
                        HOperatorSet.GrayFeatures(SelectRegions[hv_Index], Image, "min", out GrayMin);
                        HOperatorSet.GrayFeatures(SelectRegions[hv_Index], Image, "max", out GrayMax);

                        HObject SecImage = clsStaticTool.GetChannelImage(Input_ImgList[Recipe.Param.AlgorithmList[listViewAlgorithm.SelectedIndices[0]].SecondSrcImgID], Recipe.Param.AlgorithmList[listViewAlgorithm.SelectedIndices[0]].SecondImgBand);
                        HTuple SecGrayMin, SecGrayMax,SecGrayMean;
                        HOperatorSet.GrayFeatures(SelectRegions[hv_Index], SecImage, "min", out SecGrayMin);
                        HOperatorSet.GrayFeatures(SelectRegions[hv_Index], SecImage, "max", out SecGrayMax);
                        HOperatorSet.GrayFeatures(SelectRegions[hv_Index], SecImage, "mean", out SecGrayMean);

                        HOperatorSet.RegionFeatures(SelectRegions[hv_Index], "area", out Area);
                        HOperatorSet.RegionFeatures(SelectRegions[hv_Index], "width", out Width);
                        HOperatorSet.RegionFeatures(SelectRegions[hv_Index], "height", out Height);
                        double A = Area[0] * (Locate_Method_FS.pixel_resolution_ * Locate_Method_FS.pixel_resolution_);
                        double W = (Width[0] * Locate_Method_FS.pixel_resolution_);
                        double H = (Height[0] * Locate_Method_FS.pixel_resolution_);

                        labW.Text = W.ToString("#0.000");
                        labH.Text = H.ToString("#0.000");
                        labA.Text = A.ToString("#0.000");
                        labGrayMean.Text = Avg.D.ToString("#0.00");
                        labInfoGrayMin.Text = GrayMin.D.ToString();
                        labInfoGrayMax.Text = GrayMax.D.ToString();
                        labID0.Text = ImgIndex.ToString();
                        labID1.Text = Recipe.Param.AlgorithmList[listViewAlgorithm.SelectedIndices[0]].SecondSrcImgID.ToString();

                        labSecGray.Text = SecGrayMean.D.ToString("#0.00");
                        labSecGrayMin.Text = SecGrayMin.D.ToString();
                        labSecGrayMax.Text = SecGrayMax.D.ToString();

                        HTuple convexity, rectangularity, holes_num, circularity, roundness, inner_radius, Anisometry;
                        HOperatorSet.RegionFeatures(SelectRegions[hv_Index], "convexity", out convexity);
                        HOperatorSet.RegionFeatures(SelectRegions[hv_Index], "rectangularity", out rectangularity);
                        HOperatorSet.RegionFeatures(SelectRegions[hv_Index], "holes_num", out holes_num);
                        HOperatorSet.RegionFeatures(SelectRegions[hv_Index], "circularity", out circularity);
                        HOperatorSet.RegionFeatures(SelectRegions[hv_Index], "roundness", out roundness);
                        HOperatorSet.RegionFeatures(SelectRegions[hv_Index], "inner_radius", out inner_radius);
                        HOperatorSet.RegionFeatures(SelectRegions[hv_Index], "anisometry", out Anisometry);

                        labconvexity.Text = convexity.D.ToString("#0.000");
                        labrectangularity.Text = rectangularity.D.ToString("#0.000");
                        labholes_num.Text = holes_num.D.ToString("#0.000");
                        labcircularity.Text = circularity.D.ToString("#0.000");
                        labroundness.Text = roundness.D.ToString("#0.000");
                        labinner_radius.Text = inner_radius.D.ToString("#0.000");
                        label94.Text = Anisometry.D.ToString("#0.000");

                        if (hv_Index <= Radius.Length)
                            labRadiusValue.Text = Radius[hv_Index - 1].D.ToString("#0.000");
                        else
                            labRadiusValue.Text = "0";
                        if (hv_Index <= Mean.Length)
                            labMeanValue.Text = Mean[hv_Index - 1].D.ToString("#0.000");
                        else
                            labMeanValue.Text = "0";
                        if (hv_Index <= Min.Length)
                            labGrayMinValue.Text = Min[hv_Index - 1].D.ToString();
                        else
                            labGrayMinValue.Text = "0";
                        if (hv_Index <= Ratio.Length)
                            labDarkRatioValue.Text = Ratio[hv_Index - 1].D.ToString("#0.000");
                        else
                            labDarkRatioValue.Text = "0";

                        HOperatorSet.SetDraw(DisplayWindows.HalconWindow, DisplayType.ToString());
                        HOperatorSet.SetColor(DisplayWindows.HalconWindow, "blue");
                        HOperatorSet.DispObj(SelectRegions[hv_Index], DisplayWindows.HalconWindow);

                        Image.Dispose();
                        SecImage.Dispose();
                    }
                }
            }
            #endregion

        }

        /// <summary>
        /// 來源影像切換 (圖像教導)，調整【影像ID:】控制項上限
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_Img_Match_CheckedChanged(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            RadioButton rbtn = sender as RadioButton;
            if (rbtn.Checked == false) return;

            /*
            // 檢查是否已讀取影像
            if (!Check_Input_ImgList())
            {
                this.radioButton_OrigImg_Match.CheckedChanged -= new System.EventHandler(this.radioButton_Img_Match_CheckedChanged);
                radioButton_OrigImg_Match.Checked = true;
                this.radioButton_OrigImg_Match.CheckedChanged += new System.EventHandler(this.radioButton_Img_Match_CheckedChanged);
                return;
            }
            */

            // 調整【影像ID:】控制項上限
            string Tag = rbtn.Tag.ToString();
            if (Tag == enu_ImageSource.原始.ToString())
                nudInspectImgID.Maximum = Recipe.Param.ImgCount - 1;
            else if (Tag == enu_ImageSource.影像A.ToString())
            {
                if (Recipe.Param.AlgoImg.ListAlgoImage.Count > 0)
                    nudInspectImgID.Maximum = Recipe.Param.AlgoImg.ListAlgoImage.Count - 1;
                else
                {
                    MessageBox.Show("影像A數量為0", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    radioButton_OrigImg_Match.Checked = true; // 原始影像
                }
            }
        }

        /// <summary>
        /// 【頻帶:】 (圖像教導)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void combxBand_SelectedIndexChanged(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            if (b_LoadRecipe) // (20200130) Jeff Revised!
                return;

            if (Input_ImgList.Count > 0 && combxBand.SelectedIndex >= 0)
            {
                HObject SrcImg_Match = Get_SourceImage_Match(); // (20200130) Jeff Revised!
                HObject DisplayImage = clsStaticTool.GetChannelImage(SrcImg_Match, (enuBand)combxBand.SelectedIndex);
                HOperatorSet.ClearWindow(DisplayWindows.HalconWindow);
                HOperatorSet.DispObj(DisplayImage, DisplayWindows.HalconWindow);
                DisplayImage.Dispose();
            }
        }

        private void DisplayWindows_HMouseMove(object sender, HMouseEventArgs e)
        {
            try
            {
                if (Input_ImgList.Count > 0)
                {
                    HObject nowImage = Input_ImgList[combxDisplayImg.SelectedIndex];
                    if (nowImage == null) return;
                    HTuple width = null, height = null;
                    HOperatorSet.GetImageSize(nowImage, out width, out height);
                    txt_CursorCoordinate.Text = "(" + String.Format("{0:#}", e.X) + ", " + String.Format("{0:#}", e.Y) + ")";
                    try
                    {
                        if (e.X >= 0 && e.X < width && e.Y >= 0 && e.Y < height)
                        {
                            HTuple grayval;
                            HOperatorSet.GetGrayval(nowImage, e.Y + 0.5, e.X + 0.5, out grayval);
                            txt_ColorValue.Text = (grayval.TupleInt()).ToString();
                            HObject GrayImg;
                            HOperatorSet.Rgb1ToGray(nowImage, out GrayImg);
                            HOperatorSet.GetGrayval(GrayImg, e.Y + 0.5, e.X + 0.5, out grayval);
                            txbGrayValue.Text = grayval.TupleInt().ToString();
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
            }
            catch
            { }
        }
        
        
        private void btnThinInsp_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            ClearDisplay(Input_ImgList[int.Parse(txbThinID.Text)]);
            ParamGetFromUI();

            HObject Thin_Dark, Thin_Bright;
            HOperatorSet.GenEmptyObj(out Thin_Dark);
            HOperatorSet.GenEmptyObj(out Thin_Bright);

            HObject RDefect_Dark, RDefect_Bright;
            HOperatorSet.GenEmptyObj(out RDefect_Dark);
            HOperatorSet.GenEmptyObj(out RDefect_Bright);

            HObject Stain_Dark, Stain_Bright;
            HOperatorSet.GenEmptyObj(out Stain_Dark);
            HOperatorSet.GenEmptyObj(out Stain_Bright);


            HObject CellRegion, ThinRegion, StainRegion, ScratchRegion;

            HOperatorSet.GenEmptyObj(out CellRegion);
            HOperatorSet.GenEmptyObj(out ThinRegion);
            HOperatorSet.GenEmptyObj(out StainRegion);
            HOperatorSet.GenEmptyObj(out ScratchRegion);

            HTuple Angle;
            HObject SrcImg_Match = Get_SourceImage_Match(); // (20200130) Jeff Revised!
            Method.PtternMatchAndSegRegion(SrcImg_Match, Recipe, out PatternRegions, out Angle, out CellRegion, out ThinRegion, out StainRegion, out ScratchRegion);
            HObject RShift_Dark, RShift_Bright;
            HOperatorSet.GenEmptyObj(out RShift_Dark);
            HOperatorSet.GenEmptyObj(out RShift_Bright);

            Method.PartitionRegions(Input_ImgList, Recipe, ThinRegion, ScratchRegion, StainRegion, out Thin_Dark, out Thin_Bright, out RDefect_Dark, out RDefect_Bright, out Stain_Dark, out Stain_Bright, out RShift_Dark, out RShift_Bright);

            HObject ThinDefectRegions, MapRegion;
            HOperatorSet.GenEmptyObj(out ThinDefectRegions);
           
            InductanceInsp.ThinParam ThinParam = new InductanceInsp.ThinParam(Input_ImgList[Recipe.Param.RisistAOIParam.ThinParam.InspID], Recipe, CellRegion, Thin_Dark, Thin_Bright);
            var ThinTask = new Task<InductanceInsp.clsThinInspResult>(InductanceInsp.ThinInspTask, ThinParam);
            ThinTask.Start();

            ThinDefectRegions = ThinTask.Result.ThinRes;
            Method.LocalDefectMappping(ThinDefectRegions, CellRegion, out MapRegion);

            ThinTask.Dispose();
            if (!Recipe.Param.RisistAOIParam.ThinParam.DefectSumAreaEnabled)
                HOperatorSet.Connection(ThinDefectRegions, out SelectRegions);
            else
            {
                SelectRegions.Dispose();
                SelectRegions = ThinDefectRegions.Clone();
            }
            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
            HOperatorSet.DispObj(ThinDefectRegions, DisplayWindows.HalconWindow);
            Method.Convert2Circle(MapRegion, out MapRegion);
            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "green");
            HOperatorSet.DispObj(MapRegion, DisplayWindows.HalconWindow);

            DrawImg(ThinDefectRegions, MapRegion, int.Parse(txbThinID.Text));
        }
        
        private void btnScratchInsp_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            if (Input_ImgList == null)
            { return; }
            if (Input_ImgList[int.Parse(txbScratchID.Text)] == null)
            { return; }
            ClearDisplay(Input_ImgList[int.Parse(txbScratchID.Text)]);
            ParamGetFromUI();
            HObject CellRegion, ThinRegion, StainRegion, ScratchRegion;
            HOperatorSet.GenEmptyObj(out CellRegion);
            HOperatorSet.GenEmptyObj(out ThinRegion);
            HOperatorSet.GenEmptyObj(out StainRegion);
            HOperatorSet.GenEmptyObj(out ScratchRegion);

            HTuple Angle;
            HObject SrcImg_Match = Get_SourceImage_Match(); // (20200130) Jeff Revised!
            Method.PtternMatchAndSegRegion(SrcImg_Match, Recipe, out PatternRegions, out Angle, out CellRegion, out ThinRegion, out StainRegion, out ScratchRegion);
            HObject ScratchDefectRegions, Mapregion;

            InductanceInsp.ScratchParam ScratchParam = new InductanceInsp.ScratchParam(Input_ImgList[Recipe.Param.RisistAOIParam.ScratchParam.InspID], Recipe, CellRegion, ScratchRegion, ThinRegion);
            var ScratchTask = new Task<HObject>(InductanceInsp.ScratchInspTask, ScratchParam);
            ScratchTask.Start();

            ScratchDefectRegions = ScratchTask.Result;
            Method.LocalDefectMappping(ScratchDefectRegions, CellRegion, out Mapregion);

            ScratchTask.Dispose();

            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
            HOperatorSet.DispObj(ScratchDefectRegions, DisplayWindows.HalconWindow);


            Method.Convert2Circle(Mapregion, out Mapregion);
            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "green");
            HOperatorSet.DispObj(Mapregion, DisplayWindows.HalconWindow);

            HOperatorSet.Connection(ScratchDefectRegions, out SelectRegions);

            DrawImg(ScratchDefectRegions, Mapregion, Recipe.Param.RisistAOIParam.ScratchParam.InspID);
        }
        
        private void btnRShiftTest_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            ParamGetFromUI();
            if (Input_ImgList == null)
            { return; }
            if (Input_ImgList[int.Parse(txbRDefectID.Text)] == null)
            { return; }

            ParamGetFromUI();
            ClearDisplay(Input_ImgList[int.Parse(txbRDefectID.Text)]);
            HObject Thin_Dark, Thin_Bright;
            HOperatorSet.GenEmptyObj(out Thin_Dark);
            HOperatorSet.GenEmptyObj(out Thin_Bright);

            HObject RDefect_Dark, RDefect_Bright;
            HOperatorSet.GenEmptyObj(out RDefect_Dark);
            HOperatorSet.GenEmptyObj(out RDefect_Bright);

            HObject Stain_Dark, Stain_Bright;
            HOperatorSet.GenEmptyObj(out Stain_Dark);
            HOperatorSet.GenEmptyObj(out Stain_Bright);


            HObject CellRegion, ThinRegion, StainRegion, ScratchRegion;

            HOperatorSet.GenEmptyObj(out CellRegion);
            HOperatorSet.GenEmptyObj(out ThinRegion);
            HOperatorSet.GenEmptyObj(out StainRegion);
            HOperatorSet.GenEmptyObj(out ScratchRegion);

            HTuple Angle;
            HObject SrcImg_Match = Get_SourceImage_Match(); // (20200130) Jeff Revised!
            Method.PtternMatchAndSegRegion(SrcImg_Match, Recipe, out PatternRegions, out Angle, out CellRegion, out ThinRegion, out StainRegion, out ScratchRegion);
            HObject RShift_Dark, RShift_Bright;
            HOperatorSet.GenEmptyObj(out RShift_Dark);
            HOperatorSet.GenEmptyObj(out RShift_Bright);

            Method.PartitionRegions(Input_ImgList, Recipe, ThinRegion, ScratchRegion, StainRegion, out Thin_Dark, out Thin_Bright, out RDefect_Dark, out RDefect_Bright, out Stain_Dark, out Stain_Bright, out RShift_Dark, out RShift_Bright);

            HObject RDefectRegions, MapRegion;

            InductanceInsp.RDefectParam RDefectParam = new InductanceInsp.RDefectParam(Input_ImgList[Recipe.Param.RisistAOIParam.RDefectParam.InspID], Recipe, CellRegion, RDefect_Dark, RDefect_Bright);
            var RDefectTask = new Task<HObject>(InductanceInsp.RDefectInspTask, RDefectParam);
            RDefectTask.Start();

            RDefectRegions = RDefectTask.Result;
            Method.LocalDefectMappping(RDefectRegions, CellRegion, out MapRegion);

            RDefectTask.Dispose();

            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
            HOperatorSet.DispObj(RDefectRegions, DisplayWindows.HalconWindow);
            HOperatorSet.Connection(RDefectRegions, out SelectRegions);

            Method.Convert2Circle(MapRegion, out MapRegion);
            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "green");
            HOperatorSet.DispObj(MapRegion, DisplayWindows.HalconWindow);

            DrawImg(RDefectRegions, MapRegion, Recipe.Param.RisistAOIParam.RDefectParam.InspID);
        }

        private void btnPartitionTest_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            if (Input_ImgList == null)
            { return; }
            if (Input_ImgList[int.Parse(txbThinID.Text)] == null)
            { return; }

            ParamGetFromUI();
            ClearDisplay(Input_ImgList[int.Parse(txbThinID.Text)]);
            HObject Thin_Dark, Thin_Bright;
            HOperatorSet.GenEmptyObj(out Thin_Dark);
            HOperatorSet.GenEmptyObj(out Thin_Bright);

            HObject RDefect_Dark, RDefect_Bright;
            HOperatorSet.GenEmptyObj(out RDefect_Dark);
            HOperatorSet.GenEmptyObj(out RDefect_Bright);

            HObject Stain_Dark, Stain_Bright;
            HOperatorSet.GenEmptyObj(out Stain_Dark);
            HOperatorSet.GenEmptyObj(out Stain_Bright);


            HObject CellRegion, ThinRegion, StainRegion, ScratchRegion;

            HOperatorSet.GenEmptyObj(out CellRegion);
            HOperatorSet.GenEmptyObj(out ThinRegion);
            HOperatorSet.GenEmptyObj(out StainRegion);
            HOperatorSet.GenEmptyObj(out ScratchRegion);

            HTuple Angle;
            HObject SrcImg_Match = Get_SourceImage_Match(); // (20200130) Jeff Revised!
            Method.PtternMatchAndSegRegion(SrcImg_Match, Recipe, out PatternRegions, out Angle, out CellRegion, out ThinRegion, out StainRegion, out ScratchRegion);
            HObject RShift_Dark, RShift_Bright;
            HOperatorSet.GenEmptyObj(out RShift_Dark);
            HOperatorSet.GenEmptyObj(out RShift_Bright);

            Method.PartitionRegions(Input_ImgList, Recipe, ThinRegion, ScratchRegion, StainRegion, out Thin_Dark, out Thin_Bright, out RDefect_Dark, out RDefect_Bright, out Stain_Dark, out Stain_Bright, out RShift_Dark, out RShift_Bright);

            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "black");

            HOperatorSet.DispObj(Thin_Dark, DisplayWindows.HalconWindow);

            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "green");

            HOperatorSet.DispObj(Thin_Bright, DisplayWindows.HalconWindow);

        }
        
        private void btnRDefectPartitionTest_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            if (Input_ImgList == null)
                return;

            HObject SrcImg_Match = Get_SourceImage_Match(); // (20200130) Jeff Revised!
            if (SrcImg_Match == null)
                return;

            ParamGetFromUI();
            ClearDisplay(SrcImg_Match);
            HObject Thin_Dark, Thin_Bright;
            HOperatorSet.GenEmptyObj(out Thin_Dark);
            HOperatorSet.GenEmptyObj(out Thin_Bright);

            HObject RDefect_Dark, RDefect_Bright;
            HOperatorSet.GenEmptyObj(out RDefect_Dark);
            HOperatorSet.GenEmptyObj(out RDefect_Bright);

            HObject Stain_Dark, Stain_Bright;
            HOperatorSet.GenEmptyObj(out Stain_Dark);
            HOperatorSet.GenEmptyObj(out Stain_Bright);


            HObject CellRegion, ThinRegion, StainRegion, ScratchRegion;

            HOperatorSet.GenEmptyObj(out CellRegion);
            HOperatorSet.GenEmptyObj(out ThinRegion);
            HOperatorSet.GenEmptyObj(out StainRegion);
            HOperatorSet.GenEmptyObj(out ScratchRegion);

            HTuple Angle;
         
            Method.PtternMatchAndSegRegion(SrcImg_Match, Recipe, out PatternRegions, out Angle, out CellRegion, out ThinRegion, out StainRegion, out ScratchRegion);
            HObject RShift_Dark, RShift_Bright;
            HOperatorSet.GenEmptyObj(out RShift_Dark);
            HOperatorSet.GenEmptyObj(out RShift_Bright);

            Method.PartitionRegions(Input_ImgList, Recipe, ThinRegion, ScratchRegion, StainRegion, out Thin_Dark, out Thin_Bright, out RDefect_Dark, out RDefect_Bright, out Stain_Dark, out Stain_Bright, out RShift_Dark, out RShift_Bright);

            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "black");

            HOperatorSet.DispObj(RDefect_Dark, DisplayWindows.HalconWindow);

            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "green");

            HOperatorSet.DispObj(RDefect_Bright, DisplayWindows.HalconWindow);
        }

        private void btnStainPartitionTest_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            if (Input_ImgList == null)
                return;

            HObject SrcImg_Match = Get_SourceImage_Match(); // (20200130) Jeff Revised!
            if (SrcImg_Match == null)
                return;

            ParamGetFromUI();
            ClearDisplay(SrcImg_Match);
            HObject Thin_Dark, Thin_Bright;
            HOperatorSet.GenEmptyObj(out Thin_Dark);
            HOperatorSet.GenEmptyObj(out Thin_Bright);

            HObject RDefect_Dark, RDefect_Bright;
            HOperatorSet.GenEmptyObj(out RDefect_Dark);
            HOperatorSet.GenEmptyObj(out RDefect_Bright);

            HObject Stain_Dark, Stain_Bright;
            HOperatorSet.GenEmptyObj(out Stain_Dark);
            HOperatorSet.GenEmptyObj(out Stain_Bright);


            HObject CellRegion, ThinRegion, StainRegion, ScratchRegion;

            HOperatorSet.GenEmptyObj(out CellRegion);
            HOperatorSet.GenEmptyObj(out ThinRegion);
            HOperatorSet.GenEmptyObj(out StainRegion);
            HOperatorSet.GenEmptyObj(out ScratchRegion);

            HTuple Angle;
           
            Method.PtternMatchAndSegRegion(SrcImg_Match, Recipe, out PatternRegions, out Angle, out CellRegion, out ThinRegion, out StainRegion, out ScratchRegion);
            HObject RShift_Dark, RShift_Bright;
            HOperatorSet.GenEmptyObj(out RShift_Dark);
            HOperatorSet.GenEmptyObj(out RShift_Bright);

            Method.PartitionRegions(Input_ImgList, Recipe, ThinRegion, ScratchRegion, StainRegion, out Thin_Dark, out Thin_Bright, out RDefect_Dark, out RDefect_Bright, out Stain_Dark, out Stain_Bright, out RShift_Dark, out RShift_Bright);

            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "black");

            HOperatorSet.DispObj(Stain_Dark, DisplayWindows.HalconWindow);

            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "green");

            HOperatorSet.DispObj(Stain_Bright, DisplayWindows.HalconWindow);
        }

        private void btnStainTest_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            if (Input_ImgList == null)
            { return; }
            if (Input_ImgList[int.Parse(txbStainID.Text)] == null)
            { return; }
            ClearDisplay(Input_ImgList[int.Parse(txbStainID.Text)]);
            ParamGetFromUI();
            HObject Thin_Dark, Thin_Bright;
            HOperatorSet.GenEmptyObj(out Thin_Dark);
            HOperatorSet.GenEmptyObj(out Thin_Bright);

            HObject RDefect_Dark, RDefect_Bright;
            HOperatorSet.GenEmptyObj(out RDefect_Dark);
            HOperatorSet.GenEmptyObj(out RDefect_Bright);

            HObject Stain_Dark, Stain_Bright;
            HOperatorSet.GenEmptyObj(out Stain_Dark);
            HOperatorSet.GenEmptyObj(out Stain_Bright);


            HObject CellRegion, ThinRegion, StainRegion, ScratchRegion;

            HOperatorSet.GenEmptyObj(out CellRegion);
            HOperatorSet.GenEmptyObj(out ThinRegion);
            HOperatorSet.GenEmptyObj(out StainRegion);
            HOperatorSet.GenEmptyObj(out ScratchRegion);

            HTuple Angle;
            HObject SrcImg_Match = Get_SourceImage_Match(); // (20200130) Jeff Revised!
            Method.PtternMatchAndSegRegion(SrcImg_Match, Recipe, out PatternRegions, out Angle, out CellRegion, out ThinRegion, out StainRegion, out ScratchRegion);

            HObject RShift_Dark, RShift_Bright;
            HOperatorSet.GenEmptyObj(out RShift_Dark);
            HOperatorSet.GenEmptyObj(out RShift_Bright);

            Method.PartitionRegions(Input_ImgList, Recipe, ThinRegion, ScratchRegion, StainRegion, out Thin_Dark, out Thin_Bright, out RDefect_Dark, out RDefect_Bright, out Stain_Dark, out Stain_Bright, out RShift_Dark, out RShift_Bright);

            HObject StainDefectRegions, MapRegion;

            InductanceInsp.StainParam StainParam = new InductanceInsp.StainParam(Input_ImgList[Recipe.Param.RisistAOIParam.StainParam.InspID], Recipe, CellRegion, Stain_Dark, Stain_Bright);
            var StainTask = new Task<HObject>(InductanceInsp.StainInspTask, StainParam);
            StainTask.Start();

            StainDefectRegions = StainTask.Result;

            Method.LocalDefectMappping(StainDefectRegions, CellRegion, out MapRegion);

            StainTask.Dispose();

            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
            HOperatorSet.DispObj(StainDefectRegions, DisplayWindows.HalconWindow);
            HOperatorSet.Connection(StainDefectRegions, out SelectRegions);

            Method.Convert2Circle(MapRegion, out MapRegion);
            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "green");
            HOperatorSet.DispObj(MapRegion, DisplayWindows.HalconWindow);

            DrawImg(StainDefectRegions, MapRegion, Recipe.Param.RisistAOIParam.StainParam.InspID);
        }

        private void btnScratchInTH_Click(object sender, EventArgs e)
        {
            // 檢查是否已讀取影像
            if (!Check_Input_ImgList())
                return;

            HObject SrcImg_R, SrcImg_G, SrcImg_B;

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out SrcImg_R);
            HOperatorSet.GenEmptyObj(out SrcImg_G);
            HOperatorSet.GenEmptyObj(out SrcImg_B);

            HObject SrcImg;

            HObject TeachImg;

            HOperatorSet.GenEmptyObj(out TeachImg);



            HOperatorSet.CopyImage(Input_ImgList[int.Parse(txbScratchID.Text)], out SrcImg);

            HTuple Ch;
            HOperatorSet.CountChannels(SrcImg, out Ch);
            if (Ch == 1)
                TeachImg = SrcImg.Clone();
            else
            {
                HOperatorSet.Decompose3(SrcImg, out SrcImg_R, out SrcImg_G, out SrcImg_B);
                if (combxBand.SelectedIndex == 0)
                {
                    TeachImg = SrcImg_R;
                }
                else if (combxBand.SelectedIndex == 1)
                {
                    TeachImg = SrcImg_G;
                }
                else
                {
                    TeachImg = SrcImg_B;
                }
            }

            FormThresholdAdjust MyForm = new FormThresholdAdjust(TeachImg,
                                                                 int.Parse(nudScratchInTH.Value.ToString()),
                                                                 255, FormThresholdAdjust.enType.Bright);

            MyForm.ShowDialog();

            if (MyForm.DialogResult == DialogResult.OK)
            {
                int ThMin, ThMax;
                MyForm.GetThreshold(out ThMin, out ThMax);

                nudScratchInTH.Value = ThMin;
            }

            TeachImg.Dispose();
            SrcImg.Dispose();
            SrcImg_R.Dispose();
            SrcImg_G.Dispose();
            SrcImg_B.Dispose();
        }

        private void btnScratchOutTh_Click(object sender, EventArgs e)
        {
            // 檢查是否已讀取影像
            if (!Check_Input_ImgList())
                return;

            HObject SrcImg_R, SrcImg_G, SrcImg_B;

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out SrcImg_R);
            HOperatorSet.GenEmptyObj(out SrcImg_G);
            HOperatorSet.GenEmptyObj(out SrcImg_B);

            HObject SrcImg;

            HObject TeachImg;

            HOperatorSet.GenEmptyObj(out TeachImg);



            HOperatorSet.CopyImage(Input_ImgList[int.Parse(txbScratchID.Text)], out SrcImg);

            HTuple Ch;
            HOperatorSet.CountChannels(SrcImg, out Ch);
            if (Ch == 1)
                TeachImg = SrcImg.Clone();
            else
            {
                HOperatorSet.Decompose3(SrcImg, out SrcImg_R, out SrcImg_G, out SrcImg_B);
                if (combxBand.SelectedIndex == 0)
                {
                    TeachImg = SrcImg_R;
                }
                else if (combxBand.SelectedIndex == 1)
                {
                    TeachImg = SrcImg_G;
                }
                else
                {
                    TeachImg = SrcImg_B;
                }
            }

            FormThresholdAdjust MyForm = new FormThresholdAdjust(TeachImg,
                                                                 int.Parse(nudScratchOutTH.Value.ToString()),
                                                                 255, FormThresholdAdjust.enType.Bright);

            MyForm.ShowDialog();

            if (MyForm.DialogResult == DialogResult.OK)
            {
                int ThMin, ThMax;
                MyForm.GetThreshold(out ThMin, out ThMax);

                nudScratchOutTH.Value = ThMin;
            }

            TeachImg.Dispose();
            SrcImg.Dispose();
            SrcImg_R.Dispose();
            SrcImg_G.Dispose();
            SrcImg_B.Dispose();
        }

        private void btnStainDarkTh_Click(object sender, EventArgs e)
        {
            // 檢查是否已讀取影像
            if (!Check_Input_ImgList())
                return;

            HObject SrcImg_R, SrcImg_G, SrcImg_B;

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out SrcImg_R);
            HOperatorSet.GenEmptyObj(out SrcImg_G);
            HOperatorSet.GenEmptyObj(out SrcImg_B);

            HObject SrcImg;

            HObject TeachImg;

            HOperatorSet.GenEmptyObj(out TeachImg);

            HOperatorSet.CopyImage(Input_ImgList[int.Parse(txbStainID.Text)], out SrcImg);

            HTuple Ch;
            HOperatorSet.CountChannels(SrcImg, out Ch);
            if (Ch == 1)
                TeachImg = SrcImg.Clone();
            else
            {
                HOperatorSet.Decompose3(SrcImg, out SrcImg_R, out SrcImg_G, out SrcImg_B);
                if (combxBand.SelectedIndex == 0)
                {
                    TeachImg = SrcImg_R;
                }
                else if (combxBand.SelectedIndex == 1)
                {
                    TeachImg = SrcImg_G;
                }
                else
                {
                    TeachImg = SrcImg_B;
                }
            }

            FormThresholdAdjust MyForm = new FormThresholdAdjust(TeachImg,
                                                                 0,
                                                                 int.Parse(nudStainDarkTh.Value.ToString()), FormThresholdAdjust.enType.Dark);

            MyForm.ShowDialog();

            if (MyForm.DialogResult == DialogResult.OK)
            {
                int ThMin, ThMax;
                MyForm.GetThreshold(out ThMin, out ThMax);

                nudStainDarkTh.Value = ThMax;
            }

            TeachImg.Dispose();
            SrcImg.Dispose();
            SrcImg_R.Dispose();
            SrcImg_G.Dispose();
            SrcImg_B.Dispose();
        }

        private void btnStainBrightTh_Click(object sender, EventArgs e)
        {
            // 檢查是否已讀取影像
            if (!Check_Input_ImgList())
                return;

            HObject SrcImg_R, SrcImg_G, SrcImg_B;

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out SrcImg_R);
            HOperatorSet.GenEmptyObj(out SrcImg_G);
            HOperatorSet.GenEmptyObj(out SrcImg_B);

            HObject SrcImg;

            HObject TeachImg;

            HOperatorSet.GenEmptyObj(out TeachImg);

            HOperatorSet.CopyImage(Input_ImgList[int.Parse(txbStainID.Text)], out SrcImg);

            HTuple Ch;
            HOperatorSet.CountChannels(SrcImg, out Ch);
            if (Ch == 1)
                TeachImg = SrcImg.Clone();
            else
            {
                HOperatorSet.Decompose3(SrcImg, out SrcImg_R, out SrcImg_G, out SrcImg_B);
                if (combxBand.SelectedIndex == 0)
                {
                    TeachImg = SrcImg_R;
                }
                else if (combxBand.SelectedIndex == 1)
                {
                    TeachImg = SrcImg_G;
                }
                else
                {
                    TeachImg = SrcImg_B;
                }
            }

            FormThresholdAdjust MyForm = new FormThresholdAdjust(TeachImg,
                                                                 0,
                                                                 int.Parse(nudStainBrightTh.Value.ToString()), FormThresholdAdjust.enType.Dark);

            MyForm.ShowDialog();

            if (MyForm.DialogResult == DialogResult.OK)
            {
                int ThMin, ThMax;
                MyForm.GetThreshold(out ThMin, out ThMax);

                nudStainBrightTh.Value = ThMax;
            }

            TeachImg.Dispose();
            SrcImg.Dispose();
            SrcImg_R.Dispose();
            SrcImg_G.Dispose();
            SrcImg_B.Dispose();
        }

        private void btnRDefectDarkTh_Click(object sender, EventArgs e)
        {
            // 檢查是否已讀取影像
            if (!Check_Input_ImgList())
                return;

            HObject SrcImg_R, SrcImg_G, SrcImg_B;

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out SrcImg_R);
            HOperatorSet.GenEmptyObj(out SrcImg_G);
            HOperatorSet.GenEmptyObj(out SrcImg_B);

            HObject SrcImg;

            HObject TeachImg;

            HOperatorSet.GenEmptyObj(out TeachImg);



            HOperatorSet.CopyImage(Input_ImgList[int.Parse(txbRDefectID.Text)], out SrcImg);

            HTuple Ch;
            HOperatorSet.CountChannels(SrcImg, out Ch);
            if (Ch == 1)
                TeachImg = SrcImg.Clone();
            else
            {
                HOperatorSet.Decompose3(SrcImg, out SrcImg_R, out SrcImg_G, out SrcImg_B);
                if (combxBand.SelectedIndex == 0)
                {
                    TeachImg = SrcImg_R;
                }
                else if (combxBand.SelectedIndex == 1)
                {
                    TeachImg = SrcImg_G;
                }
                else
                {
                    TeachImg = SrcImg_B;
                }
            }

            FormThresholdAdjust MyForm = new FormThresholdAdjust(TeachImg,
                                                                 int.Parse(nudRDefectDarkMinTh.Value.ToString()),
                                                                 int.Parse(nudRDefectDarkMaxTh.Value.ToString()), FormThresholdAdjust.enType.Dual);

            MyForm.ShowDialog();

            if (MyForm.DialogResult == DialogResult.OK)
            {
                int ThMin, ThMax;
                MyForm.GetThreshold(out ThMin, out ThMax);

                nudRDefectDarkMaxTh.Value = ThMax;
                nudRDefectDarkMinTh.Value = ThMin;
            }

            TeachImg.Dispose();
            SrcImg.Dispose();
            SrcImg_R.Dispose();
            SrcImg_G.Dispose();
            SrcImg_B.Dispose();
        }

        private void btnRDefectBrightTh_Click(object sender, EventArgs e)
        {
            // 檢查是否已讀取影像
            if (!Check_Input_ImgList())
                return;

            HObject SrcImg_R, SrcImg_G, SrcImg_B;

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out SrcImg_R);
            HOperatorSet.GenEmptyObj(out SrcImg_G);
            HOperatorSet.GenEmptyObj(out SrcImg_B);

            HObject SrcImg;

            HObject TeachImg;

            HOperatorSet.GenEmptyObj(out TeachImg);



            HOperatorSet.CopyImage(Input_ImgList[int.Parse(txbRDefectID.Text)], out SrcImg);

            HTuple Ch;
            HOperatorSet.CountChannels(SrcImg, out Ch);
            if (Ch == 1)
                TeachImg = SrcImg.Clone();
            else
            {
                HOperatorSet.Decompose3(SrcImg, out SrcImg_R, out SrcImg_G, out SrcImg_B);
                if (combxBand.SelectedIndex == 0)
                {
                    TeachImg = SrcImg_R;
                }
                else if (combxBand.SelectedIndex == 1)
                {
                    TeachImg = SrcImg_G;
                }
                else
                {
                    TeachImg = SrcImg_B;
                }
            }

            FormThresholdAdjust MyForm = new FormThresholdAdjust(TeachImg,
                                                                 int.Parse(nudRDefectBrightMinTh.Value.ToString()),
                                                                 int.Parse(nudRDefectBrightMaxTh.Value.ToString()), FormThresholdAdjust.enType.Dual);

            MyForm.ShowDialog();

            if (MyForm.DialogResult == DialogResult.OK)
            {
                int ThMin, ThMax;
                MyForm.GetThreshold(out ThMin, out ThMax);

                nudRDefectBrightMaxTh.Value = ThMax;
                nudRDefectBrightMinTh.Value = ThMin;
            }

            TeachImg.Dispose();
            SrcImg.Dispose();
            SrcImg_R.Dispose();
            SrcImg_G.Dispose();
            SrcImg_B.Dispose();
        }

        private void btnMaskTH_Click(object sender, EventArgs e)
        {
            // 檢查是否已讀取影像
            if (!Check_Input_ImgList())
                return;

            HObject SrcImg_R, SrcImg_G, SrcImg_B;

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out SrcImg_R);
            HOperatorSet.GenEmptyObj(out SrcImg_G);
            HOperatorSet.GenEmptyObj(out SrcImg_B);

            HObject SrcImg;

            HObject TeachImg;

            HOperatorSet.GenEmptyObj(out TeachImg);


            HObject SrcImg_Match = Get_SourceImage_Match(); // (20200130) Jeff Revised!
            HOperatorSet.CopyImage(SrcImg_Match, out SrcImg);

            TeachImg = clsStaticTool.GetChannelImage(SrcImg, (enuBand)combxBand.SelectedIndex);

            if (cbxHistoEq.Checked)
            {
                HOperatorSet.EquHistoImage(TeachImg, out TeachImg);
            }

            FormThresholdAdjust MyForm = new FormThresholdAdjust(TeachImg,
                                                                 Recipe.Param.SegParam.MaskThValueMin,
                                                                 Recipe.Param.SegParam.MaskThValueMax, FormThresholdAdjust.enType.Dual);

            MyForm.ShowDialog();

            if (MyForm.DialogResult == DialogResult.OK)
            {
                int ThMin, ThMax;
                MyForm.GetThreshold(out ThMin, out ThMax);
                Recipe.Param.SegParam.MaskThValueMax = ThMax;
                Recipe.Param.SegParam.MaskThValueMin = ThMin;

            }

            TeachImg.Dispose();
            SrcImg.Dispose();
            SrcImg_R.Dispose();
            SrcImg_G.Dispose();
            SrcImg_B.Dispose();
        }

        private void btnSettingMethod_Click(object sender, EventArgs e)
        {
            ParamGetFromUI();
            UpdateUIDisplay();
            //UpdateParameter();
        }

        /// <summary>
        /// 顯示原始影像
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void combxDisplayImg_SelectedIndexChanged(object sender, EventArgs e)
        {
            HOperatorSet.DispObj(this.Input_ImgList[combxDisplayImg.SelectedIndex], DisplayWindows.HalconWindow);
        }

        private void comboBoxInnerBand_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Input_ImgList.Count > 0)
            {
                HTuple Ch;
                HOperatorSet.CountChannels(Input_ImgList[combxDisplayImg.SelectedIndex], out Ch);
                if (Ch == 1)
                {
                    return;
                }
                HObject SrcImg_R, SrcImg_G, SrcImg_B;

                if (comboBoxInnerBand.SelectedIndex == 0)
                {
                    HOperatorSet.AccessChannel(Input_ImgList[combxDisplayImg.SelectedIndex], out SrcImg_R, 1);
                    HOperatorSet.DispObj(SrcImg_R, DisplayWindows.HalconWindow);
                    SrcImg_R.Dispose();
                }
                else if (comboBoxInnerBand.SelectedIndex == 1)
                {
                    HOperatorSet.AccessChannel(Input_ImgList[combxDisplayImg.SelectedIndex], out SrcImg_G, 2);
                    HOperatorSet.DispObj(SrcImg_G, DisplayWindows.HalconWindow);
                    SrcImg_G.Dispose();
                }
                else if (comboBoxInnerBand.SelectedIndex == 2)
                {
                    HOperatorSet.AccessChannel(Input_ImgList[combxDisplayImg.SelectedIndex], out SrcImg_B, 3);
                    HOperatorSet.DispObj(SrcImg_B, DisplayWindows.HalconWindow);
                    SrcImg_B.Dispose();
                }
                else
                {
                    HObject GrayImg;
                    HOperatorSet.Rgb1ToGray(Input_ImgList[combxDisplayImg.SelectedIndex], out GrayImg);
                    HOperatorSet.DispObj(GrayImg, DisplayWindows.HalconWindow);
                    GrayImg.Dispose();
                }
            }
        }

        private void comboBoxOuterBand_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Input_ImgList.Count > 0)
            {
                HTuple Ch;
                HOperatorSet.CountChannels(Input_ImgList[combxDisplayImg.SelectedIndex], out Ch);
                if (Ch == 1)
                {
                    return;
                }
                HObject SrcImg_R, SrcImg_G, SrcImg_B;

                if (comboBoxOuterBand.SelectedIndex == 0)
                {
                    HOperatorSet.AccessChannel(Input_ImgList[combxDisplayImg.SelectedIndex], out SrcImg_R, 1);
                    HOperatorSet.DispObj(SrcImg_R, DisplayWindows.HalconWindow);
                    SrcImg_R.Dispose();
                }
                else if (comboBoxOuterBand.SelectedIndex == 1)
                {
                    HOperatorSet.AccessChannel(Input_ImgList[combxDisplayImg.SelectedIndex], out SrcImg_G, 2);
                    HOperatorSet.DispObj(SrcImg_G, DisplayWindows.HalconWindow);
                    SrcImg_G.Dispose();
                }
                else if (comboBoxOuterBand.SelectedIndex == 2)
                {
                    HOperatorSet.AccessChannel(Input_ImgList[combxDisplayImg.SelectedIndex], out SrcImg_B, 3);
                    HOperatorSet.DispObj(SrcImg_B, DisplayWindows.HalconWindow);
                    SrcImg_B.Dispose();
                }
                else
                {
                    HObject GrayImg;
                    HOperatorSet.Rgb1ToGray(Input_ImgList[combxDisplayImg.SelectedIndex], out GrayImg);
                    HOperatorSet.DispObj(GrayImg, DisplayWindows.HalconWindow);
                    GrayImg.Dispose();
                }
            }
        }

        private void btnTestEro_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            ParamGetFromUI();
            if (!Recipe.Param.InnerInspParam.Enabled)
            {
                MessageBox.Show("未啟用");
                return;
            }

            // 檢查是否已讀取影像
            if (!Check_Input_ImgList())
                return;

            combxDisplayImg.SelectedIndex = 0;

            HObject CellRegion, ThinRegion, RDefectRegion, ScratchRegion;

            HOperatorSet.GenEmptyObj(out CellRegion);
            HOperatorSet.GenEmptyObj(out ThinRegion);
            HOperatorSet.GenEmptyObj(out RDefectRegion);
            HOperatorSet.GenEmptyObj(out ScratchRegion);

            HObject SrcImg_Match = Get_SourceImage_Match(); // (20200130) Jeff Revised!
            if (SrcImg_Match != null)
            {
                ClearDisplay(SrcImg_Match);

                HTuple Angle;

                Method.PtternMatchAndSegRegion(SrcImg_Match, Recipe, out PatternRegions, out Angle, out CellRegion, out ThinRegion, out RDefectRegion, out ScratchRegion);
            }
            HObject Erosion;
            int hv_EdgeSkipSize = (int)nudInnerEdgeSkipSize.Value * 2;
            int SkipHeight = (int)nudInnerEdgeSkipHeight.Value * 2;
            HOperatorSet.ErosionRectangle1(PatternRegions, out Erosion, hv_EdgeSkipSize, SkipHeight);

            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "green");
            HOperatorSet.DispObj(Erosion, DisplayWindows.HalconWindow);
        }

        private void btnDilTest_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            ParamGetFromUI();
            if (!Recipe.Param.OuterInspParam.Enabled)
            {
                MessageBox.Show("未啟用");
                return;
            }

            // 檢查是否已讀取影像
            if (!Check_Input_ImgList())
                return;

            combxDisplayImg.SelectedIndex = 0;

            HObject CellRegion, ThinRegion, RDefectRegion, ScratchRegion;

            HOperatorSet.GenEmptyObj(out CellRegion);
            HOperatorSet.GenEmptyObj(out ThinRegion);
            HOperatorSet.GenEmptyObj(out RDefectRegion);
            HOperatorSet.GenEmptyObj(out ScratchRegion);

            HObject SrcImg_Match = Get_SourceImage_Match(); // (20200130) Jeff Revised!
            if (SrcImg_Match != null && Input_ImgList[int.Parse(txbOuterImgIndex.Text)] != null)
            {
                ClearDisplay(Input_ImgList[int.Parse(txbOuterImgIndex.Text)]);

                HTuple Angle;
             
                Method.PtternMatchAndSegRegion(SrcImg_Match, Recipe, out PatternRegions, out Angle, out CellRegion, out ThinRegion, out RDefectRegion, out ScratchRegion);
            }

            int hv_EdgeSkipSize = (int)nudOuterEdgeSkipSize.Value;
            int SkipHeight = (int)nudOuterEdgeSkipHeight.Value;
            HObject PatternDil;
            HOperatorSet.DilationRectangle1(PatternRegions, out PatternDil, hv_EdgeSkipSize * 2, SkipHeight * 2);

            HObject DiffRegions;
            HOperatorSet.Difference(CellRegion, PatternDil, out DiffRegions);

            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "green");
            HOperatorSet.DispObj(DiffRegions, DisplayWindows.HalconWindow);
        }

        private void nudScaleMin_ValueChanged(object sender, EventArgs e)
        {
            if (nudScaleMin.Value > nudScaleMax.Value)
            {
                MessageBox.Show("Scale最小值不能大於最大值!!");
                double S = (double)nudScaleMax.Value - 0.1;
                nudScaleMin.Value = Convert.ToDecimal(S.ToString());
            }
        }

        private void nudScaleMax_ValueChanged(object sender, EventArgs e)
        {
            if (nudScaleMin.Value > nudScaleMax.Value)
            {
                MessageBox.Show("Scale最小值不能大於最大值!!");
                double S = (double)nudScaleMin.Value + 0.1;
                nudScaleMax.Value = Convert.ToDecimal(S.ToString());
            }
        }

        private void cbxTestROI_CheckedChanged(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            CheckBox Obj = (CheckBox)sender;

            #region 確認是否可執行

            if (Input_ImgList == null)
            {
                cbxASetPattern.CheckedChanged -= cbxSetPattern_CheckedChanged;
                cbxASetPattern.Checked = false;
                cbxASetPattern.CheckedChanged += cbxSetPattern_CheckedChanged;
                MessageBox.Show("請先讀取影像");
                return;
            }

            if (Input_ImgList.Count <= 0)
            {
                cbxASetPattern.CheckedChanged -= cbxSetPattern_CheckedChanged;
                cbxASetPattern.Checked = false;
                cbxASetPattern.CheckedChanged += cbxSetPattern_CheckedChanged;
                MessageBox.Show("請先讀取影像");
                return;
            }
            HObject SrcImg_Match = Get_SourceImage_Match(); // (20200130) Jeff Revised!
            ClearDisplay(SrcImg_Match);

            #endregion

            if (Obj.Checked)
            {
                if (Recipe.DAVSParam.DAVS_Mode != 0)
                {
                    DialogResult dialogResult = MessageBox.Show("若重新設定對位可能會造成AI需要重新訓練,請確認是否繼續?", "警告!!!", MessageBoxButtons.YesNo);

                    if (dialogResult != DialogResult.Yes)
                    {
                        Obj.CheckedChanged -= cbxTestROI_CheckedChanged;
                        Obj.Checked = false;
                        Obj.CheckedChanged += cbxTestROI_CheckedChanged;
                        return;
                    }
                }

                #region 設定範圍
                try
                {
                    clsStaticTool.EnableAllControls(this.tpPatternMatch, Obj, false);
                    ChangeColor(Color.Green, Obj);
                    if (drawing_Rect != null)
                    {
                        drawing_Rect.Dispose();
                        drawing_Rect = null;
                    }
                    HTuple ImgWidth, ImgHeight;
                    HOperatorSet.GetImageSize(SrcImg_Match, out ImgWidth, out ImgHeight);

                    //HTuple ID;
                    //HTuple Row = new HTuple(100, 200, 200, 100);
                    //HOperatorSet.CreateDrawingObjectXld(Row, Row ,out ID);
                    //HOperatorSet.AttachDrawingObjectToWindow(DisplayWindows.HalconWindow, ID);

                    drawing_Rect = HDrawingObject.CreateDrawingObject(HDrawingObject.HDrawingObjectType.RECTANGLE1,
                                                                     ImgHeight / 2 - 100,
                                                                     ImgWidth / 2 - 100,
                                                                     ImgHeight / 2 + 100,
                                                                     ImgWidth / 2 + 100);

                    drawing_Rect.SetDrawingObjectParams("color", "green");
                    
                    DisplayWindows.HalconWindow.AttachDrawingObjectToWindow(drawing_Rect);
                }
                catch (Exception ex)
                {
                    Obj.CheckedChanged -= cbxTestROI_CheckedChanged;
                    Obj.Checked = false;
                    Obj.CheckedChanged += cbxTestROI_CheckedChanged;
                    drawing_Rect.Dispose();
                    drawing_Rect = null;
                    clsStaticTool.EnableAllControls(this.tpPatternMatch, Obj, true);
                    ChangeColor(Color.LightGray, Obj);
                    MessageBox.Show(ex.ToString());
                }
                #endregion
            }
            else
            {
                #region 寫入設定
                try
                {
                    HObject Rect;
                    HTuple row1, column1, row2, column2;
                    GetRectData(drawing_Rect.ID, out column1, out row1, out column2, out row2);
                    HOperatorSet.GenRectangle1(out Rect, row1, column1, row2, column2);

                    HTuple Mean = 0.0, dev = 0.0;
                    if (combxDisplayImg.SelectedIndex >= 0 && Input_ImgList != null)
                    {
                        HOperatorSet.Intensity(Rect, Input_ImgList[combxDisplayImg.SelectedIndex], out Mean, out dev);
                    }

                    double W = (column2 - column1);
                    double H = (row2 - row1);
                    double DMean = Mean;

                    double Area = W * H;

                    double Wum = W * Locate_Method_FS.pixel_resolution_;
                    double Hum = H * Locate_Method_FS.pixel_resolution_;
                    double Areaum = Wum * Hum;

                    labW.Text = Wum.ToString("#.###");
                    labA.Text = Areaum.ToString("#.###");
                    labH.Text = Hum.ToString("#.###");
                    labGrayMean.Text = DMean.ToString("#.###");

                    drawing_Rect.Dispose();
                    drawing_Rect = null;

                    ChangeColor(Color.LightGray, Obj);
                    clsStaticTool.EnableAllControls(this.tpPatternMatch, Obj, true);


                    HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                    HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                    HOperatorSet.DispObj(Rect, DisplayWindows.HalconWindow);
                    Rect.Dispose();
                }
                catch (Exception ex)
                {
                    Obj.CheckedChanged -= cbxTestROI_CheckedChanged;
                    Obj.Checked = false;
                    Obj.CheckedChanged += cbxTestROI_CheckedChanged;
                    drawing_Rect.Dispose();
                    drawing_Rect = null;
                    clsStaticTool.EnableAllControls(this.tpPatternMatch, Obj, true);
                    ChangeColor(Color.LightGray, Obj);
                    MessageBox.Show(ex.ToString());
                }
                #endregion
            }
        }

        private void btnThinRegionTest_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            if (Input_ImgList == null)
            { return; }
            if (Input_ImgList[int.Parse(txbThinID.Text)] == null)
            { return; }

            ParamGetFromUI();
            ClearDisplay(Input_ImgList[int.Parse(txbThinID.Text)]);
            HObject Thin_Dark, Thin_Bright;
            HOperatorSet.GenEmptyObj(out Thin_Dark);
            HOperatorSet.GenEmptyObj(out Thin_Bright);

            HObject RDefect_Dark, RDefect_Bright;
            HOperatorSet.GenEmptyObj(out RDefect_Dark);
            HOperatorSet.GenEmptyObj(out RDefect_Bright);

            HObject Stain_Dark, Stain_Bright;
            HOperatorSet.GenEmptyObj(out Stain_Dark);
            HOperatorSet.GenEmptyObj(out Stain_Bright);


            HObject CellRegion, ThinRegion, StainRegion, ScratchRegion;

            HOperatorSet.GenEmptyObj(out CellRegion);
            HOperatorSet.GenEmptyObj(out ThinRegion);
            HOperatorSet.GenEmptyObj(out StainRegion);
            HOperatorSet.GenEmptyObj(out ScratchRegion);

            HTuple Angle;
            HObject SrcImg_Match = Get_SourceImage_Match(); // (20200130) Jeff Revised!
            Method.PtternMatchAndSegRegion(SrcImg_Match, Recipe, out PatternRegions, out Angle, out CellRegion, out ThinRegion, out StainRegion, out ScratchRegion);
            HObject RShift_Dark, RShift_Bright;
            HOperatorSet.GenEmptyObj(out RShift_Dark);
            HOperatorSet.GenEmptyObj(out RShift_Bright);

            Method.PartitionRegions(Input_ImgList, Recipe, ThinRegion, ScratchRegion, StainRegion, out Thin_Dark, out Thin_Bright, out RDefect_Dark, out RDefect_Bright, out Stain_Dark, out Stain_Bright, out RShift_Dark, out RShift_Bright);

            HObject TopHatRegionExpand, ThisInspRegionSkip;
            HOperatorSet.DilationRectangle1(ThinRegion, out TopHatRegionExpand, Recipe.Param.RisistAOIParam.ThinParam.TopHatEdgeExpandWidth, Recipe.Param.RisistAOIParam.ThinParam.TopHatEdgeExpandHeight);
            HOperatorSet.ErosionRectangle1(ThinRegion, out ThisInspRegionSkip, Recipe.Param.RisistAOIParam.ThinParam.EdgeSkipWidth, Recipe.Param.RisistAOIParam.ThinParam.EdgeSkipHeight);

            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "blue");

            HOperatorSet.DispObj(ThisInspRegionSkip, DisplayWindows.HalconWindow);
        }

        private void btnTopHatEdgeExpTest_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            if (Input_ImgList == null)
            { return; }
            if (Input_ImgList[int.Parse(txbThinID.Text)] == null)
            { return; }

            ParamGetFromUI();
            ClearDisplay(Input_ImgList[int.Parse(txbThinID.Text)]);
            HObject Thin_Dark, Thin_Bright;
            HOperatorSet.GenEmptyObj(out Thin_Dark);
            HOperatorSet.GenEmptyObj(out Thin_Bright);

            HObject RDefect_Dark, RDefect_Bright;
            HOperatorSet.GenEmptyObj(out RDefect_Dark);
            HOperatorSet.GenEmptyObj(out RDefect_Bright);

            HObject Stain_Dark, Stain_Bright;
            HOperatorSet.GenEmptyObj(out Stain_Dark);
            HOperatorSet.GenEmptyObj(out Stain_Bright);


            HObject CellRegion, ThinRegion, StainRegion, ScratchRegion;

            HOperatorSet.GenEmptyObj(out CellRegion);
            HOperatorSet.GenEmptyObj(out ThinRegion);
            HOperatorSet.GenEmptyObj(out StainRegion);
            HOperatorSet.GenEmptyObj(out ScratchRegion);

            HTuple Angle;
            HObject SrcImg_Match = Get_SourceImage_Match(); // (20200130) Jeff Revised!
            Method.PtternMatchAndSegRegion(SrcImg_Match, Recipe, out PatternRegions, out Angle, out CellRegion, out ThinRegion, out StainRegion, out ScratchRegion);
            HObject RShift_Dark, RShift_Bright;
            HOperatorSet.GenEmptyObj(out RShift_Dark);
            HOperatorSet.GenEmptyObj(out RShift_Bright);

            Method.PartitionRegions(Input_ImgList, Recipe, ThinRegion, ScratchRegion, StainRegion, out Thin_Dark, out Thin_Bright, out RDefect_Dark, out RDefect_Bright, out Stain_Dark, out Stain_Bright, out RShift_Dark, out RShift_Bright);
            
            HObject TopHatRegionExpand, ExtHRegion, DiffRegion;
            HOperatorSet.DilationRectangle1(ThinRegion, out TopHatRegionExpand, Recipe.Param.RisistAOIParam.ThinParam.TopHatEdgeExpandWidth, Recipe.Param.RisistAOIParam.ThinParam.TopHatEdgeExpandHeight);

            HOperatorSet.DilationRectangle1(TopHatRegionExpand, out ExtHRegion, 1, Recipe.Param.RisistAOIParam.ThinParam.ExtH);
            HOperatorSet.Difference(ExtHRegion, TopHatRegionExpand, out DiffRegion);


            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "blue");

            HOperatorSet.DispObj(TopHatRegionExpand, DisplayWindows.HalconWindow);

            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "green");
            HOperatorSet.DispObj(DiffRegion, DisplayWindows.HalconWindow);
        }

        private void btnTestAverage_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            if (Input_ImgList == null)
            { return; }
            if (Input_ImgList[int.Parse(txbThinID.Text)] == null)
            { return; }

            ParamGetFromUI();
            ClearDisplay(Input_ImgList[int.Parse(txbThinID.Text)]);
            HObject Thin_Dark, Thin_Bright;
            HOperatorSet.GenEmptyObj(out Thin_Dark);
            HOperatorSet.GenEmptyObj(out Thin_Bright);

            HObject RDefect_Dark, RDefect_Bright;
            HOperatorSet.GenEmptyObj(out RDefect_Dark);
            HOperatorSet.GenEmptyObj(out RDefect_Bright);

            HObject Stain_Dark, Stain_Bright;
            HOperatorSet.GenEmptyObj(out Stain_Dark);
            HOperatorSet.GenEmptyObj(out Stain_Bright);


            HObject CellRegion, ThinRegion, StainRegion, ScratchRegion;

            HOperatorSet.GenEmptyObj(out CellRegion);
            HOperatorSet.GenEmptyObj(out ThinRegion);
            HOperatorSet.GenEmptyObj(out StainRegion);
            HOperatorSet.GenEmptyObj(out ScratchRegion);

            HTuple Angle;
            HObject SrcImg_Match = Get_SourceImage_Match(); // (20200130) Jeff Revised!
            Method.PtternMatchAndSegRegion(SrcImg_Match, Recipe, out PatternRegions, out Angle, out CellRegion, out ThinRegion, out StainRegion, out ScratchRegion);
            HObject ConnectRegion;
            HOperatorSet.Connection(ThinRegion, out ConnectRegion);
            HTuple Mean, Dev;

            HOperatorSet.Intensity(ConnectRegion, SrcImg_Match, out Mean, out Dev);
            double Max = Mean.TupleMax();
            double Mid = Mean.TupleMedian();
            double Min = Mean.TupleMin();
            labMax.Text = ((int)Max).ToString();
            labMin.Text = ((int)Min).ToString();
            labMid.Text = ((int)Mid).ToString();
        }
        
        /// <summary>
        /// 【儲存結果】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveImg_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveImgDialog = new SaveFileDialog();
            saveImgDialog.Filter = "JPeg Image|*.jpg|PNG Image|*.png|Bitmap Image|*.bmp|TIFF Image|*.tif";
            saveImgDialog.Title = "另存結果影像";
            if (saveImgDialog.ShowDialog() != DialogResult.OK)
                return;

            string path = saveImgDialog.FileName;
            try
            {
                switch (saveImgDialog.FilterIndex)
                {
                    case 1:
                        HOperatorSet.WriteImage(SaveImage, "jpeg", 0, path);
                        break;

                    case 2:
                        HOperatorSet.WriteImage(SaveImage, "png", 0, path);
                        break;
                    case 3:
                        HOperatorSet.WriteImage(SaveImage, "bmp", 0, path);
                        break;

                    case 4:
                        HOperatorSet.WriteImage(SaveImage, "tiff", 0, path);
                        break;
                }

                MessageBox.Show("儲存成功");
            }
            catch(Exception Ex)
            {
                MessageBox.Show("Save result image fails...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                MessageBox.Show(Ex.ToString());
            }
        }

        private void bthHistoEqThSetting_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            if (Input_ImgList == null)
                return;

            HObject SrcImg_Match = Get_SourceImage_Match(); // (20200130) Jeff Revised!
            if (SrcImg_Match == null)
                return;

            ParamGetFromUI();
            ClearDisplay(SrcImg_Match);
            HObject Thin_Dark, Thin_Bright;
            HOperatorSet.GenEmptyObj(out Thin_Dark);
            HOperatorSet.GenEmptyObj(out Thin_Bright);

            HObject RDefect_Dark, RDefect_Bright;
            HOperatorSet.GenEmptyObj(out RDefect_Dark);
            HOperatorSet.GenEmptyObj(out RDefect_Bright);

            HObject Stain_Dark, Stain_Bright;
            HOperatorSet.GenEmptyObj(out Stain_Dark);
            HOperatorSet.GenEmptyObj(out Stain_Bright);


            HObject CellRegion, ThinRegion, StainRegion, ScratchRegion;

            HOperatorSet.GenEmptyObj(out CellRegion);
            HOperatorSet.GenEmptyObj(out ThinRegion);
            HOperatorSet.GenEmptyObj(out StainRegion);
            HOperatorSet.GenEmptyObj(out ScratchRegion);

            HTuple Angle;
            //Method.PtternMatch(SrcImg_Match, Recipe, out PatternRegions, out Angle);

            //Method.GetAllInspRegions(PatternRegions, Recipe, SrcImg_Match, out CellRegion, out ThinRegion, out StainRegion, out ScratchRegion);
            Method.PtternMatchAndSegRegion(SrcImg_Match, Recipe, out PatternRegions, out Angle, out CellRegion, out ThinRegion, out StainRegion, out ScratchRegion);
            HObject RShift_Dark, RShift_Bright;
            HOperatorSet.GenEmptyObj(out RShift_Dark);
            HOperatorSet.GenEmptyObj(out RShift_Bright);

            Method.PartitionRegions(Input_ImgList, Recipe, ThinRegion, ScratchRegion, StainRegion, out Thin_Dark, out Thin_Bright, out RDefect_Dark, out RDefect_Bright, out Stain_Dark, out Stain_Bright, out RShift_Dark, out RShift_Bright);
            //Method.PartitionRegions(Input_ImgList, Recipe, ThinRegion, ScratchRegion, StainRegion, out Thin_Dark, out Thin_Bright, out RDefect_Dark, out RDefect_Bright, out Stain_Dark, out Stain_Bright);

            HObject RDefectReg;
            HOperatorSet.ConcatObj(RDefect_Dark, RDefect_Bright, out RDefectReg);

            HObject RegionExpand,RegionSkip;

            HOperatorSet.DilationRectangle1(RDefectReg, out RegionExpand, Recipe.Param.RisistAOIParam.RDefectParam.ExtWidth, Recipe.Param.RisistAOIParam.RDefectParam.ExtHeight);
            HOperatorSet.ErosionRectangle1(RegionExpand, out RegionSkip, Recipe.Param.RisistAOIParam.RDefectParam.SkipWidth, Recipe.Param.RisistAOIParam.RDefectParam.SkipHeight);


            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "blue");

            HOperatorSet.DispObj(RegionSkip, DisplayWindows.HalconWindow);
        }

        private void nudHistoEdgeSkipTest_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            if (Input_ImgList == null)
            { return; }
            if (Input_ImgList[int.Parse(txbThinID.Text)] == null)
            { return; }

            ParamGetFromUI();
            ClearDisplay(Input_ImgList[int.Parse(txbThinID.Text)]);
            HObject Thin_Dark, Thin_Bright;
            HOperatorSet.GenEmptyObj(out Thin_Dark);
            HOperatorSet.GenEmptyObj(out Thin_Bright);

            HObject RDefect_Dark, RDefect_Bright;
            HOperatorSet.GenEmptyObj(out RDefect_Dark);
            HOperatorSet.GenEmptyObj(out RDefect_Bright);

            HObject Stain_Dark, Stain_Bright;
            HOperatorSet.GenEmptyObj(out Stain_Dark);
            HOperatorSet.GenEmptyObj(out Stain_Bright);


            HObject CellRegion, ThinRegion, StainRegion, ScratchRegion;

            HOperatorSet.GenEmptyObj(out CellRegion);
            HOperatorSet.GenEmptyObj(out ThinRegion);
            HOperatorSet.GenEmptyObj(out StainRegion);
            HOperatorSet.GenEmptyObj(out ScratchRegion);

            HTuple Angle;
            HObject SrcImg_Match = Get_SourceImage_Match(); // (20200130) Jeff Revised!
            //Method.PtternMatch(SrcImg_Match, Recipe, out PatternRegions, out Angle);

            //Method.GetAllInspRegions(PatternRegions, Recipe, SrcImg_Match, out CellRegion, out ThinRegion, out StainRegion, out ScratchRegion);
            Method.PtternMatchAndSegRegion(SrcImg_Match, Recipe, out PatternRegions, out Angle, out CellRegion, out ThinRegion, out StainRegion, out ScratchRegion);
            HObject RShift_Dark, RShift_Bright;
            HOperatorSet.GenEmptyObj(out RShift_Dark);
            HOperatorSet.GenEmptyObj(out RShift_Bright);

            Method.PartitionRegions(Input_ImgList, Recipe, ThinRegion, ScratchRegion, StainRegion, out Thin_Dark, out Thin_Bright, out RDefect_Dark, out RDefect_Bright, out Stain_Dark, out Stain_Bright, out RShift_Dark, out RShift_Bright);
            //Method.PartitionRegions(Input_ImgList, Recipe, ThinRegion, ScratchRegion, StainRegion, out Thin_Dark, out Thin_Bright, out RDefect_Dark, out RDefect_Bright, out Stain_Dark, out Stain_Bright);

            HObject TopHatRegionExpand, ThisInspRegionSkip;
            HOperatorSet.DilationRectangle1(ThinRegion, out TopHatRegionExpand, Recipe.Param.RisistAOIParam.ThinParam.TopHatEdgeExpandWidth, Recipe.Param.RisistAOIParam.ThinParam.TopHatEdgeExpandHeight);
            HOperatorSet.ErosionRectangle1(ThinRegion, out ThisInspRegionSkip, Recipe.Param.RisistAOIParam.ThinParam.HistoEdgeSkipWidth, Recipe.Param.RisistAOIParam.ThinParam.HistoEdgeSkipHeight);

            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "blue");

            HOperatorSet.DispObj(ThisInspRegionSkip, DisplayWindows.HalconWindow);
        }

        private void btnThinHatRegionTest_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            if (Input_ImgList == null)
            { return; }
            if (Input_ImgList[int.Parse(txbThinID.Text)] == null)
            { return; }

            ParamGetFromUI();
            ClearDisplay(Input_ImgList[int.Parse(txbThinID.Text)]);
            HObject Thin_Dark, Thin_Bright;
            HOperatorSet.GenEmptyObj(out Thin_Dark);
            HOperatorSet.GenEmptyObj(out Thin_Bright);

            HObject RDefect_Dark, RDefect_Bright;
            HOperatorSet.GenEmptyObj(out RDefect_Dark);
            HOperatorSet.GenEmptyObj(out RDefect_Bright);

            HObject Stain_Dark, Stain_Bright;
            HOperatorSet.GenEmptyObj(out Stain_Dark);
            HOperatorSet.GenEmptyObj(out Stain_Bright);


            HObject CellRegion, ThinRegion, StainRegion, ScratchRegion;

            HOperatorSet.GenEmptyObj(out CellRegion);
            HOperatorSet.GenEmptyObj(out ThinRegion);
            HOperatorSet.GenEmptyObj(out StainRegion);
            HOperatorSet.GenEmptyObj(out ScratchRegion);

            HTuple Angle;
            HObject SrcImg_Match = Get_SourceImage_Match(); // (20200130) Jeff Revised!
            //Method.PtternMatch(SrcImg_Match, Recipe, out PatternRegions, out Angle);

            //Method.GetAllInspRegions(PatternRegions, Recipe, SrcImg_Match, out CellRegion, out ThinRegion, out StainRegion, out ScratchRegion);
            Method.PtternMatchAndSegRegion(SrcImg_Match, Recipe, out PatternRegions, out Angle, out CellRegion, out ThinRegion, out StainRegion, out ScratchRegion);
            HObject RShift_Dark, RShift_Bright;
            HOperatorSet.GenEmptyObj(out RShift_Dark);
            HOperatorSet.GenEmptyObj(out RShift_Bright);

            Method.PartitionRegions(Input_ImgList, Recipe, ThinRegion, ScratchRegion, StainRegion, out Thin_Dark, out Thin_Bright, out RDefect_Dark, out RDefect_Bright, out Stain_Dark, out Stain_Bright, out RShift_Dark, out RShift_Bright);
            //Method.PartitionRegions(Input_ImgList, Recipe, ThinRegion, ScratchRegion, StainRegion, out Thin_Dark, out Thin_Bright, out RDefect_Dark, out RDefect_Bright, out Stain_Dark, out Stain_Bright);

            HObject ThisInspRegionSkip;

            HOperatorSet.ErosionRectangle1(ThinRegion, out ThisInspRegionSkip, Recipe.Param.RisistAOIParam.ThinParam.ThinHatEdgeSkipWidth, Recipe.Param.RisistAOIParam.ThinParam.ThinHatEdgeSkipHeight);

            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "blue");

            HOperatorSet.DispObj(ThisInspRegionSkip, DisplayWindows.HalconWindow);
        }

        private void btnScratchRegion_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            if (Input_ImgList == null)
                return;

            HObject SrcImg_Match = Get_SourceImage_Match(); // (20200130) Jeff Revised!
            if (SrcImg_Match == null)
                return;

            ParamGetFromUI();
            ClearDisplay(SrcImg_Match);
            HObject Thin_Dark, Thin_Bright;
            HOperatorSet.GenEmptyObj(out Thin_Dark);
            HOperatorSet.GenEmptyObj(out Thin_Bright);

            HObject RDefect_Dark, RDefect_Bright;
            HOperatorSet.GenEmptyObj(out RDefect_Dark);
            HOperatorSet.GenEmptyObj(out RDefect_Bright);

            HObject Stain_Dark, Stain_Bright;
            HOperatorSet.GenEmptyObj(out Stain_Dark);
            HOperatorSet.GenEmptyObj(out Stain_Bright);


            HObject CellRegion, ThinRegion, StainRegion, ScratchRegion;

            HOperatorSet.GenEmptyObj(out CellRegion);
            HOperatorSet.GenEmptyObj(out ThinRegion);
            HOperatorSet.GenEmptyObj(out StainRegion);
            HOperatorSet.GenEmptyObj(out ScratchRegion);

            HTuple Angle;
            //Method.PtternMatch(SrcImg_Match, Recipe, out PatternRegions, out Angle);

            //Method.GetAllInspRegions(PatternRegions, Recipe, SrcImg_Match, out CellRegion, out ThinRegion, out StainRegion, out ScratchRegion);
            Method.PtternMatchAndSegRegion(SrcImg_Match, Recipe, out PatternRegions, out Angle, out CellRegion, out ThinRegion, out StainRegion, out ScratchRegion);
            HObject RShift_Dark, RShift_Bright;
            HOperatorSet.GenEmptyObj(out RShift_Dark);
            HOperatorSet.GenEmptyObj(out RShift_Bright);

            Method.PartitionRegions(Input_ImgList, Recipe, ThinRegion, ScratchRegion, StainRegion, out Thin_Dark, out Thin_Bright, out RDefect_Dark, out RDefect_Bright, out Stain_Dark, out Stain_Bright, out RShift_Dark, out RShift_Bright);
            //Method.PartitionRegions(Input_ImgList, Recipe, ThinRegion, ScratchRegion, StainRegion, out Thin_Dark, out Thin_Bright, out RDefect_Dark, out RDefect_Bright, out Stain_Dark, out Stain_Bright);

            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "blue");

            HOperatorSet.DispObj(ScratchRegion, DisplayWindows.HalconWindow);

        }

        private void btnBrightRegionTh_Click(object sender, EventArgs e)
        {
            // 檢查是否已讀取影像
            if (!Check_Input_ImgList())
                return;

            HObject SrcImg_R, SrcImg_G, SrcImg_B;

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out SrcImg_R);
            HOperatorSet.GenEmptyObj(out SrcImg_G);
            HOperatorSet.GenEmptyObj(out SrcImg_B);

            HObject SrcImg;

            HObject TeachImg;

            HOperatorSet.GenEmptyObj(out TeachImg);



            HOperatorSet.CopyImage(Input_ImgList[int.Parse(txbThinID.Text)], out SrcImg);

            HTuple Ch;
            HOperatorSet.CountChannels(SrcImg, out Ch);
            if (Ch == 1)
                TeachImg = SrcImg.Clone();
            else
            {
                HOperatorSet.Decompose3(SrcImg, out SrcImg_R, out SrcImg_G, out SrcImg_B);
                if (combxBand.SelectedIndex == 0)
                {
                    TeachImg = SrcImg_R;
                }
                else if (combxBand.SelectedIndex == 1)
                {
                    TeachImg = SrcImg_G;
                }
                else
                {
                    TeachImg = SrcImg_B;
                }
            }

            FormThresholdAdjust MyForm = new FormThresholdAdjust(TeachImg,
                                                                 Recipe.Param.RisistAOIParam.ThinParam.BrightDetectTh,
                                                                 255, FormThresholdAdjust.enType.Bright);

            MyForm.ShowDialog();

            if (MyForm.DialogResult == DialogResult.OK)
            {
                int ThMin, ThMax;
                MyForm.GetThreshold(out ThMin, out ThMax);

                Recipe.Param.RisistAOIParam.ThinParam.BrightDetectTh = ThMin;
            }

            TeachImg.Dispose();
            SrcImg.Dispose();
            SrcImg_R.Dispose();
            SrcImg_G.Dispose();
            SrcImg_B.Dispose();
        }

        /// <summary>
        /// 【灰階對位】: Find the best matches of an NCC model in an image.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxNCCMode_CheckedChanged(object sender, EventArgs e)
        {
            cbxMetric.SelectedIndex = 3; // 極性模式: use_polarity
            MessageBox.Show("若切換對位模式，請重新教導對位模式，否則可能發生錯誤");
        }

        private void btnSettingHistoEqTh_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {

            #region 取得檢測區域與分區
            if (Input_ImgList == null)
            { return; }
            if (Input_ImgList[int.Parse(txbThinID.Text)] == null)
            { return; }

            ParamGetFromUI();
            ClearDisplay(Input_ImgList[int.Parse(txbThinID.Text)]);
            HObject Thin_Dark, Thin_Bright;
            HOperatorSet.GenEmptyObj(out Thin_Dark);
            HOperatorSet.GenEmptyObj(out Thin_Bright);

            HObject RDefect_Dark, RDefect_Bright;
            HOperatorSet.GenEmptyObj(out RDefect_Dark);
            HOperatorSet.GenEmptyObj(out RDefect_Bright);

            HObject Stain_Dark, Stain_Bright;
            HOperatorSet.GenEmptyObj(out Stain_Dark);
            HOperatorSet.GenEmptyObj(out Stain_Bright);


            HObject CellRegion, ThinRegion, StainRegion, ScratchRegion;

            HOperatorSet.GenEmptyObj(out CellRegion);
            HOperatorSet.GenEmptyObj(out ThinRegion);
            HOperatorSet.GenEmptyObj(out StainRegion);
            HOperatorSet.GenEmptyObj(out ScratchRegion);

            HTuple Angle;
            HObject SrcImg_Match = Get_SourceImage_Match(); // (20200130) Jeff Revised!
            //Method.PtternMatch(SrcImg_Match, Recipe, out PatternRegions, out Angle);

            //Method.GetAllInspRegions(PatternRegions, Recipe, SrcImg_Match, out CellRegion, out ThinRegion, out StainRegion, out ScratchRegion);
            Method.PtternMatchAndSegRegion(SrcImg_Match, Recipe, out PatternRegions, out Angle, out CellRegion, out ThinRegion, out StainRegion, out ScratchRegion);
            HObject RShift_Dark, RShift_Bright;
            HOperatorSet.GenEmptyObj(out RShift_Dark);
            HOperatorSet.GenEmptyObj(out RShift_Bright);

            Method.PartitionRegions(Input_ImgList, Recipe, ThinRegion, ScratchRegion, StainRegion, out Thin_Dark, out Thin_Bright, out RDefect_Dark, out RDefect_Bright, out Stain_Dark, out Stain_Bright, out RShift_Dark, out RShift_Bright);
            //Method.PartitionRegions(Input_ImgList, Recipe, ThinRegion, ScratchRegion, StainRegion, out Thin_Dark, out Thin_Bright, out RDefect_Dark, out RDefect_Bright, out Stain_Dark, out Stain_Bright);

            #endregion

            HObject InspImage;
            HOperatorSet.GenEmptyObj(out InspImage);
            {
                HObject HistoInspRegion;
                HOperatorSet.Union2(Thin_Dark, Thin_Bright, out HistoInspRegion);
                HOperatorSet.ErosionRectangle1(HistoInspRegion, out HistoInspRegion, Recipe.Param.RisistAOIParam.ThinParam.HistoEdgeSkipWidth, Recipe.Param.RisistAOIParam.ThinParam.HistoEdgeSkipWidth);
                HObject ConRegion;
                HOperatorSet.Connection(HistoInspRegion, out ConRegion);
                HTuple CountR;
                HOperatorSet.CountObj(ConRegion, out CountR);

                for (int i = 1; i <= CountR; i++)
                {
                    HObject HistoEqImg, open, close;

                    HObject ReImg, SelectObj;
                    HOperatorSet.SelectObj(ConRegion, out SelectObj, i);
                    HOperatorSet.ReduceDomain(Input_ImgList[Recipe.Param.RisistAOIParam.ThinParam.InspID], SelectObj, out ReImg);
                    HOperatorSet.EquHistoImage(ReImg, out HistoEqImg);

                    HOperatorSet.GrayOpeningRect(HistoEqImg, out open, Recipe.Param.RisistAOIParam.ThinParam.HistoEqOpeningWidth, Recipe.Param.RisistAOIParam.ThinParam.HistoEqOpeningHeight);
                    HOperatorSet.GrayClosingRect(open, out close, Recipe.Param.RisistAOIParam.ThinParam.HistoEqClosingWidth, Recipe.Param.RisistAOIParam.ThinParam.HistoEqClosingHeight);


                    {
                        HObject Tmp;
                        HOperatorSet.ConcatObj(InspImage, close, out Tmp);
                        InspImage.Dispose();
                        InspImage = Tmp;
                    }
                    ReImg.Dispose();
                    open.Dispose();
                    close.Dispose();
                    HistoEqImg.Dispose();
                    SelectObj.Dispose();
                }

            }
            try
            {

            }
            catch (Exception E)
            {
                MessageBox.Show(E.ToString());
            }
        }

        private void btnCreateNewMode_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("此按鈕會讓Mode重新建置,請確認是否繼續?", "警告!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (dialogResult != DialogResult.Yes)
            {
                return;
            }
            try
            {
                if (Mode == enuMode.Bright)
                {
                    if (Recipe.TextureInspectionModel != null)
                    {
                        HOperatorSet.ClearTextureInspectionModel(Recipe.TextureInspectionModel);
                        Recipe.TextureInspectionModel = null;
                    }
                    HOperatorSet.CreateTextureInspectionModel("basic", out Recipe.TextureInspectionModel);
                }
                else
                {
                    if (Recipe.TextureInspectionModelDark != null)
                    {
                        HOperatorSet.ClearTextureInspectionModel(Recipe.TextureInspectionModelDark);
                        Recipe.TextureInspectionModelDark = null;
                    }
                    HOperatorSet.CreateTextureInspectionModel("basic", out Recipe.TextureInspectionModelDark);
                }
                btnSaveMode.Enabled = true;
                MessageBox.Show("新建完成，請重新開始訓練");
            }
            catch
            {

            }
        }

        private void btnAddImage_Click(object sender, EventArgs e)
        {
            try
            {
                HTuple hv_Indices;
                HObject SelectObj, ReducImg;

                //HObject T1, Entropy, I;
                //HOperatorSet.DerivateGauss(Input_ImgList[Recipe.Param.RisistAOIParam.ThinParam.InspID], out T1, 7, "laplace");
                //HOperatorSet.ScaleImageMax(T1, out T1);
                //HOperatorSet.EntropyImage(Input_ImgList[Recipe.Param.RisistAOIParam.ThinParam.InspID], out Entropy, 11, 11);
                //HOperatorSet.SubImage(T1, Entropy, out I, 2, 128);

                HOperatorSet.SelectObj(TrainRegion, out SelectObj, comboBoxSelectRegion.SelectedIndex + 1);
                HOperatorSet.ReduceDomain(Input_ImgList[Recipe.Param.RisistAOIParam.ThinParam.InspID], SelectObj, out ReducImg);
                HObject Crop;
                HOperatorSet.CropDomain(ReducImg, out Crop);
                
                if (Mode == enuMode.Bright)
                    HOperatorSet.AddTextureInspectionModelImage(Crop, Recipe.TextureInspectionModel, out hv_Indices);
                else
                    HOperatorSet.AddTextureInspectionModelImage(Crop, Recipe.TextureInspectionModelDark, out hv_Indices);
                ReducImg.Dispose();
                SelectObj.Dispose();
                MessageBox.Show("加入成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnTextTest_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            try
            {
                if (Input_ImgList == null)
                { return; }
                if (Input_ImgList[int.Parse(txbThinID.Text)] == null)
                { return; }

                if (Mode == enuMode.Bright)
                {
                    if (Recipe.TextureInspectionModel == null)
                    {
                        MessageBox.Show("尚未建立Mode");
                        return;
                    }
                }
                else
                {
                    if (Recipe.TextureInspectionModelDark == null)
                    {
                        MessageBox.Show("尚未建立Mode");
                        return;
                    }
                }

                ParamGetFromUI();
                ClearDisplay(Input_ImgList[int.Parse(txbThinID.Text)]);
                HObject Thin_Dark, Thin_Bright;
                HOperatorSet.GenEmptyObj(out Thin_Dark);
                HOperatorSet.GenEmptyObj(out Thin_Bright);

                HObject RDefect_Dark, RDefect_Bright;
                HOperatorSet.GenEmptyObj(out RDefect_Dark);
                HOperatorSet.GenEmptyObj(out RDefect_Bright);

                HObject Stain_Dark, Stain_Bright;
                HOperatorSet.GenEmptyObj(out Stain_Dark);
                HOperatorSet.GenEmptyObj(out Stain_Bright);


                HObject CellRegion, ThinRegion, StainRegion, ScratchRegion;

                HOperatorSet.GenEmptyObj(out CellRegion);
                HOperatorSet.GenEmptyObj(out ThinRegion);
                HOperatorSet.GenEmptyObj(out StainRegion);
                HOperatorSet.GenEmptyObj(out ScratchRegion);

                HTuple Angle;
                HObject SrcImg_Match = Get_SourceImage_Match(); // (20200130) Jeff Revised!
                Method.PtternMatchAndSegRegion(SrcImg_Match, Recipe, out PatternRegions, out Angle, out CellRegion, out ThinRegion, out StainRegion, out ScratchRegion);
                HObject RShift_Dark, RShift_Bright;
                HOperatorSet.GenEmptyObj(out RShift_Dark);
                HOperatorSet.GenEmptyObj(out RShift_Bright);

                Method.PartitionRegions(Input_ImgList, Recipe, ThinRegion, ScratchRegion, StainRegion, out Thin_Dark, out Thin_Bright, out RDefect_Dark, out RDefect_Bright, out Stain_Dark, out Stain_Bright, out RShift_Dark, out RShift_Bright);
                
                HObject EroRegion;
                if (Mode == enuMode.Bright)
                {
                    HOperatorSet.ErosionRectangle1(Thin_Bright, out EroRegion, Recipe.Param.RisistAOIParam.ThinParam.ThinScratchEdgeSkipWidth, Recipe.Param.RisistAOIParam.ThinParam.ThinScratchEdgeSkipHeight);
                }
                else
                    HOperatorSet.ErosionRectangle1(Thin_Dark, out EroRegion, Recipe.Param.RisistAOIParam.ThinParam.ThinScratchEdgeSkipWidth, Recipe.Param.RisistAOIParam.ThinParam.ThinScratchEdgeSkipHeight);


                HObject DilRegion;
                HOperatorSet.DilationRectangle1(EroRegion, out DilRegion, Recipe.Param.RisistAOIParam.ThinParam.ThinScratchEdgeExWidth, Recipe.Param.RisistAOIParam.ThinParam.ThinScratchEdgeExHeight);
                HOperatorSet.Connection(DilRegion, out DilRegion);

                HTuple ResultID;
                HObject Defect;
                HOperatorSet.GenEmptyObj(out Defect);
                HObject CellDefectRegion;
                
                HObject CropImg;
                HTuple Column1, Row1, Row2, Column2;
                HOperatorSet.RegionFeatures(DilRegion, "row1", out Row1);
                HOperatorSet.RegionFeatures(DilRegion, "column1", out Column1);
                HOperatorSet.RegionFeatures(DilRegion, "row2", out Row2);
                HOperatorSet.RegionFeatures(DilRegion, "column2", out Column2);

                if (Row1.Length <= 0)
                    return;

                //HObject T1, Entropy,I;
                //HOperatorSet.DerivateGauss(Input_ImgList[Recipe.Param.RisistAOIParam.ThinParam.InspID], out T1, 7, "laplace");
                //HOperatorSet.ScaleImageMax(T1, out T1);
                //HOperatorSet.EntropyImage(Input_ImgList[Recipe.Param.RisistAOIParam.ThinParam.InspID], out Entropy, 11, 11);
                //HOperatorSet.SubImage(T1, Entropy, out I, 2, 128);
                HOperatorSet.CropRectangle1(Input_ImgList[Recipe.Param.RisistAOIParam.ThinParam.InspID], out CropImg, Row1, Column1, Row2, Column2);

                if (Mode == enuMode.Bright)
                    HOperatorSet.ApplyTextureInspectionModel(CropImg, out CellDefectRegion, Recipe.TextureInspectionModel, out ResultID);
                else
                    HOperatorSet.ApplyTextureInspectionModel(CropImg, out CellDefectRegion, Recipe.TextureInspectionModelDark, out ResultID);

                HTuple Count;
                HOperatorSet.CountObj(CellDefectRegion, out Count);
                HObject MoveRegion;
                HOperatorSet.GenEmptyObj(out MoveRegion);
                for (int i = 0; i < Count; i++)
                {
                    HObject SelectRegion, Tmp;
                    HOperatorSet.SelectObj(CellDefectRegion, out SelectRegion, i + 1);
                    HOperatorSet.MoveRegion(SelectRegion, out Tmp, Row1[i], Column1[i]);
                    HOperatorSet.ConcatObj(Tmp, MoveRegion, out MoveRegion);
                }
                Defect = MoveRegion;

                HOperatorSet.ClearTextureInspectionResult(ResultID);

                CropImg.Dispose();
                
                HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");

                HOperatorSet.DispObj(Defect, DisplayWindows.HalconWindow);
                //btnSaveMode.Enabled = true;
            }
            catch (HalconException Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }
        private static clsLog TempErrorLog = new clsLog("D://DSI//Log//TempLog");
        private static void Worker_Training(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker Worker = (BackgroundWorker)sender;
            InductanceInspRole Recipe = (InductanceInspRole)e.Argument;
            clsProgressbar m_ProgressBar = new clsProgressbar();
            m_ProgressBar.SetShowText("請等待Mode訓練......");
            m_ProgressBar.SetShowCaption("訓練中......");
            m_ProgressBar.ShowWaitProgress();
            try
            {
                if (Mode == enuMode.Bright)
                {
                    TempErrorLog.WriteLog("Bright Set Param Start");
                    HOperatorSet.SetTextureInspectionModelParam(Recipe.TextureInspectionModel, "patch_normalization", "weber");
                    TempErrorLog.WriteLog("patch_normalization Done");
                    HOperatorSet.SetTextureInspectionModelParam(Recipe.TextureInspectionModel, "levels", (((new HTuple(2)).TupleConcat(3)).TupleConcat(4).TupleConcat(5)));
                    TempErrorLog.WriteLog("levels Done");
                    HOperatorSet.SetTextureInspectionModelParam(Recipe.TextureInspectionModel, "sensitivity", Recipe.Param.RisistAOIParam.ThinParam.ThinScratchSensitivity);
                    TempErrorLog.WriteLog("sensitivity Done");
                    HOperatorSet.SetTextureInspectionModelParam(Recipe.TextureInspectionModel, "gmm_em_max_iter", 200);
                    TempErrorLog.WriteLog("gmm_em_max_iter Done");
                    try
                    {
                        HOperatorSet.TrainTextureInspectionModel(Recipe.TextureInspectionModel);
                    }
                    catch(Exception B)
                    {
                        MessageBox.Show(B.ToString());
                    }
                    TempErrorLog.WriteLog("Train Done");
                }
                else
                {
                    TempErrorLog.WriteLog("Dark Set Param Start");
                    HOperatorSet.SetTextureInspectionModelParam(Recipe.TextureInspectionModelDark, "patch_normalization", "weber");
                    TempErrorLog.WriteLog("patch_normalization Done");
                    HOperatorSet.SetTextureInspectionModelParam(Recipe.TextureInspectionModelDark, "levels", (((new HTuple(2)).TupleConcat(3)).TupleConcat(4).TupleConcat(5)));
                    TempErrorLog.WriteLog("levels Done");
                    HOperatorSet.SetTextureInspectionModelParam(Recipe.TextureInspectionModelDark, "sensitivity", Recipe.Param.RisistAOIParam.ThinParam.ThinScratchSensitivity);
                    TempErrorLog.WriteLog("sensitivity Done");
                    HOperatorSet.SetTextureInspectionModelParam(Recipe.TextureInspectionModelDark, "gmm_em_max_iter", 200);
                    TempErrorLog.WriteLog("gmm_em_max_iter Done");
                    try
                    {
                        HOperatorSet.TrainTextureInspectionModel(Recipe.TextureInspectionModelDark);
                    }
                    catch(Exception D)
                    {
                        MessageBox.Show(D.ToString());
                    }
                    TempErrorLog.WriteLog("Train Done");
                }
                m_ProgressBar.CloseProgress();
                MessageBox.Show("訓練完成");
            }
            catch (HalconException Ex)
            {
                m_ProgressBar.CloseProgress();
                MessageBox.Show(Ex.ToString());
            }
        }
        private void btnTrain_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("此按鈕會讓Mode重新訓練,請確認是否繼續?", "警告!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (dialogResult != DialogResult.Yes)
            {
                return;
            }
            if (Mode == enuMode.Bright)
            {
                if (Recipe.TextureInspectionModel == null)
                {
                    MessageBox.Show("尚未建立Mode");
                    return;
                }
            }
            else
            {
                if (Recipe.TextureInspectionModelDark == null)
                {
                    MessageBox.Show("尚未建立Mode");
                    return;
                }
            }
            ParamGetFromUI();
            BackgroundWorker Worker = new BackgroundWorker();
            InductanceInspRole Param = InductanceInspRole.GetSingleton();
            Worker.WorkerReportsProgress = true;
            Worker.DoWork += new DoWorkEventHandler(Worker_Training);
            Worker.RunWorkerAsync(Param);

            btnSaveMode.Enabled = true;
        }

        private void btnSelectObj_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            if (Input_ImgList == null)
            { return; }
            if (Input_ImgList[int.Parse(txbThinID.Text)] == null)
            { return; }

            ParamGetFromUI();
            ClearDisplay(Input_ImgList[int.Parse(txbThinID.Text)]);
            HObject Thin_Dark, Thin_Bright;
            HOperatorSet.GenEmptyObj(out Thin_Dark);
            HOperatorSet.GenEmptyObj(out Thin_Bright);

            HObject RDefect_Dark, RDefect_Bright;
            HOperatorSet.GenEmptyObj(out RDefect_Dark);
            HOperatorSet.GenEmptyObj(out RDefect_Bright);

            HObject Stain_Dark, Stain_Bright;
            HOperatorSet.GenEmptyObj(out Stain_Dark);
            HOperatorSet.GenEmptyObj(out Stain_Bright);


            HObject CellRegion, ThinRegion, StainRegion, ScratchRegion;

            HOperatorSet.GenEmptyObj(out CellRegion);
            HOperatorSet.GenEmptyObj(out ThinRegion);
            HOperatorSet.GenEmptyObj(out StainRegion);
            HOperatorSet.GenEmptyObj(out ScratchRegion);

            HTuple Angle;
            HObject SrcImg_Match = Get_SourceImage_Match(); // (20200130) Jeff Revised!
            //Method.PtternMatch(SrcImg_Match, Recipe, out PatternRegions, out Angle);

            //Method.GetAllInspRegions(PatternRegions, Recipe, SrcImg_Match, out CellRegion, out ThinRegion, out StainRegion, out ScratchRegion);
            Method.PtternMatchAndSegRegion(SrcImg_Match, Recipe, out PatternRegions, out Angle, out CellRegion, out ThinRegion, out StainRegion, out ScratchRegion);
            HObject RShift_Dark, RShift_Bright;
            HOperatorSet.GenEmptyObj(out RShift_Dark);
            HOperatorSet.GenEmptyObj(out RShift_Bright);

            Method.PartitionRegions(Input_ImgList, Recipe, ThinRegion, ScratchRegion, StainRegion, out Thin_Dark, out Thin_Bright, out RDefect_Dark, out RDefect_Bright, out Stain_Dark, out Stain_Bright, out RShift_Dark, out RShift_Bright);
            //Method.PartitionRegions(Input_ImgList, Recipe, ThinRegion, ScratchRegion, StainRegion, out Thin_Dark, out Thin_Bright, out RDefect_Dark, out RDefect_Bright, out Stain_Dark, out Stain_Bright);


            TrainRegion.Dispose();
            if (Mode == enuMode.Bright)
            {
                HOperatorSet.ErosionRectangle1(Thin_Bright, out TrainRegion, Recipe.Param.RisistAOIParam.ThinParam.ThinScratchEdgeSkipWidth, Recipe.Param.RisistAOIParam.ThinParam.ThinScratchEdgeSkipHeight);
                HOperatorSet.DilationRectangle1(TrainRegion, out TrainRegion, Recipe.Param.RisistAOIParam.ThinParam.ThinScratchEdgeExWidth, Recipe.Param.RisistAOIParam.ThinParam.ThinScratchEdgeExHeight);
            }
            else
            {
                HOperatorSet.ErosionRectangle1(Thin_Dark, out TrainRegion, Recipe.Param.RisistAOIParam.ThinParam.ThinScratchEdgeSkipWidth, Recipe.Param.RisistAOIParam.ThinParam.ThinScratchEdgeSkipHeight);
                HOperatorSet.DilationRectangle1(TrainRegion, out TrainRegion, Recipe.Param.RisistAOIParam.ThinParam.ThinScratchEdgeExWidth, Recipe.Param.RisistAOIParam.ThinParam.ThinScratchEdgeExHeight);
            }
            HTuple Count;
            HOperatorSet.Connection(TrainRegion, out TrainRegion);
            HOperatorSet.SortRegion(TrainRegion, out TrainRegion, "character", "true", "row");
            HOperatorSet.CountObj(TrainRegion, out Count);
            comboBoxSelectRegion.Items.Clear();
            for (int i = 1; i <= Count; i++)
            {
                comboBoxSelectRegion.Items.Add(i.ToString());
            }

            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "blue");
            HOperatorSet.DispObj(TrainRegion, DisplayWindows.HalconWindow);
        }

        private void comboBoxSelectRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TrainRegion != null)
            {
                ClearDisplay(Input_ImgList[int.Parse(txbThinID.Text)]);
                HObject Select;
                HOperatorSet.SelectObj(TrainRegion, out Select, comboBoxSelectRegion.SelectedIndex + 1);
                HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                HOperatorSet.SetColor(DisplayWindows.HalconWindow, "blue");
                HOperatorSet.DispObj(Select, DisplayWindows.HalconWindow);
            }
        }

        private void cbxTrainROI_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox Obj = (CheckBox)sender;

            #region 確認是否可執行
            if (Input_ImgList == null)
            {
                cbxTrainROI.CheckedChanged -= cbxTrainROI_CheckedChanged;
                cbxTrainROI.Checked = false;
                cbxTrainROI.CheckedChanged += cbxTrainROI_CheckedChanged;
                MessageBox.Show("請先讀取影像");
                return;
            }

            if (Input_ImgList.Count <= 0)
            {
                cbxTrainROI.CheckedChanged -= cbxTrainROI_CheckedChanged;
                cbxTrainROI.Checked = false;
                cbxTrainROI.CheckedChanged += cbxTrainROI_CheckedChanged;
                MessageBox.Show("請先讀取影像");
                return;
            }

            if (Input_ImgList.Count <= int.Parse(txbThinID.Text))
            {
                cbxTrainROI.CheckedChanged -= cbxTrainROI_CheckedChanged;
                cbxTrainROI.Checked = false;
                cbxTrainROI.CheckedChanged += cbxTrainROI_CheckedChanged;
                MessageBox.Show("影像個數小於Index");
                return;
            }

            #endregion

            if (Obj.Checked)
            {
                #region 設定範圍

                try
                {
                    clsStaticTool.EnableAllControls(this.Thin, Obj, false);
                    ChangeColor(Color.Green, Obj);
                    if (drawing_Rect != null)
                    {
                        drawing_Rect.Dispose();
                        drawing_Rect = null;
                    }

                    drawing_Rect = HDrawingObject.CreateDrawingObject(HDrawingObject.HDrawingObjectType.RECTANGLE1,
                                                                  Recipe.Param.SegParam.GoldenCenterY - 100,
                                                                  Recipe.Param.SegParam.GoldenCenterX - 100,
                                                                  Recipe.Param.SegParam.GoldenCenterY + 100,
                                                                  Recipe.Param.SegParam.GoldenCenterX + 100);

                    drawing_Rect.SetDrawingObjectParams("color", "green");
                    DisplayWindows.HalconWindow.AttachDrawingObjectToWindow(drawing_Rect);
                }
                catch (Exception ex)
                {
                    cbxTrainROI.CheckedChanged -= cbxTrainROI_CheckedChanged;
                    cbxTrainROI.Checked = false;
                    cbxTrainROI.CheckedChanged += cbxTrainROI_CheckedChanged;
                    drawing_Rect.Dispose();
                    drawing_Rect = null;
                    clsStaticTool.EnableAllControls(this.tpPatternMatch, Obj, true);
                    ChangeColor(Color.LightGray, Obj);
                    MessageBox.Show(ex.ToString());
                }
                #endregion
            }
            else
            {
                #region 寫入設定
                try
                {
                    DialogResult dialogResult = MessageBox.Show("是否確認影像加入Mode中,請確認是否繼續?", "警告!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (dialogResult != DialogResult.Yes)
                    {
                        cbxTrainROI.CheckedChanged -= cbxTrainROI_CheckedChanged;
                        cbxTrainROI.Checked = false;
                        cbxTrainROI.CheckedChanged += cbxTrainROI_CheckedChanged;
                        drawing_Rect.Dispose();
                        drawing_Rect = null;
                        clsStaticTool.EnableAllControls(this.Thin, Obj, true);
                        ChangeColor(Color.LightGray, Obj);
                        return;
                    }

                    HObject Hotspot_Rect = null;
                    HTuple row1, column1, row2, column2;
                    GetRectData(drawing_Rect.ID, out column1, out row1, out column2, out row2);
                    HOperatorSet.GenRectangle1(out Hotspot_Rect, row1, column1, row2, column2);
                    HTuple hv_Indices;

                    HObject ReducImg;
                    HOperatorSet.ReduceDomain(Input_ImgList[Recipe.Param.RisistAOIParam.ThinParam.InspID], Hotspot_Rect, out ReducImg);
                    HObject Crop;
                    
                    HOperatorSet.CropDomain(ReducImg, out Crop);

                    if (Mode == enuMode.Bright)
                        HOperatorSet.AddTextureInspectionModelImage(Crop, Recipe.TextureInspectionModel, out hv_Indices);
                    else
                        HOperatorSet.AddTextureInspectionModelImage(Crop, Recipe.TextureInspectionModelDark, out hv_Indices);
                    ReducImg.Dispose();
                    drawing_Rect.Dispose();
                    drawing_Rect = null;
                    ChangeColor(Color.LightGray, Obj);
                    clsStaticTool.EnableAllControls(this.Thin, Obj, true);
                    MessageBox.Show("加入成功");
                }
                catch (Exception ex)
                {
                    cbxTrainROI.CheckedChanged -= cbxTrainROI_CheckedChanged;
                    cbxTrainROI.Checked = false;
                    cbxTrainROI.CheckedChanged += cbxTrainROI_CheckedChanged;
                    drawing_Rect.Dispose();
                    drawing_Rect = null;
                    clsStaticTool.EnableAllControls(this.Thin, Obj, true);
                    ChangeColor(Color.LightGray, Obj);
                    MessageBox.Show(ex.ToString());
                }
                #endregion
            }
        }

        private void btnSaveMode_Click(object sender, EventArgs e)
        {

            DialogResult dialogResult = MessageBox.Show("是否確認儲存Mode,請確認是否繼續?", "警告!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (dialogResult != DialogResult.Yes)
            {
                return;
            }

            try
            {
                if (Mode == enuMode.Bright)
                {
                    if (Recipe.TextureInspectionModel != null)
                    {
                        HOperatorSet.WriteTextureInspectionModel(Recipe.TextureInspectionModel, Path + "TextureModel.htim");
                    }
                }
                else
                {
                    if (Recipe.TextureInspectionModelDark != null)
                    {
                        HOperatorSet.WriteTextureInspectionModel(Recipe.TextureInspectionModelDark, Path + "TextureModelDark.htim");
                    }
                }

                btnSaveMode.Enabled = false;
                MessageBox.Show("儲存完成");
            }
            catch (HalconException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void comboxModeSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboxModeSelect.SelectedIndex >= 0)
            {
                switch (comboxModeSelect.SelectedIndex)
                {
                    case 0:
                        Mode = enuMode.Dark;
                        break;

                    case 1:
                        Mode = enuMode.Bright;
                        break;
                }
            }
        }

        private void btnRShiftInsp_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            ParamGetFromUI();
            if (Input_ImgList == null)
            { return; }
            if (Input_ImgList[int.Parse(txbRDefectID.Text)] == null)
            { return; }

            ParamGetFromUI();
            ClearDisplay(Input_ImgList[int.Parse(txbRDefectID.Text)]);
            HObject Thin_Dark, Thin_Bright;
            HOperatorSet.GenEmptyObj(out Thin_Dark);
            HOperatorSet.GenEmptyObj(out Thin_Bright);

            HObject RDefect_Dark, RDefect_Bright;
            HOperatorSet.GenEmptyObj(out RDefect_Dark);
            HOperatorSet.GenEmptyObj(out RDefect_Bright);

            HObject Stain_Dark, Stain_Bright;
            HOperatorSet.GenEmptyObj(out Stain_Dark);
            HOperatorSet.GenEmptyObj(out Stain_Bright);


            HObject CellRegion, ThinRegion, StainRegion, ScratchRegion;

            HOperatorSet.GenEmptyObj(out CellRegion);
            HOperatorSet.GenEmptyObj(out ThinRegion);
            HOperatorSet.GenEmptyObj(out StainRegion);
            HOperatorSet.GenEmptyObj(out ScratchRegion);

            HTuple Angle;
            HObject SrcImg_Match = Get_SourceImage_Match(); // (20200130) Jeff Revised!
            //Method.PtternMatch(SrcImg_Match, Recipe, out PatternRegions, out Angle);

            //Method.GetAllInspRegions(PatternRegions, Recipe, SrcImg_Match, out CellRegion, out ThinRegion, out StainRegion, out ScratchRegion);
            Method.PtternMatchAndSegRegion(SrcImg_Match, Recipe, out PatternRegions, out Angle, out CellRegion, out ThinRegion, out StainRegion, out ScratchRegion);

            HObject RShift_Dark, RShift_Bright;
            HOperatorSet.GenEmptyObj(out RShift_Dark);
            HOperatorSet.GenEmptyObj(out RShift_Bright);

            Method.PartitionRegions(Input_ImgList, Recipe, ThinRegion, ScratchRegion, StainRegion, out Thin_Dark, out Thin_Bright, out RDefect_Dark, out RDefect_Bright, out Stain_Dark, out Stain_Bright, out RShift_Dark, out RShift_Bright);

            HObject Defect;
            HOperatorSet.GenEmptyObj(out Defect);

            HObject Dark_Defect, Bright_Defect;
            HOperatorSet.GenEmptyObj(out Dark_Defect);
            HOperatorSet.GenEmptyObj(out Bright_Defect);

            try
            {
                InductanceInsp.RShiftParam RShiftParam_Bright = new InductanceInsp.RShiftParam(Input_ImgList[Recipe.Param.RisistAOIParam.RDefectParam.InspID], RShift_Bright, Recipe);
                var RShiftTask_Bright = new Task<HObject>(InductanceInsp.RshiftNewInspTask, RShiftParam_Bright);
                RShiftTask_Bright.Start();
                
                InductanceInsp.RShiftParam RShiftParam_Dark = new InductanceInsp.RShiftParam(Input_ImgList[Recipe.Param.RisistAOIParam.RDefectParam.InspID], RShift_Dark, Recipe);
                var RShiftTask_Dark = new Task<HObject>(InductanceInsp.RshiftNewInspTask, RShiftParam_Dark);
                RShiftTask_Dark.Start();
                

                Bright_Defect = RShiftTask_Bright.Result;
                Dark_Defect = RShiftTask_Dark.Result;


                RShiftTask_Dark.Dispose();
                RShiftTask_Bright.Dispose();

                HOperatorSet.ConcatObj(Dark_Defect, Bright_Defect, out Defect);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
            HOperatorSet.DispObj(Defect, DisplayWindows.HalconWindow);
        }

        private void btnTargetETest_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            ParamGetFromUI();

            if (Input_ImgList == null)
                return;

            if (Input_ImgList[int.Parse(txbRDefectID.Text)] == null)
                return;

            ParamGetFromUI();
            ClearDisplay(Input_ImgList[int.Parse(txbRDefectID.Text)]);
            HObject Thin_Dark, Thin_Bright;
            HOperatorSet.GenEmptyObj(out Thin_Dark);
            HOperatorSet.GenEmptyObj(out Thin_Bright);

            HObject RDefect_Dark, RDefect_Bright;
            HOperatorSet.GenEmptyObj(out RDefect_Dark);
            HOperatorSet.GenEmptyObj(out RDefect_Bright);

            HObject Stain_Dark, Stain_Bright;
            HOperatorSet.GenEmptyObj(out Stain_Dark);
            HOperatorSet.GenEmptyObj(out Stain_Bright);


            HObject CellRegion, ThinRegion, StainRegion, ScratchRegion;

            HOperatorSet.GenEmptyObj(out CellRegion);
            HOperatorSet.GenEmptyObj(out ThinRegion);
            HOperatorSet.GenEmptyObj(out StainRegion);
            HOperatorSet.GenEmptyObj(out ScratchRegion);

            HTuple Angle;
            HObject SrcImg_Match = Get_SourceImage_Match(); // (20200130) Jeff Revised!
            Method.PtternMatchAndSegRegion(SrcImg_Match, Recipe, out PatternRegions, out Angle, out CellRegion, out ThinRegion, out StainRegion, out ScratchRegion);

            HObject RShift_Dark, RShift_Bright;
            HOperatorSet.GenEmptyObj(out RShift_Dark);
            HOperatorSet.GenEmptyObj(out RShift_Bright);

            Method.PartitionRegions(Input_ImgList, Recipe, ThinRegion, ScratchRegion, StainRegion, out Thin_Dark, out Thin_Bright, out RDefect_Dark, out RDefect_Bright, out Stain_Dark, out Stain_Bright, out RShift_Dark, out RShift_Bright);

            HObject ThRegion_Dark, TargetE_Dark, TargetR_Dark;
            HObject ThRegion_Bright, TargetE_Bright, TargetR_Bright;
            HOperatorSet.GenEmptyObj(out ThRegion_Dark);
            HOperatorSet.GenEmptyObj(out TargetE_Dark);
            HOperatorSet.GenEmptyObj(out TargetR_Dark);

            HOperatorSet.GenEmptyObj(out ThRegion_Bright);
            HOperatorSet.GenEmptyObj(out TargetE_Bright);
            HOperatorSet.GenEmptyObj(out TargetR_Bright);
            HObject Defect;
            HOperatorSet.GenEmptyObj(out Defect);

            try
            {

                InductanceInsp.RshiftNewInsp(Input_ImgList[int.Parse(txbRDefectID.Text)], RShift_Dark, Recipe, out ThRegion_Dark, out TargetE_Dark, out TargetR_Dark);
                InductanceInsp.RshiftNewInsp(Input_ImgList[int.Parse(txbRDefectID.Text)], RShift_Bright, Recipe, out ThRegion_Bright, out TargetE_Bright, out TargetR_Bright);

                HOperatorSet.ConcatObj(TargetE_Dark, TargetE_Bright, out Defect);
                //HObject UnionCellRegion;
                //HOperatorSet.Union1(RShift_Dark, out UnionCellRegion);

                //HObject ReduceImg;
                //HOperatorSet.ReduceDomain(SrcImg_Match, UnionCellRegion, out ReduceImg);

                //HObject OpenImg;
                //HOperatorSet.GrayOpeningRect(ReduceImg, out OpenImg, 31, 31);


                //HObject AutoTgRegion;
                //HOperatorSet.AutoThreshold(OpenImg, out AutoTgRegion, Recipe.Param.RisistAOIParam.RShiftNewParam.AutoThSigma);


                //HObject ConnectionRegion1;
                //HOperatorSet.Connection(AutoTgRegion, out ConnectionRegion1);

                //HObject SelectArea;
                //HOperatorSet.SelectShape(ConnectionRegion1, out SelectArea, "area", "and", Recipe.Param.RisistAOIParam.RShiftNewParam.SelectArea, 9999999999);

                //HObject TargetE;
                //HOperatorSet.ShapeTrans(SelectArea, out TargetE, "inner_rectangle1");

                //HTuple A, C, R;
                //HOperatorSet.AreaCenter(TargetE, out A, out R, out C);

                //Defect = TargetE;



                //OpenImg.Dispose();
                //UnionCellRegion.Dispose();
                //OpenImg.Dispose();
                //AutoTgRegion.Dispose();
                //ConnectionRegion1.Dispose();
                //SelectArea.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }



            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
            HOperatorSet.DispObj(Defect, DisplayWindows.HalconWindow);
        }

        private void btnTargetRTest_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            ParamGetFromUI();
            if (Input_ImgList == null)
            { return; }
            if (Input_ImgList[int.Parse(txbRDefectID.Text)] == null)
            { return; }

            ParamGetFromUI();
            ClearDisplay(Input_ImgList[int.Parse(txbRDefectID.Text)]);
            HObject Thin_Dark, Thin_Bright;
            HOperatorSet.GenEmptyObj(out Thin_Dark);
            HOperatorSet.GenEmptyObj(out Thin_Bright);

            HObject RDefect_Dark, RDefect_Bright;
            HOperatorSet.GenEmptyObj(out RDefect_Dark);
            HOperatorSet.GenEmptyObj(out RDefect_Bright);

            HObject Stain_Dark, Stain_Bright;
            HOperatorSet.GenEmptyObj(out Stain_Dark);
            HOperatorSet.GenEmptyObj(out Stain_Bright);


            HObject CellRegion, ThinRegion, StainRegion, ScratchRegion;

            HOperatorSet.GenEmptyObj(out CellRegion);
            HOperatorSet.GenEmptyObj(out ThinRegion);
            HOperatorSet.GenEmptyObj(out StainRegion);
            HOperatorSet.GenEmptyObj(out ScratchRegion);

            HTuple Angle;
            HObject SrcImg_Match = Get_SourceImage_Match(); // (20200130) Jeff Revised!
            Method.PtternMatchAndSegRegion(SrcImg_Match, Recipe, out PatternRegions, out Angle, out CellRegion, out ThinRegion, out StainRegion, out ScratchRegion);

            HObject RShift_Dark, RShift_Bright;
            HOperatorSet.GenEmptyObj(out RShift_Dark);
            HOperatorSet.GenEmptyObj(out RShift_Bright);

            Method.PartitionRegions(Input_ImgList, Recipe, ThinRegion, ScratchRegion, StainRegion, out Thin_Dark, out Thin_Bright, out RDefect_Dark, out RDefect_Bright, out Stain_Dark, out Stain_Bright, out RShift_Dark, out RShift_Bright);


            HObject ThRegion_Dark, TargetE_Dark, TargetR_Dark;
            HObject ThRegion_Bright, TargetE_Bright, TargetR_Bright;
            HOperatorSet.GenEmptyObj(out ThRegion_Dark);
            HOperatorSet.GenEmptyObj(out TargetE_Dark);
            HOperatorSet.GenEmptyObj(out TargetR_Dark);

            HOperatorSet.GenEmptyObj(out ThRegion_Bright);
            HOperatorSet.GenEmptyObj(out TargetE_Bright);
            HOperatorSet.GenEmptyObj(out TargetR_Bright);
            HObject Defect;
            HOperatorSet.GenEmptyObj(out Defect);

            try
            {

                InductanceInsp.RshiftNewInsp(Input_ImgList[int.Parse(txbRDefectID.Text)], RShift_Dark, Recipe, out ThRegion_Dark, out TargetE_Dark, out TargetR_Dark);
                InductanceInsp.RshiftNewInsp(Input_ImgList[int.Parse(txbRDefectID.Text)], RShift_Bright, Recipe, out ThRegion_Bright, out TargetE_Bright, out TargetR_Bright);

                HOperatorSet.ConcatObj(TargetR_Dark, TargetR_Bright, out Defect);

                //HObject UnionCellRegion;
                //HOperatorSet.Union1(RShift_Dark, out UnionCellRegion);

                //HObject ReduceImg;
                //HOperatorSet.ReduceDomain(SrcImg_Match, UnionCellRegion, out ReduceImg);

                //HObject OpenImg;
                //HOperatorSet.GrayOpeningRect(ReduceImg, out OpenImg, 31, 31);


                //HObject AutoTgRegion;
                //HOperatorSet.AutoThreshold(OpenImg, out AutoTgRegion, Recipe.Param.RisistAOIParam.RShiftNewParam.AutoThSigma);

                //HObject ConnectionRegion1;
                //HOperatorSet.Connection(AutoTgRegion, out ConnectionRegion1);

                //HObject SelectArea;
                //HOperatorSet.SelectShape(ConnectionRegion1, out SelectArea, "area", "and", Recipe.Param.RisistAOIParam.RShiftNewParam.SelectArea, 9999999999);

                //HObject TargetE;
                //HOperatorSet.ShapeTrans(SelectArea, out TargetE, "inner_rectangle1");

                //HObject UnionRegion;
                //HOperatorSet.Union1(AutoTgRegion, out UnionRegion);

                //HObject DiffRegion;
                //HOperatorSet.Difference(UnionRegion, SelectArea, out DiffRegion);

                //HObject ConRegion;
                //HOperatorSet.Connection(DiffRegion, out ConRegion);

                //HObject SelectWidth;
                //HOperatorSet.SelectShape(ConRegion, out SelectWidth, "width", "and", Recipe.Param.RisistAOIParam.RShiftNewParam.SelectWidth, 9999999999);

                //HObject InterRegion;
                //HOperatorSet.Intersection(CellRegion, SelectWidth, out InterRegion);


                //Defect = InterRegion;

                //HObject TargetR;
                //HOperatorSet.ShapeTrans(InterRegion, out TargetR, "convex");

                //HObject TR, TE;

                //HOperatorSet.SortRegion(TargetR, out TR, "character", "true", "column");
                //HOperatorSet.SortRegion(TargetE, out TE, "character", "true", "column");

                //HTuple AreaE, RowE, ColumnE;
                //HTuple AreaR, RowR, ColumnR;

                //HOperatorSet.AreaCenter(TR, out AreaR, out RowR, out ColumnR);
                //HOperatorSet.AreaCenter(TE, out AreaE, out RowE, out ColumnE);



                //UnionCellRegion.Dispose();
                //ReduceImg.Dispose();
                //AutoTgRegion.Dispose();
                //OpenImg.Dispose();
                //ConnectionRegion1.Dispose();
                //SelectArea.Dispose();
                //TargetE.Dispose();
                //UnionRegion.Dispose();
                //DiffRegion.Dispose();
                //ConRegion.Dispose();
                //SelectWidth.Dispose();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "blue");
            HOperatorSet.DispObj(Defect, DisplayWindows.HalconWindow);
        }

        private void btnAutoThTest_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            if (Input_ImgList == null)
                return;

            if (Input_ImgList[int.Parse(txbRDefectID.Text)] == null)
                return;

            ParamGetFromUI();
            ClearDisplay(Input_ImgList[int.Parse(txbRDefectID.Text)]);
            HObject Thin_Dark, Thin_Bright;
            HOperatorSet.GenEmptyObj(out Thin_Dark);
            HOperatorSet.GenEmptyObj(out Thin_Bright);

            HObject RDefect_Dark, RDefect_Bright;
            HOperatorSet.GenEmptyObj(out RDefect_Dark);
            HOperatorSet.GenEmptyObj(out RDefect_Bright);

            HObject Stain_Dark, Stain_Bright;
            HOperatorSet.GenEmptyObj(out Stain_Dark);
            HOperatorSet.GenEmptyObj(out Stain_Bright);


            HObject CellRegion, ThinRegion, StainRegion, ScratchRegion;

            HOperatorSet.GenEmptyObj(out CellRegion);
            HOperatorSet.GenEmptyObj(out ThinRegion);
            HOperatorSet.GenEmptyObj(out StainRegion);
            HOperatorSet.GenEmptyObj(out ScratchRegion);

            HTuple Angle;
            HObject SrcImg_Match = Get_SourceImage_Match(); // (20200130) Jeff Revised!
            Method.PtternMatchAndSegRegion(SrcImg_Match, Recipe, out PatternRegions, out Angle, out CellRegion, out ThinRegion, out StainRegion, out ScratchRegion);

            HObject RShift_Dark, RShift_Bright;
            HOperatorSet.GenEmptyObj(out RShift_Dark);
            HOperatorSet.GenEmptyObj(out RShift_Bright);

            Method.PartitionRegions(Input_ImgList, Recipe, ThinRegion, ScratchRegion, StainRegion, out Thin_Dark, out Thin_Bright, out RDefect_Dark, out RDefect_Bright, out Stain_Dark, out Stain_Bright, out RShift_Dark, out RShift_Bright);

            HObject ThRegion_Dark, TargetE_Dark, TargetR_Dark;
            HObject ThRegion_Bright, TargetE_Bright, TargetR_Bright;
            HOperatorSet.GenEmptyObj(out ThRegion_Dark);
            HOperatorSet.GenEmptyObj(out TargetE_Dark);
            HOperatorSet.GenEmptyObj(out TargetR_Dark);

            HOperatorSet.GenEmptyObj(out ThRegion_Bright);
            HOperatorSet.GenEmptyObj(out TargetE_Bright);
            HOperatorSet.GenEmptyObj(out TargetR_Bright);

            HObject Defect;
            HOperatorSet.GenEmptyObj(out Defect);

            try
            {

                InductanceInsp.RshiftNewInsp(Input_ImgList[int.Parse(txbRDefectID.Text)], RShift_Dark, Recipe, out ThRegion_Dark, out TargetE_Dark, out TargetR_Dark);
                InductanceInsp.RshiftNewInsp(Input_ImgList[int.Parse(txbRDefectID.Text)], RShift_Bright, Recipe, out ThRegion_Bright, out TargetE_Bright, out TargetR_Bright);

                HOperatorSet.ConcatObj(ThRegion_Dark, ThRegion_Bright, out Defect);
                //HObject UnionCellRegion;
                //HOperatorSet.Union1(RShift_Dark, out UnionCellRegion);

                //HObject ReduceImg;
                //HOperatorSet.ReduceDomain(SrcImg_Match, UnionCellRegion, out ReduceImg);

                //HObject OpenImg;
                //HOperatorSet.GrayOpeningRect(ReduceImg, out OpenImg, 31, 31);

                //HObject AutoTgRegion;
                //HOperatorSet.AutoThreshold(OpenImg, out AutoTgRegion, Recipe.Param.RisistAOIParam.RShiftNewParam.AutoThSigma);

                //Defect = AutoTgRegion;
                //OpenImg.Dispose();
                //ReduceImg.Dispose();
                //UnionCellRegion.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
            HOperatorSet.SetColored(DisplayWindows.HalconWindow, 12);
            HOperatorSet.DispObj(Defect, DisplayWindows.HalconWindow);


        }

        private void btnHoleInsp_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            ClearDisplay(Input_ImgList[int.Parse(txbThinID.Text)]);
 			ParamGetFromUI();

            HObject Thin_Dark, Thin_Bright;
            HOperatorSet.GenEmptyObj(out Thin_Dark);
            HOperatorSet.GenEmptyObj(out Thin_Bright);

            HObject RDefect_Dark, RDefect_Bright;
            HOperatorSet.GenEmptyObj(out RDefect_Dark);
            HOperatorSet.GenEmptyObj(out RDefect_Bright);

            HObject Stain_Dark, Stain_Bright;
            HOperatorSet.GenEmptyObj(out Stain_Dark);
            HOperatorSet.GenEmptyObj(out Stain_Bright);


            HObject CellRegion, ThinRegion, StainRegion, ScratchRegion;

            HOperatorSet.GenEmptyObj(out CellRegion);
            HOperatorSet.GenEmptyObj(out ThinRegion);
            HOperatorSet.GenEmptyObj(out StainRegion);
            HOperatorSet.GenEmptyObj(out ScratchRegion);

            HTuple Angle;
            HObject SrcImg_Match = Get_SourceImage_Match(); // (20200130) Jeff Revised!
            Method.PtternMatchAndSegRegion(SrcImg_Match, Recipe, out PatternRegions, out Angle, out CellRegion, out ThinRegion, out StainRegion, out ScratchRegion);
            HObject RShift_Dark, RShift_Bright;
            HOperatorSet.GenEmptyObj(out RShift_Dark);
            HOperatorSet.GenEmptyObj(out RShift_Bright);

            Method.PartitionRegions(Input_ImgList, Recipe, ThinRegion, ScratchRegion, StainRegion, out Thin_Dark, out Thin_Bright, out RDefect_Dark, out RDefect_Bright, out Stain_Dark, out Stain_Bright, out RShift_Dark, out RShift_Bright);

            HObject ThinDefectRegions, MapRegion;
            HOperatorSet.GenEmptyObj(out ThinDefectRegions);
			InductanceInsp.ThinParam ThinParam = new InductanceInsp.ThinParam(Input_ImgList[Recipe.Param.RisistAOIParam.ThinParam.InspID], Recipe, CellRegion, Thin_Dark, Thin_Bright);
            var ThinTask = new Task<InductanceInsp.clsThinInspResult>(InductanceInsp.ThinInspTask, ThinParam);
            ThinTask.Start();

            ThinDefectRegions = ThinTask.Result.HoleRes;
            Method.LocalDefectMappping(ThinDefectRegions, CellRegion, out MapRegion);

            ThinTask.Dispose();
            if (!Recipe.Param.RisistAOIParam.ThinParam.DefectSumAreaEnabled)
                HOperatorSet.Connection(ThinDefectRegions, out SelectRegions);
 			else
            {
                SelectRegions.Dispose();
                SelectRegions = ThinDefectRegions.Clone();
            }
            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
			HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
            HOperatorSet.DispObj(ThinDefectRegions, DisplayWindows.HalconWindow);
            Method.Convert2Circle(MapRegion, out MapRegion);            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "green");
            HOperatorSet.DispObj(MapRegion, DisplayWindows.HalconWindow);

            DrawImg(ThinDefectRegions, MapRegion, int.Parse(txbThinID.Text));
        }

        /// <summary>
        /// 【設定】 (二值化)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMethodThSet_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            // 檢查是否已讀取影像
            if (!Check_Input_ImgList())
                return;

            HObject SrcImg_R, SrcImg_G, SrcImg_B;

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out SrcImg_R);
            HOperatorSet.GenEmptyObj(out SrcImg_G);
            HOperatorSet.GenEmptyObj(out SrcImg_B);
            
            HObject TeachImg;

            HOperatorSet.GenEmptyObj(out TeachImg);

            enu_ImageSource ImageSource; // (20200130) Jeff Revised!
            if (radioButton_OrigImg_RegMethod.Checked)
                ImageSource = enu_ImageSource.原始;
            else
                ImageSource = enu_ImageSource.影像A;
            HObject SrcImg = Get_SourceImage(ImageSource, int.Parse(nudMethodThImageID.Value.ToString()));

            HObject GoldenRegion, ReduceImage;

            HOperatorSet.GenRectangle1(out GoldenRegion, Recipe.Param.SegParam.CellRow1, Recipe.Param.SegParam.CellColumn1, Recipe.Param.SegParam.CellRow2, Recipe.Param.SegParam.CellColumn2);

            HOperatorSet.ReduceDomain(SrcImg, GoldenRegion, out ReduceImage);

            TeachImg = clsStaticTool.GetChannelImage(ReduceImage, (enuBand)combxMethodThBand.SelectedIndex);

            FormThresholdAdjust MyForm = new FormThresholdAdjust(TeachImg,
                                                                 int.Parse(nudMethodThMin.Value.ToString()),
                                                                 int.Parse(nudMethodThMax.Value.ToString()), FormThresholdAdjust.enType.Dual);

            MyForm.ShowDialog();

            if (MyForm.DialogResult == DialogResult.OK)
            {
                int ThMin, ThMax;
                MyForm.GetThreshold(out ThMin, out ThMax);

                nudMethodThMin.Value = decimal.Parse(ThMin.ToString());
                nudMethodThMax.Value = decimal.Parse(ThMax.ToString());
            }

            TeachImg.Dispose();
            //SrcImg.Dispose();
            SrcImg_R.Dispose();
            SrcImg_G.Dispose();
            SrcImg_B.Dispose();
            ReduceImage.Dispose();
        }

        /// <summary>
        /// 【測試】 (二值化)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMethodThTest_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            // 檢查是否已讀取影像
            if (!Check_Input_ImgList())
                return;

            try
            {
                ParamGetFromUI();
                HObject GoldenRegion, ReduceImage;

                HOperatorSet.GenRectangle1(out GoldenRegion, Recipe.Param.SegParam.CellRow1, Recipe.Param.SegParam.CellColumn1, Recipe.Param.SegParam.CellRow2, Recipe.Param.SegParam.CellColumn2);

                enu_ImageSource ImageSource; // (20200130) Jeff Revised!
                if (radioButton_OrigImg_RegMethod.Checked)
                    ImageSource = enu_ImageSource.原始;
                else
                    ImageSource = enu_ImageSource.影像A;
                HObject SrcImg = Get_SourceImage(ImageSource, int.Parse(nudMethodThImageID.Value.ToString()));
                
                ClearDisplay(SrcImg);

                HOperatorSet.ReduceDomain(SrcImg, GoldenRegion, out ReduceImage);

                HObject Thregion;
                HTuple Min, Max;
                Min = int.Parse(nudMethodThMin.Value.ToString());
                Max = int.Parse(nudMethodThMax.Value.ToString());

                HObject Image = clsStaticTool.GetChannelImage(ReduceImage, (enuBand)combxMethodThBand.SelectedIndex);

                HOperatorSet.Threshold(Image, out Thregion, Min, Max);

                HOperatorSet.SetDraw(DisplayWindows.HalconWindow, DisplayType.ToString());
                HOperatorSet.SetColor(DisplayWindows.HalconWindow, "blue");
                HOperatorSet.DispObj(Thregion, DisplayWindows.HalconWindow);

                GoldenRegion.Dispose();
                ReduceImage.Dispose();
                Thregion.Dispose();
                Image.Dispose();
            }
            catch (Exception E)
            {
                MessageBox.Show("錯誤! ");
                MessageBox.Show("錯誤訊息: " + E.ToString());
            }
        }

        /// <summary>
        /// 來源影像切換 (檢測範圍設定)，調整【影像ID:】控制項上限
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_Img_RegMethod_CheckedChanged(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            RadioButton rbtn = sender as RadioButton;
            if (rbtn.Checked == false) return;

            /*
            // 檢查是否已讀取影像
            if (!Check_Input_ImgList())
            {
                this.radioButton_OrigImg_RegMethod.CheckedChanged -= new System.EventHandler(this.radioButton_Img_RegMethod_CheckedChanged);
                radioButton_OrigImg_RegMethod.Checked = true;
                this.radioButton_OrigImg_RegMethod.CheckedChanged += new System.EventHandler(this.radioButton_Img_RegMethod_CheckedChanged);
                return;
            }
            */

            // 調整【影像ID:】控制項上限
            string Tag = rbtn.Tag.ToString();
            if (Tag == enu_ImageSource.原始.ToString())
                nudMethodThImageID.Maximum = Recipe.Param.ImgCount - 1;
            else if (Tag == enu_ImageSource.影像A.ToString())
            {
                if (Recipe.Param.AlgoImg.ListAlgoImage.Count > 0)
                    nudMethodThImageID.Maximum = Recipe.Param.AlgoImg.ListAlgoImage.Count - 1;
                else
                {
                    MessageBox.Show("影像A數量為0", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    radioButton_OrigImg_RegMethod.Checked = true; // 原始影像
                }
            }
        }

        /// <summary>
        /// 【頻帶:】 (檢測範圍設定)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void combxMethodThBand_SelectedIndexChanged(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            if (b_LoadRecipe) // (20200130) Jeff Revised!
                return;

            if (combxMethodThBand.SelectedIndex < 0 || Input_ImgList.Count <= 0)
                return;
            
            // 檢查是否已讀取影像
            if (!Check_Input_ImgList())
                return;

            try
            {
                enu_ImageSource ImageSource; // (20200130) Jeff Revised!
                if (radioButton_OrigImg_RegMethod.Checked)
                    ImageSource = enu_ImageSource.原始;
                else
                    ImageSource = enu_ImageSource.影像A;
                HObject SrcImg = Get_SourceImage(ImageSource, int.Parse(nudMethodThImageID.Value.ToString()));
                
                HObject DisplayImage = clsStaticTool.GetChannelImage(SrcImg, (enuBand)combxMethodThBand.SelectedIndex);
                HOperatorSet.ClearWindow(DisplayWindows.HalconWindow);
                HOperatorSet.DispObj(DisplayImage, DisplayWindows.HalconWindow);
                DisplayImage.Dispose();

                //HTuple Ch;
                //HOperatorSet.CountChannels(Input_ImgList[combxDisplayImg.SelectedIndex], out Ch);
                //if (Ch == 1)
                //{
                //    return;
                //}
                //HObject SrcImg_R, SrcImg_G, SrcImg_B;

                //if (combxMethodThBand.SelectedIndex == 0)
                //{
                //    HOperatorSet.AccessChannel(Input_ImgList[combxDisplayImg.SelectedIndex], out SrcImg_R, 1);
                //    HOperatorSet.DispObj(SrcImg_R, DisplayWindows.HalconWindow);
                //    SrcImg_R.Dispose();
                //}
                //else if (combxMethodThBand.SelectedIndex == 1)
                //{
                //    HOperatorSet.AccessChannel(Input_ImgList[combxDisplayImg.SelectedIndex], out SrcImg_G, 2);
                //    HOperatorSet.DispObj(SrcImg_G, DisplayWindows.HalconWindow);
                //    SrcImg_G.Dispose();
                //}
                //else if (combxMethodThBand.SelectedIndex == 2)
                //{
                //    HOperatorSet.AccessChannel(Input_ImgList[combxDisplayImg.SelectedIndex], out SrcImg_B, 3);
                //    HOperatorSet.DispObj(SrcImg_B, DisplayWindows.HalconWindow);
                //    SrcImg_B.Dispose();
                //}
                //else
                //{
                //    HObject GrayImg;
                //    HOperatorSet.Rgb1ToGray(Input_ImgList[combxDisplayImg.SelectedIndex], out GrayImg);
                //    HOperatorSet.DispObj(GrayImg, DisplayWindows.HalconWindow);
                //    GrayImg.Dispose();
                //}
            }
            catch (Exception ex)
            {
                string message = "Error: " + ex.Message;
                MessageBox.Show(message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 【新增】 (二值化)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMethodThAdd_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            // 檢查是否已讀取影像
            if (!Check_Input_ImgList())
                return;

            try
            {

                #region 

                ParamGetFromUI();
                HObject GoldenRegion,ReduceImage;

                HOperatorSet.GenRectangle1(out GoldenRegion, Recipe.Param.SegParam.CellRow1, Recipe.Param.SegParam.CellColumn1, Recipe.Param.SegParam.CellRow2, Recipe.Param.SegParam.CellColumn2);

                enu_ImageSource ImageSource; // (20200130) Jeff Revised!
                if (radioButton_OrigImg_RegMethod.Checked)
                    ImageSource = enu_ImageSource.原始;
                else
                    ImageSource = enu_ImageSource.影像A;
                HObject SrcImg = Get_SourceImage(ImageSource, int.Parse(nudMethodThImageID.Value.ToString()));
                
                ClearDisplay(SrcImg);

                HOperatorSet.ReduceDomain(SrcImg, GoldenRegion, out ReduceImage);

                GoldenRegion.Dispose();

                #endregion


                HObject Thregion;
                HTuple Min, Max;
                Min = int.Parse(nudMethodThMin.Value.ToString());
                Max = int.Parse(nudMethodThMax.Value.ToString());

                HObject Image = clsStaticTool.GetChannelImage(ReduceImage, (enuBand)combxMethodThBand.SelectedIndex);
                ReduceImage.Dispose();

                HOperatorSet.Threshold(Image, out Thregion, Min, Max);
                Image.Dispose();

                DialogResult dialogResult = MessageBox.Show("確認是否新增?", "提示!", MessageBoxButtons.YesNo);

                if (dialogResult != DialogResult.Yes)
                {
                    return;
                }
                string RegionName = "ThRegion";
                WorkSheetForm MyForm = new WorkSheetForm("Name", 1);

                MyForm.ShowDialog();

                if (!MyForm.Saved)
                    RegionName = "ThRegion";
                else
                    RegionName = MyForm.NewRecipeName;


                bool IsMemberExists = Recipe.Param.MethodRegionList.Exists(x => x.Name == RegionName);

                if (IsMemberExists)
                {
                    MessageBox.Show("存在相同名稱,請重新命名");
                    return;
                }

                HTuple Area, row, column;
                HOperatorSet.AreaCenter(Thregion, out Area, out row, out column);
                clsRecipe.clsMethodRegion MethodRegion = new clsRecipe.clsMethodRegion();
                MethodRegion.HotspotY = Recipe.Param.SegParam.GoldenCenterY - row;
                MethodRegion.HotspotX = Recipe.Param.SegParam.GoldenCenterX - column;
                MethodRegion.Row = row;
                MethodRegion.Column = column;
                MethodRegion.Name = RegionName;
                MethodRegion.RegionPath = Recipe.MethodRegionPath + RegionName;
                MethodRegion.Region = Thregion;
                Recipe.Param.MethodRegionList.Add(MethodRegion);

                UpdateMethodListView(Recipe.Param.MethodRegionList);
            }
            catch (Exception E)
            {
                MessageBox.Show("錯誤! ");
                MessageBox.Show("錯誤訊息: " + E.ToString());
            }
        }
        HObject AdjustRegion = null;
        private void listViewMethod_SelectedIndexChanged(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            // 檢查是否已讀取影像
            if (!Check_Input_ImgList())
                return;

            if (listViewMethod.SelectedIndices.Count <= 0)
                return;
            ParamGetFromUI();
            try
            {
                HTuple Row = null, Column = null, Angle = null, Score = null;

                HObject PatternRegions, CellRegion;
                HObject SrcImg_Match = Get_SourceImage_Match(); // (20200130) Jeff Revised!
                InductanceInsp.PtternMatchAndSegRegion_New(SrcImg_Match, Recipe, out PatternRegions, out Angle, out Row, out Column, out CellRegion);
                
                //if (SrcImg_Match != null)
                //{
                //    InductanceInsp.GetMatchInfo(SrcImg_Match, Recipe, out Row, out Column, out Angle, out Score);
                //}

                //if (Score.Length <= 0)
                //{
                //    MessageBox.Show("未搜尋到此特徵,可能為分數、角度、亮暗差異過大");
                //    return;
                //}

                HObject LoadRegion = Recipe.Param.MethodRegionList[listViewMethod.SelectedIndices[0]].Region;

                HTuple hv_HomMat2D;
                HObject Region;
                HOperatorSet.GenEmptyObj(out Region);
                HObject DisplayRegion;
                HOperatorSet.GenEmptyObj(out DisplayRegion);

                HTuple hv_RefRow, hv_RefColumn, hv_ModelRegionArea;
                HOperatorSet.AreaCenter(LoadRegion, out hv_ModelRegionArea, out hv_RefRow, out hv_RefColumn);

                HTuple Pat_A, Pat_R, Pat_C;
                HOperatorSet.AreaCenter(PatternRegions, out Pat_A, out Pat_R, out Pat_C);

                for (int i = 0; i < (int)((new HTuple(Angle.TupleLength()))); i++)
                {
                    HOperatorSet.HomMat2dIdentity(out hv_HomMat2D);
                    //HOperatorSet.HomMat2dTranslate(hv_HomMat2D, -Recipe.Param.SegParam.GoldenCenterY / Recipe.Param.AdvParam.MatchSacleSize, -Recipe.Param.SegParam.GoldenCenterX / Recipe.Param.AdvParam.MatchSacleSize, out hv_HomMat2D);
                    HOperatorSet.HomMat2dTranslate(hv_HomMat2D, -hv_RefRow, -hv_RefColumn, out hv_HomMat2D);
                    HOperatorSet.HomMat2dRotate(hv_HomMat2D, Angle.TupleSelect(i), 0, 0, out hv_HomMat2D);


                    HTuple cosA, sinA;
                    HOperatorSet.TupleCos(Angle, out cosA);
                    HOperatorSet.TupleSin(Angle, out sinA);
                    HTuple HX = Recipe.Param.MethodRegionList[listViewMethod.SelectedIndices[0]].HotspotX * cosA[i] + Recipe.Param.MethodRegionList[listViewMethod.SelectedIndices[0]].HotspotY * sinA[i];
                    HTuple HY = -Recipe.Param.MethodRegionList[listViewMethod.SelectedIndices[0]].HotspotX * sinA[i] + Recipe.Param.MethodRegionList[listViewMethod.SelectedIndices[0]].HotspotY * cosA[i];

                    HTuple ShiftRow = Pat_R.TupleSelect(i) - HY;
                    HTuple ShiftColumn = Pat_C.TupleSelect(i) - HX;


                    //HTuple ShiftRow = Pat_R.TupleSelect(i) - Recipe.Param.MethodRegionList[listViewMethod.SelectedIndices[0]].HotspotY;
                    //HTuple ShiftColumn = Pat_C.TupleSelect(i) - Recipe.Param.MethodRegionList[listViewMethod.SelectedIndices[0]].HotspotX;
                    HOperatorSet.HomMat2dTranslate(hv_HomMat2D, ShiftRow, ShiftColumn, out hv_HomMat2D);


                    Region.Dispose();
                    HOperatorSet.AffineTransRegion(LoadRegion, out Region, hv_HomMat2D, "nearest_neighbor");

                    HObject Un1;
                    HOperatorSet.Union1(Region, out Un1);

                    HTuple row1, row2, column1, column2;
                    HOperatorSet.RegionFeatures(Un1, "row1", out row1);
                    HOperatorSet.RegionFeatures(Un1, "row2", out row2);
                    HOperatorSet.RegionFeatures(Un1, "column1", out column1);
                    HOperatorSet.RegionFeatures(Un1, "column2", out column2);
                    if (!Recipe.Param.bInspOutboundary)
                    {
                        if (row1 < 0 || column1 < 0 || row2 > Recipe.Param.ImageHeight || column2 > Recipe.Param.ImageWidth)
                            continue;
                    }

                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(DisplayRegion, Un1, out ExpTmpOutVar_0);
                        DisplayRegion.Dispose();
                        DisplayRegion = ExpTmpOutVar_0;
                    }
                }

                ClearDisplay(Input_ImgList[combxDisplayImg.SelectedIndex]);

                if (AdjustRegion != null)
                {
                    AdjustRegion.Dispose();
                    AdjustRegion = null;
                    HOperatorSet.GenEmptyObj(out AdjustRegion);
                }
                AdjustRegion = DisplayRegion.Clone();
                HOperatorSet.SetDraw(DisplayWindows.HalconWindow, DisplayType.ToString());
                HOperatorSet.SetColored(DisplayWindows.HalconWindow, 12);
                HOperatorSet.DispObj(DisplayRegion, DisplayWindows.HalconWindow);
            }
            catch(Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        /// <summary>
        /// 【新增】 (自訂範圍)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxCustomizeAdd_CheckedChanged(object sender, EventArgs e) // (20200429) Jeff Revised!
        {
            CheckBox Obj = (CheckBox)sender;

            #region 確認是否可執行

            if (Input_ImgList == null)
            {
                Obj.CheckedChanged -= cbxCustomizeAdd_CheckedChanged;
                Obj.Checked = false;
                Obj.CheckedChanged += cbxCustomizeAdd_CheckedChanged;
                MessageBox.Show("請先讀取影像");
                return;
            }

            if (Input_ImgList.Count <= 0)
            {
                Obj.CheckedChanged -= cbxCustomizeAdd_CheckedChanged;
                Obj.Checked = false;
                Obj.CheckedChanged += cbxCustomizeAdd_CheckedChanged;
                MessageBox.Show("請先讀取影像");
                return;
            }

            if (combxCustomizeType.SelectedIndex < 0) // (20200429) Jeff Revised!
            {
                Obj.CheckedChanged -= cbxCustomizeAdd_CheckedChanged;
                Obj.Checked = false;
                Obj.CheckedChanged += cbxCustomizeAdd_CheckedChanged;
                MessageBox.Show("尚未選擇Type");
                return;
            }

            #endregion

            enu_ImageSource ImageSource; // (20200130) Jeff Revised!
            if (radioButton_OrigImg_RegMethod.Checked)
                ImageSource = enu_ImageSource.原始;
            else
                ImageSource = enu_ImageSource.影像A;
            HObject SrcImg = Get_SourceImage(ImageSource, int.Parse(nudMethodThImageID.Value.ToString()));
            
            ClearDisplay(SrcImg);
            
            if (Obj.Checked)
            {
                #region 設定範圍
                try
                {
                    clsStaticTool.EnableAllControls(this.tpUserSet, Obj, false);
                    ChangeColor(Color.Green, Obj);
                    if (drawing_Rect != null)
                    {
                        drawing_Rect.Dispose();
                        drawing_Rect = null;
                    }
                    HTuple ImgWidth, ImgHeight;
                    HOperatorSet.GetImageSize(SrcImg, out ImgWidth, out ImgHeight);

                    {
                        HDrawingObject.HDrawingObjectType Type = (HDrawingObject.HDrawingObjectType)combxCustomizeType.SelectedIndex;
                        drawing_Rect = GetDrawObj(Type, Recipe);
                    }

                    drawing_Rect.SetDrawingObjectParams("color", "green");
                    DisplayWindows.HalconWindow.AttachDrawingObjectToWindow(drawing_Rect);
                }
                catch (Exception ex)
                {
                    Obj.CheckedChanged -= cbxCustomizeAdd_CheckedChanged;
                    Obj.Checked = false;
                    Obj.CheckedChanged += cbxCustomizeAdd_CheckedChanged;
                    drawing_Rect.Dispose();
                    drawing_Rect = null;
                    clsStaticTool.EnableAllControls(this.tpUserSet, Obj, true);
                    ChangeColor(Color.LightGray, Obj);
                    MessageBox.Show(ex.ToString());
                }
                #endregion
            }
            else
            {
                #region 寫入設定
                try
                {
                    HDrawingObject.HDrawingObjectType Type = (HDrawingObject.HDrawingObjectType)combxCustomizeType.SelectedIndex;

                    HObject Region;
                    HTuple CenterX, CenterY;
                    clsStaticTool.GetDrawRegionRes(drawing_Rect.ID, Type, out Region, out CenterX, out CenterY);
                    
                    DialogResult dialogResult = MessageBox.Show("確認是否新增?", "提示!", MessageBoxButtons.YesNo);

                    if (dialogResult != DialogResult.Yes)
                    {
                        Region.Dispose();
                        Obj.CheckedChanged -= cbxCustomizeAdd_CheckedChanged;
                        Obj.Checked = false;
                        Obj.CheckedChanged += cbxCustomizeAdd_CheckedChanged;
                        drawing_Rect.Dispose();
                        drawing_Rect = null;
                        clsStaticTool.EnableAllControls(this.tpUserSet, Obj, true);
                        ChangeColor(Color.LightGray, Obj);
                        return;
                    }
                    string RegionName = "CustomizeRegion";
                    WorkSheetForm MyForm = new WorkSheetForm("Name", 1);

                    MyForm.ShowDialog();

                    if (!MyForm.Saved)
                        RegionName = "CustomizeRegion";
                    else
                        RegionName = MyForm.NewRecipeName;

                    bool IsMemberExists = Recipe.Param.MethodRegionList.Exists(x => x.Name == RegionName);

                    if (IsMemberExists)
                    {
                        MessageBox.Show("存在相同名稱,請重新命名");
                        return;
                    }

                    clsRecipe.clsMethodRegion MethodRegion = new clsRecipe.clsMethodRegion();

                    MethodRegion.HotspotY = Recipe.Param.SegParam.GoldenCenterY - CenterY;
                    MethodRegion.HotspotX = Recipe.Param.SegParam.GoldenCenterX - CenterX;
                    MethodRegion.Row = CenterY;
                    MethodRegion.Column = CenterX;
                    MethodRegion.Name = RegionName;
                    MethodRegion.RegionPath = Recipe.MethodRegionPath + RegionName;
                    MethodRegion.Region = Region;
                    Recipe.Param.MethodRegionList.Add(MethodRegion);

                    UpdateMethodListView(Recipe.Param.MethodRegionList);

                    drawing_Rect.Dispose();
                    drawing_Rect = null;
                    ChangeColor(Color.LightGray, Obj);
                    clsStaticTool.EnableAllControls(this.tpUserSet, Obj, true);
                }
                catch (Exception ex)
                {
                    Obj.CheckedChanged -= cbxCustomizeAdd_CheckedChanged;
                    Obj.Checked = false;
                    Obj.CheckedChanged += cbxCustomizeAdd_CheckedChanged;
                    drawing_Rect.Dispose();
                    drawing_Rect = null;
                    clsStaticTool.EnableAllControls(this.tpUserSet, Obj, true);
                    ChangeColor(Color.LightGray, Obj);
                    MessageBox.Show(ex.ToString());
                }
                #endregion
            }
        }

        private void btnRemoveMethodRegion_Click(object sender, EventArgs e)
        {
            if (listViewMethod.SelectedIndices.Count <= 0)
            {
                MessageBox.Show("請先選擇要移除之Region");
                return;
            }
            if (Recipe.Param.MethodRegionList.Count < listViewMethod.SelectedIndices[0])
                return;

            try
            {
                DialogResult dialogResult = MessageBox.Show("確認是否移除此Region?", "警告!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dialogResult != DialogResult.Yes)
                {
                    return;
                }

                Recipe.Param.MethodRegionList[listViewMethod.SelectedIndices[0]].Region.Dispose();

                Recipe.Param.MethodRegionList.RemoveAt(listViewMethod.SelectedIndices[0]);

                UpdateMethodListView(Recipe.Param.MethodRegionList);
            }
            catch(Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        private void btnOpenEditRegionForm_Click(object sender, EventArgs e)
        {
            // 檢查是否已讀取影像
            if (!Check_Input_ImgList())
                return;

            FormEditRegion MyForm = new FormEditRegion(Input_ImgList, Recipe.Param.MethodRegionList, Recipe.Param.EditRegionList, Recipe);
            MyForm.ShowDialog();

            UpdateEditListView(Recipe.Param.EditRegionList);
        }

        /// <summary>
        /// 【測試】 (Dual 二值化)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDualThTest_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            // 檢查是否已讀取影像
            if (!Check_Input_ImgList())
                return;

            try
            {
                ParamGetFromUI();
                HObject GoldenRegion, ReduceImage;

                HOperatorSet.GenRectangle1(out GoldenRegion, Recipe.Param.SegParam.CellRow1, Recipe.Param.SegParam.CellColumn1, Recipe.Param.SegParam.CellRow2, Recipe.Param.SegParam.CellColumn2);

                enu_ImageSource ImageSource; // (20200130) Jeff Revised!
                if (radioButton_OrigImg_RegMethod.Checked)
                    ImageSource = enu_ImageSource.原始;
                else
                    ImageSource = enu_ImageSource.影像A;
                HObject SrcImg = Get_SourceImage(ImageSource, int.Parse(nudMethodThImageID.Value.ToString()));
                
                ClearDisplay(SrcImg);

                HOperatorSet.ReduceDomain(SrcImg, GoldenRegion, out ReduceImage);
                GoldenRegion.Dispose();

                HObject Thregion;
                HTuple MinSize, MinGray,Th;
                MinSize = int.Parse(nudDualThMinSize.Value.ToString());
                MinGray = int.Parse(nudDualThMinGray.Value.ToString());
                Th = int.Parse(nudDualThThreshold.Value.ToString());

                HObject Image = clsStaticTool.GetChannelImage(ReduceImage, (enuBand)combxMethodThBand.SelectedIndex);
                ReduceImage.Dispose();

                HOperatorSet.DualThreshold(Image, out Thregion, MinSize, MinGray, Th);

                HOperatorSet.SetDraw(DisplayWindows.HalconWindow, DisplayType.ToString());
                HOperatorSet.SetColored(DisplayWindows.HalconWindow, 12);
                HOperatorSet.DispObj(Thregion, DisplayWindows.HalconWindow);

                Thregion.Dispose();
                Image.Dispose();
            }
            catch (Exception E)
            {
                MessageBox.Show("錯誤! ");
                MessageBox.Show("錯誤訊息: " + E.ToString());
            }
        }

        /// <summary>
        /// 【新增】 (Dual 二值化)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDualThAdd_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            // 檢查是否已讀取影像
            if (!Check_Input_ImgList())
                return;

            try
            {

                #region 

                ParamGetFromUI();
                HObject GoldenRegion, ReduceImage;

                HOperatorSet.GenRectangle1(out GoldenRegion, Recipe.Param.SegParam.CellRow1, Recipe.Param.SegParam.CellColumn1, Recipe.Param.SegParam.CellRow2, Recipe.Param.SegParam.CellColumn2);

                enu_ImageSource ImageSource; // (20200130) Jeff Revised!
                if (radioButton_OrigImg_RegMethod.Checked)
                    ImageSource = enu_ImageSource.原始;
                else
                    ImageSource = enu_ImageSource.影像A;
                HObject SrcImg = Get_SourceImage(ImageSource, int.Parse(nudMethodThImageID.Value.ToString()));
                
                ClearDisplay(SrcImg);

                HOperatorSet.ReduceDomain(SrcImg, GoldenRegion, out ReduceImage);
                GoldenRegion.Dispose();

                #endregion
                
                HObject Thregion;
                HTuple MinSize, MinGray, Th;
                MinSize = int.Parse(nudDualThMinSize.Value.ToString());
                MinGray = int.Parse(nudDualThMinGray.Value.ToString());
                Th = int.Parse(nudDualThThreshold.Value.ToString());

                HObject Image = clsStaticTool.GetChannelImage(ReduceImage, (enuBand)combxMethodThBand.SelectedIndex);
                ReduceImage.Dispose();

                HOperatorSet.DualThreshold(Image, out Thregion, MinSize, MinGray, Th);
                Image.Dispose();

                DialogResult dialogResult = MessageBox.Show("確認是否新增?", "提示!", MessageBoxButtons.YesNo);

                if (dialogResult != DialogResult.Yes)
                {
                    return;
                }
                string RegionName = "DualThRegion";
                WorkSheetForm MyForm = new WorkSheetForm("Name", 1);

                MyForm.ShowDialog();

                if (!MyForm.Saved)
                    RegionName = "DualThRegion";
                else
                    RegionName = MyForm.NewRecipeName;

                bool IsMemberExists = Recipe.Param.MethodRegionList.Exists(x => x.Name == RegionName);

                if (IsMemberExists)
                {
                    MessageBox.Show("存在相同名稱,請重新命名");
                    return;
                }

                HTuple RegionCount;
                HOperatorSet.CountObj(Thregion, out RegionCount);

                if (RegionCount > 1)
                {
                    HOperatorSet.Union1(Thregion, out Thregion);
                }

                HTuple Area, row, column;
                HOperatorSet.AreaCenter(Thregion, out Area, out row, out column);
                clsRecipe.clsMethodRegion MethodRegion = new clsRecipe.clsMethodRegion();

                MethodRegion.HotspotY = Recipe.Param.SegParam.GoldenCenterY - row;
                MethodRegion.HotspotX = Recipe.Param.SegParam.GoldenCenterX - column;
                MethodRegion.Row = row;
                MethodRegion.Column = column;
                MethodRegion.Name = RegionName;
                MethodRegion.RegionPath = Recipe.MethodRegionPath + RegionName;
                MethodRegion.Region = Thregion;
                Recipe.Param.MethodRegionList.Add(MethodRegion);

                UpdateMethodListView(Recipe.Param.MethodRegionList);
            }
            catch (Exception E)
            {
                MessageBox.Show("錯誤! ");
                MessageBox.Show("錯誤訊息: " + E.ToString());
            }
        }

        private void listViewEditRegion_SelectedIndexChanged(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            // 檢查是否已讀取影像
            if (!Check_Input_ImgList())
                return;

            if (listViewEditRegion.SelectedIndices.Count <= 0)
                return;

            ParamGetFromUI();
            try
            {
                HTuple Row = null, Column = null, Angle = null, Score = null;
                HObject SrcImg_Match = Get_SourceImage_Match(); // (20200130) Jeff Revised!
                //InductanceInsp.GetMatchInfo(SrcImg_Match, Recipe, out Row, out Column, out Angle, out Score);

                HObject DisplayRegion, CellRegion;
                InductanceInsp.PtternMatchAndSegRegion_New(SrcImg_Match, Recipe, out PatternRegions, out Angle,out Row,out Column, out CellRegion);
                if (Angle.Length <= 0)
                {
                    MessageBox.Show("未搜尋到此特徵,可能為分數、角度、亮暗差異過大");
                    return;
                }

                //InductanceInsp.GetCellRegion(Row, Column, Angle, Score, Recipe, out CellRegion);
                if (!InductanceInsp.GetSelectEditRegion(PatternRegions,Row, Column, Angle, Score, Recipe, listViewEditRegion.SelectedIndices[0], CellRegion, out DisplayRegion))
                {
                    MessageBox.Show("Fail");
                    return;
                }
                
                ClearDisplay(Input_ImgList[combxDisplayImg.SelectedIndex]);

                HOperatorSet.SetDraw(DisplayWindows.HalconWindow, DisplayType.ToString());
                HOperatorSet.SetColored(DisplayWindows.HalconWindow, 12);
                HOperatorSet.DispObj(DisplayRegion, DisplayWindows.HalconWindow);

            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        private void btnEditRegionRemove_Click(object sender, EventArgs e)
        {
            if (listViewEditRegion.SelectedIndices.Count <= 0)
            {
                MessageBox.Show("請先選擇要移除之Region");
                return;
            }
            if (Recipe.Param.EditRegionList.Count < listViewEditRegion.SelectedIndices[0])
                return;

            try
            {
                DialogResult dialogResult = MessageBox.Show("確認是否移除此Region?", "警告!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dialogResult != DialogResult.Yes)
                {
                    return;
                }

                Recipe.Param.EditRegionList[listViewEditRegion.SelectedIndices[0]].Region.Dispose();

                Recipe.Param.EditRegionList.RemoveAt(listViewEditRegion.SelectedIndices[0]);

                UpdateEditListView(Recipe.Param.EditRegionList);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        /// <summary>
        /// 顯示Region型態 (填滿 or 輪廓)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxDisplayType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox Obj = (ComboBox)sender;
            if (Obj.SelectedIndex == 0)
                DisplayType = enuDisplayRegionType.fill;
            else
                DisplayType = enuDisplayRegionType.margin;
        }

        /// <summary>
        /// 【測試】 (自動二值化)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            // 檢查是否已讀取影像
            if (!Check_Input_ImgList())
                return;

            try
            {
                ParamGetFromUI();
                comboBoxAutoThSelect.Items.Clear();
                HObject GoldenRegion, ReduceImage;

                HOperatorSet.GenRectangle1(out GoldenRegion, Recipe.Param.SegParam.CellRow1, Recipe.Param.SegParam.CellColumn1, Recipe.Param.SegParam.CellRow2, Recipe.Param.SegParam.CellColumn2);

                enu_ImageSource ImageSource; // (20200130) Jeff Revised!
                if (radioButton_OrigImg_RegMethod.Checked)
                    ImageSource = enu_ImageSource.原始;
                else
                    ImageSource = enu_ImageSource.影像A;
                HObject SrcImg = Get_SourceImage(ImageSource, int.Parse(nudMethodThImageID.Value.ToString()));
                
                ClearDisplay(SrcImg);

                HOperatorSet.ReduceDomain(SrcImg, GoldenRegion, out ReduceImage);
                GoldenRegion.Dispose();

                HTuple sigma;
                sigma = double.Parse(nudAutoThSigma.Value.ToString());


                HObject Image = clsStaticTool.GetChannelImage(ReduceImage, (enuBand)combxMethodThBand.SelectedIndex);
                ReduceImage.Dispose();

                HOperatorSet.AutoThreshold(Image, out AutoRegion, sigma);

                HTuple Count;
                HOperatorSet.CountObj(AutoRegion, out Count);
                for (int i = 1; i <= Count; i++)
                {
                    comboBoxAutoThSelect.Items.Add(i.ToString());
                }

                HOperatorSet.SetDraw(DisplayWindows.HalconWindow, DisplayType.ToString());
                HOperatorSet.SetColored(DisplayWindows.HalconWindow, 12);
                HOperatorSet.DispObj(AutoRegion, DisplayWindows.HalconWindow);

                Image.Dispose();
            }
            catch (Exception E)
            {
                MessageBox.Show("錯誤! ");
                MessageBox.Show("錯誤訊息: " + E.ToString());
            }
        }

        /// <summary>
        /// 【選取範圍 : 】 (自動二值化)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxAutoThSelect_SelectedIndexChanged(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            enu_ImageSource ImageSource; // (20200130) Jeff Revised!
            if (radioButton_OrigImg_RegMethod.Checked)
                ImageSource = enu_ImageSource.原始;
            else
                ImageSource = enu_ImageSource.影像A;
            HObject SrcImg = Get_SourceImage(ImageSource, int.Parse(nudMethodThImageID.Value.ToString()));

            ClearDisplay(SrcImg);
            HObject Select;
            HOperatorSet.SelectObj(AutoRegion, out Select, comboBoxAutoThSelect.SelectedIndex + 1);

            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, DisplayType.ToString());
            HOperatorSet.SetColored(DisplayWindows.HalconWindow, 12);
            HOperatorSet.DispObj(Select, DisplayWindows.HalconWindow);
        }

        /// <summary>
        /// 【新增】 (自動二值化)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAutoThAdd_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            // 檢查是否已讀取影像
            if (!Check_Input_ImgList())
                return;

            if (comboBoxAutoThSelect.SelectedIndex<0)
            {
                MessageBox.Show("請先選擇範圍");
                return;
            }

            try
            {

                #region 

                ParamGetFromUI();
                HObject GoldenRegion, ReduceImage;

                HOperatorSet.GenRectangle1(out GoldenRegion, Recipe.Param.SegParam.CellRow1, Recipe.Param.SegParam.CellColumn1, Recipe.Param.SegParam.CellRow2, Recipe.Param.SegParam.CellColumn2);

                enu_ImageSource ImageSource; // (20200130) Jeff Revised!
                if (radioButton_OrigImg_RegMethod.Checked)
                    ImageSource = enu_ImageSource.原始;
                else
                    ImageSource = enu_ImageSource.影像A;
                HObject SrcImg = Get_SourceImage(ImageSource, int.Parse(nudMethodThImageID.Value.ToString()));
                
                ClearDisplay(SrcImg);

                HOperatorSet.ReduceDomain(SrcImg, GoldenRegion, out ReduceImage);
                GoldenRegion.Dispose();

                #endregion


                HObject Image = clsStaticTool.GetChannelImage(ReduceImage, (enuBand)combxMethodThBand.SelectedIndex);
                ReduceImage.Dispose();

                HObject Thregion;
                HOperatorSet.SelectObj(AutoRegion, out Thregion, comboBoxAutoThSelect.SelectedIndex + 1);

                Image.Dispose();

                DialogResult dialogResult = MessageBox.Show("確認是否新增?", "提示!", MessageBoxButtons.YesNo);

                if (dialogResult != DialogResult.Yes)
                {
                    return;
                }
                string RegionName = "AutoThRegion";
                WorkSheetForm MyForm = new WorkSheetForm("Name", 1);

                MyForm.ShowDialog();

                if (!MyForm.Saved)
                    RegionName = "AutoThRegion";
                else
                    RegionName = MyForm.NewRecipeName;

                bool IsMemberExists = Recipe.Param.MethodRegionList.Exists(x => x.Name == RegionName);

                if (IsMemberExists)
                {
                    MessageBox.Show("存在相同名稱,請重新命名");
                    return;
                }

                HTuple Area, row, column;
                HOperatorSet.AreaCenter(Thregion, out Area, out row, out column);
                clsRecipe.clsMethodRegion MethodRegion = new clsRecipe.clsMethodRegion();
                MethodRegion.HotspotY = Recipe.Param.SegParam.GoldenCenterY - row;
                MethodRegion.HotspotX = Recipe.Param.SegParam.GoldenCenterX - column;
                MethodRegion.Row = row;
                MethodRegion.Column = column;
                MethodRegion.Name = RegionName;
                MethodRegion.RegionPath = Recipe.MethodRegionPath + RegionName;
                MethodRegion.Region = Thregion;
                Recipe.Param.MethodRegionList.Add(MethodRegion);

                UpdateMethodListView(Recipe.Param.MethodRegionList);
            }
            catch (Exception E)
            {
                MessageBox.Show("錯誤! ");
                MessageBox.Show("錯誤訊息: " + E.ToString());
            }
        }

        /// <summary>
        /// 【讀取教導影像】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoadGolden_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            string FilePath = Path + "GoldenImg";
            //if (File.Exists(FilePath))
            {
                List<string> FileList = new List<string>();

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
                try
                {
                    for (int i = 0; i < Recipe.Param.ImgCount; i++)
                    {
                        HOperatorSet.ReadImage(out TmpImg, FilePath + i + ".tif");
                        Input_ImgList.Add(TmpImg.Clone());
                        TmpImg.Dispose();
                    }

                    TmpImg.Dispose();
                    TmpImg = null;

                    UpdateDisplayList();

                    // 顯示影像
                    if (Input_ImgList.Count > 0)
                        HOperatorSet.DispObj(Input_ImgList[0], DisplayWindows.HalconWindow);

                    HOperatorSet.SetPart(DisplayWindows.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part

                    // (20200130) Jeff Revised!
                    btn_Execute_Algo_Click(null, null); //【執行】
                    btn_Execute_Algo_UsedRegion_Click(null, null); //【執行】 (使用範圍列表)
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning); // (20200717) Jeff Revised!
                    if (Input_ImgList.Count > 0)
                        HOperatorSet.DispObj(Input_ImgList[0], DisplayWindows.HalconWindow);
                }
            }
        }

        private void btnExportRegion_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            // 檢查是否已讀取影像
            if (!Check_Input_ImgList())
                return;

            if (listViewUsedRegion.SelectedIndices.Count <= 0)
                return;


            ParamGetFromUI();
            try
            {
                FolderBrowserDialog SaveDialog = new FolderBrowserDialog();
                SaveDialog.ShowDialog();
                string SvaePath = SaveDialog.SelectedPath + "\\";
                HObject TempSaveregion, CellRegion;
                for (int i = 0; i < Recipe.Param.UsedRegionList.Count; i++)
                {
                    HTuple Row = null, Column = null, Angle = null, Score = null;
                    HObject PatternRegions;
                    HObject SrcImg_Match = Get_SourceImage_Match(); // (20200130) Jeff Revised!
                    InductanceInsp.PtternMatchAndSegRegion_New(SrcImg_Match, Recipe, out PatternRegions, out Angle, out Row, out Column, out CellRegion);

                    HOperatorSet.WriteTuple(Angle, SvaePath + "A");

                    if (!InductanceInsp.GetUsedRegion(PatternRegions, Row, Column, Angle, Score, Recipe, i, CellRegion, out TempSaveregion))
                    {
                        MessageBox.Show(i + "_Fail");
                        continue;
                    }

                    HOperatorSet.WriteObject(TempSaveregion, SvaePath + Recipe.Param.UsedRegionList[i].Name);
                }

                HOperatorSet.WriteImage(Input_ImgList[combxDisplayImg.SelectedIndex], "tiff", 0, SvaePath + "Image");

                MessageBox.Show("匯出完成!!");
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        private void btnEditRegionRemoveAll_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("確認是否移除全部Region?", "警告!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dialogResult != DialogResult.Yes)
                {
                    return;
                }

                dialogResult = MessageBox.Show("確定移除全部Region ?", "警告!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dialogResult != DialogResult.Yes)
                {
                    return;
                }

                for (int i = 0; i < Recipe.Param.EditRegionList.Count; i++)
                {
                    Recipe.Param.EditRegionList[i].Region.Dispose();
                }
                Recipe.Param.EditRegionList.Clear();

                UpdateEditListView(Recipe.Param.EditRegionList);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        private void btnClearMethodRegion_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("確認是否移除全部Region?", "警告!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dialogResult != DialogResult.Yes)
                {
                    return;
                }

                dialogResult = MessageBox.Show("確定移除全部Region ?", "警告!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dialogResult != DialogResult.Yes)
                {
                    return;
                }

                for (int i = 0; i < Recipe.Param.MethodRegionList.Count; i++)
                {
                    Recipe.Param.MethodRegionList[i].Region.Dispose();
                }
                Recipe.Param.MethodRegionList.Clear();

                UpdateMethodListView(Recipe.Param.MethodRegionList);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        /// <summary>
        /// 【新增】 (演算法參數設定)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddAlgorithm_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            if (comboBoxAlgorithmSelect.SelectedIndex < 0)
            {
                MessageBox.Show("請先選擇要新增之演算法");
                return;
            }
            if (string.IsNullOrEmpty(txbAlgorithmName.Text))
            {
                MessageBox.Show("請先輸入名稱");
                return;
            }
            if (comboBoxRegionSelect.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇要使用之檢測範圍");
                return;
            }
            clsRecipe.clsAlgorithm Insp = new clsRecipe.clsAlgorithm();
            Insp.Type = (InductanceInsp.enuAlgorithm)comboBoxAlgorithmSelect.SelectedIndex;
            Insp.Param = ConvertParam(Insp.Type);
            Insp.Name = txbAlgorithmName.Text;
            Insp.Priority = int.Parse(nudPriority.Value.ToString());
            Insp.DefectName = txbAlgorithmName.Text;
            Insp.SelectRegionIndex = comboBoxRegionSelect.SelectedIndex;
            if (radioButton_OrigImg_AOI.Checked) // (20200130) Jeff Revised!
                Insp.ImageSource = enu_ImageSource.原始;
            else if (radioButton_ImgA_AOI.Checked)
                Insp.ImageSource = enu_ImageSource.影像A;
            else
                Insp.ImageSource = enu_ImageSource.影像B;
            Insp.InspImageIndex = int.Parse(nudInspImageIndex.Value.ToString());
            Recipe.Param.AlgorithmList.Add(Insp);

            UpdateAlgorithmList(); // 更新演算法列表(listViewAlgorithm)
            InductanceInspRole.UpdateDAVSArray(Recipe.Param, ref Recipe.DAVSInspArray);
        }

        private void btnAlgorithmClear_Click(object sender, EventArgs e)
        {

            try
            {
                DialogResult dialogResult = MessageBox.Show("確認是否移除全部演算法?", "警告!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dialogResult != DialogResult.Yes)
                {
                    return;
                }
                Recipe.Param.AlgorithmList.Clear();

                UpdateAlgorithmList(); // 更新演算法列表(listViewAlgorithm)
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }

        }

        private void btnAlgorithmRemove_Click(object sender, EventArgs e)
        {
            if (listViewAlgorithm.SelectedIndices.Count <= 0 || listViewAlgorithm.SelectedIndices.Count > 1)
            {
                MessageBox.Show("請先選擇要調整之演算法");
                return;
            }
            if (Recipe.Param.AlgorithmList.Count < listViewAlgorithm.SelectedIndices[0])
                return;

            try
            {
                DialogResult dialogResult = MessageBox.Show("確認是否移除此演算法?", "警告!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dialogResult != DialogResult.Yes)
                {
                    return;
                }

                Recipe.Param.AlgorithmList.RemoveAt(listViewAlgorithm.SelectedIndices[0]);

                UpdateAlgorithmList(); // 更新演算法列表(listViewAlgorithm)
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        /// <summary>
        /// AOI演算法視窗是否已關閉
        /// </summary>
        public bool bFormClosed { get; set; } = true;
        /// <summary>
        /// 【參數設定】 (演算法列表)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAlgorithmSetup_Click(object sender, EventArgs e)
        {
            if (listViewAlgorithm.SelectedIndices.Count <= 0 || listViewAlgorithm.SelectedIndices.Count > 1)
            {
                MessageBox.Show("請先選擇要調整之演算法");
                return;
            }
            if (!bFormClosed)
            {
                MessageBox.Show("請先關閉上一個調整視窗", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
                return;
            }

            // 檢查是否已讀取影像
            if (!Check_Input_ImgList())
                return;

            Form EditForm = GetAlgorithmForm(Recipe.Param.AlgorithmList[listViewAlgorithm.SelectedIndices[0]].Type, DisplayWindows, Recipe.Param.AlgorithmList[listViewAlgorithm.SelectedIndices[0]], listViewAlgorithm.SelectedIndices[0]);

            EditForm.Show();
            
            bFormClosed = false;
        }

        /// <summary>
        /// 【單項檢測】 (演算法參數設定)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMethTest_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            // 檢查是否已讀取影像
            if (!Check_Input_ImgList())
                return;

            if (listViewAlgorithm.SelectedIndices.Count <= 0)
                return;
            ParamGetFromUI();
            try
            {
                HObject CellRegion, InspRegion;

                HTuple Row = null, Column = null, Angle = null, Score = null;
                HObject SrcImg_Match = Get_SourceImage_Match(); // (20200130) Jeff Revised!
                InductanceInsp.PtternMatchAndSegRegion_New(SrcImg_Match, Recipe, out PatternRegions, out Angle, out Row, out Column, out CellRegion);

                if (!InductanceInsp.GetUsedRegion(PatternRegions, Row, Column, Angle, Score, Recipe, Recipe.Param.AlgorithmList[listViewAlgorithm.SelectedIndices[0]].SelectRegionIndex, CellRegion, out InspRegion))
                {
                    MessageBox.Show("Fail");
                    return;
                }
                
                HObject DefectRegions, MapRegions;

                InductanceInsp.InspParam InspParam = new InductanceInsp.InspParam(Input_ImgList, InspRegion, Recipe, listViewAlgorithm.SelectedIndices[0], Locate_Method_FS.pixel_resolution_, Recipe.Param.AlgorithmList[listViewAlgorithm.SelectedIndices[0]].Type, ResultImages, ResultImages_UsedRegion); // (20200130) Jeff Revised!
                var InspTask = new Task<HObject>(InductanceInsp.AlgorithmTask, InspParam, TaskCreationOptions.LongRunning);
                InspTask.Start();
                
                DefectRegions = InspTask.Result;
                Method.LocalDefectMappping(DefectRegions, CellRegion, out MapRegions);
                InspTask.Dispose();
                InspTask = null;
                
                enu_ImageSource ImageSource = Recipe.Param.AlgorithmList[listViewAlgorithm.SelectedIndices[0]].ImageSource; // (20200130) Jeff Revised!
                int ImageIndex = Recipe.Param.AlgorithmList[listViewAlgorithm.SelectedIndices[0]].InspImageIndex;
                HObject SrcImg = Get_SourceImage(ImageSource, ImageIndex);

                ClearDisplay(SrcImg); // (20200130) Jeff Revised!

                HOperatorSet.SetDraw(DisplayWindows.HalconWindow, DisplayType.ToString());
                HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                HOperatorSet.DispObj(DefectRegions, DisplayWindows.HalconWindow);

                HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "margin");
                HOperatorSet.SetColor(DisplayWindows.HalconWindow, "green");
                HOperatorSet.DispObj(MapRegions, DisplayWindows.HalconWindow);

                SelectRegions.Dispose();
                HOperatorSet.Connection(DefectRegions, out SelectRegions);

                DrawImg(DefectRegions, MapRegions, ImageIndex, ImageSource); // (20200130) Jeff Revised!
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        /// <summary>
        /// 【使用範圍 : 】 (演算法參數設定)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxRegionSelect_SelectedIndexChanged(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            // 檢查是否已讀取影像
            if (!Check_Input_ImgList())
                return;

            ParamGetFromUI();
            try
            {
                HObject InspRegion, CellRegion;

                HTuple Row = null, Column = null, Angle = null, Score = null;
                HObject PatternRegions;
                HObject SrcImg_Match = Get_SourceImage_Match(); // (20200130) Jeff Revised!
                InductanceInsp.PtternMatchAndSegRegion_New(SrcImg_Match, Recipe, out PatternRegions, out Angle, out Row, out Column, out CellRegion);
                if (!InductanceInsp.GetUsedRegion(PatternRegions, Row, Column, Angle, Score, Recipe, comboBoxRegionSelect.SelectedIndex, CellRegion, out InspRegion))
                {
                    MessageBox.Show("Fail");
                    return;
                }

                enu_ImageSource ImageSource; // (20200130) Jeff Revised!
                if (radioButton_OrigImg_AOI.Checked) // 原始影像
                    ImageSource = enu_ImageSource.原始;
                else if (radioButton_ImgA_AOI.Checked) // 影像A
                    ImageSource = enu_ImageSource.影像A;
                else // 影像B
                    ImageSource = enu_ImageSource.影像B;
                HObject SrcImg = Get_SourceImage(ImageSource, int.Parse(nudInspImageIndex.Value.ToString()));
                
                ClearDisplay(SrcImg);
                
                HOperatorSet.SetDraw(DisplayWindows.HalconWindow, DisplayType.ToString());
                HOperatorSet.SetColored(DisplayWindows.HalconWindow, 12);
                HOperatorSet.DispObj(InspRegion, DisplayWindows.HalconWindow);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        /// <summary>
        /// 取得來源影像
        /// </summary>
        /// <param name="ImageSource"></param>
        /// <param name="ImageIndex"></param>
        /// <returns></returns>
        private HObject Get_SourceImage(enu_ImageSource ImageSource = enu_ImageSource.原始, int ImageIndex = 0) // (20200130) Jeff Revised!
        {
            HObject SrcImg;
            if (ImageSource == enu_ImageSource.原始)
                SrcImg = Input_ImgList[ImageIndex];
            else if (ImageSource == enu_ImageSource.影像A)
                SrcImg = ResultImages[ImageIndex];
            else
                SrcImg = ResultImages_UsedRegion[ImageIndex];

            return SrcImg;
        }

        /// <summary>
        /// 取得來源影像 (圖像教導)
        /// </summary>
        /// <returns></returns>
        private HObject Get_SourceImage_Match() // (20200130) Jeff Revised!
        {
            enu_ImageSource ImageSource;
            if (radioButton_OrigImg_Match.Checked)
                ImageSource = enu_ImageSource.原始;
            else
                ImageSource = enu_ImageSource.影像A;
            return Get_SourceImage(ImageSource, int.Parse(nudInspectImgID.Value.ToString()));
        }

        /// <summary>
        /// 來源影像切換 (演算法參數設定)，調整【影像ID:】控制項上限
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_Img_AOI_CheckedChanged(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            RadioButton rbtn = sender as RadioButton;
            if (rbtn.Checked == false) return;

            /*
            // 檢查是否已讀取影像
            if (!Check_Input_ImgList())
            {
                this.radioButton_OrigImg_AOI.CheckedChanged -= new System.EventHandler(this.radioButton_Img_AOI_CheckedChanged);
                radioButton_OrigImg_AOI.Checked = true;
                this.radioButton_OrigImg_AOI.CheckedChanged += new System.EventHandler(this.radioButton_Img_AOI_CheckedChanged);
                return;
            }
            */

            // 調整【影像ID:】控制項上限
            string Tag = rbtn.Tag.ToString();
            if (Tag == enu_ImageSource.原始.ToString())
                nudInspImageIndex.Maximum = Recipe.Param.ImgCount - 1;
            else if (Tag == enu_ImageSource.影像A.ToString())
            {
                if (Recipe.Param.AlgoImg.ListAlgoImage.Count > 0)
                    nudInspImageIndex.Maximum = Recipe.Param.AlgoImg.ListAlgoImage.Count - 1;
                else
                {
                    MessageBox.Show("影像A數量為0", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    radioButton_OrigImg_AOI.Checked = true; // 原始影像
                }
            }
            else
            {
                if (Recipe.Param.AlgoImg_UsedRegion.ListAlgoImage.Count > 0)
                    nudInspImageIndex.Maximum = Recipe.Param.AlgoImg_UsedRegion.ListAlgoImage.Count - 1;
                else
                {
                    MessageBox.Show("影像B數量為0", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    radioButton_OrigImg_AOI.Checked = true; // 原始影像
                }
            }
        }

        private void listUsedRegion_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (listViewUsedRegion.SelectedIndices.Count <= 0)
                return;
            int Index = listViewUsedRegion.SelectedIndices[0];
            if (Index >= 0)
            {
                if (e.KeyChar == 'f' || e.KeyChar == 'F')
                {
                    Recipe.Param.UsedRegionList[Index].bShowRegion = false;
                }
                else if (e.KeyChar == 't' || e.KeyChar == 'T')
                {
                    Recipe.Param.UsedRegionList[Index].bShowRegion = true;
                }
                else if (e.KeyChar == '0')
                {
                    Recipe.Param.UsedRegionList[Index].ShowImageID = 0;
                }
                else if (e.KeyChar == '1')
                {
                    Recipe.Param.UsedRegionList[Index].ShowImageID = 1;
                }
                else if (e.KeyChar == '2')
                {
                    Recipe.Param.UsedRegionList[Index].ShowImageID = 2;
                }
                else if (e.KeyChar == '3')
                {
                    Recipe.Param.UsedRegionList[Index].ShowImageID = 3;
                }
                else if (e.KeyChar == '4')
                {
                    Recipe.Param.UsedRegionList[Index].ShowImageID = 4;
                }
                else if (e.KeyChar == '5')
                {
                    Recipe.Param.UsedRegionList[Index].ShowImageID = 5;
                }
                else if (e.KeyChar == '6')
                {
                    Recipe.Param.UsedRegionList[Index].ShowImageID = 6;
                }
                else if (e.KeyChar == '7')
                {
                    Recipe.Param.UsedRegionList[Index].ShowImageID = 7;
                }
                else if (e.KeyChar == '8')
                {
                    Recipe.Param.UsedRegionList[Index].ShowImageID = 8;
                }
                else if (e.KeyChar == '9')
                {
                    Recipe.Param.UsedRegionList[Index].ShowImageID = 9;
                }
                else if (e.KeyChar == 'c'|| e.KeyChar == 'C')
                {
                    int SelectIndex = listViewUsedRegion.SelectedIndices[0];
                    ColorDialog _ColorDilog = new ColorDialog();
                    if (_ColorDilog.ShowDialog() == DialogResult.OK)
                    {
                        Recipe.Param.UsedRegionList[listViewUsedRegion.SelectedIndices[0]].Color = clsStaticTool.GetHalconColor(_ColorDilog.Color);
                    }
                }

                    UpdateUsedListView(Recipe.Param.UsedRegionList);
            }
        }

        private void btnAddUsed_Click(object sender, EventArgs e)
        {
            if (listViewEditRegion.SelectedIndices.Count <= 0)
            {
                MessageBox.Show("請先選擇要新增的區域");
                return;
            }

            int SelectIndex = listViewEditRegion.SelectedIndices[0];

            clsRecipe.clsMethodRegion RegionEdit = Recipe.Param.EditRegionList[SelectIndex];
            
            bool IsMemberExists = Recipe.Param.UsedRegionList.Exists(x => x.Name == RegionEdit.Name);

            if (IsMemberExists)
            {
                MessageBox.Show("已存在");
                return;
            }

            Recipe.Param.UsedRegionList.Add(clsStaticTool.Clone<clsRecipe.clsMethodRegion>(RegionEdit));

            UpdateUsedListView(Recipe.Param.UsedRegionList);
        }

        private void btnRemoveUseRegion_Click(object sender, EventArgs e)
        {
            if (listViewUsedRegion.SelectedIndices.Count <= 0)
            {
                MessageBox.Show("請先選擇要移除之Region");
                return;
            }
            if (Recipe.Param.UsedRegionList.Count < listViewUsedRegion.SelectedIndices[0])
                return;

            try
            {
                DialogResult dialogResult = MessageBox.Show("確認是否移除此Region?", "警告!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dialogResult != DialogResult.Yes)
                {
                    return;
                }

                Recipe.Param.UsedRegionList[listViewUsedRegion.SelectedIndices[0]].Region.Dispose();

                Recipe.Param.UsedRegionList.RemoveAt(listViewUsedRegion.SelectedIndices[0]);

                UpdateUsedListView(Recipe.Param.UsedRegionList);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        private void btnClearUsedRegion_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("確認是否移除全部Region?", "警告!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dialogResult != DialogResult.Yes)
                {
                    return;
                }

                dialogResult = MessageBox.Show("確定移除全部Region ?", "警告!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dialogResult != DialogResult.Yes)
                {
                    return;
                }

                for (int i = 0; i < Recipe.Param.UsedRegionList.Count; i++)
                {
                    Recipe.Param.UsedRegionList[i].Region.Dispose();
                }
                Recipe.Param.UsedRegionList.Clear();

                UpdateUsedListView(Recipe.Param.UsedRegionList);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        private void listViewUsedRegion_SelectedIndexChanged(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            // 檢查是否已讀取影像
            if (!Check_Input_ImgList())
                return;

            if (listViewUsedRegion.SelectedIndices.Count <= 0)
                return;

            ParamGetFromUI();
            try
            {
                HTuple Row = null, Column = null, Angle = null, Score = null;
                HObject SrcImg_Match = Get_SourceImage_Match(); // (20200130) Jeff Revised!
                //InductanceInsp.GetMatchInfo(SrcImg_Match, Recipe, out Row, out Column, out Angle, out Score);

                HObject DisplayRegion, CellRegion;
                InductanceInsp.PtternMatchAndSegRegion_New(SrcImg_Match, Recipe, out PatternRegions, out Angle, out Row, out Column, out CellRegion);
                if (Angle.Length <= 0)
                {
                    MessageBox.Show("未搜尋到此特徵,可能為分數、角度、亮暗差異過大");
                    return;
                }

                //InductanceInsp.GetCellRegion(Row, Column, Angle, Score, Recipe, out CellRegion);
                if (!InductanceInsp.GetUsedRegion(PatternRegions, Row, Column, Angle, Score, Recipe, listViewUsedRegion.SelectedIndices[0], CellRegion, out DisplayRegion))
                {
                    MessageBox.Show("Fail");
                    return;
                }

                ClearDisplay(Input_ImgList[combxDisplayImg.SelectedIndex]);

                HOperatorSet.SetDraw(DisplayWindows.HalconWindow, DisplayType.ToString());
                HOperatorSet.SetColored(DisplayWindows.HalconWindow, 12);
                HOperatorSet.DispObj(DisplayRegion, DisplayWindows.HalconWindow);

            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        private void listViewAlgorithm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (listViewAlgorithm.SelectedIndices.Count <= 0)
                return;
            //if (Index >= 0)
            {
                if (e.KeyChar == '0')
                {
                    foreach (int Index in listViewAlgorithm.SelectedIndices)
                        Recipe.Param.AlgorithmList[Index].Priority = 0;
                }
                else if (e.KeyChar == '1')
                {
                    foreach (int Index in listViewAlgorithm.SelectedIndices)
                        Recipe.Param.AlgorithmList[Index].Priority = 1;
                }
                else if (e.KeyChar == '2')
                {
                    foreach (int Index in listViewAlgorithm.SelectedIndices)
                        Recipe.Param.AlgorithmList[Index].Priority = 2;
                }
                else if (e.KeyChar == '3')
                {
                    foreach (int Index in listViewAlgorithm.SelectedIndices)
                        Recipe.Param.AlgorithmList[Index].Priority = 3;
                }
                else if (e.KeyChar == '4')
                {
                    foreach (int Index in listViewAlgorithm.SelectedIndices)
                        Recipe.Param.AlgorithmList[Index].Priority = 4;
                }
                else if (e.KeyChar == '5')
                {
                    foreach (int Index in listViewAlgorithm.SelectedIndices)
                        Recipe.Param.AlgorithmList[Index].Priority = 5;
                }
                else if (e.KeyChar == '6')
                {
                    foreach (int Index in listViewAlgorithm.SelectedIndices)
                        Recipe.Param.AlgorithmList[Index].Priority = 6;
                }
                else if (e.KeyChar == '7')
                {
                    foreach (int Index in listViewAlgorithm.SelectedIndices)
                        Recipe.Param.AlgorithmList[Index].Priority = 7;
                }
                else if (e.KeyChar == '8')
                {
                    foreach (int Index in listViewAlgorithm.SelectedIndices)
                        Recipe.Param.AlgorithmList[Index].Priority = 8;
                }
                else if (e.KeyChar == '9')
                {
                    foreach (int Index in listViewAlgorithm.SelectedIndices)
                        Recipe.Param.AlgorithmList[Index].Priority = 9;
                }
                else if (e.KeyChar == 'c' || e.KeyChar == 'C')
                {
                    int SelectIndex = listViewAlgorithm.SelectedIndices[0];
                    ColorDialog _ColorDilog = new ColorDialog();
                    if (_ColorDilog.ShowDialog() == DialogResult.OK)
                    {
                        Recipe.Param.AlgorithmList[listViewAlgorithm.SelectedIndices[0]].Color = clsStaticTool.GetHalconColor(_ColorDilog.Color);
                    }
                }
                
                UpdateAlgorithmList(); // 更新演算法列表(listViewAlgorithm)
            }
        }

        /// <summary>
        /// 【重整名稱】: 重整所有具有相同優先權之AOI演算法之名稱 (第一次出現之名稱)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateDefectName_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Recipe.Param.PriorityLayerCount + 1; i++)
            {
                List<clsRecipe.clsAlgorithm> TempList = Recipe.Param.AlgorithmList.FindAll(x => x.Priority == i);

                if (TempList.Count > 0)
                {
                    foreach (clsRecipe.clsAlgorithm Al in TempList)
                    {
                        Al.DefectName = TempList[0].DefectName;
                    }
                }

            }
        }

        private void txbSynchronizePath_MouseClick(object sender, MouseEventArgs e)
        {
            FolderBrowserDialog open_file_dialog = new FolderBrowserDialog();

            if (open_file_dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            string FilePath = open_file_dialog.SelectedPath + "\\";

            txbSynchronizePath.Text = FilePath;
        }

        private void btnSynchronize_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txbSynchronizePath.Text))
            {
                MessageBox.Show("請先選擇路徑");
                return;
            }

            BtnLog.WriteLog("[Inspection UI] Synchronize Click");
            DialogResult dialogResult = MessageBox.Show("請確認是否同步更新此Recipe?", "Yes / NO", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (dialogResult != DialogResult.Yes)
            {
                BtnLog.WriteLog("[Inspection UI] Synchronize Cancel");
                return;
            }
            
            try
            {
                SynchronizeRecipe(txbSynchronizePath.Text);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        /// <summary>
        /// recursively copy dir
        /// </summary>
        /// <param name="sourceDirName"></param>
        /// <param name="destDirName"></param>
        public static void CopyDirectory(string sourceDirName, string destDirName)
        {
            if (!Directory.Exists(sourceDirName))
                return;

            // If the destination directory doesn't exist, create it. 
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            string[] Files = Directory.GetFiles(sourceDirName);
            foreach (string SrcPathFile in Files)
            {
                string DstPathFile = destDirName + "\\" + System.IO.Path.GetFileName(SrcPathFile);
                File.Copy(SrcPathFile, DstPathFile, true);
            }

            //copy sub-dir
            try
            {
                foreach (string SubPath in Directory.GetDirectories(sourceDirName))
                {
                    CopyDirectory(SubPath, SubPath.Replace(sourceDirName, destDirName));
                }
            }
            catch (Exception e)
            {

            }
        }

        public static bool SynchronizeRecipe(string NewRecipePath)
        {
            bool Status = false;

            if (string.IsNullOrEmpty(NewRecipePath))
            {
                return Status;
            }

            string SourcePath = ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\InductanceInsp\\";
            string DstPath = NewRecipePath + "InductanceInsp\\";

            if (SourcePath == DstPath)
            {
                return Status;
            }

            try
            {
                Directory.Delete(DstPath, true);

                CopyDirectory(SourcePath, DstPath);
            }
            catch
            {
                return Status;
            }


            Status = true;
            return Status;
        }

        private void btnImportParam_Click(object sender, EventArgs e)
        {
            if (listViewAlgorithm.SelectedIndices.Count <= 0)
                return;
            InductanceInsp.enuAlgorithm Type = Recipe.Param.AlgorithmList[listViewAlgorithm.SelectedIndices[0]].Type;
            for (int i = 0; i < listViewAlgorithm.SelectedIndices.Count; i++)
            {
                if (Recipe.Param.AlgorithmList[listViewAlgorithm.SelectedIndices[i]].Type != Type)
                {
                    MessageBox.Show("請選擇同一種演算法進行匯入參數程序");
                    return;
                }
            }
            try
            {
                OpenFileDialog Open_Dialog = new OpenFileDialog();
                Open_Dialog.Filter = "files (*.xml)|*.xml";

                if (Open_Dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        object LoadParam = GetParm(Open_Dialog.FileName, Type);

                        for (int i = 0; i < listViewAlgorithm.SelectedIndices.Count; i++)
                        {
                            if (Recipe.Param.AlgorithmList[listViewAlgorithm.SelectedIndices[i]].Type == Type)
                            {
                                Recipe.Param.AlgorithmList[listViewAlgorithm.SelectedIndices[i]].Param = clsStaticTool.Clone<object>(LoadParam);
                            }
                            else
                            {

                            }
                        }

                        MessageBox.Show("匯入完成");
                    }
                    catch (Exception Ex)
                    {
                        MessageBox.Show(Ex.ToString());
                    }

                }
            }
            catch
            {

            }
        }

        public static object GetParm(string PathFile, InductanceInsp.enuAlgorithm Type)
        {
            try
            {
                switch (Type)
                {
                    case InductanceInsp.enuAlgorithm.ThresholdInsp:
                        {
                            clsRecipe.clsThresholdInsp Res = new clsRecipe.clsThresholdInsp();
                            XmlSerializer XmlS = new XmlSerializer(Res.GetType());
                            Stream S = File.Open(PathFile, FileMode.Open);
                            Res = (clsRecipe.clsThresholdInsp)XmlS.Deserialize(S);
                            S.Close();
                            return (clsRecipe.clsThresholdInsp)Res;
                        }
                    case InductanceInsp.enuAlgorithm.DAVSInsp:
                        {
                            clsRecipe.clsDAVSInsp Res = new clsRecipe.clsDAVSInsp();
                            XmlSerializer XmlS = new XmlSerializer(Res.GetType());
                            Stream S = File.Open(PathFile, FileMode.Open);
                            Res = (clsRecipe.clsDAVSInsp)XmlS.Deserialize(S);
                            S.Close();
                            return (clsRecipe.clsDAVSInsp)Res;
                        }
                    case InductanceInsp.enuAlgorithm.LaserHoleInsp:
                        {
                            clsRecipe.clsLaserHoleInsp Res = new clsRecipe.clsLaserHoleInsp();
                            XmlSerializer XmlS = new XmlSerializer(Res.GetType());
                            Stream S = File.Open(PathFile, FileMode.Open);
                            Res = (clsRecipe.clsLaserHoleInsp)XmlS.Deserialize(S);
                            S.Close();
                            return (clsRecipe.clsLaserHoleInsp)Res;
                        }
                    case InductanceInsp.enuAlgorithm.TextureInsp:
                        {
                            clsRecipe.clsTextureInsp Res = new clsRecipe.clsTextureInsp();
                            XmlSerializer XmlS = new XmlSerializer(Res.GetType());
                            Stream S = File.Open(PathFile, FileMode.Open);
                            Res = (clsRecipe.clsTextureInsp)XmlS.Deserialize(S);
                            S.Close();
                            return (clsRecipe.clsTextureInsp)Res;
                        }
                    case InductanceInsp.enuAlgorithm.LineInsp:
                        {
                            clsRecipe.clsLineSearchInsp Res = new clsRecipe.clsLineSearchInsp();
                            XmlSerializer XmlS = new XmlSerializer(Res.GetType());
                            Stream S = File.Open(PathFile, FileMode.Open);
                            Res = (clsRecipe.clsLineSearchInsp)XmlS.Deserialize(S);
                            S.Close();
                            return (clsRecipe.clsLineSearchInsp)Res;
                        }
                    default:
                        return null;
                }
                
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            
        }

        private void btnDown_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            if (AdjustRegion == null)
            {
                MessageBox.Show("請先選擇設定之Region");
                return;
            }

            int OffsetY,OffsetX;
            if (!int.TryParse(txbAdjY.Text, out OffsetY))
            {
                MessageBox.Show("轉換錯誤,請確認是否為數字字元");
                return;
            }
            if (!int.TryParse(txbAdjX.Text, out OffsetX))
            {
                MessageBox.Show("轉換錯誤,請確認是否為數字字元");
                return;
            }
            OffsetY = OffsetY + 1;
            txbAdjY.Text = OffsetY.ToString();

            HObject OffsetRegion;
            HOperatorSet.MoveRegion(AdjustRegion, out OffsetRegion, OffsetY, OffsetX);

            HObject SrcImg_Match = Get_SourceImage_Match(); // (20200130) Jeff Revised!
            ClearDisplay(SrcImg_Match);
            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, DisplayType.ToString());
            HOperatorSet.SetColored(DisplayWindows.HalconWindow, 12);
            HOperatorSet.DispObj(OffsetRegion, DisplayWindows.HalconWindow);
        }

        private void btnUp_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            if (AdjustRegion == null)
            {
                MessageBox.Show("請先選擇設定之Region");
                return;
            }

            int OffsetY, OffsetX;
            if (!int.TryParse(txbAdjY.Text, out OffsetY))
            {
                MessageBox.Show("轉換錯誤,請確認是否為數字字元");
                return;
            }
            if (!int.TryParse(txbAdjX.Text, out OffsetX))
            {
                MessageBox.Show("轉換錯誤,請確認是否為數字字元");
                return;
            }
            OffsetY = OffsetY - 1;
            txbAdjY.Text = OffsetY.ToString();

            HObject OffsetRegion;
            HOperatorSet.MoveRegion(AdjustRegion, out OffsetRegion, OffsetY, OffsetX);

            HObject SrcImg_Match = Get_SourceImage_Match(); // (20200130) Jeff Revised!
            ClearDisplay(SrcImg_Match);
            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, DisplayType.ToString());
            HOperatorSet.SetColored(DisplayWindows.HalconWindow, 12);
            HOperatorSet.DispObj(OffsetRegion, DisplayWindows.HalconWindow);
        }

        private void btnRight_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            if (AdjustRegion == null)
            {
                MessageBox.Show("請先選擇設定之Region");
                return;
            }

            int OffsetY, OffsetX;
            if (!int.TryParse(txbAdjY.Text, out OffsetY))
            {
                MessageBox.Show("轉換錯誤,請確認是否為數字字元");
                return;
            }
            if (!int.TryParse(txbAdjX.Text, out OffsetX))
            {
                MessageBox.Show("轉換錯誤,請確認是否為數字字元");
                return;
            }
            OffsetX = OffsetX + 1;
            txbAdjX.Text = OffsetX.ToString();

            HObject OffsetRegion;
            HOperatorSet.MoveRegion(AdjustRegion, out OffsetRegion, OffsetY, OffsetX);

            HObject SrcImg_Match = Get_SourceImage_Match(); // (20200130) Jeff Revised!
            ClearDisplay(SrcImg_Match);
            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, DisplayType.ToString());
            HOperatorSet.SetColored(DisplayWindows.HalconWindow, 12);
            HOperatorSet.DispObj(OffsetRegion, DisplayWindows.HalconWindow);
        }

        private void btnLeft_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            if (AdjustRegion == null)
            {
                MessageBox.Show("請先選擇設定之Region");
                return;
            }

            int OffsetY, OffsetX;
            if (!int.TryParse(txbAdjY.Text, out OffsetY))
            {
                MessageBox.Show("轉換錯誤,請確認是否為數字字元");
                return;
            }
            if (!int.TryParse(txbAdjX.Text, out OffsetX))
            {
                MessageBox.Show("轉換錯誤,請確認是否為數字字元");
                return;
            }
            OffsetX = OffsetX - 1;
            txbAdjX.Text = OffsetX.ToString();

            HObject OffsetRegion;
            HOperatorSet.MoveRegion(AdjustRegion, out OffsetRegion, OffsetY, OffsetX);

            HObject SrcImg_Match = Get_SourceImage_Match(); // (20200130) Jeff Revised!
            ClearDisplay(SrcImg_Match);
            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, DisplayType.ToString());
            HOperatorSet.SetColored(DisplayWindows.HalconWindow, 12);
            HOperatorSet.DispObj(OffsetRegion, DisplayWindows.HalconWindow);
        }

        private void btnAdjSet_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("請確認是否更改相對位置?", "Yes / NO", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
            if (dialogResult != DialogResult.Yes)
            {
                this.ParentForm.Focus();
                return;
            }

            int OffsetY, OffsetX;
            if (!int.TryParse(txbAdjY.Text, out OffsetY))
            {
                MessageBox.Show("轉換錯誤,請確認是否為數字字元");
                return;
            }
            if (!int.TryParse(txbAdjX.Text, out OffsetX))
            {
                MessageBox.Show("轉換錯誤,請確認是否為數字字元");
                return;
            }

            for (int i = 0; i < Recipe.Param.MethodRegionList.Count; i++)
            {
                Recipe.Param.MethodRegionList[i].HotspotX = Recipe.Param.MethodRegionList[i].HotspotX - OffsetX;
                Recipe.Param.MethodRegionList[i].HotspotY = Recipe.Param.MethodRegionList[i].HotspotY - OffsetY;
            }
            for (int i = 0; i < Recipe.Param.EditRegionList.Count; i++)
            {
                Recipe.Param.EditRegionList[i].HotspotX = Recipe.Param.EditRegionList[i].HotspotX - OffsetX;
                Recipe.Param.EditRegionList[i].HotspotY = Recipe.Param.EditRegionList[i].HotspotY - OffsetY;
            }
            for (int i = 0; i < Recipe.Param.UsedRegionList.Count; i++)
            {
                Recipe.Param.UsedRegionList[i].HotspotX = Recipe.Param.UsedRegionList[i].HotspotX - OffsetX;
                Recipe.Param.UsedRegionList[i].HotspotY = Recipe.Param.UsedRegionList[i].HotspotY - OffsetY;
            }

            MessageBox.Show("修正完成");
        }

        #region 【批量測試】 (20200319) Jeff Revised!

        private class BatchTest_Info
        {
            #region 參數

            /// <summary>
            /// 編號
            /// </summary>
            public int ID { get; set; } = 0;

            /// <summary>
            /// 第一張影像之檔名
            /// </summary>
            public string FileName { get; set; } = "";

            /// <summary>
            /// 該位置所有影像之檔案路徑
            /// </summary>
            public List<string> List_Path { get; set; } = new List<string>();

            /// <summary>
            /// 檢測結果為OK之數量
            /// </summary>
            public int count_OK { get; set; } = 0;

            /// <summary>
            /// 檢測結果為NG之數量
            /// </summary>
            public int count_NG { get; set; } = 0;

            /// <summary>
            /// AOI檢測是否成功
            /// </summary>
            public bool B_AOI_success { get; set; } = true;

            #endregion

            public BatchTest_Info() { }

            public void BatchTest_Info_Constructor(int ID_, string FileName_, List<string> List_Path_, int count_OK_ = 0, int count_NG_ = 0)
            {
                this.ID = ID_;
                this.FileName = FileName_;
                this.List_Path = List_Path_;
                this.count_OK = count_OK_;
                this.count_NG = count_NG_;
            }


        }

        private Dictionary<int, BatchTest_Info> Dict_Path_BatchTest { get; set; } = new Dictionary<int, BatchTest_Info>();

        /// <summary>
        /// 【批量測試】選擇路徑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txb_BatchTest_Path_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog Dilg = new FolderBrowserDialog();
            Dilg.SelectedPath = this.txb_BatchTest_Path.Text; // 初始路徑
            if (Dilg.ShowDialog() != DialogResult.OK)
                return;

            this.txb_BatchTest_Path.Text = Dilg.SelectedPath;
            if (string.IsNullOrEmpty(this.txb_BatchTest_Path.Text))
            {
                SystemSounds.Exclamation.Play();
                MessageBox.Show("路徑無效!", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.txb_BatchTest_Path.Text = "";
                return;
            }
        }
        
        private List<Control> ListCtrl_Bypass { get; set; } = new List<Control>();

        /// <summary>
        /// 【批量測試】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBatchTest_Click(object sender, EventArgs e)
        {
            // 檢查是否已選擇路徑
            if (this.txb_BatchTest_Path.Text == "")
            {
                ctrl_timer1 = txb_BatchTest_Path;
                BackColor_ctrl_timer1_1 = SystemColors.Control;
                BackColor_ctrl_timer1_2 = Color.Green;
                timer1.Enabled = true;
                SystemSounds.Exclamation.Play();
                MessageBox.Show("請選擇路徑", "溫馨提醒", MessageBoxButtons.OK, MessageBoxIcon.Information);
                timer1.Enabled = false;
                ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
                return;
            }

            try
            {
                // 清除顯示內容
                txb_BatchTest_Count.Text = "";
                txb_BatchTest_ID.Text = "";
                listView_BatchTest_Result.Items.Clear();
                listView_BatchTest_Info.Items.Clear();

                panelImageControl.Enabled = false;
                panelInspectionControl.Enabled = false;
                tabControl_Info.Enabled = false;
                tbInspParamSetting.Enabled = false;

                // 載入影像
                string FilePath = this.txb_BatchTest_Path.Text + "\\";
                // 法1: 載入各種檔案類型
                //string[] OpenFileList = System.IO.Directory.GetFiles(FilePath);
                // 法2: 只載入影像相關檔案類型
                HTuple hv_ImageFiles = new HTuple();
                clsStaticTool.list_image_files(FilePath, "default", new HTuple(), out hv_ImageFiles);
                if (hv_ImageFiles.Length == 0)
                {
                    MessageBox.Show("影像數量為0", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string[] OpenFileList_Image = hv_ImageFiles.SArr;

                // 檢查影像數量是否正確
                ParamGetFromUI();
                Method.set_parameter(Recipe);
                if (OpenFileList_Image.Length <= 0)
                {
                    MessageBox.Show("影像數量為0", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (OpenFileList_Image.Length % Recipe.Param.ImgCount != 0)
                {
                    MessageBox.Show("影像數量錯誤", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 設定 Dict_Path_BatchTest
                Dict_Path_BatchTest.Clear();
                for (int i = 0, id = 0; i < OpenFileList_Image.Length; i += Recipe.Param.ImgCount, id++)
                {
                    string FileName = System.IO.Path.GetFileName(OpenFileList_Image[i]);
                    List<string> List_Path = new List<string>();
                    for (int j = 0; j < Recipe.Param.ImgCount; j++)
                        List_Path.Add(OpenFileList_Image[i + j]);
                    BatchTest_Info bt = new BatchTest_Info();
                    bt.BatchTest_Info_Constructor(id, FileName, List_Path);
                    Dict_Path_BatchTest.Add(id, bt);
                }

                int method = 2; // method: 1 (不顯示進度條), 2 (顯示進度條)
                if (method == 1) // 不顯示進度條
                {
                    // 檢測及統計
                    foreach (BatchTest_Info bt in Dict_Path_BatchTest.Values) // For 所有位置影像
                    {
                        // 設定
                        if (Recipe.DAVS == null)
                            Recipe.DAVS = new clsDAVS(Recipe.DAVSParam, Recipe.GetRecipePath());

                        MOVE_ID = (bt.ID + 1).ToString();
                        Recipe.DAVS.SetPathParam(ModuleName, SB_ID, MOVE_ID, PartNumber);

                        // 載入該位置所有影像
                        List<HObject> imgList = new List<HObject>();
                        for (int i = 0; i < bt.List_Path.Count; i++)
                        {
                            HObject img;
                            HOperatorSet.ReadImage(out img, bt.List_Path[i]);
                            imgList.Add(img);
                        }

                        // 檢測
                        HObject ResRegion;
                        List<Defect> ResList;
                        List<int> IndexList;
                        if (!Method.executeNew(Recipe, imgList, true, out ResRegion, out ResList, out IndexList))
                        {
                            BtnLog.WriteLog("檢測錯誤");
                            bt.B_AOI_success = false;
                        }

                        // 統計 OK, NG 顆數
                        if (bt.B_AOI_success) // AOI檢測成功
                        {
                            HTuple Count_NG, Count_Cell;
                            HOperatorSet.CountObj(ResRegion, out Count_NG);
                            HOperatorSet.CountObj(Method.PatternRegion, out Count_Cell);
                            bt.count_NG = Count_NG.I;
                            bt.count_OK = (Count_Cell - Count_NG).I;
                        }

                        // Dispose
                        clsStaticTool.DisposeAll(imgList);
                        ResRegion.Dispose();
                        clsStaticTool.DisposeAll(ResList);
                    }

                    // 顯示批量測試統計結果及詳細資訊
                    int total_OK = 0, total_NG = 0;
                    foreach (BatchTest_Info bt in Dict_Path_BatchTest.Values) // For 所有位置影像
                    {
                        total_OK += bt.count_OK;
                        total_NG += bt.count_NG;
                    }
                    Update_listView_BatchTest_Result(listView_BatchTest_Result, total_OK, total_NG);
                    txb_BatchTest_Count.Text = (total_OK + total_NG).ToString(); // 總數
                    Update_listView_BatchTest_Info(listView_BatchTest_Info);

                    /*
                    for (int i = 0; i < OpenFileList_Image.Length; i++)
                    {
                        HObject TempImage;
                        HOperatorSet.ReadImage(out TempImage, OpenFileList_Image[i]);

                        clsStaticTool.DisposeAll(Input_ImgList);

                        Input_ImgList.Clear();

                        Input_ImgList.Add(TempImage.Clone());

                        if (Recipe.DAVS == null)
                            Recipe.DAVS = new clsDAVS(Recipe.DAVSParam, Recipe.GetRecipePath());

                        MOVE_ID = (i + 1).ToString();

                        Recipe.DAVS.SetPathParam(ModuleName, SB_ID, MOVE_ID, PartNumber);
                        List<Defect> ResList;
                        HObject ResRegion;

                        List<int> IndexList;
                        if (!Method.executeNew(Recipe, Input_ImgList, true, out ResRegion, out ResList, out IndexList))
                        {
                            BtnLog.WriteLog("檢測錯誤");
                            BtnLog.WriteLog("File : " + OpenFileList_Image[i]);
                        }

                        TempImage.Dispose();
                        Trace.WriteLine("================ 執行個數 " + i.ToString() + " ======================");
                    }
                    */
                }
                else if (method == 2) // 顯示進度條
                {
                    if (bw_BatchTest.IsBusy)
                        throw new Exception("BackgroundWorker 忙碌中");
                    
                    bw_BatchTest.RunWorkerAsync();

                    ctrl_timer1 = btnBatchTest;
                    BackColor_ctrl_timer1_1 = Color.Lime;
                    BackColor_ctrl_timer1_2 = Color.Transparent;
                    timer1.Enabled = true;
                }

            }
            catch (Exception ex)
            {
                string message = "Error: " + ex.Message;
                MessageBox.Show(message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                if (bw_BatchTest.IsBusy == false)
                {
                    panelImageControl.Enabled = true;
                    panelInspectionControl.Enabled = true;
                    tabControl_Info.Enabled = true;
                    tbInspParamSetting.Enabled = true;
                }
            }

        }

        private void bw_DoWork_BatchTest(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker Worker = (BackgroundWorker)sender;
            clsProgressbar m_ProgressBar = new clsProgressbar();
            
            m_ProgressBar.FormClosedEvent2 += new clsProgressbar.FormClosedHandler2(SetFormClosed2);

            m_ProgressBar.SetShowText("請等待批量測試......");
            m_ProgressBar.SetShowCaption("執行中......");
            m_ProgressBar.ShowRunProgress();

            try
            {
                // 檢測及統計
                foreach (BatchTest_Info bt in Dict_Path_BatchTest.Values) // For 所有位置影像
                {
                    // 設定
                    if (Recipe.DAVS == null)
                        Recipe.DAVS = new clsDAVS(Recipe.DAVSParam, Recipe.GetRecipePath());

                    MOVE_ID = (bt.ID + 1).ToString();
                    Recipe.DAVS.SetPathParam(ModuleName, SB_ID, MOVE_ID, PartNumber);

                    // 載入該位置所有影像
                    List<HObject> imgList = new List<HObject>();
                    for (int i = 0; i < bt.List_Path.Count; i++)
                    {
                        HObject img;
                        HOperatorSet.ReadImage(out img, bt.List_Path[i]);
                        imgList.Add(img);
                    }

                    // 檢測
                    HObject ResRegion;
                    List<Defect> ResList;
                    List<int> IndexList;
                    if (!Method.executeNew(Recipe, imgList, true, out ResRegion, out ResList, out IndexList))
                    {
                        BtnLog.WriteLog("檢測錯誤");
                        bt.B_AOI_success = false;
                    }

                    // 統計 OK, NG 顆數
                    if (bt.B_AOI_success) // AOI檢測成功
                    {
                        HTuple Count_NG, Count_Cell;
                        HOperatorSet.CountObj(ResRegion, out Count_NG);
                        HOperatorSet.CountObj(Method.PatternRegion, out Count_Cell);
                        bt.count_NG = Count_NG.I;
                        bt.count_OK = (Count_Cell - Count_NG).I;
                    }

                    // Dispose
                    clsStaticTool.DisposeAll(imgList);
                    ResRegion.Dispose();
                    clsStaticTool.DisposeAll(ResList);

                    if (bw_BatchTest.CancellationPending == true) // 判斷是否停止BackgroundWorker執行
                    {
                        e.Cancel = true;
                        m_ProgressBar.CloseProgress();
                        return;
                    }

                    // 更新目前進度條的位置
                    m_ProgressBar.SetStep(100 * (bt.ID + 1) / (Dict_Path_BatchTest.Count));
                }

                e.Result = "Success";
                m_ProgressBar.CloseProgress(); // 關閉進度條視窗
            }
            catch
            {
                e.Result = "Exception";
                m_ProgressBar.CloseProgress();
                return;
            }
        }

        /// <summary>
        /// 停止BackgroundWorker執行
        /// </summary>
        public void SetFormClosed2()
        {
            if (bw_BatchTest.WorkerSupportsCancellation == true)
            {
                if (bw_BatchTest.IsBusy)
                    bw_BatchTest.CancelAsync();
            }
        }

        private void bw_RunWorkerCompleted_BatchTest(object sender, RunWorkerCompletedEventArgs e)
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
                SystemSounds.Exclamation.Play();
                MessageBox.Show("批量測試失敗", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (e.Result != null && e.Result.ToString() == "Success")
            {
                // 顯示批量測試統計結果及詳細資訊
                int total_OK = 0, total_NG = 0;
                foreach (BatchTest_Info bt in Dict_Path_BatchTest.Values) // For 所有位置影像
                {
                    total_OK += bt.count_OK;
                    total_NG += bt.count_NG;
                }
                Update_listView_BatchTest_Result(listView_BatchTest_Result, total_OK, total_NG);
                txb_BatchTest_Count.Text = (total_OK + total_NG).ToString(); // 總數
                Update_listView_BatchTest_Info(listView_BatchTest_Info);
            }
            else
                return;

            panelImageControl.Enabled = true;
            panelInspectionControl.Enabled = true;
            tabControl_Info.Enabled = true;
            tbInspParamSetting.Enabled = true;
            timer1.Enabled = false;
            ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;

            this.Focus();
        }

        /// <summary>
        /// 更新 listView_BatchTest_Result
        /// </summary>
        /// <param name="listView"></param>
        /// <param name="total_OK"></param>
        /// <param name="total_NG"></param>
        public void Update_listView_BatchTest_Result(ListView listView, int total_OK, int total_NG)
        {
            listView.BeginUpdate();
            listView.Items.Clear();

            /* 資料建立 */
            Dictionary<string, int> Datas_count = new Dictionary<string, int>();
            Dictionary<string, Color> Datas_color = new Dictionary<string, Color>();
            // OK
            Datas_count.Add("OK", total_OK);
            Datas_color.Add("OK", Color.Green);
            // NG
            Datas_count.Add("NG", total_NG);
            Datas_color.Add("NG", Color.Red);

            foreach (KeyValuePair<string, int> item in Datas_count)
            {
                ListViewItem lvi = new ListViewItem(item.Key); // 檢測結果
                lvi.ForeColor = Datas_color[item.Key];
                lvi.SubItems.Add(item.Value.ToString()); // 數量

                listView.Items.Add(lvi);
            }

            listView.EndUpdate();
        }

        /// <summary>
        /// 更新 listView_BatchTest_Info
        /// </summary>
        /// <param name="listView"></param>
        public void Update_listView_BatchTest_Info(ListView listView)
        {
            listView.ForeColor = Color.DarkViolet;

            listView.BeginUpdate();
            listView.Items.Clear();

            foreach (BatchTest_Info bt in Dict_Path_BatchTest.Values) // For 所有位置影像
            {
                ListViewItem lvi = new ListViewItem(bt.ID.ToString()); // 編號
                lvi.SubItems.Add(bt.FileName); // 檔名
                lvi.UseItemStyleForSubItems = false;
                lvi.SubItems.Add(bt.count_OK.ToString(), Color.Green, Color.AliceBlue, new Font("微軟正黑體", (float)10, FontStyle.Bold)); // OK
                lvi.SubItems.Add(bt.count_NG.ToString(), Color.Red, Color.AliceBlue, new Font("微軟正黑體", (float)10, FontStyle.Bold)); // NG

                listView.Items.Add(lvi);
            }
            
            listView.EndUpdate();
        }

        /// <summary>
        /// 【載入影像】(【批量測試】)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_BatchTest_LoadImg_Click(object sender, EventArgs e)
        {
            if (Dict_Path_BatchTest.Count <= 0)
                return;

            if (listView_BatchTest_Info.SelectedIndices.Count <= 0)
            {
                txb_BatchTest_ID.Text = "";
                ctrl_timer1 = listView_BatchTest_Info;
                BackColor_ctrl_timer1_1 = Color.AliceBlue;
                BackColor_ctrl_timer1_2 = Color.Green;
                timer1.Enabled = true;
                SystemSounds.Exclamation.Play();
                MessageBox.Show("請選擇編號", "溫馨提醒", MessageBoxButtons.OK, MessageBoxIcon.Information);
                timer1.Enabled = false;
                ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
                return;
            }

            try
            {
                // 載入該位置所有影像
                int id = listView_BatchTest_Info.SelectedIndices[0];
                txb_BatchTest_ID.ForeColor = Color.DarkViolet;
                txb_BatchTest_ID.Text = id.ToString();
                clsStaticTool.DisposeAll(Input_ImgList);
                Input_ImgList.Clear();
                for (int i = 0; i < Dict_Path_BatchTest[id].List_Path.Count; i++)
                {
                    HObject img;
                    HOperatorSet.ReadImage(out img, Dict_Path_BatchTest[id].List_Path[i]);
                    Input_ImgList.Add(img);
                }

                UpdateDisplayList();

                // 顯示影像
                if (Input_ImgList.Count > 0)
                {
                    HOperatorSet.DispObj(Input_ImgList[0], DisplayWindows.HalconWindow);
                    HOperatorSet.SetPart(DisplayWindows.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part
                }

                if (Input_ImgList != null && Input_ImgList.Count > 0)
                {
                    btn_Execute_Algo_Click(null, null); //【執行】
                    btn_Execute_Algo_UsedRegion_Click(null, null); //【執行】 (使用範圍列表)
                }
            }
            catch (Exception ex)
            {
                txb_BatchTest_ID.Text = "";
                string message = "Error: " + ex.Message;
                MessageBox.Show(message, "【載入影像】錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        #endregion

        private void btnAddImsge_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenImgDilg = new OpenFileDialog();

            if (OpenImgDilg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;
            string path = OpenImgDilg.FileName;

            try
            {
                HObject TmpImg;
                HOperatorSet.ReadImage(out TmpImg, path);
                Input_ImgList.Add(TmpImg.Clone());
                UpdateDisplayList();
            }
            catch
            {
                MessageBox.Show("讀取檔案錯誤");
            }
        }

        /// <summary>
        /// 【測試】 (動態二值化)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDynTest_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            // 檢查是否已讀取影像
            if (!Check_Input_ImgList())
                return;

            if (comboBoxDynType.SelectedIndex < 0)
            {
                MessageBox.Show("請先選擇Type");
                return;
            }

            try
            {
                ParamGetFromUI();
                HObject GoldenRegion, ReduceImage;

                HOperatorSet.GenRectangle1(out GoldenRegion, Recipe.Param.SegParam.CellRow1, Recipe.Param.SegParam.CellColumn1, Recipe.Param.SegParam.CellRow2, Recipe.Param.SegParam.CellColumn2);

                enu_ImageSource ImageSource; // (20200130) Jeff Revised!
                if (radioButton_OrigImg_RegMethod.Checked)
                    ImageSource = enu_ImageSource.原始;
                else
                    ImageSource = enu_ImageSource.影像A;
                HObject SrcImg = Get_SourceImage(ImageSource, int.Parse(nudMethodThImageID.Value.ToString()));
                
                ClearDisplay(SrcImg);

                HOperatorSet.ReduceDomain(SrcImg, GoldenRegion, out ReduceImage);
                GoldenRegion.Dispose();

                HObject Thregion;
                HTuple MeanW, MeanH, Offset;
                MeanW = int.Parse(nudMeanWidth.Value.ToString());
                MeanH = int.Parse(nudMeanHeight.Value.ToString());
                Offset = int.Parse(nudDynOffset.Value.ToString());

                HObject Image = clsStaticTool.GetChannelImage(ReduceImage, (enuBand)combxMethodThBand.SelectedIndex);
                ReduceImage.Dispose();

                HObject MeanImage;
                HOperatorSet.MeanImage(Image, out MeanImage, MeanW, MeanH);
                
                InductanceInsp.enuDynThresholdType Type = (InductanceInsp.enuDynThresholdType)comboBoxDynType.SelectedIndex;

                HOperatorSet.DynThreshold(Image, MeanImage, out Thregion, Offset, Type.ToString());

                HOperatorSet.SetDraw(DisplayWindows.HalconWindow, DisplayType.ToString());
                HOperatorSet.SetColored(DisplayWindows.HalconWindow, 12);
                HOperatorSet.DispObj(Thregion, DisplayWindows.HalconWindow);

                MeanImage.Dispose(); // (20200729) Jeff Revised!
                Thregion.Dispose();
                Image.Dispose();
            }
            catch (Exception E)
            {
                MessageBox.Show("錯誤! ");
                MessageBox.Show("錯誤訊息: " + E.ToString());
            }
        }

        /// <summary>
        /// 【新增】 (動態二值化)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDynThAdd_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            // 檢查是否已讀取影像
            if (!Check_Input_ImgList())
                return;

            try
            {

                #region 

                ParamGetFromUI();
                HObject GoldenRegion, ReduceImage;

                HOperatorSet.GenRectangle1(out GoldenRegion, Recipe.Param.SegParam.CellRow1, Recipe.Param.SegParam.CellColumn1, Recipe.Param.SegParam.CellRow2, Recipe.Param.SegParam.CellColumn2);

                enu_ImageSource ImageSource; // (20200130) Jeff Revised!
                if (radioButton_OrigImg_RegMethod.Checked)
                    ImageSource = enu_ImageSource.原始;
                else
                    ImageSource = enu_ImageSource.影像A;
                HObject SrcImg = Get_SourceImage(ImageSource, int.Parse(nudMethodThImageID.Value.ToString()));
                
                ClearDisplay(SrcImg);

                HOperatorSet.ReduceDomain(SrcImg, GoldenRegion, out ReduceImage);
                GoldenRegion.Dispose();

                #endregion


                HObject Image = clsStaticTool.GetChannelImage(ReduceImage, (enuBand)combxMethodThBand.SelectedIndex);
                ReduceImage.Dispose();

                HObject Thregion;
                HTuple MeanW, MeanH, Offset;
                MeanW = int.Parse(nudMeanWidth.Value.ToString());
                MeanH = int.Parse(nudMeanHeight.Value.ToString());
                Offset = int.Parse(nudDynOffset.Value.ToString());
                
                HObject MeanImage;
                HOperatorSet.MeanImage(Image, out MeanImage, MeanW, MeanH);


                InductanceInsp.enuDynThresholdType Type = (InductanceInsp.enuDynThresholdType)comboBoxDynType.SelectedIndex;

                HOperatorSet.DynThreshold(Image, MeanImage, out Thregion, Offset, Type.ToString());
                Image.Dispose();

                DialogResult dialogResult = MessageBox.Show("確認是否新增?", "提示!", MessageBoxButtons.YesNo);

                if (dialogResult != DialogResult.Yes)
                {
                    return;
                }
                string RegionName = "DynThRegion";
                WorkSheetForm MyForm = new WorkSheetForm("Name", 1);

                MyForm.ShowDialog();

                if (!MyForm.Saved)
                    RegionName = "DynThRegion";
                else
                    RegionName = MyForm.NewRecipeName;

                bool IsMemberExists = Recipe.Param.MethodRegionList.Exists(x => x.Name == RegionName);

                if (IsMemberExists)
                {
                    MessageBox.Show("存在相同名稱,請重新命名");
                    return;
                }

                HTuple Area, row, column;
                HOperatorSet.AreaCenter(Thregion, out Area, out row, out column);
                clsRecipe.clsMethodRegion MethodRegion = new clsRecipe.clsMethodRegion();
                MethodRegion.HotspotY = Recipe.Param.SegParam.GoldenCenterY - row;
                MethodRegion.HotspotX = Recipe.Param.SegParam.GoldenCenterX - column;
                MethodRegion.Row = row;
                MethodRegion.Column = column;
                MethodRegion.Name = RegionName;
                MethodRegion.RegionPath = Recipe.MethodRegionPath + RegionName;
                MethodRegion.Region = Thregion;
                Recipe.Param.MethodRegionList.Add(MethodRegion);

                UpdateMethodListView(Recipe.Param.MethodRegionList);
            }
            catch (Exception E)
            {
                MessageBox.Show("錯誤! ");
                MessageBox.Show("錯誤訊息: " + E.ToString());
            }
        }

        #endregion

        #region 【影像演算法】 // (20200119) Jeff Revised!


        /// <summary>
        /// 檢查是否已讀取影像
        /// </summary>
        /// <returns></returns>
        private bool Check_Input_ImgList()
        {
            if (Input_ImgList == null || Input_ImgList.Count <= 0)
            {
                MessageBox.Show("請先讀取影像");
                return false;
            }

            return true;
        }

        #region 影像區域演算法A (不使用範圍列表)

        /// <summary>
        /// 影像演算法物件 (不使用範圍列表)
        /// </summary>
        private Algo_Image AlgoImg = new Algo_Image();

        /// <summary>
        /// 結果影像集合 (不使用範圍列表)
        /// </summary>
        private List<HObject> ResultImages = new List<HObject>();

        /// <summary>
        /// 結果區域集合 (不使用範圍列表)
        /// </summary>
        private List<HObject> ResultRegions = new List<HObject>(); // (20200729) Jeff Revised!

        /// <summary>
        /// 【結果影像A(編號)】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbx_ResultImgID_A_SelectedIndexChanged(object sender, EventArgs e) // (20200729) Jeff Revised!
        {
            if (this.cbx_ResultImgID_A.SelectedIndex < 0)
                return;

            // 檢查是否已讀取影像
            if (!this.Check_Input_ImgList())
            {
                (sender as ComboBox).SelectedIndex = -1;
                return;
            }
            
            HOperatorSet.ClearWindow(DisplayWindows.HalconWindow);
            try
            {
                int id = this.cbx_ResultImgID_A.SelectedIndex;
                // 顯示影像
                if (this.AlgoImg.ListAlgoImage[id].B_ImageAlgo) // 影像演算法
                    HOperatorSet.DispObj(this.ResultImages[id], DisplayWindows.HalconWindow);
                else // 區域演算法
                {
                    (sender as ComboBox).SelectedIndex = -1;
                    this.combxDisplayImg_SelectedIndexChanged(null, null);
                }

                // 顯示區域
                if (this.cbx_ResultRegID_A.SelectedIndex == id)
                    this.cbx_ResultRegID_A_SelectedIndexChanged(this.cbx_ResultRegID_A, null);
                else
                    this.cbx_ResultRegID_A.SelectedIndex = id;
            }
            catch (Exception ex)
            {
                string message = "Error: " + ex.Message;
                MessageBox.Show(message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 【結果區域A(編號)】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbx_ResultRegID_A_SelectedIndexChanged(object sender, EventArgs e) // (20200729) Jeff Revised!
        {
            if (this.cbx_ResultRegID_A.SelectedIndex < 0)
                return;

            // 檢查是否已讀取影像
            if (!this.Check_Input_ImgList())
            {
                (sender as ComboBox).SelectedIndex = -1;
                return;
            }

            try
            {
                HOperatorSet.SetDraw(DisplayWindows.HalconWindow, DisplayType.ToString());
                HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                HOperatorSet.DispObj(this.ResultRegions[this.cbx_ResultRegID_A.SelectedIndex], DisplayWindows.HalconWindow);
            }
            catch (Exception ex)
            {
                string message = "Error: " + ex.Message;
                MessageBox.Show(message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 【另存結果影像A】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_SaveAsResultImgA_Click(object sender, EventArgs e) // (20200219) Jeff Revised!
        {
            if (cbx_ResultImgID_A.SelectedIndex < 0)
            {
                ctrl_timer2 = cbx_ResultImgID_A;
                BackColor_ctrl_timer2 = ctrl_timer2.BackColor;
                timer2.Enabled = true;
                MessageBox.Show("請選擇【結果影像A(編號)】", "溫馨提醒", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                // 1. Set 
                SaveFileDialog saveImgDialog = new SaveFileDialog();
                saveImgDialog.Filter = "TIFF Image|*.tif|JPeg Image|*.jpg|PNG Image|*.png|Bitmap Image|*.bmp";

                // 2. 開啟對話框
                if (saveImgDialog.ShowDialog() != DialogResult.OK)
                    return;

                // 3. Save image
                string ImageFormat = ((enu_ImageFormat)(saveImgDialog.FilterIndex - 1)).ToString();
                string fileName = saveImgDialog.FileName;
                HObject imgSaved = null;
                imgSaved = ResultImages[cbx_ResultImgID_A.SelectedIndex];
                HOperatorSet.WriteImage(imgSaved, ImageFormat, 0, fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "另存影像失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 【執行】 (不使用範圍列表)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Execute_Algo_Click(object sender, EventArgs e) // (20200729) Jeff Revised!
        {
            // 檢查是否已讀取影像
            if (!Check_Input_ImgList())
                return;

            // 執行所有演算法計算
            AlgoImg.ImageSource = Input_ImgList;
            clsStaticTool.DisposeAll(this.ResultImages);
            clsStaticTool.DisposeAll(this.ResultRegions);
            if (!(AlgoImg.Execute(out this.ResultImages, out this.ResultRegions)))
                return;

            // 更新【結果影像A(編號)】&【結果區域A(編號)】
            this.cbx_ResultImgID_A.Items.Clear();
            this.cbx_ResultRegID_A.Items.Clear();
            for (int i = 0; i < this.AlgoImg.ListAlgoImage.Count; i++)
            {
                this.cbx_ResultImgID_A.Items.Add(i.ToString());
                this.cbx_ResultRegID_A.Items.Add(i.ToString());
            }
            
            // 更新 Recipe 內之物件
            Recipe.Param.AlgoImg = this.AlgoImg; // (20200130) Jeff Revised!
        }

        /// <summary>
        /// 顯示單一演算法結果 (不使用範圍列表)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView_EditAlgo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView_EditAlgo.SelectedIndices.Count <= 0)
                return;

            try
            {
                int id_ResultImg = listView_EditAlgo.SelectedIndices[0];
                if (cbx_ResultImgID_A.SelectedIndex != id_ResultImg)
                    cbx_ResultImgID_A.SelectedIndex = id_ResultImg;
                else
                    cbx_ResultImgID_A_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                string message = "Error: " + ex.Message;
                MessageBox.Show(message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 【新增】 (不使用範圍列表)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Add_Algo_Click(object sender, EventArgs e)
        {
            // 檢查是否已讀取影像
            if (!Check_Input_ImgList())
                return;

            int id_ResultImg = listView_EditAlgo.Items.Count;
            AlgoImg.ImageSource = Input_ImgList;
            using (Algo_Image_Form form = new Algo_Image_Form(AlgoImg, id_ResultImg, ref this.ResultImages, ref this.ResultRegions)) // (20200729) Jeff Revised!
            {
                if (form.ShowDialog() == DialogResult.Yes)
                {
                    AlgoImg = form.AlgoImg_yes;
                    AlgoImg.Update_listView_Edit(listView_EditAlgo); // 更新 listView_EditAlgo
                    btn_Execute_Algo_Click(null, null); //【執行】
                    cbx_ResultImgID_A.SelectedIndex = id_ResultImg; //【結果影像A(編號)】: 顯示結果影像
                }
            }
        }

        /// <summary>
        /// 【編輯】 (不使用範圍列表)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Edit_Algo_Click(object sender, EventArgs e)
        {
            // 檢查是否已讀取影像
            if (!Check_Input_ImgList())
                return;

            if (listView_EditAlgo.SelectedIndices.Count <= 0)
            {
                ctrl_timer1 = listView_EditAlgo;
                BackColor_ctrl_timer1_1 = Color.AliceBlue;
                BackColor_ctrl_timer1_2 = Color.Green;
                timer1.Enabled = true;
                SystemSounds.Exclamation.Play();
                MessageBox.Show("請選擇演算法編號", "溫馨提醒", MessageBoxButtons.OK, MessageBoxIcon.Information);
                timer1.Enabled = false;
                ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
                return;
            }

            int id_ResultImg = listView_EditAlgo.SelectedIndices[0];
            AlgoImg.ImageSource = Input_ImgList;
            using (Algo_Image_Form form = new Algo_Image_Form(AlgoImg, id_ResultImg, ref ResultImages, ref this.ResultRegions)) // (20200729) Jeff Revised!
            {
                if (form.ShowDialog() == DialogResult.Yes)
                {
                    AlgoImg = form.AlgoImg_yes;
                    AlgoImg.Update_listView_Edit(listView_EditAlgo); // 更新 listView_EditAlgo
                    btn_Execute_Algo_Click(null, null); //【執行】
                    cbx_ResultImgID_A.SelectedIndex = id_ResultImg; //【結果影像(編號)】: 顯示結果影像
                }
            }
        }

        /// <summary>
        /// 【刪除】 (不使用範圍列表)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Remove_Algo_Click(object sender, EventArgs e)
        {
            if (listView_EditAlgo.SelectedIndices.Count <= 0)
            {
                ctrl_timer1 = listView_EditAlgo;
                BackColor_ctrl_timer1_1 = Color.AliceBlue;
                BackColor_ctrl_timer1_2 = Color.Green;
                timer1.Enabled = true;
                SystemSounds.Exclamation.Play();
                MessageBox.Show("請選擇演算法編號", "溫馨提醒", MessageBoxButtons.OK, MessageBoxIcon.Information);
                timer1.Enabled = false;
                ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
                return;
            }

            DialogResult dr = MessageBox.Show("確定要刪除演算法編號【" + listView_EditAlgo.SelectedItems[0].Text + "】?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No)
                return;

            if (!AlgoImg.Remove_1_Algo(listView_EditAlgo.SelectedIndices[0]))
                return;

            AlgoImg.Update_listView_Edit(listView_EditAlgo); // 更新 listView_EditAlgo
            btn_Execute_Algo_Click(null, null); //【執行】
        }

        /// <summary>
        /// 【清空】 (不使用範圍列表)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_RemoveAll_Algo_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("確定要清空所有演算法編號?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No)
                return;
            
            AlgoImg.RemoveAll_Algo();
            AlgoImg.Update_listView_Edit(listView_EditAlgo); // 更新 listView_EditAlgo
            btn_Execute_Algo_Click(null, null); //【執行】
        }

        #endregion


        #region 影像區域演算法B (使用範圍列表)

        /// <summary>
        /// 影像演算法物件 (使用範圍列表)
        /// </summary>
        private Algo_Image AlgoImg_UsedRegion = new Algo_Image();

        /// <summary>
        /// 結果影像集合 (使用範圍列表)
        /// </summary>
        private List<HObject> ResultImages_UsedRegion = new List<HObject>();

        /// <summary>
        /// 結果區域集合 (使用範圍列表)
        /// </summary>
        private List<HObject> ResultRegions_UsedRegion = new List<HObject>(); // (20200729) Jeff Revised!

        /// <summary>
        /// 【結果影像B(編號)】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbx_ResultImgID_B_SelectedIndexChanged(object sender, EventArgs e) // (20200729) Jeff Revised!
        {
            if (this.cbx_ResultImgID_B.SelectedIndex < 0)
                return;

            // 檢查是否已讀取影像
            if (!this.Check_Input_ImgList())
            {
                (sender as ComboBox).SelectedIndex = -1;
                return;
            }

            HOperatorSet.ClearWindow(DisplayWindows.HalconWindow);
            try
            {
                int id = this.cbx_ResultImgID_B.SelectedIndex;
                // 顯示影像
                if (this.AlgoImg_UsedRegion.ListAlgoImage[id].B_ImageAlgo) // 影像演算法
                    HOperatorSet.DispObj(this.ResultImages_UsedRegion[this.cbx_ResultImgID_B.SelectedIndex], DisplayWindows.HalconWindow);
                else // 區域演算法
                {
                    (sender as ComboBox).SelectedIndex = -1;
                    this.combxDisplayImg_SelectedIndexChanged(null, null);
                }

                // 顯示區域
                if (this.cbx_ResultRegID_B.SelectedIndex == id)
                    this.cbx_ResultRegID_B_SelectedIndexChanged(this.cbx_ResultRegID_B, null);
                else
                    this.cbx_ResultRegID_B.SelectedIndex = id;
            }
            catch (Exception ex)
            {
                string message = "Error: " + ex.Message;
                MessageBox.Show(message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 【結果區域B(編號)】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbx_ResultRegID_B_SelectedIndexChanged(object sender, EventArgs e) // (20200729) Jeff Revised!
        {
            if (this.cbx_ResultRegID_B.SelectedIndex < 0)
                return;

            // 檢查是否已讀取影像
            if (!this.Check_Input_ImgList())
            {
                (sender as ComboBox).SelectedIndex = -1;
                return;
            }

            try
            {
                HOperatorSet.SetDraw(DisplayWindows.HalconWindow, DisplayType.ToString());
                HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                HOperatorSet.DispObj(this.ResultRegions_UsedRegion[this.cbx_ResultRegID_B.SelectedIndex], DisplayWindows.HalconWindow);
            }
            catch (Exception ex)
            {
                string message = "Error: " + ex.Message;
                MessageBox.Show(message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 【另存結果影像B】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_SaveAsResultImgB_Click(object sender, EventArgs e) // (20200219) Jeff Revised!
        {
            if (cbx_ResultImgID_B.SelectedIndex < 0)
            {
                ctrl_timer2 = cbx_ResultImgID_B;
                BackColor_ctrl_timer2 = ctrl_timer2.BackColor;
                timer2.Enabled = true;
                MessageBox.Show("請選擇【結果影像B(編號)】", "溫馨提醒", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                // 1. Set 
                SaveFileDialog saveImgDialog = new SaveFileDialog();
                saveImgDialog.Filter = "TIFF Image|*.tif|JPeg Image|*.jpg|PNG Image|*.png|Bitmap Image|*.bmp";

                // 2. 開啟對話框
                if (saveImgDialog.ShowDialog() != DialogResult.OK)
                    return;

                // 3. Save image
                string ImageFormat = ((enu_ImageFormat)(saveImgDialog.FilterIndex - 1)).ToString();
                string fileName = saveImgDialog.FileName;
                HObject imgSaved = null;
                imgSaved = ResultImages_UsedRegion[cbx_ResultImgID_B.SelectedIndex];
                HOperatorSet.WriteImage(imgSaved, ImageFormat, 0, fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "另存影像失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 【計算使用範圍】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Compute_UsedRegion_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            try
            {
                List<HObject> inspRegionList;
                List<string> name_UsedRegionList;
                if (Get_InspRegionList(out inspRegionList, out name_UsedRegionList))
                {
                    HObject SrcImg_Match = Get_SourceImage_Match(); // (20200130) Jeff Revised!
                    ClearDisplay(SrcImg_Match);
                    HOperatorSet.SetDraw(DisplayWindows.HalconWindow, DisplayType.ToString());
                    HOperatorSet.SetColored(DisplayWindows.HalconWindow, 12);
                    foreach (HObject reg in inspRegionList)
                    {
                        HOperatorSet.DispObj(reg, DisplayWindows.HalconWindow);
                        reg.Dispose();
                    }
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        private bool Get_InspRegionList(out List<HObject> inspRegionList, out List<string> name_UsedRegionList, bool b_ParamGetFromUI = true) // (20200130) Jeff Revised!
        {
            inspRegionList = new List<HObject>();
            name_UsedRegionList = new List<string>();

            // 檢查是否已讀取影像
            if (!Check_Input_ImgList())
                return false;

            if (b_ParamGetFromUI)
                ParamGetFromUI();

            bool b_status_ = false;
            try
            {
                HTuple Row = null, Column = null, Angle = null, Score = null;
                HObject CellRegion;

                HObject SrcImg_Match = Get_SourceImage_Match(); // (20200130) Jeff Revised!
                InductanceInsp.PtternMatchAndSegRegion_New(SrcImg_Match, Recipe, out PatternRegions, out Angle, out Row, out Column, out CellRegion);
                if (Angle.Length <= 0)
                {
                    MessageBox.Show("未搜尋到此特徵,可能為分數、角度、亮暗差異過大");
                    return false;
                }

                for (int i = 0; i < Recipe.Param.UsedRegionList.Count; i++)
                {
                    HObject InspRegion;
                    HOperatorSet.GenEmptyObj(out InspRegion);
                    if (!InductanceInsp.GetUsedRegion(PatternRegions, Row, Column, Angle, Score, Recipe, i, CellRegion, out InspRegion))
                        continue;
                    inspRegionList.Add(InspRegion);
                    name_UsedRegionList.Add(Recipe.Param.UsedRegionList[i].Name);
                }
                CellRegion.Dispose();

                b_status_ = true;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }

            return b_status_;
        }

        /// <summary>
        /// 設定 AlgoImg_UsedRegion 之使用編輯範圍相關資訊
        /// </summary>
        /// <returns></returns>
        private bool AlgoImg_UsedRegion_Constructor()
        {
            List<HObject> inspRegionList;
            List<string> name_UsedRegionList;
            if (!(Get_InspRegionList(out inspRegionList, out name_UsedRegionList)))
                return false;
            else
                AlgoImg_UsedRegion.Algo_Image_Constructor(true, inspRegionList, name_UsedRegionList);
            return true;
        }

        /// <summary>
        /// 【執行】 (使用範圍列表)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Execute_Algo_UsedRegion_Click(object sender, EventArgs e) // (20200729) Jeff Revised!
        {
            if (!AlgoImg_UsedRegion_Constructor())
                return;

            // 執行所有演算法計算
            AlgoImg_UsedRegion.ImageSource = Input_ImgList;
            clsStaticTool.DisposeAll(this.ResultImages_UsedRegion);
            clsStaticTool.DisposeAll(this.ResultRegions_UsedRegion);
            if (!(AlgoImg_UsedRegion.Execute(out ResultImages_UsedRegion, out this.ResultRegions_UsedRegion)))
                return;

            // 更新【結果影像B(編號)】&【結果區域B(編號)】
            this.cbx_ResultImgID_B.Items.Clear();
            this.cbx_ResultRegID_B.Items.Clear();
            for (int i = 0; i < this.AlgoImg_UsedRegion.ListAlgoImage.Count; i++)
            {
                this.cbx_ResultImgID_B.Items.Add(i.ToString());
                this.cbx_ResultRegID_B.Items.Add(i.ToString());
            }
            
            // 更新 Recipe 內之物件
            Recipe.Param.AlgoImg_UsedRegion = this.AlgoImg_UsedRegion; // (20200130) Jeff Revised!
        }

        /// <summary>
        /// 顯示單一演算法結果 (使用範圍列表)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView_EditAlgo_UsedRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView_EditAlgo_UsedRegion.SelectedIndices.Count <= 0)
                return;

            try
            {
                int id_ResultImg = listView_EditAlgo_UsedRegion.SelectedIndices[0];
                if (cbx_ResultImgID_B.SelectedIndex != id_ResultImg)
                    cbx_ResultImgID_B.SelectedIndex = id_ResultImg;
                else
                    cbx_ResultImgID_B_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                string message = "Error: " + ex.Message;
                MessageBox.Show(message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 【新增】 (使用範圍列表)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Add_Algo_UsedRegion_Click(object sender, EventArgs e)
        {
            if (!AlgoImg_UsedRegion_Constructor())
                return;

            int id_ResultImg = listView_EditAlgo_UsedRegion.Items.Count;
            AlgoImg_UsedRegion.ImageSource = Input_ImgList;
            using (Algo_Image_Form form = new Algo_Image_Form(AlgoImg_UsedRegion, id_ResultImg, ref ResultImages_UsedRegion, ref this.ResultRegions_UsedRegion)) // (20200729) Jeff Revised!
            {
                if (form.ShowDialog() == DialogResult.Yes)
                {
                    AlgoImg_UsedRegion = form.AlgoImg_yes;
                    AlgoImg_UsedRegion.Update_listView_Edit(listView_EditAlgo_UsedRegion); // 更新 listView_EditAlgo_UsedRegion
                    btn_Execute_Algo_UsedRegion_Click(null, null); //【執行】 (使用範圍列表)
                    cbx_ResultImgID_B.SelectedIndex = id_ResultImg; //【結果影像B(編號)】: 顯示結果影像
                }
            }
        }

        /// <summary>
        /// 【編輯】 (使用範圍列表)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Edit_Algo_UsedRegion_Click(object sender, EventArgs e)
        {
            if (!AlgoImg_UsedRegion_Constructor())
                return;

            if (listView_EditAlgo_UsedRegion.SelectedIndices.Count <= 0)
            {
                ctrl_timer1 = listView_EditAlgo_UsedRegion;
                BackColor_ctrl_timer1_1 = Color.AliceBlue;
                BackColor_ctrl_timer1_2 = Color.Green;
                timer1.Enabled = true;
                SystemSounds.Exclamation.Play();
                MessageBox.Show("請選擇演算法編號", "溫馨提醒", MessageBoxButtons.OK, MessageBoxIcon.Information);
                timer1.Enabled = false;
                ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
                return;
            }

            int id_ResultImg = listView_EditAlgo_UsedRegion.SelectedIndices[0];
            AlgoImg_UsedRegion.ImageSource = Input_ImgList;
            using (Algo_Image_Form form = new Algo_Image_Form(AlgoImg_UsedRegion, id_ResultImg, ref ResultImages_UsedRegion, ref this.ResultRegions_UsedRegion)) // (20200729) Jeff Revised!
            {
                if (form.ShowDialog() == DialogResult.Yes)
                {
                    AlgoImg_UsedRegion = form.AlgoImg_yes;
                    AlgoImg_UsedRegion.Update_listView_Edit(listView_EditAlgo_UsedRegion); // 更新 listView_EditAlgo_UsedRegion
                    btn_Execute_Algo_UsedRegion_Click(null, null); //【執行】 (使用範圍列表)
                    cbx_ResultImgID_B.SelectedIndex = id_ResultImg; //【結果影像B(編號)】: 顯示結果影像
                }
            }
        }

        /// <summary>
        /// 【刪除】 (使用範圍列表)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Remove_Algo_UsedRegion_Click(object sender, EventArgs e)
        {
            if (listView_EditAlgo_UsedRegion.SelectedIndices.Count <= 0)
            {
                ctrl_timer1 = listView_EditAlgo_UsedRegion;
                BackColor_ctrl_timer1_1 = Color.AliceBlue;
                BackColor_ctrl_timer1_2 = Color.Green;
                timer1.Enabled = true;
                SystemSounds.Exclamation.Play();
                MessageBox.Show("請選擇演算法編號", "溫馨提醒", MessageBoxButtons.OK, MessageBoxIcon.Information);
                timer1.Enabled = false;
                ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
                return;
            }

            DialogResult dr = MessageBox.Show("確定要刪除演算法編號【" + listView_EditAlgo_UsedRegion.SelectedItems[0].Text + "】?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No)
                return;

            if (!AlgoImg_UsedRegion.Remove_1_Algo(listView_EditAlgo_UsedRegion.SelectedIndices[0]))
                return;

            AlgoImg_UsedRegion.Update_listView_Edit(listView_EditAlgo_UsedRegion); // 更新 listView_EditAlgo_UsedRegion
            btn_Execute_Algo_UsedRegion_Click(null, null); //【執行】 (使用範圍列表)
        }
        
        /// <summary>
        /// 【清空】 (使用範圍列表)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_RemoveAll_Algo_UsedRegion_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("確定要清空所有演算法編號?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No)
                return;

            //AlgoImg.ListAlgoImage.Clear();
            AlgoImg_UsedRegion.RemoveAll_Algo();
            AlgoImg_UsedRegion.Update_listView_Edit(listView_EditAlgo_UsedRegion); // 更新 listView_EditAlgo_UsedRegion
            btn_Execute_Algo_UsedRegion_Click(null, null); //【執行】 (使用範圍列表)
        }

        #endregion

        #endregion

        #region Timer

        private Control ctrl_timer1 { get; set; } = null;
        private Color BackColor_ctrl_timer1_1 = Color.Transparent, BackColor_ctrl_timer1_2 = Color.Green;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (ctrl_timer1 == null) return;

            if (ctrl_timer1.BackColor == BackColor_ctrl_timer1_1)
                ctrl_timer1.BackColor = BackColor_ctrl_timer1_2;
            else
                ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
        }
        
        private Control ctrl_timer2 { get; set; } = null;
        private int Count_timer2_Tick = 0, MaxCount_timer2_Tick = 15;
        private Color BackColor_ctrl_timer2 = System.Drawing.SystemColors.Control;
        private void timer2_Tick(object sender, EventArgs e)
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
        
    }

}
