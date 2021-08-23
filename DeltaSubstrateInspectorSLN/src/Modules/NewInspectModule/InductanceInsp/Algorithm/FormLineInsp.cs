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
    public partial class FormLineInsp : Form
    {
        clsRecipe.clsLineSearchInsp Param;
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

        public delegate void FormClosedHandler(bool FormClosed);//定義委派
        public event FormClosedHandler FormClosedEvent;  //定義事件
        
        public FormLineInsp(List<HObject> pmImageList, HSmartWindowControl pmDisplay, InductanceInspRole pmRecipe, clsRecipe.clsAlgorithm pmAlgorithmParam, List<HObject> resultImages = null, List<HObject> resultImages_UsedRegion = null) // (20200130) Jeff Revised!
        {
            InitializeComponent();
            this.ImageList = pmImageList;
            this.Display = pmDisplay;
            this.Recipe = pmRecipe;
            this.AlgorithmParam = pmAlgorithmParam;
            this.Param = (clsRecipe.clsLineSearchInsp)AlgorithmParam.Param;
            this.ResultImages = resultImages; // (20200130) Jeff Revised!
            this.ResultImages_UsedRegion = resultImages_UsedRegion; // (20200130) Jeff Revised!

            InsertCombobox2UI();
            //nudImageIndex.Maximum = ImageList.Count - 1; // (20200130) Jeff Revised!
            UpdateControl();

            clsLanguage.clsLanguage.SetLanguateToControls(this); // (20200130) Jeff Revised!
        }

        public void InsertCombobox2UI()
        {
            string[] ArrayEnum = Enum.GetNames(typeof(enuBand));
            comboBoxMultiTHBand.Items.Clear();
            foreach (string s in ArrayEnum)
            {
                comboBoxMultiTHBand.Items.Add(s);
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
            nudExtWidth.Value = decimal.Parse(Param.EdgeExtSizeWidth.ToString());
            nudExtHeight.Value = decimal.Parse(Param.EdgeExtSizeHeight.ToString());
         
            nudAreaMin.Value = decimal.Parse(Param.AreaMin.ToString());
            nudHeightMin.Value = decimal.Parse(Param.HeightMin.ToString());
            nudWidthMin.Value = decimal.Parse(Param.WidthMin.ToString());
            nudAreaMax.Value = decimal.Parse(Param.AreaMax.ToString());
            nudWidthMax.Value = decimal.Parse(Param.WidthMax.ToString());
            nudHeightMax.Value = decimal.Parse(Param.HeightMax.ToString());
            cbxInspAreaEnabled.Checked = Param.AEnabled;
            cbxInspWidthEnabled.Checked = Param.WEnabled;
            cbxInspHeightEnabled.Checked = Param.HEnabled;

         
            cbxAIInspection.Checked = AlgorithmParam.bUsedDAVS;

            #endregion

            #region Gauss Line Param

            nudLineGaussSigma.Value = decimal.Parse(Param.Sigma.ToString());
            nudLineGaussLow.Value = decimal.Parse(Param.Low.ToString());
            nudLineGaussHigh.Value = decimal.Parse(Param.High.ToString());
            comboBoxDarkLight.SelectedIndex = (int)Param.LightDark;
            nudMaxDistAbs.Value = decimal.Parse(Param.MaxDistAbs.ToString());
            nudMaxDistRel.Value = decimal.Parse(Param.MaxDistRel.ToString());
            nudMaxShift.Value = decimal.Parse(Param.MaxShift.ToString());
            nudMaxAngle.Value = decimal.Parse(Param.MaxAngle.ToString());

            nudSelectContlengthMin.Value = decimal.Parse(Param.LengthMin.ToString());
            nudSelectContlengthMax.Value = decimal.Parse(Param.LengthMax.ToString());

            #endregion
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

                InductanceInsp.GetUsedRegion(PatternRegions, Row, Column, Angle, Score, Recipe, AlgorithmParam.SelectRegionIndex, CellRegion, out InspRegion);

                HObject ErosionRegion, DilationRegion;
                HOperatorSet.GenEmptyObj(out ErosionRegion);
                HOperatorSet.GenEmptyObj(out DilationRegion);


                HOperatorSet.ErosionRectangle1(InspRegion, out ErosionRegion, Param.EdgeSkipSizeWidth, Param.EdgeSkipSizeHeight);

                HOperatorSet.DilationRectangle1(ErosionRegion, out DilationRegion, Param.EdgeExtSizeWidth, Param.EdgeExtSizeHeight);


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
        /// 取得來源影像 (圖像教導)
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

        private void FormThresholdInsp_FormClosed(object sender, FormClosedEventArgs e)
        {
            FormClosedEvent(true);
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

        private void nudExtWidth_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;
            Param.EdgeExtSizeWidth = int.Parse(Obj.Value.ToString());
        }

        private void nudExtHeight_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;
            Param.EdgeExtSizeHeight = int.Parse(Obj.Value.ToString());
        }

        private void nudLineGaussSigma_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;
            Param.Sigma = double.Parse(Obj.Value.ToString());
        }

        private void nudLineGaussLow_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;
            Param.Low = int.Parse(Obj.Value.ToString());
        }

        private void nudLineGaussHigh_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;
            Param.High = int.Parse(Obj.Value.ToString());
        }

        private void nudMaxDistAbs_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;
            Param.MaxDistAbs = int.Parse(Obj.Value.ToString());
        }

        private void nudMaxDistRel_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;
            Param.MaxDistRel = int.Parse(Obj.Value.ToString());
        }

        private void nudMaxShift_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;
            Param.MaxShift = int.Parse(Obj.Value.ToString());
        }

        private void nudMaxAngle_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;
            Param.MaxAngle = double.Parse(Obj.Value.ToString());
        }

        private void nudSelectContlengthMin_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;
            Param.LengthMin = int.Parse(Obj.Value.ToString());
        }

        private void nudSelectContlengthMax_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;
            Param.LengthMax = int.Parse(Obj.Value.ToString());
        }

        private void comboBoxDarkLight_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox Obj = (ComboBox)sender;
            if (Obj.SelectedIndex >= 0)
                Param.LightDark = (enuBinaryThSearchType)Obj.SelectedIndex;
        }

        /// <summary>
        /// 【尋線測試】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchLineTest_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            try
            {
                HOperatorSet.ClearWindow(Display.HalconWindow);
                HObject SrcImg = Get_SourceImage(); // (20200130) Jeff Revised!
                HOperatorSet.DispObj(SrcImg, Display.HalconWindow);
                HObject InspRegion;
                HObject CellRegion;

                HTuple Row = null, Column = null, Angle = null, Score = null;
                HObject PatternRegions;
                InductanceInsp.PtternMatchAndSegRegion_New(Get_SourceImage_Match(), Recipe, out PatternRegions, out Angle, out Row, out Column, out CellRegion); // (20200130) Jeff Revised!

                InductanceInsp.GetUsedRegion(PatternRegions, Row, Column, Angle, Score, Recipe, AlgorithmParam.SelectRegionIndex, CellRegion, out InspRegion);

                HObject LineXLD;
                if (!InductanceInsp.LineSearch(SrcImg, InspRegion, AlgorithmParam.Band, Param.EdgeSkipSizeWidth, Param.EdgeSkipSizeHeight, Param.EdgeExtSizeWidth, Param.EdgeExtSizeHeight,
                           Param.Sigma, Param.Low, Param.High, Param.LightDark.ToString(), Param.LineModel.ToString(), out LineXLD)) // (20200130) Jeff Revised!
                {
                    MessageBox.Show("Search Line Error...");
                }

                HOperatorSet.SetColored(Display.HalconWindow, 12);
                HOperatorSet.DispObj(LineXLD, Display.HalconWindow);
            }
            catch(Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        /// <summary>
        /// 【連接測試】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUnionTest_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            try
            {
                HOperatorSet.ClearWindow(Display.HalconWindow);
                HObject SrcImg = Get_SourceImage(); // (20200130) Jeff Revised!
                HOperatorSet.DispObj(SrcImg, Display.HalconWindow);
                HObject InspRegion;
                HObject CellRegion;

                HTuple Row = null, Column = null, Angle = null, Score = null;
                HObject PatternRegions;
                InductanceInsp.PtternMatchAndSegRegion_New(Get_SourceImage_Match(), Recipe, out PatternRegions, out Angle, out Row, out Column, out CellRegion); // (20200130) Jeff Revised!

                InductanceInsp.GetUsedRegion(PatternRegions, Row, Column, Angle, Score, Recipe, AlgorithmParam.SelectRegionIndex, CellRegion, out InspRegion);

                HObject LineXLD;
                if (!InductanceInsp.LineSearch(SrcImg, InspRegion, AlgorithmParam.Band, Param.EdgeSkipSizeWidth, Param.EdgeSkipSizeHeight, Param.EdgeExtSizeWidth, Param.EdgeExtSizeHeight,
                           Param.Sigma, Param.Low, Param.High, Param.LightDark.ToString(), Param.LineModel.ToString(), out LineXLD)) // (20200130) Jeff Revised!
                {
                    MessageBox.Show("Search Line Error...");
                }

                HObject UnionRegion;
                if (!InductanceInsp.UnionAndSelect(LineXLD, Param.MaxDistAbs, Param.MaxDistRel, Param.MaxShift, Param.MaxAngle, Param.LengthMin, Param.LengthMax, out UnionRegion))
                {
                    MessageBox.Show("Union Line Error...");
                }

                HObject Connection;
                HOperatorSet.Connection(UnionRegion, out Connection);

                HOperatorSet.SetColored(Display.HalconWindow, 12);
                HOperatorSet.DispObj(Connection, Display.HalconWindow);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }
    }
}
