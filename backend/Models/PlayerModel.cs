using backend.Repositories;
using HalmaWebApi.Models;
using Humanizer;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalmaServer.Models {

    public class PlayerModel : IGetGuid {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string PlayerGuid {get; set;}
        public string ConnectionId {get; set;}

        [ForeignKey("UserGuid")]
        public User User { get; set;}


        public PlayerModel(string playerGuid, string connectionId) {
            PlayerGuid = playerGuid;
            ConnectionId = connectionId;
        }

        public PlayerModel(string connectionId) {
            PlayerGuid = Guid.NewGuid().ToString();
            ConnectionId = connectionId;
        }

        public string GetGuid()
        {
            return PlayerGuid;
        }
    }
}