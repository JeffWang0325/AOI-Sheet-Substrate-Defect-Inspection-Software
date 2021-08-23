using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;
using System.IO.Ports;
using System.IO;

namespace PM_24084R
{
    public partial class Form_24084R : Form
    {
        
        const string STR_MODEL = " PM-24084R ";                 // Model: PM-24084R

        /* Command ********************************************************** */
        const byte CMD_READ_DATA = 0x80;                        // Command: Read Data (讀取資料)
        const byte CMD_SYNC_EN   = 0x70;                        // Command: SYNC Enable (同步致能)
        const byte CMD_L1_SET    = 0x91;                        // Command: L1 Set (L1設定)
        const byte CMD_L2_SET    = 0x92;                        // Command: L2 Set (L2設定)
        const byte CMD_L3_SET    = 0x93;                        // Command: L3 Set (L3設定)
        const byte CMD_L4_SET    = 0x94;                        // Command: L4 Set (L4設定)
        const byte CMD_L1_OUTPUT = 0xA1;                        // Command: L1 Output (L1輸出電流)
        const byte CMD_L2_OUTPUT = 0xA2;                        // Command: L2 Output (L2輸出電流)
        const byte CMD_L3_OUTPUT = 0xA3;                        // Command: L3 Output (L3輸出電流)
        const byte CMD_L4_OUTPUT = 0xA4;                        // Command: L4 Output (L4輸出電流)
        const byte CMD_L1_ON_OFF = 0xC1;                        // Command: L1 ON/OFF
        const byte CMD_L2_ON_OFF = 0xC2;                        // Command: L2 ON/OFF
        const byte CMD_L3_ON_OFF = 0xC3;                        // Command: L3 ON/OFF
        const byte CMD_L4_ON_OFF = 0xC4;                        // Command: L4 ON/OFF

        /* ACK_State ======================================================== */
        const byte AS_FAILED   = 0x80;                          // bit7 命令執行狀態    : 0=成功, 1=失敗
        const byte AS_SYNC_EN1 = 0x40;                          // bit6~5 ON/OFF同步致能: 00=ASYNC (全部個別控制)
        const byte AS_SYNC_EN0 = 0x20;                          //                        01=SYNC2 (L1~L2同步控制)
                                                                //                        10=SYNC3 (L1~L3同步控制)
                                                                //                        11=SYNC4 (L1~L4同步控制)
        const byte AS_PWR_ERR  = 0x10;                          // bit4 電源狀態        : 0=OK, 1=Error
        const byte AS_L4_ERR   = 0x08;                          // bit3 L4狀態          : 0=OK, 1=Error
        const byte AS_L3_ERR   = 0x04;                          // bit2 L3狀態          : 0=OK, 1=Error
        const byte AS_L2_ERR   = 0x02;                          // bit1 L2狀態          : 0=OK, 1=Error
        const byte AS_L1_ERR   = 0x01;                          // bit0 L1狀態          : 0=OK, 1=Error

        /* SYNC_EN ========================================================== */
        const byte ASYNC = 0;                                   // ASYNC (全部個別控制)
        const byte SYNC2 = 1;                                   // SYNC2 (L1~L2同步控制)
        const byte SYNC3 = 2;                                   // SYNC3 (L1~L3同步控制)
        const byte SYNC4 = 3;                                   // SYNC4 (L1~L4同步控制)

        /* Lx_Set =========================================================== */
        const byte SET_EXT_CTRL = 0x02;                         // bit1 外部ON/OFF控制: 0=Disable, 1=Enable
        const byte SET_ON_OFF   = 0x01;                         // bit0 ON/OFF        : 0=OFF, 1=ON

        /* ================================================================== */
        const ushort Lines_Max = 256;                           // 最大行數
        const byte ACK_TO = 10;                                 // 回應逾時, 約500ms (10 * 50ms)
        const byte AUTO_TMR = 60;                               // 自動詢問計時, 約3sec (60 * 50ms)

        /* ****************************************************************** */
        ushort Line_No = 0;                                     // 行數編號
        ushort Line_CTR = 0;                                    // 行數計數
        String[] PortNo;                                        // 埠編號
        bool PORT_Flag = false;                                 // PORT 旗標
        bool READ_Flag = false;                                 // READ 旗標
        byte ACK_TO_CTR = 0;                                    // 回應逾時計時
        byte ACK_State = 0x00;                                  // 回應狀態
        byte Auto_CTR = 0;                                      // 自動詢問計數
        byte Flash_CTR = 0;                                     // 閃爍計數
        int[] MaxCurrent = new int[] { 500, 600, 700, 800 };

        /* TX =============================================================== */
        bool TX_Flag = false;                                   // TX 旗標
        byte[] TX_Packet = new byte[7];                         // TX 封包
        byte TX_CMD = 0;                                        // TX 命令
        byte TX_CTR = 0;                                        // TX 傳送次數

        /* RX =============================================================== */
        bool RX_Flag = false;                                   // RX 旗標
        byte[] RX_Buffer = new byte[128];                       // RX 緩衝區
        byte[] RX_Packet = new byte[32];                        // RX 封包
        byte RX_Index = 0;                                      // RX 封包索引
        byte RX_Length = 0;                                     // RX 封包長度

        /* ================================================================== */
        bool SYNC_EN_CHG = false;                               // SYNC Enable 已改變
        byte SYNC_EN = ASYNC;                                   // SYNC Enable

        /* L1 =============================================================== */
        bool L1_Set_CHG = false;                                // L1 Set 已改變
        bool L1_Output_CHG = false;                             // L1 Output 已改變
        bool L1_ON_OFF_CHG = false;                             // L1 ON/OFF 已改變
        byte L1_Set = 0;                                        // L1 設定值
        int L1_Output = 0;                                      // L1 輸出值

        /* L2 =============================================================== */
        bool L2_Set_CHG = false;                                // L2 Set 已改變
        bool L2_Output_CHG = false;                             // L2 Output 已改變
        bool L2_ON_OFF_CHG = false;                             // L2 ON/OFF 已改變
        byte L2_Set = 0;                                        // L2 設定值
        int L2_Output = 0;                                      // L2 輸出值

        /* L3 =============================================================== */
        bool L3_Set_CHG = false;                                // L3 Set 已改變
        bool L3_Output_CHG = false;                             // L3 Output 已改變
        bool L3_ON_OFF_CHG = false;                             // L3 ON/OFF 已改變
        byte L3_Set = 0;                                        // L3 設定值
        int L3_Output = 0;                                      // L3 輸出值

        /* L4 =============================================================== */
        bool L4_Set_CHG = false;                                // L4 Set 已改變
        bool L4_Output_CHG = false;                             // L4 Output 已改變
        bool L4_ON_OFF_CHG = false;                             // L4 ON/OFF 已改變
        byte L4_Set = 0;                                        // L4 設定值
        int L4_Output = 0;                                      // L4 輸出值

        /* ****************************************************************** */

        private bool LightOffsetEnabled = false;
        private string m_strLightPath = "D:\\DSI\\Light\\";
        private int[] m_LOffset1 = new int[801];
        private int[] m_LOffset2 = new int[801];
        private int[] m_LOffset3 = new int[801];
        private int[] m_LOffset4 = new int[801];

        public Form_24084R()
        {
            InitializeComponent();
            serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
            ReadLightOffset();
            cbxLightOffSetEnabled.Checked = LightOffsetEnabled;
        }

        private void Form_24084R_Load(object sender, EventArgs e)
        {
            if (MaxLightValueIndexChagnge == false)
            {
                cbb_L1_MaxC.SelectedIndex = 0;                      // 500mA
                cbb_L2_MaxC.SelectedIndex = 0;                      // 500mA
                cbb_L3_MaxC.SelectedIndex = 0;                      // 500mA
                cbb_L4_MaxC.SelectedIndex = 0;                      // 500mA
            }
           

            COMPort_Check();                                    // 串列埠檢知

            this.TopMost = true;
            cbxLightOffSetEnabled.Checked = LightOffsetEnabled;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (PORT_Flag == true)                              // 串列埠已開啟?
            {
                if (RX_Flag == true)                            // 已接收封包?
                {
                    TX_Flag = RX_Flag = false;
                    TX_CTR = ACK_TO_CTR = 0;
                    if (READ_Flag == false)
                    {
                        Update_Lx();                            // 更新 (Lx參數)
                        Update_ACK();                           // 更新 (ACK)
                        READ_Flag = true;
                    }
                    else Update_ACK();                          // 更新 (ACK)
                }
                else
                {
                    if (TX_Flag == true)                        // 已傳送命令?
                    {
                        Check_RX_TO();                          // 檢查接收逾時
                    }
                    else
                    {
                        Check_TX_CMD();                         // 檢查傳送命令
                    }
                    Auto_Ask();                                 // 自動詢問
                }
            }
            ERR_Flash();                                        // 錯誤提示閃爍
        }

        #region 額外新增Function (For dll)

        private string nowComportName = "COM1";
        public void Connect(string comportName)
        {
            nowComportName = comportName;

            COMPort_Check();

            cbb_PortNo.Text = nowComportName;

            btn_Connect_Click(btn_Connect, null);
        }

        public bool IsConnect()
        {
            return PORT_Flag;
        }

        private bool MaxLightValueIndexChagnge  = false;
        public bool SetMaxLigthValueIndex(int _MaxL1ValId, int _MaxL2ValId, int _MaxL3ValId, int _MaxL4ValId)
        {
            bool flag = true;

            cbb_L1_MaxC.SelectedIndex = _MaxL1ValId;
            cbb_L2_MaxC.SelectedIndex = _MaxL2ValId;
            cbb_L3_MaxC.SelectedIndex = _MaxL3ValId;
            cbb_L4_MaxC.SelectedIndex = _MaxL4ValId;

            MaxLightValueIndexChagnge = true;

            return flag;
        }

        public bool GetMaxLightValueIndex(out int _MaxL1ValId, out int _MaxL2ValId, out int _MaxL3ValId, out int _MaxL4ValId)
        {
            bool flag = true;

            _MaxL1ValId = cbb_L1_MaxC.SelectedIndex;
            _MaxL2ValId = cbb_L2_MaxC.SelectedIndex;
            _MaxL3ValId = cbb_L3_MaxC.SelectedIndex;
            _MaxL4ValId = cbb_L4_MaxC.SelectedIndex;

            return flag;
        }

        public bool SetLightValue(int _L1Val, int _L2Val, int _L3Val, int _L4Val)
        {
            bool flag = true;

            tkb_L1_Output.Value = _L1Val;
            tkb_L2_Output.Value = _L2Val;
            tkb_L3_Output.Value = _L3Val;
            tkb_L4_Output.Value = _L4Val;

            return flag;
        }

        public bool GetLightValue(out int _L1Val, out int _L2Val, out int _L3Val, out int _L4Val)
        {
            bool flag = true;

            _L1Val = tkb_L1_Output.Value;
            _L2Val = tkb_L2_Output.Value;
            _L3Val = tkb_L3_Output.Value;
            _L4Val = tkb_L4_Output.Value;

            return flag;
        }
        
        public void SetLightOffset(bool Enabled)
        {
            LightOffsetEnabled = Enabled;
        }

        public bool GetLightOffSetEnabled()
        {
            return LightOffsetEnabled;
        }

        private void ReadLightOffset()
        {
            try
            {
                string strFn = "offset.ini";

                if (!Directory.Exists(m_strLightPath))  //check light ini Directory
                {
                    Directory.CreateDirectory(m_strLightPath);
                }

                string strLightINIfn = m_strLightPath + strFn;
                if (!File.Exists(strLightINIfn))
                {
                    return;
                }

                for (int i = 0; i < 801; i++)
                {
                    clsWinAPI.GetPrivateProfileString("Light1", i.ToString(), i.ToString(), clsWinAPI.GetString, 255, strLightINIfn);
                    int.TryParse(clsWinAPI.GetString.ToString(), out m_LOffset1[i]);
                    clsWinAPI.GetPrivateProfileString("Light2", i.ToString(), i.ToString(), clsWinAPI.GetString, 255, strLightINIfn);
                    int.TryParse(clsWinAPI.GetString.ToString(), out m_LOffset2[i]);
                    clsWinAPI.GetPrivateProfileString("Light3", i.ToString(), i.ToString(), clsWinAPI.GetString, 255, strLightINIfn);
                    int.TryParse(clsWinAPI.GetString.ToString(), out m_LOffset3[i]);
                    clsWinAPI.GetPrivateProfileString("Light4", i.ToString(), i.ToString(), clsWinAPI.GetString, 255, strLightINIfn);
                    int.TryParse(clsWinAPI.GetString.ToString(), out m_LOffset4[i]);
                }
            }
            catch (System.Exception ex)
            {

            }
        }

        public void OpenLightValue(int _L1Val, int _L2Val, int _L3Val, int _L4Val)
        {
            ReadLightOffset();
            if (!LightOffsetEnabled)
            {
                tkb_L1_Output.Value = _L1Val;
                tkb_L2_Output.Value = _L2Val;
                tkb_L3_Output.Value = _L3Val;
                tkb_L4_Output.Value = _L4Val;
            }
            else
            {
                if (m_LOffset1[_L1Val] != 0)
                    tkb_L1_Output.Value = m_LOffset1[_L1Val];
                else
                    tkb_L1_Output.Value = _L1Val;

                if (m_LOffset2[_L2Val] != 0)
                    tkb_L2_Output.Value = m_LOffset2[_L2Val];
                else
                    tkb_L2_Output.Value = _L2Val;

                if (m_LOffset3[_L3Val] != 0)
                    tkb_L3_Output.Value = m_LOffset3[_L3Val];
                else
                    tkb_L3_Output.Value = _L3Val;

                if (m_LOffset4[_L4Val] != 0)
                    tkb_L4_Output.Value = m_LOffset4[_L4Val];
                else
                    tkb_L4_Output.Value = _L4Val;
            }
        }

        /// <summary>
        /// 取得目前已連接之 COM Port
        /// </summary>
        /// <returns></returns>
        public string Get_PortName() // (20191102) Jeff Revised!
        {
            if (serialPort.IsOpen)
                return serialPort.PortName;
            else
                return "";
        }

        #endregion

        /* ================================================================== */

        private void Check_RX_TO()                              // 檢查接收逾時
        {
            if (++ACK_TO_CTR >= ACK_TO)
            {
                ACK_TO_CTR = 0;
                if (TX_CTR > 2)
                {
                    Dsp_String(Color.Red, STR_MODEL + "逾時未回應 !\n");
                    COMPort_Disconnect();                       // 串列埠離線
                    SystemSounds.Beep.Play();
                }
                else
                {
                    switch (TX_CMD)
                    {
                        case CMD_READ_DATA: TX_Read_Data(); break;  // 傳送: Read Data
                        case CMD_SYNC_EN  : TX_SYNC_EN(); break;    // 傳送: SYNC Enable
                        case CMD_L1_SET   : TX_L1_Set(); break;     // 傳送: L1 Set
                        case CMD_L2_SET   : TX_L2_Set(); break;     // 傳送: L2 Set
                        case CMD_L3_SET   : TX_L3_Set(); break;     // 傳送: L3 Set
                        case CMD_L4_SET   : TX_L4_Set(); break;     // 傳送: L4 Set
                        case CMD_L1_OUTPUT: TX_L1_Output(); break;  // 傳送: L1 Output
                        case CMD_L2_OUTPUT: TX_L2_Output(); break;  // 傳送: L2 Output
                        case CMD_L3_OUTPUT: TX_L3_Output(); break;  // 傳送: L3 Output
                        case CMD_L4_OUTPUT: TX_L4_Output(); break;  // 傳送: L4 Output
                        case CMD_L1_ON_OFF: TX_L1_ON_OFF(); break;  // 傳送: L1 ON/OFF
                        case CMD_L2_ON_OFF: TX_L2_ON_OFF(); break;  // 傳送: L2 ON/OFF
                        case CMD_L3_ON_OFF: TX_L3_ON_OFF(); break;  // 傳送: L3 ON/OFF
                        case CMD_L4_ON_OFF: TX_L4_ON_OFF(); break;  // 傳送: L4 ON/OFF
                    }
                }
            }
        }

        private void Check_TX_CMD()                             // 檢查傳送命令
        {
            if (SYNC_EN_CHG == true) { TX_SYNC_EN(); return; }
            if (L1_Set_CHG == true) { TX_L1_Set(); return; }
            if (L2_Set_CHG == true) { TX_L2_Set(); return; }
            if (L3_Set_CHG == true) { TX_L3_Set(); return; }
            if (L4_Set_CHG == true) { TX_L4_Set(); return; }
            if (L1_Output_CHG == true) { TX_L1_Output(); return; }
            if (L2_Output_CHG == true) { TX_L2_Output(); return; }
            if (L3_Output_CHG == true) { TX_L3_Output(); return; }
            if (L4_Output_CHG == true) { TX_L4_Output(); return; }
            if (L1_ON_OFF_CHG == true) { TX_L1_ON_OFF(); return; }
            if (L2_ON_OFF_CHG == true) { TX_L2_ON_OFF(); return; }
            if (L3_ON_OFF_CHG == true) { TX_L3_ON_OFF(); return; }
            if (L4_ON_OFF_CHG == true) { TX_L4_ON_OFF(); return; }
        }

        private void Auto_Ask()                                 // 自動詢問
        {
            if (++Auto_CTR >= AUTO_TMR) TX_Read_Data();         // 傳送: Read Data
            if (cbb_L1_MaxC.DroppedDown == true) Auto_CTR = 0;
            if (cbb_L2_MaxC.DroppedDown == true) Auto_CTR = 0;
            if (cbb_L3_MaxC.DroppedDown == true) Auto_CTR = 0;
            if (cbb_L4_MaxC.DroppedDown == true) Auto_CTR = 0;
            if (nud_L1_Output.Focused == true) Auto_CTR = 0;
            if (nud_L2_Output.Focused == true) Auto_CTR = 0;
            if (nud_L3_Output.Focused == true) Auto_CTR = 0;
            if (nud_L4_Output.Focused == true) Auto_CTR = 0;
            if (cbb_SYNC_EN.DroppedDown == true) Auto_CTR = 0;
        }

        private void ERR_Flash()                                // 錯誤提示閃爍
        {
            if (Flash_CTR == 0)
            {
                if ((ACK_State & AS_PWR_ERR) == AS_PWR_ERR) lb_PWR_ERR.BackColor = Color.Red;
                if ((ACK_State & AS_L4_ERR) == AS_L4_ERR) lb_L4.BackColor = Color.Red;
                if ((ACK_State & AS_L3_ERR) == AS_L3_ERR) lb_L3.BackColor = Color.Red;
                if ((ACK_State & AS_L2_ERR) == AS_L2_ERR) lb_L2.BackColor = Color.Red;
                if ((ACK_State & AS_L1_ERR) == AS_L1_ERR) lb_L1.BackColor = Color.Red;
            }
            if (Flash_CTR == 5)
            {
                if ((ACK_State & AS_PWR_ERR) == AS_PWR_ERR) lb_PWR_ERR.BackColor = Color.Black;
                if ((ACK_State & AS_L4_ERR) == AS_L4_ERR) lb_L4.BackColor = Color.Black;
                if ((ACK_State & AS_L3_ERR) == AS_L3_ERR) lb_L3.BackColor = Color.Black;
                if ((ACK_State & AS_L2_ERR) == AS_L2_ERR) lb_L2.BackColor = Color.Black;
                if ((ACK_State & AS_L1_ERR) == AS_L1_ERR) lb_L1.BackColor = Color.Black;
            }
            if (++Flash_CTR > 9) Flash_CTR = 0;
        }

        /* ================================================================== */

        private void Update_Lx()                                // 更新 (Lx參數)
        {
            Dsp_RX_Packet(SystemColors.WindowText, 17);

            byte v = 0;
            if ((RX_Packet[3] & AS_SYNC_EN1) == AS_SYNC_EN1) v |= 2;
            if ((RX_Packet[3] & AS_SYNC_EN0) == AS_SYNC_EN0) v |= 1;
            cbb_SYNC_EN.SelectedIndex = SYNC_EN = v;

            Update_L1();                                        // 更新 (L1)
            Update_L2();                                        // 更新 (L2)
            Update_L3();                                        // 更新 (L3)
            Update_L4();                                        // 更新 (L4)
        }

        private void Update_L1()                                // 更新 (L1)
        {
            L1_Set = (byte)(RX_Packet[4] & 0x33);
            if ((L1_Set & SET_ON_OFF) == 0)                     // L1 ON/OFF ?
            {
                pn_L1_LED.BackColor = Color.Black;
                btn_L1.Text = "ON";
            }
            else
            {
                pn_L1_LED.BackColor = Color.Green;
                btn_L1.Text = "OFF";
            }
            if ((L1_Set & SET_EXT_CTRL) == 0)                   // L1外部ON/OFF控制 ?
            {
                ckb_L1_EXT.Checked = false;
                pn_L1_LED.Visible = btn_L1.Visible = true;
            }
            else
            {
                ckb_L1_EXT.Checked = true;
                pn_L1_LED.Visible = btn_L1.Visible = false;
            }

            cbb_L1_MaxC.SelectedIndex = (L1_Set >> 4) & 0x03;
            int MaxC = MaxCurrent[cbb_L1_MaxC.SelectedIndex];
            int Output = RX_Packet[5];
            Output <<= 8;
            Output |= RX_Packet[6];
            nud_L1_Output.Maximum = tkb_L1_Output.Maximum = MaxC;
            nud_L1_Output.Value = tkb_L1_Output.Value = Output;
            tkb_L1_Output.TickFrequency = MaxC / 10;
            lb_L1_PCV.Text = String.Format("{0:F1}", (float)(Output * 100) / MaxC);
            L1_Output = Output;
        }

        private void Update_L2()                                // 更新 (L2)
        {
            L2_Set = (byte)(RX_Packet[7] & 0x33);
            if ((L2_Set & SET_ON_OFF) == 0)                     // L2 ON/OFF ?
            {
                pn_L2_LED.BackColor = Color.Black;
                btn_L2.Text = "ON";
            }
            else
            {
                pn_L2_LED.BackColor = Color.Green;
                btn_L2.Text = "OFF";
            }
            if ((L2_Set & SET_EXT_CTRL) == 0)                   // L2外部ON/OFF控制 ?
            {
                ckb_L2_EXT.Checked = false;
                pn_L2_LED.Visible = btn_L2.Visible = true;
            }
            else
            {
                ckb_L2_EXT.Checked = true;
                pn_L2_LED.Visible = btn_L2.Visible = false;
            }

            cbb_L2_MaxC.SelectedIndex = (L2_Set >> 4) & 0x03;
            int MaxC = MaxCurrent[cbb_L2_MaxC.SelectedIndex];
            int Output = RX_Packet[8];
            Output <<= 8;
            Output |= RX_Packet[9];
            nud_L2_Output.Maximum = tkb_L2_Output.Maximum = MaxC;
            nud_L2_Output.Value = tkb_L2_Output.Value = Output;
            tkb_L2_Output.TickFrequency = MaxC / 10;
            lb_L2_PCV.Text = String.Format("{0:F1}", (float)(Output * 100) / MaxC);
            L2_Output = Output;
        }

        private void Update_L3()                                // 更新 (L3)
        {
            L3_Set = (byte)(RX_Packet[10] & 0x33);
            if ((L3_Set & SET_ON_OFF) == 0)                     // L3 ON/OFF ?
            {
                pn_L3_LED.BackColor = Color.Black;
                btn_L3.Text = "ON";
            }
            else
            {
                pn_L3_LED.BackColor = Color.Green;
                btn_L3.Text = "OFF";
            }
            if ((L3_Set & SET_EXT_CTRL) == 0)                   // L3外部ON/OFF控制 ?
            {
                ckb_L3_EXT.Checked = false;
                pn_L3_LED.Visible = btn_L3.Visible = true;
            }
            else
            {
                ckb_L3_EXT.Checked = true;
                pn_L3_LED.Visible = btn_L3.Visible = false;
            }

            cbb_L3_MaxC.SelectedIndex = (L3_Set >> 4) & 0x03;
            int MaxC = MaxCurrent[cbb_L3_MaxC.SelectedIndex];
            int Output = RX_Packet[11];
            Output <<= 8;
            Output |= RX_Packet[12];
            nud_L3_Output.Maximum = tkb_L3_Output.Maximum = MaxC;
            nud_L3_Output.Value = tkb_L3_Output.Value = Output;
            tkb_L3_Output.TickFrequency = MaxC / 10;
            lb_L3_PCV.Text = String.Format("{0:F1}", (float)(Output * 100) / MaxC);
            L3_Output = Output;
        }

        private void Update_L4()                                // 更新 (L4)
        {
            L4_Set = (byte)(RX_Packet[13] & 0x33);
            if ((L4_Set & SET_ON_OFF) == 0)                     // L4 ON/OFF ?
            {
                pn_L4_LED.BackColor = Color.Black;
                btn_L4.Text = "ON";
            }
            else
            {
                pn_L4_LED.BackColor = Color.Green;
                btn_L4.Text = "OFF";
            }
            if ((L4_Set & SET_EXT_CTRL) == 0)                   // L4外部ON/OFF控制 ?
            {
                ckb_L4_EXT.Checked = false;
                pn_L4_LED.Visible = btn_L4.Visible = true;
            }
            else
            {
                ckb_L4_EXT.Checked = true;
                pn_L4_LED.Visible = btn_L4.Visible = false;
            }

            cbb_L4_MaxC.SelectedIndex = (L4_Set >> 4) & 0x03;
            int MaxC = MaxCurrent[cbb_L4_MaxC.SelectedIndex];
            int Output = RX_Packet[14];
            Output <<= 8;
            Output |= RX_Packet[15];
            nud_L4_Output.Maximum = tkb_L4_Output.Maximum = MaxC;
            nud_L4_Output.Value = tkb_L4_Output.Value = Output;
            tkb_L4_Output.TickFrequency = MaxC / 10;
            lb_L4_PCV.Text = String.Format("{0:F1}", (float)(Output * 100) / MaxC);
            L4_Output = Output;
        }

        private void Update_ACK()                               // 更新 (ACK)
        {
            if (RX_Length == 5) Dsp_RX_Packet(SystemColors.WindowText, 5);

            ACK_State = RX_Packet[3];
            if ((ACK_State & 0x9F) != 0) SystemSounds.Beep.Play();  // AS_FAILED | AS_PWR_ERR | AS_L4_ERR | AS_L3_ERR | AS_L2_ERR | AS_L1_ERR

            if ((ACK_State & AS_FAILED) == AS_FAILED) Dsp_String(Color.Red, STR_MODEL + "命令執行失敗 !\n");

            if ((ACK_State & AS_L1_ERR) == AS_L1_ERR)
            {
                lb_L1.Text = "L1 Error";
                Dsp_String(Color.Red, STR_MODEL + "L1錯誤 !\n");
            }
            else
            {
                lb_L1.Text = "L1";
                lb_L1.BackColor = Color.Black;
            }

            if ((ACK_State & AS_L2_ERR) == AS_L2_ERR)
            {
                lb_L2.Text = "L2 Error";
                Dsp_String(Color.Red, STR_MODEL + "L2錯誤 !\n");
            }
            else
            {
                lb_L2.Text = "L2";
                lb_L2.BackColor = Color.Black;
            }

            if ((ACK_State & AS_L3_ERR) == AS_L3_ERR)
            {
                lb_L3.Text = "L3 Error";
                Dsp_String(Color.Red, STR_MODEL + "L3錯誤 !\n");
            }
            else
            {
                lb_L3.Text = "L3";
                lb_L3.BackColor = Color.Black;
            }

            if ((ACK_State & AS_L4_ERR) == AS_L4_ERR)
            {
                lb_L4.Text = "L4 Error";
                Dsp_String(Color.Red, STR_MODEL + "L4錯誤 !\n");
            }
            else
            {
                lb_L4.Text = "L4";
                lb_L4.BackColor = Color.Black;
            }

            if ((ACK_State & AS_PWR_ERR) == AS_PWR_ERR)
            {
                lb_PWR_ERR.ForeColor = Color.White;
                Dsp_String(Color.Red, STR_MODEL + "電源過低 !\n");
                gpb_ON_OFF.Enabled = false;
                gpb_L1.Enabled = gpb_L2.Enabled = false;
                gpb_L3.Enabled = gpb_L4.Enabled = false;
            }
            else
            {
                lb_PWR_ERR.ForeColor = Color.Gray;
                lb_PWR_ERR.BackColor = Color.Black;
                gpb_ON_OFF.Enabled = true;
                if ((ACK_State & AS_L1_ERR) == 0) gpb_L1.Enabled = true;
                else gpb_L1.Enabled = false;
                if ((ACK_State & AS_L2_ERR) == 0) gpb_L2.Enabled = true;
                else gpb_L2.Enabled = false;
                if ((ACK_State & AS_L3_ERR) == 0) gpb_L3.Enabled = true;
                else gpb_L3.Enabled = false;
                if ((ACK_State & AS_L4_ERR) == 0) gpb_L4.Enabled = true;
                else gpb_L4.Enabled = false;
            }
        }

        /* RichTextBox 顯示 ************************************************* */

        private void Dsp_String(Color text_color, string str)   // 顯示字串
        {
            if (++Line_CTR > Lines_Max) { Line_CTR = 0; richTextBox.Clear(); }

            richTextBox.SelectionStart = richTextBox.Text.Length;
            richTextBox.ScrollToCaret();
            richTextBox.SelectionBackColor = Color.Gainsboro;
            richTextBox.SelectionColor = SystemColors.WindowText;
            richTextBox.AppendText(String.Format("{0:X4}", Line_No++));
            richTextBox.SelectionBackColor = SystemColors.Window;
            richTextBox.SelectionColor = text_color;
            richTextBox.AppendText(str);
            richTextBox.Select();
        }

        private void Dsp_TX_Packet(Color text_color, byte length, byte counter)  // 顯示 TX 封包
        {
            if (++Line_CTR > Lines_Max) { Line_CTR = 0; richTextBox.Clear(); }

            richTextBox.SelectionStart = richTextBox.Text.Length;
            richTextBox.ScrollToCaret();
            richTextBox.SelectionBackColor = Color.Gainsboro;
            richTextBox.SelectionColor = SystemColors.WindowText;
            richTextBox.AppendText(String.Format("{0:X4}", Line_No++));
            richTextBox.SelectionBackColor = SystemColors.Window;
            richTextBox.SelectionColor = text_color;

            string str = " TX:";
            for (byte i = 0; i < length; i++) str += String.Format(" {0:X2}", TX_Packet[i]);
            str += " (" + Convert.ToString(counter) + ")\n";
            richTextBox.AppendText(str);
            richTextBox.Select();
        }

        private void Dsp_RX_Packet(Color text_color, byte length)  // 顯示 RX 封包
        {
            if (++Line_CTR > Lines_Max) { Line_CTR = 0; richTextBox.Clear(); }

            richTextBox.SelectionStart = richTextBox.Text.Length;
            richTextBox.ScrollToCaret();
            richTextBox.SelectionBackColor = Color.Gainsboro;
            richTextBox.SelectionColor = SystemColors.WindowText;
            richTextBox.AppendText(String.Format("{0:X4}", Line_No++));
            richTextBox.SelectionBackColor = SystemColors.Window;
            richTextBox.SelectionColor = text_color;

            string str = " RX:";
            for (byte i = 0; i < length; i++) str += String.Format(" {0:X2}", RX_Packet[i]);
            str += "\n";
            richTextBox.AppendText(str);
            richTextBox.Select();
        }

        /* 串列埠 *********************************************************** */

        private void cbb_PortNo_Click(object sender, EventArgs e)
        {
            COMPort_Check();                                    // 串列埠檢知
        }

        private void btn_Connect_Click(object sender, EventArgs e)
        {
            bool state = false;

            if (serialPort.IsOpen)                              // 串列埠是否已開啟?
            {
                COMPort_Disconnect();                           // 串列埠離線
            }
            else
            {
                PortNo = SerialPort.GetPortNames();             // 取得存在的 COM Port.
                if (PortNo.Length >= 1)
                {
                    if (cbb_PortNo.Text == "")
                    {
                        Dsp_String(Color.Red, " 未選擇串列埠 !\n");
                        SystemSounds.Beep.Play();
                    }
                    else
                    {
                        foreach (string port in PortNo)
                        {
                            if (port == cbb_PortNo.Text) { state = true; break; }
                        }
                        if (state == true)
                        {
                            if (serialPort.IsOpen)
                            {
                                COMPort_Disconnect();           // 串列埠離線
                            }
                            else
                            {
                                COMPort_Connect();              // 串列埠連線
                                if (PORT_Flag == true)
                                {
                                    TX_Read_Data();             // 傳送: Read Data
                                }
                            }
                        }
                    }
                }
                else
                {
                    cbb_PortNo.Items.Clear();                   // 清除 ComboBox.Items 的內容.
                    Dsp_String(Color.Red, " 未發現任何串列埠 !\n");
                    SystemSounds.Beep.Play();
                }
            }
        }

        private void COMPort_Check()                            // 串列埠檢知
        {
            string Port_No = cbb_PortNo.Text;

            PortNo = SerialPort.GetPortNames();                 // 取得存在的 COM Port.
            cbb_PortNo.Items.Clear();                           // 清除 ComboBox.Items 的內容.
            if (PortNo.Length >= 1)
            {
                foreach (string port in PortNo)                 // 將找到的現有 COM Port 加入 ComboBox.Items
                {
                    cbb_PortNo.Items.Add(port);
                    if (port == Port_No) cbb_PortNo.Text = Port_No;
                }
                if (cbb_PortNo.Text == "")
                {
                    cbb_PortNo.Text = PortNo[0];                // ComboBox.Text 先顯示一個現存的 COM Port.
                }
            }
            else
            {
                Dsp_String(Color.Red, " 未發現任何串列埠 !\n");
                SystemSounds.Beep.Play();
            }
        }

        private void COMPort_Connect()                          // 串列埠連線
        {
            TX_Flag = RX_Flag = PORT_Flag = false;
            TX_CMD = TX_CTR = RX_Index = 0;
            SYNC_EN_CHG = false;
            L1_Set_CHG = L1_Output_CHG = L1_ON_OFF_CHG = false;
            L2_Set_CHG = L2_Output_CHG = L2_ON_OFF_CHG = false;
            L3_Set_CHG = L3_Output_CHG = L3_ON_OFF_CHG = false;
            L4_Set_CHG = L4_Output_CHG = L4_ON_OFF_CHG = false;
            serialPort.PortName = cbb_PortNo.Text;

            try
            {
                serialPort.Open();                              // 開啟新的序列埠連線.
                serialPort.DiscardInBuffer();                   // 捨棄序列驅動程式接收緩衝區的資料.
                serialPort.DiscardOutBuffer();                  // 捨棄序列驅動程式傳輸緩衝區的資料.
                cbb_PortNo.Enabled = false;
                btn_Connect.Text = "Disconnect";
                tss_Status.Text = "Connect";
                Dsp_String(Color.Magenta, " " + cbb_PortNo.Text + " connect.\n");
                PORT_Flag = true;
            }
            catch (Exception)
            {
                Dsp_String(Color.Red, " 開啟串列埠失敗 !\n");
                SystemSounds.Beep.Play();
            }
        }

        private void COMPort_Disconnect()                       // 串列埠離線
        {
            TX_Flag = RX_Flag = PORT_Flag = false;
            TX_CMD = TX_CTR = RX_Index = ACK_State = 0;
            SYNC_EN_CHG = false;
            L1_Set_CHG = L1_Output_CHG = L1_ON_OFF_CHG = false;
            L2_Set_CHG = L2_Output_CHG = L2_ON_OFF_CHG = false;
            L3_Set_CHG = L3_Output_CHG = L3_ON_OFF_CHG = false;
            L4_Set_CHG = L4_Output_CHG = L4_ON_OFF_CHG = false;

            try
            {
                serialPort.Close();                             // 關閉連接埠連線.
                Dsp_String(Color.Magenta, " " + cbb_PortNo.Text + " disconnect.\n");
            }
            catch (Exception)
            {
                COMPort_Check();                                // 串列埠檢知
            }

            cbb_PortNo.Enabled = true;
            btn_Connect.Text = "Connect";
            tss_Status.Text = "Disconnect";
            lb_L1.Text = "L1";
            lb_L2.Text = "L2";
            lb_L3.Text = "L3";
            lb_L4.Text = "L4";
            lb_L1.BackColor = lb_L2.BackColor = Color.Black;
            lb_L3.BackColor = lb_L4.BackColor = Color.Black;
            lb_PWR_ERR.ForeColor = Color.Gray;
            lb_PWR_ERR.BackColor = Color.Black;
            gpb_ON_OFF.Enabled = false;
            gpb_L1.Enabled = gpb_L2.Enabled = false;
            gpb_L3.Enabled = gpb_L4.Enabled = false;
        }

        /* 串列埠傳送 ******************************************************* */

        private void TX_Read_Data()                             // 傳送: Read Data
        {
            byte i, sum;

            if (TX_Flag == false)
            {
                TX_Packet[0] = 0x4D;                            // Header 'M'
                TX_Packet[1] = 0x53;                            // Header 'S'
                TX_Packet[2] = 5;                               // Packet Length
                TX_Packet[3] = CMD_READ_DATA;                   // Command
                for (i = sum = 0; i < 4; i++) sum += TX_Packet[i];
                TX_Packet[4] = sum;                             // Checksum
                TX_CMD = CMD_READ_DATA;
                TX_CTR = 0;
                TX_Flag = true;
                READ_Flag = false;
            }

            if (TX_CTR == 0) Dsp_TX_Packet(Color.Black, 5, ++TX_CTR);
            else Dsp_TX_Packet(Color.Magenta, 5, ++TX_CTR);
            ACK_TO_CTR = Auto_CTR = 0;

            try
            {
                serialPort.Write(TX_Packet, 0, 5);
            }
            catch (Exception)
            {
                Dsp_String(Color.Red, " 傳送封包 (Read Data), 發生不可預期的錯誤 !\n");
                SystemSounds.Beep.Play();
                COMPort_Disconnect();                           // 串列埠離線
            }
        }

        private void TX_SYNC_EN()                               // 傳送: SYNC Enable
        {
            byte i, sum;

            if (TX_Flag == false)
            {
                TX_Packet[0] = 0x4D;                            // Header 'M'
                TX_Packet[1] = 0x53;                            // Header 'S'
                TX_Packet[2] = 6;                               // Packet Length
                TX_Packet[3] = CMD_SYNC_EN;                     // Command
                TX_Packet[4] = SYNC_EN;
                for (i = sum = 0; i < 5; i++) sum += TX_Packet[i];
                TX_Packet[5] = sum;                             // Checksum
                TX_CMD = CMD_SYNC_EN;
                TX_CTR = 0;
                TX_Flag = true;
                SYNC_EN_CHG = false;
            }

            if (TX_CTR == 0) Dsp_TX_Packet(Color.Black, 6, ++TX_CTR);
            else Dsp_TX_Packet(Color.Magenta, 6, ++TX_CTR);
            ACK_TO_CTR = Auto_CTR = 0;

            try
            {
                serialPort.Write(TX_Packet, 0, 6);
            }
            catch (Exception)
            {
                Dsp_String(Color.Red, " 傳送封包 (SYNC Enable), 發生不可預期的錯誤 !\n");
                SystemSounds.Beep.Play();
                COMPort_Disconnect();                           // 串列埠離線
            }
        }

        private void TX_L1_Set()                                // 傳送: L1 Set
        {
            byte i, sum;

            if (TX_Flag == false)
            {
                TX_Packet[0] = 0x4D;                            // Header 'M'
                TX_Packet[1] = 0x53;                            // Header 'S'
                TX_Packet[2] = 6;                               // Packet Length
                TX_Packet[3] = CMD_L1_SET;                      // Command
                TX_Packet[4] = L1_Set;                          // L1 設定值
                for (i = sum = 0; i < 5; i++) sum += TX_Packet[i];
                TX_Packet[5] = sum;                             // Checksum
                TX_CMD = CMD_L1_SET;
                TX_CTR = 0;
                TX_Flag = true;
                L1_Set_CHG = false;
            }

            if (TX_CTR == 0) Dsp_TX_Packet(Color.Maroon, 6, ++TX_CTR);
            else Dsp_TX_Packet(Color.Magenta, 6, ++TX_CTR);
            ACK_TO_CTR = Auto_CTR = 0;

            try
            {
                serialPort.Write(TX_Packet, 0, 6);
            }
            catch (Exception)
            {
                Dsp_String(Color.Red, " 傳送封包 (L1 Set), 發生不可預期的錯誤 !\n");
                SystemSounds.Beep.Play();
                COMPort_Disconnect();                           // 串列埠離線
            }
        }

        private void TX_L2_Set()                                // 傳送: L2 Set
        {
            byte i, sum;

            if (TX_Flag == false)
            {
                TX_Packet[0] = 0x4D;                            // Header 'M'
                TX_Packet[1] = 0x53;                            // Header 'S'
                TX_Packet[2] = 6;                               // Packet Length
                TX_Packet[3] = CMD_L2_SET;                      // Command
                TX_Packet[4] = L2_Set;                          // L2 設定值
                for (i = sum = 0; i < 5; i++) sum += TX_Packet[i];
                TX_Packet[5] = sum;                             // Checksum
                TX_CMD = CMD_L2_SET;
                TX_CTR = 0;
                TX_Flag = true;
                L2_Set_CHG = false;
            }

            if (TX_CTR == 0) Dsp_TX_Packet(Color.Maroon, 6, ++TX_CTR);
            else Dsp_TX_Packet(Color.Magenta, 6, ++TX_CTR);
            ACK_TO_CTR = Auto_CTR = 0;

            try
            {
                serialPort.Write(TX_Packet, 0, 6);
            }
            catch (Exception)
            {
                Dsp_String(Color.Red, " 傳送封包 (L2 Set), 發生不可預期的錯誤 !\n");
                SystemSounds.Beep.Play();
                COMPort_Disconnect();                           // 串列埠離線
            }
        }

        private void TX_L3_Set()                                // 傳送: L3 Set
        {
            byte i, sum;

            if (TX_Flag == false)
            {
                TX_Packet[0] = 0x4D;                            // Header 'M'
                TX_Packet[1] = 0x53;                            // Header 'S'
                TX_Packet[2] = 6;                               // Packet Length
                TX_Packet[3] = CMD_L3_SET;                      // Command
                TX_Packet[4] = L3_Set;                          // L3 設定值
                for (i = sum = 0; i < 5; i++) sum += TX_Packet[i];
                TX_Packet[5] = sum;                             // Checksum
                TX_CMD = CMD_L3_SET;
                TX_CTR = 0;
                TX_Flag = true;
                L3_Set_CHG = false;
            }

            if (TX_CTR == 0) Dsp_TX_Packet(Color.Maroon, 6, ++TX_CTR);
            else Dsp_TX_Packet(Color.Magenta, 6, ++TX_CTR);
            ACK_TO_CTR = Auto_CTR = 0;

            try
            {
                serialPort.Write(TX_Packet, 0, 6);
            }
            catch (Exception)
            {
                Dsp_String(Color.Red, " 傳送封包 (L3 Set), 發生不可預期的錯誤 !\n");
                SystemSounds.Beep.Play();
                COMPort_Disconnect();                           // 串列埠離線
            }
        }

        private void TX_L4_Set()                                // 傳送: L4 Set
        {
            byte i, sum;

            if (TX_Flag == false)
            {
                TX_Packet[0] = 0x4D;                            // Header 'M'
                TX_Packet[1] = 0x53;                            // Header 'S'
                TX_Packet[2] = 6;                               // Packet Length
                TX_Packet[3] = CMD_L4_SET;                      // Command
                TX_Packet[4] = L4_Set;                          // L4 設定值
                for (i = sum = 0; i < 5; i++) sum += TX_Packet[i];
                TX_Packet[5] = sum;                             // Checksum
                TX_CMD = CMD_L4_SET;
                TX_CTR = 0;
                TX_Flag = true;
                L4_Set_CHG = false;
            }

            if (TX_CTR == 0) Dsp_TX_Packet(Color.Maroon, 6, ++TX_CTR);
            else Dsp_TX_Packet(Color.Magenta, 6, ++TX_CTR);
            ACK_TO_CTR = Auto_CTR = 0;

            try
            {
                serialPort.Write(TX_Packet, 0, 6);
            }
            catch (Exception)
            {
                Dsp_String(Color.Red, " 傳送封包 (L4 Set), 發生不可預期的錯誤 !\n");
                SystemSounds.Beep.Play();
                COMPort_Disconnect();                           // 串列埠離線
            }
        }

        private void TX_L1_Output()                             // 傳送: L1 Output
        {
            byte i, sum;

            if (TX_Flag == false)
            {
                TX_Packet[0] = 0x4D;                            // Header 'M'
                TX_Packet[1] = 0x53;                            // Header 'S'
                TX_Packet[2] = 7;                               // Packet Length
                TX_Packet[3] = CMD_L1_OUTPUT;                   // Command
                TX_Packet[4] = (byte)(L1_Output >> 8);          // L1 Output, HB
                TX_Packet[5] = (byte)L1_Output;                 // L1 Output, LB
                for (i = sum = 0; i < 6; i++) sum += TX_Packet[i];
                TX_Packet[6] = sum;                             // Checksum
                TX_CMD = CMD_L1_OUTPUT;
                TX_CTR = 0;
                TX_Flag = true;
                L1_Output_CHG = false;
            }

            if (TX_CTR == 0) Dsp_TX_Packet(Color.Blue, 7, ++TX_CTR);
            else Dsp_TX_Packet(Color.Magenta, 7, ++TX_CTR);
            ACK_TO_CTR = Auto_CTR = 0;

            try
            {
                serialPort.Write(TX_Packet, 0, 7);
            }
            catch (Exception)
            {
                Dsp_String(Color.Red, " 傳送封包 (L1 Output), 發生不可預期的錯誤 !\n");
                SystemSounds.Beep.Play();
                COMPort_Disconnect();                           // 串列埠離線
            }
        }

        private void TX_L2_Output()                             // 傳送: L2 Output
        {
            byte i, sum;

            if (TX_Flag == false)
            {
                TX_Packet[0] = 0x4D;                            // Header 'M'
                TX_Packet[1] = 0x53;                            // Header 'S'
                TX_Packet[2] = 7;                               // Packet Length
                TX_Packet[3] = CMD_L2_OUTPUT;                   // Command
                TX_Packet[4] = (byte)(L2_Output >> 8);          // L2 Output, HB
                TX_Packet[5] = (byte)L2_Output;                 // L2 Output, LB
                for (i = sum = 0; i < 6; i++) sum += TX_Packet[i];
                TX_Packet[6] = sum;                             // Checksum
                TX_CMD = CMD_L2_OUTPUT;
                TX_CTR = 0;
                TX_Flag = true;
                L2_Output_CHG = false;
            }

            if (TX_CTR == 0) Dsp_TX_Packet(Color.Blue, 7, ++TX_CTR);
            else Dsp_TX_Packet(Color.Magenta, 7, ++TX_CTR);
            ACK_TO_CTR = Auto_CTR = 0;

            try
            {
                serialPort.Write(TX_Packet, 0, 7);
            }
            catch (Exception)
            {
                Dsp_String(Color.Red, " 傳送封包 (L2 Output), 發生不可預期的錯誤 !\n");
                SystemSounds.Beep.Play();
                COMPort_Disconnect();                           // 串列埠離線
            }
        }

        private void TX_L3_Output()                             // 傳送: L3 Output
        {
            byte i, sum;

            if (TX_Flag == false)
            {
                TX_Packet[0] = 0x4D;                            // Header 'M'
                TX_Packet[1] = 0x53;                            // Header 'S'
                TX_Packet[2] = 7;                               // Packet Length
                TX_Packet[3] = CMD_L3_OUTPUT;                   // Command
                TX_Packet[4] = (byte)(L3_Output >> 8);          // L3 Output, HB
                TX_Packet[5] = (byte)L3_Output;                 // L3 Output, LB
                for (i = sum = 0; i < 6; i++) sum += TX_Packet[i];
                TX_Packet[6] = sum;                             // Checksum
                TX_CMD = CMD_L3_OUTPUT;
                TX_CTR = 0;
                TX_Flag = true;
                L3_Output_CHG = false;
            }

            if (TX_CTR == 0) Dsp_TX_Packet(Color.Blue, 7, ++TX_CTR);
            else Dsp_TX_Packet(Color.Magenta, 7, ++TX_CTR);
            ACK_TO_CTR = Auto_CTR = 0;

            try
            {
                serialPort.Write(TX_Packet, 0, 7);
            }
            catch (Exception)
            {
                Dsp_String(Color.Red, " 傳送封包 (L3 Output), 發生不可預期的錯誤 !\n");
                SystemSounds.Beep.Play();
                COMPort_Disconnect();                           // 串列埠離線
            }
        }

        private void TX_L4_Output()                             // 傳送: L4 Output
        {
            byte i, sum;

            if (TX_Flag == false)
            {
                TX_Packet[0] = 0x4D;                            // Header 'M'
                TX_Packet[1] = 0x53;                            // Header 'S'
                TX_Packet[2] = 7;                               // Packet Length
                TX_Packet[3] = CMD_L4_OUTPUT;                   // Command
                TX_Packet[4] = (byte)(L4_Output >> 8);          // L4 Output, HB
                TX_Packet[5] = (byte)L4_Output;                 // L4 Output, LB
                for (i = sum = 0; i < 6; i++) sum += TX_Packet[i];
                TX_Packet[6] = sum;                             // Checksum
                TX_CMD = CMD_L4_OUTPUT;
                TX_CTR = 0;
                TX_Flag = true;
                L4_Output_CHG = false;
            }

            if (TX_CTR == 0) Dsp_TX_Packet(Color.Blue, 7, ++TX_CTR);
            else Dsp_TX_Packet(Color.Magenta, 7, ++TX_CTR);
            ACK_TO_CTR = Auto_CTR = 0;

            try
            {
                serialPort.Write(TX_Packet, 0, 7);
            }
            catch (Exception)
            {
                Dsp_String(Color.Red, " 傳送封包 (L4 Output), 發生不可預期的錯誤 !\n");
                SystemSounds.Beep.Play();
                COMPort_Disconnect();                           // 串列埠離線
            }
        }

        private void TX_L1_ON_OFF()                             // 傳送: L1 ON/OFF
        {
            byte i, sum;

            if (TX_Flag == false)
            {
                TX_Packet[0] = 0x4D;                            // Header 'M'
                TX_Packet[1] = 0x53;                            // Header 'S'
                TX_Packet[2] = 6;                               // Packet Length
                TX_Packet[3] = CMD_L1_ON_OFF;                   // Command
                TX_Packet[4] = (byte)(L1_Set & SET_ON_OFF);     // L1 ON/OFF
                for (i = sum = 0; i < 5; i++) sum += TX_Packet[i];
                TX_Packet[5] = sum;                             // Checksum
                TX_CMD = CMD_L1_ON_OFF;
                TX_CTR = 0;
                TX_Flag = true;
                L1_ON_OFF_CHG = false;
            }

            if (TX_CTR == 0) Dsp_TX_Packet(Color.Green, 6, ++TX_CTR);
            else Dsp_TX_Packet(Color.Magenta, 6, ++TX_CTR);
            ACK_TO_CTR = Auto_CTR = 0;

            try
            {
                serialPort.Write(TX_Packet, 0, 6);
            }
            catch (Exception)
            {
                Dsp_String(Color.Red, " 傳送封包 (L1 ON/OFF), 發生不可預期的錯誤 !\n");
                SystemSounds.Beep.Play();
                COMPort_Disconnect();                           // 串列埠離線
            }
        }

        private void TX_L2_ON_OFF()                             // 傳送: L2 ON/OFF
        {
            byte i, sum;

            if (TX_Flag == false)
            {
                TX_Packet[0] = 0x4D;                            // Header 'M'
                TX_Packet[1] = 0x53;                            // Header 'S'
                TX_Packet[2] = 6;                               // Packet Length
                TX_Packet[3] = CMD_L2_ON_OFF;                   // Command
                TX_Packet[4] = (byte)(L2_Set & SET_ON_OFF);     // L2 ON/OFF
                for (i = sum = 0; i < 5; i++) sum += TX_Packet[i];
                TX_Packet[5] = sum;                             // Checksum
                TX_CMD = CMD_L2_ON_OFF;
                TX_CTR = 0;
                TX_Flag = true;
                L2_ON_OFF_CHG = false;
            }

            if (TX_CTR == 0) Dsp_TX_Packet(Color.Green, 6, ++TX_CTR);
            else Dsp_TX_Packet(Color.Magenta, 6, ++TX_CTR);
            ACK_TO_CTR = Auto_CTR = 0;

            try
            {
                serialPort.Write(TX_Packet, 0, 6);
            }
            catch (Exception)
            {
                Dsp_String(Color.Red, " 傳送封包 (L2 ON/OFF), 發生不可預期的錯誤 !\n");
                SystemSounds.Beep.Play();
                COMPort_Disconnect();                           // 串列埠離線
            }
        }

        private void TX_L3_ON_OFF()                             // 傳送: L3 ON/OFF
        {
            byte i, sum;

            if (TX_Flag == false)
            {
                TX_Packet[0] = 0x4D;                            // Header 'M'
                TX_Packet[1] = 0x53;                            // Header 'S'
                TX_Packet[2] = 6;                               // Packet Length
                TX_Packet[3] = CMD_L3_ON_OFF;                   // Command
                TX_Packet[4] = (byte)(L3_Set & SET_ON_OFF);     // L3 ON/OFF
                for (i = sum = 0; i < 5; i++) sum += TX_Packet[i];
                TX_Packet[5] = sum;                             // Checksum
                TX_CMD = CMD_L3_ON_OFF;
                TX_CTR = 0;
                TX_Flag = true;
                L3_ON_OFF_CHG = false;
            }

            if (TX_CTR == 0) Dsp_TX_Packet(Color.Green, 6, ++TX_CTR);
            else Dsp_TX_Packet(Color.Magenta, 6, ++TX_CTR);
            ACK_TO_CTR = Auto_CTR = 0;

            try
            {
                serialPort.Write(TX_Packet, 0, 6);
            }
            catch (Exception)
            {
                Dsp_String(Color.Red, " 傳送封包 (L3 ON/OFF), 發生不可預期的錯誤 !\n");
                SystemSounds.Beep.Play();
                COMPort_Disconnect();                           // 串列埠離線
            }
        }

        private void TX_L4_ON_OFF()                             // 傳送: L4 ON/OFF
        {
            byte i, sum;

            if (TX_Flag == false)
            {
                TX_Packet[0] = 0x4D;                            // Header 'M'
                TX_Packet[1] = 0x53;                            // Header 'S'
                TX_Packet[2] = 6;                               // Packet Length
                TX_Packet[3] = CMD_L4_ON_OFF;                   // Command
                TX_Packet[4] = (byte)(L4_Set & SET_ON_OFF);     // L4 ON/OFF
                for (i = sum = 0; i < 5; i++) sum += TX_Packet[i];
                TX_Packet[5] = sum;                             // Checksum
                TX_CMD = CMD_L4_ON_OFF;
                TX_CTR = 0;
                TX_Flag = true;
                L4_ON_OFF_CHG = false;
            }

            if (TX_CTR == 0) Dsp_TX_Packet(Color.Green, 6, ++TX_CTR);
            else Dsp_TX_Packet(Color.Magenta, 6, ++TX_CTR);
            ACK_TO_CTR = Auto_CTR = 0;

            try
            {
                serialPort.Write(TX_Packet, 0, 6);
            }
            catch (Exception)
            {
                Dsp_String(Color.Red, " 傳送封包 (L4 ON/OFF), 發生不可預期的錯誤 !\n");
                SystemSounds.Beep.Play();
                COMPort_Disconnect();                           // 串列埠離線
            }
        }

        /* 串列埠接收 ******************************************************* */

        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int bytes = serialPort.BytesToRead;

            if (bytes > 0)
            {
                serialPort.Read(RX_Buffer, 0, bytes);
                Packet_Check(bytes);                            // 封包資料判斷
            }
        }

        private void Packet_Check(int length)                   // 封包資料判斷
        {
            byte i, n, sum;

            for (i = 0; i < length; i++)
            {
                RX_Packet[RX_Index] = RX_Buffer[i];
                if (RX_Index == 0)                              // Header 'S'
                {
                    if (RX_Packet[0] == 0x53) RX_Index++;
                }
                else
                {
                    if (RX_Index == 1)                          // Header 'M'
                    {
                        if (RX_Packet[1] == 0x4D) RX_Index++;
                        else RX_Index = 0;
                    }
                    else
                    {
                        if (RX_Index == 2)                      // Packet Length
                        {
                            if ((RX_Packet[2] == 17) || (RX_Packet[2] == 5))
                            {
                                RX_Index++;
                                RX_Length = RX_Packet[2];
                            }
                            else RX_Index = 0;
                        }
                        else
                        {
                            if (++RX_Index >= RX_Length)
                            {
                                RX_Index = 0;
                                for (n = sum = 0; n < (RX_Length - 1); n++) sum += RX_Packet[n];
                                if (sum == RX_Packet[n]) Packet_OK();  // 封包資料格式 OK
                            }
                        }
                    }
                }
            }
        }

        private void Packet_OK()                                // 封包資料格式 OK
        {
            switch (TX_CMD)
            {
                case CMD_READ_DATA: if (RX_Length == 17) RX_Flag = true;
                                    break;
                case CMD_SYNC_EN  : if (RX_Length == 5) RX_Flag = true;
                                    break;
                case CMD_L1_SET   : if (RX_Length == 5) RX_Flag = true;
                                    break;
                case CMD_L2_SET   : if (RX_Length == 5) RX_Flag = true;
                                    break;
                case CMD_L3_SET   : if (RX_Length == 5) RX_Flag = true;
                                    break;
                case CMD_L4_SET   : if (RX_Length == 5) RX_Flag = true;
                                    break;
                case CMD_L1_OUTPUT: if (RX_Length == 5) RX_Flag = true;
                                    break;
                case CMD_L2_OUTPUT: if (RX_Length == 5) RX_Flag = true;
                                    break;
                case CMD_L3_OUTPUT: if (RX_Length == 5) RX_Flag = true;
                                    break;
                case CMD_L4_OUTPUT: if (RX_Length == 5) RX_Flag = true;
                                    break;
                case CMD_L1_ON_OFF: if (RX_Length == 5) RX_Flag = true;
                                    break;
                case CMD_L2_ON_OFF: if (RX_Length == 5) RX_Flag = true;
                                    break;
                case CMD_L3_ON_OFF: if (RX_Length == 5) RX_Flag = true;
                                    break;
                case CMD_L4_ON_OFF: if (RX_Length == 5) RX_Flag = true;
                                    break;
            }
        }

        /* L1輸出調整 ******************************************************* */

        private void cbb_L1_MaxC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((L1_Set >> 4) & 0x03) != cbb_L1_MaxC.SelectedIndex)
            {
                byte v = (byte)(L1_Set & (SET_EXT_CTRL | SET_ON_OFF));
                L1_Set = (byte)((cbb_L1_MaxC.SelectedIndex << 4) | v);

                nud_L1_Output.Value = tkb_L1_Output.Value = L1_Output = 0;
                nud_L1_Output.Maximum = tkb_L1_Output.Maximum = MaxCurrent[cbb_L1_MaxC.SelectedIndex];
                tkb_L1_Output.TickFrequency = tkb_L1_Output.Maximum / 10;
                L1_Output_CHG = false;

                if (READ_Flag == true) L1_Set_CHG = true;
            }
        }

        private void nud_L1_Output_ValueChanged(object sender, EventArgs e)
        {
            tkb_L1_Output.Value = (int)nud_L1_Output.Value;
            L1_Output = tkb_L1_Output.Value;
            int MaxC = MaxCurrent[cbb_L1_MaxC.SelectedIndex];
            lb_L1_PCV.Text = String.Format("{0:F1}", (float)(L1_Output * 100) / MaxC);

            if (READ_Flag == true) L1_Output_CHG = true;
        }

        private void tkb_L1_Output_ValueChanged(object sender, EventArgs e)
        {
            nud_L1_Output.Value = tkb_L1_Output.Value;
            L1_Output = tkb_L1_Output.Value;
            int MaxC = MaxCurrent[cbb_L1_MaxC.SelectedIndex];
            lb_L1_PCV.Text = String.Format("{0:F1}", (float)(L1_Output * 100) / MaxC);

            if (READ_Flag == true) L1_Output_CHG = true;
        }

        /* L1 ON/OFF ======================================================== */

        private void ckb_L1_EXT_CheckedChanged(object sender, EventArgs e)
        {
            if (ckb_L1_EXT.Checked == false)                    // Enable to Disable
            {
                pn_L1_LED.Visible = btn_L1.Visible = true;
                L1_Set &= (SET_EXT_CTRL ^ 0xFF);                // L1外部ON/OFF控制 = Disable
            }
            else                                                // Disable to Enable
            {
                pn_L1_LED.Visible = btn_L1.Visible = false;
                L1_Set |= SET_EXT_CTRL;                         // L1外部ON/OFF控制 = Enable
            }

            if (READ_Flag == true) L1_Set_CHG = true;

            switch (cbb_SYNC_EN.SelectedIndex)
            {
                case SYNC2: ckb_L2_EXT.Checked = ckb_L1_EXT.Checked; break;
                case SYNC3: ckb_L3_EXT.Checked = ckb_L2_EXT.Checked = ckb_L1_EXT.Checked; break;
                case SYNC4: ckb_L4_EXT.Checked = ckb_L3_EXT.Checked = ckb_L2_EXT.Checked = ckb_L1_EXT.Checked; break;
            }
        }

        private void btn_L1_Click(object sender, EventArgs e)
        {
            if ((L1_Set & SET_ON_OFF) == 0)                     // OFF to ON
            {
                pn_L1_LED.BackColor = Color.Green;
                btn_L1.Text = "OFF";
                L1_Set |= SET_ON_OFF;                           // L1 ON/OFF = ON
            }
            else                                                // ON to OFF
            {
                pn_L1_LED.BackColor = Color.Black;
                btn_L1.Text = "ON";
                L1_Set &= (SET_ON_OFF ^ 0xFF);                  // L1 ON/OFF = OFF
            }

            if (READ_Flag == true) L1_ON_OFF_CHG = true;

            if (SYNC_EN > ASYNC)
            {
                pn_L2_LED.BackColor = pn_L1_LED.BackColor;
                btn_L2.Text = btn_L1.Text;
                if ((L1_Set & SET_ON_OFF) == 0) L2_Set &= (SET_ON_OFF ^ 0xFF);
                else L2_Set |= SET_ON_OFF;
            }

            if (SYNC_EN > SYNC2)
            {
                pn_L3_LED.BackColor = pn_L1_LED.BackColor;
                btn_L3.Text = btn_L1.Text;
                if ((L1_Set & SET_ON_OFF) == 0) L3_Set &= (SET_ON_OFF ^ 0xFF);
                else L3_Set |= SET_ON_OFF;
            }

            if (SYNC_EN > SYNC3)
            {
                pn_L4_LED.BackColor = pn_L1_LED.BackColor;
                btn_L4.Text = btn_L1.Text;
                if ((L1_Set & SET_ON_OFF) == 0) L4_Set &= (SET_ON_OFF ^ 0xFF);
                else L4_Set |= SET_ON_OFF;
            }
        }

        /* L2輸出調整 ******************************************************* */

        private void cbb_L2_MaxC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((L2_Set >> 4) & 0x03) != cbb_L2_MaxC.SelectedIndex)
            {
                byte v = (byte)(L2_Set & (SET_EXT_CTRL | SET_ON_OFF));
                L2_Set = (byte)((cbb_L2_MaxC.SelectedIndex << 4) | v);

                nud_L2_Output.Value = tkb_L2_Output.Value = L2_Output = 0;
                nud_L2_Output.Maximum = tkb_L2_Output.Maximum = MaxCurrent[cbb_L2_MaxC.SelectedIndex];
                tkb_L2_Output.TickFrequency = tkb_L2_Output.Maximum / 10;
                L2_Output_CHG = false;

                if (READ_Flag == true) L2_Set_CHG = true;
            }
        }

        private void nud_L2_Output_ValueChanged(object sender, EventArgs e)
        {
            tkb_L2_Output.Value = (int)nud_L2_Output.Value;
            L2_Output = tkb_L2_Output.Value;
            int MaxC = MaxCurrent[cbb_L2_MaxC.SelectedIndex];
            lb_L2_PCV.Text = String.Format("{0:F1}", (float)(L2_Output * 100) / MaxC);

            if (READ_Flag == true) L2_Output_CHG = true;
        }

        private void tkb_L2_Output_ValueChanged(object sender, EventArgs e)
        {
            nud_L2_Output.Value = tkb_L2_Output.Value;
            L2_Output = tkb_L2_Output.Value;
            int MaxC = MaxCurrent[cbb_L2_MaxC.SelectedIndex];
            lb_L2_PCV.Text = String.Format("{0:F1}", (float)(L2_Output * 100) / MaxC);

            if (READ_Flag == true) L2_Output_CHG = true;
        }

        /* L2 ON/OFF ======================================================== */

        private void ckb_L2_EXT_CheckedChanged(object sender, EventArgs e)
        {
            if (ckb_L2_EXT.Checked == false)                    // Enable to Disable
            {
                pn_L2_LED.Visible = btn_L2.Visible = true;
                L2_Set &= (SET_EXT_CTRL ^ 0xFF);                // L2外部ON/OFF控制 = Disable
            }
            else                                                // Disable to Enable
            {
                pn_L2_LED.Visible = btn_L2.Visible = false;
                L2_Set |= SET_EXT_CTRL;                         // L2外部ON/OFF控制 = Enable
            }

            if ((READ_Flag == true) && (SYNC_EN < SYNC2)) L2_Set_CHG = true;
        }

        private void btn_L2_Click(object sender, EventArgs e)
        {
            if ((L2_Set & SET_ON_OFF) == 0)                     // OFF to ON
            {
                pn_L2_LED.BackColor = Color.Green;
                btn_L2.Text = "OFF";
                L2_Set |= SET_ON_OFF;                           // L2 ON/OFF = ON
            }
            else                                                // ON to OFF
            {
                pn_L2_LED.BackColor = Color.Black;
                btn_L2.Text = "ON";
                L2_Set &= (SET_ON_OFF ^ 0xFF);                  // L2 ON/OFF = OFF
            }

            if ((READ_Flag == true) && (SYNC_EN < SYNC2)) L2_ON_OFF_CHG = true;
        }

        /* L3輸出調整 ******************************************************* */

        private void cbb_L3_MaxC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((L3_Set >> 4) & 0x03) != cbb_L3_MaxC.SelectedIndex)
            {
                byte v = (byte)(L3_Set & (SET_EXT_CTRL | SET_ON_OFF));
                L3_Set = (byte)((cbb_L3_MaxC.SelectedIndex << 4) | v);

                nud_L3_Output.Value = tkb_L3_Output.Value = L3_Output = 0;
                nud_L3_Output.Maximum = tkb_L3_Output.Maximum = MaxCurrent[cbb_L3_MaxC.SelectedIndex];
                tkb_L3_Output.TickFrequency = tkb_L3_Output.Maximum / 10;
                L3_Output_CHG = false;

                if (READ_Flag == true) L3_Set_CHG = true;
            }
        }

        private void nud_L3_Output_ValueChanged(object sender, EventArgs e)
        {
            tkb_L3_Output.Value = (int)nud_L3_Output.Value;
            L3_Output = tkb_L3_Output.Value;
            int MaxC = MaxCurrent[cbb_L3_MaxC.SelectedIndex];
            lb_L3_PCV.Text = String.Format("{0:F1}", (float)(L3_Output * 100) / MaxC);

            if (READ_Flag == true) L3_Output_CHG = true;
        }

        private void tkb_L3_Output_ValueChanged(object sender, EventArgs e)
        {
            nud_L3_Output.Value = tkb_L3_Output.Value;
            L3_Output = tkb_L3_Output.Value;
            int MaxC = MaxCurrent[cbb_L3_MaxC.SelectedIndex];
            lb_L3_PCV.Text = String.Format("{0:F1}", (float)(L3_Output * 100) / MaxC);

            if (READ_Flag == true) L3_Output_CHG = true;
        }

        /* L3 ON/OFF ======================================================== */

        private void ckb_L3_EXT_CheckedChanged(object sender, EventArgs e)
        {
            if (ckb_L3_EXT.Checked == false)                    // Enable to Disable
            {
                pn_L3_LED.Visible = btn_L3.Visible = true;
                L3_Set &= (SET_EXT_CTRL ^ 0xFF);                // L3外部ON/OFF控制 = Disable
            }
            else                                                // Disable to Enable
            {
                pn_L3_LED.Visible = btn_L3.Visible = false;
                L3_Set |= SET_EXT_CTRL;                         // L3外部ON/OFF控制 = Enable
            }

            if ((READ_Flag == true) && (SYNC_EN < SYNC3)) L3_Set_CHG = true;
        }

        private void btn_L3_Click(object sender, EventArgs e)
        {
            if ((L3_Set & SET_ON_OFF) == 0)                     // OFF to ON
            {
                pn_L3_LED.BackColor = Color.Green;
                btn_L3.Text = "OFF";
                L3_Set |= SET_ON_OFF;                           // L3 ON/OFF = ON
            }
            else                                                // ON to OFF
            {
                pn_L3_LED.BackColor = Color.Black;
                btn_L3.Text = "ON";
                L3_Set &= (SET_ON_OFF ^ 0xFF);                  // L3 ON/OFF = OFF
            }

            if ((READ_Flag == true) && (SYNC_EN < SYNC3)) L3_ON_OFF_CHG = true;
        }

        /* L4輸出調整 ******************************************************* */

        private void cbb_L4_MaxC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((L4_Set >> 4) & 0x03) != cbb_L4_MaxC.SelectedIndex)
            {
                byte v = (byte)(L4_Set & (SET_EXT_CTRL | SET_ON_OFF));
                L4_Set = (byte)((cbb_L4_MaxC.SelectedIndex << 4) | v);

                nud_L4_Output.Value = tkb_L4_Output.Value = L4_Output = 0;
                nud_L4_Output.Maximum = tkb_L4_Output.Maximum = MaxCurrent[cbb_L4_MaxC.SelectedIndex];
                tkb_L4_Output.TickFrequency = tkb_L4_Output.Maximum / 10;
                L4_Output_CHG = false;

                if (READ_Flag == true) L4_Set_CHG = true;
            }
        }

        private void nud_L4_Output_ValueChanged(object sender, EventArgs e)
        {
            tkb_L4_Output.Value = (int)nud_L4_Output.Value;
            L4_Output = tkb_L4_Output.Value;
            int MaxC = MaxCurrent[cbb_L4_MaxC.SelectedIndex];
            lb_L4_PCV.Text = String.Format("{0:F1}", (float)(L4_Output * 100) / MaxC);

            if (READ_Flag == true) L4_Output_CHG = true;
        }

        private void tkb_L4_Output_ValueChanged(object sender, EventArgs e)
        {
            nud_L4_Output.Value = tkb_L4_Output.Value;
            L4_Output = tkb_L4_Output.Value;
            int MaxC = MaxCurrent[cbb_L4_MaxC.SelectedIndex];
            lb_L4_PCV.Text = String.Format("{0:F1}", (float)(L4_Output * 100) / MaxC);

            if (READ_Flag == true) L4_Output_CHG = true;
        }

        /* L4 ON/OFF ======================================================== */

        private void ckb_L4_EXT_CheckedChanged(object sender, EventArgs e)
        {
            if (ckb_L4_EXT.Checked == false)                    // Enable to Disable
            {
                pn_L4_LED.Visible = btn_L4.Visible = true;
                L4_Set &= (SET_EXT_CTRL ^ 0xFF);                // L4外部ON/OFF控制 = Disable
            }
            else                                                // Disable to Enable
            {
                pn_L4_LED.Visible = btn_L4.Visible = false;
                L4_Set |= SET_EXT_CTRL;                         // L4外部ON/OFF控制 = Enable
            }

            if ((READ_Flag == true) && (SYNC_EN != SYNC4)) L4_Set_CHG = true;
        }

        private void btn_L4_Click(object sender, EventArgs e)
        {
            if ((L4_Set & SET_ON_OFF) == 0)                     // OFF to ON
            {
                pn_L4_LED.BackColor = Color.Green;
                btn_L4.Text = "OFF";
                L4_Set |= SET_ON_OFF;                           // L4 ON/OFF = ON
            }
            else                                                // ON to OFF
            {
                pn_L4_LED.BackColor = Color.Black;
                btn_L4.Text = "ON";
                L4_Set &= (SET_ON_OFF ^ 0xFF);                  // L4 ON/OFF = OFF
            }

            if ((READ_Flag == true) && (SYNC_EN != SYNC4)) L4_ON_OFF_CHG = true;
        }

        /* SYNC Enable ****************************************************** */

        private void cbb_SYNC_EN_SelectedIndexChanged(object sender, EventArgs e)
        {
            SystemSounds.Exclamation.Play();
            SYNC_EN = (byte)cbb_SYNC_EN.SelectedIndex;
            switch (SYNC_EN)
            {
                case ASYNC: if ((ACK_State & AS_L2_ERR) == 0) ckb_L2_EXT.Enabled = btn_L2.Enabled = true;
                            if ((ACK_State & AS_L3_ERR) == 0) ckb_L3_EXT.Enabled = btn_L3.Enabled = true;
                            if ((ACK_State & AS_L4_ERR) == 0) ckb_L4_EXT.Enabled = btn_L4.Enabled = true;
                            break;
                case SYNC2: ckb_L2_EXT.Enabled = btn_L2.Enabled = false;
                            if ((ACK_State & AS_L3_ERR) == 0) ckb_L3_EXT.Enabled = btn_L3.Enabled = true;
                            if ((ACK_State & AS_L4_ERR) == 0) ckb_L4_EXT.Enabled = btn_L4.Enabled = true;
                            break;
                case SYNC3: ckb_L2_EXT.Enabled = btn_L2.Enabled = false;
                            ckb_L3_EXT.Enabled = btn_L3.Enabled = false;
                            if ((ACK_State & AS_L4_ERR) == 0) ckb_L4_EXT.Enabled = btn_L4.Enabled = true;
                            break;
                case SYNC4: ckb_L2_EXT.Enabled = btn_L2.Enabled = false;
                            ckb_L3_EXT.Enabled = btn_L3.Enabled = false;
                            ckb_L4_EXT.Enabled = btn_L4.Enabled = false;
                            break;
            }

            if (SYNC_EN > ASYNC)
            {
                ckb_L2_EXT.Checked = ckb_L1_EXT.Checked;
                pn_L2_LED.BackColor = pn_L1_LED.BackColor;
                pn_L2_LED.Visible = pn_L1_LED.Visible;
                btn_L2.Text = btn_L1.Text;
                btn_L2.Visible = btn_L1.Visible;
                if ((L1_Set & SET_EXT_CTRL) == 0) L2_Set &= (SET_EXT_CTRL ^ 0xFF);
                else L2_Set |= SET_EXT_CTRL;
                if ((L1_Set & SET_ON_OFF) == 0) L2_Set &= (SET_ON_OFF ^ 0xFF);
                else L2_Set |= SET_ON_OFF;
            }

            if (SYNC_EN > SYNC2)
            {
                ckb_L3_EXT.Checked = ckb_L1_EXT.Checked;
                pn_L3_LED.BackColor = pn_L1_LED.BackColor;
                pn_L3_LED.Visible = pn_L1_LED.Visible;
                btn_L3.Text = btn_L1.Text;
                btn_L3.Visible = btn_L1.Visible;
                if ((L1_Set & SET_EXT_CTRL) == 0) L3_Set &= (SET_EXT_CTRL ^ 0xFF);
                else L3_Set |= SET_EXT_CTRL;
                if ((L1_Set & SET_ON_OFF) == 0) L3_Set &= (SET_ON_OFF ^ 0xFF);
                else L3_Set |= SET_ON_OFF;
            }

            if (SYNC_EN > SYNC3)
            {
                ckb_L4_EXT.Checked = ckb_L1_EXT.Checked;
                pn_L4_LED.BackColor = pn_L1_LED.BackColor;
                pn_L4_LED.Visible = pn_L1_LED.Visible;
                btn_L4.Text = btn_L1.Text;
                btn_L4.Visible = btn_L1.Visible;
                if ((L1_Set & SET_EXT_CTRL) == 0) L4_Set &= (SET_EXT_CTRL ^ 0xFF);
                else L4_Set |= SET_EXT_CTRL;
                if ((L1_Set & SET_ON_OFF) == 0) L4_Set &= (SET_ON_OFF ^ 0xFF);
                else L4_Set |= SET_ON_OFF;
            }

            if (READ_Flag == true) SYNC_EN_CHG = true;
        }

        public bool HideEnable = false;
        private void Form_24084R_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (HideEnable == true)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbxLightOffSetEnabled_CheckedChanged(object sender, EventArgs e)
        {
            LightOffsetEnabled = cbxLightOffSetEnabled.Checked;
        }

    }
}
