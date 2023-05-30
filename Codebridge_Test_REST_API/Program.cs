using Codebridge_Test_REST_API.Domain;
using Microsoft.EntityFrameworkCore;

namespace Codebridge_Test_REST_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<AppDbContext>(c => c.UseSqlServer(builder.Configuration.GetSection("ConnectionString").Value));

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.MapControllers();

            app.Run();
        }
    }
}