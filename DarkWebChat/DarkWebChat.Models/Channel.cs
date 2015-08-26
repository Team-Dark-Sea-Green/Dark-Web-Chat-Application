namespace DarkWebChat.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Channel
    {
        public Channel()
        {
            this.Users = new HashSet<ApplicationUser>();
            this.Messages = new HashSet<ChannelMessage>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }

        public virtual ICollection<ChannelMessage> Messages { get; set; }
    }
}
