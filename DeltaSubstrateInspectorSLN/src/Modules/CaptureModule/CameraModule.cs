using HalconDotNet;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeltaSubstrateInspector.src.Modules.CaptureModule
{
    public class CameraModule
    {
        protected static ConcurrentQueue<ImageObj> img_queue_ { get; set; } = new ConcurrentQueue<ImageObj>();
        private int move_index_ = 0; 
        
        private int movement_count_ = 0;
        private int light_count_ = 0;
        protected static HWindowControl hwindow_ = null;
        protected static HSmartWindowControl hsmwindow_ = null;
     

        // 181121, andy
        private bool mCameraInitOK = false;
        public bool CameraInitOK
        {
            get { return mCameraInitOK; }
            set { this.mCameraInitOK = value; }
        }


        private int mRunMode = 2;// 181128, andy
        public int RunMode
        {
            get { return mRunMode; }
            set { this.mRunMode = value; }
        }


        public virtual void run() { }

        public virtual void run_connectNoCapture() { } // (20190620) Jeff Revised!

        public virtual void create_img_obj() {}

        public ConcurrentQueue<ImageObj> ImageQueue
        {
            get { return img_queue_; }
        }
       
        public int MovementCount
        {
            get { return this.movement_count_; }
            set { this.movement_count_ = value; }
        }

        public int LightCount
        {
            get { return this.light_count_; }
            set { this.light_count_ = value; }
        }

        public HWindowControl Window
        {
            get { return hwindow_; }
            set { hwindow_ = value; }
        }

        public HSmartWindowControl SMWindow
        {
            get { return hsmwindow_; }
            set { hsmwindow_ = value; }
        }


    }
}
