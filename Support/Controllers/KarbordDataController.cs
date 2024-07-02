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
            var list = db.Database.SqlQuery<string>(sql);
            return Ok(list);
        }


        // GET: api/KarbordData/Time تاریخ سرور
        [Route("api/KarbordData/Time")]
        public async Task<IHttpActionResult> GetWeb_Time()
        {
            string sql = string.Format(@"select cast(Convert(Time(0), GetDate()) as nvarchar(8)) as Time");
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



        public class LoginObject
        {
            public string userName { get; set; }

            public string pass { get; set; }

            public string param1 { get; set; }

            public string param2 { get; set; }

        }


        public class LoginData
        {
            public int Value { get; set; }

            public string Name { get; set; }

            public string VstrCode { get; set; }

        }

        // Post: api/KarbordData/ اطلاعات لاگین   
        [Route("api/KarbordData/Login")]
        public async Task<IHttpActionResult> PostWeb_Login(LoginObject LoginObject)
        {
            if (LoginObject.pass == "null")
                LoginObject.pass = "";
            string sql = string.Format(@"DECLARE @return_value int,
                                                 @name nvarchar(100),
		                                         @vstrcode nvarchar(100)

                                         EXEC    @return_value = [dbo].[Web_Login]
                                                 @Code1 = '{0}',
		                                         @UserCode = N'{1}',
                                                 @Code2 = '{2}',
		                                         @Psw = N'{3}',
                                                 @Name = @name OUTPUT,
		                                         @vstrcode = @VstrCode OUTPUT
                                         SELECT  @return_value as Value, @Name as Name ,  @vstrcode as VstrCode",
                                         LoginObject.param1, LoginObject.userName, LoginObject.param2, LoginObject.pass);
            var list = db.Database.SqlQuery<LoginData>(sql).ToList();
            return Ok(list);
        }





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

            public bool ChatActive { get; set; }

            //public bool SendSms { get; set; }

            public bool DocRead { get; set; }

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
		                                    @ChatActive = {29},
		                                    @DocRead = {30},
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
                                           ErjSaveTicket_HI.ChatMode,
                                           ErjSaveTicket_HI.ChatActive,
                                           ErjSaveTicket_HI.DocRead
                                           );

            var dataAccount = UnitDatabase.ReadUserPassHeader(this.Request.Headers);
            var list = db.Database.SqlQuery<int>(sql).Single();
            await db.SaveChangesAsync();
            /* string resSend = "";
             if (ErjSaveTicket_HI.SendSms == true)
             {
                 string mess = "درخواست چت از " + ErjSaveTicket_HI.Motaghazi;
                 resSend = UnitPublic.Send_SorenaSms(ErjSaveTicket_HI.UserCode, "null", mess);
             }*/
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

            public int? top { get; set; }

            public string Status { get; set; }

        }


        [Route("api/KarbordData/Web_ErjDocXK")]
        public async Task<IHttpActionResult> PostWeb_ErjDocXK(Object_ErjDocXK Object_ErjDocXK)
        {
            string sql = "select ";

            if (Object_ErjDocXK.top != null)
                sql += " top (" + Object_ErjDocXK.top.ToString() + ") ";

            sql += string.Format(" * from dbo.Web_ErjDocXK({0},'{1}') where 1 = 1  ", Object_ErjDocXK.ModeCode, Object_ErjDocXK.LockNo);

            if (Object_ErjDocXK.SerialNumber > 0)
            {
                sql += " and SerialNumber = " + Object_ErjDocXK.SerialNumber.ToString();
            }

            if (Object_ErjDocXK.Status != null && Object_ErjDocXK.Status != "")
            {
                sql += string.Format(" and Status = '{0}'", Object_ErjDocXK.Status.ToString());
            }

            sql += " order by DocDate desc , SerialNumber desc";

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
                                              @Group = N'',
                                              @Year = N'{2}',
                                              @DMode = N'{3}',
                                              @SerialNumber = {4},
                                              @BandNo = {5},
                                              @ByData = {6}",
                                              DocAttachObject.ProgName,
                                              DocAttachObject.Group,
                                              DocAttachObject.Year,
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

            SqlCommand cmd = new SqlCommand("Web_ErjDocAttach_Save", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            //cmd.Parameters.AddWithValue("@ProgName_", ProgName);
            cmd.Parameters.AddWithValue("@ModeCode_", ModeCode);
            cmd.Parameters.AddWithValue("@SerialNumber_", SerialNumber);
            cmd.Parameters.AddWithValue("@BandNo_", BandNo);
            cmd.Parameters.AddWithValue("@Code_", Code);
            cmd.Parameters.AddWithValue("@Comm_", Comm);
            cmd.Parameters.AddWithValue("@FName_", FName);
            cmd.Parameters.AddWithValue("@Atch_", filebyte);

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
                                              @Group = N'',
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


        // Post: api/KarbordData/Web_RepFromUsers   ارجاع شونده/ارجاع دهنده
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






        public class Web_ErjStatus
        {
            public int OrderFld { get; set; }
            public string Status { get; set; }
        }

        [Route("api/KarbordData/ErjStatus/")]
        public async Task<IHttpActionResult> GetWeb_ErjStatus()
        {
            string sql = string.Format(@"Select * from Web_ErjStatus");
            var list = db.Database.SqlQuery<Web_ErjStatus>(sql);
            return Ok(list);
        }


        public class ErjUsersObject
        {
            public string userCode { get; set; }

            public long SerialNumber { get; set; }
        }


        public partial class Web_ErjUsers
        {
            public string Code { get; set; }

            public string Name { get; set; }

            public string Spec { get; set; }
        }


        // Post: api/KarbordData/Web_ErjUsers   ارجاع شونده/ارجاع دهنده
        [Route("api/KarbordData/Web_ErjUsers/")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PostWeb_ErjUsers(ErjUsersObject ErjUsersObject)
        {
            string sql = string.Format(@"Select * from Web_ErjUsers('{0}',{1}) order by SrchOrder Desc,Name Asc", ErjUsersObject.userCode, ErjUsersObject.SerialNumber);
            var list = db.Database.SqlQuery<Web_ErjUsers>(sql);
            return Ok(list);
        }


        public class DocXUsersObject
        {
            public int TrsId { get; set; }

            public string UserCode { get; set; }

        }


        public partial class Web_DocXUsers
        {
            public string Code { get; set; }

            public string Name { get; set; }

        }


        // Post: api/KarbordData/Web_DocXUsers   ارجاع شونده/ارجاع دهنده
        [Route("api/KarbordData/Web_DocXUsers/")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PostWeb_DocXUsers(DocXUsersObject DocXUsersObject)
        {
            string sql = string.Format(@"Select * from Web_DocXUsers({0},'{1}') order by Name ", DocXUsersObject.TrsId, DocXUsersObject.UserCode);
            var list = db.Database.SqlQuery<Web_DocXUsers>(sql);
            return Ok(list);
        }





        public class Web_ErjSaveTicket_BSave
        {
            public long SerialNumber { get; set; }

            public string Natijeh { get; set; }

            public string FromUserCode { get; set; }

            public string ToUserCode { get; set; }

            public string RjDate { get; set; }

            public string RjTime { get; set; }

            public string RjMhltDate { get; set; }

            public int BandNo { get; set; }

            public int SrMode { get; set; }

            public string RjStatus { get; set; }

            public int? FarayandCode { get; set; }

            public string RjHour { get; set; }

            //public string MessageSms { get; set; }

        }


        // POST: api/KarbordData/ErjSaveTicket_BSave
        [Route("api/KarbordData/ErjSaveTicket_BSave/")]
        public async Task<IHttpActionResult> PostErjSaveTicket_BSave(Web_ErjSaveTicket_BSave d)
        {
            string sql = string.Format(
                        @" DECLARE	@return_value int,
		                            @BandNo nvarchar(10)
                            EXEC	@return_value = [dbo].[Web_ErjSaveTicket_BSave]
		                            @SerialNumber = {0},
		                            @BandNo = {1} ,
		                            @Natijeh = N'{2}',
		                            @FromUserCode = N'{3}',
		                            @ToUserCode = N'{4}',
		                            @RjDate = N'{5}',
		                            @RjTime = {6},
		                            @RjMhltDate = N'{7}',
                                    @SrMode = {8},
                                    @RjStatus = '{9}',
                                    @FarayandCode = {10},
                                    @RjHour = N'{11}'
                            SELECT	@BandNo as N'@BandNo' ",
                        d.SerialNumber,
                        d.BandNo,
                        UnitPublic.ConvertTextWebToWin(d.Natijeh ?? ""),
                        d.FromUserCode,
                        d.ToUserCode,
                        d.RjDate,
                        d.RjTime,
                        d.RjMhltDate,
                        d.SrMode,
                        d.RjStatus,
                        d.FarayandCode ?? 0,
                        d.RjHour
                        );

            string list = db.Database.SqlQuery<string>(sql).Single();
            if (!string.IsNullOrEmpty(list))
            {
                await db.SaveChangesAsync();
            }

            //string mess = string.Format("{0} درخواست چت از", d.ToUserCode);
            //string resSend = UnitPublic.Send_SorenaSms(d.ToUserCode, "null", d.MessageSms);

            return Ok(list);
        }


        public class Web_ErjDocXErja
        {
            public long SerialNumber { get; set; }

            public int? BandNo { get; set; }

            public int? DocBMode { get; set; }

            public string RjComm { get; set; }

            public string RjDate { get; set; }

            public string RjStatus { get; set; }

            public string RjTimeSt { get; set; }

            public string FromUserCode { get; set; }

            public string FromUserName { get; set; }

            public string ToUserCode { get; set; }

            public string ToUserName { get; set; }

            public string RjReadSt { get; set; }

            public string RooneveshtUsers { get; set; }

            public string FarayandName { get; set; }

            public string RjHour { get; set; }
        }

        public class ErjDocXErja
        {
            public long SerialNumber { get; set; }
        }

        // Post: api/KarbordData/Web_ErjDocXErja  ریز ارجاعات تیکت ها  
        [Route("api/KarbordData/Web_ErjDocXErja/")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PostWeb_ErjDocXErja(ErjDocXErja ErjDocXErja)
        {
            string sql = string.Format(CultureInfo.InvariantCulture,
                         @"select top (10000) * FROM  Web_ErjDocXErja({0}) AS ErjDocErja where 1 = 1 order by BandNo,DocBMode "
                         , ErjDocXErja.SerialNumber);

            var list = db.Database.SqlQuery<Web_ErjDocXErja>(sql);
            return Ok(list);
        }




        public class ErjDocHObject
        {
            public byte Mode { get; set; }

            public string UserCode { get; set; }

            public int select { get; set; }

            public bool accessSanad { get; set; }

            public string Sal { get; set; }

            public string Status { get; set; }

            public string DocNo { get; set; }

            public string Sort { get; set; }

            public string ModeSort { get; set; }

        }


        // Post: api/KarbordData/ErjDocH  فهرست پرونده ها  
        [Route("api/KarbordData/ErjDocH")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PostWeb_ErjDocH(ErjDocHObject ErjDocHObject)
        {
            var dataAccount = UnitDatabase.ReadUserPassHeader(this.Request.Headers);

            string sql = string.Format(CultureInfo.InvariantCulture,
                            @"declare @Sal nvarchar(10) = '{0}'
                               declare @Status nvarchar(30) = N'{1}'
                               declare @DocNo nvarchar(50) = '{2}' ",
                             ErjDocHObject.Sal,
                             ErjDocHObject.Status,
                             ErjDocHObject.DocNo);

            sql += "select ";
            if (ErjDocHObject.select == 0)
                sql += " top(100) ";

            sql += string.Format(CultureInfo.InvariantCulture,
                        @" * FROM  Web_ErjDocH_F({0},'{1}') AS ErjDocH where 
                              (@sal = ''  or substring(docdate, 1, 4) = @Sal) and
                              (@Status = ''  or Status = @Status) and
                              (@DocNo = ''  or DocNo = @DocNo) ",
                          ErjDocHObject.Mode, dataAccount[2]);
            if (ErjDocHObject.accessSanad == false)
                sql += " and Eghdam = '" + ErjDocHObject.UserCode + "' ";

            sql += " order by ";

            if (ErjDocHObject.Sort == "" || ErjDocHObject.Sort == null)
            {
                ErjDocHObject.Sort = "DocDate Desc,DocNo Desc";
            }
            else if (ErjDocHObject.Sort == "DocDate")
            {
                if (ErjDocHObject.ModeSort == "ASC")
                    ErjDocHObject.Sort = "DocDate Asc,DocNo Asc";
                else
                    ErjDocHObject.Sort = "DocDate Desc,DocNo Desc";
            }
            else if (ErjDocHObject.Sort == "Status")
            {
                if (ErjDocHObject.ModeSort == "ASC")
                    ErjDocHObject.Sort = "Status Asc, DocDate Asc,DocNo Asc";
                else
                    ErjDocHObject.Sort = "Status Desc, DocDate Desc,DocNo Desc";
            }
            else
            {
                ErjDocHObject.Sort = ErjDocHObject.Sort + " " + ErjDocHObject.ModeSort;
            }

            sql += ErjDocHObject.Sort;


            var list = db.Database.SqlQuery<Web_ErjDocH>(sql);
            return Ok(list);

        }



        public class Web_ErjDocErja
        {
            public long SerialNumber { get; set; }

            public int? BandNo { get; set; }

            public int? DocBMode { get; set; }

            public string RjComm { get; set; }

            public string RjDate { get; set; }

            public string RjStatus { get; set; }

            public string RjTimeSt { get; set; }

            public string FromUserCode { get; set; }

            public string FromUserName { get; set; }

            public string ToUserCode { get; set; }

            public string ToUserName { get; set; }

            public string RjReadSt { get; set; }

            public string RooneveshtUsers { get; set; }

            public string FarayandName { get; set; }

            public string RjHour { get; set; }
        }

        public class ErjDocErja
        {
            public long SerialNumber { get; set; }
        }

        // Post: api/KarbordData/ErjDocErja  ریز ارجاعات  
        [Route("api/KarbordData/ErjDocErja")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PostWeb_ErjDocErja(ErjDocErja ErjDocErja)
        {
            string sql = string.Format(CultureInfo.InvariantCulture,
                         @"select top (10000)  * FROM  Web_ErjDocErja({0}) AS ErjDocErja where 1 = 1 order by BandNo,DocBMode "
                         , ErjDocErja.SerialNumber);
            var list = db.Database.SqlQuery<Web_ErjDocErja>(sql);
            return Ok(list);

        }

        public class ErjResultObject
        {
            public string SerialNumber { get; set; }

            public string DocBMode { get; set; }

            public string ToUserCode { get; set; }

            public int? BandNo { get; set; }
        }


        public partial class Web_ErjResult
        {
            public int DocBMode { get; set; }

            public string ToUserCode { get; set; }

            public string ToUserName { get; set; }

            public long SerialNumber { get; set; }

            public int? BandNo { get; set; }

            public string RjResult { get; set; }
        }

        // Post: api/KarbordData/Web_ErjResult   نتیجه در اتوماسیون
        [Route("api/KarbordData/ErjResult")]
        public async Task<IHttpActionResult> PostWeb_ErjResult(ErjResultObject ErjResultObject)
        {
            string sql = string.Format(@"Select * from Web_ErjResult where SerialNumber = {0}", ErjResultObject.SerialNumber);

            if (ErjResultObject.BandNo != null)
            {

                sql += string.Format(@" and  BandNo = {0} ", ErjResultObject.BandNo);
            }

            if (ErjResultObject.DocBMode != null)
                sql += string.Format(@" and  DocBMode = {0} and ToUserCode = '{1}'",
                     ErjResultObject.DocBMode,
                     ErjResultObject.DocBMode == "0" ? "" : ErjResultObject.ToUserCode
                    );

            var list = db.Database.SqlQuery<Web_ErjResult>(sql);
            return Ok(list);

        }




        public class ErjCustObject
        {
            public string userCode { get; set; }

            public byte Mode { get; set; }
        }


        public partial class Web_ErjCust
        {
            public string Code { get; set; }

            public string Name { get; set; }

            public string Spec { get; set; }
        }


        //  Post: api/KarbordData/ErjCust لیست مشتریان ارجاعات

        [Route("api/KarbordData/ErjCust")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PostErjCust(ErjCustObject ErjCustObject)
        {
            string sql = string.Format(@"Select code,name,spec from Web_ErjCust_F({0},'{1}')",
                   ErjCustObject.Mode, ErjCustObject.userCode);

            var dataAccount = UnitDatabase.ReadUserPassHeader(this.Request.Headers);
            var list = db.Database.SqlQuery<Web_ErjCust>(sql);
            return Ok(list);
        }

        public partial class Web_Khdt
        {

            public int Code { get; set; }

            public string Name { get; set; }

            public string Spec { get; set; }

            public byte HasTime { get; set; }
        }


        //  GET: api/KarbordData/Khdt

        [Route("api/KarbordData/Khdt")]
        public async Task<IHttpActionResult> GetWeb_Khdt()
        {
            string sql = string.Format(@"Select * from Web_Khdt");

            var dataAccount = UnitDatabase.ReadUserPassHeader(this.Request.Headers);
            var list = db.Database.SqlQuery<Web_Khdt>(sql);
            return Ok(list);
        }


        public partial class Web_Mahramaneh
        {
            public int Code { get; set; }

            public string Name { get; set; }
        }

        // GET: api/KarbordData/Mahramaneh   محرمانه یا نوع در اتوماسیون
        [Route("api/KarbordData/Mahramaneh")]
        public async Task<IHttpActionResult> GetWeb_Mahramaneh()
        {
            var dataAccount = UnitDatabase.ReadUserPassHeader(this.Request.Headers);
            string sql = string.Format(@"Select * from Web_Mahramaneh('{0}')", dataAccount[2]);
            var list = db.Database.SqlQuery<Web_Mahramaneh>(sql);
            return Ok(list);
        }







        public class Web_RjStatus
        {
            public string Name { get; set; }
        }

        // GET: api/KarbordData/RjStatus لیست وضعیت ارجاع  
        [Route("api/KarbordData/RjStatus")]
        public async Task<IHttpActionResult> GetWeb_RjStatus()
        {
            string sql = string.Format(@"Select * from Web_RjStatus");
            var dataAccount = UnitDatabase.ReadUserPassHeader(this.Request.Headers);
            var list = db.Database.SqlQuery<Web_RjStatus>(sql);
            return Ok(list);
        }






        public class ErjDocB_Last
        {
            public string erjaMode { get; set; }

            public string docBMode { get; set; }

            public string fromUserCode { get; set; }

            public string toUserCode { get; set; }

            public string srchSt { get; set; }

            public string azDocDate { get; set; }

            public string taDocDate { get; set; }

            public string azRjDate { get; set; }

            public string taRjDate { get; set; }

            public string azMhltDate { get; set; }

            public string taMhltDate { get; set; }

            public string status { get; set; }

            public string custCode { get; set; }

            public string khdtCode { get; set; }
        }

        // Post: api/KarbordData/ErjDocB_Last گزارش فهرست ارجاعات  
        [Route("api/KarbordData/ErjDocB_Last")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PostWeb_ErjDocB_Last(ErjDocB_Last ErjDocB_Last)
        {
            var dataAccount = UnitDatabase.ReadUserPassHeader(this.Request.Headers);
            string sql = string.Format(CultureInfo.InvariantCulture,
                        @"select  top (10000) * FROM  Web_ErjDocB_Last({0}, {1},'{2}','{3}','{4}','{5}') AS ErjDocB_Last where 1 = 1 "
                        , ErjDocB_Last.erjaMode
                        , ErjDocB_Last.docBMode
                        , ErjDocB_Last.fromUserCode
                        , ErjDocB_Last.toUserCode
                        , ErjDocB_Last.srchSt
                        , dataAccount[2]);

            if (ErjDocB_Last.azDocDate != "")
                sql += string.Format(" and DocDate >= '{0}' ", ErjDocB_Last.azDocDate);

            if (ErjDocB_Last.taDocDate != "")
                sql += string.Format(" and DocDate <= '{0}' ", ErjDocB_Last.taDocDate);

            if (ErjDocB_Last.azRjDate != "")
                sql += string.Format(" and RjDate >= '{0}' ", ErjDocB_Last.azRjDate);

            if (ErjDocB_Last.taRjDate != "")
                sql += string.Format(" and RjDate <= '{0}' ", ErjDocB_Last.taRjDate);

            if (ErjDocB_Last.azMhltDate != "")
                sql += string.Format(" and MhltDate >= '{0}' ", ErjDocB_Last.azMhltDate);

            if (ErjDocB_Last.taMhltDate != "")
                sql += string.Format(" and MhltDate <= '{0}' ", ErjDocB_Last.taMhltDate);

            if (ErjDocB_Last.status != "")
                sql += string.Format(" and Status = '{0}' ", ErjDocB_Last.status);


            sql += UnitPublic.SpiltCodeAnd("KhdtCode", ErjDocB_Last.khdtCode);
            sql += UnitPublic.SpiltCodeAnd("CustCode", ErjDocB_Last.custCode);

            if (ErjDocB_Last.erjaMode == "1")
                sql += "order by SortRjStatus";
            else
                sql += "order by SortRjDate";


            var list = db.Database.SqlQuery<Web_ErjDocB_Last>(sql);
            return Ok(list);
        }




        public class ErjDocKObject
        {
            public string userName { get; set; }

            public string userMode { get; set; }

            public string azTarikh { get; set; }

            public string taTarikh { get; set; }

            public string Status { get; set; }

            public string CustCode { get; set; }

            public string KhdtCode { get; set; }

            public string SrchSt { get; set; } // جستجو برای

            public long SerialNumber { get; set; }

        }


        // Post: api/KarbordData/ErjDocK گزارش فهرست پرونده ها  
        [Route("api/KarbordData/ErjDocK")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PostWeb_ErjDocK(ErjDocKObject ErjDocKObject)
        {
            var dataAccount = UnitDatabase.ReadUserPassHeader(this.Request.Headers);
            string sql = string.Format(CultureInfo.InvariantCulture,
                          @"select top (10000)  * FROM  Web_ErjDocK('{0}','{1}') AS ErjDocK where 1 = 1 and ShowDocTrs = 1 ",
                          ErjDocKObject.SrchSt, dataAccount[2]);

            if (ErjDocKObject.azTarikh != "")
                sql += string.Format(" and DocDate >= '{0}' ", ErjDocKObject.azTarikh);

            if (ErjDocKObject.taTarikh != "")
                sql += string.Format(" and DocDate <= '{0}' ", ErjDocKObject.taTarikh);


            if (ErjDocKObject.Status != "")
                sql += string.Format(" and Status = '{0}' ", ErjDocKObject.Status);


            sql += UnitPublic.SpiltCodeAnd("CustCode", ErjDocKObject.CustCode);
            sql += UnitPublic.SpiltCodeAnd("KhdtCode", ErjDocKObject.KhdtCode);

            if (ErjDocKObject.SerialNumber > 0)
                sql += " and SerialNumber = " + ErjDocKObject.SerialNumber;


            var list = db.Database.SqlQuery<Web_ErjDocK>(sql);
            return Ok(list);

        }









        public class Object_ErjDocXH
        {
            public long SerialNumber { get; set; }

            public int ModeCode { get; set; }

            public string UserCode { get; set; }

            public string IP { get; set; }

            public string CallProg { get; set; }

            public bool LoginLink { get; set; }

            public int? top { get; set; }

            public string Status { get; set; }

            public int? ChatMode { get; set; }

        }


        [Route("api/KarbordData/Web_ErjDocXH")]
        public async Task<IHttpActionResult> PostWeb_ErjDocXH(Object_ErjDocXH Object_ErjDocXH)
        {
            string sql = "select ";

            if (Object_ErjDocXH.top != null)
                sql += " top (" + Object_ErjDocXH.top.ToString() + ") ";

            sql += string.Format(@" *
                                   from dbo.Web_ErjDocXH_F({0},'{1}') where 1 = 1  ",
                                   Object_ErjDocXH.ModeCode,
                                   Object_ErjDocXH.UserCode);

            if (Object_ErjDocXH.SerialNumber > 0)
            {
                sql += " and SerialNumber = " + Object_ErjDocXH.SerialNumber.ToString();
            }

            if (Object_ErjDocXH.ChatMode != null)
            {
                sql += " and ChatMode = " + Object_ErjDocXH.ChatMode.ToString();
            }

            if (Object_ErjDocXH.Status != null && Object_ErjDocXH.Status != "")
            {
                sql += string.Format(" and Status = '{0}'", Object_ErjDocXH.Status.ToString());
            }

            sql += " order by DocDate desc , SerialNumber desc";

            var list = db.Database.SqlQuery<Web_ErjDocXH_F>(sql);
            return Ok(list);
        }




        public class ErjDocXB_Last
        {
            public string erjaMode { get; set; }

            public string docBMode { get; set; }

            public string fromUserCode { get; set; }

            public string toUserCode { get; set; }

            public string srchSt { get; set; }

            public string azDocDate { get; set; }

            public string taDocDate { get; set; }

            public string azRjDate { get; set; }

            public string taRjDate { get; set; }

            public string azMhltDate { get; set; }

            public string taMhltDate { get; set; }

            public string status { get; set; }

            public string custCode { get; set; }

            public string khdtCode { get; set; }
        }

        // Post: api/KarbordData/ErjDocXB_Last گزارش فهرست ارجاعات  
        [Route("api/KarbordData/ErjDocXB_Last")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PostWeb_ErjDocXB_Last(ErjDocXB_Last ErjDocXB_Last)
        {
            var dataAccount = UnitDatabase.ReadUserPassHeader(this.Request.Headers);
            string sql = string.Format(CultureInfo.InvariantCulture,
                        @"select  top (10000) * FROM  Web_ErjDocXB_Last({0}, {1},'{2}','{3}','{4}','{5}') AS ErjDocXB_Last where 1 = 1 "
                        , ErjDocXB_Last.erjaMode
                        , ErjDocXB_Last.docBMode
                        , ErjDocXB_Last.fromUserCode
                        , ErjDocXB_Last.toUserCode
                        , ErjDocXB_Last.srchSt
                        , dataAccount[2]);

            if (ErjDocXB_Last.azDocDate != "")
                sql += string.Format(" and DocDate >= '{0}' ", ErjDocXB_Last.azDocDate);

            if (ErjDocXB_Last.taDocDate != "")
                sql += string.Format(" and DocDate <= '{0}' ", ErjDocXB_Last.taDocDate);

            if (ErjDocXB_Last.azRjDate != "")
                sql += string.Format(" and RjDate >= '{0}' ", ErjDocXB_Last.azRjDate);

            if (ErjDocXB_Last.taRjDate != "")
                sql += string.Format(" and RjDate <= '{0}' ", ErjDocXB_Last.taRjDate);

            if (ErjDocXB_Last.azMhltDate != "")
                sql += string.Format(" and MhltDate >= '{0}' ", ErjDocXB_Last.azMhltDate);

            if (ErjDocXB_Last.taMhltDate != "")
                sql += string.Format(" and MhltDate <= '{0}' ", ErjDocXB_Last.taMhltDate);

            if (ErjDocXB_Last.status != "")
                sql += string.Format(" and Status = '{0}' ", ErjDocXB_Last.status);


            sql += UnitPublic.SpiltCodeAnd("KhdtCode", ErjDocXB_Last.khdtCode);
            sql += UnitPublic.SpiltCodeAnd("CustCode", ErjDocXB_Last.custCode);

            if (ErjDocXB_Last.erjaMode == "1")
                sql += "order by SortRjStatus";
            else
                sql += "order by SortRjDate";


            var list = db.Database.SqlQuery<Web_ErjDocXB_Last>(sql);
            return Ok(list);
        }


        public class ErjXResultObject
        {
            public string SerialNumber { get; set; }

            public string DocBMode { get; set; }

            public string ToUserCode { get; set; }

            public int? BandNo { get; set; }
        }


        public partial class Web_ErjXResult
        {
            public int DocBMode { get; set; }

            public string ToUserCode { get; set; }

            public string ToUserName { get; set; }

            public long SerialNumber { get; set; }

            public int? BandNo { get; set; }

            public string RjResult { get; set; }
        }

        // Post: api/KarbordData/Web_ErjXResult   نتیجه در اتوماسیون
        [Route("api/KarbordData/ErjXResult")]
        public async Task<IHttpActionResult> PostWeb_ErjXResult(ErjXResultObject ErjXResultObject)
        {
            string sql = string.Format(@"Select * from Web_ErjXResult where SerialNumber = {0}", ErjXResultObject.SerialNumber);

            if (ErjXResultObject.BandNo != null)
            {

                sql += string.Format(@" and  BandNo = {0} ", ErjXResultObject.BandNo);
            }

            if (ErjXResultObject.DocBMode != null)
                sql += string.Format(@" and  DocBMode = {0} and ToUserCode = '{1}'",
                     ErjXResultObject.DocBMode,
                     ErjXResultObject.DocBMode == "0" ? "" : ErjXResultObject.ToUserCode
                    );

            var list = db.Database.SqlQuery<Web_ErjXResult>(sql);
            return Ok(list);

        }





        public class ErjSaveTicket_DocReadObject
        {
            public long SerialNumber { get; set; }

            public string DocReadSt { get; set; }

        }

        // Post: api/KarbordData/ دیده شدن تیکت  
        [Route("api/KarbordData/ErjSaveTicket_DocRead")]
        public async Task<IHttpActionResult> PostWeb_ErjSaveTicket_DocRead(ErjSaveTicket_DocReadObject d)
        {
            string sql = string.Format(@"EXEC	[dbo].[Web_ErjSaveTicket_DocRead] @SerialNumber = {0}, @DocReadSt = N'{1}' select 0", d.SerialNumber, d.DocReadSt);
            var value = db.Database.SqlQuery<int>(sql).Single();
            return Ok(value);
        }



        public class ErjSaveTicket_RjReadObject
        {
            public long SerialNumber { get; set; }

            public string RjReadSt { get; set; }

            public int DocBMode { get; set; }

            public int BandNo { get; set; }

        }

        // Post: api/KarbordData/ دیده شدن ارجاع تیکت  
        [Route("api/KarbordData/ErjSaveTicket_RjRead")]
        public async Task<IHttpActionResult> PostWeb_ErjSaveTicket_RjRead(ErjSaveTicket_RjReadObject d)
        {
            string sql = string.Format(@"EXEC [dbo].[Web_ErjSaveTicket_RjRead] @DocBMode = {0},	@SerialNumber = {1},@BandNo = {2},@RjReadSt = N'{3}' select 0",
                                       d.DocBMode, d.SerialNumber, d.BandNo, d.RjReadSt);
            var value = db.Database.SqlQuery<int>(sql).Single();
            return Ok(value);
        }





        public class ErjDayRHObject
        {
            public byte Mode { get; set; }

            public string UserCode { get; set; }

            public string Status { get; set; }

            public string DocDate { get; set; }

            public string Eghdam { get; set; }

            public string Tanzim { get; set; }
        }


        // Post: api/KarbordData/Web_ErjDayRH   لیست گزارش کارها
        [Route("api/KarbordData/ErjDayRH")]
        public async Task<IHttpActionResult> PostErjDayRH(ErjDayRHObject d)
        {
            string sql = string.Format(@"Select * from Web_ErjDayRH_F({0},'{1}') where 1 = 1 ", d.Mode, d.UserCode);

            if (d.Status != "") sql += string.Format(@" and  Status = '{0}' ", d.Status);
            if (d.DocDate != "") sql += string.Format(@" and  DocDate = '{0}' ", d.DocDate);
            if (d.Eghdam != "") sql += string.Format(@" and  Eghdam = '{0}' ", d.Eghdam);
            if (d.Tanzim != "") sql += string.Format(@" and  Tanzim = '{0}' ", d.Tanzim);

            sql += " order by Docdate desc , eghdam";

            var list = db.Database.SqlQuery<DayRH>(sql);
            return Ok(list);

        }

        public class ErjDayRBObject
        {
            public long SerialNumber { get; set; }

        }

        // Post: api/KarbordData/Web_ErjDayRB   لیست گزارش کارها
        [Route("api/KarbordData/ErjDayRB")]
        public async Task<IHttpActionResult> PostErjDayRB(ErjDayRBObject d)
        {
            string sql = string.Format(@"Select * from Web_ErjDayRB_F() where SerialNumber = {0} ", d.SerialNumber);
            var list = db.Database.SqlQuery<DayRB>(sql);
            return Ok(list);

        }


        // دریافت اطلاعات سطح دسترسی کاربر
        public class AccessUser
        {
            public string OrgProgName { get; set; }

            public string TrsName { get; set; }
        }

        [Route("api/KarbordData/AccessUser/{progName}/{user}")]

        public async Task<IHttpActionResult> GetWeb_AccessUser(string progName, string user)
        {
            string sql = string.Format(@"EXEC [dbo].[Web_UserTrs] @ProgName = '{0}' , @GroupNo = 0, @UserCode = '{1}'", progName, user);
            var list = db.Database.SqlQuery<AccessUser>(sql).ToList();
            return Ok(list);
        }



        public class Web_ErjSaveDoc_RjRead
        {
            public int DocBMode { get; set; }

            public long SerialNumber { get; set; }

            public int BandNo { get; set; }

            public string RjReadSt { get; set; }
        }

        // POST: api/KarbordData/ErjSaveDoc_RjRead
        [Route("api/KarbordData/ErjSaveDoc_RjRead")]
        public async Task<IHttpActionResult> PostErjSaveDoc_RjRead(Web_ErjSaveDoc_RjRead Web_ErjSaveDoc_RjRead)
        {
            string sql = string.Format(CultureInfo.InvariantCulture,
                         @" DECLARE	@return_value int
                            EXEC	@return_value = [dbo].[Web_ErjSaveDoc_RjRead]
                                    @DocBMode = {0},		                            
                                    @SerialNumber = {1},
		                            @BandNo = {2},
		                            @RjReadSt = '{3}'
                            SELECT	'Return Value' = @return_value",
                        Web_ErjSaveDoc_RjRead.DocBMode,
                        Web_ErjSaveDoc_RjRead.SerialNumber,
                        Web_ErjSaveDoc_RjRead.BandNo,
                        Web_ErjSaveDoc_RjRead.RjReadSt
                        );


            var list = db.Database.SqlQuery<int>(sql).Single();
            await db.SaveChangesAsync();
            return Ok(list);
        }
        //با نام کاربر
        //http://localhost:52798/api/KarbordData/SendSmsChat/ace/null/bodymessage/4OClgAD-oIzeawIDNx86MvzfUjUlCURKy-4gjG1r3pI=   

        // با شماره موبایل
        //http://localhost:52798/api/KarbordData/SendSmsChat/null/09354963991/bodymessage/4OClgAD-oIzeawIDNx86MvzfUjUlCURKy-4gjG1r3pI=
        //ارسال sms زمان ارجاع چت
        [Route("api/KarbordData/SendSmsChat/{UserCode}/{Mobile}/{Message}/{Token}")]
        public async Task<IHttpActionResult> GetSendSmsChat(string UserCode, string Mobile, string Message, string Token)
        {
            long currentDate = DateTime.Now.Ticks;
            string resSend = "";
            var inputToken = UnitPublic.Decrypt(Token);
            var data = inputToken.Split('-');
            if (data.Length == 3)
            {
                string lockNumber = data[0];
                Int64 tik = Int64.Parse(data[2]);
                long elapsedTicks = currentDate - tik;
                TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);
#if (DEBUG)

                resSend = UnitPublic.Send_SorenaSms(UserCode, Mobile, Message);

#else
                if (elapsedSpan.TotalMinutes <= 1)
                {
                  resSend = UnitPublic.Send_SorenaSms(UserCode,Mobile, Message);
                }
#endif
            }

            return Ok(resSend.ToString());
        }


        public class SendMessageSorenaObject
        {
            public string UserCode { get; set; }

            public string Message { get; set; }
        }


        [Route("api/KarbordData/SendMessageSorena/")]
        public async Task<IHttpActionResult> Post_SendMessageSorena(SendMessageSorenaObject d)
        {
            string resSend = "";
            resSend = UnitPublic.Send_SorenaSms(d.UserCode, "null", d.Message);
            return Ok(resSend.ToString());
        }




        public class ErjSaveDoc_HUObject
        {
            public long SerialNumber { get; set; }

            public string Tanzim { get; set; }

            public string Status { get; set; }

            public string Spec { get; set; }

            public string DocDesc { get; set; }

            public string EghdamComm { get; set; }

            public string FinalComm { get; set; }

            public string SpecialComm { get; set; }

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

            public string UserCode { get; set; }
        }



        // POST: api/KarbordData/ErjSaveDoc_HU
        [Route("api/KarbordData/ErjSaveDoc_HU/")]
        public async Task<IHttpActionResult> PostErjSaveDoc_HU(ErjSaveDoc_HUObject ErjSaveDoc_HUObject)
        {
            int value = 0;
            try
            {
                string sql = string.Format(
                     @"DECLARE	@return_value int

                            EXEC	@return_value = [dbo].[Web_ErjSaveDoc_HU]
                                    @SerialNumber = {0},
		                            @UserCode = '{1}',
		                            @Tanzim = '{2}',
		                            @Status = '{3}',
		                            @Spec = N'{4}',
		                            @DocDesc = N'{5}',
		                            @EghdamComm = N'{6}',
		                            @FinalComm = N'{7}',
		                            @SpecialComm = N'{8}',
		                            @F01 = N'{9}',
		                            @F02 = N'{10}',
		                            @F03 = N'{11}',
		                            @F04 = N'{12}',
		                            @F05 = N'{13}',
		                            @F06 = N'{14}',
		                            @F07 = N'{15}',
		                            @F08 = N'{16}',
		                            @F09 = N'{17}',
		                            @F10 = N'{18}',
		                            @F11 = N'{19}',
		                            @F12 = N'{20}',
		                            @F13 = N'{21}',
		                            @F14 = N'{22}',
		                            @F15 = N'{23}',
		                            @F16 = N'{24}',
		                            @F17 = N'{25}',
		                            @F18 = N'{26}',
		                            @F19 = N'{27}',
		                            @F20 = N'{28}'
                            SELECT	'Return Value' = @return_value ",
                        ErjSaveDoc_HUObject.SerialNumber,
                        ErjSaveDoc_HUObject.UserCode,
                        ErjSaveDoc_HUObject.Tanzim,
                        ErjSaveDoc_HUObject.Status,
                        ErjSaveDoc_HUObject.Spec,
                        UnitPublic.ConvertTextWebToWin(ErjSaveDoc_HUObject.DocDesc ?? ""),
                        UnitPublic.ConvertTextWebToWin(ErjSaveDoc_HUObject.EghdamComm ?? ""),
                        UnitPublic.ConvertTextWebToWin(ErjSaveDoc_HUObject.FinalComm ?? ""),
                        UnitPublic.ConvertTextWebToWin(ErjSaveDoc_HUObject.SpecialComm ?? ""),
                        ErjSaveDoc_HUObject.F01,
                        ErjSaveDoc_HUObject.F02,
                        ErjSaveDoc_HUObject.F03,
                        ErjSaveDoc_HUObject.F04,
                        ErjSaveDoc_HUObject.F05,
                        ErjSaveDoc_HUObject.F06,
                        ErjSaveDoc_HUObject.F07,
                        ErjSaveDoc_HUObject.F08,
                        ErjSaveDoc_HUObject.F09,
                        ErjSaveDoc_HUObject.F10,
                        ErjSaveDoc_HUObject.F11,
                        ErjSaveDoc_HUObject.F12,
                        ErjSaveDoc_HUObject.F13,
                        ErjSaveDoc_HUObject.F14,
                        ErjSaveDoc_HUObject.F15,
                        ErjSaveDoc_HUObject.F16,
                        ErjSaveDoc_HUObject.F17,
                        ErjSaveDoc_HUObject.F18,
                        ErjSaveDoc_HUObject.F19,
                        ErjSaveDoc_HUObject.F20);
                value = db.Database.SqlQuery<int>(sql).Single();
                if (value > 0)
                {
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return Ok(value);
        }



        public class Web_ErjSaveDoc_HStatus
        {
            public long SerialNumber { get; set; }

            public string Status { get; set; }
        }


        // POST: api/KarbordData/ErjSaveDoc_HStatus
        [Route("api/KarbordData/ErjSaveDoc_HStatus")]
        public async Task<IHttpActionResult> PostErjSaveDoc_HStatus(Web_ErjSaveDoc_HStatus Web_ErjSaveDoc_HStatus)
        {
            string sql = string.Format(
                        @" DECLARE	@return_value int
                            EXEC	@return_value = [dbo].[Web_ErjSaveDoc_HStatus]
		                            @SerialNumber = {0},
		                            @Status = '{1}'
                            SELECT	'Return Value' = @return_value",
                        Web_ErjSaveDoc_HStatus.SerialNumber,
                        Web_ErjSaveDoc_HStatus.Status
                        );

            var list = db.Database.SqlQuery<int>(sql).Single();
            await db.SaveChangesAsync();
            return Ok(list);
        }


        public class Web_ErjSaveDoc_BSave
        {
            public long SerialNumber { get; set; }

            public string Natijeh { get; set; }

            public string FromUserCode { get; set; }

            public string ToUserCode { get; set; }

            public string RjDate { get; set; }

            public string RjTime { get; set; }

            public string RjMhltDate { get; set; }

            public int BandNo { get; set; }

            public int SrMode { get; set; }

            public string RjStatus { get; set; }

            public int? FarayandCode { get; set; }

            public string RjHour { get; set; }
        }


        // POST: api/KarbordData/ErjSaveDoc_BSave
        [Route("api/KarbordData/ErjSaveDoc_BSave")]
        public async Task<IHttpActionResult> PostErjSaveDoc_BSave(Web_ErjSaveDoc_BSave Web_ErjSaveDoc_BSave)
        {
            string sql = string.Format(
                        @" DECLARE	@return_value int,
		                            @BandNo nvarchar(10)
                            EXEC	@return_value = [dbo].[Web_ErjSaveDoc_BSave]
		                            @SerialNumber = {0},
		                            @BandNo = {1} ,
		                            @Natijeh = N'{2}',
		                            @FromUserCode = N'{3}',
		                            @ToUserCode = N'{4}',
		                            @RjDate = N'{5}',
		                            @RjTime = {6},
		                            @RjMhltDate = N'{7}',
                                    @SrMode = {8},
                                    @RjStatus = '{9}',
                                    @FarayandCode = {10},
                                    @RjHour = {11},
                            SELECT	@BandNo as N'@BandNo' ",
                        Web_ErjSaveDoc_BSave.SerialNumber,
                        Web_ErjSaveDoc_BSave.BandNo,
                        UnitPublic.ConvertTextWebToWin(Web_ErjSaveDoc_BSave.Natijeh ?? ""),
                        Web_ErjSaveDoc_BSave.FromUserCode,
                        Web_ErjSaveDoc_BSave.ToUserCode,
                        Web_ErjSaveDoc_BSave.RjDate,
                        Web_ErjSaveDoc_BSave.RjTime,
                        Web_ErjSaveDoc_BSave.RjMhltDate,
                        Web_ErjSaveDoc_BSave.SrMode,
                        Web_ErjSaveDoc_BSave.RjStatus,
                        Web_ErjSaveDoc_BSave.FarayandCode ?? 0,
                        Web_ErjSaveDoc_BSave.RjHour 
                        );


            string list = db.Database.SqlQuery<string>(sql).Single();
            if (!string.IsNullOrEmpty(list))
            {
                await db.SaveChangesAsync();
            }
            return Ok(list);
        }


        public class Web_ErjSaveDoc_CSave
        {
            public long SerialNumber { get; set; }

            public int BandNo { get; set; }

            public string Natijeh { get; set; }

            public string ToUserCode { get; set; }

            public string RjDate { get; set; }

            public string RjTime { get; set; }

            public string RjHour { get; set; }

        }

        // POST: api/KarbordData/ErjSaveDoc_CSave
        [Route("api/KarbordData/ErjSaveDoc_CSave")]
        public async Task<IHttpActionResult> PostErjSaveDoc_CSave([FromBody]List<Web_ErjSaveDoc_CSave> Web_ErjSaveDoc_CSave)
        {
            string value = "";
            string sql = "";


            foreach (var item in Web_ErjSaveDoc_CSave)
            {
                sql = string.Format(CultureInfo.InvariantCulture,
                     @" DECLARE	@return_value int,
                                @BandNo nvarchar(10)
                               EXEC	@return_value = [dbo].[Web_ErjSaveDoc_CSave]
		                            @SerialNumber = {0},
		                            @BandNo = {1},
		                            @Natijeh = N'{2}',
		                            @ToUserCode = N'{3}',
		                            @RjDate = N'{4}',
                                    @RjTime = {5},
                                    @RjHour = '{6}',
                               SELECT @BandNo as N'@BandNo'",
                    item.SerialNumber,
                    item.BandNo,
                    UnitPublic.ConvertTextWebToWin(item.Natijeh ?? ""),
                    item.ToUserCode,
                    item.RjDate,
                    item.RjTime,
                    item.RjHour);
                value = db.Database.SqlQuery<string>(sql).Single();
            }

            await db.SaveChangesAsync();
            if (!string.IsNullOrEmpty(value))
            {
                await db.SaveChangesAsync();
            }
            return Ok(value);
        }



        public class Web_ErjSaveDoc_Rooneveshts
        {
            public long SerialNumber { get; set; }

            //public string FromUserCode { get; set; }

            public string ToUserCodes { get; set; }

            public int BandNo { get; set; }

            // public string RjDate { get; set; }

        }

        // POST: api/KarbordData/ErjSaveDoc_Rooneveshts
        [Route("api/KarbordData/ErjSaveDoc_Rooneveshts")]
        public async Task<IHttpActionResult> PostErjSaveDoc_Rooneveshts(Web_ErjSaveDoc_Rooneveshts Web_ErjSaveDoc_Rooneveshts)
        {
            string sql = string.Format(CultureInfo.InvariantCulture,
                        @" DECLARE	@return_value int
                               EXEC	@return_value = [dbo].[Web_ErjSaveDoc_Rooneveshts]
		                            @SerialNumber = {0},
                                    @ToUserCodes = N'{1}',
                                    @BandNo = {2}
                           SELECT	'Return Value' = @return_value",
                       Web_ErjSaveDoc_Rooneveshts.SerialNumber,
                       Web_ErjSaveDoc_Rooneveshts.ToUserCodes,
                       Web_ErjSaveDoc_Rooneveshts.BandNo);


            var list = db.Database.SqlQuery<int>(sql).Single();
            await db.SaveChangesAsync();
            return Ok(list);
        }



        public class Web_ErjSaveDoc_CD
        {
            public long SerialNumber { get; set; }

            public int BandNo { get; set; }
        }

        // POST: api/KarbordData/ErjSaveDoc_CD
        [Route("api/KarbordData/ErjSaveDoc_CD")]
        public async Task<IHttpActionResult> PostErjSaveDoc_CD(Web_ErjSaveDoc_CD Web_ErjSaveDoc_CD)
        {
            string sql = string.Format(CultureInfo.InvariantCulture,
                 @" DECLARE	@return_value int,
                                        @BandNo nvarchar(10)
                               EXEC	@return_value = [dbo].[Web_ErjSaveDoc_CD]
		                            @SerialNumber = {0},
		                            @BandNo = {1}
                               SELECT	@BandNo as N'@BandNo'",
                Web_ErjSaveDoc_CD.SerialNumber,
                Web_ErjSaveDoc_CD.BandNo);


            var list = db.Database.SqlQuery<string>(sql).Single();
            await db.SaveChangesAsync();
            return Ok(list);
        }





    }
}
