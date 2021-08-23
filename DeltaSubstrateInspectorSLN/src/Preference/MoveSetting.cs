using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using DeltaSubstrateInspector.src.Preference.UC;

namespace DeltaSubstrateInspector.src.MainSetting
{
    public partial class MoveSetting : UserControl
    {
        MotionControl uc_motioncontrol_ = new MotionControl();
        
        // ***************************************
        public MoveSetting()
        {
            InitializeComponent();
            pnl_light_param.Controls.Add(uc_motioncontrol_);          
        }

        private void MoveSetting_Load(object sender, EventArgs e)
        {
            DeltaSubstrateInspector.src.Models.CaptureModel.Camera.set_dict_HWindow.Add(2, uc_motioncontrol_.hWindowControl_Cam);
        }
    }
}
