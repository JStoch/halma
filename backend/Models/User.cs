using backend.Repositories;
using HalmaWebApi.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace HalmaWebApi.Models
{
    [ValidateModel]
    public class User : IdentityUser, IGetGuid
    {
        [NotMapped]
        public string Guid { get { return Id; } set { } }

        public bool IsLoggedIn { get; set; }

        public DateTime RegistrationDate { get; set; }

        

        public virtual ICollection<User>? FriendsList { get; set; }

        public string GetGuid()
        {
            return Id;
        }
    }
}
