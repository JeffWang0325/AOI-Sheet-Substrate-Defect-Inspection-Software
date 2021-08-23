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

namespace DeltaSubstrateInspector.src.PositionMethod.LocateMethod.Rotation
{
    public partial class AngleAffineUC : UserControl
    {
        private AngleAffineMethod affine_method = new AngleAffineMethod();
        HObject mark_image_1, mark_image_2;
        HObject load_image_;

        //********************************
        public AngleAffineUC()
        {
            InitializeComponent();
            affine_method.load();
            txt_pixel_resolution.Text = affine_method.pixel_resolution_.ToString();
            txt_motion_shift_dist.Text = affine_method.Motion_shift_dist_y_hat.ToString();
            txt_matching_score.Text = affine_method.match_score_.ToString();
        }

        private void btn_load_image_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog_file.ShowDialog() == DialogResult.OK)
                {
                    string path_ = openFileDialog_file.FileName;
                    HOperatorSet.ReadImage(out load_image_, path_);
                    HOperatorSet.DispColor(load_image_, hWindowControl_preview.HalconWindow);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Loading Image Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void btn_rotate_Click(object sender, EventArgs e)
        {
            HObject rotate_image_;
            if (!affine_method.rotate_image_(hWindowControl_preview.HalconWindow, load_image_, out rotate_image_))
                MessageBox.Show("Rotating Image Fail !", "Rotating Image Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btn_read_model_Click(object sender, EventArgs e)
        {
            if (!affine_method.read_shape_model())
                MessageBox.Show("Please Create Shape Model First !", "Model Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                lab_model_status.ForeColor = Color.LightGreen;
                lab_model_status.Text = "Online";
            }
        }

        private void btn_load_markimg_1_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog_file.ShowDialog() == DialogResult.OK)
                {
                    string path_ = openFileDialog_file.FileName;
                    HOperatorSet.ReadImage(out mark_image_1, path_);
                    HOperatorSet.DispColor(mark_image_1, hWindowControl_mark_1.HalconWindow);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Loading Mark Image_1 Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_create_model_Click(object sender, EventArgs e)
        {
            if (mark_image_1 != null)
            {
                if (affine_method.create_shape_model(hWindowControl_mark_1.HalconWindow, mark_image_1))
                {
                    lab_model_status.ForeColor = Color.LightGreen;
                    lab_model_status.Text = "Online";
                }
                else
                    MessageBox.Show("Create Model Fail !", "Create Model Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_find_mark_Click(object sender, EventArgs e)
        {
            bool b_status_ = false;
            if (mark_image_1 != null && mark_image_2 != null)
                b_status_ = affine_method.find_shpe_model(hWindowControl_mark_1.HalconWindow, mark_image_1, hWindowControl_mark_2.HalconWindow, mark_image_2);

            if (b_status_)
            {
                btn_rotate_images.Enabled = true;
                btn_rotate.Enabled = true;
            }
        }

        private void btn_load_markimg_2_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog_file.ShowDialog() == DialogResult.OK)
                {
                    string path_ = openFileDialog_file.FileName;
                    HOperatorSet.ReadImage(out mark_image_2, path_);
                    HOperatorSet.DispColor(mark_image_2, hWindowControl_mark_2.HalconWindow);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Loading Mark Image_2  Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (mark_image_1 != null && mark_image_2 != null)
                btn_find_mark.Enabled = true;
        }
        private void btn_rotate_mark_images_Click(object sender, EventArgs e)
        {
            HObject rotate_image_1, rotate_image_2;
            affine_method.rotate_image_(hWindowControl_mark_1.HalconWindow, mark_image_1, out rotate_image_1);
            affine_method.rotate_image_(hWindowControl_mark_2.HalconWindow, mark_image_2, out rotate_image_2);
        }
        private void btn_save_Click(object sender, EventArgs e)
        {
            if (!affine_method.save())
                MessageBox.Show("Save XML Fail !", "Save XML Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public AngleAffineMethod AffineMethodObj
        {
            get { return this.affine_method; }
        }



        #region TextBox Changed
        private void txt_pixel_resolution_TextChanged(object sender, EventArgs e)
        {
            affine_method.pixel_resolution_ = Convert.ToDouble(txt_pixel_resolution.Text);
        }

        private void txt_save_path_TextChanged(object sender, EventArgs e)
        {
            //affine_method.file_directory_ = txt_save_path.Text;
        }

        private void txt_motion_shift_dist_TextChanged(object sender, EventArgs e)
        {
            affine_method.Motion_shift_dist_y_hat = Convert.ToDouble(txt_motion_shift_dist.Text);
        }



        private void txt_matching_score_TextChanged(object sender, EventArgs e)
        {
            affine_method.match_score_ = Convert.ToDouble(txt_matching_score.Text);
        }
        #endregion




    }
}
