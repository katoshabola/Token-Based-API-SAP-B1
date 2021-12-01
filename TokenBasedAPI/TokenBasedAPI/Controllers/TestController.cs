using System.Linq;
using System.Web.Http;
using System.Security.Claims;
using System.Net.Http;
using System.Net;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using System.Text;
using System;
using System.Collections.Generic;
/// <summary>
namespace TokenBasedAPI.Controllers
{
    public class TestController : ApiController
    {
        [Authorize(Roles = "SuperAdmin, Admin, User")]
        //[HttpPost]
        [HttpGet]
        [Route("api/SAP/GetCurrencies")]
        public HttpResponseMessage Post()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var roles = identity.Claims
                        .Where(c => c.Type == ClaimTypes.Role)
                        .Select(c => c.Value);
                        string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("select * FROM  OCRN "))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            dt.TableName = "Customers";
                            sda.Fill(dt);
                            //return dt;
                            string customers = DataTableToJSONWithStringBuilder(dt);
                            //Context.Response.Clear();
                            //Context.Response.ContentType = "application/json";
                            //var json = JsonConvert.SerializeObject(customers, Newtonsoft.Json.Formatting.Indented);
                            //Context.Response.Write(customers);
                            //// return message;
                            //Context.Response.End();
                            return Request.CreateResponse(HttpStatusCode.Created, customers);
                        }
                    }
                }
            }           
        }

        [Authorize(Roles = "SuperAdmin, Admin, User")]
        //[HttpPost]
        [HttpGet]
        [Route("api/SAP/GetAccounts")]
        public HttpResponseMessage Post2()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var roles = identity.Claims
                        .Where(c => c.Type == ClaimTypes.Role)
                        .Select(c => c.Value);
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("select  top 5 T0.AcctCode, T0.AcctName from oact T0"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            dt.TableName = "Customers";
                            sda.Fill(dt);
                            //return dt;
                            string customers = DataTableToJSONWithStringBuilder(dt);
                            //Context.Response.Clear();
                            //Context.Response.ContentType = "application/json";
                            //var json = JsonConvert.SerializeObject(customers, Newtonsoft.Json.Formatting.Indented);
                            //Context.Response.Write(customers);
                            //// return message;
                            //Context.Response.End();
                            return Request.CreateResponse(HttpStatusCode.Created, customers);
                        }
                    }
                }
            }
        }

        [Authorize(Roles = "SuperAdmin, Admin, User")]
        //[HttpPost]
        [HttpGet]
        [Route("api/SAP/GetTaxes")]
        public HttpResponseMessage Post3()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var roles = identity.Claims
                        .Where(c => c.Type == ClaimTypes.Role)
                        .Select(c => c.Value);
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("select t0.Code, t0.Name, t0.Inactive,t0.Rate from OVTG t0 where  t0.Inactive='Y'"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            dt.TableName = "Customers";
                            sda.Fill(dt);
                            //return dt;
                            string customers = DataTableToJSONWithStringBuilder(dt);
                            //Context.Response.Clear();
                            //Context.Response.ContentType = "application/json";
                            //var json = JsonConvert.SerializeObject(customers, Newtonsoft.Json.Formatting.Indented);
                            //Context.Response.Write(customers);
                            //// return message;
                            //Context.Response.End();
                            return Request.CreateResponse(HttpStatusCode.Created, customers);
                        }
                    }
                }
            }
        }
        public class Invoice_Rows
        {
            public string ItemCode { get; set; }
            public string Dscription { get; set; }
            public double Quantity { get; set; }
            public string WhsCode { get; set; }
            public string LineTotal { get; set; }
            public string PriceAfVAT { get; set; }
            public string Price { get; set; }
            public string VatGroup { get; set; }
            public string VatSum { get; set; }
            public string DocEntry { get; set; }
        }

        public class Invoice_Header
        {
            public string CardCode { get; set; }
            public string CardName { get; set; }
            public string DocDate { get; set; }
            public string DocType { get; set; }
            public string CANCELED { get; set; }
            public string DocStatus { get; set; }
            public string DocTotal { get; set; }

            public string VatSum { get; set; }

            public string DocNum { get; set; }
            public string DocEntry { get; set; }
            public List<Invoice_Rows> Invoice_Rows { get; set; }
        }

        [Authorize(Roles = "SuperAdmin, Admin, User")]
        //[HttpPost]
        [HttpGet]
        [Route("api/SAP/GetInvoices")]
        public HttpResponseMessage Post4()


        {
            List<Invoice_Header> invoices = new List<Invoice_Header>();


            DataTable dt = GetData("select top 10 t0.DocEntry,t0.DocNum,t0.CardCode, t0.CardName, t0.DocDate, t0.DocType, t0.CANCELED, t0.DocStatus, t0.DocTotal, t0.VatSum from OINV t0 ");
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                Invoice_Header invoice = new Invoice_Header
                {


                    DocEntry = Convert.ToString(dt.Rows[i]["DocEntry"])
                    ,
                    DocNum = Convert.ToString(dt.Rows[i]["DocNum"])
                    ,

                    CardCode = Convert.ToString(dt.Rows[i]["CardCode"])
                    ,
                    CardName = Convert.ToString(dt.Rows[i]["CardName"])
                    ,
                    DocDate = Convert.ToString(dt.Rows[i]["DocDate"])

                    ,
                    DocType = Convert.ToString(dt.Rows[i]["DocType"])
                    ,
                    CANCELED = Convert.ToString(dt.Rows[i]["CANCELED"])
                    ,
                    DocStatus = Convert.ToString(dt.Rows[i]["DocStatus"])
                    ,
                    DocTotal = Convert.ToString(dt.Rows[i]["DocTotal"])
                    ,
                    VatSum = Convert.ToString(dt.Rows[i]["VatSum"])
                    ,
                    Invoice_Rows = GetInvoiceRows(Convert.ToString(dt.Rows[i]["DocEntry"]))
                };
                invoices.Add(invoice);
            }
            // var json = new JavaScriptSerializer().Serialize(invoices);
            var json = JsonConvert.SerializeObject(invoices, Newtonsoft.Json.Formatting.Indented);
            return Request.CreateResponse(HttpStatusCode.Created, json);
        }
        [Authorize(Roles = "SuperAdmin, Admin, User")]
        //[HttpPost]
        [HttpGet]
        [Route("api/SAP/GetInvoicesById/{DocEntry}")]
        public HttpResponseMessage Post5 (int DocEntry)
        {
         
            List<Invoice_Header> invoices = new List<Invoice_Header>();


            DataTable dt = GetData("select top 1 t0.DocEntry,t0.DocNum,t0.CardCode, t0.CardName, t0.DocDate, t0.DocType, t0.CANCELED, t0.DocStatus, t0.DocTotal, t0.VatSum from OINV t0  where t0.DocEntry ='" + DocEntry + "'");
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                Invoice_Header invoice = new Invoice_Header
                {


                    DocEntry = Convert.ToString(dt.Rows[i]["DocEntry"])
                    ,
                    DocNum = Convert.ToString(dt.Rows[i]["DocNum"])
                    ,

                    CardCode = Convert.ToString(dt.Rows[i]["CardCode"])
                    ,
                    CardName = Convert.ToString(dt.Rows[i]["CardName"])
                    ,
                    DocDate = Convert.ToString(dt.Rows[i]["DocDate"])

                    ,
                    DocType = Convert.ToString(dt.Rows[i]["DocType"])
                    ,
                    CANCELED = Convert.ToString(dt.Rows[i]["CANCELED"])
                    ,
                    DocStatus = Convert.ToString(dt.Rows[i]["DocStatus"])
                    ,
                    DocTotal = Convert.ToString(dt.Rows[i]["DocTotal"])
                    ,
                    VatSum = Convert.ToString(dt.Rows[i]["VatSum"])
                    ,
                    Invoice_Rows = GetInvoiceRows(Convert.ToString(dt.Rows[i]["DocEntry"]))
                };
                invoices.Add(invoice);
            }
            // var json = new JavaScriptSerializer().Serialize(invoices);
            var json = JsonConvert.SerializeObject(invoices, Newtonsoft.Json.Formatting.Indented);
            return Request.CreateResponse(HttpStatusCode.Created, json);
        }
        public List<Invoice_Rows> GetInvoiceRows(string DocEntry)
        {

            //     public string ItemCode { get; set; }
            //public string Dscription { get; set; }
            //public double Quantity { get; set; }
            //public string WhsCode { get; set; }
            //public string LineTotal { get; set; }
            //public string PriceAfVAT { get; set; }

            //public string VatGroup { get; set; }
            //public string VatSum { get; set; }
            List<Invoice_Rows> invoicerows = new List<Invoice_Rows>();
            DataTable dt =null;
            bool is_Numeric = IsNumeric(DocEntry);
            if  (is_Numeric == true) 
            {
                 dt = GetData(string.Format("select  T1.DocEntry ,T1.ItemCode, T1.ItemCode, T1.Dscription, T1.Quantity, T1.WhsCode, T1.LineTotal, T1.PriceAfVAT,T1.Price, T1.VatGroup, T1.VatSum from INV1 T1 INNER JOIN" +
                               "  OINV  T0  ON  T0.DocEntry =T1.DocEntry  Where T0.DocEntry ='{0}'", DocEntry));
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                invoicerows.Add(new Invoice_Rows
                {


                    DocEntry = Convert.ToString(dt.Rows[i]["DocEntry"])
                     ,
                    ItemCode = Convert.ToString(dt.Rows[i]["ItemCode"])
                    ,
                    Dscription = Convert.ToString(dt.Rows[i]["Dscription"])
                    ,
                    Quantity = Convert.ToDouble(dt.Rows[i]["Quantity"])
                    ,
                    WhsCode = Convert.ToString(dt.Rows[i]["WhsCode"])
                    ,
                    LineTotal = Convert.ToString(dt.Rows[i]["LineTotal"])
                    ,
                    PriceAfVAT = Convert.ToString(dt.Rows[i]["PriceAfVAT"])
                      ,
                    Price = Convert.ToString(dt.Rows[i]["Price"])
                      ,
                    VatGroup = Convert.ToString(dt.Rows[i]["VatGroup"])
                    ,
                    VatSum = Convert.ToString(dt.Rows[i]["VatSum"])

                });
            }
            return invoicerows;
        }

        public class Customer_Master
        {
            public string CardCode { get; set; }
            public string CardName { get; set; }
            public double Balance { get; set; }
            public string Currency { get; set; }
            public List<BillToAddress> Bill_To_Address { get; set; }
        }

        public class BillToAddress
        {
            public string CardCode { get; set; }
            public string Name { get; set; }
            public string Title { get; set; }
            public string Position { get; set; }
            public string Address { get; set; }

            public string Tel1 { get; set; }

            public string Cellolar { get; set; }
            public string E_MailL { get; set; }

            public string Active { get; set; }
            // T1.[Name], T1.[Title], T1.[Position], T1.[Address], T1.[Tel1], T1.[Cellolar], T1.[E_MailL], T1.[Active]
        }
        [Authorize(Roles = "SuperAdmin, Admin, User")]
        //[HttpPost]
        [HttpGet]
        [Route("api/SAP/GetCustomers")]
        public HttpResponseMessage Post5()
        {
            List<Customer_Master> customers = new List<Customer_Master>();


            DataTable dt = GetData("select top 10 t0.CardCode, t0.CardName, t0.Balance, t0.Currency from ocrd t0 ");
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                Customer_Master customer = new Customer_Master
                {

                    CardCode = Convert.ToString(dt.Rows[i]["CardCode"])
                    ,
                    CardName = Convert.ToString(dt.Rows[i]["CardName"])
                    ,
                    Balance = Convert.ToDouble(dt.Rows[i]["Balance"])
                    ,
                    Currency = Convert.ToString(dt.Rows[i]["Currency"])
                    ,
                    Bill_To_Address = GetContact(Convert.ToString(dt.Rows[i]["CardCode"]))
                };
                customers.Add(customer);
            }
            //  var json = new JavaScriptSerializer().Serialize(customers);
            var json = JsonConvert.SerializeObject(customers, Newtonsoft.Json.Formatting.Indented);
            return Request.CreateResponse(HttpStatusCode.Created, json);
        }

        [Authorize(Roles = "SuperAdmin, Admin, User")]
        //[HttpPost]
        [HttpGet]
        [Route("api/SAP/GetCustomersByCode/{CardCode}")]
        public HttpResponseMessage Post6(string CardCode)
        {
            List<Customer_Master> customers = new List<Customer_Master>();
            //SqlParameter[] myCardCode = new SqlParameter[1];
            //myCardCode[0] = new SqlParameter("@CardCode", CardCode);

            DataTable dt = GetData("select top 1 t0.CardCode, t0.CardName, t0.Balance, t0.Currency from ocrd t0  where  t0.CardCode='"+ CardCode+"'");
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                Customer_Master customer = new Customer_Master
                {

                    CardCode = Convert.ToString(dt.Rows[i]["CardCode"])
                    ,
                    CardName = Convert.ToString(dt.Rows[i]["CardName"])
                    ,
                    Balance = Convert.ToDouble(dt.Rows[i]["Balance"])
                    ,
                    Currency = Convert.ToString(dt.Rows[i]["Currency"])
                    ,
                    Bill_To_Address = GetContact(Convert.ToString(dt.Rows[i]["CardCode"]))
                };
                customers.Add(customer);
            }
            //  var json = new JavaScriptSerializer().Serialize(customers);
            var json = JsonConvert.SerializeObject(customers, Newtonsoft.Json.Formatting.Indented);
            return Request.CreateResponse(HttpStatusCode.Created, json);
        }

        public List<BillToAddress> GetContact(string customerId)
        {
            List<BillToAddress> billtoaddress = new List<BillToAddress>();
            DataTable dt = GetData(string.Format("select  T1.CardCode, T1.Name, T1.Title, T1.Position, T1.Address, T1.Tel1, T1.Cellolar, T1.E_MailL, T1.Active from OCPR T1 INNER JOIN" +
                           "  OCRD  T0  ON  T0.CardCode =T1.CardCode  Where T0.CardCode ='{0}'", customerId));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                billtoaddress.Add(new BillToAddress
                {
                    CardCode = Convert.ToString(dt.Rows[i]["CardCode"])
                    ,
                    Name = Convert.ToString(dt.Rows[i]["Name"])
                    ,
                    Title = Convert.ToString(dt.Rows[i]["Title"])
                    ,
                    Position = Convert.ToString(dt.Rows[i]["Position"])
                    ,
                    Address = Convert.ToString(dt.Rows[i]["Address"])
                    ,
                    Tel1 = Convert.ToString(dt.Rows[i]["Tel1"])
                      ,
                    Cellolar = Convert.ToString(dt.Rows[i]["Cellolar"])
                    ,
                    E_MailL = Convert.ToString(dt.Rows[i]["E_MailL"])
                    ,
                    Active = Convert.ToString(dt.Rows[i]["Active"])
                });
            }
            return billtoaddress;
        }

        private DataTable GetData(string query)
        {
            string conString = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            SqlCommand cmd = new SqlCommand(query);
            using (SqlConnection con = new SqlConnection(conString))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        return dt;

                    }
                }
            }
        }
        public string DataTableToJSONWithStringBuilder(DataTable table)
        {
            var JSONString = new StringBuilder();
            string dbqt = @"""";
            if (table.Rows.Count > 0)
            {
                JSONString.Append("[");
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    JSONString.Append("{");
                    for (int j = 0; j < table.Columns.Count; j++)

                    {
                        bool is_Numeric = IsNumeric(table.Rows[i][j].ToString());
                        if (j < table.Columns.Count - 1)
                        {

                            if (is_Numeric == true)
                            {

                                JSONString.Append("" + dbqt + table.Columns[j].ColumnName.ToString() + dbqt + ":" + "" + table.Rows[i][j].ToString().Replace(dbqt, "").Replace("'", "").Trim() + ",");
                            }
                            else
                            {
                                JSONString.Append("" + dbqt + table.Columns[j].ColumnName.ToString() + dbqt + ":" + "" + dbqt + table.Rows[i][j].ToString().Replace(dbqt, "").Replace("'", "").Trim() + dbqt + ",");

                            }

                            // JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == table.Columns.Count - 1)
                        {
                            if (is_Numeric == true)
                            {

                                JSONString.Append("" + dbqt + table.Columns[j].ColumnName.ToString() + dbqt + ":" + "" + table.Rows[i][j].ToString().Replace(dbqt, "").Replace("'", "").Trim() + "");
                            }
                            else
                            {
                                JSONString.Append("" + dbqt + table.Columns[j].ColumnName.ToString() + dbqt + ":" + "" + dbqt + table.Rows[i][j].ToString().Replace(dbqt, "").Replace("'", "").Trim() + dbqt + "");

                            }
                            // JSONString.Append("\"" + dbqt+ table.Columns[j].ColumnName.ToString() + dbqt + "\":" + "\"" + table.Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == table.Rows.Count - 1)
                    {
                        JSONString.Append("}");
                    }
                    else
                    {
                        JSONString.Append("},");
                    }
                }
                JSONString.Append("]");
            }
            return JSONString.ToString();
        }
        private bool IsNumeric(string str)
        {
            float f;
            return float.TryParse(str, out f);
        }
    }
}
