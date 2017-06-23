namespace Kartoteka
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class DataBaseModel : DbContext
    {
        // Контекст настроен для использования строки подключения "DataBaseModel" из файла конфигурации  
        // приложения (App.config или Web.config). По умолчанию эта строка подключения указывает на базу данных 
        // "Kartoteka.DataBaseModel" в экземпляре LocalDb. 
        // 
        // Если требуется выбрать другую базу данных или поставщик базы данных, измените строку подключения "DataBaseModel" 
        // в файле конфигурации приложения.
        public DataBaseModel()
            : base("name=DataBaseModel")
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