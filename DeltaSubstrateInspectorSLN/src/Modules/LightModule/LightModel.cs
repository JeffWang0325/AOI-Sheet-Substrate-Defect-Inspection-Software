using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeltaSubstrateInspector.src.Modules.LightModule
{
    public class LightModel
    {
        private Dictionary<string, HObject> defect_image_collection_ = new Dictionary<string, HObject>();

        public LightModel()
        {
            //Should set by user in the future
            HObject img;
            HOperatorSet.ReadImage(out img, "");


        }

        

       

    }

    #region Andy new
    public class LightValue
    {
        public static int Light01_Vaule = 100;
        public static int Light02_Vaule = 100;
        public static int Light03_Vaule = 100;
        public static int Light04_Vaule = 100;
    }
    public class LightMaxValueIndex
    {
        public static int Light01_MaxVauleIndex = 0;
        public static int Light02_MaxVauleIndex = 0;
        public static int Light03_MaxVauleIndex = 0;
        public static int Light04_MaxVauleIndex = 0;
    }
    #endregion

}
