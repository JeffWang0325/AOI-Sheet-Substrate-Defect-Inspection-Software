using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPIPTool;
using static DeltaSubstrateInspector.src.Modules.SystemModel.SystemStatus;

namespace DeltaSubstrateInspector.src.Modules.SystemModel
{
    public class TCPIPCtrl
    {
        public static TCPIPModule tcp_model_ = new TCPIPModule();

        public static void InitialTCPIP(string ip, int port)
        {
            TCPInfo info = new TCPInfo(ip, port);
            tcp_model_.initialize(info);
        }

        public static TCPIPModule GetTCPIP()
        {
            return tcp_model_;
        }

        public static void ReConnect()
        {
            tcp_model_.OpenTCPIP();
        }
    }

    public class TCPIPModule
    {
        private TCPServer tcpServer;
        public TCPInfo tcpInfo;

        public TCPIPModule()  { }

        public void initialize(TCPInfo info)
        {
            tcpInfo = info;
            tcpServer = new TCPServer(info);
        }

        public List<string> ReceiveMessage
        {
            get
            {
                return tcpServer.ReceiveMessage;
            }
        }

        public List<string> SendMessage
        {
            get
            {
                return tcpServer.SendMessage;
            }
        }

        public void ConnectClient()
        {
            tcpServer.GetClient();
        }

        public void OpenTCPIP()
        {
            if (tcpServer != null)
                tcpServer.Dispose();

            tcpServer.OpenTCP(tcpInfo);
        }

        public void SendCommand(string command)
        {
            tcpServer.Send(command);
        }

        public string ReadTCPIP()
        {
            String msg = tcpServer.Read();
            if (msg != "")
            {
                int start = msg.IndexOf("\u0002") == -1 ? 0 : msg.IndexOf("\u0002") + 1;
                int end = msg.IndexOf("\u0003") == -1 ? msg.Length : msg.IndexOf("\u0003") - 1;
                msg = msg.Substring(start, end);
            }
            return msg;
        }

        public string ClientAddresss
        {
            get { return tcpServer.ClientAddress; }
        }

        public void Dispose()
        {

        }
    }
}
