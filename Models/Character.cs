using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BE_Capstone.Models
{
    public class Character
    {
        [Key]
        public int CharacterId { get; set; }

        [Required]
        [StringLength(55, ErrorMessage = "Please shorten the name to 55 characters")]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int ProjectId {get; set;}

        public Project Project { get; set; }

     } 
}     