using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManager.Core.Entities
{
    public class LeadType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LeadTypeId { get; set; }

        public Guid LeadTypeReference { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        public List<LeadAttribute> LeadAttributes { get; set; } = new List<LeadAttribute>();

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
