using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.IO.Ports;
using System.Threading;
using gts;

namespace DeltaSubstrateInspector.src.Modules.MotionModule
{
    public class Motion
    {
        /// <summary>
        /// Googol軸卡是否連線
        /// </summary>
        public static bool b_googol_connecting_ = false;

        /// <summary>
        /// 1~4軸是否啟動
        /// </summary>
        public static bool[] b_axis_connect_ = new bool[4];

        /// <summary>
        /// 速度
        /// </summary>
        public static double vel_ = 50;
        public double set_vel
        { get { return vel_; } set { vel_ = value; } }
        /// <summary>
        /// 加速度
        /// </summary>
        static double acc_ = 0.1;
        public double set_acc
        { get { return acc_; } set { acc_ = value; } }
        /// <summary>
        /// 減速度
        /// </summary>
        static double dec_ = 0.1;
        public double set_dec
        { get { return dec_; } set { dec_ = value; } }

        /// <summary>
        /// 點膠機通訊SerialPort
        /// </summary>
        public static SerialPort SerialPort_XYZTable = new SerialPort();
        public static bool b_xyz_table_connecting_ = false;
        public static Position xyz_cur_pos_ = new Position(-1, -1, -1);
        public static bool b_xyz_motion_copleting_ = false;

        //***************************************
        public class Position
        {
            float x_;
            public float set_x
            { get { return x_; } set { x_ = value; } }
            float y_;
            public float set_y
            { get { return y_; } set { y_ = value; } }
            float z_;
            public float set_z
            { get { return z_; } set { z_ = value; } }

            public Position(float x, float y, float z)
            {
                x_ = x;
                y_ = y;
                z_ = z;
            }
        }

        #region Googol Motion
        /// <summary>
        /// Googol軸卡連線
        /// </summary>
        /// <returns></returns>
        public static bool googol_connecting()
        {
            bool b_connecting = false;
            short rtn = 0; // Googol命令狀態
            rtn = mc.GT_Open(0, 1);

            if (rtn != 0) // 連線失敗            
                b_connecting = false;
            else
            {
                b_connecting = true;
                rtn = mc.GT_Reset();              
                rtn = mc.GT_HomeInit();//舊版HOME指令才需要 VB版本可用新的HOME指令
                Motion.axis_initial(1);
                Motion.axis_initial(2);
                Motion.axis_initial(3);
            }

            Motion.b_googol_connecting_ = b_connecting;
            return b_connecting;           
        }

        /// <summary>
        /// Googol軸卡離線
        /// </summary>
        public static void googol_closing()
        {
            short rtn = 0; // Googol命令狀態
            stop_all_motion();
            rtn = mc.GT_AxisOff(1);//Servo Off
            rtn = mc.GT_AxisOff(2);
            rtn = mc.GT_AxisOff(3);
            rtn = mc.GT_AxisOff(4);
            rtn = mc.GT_Reset();
            rtn = mc.GT_Close();
        }

        public static void stop_all_motion()
        {
           mc.GT_Stop(0xff, 0);//停止全部運動
        }

        /// <summary>
        /// 初始化 【1~4軸】 相關信號
        /// (否則無法啟動)
        /// </summary>
        /// <param name="axis_">選擇【1~4軸】</param>
        /// <returns></returns>
        private static short axis_initial(short axis_)
        {
            short rtn_;
            mc.GT_AlarmOff(axis_);  // 軸驅動警報無效
            mc.GT_LmtsOff(axis_, -1);  // 軸正負限位信號無效
            mc.GT_AxisOn(axis_);  //軸驅動器開啟
            mc.GT_ClrSts(axis_, 4);
            rtn_ = mc.GT_AxisOn(axis_);

            if (rtn_ != 0)
                b_axis_connect_[axis_ - 1] = false;
            else
                b_axis_connect_[axis_ - 1] = true;
            return rtn_;
        }

        /// <summary>
        /// 顯示 1~4軸 編碼器&規劃之位置、速度、加速度及運動模式
        /// </summary>
        /// <param name="axis_">選擇第1~4軸</param>
        /// <param name="apos_">編碼器實際位置</param>
        /// <param name="ppose_">編碼器規劃位置</param>
        public static void axis_status_update(short axis_, out string apos_, out string ppose_)
        {            
            double act_pos_ = 0;
            double prf_pos_ = 0;
            uint pClock;

            mc.GT_GetAxisEncPos(axis_, out act_pos_, 1, out pClock);  // 讀取軸1編碼器-實際位置
            apos_ = act_pos_.ToString("0.00");
            mc.GT_GetPrfPos(axis_, out prf_pos_, 1, out pClock);  // 讀取軸1-規劃位置
            ppose_ = prf_pos_.ToString("0.00");

            //double act_v_, prf_v_, prf_acc_;
            //int mode_;
            //mc.GT_GetAxisEncVel(1, out act_v_, 1, out pClock);  // 讀取軸1編碼器-實際速度
            //mc.GT_GetPrfVel(1, out prf_v_, 1, out pClock);  // 讀取軸1-規劃速度
            //mc.GT_GetPrfAcc(1, out prf_acc_, 1, out pClock);  // 讀取軸1-規劃加速度
            //mc.GT_GetPrfMode(1, out mode_, 1, out pClock); // 讀取軸1-運動模式

        }

        /// <summary>
        /// Jog運動指令
        /// </summary>
        /// <param name="axis">運動軸</param>
        /// <param name="vel">設定速度</param>
        /// <param name="acc">設定加速度</param>
        /// <param name="dec">設定減速度</param>
        /// <returns></returns>
        public static short jogMotion(short axis, bool b_positive_)
        {
            short rtn = 0;
            mc.TJogPrm jogPrm;

            

            rtn = mc.GT_GetJogPrm(axis, out jogPrm);

            jogPrm.acc = acc_;
            jogPrm.dec = dec_;
            jogPrm.smooth = 0.5;
            rtn = mc.GT_PrfJog(axis);
            rtn = mc.GT_SetJogPrm(axis, ref jogPrm);
            if (b_positive_)
                rtn = mc.GT_SetVel(axis, vel_);
            else
                rtn = mc.GT_SetVel(axis, -vel_);
            rtn = mc.GT_Update(1 << (axis - 1));
            return rtn;
        }

        /// <summary>
        /// P2P運動指令
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="pos"></param>
        /// <param name="vel"></param>
        /// <param name="acc"></param>
        /// <returns></returns>
        public static short p2pMotion(short axis, double pos)
        {
            short rtn;
            mc.TTrapPrm trapPrm;

            rtn = mc.GT_GetTrapPrm((short)axis, out trapPrm);

            trapPrm.acc = acc_;
            trapPrm.dec = dec_;
            trapPrm.smoothTime = 25;
            trapPrm.velStart = 0;

            rtn = mc.GT_PrfTrap(axis);
            rtn = mc.GT_SetTrapPrm(axis, ref trapPrm);
            rtn = mc.GT_SetPos(axis, (int)pos);
            rtn = mc.GT_SetVel(axis, vel_);
            rtn = mc.GT_Update(1 << (axis - 1));

            return rtn;
        }

        /// <summary>
        /// 設定非軸DAC(Ch9~12)數值
        /// </summary>
        /// <param name="dac_channel_">非軸DAC(Ch9~12)</param>
        /// <param name="set_value_">設定電壓數值(小於5V)</param>
        /// <returns></returns>
        public static bool set_unaxis_dac(short dac_channel_, string set_value_)
        {
            bool b_error_ = false;

            if (dac_channel_ >= 9 && dac_channel_ <= 12)
            {
                double analog_ = Convert.ToDouble(set_value_);
                short sSetValue_1 = (short)(32767 / 10 * analog_);

                if (analog_ < 5.0)
                    mc.GT_SetDac(dac_channel_, ref sSetValue_1, 1);
                else
                    b_error_ = true;
            }
            else
                b_error_ = true;

            return b_error_;
        }
        #endregion

        #region 點膠機 Motion
        /// <summary>
        /// 點膠機連線
        /// </summary>
        /// <param name="comport_"></param>
        /// <returns></returns>
        public static bool xyz_connect(string comport_)
        {
            SerialPort_XYZTable.Close();
            SerialPort_XYZTable.PortName = comport_;
            SerialPort_XYZTable.BaudRate = 115200;
            SerialPort_XYZTable.DataBits = 8;
            SerialPort_XYZTable.Handshake = Handshake.None;
            SerialPort_XYZTable.Parity = Parity.None;
            SerialPort_XYZTable.StopBits = StopBits.One;
            SerialPort_XYZTable.WriteTimeout = 5000;
            SerialPort_XYZTable.ReadTimeout = 500;
            SerialPort_XYZTable.WriteBufferSize = 10240;
            SerialPort_XYZTable.ReadBufferSize = 10240;
            SerialPort_XYZTable.RtsEnable = true;
            SerialPort_XYZTable.DtrEnable = true;
            if (!SerialPort_XYZTable.IsOpen)
            {
                try
                {
                    SerialPort_XYZTable.Open();

                    if (SerialPort_XYZTable.IsOpen)
                        b_xyz_table_connecting_ = true;
                    else
                        b_xyz_table_connecting_ = false;
                    //SerialPort_XYZTable.Write("PA\r");
                    //Thread.Sleep(500);
                    //byte[] Buffer = new byte[99];
                    //SerialPort_XYZTable.Read(Buffer, 0, 99);
                    //textBox2.Text = Encoding.Default.GetString(Buffer);                    
                    //Table.Enabled = true;
                    //InitRobot.Enabled = true;
                }
                catch (Exception ex)
                {
                    b_xyz_table_connecting_ = false;
                    //SerialPort_XYZTable.RtsEnable = false;
                    //SerialPort_XYZTable.DtrEnable = false;                    
                }
            }
            else
            {
                b_xyz_table_connecting_ = true;
            }

            return b_xyz_table_connecting_;
        }

        #region 機台移動
        /// <summary>
        /// 三軸原點復歸
        /// </summary>
        public static void xyz_home()
        {
            SerialPort_XYZTable.Write("HM\r");
            //serialport_feedback();
        }

        public static void xyz_go()
        {
            SerialPort_XYZTable.Write("GO\r");
            serialport_feedback();
        }

        /// <summary>
        /// 三軸絕對座標移動
        /// </summary>
        /// <param name="abs_x_"></param>
        /// <param name="abs_y_"></param>
        /// <param name="abs_z_"></param>
        public static void xyz_abs_move(double abs_x_, double abs_y_, double abs_z_)
        {
            if (abs_x_ >= 0 && abs_y_ >= 0 && abs_z_ >= 0)
            {
                string command_ = "MA " +
                    abs_x_.ToString() + "," +
                    abs_y_.ToString() + "," +
                    abs_z_.ToString() + "\r";
                SerialPort_XYZTable.Write(command_);
                //serialport_feedback();               
            }
        }

        /// <summary>
        /// 連續 X/Y/Z軸 正/負方向移動
        /// </summary>
        /// <param name="xyz_axis_">X/Y/Z軸 (1~3)</param>
        /// <param name="direction_">正/負方向 (0/1)</param>
        public static void xyz_jog_move(int xyz_axis_,int direction_)
        {
            switch (xyz_axis_)
            {
                case 1: // X軸
                    if (direction_ == 0) //正方向
                    {
                        string command_ = "JOGXR\r ";
                        SerialPort_XYZTable.Write(command_);
                        //serialport_feedback();
                    }
                    else if (direction_ == 1) //負方向
                    {
                        string command_ = "JOGXL\r ";
                        SerialPort_XYZTable.Write(command_);
                        //serialport_feedback();
                    }
                    break;
                case 2: // Y軸
                    if (direction_ == 0) //正方向
                    {
                        string command_ = "JOGYD\r ";
                        SerialPort_XYZTable.Write(command_);
                        //serialport_feedback();
                    }
                    else if (direction_ == 1) //負方向
                    {
                        string command_ = "JOGYU\r ";
                        SerialPort_XYZTable.Write(command_);
                        //serialport_feedback();
                    }
                    break;
                case 3: // Z軸
                    if (direction_ == 0) //正方向
                    {
                        string command_ = "JOGZD\r ";
                        SerialPort_XYZTable.Write(command_);
                        //serialport_feedback();
                    }
                    else if (direction_ == 1) //負方向
                    {
                        string command_ = "JOGZU\r ";
                        SerialPort_XYZTable.Write(command_);
                        //serialport_feedback();
                    }
                    break;
            }           
        }
        #endregion

        #region 機台狀態
        /// <summary>
        /// 回傳機台是否靜止 (1:停止 0:運動中)
        /// </summary>
        /// <returns></returns>
        public static string xyz_get_motion_status()
        {
            string feed_back_ = "0";
            string command_ = "IV\r";
            SerialPort_XYZTable.Write(command_);
            feed_back_ = serialport_feedback();

            if (feed_back_.Contains("1"))
                b_xyz_motion_copleting_ = true;
            else
                b_xyz_motion_copleting_ = false;
            return feed_back_;
        }

        public static void xyz_get_xyz_position()
        {
            Thread.Sleep(500);
            string command_ = "PA\r";
            SerialPort_XYZTable.Write(command_);

            string feedback_ = serialport_feedback();
            if (feedback_ != null)
            {
                string[] lines = feedback_.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                string[] pos_ = lines.Where(x=>x.Contains(',')).ToArray()[0].Split(',');
                xyz_cur_pos_ = new Position(Convert.ToSingle(pos_[0]), Convert.ToSingle(pos_[1]), Convert.ToSingle(pos_[2]));
            }
        }

        /// <summary>
        /// 等待XYZ三軸完全靜止
        /// </summary>
        /// <returns></returns>
        public static string xyz_wait_move_completing()
        {
            string command_ = "ID\r";
            SerialPort_XYZTable.Write(command_);
            return serialport_feedback();
        }

        public static string xyz_wait_time(string time_)
        {
            string status_ = null;
            string command_ = "WAIT " +
                    time_.ToString() + "\r";
            SerialPort_XYZTable.Write(command_);
            //return serialport_feedback();

            return status_;
        }

        /// <summary>
        /// 機台運動立即停止
        /// </summary>
        /// <returns></returns>
        public static string xyz_motion_stop()
        {
            string command_ = "STOP\r";
            SerialPort_XYZTable.Write(command_);
            return serialport_feedback();
        }
        #endregion

        #region IO指令
        /// <summary>
        /// 設定機台Output狀態
        /// </summary>
        /// <param name="port_">Output Port (12為點膠訊號)</param>
        /// <param name="on_off_">1:On, 0:Off</param>
        /// <returns></returns>
        public static string xyz_io(int port_, int on_off_)
        {
            string status_ = null;
            string command_ = "OU " +
                port_.ToString() + "," +
                on_off_.ToString() + "\r";
            SerialPort_XYZTable.Write(command_);
            status_ = serialport_feedback();
            return status_;
        }
        #endregion

        #region 機台運動參數
        /// <summary>
        /// 設定機台三軸移動速度
        /// </summary>
        /// <param name="speed_"></param>
        public static void xyz_set_speed(double speed_)
        {
            if (speed_ > 0)
            {
                string command_ = "SP " +
                    speed_.ToString() + "\r";
                SerialPort_XYZTable.Write(command_);
                //serialport_feedback();
            }
        }

        /// <summary>
        /// 設定點對點移動速度Acc
        /// </summary>
        /// <param name="speed_"></param>
        public static void xyz_set_acc(double speed_)
        {
            if (speed_ > 0)
            {
                string command_ = "ACM " +
                    speed_.ToString() + "\r";
                SerialPort_XYZTable.Write(command_);
                serialport_feedback();
            }
        }
        #endregion

        /// <summary>
        /// 點膠機指令回傳
        /// </summary>
        /// <returns></returns>
        public static string serialport_feedback()
        {
            string temp_="0";
            try
            {
                Thread.Sleep(500);
                byte[] Buffer = new byte[99];
                SerialPort_XYZTable.Read(Buffer, 0, 99);
                temp_ = Encoding.Default.GetString(Buffer);
            }
            catch(TimeoutException ex)
            {
                serialport_feedback();
                temp_ = "0";
            }

            return temp_;
        }
        #endregion

    }
}
