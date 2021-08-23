using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeltaSubstrateInspector.Method
{
    class Method
    {
        private int type_ = 0;
        private string name_ = "";
        public Method(string name)
        {
            name_ = name;
        }

        public int get_method_type()
        {
            return type_;
        }

        public void set_recipe_format()
        {
            // Set up the recipe format
            //    Golden is : value and region
            //    Grid is : value
        }

   
    }
}
