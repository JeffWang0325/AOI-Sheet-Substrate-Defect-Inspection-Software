using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Risitanse_AOI.Inspection
{
    public partial class InspectionSettingPanel : UserControl
    {
        List<Button> btn_controler_collection_;

        public InspectionSettingPanel()
        {
            InitializeComponent();
            btn_controler_collection_ = new List<Button>();
        }

        public void set_control_button(string btn_count_info, string btn_name)
        {
            int name_counter = 0;
            string[] count_in_layers = btn_count_info.Split(',');
            string[] name_collection = btn_name.Split(',');

            foreach (string count in count_in_layers)
            {
                for (int i = 1; i <= Convert.ToInt32(count); i++)
                {
                    Button btn = new Button();
                    btn.Text = name_collection[name_counter];
                    btn.Name = btn.Text;
                    name_counter += 1;
                }
            }
        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
