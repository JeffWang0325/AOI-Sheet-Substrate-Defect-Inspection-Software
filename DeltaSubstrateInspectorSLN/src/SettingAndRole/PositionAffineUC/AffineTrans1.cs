using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace DeltaSubstrateInspector.src.SettingAndRole.PositionAffineUC
{
    public partial class AffineTrans1 : UserControl
    {
        public AffineTrans1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            //建立根節點
            XmlElement function = doc.CreateElement("Function");
            doc.AppendChild(function);
            //建立子節點
            XmlElement parameters= doc.CreateElement("Parameters");
            parameters.SetAttribute("TargetCount", txt_count.Text);
            function.AppendChild(parameters);

            parameters = doc.CreateElement("Parameters");
            parameters.SetAttribute("MinThreshod", txt_thre_min.Text);
            function.AppendChild(parameters);

            parameters = doc.CreateElement("Parameters");
            parameters.SetAttribute("MaxThreshod", txt_thre_max.Text);
            function.AppendChild(parameters);

            parameters = doc.CreateElement("Parameters");
            parameters.SetAttribute("MinArea", txt_area_min.Text);
            function.AppendChild(parameters);

            parameters = doc.CreateElement("Parameters");
            parameters.SetAttribute("MaxArea", txt_area_max.Text);
            function.AppendChild(parameters);

            doc.Save("AffineTrans1.xml");
        }
    }
}
