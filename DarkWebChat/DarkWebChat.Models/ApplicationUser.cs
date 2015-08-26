namespace DarkWebChat.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class ApplicationUser : IdentityUser
    {
        private ICollection<UserMessage> userMessages;

        public ApplicationUser()
        {
            this.UserMessages = new HashSet<UserMessage>();
        }

        [InverseProperty("Reciever")]
        public virtual ICollection<UserMessage> UserMessages
        {
            get { return this.userMessages; }
            set { this.userMessages = value; }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(
            UserManager<ApplicationUser> manager,
            string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(
                this,
                authenticationType);

            return userIdentity;
        }
    }
}
