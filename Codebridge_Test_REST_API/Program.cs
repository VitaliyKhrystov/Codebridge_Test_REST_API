using Codebridge_Test_REST_API.Domain;
using Codebridge_Test_REST_API.Domain.Repositories;
using Codebridge_Test_REST_API.Domain.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Threading.RateLimiting;

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
            builder.Services.AddRateLimiter(opt =>
            {
                opt.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                    RateLimitPartition.GetConcurrencyLimiter(
                        partitionKey: context.Request.Headers.Host.ToString(),
                        factory: partition => new ConcurrencyLimiterOptions
                        {
                            PermitLimit = int.TryParse(builder.Configuration.GetSection("PermitLimit").Value, out int result) ? result : 10
                        })
                );
                opt.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            });
            builder.Services.AddAutoMapper(typeof(Program).Assembly);

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.MapControllers();

            app.UseRateLimiter();

            app.Run();
        }
    }
}