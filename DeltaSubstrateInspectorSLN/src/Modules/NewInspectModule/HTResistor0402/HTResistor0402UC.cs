using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using DeltaSubstrateInspector.src.Roles;
using DeltaSubstrateInspector.src.Modules.NewInspectModule;

using HalconDotNet;

namespace DeltaSubstrateInspector.src.Modules.NewInspectModule.HTResistor0402
{
    public partial class HTResistor0402UC : UserControl
    {
        #region 標準變數
        public event EventHandler OnUserControlButtonClicked;        

        private HTResistor0402Role HTResistor0402_role = new HTResistor0402Role();
        public static HTResistor0402 HTResistor0402_method_ = new HTResistor0402();
        public static HTResistor0201.HTResistor0201  HTResistor0201_method = new HTResistor0201.HTResistor0201();
        #endregion

        HObject ho_image = null;
        
        public HTResistor0402UC()
        {
            InitializeComponent();
            this.MouseWheel += this.hSmartWindowControl1.HSmartWindowControl_MouseWheel;
        }

        // ===========================================================================================================
        private void button_Add_Click(object sender, EventArgs e)
        {
            OnUserControlButtonClicked(this, e);
        }

        // ===========================================================================================================


        // ===========================================================================================================
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
    }
}
