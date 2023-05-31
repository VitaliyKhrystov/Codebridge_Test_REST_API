using Codebridge_Test_REST_API.Domain;
using Codebridge_Test_REST_API.Domain.Repositories;
using Codebridge_Test_REST_API.Domain.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Codebridge_Test_REST_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<AppDbContext>(c => c.UseSqlServer(builder.Configuration.GetSection("ConnectionString").Value));
            builder.Services.AddTransient<IDogRepository, DogRepositoryEF>();
            builder.Services.AddControllers();

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.MapControllers();

            app.Run();
        }
    }
}