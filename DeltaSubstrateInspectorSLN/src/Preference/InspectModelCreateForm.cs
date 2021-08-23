using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using DeltaSubstrateInspector.src.Modules.MotionModule; // 181107, andy
using static DeltaSubstrateInspector.FileSystem.FileSystem;
using DeltaSubstrateInspector.FileSystem;
using DeltaSubstrateInspector.src.Modules.NewInspectModule.InductanceInsp;
using System.Media; // For SystemSounds (20200429) Jeff Revised!

namespace DeltaSubstrateInspector.src.MainSetting
{
    public partial class InspectModelCreateForm : Form
    {
        private string[] lights_ = null;
        private string light_info_ = "";
        private bool is_saved = false;
        private Dictionary<string, Label> Dictionary_Label = new Dictionary<string, Label>();

        public InspectModelCreateForm(string[] lights)
        {
            InitializeComponent();

            lights_ = lights;
            set_lights_chkbox();


            // 181107, andy
            txt_name.Text = "A";
            cmb_position.Text = "Virtual Segmentation";
            cmb_type.Text = "瑕疵檢測";
            comboBox1.Text = "逐一元件檢測";

            lbl_title.Text = "新增檢測模組";

            // (20190731) Jeff Revised!
            Dictionary_Label.Add("視覺定位", lbl_VisualPosEnable); // (20191214) Jeff Revised!
            Dictionary_Label.Add("Cell中心檢測", lbl_CellCenterInspectEnable);
            Dictionary_Label.Add("瑕疵分類", lbl_NG_classification);
            Dictionary_Label.Add("瑕疵優先權", lbl_NG_priority);
            Dictionary_Label.Add("人工覆判", lbl_Recheck);
            Dictionary_Label.Add("儲存統計結果 (.csv)", lbl_saveCSV);
            Dictionary_Label.Add("儲存瑕疵結果", lbl_SaveDefectResult);
            Dictionary_Label.Add("自動合併產生雙面工單", lbl_Auto_AB); // (20200429) Jeff Revised!
            Dictionary_Label.Add("AI分圖啟用", lbl_AICellImg_Enable); // (20190830) Jeff Revised!

            // (20190902) Jeff Revised!
            string[] ArrayBand = Enum.GetNames(typeof(enuBand));
            comboBoxDAVSBand1.Items.Clear();
            comboBoxDAVSBand2.Items.Clear();
            comboBoxDAVSBand3.Items.Clear();
            foreach (string Band in ArrayBand)
            {
                comboBoxDAVSBand1.Items.Add(Band);
                comboBoxDAVSBand2.Items.Add(Band);
                comboBoxDAVSBand3.Items.Add(Band);
            }

            // A面工單 (20200429) Jeff Revised!
            this.cbx_A_Recipe.Items.Clear();
            string path = ModuleParamDirectory + RecipeFileDirectory;
            // 檢查執行檔目錄是否存在
            if (System.IO.Directory.Exists(path))
            {
                string[] dirs = System.IO.Directory.GetDirectories(path);
                foreach (string item in dirs)
                {
                    string s = System.IO.Path.GetFileNameWithoutExtension(item);
                    this.cbx_A_Recipe.Items.Add(s);
                }
            }
        }

        /// <summary>
        /// 改變ON/OFF狀態
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbx_CheckedChanged(object sender, EventArgs e) // (20200429) Jeff Revised!
        {
            CheckBox cbx = sender as CheckBox;
            if (cbx.Checked) // ON
            {
                cbx.BackgroundImage = global::DeltaSubstrateInspector.Properties.Resources.ON;
                if (Dictionary_Label.ContainsKey(cbx.Tag.ToString()))
                {
                    Dictionary_Label[cbx.Tag.ToString()].Text = "ON";
                    Dictionary_Label[cbx.Tag.ToString()].ForeColor = Color.DeepSkyBlue;
                }
            }
            else // OFF
            {
                cbx.BackgroundImage = global::DeltaSubstrateInspector.Properties.Resources.OFF_edited;
                if (Dictionary_Label.ContainsKey(cbx.Tag.ToString()))
                {
                    Dictionary_Label[cbx.Tag.ToString()].Text = "OFF";
                    Dictionary_Label[cbx.Tag.ToString()].ForeColor = System.Drawing.SystemColors.ControlDarkDark;
                }
            }

            // Enabled狀態切換
            if (cbx == cbx_SaveDefectResult) // 【儲存瑕疵結果】(20190731) Jeff Revised!
            {
                if (cbx.Checked) // ON
                    this.groupBox_SaveDefectResult.Enabled = true;
                else
                    this.groupBox_SaveDefectResult.Enabled = false;
            }
            else if (cbx == cbx_Auto_AB) // 【自動合併產生雙面工單】(20200429) Jeff Revised!
            {
                if (cbx.Checked) // ON
                    this.groupBox_AB_Recipe.Enabled = true;
                else
                    this.groupBox_AB_Recipe.Enabled = false;
            }
            else if (cbx == cbx_AICellImg_Enable) // 【AI分圖啟用】(20190830) Jeff Revised!
            {
                if (cbx.Checked) // ON
                    this.groupBox_AICellImg_Info.Enabled = true;
                else
                    this.groupBox_AICellImg_Info.Enabled = false;
            }
        }

        /// <summary>
        /// 【路徑】(儲存瑕疵結果), 【工單路徑】, 【輸出路徑】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbx_Directory_Click(object sender, EventArgs e) // (20200429) Jeff Revised!
        {
            try
            {
                TextBox tb = sender as TextBox;
                FolderBrowserDialog Dilg = new FolderBrowserDialog();
                Dilg.Description = "【路徑】選擇";
                Dilg.SelectedPath = tb.Text;
                if (Dilg.ShowDialog() != DialogResult.OK)
                    return;

                if (string.IsNullOrEmpty(Dilg.SelectedPath))
                {
                    SystemSounds.Exclamation.Play();
                    MessageBox.Show("路徑無效!", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                tb.Text = Dilg.SelectedPath;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Mix Band 狀態改變
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDAVSMixBandEnabled_CheckedChanged(object sender, EventArgs e) // (20190830) Jeff Revised!
        {
            if (cbxDAVSMixBandEnabled.Checked)
                groupBox_MixBand.Enabled = true;
            else
                groupBox_MixBand.Enabled = false;
        }

        private void txt_light_cnt_TextChanged(object sender, EventArgs e) // (20190902) Jeff Revised!
        {
            int triggerCount;
            if (int.TryParse(txt_light_cnt.Text, out triggerCount))
            {
                if (triggerCount >= 1)
                {
                    nudDAVSImageId.Maximum = triggerCount - 1;
                    nudDAVSBand1ImageIndex.Maximum = triggerCount - 1;
                    nudDAVSBand2ImageIndex.Maximum = triggerCount - 1;
                    nudDAVSBand3ImageIndex.Maximum = triggerCount - 1;
                }
            }
        }

        private void set_lights_chkbox()
        {
            foreach (string light in lights_)
            {
                //this.chb_list_lights.Items.Add(light);
            }
        }

        private void txt_name_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            lbl_name.Text = txt.Text;
        }

        private void cmb_position_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            lbl_pos.Text = cmb.Text;
        }

        private void cmb_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            lbl_type.Text = cmb.Text;
        }

        private void chb_list_lights_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
            lbl_light.Text = "";
            light_info_ = "";
            for (int i = 0; i < chb_list_lights.Items.Count; i++)
            {
                if (chb_list_lights.GetItemChecked(i))
                {
                    lbl_light.Text = lbl_light.Text + String.Format("{0}\n", chb_list_lights.GetItemText(chb_list_lights.Items[i]));
                    light_info_ = (light_info_ == "" ) ? chb_list_lights.GetItemText(chb_list_lights.Items[i]) : light_info_ + "," + chb_list_lights.GetItemText(chb_list_lights.Items[i]);
                }
            }
            */
            
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


        private bool set_layout_value()
        {
            if (!is_number(textBox_RowNum.Text) || !is_number(textBox_ColumnNum.Text) || !is_number(textBox_LocationNum.Text) || !is_number(txt_light_cnt.Text))
            {
                MessageBox.Show("輸入格式錯誤");
                return false;
            }
            else
            {
                MovementPos.locat_pos = Int32.Parse(textBox_LocationNum.Text);
                MovementPos.row = Int32.Parse(textBox_RowNum.Text);
                MovementPos.col = Int32.Parse(textBox_ColumnNum.Text);
                MovementPos.bypass = textBox_Bypass.Text;
                MovementPos.triggerCount = Int32.Parse(txt_light_cnt.Text);

                FinalInspectParam.ProductName = this.txt_name.Text; // (20200429) Jeff Revised!
                FinalInspectParam.VisualPosEnable = cbx_VisualPosEnable.Checked; // (20191214) Jeff Revised!
                FinalInspectParam.CellCenterInspectEnable = cbx_CellCenterInspectEnable.Checked;

                int tempID = Int32.Parse(textBox_GlobalMapImgID.Text);
                if (tempID > MovementPos.triggerCount - 1)
                    textBox_GlobalMapImgID.Text = "0";
                FinalInspectParam.GlobalMapImgID = Int32.Parse(textBox_GlobalMapImgID.Text);

                FinalInspectParam.NGRatio = double.Parse(textBox_NGRatio.Text);

                // 新增檢測模組 (其他參數) (20190719) Jeff Revised!
                FinalInspectParam.PosImgID = Int32.Parse(textBox_PosImgID.Text);
                FinalInspectParam.b_NG_classification = cbx_NG_classification.Checked;
                FinalInspectParam.b_NG_priority = cbx_NG_priority.Checked;
                FinalInspectParam.b_Recheck = cbx_Recheck.Checked;
                FinalInspectParam.dispWindow = cbx_dispWindow.Text;
                FinalInspectParam.b_saveCSV = cbx_saveCSV.Checked;
                FinalInspectParam.b_SaveDefectResult = cbx_SaveDefectResult.Checked;
                FinalInspectParam.b_SaveAllCellImage = radioButton_SaveAllCellImage.Checked;
                FinalInspectParam.b_SaveDefectCellImage = radioButton_SaveDefectCellImage.Checked;
                FinalInspectParam.Directory_SaveDefectResult = tbx_Directory_SaveDefectResult.Text; // (20200429) Jeff Revised!
                FinalInspectParam.b_Auto_AB = cbx_Auto_AB.Checked; // (20200429) Jeff Revised!
                FinalInspectParam.A_Recipe = cbx_A_Recipe.Text; // (20200429) Jeff Revised!
                FinalInspectParam.B_UpDownInvert_FS = cbx_B_UpDown_invert.Checked; // (20200429) Jeff Revised!
                FinalInspectParam.B_LeftRightInvert_FS = cbx_B_LeftRight_invert.Checked; // (20200429) Jeff Revised!
                FinalInspectParam.Directory_defComb = tbx_Directory_defComb.Text; // (20200429) Jeff Revised!
                FinalInspectParam.OutputDirectory_defComb = tbx_OutputDirectory_defComb.Text; // (20200429) Jeff Revised!

                FinalInspectParam.b_AICellImg_Enable = cbx_AICellImg_Enable.Checked; // (20190830) Jeff Revised!

                // (20190902) Jeff Revised!
                FinalInspectParam.DAVSImgID = int.Parse(nudDAVSImageId.Value.ToString());
                FinalInspectParam.b_DAVSImgAlign = cbxDAVSImgAlignEnabled.Checked;
                FinalInspectParam.b_DAVSMixImgBand = cbxDAVSMixBandEnabled.Checked;
                FinalInspectParam.DAVSBand1 = (enuBand)comboBoxDAVSBand1.SelectedIndex;
                FinalInspectParam.DAVSBand2 = (enuBand)comboBoxDAVSBand2.SelectedIndex;
                FinalInspectParam.DAVSBand3 = (enuBand)comboBoxDAVSBand3.SelectedIndex;
                FinalInspectParam.DAVSBand1ImgIndex = int.Parse(nudDAVSBand1ImageIndex.Value.ToString());
                FinalInspectParam.DAVSBand2ImgIndex = int.Parse(nudDAVSBand2ImageIndex.Value.ToString());
                FinalInspectParam.DAVSBand3ImgIndex = int.Parse(nudDAVSBand3ImageIndex.Value.ToString());
                
                return true;
            }
        }

        public class clsRecipeParam
        {
            public string ModeName { get; set; }
            public string LocationNum;
            public string RowNum;
            public string ColumnNum;
            public string Bypass;
            public string light_cnt;
            public string ProductName; // (20200429) Jeff Revised!
            public bool VisualPosEnable; // (20191214) Jeff Revised!
            public bool CellCenterInspectEnable;
            public string GlobalMapImgID;
            public string NGRatio;

            // 新增檢測模組 (其他參數) (20190719) Jeff Revised!
            public string PosImgID;
            public bool b_NG_classification;
            public bool b_NG_priority;
            public bool b_Recheck;
            public string dispWindow;
            public bool b_saveCSV;
            public bool b_SaveDefectResult;
            public bool b_SaveAllCellImage;
            public bool b_SaveDefectCellImage;
            public string Directory_SaveDefectResult; // (20200429) Jeff Revised!
            public bool b_Auto_AB; // (20200429) Jeff Revised!
            public string A_Recipe; // (20200429) Jeff Revised!
            public bool B_UpDownInvert_FS; // (20200429) Jeff Revised!
            public bool B_LeftRightInvert_FS; // (20200429) Jeff Revised!
            public string Directory_defComb; // (20200429) Jeff Revised!
            public string OutputDirectory_defComb; // (20200429) Jeff Revised!

            public bool b_AICellImg_Enable; // (20190830) Jeff Revised!

            // (20190902) Jeff Revised!
            public int DAVSImgID;
            public bool b_DAVSImgAlign;
            public bool b_DAVSMixImgBand;
            public int DAVSBand1ImgIndex;
            public int DAVSBand2ImgIndex;
            public int DAVSBand3ImgIndex;
            public enuBand DAVSBand1;
            public enuBand DAVSBand2;
            public enuBand DAVSBand3;

            public clsRecipeParam() { }
        }

        public clsRecipeParam Get_Cuttent_Recipe_Param()
        {
            clsRecipeParam Res = new clsRecipeParam();

            Res.ModeName = lbl_title.Text;
            Res.LocationNum = textBox_LocationNum.Text;
            Res.RowNum = textBox_RowNum.Text;
            Res.ColumnNum = textBox_ColumnNum.Text;
            Res.Bypass = textBox_Bypass.Text;
            Res.light_cnt = txt_light_cnt.Text;
            Res.ProductName = this.txt_name.Text; // (20200429) Jeff Revised!
            Res.VisualPosEnable = cbx_VisualPosEnable.Checked; // (20191214) Jeff Revised!
            Res.CellCenterInspectEnable = cbx_CellCenterInspectEnable.Checked;
            Res.GlobalMapImgID = textBox_GlobalMapImgID.Text;
            Res.NGRatio = textBox_NGRatio.Text;

            // 新增檢測模組 (其他參數) (20190719) Jeff Revised!
            Res.PosImgID = textBox_PosImgID.Text;
            Res.b_NG_classification = cbx_NG_classification.Checked;
            Res.b_NG_priority = cbx_NG_priority.Checked;
            Res.b_Recheck = cbx_Recheck.Checked;
            Res.dispWindow = cbx_dispWindow.Text;
            Res.b_saveCSV = cbx_saveCSV.Checked;
            Res.b_SaveDefectResult = cbx_SaveDefectResult.Checked;
            Res.b_SaveAllCellImage = radioButton_SaveAllCellImage.Checked;
            Res.b_SaveDefectCellImage = radioButton_SaveDefectCellImage.Checked;
            Res.Directory_SaveDefectResult = tbx_Directory_SaveDefectResult.Text; // (20200429) Jeff Revised!
            Res.b_Auto_AB = cbx_Auto_AB.Checked; // (20200429) Jeff Revised!
            Res.A_Recipe = cbx_A_Recipe.Text; // (20200429) Jeff Revised!
            Res.B_UpDownInvert_FS = cbx_B_UpDown_invert.Checked; // (20200429) Jeff Revised!
            Res.B_LeftRightInvert_FS = cbx_B_LeftRight_invert.Checked; // (20200429) Jeff Revised!
            Res.Directory_defComb = tbx_Directory_defComb.Text; // (20200429) Jeff Revised!
            Res.OutputDirectory_defComb = tbx_OutputDirectory_defComb.Text; // (20200429) Jeff Revised!

            Res.b_AICellImg_Enable = cbx_AICellImg_Enable.Checked; // (20190830) Jeff Revised!

            // (20190902) Jeff Revised!
            Res.DAVSImgID = int.Parse(nudDAVSImageId.Value.ToString());
            Res.b_DAVSImgAlign = cbxDAVSImgAlignEnabled.Checked;
            Res.b_DAVSMixImgBand = cbxDAVSMixBandEnabled.Checked;
            Res.DAVSBand1 = (enuBand)comboBoxDAVSBand1.SelectedIndex;
            Res.DAVSBand2 = (enuBand)comboBoxDAVSBand2.SelectedIndex;
            Res.DAVSBand3 = (enuBand)comboBoxDAVSBand3.SelectedIndex;
            Res.DAVSBand1ImgIndex = int.Parse(nudDAVSBand1ImageIndex.Value.ToString());
            Res.DAVSBand2ImgIndex = int.Parse(nudDAVSBand2ImageIndex.Value.ToString());
            Res.DAVSBand3ImgIndex = int.Parse(nudDAVSBand3ImageIndex.Value.ToString());

            return Res;
        }

        public bool update_layout_value()
        {

            lbl_title.Text = "修改檢測模組: " + ModuleName;

            textBox_LocationNum.Text = MovementPos.locat_pos.ToString();
            textBox_RowNum.Text = MovementPos.row.ToString();
            textBox_ColumnNum.Text = MovementPos.col.ToString();
            textBox_Bypass.Text = MovementPos.bypass;
            txt_light_cnt.Text = MovementPos.triggerCount.ToString();

            this.txt_name.Text = FinalInspectParam.ProductName; // (20200429) Jeff Revised!
            cbx_VisualPosEnable.Checked = FinalInspectParam.VisualPosEnable; // (20191214) Jeff Revised!
            cbx_CellCenterInspectEnable.Checked = FinalInspectParam.CellCenterInspectEnable;
            textBox_GlobalMapImgID.Text = FinalInspectParam.GlobalMapImgID.ToString();

            textBox_NGRatio.Text = FinalInspectParam.NGRatio.ToString();

            // 新增檢測模組 (其他參數) (20190719) Jeff Revised!
            textBox_PosImgID.Text = FinalInspectParam.PosImgID.ToString();
            cbx_NG_classification.Checked = FinalInspectParam.b_NG_classification;
            cbx_NG_priority.Checked = FinalInspectParam.b_NG_priority;
            cbx_Recheck.Checked = FinalInspectParam.b_Recheck;
            cbx_dispWindow.Text = FinalInspectParam.dispWindow.ToString();
            cbx_saveCSV.Checked = FinalInspectParam.b_saveCSV;
            cbx_SaveDefectResult.Checked = FinalInspectParam.b_SaveDefectResult;
            radioButton_SaveAllCellImage.Checked = FinalInspectParam.b_SaveAllCellImage;
            radioButton_SaveDefectCellImage.Checked = FinalInspectParam.b_SaveDefectCellImage;
            tbx_Directory_SaveDefectResult.Text = FinalInspectParam.Directory_SaveDefectResult; // (20200429) Jeff Revised!
            cbx_Auto_AB.Checked = FinalInspectParam.b_Auto_AB; // (20200429) Jeff Revised!
            cbx_A_Recipe.Text = FinalInspectParam.A_Recipe; // (20200429) Jeff Revised!
            cbx_B_UpDown_invert.Checked = FinalInspectParam.B_UpDownInvert_FS; // (20200429) Jeff Revised!
            cbx_B_LeftRight_invert.Checked = FinalInspectParam.B_LeftRightInvert_FS; // (20200429) Jeff Revised!
            tbx_Directory_defComb.Text = FinalInspectParam.Directory_defComb; // (20200429) Jeff Revised!
            tbx_OutputDirectory_defComb.Text = FinalInspectParam.OutputDirectory_defComb; // (20200429) Jeff Revised!

            cbx_AICellImg_Enable.Checked = FinalInspectParam.b_AICellImg_Enable; // (20190830) Jeff Revised!

            // (20190902) Jeff Revised!
            nudDAVSImageId.Maximum = MovementPos.triggerCount - 1;
            nudDAVSBand1ImageIndex.Maximum = MovementPos.triggerCount - 1;
            nudDAVSBand2ImageIndex.Maximum = MovementPos.triggerCount - 1;
            nudDAVSBand3ImageIndex.Maximum = MovementPos.triggerCount - 1;

            nudDAVSImageId.Value = decimal.Parse(FinalInspectParam.DAVSImgID.ToString());
            cbxDAVSImgAlignEnabled.Checked = FinalInspectParam.b_DAVSImgAlign;
            cbxDAVSMixBandEnabled.Checked = FinalInspectParam.b_DAVSMixImgBand;
            nudDAVSBand1ImageIndex.Value = decimal.Parse(FinalInspectParam.DAVSBand1ImgIndex.ToString());
            nudDAVSBand2ImageIndex.Value = decimal.Parse(FinalInspectParam.DAVSBand2ImgIndex.ToString());
            nudDAVSBand3ImageIndex.Value = decimal.Parse(FinalInspectParam.DAVSBand3ImgIndex.ToString());
            comboBoxDAVSBand1.SelectedIndex = (int)FinalInspectParam.DAVSBand1;
            comboBoxDAVSBand2.SelectedIndex = (int)FinalInspectParam.DAVSBand2;
            comboBoxDAVSBand3.SelectedIndex = (int)FinalInspectParam.DAVSBand3;
            
            return true;
        }


        private void btn_new_Click(object sender, EventArgs e)
        {
            // 181107, andy
            set_layout_value();

            is_saved = true;
            this.Close();
        }

        private void btn_remove_Click_1(object sender, EventArgs e)
        {
            is_saved = false;
            this.Close();
        }

        public string get_model_info()
        {
            return lbl_name.Text + ";" + lbl_pos.Text + ";" + lbl_type.Text + ";" + lbl_roi.Text + ";"+light_info_;
        }

        public bool is_save()
        {
            return is_saved;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            lbl_roi.Text = cmb.Text;
        }

        #region 讓使用者可移動視窗 (20200427) Jeff Revised!

        int curr_x, curr_y;
        bool isWndMove;

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.curr_x = e.X;
                this.curr_y = e.Y;
                this.isWndMove = true;
            }
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.isWndMove)
            {
                //this.Location = new Point(this.Left + e.X - this.curr_x, this.Top + e.Y - this.curr_y);
                this.Location = new Point(Control.MousePosition.X - e.X + (e.X - this.curr_x), Control.MousePosition.Y - e.Y + (e.Y - this.curr_y));
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            this.isWndMove = false;
        }

        #endregion
    }
}
