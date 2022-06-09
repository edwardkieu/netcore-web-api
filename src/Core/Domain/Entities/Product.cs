using Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Product : AuditableEntity
    {
        [Required]
        [MaxLength(250)]
        public string Name { get; set; }

        [Required]
        [MaxLength(10)]
        public string Barcode { get; set; }

        public string Description { get; set; }

        [Required]
        [Range(1, 5)]
        public decimal Rate { get; set; }
    }
}