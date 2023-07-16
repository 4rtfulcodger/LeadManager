using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LeadManager.Core.Enums.EntityConstants;

namespace LeadManager.Core.Entities
{
    public class PhoneNumber
    {
        public int Id { get; set; }

        public PhoneNumberType PhoneType { get; set; }

        public string Number { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
