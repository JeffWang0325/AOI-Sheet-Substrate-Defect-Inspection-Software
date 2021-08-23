using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Extension;

namespace DeltaSubstrateInspector.src.PositionMethod
{
    public partial class VirtualSeg1Form : Form
    {
        private ImageManager img_manager_;
        private int min_threshold_val_ = 0;
        private int max_threshold_val_ = 0;

        public VirtualSeg1Form()
        {
            InitializeComponent();
        }

        private void set_color_method(object sender, EventArgs e)
        {
            ToolStripMenuItem color_method = sender as ToolStripMenuItem;
            img_manager_.set_color_space(color_method.Text);
        }

        private void setup_color_space(object sender, EventArgs e)
        {
            ToolStripMenuItem color_space = sender as ToolStripMenuItem;
            pic_preview.Image = img_manager_.get_color_space(color_space.Text).GetRGBBitmap();
        }

        private void btn_loadImg_Click(object sender, EventArgs e)
        {
            OpenFileDialog openImg = new OpenFileDialog();
            openImg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openImg.Filter = "BMP File| *.bmp|所有檔案 (*.*)|*.*";
            if (openImg.ShowDialog() == DialogResult.OK)
            {
                img_manager_ = new ImageManager(openImg.FileName);
                pic_src_img.Image = img_manager_.get_src_img();
                pic_preview.Image = img_manager_.get_result_img();
                tkb_dilation.Enabled = true;
                tkb_erosion.Enabled = true;
                tkb_max_thr.Enabled = true;
                tkb_min_thr.Enabled = true;
                lines_ToolStripMenuItem.Enabled = true;
            }
        }

        private void tkb_min_thr_Scroll(object sender, EventArgs e)
        {
            TrackBar tkb = sender as TrackBar;
            min_threshold_val_ = tkb.Value;
            img_manager_.threshold_operate(min_threshold_val_, max_threshold_val_);
            pic_preview.Image = img_manager_.get_result_img();
        }

        private void tkb_max_thr_Scroll(object sender, EventArgs e)
        {
            TrackBar tkb = sender as TrackBar;
            max_threshold_val_ = tkb.Value;
            img_manager_.threshold_operate(min_threshold_val_, max_threshold_val_);
            pic_preview.Image = img_manager_.get_result_img();
        }

        private void tkb_erosion_Scroll(object sender, EventArgs e)
        {
            TrackBar tkb = sender as TrackBar;
            img_manager_.erosion_operate(tkb.Value);
            pic_preview.Image = img_manager_.get_result_img();
        }

        private void tkb_dilation_Scroll(object sender, EventArgs e)
        {
            TrackBar tkb = sender as TrackBar;
            img_manager_.dilation_operate(tkb.Value);
            pic_preview.Image = img_manager_.get_result_img();
        }

        private void MenuItem_save_file_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_operate_Click(object sender, EventArgs e)
        {

        }

        private class ImageManager
        {
            private HObject src_img_ = new HObject();
            private HObject dst_img_ = new HObject();
            private HObject gray_img_ = new HObject();
            private HObject dst_region_ = null;
            private string color_method_ = "";
            private HObject[] channel_imgs = new HObject[3];
            private HTuple width, height;
           
            public ImageManager( string filemap )
            {
                HOperatorSet.ReadImage(out src_img_, filemap);
                HOperatorSet.Rgb1ToGray(src_img_, out dst_img_);
                HOperatorSet.Rgb1ToGray(src_img_, out gray_img_);
                HOperatorSet.GetImageSize(dst_img_, out width, out height);
               
            }

            private void paint_result_region()
            {
                HObject result = new HObject();
                HOperatorSet.GenImageConst(out result, "byte", width, height);
                HOperatorSet.PaintRegion(dst_region_, result, out dst_img_, 255, "fill");
                Extension.HObjectMedthods.ReleaseHObject(ref result);
            }

            public Bitmap get_src_img()
            {
                return src_img_.GetRGBBitmap();
            }

            public Bitmap get_result_img()
            {
                if (dst_region_ != null)
                    paint_result_region();

                return dst_img_.GetRGBBitmap();
            }

            public void threshold_operate(int min_val, int max_val)
            {
                if (min_val > max_val)
                    min_val = max_val;

                HOperatorSet.Threshold(gray_img_, out dst_region_, min_val, max_val);
            }

            public void erosion_operate(int erosion_val)
            {
                HOperatorSet.ErosionCircle(dst_region_, out dst_region_, erosion_val);
            }

            public void dilation_operate(int dilation_val)
            {
                HOperatorSet.DilationCircle(dst_region_, out dst_region_, dilation_val);
            }

            public void set_color_space(string color_method)
            {
                HOperatorSet.Decompose3(src_img_, out channel_imgs[0], out channel_imgs[1], out channel_imgs[2]);

                if (color_method == "LCH")
                {
                    HOperatorSet.TransFromRgb(channel_imgs[0], channel_imgs[1], channel_imgs[2], out channel_imgs[0], out channel_imgs[1], out channel_imgs[2], "cielchuv");
                }
                else if (color_method == "HSV")
                {
                    HOperatorSet.TransFromRgb(channel_imgs[0], channel_imgs[1], channel_imgs[2], out channel_imgs[0], out channel_imgs[1], out channel_imgs[2], "hsv");
                }

                color_method_ = color_method;
            }

            public HObject get_color_space(string color_space)
            {
                gray_img_ = channel_imgs[color_method_.IndexOf(color_space)]; 
                return channel_imgs[color_method_.IndexOf(color_space)];
            }
        }
    }
}
