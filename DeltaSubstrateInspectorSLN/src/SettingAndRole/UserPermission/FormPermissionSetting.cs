using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using static DeltaSubstrateInspector.AOIMainForm;
using DeltaSubstrateInspector.FileSystem;

namespace DeltaSubstrateInspector
{
    public partial class FormPermissionSetting : Form
    {
        clsPermissionSetting[] PermissionSetting;
        string SavePathFile = FileSystem.FileSystem.ModuleParamDirectory + "\\" + FileSystem.FileSystem.ConfigDirectory + FileSystem.FileSystem.UserUISettingPathFile;

        clsPermissionSetting NowUser;
        bool bSave = false;
        int NowUserLevel = 0;

        public FormPermissionSetting(clsPermissionSetting[] pmPermissionSetting,int pmNowUserLevel)
        {
            InitializeComponent();
            this.NowUserLevel = pmNowUserLevel;
            this.PermissionSetting = pmPermissionSetting.DeepClone();
            UpdateCbxList();
            comboBoxUser.SelectedIndex = 0;
            this.Text = "目前權限 : " + ConvertLevel(NowUserLevel);
        }

        public void UpdateCbxList()
        {
            string[] ArrayUI = Enum.GetNames(typeof(enuUI));
            cbxListUISetting.Items.Clear();
            for (int i = 0; i < ArrayUI.Length; i++)
            {
                cbxListUISetting.Items.Add(ArrayUI[i].ToString());
            }
        }

        public string ConvertLevel(int UserLevel)
        {
            string Res = "";

            try
            {
                switch(UserLevel)
                {
                    case 0:
                        Res = "供應商";
                        break;
                    case 1:
                        Res = "管理員";
                        break;
                    case 2:
                        Res = "操作員";
                        break;
                    default:
                        Res = "Error";
                        break;
                }
            }
            catch
            {
                Res = "Expection...";
            }

            return Res;
        }

        public void UpdateUIEnabled()
        {
            if (comboBoxUser.SelectedIndex <= NowUserLevel && NowUserLevel != 0)
            {
                cbxListUISetting.Enabled = false;
                btnSelectAll.Enabled = false;
                btnClearAll.Enabled = false;
            }
            else
            {
                cbxListUISetting.Enabled = true;
                btnSelectAll.Enabled = true;
                btnClearAll.Enabled = true;
            }
            for (int i = 0; i < NowUser.UISettingList.Count; i++)
            {
                cbxListUISetting.SetItemChecked(i, NowUser.UISettingList[i].Enabled);
            }
        }

        public static bool SaveUserXML(string SavePath, clsPermissionSetting[] P)
        {
            bool Res = false;

            try
            {
                clsPermissionSetting[] Product = clsStaticTool.Clone<clsPermissionSetting[]>(P);
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(SavePath));
                XmlSerializer XmlS = new XmlSerializer(Product.GetType());
                Stream S = File.Open(SavePath, FileMode.Create);
                XmlS.Serialize(S, Product);
                S.Close();

            }
            catch
            {
                return Res;
            }

            Res = true;
            return Res;
        }

        public bool GetbSave()
        {
            return bSave;
        }

        public clsPermissionSetting[] GetParam()
        {
            return PermissionSetting;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("是否儲存?", "Yes / NO", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (dialogResult != DialogResult.Yes)
            {
                return;
            }

            SaveUserXML(SavePathFile, PermissionSetting);
            bSave = true;
            MessageBox.Show("儲存完成");
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBoxUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxUser.SelectedIndex < 0)
                return;

            NowUser = PermissionSetting[comboBoxUser.SelectedIndex];

            UpdateUIEnabled();
        }

        private void cbxListUISetting_SelectedValueChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < NowUser.UISettingList.Count; i++)
            {
                NowUser.UISettingList[i].Enabled = cbxListUISetting.GetItemChecked(i);
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            cbxListUISetting.SelectedIndexChanged -= cbxListUISetting_SelectedValueChanged;
            for (int i = 0; i < NowUser.UISettingList.Count; i++)
            {
                NowUser.UISettingList[i].Enabled = true;
            }
            UpdateUIEnabled();
            cbxListUISetting.SelectedIndexChanged += cbxListUISetting_SelectedValueChanged;
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            cbxListUISetting.SelectedIndexChanged -= cbxListUISetting_SelectedValueChanged;
            for (int i = 0; i < NowUser.UISettingList.Count; i++)
            {
                NowUser.UISettingList[i].Enabled = false;
            }
            UpdateUIEnabled();
            cbxListUISetting.SelectedIndexChanged += cbxListUISetting_SelectedValueChanged;
        }
    }
}
