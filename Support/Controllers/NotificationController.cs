using Support.Controllers.Unit;
using Newtonsoft.Json;
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
using WebPush;
using Support.Models;
using System.Configuration;

namespace Support.Controllers
{
    public class NotificationController : ApiController
    {
        public static string publicKey = ConfigurationManager.AppSettings["N_PublicKey"];
        public static string privateKey = ConfigurationManager.AppSettings["N_PrivateKey"];
        public static string subject = ConfigurationManager.AppSettings["N_Subject"];
        public static VapidDetails vapId = new VapidDetails(subject, publicKey, privateKey);

        SupportModel db = new SupportModel();

        public partial class NotificationUser
        {
            public long Id { get; set; }

            public int? LockNumber { get; set; }

            public string SystemName { get; set; }

            public string EndPoint { get; set; }

            public string P256dh { get; set; }

            public string Auth { get; set; }

            public DateTime? CreatedDate { get; set; }

            public bool? IsActive { get; set; }

            public bool? IsExpire { get; set; }
        }

        public class UserObject
        {
            public int LockNumber { get; set; }

            public string SystemName { get; set; }

            public string EndPoint { get; set; }

            public string P256dh { get; set; }

            public string Auth { get; set; }

            public bool IsActive { get; set; }
        }

        [Route("api/Notification/SaveUser")]
        public async Task<IHttpActionResult> PostSaveUser(UserObject d)
        {
            try
            {
                string sql = string.Format(@"
                                    if EXISTS(select 1 from NotificationUser where LockNumber = {0} and SystemName = '{1}')
                                       delete NotificationUser where LockNumber = {0} and SystemName = '{1}'
                                    INSERT INTO NotificationUser(LockNumber,SystemName,[EndPoint],P256dh,Auth,IsExpire,IsActive)
                                                          VALUES({0},'{1}','{2}','{3}','{4}', 0, 1)
                                    SELECT 1",
                                    d.LockNumber, d.SystemName, d.EndPoint, d.P256dh, d.Auth);
                var list = db.Database.SqlQuery<int>(sql).Single();
                db.SaveChanges();
                return Ok(list);
            }
            catch (Exception e)
            {
                throw;
            }
        }


        public class SendObject
        {
            public int LockNumber { get; set; }

            public string SystemName { get; set; }

            public string Title { get; set; }

            public string Message { get; set; }
        }

        [Route("api/Notification/SendNotification")]
        public async Task<IHttpActionResult> SendNotification(SendObject d)
        {
            var mes = JsonConvert.SerializeObject(new
            {
                title = d.Title,
                message = d.Message,
            });

            string sql = string.Format(@"select * from NotificationUser where LockNumber = {0} and SystemName = '{1}' and IsExpire = 0 and IsActive = 1",
                                       d.LockNumber, d.SystemName);
            var list = db.Database.SqlQuery<NotificationUser>(sql).ToList();
            if (list.Count > 0)
            {
                try
                {
                    var subScript = new PushSubscription(list[0].EndPoint, list[0].P256dh, list[0].Auth);
                    var client = new WebPushClient();
                    client.SendNotification(subScript, mes, vapId);
                    return Ok("Send");
                }
                catch (WebPushException ex)
                {
                    if (ex.StatusCode == HttpStatusCode.NotFound || ex.StatusCode == HttpStatusCode.Gone)
                    {
                        sql = string.Format(@"Update NotificationUser set IsExpire = 1 where id = {0} select 1", list[0].Id);
                        var update = db.Database.SqlQuery<int>(sql).ToList();
                        db.SaveChanges();
                        return Ok("Expire");
                    }
                }
            }
            else
                return Ok("NotFind");
            return Ok("");
        }


    }
}