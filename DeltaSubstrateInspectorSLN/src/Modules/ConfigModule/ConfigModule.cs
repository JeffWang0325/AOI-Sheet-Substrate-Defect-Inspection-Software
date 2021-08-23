using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using DeltaSubstrateInspector.FileSystem;
using System.IO;
using DeltaSubstrateInspector;
using DelVivi.Common;
using System.IO.Ports; // For SerialPort

namespace IOModule
{
    public partial class ConfigModule : Form // (20191216) Jeff Revised!
    {
        private Dictionary<string, Label> Dictionary_Label = new Dictionary<string, Label>();

        public ConfigModule()
        {
            InitializeComponent();

            Dictionary_Label.Add("LightOffSet", lbl_LightOffSet);
            Dictionary_Label.Add("BypassIO", lbl_BypassIO);
            Dictionary_Label.Add("BypassLightControl", lbl_BypassLightControl);

            // (20200717) Jeff Revised!
            string[] serialPorts = SerialPort.GetPortNames();
            this.cbx_LightComportName.Items.Clear();
            foreach (string s in serialPorts)
                this.cbx_LightComportName.Items.Add(s);
        }

        private void ConfigModule_Load(object sender, EventArgs e)
        {
            #region ComboBox 新增項目

            // 新增 cbx_CameraInterface 項目
            foreach (string item in Enum.GetNames(typeof(FileSystem.enu_CameraInterface)))
                cbx_CameraInterface.Items.Add(item);

            // 新增 cbx_CallbackType 項目
            foreach (string item in Enum.GetNames(typeof(FileSystem.enu_CallBackType)))
                cbx_CallBackType.Items.Add(item);

            #endregion

            #region 更新GUI參數

            string fName = FileSystem.ModuleParamDirectory + FileSystem.ConfigDirectory + FileSystem.HardwareConfigFileName;
            if (!(File.Exists(fName)))
                MessageBox.Show("尚未建立系統設定檔，請重新設定後點擊【儲存】", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            ui_parameters(false);

            #endregion
        }

        /// <summary>
        /// 將 GUI參數 與 FileSystem參數 互傳
        /// </summary>
        /// <param name="ui_2_parameters_">True: UI傳至FileSystem, False: FileSystem傳至UI</param>
        public bool ui_parameters(bool ui_2_parameters_)
        {
            bool b_status_ = false;
            try
            {
                if (ui_2_parameters_)
                {
                    #region 將UI內容回傳至FileSystem

                    FileSystem.LightComportName = cbx_LightComportName.Text;
                    FileSystem.LightOffSet = cbx_LightOffSet.Checked;
                    FileSystem.CameraInterface = cbx_CameraInterface.SelectedIndex;
                    FileSystem.BypassIO = cbx_BypassIO.Checked;
                    FileSystem.BypassLightControl = cbx_BypassLightControl.Checked;
                    FileSystem.CallBackType = cbx_CallBackType.Text;

                    #endregion
                }
                else
                {
                    #region 將FileSystem內容傳至UI

                    cbx_LightComportName.Text = FileSystem.LightComportName;
                    cbx_LightOffSet.Checked = FileSystem.LightOffSet;
                    cbx_CameraInterface.SelectedIndex = FileSystem.CameraInterface;
                    cbx_BypassIO.Checked = FileSystem.BypassIO;
                    cbx_BypassLightControl.Checked = FileSystem.BypassLightControl;
                    cbx_CallBackType.Text = FileSystem.CallBackType;

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
        /// 改變ON/OFF狀態
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbx_CheckedChanged(object sender, EventArgs e)
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
        }

        /// <summary>
        /// 【關閉】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Close_Click(object sender, EventArgs e)
        {
            //this.Close();
            DialogResult = DialogResult.Cancel; // 會自動關閉表單
        }

        /// <summary>
        /// 【儲存】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Save_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("確認是否【儲存】?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr != DialogResult.Yes)
                return;

            if (!(ui_parameters(true)))
            {
                MessageBox.Show("【儲存】失敗!", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool b_status_ = true;
            string fName = FileSystem.ModuleParamDirectory + FileSystem.ConfigDirectory + FileSystem.HardwareConfigFileName;
            if (File.Exists(fName))
            {
                #region 儲存各參數

                DeviceConfig HardwareConfig = new DeviceConfig(fName, "Hardware");
                b_status_ &= HardwareConfig.SetParam("LightComportName", FileSystem.LightComportName);
                b_status_ &= HardwareConfig.SetParam("LightOffSet", FileSystem.LightOffSet.ToString());
                b_status_ &= HardwareConfig.SetParam("CameraInterface", FileSystem.CameraInterface.ToString());
                b_status_ &= HardwareConfig.SetParam("BypassIO", FileSystem.BypassIO.ToString());
                b_status_ &= HardwareConfig.SetParam("BypassLightControl", FileSystem.BypassLightControl.ToString());
                b_status_ &= HardwareConfig.SetParam("CallbackType", FileSystem.CallBackType);

                #endregion
            }
            else
                b_status_ = AOIMainForm.SaveConfigFile();

            if (b_status_)
            {
                DialogResult dialogResult = MessageBox.Show("Config【儲存】成功，請先關閉程式重新開啟，確認是否立即關閉程式?", "立即關閉程式", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                    DialogResult = DialogResult.Yes; // 會自動關閉表單
            }
            else
                MessageBox.Show("【儲存】失敗!", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        #region 讓使用者可移動視窗 (20200409) Jeff Revised!

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
