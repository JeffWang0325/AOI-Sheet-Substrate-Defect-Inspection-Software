using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeltaSubstrateInspector.src.InspectionForms.UC
{
    public partial class IRoleItem : UserControl
    {
        private string color_ = "一般";
        private int channel_ = 0;
        private string min_threshold_val_ = "0";
        private string max_threshold_val_ = "255";
        private string method_ = "";
        private string light_img_ = "";
        private bool is_selected_ = false;
        private int id_ = 0;

        public IRoleItem()
        {
            InitializeComponent();
        }

        public void set_color(string color, int channel)
        {
            lbl_color.Text = color;
            color_ = color;
            channel_ = channel;
        }

        public void set_threshold(string min_val, string max_val)
        {
            lbl_threshold.Text = string.Format("{0} - {1}",min_val, max_val);

            // Set up the information of threshold value : min and max
            min_threshold_val_ = min_val;
            max_threshold_val_ = max_val;
        }

        public void set_image(Bitmap image)
        {
            Bitmap resized_img = new Bitmap(image, new Size(170, 170));
            pictureBox1.Image = resized_img;
        }

        public void set_light(string light_val)
        {
            this.light_img_ = light_val;
        }

        public string get_info()
        {
            return color_ + "," + channel_ + "," + min_threshold_val_ + "," + max_threshold_val_;
        }

        public string ColorSpace
        {
            get { return color_; }
        }

        public int Channel
        {
            get { return channel_; }
        }

        public string MinThreshold
        {
            get { return min_threshold_val_; }
        }

        public string MaxThreshold
        {
            get { return max_threshold_val_; }
        }

        public string Method
        {
            get { return method_; }
        }

        public string Light
        {
            get { return light_img_; }
        }

        public bool Selected
        {
            get { return this.is_selected_; }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("is selected");
            is_selected_ ^= true;

            if (is_selected_)
                panel1.BackColor = Color.FromArgb(255, 128, 128);
            else
                panel1.BackColor = Color.White;
        }

        public int ID
        {
            get { return this.id_; }
            set { this.id_ = value; }
        }
    }
}
