using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using CouchWebApi.Models;
using MyCouch;
using MyCouch.Requests;

namespace CouchWebApi.Controllers
{
    public class NotificationsController : ApiController
    {
        public async Task<List<Notification>> Get()
        {
            var uriBuilder = GetCouchUrl();

            using (var client = new MyCouchClient(uriBuilder.Build()))
            {
                var notifications = await client.Views.QueryAsync<Notification>(new QueryViewRequest("notifications-doc", "all-notifications"));
                return notifications.Rows.Select(r => r.Value).ToList();
            }
        }
        
        public async Task<Notification> Post(Notification notification)
        {
            var uriBuilder = GetCouchUrl();

            using (var client = new MyCouchClient(uriBuilder.Build()))
            {
                var response = await client.Entities.PostAsync(notification);

                return response.Content;
            }
        }

        private MyCouchUriBuilder GetCouchUrl()
        {
            return new MyCouchUriBuilder("https://[username].cloudant.com/")
                .SetDbName("notifications")
                .SetBasicCredentials("[usename]", "[password]");
        }
    }
}
