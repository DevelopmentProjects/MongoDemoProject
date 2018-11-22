using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using MongoDB.Driver;
using MongoDB.Bson;
using Mongodbapiconnectivity.Models;
using System.Collections.Generic;

namespace Mongodbapiconnectivity.Controllers
{
    public class EmployeeController : ApiController
    {
        public HttpResponseMessage GetEmployeeList()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            string constr = ConfigurationManager.AppSettings["connectionString"];
            var Client = new MongoClient(constr);
            var db = Client.GetDatabase("Interview");
            var collection = db.GetCollection<EmployeeModel>("employee").Find(new BsonDocument()).ToList();
            if (collection != null && collection.Count > 0)
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, collection);
            else
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, "No record found");
        }
    }
}
