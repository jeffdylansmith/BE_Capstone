
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BE_Capstone.Models
{
    public class Line
    {
        [Key]
        public int LineId { get; set; }

        [Required]
        public string Description { get; set; }
        
        public int Order { get; set; }

        [Required]
        public string Body { get; set; }
        
        public int CharacterId { get; set; }

        public Character Character { get; set; }

        public int SceneId {get; set;}

        public Scene Scene { get; set; }

        public int ProjectId {get; set;}

        public Project Project { get; set; }
     } 
}     