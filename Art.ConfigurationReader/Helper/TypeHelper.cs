using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Art.ConfigurationReader.Helper
{
    public class TypeHelper
    {
        public static Type FindStringType(string t)
        {
            if (t == "String")
                return Type.GetType("System.String");
            else if (t == "Int")
                return Type.GetType("System.Int32");
            else if (t == "DateTime")
                return Type.GetType("System.DateTime");
            else if (t == "Boolean")
                return Type.GetType("System.Boolean");
            else if (t == "Double")
                return Type.GetType("System.Double");
            else
                return Type.GetType("System.Object");
        }
    }
}
