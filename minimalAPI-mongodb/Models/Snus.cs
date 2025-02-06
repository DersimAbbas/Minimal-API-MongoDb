using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;

namespace minimalAPI_mongodb.Models
{
    public class Snus
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;


        [BsonElement("type")]
        public string Type { get; set; } = string.Empty;

        [BsonElement("strength")]
        public string Strength { get; set; } = string.Empty;

        [BsonElement("tobak")]
        public string Tobak { get; set; }

        [BsonElement("price")]
        public int Price { get; set; }
    }

}



        


