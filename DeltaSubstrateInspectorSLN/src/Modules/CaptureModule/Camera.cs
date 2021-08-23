#define UseNewHardwareFun // 180318, andy

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HalconDotNet;
using Extension;
using System.Threading;
using System.ComponentModel;
using DeltaSubstrateInspector.src.Modules;
using DeltaSubstrateInspector.src.Modules.CaptureModule;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using static DeltaSubstrateInspector.FileSystem.FileSystem;

using DeltaSubstrateInspector.src.Modules.MotionModule; // 181127, andy
using Load_HDVP; // (20200717) Jeff Revised!

namespace DeltaSubstrateInspector.src.Models.CaptureModel
{

    public class Camera : CameraModule
    {

        //static ImageObj class_imageobj_ = new ImageObj();
        //public static ImageObj get_imageObj
        //{ get { return class_imageobj_; } }

        static Dictionary<int, HWindowControl> dict_HWindow_ = new Dictionary<int, HWindowControl>();
        public static Dictionary<int, HWindowControl> set_dict_HWindow
        { get { return dict_HWindow_; } set { dict_HWindow_ = value; } }

        //public static HWindowControl set_hwindow
        //{ get { return hwindow_; } set { hwindow_ = value; } }

        /// <summary>
        /// Halcon Image from Camera Grab
        /// </summary>
        static HObject ho_Image = null;
        public static HObject get_ho_Image
        {
            get { return ho_Image; }
        }
        
        /// <summary>
        /// 十字影像 (20181215) Jeff Revised!
        /// </summary>
        static HObject CrossImage = null;
        public static HObject get_CrossImage
        { get { return CrossImage; } }

        static HTuple hv_AcqHandle = null, hv_TriggerSource = null;
        public static HTuple Hv_AcqHandle // (20190615) Jeff Revised!
        {
            get { return hv_AcqHandle; }
        }
        static bool b_close_ = false;
        static bool b_autosize_;
        static int trigger_time_ = 2; // Trigger Time(Number) for Each Cell (同一位置拍攝影像張數) (20181009) Jeff Revised!
        public static int trigger_count_ = 0;
        public static bool b_clear_ = false;
        //**********************************************************************************
        static string mCameraDeviceCode = ""; // 181123,andy
        public static int total_trigger_count_ = 0; // 190102, andy
        private static int nowCapture_mode = -1;
        static HalconAPI.HFramegrabberCallback delegateCallback = new HalconAPI.HFramegrabberCallback(CameraCallbackFunction);

        //static string CallBackType = "transfer_end"; // GigE camera (20190726)

        public Camera()
        {
            // 181121, andy
            trigger_time_ = 2;
           
        }
        
        /// <summary>
        /// 連接相機並且開始取像
        /// </summary>
        public override void run()
        {
            // 181121, andy
            if (!camera_connect(RunMode))
            {
                //MessageBox.Show("Camera Disconnecting !");
                CameraInitOK = false;
            }
            else
            {
                CameraInitOK = true;
            }
        }

        /// <summary>
        /// 僅連接相機不取像
        /// </summary>
        public override void run_connectNoCapture() // (20190620) Jeff Revised!
        {
            if (!camera_connectNoCapture(RunMode))
            {
                CameraInitOK = false;
            }
            else
            {
                CameraInitOK = true;
            }
        }

        private static bool Get_CameraDeviceCode() // (20181207) Jeff revised!
        {
            bool status = false;

            HTuple hv_information = null, hv_ValueList = null;

            try
            {
                if (CameraInterface == (int)(enu_CameraInterface.USB3))
                {
                    HOperatorSet.InfoFramegrabber("USB3Vision", "info_boards", out hv_information, out hv_ValueList);
                    mCameraDeviceCode = hv_ValueList.SArr[0].Substring(hv_ValueList.SArr[0].IndexOf(":") + 1, hv_ValueList.SArr[0].LastIndexOf(" | unique_name") - hv_ValueList.SArr[0].IndexOf(":") - 1);
                }
                else if (CameraInterface == (int)(enu_CameraInterface.GigE))
                {
                    HOperatorSet.InfoFramegrabber("GigEVision", "info_boards", out hv_information, out hv_ValueList);
                    mCameraDeviceCode = hv_ValueList.SArr[0].Substring(hv_ValueList.SArr[0].IndexOf(":") + 1, hv_ValueList.SArr[0].LastIndexOf(" | ip_address") - hv_ValueList.SArr[0].IndexOf(":") - 1);
                }

                status = true;
            }
            catch
            { }

            return status;
        }

        private static bool Adv_OpenFramegrabber()
        {
            
           
            try
            {
                if (CameraInterface == (int)(enu_CameraInterface.USB3))
                {
                    int genericFast = -1; // 速度快, 但與Spinview切換時會衝突
                    HOperatorSet.OpenFramegrabber("USB3Vision", 0, 0, 0, 0, 0, 0, "progressive", -1, "default",
                                                   genericFast,
                                                   "false", "default",
                                                   mCameraDeviceCode,
                                                   0, -1,
                                                   out hv_AcqHandle);
                }
                else if (CameraInterface == (int)(enu_CameraInterface.GigE))
                {
                    HOperatorSet.OpenFramegrabber("GigEVision", 0, 0, 0, 0, 0, 0, "default", -1, "default",
                                                   -1, 
                                                   "false", "default",
                                                   mCameraDeviceCode,
                                                    0, -1,
                                                    out hv_AcqHandle);

                    // 31M 相機 (20190613) Jeff Revised!
                    /*HOperatorSet.OpenFramegrabber("GigEVision", 0, 0, 0, 0, 0, 0, "default", -1, "default",
                                                   -1,
                                                   "false", "default",
                                                   "CCD_1",
                                                    0, -1,
                                                    out hv_AcqHandle);*/
                }

                // return
                return true;


            }
            catch (Exception ex1)
            {

                try
                {
                    string genericStable = "";
                    if (CameraInterface == (int)(enu_CameraInterface.USB3))
                    {
                        genericStable = "install_driver = 1e10 / 3300"; // 速度慢, 但可成功
                        HOperatorSet.OpenFramegrabber("USB3Vision", 0, 0, 0, 0, 0, 0, "progressive", -1, "default",
                                                   genericStable,
                                                   "false", "default",
                                                   mCameraDeviceCode,
                                                   0, -1,
                                                   out hv_AcqHandle);
                    }
                    else if (CameraInterface == (int)(enu_CameraInterface.GigE)) // (20190613) Jeff Revised!
                    {
                        genericStable = "GtlForceIP=1c0faf60d9df,169.254.0.82/16";
                        HOperatorSet.OpenFramegrabber("GigEVision", 0, 0, 0, 0, 0, 0, "default", -1, "default",
                                                   genericStable,
                                                   "false", "default",
                                                   "CCD_1",
                                                    0, -1,
                                                    out hv_AcqHandle);
                    }

                    // return
                    return true;

                }
                catch (Exception ex2)
                {
                    hv_AcqHandle = null;
                    return false;
                }



            }

          
        }

        /// <summary>
        /// 取像Callback Function
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="context"></param>
        /// <param name="user_context"></param>
        /// <returns></returns>
        public static int CameraCallbackFunction(IntPtr handle, IntPtr context, IntPtr user_context)
        {
            try
            {
                mResultStatus[grabBufferIndex].GrabOK = false;

                release_hobject(ref ho_ImageBuffer[grabBufferIndex]);
                
                HOperatorSet.GrabImageAsync(out ho_ImageBuffer[grabBufferIndex], hv_AcqHandle, -1);
                // Change state
                mResultStatus[grabBufferIndex].GrabOK = true;
                mResultStatus[grabBufferIndex].GrabStart = false;
                mResultStatus[grabBufferIndex].InspectionStart = true;

                // Grab count
                grabBufferIndex++;

                // reset nowBufID
                if (grabBufferIndex >= MaxImageBufferNum)
                {
                    grabBufferIndex = 0;
                }

                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("CallBack Error : \n" + ex.ToString());
                return -1;
            }

        }
        static Thread thread_ = null;
        static Thread thread_Preprocess = null; // 190321, andy
        public static bool camera_connect(int capture_mode_)
        {
            ThreadStart thread_cpature_mode_;
            bool b_connect = false;
            b_autosize_ = false;

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Image);

            // Get Camera device code
            if(!Get_CameraDeviceCode())
            {
                b_connect = false;
                return b_connect;
            }

            // Adv-Open Framegrabber
            if (!Adv_OpenFramegrabber())
            {
                b_connect = false;
                return b_connect;
            }

            // Get now camera setting
            HOperatorSet.GetFramegrabberParam(hv_AcqHandle, "TriggerSource", out hv_TriggerSource);


            // Set trigger mode
            nowCapture_mode = capture_mode_;
            if (capture_mode_ == 2)
            {

#if UseNewHardwareFun

                thread_cpature_mode_ = hardware_trigger_capture_new; // 190318, andy

#else

                thread_cpature_mode_ = hardware_trigger_capture_; // 190318, andy

#endif

                HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "EventSelector", "ExposureEnd"); //事件設定,目前相機只支援 ExposureEnd
                HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "EventNotification", "On");  // 是否開啟事件

                HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "grab_timeout", -1);
                HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "TriggerMode", "On");
                HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "TriggerSource", "Line0");
                HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "TriggerActivation", "RisingEdge"); // 設定上升緣觸發 (20181207) Jeff Revised!
                //HOperatorSet.GrabImageStart(hv_AcqHandle, -1);
            }
            else if (capture_mode_ == 1)
            {
                thread_cpature_mode_ = capture_;
                HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "grab_timeout", -1);
                HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "TriggerMode", "Off");
                HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "TriggerSource", "Software");
                HOperatorSet.GrabImageStart(hv_AcqHandle, -1);
            }
            else
            {
                thread_cpature_mode_ = capture_;
                HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "TriggerMode", "Off");
            }

            // Thread start
            if (capture_mode_ == 2)
            {
                if (hwindow_ != null)
                {
                    b_connect = true;

#if UseNewHardwareFun
                    Init(); // 190318, andy
#endif
                    int myContext = 2;
                    IntPtr ptr = Marshal.GetFunctionPointerForDelegate(delegateCallback);

                    // (20190726)
                    //HTuple Value;
                    //HOperatorSet.GetFramegrabberParam(hv_AcqHandle, "available_callback_types", out Value);

                    HOperatorSet.SetFramegrabberCallback(hv_AcqHandle, CallBackType, ptr, myContext);
                    HOperatorSet.GrabImageStart(hv_AcqHandle, -1);

                    //thread_ = new Thread(thread_cpature_mode_); // 190318, andy
                    //thread_.Priority = ThreadPriority.Highest; // 190318, andy
                    //thread_.Start();

#if UseNewHardwareFun

                    thread_Preprocess = new Thread(preprocessRun); // 190318, andy
                    thread_Preprocess.Start();

#endif

                }
                else
                {
                    b_connect = false;
                    HOperatorSet.CloseFramegrabber(hv_AcqHandle);
                }

            }
            else if (capture_mode_ == 1)
            {
                if (hsmwindow_ != null)
                {
                    b_connect = true;
                    thread_ = new Thread(thread_cpature_mode_);
                    thread_.Start();
                }
                else
                {
                    b_connect = false;
                    HOperatorSet.CloseFramegrabber(hv_AcqHandle);
                }
            }

            // Return
            return b_connect;

                     
        }

        /// <summary>
        /// 僅連接相機不取像
        /// </summary>
        /// <param name="capture_mode_"></param>
        /// <returns></returns>
        public static bool camera_connectNoCapture(int capture_mode_) // (20190620) Jeff Revised!
        {
            bool b_connect = false;
            b_autosize_ = false;

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Image);

            // Get Camera device code
            if (!Get_CameraDeviceCode())
            {
                b_connect = false;
                return b_connect;
            }

            // Adv-Open Framegrabber
            if (!Adv_OpenFramegrabber())
            {
                b_connect = false;
                return b_connect;
            }

            // Get now camera setting
            HOperatorSet.GetFramegrabberParam(hv_AcqHandle, "TriggerSource", out hv_TriggerSource);


            // Set trigger mode
            nowCapture_mode = capture_mode_;
            if (capture_mode_ == 2)
            {
                HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "grab_timeout", -1);
                HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "TriggerMode", "On");
                HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "TriggerSource", "Line0");
                HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "TriggerActivation", "RisingEdge"); // 設定上升緣觸發 (20181207) Jeff Revised!
                //HOperatorSet.GrabImageStart(hv_AcqHandle, -1);
            }
            else if (capture_mode_ == 1)
            {
                HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "grab_timeout", -1);
                HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "TriggerMode", "Off");
                HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "TriggerSource", "Software");
                //HOperatorSet.GrabImageStart(hv_AcqHandle, -1);
            }
            else
            {
                HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "TriggerMode", "Off");
            }

            b_connect = true;

            // Return
            return b_connect;
        }

        public static bool connectCamera(int capture_mode_) // (20190615) Jeff Revised!
        {
            bool b_connect = false;

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Image);

            // Get Camera device code
            if (!Get_CameraDeviceCode())
            {
                b_connect = false;
                return b_connect;
            }

            // Adv-Open Framegrabber
            if (!Adv_OpenFramegrabber())
            {
                b_connect = false;
                return b_connect;
            }

            // Get now camera setting
            HOperatorSet.GetFramegrabberParam(hv_AcqHandle, "TriggerSource", out hv_TriggerSource);


            // Set trigger mode
            nowCapture_mode = capture_mode_;
            if (capture_mode_ == 2)
            {
                HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "grab_timeout", -1);
                HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "TriggerMode", "On");
                HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "TriggerSource", "Line0");
                HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "TriggerActivation", "RisingEdge"); // 設定上升緣觸發 (20181207) Jeff Revised!
                HOperatorSet.GrabImageStart(hv_AcqHandle, -1);
            }
            else if (capture_mode_ == 1)
            {
                HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "grab_timeout", -1);
                HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "TriggerMode", "Off");
                HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "TriggerSource", "Software");
                HOperatorSet.GrabImageStart(hv_AcqHandle, -1);
            }
            else
            {
                HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "TriggerMode", "Off");
            }
            
            // Return
            return b_connect;
        }

        private static void capture_() // (20200717) Jeff Revised!
        {
            //HObject[] ho_temp_ = new HObject[2]; // (20200717) Jeff Revised!
            //HOperatorSet.SetPart(hsmwindow_.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part

            bool b_firstDisp = true;
            while (!b_close_)
            {
                //release_hobject(ref ho_Image);
                HOperatorSet.GrabImage(out ho_Image, hv_AcqHandle);

                if (!b_autosize_)
                {
                    HTuple x, y;
                    HOperatorSet.GetImageSize(ho_Image, out x, out y);
                    //hsmwindow_.SetFullImagePart(new HImage(ho_Image));              

                    b_autosize_ = true;
                }

                if (mDisplayCrossMark)
                {
                    #region Displey CrossMark

                    HObject ho_CrossRegion;
                    GenCrossMark(ho_Image, out ho_CrossRegion); // (20190128) Jeff Revised!

                    // Show Cross mark and image
                    HOperatorSet.SetColor(hsmwindow_.HalconWindow, "red");
                    HOperatorSet.DispObj(ho_Image, hsmwindow_.HalconWindow);
                    //HOperatorSet.SetPart(hsmwindow_.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part
                    HOperatorSet.DispObj(ho_CrossRegion, hsmwindow_.HalconWindow);
                    
                    // 十字影像 (20181215) Jeff Revised!
                    // 轉成 RGB image
                    HTuple Channels;
                    HOperatorSet.CountChannels(ho_Image, out Channels);
                    if (Channels == 1)
                    {
                        //HOperatorSet.Compose3(ho_Image, ho_Image, ho_Image, out CrossImage);
                        HOperatorSet.Compose3(ho_Image.Clone(), ho_Image.Clone(), ho_Image.Clone(), out CrossImage);
                    }
                    else
                    {
                        // For debug
                        //HOperatorSet.Rgb1ToGray(ho_Image, out ho_Image);
                        //HOperatorSet.Compose3(ho_Image.Clone(), ho_Image.Clone(), ho_Image.Clone(), out CrossImage);

                        HOperatorSet.CopyImage(ho_Image, out CrossImage);
                    }
                    HOperatorSet.OverpaintRegion(CrossImage, ho_CrossRegion, ((new HTuple(255)).TupleConcat(0)).TupleConcat(0), "fill");
                    ho_CrossRegion.Dispose();

                    #endregion
                }
                else
                {

                    //HOperatorSet.DispColor(ho_Image, hwindow_.HalconWindow);
                    HOperatorSet.DispObj(ho_Image, hsmwindow_.HalconWindow);
                }
                if (b_firstDisp) // (20190620) Jeff Revised!
                {
                    HOperatorSet.SetPart(hsmwindow_.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part
                    b_firstDisp = false;
                }

                /* 【計算解析度】 */
                if (B_ComputeResol) // (20190128) Jeff Revised!
                    ComputeResol(ho_Image, hsmwindow_, RealDiameter, B_MaxCircle);

                /* 【載入HDVP】 */
                if (B_LoadHDVP) // (20200717) Jeff Revised!
                    class_load_hdvp.Execute_Disp(hsmwindow_, ho_Image);

                if (dict_HWindow_.Count > 0)
                {
                    foreach (KeyValuePair<int, HWindowControl> i in dict_HWindow_)
                    {
                        HOperatorSet.DispColor(ho_Image, i.Value.HalconWindow);
                    }
                }
            }
        }

        public static void GenCrossMark(HObject Image, out HObject CrossRegion) // (20200717) Jeff Revised!
        {
            //Get image size
            HOperatorSet.GenEmptyObj(out CrossRegion);
            HTuple hv_Width = null, hv_Height = null;
            HTuple hv_CenterX = null, hv_CenterY = null;
            HOperatorSet.GetImageSize(Image, out hv_Width, out hv_Height);
            hv_CenterX = hv_Width / 2;
            hv_CenterY = hv_Height / 2;

            int Method = 0;
            if (Method == 0)
            {
                #region Method 0

                // Local iconic variables 
                HObject ho_CrossRegion1;
                HObject ho_Rectangle_h, ho_Rectangle_v;

                // Local control variables 
                HTuple hv_Row1 = null;
                HTuple hv_Column1 = null, hv_Row2 = null, hv_Column2 = null;
                HTuple hv_Range = null;

                // Initialize local and output iconic variables 
                HOperatorSet.GenEmptyObj(out ho_CrossRegion1);
                HOperatorSet.GenEmptyObj(out ho_Rectangle_h);
                HOperatorSet.GenEmptyObj(out ho_Rectangle_v);

                // Setting range
                hv_Range = 0;

                //Set cross feature 水平
                hv_Row1 = hv_CenterY - hv_Range;
                hv_Column1 = 0;
                //Column1 := CenterX-100
                hv_Row2 = hv_CenterY + hv_Range;
                hv_Column2 = hv_Width.Clone();
                //Column2 := CenterX+100
                ho_Rectangle_h.Dispose();
                HOperatorSet.GenRectangle1(out ho_Rectangle_h, hv_Row1, hv_Column1, hv_Row2, hv_Column2);

                //Set cross feature 垂直
                hv_Row1 = 0;
                //Row1 := CenterY-100
                hv_Column1 = hv_CenterX - hv_Range;
                hv_Row2 = hv_Height.Clone();
                //Row2 := CenterY+100
                hv_Column2 = hv_CenterX + hv_Range;
                ho_Rectangle_v.Dispose();
                HOperatorSet.GenRectangle1(out ho_Rectangle_v, hv_Row1, hv_Column1, hv_Row2,
                    hv_Column2);

                //合併水平與垂直
                ho_CrossRegion1.Dispose();
                HOperatorSet.Union2(ho_Rectangle_h, ho_Rectangle_v, out ho_CrossRegion1);

                ho_Rectangle_h.Dispose();
                ho_Rectangle_v.Dispose();

                #endregion

                // Set show cross region
                //CrossRegion = ho_CrossRegion1;
                //HOperatorSet.CopyObj(ho_CrossRegion1, out CrossRegion, 1, -1);
                CrossRegion.Dispose();
                CrossRegion = ho_CrossRegion1;
            }
            else if (Method == 1)
            {
                #region Method 1

                HObject ho_CrossRegion2;
                HOperatorSet.GenEmptyObj(out ho_CrossRegion2);
                HOperatorSet.GenCrossContourXld(out ho_CrossRegion2, hv_CenterY, hv_CenterX, hv_Width, 0);

                #endregion

                HOperatorSet.GenRegionContourXld(ho_CrossRegion2, out CrossRegion, "filled");
                
                // Set show cross region
                //ho_CrossRegion = ho_CrossRegion2;
            }
        }

        #region 計時器

        private static System.Diagnostics.Stopwatch sw_inner = new System.Diagnostics.Stopwatch();
        public static double calc_sw_inner;

        private static System.Diagnostics.Stopwatch sw_preproc = new System.Diagnostics.Stopwatch();
        public static double calc_sw_preproc;

        private static DelVivi.Log.DelLogs camlog_grab = new DelVivi.Log.DelLogs("camlog_grab");
        private static DelVivi.Log.DelLogs camlog_preproc = new DelVivi.Log.DelLogs("camlog_preproc");

        #endregion

        private static void hardware_trigger_capture_()
        {
            
            // Init          
            trigger_count_ = 0;

            // 181126, andy 考慮取消
            HObject[] test_ = new HObject[trigger_time_]; // 一個位置拍攝影像張數 (20181009) Jeff Revised!

            // 181126, andy: 一定點影像集合
            List<HObject> imgs_ = new List<HObject>();
            
            // While start
            while (!b_close_)
            {
                try
                {
                    if(b_clear_)
                    {
                        b_clear_ = false;
                        release_hobject(ref ho_Image);
                        test_ = new HObject[trigger_time_]; // (20181009) Jeff Revised!
                    }
                    release_hobject(ref ho_Image);

                    // 下取像指令
                    HOperatorSet.GrabImageAsync(out ho_Image, hv_AcqHandle, -1);


                    //sw_inner.Reset();
                    //sw_inner.Start();

                    //********************************************************************************************************

                    #region 取得的影像即時顯示
                    if (!b_autosize_)
                    {
                        HTuple x, y;
                        HOperatorSet.GetImageSize(ho_Image, out x, out y);
                        hwindow_.SetFullImagePart(new HImage(ho_Image));
                        b_autosize_ = true;
                    }
                    if (ho_Image != null)
                    {
                        //HOperatorSet.DispColor(ho_Image, hwindow_.HalconWindow);
                        HOperatorSet.DispImage(ho_Image, hwindow_.HalconWindow); // (20181009) Jeff Revised!

                        // Total Trigger count add for show and debug
                        total_trigger_count_++;

                    }
#endregion

                    sw_inner.Reset();
                    sw_inner.Start();

                    // Trigger count add
                    trigger_count_++;

                    // 儲存影像模組所需影像
                    HObject temp = ho_Image.Clone(); // 速度慢, 不穩定

                    sw_inner.Stop();
                    calc_sw_inner = sw_inner.ElapsedMilliseconds;
                    camlog_grab.WriteLine(DelVivi.Log.enMessageType.Debug, calc_sw_inner.ToString());
                    //DelVivi.Log.DelLogs.GlobalInstance.WriteLine(DelVivi.Log.enMessageType.Debug, calc_sw_inner.ToString());

                    // Add to imgs
                    imgs_.Add(temp); // 181126, andy

                    // 181127, andy 更新 每個位置觸發張數
                    trigger_time_ = MovementPos.triggerCount;
                    #region 當儲存的影像到達定點觸發次數時，建立影像模組
                    if (trigger_count_ == trigger_time_)
                    {
                        
                        ImageObj img_obj = new ImageObj();

                        #region 181126, andy 待驗證 ...

                            foreach (var item in imgs_)
                             img_obj.Source.Add(item);
                           
#endregion

                        #region 181126, andy 待驗證 ...
                            
                        test_ = new HObject[trigger_time_];
                        for (int i = 0; i < trigger_time_; i++)
                        {
                            test_[i] = imgs_[i];
                        }

#endregion

                 
                        DeltaSubstrateInspector.src.Modules.clsTriggerImg.trigger_img_.Add(test_);
                        img_obj.MoveIndex = DeltaSubstrateInspector.src.Modules.clsTriggerImg.trigger_img_.Count;
                        
                        img_queue_.Enqueue(img_obj);

                        #region Reset buffer

                        //GC.Collect(); // 190318, andy: !!!!!!!!!!!!!!!! Key
                        //GC.WaitForPendingFinalizers(); // 190318, andy: !!!!!!!!!!!!!!!! Key

                        trigger_count_ = 0;
                        imgs_.Clear(); // 181126, andy
#endregion

                    }
                    #endregion

                    
                    if (dict_HWindow_.Count > 0)
                    {
                        foreach (KeyValuePair<int, HWindowControl> i in dict_HWindow_)
                        {
                            HOperatorSet.DispImage(ho_Image, i.Value.HalconWindow); // (20181009) Jeff Revised!
                        }
                    }

                    //********************************************************************************************************

                    //sw_inner.Stop();
                    //calc_sw_inner = sw_inner.ElapsedMilliseconds;
                    //System.Console.WriteLine(calc_sw_inner);
                    //camlog.WriteLine(DelVivi.Log.enMessageType.Debug, calc_sw_inner.ToString());

                    

                    // Sleep
                    Thread.Sleep(1);

                }
                catch (Exception e)
                {
                    // Exception happen
                    e.ToString();
                    break; 
                }


            }

        }

        #region 190321, andy: New HardwareTriggerCapture !!!!!

        /// <summary>
        /// 取像與前處理Buffer 狀態
        /// </summary>
        private class ResultStatus
        {
            public bool GrabStart;
            public bool GrabOK;

            public bool InspectionStart;
            public bool InspectionOK;

            public bool DisplayStart;
            public bool DisplayOK;

            public bool SaveImageStart;
            public bool SaveImageOK;
            public bool CopySaveImageStart;
            public bool CopySaveImageOK;

            public ResultStatus()
            {
                Reset();
            }

            public void Reset()
            {
                GrabStart = true;
                GrabOK = false;
                InspectionStart = false;
                InspectionOK = false;
                DisplayStart = false;
                DisplayOK = false;
                SaveImageStart = false;
                SaveImageOK = false;
                CopySaveImageStart = true;
                CopySaveImageOK = false; 
            }

        }
        private static ResultStatus[] mResultStatus;

        private static int MaxImageBufferNum;
        private static HObject[] ho_ImageBuffer;
        private static int grabBufferIndex = 0;
        /// <summary>
        /// 取像Thread
        /// </summary>
        private static void hardware_trigger_capture_new()
        {
            
            // While start
            while (!b_close_)
            {
                try
                {
                    mResultStatus[grabBufferIndex].GrabOK = false;

                    release_hobject(ref ho_ImageBuffer[grabBufferIndex]); // 190325, new

                    // 下取像指令
                    HOperatorSet.GrabImageAsync(out ho_ImageBuffer[grabBufferIndex], hv_AcqHandle, -1);

                    
                    // Change state
                    mResultStatus[grabBufferIndex].GrabOK = true;
                    mResultStatus[grabBufferIndex].GrabStart = false;
                    mResultStatus[grabBufferIndex].InspectionStart = true;

                    // Time log
                    //if(grabBufferIndex==0)
                    //{
                    //    sw_inner.Reset();
                    //    sw_inner.Start();
                    //}

                    // Grab count
                    grabBufferIndex++;

                    // reset nowBufID
                    if (grabBufferIndex >= MaxImageBufferNum)
                    {
                        // Time log
                        //sw_inner.Stop();
                        //calc_sw_inner = sw_inner.ElapsedMilliseconds;
                        //camlog_grab.WriteLine(DelVivi.Log.enMessageType.Info, "GrabAllTime: " + calc_sw_inner.ToString());

                        grabBufferIndex = 0;

                    }

                    

                    // NO SLEEP --> 全速 
                    //Thread.Sleep(1);
                    //SpinWait.SpinUntil(() => false, 1); //better than sleep(1)

                }
                catch (Exception e)
                {
                    // Log
                    camlog_grab.WriteLine(DelVivi.Log.enMessageType.Error, e.ToString());

                    // 跳出此Thread
                    break;
                }

            }

        }

        private static int inspectionBufferIndex = 0;
        private static void preprocessRun()
        {

            #region PreProc Init

            // Init          
            trigger_count_ = 0;

            // 181126, andy 考慮取消
            HObject[] test_ = new HObject[trigger_time_]; // 一個位置拍攝影像張數 (20181009) Jeff Revised!

            // 181126, andy: 一定點影像集合
            List<HObject> imgs_ = new List<HObject>();

            #endregion


            // While start
            while (!b_close_)
            {

                try
                {

                    if (b_clear_)
                    {
                        b_clear_ = false;
                        test_ = new HObject[trigger_time_]; // (20181009) Jeff Revised!
                    }

                    // Check status
                    if (mResultStatus[inspectionBufferIndex].InspectionStart == false) continue;

                    // Change state
                    mResultStatus[inspectionBufferIndex].InspectionOK = false;

                    // Time log
                    //if(inspectionBufferIndex==0)
                    //{
                    //    sw_preproc.Reset();
                    //    sw_preproc.Start();
                    //}

                    #region Pre-process


                    #region 取得的影像即時顯示
                    ho_Image = ho_ImageBuffer[inspectionBufferIndex];
                    if (!b_autosize_)
                    {
                        HTuple x, y;
                        HOperatorSet.GetImageSize(ho_Image, out x, out y);
                        hwindow_.SetFullImagePart(new HImage(ho_Image));
                        b_autosize_ = true;
                    }
                    if (ho_Image != null)
                    {
                        //HOperatorSet.DispColor(ho_Image, hwindow_.HalconWindow);
                        HOperatorSet.DispImage(ho_Image, hwindow_.HalconWindow); // (20181009) Jeff Revised!

                        // Total Trigger count add for show and debug
                        total_trigger_count_++;

                    }
                    #endregion

                   
                    // Trigger count add
                    trigger_count_++;

                    // 儲存影像模組所需影像
                    //HObject temp = ho_Image.Clone(); // 速度慢, 不穩定
                    HObject temp = ho_ImageBuffer[inspectionBufferIndex].Clone();

                    // Add to imgs
                    imgs_.Add(temp); // 181126, andy

                    // 181127, andy 更新 每個位置觸發張數
                    trigger_time_ = MovementPos.triggerCount;
                    #region 當儲存的影像到達定點觸發次數時，建立影像模組
                    if (trigger_count_ == trigger_time_)
                    {

                        ImageObj img_obj = new ImageObj();

                        #region 181126, andy 待驗證 ...

                        foreach (var item in imgs_)
                            img_obj.Source.Add(item);

                        #endregion

                        #region 181126, andy 待驗證 ...

                        test_ = new HObject[trigger_time_];
                        for (int i = 0; i < trigger_time_; i++)
                        {
                            test_[i] = imgs_[i];
                        }

                        #endregion


                        DeltaSubstrateInspector.src.Modules.clsTriggerImg.trigger_img_.Add(test_);
                        img_obj.MoveIndex = DeltaSubstrateInspector.src.Modules.clsTriggerImg.trigger_img_.Count;


                        img_queue_.Enqueue(img_obj);

                        #region Reset buffer

                        //GC.Collect(); // 190318, andy: !!!!!!!!!!!!!!!! Key
                        //GC.WaitForPendingFinalizers(); // 190318, andy: !!!!!!!!!!!!!!!! Key

                        trigger_count_ = 0;
                        imgs_.Clear(); // 181126, andy
                        #endregion

                    }
                    #endregion


                    if (dict_HWindow_.Count > 0)
                    {
                        foreach (KeyValuePair<int, HWindowControl> i in dict_HWindow_)
                        {
                            HOperatorSet.DispImage(ho_Image, i.Value.HalconWindow); // (20181009) Jeff Revised!
                        }
                    }


                    #endregion


                    // Change state
                    mResultStatus[inspectionBufferIndex].InspectionOK = true;
                    mResultStatus[inspectionBufferIndex].InspectionStart = false;
                    mResultStatus[inspectionBufferIndex].GrabStart = true;


                    // Count
                    inspectionBufferIndex++;

                    // 判斷是否所有影像計算完成
                    if (inspectionBufferIndex >= MaxImageBufferNum)
                    {
                        // Time log
                        //sw_preproc.Stop();
                        //calc_sw_preproc = sw_preproc.ElapsedMilliseconds;
                        //camlog_preproc.WriteLine(DelVivi.Log.enMessageType.Info, "PreprocAllTime: " + calc_sw_preproc.ToString());
                        //DelVivi.Log.DelLogs.GlobalInstance.WriteLine(DelVivi.Log.enMessageType.Debug, calc_sw_inner.ToString());

                        // reset
                        inspectionBufferIndex = 0;

                    }

                    // Sleep
                    //Thread.Sleep(10);
                    SpinWait.SpinUntil(() => false, 10); //better than sleep(10)

                }
                catch (Exception e)
                {
                    // Log
                    camlog_preproc.WriteLine(DelVivi.Log.enMessageType.Error, e.ToString());

                    // 跳出此Thread
                    break;
                }

            }

        }

        private static void Init()
        {
            MaxImageBufferNum = (MovementPos.locat_pos + MovementPos.row * MovementPos.col) * MovementPos.triggerCount;

            ho_ImageBuffer = new HObject[MaxImageBufferNum];

            mResultStatus = new ResultStatus[MaxImageBufferNum];
            for (int i = 0; i < MaxImageBufferNum; i++)
                mResultStatus[i] = new ResultStatus();

            grabBufferIndex = 0;
            inspectionBufferIndex = 0;
            
        }

#endregion

        public static void close_camera()
        {
            if (hv_AcqHandle != null)
            {
                // 190108, andy: 連續取像(software)與線上取像(hardware)皆可
                if (nowCapture_mode == 1)
                {
                    if (thread_ != null) // (20190620) Jeff Revised!
                    {
                        thread_.Abort();
                        thread_.Join();
                    }
                }

                try // (20190726) Jeff Revised!
                {
                    HOperatorSet.SetFramegrabberCallback(hv_AcqHandle, CallBackType, 0, 0);
                }
                catch (Exception ex)
                { }
                
                if (CameraInterface == (int)(enu_CameraInterface.USB3))
                {
                    HTuple hv_trigger_mode_;
                    HOperatorSet.GetFramegrabberParam(hv_AcqHandle, "TriggerMode", out hv_trigger_mode_);
                    HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "TriggerMode", "Off");
                }
                else if(CameraInterface == (int)(enu_CameraInterface.GigE))
                {
                    HTuple hv_trigger_mode_;
                    HOperatorSet.GetFramegrabberParam(hv_AcqHandle, "TriggerMode", out hv_trigger_mode_);

                    //HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "TriggerMode", "Off"); // Fail @this condition
                    HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "do_abort_grab", 1);

                  
                }  
                
                        
                b_close_ = true;              
                HOperatorSet.CloseFramegrabber(hv_AcqHandle);
                hv_AcqHandle = null;

            }

            try
            {
                //HOperatorSet.ClearWindow(hwindow_.HalconWindow);
            }
            catch { }
            //release_hobject(ref ho_Image);
            b_close_ = false; // ? (20190620) Jeff Revised!

        }

        public static void software_trigger()
        {
            HTuple hv_trigger_mode_, hv_trigger_source_;
            HOperatorSet.GetFramegrabberParam(hv_AcqHandle, "TriggerMode", out hv_trigger_mode_);
            HOperatorSet.GetFramegrabberParam(hv_AcqHandle, "TriggerSource", out hv_trigger_source_);
            if (hv_trigger_mode_.S == "On" && hv_trigger_source_=="Software")
                HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "TriggerSoftware", 1);
        }

        private static void release_hobject(ref HObject hobject_)
        {
            if (hobject_ != null)
            {
                hobject_.Dispose();
                hobject_ = null;
            }
        }

        /// <summary>
        /// 是否顯示中心十字
        /// </summary>
        private static bool mDisplayCrossMark { get; set; } = false;
        /// <summary>
        /// 是否顯示中心十字
        /// </summary>
        /// <param name="_mDisplayCrossMark"></param>
        public static void DisplayCrossMark(bool _mDisplayCrossMark = false)
        {
            mDisplayCrossMark = _mDisplayCrossMark;
        }
        
        #region 【計算解析度】 & 【載入HDVP】

        private static bool B_ComputeResol { get; set; } = false; // (20200717) Jeff Revised!
        private static bool B_MaxCircle { get; set; } = true; // (20200717) Jeff Revised!
        private static double RealDiameter { get; set; } = 10.0; // (20200717) Jeff Revised!

        public static void Update_ComputeResol(bool b_ComputeResol, double RealDiameter_mm, bool b_MaxCircle) // (20190128) Jeff Revised!
        {
            B_ComputeResol = b_ComputeResol;
            RealDiameter = RealDiameter_mm;
            B_MaxCircle = b_MaxCircle;
        }

        public static bool ComputeResol(HObject Image, HSmartWindowControl hsWindow, double RealDiameter_mm, bool b_MaxCircle = true) // (20200717) Jeff Revised!
        {
            bool b_status_ = false;

            // Local iconic variables 
            HObject ho_GrayImage, ho_Edges, ho_Holes, ho_Hole;

            // Local control variables
            HTuple hv_Row = null, hv_Column = null, hv_Radius = null, hv_Radius_Sorted = null;
            HTuple hv_Number = null;
            HTuple hv_DistanceMin = new HTuple(), hv_DistanceMax = new HTuple();
            HTuple hv_Resolution = new HTuple();

            // Initialize local and output iconic variables
            HOperatorSet.GenEmptyObj(out ho_GrayImage);
            HOperatorSet.GenEmptyObj(out ho_Edges);
            HOperatorSet.GenEmptyObj(out ho_Holes);
            HOperatorSet.GenEmptyObj(out ho_Hole);

            try
            {
                clsStaticTool.set_display_font(hsWindow.HalconWindow, 14, "mono", "false", "false");
                ho_GrayImage.Dispose();
                HOperatorSet.Rgb1ToGray(Image, out ho_GrayImage);

                // Segment the circular holes
                // The alpha parameter was choosen different than the default value
                // to ensure stronger smoothing and thusmore connected edge components
                ho_Edges.Dispose();
                HOperatorSet.EdgesSubPix(ho_GrayImage, out ho_Edges, "canny", 4, 20, 40);
                ho_Holes.Dispose();
                HOperatorSet.SelectShapeXld(ho_Edges, out ho_Holes, "circularity", "and", 0.99, 1.0);
                // Determine the midpoints
                HOperatorSet.SmallestCircleXld(ho_Holes, out hv_Row, out hv_Column, out hv_Radius);

                int Method = 2;
                if (Method == 1) // Method 1
                {
                    HTuple hv_MaxMin_diameter;

                    // 篩選出最大or最小圓
                    HOperatorSet.TupleSort(hv_Radius, out hv_Radius_Sorted);
                    if (b_MaxCircle)
                        hv_MaxMin_diameter = (hv_Radius_Sorted.TupleSelect((new HTuple(hv_Radius.TupleLength())) - 1)) * 2;
                    else
                        hv_MaxMin_diameter = hv_Radius_Sorted.TupleSelect(0) * 2;
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.SelectShapeXld(ho_Holes, out ExpTmpOutVar_0, "max_diameter", "and", hv_MaxMin_diameter - 1, hv_MaxMin_diameter + 1);
                        ho_Holes.Dispose();
                        ho_Holes = ExpTmpOutVar_0;
                    }
                }
                else // Method 2 // (20200717) Jeff Revised!
                {
                    HTuple hv_Radius_SortedIndex = hv_Radius.TupleSortIndex();
                    int index;
                    if (b_MaxCircle)
                        index = hv_Radius_SortedIndex[hv_Radius.Length - 1];
                    else
                        index = hv_Radius_SortedIndex[0];
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.SelectObj(ho_Holes, out ExpTmpOutVar_0, index + 1);
                        ho_Holes.Dispose();
                        ho_Holes = ExpTmpOutVar_0;
                    }
                }

                HOperatorSet.SmallestCircleXld(ho_Holes, out hv_Row, out hv_Column, out hv_Radius);
                HOperatorSet.CountObj(ho_Holes, out hv_Number);
                
                // 非找出一個圓形
                if (hv_Number != 1)
                    return false;

                ho_Hole.Dispose();
                HOperatorSet.SelectObj(ho_Holes, out ho_Hole, 1);

                // Compute the minimal and maximal radius of the holes by computing the distance of the midpoint to the contour
                HOperatorSet.DistancePc(ho_Hole, hv_Row.TupleSelect(0), hv_Column.TupleSelect(0), out hv_DistanceMin, out hv_DistanceMax);

                // Visualize the results (minimal and maximal radius)
                HOperatorSet.SetLineWidth(hsWindow.HalconWindow, 5);
                HOperatorSet.SetColor(hsWindow.HalconWindow, "yellow");
                HOperatorSet.DispObj(ho_Hole, hsWindow.HalconWindow);
                HOperatorSet.DispCross(hsWindow.HalconWindow, hv_Row.TupleSelect(0), hv_Column.TupleSelect(0), 6, 0);

                // Compute resolution
                hv_Resolution = (RealDiameter_mm * 1000) / (hv_DistanceMin + hv_DistanceMax);
                clsStaticTool.disp_message(hsWindow.HalconWindow, "Resolution: " + (hv_Resolution.TupleString(".4f")) + " μm/pixel", "image",
                             (hv_Row.TupleSelect(0)) + 0.2 * (hv_Radius.TupleSelect(0)),
                             (hv_Column.TupleSelect(0)) - 1.2 * (hv_Radius.TupleSelect(0)), "blue", "true");

                b_status_ = true;
            }
            catch (HalconException HDevExpDefaultException)
            {
                //throw HDevExpDefaultException;
            }
            finally
            {
                ho_GrayImage.Dispose();
                ho_Edges.Dispose();
                ho_Holes.Dispose();
                ho_Hole.Dispose();
            }

            return b_status_;
        }

        private static bool B_LoadHDVP { get; set; } = false; // (20200717) Jeff Revised!

        private static Class_load_hdvp class_load_hdvp { get; set; } = new Class_load_hdvp(); // (20200717) Jeff Revised!

        public static void Update_LoadHDVP(bool b_LoadHDVP, Class_load_hdvp class_load_hdvp_) // (20200717) Jeff Revised!
        {
            B_LoadHDVP = b_LoadHDVP;
            class_load_hdvp = class_load_hdvp_;
        }

        #endregion

        #region 進階設定內 相機模組

        #endregion
    }
}
