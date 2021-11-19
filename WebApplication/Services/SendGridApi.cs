using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Services
{
    public static class SendGridApi
    {

        public static async Task<bool> Execute(string userEmail, string userName, string plainTextContent, string htmlContent, string subject)
        {
            Environment.SetEnvironmentVariable("envvar", "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");
            var apiKey = Environment.GetEnvironmentVariable("envvar");

            var client = new SendGridClient("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");
            var from = new EmailAddress("ahmedeloghazy@gmail.com", "Diabetes");
            var to = new EmailAddress(userEmail, userName);

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            var response = await client.SendEmailAsync(msg);

            return await Task.FromResult(true);
        }
        public  static async Task Execute()
        {
            Environment.SetEnvironmentVariable("envvar", "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");
           var apiKey = Environment.GetEnvironmentVariable("envvar");

            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("ahmedeloghazy@gmail.com", "Example User");
            var subject = "Sending with SendGrid is Fun";
            var to = new EmailAddress("abdelzizahmed06@gmail.com", "Example User");
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }


    }

}
