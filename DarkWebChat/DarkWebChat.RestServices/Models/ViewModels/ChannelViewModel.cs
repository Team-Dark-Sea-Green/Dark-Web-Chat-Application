using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DarkWebChat.Models;

namespace DarkWebChat.RestServices.Models.ViewModels
{
    public class ChannelViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }
    }
}