using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using HalconDotNet;
using Extension;

namespace Risitanse_AOI.UIControl
{
    public partial class ColorSpaceSetting : UserControl
    {
        private HObject source_img_;
        private int selected_index = 0;
        private List<HObject> channel_imgs;

        public ColorSpaceSetting()
        {
            InitializeComponent();
            channel_imgs = new List<HObject>();
            HOperatorSet.ReadImage(out source_img_, "C:/Users/anita.yang/Desktop/1.PNG");
        }

        public void set_source_img(HObject source)
        {
            source_img_ = source;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            try
            {
                set_channels(cmb.SelectedItem.ToString());
                set_imgs();
            }
            catch { }
        }

        private void set_channels(string colorspace)
        {
            HObject chl_red, chl_green, chl_blue;

            channel_imgs.Clear();
            HOperatorSet.Decompose3(source_img_, out chl_red, out chl_green, out chl_blue);

            if (colorspace == "RGB")
            {
                channel_imgs.Add(chl_red);
                channel_imgs.Add(chl_green);
                channel_imgs.Add(chl_blue);
            }
            else
            {
                try
                {
                    HObject ch1, ch2, ch3;
                    HOperatorSet.TransFromRgb(chl_red, chl_green, chl_blue, out ch1, out ch2, out ch3, colorspace);
                }
                catch
                {
                    HTuple width, height;
                    HObject temp_img;
                    HOperatorSet.GetImageSize(source_img_, out width, out height);
                    HOperatorSet.GenImageConst(out temp_img, "byte", width, height);
                    channel_imgs.Add(temp_img);
                    channel_imgs.Add(temp_img);
                    channel_imgs.Add(temp_img);
                }
            }
        }

        private void set_imgs()
        {
            try
            {
                channel_1_img.Image = channel_imgs[0].GetRGBBitmap();
                channel_2_img.Image = channel_imgs[1].GetRGBBitmap();
                channel_3_img.Image = channel_imgs[2].GetRGBBitmap();
            }
            catch { }
        }

        public HObject get_selected_img()
        {
            return channel_imgs[selected_index];
        }

        private void channel_img_Click(object sender, EventArgs e)
        {
            PictureBox pics = sender as PictureBox;
            selected_index = Convert.ToInt32(pics.Tag) - 1;
        }
    }
}
