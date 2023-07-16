using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManager.Core.Enums
{
    public class EntityConstants
    {
        public enum ContactType
        {
            PrimaryContact = 1,
            SecondaryContact = 2
        }

        public enum AddressType
        {
            CurrentResidential = 0,
            PreviousResidential = 1,
            CurrentBusiness = 2,
            PreviousBusiness = 3,
            CorrespondenceOnly = 4
        }

        public enum Gender
        {
            Male = 0,
            Female = 1,
            Unspecified = 2
        }
        public enum PhoneNumberType
        {
            LandLine = 1,
            Mobile = 2
        }
    }
}
