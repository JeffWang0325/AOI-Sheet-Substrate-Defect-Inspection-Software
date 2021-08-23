using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeltaSubstrateInspector.src.Roles;
using System.Xml;
using static DeltaSubstrateInspector.FileSystem.FileSystem;
using System.IO;

using HalconDotNet;
using System.Diagnostics;
using System.Drawing;

using DAVS;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace DeltaSubstrateInspector.src.Modules.NewInspectModule.ResistPanel
{
    /// <summary>
    /// 總參數
    /// </summary>
    [Serializable]
    public class clsRecipe
    {
        public clsRecipe() { }

        #region 進階設定內參數

        // 測試模式設定
        public bool TestModeEnabled = false;
        public int TestModeType = 0; // 0: 忽略所有檢測, 1: 只回傳對位中心, 2: 只回傳切割範圍
        // AOI儲存影像設定
        public bool SaveAOIImgEnabled = false;
        public int SaveAOIImgType = 0; // 0:儲存所有影像, 1:儲存NG影像 , 2:儲存OK影像
        // AI Type
        public int DAVSInspType = 0; // 0:全部檢測, 1:只檢測NG , 2:只檢測OK
        // AI 影像ID
        public int AIImageID = 0;
        // 參數設定
        private int imgCount = 2;
        /// <summary>
        /// 影像數量
        /// </summary>
        public int ImgCount
        {
            get { return imgCount; }
            set { imgCount = value; } // 加入 set 才可以儲存到XML檔
        }

        private string type_BlackStripe = "Horizontal"; // "Horizontal": 水平, "Vertical": 垂直
        /// <summary>
        /// 黑條方向
        /// </summary>
        public string Type_BlackStripe // (20191112) Jeff Revised!
        {
            get { return type_BlackStripe; }
            set { type_BlackStripe = value; } // 加入 set 才可以儲存到XML檔
        }

        // 計算每種瑕疵在Cell的中心位置
        public bool Enabled_Compute_df_CellCenter = false;
        // 扭正影像
        public bool Enabled_rotate = false;

        private bool b_AOI_absolNG = false;
        /// <summary>
        /// 是否啟用AOI判斷絕對NG
        /// </summary>
        public bool B_AOI_absolNG // (20190805) Jeff Revised!
        {
            get { return b_AOI_absolNG; }
            set { b_AOI_absolNG = value; } // 加入 set 才可以儲存到XML檔
        }

        #endregion

        #region AOI參數

        private clsAOIParam_InitialPosition positionParam_Initial = new clsAOIParam_InitialPosition(); // 初定位 之參數
        /// <summary>
        /// 初定位 之參數
        /// </summary>
        public clsAOIParam_InitialPosition PositionParam_Initial
        {
            get { return positionParam_Initial; }
            set { positionParam_Initial = value; } // 加入 set 才可以儲存到XML檔
        }

        private clsAOIParam_CellSeg_CL inspParam_CellSeg_CL = new clsAOIParam_CellSeg_CL(); // Cell分割 (同軸光) 之參數
        /// <summary>
        /// Cell分割 (同軸光) 之參數
        /// </summary>
        public clsAOIParam_CellSeg_CL InspParam_CellSeg_CL
        {
            get { return inspParam_CellSeg_CL; }
            set { inspParam_CellSeg_CL = value; } // 加入 set 才可以儲存到XML檔
        }

        private clsAOIParam_CellSeg_BL inspParam_CellSeg_BL = new clsAOIParam_CellSeg_BL(); // Cell分割 (背光) 之參數
        /// <summary>
        /// Cell分割 (背光) 之參數
        /// </summary>
        public clsAOIParam_CellSeg_BL InspParam_CellSeg_BL
        {
            get { return inspParam_CellSeg_BL; }
            set { inspParam_CellSeg_BL = value; } // 加入 set 才可以儲存到XML檔
        }

        private clsAOIParam_Df_CellReg inspParam_Df_CellReg = new clsAOIParam_Df_CellReg(); // 瑕疵Cell判斷範圍 之參數
        /// <summary>
        /// 瑕疵Cell判斷範圍 之參數
        /// </summary>
        public clsAOIParam_Df_CellReg InspParam_Df_CellReg
        {
            get { return inspParam_Df_CellReg; }
            set { inspParam_Df_CellReg = value; } // 加入 set 才可以儲存到XML檔
        }

        private clsAOIParam_Cell_BlackStripe inspParam_WhiteBlob_FL = new clsAOIParam_Cell_BlackStripe(); // 檢測黑條內白點 (正光) (Frontal light) 之參數
        /// <summary>
        /// 檢測黑條內白點 (正光) (Frontal light) 之參數
        /// </summary>
        public clsAOIParam_Cell_BlackStripe InspParam_WhiteBlob_FL
        {
            get { return inspParam_WhiteBlob_FL; }
            set { inspParam_WhiteBlob_FL = value; } // 加入 set 才可以儲存到XML檔
        }

        private clsAOIParam_Cell_BlackStripe inspParam_WhiteBlob_CL = new clsAOIParam_Cell_BlackStripe(); // 檢測黑條內白點 (同軸光) (Coaxial light) 之參數
        /// <summary>
        /// 檢測黑條內白點 (同軸光) (Coaxial light) 之參數
        /// </summary>
        public clsAOIParam_Cell_BlackStripe InspParam_WhiteBlob_CL
        {
            get { return inspParam_WhiteBlob_CL; }
            set { inspParam_WhiteBlob_CL = value; } // 加入 set 才可以儲存到XML檔
        }

        private clsAOIParam_Cell_BlackStripe inspParam_Line_FL = new clsAOIParam_Cell_BlackStripe(); // 檢測絲狀異物 (正光) (Frontal Light) 之參數
        /// <summary>
        /// 檢測絲狀異物 (正光) (Frontal Light) 之參數
        /// </summary>
        public clsAOIParam_Cell_BlackStripe InspParam_Line_FL
        {
            get { return inspParam_Line_FL; }
            set { inspParam_Line_FL = value; } // 加入 set 才可以儲存到XML檔
        }

        private clsAOIParam_Cell_BlackStripe_2Images inspParam_Line_FLCL = new clsAOIParam_Cell_BlackStripe_2Images(); // 檢測絲狀異物 (正光&同軸光) 之參數
        /// <summary>
        /// 檢測絲狀異物 (正光&同軸光) 之參數
        /// </summary>
        public clsAOIParam_Cell_BlackStripe_2Images InspParam_Line_FLCL
        {
            get { return inspParam_Line_FLCL; }
            set { inspParam_Line_FLCL = value; } // 加入 set 才可以儲存到XML檔
        }

        private clsAOIParam_BlackCrack inspParam_BlackCrack_FL = new clsAOIParam_BlackCrack(); // 檢測白條內黑點 (Black Crack) (Frontal light) 之參數
        /// <summary>
        /// 檢測白條內黑點 (Black Crack) (Frontal light) 之參數
        /// </summary>
        public clsAOIParam_BlackCrack InspParam_BlackCrack_FL
        {
            get { return inspParam_BlackCrack_FL; }
            set { inspParam_BlackCrack_FL = value; } // 加入 set 才可以儲存到XML檔
        }

        private clsAOIParam_BlackCrack inspParam_BlackCrack_CL = new clsAOIParam_BlackCrack(); // 檢測白條內黑點 (Black Crack) (Coaxial light) 之參數
        /// <summary>
        /// 檢測白條內黑點 (Black Crack) (Coaxial light) 之參數
        /// </summary>
        public clsAOIParam_BlackCrack InspParam_BlackCrack_CL
        {
            get { return inspParam_BlackCrack_CL; }
            set { inspParam_BlackCrack_CL = value; } // 加入 set 才可以儲存到XML檔
        }

        private clsAOIParam_BlackBlob_CL inspParam_BlackBlob_CL = new clsAOIParam_BlackBlob_CL(); // 檢測 Black Blob (Coaxial Light) 之參數
        /// <summary>
        /// 檢測 Black Blob (Coaxial Light) 之參數
        /// </summary>
        public clsAOIParam_BlackBlob_CL InspParam_BlackBlob_CL
        {
            get { return inspParam_BlackBlob_CL; }
            set { inspParam_BlackBlob_CL = value; } // 加入 set 才可以儲存到XML檔
        }

        private clsAOIParam_Cell_BlackStripe inspParam_BlackBlob2_CL = new clsAOIParam_Cell_BlackStripe(); // 檢測 Black Blob2 (Coaxial Light) 之參數
        /// <summary>
        /// 檢測 Black Blob2 (Coaxial Light) 之參數
        /// </summary>
        public clsAOIParam_Cell_BlackStripe InspParam_BlackBlob2_CL
        {
            get { return inspParam_BlackBlob2_CL; }
            set { inspParam_BlackBlob2_CL = value; } // 加入 set 才可以儲存到XML檔
        }

        private clsAOIParam_Cell_BlackStripe inspParam_Dirty_CL = new clsAOIParam_Cell_BlackStripe(); // 檢測汙染 (Coaxial Light) 之參數
        /// <summary>
        /// 檢測汙染 (Coaxial Light) 之參數
        /// </summary>
        public clsAOIParam_Cell_BlackStripe InspParam_Dirty_CL
        {
            get { return inspParam_Dirty_CL; }
            set { inspParam_Dirty_CL = value; } // 加入 set 才可以儲存到XML檔
        }

        private clsAOIParam_Dirty_FLCL inspParam_Dirty_FLCL = new clsAOIParam_Dirty_FLCL(); // 檢測汙染 (Dirty) (Frontal light & Coaxial Light) 之參數
        /// <summary>
        /// 檢測汙染 (Dirty) (Frontal light & Coaxial Light) 之參數
        /// </summary>
        public clsAOIParam_Dirty_FLCL InspParam_Dirty_FLCL
        {
            get { return inspParam_Dirty_FLCL; }
            set { inspParam_Dirty_FLCL = value; } // 加入 set 才可以儲存到XML檔
        }

        private clsAOIParam_BrightBlob_BL inspParam_BrightBlob_BL = new clsAOIParam_BrightBlob_BL(); // 檢測電極區白點 (Back Light) 之參數
        /// <summary>
        /// 檢測電極區白點 (Back Light) 之參數
        /// </summary>
        public clsAOIParam_BrightBlob_BL InspParam_BrightBlob_BL
        {
            get { return inspParam_BrightBlob_BL; }
            set { inspParam_BrightBlob_BL = value; } // 加入 set 才可以儲存到XML檔
        }

        private clsAOIParam_LackAngle_BL inspParam_LackAngle_BL = new clsAOIParam_LackAngle_BL(); // 檢測保缺角 (LackAngle) (Back Light) 之參數
        /// <summary>
        /// 檢測保缺角 (LackAngle) (Back Light) 之參數
        /// </summary>
        public clsAOIParam_LackAngle_BL InspParam_LackAngle_BL
        {
            get { return inspParam_LackAngle_BL; }
            set { inspParam_LackAngle_BL = value; } // 加入 set 才可以儲存到XML檔
        }

        private clsAOIParam_Cell_BlackStripe inspParam_LackAngle_SL = new clsAOIParam_Cell_BlackStripe(); // 檢測保缺角 (Side Light) 之參數
        /// <summary>
        /// 檢測保缺角 (Side Light) 之參數
        /// </summary>
        public clsAOIParam_Cell_BlackStripe InspParam_LackAngle_SL
        {
            get { return inspParam_LackAngle_SL; }
            set { inspParam_LackAngle_SL = value; } // 加入 set 才可以儲存到XML檔
        }

        [Serializable] // 有加或沒加都可以正常執行!
        public class clsAOIParam_InitialPosition
        {
            /********** 擷取有效檢測區域 **********/
            /* 白色區域之外接矩形 (For Frontal Light Image) */
            public int MinGray_WhiteReg_FL = 250;
            public bool Enabled_closing_ValidReg_FL = false; // (20191112) Jeff Revised!
            public int W_closing_ValidReg_FL = 1; // (20191112) Jeff Revised!
            public int H_closing_ValidReg_FL = 1; // (20191112) Jeff Revised!
            public int MinArea_Inspect = 1100000;
            public int W_opening_ValidReg_FL = 150;
            public int H_opening_ValidReg_FL = 1; // (20191112) Jeff Revised!
            
            /* 擷取有效檢測區域 (For Back Light Image) */
            public string str_Type_ValidReg = "BL"; // str_Type_ValidReg: BL, FL (20191112) Jeff Revised!
            public int MaxGray_InspectReg_BL = 175;
            // 消除三角形誤判
            public int W_opening_ValidReg_BL = 215;
            public int H_opening_ValidReg_BL = 1; // (20191112) Jeff Revised!
            // 尺寸篩選 (20191112) Jeff Revised!
            public int MinW_BlackStripe_ValidReg = 22;
            public int MaxW_BlackStripe_ValidReg = 55;
            public int MinH_BlackStripe_ValidReg = 250;
            public int MaxH_BlackStripe_ValidReg = 2160;
            // Morphology (20191112) Jeff Revised!
            public int W_dila_ValidReg = 35;
            public int H_dila_ValidReg = 1;

            /* 消除掉非檢測Cell (黑色長條邊緣) */
            public bool Enabled_NonInspectCell = true;
            public int L_NonInspect = 897;
            public int L_NonInspect_Wsmall = 897;
            public int L_NonInspect_Wsmall3 = 897;

            /* 消除掉背光白色背景區域 (For Back Light Image) */
            public int MinGray_WhiteReg_BL = 210;
            public int MinArea_WhiteReg_BL = 50000;
            public int Radius_dila_WhiteReg_BL = 5;

            /* 擷取 黑色長條區域 (For Frontal Light Image) & 寬度縮小/放大 黑色長條區域 */
            // 影像ID
            public int ImageIndex_SegBlackStripe = 0; // (20190816) Jeff Revised!
            // 二值化處理
            public int MinGray_SegBlackStripe = 0;
            public int MaxGray_SegBlackStripe = 140;
            // 填補 縫隙 & 孔洞
            public int W_closing_SegBlackStripe = 15;
            public int H_closing_SegBlackStripe = 1; // (20190816) Jeff Revised!
            // 消除掉 Black Crack
            public int MaxWidth_BlackCrack = 25;
            public int H_opening_BlackCrack = 1; // (20190816) Jeff Revised!
            // 長寬篩選
            public int MinW_BlackStripe = 60;
            public int MaxW_BlackStripe = 5000;
            public int MinH_BlackStripe = 35;
            public int MaxH_BlackStripe = 100;
            public int W_dila_BlackStripe = 1800;
            public int H_dila_BlackStripe = 7;
            // 寬度縮小 黑色長條區域
            public int H_ero_BlackStripe_small = 17;
            // 寬度放大 黑色長條區域
            //public int H_dila_BlackStripe_large = 1;

            /* 修正有效檢測區域 */
            public bool Enabled_ValidReg_Revised = true; // (20191112) Jeff Revised!
            // 填補 邊緣縫隙 & 內部孔洞
            public int W_closing_Inspect = 1; // (20191112) Jeff Revised!
            public int H_closing_Inspect = 15;

            public clsAOIParam_InitialPosition() { }
        }

        [Serializable] // 有加或沒加都可以正常執行!
        public class clsAOIParam_CellSeg_CL
        {
            // 是否啟動
            public bool Enabled = true;

            // 影像ID
            public int ImageIndex = 1;

            /* Cell Segmentation */
            public bool Enabled_removeDef = false;
            public int MinGray_removeDef = 250;
            public int MaxGray_removeDef = 255;
            // 影像增強
            public bool Enabled_equHisto = true; // 是否做直方圖等化 (20191112) Jeff Revised!
            public bool Enabled_equHisto_part = true; // 是否做分區直方圖等化 (20191112) Jeff Revised!
            public int count_col = 5;
            // 二值化處理
            public string str_BinaryImg = "固定閥值"; // str_BinaryImg: 固定閥值, 動態閥值
            public int MinGray_CellSeg = 130;
            public int MaxGray_CellSeg = 255;
            public int MaskWidth = 33;
            public int MaskHeight = 95;
            public int Offset = 25;
            public string str_LightDark = "light"; // str_LightDark: dark, light, equal, not_equal

            // Morphology (Unit: pixel)
            public int W_opening1_CellSeg = 1;
            public int H_opening1_CellSeg = 3;
            public int W_closing_CellSeg = 9;
            public int H_closing_CellSeg = 9;
            public int W_opening2_CellSeg = 1;
            public int H_opening2_CellSeg = 21;
            public int W_dila_CellSeg = 1;
            public int H_dila_CellSeg = 75;

            // Cell分割模式
            public string str_ModeCellSeg = "mode2"; // str_ModeCellSeg: mode1, mode2

            // Cell尺寸篩選 (Unit: μm)
            public int MinHeight_CellSeg = 381;
            public int MaxHeight_CellSeg = 450;
            public int MinWidth_CellSeg = 339;
            public int MaxWidth_CellSeg = 416;

            public bool Enabled_RemainValidCells = false;

            public clsAOIParam_CellSeg_CL() { }
        }

        [Serializable] // 有加或沒加都可以正常執行!
        public class clsAOIParam_CellSeg_BL
        {
            // 是否啟動
            public bool Enabled = false;

            // 影像ID
            public int ImageIndex = 2;

            /* Cell Segmentation */
            public bool Enabled_removeDef = true;
            public int MinGray_removeDef = 115;
            public int MaxGray_removeDef = 255;
            // 影像增強
            public bool Enabled_equHisto = true; // 是否做直方圖等化 (20191112) Jeff Revised!
            public bool Enabled_equHisto_part = true; // 是否做分區直方圖等化 (20191112) Jeff Revised!
            public int count_col = 15;
            // 二值化處理
            public string str_BinaryImg = "動態閥值"; // str_BinaryImg: 固定閥值, 動態閥值
            public int MinGray_CellSeg = 0;
            public int MaxGray_CellSeg = 80;
            public int MaskWidth = 33;
            public int MaskHeight = 95;
            public int Offset = 25;
            public string str_LightDark = "dark"; // str_LightDark: dark, light, equal, not_equal

            // Morphology (Unit: pixel)
            public int W_opening1_CellSeg = 1;
            public int H_opening1_CellSeg = 7;
            public int W_closing_CellSeg = 9;
            public int H_closing_CellSeg = 9;
            public int W_opening2_CellSeg = 1;
            public int H_opening2_CellSeg = 31;
            public int W_dila_CellSeg = 1;
            public int H_dila_CellSeg = 45;

            // Cell分割模式
            public string str_ModeCellSeg = "mode2"; // str_ModeCellSeg: mode1, mode2

            // Cell尺寸篩選 (Unit: μm)
            public int MinHeight_CellSeg = 242;
            public int MaxHeight_CellSeg = 380;
            public int MinWidth_CellSeg = 124;
            public int MaxWidth_CellSeg = 242;

            public bool Enabled_RemainValidCells = false;

            public clsAOIParam_CellSeg_BL() { }
        }

        [Serializable] // 有加或沒加都可以正常執行!
        public class clsAOIParam_Df_CellReg
        {
            // 影像檢測範圍 (Unit: pixel)
            public bool Enabled_InspReg = false;
            public int W_InspReg = 2784;
            public int H_InspReg = 2160;

            // 瑕疵Cell判斷範圍 (Unit: μm)
            public int W_Cell = 483;
            public int H_Cell = 932;

            public clsAOIParam_Df_CellReg() { }
        }

        [Serializable] // 有加或沒加都可以正常執行!
        public class clsAOIParam_BlackCrack
        {
            // 是否啟動
            public bool Enabled = true;

            // 影像ID
            public int ImageIndex = 1;

            /* 檢測 Black Crack */
            // 設定檢測ROI (Unit: μm)
            public bool Enabled_Wsmall = false;
            public string str_L_NonInspect = "1";
            public bool Enabled_BlackStripe = true;
            public string str_ModeBlackStripe = "mode1";
            /// <summary>
            /// 黑條電阻寬度
            /// </summary>
            public int H_BlackStripe = 414;
            /// <summary>
            /// 黑條電阻寬度
            /// </summary>
            public int H_BlackStripe_mode2 = 0;

            // 二值化處理
            public int MinGray_BlackCrack = 0;
            public int MaxGray_BlackCrack = 195;

            // 設定檢測ROI 2 (Unit: μm)
            public bool Enabled_Cell = false;
            public int W_Cell = 414;
            public int H_Cell = 449;
            public string str_segMode = "ROI2"; // str_segMode: ROI2, 連通區域 (20190529) Jeff Revised!

            // 瑕疵尺寸篩選 (Unit: μm)
            public bool Enabled_Height = true;
            public int MinHeight_defect = 48;
            public int MaxHeight_defect = 9999;
            public bool Enabled_Width = true;
            public int MinWidth_defect = 48;
            public int MaxWidth_defect = 9999;
            public string str_op1 = "or";

            public bool Enabled_Area = true;
            public int MinArea_defect = 1380;
            public int MaxArea_defect = 99999;
            public string str_op2 = "or";

            // AOI判斷絕對NG (20190805) Jeff Revised!
            public clsAOIParam_absolNG cls_absolNG = new clsAOIParam_absolNG();

            public clsAOIParam_BlackCrack() { }
        }

        [Serializable] // 有加或沒加都可以正常執行!
        public class clsAOIParam_BlackBlob_CL
        {
            // 是否啟動
            public bool Enabled = true;

            // 影像ID
            public int ImageIndex = 1;

            /* 檢測 Black Blob (Coaxial Light) */
            //// 大顆 Black Blob
            public int MaxGray_BigBlackBlob_CL = 85;
            public double MinArea_BigBlackBlob_CL = 30.0;
            public double MaxArea_BigBlackBlob_CL = 10000.0;

            //// 小顆 Black Blob
            public double Zoom_DownScale = 2.0;
            // 去除水平線
            public bool Enabled_Reduce_Hor = true;
            public double W_Filter_FFT_y = 20.0;
            public double Shift_FFT_y = 20.0;
            // 去除垂直線
            public bool Enabled_Reduce_Ver = false;
            public double W_Filter_FFT_x = 10.0;
            public double Shift_FFT_x = 20.0;
            // 檢測 Black Blob
            public int MaxGray_BlackBlob_CL = 120;

            // 瑕疵尺寸篩選 (Unit: μm)
            public double MinHeight_defect = 5.0;
            public double MaxHeight_defect = 9999.0;
            public double MinWidth_defect = 5.0;
            public double MaxWidth_defect = 9999.0;

            // 瑕疵Region內至少有一個pixel之灰階值低於一個數值
            public int Gray_Dark_CL = 90;

            // AOI判斷絕對NG (20190805) Jeff Revised!
            public clsAOIParam_absolNG cls_absolNG = new clsAOIParam_absolNG();

            public clsAOIParam_BlackBlob_CL() { }
        }

        [Serializable] // 有加或沒加都可以正常執行!
        public class clsAOIParam_Cell_BlackStripe
        {
            // 是否啟動
            public bool Enabled = false;

            // 影像ID
            public int ImageIndex = 1;

            /* 檢測 Black Blob2 (Coaxial Light) */
            // 設定檢測ROI (Unit: μm)
            public bool Enabled_Wsmall = false;
            public string str_L_NonInspect = "1";
            public bool Enabled_Cell = true;
            public int W_Cell = 414;
            public int H_Cell = 449;
            public bool Enabled_BypassReg = true;
            public int x_BypassReg = -104;
            public int y_BypassReg = 69;
            public int W_BypassReg = 276;
            public int H_BypassReg = 138;
            public bool Enabled_BlackStripe = false;
            public string str_ModeBlackStripe = "mode1";
            public int H_BlackStripe = 414;
            public int H_BlackStripe_mode2 = 0;

            // 二值化處理
            public string str_BinaryImg_Cell = "固定閥值"; // str_BinaryImg_Cell: 固定閥值, 動態閥值
            public int MinGray_Cell = 0;
            public int MaxGray_Cell = 95;
            public int MaskWidth_Cell = 200;
            public int MaskHeight_Cell = 200;
            public string str_LightDark_Cell = "light"; // str_LightDark_Cell: dark, light, equal, not_equal
            public int Offset_Cell = 10;
            public string str_BinaryImg_Resist = "固定閥值"; // str_BinaryImg_Resist: 固定閥值, 動態閥值
            public int MinGray_Resist = 0;
            public int MaxGray_Resist = 95;
            public int MaskWidth_Resist = 45;
            public int MaskHeight_Resist = 43;
            public string str_LightDark_Resist = "light"; // str_LightDark_Resist: dark, light, equal, not_equal
            public int Offset_Resist = 105;

            // 規格2 (20190411) Jeff Revised!
            public int count_BinImg_Cell = 1;
            public int MinGray_2_Cell = 0;
            public int MaxGray_2_Cell = 95;
            public int MaskWidth_2_Cell = 200;
            public int MaskHeight_2_Cell = 200;
            public string str_LightDark_2_Cell = "light"; // str_LightDark_2_Cell: dark, light, equal, not_equal
            public int Offset_2_Cell = 10;
            public int count_BinImg_Resist = 1;
            public int MinGray_2_Resist = 0;
            public int MaxGray_2_Resist = 95;
            public int MaskWidth_2_Resist = 45;
            public int MaskHeight_2_Resist = 43;
            public string str_LightDark_2_Resist = "light"; // str_LightDark_2_Resist: dark, light, equal, not_equal
            public int Offset_2_Resist = 105;

            public bool Enabled_Gray_select = true;
            public bool Enabled_Gray_select_Cell = false;
            public bool Enabled_Gray_select_Resist = false;
            public string str_Gray_select = "min";
            public string str_Gray_select_Cell = "min";
            public string str_Gray_select_Resist = "min";
            public int Gray_select = 60;
            public int Gray_select_Cell = 60;
            public int Gray_select_Resist = 60;

            // 設定檢測ROI 2 (Unit: μm)
            public bool Enabled_Cell2 = false;
            public int W_Cell2 = 414;
            public int H_Cell2 = 449;

            // 瑕疵尺寸篩選 (Unit: μm)
            public int count_op1 = 1;

            public bool Enabled_Height = true;
            public int MinHeight_defect = 48;
            public int MaxHeight_defect = 9999;
            public bool Enabled_Width = true;
            public int MinWidth_defect = 48;
            public int MaxWidth_defect = 9999;
            public string str_op1 = "or";

            public bool Enabled_Height_op1_2 = true;
            public int MinHeight_defect_op1_2 = 48;
            public int MaxHeight_defect_op1_2 = 9999;
            public bool Enabled_Width_op1_2 = true;
            public int MinWidth_defect_op1_2 = 48;
            public int MaxWidth_defect_op1_2 = 9999;
            public string str_op1_2 = "or";

            public bool Enabled_Area = true;
            public int MinArea_defect = 1380;
            public int MaxArea_defect = 99999;
            public string str_op2 = "or";

            // AOI判斷絕對NG (20190805) Jeff Revised!
            public clsAOIParam_absolNG cls_absolNG = new clsAOIParam_absolNG();

            public clsAOIParam_Cell_BlackStripe() { }
        }

        [Serializable] // 有加或沒加都可以正常執行!
        public class clsAOIParam_Cell_BlackStripe_2Images
        {
            // 是否啟動
            public bool Enabled = true;

            /* 邏輯操作 */
            public string str_op1_2Images = "and"; // Include "and", "or", "not1" and "not2"

            // 瑕疵尺寸篩選 (Unit: μm)
            public int count_op1 = 1;

            public bool Enabled_Height = true;
            public int MinHeight_defect = 48;
            public int MaxHeight_defect = 9999;
            public bool Enabled_Width = true;
            public int MinWidth_defect = 48;
            public int MaxWidth_defect = 9999;
            public string str_op1 = "or";

            public bool Enabled_Height_op1_2 = true;
            public int MinHeight_defect_op1_2 = 48;
            public int MaxHeight_defect_op1_2 = 9999;
            public bool Enabled_Width_op1_2 = true;
            public int MinWidth_defect_op1_2 = 48;
            public int MaxWidth_defect_op1_2 = 9999;
            public string str_op1_2 = "or";

            public bool Enabled_Area = true;
            public int MinArea_defect = 1380;
            public int MaxArea_defect = 99999;
            public string str_op2 = "or";

            /* 消除誤判區域 (20190417) Jeff Revised! */
            public bool Enabled_BypassReg = false;
            public int x_BypassReg = -104;
            public int y_BypassReg = 69;
            public int W_BypassReg = 276;
            public int H_BypassReg = 138;

            // 瑕疵尺寸篩選 (Unit: μm)
            public int count_op3 = 1;

            public bool Enabled_Height_op3_1 = true;
            public int MinHeight_defect_op3_1 = 48;
            public int MaxHeight_defect_op3_1 = 9999;
            public bool Enabled_Width_op3_1 = true;
            public int MinWidth_defect_op3_1 = 48;
            public int MaxWidth_defect_op3_1 = 9999;
            public string str_op3_1 = "or";

            public bool Enabled_Height_op3_2 = true;
            public int MinHeight_defect_op3_2 = 48;
            public int MaxHeight_defect_op3_2 = 9999;
            public bool Enabled_Width_op3_2 = true;
            public int MinWidth_defect_op3_2 = 48;
            public int MaxWidth_defect_op3_2 = 9999;
            public string str_op3_2 = "or";

            // AOI判斷絕對NG (20190805) Jeff Revised!
            public clsAOIParam_absolNG cls_absolNG = new clsAOIParam_absolNG();

            public clsAOIParam_Cell_BlackStripe_2Images() { }
        }

        [Serializable] // 有加或沒加都可以正常執行!
        public class clsAOIParam_Dirty_FLCL
        {
            // 是否啟動
            public bool Enabled = true;

            // 影像ID
            public int ImageIndex1 = 0; // FL
            public int ImageIndex2 = 1; // CL

            // Dirty (汙染 or 異物): 黑色長條區域內的黑點 (Frontal light)
            public int MaxGray_Dirty_FL = 25;
            public int Gray_Dark_FL = 22;

            // Dirty (汙染 or 異物): 黑色長條區域內的白點 (Coaxial light)
            public int MinGray_Dirty_CL = 253;
            public int Gray_Bright_CL = 255;

            // 瑕疵尺寸篩選 (Unit: μm)
            public double MinHeight_defect = 5.0;
            public double MaxHeight_defect = 9999.0;
            public double MinWidth_defect = 5.0;
            public double MaxWidth_defect = 9999.0;

            // AOI判斷絕對NG (20190805) Jeff Revised!
            public clsAOIParam_absolNG cls_absolNG = new clsAOIParam_absolNG();

            public clsAOIParam_Dirty_FLCL() { }
        }

        [Serializable] // 有加或沒加都可以正常執行!
        public class clsAOIParam_BrightBlob_BL
        {
            // 是否啟動
            public bool Enabled = true;

            // 影像ID
            public int ImageIndex = 2;

            // 電極上 Bright Blob
            public int MinGray_Blob_Elect_BL = 210;

            // 瑕疵尺寸篩選 (Unit: μm)
            public int MinHeight_defect = 5;
            public int MaxHeight_defect = 9999;
            public int MinWidth_defect = 5;
            public int MaxWidth_defect = 9999;

            // AOI判斷絕對NG (20190805) Jeff Revised!
            public clsAOIParam_absolNG cls_absolNG = new clsAOIParam_absolNG();

            public clsAOIParam_BrightBlob_BL() { }
        }

        [Serializable] // 有加或沒加都可以正常執行!
        public class clsAOIParam_LackAngle_BL
        {
            // 是否啟動
            public bool Enabled = true;

            // 影像ID
            public int ImageIndex = 2;

            /* 檢測 Black Blob2 (Coaxial Light) */
            // 設定檢測ROI (Unit: μm)
            public bool Enabled_Cell = true;
            public int W_Cell = 414;
            public int H_Cell = 449;
            public bool Enabled_BypassReg = false;
            public int x_BypassReg = -104;
            public int y_BypassReg = 69;
            public int W_BypassReg = 276;
            public int H_BypassReg = 138;
            public bool Enabled_BlackStripe = true;
            public int H_BlackStripe = 483;

            // 分區做直方圖等化
            public bool Enabled_EquHisto = true;
            public int MinGray_Bright_BL = 200;
            public int MaxGray_Bright_BL = 255;
            public int w_Part = 2953;
            public int h_Part = 938;

            // 二值化處理
            public int MinGray_Cell = 150;
            public int MinGray_Resist = 150;
            public int Gray_select = 200;

            // 瑕疵尺寸篩選 (Unit: μm)
            public bool Enabled_Height = true;
            public int MinHeight_defect = 48;
            public int MaxHeight_defect = 9999;
            public bool Enabled_Width = true;
            public int MinWidth_defect = 48;
            public int MaxWidth_defect = 9999;
            public string str_op1 = "or";

            public bool Enabled_Area = true;
            public int MinArea_defect = 1200;
            public int MaxArea_defect = 99999;
            public string str_op2 = "or";

            // AOI判斷絕對NG (20190805) Jeff Revised!
            public clsAOIParam_absolNG cls_absolNG = new clsAOIParam_absolNG();

            public clsAOIParam_LackAngle_BL() { }
        }

        /// <summary>
        /// AOI判斷絕對NG 參數
        /// </summary>
        [Serializable] // 有加或沒加都可以正常執行!
        public class clsAOIParam_absolNG // (20190805) Jeff Revised!
        {
            // 是否啟用
            public bool Enabled = false;

            // 瑕疵灰階標準
            public bool Enabled_Gray_select = true;
            public string str_Gray_select = "min";
            public int Gray_select = 60;

            // 瑕疵尺寸篩選 (Unit: μm)
            public bool Enabled_Area = true;
            public int MinArea_defect = 1380;
            public int MaxArea_defect = 99999;
            public string str_op = "or";

            public clsAOIParam_absolNG() { }
        }

        #endregion
    }
    
    public class ResistPanelRole : InspectRole
    {
        #region 暫存變數

        static ResistPanelRole m_Singleton;
        private string Path = ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\ResistPanel\\";
        public HObject Pattern_Rect = null;
        public HObject InspRect = null;
        public HTuple ModelID = null;
        public clsDAVS DAVS = null;
        public clsDAVSParam DAVSParam = new clsDAVSParam();
        public clsSaveImg SaveAOIImg = null;

        #endregion

        #region 參數儲存區

        private clsRecipe param = new clsRecipe(); // 總參數
        /// <summary>
        /// 總參數
        /// </summary>
        public clsRecipe Param
        {
            get { return param; }
            set { param = value; }
        }

        #endregion

        public static ResistPanelRole GetSingleton()
        {
            if (m_Singleton == null)
            {
                m_Singleton = new ResistPanelRole();
            }
            return m_Singleton;
        }

        public ResistPanelRole()
        {

        }

        public void Init()
        {
            if (SaveAOIImg == null)
            {
                if (Param.SaveAOIImgEnabled) // (20181228) Jeff Revised!
                    SaveAOIImg = new clsSaveImg();
            }
        }

        public void Dispose()
        {
            if (DAVS != null)
            {
                DAVS.Dispose();
                DAVS = null;
            }
            if (SaveAOIImg != null)
            {
                SaveAOIImg.Dispose();
                SaveAOIImg = null;
            }
        }

        private void SaveHalconFile()
        {
            if (ModelID != null)
                HOperatorSet.WriteShapeModel(ModelID, Path + "ModelID");
        }
        
        private void ReadHalconFile()
        {
            if (File.Exists(Path + "ModelID"))
                HOperatorSet.ReadShapeModel(Path + "ModelID", out ModelID);
        }

        public override object Clone()
        {
            ResistPanelRole obj = ResistPanelRole.GetSingleton();

            // Copy the content of original object and setup a new one
            obj.inspect_name_ = this.inspect_name_;
            obj.method_ = this.method_;

            return obj;
        }

        public string GetRecipePath()
        {
            return ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\ResistPanel\\";
        }

        public static clsRecipe LoadXML(string PathFile)
        {
            clsRecipe Res = new clsRecipe();
            try
            {
                XmlSerializer XmlS = new XmlSerializer(Res.GetType());
                Stream S = File.Open(PathFile, FileMode.Open);
                Res = (clsRecipe)XmlS.Deserialize(S);
                S.Close();

            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
            }
            return Res;
        }

        public static void SaveXML(clsRecipe SrcProduct, string PathFile)
        {
            clsRecipe Product = clsStaticTool.Clone<clsRecipe>(SrcProduct);
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(PathFile));

            try
            {
                XmlSerializer XmlS = new XmlSerializer(Product.GetType());
                Stream S = File.Open(PathFile, FileMode.Create);
                XmlS.Serialize(S, Product);
                S.Close();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
            }
        }

        public override void save()
        {
            if (ModuleName == "") return; // 181031, andy
            
            XmlDocument doc = new XmlDocument();
            if (System.IO.File.Exists(ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\ResistPanel\\ResistPanel.xml"))
            {
                XmlElement element = doc.CreateElement("Parameter"); //181031, andy
                doc.AppendChild(element); //181031, andy
            }
            else
            {
                XmlElement element = doc.CreateElement("Parameter");
                doc.AppendChild(element);
            }
            XmlNode param = doc.SelectSingleNode("Parameter");
            XmlElement eleInspect = doc.CreateElement("Inspect");
            eleInspect.SetAttribute("Name", inspect_name_);
            param.AppendChild(eleInspect);
            
            #region 演算法參數儲存

            clsDAVSParam.SaveDAVSParam(DAVSParam, Path + "DAVS.xml");

            SaveHalconFile();

            SaveXML(this.Param, ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\ResistPanel\\ResistPanelParam.xml");

            #endregion
            
            if (!Directory.Exists(ModuleParamDirectory + RecipeFileDirectory + ModuleName))
            {
                Directory.CreateDirectory(ModuleParamDirectory + RecipeFileDirectory + ModuleName);
            }
            if (!Directory.Exists(ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\ResistPanel"))
            {
                Directory.CreateDirectory(ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\ResistPanel");
            }
            
            doc.Save(ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\ResistPanel\\ResistPanel.xml");
        }

        public override void load()
        {
            if (!System.IO.File.Exists(ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\ResistPanel\\ResistPanel.xml")) return;
            
            try
            {
                /*
                XmlDocument doc = new XmlDocument();
                doc.Load(ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\ResistPanel\\ResistPanel.xml");
                string andyMagicStr = "";
                string index = "//Inspect[@Name='" + andyMagicStr + "']";

                XmlNode node = doc.SelectSingleNode(index);
                */

                #region 演算法參數載入

                Path = ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\ResistPanel\\";
                ReadHalconFile(); // Pattern 模型參數
                DAVSParam = clsDAVSParam.LoadDAVSParam(Path + "DAVS.xml"); // AI參數
                this.Param = LoadXML(Path + "ResistPanelParam.xml"); // 切割與檢測參數
                if (DAVS != null)
                {
                    DAVS.Dispose();
                    DAVS = null;
                }
                if (SaveAOIImg != null)
                {
                    SaveAOIImg.Dispose();
                    SaveAOIImg = null;
                }
                this.SaveAOIImg = new clsSaveImg();
                this.DAVS = new clsDAVS(DAVSParam, GetRecipePath());
                //double debug = Param.Resolution; // For debug!

                #endregion

            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }
        }

    }

    /// <summary>
    /// AOI判斷絕對NG
    /// </summary>
    [Serializable]
    public class Cls_AOI_absolNG // (20190805) Jeff Revised!
    {
        public Cls_AOI_absolNG() { }

        /// <summary>
        /// 代替 建構子，初始化 之功能  (靜態方法)
        /// </summary>
        /// <param name="name_defect_"></param>
        /// <param name="enable_"></param>
        /// <param name="defectReg_"></param>
        /// <returns></returns>
        public static Cls_AOI_absolNG Get_Cls_AOI_absolNG(string name_defect_ = "", bool enable_ = false, HObject defectReg_ = null)
        {
            Cls_AOI_absolNG cls = new Cls_AOI_absolNG();
            cls.Name_defect = name_defect_;
            cls.Enable = enable_;
            if (defectReg_ == null)
                cls.Initialize();
            else
                cls.DefectReg = defectReg_;
            return cls;
        }

        private string name_defect = "";
        /// <summary>
        /// 瑕疵名稱
        /// </summary>
        public string Name_defect
        {
            get { return name_defect; }
            set { name_defect = value; }
        }

        private bool enable = false;
        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool Enable
        {
            get { return enable; }
            set { enable = value; }
        }

        private HObject defectReg = null;
        /// <summary>
        /// 瑕疵Region (For 影像轉正)
        /// </summary>
        public HObject DefectReg
        {
            get { return defectReg; }
            set { defectReg = value; }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            Release();
            HOperatorSet.GenEmptyObj(out defectReg);
        }

        /// <summary>
        /// 釋放記憶體
        /// </summary>
        public void Release()
        {
            Extension.HObjectMedthods.ReleaseHObject(ref defectReg);
        }

    }

}
