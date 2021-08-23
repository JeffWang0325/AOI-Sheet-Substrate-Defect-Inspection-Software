using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using Extension;
using DeltaSubstrateInspector.src.InspectionForms.UC;
using DeltaSubstrateInspector.src.Roles;
using DeltaSubstrateInspector.src.Modules.InspectModule;
using System.Collections;

using System.Configuration;
using System.Collections.Specialized;


using static DeltaSubstrateInspector.src.MainPanels.InspectionView;

namespace DeltaSubstrateInspector.src.InspectionForms.ParamPanels
{
    public partial class ParamSetting : UserControl
    {
        private List<InspectRole> roles_set_ = new List<InspectRole>();
        private List<IRoleSetPreview> inspect_roles_ui_ = new List<IRoleSetPreview>();
        private bool is_saved = false;
        private Type param_type_;
        private dynamic param_obj_ = new object();
        private string inspection_method = "";

      
        public ParamSetting()
        {
            InitializeComponent();            
        }

        /// <summary>
        /// 使用者選擇檢測方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void method_btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            inspection_method = btn.Text.ToString();

            // 透過需求取得config定義中的物件名
            string target = "MethodParameterSetting/" + btn.Tag.ToString();
            var section = ConfigurationManager.GetSection(target) as NameValueCollection;
            string uc_name = section["UC"];
            string formTypeFullName = string.Format("{0}.{1}", "DeltaSubstrateInspector.src.Roles.UC", uc_name);

            // 181031, andy
            string formTypeFullNameNew = string.Format("{0}.{1}", "DeltaSubstrateInspector.src.Modules.NewInspectModule."+btn.Tag.ToString(), uc_name);


            // 宣告物件type
            try
            {
                param_type_ = Type.GetType(formTypeFullName, true);
            }
            catch (Exception)
            {
                param_type_ = Type.GetType(formTypeFullNameNew, true);

            }
            
            param_obj_ = Activator.CreateInstance(param_type_);

            method_UC_pnl.Controls.Clear();
            // 建立物件
            method_UC_pnl.Controls.Add((UserControl)param_obj_);

            param_obj_.OnUserControlButtonClicked += new EventHandler(create_role);
                      
        }

        private void create_role(object sender, EventArgs e)
        {
            InspectRole role = param_obj_.get_role();
            role.InspectName = txt_name.Text;
            roles_set_.Add(role);
            create_preview_item(role.Method, role.InspectName);
        }
       
        private void create_preview_item(string info, string title)
        {
            IRoleSetPreview oper_preview = new IRoleSetPreview();
            oper_preview.set_information(info);
            oper_preview.set_title(title);
            oper_preview.ID = inspect_roles_ui_.Count;
            oper_preview.Location = new System.Drawing.Point(18, 10 + (10 + oper_preview.Height) * oper_preview.ID);

            oper_preview.OnUserControlButtonClicked += Oper_preview_OnUserControlButtonClicked;


            inspect_roles_ui_.Add(oper_preview);
            update_inspect_role_pnl();
            //pnl_oper_roles.Controls.Add(oper_preview);

        }

        private void Oper_preview_OnUserControlButtonClicked(object sender, EventArgs e)
        {
            string NowTarget = "";
            for (int i = 0; i < inspect_roles_ui_.Count; i++)
            {
                if (inspect_roles_ui_[i].Selected==true)
                {
                    NowTarget = inspect_roles_ui_[i].Info;
                    break;
                    
                }
               
            }

            if (NowTarget == "") return;
                      
            // 透過需求取得config定義中的物件名
            string target = "MethodParameterSetting/" + NowTarget;
            var section = ConfigurationManager.GetSection(target) as NameValueCollection;
            string uc_name = section["UC"];
            string formTypeFullName = string.Format("{0}.{1}", "DeltaSubstrateInspector.src.Roles.UC", uc_name);

            // 181031, andy
            string formTypeFullNameNew = string.Format("{0}.{1}", "DeltaSubstrateInspector.src.Modules.NewInspectModule." + NowTarget, uc_name);


            // 宣告物件type
            try
            {
                param_type_ = Type.GetType(formTypeFullName, true);
            }
            catch (Exception)
            {
                param_type_ = Type.GetType(formTypeFullNameNew, true);

            }

            param_obj_ = Activator.CreateInstance(param_type_);

            method_UC_pnl.Controls.Clear();
            // 建立物件
            method_UC_pnl.Controls.Add((UserControl)param_obj_);

            param_obj_.OnUserControlButtonClicked += new EventHandler(create_role);



            // 181105, andy 更新最新參數並顯示 !!!!!!
            try
            {
                param_obj_.UpdateParameter();
            }
            catch (Exception ex) // (20190410) Jeff Revised!
            {
                MessageBox.Show("演算法UI尚未加入 UpdateParameter，按確定後將關閉視窗!");
                throw;
            }


            // 190221, andy
            try
            {
                param_obj_.UpdateNowInput_ImgList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("演算法UI尚未加入 UpdateNowInput_ImgList，按確定後繼續開啟視窗!");
                
                //throw; --> 加入throw 則會關閉此視窗
            }

        }

        private void update_inspect_role_pnl()
        {
            pnl_oper_roles.Controls.Clear();
            foreach (var item in inspect_roles_ui_)
            {
                item.Location = new System.Drawing.Point(5, 10 + (10 + item.Height) * item.ID);
                pnl_oper_roles.Controls.Add(item);
            }
        }

        public void load(List<InspectRole> roles_set)
        {
            roles_set_.Clear();
            foreach (InspectRole role in roles_set)
            {
                roles_set_.Add(role);
                create_preview_item(role.Method, role.InspectName);
            }

            // 181214, andy
            if (inspect_roles_ui_.Count > 0)
            {
                inspect_roles_ui_[0].AutoClick();
            }

        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            
             is_saved = true;
             
            
        }

        public List<InspectRole> RoleList
        {
            get { return roles_set_; }
        }

        public bool Is_saved
        {
            get { return this.is_saved; }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<IRoleSetPreview> preview_list = new List<IRoleSetPreview>();
            List<InspectRole> inspect_role_list = new List<InspectRole>();

            for (int i = 0; i < inspect_roles_ui_.Count; i++)
            {
                if (!inspect_roles_ui_[i].Selected)
                {
                    inspect_roles_ui_[i].ID = preview_list.Count;
                    preview_list.Add(inspect_roles_ui_[i]);
                }
                else
                    roles_set_[inspect_roles_ui_[i].ID] = null;
            }

            foreach (var item in roles_set_)
            {
                if (item != null)
                {
                    inspect_role_list.Add(item);
                }
            }

            roles_set_ = null;
            roles_set_ = inspect_role_list;

            inspect_roles_ui_ = null;
            inspect_roles_ui_ = preview_list;
            update_inspect_role_pnl();
        }

        private void thresholdCtrl1_Load(object sender, EventArgs e)
        {

        }

        private void ParamSetting_Load(object sender, EventArgs e)
        {
            int i = 0;
        }

      
        
    }
}
