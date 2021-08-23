using HalconDotNet;
using DeltaSubstrateInspector.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extension;
using System.Xml;
using static DeltaSubstrateInspector.FileSystem.FileSystem;
using System.IO;
using System.Windows.Forms;
using System.Collections;

using DeltaSubstrateInspector.src.Modules.OperateMethods.InspectModule;
using DeltaSubstrateInspector.src.Roles;
using DeltaSubstrateInspector.src.Modules.MotionModule;
using DeltaSubstrateInspector.src.Modules.LightModule;

using DeltaSubstrateInspector.src.Modules.NewInspectModule.PatternCheck;
using DeltaSubstrateInspector.src.Modules.NewInspectModule.DefaultInspect;
using DeltaSubstrateInspector.src.Modules.NewInspectModule.HTResistor0201;
using DeltaSubstrateInspector.src.Modules.NewInspectModule.HTResistor0402;
using DeltaSubstrateInspector.src.Modules.NewInspectModule.InductanceInsp;
using DeltaSubstrateInspector.src.Modules.NewInspectModule.ResistPanel;

namespace DeltaSubstrateInspector.src.Modules.InspectModule
{
    public class InspectModel
    {
        private List<InspectOperator> op_collection_ = new List<InspectOperator>();

        /// <summary>
        /// 該位置影像之總瑕疵
        /// </summary>
        private List<Defect> TotalResult_collection_ = new List<Defect>();
        /// <summary>
        /// 該位置影像之各類型瑕疵結果
        /// </summary>
        private Dictionary<string, List<Defect>> dic_result_collection_ = new Dictionary<string, List<Defect>>(); // 181218, andy


        private HObject result_img;
        private HObject afine_img_;
        private Hashtable roles_table_ = new Hashtable();
        private List<string> position_info_ = new List<string>();

        // 瑕疵地圖參數 (在此class 只用在儲存)
        private string pos_cnt_ = "";
        private string row_ = "";
        private string col_ = "";
        private string bypass_ = "";

        public InspectModel()
        {            
          
            roles_table_.Add("DefaultInspect", new DefaultInspectRole()); // Andy
            roles_table_.Add("PatternCheck", new PatternCheckRole()); // Andy
            roles_table_.Add("HTResistor0201", new HTResistor0201Role()); // James Chien, 2018/11/14
            roles_table_.Add("HTResistor0402", new HTResistor0402Role()); // James Chien, 2018/11/21
            roles_table_.Add("InductanceInsp", new InductanceInspRole()); // Chris
            roles_table_.Add("ResistPanel", new ResistPanelRole()); // (20181227) Jeff Revised!
        }

        public void set_affine_img(HObject image)
        { 
            afine_img_ = image;
        }

        public void initialize()
        {
            foreach (var item in op_collection_)
            {
                item.release();
            }
            foreach (var item in TotalResult_collection_)
            {
                item.release();
            }

            TotalResult_collection_ = new List<Defect>();

            //-----------------------------------------------
            // 181218, andy
            for (int i = 0; i < dic_result_collection_.Count; i++)
            {
                //dic_result_collection_[i]
            }
            
        }

        public Dictionary<string, DefectsResult> Initialize_DefectsClassify() // (20190716) Jeff Revised!
        {
            return op_collection_[0].Initialize_DefectsClassify();
        }

        public void operator_execute(List<HObject> imgs, bool by_pass)
        {
            // TODO : (1) Get the same side of image object with op_collection
            //        (2) Do operate when the side is the same : op_collection.excute()

            int i = 0; // 用where去找檢側面相同的operator
            if (!by_pass)
            {
                op_collection_[i].excute_roles(imgs);
                TotalResult_collection_ = op_collection_[i].get_TotalResult();

                // 181218, andy
                dic_result_collection_ = op_collection_[i].get_dicDefectList();
            }
            else
            {
                TotalResult_collection_ = op_collection_[i].get_empty_result();

                // 181218, andy
                dic_result_collection_ = op_collection_[i].get_empty_dicDefectList();
            }

        }

        public void set_map_param(string pos_cnt, string row, string column, string bypass)
        {
            pos_cnt_ = pos_cnt;
            row_ = row;
            col_ = column;
            bypass_ = bypass;
        }

        // 181107, andy
        public void save_operator()
        {
            // 181107, andy
            if (ModuleName == "") return;
            
            if (!Directory.Exists(ModuleParamDirectory + RecipeFileDirectory + ModuleName))
            {
                Directory.CreateDirectory(ModuleParamDirectory + RecipeFileDirectory + ModuleName);
            }
            
            // ------

            if (op_collection_.Count > 0)
            {
                XmlDocument doc = new XmlDocument();
                for (int i = 0; i < op_collection_.Count; i++)
                {
                    XmlElement side = doc.CreateElement("Side");
                    side.SetAttribute("Side", op_collection_[i].side_.ToString());
                    doc.AppendChild(side);

                    XmlElement inspection = doc.CreateElement("Insepction");
                    inspection.SetAttribute("Name", op_collection_[i].OperatorInfo[0]);
                    inspection.SetAttribute("Position", op_collection_[i].OperatorInfo[1]);
                    inspection.SetAttribute("Measure", op_collection_[i].OperatorInfo[2]);
                    inspection.SetAttribute("InspectArea", op_collection_[i].OperatorInfo[3]);

                    XmlElement pos = doc.CreateElement("Position");
                    pos.SetAttribute("method", op_collection_[i].OperatorInfo[1]);
                    pos.SetAttribute("Info", op_collection_[i].PosAddInfo);
                    inspection.AppendChild(pos);

                    XmlElement laser_pos = doc.CreateElement("LaserPosition");
                    //pos.SetAttribute("method", op_collection_[i].OperatorInfo[1]);
                    laser_pos.SetAttribute("Info", op_collection_[i].LaserPosAddInfo);
                    inspection.AppendChild(laser_pos);


                    XmlElement map = doc.CreateElement("Map");
                    map.SetAttribute("Position", MovementPos.locat_pos.ToString());
                    map.SetAttribute("Row", MovementPos.row.ToString());
                    map.SetAttribute("Column", MovementPos.col.ToString());
                    map.SetAttribute("ByPass", MovementPos.bypass.ToString());
                    map.SetAttribute("TriggerCount", MovementPos.triggerCount.ToString());
                    inspection.AppendChild(map);

                    XmlElement light = doc.CreateElement("Light");
                    light.SetAttribute("Light01_Vaule", LightValue.Light01_Vaule.ToString());
                    light.SetAttribute("Light02_Vaule", LightValue.Light02_Vaule.ToString());
                    light.SetAttribute("Light03_Vaule", LightValue.Light03_Vaule.ToString());
                    light.SetAttribute("Light04_Vaule", LightValue.Light04_Vaule.ToString());
                    light.SetAttribute("Light01_MaxVauleIndex", LightMaxValueIndex.Light01_MaxVauleIndex.ToString());
                    light.SetAttribute("Light02_MaxVauleIndex", LightMaxValueIndex.Light02_MaxVauleIndex.ToString());
                    light.SetAttribute("Light03_MaxVauleIndex", LightMaxValueIndex.Light03_MaxVauleIndex.ToString());
                    light.SetAttribute("Light04_MaxVauleIndex", LightMaxValueIndex.Light04_MaxVauleIndex.ToString());
                    inspection.AppendChild(light);

                    XmlElement finalInspect = doc.CreateElement("FinalInspect");
                    finalInspect.SetAttribute("ProductName", FinalInspectParam.ProductName); // (20200429) Jeff Revised!
                    finalInspect.SetAttribute("VisualPosEnable", FinalInspectParam.VisualPosEnable.ToString()); // (20191214) Jeff Revised!
                    finalInspect.SetAttribute("CellCenterInspectEnable", FinalInspectParam.CellCenterInspectEnable.ToString());
                    finalInspect.SetAttribute("GlobalMapImgID", FinalInspectParam.GlobalMapImgID.ToString());
                    finalInspect.SetAttribute("NGRatio", FinalInspectParam.NGRatio.ToString());

                    // 新增檢測模組 (其他參數) (20190719) Jeff Revised!
                    finalInspect.SetAttribute("PosImgID", FinalInspectParam.PosImgID.ToString());
                    finalInspect.SetAttribute("b_NG_classification", FinalInspectParam.b_NG_classification.ToString());
                    finalInspect.SetAttribute("b_NG_priority", FinalInspectParam.b_NG_priority.ToString());
                    finalInspect.SetAttribute("b_Recheck", FinalInspectParam.b_Recheck.ToString());
                    finalInspect.SetAttribute("dispWindow", FinalInspectParam.dispWindow);
                    finalInspect.SetAttribute("b_saveCSV", FinalInspectParam.b_saveCSV.ToString());
                    finalInspect.SetAttribute("b_SaveDefectResult", FinalInspectParam.b_SaveDefectResult.ToString());
                    finalInspect.SetAttribute("b_SaveAllCellImage", FinalInspectParam.b_SaveAllCellImage.ToString());
                    finalInspect.SetAttribute("b_SaveDefectCellImage", FinalInspectParam.b_SaveDefectCellImage.ToString());
                    finalInspect.SetAttribute("Directory_SaveDefectResult", FinalInspectParam.Directory_SaveDefectResult); // (20200429) Jeff Revised!
                    finalInspect.SetAttribute("b_Auto_AB", FinalInspectParam.b_Auto_AB.ToString()); // (20200429) Jeff Revised!
                    finalInspect.SetAttribute("A_Recipe", FinalInspectParam.A_Recipe); // (20200429) Jeff Revised!
                    finalInspect.SetAttribute("B_UpDownInvert_FS", FinalInspectParam.B_UpDownInvert_FS.ToString()); // (20200429) Jeff Revised!
                    finalInspect.SetAttribute("B_LeftRightInvert_FS", FinalInspectParam.B_LeftRightInvert_FS.ToString()); // (20200429) Jeff Revised!
                    finalInspect.SetAttribute("Directory_defComb", FinalInspectParam.Directory_defComb); // (20200429) Jeff Revised!
                    finalInspect.SetAttribute("OutputDirectory_defComb", FinalInspectParam.OutputDirectory_defComb); // (20200429) Jeff Revised!

                    finalInspect.SetAttribute("b_AICellImg_Enable", FinalInspectParam.b_AICellImg_Enable.ToString()); // (20190830) Jeff Revised!

                    // (20190902) Jeff Revised!
                    finalInspect.SetAttribute("DAVSImgID", FinalInspectParam.DAVSImgID.ToString());
                    finalInspect.SetAttribute("b_DAVSImgAlign", FinalInspectParam.b_DAVSImgAlign.ToString());
                    finalInspect.SetAttribute("b_DAVSMixImgBand", FinalInspectParam.b_DAVSMixImgBand.ToString());
                    finalInspect.SetAttribute("DAVSBand1ImgIndex", FinalInspectParam.DAVSBand1ImgIndex.ToString());
                    finalInspect.SetAttribute("DAVSBand2ImgIndex", FinalInspectParam.DAVSBand2ImgIndex.ToString());
                    finalInspect.SetAttribute("DAVSBand3ImgIndex", FinalInspectParam.DAVSBand3ImgIndex.ToString());
                    finalInspect.SetAttribute("DAVSBand1", ((int)(FinalInspectParam.DAVSBand1)).ToString());
                    finalInspect.SetAttribute("DAVSBand2", ((int)(FinalInspectParam.DAVSBand2)).ToString());
                    finalInspect.SetAttribute("DAVSBand3", ((int)(FinalInspectParam.DAVSBand3)).ToString());
                    
                    inspection.AppendChild(finalInspect);


                    List<string> role_names = op_collection_[i].get_roles_name();
                    List<string> role_methods = op_collection_[i].get_methods_name();
                    for (int j = 0; j < role_names.Count; j++)
                    {
                        XmlElement defect = doc.CreateElement("Defect");
                        defect.SetAttribute("method", role_methods[j]);
                        defect.SetAttribute("name", role_names[j]);
                        inspection.AppendChild(defect);

                        //op_collection_[i].Roles[j].save(); // 181031, andy remove

                    }

                    side.AppendChild(inspection);
                }


                if (!Directory.Exists(ModuleParamDirectory + RecipeFileDirectory + ModuleName))
                {
                    Directory.CreateDirectory(ModuleParamDirectory + RecipeFileDirectory + ModuleName);
                }
                doc.Save(ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\ModuleSetting.xml");
            }
            //op_collection_[0].Roles[0].load();

        }

        /* Operator Create and Modify*/
        public void operator_modify(int index)
        {
            try
            {
                if (op_collection_[index].Roles.Count == 0)
                    op_collection_[index].create_roles();
                else
                {
                    op_collection_[index].load();

                    // 181105, andy, 參數調整完後，直接再load 一次，同步參數檔!!!!!
                    load();

                }


            }
            catch(Exception e)
            {
                e.Message.ToString();
            }
            //else
            //    op_collection_[index].show_content();
        }

        public bool load()
        {
            op_collection_ = null;
            op_collection_ = new List<InspectOperator>();

            if (!System.IO.File.Exists(ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\ModuleSetting.xml"))
            {
                MessageBox.Show("參數檔損毀或不存在，請重新設定");
                return false;
            }
            else
            {
                XmlDocument doc = new XmlDocument();
               
                doc.Load(ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\ModuleSetting.xml");

                XmlNode op_node = doc.SelectSingleNode("Side/Insepction");
                XmlElement element = (XmlElement)op_node;
                //取得節點內的"部門名稱"內容
                string inspect_name = element.GetAttribute("Name");
                string inspect_pos = element.GetAttribute("Position");
                string inspect_measure = element.GetAttribute("Measure");
                string inspect_area = element.GetAttribute("Area");

                InspectOperator op = new InspectOperator(inspect_name, 0);
                op.OperatorInfo[0] = inspect_name;
                op.OperatorInfo[1] = inspect_pos;
                op.OperatorInfo[2] = inspect_measure;
                op.OperatorInfo[3] = inspect_area;

                XmlNode map_node = doc.SelectSingleNode("Side/Insepction/Map");
                XmlElement map = (XmlElement)map_node;
                MovementPos.locat_pos = Int32.Parse(map.GetAttribute("Position"));
                MovementPos.row = Int32.Parse(map.GetAttribute("Row"));
                MovementPos.col = Int32.Parse(map.GetAttribute("Column"));
                MovementPos.bypass = map.GetAttribute("ByPass");
                try
                {
                    MovementPos.triggerCount = Int32.Parse(map.GetAttribute("TriggerCount"));
                }
                catch (Exception)
                {
                    MovementPos.triggerCount = 2;
                }

                
                XmlNode light_node = doc.SelectSingleNode("Side/Insepction/Light");
                XmlElement light = (XmlElement)light_node;
                try
                {
                    LightValue.Light01_Vaule = Int32.Parse(light.GetAttribute("Light01_Vaule"));
                    LightValue.Light02_Vaule = Int32.Parse(light.GetAttribute("Light02_Vaule"));
                    LightValue.Light03_Vaule = Int32.Parse(light.GetAttribute("Light03_Vaule"));
                    LightValue.Light04_Vaule = Int32.Parse(light.GetAttribute("Light04_Vaule"));
                }
                catch (Exception)
                {
                    LightValue.Light01_Vaule = 100;
                    LightValue.Light02_Vaule = 100;
                    LightValue.Light03_Vaule = 100;
                    LightValue.Light04_Vaule = 100;
                }
                try
                {
                    LightMaxValueIndex.Light01_MaxVauleIndex = Int32.Parse(light.GetAttribute("Light01_MaxVauleIndex"));
                    LightMaxValueIndex.Light02_MaxVauleIndex = Int32.Parse(light.GetAttribute("Light02_MaxVauleIndex"));
                    LightMaxValueIndex.Light03_MaxVauleIndex = Int32.Parse(light.GetAttribute("Light03_MaxVauleIndex"));
                    LightMaxValueIndex.Light04_MaxVauleIndex = Int32.Parse(light.GetAttribute("Light04_MaxVauleIndex"));
                }
                catch (Exception)
                {
                    LightMaxValueIndex.Light01_MaxVauleIndex = 0;
                    LightMaxValueIndex.Light02_MaxVauleIndex = 0;
                    LightMaxValueIndex.Light03_MaxVauleIndex = 0;
                    LightMaxValueIndex.Light04_MaxVauleIndex = 0;
                }
                
                XmlNode finalInspect_node = doc.SelectSingleNode("Side/Insepction/FinalInspect");
                XmlElement finalInspect = (XmlElement)finalInspect_node;
                try
                { 
                    FinalInspectParam.GlobalMapImgID = Int32.Parse(finalInspect.GetAttribute("GlobalMapImgID"));
                    FinalInspectParam.NGRatio = double.Parse(finalInspect.GetAttribute("NGRatio"));
                }
                catch
                {
                    FinalInspectParam.GlobalMapImgID = 0;
                    FinalInspectParam.NGRatio = 100;
                }
                
                try // 新增檢測模組 (其他參數) (20190719) Jeff Revised!
                {
                    FinalInspectParam.CellCenterInspectEnable = bool.Parse(finalInspect.GetAttribute("CellCenterInspectEnable"));
                    FinalInspectParam.PosImgID = Int32.Parse(finalInspect.GetAttribute("PosImgID"));
                    FinalInspectParam.b_NG_classification = bool.Parse(finalInspect.GetAttribute("b_NG_classification"));
                    FinalInspectParam.b_NG_priority = bool.Parse(finalInspect.GetAttribute("b_NG_priority"));
                    FinalInspectParam.b_Recheck = bool.Parse(finalInspect.GetAttribute("b_Recheck"));
                    FinalInspectParam.dispWindow = finalInspect.GetAttribute("dispWindow");
                    FinalInspectParam.b_saveCSV = bool.Parse(finalInspect.GetAttribute("b_saveCSV"));
                    FinalInspectParam.b_SaveDefectResult = bool.Parse(finalInspect.GetAttribute("b_SaveDefectResult"));
                    FinalInspectParam.b_SaveAllCellImage = bool.Parse(finalInspect.GetAttribute("b_SaveAllCellImage"));
                    FinalInspectParam.b_SaveDefectCellImage = bool.Parse(finalInspect.GetAttribute("b_SaveDefectCellImage"));
                }
                catch
                {
                    FinalInspectParam.CellCenterInspectEnable = false;
                    FinalInspectParam.PosImgID = 0;
                    FinalInspectParam.b_NG_classification = false;
                    FinalInspectParam.b_NG_priority = false;
                    FinalInspectParam.b_Recheck = false;
                    FinalInspectParam.dispWindow = "pictureBox";
                    FinalInspectParam.b_saveCSV = false;
                    FinalInspectParam.b_SaveDefectResult = false;
                    FinalInspectParam.b_SaveAllCellImage = true;
                    FinalInspectParam.b_SaveDefectCellImage = false;
                }

                try // (20200429) Jeff Revised!
                {
                    FinalInspectParam.ProductName = finalInspect.GetAttribute("ProductName");
                }
                catch
                {
                    FinalInspectParam.ProductName = "A";
                }

                try
                {
                    FinalInspectParam.VisualPosEnable = bool.Parse(finalInspect.GetAttribute("VisualPosEnable")); // (20191214) Jeff Revised!
                }
                catch
                {
                    FinalInspectParam.VisualPosEnable = true;
                }

                try // (20190830) Jeff Revised!
                {
                    FinalInspectParam.b_AICellImg_Enable = bool.Parse(finalInspect.GetAttribute("b_AICellImg_Enable"));
                    // (20190902) Jeff Revised!
                    FinalInspectParam.DAVSImgID = int.Parse(finalInspect.GetAttribute("DAVSImgID"));
                    FinalInspectParam.b_DAVSImgAlign = bool.Parse(finalInspect.GetAttribute("b_DAVSImgAlign"));
                    FinalInspectParam.b_DAVSMixImgBand = bool.Parse(finalInspect.GetAttribute("b_DAVSMixImgBand"));
                    FinalInspectParam.DAVSBand1 = (enuBand)int.Parse(finalInspect.GetAttribute("DAVSBand1"));
                    FinalInspectParam.DAVSBand2 = (enuBand)int.Parse(finalInspect.GetAttribute("DAVSBand2"));
                    FinalInspectParam.DAVSBand3 = (enuBand)int.Parse(finalInspect.GetAttribute("DAVSBand3"));
                    FinalInspectParam.DAVSBand1ImgIndex = int.Parse(finalInspect.GetAttribute("DAVSBand1ImgIndex"));
                    FinalInspectParam.DAVSBand2ImgIndex = int.Parse(finalInspect.GetAttribute("DAVSBand2ImgIndex"));
                    FinalInspectParam.DAVSBand3ImgIndex = int.Parse(finalInspect.GetAttribute("DAVSBand3ImgIndex"));
                }
                catch
                {
                    FinalInspectParam.b_AICellImg_Enable = false;
                    // (20190902) Jeff Revised!
                    FinalInspectParam.DAVSImgID = 0;
                    FinalInspectParam.b_DAVSImgAlign = false;
                    FinalInspectParam.b_DAVSMixImgBand = false;
                    FinalInspectParam.DAVSBand1 = enuBand.R;
                    FinalInspectParam.DAVSBand2 = enuBand.G;
                    FinalInspectParam.DAVSBand3 = enuBand.B;
                    FinalInspectParam.DAVSBand1ImgIndex = 0;
                    FinalInspectParam.DAVSBand2ImgIndex = 0;
                    FinalInspectParam.DAVSBand3ImgIndex = 0;
                }

                try // (20200429) Jeff Revised!
                {
                    FinalInspectParam.Directory_SaveDefectResult = finalInspect.GetAttribute("Directory_SaveDefectResult");
                    FinalInspectParam.b_Auto_AB = bool.Parse(finalInspect.GetAttribute("b_Auto_AB"));
                    FinalInspectParam.A_Recipe = finalInspect.GetAttribute("A_Recipe");
                    FinalInspectParam.B_UpDownInvert_FS = bool.Parse(finalInspect.GetAttribute("B_UpDownInvert_FS"));
                    FinalInspectParam.B_LeftRightInvert_FS = bool.Parse(finalInspect.GetAttribute("B_LeftRightInvert_FS"));
                    FinalInspectParam.Directory_defComb = finalInspect.GetAttribute("Directory_defComb");
                    FinalInspectParam.OutputDirectory_defComb = finalInspect.GetAttribute("OutputDirectory_defComb");
                }
                catch
                {
                    FinalInspectParam.Directory_SaveDefectResult = "D:\\DSI\\AOI_Result";
                    FinalInspectParam.b_Auto_AB = false;
                    FinalInspectParam.A_Recipe = "";
                    FinalInspectParam.B_UpDownInvert_FS = false;
                    FinalInspectParam.B_LeftRightInvert_FS = false;
                    FinalInspectParam.Directory_defComb = "D:\\DSI\\DefectCombined_Recipe\\uPOL";
                    FinalInspectParam.OutputDirectory_defComb = "D:\\DSI\\PDF";
                }
                
                XmlNode pos = doc.SelectSingleNode("Side/Insepction/Position");
                XmlElement pos_content = (XmlElement)pos;
                op.PosAddInfo = pos_content.GetAttribute("Info");

                XmlNode laser_pos = doc.SelectSingleNode("Side/Insepction/LaserPosition");
                XmlElement laser_pos_content = (XmlElement)laser_pos;
                try
                {
                    op.LaserPosAddInfo = laser_pos_content.GetAttribute("Info");
                }
                catch
                {
                    op.LaserPosAddInfo = "default";
                }

                XmlNodeList main = doc.SelectNodes("Side/Insepction/Defect");

                foreach (XmlNode node in main)
                {
                    XmlAttributeCollection attributes = node.Attributes;

                    string method_name = "";
                    string name = "";
                    foreach (XmlAttribute item in attributes)
                    {
                        if (item.Name == "name")
                            name = item.Value;
                        if (item.Name == "method")
                           method_name = item.Value;
                    }
                    InspectRole role2 = (InspectRole)((InspectRole)roles_table_[method_name]).Clone();
                    role2.InspectName = name;
                    role2.Method = method_name;
                    role2.load();

                    op.Roles.Add(role2);
                }
                op_collection_.Add(op);
            }
            return true;
        }

        public string PositionInfo
        {
            get { return op_collection_[0].PosAddInfo; }
            set { this.op_collection_[0].PosAddInfo = value; }
        }

        public string LaserPositionInfo
        {
            get { return op_collection_[0].LaserPosAddInfo; }
            set { this.op_collection_[0].LaserPosAddInfo = value; }
        }

        /// <summary>
        /// 取得該位置影像之總瑕疵
        /// </summary>
        /// <returns></returns>
        public List<Defect> get_TotalResult()
        {
            return TotalResult_collection_;
        }

        /// <summary>
        /// 取得該位置影像之各類型瑕疵結果
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, List<Defect>> get_dic_result()
        {
            return dic_result_collection_;
        }

        /// <summary>
        /// 取得該位置影像之各類型瑕疵結果
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, HObject> get_defect_with_name_New() // (20190716) Jeff Revised!
        {
            Dictionary<string, HObject> defect_with_name = new Dictionary<string, HObject>();

            foreach (string key in dic_result_collection_.Keys)
            {
                if (dic_result_collection_[key].Count > 0)
                {
                    foreach (Defect _defectList in dic_result_collection_[key])
                    {
                        if (_defectList.B_defect) // 是否為一種瑕疵，或是僅做為顯示用途 (20190716) Jeff Revised!
                            defect_with_name.Add(_defectList.DefectName, _defectList.DefectCellCenterRegion);
                    }
                }

            }

            return defect_with_name;
        }

        public Dictionary<string, HObject> get_TotalDefect_with_name()
        {
            Dictionary<string, HObject> TotalDefect_with_name = new Dictionary<string, HObject>();

            foreach (Defect dft in TotalResult_collection_)
            {
                TotalDefect_with_name.Add(dft.DefectName, dft.DefectRegion);
            }

            return TotalDefect_with_name;
        }

        /// <summary>
        /// 181203, andy
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, HObject> get_CellRegion_with_name()
        {
            Dictionary<string, HObject> CellRegion_with_name = new Dictionary<string, HObject>();

            foreach (Defect dft in TotalResult_collection_)
            {
                CellRegion_with_name.Add(dft.DefectName, dft.CellRegion);
            }

            return CellRegion_with_name;

        }

        public HObject get_CellRegion() // (20181207) Jeff Revised!
        {
            return TotalResult_collection_[0].CellRegion;
        }

        public List<string> get_roles_names(PnlSide side)
        {
            return op_collection_.Find( x => x.side_ == side ).get_roles_name();
         }

        public List<InspectOperator> OperatorCollection
        {
            get { return this.op_collection_; }
            set { this.op_collection_ = value; }
        }
    }
}
