using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicDatabase.Models
{
    public class Song
    {
        [Key]
        public int SongId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        public int DurationSeconds { get; set; }

        public SongDetails? Details { get; set; }

        public int AlbumId { get; set; }
        public Album? Album { get; set; }

        public List<Artist> Artists { get; set; } = new();
    }
}
