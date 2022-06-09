using System.ComponentModel.DataAnnotations;

namespace Domain.Common
{
    public abstract class AuditableEntity : BaseEntity
    {
        [MaxLength(68)]
        public string CreatedBy { get; set; }

        public DateTime Created { get; set; }

        [MaxLength(68)]
        public string LastModifiedBy { get; set; }

        public DateTime? LastModified { get; set; }
    }
}