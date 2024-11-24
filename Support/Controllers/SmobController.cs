using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Support.Controllers.Unit;
using Support.Models.CustomerFiles;


namespace Support.Controllers
{
    public class SmobController : ApiController
    {
        CustomerFilesModel db = new CustomerFilesModel();

        public class Row
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public int Type { get; set; }
        }

        public class Fish
        {
            public string IdPersonal { get; set; }
            public string Mobile { get; set; }
            public List<Row> RowData { get; set; }
        }

        public class AllFish
        {
            public string LockNumber { get; set; }
            public string Sal { get; set; }
            public string Mah { get; set; }
            public List<Fish> Fish { get; set; }
        }

        // Post: api/Smob/  ارسال فیش حقوقی کارمندان
        [Route("api/Smob/SendFish")]
        public async Task<IHttpActionResult> PostSendFish(AllFish d)
        {
            string res = "";
            try
            {
                long serialNumber = 0;
                int ex;
                string sql = string.Format(@"select SerialNumber from [CustomerFiles].[dbo].[Pay_DocH] where LockNumber = {0} and Sal = {1} and mah = {2}", d.LockNumber, d.Sal, d.Mah);
                var list = db.Database.SqlQuery<Pay_DocH>(sql).ToList();
                if (list.Count() == 0)
                {
                    sql = string.Format(@" declare @serialNumber bigint = (select isnull(max(SerialNumber),0) + 1 from Pay_DocH)
                                           INSERT INTO[dbo].[Pay_DocH](SerialNumber, LockNumber, Sal, Mah)
                                           VALUES(@serialNumber ,N'{0}',{1},{2})  select @serialNumber as SerialNumber",
                                           d.LockNumber, d.Sal, d.Mah);
                    serialNumber = db.Database.SqlQuery<Int64>(sql).Single();
                }
                else
                {
                    serialNumber = list[0].SerialNumber;
                    string idPersonal = "";
                    foreach (var item in d.Fish)
                    {
                        idPersonal += "'" + item.IdPersonal + "'" + ",";
                    }
                    idPersonal = idPersonal.Substring(0, idPersonal.Length - 1);
                    sql = string.Format(@"delete [CustomerFiles].[dbo].[Pay_DocB] where SerialNumber = {0} and IdPersonal in ({1}) select 0", serialNumber, idPersonal);
                    ex = db.Database.SqlQuery<int>(sql).Single();
                }

                sql = "";
                foreach (var row in d.Fish)
                {
                    foreach (var item in row.RowData)
                    {
                        sql += string.Format(@"INSERT INTO [dbo].[Pay_DocB](SerialNumber,IdPersonal,Name,Value,Type) VALUES ({0},N'{1}',N'{2}',N'{3}',{4}) ",
                        serialNumber, row.IdPersonal, item.Name, item.Value, item.Type);
                    }

                }
                sql += " select 0";

                ex = db.Database.SqlQuery<int>(sql).Single();
                res = serialNumber.ToString();

            }
            catch (Exception e)
            {
                throw;
            }
            return Ok(res);
        }



        public class Pay_DocHObject
        {
            public long SerialNumber { get; set; }
        }

        // Post: api/Smob/Pay_DocH  فیش حقوقی کارمندان
        [Route("api/Smob/Pay_DocH")]
        public async Task<IHttpActionResult> PostPay_DocH(Pay_DocHObject d)
        {
            string sql = string.Format(@"select * from Pay_DocH where SerialNumber = {0} ", d.SerialNumber);
            var list = db.Database.SqlQuery<Pay_DocH>(sql).ToList();
            return Ok(list);
        }


        public class Pay_DocBObject
        {
            public long SerialNumber { get; set; }
            public string IdPersonal { get; set; }
        }


        // Post: api/Smob/Pay_DocB  فیش حقوقی کارمندان
        [Route("api/Smob/Pay_DocB")]
        public async Task<IHttpActionResult> PostPay_DocB(Pay_DocBObject d)
        {
            string sql = string.Format(@"select * from Pay_DocB where SerialNumber = {0} and IdPersonal = '{1}'", d.SerialNumber, d.IdPersonal);
            var list = db.Database.SqlQuery<Pay_DocB>(sql).ToList();
            return Ok(list);
        }


        // Get: api/Smob/Decrypt رمز گشایی
        [Route("api/Smob/Decrypt/{value}")]
        public async Task<IHttpActionResult> GetDecrypt(string value)
        {
            return Ok(UnitPublic.Decrypt(value));
        }


    }
}
