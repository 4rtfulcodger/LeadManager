using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManager.Core.Interfaces
{
    public interface IApiResponseHandler
    {
        bool IsValidEntitySearchResult<T>(object lookupResult);

        void ReturnNotFoundIfEntityDoesNotExist<T>(object lookupResult);

        IActionResult ReturnSearchResult<Ts, TOut>(object searchResult);
        IActionResult ReturnCreateResult<TOut>(bool createdResult, string route, string Id, object newSource);
        IActionResult ReturndUpdateResult(bool updateSuccess);
        IActionResult ReturnDeleteResult(bool deleteSuccess);      
    }
}
