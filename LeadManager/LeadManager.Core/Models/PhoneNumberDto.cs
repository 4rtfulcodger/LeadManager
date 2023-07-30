using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LeadManager.Core.Enums.EntityConstants;

namespace LeadManager.Core.Models
{
    public class PhoneNumberDto
    {
        public PhoneNumberType PhoneType { get; set; }
        public string Number { get; set; }
    }

    public class PhoneNumberForCreateDto
    {
        public PhoneNumberType PhoneType { get; set; }
        public string Number { get; set; }
    }
}
