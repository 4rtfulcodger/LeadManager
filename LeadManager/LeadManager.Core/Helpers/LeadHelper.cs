using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManager.Core.Helpers
{
    public static class LeadHelper
    {
        public static string GenerateLeadReference()
        {
           return "L" + DateTime.Now.ToBinary().ToString().Replace("-",string.Empty);
        }
    }
}
