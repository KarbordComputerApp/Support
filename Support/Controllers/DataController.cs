using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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



        public class ChangePasswordObject
        {
            public int LockNumber { get; set; }

            public string OldPass { get; set; }

            public string NewPass { get; set; }

        }

        [Route("api/Data/ChangePassword/")]
        public async Task<IHttpActionResult> PostChangePassword(ChangePasswordObject ChangePasswordObject)
        {
            int res = 0;
            string sql = string.Format(@"select * from Users where (LockNumber = {0} and Password = '{1}')", ChangePasswordObject.LockNumber, EncodePassword(ChangePasswordObject.OldPass));
            var list = db.Database.SqlQuery<Users>(sql).ToList();
            if (list.Count > 0)
            {
                sql = string.Format(@"update Users set Password = '{0}' , ForceToChangePass = 1 where (LockNumber = {1} and Password = '{2}') select 1", EncodePassword(ChangePasswordObject.NewPass), ChangePasswordObject.LockNumber, EncodePassword(ChangePasswordObject.OldPass));
                res = db.Database.SqlQuery<int>(sql).Single();
            }
            return Ok(res);
        }



        public class LockNumbersObject
        {
            public int LockNumber { get; set; }

        }

        public class LockNumbers
        {
            public int LockNumber { get; set; }

            public string CompanyName { get; set; }

            public Int16? UserCountLimit { get; set; }

            public byte Status { get; set; }
        }

        [Route("api/Data/LockNumbers/")]
        public async Task<IHttpActionResult> PostLockNumbers(LockNumbersObject LockNumbersObject)
        {
            string sql = string.Format(@"select LockNumber,CompanyName,UserCountLimit,Status from LockNumbers where (LockNumber = {0})", LockNumbersObject.LockNumber);
            var list = db.Database.SqlQuery<LockNumbers>(sql).ToList();
            return Ok(list);
        }



        [Route("api/Data/FAG/")]
        public async Task<IHttpActionResult> GetFAG()
        {
            string sql = string.Format(@"select distinct 0 as id , Title , Title as Description , Title as Body , 0 as SortId  from FAQs
                                         union all
                                         select id,Title,Description,Body,SortId from FAQs
                                         order by title , SortId");
            var list = db.Database.SqlQuery<FAQs>(sql).ToList();
            return Ok(list);
        }


        [Route("api/Data/AceMessages/")]
        public async Task<IHttpActionResult> GetAceMessages()
        {
            string sql = string.Format(@"select * from AceMessages where ExtraParam = '' and Active = 1 and Expired = 0  order by id desc");
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
            string sql = string.Format(@"select * from FinancialDocuments where LockNumber = {0} order by SubmitDate desc", FinancialDocumentsObject.LockNumber);
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
            string sql = string.Format(@"select *,(select count(id) from  CustomerFileDownloadInfos where FileId = c.id ) as CountDownload 
                                         from CustomerFiles as c where LockNumber in( 10000 , {0} ) and Disabled = 0  order by LockNumber desc , id desc", CustomerFilesObject.LockNumber);
            var list = db.Database.SqlQuery<CustomerFiles>(sql).ToList();
            return Ok(list);
        }




        [Route("api/Data/CustomerFilesCount/")]
        public async Task<IHttpActionResult> PostCustomerFilesCount(CustomerFilesObject CustomerFilesObject)
        {
            string sql = string.Format(@"select count(id) from CustomerFiles where LockNumber = {0} and Disabled = 0", CustomerFilesObject.LockNumber);
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

            FileInfo f = new FileInfo(files[0]);

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
            response.Content.Headers.ContentDisposition.FileName = f.Name;
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(files[0]));
            return response;
        }


        public static string MergePaths(string part1, string part2)
        {
            return part1.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar + part2.TrimStart(Path.DirectorySeparatorChar);
        }

        [HttpGet]
        [Route("api/Data/CustomerDocumentsDownload/{lockNo}/{idCustomer}")]

        public HttpResponseMessage CustomerDocumentsDownload(int lockNo, long idCustomer)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);

            string path = GetPath(2);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string sql = string.Format(@"select *, 0 as CountDownload from CustomerFiles where id = {0}", idCustomer);
            var list = db.Database.SqlQuery<CustomerFiles>(sql).Single();

            string fullFileName = MergePaths(path, list.FilePath);

            FileInfo f = new FileInfo(fullFileName);
            string fullname = f.DirectoryName;

            string[] files = Directory.GetFiles(fullname, f.Name);


            if (!File.Exists(files[0]))
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.ReasonPhrase = string.Format("File not found: {0} .", files[0]);
                throw new HttpResponseException(response);
            }


            byte[] bytes = File.ReadAllBytes(files[0].ToString());

            response.Content = new ByteArrayContent(bytes);
            response.Content.Headers.ContentLength = bytes.LongLength;
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = f.Name;
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(files[0]));

            sql = string.Format(@"INSERT INTO CustomerFileDownloadInfos (FileId,IP,DownloadTime) VALUES ({0},'',getdate()) select 1", idCustomer);
            var list1 = db.Database.SqlQuery<int>(sql).Single();
            db.SaveChanges();

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
            FileInfo f = new FileInfo(files[0]);

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
            response.Content.Headers.ContentDisposition.FileName = f.Name;
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



        public partial class Message
        {
            public long id { get; set; }

            public string lockNumber { get; set; }

            public string expireDate { get; set; }

            public string title { get; set; }

            public string body { get; set; }

            public bool? active { get; set; }
        }

        // GET: api/Data/Messages
        [Route("api/Data/Messages/{lockNumber}")]
        public async Task<IHttpActionResult> GetWeb_Messages(string lockNumber)
        {
            string sql = string.Format("SELECT * FROM [dbo].[Message] where active = 1 and (lockNumber is null  or lockNumber = '{0}' or lockNumber = '')", lockNumber);
            var list = db.Database.SqlQuery<Message>(sql).ToList(); // db.Access.First(c => c.UserName == userName && c.Password == password);
            return Ok(list);
        }








        public class MailBox
        {
            public long id { get; set; }

            public byte mode { get; set; }

            public string readst { get; set; }

            public string lockNumber { get; set; }

            public string date { get; set; }

            public string title { get; set; }

            public string body { get; set; }

            public string namefile { get; set; }

        }

        public class MailBoxObject
        {
            public string LockNumber { get; set; }

            public byte Mode { get; set; }

            public string UserCode { get; set; }

        }


        // GET: api/Data/MailBox
        [Route("api/Data/MailBox/")]
        public async Task<IHttpActionResult> PostWeb_MailBox(MailBoxObject MailBoxObject)
        {
            string sql = string.Format(@"declare @mode tinyint = {0} 
                                         select id,mode,readst,locknumber,date,title,body,namefile from MailBox where lockNumber = '{1}' and 
                                                                ((@mode = 0 and (mode = 1 or mode = 2)) or mode = @mode)
                                         order by date desc , id desc", MailBoxObject.Mode, MailBoxObject.LockNumber);

            var list = db.Database.SqlQuery<MailBox>(sql).ToList();
            return Ok(list);
        }


        /*
        public class InsertMailBoxObject
        {

            public byte Mode { get; set; }

            public string LockNumber { get; set; }

            public string Date { get; set; }

            public string Title { get; set; }

            public string Body { get; set; }

            public string NameFile { get; set; }

            public string UserCode { get; set; }

        }


        // Post: api/Data/MailBox
        [Route("api/Data/InsertMailBox/")]
        public async Task<IHttpActionResult> PostWeb_InsertMailBox(InsertMailBoxObject InsertMailBoxObject)
        {
            string sql = string.Format("INSERT INTO MailBox (mode,locknumber,date,title,body,namefile)VALUES({0},N'{1}',N'{2}',N'{3}',N'{4}',N'{5}')",
                                      InsertMailBoxObject.Mode,
                                      InsertMailBoxObject.LockNumber,
                                      InsertMailBoxObject.Date,
                                      InsertMailBoxObject.Title,
                                      InsertMailBoxObject.Body,
                                      InsertMailBoxObject.NameFile);
            var list = db.Database.SqlQuery<MailBox>(sql).ToList();
            await db.SaveChangesAsync();
            return Ok(list);
        }
        */

        // get: api/Data/DeleteMailBox
        [Route("api/Data/DeleteMailBox/{lockNumber}/{id}")]
        public async Task<IHttpActionResult> GetWeb_DeleteMailBox(string lockNumber, long id)
        {
            string sql = string.Format("update MailBox set mode = 3 WHERE id = {0} and lockNumber = '{1}' and  mode = 1 select 0", id, lockNumber);
            var list = db.Database.SqlQuery<int>(sql).ToList();
            await db.SaveChangesAsync();
            return Ok(list);
        }





        /*   [Route("api/Data/UploadFileMailBox/{LockNumber}")]
           public async Task<IHttpActionResult> UploadFileMailBox(string LockNumber)
           {
               var folder = "C://App//Upload//" + LockNumber + "//";
               //var folder = "C://Test//App//Upload//" + LockNumber + "//";
               if (!Directory.Exists(folder))
               {
                   Directory.CreateDirectory(folder);
               }

               //var Atch = System.Web.HttpContext.Current.Request.Files["Atch"];
               //var req = HttpContext.Current.Request;
               //var file = req.Files[req.Files.Keys.Get(0)];

               var httpRequest = HttpContext.Current.Request.Files[0];

               var fname = httpRequest.FileName.Replace(" ", "");
               var name = fname.Split('.');

               string tempName = name[0] + "-" + DateTime.Now.ToString("yyMMddHHmmss") + "." + name[1];
               var filePath = folder + tempName;
               httpRequest.SaveAs(filePath);
               tempName = name[0] + "-" + DateTime.Now.ToString("yyMMddHHmmss") + "--" + name[1];
               return Ok(tempName);
           }

   */

        /*        public class FileDownload
                {
                    public string LockNumber { get; set; }

                    public string FileName { get; set; }

                }
                */


        /*   [HttpGet]
           [Route("api/Data/DownloadFileMailBox/{LockNumber}/{FileName}")]
           public HttpResponseMessage DownloadMailBox(string LockNumber, string FileName)
           {
               HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
               FileName = FileName.Replace("--", ".");
               string filePath = "C://App//Upload//" + LockNumber + "//" + FileName;

               if (!File.Exists(filePath))
               {
                   response.StatusCode = HttpStatusCode.NotFound;
                   response.ReasonPhrase = string.Format("File not found: {0} .", FileName);
                   throw new HttpResponseException(response);
               }


               byte[] bytes = File.ReadAllBytes(filePath);

               //Set the Response Content.
               response.Content = new ByteArrayContent(bytes);

               //Set the Response Content Length.
               response.Content.Headers.ContentLength = bytes.LongLength;

               //Set the Content Disposition Header Value and FileName.
               response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
               response.Content.Headers.ContentDisposition.FileName = FileName;

               //Set the File Content Type.
               response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(FileName));
               return response;
           }
           */

        [HttpGet]
        [Route("api/Data/DeleteFileMailBox/{LockNumber}/{FileName}")]
        public async Task<IHttpActionResult> DeleteFileMailBox(string LockNumber, string FileName)
        {
            FileName = FileName.Replace("--", ".");

            string fullPath = "C://App//Upload//" + LockNumber + "//" + FileName;

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
            return Ok("Ok");
        }



        [Route("api/Data/InsertMailBox/")]
        public async Task<IHttpActionResult> SaveMailBox()
        {
            string mode = HttpContext.Current.Request["mode"];
            string readst = HttpContext.Current.Request["readst"];
            string locknumber = HttpContext.Current.Request["locknumber"];
            string date = HttpContext.Current.Request["date"];
            string title = HttpContext.Current.Request["title"];
            string body = HttpContext.Current.Request["body"];
            string namefile = HttpContext.Current.Request["namefile"];
            var Atch = HttpContext.Current.Request.Files["Atch"];

            byte[] filebyte = new byte[0];
            if (Atch != null)
            {
                int lenght = Atch.ContentLength;
                filebyte = new byte[lenght];
                Atch.InputStream.Read(filebyte, 0, lenght);
            }

           

            var conStr = System.Configuration.ConfigurationManager.ConnectionStrings["SupportModel"].ConnectionString;
            SqlConnection connection = new SqlConnection(conStr);
            connection.Open();
            SqlCommand cmd = new SqlCommand("MailBox_Save", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@mode", mode);
            cmd.Parameters.AddWithValue("@readst", readst);
            cmd.Parameters.AddWithValue("@locknumber", locknumber);
            cmd.Parameters.AddWithValue("@date", date);
            cmd.Parameters.AddWithValue("@title", title);
            cmd.Parameters.AddWithValue("@body", body);
            cmd.Parameters.AddWithValue("@namefile", namefile);
            cmd.Parameters.AddWithValue("@Atch", filebyte);
            cmd.ExecuteNonQuery();
            connection.Close();
            return Ok("1");
        }



        public class DownloadFileMailBoxObject
        {
            public long id { get; set; }
        }

        public class DownloadFileMailBox
        {

            public string namefile { get; set; }

            public byte[] Atch { get; set; }

        }


        // Post: api/Data/DownloadFileMailBox   دانلود پیوست  
        [Route("api/Data/DownloadFileMailBox/")]
        public async Task<IHttpActionResult> PostDownloadFileMailBox(DownloadFileMailBoxObject DownloadFileMailBoxObject)
        {
            string sql = string.Format(CultureInfo.InvariantCulture, @"select namefile,Atch FROM MailBox where id = {0}", DownloadFileMailBoxObject.id);
            var list = db.Database.SqlQuery<DownloadFileMailBox>(sql);
            return Ok(list);
        }



        [Route("api/Data/GetDate/")]
        public async Task<IHttpActionResult> GetGetDate()
        {
            string sql = string.Format("select dbo.MiladiToShamsi(getdate()) as date");
            var list = db.Database.SqlQuery<string>(sql).ToList();
            return Ok(list);
        }


        


    }
}
