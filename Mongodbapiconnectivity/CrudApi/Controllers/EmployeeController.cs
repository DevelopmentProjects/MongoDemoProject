using System.Web.Http;
using MongoDB.Driver;
using MongoDB.Bson;
using CrudApi.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Configuration;
using System.Net;
using System;

namespace CrudApi.Controllers
{
    public class registrationController : ApiController
    {
        #region Getlist
        [HttpGet]
        public HttpResponseMessage GetregistrationList()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            string constr = ConfigurationManager.AppSettings["connectionString"];
            var Client = new MongoClient(constr);
            var db = Client.GetDatabase("Interview");
            var collection = db.GetCollection<Employee>("registration").Find(new BsonDocument()).Sort(Builders<Employee>.Sort.Descending("CreatedDate")).ToList();
            if (collection != null && collection.Count > 0)
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, collection);
            else
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, "No record found");
        }
        #endregion


        #region SaveData
        [HttpPost]
        public HttpResponseMessage SaveData(Employee registration)
        {
            //ed.Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
            if (registration != null)
            {
                string constr = ConfigurationManager.AppSettings["connectionString"];
                if (constr == null)
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Missing server side key in web.config");

                var Client = new MongoClient(constr);
                var DB = Client.GetDatabase("Interview");
                var collection = DB.GetCollection<Employee>("registration");
                if (collection.CollectionNamespace.CollectionName != null && collection.CollectionNamespace.DatabaseNamespace.DatabaseName != null)
                {
                    var registrations = collection.Find(a => a.Email == registration.Email).FirstOrDefault();
                    if (registrations != null && registrations.Email == registration.Email)
                    {
                        return Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, "Email id already exist");
                    }
                    else
                    {
                        DateTime dt = DateTime.UtcNow;
                        dt.ToJson().ToString().Replace("ISODate(\"", "").Replace("Z\")", "");
                        registration.CreatedDate = dt;
                        collection.InsertOneAsync(registration);
                        return Request.CreateResponse(System.Net.HttpStatusCode.OK, "Data save successfully!");
                    }

                }
                else
                    return Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, "Getting error to get collection name and database name");
            }
            else
                return Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, "Getting error to get registration data");
        }
        #endregion

        #region UpdateregistrationData
        [HttpPost]
        public HttpResponseMessage UpdateregistrationData(string Id, Employee registration)
        {
            if (registration != null)
            {
                string constr = ConfigurationManager.AppSettings["connectionString"];
                if (constr == null)
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Missing server side key in web.config");

                var Client = new MongoClient(constr);
                var DB = Client.GetDatabase("Interview");
                var collection = DB.GetCollection<Employee>("registration");

                var filters = Builders<Employee>.Filter.Eq("_Id", ObjectId.Parse(Id));

                var update = Builders<Employee>.Update.Set
                    ("Name", registration.Name).Set("Email", registration.Email).Set("Password", registration.Password);
                var result = collection.UpdateOne(filters, update);
                if (result != null)
                    return Request.CreateResponse(System.Net.HttpStatusCode.OK, "Data updated successfully");
                else
                    return Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, "Getting error to update data");
            }
            else
                return Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, "Getting error to get registration data");
        }
        #endregion


        #region DeleteregistrationData
        [HttpPost]
        public HttpResponseMessage DeleteData(string Id)
        {

            string constr = ConfigurationManager.AppSettings["connectionString"];
            if (constr == null)
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Missing server side key in web.config");

            var Client = new MongoClient(constr);
            var DB = Client.GetDatabase("Interview");
            var collection = DB.GetCollection<Employee>("registration");
            var registrationId = new ObjectId(Id);
            var result = collection.DeleteOne(Builders<Employee>.Filter.Eq("_Id", ObjectId.Parse(Id)));
            // var registration = collection.AsQueryable<registration>().SingleOrDefault(x => x._Id = registrationId);
            if (result != null)
                return Request.CreateResponse(HttpStatusCode.OK, "Data delete successfully");
            else
                return Request.CreateResponse(HttpStatusCode.OK, "Getting error to delete data");
        }
        #endregion

        #region GetDataById
        [HttpGet]
        public HttpResponseMessage GetregistrationData(string Id)
        {
            string constr = ConfigurationManager.AppSettings["connectionString"];
            if (constr == null)
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Missing server side key in web.config");

            var Client = new MongoClient(constr);
            var DB = Client.GetDatabase("Interview");
            var collection = DB.GetCollection<Employee>("registration");
            var empId = new ObjectId(Id);
            var registration = collection.Find(a => a._Id == ObjectId.Parse(Id)).FirstOrDefault();
            if (registration != null)
                return Request.CreateResponse(HttpStatusCode.OK, registration);
            else
                return Request.CreateResponse(HttpStatusCode.OK, "Getting error to fetch registration detail");
        }
        #endregion

        #region CheckDuplicateValue
        [HttpGet]
        public HttpResponseMessage CheckDuplicateEmail(string Email)
        {
            string constr = ConfigurationManager.AppSettings["connectionString"];
            if (constr == null)
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Missing server side key in web.config");

            var Client = new MongoClient(constr);
            var DB = Client.GetDatabase("Interview");
            var collection = DB.GetCollection<Employee>("registration");
            var registration = collection.Find(a => a.Email == Email);
            if (registration != null)
                return Request.CreateResponse(HttpStatusCode.OK, registration);
            else
                return Request.CreateResponse(HttpStatusCode.OK, "Getting error to fetch registration detail");
        }
        #endregion

    }
}
