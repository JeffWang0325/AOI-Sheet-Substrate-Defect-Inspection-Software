using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace PasswordManagement
{
    public partial class Form_PWInput : Form
    {

        #region 預設資料夾與檔案名

        private string password_FileName = "pw.bin";
        private string DirectoryName = "";
        private string full_pw_FileName = "";

        #endregion

        #region 預設帳號、密碼、權限

        private string[] default_user = new string[3] { "Delta", "Engineer", "OP" };
        private string[] default_password = new string[3] { "034526107", "1234", "0000" };
        private int[] default_level = new int[3] { 0, 1, 2 };
        private string[] level_name = new string[3] {"供應商","管理員","操作員" };

        #endregion

        public bool HideEnable = false;

        private TabPage tp; // 帳號密碼管理頁面暫存
      
        public EventHandler PasswordLevelChange; // 權限更換成功事件

       
        public void SetVenderAccessLevel()
        {
            user.LoginID = "Delta";

            // 權限更換成功觸發事件
            if (PasswordLevelChange != null)
                PasswordLevelChange(this, null);
        }

        public void ResetAccessLevel()
        {
            user.LoginID = null;

            // 權限更換成功觸發事件
            if (PasswordLevelChange != null)
                PasswordLevelChange(this, null);

            if (tabControl1.TabCount == 2)
            {
                tabControl1.TabPages.Remove(tp); //　隐藏（删除）
            }

            if (comboBox_AccList.Items.Count > 0)
            {
                comboBox_AccList.SelectedIndex = 0;
                comboBox_NowAllAcc.SelectedIndex = 0;
            }

            textBox_PW.Text = "";

        }

        public int GetAccessLevel()
        {
            int nowLevel;
            if (user.LoginID == null)
                nowLevel = 2;
            else if(user.LoginID == "Delta")
                nowLevel = 0;
            else
                nowLevel = users[user.LoginID].Level;

            return nowLevel;
        }

        public string GetAccessLevelName()
        {
            string nowLevelName;

            if (user.LoginID == null)
                nowLevelName = level_name[2];
            else if (user.LoginID == "Delta")
                nowLevelName = level_name[0];
            else
                nowLevelName = level_name[users[user.LoginID].Level];

            return nowLevelName;
        }

        private UserInfo user = new UserInfo();　// 目前操作帳號

        public Form_PWInput(string _pwFileName="", string _Directory="")
        {
            InitializeComponent();

            tp = tabControl1.TabPages[1];      // 在這裡先保存，待後續進行顯示與隐藏
            tabControl1.TabPages.Remove(tp);   // 隐藏（删除）
            //tabControl1.TabPages.Insert(1, tp) ;// 顯示（插入）

            // 初始化權限名稱
            comboBox_NewLevel.Items.AddRange(level_name);
            
            // 設定檔名
            if(_pwFileName!="")
            {
                password_FileName = _pwFileName;
            }

            // 建立資料夾
            if(_Directory!="")
            {
                DirectoryName = _Directory;
               
                if (!Directory.Exists(DirectoryName))
                {
                    Directory.CreateDirectory(DirectoryName);
                }

                DirectoryName = DirectoryName + "\\";

            }

            // 設定完整檔名
            full_pw_FileName = DirectoryName +  password_FileName;

            TopMost = true;

        }

        #region 帳號密碼管理操作

        [Serializable]
        private class UserInfo
        {
            public string LoginID
            {
                get;
                set;
            }

            public string Pwd
            {
                get;
                set;
            }

            public int Level
            {
                get;
                set;
            }

        }

        private Dictionary<string, UserInfo> users = new Dictionary<string, UserInfo>();

        private void InitPW()
        {
            for(int i=0; i< default_user.Count(); i++)
            {
                textBox_NewAcc.Text = default_user[i];
                textBox_NewPW.Text  = default_password[i];
                comboBox_NewLevel.SelectedIndex = default_level[i];

                SavePW();
            }

            textBox_NewAcc.Text = "";
            textBox_NewPW.Text = "";
            comboBox_NewLevel.Text = "";

        }


        /// <summary>
        /// 保存帳號與密碼
        /// </summary>
        private void SavePW()
        {
            UserInfo user = new UserInfo();

            // 登陸時，如果沒有 xx.bin檔創建，有就打開             
            FileStream fs = new FileStream(full_pw_FileName, FileMode.OpenOrCreate);
            BinaryFormatter bf = new BinaryFormatter();

            // 保存在实体类属性中
            user.LoginID = textBox_NewAcc.Text.Trim();
            user.Pwd = textBox_NewPW.Text.Trim();
            user.Level = comboBox_NewLevel.SelectedIndex;

            // 選在集合中是否存在帳號
            if (users.ContainsKey(user.LoginID))
            {
                //如果有清掉
                users.Remove(user.LoginID);
            }

            //　加入帳號訊息至集合中
            users.Add(user.LoginID, user);

            //　寫入文件
            bf.Serialize(fs, users);

            //　關閉
            fs.Close();
        }

        /// <summary>
        /// 刪除選定的帳號和密碼
        /// </summary>
        private void DeletePW() 
        {
           
            UserInfo user = new UserInfo();

            // 登录时 如果没有Data.bin文件就创建、有就打开
            FileStream fs = new FileStream(full_pw_FileName, FileMode.OpenOrCreate);
            BinaryFormatter bf = new BinaryFormatter();
            
            // 保存在实体类属性中
            user.LoginID = textBox_NewAcc.Text.Trim();
            user.Pwd = textBox_NewPW.Text.Trim();

            //选在集合中是否存在用户名 
            if (users.ContainsKey(user.LoginID))
            {
                // 如果有清掉
                users.Remove(user.LoginID);
            }
           
            //写入文件
            bf.Serialize(fs, users);

            //关闭
            fs.Close();
        }

        /// <summary>
        /// 打開帳號和密码
        /// </summary>
        private void OpenPW() 
        {
            FileStream fs = new FileStream(full_pw_FileName, FileMode.OpenOrCreate);
           
            if (fs.Length > 0)
            {
                BinaryFormatter bf = new BinaryFormatter();
                fs.Position = 0;

                //读出存在Data.bin 里的用户信息
                users = bf.Deserialize(fs) as Dictionary<string, UserInfo>;
                
                //循环添加到Combox1
                comboBox_AccList.Items.Clear();
                comboBox_NowAllAcc.Items.Clear();
                foreach (UserInfo user in users.Values)
                {
                    comboBox_AccList.Items.Add(user.LoginID);
                    comboBox_NowAllAcc.Items.Add(user.LoginID);
                }

               
                // combox1 用户名默认选中第一个
                if (comboBox_AccList.Items.Count > 0)
                {
                    comboBox_AccList.SelectedIndex = 0; 
                    comboBox_NowAllAcc.SelectedIndex = 0;
                }
               
            }
                    
            fs.Close();
                                        
        }

        #endregion

        private void Form_PWInput_Load(object sender, EventArgs e)
        {
            //檢查檔案是否存在
            if (!File.Exists(full_pw_FileName))
            {
                InitPW();
            }
                       
            // 更新
            OpenPW();
            

        }

        private void button_Close_Click(object sender, EventArgs e)
        {
            Close();
        }

        
        private void button_OK_Click(object sender, EventArgs e)
        {
            if (comboBox_AccList.Text == "" || textBox_PW.Text == "")
            {
                MessageBox.Show("帳號、密碼不可空白!");

                return;
            }

            user.LoginID = comboBox_AccList.Text.Trim();
            user.Pwd = textBox_PW.Text.Trim();

            if(user.Pwd == users[user.LoginID].Pwd)
            {
                // 成功
                MessageBox.Show("密碼正確，操作權限已變更");

                // 權限更換成功觸發事件
                if (PasswordLevelChange != null)
                    PasswordLevelChange(sender, e);


                // 帳號密碼管理頁面顯示                
                comboBox_NowAllAcc_SelectedIndexChanged(sender,e);
                
                if(tabControl1.TabCount==2)
                {
                    tabControl1.TabPages.Remove(tp);　//　隐藏（删除）
                }

                if (users[user.LoginID].Level<=1) // 管理員權限
                {
                    tabControl1.TabPages.Insert(1, tp); // 顯示（插入）
                }
                else
                {
                    //tabControl1.TabPages.Remove(tp);　//　隐藏（删除）
                }

                // Close
                if(HideEnable==true)
                    Close();

            } 
            else
            {
                // 失敗
                MessageBox.Show("密碼錯誤，請重新確認");
                textBox_PW.Text = "";

            }

            

        }


        private void button_Save_Click(object sender, EventArgs e)
        {
            if(textBox_NewAcc.Text=="" || textBox_NewPW.Text =="")
            {                
                MessageBox.Show("帳號、密碼不可空白!");
                return;
            }

            SavePW();
            OpenPW();
        }

        private void comboBox_NowAllAcc_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_NewAcc.Text = comboBox_NowAllAcc.Text;
            textBox_NewPW.Text = users[textBox_NewAcc.Text].Pwd;            
            comboBox_NewLevel.SelectedIndex = users[textBox_NewAcc.Text].Level;

            // 保護帳號: Delta
            int nowLevel;
            if (user.LoginID != null)
                nowLevel = users[user.LoginID].Level;
            else
                nowLevel = users[textBox_NewAcc.Text].Level;

            // 調整權限
            if (nowLevel==0)        
            {
                textBox_NewPW.PasswordChar = '\0';
                comboBox_NewLevel.Enabled = (textBox_NewAcc.Text.Trim() == "Delta") ? false : true;
                button_Delete.Enabled = (textBox_NewAcc.Text.Trim() == "Delta") ? false : true;
            }

            if(nowLevel==1)            
            {
                if (textBox_NewAcc.Text.Trim() == "Delta")
                {
                    textBox_NewPW.PasswordChar = '*';
                    comboBox_NewLevel.Enabled = false;
                    button_Save.Enabled = false;
                    button_Delete.Enabled = false;

                }
                else
                {
                    textBox_NewPW.PasswordChar = '\0';
                    comboBox_NewLevel.Enabled = true;
                    button_Save.Enabled = true;
                    button_Delete.Enabled = user.LoginID==textBox_NewAcc.Text ? false : true ;

                }


            }

        }

       
        private void comboBox_AccList_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_PW.Text = "";
            textBox_PW.Focus();

            UserInfo user = new UserInfo();
            user.LoginID = comboBox_AccList.Text.Trim();
            textBox_Level.Text = level_name[users[user.LoginID].Level];

        }

        private void textBox_PW_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button_OK_Click(sender, e);
            }
        }

        private void Form_PWInput_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(HideEnable==true)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void button_Delete_Click(object sender, EventArgs e)
        {
            // 保護帳號: Delta
            if (textBox_NewAcc.Text.Trim() == "Delta")
            {
                MessageBox.Show("此為保護帳號，無法刪除!");
                return;
            }

            if (MessageBox.Show("您是否確定刪除此帳號？", "Warning!", MessageBoxButtons.YesNo) == DialogResult.No) return;

            DeletePW();
            OpenPW();
        }

        private void comboBox_NewLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            int nowLevel;
            if (user.LoginID != null)
                nowLevel = users[user.LoginID].Level;
            else
            {                
                return;
            }

            
            if(nowLevel==1)
            {
                comboBox_NewLevel.SelectedIndex = (comboBox_NewLevel.SelectedIndex < 1)&& (textBox_NewAcc.Text.Trim() != "Delta") ? 1 : comboBox_NewLevel.SelectedIndex;
            }

        }

        private void Form_PWInput_VisibleChanged(object sender, EventArgs e)
        {
            
        }

        private void tabPage_Login_Click(object sender, EventArgs e)
        {

        }
    }
}
