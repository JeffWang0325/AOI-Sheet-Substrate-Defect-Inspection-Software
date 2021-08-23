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
    public partial class InspectResultUI : UserControl
    {
        public event EventHandler this_SelectedIndexChanged_event;
        public event EventHandler this_checkBox_DrawType_CheckedChanged_event;

        public InspectResultUI()
        {
            InitializeComponent();

        }

        #region Public for External using

        public void Set_item_name(string _itemName)
        {
            lbl_DefectName.Text = _itemName;
        }

        /// <summary>
        /// 設定是否為瑕疵
        /// </summary>
        /// <param name="b_Defect"></param>
        public void Set_button_isDefect(bool b_Defect) // (20190815) Jeff Revised!
        {
            if (b_Defect)
            {

            }
            else
            {
                button_isDefect.Text = "Non Defect";
                button_isDefect.BackColor = Color.Green;
                // 瑕疵外框
                checkBox_DefectFrame.Visible = false; // (20190815) Jeff Revised!
                nud_DefectFrame.Visible = false;
            }
        }

        public CheckedListBox GetCheckListBox()
        {
            return this.checkedListBox_DefectSel;
        }

        public string GetColor()
        {
            // 轉成Halcon 能設定的顏色字串
            string colorStr = "";
            try
            {
                colorStr = "#" + button_SetColor.BackColor.R.ToString("X2") + button_SetColor.BackColor.G.ToString("X2") + button_SetColor.BackColor.B.ToString("X2")
                               + button_SetColor.BackColor.A.ToString("X2"); // (20190624) Jeff Revised!
            }
            catch (Exception ex)
            {
                //doing nothing
            }

            return colorStr;
        }

        public Color GetColor_ColorType() // (20190624) Jeff Revised!
        {
            return button_SetColor.BackColor;
        }

        /// <summary>
        /// 將 Halcon 顏色格式轉換成 button_SetColor 能設定之顏色
        /// </summary>
        /// <param name="str_Color_Halcon">16進位格式: #RGBA</param>
        public void SetColor(string str_Color_Halcon) // (20190624) Jeff Revised!
        {
            try
            {
                if (str_Color_Halcon.Length == 9)
                    button_SetColor.BackColor = Color.FromArgb(Convert.ToInt32(str_Color_Halcon.Substring(7, 2), 16), Convert.ToInt32(str_Color_Halcon.Substring(1, 2), 16), 
                                                               Convert.ToInt32(str_Color_Halcon.Substring(3, 2), 16), Convert.ToInt32(str_Color_Halcon.Substring(5, 2), 16));
                else
                    button_SetColor.BackColor = Color.FromArgb(Convert.ToInt32(str_Color_Halcon.Substring(1, 2), 16), Convert.ToInt32(str_Color_Halcon.Substring(3, 2), 16), Convert.ToInt32(str_Color_Halcon.Substring(5, 2), 16));
            }
            catch (Exception ex)
            {
                // Default color
                button_SetColor.BackColor = Color.Red;
            }
        }

        public void SetColor(Color set_Color) // (20190715) Jeff Revised!
        {
            try
            {
                button_SetColor.BackColor = set_Color;
            }
            catch (Exception ex)
            {
                // Default color
                button_SetColor.BackColor = Color.Red;
            }
        }

        public void SetColor_update(Color set_Color) // (20190715) Jeff Revised!
        {
            try
            {
                button_SetColor.BackColor = set_Color;
                // 同步更新
                this_SelectedIndexChanged_event(lbl_DefectName, null);
            }
            catch (Exception ex)
            {
                // Default color
                button_SetColor.BackColor = Color.Red;
            }
        }

        public bool GetDrawType()
        {
            return checkBox_DrawType.Checked;
        }

        public void SetLog(string _log)
        {
            richTextBox_log.Clear();
            richTextBox_log.AppendText(_log + "\n");
            richTextBox_log.ScrollToCaret();
        }

        #endregion

        private void button_SetColor_Click(object sender, EventArgs e)
        {
            colorDialog_SetColor.Color = button_SetColor.BackColor; // 初始顏色 (20200429) Jeff Revised!
            if (colorDialog_SetColor.ShowDialog() != DialogResult.Cancel)
            {
                button_SetColor.BackColor = colorDialog_SetColor.Color;

                // 同步更新
                this_SelectedIndexChanged_event(lbl_DefectName, e);
            }
        }

        /// <summary>
        /// 外層呼叫【顏色設定】按鈕
        /// </summary>
        public void Set_button_SetColor_Click() // (20190624) Jeff Revised!
        {
            button_SetColor_Click(null, null);
        }

        private void checkedListBox_DefectSel_SelectedValueChanged(object sender, EventArgs e)
        {
            this_SelectedIndexChanged_event(this, e);
        }

        private void checkedListBox_DefectSel_DoubleClick(object sender, EventArgs e)
        {
            this_SelectedIndexChanged_event(this, e);
        }

        private void checkBox_DrawType_CheckedChanged(object sender, EventArgs e)
        {
            checkBox_DrawType.Text = (checkBox_DrawType.Checked==true) ? "填滿" : "輪廓";

            this_checkBox_DrawType_CheckedChanged_event(this, e);
        }

        /// <summary>
        /// 是否畫瑕疵外框
        /// </summary>
        /// <returns></returns>
        public bool Get_DrawDefectFrame() // (20190815) Jeff Revised!
        {
            return checkBox_DefectFrame.Checked;
        }

        /// <summary>
        /// 瑕疵外框之最小外接矩形之擴展長度
        /// </summary>
        /// <returns></returns>
        public double Get_DrawDefectFrame_Length() // (20190815) Jeff Revised!
        {
            return double.Parse(nud_DefectFrame.Value.ToString());
        }

        private void checkBox_DefectFrame_CheckedChanged(object sender, EventArgs e) // (20190815) Jeff Revised!
        {
            this_SelectedIndexChanged_event(this, e);
        }

        private void nud_DefectFrame_ValueChanged(object sender, EventArgs e) // (20190815) Jeff Revised!
        {
            this_SelectedIndexChanged_event(this, e);
        }


    }
}
