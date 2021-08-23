using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeltaSubstrateInspector.src.MainSetting
{
    public partial class LightSetting : UserControl
    {
        private List<Button> btn_collection_ = new List<Button>();

        public LightSetting()
        {
            InitializeComponent();
        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            string param = "";
            using (LightParamForm setting = new LightParamForm())
            {
                DeltaSubstrateInspector.src.Models.CaptureModel.Camera.set_dict_HWindow.Add(1, setting.hWindowControl_cam);

                setting.ShowDialog();
                param = setting.get_light_param();
            }
            try
            {
                // Set up parameters on panel
                create_light_param_btn(param);
                add_btns_in_panel();
            }
            catch { }
        }

        private void create_light_param_btn(string param)
        {
            // Button's apperence setting
            Button btn = new Button();
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightBlue;
            btn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightBlue;
            btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btn.Font = new System.Drawing.Font("微軟正黑體", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            btn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            btn.Size = new System.Drawing.Size(600, 35);
            btn.TabIndex = 0;
            btn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btn.UseVisualStyleBackColor = true;
            btn.Click += new System.EventHandler(this.btn_Click);

            // Button's text setting
            btn.Text = param;
            string[] param_data = param.Split(',');
            btn.Text = String.Format(" 光源名稱 : {0}   |  通訊埠 : {1}   |  檢測鮑率 {2}   |  光源強度 :  {3}", param_data[0], param_data[1], param_data[2], param_data[3]);

            // Button;s tag setting
            LightButtonTag tag = new LightButtonTag();
            tag.Param = param_data;
            btn.Tag = new LightButtonTag();
            btn.Tag = tag;
            
            // Button collection setting
            btn_collection_.Add(btn);
        }

        private void add_btns_in_panel()
        {
            pnl_light_param.Controls.Clear();

            for (int i = 0; i < btn_collection_.Count; i ++)
            {
                // Set buttons' location
                btn_collection_[i].Location = new System.Drawing.Point(16, 18 + (btn_collection_[i].Height + 5) * i );

                // Set the buttons' backgroud color by its' tag
                LightButtonTag tag = btn_collection_[i].Tag as LightButtonTag;
                if (!tag.Flag)
                    btn_collection_[i].BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
                else
                {
                    btn_collection_[i].BackColor = System.Drawing.Color.LightBlue;
                }
                // Add buttons in panel
                pnl_light_param.Controls.Add(btn_collection_[i]);
            }

        }

        private void btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            LightButtonTag tag = btn.Tag as LightButtonTag;
            tag.Flag ^= true;
            btn.Tag = tag;

            // Update the buttons' apperence
            add_btns_in_panel();
        }

        private string[] get_param_data(string param)
        {
            return param.Split(',');
        }

        private class LightButtonTag
        {
            private bool flag = false;
            private string[] light_param = null;

            public bool Flag
            {
                get { return this.flag;  }
                set { this.flag = value; }
            }

            public string[] Param
            {
                get { return this.light_param; }
                set { this.light_param = value; }
            }
        }
    }
}
