namespace DarkWebChat.Web.Models.ViewModels
{
    using System;
    using System.Linq.Expressions;

    using DarkWebChat.Models;

    public class ChannelMessageViewModel
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public int ContainsFile { get; set; }

        public DateTime DateSent { get; set; }

        public string Sender { get; set; }

        public static Expression<Func<ChannelMessage, ChannelMessageViewModel>> Create
        {
            get
            {
                return
                    message =>
                    new ChannelMessageViewModel
                        {
                            DateSent = message.Date, 
                            Id = message.Id, 
                            ContainsFile = message.FileContent == null ? 0 : 1, 
                            Sender = message.Sender.UserName, 
                            Text = message.Text
                        };
            }
        }

        public static ChannelMessageViewModel CreateSingleView(ChannelMessage message)
        {
            return new ChannelMessageViewModel
                       {
                           DateSent = message.Date, 
                           Id = message.Id, 
                           ContainsFile = message.FileContent == null ? 0 : 1, 
                           Sender = message.Sender.UserName, 
                           Text = message.Text
                       };
        }
    }
}