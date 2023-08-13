using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManager.Core.Entities
{
    public class LeadAttribute
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LeadAttributeId { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }
                
        public int LeadTypeId { get; set; }

        [ForeignKey("LeadTypeId")]
        public LeadType LeadType { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
