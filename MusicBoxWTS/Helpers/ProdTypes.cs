using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicBoxWTS.Helpers
{
    class ProdTypes
    {

        public ProdTypes(string name, string desc)
        {
            this.Name = name;
            this.Desc = desc;
        }

        public string Name
        {
            get;
            set;
        }
        public string Desc
        {
            get;
            set;
        }
        public override string ToString()
        {
            return this.Name;
        }
    }
}
