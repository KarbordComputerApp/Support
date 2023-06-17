using System;
using System.Collections.Generic;
using System.Data;
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


        public const int mode_CustomerFiles = 1;
        public const int mode_UploadFiles = 2;
        public const int mode_FinancialDocuments = 3;
        public const int mode_Tiket = 4;
        public const int mode_CustAccount = 5;
        public const int mode_MailBox = 6;
        public const int mode_Login = 7;
        public const int mode_Download = 11;
        public const int mode_Samane = 12;



        public const int act_Login = 1;
        public const int act_New = 2;
        public const int act_View = 3;
        public const int act_Print = 4;
        public const int act_Download = 5;
        public const int act_ChangePass = 6;



        public class LoginObject
        {
            public int LockNumber { get; set; }
            public string Pass { get; set; }
            public string IP { get; set; }
            public string CallProg { get; set; }

        }

        [Route("api/Data/Login/")]
        public async Task<IHttpActionResult> PostLogin(LoginObject LoginObject)
        {
            string sql = string.Format(@"select * from Users where (LockNumber = {0} and Password = '{1}')", LoginObject.LockNumber, EncodePassword(LoginObject.Pass));
            var list = db.Database.SqlQuery<UsersLogin>(sql).ToList();

            if (list.Count > 0)
            {
                UnitPublic.SaveLog(LoginObject.LockNumber, mode_Login, act_Login, 0, LoginObject.IP, LoginObject.CallProg, "");
            }

            return Ok(list);
        }







        public class SamaneTrsObject
        {
            public string LockNumber { get; set; }

            public string IP { get; set; }

            public string CallProg { get; set; }
        }

        [Route("api/Data/SamaneTrs/")]
        public async Task<IHttpActionResult> PostSamaneTrs(SamaneTrsObject SamaneTrsObject)
        {
            string sql = string.Format(@"SELECT Id,UserName,Password,FirstName,LastName,Email,UserType,LockNumber,DateRegistred,ForceToChangePass,VerificationStatus,TrsDownload,
                                                case when SamaneTrs = '' or SamaneTrs = '   ' then '1402/03/31	10000' else  SamaneTrs end as SamaneTrs
                                         FROM   Users where (LockNumber = {0})", SamaneTrsObject.LockNumber);
            var list = db.Database.SqlQuery<Users>(sql).ToList();

            if (list.Count > 0)
            {
                UnitPublic.SaveLog(Convert.ToInt16(SamaneTrsObject.LockNumber), mode_Samane, act_Login, 0, SamaneTrsObject.IP, SamaneTrsObject.CallProg, "");
            }

            return Ok(list);
        }




        public class ChangePasswordObject
        {
            public int LockNumber { get; set; }

            public string OldPass { get; set; }

            public string NewPass { get; set; }

            public string IP { get; set; }

            public string CallProg { get; set; }

        }

        [Route("api/Data/ChangePassword/")]
        public async Task<IHttpActionResult> PostChangePassword(ChangePasswordObject ChangePasswordObject)
        {
            int res = 0;
            string sql = string.Format(@"select * from Users where (LockNumber = {0} and Password = '{1}')", ChangePasswordObject.LockNumber, EncodePassword(ChangePasswordObject.OldPass));
            var list = db.Database.SqlQuery<Users>(sql).ToList();
            if (list.Count > 0)
            {
                sql = string.Format(@"update Users set Password = '{0}' , ForceToChangePass = 0 where (LockNumber = {1} and Password = '{2}') select 1", EncodePassword(ChangePasswordObject.NewPass), ChangePasswordObject.LockNumber, EncodePassword(ChangePasswordObject.OldPass));
                res = db.Database.SqlQuery<int>(sql).Single();
            }

            if (res == 1)
            {
                UnitPublic.SaveLog(ChangePasswordObject.LockNumber, mode_Login, act_ChangePass, 0, ChangePasswordObject.IP, ChangePasswordObject.CallProg, "");
            }

            return Ok(res);
        }

        private string Token(byte Length)
        {
            char[] Chars = new char[] {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9','@','!','#'
        };
            string String = string.Empty;
            Random Random = new Random();

            for (byte a = 0; a < Length; a++)
            {
                String += Chars[Random.Next(0, 61)];
            };

            return (String);
        }

        public class RecoveryPasswordObject
        {
            public int LockNumber { get; set; }

            public string Email { get; set; }

            public string IP { get; set; }

            public string CallProg { get; set; }
        }

        [Route("api/Data/RecoveryPassword/")]
        public async Task<IHttpActionResult> PostRecoveryPassword(RecoveryPasswordObject RecoveryPasswordObject)
        {
            int res = 0;
            string sql = string.Format(@"select * from Users where (LockNumber = {0} and Email = '{1}')", RecoveryPasswordObject.LockNumber, RecoveryPasswordObject.Email);
            var list = db.Database.SqlQuery<Users>(sql).ToList();



            if (list.Count > 0)
            {
                string newPass = Token(10);
                string resEmail = UnitPublic.SendEmail(RecoveryPasswordObject.Email, "کاربرد کامپیوتر", "کلمه ورود به سایت ساپورت = " + newPass);
                if (resEmail == "Send")
                {
                    sql = string.Format(@"update Users set Password = '{0}' , ForceToChangePass = 1 where id = {1}  select 1", EncodePassword(newPass), list[0].Id);
                    res = db.Database.SqlQuery<int>(sql).Single();
                }
                else
                {
                    return Ok(resEmail);
                }

            }

            if (res == 1)
            {
                UnitPublic.SaveLog(RecoveryPasswordObject.LockNumber, mode_Login, act_ChangePass, 0, RecoveryPasswordObject.IP, RecoveryPasswordObject.CallProg, "");
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
            string sql = string.Format(@"select LockNumber,replace (CompanyName , N'ي' , N'ی') as CompanyName,UserCountLimit,Status from LockNumbers where (LockNumber = {0})", LockNumbersObject.LockNumber);
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

            public bool FlagLog { get; set; }

            public string IP { get; set; }

            public string CallProg { get; set; }

        }

        [Route("api/Data/FinancialDocuments/")]
        public async Task<IHttpActionResult> PostFinancialDocuments(FinancialDocumentsObject FinancialDocumentsObject)
        {
            string sql = string.Format(@"select * from FinancialDocuments where LockNumber = {0} order by SubmitDate desc", FinancialDocumentsObject.LockNumber);
            var list = db.Database.SqlQuery<FinancialDocuments>(sql).ToList();

            if (FinancialDocumentsObject.FlagLog == true)
            {
                UnitPublic.SaveLog(FinancialDocumentsObject.LockNumber, mode_FinancialDocuments, act_View, 0, FinancialDocumentsObject.IP, FinancialDocumentsObject.CallProg, "");
            }

            return Ok(list);
        }


        public class CustomerFilesObject
        {
            public int LockNumber { get; set; }

            public bool FlagLog { get; set; }
            public string IP { get; set; }

            public string CallProg { get; set; }

        }

        [Route("api/Data/CustomerFiles/")]
        public async Task<IHttpActionResult> PostCustomerFiles(CustomerFilesObject CustomerFilesObject)
        {
            string sql = string.Format(@"select *,(select count(id) from  CustomerFileDownloadInfos where FileId = c.id ) as CountDownload 
                                         from CustomerFiles as c where LockNumber in( 10000 , {0} ) and Disabled = 0  order by LockNumber desc , id desc", CustomerFilesObject.LockNumber);
            var list = db.Database.SqlQuery<CustomerFiles>(sql).ToList();
            if (CustomerFilesObject.FlagLog == true)
            {
                UnitPublic.SaveLog(CustomerFilesObject.LockNumber, mode_CustomerFiles, act_View, 0, CustomerFilesObject.IP, CustomerFilesObject.CallProg, "");
            }
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
        [Route("api/Data/FinancialDocumentsDownload_Test/{lockNo}/{idFinancial}")]
        public async Task<IHttpActionResult> FinancialDocumentsDownload_Test(int lockNo, long idFinancial)
        {
            string path = GetPath(8) + "\\" + lockNo.ToString();
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string[] files = Directory.GetFiles(path, string.Format("{0}_*", idFinancial));
            if (files.Length == 0)
            {
                return Ok("NotFound");
            }
            return Ok("OK");
        }


        [HttpGet]
        [Route("api/Data/FinancialDocumentsDownload/{lockNo}/{idFinancial}/{IP}/{CallProg}")]
        public HttpResponseMessage FinancialDocumentsDownload(int lockNo, long idFinancial, string IP, string CallProg)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);

            string path = GetPath(8) + "\\" + lockNo.ToString();
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string[] files = Directory.GetFiles(path, string.Format("{0}_*", idFinancial));
            if (files.Length == 0)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.ReasonPhrase = string.Format("File not found");
                return response;
            }
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

            UnitPublic.SaveLog(lockNo, mode_FinancialDocuments, act_Download, 0, IP.Replace("-", "."), CallProg, f.Name);

            return response;
        }


        public static string MergePaths(string part1, string part2)
        {
            return part1.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar + part2.TrimStart(Path.DirectorySeparatorChar);
        }


        [HttpGet]
        [Route("api/Data/CustomerDownload_Test/{idCustomer}")]
        public async Task<IHttpActionResult> CustomerDownload_Test(long idCustomer)
        {
            string path = GetPath(2);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string sql = string.Format(@"select *, 0 as CountDownload from CustomerFiles where id = {0}", idCustomer);
            var list = db.Database.SqlQuery<CustomerFiles>(sql).Single();

            string fullFileName = MergePaths(path, list.FilePath);

            FileInfo f = new FileInfo(fullFileName);
            string fullname = f.DirectoryName;

            string[] files = Directory.GetFiles(fullname, f.Name);


            if (files.Length == 0)
            {
                return Ok("NotFound");
            }
            return Ok("OK");
        }


        [HttpGet]
        [Route("api/Data/CustomerDocumentsDownload/{lockNo}/{idCustomer}/{IP}/{CallProg}")]

        public HttpResponseMessage CustomerDocumentsDownload(int lockNo, long idCustomer, string IP, string CallProg)
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

            UnitPublic.SaveLog(lockNo, mode_CustomerFiles, act_Download, 0, IP.Replace("-", "."), CallProg, f.Name);

            return response;
        }





        public class TTMS
        {
            public string FilePath { get; set; }

        }
        [HttpGet]
        [Route("api/Data/TTMSDownload/")]

        public byte[] TTMSDownload()
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);

            string path = GetPath(2);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string sql = string.Format(@"select top(1) filepath from CustomerFiles where FilePath like '%TTMS%' order by id desc");
            var list = db.Database.SqlQuery<TTMS>(sql).ToList();

            string fullFileName = MergePaths(path, list[0].FilePath);

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

            return bytes;
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

            public string IP { get; set; }

            public string CallProg { get; set; }

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

            UnitPublic.SaveLog(FinalUploadFileObject.LockNumber, mode_UploadFiles, act_New, 0, FinalUploadFileObject.IP, FinalUploadFileObject.CallProg, "");
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

            public int CountAttach { get; set; }
            public string Tanzim { get; set; }

        }

        public class MailBoxObject
        {
            public string LockNumber { get; set; }

            public byte Mode { get; set; }

            public string UserCode { get; set; }

            public bool FlagLog { get; set; }

            public string IP { get; set; }

            public string CallProg { get; set; }
        }


        // GET: api/Data/MailBox
        [Route("api/Data/MailBox/")]
        public async Task<IHttpActionResult> PostWeb_MailBox(MailBoxObject MailBoxObject)
        {
            string sql = string.Format(@"declare @mode tinyint = {0} 
                                         select id,mode,readst,locknumber,date,title,body,Tanzim,(select COUNT(IId) from DocAttach as d where d.SerialNumber = m.id) as CountAttach from MailBox as m where m.lockNumber = '{1}' and 
                                                                ((@mode = 0 and (m.mode = 1 or m.mode = 2)) or m.mode = @mode)
                                         order by m.date desc , m.id desc", MailBoxObject.Mode, MailBoxObject.LockNumber);

            var list = db.Database.SqlQuery<MailBox>(sql).ToList();

            if (MailBoxObject.FlagLog == true)
            {
                UnitPublic.SaveLog(Int32.Parse(MailBoxObject.LockNumber), mode_MailBox, act_View, 0, MailBoxObject.IP, MailBoxObject.CallProg, "");
            }
            return Ok(list);
        }


        public class ReadMailBoxObject
        {

            public long Id { get; set; }

            public string ReadSt { get; set; }

        }


        // Post: api/Data/ReadMailBox
        [Route("api/Data/ReadMailBox/")]
        public async Task<IHttpActionResult> PostWeb_ReadMailBox(ReadMailBoxObject ReadMailBoxObject)
        {
            string sql = string.Format("update MailBox set readst = '{0}' where id = {1} select 1",
                                        ReadMailBoxObject.ReadSt,
                                        ReadMailBoxObject.Id);

            int list = db.Database.SqlQuery<int>(sql).Single();
            await db.SaveChangesAsync();
            return Ok(list);
        }


        /*       // get: api/Data/DeleteMailBox
               [Route("api/Data/DeleteMailBox/{lockNumber}/{id}")]
               public async Task<IHttpActionResult> GetWeb_DeleteMailBox(string lockNumber, long id)
               {
                   string sql = string.Format("update MailBox set mode = 3 WHERE id = {0} and lockNumber = '{1}' and  mode = 1 select 0", id, lockNumber);
                   var list = db.Database.SqlQuery<int>(sql).ToList();
                   await db.SaveChangesAsync();
                   return Ok(list);
               }


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


           */



        public class InsertMailBoxObject
        {

            public int Mode { get; set; }

            public string ReadSt { get; set; }

            public string LockNumber { get; set; }

            public string Date { get; set; }

            public string Title { get; set; }

            public string Body { get; set; }

            public string IP { get; set; }
            public string CallProg { get; set; }

        }


        public class InsertMailBox
        {
            public long id { get; set; }


        }

        [Route("api/Data/InsertMailBox/")]
        public async Task<IHttpActionResult> SaveMailBox(InsertMailBoxObject InsertMailBoxObject)
        {
            string sql = string.Format(@"INSERT INTO MailBox (mode,readst,locknumber,date,title,body) values ({0},'{1}',N'{2}',N'{3}',N'{4}',N'{5}') select cast (@@IDENTITY as bigint) as id",
                                         InsertMailBoxObject.Mode,
                                         InsertMailBoxObject.ReadSt,
                                         InsertMailBoxObject.LockNumber,
                                         InsertMailBoxObject.Date,
                                         InsertMailBoxObject.Title,
                                         UnitPublic.ConvertTextWebToWin(InsertMailBoxObject.Body));

            var list = db.Database.SqlQuery<InsertMailBox>(sql).Single();
            db.SaveChanges();

            UnitPublic.SaveLog(Int32.Parse(InsertMailBoxObject.LockNumber), mode_MailBox, act_New, 0, InsertMailBoxObject.IP, InsertMailBoxObject.CallProg, "");
            return Ok(list.id);
        }



        [Route("api/Data/UploadMailBoxFile")]
        public async Task<IHttpActionResult> UploadMailBoxFile()
        {
            string SerialNumber = HttpContext.Current.Request["SerialNumber"];
            string BandNo = HttpContext.Current.Request["BandNo"];
            string FName = HttpContext.Current.Request["FName"];
            var Atch = System.Web.HttpContext.Current.Request.Files["Atch"];

            int lenght = Atch.ContentLength;
            byte[] filebyte = new byte[lenght];
            Atch.InputStream.Read(filebyte, 0, lenght);

            var dataAccount = UnitDatabase.ReadUserPassHeader(this.Request.Headers);
            String connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SupportModel"].ConnectionString;

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            SqlCommand cmd = new SqlCommand("DocAttachMailBox_Save", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@SerialNumber", SerialNumber);
            cmd.Parameters.AddWithValue("@ProgName", "Supp");
            cmd.Parameters.AddWithValue("@BandNo", BandNo);
            cmd.Parameters.AddWithValue("@ModeCode", 1);
            cmd.Parameters.AddWithValue("@FName", FName);
            cmd.Parameters.AddWithValue("@Atch", filebyte);

            cmd.ExecuteNonQuery();
            connection.Close();
            return Ok(1);
        }





        public class DocAttachBoxListObject
        {
            public long Id { get; set; }

            public byte ByData { get; set; }
        }

        public class DocAttachBoxList
        {

            public int? IId { get; set; }

            public long? SerialNumber { get; set; }

            public int? BandNo { get; set; }

            public string FName { get; set; }

            public byte[] Atch { get; set; }

        }


        // Post: api/Data/DocAttachBoxList   لیست پیوست  
        [Route("api/Data/DocAttachBoxList/")]
        public async Task<IHttpActionResult> PostDownloadFileMailBox(DocAttachBoxListObject DocAttachBoxListObject)
        {
            string sql = "select IId,SerialNumber,BandNo,FName,";

            if (DocAttachBoxListObject.ByData == 0)
                sql += "cast('' as image) as Atch ";
            else
                sql += " Atch ";

            sql += " FROM DocAttach where ";

            if (DocAttachBoxListObject.ByData == 0)
                sql += string.Format(" SerialNumber = {0}", DocAttachBoxListObject.Id);
            else
                sql += string.Format("IId = {0}", DocAttachBoxListObject.Id);

            var list = db.Database.SqlQuery<DocAttachBoxList>(sql);
            return Ok(list);
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




        // Get: api/Data/GetToken   
        [Route("api/Data/Token/{lockNumber}")]
        public async Task<IHttpActionResult> GetToken(string lockNumber)
        {
            string currentDate = DateTime.Now.Ticks.ToString();
            var token = UnitPublic.Encrypt(lockNumber + "--" + currentDate);
            return Ok(token);
        }



        public class LastCustomerFiles
        {
            public long Id { get; set; }

            public int? LockNumber { get; set; }

            public string Date { get; set; }

            public string FileName { get; set; }

            public string FilePath { get; set; }
        }


        //http://localhost:52798/api/Data/LastCustomerFiles/4OClgAD-oIzeawIDNx86MvzfUjUlCURKy-4gjG1r3pI=

        // Post: api/Data/LastCustomerFiles   
        [Route("api/Data/LastCustomerFile/{Token}")]
        public async Task<IHttpActionResult> GetLastCustomerFile(string Token)
        {
            long currentDate = DateTime.Now.Ticks;
            var inputToken = UnitPublic.Decrypt(Token);
            var data = inputToken.Split('-');
            if (data.Length == 3)
            {
                string lockNumber = data[0];
                Int64 tik = Int64.Parse(data[2]);
                long elapsedTicks = currentDate - tik;
                TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);

                //if (elapsedSpan.TotalMinutes <= 1)
                {
                    string sql = string.Format(@"SELECT Id, LockNumber, dbo.MiladiToShamsi(UploadDate) as Date, FileName, FilePath FROM CustomerFiles
                                                 where  id = (select max(id) from CustomerFiles where LockNumber = {0})", lockNumber);
                    var list = db.Database.SqlQuery<LastCustomerFiles>(sql).Single();
                    return Ok(list.Id.ToString() + ',' + list.LockNumber.ToString() + ',' + list.Date + ',' + list.FileName + ',' + list.FilePath);
                }
            }
            return Ok("");
        }




        [HttpGet]
        [Route("api/Data/LastCustomerFileDownload/{Id}")]

        public byte[] LastCustomerFileDownload(string id)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);

            string path = GetPath(2);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string sql = string.Format(@"select filepath from CustomerFiles where id = {0}", id);
            var filePath = db.Database.SqlQuery<string>(sql).Single();

            string fullFileName = path + "\\" + filePath.ToString();

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

            return bytes;
        }




        [HttpGet]
        [Route("api/Data/KarbordDownload/{Mode}/{Name}/")]
        public HttpResponseMessage KarbordDownload(String Mode, String Name)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);

            string path = GetPath(43) + "\\" + Mode;

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string[] files = Directory.GetFiles(path, string.Format("{0}.*", Name));

            FileInfo f = new FileInfo(files[0]);

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

            return response;
        }



        public class ChangeUserObject
        {
            public long IdUser { get; set; }

            public string Name { get; set; }

            public string Tel { get; set; }

            public string Mobile { get; set; }

            public string Address { get; set; }

            public string Email { get; set; }

            public bool DelImage { get; set; }
        }

        [Route("api/Data/ChangeUser/")]
        public async Task<IHttpActionResult> PostChangeUser(ChangeUserObject ChangeUserObject)
        {
            int res = 0;
            string sql = string.Format(@"update Users set Name = N'{0}',Tel = '{1}',Mobile = '{2}',Address = N'{3}',Email = '{4}' where id = {5} select 1",
                                  ChangeUserObject.Name,
                                  ChangeUserObject.Tel,
                                  ChangeUserObject.Mobile,
                                  ChangeUserObject.Address,
                                  ChangeUserObject.Email,
                                  ChangeUserObject.IdUser
                                  );
            res = db.Database.SqlQuery<int>(sql).Single();
            db.SaveChangesAsync();

            if (ChangeUserObject.DelImage == true)
            {
                sql = string.Format(@"update Users set pic = null where id = {0} select 1", ChangeUserObject.IdUser);
                db.Database.SqlQuery<int>(sql).Single();
                db.SaveChangesAsync();
            }
            return Ok(res);
        }

        public static Image resizeImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
        }

        public byte[] ConvertToBitmap(Stream bmpStream)
        {
            Image image = Image.FromStream(bmpStream);

            image = resizeImage(image, new Size(256, 256));

            MemoryStream stream = new MemoryStream();
            image.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
            return stream.ToArray();
        }




        [Route("api/Data/SaveUserImage")]
        public async Task<IHttpActionResult> PostSaveUserImage()
        {
            var idUser = HttpContext.Current.Request["IdUser"];
            var Atch = System.Web.HttpContext.Current.Request.Files["Atch"];

            int lenght = Atch.ContentLength;
            byte[] filebyte = new byte[lenght];
            Atch.InputStream.Read(filebyte, 0, lenght);

            Stream S = new MemoryStream(filebyte);
            filebyte = ConvertToBitmap(S);
            Atch.InputStream.Read(filebyte, 0, lenght);

            var conStr = db.Database.Connection.ConnectionString;
            SqlConnection connection = new SqlConnection(conStr);
            connection.Open();
            string sql = string.Format(@"update Users set Pic = @Pic  where id = {0} ", idUser);
            SqlCommand cmd = new SqlCommand(sql, connection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Pic", filebyte);
            cmd.ExecuteNonQuery();
            connection.Close();

            sql = string.Format(@"select Pic from Users where id = {0} ", idUser);
            var res = db.Database.SqlQuery<byte[]>(sql).Single();
            return Ok(res);

        }



        // Post: api/Data/DownloadVideo   دانلود ویدیو  
        [HttpGet]
        [Route("api/Data/DownloadVideo/{LockNumber}/{CallProg}/{IP}/{VideoName}")]
        public HttpResponseMessage GetDownloadVideo(int LockNumber, string CallProg, string IP, string VideoName)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);

            string sql = string.Format(@"select TrsDownload from Users where LockNumber = {0} ", LockNumber);
            var trsDownload = db.Database.SqlQuery<string>(sql).Single();

            var trsList = trsDownload.Split('-');

            bool access = false;
            //VIDEO_AFI2-VIDEO_AFI3-VIDEO_ACC6-VIDEO_CSH5-VIDEO_FCT6-VIDEO_INV6-VIDEO_PAY6-VIDEO_ERJ1-API-
            var allVideo = "VIDEO";
            string ounVideo = "";

            if (VideoName == "Karbord-AFI2")
            {
                ounVideo = "VIDEO_AFI2";
                var results = Array.FindAll(trsList, s => s.Equals(ounVideo) || s.Equals(allVideo));
                access = results.Count() > 0;
            }
            else if (VideoName == "Karbord-AFI3")
            {
                ounVideo = "VIDEO_AFI3";
                var results = Array.FindAll(trsList, s => s.Equals(ounVideo) || s.Equals(allVideo));
                access = results.Count() > 0;
            }
            else if (VideoName == "Karbord-ACC6")
            {
                ounVideo = "VIDEO_ACC6";
                var results = Array.FindAll(trsList, s => s.Equals(ounVideo) || s.Equals(allVideo));
                access = results.Count() > 0;
            }
            else if (VideoName == "Karbord-CSH5")
            {
                ounVideo = "VIDEO_CSH5";
                var results = Array.FindAll(trsList, s => s.Equals(ounVideo) || s.Equals(allVideo));
                access = results.Count() > 0;
            }
            else if (VideoName == "Karbord-FCT6")
            {
                ounVideo = "VIDEO_FCT6";
                var results = Array.FindAll(trsList, s => s.Equals(ounVideo) || s.Equals(allVideo));
                access = results.Count() > 0;
            }
            else if (VideoName == "Karbord-INV6")
            {
                ounVideo = "VIDEO_INV6";
                var results = Array.FindAll(trsList, s => s.Equals(ounVideo) || s.Equals(allVideo));
                access = results.Count() > 0;
            }
            else if (VideoName == "Karbord-PAY6")
            {
                ounVideo = "VIDEO_PAY6";
                var results = Array.FindAll(trsList, s => s.Equals(ounVideo) || s.Equals(allVideo));
                access = results.Count() > 0;
            }
            else if (VideoName == "Karbord-ERJ1")
            {
                ounVideo = "VIDEO_ERJ1";
                var results = Array.FindAll(trsList, s => s.Equals(ounVideo) || s.Equals(allVideo));
                access = results.Count() > 0;
            }

            if (access)
            {
                string path = GetPath(43) + "\\Learn\\";

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string[] files = Directory.GetFiles(path, string.Format("{0}.*", VideoName));

                FileInfo f = new FileInfo(files[0]);

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
                UnitPublic.SaveLog(LockNumber, mode_Download, act_Download, 0, IP, CallProg, VideoName);
            }
            else
            {
                response.StatusCode = HttpStatusCode.OK;
                response.Content = new StringContent("دسترسی ندارید");
            }
            return response;
        }

    }
}
