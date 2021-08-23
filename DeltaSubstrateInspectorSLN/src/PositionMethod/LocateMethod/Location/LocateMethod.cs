#define showTimeSpan // 是否顯示程式執行時間 (20181116) Revised!

using HalconDotNet;
using DeltaSubstrateInspector.src.PositionMethod.LocateMethod.Rotation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Extension;
using static DeltaSubstrateInspector.FileSystem.FileSystem;

using DAVS; // (20190412) Jeff Revised!
using DeltaSubstrateInspector.FileSystem; // (20190629) Jeff Revised!
using DeltaSubstrateInspector.src.Modules; // (20190629) Jeff Revised!
using DeltaSubstrateInspector.src.Modules.ResultModule;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary; // (20190824) Jeff Revised!
using System.Runtime.Serialization; // (20190824) Jeff Revised!
using System.Collections; // For ArrayList, Hashtable
using System.Media; // For SystemSounds
using Create_WordPDF; // (20200429) Jeff Revised!
using DeltaSubstrateInspector.src.Modules.NewInspectModule.InductanceInsp; // (20200429) Jeff Revised!

namespace DeltaSubstrateInspector.src.PositionMethod.LocateMethod.Location
{
    [Serializable] // (20190824) Jeff Revised!
    public class LocateMethod
    //public class LocateMethod : ICloneable
    {
        /// <summary>
        /// 物件複製 (淺層)
        /// Note: 如果欄位是實值型別，則會複製出欄位的複本。 如果欄位是參考型別，將只會複製參考!
        /// </summary>
        /// <returns></returns>
        public LocateMethod Clone() // (20190822) Jeff Revised!
        {
            return this.MemberwiseClone() as LocateMethod;
        }
        
        [NonSerialized] // (20190824) Jeff Revised!
        private clsLog Log = new clsLog();

        public enum enuSearchType
        {
            TopLeft = 0,
            BottomLeft = 1,
            TopRight = 2,
            BottomRight = 3,
            Center = 4
        }
        
        #region 視覺定位 & 大圖拼接 各種參數及變數

        private AngleAffineMethod affine_method_ = new AngleAffineMethod();

        public LocateMethodRecipe_New LocateMethodRecipe = new LocateMethodRecipe_New(); // (20190722) Jeff Revised!

        /// <summary>
        /// 各基板儲存資料夾
        /// </summary>
        public string File_directory_ { get; set; } = "default";

        /// <summary>
        /// 基板型號
        /// </summary>
        public string board_type_ { get; set; } = "default";
        /// <summary>
        /// 視覺解析度 (um/pix)
        /// </summary>
        public double pixel_resolution_ { get; set; } = 1.72;
        /// <summary>
        /// 影像寬度 (pixel)
        /// </summary>
        public int frame_pwidth_ { get; set; } = 4096;
        /// <summary>
        /// 影像高度 (pixel)
        /// </summary>
        public int frame_pheight_ { get; set; } = 2160;
        
        /// <summary>
        /// 樣品寬度 (mm)
        /// </summary>
        public double size_sample_rx { get; set; } = 100; // (20180828) Jeff Revised!
        /// <summary>
        /// 樣品高度 (mm)
        /// </summary>
        public double size_sample_ry { get; set; } = 100; // (20180828) Jeff Revised!
        /// <summary>
        /// Resize
        /// </summary>
        public double resize_ { get; set; } = 0.5; // (20180828) Jeff Revised!

        /// <summary>
        /// 運動與影像座標之X方向是否反向
        /// </summary>
        public int X_dir_inv = 1, Y_dir_inv = 0; // (20180901) Jeff Revised!
        /// <summary>
        /// 第一張定位影像 X軸機械座標 (mm)
        /// </summary>
        public double mark1_rpos_x_ { get; set; } = 155.1;
        /// <summary>
        /// 第一張定位影像 Y軸機械座標 (mm)
        /// </summary>
        public double mark1_rpos_y_ { get; set; } = 275.9;
        /// <summary>
        /// 第二張定位影像 X軸機械座標 (mm)
        /// </summary>
        public double mark2_rpos_x_ { get; set; } = 155.1;
        /// <summary>
        /// 第二張定位影像 Y軸機械座標 (mm)
        /// </summary>
        public double mark2_rpos_y_ { get; set; } = 373;
        /// <summary>
        /// 第一張檢測影像 X軸機械座標 (mm)
        /// </summary>
        public double start_rpos_x_ { get; set; } = 188.4;
        /// <summary>
        /// 第一張檢測影像 Y軸機械座標 (mm)
        /// </summary>
        public double start_rpos_y_ { get; set; } = 282.5;

        /// <summary>
        /// 陣列位置 X軸次數
        /// </summary>
        public int array_x_count_ { get; set; } = 14;
        /// <summary>
        /// 陣列位置 X軸間距 (mm)
        /// </summary>
        public double array_x_rpitch_ { get; set; } = 6.5;
        /// <summary>
        ///  陣列位置 Y軸次數
        /// </summary>
        public int array_y_count_ { get; set; } = 25;
        /// <summary>
        /// 陣列位置 Y軸間距 (mm)
        /// </summary>
        public double array_y_rpitch_ { get; set; } = 3.5;
        /// <summary>
        /// 基板內cell總行數
        /// </summary>
        public int cell_col_count_ { get; set; } = 244;
        /// <summary>
        /// 基板內cell總列數
        /// </summary>
        public int cell_row_count_ { get; set; } = 130;
        
        /// <summary>
        /// 定位點至第一個Cell之dxdy (mm)
        /// </summary>
        public double mark_rdx_ = -35.177, mark_rdy_ = 5.963;
        public double mark_rdx_shift = 0.03, mark_rdy_shift = 0.0; // (20180828) Jeff Revised!
        /// <summary>
        /// cell真實寬度、高度、dx及dy (mm)
        /// </summary>
        public double cell_rwidth_ = 0.3, cell_rheight_ = 0.57, cell_rdx_ = 0.355, cell_rdy_ = 0.625;

        /// <summary>
        /// 定位點至第一個Cell之dxdy (pixel)
        /// </summary>
        public double mark_pdx_ = -1, mark_pdy_ = -1;
        /// <summary>
        /// cell真實寬度、高度、dx及dy (pixel)
        /// </summary>
        public double cell_pwidth_ = -1, cell_pheight_ = -1, cell_pdx_ = -1, cell_pdy_ = -1;

        /// <summary>
        /// X, Y方向分區數量
        /// </summary>
        public int num_region_x_ = 2, num_region_y_ = 2; // (20180828) Jeff Revised!
        /// <summary>
        /// X, Y方向各分區之Cell顆數
        /// </summary>
        public string cell_x_count_string = "122, 122", cell_y_count_string = "65, 65"; // (20180903) Jeff Revised!
        [XmlIgnore] // 帶有XmlIgnore，表示序列化時不序列化此屬性 (Note: HTuple不能序列化)
        public HTuple cell_x_count_HTuple = new HTuple(); // (20200429) Jeff Revised!
        [XmlIgnore] // 帶有XmlIgnore，表示序列化時不序列化此屬性 (Note: HTuple不能序列化)
        public HTuple cell_y_count_HTuple = new HTuple(); // (20200429) Jeff Revised!

        /// <summary>
        /// X, Y方向各分區與第一顆Cell距離 (mm)
        /// </summary>
        public string dist_region_rdx_string = "0, 46.4", dist_region_rdy_string = "0, 45.2"; // (20180903) Jeff Revised!
        [XmlIgnore] // 帶有XmlIgnore，表示序列化時不序列化此屬性 (Note: HTuple不能序列化)
        public HTuple dist_region_rdx_HTuple = new HTuple(); // (20200429) Jeff Revised!
        [XmlIgnore] // 帶有XmlIgnore，表示序列化時不序列化此屬性 (Note: HTuple不能序列化)
        public HTuple dist_region_rdy_HTuple = new HTuple(); // (20200429) Jeff Revised!

        /// <summary>
        /// 每個影像位置包含之完整Cell顆數
        /// </summary>
        [XmlIgnore] // 帶有XmlIgnore，表示序列化時不序列化此屬性 (Note: HTuple不能序列化)
        public HTuple CellCount_zone = new HTuple(); // (20181003) Jeff Revised!
        /// <summary>
        /// 大圖拼接校正補償
        /// </summary>
        public int center_x_pix_shift = 4, center_y_pix_shift = -7; // (20181022) Jeff Revised!

        public int cs_thred_min_ { get; set; } = 0;
        public int cs_thred_max_ { get; set; } = 200;
        public int cs_area_min_ { get; set; } = 12000;
        public int cs_area_max_ { get; set; } = 20000;
        public int cd_thred_min_ { get; set; } = 20;
        public int cd_thred_max_ { get; set; } = 120;
        public int cd_area_min_ { get; set; } = 4000;
        public int cd_area_max_ { get; set; } = 9000;

        /// <summary>
        /// 是否已執行 resize_parameters()
        /// </summary>
        public bool b_resizeParam { get; set; } = false; // (20180828) Jeff Revised!

        /// <summary>
        /// 檢測到兩原始定位影像之定位點 x座標
        /// </summary>
        [XmlIgnore] // 帶有XmlIgnore，表示序列化時不序列化此屬性 (Note: HTuple不能序列化)
        public HTuple origin_mark_pos_x_ { get; set; } = null;
        double origin_mark_pos_x_resize { get; set; } = -1;
        /// <summary>
        /// 檢測到兩原始定位影像之定位點 y座標
        /// </summary>
        [XmlIgnore] // 帶有XmlIgnore，表示序列化時不序列化此屬性 (Note: HTuple不能序列化)
        public HTuple origin_mark_pos_y_ { get; set; } = null;
        double origin_mark_pos_y_resize { get; set; } = -1;
        
        /// <summary>
        /// 需要扭正之角度 (degree)
        /// </summary>
        [XmlIgnore] // 帶有XmlIgnore，表示序列化時不序列化此屬性 (Note: HTuple不能序列化)
        public HTuple affine_degree_ { get; set; } = null;
        
        /// <summary>
        /// 已扭正之標準cell區域 (All Golden Cell Regions) (For 第一張檢測影像扭正)
        /// </summary>
        public HObject cellmap_sortregion_ = null;
        
        /// <summary>
        /// 各影像在大拼接圖上之FOV
        /// </summary>
        public HObject capture_map_ = null;

        /// <summary>
        /// 將歪斜扭正之旋轉矩陣
        /// </summary>
        private HTuple hommat2d_affine_; // (20200429) Jeff Revised!
        /// <summary>
        /// 復原至原始狀態之旋轉矩陣
        /// </summary>
        private HTuple hommat2d_origin_; // (20200429) Jeff Revised!

        /// <summary>
        /// 當前基板之所有瑕疵所在cell座標
        /// </summary>
        public List<Point> all_defect_id_ = new List<Point>();
        /// <summary>
        /// 當前基板之所有瑕疵所在cell座標 (人工覆判)
        /// </summary>
        public List<Point> all_defect_id_Recheck = new List<Point>(); // (20190704) Jeff Revised!

        /// <summary>
        /// 當前基板之所有瑕疵所在cell區域 (原始)
        /// </summary>
        public HObject all_intersection_defect_ = null;

        /// <summary>
        /// 當前基板之所有瑕疵所在cell區域 (原始) (Cell region)
        /// </summary>
        public HObject all_intersection_defect_CellReg = null; // (20190715) Jeff Revised!

        /// <summary>
        /// 當前基板之所有瑕疵所在cell區域 (人工覆判)
        /// </summary>
        public HObject all_intersection_defect_Recheck = null; // (20190704) Jeff Revised!

        /// <summary>
        /// 是否考慮背景瑕疵
        /// </summary>
        public bool b_BackDefect { get; set; } = false; // (20190107) Jeff Revised!
        /// <summary>
        /// 當前基板之所有背景瑕疵
        /// </summary>
        public HObject all_BackDefect_ = null; // (20181115) Jeff Revised!
        /// <summary>
        /// 已扭正(復原)之cell全域區域 (For 第一張檢測影像)
        /// </summary>
        public HObject cellmap_affine_;

        /// <summary>
        /// cellmap_affine_ 之 各Cell面積
        /// </summary>
        [XmlIgnore] // 帶有XmlIgnore，表示序列化時不序列化此屬性 (Note: HTuple不能序列化)
        public HTuple area_cellmap_affine_ = new HTuple(); // (20190701) Jeff Revised!
        /// <summary>
        /// 用於序列化 (area_cellmap_affine_)
        /// </summary>
        public double[] area_cellmap_affine_DArr // (20200429) Jeff Revised!
        {
            get // 序列化
            {
                return this.area_cellmap_affine_.DArr;
            }

            set // 反序列化
            {
                this.area_cellmap_affine_.DArr = value;
            }
        }

        /// <summary>
        /// 當前基板之所有檢測到之Cell座標
        /// </summary>
        public List<Point> all_Cell_id_ { get; set; } = new List<Point>(); // (20181207) Jeff Revised!
        /// <summary>
        /// 當前基板之所有檢測到之Cell所在cell區域
        /// </summary>
        public HObject all_intersection_Cell_ = null; // (20190701) Jeff Revised!
        /// <summary>
        /// 基板內所有未被檢測到之Cell座標 (原始)
        /// </summary>
        public List<Point> all_CellDefect_id_ { get; set; } = new List<Point>(); // (20181207) Jeff Revised!
        /// <summary>
        /// Marks中心位置之region (For 扭正之大圖)
        /// </summary>
        public HObject MarkCenter_BigMap_affine = null; // (20181226) Jeff Revised!
        /// <summary>
        /// Marks中心位置之region (For 未扭正之原始大圖)
        /// </summary>
        public HObject MarkCenter_BigMap_orig = null; // (20181226) Jeff Revised!

        /// <summary>
        /// 拼接影像
        /// </summary>
        public HObject TileImage = null; // (20200429) Jeff Revised!

        #endregion

        #region 瑕疵分類 & 人工覆判

        /// <summary>
        /// 模組名稱
        /// </summary>
        public string productName { get; set; } = ""; // (20200429) Jeff Revised!

        /// <summary>
        /// 工單名稱
        /// </summary>
        public string moduleName { get; set; } = ""; // (20200429) Jeff Revised!

        /// <summary>
        /// 生產料號
        /// </summary>
        public string partNumber { get; set; } = "Default1234"; // (20200429) Jeff Revised!

        /// <summary>
        /// 序號
        /// </summary>
        public string sB_ID { get; set; } = "00000000"; // (20200429) Jeff Revised!

        /// <summary>
        /// 瑕疵分類
        /// </summary>
        [XmlIgnore] // 帶有XmlIgnore，表示序列化時不序列化此屬性
        public Dictionary<string, DefectsResult> DefectsClassify { get; set; } = new Dictionary<string, DefectsResult>(); // (20200429) Jeff Revised!

        /// <summary>
        /// 各類型瑕疵所對應之Priority
        /// </summary>
        [XmlIgnore] // 帶有XmlIgnore，表示序列化時不序列化此屬性
        public Dictionary<int, string> Defects_priority = new Dictionary<int, string>();

        /// <summary>
        /// 瑕疵分類是否啟用
        /// </summary>
        public bool b_Defect_Classify { get; set; } = false;

        /// <summary>
        /// 瑕疵優先權是否啟用
        /// </summary>
        public bool b_Defect_priority { get; set; } = false;

        /// <summary>
        /// 人工覆判是否啟用
        /// </summary>
        public bool b_Defect_Recheck { get; set; } = false;

        /// <summary>
        /// 當前基板之所有已標註cell region (人工覆判)
        /// </summary>
        public HObject all_intersection_cellLabelled_Recheck = null; // (20190708) Jeff Revised!
        /// <summary>
        /// 當前基板之所有已標註cell座標 (人工覆判)
        /// </summary>
        public List<Point> all_cellLabelled_id_Recheck { get; set; } = new List<Point>(); // (20190708) Jeff Revised!

        /// <summary>
        /// 初始化 Defects_priority & DefectsClassify
        /// </summary>
        /// <param name="defectsClassify"></param>
        /// <returns></returns>
        public bool Initialize_DefectsClassify(Dictionary<string, DefectsResult> defectsClassify) // (20190701) Jeff Revised!
        {
            Release_DefectsClassify();

            /* 逐一加入演算法 */
            foreach (string Key in defectsClassify.Keys)
            {
                // 瑕疵優先權啟用時，檢查是否已存在相同Priority之演算法
                //if (b_Defect_priority)
                if (true) // 無論瑕疵優先權啟用與否，皆不可存在相同Priority之演算法
                {
                    int Priority = defectsClassify[Key].Priority;
                    if (Defects_priority.ContainsKey(Priority))
                    {
                        Release_DefectsClassify();
                        return false;
                    }
                    Defects_priority.Add(Priority, Key);
                }

                DefectsClassify.Add(Key, defectsClassify[Key]);
            }

            return true;
        }

        /// <summary>
        /// 初始化瑕疵分類
        /// </summary>
        public void Initialize_DefectsClassify() // (20190702) Jeff Revised!
        {
            foreach (string Key in this.DefectsClassify.Keys)
                this.DefectsClassify[Key].Initialize();
        }

        /// <summary>
        /// 釋放記憶體 & 清空 (For DefectsClassify & Defects_priority)
        /// </summary>
        public void Release_DefectsClassify() // (20190701) Jeff Revised!
        {
            foreach (DefectsResult Value in this.DefectsClassify.Values)
                Value.Release();
            this.DefectsClassify.Clear();
            this.Defects_priority.Clear();
        }

        /// <summary>
        /// 新增瑕疵類型
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Priority"></param>
        /// <param name="Str_Color_Halcon"></param>
        /// <param name="b_CellDefect">是否為Cell瑕疵</param>
        /// <returns></returns>
        public bool Add_New_DefectsClassify(string Name = "", int Priority = 0, string Str_Color_Halcon = "#ff0000ff", bool b_CellDefect = false) // (20190716) Jeff Revised!
        {
            // 檢查是否已存在相同名稱之演算法
            if (this.DefectsClassify.ContainsKey(Name))
                return false;

            // 瑕疵名稱不能為【OK】及【Cell瑕疵】
            if (!b_CellDefect) // (20190716) Jeff Revised!
            {
                if (Name == "OK" || Name == "Cell瑕疵") // (20190715) Jeff Revised!
                    return false;
            }

            // 瑕疵優先權啟用時，檢查是否已存在相同Priority之演算法
            //if (b_Defect_priority)
            if (true) // 無論瑕疵優先權啟用與否
            {
                if (this.Defects_priority.ContainsKey(Priority))
                    return false;
                this.Defects_priority.Add(Priority, Name);
            }

            this.DefectsClassify.Add(Name, DefectsResult.Get_DefectsResult(Name, Priority, Str_Color_Halcon)); // (20190727) Jeff Revised!

            return true;
        }
        
        /// <summary>
        /// 編輯已存在之演算法
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Priority_Origin">原Priority</param>
        /// <param name="Priority_New">新Priority</param>
        /// <param name="Str_Color_Halcon"></param>
        /// <returns></returns>
        public bool Edit_Priority_DefectsClassify(string Name = "", int Priority_Origin = 0, int Priority_New = 0, string Str_Color_Halcon = "#ff0000ff") // (20190701) Jeff Revised!
        {
            // 檢查是否存在此名稱之演算法
            if (!DefectsClassify.ContainsKey(Name))
                return false;

            // 更新 Defects_priority
            if (Priority_Origin != Priority_New)
            {
                // 瑕疵優先權啟用時，檢查是否存在此Priority (Priority_Origin)之演算法 & 新的Priority (Priority_New)不存在於目前演算法中
                //if (b_Defect_priority)
                if (true) // 無論瑕疵優先權啟用與否
                {
                    if (!Defects_priority.ContainsKey(Priority_Origin) || Defects_priority.ContainsKey(Priority_New))
                        return false;

                    // 先刪除，再新增新的Priority (因為Dictionary的Key不能修改)
                    Defects_priority.Remove(Priority_Origin);
                    Defects_priority.Add(Priority_New, Name);
                }
            }

            DefectsResult data = DefectsClassify[Name];
            data.Priority = Priority_New;
            data.Str_Color_Halcon = Str_Color_Halcon;

            // 如果瑕疵優先權啟用 & Priority有修改，則必須更新 DefectsResult
            if (b_Defect_priority && Priority_Origin != Priority_New)
                Update_DefectsResult_Priority();

            return true;
        }

        /// <summary>
        /// 移除演算法
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Priority"></param>
        /// <returns></returns>
        public bool Remove_1_DefectsClassify(string Name = "", int Priority = 0) // (20190701) Jeff Revised!
        {
            // 檢查是否存在此名稱之演算法
            if (!this.DefectsClassify.ContainsKey(Name))
                return false;

            // 瑕疵優先權啟用時，檢查是否存在此Priority之演算法
            //if (b_Defect_priority)
            if (true) // 無論瑕疵優先權啟用與否
            {
                if (!this.Defects_priority.ContainsKey(Priority))
                    return false;
                this.Defects_priority.Remove(Priority);
            }

            this.DefectsClassify[Name].Release(); // 釋放記憶體
            this.DefectsClassify.Remove(Name);

            // 如果瑕疵優先權啟用，則必須更新DefectsResult
            if (this.b_Defect_priority)
                this.Update_DefectsResult_Priority();

            return true;
        }

        /// <summary>
        /// 更新 all_intersection_defect_Priority & all_intersection_defect_Priority_CellReg & all_defect_id_Priority
        /// Note: 瑕疵優先權啟用狀態改變 or 瑕疵優先權啟用且順序改變 皆需執行此演算法
        /// </summary>
        public void Update_DefectsResult_Priority() // (20190715) Jeff Revised!
        {
            if (!b_Defect_priority)
                return;

            /* 瑕疵優先權由小到大排序 */
            Dictionary<int, string> Defects_priority_order = Defects_priority.OrderBy(o => o.Key).ToDictionary(o => o.Key, p => p.Value);
            //Defects_priority = Defects_priority.OrderBy(o => o.Key).ToDictionary(o => o.Key, p => p.Value);
            
            HObject exist_region;
            HOperatorSet.GenEmptyRegion(out exist_region);
            foreach (string Name in Defects_priority_order.Values)
            {
                DefectsResult dr = DefectsClassify[Name];
                dr.Release(false, true, false);
                HOperatorSet.Difference(dr.all_intersection_defect_Origin, exist_region, out dr.all_intersection_defect_Priority);
                HObject reg_NoUse;
                HOperatorSet.GenEmptyRegion(out reg_NoUse);
                HTuple cell_ids = new HTuple();
                dr.all_defect_id_Priority.Clear();
                this.label_region_1_Defect(-1, dr.all_intersection_defect_Priority, reg_NoUse, out reg_NoUse, out cell_ids,
                                           dr.all_defect_id_Priority, out dr.all_defect_id_Priority, true);
                reg_NoUse.Dispose();

                // 更新 all_intersection_defect_Priority_CellReg (20190715) Jeff Revised!
                HOperatorSet.SelectObj(this.cellmap_affine_, out dr.all_intersection_defect_Priority_CellReg, cell_ids);

                // 更新 exist_region
                HOperatorSet.Union2(exist_region, dr.all_intersection_defect_Priority_CellReg, out exist_region);
            }
            exist_region.Dispose();
        }

        /// <summary>
        /// 初始化人工覆判
        /// </summary>
        public void Initialize_Recheck() // (20190711) Jeff Revised!
        {
            if (!this.b_Defect_Recheck) // 人工覆判不啟用
                return;

            // 初始化總瑕疵
            {
                if (this.all_intersection_defect_ == null)
                    HOperatorSet.GenEmptyRegion(out this.all_intersection_defect_);

                this.all_defect_id_Recheck = this.all_defect_id_.ToList();

                // 設定 all_intersection_defect_Recheck 為瑕疵所包括之 Cell region
                Extension.HObjectMedthods.ReleaseHObject(ref this.all_intersection_defect_Recheck);
                this.all_intersection_defect_Recheck = this.Compute_CellReg_ListCellID(this.all_defect_id_Recheck);
            }

            if (!this.b_Defect_Classify) // 單一瑕疵
                ; // 僅需設定總瑕疵
            else // 多瑕疵
            {
                foreach (DefectsResult dr in this.DefectsClassify.Values)
                {
                    dr.Release(false, false, true);

                    if (!this.b_Defect_priority) // 瑕疵優先權不啟用
                        dr.all_defect_id_Recheck = dr.all_defect_id_Origin.ToList();
                    else // 瑕疵優先權啟用
                        dr.all_defect_id_Recheck = dr.all_defect_id_Priority.ToList();

                    // 設定 all_intersection_defect_Recheck 為瑕疵所包括之 Cell region
                    dr.all_intersection_defect_Recheck = this.Compute_CellReg_ListCellID(dr.all_defect_id_Recheck);
                }
            }

            // 初始化已標註Cell
            Extension.HObjectMedthods.ReleaseHObject(ref this.all_intersection_cellLabelled_Recheck);
            HOperatorSet.GenEmptyRegion(out this.all_intersection_cellLabelled_Recheck);
            this.all_cellLabelled_id_Recheck.Clear();
            this.all_cellLabelled_id_Recheck = new List<Point>();
        }

        /// <summary>
        /// 更新人工覆判結果 【適用於: 單一/多瑕疵 or 多顆標註模式 or 單顆標註模式之標註NG】
        /// Note 1: ListCellID_OK 和 ListCellID_NG 不能有任何座標相同
        /// Note 2: 如果瑕疵優先權啟用，則以使用者最後標註之瑕疵為該Cell最終之瑕疵
        /// </summary>
        /// <param name="ListCellID_OK">使用者有標註之OK座標 (無可輸入null)</param>
        /// <param name="ListCellID_NG">使用者有標註之NG座標 (無可輸入null)</param>
        /// <param name="Name">瑕疵名稱</param>
        public void Update_Recheck(List<Point> ListCellID_OK = null, List<Point> ListCellID_NG = null, string Name = "") // (20200429) Jeff Revised!
        {
            if (!b_Defect_Recheck) // 人工覆判不啟用
                return;

            // 更新總瑕疵 (20190711) Jeff Revised!
            Update_all_defect_IdRegion_Recheck(ListCellID_OK, ListCellID_NG, ref all_defect_id_Recheck, ref all_intersection_defect_Recheck);

            if (!b_Defect_Classify) // 單一瑕疵
                ; // 僅需設定總瑕疵
            else // 多瑕疵
            {
                if (!b_Defect_priority) // 瑕疵優先權不啟用
                {
                    // 只更新該瑕疵
                    Update_all_defect_IdRegion_Recheck(ListCellID_OK, ListCellID_NG, ref DefectsClassify[Name].all_defect_id_Recheck, ref DefectsClassify[Name].all_intersection_defect_Recheck);
                }
                else // 瑕疵優先權啟用
                {
                    // 更新該瑕疵
                    List<Point> new_ListCellID_NG = Update_all_defect_IdRegion_Recheck(ListCellID_OK, ListCellID_NG, ref DefectsClassify[Name].all_defect_id_Recheck, ref DefectsClassify[Name].all_intersection_defect_Recheck);

                    // 更新其他瑕疵 (皆不能含新增之瑕疵座標 new_ListCellID_NG) (參 Note 2)
                    foreach (string name in DefectsClassify.Keys)
                    {
                        if (name == Name)
                            continue;

                        // i.e. 將 new_ListCellID_NG 對其他瑕疵類型設定為 OK
                        Update_all_defect_IdRegion_Recheck(new_ListCellID_NG, null, ref DefectsClassify[name].all_defect_id_Recheck, ref DefectsClassify[name].all_intersection_defect_Recheck); // (20200429) Jeff Revised!
                    }

                }
            }
        }

        /// <summary>
        /// 更新人工覆判結果 (單一瑕疵)
        /// </summary>
        /// <param name="ListCellID_OK">使用者有標註之OK座標 (無可輸入null)</param>
        /// <param name="ListCellID_NG">使用者有標註之NG座標 (無可輸入null)</param>
        /// <param name="all_def_id_Recheck"></param>
        /// <param name="all_inter_def_Recheck"></param>
        /// <returns>ListCellID_NG中，原先不存在之座標</returns>
        public List<Point> Update_all_defect_IdRegion_Recheck(List<Point> ListCellID_OK, List<Point> ListCellID_NG, ref List<Point> all_def_id_Recheck, ref HObject all_inter_def_Recheck) // (20190708) Jeff Revised!
        {
            List<Point> new_ListCellID_NG = new List<Point>(); // 新加入之NG
            bool b_updateRegion = false;

            // 更新座標
            if (ListCellID_OK != null)
            {
                foreach (Point pt in ListCellID_OK)
                {
                    if (all_def_id_Recheck.Contains(pt)) // 原本為NG
                    {
                        b_updateRegion = true;
                        all_def_id_Recheck.Remove(pt);
                    }

                    // 更新已標註Cell
                    Update_cellLabelled_Recheck(pt);
                }
            }
            if (ListCellID_NG != null)
            {
                foreach (Point pt in ListCellID_NG)
                {
                    if (!all_def_id_Recheck.Contains(pt)) // 原本為OK
                    {
                        b_updateRegion = true;
                        all_def_id_Recheck.Add(pt);
                        new_ListCellID_NG.Add(pt);
                    }

                    // 更新已標註Cell
                    Update_cellLabelled_Recheck(pt);
                }
            }

            // 更新 Region
            if (b_updateRegion)
            {
                Extension.HObjectMedthods.ReleaseHObject(ref all_inter_def_Recheck);
                all_inter_def_Recheck = Compute_CellReg_ListCellID(all_def_id_Recheck);
            }

            return new_ListCellID_NG;
        }

        /// <summary>
        /// 更新人工覆判結果 (每種瑕疵皆判為OK) 【適用於: 單顆標註模式之標註OK】
        /// </summary>
        /// <param name="ListCellID_OK">使用者有標註之OK座標 (無可輸入null)</param>
        public void Update_All_OK_Recheck(List<Point> ListCellID_OK = null) // (20200429) Jeff Revised!
        {
            if (!b_Defect_Recheck) // 人工覆判不啟用
                return;

            if (ListCellID_OK == null)
                return;

            /* 更新總瑕疵 */
            Update_all_defect_IdRegion_Recheck(ListCellID_OK, null, ref all_defect_id_Recheck, ref all_intersection_defect_Recheck); // (20200429) Jeff Revised!

            if (!b_Defect_Classify) // 單一瑕疵
                ; // 僅需設定總瑕疵
            else // 多瑕疵
            {
                foreach (DefectsResult dr in DefectsClassify.Values)
                    Update_all_defect_IdRegion_Recheck(ListCellID_OK, null, ref dr.all_defect_id_Recheck, ref dr.all_intersection_defect_Recheck); // (20200429) Jeff Revised!
            }

        }

        /// <summary>
        /// 更新已標註Cell
        /// </summary>
        /// <param name="CellID"></param>
        public void Update_cellLabelled_Recheck(Point CellID) // (20190708) Jeff Revised!
        {
            if (!all_cellLabelled_id_Recheck.Contains(CellID))
            {
                // 新增此顆Cell
                all_cellLabelled_id_Recheck.Add(CellID);
                Extension.HObjectMedthods.ReleaseHObject(ref all_intersection_cellLabelled_Recheck);
                all_intersection_cellLabelled_Recheck = Compute_CellReg_ListCellID(all_cellLabelled_id_Recheck);
            }
        }

        #region GUI

        /// <summary>
        /// 更新 listView_Edit
        /// </summary>
        /// <param name="ListView_Edit"></param>
        /// <param name="RadioButton_Result">顯示【檢測結果】</param>
        /// <param name="RadioButton_Recheck">顯示【人工覆判結果】</param>
        /// <param name="Datas"></param>
        public void Update_listView_Edit(ListView ListView_Edit, RadioButton RadioButton_Result, RadioButton RadioButton_Recheck,
                                         Dictionary<string, DefectsResult> Datas = null) // (20190711) Jeff Revised!
        {
            if (RadioButton_Result.Checked) // 顯示檢測結果
                ListView_Edit.ForeColor = System.Drawing.SystemColors.WindowText;
            else if (RadioButton_Recheck.Checked) // 顯示人工覆判結果
                ListView_Edit.ForeColor = Color.DarkViolet;

            ListView_Edit.BeginUpdate();
            ListView_Edit.Items.Clear();

            if (Datas == null)
                Datas = this.DefectsClassify;

            foreach (KeyValuePair<string, DefectsResult> item in Datas)
            {
                ListViewItem lvi = new ListViewItem(item.Key);
                lvi.SubItems.Add(item.Value.Priority.ToString());
                lvi.UseItemStyleForSubItems = false;
                lvi.SubItems.Add(item.Value.Str_Color_Halcon, Color.Black, item.Value.GetColor(), new Font("細明體", (float)8.25, FontStyle.Regular));

                ListView_Edit.Items.Add(lvi);
            }

            ListView_Edit.EndUpdate();
        }

        /// <summary>
        /// 更新 listView_Result
        /// </summary>
        /// <param name="ListView_Result"></param>
        /// <param name="RadioButton_Result">顯示【檢測結果】</param>
        /// <param name="RadioButton_Recheck">顯示【人工覆判結果】</param>
        /// <param name="Datas"></param>
        public void Update_listView_Result(ListView ListView_Result, RadioButton RadioButton_Result, RadioButton RadioButton_Recheck,
                                           Dictionary<string, DefectsResult> Datas = null) // (20190711) Jeff Revised!
        {
            if (RadioButton_Result.Checked) // 顯示檢測結果
                ListView_Result.ForeColor = System.Drawing.SystemColors.WindowText;
            else if (RadioButton_Recheck.Checked) // 顯示人工覆判結果
                ListView_Result.ForeColor = Color.DarkViolet;

            ListView_Result.BeginUpdate();
            ListView_Result.Items.Clear();

            if (Datas == null)
                Datas = this.DefectsClassify;

            foreach (KeyValuePair<string, DefectsResult> item in Datas)
            {
                ListViewItem lvi = new ListViewItem(item.Key);

                int count_defect = 0;
                if (RadioButton_Result.Checked) // 顯示檢測結果
                {
                    if (b_Defect_priority)
                        count_defect = item.Value.all_defect_id_Priority.Count;
                    else
                        count_defect = item.Value.all_defect_id_Origin.Count;
                }
                else if (RadioButton_Recheck.Checked) // 顯示人工覆判結果
                    count_defect = item.Value.all_defect_id_Recheck.Count;

                lvi.SubItems.Add(count_defect.ToString());
                lvi.SubItems.Add((100.0 - 100.0 * count_defect / get_total_cell_count()).ToString("#0.00"));

                ListView_Result.Items.Add(lvi);
            }

            ListView_Result.EndUpdate();
        }

        /// <summary>
        /// 更新顯示瑕疵 (大拼接圖) (hSmartWindowControl_map)
        /// </summary>
        /// <param name="HSmartWindowControl_map"></param>
        /// <param name="RadioButton_Result"></param>
        /// <param name="RadioButton_Recheck"></param>
        /// <param name="RadioButton_fill"></param>
        /// <param name="RadioButton_margin"></param>
        /// <param name="ListName">欲顯示之瑕疵名稱 (單一瑕疵則輸入null)(多瑕疵顯示所有瑕疵也輸入null)</param>
        /// <param name="b_SetPart">是否做 SetPart </param>
        /// <param name="Type_DispResult">1 (顯示原始 Region), 2 (顯示 Cell Region) {For 顯示檢測結果} </param>
        /// <param name="b_ClearWindow">是否做 ClearWindow </param>
        public void Disp_DefectTest(HSmartWindowControl HSmartWindowControl_map, RadioButton RadioButton_Result, RadioButton RadioButton_Recheck,
                                    RadioButton RadioButton_fill, RadioButton RadioButton_margin, List<string> ListName = null, bool b_SetPart = true, 
                                    int Type_DispResult = 2, bool b_ClearWindow = true) // (20200429) Jeff Revised!
        {
            try
            {
                if (this.TileImage == null)
                {
                    HOperatorSet.ClearWindow(HSmartWindowControl_map.HalconWindow); // (20200429) Jeff Revised!
                    return;
                }

                // 更新 HSmartWindowControl_map
                if (b_ClearWindow)
                {
                    HOperatorSet.ClearWindow(HSmartWindowControl_map.HalconWindow);
                    HOperatorSet.DispObj(this.TileImage, HSmartWindowControl_map.HalconWindow);

                    // (20200429) Jeff Revised!
                    HOperatorSet.SetDraw(HSmartWindowControl_map.HalconWindow, "margin");
                    HOperatorSet.SetColor(HSmartWindowControl_map.HalconWindow, "green");
                    HOperatorSet.DispObj(this.cellmap_affine_, HSmartWindowControl_map.HalconWindow);
                }

                if (b_SetPart)
                    HOperatorSet.SetPart(HSmartWindowControl_map.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part
                
                if (RadioButton_fill.Checked) // 填滿
                    HOperatorSet.SetDraw(HSmartWindowControl_map.HalconWindow, "fill");
                else if (RadioButton_margin.Checked) // 邊緣
                    HOperatorSet.SetDraw(HSmartWindowControl_map.HalconWindow, "margin");
                else // 原圖
                    return;

                if (!this.b_Defect_Classify) // 單一瑕疵
                {
                    HOperatorSet.SetColor(HSmartWindowControl_map.HalconWindow, "red");


                    if (RadioButton_Result.Checked) // 顯示檢測結果
                    {
                        HOperatorSet.DispObj(this.all_intersection_defect_, HSmartWindowControl_map.HalconWindow);
                        if (Type_DispResult == 1) // (20200429) Jeff Revised!
                            HOperatorSet.DispObj(this.all_intersection_defect_, HSmartWindowControl_map.HalconWindow);
                        else if (Type_DispResult == 2)
                            HOperatorSet.DispObj(this.all_intersection_defect_CellReg, HSmartWindowControl_map.HalconWindow);
                    }
                    else if (RadioButton_Recheck.Checked) // 顯示人工覆判結果
                        HOperatorSet.DispObj(this.all_intersection_defect_Recheck, HSmartWindowControl_map.HalconWindow);
                }
                else // 多瑕疵
                {
                    if (ListName == null) // (20200429) Jeff Revised!
                    {
                        ListName = new List<string>();
                        foreach (string name in this.DefectsClassify.Keys)
                            ListName.Add(name);
                    }

                    foreach (string name in ListName)
                    {
                        DefectsResult dr = DefectsClassify[name];
                        HOperatorSet.SetColor(HSmartWindowControl_map.HalconWindow, dr.Str_Color_Halcon);
                        if (RadioButton_Result.Checked) // 顯示檢測結果
                        {
                            if (Type_DispResult == 1)
                            {
                                if (this.b_Defect_priority)
                                    HOperatorSet.DispObj(dr.all_intersection_defect_Priority, HSmartWindowControl_map.HalconWindow);
                                else
                                    HOperatorSet.DispObj(dr.all_intersection_defect_Origin, HSmartWindowControl_map.HalconWindow);
                            }
                            else if (Type_DispResult == 2) // (20190715) Jeff Revised!
                            {
                                if (this.b_Defect_priority)
                                    HOperatorSet.DispObj(dr.all_intersection_defect_Priority_CellReg, HSmartWindowControl_map.HalconWindow);
                                else
                                    HOperatorSet.DispObj(dr.all_intersection_defect_Origin_CellReg, HSmartWindowControl_map.HalconWindow);
                            }

                        }
                        else if (RadioButton_Recheck.Checked) // 顯示人工覆判結果
                            HOperatorSet.DispObj(dr.all_intersection_defect_Recheck, HSmartWindowControl_map.HalconWindow);
                    }
                }

                if (RadioButton_Recheck.Checked) // 顯示人工覆判結果
                {
                    // 顯示 Cell region
                    //if (b_ClearWindow)
                    //{
                    //    HOperatorSet.SetDraw(HSmartWindowControl_map.HalconWindow, "margin");
                    //    HOperatorSet.SetColor(HSmartWindowControl_map.HalconWindow, "green");
                    //    HOperatorSet.DispObj(this.cellmap_affine_, HSmartWindowControl_map.HalconWindow);
                    //}

                    // 顯示此位置已標註Cell
                    if (this.all_intersection_cellLabelled_Recheck != null) // (20190817) Jeff Revised!
                    {
                        HOperatorSet.SetDraw(HSmartWindowControl_map.HalconWindow, "margin");
                        HOperatorSet.SetColor(HSmartWindowControl_map.HalconWindow, "#9400D3FF"); // DarkViolet
                        HTuple line_w;
                        HOperatorSet.GetLineWidth(HSmartWindowControl_map.HalconWindow, out line_w);
                        HOperatorSet.SetLineWidth(HSmartWindowControl_map.HalconWindow, 5);
                        HOperatorSet.DispObj(this.all_intersection_cellLabelled_Recheck, HSmartWindowControl_map.HalconWindow);
                        HOperatorSet.SetLineWidth(HSmartWindowControl_map.HalconWindow, line_w);
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        public static void Disp_DefectTest(HSmartWindowControl hWindow, LocateMethod locate_method, ListView ListView_Edit, RadioButton RadioButton_Result, RadioButton RadioButton_Recheck, RadioButton RadioButton_fill, RadioButton RadioButton_margin, bool b_ClearWindow = true) // (20200429) Jeff Revised!
        {
            if (locate_method == null) // (20200429) Jeff Revised!
                return;
                
            if (locate_method.b_Defect_Classify) // 多瑕疵
            {
                List<string> ListName = new List<string>();
                if (ListView_Edit != null && ListView_Edit.SelectedIndices.Count > 0) // (20200429) Jeff Revised!
                    ListName.Add(ListView_Edit.SelectedItems[0].Text);
                else
                {
                    foreach (string name in locate_method.DefectsClassify.Keys)
                        ListName.Add(name);
                }
                locate_method.Disp_DefectTest(hWindow, RadioButton_Result, RadioButton_Recheck,
                                              RadioButton_fill, RadioButton_margin, ListName, false, 2, b_ClearWindow);
            }
            else // 單一瑕疵
            {
                locate_method.Disp_DefectTest(hWindow, RadioButton_Result, RadioButton_Recheck,
                                              RadioButton_fill, RadioButton_margin, null, false, 2, b_ClearWindow);
            }
        }

        /// <summary>
        /// 更新顯示瑕疵 (人工覆判) (hSmartWindowControl_map)
        /// </summary>
        /// <param name="HSmartWindowControl_map"></param>
        /// <param name="image_MoveIndex"></param>
        /// <param name="cellReg_MoveIndex"></param>
        /// <param name="defectReg_MoveIndex"></param>
        /// <param name="defectName_select"></param>
        /// <param name="CellLabelled_MoveIndex"></param>
        /// <param name="b_SetPart">是否做 SetPart </param>
        public void Disp_Recheck(HSmartWindowControl HSmartWindowControl_map, HObject image_MoveIndex, HObject cellReg_MoveIndex,
                                 List<HObject> defectReg_MoveIndex, List<string> defectName_select, HObject CellLabelled_MoveIndex = null, bool b_SetPart = true) // (20190713) Jeff Revised!
        {
            try
            {
                HOperatorSet.ClearWindow(HSmartWindowControl_map.HalconWindow);
                HOperatorSet.DispObj(image_MoveIndex, HSmartWindowControl_map.HalconWindow);
                if (b_SetPart)
                    HOperatorSet.SetPart(HSmartWindowControl_map.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part

                // 顯示 Cell region
                HOperatorSet.SetDraw(HSmartWindowControl_map.HalconWindow, "margin");
                HOperatorSet.SetColor(HSmartWindowControl_map.HalconWindow, "green");
                HOperatorSet.DispObj(cellReg_MoveIndex, HSmartWindowControl_map.HalconWindow);

                // 顯示該瑕疵region
                if (!b_Defect_Classify) // 單一瑕疵
                {
                    HOperatorSet.SetDraw(HSmartWindowControl_map.HalconWindow, "fill");
                    HOperatorSet.SetColor(HSmartWindowControl_map.HalconWindow, "red");
                    HOperatorSet.DispObj(defectReg_MoveIndex[0], HSmartWindowControl_map.HalconWindow);
                }
                else // 多瑕疵
                {
                    HOperatorSet.SetDraw(HSmartWindowControl_map.HalconWindow, "fill");
                    for (int i = 0; i < defectName_select.Count; i++)
                    {
                        HOperatorSet.SetColor(HSmartWindowControl_map.HalconWindow, this.DefectsClassify[defectName_select[i]].Str_Color_Halcon);
                        HOperatorSet.DispObj(defectReg_MoveIndex[i], HSmartWindowControl_map.HalconWindow);
                    }
                }

                // 顯示此位置已標註Cell
                if (CellLabelled_MoveIndex != null)
                {
                    HOperatorSet.SetDraw(HSmartWindowControl_map.HalconWindow, "margin");
                    HOperatorSet.SetColor(HSmartWindowControl_map.HalconWindow, "#9400D3FF"); // DarkViolet
                    HTuple line_w;
                    HOperatorSet.GetLineWidth(HSmartWindowControl_map.HalconWindow, out line_w);
                    HOperatorSet.SetLineWidth(HSmartWindowControl_map.HalconWindow, 5);
                    HOperatorSet.DispObj(CellLabelled_MoveIndex, HSmartWindowControl_map.HalconWindow);
                    HOperatorSet.SetLineWidth(HSmartWindowControl_map.HalconWindow, line_w);
                }

            }
            catch (Exception ex)
            { }
        }

        /// <summary>
        /// 更新顯示瑕疵 (人工覆判) (hSmartWindowControl_map)
        /// </summary>
        /// <param name="HSmartWindowControl_map"></param>
        /// <param name="image_MoveIndex"></param>
        /// <param name="cellReg_MoveIndex"></param>
        /// <param name="Dic_defectReg_MoveIndex"></param>
        /// <param name="defectName_select"></param>
        /// <param name="CellLabelled_MoveIndex"></param>
        /// <param name="b_SetPart">是否做 SetPart </param>
        public void Disp_Recheck(HSmartWindowControl HSmartWindowControl_map, HObject image_MoveIndex, HObject cellReg_MoveIndex,
                                 Dictionary<string, HObject> Dic_defectReg_MoveIndex, List<string> defectName_select, HObject CellLabelled_MoveIndex = null, bool b_SetPart = true) // (20190716) Jeff Revised!
        {
            try
            {
                HOperatorSet.ClearWindow(HSmartWindowControl_map.HalconWindow);
                HOperatorSet.DispObj(image_MoveIndex, HSmartWindowControl_map.HalconWindow);
                if (b_SetPart)
                    HOperatorSet.SetPart(HSmartWindowControl_map.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part

                // 顯示 Cell region
                HOperatorSet.SetDraw(HSmartWindowControl_map.HalconWindow, "margin");
                HOperatorSet.SetColor(HSmartWindowControl_map.HalconWindow, "green");
                HOperatorSet.DispObj(cellReg_MoveIndex, HSmartWindowControl_map.HalconWindow);

                // 顯示該瑕疵region
                if (!b_Defect_Classify) // 單一瑕疵
                {
                    HOperatorSet.SetDraw(HSmartWindowControl_map.HalconWindow, "fill");
                    HOperatorSet.SetColor(HSmartWindowControl_map.HalconWindow, "red");
                    HOperatorSet.DispObj(Dic_defectReg_MoveIndex["NG"], HSmartWindowControl_map.HalconWindow);
                }
                else // 多瑕疵
                {
                    HOperatorSet.SetDraw(HSmartWindowControl_map.HalconWindow, "fill");
                    for (int i = 0; i < defectName_select.Count; i++)
                    {
                        HOperatorSet.SetColor(HSmartWindowControl_map.HalconWindow, this.DefectsClassify[defectName_select[i]].Str_Color_Halcon);
                        HOperatorSet.DispObj(Dic_defectReg_MoveIndex[defectName_select[i]], HSmartWindowControl_map.HalconWindow);
                    }
                }

                // 顯示此位置已標註Cell
                if (CellLabelled_MoveIndex != null)
                {
                    HOperatorSet.SetDraw(HSmartWindowControl_map.HalconWindow, "margin");
                    HOperatorSet.SetColor(HSmartWindowControl_map.HalconWindow, "#9400D3FF"); // DarkViolet
                    HTuple line_w;
                    HOperatorSet.GetLineWidth(HSmartWindowControl_map.HalconWindow, out line_w);
                    HOperatorSet.SetLineWidth(HSmartWindowControl_map.HalconWindow, 5);
                    HOperatorSet.DispObj(CellLabelled_MoveIndex, HSmartWindowControl_map.HalconWindow);
                    HOperatorSet.SetLineWidth(HSmartWindowControl_map.HalconWindow, line_w);
                }

            }
            catch (Exception ex)
            { }
        }

        #endregion

        #endregion
        
        /// <summary>
        /// 移動路徑之X軸機械座標 轉換成 視覺座標
        /// </summary>
        [XmlIgnore] // 帶有XmlIgnore，表示序列化時不序列化此屬性 (Note: HTuple不能序列化)
        public HTuple center_points_x_ { get; set; } = new HTuple(); // (20190402) Jeff Revised!
        /// <summary>
        /// 用於序列化 (center_points_x_)
        /// </summary>
        public int[] center_points_x_IArr // (20200429) Jeff Revised!
        {
            get // 序列化
            {
                return this.center_points_x_.IArr;
            }

            set // 反序列化
            {
                this.center_points_x_.IArr = value;
            }
        }

        /// <summary>
        /// 移動路徑之Y軸機械座標 轉換成 視覺座標
        /// </summary>
        [XmlIgnore] // 帶有XmlIgnore，表示序列化時不序列化此屬性 (Note: HTuple不能序列化)
        public HTuple center_points_y_ { get; set; } = new HTuple(); // (20190402) Jeff Revised!
        /// <summary>
        /// 用於序列化 (center_points_y_)
        /// </summary>
        public int[] center_points_y_IArr // (20200429) Jeff Revised!
        {
            get // 序列化
            {
                return this.center_points_y_.IArr;
            }

            set // 反序列化
            {
                this.center_points_y_.IArr = value;
            }
        }

        double Mark_pdx_ = -1, Mark_pdy_ = -1;
        /// <summary>
        /// tile_images_offset所需要之ROI
        /// </summary>
        [XmlIgnore] // 帶有XmlIgnore，表示序列化時不序列化此屬性 (Note: HTuple不能序列化)
        public HTuple tile_rect_ = new HTuple();
        /// <summary>
        /// 第一顆cell之原始位置 (For 第一張檢測影像)
        /// </summary>
        HTuple origin_fc_x_, origin_fc_y_;
        
        [NonSerialized] // (20190824) Jeff Revised!
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch(); // 引用stopwatch物件做計時 (20181116) Jeff Revised!
        private double elapseTime { get; set; } = -1; // (20181116) Jeff Revised!
        
        public LocateMethod()
        {
            LocateMethod_Constructor(); // (20200429) Jeff Revised!
        }

        /// <summary>
        /// 代替 建構子，初始化 之功能 
        /// Note: 建構式有參數輸入時，會無法儲存到XML!
        /// </summary>
        public void LocateMethod_Constructor() // (20200429) Jeff Revised!
        {
            // Convert string into HTuple (20180904) Jeff Revised!
            string_2_HTuple(cell_x_count_string, out cell_x_count_HTuple, true);
            string_2_HTuple(cell_y_count_string, out cell_y_count_HTuple, true);
            string_2_HTuple(dist_region_rdx_string, out dist_region_rdx_HTuple);
            string_2_HTuple(dist_region_rdy_string, out dist_region_rdy_HTuple);
        }

        /// <summary>
        /// Resize parameters
        /// </summary>
        /// <returns></returns>
        public bool resize_parameters() // (20180828) Jeff Revised!
        {
            if (!b_resizeParam)
            {
                // Resize parameters
                frame_pwidth_ = (int)(frame_pwidth_ * resize_);
                frame_pheight_ = (int)(frame_pheight_ * resize_);
                size_sample_rx *= resize_;
                size_sample_ry *= resize_;
                mark1_rpos_x_ *= resize_;
                mark1_rpos_y_ *= resize_;
                mark2_rpos_x_ *= resize_;
                mark2_rpos_y_ *= resize_;
                start_rpos_x_ *= resize_;
                start_rpos_y_ *= resize_;
                array_x_rpitch_ *= resize_;
                array_y_rpitch_ *= resize_;
                mark_rdx_ *= resize_;
                mark_rdy_ *= resize_;
                mark_rdx_shift *= resize_;
                mark_rdy_shift *= resize_;
                cell_rwidth_ *= resize_;
                cell_rheight_ *= resize_;
                cell_rdx_ *= resize_;
                cell_rdy_ *= resize_;

                b_resizeParam = true;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Resize parameters back to the original size
        /// </summary>
        /// <returns></returns>
        public bool resize_parameters_back() // (20180828) Jeff Revised!
        {
            if (b_resizeParam)
            {
                // Resize parameters back to the original size
                frame_pwidth_ = (int)(frame_pwidth_ / resize_);
                frame_pheight_ = (int)(frame_pheight_ / resize_);
                size_sample_rx /= resize_;
                size_sample_ry /= resize_;
                mark1_rpos_x_ /= resize_;
                mark1_rpos_y_ /= resize_;
                mark2_rpos_x_ /= resize_;
                mark2_rpos_y_ /= resize_;
                start_rpos_x_ /= resize_;
                start_rpos_y_ /= resize_;
                array_x_rpitch_ /= resize_;
                array_y_rpitch_ /= resize_;
                mark_rdx_ /= resize_;
                mark_rdy_ /= resize_;
                mark_rdx_shift /= resize_;
                mark_rdy_shift /= resize_;
                cell_rwidth_ /= resize_;
                cell_rheight_ /= resize_;
                cell_rdx_ /= resize_;
                cell_rdy_ /= resize_;

                b_resizeParam = false;
                return true;
            }

            return false;
        }

        /// <summary>
        ///  單位轉換 (mm → pixel)
        /// </summary>
        /// <returns></returns>
        public bool unitTrans_mm2pixel() // (20180828) Jeff Revised!
        {
            Mark_pdx_ = (mark_rdx_ + mark_rdx_shift) * 1000 / pixel_resolution_;
            Mark_pdy_ = (mark_rdy_ + mark_rdy_shift) * 1000 / pixel_resolution_;
            cell_pwidth_ = cell_rwidth_ * 1000 / pixel_resolution_;
            cell_pheight_ = cell_rheight_ * 1000 / pixel_resolution_;
            cell_pdx_ = cell_rdx_ * 1000 / pixel_resolution_;
            cell_pdy_ = cell_rdy_ * 1000 / pixel_resolution_;

            return true;
        }

        /// <summary>
        /// 將 輸入字串 轉成 HTuple
        /// </summary>
        /// <param name="str_in">輸入字串，各元素用逗號","分隔</param>
        /// <param name="HTuple_out"></param>
        /// <param name="b_int">數值是否為整數</param>
        /// <returns></returns>
        public bool string_2_HTuple(string str_in, out HTuple HTuple_out, bool b_int = false) // (20180903) Jeff Revised!
        {
            try
            {
                if (str_in.Length == 0)
                {
                    HTuple_out = null;
                    return false;
                }

                // str_in 轉成 HTuple_out
                string str_in_rest = string.Copy(str_in);
                HTuple_out = new HTuple();
                while (str_in_rest.Length != 0)
                {
                    int Index = str_in_rest.IndexOf(',');
                    string str_temp;
                    if (Index == -1)
                    {
                        str_temp = string.Copy(str_in_rest);
                        // Update str_in_rest
                        str_in_rest = "";
                    }
                    else
                    {
                        str_temp = str_in_rest.Substring(0, Index);
                        // Update str_in_rest
                        str_in_rest = str_in_rest.Substring(Index + 1);
                    }

                    // 判斷是 數值 (整數, 浮點數) or 字串
                    double d;
                    if (double.TryParse(str_temp, out d)) // 數值
                    {
                        if (b_int)
                            HOperatorSet.TupleConcat(HTuple_out, (int)d, out HTuple_out);
                        else
                            HOperatorSet.TupleConcat(HTuple_out, d, out HTuple_out);
                    }
                    else // 字串
                        HOperatorSet.TupleConcat(HTuple_out, str_temp, out HTuple_out);
                }
            }
            catch
            {
                HTuple_out = null;
                return false;
            }

            return true;
        }

        /// <summary>
        /// 將 輸入HTuple 轉成 單一字串
        /// </summary>
        /// <param name="HTuple_in"></param>
        /// <param name="str_out"></param>
        /// <returns></returns>
        public bool HTuple_2_string(HTuple HTuple_in, out string str_out) // (20180903) Jeff Revised!
        {
            try
            {
                // 轉成 單一字串
                str_out = "";
                HTuple n;
                HOperatorSet.TupleLength(HTuple_in, out n);
                for (int i = 0; i < n; i++)
                {
                    str_out += HTuple_in[i];
                    if (i < n - 1)
                        str_out += ",";
                }
            }
            catch
            {
                str_out = "";
                return false;
            }

            return true;
        }

        /// <summary>
        /// 更新 num_region_x_
        /// </summary>
        /// <param name="count_region_x"></param>
        public void Update_count_region_x(int count_region_x = 1) // (20191108) Jeff Revised!
        {
            this.num_region_x_ = count_region_x;

            int[] count_cell_x = new int[count_region_x];
            cell_x_count_HTuple = new HTuple(count_cell_x);
            HTuple_2_string(cell_x_count_HTuple, out cell_x_count_string);

            double[] dist_region_rdx = new double[count_region_x];
            dist_region_rdx_HTuple = new HTuple(dist_region_rdx);
            HTuple_2_string(dist_region_rdx_HTuple, out dist_region_rdx_string);
        }

        /// <summary>
        /// 更新 num_region_y_
        /// </summary>
        /// <param name="count_region_y"></param>
        public void Update_count_region_y(int count_region_y = 1) // (20191108) Jeff Revised!
        {
            this.num_region_y_ = count_region_y;

            int[] count_cell_y = new int[count_region_y];
            cell_y_count_HTuple = new HTuple(count_cell_y);
            HTuple_2_string(cell_y_count_HTuple, out cell_y_count_string);

            double[] dist_region_rdy = new double[count_region_y];
            dist_region_rdy_HTuple = new HTuple(dist_region_rdy);
            HTuple_2_string(dist_region_rdy_HTuple, out dist_region_rdy_string);
        }

        /// <summary>
        /// 從.xml檔讀取變數
        /// </summary>
        /// <returns></returns>
        public bool load() // (20200429) Jeff Revised!
        {
            bool b_status_ = false;
            // 是否有此Type之基板資料夾
            File_directory_ = ModuleParamDirectory + PositionParam + "\\" + board_type_;
            if (!Directory.Exists(File_directory_))
                Directory.CreateDirectory(File_directory_);

            try
            {
                XmlDocument xml_doc_ = new XmlDocument();
                //xml_doc_.Load(ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\Label Defect.xml");
                if (!System.IO.File.Exists(ModuleParamDirectory + PositionParam + "\\Label Defect.xml"))
                    return false;
                else
                    xml_doc_.Load(ModuleParamDirectory + PositionParam + "\\Label Defect.xml");
               
                XmlNode xml_node_ = xml_doc_.SelectSingleNode("Board_Type/Type_" + board_type_);
                if (xml_node_ == null)
                    return false;
                else
                {
                    // 更新參數
                    XmlElement xml_ele_ = (XmlElement)xml_node_.SelectSingleNode("Label_Defect");
                    //save_directory_ = xml_ele_.GetAttribute("Save_Cell_Map_Directory");
                    //cellmap_region_directory_ = xml_ele_.GetAttribute("Load_Cell_Map_Abs_Directory");
                    pixel_resolution_ = Convert.ToDouble(xml_ele_.GetAttribute("Pixel_Resolution"));
                    frame_pwidth_ = Convert.ToInt32(xml_ele_.GetAttribute("Frame_Width"));
                    frame_pheight_ = Convert.ToInt32(xml_ele_.GetAttribute("Frame_Height"));
                    size_sample_rx = Convert.ToDouble(xml_ele_.GetAttribute("size_sample_rx"));
                    size_sample_ry = Convert.ToDouble(xml_ele_.GetAttribute("size_sample_ry"));
                    resize_ = Convert.ToDouble(xml_ele_.GetAttribute("resize_"));
                    X_dir_inv = Convert.ToInt32(xml_ele_.GetAttribute("X_dir_inv")); // (20180901) Jeff Revised!
                    Y_dir_inv = Convert.ToInt32(xml_ele_.GetAttribute("Y_dir_inv")); // (20180901) Jeff Revised!
                    mark1_rpos_x_ = Convert.ToDouble(xml_ele_.GetAttribute("Mark1_MotionPos_X"));
                    mark1_rpos_y_ = Convert.ToDouble(xml_ele_.GetAttribute("Mark1_MotionPos_Y"));
                    mark2_rpos_x_ = Convert.ToDouble(xml_ele_.GetAttribute("Mark2_MotionPos_X"));
                    mark2_rpos_y_ = Convert.ToDouble(xml_ele_.GetAttribute("Mark2_MotionPos_Y"));
                    start_rpos_x_ = Convert.ToDouble(xml_ele_.GetAttribute("Start_MotionPos_X"));
                    start_rpos_y_ = Convert.ToDouble(xml_ele_.GetAttribute("Start_MotionPos_Y"));
                    array_x_count_ = Convert.ToInt32(xml_ele_.GetAttribute("Move_Array_Count_X"));
                    array_x_rpitch_ = Convert.ToDouble(xml_ele_.GetAttribute("Move_Array_Pitch_X"));
                    array_y_count_ = Convert.ToInt32(xml_ele_.GetAttribute("Move_Array_Count_Y"));
                    array_y_rpitch_ = Convert.ToDouble(xml_ele_.GetAttribute("Move_Array_Pitch_Y"));
                    cell_col_count_ = Convert.ToInt32(xml_ele_.GetAttribute("All_Cell_Count_Col"));
                    cell_row_count_ = Convert.ToInt32(xml_ele_.GetAttribute("All_Cell_Count_Row"));
                    mark_rdx_ = Convert.ToDouble(xml_ele_.GetAttribute("Mark_Real_Dx"));
                    mark_rdy_ = Convert.ToDouble(xml_ele_.GetAttribute("Mark_Real_Dy"));
                    mark_rdx_shift = Convert.ToDouble(xml_ele_.GetAttribute("Mark_Real_Dx_shift"));
                    mark_rdy_shift = Convert.ToDouble(xml_ele_.GetAttribute("Mark_Real_Dy_shift"));
                    cell_rwidth_ = Convert.ToDouble(xml_ele_.GetAttribute("Cell_Real_Width"));
                    cell_rheight_ = Convert.ToDouble(xml_ele_.GetAttribute("Cell_Real_Height"));
                    cell_rdx_ = Convert.ToDouble(xml_ele_.GetAttribute("Cell_Real_Dx"));
                    cell_rdy_ = Convert.ToDouble(xml_ele_.GetAttribute("Cell_Real_Dy"));
                    mark_pdx_ = Convert.ToDouble(xml_ele_.GetAttribute("Mark_Pix_Dx"));
                    mark_pdy_ = Convert.ToDouble(xml_ele_.GetAttribute("Mark_Pix_Dy"));
                    cell_pwidth_ = Convert.ToDouble(xml_ele_.GetAttribute("Cell_Pix_Width"));
                    cell_pheight_ = Convert.ToDouble(xml_ele_.GetAttribute("Cell_Pix_Height"));
                    cell_pdx_ = Convert.ToDouble(xml_ele_.GetAttribute("Cell_Pix_Dx"));
                    cell_pdy_ = Convert.ToDouble(xml_ele_.GetAttribute("Cell_Pix_Dy"));
                    num_region_x_ = Convert.ToInt32(xml_ele_.GetAttribute("num_region_x"));
                    num_region_y_ = Convert.ToInt32(xml_ele_.GetAttribute("num_region_y"));
                    cell_x_count_string = xml_ele_.GetAttribute("cell_x_count_string"); // (20180903) Jeff Revised!
                    string_2_HTuple(cell_x_count_string, out cell_x_count_HTuple, true); // (20180903) Jeff Revised!
                    cell_y_count_string = xml_ele_.GetAttribute("cell_y_count_string"); // (20180903) Jeff Revised!
                    string_2_HTuple(cell_y_count_string, out cell_y_count_HTuple, true); // (20180903) Jeff Revised!
                    dist_region_rdx_string = xml_ele_.GetAttribute("dist_region_rdx_string"); // (20180903) Jeff Revised!
                    string_2_HTuple(dist_region_rdx_string, out dist_region_rdx_HTuple); // (20180903) Jeff Revised!
                    dist_region_rdy_string = xml_ele_.GetAttribute("dist_region_rdy_string"); // (20180903) Jeff Revised!
                    string_2_HTuple(dist_region_rdy_string, out dist_region_rdy_HTuple); // (20180903) Jeff Revised!
                    center_x_pix_shift = Convert.ToInt32(xml_ele_.GetAttribute("center_x_pix_shift")); // (20181022) Jeff Revised!
                    center_y_pix_shift = Convert.ToInt32(xml_ele_.GetAttribute("center_y_pix_shift")); // (20181022) Jeff Revised!

                    cs_thred_min_ = Convert.ToInt32(xml_ele_.GetAttribute("Cs_Thred_Min"));
                    cs_thred_max_ = Convert.ToInt32(xml_ele_.GetAttribute("Cs_Thred_Max"));
                    cs_area_min_ = Convert.ToInt32(xml_ele_.GetAttribute("Cs_Area_Min"));
                    cs_area_max_ = Convert.ToInt32(xml_ele_.GetAttribute("Cs_Area_Max"));
                    cd_thred_min_ = Convert.ToInt32(xml_ele_.GetAttribute("Cd_Thred_Min"));
                    cd_thred_max_ = Convert.ToInt32(xml_ele_.GetAttribute("Cd_Thred_Max"));
                    cd_area_min_ = Convert.ToInt32(xml_ele_.GetAttribute("Cd_Area_Min"));
                    cd_area_max_ = Convert.ToInt32(xml_ele_.GetAttribute("Cd_Area_Max"));

                    // Resize parameters (20180828) Jeff Revised!
                    b_resizeParam = false;
                    resize_parameters();

                    // 單位轉換 (mm → pixel) (20180828) Jeff Revised!
                    unitTrans_mm2pixel();

                    // 先產生一張影像，之後AffineMethodObj.load()才能順利載入region (20190509) Jeff Revised!
                    HObject img;
                    //HOperatorSet.GenImageConst(out img, "byte", frame_pwidth_, frame_pheight_);
                    HOperatorSet.GenImageConst(out img, "byte", frame_pwidth_ / resize_, frame_pheight_ / resize_);
                    //HOperatorSet.GenImageConst(out img, "byte", frame_pwidth_, frame_pheight_); // (20191216) MIL Jeff Revised!
                    img.Dispose();

                    #region LocateMethodRecipe_New

                    LocateMethodRecipe.ListPt_BypassCell.Clear(); // (20190722) Jeff Revised!
                    if (clsStaticTool.LoadXML(File_directory_ + "\\LabelDefect_New.xml", out this.LocateMethodRecipe) == false) // (20200429) Jeff Revised!
                        return false;

                    if (!(LocateMethodRecipe.VisualPosEnable)) // (20191216) MIL Jeff Revised!
                        affine_degree_ = 0.0;

                    #endregion

                    b_status_ = true;
                }
            }
            catch (Exception ex)
            {
                b_status_ = false;
            }

            return b_status_;
        }

        public bool DeleteParam()
        {
            bool b_status_ = false;

            try
            {
                XmlDocument xml_doc_ = new XmlDocument();
                if (System.IO.File.Exists(ModuleParamDirectory + PositionParam + "\\Label Defect.xml"))
                    xml_doc_.Load(ModuleParamDirectory + PositionParam + "\\Label Defect.xml");

                XmlNode xml_node_ = xml_doc_.SelectSingleNode("Board_Type");
                if (xml_node_ == null)
                {
                    return false;
                }

                resize_parameters_back();

                XmlNode xml_type_ = xml_doc_.SelectSingleNode("Board_Type/Type_" + board_type_);
                if (xml_type_ != null)
                {
                    xml_node_.RemoveChild(xml_type_);
                }
                xml_doc_.Save(ModuleParamDirectory + PositionParam + "\\Label Defect.xml");
                b_status_ = true;
            }
            catch (Exception ex)
            {
                b_status_ = false;
            }

            // Resize parameters
            resize_parameters();

            return b_status_;
        }

        public bool SaveAs(string ParamName)
        {
            bool b_status_ = false;

            try
            {
                XmlDocument xml_doc_ = new XmlDocument();
                if (System.IO.File.Exists(ModuleParamDirectory + PositionParam + "\\Label Defect.xml"))
                    xml_doc_.Load(ModuleParamDirectory + PositionParam + "\\Label Defect.xml");

                XmlNode xml_node_ = xml_doc_.SelectSingleNode("Board_Type");
                if (xml_node_ == null)
                {
                    xml_node_ = xml_doc_.CreateElement("Board_Type");
                    xml_doc_.AppendChild(xml_node_);
                }

                resize_parameters_back();

                XmlNode xml_type_ = xml_doc_.SelectSingleNode("Board_Type/Type_" + ParamName);
                if (xml_type_ == null)
                {
                    xml_type_ = xml_doc_.CreateElement("Type_" + ParamName);
                    xml_node_.AppendChild(xml_type_);

                    XmlElement xml_ele_ = xml_doc_.CreateElement("Label_Defect");
                    // 儲存參數
                    xml_ele_.SetAttribute("Pixel_Resolution", pixel_resolution_.ToString());
                    xml_ele_.SetAttribute("Frame_Width", frame_pwidth_.ToString());
                    xml_ele_.SetAttribute("Frame_Height", frame_pheight_.ToString());
                    xml_ele_.SetAttribute("size_sample_rx", size_sample_rx.ToString());
                    xml_ele_.SetAttribute("size_sample_ry", size_sample_ry.ToString());
                    xml_ele_.SetAttribute("resize_", resize_.ToString());
                    xml_ele_.SetAttribute("X_dir_inv", X_dir_inv.ToString()); // (20180901) Jeff Revised!
                    xml_ele_.SetAttribute("Y_dir_inv", Y_dir_inv.ToString()); // (20180901) Jeff Revised!
                    xml_ele_.SetAttribute("Mark1_MotionPos_X", mark1_rpos_x_.ToString());
                    xml_ele_.SetAttribute("Mark1_MotionPos_Y", mark1_rpos_y_.ToString());
                    xml_ele_.SetAttribute("Mark2_MotionPos_X", mark2_rpos_x_.ToString());
                    xml_ele_.SetAttribute("Mark2_MotionPos_Y", mark2_rpos_y_.ToString());
                    xml_ele_.SetAttribute("Start_MotionPos_X", start_rpos_x_.ToString());
                    xml_ele_.SetAttribute("Start_MotionPos_Y", start_rpos_y_.ToString());
                    xml_ele_.SetAttribute("Move_Array_Count_X", array_x_count_.ToString());
                    xml_ele_.SetAttribute("Move_Array_Pitch_X", array_x_rpitch_.ToString());
                    xml_ele_.SetAttribute("Move_Array_Count_Y", array_y_count_.ToString());
                    xml_ele_.SetAttribute("Move_Array_Pitch_Y", array_y_rpitch_.ToString());
                    xml_ele_.SetAttribute("All_Cell_Count_Col", cell_col_count_.ToString());
                    xml_ele_.SetAttribute("All_Cell_Count_Row", cell_row_count_.ToString());
                    xml_ele_.SetAttribute("Mark_Real_Dx", mark_rdx_.ToString());
                    xml_ele_.SetAttribute("Mark_Real_Dy", mark_rdy_.ToString());
                    xml_ele_.SetAttribute("Mark_Real_Dx_shift", mark_rdx_shift.ToString());
                    xml_ele_.SetAttribute("Mark_Real_Dy_shift", mark_rdy_shift.ToString());
                    xml_ele_.SetAttribute("Cell_Real_Width", cell_rwidth_.ToString());
                    xml_ele_.SetAttribute("Cell_Real_Height", cell_rheight_.ToString());
                    xml_ele_.SetAttribute("Cell_Real_Dx", cell_rdx_.ToString());
                    xml_ele_.SetAttribute("Cell_Real_Dy", cell_rdy_.ToString());
                    xml_ele_.SetAttribute("Mark_Pix_Dx", mark_pdx_.ToString());
                    xml_ele_.SetAttribute("Mark_Pix_Dy", mark_pdy_.ToString());
                    xml_ele_.SetAttribute("Cell_Pix_Width", cell_pwidth_.ToString());
                    xml_ele_.SetAttribute("Cell_Pix_Height", cell_pheight_.ToString());
                    xml_ele_.SetAttribute("Cell_Pix_Dx", cell_pdx_.ToString());
                    xml_ele_.SetAttribute("Cell_Pix_Dy", cell_pdy_.ToString());
                    xml_ele_.SetAttribute("num_region_x", num_region_x_.ToString());
                    xml_ele_.SetAttribute("num_region_y", num_region_y_.ToString());
                    HTuple_2_string(cell_x_count_HTuple, out cell_x_count_string); // (20180903) Jeff Revised!
                    xml_ele_.SetAttribute("cell_x_count_string", cell_x_count_string); // (20180903) Jeff Revised!
                    HTuple_2_string(cell_y_count_HTuple, out cell_y_count_string); // (20180903) Jeff Revised!
                    xml_ele_.SetAttribute("cell_y_count_string", cell_y_count_string); // (20180903) Jeff Revised!
                    HTuple_2_string(dist_region_rdx_HTuple, out dist_region_rdx_string); // (20180903) Jeff Revised!
                    xml_ele_.SetAttribute("dist_region_rdx_string", dist_region_rdx_string); // (20180903) Jeff Revised!
                    HTuple_2_string(dist_region_rdy_HTuple, out dist_region_rdy_string); // (20180903) Jeff Revised!
                    xml_ele_.SetAttribute("dist_region_rdy_string", dist_region_rdy_string); // (20180903) Jeff Revised!
                    xml_ele_.SetAttribute("center_x_pix_shift", center_x_pix_shift.ToString()); // (20181022) Jeff Revised!
                    xml_ele_.SetAttribute("center_y_pix_shift", center_y_pix_shift.ToString()); // (20181022) Jeff Revised!

                    xml_ele_.SetAttribute("Cs_Thred_Min", cs_thred_min_.ToString());
                    xml_ele_.SetAttribute("Cs_Thred_Max", cs_thred_max_.ToString());
                    xml_ele_.SetAttribute("Cs_Area_Min", cs_area_min_.ToString());
                    xml_ele_.SetAttribute("Cs_Area_Max", cs_area_max_.ToString());
                    xml_ele_.SetAttribute("Cd_Thred_Min", cd_thred_min_.ToString());
                    xml_ele_.SetAttribute("Cd_Thred_Max", cd_thred_max_.ToString());
                    xml_ele_.SetAttribute("Cd_Area_Min", cd_area_min_.ToString());
                    xml_ele_.SetAttribute("Cd_Area_Max", cd_area_max_.ToString());
                    xml_type_.AppendChild(xml_ele_);
                }
                xml_doc_.Save(ModuleParamDirectory + PositionParam + "\\Label Defect.xml");

                #region LocateMethodRecipe_New

                string Path = ModuleParamDirectory + PositionParam + "\\" + ParamName + "\\";
                b_status_ = clsStaticTool.SaveXML(this.LocateMethodRecipe, Path + "\\LabelDefect_New.xml"); // (20190722) Jeff Revised!

                #endregion

                //b_status_ = true;
            }
            catch (Exception ex)
            {
                b_status_ = false;
            }

            // Resize parameters
            resize_parameters();

            return b_status_;
        }

        /// <summary>
        /// 儲存變數至.xml檔
        /// </summary>
        /// <returns></returns>
        public bool save()
        {
            bool b_status_ = false;

            try
            {
                XmlDocument xml_doc_ = new XmlDocument();
                if (System.IO.File.Exists(ModuleParamDirectory + PositionParam + "\\Label Defect.xml"))
                    xml_doc_.Load(ModuleParamDirectory + PositionParam + "\\Label Defect.xml");

                XmlNode xml_node_ = xml_doc_.SelectSingleNode("Board_Type");
                if (xml_node_ == null)
                {
                    xml_node_ = xml_doc_.CreateElement("Board_Type");
                    xml_doc_.AppendChild(xml_node_);
                }

                // Resize parameters back to the original size
                resize_parameters_back(); // (20180828) Jeff Revised!

                XmlNode xml_type_ = xml_doc_.SelectSingleNode("Board_Type/Type_" + board_type_);
                if (xml_type_ == null)
                {
                    xml_type_ = xml_doc_.CreateElement("Type_" + board_type_);
                    xml_node_.AppendChild(xml_type_);

                    XmlElement xml_ele_ = xml_doc_.CreateElement("Label_Defect");
                    // 儲存參數
                    //xml_ele_.SetAttribute("Save_Cell_Map_Directory", ModuleParamDirectory + RecipeFileDirectory + ModuleName);
                    //xml_ele_.SetAttribute("Load_Cell_Map_Abs_Directory", ModuleParamDirectory + RecipeFileDirectory + ModuleName );
                    xml_ele_.SetAttribute("Pixel_Resolution", pixel_resolution_.ToString());
                    xml_ele_.SetAttribute("Frame_Width", frame_pwidth_.ToString());
                    xml_ele_.SetAttribute("Frame_Height", frame_pheight_.ToString());
                    xml_ele_.SetAttribute("size_sample_rx", size_sample_rx.ToString());
                    xml_ele_.SetAttribute("size_sample_ry", size_sample_ry.ToString());
                    xml_ele_.SetAttribute("resize_", resize_.ToString());
                    xml_ele_.SetAttribute("X_dir_inv", X_dir_inv.ToString()); // (20180901) Jeff Revised!
                    xml_ele_.SetAttribute("Y_dir_inv", Y_dir_inv.ToString()); // (20180901) Jeff Revised!
                    xml_ele_.SetAttribute("Mark1_MotionPos_X", mark1_rpos_x_.ToString());
                    xml_ele_.SetAttribute("Mark1_MotionPos_Y", mark1_rpos_y_.ToString());
                    xml_ele_.SetAttribute("Mark2_MotionPos_X", mark2_rpos_x_.ToString());
                    xml_ele_.SetAttribute("Mark2_MotionPos_Y", mark2_rpos_y_.ToString());
                    xml_ele_.SetAttribute("Start_MotionPos_X", start_rpos_x_.ToString());
                    xml_ele_.SetAttribute("Start_MotionPos_Y", start_rpos_y_.ToString());
                    xml_ele_.SetAttribute("Move_Array_Count_X", array_x_count_.ToString());
                    xml_ele_.SetAttribute("Move_Array_Pitch_X", array_x_rpitch_.ToString());
                    xml_ele_.SetAttribute("Move_Array_Count_Y", array_y_count_.ToString());
                    xml_ele_.SetAttribute("Move_Array_Pitch_Y", array_y_rpitch_.ToString());
                    xml_ele_.SetAttribute("All_Cell_Count_Col", cell_col_count_.ToString());
                    xml_ele_.SetAttribute("All_Cell_Count_Row", cell_row_count_.ToString());
                    xml_ele_.SetAttribute("Mark_Real_Dx", mark_rdx_.ToString());
                    xml_ele_.SetAttribute("Mark_Real_Dy", mark_rdy_.ToString());
                    xml_ele_.SetAttribute("Mark_Real_Dx_shift", mark_rdx_shift.ToString());
                    xml_ele_.SetAttribute("Mark_Real_Dy_shift", mark_rdy_shift.ToString());
                    xml_ele_.SetAttribute("Cell_Real_Width", cell_rwidth_.ToString());
                    xml_ele_.SetAttribute("Cell_Real_Height", cell_rheight_.ToString());
                    xml_ele_.SetAttribute("Cell_Real_Dx", cell_rdx_.ToString());
                    xml_ele_.SetAttribute("Cell_Real_Dy", cell_rdy_.ToString());
                    xml_ele_.SetAttribute("Mark_Pix_Dx", mark_pdx_.ToString());
                    xml_ele_.SetAttribute("Mark_Pix_Dy", mark_pdy_.ToString());
                    xml_ele_.SetAttribute("Cell_Pix_Width", cell_pwidth_.ToString());
                    xml_ele_.SetAttribute("Cell_Pix_Height", cell_pheight_.ToString());
                    xml_ele_.SetAttribute("Cell_Pix_Dx", cell_pdx_.ToString());
                    xml_ele_.SetAttribute("Cell_Pix_Dy", cell_pdy_.ToString());
                    xml_ele_.SetAttribute("num_region_x", num_region_x_.ToString());
                    xml_ele_.SetAttribute("num_region_y", num_region_y_.ToString());
                    HTuple_2_string(cell_x_count_HTuple, out cell_x_count_string); // (20180903) Jeff Revised!
                    xml_ele_.SetAttribute("cell_x_count_string", cell_x_count_string); // (20180903) Jeff Revised!
                    HTuple_2_string(cell_y_count_HTuple, out cell_y_count_string); // (20180903) Jeff Revised!
                    xml_ele_.SetAttribute("cell_y_count_string", cell_y_count_string); // (20180903) Jeff Revised!
                    HTuple_2_string(dist_region_rdx_HTuple, out dist_region_rdx_string); // (20180903) Jeff Revised!
                    xml_ele_.SetAttribute("dist_region_rdx_string", dist_region_rdx_string); // (20180903) Jeff Revised!
                    HTuple_2_string(dist_region_rdy_HTuple, out dist_region_rdy_string); // (20180903) Jeff Revised!
                    xml_ele_.SetAttribute("dist_region_rdy_string", dist_region_rdy_string); // (20180903) Jeff Revised!
                    xml_ele_.SetAttribute("center_x_pix_shift", center_x_pix_shift.ToString()); // (20181022) Jeff Revised!
                    xml_ele_.SetAttribute("center_y_pix_shift", center_y_pix_shift.ToString()); // (20181022) Jeff Revised!

                    xml_ele_.SetAttribute("Cs_Thred_Min", cs_thred_min_.ToString());
                    xml_ele_.SetAttribute("Cs_Thred_Max", cs_thred_max_.ToString());
                    xml_ele_.SetAttribute("Cs_Area_Min", cs_area_min_.ToString());
                    xml_ele_.SetAttribute("Cs_Area_Max", cs_area_max_.ToString());
                    xml_ele_.SetAttribute("Cd_Thred_Min", cd_thred_min_.ToString());
                    xml_ele_.SetAttribute("Cd_Thred_Max", cd_thred_max_.ToString());
                    xml_ele_.SetAttribute("Cd_Area_Min", cd_area_min_.ToString());
                    xml_ele_.SetAttribute("Cd_Area_Max", cd_area_max_.ToString());
                    xml_type_.AppendChild(xml_ele_);
                }
                else
                {
                    XmlNode xml_ele_ = xml_doc_.SelectSingleNode("Board_Type/Type_" + board_type_ + "/ Label_Defect");
                    xml_ele_.Attributes["Pixel_Resolution"].Value = pixel_resolution_.ToString();
                    xml_ele_.Attributes["Frame_Width"].Value = frame_pwidth_.ToString();
                    xml_ele_.Attributes["Frame_Height"].Value = frame_pheight_.ToString();
                    xml_ele_.Attributes["size_sample_rx"].Value = size_sample_rx.ToString();
                    xml_ele_.Attributes["size_sample_ry"].Value = size_sample_ry.ToString();
                    xml_ele_.Attributes["resize_"].Value = resize_.ToString();
                    xml_ele_.Attributes["X_dir_inv"].Value = X_dir_inv.ToString(); // (20180901) Jeff Revised!
                    xml_ele_.Attributes["Y_dir_inv"].Value = Y_dir_inv.ToString(); // (20180901) Jeff Revised!
                    xml_ele_.Attributes["Mark1_MotionPos_X"].Value = mark1_rpos_x_.ToString();
                    xml_ele_.Attributes["Mark1_MotionPos_Y"].Value = mark1_rpos_y_.ToString();
                    xml_ele_.Attributes["Mark2_MotionPos_X"].Value = mark2_rpos_x_.ToString();
                    xml_ele_.Attributes["Mark2_MotionPos_Y"].Value = mark2_rpos_y_.ToString();
                    xml_ele_.Attributes["Start_MotionPos_X"].Value = start_rpos_x_.ToString();
                    xml_ele_.Attributes["Start_MotionPos_Y"].Value = start_rpos_y_.ToString();
                    xml_ele_.Attributes["Move_Array_Count_X"].Value = array_x_count_.ToString();
                    xml_ele_.Attributes["Move_Array_Pitch_X"].Value = array_x_rpitch_.ToString();
                    xml_ele_.Attributes["Move_Array_Count_Y"].Value = array_y_count_.ToString();
                    xml_ele_.Attributes["Move_Array_Pitch_Y"].Value = array_y_rpitch_.ToString();
                    xml_ele_.Attributes["All_Cell_Count_Col"].Value = cell_col_count_.ToString();
                    xml_ele_.Attributes["All_Cell_Count_Row"].Value = cell_row_count_.ToString();
                    xml_ele_.Attributes["Mark_Real_Dx"].Value = mark_rdx_.ToString();
                    xml_ele_.Attributes["Mark_Real_Dy"].Value = mark_rdy_.ToString();
                    xml_ele_.Attributes["Mark_Real_Dx_shift"].Value = mark_rdx_shift.ToString();
                    xml_ele_.Attributes["Mark_Real_Dy_shift"].Value = mark_rdy_shift.ToString();
                    xml_ele_.Attributes["Cell_Real_Width"].Value = cell_rwidth_.ToString();
                    xml_ele_.Attributes["Cell_Real_Height"].Value = cell_rheight_.ToString();
                    xml_ele_.Attributes["Cell_Real_Dx"].Value = cell_rdx_.ToString();
                    xml_ele_.Attributes["Cell_Real_Dy"].Value = cell_rdy_.ToString();
                    xml_ele_.Attributes["Mark_Pix_Dx"].Value = mark_pdx_.ToString();
                    xml_ele_.Attributes["Mark_Pix_Dy"].Value = mark_pdy_.ToString();
                    xml_ele_.Attributes["Cell_Pix_Width"].Value = cell_pwidth_.ToString();
                    xml_ele_.Attributes["Cell_Pix_Height"].Value = cell_pheight_.ToString();
                    xml_ele_.Attributes["Cell_Pix_Dx"].Value = cell_pdx_.ToString();
                    xml_ele_.Attributes["Cell_Pix_Dy"].Value = cell_pdy_.ToString();
                    xml_ele_.Attributes["num_region_x"].Value = num_region_x_.ToString();
                    xml_ele_.Attributes["num_region_y"].Value = num_region_y_.ToString();
                    HTuple_2_string(cell_x_count_HTuple, out cell_x_count_string); // (20180903) Jeff Revised!
                    xml_ele_.Attributes["cell_x_count_string"].Value = cell_x_count_string; // (20180903) Jeff Revised!
                    HTuple_2_string(cell_y_count_HTuple, out cell_y_count_string); // (20180903) Jeff Revised!
                    xml_ele_.Attributes["cell_y_count_string"].Value = cell_y_count_string; // (20180903) Jeff Revised!
                    HTuple_2_string(dist_region_rdx_HTuple, out dist_region_rdx_string); // (20180903) Jeff Revised!
                    xml_ele_.Attributes["dist_region_rdx_string"].Value = dist_region_rdx_string; // (20180903) Jeff Revised!
                    HTuple_2_string(dist_region_rdy_HTuple, out dist_region_rdy_string); // (20180903) Jeff Revised!
                    xml_ele_.Attributes["dist_region_rdy_string"].Value = dist_region_rdy_string; // (20180903) Jeff Revised!
                    xml_ele_.Attributes["center_x_pix_shift"].Value = center_x_pix_shift.ToString(); // (20181022) Jeff Revised!
                    xml_ele_.Attributes["center_y_pix_shift"].Value = center_y_pix_shift.ToString(); // (20181022) Jeff Revised!

                    xml_ele_.Attributes["Cs_Thred_Min"].Value = cs_thred_min_.ToString();
                    xml_ele_.Attributes["Cs_Thred_Max"].Value = cs_thred_max_.ToString();
                    xml_ele_.Attributes["Cs_Area_Min"].Value = cs_area_min_.ToString();
                    xml_ele_.Attributes["Cs_Area_Max"].Value = cs_area_max_.ToString();
                    xml_ele_.Attributes["Cd_Thred_Min"].Value = cd_thred_min_.ToString();
                    xml_ele_.Attributes["Cd_Thred_Max"].Value = cd_thred_max_.ToString();
                    xml_ele_.Attributes["Cd_Area_Min"].Value = cd_area_min_.ToString();
                    xml_ele_.Attributes["Cd_Area_Max"].Value = cd_area_max_.ToString();
                }

               
                //xml_doc_.Save(ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\Label Defect.xml");
                xml_doc_.Save(ModuleParamDirectory + PositionParam + "\\Label Defect.xml");

                #region LocateMethodRecipe_New

                b_status_ = clsStaticTool.SaveXML(this.LocateMethodRecipe, File_directory_ + "\\LabelDefect_New.xml"); // (20190722) Jeff Revised!

                #endregion

                //b_status_ = true;
            }
            catch (Exception ex)
            {
                b_status_ = false;
            }

            // Resize parameters
            resize_parameters(); // (20180828) Jeff Revised!

            return b_status_;
        }

        /// <summary>
        /// 執行 Label Defect Class
        /// </summary>
        /// <returns></returns>
        public bool execute(HSmartWindowControl hwindow_, string file_directory_, HObject mark_1_img_, HObject mark_2_img_, List<HObject> all_capture_images_)
        {
            bool b_status_ = false;

            // 轉成RGB image (20180829) Jeff Revised!
            HTuple Channels;
            HOperatorSet.CountChannels(mark_1_img_, out Channels);
            if (Channels == 1)
            {
                HOperatorSet.Compose3(mark_1_img_.Clone(), mark_1_img_.Clone(), mark_1_img_.Clone(), out mark_1_img_); // (20181224) Jeff Revised!
            }
            HOperatorSet.CountChannels(mark_2_img_, out Channels);
            if (Channels == 1)
            {
                HOperatorSet.Compose3(mark_2_img_.Clone(), mark_2_img_.Clone(), mark_2_img_.Clone(), out mark_2_img_); // (20181224) Jeff Revised!
            }

            // Rotate
            if (!affine_method_.read_shape_model())
                affine_method_.create_shape_model(hwindow_.HalconWindow, mark_1_img_);
            affine_method_.find_shpe_model(hwindow_.HalconWindow, mark_1_img_, hwindow_.HalconWindow, mark_2_img_);
            affine_degree_ = affine_method_.Affine_degree_;
            origin_mark_pos_x_ = affine_method_.mark_x_[0];
            origin_mark_pos_y_ = affine_method_.mark_y_[0];

            // Virtual Segmentation
            Create_cell_map(hwindow_, all_capture_images_[0]); // (20180829) Jeff Revised!
            tile_images(hwindow_, all_capture_images_);
            Cell_region_mapping();
            b_status_ = true;

            return b_status_;
        }

        /// <summary>
        /// 更新基板偏斜角度及Mark相關資訊
        /// </summary>
        /// <param name="method"></param>
        public void update_affine(AngleAffineMethod method)
        {
            affine_method_ = method;
            affine_degree_ = affine_method_.Affine_degree_;
            origin_mark_pos_x_ = affine_method_.mark_x_[0];
            origin_mark_pos_y_ = affine_method_.mark_y_[0];
        }

        /// <summary>
        /// 執行 Virtual Segmentation
        /// </summary>
        public void initialize() // (20181116) Jeff Revised!
        {
            create_cell_map2();
            Cell_region_mapping();

            motion_path_init();
        }

        /// <summary>
        /// 建立 Cell Map (不做視覺定位)
        /// </summary>
        /// <returns></returns>
        public bool initialize_NoPos() // (20191216) Jeff Revised!
        {
            bool b_status_ = false;
            if (create_cell_map2_NoPos())
            {
                if (Cell_region_mapping())
                    b_status_ = motion_path_init();
            }
            return b_status_;
        }

        #region calculate cell width、height、dx and dy
        
        /// <summary>
        /// 計算每張影像機械位置之關係 (轉換成pix表示)
        /// 得到: 各影像在大拼接圖上之FOV (capture_map_) & center_points_x_, center_points_y_, tile_rect_
        /// </summary>
        /// <returns></returns>
        public bool motion_path_init() // 加入shift補償值 (20181022) Jeff Revised!
        {
            // 加入shift補償值 (20181022) Jeff Revised!
            // For 1109_A_MB1 & 1109_B_MB2
            //int center_x_pix_shift = -4;
            //int center_y_pix_shift = 7;
            // For 1109_B_MB1 & 1109_A_MB2
            //int center_x_pix_shift = 4;
            //int center_y_pix_shift = -7;

            List<int> Center_x_pix_shift = new List<int>();
            List<int> Center_y_pix_shift = new List<int>();
            for (int x = 1, y_pix_shift = 0; x <= array_x_count_; x++, y_pix_shift += center_y_pix_shift)
                Center_y_pix_shift.Add(y_pix_shift);
            for (int y = 1, x_pix_shift = 0; y <= array_y_count_; y++, x_pix_shift += center_x_pix_shift)
                Center_x_pix_shift.Add(x_pix_shift);

            bool b_status_ = false;
            int path_count_ = 0;
            double array_x_pix_ = array_x_rpitch_ * 1000 / pixel_resolution_;
            double array_y_pix_ = array_y_rpitch_ * 1000 / pixel_resolution_;
            double center_x_, center_y_; // 各位置影像 中點/左上角點 (pixel)
            //HOperatorSet.GenEmptyRegion(out capture_map_);
            HOperatorSet.GenEmptyObj(out capture_map_); // (20190624) Jeff Revised!

            if (center_points_x_ == null) center_points_x_ = new HTuple(); // (20190402) Jeff Revised!
            if (center_points_y_ == null) center_points_y_ = new HTuple(); // (20190402) Jeff Revised!
            center_points_x_ = new HTuple(); // (20191216) MIL Jeff Revised!
            center_points_y_ = new HTuple(); // (20191216) MIL Jeff Revised!
            for (int y = 1; y <= array_y_count_; y++)
            {
                for (int x = 1; x <= array_x_count_; x++)
                {
                    if (y % 2 == 0) // 走停拍模式: S動作
                    {
                        center_x_ = array_x_pix_ * (array_x_count_ - x) + Center_x_pix_shift[y - 1];
                        center_y_ = array_y_pix_ * (y - 1) + Center_y_pix_shift[array_x_count_ - x];
                    }
                    else
                    {
                        center_x_ = array_x_pix_ * (x - 1) + Center_x_pix_shift[y - 1]; // (20180824) Jeff Revised!
                        center_y_ = array_y_pix_ * (y - 1) + Center_y_pix_shift[x - 1];
                    }

                    HObject rect_;
                    //HOperatorSet.GenRectangle2(out rect_, center_y_, center_x_, 0, frame_pwidth_ / 2, frame_pheight_ / 2);
                    HOperatorSet.GenRectangle1(out rect_, center_y_, center_x_, center_y_ + frame_pheight_, center_x_ + frame_pwidth_); // (20181022) Jeff Revised!
                    HOperatorSet.ConcatObj(capture_map_, rect_, out capture_map_);
                    rect_.Dispose();

                    center_points_x_[path_count_] = (int)(center_x_ + 0.5); // (20181022) Jeff Revised!
                    center_points_y_[path_count_] = (int)(center_y_ + 0.5); // (20181022) Jeff Revised!
                    tile_rect_[path_count_] = -1; // the corresponding input image is not cropped!
                    path_count_++;
                }
            }
            //HOperatorSet.WriteRegion(capture_map_, file_directory_ + "\\" + "capture_map.hobj"); // For debug! (20180824) Jeff Revised!

            b_status_ = true;

            return b_status_;
        }

        /// <summary>
        /// 所有觸發影像拼接
        /// </summary>
        /// <param name="hwindow_">預覽UI</param>
        /// <param name="all_capture_images_">所有觸發影像</param>
        /// <returns></returns>
        public bool tile_images(HSmartWindowControl hwindow_, List<HObject> all_capture_images_)
        {
            bool b_status_ = false;
            HObject all_concat_images_ = null;

            try // (20190603) Jeff Revised!
            {
                if (motion_path_init())
                {
                    // 將 all_capture_images_ 轉換成 all_concat_images_
                    HOperatorSet.GenEmptyObj(out all_concat_images_); // (20190604) Jeff Revised!
                    foreach (var i in all_capture_images_)
                    {
                        // Resize
                        HObject i_resize;
                        HOperatorSet.ZoomImageFactor(i, out i_resize, resize_, resize_, "bilinear");

                        HOperatorSet.ConcatObj(all_concat_images_, i_resize, out all_concat_images_); // (20190604) Jeff Revised!
                    }

                    HTuple number_;
                    HOperatorSet.CountObj(all_concat_images_, out number_);

#if (showTimeSpan)
                    sw.Reset(); // 碼表歸零
                    sw.Start(); // 碼表開始計時
#endif
                    double array_x_pix_ = array_x_rpitch_ * 1000 / pixel_resolution_;
                    double array_y_pix_ = array_y_rpitch_ * 1000 / pixel_resolution_;
                    Extension.HObjectMedthods.ReleaseHObject(ref this.TileImage); // (20200429) Jeff Revised!
                    if ((array_x_pix_ >= frame_pwidth_) && (array_y_pix_ >= frame_pheight_)) // 兩相鄰影像沒有Overlap
                    {
                        HOperatorSet.TileImagesOffset(all_concat_images_, out this.TileImage, center_points_y_.TupleSelectRange(0, number_ - 1), center_points_x_.TupleSelectRange(0, number_ - 1),
                            tile_rect_.TupleSelectRange(0, number_ - 1), tile_rect_.TupleSelectRange(0, number_ - 1), tile_rect_.TupleSelectRange(0, number_ - 1), tile_rect_.TupleSelectRange(0, number_ - 1),
                            frame_pwidth_ * array_x_count_, frame_pheight_ * array_y_count_);
                    }
                    else // 兩相鄰影像有Overlap
                    {
                        HOperatorSet.TileImagesOffset(all_concat_images_, out this.TileImage, center_points_y_.TupleSelectRange(0, number_ - 1), center_points_x_.TupleSelectRange(0, number_ - 1),
                            tile_rect_.TupleSelectRange(0, number_ - 1), tile_rect_.TupleSelectRange(0, number_ - 1), tile_rect_.TupleSelectRange(0, number_ - 1), tile_rect_.TupleSelectRange(0, number_ - 1),
                            frame_pwidth_ * array_x_count_ - (frame_pwidth_ - array_x_pix_) * (array_x_count_ - 1), frame_pheight_ * array_y_count_ - (frame_pheight_ - array_y_pix_) * (array_y_count_ - 1));
                    }

                    all_concat_images_.Dispose(); // (20190403) Jeff Revised!

#if (showTimeSpan)
                    sw.Stop(); // 碼錶停止
                    elapseTime = sw.Elapsed.TotalMilliseconds;
#endif

                    HOperatorSet.ClearWindow(hwindow_.HalconWindow);
                    // 轉成RGB image (20180822) Jeff Revised!
                    //HTuple Channels;
                    //HOperatorSet.CountChannels(this.TileImage, out Channels);
                    //if (Channels == 1)
                    //{
                    //    HOperatorSet.Compose3(this.TileImage.Clone(), this.TileImage.Clone(), this.TileImage.Clone(), out this.TileImage); // (20181224) Jeff Revised!
                    //}
                    //HOperatorSet.DispColor(this.TileImage, hwindow_.HalconWindow);
                    HOperatorSet.DispObj(this.TileImage, hwindow_.HalconWindow);
                    HOperatorSet.SetPart(hwindow_.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part
                    b_status_ = true;
                }
            }
            catch
            {

            }

            return b_status_;
        }

        /// <summary>
        /// 所有觸發影像拼接
        /// </summary>
        /// <param name="hwindow_"></param>
        /// <param name="all_concat_images_"></param>
        /// <param name="b_resize_finished">輸入影像all_concat_images_是否已做resize</param>
        /// <returns></returns>
        public bool tile_images(HSmartWindowControl hwindow_, HObject all_concat_images_, bool b_resize_finished = false) // (20200429) Jeff Revised!
        {
            bool b_status_ = false;

            try // (20190603) Jeff Revised!
            {
                if (motion_path_init())
                {
                    if (resize_ != 1.0 && !b_resize_finished) // (20190606) Jeff Revised!
                    {
                        // Resize
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ZoomImageFactor(all_concat_images_, out ExpTmpOutVar_0, resize_, resize_, "bilinear");
                            all_concat_images_.Dispose();
                            all_concat_images_ = ExpTmpOutVar_0;
                        }
                    }

                    HTuple number_;
                    HOperatorSet.CountObj(all_concat_images_, out number_);

#if (showTimeSpan)
                    sw.Reset(); // 碼表歸零
                    sw.Start(); // 碼表開始計時
#endif
                    double array_x_pix_ = array_x_rpitch_ * 1000 / pixel_resolution_;
                    double array_y_pix_ = array_y_rpitch_ * 1000 / pixel_resolution_;
                    Extension.HObjectMedthods.ReleaseHObject(ref this.TileImage); // (20200429) Jeff Revised!
                    if ((array_x_pix_ >= frame_pwidth_) && (array_y_pix_ >= frame_pheight_)) // 兩相鄰影像沒有Overlap
                    {
                        HOperatorSet.TileImagesOffset(all_concat_images_, out this.TileImage, center_points_y_.TupleSelectRange(0, number_ - 1), center_points_x_.TupleSelectRange(0, number_ - 1),
                            tile_rect_.TupleSelectRange(0, number_ - 1), tile_rect_.TupleSelectRange(0, number_ - 1), tile_rect_.TupleSelectRange(0, number_ - 1), tile_rect_.TupleSelectRange(0, number_ - 1),
                            frame_pwidth_ * array_x_count_, frame_pheight_ * array_y_count_);
                    }
                    else // 兩相鄰影像有Overlap
                    {
                        HOperatorSet.TileImagesOffset(all_concat_images_, out this.TileImage, center_points_y_.TupleSelectRange(0, number_ - 1), center_points_x_.TupleSelectRange(0, number_ - 1),
                            tile_rect_.TupleSelectRange(0, number_ - 1), tile_rect_.TupleSelectRange(0, number_ - 1), tile_rect_.TupleSelectRange(0, number_ - 1), tile_rect_.TupleSelectRange(0, number_ - 1),
                            frame_pwidth_ * array_x_count_ - (frame_pwidth_ - array_x_pix_) * (array_x_count_ - 1), frame_pheight_ * array_y_count_ - (frame_pheight_ - array_y_pix_) * (array_y_count_ - 1));
                    }

#if (showTimeSpan)
                    sw.Stop(); // 碼錶停止
                    elapseTime = sw.Elapsed.TotalMilliseconds;
#endif

                    HOperatorSet.ClearWindow(hwindow_.HalconWindow);
                    // 轉成RGB image (20180822) Jeff Revised!
                    //HTuple Channels;
                    //HOperatorSet.CountChannels(this.TileImage, out Channels);
                    //if (Channels == 1)
                    //{
                    //    HOperatorSet.Compose3(this.TileImage.Clone(), this.TileImage.Clone(), this.TileImage.Clone(), out this.TileImage); // (20181224) Jeff Revised!
                    //}
                    hwindow_.SetFullImagePart(new HImage(this.TileImage));
                    //HOperatorSet.DispColor(this.TileImage, hwindow_.HalconWindow);
                    HOperatorSet.DispObj(this.TileImage, hwindow_.HalconWindow);
                    HOperatorSet.SetPart(hwindow_.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part
                    b_status_ = true;
                }
            }
            catch
            { }

            return b_status_;
        }

        /// <summary>
        /// 計算每個影像位置包含之完整Cell顆數
        /// </summary>
        /// <returns></returns>
        public bool CountCell_zone() // (20181003) Jeff Revised!
        {
            try
            {
                CellCount_zone = new HTuple();

                // Local iconic variables 
                HObject ho_Rect_zone = null, ho_Rects = null;

                // Local control variables
                HTuple hv_c1 = new HTuple(), hv_r1 = new HTuple(), hv_c2 = new HTuple(), hv_r2 = new HTuple();
                HTuple hv_Number = new HTuple();
                // Initialize local and output iconic variables 
                HOperatorSet.GenEmptyObj(out ho_Rect_zone);
                HOperatorSet.GenEmptyObj(out ho_Rects);

                int Method = 3;
                if (Method == 1)
                {
                    #region Method 1

                    // Local control variables 
                    HTuple hv_MaxArea = null, hv_MinArea = null;
                    HTuple hv_n = null, hv_Index = null;

                    // 找所有Cell的面積最小/大值
                    HOperatorSet.TupleMax(area_cellmap_affine_, out hv_MaxArea);
                    HOperatorSet.TupleMin(area_cellmap_affine_, out hv_MinArea);

                    // 
                    hv_n = new HTuple(center_points_x_.TupleLength());
                    HTuple end_val20 = hv_n;
                    HTuple step_val20 = 1;
                    for (hv_Index = 1; hv_Index.Continue(end_val20, step_val20); hv_Index = hv_Index.TupleAdd(step_val20))
                    {
                        hv_c1 = center_points_x_.TupleSelect(hv_Index - 1);
                        hv_r1 = center_points_y_.TupleSelect(hv_Index - 1);
                        hv_c2 = (hv_c1 + frame_pwidth_) - 1;
                        hv_r2 = (hv_r1 + frame_pheight_) - 1;
                        ho_Rect_zone.Dispose();
                        HOperatorSet.GenRectangle1(out ho_Rect_zone, hv_r1, hv_c1, hv_r2, hv_c2);
                        ho_Rects.Dispose();
                        HOperatorSet.Intersection(cellmap_affine_, ho_Rect_zone, out ho_Rects);
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.SelectShape(ho_Rects, out ExpTmpOutVar_0, "area", "and", hv_MinArea, hv_MaxArea);
                            ho_Rects.Dispose();
                            ho_Rects = ExpTmpOutVar_0;
                        }
                        HOperatorSet.CountObj(ho_Rects, out hv_Number);
                        HOperatorSet.TupleConcat(CellCount_zone, hv_Number, out CellCount_zone);
                    }

                    #endregion
                }
                else if (Method == 2 || Method == 3) // (20190627) Jeff Revised!
                {
                    #region Method 2 or 3
                    
                    for (int i = 1; i <= center_points_x_.TupleLength(); i++)
                    {
                        ho_Rect_zone.Dispose();
                        if (Method == 2)
                        {
                            hv_c1 = center_points_x_.TupleSelect(i - 1);
                            hv_r1 = center_points_y_.TupleSelect(i - 1);
                            hv_c2 = (hv_c1 + frame_pwidth_) - 1;
                            hv_r2 = (hv_r1 + frame_pheight_) - 1;
                            HOperatorSet.GenRectangle1(out ho_Rect_zone, hv_r1, hv_c1, hv_r2, hv_c2);
                        }
                        else if (Method == 3)
                            HOperatorSet.SelectObj(capture_map_, out ho_Rect_zone, i);

                        ho_Rects.Dispose();
                        HOperatorSet.Intersection(cellmap_affine_, ho_Rect_zone, out ho_Rects);

                        // 計算此位置包含之完整Cell顆數
                        HTuple hv_Area_allCells2 = new HTuple(), hv_Equal = new HTuple(), hv_Indices = new HTuple();
                        HOperatorSet.RegionFeatures(ho_Rects, "area", out hv_Area_allCells2);
                        HOperatorSet.TupleEqualElem(area_cellmap_affine_, hv_Area_allCells2, out hv_Equal);
                        HOperatorSet.TupleFind(hv_Equal, 1, out hv_Indices);
                        ho_Rects.Dispose();
                        if ((int)(new HTuple(hv_Indices.TupleNotEqual(-1))) == 1)
                        {
                            hv_Indices = hv_Indices + 1;
                            HOperatorSet.SelectObj(cellmap_affine_, out ho_Rects, hv_Indices);

                        }
                        else
                            HOperatorSet.GenEmptyObj(out ho_Rects);

                        HOperatorSet.CountObj(ho_Rects, out hv_Number);
                        HOperatorSet.TupleConcat(CellCount_zone, hv_Number, out CellCount_zone);
                    }
                    
                    #endregion
                }

                ho_Rect_zone.Dispose();
                ho_Rects.Dispose();

                // 將每個影像位置包含之完整Cell顆數儲存至.txt檔
                string str = ((int)(CellCount_zone.TupleSelect(0))).ToString();
                int total_cell = (int)(CellCount_zone.TupleSelect(0)); // 驗證總完整Cell數是否正確
                for (int i = 2; i <= CellCount_zone.TupleLength(); i++)
                {
                    //str += "\n" + ((int)(CellCount_zone.TupleSelect(i - 1))).ToString();
                    str += "     \n" + ((int)(CellCount_zone.TupleSelect(i - 1))).ToString();
                    total_cell += (int)(CellCount_zone.TupleSelect(i - 1)); // For debug!
                }
                File.WriteAllText(File_directory_ + "\\" + "CellCount.txt", str);
            }
            catch
            {
                return false;
            }

            return true;
        }

        private Object lock_CreateCellMap { get; set; } = new object(); // (20191214) Jeff Revised!
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwindow_"></param>
        /// <param name="start_img_">第一張檢測影像</param>
        /// <param name="b_Parallel">是否執行平行運算</param>
        /// <returns></returns>
        public bool Create_cell_map(HSmartWindowControl hwindow_, HObject start_img_, bool b_Parallel = false) // (20191216) MIL Jeff Revised!
        {
            bool b_status_ = false;

            if (!(LocateMethodRecipe.VisualPosEnable)) // (20191216) MIL Jeff Revised!
            {
#if (showTimeSpan)
                sw.Reset(); // 碼表歸零
                sw.Start(); // 碼表開始計時
#endif

                b_status_ = create_cell_map2_NoPos(b_Parallel);

#if (showTimeSpan)
                sw.Stop(); // 碼錶停止
                elapseTime = sw.Elapsed.TotalMilliseconds;
#endif
                return b_status_;
            }

            // Resize
            origin_mark_pos_x_resize = origin_mark_pos_x_[0].D * resize_;
            origin_mark_pos_y_resize = origin_mark_pos_y_[0].D * resize_;
            HTuple dist_region_rdx_HTuple_resize = dist_region_rdx_HTuple * resize_;
            HTuple dist_region_rdy_HTuple_resize = dist_region_rdy_HTuple * resize_;
            HObject start_img_resize;
            HOperatorSet.ZoomImageFactor(start_img_, out start_img_resize, resize_, resize_, "bilinear");

            HObject start_affine_img_resize;
            HTuple affine_mark_1_pos_x_, affine_mark_1_pos_y_;

            HOperatorSet.HomMat2dIdentity(out hommat2d_affine_);
            HOperatorSet.HomMat2dRotate(hommat2d_affine_, affine_degree_.TupleRad(), 0, 0, out hommat2d_affine_);
            HOperatorSet.AffineTransImage(start_img_resize, out start_affine_img_resize, hommat2d_affine_, "constant", "false");
            // 扭正後之定位點位置 (For Mark AffineImage 1)
            /* Method1:  */
            HOperatorSet.AffineTransPixel(hommat2d_affine_, origin_mark_pos_y_resize, origin_mark_pos_x_resize, out affine_mark_1_pos_y_, out affine_mark_1_pos_x_);
            /* Method2: 扭正前後之第一個定位點位置不變 (i.e. 相對第一個定位點位置做扭正) (20181226) Jeff Revised! */
            //affine_mark_1_pos_y_ = origin_mark_pos_y_resize;
            //affine_mark_1_pos_x_ = origin_mark_pos_x_resize;

            // 如果實際運動(機械)座標和影像座標方向不同，必須做此轉換 (i.e. 以第一張檢測位置當成是原點)
            // Note: 全自動機台之ME1軸方向與影像X軸方向相反
            HTuple Mark1_rpos_x_ = null, Start_rpos_x_ = null;
            if (Convert.ToBoolean(X_dir_inv)) // 運動與影像座標之X方向反向 (20180901) Jeff Revised!
            {
                Mark1_rpos_x_ = start_rpos_x_ - mark1_rpos_x_;
                Start_rpos_x_ = 0.0;
            }
            else
            {
                Mark1_rpos_x_ = mark1_rpos_x_;
                Start_rpos_x_ = start_rpos_x_;
            }
            // Note: 全自動機台之MB1/MB2軸方向與影像Y軸方向相同
            HTuple Mark1_rpos_y_ = null, Start_rpos_y_ = null;
            if (Convert.ToBoolean(Y_dir_inv)) // 運動與影像座標之Y方向反向 (20180901) Jeff Revised!
            {
                Mark1_rpos_y_ = start_rpos_y_ - mark1_rpos_y_;
                Start_rpos_y_ = 0.0;
            }
            else
            {
                Mark1_rpos_y_ = mark1_rpos_y_;
                Start_rpos_y_ = start_rpos_y_;
            }

            // 單位轉換: mm → pixel
            double mm2pixel = 1000 / pixel_resolution_;
            HTuple Mark1_ppos_x_ = Mark1_rpos_x_ * mm2pixel;
            HTuple Mark1_ppos_y_ = Mark1_rpos_y_ * mm2pixel; // (20180901) Jeff Revised!
            HTuple Start_ppos_x_ = Start_rpos_x_ * mm2pixel;
            HTuple Start_ppos_y_ = Start_rpos_y_ * mm2pixel; // (20180901) Jeff Revised!
            HTuple dist_region_pdx = dist_region_rdx_HTuple_resize * mm2pixel;
            HTuple dist_region_pdy = dist_region_rdy_HTuple_resize * mm2pixel;
            HTuple size_sample_px = size_sample_rx * mm2pixel;
            HTuple size_sample_py = size_sample_ry * mm2pixel;

            // 扭正後之第一顆cell中心位置 (For 第一張檢測影像扭正)
            HTuple affine_fc_x_ = affine_mark_1_pos_x_ - (Start_ppos_x_ - Mark1_ppos_x_) + Mark_pdx_;
            HTuple affine_fc_y_ = affine_mark_1_pos_y_ - (Start_ppos_y_ - Mark1_ppos_y_) + Mark_pdy_;
#if (DEBUG)
            hwindow_.SetFullImagePart(new HImage(start_affine_img_resize));
            HOperatorSet.DispColor(start_affine_img_resize, hwindow_.HalconWindow);
            HOperatorSet.SetColor(hwindow_.HalconWindow, "red");
            HOperatorSet.SetDraw(hwindow_.HalconWindow, "fill");
            // 檢查扭正後第一顆【cell中心位置】是否正確 (For 第一張檢測影像扭正)
            HOperatorSet.DispCircle(hwindow_.HalconWindow, affine_fc_y_, affine_fc_x_, 10);
            HOperatorSet.SetPart(hwindow_.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part
            MessageBox.Show("Press button to continue", "Program stop");
#endif

            // 未扭正之第一顆cell中心位置 (For 第一張檢測影像)
            HOperatorSet.HomMat2dIdentity(out hommat2d_origin_);
            HOperatorSet.HomMat2dRotate(hommat2d_origin_, new HTuple(-affine_degree_).TupleRad(), 0, 0, out hommat2d_origin_);
            HOperatorSet.AffineTransPixel(hommat2d_origin_, affine_fc_y_, affine_fc_x_, out origin_fc_y_, out origin_fc_x_);
#if (DEBUG)
            HOperatorSet.DispColor(start_img_resize, hwindow_.HalconWindow);
            // 檢查未扭正第一顆cell位置是否正確 (For 第一張檢測影像)
            HOperatorSet.DispCircle(hwindow_.HalconWindow, origin_fc_y_, origin_fc_x_, 10);
            HOperatorSet.SetPart(hwindow_.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part
            MessageBox.Show("Press button to continue", "Program stop");
#endif

            // Release memories (20190624) Jeff Revised!
            start_img_resize.Dispose();
            start_affine_img_resize.Dispose();

#if (showTimeSpan)
            sw.Reset(); // 碼表歸零
            sw.Start(); // 碼表開始計時
#endif

            #region 開始建立cell全域地圖 cellmap_sortregion_

            HOperatorSet.SetSystem("height", size_sample_py.TupleInt());
            HOperatorSet.SetSystem("width", size_sample_px.TupleInt());

            HObject cellmap_region_, rect_group;
            //HOperatorSet.GenEmptyRegion(out cellmap_region_);
            //HOperatorSet.GenEmptyRegion(out rect_group);
            HOperatorSet.GenEmptyObj(out cellmap_region_); // (20190624) Jeff Revised!
            HOperatorSet.GenEmptyObj(out rect_group); // (20190624) Jeff Revised!
            
            #region y方向

            for (int y_reg = 0; y_reg < num_region_y_; y_reg++)
            {
                if (!b_Parallel)
                {
                    /* 法1 */
                    for (int y = 0; y < cell_y_count_HTuple[y_reg]; y++)
                    {
                        HObject cell_rect_;
                        HOperatorSet.GenRectangle2(out cell_rect_, affine_fc_y_ + cell_pdy_ * y + dist_region_pdy[y_reg], affine_fc_x_, 0, cell_pwidth_ / 2, cell_pheight_ / 2);
                        //HOperatorSet.Union2(rect_group, cell_rect_, out rect_group);
                        HOperatorSet.ConcatObj(rect_group, cell_rect_, out rect_group); // (20190306) Jeff Revised!
                        cell_rect_.Dispose();
                        // (20190401) Jeff Revised!
                        //GC.Collect();
                        //GC.WaitForPendingFinalizers();
                    }
                }
                else // Note: 如果Cell顆數不多時，平行運算反而比較慢!!!
                {
                    /* 法2: 平行運算 (Parallel Programming) */
                    bool b_error_Parallel = false; // 平行運算中，是否遇到例外狀況
                    Parallel.For(0, cell_y_count_HTuple[y_reg], (y, loopState) =>
                    {
                        try
                        {
                            HObject cell_rect_;
                            HOperatorSet.GenRectangle2(out cell_rect_, affine_fc_y_ + cell_pdy_ * y + dist_region_pdy[y_reg], affine_fc_x_, 0, cell_pwidth_ / 2, cell_pheight_ / 2);
                            // 法2-1: 同時存取rect_group時，會有問題!
                            //HOperatorSet.ConcatObj(rect_group, cell_rect_, out rect_group); // (20190306) Jeff Revised!
                            // 法2-2: 鎖住資源
                            lock (this)
                                HOperatorSet.ConcatObj(rect_group, cell_rect_, out rect_group);
                            // 法2-3: 鎖住資源 (20191214) Jeff Revised!
                            //lock (lock_CreateCellMap)
                            //    HOperatorSet.ConcatObj(rect_group, cell_rect_, out rect_group);

                            cell_rect_.Dispose();
                        }
                        catch
                        {
                            // 停止並退出Parallel.For
                            loopState.Stop();
                            b_error_Parallel = true;
                            return;
                        }
                    });

                    if (b_error_Parallel) // 判斷平行運算中，是否遇到例外狀況
                    {
                        #region Release memories
                        cellmap_region_.Dispose();
                        rect_group.Dispose();
                        #endregion
                        return b_status_;
                    }
                }
            }

            #endregion

            #region x方向

            for (int x_reg = 0; x_reg < num_region_x_; x_reg++)
            {
                if (!b_Parallel)
                {
                    /* 法1 */
                    for (int x = 0; x < cell_x_count_HTuple[x_reg]; x++)
                    {
                        HObject moved;
                        HOperatorSet.MoveRegion(rect_group, out moved, 0, cell_pdx_ * x + dist_region_pdx[x_reg]);
                        //HOperatorSet.Union2(cellmap_region_, moved, out cellmap_region_);
                        HOperatorSet.ConcatObj(cellmap_region_, moved, out cellmap_region_); // (20190306) Jeff Revised!
                        moved.Dispose();
                    }
                }
                else // Note: 如果Cell顆數不多時，平行運算反而比較慢!!!
                {
                    /* 法2: 平行運算 (Parallel Programming) */
                    bool b_error_Parallel = false; // 平行運算中，是否遇到例外狀況
                    Parallel.For(0, cell_x_count_HTuple[x_reg], (x, loopState) =>
                    {
                        try
                        {
                            HObject moved;
                            HOperatorSet.MoveRegion(rect_group, out moved, 0, cell_pdx_ * x + dist_region_pdx[x_reg]);
                            // 法2-1: 同時存取rect_group時，會有問題!
                            //HOperatorSet.ConcatObj(cellmap_region_, moved, out cellmap_region_); // (20190306) Jeff Revised!
                            // 法2-2: 鎖住資源
                            lock (this)
                                HOperatorSet.ConcatObj(cellmap_region_, moved, out cellmap_region_);
                            // 法2-3: 鎖住資源 (20191214) Jeff Revised!
                            //lock (lock_CreateCellMap)
                            //    HOperatorSet.ConcatObj(cellmap_region_, moved, out cellmap_region_);

                            moved.Dispose();
                        }
                        catch
                        {
                            // 停止並退出Parallel.For
                            loopState.Stop();
                            b_error_Parallel = true;
                            return;
                        }
                    });

                    if (b_error_Parallel) // 判斷平行運算中，是否遇到例外狀況
                    {
                        #region Release memories
                        cellmap_region_.Dispose();
                        rect_group.Dispose();
                        #endregion
                        return b_status_;
                    }
                }
            }

            #endregion

            //HOperatorSet.Connection(cellmap_region_, out cellmap_region_); // (20190624) Jeff Revised!
            HOperatorSet.SortRegion(cellmap_region_, out cellmap_sortregion_, "first_point", "true", "row");

            // Release memories (20190624) Jeff Revised!
            cellmap_region_.Dispose();
            rect_group.Dispose();

            #endregion

#if (showTimeSpan)
            sw.Stop(); // 碼錶停止
            elapseTime = sw.Elapsed.TotalMilliseconds;
#endif

            // 建立 Marks' center 在大拼接圖上(已扭正)之位置 (20181226) Jeff Revised!
            //HOperatorSet.GenCircle(out MarkCenter_BigMap_affine, affine_fc_y_ - Mark_pdy_, affine_fc_x_ - Mark_pdx_, 10);
            HTuple pos_mark1_pdy, pos_mark1_pdx;
            HOperatorSet.AffineTransPixel(hommat2d_origin_, affine_fc_y_ - Mark_pdy_, affine_fc_x_ - Mark_pdx_, out pos_mark1_pdy, out pos_mark1_pdx);
            HOperatorSet.GenCircle(out MarkCenter_BigMap_orig, pos_mark1_pdy, pos_mark1_pdx, 10);
            //HOperatorSet.GenCircle(out MarkCenter_BigMap_orig, origin_fc_y_ - Mark_pdy_, origin_fc_x_ - Mark_pdx_, 10);
            
            b_status_ = true;

            return b_status_;
        }

        /// <summary>
        /// 計算Cell的寬高
        /// </summary>
        /// <param name="hwindow_">預覽UI</param>
        /// <param name="mark_1_img_">定位點影像_1(已扭正)</param>
        /// <param name="cell_height_">Cell高</param>
        /// <param name="cell_width_">Cell寬</param>
        /// <returns></returns>
        public bool cal_cell_size(HWindowControl hwindow_, HObject mark_1_affine_img_, out double cell_pheight_, out double cell_pwidth_)
        {
            bool b_status_ = false;
            HObject mark_1_gray_img_, roi_region_, roi_img_;
            HObject thred_region_, mark_region_;
            HTuple roi_r1_, roi_r2_, roi_c1_, roi_c2_;
            HTuple mark_area_, mark_row_, mark_col_;
            HTuple rect_r1_, rect_r2_, rect_c1_, rect_c2_;
            HTuple cell_area_, cell_row_, cell_col_;
            cell_pheight_ = 0;
            cell_pwidth_ = 0;


            HOperatorSet.Rgb1ToGray(mark_1_affine_img_, out mark_1_gray_img_);
            HOperatorSet.DrawRectangle1(hwindow_.HalconWindow, out roi_r1_, out roi_r2_, out roi_c1_, out roi_c2_);
            HOperatorSet.GenRectangle1(out roi_region_, roi_r1_, roi_r2_, roi_c1_, roi_c2_);
            HOperatorSet.ReduceDomain(mark_1_gray_img_, roi_region_, out roi_img_);

            HOperatorSet.Threshold(roi_img_, out thred_region_, cs_thred_min_, cs_thred_max_);
            HOperatorSet.Connection(thred_region_, out thred_region_);
            HOperatorSet.SelectShape(thred_region_, out mark_region_, "area", "and", cs_area_min_, cs_area_max_);
            HOperatorSet.AreaCenter(mark_region_, out mark_area_, out mark_row_, out mark_col_);
            HOperatorSet.Union1(mark_region_, out mark_region_);
            HOperatorSet.SmallestRectangle1(mark_region_, out rect_r1_, out rect_c1_, out rect_r2_, out rect_c2_);

            if (mark_row_.Length == 0 && mark_region_ == null)
                b_status_ = false;
            else
            {
                HObject cell_region_;
                HOperatorSet.GenRectangle1(out cell_region_, mark_row_[0], rect_c1_, mark_row_[1], rect_c2_);
                HOperatorSet.AreaCenter(cell_region_, out cell_area_, out cell_row_, out cell_col_);
                cell_region_.Dispose();
                cell_pheight_ = mark_row_[1] - mark_row_[0];
                cell_pwidth_ = rect_c2_ - rect_c1_;
                b_status_ = true;
            }

            return b_status_;
        }

        /// <summary>
        /// 計算Cell的dx、dy
        /// </summary>
        /// <param name="dx_region_">已Reduce之dx區域</param>
        /// <param name="dy_region_">已Reduce之dy區域</param>
        /// <param name="cell_dx_">Cell dx數值</param>
        /// <param name="cell_dy_">Cell dy數值</param>
        /// <returns></returns>
        public bool cal_cell_dxdy(HObject dx_region_, HObject dy_region_, out double cell_dx_, out double cell_dy_)
        {
            bool b_status_ = true;
            HObject dx_thred_region_, dx_mark_region_, dy_thred_region_, dy_mark_region_;
            HTuple cell_dx_area_, cell_dx_row_, cell_dx_col_;
            HTuple cell_dy_area_, cell_dy_row_, cell_dy_col_;
            cell_dx_ = 0;
            cell_dy_ = 0;

            // 計算Cell之dx
            HOperatorSet.Threshold(dx_region_, out dx_thred_region_, cd_thred_min_, cd_thred_max_);
            HOperatorSet.Connection(dx_thred_region_, out dx_thred_region_);
            HOperatorSet.SelectShape(dx_thred_region_, out dx_mark_region_, "area", "and", cd_area_min_, cd_area_max_);
            HOperatorSet.AreaCenter(dx_mark_region_, out cell_dx_area_, out cell_dx_row_, out cell_dx_col_);

            if (cell_dx_row_.Length != 2)
                b_status_ = false;
            else
                cell_dx_ = Math.Abs(cell_dx_col_[1].D - cell_dx_col_[0].D);

            // 計算Cell之dy
            HOperatorSet.Threshold(dy_region_, out dy_thred_region_, cd_thred_min_, cd_thred_max_);
            HOperatorSet.Connection(dy_thred_region_, out dy_thred_region_);
            HOperatorSet.SelectShape(dy_thred_region_, out dy_mark_region_, "area", "and", cd_area_min_, cd_area_max_);
            HOperatorSet.AreaCenter(dy_mark_region_, out cell_dy_area_, out cell_dy_row_, out cell_dy_col_);

            if (cell_dy_row_.Length != 2)
                b_status_ = false;
            else
                cell_dy_ = Math.Abs(cell_dy_row_[1].D - cell_dy_row_[0].D);

            return b_status_;
        }

        /// <summary>
        /// 將 已扭正之標準cellmap 全域區域 (For 第一張檢測影像扭正) 轉回原來的 第一張檢測影像
        /// </summary>
        /// <param name="affine_cellmap_"></param>
        /// <returns></returns>
        public bool Cell_region_mapping()
        {
            bool b_status_ = false;

            HOperatorSet.HomMat2dIdentity(out hommat2d_origin_);
            HOperatorSet.HomMat2dRotate(hommat2d_origin_, new HTuple(-affine_degree_).TupleRad(), 0, 0, out hommat2d_origin_);
            Extension.HObjectMedthods.ReleaseHObject(ref cellmap_affine_);
            HOperatorSet.AffineTransRegion(cellmap_sortregion_, out cellmap_affine_, hommat2d_origin_, "nearest_neighbor");
            //cellmap_affine_ = affine_cellmap_.Clone();
            //if (cellmap_affine_ != null) // Release memory (20190405) Jeff Revised!
            //{
            //    cellmap_affine_.Dispose();
            //    cellmap_affine_ = null;
            //}
            HOperatorSet.RegionFeatures(cellmap_affine_, "area", out area_cellmap_affine_); // (20190701) Jeff Revised!

            // 將 Marks中心位置之region 轉回未扭正時之位置 (20181226) Jeff Revised!
            //if (MarkCenter_BigMap_affine != null)
            //    HOperatorSet.AffineTransRegion(MarkCenter_BigMap_affine, out MarkCenter_BigMap_orig, hommat2d_origin_, "nearest_neighbor");

            // 消除 Bypass Cell
            HObject Reg = Compute_CellReg_ListCellID(LocateMethodRecipe.ListPt_BypassCell); // (20190822) Jeff Revised!
            {
                HObject ExpTmpOutVar_0;
                HOperatorSet.Difference(cellmap_affine_, Reg, out ExpTmpOutVar_0);
                cellmap_affine_.Dispose();
                cellmap_affine_ = ExpTmpOutVar_0;
            }
            Reg.Dispose();

            b_status_ = true;

            return b_status_;
        }

        /// <summary>
        /// 計算目前基板之第一顆cell實際位置
        /// 得到 affine_fc_x_, affine_fc_y_, origin_fc_x_, origin_fc_y_
        /// </summary>
        /// <param name="hwindow_">預覽UI</param>
        /// <param name="mark_1_img_">第一張定位影像 (Mark1影像)</param>
        /// <returns></returns>
        private bool cal_cell_realpos(HWindowControl hwindow_, HObject mark_1_img_)
        {
            bool b_status_ = false;
            HObject mark_1_affine_img_;
            HTuple affine_mark_1_pos_x_, affine_mark_1_pos_y_;
            HTuple affine_fc_x_, affine_fc_y_;

            HOperatorSet.HomMat2dIdentity(out hommat2d_affine_);
            HOperatorSet.HomMat2dRotate(hommat2d_affine_, affine_degree_.TupleRad(), 0, 0, out hommat2d_affine_);
            HOperatorSet.AffineTransImage(mark_1_img_, out mark_1_affine_img_, hommat2d_affine_, "constant", "false");
            // 扭正後之定位點位置 (For Mark AffineImage 1)
            HOperatorSet.AffineTransPixel(hommat2d_affine_, origin_mark_pos_y_[0].D, origin_mark_pos_x_[0].D, out affine_mark_1_pos_y_, out affine_mark_1_pos_x_);
            HOperatorSet.SetColor(hwindow_.HalconWindow, "red");
            // 扭正後之第一顆cell中心位置 (For Mark AffineImage 1)
            affine_fc_x_ = affine_mark_1_pos_x_ + mark_pdx_;
            affine_fc_y_ = affine_mark_1_pos_y_+ mark_pdy_;
#if DEBUG
            //// 檢查【mark中心位置】扭正是否正確 (Mark AffineImage 1)
            //HObject check_affine_mark1_center_;
            //HOperatorSet.GenCircle(out check_affine_mark1_center_, affine_mark_1_pos_y_, affine_mark_1_pos_x_, 10);
            //// 檢查扭正後第一顆【cell中心位置】是否正確 (Mark AffineImage 1)
            //HObject check_affine_mark1_fc_;
            //HOperatorSet.GenCircle(out check_affine_mark1_fc_, affine_mark_1_pos_y_, affine_mark_1_pos_x_, 10);

            //HOperatorSet.SetColor(hwindow_.HalconWindow, "red");
            //HOperatorSet.DispColor(mark_1_affine_img_, hwindow_.HalconWindow);
            //HOperatorSet.DispRegion(check_affine_mark1_center_, hwindow_.HalconWindow);
            //HOperatorSet.DispRegion(check_affine_mark1_fc_, hwindow_.HalconWindow);
            //MessageBox.Show("Press button to continue", "Program stop");
#endif

            //******************************************
            // 未扭正之第一顆cell中心位置 (For Mark Image 1)
            HOperatorSet.HomMat2dIdentity(out hommat2d_origin_);
            //HOperatorSet.HomMat2dRotate(hommat2d_origin_, new HTuple(360 - affine_degree_).TupleRad(), 0, 0, out hommat2d_origin_);
            //HOperatorSet.HomMat2dRotate(hommat2d_origin_, new HTuple(360).TupleRad() - affine_degree_.TupleRad(), 0, 0, out hommat2d_origin_);
            HOperatorSet.HomMat2dRotate(hommat2d_origin_, new HTuple(-affine_degree_).TupleRad(), 0, 0, out hommat2d_origin_); // (20180825) Jeff Revised!
            HOperatorSet.AffineTransPixel(hommat2d_origin_, affine_fc_y_, affine_fc_x_, out origin_fc_y_, out origin_fc_x_);
#if DEBUG
            //// 檢查未扭正第一顆cell位置是否正確 (Mark Image 1)
            //HObject check_origin_mark1_fc_;
            //HOperatorSet.GenCircle(out check_origin_mark1_fc_, origin_fc_y_, origin_fc_x_, 10);
            //HOperatorSet.SetColor(hwindow_.HalconWindow, "red");
            //HOperatorSet.DispColor(mark_1_img_, hwindow_.HalconWindow);
            //HOperatorSet.DispRegion(check_origin_mark1_fc_, hwindow_.HalconWindow);
            //MessageBox.Show("Press button to continue", "Program stop");
#endif
            b_status_ = true; // (20180825) Jeff Revised!

            return b_status_;
        }

        /// <summary>
        /// 將 defect region 標記於大圖上，並且計算所有瑕疵所在cell之id座標
        /// Note: 針對不同瑕疵類型，可以分別設定是否考慮背景瑕疵
        /// </summary>
        /// <param name="capture_index_">第一個檢測位置為1</param>
        /// <param name="defect_region_"></param>
        /// <param name="intersection_defect_"></param>
        /// <param name="defect_id_">該位置瑕疵id座標</param>
        /// <param name="BackDefect">背景瑕疵</param>
        /// <param name="b_Update_cellID">是否更新瑕疵座標</param>
        /// <param name="b_Update_defect_CellReg">是否更新 all_intersection_defect_CellReg</param>
        /// <returns></returns>
        public bool label_defect(int capture_index_, HObject defect_region_, out HObject intersection_defect_, out List<Point> defect_id_, out HObject BackDefect, 
                                 bool b_Update_cellID = true, bool b_Update_defect_CellReg = true) // (20190715) Jeff Revised!
        {
            bool b_status_ = false;
            defect_id_ = new List<Point>();
            HOperatorSet.GenEmptyRegion(out intersection_defect_);
            HOperatorSet.GenEmptyRegion(out BackDefect);

            try // (20190624) Jeff Revised!
            {
                HObject defect_region_zoom, defect_move_region_;
                if (this.all_intersection_defect_ == null)
                    HOperatorSet.GenEmptyRegion(out this.all_intersection_defect_);
                HOperatorSet.ZoomRegion(defect_region_, out defect_region_zoom, this.resize_, this.resize_); // (20190306) Jeff Revised!
                HOperatorSet.MoveRegion(defect_region_zoom, out defect_move_region_, this.center_points_y_[capture_index_ - 1], this.center_points_x_[capture_index_ - 1]);
                intersection_defect_.Dispose();
                HOperatorSet.Intersection(this.cellmap_affine_, defect_move_region_, out intersection_defect_); // 消除掉非cell region
                HOperatorSet.Union2(this.all_intersection_defect_, intersection_defect_, out this.all_intersection_defect_);

                // 背景瑕疵 (20181115) Jeff Revised!
                if (this.b_BackDefect)
                {
                    if (this.all_BackDefect_ == null)
                        HOperatorSet.GenEmptyRegion(out this.all_BackDefect_);
                    BackDefect.Dispose();
                    HOperatorSet.Difference(defect_move_region_, intersection_defect_, out BackDefect);
                    HOperatorSet.Union2(this.all_BackDefect_, BackDefect, out this.all_BackDefect_);
                }
                defect_region_zoom.Dispose();
                defect_move_region_.Dispose();

                #region 是否更新瑕疵座標 Note: 全部檢測完畢再更新，以節省時間!

                if (b_Update_cellID) // (20190703) Jeff Revised!
                {
                    if (clsStaticTool.Count_HObject(intersection_defect_) > 0)
                    {
                        HTuple cell_ids = new HTuple(); // (20190706) Jeff Revised!
                        int Method = 2;
                        if (Method == 1) // Method 2 速度較快!
                        {
                            HTuple area_defect_, row_defect_, col_defect_;
                            HOperatorSet.AreaCenter(intersection_defect_, out area_defect_, out row_defect_, out col_defect_);
                            for (int index_ = 1; index_ <= area_defect_.Length; index_++) // area_defect_.Length: 總Cell顆數
                            {
                                if (area_defect_[index_ - 1].I > 0) // defect region
                                    HOperatorSet.TupleConcat(cell_ids, index_, out cell_ids); // index_: 最左上角是1，由左到右，由上到下遞增
                            }
                        }
                        else if (Method == 2) // (20190701) Jeff Revised!
                        {
                            HTuple hv_Area = new HTuple(), hv_Greater = new HTuple(), hv_Indices = new HTuple();
                            HOperatorSet.RegionFeatures(intersection_defect_, "area", out hv_Area);
                            HOperatorSet.TupleGreaterElem(hv_Area, 0, out hv_Greater);
                            HOperatorSet.TupleFind(hv_Greater, 1, out hv_Indices);
                            if ((int)(new HTuple(hv_Indices.TupleNotEqual(-1))) == 1)
                                cell_ids = hv_Indices + 1; // index_: 最左上角是1，由左到右，由上到下遞增
                        }

                        if (cell_ids.Length > 0) // (20190706) Jeff Revised!
                        {
                            List<Point> ListCellID = HTuple_2_ListCellID(cell_ids);
                            foreach (Point pt in ListCellID)
                            {
                                defect_id_.Add(pt);
                                if (!this.all_defect_id_.Contains(pt))
                                    this.all_defect_id_.Add(pt);
                            }
                        }
                    }
                }
                else if (capture_index_ == this.array_x_count_ * this.array_y_count_) // (20190703) Jeff Revised!
                {
                    HObject reg_NoUse;
                    HOperatorSet.GenEmptyRegion(out reg_NoUse);
                    HTuple cell_ids = new HTuple();
                    this.label_region_1_Defect(-1, this.all_intersection_defect_, reg_NoUse, out reg_NoUse, out cell_ids, this.all_defect_id_, out this.all_defect_id_, true);
                    reg_NoUse.Dispose();
                }

                #endregion

                /* 是否更新 all_intersection_defect_CellReg Note: 全部檢測完畢再更新，以節省時間! */
                if (b_Update_defect_CellReg || capture_index_ == this.array_x_count_ * this.array_y_count_) // (20190715) Jeff Revised!
                {
                    Extension.HObjectMedthods.ReleaseHObject(ref this.all_intersection_defect_CellReg);
                    //HOperatorSet.SelectObj(this.cellmap_affine_, out this.all_intersection_defect_CellReg, this.ListCellID_2_HTuple(this.all_defect_id_));
                    this.all_intersection_defect_CellReg = this.Compute_CellReg_ListCellID(this.all_defect_id_);
                }

                b_status_ = true;
            }
            catch
            { }

            return b_status_;
        }
        
        /// <summary>
        /// 計算各瑕疵所在cell之id座標
        /// Note: 瑕疵分類開啟，才需執行!
        /// </summary>
        /// <param name="capture_index_">第一個檢測位置為1</param>
        /// <param name="defects_with_names">帶有各瑕疵名稱及瑕疵region之Dictionary</param>
        /// <param name="b_Do_Priority">是否針對Priority做瑕疵結果更新</param>
        /// <param name="b_Update_cellID">是否更新瑕疵座標</param>
        /// <returns></returns>
        public bool label_defect_DefectsClassify(int capture_index_, Dictionary<string, HObject> defects_with_names, bool b_Do_Priority = false, bool b_Update_cellID = false) // (20200429) Jeff Revised!
        {
            bool b_status_ = false;

            if (!this.b_Defect_Classify)
                return false;

            try
            {
                /* 計算 all_intersection_defect_Origin & all_defect_id_Origin */
                foreach (string Name in defects_with_names.Keys)
                {
                    // 判斷是否包含此瑕疵名稱
                    if (!this.DefectsClassify.ContainsKey(Name))
                        continue;

                    HTuple cell_ids = new HTuple();
                    DefectsResult dr = this.DefectsClassify[Name];
                    if (!(this.label_region_1_Defect(capture_index_, defects_with_names[Name], dr.all_intersection_defect_Origin, out dr.all_intersection_defect_Origin, out cell_ids, dr.all_defect_id_Origin, out dr.all_defect_id_Origin, b_Update_cellID)))
                        return false;

                    /* 是否更新 all_intersection_defect_Origin_CellReg Note: 全部檢測完畢再更新，以節省時間! */
                    if (b_Update_cellID || capture_index_ == this.array_x_count_ * this.array_y_count_) // (20190715) Jeff Revised!
                    {
                        // 更新 all_intersection_defect_Origin_CellReg
                        Extension.HObjectMedthods.ReleaseHObject(ref dr.all_intersection_defect_Origin_CellReg);
                        HOperatorSet.SelectObj(this.cellmap_affine_, out dr.all_intersection_defect_Origin_CellReg, cell_ids);
                    }
                }

                /* 瑕疵優先權啟用時，需更新 Note: 全部檢測完畢再更新，以節省時間! */
                if (b_Do_Priority || capture_index_ == this.array_x_count_ * this.array_y_count_)
                {
                    if (this.b_Defect_priority)
                        this.Update_DefectsResult_Priority();
                }

                b_status_ = true;
            }
            catch
            { }

            return b_status_;
        }

        /// <summary>
        /// 計算單一瑕疵所在cell之id座標
        /// </summary>
        /// <param name="capture_index_"> =-1 (大圖瑕疵region), >=1 (各取像位置) </param>
        /// <param name="defect_region_"></param>
        /// <param name="all_inter_defect"></param>
        /// <param name="All_inter_defect"></param>
        /// <param name="cell_ids">該瑕疵之id座標所在cellmap_affine_之index</param>
        /// <param name="all_def_id"></param>
        /// <param name="All_def_id"></param>
        /// <param name="b_Update_cellID">是否更新瑕疵座標</param>
        /// <returns></returns>
        public bool label_region_1_Defect(int capture_index_, HObject defect_region_, HObject all_inter_defect, out HObject All_inter_defect, out HTuple cell_ids,
                                          List<Point> all_def_id, out List<Point> All_def_id, bool b_Update_cellID = false) // (20200429) Jeff Revised!
        {
            bool b_status_ = false;
            HOperatorSet.GenEmptyRegion(out All_inter_defect);
            cell_ids = new HTuple();
            All_def_id = all_def_id.ToList();

            try
            {
                HObject defect_region_zoom, defect_move_region_, intersection_defect_;
                if (all_inter_defect == null)
                    HOperatorSet.GenEmptyRegion(out all_inter_defect);

                if (capture_index_ != -1) // 各取像位置
                {
                    HOperatorSet.ZoomRegion(defect_region_, out defect_region_zoom, this.resize_, this.resize_);
                    HOperatorSet.MoveRegion(defect_region_zoom, out defect_move_region_, center_points_y_[capture_index_ - 1], center_points_x_[capture_index_ - 1]);
                    defect_region_zoom.Dispose();
                    HOperatorSet.Intersection(cellmap_affine_, defect_move_region_, out intersection_defect_); // 消除掉非cell region
                    defect_move_region_.Dispose();
                }
                else // 大圖瑕疵region
                    HOperatorSet.Intersection(cellmap_affine_, defect_region_, out intersection_defect_); // 消除掉非cell region

                All_inter_defect.Dispose();
                HOperatorSet.Union2(all_inter_defect, intersection_defect_, out All_inter_defect);
                intersection_defect_.Dispose();

                #region 是否更新瑕疵座標 Note: 全部檢測完畢再更新，以節省時間!

                if (b_Update_cellID || capture_index_ == this.array_x_count_ * this.array_y_count_) // (20190703) Jeff Revised!
                {
                    HObject Reg;
                    HOperatorSet.Intersection(cellmap_affine_, All_inter_defect, out Reg); // 消除掉非cell region

                    if (clsStaticTool.Count_HObject(Reg) > 0)
                    {
                        int Method = 2; // Method 2 速度較快!
                        if (Method == 1)
                        {
                            HTuple area_defect_, row_defect_, col_defect_;
                            HOperatorSet.AreaCenter(Reg, out area_defect_, out row_defect_, out col_defect_);
                            for (int index_ = 1; index_ <= area_defect_.Length; index_++) // area_defect_.Length: 總Cell顆數
                            {
                                if (area_defect_[index_ - 1].I > 0) // defect region
                                    HOperatorSet.TupleConcat(cell_ids, index_, out cell_ids); // index_: 最左上角是1，由左到右，由上到下遞增
                            }
                        }
                        else if (Method == 2) // (20190701) Jeff Revised!
                        {
                            HTuple hv_Area = new HTuple(), hv_Greater = new HTuple(), hv_Indices = new HTuple();
                            HOperatorSet.RegionFeatures(Reg, "area", out hv_Area);
                            HOperatorSet.TupleGreaterElem(hv_Area, 0, out hv_Greater);
                            HOperatorSet.TupleFind(hv_Greater, 1, out hv_Indices);
                            if ((int)(new HTuple(hv_Indices.TupleNotEqual(-1))) == 1)
                                cell_ids = hv_Indices + 1; // index_: 最左上角是1，由左到右，由上到下遞增
                        }

                        if (cell_ids.Length > 0) // (20190706) Jeff Revised!
                        {
                            List<Point> ListCellID = HTuple_2_ListCellID(cell_ids);
                            foreach (Point pt in ListCellID)
                            {
                                if (!All_def_id.Contains(pt))
                                    All_def_id.Add(pt);
                            }
                        }

                    }

                    Reg.Dispose();
                }

                #endregion
                
                b_status_ = true;
            }
            catch
            { }

            return b_status_;
        }

        /// <summary>
        /// 紀錄一個位置影像檢測到的Cell中心點regions之座標
        /// </summary>
        /// <param name="capture_index_"></param>
        /// <param name="CellCenter_region_">一個位置影像檢測到的Cell中心點regions</param>
        /// <returns></returns>
        public bool label_CellCenter(int capture_index_, HObject CellCenter_region_) // (20200429) Jeff Revised!
        {
            bool b_status_ = false;

            try
            {
                List<int> Cell_id_array_ = new List<int>();
                HObject CellCenter_zoom_region_, CellCenter_move_region_, intersection_CellCenter_;
                HOperatorSet.ZoomRegion(CellCenter_region_, out CellCenter_zoom_region_, this.resize_, this.resize_);
                HOperatorSet.MoveRegion(CellCenter_zoom_region_, out CellCenter_move_region_, this.center_points_y_[capture_index_ - 1], this.center_points_x_[capture_index_ - 1]);
                HOperatorSet.Intersection(this.cellmap_affine_, CellCenter_move_region_, out intersection_CellCenter_); // 消除掉非cell region

                int METHOD = 1;
                if (METHOD == 1) // 每一位置影像先不計算all_Cell_id_，可以節省很多時間 (20190701) Jeff Revised!
                {
                    if (this.all_intersection_Cell_ == null)
                        HOperatorSet.GenEmptyRegion(out this.all_intersection_Cell_);
                    HOperatorSet.Union2(this.all_intersection_Cell_, intersection_CellCenter_, out this.all_intersection_Cell_);
                }
                else if (METHOD == 2)
                {
                    if (clsStaticTool.Count_HObject(intersection_CellCenter_) > 0)
                    {
                        int Method = 1;
                        if (Method == 1)
                        {
                            HTuple area_CellCenter_, row_CellCenter_, col_CellCenter_;
                            HOperatorSet.AreaCenter(intersection_CellCenter_, out area_CellCenter_, out row_CellCenter_, out col_CellCenter_);
                            for (int index_ = 1; index_ <= area_CellCenter_.Length; index_++) // area_CellCenter_.Length: 總Cell顆數
                            {
                                if (area_CellCenter_[index_ - 1].I > 0) // CellCenter region
                                    Cell_id_array_.Add(index_); // index_: 最左上角是1，由左到右，由上到下遞增
                            }
                        }
                        else if (Method == 2) // (20190701) Jeff Revised!
                        {
                            HTuple hv_Area = new HTuple(), hv_Greater = new HTuple(), hv_Indices = new HTuple();
                            HOperatorSet.RegionFeatures(intersection_CellCenter_, "area", out hv_Area);
                            HOperatorSet.TupleGreaterElem(hv_Area, 0, out hv_Greater);
                            HOperatorSet.TupleFind(hv_Greater, 1, out hv_Indices);
                            if ((int)(new HTuple(hv_Indices.TupleNotEqual(-1))) == 1)
                            {
                                hv_Indices = hv_Indices + 1;
                                // HTuple 轉 List<int>
                                Cell_id_array_ = clsStaticTool.HTuple_2_ListInt(hv_Indices);
                            }
                        }

                        foreach (int i in Cell_id_array_)
                        {
                            Point pt = this.int_2_cellID(i);
                            if (!this.all_Cell_id_.Contains(pt))
                                this.all_Cell_id_.Add(pt);
                        }
                    }
                }

                CellCenter_zoom_region_.Dispose();
                CellCenter_move_region_.Dispose();
                intersection_CellCenter_.Dispose();

                b_status_ = true;
            }
            catch
            { }

            return b_status_;
        }

        /// <summary>
        /// 一次紀錄所有未被檢測到的Cell中心點regions之座標，並將之加入至瑕疵座標 all_defect_id_ 中
        /// </summary>
        /// <param name="b_CellDefect">是否考慮未被檢測到的Cell中心點regions之座標</param>
        /// <returns></returns>
        public bool label_all_CellDefect(bool b_CellDefect = false) // (20200429) Jeff Revised!
        {
            // 清除 all_CellDefect_id_
            this.all_CellDefect_id_.Clear();
            this.all_CellDefect_id_ = new List<Point>();

            if (!b_CellDefect) // (20190422) Jeff Revised!
                return false;

#if (showTimeSpan)
            sw.Reset(); // 碼表歸零
            sw.Start(); // 碼表開始計時
#endif

            Log.WriteLog("label_all_CellDefect() starts");

            int METHOD = 1;
            if (METHOD == 1) // 每一位置影像先不計算all_Cell_id_，可以節省很多時間 (20190701) Jeff Revised!
            {
                if (this.all_intersection_Cell_ == null)
                    return false;

                HObject all_intersection_Cell_int;
                HOperatorSet.Intersection(cellmap_affine_, this.all_intersection_Cell_, out all_intersection_Cell_int);
                HTuple hv_Area = new HTuple(), hv_Indices = new HTuple();
                HOperatorSet.RegionFeatures(all_intersection_Cell_int, "area", out hv_Area);
                HOperatorSet.TupleFind(hv_Area, 0, out hv_Indices);
                
                if ((int)(new HTuple(hv_Indices.TupleNotEqual(-1))) == 1)
                {
                    hv_Indices = hv_Indices + 1;

                    if (hv_Indices.Length > 0) // (20190706) Jeff Revised!
                    {
                        List<Point> ListCellID = this.HTuple_2_ListCellID(hv_Indices);
                        foreach (Point pt in ListCellID)
                        {
                            if (this.LocateMethodRecipe.ListPt_BypassCell.Contains(pt)) // 忽略Cell之id (20190812) Jeff Revised!
                                continue;
                            this.all_CellDefect_id_.Add(pt);
                            if (!this.all_defect_id_.Contains(pt))
                                this.all_defect_id_.Add(pt);
                        }
                    }
                }

                all_intersection_Cell_int.Dispose();
            }
            else if (METHOD == 2)
            {
                for (int y = 1; y <= this.cell_row_count_; y++)
                {
                    for (int x = 1; x <= this.cell_col_count_; x++)
                    {
                        if (!this.all_Cell_id_.Contains(new Point(x, y))) // 未被檢測到的Cell中心點regions之座標
                        {
                            if (this.LocateMethodRecipe.ListPt_BypassCell.Contains(new Point(x, y))) // 忽略Cell之id (20190812) Jeff Revised!
                                continue;
                            this.all_CellDefect_id_.Add(new Point(x, y));
                            if (!this.all_defect_id_.Contains(new Point(x, y)))
                                this.all_defect_id_.Add(new Point(x, y));
                        }
                    }
                }
            }

            Log.WriteLog("label_all_CellDefect() ends");

#if (showTimeSpan)
            sw.Stop(); // 碼錶停止
            elapseTime = sw.Elapsed.TotalMilliseconds;
#endif

            // 更新 all_intersection_defect_CellReg
            Extension.HObjectMedthods.ReleaseHObject(ref this.all_intersection_defect_CellReg);
            this.all_intersection_defect_CellReg = this.Compute_CellReg_ListCellID(this.all_defect_id_);

            #region 新增瑕疵類型 【Cell瑕疵】

            if (this.b_Defect_Classify) // 多瑕疵
            {
                int i = 0;
                while (true)
                {
                    if (this.Add_New_DefectsClassify("Cell瑕疵", i, "#ff0000ff", true)) // (20190716) Jeff Revised!
                        break; // 新增瑕疵類型成功
                    i++;
                }

                /* 設定瑕疵資訊 */
                DefectsResult dr = this.DefectsClassify["Cell瑕疵"];
                dr.all_defect_id_Origin = this.all_CellDefect_id_.ToList();
                dr.Release(true, false, false);
                dr.all_intersection_defect_Origin = this.Compute_CellReg_ListCellID(this.all_CellDefect_id_);
                dr.all_intersection_defect_Origin_CellReg = dr.all_intersection_defect_Origin.Clone();

                if (this.b_Defect_priority) // 瑕疵優先權
                {
                    dr.all_defect_id_Priority = this.all_CellDefect_id_.ToList();
                    dr.Release(false, true, false);
                    dr.all_intersection_defect_Priority = dr.all_intersection_defect_Origin.Clone();
                    dr.all_intersection_defect_Priority_CellReg = dr.all_intersection_defect_Origin.Clone();
                }
            }

            #endregion

            return true;
        }

        /// <summary>
        /// 指定大拼接圖影像之任一點座標，計算其對應之Cell座標
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="cell_ids">cellmap_affine_ 之index</param>
        /// <param name="ListCellID"></param>
        /// <returns> true (此點在Cell區域內), false (此點非在Cell區域內) </returns>
        public bool pos_2_cellID(int row, int col, out HTuple cell_ids, out List<Point> ListCellID) // (20190706) Jeff Revised!
        {
            bool b_status_ = false;
            cell_ids = new HTuple();
            ListCellID = new List<Point>();

            int Method = 3;
            if (Method == 1 || Method == 2)
            {
                #region Method 1, 2

                HObject reg_point, reg_intersection;
                HOperatorSet.SetSystem("clip_region", "false"); // (20190627) Jeff Revised!
                HOperatorSet.GenRegionPoints(out reg_point, row, col);
                HOperatorSet.Intersection(this.cellmap_affine_, reg_point, out reg_intersection);
                HOperatorSet.SetSystem("clip_region", "true"); // (20190627) Jeff Revised!
                if (clsStaticTool.Count_HObject(reg_intersection) > 0)
                {
                    if (Method == 1)
                    {
                        HTuple area_point_, row_point_, col_point_;
                        HOperatorSet.AreaCenter(reg_intersection, out area_point_, out row_point_, out col_point_);
                        for (int index_ = 1; index_ <= area_point_.Length; index_++) // area_point_.Length: 總Cell顆數
                        {
                            if (area_point_[index_ - 1].I > 0) // point
                                HOperatorSet.TupleConcat(cell_ids, index_, out cell_ids); // (20190706) Jeff Revised!
                        }
                    }
                    else if (Method == 2) // (20190701) Jeff Revised!
                    {
                        HTuple hv_Area = new HTuple(), hv_Greater = new HTuple(), hv_Indices = new HTuple();
                        HOperatorSet.RegionFeatures(reg_intersection, "area", out hv_Area);
                        HOperatorSet.TupleGreaterElem(hv_Area, 0, out hv_Greater);
                        HOperatorSet.TupleFind(hv_Greater, 1, out hv_Indices);
                        if ((int)(new HTuple(hv_Indices.TupleNotEqual(-1))) == 1)
                            cell_ids = hv_Indices + 1;
                    }

                    if (cell_ids.Length > 0)
                    {
                        ListCellID = this.HTuple_2_ListCellID(cell_ids); // (20190706) Jeff Revised!
                        b_status_ = true;
                    }
                }

                reg_point.Dispose();
                reg_intersection.Dispose();

                #endregion
            }
            else if (Method == 3) // (20190706) Jeff Revised!
            {
                HOperatorSet.GetRegionIndex(this.cellmap_affine_, row, col, out cell_ids);
                if (cell_ids.Length > 0)
                {
                    ListCellID = this.HTuple_2_ListCellID(cell_ids); // (20190706) Jeff Revised!
                    b_status_ = true;
                }
            }

            return b_status_;
        }

        /// <summary>
        /// 指定大/小圖影像之任一點座標，計算其對應之Cell座標
        /// </summary>
        /// <param name="MoveIndex"> =-1 (大圖region), >=1 (各取像位置) </param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="cell_ids">cellmap_affine_ 之index</param>
        /// <param name="ListCellID"></param>
        /// <returns> true (此點在Cell區域內), false (此點非在Cell區域內) </returns>
        public bool pos_2_cellID_MoveIndex(int MoveIndex, double row, double col, out HTuple cell_ids, out List<Point> ListCellID) // (20190709) Jeff Revised!
        {
            if (MoveIndex == -1) // 大圖region
                return this.pos_2_cellID((int)(row + 0.5), (int)(col + 0.5), out cell_ids, out ListCellID);

            bool b_status_ = false;
            cell_ids = new HTuple();
            ListCellID = new List<Point>();

            if (MoveIndex < 1 || MoveIndex > this.array_x_count_ * this.array_y_count_)
                return false;

            try
            {
                /* Resize + 平移到大圖位置 */
                row *= this.resize_;
                col *= this.resize_;
                row += this.center_points_y_[MoveIndex - 1];
                col += this.center_points_x_[MoveIndex - 1];

                HOperatorSet.GetRegionIndex(this.cellmap_affine_, (int)(row + 0.5), (int)(col + 0.5), out cell_ids);
                if (cell_ids.Length > 0)
                {
                    ListCellID = this.HTuple_2_ListCellID(cell_ids);
                    b_status_ = true;
                }
            }
            catch
            { }

            return b_status_;
        }
        
        /// <summary>
        /// 指定大/小圖影像之多點座標，計算其對應之Cell座標
        /// </summary>
        /// <param name="MoveIndex"> =-1 (大圖region), >=1 (各取像位置) </param>
        /// <param name="ListRow"></param>
        /// <param name="ListCol"></param>
        /// <param name="ListCellID"></param>
        /// <returns></returns>
        public bool MultiPos_2_cellID_MoveIndex(int MoveIndex, List<double> ListRow, List<double> ListCol, out List<Point> ListCellID) // (20190731) Jeff Revised!
        {
            bool b_status_ = true;
            ListCellID = new List<Point>();
            for (int i = 0; i < ListRow.Count; i++)
            {
                HTuple cell_ids = new HTuple();
                List<Point> cell_1id = new List<Point>();
                if (this.pos_2_cellID_MoveIndex(MoveIndex, ListRow[i], ListCol[i], out cell_ids, out cell_1id))
                    ListCellID.Add(cell_1id[0]);
                else
                {
                    b_status_ = false;
                    break;
                }
            }

            return b_status_;
        }

        /// <summary>
        /// 指定大圖影像之任一點座標及走停拍位置(MoveIndex)，計算於該走停拍位置之座標
        /// </summary>
        /// <param name="MoveIndex"> >=1 (取像位置) </param>
        /// <param name="posBigMap"> 大圖上座標 </param>
        /// <returns></returns>
        public PointF posBigMap_2_posMoveIndex(int MoveIndex, PointF posBigMap) // (20190902) Jeff Revised!
        {
            PointF posMoveIndex = new Point(-1, -1);
            try
            {
                /* 平移 + Resize */
                posMoveIndex.Y = posBigMap.Y - (float)(this.center_points_y_[MoveIndex - 1].I);
                posMoveIndex.X = posBigMap.X - (float)(this.center_points_x_[MoveIndex - 1].I);
                posMoveIndex.Y = (float)(posMoveIndex.Y / this.resize_);
                posMoveIndex.X = (float)(posMoveIndex.X / this.resize_);
            }
            catch (Exception ex)
            { }

            return posMoveIndex;
        }

        /// <summary>
        /// 指定大圖影像之任一點座標，計算包含該點之所有走停拍位置(MoveIndex)
        /// </summary>
        /// <param name="posBigMap"></param>
        /// <returns></returns>
        public List<int> posBigMap_2_ListMoveIndex(PointF posBigMap) // (20190902) Jeff Revised!
        {
            HTuple capture_Index = new HTuple();
            HOperatorSet.GetRegionIndex(this.capture_map_, (int)(posBigMap.Y + 0.5), (int)(posBigMap.X + 0.5), out capture_Index);
            return clsStaticTool.HTuple_2_ListInt(capture_Index);
        }

        /// <summary>
        /// 指定任一顆Cell中心座標(For 大圖)，計算包含之所有走停拍位置(MoveIndex)及對應該走停拍位置之Cell region
        /// </summary>
        /// <param name="posBigMapCellCenter"></param>
        /// <param name="ListMoveIndex"></param>
        /// <param name="ListCellReg_MoveIndex"></param>
        /// <returns></returns>
        public bool posBigMapCellCenter_2_MoveIndex(PointF posBigMapCellCenter, out List<int> ListMoveIndex, out List<HObject> ListCellReg_MoveIndex) // (20200429) Jeff Revised!
        {
            ListMoveIndex = new List<int>();
            ListCellReg_MoveIndex = new List<HObject>();
            if (CellReg_MoveIndex_FS.Count != this.array_x_count_ * this.array_y_count_)
                return false;

            bool b_status_ = false;
            try
            {
                List<int> listMoveIndex = this.posBigMap_2_ListMoveIndex(posBigMapCellCenter);
                /* 判斷各走停拍位置是否包含此點所在Cell */
                foreach (int moveIndex in listMoveIndex)
                {
                    // 計算於該走停拍位置之座標
                    PointF posMoveIndex = this.posBigMap_2_posMoveIndex(moveIndex, posBigMapCellCenter);

                    // 計算包含此點所在Cell
                    HTuple cell_Index = new HTuple();
                    HOperatorSet.GetRegionIndex(CellReg_MoveIndex_FS[moveIndex - 1], (int)(posMoveIndex.Y + 0.5), (int)(posMoveIndex.X + 0.5), out cell_Index);
                    if (cell_Index.Length > 0)
                    {
                        ListMoveIndex.Add(moveIndex);
                        ListCellReg_MoveIndex.Add(CellReg_MoveIndex_FS[moveIndex - 1].SelectObj((int)(cell_Index[0])));
                    }
                }
                b_status_ = true;
            }
            catch
            { }
            
            return b_status_;
        }

        /// <summary>
        /// 瑕疵Cell座標 轉 int (cellmap_affine_ 之index)
        /// </summary>
        /// <param name="cellID"></param>
        /// <returns></returns>
        public int cellID_2_int(Point cellID) // (20181116) Jeff Revised!
        {
            return (cellID.Y - 1) * this.cell_col_count_ + cellID.X;
        }

        /// <summary>
        /// List的瑕疵Cell座標 轉 HTuple (cellmap_affine_ 之index)
        /// </summary>
        /// <param name="ListCellID"></param>
        /// <returns></returns>
        public HTuple ListCellID_2_HTuple(List<Point> ListCellID) // (20190702) Jeff Revised!
        {
            HTuple result = new HTuple();
            foreach (Point pt in ListCellID)
                HOperatorSet.TupleConcat(result, this.cellID_2_int(pt), out result);
            return result;
        }

        /// <summary>
        /// 指定座標組，計算其Cell region
        /// </summary>
        /// <param name="ListCellID"></param>
        /// <param name="cellmap_affine"></param>
        /// <returns></returns>
        public HObject Compute_CellReg_ListCellID(List<Point> ListCellID, HObject cellmap_affine = null) // (20200429) Jeff Revised!
        {
            HObject result;
            HOperatorSet.GenEmptyObj(out result);
            if (ListCellID.Count <= 0)
                return result;

            try
            {
                if (cellmap_affine == null)
                    cellmap_affine = this.cellmap_affine_; // (20200429) Jeff Revised!
                HTuple index = this.ListCellID_2_HTuple(ListCellID);
                result.Dispose();
                HOperatorSet.SelectObj(cellmap_affine, out result, index);
            }
            catch (Exception ex)
            { }

            return result;
        }

        /// <summary>
        /// 瑕疵Cell座標 轉 MoveIndex (capture_map_ 之index)
        /// </summary>
        /// <param name="cellID"></param>
        /// <returns></returns>
        public List<int> cellID_2_MoveIndex(Point cellID) // (20190801) Jeff Revised!
        {
            // 該座標轉其對應 cellmap_affine_ 之index
            int index_cellmap_affine_ = this.cellID_2_int(cellID);
            return this.IndexCellmapAffine_2_MoveIndex(index_cellmap_affine_);
        }

        /// <summary>
        /// 瑕疵Cell index 轉 MoveIndex (capture_map_ 之index)
        /// </summary>
        /// <param name="index_cellmap_affine_">cellmap_affine_ 之index</param>
        /// <returns></returns>
        public List<int> IndexCellmapAffine_2_MoveIndex(int index_cellmap_affine_) // (20190801) Jeff Revised!
        {
            List<int> List_MoveIndex = new List<int>();

            HOperatorSet.SetSystem("clip_region", "false");
            try
            {
                HObject CellReg;
                HOperatorSet.SelectObj(this.cellmap_affine_, out CellReg, index_cellmap_affine_);

                HObject CellReg_Intersection;
                HOperatorSet.Intersection(this.capture_map_, CellReg, out CellReg_Intersection);

                HTuple hv_Area1 = new HTuple(), hv_Area2 = new HTuple(), hv_Equal = new HTuple(), hv_Indices = new HTuple();
                HOperatorSet.RegionFeatures(CellReg, "area", out hv_Area1); // hv_Area1 只有一個元素
                HOperatorSet.RegionFeatures(CellReg_Intersection, "area", out hv_Area2);
                HOperatorSet.TupleEqualElem(hv_Area1, hv_Area2, out hv_Equal);
                HOperatorSet.TupleFind(hv_Equal, 1, out hv_Indices);
                if ((int)(new HTuple(hv_Indices.TupleNotEqual(-1))) == 1)
                {
                    hv_Indices = hv_Indices + 1;
                    List_MoveIndex = clsStaticTool.HTuple_2_ListInt(hv_Indices);
                }

                CellReg.Dispose();
                CellReg_Intersection.Dispose();
            }
            catch (Exception ex)
            { }
            finally
            {
                HOperatorSet.SetSystem("clip_region", "true");
            }

            return List_MoveIndex;
        }

        /// <summary>
        /// List的瑕疵Cell座標 轉 各自對應之最小MoveIndex (capture_map_ 之index)
        /// </summary>
        /// <param name="ListCellID"></param>
        /// <param name="List_MoveIndex"></param>
        /// <returns></returns>
        public bool ListCellID_2_ListMoveIndex(List<Point> ListCellID, out List<int> List_MoveIndex) // (20190801) Jeff Revised!
        {
            bool b_status_ = true;
            List_MoveIndex = new List<int>();

            foreach (Point pt in ListCellID)
            {
                List<int> MoveIndex_1 = this.cellID_2_MoveIndex(pt);
                if (MoveIndex_1.Count <= 0)
                {
                    b_status_ = false;
                    break;
                }
                else
                    List_MoveIndex.Add(MoveIndex_1[0]);
            }
            
            return b_status_;
        }

        /// <summary>
        /// ConcatCellIndex 轉 各自對應之最小MoveIndex (capture_map_ 之index)
        /// </summary>
        /// <param name="ConcatCellIndex"></param>
        /// <param name="List_MoveIndex"></param>
        /// <returns></returns>
        public bool ConcatCellIndex_2_ListMoveIndex(HTuple ConcatCellIndex, out List<int> List_MoveIndex) // (20190801) Jeff Revised!
        {
            bool b_status_ = true;
            List_MoveIndex = new List<int>();

            for (int i = 0; i < ConcatCellIndex.Length; i++)
            {
                List<int> MoveIndex_1 = this.IndexCellmapAffine_2_MoveIndex(ConcatCellIndex[i].I);
                if (MoveIndex_1.Count <= 0)
                {
                    List_MoveIndex.Clear();
                    b_status_ = false;
                    break;
                }
                else
                    List_MoveIndex.Add(MoveIndex_1[0]);
            }

            return b_status_;
        }

        /// <summary>
        /// ConcatCellIndex 轉 各自對應之最小MoveIndex (capture_map_ 之index)
        /// </summary>
        /// <param name="ConcatCellIndex"></param>
        /// <param name="Concat_MoveIndex"></param>
        /// <returns></returns>
        public bool ConcatCellIndex_2_ConcatMoveIndex(HTuple ConcatCellIndex, out HTuple Concat_MoveIndex) // (20190801) Jeff Revised!
        {
            bool b_status_ = true;
            Concat_MoveIndex = new HTuple();

            for (int i = 0; i < ConcatCellIndex.Length; i++)
            {
                List<int> MoveIndex_1 = this.IndexCellmapAffine_2_MoveIndex(ConcatCellIndex[i].I);
                if (MoveIndex_1.Count <= 0)
                {
                    Concat_MoveIndex = new HTuple();
                    b_status_ = false;
                    break;
                }
                else
                    HOperatorSet.TupleConcat(Concat_MoveIndex, MoveIndex_1[0], out Concat_MoveIndex);
            }

            return b_status_;
        }

        /// <summary>
        /// int (cellmap_affine_ 之index) 轉 瑕疵Cell座標
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Point int_2_cellID(int i) // (20190701) Jeff Revised!
        {
            int id_x_, id_y_;
            if (i % this.cell_col_count_ == 0)
            {
                id_x_ = this.cell_col_count_;
                id_y_ = i / this.cell_col_count_;
            }
            else
            {
                id_x_ = i % this.cell_col_count_;
                id_y_ = i / this.cell_col_count_ + 1;
            }
            return new Point(id_x_, id_y_);
        }

        /// <summary>
        /// cell_ids (cellmap_affine_ 之index) 轉 瑕疵Cell座標
        /// </summary>
        /// <param name="cell_ids">cellmap_affine_ 之index</param>
        /// <returns></returns>
        public List<Point> HTuple_2_ListCellID(HTuple cell_ids) // (20190706) Jeff Revised!
        {
            List<Point> ListCellID = new List<Point>();

            for (int i = 0; i < cell_ids.Length; i++)
                ListCellID.Add(this.int_2_cellID(cell_ids[i]));

            return ListCellID;
        }

        /// <summary>
        /// 計算某一取像位置之完整 Cell regions (平移到原點)
        /// </summary>
        /// <param name="MoveIndex"> =-1 (大圖region), >=1 (各取像位置) </param>
        /// <param name="CellReg_MoveIndex"></param>
        /// <returns></returns>
        public bool Compute_CellReg_MoveIndex(int MoveIndex, out HObject CellReg_MoveIndex) // (20190708) Jeff Revised!
        {
            if (MoveIndex == -1) // 大圖region
            {
                CellReg_MoveIndex = this.cellmap_affine_.Clone();
                return true;
            }

            HOperatorSet.GenEmptyRegion(out CellReg_MoveIndex);
            if (MoveIndex < 1 || MoveIndex > this.array_x_count_ * this.array_y_count_)
                return false;

            bool b_status_ = false;
            HOperatorSet.SetSystem("clip_region", "false");
            try
            {
                HObject capture_FOV, CellReg_capture, CellReg_select, CellReg_move;
                HOperatorSet.SelectObj(this.capture_map_, out capture_FOV, MoveIndex);
                HOperatorSet.Intersection(this.cellmap_affine_, capture_FOV, out CellReg_capture);
                capture_FOV.Dispose();

                // 消除不完整Cell
                HTuple hv_Area_allCells2 = new HTuple(), hv_Equal = new HTuple(), hv_Indices = new HTuple();
                HOperatorSet.RegionFeatures(CellReg_capture, "area", out hv_Area_allCells2);
                HOperatorSet.TupleEqualElem(this.area_cellmap_affine_, hv_Area_allCells2, out hv_Equal);
                HOperatorSet.TupleFind(hv_Equal, 1, out hv_Indices);
                if ((int)(new HTuple(hv_Indices.TupleNotEqual(-1))) == 1)
                {
                    hv_Indices = hv_Indices + 1;
                    HOperatorSet.SelectObj(this.cellmap_affine_, out CellReg_select, hv_Indices);
                }
                else
                    HOperatorSet.GenEmptyRegion(out CellReg_select);
                CellReg_capture.Dispose();

                // 平移到原點
                HOperatorSet.MoveRegion(CellReg_select, out CellReg_move, -this.center_points_y_[MoveIndex - 1], -this.center_points_x_[MoveIndex - 1]);
                CellReg_select.Dispose();

                // Resize回原始大小
                CellReg_MoveIndex.Dispose();
                HOperatorSet.ZoomRegion(CellReg_move, out CellReg_MoveIndex, 1 / this.resize_, 1 / this.resize_);
                CellReg_move.Dispose();

                b_status_ = true;
            }
            catch
            { }
            finally
            {
                HOperatorSet.SetSystem("clip_region", "true");
            }
            
            return b_status_;
        }

        /// <summary>
        /// 計算某一取像位置之特定 Cell regions (平移到原點)
        /// </summary>
        /// <param name="MoveIndex"> =-1 (大圖region), >=1 (各取像位置) </param>
        /// <param name="ConcatCellIndex"></param>
        /// <param name="CellReg_MoveIndex"></param>
        /// <returns></returns>
        public bool compute_CellReg_MoveIndex_From_ConcatCellIndex(int MoveIndex, HTuple ConcatCellIndex, out HObject CellReg_MoveIndex) // (20190801) Jeff Revised!
        {
            if (MoveIndex == -1) // 大圖region
            {
                HOperatorSet.SelectObj(this.cellmap_affine_, out CellReg_MoveIndex, ConcatCellIndex);
                return true;
            }

            HOperatorSet.GenEmptyRegion(out CellReg_MoveIndex);
            if (MoveIndex < 1 || MoveIndex > this.array_x_count_ * this.array_y_count_)
                return false;

            bool b_status_ = false;
            HOperatorSet.SetSystem("clip_region", "false");
            try
            {
                HObject CellReg_select, CellReg_move;
                HOperatorSet.SelectObj(this.cellmap_affine_, out CellReg_select, ConcatCellIndex);

                // 平移到原點
                HOperatorSet.MoveRegion(CellReg_select, out CellReg_move, -this.center_points_y_[MoveIndex - 1], -this.center_points_x_[MoveIndex - 1]);
                CellReg_select.Dispose();

                // Resize回原始大小
                HOperatorSet.ZoomRegion(CellReg_move, out CellReg_MoveIndex, 1 / this.resize_, 1 / this.resize_);
                CellReg_move.Dispose();

                b_status_ = true;
            }
            catch
            { }
            finally
            {
                HOperatorSet.SetSystem("clip_region", "true");
            }

            return b_status_;
        }

        /// <summary>
        /// 將大圖瑕疵Cell區域 轉到 某一取像位置 (平移到原點)
        /// </summary>
        /// <param name="MoveIndex"> =-1 (大圖region), >=1 (各取像位置) </param>
        /// <param name="All_inter_defect_Cell"></param>
        /// <param name="DefectReg_MoveIndex"></param>
        /// <returns></returns>
        public bool Compute_DefectReg_MoveIndex(int MoveIndex, HObject All_inter_defect_Cell, out HObject DefectReg_MoveIndex) // (20190708) Jeff Revised!
        {
            if (MoveIndex == -1) // 大圖region
            {
                DefectReg_MoveIndex = All_inter_defect_Cell.Clone();
                return true;
            }

            HOperatorSet.GenEmptyRegion(out DefectReg_MoveIndex);
            if (MoveIndex < 1 || MoveIndex > this.array_x_count_ * this.array_y_count_)
                return false;

            bool b_status_ = false;
            HOperatorSet.SetSystem("clip_region", "false");
            try
            {
                HOperatorSet.Intersection(this.cellmap_affine_, All_inter_defect_Cell, out All_inter_defect_Cell);

                HObject capture_FOV, CellReg_capture, CellReg_select, CellReg_move;
                HOperatorSet.SelectObj(this.capture_map_, out capture_FOV, MoveIndex);
                HOperatorSet.Intersection(All_inter_defect_Cell, capture_FOV, out CellReg_capture);
                capture_FOV.Dispose();

                // 消除不完整Cell
                HTuple hv_Area_allCells1 = new HTuple(), hv_Area_allCells2 = new HTuple(), hv_Equal = new HTuple(), hv_Indices = new HTuple();
                HOperatorSet.RegionFeatures(All_inter_defect_Cell, "area", out hv_Area_allCells1);
                HOperatorSet.RegionFeatures(CellReg_capture, "area", out hv_Area_allCells2);
                HOperatorSet.TupleEqualElem(hv_Area_allCells1, hv_Area_allCells2, out hv_Equal);
                HOperatorSet.TupleFind(hv_Equal, 1, out hv_Indices);
                if ((int)(new HTuple(hv_Indices.TupleNotEqual(-1))) == 1)
                {
                    hv_Indices = hv_Indices + 1;
                    HOperatorSet.SelectObj(All_inter_defect_Cell, out CellReg_select, hv_Indices);
                }
                else
                    HOperatorSet.GenEmptyRegion(out CellReg_select);
                CellReg_capture.Dispose();

                // 平移到原點
                HOperatorSet.MoveRegion(CellReg_select, out CellReg_move, -this.center_points_y_[MoveIndex - 1], -this.center_points_x_[MoveIndex - 1]);
                CellReg_select.Dispose();

                // Resize回原始大小
                HOperatorSet.ZoomRegion(CellReg_move, out DefectReg_MoveIndex, 1 / this.resize_, 1 / this.resize_);
                CellReg_move.Dispose();

                b_status_ = true;
            }
            catch
            { }
            finally
            {
                HOperatorSet.SetSystem("clip_region", "true");
            }

            return b_status_;
        }

        #endregion
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="b_Parallel">是否執行平行運算</param>
        /// <returns></returns>
        public bool create_cell_map2(bool b_Parallel = false) // (20190624) Jeff Revised!
        {
            bool b_status_ = false;

            // Resize
            origin_mark_pos_x_resize = origin_mark_pos_x_[0].D * resize_;
            origin_mark_pos_y_resize = origin_mark_pos_y_[0].D * resize_;
            HTuple dist_region_rdx_HTuple_resize = dist_region_rdx_HTuple * resize_; // (20190306) Jeff Revised!
            HTuple dist_region_rdy_HTuple_resize = dist_region_rdy_HTuple * resize_; // (20190306) Jeff Revised!
            
            HTuple affine_mark_1_pos_x_, affine_mark_1_pos_y_;
            HTuple affine_fc_x_, affine_fc_y_;

            HOperatorSet.HomMat2dIdentity(out hommat2d_affine_);
            HOperatorSet.HomMat2dRotate(hommat2d_affine_, affine_degree_.TupleRad(), 0, 0, out hommat2d_affine_);
            // 扭正後之定位點位置 (For Mark AffineImage 1)
            /* Method1:  */
            HOperatorSet.AffineTransPixel(hommat2d_affine_, origin_mark_pos_y_resize, origin_mark_pos_x_resize, out affine_mark_1_pos_y_, out affine_mark_1_pos_x_);
            /* Method2: 扭正前後之第一個定位點位置不變 (i.e. 相對第一個定位點位置做扭正) (20181226) Jeff Revised! */
            //affine_mark_1_pos_y_ = origin_mark_pos_y_resize;
            //affine_mark_1_pos_x_ = origin_mark_pos_x_resize;

            // 如果實際運動(機械)座標和影像座標方向不同，必須做此轉換 (i.e. 以第一張檢測位置當成是原點)
            // Note: 全自動機台之ME1軸方向與影像X軸方向相反
            HTuple Mark1_rpos_x_ = null, Start_rpos_x_ = null;
            if (Convert.ToBoolean(X_dir_inv)) // 運動與影像座標之X方向反向 (20180901) Jeff Revised!
            {
                Mark1_rpos_x_ = start_rpos_x_ - mark1_rpos_x_;
                Start_rpos_x_ = 0.0;
            }
            else
            {
                Mark1_rpos_x_ = mark1_rpos_x_;
                Start_rpos_x_ = start_rpos_x_;
            }
            // Note: 全自動機台之MB1/MB2軸方向與影像Y軸方向相同
            HTuple Mark1_rpos_y_ = null, Start_rpos_y_ = null;
            if (Convert.ToBoolean(Y_dir_inv)) // 運動與影像座標之Y方向反向 (20180901) Jeff Revised!
            {
                Mark1_rpos_y_ = start_rpos_y_ - mark1_rpos_y_;
                Start_rpos_y_ = 0.0;
            }
            else
            {
                Mark1_rpos_y_ = mark1_rpos_y_;
                Start_rpos_y_ = start_rpos_y_;
            }

            // 單位轉換: mm → pixel
            double mm2pixel = 1000 / pixel_resolution_;
            HTuple Mark1_ppos_x_ = Mark1_rpos_x_ * mm2pixel;
            HTuple mark1_ppos_y_ = Mark1_rpos_y_ * mm2pixel; // (20180901) Jeff Revised!
            HTuple Start_ppos_x_ = Start_rpos_x_ * mm2pixel;
            HTuple start_ppos_y_ = Start_rpos_y_ * mm2pixel; // (20180901) Jeff Revised!
            HTuple dist_region_pdx = dist_region_rdx_HTuple_resize * mm2pixel; // (20190306) Jeff Revised!
            HTuple dist_region_pdy = dist_region_rdy_HTuple_resize * mm2pixel; // (20190306) Jeff Revised!
            HTuple size_sample_px = size_sample_rx * mm2pixel;
            HTuple size_sample_py = size_sample_ry * mm2pixel;

            // 扭正後之第一顆cell中心位置 (For 第一張檢測影像扭正)
            affine_fc_x_ = affine_mark_1_pos_x_ - (Start_ppos_x_ - Mark1_ppos_x_) + Mark_pdx_;
            affine_fc_y_ = affine_mark_1_pos_y_ - (start_ppos_y_ - mark1_ppos_y_) + Mark_pdy_;

            // 未扭正之第一顆cell中心位置 (For 第一張檢測影像)
            HOperatorSet.HomMat2dIdentity(out hommat2d_origin_);
            HOperatorSet.HomMat2dRotate(hommat2d_origin_, new HTuple(-affine_degree_).TupleRad(), 0, 0, out hommat2d_origin_);
            HOperatorSet.AffineTransPixel(hommat2d_origin_, affine_fc_y_, affine_fc_x_, out origin_fc_y_, out origin_fc_x_);

            #region 開始建立cell全域地圖 cellmap_sortregion_

            HOperatorSet.SetSystem("height", size_sample_py.TupleInt());
            HOperatorSet.SetSystem("width", size_sample_px.TupleInt());

            HObject cellmap_region_, rect_group;
            //HOperatorSet.GenEmptyRegion(out cellmap_region_);
            //HOperatorSet.GenEmptyRegion(out rect_group);
            HOperatorSet.GenEmptyObj(out cellmap_region_); // (20190624) Jeff Revised!
            HOperatorSet.GenEmptyObj(out rect_group); // (20190624) Jeff Revised!
            
            #region y方向

            for (int y_reg = 0; y_reg < num_region_y_; y_reg++)
            {
                if (!b_Parallel)
                {
                    /* 法1 */
                    for (int y = 0; y < cell_y_count_HTuple[y_reg]; y++)
                    {
                        HObject cell_rect_;
                        HOperatorSet.GenRectangle2(out cell_rect_, affine_fc_y_ + cell_pdy_ * y + dist_region_pdy[y_reg], affine_fc_x_, 0, cell_pwidth_ / 2, cell_pheight_ / 2);
                        //HOperatorSet.Union2(rect_group, cell_rect_, out rect_group);
                        HOperatorSet.ConcatObj(rect_group, cell_rect_, out rect_group); // (20190306) Jeff Revised!
                        cell_rect_.Dispose();
                        // (20190401) Jeff Revised!
                        //GC.Collect();
                        //GC.WaitForPendingFinalizers();
                    }
                }
                else // Note: 如果Cell顆數不多時，平行運算反而比較慢!!!
                {
                    /* 法2: 平行運算 (Parallel Programming) */
                    bool b_error_Parallel = false; // 平行運算中，是否遇到例外狀況
                    Parallel.For(0, cell_y_count_HTuple[y_reg], (y, loopState) =>
                    {
                        try
                        {
                            HObject cell_rect_;
                            HOperatorSet.GenRectangle2(out cell_rect_, affine_fc_y_ + cell_pdy_ * y + dist_region_pdy[y_reg], affine_fc_x_, 0, cell_pwidth_ / 2, cell_pheight_ / 2);
                            // 法2-1: 同時存取rect_group時，會有問題!
                            //HOperatorSet.ConcatObj(rect_group, cell_rect_, out rect_group); // (20190306) Jeff Revised!
                            // 法2-2: 鎖住資源
                            lock (this)
                                HOperatorSet.ConcatObj(rect_group, cell_rect_, out rect_group);
                            // 法2-3: 鎖住資源 (20191214) Jeff Revised!
                            //lock (lock_CreateCellMap)
                            //    HOperatorSet.ConcatObj(rect_group, cell_rect_, out rect_group);

                            cell_rect_.Dispose();
                        }
                        catch
                        {
                            // 停止並退出Parallel.For
                            loopState.Stop();
                            b_error_Parallel = true;
                            return;
                        }
                    });

                    if (b_error_Parallel) // 判斷平行運算中，是否遇到例外狀況
                    {
                        #region Release memories
                        cellmap_region_.Dispose();
                        rect_group.Dispose();
                        #endregion
                        return b_status_;
                    }
                }
            }

            #endregion

            #region x方向

            for (int x_reg = 0; x_reg < num_region_x_; x_reg++)
            {
                if (!b_Parallel)
                {
                    /* 法1 */
                    for (int x = 0; x < cell_x_count_HTuple[x_reg]; x++)
                    {
                        HObject moved;
                        HOperatorSet.MoveRegion(rect_group, out moved, 0, cell_pdx_ * x + dist_region_pdx[x_reg]);
                        //HOperatorSet.Union2(cellmap_region_, moved, out cellmap_region_);
                        HOperatorSet.ConcatObj(cellmap_region_, moved, out cellmap_region_); // (20190306) Jeff Revised!
                        moved.Dispose();
                    }
                }
                else // Note: 如果Cell顆數不多時，平行運算反而比較慢!!!
                {
                    /* 法2: 平行運算 (Parallel Programming) */
                    bool b_error_Parallel = false; // 平行運算中，是否遇到例外狀況
                    Parallel.For(0, cell_x_count_HTuple[x_reg], (x, loopState) =>
                    {
                        try
                        {
                            HObject moved;
                            HOperatorSet.MoveRegion(rect_group, out moved, 0, cell_pdx_ * x + dist_region_pdx[x_reg]);
                            // 法2-1: 同時存取rect_group時，會有問題!
                            //HOperatorSet.ConcatObj(cellmap_region_, moved, out cellmap_region_); // (20190306) Jeff Revised!
                            // 法2-2: 鎖住資源
                            lock (this)
                                HOperatorSet.ConcatObj(cellmap_region_, moved, out cellmap_region_);
                            // 法2-3: 鎖住資源 (20191214) Jeff Revised!
                            //lock (lock_CreateCellMap)
                            //    HOperatorSet.ConcatObj(cellmap_region_, moved, out cellmap_region_);

                            moved.Dispose();
                        }
                        catch
                        {
                            // 停止並退出Parallel.For
                            loopState.Stop();
                            b_error_Parallel = true;
                            return;
                        }
                    });

                    if (b_error_Parallel) // 判斷平行運算中，是否遇到例外狀況
                    {
                        #region Release memories
                        cellmap_region_.Dispose();
                        rect_group.Dispose();
                        #endregion
                        return b_status_;
                    }
                }
            }

            #endregion

            //HOperatorSet.Connection(cellmap_region_, out cellmap_region_); // (20190624) Jeff Revised!
            HOperatorSet.SortRegion(cellmap_region_, out cellmap_sortregion_, "first_point", "true", "row");

            // Release memories (20190624) Jeff Revised!
            cellmap_region_.Dispose();
            rect_group.Dispose();

            #endregion

            // 建立 Marks' center 在大拼接圖上(已扭正)之位置 (20181226) Jeff Revised!
            //HOperatorSet.GenCircle(out MarkCenter_BigMap_affine, affine_fc_y_ - Mark_pdy_, affine_fc_x_ - Mark_pdx_, 10);
            HTuple pos_mark1_pdy, pos_mark1_pdx;
            HOperatorSet.AffineTransPixel(hommat2d_origin_, affine_fc_y_ - Mark_pdy_, affine_fc_x_ - Mark_pdx_, out pos_mark1_pdy, out pos_mark1_pdx);
            HOperatorSet.GenCircle(out MarkCenter_BigMap_orig, pos_mark1_pdy, pos_mark1_pdx, 10);
            //HOperatorSet.GenCircle(out MarkCenter_BigMap_orig, origin_fc_y_ - Mark_pdy_, origin_fc_x_ - Mark_pdx_, 10);

            b_status_ = true;

            return b_status_;
        }

        /// <summary>
        /// 建立 Cell Map (不做視覺定位)
        /// </summary>
        /// <param name="b_Parallel">是否執行平行運算</param>
        /// <returns></returns>
        public bool create_cell_map2_NoPos(bool b_Parallel = false) // (20191216) MIL Jeff Revised!
        {
            bool b_status_ = false;

            if (LocateMethodRecipe.VisualPosEnable)
                return false;

            // Resize
            HTuple dist_region_rdx_HTuple_resize = dist_region_rdx_HTuple * resize_; // (20190306) Jeff Revised!
            HTuple dist_region_rdy_HTuple_resize = dist_region_rdy_HTuple * resize_; // (20190306) Jeff Revised!
            
            // 單位轉換: mm → pixel
            double mm2pixel = 1000 / pixel_resolution_;
            HTuple dist_region_pdx = dist_region_rdx_HTuple_resize * mm2pixel; // (20190306) Jeff Revised!
            HTuple dist_region_pdy = dist_region_rdy_HTuple_resize * mm2pixel; // (20190306) Jeff Revised!
            HTuple size_sample_px = size_sample_rx * mm2pixel;
            HTuple size_sample_py = size_sample_ry * mm2pixel;

            // 扭正後之第一顆cell中心位置 (For 第一張檢測影像扭正)
            HTuple affine_fc_x_, affine_fc_y_;
            affine_fc_x_ = Mark_pdx_;
            affine_fc_y_ = Mark_pdy_;

            // 未扭正之第一顆cell中心位置 (For 第一張檢測影像)
            affine_degree_ = 0.0;
            HOperatorSet.HomMat2dIdentity(out hommat2d_origin_);
            HOperatorSet.HomMat2dRotate(hommat2d_origin_, new HTuple(-affine_degree_).TupleRad(), 0, 0, out hommat2d_origin_);
            HOperatorSet.AffineTransPixel(hommat2d_origin_, affine_fc_y_, affine_fc_x_, out origin_fc_y_, out origin_fc_x_);

            #region 開始建立cell全域地圖 cellmap_sortregion_

            HOperatorSet.SetSystem("height", size_sample_py.TupleInt());
            HOperatorSet.SetSystem("width", size_sample_px.TupleInt());

            HObject cellmap_region_, rect_group;
            HOperatorSet.GenEmptyObj(out cellmap_region_); // (20190624) Jeff Revised!
            HOperatorSet.GenEmptyObj(out rect_group); // (20190624) Jeff Revised!
            
            #region y方向

            for (int y_reg = 0; y_reg < num_region_y_; y_reg++)
            {
                if (!b_Parallel)
                {
                    /* 法1 */
                    for (int y = 0; y < cell_y_count_HTuple[y_reg]; y++)
                    {
                        HObject cell_rect_;
                        HOperatorSet.GenRectangle2(out cell_rect_, affine_fc_y_ + cell_pdy_ * y + dist_region_pdy[y_reg], affine_fc_x_, 0, cell_pwidth_ / 2, cell_pheight_ / 2);
                        //HOperatorSet.Union2(rect_group, cell_rect_, out rect_group);
                        HOperatorSet.ConcatObj(rect_group, cell_rect_, out rect_group); // (20190306) Jeff Revised!
                        cell_rect_.Dispose();
                        // (20190401) Jeff Revised!
                        //GC.Collect();
                        //GC.WaitForPendingFinalizers();
                    }
                }
                else // Note: 如果Cell顆數不多時，平行運算反而比較慢!!!
                {
                    /* 法2: 平行運算 (Parallel Programming) */
                    bool b_error_Parallel = false; // 平行運算中，是否遇到例外狀況
                    Parallel.For(0, cell_y_count_HTuple[y_reg], (y, loopState) =>
                    {
                        try
                        {
                            HObject cell_rect_;
                            HOperatorSet.GenRectangle2(out cell_rect_, affine_fc_y_ + cell_pdy_ * y + dist_region_pdy[y_reg], affine_fc_x_, 0, cell_pwidth_ / 2, cell_pheight_ / 2);
                            // 法2-1: 同時存取rect_group時，會有問題!
                            //HOperatorSet.ConcatObj(rect_group, cell_rect_, out rect_group); // (20190306) Jeff Revised!
                            // 法2-2: 鎖住資源
                            lock (this)
                                HOperatorSet.ConcatObj(rect_group, cell_rect_, out rect_group);
                            // 法2-3: 鎖住資源 (20191214) Jeff Revised!
                            //lock (lock_CreateCellMap)
                            //    HOperatorSet.ConcatObj(rect_group, cell_rect_, out rect_group);

                            cell_rect_.Dispose();
                        }
                        catch
                        {
                            // 停止並退出Parallel.For
                            loopState.Stop();
                            b_error_Parallel = true;
                            return;
                        }
                    });

                    if (b_error_Parallel) // 判斷平行運算中，是否遇到例外狀況
                    {
                        #region Release memories
                        cellmap_region_.Dispose();
                        rect_group.Dispose();
                        #endregion
                        return b_status_;
                    }
                }
            }

            #endregion

            #region x方向

            for (int x_reg = 0; x_reg < num_region_x_; x_reg++)
            {
                if (!b_Parallel)
                {
                    /* 法1 */
                    for (int x = 0; x < cell_x_count_HTuple[x_reg]; x++)
                    {
                        HObject moved;
                        HOperatorSet.MoveRegion(rect_group, out moved, 0, cell_pdx_ * x + dist_region_pdx[x_reg]);
                        //HOperatorSet.Union2(cellmap_region_, moved, out cellmap_region_);
                        HOperatorSet.ConcatObj(cellmap_region_, moved, out cellmap_region_); // (20190306) Jeff Revised!
                        moved.Dispose();
                    }
                }
                else // Note: 如果Cell顆數不多時，平行運算反而比較慢!!!
                {
                    /* 法2: 平行運算 (Parallel Programming) */
                    bool b_error_Parallel = false; // 平行運算中，是否遇到例外狀況
                    Parallel.For(0, cell_x_count_HTuple[x_reg], (x, loopState) =>
                    {
                        try
                        {
                            HObject moved;
                            HOperatorSet.MoveRegion(rect_group, out moved, 0, cell_pdx_ * x + dist_region_pdx[x_reg]);
                            // 法2-1: 同時存取rect_group時，會有問題!
                            //HOperatorSet.ConcatObj(cellmap_region_, moved, out cellmap_region_); // (20190306) Jeff Revised!
                            // 法2-2: 鎖住資源
                            lock (this)
                                HOperatorSet.ConcatObj(cellmap_region_, moved, out cellmap_region_);
                            // 法2-3: 鎖住資源 (20191214) Jeff Revised!
                            //lock (lock_CreateCellMap)
                            //    HOperatorSet.ConcatObj(cellmap_region_, moved, out cellmap_region_);

                            moved.Dispose();
                        }
                        catch
                        {
                            // 停止並退出Parallel.For
                            loopState.Stop();
                            b_error_Parallel = true;
                            return;
                        }
                    });

                    if (b_error_Parallel) // 判斷平行運算中，是否遇到例外狀況
                    {
                        #region Release memories
                        cellmap_region_.Dispose();
                        rect_group.Dispose();
                        #endregion
                        return b_status_;
                    }
                }
            }

            #endregion

            HOperatorSet.SortRegion(cellmap_region_, out cellmap_sortregion_, "first_point", "true", "row");

            // Release memories (20190624) Jeff Revised!
            cellmap_region_.Dispose();
            rect_group.Dispose();

            #endregion

            // 建立 Marks' center 在大拼接圖上(已扭正)之位置
            Extension.HObjectMedthods.ReleaseHObject(ref MarkCenter_BigMap_orig);
            HOperatorSet.GenEmptyRegion(out MarkCenter_BigMap_orig);

            b_status_ = true;

            return b_status_;
        }
        
        /// <summary>
        /// 所有觸發影像拼接
        /// </summary>
        /// <param name="all_capture_images_"></param>
        /// <returns></returns>
        public HObject tile_images2(List<HObject> all_capture_images_) // (20190604) Jeff Revised!
        {
            HObject all_concat_images_ = null;
            HObject tile_image_ = null;

            try
            {
                // 將 all_capture_images_ 轉換成 all_concat_images_
                HOperatorSet.GenEmptyObj(out all_concat_images_); // (20190604) Jeff Revised!
                foreach (var i in all_capture_images_)
                {
                    // Resize
                    HObject i_resize;
                    HOperatorSet.ZoomImageFactor(i, out i_resize, resize_, resize_, "bilinear");

                    HOperatorSet.ConcatObj(all_concat_images_, i_resize, out all_concat_images_); // (20190604) Jeff Revised!
                }

                HTuple number_;
                HOperatorSet.CountObj(all_concat_images_, out number_);
                
                double array_x_pix_ = array_x_rpitch_ * 1000 / pixel_resolution_;
                double array_y_pix_ = array_y_rpitch_ * 1000 / pixel_resolution_;
                if ((array_x_pix_ >= frame_pwidth_) && (array_y_pix_ >= frame_pheight_)) // 兩相鄰影像沒有Overlap
                {
                    HOperatorSet.TileImagesOffset(all_concat_images_, out tile_image_, center_points_y_.TupleSelectRange(0, number_ - 1), center_points_x_.TupleSelectRange(0, number_ - 1),
                        tile_rect_.TupleSelectRange(0, number_ - 1), tile_rect_.TupleSelectRange(0, number_ - 1), tile_rect_.TupleSelectRange(0, number_ - 1), tile_rect_.TupleSelectRange(0, number_ - 1),
                        frame_pwidth_ * array_x_count_, frame_pheight_ * array_y_count_);
                }
                else // 兩相鄰影像有Overlap
                {
                    HOperatorSet.TileImagesOffset(all_concat_images_, out tile_image_, center_points_y_.TupleSelectRange(0, number_ - 1), center_points_x_.TupleSelectRange(0, number_ - 1),
                        tile_rect_.TupleSelectRange(0, number_ - 1), tile_rect_.TupleSelectRange(0, number_ - 1), tile_rect_.TupleSelectRange(0, number_ - 1), tile_rect_.TupleSelectRange(0, number_ - 1),
                        frame_pwidth_ * array_x_count_ - (frame_pwidth_ - array_x_pix_) * (array_x_count_ - 1), frame_pheight_ * array_y_count_ - (frame_pheight_ - array_y_pix_) * (array_y_count_ - 1));
                }

                all_concat_images_.Dispose(); // (20190403) Jeff Revised!

                // 轉成RGB image (20180822) Jeff Revised!
                //HTuple Channels;
                //HOperatorSet.CountChannels(tile_image_, out Channels);
                //if (Channels == 1)
                //{
                //    HOperatorSet.Compose3(tile_image_.Clone(), tile_image_.Clone(), tile_image_.Clone(), out tile_image_); // (20181224) Jeff Revised!
                //}

                return tile_image_;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 所有觸發影像拼接
        /// </summary>
        /// <param name="all_concat_images_"></param>
        /// <param name="b_resize_finished">輸入影像all_concat_images_是否已做resize</param>
        /// <returns></returns>
        public HObject tile_images2(HObject all_concat_images_, bool b_resize_finished = false) // (20200429) Jeff Revised!
        {
            HObject tile_image_ = null;

            try
            {
                if (resize_ != 1.0 && !b_resize_finished) // (20190606) Jeff Revised!
                {
                    // Resize
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ZoomImageFactor(all_concat_images_, out ExpTmpOutVar_0, resize_, resize_, "bilinear");
                        all_concat_images_.Dispose();
                        all_concat_images_ = ExpTmpOutVar_0;
                    }
                }

                HTuple number_;
                HOperatorSet.CountObj(all_concat_images_, out number_);
                
                double array_x_pix_ = array_x_rpitch_ * 1000 / pixel_resolution_;
                double array_y_pix_ = array_y_rpitch_ * 1000 / pixel_resolution_;
                if ((array_x_pix_ >= frame_pwidth_) && (array_y_pix_ >= frame_pheight_)) // 兩相鄰影像沒有Overlap
                {
                    HOperatorSet.TileImagesOffset(all_concat_images_, out tile_image_, center_points_y_.TupleSelectRange(0, number_ - 1), center_points_x_.TupleSelectRange(0, number_ - 1),
                        tile_rect_.TupleSelectRange(0, number_ - 1), tile_rect_.TupleSelectRange(0, number_ - 1), tile_rect_.TupleSelectRange(0, number_ - 1), tile_rect_.TupleSelectRange(0, number_ - 1),
                        frame_pwidth_ * array_x_count_, frame_pheight_ * array_y_count_);
                }
                else // 兩相鄰影像有Overlap
                {
                    HOperatorSet.TileImagesOffset(all_concat_images_, out tile_image_, center_points_y_.TupleSelectRange(0, number_ - 1), center_points_x_.TupleSelectRange(0, number_ - 1),
                        tile_rect_.TupleSelectRange(0, number_ - 1), tile_rect_.TupleSelectRange(0, number_ - 1), tile_rect_.TupleSelectRange(0, number_ - 1), tile_rect_.TupleSelectRange(0, number_ - 1),
                        frame_pwidth_ * array_x_count_ - (frame_pwidth_ - array_x_pix_) * (array_x_count_ - 1), frame_pheight_ * array_y_count_ - (frame_pheight_ - array_y_pix_) * (array_y_count_ - 1));
                }

                //all_concat_images_.Dispose(); // (20190403) Jeff Revised!

                // 轉成RGB image (20180822) Jeff Revised!
                //HTuple Channels;
                //HOperatorSet.CountChannels(tile_image_, out Channels);
                //if (Channels == 1)
                //{
                //    HOperatorSet.Compose3(tile_image_.Clone(), tile_image_.Clone(), tile_image_.Clone(), out tile_image_); // (20181224) Jeff Revised!
                //}

                this.TileImage = tile_image_; // (20200429) Jeff Revised!
                return tile_image_;
            }
            catch
            {
                Extension.HObjectMedthods.ReleaseHObject(ref tile_image_);
                return null;
            }
        }

        /// <summary>
        /// 總Cell顆數
        /// </summary>
        /// <returns></returns>
        public int get_total_cell_count() // (20190722) Jeff Revised!
        {
            return cell_row_count_ * cell_col_count_ - (LocateMethodRecipe.ListPt_BypassCell.Count);
        }

        public List<Point> Get_all_CellDefect_id_List()
        {
            return this.all_CellDefect_id_;
        }

        public List<Point> get_total_defect_pos()
        {
            return this.all_defect_id_;
        }

        public string BoardType
        {
            get { return this.board_type_; }
            set { this.board_type_ = value; }
        }

        public double ElapseTime // (20181116) Jeff Revised!
        {
            get { return this.elapseTime; }
        }

        /// <summary>
        /// 清除所有瑕疵區域及瑕疵座標
        /// </summary>
        /// <returns></returns>
        public bool clear_all_defects() // (20200429) Jeff Revised!
        {
            bool b_status_ = false;
            try
            {
                Extension.HObjectMedthods.ReleaseHObject(ref this.all_intersection_defect_);
                HOperatorSet.GenEmptyRegion(out this.all_intersection_defect_);

                Extension.HObjectMedthods.ReleaseHObject(ref this.all_intersection_defect_CellReg); // (20190720) Jeff Revised!
                HOperatorSet.GenEmptyRegion(out this.all_intersection_defect_CellReg);

                Extension.HObjectMedthods.ReleaseHObject(ref this.all_intersection_Cell_); // (20190703) Jeff Revised!
                HOperatorSet.GenEmptyRegion(out this.all_intersection_Cell_);

                Extension.HObjectMedthods.ReleaseHObject(ref this.all_BackDefect_);
                HOperatorSet.GenEmptyRegion(out this.all_BackDefect_);

                this.all_defect_id_.Clear();
                this.all_defect_id_ = new List<Point>();
                this.all_Cell_id_.Clear(); // (20181207) Jeff Revised!
                this.all_Cell_id_ = new List<Point>(); // (20181207) Jeff Revised!
                this.all_CellDefect_id_.Clear(); // (20200429) Jeff Revised!
                this.all_CellDefect_id_ = new List<Point>(); // (20200429) Jeff Revised!

                // 初始化瑕疵分類
                this.Initialize_DefectsClassify();

                // 初始化人工覆判
                this.Initialize_Recheck();

                // 初始化【計算瑕疵座標測試】
                //this.Dispose_Dictionary_CellImage(); // (20200429) Jeff Revised!

                //GC.Collect();
                //GC.WaitForPendingFinalizers();

                b_status_ = true;
            }
            catch
            { }

            return b_status_;
        }

        /// <summary>
        /// 釋放記憶體
        /// </summary>
        public void Dispose() // (20200429) Jeff Revised!
        {
            Extension.HObjectMedthods.ReleaseHObject(ref this.TileImage);
            this.Dispose_Dictionary_CellImage();
        }

        /// <summary>
        /// 尋找邊緣頂點
        /// </summary>
        /// <param name="ho_srcImage"></param>
        /// <param name="ho_ROI"></param>
        /// <param name="ho_ThvResultRgn"></param>
        /// <param name="ho_EdgeResultRgn"></param>
        /// <param name="ho_MarkRgn"></param>
        /// <param name="hv_Thv_Low"></param>
        /// <param name="hv_Thv_High"></param>
        /// <param name="hv_DeNoiseSize"></param>
        /// <param name="hv_PosOption"></param>
        /// <param name="hv_TransType"></param>
        /// <param name="hv_OpeningSize"></param>
        /// <param name="hv_ClosingSize"></param>
        /// <param name="hv_MarkX"></param>
        /// <param name="hv_MarkY"></param>
        /// <returns></returns>
        public bool FindMark_EdgeFeature_Method(HObject ho_srcImage, HObject ho_ROI, out HObject ho_ThvResultRgn, out HObject ho_EdgeResultRgn, out HObject ho_MarkRgn, HTuple hv_Thv_Low, HTuple hv_Thv_High, HTuple hv_DeNoiseSize, enuSearchType hv_PosOption, HTuple hv_TransType, HTuple hv_OpeningSize, HTuple hv_ClosingSize, out HTuple hv_MarkX, out HTuple hv_MarkY)
        {
            bool Res = false;

            #region 變數宣告
            // Local iconic variables 
            HObject ho_ImageReduced, ho_Region, ho_RegionClosing;
            HObject ho_RegionOpening, ho_Contours, ho_ContoursSplit;
            HObject ho_ObjectSelected = null;
            // Local control variables 
            HTuple hv_XCoordCorners = null, hv_YCoordCorners = null;
            HTuple hv_Number = null, hv_Index = null, hv_RowBegin = new HTuple();
            HTuple hv_ColBegin = new HTuple(), hv_RowEnd = new HTuple();
            HTuple hv_ColEnd = new HTuple(), hv_Nr = new HTuple();
            HTuple hv_Nc = new HTuple(), hv_Dist = new HTuple(), hv_Area = null;
            HTuple hv_Row = null, hv_Column = null, hv_Length = null;
            HTuple hv_I = null, hv_TopLeftX = new HTuple(), hv_TopLeftY = new HTuple();
            HTuple hv_BottomLeftX = new HTuple(), hv_BottomLeftY = new HTuple();
            HTuple hv_TopRightX = new HTuple(), hv_TopRightY = new HTuple();
            HTuple hv_BottomRightX = new HTuple(), hv_BottomRightY = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ThvResultRgn);
            HOperatorSet.GenEmptyObj(out ho_EdgeResultRgn);
            HOperatorSet.GenEmptyObj(out ho_MarkRgn);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening);
            HOperatorSet.GenEmptyObj(out ho_Contours);
            HOperatorSet.GenEmptyObj(out ho_ContoursSplit);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected);
            hv_MarkX = new HTuple();
            hv_MarkY = new HTuple();

            #endregion

            try
            {

                #region 前處理
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_srcImage, ho_ROI, out ho_ImageReduced);
                ho_Region.Dispose();
                HOperatorSet.Threshold(ho_ImageReduced, out ho_Region, hv_Thv_Low, hv_Thv_High);
                ho_ThvResultRgn.Dispose();
                HOperatorSet.ErosionCircle(ho_Region, out ho_ThvResultRgn, hv_DeNoiseSize);
                ho_RegionClosing.Dispose();
                HOperatorSet.ClosingCircle(ho_ThvResultRgn, out ho_RegionClosing, hv_ClosingSize);
                ho_RegionOpening.Dispose();
                HOperatorSet.OpeningCircle(ho_RegionClosing, out ho_RegionOpening, hv_OpeningSize);
                #endregion

                #region 取得四頂點座標
                ho_EdgeResultRgn.Dispose();
                HOperatorSet.ShapeTrans(ho_RegionOpening, out ho_EdgeResultRgn, hv_TransType);
                ho_Contours.Dispose();
                HOperatorSet.GenContourRegionXld(ho_EdgeResultRgn, out ho_Contours, "border");
                ho_ContoursSplit.Dispose();
                HOperatorSet.SegmentContoursXld(ho_Contours, out ho_ContoursSplit, "lines", 5, 10, 1);
                hv_XCoordCorners = new HTuple();
                hv_YCoordCorners = new HTuple();

                HOperatorSet.CountObj(ho_ContoursSplit, out hv_Number);
                HTuple end_val15 = hv_Number;
                HTuple step_val15 = 1;
                for (hv_Index = 1; hv_Index.Continue(end_val15, step_val15); hv_Index = hv_Index.TupleAdd(step_val15))
                {
                    ho_ObjectSelected.Dispose();
                    HOperatorSet.SelectObj(ho_ContoursSplit, out ho_ObjectSelected, hv_Index);
                    HOperatorSet.FitLineContourXld(ho_ObjectSelected, "tukey", -1, 0, 5, 3, out hv_RowBegin,
                        out hv_ColBegin, out hv_RowEnd, out hv_ColEnd, out hv_Nr, out hv_Nc, out hv_Dist);
                    HOperatorSet.TupleConcat(hv_YCoordCorners, hv_RowBegin, out hv_YCoordCorners);
                    HOperatorSet.TupleConcat(hv_XCoordCorners, hv_ColBegin, out hv_XCoordCorners);
                }

                #endregion

                #region Setting Output
                HOperatorSet.AreaCenter(ho_EdgeResultRgn, out hv_Area, out hv_Row, out hv_Column);
                HOperatorSet.TupleLength(hv_XCoordCorners, out hv_Length);
                HTuple end_val22 = hv_Length - 1;
                HTuple step_val22 = 1;
                for (hv_I = 0; hv_I.Continue(end_val22, step_val22); hv_I = hv_I.TupleAdd(step_val22))
                {
                    if ((int)(new HTuple(((hv_XCoordCorners.TupleSelect(hv_I))).TupleLess(hv_Column))) != 0)
                    {
                        if ((int)(new HTuple(((hv_YCoordCorners.TupleSelect(hv_I))).TupleLess(hv_Row))) != 0)
                        {
                            hv_TopLeftX = hv_XCoordCorners.TupleSelect(hv_I);
                            hv_TopLeftY = hv_YCoordCorners.TupleSelect(hv_I);
                        }
                        else
                        {
                            hv_BottomLeftX = hv_XCoordCorners.TupleSelect(hv_I);
                            hv_BottomLeftY = hv_YCoordCorners.TupleSelect(hv_I);
                        }
                    }
                    else
                    {
                        if ((int)(new HTuple(((hv_YCoordCorners.TupleSelect(hv_I))).TupleLess(hv_Row))) != 0)
                        {
                            hv_TopRightX = hv_XCoordCorners.TupleSelect(hv_I);
                            hv_TopRightY = hv_YCoordCorners.TupleSelect(hv_I);
                        }
                        else
                        {
                            hv_BottomRightX = hv_XCoordCorners.TupleSelect(hv_I);
                            hv_BottomRightY = hv_YCoordCorners.TupleSelect(hv_I);
                        }
                    }

                }

                if (hv_PosOption == enuSearchType.TopLeft)
                {
                    hv_MarkX = hv_TopLeftX.Clone();
                    hv_MarkY = hv_TopLeftY.Clone();
                }
                else if (hv_PosOption == enuSearchType.BottomLeft)
                {
                    hv_MarkX = hv_BottomLeftX.Clone();
                    hv_MarkY = hv_BottomLeftY.Clone();
                }
                else if (hv_PosOption == enuSearchType.TopRight)
                {
                    hv_MarkX = hv_TopRightX.Clone();
                    hv_MarkY = hv_TopRightY.Clone();
                }
                else if (hv_PosOption == enuSearchType.BottomRight)
                {
                    hv_MarkX = hv_BottomRightX.Clone();
                    hv_MarkY = hv_BottomRightY.Clone();
                }
                else if (hv_PosOption == enuSearchType.Center)
                {
                    hv_MarkX = hv_Column.Clone();
                    hv_MarkY = hv_Row.Clone();
                }
                else
                {
                    ho_MarkRgn.Dispose();
                    ho_ImageReduced.Dispose();
                    ho_Region.Dispose();
                    ho_RegionClosing.Dispose();
                    ho_RegionOpening.Dispose();
                    ho_Contours.Dispose();
                    ho_ContoursSplit.Dispose();
                    ho_ObjectSelected.Dispose();
                    Res = true;
                    return Res;
                }
                HOperatorSet.GenCircle(out ho_MarkRgn, hv_MarkY, hv_MarkX, 15);

                #endregion
            }
            catch (HalconException ex)
            {
                ho_MarkRgn.Dispose();
                ho_ImageReduced.Dispose();
                ho_Region.Dispose();
                ho_RegionClosing.Dispose();
                ho_RegionOpening.Dispose();
                ho_Contours.Dispose();
                ho_ContoursSplit.Dispose();
                ho_ObjectSelected.Dispose();
                return Res;
            }

            #region Dispose
            ho_MarkRgn.Dispose();
            ho_ImageReduced.Dispose();
            ho_Region.Dispose();
            ho_RegionClosing.Dispose();
            ho_RegionOpening.Dispose();
            ho_Contours.Dispose();
            ho_ContoursSplit.Dispose();
            ho_ObjectSelected.Dispose();
            #endregion


            Res = true;
            return Res;
        }



        #region 【計算瑕疵座標測試】

        /// <summary>
        /// 瑕疵檔路徑
        /// </summary>
        public string Path_DefectTest { get; set; } = ""; // (20200429) Jeff Revised!

        /// <summary>
        /// Cell影像 資訊
        /// </summary>
        public cls_CellImage Cls_CellImage { get; set; } = new cls_CellImage(); // (20200429) Jeff Revised!

        /// <summary>
        /// 每顆Cell之影像
        /// </summary>
        [XmlIgnore] // 帶有XmlIgnore，表示序列化時不序列化此屬性
        [NonSerialized] // 不做DeepClone()!!! (20200429) Jeff Revised!
        //public Dictionary<Point, clsHalconRegion> Dictionary_CellImage { get; set; } = new Dictionary<Point, clsHalconRegion>(); // (20200429) Jeff Revised!
        public Dictionary<Point, clsHalconRegion> Dictionary_CellImage = new Dictionary<Point, clsHalconRegion>(); // (20200429) Jeff Revised!

        /// <summary>
        /// 初始化【計算瑕疵座標測試】的每顆Cell之影像
        /// </summary>
        public void Dispose_Dictionary_CellImage() // (20190801) Jeff Revised!
        {
            try
            {
                foreach (clsHalconRegion cls in this.Dictionary_CellImage.Values)
                {
                    if (cls != null)
                        cls.Dispose();
                }
            }
            catch (Exception ex)
            { }
            this.Dictionary_CellImage.Clear();
            this.Dictionary_CellImage = new Dictionary<Point, clsHalconRegion>();
            this.Cls_CellImage.B_CellImage_Done = false;
        }

        /// <summary>
        /// 由 LocateMethod 取得 DefectTest_Recipe
        /// </summary>
        /// <param name="directory_"></param>
        /// <returns></returns>
        public DefectTest_Recipe Get_DefectTest_Recipe(string directory_ = null) // (20200429) Jeff Revised!
        {
            DefectTest_Recipe Recipe = new DefectTest_Recipe();

            Recipe.LocateMethodRecipe = this; // (20200429) Jeff Revised!

            #region 大圖拼接與瑕疵統計相關資訊
            
            Recipe.capture_map_ = this.capture_map_;
            Recipe.all_intersection_defect_ = this.all_intersection_defect_;
            Recipe.all_intersection_defect_CellReg = this.all_intersection_defect_CellReg;
            Recipe.all_intersection_defect_Recheck = this.all_intersection_defect_Recheck;
            Recipe.all_BackDefect_ = this.all_BackDefect_;
            Recipe.cellmap_affine_ = this.cellmap_affine_;
            Recipe.cellmap_sortregion_ = this.cellmap_sortregion_; // (20200429) Jeff Revised!
            Recipe.all_intersection_Cell_ = this.all_intersection_Cell_;
            Recipe.MarkCenter_BigMap_affine = this.MarkCenter_BigMap_affine;
            Recipe.MarkCenter_BigMap_orig = this.MarkCenter_BigMap_orig;

            if (this.TileImage == null) // (20200429) Jeff Revised!
                HOperatorSet.GenEmptyObj(out this.TileImage);
            Recipe.tile_image_ = this.TileImage;
            HTuple width, height; // (20190729) Jeff Revised!
            HOperatorSet.GetImageSize(this.TileImage, out width, out height);
            Recipe.width_tileImage = width.I;
            Recipe.height_tileImage = height.I;

            #endregion

            #region Halcon Region
            
            if (this.Cls_CellImage.B_CellImage_Done)
            {
                if (this.Cls_CellImage.Type_CellImage == enu_Type_CellImage.AllCellImage) // 【所有Cell影像】
                    Recipe.Dictionary_AllCellImage = this.Dictionary_CellImage;
                else if (this.Cls_CellImage.Type_CellImage == enu_Type_CellImage.DefectCellImage) // 【瑕疵Cell影像】
                    Recipe.Dictionary_DefectCellImage = this.Dictionary_CellImage;
            }

            #endregion

            #region 瑕疵分類 & 人工覆判

            Recipe.DefectsClassify = this.DefectsClassify;
            Recipe.Defects_priority = this.Defects_priority;
            Recipe.all_intersection_cellLabelled_Recheck = this.all_intersection_cellLabelled_Recheck;

            #endregion

            if (directory_ != null)
                Recipe.Directory_ = directory_;

            // 更新 ListRegion_DefectTest
            Recipe.Update_ListRegion_DefectTest();

            return Recipe;
        }
        
        /// <summary>
        /// 由 DefectTest_Recipe 更新 LocateMethod
        /// </summary>
        /// <param name="locateMethod_"></param>
        /// <param name="Recipe"></param>
        public static void UpdateLocateMethod_From_DefectTest_Recipe(ref LocateMethod locateMethod_, DefectTest_Recipe Recipe) // (20200429) Jeff Revised!
        {
            HOperatorSet.SetSystem("clip_region", "false");
            
            locateMethod_ = Recipe.LocateMethodRecipe; // (20200429) Jeff Revised!

            #region 大圖拼接與瑕疵統計相關資訊
            
            locateMethod_.capture_map_ = Recipe.capture_map_;
            locateMethod_.all_intersection_defect_ = Recipe.all_intersection_defect_;
            locateMethod_.all_intersection_defect_CellReg = Recipe.all_intersection_defect_CellReg;
            locateMethod_.all_intersection_defect_Recheck = Recipe.all_intersection_defect_Recheck;
            locateMethod_.all_BackDefect_ = Recipe.all_BackDefect_;
            locateMethod_.cellmap_affine_ = Recipe.cellmap_affine_;
            locateMethod_.cellmap_sortregion_ = Recipe.cellmap_sortregion_; // (20200429) Jeff Revised!
            locateMethod_.all_intersection_Cell_ = Recipe.all_intersection_Cell_;
            locateMethod_.MarkCenter_BigMap_affine = Recipe.MarkCenter_BigMap_affine;
            locateMethod_.MarkCenter_BigMap_orig = Recipe.MarkCenter_BigMap_orig;
            locateMethod_.TileImage = Recipe.tile_image_; // (20200429) Jeff Revised!

            #endregion

            #region Halcon Region

            if (locateMethod_.Cls_CellImage.B_CellImage_Done)
            {
                if (locateMethod_.Cls_CellImage.Type_CellImage == enu_Type_CellImage.AllCellImage) // 【所有Cell影像】
                    locateMethod_.Dictionary_CellImage = Recipe.Dictionary_AllCellImage;
                else if (locateMethod_.Cls_CellImage.Type_CellImage == enu_Type_CellImage.DefectCellImage) // 【瑕疵Cell影像】
                    locateMethod_.Dictionary_CellImage = Recipe.Dictionary_DefectCellImage;
            }

            #endregion

            #region 瑕疵分類 & 人工覆判

            locateMethod_.DefectsClassify = Recipe.DefectsClassify;
            locateMethod_.Defects_priority = Recipe.Defects_priority;
            locateMethod_.all_intersection_cellLabelled_Recheck = Recipe.all_intersection_cellLabelled_Recheck;

            #endregion

            HOperatorSet.SetSystem("clip_region", "true");
        }

        /// <summary>
        /// 儲存 DefectTest_Recipe 類別工單
        /// </summary>
        /// <param name="directory_"></param>
        /// <param name="Recipe"></param>
        /// <returns></returns>
        public bool Save_DefectTest_Recipe(string directory_ = null, DefectTest_Recipe Recipe = null) // (20200429) Jeff Revised!
        {
            bool b_status_ = false;
            try
            {
                if (Recipe == null)
                    Recipe = this.Get_DefectTest_Recipe(directory_);
                
                if (clsStaticTool.SaveXML(Recipe, Recipe.Directory_ + "DefectTest_Recipe.xml"))
                    b_status_ = Recipe.SaveHalconFile();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }

            return b_status_;
        }

        /// <summary>
        /// 載入 DefectTest_Recipe 類別工單
        /// </summary>
        /// <param name="locateMethod_">載入工單對應之 LocateMethod</param>
        /// <param name="directory_"></param>
        /// <returns></returns>
        public static bool Load_DefectTest_Recipe(ref LocateMethod locateMethod_, string directory_) // (20200429) Jeff Revised!
        {
            bool b_status_ = false;

            try
            {
                if (File.Exists(directory_ + "DefectTest_Recipe.xml"))
                {
                    DefectTest_Recipe Recipe;
                    if (clsStaticTool.LoadXML(directory_ + "DefectTest_Recipe.xml", out Recipe)) // (20200429) Jeff Revised!
                    {
                        // 避免使用者移動資料夾導致路徑變更，必須更新路徑為當前使用者選擇之路徑
                        Recipe.LocateMethodRecipe.Path_DefectTest = directory_.Substring(0, directory_.Length - 1);
                        Recipe.Directory_ = directory_;
                        if (Recipe.ReadHalconFile())
                        {
                            // 更新到 LocateMethod
                            LocateMethod.UpdateLocateMethod_From_DefectTest_Recipe(ref locateMethod_, Recipe);
                            
                            b_status_ = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }

            return b_status_;
        }

        /// <summary>
        /// 計算每顆Cell之所有影像
        /// </summary>
        /// <param name="listMapItem">瑕疵地圖各位置相關資訊</param>
        /// <returns></returns>
        public bool Compute_Dictionary_AllCellImage(List<MapItem> listMapItem) // (20190801) Jeff Revised!
        {
            bool b_status_ = false;

            // 初始化【計算瑕疵座標測試】的每顆Cell之影像
            this.Dispose_Dictionary_CellImage();

            if (listMapItem.Count != this.array_x_count_ * this.array_y_count_)
                return false;

            try
            {
                List<string> ListStr = new List<string>();
                int Method = 4; // Method: 1 ~ 5, Note: Method 4 速度最快!
                if (Method == 1 || Method == 2)
                {
                    #region Method 1, 2

                    for (int capture_index_ = 1; capture_index_ <= listMapItem.Count; capture_index_++) // 每一檢測位置
                    {
                        // 取出每一檢測位置之所有影像
                        HObject Images_capture_index = clsStaticTool.ListHObject_2_concatHObject(listMapItem[capture_index_ - 1].ImgObj.Source);

                        // 計算此位置所有Cell
                        HObject CellReg_MoveIndex;
                        this.Compute_CellReg_MoveIndex(capture_index_, out CellReg_MoveIndex);

                        HTuple area_, row_, col_;
                        HOperatorSet.AreaCenter(CellReg_MoveIndex, out area_, out row_, out col_);

                        if (Method == 1)
                        {
                            #region Method 1

                            for (int i = 0; i < area_.Length; i++) // 每一顆Cell
                            {
                                // 計算此Cell之座標
                                HTuple cell_ids = new HTuple();
                                List<Point> cell_1id = new List<Point>();
                                if (!this.pos_2_cellID_MoveIndex(capture_index_, row_[i], col_[i], out cell_ids, out cell_1id))
                                {
                                    Extension.HObjectMedthods.ReleaseHObject(ref Images_capture_index);
                                    Extension.HObjectMedthods.ReleaseHObject(ref CellReg_MoveIndex);
                                    return false;
                                }

                                // 計算此Cell之所有影像
                                HObject Imgs_Cell;
                                HOperatorSet.ReduceDomain(Images_capture_index, CellReg_MoveIndex.SelectObj(i + 1), out Imgs_Cell);
                                {
                                    HObject ExpTmpOutVar_0;
                                    HOperatorSet.CropDomain(Imgs_Cell, out ExpTmpOutVar_0);
                                    Imgs_Cell.Dispose();
                                    Imgs_Cell = ExpTmpOutVar_0;
                                }

                                // 判斷是否已存在此座標，如果沒有則加入至 Dictionary_CellImage
                                Point pt = cell_1id[0];
                                if (!this.Dictionary_CellImage.ContainsKey(pt))
                                {
                                    try
                                    {
                                        this.Dictionary_CellImage.Add(pt, clsHalconRegion.clsHalconRegion_Constructor_Regs(clsStaticTool.concatHObject_2_ListHObject(Imgs_Cell), ListStr, ListStr, pt));
                                    }
                                    catch (Exception ex) // 已存在此座標
                                    { }
                                }
                                Extension.HObjectMedthods.ReleaseHObject(ref Imgs_Cell);
                            }

                            #endregion

                        }
                        else if (Method == 2)
                        {
                            #region Method 2

                            // 計算此位置所有Cell之座標
                            List<Point> cell_id = new List<Point>();
                            this.MultiPos_2_cellID_MoveIndex(capture_index_, clsStaticTool.HTuple_2_ListDouble(row_), clsStaticTool.HTuple_2_ListDouble(col_), out cell_id);

                            // 計算此位置所有Cell之所有影像
                            HTuple hv_Value_R1 = new HTuple(), hv_Value_C1 = new HTuple(), hv_Value_R2 = new HTuple(), hv_Value_C2 = new HTuple();
                            HOperatorSet.RegionFeatures(CellReg_MoveIndex, "row1", out hv_Value_R1);
                            HOperatorSet.RegionFeatures(CellReg_MoveIndex, "column1", out hv_Value_C1);
                            HOperatorSet.RegionFeatures(CellReg_MoveIndex, "row2", out hv_Value_R2);
                            HOperatorSet.RegionFeatures(CellReg_MoveIndex, "column2", out hv_Value_C2);
                            HObject Imgs_Cells;
                            try
                            {
                                //lock (this) // 鎖住資源，否則會跳例外狀況!!!
                                {
                                    HOperatorSet.CropRectangle1(Images_capture_index, out Imgs_Cells, hv_Value_R1, hv_Value_C1, hv_Value_R2, hv_Value_C2);
                                }
                            }
                            catch (Exception ex)
                            {
                                if (Images_capture_index != null)
                                {
                                    int count_img_ = Images_capture_index.CountObj();
                                    // For debug!
                                    Directory.CreateDirectory("D:\\Exception");
                                    for (int i = 1; i <= count_img_; i++)
                                        HOperatorSet.WriteImage(Images_capture_index.SelectObj(i), "tiff", 0, "D:\\Exception\\" + "Images_capture_index " + i);
                                }
                                Extension.HObjectMedthods.ReleaseHObject(ref Images_capture_index);
                                Extension.HObjectMedthods.ReleaseHObject(ref CellReg_MoveIndex);
                                return false;
                            }

                            // 判斷是否已存在此座標，如果沒有則加入至 Dictionary_CellImage
                            int count_img = Images_capture_index.CountObj();
                            int count_cell = cell_id.Count;
                            for (int i = 0; i < count_cell; i++) // 每一顆Cell
                            {
                                Point pt = cell_id[i];
                                if (!this.Dictionary_CellImage.ContainsKey(pt))
                                {
                                    // 計算此座標之所有Cell影像
                                    List<HObject> ListImg = new List<HObject>();
                                    for (int ii = 0; ii < count_img; ii++)
                                        ListImg.Add(Imgs_Cells.SelectObj(i + 1 + ii * count_cell));
                                    try
                                    {
                                        this.Dictionary_CellImage.Add(pt, clsHalconRegion.clsHalconRegion_Constructor_Regs(ListImg, ListStr, ListStr, pt));
                                        //clsStaticTool.DisposeAll(ListImg);
                                    }
                                    catch (Exception ex) // 已存在此座標
                                    { }
                                }
                            }
                            Extension.HObjectMedthods.ReleaseHObject(ref Imgs_Cells);

                            #endregion

                        }

                        Extension.HObjectMedthods.ReleaseHObject(ref Images_capture_index);
                        Extension.HObjectMedthods.ReleaseHObject(ref CellReg_MoveIndex);
                    }

                    #endregion

                }
                else if (Method == 3 || Method == 4) // 平行運算 (Parallel Programming)
                {
                    #region Method 3, 4

                    bool b_error_Parallel = false; // 平行運算中，是否遇到例外狀況
                    Dictionary<Point, clsHalconRegion> dictionary_CellImage_temp = new Dictionary<Point, clsHalconRegion>();
                    Parallel.For(1, listMapItem.Count + 1, (capture_index_, loopState) => // 每一檢測位置
                    {
                        try
                        {
                            // 取出每一檢測位置之所有影像
                            //HObject Images_capture_index = clsStaticTool.ListHObject_2_concatHObject(listMapItem[capture_index_ - 1].ImgObj.Source);
                            HObject Images_capture_index = null;
                            int count_img = listMapItem[capture_index_ - 1].ImgObj.Source.Count;

                            // 計算此位置所有Cell
                            HObject CellReg_MoveIndex;
                            this.Compute_CellReg_MoveIndex(capture_index_, out CellReg_MoveIndex);

                            HTuple area_, row_, col_;
                            HOperatorSet.AreaCenter(CellReg_MoveIndex, out area_, out row_, out col_);

                            if (Method == 3)
                            {
                                #region Method 3

                                Images_capture_index = clsStaticTool.ListHObject_2_concatHObject(listMapItem[capture_index_ - 1].ImgObj.Source);
                                for (int i = 0; i < area_.Length; i++) // 每一顆Cell
                                {
                                    // 計算此Cell之座標
                                    HTuple cell_ids = new HTuple();
                                    List<Point> cell_1id = new List<Point>();
                                    if (!this.pos_2_cellID_MoveIndex(capture_index_, row_[i], col_[i], out cell_ids, out cell_1id))
                                    {
                                        Extension.HObjectMedthods.ReleaseHObject(ref CellReg_MoveIndex);
                                        // 停止並退出Parallel.For
                                        loopState.Stop();
                                        b_error_Parallel = true;
                                        return;
                                    }

                                    // 計算此Cell之所有影像
                                    HObject Imgs_Cell;
                                    HOperatorSet.ReduceDomain(Images_capture_index, CellReg_MoveIndex.SelectObj(i + 1), out Imgs_Cell);
                                    {
                                        HObject ExpTmpOutVar_0;
                                        HOperatorSet.CropDomain(Imgs_Cell, out ExpTmpOutVar_0);
                                        Imgs_Cell.Dispose();
                                        Imgs_Cell = ExpTmpOutVar_0;
                                    }

                                    // 判斷是否已存在此座標，如果沒有則加入至 dictionary_CellImage_temp
                                    Point pt = cell_1id[0];
                                    if (!dictionary_CellImage_temp.ContainsKey(pt))
                                    {
                                        try
                                        {
                                            dictionary_CellImage_temp.Add(pt, clsHalconRegion.clsHalconRegion_Constructor_Regs(clsStaticTool.concatHObject_2_ListHObject(Imgs_Cell), ListStr, ListStr, pt));
                                        }
                                        catch (Exception ex) // 已存在此座標
                                        { }
                                    }
                                    Extension.HObjectMedthods.ReleaseHObject(ref Imgs_Cell);

                                }

                                #endregion

                            }
                            else if (Method == 4)
                            {
                                #region Method 4

                                // 計算此位置所有Cell之座標
                                List<Point> cell_id = new List<Point>();
                                this.MultiPos_2_cellID_MoveIndex(capture_index_, clsStaticTool.HTuple_2_ListDouble(row_), clsStaticTool.HTuple_2_ListDouble(col_), out cell_id);

                                // 計算此位置所有Cell之所有影像
                                HTuple hv_Value_R1 = new HTuple(), hv_Value_C1 = new HTuple(), hv_Value_R2 = new HTuple(), hv_Value_C2 = new HTuple();
                                HOperatorSet.RegionFeatures(CellReg_MoveIndex, "row1", out hv_Value_R1);
                                HOperatorSet.RegionFeatures(CellReg_MoveIndex, "column1", out hv_Value_C1);
                                HOperatorSet.RegionFeatures(CellReg_MoveIndex, "row2", out hv_Value_R2);
                                HOperatorSet.RegionFeatures(CellReg_MoveIndex, "column2", out hv_Value_C2);
                                HObject Imgs_Cells;
                                try
                                {
                                    // 法1: 該位置所有影像做 CropRectangle1()
                                    // Note: 有時會失敗!!!
                                    //lock (this) // 鎖住資源，否則會跳例外狀況!!!
                                    //{
                                    //    Images_capture_index = clsStaticTool.ListHObject_2_concatHObject(listMapItem[capture_index_ - 1].ImgObj.Source);
                                    //    HOperatorSet.CropRectangle1(Images_capture_index, out Imgs_Cells, hv_Value_R1, hv_Value_C1, hv_Value_R2, hv_Value_C2);
                                    //}

                                    // 法2: 一次1張影像做 CropRectangle1()
                                    HOperatorSet.GenEmptyObj(out Imgs_Cells);
                                    for (int i = 1; i <= count_img; i++)
                                    {
                                        HObject temp;
                                        HOperatorSet.CropRectangle1(listMapItem[capture_index_ - 1].ImgObj.Source[i - 1], out temp, hv_Value_R1, hv_Value_C1, hv_Value_R2, hv_Value_C2);
                                        {
                                            HObject ExpTmpOutVar_0;
                                            HOperatorSet.ConcatObj(Imgs_Cells, temp, out ExpTmpOutVar_0);
                                            Imgs_Cells.Dispose();
                                            Imgs_Cells = ExpTmpOutVar_0;
                                        }
                                        temp.Dispose();
                                    }

                                }
                                catch (Exception ex)
                                {
                                    if (Images_capture_index != null)
                                    {
                                        int count_img_ = Images_capture_index.CountObj();
                                        // For debug!
                                        Directory.CreateDirectory("D:\\Exception");
                                        for (int i = 1; i <= count_img_; i++)
                                            HOperatorSet.WriteImage(Images_capture_index.SelectObj(i), "tiff", 0, "D:\\Exception\\" + "Images_capture_index " + i);
                                    }
                                    Extension.HObjectMedthods.ReleaseHObject(ref Images_capture_index);
                                    Extension.HObjectMedthods.ReleaseHObject(ref CellReg_MoveIndex);
                                    // 停止並退出Parallel.For
                                    loopState.Stop();
                                    b_error_Parallel = true;
                                    return;
                                }

                                // 判斷是否已存在此座標，如果沒有則加入至 dictionary_CellImage_temp
                                int count_cell = cell_id.Count;
                                for (int i = 0; i < count_cell; i++) // 每一顆Cell
                                {
                                    Point pt = cell_id[i];
                                    if (!dictionary_CellImage_temp.ContainsKey(pt))
                                    {
                                        // 計算此座標之所有Cell影像
                                        List<HObject> ListImg = new List<HObject>();
                                        for (int ii = 0; ii < count_img; ii++)
                                            ListImg.Add(Imgs_Cells.SelectObj(i + 1 + ii * count_cell));
                                        try
                                        {
                                            dictionary_CellImage_temp.Add(pt, clsHalconRegion.clsHalconRegion_Constructor_Regs(ListImg, ListStr, ListStr, pt));
                                            //clsStaticTool.DisposeAll(ListImg);
                                        }
                                        catch (Exception ex) // 已存在此座標
                                        { }
                                    }
                                }
                                Extension.HObjectMedthods.ReleaseHObject(ref Imgs_Cells);

                                #endregion

                            }

                            Extension.HObjectMedthods.ReleaseHObject(ref Images_capture_index);
                            Extension.HObjectMedthods.ReleaseHObject(ref CellReg_MoveIndex);
                        }
                        catch (Exception ex)
                        {
                            // 停止並退出Parallel.For
                            loopState.Stop();
                            b_error_Parallel = true;
                            return;
                        }
                    });

                    if (b_error_Parallel) // 判斷平行運算中，是否遇到例外狀況
                    {
                        foreach (clsHalconRegion cls in dictionary_CellImage_temp.Values)
                        {
                            if (cls != null)
                                cls.Dispose();
                        }
                        dictionary_CellImage_temp.Clear();
                        return false;
                    }
                    else
                        this.Dictionary_CellImage = dictionary_CellImage_temp;

                    #endregion

                }
                else if (Method == 5) // (20190801) Jeff Revised!
                {
                    #region Method 5

                    HTuple ConcatCellIndex = new HTuple();
                    HOperatorSet.TupleGenSequence(1, this.cellmap_affine_.CountObj(), 1, out ConcatCellIndex);
                    Dictionary<Point, clsHalconRegion> dictionary_CellImage = new Dictionary<Point, clsHalconRegion>();
                    if (!this.compute_dictionary_cellImage_From_ConcatCellIndex(listMapItem, ConcatCellIndex, out dictionary_CellImage))
                    {
                        foreach (clsHalconRegion cls in dictionary_CellImage.Values)
                        {
                            if (cls != null)
                                cls.Dispose();
                        }
                        dictionary_CellImage.Clear();
                        return false;
                    }
                    else
                        this.Dictionary_CellImage = dictionary_CellImage;

                    #endregion
                }

                this.Cls_CellImage.B_CellImage_Done = true;
                this.Cls_CellImage.CountImg_1Cell = listMapItem[0].ImgObj.Source.Count;
                b_status_ = true;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }

            return b_status_;
        }

        /// <summary>
        /// 計算 指定Cell之ConcatCellIndex 的所有影像
        /// </summary>
        /// <param name="listMapItem">瑕疵地圖各位置相關資訊</param>
        /// <param name="ConcatCellIndex"></param>
        /// <param name="dictionary_CellImage"></param>
        /// <param name="CaptureIndex_"> = -1 (所有檢測位置), >= 1 (單一檢測位置) </param>
        /// <returns></returns>
        public bool compute_dictionary_cellImage_From_ConcatCellIndex(List<MapItem> listMapItem, HTuple ConcatCellIndex, 
                                                                      out Dictionary<Point, clsHalconRegion> dictionary_CellImage, int CaptureIndex_ = -1) // (20190802) Jeff Revised!
        {
            bool b_status_ = false;
            dictionary_CellImage = new Dictionary<Point, clsHalconRegion>();

            if (listMapItem.Count != this.array_x_count_ * this.array_y_count_)
                return false;
            else if (CaptureIndex_ > this.array_x_count_ * this.array_y_count_)
                return false;
            else if (ConcatCellIndex.Length <= 0)
                return true;

            try
            {
                List<string> ListStr = new List<string>();

                // ConcatCellIndex 轉 各自對應之最小MoveIndex (capture_map_ 之index)
                HTuple Concat_MoveIndex;
                this.ConcatCellIndex_2_ConcatMoveIndex(ConcatCellIndex, out Concat_MoveIndex);
                if (Concat_MoveIndex.Length <= 0)
                    return false;

                int Method = 1; // Method: 1 ~ 2
                if (Method == 1 || CaptureIndex_ != -1) // Method 1 or 單一檢測位置
                {
                    #region Method 1 or 單一檢測位置

                    for (int capture_index_ = 1; capture_index_ <= listMapItem.Count; capture_index_++) // 每一檢測位置
                    {
                        if (CaptureIndex_ != -1) // 單一檢測位置
                            capture_index_ = CaptureIndex_;

                        // 判斷該檢測位置是否包含欲得到之cell
                        HTuple hv_Indices = new HTuple();
                        HOperatorSet.TupleFind(Concat_MoveIndex, capture_index_, out hv_Indices);
                        if ((int)(new HTuple(hv_Indices.TupleNotEqual(-1))) != 1)
                            continue;

                        // 取出該檢測位置之所有影像
                        HObject Images_capture_index = clsStaticTool.ListHObject_2_concatHObject(listMapItem[capture_index_ - 1].ImgObj.Source);

                        // 計算此位置包含之Cell
                        HTuple ConcatCellIndex_select = new HTuple();
                        HOperatorSet.TupleSelect(ConcatCellIndex, hv_Indices, out ConcatCellIndex_select);
                        HObject CellReg_MoveIndex;
                        if (!this.compute_CellReg_MoveIndex_From_ConcatCellIndex(capture_index_, ConcatCellIndex_select, out CellReg_MoveIndex))
                        {
                            Extension.HObjectMedthods.ReleaseHObject(ref Images_capture_index);
                            Extension.HObjectMedthods.ReleaseHObject(ref CellReg_MoveIndex);
                            return false;
                        }

                        // 計算此位置包含之Cell座標
                        List<Point> cell_id = this.HTuple_2_ListCellID(ConcatCellIndex_select);

                        // 計算此位置包含之Cell所有影像
                        HTuple hv_Value_R1 = new HTuple(), hv_Value_C1 = new HTuple(), hv_Value_R2 = new HTuple(), hv_Value_C2 = new HTuple();
                        HOperatorSet.RegionFeatures(CellReg_MoveIndex, "row1", out hv_Value_R1);
                        HOperatorSet.RegionFeatures(CellReg_MoveIndex, "column1", out hv_Value_C1);
                        HOperatorSet.RegionFeatures(CellReg_MoveIndex, "row2", out hv_Value_R2);
                        HOperatorSet.RegionFeatures(CellReg_MoveIndex, "column2", out hv_Value_C2);
                        HObject Imgs_Cells;
                        try
                        {
                            //lock (this) // 鎖住資源，否則會跳例外狀況!!!
                            {
                                HOperatorSet.CropRectangle1(Images_capture_index, out Imgs_Cells, hv_Value_R1, hv_Value_C1, hv_Value_R2, hv_Value_C2);
                            }
                        }
                        catch (Exception ex)
                        {
                            if (Images_capture_index != null)
                            {
                                int count_img_ = Images_capture_index.CountObj();
                                // For debug!
                                Directory.CreateDirectory("D:\\Exception");
                                for (int i = 1; i <= count_img_; i++)
                                    HOperatorSet.WriteImage(Images_capture_index.SelectObj(i), "tiff", 0, "D:\\Exception\\" + "Images_capture_index " + i);
                            }
                            Extension.HObjectMedthods.ReleaseHObject(ref Images_capture_index);
                            Extension.HObjectMedthods.ReleaseHObject(ref CellReg_MoveIndex);
                            return false;
                        }

                        // 加入至 dictionary_CellImage
                        int count_img = Images_capture_index.CountObj();
                        int count_cell = cell_id.Count;
                        for (int i = 0; i < count_cell; i++) // 每一顆Cell
                        {
                            Point pt = cell_id[i];
                            // 計算此座標之所有Cell影像
                            List<HObject> ListImg = new List<HObject>();
                            for (int ii = 0; ii < count_img; ii++)
                                ListImg.Add(Imgs_Cells.SelectObj(i + 1 + ii * count_cell));
                            try
                            {
                                dictionary_CellImage.Add(pt, clsHalconRegion.clsHalconRegion_Constructor_Regs(ListImg, ListStr, ListStr, pt));
                                //clsStaticTool.DisposeAll(ListImg);
                            }
                            catch (Exception ex) // 已存在此座標
                            { }
                        }
                        Extension.HObjectMedthods.ReleaseHObject(ref Imgs_Cells);
                        Extension.HObjectMedthods.ReleaseHObject(ref Images_capture_index);
                        Extension.HObjectMedthods.ReleaseHObject(ref CellReg_MoveIndex);

                        if (CaptureIndex_ != -1) // 單一檢測位置
                            break;

                    }

                    #endregion

                }
                else if (Method == 2) // 平行運算 (Parallel Programming)
                {
                    #region Method 2

                    bool b_error_Parallel = false; // 平行運算中，是否遇到例外狀況
                    Dictionary<Point, clsHalconRegion> dictionary_CellImage_temp = new Dictionary<Point, clsHalconRegion>();
                    Parallel.For(1, listMapItem.Count + 1, (capture_index_, loopState) => // 每一檢測位置
                    {
                        try
                        {
                            // 判斷該檢測位置是否包含欲得到之cell
                            HTuple hv_Indices = new HTuple();
                            HOperatorSet.TupleFind(Concat_MoveIndex, capture_index_, out hv_Indices);
                            /*if ((int)(new HTuple(hv_Indices.TupleNotEqual(-1))) != 1)
                            {
                                // 跳出當前執行單元
                                loopState.Break();
                                //return; // 不加return，可能會發生該程序資源未釋放。
                            }*/

                            if ((int)(new HTuple(hv_Indices.TupleNotEqual(-1))) == 1)
                            {
                                // 取出該檢測位置之所有影像
                                //HObject Images_capture_index = clsStaticTool.ListHObject_2_concatHObject(listMapItem[capture_index_ - 1].ImgObj.Source);
                                HObject Images_capture_index = null;
                                int count_img = listMapItem[capture_index_ - 1].ImgObj.Source.Count;

                                // 計算此位置包含之Cell
                                HTuple ConcatCellIndex_select = new HTuple();
                                HOperatorSet.TupleSelect(ConcatCellIndex, hv_Indices, out ConcatCellIndex_select);
                                HObject CellReg_MoveIndex;
                                if (!this.compute_CellReg_MoveIndex_From_ConcatCellIndex(capture_index_, ConcatCellIndex_select, out CellReg_MoveIndex))
                                {
                                    Extension.HObjectMedthods.ReleaseHObject(ref Images_capture_index);
                                    Extension.HObjectMedthods.ReleaseHObject(ref CellReg_MoveIndex);
                                    // 停止並退出Parallel.For
                                    loopState.Stop();
                                    b_error_Parallel = true;
                                    return;
                                }

                                // 計算此位置包含之Cell座標
                                List<Point> cell_id = this.HTuple_2_ListCellID(ConcatCellIndex_select);

                                // 計算此位置包含之Cell所有影像
                                HTuple hv_Value_R1 = new HTuple(), hv_Value_C1 = new HTuple(), hv_Value_R2 = new HTuple(), hv_Value_C2 = new HTuple();
                                HOperatorSet.RegionFeatures(CellReg_MoveIndex, "row1", out hv_Value_R1);
                                HOperatorSet.RegionFeatures(CellReg_MoveIndex, "column1", out hv_Value_C1);
                                HOperatorSet.RegionFeatures(CellReg_MoveIndex, "row2", out hv_Value_R2);
                                HOperatorSet.RegionFeatures(CellReg_MoveIndex, "column2", out hv_Value_C2);
                                HObject Imgs_Cells;
                                try
                                {
                                    // 法1: 該位置所有影像做 CropRectangle1()
                                    // Note: 有時會失敗!!!
                                    //lock (this) // 鎖住資源，否則會跳例外狀況!!!
                                    //{
                                    //    Images_capture_index = clsStaticTool.ListHObject_2_concatHObject(listMapItem[capture_index_ - 1].ImgObj.Source);
                                    //    HOperatorSet.CropRectangle1(Images_capture_index, out Imgs_Cells, hv_Value_R1, hv_Value_C1, hv_Value_R2, hv_Value_C2);
                                    //}

                                    // 法2: 一次1張影像做 CropRectangle1()
                                    HOperatorSet.GenEmptyObj(out Imgs_Cells);
                                    for (int i = 1; i <= count_img; i++)
                                    {
                                        HObject temp;
                                        HOperatorSet.CropRectangle1(listMapItem[capture_index_ - 1].ImgObj.Source[i - 1], out temp, hv_Value_R1, hv_Value_C1, hv_Value_R2, hv_Value_C2);
                                        {
                                            HObject ExpTmpOutVar_0;
                                            HOperatorSet.ConcatObj(Imgs_Cells, temp, out ExpTmpOutVar_0);
                                            Imgs_Cells.Dispose();
                                            Imgs_Cells = ExpTmpOutVar_0;
                                        }
                                        temp.Dispose();
                                    }

                                }
                                catch (Exception ex)
                                {
                                    if (Images_capture_index != null)
                                    {
                                        int count_img_ = Images_capture_index.CountObj();
                                        // For debug!
                                        Directory.CreateDirectory("D:\\Exception");
                                        for (int i = 1; i <= count_img_; i++)
                                            HOperatorSet.WriteImage(Images_capture_index.SelectObj(i), "tiff", 0, "D:\\Exception\\" + "Images_capture_index " + i);
                                    }
                                    Extension.HObjectMedthods.ReleaseHObject(ref Images_capture_index);
                                    Extension.HObjectMedthods.ReleaseHObject(ref CellReg_MoveIndex);
                                    // 停止並退出Parallel.For
                                    loopState.Stop();
                                    b_error_Parallel = true;
                                    return;
                                }

                                // 加入至 dictionary_CellImage_temp
                                int count_cell = cell_id.Count;
                                for (int i = 0; i < count_cell; i++)
                                {
                                    Point pt = cell_id[i];
                                    // 計算此座標之所有Cell影像
                                    List<HObject> ListImg = new List<HObject>();
                                    for (int ii = 0; ii < count_img; ii++)
                                        ListImg.Add(Imgs_Cells.SelectObj(i + 1 + ii * count_cell));
                                    try
                                    {
                                        dictionary_CellImage_temp.Add(pt, clsHalconRegion.clsHalconRegion_Constructor_Regs(ListImg, ListStr, ListStr, pt));
                                        //clsStaticTool.DisposeAll(ListImg);
                                    }
                                    catch (Exception ex) // 已存在此座標
                                    { }
                                }
                                Extension.HObjectMedthods.ReleaseHObject(ref Imgs_Cells);
                                Extension.HObjectMedthods.ReleaseHObject(ref Images_capture_index);
                                Extension.HObjectMedthods.ReleaseHObject(ref CellReg_MoveIndex);
                            }

                        }
                        catch (Exception ex)
                        {
                            // 停止並退出Parallel.For
                            loopState.Stop();
                            b_error_Parallel = true;
                            return;
                        }
                    });

                    if (b_error_Parallel) // 判斷平行運算中，是否遇到例外狀況
                    {
                        foreach (clsHalconRegion cls in dictionary_CellImage_temp.Values)
                        {
                            if (cls != null)
                                cls.Dispose();
                        }
                        dictionary_CellImage_temp.Clear();
                        return false;
                    }
                    else
                        dictionary_CellImage = dictionary_CellImage_temp;

                    #endregion

                }

                b_status_ = true;

            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }

            return b_status_;
        }

        /// <summary>
        /// 計算每顆Cell (或指定Cell座標) 之所有影像
        /// </summary>
        /// <param name="listMapItem"></param>
        /// <param name="b_AllCells">是否計算所有Cell影像</param>
        /// <param name="ListPt_specificCell">指定Cell座標</param>
        /// <returns></returns>
        public bool Compute_Dictionary_CellImage_From_CellReg_MoveIndex(List<MapItem> listMapItem, bool b_AllCells = true, List<Point> ListPt_specificCell = null) // (20200907) Jeff Revised! (拿掉平行運算)
        {
            bool b_status_ = false;

            // 初始化【計算瑕疵座標測試】的每顆Cell之影像
            this.Dispose_Dictionary_CellImage();

            if (listMapItem.Count != this.array_x_count_ * this.array_y_count_)
                return false;
            if (CellReg_MoveIndex_FS.Count != this.array_x_count_ * this.array_y_count_)
            {
                int i = CellReg_MoveIndex_FS.Count; // For debug! // (20200429) Jeff Revised!
                return false;
            }
            if (DefReg_MoveIndex_FS.Count != this.array_x_count_ * this.array_y_count_)
                return false;

            try
            {
                int Method = 4; // Method: 1 ~ 3; Method 1: 顆數少時速度快，但多顆時速度慢，記憶體耗用少;  Method 3: 顆數多時速度快，但少顆時速度慢，記憶體耗用大;
                                // Method 2: 優化Method 1，平行運算完再計算 Dictionary_CellImage 會較穩定!
                                // Method 4: Method 2 拿掉平行運算 // (20200907) Jeff Revised! (拿掉平行運算)
                if (Method == 1) // Method: 1
                {
                    #region Method 1

                    HObject cells;
                    if (b_AllCells)
                        cells = this.cellmap_affine_.Clone();
                    else
                    {
                        if (ListPt_specificCell == null || ListPt_specificCell.Count == 0)
                            return true;
                        HTuple cell_ids = this.ListCellID_2_HTuple(ListPt_specificCell);
                        HOperatorSet.SelectObj(this.cellmap_affine_, out cells, cell_ids);
                    }

                    // 計算各Cell中心座標
                    HTuple x = new HTuple(), y = new HTuple();
                    HOperatorSet.RegionFeatures(cells, "column", out x);
                    HOperatorSet.RegionFeatures(cells, "row", out y);
                    cells.Dispose();

                    // 平行運算 (Parallel Programming)
                    Dictionary<Point, clsHalconRegion> dictionary_CellImage_temp = new Dictionary<Point, clsHalconRegion>();
                    bool b_error_Parallel = false; // 平行運算中，是否遇到例外狀況
                    Parallel.For(0, x.Length, (i, loopState) => // For 每一顆Cell
                    {
                        try
                        {
                            List<int> ListMoveIndex = new List<int>();
                            List<HObject> ListCellReg_MoveIndex = new List<HObject>();
                            if (!this.posBigMapCellCenter_2_MoveIndex(new PointF((float)(x[i].D), (float)(y[i].D)), out ListMoveIndex, out ListCellReg_MoveIndex))
                            {
                                // 停止並退出Parallel.For
                                loopState.Stop();
                                b_error_Parallel = true;
                                return;
                            }

                            if (ListMoveIndex.Count > 0)
                            {
                                // 計算此Cell之所有影像
                                List<HObject> ListImg = new List<HObject>();
                                int count_img = listMapItem[ListMoveIndex[0] - 1].ImgObj.Source.Count;
                                for (int ii = 0; ii < count_img; ii++)
                                {
                                    HObject ReduceImg, CropImg;
                                    HOperatorSet.ReduceDomain(listMapItem[ListMoveIndex[0] - 1].ImgObj.Source[ii], ListCellReg_MoveIndex[0], out ReduceImg);
                                    HOperatorSet.CropDomain(ReduceImg, out CropImg);
                                    ReduceImg.Dispose();
                                    ListImg.Add(CropImg);
                                }
                                
                                // 計算座標
                                HTuple cell_id;
                                List<Point> ListCellID;
                                this.pos_2_cellID((int)(y[i].D), (int)(x[i].D), out cell_id, out ListCellID);
                                Point cellID = ListCellID[0];

                                // 計算此Cell之瑕疵 Region
                                bool b_Defect = false;
                                HObject DefectReg;
                                if (this.all_defect_id_.Contains(cellID))
                                {
                                    b_Defect = true;
                                    HObject DefectReg_inter;
                                    HOperatorSet.Intersection(DefReg_MoveIndex_FS[ListMoveIndex[0] - 1], ListCellReg_MoveIndex[0], out DefectReg_inter);
                                    // 平移到原點
                                    HTuple hv_R1 = new HTuple(), hv_C1 = new HTuple();
                                    HOperatorSet.RegionFeatures(ListCellReg_MoveIndex[0], "row1", out hv_R1);
                                    HOperatorSet.RegionFeatures(ListCellReg_MoveIndex[0], "column1", out hv_C1);
                                    HOperatorSet.MoveRegion(DefectReg_inter, out DefectReg, -hv_R1, -hv_C1);
                                    DefectReg_inter.Dispose();
                                    clsStaticTool.DisposeAll(ListCellReg_MoveIndex);
                                }
                                else
                                    HOperatorSet.GenEmptyRegion(out DefectReg);

                                // 存入 dictionary_CellImage_temp
                                try
                                {
                                    List<string> ListStr = new List<string>();
                                    if (dictionary_CellImage_temp.ContainsKey(cellID) == false)
                                        dictionary_CellImage_temp.Add(cellID, clsHalconRegion.clsHalconRegion_Constructor_Regs(ListImg, ListStr, ListStr, cellID, DefectReg, b_Defect));
                                }
                                catch (Exception ex) // 已存在此座標
                                { }

                            }
                        }
                        catch (Exception ex)
                        {
                            // 停止並退出Parallel.For
                            loopState.Stop();
                            b_error_Parallel = true;
                            return;
                        }
                    });

                    if (b_error_Parallel) // 判斷平行運算中，是否遇到例外狀況
                    {
                        foreach (clsHalconRegion cls in dictionary_CellImage_temp.Values)
                        {
                            if (cls != null)
                                cls.Dispose();
                        }
                        dictionary_CellImage_temp.Clear();
                        return false;
                    }
                    else
                        this.Dictionary_CellImage = dictionary_CellImage_temp;

                    #endregion
                }
                else if (Method == 2)
                {
                    #region Method 2

                    HObject cells;
                    if (b_AllCells)
                        cells = this.cellmap_affine_.Clone();
                    else
                    {
                        if (ListPt_specificCell == null || ListPt_specificCell.Count == 0)
                            return true;
                        HTuple cell_ids = this.ListCellID_2_HTuple(ListPt_specificCell);
                        HOperatorSet.SelectObj(this.cellmap_affine_, out cells, cell_ids);
                    }

                    // 計算各Cell中心座標
                    HTuple x = new HTuple(), y = new HTuple();
                    HOperatorSet.RegionFeatures(cells, "column", out x);
                    HOperatorSet.RegionFeatures(cells, "row", out y);
                    cells.Dispose();

                    // 平行運算 (Parallel Programming)
                    //List<clsHalconRegion> List_clsHalconRegion = new List<clsHalconRegion>();
                    clsHalconRegion[] Array_clsHalconRegion = new clsHalconRegion[x.Length]; // Note: clsHalconRegion[] 比 List<clsHalconRegion> 穩定多了!!!
                    bool b_error_Parallel = false; // 平行運算中，是否遇到例外狀況
                    Parallel.For(0, x.Length, (i, loopState) => // For 每一顆Cell
                    {
                        try
                        {
                            List<int> ListMoveIndex = new List<int>();
                            List<HObject> ListCellReg_MoveIndex = new List<HObject>();
                            if (!this.posBigMapCellCenter_2_MoveIndex(new PointF((float)(x[i].D), (float)(y[i].D)), out ListMoveIndex, out ListCellReg_MoveIndex))
                            {
                                // 停止並退出Parallel.For
                                loopState.Stop();
                                b_error_Parallel = true;
                                return;
                            }

                            if (ListMoveIndex.Count > 0)
                            {
                                // 計算此Cell之所有影像
                                List<HObject> ListImg = new List<HObject>();
                                int count_img = listMapItem[ListMoveIndex[0] - 1].ImgObj.Source.Count;
                                for (int ii = 0; ii < count_img; ii++)
                                {
                                    HObject ReduceImg, CropImg;
                                    HOperatorSet.ReduceDomain(listMapItem[ListMoveIndex[0] - 1].ImgObj.Source[ii], ListCellReg_MoveIndex[0], out ReduceImg);
                                    HOperatorSet.CropDomain(ReduceImg, out CropImg);
                                    ReduceImg.Dispose();
                                    ListImg.Add(CropImg);
                                }

                                // 計算座標
                                HTuple cell_id;
                                List<Point> ListCellID;
                                this.pos_2_cellID((int)(y[i].D), (int)(x[i].D), out cell_id, out ListCellID);
                                Point cellID = ListCellID[0];

                                // 計算此Cell之瑕疵 Region
                                bool b_Defect = false;
                                HObject DefectReg;
                                if (this.all_defect_id_.Contains(cellID))
                                {
                                    b_Defect = true;
                                    HObject DefectReg_inter;
                                    HOperatorSet.Intersection(DefReg_MoveIndex_FS[ListMoveIndex[0] - 1], ListCellReg_MoveIndex[0], out DefectReg_inter);
                                    // 平移到原點
                                    HTuple hv_R1 = new HTuple(), hv_C1 = new HTuple();
                                    HOperatorSet.RegionFeatures(ListCellReg_MoveIndex[0], "row1", out hv_R1);
                                    HOperatorSet.RegionFeatures(ListCellReg_MoveIndex[0], "column1", out hv_C1);
                                    HOperatorSet.MoveRegion(DefectReg_inter, out DefectReg, -hv_R1, -hv_C1);
                                    DefectReg_inter.Dispose();
                                    clsStaticTool.DisposeAll(ListCellReg_MoveIndex);
                                }
                                else
                                    HOperatorSet.GenEmptyRegion(out DefectReg);

                                // 存入 List_clsHalconRegion
                                try
                                {
                                    List<string> ListStr = new List<string>();
                                    //List_clsHalconRegion.Add(clsHalconRegion.clsHalconRegion_Constructor_Regs(ListImg, ListStr, ListStr, cellID, DefectReg, b_Defect));
                                    Array_clsHalconRegion[i] = clsHalconRegion.clsHalconRegion_Constructor_Regs(ListImg, ListStr, ListStr, cellID, DefectReg, b_Defect);
                                }
                                catch (Exception ex)
                                { }

                            }
                            else // Cell瑕疵: AOI未成功找出 Cell Region
                                Array_clsHalconRegion[i] = null;
                        }
                        catch (Exception ex)
                        {
                            // 停止並退出Parallel.For
                            loopState.Stop();
                            b_error_Parallel = true;
                            return;
                        }
                    });

                    if (b_error_Parallel) // 判斷平行運算中，是否遇到例外狀況
                        return false;

                    // 存入 Dictionary_CellImage
                    //foreach (clsHalconRegion cls in List_clsHalconRegion)
                    //foreach (clsHalconRegion cls in Array_clsHalconRegion)
                    for (int i = 0; i < Array_clsHalconRegion.Length; i++)
                    {
                        if (Array_clsHalconRegion[i] == null)
                            continue;
                        clsHalconRegion cls = Array_clsHalconRegion[i];
                        Point pt = cls.cellID;
                        if (this.Dictionary_CellImage.ContainsKey(pt) == false)
                            this.Dictionary_CellImage.Add(pt, cls);
                        else
                            cls.Dispose();
                    }

                    #endregion
                }
                else if (Method == 3) // Method: 3
                {
                    #region Method 3

                    if (CellReg_MoveIndex_FS.Count != this.array_x_count_ * this.array_y_count_)
                        return false;

                    // 每一檢測位置影像邊界範圍
                    HObject Reg_ImageBorder;
                    HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
                    HOperatorSet.GetImageSize(listMapItem[0].ImgObj.Source[0], out hv_Width, out hv_Height);
                    HOperatorSet.GenRectangle1(out Reg_ImageBorder, 0, 0, hv_Height - 1, hv_Width - 1);

                    bool b_error_Parallel = false; // 平行運算中，是否遇到例外狀況
                    int count_img = listMapItem[0].ImgObj.Source.Count;
                    List<List<Point>> cell_id_MoveIndex = new List<List<Point>>(); // 每一檢測位置之所有Cell座標
                    for (int capture_index_ = 1; capture_index_ <= listMapItem.Count; capture_index_++) // 每一檢測位置
                    {
                        List<Point> cell_id = new List<Point>();
                        cell_id_MoveIndex.Add(cell_id);
                    }
                    //List<HObject> CellImgs_MoveIndex = new List<HObject>(); // 每一檢測位置之所有Cell影像
                    HObject[] CellImgs_MoveIndex = new HObject[listMapItem.Count]; // 每一檢測位置之所有Cell影像
                    HObject[] Array_CellRegs_MoveIndex = new HObject[listMapItem.Count]; // 每一檢測位置之所有Cell Region
                    Parallel.For(1, listMapItem.Count + 1, (capture_index_, loopState) => // 每一檢測位置
                    {
                        try
                        {
                            // 計算此位置所有Cell
                            HTuple area_, row_, col_;
                            HOperatorSet.AreaCenter(CellReg_MoveIndex_FS[capture_index_ - 1], out area_, out row_, out col_);

                            // 計算此位置所有Cell之座標
                            List<Point> cell_id = new List<Point>();
                            this.MultiPos_2_cellID_MoveIndex(capture_index_, clsStaticTool.HTuple_2_ListDouble(row_), clsStaticTool.HTuple_2_ListDouble(col_), out cell_id);
                            cell_id_MoveIndex[capture_index_ - 1] = cell_id;

                            // 消除 Cell Region 超出影像範圍部分
                            HObject CellReg_MoveIndex;
                            HOperatorSet.Intersection(CellReg_MoveIndex_FS[capture_index_ - 1], Reg_ImageBorder, out CellReg_MoveIndex);
                            Array_CellRegs_MoveIndex[capture_index_ - 1] = CellReg_MoveIndex;

                            // 計算此位置所有Cell之所有影像
                            HTuple hv_Value_R1 = new HTuple(), hv_Value_C1 = new HTuple(), hv_Value_R2 = new HTuple(), hv_Value_C2 = new HTuple();
                            HOperatorSet.RegionFeatures(CellReg_MoveIndex, "row1", out hv_Value_R1);
                            HOperatorSet.RegionFeatures(CellReg_MoveIndex, "column1", out hv_Value_C1);
                            HOperatorSet.RegionFeatures(CellReg_MoveIndex, "row2", out hv_Value_R2);
                            HOperatorSet.RegionFeatures(CellReg_MoveIndex, "column2", out hv_Value_C2);
                            HObject Imgs_Cells;
                            try
                            {
                                // 一次1張影像做 CropRectangle1()
                                HOperatorSet.GenEmptyObj(out Imgs_Cells);
                                for (int i = 1; i <= count_img; i++)
                                {
                                    HObject temp;
                                    HOperatorSet.CropRectangle1(listMapItem[capture_index_ - 1].ImgObj.Source[i - 1], out temp, hv_Value_R1, hv_Value_C1, hv_Value_R2, hv_Value_C2);
                                    {
                                        HObject ExpTmpOutVar_0;
                                        HOperatorSet.ConcatObj(Imgs_Cells, temp, out ExpTmpOutVar_0);
                                        Imgs_Cells.Dispose();
                                        Imgs_Cells = ExpTmpOutVar_0;
                                    }
                                    temp.Dispose();
                                }

                                CellImgs_MoveIndex[capture_index_ - 1] = Imgs_Cells;
                            }
                            catch (Exception ex)
                            {
                                // 停止並退出Parallel.For
                                loopState.Stop();
                                b_error_Parallel = true;
                                return;
                            }

                        }
                        catch (Exception ex)
                        {
                            // 停止並退出Parallel.For
                            loopState.Stop();
                            b_error_Parallel = true;
                            return;
                        }
                    });

                    if (b_error_Parallel) // 判斷平行運算中，是否遇到例外狀況
                    {
                        clsStaticTool.DisposeAll(CellImgs_MoveIndex);
                        return false;
                    }

                    for (int capture_index_ = 1; capture_index_ <= listMapItem.Count; capture_index_++) // 每一檢測位置
                    {
                        int count_cell = cell_id_MoveIndex[capture_index_ - 1].Count;
                        for (int i = 0; i < count_cell; i++) // 每一顆Cell
                        {
                            Point pt = cell_id_MoveIndex[capture_index_ - 1][i];
                            if (this.Dictionary_CellImage.ContainsKey(pt) == false)
                            {
                                if (b_AllCells == false && ListPt_specificCell.Contains(pt) == false)
                                    continue;

                                // 計算此座標之所有Cell影像
                                List<HObject> ListImg = new List<HObject>();
                                for (int ii = 0; ii < count_img; ii++)
                                    ListImg.Add(CellImgs_MoveIndex[capture_index_ - 1].SelectObj(i + 1 + ii * count_cell));

                                // 計算此Cell之瑕疵 Region
                                bool b_Defect = false;
                                HObject DefectReg;
                                if (this.all_defect_id_.Contains(pt))
                                {
                                    b_Defect = true;
                                    HObject cellReg = Array_CellRegs_MoveIndex[capture_index_ - 1].SelectObj(i + 1);
                                    HObject DefectReg_inter;
                                    HOperatorSet.Intersection(DefReg_MoveIndex_FS[capture_index_ - 1], cellReg, out DefectReg_inter);
                                    // 平移到原點
                                    HTuple hv_R1 = new HTuple(), hv_C1 = new HTuple();
                                    HOperatorSet.RegionFeatures(cellReg, "row1", out hv_R1);
                                    HOperatorSet.RegionFeatures(cellReg, "column1", out hv_C1);
                                    HOperatorSet.MoveRegion(DefectReg_inter, out DefectReg, -hv_R1, -hv_C1);
                                    DefectReg_inter.Dispose();
                                    cellReg.Dispose();
                                }
                                else
                                    HOperatorSet.GenEmptyRegion(out DefectReg);

                                try
                                {
                                    List<string> ListStr = new List<string>();
                                    this.Dictionary_CellImage.Add(pt, clsHalconRegion.clsHalconRegion_Constructor_Regs(ListImg, ListStr, ListStr, pt, DefectReg, b_Defect));
                                }
                                catch (Exception ex) // 已存在此座標
                                { }
                            }
                        }
                    }

                    clsStaticTool.DisposeAll(CellImgs_MoveIndex);
                    clsStaticTool.DisposeAll(Array_CellRegs_MoveIndex);
                    Extension.HObjectMedthods.ReleaseHObject(ref Reg_ImageBorder);

                    #endregion
                }
                else if (Method == 4)
                {
                    #region Method 2

                    HObject cells;
                    if (b_AllCells)
                        cells = this.cellmap_affine_.Clone();
                    else
                    {
                        if (ListPt_specificCell == null || ListPt_specificCell.Count == 0)
                            return true;
                        HTuple cell_ids = this.ListCellID_2_HTuple(ListPt_specificCell);
                        HOperatorSet.SelectObj(this.cellmap_affine_, out cells, cell_ids);
                    }

                    // 計算各Cell中心座標
                    HTuple x = new HTuple(), y = new HTuple();
                    HOperatorSet.RegionFeatures(cells, "column", out x);
                    HOperatorSet.RegionFeatures(cells, "row", out y);
                    cells.Dispose();
                    
                    //List<clsHalconRegion> List_clsHalconRegion = new List<clsHalconRegion>();
                    clsHalconRegion[] Array_clsHalconRegion = new clsHalconRegion[x.Length]; // Note: clsHalconRegion[] 比 List<clsHalconRegion> 穩定多了!!!
                    bool b_error_Parallel = false; // 平行運算中，是否遇到例外狀況
                    for (int i = 0; i < x.Length; i++) // For 每一顆Cell
                    {
                        try
                        {
                            List<int> ListMoveIndex = new List<int>();
                            List<HObject> ListCellReg_MoveIndex = new List<HObject>();
                            if (!this.posBigMapCellCenter_2_MoveIndex(new PointF((float)(x[i].D), (float)(y[i].D)), out ListMoveIndex, out ListCellReg_MoveIndex))
                            {
                                // 停止並退出
                                b_error_Parallel = true;
                                break;
                            }

                            if (ListMoveIndex.Count > 0)
                            {
                                // 計算此Cell之所有影像
                                List<HObject> ListImg = new List<HObject>();
                                int count_img = listMapItem[ListMoveIndex[0] - 1].ImgObj.Source.Count;
                                for (int ii = 0; ii < count_img; ii++)
                                {
                                    HObject ReduceImg, CropImg;
                                    HOperatorSet.ReduceDomain(listMapItem[ListMoveIndex[0] - 1].ImgObj.Source[ii], ListCellReg_MoveIndex[0], out ReduceImg);
                                    HOperatorSet.CropDomain(ReduceImg, out CropImg);
                                    ReduceImg.Dispose();
                                    ListImg.Add(CropImg);
                                }

                                // 計算座標
                                HTuple cell_id;
                                List<Point> ListCellID;
                                this.pos_2_cellID((int)(y[i].D), (int)(x[i].D), out cell_id, out ListCellID);
                                Point cellID = ListCellID[0];

                                // 計算此Cell之瑕疵 Region
                                bool b_Defect = false;
                                HObject DefectReg;
                                if (this.all_defect_id_.Contains(cellID))
                                {
                                    b_Defect = true;
                                    HObject DefectReg_inter;
                                    HOperatorSet.Intersection(DefReg_MoveIndex_FS[ListMoveIndex[0] - 1], ListCellReg_MoveIndex[0], out DefectReg_inter);
                                    // 平移到原點
                                    HTuple hv_R1 = new HTuple(), hv_C1 = new HTuple();
                                    HOperatorSet.RegionFeatures(ListCellReg_MoveIndex[0], "row1", out hv_R1);
                                    HOperatorSet.RegionFeatures(ListCellReg_MoveIndex[0], "column1", out hv_C1);
                                    HOperatorSet.MoveRegion(DefectReg_inter, out DefectReg, -hv_R1, -hv_C1);
                                    DefectReg_inter.Dispose();
                                    clsStaticTool.DisposeAll(ListCellReg_MoveIndex);
                                }
                                else
                                    HOperatorSet.GenEmptyRegion(out DefectReg);

                                // 存入 List_clsHalconRegion
                                try
                                {
                                    List<string> ListStr = new List<string>();
                                    //List_clsHalconRegion.Add(clsHalconRegion.clsHalconRegion_Constructor_Regs(ListImg, ListStr, ListStr, cellID, DefectReg, b_Defect));
                                    Array_clsHalconRegion[i] = clsHalconRegion.clsHalconRegion_Constructor_Regs(ListImg, ListStr, ListStr, cellID, DefectReg, b_Defect);
                                }
                                catch (Exception ex)
                                { }

                            }
                            else // Cell瑕疵: AOI未成功找出 Cell Region
                                Array_clsHalconRegion[i] = null;
                        }
                        catch (Exception ex)
                        {
                            // 停止並退出
                            b_error_Parallel = true;
                            break;
                        }
                    }

                    if (b_error_Parallel) // 判斷平行運算中，是否遇到例外狀況
                        return false;

                    // 存入 Dictionary_CellImage
                    //foreach (clsHalconRegion cls in List_clsHalconRegion)
                    //foreach (clsHalconRegion cls in Array_clsHalconRegion)
                    for (int i = 0; i < Array_clsHalconRegion.Length; i++)
                    {
                        if (Array_clsHalconRegion[i] == null)
                            continue;
                        clsHalconRegion cls = Array_clsHalconRegion[i];
                        Point pt = cls.cellID;
                        if (this.Dictionary_CellImage.ContainsKey(pt) == false)
                            this.Dictionary_CellImage.Add(pt, cls);
                        else
                            cls.Dispose();
                    }

                    #endregion
                }

                this.Cls_CellImage.B_CellImage_Done = true;
                this.Cls_CellImage.CountImg_1Cell = listMapItem[0].ImgObj.Source.Count;
                b_status_ = true;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }

            return b_status_;
        }

        #endregion

    }

    [Serializable]
    public class LocateMethodRecipe_New // (20200429) Jeff Revised!
    {
        public LocateMethodRecipe_New() { }

        /// <summary>
        /// 視覺定位
        /// </summary>
        public bool VisualPosEnable { get; set; } = true; // (20191216) MIL Jeff Revised!
        
        /// <summary>
        /// 忽略Cell之id
        /// </summary>
        public List<Point> ListPt_BypassCell { get; set; } = new List<Point>();
    }

    /// <summary>
    /// 【計算瑕疵座標測試】Recipe
    /// </summary>
    [Serializable]
    public class DefectTest_Recipe // (20200429) Jeff Revised!
    {
        public LocateMethod LocateMethodRecipe { get; set; } = new LocateMethod(); // (20200429) Jeff Revised!

        #region 大圖拼接與瑕疵統計相關資訊
        
        /// <summary>
        /// 各影像在大拼接圖上之FOV
        /// </summary>
        public HObject capture_map_ = null;
        
        /// <summary>
        /// 當前基板之所有瑕疵所在cell區域 (原始)
        /// </summary>
        public HObject all_intersection_defect_ = null;

        /// <summary>
        /// 當前基板之所有瑕疵所在cell區域 (原始) (Cell region)
        /// </summary>
        public HObject all_intersection_defect_CellReg = null;

        /// <summary>
        /// 當前基板之所有瑕疵所在cell區域 (人工覆判)
        /// </summary>
        public HObject all_intersection_defect_Recheck = null;
        
        /// <summary>
        /// 當前基板之所有背景瑕疵
        /// </summary>
        public HObject all_BackDefect_ = null;
        /// <summary>
        /// 已扭正(復原)之cell全域區域 (For 第一張檢測影像)
        /// </summary>
        public HObject cellmap_affine_;
        /// <summary>
        /// 已扭正之標準cell區域 (All Golden Cell Regions) (For 第一張檢測影像扭正)
        /// </summary>
        public HObject cellmap_sortregion_; // (20200429) Jeff Revised!
        
        /// <summary>
        /// 當前基板之所有檢測到之Cell所在cell區域
        /// </summary>
        public HObject all_intersection_Cell_ = null;
        
        /// <summary>
        /// Marks中心位置之region (For 扭正之大圖)
        /// </summary>
        public HObject MarkCenter_BigMap_affine = null;
        /// <summary>
        /// Marks中心位置之region (For 未扭正之原始大圖)
        /// </summary>
        public HObject MarkCenter_BigMap_orig = null;

        /// <summary>
        /// 拼接影像
        /// </summary>
        public HObject tile_image_ = null;
        /// <summary>
        /// 拼接影像之寬度
        /// </summary>
        public int width_tileImage { get; set; } = 9999;
        /// <summary>
        /// 拼接影像之高度
        /// </summary>
        public int height_tileImage { get; set; } = 9999;

        #endregion

        #region Halcon Region

        /// <summary>
        /// 【計算瑕疵座標測試】中所需 Halcon Region
        /// </summary>
        private List<clsHalconRegion> ListRegion_DefectTest { get; set; } = new List<clsHalconRegion>();

        /// <summary>
        /// 每顆瑕疵Cell之影像 & 該Cell之瑕疵 Region (【瑕疵Cell影像】)
        /// </summary>
        [XmlIgnore] // 帶有XmlIgnore，表示序列化時不序列化此屬性
        public Dictionary<Point, clsHalconRegion> Dictionary_DefectCellImage { get; set; } = new Dictionary<Point, clsHalconRegion>();

        /// <summary>
        /// 用於序列化 (Dictionary_DefectCellImage)
        /// </summary>
        public clsHalconRegion[] Array_DefectCellImage // (20190727) Jeff Revised!
        {
            get // 序列化
            {
                List<clsHalconRegion> list = new List<clsHalconRegion>(Dictionary_DefectCellImage.Count);
                foreach (KeyValuePair<Point, clsHalconRegion> pair in Dictionary_DefectCellImage)
                    list.Add(pair.Value);
                return list.ToArray();
            }

            set // 反序列化
            {
                Dictionary_DefectCellImage = new Dictionary<Point, clsHalconRegion>();
                foreach (var v in value)
                    Dictionary_DefectCellImage.Add(v.cellID, v);
            }
        }

        /// <summary>
        /// 每顆Cell之影像 & 該Cell之瑕疵 Region (【所有Cell影像】)
        /// </summary>
        [XmlIgnore] // 帶有XmlIgnore，表示序列化時不序列化此屬性
        public Dictionary<Point, clsHalconRegion> Dictionary_AllCellImage { get; set; } = new Dictionary<Point, clsHalconRegion>();

        /// <summary>
        /// 用於序列化 (Dictionary_AllCellImage)
        /// </summary>
        public clsHalconRegion[] Array_CellImage // (20190727) Jeff Revised!
        {
            get // 序列化
            {
                List<clsHalconRegion> list = new List<clsHalconRegion>(Dictionary_AllCellImage.Count);
                foreach (KeyValuePair<Point, clsHalconRegion> pair in Dictionary_AllCellImage)
                    list.Add(pair.Value);
                return list.ToArray();
            }

            set // 反序列化
            {
                Dictionary_AllCellImage = new Dictionary<Point, clsHalconRegion>();
                foreach (var v in value)
                    Dictionary_AllCellImage.Add(v.cellID, v);
            }
        }

        #endregion

        #region 瑕疵分類 & 人工覆判

        /// <summary>
        /// 瑕疵分類
        /// </summary>
        [XmlIgnore] // 帶有XmlIgnore，表示序列化時不序列化此屬性
        public Dictionary<string, DefectsResult> DefectsClassify { get; set; } = new Dictionary<string, DefectsResult>();

        /// <summary>
        /// 用於序列化 (DefectsClassify)
        /// </summary>
        public DefectsResult[] Array_DefectsClassify // (20190727) Jeff Revised!
        {
            get // 序列化
            {
                List<DefectsResult> list = new List<DefectsResult>(this.DefectsClassify.Count);
                foreach (KeyValuePair<string, DefectsResult> pair in this.DefectsClassify)
                    list.Add(pair.Value);
                return list.ToArray();
            }

            set // 反序列化
            {
                this.DefectsClassify = new Dictionary<string, DefectsResult>();
                foreach (var v in value)
                    this.DefectsClassify.Add(v.Name, v);
            }
        }

        /// <summary>
        /// 各類型瑕疵所對應之Priority
        /// </summary>
        [XmlIgnore] // 帶有XmlIgnore，表示序列化時不序列化此屬性
        public Dictionary<int, string> Defects_priority { get; set; } = new Dictionary<int, string>();

        public string[] Array_Defects_priority // (20190727) Jeff Revised!
        {
            get // 序列化
            {
                List<string> list = new List<string>(Defects_priority.Count);
                foreach (KeyValuePair<int, string> pair in Defects_priority)
                    list.Add(pair.Value);
                return list.ToArray();
            }

            set // 反序列化
            {
                Defects_priority = new Dictionary<int, string>();
                foreach (var v in value)
                {
                    if (DefectsClassify.ContainsKey(v))
                        Defects_priority.Add(DefectsClassify[v].Priority, v);
                }
            }
        }
        
        /// <summary>
        /// 當前基板之所有已標註cell region (人工覆判)
        /// </summary>
        public HObject all_intersection_cellLabelled_Recheck = null;

        #endregion
        
        /// <summary>
        /// 目錄資料夾
        /// </summary>
        public string Directory_ { get; set; } = "D:\\DSI\\DefectTest\\";
        
        public DefectTest_Recipe() { }

        #region 方法

        /// <summary>
        /// 更新 ListRegion_DefectTest
        /// </summary>
        /// <param name="directory_">目錄資料夾</param>
        /// <param name="b_HalconReg2List"></param>
        public void Update_ListRegion_DefectTest(string directory_ = null, bool b_HalconReg2List = true)
        {
            if (directory_ != null)
                this.Directory_ = directory_;

            if (b_HalconReg2List)
            {
                //Dispose_ListRegion_DefectTest(); // (20200429) Jeff Revised!
                this.ListRegion_DefectTest.Clear();

                #region 加入所有 HObject

                this.ListRegion_DefectTest.Add(Set_clsHalconRegion_1Reg(ref this.capture_map_, "capture_map_", this.Directory_ + "capture_map_"));
                this.ListRegion_DefectTest.Add(Set_clsHalconRegion_1Reg(ref this.all_intersection_defect_, "all_intersection_defect_", this.Directory_ + "all_intersection_defect_"));
                this.ListRegion_DefectTest.Add(Set_clsHalconRegion_1Reg(ref this.all_intersection_defect_CellReg, "all_intersection_defect_CellReg", this.Directory_ + "all_intersection_defect_CellReg"));
                this.ListRegion_DefectTest.Add(Set_clsHalconRegion_1Reg(ref this.all_intersection_defect_Recheck, "all_intersection_defect_Recheck", this.Directory_ + "all_intersection_defect_Recheck"));
                this.ListRegion_DefectTest.Add(Set_clsHalconRegion_1Reg(ref this.all_BackDefect_, "all_BackDefect_", this.Directory_ + "all_BackDefect_"));
                this.ListRegion_DefectTest.Add(Set_clsHalconRegion_1Reg(ref this.cellmap_affine_, "cellmap_affine_", this.Directory_ + "cellmap_affine_"));
                this.ListRegion_DefectTest.Add(Set_clsHalconRegion_1Reg(ref this.cellmap_sortregion_, "cellmap_sortregion_", this.Directory_ + "cellmap_sortregion_")); // (20200429) Jeff Revised!
                this.ListRegion_DefectTest.Add(Set_clsHalconRegion_1Reg(ref this.all_intersection_Cell_, "all_intersection_Cell_", this.Directory_ + "all_intersection_Cell_"));
                this.ListRegion_DefectTest.Add(Set_clsHalconRegion_1Reg(ref this.MarkCenter_BigMap_affine, "MarkCenter_BigMap_affine", this.Directory_ + "MarkCenter_BigMap_affine"));
                this.ListRegion_DefectTest.Add(Set_clsHalconRegion_1Reg(ref this.MarkCenter_BigMap_orig, "MarkCenter_BigMap_orig", this.Directory_ + "MarkCenter_BigMap_orig"));
                //this.ListRegion_DefectTest.Add(Set_clsHalconRegion_1Reg(ref this.tile_image_, "tile_image_", this.Directory_ + "tile_image_"));
                this.ListRegion_DefectTest.Add(Set_clsHalconRegion_1Reg(ref this.all_intersection_cellLabelled_Recheck, "all_intersection_cellLabelled_Recheck", this.Directory_ + "all_intersection_cellLabelled_Recheck"));


                #endregion
            }
            else // (20200429) Jeff Revised!
            {
                this.capture_map_ = this.ListRegion_DefectTest[0].ListRegion[0];
                this.all_intersection_defect_ = this.ListRegion_DefectTest[1].ListRegion[0];
                this.all_intersection_defect_CellReg = this.ListRegion_DefectTest[2].ListRegion[0];
                this.all_intersection_defect_Recheck = this.ListRegion_DefectTest[3].ListRegion[0];
                this.all_BackDefect_ = this.ListRegion_DefectTest[4].ListRegion[0];
                this.cellmap_affine_ = this.ListRegion_DefectTest[5].ListRegion[0];
                this.cellmap_sortregion_ = this.ListRegion_DefectTest[6].ListRegion[0]; // (20200429) Jeff Revised!
                this.all_intersection_Cell_ = this.ListRegion_DefectTest[7].ListRegion[0];
                this.MarkCenter_BigMap_affine = this.ListRegion_DefectTest[8].ListRegion[0];
                this.MarkCenter_BigMap_orig = this.ListRegion_DefectTest[9].ListRegion[0];
                this.all_intersection_cellLabelled_Recheck = this.ListRegion_DefectTest[10].ListRegion[0];
            }
            
        }

        /// <summary>
        /// 將單一 Region 存放到 clsHalconRegion 物件中
        /// </summary>
        /// <param name="reg"></param>
        /// <param name="Name"></param>
        /// <param name="Path"></param>
        /// <returns></returns>
        private clsHalconRegion Set_clsHalconRegion_1Reg(ref HObject reg, string Name, string Path)
        {
            clsHalconRegion cls = new clsHalconRegion();
            cls.ListRegion.Add(reg);
            cls.ListString_Name.Add(Name);
            cls.ListString_Path.Add(Path);
            return cls;
        }

        /// <summary>
        /// 指定座標，將 Dictionary_AllCellImage 更新到 Dictionary_DefectCellImage
        /// </summary>
        /// <param name="defect_id_"></param>
        public void Update_Dictionary_DefectCellImage_from_AllCellImage(List<Point> defect_id_)
        {
            DisposeHObject_Dictionary_Point_clsHalconRegion(Dictionary_DefectCellImage);
            foreach (Point pt in defect_id_)
            {
                if (Dictionary_AllCellImage.ContainsKey(pt))
                {
                    //Dictionary_DefectCellImage.Add(pt, Dictionary_AllCellImage[pt]);
                    Dictionary_DefectCellImage.Add(pt, clsStaticTool.Clone<clsHalconRegion>(Dictionary_AllCellImage[pt]));
                }
            }
        }

        /// <summary>
        /// 釋放記憶體 (ListRegion_DefectTest)
        /// </summary>
        public void Dispose_ListRegion_DefectTest()
        {
            clsStaticTool.DisposeAll(ListRegion_DefectTest);
        }

        /// <summary>
        /// 釋放記憶體 (HObject in Dictionary<Point, clsHalconRegion>)
        /// </summary>
        /// <param name="dict"></param>
        public void DisposeHObject_Dictionary_Point_clsHalconRegion(Dictionary<Point, clsHalconRegion> dict) // (20200429) Jeff Revised!
        {
            foreach (clsHalconRegion cls in dict.Values)
                cls.Dispose(); // (20200429) Jeff Revised!
        }

        /// <summary>
        /// 儲存 影像 & Halcon region
        /// </summary>
        /// <param name="directory_"></param>
        /// <returns></returns>
        public bool SaveHalconFile(string directory_ = null) // (20200429) Jeff Revised!
        {
            bool b_status_ = false;
            if (directory_ != null)
                this.Directory_ = directory_;

            try
            {
                if (!Directory.Exists(this.Directory_))
                {
                    Directory.CreateDirectory(this.Directory_);
                }

                // ListRegion_DefectTest
                foreach (clsHalconRegion cls in ListRegion_DefectTest)
                {
                    if (cls.ListRegion[0] != null)
                    {
                        //HOperatorSet.WriteRegion(cls.ListRegion[0], cls.ListString_Path[0] + ".reg");
                        HOperatorSet.WriteRegion(cls.ListRegion[0], cls.ListString_Path[0] + ".hobj");
                    }
                }

                // 大圖影像
                try
                {
                    HOperatorSet.WriteImage(tile_image_, "tiff", 0, this.Directory_ + "tile_image_");
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.ToString());
                }

                #region Cell影像 & 該Cell之瑕疵 Region

                if (this.LocateMethodRecipe.Cls_CellImage.B_CellImage_Done) // (20200429) Jeff Revised!
                {
                    string fileName = "";
                    Dictionary<Point, clsHalconRegion> Dict = new Dictionary<Point, clsHalconRegion>();
                    if (this.LocateMethodRecipe.Cls_CellImage.Type_CellImage == enu_Type_CellImage.DefectCellImage) //【瑕疵Cell影像】
                    {
                        fileName = "Defect Cell Image";
                        Dict = Dictionary_DefectCellImage;
                    }
                    else if (this.LocateMethodRecipe.Cls_CellImage.Type_CellImage == enu_Type_CellImage.AllCellImage) //【所有Cell影像】
                    {
                        fileName = "Cell Image";
                        Dict = Dictionary_AllCellImage;
                    }

                    if (!Directory.Exists(this.Directory_ + fileName + "\\"))
                        Directory.CreateDirectory(this.Directory_ + fileName + "\\");

                    int Method = 1; // Method: 1 ~ 2, Note: Method 2 速度快2倍多! // (20200907) Jeff Revised! (拿掉平行運算)
                    if (Method == 1)
                    {
                        foreach (clsHalconRegion cls in Dict.Values)
                        {
                            for (int i = 0; i < cls.ListRegion.Count; i++)
                            {
                                if (cls.ListRegion[i] != null)
                                {
                                    //HOperatorSet.WriteImage(cls.ListRegion[i], "tiff", 0, cls.ListString_Path[i]);
                                    HOperatorSet.WriteImage(cls.ListRegion[i], "tiff", 0, this.Directory_ + fileName + "\\" + "(" + cls.cellID.X + ", " + cls.cellID.Y + ")-" + i);
                                }
                            }
                        }
                    }
                    else if (Method == 2) // 平行運算 (Parallel Programming)
                    {
                        bool b_error_Parallel = false; // 平行運算中，是否遇到例外狀況
                        Parallel.ForEach(Dict.Values, (cls, loopState) =>
                        {
                            try
                            {
                                for (int i = 0; i < cls.ListRegion.Count; i++)
                                {
                                    if (cls.ListRegion[i] != null)
                                    {
                                        //HOperatorSet.WriteImage(cls.ListRegion[i], "tiff", 0, cls.ListString_Path[i]);
                                        HOperatorSet.WriteImage(cls.ListRegion[i], "tiff", 0, this.Directory_ + fileName + "\\" + "(" + cls.cellID.X + ", " + cls.cellID.Y + ")-" + i);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                // 停止並退出Parallel.ForEach
                                loopState.Stop();
                                b_error_Parallel = true;
                                return;
                            }
                        });

                        if (b_error_Parallel) // 判斷平行運算中，是否遇到例外狀況
                            return false;
                    }

                    // 該Cell之瑕疵 Region // (20200429) Jeff Revised!
                    HObject concatHObject;
                    HOperatorSet.GenEmptyObj(out concatHObject);
                    int count0 = 0, count1 = 0; // For debug!
                    foreach (clsHalconRegion cls in Dict.Values)
                    {
                        if (cls.B_Defect)
                        {
                            HObject reg;
                            if (cls.DefectReg == null || cls.DefectReg.CountObj() == 0)
                                HOperatorSet.GenEmptyRegion(out reg); // Note: GenEmptyRegion 和 GenEmptyObj 不同，前者 ConcatObj 後數量才會加1
                            else
                                HOperatorSet.Union1(cls.DefectReg, out reg);
                            
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(concatHObject, reg, out ExpTmpOutVar_0);
                            concatHObject.Dispose();
                            concatHObject = ExpTmpOutVar_0;
                            count0 = count1;
                            count1 = concatHObject.CountObj(); // For debug!
                            if (count0 == count1) // For debug!
                                ;
                        }
                        else // For debug!
                            ;
                    }
                    int count2 = concatHObject.CountObj(); // For debug!
                    HOperatorSet.WriteRegion(concatHObject, this.Directory_ + fileName + "\\" + "DefectReg" + ".hobj");
                    concatHObject.Dispose();
                }

                #endregion

                #region DefectsClassify

                foreach (string name in DefectsClassify.Keys)
                {
                    string path = this.Directory_ + name + "\\";
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    if (DefectsClassify[name].all_intersection_defect_Origin != null)
                        HOperatorSet.WriteRegion(DefectsClassify[name].all_intersection_defect_Origin, path + "all_intersection_defect_Origin" + ".hobj");

                    if (DefectsClassify[name].all_intersection_defect_Origin_CellReg != null)
                        HOperatorSet.WriteRegion(DefectsClassify[name].all_intersection_defect_Origin_CellReg, path + "all_intersection_defect_Origin_CellReg" + ".hobj");

                    if (DefectsClassify[name].all_intersection_defect_Priority != null)
                        HOperatorSet.WriteRegion(DefectsClassify[name].all_intersection_defect_Priority, path + "all_intersection_defect_Priority" + ".hobj");

                    if (DefectsClassify[name].all_intersection_defect_Priority_CellReg != null)
                        HOperatorSet.WriteRegion(DefectsClassify[name].all_intersection_defect_Priority_CellReg, path + "all_intersection_defect_Priority_CellReg" + ".hobj");

                    if (DefectsClassify[name].all_intersection_defect_Recheck != null)
                        HOperatorSet.WriteRegion(DefectsClassify[name].all_intersection_defect_Recheck, path + "all_intersection_defect_Recheck" + ".hobj");
                }

                #endregion

                b_status_ = true;
            }
            catch (Exception ex)
            { }

            return b_status_;
        }

        /// <summary>
        /// 載入 影像 & Halcon region
        /// </summary>
        /// <param name="directory_"></param>
        /// <returns></returns>
        public bool ReadHalconFile(string directory_ = null) // (20200429) Jeff Revised!
        {
            bool b_status_ = false;
            if (directory_ != null)
                this.Directory_ = directory_;

            try
            {
                // 先產生一張影像，之後才能順利載入region (20190729) Jeff Revised!
                {
                    HObject img;
                    HOperatorSet.GenImageConst(out img, "byte", width_tileImage, height_tileImage);
                    img.Dispose();
                }

                // ListRegion_DefectTest
                Update_ListRegion_DefectTest();
                foreach (clsHalconRegion cls in ListRegion_DefectTest)
                {
                    if (File.Exists(cls.ListString_Path[0] + ".hobj"))
                    {
                        HObject reg;
                        HOperatorSet.ReadRegion(out reg, cls.ListString_Path[0] + ".hobj");
                        cls.ListRegion[0] = reg;
                    }
                }
                Update_ListRegion_DefectTest(null, false);

                // 大圖影像
                Extension.HObjectMedthods.ReleaseHObject(ref tile_image_);
                if (File.Exists(this.Directory_ + "tile_image_.tif"))
                    HOperatorSet.ReadImage(out tile_image_, this.Directory_ + "tile_image_");
                else
                    HOperatorSet.GenEmptyObj(out tile_image_);

                #region Cell影像 & 該Cell之瑕疵 Region

                if (this.LocateMethodRecipe.Cls_CellImage.B_CellImage_Done) // (20200429) Jeff Revised!
                {
                    string fileName = "";
                    Dictionary<Point, clsHalconRegion> Dict = new Dictionary<Point, clsHalconRegion>();
                    if (this.LocateMethodRecipe.Cls_CellImage.Type_CellImage == enu_Type_CellImage.DefectCellImage) //【瑕疵Cell影像】
                    {
                        fileName = "Defect Cell Image";
                        Dict = Dictionary_DefectCellImage;
                    }
                    else if (this.LocateMethodRecipe.Cls_CellImage.Type_CellImage == enu_Type_CellImage.AllCellImage) //【所有Cell影像】
                    {
                        fileName = "Cell Image";
                        Dict = Dictionary_AllCellImage;
                    }

                    if (Directory.Exists(this.Directory_ + fileName + "\\"))
                    {
                        int Method = 1; // Method: 1 ~ 2, Note: Method 2 速度快2倍多! // (20200907) Jeff Revised! (拿掉平行運算)
                        if (Method == 1)
                        {
                            foreach (clsHalconRegion cls in Dict.Values)
                            {
                                for (int i = 0; i < cls.ListRegion.Count; i++)
                                {
                                    string path_cellImg = this.Directory_ + fileName + "\\" + "(" + cls.cellID.X + ", " + cls.cellID.Y + ")-" + i + ".tif";
                                    //if (File.Exists(cls.ListString_Path[i]))
                                    if (File.Exists(path_cellImg))
                                    {
                                        HObject img;
                                        HOperatorSet.ReadImage(out img, path_cellImg);
                                        cls.ListRegion[i] = img;
                                    }
                                }
                            }
                        }
                        else if (Method == 2) // 平行運算 (Parallel Programming)
                        {
                            bool b_error_Parallel = false; // 平行運算中，是否遇到例外狀況
                            Parallel.ForEach(Dict.Values, (cls, loopState) =>
                            {
                                try
                                {
                                    for (int i = 0; i < cls.ListRegion.Count; i++)
                                    {
                                        string path_cellImg = this.Directory_ + fileName + "\\" + "(" + cls.cellID.X + ", " + cls.cellID.Y + ")-" + i + ".tif";
                                        //if (File.Exists(cls.ListString_Path[i]))
                                        if (File.Exists(path_cellImg))
                                        {
                                            HObject img;
                                            HOperatorSet.ReadImage(out img, path_cellImg);
                                            cls.ListRegion[i] = img;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    // 停止並退出Parallel.ForEach
                                    loopState.Stop();
                                    b_error_Parallel = true;
                                    return;
                                }
                            });

                            if (b_error_Parallel) // 判斷平行運算中，是否遇到例外狀況
                                return false;
                        }

                        // 該Cell之瑕疵 Region // (20200429) Jeff Revised!
                        if (File.Exists(this.Directory_ + fileName + "\\" + "DefectReg" + ".hobj"))
                        {
                            HObject concatHObject;
                            HOperatorSet.ReadRegion(out concatHObject, this.Directory_ + fileName + "\\" + "DefectReg" + ".hobj");
                            int count = concatHObject.CountObj(); // For debug!
                            int i = 1;
                            foreach (clsHalconRegion cls in Dict.Values)
                            {
                                if (cls.B_Defect)
                                    cls.DefectReg = concatHObject.SelectObj(i++); // i++: 後加
                                else // 不能讓Region為null，否則後續 DeepClone() 會有例外狀況!!!
                                    HOperatorSet.GenEmptyRegion(out cls.DefectReg);
                            }
                            concatHObject.Dispose();
                        }
                    }

                }

                #endregion

                #region DefectsClassify

                foreach (string name in DefectsClassify.Keys)
                {
                    string path = this.Directory_ + name + "\\";
                    if (Directory.Exists(path))
                    {
                        DefectsResult dr = this.DefectsClassify[name];
                        if (File.Exists(path + "all_intersection_defect_Origin" + ".hobj"))
                            HOperatorSet.ReadRegion(out dr.all_intersection_defect_Origin, path + "all_intersection_defect_Origin" + ".hobj");

                        if (File.Exists(path + "all_intersection_defect_Origin_CellReg" + ".hobj"))
                            HOperatorSet.ReadRegion(out dr.all_intersection_defect_Origin_CellReg, path + "all_intersection_defect_Origin_CellReg" + ".hobj");

                        if (File.Exists(path + "all_intersection_defect_Priority" + ".hobj"))
                            HOperatorSet.ReadRegion(out dr.all_intersection_defect_Priority, path + "all_intersection_defect_Priority" + ".hobj");

                        if (File.Exists(path + "all_intersection_defect_Priority_CellReg" + ".hobj"))
                            HOperatorSet.ReadRegion(out dr.all_intersection_defect_Priority_CellReg, path + "all_intersection_defect_Priority_CellReg" + ".hobj");

                        if (File.Exists(path + "all_intersection_defect_Recheck" + ".hobj"))
                            HOperatorSet.ReadRegion(out dr.all_intersection_defect_Recheck, path + "all_intersection_defect_Recheck" + ".hobj");
                    }
                }

                #endregion

                b_status_ = true;
            }
            catch (Exception ex)
            { }

            return b_status_;
        }

        #endregion
    }

    /// <summary>
    /// 儲存Halcon Region or Image
    /// </summary>
    [Serializable]
    public class clsHalconRegion // (20200429) Jeff Revised!
    {
        /// <summary>
        /// Region or 該Cell座標之影像組
        /// </summary>
        public List<HObject> ListRegion { get; set; } = new List<HObject>();

        /// <summary>
        /// 該Cell之瑕疵 Region
        /// </summary>
        public HObject DefectReg = null;

        /// <summary>
        /// 該Cell是否為NG
        /// </summary>
        public bool B_Defect { get; set; } = false;

        /// <summary>
        /// 各影像組之名稱
        /// </summary>
        public List<string> ListString_Name { get; set; } = new List<string>();

        /// <summary>
        /// 各影像組之檔案路徑
        /// </summary>
        public List<string> ListString_Path { get; set; } = new List<string>();

        /// <summary>
        /// Cell座標
        /// </summary>
        public Point cellID { get; set; } = new Point();

        public clsHalconRegion() { }

        /// <summary>
        /// 代替 建構子，初始化 之功能  (靜態方法)
        /// </summary>
        /// <param name="reg"></param>
        /// <param name="Name"></param>
        /// <param name="Path"></param>
        /// <param name="pt"></param>
        /// <param name="defectReg">該Cell之瑕疵 Region</param>
        /// <param name="b_Defect">該Cell是否為NG</param>
        /// <returns></returns>
        public static clsHalconRegion clsHalconRegion_Constructor_1Reg(ref HObject reg, string Name, string Path, Point pt, HObject defectReg = null, bool b_Defect = false) // (20200429) Jeff Revised!
        {
            clsHalconRegion cls = new clsHalconRegion();
            cls.ListRegion.Add(reg);
            cls.ListString_Name.Add(Name);
            cls.ListString_Path.Add(Path);
            cls.cellID = pt;
            cls.DefectReg = defectReg; // (20200429) Jeff Revised!
            cls.B_Defect = b_Defect; // (20200429) Jeff Revised!
            return cls;
        }

        /// <summary>
        /// 代替 建構子，初始化 之功能  (靜態方法)
        /// </summary>
        /// <param name="listRegion"></param>
        /// <param name="listString_Name"></param>
        /// <param name="listString_Path"></param>
        /// <param name="pt"></param>
        /// <param name="defectReg">該Cell之瑕疵 Region</param>
        /// <param name="b_Defect">該Cell是否為NG</param>
        /// <returns></returns>
        public static clsHalconRegion clsHalconRegion_Constructor_Regs(List<HObject> listRegion, List<string> listString_Name, List<string> listString_Path, Point pt, HObject defectReg = null, bool b_Defect = false) // (20200429) Jeff Revised!
        {
            clsHalconRegion cls = new clsHalconRegion();
            cls.ListRegion.AddRange(listRegion);
            cls.ListString_Name.AddRange(listString_Name);
            cls.ListString_Path.AddRange(listString_Path);
            cls.cellID = pt;
            cls.DefectReg = defectReg; // (20200429) Jeff Revised!
            cls.B_Defect = b_Defect; // (20200429) Jeff Revised!
            return cls;
        }
        
        /// <summary>
        /// 釋放記憶體
        /// </summary>
        public void Dispose() // (20200429) Jeff Revised!
        {
            clsStaticTool.DisposeAll(this.ListRegion);
            Extension.HObjectMedthods.ReleaseHObject(ref this.DefectReg);
        }
    }

    /// <summary>
    /// 儲存/載入之Cell影像類型
    /// </summary>
    public enum enu_Type_CellImage // (20190803) Jeff Revised!
    {
        /// <summary>
        /// 所有Cell影像
        /// </summary>
        AllCellImage,
        /// <summary>
        /// 瑕疵Cell影像
        /// </summary>
        DefectCellImage
    }

    /// <summary>
    /// Cell影像 資訊
    /// </summary>
    [Serializable]
    public class cls_CellImage // (20200429) Jeff Revised!
    {
        /// <summary>
        /// Cell影像類型
        /// </summary>
        public enu_Type_CellImage Type_CellImage { get; set; } = enu_Type_CellImage.AllCellImage;

        /// <summary>
        /// 每顆Cell之影像個數
        /// </summary>
        public int CountImg_1Cell { get; set; } = 1;

        /// <summary>
        /// Cell Image 是否計算完成
        /// </summary>
        public bool B_CellImage_Done { get; set; } = false;

        public cls_CellImage() { }
    }

    /// <summary>
    /// 【雙面統計結果合併】Recipe
    /// </summary>
    [Serializable]
    public class DefectTest_AB_Recipe // (20200429) Jeff Revised!
    {
        public DefectTest_AB_Recipe() { }

        #region 儲存XML之參數

        /// <summary>
        /// 【雙面統計結果合併】Recipe路徑
        /// </summary>
        public string Directory_AB_Recipe { get; set; } = ""; // (20200429) Jeff Revised!

        /// <summary>
        /// 模組名稱
        /// </summary>
        public string productName { get; set; } = ""; // (20200429) Jeff Revised!

        /// <summary>
        /// 工單名稱
        /// </summary>
        public string moduleName { get; set; } = "";

        /// <summary>
        /// 生產料號
        /// </summary>
        public string partNumber { get; set; } = "Default1234";

        /// <summary>
        /// 序號
        /// </summary>
        public string sB_ID { get; set; } = "00000000";

        /// <summary>
        /// A面路徑
        /// </summary>
        public string Path_DefectTest_A { get; set; } = "";

        /// <summary>
        /// B面路徑
        /// </summary>
        public string Path_DefectTest_B { get; set; } = "";

        /// <summary>
        /// 雙面合併是否上下翻轉
        /// </summary>
        public bool B_UpDown_invert { get; set; } = false;

        /// <summary>
        /// 雙面合併是否左右翻轉
        /// </summary>
        public bool B_LeftRight_invert { get; set; } = false;

        /// <summary>
        /// AB面合併之所有瑕疵所在cell座標 (以A面為參考座標) (原始檢測結果)
        /// </summary>
        public List<Point> All_AB_defect_id { get; set; } = new List<Point>();

        /// <summary>
        /// AB面合併之所有瑕疵所在cell座標 (以A面為參考座標) (人工覆判結果)
        /// </summary>
        public List<Point> All_AB_defect_id_Recheck { get; set; } = new List<Point>();

        /// <summary>
        /// 瑕疵分類統計 (AB面瑕疵合併，如果同一座標雙面皆有瑕疵，則以A面之瑕疵種類計算)
        /// </summary>
        public bool B_NG_Classify { get; set; } = true; // (20200429) Jeff Revised!

        /// <summary>
        /// 瑕疵分類統計 (AB面瑕疵合併) (人工覆判結果)
        /// </summary>
        [XmlIgnore] // 帶有XmlIgnore，表示序列化時不序列化此屬性
        public Dictionary<string, DefectsResult> NGClassify_Statistics { get; set; } = new Dictionary<string, DefectsResult>(); // (20200429) Jeff Revised!

        /// <summary>
        /// 用於序列化 (NGClassify_Statistics)
        /// </summary>
        public DefectsResult[] Array_NGClassify_Statistics // (20200429) Jeff Revised!
        {
            get // 序列化
            {
                List<DefectsResult> list = new List<DefectsResult>(this.NGClassify_Statistics.Count);
                foreach (KeyValuePair<string, DefectsResult> pair in this.NGClassify_Statistics)
                    list.Add(pair.Value);
                return list.ToArray();
            }

            set // 反序列化
            {
                this.NGClassify_Statistics = new Dictionary<string, DefectsResult>();
                foreach (var v in value)
                    this.NGClassify_Statistics.Add(v.Name, v);
            }
        }

        #endregion

        #region 其他參數或物件 ([XmlIgnore])

        /// <summary>
        /// LocateMethod (A面)
        /// </summary>
        [XmlIgnore] // 帶有XmlIgnore，表示序列化時不序列化此屬性
        public LocateMethod Locate_method_A { get; set; } = null;

        /// <summary>
        /// LocateMethod (B面)
        /// </summary>
        [XmlIgnore] // 帶有XmlIgnore，表示序列化時不序列化此屬性
        public LocateMethod Locate_method_B { get; set; } = null;

        /// <summary>
        /// LocateMethod (B面轉到A面)
        /// </summary>
        [XmlIgnore] // 帶有XmlIgnore，表示序列化時不序列化此屬性
        public LocateMethod Locate_method_B_2_A { get; set; } = null;

        /// <summary>
        /// B面之所有瑕疵所在cell座標 (以A面為參考座標) (原始檢測結果)
        /// </summary>
        [XmlIgnore] // 帶有XmlIgnore，表示序列化時不序列化此屬性
        public List<Point> All_B_2_A_defect_id { get; set; } = new List<Point>();
        
        /// <summary>
        /// B面之所有未被檢測到之Cell座標 (以A面為參考座標) (原始檢測結果)
        /// </summary>
        [XmlIgnore] // 帶有XmlIgnore，表示序列化時不序列化此屬性
        public List<Point> All_B_2_A_CellDefect_id { get; set; } = new List<Point>();
        
        /// <summary>
        /// AB面合併之所有未被檢測到之Cell座標 (以A面為參考座標) (原始檢測結果)
        /// </summary>
        [XmlIgnore] // 帶有XmlIgnore，表示序列化時不序列化此屬性
        public List<Point> All_AB_CellDefect_id { get; set; } = new List<Point>();

        /// <summary>
        /// B面之所有瑕疵所在cell座標 (以A面為參考座標) (人工覆判結果)
        /// </summary>
        [XmlIgnore] // 帶有XmlIgnore，表示序列化時不序列化此屬性
        public List<Point> All_B_2_A_defect_id_Recheck { get; set; } = new List<Point>();

        /// <summary>
        /// AB面合併之所有已標註cell座標 (以A面為參考座標) (人工覆判)
        /// </summary>
        [XmlIgnore] // 帶有XmlIgnore，表示序列化時不序列化此屬性
        public List<Point> All_AB_cellLabelled_id_Recheck { get; set; } = new List<Point>();

        /// <summary>
        /// B面之所有已標註cell座標 (以A面為參考座標) (人工覆判)
        /// </summary>
        [XmlIgnore] // 帶有XmlIgnore，表示序列化時不序列化此屬性
        public List<Point> All_B_2_A_cellLabelled_id_Recheck { get; set; } = new List<Point>();

        #endregion

        #region 方法

        /// <summary>
        /// 設定B面翻轉方式
        /// </summary>
        /// <param name="b_UpDown_invert_"></param>
        /// <param name="b_LeftRight_invert_"></param>
        public void Set_B_invert(bool b_UpDown_invert_, bool b_LeftRight_invert_)
        {
            this.B_UpDown_invert = b_UpDown_invert_;
            this.B_LeftRight_invert = b_LeftRight_invert_;
        }

        /// <summary>
        /// 設定 A, B面 LocateMethod
        /// </summary>
        /// <param name="locate_method_A_"></param>
        /// <param name="locate_method_B_"></param>
        public void Set_LocateMethod(LocateMethod locate_method_A_, LocateMethod locate_method_B_)
        {
            this.Locate_method_A = locate_method_A_;
            this.Locate_method_B = locate_method_B_;

            /* Note: Dictionary_CellImage加了[NonSerialized]後，就不需以下技巧! */
            // 避免 DeepClone 時，如果 Locate_method_B 資料流太長會失敗，先釋放 Locate_method_B.Dictionary_CellImage 資源，後續再加回去!!! // (20200429) Jeff Revised!
            //Dictionary<Point, clsHalconRegion> temp = clsStaticTool.DeepClone<Dictionary<Point, clsHalconRegion>>(this.Locate_method_B.Dictionary_CellImage);
            //bool b_CellImage_Done = this.Locate_method_B.Cls_CellImage.B_CellImage_Done;
            //this.Locate_method_B.Dispose_Dictionary_CellImage();

            //this.Locate_method_B_2_A = this.Locate_method_B.Clone();
            this.Locate_method_B_2_A = clsStaticTool.DeepClone<LocateMethod>(this.Locate_method_B); // Note: 資料流太長會失敗!

            // 復原 this.Locate_method_B // (20200429) Jeff Revised!
            //this.Locate_method_B.Cls_CellImage.B_CellImage_Done = b_CellImage_Done;
            //this.Locate_method_B.Dictionary_CellImage = temp;

            this.Initialize_Locate_method_B_2_A();
        }

        /// <summary>
        /// 初始化 Locate_method_B_2_A
        /// </summary>
        /// <param name="b_initial_B_2_A"></param>
        /// <returns></returns>
        public bool Initialize_Locate_method_B_2_A(bool b_initial_B_2_A = true) // (20200429) Jeff Revised!
        {
            if (this.Locate_method_A == null || this.Locate_method_B_2_A == null || this.Locate_method_B == null)
                return false;

            if (this.Locate_method_B.Path_DefectTest == "") // (20200429) Jeff Revised!
                return false;

            if (b_initial_B_2_A)
            {
                #region A面相關資訊複製到 Locate_method_B_2_A

                // 更新 cellmap_affine_ & cellmap_sortregion_ & capture_map_ & area_cellmap_affine_
                Extension.HObjectMedthods.ReleaseHObject(ref this.Locate_method_B_2_A.cellmap_affine_);
                if (this.Locate_method_A.cellmap_affine_ != null)
                    this.Locate_method_B_2_A.cellmap_affine_ = this.Locate_method_A.cellmap_affine_.Clone();
                Extension.HObjectMedthods.ReleaseHObject(ref this.Locate_method_B_2_A.cellmap_sortregion_);
                if (this.Locate_method_A.cellmap_sortregion_ != null)
                    this.Locate_method_B_2_A.cellmap_sortregion_ = this.Locate_method_A.cellmap_sortregion_.Clone();
                Extension.HObjectMedthods.ReleaseHObject(ref this.Locate_method_B_2_A.capture_map_);
                if (this.Locate_method_A.capture_map_ != null)
                    this.Locate_method_B_2_A.capture_map_ = this.Locate_method_A.capture_map_.Clone();
                this.Locate_method_B_2_A.area_cellmap_affine_ = this.Locate_method_A.area_cellmap_affine_.Clone();

                // 更新 center_points_x_ & center_points_y_ & tile_rect_
                this.Locate_method_B_2_A.center_points_x_ = this.Locate_method_A.center_points_x_.Clone();
                this.Locate_method_B_2_A.center_points_y_ = this.Locate_method_A.center_points_y_.Clone();
                this.Locate_method_B_2_A.tile_rect_ = this.Locate_method_A.tile_rect_.Clone();

                // 更新 X, Y方向各分區之Cell顆數
                this.Locate_method_B_2_A.cell_x_count_string = this.Locate_method_A.cell_x_count_string;
                this.Locate_method_B_2_A.cell_y_count_string = this.Locate_method_A.cell_y_count_string;
                this.Locate_method_B_2_A.cell_x_count_HTuple = this.Locate_method_A.cell_x_count_HTuple.Clone();
                this.Locate_method_B_2_A.cell_y_count_HTuple = this.Locate_method_A.cell_y_count_HTuple.Clone();

                // 更新 X, Y方向各分區與第一顆Cell距離 (mm)
                this.Locate_method_B_2_A.dist_region_rdx_string = this.Locate_method_A.dist_region_rdx_string;
                this.Locate_method_B_2_A.dist_region_rdy_string = this.Locate_method_A.dist_region_rdy_string;
                this.Locate_method_B_2_A.dist_region_rdx_HTuple = this.Locate_method_A.dist_region_rdx_HTuple.Clone();
                this.Locate_method_B_2_A.dist_region_rdy_HTuple = this.Locate_method_A.dist_region_rdy_HTuple.Clone();

                // 更新 TileImage (20200429) Jeff Revised!
                Extension.HObjectMedthods.ReleaseHObject(ref this.Locate_method_B_2_A.TileImage);
                if (this.Locate_method_A.TileImage != null)
                    this.Locate_method_B_2_A.TileImage = this.Locate_method_A.TileImage.Clone();

                #endregion
            }

            this.Update();

            // 更新 LocateMethodRecipe.ListPt_BypassCell
            List<Point> ListPt_A_0;
            if (!this.Convert_coordinate_B_2_A_List(this.Locate_method_B.LocateMethodRecipe.ListPt_BypassCell, out ListPt_A_0))
                return false;
            this.Locate_method_B_2_A.LocateMethodRecipe.ListPt_BypassCell = ListPt_A_0;

            // 更新 all_defect_id_ & all_intersection_defect_ & all_intersection_defect_CellReg & all_CellDefect_id_
            this.Locate_method_B_2_A.all_defect_id_ = this.All_B_2_A_defect_id.ToList();
            Extension.HObjectMedthods.ReleaseHObject(ref this.Locate_method_B_2_A.all_intersection_defect_);
            this.Locate_method_B_2_A.all_intersection_defect_ = this.Locate_method_B_2_A.Compute_CellReg_ListCellID(this.All_B_2_A_defect_id);
            Extension.HObjectMedthods.ReleaseHObject(ref this.Locate_method_B_2_A.all_intersection_defect_CellReg);
            this.Locate_method_B_2_A.all_intersection_defect_CellReg = this.Locate_method_B_2_A.all_intersection_defect_.Clone();
            this.Locate_method_B_2_A.all_CellDefect_id_ = this.All_B_2_A_CellDefect_id.ToList();
            
            // 更新 DefectsClassify
            if (this.Locate_method_B_2_A.b_Defect_Classify) // 多瑕疵
            {
                #region 更新各類型瑕疵

                int Method = 1; // Method: 1, 2 (平行運算) // (20200907) Jeff Revised! (拿掉平行運算)
                if (Method == 1)
                {
                    #region foreach

                    foreach (KeyValuePair<string, DefectsResult> item in this.Locate_method_B_2_A.DefectsClassify)
                    {
                        item.Value.Release(true, true, false);

                        // 更新 all_defect_id_Origin & all_intersection_defect_Origin & all_intersection_defect_Origin_CellReg
                        List<Point> ListPt_A_1;
                        if (!this.Convert_coordinate_B_2_A_List(this.Locate_method_B.DefectsClassify[item.Key].all_defect_id_Origin, out ListPt_A_1))
                            return false;
                        item.Value.all_defect_id_Origin = ListPt_A_1;
                        item.Value.all_intersection_defect_Origin = this.Locate_method_B_2_A.Compute_CellReg_ListCellID(ListPt_A_1);
                        item.Value.all_intersection_defect_Origin_CellReg = item.Value.all_intersection_defect_Origin.Clone();

                        // 更新 all_defect_id_Priority & all_intersection_defect_Priority & all_intersection_defect_Priority_CellReg
                        if (this.Locate_method_B_2_A.b_Defect_priority) // 瑕疵優先權
                        {
                            List<Point> ListPt_A_2;
                            if (!this.Convert_coordinate_B_2_A_List(this.Locate_method_B.DefectsClassify[item.Key].all_defect_id_Priority, out ListPt_A_2))
                                return false;
                            item.Value.all_defect_id_Priority = ListPt_A_2;
                            item.Value.all_intersection_defect_Priority = this.Locate_method_B_2_A.Compute_CellReg_ListCellID(ListPt_A_2);
                            item.Value.all_intersection_defect_Priority_CellReg = item.Value.all_intersection_defect_Priority.Clone();
                        }
                    }

                    #endregion
                }
                else if (Method == 2)
                {
                    #region  平行運算

                    bool b_error_Parallel = false; // 平行運算中，是否遇到例外狀況
                    Parallel.ForEach(this.Locate_method_B_2_A.DefectsClassify, (item, loopState) =>
                    {
                        try
                        {
                            item.Value.Release(true, true, false);

                            // 更新 all_defect_id_Origin & all_intersection_defect_Origin & all_intersection_defect_Origin_CellReg
                            List<Point> ListPt_A_1;
                            if (!this.Convert_coordinate_B_2_A_List(this.Locate_method_B.DefectsClassify[item.Key].all_defect_id_Origin, out ListPt_A_1))
                            {
                                // 停止並退出Parallel.ForEach
                                loopState.Stop();
                                b_error_Parallel = true;
                                return;
                            }
                            item.Value.all_defect_id_Origin = ListPt_A_1;
                            item.Value.all_intersection_defect_Origin = this.Locate_method_B_2_A.Compute_CellReg_ListCellID(ListPt_A_1);
                            item.Value.all_intersection_defect_Origin_CellReg = item.Value.all_intersection_defect_Origin.Clone();

                            // 更新 all_defect_id_Priority & all_intersection_defect_Priority & all_intersection_defect_Priority_CellReg
                            if (this.Locate_method_B_2_A.b_Defect_priority) // 瑕疵優先權
                            {
                                List<Point> ListPt_A_2;
                                if (!this.Convert_coordinate_B_2_A_List(this.Locate_method_B.DefectsClassify[item.Key].all_defect_id_Priority, out ListPt_A_2))
                                {
                                    // 停止並退出Parallel.ForEach
                                    loopState.Stop();
                                    b_error_Parallel = true;
                                    return;
                                }
                                item.Value.all_defect_id_Priority = ListPt_A_2;
                                item.Value.all_intersection_defect_Priority = this.Locate_method_B_2_A.Compute_CellReg_ListCellID(ListPt_A_2);
                                item.Value.all_intersection_defect_Priority_CellReg = item.Value.all_intersection_defect_Priority.Clone();
                            }
                        }
                        catch (Exception ex)
                        {
                            // 停止並退出Parallel.ForEach
                            loopState.Stop();
                            b_error_Parallel = true;
                            return;
                        }
                    });

                    if (b_error_Parallel) // 判斷平行運算中，是否遇到例外狀況
                        return false;

                    #endregion
                }

                #endregion
            }

            // 更新 Locate_method_B_2_A 之人工覆判
            if (this.Update_Locate_method_B_2_A_Recheck(false) == false)
                return false;
            
            return true;
        }

        /// <summary>
        /// 更新 瑕疵分類統計
        /// </summary>
        /// <returns></returns>
        public bool Update_NGClassify_Statistics() // (20200429) Jeff Revised!
        {
            this.NGClassify_Statistics.Clear();

            if (this.B_NG_Classify == false)
                return false;

            if (this.Locate_method_A == null || this.Locate_method_B_2_A == null || this.Locate_method_B == null)
                return false;

            if (this.Locate_method_B.Path_DefectTest == "")
                return false;

            bool b_status_ = false;
            try
            {
                #region 指定A面所有瑕疵座標

                if (this.Locate_method_A.b_Defect_Classify) // 多瑕疵
                {
                    foreach (KeyValuePair<string, DefectsResult> item in this.Locate_method_A.DefectsClassify)
                        this.NGClassify_Statistics.Add(item.Key, DefectsResult.Get_DefectsResult2(item.Key, item.Value.all_defect_id_Recheck.Count));
                }
                else // 單一瑕疵
                    this.NGClassify_Statistics.Add("NG", DefectsResult.Get_DefectsResult2("NG", this.Locate_method_A.all_defect_id_Recheck.Count));

                #endregion

                #region 指定B面所有未被A面指定之瑕疵座標

                if (this.Locate_method_B_2_A.b_Defect_Classify) // 多瑕疵
                {
                    foreach (KeyValuePair<string, DefectsResult> item in this.Locate_method_B_2_A.DefectsClassify)
                    {
                        List<Point> list_pts = item.Value.all_defect_id_Recheck.ToList();
                        // 消除已被A面指定之瑕疵座標
                        list_pts.RemoveAll(pt => this.Locate_method_A.all_defect_id_Recheck.Contains(pt));

                        if (this.NGClassify_Statistics.ContainsKey(item.Key)) // 瑕疵種類和A面相同
                            this.NGClassify_Statistics[item.Key].Count_NG += list_pts.Count;
                        else
                            this.NGClassify_Statistics.Add(item.Key, DefectsResult.Get_DefectsResult2(item.Key, list_pts.Count));
                    }
                }
                else // 單一瑕疵
                {
                    List<Point> list_pts = this.Locate_method_B_2_A.all_defect_id_Recheck.ToList();
                    // 消除已被A面指定之瑕疵座標
                    list_pts.RemoveAll(pt => this.Locate_method_A.all_defect_id_Recheck.Contains(pt));

                    if (this.NGClassify_Statistics.ContainsKey("NG")) // A面也是單一瑕疵
                        this.NGClassify_Statistics["NG"].Count_NG += list_pts.Count;
                    else
                        this.NGClassify_Statistics.Add("NG", DefectsResult.Get_DefectsResult2("NG", list_pts.Count));
                }

                #endregion

                b_status_ = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
            return b_status_;
        }

        /// <summary>
        /// 座標轉換: B→A (單點)
        /// </summary>
        /// <param name="pt_B"></param>
        /// <param name="pt_A"></param>
        /// <returns></returns>
        public bool Convert_coordinate_B_2_A(Point pt_B, out Point pt_A)
        {
            pt_A = pt_B;
            if (this.Locate_method_A == null || this.Locate_method_B == null)
                return false;

            int Cell_col_count = this.Locate_method_A.cell_col_count_,
                Cell_row_count = this.Locate_method_A.cell_row_count_;

            // y座標: 雙面合併是否上下翻轉
            if (this.B_UpDown_invert)
                pt_A.Y = Cell_row_count - pt_B.Y + 1;

            // x座標: 雙面合併是否左右翻轉
            if (this.B_LeftRight_invert)
                pt_A.X = Cell_col_count - pt_B.X + 1;

            return true;
        }

        /// <summary>
        /// 座標轉換: A→B (單點)
        /// </summary>
        /// <param name="pt_A"></param>
        /// <param name="pt_B"></param>
        /// <returns></returns>
        public bool Convert_coordinate_A_2_B(Point pt_A, out Point pt_B)
        {
            return this.Convert_coordinate_B_2_A(pt_A, out pt_B);
        }

        /// <summary>
        /// 座標轉換: B→A (多點)
        /// </summary>
        /// <param name="ListPt_B"></param>
        /// <param name="ListPt_A"></param>
        /// <returns></returns>
        public bool Convert_coordinate_B_2_A_List(List<Point> ListPt_B, out List<Point> ListPt_A)
        {
            ListPt_A = ListPt_B.ToList();
            for (int i = 0; i < ListPt_B.Count; i++)
            {
                Point pt_A;
                if (this.Convert_coordinate_B_2_A(ListPt_B[i], out pt_A))
                    ListPt_A[i] = pt_A;
                else
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 更新所有座標 (總瑕疵)
        /// </summary>
        /// <returns></returns>
        public bool Update()
        {
            if (!this.Update_All_defect_id())
                return false;
            if (!this.Update_All_CellDefect_id())
                return false;
            if (!this.Update_All_defect_id_Recheck())
                return false;
            if (!this.Update_All_cellLabelled_id_Recheck())
                return false;

            return true;
        }

        /// <summary>
        /// 更新 All_AB_defect_id & All_B_2_A_defect_id
        /// </summary>
        /// <returns></returns>
        public bool Update_All_defect_id()
        {
            if (this.Locate_method_A == null || this.Locate_method_B == null)
                return false;

            this.All_AB_defect_id.Clear();
            this.All_AB_defect_id = this.Locate_method_A.all_defect_id_.ToList();
            this.All_B_2_A_defect_id.Clear();
            foreach (Point pt_B in this.Locate_method_B.all_defect_id_)
            {
                // 座標轉換: B→A
                Point pt_A = new Point();
                if (!this.Convert_coordinate_B_2_A(pt_B, out pt_A))
                    return false;
                this.All_B_2_A_defect_id.Add(pt_A);
                if (!this.All_AB_defect_id.Contains(pt_A))
                    this.All_AB_defect_id.Add(pt_A);
            }

            return true;
        }

        /// <summary>
        /// 更新 All_AB_CellDefect_id & All_B_2_A_CellDefect_id
        /// </summary>
        /// <returns></returns>
        public bool Update_All_CellDefect_id()
        {
            if (this.Locate_method_A == null || this.Locate_method_B == null)
                return false;

            this.All_AB_CellDefect_id.Clear();
            this.All_AB_CellDefect_id = this.Locate_method_A.all_CellDefect_id_.ToList();
            this.All_B_2_A_CellDefect_id.Clear();
            foreach (Point pt_B in this.Locate_method_B.all_CellDefect_id_)
            {
                // 座標轉換: B→A
                Point pt_A = new Point();
                if (!this.Convert_coordinate_B_2_A(pt_B, out pt_A))
                    return false;
                this.All_B_2_A_CellDefect_id.Add(pt_A);
                if (!this.All_AB_CellDefect_id.Contains(pt_A))
                    this.All_AB_CellDefect_id.Add(pt_A);
            }

            return true;
        }

        /// <summary>
        /// 更新 All_AB_defect_id_Recheck & All_B_2_A_defect_id_Recheck
        /// </summary>
        /// <returns></returns>
        public bool Update_All_defect_id_Recheck()
        {
            if (this.Locate_method_A == null || this.Locate_method_B == null)
                return false;

            this.All_AB_defect_id_Recheck.Clear();
            this.All_AB_defect_id_Recheck = this.Locate_method_A.all_defect_id_Recheck.ToList();
            this.All_B_2_A_defect_id_Recheck.Clear();
            foreach (Point pt_B in this.Locate_method_B.all_defect_id_Recheck)
            {
                // 座標轉換: B→A
                Point pt_A = new Point();
                if (!this.Convert_coordinate_B_2_A(pt_B, out pt_A))
                    return false;
                this.All_B_2_A_defect_id_Recheck.Add(pt_A);
                if (!this.All_AB_defect_id_Recheck.Contains(pt_A))
                    this.All_AB_defect_id_Recheck.Add(pt_A);
            }

            return true;
        }

        /// <summary>
        /// 更新 All_AB_cellLabelled_id_Recheck & All_B_2_A_cellLabelled_id_Recheck
        /// </summary>
        /// <returns></returns>
        public bool Update_All_cellLabelled_id_Recheck()
        {
            if (this.Locate_method_A == null || this.Locate_method_B == null)
                return false;

            this.All_AB_cellLabelled_id_Recheck.Clear();
            this.All_AB_cellLabelled_id_Recheck = this.Locate_method_A.all_cellLabelled_id_Recheck.ToList();
            this.All_B_2_A_cellLabelled_id_Recheck.Clear();
            foreach (Point pt_B in this.Locate_method_B.all_cellLabelled_id_Recheck)
            {
                // 座標轉換: B→A
                Point pt_A = new Point();
                if (!this.Convert_coordinate_B_2_A(pt_B, out pt_A))
                    return false;
                this.All_B_2_A_cellLabelled_id_Recheck.Add(pt_A);
                if (!this.All_AB_cellLabelled_id_Recheck.Contains(pt_A))
                    this.All_AB_cellLabelled_id_Recheck.Add(pt_A);
            }

            return true;
        }

        /// <summary>
        /// 更新 Locate_method_B_2_A 之人工覆判 & 雙面合併瑕疵資訊
        /// </summary>
        /// <param name="b_Update"></param>
        /// <param name="locate_method_A_"></param>
        /// <param name="locate_method_B_"></param>
        /// <returns></returns>
        public bool Update_Locate_method_B_2_A_Recheck(bool b_Update = true, LocateMethod locate_method_A_ = null, LocateMethod locate_method_B_ = null) // (20200429) Jeff Revised!
        {
            if (locate_method_A_ != null) // (20200429) Jeff Revised!
                this.Locate_method_A = locate_method_A_;
            if (locate_method_B_ != null) // (20200429) Jeff Revised!
                this.Locate_method_B = locate_method_B_;

            if (this.Locate_method_A == null || this.Locate_method_B_2_A == null || this.Locate_method_B == null)
                return false;

            if (b_Update)
                this.Update();

            if (this.Locate_method_B_2_A.b_Defect_Recheck) // 人工覆判
            {
                // 更新 all_defect_id_Recheck & all_intersection_defect_Recheck
                this.Locate_method_B_2_A.all_defect_id_Recheck = this.All_B_2_A_defect_id_Recheck.ToList();
                Extension.HObjectMedthods.ReleaseHObject(ref this.Locate_method_B_2_A.all_intersection_defect_Recheck);
                this.Locate_method_B_2_A.all_intersection_defect_Recheck = this.Locate_method_B_2_A.Compute_CellReg_ListCellID(this.All_B_2_A_defect_id_Recheck);

                // 更新 all_cellLabelled_id_Recheck & all_intersection_cellLabelled_Recheck
                this.Locate_method_B_2_A.all_cellLabelled_id_Recheck = this.All_B_2_A_cellLabelled_id_Recheck.ToList();
                Extension.HObjectMedthods.ReleaseHObject(ref this.Locate_method_B_2_A.all_intersection_cellLabelled_Recheck);
                this.Locate_method_B_2_A.all_intersection_cellLabelled_Recheck = this.Locate_method_B_2_A.Compute_CellReg_ListCellID(this.All_B_2_A_cellLabelled_id_Recheck);
            }
            
            // 更新 DefectsClassify
            if (this.Locate_method_B_2_A.b_Defect_Classify) // 多瑕疵
            {
                #region 更新各類型瑕疵

                int Method = 1; // Method: 1, 2 (平行運算) // (20200907) Jeff Revised! (拿掉平行運算)
                if (Method == 1)
                {
                    #region foreach

                    foreach (KeyValuePair<string, DefectsResult> item in this.Locate_method_B_2_A.DefectsClassify)
                    {
                        item.Value.Release(false, false, true);

                        // 更新 all_defect_id_Recheck & all_intersection_defect_Recheck
                        if (this.Locate_method_B_2_A.b_Defect_Recheck) // 人工覆判
                        {
                            List<Point> ListPt_A_3;
                            if (!this.Convert_coordinate_B_2_A_List(this.Locate_method_B.DefectsClassify[item.Key].all_defect_id_Recheck, out ListPt_A_3))
                                return false;
                            item.Value.all_defect_id_Recheck = ListPt_A_3;
                            item.Value.all_intersection_defect_Recheck = this.Locate_method_B_2_A.Compute_CellReg_ListCellID(ListPt_A_3);
                        }
                    }

                    #endregion
                }
                else if (Method == 2)
                {
                    #region  平行運算

                    bool b_error_Parallel = false; // 平行運算中，是否遇到例外狀況
                    Parallel.ForEach(this.Locate_method_B_2_A.DefectsClassify, (item, loopState) =>
                    {
                        try
                        {
                            item.Value.Release(false, false, true);

                            // 更新 all_defect_id_Recheck & all_intersection_defect_Recheck
                            if (this.Locate_method_B_2_A.b_Defect_Recheck) // 人工覆判
                            {
                                List<Point> ListPt_A_3;
                                if (!this.Convert_coordinate_B_2_A_List(this.Locate_method_B.DefectsClassify[item.Key].all_defect_id_Recheck, out ListPt_A_3))
                                {
                                    // 停止並退出Parallel.ForEach
                                    loopState.Stop();
                                    b_error_Parallel = true;
                                    return;
                                }
                                item.Value.all_defect_id_Recheck = ListPt_A_3;
                                item.Value.all_intersection_defect_Recheck = this.Locate_method_B_2_A.Compute_CellReg_ListCellID(ListPt_A_3);
                            }
                        }
                        catch (Exception ex)
                        {
                            // 停止並退出Parallel.ForEach
                            loopState.Stop();
                            b_error_Parallel = true;
                            return;
                        }
                    });

                    if (b_error_Parallel) // 判斷平行運算中，是否遇到例外狀況
                        return false;

                    #endregion
                }

                #endregion
            }

            // 更新 瑕疵分類統計
            if (this.B_NG_Classify)
            {
                if (this.Update_NGClassify_Statistics() == false) // (20200429) Jeff Revised!
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 更新 Locate_method_B_2_A 之 DefectsClassify 顏色
        /// </summary>
        /// <param name="locate_method_A_"></param>
        /// <param name="locate_method_B_"></param>
        /// <returns></returns>
        public bool Update_Color_Locate_method_B_2_A(LocateMethod locate_method_A_ = null, LocateMethod locate_method_B_ = null) // (20200429) Jeff Revised!
        {
            if (locate_method_A_ != null) // (20200429) Jeff Revised!
                this.Locate_method_A = locate_method_A_;
            if (locate_method_B_ != null) // (20200429) Jeff Revised!
                this.Locate_method_B = locate_method_B_;

            if (this.Locate_method_A == null || this.Locate_method_B_2_A == null || this.Locate_method_B == null)
                return false;

            // 更新 DefectsClassify 顏色
            if (this.Locate_method_B_2_A.b_Defect_Classify) // 多瑕疵
            {
                foreach (KeyValuePair<string, DefectsResult> item in this.Locate_method_B_2_A.DefectsClassify)
                    item.Value.Str_Color_Halcon = this.Locate_method_B.DefectsClassify[item.Key].Str_Color_Halcon;
            }

            return true;
        }
        
        /// <summary>
        /// 載入Recipe
        /// </summary>
        /// <param name="directory_"></param>
        /// <param name="Recipe"></param>
        /// <returns></returns>
        public static bool Load_DefectTest_AB_Recipe(string directory_, out DefectTest_AB_Recipe Recipe) // (20200429) Jeff Revised!
        {
            Recipe = new DefectTest_AB_Recipe();
            return clsStaticTool.LoadXML(directory_ + "DefectTest_AB_Recipe.xml", out Recipe);
        }

        /// <summary>
        /// 儲存Recipe
        /// </summary>
        /// <param name="directory_"></param>
        /// <returns></returns>
        public bool Save_DefectTest_AB_Recipe(string directory_ = null) // (20200429) Jeff Revised!
        {
            if (directory_ != null)
                this.Directory_AB_Recipe = directory_;

            // 工單相關資訊 (只儲存A面)
            if (this.Locate_method_A != null)
            {
                this.productName = this.Locate_method_A.productName;
                this.moduleName = this.Locate_method_A.moduleName;
                this.partNumber = this.Locate_method_A.partNumber;
                this.sB_ID = this.Locate_method_A.sB_ID;
            }

            return clsStaticTool.SaveXML(this, this.Directory_AB_Recipe + "\\" + "DefectTest_AB_Recipe.xml");
        }

        /// <summary>
        /// 設定所有參數
        /// </summary>
        /// <param name="directory_AB_Recipe"></param>
        /// <param name="productName_"></param>
        /// <param name="moduleName_"></param>
        /// <param name="partNumber_"></param>
        /// <param name="sB_ID_"></param>
        /// <param name="path_DefectTest_A"></param>
        /// <param name="path_DefectTest_B"></param>
        /// <param name="b_UpDown_invert"></param>
        /// <param name="b_LeftRight_invert"></param>
        public void Set_Params(string directory_AB_Recipe, string productName_, string moduleName_, string partNumber_, string sB_ID_, string path_DefectTest_A, string path_DefectTest_B, bool b_UpDown_invert, bool b_LeftRight_invert) // (20200429) Jeff Revised!
        {
            this.Directory_AB_Recipe = directory_AB_Recipe;
            this.productName = productName_;
            this.moduleName = moduleName_;
            this.partNumber = partNumber_;
            this.sB_ID = sB_ID_;
            this.Path_DefectTest_A = path_DefectTest_A;
            this.Path_DefectTest_B = path_DefectTest_B;
            this.B_UpDown_invert = b_UpDown_invert;
            this.B_LeftRight_invert = b_LeftRight_invert;
        }

        /// <summary>
        /// 【雙面統計結果合併】: 更新 ListView_Result_AB
        /// </summary>
        /// <param name="ListView_Result_AB"></param>
        /// <param name="RadioButton_Result"></param>
        /// <param name="RadioButton_Recheck"></param>
        public void Update_listView_Result_AB(ListView ListView_Result_AB, RadioButton RadioButton_Result, RadioButton RadioButton_Recheck) // (20200429) Jeff Revised!
        {
            if (this.Locate_method_A == null || this.Locate_method_B_2_A == null || this.Locate_method_B == null) // (20200429) Jeff Revised!
            {
                ListView_Result_AB.Items.Clear();
                return;
            }

            if (RadioButton_Result.Checked) // 顯示檢測結果
                ListView_Result_AB.ForeColor = System.Drawing.SystemColors.WindowText;
            else if (RadioButton_Recheck.Checked) // 顯示人工覆判結果
                ListView_Result_AB.ForeColor = Color.DarkViolet;

            ListView_Result_AB.BeginUpdate();
            ListView_Result_AB.Items.Clear();

            #region 資料建立

            Dictionary<string, List<int>> Datas = new Dictionary<string, List<int>>();
            List<int> ListInt = new List<int>();
            if (RadioButton_Result.Checked) // 顯示檢測結果
            {
                // A面
                ListInt.Clear();
                ListInt.Add(this.Locate_method_A.all_defect_id_.Count);
                ListInt.Add(this.Locate_method_A.get_total_cell_count());
                Datas.Add("A面", ListInt.ToList()); // 必須加入 ToList()，否則值會被覆蓋掉
                // B面
                ListInt.Clear();
                ListInt.Add(this.All_B_2_A_defect_id.Count);
                ListInt.Add(this.Locate_method_B_2_A.get_total_cell_count());
                Datas.Add("B面", ListInt.ToList());
                // 雙面合併
                ListInt.Clear();
                ListInt.Add(this.All_AB_defect_id.Count);
                ListInt.Add(this.Locate_method_A.get_total_cell_count());
                Datas.Add("雙面合併", ListInt.ToList());
            }
            else if (RadioButton_Recheck.Checked) // 顯示人工覆判結果
            {
                // A面
                ListInt.Clear();
                ListInt.Add(this.Locate_method_A.all_defect_id_Recheck.Count);
                ListInt.Add(this.Locate_method_A.get_total_cell_count());
                Datas.Add("A面", ListInt.ToList());
                // B面
                ListInt.Clear();
                ListInt.Add(this.All_B_2_A_defect_id_Recheck.Count);
                ListInt.Add(this.Locate_method_B_2_A.get_total_cell_count());
                Datas.Add("B面", ListInt.ToList());
                // 雙面合併
                ListInt.Clear();
                ListInt.Add(this.All_AB_defect_id_Recheck.Count);
                ListInt.Add(this.Locate_method_A.get_total_cell_count());
                Datas.Add("雙面合併", ListInt.ToList());
            }

            #endregion

            foreach (KeyValuePair<string, List<int>> item in Datas)
            {
                ListViewItem lvi = new ListViewItem(item.Key);
                lvi.SubItems.Add(item.Value[0].ToString());
                lvi.SubItems.Add((100.0 - 100.0 * item.Value[0] / item.Value[1]).ToString("#0.00"));
                ListView_Result_AB.Items.Add(lvi);
            }

            ListView_Result_AB.EndUpdate();
        }

        /// <summary>
        /// 設定 B_NG_Classify & 更新 NGClassify_Statistics
        /// </summary>
        /// <param name="b_NG_Classify"></param>
        public void Set_B_NG_Classify(bool b_NG_Classify) // (20200429) Jeff Revised!
        {
            this.B_NG_Classify = b_NG_Classify;
            this.Update_NGClassify_Statistics();
        }

        /// <summary>
        /// 【瑕疵分類統計 (人工覆判結果)】: 更新 listView_NGClassify_Statistics
        /// </summary>
        /// <param name="listView_NGClassify_Statistics"></param>
        public void Update_listView_NGClassify_Statistics(ListView listView_NGClassify_Statistics) // (20200429) Jeff Revised!
        {
            if (this.Locate_method_A == null || this.Locate_method_B_2_A == null || this.Locate_method_B == null || this.B_NG_Classify == false)
            {
                listView_NGClassify_Statistics.Items.Clear();
                return;
            }

            listView_NGClassify_Statistics.BeginUpdate();
            listView_NGClassify_Statistics.Items.Clear();

            foreach (KeyValuePair<string, DefectsResult> item in this.NGClassify_Statistics)
            {
                ListViewItem lvi = new ListViewItem(item.Key);
                lvi.SubItems.Add(item.Value.Count_NG.ToString());
                lvi.SubItems.Add((100.0 - 100.0 * item.Value.Count_NG / this.Locate_method_A.get_total_cell_count()).ToString("#0.00"));
                listView_NGClassify_Statistics.Items.Add(lvi);
            }

            listView_NGClassify_Statistics.EndUpdate();
        }

        #endregion

    }

    /// <summary>
    /// 輸出類型
    /// </summary>
    public enum enu_OutputType // (20200429) Jeff Revised!
    {
        Image,
        Word,
        PDF
    }
    
    /// <summary>
    /// 【瑕疵整合輸出】Recipe
    /// </summary>
    [Serializable]
    public class cls_DefectCombined // (20200429) Jeff Revised!
    {
        public cls_DefectCombined()
        {
            // 初始化List參數
            this.Initial_List();
        }
        
        /// <summary>
        /// 各單一單/雙面瑕疵結果
        /// </summary>
        [Serializable]
        public class DefectResult_1pcs
        {
            public DefectResult_1pcs() { }

            #region 參數

            /// <summary>
            /// 是否為雙面，否則為單面
            /// </summary>
            public bool B_doubleSide { get; set; } = true;

            /// <summary>
            /// 各單/雙面瑕疵結果 Recipe路徑
            /// </summary>
            public string Directory { get; set; } = "";

            /// <summary>
            /// 瑕疵整合是否上下翻轉
            /// </summary>
            public bool B_UpDown_invert { get; set; } = false;

            /// <summary>
            /// 瑕疵整合是否左右翻轉
            /// </summary>
            public bool B_LeftRight_invert { get; set; } = false;

            /// <summary>
            /// 所有瑕疵所在cell座標 (翻轉後座標) (人工覆判結果)
            /// </summary>
            [XmlIgnore] // 帶有XmlIgnore，表示序列化時不序列化此屬性
            public List<Point> All_defect_id_Recheck { get; set; } = new List<Point>();

            /// <summary>
            /// 模組名稱
            /// </summary>
            public string productName { get; set; } = ""; // (20200429) Jeff Revised!

            /// <summary>
            /// 工單名稱
            /// </summary>
            public string moduleName { get; set; } = "";

            /// <summary>
            /// 生產料號
            /// </summary>
            public string partNumber { get; set; } = "Default1234";

            /// <summary>
            /// 序號
            /// </summary>
            public string sB_ID { get; set; } = "00000000";

            #endregion

            #region 方法

            /// <summary>
            /// 代替 建構子，初始化 之功能  (靜態方法)
            /// </summary>
            /// <param name="b_doubleSide"></param>
            /// <param name="directory"></param>
            /// <param name="b_UpDown_invert"></param>
            /// <param name="b_LeftRight_invert"></param>
            /// <param name="cell_col_count_">X方向總Cell顆數</param>
            /// <param name="cell_row_count_">Y方向總Cell顆數</param>
            /// <returns></returns>
            public static DefectResult_1pcs DefectResult_1pcs_Constructor(bool b_doubleSide, string directory, bool b_UpDown_invert, bool b_LeftRight_invert, int cell_col_count_, int cell_row_count_)
            {
                DefectResult_1pcs recipe = new DefectResult_1pcs();
                recipe.B_doubleSide = b_doubleSide;
                recipe.Directory = directory;
                recipe.B_UpDown_invert = b_UpDown_invert;
                recipe.B_LeftRight_invert = b_LeftRight_invert;
                if (recipe.Execute(cell_col_count_, cell_row_count_) == false)
                    recipe = null;
                return recipe;
            }

            /// <summary>
            /// 計算 All_defect_id_Recheck
            /// </summary>
            /// <param name="cell_col_count_">X方向總Cell顆數</param>
            /// <param name="cell_row_count_">Y方向總Cell顆數</param>
            /// <returns></returns>
            public bool Execute(int cell_col_count_, int cell_row_count_)
            {
                bool b_status_ = false;

                try
                {
                    #region 載入工單，以取得原始所有人工覆判瑕疵座標 & 工單相關資訊

                    this.All_defect_id_Recheck.Clear();
                    if (this.B_doubleSide) // 雙面
                    {
                        DefectTest_AB_Recipe rec = new DefectTest_AB_Recipe();
                        if (clsStaticTool.LoadXML(this.Directory + "\\" + "DefectTest_AB_Recipe.xml", out rec) == false)
                            throw new Exception("載入雙面工單失敗，路徑: " + this.Directory + "\\" + "DefectTest_AB_Recipe.xml"); // 丟出例外狀況
                        this.All_defect_id_Recheck = rec.All_AB_defect_id_Recheck;

                        // 工單相關資訊
                        this.productName = rec.productName;
                        this.moduleName = rec.moduleName;
                        this.partNumber = rec.partNumber;
                        this.sB_ID = rec.sB_ID;
                    }
                    else // 單面
                    {
                        DefectTest_Recipe rec = new DefectTest_Recipe();
                        if (clsStaticTool.LoadXML(this.Directory + "\\" + "DefectTest_Recipe.xml", out rec) == false)
                            throw new Exception("載入單面工單失敗，路徑: " + this.Directory + "\\" + "DefectTest_Recipe.xml"); // 丟出例外狀況
                        this.All_defect_id_Recheck = rec.LocateMethodRecipe.all_defect_id_Recheck;

                        // 工單相關資訊
                        this.productName = rec.LocateMethodRecipe.productName; // (20200429) Jeff Revised!
                        this.moduleName = rec.LocateMethodRecipe.moduleName;
                        this.partNumber = rec.LocateMethodRecipe.partNumber;
                        this.sB_ID = rec.LocateMethodRecipe.sB_ID;
                    }

                    #endregion

                    #region 座標轉換

                    if (this.B_UpDown_invert || this.B_LeftRight_invert)
                    {
                        List<Point> temp;
                        this.Convert_coordinate_B_2_A_List(this.All_defect_id_Recheck, out temp, cell_col_count_, cell_row_count_);
                        this.All_defect_id_Recheck = temp;
                    }

                    #endregion

                    b_status_ = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "各單一單/雙面瑕疵結果執行錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                return b_status_;
            }

            /// <summary>
            /// 座標轉換: B→A (單點)
            /// </summary>
            /// <param name="pt_B"></param>
            /// <param name="pt_A"></param>
            /// <param name="cell_col_count_">X方向總Cell顆數</param>
            /// <param name="cell_row_count_">Y方向總Cell顆數</param>
            /// <returns></returns>
            private void Convert_coordinate_B_2_A(Point pt_B, out Point pt_A, int cell_col_count_, int cell_row_count_)
            {
                pt_A = pt_B;
                // y座標: 雙面合併是否上下翻轉
                if (this.B_UpDown_invert)
                    pt_A.Y = cell_row_count_ - pt_B.Y + 1;

                // x座標: 雙面合併是否左右翻轉
                if (this.B_LeftRight_invert)
                    pt_A.X = cell_col_count_ - pt_B.X + 1;
            }

            /// <summary>
            /// 座標轉換: B→A (多點)
            /// </summary>
            /// <param name="ListPt_B"></param>
            /// <param name="ListPt_A"></param>
            /// <param name="cell_col_count_">X方向總Cell顆數</param>
            /// <param name="cell_row_count_">Y方向總Cell顆數</param>
            private void Convert_coordinate_B_2_A_List(List<Point> ListPt_B, out List<Point> ListPt_A, int cell_col_count_, int cell_row_count_)
            {
                ListPt_A = ListPt_B.ToList();
                for (int i = 0; i < ListPt_B.Count; i++)
                {
                    Point pt_A;
                    this.Convert_coordinate_B_2_A(ListPt_B[i], out pt_A, cell_col_count_, cell_row_count_);
                    ListPt_A[i] = pt_A;
                }
            }

            #endregion
        }

        #region 參數

        /// <summary>
        /// 各單/雙面瑕疵結果工單
        /// </summary>
        public List<DefectResult_1pcs> Recipes_defComb = new List<DefectResult_1pcs>();

        /// <summary>
        /// 瑕疵整合座標 (人工覆判結果)
        /// </summary>
        [XmlIgnore] // 帶有XmlIgnore，表示序列化時不序列化此屬性
        public List<Point> All_defect_id_Recheck_defComb { get; set; } = new List<Point>();

        /// <summary>
        /// 忽略Cell之id
        /// </summary>
        public List<Point> ListPt_BypassCell { get; set; } = new List<Point>();

        /// <summary>
        /// 【瑕疵整合輸出】Recipe路徑
        /// </summary>
        public string Directory { get; set; } = "";

        /// <summary>
        /// 瑕疵標注卡原始影像所在檔案位置 (Note: 包含影像名稱及副檔名)
        /// </summary>
        public string Path_Image { get; set; } = "";

        /// <summary>
        /// 瑕疵標注卡原始影像
        /// </summary>
        public HObject Image = null;
        
        /// <summary>
        /// 是否顯示每顆Cell區域
        /// </summary>
        public bool B_dispCell { get; set; } = false;

        /// <summary>
        /// 所有 Cell Region
        /// </summary>
        [XmlIgnore] // 帶有XmlIgnore，表示序列化時不序列化此屬性
        private HObject CellReg = null;

        /// <summary>
        /// 所有 NG Cell Region
        /// </summary>
        [XmlIgnore] // 帶有XmlIgnore，表示序列化時不序列化此屬性
        private HObject CellReg_NG = null;

        #region 瑕疵標注卡設定

        #region Cell資訊

        /// <summary>
        /// Cell 寬度
        /// </summary>
        public double cell_pWidth { get; set; } = 1.0;

        /// <summary>
        /// Cell 高度
        /// </summary>
        public double cell_pHeight { get; set; } = 1.0;

        /// <summary>
        /// Cell dx
        /// </summary>
        public double cell_pdx { get; set; } = 1.0;

        /// <summary>
        /// Cell dy
        /// </summary>
        public double cell_pdy { get; set; } = 1.0;

        /// <summary>
        /// Cell 顏色
        /// </summary>
        public string Str_CellColor_Halcon { get; set; } = "#00ff00ff";

        /// <summary>
        /// NG Cell 顏色
        /// </summary>
        public string Str_NGColor_Halcon { get; set; } = "#ff0000ff";

        #endregion

        #region X方向

        /// <summary>
        /// 分區數量 (X方向)
        /// </summary>
        public int num_region_x { get; set; } = 1;

        /// <summary>
        /// 各分區之Cell顆數 (X方向)
        /// </summary>
        public List<int> reg_cell_x_count { get; set; } = new List<int>();

        /// <summary>
        /// X方向總Cell顆數
        /// </summary>
        [XmlIgnore] // 帶有XmlIgnore，表示序列化時不序列化此屬性
        public int cell_col_count
        {
            get
            {
                int total = 0;
                foreach (int i in this.reg_cell_x_count)
                    total += i;
                return total;
            }
        }

        /// <summary>
        /// X方向各分區與原點距離 (Pixel)
        /// </summary>
        public List<double> dist_reg_pdx { get; set; } = new List<double>();

        #endregion

        #region Y方向

        /// <summary>
        /// 分區數量 (Y方向)
        /// </summary>
        public int num_region_y { get; set; } = 1;

        /// <summary>
        /// 各分區之Cell顆數 (Y方向)
        /// </summary>
        public List<int> reg_cell_y_count { get; set; } = new List<int>();

        /// <summary>
        /// Y方向總Cell顆數
        /// </summary>
        [XmlIgnore] // 帶有XmlIgnore，表示序列化時不序列化此屬性
        public int cell_row_count
        {
            get
            {
                int total = 0;
                foreach (int i in this.reg_cell_y_count)
                    total += i;
                return total;
            }
        }

        /// <summary>
        /// Y方向各分區與原點距離 (Pixel)
        /// </summary>
        public List<double> dist_reg_pdy { get; set; } = new List<double>();

        #endregion

        #region 輸出設定

        /// <summary>
        /// 輸出類型
        /// </summary>
        public enu_OutputType OutputType { get; set; } = enu_OutputType.Image;

        /// <summary>
        /// 影像輸出格式
        /// </summary>
        public enu_DispImageFormat ImageFormat { get; set; } = enu_DispImageFormat.png;

        /// <summary>
        /// Word輸出格式
        /// </summary>
        public enu_WordFormat WordFormat { get; set; } = enu_WordFormat.docx;

        /// <summary>
        /// 影像寬度 (cm)
        /// </summary>
        public float ImageWidth_cm { get; set; } = (float)10.0;

        /// <summary>
        /// 影像高度 (cm)
        /// </summary>
        public float ImageHeight_cm { get; set; } = (float)10.0;

        #endregion

        #endregion

        #endregion

        #region 方法

        /// <summary>
        /// 初始化List參數
        /// </summary>
        public void Initial_List()
        {
            // 初始化參數
            this.reg_cell_x_count.Add(1);
            this.reg_cell_y_count.Add(1);
            this.dist_reg_pdx.Add(0);
            this.dist_reg_pdy.Add(0);
        }

        public void Clear_List()
        {
            this.reg_cell_x_count.Clear();
            this.reg_cell_y_count.Clear();
            this.dist_reg_pdx.Clear();
            this.dist_reg_pdy.Clear();
        }

        /// <summary>
        /// 載入 XML 後，原先經過初始化的List會多一個元素，必須移除掉!!!
        /// </summary>
        public void Recover_List()
        {
            if (this.reg_cell_x_count.Count > 0)
                this.reg_cell_x_count.RemoveAt(0);
            if (this.reg_cell_y_count.Count > 0)
                this.reg_cell_y_count.RemoveAt(0);
            if (this.dist_reg_pdx.Count > 0)
                this.dist_reg_pdx.RemoveAt(0);
            if (this.dist_reg_pdy.Count > 0)
                this.dist_reg_pdy.RemoveAt(0);
        }

        /// <summary>
        /// 【載入工單】
        /// </summary>
        /// <param name="Recipe"></param>
        /// <param name="directory_default"></param>
        /// <param name="b_ShowDialog"></param>
        /// <returns></returns>
        public static bool Load_Recipe(out cls_DefectCombined Recipe, string directory_default = "", bool b_ShowDialog = true)
        {
            Recipe = new cls_DefectCombined();

            if (b_ShowDialog)
            {
                FolderBrowserDialog Dilg = new FolderBrowserDialog();
                Dilg.Description = "【載入工單】";
                Dilg.SelectedPath = directory_default; // 初始路徑
                if (Dilg.ShowDialog() != DialogResult.OK)
                    return false;
                directory_default = Dilg.SelectedPath;
            }

            if (string.IsNullOrEmpty(directory_default))
            {
                SystemSounds.Exclamation.Play();
                MessageBox.Show("路徑無效!", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            //Recipe.Clear_List();
            if (clsStaticTool.LoadXML(directory_default + "\\" + "DefectCombined_Recipe.xml", out Recipe))
            {
                Recipe.Directory = directory_default; // 避免工單內儲存之 Directory 是錯誤的!
                Recipe.Recover_List();
                Recipe.Load_Image();
                if (Recipe.Execute())
                    return true;
            }
            
            SystemSounds.Exclamation.Play();
            MessageBox.Show("【載入工單】失敗", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        /// <summary>
        /// 【儲存工單】
        /// </summary>
        /// <returns></returns>
        public bool Save_Recipe()
        {
            if (this.Directory == "")
            {
                FolderBrowserDialog Dilg = new FolderBrowserDialog();
                Dilg.Description = "【儲存工單】";
                if (Dilg.ShowDialog() != DialogResult.OK)
                    return false;

                if (string.IsNullOrEmpty(Dilg.SelectedPath))
                {
                    SystemSounds.Exclamation.Play();
                    MessageBox.Show("路徑無效!", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                else
                    this.Directory = Dilg.SelectedPath;
            }

            return clsStaticTool.SaveXML(this, this.Directory + "\\" + "DefectCombined_Recipe.xml");
        }

        /// <summary>
        /// 【新增】工單
        /// </summary>
        /// <param name="b_doubleSide"></param>
        /// <param name="b_UpDown_invert"></param>
        /// <param name="b_LeftRight_invert"></param>
        /// <param name="directory_default"></param>
        /// <param name="b_ShowDialog"></param>
        /// <returns></returns>
        public bool Add(bool b_doubleSide, bool b_UpDown_invert, bool b_LeftRight_invert, string directory_default = "", bool b_ShowDialog = true)
        {
            if (b_ShowDialog)
            {
                FolderBrowserDialog Dilg = new FolderBrowserDialog();
                Dilg.Description = "【新增瑕疵檔】";
                Dilg.SelectedPath = directory_default; // 初始路徑
                if (Dilg.ShowDialog() != DialogResult.OK)
                    return false;
                directory_default = Dilg.SelectedPath;
            }

            DefectResult_1pcs dr = DefectResult_1pcs.DefectResult_1pcs_Constructor(b_doubleSide, directory_default, b_UpDown_invert, b_LeftRight_invert, this.cell_col_count, this.cell_row_count);
            if (dr == null)
                return false;
            else
            {
                this.Recipes_defComb.Add(dr);
                return true;
            }
        }

        /// <summary>
        /// 【刪除】工單
        /// </summary>
        /// <param name="index"></param>
        public void Remove(int index)
        {
            if (index >= 0 && index < this.Recipes_defComb.Count)
                this.Recipes_defComb.RemoveAt(index);
        }

        /// <summary>
        /// 【清空】工單
        /// </summary>
        public void RemoveAll()
        {
            this.Recipes_defComb.Clear();
            this.All_defect_id_Recheck_defComb.Clear();
            Extension.HObjectMedthods.ReleaseHObject(ref this.CellReg_NG);
        }

        /// <summary>
        /// 執行
        /// </summary>
        /// <returns></returns>
        public bool Execute()
        {
            bool b_status_ = false;
            try
            {
                // 更新各單/雙面瑕疵結果之瑕疵座標
                foreach (DefectResult_1pcs dr in this.Recipes_defComb)
                    dr.Execute(this.cell_col_count, this.cell_row_count);

                // 瑕疵整合 (計算 All_defect_id_Recheck_defComb)
                this.Compute_All_defect_id_Recheck_defComb();

                // 計算 CellReg
                if (this.Compute_CellReg() == false)
                    return false;

                // 計算 CellReg_NG
                if (this.Compute_CellReg_NG() == false)
                    return false;

                b_status_ = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "各單/雙面瑕疵結果執行錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return b_status_;
        }

        /// <summary>
        /// 計算 All_defect_id_Recheck_defComb
        /// </summary>
        private void Compute_All_defect_id_Recheck_defComb()
        {
            this.All_defect_id_Recheck_defComb.Clear();
            foreach (DefectResult_1pcs dr in this.Recipes_defComb)
            {
                foreach (Point pt in dr.All_defect_id_Recheck)
                {
                    // 確認座標是否在合理範圍內
                    if (pt.X >= 1 && pt.X <= this.cell_col_count && pt.Y >= 1 && pt.Y <= this.cell_row_count)
                    {
                        // 確認是否為 Bypass Cell
                        if (this.ListPt_BypassCell.Contains(pt))
                            continue;

                        if (this.All_defect_id_Recheck_defComb.Contains(pt) == false)
                            this.All_defect_id_Recheck_defComb.Add(pt);
                    }
                }
            }
        }

        /// <summary>
        /// 計算 CellReg
        /// </summary>
        /// <returns></returns>
        private bool Compute_CellReg()
        {
            bool b_status_ = false;
            Extension.HObjectMedthods.ReleaseHObject(ref this.CellReg);
            try
            {
                #region 建立Cell全域地圖

                if (this.Image != null)
                {
                    HTuple width, height;
                    HOperatorSet.GetImageSize(this.Image, out width, out height);
                    HOperatorSet.SetSystem("height", height);
                    HOperatorSet.SetSystem("width", width);

                    // 先產生一張影像，之後才能順利載入region
                    //HObject img;
                    //HOperatorSet.GenImageConst(out img, "byte", width, height);
                    //img.Dispose();
                }

                HObject cellmap_region_, rect_group;
                HOperatorSet.GenEmptyObj(out cellmap_region_);
                HOperatorSet.GenEmptyObj(out rect_group);

                #region y方向

                for (int y_reg = 0; y_reg < this.num_region_y; y_reg++)
                {
                    for (int y = 0; y < this.reg_cell_y_count[y_reg]; y++)
                    {
                        HObject cell_rect_;
                        HOperatorSet.GenRectangle2(out cell_rect_, this.cell_pdy * y + this.dist_reg_pdy[y_reg], 0, 0, this.cell_pWidth / 2, this.cell_pHeight / 2);
                        HOperatorSet.ConcatObj(rect_group, cell_rect_, out rect_group);
                        cell_rect_.Dispose();
                    }
                }

                #endregion

                #region x方向

                for (int x_reg = 0; x_reg < this.num_region_x; x_reg++)
                {
                    for (int x = 0; x < this.reg_cell_x_count[x_reg]; x++)
                    {
                        HObject moved;
                        HOperatorSet.MoveRegion(rect_group, out moved, 0, this.cell_pdx * x + this.dist_reg_pdx[x_reg]);
                        HOperatorSet.ConcatObj(cellmap_region_, moved, out cellmap_region_);
                        moved.Dispose();
                    }
                }

                #endregion
                
                HOperatorSet.SortRegion(cellmap_region_, out this.CellReg, "first_point", "true", "row");

                // Release memories
                cellmap_region_.Dispose();
                rect_group.Dispose();

                #endregion

                // 消除 Bypass Cell
                HObject Reg = this.Compute_CellReg_ListCellID(this.ListPt_BypassCell);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Difference(this.CellReg, Reg, out ExpTmpOutVar_0);
                    this.CellReg.Dispose();
                    this.CellReg = ExpTmpOutVar_0;
                }

                b_status_ = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "計算 Cell Region 錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return b_status_;
        }

        /// <summary>
        /// 瑕疵Cell座標 轉 int (CellReg 之index)
        /// </summary>
        /// <param name="cellID"></param>
        /// <returns></returns>
        private int cellID_2_int(Point cellID)
        {
            return (cellID.Y - 1) * this.cell_col_count + cellID.X;
        }

        /// <summary>
        /// List的瑕疵Cell座標 轉 HTuple (CellReg 之index)
        /// </summary>
        /// <param name="ListCellID"></param>
        /// <returns></returns>
        private HTuple ListCellID_2_HTuple(List<Point> ListCellID)
        {
            HTuple result = new HTuple();
            foreach (Point pt in ListCellID)
            {
                // 確認座標是否在合理範圍內才進行轉換
                if (pt.X >= 1 && pt.X <= this.cell_col_count && pt.Y >= 1 && pt.Y <= this.cell_row_count)
                {
                    // 確認是否為 Bypass Cell
                    //if (this.ListPt_BypassCell.Contains(pt))
                    //    continue;
                    HOperatorSet.TupleConcat(result, this.cellID_2_int(pt), out result);
                }
            }
            return result;
        }

        /// <summary>
        /// 指定座標組，計算其 Cell Region
        /// </summary>
        /// <param name="ListCellID"></param>
        /// <returns></returns>
        private HObject Compute_CellReg_ListCellID(List<Point> ListCellID)
        {
            HObject result;
            HOperatorSet.GenEmptyObj(out result);
            if (ListCellID.Count <= 0)
                return result;

            try
            {
                HTuple index = this.ListCellID_2_HTuple(ListCellID);
                result.Dispose();
                HOperatorSet.SelectObj(this.CellReg, out result, index);
            }
            catch (Exception ex)
            { }

            return result;
        }

        /// <summary>
        /// 計算 CellReg_NG
        /// </summary>
        /// <returns></returns>
        private bool Compute_CellReg_NG()
        {
            bool b_status_ = false;
            Extension.HObjectMedthods.ReleaseHObject(ref this.CellReg_NG);
            try
            {
                this.CellReg_NG = this.Compute_CellReg_ListCellID(this.All_defect_id_Recheck_defComb);

                b_status_ = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "計算 NG Cell Region 錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return b_status_;
        }

        /// <summary>
        /// num_region_x 數值更新
        /// </summary>
        /// <param name="num_region_x_"></param>
        public void Update_num_region_x(int num_region_x_)
        {
            this.num_region_x = num_region_x_;

            // 初始化 reg_cell_x_count & dist_reg_pdx
            this.reg_cell_x_count.Clear();
            this.dist_reg_pdx.Clear();
            for (int i = 1; i <= this.num_region_x; i++)
            {
                this.reg_cell_x_count.Add(1);
                this.dist_reg_pdx.Add(0);
            }
        }

        /// <summary>
        /// num_region_y 數值更新
        /// </summary>
        /// <param name="num_region_y_"></param>
        public void Update_num_region_y(int num_region_y_)
        {
            this.num_region_y = num_region_y_;

            // 初始化 reg_cell_y_count & dist_reg_pdy
            this.reg_cell_y_count.Clear();
            this.dist_reg_pdy.Clear();
            for (int i = 1; i <= this.num_region_y; i++)
            {
                this.reg_cell_y_count.Add(1);
                this.dist_reg_pdy.Add(0);
            }
        }

        /// <summary>
        /// 【載入影像】
        /// </summary>
        /// <returns></returns>
        public bool Load_Image()
        {
            bool b_status_ = false;
            Extension.HObjectMedthods.ReleaseHObject(ref this.Image);
            try
            {
                HOperatorSet.ReadImage(out this.Image, this.Path_Image);

                b_status_ = true;
            }
            catch (Exception ex)
            {
                Extension.HObjectMedthods.ReleaseHObject(ref this.Image);
                MessageBox.Show(ex.ToString(), "載入影像錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return b_status_;
        }

        /// <summary>
        /// 顯示
        /// </summary>
        /// <param name="cbx_dispCell_defComb">顯示每顆Cell區域</param>
        /// <param name="rbt_OrigImg_defComb">原始影像</param>
        /// <param name="rbt_ResultImg_defComb">結果影像</param>
        public void Display(HSmartWindowControl hSmartWindowControl, CheckBox cbx_dispCell_defComb, RadioButton rbt_OrigImg_defComb, RadioButton rbt_ResultImg_defComb)
        {
            this.B_dispCell = cbx_dispCell_defComb.Checked;
            if (this.Image == null)
                return;

            HOperatorSet.ClearWindow(hSmartWindowControl.HalconWindow);
            HOperatorSet.DispObj(this.Image, hSmartWindowControl.HalconWindow);

            if (rbt_ResultImg_defComb.Checked) // 結果影像
            {
                // 顯示每顆Cell區域
                if (this.B_dispCell)
                {
                    if (this.CellReg != null)
                    {
                        try
                        {
                            HOperatorSet.SetDraw(hSmartWindowControl.HalconWindow, "margin");
                            HOperatorSet.SetColor(hSmartWindowControl.HalconWindow, this.Str_CellColor_Halcon);
                            HOperatorSet.DispObj(this.CellReg, hSmartWindowControl.HalconWindow);
                        }
                        catch (Exception ex)
                        { }
                    }
                }

                // 顯示瑕疵Cell
                if (this.CellReg_NG != null)
                {
                    try
                    {
                        HOperatorSet.SetDraw(hSmartWindowControl.HalconWindow, "fill");
                        HOperatorSet.SetColor(hSmartWindowControl.HalconWindow, this.Str_NGColor_Halcon);
                        HOperatorSet.DispObj(this.CellReg_NG, hSmartWindowControl.HalconWindow);
                    }
                    catch (Exception ex)
                    { }
                }
            }
        }

        /// <summary>
        /// 【儲存】(根據輸出類型及格式)
        /// </summary>
        /// <param name="b_ShowDialog"></param>
        /// <param name="savePath">輸出路徑 (含檔名但不含副檔名)</param>
        /// <param name="b_combine_1File">是否整合成一個檔案輸出</param>
        /// <param name="b_Initial">是否為第一筆資料</param>
        /// <param name="b_End">是否為最後一筆資料</param>
        public void Save_Result(bool b_ShowDialog = true, string savePath = "", bool b_combine_1File = false, bool b_Initial = true, bool b_End = false)
        {
            if (this.OutputType == enu_OutputType.Image)
                this.Save_ResultImage(b_ShowDialog, savePath);
            else
                this.Save_Result_WordPDF(b_ShowDialog, savePath, b_combine_1File, b_Initial, b_End);
        }

        /// <summary>
        /// 另存結果影像
        /// </summary>
        /// <param name="b_ShowDialog"></param>
        /// <param name="savePath">輸出路徑 (含檔名但不含副檔名)</param>
        private void Save_ResultImage(bool b_ShowDialog = true, string savePath = "")
        {
            if (this.Image == null)
            {
                MessageBox.Show("尚未載入瑕疵標注卡影像", "溫馨提醒", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (b_ShowDialog)
            {
                SaveFileDialog saveImgDialog = new SaveFileDialog();
                //saveImgDialog.Filter = "TIFF Image|*.tif|JPG Image|*.jpg|PNG Image|*.png|Bitmap Image|*.bmp";
                //saveImgDialog.Filter = "PNG Image|*.png";
                saveImgDialog.Filter = SaveImageFilter[(int)this.ImageFormat];

                saveImgDialog.Title = "另存結果影像";
                if (saveImgDialog.ShowDialog() != DialogResult.OK)
                    return;
                savePath = saveImgDialog.FileName;
            }
            else
                savePath += "." + this.ImageFormat.ToString();

            try
            {
                HObject ResultImage = this.Compute_ResultImage();
                
                //switch (saveImgDialog.FilterIndex)
                //{
                //    case 1:
                //        HOperatorSet.WriteImage(ResultImage, "tiff", 0, path);
                //        break;
                //    case 2:
                //        HOperatorSet.WriteImage(ResultImage, "jpeg", 0, path);
                //        break;
                //    case 3:
                //        HOperatorSet.WriteImage(ResultImage, "png", 0, path);
                //        break;
                //    case 4:
                //        HOperatorSet.WriteImage(ResultImage, "bmp", 0, path);
                //        break;
                //}
                HOperatorSet.WriteImage(ResultImage, ((enu_ImageFormat)((int)this.ImageFormat)).ToString(), 0, savePath);

                MessageBox.Show("儲存影像成功!");
                ResultImage.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "儲存影像錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        /// <summary>
        /// 計算結果影像
        /// </summary>
        private HObject Compute_ResultImage()
        {
            HObject ResultImage;
            // 轉成RGB image
            HTuple Channels;
            HOperatorSet.CountChannels(this.Image, out Channels);
            if (Channels == 1)
                HOperatorSet.Compose3(this.Image.Clone(), this.Image.Clone(), this.Image.Clone(), out ResultImage);
            else
                HOperatorSet.CopyImage(this.Image, out ResultImage);

            // 顯示每顆Cell區域
            if (this.B_dispCell)
            {
                if (this.CellReg != null)
                {
                    Color color = clsStaticTool.GetSystemColor(this.Str_CellColor_Halcon);
                    HOperatorSet.OverpaintRegion(ResultImage, this.CellReg, ((new HTuple((int)color.R)).TupleConcat((int)color.G)).TupleConcat((int)color.B), "margin");
                }
            }

            // 顯示瑕疵Cell
            if (this.CellReg_NG != null)
            {
                Color color = clsStaticTool.GetSystemColor(this.Str_NGColor_Halcon);
                HOperatorSet.OverpaintRegion(ResultImage, this.CellReg_NG, ((new HTuple((int)color.R)).TupleConcat((int)color.G)).TupleConcat((int)color.B), "fill");
            }

            return ResultImage;
        }

        private Cls_WordPDF clsWordPDF { get; set; } = new Cls_WordPDF();
        public void Assign_clsWordPDF(Cls_WordPDF clsWordPDF_)
        {
            this.clsWordPDF = clsWordPDF_;
        }
        public Cls_WordPDF Get_clsWordPDF()
        {
            return this.clsWordPDF;
        }

        /// <summary>
        /// 另存結果 Word/PDF
        /// </summary>
        /// <param name="b_ShowDialog"></param>
        /// <param name="savePath">輸出路徑 (含檔名但不含副檔名)</param>
        /// <param name="b_combine_1File">是否整合成一個檔案輸出</param>
        /// <param name="b_Initial">是否為第一筆資料</param>
        /// <param name="b_End">是否為最後一筆資料</param>
        private void Save_Result_WordPDF(bool b_ShowDialog = true, string savePath = "", bool b_combine_1File = false, bool b_Initial = true, bool b_End = false)
        {
            if (this.Image == null)
            {
                MessageBox.Show("尚未載入瑕疵標注卡影像", "溫馨提醒", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            enu_OutputFormat outputFormat = enu_OutputFormat.docx;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (this.OutputType == enu_OutputType.Word)
            {
                outputFormat = (enu_OutputFormat)Enum.Parse(typeof(enu_OutputFormat), this.WordFormat.ToString());
                if (this.WordFormat == enu_WordFormat.doc)
                {
                    saveFileDialog.Filter = "Word|*.doc";
                    savePath += ".doc"; // 加入副檔名
                }
                else if (this.WordFormat == enu_WordFormat.docx)
                {
                    saveFileDialog.Filter = "Word|*.docx";
                    savePath += ".docx"; // 加入副檔名
                }
            }
            else if (this.OutputType == enu_OutputType.PDF)
            {
                outputFormat = enu_OutputFormat.pdf;
                saveFileDialog.Filter = "PDF|*.pdf";
                savePath += ".pdf"; // 加入副檔名
            }

            if (b_ShowDialog)
            {
                saveFileDialog.Title = "另存結果 " + this.WordFormat.ToString();
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                savePath = saveFileDialog.FileName;
            }

            if (b_combine_1File == false || b_Initial)
                this.clsWordPDF = new Cls_WordPDF();
            //enu_OutputFormat outputFormat = enu_OutputFormat.docx;
            string title = "乾坤uPOL瑕疵標注卡 (QC1~QC3瑕疵整合輸出)";
            float FontSize_title = 16;
            int numRows = 2, numColumns = 8;

            List<List<float>> FontSize_tb = new List<List<float>>();
            List<float> FontSize_Row1 = new List<float>() { 12, 12, 12, 12, 12, 12, 12, 12 };
            List<float> FontSize_Row2 = new List<float>() { 9, 9, 9, 9, 9, 9, 9, 9 };
            FontSize_tb.Add(FontSize_Row1);
            FontSize_tb.Add(FontSize_Row2);

            List<List<string>> Text_tb = new List<List<string>>();
            List<string> Text_Row1 = new List<string>() { "工單", "產品名稱", "LotNum", "片號", "NG顆數", "良率(%)", "日期", "時間" };
            string AllModuleName = "", AllProductName = "", AllPartNumber = "", AllSB_ID = ""; // 工單, 產品名稱, 生產料號, 序號
            if (this.Recipes_defComb.Count > 0)
            {
                AllModuleName = this.Recipes_defComb[0].moduleName;
                AllProductName = this.Recipes_defComb[0].productName;
                AllPartNumber = this.Recipes_defComb[0].partNumber;
                AllSB_ID = this.Recipes_defComb[0].sB_ID;
                for (int i = 1; i < this.Recipes_defComb.Count; i++)
                {
                    AllModuleName += "\n" + this.Recipes_defComb[i].moduleName;
                    AllProductName += "\n" + this.Recipes_defComb[i].productName;
                    AllPartNumber += "\n" + this.Recipes_defComb[i].partNumber;
                    AllSB_ID += "\n" + this.Recipes_defComb[i].sB_ID;
                }
            }
            //List<string> Text_Row2 = new List<string>() { "QC1-A-18x12_20191101\nQC2-A-18x12_20191101\nQC3-A-18x12_20191101", "QC1_1234\nQC2_1234\nQC3_1234", "00000067\n00000168\n00000020", "11", "99.56", DateTime.Now.ToShortDateString(), DateTime.Now.ToString("HH:mm:ss") };
            List<string> Text_Row2 = new List<string>() { AllModuleName, AllProductName, AllPartNumber, AllSB_ID, this.Compute_NGCount().ToString(), this.Compute_Yield().ToString("#0.00"), DateTime.Now.ToShortDateString(), DateTime.Now.ToString("HH:mm:ss") };
            Text_tb.Add(Text_Row1);
            Text_tb.Add(Text_Row2);

            List<List<float>> Width_cm_tb = new List<List<float>>();
            List<float> Width_cm_Row1 = new List<float>() { 5, 3, 3, 3, 1.8f, 1.8f, 1.8f, 1.8f };
            for (int y = 0; y < numRows; y++)
                Width_cm_tb.Add(Width_cm_Row1);

            HObject ResultImage = this.Compute_ResultImage();
            Bitmap Image = ResultImage.GetRGBBitmap(1);
            if (b_combine_1File)
                clsWordPDF.CreateAdd_WordPDF_withImg(savePath, outputFormat, title, FontSize_title,
                                                  numRows, numColumns, FontSize_tb, Text_tb, Width_cm_tb,
                                                  Image, this.ImageWidth_cm, this.ImageHeight_cm,
                                                  b_Initial, true, b_End);
            else
                clsWordPDF.Create_WordPDF_withImg(savePath, outputFormat, title, FontSize_title,
                                              numRows, numColumns, FontSize_tb, Text_tb, Width_cm_tb,
                                              Image, this.ImageWidth_cm, this.ImageHeight_cm);

            Image.Dispose();
            ResultImage.Dispose();
        }

        /// <summary>
        /// 計算NG顆數
        /// </summary>
        /// <returns></returns>
        private int Compute_NGCount()
        {
            return this.All_defect_id_Recheck_defComb.Count;
        }

        /// <summary>
        /// 計算良率 (%)
        /// </summary>
        /// <returns></returns>
        private double Compute_Yield()
        {
            double total = this.cell_row_count * this.cell_col_count - this.ListPt_BypassCell.Count;
            return (1.0 - (this.Compute_NGCount() / total)) * 100.0;
        }

        /// <summary>
        /// 釋放記憶體
        /// </summary>
        public void Release()
        {
            Extension.HObjectMedthods.ReleaseHObject(ref this.Image);
            Extension.HObjectMedthods.ReleaseHObject(ref this.CellReg);
            Extension.HObjectMedthods.ReleaseHObject(ref this.CellReg_NG);
        }

        #endregion
    }

    /// <summary>
    /// 批量測試 (人工覆判 & 瑕疵整合輸出)
    /// </summary>
    [Serializable]
    public class cls_BatchTest // (20200429) Jeff Revised!
    {
        public cls_BatchTest() { }

        #region 參數

        /// <summary>
        /// 批量載入各工單之模組名稱 (i.e. 檢測站點)
        /// </summary>
        public List<string> productNames { get; set; } = new List<string>(); // (20200429) Jeff Revised!

        /// <summary>
        /// 生產料號
        /// </summary>
        public string partNumber { get; set; } = "";

        /// <summary>
        /// 序號
        /// </summary>
        public string SBID { get; set; } = "";

        /// <summary>
        /// 是否搜尋 生產料號 下所有 序號
        /// </summary>
        public bool B_All_SBID { get; set; } = true;

        /// <summary>
        /// 批量載入各工單路徑
        /// </summary>
        public List<string> Directories { get; set; } = new List<string>();

        /// <summary>
        /// 雙面合併工單 資料夾路徑
        /// </summary>
        public string Directory_AB { get; set; } = "";

        /// <summary>
        /// 是否整合成一個檔案輸出
        /// </summary>
        public bool B_combine_1File { get; set; } = true;

        #endregion

        #region 方法

        /// <summary>
        /// 根據 儲存瑕疵結果路徑\\生產料號\\序號\\雙面合併工單 ，尋找所有子目錄資料夾 (i.e. 檢測站點)
        /// </summary>
        /// <returns></returns>
        public bool Update_Directories()
        {
            this.Directories.Clear();
            this.productNames.Clear();
            string directory_AB_Recipe = FinalInspectParam.Directory_SaveDefectResult + "\\" + this.partNumber + "\\" + this.SBID + "\\" + "雙面合併工單" + "\\";
            if (Directory.Exists(directory_AB_Recipe) == false)
            {
                SystemSounds.Exclamation.Play();
                MessageBox.Show("路徑【" + directory_AB_Recipe + "】不存在，請確認輸入參數是否正確", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            int Method = 2;
            if (Method == 1) // 依數字1, 2, 3...依序命名
            {
                int i = 1;
                while (true)
                {
                    if (Directory.Exists(directory_AB_Recipe + i.ToString()))
                    {
                        this.Directories.Add(directory_AB_Recipe + i.ToString());
                        i++;
                    }
                    else
                        break;
                }
            }
            else
            {
                string[] dirs = Directory.GetDirectories(directory_AB_Recipe);
                this.Directories = dirs.ToList();
                foreach (string s in dirs)
                    this.productNames.Add(Path.GetFileNameWithoutExtension(s));
            }

            this.Directory_AB = directory_AB_Recipe;
            return true;
        }

        /// <summary>
        /// 給定 生產料號，計算包含之所有 序號
        /// </summary>
        /// <param name="list_SBID"></param>
        /// <returns></returns>
        public bool Get_AllSBID_from_partNumber(out List<string> list_SBID)
        {
            list_SBID = new List<string>();
            string dir_partNumber = FinalInspectParam.Directory_SaveDefectResult + "\\" + this.partNumber;
            if (Directory.Exists(dir_partNumber) == false)
            {
                SystemSounds.Exclamation.Play();
                MessageBox.Show("路徑【" + dir_partNumber + "】不存在，請確認輸入參數是否正確", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            string[] dirs = Directory.GetDirectories(dir_partNumber);
            foreach (string s in dirs)
                list_SBID.Add(Path.GetFileNameWithoutExtension(s));
            return true;
        }
        
        #endregion
    }

    /// <summary>
    /// 【瑕疵分類統計】
    /// </summary>
    [Serializable]
    public class cls_NGClassify_Statistics_Multi // (20200429) Jeff Revised!
    {
        public cls_NGClassify_Statistics_Multi() { }

        #region 參數

        /// <summary>
        /// 該LotNum之所有檢測站點 & 各自對應之所有 雙面合併工單 資料夾路徑
        /// </summary>
        [XmlIgnore] // 帶有XmlIgnore，表示序列化時不序列化此屬性
        public Dictionary<string, List<string>> Recipes_ListPath { get; set; } = new Dictionary<string, List<string>>();

        #endregion

        #region 方法

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="recipes_ListPath"></param>
        public void Initialize(Dictionary<string, List<string>> recipes_ListPath = null)
        {
            this.Recipes_ListPath.Clear();
            if (recipes_ListPath != null)
                this.Recipes_ListPath = recipes_ListPath;
        }


        public bool Compute_NGClassify_Statistics_Multi(string productName, ListView listView_NGClassify_Statistics_Multi = null, int cellCount_1pc = 1000)
        {
            if (this.Recipes_ListPath.ContainsKey(productName) == false)
                return false;

            bool b_status_ = false;
            try
            {
                #region 統計各瑕疵數量

                List<string> ListPath = this.Recipes_ListPath[productName];
                int totalChip = ListPath.Count;
                Dictionary<string, DefectsResult> NGClassify_Statistics_Multi = new Dictionary<string, DefectsResult>();
                for (int i = 0; i < totalChip; i++)
                {
                    DefectTest_AB_Recipe AB_Recipe = new DefectTest_AB_Recipe();
                    if (clsStaticTool.LoadXML(ListPath[i] + "\\" + "DefectTest_AB_Recipe.xml", out AB_Recipe) == false)
                    {
                        MessageBox.Show("載入XML路徑: " + ListPath[i] + "\\" + "DefectTest_AB_Recipe.xml" + "失敗", "Load XML Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }

                    if (i == 0)
                        NGClassify_Statistics_Multi = AB_Recipe.NGClassify_Statistics;
                    else
                    {
                        foreach (KeyValuePair<string, DefectsResult> item in AB_Recipe.NGClassify_Statistics)
                        {
                            if (NGClassify_Statistics_Multi.ContainsKey(item.Key))
                                NGClassify_Statistics_Multi[item.Key].Count_NG += item.Value.Count_NG;
                            else
                                NGClassify_Statistics_Multi.Add(item.Key, item.Value);
                        }
                    }
                }

                #endregion

                // 更新 ListView
                if (listView_NGClassify_Statistics_Multi != null)
                {
                    listView_NGClassify_Statistics_Multi.BeginUpdate();
                    listView_NGClassify_Statistics_Multi.Items.Clear();

                    foreach (KeyValuePair<string, DefectsResult> item in NGClassify_Statistics_Multi)
                    {
                        ListViewItem lvi = new ListViewItem(item.Key);
                        lvi.SubItems.Add(item.Value.Count_NG.ToString());
                        lvi.SubItems.Add((100.0 - 100.0 * item.Value.Count_NG / (cellCount_1pc * totalChip)).ToString("#0.00"));
                        listView_NGClassify_Statistics_Multi.Items.Add(lvi);
                    }

                    listView_NGClassify_Statistics_Multi.EndUpdate();
                }

                b_status_ = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return b_status_;
        }

        #endregion
    }

    /// <summary>
    /// AI分圖 Recipe
    /// </summary>
    [Serializable]
    public class cls_AICellImg_Recipe // (20200429) Jeff Revised!
    {
        public HObject OK_CellReg = null;
        public HObject NG_CellReg = null;
        public HObject NONE_CellReg = null;

        /// <summary>
        /// Cell座標 (OK)
        /// </summary>
        public List<Point> CellId_OK = new List<Point>();

        /// <summary>
        /// Cell座標 (NG)
        /// </summary>
        public List<Point> CellId_NG = new List<Point>();

        /// <summary>
        /// Cell座標 (NONE)
        /// </summary>
        public List<Point> CellId_NONE = new List<Point>();

        #region 【AI分圖資訊】

        public int DAVSImgID = 0;
        public bool b_DAVSImgAlign = false;
        public bool b_DAVSMixImgBand = false;
        public int DAVSBand1ImgIndex = 0;
        public int DAVSBand2ImgIndex = 0;
        public int DAVSBand3ImgIndex = 0;
        public enuBand DAVSBand1 = enuBand.R;
        public enuBand DAVSBand2 = enuBand.G;
        public enuBand DAVSBand3 = enuBand.B;

        #endregion

        public cls_AICellImg_Recipe() { }

        /// <summary>
        /// 代替 建構子，初始化 之功能 
        /// Note: 建構式有參數輸入時，會無法儲存到XML!
        /// </summary>
        public void Constructor(LocateMethod locate_method = null)
        {
            Initialize();
            if (locate_method != null)
            {
                CellId_NG = locate_method.all_defect_id_.ToList();
                if (locate_method.all_intersection_defect_CellReg != null)
                    NG_CellReg = locate_method.all_intersection_defect_CellReg.Clone();

                for (int y = 1; y <= locate_method.cell_row_count_; y++)
                {
                    for (int x = 1; x <= locate_method.cell_col_count_; x++)
                    {
                        if (!CellId_NG.Contains(new Point(x, y)))
                        {
                            if (locate_method.LocateMethodRecipe.ListPt_BypassCell.Contains((new Point(x, y)))) // 忽略Cell之id
                                continue;
                            CellId_OK.Add(new Point(x, y));
                        }
                    }
                }
                OK_CellReg = locate_method.Compute_CellReg_ListCellID(CellId_OK);
            }

            #region 【AI分圖資訊】

            DAVSImgID = FinalInspectParam.DAVSImgID;
            b_DAVSImgAlign = FinalInspectParam.b_DAVSImgAlign;
            b_DAVSMixImgBand = FinalInspectParam.b_DAVSMixImgBand;
            DAVSBand1ImgIndex = FinalInspectParam.DAVSBand1ImgIndex;
            DAVSBand2ImgIndex = FinalInspectParam.DAVSBand2ImgIndex;
            DAVSBand3ImgIndex = FinalInspectParam.DAVSBand3ImgIndex;
            DAVSBand1 = FinalInspectParam.DAVSBand1;
            DAVSBand2 = FinalInspectParam.DAVSBand2;
            DAVSBand3 = FinalInspectParam.DAVSBand3;

            #endregion

        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            Release();
            HOperatorSet.GenEmptyRegion(out OK_CellReg);
            HOperatorSet.GenEmptyRegion(out NG_CellReg);
            HOperatorSet.GenEmptyRegion(out NONE_CellReg);
            CellId_OK.Clear();
            CellId_OK = new List<Point>();
            CellId_NG.Clear();
            CellId_NG = new List<Point>();
            CellId_NONE.Clear();
            CellId_NONE = new List<Point>();
        }

        /// <summary>
        /// 釋放記憶體
        /// </summary>
        public void Release()
        {
            Extension.HObjectMedthods.ReleaseHObject(ref OK_CellReg);
            Extension.HObjectMedthods.ReleaseHObject(ref NG_CellReg);
            Extension.HObjectMedthods.ReleaseHObject(ref NONE_CellReg);
        }

        /// <summary>
        /// 標註AI分圖類別，並且更新 Cell Region
        /// </summary>
        /// <param name="locate_method"></param>
        /// <param name="cellID"></param>
        /// <param name="labelType"></param>
        public void Label_cell(LocateMethod locate_method, Point cellID, enu_Label_AICellImg labelType)
        {
            // 更新座標
            switch (labelType)
            {
                case enu_Label_AICellImg.OK:
                    {
                        if (CellId_OK.Contains(cellID))
                            return;
                        else
                        {
                            CellId_OK.Add(cellID);
                            CellId_NG.Remove(cellID);
                            CellId_NONE.Remove(cellID);
                        }
                    }
                    break;
                case enu_Label_AICellImg.NG:
                    {
                        if (CellId_NG.Contains(cellID))
                            return;
                        else
                        {
                            CellId_NG.Add(cellID);
                            CellId_OK.Remove(cellID);
                            CellId_NONE.Remove(cellID);
                        }
                    }
                    break;
                case enu_Label_AICellImg.NONE:
                    {
                        if (CellId_NONE.Contains(cellID))
                            return;
                        else
                        {
                            CellId_NONE.Add(cellID);
                            CellId_NG.Remove(cellID);
                            CellId_OK.Remove(cellID);
                        }
                    }
                    break;
            }

            // 更新 Cell Region
            Update_CellReg(locate_method);

        }

        /// <summary>
        /// 更新 Cell Region
        /// </summary>
        /// <param name="locate_method"></param>
        public void Update_CellReg(LocateMethod locate_method) // (20190906) Jeff Revised!
        {
            Release();
            OK_CellReg = locate_method.Compute_CellReg_ListCellID(CellId_OK);
            NG_CellReg = locate_method.Compute_CellReg_ListCellID(CellId_NG);
            NONE_CellReg = locate_method.Compute_CellReg_ListCellID(CellId_NONE);
        }

        /// <summary>
        /// 儲存一種AI標注類型之影像 
        /// </summary>
        /// <param name="locate_method"></param>
        /// <param name="labelType"></param>
        /// <param name="directory_"></param>
        /// <param name="titleName_image">影像命名</param>
        /// <param name="listMapItem">瑕疵地圖各位置相關資訊</param>
        /// <param name="b_save_specificCell">是否只儲存特定Cell座標</param>
        /// <param name="ListPt_specificCell"></param>
        /// <returns></returns>
        public bool Save_1LabelType_AICellImg(LocateMethod locate_method, enu_Label_AICellImg labelType, string directory_, string titleName_image, List<MapItem> listMapItem,
                                              bool b_save_specificCell = false, List<Point> ListPt_specificCell = null) // (20200429) Jeff Revised!
        {
            bool b_status_ = false;

            try
            {
                // 建立目錄
                //if (!Directory.Exists(directory_))
                //    Directory.CreateDirectory(directory_);

                List<Point> CellId = new List<Point>();
                string Directory_ = directory_;
                if (!b_save_specificCell)
                {
                    switch (labelType)
                    {
                        case enu_Label_AICellImg.OK:
                            {
                                CellId = this.CellId_OK;
                                Directory_ += "OK\\";
                            }
                            break;
                        case enu_Label_AICellImg.NG:
                            {
                                CellId = this.CellId_NG;
                                Directory_ += "NG\\";
                            }
                            break;
                        case enu_Label_AICellImg.NONE:
                            {
                                CellId = this.CellId_NONE;
                                Directory_ += "NONE\\";
                            }
                            break;
                    }
                }
                else // 只儲存特定Cell座標
                    CellId = ListPt_specificCell.ToList();

                // 建立子目錄
                if (!Directory.Exists(Directory_))
                    Directory.CreateDirectory(Directory_);
                else // 如果檔案已存在，則先刪除再儲存!
                {
                    if (!b_save_specificCell)
                    {
                        Directory.Delete(Directory_, true); // true表示指定目錄與其子目錄一起刪除
                        Directory.CreateDirectory(Directory_);
                    }
                }
                Directory_ += titleName_image + "_" + labelType.ToString() + "_";

                if (CellId.Count == 0)
                    return true;

                // 計算各Cell中心座標
                HTuple cell_ids = locate_method.ListCellID_2_HTuple(CellId);
                HObject cells;
                HOperatorSet.SelectObj(locate_method.cellmap_affine_, out cells, cell_ids);
                HTuple x = new HTuple(), y = new HTuple();
                HOperatorSet.RegionFeatures(cells, "column", out x);
                HOperatorSet.RegionFeatures(cells, "row", out y);
                cells.Dispose();

                // 平行運算 (Parallel Programming)
                bool b_error_Parallel = false; // 平行運算中，是否遇到例外狀況
                Parallel.For(0, x.Length, (i, loopState) =>
                {
                    try
                    {
                        List<int> ListMoveIndex = new List<int>();
                        List<HObject> ListCellReg_MoveIndex = new List<HObject>();
                        if (!locate_method.posBigMapCellCenter_2_MoveIndex(new PointF((float)(x[i].D), (float)(y[i].D)), out ListMoveIndex, out ListCellReg_MoveIndex))
                        {
                            // 停止並退出Parallel.For
                            loopState.Stop();
                            b_error_Parallel = true;
                            return;
                        }

                        // 計算AI影像
                        HObject dispImg = clsStaticTool.MixImageBand(listMapItem[ListMoveIndex[0] - 1].ImgObj.Source,
                                                                     this.DAVSBand1, this.DAVSBand2, this.DAVSBand3,
                                                                     this.DAVSBand1ImgIndex, this.DAVSBand2ImgIndex, this.DAVSBand3ImgIndex,
                                                                     this.b_DAVSMixImgBand, this.DAVSImgID, ListCellReg_MoveIndex[0]);
                        clsStaticTool.DisposeAll(ListCellReg_MoveIndex);

                        // 計算座標
                        HTuple cell_id;
                        List<Point> ListCellID;
                        locate_method.pos_2_cellID((int)(y[i].D), (int)(x[i].D), out cell_id, out ListCellID);
                        Point cellID = ListCellID[0];

                        // 儲存影像
                        HOperatorSet.WriteImage(dispImg, "tiff", 0, Directory_ + "(" + cellID.X + "," + cellID.Y + ")");
                        dispImg.Dispose();
                    }
                    catch
                    {
                        // 停止並退出Parallel.For
                        loopState.Stop();
                        b_error_Parallel = true;
                        return;
                    }
                });

                if (b_error_Parallel) // 判斷平行運算中，是否遇到例外狀況
                    return false;

                b_status_ = true;
            }
            catch (Exception ex)
            { }

            return b_status_;
        }
        
        public static bool Load(string directory_, LocateMethod locate_method, out cls_AICellImg_Recipe Recipe) // (20190906) Jeff Revised!
        {
            bool b_status_ = false;
            Recipe = new cls_AICellImg_Recipe();
            try
            {
                if (File.Exists(directory_ + "AICellImg_Recipe.xml"))
                {
                    if (LoadXML(directory_ + "AICellImg_Recipe.xml", out Recipe))
                    {
                        // 更新 Cell Region
                        Recipe.Update_CellReg(locate_method);
                        b_status_ = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }

            return b_status_;
        }

        /// <summary>
        /// 載入XML檔 (AICellImg_Recipe.xml)
        /// </summary>
        /// <param name="PathFile"></param>
        /// <param name="Recipe"></param>
        /// <returns></returns>
        public static bool LoadXML(string PathFile, out cls_AICellImg_Recipe Recipe) // (20190906) Jeff Revised!
        {
            bool b_status_ = false;
            Recipe = new cls_AICellImg_Recipe();
            try
            {
                XmlSerializer XmlS = new XmlSerializer(Recipe.GetType());
                Stream S = File.Open(PathFile, FileMode.Open);
                Recipe = (cls_AICellImg_Recipe)XmlS.Deserialize(S);
                S.Close();
                b_status_ = true;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }

            return b_status_;
        }

    }

    /// <summary>
    /// AI分圖標註種類
    /// </summary>
    public enum enu_Label_AICellImg // (20200429) Jeff Revised!
    {
        OK,
        NG,
        NONE
    }



}
