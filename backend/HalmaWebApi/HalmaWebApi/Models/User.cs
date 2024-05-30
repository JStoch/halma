using HalmaWebApi.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalmaWebApi.Models
{
    [ValidateModel]
    public class User : IdentityUser
    {

        //public bool IsLoggedIn { get; set; }

        //public DateTime RegistrationDate { get; set; }

        //public ICollection<User> FriendsList { get; set; }

        //[ForeignKey("Statistic")]
        //public int StatisticRefId { get; set; }
        //public Statistic Statistic { get; set; }

        [Key]
        public string Guid { get; set; }
        [Required]
        public string UserName { get; set; }

        [Required]
        public string UserEmail { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; }

        public bool IsLoggedIn { get; set; }

        public DateTime RegistrastionDate { get; set; }

        public ICollection<User> FriendsList { get; set; }

        [ForeignKey("Statistic")]
        public int StatisticRefId { get; set; }
        public Statistic Statistic { get; set; }
    }
}
