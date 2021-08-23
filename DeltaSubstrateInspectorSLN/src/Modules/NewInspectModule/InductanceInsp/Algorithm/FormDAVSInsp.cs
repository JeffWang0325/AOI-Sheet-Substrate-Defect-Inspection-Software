using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DeltaSubstrateInspector.FileSystem.FileSystem;
using HalconDotNet;
using System.Threading;
using System.IO;

namespace DeltaSubstrateInspector.src.Modules.NewInspectModule.InductanceInsp
{
    public partial class FormDAVSInsp : Form
    {
        clsRecipe.clsDAVSInsp Param;
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

        InductanceInspRole Recipe;
        clsRecipe.clsAlgorithm AlgorithmParam;
        double Resoluton = 0;
        clsDAVSInspMethod DAVSMethod;
        
        public delegate void FormClosedHandler(bool FormClosed);//定義委派
        public event FormClosedHandler FormClosedEvent;  //定義事件


        public FormDAVSInsp(List<HObject> pmImageList, InductanceInspRole pmRecipe, clsRecipe.clsAlgorithm pmAlgorithmParam, double pmResoluton,clsDAVSInspMethod pmDAVSMethod, List<HObject> resultImages = null, List<HObject> resultImages_UsedRegion = null) // (20200130) Jeff Revised!
        {
            InitializeComponent();

            this.ImageList = pmImageList;
            this.Recipe = pmRecipe;
            this.AlgorithmParam = pmAlgorithmParam;
            this.Param = (clsRecipe.clsDAVSInsp)AlgorithmParam.Param;
            this.Resoluton = pmResoluton;
            this.DAVSMethod = pmDAVSMethod;
            this.ResultImages = resultImages; // (20200130) Jeff Revised!
            this.ResultImages_UsedRegion = resultImages_UsedRegion; // (20200130) Jeff Revised!

            InsertBand2UI(); // (20200130) Jeff Revised!
            UpdateControl();
            this.HWindowDisplay.MouseWheel += HWindowDisplay.HSmartWindowControl_MouseWheel;
            //nudImageIndex.Maximum = ImageList.Count - 1; // (20200130) Jeff Revised!

            DAVSMethod.SetPathParam(ModuleName, SB_ID, MOVE_ID, PartNumber);
            HOperatorSet.DispObj(Get_SourceImage(), HWindowDisplay.HalconWindow);

            clsLanguage.clsLanguage.SetLanguateToControls(this); // (20200130) Jeff Revised!
        }

        public void InsertBand2UI() // (20200130) Jeff Revised!
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
            nudEdgeExtWidth.Value = decimal.Parse(Param.EdgeExtSizeWidth.ToString());
            nudEdgeExtHeight.Value = decimal.Parse(Param.EdgeExtSizeHeight.ToString());
            comboBoxInspType.SelectedIndex = (int)Param.DAVS_InspMode;
            comboBoxPredictionMode.SelectedIndex = (int)Param.DAVS_PredictMode;

            txbImgHSDirPath.Text = Param.DAVS_ImgHSDir;
            txbModelPath.Text = Param.ModelPath;

            nudFixSizeWidth.Value = decimal.Parse(Param.FixSizeWidth.ToString());
            nudFixSizeHeight.Value = decimal.Parse(Param.FixSizeHeight.ToString());
            cbxbFixSize.Checked = Param.bFixSize;
            txbSaveImagePath.Text = Param.SaveImagePath;

            if (DAVSMethod.AIClass.ClassNUM != 0)
                labNum.Text = DAVSMethod.AIClass.ClassNUM.ToString();
            else
                labNum.Text = "無AI參數";

            for (int i = 0; i < Param.DAVS_SaveImgEnabledList.Count; i++)
            {
                cbxListSaveImg.Items.Add(DAVSMethod.AIClass.ResultRegionsList[i].Name);
                cbxListSaveImg.SetItemChecked(i, Param.DAVS_SaveImgEnabledList[i]);
            }

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

        private void btnInspRegionTest_Click(object sender, EventArgs e)
        {
            // 檢查是否已讀取影像
            if (!Check_ImageList())
                return;

            HObject InspRegion;
            HObject CellRegion;
            HObject PatternRegions;
            try
            {
                HOperatorSet.ClearWindow(HWindowDisplay.HalconWindow);
                HOperatorSet.DispObj(Get_SourceImage(), HWindowDisplay.HalconWindow);
                HTuple Row = null, Column = null, Angle = null, Score = null;
                InductanceInsp.PtternMatchAndSegRegion_New(Get_SourceImage_Match(), Recipe, out PatternRegions, out Angle, out Row, out Column, out CellRegion);
                //InductanceInsp.GetMatchInfo(Get_SourceImage_Match(), Recipe, out Row, out Column, out Angle, out Score);
                //InductanceInsp.GetCellRegion(Row, Column, Angle, Score, Recipe, out CellRegion);

                InductanceInsp.GetUsedRegion(PatternRegions, Row, Column, Angle, Score, Recipe, AlgorithmParam.SelectRegionIndex,CellRegion, out InspRegion);

                HObject ErosionRegion;
                HOperatorSet.ErosionRectangle1(InspRegion, out ErosionRegion, Param.EdgeSkipSizeWidth, Param.EdgeSkipSizeHeight);

                HObject DilationRegion;
                HOperatorSet.DilationRectangle1(ErosionRegion, out DilationRegion, Param.EdgeExtSizeWidth, Param.EdgeExtSizeHeight);

                HOperatorSet.SetDraw(HWindowDisplay.HalconWindow, "margin");
                HOperatorSet.SetColor(HWindowDisplay.HalconWindow, "blue");
                HOperatorSet.DispObj(DilationRegion, HWindowDisplay.HalconWindow);

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
            HOperatorSet.ClearWindow(HWindowDisplay.HalconWindow);
            HOperatorSet.DispObj(DisplayImage, HWindowDisplay.HalconWindow);
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

        private void nudEdgeExtWidth_ValueChanged(object sender, EventArgs e)
        {
            Param.EdgeExtSizeWidth = int.Parse(nudEdgeExtWidth.Value.ToString());
        }

        private void nudEdgeExtHeight_ValueChanged(object sender, EventArgs e)
        {
            Param.EdgeExtSizeHeight = int.Parse(nudEdgeExtHeight.Value.ToString());
        }
        
        private void FormTextureInsp_FormClosed(object sender, FormClosedEventArgs e)
        {
            FormClosedEvent(true);
        }

        private void comboBoxInspType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox Obj = (ComboBox)sender;
            if (Obj.SelectedIndex < 0)
                return;

            Param.DAVS_InspMode = (enuDAVSMode)Obj.SelectedIndex;
        }

        private void comboBoxPredictionMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox Obj = (ComboBox)sender;
            if (Obj.SelectedIndex < 0)
                return;

            Param.DAVS_PredictMode = (enuPredictionMode)Obj.SelectedIndex;
        }

        private void txbImgHSDirPath_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                FolderBrowserDialog DFileDialog = new FolderBrowserDialog();

                DialogResult result = DFileDialog.ShowDialog();
                
                if (result != DialogResult.OK)
                    return;

                if (!string.IsNullOrEmpty(DFileDialog.SelectedPath))
                {
                    string SvaePath = DFileDialog.SelectedPath + "\\";

                    txbImgHSDirPath.Text = SvaePath;

                    Param.DAVS_ImgHSDir = txbImgHSDirPath.Text;

                    MessageBox.Show("設定完成");
                }
                else
                {
                    MessageBox.Show("請先選擇路徑");
                    return;
                }

            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        private void txbModelPath_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                FolderBrowserDialog DFileDialog = new FolderBrowserDialog();

                DialogResult result = DFileDialog.ShowDialog();

                if (result != DialogResult.OK)
                    return;

                if (!string.IsNullOrEmpty(DFileDialog.SelectedPath))
                {
                    string SvaePath = DFileDialog.SelectedPath + "\\";

                    txbModelPath.Text = SvaePath;

                    Param.ModelPath = txbModelPath.Text;

                    MessageBox.Show("設定完成");
                }
                else
                {
                    MessageBox.Show("請先選擇路徑");
                    return;
                }

            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        /// <summary>
        /// 【切割】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSegTest_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
        {
            // 檢查是否已讀取影像
            if (!Check_ImageList())
                return;

            try
            {
                HObject CellRegion;
                HObject SegRegion;
                HObject SrcImg = Get_SourceImage(); // (20200130) Jeff Revised!
                HOperatorSet.ClearWindow(HWindowDisplay.HalconWindow);
                HOperatorSet.DispObj(SrcImg, HWindowDisplay.HalconWindow);

                HTuple Row = null, Column = null, Angle = null, Score = null;
                //InductanceInsp.GetMatchInfo(Get_SourceImage_Match(), Recipe, out Row, out Column, out Angle, out Score);
                //InductanceInsp.GetCellRegion(Row, Column, Angle, Score, Recipe, out CellRegion);
                HObject PatternRegions;
                InductanceInsp.PtternMatchAndSegRegion_New(Get_SourceImage_Match(), Recipe, out PatternRegions, out Angle, out Row, out Column, out CellRegion);
                InductanceInsp.GetUsedRegion(PatternRegions, Row, Column, Angle, Score, Recipe, AlgorithmParam.SelectRegionIndex, CellRegion, out SegRegion);

                HOperatorSet.SortRegion(SegRegion, out SegRegion, "character", "true", "row");
                
                DAVSMethod.InspRegions = SegRegion.Clone();
                DAVSMethod.SetParam(Param);
                if (!DAVSMethod.segment_and_save_single_chip_images(SrcImg, true))
                {
                    MessageBox.Show("請確認是否已載入影像/模板或執行模板比對!", "Sometihg Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    HObject Trans;
                    if (!Param.bFixSize)
                        HOperatorSet.ShapeTrans(SegRegion, out Trans, enuTransType.rectangle1.ToString());
                    else
                    {
                        HTuple A, R, C;
                        HOperatorSet.AreaCenter(SegRegion, out A, out R, out C);
                        HOperatorSet.GenRectangle1(out Trans, R - Param.FixSizeHeight / 2, C - Param.FixSizeWidth / 2, R + Param.FixSizeHeight / 2, C + Param.FixSizeWidth / 2);
                    }

                    HObject Erosion, Dilation; // (20200130) Jeff Revised!
                    HOperatorSet.ErosionRectangle1(Trans, out Erosion, Param.EdgeSkipSizeWidth, Param.EdgeSkipSizeHeight);
                    HOperatorSet.DilationRectangle1(Erosion, out Dilation, Param.EdgeExtSizeWidth, Param.EdgeExtSizeHeight);
                    
                    HOperatorSet.SetDraw(HWindowDisplay.HalconWindow, "margin");
                    HOperatorSet.SetColor(HWindowDisplay.HalconWindow, "blue");
                    HOperatorSet.DispObj(SrcImg, HWindowDisplay.HalconWindow);
                    HOperatorSet.DispObj(Erosion, HWindowDisplay.HalconWindow);
                    Trans.Dispose();
                    Dilation.Dispose();
                    Erosion.Dispose();
                }
                
                SegRegion.Dispose();
                CellRegion.Dispose();
            }
            catch (Exception ex)
            {
                string message = "Error";
                MessageBox.Show(message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                MessageBox.Show(ex.ToString(), "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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
        /// 【測試】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInspTest_Click(object sender, EventArgs e)
        {
            if(DAVSMethod.InspRegions == null || DAVSMethod.SingleChipImgList == null)
            {
                MessageBox.Show("請先執行切割!", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                HObject SrcImg = Get_SourceImage(); // (20200130) Jeff Revised!
                if (!DAVSMethod.DAVS_execute(Param, SrcImg))
                {
                    MessageBox.Show("Someting error!", "Sometihg Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    HOperatorSet.ClearWindow(HWindowDisplay.HalconWindow);
                    HOperatorSet.DispObj(SrcImg, HWindowDisplay.HalconWindow);

                    DAVSMethod.UpdatePredictionResult(HWindowDisplay, SrcImg);
                }
            }
            catch (Exception ex)
            {
                DAVSMethod.Dispose();
                MessageBox.Show(ex.ToString(), "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void cbxListSaveImg_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (cbxListSaveImg.SelectedIndex < 0)
                return;

            if (cbxListSaveImg.GetItemChecked(cbxListSaveImg.SelectedIndex))
                Param.DAVS_SaveImgEnabledList[cbxListSaveImg.SelectedIndex] = true;
            else
                Param.DAVS_SaveImgEnabledList[cbxListSaveImg.SelectedIndex] = false;

            DAVSMethod.SetParam(Param);
        }

        private void cbxListSaveImg_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cbxListSaveImg.SelectedIndex < 0)
                return;

            if (cbxListSaveImg.GetItemChecked(cbxListSaveImg.SelectedIndex))
                Param.DAVS_SaveImgEnabledList[cbxListSaveImg.SelectedIndex] = true;
            else
                Param.DAVS_SaveImgEnabledList[cbxListSaveImg.SelectedIndex] = false;

            DAVSMethod.SetParam(Param);
        }

        private void cbxbFixSize_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox Obj = (CheckBox)sender;
            Param.bFixSize = Obj.Checked;
        }

        private void nudFixSizeWidth_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;

            Param.FixSizeWidth = int.Parse(Obj.Value.ToString());
        }

        private void nudFixSizeHeight_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;

            Param.FixSizeHeight = int.Parse(Obj.Value.ToString());
        }

        private void txbSaveImagePath_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                FolderBrowserDialog DFileDialog = new FolderBrowserDialog();

                DialogResult result = DFileDialog.ShowDialog();

                if (result != DialogResult.OK)
                    return;

                if (!string.IsNullOrEmpty(DFileDialog.SelectedPath))
                {
                    string SvaePath = DFileDialog.SelectedPath + "\\";

                    txbSaveImagePath.Text = SvaePath;

                    Param.SaveImagePath = txbSaveImagePath.Text;

                    MessageBox.Show("設定完成");
                }
                else
                {
                    MessageBox.Show("請先選擇路徑");
                    return;
                }

            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }
    }
}
