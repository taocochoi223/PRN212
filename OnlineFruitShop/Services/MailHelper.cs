using Microsoft.Extensions.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace Services
{
    public static class MailHelper
    {
        private static readonly IConfiguration _config;

        static MailHelper()
        {
            _config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public static void SendOtpEmail(string toEmail, string otpCode)
        {
            var emailSettings = _config.GetSection("EmailSettings");

            var smtpServer = emailSettings["SmtpServer"];
            var smtpPort = int.Parse(emailSettings["SmtpPort"]);
            var senderEmail = emailSettings["SenderEmail"];
            var senderPassword = emailSettings["SenderPassword"];
            var senderName = emailSettings["SenderName"];
            var enableSsl = bool.Parse(emailSettings["EnableSsl"]);

            string subject = "🍊 Mã OTP xác thực tài khoản - Online Fruit Shop";

            string body = $@"
                        <html>
                          <body style='font-family: Arial, sans-serif; background-color: #fef8f5; padding: 20px;'>
                            <div style='max-width: 600px; margin: auto; background: #ffffff; border-radius: 10px; padding: 30px; border: 1px solid #eee;'>
                              <h2 style='color: #ff6f00;'>🍎 Online Fruit Shop</h2>
                              <p>Xin chào bạn,</p>

                              <p style='font-size: 16px;'>Cảm ơn bạn đã đăng ký tài khoản tại <strong>Online Fruit Shop</strong>!</p>
                              <p style='font-size: 16px;'>Mã xác thực (OTP) của bạn là:</p>

                              <div style='background-color: #ffecb3; padding: 15px; text-align: center; font-size: 24px; font-weight: bold; border-radius: 8px; margin: 20px 0; color: #d84315;'>
                                {otpCode}
                              </div>

                              <p style='font-size: 14px; color: #555;'>Mã OTP chỉ có hiệu lực trong vòng <strong>30 giây</strong> kể từ khi gửi. 
                                 Vui lòng không chia sẻ mã này với bất kỳ ai.</p>

                              <p style='font-size: 14px; color: #333;'>Trân trọng,<br/>
                              <strong style='color:#ff6f00;'>{senderName}</strong> 🍇</p>
                            </div>
                          </body>
                        </html>";

            using var smtpClient = new SmtpClient(smtpServer)
            {
                Port = smtpPort,
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = enableSsl
            };

            var mail = new MailMessage
            {
                From = new MailAddress(senderEmail, senderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mail.To.Add(toEmail);
            smtpClient.Send(mail);
        }
    }
}
