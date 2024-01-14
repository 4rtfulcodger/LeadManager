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
    public class TestSourceRepository: ISourceRepository
    {
        static List<Source> sourceList = new List<Source>();

        public TestSourceRepository()
        {
                
        }

        public async Task<bool> AddSourceAsync(Source source)
        {
            source.SourceId = sourceList.Count + 1;
            sourceList.Add(source);
            return true;
        }

        public async Task<bool> DeleteSource(Source lead)
        {
            await Task.Delay(0);
            return true;
        }

        public async Task<IEnumerable<Source>> GetSourcesAsync()
        {
            await Task.Delay(0);
            return sourceList;
        }

        public async Task<PagedList<Source>> GetSourcesAsync(SourceFilter filter)
        {
            IQueryable<Source> queryableSourceList = sourceList.AsQueryable();
            return await PagedList<Source>.Create(queryableSourceList, filter.PageNumber, filter.PageSize);
            
        }

        public async Task<Source?> GetSourceWithIdAsync(int Id)
        {
            await Task.Delay(0);
            return sourceList.FirstOrDefault(sl => sl.SourceId == Id);
        }

        public async Task<bool> UpdateSourceAsync(int Id)
        {
            await Task.Delay(0);
            return true;
        }
    }
}
