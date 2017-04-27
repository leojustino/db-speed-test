using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace MongoClient
{
    public interface MongoObj
    {
        ObjectId _id { get; set; }

    }
}
