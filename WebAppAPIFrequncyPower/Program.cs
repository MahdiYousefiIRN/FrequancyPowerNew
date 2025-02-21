
using Microsoft.EntityFrameworkCore;
using System;
using WebAppAPIFrequncyPower.DataContext;
using WebAppAPIFrequncyPower.Services.CLassServic;
using WebAppAPIFrequncyPower.Services.InterfaceServic;

namespace WebAppAPIFrequncyPower
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            // Configure Entity Framework and SQL Server
            builder.Services.AddDbContext<PowerGridContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("databasess"));
            });
            builder.Services.AddScoped<IFrequncy, DbFrequncy>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
