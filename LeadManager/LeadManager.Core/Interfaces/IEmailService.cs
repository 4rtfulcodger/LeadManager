using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManager.Core.Interfaces
{
    public interface IEmailService
    {
        void Send(string subject, string message, string recipientEmail = "");
    }
}
