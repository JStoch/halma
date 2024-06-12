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
        public string Guid { get; set; }


        [ForeignKey("GameModelGuid")]
        public virtual GameModel? GameModel { get; set; }

        public GameHistory()
        {
            Guid = System.Guid.NewGuid().ToString();
        }

    }
}
