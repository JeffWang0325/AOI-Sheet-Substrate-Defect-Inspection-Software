using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeltaSubstrateInspector.src.Modules.OperateMethods.InspectModule;
using DeltaSubstrateInspector.src.Roles;
using DeltaSubstrateInspector.src.Modules.InspectModule;

using HalconDotNet;

namespace DeltaSubstrateInspector.src.Modules.NewInspectModule.DefaultInspect
{
    public class DefaultInspect:OperateMethod
    {
        // 標準函示框架 **************************************************************************************************************

        #region Role        
        public DefaultInspectRole methRole = new DefaultInspectRole();

        #endregion

        #region 標準函示框架

        /// <summary>
        /// 與主軟體對接的視覺演算法
        /// </summary>
        /// <param name="role"></param>
        /// <param name="src_imgs"></param>
        /// <returns></returns>
        public override HObject get_defect_rgn(InspectRole role, List<HObject> src_imgs)
        {

            // 檢測瑕疵Region
            HObject DefectRegion;


            // 設定參數
            set_parameter((DefaultInspectRole)role);

            // 執行演算法
            execute(src_imgs, out DefectRegion);

            // 回傳檢測結果
            return DefectRegion;

        }

        /// <summary>
        /// 181203, andy: 演算法回傳每個cell的 region
        /// </summary>
        /// <param name="role"></param>
        /// <param name="src_imgs"></param>
        /// <returns></returns>
        public override HObject get_cell_rgn(InspectRole role, List<HObject> src_imgs)
        {
            HObject CellRegion;
            CellRegion = null;
            HOperatorSet.GenEmptyObj(out CellRegion);

            // Get...

            return CellRegion;
        }

        // 181218, andy
        public override List<Defect> get_defect_result(InspectRole role, List<HObject> src_imgs)
        {
            List<Defect> defectResult = new List<Defect>();

            // 設定參數
            set_parameter((DefaultInspectRole)role);

            // Defect1: DarkDefect
            string defectName1 = "DarkDefect";
            HObject defectObj1 = null;
            HOperatorSet.GenCircle(out defectObj1, 20, 20, 50);
            HObject defectObj1_rgnCenter = null;
            HOperatorSet.ShapeTrans(defectObj1,out defectObj1_rgnCenter, "inner_center");
            Defect defect1 = new Defect(defectName1, src_imgs[0].Clone(), defectObj1, defectObj1_rgnCenter);

            // Defect2: DarkDefect
            string defectName2 = "LightDefect";
            HObject defectObj2 = null;
            HOperatorSet.GenCircle(out defectObj2, 100, 100, 100);
            HObject defectObj2_rgnCenter = null;
            HOperatorSet.ShapeTrans(defectObj2, out defectObj2_rgnCenter, "inner_center");
            Defect defect2 = new Defect(defectName2, src_imgs[0].Clone(), defectObj2, defectObj2_rgnCenter);

            // Add to list
            defectResult.Add(defect1);
            defectResult.Add(defect2);

            // return
            return defectResult;

        }

        /// <summary>
        /// 設定參數
        /// </summary>
        /// <param name="role"></param>
        public void set_parameter(DefaultInspectRole role)
        {
            methRole = role;
        }

        /// <summary>
        /// 演算法實作
        /// </summary>
        /// <param name="ho_ImgList"></param>
        /// <param name="ho_DefectRegion"></param>
        public void execute(List<HObject> ho_ImgList, out HObject ho_DefectRegion)
        {
            
            HOperatorSet.GenEmptyRegion(out ho_DefectRegion);

            if (methRole.InspectBypass == true)
            {
                return;

            }
            else
            {
                // 取得影像
                HObject ho_Img = null;
                HOperatorSet.GenEmptyObj(out ho_Img);
                int inspectImgIndex =0;
                ho_Img = ho_ImgList[inspectImgIndex].Clone();

                // 進行檢測 ... 
                // ....

                // 產生一個方框模擬檢測結果
                HTuple width, height;
                HOperatorSet.GetImageSize(ho_Img, out width, out height);
                int size = 50;
                //HOperatorSet.GenRectangle1(out ho_DefectRegion, width / 2 - size, height/2- size, width / 2 + size, height/2+ size);

                HOperatorSet.GenCircle(out ho_DefectRegion, height / 2, width / 2, size);
            }

        }

        /// <summary>
        /// UserControl測試演算法完整流程
        /// </summary>
        /// <param name="input_ImgList"></param>
        /// <param name="result_image"></param>
        public void action(List<HObject> input_ImgList, out HObject result_image)
        {
            result_image = null;

            // Local iconic variables 
            HObject ho_DefectRegion_in;

            int inspectImgIndex = 0;
            result_image = input_ImgList[inspectImgIndex].Clone();

            execute(input_ImgList, out ho_DefectRegion_in);

            // 將各種瑕疵區域畫在原始影像上
            HOperatorSet.OverpaintRegion(result_image, ho_DefectRegion_in, ((new HTuple(255)).TupleConcat(0)).TupleConcat(0), "margin"); // green

            ho_DefectRegion_in.Dispose();


        }

        #endregion




    }
}
