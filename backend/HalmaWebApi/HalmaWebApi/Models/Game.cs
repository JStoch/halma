using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalmaWebApi.Models
{
    public class Game
    {
        [Key]
        public string Uid { get; set; }


        [ForeignKey("User")]
        public string Player1RefUid { get; set; }
        public User Player1 { get; set; }


        [ForeignKey("User")]
        public string Player2RefUid { get; set; }
        public User Player2 { get; set; }


    }
}
