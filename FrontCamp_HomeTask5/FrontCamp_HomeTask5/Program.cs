using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using FrontCamp_HomeTask5.Documents;
using MongoDB.Bson;
using MongoDB.Driver;
using Tag = FrontCamp_HomeTask5.Documents.Tag;

namespace FrontCamp_HomeTask5
{
  class Program
  {
    static void Main(string[] args)
    {
      //string connectionString = "mongodb://localhost:27017"; // адрес сервера
      //MongoClient client = new MongoClient(connectionString);
      //GetDatabaseNames(client);
      //GetCollectionsNames(client).Wait();


      string con = ConfigurationManager.ConnectionStrings["MongoDb"].ConnectionString;
      var client = new MongoClient(con);
      var database = client.GetDatabase("Posts");

      //Tasks 1- 3

      //CreateDocs(database).GetAwaiter().GetResult();
      //CreateComments(database).GetAwaiter().GetResult();
      //ReadAndFindDocs(database).GetAwaiter().GetResult();
      //UpdateComment(database).GetAwaiter().GetResult();
      //DeleteComment(database).GetAwaiter().GetResult();

      //CreateIndex(database).GetAwaiter().GetResult();

      //Task 4
      GetBestClass().GetAwaiter().GetResult();

      Console.ReadLine();
    }


    //Task 4
    private static async Task GetBestClass()
    {
      string con = ConfigurationManager.ConnectionStrings["MongoDb"].ConnectionString;
      var client = new MongoClient(con);
      var database = client.GetDatabase("grades");

      var collection = database.GetCollection<BsonDocument>("grades");
      var filter = Builders<BsonDocument>.Filter;
     

      //var aggregate = collection.Aggregate()
      //  .Group(new BsonDocument { { "_id", "$class_id" }, { "count", new BsonDocument("$sum", 1) } })
      //  .Group(new BsonDocument { { "_id", "$student_id" }, { "avrScores", new BsonDocument("$avg", "$scores") } })
      //  .Unwind<BsonDocument>(new BsonDocument {{ "_id", "$scores" }});
      //  //.Match(new BsonDocument { { "borough", "Queens" }, { "cuisine", "Brazilian" } })

      var aggregate1 = collection.Aggregate()
        .Unwind("scores")
        .Group(new BsonDocument { { "_id", "$class_id" }, { "count", new BsonDocument("$sum", 1) } });
        //.Group(new BsonDocument { { "_id", "$student_id" }, { "avrScore", new BsonDocument("$avg", "$score") } });
       


      var results = await aggregate1.ToListAsync();

      foreach (var c in results)
        Console.WriteLine(c);

    }



    private static async Task CreateDocs(IMongoDatabase database)
    {
      var articleCollection = database.GetCollection<Article>("articles");
      var authorCollection = database.GetCollection<Author>("authors");
      var commentsCollection = database.GetCollection<Comment>("comments");
      var tagCollections = database.GetCollection<Tag>("tags");


      var author = new Author { Id = 1, Name = "www.bbsnews.com", ListArticlesId = new List<int>() };
      author.ListArticlesId.Add(1);
      var author2 = new Author { Id = 2, Name = "www.cnet.com", ListArticlesId = new List<int>() };
      author2.ListArticlesId.Add(1);
      var author3 = new Author { Id = 3, Name = "www.eng.belta.by", ListArticlesId = new List<int>() };
      author3.ListArticlesId.Add(1);

      await authorCollection.InsertManyAsync(new[] { author, author2, author3 });

      var comment = new Comment { Id = 1, ArticleId = 1, Text = "blalblablaa111111" };
      var comment1 = new Comment { Id = 2, ArticleId = 2, Text = "blalblablaa22222" };
      var comment2 = new Comment { Id = 3, ArticleId = 1, Text = "blalblablaa33333" };
      var comment3 = new Comment { Id = 4, ArticleId = 3, Text = "blalblablaa44444" };
      await commentsCollection.InsertManyAsync(new[] { comment, comment1, comment2, comment3 });

      var tag = new Tag { Id = 1, TagName = "Facebook" };
      var tag1 = new Tag { Id = 2, TagName = "Plane" };
      var tag2 = new Tag { Id = 3, TagName = "Facebook3" };
      var tag3 = new Tag { Id = 4, TagName = "Facebook4" };
      await tagCollections.InsertManyAsync(new[] { tag, tag1, tag2, tag3 });

      var article = new Article { Id = 1, AuthorId = 1, ListCommnetsId = new List<int>(), ListTagsId = new List<int>(), Title = "Facebook is the unwilling king of the news" };
      article.ListCommnetsId.Add(1);
      article.ListCommnetsId.Add(3);
      article.ListTagsId.Add(1);

      var article2 = new Article { Id = 2, AuthorId = 2, ListCommnetsId = new List<int>(), ListTagsId = new List<int>(), Title = "Traces from EgyptAir victims point to blast on plane" };
      article2.ListCommnetsId.Add(2);
      article2.ListTagsId.Add(2);

      var article3 = new Article { Id = 3, AuthorId = 3, ListCommnetsId = new List<int>(), ListTagsId = new List<int>(), Title = "Vitebsk, Gomel city development plans revised till 2025" };
      article3.ListCommnetsId.Add(4);
      article3.ListTagsId.Add(3);

      await articleCollection.InsertManyAsync(new[] { article, article2, article3 });

    }

    private static async Task CreateComments(IMongoDatabase database)
    {
      var commentsCollection = database.GetCollection<BsonDocument>("comments");

      var comment1 = new BsonDocument
            {
                { "_id", 1 },
                { "ArticleId", 1 },
                { "Text", "blablabla22222" }
            };

      var comment2 = new BsonDocument
            {
                { "_id", 2 },
                { "ArticleId", 2 },
                { "Text", "blablabla22222" }
            };

      await commentsCollection.InsertManyAsync(new[] { comment1, comment2 });
    }

    private static async Task ReadAndFindDocs(IMongoDatabase database)
    {
      var authorCollection = database.GetCollection<BsonDocument>("authors");
      var articleCollection = database.GetCollection<BsonDocument>("articles");

      var filterAuthor = Builders<BsonDocument>.Filter.Eq("Name", "www.bbsnews.com");
      var resultAuthor = await authorCollection.Find(filterAuthor).ToListAsync();

      var filter = Builders<BsonDocument>.Filter.Eq("AuthorId", resultAuthor.FirstOrDefault()["_id"]);
      var result = await articleCollection.Find(filter).ToListAsync();

      foreach (var article in result)
      {
        Console.WriteLine("Atricle by BBS:" + article["Title"]);
      }
    }

    private static async Task UpdateComment(IMongoDatabase database)
    {
      var commentCollection = database.GetCollection<BsonDocument>("comments");
      var filter = Builders<BsonDocument>.Filter.Eq("_id", 2);

      var update = Builders<BsonDocument>.Update
       .Set("Text", "Update comment")
       .CurrentDate("lastModified");
      await commentCollection.UpdateOneAsync(filter, update);

      var comments = await commentCollection.Find(new BsonDocument()).Sort(Builders<BsonDocument>.Sort.Ascending("_id")).ToListAsync();
      foreach (var c in comments)
        Console.WriteLine(c);
    }

    private static async Task DeleteComment(IMongoDatabase database)
    {
      var commentCollection = database.GetCollection<BsonDocument>("comments");
      var filter = Builders<BsonDocument>.Filter.Eq("_id", 1);
      await commentCollection.DeleteOneAsync(filter);

      var comments = await commentCollection.Find(new BsonDocument()).ToListAsync();
      foreach (var c in comments)
        Console.WriteLine(c);
    }

    static async Task CreateIndex(IMongoDatabase database)
    {
      var collection = database.GetCollection<Author>("authors");
      await collection.Indexes.CreateOneAsync(Builders<Author>.IndexKeys.Ascending(_ => _.Name));
    }

  }
}
