using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicBoxWTS.Helpers
{
    class SOStatus
    {

        public SOStatus(string id, string name)
        {
            this.ID = id;
            this.Name = name;
        }

        public string ID
        {
            get;
            set;
        }
        public string Name
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
