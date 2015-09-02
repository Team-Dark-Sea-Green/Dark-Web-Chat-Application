namespace DarkWebChat.Web.Models.ViewModels
{
    using System.Collections.Generic;

    using DarkWebChat.Models;

    public class ChannelViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }
    }
}