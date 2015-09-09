namespace DarkWebChat.Web.Models.ViewModels
{
    using System;
    using System.Linq.Expressions;

    using DarkWebChat.Models;

    public class UserMessageViewModel
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public int ContainsFile { get; set; }

        public DateTime DateSent { get; set; }

        public string Sender { get; set; }

        public string Reciever { get; set; }

        public static Expression<Func<UserMessage, UserMessageViewModel>> Create
        {
            get
            {
                return
                    message =>
                    new UserMessageViewModel
                        {
                            DateSent = message.Date, 
                            Id = message.Id, 
                            ContainsFile = message.FileContent == null ? 0 : 1, 
                            Sender = message.Sender.UserName, 
                            Reciever = message.Reciever.UserName, 
                            Text = message.Text
                        };
            }
        }

        public static UserMessageViewModel CreateSingleView(UserMessage message)
        {
            return new UserMessageViewModel
                       {
                           DateSent = message.Date, 
                           Id = message.Id, 
                           ContainsFile = message.FileContent == null ? 0 : 1, 
                           Sender = message.Sender.UserName, 
                           Reciever = message.Reciever.UserName, 
                           Text = message.Text
                       };
        }
    }
}