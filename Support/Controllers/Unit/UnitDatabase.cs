using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Net.Http.Headers;
using System.Collections.Specialized;

namespace Support.Controllers.Unit
{
    public static class UnitDatabase
    {

        public static List<string> ReadUserPassHeader(HttpRequestHeaders header)
        {
            List<string> list = new List<string>();

            string userName = string.Empty;
            string password = string.Empty;
            string userKarbord = string.Empty;


            if (header.Contains("userName"))
            {
                userName = header.GetValues("userName").First();
            }
            if (header.Contains("password"))
            {
                password = header.GetValues("password").First();
            }
            if (header.Contains("userKarbord"))
            {
                userKarbord = header.GetValues("userKarbord").First();
            }

            list.Add(userName);
            list.Add(password);
            list.Add(userKarbord);

            return (list);
        }

    }
}
