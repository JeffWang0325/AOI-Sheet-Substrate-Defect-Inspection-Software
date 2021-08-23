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

namespace DeltaSubstrateInspector.UIControl
{
    public partial class ToDoItemUI : UserControl
    {
        public event EventHandler this_click_event;
        private HObject standerd_img_;

        public ToDoItemUI()
        {
            InitializeComponent();
        }

        public void set_item_name(string title)
        {
            lbl_name.Text = title;
        }

        public void set_info_1(string info)
        {
            lbl_info1.Text = info;
        }

        public void set_info_2(string info)
        {
            lbl_info2.Text = (info == "0") ? "瑕疵檢測" : "單元量測";
        }

        public void set_info_3(string info)
        {
            lbl_info3.Text = info;
        }

        public void set_icon_color1(int r, int g, int b)
        {
            this.icon_1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(r)))), ((int)(((byte)(g)))), ((int)(((byte)(b)))));
        }

        public void set_icon_color2(int r, int g, int b)
        {
            this.icon_2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(r)))), ((int)(((byte)(g)))), ((int)(((byte)(b)))));
        }

        public int get_height()
        {
            return this.Height;
        }

        public int get_width()
        {
            return this.Width;
        }

        private void ToDoItemUI_Click(object sender, EventArgs e)
        {
            try
            {
                this.BorderStyle = BorderStyle.None; // (20200729) Jeff Revised!
                this_click_event(sender, e);
            }
            catch { }
        }

        public string get_title_name()
        {
            return lbl_name.Text;
        }

        public HObject get_img()
        {
            return standerd_img_;
        }

        private void ToDoItemUI_MouseLeave(object sender, EventArgs e) // (20200729) Jeff Revised!
        {
            this.BackColor = Color.White;
        }

        private void ToDoItemUI_MouseEnter(object sender, EventArgs e) // (20200729) Jeff Revised!
        {
            this.BackColor = Color.LightBlue;
        }

        private void ToDoItemUI_MouseDown(object sender, MouseEventArgs e) // (20200729) Jeff Revised!
        {
            this.BorderStyle = BorderStyle.Fixed3D;
        }

        private void ToDoItemUI_MouseUp(object sender, MouseEventArgs e) // (20200729) Jeff Revised!
        {
            this.BorderStyle = BorderStyle.None;
        }
    }
}
