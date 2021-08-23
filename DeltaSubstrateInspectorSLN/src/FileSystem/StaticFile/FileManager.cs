using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeltaSubstrateInspector.src.FileSystem.StaticFile
{
    public class FileManager
    {
        protected string type_ = "";
        protected string filename_ = "";
        protected string filemap_ = "";

        virtual public void set_filename(int chip_id) { }

        virtual public void set_filemap() { }

        virtual public void format_setting() { }

        virtual public void save_data(string[] data) { }
    }
}
