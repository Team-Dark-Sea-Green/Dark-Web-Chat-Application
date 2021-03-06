﻿namespace DarkWebChat.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ChannelMessage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        public string FileContent { get; set; }

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