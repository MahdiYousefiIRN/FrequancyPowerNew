using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using WebAppAPIFrequncyPower.Services.InterfaceServic;

public class EmailAlertService : IAlertService
{
    private readonly string _smtpUser;
    private readonly string _smtpPassword;
    private readonly string _fromEmail;
    private readonly string _toEmail;
    private readonly string _smtpServer;
    private readonly int _smtpPort;

    private readonly SmtpClient _smtpClient;

    public EmailAlertService(IConfiguration configuration)
    {
        // بارگذاری مقادیر از فایل پیکربندی (appsettings.json)
        _smtpUser = configuration["EmailSettings:SmtpUser"];
        _smtpPassword = configuration["EmailSettings:SmtpPassword"];
        _fromEmail = configuration["EmailSettings:FromEmail"];
        _toEmail = configuration["EmailSettings:ToEmail"];
        _smtpServer = configuration["EmailSettings:SmtpServer"];
        _smtpPort = int.Parse(configuration["EmailSettings:SmtpPort"]);

        _smtpClient = new SmtpClient(_smtpServer)
        {
            Credentials = new NetworkCredential(_smtpUser, _smtpPassword),
            Port = _smtpPort,
            EnableSsl = true
        };
    }

    public async Task SendAlertAsync(string message)
    {
        var mailMessage = new MailMessage
        {
            From = new MailAddress(_fromEmail),
            Subject = "Power Frequency Alert",
            Body = message
        };

        mailMessage.To.Add(_toEmail);

        try
        {
            // ارسال ایمیل به صورت غیرهمزمان
            await _smtpClient.SendMailAsync(mailMessage);
        }
        catch (Exception ex)
        {
            // مدیریت استثناها و نمایش پیام خطا
            Console.WriteLine($"Error sending email: {ex.Message}");
            throw; // یا می‌توان خطا را به یک لاگ ارسال کرد
        }
    }

    // اطمینان از آزادسازی منابع
    public void Dispose()
    {
        _smtpClient?.Dispose();
    }
}
