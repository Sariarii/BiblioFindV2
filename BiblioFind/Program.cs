using BiblioFind.Data.Repositories;
using BiblioFind.Data;
using Microsoft.EntityFrameworkCore;

namespace BiblioFind
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Enregistrer la connexion � la base de donn�es
            builder.Services.AddDbContext<DataContext>(options =>
                options.UseSqlite("Data Source=data.db3"));

            // Enregistrer les services des repositories
            builder.Services.AddScoped<IBookRepository, SqlBookRepository>();

            // Ajouter les contr�leurs
            builder.Services.AddControllers();

            var app = builder.Build();

            // Configurer le pipeline des requ�tes HTTP.
            app.UseAuthorization();

            // Mapping des contr�leurs (URL -> actions des contr�leurs)
            app.MapControllers();

            // D�marrer l'application
            app.Run();
        }
    }
}