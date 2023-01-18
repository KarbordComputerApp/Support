using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Support.Controllers.Unit;
using Support.Models;

namespace Support.Controllers.Unit
{
    public class UnitPublic
    {
        public static string titleVer = "ورژن تست";
        public static string titleVerNumber = "75";

        //public static string titleVer = "ورژن";
        //public static string titleVerNumber = "1001";

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




        public static void SaveLog(int lockNumber, int mode, int act, long serialNumber, string ip , string callprog)
        {
            SupportModel dbSupport = new SupportModel();
            string sql = string.Format(@"insert into Log_Data (lockNumber,idact,idmode,serialnumber,ip,callprog) values ({0},{1},{2},{3},'{4}','{5}') select 0",
                                         lockNumber, act, mode, serialNumber, ip, callprog);
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

    }
}