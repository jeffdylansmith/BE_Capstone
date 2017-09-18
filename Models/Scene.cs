
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BE_Capstone.Models
{
    public class Scene
    {
        [Key]
        public int SceneId { get; set; }

        [Required]
        [StringLength(55, ErrorMessage = "Please shorten the title to 55 characters")]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public int Order { get; set; }

        public string Body { get; set; }

        public int ProjectId {get; set;}

        public Project Project { get; set; }

        public ICollection<Character> Characters { get; set; }

        public ICollection<Line> Lines { get; set; }

        public ICollection<Direction> Directions { get; set; }
    } 
}     