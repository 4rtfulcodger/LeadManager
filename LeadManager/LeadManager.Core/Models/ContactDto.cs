using LeadManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LeadManager.Core.Enums.EntityConstants;

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

    public class ContactDto
    {
        public Guid ContactId { get; set; }

        public ContactType PersonType { get; set; }

        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public List<PhoneNumberDto>? PhoneNumbers { get; set; }

        public List<AddressDto>? Addresses { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
        
    }
}
