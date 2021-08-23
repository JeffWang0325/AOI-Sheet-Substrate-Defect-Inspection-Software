using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using Extension;

namespace DeltaSubstrateInspector.src.InspectionForms.UC
{
    public partial class IThresholdCtrl : UserControl
    {
        private HObject source_img_;
        private HObject dst_img_, result = new HObject();
        private HObject dst_region = new HObject();
        private int min_threshold_val_ = 0;
        private int max_threshold_val_ = 255;
        private HTuple width, height;
        public event EventHandler AddButtonClicked;

        public IThresholdCtrl()
        {
            InitializeComponent();
        }

        public event EventHandler output_threshold_click;

        public void set_source(HObject img)
        {
            source_img_ = img;
            dst_img_ = img;
            picture_big.Image = img.GetRGBBitmap();
            trackBar1.Enabled = true;
            trackBar2.Enabled = true;
            btn_add.Enabled = true;
            HOperatorSet.GetImageSize(img, out width, out height);
            HOperatorSet.GenImageConst(out result, "byte", width, height);
        }

        private void tkb_min_thr_Scroll(object sender, EventArgs e)
        {
            TrackBar tkb = sender as TrackBar;
            min_threshold_val_ = tkb.Value;
            threshold_operate(min_threshold_val_, max_threshold_val_);
            picture_big.Image = dst_img_.GetRGBBitmap();
        }

        private void tkb_max_thr_Scroll(object sender, EventArgs e)
        {
            TrackBar tkb = sender as TrackBar;
            max_threshold_val_ = tkb.Value;
            threshold_operate(min_threshold_val_, max_threshold_val_);
            picture_big.Image = dst_img_.GetRGBBitmap();
        }

        public void threshold_operate(int min_val, int max_val)
        {
            if (min_val > max_val)
                min_val = max_val;

            lbl_max_val.Text = max_val.ToString();
            lbl_min_val.Text = min_val.ToString();
            HOperatorSet.Threshold(source_img_, out dst_region, min_val, max_val);
            if (dst_region != null)
            {
                HOperatorSet.PaintRegion(dst_region, result, out dst_img_, 255, "fill");
            }
        }

        private void  btn_add_Click(object sender, EventArgs e)
        {
            AddButtonClicked(this, e);
        }

        public string[] get_threshold()
        {
            string[] threshold_val = { min_threshold_val_.ToString(), max_threshold_val_.ToString() };
            return threshold_val;
        }

        public void reset_image()
        {
            set_source(source_img_);
        }

        public Bitmap get_threshold_result()
        {
            return dst_img_.GetRGBBitmap();
        }

    }
}
