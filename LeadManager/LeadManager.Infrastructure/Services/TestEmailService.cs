using LeadManager.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManager.Infrastructure.Services
{
    public class TestEmailService : IEmailService
    {
        private string _emailTo = "testrecipient@testlm.com";
        private string _emailFrom = "testsender@testlm.com";    

        public void Send(string subject, string recipientEmail, string message)
        {
            Console.WriteLine($"An email with subject \"{subject}\" and message \"{message}\" has been sent to {recipientEmail}");
        }
    }
}
