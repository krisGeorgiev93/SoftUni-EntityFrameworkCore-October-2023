using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theatre.Common;
using Theatre.Data.Models.Enums;

namespace Theatre.Data.Models
{
    
        public class Play
        {
            public Play()
            {
                Tickets = new HashSet<Ticket>();
                Casts = new HashSet<Cast>();
            }

            [Key]
            public int Id { get; set; }

            [Required]
            [MaxLength(ValidationConstants.TitleMaxLength)]
            public string Title { get; set; } = null!;

            [Required]
            public TimeSpan Duration { get; set; }

            public float Rating { get; set; }

            public Genre Genre { get; set; }

            [Required]
            [MaxLength(ValidationConstants.DescriptionMaxValue)]
            public string Description { get; set; } = null!;

            [Required]
            [MaxLength(ValidationConstants.ScreenwriterMaxLength)]
            public string Screenwriter { get; set; } = null!;

            public virtual ICollection<Cast> Casts { get; set; }

            public virtual ICollection<Ticket> Tickets { get; set; }


        }
}
