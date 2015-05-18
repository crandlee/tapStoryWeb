using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tapStoryWebData.EF.Models
{
    public class UserRelationship
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Index("IX_PrimarySecondary", 1, IsUnique = true)]
        public int PrimaryMemberId { get; set; }

        [Required]
        [Index("IX_PrimarySecondary", 2, IsUnique = true)]
        public int SecondaryMemberId { get; set; }

        [Required]
        public RelationshipType RelationshipType { get; set; }

        [ForeignKey("PrimaryMemberId")]
        public virtual ApplicationUser PrimaryMember { get; set; }

        [ForeignKey("SecondaryMemberId")]
        public virtual ApplicationUser SecondaryMember { get; set; }

    }
}
