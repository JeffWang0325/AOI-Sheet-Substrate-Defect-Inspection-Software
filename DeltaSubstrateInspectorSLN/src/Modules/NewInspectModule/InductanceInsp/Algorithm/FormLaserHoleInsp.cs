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
using System.IO;
using System.Xml.Serialization;

namespace DeltaSubstrateInspector.src.Modules.NewInspectModule.InductanceInsp
{
    public partial class FormLaserHoleInsp : Form
    {
        clsRecipe.clsLaserHoleInsp Param;
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

        public delegate void ChangeRegionHandler(HObject Obj, HTuple GrayMean, HTuple GrayMin, HTuple DarkRatio,HTuple Radius);//定義委派
        public event ChangeRegionHandler ChangeRegion;  //定義事件

        public delegate void FormClosedHandler(bool FormClosed);//定義委派
        public event FormClosedHandler FormClosedEvent;  //定義事件

        

        public FormLaserHoleInsp(List<HObject> pmImageList, HSmartWindowControl pmDisplay, InductanceInspRole pmRecipe, clsRecipe.clsAlgorithm pmAlgorithmParam, List<HObject> resultImages = null, List<HObject> resultImages_UsedRegion = null) // (20200130) Jeff Revised!
        {
            InitializeComponent();

            this.ImageList = pmImageList;
            this.Display = pmDisplay;
            this.Recipe = pmRecipe;
            this.AlgorithmParam = pmAlgorithmParam;
            this.Param = (clsRecipe.clsLaserHoleInsp)AlgorithmParam.Param;
            this.ResultImages = resultImages; // (20200130) Jeff Revised!
            this.ResultImages_UsedRegion = resultImages_UsedRegion; // (20200130) Jeff Revised!

            InsertBand2UI();
            UpdateControl();
            //nudImageIndex.Maximum = ImageList.Count - 1; // (20200130) Jeff Revised!
            nudMeanID.Maximum = ImageList.Count - 1;
            nudMinID.Maximum = ImageList.Count - 1;
            nudRatioID.Maximum = ImageList.Count - 1;

            clsLanguage.clsLanguage.SetLanguateToControls(this); // (20200130) Jeff Revised!
        }

        public void InsertBand2UI()
        {
            string[] ArrayEnum = Enum.GetNames(typeof(enuBand));
            comboBoxMultiTHBand.Items.Clear();
            comboBoxMeanBand.Items.Clear();
            comboBoxGrayMinBand.Items.Clear();
            comboBoxDarkRatioBand.Items.Clear();
            foreach (string s in ArrayEnum)
            {
                comboBoxMultiTHBand.Items.Add(s);
                comboBoxMeanBand.Items.Add(s);
                comboBoxGrayMinBand.Items.Add(s);
                comboBoxDarkRatioBand.Items.Add(s);
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
            nudEdgeExtWidth.Value = decimal.Parse(Param.EdgeExtSizeWidth.ToString());
            nudEdgeExtHeight.Value = decimal.Parse(Param.EdgeExtSizeHeight.ToString());
            nudAreaMin.Value = decimal.Parse(Param.LowArea.ToString());
            nudHeightMin.Value = decimal.Parse(Param.LowHeight.ToString());
            nudWidthMin.Value = decimal.Parse(Param.LowWidth.ToString());
            cbxInspAreaEnabled.Checked = Param.AEnabled;
            cbxInspWidthEnabled.Checked = Param.WEnabled;
            cbxInspHeightEnabled.Checked = Param.HEnabled;
            cbxAIInspection.Checked = AlgorithmParam.bUsedDAVS;

            #endregion

            #region 搜尋圓孔

            nudSearchCircleRadius.Value = decimal.Parse(Param.SearchCircleRadius.ToString());
            nudTolerance.Value = decimal.Parse(Param.SearchRadiusTolerance.ToString());
            nudMeasureLength2.Value = decimal.Parse(Param.measureLength2.ToString());
            nudSigma.Value = decimal.Parse(Param.MeasureSigma.ToString());
            nudMeasureThreshold.Value = decimal.Parse(Param.MeasureThreshold.ToString());
            nudNumInstances.Value = decimal.Parse(Param.num_instances.ToString());
            nudmin_score.Value = decimal.Parse(Param.min_score.ToString());
            comboBoxmeasure_transition.SelectedIndex = (int)Param.Trans;

            nudDilationSize.Value = decimal.Parse(Param.ErosionSize.ToString());
            cbxSearchFailPass.Checked = Param.bSearchFailPass;
            cbxSearchSizePass.Checked = Param.bSearchSizePass;

            nudSearchRadiusMin.Value = decimal.Parse(Param.SearchRadiusMin.ToString());
            nudSearchRadiusMax.Value = decimal.Parse(Param.SearchRadiusMax.ToString());

            #endregion

            #region 特徵條件

            cbxRadiusEnabled.Checked = Param.bRadius;
            cbxMeabEnabled.Checked = Param.bMeanEnabled;
            cbxGrayMinEnabled.Checked = Param.bGrayMin;
            cbxDarkRatioEnabled.Checked = Param.bDarkRatio;

            nudRadiusMin.Value = decimal.Parse(Param.RadiusMin.ToString());
            nudRadiusMax.Value = decimal.Parse(Param.RadiusMax.ToString());
            nudMeanMin.Value = decimal.Parse(Param.MeanMin.ToString());
            nudGayMin.Value = decimal.Parse(Param.GrayMin.ToString());
            nudDarkRatioMin.Value = decimal.Parse(Param.DarkRatioMin.ToString());

            comboBoxMeanBand.SelectedIndex = (int)Param.MeanBand;
            comboBoxGrayMinBand.SelectedIndex = (int)Param.GrayMinBand;
            comboBoxDarkRatioBand.SelectedIndex = (int)Param.DarkRatioBand;

            nudThMin.Value = decimal.Parse(Param.ThresholdMin.ToString());
            nudThMax.Value = decimal.Parse(Param.ThresholdMax.ToString());

            cbxDynTh.Checked = Param.bDynThreshold;
            cbxThresholdEnabled.Checked = Param.ThresholdEnabled;
            nudMeanWidth.Value = decimal.Parse(Param.MeanImgWidth.ToString());
            nudMeanHeight.Value = decimal.Parse(Param.MeanImgHeight.ToString());
            nudDynOffset.Value = decimal.Parse(Param.DynOffset.ToString());

            nudMeanID.Value = decimal.Parse(Param.MeanID.ToString());
            nudMinID.Value = decimal.Parse(Param.MinID.ToString());
            nudRatioID.Value = decimal.Parse(Param.RatioID.ToString());

            nudAcceptable_Value.Value = decimal.Parse(Param.Acceptable_Value.ToString());
            cbxAcceptable_Value.Checked = Param.bAcceptable_ValueEnabled;

            #endregion
        }


        public static bool ExportXML(clsRecipe.clsLaserHoleInsp SrcProduct, string PathFile)
        {
            bool Res = false;
            clsRecipe.clsLaserHoleInsp Product = clsStaticTool.Clone<clsRecipe.clsLaserHoleInsp>(SrcProduct);
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

        public static clsRecipe.clsLaserHoleInsp ImportXML(string PathFile)
        {
            clsRecipe.clsLaserHoleInsp Res = new clsRecipe.clsLaserHoleInsp();
            try
            {
                XmlSerializer XmlS = new XmlSerializer(Res.GetType());
                Stream S = File.Open(PathFile, FileMode.Open);
                Res = (clsRecipe.clsLaserHoleInsp)XmlS.Deserialize(S);
                S.Close();

            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            return Res;
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

        private void nudAreaMin_ValueChanged(object sender, EventArgs e)
        {
            Param.LowArea = int.Parse(nudAreaMin.Value.ToString());
        }

        private void nudWidthMin_ValueChanged(object sender, EventArgs e)
        {
            Param.LowWidth = int.Parse(nudWidthMin.Value.ToString());
        }

        private void nudHeightMin_ValueChanged(object sender, EventArgs e)
        {
            Param.LowHeight = int.Parse(nudHeightMin.Value.ToString());
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
                InductanceInsp.PtternMatchAndSegRegion_New(Get_SourceImage_Match(), Recipe, out PatternRegions, out Angle, out Row, out Column, out CellRegion);
                //InductanceInsp.GetMatchInfo(Get_SourceImage_Match(), Recipe, out Row, out Column, out Angle, out Score);
                //InductanceInsp.GetCellRegion(Row, Column, Angle, Score, Recipe, out CellRegion);

                InductanceInsp.GetUsedRegion(PatternRegions, Row, Column, Angle, Score, Recipe, AlgorithmParam.SelectRegionIndex, CellRegion, out InspRegion);

                HObject ErosionRegion;
                HOperatorSet.ErosionRectangle1(InspRegion, out ErosionRegion, Param.EdgeSkipSizeWidth, Param.EdgeSkipSizeHeight);

                HObject DilationRegion;
                HOperatorSet.DilationRectangle1(ErosionRegion, out DilationRegion, Param.EdgeExtSizeWidth, Param.EdgeExtSizeHeight);

                HOperatorSet.SetDraw(Display.HalconWindow, "margin");
                HOperatorSet.SetColor(Display.HalconWindow, "blue");
                HOperatorSet.DispObj(DilationRegion, Display.HalconWindow);

                ErosionRegion.Dispose();
                InspRegion.Dispose();
                CellRegion.Dispose();
                DilationRegion.Dispose();
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

        private void cbxAIInspection_CheckedChanged(object sender, EventArgs e)
        {
            AlgorithmParam.bUsedDAVS = cbxAIInspection.Checked;
        }

        private void nudEdgeExtWidth_ValueChanged(object sender, EventArgs e)
        {
            Param.EdgeExtSizeWidth = int.Parse(nudEdgeExtWidth.Value.ToString());
        }

        private void nudEdgeExtHeight_ValueChanged(object sender, EventArgs e)
        {
            Param.EdgeExtSizeHeight = int.Parse(nudEdgeExtHeight.Value.ToString());
        }

        private void nudSearchCircleRadius_ValueChanged(object sender, EventArgs e)
        {
            Param.SearchCircleRadius = int.Parse(nudSearchCircleRadius.Value.ToString());
        }

        private void nudTolerance_ValueChanged(object sender, EventArgs e)
        {
            Param.SearchRadiusTolerance = int.Parse(nudTolerance.Value.ToString());
        }

        private void nudMeasureLength2_ValueChanged(object sender, EventArgs e)
        {
            Param.measureLength2 = int.Parse(nudMeasureLength2.Value.ToString());
        }

        private void nudSigma_ValueChanged(object sender, EventArgs e)
        {
            Param.MeasureSigma = double.Parse(nudSigma.Value.ToString());
        }

        private void nudMeasureThreshold_ValueChanged(object sender, EventArgs e)
        {
            Param.MeasureThreshold = int.Parse(nudMeasureThreshold.Value.ToString());
        }

        private void comboBoxmeasure_transition_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxmeasure_transition.SelectedIndex < 0)
                return;

            Param.Trans = (enuSearchTransition)comboBoxmeasure_transition.SelectedIndex;
        }

        private void nudNumInstances_ValueChanged(object sender, EventArgs e)
        {
            Param.num_instances = int.Parse(nudNumInstances.Value.ToString());
        }

        private void nudmin_score_ValueChanged(object sender, EventArgs e)
        {
            Param.min_score = double.Parse(nudmin_score.Value.ToString());
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
        /// 【測試】 (tpCircleSearch)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSearchCircleTest_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            // 檢查是否已讀取影像
            if (!Check_ImageList())
                return;

            HObject CellRegion, InspRegion;
            try
            {
                HObject SrcImg = Get_SourceImage(); // (20200130) Jeff Revised!
                HOperatorSet.ClearWindow(Display.HalconWindow);
                HOperatorSet.DispObj(SrcImg, Display.HalconWindow);
                HTuple Row = null, Column = null, Angle = null, Score = null;
                HObject PatternRegions;
                InductanceInsp.PtternMatchAndSegRegion_New(Get_SourceImage_Match(), Recipe, out PatternRegions, out Angle, out Row, out Column, out CellRegion);
                //InductanceInsp.GetMatchInfo(Get_SourceImage_Match(), Recipe, out Row, out Column, out Angle, out Score);
                //InductanceInsp.GetCellRegion(Row, Column, Angle, Score, Recipe, out CellRegion);

                InductanceInsp.GetUsedRegion(PatternRegions, Row, Column, Angle, Score, Recipe, AlgorithmParam.SelectRegionIndex, CellRegion, out InspRegion);

                HObject CircleRegion, Cross,SearchRect;

                HTuple CircleRadius;

                InductanceInsp.LaserHoleSearch(SrcImg, InspRegion, Param, AlgorithmParam, out CircleRegion, out CircleRadius, out Cross,out SearchRect);

                if (cbxDislpayCross.Checked)
                {
                    HOperatorSet.SetDraw(Display.HalconWindow, "margin");
                    HOperatorSet.SetColored(Display.HalconWindow, 12);
                    HOperatorSet.DispObj(Cross, Display.HalconWindow);
                }
                if (cbxDisplayCircle.Checked)
                {
                    HOperatorSet.SetDraw(Display.HalconWindow, "margin");
                    HOperatorSet.SetColored(Display.HalconWindow, 12);
                    HOperatorSet.DispObj(CircleRegion, Display.HalconWindow);
                }
                if (cbxSearchRect.Checked)
                {
                    HOperatorSet.SetDraw(Display.HalconWindow, "margin");
                    HOperatorSet.SetColored(Display.HalconWindow, 12);
                    HOperatorSet.DispObj(SearchRect, Display.HalconWindow);
                }

                ChangeRegion(CircleRegion, 0, 0, 0, CircleRadius);
            }
            catch(Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        private void cbxRadiusEnabled_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox Obj = (CheckBox)sender;

            Param.bRadius = Obj.Checked;
        }

        private void cbxMeabEnabled_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox Obj = (CheckBox)sender;

            Param.bMeanEnabled = Obj.Checked;
        }

        private void cbxGrayMinEnabled_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox Obj = (CheckBox)sender;

            Param.bGrayMin = Obj.Checked;
        }

        private void cbxDarkRatioEnabled_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox Obj = (CheckBox)sender;

            Param.bDarkRatio = Obj.Checked;
        }

        private void nudMeanMin_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;

            Param.MeanMin = double.Parse(Obj.Value.ToString());
        }

        private void nudRadiusMin_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;

            Param.RadiusMin = double.Parse(Obj.Value.ToString());
        }

        private void nudRadiusMax_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;

            Param.RadiusMax = double.Parse(Obj.Value.ToString());
        }

        private void nudGayMin_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;

            Param.GrayMin = int.Parse(Obj.Value.ToString());
        }

        private void nudDarkRatioMin_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;

            Param.DarkRatioMin = double.Parse(Obj.Value.ToString());
        }

        private void comboBoxMeanBand_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox Obj = (ComboBox)sender;
            if (Obj.SelectedIndex < 0)
            {
                MessageBox.Show("請先選擇Band");
                return;
            }

            if (ImageList.Count <= Param.MeanID)
                return;

            Param.MeanBand = (enuBand)Obj.SelectedIndex;

            HObject DisplayImage = clsStaticTool.GetChannelImage(ImageList[Param.MeanID], Param.MeanBand);

            HOperatorSet.ClearWindow(Display.HalconWindow);
            HOperatorSet.DispObj(DisplayImage, Display.HalconWindow);

            DisplayImage.Dispose();
        }

        private void comboBoxGrayMinBand_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox Obj = (ComboBox)sender;
            if (Obj.SelectedIndex < 0)
            {
                MessageBox.Show("請先選擇Band");
                return;
            }

            if (ImageList.Count <= Param.MinID)
                return;

            Param.GrayMinBand = (enuBand)Obj.SelectedIndex;

            HObject DisplayImage = clsStaticTool.GetChannelImage(ImageList[Param.MinID], Param.GrayMinBand);

            HOperatorSet.ClearWindow(Display.HalconWindow);
            HOperatorSet.DispObj(DisplayImage, Display.HalconWindow);

            DisplayImage.Dispose();
        }

        private void comboBoxDarkRatioBand_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox Obj = (ComboBox)sender;
            if (Obj.SelectedIndex < 0)
            {
                MessageBox.Show("請先選擇Band");
                return;
            }

            if (ImageList.Count <= Param.RatioID)
                return;

            Param.DarkRatioBand = (enuBand)Obj.SelectedIndex;

            HObject DisplayImage = clsStaticTool.GetChannelImage(ImageList[Param.RatioID], Param.DarkRatioBand);

            HOperatorSet.ClearWindow(Display.HalconWindow);
            HOperatorSet.DispObj(DisplayImage, Display.HalconWindow);

            DisplayImage.Dispose();
        }

        private void nudThMin_ValueChanged(object sender, EventArgs e)
        {
            Param.ThresholdMin = int.Parse(nudThMin.Value.ToString());
        }

        private void nudThMax_ValueChanged(object sender, EventArgs e)
        {
            Param.ThresholdMax = int.Parse(nudThMax.Value.ToString());
        }

        private void btnThSetting_Click(object sender, EventArgs e)
        {
            // 檢查是否已讀取影像
            if (!Check_ImageList())
                return;

            if (AlgorithmParam.InspImageIndex >= ImageList.Count)
            {
                MessageBox.Show("影像錯誤");
                return;
            }

            HObject ThImage = clsStaticTool.GetChannelImage(ImageList[Param.RatioID], Param.DarkRatioBand);

            HObject CellRegion, InspRegion;

            HTuple Row = null, Column = null, Angle = null, Score = null;
            HObject PatternRegions;
            InductanceInsp.PtternMatchAndSegRegion_New(Get_SourceImage_Match(), Recipe, out PatternRegions, out Angle, out Row, out Column, out CellRegion);
            //InductanceInsp.GetMatchInfo(Get_SourceImage_Match(), Recipe, out Row, out Column, out Angle, out Score);
            //InductanceInsp.GetCellRegion(Row, Column, Angle, Score, Recipe, out CellRegion);

            InductanceInsp.GetUsedRegion(PatternRegions, Row, Column, Angle, Score, Recipe, AlgorithmParam.SelectRegionIndex, CellRegion, out InspRegion);

            HObject CircleRegion, Cross, SearchRect;

            HTuple CircleRadius;

            InductanceInsp.LaserHoleSearch(Get_SourceImage(), InspRegion, Param, AlgorithmParam, out CircleRegion, out CircleRadius, out Cross, out SearchRect);

            HObject Union;

            HOperatorSet.Union1(CircleRegion, out Union);

            HObject ReduceImg;
            HOperatorSet.ReduceDomain(ThImage, Union, out ReduceImg);


            FormThresholdAdjust MyForm = new FormThresholdAdjust(ReduceImg,
                                                                 int.Parse(nudThMin.Value.ToString()),
                                                                 int.Parse(nudThMax.Value.ToString()), FormThresholdAdjust.enType.Dual);
            MyForm.ShowDialog();

            if (MyForm.DialogResult == DialogResult.OK)
            {
                int ThMin, ThMax;
                MyForm.GetThreshold(out ThMin, out ThMax);

                nudThMin.Value = ThMin;
                nudThMax.Value = ThMax;
            }

            Union.Dispose();
            ThImage.Dispose();
            ReduceImg.Dispose();
            CircleRegion.Dispose();
            Cross.Dispose();
            SearchRect.Dispose();
            CellRegion.Dispose();
            InspRegion.Dispose();
        }

        private void cbxDynTh_CheckedChanged(object sender, EventArgs e)
        {
            Param.bDynThreshold = cbxDynTh.Checked;
        }

        private void nudMeanWidth_ValueChanged(object sender, EventArgs e)
        {
            Param.MeanImgWidth = int.Parse(nudMeanWidth.Value.ToString());
        }

        private void nudMeanHeight_ValueChanged(object sender, EventArgs e)
        {
            Param.MeanImgHeight = int.Parse(nudMeanHeight.Value.ToString());
        }

        private void nudDynOffset_ValueChanged(object sender, EventArgs e)
        {
            Param.DynOffset = int.Parse(nudDynOffset.Value.ToString());
        }

        private void btnDynTest_Click(object sender, EventArgs e)
        {
            // 檢查是否已讀取影像
            if (!Check_ImageList())
                return;

            if (AlgorithmParam.InspImageIndex >= ImageList.Count)
            {
                MessageBox.Show("影像錯誤");
                return;
            }

            HObject InspRegion, CellRegion;

            HObject SrcImg = Get_SourceImage(); // (20200130) Jeff Revised!
            HOperatorSet.ClearWindow(Display.HalconWindow);
            HOperatorSet.DispObj(SrcImg, Display.HalconWindow);
            HTuple Row = null, Column = null, Angle = null, Score = null;
            HObject PatternRegions;
            InductanceInsp.PtternMatchAndSegRegion_New(Get_SourceImage_Match(), Recipe, out PatternRegions, out Angle, out Row, out Column, out CellRegion);
            //InductanceInsp.GetMatchInfo(Get_SourceImage_Match(), Recipe, out Row, out Column, out Angle, out Score);

            //if (Score.Length <= 0)
            //{
            //    MessageBox.Show("未搜尋到任何對位記號");
            //    return;
            //}

            //InductanceInsp.GetCellRegion(Row, Column, Angle, Score, Recipe, out CellRegion);

            InductanceInsp.GetUsedRegion(PatternRegions, Row, Column, Angle, Score, Recipe, AlgorithmParam.SelectRegionIndex, CellRegion, out InspRegion);

            HObject CircleRegion, Cross, SearchRect;

            HTuple CircleRadius;

            InductanceInsp.LaserHoleSearch(SrcImg, InspRegion, Param, AlgorithmParam, out CircleRegion, out CircleRadius, out Cross, out SearchRect);

            HObject ReduceImage,InspUnion;
            HOperatorSet.Union1(CircleRegion, out InspUnion);

            HObject ThImage = clsStaticTool.GetChannelImage(ImageList[Param.RatioID], Param.DarkRatioBand);

            HOperatorSet.ReduceDomain(ThImage, InspUnion, out ReduceImage);

            HObject MeanImg;
            HOperatorSet.MeanImage(ReduceImage, out MeanImg, Param.MeanImgWidth, Param.MeanImgHeight);

            HObject DynThRegion;
            HOperatorSet.DynThreshold(ReduceImage, MeanImg, out DynThRegion, Param.DynOffset, "dark");

            HOperatorSet.SetDraw(Display.HalconWindow, "fill");
            HOperatorSet.SetColored(Display.HalconWindow, 12);
            HOperatorSet.DispObj(DynThRegion, Display.HalconWindow);

            ThImage.Dispose();
            MeanImg.Dispose();
            ReduceImage.Dispose();
            InspUnion.Dispose();
        }

        private void btnGetFeature_Click(object sender, EventArgs e)
        {
            // 檢查是否已讀取影像
            if (!Check_ImageList())
                return;

            if (!Param.Enabled)
            {
                MessageBox.Show("未啟用");
                return;
            }

            HObject CellRegion, InspRegion;
            try
            {
                HObject SrcImg = Get_SourceImage(); // (20200130) Jeff Revised!
                HOperatorSet.ClearWindow(Display.HalconWindow);
                HOperatorSet.DispObj(SrcImg, Display.HalconWindow);
                HTuple Row = null, Column = null, Angle = null, Score = null;
                HObject PatternRegions;
                InductanceInsp.PtternMatchAndSegRegion_New(Get_SourceImage_Match(), Recipe, out PatternRegions, out Angle, out Row, out Column, out CellRegion);
                //InductanceInsp.GetMatchInfo(Get_SourceImage_Match(), Recipe, out Row, out Column, out Angle, out Score);
                //InductanceInsp.GetCellRegion(Row, Column, Angle, Score, Recipe, out CellRegion);

                InductanceInsp.GetUsedRegion(PatternRegions, Row, Column, Angle, Score, Recipe, AlgorithmParam.SelectRegionIndex, CellRegion, out InspRegion);

                HObject CircleRegion, Cross, SearchRect;

                HTuple CircleRadius;

                InductanceInsp.LaserHoleSearch(SrcImg, InspRegion, Param, AlgorithmParam, out CircleRegion, out CircleRadius, out Cross, out SearchRect);

                HObject DefectRegion;
                HTuple Mean, Min, Ratio;
                InductanceInsp.HoleInsp(ImageList, InspRegion, CircleRegion, Param, CircleRadius, out DefectRegion, out Mean, out Min, out Ratio);

                HOperatorSet.SetDraw(Display.HalconWindow, "fill");
                HOperatorSet.SetColored(Display.HalconWindow, 12);
                HOperatorSet.DispObj(CircleRegion, Display.HalconWindow);

                ChangeRegion(CircleRegion, Mean, Min, Ratio, CircleRadius);

            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        private void btnThTest_Click(object sender, EventArgs e)
        {
            // 檢查是否已讀取影像
            if (!Check_ImageList())
                return;

            if (AlgorithmParam.InspImageIndex >= ImageList.Count)
            {
                MessageBox.Show("影像錯誤");
                return;
            }

            HObject InspRegion, CellRegion;

            HObject SrcImg = Get_SourceImage(); // (20200130) Jeff Revised!
            HOperatorSet.ClearWindow(Display.HalconWindow);
            HOperatorSet.DispObj(SrcImg, Display.HalconWindow);
            HTuple Row = null, Column = null, Angle = null, Score = null;
            HObject PatternRegions;
            InductanceInsp.PtternMatchAndSegRegion_New(Get_SourceImage_Match(), Recipe, out PatternRegions, out Angle, out Row, out Column, out CellRegion);
            ////InductanceInsp.GetMatchInfo(Get_SourceImage_Match(), Recipe, out Row, out Column, out Angle, out Score);

            ////if (Score.Length <= 0)
            ////{
            ////    MessageBox.Show("未搜尋到任何對位記號");
            ////    return;
            ////}

            ////InductanceInsp.GetCellRegion(Row, Column, Angle, Score, Recipe, out CellRegion);

            InductanceInsp.GetUsedRegion(PatternRegions, Row, Column, Angle, Score, Recipe, AlgorithmParam.SelectRegionIndex, CellRegion, out InspRegion);

            HObject CircleRegion, Cross, SearchRect;

            HTuple CircleRadius;

            InductanceInsp.LaserHoleSearch(SrcImg, InspRegion, Param, AlgorithmParam, out CircleRegion, out CircleRadius, out Cross, out SearchRect);

            HObject ReduceImage, InspUnion;
            HOperatorSet.Union1(CircleRegion, out InspUnion);

            HObject ThImage = clsStaticTool.GetChannelImage(ImageList[Param.RatioID], Param.DarkRatioBand);

            HOperatorSet.ReduceDomain(ThImage, InspUnion, out ReduceImage);

            HObject Thregion;
            HOperatorSet.Threshold(ReduceImage, out Thregion, Param.ThresholdMin, Param.ThresholdMax);

            HOperatorSet.SetDraw(Display.HalconWindow, "fill");
            HOperatorSet.SetColored(Display.HalconWindow, 12);
            HOperatorSet.DispObj(Thregion, Display.HalconWindow);

            ThImage.Dispose();
            ReduceImage.Dispose();
            InspUnion.Dispose();
        }

        private void btnMergeThRegion_Click(object sender, EventArgs e)
        {
            // 檢查是否已讀取影像
            if (!Check_ImageList())
                return;

            if (AlgorithmParam.InspImageIndex >= ImageList.Count)
            {
                MessageBox.Show("影像錯誤");
                return;
            }

            HObject InspRegion, CellRegion;

            HObject SrcImg = Get_SourceImage(); // (20200130) Jeff Revised!
            HOperatorSet.ClearWindow(Display.HalconWindow);
            HOperatorSet.DispObj(SrcImg, Display.HalconWindow);
            HTuple Row = null, Column = null, Angle = null, Score = null;

            HObject PatternRegions;
            InductanceInsp.PtternMatchAndSegRegion_New(Get_SourceImage_Match(), Recipe, out PatternRegions, out Angle, out Row, out Column, out CellRegion);
            //InductanceInsp.GetMatchInfo(Get_SourceImage_Match(), Recipe, out Row, out Column, out Angle, out Score);

            //if (Score.Length <= 0)
            //{
            //    MessageBox.Show("未搜尋到任何對位記號");
            //    return;
            //}

            //InductanceInsp.GetCellRegion(Row, Column, Angle, Score, Recipe, out CellRegion);

            InductanceInsp.GetUsedRegion(PatternRegions, Row, Column, Angle, Score, Recipe, AlgorithmParam.SelectRegionIndex, CellRegion, out InspRegion);

            HObject CircleRegion, Cross, SearchRect;

            HTuple CircleRadius;

            InductanceInsp.LaserHoleSearch(SrcImg, InspRegion, Param, AlgorithmParam, out CircleRegion, out CircleRadius, out Cross, out SearchRect);

            HObject ReduceImage, InspUnion;
            HOperatorSet.Union1(CircleRegion, out InspUnion);

            HObject ThImage = clsStaticTool.GetChannelImage(ImageList[Param.RatioID], Param.DarkRatioBand);

            HOperatorSet.ReduceDomain(ThImage, InspUnion, out ReduceImage);

            HObject MeanImg;
            HOperatorSet.MeanImage(ReduceImage, out MeanImg, Param.MeanImgWidth, Param.MeanImgHeight);

            HObject DynThRegion;
            HOperatorSet.DynThreshold(ReduceImage, MeanImg, out DynThRegion, Param.DynOffset, "dark");

            HObject ThRegion;
            HOperatorSet.Threshold(ReduceImage, out ThRegion, Param.ThresholdMin, Param.ThresholdMax);

            HObject MergeRegion;
            HOperatorSet.Union2(ThRegion, DynThRegion, out MergeRegion);

            HOperatorSet.SetDraw(Display.HalconWindow, "fill");
            HOperatorSet.SetColored(Display.HalconWindow, 12);
            HOperatorSet.DispObj(MergeRegion, Display.HalconWindow);

            ThImage.Dispose();
            MeanImg.Dispose();
            ReduceImage.Dispose();
            InspUnion.Dispose();
        }

        private void nudDilationSize_ValueChanged(object sender, EventArgs e)
        {
            Param.ErosionSize = double.Parse(nudDilationSize.Value.ToString());
        }

        private void FormLaserHoleInsp_FormClosed(object sender, FormClosedEventArgs e)
        {
            FormClosedEvent(true);
        }

        private void cbxThresholdEnabled_CheckedChanged(object sender, EventArgs e)
        {
            Param.ThresholdEnabled = cbxThresholdEnabled.Checked;
        }

        private void cbxSearchFailPass_CheckedChanged(object sender, EventArgs e)
        {
            Param.bSearchFailPass = cbxSearchFailPass.Checked;
        }

        private void cbxSearchSizePass_CheckedChanged(object sender, EventArgs e)
        {
            Param.bSearchSizePass = cbxSearchSizePass.Checked;
        }

        private void nudSearchRadiusMin_ValueChanged(object sender, EventArgs e)
        {
            Param.SearchRadiusMin = double.Parse(nudSearchRadiusMin.Value.ToString());
        }

        private void nudSearchRadiusMax_ValueChanged(object sender, EventArgs e)
        {
            Param.SearchRadiusMax = double.Parse(nudSearchRadiusMax.Value.ToString());
        }

        private void nudMeanID_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;
            Param.MeanID = int.Parse(Obj.Value.ToString());
        }

        private void nudMinID_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;
            Param.MinID = int.Parse(Obj.Value.ToString());
        }

        private void nudRatioID_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;
            Param.RatioID = int.Parse(Obj.Value.ToString());
        }

        private void nudAcceptable_Value_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;
            Param.Acceptable_Value = int.Parse(Obj.Value.ToString());
        }

        private void cbxAcceptable_Value_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox Obj = (CheckBox)sender;
            Param.bAcceptable_ValueEnabled = Obj.Checked;
        }

        private void btnExportParam_Click(object sender, EventArgs e)
        {
            SaveFileDialog Save_Dialog = new SaveFileDialog();
            Save_Dialog.Filter = "files (*.xml)|*.xml";
            if (Save_Dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    clsRecipe.clsLaserHoleInsp ExportParam = Param;

                    if (ExportXML(ExportParam, Save_Dialog.FileName))
                    {
                        MessageBox.Show("匯出成功");
                    }
                    else
                    {
                        MessageBox.Show("Fail...");
                    }
                }
                catch(Exception Ex)
                {
                    MessageBox.Show(Ex.ToString());
                }

                
            }
        }

        private void btnImportParam_Click(object sender, EventArgs e)
        {
            OpenFileDialog Open_Dialog = new OpenFileDialog();
            Open_Dialog.Filter = "files (*.xml)|*.xml";

            if (Open_Dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Param = clsStaticTool.Clone<clsRecipe.clsLaserHoleInsp>(ImportXML(Open_Dialog.FileName));

                    UpdateControl();

                    MessageBox.Show("匯入完成");
                }
                catch(Exception Ex)
                {
                    UpdateControl();
                    MessageBox.Show(Ex.ToString());
                }

            }

        }
    }
}
