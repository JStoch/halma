using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalmaWebApi.Models
{
    public class Statistic
    {
        [Key]
        public int Id { get; set; }

        public int GamesPlayed { get; set; }

        public int GamesWon { get; set; }

        public double AvgScore { get; set; }
        public double HighScore { get; set; }

        public DateTime LastPlayedDate { get; set; }

    }
}
