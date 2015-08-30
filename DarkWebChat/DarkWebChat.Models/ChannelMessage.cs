using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkWebChat.Models;

namespace DarkWebChat.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ChannelMessage
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

        public virtual ApplicationUser Sender { get; set; }

        [Required]
        [ForeignKey("Channel")]
        public int ChannelId { get; set; }

        public virtual Channel Channel { get; set; }
    }
}
