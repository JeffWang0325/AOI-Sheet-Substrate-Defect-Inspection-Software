using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using DeltaSubstrateInspector;
using static DeltaSubstrateInspector.FileSystem.FileSystem;
using HalconDotNet;

using System.Media; // For SystemSounds
using System.IO.Ports;
using System.Diagnostics;
using Load_HDVP; // (20200717) Jeff Revised!

namespace DeltaSubstrateInspector.src.MainPanels
{
    public partial class AdvSet : UserControl
    {

        //public event EventHandler OnUserControlButtonClicked_Test1; // (20190615) Jeff Revised!
        public EventHandler ContiGrab_clicked; // 連續取像/停止取像 觸發事件 於AOIMainForm.cs (20190615) Jeff Revised!
        public EventHandler connectCamera_clicked; // 連接相機/關閉相機 觸發事件 於AOIMainForm.cs (20190615) Jeff Revised!

        AOIMainForm MainForm;

        public AdvSet()
        {
            InitializeComponent();
            this.hWindowControl_InputImage.MouseWheel += hWindowControl_InputImage.HSmartWindowControl_MouseWheel;

            Bitmap bm = (Bitmap)this.button_LoadHDVP.Image; // (20200717) Jeff Revised!
            bm.MakeTransparent();
            this.button_LoadHDVP.Image = bm;

            clsLanguage.clsLanguage.SetLanguateToControls(this); // (20200214) Jeff Revised!
        }

        public void SetMainForm(AOIMainForm pmMainForm)
        {
            this.MainForm = pmMainForm;
            COMPort_Check();
        }

        public void GetMaxCurrent(out int Ch1, out int Ch2, out int Ch3, out int Ch4)
        {
            Ch1 = 500;
            Ch2 = 500;
            Ch3 = 500;
            Ch4 = 500;

            #region Ch1
            switch(Modules.LightModule.LightMaxValueIndex.Light01_MaxVauleIndex)
            {
                case 0:
                    Ch1 = 500;
                    break;
                case 1:
                    Ch1 = 600;
                    break;
                case 2:
                    Ch1 = 700;
                    break;
                case 3:
                    Ch1 = 800;
                    break;
            }
            #endregion

            #region Ch2
            switch (Modules.LightModule.LightMaxValueIndex.Light02_MaxVauleIndex)
            {
                case 0:
                    Ch2 = 500;
                    break;
                case 1:
                    Ch2 = 600;
                    break;
                case 2:
                    Ch2 = 700;
                    break;
                case 3:
                    Ch2 = 800;
                    break;
            }
            #endregion

            #region Ch3
            switch (Modules.LightModule.LightMaxValueIndex.Light03_MaxVauleIndex)
            {
                case 0:
                    Ch3 = 500;
                    break;
                case 1:
                    Ch3 = 600;
                    break;
                case 2:
                    Ch3 = 700;
                    break;
                case 3:
                    Ch3 = 800;
                    break;
            }
            #endregion

            #region Ch4
            switch (Modules.LightModule.LightMaxValueIndex.Light04_MaxVauleIndex)
            {
                case 0:
                    Ch4 = 500;
                    break;
                case 1:
                    Ch4 = 600;
                    break;
                case 2:
                    Ch4 = 700;
                    break;
                case 3:
                    Ch4 = 800;
                    break;
            }
            #endregion
        }

        public void UpdateLightControlParam(bool bConnected)
        {
            if (!bConnected)
            {
                gbxLightControl.Enabled = false;
                return;
            }

            UpdateConnectedControl();

            #region Close Event
            comboBoxMaxCurrent1.SelectedIndexChanged -= comboBoxMaxCurrent1_SelectedIndexChanged;
            comboBoxMaxCurrent2.SelectedIndexChanged -= comboBoxMaxCurrent2_SelectedIndexChanged;
            comboBoxMaxCurrent3.SelectedIndexChanged -= comboBoxMaxCurrent3_SelectedIndexChanged;
            comboBoxMaxCurrent4.SelectedIndexChanged -= comboBoxMaxCurrent4_SelectedIndexChanged;

            sidCh1Power.Scroll -= sidCh1Power_Scroll;
            sidCh2Power.Scroll -= sidCh2Power_Scroll;
            sidCh3Power.Scroll -= sidCh3Power_Scroll;
            sidCh4Power.Scroll -= sidCh4Power_Scroll;

            nudCh1Power.ValueChanged -= nudCh1Power_ValueChanged;
            nudCh2Power.ValueChanged -= nudCh2Power_ValueChanged;
            nudCh3Power.ValueChanged -= nudCh3Power_ValueChanged;
            nudCh4Power.ValueChanged -= nudCh4Power_ValueChanged;

            #endregion

            #region Init Light Value

            comboBoxMaxCurrent1.SelectedIndex = Modules.LightModule.LightMaxValueIndex.Light01_MaxVauleIndex;
            comboBoxMaxCurrent2.SelectedIndex = Modules.LightModule.LightMaxValueIndex.Light02_MaxVauleIndex;
            comboBoxMaxCurrent3.SelectedIndex = Modules.LightModule.LightMaxValueIndex.Light03_MaxVauleIndex;
            comboBoxMaxCurrent4.SelectedIndex = Modules.LightModule.LightMaxValueIndex.Light04_MaxVauleIndex;
            
            int Ch1Max, Ch2Max, Ch3Max, Ch4Max;

            GetMaxCurrent(out Ch1Max, out Ch2Max, out Ch3Max, out Ch4Max);

            sidCh1Power.Maximum = Ch1Max;
            sidCh2Power.Maximum = Ch2Max;
            sidCh3Power.Maximum = Ch3Max;
            sidCh4Power.Maximum = Ch4Max;

            nudCh1Power.Maximum = Ch1Max;
            nudCh2Power.Maximum = Ch2Max;
            nudCh3Power.Maximum = Ch3Max;
            nudCh4Power.Maximum = Ch4Max;



            sidCh1Power.Value = Modules.LightModule.LightValue.Light01_Vaule;
            sidCh2Power.Value = Modules.LightModule.LightValue.Light02_Vaule;
            sidCh3Power.Value = Modules.LightModule.LightValue.Light03_Vaule;
            sidCh4Power.Value = Modules.LightModule.LightValue.Light04_Vaule;

            nudCh1Power.Value = Modules.LightModule.LightValue.Light01_Vaule;
            nudCh2Power.Value = Modules.LightModule.LightValue.Light02_Vaule;
            nudCh3Power.Value = Modules.LightModule.LightValue.Light03_Vaule;
            nudCh4Power.Value = Modules.LightModule.LightValue.Light04_Vaule;

            sidCh1Power.Refresh();
            sidCh2Power.Refresh();
            sidCh3Power.Refresh();
            sidCh4Power.Refresh();

            #endregion

            #region Open Event

            comboBoxMaxCurrent1.SelectedIndexChanged += comboBoxMaxCurrent1_SelectedIndexChanged;
            comboBoxMaxCurrent2.SelectedIndexChanged += comboBoxMaxCurrent2_SelectedIndexChanged;
            comboBoxMaxCurrent3.SelectedIndexChanged += comboBoxMaxCurrent3_SelectedIndexChanged;
            comboBoxMaxCurrent4.SelectedIndexChanged += comboBoxMaxCurrent4_SelectedIndexChanged;

            sidCh1Power.Scroll += sidCh1Power_Scroll;
            sidCh2Power.Scroll += sidCh2Power_Scroll;
            sidCh3Power.Scroll += sidCh3Power_Scroll;
            sidCh4Power.Scroll += sidCh4Power_Scroll;

            nudCh1Power.ValueChanged += nudCh1Power_ValueChanged;
            nudCh2Power.ValueChanged += nudCh2Power_ValueChanged;
            nudCh3Power.ValueChanged += nudCh3Power_ValueChanged;
            nudCh4Power.ValueChanged += nudCh4Power_ValueChanged;

            #endregion

            cbxTriggerChannel1.Checked = true;
            cbxTriggerChannel2.Checked = true;
            cbxTriggerChannel3.Checked = true;
            cbxTriggerChannel4.Checked = true;
            MainForm.LightManager.SetTriggerExtChannel(0, true);
            MainForm.LightManager.SetTriggerExtChannel(1, true);
            MainForm.LightManager.SetTriggerExtChannel(2, true);
            MainForm.LightManager.SetTriggerExtChannel(3, true);

        }

        String[] PortNo;                                        // 埠編號
        string SelectPort = LightComportName;

        private void COMPort_Check()                            // 串列埠檢知
        {
            string Port_No = comboBoxLightControlPort.Text;

            PortNo = SerialPort.GetPortNames();                 // 取得存在的 COM Port.
            comboBoxLightControlPort.Items.Clear();                           // 清除 ComboBox.Items 的內容.
            if (PortNo.Length >= 1)
            {
                foreach (string port in PortNo)                 // 將找到的現有 COM Port 加入 ComboBox.Items
                {
                    comboBoxLightControlPort.Items.Add(port);
                    if (port == Port_No) comboBoxLightControlPort.Text = Port_No;
                }
                if (comboBoxLightControlPort.Text == "")
                {
                    if (PortNo.Any(s => s.Contains(LightComportName)))
                    {
                        comboBoxLightControlPort.Text = LightComportName;
                    }
                    else
                    {
                        MessageBox.Show("不存在此COM Port,請確認Config檔案");
                        comboBoxLightControlPort.Text = PortNo[0];                // ComboBox.Text 先顯示一個現存的 COM Port.
                    }
                }
            }
            else
            {
                
            }
        }

        private bool ignorePosAndIns = false;
        /// <summary>
        /// 忽略定位與檢測
        /// </summary>
        public bool IgnorePosAndIns // (20181119) Jeff Revised!
        {
            get { return ignorePosAndIns; }
        }
        private void checkBox_IgnorePosAndIns_CheckedChanged(object sender, EventArgs e)
        {
            ignorePosAndIns = checkBox_IgnorePosAndIns.Checked;
        }

        private bool refMainPCRecipe = true;
        /// <summary>
        /// 參考主機的Recipe
        /// </summary>
        public bool RefMainPCRecipe // (20181119) Jeff Revised!
        {
            get { return refMainPCRecipe; }
        }
        private void checkBox_RefMainPCRecipe_CheckedChanged(object sender, EventArgs e)
        {
            refMainPCRecipe = checkBox_RefMainPCRecipe.Checked;
        }
        
        private bool b_savePos = false; // (20181119) Jeff Revised!
        /// <summary>
        /// 儲存原始及結果影像 (定位)
        /// </summary>
        public bool B_savePos // (20181119) Jeff Revised!
        {
            get { return b_savePos; }
        }
        private void checkBox_Pos_CheckedChanged(object sender, EventArgs e)
        {
            b_savePos = checkBox_Pos.Checked;
            if (!b_savePos) // (20190112) Jeff Revised!
                checkBox_Pos_SaveRotImg.Checked = false;
        }

        private bool b_savePos_draw = false; // (20190105) Jeff Revised!
        /// <summary>
        /// 暫存結果影像 (定位)
        /// </summary>
        public bool B_savePos_draw // (20190105) Jeff Revised!
        {
            get { return b_savePos_draw; }
        }
        private void checkBox_Pos_draw_CheckedChanged(object sender, EventArgs e)
        {
            b_savePos_draw = checkBox_Pos_draw.Checked;
        }


        private bool b_savePos_RotImg = false; // (20190112) Jeff Revised!
        /// <summary>
        /// 儲存扭正影像 (定位)
        /// </summary>
        public bool B_savePos_RotImg // (20190112) Jeff Revised!
        {
            get { return b_savePos_RotImg; }
        }
        private void checkBox_Pos_SaveRotImg_CheckedChanged(object sender, EventArgs e) // (20190112) Jeff Revised!
        {
            b_savePos_RotImg = checkBox_Pos_SaveRotImg.Checked;
            if (b_savePos_RotImg)
                checkBox_Pos.Checked = true;
        }


        private bool b_saveLaserPos = false; // (20181119) Jeff Revised!
        /// <summary>
        /// 儲存原始及結果影像 (雷射校正)
        /// </summary>
        public bool B_saveLaserPos // (20181119) Jeff Revised!
        {
            get { return b_saveLaserPos; }
        }
        private void checkBox_LaserPos_CheckedChanged(object sender, EventArgs e)
        {
            b_saveLaserPos = checkBox_LaserPos.Checked;
        }

        private bool b_saveLaserPoss_draw = false; // (20190213) Jeff Revised!
        /// <summary>
        /// 暫存結果影像 (雷射校正)
        /// </summary>
        public bool B_saveLaserPoss_draw // (20190213) Jeff Revised!
        {
            get { return b_saveLaserPoss_draw; }
        }
        private void checkBox_LaserPos_draw_CheckedChanged(object sender, EventArgs e)
        {
            b_saveLaserPoss_draw = checkBox_LaserPos_draw.Checked;
        }

        public bool B_InerCounterIDMode { get; set; } = false; // (20200429) Jeff Revised!
        private void checkBox_InerCounterIDMode_CheckedChanged(object sender, EventArgs e) // (20200429) Jeff Revised!
        {
            this.B_InerCounterIDMode = this.checkBox_InerCounterIDMode.Checked;
            B_SB_InerCountID = this.B_InerCounterIDMode; // 是否啟用內部計數ID模式 (20190424) Jeff Revised!
            if (this.checkBox_InerCounterIDMode.Checked) // (20200429) Jeff Revised!
                this.checkBox_ConstSBID.Checked = false;
        }

        /// <summary>
        /// 固定序號
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_ConstSBID_CheckedChanged(object sender, EventArgs e) // (20200429) Jeff Revised!
        {
            this.textBox_ConstSBID.Enabled = this.checkBox_ConstSBID.Checked;
            if (this.checkBox_ConstSBID.Checked)
            {
                this.checkBox_InerCounterIDMode.Checked = false;
                this.textBox_ConstSBID.Text = SB_ID;
            }
        }

        /// <summary>
        /// 設定固定序號
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_ConstSBID_TextChanged(object sender, EventArgs e) // (20200429) Jeff Revised!
        {
            SB_ID = this.textBox_ConstSBID.Text;
        }

        private bool b_BigMap = false; // 檢視大圖 (20190405) Jeff Revised!
        public bool B_BigMap
        {
            get { return b_BigMap; }
        }
        private void checkBox_BigMap_CheckedChanged(object sender, EventArgs e)
        {
            b_BigMap = checkBox_BigMap.Checked;
            if (!checkBox_BigMap.Checked) // 同時關閉儲存大圖 (20190405) Jeff Revised!
                cbxbSaveTileImg.Checked = false;
        }

        /// <summary>
        /// 設定 checkBox_BigMap 核取狀態
        /// </summary>
        /// <param name="Checked"></param>
        public void Set_checkBox_BigMap(bool Checked) // (20200429) Jeff Revised!
        {
            if (this.checkBox_BigMap.Checked != Checked)
            {
                if (this.checkBox_BigMap.InvokeRequired)
                    this.checkBox_BigMap.BeginInvoke(new Action(() => this.checkBox_BigMap.Checked = Checked));
                else
                    this.checkBox_BigMap.Checked = Checked;
            }
        }

        private void button_ContiGrab_Click(object sender, EventArgs e)
        {
            tabControl_Camera.Enabled = false; // (20190620) Jeff Revised!
            timer_loadCameraParam.Enabled = false; // (20190620) Jeff Revised!
            //Image = null; // (20190128) Jeff Revised!
            if (Image != null) // (20190615) Jeff Revised!
            {
                Image.Dispose();
                Image = null;
            }
            HOperatorSet.ClearWindow(hWindowControl_InputImage.HalconWindow); // (20190128) Jeff Revised!
            ContiGrab_clicked(sender, e);
        }

       

        public HSmartWindowControl GetCamShow2()
        {
            return this.hWindowControl_InputImage;

           
        }

        private bool CameraInitOK { get; set; } = false;

        /// <summary>
        /// 正在連續取像
        /// </summary>
        private bool LiveShow { get; set; } = false;

        public void SetCameraInitOK(bool _CameraInitOK)
        {
            CameraInitOK = _CameraInitOK;
        }
        public void SetLiveShow(bool _liveshowOK)
        {
            LiveShow = _liveshowOK;

            if (LiveShow)
            {                
                button_ContiGrab.Text = "停止取像";
                button_ContiGrab.BackColor = Color.Red;
                button_ContiGrab.ForeColor = Color.White;

                btn_LoadImg.Enabled = false; // (20190128) Jeff Revised!
                btn_connectCamera.Enabled = false; // (20190620) Jeff Revised!
            }
            else
            {                              
                button_ContiGrab.Text = "連續取像";
                button_ContiGrab.BackColor = Color.Transparent;
                button_ContiGrab.ForeColor = Color.Black;

                btn_LoadImg.Enabled = true; // (20190128) Jeff Revised!
                btn_connectCamera.Enabled = true; // (20190620) Jeff Revised!
            }


        }
        public bool GetLiveShow()
        {
            return LiveShow;            
        }
        public void SetContiGrab(bool _set)
        {
            button_ContiGrab.Enabled = _set;
        }

        private void checkBox_ShowCross_CheckedChanged(object sender, EventArgs e) // (20190128) Jeff Revised!
        {
            if (LiveShow)
                DeltaSubstrateInspector.src.Models.CaptureModel.Camera.DisplayCrossMark(checkBox_ShowCross.Checked);
            else
                this.Update_ComputeResol_CrossMark_HDVP(); // (20200717) Jeff Revised!
        }

        private void buttonSaveImageAs_Click(object sender, EventArgs e)
        {
            // 1. Set 
            SaveFileDialog saveFD = new SaveFileDialog();
            saveFD.Filter = "TIF檔|*.tif";
            //saveFD.InitialDirectory = defaultDirName;
            //saveFD.FileName = defaultFileName;
           

            // 2. 開啟對話框
            if (!(saveFD.ShowDialog() == DialogResult.OK)) return;

            // 3, save image
           // string fileName = saveFD.FileName;
            //HOperatorSet.WriteImage(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.get_ho_Image,"tiff", 0, fileName);


            // 3, save image
            string fileName = saveFD.FileName;
            if (LiveShow) // (20190128) Jeff Revised!
            {
                if (!checkBox_ShowCross.Checked) // (20181215) Jeff Revised!
                    HOperatorSet.WriteImage(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.get_ho_Image, "tiff", 0, fileName);
                else
                    HOperatorSet.WriteImage(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.get_CrossImage, "tiff", 0, fileName);
            }
            else
            {
                HOperatorSet.DumpWindow(hWindowControl_InputImage.HalconWindow, "tiff", fileName);
            }
        }

        private void hWindowControl_InputImage_HMouseMove(object sender, HMouseEventArgs e)
        {
            HObject nowImage = null;
            if (LiveShow)
                nowImage = DeltaSubstrateInspector.src.Models.CaptureModel.Camera.get_ho_Image;
            else
                nowImage = Image;

            if (nowImage == null) return;
            HTuple width = null, height = null; 
            HOperatorSet.GetImageSize(nowImage, out width, out height);

            //txt_CursorCoordinate.Text = "(" + String.Format("{0:#.##}", e.X) + ", " + String.Format("{0:#.##}", e.Y) + ")";
            txt_CursorCoordinate.Text = "(" + String.Format("{0:#}", e.X) + ", " + String.Format("{0:#}", e.Y) + ")";
            try
            {
                if (e.X >= 0 && e.X < width && e.Y >= 0 && e.Y < height)
                {
                    HTuple grayval;
                    HOperatorSet.GetGrayval(nowImage, e.Y, e.X, out grayval);
                    //txt_RGBValue.Text = (grayval.TupleInt()).ToString();
                    HOperatorSet.GetGrayval(nowImage, e.Y, e.X, out grayval);
                    txt_GrayValue.Text = (grayval.TupleInt()).ToString();
                }
                else
                {
                    //txt_RGBValue.Text = "";
                    txt_GrayValue.Text = "";
                }
            }
            catch
            { }
        }

        private void hWindowControl_InputImage_Load(object sender, EventArgs e)
        {

        }

        private void hWindowControl_InputImage_HMouseDoubleClick(object sender, HMouseEventArgs e)
        {

        }

        #region 【計算解析度】 & 【載入HDVP】

        private HObject Image = null; // (20190128) Jeff Revised!

        /// <summary>
        /// 【載入影像】(【計算解析度】 & 【載入HDVP】)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_LoadImg_Click(object sender, EventArgs e) // (20200717) Jeff Revised!
        {
            OpenFileDialog OpenImgDilg = new OpenFileDialog();
            OpenImgDilg.Filter = "(*.tif)|*.tif|(*.bmp)|*.bmp|(*.jpg)|*.jpg|(*.png)|*.png";

            if (OpenImgDilg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;
            Extension.HObjectMedthods.ReleaseHObject(ref this.Image);
            HOperatorSet.ReadImage(out this.Image, OpenImgDilg.FileName);
            this.Update_ComputeResol_CrossMark_HDVP(); // (20200717) Jeff Revised!
            HOperatorSet.SetPart(hWindowControl_InputImage.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part
        }

        /// <summary>
        /// 【啟用】計算解析度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_ComputeResol_CheckedChanged(object sender, EventArgs e) // (20200717) Jeff Revised!
        {
            this.Update_ComputeResol_CrossMark_HDVP(); // (20200717) Jeff Revised!
        }

        /// <summary>
        /// 【最大圓】&【最小圓】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_MaxMinCircle_CheckedChanged(object sender, EventArgs e) // (20190128) Jeff Revised!
        {
            RadioButton rbtn = sender as RadioButton;
            if (rbtn.Checked == false) return;

            this.Update_ComputeResol_CrossMark_HDVP(); // (20200717) Jeff Revised!
        }

        /// <summary>
        /// 【直徑 (mm)】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nud_RealDiameter_mm_ValueChanged(object sender, EventArgs e) // (20190128) Jeff Revised!
        {
            this.Update_ComputeResol_CrossMark_HDVP(); // (20200717) Jeff Revised!
        }

        private Class_load_hdvp class_load_hdvp = new Class_load_hdvp();
        /// <summary>
        /// 【載入HDVP】ON/OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbx_LoadHDVP_CheckedChanged(object sender, EventArgs e) // (20200717) Jeff Revised!
        {
            CheckBox cbx = sender as CheckBox;
            if (cbx.Checked) // ON
            {
                cbx.BackgroundImage = global::DeltaSubstrateInspector.Properties.Resources.ON;
                this.lbl_LoadHDVP.Text = "ON";
                this.lbl_LoadHDVP.ForeColor = Color.DeepSkyBlue;
                this.panel_LoadHDVP.Enabled = true;
            }
            else // OFF
            {
                cbx.BackgroundImage = global::DeltaSubstrateInspector.Properties.Resources.OFF_edited;
                this.lbl_LoadHDVP.Text = "OFF";
                this.lbl_LoadHDVP.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
                this.panel_LoadHDVP.Enabled = false;
            }
            this.Update_ComputeResol_CrossMark_HDVP();
        }

        /// <summary>
        /// 【載入HDVP】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_LoadHDVP_Click(object sender, EventArgs e) // (20200717) Jeff Revised!
        {
            this.class_load_hdvp.Load_Procedure();
            this.Update_ComputeResol_CrossMark_HDVP();
        }

        /// <summary>
        /// 更新 開啟十字、計算解析度、載入HDVP
        /// </summary>
        private void Update_ComputeResol_CrossMark_HDVP() // (20200717) Jeff Revised!
        {
            // 計算解析度
            DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Update_ComputeResol(this.checkBox_ComputeResol.Checked, double.Parse(this.nud_RealDiameter_mm.Value.ToString()), this.radioButton_MaxCircle.Checked);

            // 載入HDVP
            DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Update_LoadHDVP(this.cbx_LoadHDVP.Checked, this.class_load_hdvp);

            if (this.LiveShow)
            { }
            else
            {
                if (this.Image != null)
                {
                    HOperatorSet.ClearWindow(hWindowControl_InputImage.HalconWindow);
                    HOperatorSet.DispObj(this.Image, hWindowControl_InputImage.HalconWindow);

                    // 計算解析度
                    if (this.checkBox_ComputeResol.Checked)
                        DeltaSubstrateInspector.src.Models.CaptureModel.Camera.ComputeResol(this.Image, hWindowControl_InputImage, double.Parse(this.nud_RealDiameter_mm.Value.ToString()), this.radioButton_MaxCircle.Checked);

                    // 開啟十字
                    if (this.checkBox_ShowCross.Checked)
                    {
                        HObject CrossRegion;
                        DeltaSubstrateInspector.src.Models.CaptureModel.Camera.GenCrossMark(this.Image, out CrossRegion);
                        HOperatorSet.SetColor(hWindowControl_InputImage.HalconWindow, "red");
                        HOperatorSet.DispObj(CrossRegion, hWindowControl_InputImage.HalconWindow);
                        CrossRegion.Dispose();
                    }

                    // 載入HDVP
                    if (this.cbx_LoadHDVP.Checked)
                        this.class_load_hdvp.Execute_Disp(this.hWindowControl_InputImage, this.Image);
                }
            }
        }

        #endregion

        private bool b_SaveTileImg = false; // 儲存大圖
        public bool get_SaveTileImg
        {
            get { return b_SaveTileImg; }
        }
        private void cbxbSaveTileImg_CheckedChanged(object sender, EventArgs e)
        {
            b_SaveTileImg = cbxbSaveTileImg.Checked;
            if (cbxbSaveTileImg.Checked) // 同時啟用檢視大圖 (20190405) Jeff Revised!
                checkBox_BigMap.Checked = true;
        }



        #region 相機模組


        private double W_pictureBox_image, H_pictureBox_image; // pictureBox Size
        private double WidthMax, HeightMax; // 影像最大Size
        private double scale_W, scale_H; // scale

        private void param_Camera_ValueChanged(object sender, EventArgs e) // (20190620) Jeff Revised!
        {
            if (b_loadCameraParam || blnDraw) return;

            try
            {
                Control c = sender as Control;
                string tag = c.Tag.ToString();
                #region 更新相機參數
                switch (tag)
                {
                    #region Settings
                    case "AcquisitionFrameRateAuto":
                        {
                            HOperatorSet.SetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "AcquisitionFrameRateAuto", c.Text);
                        }
                        break;
                    case "AcquisitionFrameRate":
                        {
                            HOperatorSet.SetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "AcquisitionFrameRate", int.Parse(((NumericUpDown)c).Value.ToString()));
                        }
                        break;

                    case "ExposureMode":
                        {
                            HOperatorSet.SetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "ExposureMode", c.Text);
                        }
                        break;
                    case "ExposureAuto":
                        {
                            //HOperatorSet.SetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "ExposureAuto", c.Text.ToString());
                            HOperatorSet.SetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "ExposureAuto", c.Text);
                        }
                        break;
                    case "ExposureTime":
                        {
                            if (b_ExposureAuto_conti)
                                return;

                            HOperatorSet.SetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "ExposureTime", int.Parse(((NumericUpDown)c).Value.ToString()));
                        }
                        break;

                    case "GainAuto":
                        {
                            HOperatorSet.SetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "GainAuto", c.Text);
                        }
                        break;
                    case "Gain":
                        {
                            if (b_GainAuto_conti)
                                return;

                            HOperatorSet.SetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "Gain", int.Parse(((NumericUpDown)c).Value.ToString()));
                        }
                        break;

                    case "Gamma":
                        {
                            HOperatorSet.SetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "Gamma", double.Parse(((NumericUpDown)c).Value.ToString()));
                        }
                        break;
                    case "BlackLevel":
                        {
                            HOperatorSet.SetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "BlackLevel", double.Parse(((NumericUpDown)c).Value.ToString()));
                        }
                        break;

                    /* Color Camera */
                    case "BalanceRatioSelector":
                        {
                            HOperatorSet.SetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "BalanceRatioSelector", c.Text);
                        }
                        break;
                    case "BalanceRatio":
                        {
                            if (b_BalanceWhiteAuto_notOff)
                                return;

                            HOperatorSet.SetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "BalanceRatio", double.Parse(((NumericUpDown)c).Value.ToString()));
                        }
                        break;
                    case "BalanceWhiteAuto":
                        {
                            HOperatorSet.SetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "BalanceWhiteAuto", c.Text);
                        }
                        break;
                    #endregion

                    #region Image Format
                    case "Width":
                        {
                            HOperatorSet.SetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "Width", int.Parse(((NumericUpDown)c).Value.ToString()));
                            // 更新文字資訊
                            update_label_image();
                        }
                        break;
                    case "Height":
                        {
                            HOperatorSet.SetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "Height", int.Parse(((NumericUpDown)c).Value.ToString()));
                            // 更新文字資訊
                            update_label_image();
                        }
                        break;
                    case "OffsetX":
                        {
                            HOperatorSet.SetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "OffsetX", int.Parse(((NumericUpDown)c).Value.ToString()));
                            // 更新文字資訊
                            update_label_image();
                        }
                        break;
                    case "OffsetY":
                        {
                            HOperatorSet.SetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "OffsetY", int.Parse(((NumericUpDown)c).Value.ToString()));
                            // 更新文字資訊
                            update_label_image();
                        }
                        break;

                    // Button
                    case "Max Image Size":
                        {
                            HTuple V_WidthMax, V_HeightMax;
                            HOperatorSet.GetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "WidthMax", out V_WidthMax);
                            HOperatorSet.GetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "HeightMax", out V_HeightMax);

                            // 順序很重要，先設定Offset都是0，再設定長寬，才不會判斷超出影像最大邊界，而被裁切掉!
                            HOperatorSet.SetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "OffsetX", 0);
                            HOperatorSet.SetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "OffsetY", 0);
                            HOperatorSet.SetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "Width", V_WidthMax);
                            HOperatorSet.SetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "Height", V_HeightMax);
                        }
                        break;
                    case "Center ROI":
                        {
                            HTuple V_WidthMax, V_HeightMax, V_Width, V_Height;
                            HOperatorSet.GetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "WidthMax", out V_WidthMax);
                            HOperatorSet.GetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "HeightMax", out V_HeightMax);
                            HOperatorSet.GetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "Width", out V_Width);
                            HOperatorSet.GetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "Height", out V_Height);

                            //HOperatorSet.SetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "OffsetX", (int)((V_WidthMax - V_Width) / 2.0 + 0.5));
                            //HOperatorSet.SetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "OffsetY", (int)((V_HeightMax - V_Height) / 2.0 + 0.5));
                            HOperatorSet.SetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "OffsetX", (V_WidthMax - V_Width) / 2);
                            HOperatorSet.SetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "OffsetY", (V_HeightMax - V_Height) / 2);
                        }
                        break;
                    #endregion

                    #region User Set Control
                    case "UserSetSelector":
                        {
                            HOperatorSet.SetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "UserSetSelector", c.Text);
                        }
                        break;
                    // Button
                    case "UserSetLoad":
                        {
                            HOperatorSet.SetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "UserSetLoad", "true");
                        }
                        break;
                    case "UserSetSave":
                        {
                            SystemSounds.Exclamation.Play();
                            DialogResult dr = MessageBox.Show("確定儲存相機參數?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dr == DialogResult.Yes)
                            {
                                try
                                {
                                    HOperatorSet.SetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "UserSetSave", combx_UserSetSelector.Text);
                                    MessageBox.Show("儲存成功!");
                                }
                                catch
                                {
                                    SystemSounds.Exclamation.Play();
                                    MessageBox.Show("儲存失敗...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                        }
                        break;
                    #endregion
                }
                #endregion
            }
            catch (Exception ex)
            {
                blnDraw = false;
                SystemSounds.Exclamation.Play();
                MessageBox.Show("更新相機參數失敗", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // 更新顯示參數
            if (!blnDraw)
                btn_loadCameraParam_Click(sender, e);
        }

        private bool b_loadCameraParam = false; // (20190615) Jeff Revised!
        private void btn_loadCameraParam_Click(object sender, EventArgs e) // (20190620) Jeff Revised!
        {
            if (DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle == null)
            {
                SystemSounds.Exclamation.Play();
                MessageBox.Show("尚未連接至相機", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            b_loadCameraParam = true;

            try
            {
                #region 取得目前相機參數並且更新UI顯示


                HTuple Param;

                #region Settings
                // Frame Rate Auto:
                try
                {
                    HOperatorSet.GetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "AcquisitionFrameRateAuto", out Param);
                    combx_FrameRateAuto.Text = Param.S;
                    if (combx_FrameRateAuto.Text == "Continuous")
                        nud_AcquisitionFrameRate.Enabled = false;
                    else
                        nud_AcquisitionFrameRate.Enabled = true;
                    combx_FrameRateAuto.Enabled = true;
                }
                catch
                {
                    combx_FrameRateAuto.Enabled = false;
                    nud_AcquisitionFrameRate.Enabled = true;
                }

                // AcquisitionFrameRate:
                HOperatorSet.GetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "AcquisitionFrameRate", out Param);
                nud_AcquisitionFrameRate.Value = (int)(Param.D);

                // Exposure Mode:
                try
                {
                    HOperatorSet.GetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "ExposureMode", out Param);
                    combx_ExposureMode.Text = Param.S;
                    if (combx_ExposureMode.Text == "TriggerWidth")
                    {
                        combx_ExposureAuto.Enabled = false;
                        nud_ExposureTime.Enabled = false;
                    }
                    else
                    {
                        combx_ExposureAuto.Enabled = true;
                        nud_ExposureTime.Enabled = true;
                    }
                    combx_ExposureMode.Enabled = true;
                }
                catch
                {
                    combx_ExposureMode.Enabled = false;
                }

                // Exposure Auto:
                HOperatorSet.GetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "ExposureAuto", out Param);
                combx_ExposureAuto.Text = Param.S;
                if (combx_ExposureAuto.Text == "Continuous")
                {
                    b_ExposureAuto_conti = true;
                    nud_ExposureTime.Enabled = false;
                    //timer_loadCameraParam.Enabled = true;
                }
                else
                {
                    b_ExposureAuto_conti = false;
                    if (combx_ExposureMode.Text != "TriggerWidth" || combx_ExposureMode.Enabled == false)
                        nud_ExposureTime.Enabled = true;
                    //timer_loadCameraParam.Enabled = false;
                }

                // Exposure Time:
                HOperatorSet.GetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "ExposureTime", out Param);
                nud_ExposureTime.Value = (int)(Param.D);
                
                // Gain Auto:
                HOperatorSet.GetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "GainAuto", out Param);
                combx_GainAuto.Text = Param.S;
                if (combx_GainAuto.Text == "Continuous")
                {
                    b_GainAuto_conti = true;
                    nud_Gain.Enabled = false;
                    //timer_loadCameraParam.Enabled = true;
                }
                else
                {
                    b_GainAuto_conti = false;
                    nud_Gain.Enabled = true;
                    //timer_loadCameraParam.Enabled = false;
                }
                
                // Gain:
                HOperatorSet.GetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "Gain", out Param);
                nud_Gain.Value = (int)(Param.D);

                // Gamma:
                try // (20200717) Jeff Revised!
                {
                    HOperatorSet.GetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "Gamma", out Param);
                    nud_Gamma.Value = (decimal)(Param.D);
                    nud_Gamma.Enabled = true;
                }
                catch
                {
                    nud_Gamma.Enabled = false;
                }

                // BlackLevel:
                HOperatorSet.GetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "BlackLevel", out Param);
                nud_BlackLevel.Value = (decimal)(Param.D);

                /* Color Camera */
                if (tabControl_Camera.Enabled == false)
                {
                    combx_BalanceRatioSelector.Enabled = true;
                    nud_BalanceRatio.Enabled = true;
                    combx_BalanceWhiteAuto.Enabled = true;
                }
                try
                {
                    // Balance Ratio Selector:
                    HOperatorSet.GetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "BalanceRatioSelector", out Param);
                    combx_BalanceRatioSelector.Text = Param.S;

                    // Balance White Auto:
                    HOperatorSet.GetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "BalanceWhiteAuto", out Param);
                    combx_BalanceWhiteAuto.Text = Param.S;
                    if (combx_BalanceWhiteAuto.Text != "Off")
                    {
                        b_BalanceWhiteAuto_notOff = true;
                        nud_BalanceRatio.Enabled = false;
                        //timer_loadCameraParam.Enabled = true;
                    }
                    else
                    {
                        b_BalanceWhiteAuto_notOff = false;
                        nud_BalanceRatio.Enabled = true;
                        //timer_loadCameraParam.Enabled = false;
                    }

                    // Balance Ratio:
                    HOperatorSet.GetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "BalanceRatio", out Param);
                    nud_BalanceRatio.Value = (decimal)(Param.D);
                }
                catch // Not Color Camera
                {
                    b_BalanceWhiteAuto_notOff = false;
                    if (tabControl_Camera.Enabled == false)
                    {
                        combx_BalanceRatioSelector.Enabled = false;
                        nud_BalanceRatio.Enabled = false;
                        combx_BalanceWhiteAuto.Enabled = false;
                    }
                }
                #endregion

                // Timer
                if (b_ExposureAuto_conti || b_GainAuto_conti || b_BalanceWhiteAuto_notOff)
                    timer_loadCameraParam.Enabled = true;
                else
                    timer_loadCameraParam.Enabled = false;

                #region Image Format
                // Width:
                HOperatorSet.GetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "Width", out Param);
                nud_Width.Value = (int)(Param.D);
                // Height:
                HOperatorSet.GetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "Height", out Param);
                nud_Height.Value = (int)(Param.D);
                // Offset X:
                HOperatorSet.GetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "OffsetX", out Param);
                nud_OffsetX.Value = (int)(Param.D);
                // Offset Y:
                HOperatorSet.GetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "OffsetY", out Param);
                nud_OffsetY.Value = (int)(Param.D);
                #endregion

                #region User Set Control
                // Current User Set:
                try
                {
                    HOperatorSet.GetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "UserSetCurrent", out Param);
                    textBox_UserSetCurrent.Text = Param.ToString();
                    textBox_UserSetCurrent.Enabled = true;
                }
                catch
                {
                    textBox_UserSetCurrent.Text = "";
                    textBox_UserSetCurrent.Enabled = false;
                }
                
                // User Set Selector:
                HOperatorSet.GetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "UserSetSelector", out Param);
                combx_UserSetSelector.Text = Param.S;
                #endregion


                #endregion

                if (tabControl_Camera.Enabled == false)
                {
                    #region 參數初始化
                    // pictureBox Size
                    W_pictureBox_image = pictureBox_image.Width;
                    H_pictureBox_image = pictureBox_image.Height;
                    // 影像最大Size
                    HTuple V_WidthMax, V_HeightMax;
                    HOperatorSet.GetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "WidthMax", out V_WidthMax);
                    HOperatorSet.GetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "HeightMax", out V_HeightMax);
                    WidthMax = V_WidthMax.D;
                    HeightMax = V_HeightMax.D;
                    nud_Width.Maximum = (int)WidthMax;
                    nud_Height.Maximum = (int)HeightMax;
                    // scale
                    scale_W = W_pictureBox_image / WidthMax;
                    scale_H = H_pictureBox_image / HeightMax;
                    #endregion

                    tabControl_Camera.Enabled = true;
                    SystemSounds.Exclamation.Play();
                    MessageBox.Show("成功載入目前相機參數! \n 請注意: 調整參數後，一旦關閉相機，下次開啟相機時之參數為上一次關閉相機前之參數~", "溫馨提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                // 更新文字資訊
                update_label_image();

                // 更新 pictureBox_image
                rect.X = (int)(double.Parse(nud_OffsetX.Value.ToString()) * scale_W + 0.5);
                rect.Y = (int)(double.Parse(nud_OffsetY.Value.ToString()) * scale_H + 0.5);
                rect.Width = (int)(double.Parse(nud_Width.Value.ToString()) * scale_W + 0.5);
                rect.Height = (int)(double.Parse(nud_Height.Value.ToString()) * scale_H + 0.5);

                pictureBox_image.Invalidate();
            }
            catch
            {
                tabControl_Camera.Enabled = false;
                blnDraw = false;
                SystemSounds.Exclamation.Play();
                MessageBox.Show("載入目前相機參數失敗", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            b_loadCameraParam = false;
        }
        
        /// <summary>
        /// Exposure Auto 是否為 "Continuous" mode
        /// </summary>
        private bool b_ExposureAuto_conti = false; // (20190620) Jeff Revised!
        /// <summary>
        /// Gain Auto 是否為 "Continuous" mode
        /// </summary>
        private bool b_GainAuto_conti = false; // (20190620) Jeff Revised!
        /// <summary>
        /// Balance White Auto 是否不為 "Off" (i.e. 為 "Once" 或 "Continuous")
        /// </summary>
        private bool b_BalanceWhiteAuto_notOff = false; // (20190620) Jeff Revised!
        /// <summary>
        /// Exposure Auto 或 Gain Auto 在 "Continuous" mode時，需要動態載入其數值 (動態更新顯示)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_loadCameraParam_Tick(object sender, EventArgs e)
        {
            try
            {
                #region 取得目前相機參數
                HTuple Param;

                // Exposure Time:
                if (b_ExposureAuto_conti)
                {
                    HOperatorSet.GetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "ExposureTime", out Param);
                    nud_ExposureTime.Value = (int)(Param.D);
                }

                // Gain:
                if (b_GainAuto_conti)
                {
                    HOperatorSet.GetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "Gain", out Param);
                    nud_Gain.Value = (int)(Param.D);
                }

                // Balance White Auto & Balance Ratio
                if (b_BalanceWhiteAuto_notOff)
                {
                    // Balance White Auto:
                    if (combx_BalanceWhiteAuto.Text == "Once")
                    {
                        HOperatorSet.GetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "BalanceWhiteAuto", out Param);
                        if (Param.S == "Off")
                        {
                            combx_BalanceWhiteAuto.Text = "Off";
                            b_BalanceWhiteAuto_notOff = false;
                            nud_BalanceRatio.Enabled = true;
                        }

                        // Timer
                        if (b_ExposureAuto_conti || b_GainAuto_conti || b_BalanceWhiteAuto_notOff)
                            timer_loadCameraParam.Enabled = true;
                        else
                            timer_loadCameraParam.Enabled = false;
                    }

                    // Balance Ratio:
                    HOperatorSet.GetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "BalanceRatio", out Param);
                    nud_BalanceRatio.Value = (decimal)(Param.D);
                }
                #endregion
            }
            catch
            {
                //SystemSounds.Exclamation.Play();
                //MessageBox.Show("載入目前相機參數失敗", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool b_connected = false; // (20190615) Jeff Revised!
        public bool Get_b_connected()
        {
            return b_connected;
        }
        public void Set_connectCamera(bool _b_connected)
        {
            b_connected = _b_connected;

            if (b_connected)
            {
                btn_connectCamera.Text = "關閉相機";
                btn_connectCamera.BackColor = Color.Red;
                btn_connectCamera.ForeColor = Color.White;

                //btn_LoadImg.Enabled = false;
                gbx_ComputeResol.Enabled = false;
                button_ContiGrab.Enabled = false;

                // 相機模組元件設定
                btn_UserSetLoad.Enabled = true;
                btn_UserSetSave.Enabled = true;
                btn_MaxImageSize.Enabled = true;
                nud_Width.Enabled = true;
                nud_Height.Enabled = true;
                pictureBox_image.Enabled = true;
            }
            else
            {
                btn_connectCamera.Text = "連接相機";
                btn_connectCamera.BackColor = Color.Transparent;
                btn_connectCamera.ForeColor = Color.Black;

                //btn_LoadImg.Enabled = true;
                gbx_ComputeResol.Enabled = true;
                button_ContiGrab.Enabled = true;

                // 相機模組元件設定
                btn_UserSetLoad.Enabled = false;
                btn_UserSetSave.Enabled = false;
                btn_MaxImageSize.Enabled = false;
                nud_Width.Enabled = false;
                nud_Height.Enabled = false;
                pictureBox_image.Enabled = false;
            }
        }
        private void btn_connectCamera_Click(object sender, EventArgs e) // (20190620) Jeff Revised!
        {
            tabControl_Camera.Enabled = false;
            timer_loadCameraParam.Enabled = false;
            gbx_ComputeResol.Enabled = false;
            if (Image != null)
            {
                Image.Dispose();
                Image = null;
            }
            HOperatorSet.ClearWindow(hWindowControl_InputImage.HalconWindow);
            connectCamera_clicked(sender, e);
        }


        #region pictureBox_image (20190620) Jeff Revised!
        
        private Point start; // 畫框的起始點
        private Point end; // 畫框的結束點
        private bool blnDraw = false; // 判斷是否繪製
        private Rectangle rect;

        private void pictureBox_image_MouseDown(object sender, MouseEventArgs e)
        {
            start = e.Location;
            rect.Location = new Point(start.X, start.Y);
            //pictureBox_image.Invalidate();
            blnDraw = true;

            // 更新 影像最大Size
            HTuple V_WidthMax, V_HeightMax;
            HOperatorSet.GetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "WidthMax", out V_WidthMax);
            HOperatorSet.GetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "HeightMax", out V_HeightMax);
            WidthMax = V_WidthMax.D;
            HeightMax = V_HeightMax.D;
            nud_Width.Maximum = (int)WidthMax;
            nud_Height.Maximum = (int)HeightMax;

            // 更新顯示參數
            //nud_OffsetX.Value = (int)(start.X / scale_W + 0.5);
            //nud_OffsetY.Value = (int)(start.Y / scale_H + 0.5);
        }

        private void pictureBox_image_MouseMove(object sender, MouseEventArgs e)
        {
            if (blnDraw)
            {
                if (e.Button != MouseButtons.Left) // 判斷是否按下左鍵
                    return;

                /* 記錄框的位置和大小 */
                Point tempEndPoint = e.Location;
                // 限制框範圍
                if (tempEndPoint.X < 0)
                    tempEndPoint.X = 0;
                else if (tempEndPoint.X > (int)W_pictureBox_image)
                    tempEndPoint.X = (int)W_pictureBox_image;

                if (tempEndPoint.Y < 0)
                    tempEndPoint.Y = 0;
                else if (tempEndPoint.Y > (int)H_pictureBox_image)
                    tempEndPoint.Y = (int)H_pictureBox_image;

                tempEndPoint.Y = (tempEndPoint.Y >= 0) ? tempEndPoint.Y : 0;

                rect.Location = new Point(Math.Min(start.X, tempEndPoint.X), Math.Min(start.Y, tempEndPoint.Y));
                rect.Size = new Size(Math.Abs(start.X - tempEndPoint.X), Math.Abs(start.Y - tempEndPoint.Y));

                pictureBox_image.Invalidate();

                /* 更新顯示參數 */
                int OffsetX = (int)(rect.X / scale_W + 0.5);
                nud_OffsetX.Value = (OffsetX > nud_OffsetX.Minimum) ? OffsetX : nud_OffsetX.Minimum;

                int OffsetY = (int)(rect.Y / scale_H + 0.5);
                nud_OffsetY.Value = (OffsetY > nud_OffsetY.Minimum) ? OffsetY : nud_OffsetY.Minimum;

                int Width = (int)(rect.Width / scale_W + 0.5);
                if (Width < nud_Width.Maximum)
                    nud_Width.Value = (Width > nud_Width.Minimum) ? Width : nud_Width.Minimum;
                else
                    nud_Width.Value = nud_Width.Maximum;

                int Height = (int)(rect.Height / scale_H + 0.5);
                if (Height < nud_Height.Maximum)
                    nud_Height.Value = (Height > nud_Height.Minimum) ? Height : nud_Height.Minimum;
                else
                    nud_Height.Value = nud_Height.Maximum;

                // 更新文字資訊
                update_label_image();
            }
        }

        // 190625, andy: remote motor
        private bool b_RemoteMotor = false;
        public bool get_RemoteMotor
        {
            get { return b_RemoteMotor; }
        }
        private void checkBox_RemoteMotor_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_RemoteMotor.Checked == true)
            {
                if (MessageBox.Show("您是否確定要進行點圖遙控功能(需注意平台移動)？", "Warning!", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    checkBox_RemoteMotor.Checked = false;
                }
            }

            b_RemoteMotor = checkBox_RemoteMotor.Checked;

        }

        private void comboBoxMaxCurrent1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox Obj = (ComboBox)sender;

            MainForm.LightManager.SetMaxLightValueChannel(0, Obj.SelectedIndex);

            Modules.LightModule.LightMaxValueIndex.Light01_MaxVauleIndex = Obj.SelectedIndex;

            int Ch1Max, Ch2Max, Ch3Max, Ch4Max;

            GetMaxCurrent(out Ch1Max, out Ch2Max, out Ch3Max, out Ch4Max);

            sidCh1Power.Maximum = Ch1Max;
            nudCh1Power.Maximum = Ch1Max;

            sidCh1Power.Value = 0;
            sidCh1Power.Refresh();

            nudCh1Power.Value = decimal.Parse("0");
        }

        private void nudCh1Power_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;
            int value = int.Parse(Obj.Value.ToString());
            MainForm.LightManager.SetLightValueChannel(0, value);

            Modules.LightModule.LightValue.Light01_Vaule = int.Parse(Obj.Value.ToString());
        }

        private void nudCh2Power_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;
            int value = int.Parse(Obj.Value.ToString());
            MainForm.LightManager.SetLightValueChannel(1, value);

            Modules.LightModule.LightValue.Light02_Vaule = int.Parse(Obj.Value.ToString());
        }

        private void nudCh3Power_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;
            int value = int.Parse(Obj.Value.ToString());
            MainForm.LightManager.SetLightValueChannel(2, value);

            Modules.LightModule.LightValue.Light03_Vaule = int.Parse(Obj.Value.ToString());
        }

        private void nudCh4Power_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Obj = (NumericUpDown)sender;
            int value = int.Parse(Obj.Value.ToString());
            MainForm.LightManager.SetLightValueChannel(3, value);

            Modules.LightModule.LightValue.Light04_Vaule = int.Parse(Obj.Value.ToString());
        }

        private void comboBoxMaxCurrent2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox Obj = (ComboBox)sender;

            MainForm.LightManager.SetMaxLightValueChannel(1, Obj.SelectedIndex);

            Modules.LightModule.LightMaxValueIndex.Light02_MaxVauleIndex = Obj.SelectedIndex;

            int Ch1Max, Ch2Max, Ch3Max, Ch4Max;

            GetMaxCurrent(out Ch1Max, out Ch2Max, out Ch3Max, out Ch4Max);

            sidCh2Power.Maximum = Ch2Max;
            nudCh2Power.Maximum = Ch2Max;

            sidCh2Power.Value = 0;
            sidCh2Power.Refresh();

            nudCh2Power.Value = decimal.Parse("0");
        }

        private void comboBoxMaxCurrent3_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox Obj = (ComboBox)sender;

            MainForm.LightManager.SetMaxLightValueChannel(2, Obj.SelectedIndex);

            Modules.LightModule.LightMaxValueIndex.Light03_MaxVauleIndex = Obj.SelectedIndex;

            int Ch1Max, Ch2Max, Ch3Max, Ch4Max;

            GetMaxCurrent(out Ch1Max, out Ch2Max, out Ch3Max, out Ch4Max);

            sidCh3Power.Maximum = Ch3Max;
            nudCh3Power.Maximum = Ch3Max;

            sidCh3Power.Value = 0;
            sidCh3Power.Refresh();

            nudCh3Power.Value = decimal.Parse("0");
        }

        private void comboBoxMaxCurrent4_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox Obj = (ComboBox)sender;

            MainForm.LightManager.SetMaxLightValueChannel(3, Obj.SelectedIndex);

            Modules.LightModule.LightMaxValueIndex.Light04_MaxVauleIndex = Obj.SelectedIndex;

            int Ch1Max, Ch2Max, Ch3Max, Ch4Max;

            GetMaxCurrent(out Ch1Max, out Ch2Max, out Ch3Max, out Ch4Max);

            sidCh4Power.Maximum = Ch4Max;
            nudCh4Power.Maximum = Ch4Max;

            sidCh4Power.Value = 0;
            sidCh4Power.Refresh();

            nudCh4Power.Value = decimal.Parse("0");
        }

        private void sidCh1Power_Scroll(object sender, ScrollEventArgs e)
        {
            int Power = sidCh1Power.Value;

            nudCh1Power.Value = decimal.Parse(Power.ToString());
        }

        private void sidCh2Power_Scroll(object sender, ScrollEventArgs e)
        {
            int Power = sidCh2Power.Value;

            nudCh2Power.Value = decimal.Parse(Power.ToString());
        }

        private void sidCh3Power_Scroll(object sender, ScrollEventArgs e)
        {
            int Power = sidCh3Power.Value;

            nudCh3Power.Value = decimal.Parse(Power.ToString());
        }

        private void sidCh4Power_Scroll(object sender, ScrollEventArgs e)
        {
            int Power = sidCh4Power.Value;

            nudCh4Power.Value = decimal.Parse(Power.ToString());
        }

        private void cbxTriggerChannel1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox Obj = (CheckBox)sender;
            MainForm.LightManager.SetTriggerExtChannel(0, Obj.Checked);
        }

        private void cbxTriggerChannel2_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox Obj = (CheckBox)sender;
            MainForm.LightManager.SetTriggerExtChannel(1, Obj.Checked);
        }

        private void cbxTriggerChannel3_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox Obj = (CheckBox)sender;
            MainForm.LightManager.SetTriggerExtChannel(2, Obj.Checked);
        }

        private void cbxTriggerChannel4_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox Obj = (CheckBox)sender;
            MainForm.LightManager.SetTriggerExtChannel(3, Obj.Checked);
        }

        public bool UpdateConnectedControl()
        {
            if (MainForm.LightManager.IsConnect())
            {
                gbxLightControl.Enabled = true;
                btnLightControlConnected.Text = "Disconnect";
                return true;
            }
            else
            {
                gbxLightControl.Enabled = false;
                btnLightControlConnected.Text = "Connect";
                return false;
            }
        }

        private async void btnLightControlConnected_Click(object sender, EventArgs e)
        {
            MainForm.LightManager.Connect(SelectPort);

            await Task.Delay(1500);

            UpdateConnectedControl();
        }
        
        private void pictureBox_image_MouseUp(object sender, MouseEventArgs e)
        {
            blnDraw = false; // 結束繪製
            // 更新參數: 先Offset回0，以免因設定順序關係導致超出影像邊界，產生設定失敗!
            HOperatorSet.SetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "OffsetX", 0);
            HOperatorSet.SetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "OffsetY", 0);
            HOperatorSet.SetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "Width", int.Parse((nud_Width).Value.ToString()));
            HOperatorSet.SetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "Height", int.Parse((nud_Height).Value.ToString()));
            HOperatorSet.SetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "OffsetX", int.Parse((nud_OffsetX).Value.ToString()));
            HOperatorSet.SetFramegrabberParam(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.Hv_AcqHandle, "OffsetY", int.Parse((nud_OffsetY).Value.ToString()));
            
            btn_loadCameraParam_Click(sender, e);
        }

        private void pictureBox_image_Paint(object sender, PaintEventArgs e)
        {
            if (rect != null && rect.Width > 0 && rect.Height > 0)
            {
                e.Graphics.DrawRectangle(new Pen(Color.Red, 3), rect); // 重新繪製顏色為紅色
            }
        }

        /// <summary>
        /// 更新文字資訊 (Image Format)
        /// </summary>
        private void update_label_image()
        {
            label_image_1.Text = "Start: (" + nud_OffsetX.Value.ToString() + "," + nud_OffsetY.Value.ToString() +
                                 ") End: (" + (nud_OffsetX.Value + nud_Width.Value).ToString() + "," + (nud_OffsetY.Value + nud_Height.Value).ToString() + ")";
            label_image_2.Text = "Dimensions: " + nud_Width.Value.ToString() + " x " + nud_Height.Value.ToString();
        }

        #endregion


        #endregion
    }
}
