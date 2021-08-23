using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeltaSubstrateInspector.src.InspectionForms.UC
{
    public partial class IRoleSetPreview : UserControl
    {

        public event EventHandler OnUserControlButtonClicked;

        private bool is_selected_ = false;
        private int id_ = 0;
       
        public IRoleSetPreview()
        {
            InitializeComponent();
        }

        public void set_title(string title)
        {
            lbl_title.Text = title;
        }

        public void set_information(string info)
        {
            lbl_info.Text = info;
        }

     
        private void IRoleSetPreview_Load(object sender, EventArgs e)
        {
            
        }

        public bool Selected
        {
            get { return this.is_selected_; }
        }

        public int ID
        {
            get { return this.id_; }
            set { this.id_ = value; }
        }

        // 181105, andy
        public string Info
        {
            get { return this.lbl_info.Text; }
            //set { this.id_ = value; }
        }
             
        private void IRoleSetPreview_Click(object sender, EventArgs e)
        {
            this.is_selected_ ^= true;

            if (is_selected_)
                this.BackColor = Color.FromArgb(255, 128, 128);
            else
                this.BackColor = Color.FromArgb(240, 240, 240);

            //181105,  andy
            OnUserControlButtonClicked(sender, e);

        }


        public void AutoClick()
        {
            IRoleSetPreview_Click(this, null);
        }


    }
}
