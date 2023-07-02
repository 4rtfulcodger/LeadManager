using LeadManager.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace LeadManager.API.BusinessLogic.Common
{
    public class EndpointValidation : ControllerBase, IApiEndpointValidation
    {
        public bool IsValidCreateResult(object createdResult)
        {
           return (bool)createdResult == true ? true : false;
        }

        public bool ValidateDeleteResult(object deleteResult)
        {
            return (bool)deleteResult == true ? true : false;
        }

        public bool IsValidDeleteResult(object searchResult)
        {
            return searchResult != null ? true : false;                      
        }

        public bool IsValidUpdateResult(object updateResult)
        {
            return (bool)updateResult == true ? true : false;
        }

        public bool IsValidEntitySearchResult<T>(object lookupResult)
        {
            return lookupResult is T && lookupResult != null;
        }
    }
}
