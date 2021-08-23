using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Extension;
namespace DeltaSubstrateInspector.src.Modules.PositionModule
{
    public partial class BigmapShowForm : Form
    {
        public BigmapShowForm(HObject big_img)
        {
            InitializeComponent();
            pictureBox1.Image = big_img.GetRGBBitmap();
        }
    }
}
