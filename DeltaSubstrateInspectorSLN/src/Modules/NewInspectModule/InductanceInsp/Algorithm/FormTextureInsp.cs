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

namespace DeltaSubstrateInspector.src.Modules.NewInspectModule.InductanceInsp
{
    public partial class FormTextureInsp : Form
    {
        clsRecipe.clsTextureInsp Param;
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

        HSmartWindowControl Display { get; set; }
        InductanceInspRole Recipe;
        clsRecipe.clsAlgorithm AlgorithmParam;
        double Resoluton = 0;

        HTuple TextureModel = null;


        public delegate void ChangeRegionHandler(HObject Obj, HTuple GrayMean, HTuple GrayMin, HTuple DarkRatio,HTuple Radius);//定義委派
        public event ChangeRegionHandler ChangeRegion;  //定義事件

        public delegate void FormClosedHandler(bool FormClosed);//定義委派
        public event FormClosedHandler FormClosedEvent;  //定義事件


        public FormTextureInsp(List<HObject> pmImageList, HSmartWindowControl pmDisplay, InductanceInspRole pmRecipe, clsRecipe.clsAlgorithm pmAlgorithmParam,double pmResoluton, List<HObject> resultImages = null, List<HObject> resultImages_UsedRegion = null) // (20200130) Jeff Revised!
        {
            InitializeComponent();

            this.ImageList = pmImageList;
            this.Display = pmDisplay;
            this.Recipe = pmRecipe;
            this.AlgorithmParam = pmAlgorithmParam;
            this.Param = (clsRecipe.clsTextureInsp)AlgorithmParam.Param;
            this.Resoluton = pmResoluton;
            this.ResultImages = resultImages; // (20200130) Jeff Revised!
            this.ResultImages_UsedRegion = resultImages_UsedRegion; // (20200130) Jeff Revised!

            InsertBand2UI(); // (20200130) Jeff Revised!
            InsertCombobox2UI(); // (20200319) Jeff Revised!
            UpdateControl();
            InitLoadModel();
            this.Width = 380;
            this.HWindowDisplay.MouseWheel += HWindowDisplay.HSmartWindowControl_MouseWheel;
            //nudImageIndex.Maximum = ImageList.Count - 1; // (20200130) Jeff Revised!

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

        public void InsertCombobox2UI() // (20200319) Jeff Revised!
        {
            string[] ArrayEnum = Enum.GetNames(typeof(enu_patch_normalization));
            comboBox_patch_normalization.Items.Clear();
            foreach (string s in ArrayEnum)
            {
                comboBox_patch_normalization.Items.Add(s);
            }
        }

        public void InitLoadModel()
        {
            try
            {
                if (!string.IsNullOrEmpty(Param.ModelPath))
                {
                    if (!File.Exists(Param.ModelPath + "TextureModel.htim"))
                    {
                        MessageBox.Show("Model檔案不存在,請建立新Model");
                        return;
                    }
                    HOperatorSet.ReadTextureInspectionModel(Param.ModelPath + "TextureModel.htim", out TextureModel);
                }
            }
            catch(Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        /// <summary>
        /// 更新各控制項參數
        /// </summary>
        public void UpdateControl() // (20200319) Jeff Revised!
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

            // 檢測ROI (20200319) Jeff Revised!
            cbx_ROI_Enabled.Checked = Param.ROI_Enabled;
            nud_ROI_SkipWidth.Value = decimal.Parse(Param.ROI_SkipWidth.ToString());
            nud_ROI_SkipHeight.Value = decimal.Parse(Param.ROI_SkipHeight.ToString());

            nudAreaMin.Value = decimal.Parse(Param.LowArea.ToString());
            nudHeightMin.Value = decimal.Parse(Param.LowHeight.ToString());
            nudWidthMin.Value = decimal.Parse(Param.LowWidth.ToString());
            cbxInspAreaEnabled.Checked = Param.AEnabled;
            cbxInspWidthEnabled.Checked = Param.WEnabled;
            cbxInspHeightEnabled.Checked = Param.HEnabled;
            cbxAIInspection.Checked = AlgorithmParam.bUsedDAVS;

            cbxanisometryEnabled.Checked = Param.anisometryEnabled;
            nudanisometry.Value = decimal.Parse(Param.anisometryMin.ToString());

            #endregion

            #region 訓練參數

            txbModelPath.Text = Param.ModelPath;
            nudSensitivity.Value = decimal.Parse(Param.Sensitivity.ToString());
            nudLevelStart.Value = decimal.Parse(Param.LevelStart.ToString());
            nudLevelEnd.Value = decimal.Parse(Param.LevelEnd.ToString());
            nudIterations.Value = decimal.Parse(Param.max_Iter.ToString());
            nudGetResultLevel.Value=decimal.Parse(Param.LevelStart.ToString());

            comboBox_patch_normalization.SelectedIndexChanged -= comboBox_patch_normalization_SelectedIndexChanged; // (20200319) Jeff Revised!
            comboBox_patch_normalization.SelectedIndex = (int)Param.patch_normalization;
            comboBox_patch_normalization.SelectedIndexChanged += comboBox_patch_normalization_SelectedIndexChanged;

            cbx_patch_rotational_robustness.Checked = Param.patch_rotational_robustness; // (20200319) Jeff Revised!

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

        /// <summary>
        /// 【檢測ROI】之【啟用】切換
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbx_ROI_Enabled_CheckedChanged(object sender, EventArgs e) // (20200319) Jeff Revised!
        {
            // 參數更新
            Param.ROI_Enabled = cbx_ROI_Enabled.Checked;

            // Enabled狀態切換
            nud_ROI_SkipWidth.Enabled = cbx_ROI_Enabled.Checked;
            nud_ROI_SkipHeight.Enabled = cbx_ROI_Enabled.Checked;
            btn_Insp_ROI.Enabled = cbx_ROI_Enabled.Checked;
        }

        private void nud_ROI_SkipWidth_ValueChanged(object sender, EventArgs e) // (20200319) Jeff Revised!
        {
            Param.ROI_SkipWidth = int.Parse(nud_ROI_SkipWidth.Value.ToString());
        }

        private void nud_ROI_SkipHeight_ValueChanged(object sender, EventArgs e) // (20200319) Jeff Revised!
        {
            Param.ROI_SkipHeight = int.Parse(nud_ROI_SkipHeight.Value.ToString());
        }

        /// <summary>
        /// 【檢測ROI】之【顯示範圍】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Insp_ROI_Click(object sender, EventArgs e) // (20200319) Jeff Revised!
        {
            // 檢查是否已讀取影像
            if (!Check_ImageList())
                return;

            try
            {
                HOperatorSet.ClearWindow(Display.HalconWindow);
                HOperatorSet.DispObj(Get_SourceImage(), Display.HalconWindow);
                HObject InspRegion;
                HObject CellRegion;

                HTuple Row = null, Column = null, Angle = null, Score = null;
                HObject PatternRegions;
                InductanceInsp.PtternMatchAndSegRegion_New(Get_SourceImage_Match(), Recipe, out PatternRegions, out Angle, out Row, out Column, out CellRegion);
                
                InductanceInsp.GetUsedRegion(PatternRegions, Row, Column, Angle, Score, Recipe, AlgorithmParam.SelectRegionIndex, CellRegion, out InspRegion);

                HObject ErosionRegion;
                HOperatorSet.ErosionRectangle1(InspRegion, out ErosionRegion, Param.EdgeSkipSizeWidth, Param.EdgeSkipSizeHeight);

                HObject DilationRegion;
                HOperatorSet.DilationRectangle1(ErosionRegion, out DilationRegion, Param.EdgeExtSizeWidth, Param.EdgeExtSizeHeight);
                
                HObject Union;
                HOperatorSet.Union1(DilationRegion, out Union);

                // 檢測ROI (20200319) Jeff Revised!
                HObject roi;
                HOperatorSet.ErosionRectangle1(Union, out roi, Param.ROI_SkipWidth, Param.ROI_SkipHeight);

                HOperatorSet.SetDraw(Display.HalconWindow, "margin");
                HOperatorSet.SetColor(Display.HalconWindow, "blue");
                HOperatorSet.DispObj(roi, Display.HalconWindow);

                InspRegion.Dispose();
                CellRegion.Dispose();
                PatternRegions.Dispose();
                ErosionRegion.Dispose();
                DilationRegion.Dispose();
                Union.Dispose();
                roi.Dispose();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
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
                HOperatorSet.DispObj(Get_SourceImage(), Display.HalconWindow);
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

            HObject DisplayImage = clsStaticTool.GetChannelImage(Get_SourceImage(), AlgorithmParam.Band);
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

        private void txbModelPath_Click(object sender, EventArgs e) // (20200319) Jeff Revised!
        {
            try
            {
                FolderBrowserDialog FileDialog = new FolderBrowserDialog();
                FileDialog.SelectedPath = txbModelPath.Text; // (20200319) Jeff Revised!
                DialogResult result = FileDialog.ShowDialog();

                if (result != DialogResult.OK)
                    return;

                if (!string.IsNullOrEmpty(FileDialog.SelectedPath))
                {
                    string SvaePath = FileDialog.SelectedPath + "\\";

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
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
                if (TextureModel != null)
                {
                    if (TextureModel.Length > 0)
                    {
                        HOperatorSet.ClearTextureInspectionModel(TextureModel);
                        TextureModel = null;
                    }
                }
                HOperatorSet.CreateTextureInspectionModel("basic", out TextureModel);
                btnSaveModel.Enabled = true;
            }
            catch(Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        private void nudSensitivity_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;

            Param.Sensitivity = int.Parse(Obj.Value.ToString());
        }

        private void nudLevelStart_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;

            int End = int.Parse(nudLevelEnd.Value.ToString());
            int Start = int.Parse(nudLevelStart.Value.ToString());

            if (Start > End)
            {
                Obj.Value = decimal.Parse((End - 1).ToString());
                return;
            }

            Param.LevelStart = int.Parse(Obj.Value.ToString());
        }

        private void nudLevelEnd_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;

            int End = int.Parse(nudLevelEnd.Value.ToString());
            int Start = int.Parse(nudLevelStart.Value.ToString());

            if (Start > End)
            {
                Obj.Value = decimal.Parse((Start + 1).ToString());
                return;
            }

            Param.LevelEnd = int.Parse(Obj.Value.ToString());
        }

        private void nudIterations_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;

            Param.max_Iter = int.Parse(Obj.Value.ToString());
        }

        /// <summary>
        /// 【patch_normalization:】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_patch_normalization_SelectedIndexChanged(object sender, EventArgs e) // (20200319) Jeff Revised!
        {
            Param.patch_normalization = (enu_patch_normalization)comboBox_patch_normalization.SelectedIndex;
        }

        /// <summary>
        /// 【patch_rotational_robustness:】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbx_patch_rotational_robustness_CheckedChanged(object sender, EventArgs e) // (20200319) Jeff Revised!
        {
            Param.patch_rotational_robustness = cbx_patch_rotational_robustness.Checked;
        }

        HObject TrainingRegion = null;

        /// <summary>
        /// 【檢視範圍】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectObj_Click(object sender, EventArgs e) // (20200319) Jeff Revised!
        {
            // 檢查是否已讀取影像
            if (!Check_ImageList())
                return;

            HObject CellRegion;
            try
            {
                if (TrainingRegion != null)
                {
                    TrainingRegion.Dispose();
                    TrainingRegion = null;
                }

                HOperatorSet.ClearWindow(Display.HalconWindow);
                HOperatorSet.DispObj(Get_SourceImage(), Display.HalconWindow);

                HTuple Row = null, Column = null, Angle = null, Score = null;
                HObject PatternRegions;
                InductanceInsp.PtternMatchAndSegRegion_New(Get_SourceImage_Match(), Recipe, out PatternRegions, out Angle, out Row, out Column, out CellRegion);
                //InductanceInsp.GetMatchInfo(Get_SourceImage_Match(), Recipe, out Row, out Column, out Angle, out Score);
                //InductanceInsp.GetCellRegion(Row, Column, Angle, Score, Recipe, out CellRegion);

                InductanceInsp.GetUsedRegion(PatternRegions, Row, Column, Angle, Score, Recipe, AlgorithmParam.SelectRegionIndex, CellRegion, out TrainingRegion);

                HOperatorSet.SortRegion(TrainingRegion, out TrainingRegion, "character", "true", "row");

                HOperatorSet.ErosionRectangle1(TrainingRegion, out TrainingRegion, Param.EdgeSkipSizeWidth, Param.EdgeSkipSizeHeight); // (20200319) Jeff Revised!
                HOperatorSet.DilationRectangle1(TrainingRegion, out TrainingRegion, Param.EdgeExtSizeWidth, Param.EdgeExtSizeHeight);
                
                HTuple Count;
                HOperatorSet.CountObj(TrainingRegion, out Count);

                comboBoxSelectRegion.Items.Clear();

                for (int i = 0; i < Count; i++)
                {
                    comboBoxSelectRegion.Items.Add(i.ToString());
                }

            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        private void comboBoxSelectRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox Obj = (ComboBox)sender;

            if (Obj.SelectedIndex < 0)
            {
                return;
            }
            try
            {
                HOperatorSet.ClearWindow(Display.HalconWindow);
                HOperatorSet.DispObj(Get_SourceImage(), Display.HalconWindow);

                if (TrainingRegion != null)
                {
                    HObject SelectRegion;
                    HOperatorSet.SelectObj(TrainingRegion, out SelectRegion, Obj.SelectedIndex + 1);

                    HOperatorSet.SetDraw(Display.HalconWindow, "margin");
                    HOperatorSet.SetColored(Display.HalconWindow, 12);
                    HOperatorSet.DispObj(SelectRegion, Display.HalconWindow);
                }
            }
            catch(Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        private void btnAddImage_Click(object sender, EventArgs e)
        {
            try
            {
                HTuple hv_Indices;
                HObject SelectObj, ReducImg;

                HOperatorSet.SelectObj(TrainingRegion, out SelectObj, comboBoxSelectRegion.SelectedIndex + 1);

                HObject InspImage = clsStaticTool.GetChannelImage(Get_SourceImage(), AlgorithmParam.Band);

                HOperatorSet.ReduceDomain(InspImage, SelectObj, out ReducImg);
                HObject Crop;
                HOperatorSet.CropDomain(ReducImg, out Crop);

                HOperatorSet.AddTextureInspectionModelImage(Crop, TextureModel, out hv_Indices);

                InspImage.Dispose();
                SelectObj.Dispose();
                ReducImg.Dispose();

                MessageBox.Show("加入影像完成");
            }
            catch(Exception Ex)
            {
                MessageBox.Show("加入影像失敗");
                MessageBox.Show(Ex.ToString());
                return;
            }
        }

        public class clsTraining
        {
            public clsRecipe.clsTextureInsp Recipe;
            public HTuple TextureModel;

            public clsTraining(clsRecipe.clsTextureInsp pmRecipe, HTuple pmTextureModel)
            {
                this.Recipe = pmRecipe;
                this.TextureModel = pmTextureModel;
            }
        }

        private static void Worker_Training(object sender, DoWorkEventArgs e) // (20200319) Jeff Revised!
        {
            BackgroundWorker Worker = (BackgroundWorker)sender;
            clsTraining Param = (clsTraining)e.Argument;
            clsProgressbar m_ProgressBar = new clsProgressbar();
            m_ProgressBar.SetShowText("請等待Mode訓練......");
            m_ProgressBar.SetShowCaption("訓練中......");
            m_ProgressBar.ShowWaitProgress();
            try
            {
                HTuple Levels = new HTuple(Param.Recipe.LevelStart);

                for (int i = Param.Recipe.LevelStart + 1; i <= Param.Recipe.LevelEnd; i++)
                {
                    HTuple Obj = i;
                    HOperatorSet.TupleConcat(Levels, Obj, out Levels);
                    //Levels.TupleConcat(Obj);
                }

                HOperatorSet.SetTextureInspectionModelParam(Param.TextureModel, "patch_normalization", Param.Recipe.patch_normalization.ToString()); // (20200319) Jeff Revised!

                string patch_rotational_robustness = Param.Recipe.patch_rotational_robustness ? "true" : "false";
                HOperatorSet.SetTextureInspectionModelParam(Param.TextureModel, "patch_rotational_robustness", patch_rotational_robustness); // (20200319) Jeff Revised!
                HOperatorSet.SetTextureInspectionModelParam(Param.TextureModel, "levels", Levels);
                HOperatorSet.SetTextureInspectionModelParam(Param.TextureModel, "sensitivity", Param.Recipe.Sensitivity);
                HOperatorSet.SetTextureInspectionModelParam(Param.TextureModel, "gmm_em_max_iter", Param.Recipe.max_Iter);
                
                try
                {
                    HOperatorSet.TrainTextureInspectionModel(Param.TextureModel);
                }
                catch (Exception E)
                {
                    MessageBox.Show(E.ToString());
                }

                m_ProgressBar.CloseProgress();
                MessageBox.Show("訓練完成", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
            }
            catch (HalconException Ex)
            {
                m_ProgressBar.CloseProgress();
                MessageBox.Show(Ex.ToString());
            }
        }


        private void btnTrain_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("此按鈕會讓Mode重新訓練,請確認是否繼續?", "警告!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2, (MessageBoxOptions)0x40000);
            
            if (dialogResult != DialogResult.Yes)
            {
                return;
            }

            if (TextureModel == null)
            {
                MessageBox.Show("尚未建立Mode");
                return;
            }

            BackgroundWorker Worker = new BackgroundWorker();
            clsTraining P = new clsTraining(Param, TextureModel);
            Worker.WorkerReportsProgress = true;
            Worker.DoWork += new DoWorkEventHandler(Worker_Training);
            Worker.RunWorkerAsync(P);

            btnSaveModel.Enabled = true;
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
        /// 【測試】 (訓練參數)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTextTest_Click(object sender, EventArgs e) // (20200319) Jeff Revised!
        {
            // 檢查是否已讀取影像
            if (!Check_ImageList())
                return;

            HObject CellRegion,InspRegion;
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

                HOperatorSet.SortRegion(InspRegion, out InspRegion, "character", "true", "row");

                HObject DefectRegion;
                HOperatorSet.GenEmptyObj(out DefectRegion);

                InductanceInsp.TextureInsp(SrcImg, InspRegion, Param, AlgorithmParam, TextureModel, Resoluton, out DefectRegion, true); // (20200319) Jeff Revised!

                HOperatorSet.SetDraw(Display.HalconWindow, "fill");
                HOperatorSet.SetColored(Display.HalconWindow, 12);
                HOperatorSet.DispObj(DefectRegion, Display.HalconWindow);

            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        private void btnSaveModel_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Param.ModelPath))
            {
                MessageBox.Show("請先確認檔案路徑");
                return;
            }

            DialogResult dialogResult = MessageBox.Show("是否確認儲存Mode,請確認是否繼續?", "警告!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (dialogResult != DialogResult.Yes)
            {
                return;
            }

            if (!Directory.Exists(Param.ModelPath))
            {
                Directory.CreateDirectory(Param.ModelPath);
            }

            if (File.Exists(Param.ModelPath + "TextureModel.htim"))
            {
                DialogResult FileExistsDialog = MessageBox.Show("檔案已存在,確認是否覆蓋檔案?? \n※警告: 如果已載入舊Model，要設定Novelty時，請重新新建Model後做設定，接著儲存，否則會儲存失敗導致檔案損毀!", "警告!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (FileExistsDialog != DialogResult.Yes)
                {
                    return;
                }
            }

            try
            {
                HOperatorSet.WriteTextureInspectionModel(TextureModel, Param.ModelPath + "TextureModel.htim");
                btnSaveModel.Enabled = false;
                MessageBox.Show("儲存完成");
            }
            catch (HalconException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void FormTextureInsp_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (TextureModel != null)
            {
                if (TextureModel.Length > 0)
                {
                    HOperatorSet.ClearTextureInspectionModel(TextureModel);
                    TextureModel = null;
                }
            }

            if (TrainingRegion != null)
            {
                TrainingRegion.Dispose();
                TrainingRegion = null;
            }

            FormClosedEvent(true);
        }

        private void cbxAdvSetting_CheckedChanged(object sender, EventArgs e) // (20200319) Jeff Revised!
        {
            CheckBox Obj = (CheckBox)sender;

            gbxAdvParam.Visible = Obj.Checked;

            if (Obj.Checked)
            {
                this.Width = 1220;
            }
            else
            {
                this.Width = 380;
            }
        }

        HObject TrainingImage = null;
        private void btnGetTrainingImage_Click(object sender, EventArgs e)
        {
            if (TextureModel == null)
                return;
            if (TextureModel.Length < 0)
                return;
            try
            {
                if (TrainingImage!=null)
                {
                    TrainingImage.Dispose();
                    TrainingImage = null;
                }

                HOperatorSet.GetTextureInspectionModelImage(out TrainingImage, TextureModel);
                HTuple Count;
                comboBoxTrainingImageList.Items.Clear();
                HOperatorSet.CountObj(TrainingImage, out Count);
                for (int i = 0; i < Count; i++)
                {
                    comboBoxTrainingImageList.Items.Add(i.ToString());
                }
            }
            catch(Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        private void comboBoxTrainingImageList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox Obj = (ComboBox)sender;
            if (TrainingImage == null)
                return;
            HTuple Count;
            HOperatorSet.CountObj(TrainingImage, out Count);
            if (Count <= 0)
                return;

            HOperatorSet.ClearWindow(HWindowDisplay.HalconWindow);
            HOperatorSet.DispObj(TrainingImage[Obj.SelectedIndex + 1], HWindowDisplay.HalconWindow);
        }

        private void nudGetResultLevel_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;

            int Max = int.Parse(Param.LevelEnd.ToString());

            int Min = int.Parse(Param.LevelStart.ToString());

            int Value = int.Parse(Obj.Value.ToString());

            if (Value < Min || Value > Max)
            {
                Obj.Value = decimal.Parse(Min.ToString());
                MessageBox.Show("超出範圍");
                return;
            }
        }

        /// <summary>
        /// 【取得結果】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetResult_Click(object sender, EventArgs e)
        {
            // 檢查是否已讀取影像
            if (!Check_ImageList())
                return;

            HObject CellRegion, InspRegion;
            try
            {
                HOperatorSet.ClearWindow(HWindowDisplay.HalconWindow);
                
                HTuple Row = null, Column = null, Angle = null, Score = null;
                HObject PatternRegions;
                InductanceInsp.PtternMatchAndSegRegion_New(Get_SourceImage_Match(), Recipe, out PatternRegions, out Angle, out Row, out Column, out CellRegion);
                //InductanceInsp.GetMatchInfo(Get_SourceImage_Match(), Recipe, out Row, out Column, out Angle, out Score);
                //InductanceInsp.GetCellRegion(Row, Column, Angle, Score, Recipe, out CellRegion);

                InductanceInsp.GetUsedRegion(PatternRegions, Row, Column, Angle, Score, Recipe, AlgorithmParam.SelectRegionIndex, CellRegion, out InspRegion);

                HOperatorSet.SortRegion(InspRegion, out InspRegion, "character", "true", "row");

                HObject Image, Region;
                HOperatorSet.GenEmptyObj(out Image);

                InductanceInsp.TextureLevelResult(Get_SourceImage(), InspRegion, Param, AlgorithmParam, TextureModel, out Image, out Region);

                int SelectLevel = int.Parse(nudGetResultLevel.Value.ToString()) - Param.LevelStart + 1;

                HObject SelectImage, SelectRegion;
                HOperatorSet.SelectObj(Image, out SelectImage, SelectLevel);
                HOperatorSet.SelectObj(Region, out SelectRegion, SelectLevel);

                HOperatorSet.SetDraw(HWindowDisplay.HalconWindow, "margin");
                HOperatorSet.SetColored(HWindowDisplay.HalconWindow, 12);
                HOperatorSet.DispObj(SelectImage, HWindowDisplay.HalconWindow);

                HOperatorSet.DispObj(SelectRegion, HWindowDisplay.HalconWindow);

            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        private void btnRemoveImage_Click(object sender, EventArgs e)
        {
            if (TextureModel == null)
                return;
            if (TextureModel.Length < 0)
                return;
            if (comboBoxTrainingImageList.SelectedIndex < 0)
                return;
            try
            {
                HTuple RemainIndex;
                HTuple SelectIndex = comboBoxTrainingImageList.SelectedIndex + 1;
                HOperatorSet.RemoveTextureInspectionModelImage(TextureModel, SelectIndex, out RemainIndex);

                btnGetTrainingImage.PerformClick();

                MessageBox.Show("移除完成,請重新訓練Model");

            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        private void btnGetNovelty_TH_Click(object sender, EventArgs e)
        {
            if (TextureModel == null)
                return;
            if (TextureModel.Length < 0)
                return;

            try
            {
                HTuple Value;
                HOperatorSet.GetTextureInspectionModelParam(TextureModel, "novelty_threshold", out Value);
                string Text = "[";
                for (int i = 0; i < Value.Length; i++)
                {
                    double S = Value[i].D;
                    if (i == Value.Length - 1)
                        Text += S.ToString("###.#");
                    else
                        Text += S.ToString("###.#") + ",";
                }
                txbNovelty_TH.Text = Text + "]";
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        private void btnSetNovelty_TH_Click(object sender, EventArgs e)
        {
            if (TextureModel == null)
                return;
            if (TextureModel.Length < 0)
                return;

            try
            {
                HTuple OrgValue;
                HOperatorSet.GetTextureInspectionModelParam(TextureModel, "novelty_threshold", out OrgValue);

                int LevelStart = int.Parse(nudLevelStart.Value.ToString());
                int Level = int.Parse(nudGetResultLevel.Value.ToString());
                int Offset = int.Parse(nudThOffset.Value.ToString());

                OrgValue[Level - LevelStart] += Offset;

                HOperatorSet.SetTextureInspectionModelParam(TextureModel, "novelty_threshold", OrgValue);

                btnSaveModel.Enabled = true;

            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        private void nudanisometry_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;
            Param.anisometryMin = double.Parse(Obj.Value.ToString());
        }

        private void cbxanisometryEnabled_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox Obj = (CheckBox)sender;
            Param.anisometryEnabled = Obj.Checked;
        }
        public HDrawingObject drawing_Rect = null;
        private void cbxTrainROI_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox Obj = (CheckBox)sender;
            
            if (Obj.Checked)
            {
                #region 設定範圍

                try
                {
                    clsStaticTool.EnableAllControls(this.tbTestureInspParam, Obj, false);
                    InductanceInspUC.ChangeColor(Color.Green, Obj);
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
                    Display.HalconWindow.AttachDrawingObjectToWindow(drawing_Rect);
                }
                catch (Exception ex)
                {
                    cbxTrainROI.CheckedChanged -= cbxTrainROI_CheckedChanged;
                    cbxTrainROI.Checked = false;
                    cbxTrainROI.CheckedChanged += cbxTrainROI_CheckedChanged;
                    drawing_Rect.Dispose();
                    drawing_Rect = null;
                    clsStaticTool.EnableAllControls(this.tbTestureInspParam, Obj, true);
                    InductanceInspUC.ChangeColor(Color.LightGray, Obj);
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
                        clsStaticTool.EnableAllControls(this.tbTestureInspParam, Obj, true);
                        InductanceInspUC.ChangeColor(Color.LightGray, Obj);
                        return;
                    }

                    HObject Hotspot_Rect = null;
                    HTuple row1, column1, row2, column2;
                    InductanceInspUC.GetRectData(drawing_Rect.ID, out column1, out row1, out column2, out row2);
                    HOperatorSet.GenRectangle1(out Hotspot_Rect, row1, column1, row2, column2);
                    HTuple hv_Indices;

                    HObject ReducImg;
                    HOperatorSet.ReduceDomain(Get_SourceImage(), Hotspot_Rect, out ReducImg);

                    HObject Crop;
                    HOperatorSet.CropDomain(ReducImg, out Crop);

                    HObject TrainImage;
                    TrainImage = clsStaticTool.GetChannelImage(Crop, AlgorithmParam.Band);

                    HOperatorSet.AddTextureInspectionModelImage(TrainImage, TextureModel, out hv_Indices);

                    Crop.Dispose();
                    ReducImg.Dispose();
                    drawing_Rect.Dispose();
                    drawing_Rect = null;
                    InductanceInspUC.ChangeColor(Color.LightGray, Obj);
                    clsStaticTool.EnableAllControls(this.tbTestureInspParam, Obj, true);
                    MessageBox.Show("加入成功");
                }
                catch (Exception ex)
                {
                    cbxTrainROI.CheckedChanged -= cbxTrainROI_CheckedChanged;
                    cbxTrainROI.Checked = false;
                    cbxTrainROI.CheckedChanged += cbxTrainROI_CheckedChanged;
                    drawing_Rect.Dispose();
                    drawing_Rect = null;
                    clsStaticTool.EnableAllControls(this.tbTestureInspParam, Obj, true);
                    InductanceInspUC.ChangeColor(Color.LightGray, Obj);
                    MessageBox.Show(ex.ToString());
                }
                #endregion
            }
        }
    }
}
