using LeadManager.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManager.Infrastructure.Services
{
    public class TestEmailService : IEmailService
    {
        private string _emailTo = String.Empty;
        private string _emailFrom = String.Empty;

        public TestEmailService(IConfiguration configuration)
        {
            _emailTo = configuration["MailSettings:mailToAddress"];
            _emailFrom = configuration["MailSettings:mailFromAddress"];
        }

        public void Send(string subject, string message, string recipientEmail = "")
        {
            if (recipientEmail == String.Empty)
                recipientEmail = _emailTo;

            Console.WriteLine($"An email with subject \"{subject}\" and message \"{message}\" has been sent to {recipientEmail} from {_emailFrom}");
        }
    }
}
