namespace DarkWebChat.RestServices.Models.BindingModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using DarkWebChat.Models;

    public class UserMessageBindingModel
    {
        [Required]
        public string Content { get; set; }

        [Required]
        public int IsFile { get; set; }
    }
}