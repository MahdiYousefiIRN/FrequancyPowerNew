using MVCApp_PowerFrequency.Services;

namespace MVCApp_PowerFrequency
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // اضافه کردن سرویس‌های HTTPClient برای استفاده در PowerFrequencyService
            builder.Services.AddHttpClient<PowerFrequencyService>(client =>
            {
                var apiUrl = builder.Configuration["ApiUrl"] ?? Environment.GetEnvironmentVariable("API_URL");
                if (string.IsNullOrEmpty(apiUrl))
                {
                    throw new InvalidOperationException("API URL is not configured.");
                }
                client.BaseAddress = new Uri(apiUrl);
            });

            // اضافه کردن سرویس‌های MVC برای اپلیکیشن
            builder.Services.AddControllersWithViews();

            // ایجاد اپلیکیشن
            var app = builder.Build();

            // استفاده از مسیرهای پیش‌فرض و فایل‌های استاتیک
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseStaticFiles(); // برای دسترسی به فایل‌های استاتیک

            // فعال کردن مسیریابی برای MVC
            app.UseRouting();

            // استفاده از MapControllerRoute برای پیکربندی مسیرها
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // اجرای اپلیکیشن
            app.Run();
        }
    }
}
