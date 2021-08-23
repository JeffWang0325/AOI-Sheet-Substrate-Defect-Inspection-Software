using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using HalconDotNet;

namespace DeltaSubstrateInspector.src.PositionMethod.LocateMethod.Location
{
    public partial class DispWindow_AICellImg : Form // (20191023) Jeff Revised!
    {
        public delegate void FormClosedHandler(bool FormClosed); // 定義委派
        public event FormClosedHandler FormClosedEvent; // 定義事件

        public DispWindow_AICellImg()
        {
            InitializeComponent();
            this.hSmartWindowControl_AICellImg.MouseWheel += hSmartWindowControl_AICellImg.HSmartWindowControl_MouseWheel;
        }

        /// <summary>
        /// 設定標題文字
        /// </summary>
        /// <param name="Title"></param>
        public void Set_Title(string Title = "")
        {
            this.Text = Title;
        }

        /// <summary>
        /// 設定 txt_LabelType_AICellImg 之 Visible 屬性
        /// </summary>
        /// <param name="Visible"></param>
        public void Set_txt_LabelType_Visible(bool Visible = false) // (20200429) Jeff Revised!
        {
            this.txt_LabelType_AICellImg.Visible = Visible;
        }
        
        private void DispWindow_AICellImg_Load(object sender, EventArgs e)
        {
            #region 初始化參數

            this.nud_Width.Value = hSmartWindowControl_AICellImg.Width;
            this.trackBar_Width.Value = hSmartWindowControl_AICellImg.Width;
            this.nud_Height.Value = hSmartWindowControl_AICellImg.Height;
            this.trackBar_Height.Value = trackBar_Height.Maximum - hSmartWindowControl_AICellImg.Height;

            #endregion
        }

        /// <summary>
        /// 更新影像顯示
        /// </summary>
        /// <param name="img"></param>
        /// <param name="b_SetPart"></param>
        /// <param name="defectReg">瑕疵Region</param>
        public void Update_DispImg(HObject img, bool b_SetPart = false, HObject defectReg = null) // (20200429) Jeff Revised!
        {
            HOperatorSet.ClearWindow(hSmartWindowControl_AICellImg.HalconWindow);
            if (img == null)
                return;

            HOperatorSet.DispObj(img, hSmartWindowControl_AICellImg.HalconWindow);
            if (b_SetPart)
                HOperatorSet.SetPart(hSmartWindowControl_AICellImg.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part

            if (defectReg != null)
            {
                HOperatorSet.SetDraw(hSmartWindowControl_AICellImg.HalconWindow, "fill");
                HOperatorSet.SetColor(hSmartWindowControl_AICellImg.HalconWindow, "red");
                HOperatorSet.DispObj(defectReg, hSmartWindowControl_AICellImg.HalconWindow);
            }
        }

        /// <summary>
        /// 更新顯示該顆Cell分類
        /// </summary>
        /// <param name="text"></param>
        /// <param name="BackColor"></param>
        public void Update_LabelType(string text, Color BackColor)
        {
            this.txt_LabelType_AICellImg.BackColor = BackColor;
            this.txt_LabelType_AICellImg.Text = text;
        }

        private void DispWindow_AICellImg_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                // 觸發事件
                FormClosedEvent(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void trackBar_Scroll(object sender, EventArgs e)
        {
            Control c = sender as Control;
            string tag = c.Tag.ToString();
            switch (tag)
            {
                case "Width":
                    {
                        nud_Width.Value = trackBar_Width.Value;
                    }
                    break;
                case "Height":
                    {
                        nud_Height.Value = trackBar_Height.Minimum + trackBar_Height.Maximum - trackBar_Height.Value;
                    }
                    break;
            }
        }

        private void nud_ValueChanged(object sender, EventArgs e)
        {
            Control c = sender as Control;
            string tag = c.Tag.ToString();
            switch (tag)
            {
                case "Width":
                    {
                        this.trackBar_Width.Scroll -= new System.EventHandler(this.trackBar_Scroll);
                        trackBar_Width.Value = int.Parse(nud_Width.Value.ToString());
                        this.trackBar_Width.Scroll += new System.EventHandler(this.trackBar_Scroll);

                        hSmartWindowControl_AICellImg.Width = int.Parse(nud_Width.Value.ToString());
                        this.Width = hSmartWindowControl_AICellImg.Width + 90;
                    }
                    break;
                case "Height":
                    {
                        this.trackBar_Height.Scroll -= new System.EventHandler(this.trackBar_Scroll);
                        trackBar_Height.Value = trackBar_Height.Minimum + trackBar_Height.Maximum - int.Parse(nud_Height.Value.ToString());
                        this.trackBar_Height.Scroll += new System.EventHandler(this.trackBar_Scroll);

                        hSmartWindowControl_AICellImg.Height = int.Parse(nud_Height.Value.ToString());
                        this.Height = hSmartWindowControl_AICellImg.Height + 90;
                    }
                    break;
            }
            HOperatorSet.SetPart(hSmartWindowControl_AICellImg.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part
        }

    }
}
