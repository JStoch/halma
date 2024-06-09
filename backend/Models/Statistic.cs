using backend.Repositories;
using HalmaServer.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalmaWebApi.Models
{
    public class Statistic : IGetGuid, IPlayerAccessible
    {

        public Statistic()
        {
            StatisticGuid = Guid.NewGuid().ToString();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string StatisticGuid { get; set; }

        [ForeignKey("PlayerGuid")]
        public string PlayerGuid { get; set; }

        public PlayerModel Owner { get; set; }

        public int GamesPlayed { get; set; }

        public int GamesWon { get; set; }

        public double AvgWinRate { get; set; } //where score?

        public DateTime LastPlayedDate { get; set; }

        internal void UpdateLoss()
        {
            UpdateStatistic();
        }

        internal void UpdateWin()
        {
            GamesWon++;
            UpdateStatistic();

        }

        internal void UpdateStatistic()
        {
            GamesPlayed++;
            AvgWinRate = GamesWon / GamesPlayed;
            LastPlayedDate = DateTime.Now;
        }

        public string GetGuid()
        {
            return StatisticGuid;
        }

        public string GetPlayerGuid()
        {
            return this.PlayerGuid;
        }
    }
}
