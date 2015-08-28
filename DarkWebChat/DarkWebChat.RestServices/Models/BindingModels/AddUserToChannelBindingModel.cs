using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DarkWebChat.RestServices.Models.BindingModels
{
    public class AddUserToChannelBindingModel
    {
        [Required]
        public int channelId { get; set; }
        [Required]
        public string userlId { get; set; }
    }
}