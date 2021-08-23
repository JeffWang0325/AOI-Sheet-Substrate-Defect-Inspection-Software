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

namespace DeltaSubstrateInspector.src.Modules.NewInspectModule.PatternCheck
{
    public partial class PatternCheckUC : UserControl
    {
        #region 標準變數
        public event EventHandler OnUserControlButtonClicked;
        
        public static PatternCheck patternCheck_method_ = new PatternCheck();
        private PatternCheckRole patternCheck_role = new PatternCheckRole();
        #endregion

        // 內部參數
        OpenFileDialog openFileDialog1 = new OpenFileDialog();
        HObject load_image = null;


        public PatternCheckUC()
        {
            InitializeComponent();
            this.hWindowControl_InputImage.MouseWheel += hWindowControl_InputImage.HSmartWindowControl_MouseWheel;
            this.hSmartWindowControl_ModelContour.MouseWheel += hSmartWindowControl_ModelContour.HSmartWindowControl_MouseWheel;
        }

        private void button_Add_Click(object sender, EventArgs e)
        {
            OnUserControlButtonClicked(this, e);
        }

        public InspectRole get_role()
        {
            return create_role();
        }

        private PatternCheckRole create_role()
        {
            PatternCheckRole role = new PatternCheckRole();
            role.Method = "PatternCheck";
            return role;
        }

        public void UpdateParameter()
        {
            // Load parameter from XML file
            patternCheck_role.load();

            // Set parameter to UI
            checkBox_InspectBypass.Checked = patternCheck_role.InspectBypass;

            textBox_InspectImgID.Text = patternCheck_role.InspectImgIndex.ToString();
            textBox_Score.Text = patternCheck_role.hv_MinScore.ToString();
            textBox_MaxOverlap.Text = patternCheck_role.hv_MaxOverlap.ToString();
            textBox_EnhancedPat.Text = patternCheck_role.hv_EnhancedPatternMatch.ToString();

            comboBox_NowDAVSMode.SelectedIndex = patternCheck_role.DAVS_Mode;
            comboBox_ImageFmt.SelectedIndex = patternCheck_role.DAVS_ImageFmt;

            comboBox_PredictMode.SelectedIndex = patternCheck_role.DAVS_PredictMode;

            checkBox_PModeSaveOK.Checked = patternCheck_role.DAVS_PModeSaveOK;
            checkBox_PModeSaveNG.Checked = patternCheck_role.DAVS_PModeSaveNG;
            checkBox_TModeSaveAll.Checked = patternCheck_role.DAVS_TModeSaveAll;


        }

        public void UpdateNowInput_ImgList()
        {
            /*
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
            */

        }

        //*****************************************************************************************************************
        private void button_LoadSingleImage_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog1.FileName;
                HOperatorSet.ReadImage(out load_image, path);
                //hWindowControl_InputImage.SetFullImagePart(new HImage(load_image));
                              
                // 顯示影像
                HOperatorSet.DispObj(load_image, hWindowControl_InputImage.HalconWindow);
               
            }
        }

        private void button_LoadPatternModel_Click(object sender, EventArgs e)
        {
            if (!patternCheck_method_.read_shape_model())
            {
                MessageBox.Show("請確認是否已建立樣板!", "Model Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else
            {
                HOperatorSet.SetColor(hSmartWindowControl_ModelContour.HalconWindow, "red");                
                HOperatorSet.DispObj(patternCheck_method_.ModelID_contour, hSmartWindowControl_ModelContour.HalconWindow);
            }
        }

        private void button_PatternMatch_Click(object sender, EventArgs e)
        {
            // 從UI取得數值並且設定Role
            ParamGetFromUIandSet2Role();


            if (!patternCheck_method_.find_shape_model(load_image))
            {
                MessageBox.Show("請確認是否已載入影像或模板!", "Image/Model Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {

                HOperatorSet.ClearWindow(hWindowControl_InputImage.HalconWindow);
                HOperatorSet.SetColor(hWindowControl_InputImage.HalconWindow, "green");                           
                HOperatorSet.DispObj(load_image, hWindowControl_InputImage.HalconWindow);
                HOperatorSet.DispObj(patternCheck_method_.PaternMatchRegions, hWindowControl_InputImage.HalconWindow);

                // Info show
                Button btn = (Button)sender;
                richTextBox_Info.AppendText("模板比對時間 (ms) : " + patternCheck_method_.calc_MS.ToString() + "\n");

            }
        }

        private void button_SaveParam_Click(object sender, EventArgs e)
        {
            // 從UI取得數值並且設定Role
            ParamGetFromUIandSet2Role();

            // Save parameter to XML file !!!! 一定要加
            patternCheck_role.save();
           
        }

        private void ParamGetFromUIandSet2Role()
        {
            // Get parameter from UI
            patternCheck_role.InspectBypass = checkBox_InspectBypass.Checked;
            patternCheck_role.InspectImgIndex = int.Parse(textBox_InspectImgID.Text);
            patternCheck_role.hv_MinScore = double.Parse(textBox_Score.Text);
            patternCheck_role.hv_MaxOverlap = double.Parse(textBox_MaxOverlap.Text);
            patternCheck_role.hv_EnhancedPatternMatch = int.Parse(textBox_EnhancedPat.Text);

            patternCheck_role.DAVS_Mode = comboBox_NowDAVSMode.SelectedIndex;
            patternCheck_role.DAVS_ImageFmt = comboBox_ImageFmt.SelectedIndex;

            patternCheck_role.DAVS_PredictMode = comboBox_PredictMode.SelectedIndex;

            patternCheck_role.DAVS_PModeSaveOK = checkBox_PModeSaveOK.Checked;
            patternCheck_role.DAVS_PModeSaveNG = checkBox_PModeSaveNG.Checked;
            patternCheck_role.DAVS_TModeSaveAll = checkBox_TModeSaveAll.Checked;


            // Set parameter to method
            patternCheck_method_.set_parameter(patternCheck_role);

        }

        private void button_Inspection_Click(object sender, EventArgs e)
        {
            // 從UI取得數值並且設定Role
            ParamGetFromUIandSet2Role();


            // GO
            List<HObject> Input_ImgList = new List<HObject>();

            // 一次Load一張
            Input_ImgList.Add(load_image);

            try
            {
                HObject Result_image;
                patternCheck_method_.action(Input_ImgList, out Result_image);

                if (Result_image != null)
                {
                    UpdatePredictionResult();
                }

            }
            catch(Exception ex)
            {
                string message = "Error: " + ex.Message;
                MessageBox.Show(message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button_LoadParam_Click(object sender, EventArgs e)
        {
            UpdateParameter();
        }

        private void tabPage_ParamPatternIner_Click(object sender, EventArgs e)
        {

        }

        private void button_SegAndWriteSingleChipImg_Click(object sender, EventArgs e)
        {


            try
            {
                if (!patternCheck_method_.segment_and_save_single_chip_images(load_image))
                {
                    MessageBox.Show("請確認是否已載入影像/模板或執行模板比對!", "Sometihg Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {

                    HOperatorSet.SetDraw(hWindowControl_InputImage.HalconWindow, "margin");
                    HOperatorSet.SetColor(hWindowControl_InputImage.HalconWindow, "blue");
                    HOperatorSet.DispObj(load_image, hWindowControl_InputImage.HalconWindow);
                    HOperatorSet.DispObj(patternCheck_method_.PaternMatchSegRegions, hWindowControl_InputImage.HalconWindow);

                    // Info show
                    richTextBox_Info.AppendText(button_SegAndWriteSingleChipImg.Text + " : " + patternCheck_method_.calc_MS.ToString() + "\n");
                    //richTextBox_Info.AppendText("ImageCount: " + patternCheck_method_.ai_resultList.Count.ToString() + "\n");

                }

            }
            catch
            {
                string message = "Error";
                MessageBox.Show(message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
           
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // 從UI取得數值並且設定Role
            ParamGetFromUIandSet2Role();

            try
            {
                if (!patternCheck_method_.DAVS_execute())
                {
                    //MessageBox.Show("請確認是否存在預測影像!", "Sometihg Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    MessageBox.Show("Someting error!", "Sometihg Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    UpdatePredictionResult();
                }
            }
            catch(Exception ex)
            {
                string message = "Error";
                MessageBox.Show(message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }

        private void UpdatePredictionResult()
        {

            HOperatorSet.ClearWindow(hWindowControl_InputImage.HalconWindow);

            // 原圖
            HOperatorSet.DispObj(load_image, hWindowControl_InputImage.HalconWindow);

            switch (patternCheck_role.DAVS_Mode)
            {
                case 0: // 不啟用, 顯示樣板比對結果

                    HOperatorSet.SetDraw(hWindowControl_InputImage.HalconWindow, "margin");
                    HOperatorSet.SetColor(hWindowControl_InputImage.HalconWindow, "green");
                    HOperatorSet.DispObj(patternCheck_method_.PaternMatchRegions, hWindowControl_InputImage.HalconWindow);

                    break;

                case 1: // 線上檢測, 顯示AI: NG結果: Red , OK結果:Green


                    HOperatorSet.SetDraw(hWindowControl_InputImage.HalconWindow, "margin");
                    
                    // OK Region show
                    HOperatorSet.SetColor(hWindowControl_InputImage.HalconWindow, "green");
                    HOperatorSet.DispObj(patternCheck_method_.AIPredictionOKRegions, hWindowControl_InputImage.HalconWindow);

                    // NG Region show
                    HOperatorSet.SetColor(hWindowControl_InputImage.HalconWindow, "red");
                    HOperatorSet.DispObj(patternCheck_method_.AIPredictionDefectRegions, hWindowControl_InputImage.HalconWindow);

                    #region Prediction info

                    // Prediction info
                    int nowFileCnt = patternCheck_method_.ai_resultList.Count();
                    string resultstring = "";
                    for (int i = 0; i < nowFileCnt; i++)
                    {
                        string nowSTR;
                        if (patternCheck_method_.ai_resultList[i]==true)
                        {
                            nowSTR = "NG";
                        }
                        else
                        {
                            nowSTR = "OK";
                        }

                        resultstring += nowSTR + "\n";
                    }

                    richTextBox_PredictionResult.Clear();
                    richTextBox_PredictionResult.AppendText(resultstring);

                    #endregion

                    break;

                case 2: // 離線學習, 顯示切割結果

                    HOperatorSet.SetDraw(hWindowControl_InputImage.HalconWindow, "margin");
                    HOperatorSet.SetColor(hWindowControl_InputImage.HalconWindow, "blue");
                    HOperatorSet.DispObj(load_image, hWindowControl_InputImage.HalconWindow);
                    HOperatorSet.DispObj(patternCheck_method_.PaternMatchSegRegions, hWindowControl_InputImage.HalconWindow);

                    break;

            }            

            // Info show
            richTextBox_Info.AppendText(button_davsInspect.Text + "(ms) : " + patternCheck_method_.calc_MS.ToString() + "\n");

        }

        private void richTextBox_Info_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox_PModeSaveOK_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
