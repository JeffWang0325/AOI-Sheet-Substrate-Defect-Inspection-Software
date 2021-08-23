using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DeltaSubstrateInspector.src.PositionMethod.LocateMethod.Rotation;
using DeltaSubstrateInspector.src.PositionMethod.LocateMethod.Location;
using static DeltaSubstrateInspector.FileSystem.FileSystem;
using System.IO;
using System.Xml;

namespace DeltaSubstrateInspector.src.MainPanels
{
    public partial class PositionView : UserControl
    {
        private PositionMethod.LocateMethod.Location.LocationUC locationUC1; // (20200214) Jeff Revised!
        public EventHandler ok_clicked;
        private string pos_info_ = "";

        public PositionView()
        {
            InitializeComponent();

            #region 加入locationUC1

            this.locationUC1 = new DeltaSubstrateInspector.src.PositionMethod.LocateMethod.Location.LocationUC();
            this.locationUC1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.locationUC1.Location = new System.Drawing.Point(223, 103);
            this.locationUC1.Name = "locationUC1";
            this.locationUC1.Size = new System.Drawing.Size(1580, 860);
            this.locationUC1.TabIndex = 0;
            this.Controls.Add(this.locationUC1);

            #endregion

            load_param_set_lst();

            clsLanguage.clsLanguage.SetLanguateToControls(this); // (20200214) Jeff Revised!
        }

        private void load_param_set_lst()
        {
            try
            {
                XmlDocument xml_doc_ = new XmlDocument();
                //xml_doc_.Load(ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\Label Defect.xml");
                if (!System.IO.File.Exists(ModuleParamDirectory + PositionParam + "\\Label Defect.xml"))
                    return;
                else
                    xml_doc_.Load(ModuleParamDirectory + PositionParam + "\\Label Defect.xml");

                XmlNode xml_node_ = xml_doc_.SelectSingleNode("Board_Type");
                XmlNodeList xml_child = xml_node_.ChildNodes;
                foreach (XmlNode item in xml_child)
                {
                    int index = item.Name.IndexOf('_'); // index = 4
                    string name = new String(item.Name.Skip(index + 1).Take(item.Name.Length - 1).ToArray());
                    cmb_param_lst.Items.Add(name);
                }
            }
            catch { }
        }

        public AngleAffineMethod get_affine_method()
        {
            return locationUC1.AffineMethodObj;
        }

        public LocateMethod get_locate_method()
        {
            return locationUC1.LocateMethodObj;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (locationUC1.AffineMethodObj.ModelEmpty && cmb_param_lst.Text=="")
                MessageBox.Show("尚未取得旋轉角度所需標誌設定，請確認標誌是否設定完成!");
            else
            {
                lb_is_set.Text = "完成設定";
                lb_is_set.ForeColor = Color.ForestGreen;
                pos_info_ = cmb_param_lst.Text == "" ? locationUC1.Info : cmb_param_lst.Text;
                ok_clicked(this, e);
            }
        }

        private void DeleteDir(string DirPath)
        {
            if (Directory.Exists(DirPath))
                Directory.Delete(DirPath, true);
        }

        public string PosInfo
        {
            get { return this.pos_info_; }
            set { this.pos_info_ = value; }
        }

        private void cmb_param_lst_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            UpdatePositionPara(cmb.Text, false);

            /*
            pos_info_ = cmb.Text;

            // 即時更新class內及GUI顯示之參數 (20180830) Jeff Revised!
            locationUC1.AffineMethodObj.board_type_ = pos_info_;
            locationUC1.LocateMethodObj.board_type_ = pos_info_;
            locationUC1.AffineMethodObj.load();
            locationUC1.LocateMethodObj.load();
            locationUC1.ui_parameters(false);
            */
        }

        // 181026, andy
        public void UpdatePositionPara(string PositionRecipeName, bool Ext_Call=true)
        {
            if (Ext_Call == true) cmb_param_lst.Text = PositionRecipeName;

            pos_info_ = PositionRecipeName;

            // 即時更新class內及GUI顯示之參數 (20180830) Jeff Revised!
            locationUC1.AffineMethodObj.BoardType = pos_info_;
            locationUC1.LocateMethodObj.board_type_ = pos_info_;
            //locationUC1.LocateMethodObj.load(); // 比AffineMethodObj.load()先執行，為了先產生一張影像，才能順利載入region (20190509) Jeff Revised!
            //locationUC1.AffineMethodObj.load();
            //locationUC1.ui_parameters(false);
            locationUC1.update_disp_status(locationUC1.LocateMethodObj.load() && locationUC1.AffineMethodObj.load()); // 比AffineMethodObj.load()先執行，為了先產生一張影像，才能順利載入region (20190509) Jeff Revised!
            try // (20200429) Jeff Revised!
            {
                if (!(locationUC1.LocateMethodObj.LocateMethodRecipe.VisualPosEnable)) // (20191216) Jeff Revised!
                    Affine_angle_degree = locationUC1.AffineMethodObj.Affine_degree_ = 0.0;
            }
            catch (Exception ex)
            {
                locationUC1.LocateMethodObj.LocateMethodRecipe = new LocateMethodRecipe_New();
                Affine_angle_degree = locationUC1.AffineMethodObj.Affine_degree_ = 0.0;
            }
        }

        /// <summary>
        /// 【另存設定檔】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveAs_Click(object sender, EventArgs e)
        {
            BtnLog.WriteLog(" [Location UC] Save As Click");
            if (string.IsNullOrEmpty(txbNewName.Text))
            {
                MessageBox.Show("請先輸入參數名稱");
                return;
            }
            BtnLog.WriteLog(" [Location UC] Save As Name : " + txbNewName.Text);
            for (int i = 0; i < cmb_param_lst.Items.Count; i++)
            {
                if (cmb_param_lst.Items[i].ToString() == txbNewName.Text)
                {
                    MessageBox.Show("此名稱已經被使用,請重新命名");
                    return;
                }
            }
            if (string.IsNullOrEmpty(pos_info_))
            {
                MessageBox.Show("請先選擇要另存之設定檔");
                return;
            }
            try
            {
                if (!locationUC1.AffineMethodObj.SaveAs(txbNewName.Text) || !locationUC1.LocateMethodObj.SaveAs(txbNewName.Text))
                {
                    BtnLog.WriteLog(" [Location UC] Save As Fail");
                    MessageBox.Show("儲存失敗");
                    return;
                }

                cmb_param_lst.Items.Clear();
                load_param_set_lst();
                BtnLog.WriteLog(" [Location UC] Save As Done");
                MessageBox.Show("儲存成功");
            }
            catch(Exception Ex)
            {
                MessageBox.Show("儲存失敗");
                MessageBox.Show(Ex.ToString());
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            BtnLog.WriteLog(" [Location UC] Remove Click");
            if (string.IsNullOrEmpty(pos_info_))
            {
                MessageBox.Show("請先選擇要刪除之設定檔");
                return;
            }
            BtnLog.WriteLog(" [Location UC] Remove Name : " + pos_info_);
            DialogResult dialogResult = MessageBox.Show("確認是否刪除檔案?", "Yes / NO", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (dialogResult != DialogResult.Yes)
            {
                BtnLog.WriteLog(" [Location UC] Remove Cancel");
                return;
            }

            try
            {
                if (!locationUC1.AffineMethodObj.DeleteParam() || !locationUC1.LocateMethodObj.DeleteParam())
                {
                    BtnLog.WriteLog(" [Location UC] Remove Fail");
                    MessageBox.Show("刪除失敗");
                    return;
                }
                DeleteDir(ModuleParamDirectory + PositionParam + "pos_info_//");
                cmb_param_lst.Items.Clear();
                load_param_set_lst();
                BtnLog.WriteLog(" [Location UC] Remove Done");
                MessageBox.Show("刪除完成");
            }
            catch (Exception Ex)
            {
                MessageBox.Show("刪除失敗");
                MessageBox.Show(Ex.ToString());
            }
        }
    }
}
