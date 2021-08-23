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

namespace DeltaSubstrateInspector.src.Modules.NewInspectModule.HTResistor0201
{
    public class HTResistor0201Role : InspectRole
    {
        #region 演算法參數
        //public HTuple hv_MinScore = new HTuple(); // 樣板比對分數
        //public HTuple hv_EnhancedPatternMatch = new HTuple(); // 樣板比對強化方式
        public int InspectImgIndex = 0; // 檢測時使用的影像id

        public string strAreaLower_BrightBlobBL;
        public string strAreaUpper_BrightBlobBL;
        public string strThresholdOffset_BrightBlobBL;

        public bool bolVertClose_BlackCrackFL;
        public string strAreaLower_BlackCrackFL;
        public string strAreaUpper_BlackCrackFL;
        public string strCrackHeight_BlackCrackFL;
        public string strThresholdScale_BlackCrackFL;

        public bool bolVertClose_WhiteCrackFL;
        public string strAreaLower_WhiteCrackFL;
        public string strAreaUpper_WhiteCrackFL;
        public string strCrackWidth_WhiteCrackFL;
        public string strThresholdScale_WhiteCrackFL;

        public bool bolVertClose_WhiteBlobFL;
        public string strAreaLower_WhiteBlobFL;
        public string strAreaUpper_WhiteBlobFL;
        public string strBlobHeight_WhiteBlobFL;
        public string strThresholdScale_WhiteBlobFL;

        public string strMaskSize_BlackBlobCL;
        public string strAreaLower_BlackBlobCL;
        public string strAreaUpper_BlackBlobCL;
        public string strThresholdScale_BlackBlobCL;
        #endregion

        public HTResistor0201Role()
        {
            //hv_MinScore = 0.5;
            //hv_EnhancedPatternMatch = 0;
            //InspectImgIndex = 0;
        }

        public override object Clone()
        {
            HTResistor0201Role obj = new HTResistor0201Role();

            // Copy the content of original object and setup a new one
            obj.inspect_name_ = this.inspect_name_;
            obj.method_ = this.method_;

            // 演算法參數複製
            // ...

            return obj;
        }

        // ===========================================================================================================
        //public override object Clone()
        //{
        //    HTResistor0201Role obj = new HTResistor0201Role();

        //    // Copy the content of original object and setup a new one
        //    obj.inspect_name_ = this.inspect_name_;
        //    obj.method_ = this.method_;
        //    return obj;
        //}

        // ===========================================================================================================
        public override void save()
        {
            if (ModuleName == "") return; // 181031, andy

            XmlDocument doc = new XmlDocument();
            if (System.IO.File.Exists(ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\HTResistor0201\\HTResistor0201.xml"))
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
            //SetXMLElementToDocument(doc, eleInspect, "hv_MinScore", hv_MinScore.ToString());
            //SetXMLElementToDocument(doc, eleInspect, "hv_EnhancedPatternMatch", hv_EnhancedPatternMatch.ToString());
            //SetXMLElementToDocument(doc, eleInspect, "InspectImgIndex", InspectImgIndex.ToString());

            SetXMLElementToDocument(doc, eleInspect, "AreaLower_BrightBlobBL", strAreaLower_BrightBlobBL);
            SetXMLElementToDocument(doc, eleInspect, "AreaUpper_BrightBlobBL", strAreaUpper_BrightBlobBL);
            SetXMLElementToDocument(doc, eleInspect, "ThresholdOffset_BrightBlobBL", strThresholdOffset_BrightBlobBL);

            SetXMLElementToDocument(doc, eleInspect, "VertClose_BlackCrackFL", bolVertClose_BlackCrackFL.ToString());
            SetXMLElementToDocument(doc, eleInspect, "AreaLower_BlackCrackFL", strAreaLower_BlackCrackFL);
            SetXMLElementToDocument(doc, eleInspect, "AreaUpper_BlackCrackFL", strAreaUpper_BlackCrackFL);
            SetXMLElementToDocument(doc, eleInspect, "CrackHeight_BlackCrackFL", strCrackHeight_BlackCrackFL);
            SetXMLElementToDocument(doc, eleInspect, "ThresholdScale_BlackCrackFL", strThresholdScale_BlackCrackFL);

            SetXMLElementToDocument(doc, eleInspect, "VertClose_WhiteCrackFL", bolVertClose_WhiteCrackFL.ToString());
            SetXMLElementToDocument(doc, eleInspect, "AreaLower_WhiteCrackFL", strAreaLower_WhiteCrackFL);
            SetXMLElementToDocument(doc, eleInspect, "AreaUpper_WhiteCrackFL", strAreaUpper_WhiteCrackFL);
            SetXMLElementToDocument(doc, eleInspect, "CrackWidth_WhiteCrackFL", strCrackWidth_WhiteCrackFL);
            SetXMLElementToDocument(doc, eleInspect, "ThresholdScale_WhiteCrackFL", strThresholdScale_WhiteCrackFL);

            SetXMLElementToDocument(doc, eleInspect, "VertClose_WhiteBlobFL", bolVertClose_WhiteBlobFL.ToString());
            SetXMLElementToDocument(doc, eleInspect, "AreaLower_WhiteBlobFL", strAreaLower_WhiteBlobFL);
            SetXMLElementToDocument(doc, eleInspect, "AreaUpper_WhiteBlobFL", strAreaUpper_WhiteBlobFL);
            SetXMLElementToDocument(doc, eleInspect, "BlobHeight_WhiteBlobFL", strBlobHeight_WhiteBlobFL);
            SetXMLElementToDocument(doc, eleInspect, "ThresholdScale_WhiteBlobFL", strThresholdScale_WhiteBlobFL);

            SetXMLElementToDocument(doc, eleInspect, "MaskSize_BlackBlobCL", strMaskSize_BlackBlobCL);
            SetXMLElementToDocument(doc, eleInspect, "AreaLower_BlackBlobCL", strAreaLower_BlackBlobCL);
            SetXMLElementToDocument(doc, eleInspect, "AreaUpper_BlackBlobCL", strAreaUpper_BlackBlobCL);
            SetXMLElementToDocument(doc, eleInspect, "ThresholdScale_BlackBlobCL", strThresholdScale_BlackBlobCL);
            #endregion

            if (!Directory.Exists(ModuleParamDirectory + RecipeFileDirectory + ModuleName))
                Directory.CreateDirectory(ModuleParamDirectory + RecipeFileDirectory + ModuleName);

            if (!Directory.Exists(ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\HTResistor0201"))
                Directory.CreateDirectory(ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\HTResistor0201");

            doc.Save(ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\HTResistor0201\\HTResistor0201.xml");
        }

        // ===========================================================================================================
        public override void load()
        {
            if (!System.IO.File.Exists(ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\HTResistor0201\\HTResistor0201.xml")) return;

            XmlDocument doc = new XmlDocument();

            //try
            //{
            doc.Load(ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\HTResistor0201\\HTResistor0201.xml");
            //string index = "//Inspect[@Name='" + inspect_name_ + "']"; // 
            string andyMagicStr = "";
            string index = "//Inspect[@Name='" + andyMagicStr + "']";

            XmlNode node = doc.SelectSingleNode(index);

            #region 演算法參數載入
            // 模板比對(Shape Model)               
            //hv_MinScore = double.Parse(GetXmlContext(node.SelectSingleNode("hv_MinScore"), "0"));
            //hv_EnhancedPatternMatch = int.Parse(GetXmlContext(node.SelectSingleNode("hv_EnhancedPatternMatch"), "0"));
            //InspectImgIndex = int.Parse(GetXmlContext(node.SelectSingleNode("InspectImgIndex"), "0"));

            strAreaLower_BrightBlobBL       = GetXmlContext(node.SelectSingleNode("AreaLower_BrightBlobBL"), "0");
            strAreaUpper_BrightBlobBL       = GetXmlContext(node.SelectSingleNode("AreaUpper_BrightBlobBL"), "0");
            strThresholdOffset_BrightBlobBL = GetXmlContext(node.SelectSingleNode("ThresholdOffset_BrightBlobBL"), "0");

            strAreaLower_BlackCrackFL       = GetXmlContext(node.SelectSingleNode("AreaLower_BlackCrackFL"), "0");
            strAreaUpper_BlackCrackFL       = GetXmlContext(node.SelectSingleNode("AreaUpper_BlackCrackFL"), "0");
            strCrackHeight_BlackCrackFL     = GetXmlContext(node.SelectSingleNode("CrackHeight_BlackCrackFL"), "0");
            strThresholdScale_BlackCrackFL  = GetXmlContext(node.SelectSingleNode("ThresholdScale_BlackCrackFL"), "0");
            bolVertClose_BlackCrackFL       = bool.Parse(GetXmlContext(node.SelectSingleNode("VertClose_BlackCrackFL"), "0"));

            strAreaLower_WhiteCrackFL       = GetXmlContext(node.SelectSingleNode("AreaLower_WhiteCrackFL"), "0");
            strAreaUpper_WhiteCrackFL       = GetXmlContext(node.SelectSingleNode("AreaUpper_WhiteCrackFL"), "0");
            strCrackWidth_WhiteCrackFL      = GetXmlContext(node.SelectSingleNode("CrackWidth_WhiteCrackFL"), "0");
            strThresholdScale_WhiteCrackFL  = GetXmlContext(node.SelectSingleNode("ThresholdScale_WhiteCrackFL"), "0");
            bolVertClose_WhiteCrackFL       = bool.Parse(GetXmlContext(node.SelectSingleNode("VertClose_WhiteCrackFL"), "0"));

            strAreaLower_WhiteBlobFL        = GetXmlContext(node.SelectSingleNode("AreaLower_WhiteBlobFL"), "0");
            strAreaUpper_WhiteBlobFL        = GetXmlContext(node.SelectSingleNode("AreaUpper_WhiteBlobFL"), "0");
            strBlobHeight_WhiteBlobFL       = GetXmlContext(node.SelectSingleNode("BlobHeight_WhiteBlobFL"), "0");
            strThresholdScale_WhiteBlobFL   = GetXmlContext(node.SelectSingleNode("ThresholdScale_WhiteBlobFL"), "0");
            bolVertClose_WhiteBlobFL        = bool.Parse(GetXmlContext(node.SelectSingleNode("VertClose_WhiteBlobFL"), "0"));

            strMaskSize_BlackBlobCL         = GetXmlContext(node.SelectSingleNode("MaskSize_BlackBlobCL"), "0");
            strAreaLower_BlackBlobCL        = GetXmlContext(node.SelectSingleNode("AreaLower_BlackBlobCL"), "0");
            strAreaUpper_BlackBlobCL        = GetXmlContext(node.SelectSingleNode("AreaUpper_BlackBlobCL"), "0");
            strThresholdScale_BlackBlobCL   = GetXmlContext(node.SelectSingleNode("ThresholdScale_BlackBlobCL"), "0");
            #endregion
            //}
            //catch { }
        }
    }
}
