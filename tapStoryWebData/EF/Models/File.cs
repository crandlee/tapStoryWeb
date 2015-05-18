using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tapStoryWebData.EF.Models
{
    public class File
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string FileName { get; set; }

        [Required]
        public FileType FileType { get; set; }

        [Required, MaxLength(2000)]
        public string FileLocation { get; set; }

        [Required]
        public int FileGroupId { get; set; }

        [ForeignKey("FileGroupId")]
        public virtual FileGroup FileGroup { get; set; }

    }
}
