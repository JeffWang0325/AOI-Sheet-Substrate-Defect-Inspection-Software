using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using HalconDotNet;
using Extension;
using DeltaSubstrateInspector.src.Modules.InspectModule.ImageProcess;
using DeltaSubstrateInspector.src.Modules.MotionModule;

namespace DeltaSubstrateInspector.src.SettingAndRole
{
    public partial class PositionSetting : UserControl
    {
        private Type param_type_;
        private object param_obj_ = new object();
        private string file_name = "";

        public PositionSetting()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            string function_name = cmb.Text;

            string section = ConfigurationManager.AppSettings[function_name];
            string formTypeFullName = string.Format("{0}.{1}", "DeltaSubstrateInspector.src.SettingAndRole.PositionAffineUC", section);
            file_name = formTypeFullName + ".xml";
            param_type_ = Type.GetType(formTypeFullName, true);
            param_obj_ = Activator.CreateInstance(param_type_);
            UserControl test = (UserControl)param_obj_;
            test.Location = new System.Drawing.Point(12, 21);

            load_image();
        }

        private void load_image()
        {
            //HObject src_img, dst_img;
            //HOperatorSet.ReadImage(out src_img, "D:/@Work/基板AOI/Jeff/0402右斜/abc-1_M002_F001.tif");
            ////HOperatorSet.ReadImage(out src_img, "C:/Users/User/Desktop/AffineTrans Sample/abc-1_M002_F001");
            //pictureBox1.Image = src_img.GetRGBBitmap();

            //AfineTransform1 trans = new AfineTransform1();
            //double angle = 0;
            //trans.create_afffine_trans(src_img, out angle, out dst_img);


            //pictureBox2.Image = dst_img.GetRGBBitmap();
        }

        public string AfineMethod
        {
            get { return file_name; }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        
    }
}
