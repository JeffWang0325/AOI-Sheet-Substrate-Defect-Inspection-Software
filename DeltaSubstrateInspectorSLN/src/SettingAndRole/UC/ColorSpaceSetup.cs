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
using System.IO;
using System.Collections;

namespace DeltaSubstrateInspector.UIControl
{
    public partial class ColorSpaceSetup : UserControl
    {
        private HObject source_img_;
        private HObject dst_img_;
        private int selected_index_ = 0;
        private List<HObject> channel_imgs;
        private string color_space_ = "normal";
        private string light_img_ = "";
        public event EventHandler OnUserControlButtonClicked;
        private List<string> image_files_name_list_;
        private string file_name_ = "";
        private Hashtable img_table_ = new Hashtable();


        public ColorSpaceSetup()
        {
            InitializeComponent();
            channel_imgs = new List<HObject>();  
        }

        // Set up actions and image for operation 
        public void set_source(List<HObject> src)
        {
            // Setup global image 
            img_table_.Add("OL", src[0]);
            img_table_.Add("BL", src[1]);
            source_img_ = (HObject)img_table_["OL"];
            set_channels();
            set_imgs();

            // Set Action for each channel images
            this.channel_1_img.Click += new EventHandler(OnButtonClicked);
            this.channel_2_img.Click += new EventHandler(OnButtonClicked);
            this.channel_3_img.Click += new EventHandler(OnButtonClicked);
        }

        // Set color space 
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            color_space_ = cmb.Text;
            set_channels();
            set_imgs();
        }

        // Set 3 channels' content from selected color space 
        private void set_channels()
        {
            channel_imgs.Clear();
            try
            {
                HObject chl_red, chl_green, chl_blue;
                HOperatorSet.Decompose3(source_img_, out chl_red, out chl_green, out chl_blue);

                if (color_space_ == "normal")
                {
                    HOperatorSet.Rgb3ToGray(chl_red, chl_green, chl_blue, out dst_img_);
                    channel_imgs.Add(dst_img_);
                    channel_imgs.Add(dst_img_);
                    channel_imgs.Add(dst_img_);
                }
                else if (color_space_ == "RGB")
                {
                    channel_imgs.Add(chl_red);
                    channel_imgs.Add(chl_green);
                    channel_imgs.Add(chl_blue);
                }
                else if (color_space_ == "HSV")
                {
                    HObject ch1, ch2, ch3;
                    HOperatorSet.TransFromRgb(chl_red, chl_green, chl_blue, out ch1, out ch2, out ch3, "hsv");
                    channel_imgs.Add(ch1);
                    channel_imgs.Add(ch2);
                    channel_imgs.Add(ch3);
                }
                else if (color_space_ == "LCH")
                {
                    HObject ch1, ch2, ch3;
                    HOperatorSet.TransFromRgb(chl_red, chl_green, chl_blue, out ch1, out ch2, out ch3, "cielchuv");
                    channel_imgs.Add(ch1);
                    channel_imgs.Add(ch2);
                    channel_imgs.Add(ch3);
                }
            }
            catch
            {
                channel_imgs.Add(null);
                channel_imgs.Add(null);
                channel_imgs.Add(null);
            }
        }

        // Load 3 channels' content in each picturebox
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

        // Set light image
        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            this.light_img_ = cmb.Text;
            source_img_ = (HObject)img_table_[cmb.Text];
            set_channels();
            set_imgs();
        }
        // The action result conacted with the caller which use this User Control
        private void OnButtonClicked(object sender, EventArgs e)
        {
            PictureBox pics = sender as PictureBox;
            selected_index_ = Convert.ToInt32(pics.Tag) - 1;
            OnUserControlButtonClicked(this, e);
        }

        public HObject get_selected_img()
        {
            try
            {
                return channel_imgs[selected_index_];
            }
            catch { return null; }
        }

        public string get_color_space()
        {
            return color_space_;
        }

        public int get_channel()
        {
            return selected_index_;
        }

        public string get_light_img()
        {
            return light_img_;
        }

        private void load_img_Click(object sender, EventArgs e)
        {
            //HObject img;
            //HOperatorSet.ReadImage(out img, file_select());
            //set_source(img);
        }

        private string file_select()
        {
            string file_direction = "";
            OpenFileDialog open_file_dialog = new OpenFileDialog();
            if (open_file_dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                file_direction = open_file_dialog.FileName;
            }
            return file_direction;
        }

        private void load_imgs_from_filemap(string filemap)
        {
            string folder_filemap = Path.GetDirectoryName(filemap);
            var extensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".bmp", ".tif", "jpg", "png" };
            var valid_img_filename = Directory.EnumerateFiles(folder_filemap, "*.*").Where(s => extensions.Any(ext => ext == Path.GetExtension(s).ToLower())).OrderBy(s => s);
            file_name_ = Path.GetFileNameWithoutExtension(filemap);
            image_files_name_list_ = valid_img_filename.ToList();
        }
    }
}
