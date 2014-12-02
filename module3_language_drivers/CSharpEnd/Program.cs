using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoSample
{
	class Program
	{
		static void Main(string[] args)
		{
			MongoClient client = new MongoClient("mongodb://127.0.0.1:27017/test"); // connect to localhost
			MongoServer server = client.GetServer();

			MongoDatabase database = server.GetDatabase("test"); // "test" is the name of the database
			MongoCollection<BsonDocument> bankData = database.GetCollection<BsonDocument>("bank_data");
			BsonDocument person = new BsonDocument {
				{ "first_name", "Steven"},
				{ "last_name", "Edouard"},
				{ "accounts", new BsonArray {
					new BsonDocument {
						{ "account_balance", 50000000},
						{ "account_type", "Investment"},
						{ "currency", "USD"}
					}
				}}
			};
			bankData.Insert(person);
			System.Console.WriteLine(person["_id"]);

			//increment this persons balance by 100000
			person["accounts"][0]["account_balance"] = person["accounts"][0]["account_balance"].AsInt32 + 100000;
			bankData.Save(person);
			System.Console.WriteLine("Successfully updated 1 document.");

			//retrieve the inserted collection from mongodb
			//should be the exact same object we just updated
			BsonDocument newPerson = bankData.FindOneById(person["_id"]);
			//check if the account balance was updated.
			System.Console.WriteLine(newPerson["accounts"][0]["account_balance"].AsInt32);


			
			//now delete the document we just inserted
			var query = Query.EQ("_id", newPerson["_id"]);
			WriteConcernResult result = bankData.Remove(query);
			System.Console.WriteLine("number of documents removed: " + result.DocumentsAffected);
		}
	}
}
