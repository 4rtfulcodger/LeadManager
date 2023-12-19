using LeadManager.Core.Entities.Source;
using LeadManager.Core.Helpers;
using LeadManager.Core.Interfaces.Source;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManager.Test.TestRepositories
{
    public class TestSourceRepository : ISourceRepository
    {
        static List<Source> sourceList = new List<Source>();

        public async Task<bool> AddSourceAsync(Source source)
        {
            sourceList.Add(source);
            return true;
        }

        public async Task<bool> DeleteSource(Source lead)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Source>> GetSourcesAsync()
        {
            await Task.Delay(0);
            return sourceList;
        }

        public async Task<PagedList<Source>> GetSourcesAsync(SourceFilter filter)
        {
            throw new NotImplementedException();
        }

        public async Task<Source?> GetSourceWithIdAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateSourceAsync(int Id)
        {
            throw new NotImplementedException();
        }
    }
}
