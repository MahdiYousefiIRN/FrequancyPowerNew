using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebAppAPIFrequncyPower.DataContext;
using WebAppAPIFrequncyPower.Services.Actions;
using WebAppAPIFrequncyPower.Services.ClassServic;
using WebAppAPIFrequncyPower.Services.Implementations;
using WebAppAPIFrequncyPower.Services.InterfaceServic;
using WebAppAPIFrequncyPower.Services.SignalR;

namespace WebAppAPIFrequncyPower
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // تنظیمات Swagger
            builder.Services.AddSwaggerGen();

            // سرویس‌های پایه
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSignalR(); // سرویس SignalR

            // اتصال به دیتابیس (SQL Server)
            builder.Services.AddDbContext<PowerGridContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // اضافه کردن سرویس‌های مختلف
            builder.Services.AddScoped<IFrequncy, DbFrequncy>();
            builder.Services.AddScoped<IAlertService, EmailAlertService>();
            builder.Services.AddScoped<ILoggingService, FileLoggingService>();

            // ثبت اکشن‌ها
            builder.Services.AddScoped<IAction, SaveFrequencyDataAction>();
            builder.Services.AddScoped<IAction, SendAlertAction>();
            builder.Services.AddScoped<IAction, LogFrequencyAction>();

            // ثبت ActionManager
            builder.Services.AddScoped<ActionManager>();

            var app = builder.Build();

            // تنظیمات Pipeline HTTP
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.MapHub<FrequencyHub>("/frequencyHub");
            app.Run();
        }
    }
}
