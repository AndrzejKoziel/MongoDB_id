// See https://aka.ms/new-console-template for more information

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB_id;
using System.Text.Json.Serialization;
using System.Text.Json;

BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));


BsonDefaults.GuidRepresentationMode = GuidRepresentationMode.V3;

var mongoUrl = new MongoUrl("mongodb://admin:admin@localhost:27017/?directConnection=true");
var dbName = "TestDB";
var colletionName = "TestCollection";

var mongoClient = new MongoClient(mongoUrl);
var mongoDB = mongoClient.GetDatabase(dbName);
var mongoCollection = mongoDB.GetCollection<Model>(colletionName);

var document = new Model
{
    Id = Guid.NewGuid()

};

mongoCollection.InsertOne(document);

var res1 = mongoCollection.FindSync(x => x.Id == document.Id).ToList();
var res2 = mongoCollection.Find(x => x.Id == document.Id).ToList();

var filter = "{ \"_id\" : UUID(\"" +document.Id.ToString() + "\") }";

var res3 = mongoCollection.Find(filter).ToList();

if (res1.Count <= 0)
    Console.WriteLine("Res1 - no documents");
else
    Console.WriteLine($"Res1 - {res2.Count} documents");

if (res2.Count <= 0)
    Console.WriteLine("Res2 - no documents");
else
    Console.WriteLine($"Res2 - {res1.Count} documents");

if (res3.Count <= 0)
    Console.WriteLine("Res3 - no documents");
else
    Console.WriteLine($"Res3 - {res2.Count} documents");


JsonSerializerOptions serializerOptions = new()
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};

serializerOptions.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
serializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, true));
builder.Services.AddSingleton(serializerOptions);
        }


Console.ReadLine();




