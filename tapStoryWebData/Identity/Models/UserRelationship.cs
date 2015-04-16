using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tapStoryWebData.Identity.Models
{
    [Table("UserRelationship")]
    public class UserRelationship
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, ForeignKey("PrimaryMember")]
        public int PrimaryMemberId { get; set; }

        [Required, ForeignKey("SecondaryMember")]
        public int SecondaryMemberId { get; set; }

        [Required]
        public RelationshipType RelationshipType { get; set; }

        public virtual ApplicationUser PrimaryMember { get; set; }
        public virtual ApplicationUser SecondaryMember { get; set; }

    }
}
