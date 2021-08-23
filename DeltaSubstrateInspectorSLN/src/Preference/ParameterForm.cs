
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
using Risitanse_AOI.src.PositionMethod;

namespace Risitanse_AOI.src.MainSetting
{
    public partial class ParameterForm : Form
    {
        public ParameterForm(string info)
        {
            InitializeComponent();
            label1.Text = info;

            string[] model_info = info.Split(';');
            string ttt = model_info[1];
            string section = ConfigurationManager.AppSettings[ttt];


            //UserControl usrctl = (UserControl)Assembly.GetExecutingAssembly().CreateInstance(String.Format("Application1.PopupContent.{0}", "InspectModelSetting"));

            //Type type = Type.GetType("InspectModelSetting"); //target type
            //object o = Activator.CreateInstance(type); // an instance of target type

            string formTypeFullName = string.Format("{0}.{1}", "Risitanse_AOI.src.PositionMethod", section);
            Type type = Type.GetType(formTypeFullName, true);
            Form item = (Form)Activator.CreateInstance(type);


            using (item)
            {
                item.ShowDialog();
            }
                //this.Controls.Add(item);




            //var section = ConfigurationManager.GetSection("Test");
            //var comm = section.Comm;
            //MessageBox.Show("Name: " + section.Name + "         " + "Type: " + section.Type + "         " + "Value: " + comm.Value);

        }
    }
}
