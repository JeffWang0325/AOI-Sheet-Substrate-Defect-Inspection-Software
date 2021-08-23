using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Drawing;

namespace DeltaSubstrateInspector.src.Modules.SystemModel
{
    public class SystemStatus
    {
        // 系統部屬狀態 : 相機走位與觸發次數
        public static int InspectModelIndex = 0;
        public static Point[] MarkPoints = new Point[2];
        public static int MovementCount = 0;
        public static bool CameraStart = false;

        // 瑕疵地圖設定
        public static List<int> MapRowCount = new List<int>();
        public static List<int> MapColCount = new List<int>();
        public static List<int> LocateCount = new List<int>();
        public static List<int[]> InspectByPass = new List<int[]>();
        public static List<int> TriggerCounts = new List<int>();
        public static string InspectionRegion = "Area";


        // 軸控連線狀態
        public static bool MotionCard = false;
        public static bool XAxis = false;
        public static bool YAxis = false;
        public static bool ZAxis = false;

        // 光源連線狀態
        public static bool Light = false;

        // 相機連線狀態
        public static bool CameraConnect = false;

        // TCP/IP連線狀態
        public static bool TCPIP = true;
        public static string TCPIP_IP = "";
        public static int TCPIP_Port = 0;
        public static bool TCPIPConnect = false;

        // 工單號與型別
        public static string RecipeName = "None";
        public static string RecipeType = "None";
        public static int ChipId = 0;
        public static string Mode = "";

        // 檢測結果
        public static string ResultString = "";

        // 檢測定位設定內容
        public static string LocatePositionMethod = "VirtualSegmentation";

        //
        public static Dictionary<int, object> ChipCollection = new Dictionary<int, object>();

        // Labeler Tool Need
        public static object GoldenRgn = null;
        public static object LabelerRole = null;

        // Status Functions
        public static bool get_status_by_name(string name)
        {
            Type myType = typeof(SystemStatus);
            return (bool)myType.GetField(name).GetValue(0);
        }
        public static List<string> get_status_names()
        {
            Type myType = typeof(SystemStatus);
            return myType.GetMembers().Where(x => x.MemberType == System.Reflection.MemberTypes.Field &&  x.ToString().Substring(0, 7) == "Boolean").ToList().Select(x => x.Name).ToList();
        }
    }
}
