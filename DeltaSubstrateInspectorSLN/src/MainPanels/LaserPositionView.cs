using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DeltaSubstrateInspector.src.PositionMethod.LocateMethod.Rotation;
using DeltaSubstrateInspector.src.PositionMethod.LocateMethod.Location;
using static DeltaSubstrateInspector.FileSystem.FileSystem;
using System.IO;
using System.Xml;

namespace DeltaSubstrateInspector.src.MainPanels
{
    public partial class LaserPositionView : UserControl // (20181031) Jeff Revised!
    {
        public EventHandler ok_clicked;
        private string laser_pos_info_ = "";

        public LaserPositionView()
        {
            InitializeComponent();
            load_param_set_lst();
        }

        private void load_param_set_lst()
        {
            try
            {
                XmlDocument xml_doc_ = new XmlDocument();
                //xml_doc_.Load(ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\Marks center.xml");
                if (!System.IO.File.Exists(ModuleParamDirectory + LaserPositionParam + "\\Marks center.xml"))
                    return;
                else
                    xml_doc_.Load(ModuleParamDirectory + LaserPositionParam + "\\Marks center.xml");

                XmlNode xml_node_ = xml_doc_.SelectSingleNode("Board_Type");
                XmlNodeList xml_child = xml_node_.ChildNodes;
                foreach (XmlNode item in xml_child)
                {
                    int index = item.Name.IndexOf('_'); // index = 4
                    string name = new String(item.Name.Skip(index + 1).Take(item.Name.Length - 1).ToArray());
                    cmb_param_lst.Items.Add(name);
                }
            }
            catch { }
        }

        public LaserLocateMethod get_laser_locate_method()
        {
            return laser_locationUC1.LaserLocateMethodObj;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cmb_param_lst.Text=="")
                MessageBox.Show("尚未選擇參數檔!");
            else
            {
                lb_is_set.Text = "完成設定";
                lb_is_set.ForeColor = Color.ForestGreen;
                laser_pos_info_ = cmb_param_lst.Text == "" ? laser_locationUC1.Info : cmb_param_lst.Text;
                ok_clicked(this, e);
            }
        }

        public string LaserPosInfo
        {
            get { return this.laser_pos_info_; }
            set { this.laser_pos_info_ = value; }
        }

        private void cmb_param_lst_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            UpdatePositionPara(cmb.Text, false);

            /*
            laser_pos_info_ = cmb.Text;

            // 即時更新class內及GUI顯示之參數
            laser_locationUC1.LaserLocateMethodObj.board_type_ = laser_pos_info_;
            laser_locationUC1.LaserLocateMethodObj.load();
            laser_locationUC1.ui_parameters(false);
            */

        }

        public void UpdatePositionPara(string PositionRecipeName, bool Ext_Call = true)
        {
            if (Ext_Call == true) cmb_param_lst.Text = PositionRecipeName;

            laser_pos_info_ = PositionRecipeName;

            // 即時更新class內及GUI顯示之參數
            laser_locationUC1.LaserLocateMethodObj.BoardType = laser_pos_info_;
            laser_locationUC1.LaserLocateMethodObj.load();
            laser_locationUC1.ui_parameters(false);


        }

    }
}
