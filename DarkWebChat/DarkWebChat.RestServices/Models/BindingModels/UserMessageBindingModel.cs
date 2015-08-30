namespace DarkWebChat.RestServices.Models.BindingModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using DarkWebChat.Models;

    public class UserMessageBindingModel
    {
        [Required]
        public int ContentId { get; set; }

        [Required]
        public string Data { get; set; }

        [Required]
        public bool IsFile { get; set; }

        [Required]
        public string SenderId { get; set; }

        [Required]
        public string RecieverId { get; set; }
    }
}