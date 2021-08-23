using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StyleTabTable;
using DeltaSubstrateInspector.src.MainPanels;
using DeltaSubstrateInspector.src.MainSetting;
using DeltaSubstrateInspector.src.Modules.InspectModule;
using DeltaSubstrateInspector.src.Modules.CaptureModule;
using DeltaSubstrateInspector.src.Modules.MotionModule;
using DeltaSubstrateInspector.src.Modules.LightModule;
using HalconDotNet;
using Extension;
using DeltaSubstrateInspector.src.Modules;
using DeltaSubstrateInspector.src.Modules.CaptureModule;
using System.Threading;
using DeltaSubstrateInspector.FileSystem;
using DeltaSubstrateInspector.src.Modules.ResultModule;
using System.Configuration;
using System.Collections.Specialized;
using static DeltaSubstrateInspector.FileSystem.FileSystem;
using DeltaSubstrateInspector.src.SystemSource.WorkSheet;
using System.Collections;
using DeltaSubstrateInspector.src.SystemSource.Recipe;
using DeltaSubstrateInspector.src.SystemSource;

using IOModule;  // 181016 andy
using PM_24084R; // 181210 andy
using DeltaSubstrateInspector.src.Modules.TCPIP4Sorter;  // 181016 andy
using DeltaSubstrateInspector.src.Modules.SystemModel; // 181016 andy
using DelVivi.Log; // 181016 andy
using DelVivi.Common; // 181121 andy
using DSI_Reviewer;
using PasswordManagement;

using DeltaSubstrateInspector.src.Modules.StorageModule;
using System.IO;
using clsLanguage;
using DeltaSubstrateInspector.src.PositionMethod.LocateMethod.Location; // (20200429) Jeff Revised!

namespace DeltaSubstrateInspector
{
    public partial class AOIMainForm : Form
    {
        // 主畫面Tab Table
        private StyledTabtable tab_table_ { get; set; } = new StyledTabtable();

        // 主畫面標籤頁面
        private static InspectionView inspect_view_ { get; set; } = new InspectionView();
        private static PositionView position_view_ { get; set; } = new PositionView();
        private static ResultPanel result_view_ { get; set; } = new ResultPanel();       
        public AdvSet advSet { get; set; } = new AdvSet(); // andy
        public LaserSetting laser_setting { get; set; } = new LaserSetting(); // 181205, andy

        // 系統模組
        private static InspectModel inspect_model_ { get; set; } = new InspectModel();
        private static CaptureModel capture_model_ { get; set; } //  = new CaptureModel(); // 181121, andy
        private static PositionModel position_model { get; set; } = new PositionModel();
        //public static PositionModel position_model = new PositionModel(); // (20190405) Jeff Revised!
        private static ResultManager result_manager_ { get; set; }
        private static LaserPositionModel laser_position_model { get; set; } = new LaserPositionModel(); // (20181031) Jeff Revised!

        private clsSaveTileImgThread ThreadSaveImg { get; set; } = null;
        /// <summary>
        /// 走停拍位置總數
        /// </summary>
        private static int move_bound { get; set; } = 0;
        /// <summary>
        /// 視覺定位位置數量
        /// </summary>
        private static int pos_move_count { get; set; } = 2;
        private List<int> by_pass_pos_ { get; set; } = new List<int>();

        /// <summary>
        /// 大圖拼接各位置影像
        /// </summary>
        private List<HObject> img_lst { get; set; } = new List<HObject>();
        /// <summary>
        /// 大圖拼接各位置影像
        /// </summary>
        private HObject img_concat = null; // (20190606) Jeff Revised!

        // 檢測執行緒
        Thread listen_queue_thread_ { get; set; }

        // IO 模組,  181016 andy
        public DIO_Board IOManager { get; set; } = new DIO_Board();

        // Light 模組, 181210 andy
        public Form_24084R LightManager { get; set; } = new Form_24084R();

        // TCP/IP 模組,  181016 andy
        public FormTCPIPSorter TCPIPSorter_Form { get; set; }

        // Reviewer模組, 190107 andy
        public FormReviewer Reviewer_Form { get; set; } = new FormReviewer(true);

        // 帳號密碼模組, 190218 andy
        private Form_PWInput pwModule { get; set; }

        // 流程控制模組, 181016 andy
        private RunningThread runningThread { get; set; }

        /* 通訊Flag, 181016 andy */
        /// <summary>
        /// 0: ready, 1:定位狀態, 2:檢測演算法狀態 , 3:雷射校正驗算法狀態
        /// </summary>
        public int PosInsState { get; set; } = 0;
        /// <summary>
        /// 定位演算法是否完成
        /// </summary>
        public bool PositionFinish { get; set; } = false;
        /// <summary>
        /// 定位演算法是否成功
        /// </summary>
        public bool PositionSuccess { get; set; } = false;
        /// <summary>
        /// 檢測演算法是否完成
        /// </summary>
        private bool inspectionFinish { get; set; } = false;
        public bool InspectionFinish // (20190402) Jeff Revised!
        {
            get { return inspectionFinish; }
            set { inspectionFinish = value; }
        }
        /// <summary>
        /// 檢測演算法是否成功
        /// </summary>
        public bool InspectionSuccess { get; set; } = false;
        /// <summary>
        /// 定位演算法是否完成
        /// </summary>
        public bool LaserPositionFinish { get; set; } = false;
        /// <summary>
        /// 定位演算法是否成功
        /// </summary>
        public bool LaserPositionSuccess { get; set; } = false;
        /// <summary>
        /// 真實料片的視覺定位座標
        /// </summary>
        public float MARKPOS_X1 { get; set; } = 0;
        /// <summary>
        /// 真實料片的視覺定位座標
        /// </summary>
        public float MARKPOS_Y1 { get; set; } = 0;
        /// <summary>
        /// 真實料片的視覺定位座標
        /// </summary>
        public float MARKPOS_X2 { get; set; } = 0;
        /// <summary>
        /// 真實料片的視覺定位座標
        /// </summary>
        public float MARKPOS_Y2 { get; set; } = 0;
        //public List<Point> LaserFINDMarkPosList = new List<Point>();
        public List<PointF> LaserFINDMarkPosList { get; set; } = new List<PointF>(); // (20181119) Jeff Revised!
        public List<Point> RealDefectPosList { get; set; } = new List<Point>();
        public int Cell_X_Count, Cell_Y_Count, Cell_Total_Count;
        
        // Log模組, 181016 andy
        public DelVivi.Log.DelLogs syslog { get; set; }
        private DelLogForm LogForm { get; set; }

        // 設定檔模組
        private DeviceConfig HardwareConfig { get; set; }

        //181026, andy
        private delegate bool DelegateChangeRecipeUpdate(string RecipeName);
        private DelegateChangeRecipeUpdate delegateChangeRecipeUpdate { get; set; }

        // 181119, andy
        public delegate bool delegate_VisionPositionResultUpdate();
        public delegate_VisionPositionResultUpdate dg_ViPosResultUpdate { get; set; }

        public delegate bool delegate_NewChip();
        public delegate_NewChip dg_NewChip { get; set; }

        public delegate bool delegate_CombineVisionResult();
        public delegate_CombineVisionResult dg_CombineVisionResult { get; set; }

        // 離線檢測模式 (20190404) Jeff Revised!
        private bool b_OffLine_test { get; set; } = true;

        /// <summary>
        /// 做大圖拼接之影像是否先執行Resize
        /// </summary>
        private bool b_resize_first { get; set; } = true; // (20190611) Jeff Revised!
        
        /// <summary>
        /// 利用BackgroundWorker讀取所有拼接影像
        /// </summary>
        private BackgroundWorker bw_Concat_tileImgs { get; set; } // (20190613) Jeff Revised!

        public static DeltaSubstrateInspector.src.Modules.NewInspectModule.InductanceInsp.clsSaveImg SaveAOICellImgThread { get; set; } = null;

        public clsPermissionSetting[] PermissionSetting { get; set; } = new clsPermissionSetting[3];

        public enuState NowState { get; set; } = enuState.Stop;

        #region Enum & Class

        public enum enuState
        {
            Run,
            Stop
        }

        public enuOffLineType OffLineType { get; set; } = enuOffLineType.None;
        public enum enuOffLineType
        {
            None,
            Cycle,
            Mult
        }

        /// <summary>
        /// 權限設定項目列舉
        /// </summary>
        [Serializable]
        public enum enuUI
        {
            Config模組, // (20191216) Jeff Revised!
            離線檢測,
            切換參數,
            IO,
            光源,
            TCPIP,
            Reviewer,
            定位設定,
            雷射設定,
            進階設定,
            參數設定,
            檢測結果,
        }

        [Serializable]
        public class clsUISetting
        {
            public enuUI UI = enuUI.IO;
            public bool Enabled = false;

            public clsUISetting() { }
        }

        [Serializable]
        public class clsPermissionSetting
        {
            public int User = 0;
            public List<clsUISetting> UISettingList = new List<clsUISetting>();

            public clsPermissionSetting()
            {

            }

            public clsPermissionSetting(bool New)
            {
                string[] ArrayUI = Enum.GetNames(typeof(enuUI));
                UISettingList.Clear();
                for (int i = 0; i < ArrayUI.Length; i++)
                {
                    clsUISetting Tmp = new clsUISetting();
                    Tmp.UI = (enuUI)i;
                    Tmp.Enabled = false;
                    UISettingList.Add(Tmp);
                }
            }
        }

        #endregion

        public AOIMainForm()
        {
            InitializeComponent();  
            
            // 181121, andy 硬體初始化
            Initialize_Hardware();
            capture_model_ = new CaptureModel();

            // 計算與顯示Thread
            listen_queue_thread_ = new Thread(new ThreadStart(queue_listen));

            // 設定表格
            set_tab_table();
            set_table_contents();

            listen_queue_thread_.IsBackground = true;            

            result_manager_ = new ResultManager();
            result_manager_.setup_static_board(inspect_view_.ResultStaticTable);

            InspectDate = DateTime.Now.ToString("yyyyMMdd");

            #region IO 模組,  181016 andy
            /*
            if(!IOManager.initialConnect())
            {
                MessageBox.Show("IO 模組初始化異常，請確認硬體狀態!\n按 [確定] 以繼續程式。");               
            }*/
            if (!BypassIO)
            {
                IOManager.initialConnect();
                IOManager.SetOutputBit("ModuleOnline", false);
                IOManager.SetOutputBit("ModuleMeasureStart", false);
                IOManager.SetOutputBit("ModuleMeasureDone", false);
            }
            #endregion

            #region 光源 模組, 181210, andy

            if (!BypassLightControl)
            {
                LightManager.HideEnable = true;
                LightManager.FormClosing += Light_OnOKButtonClicked;
                LightManager.Connect(LightComportName);
                timer_InitCheck.Interval = 3000;
                timer_InitCheck.Enabled = true;
            }

            #endregion

            #region TCP/IP 模組,  181016 andy

            //string TCPIP4Sorter_IP = "127.0.0.1";
            string TCPIP4Sorter_IP = "192.168.0.2";
            string TCPIP4Sorter_Port = "5001";
            TCPIPSorter_Form = new FormTCPIPSorter();
            TCPIPSorter_Form.SetIPandPort(TCPIP4Sorter_IP, TCPIP4Sorter_Port);     // XML參數設定IP and Port
            TCPIPSorter_Form.dMes.Start(TCPIP4Sorter_IP, Int32.Parse(TCPIP4Sorter_Port));

            #endregion

            #region Log 模組, 181016 andy
            syslog = new DelLogs("syslog");   
            LogForm = new DelLogForm();
            LogForm.LogsSource = syslog;
            #endregion

            #region 控制流程, 181016 andy
            runningThread = new RunningThread(this);

            runningThread.delegateChangeRecipe += invokeChangeRecipeAndUpdate;
            delegateChangeRecipeUpdate = new DelegateChangeRecipeUpdate(ChangeRecipe);
            #endregion

            #region 帳號密碼模組, 190218 andy
            string configDir = ConfigDirectory.Replace("\\", "");
            string pwDir = ModuleParamDirectory + "\\" + configDir;
            string UIPathFile = ModuleParamDirectory + "\\" + ConfigDirectory + UserUISettingPathFile;
            PermissionSetting = LoadUserXML(UIPathFile);
            pwModule = new Form_PWInput("pw.bin", pwDir);
            pwModule.HideEnable = true;
            pwModule.PasswordLevelChange = LevelChange;
            LevelChange(this, null);

            #endregion

            initialize();

            advSet.SetMainForm(this);
            // 181119, andy
            dg_NewChip = new delegate_NewChip(NewChip);
            dg_CombineVisionResult = new delegate_CombineVisionResult(CombineVisionResult);
            dg_ViPosResultUpdate = new delegate_VisionPositionResultUpdate(ViPosResultUpdate);

            #region 讀取Recipe

            LoadLastRecipeName();
            InitRecipe(LastRecipeName);

            #endregion

            // 181026, andy
            runningThread.RunningThreadStart();

            if (ThreadSaveImg == null)
                ThreadSaveImg = new clsSaveTileImgThread();

            // (20190402) Jeff Revised!
            inspect_view_.set_AOIMainForm(this);

            //initBackgroundWorker(); // (20190613) Jeff Revised!

            //ActiveDevice();

            #region 語言切換 (20200122) Jeff Revised!

            Language = clsLanguage.clsLanguage.GetLanguage(); // (20200214) Jeff Revised!
            if (Language == "Chinese")
                中文ToolStripMenuItem.Checked = true;
            else if (Language == "English")
                英文ToolStripMenuItem.Checked = true;

            /* 特殊字元先刪除 (因為無法存入INI檔) */
            if (run_StripMenuItem1.Text == "執行 ▶")
                run_StripMenuItem1.Text = run_StripMenuItem1.Text.Substring(0, 2);
            else if (run_StripMenuItem1.Text == "Run ▶")
                run_StripMenuItem1.Text = run_StripMenuItem1.Text.Substring(0, 3);

            //clsLanguage.clsLanguage.SetLanguateToControls(this); // (20200214) Jeff Revised!
            clsLanguage.clsLanguage.SetLanguateToControls(this.panel1); // (20200214) Jeff Revised!
            clsLanguage.clsLanguage.SetLanguateToControls(this.tab_table_); // (20200214) Jeff Revised!

            /* 特殊字元補回 (因為無法存入INI檔) */
            run_StripMenuItem1.Text += " ▶";

            #endregion

        }

        private void AOIMainForm_Load(object sender, EventArgs e)
        {
            label_Version.Text = "版本: " + Application.ProductVersion;

            #region 存小圖Thread

            if (SaveAOICellImgThread == null)
            {
                SaveAOICellImgThread = new src.Modules.NewInspectModule.InductanceInsp.clsSaveImg();
            }
            else
            {
                SaveAOICellImgThread.Dispose();
                SaveAOICellImgThread = null;
                SaveAOICellImgThread = new src.Modules.NewInspectModule.InductanceInsp.clsSaveImg();
            }

            #endregion
        }

        public static clsPermissionSetting[] LoadUserXML(string LoadPath)
        {
            clsPermissionSetting[] Res = new clsPermissionSetting[3];

            if (!File.Exists(LoadPath))
            {
                for (int i = 0; i < Res.Length; i++)
                {
                    clsPermissionSetting Tmp = new clsPermissionSetting(true);
                    Res[i] = Tmp;
                    Res[i].User = i;
                }
                return Res;
            }
            try
            {
                System.Xml.Serialization.XmlSerializer XmlS = new System.Xml.Serialization.XmlSerializer(Res.GetType());
                Stream S = File.Open(LoadPath, FileMode.Open);
                Res = (clsPermissionSetting[])XmlS.Deserialize(S);
                S.Close();
            }
            catch
            {
                for (int i = 0; i < Res.Length; i++)
                {
                    clsPermissionSetting Tmp = new clsPermissionSetting(true);
                    Res[i] = Tmp;
                    Res[i].User = i;
                }
                MessageBox.Show("權限管控錯誤...");
                return Res;
            }

            return Res;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.S))
            {
                pwModule.SetVenderAccessLevel();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// 硬體初始化
        /// </summary>
        /// <returns></returns>
        private bool Initialize_Hardware()
        {
            string fName = ModuleParamDirectory + ConfigDirectory + HardwareConfigFileName;
            if (File.Exists(fName))
            {
                HardwareConfig = new DeviceConfig(fName, "Hardware");
                //TriggerCountsPerMove = int.Parse((HardwareConfig.GetParam("TriggerCountsPerMove") == "") ? "2" : HardwareConfig.GetParam("TriggerCountsPerMove"));
                LightComportName = (HardwareConfig.GetParam("LightComportName") == "") ? "COM1" : HardwareConfig.GetParam("LightComportName");
                LightOffSet = (HardwareConfig.GetParam("LightOffSet") == "") ? false : Convert.ToBoolean(HardwareConfig.GetParam("LightOffSet"));
                //string debug = HardwareConfig.GetParam("CameraInterface"); // For debug! (20190613) Jeff Revised!
                CameraInterface = (HardwareConfig.GetParam("CameraInterface") == "") ? 0 : Convert.ToInt32(HardwareConfig.GetParam("CameraInterface"));
                BypassIO = (HardwareConfig.GetParam("BypassIO") == "") ? false : Convert.ToBoolean(HardwareConfig.GetParam("BypassIO"));
                BypassLightControl = (HardwareConfig.GetParam("BypassLightControl") == "") ? false : Convert.ToBoolean(HardwareConfig.GetParam("BypassLightControl"));
                CallBackType = (HardwareConfig.GetParam("CallbackType") == "") ? "EventExposureEnd" : (HardwareConfig.GetParam("CallbackType"));
                /*
                mStaticCaptureTest = int.Parse((HardwareConfig.GetParam("StaticCaptureTest") == "") ? "0" : HardwareConfig.GetParam("StaticCaptureTest"));
                mLogClear_DurationInHr = double.Parse((HardwareConfig.GetParam("LogClear_DurationInHr") == "") ? "1" : HardwareConfig.GetParam("LogClear_DurationInHr"));
                mTriggerBoardSimResponseTime = int.Parse((HardwareConfig.GetParam("TriggerBoardSimResponseTime") == "") ? "500" : HardwareConfig.GetParam("TriggerBoardSimResponseTime"));
                mWaitMotionFinishResponseTime = int.Parse((HardwareConfig.GetParam("WaitMotionFinishResponseTime") == "") ? "30000" : HardwareConfig.GetParam("WaitMotionFinishResponseTime"));
                mWaitVisionDoneReadyResponseTime = int.Parse((HardwareConfig.GetParam("WaitVisionDoneReadyResponseTime") == "") ? "30000" : HardwareConfig.GetParam("WaitVisionDoneReadyResponseTime"));
                mWaitSysMDOnResponseTime = int.Parse((HardwareConfig.GetParam("WaitSysMDOnResponseTime") == "") ? "60000" : HardwareConfig.GetParam("WaitSysMDOnResponseTime"));
                */
            }
            else
            {
                // Defaul seting
                //TriggerCountsPerMove = 2;
                LightComportName = "COM1";
                LightOffSet = false;
                CameraInterface = (int)(enu_CameraInterface.USB3);
                BypassIO = false;
                BypassLightControl = false;
                CallBackType = "EventExposureEnd";

                // Show message
                /*
                MessageBox.Show("尚未建立系統設定檔，按確定後，DSI將自動建立\n"+ 
                                "TriggerCountsPerMove: " + TriggerCountsPerMove.ToString());
                                */
                MessageBox.Show("尚未建立系統設定檔，按確定後，DSI將自動建立\n");
                SaveConfigFile();


            }


            return true;

        }

        public void InitRecipe(string RecipeName)
        {
            try
            {
                if (!string.IsNullOrEmpty(LastRecipeName))
                {
                    if (!ChangeRecipe(LastRecipeName))
                    {
                        MessageBox.Show("載入Recipe失敗,請手動載入");
                        return;
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool LoadLastRecipeName()
        {
            bool Res = false;

            try
            {
                string fName = ModuleParamDirectory + ConfigDirectory + LastRecipePathFile;
                if (File.Exists(fName))
                {
                    clsWinAPI.GetPrivateProfileString("Recipe", "RecipeName", "", clsWinAPI.GetString, 255, fName);
                    LastRecipeName = clsWinAPI.GetString.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return Res;
            }

            Res = true;
            return Res;
        }

        private void initBackgroundWorker() // (20190613) Jeff Revised!
        {
            bw_Concat_tileImgs = new BackgroundWorker();
            bw_Concat_tileImgs.WorkerReportsProgress = false;
            bw_Concat_tileImgs.WorkerSupportsCancellation = true;
            bw_Concat_tileImgs.DoWork += new DoWorkEventHandler(bw_DoWork_Concat_tileImgs);
            bw_Concat_tileImgs.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted_Concat_tileImgs);
        }

        private void UpdateStateControl(enuState State)
        {
            bool StateEnabled = false;
            switch(State)
            {
                case enuState.Run:
                    {
                        StateEnabled = false;

                        #region 改變狀態
                        xu6vu04ToolStripMenuItem.Enabled = StateEnabled;
                        file_ToolStripMenuItem.Enabled = StateEnabled;
                        StopToolStripMenuItem.Enabled = !StateEnabled;
                        run_StripMenuItem1.Enabled = StateEnabled;
                        光源模組ToolStripMenuItem.Enabled = StateEnabled;
                        帳號密碼模組ToolStripMenuItem.Enabled = StateEnabled;
                        tab_table_.get_panel_by_name("檢測狀態").Enabled = !StateEnabled;
                        tab_table_.get_panel_by_name("定位設定").Enabled = StateEnabled;
                        tab_table_.get_panel_by_name("雷射設定").Enabled = StateEnabled;
                        tab_table_.get_panel_by_name("進階設定").Enabled = StateEnabled;
                        foreach (Control control in inspect_view_.Controls)
                        {
                            if (control.Name == "panel1")
                            {
                                control.Enabled = StateEnabled;
                                break;
                            }
                        }
                        label_RecipeName.Click -= label_RecipeName_Click;
                        #endregion
                    }
                    break;
                case enuState.Stop:
                    {
                        StateEnabled = true;

                        #region 改變狀態
                        xu6vu04ToolStripMenuItem.Enabled = StateEnabled;
                        file_ToolStripMenuItem.Enabled = StateEnabled;
                        StopToolStripMenuItem.Enabled = !StateEnabled;
                        run_StripMenuItem1.Enabled = StateEnabled;
                        光源模組ToolStripMenuItem.Enabled = StateEnabled;
                        帳號密碼模組ToolStripMenuItem.Enabled = StateEnabled;
                        tab_table_.get_panel_by_name("定位設定").Enabled = StateEnabled;
                        tab_table_.get_panel_by_name("雷射設定").Enabled = StateEnabled;
                        tab_table_.get_panel_by_name("進階設定").Enabled = StateEnabled;
                        foreach (Control control in inspect_view_.Controls)
                        {
                            if (control.Name == "panel1")
                            {
                                control.Enabled = StateEnabled;
                                break;
                            }
                        }
                        label_RecipeName.Click -= label_RecipeName_Click;
                        label_RecipeName.Click += label_RecipeName_Click;
                        #endregion
                    }
                    break;
                default:
                    {

                    }
                    break;
            }
        }

        private bool SaveLastRecipeName()
        {
            bool Res = false;

            try
            {
                if (string.IsNullOrEmpty(ModuleName))
                {
                    return Res;
                }
                string nowFN = ModuleParamDirectory;
                if (!Directory.Exists(nowFN))
                {
                    Directory.CreateDirectory(nowFN);
                }
                nowFN = ModuleParamDirectory + ConfigDirectory;
                if (!Directory.Exists(nowFN))
                {
                    Directory.CreateDirectory(nowFN);
                }
                nowFN = ModuleParamDirectory + ConfigDirectory + LastRecipePathFile;

                FileStream fs = new FileStream(nowFN, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs, Encoding.Default);

                string RecipeName = "RecipeName=" + ModuleName;


                sw.WriteLine("[Recipe]");
                sw.WriteLine(RecipeName);


                sw.Flush();
                sw.Close();
                fs.Dispose();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return Res;
            }

            Res = true;
            return Res;
        }

        /// <summary>
        /// 儲存 Hardware Config
        /// </summary>
        /// <returns></returns>
        public static bool SaveConfigFile() // (20191216) Jeff Revised!
        {
            // Create
            string nowFN = ModuleParamDirectory;
            if (!Directory.Exists(nowFN))
            {
                Directory.CreateDirectory(nowFN);
            }
            nowFN = ModuleParamDirectory + ConfigDirectory;
            if (!Directory.Exists(nowFN))
            {
                Directory.CreateDirectory(nowFN);
            }
            nowFN = ModuleParamDirectory + ConfigDirectory + HardwareConfigFileName;
            //if (!File.Exists(nowFN))
            {
                #region 暴力產生 ini檔
                FileStream fs = new FileStream(nowFN, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);

                //string tmp1 = "TriggerCountsPerMove=" + TriggerCountsPerMove.ToString();
                string tmp1 = "LightComportName=" + LightComportName;
                string tmp2 = "LightOffSet=" + LightOffSet.ToString();
                string tmp3 = "CameraInterface=" + CameraInterface.ToString();
                string tmp4 = "BypassIO=" + BypassIO.ToString();
                string tmp5 = "BypassLightControl=" + BypassLightControl.ToString();
                string tmp6 = "CallbackType=" + CallBackType.ToString();

                sw.WriteLine("[Hardware]");
                sw.WriteLine(tmp1);
                sw.WriteLine(tmp2);
                sw.WriteLine(tmp3);
                sw.WriteLine(tmp4);
                sw.WriteLine(tmp5);
                sw.WriteLine(tmp6);

                sw.Flush();
                sw.Close();

                #endregion
            }

            return true;
        }

        private void LevelChange(object sender, EventArgs e)
        {
            labUserLevel.Text = pwModule.GetAccessLevelName();
            UpdateUserLevelControl(pwModule.GetAccessLevel());
            //MessageBox.Show("hello: " + pwModule.GetAccessLevel());

            //button_test.Enabled = pwModule.GetAccessLevel() == 2 ? false : true;
            //button_test.Text = pwModule.GetAccessLevelName();

        }

        public void UpdateUserLevelControl(int UserLevel)
        {
            clsPermissionSetting Param;

            Param = PermissionSetting[UserLevel];

            #region 設定權限開啟與關閉UI
            for (int i = 0; i < Param.UISettingList.Count; i++)
            {
                if (Param.UISettingList[i].UI == enuUI.Config模組) // (20191216) Jeff Revised!
                    config模組ToolStripMenuItem.Enabled = Param.UISettingList[i].Enabled;
                
                if (Param.UISettingList[i].UI == enuUI.IO)
                    iO模組ToolStripMenuItem.Enabled = Param.UISettingList[i].Enabled;

                if (Param.UISettingList[i].UI == enuUI.Reviewer)
                    reviewer模組ToolStripMenuItem.Enabled = Param.UISettingList[i].Enabled;

                if (Param.UISettingList[i].UI == enuUI.TCPIP)
                    tCPIP模組ToolStripMenuItem.Enabled = Param.UISettingList[i].Enabled;
                
                if (Param.UISettingList[i].UI == enuUI.光源)
                    光源模組ToolStripMenuItem.Enabled = Param.UISettingList[i].Enabled;

                if (Param.UISettingList[i].UI == enuUI.切換參數)
                {
                    file_ToolStripMenuItem.Enabled = Param.UISettingList[i].Enabled;
                     if (Param.UISettingList[i].Enabled)
                    {
                        label_RecipeName.Click -= label_RecipeName_Click;
                        label_RecipeName.Click += label_RecipeName_Click;
                    }
                     else
                    {
                        label_RecipeName.Click -= label_RecipeName_Click;
                    }
                }

                if (Param.UISettingList[i].UI == enuUI.參數設定)
                {
                    foreach (Control control in inspect_view_.Controls)
                    {
                        if (control.Name == "panel1")
                        {
                            control.Enabled = Param.UISettingList[i].Enabled;
                            break;
                        }
                    }
                }

                if (Param.UISettingList[i].UI == enuUI.定位設定)
                    tab_table_.get_panel_by_name("定位設定").Enabled = Param.UISettingList[i].Enabled;

                if (Param.UISettingList[i].UI == enuUI.檢測結果)
                {
                    foreach (Control control in inspect_view_.Controls)
                    {
                        if (control.Name == "panel5")
                        {
                            control.Enabled = Param.UISettingList[i].Enabled;
                            break;
                        }
                    }
                }

                if (Param.UISettingList[i].UI == enuUI.進階設定)
                    tab_table_.get_panel_by_name("進階設定").Enabled = Param.UISettingList[i].Enabled;

                if (Param.UISettingList[i].UI == enuUI.離線檢測)
                    xu6vu04ToolStripMenuItem.Enabled = Param.UISettingList[i].Enabled;

                if (Param.UISettingList[i].UI == enuUI.雷射設定)
                    tab_table_.get_panel_by_name("雷射設定").Enabled = Param.UISettingList[i].Enabled;
            }
            #endregion

        }

        #region 設定主UI畫面上的各分頁

        // Set UI of main form
        //    @function : set_tab_table : Set up the color, lebel, amount of tab table.
        //    @function : set_table_contents : Create related panel to each page of table.
        private void set_tab_table()
        {
            int[] hover_color_ = { 37, 91, 165 };
            int[] clicked_color_ = { 47, 101, 195 };
            int[] back_color_ = { 27, 85, 159 };

            tab_table_.set_layout(100, 100, 1920, 1010, "vertical", true);
            tab_table_.set_colors(hover_color_, clicked_color_, back_color_);
            tab_table_.new_tab_item("檢測狀態");
            tab_table_.new_tab_item("定位設定");
            tab_table_.new_tab_item("雷射設定"); // andy
            tab_table_.new_tab_item("進階設定"); // (20181031) Jeff Revised!

            //tab_table_.new_tab_item("瑕疵檢視");
            //tab_table_.new_tab_item("瑕疵統計");    


            tab_table_.Location = new System.Drawing.Point(0, 45);
            this.Controls.Add(tab_table_);
        }

        private void set_table_contents()
        {
            set_inspect_view_pnl();
            set_position_view_pnl();                    
            set_lasersetting_pnl();
            set_adv_view_pnl();

            //set_result_view_pnl();

        }

        private void set_position_view_pnl()
        {
            Panel container = tab_table_.get_panel_by_name("定位設定");
            container.Controls.Add(position_view_);
            position_view_.ok_clicked += new EventHandler(position_param_ok);
        }
       
        private void set_inspect_view_pnl()
        {
            Panel container = tab_table_.get_panel_by_name("檢測狀態");
            container.Controls.Add(inspect_view_);
            inspect_view_.model_item_clicked += new EventHandler(model_param_setup);
            inspect_view_.map_setting_clicked += new EventHandler(map_setup);
        }
      
        private void set_adv_view_pnl()
        {
            Panel container = tab_table_.get_panel_by_name("進階設定");
            container.Controls.Add(advSet);
            advSet.ContiGrab_clicked += AdvSet_ContiGrab;
            advSet.connectCamera_clicked += AdvSet_connectCamera;
        }

        private void set_lasersetting_pnl()
        {
            Panel container = tab_table_.get_panel_by_name("雷射設定");
            container.Controls.Add(laser_setting);
            laser_setting.laser_position_view_.ok_clicked += new EventHandler(laser_position_param_ok);
            
        }

        private void set_result_view_pnl()
        {
            Panel container = tab_table_.get_panel_by_name("瑕疵檢視");
            container.Controls.Add(result_view_);
        }

        #endregion


        #region 模組維護 按鈕事件

        private void iO模組ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (BypassIO)
                return;
            using (DIO_Board form = new DIO_Board())
            {
                form.ShowDialog();
            }
        }

        private void config模組ToolStripMenuItem_Click(object sender, EventArgs e) // (20191216) Jeff Revised!
        {
            using (ConfigModule form = new ConfigModule())
            {
                if (form.ShowDialog() == DialogResult.Yes) // 立即關閉程式
                    this.Close();
            }
        }

        private void 光源模組ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (BypassLightControl)
                return;

            if (LightManager.IsConnect() == true)
            {
                LightManager.SetMaxLigthValueIndex(LightMaxValueIndex.Light01_MaxVauleIndex, LightMaxValueIndex.Light02_MaxVauleIndex, LightMaxValueIndex.Light03_MaxVauleIndex, LightMaxValueIndex.Light04_MaxVauleIndex);
                LightManager.SetLightValue(LightValue.Light01_Vaule, LightValue.Light02_Vaule, LightValue.Light03_Vaule, LightValue.Light04_Vaule);
            }
            LightManager.Show();            
        }

        private void tCPIP模組ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 顯示TCP/IP form
            TCPIPSorter_Form.Show();

        }

        private void 訊息模組ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 顯示Log form
            LogForm.Show();
        }


        private void file_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void Light_OnOKButtonClicked(object sender, EventArgs e)
        {

            //MessageBox.Show("get u");
            if (LightManager.IsConnect() == true)
            {
                LightManager.GetMaxLightValueIndex(out LightMaxValueIndex.Light01_MaxVauleIndex, out LightMaxValueIndex.Light02_MaxVauleIndex, out LightMaxValueIndex.Light03_MaxVauleIndex, out LightMaxValueIndex.Light04_MaxVauleIndex);
                LightManager.GetLightValue(out LightValue.Light01_Vaule, out LightValue.Light02_Vaule, out LightValue.Light03_Vaule, out LightValue.Light04_Vaule);
                LightOffSet = LightManager.GetLightOffSetEnabled();
                SaveConfigFile();
            }

        }

        private void reviewer模組ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reviewer_Form.Show();
        }

        #endregion

        /// <summary>
        /// 實際執行檢測
        /// </summary>
        private void queue_listen()
        {
            bool cal_rotation_angle_Success = false; // (20190401) Jeff Revised!
            while (true)
            {

                //PosInsState = 0;    // 181016 andy
                if (capture_model_.get_img_queue().Count > 0)
                {
                    // 取得影像物件 ImageObj
                    ImageObj img_set;
                    capture_model_.get_img_queue().TryDequeue(out img_set);

                    if (FinalInspectParam.VisualPosEnable || (runningThread.WorkMode == "FIND")) // 包含視覺定位 or 雷射校正
                    {
                        #region 包含視覺定位 or 雷射校正

                        try
                        {

                            // 181120, andy:儲存取得的影像                        
                            if (inspect_view_.SaveImgOn == true)
                            {
                                StorageManager.write_source_img(img_set, advSet.B_InerCounterIDMode);
                            }


                            #region 執行視覺定位, 檢測, 校正
                            if (runningThread.WorkMode != "FIND")
                            {
                                if ((img_set.MoveIndex) - pos_move_count <= 0)
                                {
                                    if (img_set.MoveIndex == 1)
                                        syslog.WriteLine(enMessageType.Info, "---------- Process images of first position ----------"); // (20200717) Jeff Revised!

                                    #region [視覺定位流程] 前兩個位置執行

                                    PosInsState = 1;  // 181016 andy
                                    PositionFinish = false; // 181016 andy
                                    PositionSuccess = false;  // 181016 andy
                                    inspectionFinish = false; // 190227, andy: !!!! 重要 Reset !!!!
                                    InspectionSuccess = false; // Chris
                                    B_imagesFinished = false; // (20190604) Jeff Revised!

                                    if (advSet.IgnorePosAndIns == false)
                                    {
                                        #region Real Doing

                                        // 利用定位影像作定位、建立位置地圖
                                        //position_model.locateUseImgs.Add(img_set.Source[0]);
                                        position_model.locateUseImgs.Add(img_set.Source[FinalInspectParam.PosImgID]); // (20190627) Jeff Revised!

                                        if (img_set.MoveIndex == pos_move_count)
                                        {
                                            // 190403, andy GC setting
                                            //GC.Collect();
                                            //GC.WaitForPendingFinalizers();

                                            syslog.WriteLine(enMessageType.Info, "Start marks positioning"); // (20200717) Jeff Revised!

                                            // 執行視覺定位演算法
                                            position_model.cal_rotation_angle(advSet.B_savePos, advSet.B_savePos_draw, advSet.B_savePos_RotImg); // (20190112) Jeff Revised!
                                            cal_rotation_angle_Success = position_model.Find_Shpe_Model_Success;

                                            syslog.WriteLine(enMessageType.Info, "Marks positioning ends"); // (20200717) Jeff Revised!

                                            // 181119, andy: 設定總chip數量, ex: MJ, 1287=33*39
                                            result_manager_.setup_static_board_total(position_model.total);
                                            result_manager_.SetCellCount(position_model.total);//更新顆數

                                            // Get position                                            
                                            MARKPOS_X1 = MARKPOS_Y1 = MARKPOS_X2 = MARKPOS_Y2 = 0; // 190403, andy
                                            MARKPOS_X1 = position_model.mark_position_lst_f[0].X;
                                            MARKPOS_Y1 = position_model.mark_position_lst_f[0].Y;
                                            MARKPOS_X2 = position_model.mark_position_lst_f[1].X;
                                            MARKPOS_Y2 = position_model.mark_position_lst_f[1].Y;

                                            // 190107, andy: Update vision postion result
                                            Invoke(dg_ViPosResultUpdate);

                                            // Set process result string                                            
                                            SetProcessResultString(cal_rotation_angle_Success); // 190116, andy

                                            // Set
                                            PosInsState = 0; // 190213, andy: reset state
                                            PositionFinish = true; // 181016 andy
                                            PositionSuccess = cal_rotation_angle_Success; // 190116. andy

                                            // Get position (20190401) Jeff Revised!
                                            //MARKPOS_X1 = MARKPOS_Y1 = MARKPOS_X2 = MARKPOS_Y2 = 0; // 190403, andy
                                            //MARKPOS_X1 = position_model.mark_position_lst_f[0].X;
                                            //MARKPOS_Y1 = position_model.mark_position_lst_f[0].Y;
                                            //MARKPOS_X2 = position_model.mark_position_lst_f[1].X;
                                            //MARKPOS_Y2 = position_model.mark_position_lst_f[1].Y;

                                        }

                                        #endregion

                                    }
                                    else
                                    {
                                        #region Bypass Doing

                                        if (img_set.MoveIndex == pos_move_count)
                                        {
                                            // Set process result string
                                            SetProcessResultString(true);

                                            // Set
                                            PosInsState = 0; // 190213, andy: reset state
                                            PositionFinish = true; // 181016 andy
                                            PositionSuccess = true; // 181016 andy
                                        }

                                        #endregion

                                    }

                                    #endregion

                                }
                                //else if (!cal_rotation_angle_Success) // 定位失敗 (20190401) Jeff Revised!
                                //{
                                //    SetProcessResultString(false);
                                //    SetWorkResultString(false);
                                //    int testCount = capture_model_.get_img_queue().Count;
                                //    if (testCount == 0)
                                //    {
                                //        Invoke(dg_CombineVisionResult);

                                //        // 統計結果存到 Excel
                                //        result_manager_.store_up_chip();
                                //    }

                                //    PositionFinish = true;
                                //    PositionSuccess = false;

                                //    PosInsState = 0;



                                //}
                                else
                                {

                                    #region [視覺檢測流程] 第三個位置開始執行

                                    if (PosInsState != 2)
                                        result_manager_.Set_InerCountID(); // 設定基板序號 (內部計數ID模式)

                                    PosInsState = 2;          // 181016 andy
                                    inspectionFinish = false; // 181016 andy
                                    InspectionSuccess = false; // 181016 andy
                                    PositionFinish = false; // (20190417) Jeff Revised!

                                    #region [視覺檢測-單Move檢測] 

                                    // 調整影像物件的位移編號 : 扣除定位影像的數量後，才是真正的位移位置
                                    img_set.MoveIndex = ((img_set.MoveIndex - pos_move_count) % move_bound == 0) ? move_bound : (img_set.MoveIndex - pos_move_count) % move_bound;


                                    // 每一新料片開始檢測前的初始設定
                                    if (img_set.MoveIndex == 1)
                                    {
                                        //img_lst.Clear();
                                        // (20190606) Jeff Revised!
                                        if (img_concat != null)
                                        {
                                            img_concat.Dispose();
                                            img_concat = null;
                                        }
                                        HOperatorSet.GenEmptyObj(out img_concat);

                                        inspect_view_.release();
                                        inspect_model_.initialize();
                                        inspect_view_.setup_map(MovementPos.col, MovementPos.row); //20180530
                                        RealDefectPosList.Clear(); // 190403, andy

                                        // 181119, andy
                                        Invoke(dg_NewChip);

                                        //inspect_view_.set_big_map(null);
                                        //inspect_view_.set_big_map2(null, null, null, null); // (20181025) Jeff Revised!
                                        inspect_view_.set_big_map3(null); // (20181116) Jeff Revised!
                                        inspect_view_.set_locate_method(position_model.get_locate_method()); // (20181116) Jeff Revised!

                                        position_model.Clear_all_defects(); // 清除所有瑕疵區域及瑕疵座標 (20181112) Jeff Revised!
                                        position_model.Dispose(); // 釋放記憶體 (20200429) Jeff Revised!

                                        // 初始化瑕疵分類 (20190716) Jeff Revised!
                                        position_model.Initialize_DefectsClassify(inspect_model_.Initialize_DefectsClassify());
                                        
                                        // 人工覆判 或 AI分圖
                                        if (FinalInspectParam.b_Recheck || FinalInspectParam.b_AICellImg_Enable) // (20200429) Jeff Revised!
                                        {
                                            clsStaticTool.DisposeAll(CellReg_MoveIndex_FS);
                                            CellReg_MoveIndex_FS.Clear();
                                            clsStaticTool.DisposeAll(DefReg_MoveIndex_FS);
                                            DefReg_MoveIndex_FS.Clear();
                                        }

                                        // 設定 儲存瑕疵結果 相關資訊
                                        if (FinalInspectParam.b_SaveDefectResult) // (20200429) Jeff Revised!
                                        {
                                            // 儲存瑕疵結果啟用時，則強制核取 AdvSet 中的 檢視大圖
                                            advSet.Set_checkBox_BigMap(true);
                                            
                                            // 設定瑕疵檔路徑
                                            string SBID = ""; // 基板序號
                                            if (B_SB_InerCountID)
                                                SBID = SB_InerCountID;
                                            else
                                                SBID = SB_ID;
                                            position_model.Set_Path_DefectTest(FinalInspectParam.Directory_SaveDefectResult + "\\" + PartNumber + "\\" + SBID + "\\" + ModuleName);

                                            // 設定 模組名稱、生產料號、序號及工單名稱
                                            LocateMethod locate_method_ = position_model.get_locate_method();
                                            locate_method_.productName = FinalInspectParam.ProductName;
                                            locate_method_.partNumber = PartNumber;
                                            locate_method_.sB_ID = SBID;
                                            locate_method_.moduleName = ModuleName;
                                        }
                                    }

                                    if (advSet.IgnorePosAndIns == false)
                                    {

                                        // 儲存影像至img_lst或img_concat，最後拼定位大圖用
                                        int globalMapImagID = FinalInspectParam.GlobalMapImgID;
                                        if (advSet.B_BigMap || advSet.get_SaveTileImg) // 檢視大圖 or 儲存大圖 (20190405) Jeff Revised!
                                        {
                                            //img_lst.Add(img_set.Source[globalMapImagID]); // (20190405) Jeff Revised!
                                            //img_lst.Add(img_set.Source[globalMapImagID].Clone()); // (20190408) Jeff Revised!
                                            // (20190606) Jeff Revised!
                                            //HOperatorSet.ConcatObj(img_concat, img_set.Source[globalMapImagID], out img_concat);

                                            #region Method 1:
                                            // (20190611) Jeff Revised!
                                            if (!b_resize_first)
                                            {
                                                {
                                                    HObject ExpTmpOutVar_0;
                                                    HOperatorSet.ConcatObj(img_concat, img_set.Source[globalMapImagID], out ExpTmpOutVar_0);
                                                    img_concat.Dispose();
                                                    img_concat = ExpTmpOutVar_0;
                                                }
                                            }
                                            else
                                            {
                                                HObject img_resize;
                                                HOperatorSet.ZoomImageFactor(img_set.Source[globalMapImagID], out img_resize, position_model.get_locate_method().resize_, position_model.get_locate_method().resize_, "bilinear");
                                                {
                                                    HObject ExpTmpOutVar_0;
                                                    HOperatorSet.ConcatObj(img_concat, img_resize, out ExpTmpOutVar_0);
                                                    img_concat.Dispose();
                                                    img_concat = ExpTmpOutVar_0;
                                                }
                                                img_resize.Dispose();
                                            }
                                            #endregion
                                            #region Method 2:
                                            // (20190613) Jeff Revised!
                                            // 建立一個執行緒，並且傳入一個委派物件 ParameterizedThreadStart，並且設定指向 Concat_tileImgs 方法
                                            //Thread myThread = new Thread(new ParameterizedThreadStart(Thread_Concat_tileImgs));
                                            // 啟動執行緒物件，並且傳入參數
                                            //myThread.Start(img_set.Source[globalMapImagID]);
                                            #endregion
                                            #region Method 3: 跑一次後，不會觸發RunWorkerCompleted事件，bw_Concat_tileImgs.IsBusy一直為true，不知哪裡出問題???
                                            //while (true)
                                            //{
                                            //    if (!bw_Concat_tileImgs.IsBusy)
                                            //    {
                                            //        //bw_Concat_tileImgs.RunWorkerAsync(img_set.Source[globalMapImagID]);
                                            //        bw_Concat_tileImgs.RunWorkerAsync();
                                            //        break;
                                            //    }
                                            //}
                                            #endregion
                                            #region Method 4: 不會觸發RunWorkerCompleted事件，一直創新物件bw_Concat_tileImgs2，記憶體可能會爆掉???
                                            //BackgroundWorker bw_Concat_tileImgs2 = new BackgroundWorker();
                                            //bw_Concat_tileImgs2.WorkerReportsProgress = false;
                                            //bw_Concat_tileImgs2.WorkerSupportsCancellation = true;
                                            //bw_Concat_tileImgs2.DoWork += new DoWorkEventHandler(bw_DoWork_Concat_tileImgs);
                                            //bw_Concat_tileImgs2.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted_Concat_tileImgs);
                                            //bw_Concat_tileImgs2.RunWorkerAsync(img_set.Source[globalMapImagID]);
                                            #endregion
                                        }

                                        /* 執行檢測 */
                                        MOVE_ID = img_set.MoveIndex.ToString(); // 181114, andy
                                        inspect_model_.operator_execute(img_set.Source, by_pass_pos_.Contains(img_set.MoveIndex));

                                        /* 更新 結果物件內容 (計算瑕疵統計結果) */
                                        // 181218, andy
                                        //result_manager_.update_chip(img_set,
                                        //                            inspect_model_.get_TotalResult(),
                                        //                            position_model.get_defect_locate(inspect_model_.get_TotalDefect_with_name(), img_set.MoveIndex),
                                        //                            inspect_model_.get_dic_result(),
                                        //                            position_model.get_defect_locate(inspect_model_.get_defect_with_name_New(), img_set.MoveIndex));
                                        // 總瑕疵 (20190718) Jeff Revised!
                                        result_manager_.update_chip(img_set,
                                                                    inspect_model_.get_TotalResult(),
                                                                    position_model.get_defect_locate(inspect_model_.get_TotalDefect_with_name(), img_set.MoveIndex),
                                                                    position_model.get_defect_locate(inspect_model_.get_defect_with_name_New(), img_set.MoveIndex),
                                                                    inspect_model_.get_dic_result());
                                        result_manager_.update_current_defect_count((position_model.get_locates()).Count);

                                        // 瑕疵分類統計
                                        position_model.get_DefectsClassify_locate(inspect_model_.get_defect_with_name_New(), img_set.MoveIndex); // (20190717) Jeff Revised!

                                        if (!cal_rotation_angle_Success) // (20200114) Jeff Revised!
                                            throw new Exception("視覺定位失敗");

                                        /* 更新 Cell Center Region */
                                        if (FinalInspectParam.CellCenterInspectEnable)
                                            position_model.update_CellCenterReg(inspect_model_.get_CellRegion(), img_set.MoveIndex); // (20181207) Jeff Revised!

                                        /* 更新 顯示內容 */
                                        if (FinalInspectParam.dispWindow == "pictureBox") // (20190718) Jeff Revised!
                                        {
                                            // Display result image in the inspect view 
                                            inspect_view_.set_result_img(result_manager_.get_last_defect_img(img_set.MoveIndex - 1));

                                            inspect_view_.set_result_map(img_set,
                                                                         result_manager_.get_defect_rgn(img_set.MoveIndex),
                                                                         position_model.get_locates(),
                                                                         result_manager_.get_dic_defect(img_set.MoveIndex));
                                        }
                                        else
                                        {
                                            // Display result image in the inspect view 
                                            HObject TotalDefect_region = result_manager_.get_last_defect(img_set.MoveIndex - 1);
                                            inspect_view_.set_result_img(img_set.Source[0], TotalDefect_region, img_set.MoveIndex);

                                            inspect_view_.set_result_map(img_set,
                                                                         result_manager_.get_defect_rgn(img_set.MoveIndex),
                                                                         position_model.get_locates(),
                                                                         result_manager_.get_dic_defect(img_set.MoveIndex),
                                                                         TotalDefect_region);
                                            //TotalDefect_region.Dispose();
                                        }

                                        inspect_view_.ResultStaticTable = result_manager_.StaticTable;

                                    }
                                    else
                                    {
                                        // Do nothing...
                                    }

                                    #endregion

                                    #region【視覺檢測-大圖拼接流程】 & 【人工覆判】

                                    /* 最後一個位置檢測結束後的處理
                                    *      - 儲存csv等統計結果檔案
                                    *      - 拼定位大圖
                                    *      - On-line 檢測要清除內部影像內容(計算位移編號用的內容)
                                    *      - 清除定位物件內部儲存內容
                                    */

                                    if (img_set.MoveIndex % move_bound == 0)
                                    {
                                        B_imagesFinished = true; // (20190604) Jeff Revised!

                                        if (advSet.IgnorePosAndIns == false)
                                        {
                                            #region Real Doing

                                            // 更新 所有未被檢測到的Cell中心點regions之座標
                                            bool b_CellDefect = FinalInspectParam.CellCenterInspectEnable;
                                            position_model.update_all_CellDefect(b_CellDefect); // (20181207) Jeff Revised!
                                            if (b_CellDefect)
                                            {
                                                result_manager_.SetMatchFailCount(position_model.GetMatchFailCount());
                                                result_manager_.update_current_defect_count((position_model.get_locates()).Count);
                                            }

                                            result_manager_.SetCellCount(position_model.total);//更新顆數

                                            /*if (advSet.B_BigMap) // 檢視大圖 (20190405) Jeff Revised!
                                            {
                                                //inspect_view_.set_big_map(position_model.get_big_map(img_lst));
                                                //inspect_view_.set_big_map2(position_model.get_big_map(img_lst), position_model.get_cellmap_affine(), position_model.get_defect_region(), position_model.get_capture_map()); // (20181025) Jeff Revised!
                                                inspect_view_.set_big_map3(position_model.get_big_map(img_lst)); // (20181116) Jeff Revised!
                                            }*/

                                            // 初始化人工覆判
                                            position_model.Initialize_Recheck(); // (20190719) Jeff Revised!

                                            DeltaSubstrateInspector.src.Modules.clsTriggerImg.trigger_img_.Clear();
                                            DeltaSubstrateInspector.src.Modules.clsTriggerImg.trigger_img_ = null;
                                            DeltaSubstrateInspector.src.Modules.clsTriggerImg.trigger_img_ = new List<HObject[]>();
                                            DeltaSubstrateInspector.src.Models.CaptureModel.Camera.trigger_count_ = 0;
                                            // 190102, andy
                                            DeltaSubstrateInspector.src.Models.CaptureModel.Camera.total_trigger_count_ = 0;

                                            // 更新瑕疵分類
                                            if (FinalInspectParam.b_NG_classification) // (20190720) Jeff Revised!
                                                result_manager_.update_DefectsClassify(position_model.Get_DefectsClassify());

                                            position_model.initialize();

                                            // Get 所有瑕疵座標 
                                            Cell_X_Count = position_model.Get_Cell_X_Count();
                                            Cell_Y_Count = position_model.Get_Cell_Y_Count();
                                            Cell_Total_Count = position_model.total;
                                            RealDefectPosList.Clear();
                                            RealDefectPosList.AddRange(position_model.get_locates());

                                            //runningThread.AOIStartProcReceive.WorkMode = "MEASURE";
                                            //runningThread.AOIStartProcReceive.ID = "1";
                                            //runningThread.AOIStartProcReceive.RecipeName = "defaultRecipe";
                                            //runningThread.GetUploadFinalString(false);

                                            // 設定 work result string                                           
                                            SetProcessResultString(position_model.Find_Shpe_Model_Success); // 190116, andy: 參考視覺定位是否成功
                                                                                                            //SetWorkResultString(true);
                                            SetWorkResultString(cal_rotation_angle_Success); // (20200114) Jeff Revised!

                                            // 181221, andy: 更新統計結果
                                            Invoke(dg_CombineVisionResult);

                                            // 統計結果存到 Excel (.csv)
                                            if (FinalInspectParam.b_saveCSV)
                                            {
                                                result_manager_.store_up_chip();
                                                InspectionView.ExportSummaryFile(DateTime.Now.ToString());
                                            }
                                            //else // (20190902) Jeff Revised!
                                            //    result_manager_.Set_InerCountID(); // 設定基板序號 (內部計數ID模式)

                                            // Set
                                            //PosInsState = 0; // 190213, andy: reset state
                                            //inspectionFinish = true; // 181016 andy
                                            //InspectionSuccess = position_model.Find_Shpe_Model_Success; // 190116, andy: 參考視覺定位是否成功

                                            // 放在 "inspectionFinish = true" 後以善用換下一片的平台移動時間 (20190405) Jeff Revised!
                                            if (advSet.B_BigMap || advSet.get_SaveTileImg) // 檢視大圖 or 儲存大圖 (20190405) Jeff Revised!
                                            {
                                                syslog.WriteLine(enMessageType.Info, "Start tile big map."); // (20200429) Jeff Revised!
                                                //inspect_view_.set_big_map3(position_model.get_big_map(img_lst)); // (20181116) Jeff Revised!
                                                inspect_view_.set_big_map3(position_model.get_big_map(img_concat, b_resize_first)); // (20190606) Jeff Revised!
                                                SaveTileImg();
                                                syslog.WriteLine(enMessageType.Info, "Tile big map ends."); // (20200429) Jeff Revised!
                                            }

                                            // 儲存瑕疵結果
                                            if (FinalInspectParam.b_SaveDefectResult) // (20200429) Jeff Revised!
                                            {
                                                syslog.WriteLine(enMessageType.Info, "Start saving defect result."); // (20200429) Jeff Revised!

                                                // 【載入】所有/瑕疵 Cell影像
                                                LocateMethod locate_method_ = position_model.get_locate_method();
                                                locate_method_.Compute_Dictionary_CellImage_From_CellReg_MoveIndex(inspect_view_.Get_ListMapItem(), FinalInspectParam.b_SaveAllCellImage, locate_method_.all_defect_id_);

                                                // 儲存此面瑕疵結果
                                                locate_method_.Save_DefectTest_Recipe(locate_method_.Path_DefectTest + "\\");

                                                syslog.WriteLine(enMessageType.Info, "Save defect result ends."); // (20200429) Jeff Revised!

                                                // 自動合併產生雙面工單
                                                if (FinalInspectParam.b_Auto_AB)
                                                {
                                                    if (FinalInspectParam.A_Recipe != "")
                                                    {
                                                        string SBID = ""; // 基板序號
                                                        if (B_SB_InerCountID)
                                                            SBID = SB_InerCountID;
                                                        else
                                                            SBID = SB_ID;
                                                        string dir = FinalInspectParam.Directory_SaveDefectResult + "\\" + PartNumber + "\\" + SBID + "\\";
                                                        // 判斷A面瑕疵工單是否存在
                                                        if (File.Exists(dir + FinalInspectParam.A_Recipe + "\\" + "DefectTest_Recipe.xml"))
                                                        {
                                                            #region 儲存雙面工單

                                                            syslog.WriteLine(enMessageType.Info, "Start saving AB recipe."); // (20200429) Jeff Revised!

                                                            DefectTest_AB_Recipe Recipe_AB = new DefectTest_AB_Recipe();
                                                            string directory_AB_Recipe = dir + "雙面合併工單" + "\\";

                                                            int Method = 2;
                                                            if (Method == 1) // 依數字1, 2, 3...依序命名
                                                            {
                                                                int i = 1;
                                                                while (true)
                                                                {
                                                                    if (Directory.Exists(directory_AB_Recipe + i.ToString()) == false)
                                                                    {
                                                                        directory_AB_Recipe += i.ToString();
                                                                        break;
                                                                    }
                                                                    i++;
                                                                }
                                                            }
                                                            else
                                                                directory_AB_Recipe += FinalInspectParam.ProductName;

                                                            Recipe_AB.Set_Params(directory_AB_Recipe, FinalInspectParam.ProductName, FinalInspectParam.A_Recipe, PartNumber, SBID, dir + FinalInspectParam.A_Recipe, locate_method_.Path_DefectTest, FinalInspectParam.B_UpDownInvert_FS, FinalInspectParam.B_LeftRightInvert_FS);
                                                            Recipe_AB.Save_DefectTest_AB_Recipe();

                                                            syslog.WriteLine(enMessageType.Info, "Save AB recipe ends."); // (20200429) Jeff Revised!

                                                            #endregion
                                                        }
                                                    }
                                                }
                                            }

                                            // Set // (20200429) Jeff Revised!
                                            PosInsState = 0; // 190213, andy: reset state
                                            inspectionFinish = true; // 181016 andy
                                            InspectionSuccess = position_model.Find_Shpe_Model_Success; // 190116, andy: 參考視覺定位是否成功

                                            #endregion
                                        }
                                        else
                                        {
                                            #region Bypass Doing

                                            DeltaSubstrateInspector.src.Modules.clsTriggerImg.trigger_img_.Clear();
                                            DeltaSubstrateInspector.src.Modules.clsTriggerImg.trigger_img_ = null;
                                            DeltaSubstrateInspector.src.Modules.clsTriggerImg.trigger_img_ = new List<HObject[]>();
                                            DeltaSubstrateInspector.src.Models.CaptureModel.Camera.trigger_count_ = 0;
                                            // 190102, andy
                                            DeltaSubstrateInspector.src.Models.CaptureModel.Camera.total_trigger_count_ = 0;

                                            // 設定 work result string
                                            SetProcessResultString(true);
                                            work_result_string = "OK";

                                            // 181221, andy: 更新統計結果
                                            Invoke(dg_CombineVisionResult);

                                            // Set 
                                            PosInsState = 0; // 190213, andy: reset state
                                            inspectionFinish = true; // 181016 andy
                                            InspectionSuccess = true; // 181016 andy

                                            #endregion
                                        }

                                        // Release memory (20190404) Jeff Revised!
                                        //for (int i = 0; i < img_lst.Count; i++)
                                        //{
                                        //    if (img_lst[i] != null)
                                        //    {
                                        //        img_lst[i].Dispose();
                                        //        img_lst[i] = null;
                                        //    }
                                        //}
                                        //img_lst.Clear();
                                        // (20190606) Jeff Revised!
                                        Extension.HObjectMedthods.ReleaseHObject(ref img_concat);

                                        // 離線檢測模式 (20190404) Jeff Revised!
                                        if (b_OffLine_test)
                                        {
                                            GC.Collect();
                                            GC.WaitForPendingFinalizers();
                                            syslog.WriteLine(enMessageType.Info, "GC finish"); // (20200429) Jeff Revised!
                                        }
                                    }

                                    #endregion

                                    #endregion

                                }

                            }
                            else
                            {

                                #region [雷射校正流程]

                                PosInsState = 3;
                                LaserPositionFinish = false;
                                LaserPositionSuccess = false;

                                // 加入影像
                                int IndexPerPos = 0; // 每個取像位置的第幾張影像
                                laser_position_model.laser_locateUseImgs_new.Add(img_set.Source[IndexPerPos]);

                                // 判斷與計算
                                if (img_set.MoveIndex == runningThread.TriggerCount)
                                {
                                    // calc
                                    laser_position_model.get_laser_locates_new(advSet.B_saveLaserPos, advSet.B_saveLaserPoss_draw); // (20190213) Jeff Revised!

                                    // get postion
                                    LaserFINDMarkPosList.Clear();
                                    LaserFINDMarkPosList.AddRange(laser_position_model.laser_mark_position_lst_);

                                    // 190213, andy
                                    Invoke(dg_ViPosResultUpdate);

                                    // reset buffer
                                    DeltaSubstrateInspector.src.Modules.clsTriggerImg.trigger_img_.Clear();
                                    DeltaSubstrateInspector.src.Modules.clsTriggerImg.trigger_img_ = null;
                                    DeltaSubstrateInspector.src.Modules.clsTriggerImg.trigger_img_ = new List<HObject[]>();
                                    DeltaSubstrateInspector.src.Models.CaptureModel.Camera.trigger_count_ = 0;
                                    // 190102, andy
                                    DeltaSubstrateInspector.src.Models.CaptureModel.Camera.total_trigger_count_ = 0;

                                    laser_position_model.initialize();

                                    // set flag
                                    PosInsState = 0; // 190213, andy: reset state
                                    LaserPositionFinish = true;
                                    LaserPositionSuccess = true;

                                }
                                else
                                {
                                    //string str = "目前已模擬取像張數: " + simTriggerCount.ToString() + "\n";
                                    //syslog.WriteLine(enMessageType.Info, str);
                                }

                                #endregion

                            }

                            #endregion


                            #region 釋放影像物件空間
                            // 釋放影像物件空間
                            img_set.release();
                            img_set = null;
                            //GC.Collect();
                            //GC.WaitForPendingFinalizers();
                            #endregion


                        }
                        catch (Exception ex)
                        {
                            string str = "EXMsg: " + ex.Message.ToString() + "PosInsState: " + PosInsState.ToString() + "," + "MoveIndex: " + img_set.MoveIndex.ToString();
                            syslog.WriteLine(enMessageType.Error, str);

                            #region 軟體例外處理

                            // 190103, andy       
                            if ((PosInsState == 1) || (PosInsState == 2))
                            {
                                SetProcessResultString(false);
                                SetWorkResultString(false);
                                //int testCount = capture_model_.get_img_queue().Count;
                                // if (testCount == 0)
                                //if (img_set.MoveIndex % move_bound == 0) // 190403, andy
                                if ((img_set.MoveIndex % move_bound == 0) && PosInsState != 1) // (20200114) Jeff Revised!
                                {
                                    #region 190403, andy: Reset buffer

                                    DeltaSubstrateInspector.src.Modules.clsTriggerImg.trigger_img_.Clear();
                                    DeltaSubstrateInspector.src.Modules.clsTriggerImg.trigger_img_ = null;
                                    DeltaSubstrateInspector.src.Modules.clsTriggerImg.trigger_img_ = new List<HObject[]>();
                                    DeltaSubstrateInspector.src.Models.CaptureModel.Camera.trigger_count_ = 0;
                                    // 190102, andy
                                    DeltaSubstrateInspector.src.Models.CaptureModel.Camera.total_trigger_count_ = 0;

                                    position_model.initialize();

                                    RealDefectPosList.Clear();

                                    #endregion

                                    Invoke(dg_CombineVisionResult);

                                    result_manager_.store_up_chip(); // 190403, andy

                                    inspectionFinish = true;      // 190417 andy
                                    InspectionSuccess = false;    // 190417 andy

                                }
                            }

                            if (PosInsState == 1) // 視覺定位
                            {
                                PositionFinish = true;       // 181016 andy
                                PositionSuccess = false;     // 181016 andy                           
                            }
                            else if (PosInsState == 2) // 視覺檢測
                            {
                                //inspectionFinish = true;      // 190417 andy
                                //InspectionSuccess = false;    // 190417 andy
                            }
                            else if (PosInsState == 3) // 雷射大小校正
                            {
                                LaserPositionFinish = true;
                                LaserPositionSuccess = false;
                            }

                            PosInsState = 0; // 190213, andy: reset state


                            #endregion

                            // 190403, andy
                            img_set.release();
                            img_set = null;

                        }
                        finally
                        { }

                        #endregion
                    }
                    else // 不做視覺定位 (20191216) Jeff Revised!
                    {
                        #region 不做視覺定位
                        
                        try
                        {

                            // 181120, andy:儲存取得的影像                        
                            if (inspect_view_.SaveImgOn == true)
                            {
                                StorageManager.write_source_img(img_set, advSet.B_InerCounterIDMode);
                            }
                            
                            if (img_set.MoveIndex == 1)
                            {
                                syslog.WriteLine(enMessageType.Info, "---------- Process images of first position ----------"); // (20200717) Jeff Revised!

                                // Get position                                            
                                MARKPOS_X1 = MARKPOS_Y1 = MARKPOS_X2 = MARKPOS_Y2 = -1;

                                // 建立 Cell Map (不做視覺定位)
                                if (!(position_model.initialize_NoPos()))
                                    throw new Exception("建立 Cell Map (不做視覺定位) 失敗!"); // 丟出例外狀況

                                // 181119, andy: 設定總chip數量, ex: MJ, 1287=33*39
                                result_manager_.setup_static_board_total(position_model.total);
                                result_manager_.SetCellCount(position_model.total); // 更新顆數 (20200118) Jeff Revised!

                                // Set process result string
                                SetProcessResultString(true);

                                PosInsState = 0; // 190213, andy: reset state
                                PositionFinish = true; // 181016 andy
                                PositionSuccess = true;
                                inspectionFinish = false; // 190227, andy: !!!! 重要 Reset !!!!
                                InspectionSuccess = false; // Chris
                                B_imagesFinished = false;
                            }

                            #region [視覺檢測流程]

                            if (PosInsState != 2)
                                result_manager_.Set_InerCountID(); // 設定基板序號 (內部計數ID模式)

                            PosInsState = 2;          // 181016 andy
                            inspectionFinish = false; // 181016 andy
                            InspectionSuccess = false; // 181016 andy
                            PositionFinish = false; // (20190417) Jeff Revised!

                            #region [視覺檢測-單Move檢測] 
                            
                            // 每一新料片開始檢測前的初始設定
                            if (img_set.MoveIndex == 1)
                            {
                                // (20190606) Jeff Revised!
                                if (img_concat != null)
                                {
                                    img_concat.Dispose();
                                    img_concat = null;
                                }
                                HOperatorSet.GenEmptyObj(out img_concat);

                                inspect_view_.release();
                                inspect_model_.initialize();
                                inspect_view_.setup_map(MovementPos.col, MovementPos.row); //20180530
                                RealDefectPosList.Clear(); // 190403, andy

                                // 181119, andy
                                Invoke(dg_NewChip);
                                
                                inspect_view_.set_big_map3(null); // (20181116) Jeff Revised!
                                inspect_view_.set_locate_method(position_model.get_locate_method()); // (20181116) Jeff Revised!

                                position_model.Clear_all_defects(); // 清除所有瑕疵區域及瑕疵座標 (20181112) Jeff Revised!
                                position_model.Dispose(); // 釋放記憶體 (20200429) Jeff Revised!

                                // 初始化瑕疵分類 (20190716) Jeff Revised!
                                position_model.Initialize_DefectsClassify(inspect_model_.Initialize_DefectsClassify());
                                
                                // 人工覆判 或 AI分圖
                                if (FinalInspectParam.b_Recheck || FinalInspectParam.b_AICellImg_Enable) // (20200429) Jeff Revised!
                                {
                                    clsStaticTool.DisposeAll(CellReg_MoveIndex_FS);
                                    CellReg_MoveIndex_FS.Clear();
                                    clsStaticTool.DisposeAll(DefReg_MoveIndex_FS);
                                    DefReg_MoveIndex_FS.Clear();
                                }

                                // 設定 儲存瑕疵結果 相關資訊
                                if (FinalInspectParam.b_SaveDefectResult) // (20200429) Jeff Revised!
                                {
                                    // 儲存瑕疵結果啟用時，則強制核取 AdvSet 中的 檢視大圖
                                    advSet.Set_checkBox_BigMap(true);

                                    // 設定瑕疵檔路徑
                                    string SBID = ""; // 基板序號
                                    if (B_SB_InerCountID)
                                        SBID = SB_InerCountID;
                                    else
                                        SBID = SB_ID;
                                    position_model.Set_Path_DefectTest(FinalInspectParam.Directory_SaveDefectResult + "\\" + PartNumber + "\\" + SBID + "\\" + ModuleName);

                                    // 設定 模組名稱、生產料號、序號及工單名稱
                                    LocateMethod locate_method_ = position_model.get_locate_method();
                                    locate_method_.productName = FinalInspectParam.ProductName;
                                    locate_method_.partNumber = PartNumber;
                                    locate_method_.sB_ID = SBID;
                                    locate_method_.moduleName = ModuleName;
                                }
                            }

                            if (advSet.IgnorePosAndIns == false)
                            {

                                // 儲存影像至img_concat，最後拼定位大圖用
                                int globalMapImagID = FinalInspectParam.GlobalMapImgID;
                                if (advSet.B_BigMap || advSet.get_SaveTileImg) // 檢視大圖 or 儲存大圖 (20190405) Jeff Revised!
                                {
                                    #region Method 1:
                                    // (20190611) Jeff Revised!
                                    if (!b_resize_first)
                                    {
                                        {
                                            HObject ExpTmpOutVar_0;
                                            HOperatorSet.ConcatObj(img_concat, img_set.Source[globalMapImagID], out ExpTmpOutVar_0);
                                            img_concat.Dispose();
                                            img_concat = ExpTmpOutVar_0;
                                        }
                                    }
                                    else
                                    {
                                        HObject img_resize;
                                        HOperatorSet.ZoomImageFactor(img_set.Source[globalMapImagID], out img_resize, position_model.get_locate_method().resize_, position_model.get_locate_method().resize_, "bilinear");
                                        {
                                            HObject ExpTmpOutVar_0;
                                            HOperatorSet.ConcatObj(img_concat, img_resize, out ExpTmpOutVar_0);
                                            img_concat.Dispose();
                                            img_concat = ExpTmpOutVar_0;
                                        }
                                        img_resize.Dispose();
                                    }
                                    #endregion
                                }

                                /* 執行檢測 */
                                MOVE_ID = img_set.MoveIndex.ToString(); // 181114, andy
                                inspect_model_.operator_execute(img_set.Source, by_pass_pos_.Contains(img_set.MoveIndex));

                                /* 更新 結果物件內容 (計算瑕疵統計結果) */
                                // 總瑕疵 (20190718) Jeff Revised!
                                result_manager_.update_chip(img_set,
                                                            inspect_model_.get_TotalResult(),
                                                            position_model.get_defect_locate(inspect_model_.get_TotalDefect_with_name(), img_set.MoveIndex),
                                                            position_model.get_defect_locate(inspect_model_.get_defect_with_name_New(), img_set.MoveIndex),
                                                            inspect_model_.get_dic_result());
                                result_manager_.update_current_defect_count((position_model.get_locates()).Count);

                                // 瑕疵分類統計
                                position_model.get_DefectsClassify_locate(inspect_model_.get_defect_with_name_New(), img_set.MoveIndex); // (20190717) Jeff Revised!

                                /* 更新 Cell Center Region */
                                if (FinalInspectParam.CellCenterInspectEnable)
                                    position_model.update_CellCenterReg(inspect_model_.get_CellRegion(), img_set.MoveIndex); // (20181207) Jeff Revised!

                                /* 更新 顯示內容 */
                                if (FinalInspectParam.dispWindow == "pictureBox") // (20190718) Jeff Revised!
                                {
                                    // Display result image in the inspect view 
                                    inspect_view_.set_result_img(result_manager_.get_last_defect_img(img_set.MoveIndex - 1));

                                    inspect_view_.set_result_map(img_set,
                                                                    result_manager_.get_defect_rgn(img_set.MoveIndex),
                                                                    position_model.get_locates(),
                                                                    result_manager_.get_dic_defect(img_set.MoveIndex));
                                }
                                else
                                {
                                    // Display result image in the inspect view 
                                    HObject TotalDefect_region = result_manager_.get_last_defect(img_set.MoveIndex - 1);
                                    inspect_view_.set_result_img(img_set.Source[0], TotalDefect_region, img_set.MoveIndex);

                                    inspect_view_.set_result_map(img_set,
                                                                    result_manager_.get_defect_rgn(img_set.MoveIndex),
                                                                    position_model.get_locates(),
                                                                    result_manager_.get_dic_defect(img_set.MoveIndex),
                                                                    TotalDefect_region);
                                    //TotalDefect_region.Dispose();
                                }

                                inspect_view_.ResultStaticTable = result_manager_.StaticTable;

                            }
                            else
                            {
                                // Do nothing...
                            }

                            #endregion

                            #region【視覺檢測-大圖拼接流程】 & 【人工覆判】

                            /* 最後一個位置檢測結束後的處理
                            *      - 儲存csv等統計結果檔案
                            *      - 拼定位大圖
                            *      - On-line 檢測要清除內部影像內容(計算位移編號用的內容)
                            *      - 清除定位物件內部儲存內容
                            */

                            if (img_set.MoveIndex % move_bound == 0)
                            {
                                B_imagesFinished = true; // (20190604) Jeff Revised!

                                if (advSet.IgnorePosAndIns == false)
                                {
                                    #region Real Doing

                                    // 更新 所有未被檢測到的Cell中心點regions之座標
                                    bool b_CellDefect = FinalInspectParam.CellCenterInspectEnable;
                                    position_model.update_all_CellDefect(b_CellDefect); // (20181207) Jeff Revised!
                                    if (b_CellDefect) // (20200118) Jeff Revised!
                                    {
                                        result_manager_.SetMatchFailCount(position_model.GetMatchFailCount());
                                        result_manager_.update_current_defect_count((position_model.get_locates()).Count);
                                    }

                                    result_manager_.SetCellCount(position_model.total);//更新顆數

                                    // 初始化人工覆判
                                    position_model.Initialize_Recheck(); // (20190719) Jeff Revised!

                                    DeltaSubstrateInspector.src.Modules.clsTriggerImg.trigger_img_.Clear();
                                    DeltaSubstrateInspector.src.Modules.clsTriggerImg.trigger_img_ = null;
                                    DeltaSubstrateInspector.src.Modules.clsTriggerImg.trigger_img_ = new List<HObject[]>();
                                    DeltaSubstrateInspector.src.Models.CaptureModel.Camera.trigger_count_ = 0;
                                    // 190102, andy
                                    DeltaSubstrateInspector.src.Models.CaptureModel.Camera.total_trigger_count_ = 0;

                                    // 更新瑕疵分類
                                    if (FinalInspectParam.b_NG_classification) // (20190720) Jeff Revised!
                                        result_manager_.update_DefectsClassify(position_model.Get_DefectsClassify());

                                    position_model.initialize();

                                    // Get 所有瑕疵座標 
                                    Cell_X_Count = position_model.Get_Cell_X_Count();
                                    Cell_Y_Count = position_model.Get_Cell_Y_Count();
                                    Cell_Total_Count = position_model.total;
                                    RealDefectPosList.Clear();
                                    RealDefectPosList.AddRange(position_model.get_locates());

                                    // 設定 work result string                                           
                                    SetProcessResultString(true); // 190116, andy: 參考視覺定位是否成功
                                    SetWorkResultString(true);

                                    // 181221, andy: 更新統計結果
                                    Invoke(dg_CombineVisionResult);

                                    // 統計結果存到 Excel (.csv)
                                    if (FinalInspectParam.b_saveCSV) // (20200118) Jeff Revised!
                                    {
                                        result_manager_.store_up_chip();
                                        InspectionView.ExportSummaryFile(DateTime.Now.ToString());
                                    }
                                    //else // (20190902) Jeff Revised!
                                    //    result_manager_.Set_InerCountID(); // 設定基板序號 (內部計數ID模式)

                                    // Set
                                    //PosInsState = 0; // 190213, andy: reset state
                                    //inspectionFinish = true; // 181016 andy
                                    //InspectionSuccess = position_model.Find_Shpe_Model_Success; // 190116, andy: 參考視覺定位是否成功

                                    // 放在 "inspectionFinish = true" 後以善用換下一片的平台移動時間 (20190405) Jeff Revised!
                                    if (advSet.B_BigMap || advSet.get_SaveTileImg) // 檢視大圖 or 儲存大圖 (20190405) Jeff Revised!
                                    {
                                        syslog.WriteLine(enMessageType.Info, "Start tile big map."); // (20200429) Jeff Revised!
                                        inspect_view_.set_big_map3(position_model.get_big_map(img_concat, b_resize_first)); // (20190606) Jeff Revised!
                                        SaveTileImg();
                                        syslog.WriteLine(enMessageType.Info, "Tile big map ends."); // (20200429) Jeff Revised!
                                    }

                                    // 儲存瑕疵結果
                                    if (FinalInspectParam.b_SaveDefectResult) // (20200429) Jeff Revised!
                                    {
                                        syslog.WriteLine(enMessageType.Info, "Start saving defect result."); // (20200429) Jeff Revised!

                                        // 【載入】所有/瑕疵 Cell影像
                                        LocateMethod locate_method_ = position_model.get_locate_method();
                                        locate_method_.Compute_Dictionary_CellImage_From_CellReg_MoveIndex(inspect_view_.Get_ListMapItem(), FinalInspectParam.b_SaveAllCellImage, locate_method_.all_defect_id_);

                                        // 儲存此面瑕疵結果
                                        locate_method_.Save_DefectTest_Recipe(locate_method_.Path_DefectTest + "\\");

                                        syslog.WriteLine(enMessageType.Info, "Save defect result ends."); // (20200429) Jeff Revised!

                                        // 自動合併產生雙面工單
                                        if (FinalInspectParam.b_Auto_AB)
                                        {
                                            if (FinalInspectParam.A_Recipe != "")
                                            {
                                                string SBID = ""; // 基板序號
                                                if (B_SB_InerCountID)
                                                    SBID = SB_InerCountID;
                                                else
                                                    SBID = SB_ID;
                                                string dir = FinalInspectParam.Directory_SaveDefectResult + "\\" + PartNumber + "\\" + SBID + "\\";
                                                // 判斷A面瑕疵工單是否存在
                                                if (File.Exists(dir + FinalInspectParam.A_Recipe + "\\" + "DefectTest_Recipe.xml"))
                                                {
                                                    #region 儲存雙面工單

                                                    syslog.WriteLine(enMessageType.Info, "Start saving AB recipe."); // (20200429) Jeff Revised!

                                                    DefectTest_AB_Recipe Recipe_AB = new DefectTest_AB_Recipe();
                                                    string directory_AB_Recipe = dir + "雙面合併工單" + "\\";

                                                    int Method = 2;
                                                    if (Method == 1) // 依數字1, 2, 3...依序命名
                                                    {
                                                        int i = 1;
                                                        while (true)
                                                        {
                                                            if (Directory.Exists(directory_AB_Recipe + i.ToString()) == false)
                                                            {
                                                                directory_AB_Recipe += i.ToString();
                                                                break;
                                                            }
                                                            i++;
                                                        }
                                                    }
                                                    else
                                                        directory_AB_Recipe += FinalInspectParam.ProductName;

                                                    Recipe_AB.Set_Params(directory_AB_Recipe, FinalInspectParam.ProductName, FinalInspectParam.A_Recipe, PartNumber, SBID, dir + FinalInspectParam.A_Recipe, locate_method_.Path_DefectTest, FinalInspectParam.B_UpDownInvert_FS, FinalInspectParam.B_LeftRightInvert_FS);
                                                    Recipe_AB.Save_DefectTest_AB_Recipe();

                                                    syslog.WriteLine(enMessageType.Info, "Save AB recipe ends."); // (20200429) Jeff Revised!

                                                    #endregion
                                                }
                                            }
                                        }
                                    }

                                    // Set // (20200429) Jeff Revised!
                                    PosInsState = 0; // 190213, andy: reset state
                                    inspectionFinish = true; // 181016 andy
                                    InspectionSuccess = position_model.Find_Shpe_Model_Success; // 190116, andy: 參考視覺定位是否成功

                                    #endregion
                                }
                                else
                                {
                                    #region Bypass Doing

                                    DeltaSubstrateInspector.src.Modules.clsTriggerImg.trigger_img_.Clear();
                                    DeltaSubstrateInspector.src.Modules.clsTriggerImg.trigger_img_ = null;
                                    DeltaSubstrateInspector.src.Modules.clsTriggerImg.trigger_img_ = new List<HObject[]>();
                                    DeltaSubstrateInspector.src.Models.CaptureModel.Camera.trigger_count_ = 0;
                                    // 190102, andy
                                    DeltaSubstrateInspector.src.Models.CaptureModel.Camera.total_trigger_count_ = 0;

                                    // 設定 work result string
                                    SetProcessResultString(true);
                                    work_result_string = "OK";

                                    // 181221, andy: 更新統計結果
                                    Invoke(dg_CombineVisionResult);

                                    // Set 
                                    PosInsState = 0; // 190213, andy: reset state
                                    inspectionFinish = true; // 181016 andy
                                    InspectionSuccess = true; // 181016 andy

                                    #endregion
                                }
                                        
                                Extension.HObjectMedthods.ReleaseHObject(ref img_concat);

                                // 離線檢測模式 (20190404) Jeff Revised!
                                if (b_OffLine_test)
                                {
                                    GC.Collect();
                                    GC.WaitForPendingFinalizers();
                                    syslog.WriteLine(enMessageType.Info, "GC finish"); // (20200429) Jeff Revised!
                                }
                            }

                            #endregion

                            #endregion
                                    

                            #region 釋放影像物件空間
                            // 釋放影像物件空間
                            img_set.release();
                            img_set = null;
                            //GC.Collect();
                            //GC.WaitForPendingFinalizers();
                            #endregion


                        }
                        catch (Exception ex)
                        {
                            string str = "EXMsg: " + ex.Message.ToString() + "PosInsState: " + PosInsState.ToString() + "," + "MoveIndex: " + img_set.MoveIndex.ToString();
                            syslog.WriteLine(enMessageType.Error, str);

                            #region 軟體例外處理

                            // 190103, andy       
                            if ((PosInsState == 1) || (PosInsState == 2))
                            {
                                SetProcessResultString(false);
                                SetWorkResultString(false);
                                //int testCount = capture_model_.get_img_queue().Count;
                                // if (testCount == 0)
                                //if (img_set.MoveIndex % move_bound == 0) // 190403, andy
                                if ((img_set.MoveIndex % move_bound == 0) && PosInsState != 1) // (20200114) Jeff Revised!
                                {
                                    #region 190403, andy: Reset buffer

                                    DeltaSubstrateInspector.src.Modules.clsTriggerImg.trigger_img_.Clear();
                                    DeltaSubstrateInspector.src.Modules.clsTriggerImg.trigger_img_ = null;
                                    DeltaSubstrateInspector.src.Modules.clsTriggerImg.trigger_img_ = new List<HObject[]>();
                                    DeltaSubstrateInspector.src.Models.CaptureModel.Camera.trigger_count_ = 0;
                                    // 190102, andy
                                    DeltaSubstrateInspector.src.Models.CaptureModel.Camera.total_trigger_count_ = 0;

                                    position_model.initialize();

                                    RealDefectPosList.Clear();

                                    #endregion

                                    Invoke(dg_CombineVisionResult);

                                    result_manager_.store_up_chip(); // 190403, andy

                                    inspectionFinish = true;      // 190417 andy
                                    InspectionSuccess = false;    // 190417 andy

                                }
                            }

                            if (PosInsState == 1) // 視覺定位
                            {
                                PositionFinish = true;       // 181016 andy
                                PositionSuccess = false;     // 181016 andy                           
                            }
                            else if (PosInsState == 2) // 視覺檢測
                            {
                                //inspectionFinish = true;      // 190417 andy
                                //InspectionSuccess = false;    // 190417 andy
                            }
                            else if (PosInsState == 3) // 雷射大小校正
                            {
                                LaserPositionFinish = true;
                                LaserPositionSuccess = false;
                            }

                            PosInsState = 0; // 190213, andy: reset state


                            #endregion

                            // 190403, andy
                            img_set.release();
                            img_set = null;

                        }
                        finally
                        {

                        }

                        #endregion
                    }

                }
                
                SpinWait.SpinUntil(() => false, 1);

            } // thread while end
            
        }

        /// <summary>
        /// 委派物件 ParameterizedThreadStart 之方法
        /// </summary>
        /// <param name="value"></param>
        private void Thread_Concat_tileImgs(object value) // (20190613) Jeff Revised!
        {
            try
            {
                //HObject tileImg = (HObject)value;
                var tileImg = (HObject)value;

                if (!b_resize_first)
                {
                    // 當目前執行緒執行這個方法時，會鎖住資源，其他執行緒無法存取，直到該執行緒工作完成
                    lock (this) // this 表示，目前執行緒所在的類別，也就是鎖住這個類別的資源
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(img_concat, tileImg, out ExpTmpOutVar_0);
                        img_concat.Dispose();
                        img_concat = ExpTmpOutVar_0;
                    }
                }
                else
                {
                    HObject img_resize;
                    HOperatorSet.ZoomImageFactor(tileImg, out img_resize, position_model.get_locate_method().resize_, position_model.get_locate_method().resize_, "bilinear");

                    // 當目前執行緒執行這個方法時，會鎖住資源，其他執行緒無法存取，直到該執行緒工作完成
                    lock (this) // this 表示，目前執行緒所在的類別，也就是鎖住這個類別的資源
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(img_concat, img_resize, out ExpTmpOutVar_0);
                        img_concat.Dispose();
                        img_concat = ExpTmpOutVar_0;
                    }
                    img_resize.Dispose();
                }

                //tileImg.Dispose(); // 不能釋放記憶體，否則會出問題!!!
            }
            catch
            { }
        }

        /// <summary>
        /// BackgroundWorker 之 DoWork 事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bw_DoWork_Concat_tileImgs(object sender, DoWorkEventArgs e) // (20190613) Jeff Revised!
        {
            BackgroundWorker Worker = (BackgroundWorker)sender;
            var tileImg = (HObject)e.Argument;

            /* 法1 */
            Thread_Concat_tileImgs(tileImg);
            //tileImg.Dispose(); // 不能釋放記憶體，否則會出問題!!!

            /* 法2 */
            //Thread_Concat_tileImgs(e.Argument);

            e.Result = "Success";

            //Worker.CancelAsync();
            //e.Cancel = true;
        }

        private void bw_RunWorkerCompleted_Concat_tileImgs(object sender, RunWorkerCompletedEventArgs e) // (20190613) Jeff Revised!
        {

        }

        private void SaveTileImg()
        {
            if (advSet.get_SaveTileImg)
            {
                try
                {
                    HTuple allDefect_id = new HTuple();
                    int DefectCount = position_model.get_locates().Count;
                    for (int i = 0; i < DefectCount; i++)
                    {
                        HOperatorSet.TupleConcat(allDefect_id.Clone(), position_model.get_locate_method().cellID_2_int(new Point(position_model.get_locates()[i].X, position_model.get_locates()[i].Y)), out allDefect_id);
                    }
                    HObject reg_All_DefectCell;
                    HOperatorSet.SelectObj(position_model.get_cellmap_affine(), out reg_All_DefectCell, allDefect_id);
                    string FilePath = ModuleParamDirectory + ImageFileDirectory + DateTime.Now.ToString("yyyyMMdd") + "\\";
                    string SBID = "";
                    if (advSet.B_InerCounterIDMode)
                        SBID = SB_InerCountID;
                    else
                        SBID = SB_ID;

                    clsSaveTileImgThread.clsPacket P = new clsSaveTileImgThread.clsPacket(inspect_view_.get_big_Map(), reg_All_DefectCell, FilePath, ModuleName, SBID, MOVE_ID, PartNumber, Locate_Method_FS.all_intersection_defect_);
                    if (ThreadSaveImg != null)
                        ThreadSaveImg.PushImg(P);
                }
                catch
                { }
            }
        }

        /// <summary>
        /// 跨執行續存取UI物件，必須使用委派! 否則使用執行檔時，會死當
        /// </summary>
        /// <returns></returns>
        public bool NewChip()
        {

            result_manager_.new_chip();

            return true;

        }

        public bool CombineVisionResult()
        {
            result_manager_.updat_currenct_result(process_result_string, work_result_string);            
            inspect_view_.AddInsResultToStaticInfo(process_result_string, work_result_string, position_model.get_locates().Count);
            return true;
        }

        public bool ViPosResultUpdate()
        {
            if (runningThread.WorkMode != "FIND")
            {
                if (advSet.B_savePos_draw == false) return false;

                // 190107, andy: Display mark-image
                Reviewer_Form.DisplayVisionPositionMarkImage(position_model.Now_Mark1_ResultImg, position_model.Now_Mark2_ResultImg, true);

                // 190107, andy: Display mark-image
                Reviewer_Form.DisplayVisionPositionMarkPos(position_model.mark_position_lst_f);
            }
            else
            {
                if (advSet.B_saveLaserPoss_draw == false) return false;

                foreach (HObject img in laser_position_model.Now_Mark_ResultImg)
                {
                    Reviewer_Form.DisplayLaserVisionPositionMarkImage(img, true);
                }

            }


            return true;
        }


        private string process_result_string = "";
        public string ProcessResultString
        {
            get { return process_result_string; }
        }
        public void SetProcessResultString(bool _InspectionSuccess)
        {
            process_result_string = (_InspectionSuccess == true) ? "SUCCESS" : "FAIL";
        }

        private string work_result_string = "";
        public string WorkResultString
        {
            get { return work_result_string; }
        }

        public void SetWorkResultString(bool b_WorkResultSuccess) // (20190719) Jeff Revised!
        {
            if (b_WorkResultSuccess)
            {
                int realNgCount = RealDefectPosList.Count;
                double realNgRatio = Convert.ToDouble(realNgCount) / Convert.ToDouble(Cell_Total_Count) * 100;

                // NG 比例小於等於門檻丟OK bin，否則丟NG bin
                if (realNgRatio <= FinalInspectParam.NGRatio)
                    work_result_string = "OK";
                else
                    work_result_string = "NG";
            }
            else
                work_result_string = "NG";

        }
        
        // Create New Inspect Recipe and Preference setting
        private void create_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Set up Motion, Light, System Excution, Model Operator
            using (Preference setting = new Preference())
            {
                //setting.set_class_camera = class_camera_;
                setting.ShowDialog();
                if (setting.model_saved())
                {
                    //  Show up the model on inspect view panel
                    inspect_view_.setup_model_panel(setting.get_inspect_models());

                    //  Set up operators from preference setting result
                    inspect_model_.OperatorCollection = setting.get_inspect_models();
                }
            }

            // Should be set in parameter
        }

        // Setup inspect operator content 
        private void model_param_setup(object sender, EventArgs e)
        {
            inspect_model_.operator_modify(inspect_view_.get_index());
            result_manager_.setup_static_board_head(inspect_model_.get_roles_names((PnlSide)0));
        }

        private void map_setup(object sender, EventArgs e)
        {
            using (MapSettingForm form = new MapSettingForm())
            {
                form.ShowDialog();
                if (form.Saved)
                {
                    initialize();
                    if (form.Saved)
                    {
                        MovementPos.bypass = form.Bypass;
                        inspect_model_.set_map_param(form.PosCnt, form.Row, form.Col, form.Bypass);
                    }
                }
            }
        }

        private void position_param_ok(object sender, EventArgs e)
        {
            if (inspect_model_.OperatorCollection.Count > 0)
            {
                inspect_model_.PositionInfo = position_view_.PosInfo;
                position_model.set_affine_method(position_view_.get_affine_method());
                position_model.set_locate_method(position_view_.get_locate_method());
                position_model.set_type_name(position_view_.PosInfo);
                position_model.load();
            }
            else
                MessageBox.Show("未有檢測模組，定位設定未寫入");
        }

        private void laser_position_param_ok(object sender, EventArgs e) // (20181031) Jeff Revised!
        {
            if (inspect_model_.OperatorCollection.Count > 0)
            {
                //inspect_model_.PositionInfo = laser_setting.laser_position_view_.LaserPosInfo;
                inspect_model_.LaserPositionInfo = laser_setting.laser_position_view_.LaserPosInfo;
                laser_position_model.set_locate_method(laser_setting.laser_position_view_.get_laser_locate_method());
                laser_position_model.set_type_name(laser_setting.laser_position_view_.LaserPosInfo);
                laser_position_model.load();
            }
            else
                MessageBox.Show("未有檢測模組，雷射校正設定未寫入");
        }
        
        // Excution and camera build
        //   @function : run_StripMenuItem1_Click : On-line inspection
        //   @function : simulation_ToolStripMenuItem_Click : Off- line inspection
        private void run_StripMenuItem1_Click(object sender, EventArgs e)
        {
            TimerCycleTest.Enabled = false;
            if (ModuleName == "")
            {
                MessageBox.Show("尚未選擇程式");
                return;
            }

            // 暫時忽略定位 (20180814) Jeff Revised!
            if (!position_model.Status)
            {
                MessageBox.Show("尚未完成定位設定");
                return;
            }

            //if (listen_queue_thread_.ThreadState != ThreadState.Background)
            if (!listen_queue_thread_.IsAlive)
                listen_queue_thread_.Start();

            // 開始等待取像           
            capture_model_.set_showup_window(inspect_view_.show_window());
            capture_model_.SetCameraRunMode(2); // 181128, andy
            capture_model_.excute("On-line");

           
            // 181121, andy
            if (!capture_model_.CameraInitOK)
            {
                // 開啟失敗..
                MessageBox.Show("相機初始化異常! 請確認: \n\n" + "1.相機是否有接上電腦? \n2.相機介面設定是否正確" );
                
            }
            else
            {
                // 開啟成功..
                //MessageBox.Show("相機開啟成功，請開始做走停拍動作~"); // (20180920) Jeff Revised!
                //run_StripMenuItem1.Enabled = false; // (20180920) Jeff Revised!
                //StopToolStripMenuItem.Enabled = true;
                advSet.SetContiGrab(false);
                NowState = enuState.Run;
                UpdateStateControl(NowState);
                InitState();
                //timer_InfoUpdate.Enabled = true;
                b_OffLine_test = false; // (20190404) Jeff Revised!

            }

            

        }

        private void InitState()
        {
            PosInsState = 0; // 0: ready, 1:定位狀態, 2:檢測演算法狀態 , 3:雷射校正驗算法狀態
            PositionFinish = false;    // 定位演算法是否完成
            PositionSuccess = false;   // 定位演算法是否成功
            inspectionFinish = false;  // 檢測演算法是否完成
            InspectionSuccess = false; // 檢測演算法是否成功
            LaserPositionFinish = false;    // 定位演算法是否完成
            LaserPositionSuccess = false;   // 定位演算法是否成功

            //inspect_view_.set_big_map3(position_model.get_big_map(img_lst)); // (20181116) Jeff Revised!
            inspect_view_.set_big_map3(null); // (20190405) Jeff Revised!

            DeltaSubstrateInspector.src.Modules.clsTriggerImg.trigger_img_.Clear();
            DeltaSubstrateInspector.src.Modules.clsTriggerImg.trigger_img_ = null;
            DeltaSubstrateInspector.src.Modules.clsTriggerImg.trigger_img_ = new List<HObject[]>();
            DeltaSubstrateInspector.src.Models.CaptureModel.Camera.trigger_count_ = 0;
            // 190102, andy
            DeltaSubstrateInspector.src.Models.CaptureModel.Camera.total_trigger_count_ = 0;

            position_model.initialize();
            laser_position_model.initialize();
            //OperateEngine.TRIM.release_ImageView_HObject(); // For 華新trim痕檢測 (20180906) Jeff Revised!
            GC.Collect(); // 190403, andy
            GC.WaitForPendingFinalizers(); // 190403, andy
            IOManager.SetOutputBit("ModuleOnline", true);
        }

        private void simulation_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            if (ModuleName == "")
            {
                MessageBox.Show("尚未選擇程式");
                return;
            }
           

            // 暫時忽略定位 (20180814) Jeff Revised!
            if (!position_model.Status)
            {
                MessageBox.Show("尚未完成定位設定");
                return;
            }

            if (capture_model_.get_img_queue().Count <= 0 && PosInsState == 0 && TT_pictureBox_DeltaIcon_now == "     待命  ")
            {

                //if (listen_queue_thread_.ThreadState != ThreadState.Background)
                if (!listen_queue_thread_.IsAlive)
                    listen_queue_thread_.Start();

                ToolStripMenuItem btn = sender as ToolStripMenuItem;
                string sim_type = (btn.Text == "Single") ? "Off-line-single" : "Off-line-all";
                ChangeRecipe(ModuleName);
                capture_model_.excute(sim_type);

                b_OffLine_test = true; // (20190404) Jeff Revised!
            }
        }

        // System action
        private void save_ToolStripMenuItem_Click(object sender, EventArgs e)
        {                    
            //Form_OnUserControlButtonClicked(sender,e);
        }

        private void StopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOManager.SetOutputBit("ModuleOnline", false);
            TimerCycleTest.Enabled = false;
            DeltaSubstrateInspector.src.Models.CaptureModel.Camera.close_camera();


            //inspect_view_.set_big_map(position_model.get_big_map(img_lst));
            //inspect_view_.set_big_map2(position_model.get_big_map(img_lst), position_model.get_cellmap_affine(), position_model.get_defect_region(), position_model.get_capture_map()); // (20181025) Jeff Revised!
            //inspect_view_.set_big_map3(position_model.get_big_map(img_lst)); // (20181116) Jeff Revised!
            inspect_view_.set_big_map3(null); // (20190405) Jeff Revised!

            DeltaSubstrateInspector.src.Modules.clsTriggerImg.trigger_img_.Clear();
            DeltaSubstrateInspector.src.Modules.clsTriggerImg.trigger_img_ = null;
            DeltaSubstrateInspector.src.Modules.clsTriggerImg.trigger_img_ = new List<HObject[]>();
            DeltaSubstrateInspector.src.Models.CaptureModel.Camera.trigger_count_ = 0;
            // 190102, andy
            DeltaSubstrateInspector.src.Models.CaptureModel.Camera.total_trigger_count_ = 0;

            position_model.initialize();
            laser_position_model.initialize();
            //OperateEngine.TRIM.release_ImageView_HObject(); // For 華新trim痕檢測 (20180906) Jeff Revised!
            GC.Collect(); // 190403, andy
            GC.WaitForPendingFinalizers(); // 190403, andy

            // Change button state
            //run_StripMenuItem1.Enabled = true;
            //StopToolStripMenuItem.Enabled = false;
            NowState = enuState.Stop;
            UpdateStateControl(NowState);
            OffLineType = enuOffLineType.None;
            advSet.SetContiGrab(true);
            UpdateUserLevelControl(pwModule.GetAccessLevel());
            
        }

        private void LoadRecipeForm()
        {
            string path = ModuleParamDirectory + RecipeFileDirectory;

            // 檢查執行檔目錄是否存在
            if (!System.IO.Directory.Exists(path))
            {
                MessageBox.Show("尚未建立: " + path);
                return;
            }

            string[] dirs = System.IO.Directory.GetDirectories(path);
            List<string> dirlist = new List<string>();
            foreach (string item in dirs)
            {
                dirlist.Add(System.IO.Path.GetFileNameWithoutExtension(item));
            }
            if (dirlist.Count > 0)
            {
                using (RecipeSelectForm form = new RecipeSelectForm(dirlist))
                {
                    form.OnUserControlButtonClicked_SaveAs += Form_OnUserControlButtonClicked_SaveAs;
                    form.OnUserControlButtonClicked_Save += Form_OnUserControlButtonClicked_Save;
                    form.SetNowRecipeName(ModuleName);
                    form.SetPartNumber(PartNumber);
                    form.ShowDialog();
                    if (form.is_set)
                    {
                        ModuleName = form.RecipeName;

                        // 181107, andy
                        inspect_model_ = null;
                        inspect_model_ = new InspectModel();

                        #region 181026, andy 載入recipe 動作
                        if (!ChangeRecipe(ModuleName)) return;

                        #endregion


                    }

                    // 190116, andy
                    PartNumber = form.PartNumber;
                    //label_PartNumber.Text = "生產料號: " + PartNumber;
                    label_PartNumber.Text = "LotNum: " + PartNumber; // (20200429) Jeff Revised!
                }
            }
            else
                MessageBox.Show("未有歷史紀錄，請先新增與儲存程式");
        }

        private void load_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadRecipeForm();
        }

        private void Form_OnUserControlButtonClicked_SaveAs(object sender, EventArgs e)
        {
            
            if (ModuleName == "")
            {
                MessageBox.Show("尚未指定程式名稱");
                return;
            }

            // !!!!!! 設定
            create_ToolStripMenuItem_Click(sender, e);

            // Save
            inspect_model_.save_operator();
            position_model.save();

        }

        private void Form_OnUserControlButtonClicked_Save(object sender, EventArgs e)
        {

            if (ModuleName == "")
            {
                MessageBox.Show("尚未指定程式名稱");
                return;
            }

            // Save
            inspect_model_.save_operator();
            position_model.save();

        }
        
        public bool invokeChangeRecipeAndUpdate(string nowRecipeName)
        {
            this.Invoke(delegateChangeRecipeUpdate, nowRecipeName);

            return true;
        }

        public bool ChangeRecipe(string nowRecipeName)
        {
            bool Success = false;
            label_RecipeName.Text = "程式名稱: ";

            ModuleName = nowRecipeName;

            #region 載入recipe 動作

            if (!inspect_model_.load()) return false;
            
            result_manager_.setup_static_board_head(inspect_model_.get_roles_names((PnlSide)0));
            inspect_view_.setup_model_panel(inspect_model_.OperatorCollection);
            initialize();
            

            position_model.set_type_name(inspect_model_.PositionInfo);
            position_model.load();
            Locate_Method_FS = position_model.get_locate_method(); // 每載入一次工單，都會去更新目前FileSystem內的locate_method_ (20190131) Jeff Revised!
            // 更新目前FileSystem內的參數 (20190719) Jeff Revised!
            //Locate_Method_FS.b_Defect_Classify = (FinalInspectParam.b_NG_classification == 1) ? true : false;
            Locate_Method_FS.b_Defect_Classify = FinalInspectParam.b_NG_classification;
            Locate_Method_FS.b_Defect_priority = FinalInspectParam.b_NG_priority;
            Locate_Method_FS.b_Defect_Recheck = FinalInspectParam.b_Recheck;

            // 改變圖像顯示視窗類型
            inspect_view_.Change_dispWindow(); // (20190718) Jeff Revised!

            // 281026, andy
            position_view_.UpdatePositionPara(inspect_model_.PositionInfo);

            // 181210, andy: 光源亮度切換
            if (LightManager.IsConnect() == true)
            {
                LightManager.SetLightOffset(LightOffSet);
                LightManager.SetMaxLigthValueIndex(LightMaxValueIndex.Light01_MaxVauleIndex, LightMaxValueIndex.Light02_MaxVauleIndex, LightMaxValueIndex.Light03_MaxVauleIndex, LightMaxValueIndex.Light04_MaxVauleIndex);
                if (LightOffSet)
                    LightManager.OpenLightValue(LightValue.Light01_Vaule, LightValue.Light02_Vaule, LightValue.Light03_Vaule, LightValue.Light04_Vaule);
                else
                    LightManager.SetLightValue(LightValue.Light01_Vaule, LightValue.Light02_Vaule, LightValue.Light03_Vaule, LightValue.Light04_Vaule);

                advSet.UpdateLightControlParam(true);
            }
            else
            {
                advSet.UpdateLightControlParam(false);
            }

            // 181220, andy: 設定雷射打標測試頁面的 X, Y          
            laser_setting.laserPos_view1_.SetXYCount(position_model.Get_Cell_X_Count(), position_model.Get_Cell_Y_Count());
            laser_setting.laserPos_view2_.SetXYCount(position_model.Get_Cell_X_Count(), position_model.Get_Cell_Y_Count());

            // 181220, andy: 雷射大小校正
            laser_position_model.set_type_name(inspect_model_.LaserPositionInfo);
            laser_position_model.load();
            laser_setting.laser_position_view_.UpdatePositionPara(inspect_model_.LaserPositionInfo);



            #endregion
            label_RecipeName.Text = "程式名稱: " + ModuleName;
            //label_PartNumber.Text = "生產料號: " + PartNumber; // (20200429) Jeff Revised!
            label_PartNumber.Text = "LotNum: " + PartNumber; // (20200429) Jeff Revised!

            Success = true;
            return Success;
        }

        private void initialize()
        {
            inspect_view_.setup_map(MovementPos.col, MovementPos.row);
            result_manager_.MoveBound = MovementPos.col * MovementPos.row;
            move_bound = MovementPos.col * MovementPos.row;
            set_by_pass();
        }

        private void Release()
        {
            DeltaSubstrateInspector.src.Models.CaptureModel.Camera.close_camera();
            DeltaSubstrateInspector.src.Modules.NewInspectModule.InductanceInsp.InductanceInspRole.GetSingleton().Dispose();
            if (SaveAOICellImgThread != null)
            {
                SaveAOICellImgThread.Dispose();
                SaveAOICellImgThread = null;
            }
            DeltaSubstrateInspector.src.Modules.NewInspectModule.ResistPanel.ResistPanelRole.GetSingleton().Dispose(); // (20181228) Jeff Revised!
            if (ThreadSaveImg != null)
            {
                ThreadSaveImg.Dispose();
                ThreadSaveImg = null;
            }
        }

        private void set_by_pass()
        {
            if (MovementPos.bypass != "")
            {
                string[] str = MovementPos.bypass.Split(',');
                for (int i = 0; i < str.Length; i++)
                {
                    if (is_number(str[i]))
                    {
                        by_pass_pos_.Add(Int32.Parse(str[i]));
                    }
                    else
                    {
                        MessageBox.Show("檢測 by pass 位置設定格式錯誤");
                    }
                }
            }
        }

        private bool is_number(string str)
        {
            try
            {
                int i = Convert.ToInt32(str);
                return true;
            }
            catch
            {
                return false;
            }

        }
        private void exit_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AOIMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveLastRecipeName();
            Release();
            Environment.Exit(0);
        }

        private void account_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (WorkSheetForm form = new WorkSheetForm(ModuleName))
            {
                form.ShowDialog();
                if (form.Saved)
                {
                    ModuleName = form.WorkNumber;
                   // ModuleType = form.WorkType; // 181026,andy
                }
            }
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("確認是否關閉程式?", "此動作會關閉程式,請確認是否執行?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogResult != DialogResult.Yes)
            {
                return;
            }
            this.Close();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void AdvSet_ContiGrab(object sender, EventArgs e)
        {
            if (advSet.GetLiveShow() == false)
            {

                #region 開始連續取像

                // 開啟相機
                capture_model_.set_showup_smwindow(advSet.GetCamShow2());
                capture_model_.SetCameraRunMode(1);
                capture_model_.excute("On-line");
               

                // 181121, andy
                if (!capture_model_.CameraInitOK)
                {
                    MessageBox.Show("相機初始化異常! 請確認: \n\n" + "1.相機是否有接上電腦? \n" + "2.相機介面(CameraInterface)是否正確? \n"); // (20190620) Jeff Revised!

                    // 開啟失敗
                    advSet.SetCameraInitOK(false);
                    

                }
                else
                {
                    // 開啟成功..
                    advSet.SetCameraInitOK(true);
                    advSet.SetLiveShow(true);

                    // 無法進行線上檢測
                    run_StripMenuItem1.Enabled = false;

                    // 預防快速點擊造成當機
                    Thread.Sleep(100);

                }

                #endregion

            }
            else
            {

                #region 關閉連續取像
               
                // Close camera
                DeltaSubstrateInspector.src.Models.CaptureModel.Camera.close_camera();

                // 設定
                advSet.SetLiveShow(false);

                // 可進行線上檢測
                run_StripMenuItem1.Enabled = true; 

                #endregion

            }

        }

        private void AdvSet_connectCamera(object sender, EventArgs e) // (20190620) Jeff Revised!
        {
            if (advSet.Get_b_connected() == false)
            {
                #region 連接相機
                // 連接相機
                capture_model_.set_showup_smwindow(advSet.GetCamShow2());
                capture_model_.SetCameraRunMode(1);
                capture_model_.excute_connectNoCapture("On-line");
                
                if (!capture_model_.CameraInitOK)
                {
                    MessageBox.Show("相機初始化異常! 請確認: \n\n" + "1.相機是否有接上電腦? \n" + "2.相機介面(CameraInterface)是否正確? \n"); // (20190620) Jeff Revised!

                    // 開啟失敗
                    advSet.SetCameraInitOK(false);
                }
                else
                {
                    // 開啟成功..
                    advSet.SetCameraInitOK(true);
                    advSet.Set_connectCamera(true);

                    // 無法進行線上檢測
                    run_StripMenuItem1.Enabled = false;

                    // 預防快速點擊造成當機
                    Thread.Sleep(100);
                }
                #endregion
            }
            else
            {
                #region 關閉相機
                // Close camera
                DeltaSubstrateInspector.src.Models.CaptureModel.Camera.close_camera();

                // 設定
                advSet.Set_connectCamera(false);

                // 可進行線上檢測
                run_StripMenuItem1.Enabled = true;
                #endregion
            }
        }

        private void exit_ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("確認是否關閉程式?", "此動作會關閉程式,請確認是否執行?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogResult != DialogResult.Yes)
            {
                return;
            }
            this.Close();
        }

        private void timer_InitCheck_Tick(object sender, EventArgs e)
        {
            timer_InitCheck.Enabled = false;

            if (BypassLightControl)
                return;

            if (LightManager.IsConnect() == false)
            {
                advSet.UpdateConnectedControl();
                MessageBox.Show("光源模組初始化異常, 請確認\n 1.電腦與光源控制器連線狀態\n2.光源控制是否正常");
            }
        }

        private void SysModuletoolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        int nowA = 255; // 透明度: 0~255
        int dir = 1;    // 呼吸燈方向: 1: 遞減, -1: 遞增
        int step = 25;  // 呼吸燈速度: 數值越大越快

        private void timer_InfoUpdate_Tick(object sender, EventArgs e)
        {
            inspect_view_.UpdateInfo(DeltaSubstrateInspector.src.Models.CaptureModel.Camera.total_trigger_count_);

           
            if (PosInsState == 1) // 視覺定位
            {
                
                if(laser_setting.laserPos_view1_.GetLaserPostionTest()== true || laser_setting.laserPos_view2_.GetLaserPostionTest() == true)
                {
                    TT_pictureBox_DeltaIcon_now = "     視覺定位:雷射打標測試  ";
                    pictureBox_DeltaIcon.BackColor = (pictureBox_DeltaIcon.BackColor == Color.Transparent) ? Color.Orange : Color.Transparent;
                }
                else
                {
                    TT_pictureBox_DeltaIcon_now = "     視覺定位  ";
                    pictureBox_DeltaIcon.BackColor = (pictureBox_DeltaIcon.BackColor == Color.Transparent) ? Color.LawnGreen : Color.Transparent;

                }

            }
            else if (PosInsState == 2) // 視覺檢測
            {
                if (laser_setting.laserPos_view1_.GetLaserPostionTest() == true || laser_setting.laserPos_view2_.GetLaserPostionTest() == true)
                {
                    TT_pictureBox_DeltaIcon_now = "     視覺檢測:雷射打標測試  ";
                    pictureBox_DeltaIcon.BackColor = (pictureBox_DeltaIcon.BackColor == Color.Transparent) ? Color.Orange : Color.Transparent;
                }
                else
                {
                    TT_pictureBox_DeltaIcon_now = "     視覺檢測  ";
                    pictureBox_DeltaIcon.BackColor = (pictureBox_DeltaIcon.BackColor == Color.Transparent) ? Color.LawnGreen : Color.Transparent;
                }
                          

            }
            else if (PosInsState == 3) // 雷射大小校正
            {
                TT_pictureBox_DeltaIcon_now = "     雷射校正  ";
                pictureBox_DeltaIcon.BackColor = (pictureBox_DeltaIcon.BackColor == Color.Transparent) ? Color.OrangeRed : Color.Transparent;
            }
            else
            {
                TT_pictureBox_DeltaIcon_now = "     待命  ";
                pictureBox_DeltaIcon.BackColor = Color.Transparent;

                /*
                #region 模擬呼吸燈
                pictureBox_DeltaIcon.BackColor = Color.FromArgb(nowA, 0, 0, 150);

                if (dir == 1 && (nowA <= 255 && nowA > 0))
                {
                    nowA -= step;
                    nowA = (nowA <= 0) ? 0 : nowA;
                }
                else if (nowA <= 0)
                {
                    dir = -1;
                    nowA += step;
                }
                else if(dir == -1 && (nowA < 255 && nowA > 0))
                {
                    nowA += step;
                    nowA = (nowA > 255) ? 255 : nowA;
                }
                else if(nowA==255)
                {
                    dir = 1;
                    nowA -= step;
                }

                #endregion
                */

            }

        }

        ToolTip TT_pictureBox_DeltaIcon = new ToolTip();
        string TT_pictureBox_DeltaIcon_now = "";
        private void pictureBox_DeltaIcon_MouseHover(object sender, EventArgs e)
        {

            TT_pictureBox_DeltaIcon.Show(TT_pictureBox_DeltaIcon_now, pictureBox_DeltaIcon);
            
        }

        private void AOIMainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F6 && e.Modifiers == Keys.Control)
            {
                pwModule.SetVenderAccessLevel();
            }
        }

        private void 登入管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pwModule.Show();
        }

        private void 登出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pwModule.ResetAccessLevel();
        }

        private void file_ToolStripMenuItem_MouseDown(object sender, MouseEventArgs e)
        {
            
        }

        private void file_ToolStripMenuItem_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        private void file_ToolStripMenuItem_MouseHover(object sender, EventArgs e)
        {
            
        }

        private void file_ToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            
        }

        private void file_ToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
           
        }

        private void file_ToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            file_ToolStripMenuItem.ForeColor = Color.DarkBlue;
        }

        private void file_ToolStripMenuItem_DropDownClosed(object sender, EventArgs e)
        {
            file_ToolStripMenuItem.ForeColor = Color.White;
        }

        private void xu6vu04ToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            xu6vu04ToolStripMenuItem.ForeColor = Color.DarkBlue;
        }

        private void xu6vu04ToolStripMenuItem_DropDownClosed(object sender, EventArgs e)
        {
            xu6vu04ToolStripMenuItem.ForeColor = Color.White;
        }

        private void SysModuletoolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            SysModuletoolStripMenuItem.ForeColor = Color.DarkBlue;
        }

        private void SysModuletoolStripMenuItem_DropDownClosed(object sender, EventArgs e)
        {
            SysModuletoolStripMenuItem.ForeColor = Color.White;
        }

        private void label_PartNumber_Click(object sender, EventArgs e)
        {
            WorkSheetForm MyForm = new WorkSheetForm("生產料號", 1, PartNumber); // (20200429) Jeff Revised!

            MyForm.ShowDialog();

            if (!MyForm.Saved)
            {
                return;
            }
            if (string.IsNullOrEmpty(MyForm.NewRecipeName))
            {
                MessageBox.Show("請先輸入生產料號");
                return;
            }

            PartNumber = MyForm.NewRecipeName;
            //label_PartNumber.Text = "生產料號: " + PartNumber;
            label_PartNumber.Text = "LotNum: " + PartNumber; // (20200429) Jeff Revised!
        }

        private void label_RecipeName_Click(object sender, EventArgs e)
        {
            LoadRecipeForm();
        }

        private void cycleTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ModuleName == "")
            {
                MessageBox.Show("尚未選擇程式");
                return;
            }

            if (!position_model.Status)
            {
                MessageBox.Show("尚未完成定位設定");
                return;
            }
            SimulationCamera.FS = "";
            OpenFileDialog open_file_dialog = new OpenFileDialog();
            if (open_file_dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            { 
                return;
            }
            SimulationCamera.FS = open_file_dialog.FileName;

            if (!string.IsNullOrEmpty(SimulationCamera.FS))
            {
                run_StripMenuItem1.Enabled = false;
                StopToolStripMenuItem.Enabled = true;
                OffLineType = enuOffLineType.Cycle;
                TimerCycleTest.Enabled = true;
            }

            b_OffLine_test = true; // (20190404) Jeff Revised!
        }
        public string ForderPath = "";
        public int Index = 0;

        private void labUserLevel_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
            {
                pwModule.Show();
            }
            else if (me.Button == MouseButtons.Right)
            {
                pwModule.ResetAccessLevel();
            }
        }

        private void 權限設定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormPermissionSetting MyForm = new FormPermissionSetting(PermissionSetting, pwModule.GetAccessLevel());

            MyForm.ShowDialog();

            if (MyForm.GetbSave())
            {
                PermissionSetting = MyForm.GetParam();
                UpdateUserLevelControl(pwModule.GetAccessLevel());
            }
        }

        private void btnMinForm_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;  // 設定表單最小化 
        }

        /// <summary>
        /// 轉換語言 (任何語言轉中文)
        /// </summary>
        private void ResetLanguage() // (20200214) Jeff Revised!
        {
            clsLanguage.clsLanguage.UpdateRestoreLanguageLib();

            //clsLanguage.clsLanguage.SetLanguateToControls(this); // (20200214) Jeff Revised!
            clsLanguage.clsLanguage.SetLanguateToControls(this.panel1); // (20200214) Jeff Revised!
            clsLanguage.clsLanguage.SetLanguateToControls(this.tab_table_); // (20200214) Jeff Revised!
            clsLanguage.clsLanguage.SetLanguateToControls(inspect_view_); // (20200214) Jeff Revised!
            clsLanguage.clsLanguage.SetLanguateToControls(position_view_, true); // (20200214) Jeff Revised!
            clsLanguage.clsLanguage.SetLanguateToControls(laser_setting); // (20200214) Jeff Revised!
            clsLanguage.clsLanguage.SetLanguateToControls(advSet); // (20200214) Jeff Revised!

            clsLanguage.clsLanguage.RefreshLib();
        }

        /// <summary>
        /// 轉換語言 (任何語言轉其他語言)
        /// </summary>
        private void ChangeLanguage() // (20200214) Jeff Revised!
        {
            clsLanguage.clsLanguage.UpdateLanguageLib();

            //clsLanguage.clsLanguage.SetLanguateToControls(this); // (20200214) Jeff Revised!
            clsLanguage.clsLanguage.SetLanguateToControls(this.panel1); // (20200214) Jeff Revised!
            clsLanguage.clsLanguage.SetLanguateToControls(this.tab_table_); // (20200214) Jeff Revised!
            clsLanguage.clsLanguage.SetLanguateToControls(inspect_view_); // (20200214) Jeff Revised!
            clsLanguage.clsLanguage.SetLanguateToControls(position_view_, true); // (20200214) Jeff Revised!
            clsLanguage.clsLanguage.SetLanguateToControls(laser_setting); // (20200214) Jeff Revised!
            clsLanguage.clsLanguage.SetLanguateToControls(advSet); // (20200214) Jeff Revised!

            clsLanguage.clsLanguage.RefreshLib();
        }

        private void 中文ToolStripMenuItem_Click_1(object sender, EventArgs e) // (20200118) Jeff Revised!
        {
            中文ToolStripMenuItem.Checked = true;
            英文ToolStripMenuItem.Checked = false;
            Language = "Chinese"; // (20200214) Jeff Revised!

            /* 特殊字元先刪除 (因為無法存入INI檔) */
            if (run_StripMenuItem1.Text == "執行 ▶")
                run_StripMenuItem1.Text = run_StripMenuItem1.Text.Substring(0, 2);
            else if (run_StripMenuItem1.Text == "Run ▶")
                run_StripMenuItem1.Text = run_StripMenuItem1.Text.Substring(0, 3);

            clsIniFile iniSystem = new clsIniFile(clsData.g_strSystemIniFilePath); // clsData.g_strSystemIniFilePath = Application.StartupPath + "\\INI\\System.ini"
            ResetLanguage();
            iniSystem.WriteValue("System", "Language", "Chinese"); // 儲存目前使用之語言種類

            /* 特殊字元補回 (因為無法存入INI檔) */
            run_StripMenuItem1.Text += " ▶";
        }

        private void 英文ToolStripMenuItem_Click_1(object sender, EventArgs e) // (20200214) Jeff Revised!
        {
            中文ToolStripMenuItem.Checked = false;
            英文ToolStripMenuItem.Checked = true;
            Language = "English"; // (20200214) Jeff Revised!

            /* 特殊字元先刪除 (因為無法存入INI檔) */
            if (run_StripMenuItem1.Text == "執行 ▶")
                run_StripMenuItem1.Text = run_StripMenuItem1.Text.Substring(0, 2);
            else if (run_StripMenuItem1.Text == "Run ▶")
                run_StripMenuItem1.Text = run_StripMenuItem1.Text.Substring(0, 3);

            clsIniFile iniSystem = new clsIniFile(clsData.g_strSystemIniFilePath); // clsData.g_strSystemIniFilePath = Application.StartupPath + "\\INI\\System.ini"
            ResetLanguage();
            ChangeLanguage();
            iniSystem.WriteValue("System", "Language", "English"); // 儲存目前使用之語言種類

            /* 特殊字元補回 (因為無法存入INI檔) */
            run_StripMenuItem1.Text += " ▶";
        }

        private void TimerCycleTest_Tick(object sender, EventArgs e)
        {
            if (capture_model_.get_img_queue().Count <= 0 && PosInsState == 0 && TT_pictureBox_DeltaIcon_now == "     待命  ")
            {
                string sim_Type;
                if (OffLineType == enuOffLineType.Cycle)
                    sim_Type = "Off-line-Cycle";
                else if (OffLineType == enuOffLineType.Mult)
                    sim_Type = "Off-line-Mult";
                else
                {
                    sim_Type = "";
                    return;
                }
                if (OffLineType == enuOffLineType.Mult)
                {
                    List<string> PathList = new List<string>(Directory.EnumerateDirectories(ForderPath));
                    if (Index >= PathList.Count)
                    {
                        OffLineType = enuOffLineType.None;
                        ForderPath = "";
                        Index = 0;
                        TimerCycleTest.Enabled = false;
                        return;
                    }
                    List<string> FileList = new List<string>(Directory.GetFiles(PathList[Index]));
                    if (FileList.Count <= 0)
                    {
                        OffLineType = enuOffLineType.None;
                        ForderPath = "";
                        Index = 0;
                        TimerCycleTest.Enabled = false;
                        return;
                    }

                    SimulationCamera.FS = FileList[0];
                }
                ChangeRecipe(ModuleName);

                if (!listen_queue_thread_.IsAlive)
                    listen_queue_thread_.Start();


                Index++;
                capture_model_.excute(sim_Type);
            }
        }

        private void 多組測試ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ModuleName == "")
            {
                MessageBox.Show("尚未選擇程式");
                return;
            }

            if (!position_model.Status)
            {
                MessageBox.Show("尚未完成定位設定");
                return;
            }
            ForderPath = "";
            Index = 0;

            FolderBrowserDialog open_file_dialog = new FolderBrowserDialog();
            
            if (open_file_dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            string FilePath = open_file_dialog.SelectedPath + "\\";
            ForderPath = FilePath;

            if (!string.IsNullOrEmpty(ForderPath))
            {
                run_StripMenuItem1.Enabled = false;
                StopToolStripMenuItem.Enabled = true;
                OffLineType = enuOffLineType.Mult;
                TimerCycleTest.Enabled = true;
            }

            b_OffLine_test = true;
        }
    }
}
