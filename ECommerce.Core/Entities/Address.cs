using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Core.Entities
{
    public class Address : BaseEntity<int>
    {
        public string FristName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string AppUserId { get; set; }
        [ForeignKey("AppUserId")]
        public virtual AppUser AppUser { get; set; } // Navigation property
    }
}