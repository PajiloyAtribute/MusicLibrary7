using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicDatabase.Models
{
    public class Artist
    {
        [Key]
        public int ArtistId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Country { get; set; }

        public DateTime? BirthDate { get; set; }

        // image file name stored
        [MaxLength(260)]
        public string? PhotoFileName { get; set; }

        public List<Album> Albums { get; set; } = new();

        public List<Song> Songs { get; set; } = new();
    }
}
