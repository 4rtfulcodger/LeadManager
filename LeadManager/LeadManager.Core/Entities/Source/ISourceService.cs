using LeadManager.Core.ViewModels;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManager.Core.Entities.Source
{
    public interface ISourceService
    {
        //Need to add a filter parameter
        Task<IEnumerable<Source>> GetSourcesAsync();
        Task<Source> GetSourceWithIdAsync(int sourceId);
        Task<bool> CreateSourceAsync(Source source);
        Task<bool> UpdateSourceAsync(int sourceId);
        Task<bool> DeleteSource(Source source);
    }
}
