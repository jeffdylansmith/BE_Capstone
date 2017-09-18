
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BE_Capstone.Models
{
    public class Direction
    {
        [Key]
        public int DirectionId { get; set; }

        [Required]
        public int Order { get; set; }
        
        [Required]
        public string Body { get; set; }

        [Required]
        public int SceneId {get; set;}

        public Scene Scene { get; set; }
     } 
}     