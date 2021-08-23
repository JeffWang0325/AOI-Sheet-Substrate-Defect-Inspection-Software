using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using DeltaSubstrateInspector.src.Modules.NewInspectModule;
using DeltaSubstrateInspector.src.Roles;

using HalconDotNet;

namespace DeltaSubstrateInspector.src.Modules.NewInspectModule.DefaultInspect
{
    public partial class DefaultInspectUC : UserControl
    {
        #region 標準變數
        public event EventHandler OnUserControlButtonClicked;

        public static DefaultInspect DefaultInspect_method = new DefaultInspect();
        private DefaultInspectRole DefaultInspect_role = new DefaultInspectRole();
        #endregion

        // 內部參數
        OpenFileDialog openFileDialog1 = new OpenFileDialog();
        HObject load_image = null;


        public DefaultInspectUC()
        {
            InitializeComponent();

            this.hWindowControl_InputImage.MouseWheel += hWindowControl_InputImage.HSmartWindowControl_MouseWheel;
    
        }

        private void button_Add_Click(object sender, EventArgs e)
        {
            OnUserControlButtonClicked(this, e);
        }

        public InspectRole get_role()
        {
            return create_role();
        }

        private DefaultInspectRole create_role()
        {
            DefaultInspectRole role = new DefaultInspectRole();
            role.Method = "DefaultInspect";

            
            return role;
        }

        public void UpdateParameter()
        {
            // Load parameter from XML file
            DefaultInspect_role.load();

            // Set parameter to UI
            checkBox_InspectBypass.Checked = DefaultInspect_role.InspectBypass;


        }



        //*****************************************************************************************************************
        private void button_SaveParam_Click(object sender, EventArgs e)
        {
            // 從UI取得數值並且設定Role
            ParamGetFromUIandSet2Role();

            // Save parameter to XML file !!!! 一定要加
            DefaultInspect_role.save();
        }

        private void ParamGetFromUIandSet2Role()
        {
            // Get parameter from UI
            DefaultInspect_role.InspectBypass = checkBox_InspectBypass.Checked;



            // Set parameter to method
            DefaultInspect_method.set_parameter(DefaultInspect_role);

        }

        private void button_Inspection_Click(object sender, EventArgs e)
        {
            // 從UI取得數值並且設定Role
            ParamGetFromUIandSet2Role();


            // GO
            List<HObject> Input_ImgList = new List<HObject>();

            // 一次Load一張
            Input_ImgList.Add(load_image);

            try
            {
                HObject Result_image;
                DefaultInspect_method.action(Input_ImgList, out Result_image);


                // Show result
                if (Result_image != null)
                {
                    // Clear 
                    HOperatorSet.ClearWindow(hWindowControl_InputImage.HalconWindow);

                    // 原圖
                    HOperatorSet.DispObj(Result_image, hWindowControl_InputImage.HalconWindow);

                }


            }
            catch (Exception ex)
            {
                string message = "Error: " + ex.Message;
                MessageBox.Show(message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button_LoadSingleImage_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog1.FileName;
                HOperatorSet.ReadImage(out load_image, path);
                //hWindowControl_InputImage.SetFullImagePart(new HImage(load_image));

                // 顯示影像
                HOperatorSet.DispObj(load_image, hWindowControl_InputImage.HalconWindow);

            }
        }
    }
}
