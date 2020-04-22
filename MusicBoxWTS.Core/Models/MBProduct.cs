using System;
using System.Collections.Generic;
using System.Text;

namespace MusicBoxWTS.Core.Models
{
    // TODO WTS: Remove this class once your pages/features are using your data.
    // This is used by the MySQLDataService.
    // It is the model class we use to display data on pages like Grid, Chart, and Master Detail.
    public class MBProduct
    {
        public int ProdId { get; set; }

        public string ProdArtistName { get; set; }

        public string ProdTitle { get; set; }

        public string ProdDesc { get; set; }

        public string ProdExtId { get; set; }

        public double ProdPrice { get; set; }

        public string ProdVendor { get; set; }

        public string ProdType { get; set; }



        public override string ToString() {
            StringBuilder ret = new StringBuilder();

            ret.Append("ProdId = " + ProdId + Environment.NewLine);
            ret.Append("ProdArtistName = " + ProdArtistName + Environment.NewLine);
            ret.Append("ProdTitle = " + ProdTitle + Environment.NewLine);
            ret.Append("ProdDesc = " + ProdDesc + Environment.NewLine);
            ret.Append("ProdExtId = " + ProdExtId + Environment.NewLine);
            ret.Append("ProdPrice = " + ProdPrice + Environment.NewLine);
            ret.Append("ProdVendor = " + ProdVendor + Environment.NewLine);
            ret.Append("ProdType = " + ProdType);


            return ret.ToString();
        
        }
    }
}
