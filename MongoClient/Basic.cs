// A Sample Application by Michael Kennedy
// http://www.michaelckennedy.net  | @mkennedy
// 
// This application is meant to run a very rudimentary comparison
// of performance between SQL Server and MongoDB in .NET
// See the full blog post here:
//
//    URL-TO-COME
//
using System;
using MongoDB.Bson;

namespace MongoClient
{
    public class Basic : MongoObj
    {
        public ObjectId _id { get; set; }
        public string TheData { get; set; }
        public DateTime TheTime { get; set; }
    }
}