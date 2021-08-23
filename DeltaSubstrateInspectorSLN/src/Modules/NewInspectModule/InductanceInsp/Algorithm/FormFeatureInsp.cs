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
    public partial class FormFeatureInsp : Form
    {
        public class clsTrainInfo
        {
            public HObject Image;
            public List<HObject> RegionList = new List<HObject>();
            public HObject OriginalInspRegion;

            public clsTrainInfo() { }
        }

        clsRecipe.clsFeatureInsp Param;
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
        double Resoluton = 0;

        HTuple MLPModel = null;
        List<clsTrainInfo> TrainingInfoList = new List<clsTrainInfo>();
        

        public delegate void ChangeRegionHandler(HObject Obj, HTuple GrayMean, HTuple GrayMin, HTuple DarkRatio,HTuple Radius);//定義委派
        public event ChangeRegionHandler ChangeRegion;  //定義事件

        public delegate void FormClosedHandler(bool FormClosed);//定義委派
        public event FormClosedHandler FormClosedEvent;  //定義事件


        public FormFeatureInsp(List<HObject> pmImageList, HSmartWindowControl pmDisplay, InductanceInspRole pmRecipe, clsRecipe.clsAlgorithm pmAlgorithmParam, double pmResoluton, List<HObject> resultImages = null, List<HObject> resultImages_UsedRegion = null) // (20200130) Jeff Revised!
        {
            InitializeComponent();

            this.ImageList = pmImageList;
            this.Display = pmDisplay;
            this.Recipe = pmRecipe;
            this.AlgorithmParam = pmAlgorithmParam;
            this.Param = (clsRecipe.clsFeatureInsp)AlgorithmParam.Param;
            this.Resoluton = pmResoluton;
            this.ResultImages = resultImages; // (20200130) Jeff Revised!
            this.ResultImages_UsedRegion = resultImages_UsedRegion; // (20200130) Jeff Revised!

            InsertBand2UI(); // (20200130) Jeff Revised!
            UpdateControl();
            InitLoadModel();
            this.HWindowDisplay.MouseWheel += HWindowDisplay.HSmartWindowControl_MouseWheel;
            //nudImageIndex.Maximum = ImageList.Count - 1; // (20200130) Jeff Revised!
            LoadFeatureType();
            UpdateTextureLaws();

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

        public void LoadFeatureType()
        {
            string[] ArrayMethod = Enum.GetNames(typeof(enuMLPTextureLaws));
            if (ArrayMethod.Length > 0)
            {
                foreach (string s in ArrayMethod)
                {
                    comboBoxFeature.Items.Add(s);
                }
            }
        }
        

        public void InitLoadModel()
        {
            try
            {
                if (!string.IsNullOrEmpty(Param.ModelPath))
                {
                    if (!File.Exists(Param.ModelPath + "classifier"))
                    {
                        MessageBox.Show("Model檔案不存在,請建立新Model");
                        return;
                    }
                    HOperatorSet.ReadClassMlp(Param.ModelPath + "classifier", out MLPModel);
                }
            }
            catch(Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        public void UpdateControl()
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
            nudAreaMin.Value = decimal.Parse(Param.AreaMin.ToString());
            nudHeightMin.Value = decimal.Parse(Param.HeightMin.ToString());
            nudWidthMin.Value = decimal.Parse(Param.WidthMin.ToString());
            cbxInspAreaEnabled.Checked = Param.AEnabled;
            cbxInspWidthEnabled.Checked = Param.WEnabled;
            cbxInspHeightEnabled.Checked = Param.HEnabled;
            cbxAIInspection.Checked = AlgorithmParam.bUsedDAVS;

            #endregion

            #region 訓練參數

            txbModelPath.Text = Param.ModelPath;
      
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

                InductanceInsp.GetUsedRegion(PatternRegions, Row, Column, Angle, Score, Recipe, AlgorithmParam.SelectRegionIndex,CellRegion, out InspRegion);

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
        private void comboBoxMultiTHBand_SelectedIndexChanged(object sender, EventArgs e) // (20200211) Jeff Revised!
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
            HOperatorSet.ClearWindow(HWindowDisplay.HalconWindow); // (20200211) Jeff Revised!
            HOperatorSet.DispObj(DisplayImage, HWindowDisplay.HalconWindow); // (20200211) Jeff Revised!
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

        private void txbModelPath_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog FileDialog = new FolderBrowserDialog();
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
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
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
                if (MLPModel != null)
                {
                    if (MLPModel.Length > 0)
                    {
                        HOperatorSet.ClearClassMlp(MLPModel);
                        MLPModel = null;
                    }
                }
                HOperatorSet.CreateClassMlp(Param.FeatureLaws.Count, Param.NumHidden, 2, "softmax", "normalization", 3, 42, out MLPModel);
                btnSaveModel.Enabled = true;
            }
            catch(Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        private void nudLevelEnd_ValueChanged(object sender, EventArgs e)
        {

        }

        HObject TrainingRegion = null;

        private void btnAddImage_Click(object sender, EventArgs e)
        {
            try
            {
                HObject ReadImage;
                ReadImage = clsStaticTool.GetChannelImage(Get_SourceImage(), AlgorithmParam.Band);

                #region Get Insp Region

                HObject CellRegion, InspRegion;

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

                #endregion

                clsTrainInfo Info = new clsTrainInfo();
                Info.Image = ReadImage;
                Info.OriginalInspRegion = DilationRegion;

                TrainingInfoList.Add(Info);

                UpdateComboboxUI();

                comboBoxImageList.SelectedIndex = TrainingInfoList.Count - 1;

                

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
            public clsRecipe.clsFeatureInsp Recipe;
            public HTuple Model;

            public clsTraining(clsRecipe.clsFeatureInsp pmRecipe, HTuple pmModel)
            {
                this.Recipe = pmRecipe;
                this.Model = pmModel;
            }
        }

        private static void Worker_Training(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker Worker = (BackgroundWorker)sender;
            clsTraining Param = (clsTraining)e.Argument;
            clsProgressbar m_ProgressBar = new clsProgressbar();
            m_ProgressBar.SetShowText("請等待Mode訓練......");
            m_ProgressBar.SetShowCaption("訓練中......");
            m_ProgressBar.ShowWaitProgress();
            try
            {
                HTuple Error, ErrorLog;
                HOperatorSet.TrainClassMlp(Param.Model, 200, 1, 0.01, out Error, out ErrorLog);

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

            if (MLPModel == null)
            {
                MessageBox.Show("尚未建立Mode");
                return;
            }

            BackgroundWorker Worker = new BackgroundWorker();
            clsTraining P = new clsTraining(Param, MLPModel);
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
        private void btnTextTest_Click(object sender, EventArgs e) // (20200130) Jeff Revised!
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

                HOperatorSet.Union1(InspRegion, out InspRegion);


                HObject InspImg = InductanceInsp.Convert2TrainingImg(clsStaticTool.GetChannelImage(SrcImg, AlgorithmParam.Band), Param.FeatureLaws);

                HObject RedueImg;
                HOperatorSet.ReduceDomain(InspImg, InspRegion, out RedueImg);

                HObject ClassRegion;
                HOperatorSet.ClassifyImageClassMlp(RedueImg, out ClassRegion, MLPModel, 0.5);

                HOperatorSet.SelectObj(ClassRegion, out DefectRegion, 1);


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

            if (File.Exists(Param.ModelPath + "classifier"))
            {
                DialogResult FileExistsDialog = MessageBox.Show("檔案已存在,確認是否覆蓋檔案??", "警告!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (FileExistsDialog != DialogResult.Yes)
                {
                    return;
                }
            }

            try
            {
                HOperatorSet.WriteClassMlp(MLPModel, Param.ModelPath + "classifier");
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
            if (MLPModel != null)
            {
                if (MLPModel.Length > 0)
                {
                    HOperatorSet.ClearClassMlp(MLPModel);
                    MLPModel = null;
                }
            }

            if (TrainingRegion != null)
            {
                TrainingRegion.Dispose();
                TrainingRegion = null;
            }

            if (TrainingInfoList != null)
            {
                clsStaticTool.DisposeAll(TrainingInfoList);
                TrainingInfoList = null;
            }

            FormClosedEvent(true);
        }
        
        HDrawingObject drawing_Rect = null;
        private void cbxaddDefectRegion_CheckedChanged(object sender, EventArgs e)
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

                    drawing_Rect = HDrawingObject.CreateDrawingObject(HDrawingObject.HDrawingObjectType.RECTANGLE2,
                                                                  1500,
                                                                  2048,
                                                                  0, 100, 100);


                    drawing_Rect.SetDrawingObjectParams("color", "green");
                    HWindowDisplay.HalconWindow.AttachDrawingObjectToWindow(drawing_Rect);
                }
                catch (Exception ex)
                {
                    Obj.CheckedChanged -= cbxaddDefectRegion_CheckedChanged;
                    Obj.Checked = false;
                    Obj.CheckedChanged += cbxaddDefectRegion_CheckedChanged;
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
                    HObject Rect = null;
                    //HTuple row1, column1, row2, column2;
                    //InductanceInspUC.GetRectData(drawing_Rect.ID, out column1, out row1, out column2, out row2);
                    //HOperatorSet.GenRectangle1(out Hotspot_Rect, row1, column1, row2, column2);

                    HTuple row, column, phi, length1, length2;
                    InductanceInspUC.GetRect2Data(drawing_Rect.ID, out column, out row, out phi, out length1, out length2);
                    HOperatorSet.GenRectangle2(out Rect, row, column, phi, length1, length2);

                    TrainingInfoList[comboBoxImageList.SelectedIndex].RegionList.Add(Rect);

                    UpdateDefectList(comboBoxImageList.SelectedIndex);

                    drawing_Rect.Dispose();
                    drawing_Rect = null;
                    InductanceInspUC.ChangeColor(Color.LightGray, Obj);
                    clsStaticTool.EnableAllControls(this.tbTestureInspParam, Obj, true);
                    MessageBox.Show("新增成功");
                }
                catch (Exception ex)
                {
                    Obj.CheckedChanged -= cbxaddDefectRegion_CheckedChanged;
                    Obj.Checked = false;
                    Obj.CheckedChanged += cbxaddDefectRegion_CheckedChanged;
                    drawing_Rect.Dispose();
                    drawing_Rect = null;
                    clsStaticTool.EnableAllControls(this.tbTestureInspParam, Obj, true);
                    InductanceInspUC.ChangeColor(Color.LightGray, Obj);
                    MessageBox.Show(ex.ToString());
                }
                #endregion
            }
        }

        public void UpdateDefectList(int SelectIndex)
        {
            comboBoxDefectList.Items.Clear();

            for (int i = 0; i < TrainingInfoList[SelectIndex].RegionList.Count; i++)
            {
                comboBoxDefectList.Items.Add(i.ToString());
            }
        }

        private void comboBoxDefectList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox Obj = (ComboBox)sender;

            int ImageSelectIndex = comboBoxImageList.SelectedIndex;

            if (ImageSelectIndex < 0)
                return;

            if (Obj.SelectedIndex < 0)
                return;

            if (TrainingInfoList[ImageSelectIndex].RegionList == null)
                return;

            if (TrainingInfoList[ImageSelectIndex].RegionList.Count <= 0)
                return;

            HTuple SelectIndex = Obj.SelectedIndex;

            HObject SelectObj = TrainingInfoList[ImageSelectIndex].RegionList[SelectIndex];
            
            HOperatorSet.SetDraw(HWindowDisplay.HalconWindow, "margin");
            HOperatorSet.SetColor(HWindowDisplay.HalconWindow, "red");
            HOperatorSet.DispObj(SelectObj, HWindowDisplay.HalconWindow);
        }

        private void comboBoxFeature_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxFeature.SelectedIndex < 0)
                return;

            try
            {
                HObject Image = clsStaticTool.GetChannelImage(Get_SourceImage(), AlgorithmParam.Band);

                HTuple filter = ((enuMLPTextureLaws)comboBoxFeature.SelectedIndex).ToString();

                HTuple Shift = int.Parse(nudShift.Value.ToString());

                HTuple FilterSize = int.Parse(nudFilterSize.Value.ToString());

                HObject FeatureImage;
                HOperatorSet.TextureLaws(Image, out FeatureImage, filter, Shift, FilterSize);

                HOperatorSet.DispObj(FeatureImage, HWindowDisplay.HalconWindow);

                FeatureImage.Dispose();
                Image.Dispose();
            }
            catch
            {
                MessageBox.Show("Error");
            }
        }

        public void UpdateTextureLaws()
        {
            this.listViewLaws.BeginUpdate();
            listViewLaws.Items.Clear();
            for (int i = 0; i < Param.FeatureLaws.Count; i++)
            {
                ListViewItem lvi = new ListViewItem(Param.FeatureLaws[i].FeatureLaws.ToString());
                lvi.SubItems.Add(Param.FeatureLaws[i].Size.ToString());
                lvi.SubItems.Add(Param.FeatureLaws[i].Shift.ToString());
                
                listViewLaws.Items.Add(lvi);
            }
            this.listViewLaws.EndUpdate();
        }

        public void UpdateComboboxUI()
        {
            comboBoxImageList.Items.Clear();

            for (int i = 0; i < TrainingInfoList.Count; i++)
            {
                comboBoxImageList.Items.Add(i.ToString());
            }
        }
        private void btnAddLaw_Click(object sender, EventArgs e)
        {
            int SelectIndex = comboBoxFeature.SelectedIndex;
            if (SelectIndex < 0)
                return;

            int Size = int.Parse(nudFilterSize.Value.ToString());
            int Shift = int.Parse(nudShift.Value.ToString());

            clsRecipe.clsFeatureLaws Tmp = new clsRecipe.clsFeatureLaws();

            Tmp.FeatureLaws = (enuMLPTextureLaws)SelectIndex;
            Tmp.Shift = Shift;
            Tmp.Size = Size;
            
            Param.FeatureLaws.Add(Tmp);

            UpdateTextureLaws();
        }

        private void btnRemoveLaw_Click(object sender, EventArgs e)
        {
            if (listViewLaws.SelectedIndices.Count <= 0)
                return;

            int SelectIndex = listViewLaws.SelectedIndices[0];

            Param.FeatureLaws.RemoveAt(SelectIndex);

            UpdateTextureLaws();
        }

        private void btnClearLaws_Click(object sender, EventArgs e)
        {
            Param.FeatureLaws.Clear();
            UpdateTextureLaws();
        }

        private void btnRemoveImg_Click(object sender, EventArgs e)
        {
            try
            {
                int SelectIndex = comboBoxImageList.SelectedIndex;
                if (SelectIndex < 0)
                    return;

                TrainingInfoList.RemoveAt(SelectIndex);

                UpdateComboboxUI();

            }
            catch (Exception Ex)
            {
                MessageBox.Show("Fail...");
                MessageBox.Show(Ex.ToString());
                return;
            }
        }

        private void comboBoxImageList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int SelectIndex = comboBoxImageList.SelectedIndex;
            if (SelectIndex < 0)
                return;

            comboBoxDefectList.Items.Clear();

            for (int i = 0; i < TrainingInfoList[SelectIndex].RegionList.Count; i++)
            {
                comboBoxDefectList.Items.Add(i.ToString());
            }

            HOperatorSet.ClearWindow(HWindowDisplay.HalconWindow);
            HOperatorSet.DispObj(TrainingInfoList[SelectIndex].Image, HWindowDisplay.HalconWindow);

        }

        private void btnAdd2Mode_Click(object sender, EventArgs e)
        {
            try
            {
                if (MLPModel==null)
                {
                    MessageBox.Show("尚未建立Model");
                    return;
                }

                for (int i = 0; i < TrainingInfoList.Count; i++)
                {
                    HObject TrainingImage = InductanceInsp.Convert2TrainingImg(clsStaticTool.GetChannelImage(Get_SourceImage(), AlgorithmParam.Band), Param.FeatureLaws);

                    HObject InspRegion = TrainingInfoList[i].OriginalInspRegion;

                    HObject UnionInspRegion;
                    HOperatorSet.Union1(InspRegion, out UnionInspRegion);

                    HObject DefectRegionSummary;
                    HOperatorSet.GenEmptyObj(out DefectRegionSummary);
                    for (int j = 0; j < TrainingInfoList[i].RegionList.Count; j++)
                    {
                        HOperatorSet.Union2(TrainingInfoList[i].RegionList[j], DefectRegionSummary, out DefectRegionSummary);
                    }
                    HObject PASSRegion;
                    HOperatorSet.Difference(UnionInspRegion, DefectRegionSummary, out PASSRegion);

                    HObject TrainRegion;
                    HOperatorSet.ConcatObj(DefectRegionSummary, PASSRegion, out TrainRegion);

                    HOperatorSet.AddSamplesImageClassMlp(TrainingImage, TrainRegion, MLPModel);
                }
            }
            catch(Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        private void listViewLaws_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewLaws.SelectedIndices.Count <= 0)
                return;

            int SelectIndex = listViewLaws.SelectedIndices[0];

            try
            {
                HObject Image = clsStaticTool.GetChannelImage(Get_SourceImage(), AlgorithmParam.Band);

                HTuple filter = Param.FeatureLaws[SelectIndex].FeatureLaws.ToString();

                HTuple Shift = Param.FeatureLaws[SelectIndex].Shift;

                HTuple FilterSize = Param.FeatureLaws[SelectIndex].Size;

                HObject FeatureImage;
                HOperatorSet.TextureLaws(Image, out FeatureImage, filter, Shift, FilterSize);

                HOperatorSet.DispObj(FeatureImage, HWindowDisplay.HalconWindow);

                FeatureImage.Dispose();
                Image.Dispose();
            }
            catch
            {

            }
        }
    }
}
