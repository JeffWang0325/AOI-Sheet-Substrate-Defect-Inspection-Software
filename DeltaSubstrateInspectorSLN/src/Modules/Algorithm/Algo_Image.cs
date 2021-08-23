using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HalconDotNet;
using System.Collections; // For ArrayList, Hashtable
using DeltaSubstrateInspector.src.Modules.NewInspectModule.InductanceInsp; // For enuBand
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

namespace DeltaSubstrateInspector.src.Modules.Algorithm // (20191226) Jeff Revised!
{
    #region 影像 & 區域 演算法列表

    /// <summary>
    /// 以影像為基礎之演算法
    /// </summary>
    public enum enu_Algo_Image
    {
        /// <summary>
        /// convert_image_type
        /// </summary>
        轉換影像型態,
        /// <summary>
        /// gen_image_const
        /// </summary>
        創建影像,
        /// <summary>
        /// reduce_domain
        /// </summary>
        影像ROI,
        /// <summary>
        /// gen_rectangle1 + reduce_domain + crop_domain
        /// </summary>
        影像分割, // (20200310) Jeff Revised!
        /// <summary>
        /// paint_gray
        /// </summary>
        Paint_Gray,
        /// <summary>
        /// paint_region
        /// </summary>
        Paint_Region,
        /// <summary>
        /// crop_domain
        /// </summary>
        Crop_Domain,
        /// <summary>
        /// zoom_image_factor
        /// </summary>
        Zoom_Factor,
        /// <summary>
        /// zoom_image_size
        /// </summary>
        Zoom_Size,
        /// <summary>
        /// scale_image
        /// </summary>
        Scale,
        /// <summary>
        /// scale_image_max
        /// </summary>
        Scale_Max,
        /// <summary>
        /// equ_histo_image
        /// </summary>
        直方圖等化,
        /// <summary>
        /// HistogramEqualization_Part()
        /// </summary>
        分區直方圖等化,
        /// <summary>
        /// compose3
        /// </summary>
        組合RGB影像,
        /// <summary>
        /// add_image
        /// </summary>
        影像相加,
        /// <summary>
        /// sub_image
        /// </summary>
        影像相減,
        /// <summary>
        /// mult_image
        /// </summary>
        影像相乘,
        /// <summary>
        /// div_image
        /// </summary>
        影像相除,
        /// <summary>
        /// bandpass_image
        /// </summary>
        帶通濾波,
        /// <summary>
        /// highpass_image
        /// </summary>
        高通濾波,
        /// <summary>
        /// invert_image
        /// </summary>
        反向,
        /// <summary>
        /// max_image
        /// </summary>
        Max,
        /// <summary>
        /// min_image
        /// </summary>
        Min,
        /// <summary>
        /// abs_image
        /// </summary>
        影像絕對值,
        /// <summary>
        /// abs_diff_image
        /// </summary>
        影像絕對誤差,
        /// <summary>
        /// sin_image
        /// </summary>
        sine,
        /// <summary>
        /// cos_image
        /// </summary>
        cosine,
        /// <summary>
        /// dots_image
        /// </summary>
        Dots,
        /// <summary>
        /// fft_image
        /// </summary>
        FFT,
        /// <summary>
        /// fft_image_inv
        /// </summary>
        IFFT,
    };

    /// <summary>
    /// 區域演算法類型列表
    /// </summary>
    public enum enuTools // (20200729) Jeff Revised!
    {
        Threshold,
        RegionSets,
        Morphology,
        Select
    }

    /// <summary>
    /// 區域演算法列表 (Threshold)
    /// </summary>
    public enum enuThreshold // (20200729) Jeff Revised!
    {
        一般二值化,
        Dual二值化,
        自動二值化,
        動態二值化
    }
    
    /// <summary>
    /// 區域演算法列表 (RegionSets)
    /// </summary>
    public enum enuRegionSets // (20200729) Jeff Revised!
    {
        CreateROI,
        Union1,
        Union2,
        Difference,
        Intersection,
        Complement,
        Connection,
        fillup,
        Copy,
        Shapetrans,
        FillupShape,
        Concat
    }

    /// <summary>
    /// 區域演算法列表 (Morphology)
    /// </summary>
    public enum enuMorphology // (20200729) Jeff Revised!
    {
        opening_rect1,
        opening_circle,
        closing_rect1,
        closing_circle,
        dilation_rect1,
        dilation_circle,
        erosion_rect1,
        erosion_circle
    }

    /// <summary>
    /// 區域演算法列表 (Select)
    /// </summary>
    public enum enuSelect // (20200729) Jeff Revised!
    {
        // select_shape
        area,
        row,
        column,
        width,
        height,
        row1,
        column1,
        row2,
        column2,
        circularity,
        compactness,
        contlength,
        convexity,
        rectangularity,
        outer_radius,
        inner_radius,
        inner_width,
        inner_height,
        dist_mean,
        dist_deviation,
        num_sides,
        connect_num,
        holes_num,
        area_holes,
        max_diameter,
        orientation,

        // select_gray
        min,
        max,
        mean,
        deviation,
        entropy,
    }

    #endregion

    #region 演算法之字串型別參數列表

    /// <summary>
    /// 影像型態
    /// Note: 要去掉最前面的"_"
    /// </summary>
    public enum enu_ImageType
    {
        _byte,
        _complex,
        _cyclic,
        _direction,
        _int1,
        _int2,
        _int4,
        _int8,
        _real,
        _uint2,
        _vector_field_absolute,
        _vector_field_relative
    };

    /// <summary>
    /// Paint regions filled or as boundaries.
    /// </summary>
    public enum enu_PaintType
    {
        fill,
        margin
    };

    /// <summary>
    /// 內插法
    /// </summary>
    public enum enu_interpolation
    {
        bicubic,
        bilinear,
        constant,
        nearest_neighbor,
        weighted
    };

    public enum enu_BandpassImage_filterType
    {
        lines
    };

    public enum enu_DotsImage_filterType
    {
        light,
        all,
        dark
    };

    public enum enu_LightDark // (20200729) Jeff Revised!
    {
        light,
        dark,
        equal,
        not_equal
    };

    public enum enu_ShapeTrans_Type // (20200729) Jeff Revised!
    {
        convex,
        ellipse,
        outer_circle,
        inner_circle,
        rectangle1,
        rectangle2,
        inner_rectangle1,
        inner_center,
    };

    public enum enu_FillUp_Feature // (20200729) Jeff Revised!
    {
        anisometry,
        area,
        compactness,
        convexity,
        inner_circle,
        outer_circle,
        phi,
        ra,
        rb,
    };

    #endregion

    [Serializable]
    public class Algo_Image
    {
        #region 參數

        /// <summary>
        /// 原始影像集合
        /// </summary>
        public List<HObject> ImageSource { get; set; } = new List<HObject>();

        /// <summary>
        /// 演算法集合
        /// </summary>
        public List<Single_AlgoImage> ListAlgoImage { get; set; } = new List<Single_AlgoImage>();

        /// <summary>
        /// 是否使用編輯範圍 (只對有ROI之影像演算法 or 區域演算法B)
        /// </summary>
        public bool B_UsedRegion { get; set; } = false; // (20200119) Jeff Revised!

        /// <summary>
        /// 使用範圍列表
        /// </summary>
        [XmlIgnore] // 帶有XmlIgnore，表示序列化時不序列化此屬性
        public List<HObject> UsedRegionList { get; set; } = new List<HObject>(); // (20200119) Jeff Revised!

        /// <summary>
        /// 各使用範圍之名稱
        /// </summary>
        public List<string> Name_UsedRegionList { get; set; } = new List<string>(); // (20200119) Jeff Revised!

        #endregion

        #region 方法

        public Algo_Image() { }
        
        /// <summary>
        /// 代替 建構子，初始化 之功能
        /// </summary>
        /// <param name="b_UsedRegion"></param>
        /// <param name="usedRegionList"></param>
        /// <param name="name_UsedRegionList"></param>
        public void Algo_Image_Constructor(bool b_UsedRegion, List<HObject> usedRegionList, List<string> name_UsedRegionList)
        {
            this.B_UsedRegion = b_UsedRegion;
            clsStaticTool.DisposeAll(this.UsedRegionList);
            this.UsedRegionList = usedRegionList;
            this.Name_UsedRegionList = name_UsedRegionList;
        }

        /// <summary>
        /// 執行所有已新增之 影像/區域演算法
        /// </summary>
        /// <param name="ResultImages">各演算法之結果影像</param>
        /// <param name="ResultRegions">各演算法之結果區域</param>
        /// <returns></returns>
        public bool Execute(out List<HObject> ResultImages, out List<HObject> ResultRegions) // (20200729) Jeff Revised!
        {
            bool b_status_ = false;
            ResultImages = new List<HObject>();
            ResultRegions = new List<HObject>();

            try
            {
                for (int id_Algo = 0; id_Algo < this.ListAlgoImage.Count; id_Algo++)
                {
                    #region 計算各演算法

                    Single_AlgoImage AlgoImage = this.ListAlgoImage[id_Algo];

                    /* 演算法各影像 */
                    List<HObject> InputImages = new List<HObject>(); // 輸入到演算法之影像
                    for (int i = 0; i < AlgoImage.CountImg; i++)
                    {
                        if (AlgoImage.B_ImageSource[i]) // 原始影像
                            InputImages.Add(this.ImageSource[AlgoImage.ID_InputImage[i]]);
                        else // 其他演算法計算結果影像
                            InputImages.Add(ResultImages[AlgoImage.ID_InputImage[i]]);
                    }

                    /* 演算法各區域 */
                    List<HObject> InputRegions = new List<HObject>(); // 輸入到演算法之區域
                    for (int i = 0; i < AlgoImage.CountReg; i++)
                    {
                        if (AlgoImage.B_RegionSource[i]) // 其他演算法計算結果區域
                            InputRegions.Add(ResultRegions[AlgoImage.ID_InputRegion[i]]);
                        else // 使用範圍
                            InputRegions.Add(this.UsedRegionList[AlgoImage.ID_InputRegion[i]]);
                    }

                    // 計算單一影像區域演算法
                    HObject imageResult, regionResult;
                    if (!(Compute_1_AlgoImage(InputImages, InputRegions, AlgoImage, out imageResult, out regionResult, this.UsedRegionList)))
                        return false;
                    ResultImages.Add(imageResult);
                    ResultRegions.Add(regionResult);

                    #endregion
                }

                b_status_ = true;
            }
            catch (Exception ex)
            {
                string message = "Error: " + ex.Message;
                MessageBox.Show(message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return b_status_;
        }

        /// <summary>
        /// 計算單一影像區域演算法
        /// </summary>
        /// <param name="images">輸入影像</param>
        /// <param name="regions">輸入區域</param>
        /// <param name="AlgoImage">影像區域演算法</param>
        /// <param name="imageResult">結果影像</param>
        /// <param name="regionResult">結果區域</param>
        /// <param name="usedRegionList">使用範圍列表</param>
        /// <returns></returns>
        public static bool Compute_1_AlgoImage(List<HObject> images, List<HObject> regions, Single_AlgoImage AlgoImage, out HObject imageResult, out HObject regionResult, List<HObject> usedRegionList = null) // (20200729) Jeff Revised!
        {
            bool b_status_ = false;
            //HOperatorSet.GenEmptyObj(out imageResult);
            //imageResult = null;
            //regionResult = null;
            HOperatorSet.GenEmptyObj(out imageResult);
            HOperatorSet.GenEmptyObj(out regionResult);

            List<enuBand> bands = AlgoImage.Bands;
            Hashtable parameters = AlgoImage.Parameters;
            
            // images數量與bands數量是否匹配
            //if ((images.Count != bands.Count) || (images.Count == 0))
            if (images.Count != bands.Count)
                return false;

            int CountImg = images.Count;
            List<HObject> Images = new List<HObject>();
            try
            {
                // 抽取各影像Band
                for (int i = 0; i < CountImg; i++)
                    Images.Add(clsStaticTool.GetChannelImage(images[i], bands[i]));

                #region 各類型 影像/區域演算法 計算

                if (AlgoImage.B_ImageAlgo) // 影像演算法
                {
                    #region 各類型影像演算法
                    
                    switch (AlgoImage.Algo)
                    {
                        case enu_Algo_Image.轉換影像型態:
                            {
                                // 影像數量檢查
                                if (CountImg != 1)
                                    return false;
                                HOperatorSet.ConvertImageType(Images[0], out imageResult, parameters["newType"].ToString());
                            }
                            break;
                        case enu_Algo_Image.創建影像:
                            {
                                // 影像數量檢查
                                if (CountImg != 0)
                                    return false;
                                HOperatorSet.GenImageConst(out imageResult, parameters["newType"].ToString(), int.Parse(parameters["width"].ToString()), int.Parse(parameters["height"].ToString()));
                            }
                            break;
                        case enu_Algo_Image.影像ROI:
                            {
                                // 影像數量檢查
                                if (CountImg != 1)
                                    return false;

                                // ROI
                                HObject ROI = null;
                                if (parameters.ContainsKey("region"))
                                    ROI = ((HObject)(parameters["region"])).Clone();
                                else if (parameters.ContainsKey("SelectRegionIndex"))
                                {
                                    HObject SelectRegion = usedRegionList[int.Parse(parameters["SelectRegionIndex"].ToString())];
                                    if (bool.Parse(parameters["b_ROI_Invert"].ToString()))
                                        Region_Invert(Images[0], SelectRegion, out ROI);
                                    else
                                        ROI = SelectRegion.Clone();
                                }

                                HOperatorSet.ReduceDomain(Images[0], ROI, out imageResult);
                                ROI.Dispose();
                            }
                            break;
                        case enu_Algo_Image.影像分割: // (20200310) Jeff Revised!
                            {
                                // 影像數量檢查
                                if (CountImg != 1)
                                    return false;

                                // ROI
                                HObject ROI;
                                HOperatorSet.GenRectangle1(out ROI, int.Parse(parameters["y1"].ToString()), int.Parse(parameters["x1"].ToString()), int.Parse(parameters["y2"].ToString()), int.Parse(parameters["x2"].ToString()));

                                HObject imageTemp;
                                HOperatorSet.ReduceDomain(Images[0], ROI, out imageTemp);
                                ROI.Dispose();

                                HOperatorSet.CropDomain(imageTemp, out imageResult);
                                imageTemp.Dispose();
                            }
                            break;
                        case enu_Algo_Image.Paint_Gray:
                            {
                                // 影像數量檢查
                                if (CountImg != 2)
                                    return false;
                                HOperatorSet.PaintGray(Images[0], Images[1], out imageResult);
                            }
                            break;
                        case enu_Algo_Image.Paint_Region:
                            {
                                // 影像數量檢查
                                if (CountImg != 1)
                                    return false;

                                HTuple width, height;
                                HOperatorSet.GetImageSize(Images[0], out width, out height);

                                // ROI
                                HObject ROI = null;
                                if (!(bool.Parse(parameters["b_ROI"].ToString())))
                                    HOperatorSet.GenRectangle1(out ROI, 0, 0, height - 1, width - 1);
                                else if (parameters.ContainsKey("region"))
                                    ROI = ((HObject)(parameters["region"])).Clone();
                                else if (parameters.ContainsKey("SelectRegionIndex"))
                                {
                                    HObject SelectRegion = usedRegionList[int.Parse(parameters["SelectRegionIndex"].ToString())];
                                    if (bool.Parse(parameters["b_ROI_Invert"].ToString()))
                                        Region_Invert(Images[0], SelectRegion, out ROI);
                                    else
                                        ROI = SelectRegion.Clone();
                                }

                                HOperatorSet.PaintRegion(ROI, Images[0], out imageResult, double.Parse(parameters["grayval"].ToString()), parameters["PaintType"].ToString());
                                ROI.Dispose();
                            }
                            break;
                        case enu_Algo_Image.Crop_Domain:
                            {
                                // 影像數量檢查
                                if (CountImg != 1)
                                    return false;
                                HOperatorSet.CropDomain(Images[0], out imageResult);
                            }
                            break;
                        case enu_Algo_Image.Zoom_Factor:
                            {
                                // 影像數量檢查
                                if (CountImg != 1)
                                    return false;
                                HOperatorSet.ZoomImageFactor(Images[0], out imageResult, double.Parse(parameters["scaleWidth"].ToString()), double.Parse(parameters["scaleHeight"].ToString()), parameters["interpolation"].ToString());
                            }
                            break;
                        case enu_Algo_Image.Zoom_Size:
                            {
                                // 影像數量檢查
                                if (CountImg != 1)
                                    return false;
                                HOperatorSet.ZoomImageSize(Images[0], out imageResult, int.Parse(parameters["width"].ToString()), int.Parse(parameters["height"].ToString()), parameters["interpolation"].ToString());
                            }
                            break;
                        case enu_Algo_Image.Scale:
                            {
                                // 影像數量檢查
                                if (CountImg != 1)
                                    return false;
                                HOperatorSet.ScaleImage(Images[0], out imageResult, double.Parse(parameters["mult"].ToString()), double.Parse(parameters["add"].ToString()));
                            }
                            break;
                        case enu_Algo_Image.Scale_Max:
                            {
                                // 影像數量檢查
                                if (CountImg != 1)
                                    return false;
                                HOperatorSet.ScaleImageMax(Images[0], out imageResult);
                            }
                            break;
                        case enu_Algo_Image.直方圖等化:
                            {
                                // 影像數量檢查
                                if (CountImg != 1)
                                    return false;
                                HOperatorSet.EquHistoImage(Images[0], out imageResult);
                            }
                            break;
                        case enu_Algo_Image.分區直方圖等化:
                            {
                                // 影像數量檢查
                                if (CountImg != 1)
                                    return false;

                                // ROI
                                HObject ROI = null;
                                if (parameters.ContainsKey("region"))
                                    ROI = ((HObject)(parameters["region"])).Clone();
                                else if (parameters.ContainsKey("SelectRegionIndex"))
                                {
                                    HObject SelectRegion = usedRegionList[int.Parse(parameters["SelectRegionIndex"].ToString())];
                                    if (bool.Parse(parameters["b_ROI_Invert"].ToString()))
                                        Region_Invert(Images[0], SelectRegion, out ROI);
                                    else
                                        ROI = SelectRegion.Clone();
                                }

                                HistogramEqualization_Part(Images[0], out imageResult, int.Parse(parameters["count_row"].ToString()), int.Parse(parameters["count_col"].ToString()), bool.Parse(parameters["b_ROI"].ToString()), ROI);
                                ROI.Dispose();
                            }
                            break;
                        case enu_Algo_Image.組合RGB影像:
                            {
                                // 影像數量檢查
                                if (CountImg != 3)
                                    return false;
                                HOperatorSet.Compose3(Images[0], Images[1], Images[2], out imageResult);
                            }
                            break;
                        case enu_Algo_Image.影像相加:
                            {
                                // 影像數量檢查
                                if (CountImg != 2)
                                    return false;
                                HOperatorSet.AddImage(Images[0], Images[1], out imageResult, double.Parse(parameters["mult"].ToString()), double.Parse(parameters["add"].ToString()));
                            }
                            break;
                        case enu_Algo_Image.影像相減:
                            {
                                // 影像數量檢查
                                if (CountImg != 2)
                                    return false;
                                HOperatorSet.SubImage(Images[0], Images[1], out imageResult, double.Parse(parameters["mult"].ToString()), double.Parse(parameters["add"].ToString()));
                            }
                            break;
                        case enu_Algo_Image.影像相乘:
                            {
                                // 影像數量檢查
                                if (CountImg != 2)
                                    return false;
                                HOperatorSet.MultImage(Images[0], Images[1], out imageResult, double.Parse(parameters["mult"].ToString()), double.Parse(parameters["add"].ToString()));
                            }
                            break;
                        case enu_Algo_Image.影像相除:
                            {
                                // 影像數量檢查
                                if (CountImg != 2)
                                    return false;
                                HOperatorSet.DivImage(Images[0], Images[1], out imageResult, double.Parse(parameters["mult"].ToString()), double.Parse(parameters["add"].ToString()));
                            }
                            break;
                        case enu_Algo_Image.帶通濾波:
                            {
                                // 影像數量檢查
                                if (CountImg != 1)
                                    return false;
                                HOperatorSet.BandpassImage(Images[0], out imageResult, parameters["filterType"].ToString());
                            }
                            break;
                        case enu_Algo_Image.高通濾波:
                            {
                                // 影像數量檢查
                                if (CountImg != 1)
                                    return false;
                                HOperatorSet.HighpassImage(Images[0], out imageResult, int.Parse(parameters["width"].ToString()), int.Parse(parameters["height"].ToString()));
                            }
                            break;
                        case enu_Algo_Image.反向:
                            {
                                // 影像數量檢查
                                if (CountImg != 1)
                                    return false;
                                HOperatorSet.InvertImage(Images[0], out imageResult);
                            }
                            break;
                        case enu_Algo_Image.Max:
                            {
                                // 影像數量檢查
                                if (CountImg != 2)
                                    return false;
                                HOperatorSet.MaxImage(Images[0], Images[1], out imageResult);
                            }
                            break;
                        case enu_Algo_Image.Min:
                            {
                                // 影像數量檢查
                                if (CountImg != 2)
                                    return false;
                                HOperatorSet.MinImage(Images[0], Images[1], out imageResult);
                            }
                            break;
                        case enu_Algo_Image.影像絕對值:
                            {
                                // 影像數量檢查
                                if (CountImg != 1)
                                    return false;
                                HOperatorSet.AbsImage(Images[0], out imageResult);
                            }
                            break;
                        case enu_Algo_Image.影像絕對誤差:
                            {
                                // 影像數量檢查
                                if (CountImg != 2)
                                    return false;
                                HOperatorSet.AbsDiffImage(Images[0], Images[1], out imageResult, double.Parse(parameters["mult"].ToString()));
                            }
                            break;
                        case enu_Algo_Image.sine:
                            {
                                // 影像數量檢查
                                if (CountImg != 1)
                                    return false;
                                HOperatorSet.SinImage(Images[0], out imageResult);
                            }
                            break;
                        case enu_Algo_Image.cosine:
                            {
                                // 影像數量檢查
                                if (CountImg != 1)
                                    return false;
                                HOperatorSet.CosImage(Images[0], out imageResult);
                            }
                            break;
                        case enu_Algo_Image.Dots:
                            {
                                // 影像數量檢查
                                if (CountImg != 1)
                                    return false;
                                HOperatorSet.DotsImage(Images[0], out imageResult, int.Parse(parameters["diameter"].ToString()), parameters["filterType"].ToString(), int.Parse(parameters["pixelShift"].ToString()));
                            }
                            break;
                        case enu_Algo_Image.FFT:
                            {
                                // 影像數量檢查
                                if (CountImg != 1)
                                    return false;
                                HOperatorSet.FftImage(Images[0], out imageResult);
                            }
                            break;
                        case enu_Algo_Image.IFFT:
                            {
                                // 影像數量檢查
                                if (CountImg != 1)
                                    return false;
                                HOperatorSet.FftImageInv(Images[0], out imageResult);
                            }
                            break;
                        default:
                            return false;
                    }

                    #endregion
                }
                else // 區域演算法
                {
                    #region 各類型區域演算法

                    // 影像數量檢查
                    if (CountImg != 1)
                        return false;

                    // 僅供顯示用途
                    //imageResult = images[0].Clone(); // Note: 會耗用記憶體及計算時間!

                    int CountReg = regions.Count;
                    enuTools type = AlgoImage.Type_Algo;
                    if (type == enuTools.Threshold)
                    {
                        #region Threshold

                        // 區域數量檢查
                        if (CountReg != 0)
                            return false;

                        switch ((enuThreshold)AlgoImage.Index_Algo_Region)
                        {
                            case enuThreshold.一般二值化:
                                {
                                    HOperatorSet.Threshold(Images[0], out regionResult, int.Parse(parameters["ThMin"].ToString()), int.Parse(parameters["ThMax"].ToString()));
                                }
                                break;
                            case enuThreshold.Dual二值化:
                                {
                                    HOperatorSet.DualThreshold(Images[0], out regionResult, int.Parse(parameters["MinSize"].ToString()), double.Parse(parameters["MinGray"].ToString()), double.Parse(parameters["Threshold"].ToString()));
                                }
                                break;
                            case enuThreshold.自動二值化:
                                {
                                    HOperatorSet.AutoThreshold(Images[0], out regionResult, double.Parse(parameters["Sigma"].ToString()));
                                }
                                break;
                            case enuThreshold.動態二值化:
                                {
                                    HObject MeanImage;
                                    HOperatorSet.MeanImage(Images[0], out MeanImage, int.Parse(parameters["MaskWidth"].ToString()), int.Parse(parameters["MaskHeight"].ToString()));
                                    HOperatorSet.DynThreshold(Images[0], MeanImage, out regionResult, int.Parse(parameters["Offset"].ToString()), parameters["LightDark"].ToString());
                                    MeanImage.Dispose();
                                }
                                break;
                            default:
                                break;
                        }

                        #endregion
                    }
                    else if (type == enuTools.RegionSets)
                    {
                        #region RegionSets
                        
                        switch ((enuRegionSets)AlgoImage.Index_Algo_Region)
                        {
                            case enuRegionSets.CreateROI:
                                {
                                    // 區域數量檢查
                                    if (CountReg != 0)
                                        return false;

                                    if (AlgoImage.B_UsedRegion && bool.Parse(parameters["b_ROI_UsedRegion"].ToString()))
                                    {
                                        HObject SelectRegion = usedRegionList[int.Parse(parameters["SelectRegionIndex"].ToString())];
                                        if (bool.Parse(parameters["b_ROI_Invert"].ToString()))
                                            Region_Invert(Images[0], SelectRegion, out regionResult);
                                        else
                                            regionResult = SelectRegion.Clone();
                                    }
                                    else
                                        regionResult = ((HObject)(parameters["region"])).Clone();
                                }
                                break;
                            case enuRegionSets.Union1:
                                {
                                    // 區域數量檢查
                                    if (CountReg != 1)
                                        return false;

                                    HOperatorSet.Union1(regions[0], out regionResult);
                                }
                                break;
                            case enuRegionSets.Union2:
                                {
                                    // 區域數量檢查
                                    if (CountReg != 2)
                                        return false;

                                    HOperatorSet.Union2(regions[0], regions[1], out regionResult);
                                }
                                break;
                            case enuRegionSets.Difference:
                                {
                                    // 區域數量檢查
                                    if (CountReg != 2)
                                        return false;

                                    HOperatorSet.Difference(regions[0], regions[1], out regionResult);
                                }
                                break;
                            case enuRegionSets.Intersection:
                                {
                                    // 區域數量檢查
                                    if (CountReg != 2)
                                        return false;

                                    HOperatorSet.Intersection(regions[0], regions[1], out regionResult);
                                }
                                break;
                            case enuRegionSets.Complement:
                                {
                                    // 區域數量檢查
                                    if (CountReg != 1)
                                        return false;

                                    HOperatorSet.SetSystem("clip_region", "true");
                                    HOperatorSet.Complement(regions[0], out regionResult);
                                    HOperatorSet.SetSystem("clip_region", "false");
                                }
                                break;
                            case enuRegionSets.Connection:
                                {
                                    // 區域數量檢查
                                    if (CountReg != 1)
                                        return false;
                                    
                                    HOperatorSet.Connection(regions[0], out regionResult);
                                }
                                break;
                            case enuRegionSets.fillup:
                                {
                                    // 區域數量檢查
                                    if (CountReg != 1)
                                        return false;

                                    HOperatorSet.FillUp(regions[0], out regionResult);
                                }
                                break;
                            case enuRegionSets.Copy:
                                {
                                    // 區域數量檢查
                                    if (CountReg != 1)
                                        return false;

                                    regionResult = regions[0].Clone();
                                }
                                break;
                            case enuRegionSets.Shapetrans:
                                {
                                    // 區域數量檢查
                                    if (CountReg != 1)
                                        return false;

                                    HOperatorSet.ShapeTrans(regions[0], out regionResult, parameters["ShapeTrans_Type"].ToString());
                                }
                                break;
                            case enuRegionSets.FillupShape:
                                {
                                    // 區域數量檢查
                                    if (CountReg != 1)
                                        return false;

                                    HOperatorSet.FillUpShape(regions[0], out regionResult, parameters["FillUp_Feature"].ToString(), double.Parse(parameters["Min"].ToString()), double.Parse(parameters["Max"].ToString()));
                                }
                                break;
                            case enuRegionSets.Concat:
                                {
                                    // 區域數量檢查
                                    if (CountReg != 2)
                                        return false;

                                    HOperatorSet.ConcatObj(regions[0], regions[1], out regionResult);
                                }
                                break;
                            default:
                                break;
                        }

                        #endregion
                    }
                    else if (type == enuTools.Morphology)
                    {
                        #region Morphology

                        // 區域數量檢查
                        if (CountReg != 1)
                            return false;
                        
                        switch ((enuMorphology)AlgoImage.Index_Algo_Region)
                        {
                            case enuMorphology.opening_rect1:
                                {
                                    HOperatorSet.OpeningRectangle1(regions[0], out regionResult, int.Parse(parameters["Width"].ToString()), int.Parse(parameters["Height"].ToString()));
                                }
                                break;
                            case enuMorphology.opening_circle:
                                {
                                    HOperatorSet.OpeningCircle(regions[0], out regionResult, double.Parse(parameters["Radius"].ToString()));
                                }
                                break;
                            case enuMorphology.closing_rect1:
                                {
                                    HOperatorSet.ClosingRectangle1(regions[0], out regionResult, int.Parse(parameters["Width"].ToString()), int.Parse(parameters["Height"].ToString()));
                                }
                                break;
                            case enuMorphology.closing_circle:
                                {
                                    HOperatorSet.ClosingCircle(regions[0], out regionResult, double.Parse(parameters["Radius"].ToString()));
                                }
                                break;
                            case enuMorphology.dilation_rect1:
                                {
                                    HOperatorSet.DilationRectangle1(regions[0], out regionResult, int.Parse(parameters["Width"].ToString()), int.Parse(parameters["Height"].ToString()));
                                }
                                break;
                            case enuMorphology.dilation_circle:
                                {
                                    HOperatorSet.DilationCircle(regions[0], out regionResult, double.Parse(parameters["Radius"].ToString()));
                                }
                                break;
                            case enuMorphology.erosion_rect1:
                                {
                                    HOperatorSet.ErosionRectangle1(regions[0], out regionResult, int.Parse(parameters["Width"].ToString()), int.Parse(parameters["Height"].ToString()));
                                }
                                break;
                            case enuMorphology.erosion_circle:
                                {
                                    HOperatorSet.ErosionCircle(regions[0], out regionResult, double.Parse(parameters["Radius"].ToString()));
                                }
                                break;

                            default:
                                break;
                        }
                        
                        #endregion
                    }
                    else
                    {
                        #region Select

                        // 區域數量檢查
                        if (CountReg != 1)
                            return false;

                        enuSelect algo = (enuSelect)AlgoImage.Index_Algo_Region;
                        if ((algo == enuSelect.min) || (algo == enuSelect.max) || (algo == enuSelect.mean) || (algo == enuSelect.deviation) || (algo == enuSelect.entropy))
                        {
                            HOperatorSet.SelectGray(regions[0], Images[0], out regionResult, (new HTuple(algo.ToString())), "and", double.Parse(parameters["Min"].ToString()), double.Parse(parameters["Max"].ToString()));
                        }
                        else
                        {
                            HOperatorSet.SelectShape(regions[0], out regionResult, (new HTuple(algo.ToString())), "and", double.Parse(parameters["Min"].ToString()), double.Parse(parameters["Max"].ToString()));
                        }
                        
                        #endregion
                    }

                    #endregion
                }

                #endregion

                b_status_ = true;
            }
            catch (Exception ex)
            {
                string message = "Error: " + ex.Message;
                MessageBox.Show(message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                // Release memories
                clsStaticTool.DisposeAll(Images);
            }

            return b_status_;
        }
        
        /// <summary>
        /// 計算區域反向
        /// </summary>
        /// <param name="Image"></param>
        /// <param name="Region"></param>
        /// <param name="ResultRegion"></param>
        /// <returns></returns>
        public static bool Region_Invert(HObject Image, HObject Region, out HObject ResultRegion)
        {
            bool b_status_ = false;
            HOperatorSet.GenEmptyObj(out ResultRegion);

            try
            {
                HTuple width, height;
                HOperatorSet.GetImageSize(Image, out width, out height);
                HObject reg_WholeImg;
                HOperatorSet.GenRectangle1(out reg_WholeImg, 0, 0, height - 1, width - 1);
                ResultRegion.Dispose();
                HOperatorSet.Difference(reg_WholeImg, Region, out ResultRegion);
                reg_WholeImg.Dispose();

                b_status_ = true;
            }
            catch (Exception ex)
            { }

            return b_status_;
        }

        /// <summary>
        /// 分區做直方圖等化
        /// </summary>
        /// <param name="Image"></param>
        /// <param name="ResultImage"></param>
        /// <param name="count_row"></param>
        /// <param name="count_col"></param>
        /// <param name="b_ROI"></param>
        /// <param name="Region_ROI"></param>
        /// <returns></returns>
        public static bool HistogramEqualization_Part(HObject Image, out HObject ResultImage, int count_row = 1, int count_col = 1, bool b_ROI = false, HObject Region_ROI = null) // (20200729) Jeff Revised!
        {
            bool b_status_ = false;
            HOperatorSet.GenEmptyObj(out ResultImage);

            // 檢查輸入參數
            if (count_row < 1 || count_col < 1)
                return false;

            try
            {
                HTuple width, height;
                HOperatorSet.GetImageSize(Image, out width, out height);

                // ROI
                HObject ROI, Image_ROI;
                if (!b_ROI || Region_ROI == null)
                    HOperatorSet.GenRectangle1(out ROI, 0, 0, height - 1, width - 1);
                else
                {
                    //ROI = Region_ROI.Clone();
                    HOperatorSet.Union1(Region_ROI, out ROI); // (20200729) Jeff Revised!
                }
                HOperatorSet.ReduceDomain(Image, ROI, out Image_ROI);
                HTuple hv_Value_WH = new HTuple();
                HOperatorSet.RegionFeatures(ROI, (new HTuple("width")).TupleConcat("height"), out hv_Value_WH);
                HTuple hv_W = new HTuple(), hv_H = new HTuple();
                hv_W = hv_Value_WH.TupleSelect(0);
                hv_H = hv_Value_WH.TupleSelect(1);

                // 分區做直方圖等化
                HTuple hv_w = new HTuple(), hv_h = new HTuple();
                hv_w = hv_W / count_col;
                hv_h = hv_H / count_row;

                HObject ho_Partitioned, ho_ImagePart, ho_ImageEquHisto, ho_TiledImage;
                HTuple hv_Value_R1 = new HTuple(), hv_Value_C1 = new HTuple(), hv_Value_R2 = new HTuple(), hv_Value_C2 = new HTuple(), hv_tile_rect = new HTuple();
                HOperatorSet.PartitionRectangle(ROI, out ho_Partitioned, hv_w, hv_h);
                HOperatorSet.RegionFeatures(ho_Partitioned, "row1", out hv_Value_R1);
                HOperatorSet.RegionFeatures(ho_Partitioned, "column1", out hv_Value_C1);
                HOperatorSet.RegionFeatures(ho_Partitioned, "row2", out hv_Value_R2);
                HOperatorSet.RegionFeatures(ho_Partitioned, "column2", out hv_Value_C2);
                HOperatorSet.CropRectangle1(Image_ROI, out ho_ImagePart, hv_Value_R1, hv_Value_C1, hv_Value_R2, hv_Value_C2);
                HOperatorSet.EquHistoImage(ho_ImagePart, out ho_ImageEquHisto);
                HOperatorSet.TupleGenConst(count_row * count_col, -1, out hv_tile_rect);
                HOperatorSet.TileImagesOffset(ho_ImageEquHisto, out ho_TiledImage, hv_Value_R1, hv_Value_C1,
                                              hv_tile_rect, hv_tile_rect, hv_tile_rect, hv_tile_rect, width, height);

                // 將結果影像貼到原始影像上
                HOperatorSet.PaintGray(ho_TiledImage, Image, out ResultImage);

                // Release Memories
                ROI.Dispose();
                Image_ROI.Dispose();
                ho_Partitioned.Dispose();
                ho_ImagePart.Dispose();
                ho_ImageEquHisto.Dispose();
                ho_TiledImage.Dispose();

                b_status_ = true;
            }
            catch (Exception ex)
            { }

            return b_status_;
        }

        /// <summary>
        /// 更新 listView_Edit
        /// </summary>
        /// <param name="ListView_Edit"></param>
        public void Update_listView_Edit(ListView ListView_Edit) // (20200729) Jeff Revised!
        {
            ListView_Edit.ForeColor = Color.DarkViolet;

            ListView_Edit.BeginUpdate();
            ListView_Edit.Items.Clear();

            for (int i = 0; i < this.ListAlgoImage.Count; i++)
            {
                ListViewItem lvi = new ListViewItem(i.ToString()); // 編號
                lvi.SubItems.Add(this.ListAlgoImage[i].Name); // 名稱

                // 演算法
                if (this.ListAlgoImage[i].B_ImageAlgo) // 影像演算法
                    lvi.SubItems.Add(this.ListAlgoImage[i].Algo.ToString());
                else // 區域演算法
                {
                    string name_Algo;
                    int index = this.ListAlgoImage[i].Index_Algo_Region;
                    enuTools type = this.ListAlgoImage[i].Type_Algo;
                    if (type == enuTools.Threshold)
                        name_Algo = ((enuThreshold)index).ToString();
                    else if (type == enuTools.RegionSets)
                        name_Algo = ((enuRegionSets)index).ToString();
                    else if (type == enuTools.Morphology)
                        name_Algo = ((enuMorphology)index).ToString();
                    else
                        name_Algo = ((enuSelect)index).ToString();
                    lvi.SubItems.Add(name_Algo);
                }

                ListView_Edit.Items.Add(lvi);
            }

            ListView_Edit.EndUpdate();
        }

        /// <summary>
        /// 【刪除】其中一個演算法
        /// </summary>
        /// <param name="id">欲刪除演算法之ID</param>
        /// <returns></returns>
        public bool Remove_1_Algo(int id) // (20200729) Jeff Revised!
        {
            bool b_status_ = false;
            
            #region 判斷其他演算法是否有參考欲刪除演算法之結果

            foreach (Single_AlgoImage single_algoImg in ListAlgoImage)
            {
                for (int i = 0; i < single_algoImg.CountImg; i++) // 影像
                {
                    if (single_algoImg.B_ImageSource[i] == false) // 參考其他演算法計算結果影像
                    {
                        if (single_algoImg.ID_InputImage[i] == id)
                        {
                            MessageBox.Show("其他演算法有參考到此演算法之結果影像，請先更改或刪除該演算法", "刪除失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                    }
                }

                for (int i = 0; i < single_algoImg.CountReg; i++) // 區域
                {
                    if (single_algoImg.B_RegionSource[i] == true) // 參考其他演算法計算結果區域
                    {
                        if (single_algoImg.ID_InputRegion[i] == id)
                        {
                            MessageBox.Show("其他演算法有參考到此演算法之結果區域，請先更改或刪除該演算法", "刪除失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                    }
                }
            }

            #endregion
            
            #region 更新【刪除】後之 ListAlgoImage 物件

            ListAlgoImage[id].Dispose(); // 釋放記憶體
            ListAlgoImage.RemoveAt(id);
            // 更新已刪除演算法及其後之ID參數
            for (int ii = id; ii < ListAlgoImage.Count; ii++)
            {
                Single_AlgoImage single_algoImg = ListAlgoImage[ii];
                single_algoImg.ID = ii;

                for (int i = 0; i < single_algoImg.CountImg; i++) // 影像
                {
                    if (single_algoImg.B_ImageSource[i] == false) // 參考其他演算法計算結果影像
                    {
                        if (single_algoImg.ID_InputImage[i] > id)
                            single_algoImg.ID_InputImage[i]--;
                    }
                }

                for (int i = 0; i < single_algoImg.CountReg; i++) // 區域
                {
                    if (single_algoImg.B_RegionSource[i] == true) // 參考其他演算法計算結果區域
                    {
                        if (single_algoImg.ID_InputRegion[i] > id)
                            single_algoImg.ID_InputImage[i]--;
                    }
                }
            }

            #endregion

            b_status_ = true;
            return b_status_;
        }

        /// <summary>
        /// 儲存 Algo_Image 物件
        /// </summary>
        /// <param name="directory_"></param>
        /// <param name="Recipe"></param>
        /// <returns></returns>
        public static bool Save_ImageAlgo_Recipe(string directory_, Algo_Image Recipe)
        {
            bool b_status_ = false;
            try
            {
                if (clsStaticTool.SaveXML(Recipe, directory_ + "ImageAlgo_Recipe.xml"))
                    b_status_ = Recipe.SaveHalconFile(directory_);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }

            return b_status_;
        }

        /// <summary>
        /// 載入 Algo_Image 物件
        /// </summary>
        /// <param name="directory_"></param>
        /// <param name="Recipe"></param>
        /// <param name="b_Load_ImageSource"></param>
        /// <returns></returns>
        public static bool Load_ImageAlgo_Recipe(string directory_, ref Algo_Image Recipe, bool b_Load_ImageSource = true) // (20200729) Jeff Revised!
        {
            bool b_status_ = false;
            Recipe.Dispose(b_Load_ImageSource);
            Recipe = new Algo_Image();

            try
            {
                if (File.Exists(directory_ + "ImageAlgo_Recipe.xml"))
                {
                    if (clsStaticTool.LoadXML(directory_ + "ImageAlgo_Recipe.xml", out Recipe)) // (20200729) Jeff Revised!
                        b_status_ = Recipe.ReadHalconFile(directory_, b_Load_ImageSource);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }

            return b_status_;
        }
        
        public bool SaveHalconFile(string directory_, bool b_Save_ImageSource = true)
        {
            bool b_status_ = false;
            try
            {
                #region 儲存原始影像集合 (ImageSource)

                if (b_Save_ImageSource)
                {
                    try
                    {
                        for (int i = 0; i < this.ImageSource.Count; i++)
                            HOperatorSet.WriteImage(this.ImageSource[i], "tiff", 0, directory_ + "ImageSource_ID" + i.ToString());
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex.ToString());
                    }
                }

                #endregion

                #region 儲存各演算法內 Halcon Region

                foreach (Single_AlgoImage AlgoImage in this.ListAlgoImage)
                {
                    if (!(AlgoImage.SaveHalconFile(directory_)))
                        return false;
                }

                #endregion

                b_status_ = true;
            }
            catch (Exception ex)
            { }

            return b_status_;
        }

        public bool ReadHalconFile(string directory_, bool b_Load_ImageSource = true)
        {
            bool b_status_ = false;
            try
            {
                #region 讀取原始影像集合 (ImageSource)

                if (b_Load_ImageSource)
                {
                    for (int i = 0; i < this.ImageSource.Count; i++)
                    {
                        HObject img;
                        if (File.Exists(directory_ + "ImageSource_ID" + i.ToString() + ".tif"))
                            HOperatorSet.ReadImage(out img, directory_ + "ImageSource_ID" + i.ToString());
                        else
                            HOperatorSet.GenEmptyObj(out img);
                        this.ImageSource[i] = img;
                    }
                }

                #endregion

                #region 讀取各演算法內 Halcon Region

                foreach (Single_AlgoImage AlgoImage in this.ListAlgoImage)
                {
                    if (!(AlgoImage.ReadHalconFile(directory_)))
                        return false;
                }

                #endregion

                b_status_ = true;
            }
            catch (Exception ex)
            { }

            return b_status_;
        }

        /// <summary>
        /// 物件複製 (淺層)
        /// Note: 如果欄位是實值型別，則會複製出欄位的複本。 如果欄位是參考型別，將只會複製參考!
        /// </summary>
        /// <returns></returns>
        public Algo_Image Clone()
        {
            return this.MemberwiseClone() as Algo_Image;
        }
        
        /// <summary>
        /// 物件複製 (手動暴力法)
        /// </summary>
        /// <param name="b_HObject_Clone">ImageSource是否複製到新記憶體</param>
        /// <returns></returns>
        public Algo_Image Clone_Manual(bool b_HObject_Clone = false)
        {
            Algo_Image algoImage = new Algo_Image();

            if (b_HObject_Clone)
            {
                for (int i = 0; i < this.ImageSource.Count; i++)
                    algoImage.ImageSource.Add(this.ImageSource[i].Clone());
            }
            else
                algoImage.ImageSource = this.ImageSource;

            for (int i = 0; i < this.ListAlgoImage.Count; i++)
            {
                algoImage.ListAlgoImage.Add(this.ListAlgoImage[i].Clone_Manual());
                //algoImage.ListAlgoImage.Add(this.ListAlgoImage[i].DeepClone());
            }

            algoImage.B_UsedRegion = this.B_UsedRegion;

            if (b_HObject_Clone)
            {
                for (int i = 0; i < this.UsedRegionList.Count; i++)
                    algoImage.UsedRegionList.Add(this.UsedRegionList[i].Clone());
            }
            else
                algoImage.UsedRegionList = this.UsedRegionList;

            algoImage.Name_UsedRegionList = this.Name_UsedRegionList.ToList();

            return algoImage;
        }

        /// <summary>
        /// 釋放記憶體 (class內所有HObject物件)
        /// </summary>
        /// <param name="b_Dispose_ImageSource"></param>
        /// <param name="b_Dispose_UsedRegionList"></param>
        public void Dispose(bool b_Dispose_ImageSource = true, bool b_Dispose_UsedRegionList = false)
        {
            if (b_Dispose_ImageSource)
            {
                clsStaticTool.DisposeAll(this.ImageSource);
                ImageSource = new List<HObject>();
            }
            if (b_Dispose_UsedRegionList)
            {
                clsStaticTool.DisposeAll(this.UsedRegionList);
                this.UsedRegionList = new List<HObject>();
            }
            foreach (Single_AlgoImage AlgoImage in this.ListAlgoImage)
                AlgoImage.Dispose();
        }

        /// <summary>
        /// 【清空】所有演算法
        /// </summary>
        public void RemoveAll_Algo()
        {
            foreach (Single_AlgoImage AlgoImage in this.ListAlgoImage)
                AlgoImage.Dispose();
            this.ListAlgoImage.Clear();
        }

        #endregion
    }

    [Serializable]
    public class Single_AlgoImage
    {
        #region 參數

        /// <summary>
        /// 編號
        /// </summary>
        public int ID { get; set; } = 0;

        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// 是否為 影像演算法，false 則為 區域演算法
        /// </summary>
        public bool B_ImageAlgo { get; set; } = true; // (20200729) Jeff Revised!

        /// <summary>
        /// 影像演算法
        /// </summary>
        public enu_Algo_Image Algo { get; set; } = enu_Algo_Image.直方圖等化;

        /// <summary>
        /// 區域演算法 類型
        /// </summary>
        public enuTools Type_Algo { get; set; } = enuTools.RegionSets; // (20200729) Jeff Revised!

        /// <summary>
        /// 區域演算法 之 Index
        /// </summary>
        public int Index_Algo_Region { get; set; } = 0; // (20200729) Jeff Revised!

        /// <summary>
        /// 輸入影像數量
        /// </summary>
        public int CountImg { get; set; } = 1;

        /// <summary>
        /// 演算法之各輸入影像是否為原始影像
        /// Note: 如果false，則為其他演算法計算結果影像
        /// </summary>
        public List<bool> B_ImageSource { get; set; } = new List<bool>();

        /// <summary>
        /// 各輸入影像在 (原始影像/其他演算法計算結果影像) 之ID編號
        /// </summary>
        public List<int> ID_InputImage { get; set; } = new List<int>();

        /// <summary>
        /// 各輸入影像對應之Band
        /// </summary>
        public List<enuBand> Bands { get; set; } = new List<enuBand>();

        /// <summary>
        /// 輸入區域數量
        /// </summary>
        public int CountReg { get; set; } = 0; // (20200729) Jeff Revised!

        /// <summary>
        /// 演算法之各輸入區域是否為 其他演算法計算結果區域
        /// Note: 如果false，則為 使用範圍
        /// </summary>
        public List<bool> B_RegionSource { get; set; } = new List<bool>(); // (20200729) Jeff Revised!

        /// <summary>
        /// 各輸入區域在 (其他演算法計算結果區域/使用範圍) 之ID編號
        /// </summary>
        public List<int> ID_InputRegion { get; set; } = new List<int>(); // (20200729) Jeff Revised!

        /// <summary>
        /// 該 影像/區域  演算法類型對應之輸入參數
        /// </summary>
        [XmlIgnore] // 帶有XmlIgnore，表示序列化時不序列化此屬性
        public Hashtable Parameters { get; set; } = new Hashtable();

        /// <summary>
        /// 序列化 & 反序列化 Hashtable類型之 Parameters
        /// </summary>
        public object[] Array_KeyValue
        {
            get // 序列化
            {
                List<object> list_KeyValue = new List<object>();
                foreach (DictionaryEntry pair in this.Parameters)
                {
                    list_KeyValue.Add(pair.Key);
                    list_KeyValue.Add(pair.Value);
                }
                return list_KeyValue.ToArray();
            }

            set // 反序列化
            {
                this.Dispose(); // 釋放記憶體
                this.Parameters.Clear();
                this.Parameters = new Hashtable();
                List<object> list_KeyValue = value.ToList();
                for (int i = 0; i < list_KeyValue.Count - 1; i += 2)
                    this.Parameters.Add(list_KeyValue[i], list_KeyValue[i + 1]);
            }
        }

        /// <summary>
        /// 是否使用編輯範圍 (只對有ROI之影像演算法 or 區域演算法B)
        /// </summary>
        public bool B_UsedRegion { get; set; } = false; // (20200119) Jeff Revised!

        #endregion

        #region 方法

        public Single_AlgoImage() { }

        /// <summary>
        /// 代替 建構子，初始化 之功能 
        /// Note: 建構式有參數輸入時，會無法儲存到XML!
        /// </summary>
        /// <param name="id">編號</param>
        /// <param name="name">名稱</param>
        /// <param name="index_algo">演算法之index</param>
        /// <param name="b_UsedRegion"></param>
        /// <param name="b_ImageAlgo">true: 影像演算法, false: 區域演算法</param>
        /// <param name="index_Type_Algo">演算法類型之index</param>
        public void Single_AlgoImage_Constructor(int id, string name, int index_algo, bool b_UsedRegion = false, bool b_ImageAlgo = true, int index_Type_Algo = 0) // (20200729) Jeff Revised!
        {
            this.ID = id;
            this.Name = name;
            this.B_UsedRegion = b_UsedRegion;

            this.Initialize_Algo(b_ImageAlgo, index_algo, index_Type_Algo);
        }

        /// <summary>
        /// 初始化該演算法所需: 輸入影像/區域數量、輸入參數
        /// </summary>
        /// <param name="b_ImageAlgo">true: 影像演算法, false: 區域演算法</param>
        /// <param name="index_algo">演算法之index</param>
        /// <param name="index_Type_Algo">演算法類型之index</param>
        public void Initialize_Algo(bool b_ImageAlgo, int index_algo, int index_Type_Algo = 0) // (20200729) Jeff Revised!
        {
            this.B_ImageAlgo = b_ImageAlgo;
            int countImg = 1; // 輸入影像個數，如果為0，則預留一張作為顯示用途
            int countReg = 0; // (20200729) Jeff Revised!
            this.Dispose(); // 釋放記憶體
            this.Parameters.Clear();

            if (b_ImageAlgo) // 影像演算法
            {
                #region 各類型影像演算法

                this.Algo = (enu_Algo_Image)(index_algo);
                switch (this.Algo)
                {
                    case enu_Algo_Image.轉換影像型態:
                        {
                            countImg = 1;
                            Parameters.Add("newType", enu_ImageType._byte.ToString().Substring(1));
                        }
                        break;
                    case enu_Algo_Image.創建影像:
                        {
                            countImg = 0;
                            Parameters.Add("newType", enu_ImageType._byte.ToString().Substring(1));
                            Parameters.Add("width", (int)512);
                            Parameters.Add("height", (int)512);
                        }
                        break;
                    case enu_Algo_Image.影像ROI:
                        {
                            countImg = 1;
                            if (!this.B_UsedRegion) // (20200119) Jeff Revised!
                            {
                                Parameters.Add("Type_ROI", HDrawingObject.HDrawingObjectType.RECTANGLE1.ToString());
                                HObject reg;
                                HOperatorSet.GenRectangle1(out reg, 0, 0, 299, 299); // 預設region
                                Parameters.Add("region", reg);
                            }
                            else
                            {
                                Parameters.Add("SelectRegionIndex", -1);
                                Parameters.Add("b_ROI_Invert", false);
                            }
                        }
                        break;
                    case enu_Algo_Image.影像分割: // (20200310) Jeff Revised!
                        {
                            countImg = 1;
                            Parameters.Add("x1", (int)0);
                            Parameters.Add("y1", (int)0);
                            Parameters.Add("x2", (int)0);
                            Parameters.Add("y2", (int)0);
                        }
                        break;
                    case enu_Algo_Image.Paint_Gray:
                        {
                            countImg = 2;
                        }
                        break;
                    case enu_Algo_Image.Paint_Region:
                        {
                            countImg = 1;
                            Parameters.Add("grayval", (double)255.0);
                            Parameters.Add("PaintType", enu_PaintType.fill.ToString());
                            Parameters.Add("b_ROI", false);
                            if (!this.B_UsedRegion) // (20200119) Jeff Revised!
                            {
                                Parameters.Add("Type_ROI", HDrawingObject.HDrawingObjectType.RECTANGLE1.ToString());
                                HObject reg;
                                HOperatorSet.GenRectangle1(out reg, 0, 0, 299, 299); // 預設region
                                Parameters.Add("region", reg);
                            }
                            else
                            {
                                Parameters.Add("SelectRegionIndex", -1);
                                Parameters.Add("b_ROI_Invert", false);
                            }
                        }
                        break;
                    case enu_Algo_Image.Crop_Domain:
                        {
                            countImg = 1;
                        }
                        break;
                    case enu_Algo_Image.Zoom_Factor:
                        {
                            countImg = 1;
                            Parameters.Add("scaleWidth", (double)0.5);
                            Parameters.Add("scaleHeight", (double)0.5);
                            Parameters.Add("interpolation", enu_interpolation.constant.ToString());
                        }
                        break;
                    case enu_Algo_Image.Zoom_Size:
                        {
                            countImg = 1;
                            Parameters.Add("width", (int)512);
                            Parameters.Add("height", (int)512);
                            Parameters.Add("interpolation", enu_interpolation.constant.ToString());
                        }
                        break;
                    case enu_Algo_Image.Scale:
                        {
                            countImg = 1;
                            Parameters.Add("mult", (double)0.01);
                            Parameters.Add("add", (double)0.0);
                        }
                        break;
                    case enu_Algo_Image.Scale_Max:
                        {
                            countImg = 1;
                        }
                        break;
                    case enu_Algo_Image.直方圖等化:
                        {
                            countImg = 1;
                        }
                        break;
                    case enu_Algo_Image.分區直方圖等化:
                        {
                            countImg = 1;
                            Parameters.Add("count_row", (int)1);
                            Parameters.Add("count_col", (int)1);
                            Parameters.Add("b_ROI", false);
                            if (!this.B_UsedRegion) // (20200119) Jeff Revised!
                            {
                                Parameters.Add("Type_ROI", HDrawingObject.HDrawingObjectType.RECTANGLE1.ToString());
                                HObject reg;
                                HOperatorSet.GenRectangle1(out reg, 0, 0, 299, 299); // 預設region
                                Parameters.Add("region", reg);
                            }
                            else
                            {
                                Parameters.Add("SelectRegionIndex", -1);
                                Parameters.Add("b_ROI_Invert", false);
                            }
                        }
                        break;
                    case enu_Algo_Image.組合RGB影像:
                        {
                            countImg = 3;
                        }
                        break;
                    case enu_Algo_Image.影像相加:
                        {
                            countImg = 2;
                            Parameters.Add("mult", (double)0.5);
                            Parameters.Add("add", (double)0.0);
                        }
                        break;
                    case enu_Algo_Image.影像相減:
                        {
                            countImg = 2;
                            Parameters.Add("mult", (double)1.0);
                            Parameters.Add("add", (double)128.0);
                        }
                        break;
                    case enu_Algo_Image.影像相乘:
                        {
                            countImg = 2;
                            Parameters.Add("mult", (double)0.005);
                            Parameters.Add("add", (double)0.0);
                        }
                        break;
                    case enu_Algo_Image.影像相除:
                        {
                            countImg = 2;
                            Parameters.Add("mult", (double)255.0);
                            Parameters.Add("add", (double)0.0);
                        }
                        break;
                    case enu_Algo_Image.帶通濾波:
                        {
                            countImg = 1;
                            Parameters.Add("filterType", enu_BandpassImage_filterType.lines.ToString());
                        }
                        break;
                    case enu_Algo_Image.高通濾波:
                        {
                            countImg = 1;
                            Parameters.Add("width", (int)9);
                            Parameters.Add("height", (int)9);
                        }
                        break;
                    case enu_Algo_Image.反向:
                        {
                            countImg = 1;
                        }
                        break;
                    case enu_Algo_Image.Max:
                        {
                            countImg = 2;
                        }
                        break;
                    case enu_Algo_Image.Min:
                        {
                            countImg = 2;
                        }
                        break;
                    case enu_Algo_Image.影像絕對值:
                        {
                            countImg = 1;
                        }
                        break;
                    case enu_Algo_Image.影像絕對誤差:
                        {
                            countImg = 2;
                            Parameters.Add("mult", (double)1.0);
                        }
                        break;
                    case enu_Algo_Image.sine:
                        {
                            countImg = 1;
                        }
                        break;
                    case enu_Algo_Image.cosine:
                        {
                            countImg = 1;
                        }
                        break;
                    case enu_Algo_Image.Dots:
                        {
                            countImg = 1;
                            Parameters.Add("diameter", (int)5);
                            Parameters.Add("filterType", enu_DotsImage_filterType.light.ToString());
                            Parameters.Add("pixelShift", (int)0);
                        }
                        break;
                    case enu_Algo_Image.FFT:
                        {
                            countImg = 1;
                        }
                        break;
                    case enu_Algo_Image.IFFT:
                        {
                            countImg = 1;
                        }
                        break;
                    default:
                        break;
                }

                #endregion
            }
            else // 區域演算法
            {
                #region 各類型區域演算法

                this.Type_Algo = (enuTools)index_Type_Algo;
                this.Index_Algo_Region = index_algo;
                if (this.Type_Algo == enuTools.Threshold)
                {
                    #region Threshold
                    
                    countReg = 0;
                    switch ((enuThreshold)index_algo)
                    {
                        case enuThreshold.一般二值化:
                            {
                                Parameters.Add("ThMin", (int)128);
                                Parameters.Add("ThMax", (int)255);
                            }
                            break;
                        case enuThreshold.Dual二值化:
                            {
                                Parameters.Add("MinSize", (int)20);
                                Parameters.Add("MinGray", (double)5.0);
                                Parameters.Add("Threshold", (double)2.0);
                            }
                            break;
                        case enuThreshold.自動二值化:
                            {
                                Parameters.Add("Sigma", (double)2.0);
                            }
                            break;
                        case enuThreshold.動態二值化:
                            {
                                Parameters.Add("MaskWidth", (int)9);
                                Parameters.Add("MaskHeight", (int)9);
                                Parameters.Add("Offset", (int)5);
                                Parameters.Add("LightDark", enu_LightDark.light.ToString());
                            }
                            break;
                        default:
                            break;
                    }

                    #endregion
                }
                else if (this.Type_Algo == enuTools.RegionSets)
                {
                    #region RegionSets
                    
                    switch ((enuRegionSets)index_algo)
                    {
                        case enuRegionSets.CreateROI:
                            {
                                countReg = 0;

                                if (!this.B_UsedRegion)
                                {
                                    Parameters.Add("Type_ROI", HDrawingObject.HDrawingObjectType.RECTANGLE1.ToString());
                                    HObject reg;
                                    HOperatorSet.GenRectangle1(out reg, 0, 0, 299, 299); // 預設region
                                    Parameters.Add("region", reg);
                                }
                                else
                                {
                                    Parameters.Add("b_ROI_UsedRegion", true);
                                    Parameters.Add("SelectRegionIndex", -1);
                                    Parameters.Add("b_ROI_Invert", false);
                                }
                            }
                            break;
                        case enuRegionSets.Union1:
                            {
                                countReg = 1;
                            }
                            break;
                        case enuRegionSets.Union2:
                            {
                                countReg = 2;
                            }
                            break;
                        case enuRegionSets.Difference:
                            {
                                countReg = 2;
                            }
                            break;
                        case enuRegionSets.Intersection:
                            {
                                countReg = 2;
                            }
                            break;
                        case enuRegionSets.Complement:
                            {
                                countReg = 1;
                            }
                            break;
                        case enuRegionSets.Connection:
                            {
                                countReg = 1;
                            }
                            break;
                        case enuRegionSets.fillup:
                            {
                                countReg = 1;
                            }
                            break;
                        case enuRegionSets.Copy:
                            {
                                countReg = 1;
                            }
                            break;
                        case enuRegionSets.Shapetrans:
                            {
                                countReg = 1;
                                Parameters.Add("ShapeTrans_Type", enu_ShapeTrans_Type.convex.ToString());
                            }
                            break;
                        case enuRegionSets.FillupShape:
                            {
                                countReg = 1;
                                Parameters.Add("FillUp_Feature", enu_FillUp_Feature.anisometry.ToString());
                                Parameters.Add("Min", (double)1.0);
                                Parameters.Add("Max", (double)100.0);
                            }
                            break;
                        case enuRegionSets.Concat:
                            {
                                countReg = 2;
                            }
                            break;
                        default:
                            break;
                    }

                    #endregion
                }
                else if (this.Type_Algo == enuTools.Morphology)
                {
                    #region Morphology
                    
                    countReg = 1;
                    enuMorphology algo = (enuMorphology)index_algo;
                    if ((algo == enuMorphology.opening_rect1) || (algo == enuMorphology.closing_rect1) || (algo == enuMorphology.dilation_rect1) || (algo == enuMorphology.erosion_rect1))
                    {
                        Parameters.Add("Width", (int)10);
                        Parameters.Add("Height", (int)10);
                    }
                    else
                        Parameters.Add("Radius", (double)3.5);

                    #endregion
                }
                else
                {
                    #region Select
                    
                    countReg = 1;
                    //enuSelect algo = (enuSelect)index_algo;
                    //if ((algo == enuSelect.min) || (algo == enuSelect.max) || (algo == enuSelect.mean) || (algo == enuSelect.deviation) || (algo == enuSelect.entropy))
                    //    countImg = 1;
                    Parameters.Add("Min", (double)128.0);
                    Parameters.Add("Max", (double)255.0);
                    
                    #endregion
                }

                #endregion
            }

            // 更新影像數量
            this.Update_CountImg(countImg, countReg);
        }

        /// <summary>
        /// 更新影像數量
        /// </summary>
        /// <param name="countImg">輸入影像數量</param>
        /// <param name="countReg">輸入區域數量</param>
        public void Update_CountImg(int countImg, int countReg)
        {
            this.CountImg = countImg;
            this.CountReg = countReg;

            #region 初始化 List<>

            // 影像
            this.B_ImageSource.Clear();
            this.ID_InputImage.Clear();
            this.Bands.Clear();
            for (int i = 0; i < countImg; i++)
            {
                this.B_ImageSource.Add(true);
                this.ID_InputImage.Add(0);
                this.Bands.Add(enuBand.None);
            }

            // 區域
            this.B_RegionSource.Clear();
            this.ID_InputRegion.Clear();
            for (int i = 0; i < countReg; i++)
            {
                this.B_RegionSource.Add(true);
                this.ID_InputRegion.Add(0);
            }

            #endregion
        }

        /// <summary>
        /// 儲存 Halcon Region
        /// </summary>
        /// <param name="directory_"></param>
        /// <returns></returns>
        public bool SaveHalconFile(string directory_)
        {
            bool b_status_ = false;
            try
            {
                if (this.Parameters.ContainsKey("region"))
                {
                    string path = directory_ + "Algo_ID" + ID.ToString() + "\\";
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    HOperatorSet.WriteRegion((HObject)this.Parameters["region"], path + "region" + ".hobj");
                }

                b_status_ = true;
            }
            catch (Exception ex)
            { }

            return b_status_;
        }

        /// <summary>
        /// 讀取 Halcon Region
        /// </summary>
        /// <param name="directory_"></param>
        /// <returns></returns>
        public bool ReadHalconFile(string directory_)
        {
            bool b_status_ = false;
            try
            {
                if (this.Parameters.ContainsKey("region"))
                {
                    string path = directory_ + "Algo_ID" + this.ID.ToString() + "\\";
                    if (Directory.Exists(path))
                    {
                        HObject reg;
                        HOperatorSet.ReadRegion(out reg, path + "region" + ".hobj");
                        this.Parameters["region"] = reg;
                    }
                    else
                        return false;
                }

                b_status_ = true;
            }
            catch (Exception ex)
            { }

            return b_status_;
        }

        /// <summary>
        /// 物件複製 (淺層)
        /// Note: 如果欄位是實值型別，則會複製出欄位的複本。 如果欄位是參考型別，將只會複製參考!
        /// </summary>
        /// <returns></returns>
        public Single_AlgoImage Clone()
        {
            return this.MemberwiseClone() as Single_AlgoImage;
        }

        /// <summary>
        /// 物件複製 (手動暴力法)
        /// </summary>
        /// <returns></returns>
        public Single_AlgoImage Clone_Manual() // (20200729) Jeff Revised!
        {
            Single_AlgoImage singleAlgoImage = new Single_AlgoImage();
            singleAlgoImage.ID = this.ID;
            singleAlgoImage.Name = this.Name;
            singleAlgoImage.B_ImageAlgo = this.B_ImageAlgo;
            singleAlgoImage.Algo = this.Algo;
            singleAlgoImage.Type_Algo = this.Type_Algo;
            singleAlgoImage.Index_Algo_Region = this.Index_Algo_Region;
            singleAlgoImage.CountImg = this.CountImg;
            singleAlgoImage.B_ImageSource = this.B_ImageSource.ToList();
            singleAlgoImage.ID_InputImage = this.ID_InputImage.ToList();
            singleAlgoImage.Bands = this.Bands.ToList();
            singleAlgoImage.CountReg = this.CountReg;
            singleAlgoImage.B_RegionSource = this.B_RegionSource.ToList();
            singleAlgoImage.ID_InputRegion = this.ID_InputRegion.ToList();

            // Hashtable 複製
            //singleAlgoImage.Parameters = (Hashtable)(this.Parameters.Clone()); // 物件複製 (淺層)
            //singleAlgoImage.Parameters = this.Parameters.DeepClone(); // 物件複製 (深層)
            singleAlgoImage.Parameters = clsStaticTool.DeepClone<Hashtable>(this.Parameters); // 物件複製 (深層)

            singleAlgoImage.B_UsedRegion = this.B_UsedRegion;

            return singleAlgoImage;
        }

        /// <summary>
        /// 釋放記憶體 (class內所有HObject物件)
        /// </summary>
        public void Dispose()
        {
            if (this.Parameters.ContainsKey("region"))
            {
                try
                {
                    if ((HObject)this.Parameters["region"] != null)
                        ((HObject)this.Parameters["region"]).Dispose();
                }
                catch (Exception ex)
                { }
            }
        }

        #endregion
    }
    
}
