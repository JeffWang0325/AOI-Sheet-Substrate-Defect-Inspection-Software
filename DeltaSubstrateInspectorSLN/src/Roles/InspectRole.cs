using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DeltaSubstrateInspector.src.Roles
{
    public class InspectRole : ICloneable
    {
        // Basic attribute of inspect roles.
        protected string inspect_name_ = "";
        protected string method_ = "";

        protected string target_name_ = ""; // 181105, andy

        // Necessary function for ICloneable object
        virtual public object Clone() { return null; }

        virtual public void load() { }

        virtual public void save() { }

        protected void SetXMLElementToDocument(XmlDocument doc, XmlElement root, string name, string value)
        {
            XmlElement element = doc.CreateElement(name);
            element.InnerText = value;
            root.AppendChild(element);
        }

        protected string GetSingleNodeContext(XmlDocument doc, string node, string defaultValue)
        {
            string value = defaultValue;
            try
            {
                if (doc.SelectSingleNode(node) != null)
                    value = doc.SelectSingleNode(node).InnerText;
            }
            catch { value = defaultValue; }
            return value;
        }

        protected string GetXmlContext(XmlNode node, string defaultValue)
        {
            string value = defaultValue;
            try
            {
                if (node != null)
                    value = node.InnerText;
            }
            catch { value = defaultValue; }
            return value;
        }

        // Object's attribute setting
        public string Method
        {
            get { return this.method_; }
            set { this.method_ = value; }
        }

        public string InspectName
        {
            get { return this.inspect_name_; }
            set { this.inspect_name_ = value; }
        }

        public string TargetName
        {
            get { return this.target_name_; }
            set { this.target_name_ = value; }
        }
    }
}
