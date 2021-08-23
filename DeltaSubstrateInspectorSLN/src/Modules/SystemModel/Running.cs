using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using DelVivi.Log;
using System.Drawing;

using static DeltaSubstrateInspector.FileSystem.FileSystem;
using DeltaSubstrateInspector.FileSystem;

namespace DeltaSubstrateInspector.src.Modules.SystemModel
{
    public class RunningThread
    {
        // 使用UI元件時, 若於Thread呼叫, 一定要用委派
        public delegate bool DelegateChangeRecipe(string RecipeName);
        public DelegateChangeRecipe delegateChangeRecipe;

        private AOIMainForm mainForm;
        private bool is_running = false;
        private Thread controlThread;
        private string SorterTCPIPReceive;
        public StartProcReceive AOIStartProcReceive;
        public string WorkMode
        {           
            get { return AOIStartProcReceive.WorkMode; }
         
        }
        public int MarkCount
        {
            get { return Convert.ToInt32(AOIStartProcReceive.MarkCount); }
        }
        public int TriggerCount
        {
            get { return Convert.ToInt32(AOIStartProcReceive.TriggerCount); }
        }

        public int aoi_start_case = 0;
        public int aoi_end_case = 0;
        private bool MeasureStart = false; // 準備好開始取像
        private bool MeasureDone = false;  // 準備好資料

        public string SorterTCPIPSend = "";

        public RunningThread(AOIMainForm form)
        {
            mainForm = form;
            AOIStartProcReceive = new StartProcReceive();

            // Control Thread
            is_running = true;
            controlThread = new Thread(new ThreadStart(ControlRun));
            controlThread.IsBackground = true;
            //controlThread.Start(); // 181026

        }

        public void RunningThreadStart()
        {
            controlThread.Start();
        }

        private void ControlRun()
        {
            // -------------------  INIT ------------------- 

            // -----------  WAIT FOR EVENTS  --------------- 
            while (is_running)
            {
                /*
                if (FileSystem.ModuleReady)
                    OP("Mod_Online", true);
                else
                    OP("Mod_Online", false);
               */

                /*
                InitAllProcess();
                MeasureStartProcess();
                MeasureDoneProcess();
                           
                FileSystem.OperationTime = DateTime.Now.ToString("yyyyMMdd");
                */
               
                MeasureStartProcess();
                MeasureDoneProcess();

                Thread.Sleep(3);
            }

            // --------  RESET/CLOSE ALL HANDLES  ---------- 
            controlThread.Join();
            return;

        }

        private void MeasureStartProcess()
        {
     
            switch (aoi_start_case)
            {
                case 0:
                    if (!mainForm.IOManager.IsInputBitOn("SystemMeasureStart") || MeasureStart == true) break;

                        // Log
                        mainForm.syslog.WriteLine(enMessageType.Info, "Sys_MeasureStart ON");
                        
                        // change status                    
                        aoi_start_case = 10000;

                    break;

                case 10000:
                    SorterTCPIPReceive = mainForm.TCPIPSorter_Form.dMes.LastReceiveStr;
                    if (AOIStartProcReceive.PackDecode(SorterTCPIPReceive) != 0)
                    {

                        #region TCP/IP字串例外處理
                        if (mainForm.TCPIPSorter_Form.dMes.LastReceiveStr.Length <= 0)
                        {
                            // Log
                            mainForm.syslog.WriteLine(enMessageType.Warnning, "主機尚未連線，自動重試");
                            Console.WriteLine( "主機尚未連線，自動重試");


                            // change status                    
                            aoi_start_case = 0;
                            break;
                        }

                        // Log
                        mainForm.syslog.WriteLine(enMessageType.Warnning, AOIStartProcReceive.ErrorMessage);
                        Console.WriteLine(AOIStartProcReceive.ErrorMessage);
                        #endregion

                    }

                    // Update SB_ID
                    SB_ID = AOIStartProcReceive.ID;


                    // log
                    mainForm.syslog.WriteLine(enMessageType.Info, "TCP 接收字串:  " + SorterTCPIPReceive);
                    mainForm.syslog.WriteLine(enMessageType.Info, "拆解驗證, ID: " + AOIStartProcReceive.ID + " , RecipeName: " + AOIStartProcReceive.RecipeName + " , WorkMode: " + AOIStartProcReceive.WorkMode);
                    
                    // change status                    
                    aoi_start_case = 20000;

                    break;

                case 20000:
                    if (mainForm.advSet.RefMainPCRecipe) // (20181119) Jeff Revised!
                    {
                        // 自動切換 Recipe
                        if (delegateChangeRecipe != null)
                        {
                            bool status = delegateChangeRecipe(AOIStartProcReceive.RecipeName);

                            if(status==false)
                            {
                                // Log
                                mainForm.syslog.WriteLine(enMessageType.Info, "Recipe 切換失敗, 請確認參數");

                                // change status                    
                                aoi_start_case = 0;
                                break;
                            }

                        }
                       
                    }

                    // Log
                    mainForm.syslog.WriteLine(enMessageType.Info, "Recipe 切換完成");
                  
                    // change status                    
                    aoi_start_case = 30000;
                    break;

                case 30000:
                    // 全部準備完成，motion可以動作
                    #region IO底層確認...
                    mainForm.IOManager.SetOutputBit("ModuleMeasureStart", true);
                    #endregion

                    // Log
                    mainForm.syslog.WriteLine(enMessageType.Info, "Mod_MeasureStart ON");
                    
                    // change status                    
                    aoi_start_case = 40000;

                    break;

                case 40000:
                    if( mainForm.IOManager.IsInputBitOn("SystemMeasureStart") == true) break;

                    // Log      
                    mainForm.syslog.WriteLine(enMessageType.Info, "Sys_MeasureStart OFF");
                   
                    // change status                    
                    aoi_start_case = 50000;

                    break;

                case 50000:
                    mainForm.IOManager.SetOutputBit("ModuleMeasureStart", false);

                    // Log
                    mainForm.syslog.WriteLine(enMessageType.Info, "Mod_MeasureStart OFF");
                    
                    // Set MeasureStart ON
                    MeasureStart = true;

                    // change status                    
                    aoi_start_case = 0;

                    break;

            }


        }

        private void MeasureDoneProcess()
        {
            switch(aoi_end_case)
            {                
                case 0:
                    if (MeasureStart == false) break;

                    // 檢查是否計算完成視覺定位, 視覺演算法, 雷射校正
                    if (mainForm.PositionFinish == true || mainForm.InspectionFinish == true || mainForm.LaserPositionFinish)
                    {
                        // Log (20190417) Jeff Revised!
                        mainForm.syslog.WriteLine(enMessageType.Info, "排查狀態, " + "PositionFinish: " + mainForm.PositionFinish +
                                                                                 ", InspectionFinish: " + mainForm.InspectionFinish +
                                                                              ", LaserPositionFinish: " + mainForm.LaserPositionFinish);

                        if (mainForm.advSet.IgnorePosAndIns == true)
                        {
                            // 填預設資料
                            SorterTCPIPSend = GetUploadFinalString(true);

                            // Log
                            mainForm.syslog.WriteLine(enMessageType.Info, "Bypass position and inspection");

                        }
                        else
                        {
                            // 填預設資料
                            SorterTCPIPSend = GetUploadFinalString(false);

                            // Log (20190412) Jeff Revised!
                            if (AOIStartProcReceive.WorkMode == "CAL")
                                mainForm.syslog.WriteLine(enMessageType.Info, "Do position");
                            else if (AOIStartProcReceive.WorkMode == "MEASURE")
                                mainForm.syslog.WriteLine(enMessageType.Info, "Do inspection");
                            else
                                mainForm.syslog.WriteLine(enMessageType.Info, "Do laser position");
                        }

                        // change status                    
                        aoi_end_case = 20000;
                    }

                    break;

                case 20000:
                    // TCP傳送結果                    
                    mainForm.TCPIPSorter_Form.dMes.DirectSendResult(SorterTCPIPSend);

                    // Log
                    mainForm.syslog.WriteLine(enMessageType.Info, "TCP 傳送字串:  " + SorterTCPIPSend);
                   
                    // change status                    
                    aoi_end_case = 25000;
                    break;

                case 25000:
                    // ModuleMeasureDone ON
                    #region IO底層確認...
                    mainForm.IOManager.SetOutputBit("ModuleMeasureDone", true);
                    #endregion

                    // Log
                    mainForm.syslog.WriteLine(enMessageType.Info, "Mod_MeasureDone ON");


                    // 190403, andy GC
                    if (AOIStartProcReceive.WorkMode == "MEASURE")
                    {
                        GC.Collect();
                        GC.WaitForPendingFinalizers();

                        // Log
                        mainForm.syslog.WriteLine(enMessageType.Info, "GC finish");
                    }


                    // change status                    
                    aoi_end_case = 30000;
                    break;

                case 30000:
                    if (!mainForm.IOManager.IsInputBitOn("SystemMeasureDone")) break;

                    #region IO底層確認...
                    mainForm.IOManager.SetOutputBit("ModuleMeasureDone", false);

                    #endregion

                    // Log
                    mainForm.syslog.WriteLine(enMessageType.Info, "Mod_MeasureDone OFF");
                   
                    // Set MeasureStart ON
                    MeasureStart = false;

                    // Change                   
                    aoi_end_case = 0;

                    break;
            }

        }


        public string GetUploadFinalString(bool bypass = true)
        {
            string uploadStr = "";
            string nowWorkMode = AOIStartProcReceive.WorkMode;
            if (nowWorkMode == "CAL")
            {

                #region 視覺定位 Mode
                if (bypass == true)
                {

                    #region default data

                    uploadStr = string.Format("{0:D8}", AOIStartProcReceive.ID) + ";"
                                          + "SUCCESS" + ";"
                                          + "OK" + ";"
                                          + AOIStartProcReceive.RecipeName + ";"
                                          + "00000000;"
                                          + "00000000;"
                                          + "(00000.000,00000.000);(00000.000,00000.000);";
                    #endregion

                }
                else
                {
                    #region real data

                    string temp_sendProcessResult = "";
                    string temp_sendWorkResult = "";
                    string CbMarkPosiStr = "";

                    // Fill Process result                    
                    temp_sendProcessResult = mainForm.ProcessResultString;

                    // Fill Work result (20190412) Jeff Revised!
                    if (temp_sendProcessResult == "SUCCESS")
                        temp_sendWorkResult = "OK";
                    else
                        temp_sendWorkResult = "NG";

                    // Fill Mark Position string                   
                    string MARKPOS_X1 = String.Format("{0:00000.000}", mainForm.MARKPOS_X1);
                    string MARKPOS_Y1 = String.Format("{0:00000.000}", mainForm.MARKPOS_Y1);
                    string MARKPOS_X2 = String.Format("{0:00000.000}", mainForm.MARKPOS_X2);
                    string MARKPOS_Y2 = String.Format("{0:00000.000}", mainForm.MARKPOS_Y2);
                    CbMarkPosiStr = "(" + MARKPOS_X1 + "," + MARKPOS_Y1 + ");(" + MARKPOS_X2 + "," + MARKPOS_Y2 + ");";
                   
                    // 組合......
                    string sendID            = String.Format("{0:00000000}", AOIStartProcReceive.ID);
                    string sendProcessResult = temp_sendProcessResult;
                    string sendWorkResult    = temp_sendWorkResult;
                    string sendRecipeName    = AOIStartProcReceive.RecipeName;
                    string sendTotalCount    = String.Format("{0:00000000}", 2);
                    string sendNgCount       = String.Format("{0:00000000}", 2);
                    string sendMarkPos       = CbMarkPosiStr;

                    uploadStr = sendID + ";"
                                          + sendProcessResult + ";"
                                          + sendWorkResult + ";"
                                          + sendRecipeName + ";"
                                          + sendTotalCount +";"
                                          + sendNgCount + ";"
                                          + CbMarkPosiStr;

                    #endregion

                }

                #endregion

                mainForm.PositionFinish = false;

            }
            else if (nowWorkMode == "MEASURE")   // 181122, andy
            {

                #region MEASURE Mode

                if (bypass == true)
                {

                    #region default data

                    uploadStr = string.Format("{0:D8}", AOIStartProcReceive.ID) + ";"
                                  + "SUCCESS;"
                                  + "OK;"
                                  + AOIStartProcReceive.RecipeName + ";"
                                  + "00000000;"
                                  + "00000000;"
                                  + "(00000.000,00000.000);";

                    #endregion

                }
                else
                {

                    #region real data 

                    string temp_sendProcessResult = "";
                    string temp_sendWorkResult = "";
                    string CbRealDefectPosiStr = "";


                    // Fill Process result                    
                    temp_sendProcessResult = mainForm.ProcessResultString;


                    // Fill Work result                   
                    temp_sendWorkResult = mainForm.WorkResultString;


                    // Fill NG position string
                    foreach (Point i in mainForm.RealDefectPosList)
                    {
                        string X = String.Format("{0:00000.000}", i.X - 1); // start from 0
                        string Y = String.Format("{0:00000.000}", i.Y - 1); // start from 0
                        string tempStr = "(" + X + "," + Y + ");";
                        CbRealDefectPosiStr = CbRealDefectPosiStr + tempStr;
                    }


                    // 組合......
                    string sendID = String.Format("{0:00000000}", AOIStartProcReceive.ID);
                    string sendProcessResult = temp_sendProcessResult;
                    string sendWorkResult = temp_sendWorkResult;
                    string sendRecipeName = AOIStartProcReceive.RecipeName;
                    string sendTotalCount = String.Format("{0:00000000}", mainForm.Cell_Total_Count);
                    string sendNgCount = String.Format("{0:00000000}", mainForm.RealDefectPosList.Count);
                    string sendNgData = CbRealDefectPosiStr;

                    uploadStr = sendID + ";"
                                          + sendProcessResult + ";"
                                          + sendWorkResult + ";"
                                          + sendRecipeName + ";"
                                          + sendTotalCount + ";"
                                          + sendNgCount + ";"
                                          + sendNgData;


                    #endregion

                }

                #region 強制取代成指定瑕疵座標
                // !!!!!!!!! ! 強制取代成手動輸入!!!!!!!!!!!!! 綁定Recipe, 切換 laserPos_view2_ or laserPos_view1_ : 尚未測試
                if ((mainForm.laser_setting.laserPos_view1_.GetLaserPostionTest() == true) && (AOIStartProcReceive.RecipeName == mainForm.laser_setting.laserPos_view1_.GetAssignRecipeName()))
                {
                        #region 第1軸
                        string sendID = String.Format("{0:00000000}", AOIStartProcReceive.ID);
                        string sendRecipeName = AOIStartProcReceive.RecipeName;
                        string sendProcessResult = mainForm.laser_setting.laserPos_view1_.GetPorcessResultStr();
                        string sendWorkResult = mainForm.laser_setting.laserPos_view1_.GetWorkResultStr();
                        string sendTotalCount = mainForm.laser_setting.laserPos_view1_.GetTotalCountStr();
                        string sendNgCount = mainForm.laser_setting.laserPos_view1_.GetDefectCountStr();
                        string sendNgData = mainForm.laser_setting.laserPos_view1_.GetDefectIndexStr();

                        uploadStr = sendID + ";" 
                                    + sendProcessResult + ";" 
                                    + sendWorkResult + ";" 
                                    + sendRecipeName + ";" 
                                    + sendTotalCount + ";" 
                                    + sendNgCount + ";" 
                                    + sendNgData;

                        #endregion
                }

                if ((mainForm.laser_setting.laserPos_view2_.GetLaserPostionTest() == true) && (AOIStartProcReceive.RecipeName == mainForm.laser_setting.laserPos_view2_.GetAssignRecipeName()))
                {
                        #region 第2軸
                        string sendID = String.Format("{0:00000000}", AOIStartProcReceive.ID);
                        string sendRecipeName = AOIStartProcReceive.RecipeName;
                        string sendProcessResult = mainForm.laser_setting.laserPos_view2_.GetPorcessResultStr();
                        string sendWorkResult = mainForm.laser_setting.laserPos_view2_.GetWorkResultStr();
                        string sendTotalCount = mainForm.laser_setting.laserPos_view2_.GetTotalCountStr();
                        string sendNgCount = mainForm.laser_setting.laserPos_view2_.GetDefectCountStr();
                        string sendNgData = mainForm.laser_setting.laserPos_view2_.GetDefectIndexStr();

                        uploadStr = sendID + ";"
                                   + sendProcessResult + ";"
                                   + sendWorkResult + ";"
                                   + sendRecipeName + ";"
                                   + sendTotalCount + ";"
                                   + sendNgCount + ";"
                                   + sendNgData;


                        #endregion
                }
                #endregion

                #endregion

                mainForm.InspectionFinish = false;

            }
            else if (nowWorkMode == "FIND")
            {

                #region FIND Mode

                if (bypass == true)
                {

                    #region default data

                    uploadStr = string.Format("{0:D8}", AOIStartProcReceive.ID) + ";"
                                          + "SUCCESS" + ";"
                                          + "OK" + ";"
                                          + AOIStartProcReceive.RecipeName + ";"
                                          + "00000000;"
                                          + "00000000;"
                                          + "(00000.000,00000.000);(00000.000,00000.000);";

                    // log
                    mainForm.syslog.WriteLine(enMessageType.Info, "MarkCount: " + AOIStartProcReceive.MarkCount + " , TriggerCount: " + AOIStartProcReceive.TriggerCount);

                    #endregion

                }
                else
                {
                    #region real data

                    string temp_sendProcessResult = "";
                    string CbMarkPosiStr = "";
                    int CheckAllCount = MarkCount * TriggerCount;
                    if (mainForm.LaserFINDMarkPosList.Count != CheckAllCount)
                    {
                        temp_sendProcessResult = "FAIL";
                        CbMarkPosiStr = "(00000.000,00000.000);(00000.000,00000.000);";
                    }
                    else
                    {

                        foreach (PointF i in mainForm.LaserFINDMarkPosList) // (20181119) Jeff Revised!
                        {
                            string str = "X = " + Point.Round(i).X.ToString() + ", Y = " + Point.Round(i).Y.ToString() + "\n";
                            
                            string X = String.Format("{0:00000.000}", i.X);
                            string Y = String.Format("{0:00000.000}", i.Y);
                            string tempStr = "(" + X + "," + Y + ");";
                            CbMarkPosiStr = CbMarkPosiStr + tempStr;
                           
                        }

                        temp_sendProcessResult = (mainForm.LaserPositionSuccess == true) ? "SUCCESS" : "FAIL";

                    }



                    // 組合......
                    string sendID = String.Format("{0:00000000}", AOIStartProcReceive.ID);
                    string sendProcessResult = temp_sendProcessResult;
                    string sendWorkResult = "OK";
                    string sendRecipeName = AOIStartProcReceive.RecipeName;
                    string sendTotalCount = String.Format("{0:00000000}", mainForm.LaserFINDMarkPosList.Count);
                    string sendNgCount = String.Format("{0:00000000}", mainForm.LaserFINDMarkPosList.Count);
                    string sendMarkPos = CbMarkPosiStr;

                    uploadStr = sendID + ";"
                                          + sendProcessResult + ";"
                                          + sendWorkResult + ";"
                                          + sendRecipeName + ";"
                                          + sendTotalCount + ";"
                                          + sendNgCount + ";"
                                          + CbMarkPosiStr;

                    #endregion

                }

                #endregion

                mainForm.LaserPositionFinish = false;

            }
            else if (nowWorkMode == "WORK;")  // 實際字串為....WORK; --> 已取消
            {

                #region 舊工作模式
                if (bypass == true)
                {
                    uploadStr = string.Format("{0:D8}", AOIStartProcReceive.ID) + ";"
                                  + "SUCCESS;"
                                  + "OK;"
                                  + AOIStartProcReceive.RecipeName + ";"
                                  + "00000000;"
                                  + "00000000;"
                                  + "(00000,00000);";
                }
                else
                {
                    // 組合......

                }

                #endregion

            }           
            else if (nowWorkMode == "MEASURE1")  // 實際字串為....MEASURE1; --> 已取消
            {

                #region OLD MB1

                if (bypass == true)
                {
                    #region default data
                    uploadStr = string.Format("{0:D8}", AOIStartProcReceive.ID) + ";"
                                  + "SUCCESS;"
                                  + "OK;"
                                  + AOIStartProcReceive.RecipeName + ";"
                                  + "00000000;"
                                  + "00000000;"
                                  + "(00000000,00000000);";
                    #endregion

                }
                else
                {
                    
                    #region real data 
                    string temp_sendProcessResult = "";
                    string temp_sendWorkResult = "";
                    string CbRealDefectPosiStr = "";
                    foreach (Point i in mainForm.RealDefectPosList)
                    {
                        //string X = String.Format("{0:00000000}", i.X - 1); // start from 0
                        //string Y = String.Format("{0:00000000}", i.Y - 1); // start from 0

                        string X = String.Format("{0:00000.000}", i.X - 1); // start from 0
                        string Y = String.Format("{0:00000.000}", i.Y - 1); // start from 0
                        string tempStr = "(" + X + "," + Y + ");";
                        CbRealDefectPosiStr = CbRealDefectPosiStr + tempStr;
                    }
                    //syslog.WriteLine(enMessageType.Info, CbRealDefectPosiStr);                     
                    temp_sendProcessResult = "SUCCESS";
                    temp_sendWorkResult = "OK";


                    // 組合......
                    string sendID = String.Format("{0:00000000}", AOIStartProcReceive.ID);
                    string sendProcessResult = temp_sendProcessResult;
                    string sendWorkResult = temp_sendWorkResult;
                    string sendRecipeName = AOIStartProcReceive.RecipeName;
                    string sendTotalCount = String.Format("{0:00000000}", mainForm.Cell_Total_Count);
                    string sendNgCount =    String.Format("{0:00000000}", mainForm.RealDefectPosList.Count);
                    string sendNgData = CbRealDefectPosiStr;

                    uploadStr = sendID + ";"
                                          + sendProcessResult + ";"
                                          + sendWorkResult + ";"
                                          + sendRecipeName + ";"
                                          + sendTotalCount + ";"
                                          + sendNgCount    + ";"
                                          + sendNgData;

                    mainForm.syslog.WriteLine(enMessageType.Info, uploadStr); 

                    #endregion

                }

                // 強制取代成手動輸入
                if (mainForm.laser_setting.laserPos_view1_.GetLaserPostionTest() == true)
                {

                    #region 強制取代手動輸入
                    string sendID = String.Format("{0:00000000}", AOIStartProcReceive.ID);
                    string sendRecipeName = AOIStartProcReceive.RecipeName;
                    string sendProcessResult = mainForm.laser_setting.laserPos_view1_.GetPorcessResultStr();
                    string sendWorkResult = mainForm.laser_setting.laserPos_view1_.GetWorkResultStr();
                    string sendTotalCount = mainForm.laser_setting.laserPos_view1_.GetTotalCountStr();
                    string sendNgCount = mainForm.laser_setting.laserPos_view1_.GetDefectCountStr();
                    string sendNgData = mainForm.laser_setting.laserPos_view1_.GetDefectIndexStr();

                    uploadStr = sendID + ";" + sendProcessResult + ";" + sendWorkResult + ";" + sendRecipeName + ";" + sendTotalCount + ";" + sendNgCount + ";" + sendNgData;
                    #endregion

                }


                #endregion

                mainForm.InspectionFinish = false;

            }
            else if (nowWorkMode == "MEASURE2")  // 實際字串為....MEASURE2; --> 已取消
            {

                #region OLD-MB2

                if (bypass == true)
                {
                    uploadStr = string.Format("{0:D8}", AOIStartProcReceive.ID) + ";"
                                  + "SUCCESS;"
                                  + "OK;"
                                  + AOIStartProcReceive.RecipeName + ";"
                                  + "00000000;"
                                  + "00000000;"
                                  + "(00000000,00000000);";
                }
                else
                {
                   
                    #region real data 
                    string temp_sendProcessResult = "";
                    string temp_sendWorkResult = "";
                    string CbRealDefectPosiStr = "";
                    foreach (Point i in mainForm.RealDefectPosList)
                    {
                        //string X = String.Format("{0:00000000}", i.X - 1); // start from 0
                        //string Y = String.Format("{0:00000000}", i.Y - 1); // start from 0

                        string X = String.Format("{0:00000.000}", i.X - 1); // start from 0
                        string Y = String.Format("{0:00000.000}", i.Y - 1); // start from 0
                        string tempStr = "(" + X + "," + Y + ");";
                        CbRealDefectPosiStr = CbRealDefectPosiStr + tempStr;
                    }
                    //syslog.WriteLine(enMessageType.Info, CbRealDefectPosiStr);                     
                    temp_sendProcessResult = "SUCCESS";
                    temp_sendWorkResult = "OK";


                    // 組合......
                    string sendID = String.Format("{0:00000000}", AOIStartProcReceive.ID);
                    string sendProcessResult = temp_sendProcessResult;
                    string sendWorkResult = temp_sendWorkResult;
                    string sendRecipeName = AOIStartProcReceive.RecipeName;
                    string sendTotalCount = String.Format("{0:00000000}", mainForm.Cell_Total_Count);
                    string sendNgCount = String.Format("{0:00000000}", mainForm.RealDefectPosList.Count);
                    string sendNgData = CbRealDefectPosiStr;

                    uploadStr = sendID + ";"
                                          + sendProcessResult + ";"
                                          + sendWorkResult + ";"
                                          + sendRecipeName + ";"
                                          + sendTotalCount + ";"
                                          + sendNgCount + ";"
                                          + sendNgData;


                    #endregion

                }

                // 強制取代成手動輸入
                if (mainForm.laser_setting.laserPos_view2_.GetLaserPostionTest() == true)
                {
                    #region 強制取代手動輸入
                    string sendID = String.Format("{0:00000000}", AOIStartProcReceive.ID);
                    string sendRecipeName = AOIStartProcReceive.RecipeName;
                    string sendProcessResult = mainForm.laser_setting.laserPos_view2_.GetPorcessResultStr();
                    string sendWorkResult = mainForm.laser_setting.laserPos_view2_.GetWorkResultStr();
                    string sendTotalCount = mainForm.laser_setting.laserPos_view2_.GetTotalCountStr();
                    string sendNgCount = mainForm.laser_setting.laserPos_view2_.GetDefectCountStr();
                    string sendNgData = mainForm.laser_setting.laserPos_view2_.GetDefectIndexStr();

                    uploadStr = sendID + ";" + sendProcessResult + ";" + sendWorkResult + ";" + sendRecipeName + ";" + sendTotalCount + ";" + sendNgCount + ";" + sendNgData;
                    #endregion
                }

                #endregion

                mainForm.InspectionFinish = false;

            }
           

            // Head End
            char head = (char)02;
            char end = (char)03;

            // return 
            return head+ uploadStr+ end;

        }

    }
}
