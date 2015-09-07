namespace DarkWebChat.Web.Models.ViewModels
{
    using System;
    using System.Linq.Expressions;

    using DarkWebChat.Models;

    public class MessageViewModel
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime DateSent { get; set; }

        public string Sender { get; set; }

        public string Reciever { get; set; }

        public static Expression<Func<UserMessage, MessageViewModel>> Create
        {
            get
            {
                return
                    message =>
                    new MessageViewModel
                        {
                            DateSent = message.Date, 
                            Id = message.Id, 
                            Sender = message.Sender.UserName, 
                            Reciever = message.Reciever.UserName, 
                            Text = message.Text
                        };
            }
        }

        public static MessageViewModel CreateSingleView(UserMessage message)
        {
            return new MessageViewModel
                       {
                           DateSent = message.Date,
                           Text = message.Text,
                           Sender = message.Sender.UserName,
                           Reciever = message.Reciever.UserName,
                       };
        }
    }
}