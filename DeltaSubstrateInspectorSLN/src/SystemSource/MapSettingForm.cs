using DeltaSubstrateInspector.src.Modules.MotionModule;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeltaSubstrateInspector.src.SystemSource
{
    public partial class MapSettingForm : Form
    {
        private bool is_saved_ = false;
        private List<int> by_pass_pos_ = new List<int>();
        private string pos_cnt_ = "";
        private string row_ = "";
        private string col_ = "";
        private string bypass_ = "";

        public MapSettingForm()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (!is_number(txt.Text))
            {
                MessageBox.Show("輸入格式錯誤");
                txt.Text = MovementPos.locat_pos.ToString();
            }
            else
            {
                MovementPos.locat_pos = Int32.Parse(txt.Text);
            }
        }

        private bool is_number(string str)
        {
            try
            {
                int i = Convert.ToInt32(str);
                return true;
            }
            catch
            {
                return false;
            }

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (!is_number(txt.Text))
            {
                MessageBox.Show("輸入格式錯誤");
                txt.Text = MovementPos.row.ToString();
            }
            else
            {
                MovementPos.row = Int32.Parse(txt.Text);
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (!is_number(txt.Text))
            {
                MessageBox.Show("輸入格式錯誤");
                txt.Text = MovementPos.col.ToString();
            }
            else
            {
                MovementPos.col = Int32.Parse(txt.Text);
            }
        }

        public bool Saved
        {
            get { return this.is_saved_; }
        }

        private bool set_layout_value()
        {
            if (!is_number(textBox1.Text) || !is_number(textBox3.Text) || !is_number(textBox4.Text))
            {
                MessageBox.Show("輸入格式錯誤");
                return false;
            }
            else
            {
                MovementPos.locat_pos = Int32.Parse(textBox1.Text);
                MovementPos.row = Int32.Parse(textBox3.Text);
                MovementPos.col = Int32.Parse(textBox4.Text);
                MovementPos.bypass = txt_bypass.Text;
                return true;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (set_by_pass() && set_layout_value())
            {
                pos_cnt_ = textBox1.Text;
                row_ = textBox3.Text;
                col_ = textBox4.Text;
                bypass_ = txt_bypass.Text;
                is_saved_ = true;
                this.Close();
            }
        }

        private bool set_by_pass()
        {
            string bypass = txt_bypass.Text;
            if (bypass != "")
            {
                try
                {
                    string[] str = bypass.Split(',');
                    for (int i = 0; i < str.Length; i++)
                    {
                        if (is_number(str[i]))
                        {
                            by_pass_pos_.Add(Int32.Parse(str[i]));
                        }
                        else
                        {
                            MessageBox.Show("檢測 by pass 位置設定格式錯誤");
                            return false;
                        }
                    }
                    return true;
                }
                catch
                {
                    MessageBox.Show("檢測 by pass 位置設定格式錯誤");
                }
                return false;
            }
            return true;
        }

        public List<int> ByPassPos
        {
            get { return by_pass_pos_; }
        }

        public string PosCnt
        {
            get { return pos_cnt_; }
        }

        public string Row
        {
            get { return row_; }
        }

        public string Col
        {
            get { return col_; }
        }

        public string Bypass
        {
            get { return bypass_; }
        }
    }
}
