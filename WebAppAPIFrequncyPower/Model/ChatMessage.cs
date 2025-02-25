namespace WebAppAPIFrequncyPower.Model
{
    public class ChatMessage
    {
        // شماره پیام (برای مثال، می‌تواند برای شناسایی پیام‌ها استفاده شود)
        public double NumberMessage { get; set; }

        // نام فرستنده پیام
        public string SenderName { get; set; }

        // زمان ارسال پیام
        public DateTimeOffset DateSentAt { get; set; }
    }

}
