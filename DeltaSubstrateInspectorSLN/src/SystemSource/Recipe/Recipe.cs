using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeltaSubstrateInspector.src.SystemSource.Recipe
{
    public class Recipe
    {
        public void Load(string recipe_name)
        {

        }

        public void Save(string recipe_name)
        {

        }

        //private string GetFileFullName(string recipeName)
        //{
        //    string fileFullName;
        //    string fileName = "PropertyReicpe";
        //    string path = string.Format("{0}{1}", Application.StartupPath, FileSystem.RecipeFileDirectory);

        //    if (recipeName != "")
        //        fileFullName = string.Format("{0}{1}\\{2}.xml", path, recipeName, fileName);
        //    else if (FileSystem.SolarRecipeName != "")
        //        fileFullName = string.Format("{0}{1}\\{2}.xml", path, FileSystem.SolarRecipeName, fileName);
        //    else
        //        fileFullName = string.Format("{0}Default\\{1}.xml", path, fileName);
        //    return fileFullName;
        //}

        //private void SetXMLElementToDocument(XmlDocument doc, XmlElement root, string name, string value)
        //{
        //    XmlElement element = doc.CreateElement(name);
        //    element.InnerText = value;
        //    root.AppendChild(element);
        //}

        //private string GetSingleNodeContext(XmlDocument doc, string node, string defaultValue)
        //{
        //    string value = defaultValue;
        //    try
        //    {
        //        if (doc.SelectSingleNode(node) != null)
        //            value = doc.SelectSingleNode(node).InnerText;
        //    }
        //    catch { value = defaultValue; }
        //    return value;
        //}

    }
}
