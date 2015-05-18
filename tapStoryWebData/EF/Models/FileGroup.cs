using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tapStoryWebData.EF.Models
{
    public class FileGroup
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public FileGroupType FileGroupType { get; set; }

        [Required, MaxLength(200)]
        public string GroupName { get; set; }

        public virtual ICollection<File> Files { get; set; }

    }
}
