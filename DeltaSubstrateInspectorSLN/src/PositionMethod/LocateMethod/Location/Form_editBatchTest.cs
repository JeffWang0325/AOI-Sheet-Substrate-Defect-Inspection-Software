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
    public partial class Form_editBatchTest : Form // (20200429) Jeff Revised!
    {
        /// <summary>
        /// 批量測試 (人工覆判 & 瑕疵整合輸出)
        /// </summary>
        private cls_BatchTest BatchTest { get; set; } = new cls_BatchTest();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="batchTest"></param>
        /// <param name="b_OutputFile">是否為瑕疵整合輸出</param>
        /// <param name="b_OnlyDisp_LotNum">是否只顯示LotNum項目</param>
        public Form_editBatchTest(string title, cls_BatchTest batchTest, bool b_OutputFile = false, bool b_OnlyDisp_LotNum = false)
        {
            InitializeComponent();

            this.Text = title;
            this.BatchTest = batchTest;
            this.ui_parameters(false);
            if (b_OutputFile)
                this.cbx_B_combine_1File.Visible = true;
            if (b_OnlyDisp_LotNum)
            {
                this.label_SBID.Visible = false;
                this.textBox_SBID.Visible = false;
                this.cbx_B_All_SBID.Visible = false;
                this.cbx_B_combine_1File.Visible = false;
            }
        }
        
        /// <summary>
        /// 將 GUI參數 與 class參數 互傳
        /// </summary>
        /// <param name="ui_2_parameters_">True: UI傳至class, False: class傳至UI</param>
        public bool ui_parameters(bool ui_2_parameters_)
        {
            bool b_status_ = false;
            try
            {
                if (ui_2_parameters_)
                {
                    #region 將UI內容回傳至class

                    this.BatchTest.partNumber = this.textBox_PartNumber.Text;
                    this.BatchTest.B_All_SBID = this.cbx_B_All_SBID.Checked;
                    this.BatchTest.SBID = this.textBox_SBID.Text;
                    this.BatchTest.B_combine_1File = this.cbx_B_combine_1File.Checked;

                    #endregion
                }
                else
                {
                    #region 將class內容傳至UI

                    this.textBox_PartNumber.Text = this.BatchTest.partNumber;
                    this.cbx_B_All_SBID.Checked = this.BatchTest.B_All_SBID;
                    if (this.BatchTest.B_All_SBID == false)
                        this.textBox_SBID.Text = this.BatchTest.SBID;
                    this.cbx_B_combine_1File.Checked = this.BatchTest.B_combine_1File;

                    #endregion
                }

                b_status_ = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return b_status_;
        }

        /// <summary>
        /// 【搜尋所有序號】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbx_B_All_SBID_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbx_B_All_SBID.Checked)
            {
                this.textBox_SBID.Text = "";
                this.textBox_SBID.Enabled = false;
            }
            else
                this.textBox_SBID.Enabled = true;
        }

        /// <summary>
        /// 【確定】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_yes_Click(object sender, EventArgs e)
        {
            this.ui_parameters(true);
            DialogResult = DialogResult.OK; // 會自動關閉表單
        }

        /// <summary>
        /// 【取消】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel; // 會自動關閉表單
        }
    }
}
