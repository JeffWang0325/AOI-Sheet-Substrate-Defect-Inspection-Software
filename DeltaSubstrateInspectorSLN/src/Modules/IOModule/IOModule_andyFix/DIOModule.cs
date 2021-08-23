/**
* @author : Jerome Tseng
* @modifier (2018.07): Anita 
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using Automation.BDaq;
using System.Globalization;

namespace IOModule
{
    public partial class DIO_Board : Form
    {
        public DIO_Board()
        {
            InitializeComponent();
        }

        private const int m_startPort = 0;
        private const int m_portCountShow = 4;
        private byte[] portDIData = new byte[m_portCountShow] { 0, 0, 0, 0 };  //DI Hex Data 
        private byte[] portDOData = new byte[m_portCountShow] { 0, 0, 0, 0 };  //DO Hex Data
        string ButtonBinary_;
        string check_SystemStatus_;
        string check_MeasureStartStatus_;
        string check_MeasureDoneStatus_;

        public DIO_Board(int deviceNumber)
        {
            InitializeComponent();
            instantDiCtrl1.SelectedDevice = new DeviceInformation(deviceNumber);
            instantDoCtrl1.SelectedDevice = new DeviceInformation(deviceNumber);

        }

        public bool initialConnect()
        {
            if (!instantDiCtrl1.Initialized)
            {
                //MessageBox.Show("No device be selected or device open failed!", "StaticDI");
                this.Close();
                return false;
            }
            InitialDOPortState();
            timer1.Enabled = true;

            return true;
        }

        private void InitialDOPortState()
        {
            byte portDODir = 0xFF;
            ErrorCode errDO = ErrorCode.Success;
            for (int i = 0; (i + m_startPort) < instantDoCtrl1.Features.PortCount && i < m_portCountShow; ++i)  //Have to check instanDOCtrl1 to set the PCIE1756 card 
            {
                errDO = instantDoCtrl1.Read(i + m_startPort, out portDOData[i]);   //Read DO Data and save it into portDOData
                if (errDO != ErrorCode.Success)
                {
                    HandleError(errDO);
                    return;
                }

                if (instantDoCtrl1.Ports != null)
                {
                    portDODir = (byte)instantDoCtrl1.Ports[i + m_startPort].Direction;
                }
            }
        }

        public void timer1_Tick(object sender, EventArgs e)
        {
            // Read Di port state
            ErrorCode errDI = ErrorCode.Success;

            for (int i = 0; (i + m_startPort) < instantDiCtrl1.Features.PortCount && i < m_portCountShow; ++i)
            {
                errDI = instantDiCtrl1.Read(i + m_startPort, out portDIData[i]);  //Read DI Data and save it into portDIData
                ButtonBinary_ = Convert.ToString(portDIData[i], 2).PadLeft(16, '0');
                if (i == 0)
                {
                    check_SystemStatus_ = ButtonBinary_[15].ToString();
                    check_MeasureStartStatus_ = ButtonBinary_[14].ToString();
                    check_MeasureDoneStatus_ = ButtonBinary_[13].ToString();
                }
                if (errDI != ErrorCode.Success)
                {
                    timer1.Enabled = false;
                    HandleError(errDI);
                    return;
                }
            }

            // set label backcolor
            Sys_Online.BackColor = IsInputBitOn("SystemOnline") ? Color.Lime : Color.Silver;
            Sys_MeasureStart.BackColor = IsInputBitOn("SystemMeasureStart") ? Color.Lime : Color.Silver;
            Sys_MeasureDone.BackColor = IsInputBitOn("SystemMeasureDone") ? Color.Lime : Color.Silver;
            ModuleOnline.BackColor = IsOutputBitOn("ModuleOnline") ? Color.Lime : Color.Silver;
            ModuleMeasureStart.BackColor = IsOutputBitOn("ModuleMeasureStart") ? Color.Lime : Color.Silver;
            ModuleMeasureDone.BackColor = IsOutputBitOn("ModuleMeasureDone") ? Color.Lime : Color.Silver;
        }

        private void ChangeDOState()
        {
            byte portDir = 0xFF;
            ErrorCode errS = ErrorCode.Success;
            byte[] mask = instantDoCtrl1.Features.DoDataMask;
            for (int i = 0; (i + m_startPort) < instantDoCtrl1.Features.PortCount && i < m_portCountShow; ++i)
            {
                if (errS != ErrorCode.Success)
                {
                    HandleError(errS);
                    return;
                }
                if (instantDoCtrl1.Ports != null)
                {
                    portDir = (byte)instantDoCtrl1.Ports[i + m_startPort].Direction;
                }
                errS = instantDoCtrl1.Write(i, portDOData[i]);   // write Data into PCIE1756 card
            }
            InitialDOPortState();
        }    

        private struct DoBitInformation
        {
            #region fields
            private int m_bitValue;
            private int m_portDo;
            private int m_bitDo;
            #endregion

            public DoBitInformation(int bitValue, int portDo, int bitDo)
            {
                m_bitValue = bitValue;
                m_portDo = portDo;
                m_bitDo = bitDo;
            }

            #region Properties
            public int BitValue
            {
                get { return m_bitValue; }
                set
                {
                    m_bitValue = value & 0x1;
                }
            }
            public int PortNum
            {
                get { return m_portDo; }
                set
                {
                    if ((value - m_startPort) >= 0
                       && (value - m_startPort) <= (m_portCountShow - 1))
                    {
                        m_portDo = value;
                    }
                }
            }
            public int BitNum
            {
                get { return m_bitDo; }
                set
                {
                    if (value >= 0 && value <= 7)
                    {
                        m_bitDo = value;
                    }
                }
            }
            #endregion
        }

        private void CheckState()
        {
            ErrorCode errDI = ErrorCode.Success;
            ErrorCode errDO = ErrorCode.Success;
            byte portDODir = 0xFF;
            for (int i = 0; (i + m_startPort) < instantDoCtrl1.Features.PortCount && i < m_portCountShow; ++i)
            {
                errDI = instantDiCtrl1.Read(i + m_startPort, out portDIData[i]);
                errDO = instantDoCtrl1.Read(i + m_startPort, out portDOData[i]);
                if (instantDoCtrl1.Ports != null)
                {
                    portDODir = (byte)instantDoCtrl1.Ports[i + m_startPort].Direction;
                }
            }
        }

        private void ControlOutputState(string name, bool state)
        {
            // 用以判斷最終是否成功改變狀態
            bool error = false;

            // 控制指定點位的狀態
            switch (name)
            {
                case "ModuleOnline":
                    portDOData[0] = state ? (byte)1 : (byte)0;
                    break;

                case "ModuleMeasureStart":
                    portDOData[0] = state ? (byte)3 : (byte)1;
                    break;

                case "ModuleMeasureDone":
                    portDOData[0] = state ? (byte)5 : (byte)1;
                    break;

                case "ModuleOffline":
                    portDOData[0] = 0;
                    break;

                 default :
                    error = true;
                  //  MessageBox.Show("IO Name or State Error");
                    break;
            }

            // 改變輸出狀態
            if (!error)
                ChangeDOState();
        }

        private bool ReadInputState(string name)
        {
            // 確認輸入輸出狀態
            CheckState();

            // 當System端未連線或斷線情況
             if (check_SystemStatus_ == "0" || check_SystemStatus_ == null)
            {
              //  MessageBox.Show("System Off-line Or System Error");
                return false;
            }

             // System端已連線時，判斷指定點位的狀態
            switch (name)
            {
                case "SystemOnline":
                    return true;

                case "SystemMeasureStart":
                    return (check_MeasureStartStatus_ == "1") ? true : false;

                case "SystemMeasureDone":
                    return (check_MeasureDoneStatus_ == "1") ? true : false;

                default:
                    MessageBox.Show("System Error");
                    break;
            }
            return false;
        }

        private bool ReadOutputState(string name)
        {
            // 確認輸入輸出狀態
            CheckState();

            // 未連線或斷線情況
            if (portDOData[0] == 0)
            {
                //MessageBox.Show("ModuleOffline");
                return false;
            }

            // 已連線時，判斷指定點位的狀態
            switch (name)
            {
                case "ModuleOnline":
                    return (portDOData[0] % 2 != 0) ? true : false;
                    
                case "ModuleMeasureStart":
                    return (portDOData[0] == 3) ? true : false;

                case "ModuleMeasureDone":
                    return (portDOData[0] == 5) ? true : false;

                default:
                    MessageBox.Show("Module Output Error");
                    break;
            }

            return false;
        }

        public void SetOutputBit(string name, bool setBitOn)
        {
            if (IsOutputBitOn(name) != setBitOn)
                ControlOutputState(name, setBitOn);
        }

        public bool IsInputBitOn(string name)
        {
            return ReadInputState(name);
        }

        public bool IsOutputBitOn(string name)
        {
            return ReadOutputState(name);
        }

        public void HandleError(ErrorCode err)
        {
            if (err != ErrorCode.Success)
            {
                MessageBox.Show("Sorry ! Some errors happened, the error code is: " + err.ToString(), "Static DI");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            initialConnect();
        }

        private void output_control(object sender, EventArgs e)
        {
            Label lbl = sender as Label;
            string name = lbl.Name;
            ControlOutputState(name, ReadOutputState(name)^true);
        }
    }
}
