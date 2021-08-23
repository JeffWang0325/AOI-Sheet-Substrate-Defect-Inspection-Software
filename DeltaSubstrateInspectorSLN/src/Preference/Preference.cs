using DeltaSubstrateInspector.src.Models;
using DeltaSubstrateInspector.src.Modules.InspectModule;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeltaSubstrateInspector.src.MainSetting
{
    public partial class Preference : Form
    {
        private LightSetting light_setting_ = new LightSetting();
        private SystemSetting inspect_setting_ = new SystemSetting();
        private MoveSetting movement_setting_ = new MoveSetting();        
        private InspectModelSetting inspect_model_setting_ = new InspectModelSetting();
        private bool is_saved = false;

        public Preference()
        {
            InitializeComponent();
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            is_saved = true;
            this.Close();
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {

        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_light_setting_Click(object sender, EventArgs e)
        {
            //light_setting_.set_class_camera = class_camera_;
            pnl_setting.Controls.Clear();
            pnl_setting.Controls.Add(light_setting_);
        }

        private void btn_inspect_set_Click(object sender, EventArgs e)
        {
            pnl_setting.Controls.Clear();
            pnl_setting.Controls.Add(inspect_setting_);
        }

        private void btn_move_Click(object sender, EventArgs e)
        {
            //movement_setting_.set_class_camera = class_camera_;
            pnl_setting.Controls.Clear();
            pnl_setting.Controls.Add(movement_setting_);
        }

        private void btn_inspect_model_Click(object sender, EventArgs e)
        {
            pnl_setting.Controls.Clear();
            pnl_setting.Controls.Add(inspect_model_setting_);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        public bool model_saved()
        {
            return is_saved;
        }

        public List<InspectOperator> get_inspect_models()
        {
            List<InspectOperator> op_collection = new List<InspectOperator>();

            for (int i = 0; i < inspect_model_setting_.get_models().Count; i++)
            {
                  InspectOperator insp_op = new InspectOperator(inspect_model_setting_.get_models()[i], 0); //0 should be related to pnlSide
                  insp_op.OperatorInfo = inspect_model_setting_.ModelInfo[i];
                  op_collection.Add(insp_op);
            }
            
           return op_collection;
        }

        private void Preference_FormClosing(object sender, FormClosingEventArgs e)
        {
            DeltaSubstrateInspector.src.Models.CaptureModel.Camera.set_dict_HWindow.Clear();
        }
    }
}
