using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManager.Core.Entities
{
    public class Lead
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LeadId { get; set; }

        public string LeadRef { get; set; }

        [ForeignKey("LeadTypeId")]
        public int LeadTypeId { get; set; }

        [ForeignKey("SourceId")]
        public Source.Source Source { get; set; }
        public int SourceId { get; set; }

        [ForeignKey("SupplierId")]
        public Supplier.Supplier Supplier { get; set; }
        public int SupplierId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Description { get; set; }

        public List<Contact> Contacts { get; set; }

        public List<LeadAttributeValue>? LeadAttributeValues { get; set; } = null;

        public Lead(int sourceId, int supplierId, string name, string? description)
        {
            SourceId = sourceId;
            SupplierId = supplierId;
            Name = name;
            Description = description;
        }
    }
}
