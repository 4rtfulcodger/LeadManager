using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LeadManager.Core.Enums.EntityConstants;

namespace LeadManager.Core.Entities.Contact
{
    public class Contact
    {
        public int Id { get; set; }

        public Guid ContactId { get; set; }

        public ContactType PersonType { get; set; }

        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public List<PhoneNumber> PhoneNumbers { get; set; }

        public List<Address> Addresses { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public Contact()
        {

        }
    }
}
