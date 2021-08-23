using HalconDotNet;
using DeltaSubstrateInspector.src.Models.CaptureModel;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeltaSubstrateInspector.src.Modules.CaptureModule
{
    public class CaptureModel
    {
        ConcurrentQueue<ImageObj> img_queue_ = new ConcurrentQueue<ImageObj>();

        private Hashtable camera_table_ = new Hashtable();  // Table for the cameras.
        private CameraModule camera_ = new CameraModule();
        private int _runMode = 2;

        // 181121, andy
        //private bool mCameraInitOK = false;
        public bool CameraInitOK
        {
            get { return camera_.CameraInitOK; }           
        }

        public CaptureModel()
        {
            camera_table_.Add("On-line", new Camera());
            camera_table_.Add("Off-line-all", new SimulationCamera("All"));
            camera_table_.Add("Off-line-Cycle", new SimulationCamera("Cycle"));
            camera_table_.Add("Off-line-single", new SimulationCamera("Single"));
            camera_table_.Add("Off-line-Mult", new SimulationCamera("Mult"));
        }

        public void read_image_from_disk(bool is_single)
        {
            string path = "D://@Work//uPOL//Golden Image//GoldenImg";
            List<string> files = new List<string>();
            List<string> light_name = new List<string> { "同軸光", "低角度" };
            List<HObject> img_list = new List<HObject>();

            if (Directory.Exists(path))
            {
                files.AddRange(Directory.GetFiles(path));
            }

            foreach (var filename in files)
            {

                string[] info = filename.Split('_');
                if (light_name.Contains(info[0]))
                {
                    HObject img;
                    HOperatorSet.ReadImage(out img, filename);
                    img_list.Add(img);
                }

            }

        }

        public void set_showup_window(HWindowControl window)
        {
            camera_.Window = window;
        }

        public void set_showup_smwindow(HSmartWindowControl smwindow)
        {
            camera_.SMWindow = smwindow;
        }


        public ConcurrentQueue<ImageObj> get_img_queue()
        {
           return camera_.ImageQueue;
        }

        public void excute(string type)
        {
            camera_ = (CameraModule)camera_table_[type];

            camera_.RunMode = _runMode;

            camera_.run();
        }

        /// <summary>
        /// 僅連接相機不取像
        /// </summary>
        /// <param name="type"></param>
        public void excute_connectNoCapture(string type) // (20190620) Jeff Revised!
        {
            camera_ = (CameraModule)camera_table_[type];

            camera_.RunMode = _runMode;

            camera_.run_connectNoCapture();
        }

        public ConcurrentQueue<ImageObj> img_queue()
        {
            return camera_.ImageQueue;
        }

        public void SetCameraRunMode(int opt)
        {
            _runMode = opt;
        }


    }

}
