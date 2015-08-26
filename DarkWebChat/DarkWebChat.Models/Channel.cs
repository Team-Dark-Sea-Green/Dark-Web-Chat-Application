namespace DarkWebChat.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Channel
    {
        private ICollection<ApplicationUser> users;
        private ICollection<ChannelMessage> messages;

        public Channel()
        {
            this.Users = new HashSet<ApplicationUser>();
            this.Messages = new HashSet<ChannelMessage>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<ApplicationUser> Users 
        {
            get
            {
                return this.users;
            }
            set
            {
                this.users = value;
            }
        }

        public virtual ICollection<ChannelMessage> Messages
        {
            get { return this.messages; }
            set { this.messages = value; }
        }
    }
}
