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




        [Route("api/Data/FAG/")]
        public async Task<IHttpActionResult> GetFAG()
        {
            string sql = string.Format(@"select * from FAQs");
            var list = db.Database.SqlQuery<FAQs>(sql).ToList();
            return Ok(list);
        }


        [Route("api/Data/AceMessages/")]
        public async Task<IHttpActionResult> GetAceMessages()
        {
            string sql = string.Format(@"select * from AceMessages where Type = 100 and Active = 1 and Expired = 0");
            var list = db.Database.SqlQuery<AceMessages>(sql).ToList();
            return Ok(list);
        }



        


        public class FinancialDocumentsObject
        {
            public int LockNumber { get; set; }

        }

        [Route("api/Data/FinancialDocuments/")]
        public async Task<IHttpActionResult> PostFinancialDocuments(FinancialDocumentsObject FinancialDocumentsObject)
        {
            string sql = string.Format(@"select * from FinancialDocuments where LockNumber = {0} and (Download < 2 or Download is null)  order by SubmitDate desc", FinancialDocumentsObject.LockNumber);
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
            string sql = string.Format(@"select * from CustomerFiles where LockNumber = {0} and Disabled = 1  order by UploadDate desc", CustomerFilesObject.LockNumber);
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
            db.SaveChanges();

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
            var Atch = System.Web.HttpContext.Current.Request.Files["Atch"];
            var lockNumber = System.Web.HttpContext.Current.Request["LockNumber"];

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string pathtemp = path + "\\TempDirectory";
            if (!Directory.Exists(pathtemp))
                Directory.CreateDirectory(pathtemp);

            pathtemp = path + "\\TempDirectory\\" + lockNumber;
            if (!Directory.Exists(pathtemp))
                Directory.CreateDirectory(pathtemp);


          
            int lenght = Atch.ContentLength;
            byte[] filebyte = new byte[lenght];
            Atch.InputStream.Read(filebyte, 0, lenght);
            File.WriteAllBytes(pathtemp + "\\" + Atch.FileName, filebyte);
            return Ok("Ok");
        }



        public class FinalUploadFileObject
        {
            public int LockNumber { get; set; }

            public string Desc { get; set; }

        }

        [Route("api/Data/FinalUploadFile/")]
        public async Task<IHttpActionResult> FinalUploadFile(FinalUploadFileObject FinalUploadFileObject)
         {
            string path = GetPath(3);
            string date = CustomPersianCalendar.ToPersianDate(DateTime.Now).Replace('/', '-');
            string ticket = string.Format("{0:yyyyMMddHHmmssfff}", CustomPersianCalendar.GetCurrentIRNow(false));

            string pathFile = path + "\\" + date;
             if (!Directory.Exists(pathFile))
                 Directory.CreateDirectory(pathFile);

            string fullPath = string.Format("{0}\\{1}_{2}", pathFile, ticket, FinalUploadFileObject.LockNumber.ToString());
            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

           string pathtemp = path + "\\TempDirectory\\" + FinalUploadFileObject.LockNumber.ToString();

            if (Directory.Exists(pathtemp))
            {
                foreach (var file in new DirectoryInfo(pathtemp).GetFiles())
                {
                    file.MoveTo($@"{fullPath}\{file.Name}");
                }
            }

            File.WriteAllText(string.Format("{0}\\{1}.txt", fullPath, FinalUploadFileObject.LockNumber), FinalUploadFileObject.Desc, System.Text.Encoding.UTF8);

            return Ok("Ok");

        }



    }
}
