using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HalconDotNet;
using System.Drawing;

namespace DeltaSubstrateInspector.src.Modules
{
    [Serializable] // (20190723) Jeff Revised!
    public class DefectsResult // (20200429) Jeff Revised!
    {
        #region 屬性

        /// <summary>
        /// 瑕疵名稱
        /// </summary>
        public string Name = ""; // (20190727) Jeff Revised!

        /// <summary>
        /// 優先權: 0, 1, 2, ...
        /// Note: 各類型瑕疵之優先權不可相同
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// 顏色
        /// </summary>
        public string Str_Color_Halcon { get; set; }

        /// <summary>
        /// NG數量
        /// </summary>
        public int Count_NG { get; set; } = 0; // (20200429) Jeff Revised!

        #endregion

        #region 檢測結果

        /// <summary>
        /// 當前基板之該類型瑕疵所在cell區域 (原始)
        /// </summary>
        public HObject all_intersection_defect_Origin = null;

        /// <summary>
        /// 當前基板之該類型瑕疵所在cell區域 (原始) (Cell region)
        /// </summary>
        public HObject all_intersection_defect_Origin_CellReg = null;

        /// <summary>
        /// 當前基板之該類型瑕疵所在cell區域 (瑕疵優先權)
        /// </summary>
        public HObject all_intersection_defect_Priority = null;

        /// <summary>
        /// 當前基板之該類型瑕疵所在cell區域 (瑕疵優先權) (Cell region)
        /// </summary>
        public HObject all_intersection_defect_Priority_CellReg = null;

        /// <summary>
        /// 當前基板之該類型瑕疵所在cell區域 (人工覆判)
        /// </summary>
        public HObject all_intersection_defect_Recheck = null;

        /// <summary>
        /// 當前基板之該類型瑕疵所在cell座標 (原始)
        /// </summary>
        public List<Point> all_defect_id_Origin = new List<Point>();

        /// <summary>
        /// 當前基板之該類型瑕疵所在cell座標 (瑕疵優先權)
        /// </summary>
        public List<Point> all_defect_id_Priority = new List<Point>();

        /// <summary>
        /// 當前基板之該類型瑕疵所在cell座標 (人工覆判)
        /// </summary>
        public List<Point> all_defect_id_Recheck = new List<Point>();

        #endregion
        
        public DefectsResult() { }

        #region 方法

        /// <summary>
        /// 代替 建構子，初始化 之功能 
        /// Note: 建構式有參數輸入時，會無法儲存到XML!
        /// </summary>
        /// <param name="name"></param>
        /// <param name="priority"></param>
        /// <param name="str_Color_Halcon"></param>
        public void DefectsResult_Constructor(string name = "", int priority = 0, string str_Color_Halcon = "#ff0000ff") // (20190727) Jeff Revised!
        {
            this.Name = name;
            this.Priority = priority;
            this.Str_Color_Halcon = str_Color_Halcon;
            this.Initialize();
        }

        /// <summary>
        /// 代替 建構子，初始化 之功能  (靜態方法)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="priority"></param>
        /// <param name="str_Color_Halcon"></param>
        /// <returns></returns>
        public static DefectsResult Get_DefectsResult(string name = "", int priority = 0, string str_Color_Halcon = "#ff0000ff") // (20190727) Jeff Revised!
        {
            DefectsResult dr = new DefectsResult();
            dr.DefectsResult_Constructor(name, priority, str_Color_Halcon);
            return dr;
        }

        /// <summary>
        /// 代替 建構子，初始化 之功能  (靜態方法)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="count_NG"></param>
        /// <returns></returns>
        public static DefectsResult Get_DefectsResult2(string name = "", int count_NG = 0) // (20200429) Jeff Revised!
        {
            DefectsResult dr = new DefectsResult();
            dr.Name = name;
            dr.Count_NG = count_NG;
            return dr;
        }

        /// <summary>
        /// 檢測結果初始化
        /// </summary>
        public void Initialize()
        {
            Release();
            HOperatorSet.GenEmptyRegion(out all_intersection_defect_Origin);
            HOperatorSet.GenEmptyRegion(out all_intersection_defect_Origin_CellReg);
            HOperatorSet.GenEmptyRegion(out all_intersection_defect_Priority);
            HOperatorSet.GenEmptyRegion(out all_intersection_defect_Priority_CellReg);
            HOperatorSet.GenEmptyRegion(out all_intersection_defect_Recheck);
            all_defect_id_Origin.Clear();
            all_defect_id_Origin = new List<Point>();
            all_defect_id_Priority.Clear();
            all_defect_id_Priority = new List<Point>();
            all_defect_id_Recheck.Clear();
            all_defect_id_Recheck = new List<Point>();
        }

        /// <summary>
        /// 釋放記憶體
        /// </summary>
        /// <param name="b_Origin"></param>
        /// <param name="b_Priority"></param>
        /// <param name="b_Recheck"></param>
        public void Release(bool b_Origin = true, bool b_Priority = true, bool b_Recheck = true)
        {
            if (b_Origin)
            {
                Extension.HObjectMedthods.ReleaseHObject(ref all_intersection_defect_Origin);
                Extension.HObjectMedthods.ReleaseHObject(ref all_intersection_defect_Origin_CellReg);
            }
            if (b_Priority)
            {
                Extension.HObjectMedthods.ReleaseHObject(ref all_intersection_defect_Priority);
                Extension.HObjectMedthods.ReleaseHObject(ref all_intersection_defect_Priority_CellReg);
            }
            if (b_Recheck)
                Extension.HObjectMedthods.ReleaseHObject(ref all_intersection_defect_Recheck);
        }

        /// <summary>
        /// 設定 Str_Color_Halcon (Color類別 轉 Halcon 顏色格式)
        /// </summary>
        public void SetColor(Color ConvertColor) // (20200429) Jeff Revised!
        {
            this.Str_Color_Halcon = clsStaticTool.GetHalconColor(ConvertColor);
        }

        /// <summary>
        /// 將 Halcon 顏色格式轉換成 Color類別
        /// </summary>
        /// <returns></returns>
        public Color GetColor() // (20200429) Jeff Revised!
        {
            return clsStaticTool.GetSystemColor(this.Str_Color_Halcon);
        }
        
        #endregion
    }
}
