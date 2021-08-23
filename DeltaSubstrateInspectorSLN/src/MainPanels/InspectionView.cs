using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DeltaSubstrateInspector.UIControl;
using DeltaSubstrateInspector.src.Models;
using DeltaSubstrateInspector.src.InspectionForms.ParamPanels;
using DeltaSubstrateInspector.src.Modules.InspectModule;
using HalconDotNet;
using Extension;
using DeltaSubstrateInspector.src.Modules.ResultModule;
using DeltaSubstrateInspector.src.Modules;
using DeltaSubstrateInspector.src.Modules.PositionModule;
using DeltaSubstrateInspector.src.PositionMethod.LocateMethod.Location; // (20181011) Jeff Revised!
using static DeltaSubstrateInspector.FileSystem.FileSystem; // (20190604) Jeff Revised!
using System.Threading; // (20190604) Jeff Revised!
using DeltaSubstrateInspector.FileSystem;
using System.IO;
using System.Diagnostics;

namespace DeltaSubstrateInspector.src.MainPanels
{
    public partial class InspectionView : UserControl
    {
        public EventHandler model_item_clicked;
        public EventHandler map_setting_clicked;
        private DefectMap defect_map_;
        private int selected_item_ = 0;
        private HObject big_map_ = null;
        private HObject big_map2_ = null; // (20181011) Jeff Revised!
        private LocateMethod locate_method_ = new LocateMethod(); // (20181116) Jeff Revised!
        //private HObject Cellmap_affine_ = null; // (20181011) Jeff Revised!
        //private HObject Defect_region_ = null, Capture_map_ = null; // (20181025) Jeff Revised!
        private bool save_img_on_ { get; set; } = false;

        // 生產統計
        private int numberOfInspection;       // 檢測總"片"數//
        private int numberOfInspectionOK;     // OK總片數
        private int numberOfInspectionNG;     // NG總片數
        private int numberOfInspectionCellNG; // 不良總顆數

        // AOIMainForm (20190402) Jeff Revised!
        private AOIMainForm mainForm;

        private BackgroundWorker bw; // (20190604) Jeff Revised!

        //190625, andy
        public delegate void delegate_RemoteMotor(int SendPositionID);
        public delegate_RemoteMotor dg_RemoteMotor;

        public class clsExportDefectData
        {
            public string Name;
            public int Count = 0;
            public double DefectYield = 0;

            public clsExportDefectData() { }
        }

        public class clsExportCSVParam
        {
            public string Date;
            public string LotID;
            public string ParamName;
            public string StartTime;
            public string EndTime;
            public int TotalCount = 0;
            public int NGCount = 0;
            public double Yield = 0;
            public int CellNGCount = 0;
            public int TotalCellCount = 0;
            public int TotalMatchFailCount = 0;
            public TimeSpan TotalTime = new TimeSpan(0, 0, 0);
            public List<clsExportDefectData> ExportDataList = new List<clsExportDefectData>();

            public clsExportCSVParam() { }

            public bool SummaryResult(List<string> OrgDataList)
            {
                bool Res = false;

                if (OrgDataList.Count <= 1)
                    return Res;
                try
                {
                    #region 統計總數量

                    double YSum = 0.0;
                    string[] HeaderString = OrgDataList[0].Split(',');
                    string[] FirstSPLITString = null;
                    for (int i = 1; i < OrgDataList.Count; i++)
                    {
                        if (OrgDataList[i].Split(',')[8] == "SUCCESS")
                        {
                            FirstSPLITString = OrgDataList[i].Split(',');
                            break;
                        }
                    }

                    TotalCellCount = int.Parse(FirstSPLITString[3]);

                    #region Defect列表

                    int DefectTypeCount = FirstSPLITString.Length - 13;

                    for (int j = 13; j < DefectTypeCount + 13; j++)
                    {
                        clsExportDefectData DefectData = new clsExportDefectData();
                        DefectData.Name = HeaderString[j];
                        this.ExportDataList.Add(DefectData);
                    }

                    #endregion

                    for (int i = 1; i < OrgDataList.Count; i++)
                    {
                        string[] SPLITString = OrgDataList[i].Split(',');

                        #region 基本資訊

                        if (i == 1)
                        {
                            this.TotalCount = OrgDataList.Count - 1;
                            this.Date = SPLITString[0];
                            this.LotID = SPLITString[1];
                            this.ParamName = SPLITString[2];
                            this.StartTime = SPLITString[5];
                        }
                        if (i == OrgDataList.Count - 1)
                        {
                            this.EndTime = SPLITString[6];
                        }
                        if (!string.IsNullOrEmpty(SPLITString[5]) && !string.IsNullOrEmpty(SPLITString[6]))
                        {
                            DateTime Start_Time, End_Time;
                            if (DateTime.TryParse(SPLITString[5], out Start_Time) && DateTime.TryParse(SPLITString[6], out End_Time))
                            {
                                this.TotalTime += End_Time.Subtract(Start_Time);
                            }

                        }

                        #endregion

                        #region NG片數量

                        if (SPLITString[9] == "NG")
                        {
                            this.NGCount++;
                            if (SPLITString[8] == "FAIL" || SPLITString[8] == "Fail")
                            {
                                continue;
                            }
                        }

                        #endregion

                        #region 總良率
                        YSum += double.Parse(SPLITString[10]);
                        #endregion

                        #region 總NG顆數

                        this.CellNGCount += int.Parse(SPLITString[11]);
                        this.TotalMatchFailCount += int.Parse(SPLITString[12]);

                        #endregion

                        for (int DC = 0; DC < ExportDataList.Count; DC++)
                        {
                            if (13 + DC >= SPLITString.Length)
                            {
                                ExportDataList[DC].Count += 0;
                                continue;
                            }
                            ExportDataList[DC].Count += int.Parse(SPLITString[13 + DC]);
                        }

                    }

                    this.Yield = YSum / ((double)OrgDataList.Count - 1);


                    #endregion
                }
                catch
                {
                    return Res;
                }

                Res = true;
                return Res;
            }
        }

        public InspectionView()
        {
            InitializeComponent();

            // 改變標題列顏色
            //dataGridView1.EnableHeadersVisualStyles = false; //!!!!!! 重要參數                           
            //dataGridView1.ColumnHeadersDefaultCellStyle.BackColor =  Color.Green;

            this.hSmartWindowControl_InspectView.MouseWheel += hSmartWindowControl_InspectView.HSmartWindowControl_MouseWheel; // (20190718) Jeff Revised!
            initBackgroundWorker(); // (20190604) Jeff Revised!

            //locate_method_.LocateMethod_Constructor(); // (20200429) Jeff Revised!

            clsLanguage.clsLanguage.SetLanguateToControls(this); // (20200214) Jeff Revised!
        }

        public void EnableDoubleBuffering()
        {
            // Set the value of the double-buffering style bits to true.
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
        }

        private void initBackgroundWorker() // (20190604) Jeff Revised!
        {
            bw = new BackgroundWorker();
            bw.WorkerReportsProgress = false;
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            //bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
        }

        private CellMapForm bw_form_cellmap_ = new CellMapForm(); // (20190606) Jeff Revised!
        /// <summary>
        /// 背景執行: 等待大圖拼接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bw_DoWork(object sender, DoWorkEventArgs e) // (20190604) Jeff Revised!
        {
            BackgroundWorker Worker = (BackgroundWorker)sender;
            clsProgressbar m_ProgressBar = new clsProgressbar();

            m_ProgressBar.FormClosedEvent2 += new clsProgressbar.FormClosedHandler2(SetFormClosed2);

            m_ProgressBar.SetShowText("請等待大圖拼接......");
            m_ProgressBar.SetShowCaption("執行中......");
            m_ProgressBar.ShowWaitProgress();

            try
            {
                while (true)
                {
                    //if (ShowDialog_BigMap())
                    //    break;

                    if (bw.CancellationPending == true)
                    {
                        e.Cancel = true;
                        break;
                    }
                    else
                    {
                        if (big_map2_ != null)
                        {
                            // Method 1

                            // Method 2 (20190606) Jeff Revised!
                            //bw_form_cellmap_ = new CellMapForm();
                            //bw_form_cellmap_.set_locate_method(locate_method_); // (20181116) Jeff Revised!
                            //bw_form_cellmap_.Cell_map_img_ = big_map2_;

                            break;
                        }

                        //Application.DoEvents();
                        Thread.Sleep(300);
                    }
                }

                m_ProgressBar.CloseProgress();

                e.Result = "Success";
            }
            catch (Exception ex)
            {
                m_ProgressBar.CloseProgress();
                MessageBox.Show(ex.ToString());

                e.Result = "Exception";
            }
        }

        /// <summary>
        /// 停止BackgroundWorker執行
        /// </summary>
        public void SetFormClosed2() // (20190604) Jeff Revised!
        {
            if (bw.WorkerSupportsCancellation == true)
            {
                if (bw.IsBusy)
                    bw.CancelAsync();
            }
        }

        /// <summary>
        /// BackgroundWorker執行完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) // (20190604) Jeff Revised!
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
                MessageBox.Show("Exception");
            }
            else if (e.Result != null && e.Result.ToString() == "Success")
            {
                //MessageBox.Show("完成!");

                // Method 1
                ShowDialog_BigMap();
                // Method 2 (20190606) Jeff Revised!
                //bw_form_cellmap_.ShowDialog();
                //bw_form_cellmap_.Dispose();
            }
        }

        public void set_AOIMainForm(AOIMainForm form) // (20190402) Jeff Revised!
        {
            mainForm = form;
        }

        public void setup_model_panel(List<InspectOperator> inspect_models_collection)
        {
            pnl_insp_model.Controls.Clear(); // 181025, Andy

            // Create model items on panels
            for (int i = 0; i < inspect_models_collection.Count; i++)
            {
                // Set up model item's attribute
                ToDoItemUI todo = new ToDoItemUI();
                todo.Location = new System.Drawing.Point(11, 15 + 120 * i);
                todo.Size = new System.Drawing.Size(270, 110);
                todo.Click += new System.EventHandler(this.model_item_click);
                todo.Tag = i;

                // Set up model item's content
                set_item_content(inspect_models_collection[i], todo);

                // Add model item into panel: @pnl_insp_model
                pnl_insp_model.Controls.Add(todo);
            }


        }

        public void setup_map(int row, int col)
        {
            if (defect_map_ != null) defect_map_.release(); // 181026, andy !!!!!!!!!!!!!!!!!!!!!

            defect_map_ = null;
            defect_map_ = new DefectMap(row, col, pictureBox_InspectMap);
            pictureBox_InspectMap.Controls.Clear();
            pictureBox_InspectMap = defect_map_.Canvas;

            // 190221, andy
            if (defect_map_.ParameterFormShow_clicked == null)
                defect_map_.ParameterFormShow_clicked += new EventHandler(ParameterFormShow);

            // 190625, andy
            defect_map_.delegateRemoteMotor += InvokeTCPIP_DirectSendResult;
            dg_RemoteMotor = new delegate_RemoteMotor(TCPIP_DirectSendResult);

        }


        #region 190625, andy: Remote motor
        private void TCPIP_DirectSendResult(int PositionID)
        {
            if (mainForm.TCPIPSorter_Form == null || mainForm.advSet.get_RemoteMotor == false) return;

            // 組合
            string InitIndexStr = "2";                  // String.Format("{0:00000.000}", 2);  // start from 2 (視覺定位目前2個位置)
            string nowPosStr = string.Format("{0:D8}", PositionID);   // String.Format("{0:00000.000}", PositionID); // start from 0
            string tempStr = "(" + InitIndexStr + "," + nowPosStr + ");";

            string content =

                       string.Format("{0:D8}", SB_ID) + ";"
                                          + "REMOTE" + ";"
                                          + "OK" + ";"
                                          + ModuleName + ";"
                                          + "00000000;"
                                          + nowPosStr + ";";



            // Final
            char head = (char)02;
            char end = (char)03;
            string sendStr = head + content + end;
            mainForm.TCPIPSorter_Form.dMes.DirectSendResult(sendStr);

        }

        private void InvokeTCPIP_DirectSendResult(int PositionID)
        {

            this.Invoke(dg_RemoteMotor, PositionID);
        }

        #endregion


        // 190221
        private void ParameterFormShow(object sender, EventArgs e)
        {
            model_item_click(sender, e);
        }

        public void set_item_content(InspectOperator insp_operator, ToDoItemUI item)
        {
            // name
            //item.set_item_name(insp_operator.get_name());
            item.set_item_name(insp_operator.OperatorInfo[0]);
            item.set_info_1(insp_operator.OperatorInfo[1]);
            item.set_info_2(insp_operator.OperatorInfo[2]);
            item.set_info_3(insp_operator.OperatorInfo[3]);
            // setting done or not
        }

        public void model_item_click(object sender, EventArgs e)
        {
            ToDoItemUI item = sender as ToDoItemUI;

            // 190221, andy
            if (item != null)
                selected_item_ = Convert.ToInt32(item.Tag);
            else
                selected_item_ = 0;

            model_item_clicked(this, e);
        }

        public int get_index()
        {
            return selected_item_;
        }

        //private HObject Img = null; // For 華新trim痕檢測 (20180906) Jeff Revised!
        /// <summary>
        /// 顯示 【INSPECT VIEW】
        /// </summary>
        /// <param name="img"></param>
        public void set_result_img(HObject img, HObject def_reg = null, int MoveIndex = 1) // (20190718) Jeff Revised!
        {
            // For 華新trim痕檢測 (20180906) Jeff Revised!
            //HOperatorSet.CopyImage(img, out Img);
            //int i = OperateEngine.TRIM.ImageView_HObject.Count - 1;
            //pictureBox1.Image = OperateEngine.TRIM.ImageView_HObject[i].GetRGBBitmap();

            if (FinalInspectParam.dispWindow == "pictureBox")
                pictureBox_InspectView.Image = img.GetRGBBitmap();
            else
            {
                HOperatorSet.DispObj(img, hSmartWindowControl_InspectView.HalconWindow);
                if (MoveIndex == 1)
                {
                    HOperatorSet.SetColor(hSmartWindowControl_InspectView.HalconWindow, "red");
                    HOperatorSet.SetDraw(hSmartWindowControl_InspectView.HalconWindow, "fill");
                    HOperatorSet.SetPart(hSmartWindowControl_InspectView.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part
                }
                if (def_reg != null)
                    HOperatorSet.DispObj(def_reg, hSmartWindowControl_InspectView.HalconWindow);
            }
        }

        /// <summary>
        /// 改變圖像顯示視窗類型
        /// </summary>
        public void Change_dispWindow() // (20190718) Jeff Revised!
        {
            if (FinalInspectParam.dispWindow == "pictureBox")
            {
                pictureBox_InspectView.Visible = true;
                hSmartWindowControl_InspectView.Visible = false;
                pictureBox_InspectView.Image = null;
            }
            else
            {
                //if (pictureBox_InspectView.Image != null)
                //    pictureBox_InspectView.Image.Dispose();
                pictureBox_InspectView.Image = null;
                pictureBox_InspectView.Visible = false;
                hSmartWindowControl_InspectView.Visible = true;
                HOperatorSet.ClearWindow(hSmartWindowControl_InspectView.HalconWindow);
            }
        }

        public void set_result_map(ImageObj imgs, List<Defect> defects, List<Point> pos)
        {
            defect_map_.setup_item(imgs, defects, pictureBox_InspectView.Image, pos);
            //defect_map_.setup_item(imgs, defects, Img.GetRGBBitmap(), pos); // For 華新trim痕檢測 (20180906) Jeff Revised!
            pictureBox_InspectMap = defect_map_.Canvas;
            dataGridView1.BackgroundColor = Color.White;
        }

        public void set_result_map(ImageObj imgs, List<Defect> defects, List<Point> pos, Dictionary<string, List<Defect>> dic_defects, HObject TotalDefect_region = null) // (20190718) Jeff Revised!
        {
            if (TotalDefect_region == null)
                defect_map_.setup_item(imgs, defects, pictureBox_InspectView.Image, pos, dic_defects);
            else
                defect_map_.setup_item(imgs, defects, TotalDefect_region, pos, dic_defects);
            pictureBox_InspectMap = defect_map_.Canvas;
            dataGridView1.BackgroundColor = Color.White;
        }

        public void release()
        {
            defect_map_.release();
            defect_map_ = null;
            //GC.Collect();
            //GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// 統計結果
        /// </summary>
        public DataGridView ResultStaticTable
        {
            get { return dataGridView1; }
            set { dataGridView1 = value; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //anita
            //List<HObject[]> temp_ = DeltaSubstrateInspector.src.Modules.ImageObj.trigger_img_;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.save_img_on_ ^= true; // 邏輯互斥 OR 運算子: 相同為0，不同為1
            if (this.save_img_on_)
            {
                this.btn_save.BackgroundImage = global::DeltaSubstrateInspector.Properties.Resources.ON;
                this.lbl_save.Text = "ON";
                this.lbl_save.ForeColor = Color.DeepSkyBlue;
            }
            else
            {
                this.btn_save.BackgroundImage = global::DeltaSubstrateInspector.Properties.Resources.OFF_edited;
                this.lbl_save.Text = "OFF";
                this.lbl_save.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {          //anita              
            //DeltaSubstrateInspector.src.Modules.ImageObj.trigger_img_.Clear();
            //DeltaSubstrateInspector.src.Modules.ImageObj.trigger_img_ = new List<HObject[]>();
            DeltaSubstrateInspector.src.Models.CaptureModel.Camera.trigger_count_ = 0;
        }


        public HWindowControl show_window()
        {
            return this.hWindowControl_cam;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            map_setting_clicked(this, e);
        }

        public void set_big_map(HObject img)
        {
            try
            {
                HOperatorSet.ZoomImageSize(img, out big_map_, 600, 600, "constant");
            }
            catch
            {
                big_map_ = null;
            }
        }

        //public void set_big_map2(HObject img, HObject cellmap_affine_, HObject defect_region_, HObject capture_map_) // (20181108) Jeff Revised!
        //{
        //    try
        //    {
        //        HOperatorSet.CopyImage(img, out big_map2_);
        //        if (cellmap_affine_ != null)
        //            HOperatorSet.CopyObj(cellmap_affine_, out Cellmap_affine_, 1, -1);
        //        else
        //            Cellmap_affine_ = null;

        //        if (defect_region_ != null)
        //            HOperatorSet.CopyObj(defect_region_, out Defect_region_, 1, -1);
        //        else
        //            Defect_region_ = null;
        //        if (capture_map_ != null)
        //            HOperatorSet.CopyObj(capture_map_, out Capture_map_, 1, -1);
        //        else
        //            Capture_map_ = null;
        //    }
        //    catch
        //    {
        //        big_map2_ = null;
        //        Cellmap_affine_ = null;
        //        Defect_region_ = null;
        //        Capture_map_ = null;
        //    }
        //}

        public HObject get_big_Map()
        {
            return big_map2_;
        }

        public void set_big_map3(HObject img) // (20181116) Jeff Revised!
        {
            try
            {
                if (img != null)
                {
                    Extension.HObjectMedthods.ReleaseHObject(ref big_map2_);
                    HOperatorSet.CopyImage(img, out big_map2_);
                }
                else
                {
                    Extension.HObjectMedthods.ReleaseHObject(ref big_map2_);
                }
            }
            catch
            {
                Extension.HObjectMedthods.ReleaseHObject(ref big_map2_);
            }
        }

        public void set_locate_method(LocateMethod method) // (20181116) Jeff Revised!
        {
            locate_method_ = method;
        }

        public static List<string> loadCsvFile(string filePath)
        {
            List<string> searchList = new List<string>();
            try
            {
                #region Excel Open

                Process[] MyProcess = Process.GetProcessesByName("EXCEL");
                string FileName = filePath.Substring(filePath.LastIndexOf("\\") + 1);
                bool bFileOpen = false;
                int Index = 0;

                for (int i = 0; i < MyProcess.Length; i++)
                {
                    if (MyProcess[i].MainWindowTitle.Contains(FileName))
                    {
                        Index = i;
                        bFileOpen = true;
                        break;
                    }
                }

                if (MyProcess.Length > 0 && bFileOpen)
                    MyProcess[Index].Kill();

                #endregion

                #region Libre Office

                Process[] CSVProcess = Process.GetProcessesByName("soffice.bin");

                if (CSVProcess.Length > 0)
                {
                    foreach (Process P in CSVProcess)
                    {
                        P.Kill();
                        P.WaitForExit();
                    }
                }
                #endregion

                var reader = new StreamReader(File.OpenRead(filePath), Encoding.Default);
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    searchList.Add(line);
                }
                reader.Dispose();
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }
            return searchList;
        }

        public static void ExportSummaryFile(string SelectDate)
        {
            List<clsExportCSVParam> ExportList = new List<clsExportCSVParam>();
            DateTime dt;
            FileStream fs = null;
            StreamWriter sw = null;
            dt = Convert.ToDateTime(SelectDate);
            string CSVFilePath = ModuleParamDirectory + HistoryDirectory + dt.ToString("yyyyMMdd") + "\\";
            string FilePath = CSVFilePath + dt.ToString("yyyy-MM-dd") + ".csv";
            if (File.Exists(FilePath))
            {
                #region Libre Office
                try
                {
                    clsStaticTool.CloseLibreOffice();

                    File.Delete(FilePath);
                }
                catch { }

                #endregion
            }

            if (!Directory.Exists(@CSVFilePath))
                return;

            var txtFiles = Directory.EnumerateFileSystemEntries(@CSVFilePath, "*.csv", SearchOption.AllDirectories).ToArray();

            if (txtFiles.Length <= 0)
                return;

            try
            {
                #region 統整資料夾內所有CSV檔案

                foreach (string CurrentFilePath in txtFiles)
                {
                    List<string> ListCSV = loadCsvFile(CurrentFilePath);
                    if (ListCSV.Count <= 1)
                        continue;
                    clsExportCSVParam Param = new clsExportCSVParam();
                    Param.SummaryResult(ListCSV);
                    ExportList.Add(Param);
                }

                #endregion

                fs = new FileStream(FilePath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                sw = new StreamWriter(fs, System.Text.Encoding.Default);

                #region Header

                string Hearer = "日期,批號,參數,排板顆,總檢驗顆數,起始時間,結束時間,總生產時間,投入數,總OK數,總NG數,良率%,異常總數,";

                #region 取得所有Defect名稱並加入Header

                List<string> DefectNameList = new List<string>();
                for (int i = 0; i < ExportList.Count; i++)
                {
                    foreach (clsExportDefectData S in ExportList[i].ExportDataList)
                    {
                        if (!DefectNameList.Exists(x => x.Contains(S.Name)))
                        {
                            Hearer += S.Name + ",";
                            DefectNameList.Add(S.Name);
                        }
                    }
                }
                Hearer += "定位異常,";
                foreach (string S in DefectNameList)
                {
                    Hearer += S + "%" + ",";
                }

                Hearer += "定位異常%,";

                #endregion


                sw.WriteLine(Hearer);

                #endregion

                #region DATA

                for (int i = 0; i < ExportList.Count; i++)
                {
                    DateTime S = DateTime.Parse(ExportList[i].StartTime);
                    DateTime E = DateTime.Parse(ExportList[i].EndTime);
                    string Span = E.Subtract(S).TotalMinutes.ToString("#.##");

                    //"日期,機台序號,批號,參數,作業者,排板顆,總檢驗顆數,起始時間,結束時間,總生產時間,投入數,總OK數,總NG數,良率%,異常總數,"
                    string ResultData = ExportList[i].Date + "," +
                                        ExportList[i].LotID + "," +
                                        ExportList[i].ParamName + "," +
                                        ExportList[i].TotalCellCount + "," +
                                        ExportList[i].TotalCellCount * ExportList[i].TotalCount + "," +
                                        ExportList[i].StartTime + "," +
                                        ExportList[i].EndTime + "," +
                                        ExportList[i].TotalTime.TotalMinutes.ToString("#.##") + "," +
                                        ExportList[i].TotalCount + "," +
                                        (ExportList[i].TotalCount - ExportList[i].NGCount) + "," +
                                        ExportList[i].NGCount + "," +
                                        ExportList[i].Yield.ToString("#.##") + "%," +
                                        ExportList[i].CellNGCount + ",";


                    for (int j = 0; j < DefectNameList.Count; j++)
                    {
                        var T = ExportList[i].ExportDataList.Find(x => x.Name.Contains(DefectNameList[j]));
                        if (T != null)
                            ResultData += T.Count.ToString() + ",";
                        else
                            ResultData += "0,";
                    }

                    ResultData += ExportList[i].TotalMatchFailCount + ",";

                    for (int k = 0; k < DefectNameList.Count; k++)
                    {
                        var T = ExportList[i].ExportDataList.Find(x => x.Name.Contains(DefectNameList[k]));
                        if (T != null)
                            ResultData += ((double)T.Count / ((double)ExportList[i].TotalCellCount * ExportList[i].TotalCount) * 100).ToString("0.#") + "%,";
                        else
                            ResultData += "0%,";
                    }

                    ResultData += ((double)ExportList[i].TotalMatchFailCount / ((double)ExportList[i].TotalCellCount * ExportList[i].TotalCount) * 100).ToString("0.#") + "%,";

                    sw.WriteLine(ResultData);
                }

                #endregion

                sw.Close();
                fs.Close();

            }
            catch (Exception Ex)
            {
                if (sw != null)
                    sw.Close();
                if (fs != null)
                    fs.Close();
                throw new Exception(Ex.ToString());
            }
        }

        private void BigMap_Click(object sender, EventArgs e)
        {
            //if (big_map_ != null)
            //{
            //    using (BigmapShowForm form = new BigmapShowForm(big_map_))
            //    {
            //        form.ShowDialog();
            //    }
            //}

            // (20181025) Jeff Revised!
            //if (big_map2_ != null)
            //{               
            //    using (CellMapForm form_cellmap_ = new CellMapForm())
            //    {
            //        form_cellmap_.cell_map_img_ = big_map2_;
            //        form_cellmap_.golden_cell_region_ = Cellmap_affine_;
            //        form_cellmap_.defect_region_ = Defect_region_;
            //        form_cellmap_.capture_map_ = Capture_map_;
            //        form_cellmap_.ShowDialog();
            //    }
            //}

            // (20181116) Jeff Revised!
            //if (big_map2_ != null)
            //{
            //    using (CellMapForm form_cellmap_ = new CellMapForm())
            //    {
            //        form_cellmap_.set_locate_method(locate_method_); // (20181116) Jeff Revised!
            //        form_cellmap_.Cell_map_img_ = big_map2_;
            //        form_cellmap_.ShowDialog();
            //    }
            //}

            // (20190405) Jeff Revised!
            //if (mainForm.advSet.B_BigMap) // 檢視大圖
            //{
            //    ShowDialog_BigMap();
            //}

            // (20190604) Jeff Revised!
            if (mainForm.advSet.B_BigMap && B_imagesFinished) // 檢視大圖
            {
                button_BigMap.Enabled = false;
                if (bw.IsBusy != true)
                {
                    if (big_map2_ != null) // 拼圖已完成
                        ShowDialog_BigMap();
                    else // 拼圖尚未完成
                        bw.RunWorkerAsync();
                }
                button_BigMap.Enabled = true;
            }
        }

        private bool ShowDialog_BigMap() // (20190604) Jeff Revised!
        {
            if (big_map2_ != null)
            {
                using (CellMapForm form_cellmap_ = new CellMapForm())
                {
                    //form_cellmap_.set_locate_method(locate_method_); // (20181116) Jeff Revised!
                    form_cellmap_.set_locate_method(Locate_Method_FS); // (20190703) Jeff Revised!
                    form_cellmap_.Cell_map_img_ = big_map2_;
                    form_cellmap_.ListMapItem = defect_map_.ListMapItem; // (20190729) Jeff Revised!
                    form_cellmap_.ShowDialog();
                    return true;
                }
            }

            return false;
        }

        public List<MapItem> Get_ListMapItem() // (20200429) Jeff Revised!
        {
            return defect_map_.ListMapItem;
        }

        public void set_button_BigMap_enabled(bool Enabled) // (20190405) Jeff Revised!
        {
            button_BigMap.Enabled = Enabled;
        }

        public void save_imgs()
        {
            try
            {
                int count_ = 1;

                foreach (HObject[] i in DeltaSubstrateInspector.src.Modules.clsTriggerImg.trigger_img_)
                {
                    string move_format = string.Format("M{0:d3}", count_);
                    string light_format_1 = string.Format("F{0:d3}", 1);
                    string light_format_2 = string.Format("F{0:d3}", 2); // 一個位置兩張影像 (20181112) Jeff Revised!

                    HOperatorSet.WriteImage(i[0], "tiff", 0, "abc-1_" + move_format + "_" + light_format_1);
                    HOperatorSet.WriteImage(i[1], "tiff", 0, "abc-1_" + move_format + "_" + light_format_2); // 一個位置兩張影像 (20181112) Jeff Revised!
                    count_++;
                }

            }
            catch
            {
                MessageBox.Show("儲存影像發生錯誤，請確認影像內容");
            }
        }

        /// <summary>
        /// 是否儲存影像
        /// </summary>
        public bool SaveImgOn
        {
            get { return this.save_img_on_; }
            set { this.save_img_on_ = false; }
        }

        /// <summary>
        /// 更新【STATIC INFO】資訊
        /// </summary>
        /// <param name="_processResultString"></param>
        /// <param name="_workResultString"></param>
        /// <param name="_now_numberOfInspectionCellNG"></param>
        public void AddInsResultToStaticInfo(string _processResultString, string _workResultString, int _now_numberOfInspectionCellNG)
        {
            numberOfInspection++;
            if (_workResultString == "OK")
            {
                numberOfInspectionOK++;

                // 190225, andy: 僅加總OK片的不良總數 for Cyntec
                numberOfInspectionCellNG += _now_numberOfInspectionCellNG;

            }
            else
            {
                numberOfInspectionNG++;
            }

            // Update textbox result
            labelNumberOfInspection.Text = numberOfInspection.ToString();
            labelNumberOfInspectionOK.Text = numberOfInspectionOK.ToString();
            labelNumberOfInspectionNG.Text = numberOfInspectionNG.ToString();
            labelNumberOfInspectionNGChip.Text = numberOfInspectionCellNG.ToString();

        }

        private void button_Reset_Click(object sender, EventArgs e)
        {
            // 刪除到回收站
            if (MessageBox.Show("您是否確定要進行重置？", "Warning!", MessageBoxButtons.YesNo) == DialogResult.No) return;

            //--------------------------------------------
            // Reset 檢測統計資訊
            //--------------------------------------------
            numberOfInspection = numberOfInspectionOK = numberOfInspectionNG = numberOfInspectionCellNG = 0;

            labelNumberOfInspection.Text = numberOfInspection.ToString();
            labelNumberOfInspectionOK.Text = numberOfInspectionOK.ToString();
            labelNumberOfInspectionNG.Text = numberOfInspectionNG.ToString();
            labelNumberOfInspectionNGChip.Text = numberOfInspectionCellNG.ToString();

            this.dataGridView1.Rows.Clear();

            ResultManager.ResetInerChipCount();

            // 執行GC.Collect() (20190402) Jeff Revised!
            if (mainForm.InspectionFinish)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        public void UpdateInfo(int captureCount)
        {
            label_CaptureCount.Text = "Count: " + captureCount.ToString();
        }

        private void lbl_save_Click(object sender, EventArgs e)
        {

        }
    }
}
