namespace DarkWebChat.RestServices.Models.ViewModels
{
    using System;

    public class MessageViewModel
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime DateSent { get; set; }

        public string Sender { get; set; }

        public int IsFile { get; set; }
    }
}
