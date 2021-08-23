using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using DeltaSubstrateInspector.src.Modules;
using System.Media; // For SystemSounds

namespace DeltaSubstrateInspector.src.PositionMethod.LocateMethod.Location
{
    public partial class Form_editDefect : Form // (20190701) Jeff Revised!
    {

        private LocateMethod Locate_Method = new LocateMethod();
        private bool B_EditItem = true;
        private object Key = 0;
        private bool b_error = false;

        /// <summary>
        /// 原Priority
        /// </summary>
        private int Priority_Origin = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locate_Method"></param>
        /// <param name="b_EditItem">是否為【編輯項目】</param>
        /// <param name="key"></param>
        /// <param name="b_PriorityRevised">是否可以修改Priority</param>
        public Form_editDefect(LocateMethod locate_Method, bool b_EditItem = false, object key = null, bool b_PriorityRevised = true) // (20190711) Jeff Revised!
        {
            InitializeComponent();

            try
            {
                this.Locate_Method = locate_Method;
                this.B_EditItem = b_EditItem;
                this.Key = key;
                if (this.B_EditItem) // 編輯項目
                {
                    this.textBox_Name.Enabled = false;
                    // 取出該Key之Value
                    DefectsResult data = this.Locate_Method.DefectsClassify[(string)this.Key];
                    this.textBox_Name.Text = (string)this.Key;
                    this.nud_Priority.Value = data.Priority;
                    this.button_SetColor.BackColor = data.GetColor();
                    this.trackBar_transparency.Value = this.button_SetColor.BackColor.A; // (20190710) Jeff Revised!
                    this.Priority_Origin = data.Priority;
                    if (!b_PriorityRevised) // (20190711) Jeff Revised!
                        this.nud_Priority.Enabled = false;
                }
                else // 新增項目
                {

                }
            }
            catch
            {
                b_error = true;
            }
        }

        private void Form_editDefect_Load(object sender, EventArgs e)
        {
            if (b_error)
            {
                DialogResult = DialogResult.Cancel; // 會自動關閉表單
                MessageBox.Show("開啟表單失敗", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 顏色設定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_SetColor_Click(object sender, EventArgs e)
        {
            if (colorDialog_SetColor.ShowDialog() != DialogResult.Cancel)
            {
                button_SetColor.BackColor = colorDialog_SetColor.Color;
                // 更新透明度
                trackBar_transparency.Value = button_SetColor.BackColor.A; // (20190710) Jeff Revised!
            }
        }

        /// <summary>
        /// 使用者移動滑桿
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trackBar_transparency_Scroll(object sender, EventArgs e) // (20190710) Jeff Revised!
        {
            button_SetColor.BackColor = Color.FromArgb(trackBar_transparency.Value, button_SetColor.BackColor.R, button_SetColor.BackColor.G, button_SetColor.BackColor.B);
        }

        /// <summary>
        /// 更新滑桿數值顯示標籤及其位置 (滑桿數值改變)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trackBar_transparency_ValueChanged(object sender, EventArgs e) // (20190710) Jeff Revised!
        {
            //  更新滑桿數值顯示
            label_transparencyValue.Text = trackBar_transparency.Value.ToString();

            // 更新顯示標籤位置
            int Min = label_transparencyValue_Min.Top, Max = label_transparencyValue_Max.Top;
            int value = trackBar_transparency.Value;
            label_transparencyValue.Top = (int)(Min + (Max - Min) / ((double)(trackBar_transparency.Maximum - trackBar_transparency.Minimum)) * (value - trackBar_transparency.Minimum) + 0.5);
        }

        private void button_yes_Click(object sender, EventArgs e)
        {
            string Str_Color_Halcon = clsStaticTool.GetHalconColor(button_SetColor.BackColor); // (20200429) Jeff Revised!

            if (B_EditItem) // 編輯項目
            {
                // 設定該Key之Value
                if (!(Locate_Method.Edit_Priority_DefectsClassify((string)Key, Priority_Origin, (int)nud_Priority.Value, Str_Color_Halcon)))
                {
                    SystemSounds.Exclamation.Play();
                    MessageBox.Show("已存在相同優先權，請重新設定!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else //新增項目
            {
                if (!(Locate_Method.Add_New_DefectsClassify(textBox_Name.Text, (int)nud_Priority.Value, Str_Color_Halcon)))
                {
                    SystemSounds.Exclamation.Play();
                    MessageBox.Show("新增失敗，確請認: \n 1. 瑕疵名稱是否已存在 \n 2. 瑕疵名稱不能為【OK】及【Cell瑕疵】 \n 3. 優先權是否已存在", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            DialogResult = DialogResult.OK; // 會自動關閉表單
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel; // 會自動關閉表單
        }
        
    }
}
