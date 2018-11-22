using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace CrudApi.Models
{
    public class Employee
    {
        [BsonId]
        public ObjectId _Id { set; get; }
        [BsonElement("Name")]
        public string Name { get; set; }
        [BsonElement("Email")]
        public string Email { get; set; }
        [BsonElement("password")]
        public string Password { get; set; }
        [BsonElement("CreatedDate")]
        public DateTime CreatedDate { set; get; }
    }
}