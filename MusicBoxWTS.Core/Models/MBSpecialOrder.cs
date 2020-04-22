using System;
using System.Collections.Generic;
using System.Text;

namespace MusicBoxWTS.Core.Models
{
    // TODO WTS: Remove this class once your pages/features are using your data.
    // This is used by the MySQLDataService.
    // It is the model class we use to display data on pages like Grid, Chart, and Master Detail.
    public class MBSpecialOrder
    {
        public int SOId { get; set; }

        public DateTime SODate { get; set; }

        public string SOStatus { get; set; }

        public DateTime SOCloseDate { get; set; }

        public int Customer_CustId { get; set; }

        public override string ToString() {
            StringBuilder ret = new StringBuilder();

            ret.Append("SOId = " + SOId + Environment.NewLine);
            ret.Append("SODate = " + SODate.ToString() + Environment.NewLine);
            ret.Append("SOStatus = " + SOStatus + Environment.NewLine);
            ret.Append("SOCloseDate = " + SOCloseDate.ToString() + Environment.NewLine);
            ret.Append("Customer_CustId = " + Customer_CustId);


            return ret.ToString();
        
        }
    }
}
