using Acme.BookStore.Authors;
using Acme.BookStore.Books;
using Acme.BookStore.Musics;
using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Acme.BookStore.MongoDB;

[ConnectionStringName("Default")]
public class BookStoreMongoDbContext : AbpMongoDbContext
{
    public IMongoCollection<Book> Books => Collection<Book>();

    public IMongoCollection<Author> Authors => Collection<Author>();

    public IMongoCollection<Music> Musics => Collection<Music>();


    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        //modelBuilder.Entity<YourEntity>(b =>
        //{
        //    //...
        //});
    }
}
