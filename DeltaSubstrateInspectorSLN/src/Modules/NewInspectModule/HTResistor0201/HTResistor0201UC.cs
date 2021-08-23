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

using static DeltaSubstrateInspector.FileSystem.FileSystem;
using System.IO;

using System.Xml;
using HalconDotNet;

namespace DeltaSubstrateInspector.src.Modules.NewInspectModule.HTResistor0201
{
    public partial class HTResistor0201UC : UserControl
    {
        #region 標準變數
        public event EventHandler OnUserControlButtonClicked;

        private HTResistor0201Role HTResistor0201_role = new HTResistor0201Role();
        public static HTResistor0201 HTResistor0201_method = new HTResistor0201();
        #endregion

        HObject ho_image = null;

        public HTResistor0201UC()
        {
            InitializeComponent();
            this.MouseWheel += this.hSmartWindowControl1.HSmartWindowControl_MouseWheel;
        }

        // ===========================================================================================================
        private void HTResistor0201UC_Load(object sender, EventArgs e)
        {
            HOperatorSet.GenEmptyObj(out ho_image);
        }

        // ===========================================================================================================
        private void button_Add_Click(object sender, EventArgs e)
        {
            OnUserControlButtonClicked(this, e);
        }

        // ===========================================================================================================
        public InspectRole get_role()
        {
            return create_role();
        }

        // ===========================================================================================================
        private HTResistor0201Role create_role()
        {
            HTResistor0201Role role = new HTResistor0201Role();
            role.Method = "HTResistor0201";
            return role;
        }

        // ===========================================================================================================
        public void UpdateParameter()
        {
            // Load parameter from XML file
            HTResistor0201_role.load();

            // Set parameter to UI
            this.txtboxAreaLower_BrightBlobBL.Text = HTResistor0201_role.strAreaLower_BrightBlobBL;
            this.txtboxAreaUpper_BrightBlobBL.Text = HTResistor0201_role.strAreaUpper_BrightBlobBL;
            this.txtboxThresholdOffset_BrightBlobBL.Text = HTResistor0201_role.strThresholdOffset_BrightBlobBL;

            this.txtboxCrackHeight_BlackCrackFL.Text = HTResistor0201_role.strCrackHeight_BlackCrackFL;
            this.txtboxAreaLower_BlackCrackFL.Text = HTResistor0201_role.strAreaLower_BlackCrackFL;
            this.txtboxAreaUpper_BlackCrackFL.Text = HTResistor0201_role.strAreaUpper_BlackCrackFL;
            this.txtboxThresholdScale_BlackCrackFL.Text = HTResistor0201_role.strThresholdScale_BlackCrackFL;
            this.chkboxVertClose_BlackCrackFL.Checked = HTResistor0201_role.bolVertClose_BlackCrackFL;

            this.txtboxAreaLower_WhiteBlobFL.Text = HTResistor0201_role.strAreaLower_WhiteBlobFL;
            this.txtboxAreaUpper_WhiteBlobFL.Text = HTResistor0201_role.strAreaUpper_WhiteBlobFL;
            this.txtboxBlobHeight_WhiteBlobFL.Text = HTResistor0201_role.strBlobHeight_WhiteBlobFL;
            this.txtboxThresholdScale_WhiteBlobFL.Text = HTResistor0201_role.strThresholdScale_WhiteBlobFL;
            this.chkboxVertClose_WhiteBlobFL.Checked = HTResistor0201_role.bolVertClose_WhiteBlobFL;

            this.txtboxAreaLower_WhiteCrackFL.Text = HTResistor0201_role.strAreaLower_WhiteCrackFL;
            this.txtboxAreaUpper_WhiteCrackFL.Text = HTResistor0201_role.strAreaUpper_WhiteCrackFL;
            this.txtboxCrackWidth_WhiteCrackFL.Text = HTResistor0201_role.strCrackWidth_WhiteCrackFL;
            this.txtboxThresholdScale_WhiteCrackFL.Text = HTResistor0201_role.strThresholdScale_WhiteCrackFL;
            this.chkboxVertClose_WhiteCrackFL.Checked = HTResistor0201_role.bolVertClose_WhiteCrackFL;

            this.txtboxMaskSize_BlackBlobCL.Text = HTResistor0201_role.strMaskSize_BlackBlobCL;
            this.txtboxAreaLower_BlackBlobCL.Text = HTResistor0201_role.strAreaLower_BlackBlobCL;
            this.txtboxAreaUpper_BlackBlobCL.Text = HTResistor0201_role.strAreaUpper_BlackBlobCL;
            this.txtboxThresholdScale_BlackBlobCL.Text = HTResistor0201_role.strThresholdScale_BlackBlobCL;
        }

        // ==================================================================================
        private void btnLoadImage_Click(object sender, EventArgs e)
        {
            HTuple hv_Width = null, hv_Height = null;

            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //ho_image.Dispose();
                HOperatorSet.ReadImage(out ho_image, this.openFileDialog1.FileName);
                this.labFileName.Text = "File Name: " + this.openFileDialog1.SafeFileName;

                HOperatorSet.GetImageSize(ho_image, out hv_Width, out hv_Height);

                HWindow Window = hSmartWindowControl1.HalconWindow;
                Window.SetPart(0, 0, (int)hv_Height - 1, (int)hv_Width - 1);

                HOperatorSet.DispObj(ho_image, hSmartWindowControl1.HalconWindow);
            }
        }

        // ===========================================================================================================
        private void btnLoadParameters_Click(object sender, EventArgs e)
        {
            UpdateParameter();

            //XmlDocument doc = new XmlDocument();

            //doc.Load("test.xml");
            //XmlNode node = doc.SelectSingleNode("//Inspect[@Name='HTResistor0201']");
            //this.txtboxAreaLower_BrightBlobBL.Text = node.SelectSingleNode("AreaLower_BrightBlobBL").InnerText;
            //this.txtboxAreaUpper_BrightBlobBL.Text = node.SelectSingleNode("AreaUpper_BrightBlobBL").InnerText;
        }

        // ===========================================================================================================
        private void btnSaveParameters_Click(object sender, EventArgs e)
        {
            // Get parameter from UI
            HTResistor0201_role.strAreaLower_BrightBlobBL = this.txtboxAreaLower_BrightBlobBL.Text;
            HTResistor0201_role.strAreaUpper_BrightBlobBL = this.txtboxAreaUpper_BrightBlobBL.Text;
            HTResistor0201_role.strThresholdOffset_BrightBlobBL = this.txtboxThresholdOffset_BrightBlobBL.Text;

            HTResistor0201_role.bolVertClose_BlackCrackFL = this.chkboxVertClose_BlackCrackFL.Checked;
            HTResistor0201_role.strCrackHeight_BlackCrackFL = this.txtboxCrackHeight_BlackCrackFL.Text;
            HTResistor0201_role.strAreaLower_BlackCrackFL = this.txtboxAreaLower_BlackCrackFL.Text;
            HTResistor0201_role.strAreaUpper_BlackCrackFL = this.txtboxAreaUpper_BlackCrackFL.Text;
            HTResistor0201_role.strThresholdScale_BlackCrackFL = this.txtboxThresholdScale_BlackCrackFL.Text;

            HTResistor0201_role.bolVertClose_WhiteBlobFL = this.chkboxVertClose_WhiteBlobFL.Checked;
            HTResistor0201_role.strAreaLower_WhiteBlobFL = this.txtboxAreaLower_WhiteBlobFL.Text;
            HTResistor0201_role.strAreaUpper_WhiteBlobFL = this.txtboxAreaUpper_WhiteBlobFL.Text;
            HTResistor0201_role.strBlobHeight_WhiteBlobFL = this.txtboxBlobHeight_WhiteBlobFL.Text;
            HTResistor0201_role.strThresholdScale_WhiteBlobFL = this.txtboxThresholdScale_WhiteBlobFL.Text;

            HTResistor0201_role.bolVertClose_WhiteCrackFL = this.chkboxVertClose_WhiteCrackFL.Checked;
            HTResistor0201_role.strAreaLower_WhiteCrackFL = this.txtboxAreaLower_WhiteCrackFL.Text;
            HTResistor0201_role.strAreaUpper_WhiteCrackFL = this.txtboxAreaUpper_WhiteCrackFL.Text;
            HTResistor0201_role.strCrackWidth_WhiteCrackFL = this.txtboxCrackWidth_WhiteCrackFL.Text;
            HTResistor0201_role.strThresholdScale_WhiteCrackFL = this.txtboxThresholdScale_WhiteCrackFL.Text;

            HTResistor0201_role.strMaskSize_BlackBlobCL = this.txtboxMaskSize_BlackBlobCL.Text;
            HTResistor0201_role.strAreaLower_BlackBlobCL = this.txtboxAreaLower_BlackBlobCL.Text;
            HTResistor0201_role.strAreaUpper_BlackBlobCL = this.txtboxAreaUpper_BlackBlobCL.Text;
            HTResistor0201_role.strThresholdScale_BlackBlobCL = this.txtboxThresholdScale_BlackBlobCL.Text;

            // Set parameter to method
            //HTResistor0201_method.set_parameter(HTResistor0201_role);

            // Save parameter to XML file !!!! 一定要加
            HTResistor0201_role.save();

            //XmlDocument doc = new XmlDocument();
            //XmlElement element = doc.CreateElement("Parameter");
            //doc.AppendChild(element);

            //XmlElement eleInspect = doc.CreateElement("Inspect");
            //eleInspect.SetAttribute("Name", "HTResistor0201");
            //element.AppendChild(eleInspect);

            //element = doc.CreateElement("AreaLower_BrightBlobBL");
            //element.InnerText = this.txtboxAreaLower_BrightBlobBL.Text;
            //eleInspect.AppendChild(element);

            //element = doc.CreateElement("AreaUpper_BrightBlobBL");
            //element.InnerText = this.txtboxAreaUpper_BrightBlobBL.Text;
            //eleInspect.AppendChild(element);

            //doc.Save("test.xml");
        }
    }
}
