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


namespace Advantech_IOCard
{
    public partial class DIO_Board : Form
    {

        private Label[] m_PortDi;
        private Label[] m_HexDi;
        private CheckBox[,] m_DI;
        private Label[] m_PortDo;
        private Label[] m_HexDo;
        private CheckBox[,] m_DO;
        private const int m_startPort = 0;
        private const int m_portCountShow = 4;
        // define save data 
        private Label[] m_Show;
        private byte[,] portSData = new byte[4,m_portCountShow] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 } , { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };
        public DIO_Board()
        {
            InitializeComponent();
        }

        public DIO_Board(int deviceNumber)
        {
            InitializeComponent();
            instantDiCtrl1.SelectedDevice = new DeviceInformation(deviceNumber);
            instantDoCtrl1.SelectedDevice = new DeviceInformation(deviceNumber);
        }
        private void DIO_Board_Load(object sender, EventArgs e)
        {
            //The default device of project is demo device, users can choose other devices according to their needs. 
            if (!instantDiCtrl1.Initialized)
            {
                MessageBox.Show("No device be selected or device open failed!", "StaticDI");
                this.Close();
                return;
            }

            //set Port, Hex, Di and Do
            m_PortDi = new Label[m_portCountShow] { PortDi1, PortDi2, PortDi3, PortDi4 };
            m_HexDi = new Label[m_portCountShow] { HexDi1, HexDi2, HexDi3, HexDi4 };
            m_DI = new CheckBox[m_portCountShow, 8]{
             {DI01, DI02, DI03, DI04, DI05, DI06, DI07, DI08},{DI09, DI10, DI11, DI12, DI13, DI14, DI15, DI16},
             {DI17, DI18, DI19, DI20, DI21, DI22, DI23, DI24},{DI25, DI26, DI27, DI28, DI29, DI30, DI31, DI32}};

            m_PortDo = new Label[m_portCountShow] { PortDo1, PortDo2, PortDo3, PortDo4 };
            m_HexDo = new Label[m_portCountShow] { HexDo1, HexDo2, HexDo3, HexDo4 };
            m_DO = new CheckBox[m_portCountShow, 8]{
             {DO01, DO02, DO03, DO04, DO05, DO06, DO07, DO08},{DO09, DO10, DO11, DO12, DO13, DO14, DO15, DO16},
             {DO17, DO18, DO19, DO20, DO21, DO22, DO23, DO24},{DO25, DO26, DO27, DO28, DO29, DO30, DO31, DO32}};

            m_Show = new Label[4] { lbl_Show1, lbl_Show2, lbl_Show3, lbl_Show4 };

            // Call the DOstate and using timer1 to read the DIstate
            InitialDOPortState();
            timer1.Enabled = true;
        }

        private void InitialDOPortState()
        {
            byte portDOData = 0;
            byte portDODir = 0xFF;
            ErrorCode errDO = ErrorCode.Success;
            byte[] mask = instantDoCtrl1.Features.DoDataMask;
            for (int i = 0; (i + m_startPort) < instantDoCtrl1.Features.PortCount && i < m_portCountShow; ++i)
            {
                errDO = instantDoCtrl1.Read(i + m_startPort, out portDOData);
                if (errDO != ErrorCode.Success)
                {
                    HandleError(errDO);
                    return;
                }

                m_PortDo[i].Text = (i + m_startPort).ToString();
                m_HexDo[i].Text = portDOData.ToString("X2");

                if (instantDoCtrl1.Ports != null)
                {
                    portDODir = (byte)instantDoCtrl1.Ports[i + m_startPort].Direction;
                }

                // Set CheckBox state
                for (int j = 0; j < 8; ++j)
                {
                    if (((portDODir >> j) & 0x1) == 0 || ((mask[i] >> j) & 0x1) == 0)  // Bit direction is input.
                    {
                        m_DO[i, j].BackColor = Color.Silver;
                        m_DO[i, j].Enabled = false;
                    }
                    else
                    {
                        m_DO[i, j].Click += new EventHandler(DO_CheckedChanged);
                        m_DO[i, j].Tag = new DoBitInformation((portDOData >> j) & 0x1, i + m_startPort, j);
                        if ((portDOData >> j & 0x1) > 0)
                        {
                            m_DO[i, j].Checked = true;
                            m_DO[i, j].CheckState = CheckState.Checked;
                            m_DO[i, j].BackColor = Color.Red;
                        }
                        else
                        {
                            m_DO[i, j].Checked = false;
                            m_DO[i, j].CheckState = CheckState.Unchecked;
                            m_DO[i, j].BackColor = Color.Lime;
                        }
                    }
                    m_DO[i, j].Invalidate();
                }
            }

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            // Read Di port state
            byte portDIData = 0;
            ErrorCode errDI = ErrorCode.Success;

            for (int i = 0; (i + m_startPort) < instantDiCtrl1.Features.PortCount && i < m_portCountShow; ++i)
            {
                errDI = instantDiCtrl1.Read(i + m_startPort, out portDIData);
                if (errDI != ErrorCode.Success)
                {
                    timer1.Enabled = false;
                    HandleError(errDI);
                    return;
                }

                m_PortDi[i].Text = (i + m_startPort).ToString();
                m_HexDi[i].Text = portDIData.ToString("X2");

                // Set CheckBox state
                for (int j = 0; j < 8; ++j)
                {
                    if ((portDIData >> j & 0x1) > 0)
                    {
                        m_DI[i, j].Checked = true;
                        m_DI[i, j].CheckState = CheckState.Checked;
                        m_DI[i, j].BackColor = Color.Red;
                    }
                    else
                    {
                        m_DI[i, j].Checked = false;
                        m_DI[i, j].CheckState = CheckState.Unchecked;
                        m_DI[i, j].BackColor = Color.Lime;
                    }
                    m_DI[i, j].Invalidate();
                }
            }
        }

        private void DO_CheckedChanged(object sender, EventArgs e)
        {
            ErrorCode errDO = ErrorCode.Success;
            CheckBox box = (CheckBox)sender;
            DoBitInformation boxInfo = (DoBitInformation)box.Tag;

            boxInfo.BitValue = (~(int)(boxInfo).BitValue) & 0x1;
            box.Tag = boxInfo;
            if (box.Checked)
            {
                box.BackColor = Color.Red;
            }
            else
            {
                box.BackColor = Color.Lime;
            }
            box.Invalidate();

            // refresh hex
            int state = Int32.Parse(m_HexDo[boxInfo.PortNum - m_startPort].Text, NumberStyles.AllowHexSpecifier);
            state &= ~(0x1 << boxInfo.BitNum);
            state |= boxInfo.BitValue << boxInfo.BitNum;

            m_HexDo[boxInfo.PortNum - m_startPort].Text = state.ToString("X2");
            errDO = instantDoCtrl1.Write(boxInfo.PortNum, (byte)state);
            if (errDO != ErrorCode.Success)
            {
                HandleError(errDO);
            }
        }

        public struct DoBitInformation
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

        // If err can not be read, showing the error message
        private void HandleError(ErrorCode err)
        {
            if (err != ErrorCode.Success)
            {
                MessageBox.Show("Sorry ! Some errors happened, the error code is: " + err.ToString(), "Static DI");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ErrorCode errREDO = ErrorCode.Success;
            byte portReDOData = 0;
            for (int i = 0; (i + m_startPort) < instantDiCtrl1.Features.PortCount && i < m_portCountShow; ++i)
            {
                m_HexDo[i].Text = portReDOData.ToString("X2");

                // Reset CheckBox state
                for (int j = 0; j < 8; ++j)
                {
                    m_DO[i, j].Click += new EventHandler(DO_CheckedChanged);
                    m_DO[i, j].Tag = new DoBitInformation((portReDOData >> j) & 0x1, i + m_startPort, j);
                    //Uncheck the checkbox state
                    m_DO[i, j].Checked = false;
                    m_DO[i, j].CheckState = CheckState.Unchecked;
                    m_DO[i, j].BackColor = Color.Lime;
                    m_DO[i, j].Invalidate();
                }
                errREDO = instantDoCtrl1.Write(i, portReDOData);
                if (errREDO != ErrorCode.Success)
                {
                    HandleError(errREDO);
                }
            }
            InitialDOPortState();
        }

        private void btn_Save1_Click(object sender, EventArgs e)
        {
            btn_SaveClick(0);
        }

        private void btn_Output1_Click(object sender, EventArgs e)
        {
            btn_OutputClick(0);
        }

        private void btn_Zero1_Click(object sender, EventArgs e)
        {
            btn_ZeroClick(0);
        }

        private void btn_Save2_Click(object sender, EventArgs e)
        {
            btn_SaveClick(1);
        }

        private void btn_Output2_Click(object sender, EventArgs e)
        {
            btn_OutputClick(1);
        }

        private void btn_Zero2_Click(object sender, EventArgs e)
        {
            btn_ZeroClick(1);
        }

        private void btn_Save3_Click(object sender, EventArgs e)
        {
            btn_SaveClick(2);
        }

        private void btn_Output3_Click(object sender, EventArgs e)
        {
            btn_OutputClick(2);
        }

        private void btn_Zero3_Click(object sender, EventArgs e)
        {
            btn_ZeroClick(2);
        }

        private void btn_Save4_Click(object sender, EventArgs e)
        {
            btn_SaveClick(3);
        }

        private void btn_Output4_Click(object sender, EventArgs e)
        {
            btn_OutputClick(3);
        }

        private void btn_Zero4_Click(object sender, EventArgs e)
        {
            btn_ZeroClick(3);
        }

        private void btn_SaveClick(int S)
        {
            ErrorCode errS = ErrorCode.Success;
            byte[] mask = instantDoCtrl1.Features.DoDataMask;
            for (int i = 0; (i + m_startPort) < instantDoCtrl1.Features.PortCount && i < m_portCountShow; ++i)
            {
                errS = instantDoCtrl1.Read(i + m_startPort, out portSData[S,i]);
                if (errS != ErrorCode.Success)
                {
                    HandleError(errS);
                    return;
                }
                else
                {
                    m_Show[S].BackColor = Color.Lime;
                }
            }
        }

        private void btn_OutputClick(int O)
        {
            if (m_Show[O].BackColor == SystemColors.ButtonFace)
            {
                return;
            }
            byte portSDir = 0xFF;
            ErrorCode errS = ErrorCode.Success;
            byte[] mask = instantDoCtrl1.Features.DoDataMask;
            for (int i = 0; (i + m_startPort) < instantDoCtrl1.Features.PortCount && i < m_portCountShow; ++i)
            {
                if (errS != ErrorCode.Success)
                {
                    HandleError(errS);
                    return;
                }

                m_HexDo[i].Text = portSData[O,i].ToString("X2");

                if (instantDoCtrl1.Ports != null)
                {
                    portSDir = (byte)instantDoCtrl1.Ports[i + m_startPort].Direction;
                }

                // Set CheckBox state
                for (int j = 0; j < 8; ++j)
                {
                    if (((portSDir >> j) & 0x1) == 0 || ((mask[i] >> j) & 0x1) == 0)  // Bit direction is input.
                    {
                        m_DO[i, j].BackColor = Color.Silver;
                        m_DO[i, j].Enabled = false;
                    }
                    else
                    {
                        m_DO[i, j].Click += new EventHandler(DO_CheckedChanged);
                        m_DO[i, j].Tag = new DoBitInformation((portSData[O,i] >> j) & 0x1, i + m_startPort, j);
                        if ((portSData[O,i] >> j & 0x1) > 0)
                        {
                            m_DO[i, j].Checked = true;
                            m_DO[i, j].CheckState = CheckState.Checked;
                            m_DO[i, j].BackColor = Color.Red;
                        }
                        else
                        {
                            m_DO[i, j].Checked = false;
                            m_DO[i, j].CheckState = CheckState.Unchecked;
                            m_DO[i, j].BackColor = Color.Lime;
                        }
                    }
                    errS = instantDoCtrl1.Write(i, portSData[O,i]);
                    m_DO[i, j].Invalidate();
                }
            }
            InitialDOPortState();
        }

        private void btn_ZeroClick(int Z)
        {
            for (int i = 0; i < m_portCountShow; ++i)
            {
                portSData[Z, i] = 0;
            }
            
            m_Show[Z].BackColor = SystemColors.ButtonFace;
        }
    }
}
