using LeadManager.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManager.Test
{
    public class LeadFilterTests
    {
        [Fact]
        public void LeadHelper_LeadFilter_HasDefaultValuesSetForPageNumberAndPageSize() {

            LeadFilter leadFilter = new LeadFilter();

            Assert.Equal(1, leadFilter.PageNumber);
            Assert.Equal(5, leadFilter.PageSize);
        }

        [Fact]
        public void LeadHelper_LeadFilter_WillResetPageSizeTo20WhenAttemptingToSetAValueHigherThan20()
        {
            LeadFilter leadFilter = new LeadFilter();
            leadFilter.PageSize = 21;
            Assert.Equal(20, leadFilter.PageSize);
        }
    }
}
