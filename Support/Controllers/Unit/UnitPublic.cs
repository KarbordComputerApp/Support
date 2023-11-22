using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Support.Controllers.Unit;
using Support.Models;
using System.IO.Compression;
using Ionic.Zip;

namespace Support.Controllers.Unit
{
    public class UnitPublic
    {
        public static string titleVer = "ورژن تست";
        public static string titleVerNumber = "92";

        //public static string titleVer = "ورژن";
        //public static string titleVerNumber = "1024";

        public static string Appddress; //ادرس نرم افزار
        public static IniFile MyIniServer;
        public static string apiAddress;
        public static char[] afiAccess;


        static string IniPath = HttpContext.Current.Server.MapPath("~/Content/ini/ServerConfig.Ini");

        static IniFile MyIni = new IniFile(IniPath);

        public static string sql_Name_Ticket = MyIni.Read("SqlServerName", "TicketDatabase");
        public static string sql_DatabaseName_Ticket = MyIni.Read("DatabaseName", "TicketDatabase");
        public static string sql_UserName_Ticket = MyIni.Read("SqlUserName", "TicketDatabase");
        public static string sql_Password_Ticket = MyIni.Read("SqlPassword", "TicketDatabase");

        public static string sql_Group_Ticket = sql_DatabaseName_Ticket.Substring(8, 2);

        public static string ConnectionString_Ticket = String.Format(@"data source = {0};initial catalog = {1};persist security info = True;user id = {2}; password = {3}; multipleactiveresultsets = True; application name = EntityFramework",
                                                       sql_Name_Ticket, sql_DatabaseName_Ticket, sql_UserName_Ticket, sql_Password_Ticket);

        public static string sql_Name_CustAccount = MyIni.Read("SqlServerName", "CustAccountDatabase");
        public static string sql_DatabaseName_CustAccount = MyIni.Read("DatabaseName", "CustAccountDatabase");
        public static string sql_UserName_CustAccount = MyIni.Read("SqlUserName", "CustAccountDatabase");
        public static string sql_Password_CustAccount = MyIni.Read("SqlPassword", "CustAccountDatabase");

        public static string sql_Group_CustAccount = sql_DatabaseName_CustAccount.Substring(8, 2);

        public static string ConnectionString_CustAccount = String.Format(@"data source = {0};initial catalog = {1};persist security info = True;user id = {2}; password = {3}; multipleactiveresultsets = True; application name = EntityFramework",
                                                           sql_Name_CustAccount, sql_DatabaseName_CustAccount, sql_UserName_CustAccount, sql_Password_CustAccount);

        public static string ConnectionString_Config = String.Format(@"data source = {0};initial catalog = {1};persist security info = True;user id = {2}; password = {3}; multipleactiveresultsets = True; application name = EntityFramework",
                                                       sql_Name_Ticket, "Ace_WebConfig", sql_UserName_Ticket, sql_Password_Ticket);




        public static void SaveLog(int lockNumber, int mode, int act, long serialNumber, string ip, string callprog, string spec)
        {
            SupportModel dbSupport = new SupportModel();
            string sql = string.Format(@"insert into Log_Data (lockNumber,idact,idmode,serialnumber,ip,callprog,spec) values ({0},{1},{2},{3},N'{4}',N'{5}',N'{6}') select 0",
                                         lockNumber, act, mode, serialNumber, ip, callprog, spec);
            int res = dbSupport.Database.SqlQuery<int>(sql).Single();
        }


        public class listDatabase
        {
            public string name { get; set; }
        }

        public static string MD5Hash(string itemToHash)
        {
            return string.Join("", MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(itemToHash)).Select(s => s.ToString("x2")));
        }

        public static string Encrypt(string str)
        {
            string EncrptKey = "2013;[pnuLIT)WebCodeExpertKarbordComputer";
            byte[] byKey = { };
            byte[] IV = { 18, 52, 86, 120, 144, 171, 205, 239 };
            byKey = System.Text.Encoding.UTF8.GetBytes(EncrptKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(str);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            var res = Convert.ToBase64String(ms.ToArray());
            res = res.Replace("/", "-");
            res = res.Replace("+", ";");
            return res;
        }

        public static string Decrypt(string str)
        {

            str = str.Replace("-", "/");
            str = str.Replace(" ", "-");
            str = str.Replace(";", "+");
            string DecryptKey = "2013;[pnuLIT)WebCodeExpertKarbordComputer";
            byte[] byKey = { };
            byte[] IV = { 18, 52, 86, 120, 144, 171, 205, 239 };
            byte[] inputByteArray = new byte[str.Length];

            byKey = System.Text.Encoding.UTF8.GetBytes(DecryptKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            inputByteArray = Convert.FromBase64String(str.Replace(" ", "+"));
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            System.Text.Encoding encoding = System.Text.Encoding.UTF8;
            return encoding.GetString(ms.ToArray());
        }


        public static bool TestEncrypt(string str)
        {

            DateTime centuryBegin = DateTime.Now;
            DateTime currentDate = DateTime.Now;

            long elapsedTicks = currentDate.Ticks - centuryBegin.Ticks;
            TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);

            var a = elapsedSpan.TotalSeconds;
            var b = elapsedSpan.TotalMinutes;


            var dateNow = DateTime.Now.Ticks.ToString();
            dateNow = DateTime.Now.Ticks.ToString();
            dateNow = DateTime.Now.Ticks.ToString();
            //var token = UnitPublic.Encrypt(TokenObject.LockNumber + "--" + dateNow);
            // var token2 = UnitPublic.Decrypt(token);

            return true;

        }


        public static string ConvertTextWebToWin(string text)
        {
            int i = 0;
            string data = "";
            string[] splitted = text.Split('\n');
            foreach (string substring in splitted)
            {
                i++;
                if (i <= splitted.Count() - 1)
                    data += substring + (char)(13) + (char)(10);
                else
                    data += substring;
            }
            return data;
        }

        public static string SendEmail(string to, string subject, string body)
        {
            string email = MyIni.Read("email", "SendMail");
            string host = MyIni.Read("host", "SendMail");
            int port = Convert.ToInt32(MyIni.Read("port", "SendMail"));
            string userName = MyIni.Read("userName", "SendMail");
            string pass = MyIni.Read("pass", "SendMail");
            bool enableSsl = MyIni.Read("enableSsl", "SendMail") == "true";
            bool useDefaultCredentials = MyIni.Read("UseDefaultCredentials", "SendMail") == "true";

            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(email);
                message.To.Add(new MailAddress(to));
                message.Subject = subject;
                message.SubjectEncoding = Encoding.UTF8;
                message.IsBodyHtml = true;

                string text =
                    "<table style=\"width: 40%;direction: rtl;text-align: right;font-family: tahoma;margin-left: 30%;margin-right: 30%;box-shadow: 1px 1px 10px 0px black;padding: 50px;\"><tbody><tr> <td style=\"text-align: center\">  <img src=\"http://185.208.174.64:8001/Content/img/LogoMail.png\" style=\"padding: 50px;width: 250px;\"></td></tr><tr><td style=\"color: #950003;padding-bottom: 50px;padding-top: 5px;font-family: tahoma;font-weight: bold;\">بازیابی رمز ورود</td></tr><tr><td>با سلام</td></tr><tr><td>شما درخواست بازیابی رمز عبور خود را ارسال کرده اید که برای ورود به پنل پشتیبایی باید پسورد زیر را وارد نمایید.</td></tr><tr><td style=\"padding-top: 30px;padding-bottom: 100px;font-weight: bold;\">رمز ورود جدید :"
                    + body +
                    "</td></tr><tr><td >با تشکر</td></tr><tr><td style=\"padding-bottom: 100px\">شرکت کاربرد کامپیوتر</td></tr><tr><td style=\"background-color:#b70002;color:white;text-align: center;padding: 20px\">این ایمیل توسط شرکت کاربرد کامپیوتر برای شما ارسال شده است در صورتی که در بازیابی رمز عبور به مشکل برخورد کردید لطفا با کارشناسان شرکت کاربرد کامپیوتر تماس حاصل فرمائید.</td></tr></tbody></table>";

                message.Body = text;
                message.BodyEncoding = Encoding.UTF8;

                smtp.Port = port;
                smtp.Host = host;
                smtp.EnableSsl = enableSsl;
                smtp.UseDefaultCredentials = useDefaultCredentials;
                smtp.Credentials = new NetworkCredential(userName, pass);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
                message.Dispose();
                return "Send";
            }
            catch (Exception e)
            {
                return e.Message.ToString();

            }
            /*
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(email);
                message.To.Add(new MailAddress(to));
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = body;
                smtp.Port = port;
                smtp.Host = host;
                smtp.EnableSsl = enableSsl;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(userName, pass);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception e)
            {
                var a = e.Message.ToString();
            }*/
        }

        public static byte[] Decompress(byte[] data)
        {
            var from = new MemoryStream(data);
            using (ZipFile zout = ZipFile.Read(from))
            {
                ZipEntry entry = zout.FirstOrDefault();
                MemoryStream zos = new MemoryStream();
                entry.Extract(zos);
                return zos.ToArray();
            }
        }

        public static string GetPath(int IdConfig)
        {
            SupportModel db = new SupportModel();
            string sql = string.Format(@"select value from Configs where id = {0}", IdConfig.ToString());
            string list = db.Database.SqlQuery<string>(sql).Single();
            return "C:" + list;
        }

        public static string HasContract(string lockNo)
        {
            KarbordModel dbConfig = new KarbordModel(UnitPublic.ConnectionString_Config);
            string sql = string.Format(@"DECLARE	@return_value int,
		                                            @EndDate nvarchar(10)

                                            EXEC	@return_value = [dbo].[Web_HasContract]
		                                            @LockNumber = N'{0}',
		                                            @EndDate = @EndDate OUTPUT
                                            SELECT	CONVERT(nvarchar, @return_value) +'-'+ @EndDate", lockNo);
            var list = dbConfig.Database.SqlQuery<string>(sql).Single();
            return list;
        }

    }
}