using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicDatabase.Models
{
    public class SongDetails
    {
        [Key]
        [ForeignKey("Song")]
        public int SongId { get; set; }

        [MaxLength(200)]
        public string? LyricsExcerpt { get; set; }

        public DateTime? RecordingDate { get; set; }

        [MaxLength(100)]
        public string? RecordingStudio { get; set; }

        public int? Bitrate { get; set; }

        public Song? Song { get; set; }
    }
}
