<<<<<<< HEAD
using BiblioFind.Data.Repositories;
using BiblioFind.Data;
using Microsoft.EntityFrameworkCore;

=======
>>>>>>> bc523e1f68da446c605fa0f30e0e3b413111db3c
namespace BiblioFind
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

<<<<<<< HEAD

            // Enregistrer la connexion � la base de donn�es
            builder.Services.AddDbContext<DataContext>(options =>
                options.UseSqlite("Data Source=data.db3"));

            // Enregistrer les services des repositories
            builder.Services.AddScoped<IBookRepository, SqlBookRepository>();

            // Ajouter les contr�leurs
=======
            // Add services to the container.

>>>>>>> bc523e1f68da446c605fa0f30e0e3b413111db3c
            builder.Services.AddControllers();

            var app = builder.Build();

<<<<<<< HEAD
            // Configurer le pipeline des requ�tes HTTP.
            app.UseAuthorization();

            // Mapping des contr�leurs (URL -> actions des contr�leurs)
            app.MapControllers();

            // D�marrer l'application
            app.Run();
        }
    }
}
=======
            // Configure the HTTP request pipeline.

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
>>>>>>> bc523e1f68da446c605fa0f30e0e3b413111db3c
