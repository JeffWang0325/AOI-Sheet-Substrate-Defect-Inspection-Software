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

namespace DeltaSubstrateInspector.src.Modules.NewInspectModule.PatternCheck
{
    public class PatternCheckRole: InspectRole
    {
        // [二次開發] 演算法名稱
        private string InspectionName = "PatternCheck";

        #region [二次開發] 演算法參數

        public bool InspectBypass = false;

        #region PatternMatching
        
        public HTuple hv_MinScore = new HTuple(); // 樣板比對分數
        public HTuple hv_MaxOverlap = new HTuple(); // 樣板比對分數
        public HTuple hv_EnhancedPatternMatch = new HTuple(); // 樣板比對強化方式
        public int InspectImgIndex = 0; // 檢測時使用的影像id

        #endregion


        #region DAVS
        public int DAVS_Mode = 0; // 模式, 0:不啟用, 1:線上檢測, 2:離線學習
        public int DAVS_ImageFmt = 0;  // 存檔格式, 0: jpeg, 1:tiff
        public int DAVS_PredictMode = 0; // Predict模式, 0:bgr , 1: rgb
        public bool DAVS_PModeSaveOK = true; // PredictionMode OK影像儲存
        public bool DAVS_PModeSaveNG = true; // PredictionMode NG影像儲存
        public bool DAVS_TModeSaveAll = true; // TrainMode OK影像儲存

        #endregion

        #endregion


        public PatternCheckRole()
        {
            // 初始化
            InspectBypass = false;

            hv_MinScore = 0.5;
            hv_MaxOverlap = 0.5;
            hv_EnhancedPatternMatch = 0;
            InspectImgIndex = 0;

            DAVS_Mode = 0;
            DAVS_ImageFmt = 0;
            DAVS_PredictMode = 0;
            DAVS_PModeSaveOK = true;
            DAVS_PModeSaveNG = true;
            DAVS_TModeSaveAll = true;

        }

        public override object Clone()
        {
            PatternCheckRole obj = new PatternCheckRole();

            // Copy the content of original object and setup a new one
            obj.inspect_name_ = this.inspect_name_;
            obj.method_ = this.method_;

            // 演算法參數複製
            // ...

            return obj;
        }

        public override void save()
        {
            if (ModuleName == "") return; // 181031, andy


            XmlDocument doc = new XmlDocument();           
            if (System.IO.File.Exists(ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\"+ InspectionName + "\\" + InspectionName + ".xml"))
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

            #region [二次開發] 演算法參數儲存
            
            SetXMLElementToDocument(doc, eleInspect, "InspectBypass", InspectBypass.ToString());

            SetXMLElementToDocument(doc, eleInspect, "hv_MinScore", hv_MinScore.ToString());
            SetXMLElementToDocument(doc, eleInspect, "hv_MaxOverlap", hv_MaxOverlap.ToString());
            SetXMLElementToDocument(doc, eleInspect, "hv_EnhancedPatternMatch", hv_EnhancedPatternMatch.ToString());
            SetXMLElementToDocument(doc, eleInspect, "InspectImgIndex", InspectImgIndex.ToString());
            SetXMLElementToDocument(doc, eleInspect, "DAVS_Mode", DAVS_Mode.ToString());
            SetXMLElementToDocument(doc, eleInspect, "DAVS_ImageFmt", DAVS_ImageFmt.ToString());
            SetXMLElementToDocument(doc, eleInspect, "DAVS_PredictMode", DAVS_PredictMode.ToString());
            SetXMLElementToDocument(doc, eleInspect, "DAVS_PModeSaveOK", DAVS_PModeSaveOK.ToString());
            SetXMLElementToDocument(doc, eleInspect, "DAVS_PModeSaveNG", DAVS_PModeSaveNG.ToString());
            SetXMLElementToDocument(doc, eleInspect, "DAVS_TModeSaveAll", DAVS_TModeSaveAll.ToString());

            #endregion


            // 建立目錄
            if (!Directory.Exists(ModuleParamDirectory + RecipeFileDirectory + ModuleName))
            {
                Directory.CreateDirectory(ModuleParamDirectory + RecipeFileDirectory + ModuleName);
            }
            if (!Directory.Exists(ModuleParamDirectory + RecipeFileDirectory + ModuleName+ "\\"+ InspectionName))
            {
                Directory.CreateDirectory(ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\"+ InspectionName);
            }

            // 儲存
            doc.Save(ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\" + InspectionName + "\\" + InspectionName + ".xml");


        }

        public override void load()
        {            
            if (!System.IO.File.Exists(ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\" + InspectionName + "\\" + InspectionName + ".xml")) return;

            XmlDocument doc = new XmlDocument();

            try
            {
                doc.Load(ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\" + InspectionName + "\\" + InspectionName + ".xml");
                //string index = "//Inspect[@Name='" + inspect_name_ + "']"; // 
                string andyMagicStr = "";
                string index = "//Inspect[@Name='" + andyMagicStr + "']";

                XmlNode node = doc.SelectSingleNode(index);


                #region [二次開發] 演算法參數載入

                // 模板比對(Shape Model)             
                InspectBypass =  bool.Parse(GetXmlContext(node.SelectSingleNode("InspectBypass"), "false"));

                hv_MinScore = double.Parse(GetXmlContext(node.SelectSingleNode("hv_MinScore"), "0"));
                hv_MaxOverlap = double.Parse(GetXmlContext(node.SelectSingleNode("hv_MaxOverlap"), "0"));
                hv_EnhancedPatternMatch = int.Parse(GetXmlContext(node.SelectSingleNode("hv_EnhancedPatternMatch"), "0"));
                InspectImgIndex = int.Parse(GetXmlContext(node.SelectSingleNode("InspectImgIndex"), "0"));

                DAVS_Mode = int.Parse(GetXmlContext(node.SelectSingleNode("DAVS_Mode"), "0"));
                DAVS_ImageFmt = int.Parse(GetXmlContext(node.SelectSingleNode("DAVS_ImageFmt"), "0"));
                DAVS_PredictMode = int.Parse(GetXmlContext(node.SelectSingleNode("DAVS_PredictMode"), "0"));
                DAVS_PModeSaveOK = bool.Parse(GetXmlContext(node.SelectSingleNode("DAVS_PModeSaveOK"), "true"));
                DAVS_PModeSaveNG = bool.Parse(GetXmlContext(node.SelectSingleNode("DAVS_PModeSaveNG"), "true"));
                DAVS_TModeSaveAll = bool.Parse(GetXmlContext(node.SelectSingleNode("DAVS_TModeSaveAll"), "true"));


                #endregion


            }
            catch { }



        }


    }
}
