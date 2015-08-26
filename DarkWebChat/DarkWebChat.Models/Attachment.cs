namespace DarkWebChat.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.IO;

    public class Attachment
    {
        public int Id { get; set; }

        [Required]
        public string File { get; set; }

        [Required]
        public string SenderId { get; set; }

        public virtual ApplicationUser Sender { get; set; }

        [Required]
        public string RecieverId { get; set; }

        public virtual ApplicationUser Reciever { get; set; }
    }
}
