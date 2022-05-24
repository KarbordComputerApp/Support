using System;
using System.Globalization;
using System.IO;
using System.Net.Sockets;

namespace Support.Controllers.Unit
{
    public static class CustomPersianCalendar
    {
        public static string ToPersianDate(DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                string format = "{0}/{1:00}/{2:00}";
                PersianCalendar PC = new PersianCalendar();
                return string.Format(format, PC.GetYear(dateTime.Value), PC.GetMonth(dateTime.Value), PC.GetDayOfMonth(dateTime.Value));
            }
            else
                return string.Empty;
        }

        public static string ToPersianDateTime(DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                string format = "{0}/{1:00}/{2:00} {3}:{4}:{5}";
                PersianCalendar PC = new PersianCalendar();
                return string.Format(format, PC.GetYear(dateTime.Value), PC.GetMonth(dateTime.Value), PC.GetDayOfMonth(dateTime.Value), dateTime.Value.Hour, dateTime.Value.Minute, dateTime.Value.Second);
            }
            else
                return string.Empty;
        }

        public static DateTime ToDateTime(string persianTime)
        {
            string[] splited = persianTime.Split('/');
            return new PersianCalendar().ToDateTime(Convert.ToInt32(splited[0]), Convert.ToInt32(splited[1]), Convert.ToInt32(splited[2]), 0, 0, 0, 0);
        }

        public static DateTime? ToNullableDateTime(string persianTime)
        {
            persianTime = persianTime.Trim();
            if (string.IsNullOrEmpty(persianTime))
                return null;
            else
            {
                string[] splited = persianTime.Split('/');
                return new PersianCalendar().ToDateTime(Convert.ToInt32(splited[0]), Convert.ToInt32(splited[1]), Convert.ToInt32(splited[2]), 0, 0, 0, 0);
            }
        }

        public static DateTime GetCurrentIRNow(bool fromTimeServer)
        {
            return GetCurrentZoneNow(fromTimeServer, "Iran Standard Time");
        }

        public static DateTime GetCurrentZoneNow(bool fromTimeServer, string timeZoneId)
        {
            DateTime time;
            //Forotan Programer
            /*if (fromTimeServer)
            {
                time = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(GetTimeFromServer(true), "UTC", timeZoneId);
            }
            else
            {
                time = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, timeZoneId);
            }*/

            //Ehsanifar Change fromTimeServer True To False
            time = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, timeZoneId);

            return time;
        }

        public static DateTime GetCurrentIRToday(bool fromTimeServer)
        {
            DateTime time = GetCurrentIRNow(fromTimeServer);
            return new DateTime(time.Year, time.Month, time.Day, 0, 0, 0, DateTimeKind.Unspecified);
        }

        //        'Server IP addresses from 
        //'http://www.boulder.nist.gov/timefreq/service/time-servers.html
        private static String[] Servers = new string[] {
             "129.6.15.28",
             "129.6.15.29",
             "132.163.4.101",
             "132.163.4.102",
             "132.163.4.103",
             "128.138.140.44",
             "192.43.244.18",
             "131.107.1.10",
             "66.243.43.21",
             "216.200.93.8",
             "208.184.49.9",
             "207.126.98.204",
             "205.188.185.33" };


        private static DateTime LastUpdateTime = DateTime.MinValue;
        private static long? differentTicks = null;
        private static DateTime GetTimeFromServer(bool fromCache)
        {
            if (fromCache)
            {
                if ((DateTime.UtcNow - LastUpdateTime).TotalDays > 1)
                    differentTicks = null;

                if (differentTicks.HasValue)
                    return DateTime.UtcNow.AddTicks(differentTicks.Value);
            }
            //'Returns UTC/GMT using an NIST server if possible, 
            //' degrading to simply returning the system clock

            //'If we are successful in getting NIST time, then
            //' LastHost indicates which server was used and
            //' LastSysTime contains the system time of the call
            //' If LastSysTime is not within 15 seconds of NIST time,
            //'  the system clock may need to be reset
            //' If LastHost is "", time is equal to system clock

            DateTime result = DateTime.UtcNow;

            LastUpdateTime = result;

            foreach (string host in Servers)
            {
                try
                {
                    result = GetNISTTime(host);
                }
                catch
                {
                    result = DateTime.MinValue;
                }
                if (result > DateTime.MinValue)
                    break;
            }

            if (result > DateTime.MinValue)
            {
                differentTicks = (result - DateTime.UtcNow).Ticks;

                return result;
            }

            return DateTime.UtcNow;
        }

        private static DateTime GetNISTTime(String host)
        {
            //Returns DateTime.MinValue if host unreachable or does not produce time
            string timeStr;

            try
            {
                StreamReader reader = new StreamReader(new TcpClient(host, 13).GetStream());
                timeStr = reader.ReadToEnd();
                reader.Close();
            }
            catch (SocketException)
            {
                //Couldn't connect to server, transmission error
                //Debug.WriteLine("Socket Exception [" & host & "]")
                return DateTime.MinValue;
            }
            catch (Exception)
            {
                //Some other error, such as Stream under/overflow
                return DateTime.MinValue;
            }


            //'Parse timeStr
            if (timeStr.Substring(38, 9) != "UTC(NIST)")
            {
                //This signature should be there
                return DateTime.MinValue;
            }
            if (timeStr.Substring(30, 1) != "0")
            {
                //'Server reports non-optimum status, time off by as much as 5 seconds
                return DateTime.MinValue;    //'Try a different server
            }

            int jd = int.Parse(timeStr.Substring(1, 5));
            int yr = int.Parse(timeStr.Substring(7, 2));
            int mo = int.Parse(timeStr.Substring(10, 2));
            int dy = int.Parse(timeStr.Substring(13, 2));
            int hr = int.Parse(timeStr.Substring(16, 2));
            int mm = int.Parse(timeStr.Substring(19, 2));
            int sc = int.Parse(timeStr.Substring(22, 2));

            if (jd < 15020)
            {
                //'Date is before 1900
                return DateTime.MinValue;
            }
            if (jd > 51544)
                yr += 2000;
            else
                yr += 1900;

            return new DateTime(yr, mo, dy, hr, mm, sc);

        }
    }
}