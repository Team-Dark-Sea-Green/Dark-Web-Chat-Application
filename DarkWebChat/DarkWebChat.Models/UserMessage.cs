﻿namespace DarkWebChat.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class UserMessage
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

        [ForeignKey("SenderId")]
        public virtual ApplicationUser Sender { get; set; }

        [Required]
        public string RecieverId { get; set; }

        [ForeignKey("RecieverId")]
        public virtual ApplicationUser Reciever { get; set; }
    }
}