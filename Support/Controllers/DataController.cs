using System;
using System.Collections.Generic;
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
using System.Web.Http;
using System.Web.UI;
using Support.Controllers.Unit;
using Support.Models;

namespace Support.Controllers
{
    public class DataController : ApiController
    {
        SupportModel db = new SupportModel();

        public static string EncodePassword(string originalPassword)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            Byte[] originalBytes = ASCIIEncoding.Default.GetBytes(originalPassword);
            Byte[] encodedBytes = md5.ComputeHash(originalBytes);

            return BitConverter.ToString(encodedBytes).Replace("-", "");
        }


        public class LoginObject
        {
            public int LockNumber { get; set; }
            public string Pass { get; set; }

        }

        [Route("api/Data/Login/")]
        public async Task<IHttpActionResult> PostLogin(LoginObject LoginObject)
        {
            string sql = string.Format(@"select * from Users where (LockNumber = {0} and Password = '{1}')", LoginObject.LockNumber, EncodePassword(LoginObject.Pass));
            var list = db.Database.SqlQuery<Users>(sql).ToList();
            return Ok(list);
        }


        public class FinancialDocumentsObject
        {
            public int LockNumber { get; set; }

        }

        [Route("api/Data/FinancialDocuments/")]
        public async Task<IHttpActionResult> PostFinancialDocuments(FinancialDocumentsObject FinancialDocumentsObject)
        {
            string sql = string.Format(@"select * from FinancialDocuments where LockNumber = {0} and (Download <= 2 or Download is null)", FinancialDocumentsObject.LockNumber);
            var list = db.Database.SqlQuery<FinancialDocuments>(sql).ToList();
            return Ok(list);
        }




        public class CustomerFilesObject
        {
            public int LockNumber { get; set; }

        }

        [Route("api/Data/CustomerFiles/")]
        public async Task<IHttpActionResult> PostCustomerFiles(CustomerFilesObject CustomerFilesObject)
        {
            string sql = string.Format(@"select * from CustomerFiles where LockNumber = {0} and Disabled = 1", CustomerFilesObject.LockNumber);
            var list = db.Database.SqlQuery<CustomerFiles>(sql).ToList();
            return Ok(list);
        }


        [Route("api/Data/CustomerFilesCount/")]
        public async Task<IHttpActionResult> PostCustomerFilesCount(CustomerFilesObject CustomerFilesObject)
        {
            string sql = string.Format(@"select count(id) from CustomerFiles where LockNumber = {0} and Disabled = 1", CustomerFilesObject.LockNumber);
            var list = db.Database.SqlQuery<int>(sql).ToList();
            return Ok(list);
        }





        public string GetPath(int IdConfig)
        {
            string sql = string.Format(@"select value from Configs where id = {0}", IdConfig.ToString());
            string list = db.Database.SqlQuery<string>(sql).Single();
            return "C:" + list;
        }


        [HttpGet]
        [Route("api/Data/FinancialDocumentsDownload/{lockNo}/{idFinancial}")]
        public HttpResponseMessage FinancialDocumentsDownload(int lockNo, long idFinancial)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);

            string path = GetPath(8) + "\\" + lockNo.ToString();
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string[] files = Directory.GetFiles(path, string.Format("{0}_*", idFinancial));


            if (!File.Exists(files[0]))
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.ReasonPhrase = string.Format("File not found: {0} .", files[0]);
                throw new HttpResponseException(response);
            }


            byte[] bytes = File.ReadAllBytes(files[0].ToString());

            string sql = string.Format(@"update FinancialDocuments set ReadStatus = 1 , Download = isnull(Download,0) + 1 where id = {0} select 1", idFinancial);
            var list = db.Database.SqlQuery<int>(sql).Single();
            db.SaveChangesAsync();

            response.Content = new ByteArrayContent(bytes);
            response.Content.Headers.ContentLength = bytes.LongLength;
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = files[0];
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(files[0]));
            return response;
        }


        [HttpGet]
        [Route("api/Data/CustomerFiles/{lockNo}/{idCustomerFiles}")]
        public HttpResponseMessage CustomerFiles(int lockNo, long idCustomerFiles)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);

            string path = GetPath(2) + "\\" + lockNo.ToString();
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string[] files = Directory.GetFiles(path, string.Format("{0}_*", idCustomerFiles));


            if (!File.Exists(files[0]))
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.ReasonPhrase = string.Format("File not found: {0} .", files[0]);
                throw new HttpResponseException(response);
            }


            byte[] bytes = File.ReadAllBytes(files[0].ToString());

           // string sql = string.Format(@"update FinancialDocuments set ReadStatus = 1 , Download = isnull(Download,0) + 1 where id = {0} select 1", idCustomerFiles);
           // var list = db.Database.SqlQuery<int>(sql).Single();
           // db.SaveChangesAsync();

            response.Content = new ByteArrayContent(bytes);
            response.Content.Headers.ContentLength = bytes.LongLength;
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = files[0];
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(files[0]));
            return response;
        }





        [Route("api/Data/UploadFile/")]
        public async Task<IHttpActionResult> UploadFile()
        {
            string path = GetPath(3);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string pathtemp = path + "\\TempDirectory";
            if (!Directory.Exists(pathtemp))
                Directory.CreateDirectory(pathtemp);

            var Atch = System.Web.HttpContext.Current.Request.Files["Atch"];
            var lockNumber = System.Web.HttpContext.Current.Request["LockNumber"];

            //var req = HttpContext.Current.Request;
            //var file = req.Files[req.Files.Keys.Get(0)];



            int lenght = Atch.ContentLength;
            byte[] filebyte = new byte[lenght];
            Atch.InputStream.Read(filebyte, 0, lenght);
            File.WriteAllBytes(pathtemp + "\\" + lockNumber + "___" + Atch.FileName, filebyte);


            
            return Ok("000");
        }      



    }
}
