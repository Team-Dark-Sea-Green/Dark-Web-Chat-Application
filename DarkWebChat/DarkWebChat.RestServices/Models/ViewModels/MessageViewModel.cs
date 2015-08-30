namespace DarkWebChat.RestServices.Models.ViewModels
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

        public bool IsFile { get; set; }

        public static Expression<Func<UserMessage, MessageViewModel>> Create
        {
            get
            {
                return message => new MessageViewModel()
                {
                    DateSent = message.Date,
                    Id = message.Id,
                    IsFile = message.Content.IsFile,
                    Sender = message.Sender.UserName,
                    Text = message.Content.Data
                };
            }
        }

        public static MessageViewModel CreateSingleView(UserMessage message)
        {
            return new MessageViewModel()
            {
                DateSent = message.Date,
                IsFile = message.Content.IsFile,
                Sender = message.Sender.UserName,
                Text = message.Content.Data
            };
        }
    }
}
