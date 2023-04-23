using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManager.Core.Interfaces
{
    public interface IApiEndpointValidation
    {
        bool IsValidDeleteResult(object searchResult);
        bool IsValidCreateResult(object createdResult);
        bool ValidateDeleteResult(object deleteResult);
        bool IsValidUpdateResult(object updateResult);
    }
}
