using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using Extension;

namespace DeltaSubstrateInspector.src.PositionMethod
{
    public partial class GoldenTeach : Form
    {
        public GoldenTeach()
        {
            InitializeComponent();
        }

        private void btn_load_img_Click(object sender, EventArgs e)
        {
            OpenFileDialog openImg = new OpenFileDialog();
            openImg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openImg.Filter = "BMP File| *.bmp|所有檔案 (*.*)|*.*";
            if (openImg.ShowDialog() == DialogResult.OK)
            {
                HObject src_img_;
                HOperatorSet.ReadImage(out src_img_, openImg.FileName);
                //img_manager_ = new ImageManager();
                pic_showup.Image = src_img_.GetRGBBitmap();
                //pic_src_img.Image = img_manager_.get_src_img();
                //pic_preview.Image = img_manager_.get_result_img();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btn_rect_add_Click(object sender, EventArgs e)
        {

        }

        private void btn_rect_ok_Click(object sender, EventArgs e)
        {

        }

        private void btn_learn_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void btn_save_Click(object sender, EventArgs e)
        {

        }
    }
}
