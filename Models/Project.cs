
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BE_Capstone.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }

        [Required]
        [StringLength(55, ErrorMessage = "Please shorten the project title to 55 characters")]
        public string Title { get; set; }

        [Required] 
        public ApplicationUser User { get; set; } 

        [Required]
        public string Description { get; set; }

        public IEnumerable<Scene> Scenes { get; set; }

        public ICollection<Character> Characters { get; set; }
     } 
}     