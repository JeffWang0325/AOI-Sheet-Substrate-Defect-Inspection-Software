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

namespace DeltaSubstrateInspector.src.Modules.NewInspectModule.DefaultInspect
{
    public class DefaultInspectRole : InspectRole
    {
        // [二次開發] 演算法名稱
        private string InspectionName = " DefaultInspect";

        #region [二次開發] 演算法參數

        public bool InspectBypass = true;

        #endregion

        public DefaultInspectRole()
        {
            // 初始化
            InspectBypass = true;

        }

        public override object Clone()
        {
            DefaultInspectRole obj = new DefaultInspectRole();

            // Copy the content of original object and setup a new one
            obj.inspect_name_ = this.inspect_name_;
            obj.method_ = this.method_;

            // 演算法參數複製
            // ...

            return obj;
        }

        public override void save()
        {


            #region 標準框架, 無須修改

            if (ModuleName == "") return; // 181031, andy

            XmlDocument doc = new XmlDocument();
            if (System.IO.File.Exists(ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\" + InspectionName + "\\" + InspectionName + ".xml"))
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

            #endregion


            #region [二次開發] 演算法參數儲存

            SetXMLElementToDocument(doc, eleInspect, "InspectBypass", InspectBypass.ToString());

           
            #endregion


            #region 標準框架, 無須修改

            // 建立目錄
            if (!Directory.Exists(ModuleParamDirectory + RecipeFileDirectory + ModuleName))
            {
                Directory.CreateDirectory(ModuleParamDirectory + RecipeFileDirectory + ModuleName);
            }
            if (!Directory.Exists(ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\" + InspectionName))
            {
                Directory.CreateDirectory(ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\" + InspectionName);
            }

            // 儲存
            doc.Save(ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\" + InspectionName + "\\" + InspectionName + ".xml");
            
            #endregion


        }

        public override void load()
        {
           

            try
            {
                #region 標準框架, 無須西改

                if (!System.IO.File.Exists(ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\" + InspectionName + "\\" + InspectionName + ".xml")) return;

                XmlDocument doc = new XmlDocument();
                doc.Load(ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\" + InspectionName + "\\" + InspectionName + ".xml");
                //string index = "//Inspect[@Name='" + inspect_name_ + "']"; // 
                string andyMagicStr = "";
                string index = "//Inspect[@Name='" + andyMagicStr + "']";
                XmlNode node = doc.SelectSingleNode(index);

                #endregion


                #region [二次開發] 演算法參數載入

                // 模板比對(Shape Model)             
                InspectBypass = bool.Parse(GetXmlContext(node.SelectSingleNode("InspectBypass"), "false"));

              
                #endregion


            }
            catch
            {

            }



        }


    }
}
