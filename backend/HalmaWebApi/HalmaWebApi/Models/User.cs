using HalmaWebApi.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace HalmaWebApi.Models
{
    [ValidateModel]
    public class User : IdentityUser
    {
        
        public string? Guid { get { return Id; } private set { } }

        public bool IsLoggedIn { get; set; }

        public DateTime RegistrationDate { get; set; }

        [ForeignKey("Statistic")]
        public int? StatisticId { get; set; }

        [ForeignKey("StatisticId")]
        public virtual Statistic? Statistic { get; set; }

        public virtual ICollection<User>? FriendsList { get; set; }
    }
}
