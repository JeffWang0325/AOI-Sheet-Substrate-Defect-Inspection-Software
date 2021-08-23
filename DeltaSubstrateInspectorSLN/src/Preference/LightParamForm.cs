using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Threading;
using HalconDotNet;
using DeltaSubstrateInspector.src.Models.CaptureModel;

namespace DeltaSubstrateInspector.src.MainSetting
{
    public partial class LightParamForm : Form
    {
        // Save and return when the window closed
        private string light_params = "";

        public LightParamForm()
        {
            InitializeComponent();            
        }      

        private void LightParamForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DeltaSubstrateInspector.src.Models.CaptureModel.Camera.set_dict_HWindow.Remove(1);
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_new_Click_1(object sender, EventArgs e)
        {
            light_params = txt_name.Text.ToString() + "," + cbo_port.Text.ToString() + "," + cbo_bao.Text.ToString() + "," + trackBar1.Value.ToString();
            this.Close();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            TrackBar tkb = sender as TrackBar;
            lbl_light_val.Text = tkb.Value.ToString() + "%";
        }

        public string get_light_param()
        {
            return light_params;
        }


    }
}
