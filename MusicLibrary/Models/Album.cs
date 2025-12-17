using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicDatabase.Models
{
    public class Album
    {
        [Key]
        public int AlbumId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        public DateTime ReleaseDate { get; set; }

        [MaxLength(50)]
        public string? Genre { get; set; }

        public List<Song> Songs { get; set; } = new();

        public int ArtistId { get; set; }
        public Artist? Artist { get; set; }
    }
}
