using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tapStoryWebData.EF.Models
{
    public class UserFileGroup
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [Required]
        public int FileGroupId { get; set; }

        [ForeignKey("FileGroupId")]
        public virtual FileGroup FileGroup { get; set; }

    }
}
