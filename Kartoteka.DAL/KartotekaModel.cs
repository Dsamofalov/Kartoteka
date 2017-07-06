namespace Kartoteka.DAL
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class KartotekaModel : DbContext
    {
        public KartotekaModel()
            : base("name=KartotekaModel")
        {
        }
        public DbSet<AuthorModel> authors { get; set; }
        public DbSet<BookModel> books { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<AuthorModel>()
                        .HasMany<BookModel>(s => s.books)
                        .WithMany(c => c.authors)
                        .Map(cs =>
                        {
                            cs.MapLeftKey("AuthorID");
                            cs.MapRightKey("BookID");
                            cs.ToTable("AuthorBook");
                        });

        }
    }

}