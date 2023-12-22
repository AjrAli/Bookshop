using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Bookshop.Domain.Common
{
    public abstract class AuditableEntity
    {
        [Key]
        public long Id { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTimeOffset LastModifiedDate { get; set; }
        [JsonIgnore]
        protected Action<object, string> LazyLoader { get; set; }
    }
}

