
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using WebAppAPIFrequncyPower.DataContext;
using WebAppAPIFrequncyPower.Services.Actions;
using WebAppAPIFrequncyPower.Services.ClassServic;
using WebAppAPIFrequncyPower.Services.Implementations;
using WebAppAPIFrequncyPower.Services.InterfaceServic;

namespace WebAppAPIFrequncyPower
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
      
            builder.Services.AddSwaggerGen();

            // افزودن سرویس‌های پایه
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // اتصال به دیتابیس (SQL Server)
            builder.Services.AddDbContext<PowerGridContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IFrequncy, DbFrequncy>();
            builder.Services.AddScoped<IAlertService, EmailAlertService>(); // یا هر سرویس دیگری برای هشدار
            builder.Services.AddScoped<ILoggingService, FileLoggingService>(); // یا هر سرویس دیگری برای گزارش

            // ثبت اکشن‌ها
            builder.Services.AddScoped<IAction, SaveFrequencyDataAction>();
            builder.Services.AddScoped<IAction, SendAlertAction>();
            builder.Services.AddScoped<IAction, LogFrequencyAction>();

            // ثبت مدیر اکشن‌ها
            builder.Services.AddScoped<ActionManager>();


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
