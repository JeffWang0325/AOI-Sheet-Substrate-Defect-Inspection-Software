using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeltaSubstrateInspector
{
    public partial class Form_DynamicButtons : Form // (20190708) Jeff Revised!
    {
        /// <summary>
        /// 使用者點擊之按鈕
        /// </summary>
        private string result = "取消";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DispInfo">對話方塊顯示之資訊</param>
        /// <param name="Text_form">視窗名稱</param>
        /// <param name="Size_form">視窗大小 (Size類別)</param>
        public Form_DynamicButtons(string DispInfo = null, string Text_form = null, object Size_form = null)
        {
            InitializeComponent();

            if (DispInfo != null)
                label_Info.Text = DispInfo;

            // Form屬性設定
            if (Text_form != null)
                this.Text = Text_form;
            if (Size_form is Size)
                this.Size = (Size)Size_form;
        }

        /// <summary>
        /// 動態新增按鈕
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="Location">(Point類別)</param>
        /// <param name="Size">(Size類別)</param>
        /// <param name="ForeColor">(Color類別)</param>
        /// <param name="BackColor">(Color類別)</param>
        public void Add_Button(string Text = null, object Location = null, object Font = null, object Size = null, object ForeColor = null, object BackColor = null)
        {
            Button bt = new Button();

            /* Button屬性設定 */
            if (Text != null)
                bt.Text = Text;
            if (Location is Point)
                bt.Location = (Point)Location;

            // 有 Default value
            if (Font is Font)
                bt.Font = (Font)Font;
            else
                bt.Font = button_cancel.Font;
            if (Size is Size)
                bt.Size = (Size)Size;
            else
                bt.Size = button_cancel.Size;
            if (ForeColor is Color)
                bt.ForeColor = (Color)ForeColor;
            else
                bt.ForeColor = Color.White;
            if (BackColor is Color)
                bt.BackColor = (Color)BackColor;
            else
                bt.BackColor = Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(166)))), ((int)(((byte)(137)))));

            /* 按鍵觸發事件 */
            bt.Click += DynamicButtons_Click;

            this.Controls.Add(bt);
        }

        void DynamicButtons_Click(object sender, EventArgs e)
        {
            //Button bt = sender as Button;
            result = (sender as Button).Text;
            DialogResult = DialogResult.OK; // 會自動關閉表單
        }

        /// <summary>
        /// 回傳使用者點擊之按鈕
        /// </summary>
        /// <returns></returns>
        public string GetResult()
        {
            return result;
        }

        /// <summary>
        /// 使用者點擊【取消】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel; // 會自動關閉表單
        }
    }
}
