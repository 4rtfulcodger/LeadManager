using LeadManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManager.Core.Models
{
    public class ContactForCreateDto
    {
        public Guid ContactId { get; set; } = Guid.NewGuid();

        public string Title { get; set; } = string.Empty;
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public List<PhoneNumberForCreateDto>? PhoneNumbers { get; set; }

        public List<AddressForCreateDto>? Addresses { get; set; }
    }
}
