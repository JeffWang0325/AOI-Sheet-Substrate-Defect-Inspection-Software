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
using DeltaSubstrateInspector.src.Models;

namespace DeltaSubstrateInspector.src.MainSetting
{
    public partial class InspectModelSetting : UserControl
    {
        private List<Button> btn_collection_ = new List<Button>();
        private Dictionary<Button, string> inspect_models = new Dictionary<Button, string>();
        private List<string[]> model_info_collection_ = new List<string[]>();

        public InspectModelSetting()
        {
            InitializeComponent();
        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            string info = "";
            string[] lights_ = { "CL", "OL", "BL", "BL3" };
            using (InspectModelCreateForm create_form = new InspectModelCreateForm(lights_))
            {
                create_form.ShowDialog();
                if (create_form.is_save()) { 
                    // Set up parameters on panel
                    create_light_param_btn(create_form.get_model_info());
                    add_btns_in_panel();
                }
            }
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
            btn.Size = new System.Drawing.Size(1000, 35);
            btn.TabIndex = 0;
            btn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btn.UseVisualStyleBackColor = true;
            btn.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btn_Click);
            // Button's text setting
            btn.Text = param;
            string[] param_data = param.Split(';');
            btn.Text = String.Format(" 模組名稱 : {0}   |  定位方式 : {1}   |  檢測類型 {2}   |  光源影像 :  {3}", param_data[0], param_data[1], param_data[2], param_data[3]);

            // Button;s tag setting
            LightButtonTag tag = new LightButtonTag();
            tag.Param = param_data;
            btn.Tag = new LightButtonTag();
            btn.Tag = tag;

            // Button collection setting
            btn_collection_.Add(btn);
            inspect_models.Add(btn, param);
            model_info_collection_.Add(param_data);
        }

        private void add_btns_in_panel()
        {
            pnl_light_param.Controls.Clear();

            for (int i = 0; i < btn_collection_.Count; i++)
            {
                // Set buttons' location
                btn_collection_[i].Location = new System.Drawing.Point(16, 18 + (btn_collection_[i].Height + 5) * i);

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

        private void btn_Click(object sender, MouseEventArgs e)
        {
            try
            {
                Button btn2 = sender as Button;
                switch (e.Button)
                {

                    case MouseButtons.Left:
                        string[] model_info = (inspect_models[btn2]).Split(';');
                        
                        // Get form by user setting
                        string section = ConfigurationManager.AppSettings[model_info[1]];
                        string formTypeFullName = string.Format("{0}.{1}", "DeltaSubstrateInspector.src.PositionMethod", section);
                        Type type = Type.GetType(formTypeFullName, true);
                        Form item = (Form)Activator.CreateInstance(type);

                        using (item)
                        {
                            item.ShowDialog();
                        }
                        break;

                        //case MouseButtons.Right:
                        //    LightButtonTag tag = btn2.Tag as LightButtonTag;
                        //    tag.Flag ^= true;
                        //    btn2.Tag = tag;
                        //    break;

                }
                //add_btns_in_panel();
            }
            catch { }
        }

        // Need fix to  not use inspect model
        public List<string> get_models()
        {
            List<string> operator_list = new List<string>();
            for (int i = 0; i < btn_collection_.Count; i++)
            {
                LightButtonTag tag = btn_collection_[i].Tag as LightButtonTag;
                operator_list.Add(tag.Param[0]);
            }

            return operator_list;
        }

        public List<string[]> ModelInfo
        {
            get { return this.model_info_collection_; }
        } 

        private class LightButtonTag
        {
            private bool flag = false;
            private string[] light_param = null;

            public bool Flag
            {
                get { return this.flag; }
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
