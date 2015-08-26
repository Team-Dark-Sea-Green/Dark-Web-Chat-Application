namespace DarkWebChat.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Message
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string SenderId { get; set; }

        public virtual ApplicationUser Sender { get; set; }

        [Required]
        public string RecieverId { get; set; }

        public virtual ApplicationUser Reciever { get; set; }

        public int? ChannelId { get; set; }

        public virtual Channel Channel { get; set; }
    }
}
