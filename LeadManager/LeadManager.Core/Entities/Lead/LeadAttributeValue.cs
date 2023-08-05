using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManager.Core.Entities
{
    public class LeadAttributeValue
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LeadAttributeValueId { get; set; }

        public int LeadAttributeId { get; set; }
        public LeadAttribute Attribute { get; set; }

        public string AttributeValue { get; set; }
    }
}
