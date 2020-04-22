using System;
using System.Collections.Generic;
using System.Text;

namespace MusicBoxWTS.Core.Models
{
    public class MBSOMaint
    {
        public int SOId { get; set; }

        public DateTime SODate { get; set; }

        public string SOStatus { get; set; }

        public DateTime SOCloseDate { get; set; }

        public string CustName { get; set; }

        public string CustAddr { get; set; }

        public string CustPhone { get; set; }

        public string CustEmail { get; set; }

        public override string ToString() {
            StringBuilder ret = new StringBuilder();

            ret.Append("SOId = " + SOId + Environment.NewLine);
            ret.Append("SODate = " + SODate.ToString() + Environment.NewLine);
            ret.Append("SOStatus = " + SOStatus + Environment.NewLine);
            ret.Append("SOCloseDate = " + SOCloseDate.ToString() + Environment.NewLine);
            ret.Append("CustName = " + CustName + Environment.NewLine);
            ret.Append("CustAddr = " + CustAddr + Environment.NewLine);
            ret.Append("CustPhone = " + CustPhone + Environment.NewLine);
            ret.Append("CustEmail = " + CustEmail);
            
            return ret.ToString();
        
        }
    }
}
