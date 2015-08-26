namespace DarkWebChat.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.IO;

    public class MessageContent
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Data { get; set; }

        [Required]
        public bool IsFile { get; set; }
    }
}
