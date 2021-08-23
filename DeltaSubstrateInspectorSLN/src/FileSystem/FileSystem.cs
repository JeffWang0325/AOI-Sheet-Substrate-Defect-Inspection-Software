using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HalconDotNet;

using DeltaSubstrateInspector.src.PositionMethod.LocateMethod.Location; // (20190131) Jeff Revised!
using DeltaSubstrateInspector.src.Modules.NewInspectModule.InductanceInsp; // For enuBand

#region 加速DataGridView更新速度

using System.Windows.Forms;
using System.Globalization;
using System.Reflection;

public static class ExtensionMethods
{
    public static void DoubleBuffered(this DataGridView dgv, bool setting)
    {
        Type dgvType = dgv.GetType();
        PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
        pi.SetValue(dgv, setting, null);
    }
}

#endregion

namespace DeltaSubstrateInspector.FileSystem
{
    public class FinalInspectParam
    {
        /// <summary>
        /// 模組名稱
        /// </summary>
        public static string ProductName { get; set; } = "A"; // (20200429) Jeff Revised!

        /// <summary>
        /// 視覺定位
        /// </summary>
        public static bool VisualPosEnable { get; set; } = true; // (20191214) Jeff Revised!
        /// <summary>
        /// Cell中心檢測
        /// </summary>
        public static bool CellCenterInspectEnable { get; set; } = false;
        /// <summary>
        /// 拼圖參考影像ID
        /// </summary>
        public static int GlobalMapImgID { get; set; } = 0;
        /// <summary>
        /// 不良比例門檻 (%)
        /// </summary>
        public static double NGRatio { get; set; } = 100;

        /* 新增檢測模組 (其他參數) (20190719) Jeff Revised! */
        /// <summary>
        /// 對位參考影像ID
        /// </summary>
        public static int PosImgID { get; set; } = 0;
        /// <summary>
        /// 瑕疵分類
        /// </summary>
        public static bool b_NG_classification { get; set; } = false;
        /// <summary>
        /// 瑕疵優先權
        /// </summary>
        public static bool b_NG_priority { get; set; } = false;
        /// <summary>
        /// 人工覆判
        /// </summary>
        public static bool b_Recheck { get; set; } = false;
        /// <summary>
        /// 圖像顯示視窗
        /// </summary>
        public static string dispWindow { get; set; } = "pictureBox";
        /// <summary>
        /// 儲存統計結果 (.csv)
        /// </summary>
        public static bool b_saveCSV { get; set; } = false;
        /// <summary>
        /// 儲存瑕疵結果
        /// </summary>
        public static bool b_SaveDefectResult { get; set; } = false;
        /// <summary>
        /// 儲存所有Cell影像
        /// </summary>
        public static bool b_SaveAllCellImage { get; set; } = true;
        /// <summary>
        /// 儲存瑕疵Cell影像
        /// </summary>
        public static bool b_SaveDefectCellImage { get; set; } = false;
        /// <summary>
        /// 儲存瑕疵結果 路徑
        /// </summary>
        public static string Directory_SaveDefectResult { get; set; } = "D:\\DSI\\AOI_Result"; // (20200429) Jeff Revised!
        /// <summary>
        /// 自動合併產生雙面工單
        /// </summary>
        public static bool b_Auto_AB { get; set; } = false; // (20200429) Jeff Revised!
        /// <summary>
        /// A面工單
        /// </summary>
        public static string A_Recipe { get; set; } = ""; // (20200429) Jeff Revised!
        /// <summary>
        /// 雙面合併是否上下翻轉
        /// </summary>
        public static bool B_UpDownInvert_FS { get; set; } = false; // (20200429) Jeff Revised!
        /// <summary>
        /// 雙面合併是否左右翻轉
        /// </summary>
        public static bool B_LeftRightInvert_FS { get; set; } = false; // (20200429) Jeff Revised!

        /// <summary>
        /// 工單路徑
        /// </summary>
        public static string Directory_defComb { get; set; } = "D:\\DSI\\DefectCombined_Recipe\\uPOL"; // (20200429) Jeff Revised!

        /// <summary>
        /// 輸出路徑
        /// </summary>
        public static string OutputDirectory_defComb { get; set; } = "D:\\DSI\\PDF"; // (20200429) Jeff Revised!

        /// <summary>
        /// AI分圖是否啟用
        /// </summary>
        public static bool b_AICellImg_Enable { get; set; } = false; // (20190830) Jeff Revised!

        // (20190902) Jeff Revised!
        /// <summary>
        /// AI分圖影像ID
        /// </summary>
        public static int DAVSImgID { get; set; } = 0;
        /// <summary>
        /// AI分圖影像扭正
        /// </summary>
        public static bool b_DAVSImgAlign { get; set; } = false;
        /// <summary>
        /// AI分圖 Mix Band
        /// </summary>
        public static bool b_DAVSMixImgBand { get; set; } = false;
        /// <summary>
        /// AI分圖Band1影像ID
        /// </summary>
        public static int DAVSBand1ImgIndex { get; set; } = 0;
        /// <summary>
        /// AI分圖Band2影像ID
        /// </summary>
        public static int DAVSBand2ImgIndex { get; set; } = 0;
        /// <summary>
        /// AI分圖Band3影像ID
        /// </summary>
        public static int DAVSBand3ImgIndex { get; set; } = 0;
        /// <summary>
        /// AI分圖Band1色域
        /// </summary>
        public static enuBand DAVSBand1 { get; set; } = enuBand.R;
        /// <summary>
        /// AI分圖Band2色域
        /// </summary>
        public static enuBand DAVSBand2 { get; set; } = enuBand.G;
        /// <summary>
        /// AI分圖Band3色域
        /// </summary>
        public static enuBand DAVSBand3 { get; set; } = enuBand.B;
    }

    public enum FileDirectory
    {
        RecipeFileDirectory,
        ImageFileDirectory,
        ProductionDirectory,
        HistoryDirectory,
        LogFileDirectory,
        DeleteFileLogDirectory,
        ErrorLogDirectory,
        RecipeRecordDirectory,
        LanguageFileDirectory
    };
    
    public enum PnlSide { A = 0, B };

    public class FileSystem
    {
        public static string ModuleParamDirectory { get; set; } = "D:\\DSI";
        public static string RecipeFileDirectory { get; set; } = "\\Recipe\\";
        public static string ImageFileDirectory { get; set; } = "\\Image\\"; // 181114, andy
        public static string ProductionDirectory { get; set; } = "\\Production\\";
        public static string HistoryDirectory { get; set; } = "\\History\\";
        public static string ConfigDirectory { get; set; } = "\\Config\\";  // 181121, andy
        public static string DeleteFileLogDirectory { get; set; } = "\\ProductionDeleteLog\\";
        public static string ErrorLogDirectory { get; set; } = "\\ErrorLog\\";
        public static string RecipeRecordDirectory { get; set; } = "\\RecipeRecord\\";
        public static string LanguageFileDirectory { get; set; } = "\\Language\\";
        public static string PositionParam { get; set; } = "\\System\\Position";
        public static string LaserPositionParam { get; set; } = "\\System\\LaserPosition"; // (20181031) Jeff Revised!
        public static string UserUISettingPathFile { get; set; } = "UserUISetting.xml";

        public static string HardwareConfigFileName { get; set; } = "hardware_cfg.ini"; // 181121, andy
        public static string LastRecipePathFile { get; set; } = "LastRecipeName.ini";
        public static string LightComportName { get; set; } = "COM1";
        public static string LastRecipeName { get; set; } = "";
        public static bool LightOffSet { get; set; } = false;
        public static bool BypassLightControl { get; set; } = false;
        public static bool BypassIO { get; set; } = false;

        /// <summary>
        /// 相機取像CallBackType
        /// </summary>
        public static string CallBackType { get; set; } = enu_CallBackType.EventExposureEnd.ToString(); // (20191216) MIL Jeff Revised!
        /// <summary>
        /// 相機取像CallBackType
        /// </summary>
        public enum enu_CallBackType // (20191216) Jeff Revised!
        {
            EventExposureEnd,
            transfer_end
        };

        /// <summary>
        /// 相機介面 (0:USB3.0, 1:GigE, 2:MIL)
        /// </summary>
        public static int CameraInterface { get; set; } = (int)(enu_CameraInterface.USB3); // (20191216) Jeff Revised!
        /// <summary>
        /// 相機介面類型
        /// </summary>
        public enum enu_CameraInterface // (20191216) Jeff Revised!
        {
            USB3,
            GigE,
            MIL
        };

        /// <summary>
        /// 影像存檔類型 (Halcon)
        /// </summary>
        public enum enu_ImageFormat // (20191226) Jeff Revised!
        {
            tiff,
            jpeg,
            png,
            bmp
        };

        public static string[] SaveImageFilter { get; set; } = new string[] { "TIFF Image|*.tif", "JPG Image|*.jpg", "PNG Image|*.png", "Bitmap Image|*.bmp" }; // (20200429) Jeff Revised!

        /// <summary>
        /// 影像輸出格式 (實際顯示之副檔名)
        /// </summary>
        public enum enu_DispImageFormat // (20200429) Jeff Revised!
        {
            tif,
            jpg,
            png,
            bmp
        }

        /// <summary>
        /// Word檔案格式
        /// </summary>
        public enum enu_WordFormat // (20200429) Jeff Revised!
        {
            doc,
            docx
        }

        /// <summary>
        /// 工單名稱
        /// </summary>
        public static string ModuleName { get; set; } = "";
        /// <summary>
        /// 生產料號
        /// </summary>
        public static string PartNumber { get; set; } = "Default1234";
        //public static string ModuleType = ""; // 181026, andy
        public static string InspectDate { get; set; } = "";

        /// <summary>
        /// Substrate ID
        /// </summary>
        public static string SB_ID { get; set; } = "00000000";
        public static string MOVE_ID { get; set; } = ""; // 181114, andy 位移ID

        /// <summary>
        /// 是否啟用內部計數ID模式
        /// </summary>
        public static bool B_SB_InerCountID { get; set; } = false;
        
        /// <summary>
        /// 基板序號 (內部計數ID模式)
        /// </summary>
        public static string SB_InerCountID { get; set; } = "00000000";

        public static HTuple Affine_angle_degree { get; set; } = 0.0; // (20190125) Jeff Revised!

        public static DAVS.clsLog BtnLog { get; set; } = new DAVS.clsLog("D://DSI//Log//Btn_Log//");

        /// <summary>
        /// 可藉此取得定位設定內所有參數
        /// </summary>
        public static LocateMethod Locate_Method_FS { get; set; } = new LocateMethod(); // (20190131) Jeff Revised!

        /// <summary>
        /// 影像是否全數 載入/取像 完畢
        /// </summary>
        public static bool B_imagesFinished { get; set; } = false; // (20190604) Jeff Revised!

        public static Dictionary<FileDirectory, string> FileDirectoryDictionary { get; set; } = new Dictionary<FileDirectory, string>()
        {
            {FileDirectory.RecipeFileDirectory, RecipeFileDirectory},
            {FileDirectory.ImageFileDirectory, ImageFileDirectory},
            {FileDirectory.ProductionDirectory, ProductionDirectory},
            {FileDirectory.HistoryDirectory, HistoryDirectory},
            {FileDirectory.DeleteFileLogDirectory, DeleteFileLogDirectory},
            {FileDirectory.ErrorLogDirectory, ErrorLogDirectory},
            {FileDirectory.RecipeRecordDirectory, RecipeRecordDirectory},
            {FileDirectory.LanguageFileDirectory, LanguageFileDirectory},
        };

        /// <summary>
        /// 每個取像位置之Cell Region之集合
        /// </summary>
        public static List<HObject> CellReg_MoveIndex_FS { get; set; } = new List<HObject>(); // (20190830) Jeff Revised!

        /// <summary>
        /// 每個取像位置之瑕疵 Region之集合
        /// </summary>
        public static List<HObject> DefReg_MoveIndex_FS { get; set; } = new List<HObject>(); // (20200429) Jeff Revised!

        /// <summary>
        /// 語言類型
        /// </summary>
        public static string Language { get; set; } = "Chinese"; // (20200214) Jeff Revised!
    }
}
