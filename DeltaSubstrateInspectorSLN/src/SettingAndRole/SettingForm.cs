using DeltaSubstrateInspector.src.Roles;
using DeltaSubstrateInspector.src.SettingAndRole;
using StyleTabTable;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeltaSubstrateInspector.src.InspectionForms
{
    public partial class SettingForm : Form
    {
        private StyledTabtable table_ = new StyledTabtable();
        //  private Panel position_pnl_ = new Panel();
        private Panel param_pnl_ = new Panel();
        private Type param_type_;
        private dynamic param_obj_ = new object();
        private string afine_name_ = "";

        
        public SettingForm()
        {
            InitializeComponent();
            set_tab_table();
          //  set_pos_pnl();
            set_param_pnl();
        }

        private void set_tab_table()
        {
            int[] hover_color_ = { 37, 91, 165 };
            int[] clicked_color_ = { 47, 101, 195 };
            int[] back_color_ = { 27, 85, 159 };

            //table_.set_layout(50, 100, 1715, 914, "horizon", true);
            table_.set_layout(50, 100, 1830, 950, "horizon", true);
            table_.set_colors(hover_color_, clicked_color_, back_color_);
           // table_.new_tab_item("定位方法");
            table_.new_tab_item("檢測參數");
            table_.Location = new System.Drawing.Point(2, 46);
            this.panel2.Controls.Add(table_);
        }

        //private void set_pos_pnl()
        //{
        //    PositionSetting pos_setting = new PositionSetting();
        //    position_pnl_ = table_.get_panel_by_name("定位方法");
        //    position_pnl_.Controls.Add(pos_setting);
        //    afine_name_ = pos_setting.AfineMethod;
        //}

        private void set_param_pnl()
        {
            param_pnl_ = table_.get_panel_by_name("檢測參數");

            string section = ConfigurationManager.AppSettings["Region Inspection"];
            string formTypeFullName = string.Format("{0}.{1}", "DeltaSubstrateInspector.src.InspectionForms.ParamPanels", section);
            param_type_ = Type.GetType(formTypeFullName, true);
            param_obj_ = Activator.CreateInstance(param_type_);
            param_pnl_.Controls.Add((UserControl)param_obj_);

            
        }
        

        public List<InspectRole> get_inspect_roles()
        {
            PropertyInfo numberPropertyInfo = param_type_.GetProperty("RoleList");
            List<InspectRole> value = (List<InspectRole>)numberPropertyInfo.GetValue(param_obj_, null);
            return value;
        }

        public string get_afine_method_name()
        {
            return afine_name_;
        }

        public void load(List<InspectRole> roles_list)
        {
            param_obj_.load(roles_list);
        }

        private void SettingForm_VisibleChanged(object sender, EventArgs e)
        {


            if (Visible == true)
                this.TopMost = false;
            else
                this.TopMost = true;


        }
    }
}
