using System;
using System.Collections.Generic;
using System.Text;

namespace MusicBoxWTS.Core.Models
{
    // TODO WTS: Remove this class once your pages/features are using your data.
    // This is used by the MySQLDataService.
    // It is the model class we use to display data on pages like Grid, Chart, and Master Detail.
    public class MBSpecialOrderLookup
    {
        public int SOLId { get; set; }

        public int SpecialOrder_SOId { get; set; }

        public int Product_ProdId { get; set; }


        public override string ToString() {
            StringBuilder ret = new StringBuilder();

            ret.Append("SOLId = " + SOLId + Environment.NewLine);
            ret.Append("SpecialOrder_SOId = " + SpecialOrder_SOId + Environment.NewLine);
            ret.Append("Product_ProdId = " + Product_ProdId + Environment.NewLine);

            return ret.ToString();
        
        }
    }
}
