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


            // Enregistrer la connexion à la base de données
            builder.Services.AddDbContext<DataContext>(options =>
                options.UseSqlite("Data Source=data.db3"));

            // Enregistrer les services des repositories
            builder.Services.AddScoped<IBookRepository, SqlBookRepository>();

            // Ajouter les contrôleurs
            builder.Services.AddControllers();

            var app = builder.Build();

            // Configurer le pipeline des requêtes HTTP.
            app.UseAuthorization();

            // Mapping des contrôleurs (URL -> actions des contrôleurs)
            app.MapControllers();

            // Démarrer l'application
            app.Run();
        }
    }
}