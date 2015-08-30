using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkWebChat.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class UserMessage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ContentId { get; set; }

        [Required]
        public string Data { get; set; }

        [Required]
        public bool IsFile { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string SenderId { get; set; }

        [ForeignKey("SenderId")]
        public virtual ApplicationUser Sender { get; set; }

        [Required]
        public string RecieverId { get; set; }

        [ForeignKey("RecieverId")]
        public virtual ApplicationUser Reciever { get; set; }
    }
}
