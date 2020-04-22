using System;
using System.Collections.Generic;
using System.Text;

namespace MusicBoxWTS.Core.Models
{
    // TODO WTS: Remove this class once your pages/features are using your data.
    // This is used by the MySQLDataService.
    // It is the model class we use to display data on pages like Grid, Chart, and Master Detail.
    public class MBSales
    {
        public int SalesId { get; set; }

        public double SalesAmt { get; set; }

        public DateTime SalesDate { get; set; }

        public string SalesNotes { get; set; }

        public string User { get; set; }

        public MBSales()
        {

        }
        
        public MBSales(int salesId, double salesAmt, DateTime salesDate, string salesNotes, string user)
        {
            SalesId = salesId;
            SalesAmt = salesAmt;
            SalesDate = salesDate;
            SalesNotes = salesNotes;
            User = user;
        }

        public override string ToString() {
            StringBuilder ret = new StringBuilder();

            ret.Append("SalesId = " + SalesId + Environment.NewLine);
            ret.Append("SalesAmt = " + SalesAmt + Environment.NewLine);
            ret.Append("SalesDate = " + SalesDate.ToString() + Environment.NewLine);
            ret.Append("SalesNotes = " + SalesNotes + Environment.NewLine);
            ret.Append("User = " + User);


            return ret.ToString();
        
        }

        public class SortByIdAsc : IComparer<MBSales>
        {
            public int Compare(MBSales mbs1, MBSales mbs2)
            {
                int ret = -1;
                if (mbs1.SalesId == mbs2.SalesId)
                    ret = 0;
                else if (mbs1.SalesId > mbs2.SalesId)
                    ret = 1;

                return ret;
            }
        }
        public class SortByIdDesc : IComparer<MBSales>
        {
            public int Compare(MBSales mbs1, MBSales mbs2)
            {
                SortByIdAsc mbsia = new SortByIdAsc();
                return -1 * mbsia.Compare(mbs1, mbs2);
            }
        }

        public class SortBySalesDateAsc : IComparer<MBSales>
        {
            public int Compare(MBSales mbs1, MBSales mbs2)
            {
                int ret = -1;
                if (mbs1.SalesDate == mbs2.SalesDate)
                    ret = 0;
                else if (mbs1.SalesDate > mbs2.SalesDate)
                    ret = 1;

                return ret;
            }
        }
        public class SortBySalesDateDesc : IComparer<MBSales>
        {
            public int Compare(MBSales mbs1, MBSales mbs2)
            {
                SortBySalesDateAsc mbssda = new SortBySalesDateAsc();
                return -1 * mbssda.Compare(mbs1, mbs2);
            }
        }

        public class SortBySalesAmtAsc : IComparer<MBSales>
        {
            public int Compare(MBSales mbs1, MBSales mbs2)
            {
                int ret = -1;
                if (mbs1.SalesAmt == mbs2.SalesAmt)
                    ret = 0;
                else if (mbs1.SalesAmt > mbs2.SalesAmt)
                    ret = 1;

                return ret;
            }
        }
        public class SortBySalesAmtDesc : IComparer<MBSales>
        {
            public int Compare(MBSales mbs1, MBSales mbs2)
            {
                SortBySalesAmtAsc mbssaa = new SortBySalesAmtAsc();
                return -1 * mbssaa.Compare(mbs1, mbs2);
            }
        }
        public class SortBySalesNotesAsc : IComparer<MBSales>
        {
            public int Compare(MBSales mbs1, MBSales mbs2)
            {
                int ret = -1;
                if (string.Compare(mbs1.SalesNotes,mbs2.SalesNotes) == 0)
                    ret = 0;
                else if (string.Compare(mbs1.SalesNotes, mbs2.SalesNotes) > 0)
                    ret = 1;

                return ret;
            }
        }
        public class SortBySalesNotesDesc : IComparer<MBSales>
        {
            public int Compare(MBSales mbs1, MBSales mbs2)
            {
                SortBySalesNotesAsc mbssna = new SortBySalesNotesAsc();
                return -1 * mbssna.Compare(mbs1, mbs2);
            }
        }
        public class SortByUserAsc : IComparer<MBSales>
        {
            public int Compare(MBSales mbs1, MBSales mbs2)
            {
                int ret = -1;
                if (string.Compare(mbs1.User, mbs2.User) == 0)
                    ret = 0;
                else if (string.Compare(mbs1.User, mbs2.User) > 0)
                    ret = 1;

                return ret;
            }
        }
        public class SortByUserDesc : IComparer<MBSales>
        {
            public int Compare(MBSales mbs1, MBSales mbs2)
            {
                SortByUserAsc mbsua = new SortByUserAsc();
                return -1 * mbsua.Compare(mbs1, mbs2);
            }
        }


    }
}
