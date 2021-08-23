using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeltaSubstrateInspector.src.SystemSource.WorkSheet
{
    public partial class WorkSheetForm : Form
    {
        private bool is_saved_ = false;
        private string work_number = "";
        private string work_type_ = "";

        private string mNewRecipeName = "";

        public WorkSheetForm(string work_number)
        {
            InitializeComponent();
            textBox_NewRecipeName.Text = work_number;
           
        }

        public WorkSheetForm(string Name, int Tmp, string defaultText = "") // (20200429) Jeff Revised!
        {
            InitializeComponent();
            label1.Text = Name;
            this.Text = Name;
            this.textBox_NewRecipeName.Text = defaultText; // (20200429) Jeff Revised!
            this.ActiveControl = textBox_NewRecipeName;
        }

        // 181026,andy
        public void Update(string work_number)
        {
            textBox_NewRecipeName.Text = work_number;
            is_saved_ = true;

        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox_NewRecipeName.Text))
            {
                MessageBox.Show("未輸入內容!");
                return;
            }
            is_saved_ = true;
            this.Close();
        }

        public bool Saved
        {
            get { return is_saved_; }
            set { this.is_saved_ = value; }
        }

        public string WorkNumber
        {
            get { return this.work_number; }
        }

        public string WorkType
        {
            get { return this.work_type_; }
        }

        public string NewRecipeName
        {
            get { return this.mNewRecipeName; }
        }


        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            work_type_ = txt.Text;
        }

        private void textBox_NewRecipeName_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;

            //work_number = txt.Text;
            mNewRecipeName = txt.Text;

        }
    }
}
