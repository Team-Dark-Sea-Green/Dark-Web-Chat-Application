namespace DarkWebChat.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Channel
    {
        public Channel()
        {
            this.Users = new HashSet<ApplicationUser>();
            this.Messages = new HashSet<Message>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }
}
