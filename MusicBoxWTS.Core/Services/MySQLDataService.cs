using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

using MusicBoxWTS.Core.Models;
using System.Transactions;

namespace MusicBoxWTS.Core.Services
{
    // This class holds sample data used by some generated pages to show how they can be used.
    // More information on using and configuring this service can be found at https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/features/sql-server-data-service.md
    // TODO WTS: Change your code to use this instead of the SampleDataService.
    public static class MySQLDataService
    {
        

        private static string GetConnectionString()
        {
            // Attempt to get the connection string from a config file
            // Learn more about specifying the connection string in a config file at https://docs.microsoft.com/dotnet/api/system.configuration.configurationmanager?view=netframework-4.7.2
            var conStr = ConfigurationManager.ConnectionStrings["MySQLConnectionString"]?.ConnectionString;

            if (!string.IsNullOrWhiteSpace(conStr))
            {
                return conStr;
            }
            else
            {
                // If no connection string is specified in a config file, use this as a fallback.
                return @"server = 127.0.0.1; uid = musicBox; pwd = r@yT#all2020;database=musicbox";
            }
        }

        #region Sales

        // This method returns data with the same structure as the SampleDataService but based on the NORTHWIND sample database.
        // Use this as an alternative to the sample data to test using a different datasource without changing any other code.
        // TODO WTS: Remove this when or if it isn't needed.
        public static async Task<IEnumerable<MBSales>> AllSales()
        {

            // This hard-coded SQL statement is included to make this sample simpler.
            // You can use Stored procedure, ORMs, or whatever is appropriate to access data in your app.
            const string getMBSalesQuery = @"
            SELECT sales.SalesId,
                sales.SalesAmt,
                sales.SalesDate,
                sales.SalesNotes,
                sales.User
            FROM musicbox.sales";

            var mbSales = new List<MBSales>();

            try
            {
                using (var conn = new MySqlConnection(GetConnectionString()))
                {
                    await conn.OpenAsync();

                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = getMBSalesQuery;

                            using (var reader = await cmd.ExecuteReaderAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    // Company Data
                                    var salesId = reader.GetInt32(0);
                                    //var mbsales = mbSales.FirstOrDefault(c => c.SalesId == salesId);
                                    var salesAmt = reader.GetDouble(1);
                                    var salesDate = reader.GetDateTime(2);
                                    var salesNotes = reader.GetString(3);
                                    var user = reader.GetString(4);

                                    var mbsales = new MBSales()
                                    {
                                        SalesId = salesId,
                                        SalesAmt = salesAmt,
                                        SalesDate = salesDate,
                                        SalesNotes = salesNotes,
                                        User = user
                                    };
                                    mbSales.Add(mbsales);
                                   

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception eSql)
            {
                // Your code may benefit from more robust error handling or logging.
                // This logging is just a reminder that you should handle exceptions when connecting to remote data.
                System.Diagnostics.Debug.WriteLine($"Exception: {eSql.Message} {eSql.InnerException?.Message}");
                throw eSql;
            }

            return mbSales;
        }

        //public static async Task<IEnumerable<DataPoint>> GetChartDataAsync()
        //{
        //    await Task.CompletedTask;
        //    return AllSales().Select(o => new DataPoint() { Category = o.Company, Value = o.OrderTotal })
        //                          .OrderBy(dp => dp.Category);
        //}

        // TODO WTS: Remove this once your ContentGrid page is displaying real data.
        //public static async Task<IEnumerable<SampleOrder>> GetContentGridDataAsync()
        //{
        //    if (_allOrders == null)
        //    {
        //        _allOrders = AllOrders();
        //    }

        //    await Task.CompletedTask;
        //    return _allOrders;
        //}

        // TODO WTS: Remove this once your grid page is displaying real data.
        public static async Task<IEnumerable<MBSales>> GetGridDataAsync()
        {
            await Task.CompletedTask;
            try
            {
                return await AllSales();
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }

        // TODO WTS: Remove this once your MasterDetail pages are displaying real data.
        //public static async Task<IEnumerable<SampleOrder>> GetMasterDetailDataAsync()
        //{
        //    await Task.CompletedTask;
        //    return AllOrders();
        //}


        /// <summary>
        /// Insert a row into the Sales table
        /// </summary>
        /// <param name="mbs"></param>
        public static async void insertSales(MBSales mbs)
        {
            var conn = new MySqlConnection(GetConnectionString());

            try
            {
                using (conn)
                {
                    await conn.OpenAsync();

                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (var cmd = conn.CreateCommand())
                        {

                            MySqlCommand comm = conn.CreateCommand();
                            comm.CommandText = "INSERT INTO musicbox.sales(SalesDate,SalesAmt,SalesNotes,User) VALUES(?dt,?amt,?notes,?user)";
                            comm.Parameters.Add("?dt", MySqlDbType.DateTime).Value = mbs.SalesDate.ToString("yyyy-MM-dd HH:mm:ss");
                            comm.Parameters.Add("?amt", MySqlDbType.Double).Value = mbs.SalesAmt;
                            comm.Parameters.Add("?notes", MySqlDbType.VarChar).Value = mbs.SalesNotes;
                            comm.Parameters.Add("?user", MySqlDbType.VarChar).Value = mbs.User;
                            comm.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception eSql)
            {
                // Your code may benefit from more robust error handling or logging.
                // This logging is just a reminder that you should handle exceptions when connecting to remote data.
                System.Diagnostics.Debug.WriteLine($"Exception: {eSql.Message} {eSql.InnerException?.Message}");
            }
            finally
            {
                conn.Close();
            }

        }
        public static async void deleteSales(int salesId)
        {
            var conn = new MySqlConnection(GetConnectionString());

            try
            {
                using (conn)
                {
                    await conn.OpenAsync();

                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (var cmd = conn.CreateCommand())
                        {

                            MySqlCommand comm = conn.CreateCommand();
                            comm.CommandText = "DELETE FROM musicbox.sales WHERE SalesId = ?id";
                            comm.Parameters.Add("?id", MySqlDbType.Int32).Value = salesId;
                            comm.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception eSql)
            {
                // Your code may benefit from more robust error handling or logging.
                // This logging is just a reminder that you should handle exceptions when connecting to remote data.
                System.Diagnostics.Debug.WriteLine($"Exception: {eSql.Message} {eSql.InnerException?.Message}");
            }
            finally
            {
                conn.Close();
            }

        }

        #endregion

        #region Customers

        public static async Task<IEnumerable<MBCustomer>> AllCustomers()
        {

            const string getMBCustQuery = @"
            SELECT customer.CustId,
                customer.CustFName,
                customer.CustMI,
                customer.CustLName,
                customer.CustSuffix,
                customer.CustAddress1,
                customer.CustAddress2,
                customer.CustCity,
                customer.CustState,
                customer.CustZip,
                customer.CustEmail1,
                customer.CustEmail2,
                customer.CustPhone1,
                customer.CustPhone2,
                customer.CustTextNum
            FROM musicbox.customer";

            var mbCustomers = new List<MBCustomer>();

            try
            {
                using (var conn = new MySqlConnection(GetConnectionString()))
                {
                    await conn.OpenAsync();

                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = getMBCustQuery;

                            using (var reader = await cmd.ExecuteReaderAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    // Company Data
                                    var custId = reader.GetInt32(0);
                                    //var mbsales = mbSales.FirstOrDefault(c => c.SalesId == salesId);
                                    var custFName = reader.GetString(1);
                                    var custMI = reader.GetString(2);
                                    var custLName = reader.GetString(3);
                                    var custSuffix = reader.GetString(4);
                                    var custAddress1 = reader.GetString(5);
                                    var custAddress2 = reader.GetString(6);
                                    var custCity = reader.GetString(7);
                                    var custState = reader.GetString(8);
                                    var custZip = reader.GetString(9);
                                    var custEmail1 = reader.GetString(10);
                                    var custEmail2 = reader.GetString(11);
                                    var custPhone1 = reader.GetString(12);
                                    var custPhone2 = reader.GetString(13);
                                    var custTextNum = reader.GetString(14);
                                    //var salesAmt = reader.GetDouble(1);
                                    //var salesDate = reader.GetDateTime(2);

                                    var mbcust = new MBCustomer
                                    {
                                        CustId = custId,
                                        CustFName = custFName,
                                        CustMI = custMI,
                                        CustLName = custLName,
                                        CustSuffix = custSuffix,
                                        CustAddress1 = custAddress1,
                                        CustAddress2 = custAddress2,
                                        CustCity = custCity,
                                        CustState = custState,
                                        CustZip = custZip,
                                        CustEmail1 = custEmail1,
                                        CustEmail2 = custEmail2,
                                        CustPhone1 = custPhone1,
                                        CustPhone2 = custPhone2,
                                        CustTextNum = custTextNum
                                    };
                                    mbCustomers.Add(mbcust);


                                }
                            }
                        }
                    }
                }
            }
            catch (Exception eSql)
            {
                // Your code may benefit from more robust error handling or logging.
                // This logging is just a reminder that you should handle exceptions when connecting to remote data.
                System.Diagnostics.Debug.WriteLine($"Exception: {eSql.Message} {eSql.InnerException?.Message}");
            }

            return mbCustomers;
        }

        public static async Task<IEnumerable<MBCustomer>> GetCustGridDataAsync()
        {
            await Task.CompletedTask;
            return await AllCustomers();
        }



        /// <summary>
        /// Insert a row into the Customer table
        /// </summary>
        /// <param name="mbs"></param>
        public static int insertCustomer(MBCustomer mbc)
        {
            //var mbCustomers = new List<MBCustomer>();
            var conn = new MySqlConnection(GetConnectionString());
            int ret = 0;

            try
            {
                using (conn)
                {
                    conn.Open();

                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (var cmd = conn.CreateCommand())
                        {

                            MySqlCommand comm = conn.CreateCommand();
                            comm.CommandText = "INSERT INTO musicbox.customer(CustFName,CustMI,CustLName,CustSuffix,CustAddress1,CustAddress2," +
                                "CustCity,CustState,CustZip,CustEmail1,CustEmail2,CustPhone1,CustPhone2,CustTextNum) VALUES(?fn,?mi,?ln,?sx,?a1,?a2,?cy,?st,?zp,?e1,?e2,?p1,?p2,?tn)";
                            comm.Parameters.Add("?fn", MySqlDbType.VarChar).Value = mbc.CustFName;
                            comm.Parameters.Add("?mi", MySqlDbType.VarChar).Value = mbc.CustMI;
                            comm.Parameters.Add("?ln", MySqlDbType.VarChar).Value = mbc.CustLName;
                            comm.Parameters.Add("?sx", MySqlDbType.VarChar).Value = mbc.CustSuffix;
                            comm.Parameters.Add("?a1", MySqlDbType.VarChar).Value = mbc.CustAddress1;
                            comm.Parameters.Add("?a2", MySqlDbType.VarChar).Value = mbc.CustAddress2;
                            comm.Parameters.Add("?cy", MySqlDbType.VarChar).Value = mbc.CustCity;
                            comm.Parameters.Add("?st", MySqlDbType.VarChar).Value = mbc.CustState;
                            comm.Parameters.Add("?zp", MySqlDbType.VarChar).Value = mbc.CustZip;
                            comm.Parameters.Add("?e1", MySqlDbType.VarChar).Value = mbc.CustEmail1;
                            comm.Parameters.Add("?e2", MySqlDbType.VarChar).Value = mbc.CustEmail2;
                            comm.Parameters.Add("?p1", MySqlDbType.VarChar).Value = mbc.CustPhone1;
                            comm.Parameters.Add("?p2", MySqlDbType.VarChar).Value = mbc.CustPhone2;
                            comm.Parameters.Add("?tn", MySqlDbType.VarChar).Value = mbc.CustTextNum;
                            //comm.Parameters.Add("?dt", MySqlDbType.DateTime).Value = mbs.SalesDate.ToString("yyyy-MM-dd HH:mm:ss");
                            //comm.Parameters.Add("?amt", MySqlDbType.Double).Value = mbs.SalesAmt;
                            //comm.Parameters.Add("?notes", MySqlDbType.VarChar).Value = mbs.SalesNotes;
                            //comm.Parameters.Add("?user", MySqlDbType.VarChar).Value = mbs.User;
                            comm.ExecuteNonQuery();
                            ret = (Int32)comm.LastInsertedId;
                        }
                    }
                }
            }
            catch (Exception eSql)
            {
                // Your code may benefit from more robust error handling or logging.
                // This logging is just a reminder that you should handle exceptions when connecting to remote data.
                System.Diagnostics.Debug.WriteLine($"Exception: {eSql.Message} {eSql.InnerException?.Message}");
                throw new TransactionAbortedException(eSql.Message);
            }
            finally
            {
                conn.Close();
            }
            return ret;
        }
        public static async void deleteCustomer(int custId)
        {
            var conn = new MySqlConnection(GetConnectionString());

            try
            {
                using (conn)
                {
                    await conn.OpenAsync();

                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (var cmd = conn.CreateCommand())
                        {

                            MySqlCommand comm = conn.CreateCommand();
                            comm.CommandText = "DELETE FROM musicbox.customer WHERE CustId = ?id";
                            comm.Parameters.Add("?id", MySqlDbType.Int32).Value = custId;
                            comm.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception eSql)
            {
                // Your code may benefit from more robust error handling or logging.
                // This logging is just a reminder that you should handle exceptions when connecting to remote data.
                System.Diagnostics.Debug.WriteLine($"Exception: {eSql.Message} {eSql.InnerException?.Message}");
            }
            finally
            {
                conn.Close();
            }

        }

        #endregion

        #region Products
        public static async Task<IEnumerable<MBProduct>> AllProducts()
        {

            const string getMBProdQuery = @"
            SELECT product.ProdId,
            product.ProdArtistName,
            product.ProdTitle,
            product.ProdDesc,
            product.ProdExtId,
            product.ProdPrice,
            product.ProdVendor,
            product.ProdType
            FROM musicbox.product";

            var mbProducts = new List<MBProduct>();

            try
            {
                using (var conn = new MySqlConnection(GetConnectionString()))
                {
                    await conn.OpenAsync();

                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = getMBProdQuery;

                            using (var reader = await cmd.ExecuteReaderAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    // Company Data
                                    var prodId = reader.GetInt32(0);
                                    var prodArtistName = reader.GetString(1);
                                    var prodTitle = reader.GetString(2);
                                    var prodDesc = reader.GetString(3);
                                    var prodExtId = reader.GetString(4);
                                    var prodPrice = reader.GetDouble(5);
                                    var prodVendor = reader.GetString(6);
                                    var prodType = reader.GetString(7);

                                    var mbprod = new MBProduct
                                    {
                                        ProdId = prodId,
                                        ProdArtistName = prodArtistName,
                                        ProdTitle = prodTitle,
                                        ProdDesc = prodDesc,
                                        ProdExtId = prodExtId,
                                        ProdPrice = prodPrice,
                                        ProdVendor = prodVendor,
                                        ProdType = prodType
                                    };
                                    mbProducts.Add(mbprod);


                                }
                            }
                        }
                    }
                }
            }
            catch (Exception eSql)
            {
                // Your code may benefit from more robust error handling or logging.
                // This logging is just a reminder that you should handle exceptions when connecting to remote data.
                System.Diagnostics.Debug.WriteLine($"Exception: {eSql.Message} {eSql.InnerException?.Message}");
            }

            return mbProducts;
        }


        public static async Task<IEnumerable<MBProduct>> GetProdGridDataAsync()
        {
            await Task.CompletedTask;
            return await AllProducts();
        }


        /// <summary>
        /// Insert a row into the Sales table
        /// </summary>
        /// <param name="mbs"></param>
        public static int insertProduct(MBProduct mbp) 
        {
            var conn = new MySqlConnection(GetConnectionString());
            int ret = -1;

            try
            {
                using (conn)
                {
                    conn.Open();

                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (var cmd = conn.CreateCommand())
                        {

                            MySqlCommand comm = conn.CreateCommand();
                            comm.CommandText = "INSERT INTO musicbox.product(ProdArtistName,ProdTitle,ProdDesc,ProdExtId,ProdPrice,ProdVendor,ProdType) " +
                                "VALUES(?an,?ti,?de,?ei,?pr,?ve,?ty)";
                            comm.Parameters.Add("?an", MySqlDbType.VarChar).Value = mbp.ProdArtistName;
                            comm.Parameters.Add("?ti", MySqlDbType.VarChar).Value = mbp.ProdTitle;
                            comm.Parameters.Add("?de", MySqlDbType.VarChar).Value = mbp.ProdDesc;
                            comm.Parameters.Add("?ei", MySqlDbType.VarChar).Value = mbp.ProdExtId;
                            comm.Parameters.Add("?pr", MySqlDbType.Double).Value = mbp.ProdPrice;
                            comm.Parameters.Add("?ve", MySqlDbType.VarChar).Value = mbp.ProdVendor;
                            comm.Parameters.Add("?ty", MySqlDbType.VarChar).Value = mbp.ProdType;
                            comm.ExecuteNonQuery();
                            ret = (Int32)comm.LastInsertedId;
                        }
                    }
                }
            }
            catch (Exception eSql)
            {
                // Your code may benefit from more robust error handling or logging.
                // This logging is just a reminder that you should handle exceptions when connecting to remote data.
                System.Diagnostics.Debug.WriteLine($"Exception: {eSql.Message} {eSql.InnerException?.Message}");
                throw new TransactionAbortedException(eSql.Message);
            }
            finally
            {
                conn.Close();
            }
            return ret;
        }
        public static async void deleteProduct(int prodId)
        {
            var conn = new MySqlConnection(GetConnectionString());

            try
            {
                using (conn)
                {
                    await conn.OpenAsync();

                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (var cmd = conn.CreateCommand())
                        {

                            MySqlCommand comm = conn.CreateCommand();
                            comm.CommandText = "DELETE FROM musicbox.product WHERE ProdId = ?id";
                            comm.Parameters.Add("?id", MySqlDbType.Int32).Value = prodId;
                            comm.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception eSql)
            {
                // Your code may benefit from more robust error handling or logging.
                // This logging is just a reminder that you should handle exceptions when connecting to remote data.
                System.Diagnostics.Debug.WriteLine($"Exception: {eSql.Message} {eSql.InnerException?.Message}");
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion

        #region Special Orders
        // This method returns data with the same structure as the SampleDataService but based on the NORTHWIND sample database.
        // Use this as an alternative to the sample data to test using a different datasource without changing any other code.
        // TODO WTS: Remove this when or if it isn't needed.
        public static async Task<IEnumerable<MBSpecialOrder>> AllSpecialOrders()
        {

            // This hard-coded SQL statement is included to make this sample simpler.
            // You can use Stored procedure, ORMs, or whatever is appropriate to access data in your app.
            const string getMBSpecialOrderQuery = @"
            SELECT specialorder.SOId,
                specialorder.SODate,
                specialorder.SOStatus,
                specialorder.SOCloseDate,
                specialorder.Customer_CustId
            FROM musicbox.specialorder";

            var mbSpecialOrder = new List<MBSpecialOrder>();

            try
            {
                using (var conn = new MySqlConnection(GetConnectionString()))
                {
                    await conn.OpenAsync();

                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = getMBSpecialOrderQuery;

                            using (var reader = await cmd.ExecuteReaderAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    var soId = reader.GetInt32(0);
                                    var soDate = reader.GetDateTime(1);
                                    var soStatus = reader.GetString(2);
                                    var soCloseDate = reader.GetDateTime(3);
                                    var customer_CustId = reader.GetInt32(4);

                                    var mbspecialorder = new MBSpecialOrder()
                                    {
                                        SOId = soId,
                                        SODate = soDate,
                                        SOStatus = soStatus,
                                        SOCloseDate = soCloseDate,
                                        Customer_CustId = customer_CustId
                                    };
                                    mbSpecialOrder.Add(mbspecialorder);


                                }
                            }
                        }
                    }
                }
            }
            catch (Exception eSql)
            {
                // Your code may benefit from more robust error handling or logging.
                // This logging is just a reminder that you should handle exceptions when connecting to remote data.
                System.Diagnostics.Debug.WriteLine($"Exception: {eSql.Message} {eSql.InnerException?.Message}");
            }

            return mbSpecialOrder;
        }

        //public static async Task<IEnumerable<DataPoint>> GetChartDataAsync()
        //{
        //    await Task.CompletedTask;
        //    return AllSales().Select(o => new DataPoint() { Category = o.Company, Value = o.OrderTotal })
        //                          .OrderBy(dp => dp.Category);
        //}

        // TODO WTS: Remove this once your ContentGrid page is displaying real data.
        //public static async Task<IEnumerable<SampleOrder>> GetContentGridDataAsync()
        //{
        //    if (_allOrders == null)
        //    {
        //        _allOrders = AllOrders();
        //    }

        //    await Task.CompletedTask;
        //    return _allOrders;
        //}

        // TODO WTS: Remove this once your grid page is displaying real data.
        public static async Task<IEnumerable<MBSpecialOrder>> GetSOGridDataAsync()
        {
            await Task.CompletedTask;
            return await AllSpecialOrders();
        }

        // TODO WTS: Remove this once your MasterDetail pages are displaying real data.
        //public static async Task<IEnumerable<SampleOrder>> GetMasterDetailDataAsync()
        //{
        //    await Task.CompletedTask;
        //    return AllOrders();
        //}


        /// <summary>
        /// Insert a row into the Sales table
        /// </summary>
        /// <param name="mbs"></param>
        public static int insertSpecialOrder(MBSpecialOrder mbso)
        {
            var conn = new MySqlConnection(GetConnectionString());
            int ret = -1;

            try
            {
                using (conn)
                {
                    conn.Open();

                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (var cmd = conn.CreateCommand())
                        {

                            MySqlCommand comm = conn.CreateCommand();
                            comm.CommandText = "INSERT INTO musicbox.specialorder(SODate,SOStatus,SOCloseDate,Customer_CustId) VALUES(?dt,?st,?cd,?cc)";
                            comm.Parameters.Add("?dt", MySqlDbType.DateTime).Value = mbso.SODate.ToString("yyyy-MM-dd HH:mm:ss");
                            comm.Parameters.Add("?st", MySqlDbType.VarChar).Value = mbso.SOStatus;
                            comm.Parameters.Add("?cd", MySqlDbType.DateTime).Value = mbso.SOCloseDate.ToString("yyyy-MM-dd HH:mm:ss");
                            comm.Parameters.Add("?cc", MySqlDbType.Int32).Value = mbso.Customer_CustId;
                            comm.ExecuteNonQuery();
                            ret = (Int32)comm.LastInsertedId;
                        }
                    }
                }
            }
            catch (Exception eSql)
            {
                // Your code may benefit from more robust error handling or logging.
                // This logging is just a reminder that you should handle exceptions when connecting to remote data.
                System.Diagnostics.Debug.WriteLine($"Exception: {eSql.Message} {eSql.InnerException?.Message}");
                throw new TransactionAbortedException(eSql.Message);
            }
            finally
            {
                conn.Close();
            }
            return ret;
        }
        public static async void deleteSpecialOrder(int soId)
        {
            var conn = new MySqlConnection(GetConnectionString());

            try
            {
                using (conn)
                {
                    await conn.OpenAsync();

                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (var cmd = conn.CreateCommand())
                        {

                            MySqlCommand comm = conn.CreateCommand();
                            comm.CommandText = "DELETE FROM musicbox.specialorder WHERE SOId = ?id";
                            comm.Parameters.Add("?id", MySqlDbType.Int32).Value = soId;
                            comm.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception eSql)
            {
                // Your code may benefit from more robust error handling or logging.
                // This logging is just a reminder that you should handle exceptions when connecting to remote data.
                System.Diagnostics.Debug.WriteLine($"Exception: {eSql.Message} {eSql.InnerException?.Message}");
            }
            finally
            {
                conn.Close();
            }

        }

        #endregion

        #region Special Order Lookup
        // This method returns data with the same structure as the SampleDataService but based on the NORTHWIND sample database.
        // Use this as an alternative to the sample data to test using a different datasource without changing any other code.
        // TODO WTS: Remove this when or if it isn't needed.
        public static async Task<IEnumerable<MBSpecialOrderLookup>> AllSOL()
        {

            // This hard-coded SQL statement is included to make this sample simpler.
            // You can use Stored procedure, ORMs, or whatever is appropriate to access data in your app.
            const string getMBSOLQuery = @"
            SELECT specialorderlookup.SOLId,
                specialorderlookup.SpecialOrder_SOId,
                specialorderlookup.Product_ProdId
            FROM musicbox.specialorderlookup";

            var mbSOL = new List<MBSpecialOrderLookup>();

            try
            {
                using (var conn = new MySqlConnection(GetConnectionString()))
                {
                    await conn.OpenAsync();

                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = getMBSOLQuery;

                            using (var reader = await cmd.ExecuteReaderAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    // Company Data
                                    var solId = reader.GetInt32(0);
                                    var specialOrder_SOId = reader.GetInt32(1);
                                    var product_ProdId = reader.GetInt32(2);

                                    var mbsol = new MBSpecialOrderLookup()
                                    {
                                        SOLId = solId,
                                        SpecialOrder_SOId = specialOrder_SOId,
                                        Product_ProdId = product_ProdId
                                    };
                                    mbSOL.Add(mbsol);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception eSql)
            {
                // Your code may benefit from more robust error handling or logging.
                // This logging is just a reminder that you should handle exceptions when connecting to remote data.
                System.Diagnostics.Debug.WriteLine($"Exception: {eSql.Message} {eSql.InnerException?.Message}");
            }

            return mbSOL;
        }

        //public static async Task<IEnumerable<DataPoint>> GetChartDataAsync()
        //{
        //    await Task.CompletedTask;
        //    return AllSales().Select(o => new DataPoint() { Category = o.Company, Value = o.OrderTotal })
        //                          .OrderBy(dp => dp.Category);
        //}

        // TODO WTS: Remove this once your ContentGrid page is displaying real data.
        //public static async Task<IEnumerable<SampleOrder>> GetContentGridDataAsync()
        //{
        //    if (_allOrders == null)
        //    {
        //        _allOrders = AllOrders();
        //    }

        //    await Task.CompletedTask;
        //    return _allOrders;
        //}

        // TODO WTS: Remove this once your grid page is displaying real data.
        public static async Task<IEnumerable<MBSpecialOrderLookup>> GetSOLGridDataAsync()
        {
            await Task.CompletedTask;
            return await AllSOL();
        }

        // TODO WTS: Remove this once your MasterDetail pages are displaying real data.
        //public static async Task<IEnumerable<SampleOrder>> GetMasterDetailDataAsync()
        //{
        //    await Task.CompletedTask;
        //    return AllOrders();
        //}


        /// <summary>
        /// Insert a row into the SpecialOrderLookup table
        /// </summary>
        /// <param name="mbs"></param>
        public static int insertSOL(MBSpecialOrderLookup mbsol)
        {
            var conn = new MySqlConnection(GetConnectionString());
            int ret = -1;

            try
            {
                using (conn)
                {
                    conn.Open();

                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (var cmd = conn.CreateCommand())
                        {

                            MySqlCommand comm = conn.CreateCommand();
                            comm.CommandText = "INSERT INTO musicbox.specordprodlookup(SpecialOrder_SOId,Product_ProdId) VALUES(?so,?pr)";
                            comm.Parameters.Add("?so", MySqlDbType.Int32).Value = mbsol.SpecialOrder_SOId;
                            comm.Parameters.Add("?pr", MySqlDbType.Int32).Value = mbsol.Product_ProdId;

                            comm.ExecuteNonQuery();
                            ret = (Int32)comm.LastInsertedId;
                        }
                    }
                }
            }
            catch (Exception eSql)
            {
                // Your code may benefit from more robust error handling or logging.
                // This logging is just a reminder that you should handle exceptions when connecting to remote data.
                System.Diagnostics.Debug.WriteLine($"Exception: {eSql.Message} {eSql.InnerException?.Message}");
                throw new TransactionAbortedException(eSql.Message);
            }
            finally
            {
                conn.Close();
            }
            return ret;
        }

        public static async void deleteSOL(int solId)
        {
            var conn = new MySqlConnection(GetConnectionString());

            try
            {
                using (conn)
                {
                    await conn.OpenAsync();

                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (var cmd = conn.CreateCommand())
                        {

                            MySqlCommand comm = conn.CreateCommand();
                            comm.CommandText = "DELETE FROM musicbox.specialorderlookup WHERE SOLId = ?id";
                            comm.Parameters.Add("?id", MySqlDbType.Int32).Value = solId;
                            comm.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception eSql)
            {
                // Your code may benefit from more robust error handling or logging.
                // This logging is just a reminder that you should handle exceptions when connecting to remote data.
                System.Diagnostics.Debug.WriteLine($"Exception: {eSql.Message} {eSql.InnerException?.Message}");
            }
            finally
            {
                conn.Close();
            }

        }

        #endregion

        #region SO Maintenance

        public static async Task<IEnumerable<MBSOMaint>> AllSOMaint(string status)
        {

            string getMBSpecialOrderQuery = "SELECT so.SOId, " +
                "so.SODate, " +
                "so.SOStatus, " +
                "so.SOCloseDate, " +
                "concat(cust.CustFName,' ', cust.CustMI,' ', cust.CustLName,' ',cust.CustSuffix) custName, " +
                "concat(cust.CustAddress1,', ',cust.CustAddress2,', ',cust.CustCity,', ',cust.CustState,' ',cust.CustZip) custAddress, " +
                "concat(cust.CustEmail1,'| ',cust.CustEmail2) custEmail, " +
                "concat(cust.CustPhone1,'| ',cust.CustPhone2) custPhone " +
            "FROM musicbox.specialorder so  " +
            "LEFT JOIN `musicbox`.`customer` cust ON so.`Customer_CustId` = cust.`CustId` " +
            "WHERE so.SOStatus = ?st";

            var mbSOMaint = new List<MBSOMaint>();

            try
            {
                using (var conn = new MySqlConnection(GetConnectionString()))
                {
                    await conn.OpenAsync();

                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (var comm = conn.CreateCommand())
                        {
//                            cmd.CommandText = getMBSpecialOrderQuery;

//                            comm = conn.CreateCommand();
                            comm.CommandText = getMBSpecialOrderQuery;
                            comm.Parameters.Add("?st", MySqlDbType.VarChar).Value = status;

                            using (var reader = await comm.ExecuteReaderAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    var soId = -1;
                                    if (!reader.IsDBNull(0))
                                        soId = reader.GetInt32(0);

                                    var soDate = new DateTime(1900, 01, 01);
                                    if (!reader.IsDBNull(1))
                                        soDate = reader.GetDateTime(1);

                                    var soStatus = "";
                                    if (!reader.IsDBNull(2))
                                        soStatus = reader.GetString(2);
                                    
                                    var soCloseDate = new DateTime(1900, 01, 01); 
                                    if (!reader.IsDBNull(3))
                                        soCloseDate = reader.GetDateTime(3);

                                    var custName = "";
                                    if (!reader.IsDBNull(4))
                                        custName = reader.GetString(4);

                                    var custAddress = "";
                                    if (!reader.IsDBNull(5))
                                        custAddress = reader.GetString(5);

                                    var custEmail = "";
                                    if (!reader.IsDBNull(6))
                                        custEmail = reader.GetString(6);

                                    var custPhone = "";
                                    if (!reader.IsDBNull(7))
                                        custPhone = reader.GetString(7);

                                    var mbsomaint = new MBSOMaint()
                                    {
                                        SOId = soId,
                                        SODate = soDate,
                                        SOStatus = soStatus,
                                        SOCloseDate = soCloseDate,
                                        CustName = custName,
                                        CustAddr = custAddress,
                                        CustPhone = custPhone,
                                        CustEmail = custEmail
                                    };
                                    mbSOMaint.Add(mbsomaint);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception eSql)
            {
                System.Diagnostics.Debug.WriteLine($"Exception: {eSql.Message} {eSql.InnerException?.Message}");
            }

            return mbSOMaint;
        }

        public static async Task<IEnumerable<MBSOMaint>> GetSOMaintGridDataAsync(string status)
        {
            await Task.CompletedTask;
            return await AllSOMaint(status);
        }
        

        public static void updateSOStatus(string status, int pk)
        {
            var conn = new MySqlConnection(GetConnectionString());

            try
            {
                using (conn)
                {
                    conn.Open();

                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            MySqlCommand comm = conn.CreateCommand();
                            comm.CommandText = "UPDATE musicbox.specialorder SET SOStatus = ?st WHERE SOId = ?pk";
                            comm.Parameters.Add("?st", MySqlDbType.VarChar).Value = status;
                            comm.Parameters.Add("?pk", MySqlDbType.Int32).Value = pk;
                            comm.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception eSql)
            {
                // Your code may benefit from more robust error handling or logging.
                // This logging is just a reminder that you should handle exceptions when connecting to remote data.
                System.Diagnostics.Debug.WriteLine($"Exception: {eSql.Message} {eSql.InnerException?.Message}");
                //throw new TransactionAbortedException(eSql.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        public static async Task<IEnumerable<MBProduct>> AllSOMaintProd(int soid)
        {

            string getMBSOMaintProdQuery = "SELECT product.ProdId, " +
            "product.ProdArtistName, " +
            "product.ProdTitle, " +
            "product.ProdDesc, " +
            "product.ProdExtId, " +
            "product.ProdPrice, " +
            "product.ProdVendor, " +
            "product.ProdType " +
            "FROM musicbox.product " +
            "JOIN musicbox.specordprodlookup ON specordprodlookup.Product_ProdId = product.ProdId " +
            "WHERE specordprodlookup.SpecialOrder_SOId = ?so";
           
            var mbSOMProd = new List<MBProduct>();

            try
            {
                using (var conn = new MySqlConnection(GetConnectionString()))
                {
                    await conn.OpenAsync();

                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (var comm = conn.CreateCommand())
                        {
                            comm.CommandText = getMBSOMaintProdQuery;
                            comm.Parameters.Add("?so", MySqlDbType.Int32).Value = soid;

                            using (var reader = await comm.ExecuteReaderAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    var prodId = -1;
                                    if (!reader.IsDBNull(0))
                                        prodId = reader.GetInt32(0);
                                    
                                    var prodArtistName = "";
                                    if (!reader.IsDBNull(1))
                                        prodArtistName = reader.GetString(1);
                                    
                                    var prodTitle = "";
                                    if (!reader.IsDBNull(2))
                                        prodTitle = reader.GetString(2);

                                    var prodDesc = "";
                                    if (!reader.IsDBNull(3))
                                        prodDesc = reader.GetString(3);

                                    var prodExtId = "";
                                    if (!reader.IsDBNull(4))
                                        prodExtId = reader.GetString(4);

                                    var prodPrice = 0.00;
                                    if (!reader.IsDBNull(5))
                                        prodPrice = reader.GetDouble(5);

                                    var prodVendor = "";
                                    if (!reader.IsDBNull(6))
                                        prodVendor = reader.GetString(6);

                                    var prodType = "";
                                    if (!reader.IsDBNull(7))
                                        prodType = reader.GetString(7);

                                    var mbsomprod = new MBProduct()
                                    {
                                        ProdId = prodId,
                                        ProdArtistName = prodArtistName,
                                        ProdTitle = prodTitle,
                                        ProdDesc = prodDesc,
                                        ProdExtId = prodExtId,
                                        ProdPrice = prodPrice,
                                        ProdVendor = prodVendor,
                                        ProdType = prodType
                                    };
                                    mbSOMProd.Add(mbsomprod);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception eSql)
            {
                System.Diagnostics.Debug.WriteLine($"Exception: {eSql.Message} {eSql.InnerException?.Message}");
            }

            return mbSOMProd;
        }

        public static async Task<IEnumerable<MBProduct>> GetSOMaintProdGridDataAsync(int soid)
        {
            await Task.CompletedTask;
            return await AllSOMaintProd(soid);
        }

    }

    #endregion

}
