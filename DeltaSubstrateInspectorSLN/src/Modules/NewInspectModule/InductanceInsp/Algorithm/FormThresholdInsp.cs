using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using HalconDotNet;
using System.Threading;
using System.Xml.Serialization;
using System.IO;

namespace DeltaSubstrateInspector.src.Modules.NewInspectModule.InductanceInsp
{
    public partial class FormThresholdInsp : Form
    {
        clsRecipe.clsThresholdInsp Param;
        /// <summary>
        /// 原始影像集合
        /// </summary>
        List<HObject> ImageList { get; set; } = null;

        /// <summary>
        /// 結果影像集合 (不使用範圍列表)
        /// </summary>
        List<HObject> ResultImages { get; set; } // (20200130) Jeff Revised!

        /// <summary>
        /// 結果影像集合 (使用範圍列表)
        /// </summary>
        List<HObject> ResultImages_UsedRegion { get; set; } // (20200130) Jeff Revised!

        HSmartWindowControl Display;
        InductanceInspRole Recipe;
        clsRecipe.clsAlgorithm AlgorithmParam;

        public delegate void ChangeRegionHandler(HObject ExportRegion); // 定義委派
        public event ChangeRegionHandler ChangeRegion; // 定義事件

        public delegate void FormClosedHandler(bool FormClosed); // 定義委派
        public event FormClosedHandler FormClosedEvent; // 定義事件
        
        public FormThresholdInsp(List<HObject> pmImageList, HSmartWindowControl pmDisplay, InductanceInspRole pmRecipe, clsRecipe.clsAlgorithm pmAlgorithmParam, List<HObject> resultImages = null, List<HObject> resultImages_UsedRegion = null) // (20200130) Jeff Revised!
        {
            InitializeComponent();

            this.ImageList = pmImageList;
            this.Display = pmDisplay;
            this.Recipe = pmRecipe;
            this.AlgorithmParam = pmAlgorithmParam;
            this.Param = (clsRecipe.clsThresholdInsp)AlgorithmParam.Param;
            this.ResultImages = resultImages; // (20200130) Jeff Revised!
            this.ResultImages_UsedRegion = resultImages_UsedRegion; // (20200130) Jeff Revised!

            InsertCombobox2UI();
            //nudImageIndex.Maximum = ImageList.Count - 1; // (20200130) Jeff Revised!
            nudSecondImgIndex.Maximum = ImageList.Count - 1;
            UpdateControl();

            clsLanguage.clsLanguage.SetLanguateToControls(this); // (20200130) Jeff Revised!
        }

        public void InsertCombobox2UI()
        {
            string[] ArrayEnum = Enum.GetNames(typeof(enuBand));
            comboBoxMultiTHBand.Items.Clear();
            comboBoxSecondImgBand.Items.Clear();
            foreach (string s in ArrayEnum)
            {
                comboBoxMultiTHBand.Items.Add(s);
                comboBoxSecondImgBand.Items.Add(s);
            }

            string[] ArrayMethod = Enum.GetNames(typeof(InductanceInsp.enuThresholdType));
            comboBoxThType.Items.Clear();
            foreach (string S in ArrayMethod)
            {
                comboBoxThType.Items.Add(S);
            }
        }

        /// <summary>
        /// 更新各控制項參數
        /// </summary>
        public void UpdateControl() // (20200130) Jeff Revised!
        {
            #region 基本參數

            foreach (clsRecipe.clsMethodRegion Region in Recipe.Param.UsedRegionList)
            {
                comboBoxRegionSelect.Items.Add(Region.Name);
            }
            if (comboBoxRegionSelect.Items.Count > AlgorithmParam.SelectRegionIndex)
                comboBoxRegionSelect.SelectedIndex = AlgorithmParam.SelectRegionIndex;
            txbInspName.Text = AlgorithmParam.DefectName;
            cbxInspEnabled.Checked = Param.Enabled;

            comboBoxMultiTHBand.SelectedIndexChanged -= comboBoxMultiTHBand_SelectedIndexChanged; // (20200130) Jeff Revised!
            comboBoxMultiTHBand.SelectedIndex = (int)AlgorithmParam.Band;
            comboBoxMultiTHBand.SelectedIndexChanged += comboBoxMultiTHBand_SelectedIndexChanged;

            comboBox_ImageSource.SelectedIndex = (int)AlgorithmParam.ImageSource; // (20200130) Jeff Revised!
            nudImageIndex.Value = decimal.Parse(AlgorithmParam.InspImageIndex.ToString());
            nudInspEdgeSkipSize.Value = decimal.Parse(Param.EdgeSkipSizeWidth.ToString());
            nudInspEdgeSkipHeight.Value = decimal.Parse(Param.EdgeSkipSizeHeight.ToString());
            nudLTh.Value = decimal.Parse(Param.LTH.ToString());
            nudHTH.Value = decimal.Parse(Param.HTH.ToString());
            nudAreaMin.Value = decimal.Parse(Param.AreaMin.ToString());
            nudHeightMin.Value = decimal.Parse(Param.HeightMin.ToString());
            nudWidthMin.Value = decimal.Parse(Param.WidthMin.ToString());
            cbxInspAreaEnabled.Checked = Param.AEnabled;
            cbxInspWidthEnabled.Checked = Param.WEnabled;
            cbxInspHeightEnabled.Checked = Param.HEnabled;

            nudAutoThSigma.Value = decimal.Parse(Param.Sigma.ToString());
            comboBoxSelectMean.SelectedIndex = Param.SelectMean;

            nudOffset.Value = decimal.Parse(Param.Offset.ToString());
            nudMeanImageWidth.Value = decimal.Parse(Param.GrayMeanWidth.ToString());
            nudMeanImageHeight.Value = decimal.Parse(Param.GrayMeanHeight.ToString());
            nudExtCircleSize.Value = decimal.Parse(Param.ExtSizeCircle.ToString());
            comboBoxDynType.SelectedIndex = (int)Param.DynType;

            cbxAIInspection.Checked = AlgorithmParam.bUsedDAVS;
            cbxEdgeEditEnabled.Checked = Param.bEdgeEdit;

            comboBoxThType.SelectedIndex = (int)Param.ThType;

            #endregion

            #region 灰階選擇

            nudGraySelectMin.Value = decimal.Parse(Param.GrayMin.ToString());
            nudGraySelectMax.Value = decimal.Parse(Param.GrayMax.ToString());
            cbxGraySelectEnabled.Checked = Param.GrayEnabled;
            comboBoxGraySelectType.SelectedIndex = (int)Param.GraySelectType;

            #endregion

            #region 特徵篩選

            nudcircularityMin.Value = decimal.Parse(Param.CircularityMin.ToString());
            nudcircularityMax.Value = decimal.Parse(Param.CircularityMax.ToString());

            nudroundnessMin.Value = decimal.Parse(Param.RoundnessMin.ToString());
            nudroundnessMax.Value = decimal.Parse(Param.RoundnessMax.ToString());

            nudrectangularityMin.Value = decimal.Parse(Param.RectangularityMin.ToString());
            nudrectangularityMax.Value = decimal.Parse(Param.RectangularityMax.ToString());

            nudconvexityMin.Value = decimal.Parse(Param.ConvexityMin.ToString());
            nudconvexityMax.Value = decimal.Parse(Param.ConvexityMax.ToString());

            nudholes_numMin.Value = decimal.Parse(Param.Holes_numMin.ToString());
            nudholes_numMax.Value = decimal.Parse(Param.Holes_numMax.ToString());

            nudInner_radiusMin.Value = decimal.Parse(Param.Inner_radiusMin.ToString());
            nudInner_radiusMax.Value = decimal.Parse(Param.Inner_radiusMax.ToString());

            nudSecondImgIndex.Value = decimal.Parse(AlgorithmParam.SecondSrcImgID.ToString());
            nudSecondSrcGrayMIn.Value = decimal.Parse(Param.SecondSrcGrayMin.ToString());
            nudSecondSrcGrayMax.Value = decimal.Parse(Param.SecondSrcGrayMax.ToString());
            comboBoxSecondImgBand.SelectedIndexChanged -= comboBoxSecondImgBand_SelectedIndexChanged;
            comboBoxSecondImgBand.SelectedIndex = (int)AlgorithmParam.SecondImgBand;
            comboBoxSecondImgBand.SelectedIndexChanged += comboBoxSecondImgBand_SelectedIndexChanged;
            comboBoxSecondSrcGgrayType.SelectedIndex = (int)Param.SecondSrcGraySelectType;
            nudanisometryMin.Value = decimal.Parse(Param.anisometryMin.ToString());
            nudanisometryMax.Value = decimal.Parse(Param.anisometryMax.ToString());


            cbxSecondSrcEnabled.Checked = Param.bSecondSrcEnabled;
            cbxcircularityEnabled.Checked = Param.CircularityEnabled;
            cbxroundnessEnabled.Checked = Param.RoundnessEnabled;
            cbxconvexityEnabled.Checked = Param.ConvexityEnabled;
            cbxrectangularityEnabled.Checked = Param.RectangularityEnabled;
            cbxholes_numEnabled.Checked = Param.Holes_numEnabled;
            cbxInner_radiusEnabled.Checked = Param.bInnerCircleEnabled;
            cbxanisometryEnabled.Checked = Param.banisometryEnabled;


            #endregion

            #region BinaryTh

            comboBoxBinaryMethod.SelectedIndex = (int)Param.BinaryThType;
            comboBoxLightDark.SelectedIndex = (int)Param.BinaryThSearchType;

            #endregion

            comboBoxAreaOperantin.SelectedIndex = (int)Param.AreaOperation;
            comboBoxWidthOperation.SelectedIndex = (int)Param.WidthOperation;
            comboBoxHeightOperation.SelectedIndex = (int)Param.HeightOperation;

            nudAreaMax.Value = decimal.Parse(Param.AreaMax.ToString());
            nudWidthMax.Value = decimal.Parse(Param.WidthMax.ToString());
            nudHeightMax.Value = decimal.Parse(Param.HeightMax.ToString());

            nudAcceptable_Value.Value = decimal.Parse(Param.Acceptable_Value.ToString());
            cbxAcceptable_Value.Checked = Param.bAcceptable_ValueEnabled;

        }

        private void cbxInspEnabled_CheckedChanged(object sender, EventArgs e)
        {
            Param.Enabled = cbxInspEnabled.Checked;
        }

        private void btnSetupName_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txbInspName.Text))
            {
                MessageBox.Show("請先輸入瑕疵名稱");
                return;
            }
            AlgorithmParam.DefectName = txbInspName.Text;
        }
        
        private void nudInspEdgeSkipSize_ValueChanged(object sender, EventArgs e)
        {
            Param.EdgeSkipSizeWidth = int.Parse(nudInspEdgeSkipSize.Value.ToString());
        }

        private void nudInspEdgeSkipHeight_ValueChanged(object sender, EventArgs e)
        {
            Param.EdgeSkipSizeHeight = int.Parse(nudInspEdgeSkipHeight.Value.ToString());
        }

        private void nudLTh_ValueChanged(object sender, EventArgs e)
        {
            Param.LTH = int.Parse(nudLTh.Value.ToString());
        }

        private void nudHTH_ValueChanged(object sender, EventArgs e)
        {
            Param.HTH = int.Parse(nudHTH.Value.ToString());
        }

        private void nudAreaMin_ValueChanged(object sender, EventArgs e)
        {
            Param.AreaMin = int.Parse(nudAreaMin.Value.ToString());
        }

        private void nudWidthMin_ValueChanged(object sender, EventArgs e)
        {
            Param.WidthMin = int.Parse(nudWidthMin.Value.ToString());
        }

        private void nudHeightMin_ValueChanged(object sender, EventArgs e)
        {
            Param.HeightMin = int.Parse(nudHeightMin.Value.ToString());
        }

        private void cbxInspAreaEnabled_CheckedChanged(object sender, EventArgs e)
        {
            Param.AEnabled = cbxInspAreaEnabled.Checked;
        }

        private void cbxInspWidthEnabled_CheckedChanged(object sender, EventArgs e)
        {
            Param.WEnabled = cbxInspWidthEnabled.Checked;
        }

        private void cbxInspHeightEnabled_CheckedChanged(object sender, EventArgs e)
        {
            Param.HEnabled = cbxInspHeightEnabled.Checked;
        }

        /// <summary>
        /// 檢查是否已讀取影像
        /// </summary>
        /// <returns></returns>
        private bool Check_ImageList() // (20200130) Jeff Revised!
        {
            if (ImageList == null || ImageList.Count <= 0)
            {
                MessageBox.Show("請先讀取影像");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 【設定】 (一般二值化設定)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnThSetup_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            // 檢查是否已讀取影像
            if (!Check_ImageList())
                return;

            HObject InspRegion;
            HObject CellRegion;
            HTuple Row = null, Column = null, Angle = null, Score = null;
            HObject PatternRegions;
            InductanceInsp.PtternMatchAndSegRegion_New(Get_SourceImage_Match(), Recipe, out PatternRegions, out Angle, out Row, out Column, out CellRegion); // (20200130) Jeff Revised!
            //InductanceInsp.GetMatchInfo(SrcImg_Match, Recipe, out Row, out Column, out Angle, out Score);

            //if (Score.Length <= 0)
            //{
            //    MessageBox.Show("未搜尋到任何圖樣");
            //    return;
            //}

            //InductanceInsp.GetCellRegion(Row, Column, Angle, Score, Recipe, out CellRegion);
            InductanceInsp.GetUsedRegion(PatternRegions, Row, Column, Angle, Score, Recipe, AlgorithmParam.SelectRegionIndex, CellRegion, out InspRegion);

            HObject Union;
            HOperatorSet.Union1(InspRegion, out Union);

            HObject ThImage = clsStaticTool.GetChannelImage(Get_SourceImage(), AlgorithmParam.Band); // (20200130) Jeff Revised!

            HObject ReduceImg;
            HOperatorSet.ReduceDomain(ThImage, Union, out ReduceImg);

            FormThresholdAdjust MyForm = new FormThresholdAdjust(ReduceImg,
                                                                 int.Parse(nudLTh.Value.ToString()),
                                                                 int.Parse(nudHTH.Value.ToString()), FormThresholdAdjust.enType.Dual);
            MyForm.ShowDialog();

            if (MyForm.DialogResult == DialogResult.OK)
            {
                int ThMin, ThMax;
                MyForm.GetThreshold(out ThMin, out ThMax);

                nudLTh.Value = ThMin;
                nudHTH.Value = ThMax;
            }
            ThImage.Dispose();
            ReduceImg.Dispose();
            Union.Dispose();
            CellRegion.Dispose();
            InspRegion.Dispose();
        }

        /// <summary>
        /// 【顯示範圍】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInspRegionTest_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            // 檢查是否已讀取影像
            if (!Check_ImageList())
                return;

            try
            {
                HOperatorSet.ClearWindow(Display.HalconWindow);
                HOperatorSet.DispObj(Get_SourceImage(), Display.HalconWindow); // (20200130) Jeff Revised!
                HObject InspRegion;
                HObject CellRegion;

                HTuple Row = null, Column = null, Angle = null, Score = null;
                HObject PatternRegions;
                InductanceInsp.PtternMatchAndSegRegion_New(Get_SourceImage_Match(), Recipe, out PatternRegions, out Angle, out Row, out Column, out CellRegion); // (20200130) Jeff Revised!
                //InductanceInsp.GetMatchInfo(SrcImg_Match, Recipe, out Row, out Column, out Angle, out Score);
                //InductanceInsp.GetCellRegion(Row, Column, Angle, Score, Recipe, out CellRegion);

                InductanceInsp.GetUsedRegion(PatternRegions, Row, Column, Angle, Score, Recipe, AlgorithmParam.SelectRegionIndex, CellRegion, out InspRegion);

                HObject ErosionRegion, DilationRegion;
                HOperatorSet.GenEmptyObj(out ErosionRegion);
                HOperatorSet.GenEmptyObj(out DilationRegion);

                if (Param.bEdgeEdit)
                {
                    HOperatorSet.ErosionRectangle1(InspRegion, out ErosionRegion, Param.EdgeSkipSizeWidth, Param.EdgeSkipSizeHeight);

                    HOperatorSet.DilationCircle(ErosionRegion, out DilationRegion, Param.ExtSizeCircle);
                }
                else
                {
                    DilationRegion = InspRegion.Clone();
                }

                HOperatorSet.SetDraw(Display.HalconWindow, "margin");
                HOperatorSet.SetColored(Display.HalconWindow, 12);
                HOperatorSet.DispObj(DilationRegion, Display.HalconWindow);

                ErosionRegion.Dispose();
                InspRegion.Dispose();
                CellRegion.Dispose();
            }
            catch(Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        /// <summary>
        /// 【Region ID : 】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxRegionSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxRegionSelect.SelectedIndex < 0)
                return;

            AlgorithmParam.SelectRegionIndex = comboBoxRegionSelect.SelectedIndex;
        }

        /// <summary>
        /// 來源影像切換 (演算法參數設定)，調整【影像ID:】控制項上限
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_ImageSource_SelectedIndexChanged(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            AlgorithmParam.ImageSource = (enu_ImageSource)(comboBox_ImageSource.SelectedIndex);

            // 調整【影像ID:】控制項上限
            if (AlgorithmParam.ImageSource == enu_ImageSource.原始)
                nudImageIndex.Maximum = ImageList.Count - 1;
            else if (AlgorithmParam.ImageSource == enu_ImageSource.影像A)
            {
                if (ResultImages.Count > 0)
                    nudImageIndex.Maximum = ResultImages.Count - 1;
                else
                {
                    MessageBox.Show("影像A數量為0", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    comboBox_ImageSource.SelectedIndex = 0; // 原始影像
                }
            }
            else
            {
                if (ResultImages_UsedRegion.Count > 0)
                    nudImageIndex.Maximum = ResultImages_UsedRegion.Count - 1;
                else
                {
                    MessageBox.Show("影像B數量為0", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    comboBox_ImageSource.SelectedIndex = 0; // 原始影像
                }
            }
        }

        /// <summary>
        /// 【影像ID:】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nudImageIndex_ValueChanged(object sender, EventArgs e)
        {
            AlgorithmParam.InspImageIndex = int.Parse(nudImageIndex.Value.ToString());
        }

        /// <summary>
        /// 【頻帶:】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxMultiTHBand_SelectedIndexChanged(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            if (comboBoxMultiTHBand.SelectedIndex < 0)
            {
                MessageBox.Show("請先選擇Band");
                return;
            }

            AlgorithmParam.Band = (enuBand)comboBoxMultiTHBand.SelectedIndex;

            HObject DisplayImage = clsStaticTool.GetChannelImage(Get_SourceImage(), AlgorithmParam.Band); // (20200130) Jeff Revised!
            HOperatorSet.ClearWindow(Display.HalconWindow);
            HOperatorSet.DispObj(DisplayImage, Display.HalconWindow);
            DisplayImage.Dispose();
        }

        /// <summary>
        /// 取得來源影像
        /// </summary>
        /// <returns></returns>
        private HObject Get_SourceImage() // (20200130) Jeff Revised!
        {
            HObject SrcImg;
            if (AlgorithmParam.ImageSource == enu_ImageSource.原始)
                SrcImg = ImageList[AlgorithmParam.InspImageIndex];
            else if (AlgorithmParam.ImageSource == enu_ImageSource.影像A)
                SrcImg = ResultImages[AlgorithmParam.InspImageIndex];
            else
                SrcImg = ResultImages_UsedRegion[AlgorithmParam.InspImageIndex];

            return SrcImg;
        }

        /// <summary>
        ///  取得來源影像 (圖像教導)
        /// </summary>
        /// <returns></returns>
        private HObject Get_SourceImage_Match() // (20200130) Jeff Revised!
        {
            HObject SrcImg_Match;
            if (Recipe.Param.SegParam.ImageSource_SegImg == enu_ImageSource.原始)
                SrcImg_Match = ImageList[Recipe.Param.SegParam.SegImgIndex];
            else
                SrcImg_Match = ResultImages[Recipe.Param.SegParam.SegImgIndex];

            return SrcImg_Match;
        }

        private void comboBoxThType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxThType.SelectedIndex < 0)
                return;

            Param.ThType = (InductanceInsp.enuThresholdType)comboBoxThType.SelectedIndex;

            tbThresholdType.SelectedIndex = comboBoxThType.SelectedIndex;
        }

        private void tbThresholdType_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tb = sender as TabControl;
            comboBoxThType.SelectedIndex = tb.SelectedIndex;
        }

        private void nudAutoThSigma_ValueChanged(object sender, EventArgs e)
        {
            Param.Sigma = double.Parse(nudAutoThSigma.Value.ToString());
        }

        HObject DisplayRegion = null;
        /// <summary>
        /// 【測試】 (自動二值化設定)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAutoThTest_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            // 檢查是否已讀取影像
            if (!Check_ImageList())
                return;

            HObject SrcImg = Get_SourceImage(); // (20200130) Jeff Revised!
            HOperatorSet.ClearWindow(Display.HalconWindow);
            HOperatorSet.DispObj(SrcImg, Display.HalconWindow); // (20200130) Jeff Revised!

            HObject InspRegion;
            HObject CellRegion;

            HTuple Row = null, Column = null, Angle = null, Score = null;

            try
            {
                HObject PatternRegions;
                InductanceInsp.PtternMatchAndSegRegion_New(Get_SourceImage_Match(), Recipe, out PatternRegions, out Angle, out Row, out Column, out CellRegion); // (20200130) Jeff Revised!

                InductanceInsp.GetUsedRegion(PatternRegions, Row, Column, Angle, Score, Recipe, AlgorithmParam.SelectRegionIndex, CellRegion, out InspRegion);
                HObject Dilation;
                if (cbxEdgeEditEnabled.Checked)
                {
                    HObject Erosion;
                    HOperatorSet.ErosionRectangle1(InspRegion, out Erosion, Param.EdgeSkipSizeWidth, Param.EdgeSkipSizeHeight);


                    HOperatorSet.DilationCircle(Erosion, out Dilation, Param.ExtSizeCircle);
                }
                else
                {
                    Dilation = InspRegion.Clone();
                }

                HObject Union;
                HOperatorSet.Union1(Dilation, out Union);

                HObject InspImage = clsStaticTool.GetChannelImage(SrcImg, AlgorithmParam.Band); // (20200130) Jeff Revised!

                HObject ReduceImg;
                HOperatorSet.ReduceDomain(InspImage, Union, out ReduceImg);

                HObject ThRegion;
                HOperatorSet.AutoThreshold(ReduceImg, out ThRegion, Param.Sigma);

                if (DisplayRegion != null)
                {
                    DisplayRegion.Dispose();
                    HOperatorSet.GenEmptyObj(out DisplayRegion);
                }
                DisplayRegion = ThRegion;
                HTuple Count;
                HOperatorSet.CountObj(DisplayRegion, out Count);
                comboBoxAutoRegion.Items.Clear();
                for (int i = 0; i < Count; i++)
                {
                    comboBoxAutoRegion.Items.Add(i.ToString());
                }

                HOperatorSet.SetDraw(Display.HalconWindow, "fill");
                HOperatorSet.SetColored(Display.HalconWindow, 12);
                HOperatorSet.DispObj(ThRegion, Display.HalconWindow);

                ReduceImg.Dispose();
                Union.Dispose();
                Dilation.Dispose();
                InspImage.Dispose();
            }
            catch(Exception E)
            {
                MessageBox.Show(E.ToString());
            }

        }

        private void comboBoxSelectMean_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSelectMean.SelectedIndex < 0)
                return;

            Param.SelectMean = comboBoxSelectMean.SelectedIndex;
        }

        private void nudMeanImageWidth_ValueChanged(object sender, EventArgs e)
        {
            Param.GrayMeanWidth = int.Parse(nudMeanImageWidth.Value.ToString());
        }

        private void nudMeanImageHeight_ValueChanged(object sender, EventArgs e)
        {
            Param.GrayMeanHeight = int.Parse(nudMeanImageHeight.Value.ToString());
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Param.Offset = int.Parse(nudOffset.Value.ToString());
        }

        private void comboBoxDynType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxDynType.SelectedIndex < 0)
                return;

            Param.DynType = (InductanceInsp.enuDynThresholdType)comboBoxDynType.SelectedIndex;
        }

        /// <summary>
        /// 【測試】 (動態二值化設定)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDynThTest_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            // 檢查是否已讀取影像
            if (!Check_ImageList())
                return;

            HObject SrcImg = Get_SourceImage(); // (20200130) Jeff Revised!
            HOperatorSet.ClearWindow(Display.HalconWindow);
            HOperatorSet.DispObj(SrcImg, Display.HalconWindow); // (20200130) Jeff Revised!

            HObject InspRegion;
            HObject CellRegion;
            HTuple Row = null, Column = null, Angle = null, Score = null;
            //InductanceInsp.GetMatchInfo(SrcImg_Match, Recipe, out Row, out Column, out Angle, out Score);

            //if (Score.Length <= 0)
            //{
            //    MessageBox.Show("未搜尋到任何圖樣");
            //    return;
            //}
            try
            {
                HObject PatternRegions;
                InductanceInsp.PtternMatchAndSegRegion_New(Get_SourceImage_Match(), Recipe, out PatternRegions, out Angle, out Row, out Column, out CellRegion); // (20200130) Jeff Revised!
                //InductanceInsp.GetCellRegion(Row, Column, Angle, Score, Recipe, out CellRegion);
                InductanceInsp.GetUsedRegion(PatternRegions, Row, Column, Angle, Score, Recipe, AlgorithmParam.SelectRegionIndex, CellRegion, out InspRegion);

                HObject Union;
                HOperatorSet.Union1(InspRegion, out Union);

                HObject InspImage = clsStaticTool.GetChannelImage(SrcImg, AlgorithmParam.Band); // (20200130) Jeff Revised!

                HObject ReduceImg;
                HOperatorSet.ReduceDomain(InspImage, Union, out ReduceImg);
                HObject MeanImage;
                HObject ThRegion;

                HOperatorSet.MeanImage(ReduceImg, out MeanImage, Param.GrayMeanWidth, Param.GrayMeanHeight);
                HOperatorSet.DynThreshold(ReduceImg, MeanImage, out ThRegion, Param.Offset, Param.DynType.ToString());


                HOperatorSet.SetDraw(Display.HalconWindow, "fill");
                HOperatorSet.SetColored(Display.HalconWindow, 12);
                HOperatorSet.DispObj(ThRegion, Display.HalconWindow);

                ReduceImg.Dispose();
                MeanImage.Dispose();
                Union.Dispose();
                InspImage.Dispose();
            }
            catch(Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
                return;
            }
        }

        private void cbxAIInspection_CheckedChanged(object sender, EventArgs e)
        {
            AlgorithmParam.bUsedDAVS = cbxAIInspection.Checked;
        }

        private void comboBoxAutoRegion_SelectedIndexChanged(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            HOperatorSet.ClearWindow(Display.HalconWindow);
            HOperatorSet.DispObj(Get_SourceImage(), Display.HalconWindow); // (20200130) Jeff Revised!
            HObject R;
            HOperatorSet.SelectObj(DisplayRegion, out R, comboBoxAutoRegion.SelectedIndex + 1);

            HOperatorSet.SetDraw(Display.HalconWindow, "fill");
            HOperatorSet.SetColored(Display.HalconWindow, 12);
            HOperatorSet.DispObj(R, Display.HalconWindow);

            R.Dispose();
        }

        private void FormThresholdInsp_FormClosed(object sender, FormClosedEventArgs e)
        {
            FormClosedEvent(true);
        }

        private void nudGraySelectMin_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;

            Param.GrayMin = int.Parse(Obj.Value.ToString());
        }

        private void nudGraySelectMax_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;

            Param.GrayMax = int.Parse(Obj.Value.ToString());
        }

        private void cbxGraySelectEnabled_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox Obj = (CheckBox)sender;

            Param.GrayEnabled = Obj.Checked;
        }

        private void comboBoxGraySelectType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox Obj = (ComboBox)sender;
            if (Obj.SelectedIndex < 0)
                return;
            Param.GraySelectType = (enuGraySelectType)Obj.SelectedIndex;
        }

        private void comboBoxAreaOperantin_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox Obj = (ComboBox)sender;
            if (Obj.SelectedIndex < 0)
                return;

            Param.AreaOperation = (enuOperation)Obj.SelectedIndex;
        }

        private void comboBoxWidthOperation_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox Obj = (ComboBox)sender;
            if (Obj.SelectedIndex < 0)
                return;

            Param.WidthOperation = (enuOperation)Obj.SelectedIndex;
        }

        private void comboBoxHeightOperation_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox Obj = (ComboBox)sender;
            if (Obj.SelectedIndex < 0)
                return;

            Param.HeightOperation = (enuOperation)Obj.SelectedIndex;
        }

        private void nudAreaMax_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;
            Param.AreaMax = int.Parse(Obj.Value.ToString());
        }

        private void nudWidthMax_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;
            Param.WidthMax = int.Parse(Obj.Value.ToString());
        }

        private void nudHeightMax_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;
            Param.HeightMax = int.Parse(Obj.Value.ToString());
        }

        private void nudcircularityMin_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;
            Param.CircularityMin = double.Parse(Obj.Value.ToString());
        }

        private void nudcircularityMax_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;
            Param.CircularityMax = double.Parse(Obj.Value.ToString());
        }

        private void nudroundnessMin_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;
            Param.RoundnessMin = double.Parse(Obj.Value.ToString());
        }

        private void nudroundnessMax_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;
            Param.RoundnessMax = double.Parse(Obj.Value.ToString());
        }

        private void cbxcircularityEnabled_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox Obj = (CheckBox)sender;

            Param.CircularityEnabled = Obj.Checked;
        }

        private void cbxroundnessEnabled_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox Obj = (CheckBox)sender;

            Param.RoundnessEnabled = Obj.Checked;
        }

        private void nudconvexityMin_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;
            Param.ConvexityMin = double.Parse(Obj.Value.ToString());
        }

        private void nudconvexityMax_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;
            Param.ConvexityMax = double.Parse(Obj.Value.ToString());
        }

        private void nudrectangularityMin_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;
            Param.RectangularityMin = double.Parse(Obj.Value.ToString());
        }

        private void nudrectangularityMax_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;
            Param.RectangularityMax = double.Parse(Obj.Value.ToString());
        }

        private void nudholes_numMin_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;
            Param.Holes_numMin = int.Parse(Obj.Value.ToString());
        }

        private void nudholes_numMax_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;
            Param.Holes_numMax = int.Parse(Obj.Value.ToString());
        }

        private void cbxconvexityEnabled_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox Obj = (CheckBox)sender;

            Param.ConvexityEnabled = Obj.Checked;
        }

        private void cbxrectangularityEnabled_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox Obj = (CheckBox)sender;

            Param.RectangularityEnabled = Obj.Checked;
        }

        private void cbxholes_numEnabled_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox Obj = (CheckBox)sender;

            Param.Holes_numEnabled = Obj.Checked;
        }

        private void cbxAcceptable_Value_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox Obj = (CheckBox)sender;
            Param.bAcceptable_ValueEnabled = Obj.Checked;
        }

        private void nudAcceptable_Value_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;
            Param.Acceptable_Value = int.Parse(Obj.Value.ToString());
        }

        private void comboBoxBinaryMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox Obj = (ComboBox)sender;
            if (Obj.SelectedIndex < 0)
                return;
            Param.BinaryThType = (enuBinaryThType)Obj.SelectedIndex;
        }

        private void comboBoxLightDark_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox Obj = (ComboBox)sender;
            if (Obj.SelectedIndex < 0)
                return;
            Param.BinaryThSearchType = (enuBinaryThSearchType)Obj.SelectedIndex;
        }

        /// <summary>
        /// 【測試】 (BinaryThreshold)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBinaryThTest_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            // 檢查是否已讀取影像
            if (!Check_ImageList())
                return;

            HObject SrcImg = Get_SourceImage(); // (20200130) Jeff Revised!
            HOperatorSet.ClearWindow(Display.HalconWindow);
            HOperatorSet.DispObj(SrcImg, Display.HalconWindow); // (20200130) Jeff Revised!

            HObject InspRegion;
            HObject CellRegion;
            HTuple Row = null, Column = null, Angle = null, Score = null;
            //InductanceInsp.GetMatchInfo(SrcImg_Match, Recipe, out Row, out Column, out Angle, out Score);

            //if (Score.Length <= 0)
            //{
            //    MessageBox.Show("未搜尋到任何圖樣");
            //    return;
            //}

            try
            {
                //InductanceInsp.GetCellRegion(Row, Column, Angle, Score, Recipe, out CellRegion);
                HObject PatternRegions;
                InductanceInsp.PtternMatchAndSegRegion_New(Get_SourceImage_Match(), Recipe, out PatternRegions, out Angle, out Row, out Column, out CellRegion); // (20200130) Jeff Revised!
                InductanceInsp.GetUsedRegion(PatternRegions, Row, Column, Angle, Score, Recipe, AlgorithmParam.SelectRegionIndex, CellRegion, out InspRegion);

                HObject Erosion;
                HOperatorSet.ErosionRectangle1(InspRegion, out Erosion, Param.EdgeSkipSizeWidth, Param.EdgeSkipSizeHeight);

                HObject InspImage = clsStaticTool.GetChannelImage(SrcImg, AlgorithmParam.Band); // (20200130) Jeff Revised!

                HObject ReduceImg;
                HOperatorSet.ReduceDomain(InspImage, Erosion, out ReduceImg);

                HObject ThRegion;
                HTuple ThValue;
                HOperatorSet.BinaryThreshold(ReduceImg, out ThRegion, new HTuple(Param.BinaryThType.ToString()), new HTuple(Param.BinaryThSearchType.ToString()), out ThValue);
                
                HOperatorSet.SetDraw(Display.HalconWindow, "fill");
                HOperatorSet.SetColored(Display.HalconWindow, 12);
                HOperatorSet.DispObj(ThRegion, Display.HalconWindow);

            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
                return;
            }
        }

        public static bool ExportXML(clsRecipe.clsThresholdInsp SrcProduct, string PathFile)
        {
            bool Res = false;
            clsRecipe.clsThresholdInsp Product = clsStaticTool.Clone<clsRecipe.clsThresholdInsp>(SrcProduct);
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
                return Res;
            }

            Res = true;
            return Res;
        }

        private void btnExportParam_Click(object sender, EventArgs e)
        {
            SaveFileDialog Save_Dialog = new SaveFileDialog();
            Save_Dialog.Filter = "files (*.xml)|*.xml";
            if (Save_Dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    clsRecipe.clsThresholdInsp ExportParam = Param;

                    if (ExportXML(ExportParam, Save_Dialog.FileName))
                    {
                        MessageBox.Show("匯出成功");
                    }
                    else
                    {
                        MessageBox.Show("Fail...");
                    }
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.ToString());
                }


            }
        }

        public static clsRecipe.clsThresholdInsp ImportXML(string PathFile)
        {
            clsRecipe.clsThresholdInsp Res = new clsRecipe.clsThresholdInsp();
            try
            {
                XmlSerializer XmlS = new XmlSerializer(Res.GetType());
                Stream S = File.Open(PathFile, FileMode.Open);
                Res = (clsRecipe.clsThresholdInsp)XmlS.Deserialize(S);
                S.Close();

            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            return Res;
        }

        private void btnImportParam_Click(object sender, EventArgs e)
        {
            OpenFileDialog Open_Dialog = new OpenFileDialog();
            Open_Dialog.Filter = "files (*.xml)|*.xml";

            if (Open_Dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Param = clsStaticTool.Clone<clsRecipe.clsThresholdInsp>(ImportXML(Open_Dialog.FileName));

                    UpdateControl();

                    MessageBox.Show("匯入完成");
                }
                catch (Exception Ex)
                {
                    UpdateControl();
                    MessageBox.Show(Ex.ToString());
                }

            }
        }

        private void nudExtCircleSize_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;

            Param.ExtSizeCircle = double.Parse(Obj.Value.ToString());
        }

        private void cbxEdgeEditEnabled_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox Obj = (CheckBox)sender;
            Param.bEdgeEdit = Obj.Checked;
        }

        private void cbxInner_radiusEnabled_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox Obj = (CheckBox)sender;
            Param.bInnerCircleEnabled = Obj.Checked;
        }

        private void nudInner_radiusMin_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;
            Param.Inner_radiusMin = double.Parse(Obj.Value.ToString());
        }

        private void nudInner_radiusMax_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;
            Param.Inner_radiusMax = double.Parse(Obj.Value.ToString());
        }

        private void nudSecondImgIndex_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;
            AlgorithmParam.SecondSrcImgID = int.Parse(Obj.Value.ToString());
        }

        private void comboBoxSecondImgBand_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox Obj = (ComboBox)sender;

            if (Obj.SelectedIndex < 0)
                return;

            AlgorithmParam.SecondImgBand = (enuBand)Obj.SelectedIndex;

            HObject DisplayImage = clsStaticTool.GetChannelImage(ImageList[AlgorithmParam.SecondSrcImgID], AlgorithmParam.SecondImgBand);

            HOperatorSet.ClearWindow(Display.HalconWindow);
            HOperatorSet.DispObj(DisplayImage, Display.HalconWindow);

            DisplayImage.Dispose();
        }

        private void cbxSecondSrcEnabled_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox Obj = (CheckBox)sender;
            Param.bSecondSrcEnabled = Obj.Checked;
        }

        private void comboBoxSecondSrcGgrayType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox Obj = (ComboBox)sender;

            if (Obj.SelectedIndex < 0)
                return;

            Param.SecondSrcGraySelectType = (enuGraySelectType)Obj.SelectedIndex;
        }

        private void nudSecondSrcGrayMIn_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;
            Param.SecondSrcGrayMin = int.Parse(Obj.Value.ToString());
        }

        private void nudSecondSrcGrayMax_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;
            Param.SecondSrcGrayMax = int.Parse(Obj.Value.ToString());
        }

        /// <summary>
        /// 【測試】 (二值化)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnThresholdTest_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            // 檢查是否已讀取影像
            if (!Check_ImageList())
                return;

            HObject SrcImg = Get_SourceImage(); // (20200130) Jeff Revised!
            HOperatorSet.ClearWindow(Display.HalconWindow);
            HOperatorSet.DispObj(SrcImg, Display.HalconWindow); // (20200130) Jeff Revised!

            HObject InspRegion;
            HObject CellRegion;

            HTuple Row = null, Column = null, Angle = null, Score = null;
            //InductanceInsp.GetMatchInfo(SrcImg_Match, Recipe, out Row, out Column, out Angle, out Score);

            //if (Score.Length <= 0)
            //{
            //    MessageBox.Show("未搜尋到任何圖樣");
            //    return;
            //}
            try
            {
                HObject PatternRegions;
                InductanceInsp.PtternMatchAndSegRegion_New(Get_SourceImage_Match(), Recipe, out PatternRegions, out Angle, out Row, out Column, out CellRegion); // (20200130) Jeff Revised!
                //InductanceInsp.GetCellRegion(Row, Column, Angle, Score, Recipe, out CellRegion);
                InductanceInsp.GetUsedRegion(PatternRegions, Row, Column, Angle, Score, Recipe, AlgorithmParam.SelectRegionIndex, CellRegion, out InspRegion);



                HObject ThImage = clsStaticTool.GetChannelImage(SrcImg, AlgorithmParam.Band); // (20200130) Jeff Revised!

                HObject ThRegion;
                InductanceInsp.GetRegionAndThreshold(ThImage, InspRegion, Param, out ThRegion);

                if (DisplayRegion != null)
                {
                    DisplayRegion.Dispose();
                    HOperatorSet.GenEmptyObj(out DisplayRegion);
                }
                DisplayRegion = ThRegion;

                if (Param.ThType == InductanceInsp.enuThresholdType.AutoThreshold)
                {
                    HTuple Count;
                    HOperatorSet.CountObj(DisplayRegion, out Count);
                    comboBoxAutoRegion.Items.Clear();
                    for (int i = 0; i < Count; i++)
                    {
                        comboBoxAutoRegion.Items.Add(i.ToString());
                    }
                }
                HOperatorSet.SetDraw(Display.HalconWindow, "fill");
                HOperatorSet.SetColored(Display.HalconWindow, 12);
                HOperatorSet.DispObj(DisplayRegion, Display.HalconWindow);

                ThImage.Dispose();
                InspRegion.Dispose();
                CellRegion.Dispose();

            }
            catch (Exception E)
            {
                MessageBox.Show(E.ToString());
            }

        }

        /// <summary>
        /// 【篩選範圍】 (Second Source Image)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPreSecondSourceRegion_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            // 檢查是否已讀取影像
            if (!Check_ImageList())
                return;

            try
            {
                HObject SrcImg = Get_SourceImage(); // (20200130) Jeff Revised!
                HOperatorSet.ClearWindow(Display.HalconWindow);
                HOperatorSet.DispObj(SrcImg, Display.HalconWindow); // (20200130) Jeff Revised!

                HObject InspRegion;
                HObject CellRegion;

                HTuple Row = null, Column = null, Angle = null, Score = null;

                HObject PatternRegions;
                InductanceInsp.PtternMatchAndSegRegion_New(Get_SourceImage_Match(), Recipe, out PatternRegions, out Angle, out Row, out Column, out CellRegion); // (20200130) Jeff Revised!
                InductanceInsp.GetUsedRegion(PatternRegions, Row, Column, Angle, Score, Recipe, AlgorithmParam.SelectRegionIndex, CellRegion, out InspRegion);
                HObject OutputRegion, FinalRegion;
                InductanceInsp.ThresholdInsp(SrcImg, ImageList[AlgorithmParam.SecondSrcImgID], InspRegion, Param, AlgorithmParam, 1, out OutputRegion, out FinalRegion); // (20200130) Jeff Revised!


                HTuple Count;
                HOperatorSet.CountObj(FinalRegion, out Count);

                if (Count <= 0)
                    return;

                HOperatorSet.SetDraw(Display.HalconWindow, "fill");
                HOperatorSet.SetColored(Display.HalconWindow, 12);
                HOperatorSet.DispObj(FinalRegion, Display.HalconWindow);

                ChangeRegion(FinalRegion);
            }
            catch(Exception Ex)
            {
                
            }
        }

        private void nudanisometryMin_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;
            Param.anisometryMin = double.Parse(Obj.Value.ToString());
        }

        private void nudanisometryMax_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;
            Param.anisometryMax = double.Parse(Obj.Value.ToString());
        }

        private void cbxanisometryEnabled_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox Obj = (CheckBox)sender;

            Param.banisometryEnabled = Obj.Checked;
        }
    }
}
