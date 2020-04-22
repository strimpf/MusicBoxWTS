using System;
using System.Collections.Generic;
using System.Text;

namespace MusicBoxWTS.Core.Models
{
    // TODO WTS: Remove this class once your pages/features are using your data.
    // This is used by the MySQLDataService.
    // It is the model class we use to display data on pages like Grid, Chart, and Master Detail.
    public class MBCustomer
    {
        public int CustId { get; set; }

        public string CustFName { get; set; }

        public string CustMI { get; set; }

        public string CustLName { get; set; }

        public string CustSuffix { get; set; }

        public string CustAddress1 { get; set; }

        public string CustAddress2 { get; set; }

        public string CustCity { get; set; }

        public string CustState { get; set; }

        public string CustZip { get; set; }

        public string CustEmail1 { get; set; }

        public string CustEmail2 { get; set; }

        public string CustPhone1 { get; set; }

        public string CustPhone2 { get; set; }

        public string CustTextNum { get; set; }


        public override string ToString() {
            StringBuilder ret = new StringBuilder();

            ret.Append("CustId = " + CustId + Environment.NewLine);
            ret.Append("CustFname = " + CustFName + Environment.NewLine);
            ret.Append("CustMI = " + CustMI + Environment.NewLine);
            ret.Append("CustLName = " + CustLName + Environment.NewLine);
            ret.Append("CustSuffix = " + CustSuffix + Environment.NewLine);
            ret.Append("CustAddress1 = " + CustAddress1 + Environment.NewLine);
            ret.Append("CustAddress2 = " + CustAddress2 + Environment.NewLine);
            ret.Append("CustCity = " + CustCity + Environment.NewLine);
            ret.Append("CustState = " + CustState + Environment.NewLine);
            ret.Append("CustZip = " + CustZip + Environment.NewLine);
            ret.Append("CustCustEmail1 = " + CustEmail1 + Environment.NewLine);
            ret.Append("CustEmail2 = " + CustEmail2 + Environment.NewLine);
            ret.Append("CustPhone1 = " + CustPhone1 + Environment.NewLine);
            ret.Append("CustPhone2 = " + CustPhone2 + Environment.NewLine);
            ret.Append("CustTextNum = " + CustTextNum);


            return ret.ToString();
        
        }
    }
}
