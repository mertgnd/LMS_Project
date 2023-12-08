using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMS_Project.Data.ModelDb
{
    public class FacultyBuildingDbModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [MaxLength(150)]
        [Required]
        public string Name { get; set; }

        [MaxLength(150)]
        [Required]
        public string Description { get; set; }

        [MaxLength(150)]
        [Required]
        public string StreetNumber { get; set; }
        [Required]
        public Guid StreetId { get; set; }
        [ForeignKey("StreetId")]
        public StreetDbModel Street { get; set; }
    }
}