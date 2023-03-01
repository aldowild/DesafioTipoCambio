using Desafio1.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Desafio1.DataBaseContext
{
    public class MyDataBaseContext:DbContext
    {
        public MyDataBaseContext(DbContextOptions options):base(options) { 

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "CambioDb");


        }

        public DbSet<CambioDataModel> Cambio { get; set; }


    }
}
