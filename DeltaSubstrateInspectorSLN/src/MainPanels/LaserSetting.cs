using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using StyleTabTable;

namespace DeltaSubstrateInspector.src.MainPanels
{
    public partial class LaserSetting : UserControl
    {
        private StyledTabtable table_ = new StyledTabtable();

        public LaserPositionCreate laserPos_view1_ = new LaserPositionCreate(); // andy
        public LaserPositionCreate laserPos_view2_ = new LaserPositionCreate(); // andy
        public LaserPositionView laser_position_view_ = new LaserPositionView(); // (20181031) Jeff Revised!

        private Panel param_pnl_ = new Panel();

        public LaserSetting()
        {
            InitializeComponent();

            set_tab_table();
            set_param_pnl();

            clsLanguage.clsLanguage.SetLanguateToControls(this); // (20200214) Jeff Revised!
        }

        private void set_tab_table()
        {
            int[] hover_color_ = { 37, 91, 165 };
            int[] clicked_color_ = { 47, 101, 195 };
            int[] back_color_ = { 27, 85, 159 };
            //int[] back_color_ = { 129, 129, 193 };

            //table_.set_layout(50, 100, 1715, 914, "horizon", true);
            table_.set_layout(50, 100, 1800, 1010, "vertical", true);
            table_.set_colors(hover_color_, clicked_color_, back_color_);
            // table_.new_tab_item("定位方法");
            table_.new_tab_item("打標測試1");
            table_.new_tab_item("打標測試2");
            table_.new_tab_item("大小校正");
            table_.Location = new System.Drawing.Point(2, 2);
            //this.panel2.Controls.Add(table_);
            this.Controls.Add(table_);
        }

        private void set_param_pnl()
        {                       
            Panel container1 = table_.get_panel_by_name("打標測試1");
            container1.Controls.Add(laserPos_view1_);

            Panel container2 = table_.get_panel_by_name("打標測試2");
            container2.Controls.Add(laserPos_view2_);

            Panel container3 = table_.get_panel_by_name("大小校正");
            container3.Controls.Add(laser_position_view_);
           

        }

       

    }
}
