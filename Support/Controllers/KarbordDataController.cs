using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Office.Interop.Word;
using Spire.Doc;
using Support.Controllers.Unit;
using Support.Models;

namespace Support.Controllers
{
    public class KarbordDataController : ApiController
    {


        public const int mode_CustomerFiles = 1;
        public const int mode_UploadFiles = 2;
        public const int mode_FinancialDocuments = 3;
        public const int mode_Tiket = 4;
        public const int mode_CustAccount = 5;
        public const int mode_MailBox = 6;
        public const int mode_Login = 7;



        public const int act_Login = 1;
        public const int act_New = 2;
        public const int act_View = 3;
        public const int act_Print = 4;
        public const int act_Download = 5;
        public const int act_ChangePass = 6;
        public const int act_NewTiketByLink = 7;
        public const int act_ViewTiketByLink = 8;


        KarbordComputer_SupportModel db = new KarbordComputer_SupportModel();


        // GET: api/KarbordData/Date تاریخ سرور
        [Route("api/KarbordData/Date")]
        public async Task<IHttpActionResult> GetWeb_Date()
        {
            string sql = string.Format(@"select dbo.Web_CurrentShamsiDate() as tarikh");
            var dataAccount = UnitDatabase.ReadUserPassHeader(this.Request.Headers);
            var list = db.Database.SqlQuery<string>(sql);
            return Ok(list);
        }

        /*   // GET: api/KarbordData/Group گروه ها
           [Route("api/KarbordData/Group")]
           public async Task<IHttpActionResult> GetWeb_Group()
           {
               var Person = new { GroupTicket = UnitPublic.sql_Group_Ticket, GroupCustAccount = UnitPublic.sql_Group_CustAccount };
               return Ok(Person);
           }*/




        public class Object_TicketStatus
        {
            public string SerialNumber { get; set; }

            public string LockNumber { get; set; }
        }


        [Route("api/KarbordData/Web_TicketStatus")]
        public async Task<IHttpActionResult> PostWeb_TicketStatus(Object_TicketStatus c)
        {
            string sql = string.Format(@"declare @serialnumber nvarchar(100) = '{0}'
                                         select * from Web_TicketStatus where (@serialnumber = '' or serialnumber = @serialnumber) ",
                                         c.SerialNumber);
            if (c.LockNumber != null)
            {
                sql += " and LockNumber = " + c.LockNumber;
            }
            var dataAccount = UnitDatabase.ReadUserPassHeader(this.Request.Headers);
            var list = db.Database.SqlQuery<Web_TicketStatus>(sql);
            return Ok(list);
        }

        public class ErjSaveTicket_HI
        {
            public long SerialNumber { get; set; }

            public string DocDate { get; set; }

            public string UserCode { get; set; }

            public string Status { get; set; }

            public string Spec { get; set; }

            public string LockNo { get; set; }

            public string Text { get; set; }

            public string F01 { get; set; }

            public string F02 { get; set; }

            public string F03 { get; set; }

            public string F04 { get; set; }

            public string F05 { get; set; }

            public string F06 { get; set; }

            public string F07 { get; set; }

            public string F08 { get; set; }

            public string F09 { get; set; }

            public string F10 { get; set; }

            public string F11 { get; set; }

            public string F12 { get; set; }

            public string F13 { get; set; }

            public string F14 { get; set; }

            public string F15 { get; set; }

            public string F16 { get; set; }

            public string F17 { get; set; }

            public string F18 { get; set; }

            public string F19 { get; set; }

            public string F20 { get; set; }

            public string Motaghazi { get; set; }

            public string IP { get; set; }

            public string CallProg { get; set; }

            public bool LoginLink { get; set; }

            public byte ChatMode { get; set; }
        }


        [Route("api/KarbordData/ErjSaveTicket_HI")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PostErjSaveTicket_HI(ErjSaveTicket_HI ErjSaveTicket_HI)
        {
            string sql = string.Format(@"
                                    DECLARE	@DocNo_Out int
                                    EXEC	[dbo].[Web_ErjSaveTicket_HI]
		                                    @SerialNumber = {0},
		                                    @DocDate = '{1}',
		                                    @UserCode = '{2}',
		                                    @Status = '{3}',
		                                    @Spec = N'{4}',
		                                    @LockNo = '{5}',
		                                    @Text = N'{6}',
		                                    @F01 = N'{7}',
		                                    @F02 = N'{8}',
		                                    @F03 = N'{9}',
		                                    @F04 = N'{10}',
		                                    @F05 = N'{11}',
		                                    @F06 = N'{12}',
		                                    @F07 = N'{13}',
		                                    @F08 = N'{14}',
		                                    @F09 = N'{15}',
		                                    @F10 = N'{16}',
		                                    @F11 = N'{17}',
		                                    @F12 = N'{18}',
		                                    @F13 = N'{19}',
		                                    @F14 = N'{20}',
		                                    @F15 = N'{21}',
		                                    @F16 = N'{22}',
		                                    @F17 = N'{23}',
		                                    @F18 = N'{24}',
		                                    @F19 = N'{25}',
		                                    @F20 = N'{26}',
		                                    @Motaghazi = N'{27}',
		                                    @ChatMode = {28},
		                                    @DocNo_Out = @DocNo_Out OUTPUT
                                    SELECT	@DocNo_Out as N'DocNo_Out'",
                                           ErjSaveTicket_HI.SerialNumber,
                                           ErjSaveTicket_HI.DocDate ?? string.Format("{0:yyyy/MM/dd}", DateTime.Now.AddDays(-1)),
                                           ErjSaveTicket_HI.UserCode,
                                           ErjSaveTicket_HI.Status,
                                           ErjSaveTicket_HI.Spec,
                                           ErjSaveTicket_HI.LockNo,
                                           UnitPublic.ConvertTextWebToWin(ErjSaveTicket_HI.Text ?? ""),
                                           ErjSaveTicket_HI.F01,
                                           ErjSaveTicket_HI.F02,
                                           ErjSaveTicket_HI.F03,
                                           ErjSaveTicket_HI.F04,
                                           ErjSaveTicket_HI.F05,
                                           ErjSaveTicket_HI.F06,
                                           ErjSaveTicket_HI.F07,
                                           ErjSaveTicket_HI.F08,
                                           ErjSaveTicket_HI.F09,
                                           ErjSaveTicket_HI.F10,
                                           ErjSaveTicket_HI.F11,
                                           ErjSaveTicket_HI.F12,
                                           ErjSaveTicket_HI.F13,
                                           ErjSaveTicket_HI.F14,
                                           ErjSaveTicket_HI.F15,
                                           ErjSaveTicket_HI.F16,
                                           ErjSaveTicket_HI.F17,
                                           ErjSaveTicket_HI.F18,
                                           ErjSaveTicket_HI.F19,
                                           ErjSaveTicket_HI.F20,
                                           ErjSaveTicket_HI.Motaghazi,
                                           ErjSaveTicket_HI.ChatMode
                                           );

            var dataAccount = UnitDatabase.ReadUserPassHeader(this.Request.Headers);
            var list = db.Database.SqlQuery<int>(sql).Single();
            await db.SaveChangesAsync();

            UnitPublic.SaveLog(Int32.Parse(ErjSaveTicket_HI.LockNo), mode_Tiket, ErjSaveTicket_HI.LoginLink == true ? act_NewTiketByLink : act_New, 0, ErjSaveTicket_HI.IP, ErjSaveTicket_HI.CallProg, "");

            return Ok(list);
        }




        public class Object_ErjDocXK
        {
            public long SerialNumber { get; set; }

            public int ModeCode { get; set; }

            public string LockNo { get; set; }

            public bool FlagLog { get; set; }

            public string IP { get; set; }

            public string CallProg { get; set; }

            public bool LoginLink { get; set; }

        }


        [Route("api/KarbordData/Web_ErjDocXK")]
        public async Task<IHttpActionResult> PostWeb_ErjDocXK(Object_ErjDocXK Object_ErjDocXK)
        {
            string sql = string.Format("select * from dbo.Web_ErjDocXK({0},'{1}') ", Object_ErjDocXK.ModeCode, Object_ErjDocXK.LockNo);
            if (Object_ErjDocXK.SerialNumber > 0)
            {
                sql += " where SerialNumber = " + Object_ErjDocXK.SerialNumber.ToString();
            }
            sql += " order by DocDate desc , SerialNumber desc";

            var dataAccount = UnitDatabase.ReadUserPassHeader(this.Request.Headers);
            var list = db.Database.SqlQuery<Web_ErjDocXK>(sql);
            if (Object_ErjDocXK.FlagLog == true)
            {
                UnitPublic.SaveLog(Int32.Parse(Object_ErjDocXK.LockNo), mode_Tiket, Object_ErjDocXK.LoginLink == true ? act_ViewTiketByLink : act_View, 0, Object_ErjDocXK.IP, Object_ErjDocXK.CallProg, "");
            }
            return Ok(list);
        }



        public class Object_CountErjDocXK
        {
            public int ModeCode { get; set; }

            public string LockNo { get; set; }

        }


        [Route("api/KarbordData/Web_CountErjDocXK")]
        public async Task<IHttpActionResult> PostWeb_CountErjDocXK(Object_CountErjDocXK Object_CountErjDocXK)
        {
            string sql = string.Format("select count(1) as countRead from dbo.Web_ErjDocXK({0},'{1}')  where ResultRead = 1", Object_CountErjDocXK.ModeCode, Object_CountErjDocXK.LockNo);
            var dataAccount = UnitDatabase.ReadUserPassHeader(this.Request.Headers);
            var list = db.Database.SqlQuery<int>(sql);
            return Ok(list);
        }


        public class Object_Ticket_UpdateResult
        {
            public long SerialNumber { get; set; }
        }

        // Post: api/KarbordData/Ticket_UpdateResult پیوست  
        [Route("api/KarbordData/Ticket_UpdateResult/")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PostWeb_Ticket_UpdateResult(Object_Ticket_UpdateResult Object_Ticket_UpdateResult)
        {
            string sql = string.Format(CultureInfo.InvariantCulture, @"EXEC Web_ErjSaveTicket_UpdateResult @SerialNumber = {0} select 1 ", Object_Ticket_UpdateResult.SerialNumber);
            var dataAccount = UnitDatabase.ReadUserPassHeader(this.Request.Headers);
            var res = db.Database.SqlQuery<int>(sql);
            return Ok(res);
        }



        public class DocAttachObject
        {
            public string ProgName { get; set; }

            public int ModeCode { get; set; }

            public string Group { get; set; }

            public string Year { get; set; }

            public long SerialNumber { get; set; }

            public int BandNo { get; set; }

            public byte ByData { get; set; }
        }


        // Post: api/KarbordData/DocAttach پیوست  
        [Route("api/KarbordData/DocAttach/")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PostWeb_DocAttach(DocAttachObject DocAttachObject)
        {
            string sql = string.Format(CultureInfo.InvariantCulture,
                                       @"EXEC [dbo].[Web_DocAttach]
                                              @ProgName = N'{0}',
                                              @Group = N'{1}',
                                              @Year = N'{2}',
                                              @DMode = N'{3}',
                                              @SerialNumber = {4},
                                              @BandNo = {5},
                                              @ByData = {6}",
                                              DocAttachObject.ProgName,
                                              DocAttachObject.Group,
                                              DocAttachObject.Year,
                                              //UnitPublic.sql_Group_Ticket,
                                              //"0000",
                                              DocAttachObject.ModeCode,
                                              DocAttachObject.SerialNumber,
                                              DocAttachObject.BandNo,
                                              DocAttachObject.ByData);
            var dataAccount = UnitDatabase.ReadUserPassHeader(this.Request.Headers);
            var list = db.Database.SqlQuery<Web_DocAttach>(sql);
            return Ok(list);

        }


        /*     public class Web_DocAttach_Del
             {

                 public string ProgName { get; set; }

                 public string ModeCode { get; set; }

                 public long SerialNumber { get; set; }

                 public int BandNo { get; set; }

             }

             // POST: api/KarbordData/ErjDocAttach_Del
             [Route("api/KarbordData/ErjDocAttach_Del")]
             public async Task<IHttpActionResult> PostErjDocAttach_Del(Web_DocAttach_Del Web_DocAttach_Del)
             {
                 string sql = string.Format(CultureInfo.InvariantCulture,
                               @"DECLARE	@return_value int
                                 EXEC	@return_value = [dbo].[Web_DocAttach_Del]
                                         @ProgName = '{0}',
                                         @ModeCode = '{1}',
                                         @SerialNumber = {2},
                                         @BandNo = {3}
                                 SELECT	'Return Value' = @return_value",
                            Web_DocAttach_Del.ProgName,
                            Web_DocAttach_Del.ModeCode,
                            Web_DocAttach_Del.SerialNumber,
                            Web_DocAttach_Del.BandNo
                            );
                 var dataAccount = UnitDatabase.ReadUserPassHeader(this.Request.Headers);
                 KarbordModel db = new KarbordModel(UnitPublic.ConnectionString_Ticket);
                 var list = db.Database.SqlQuery<int>(sql).Single();
                 await db.SaveChangesAsync();
                 return Ok(list);
             }

         */

        [Route("api/KarbordData/UploadFile")]
        public async Task<IHttpActionResult> UploadFile()
        {
            string SerialNumber = HttpContext.Current.Request["SerialNumber"];
            string ProgName = HttpContext.Current.Request["ProgName"];
            string ModeCode = HttpContext.Current.Request["ModeCode"];
            string BandNo = HttpContext.Current.Request["BandNo"];
            string Code = HttpContext.Current.Request["Code"];
            string Comm = HttpContext.Current.Request["Comm"];
            string FName = HttpContext.Current.Request["FName"];
            var Atch = System.Web.HttpContext.Current.Request.Files["Atch"];

            int lenght = Atch.ContentLength;
            byte[] filebyte = new byte[lenght];
            Atch.InputStream.Read(filebyte, 0, lenght);


            var dataAccount = UnitDatabase.ReadUserPassHeader(this.Request.Headers);

            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["KarbordComputer_SupportModel"].ConnectionString;

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            SqlCommand cmd = new SqlCommand("Web_DocAttach_Save", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ProgName", ProgName);
            cmd.Parameters.AddWithValue("@ModeCode", ModeCode);
            cmd.Parameters.AddWithValue("@SerialNumber", SerialNumber);
            cmd.Parameters.AddWithValue("@BandNo", BandNo);
            cmd.Parameters.AddWithValue("@Code", Code);
            cmd.Parameters.AddWithValue("@Comm", Comm);
            cmd.Parameters.AddWithValue("@FName", FName);
            cmd.Parameters.AddWithValue("@Atch", filebyte);

            cmd.ExecuteNonQuery();
            connection.Close();
            return Ok(1);
        }



        public class CustAccountObject
        {
            public string LockNo { get; set; }

            public bool FlagLog { get; set; }

            public string IP { get; set; }
            public string CallProg { get; set; }

        }

        public class CustAccount
        {
            public byte TasviyeCode { get; set; }

            public string Tasviye { get; set; }

            public string ModeCode { get; set; }

            public string Status { get; set; }

            public long SerialNumber { get; set; }

            public string DocNo { get; set; }

            public double? SortDocNo { get; set; }

            public string DocDate { get; set; }

            public byte? PaymentType { get; set; }

            public string PaymentTypeSt { get; set; }

            public string CustCode { get; set; }

            public string Spec { get; set; }

            public double? TotalValue { get; set; }

            public string DownloadCount { get; set; }

            public string Year { get; set; }

            public int AttachLen { get; set; }

        }

        [Route("api/KarbordData/CustAccount")]
        [ResponseType(typeof(CustAccount))]
        public async Task<IHttpActionResult> PostWeb_CustAccount(CustAccountObject CustAccountObject)
        {
            string sql = string.Format(CultureInfo.InvariantCulture, @"EXEC [dbo].[Web_CustAccount] @LockNo = '{0}' ", CustAccountObject.LockNo);

            var dataAccount = UnitDatabase.ReadUserPassHeader(this.Request.Headers);
            var list = db.Database.SqlQuery<CustAccount>(sql);
            if (CustAccountObject.FlagLog == true)
            {
                UnitPublic.SaveLog(Int32.Parse(CustAccountObject.LockNo), mode_CustAccount, act_View, 0, CustAccountObject.IP, CustAccountObject.CallProg, "");
            }
            return Ok(list);
        }


        public class CustAccountSaveObject
        {
            public int LockNumber { get; set; }

            public string Year { get; set; }

            public long SerialNumber { get; set; }

            public string OnlineParLink { get; set; }

            public string DownloadCount { get; set; }
        }


        [Route("api/KarbordData/CustAccountSave")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PostWeb_CustAccountSave(CustAccountSaveObject CustAccountSaveObject)
        {
            string sql = string.Format(CultureInfo.InvariantCulture, @"DECLARE @return_value int
                                                                       EXEC    @return_value = [dbo].[Web_CustAccountSave]
                                                                               @Year = N'{0}',
                                                                               @SerialNumber = {1}, ",
                                                                       CustAccountSaveObject.Year, CustAccountSaveObject.SerialNumber);


            if (CustAccountSaveObject.OnlineParLink != null)
                sql += string.Format(CultureInfo.InvariantCulture, " @OnlineParLink = N'''{0}''' ", CustAccountSaveObject.OnlineParLink);

            if (CustAccountSaveObject.DownloadCount != null)
                sql += string.Format(CultureInfo.InvariantCulture, " @DownloadCount = N'''{0}''' ", CustAccountSaveObject.DownloadCount);

            sql += " SELECT  'Return Value' = @return_value";

            var list = db.Database.SqlQuery<int>(sql);
            return Ok(list);
        }



        public class FDocP_CustAcountObject
        {
            public int LockNumber { get; set; }

            public string Year { get; set; }

            public long SerialNumber { get; set; }

            public string IP { get; set; }
            public string CallProg { get; set; }
        }


        [Route("api/KarbordData/FDocP_CustAcount")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PostWeb_FDocP_CustAcount(FDocP_CustAcountObject FDocP_CustAcountObject)
        {
            string sql = string.Format(@"EXEC  [dbo].[Web_FDocP]
		                                       @Year = N'{0}',
		                                       @SerialNumber = {1}",
                                       FDocP_CustAcountObject.Year,
                                       FDocP_CustAcountObject.SerialNumber
                          );

            var dataAccount = UnitDatabase.ReadUserPassHeader(this.Request.Headers);

            var list = db.Database.SqlQuery<Web_FDocP>(sql);
            UnitPublic.SaveLog(FDocP_CustAcountObject.LockNumber, mode_CustAccount, act_Print, FDocP_CustAcountObject.SerialNumber, FDocP_CustAcountObject.IP, FDocP_CustAcountObject.CallProg, "");
            return Ok(list);
        }


        public class DownloadContractObject
        {
            public int LockNumber { get; set; }

            public string Year { get; set; }

            public long SerialNumber { get; set; }

            public string IP { get; set; }

            public int BandNo { get; set; }

            public string CallProg { get; set; }
        }

        public class ContractAttach
        {

            public string FName { get; set; }

            public byte[] Atch { get; set; }

        }
        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                //TODO
            }
            finally
            {
                GC.Collect();
            }
        }

        [Route("api/KarbordData/DownloadContract")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Post_DownloadContract(DownloadContractObject DownloadContractObject)
        {

            string sql = string.Format(CultureInfo.InvariantCulture,
                                       @"EXEC [dbo].[Web_DocAttach]
                                              @ProgName = N'FCT5',
                                              @Year = N'{0}',
                                              @DMode = N'2',
                                              @SerialNumber = {1},
                                              @BandNo = {2},
                                              @ByData = 1",
                                              DownloadContractObject.Year,
                                              DownloadContractObject.SerialNumber,
                                              DownloadContractObject.BandNo
                                              );
            var list = db.Database.SqlQuery<Web_DocAttach>(sql);
            byte[] atch = list.First().Atch;
            string filename = list.First().FName;
            byte[] decompress = UnitPublic.Decompress(atch);

            var from = new MemoryStream(decompress);

            string path = UnitPublic.GetPath(45);
            string date = CustomPersianCalendar.ToPersianDate(DateTime.Now).Replace('/', '-');
            string ticket = string.Format("{0:yyyyMMddHHmmssfff}", CustomPersianCalendar.GetCurrentIRNow(false));

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string pathFile = path + "\\" + date;
            if (!Directory.Exists(pathFile))
                Directory.CreateDirectory(pathFile);

            string wordName = string.Format("{0}\\{1}_{2}.docx", pathFile, ticket, DownloadContractObject.LockNumber.ToString());
            string pdfName = string.Format("{0}\\{1}_{2}.pdf", pathFile, ticket, DownloadContractObject.LockNumber.ToString());

            Stream file = new FileStream(wordName, FileMode.Create, FileAccess.Write);
            from.WriteTo(file);
            file.Close();
            from.Close();
            object misValue = System.Reflection.Missing.Value;

            try
            {
                var Word = new Microsoft.Office.Interop.Word.Application();
                Microsoft.Office.Interop.Word.Document doc = Word.Documents.Open(wordName);
                doc.Activate();

                doc.SaveAs2(pdfName, WdSaveFormat.wdFormatPDF, misValue, misValue, misValue, misValue, misValue, misValue, misValue, misValue, misValue, misValue);
                doc.Close();
                Word.Quit();

                releaseObject(doc);
                releaseObject(Word);
            }
            catch (Exception a)
            {
                var ss = a.Message.ToString();
                throw;
            }

            var res = File.ReadAllBytes(pdfName);

            ContractAttach p = new ContractAttach()
            {
                FName = filename,
                Atch = res
            };

            return Ok(p);
        }




        public class EndChatObject
        {
            public int LockNumber { get; set; }

            public long SerialNumber { get; set; }

        }

        [Route("api/KarbordData/EndChat")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Post_EndChat(EndChatObject d)
        {
            string sql = string.Format(CultureInfo.InvariantCulture, @"EXEC [dbo].[Web_EndChat] @SerialNumber = {0} select 0 ", d.SerialNumber);
            var list = db.Database.SqlQuery<int>(sql);
            return Ok(list);
        }

        public class RepFromUsersObject
        {
            public string userCode { get; set; }

        }


        public partial class Web_RepFromUsers
        {
            public string Code { get; set; }

            public string Name { get; set; }

        }


        // Post: api/Web_Data/Web_RepFromUsers   ارجاع شونده/ارجاع دهنده
        [Route("api/KarbordData/Web_RepFromUsers")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PostWeb_RepFromUsers(RepFromUsersObject RepFromUsersObject)
        {
            string sql = string.Format(@"Select * from Web_RepFromUsers('{0}')", RepFromUsersObject.userCode);
            var list = db.Database.SqlQuery<Web_RepFromUsers>(sql);
            return Ok(list);
        }


        public class UpdateChatDownload_Object
        {
            public long SerialNumber { get; set; }

            public bool ChatDownload { get; set; }
        }

        // Post: api/KarbordData/UpdateChatDownload ارسال فایل توسط کاربر  
        [Route("api/KarbordData/UpdateChatDownload/")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PostUpdateChatDownload(UpdateChatDownload_Object c)
        {
            string sql = string.Format(CultureInfo.InvariantCulture, @"EXEC Web_ErjSaveTicket_UpdateChatDownload @SerialNumber = {0} ,@ChatDownload = {1} select 1 ", c.SerialNumber, c.ChatDownload);
            var dataAccount = UnitDatabase.ReadUserPassHeader(this.Request.Headers);
            var res = db.Database.SqlQuery<int>(sql);
            return Ok(res);
        }



        [Route("api/KarbordData/ChatQueue/{SerialNumber}")]
        public async Task<IHttpActionResult> GetChatQueue(long SerialNumber)
        {
            string sql = string.Format(@"DECLARE	@return_value int
                                         EXEC	@return_value = [dbo].[Web_ChatQueue]
		                                        @SerialNumber = {0}
                                         SELECT	 @return_value", SerialNumber);
            var list = db.Database.SqlQuery<int>(sql).Single();
            return Ok(list);
        }


    }
}
