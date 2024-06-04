using backend.Repositories;
using HalmaServer.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalmaWebApi.Models
{
    public class GameHistory : IGetGuid
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string GameHistoryGuid { get; set; }


        [ForeignKey("GameModelGuid")]
        public virtual GameModel? GameModel { get; set; }

        public GameHistory()
        {
            GameHistoryGuid = Guid.NewGuid().ToString();
        }

        public string GetGuid()
        {
            return GameHistoryGuid;
        }
    }
}
