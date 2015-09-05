﻿namespace DarkWebChat.Web.Models.ViewModels
{
    using System;
    using System.Linq.Expressions;

    using DarkWebChat.Models;

    public class MessageViewModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime DateSent { get; set; }

        public string Sender { get; set; }

        public string Reciver { get; set; }

        public bool IsFile { get; set; }

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
                            IsFile = message.IsFile, 
                            Sender = message.Sender.UserName, 
                            Reciver = message.Reciever.UserName, 
                            Content = message.Content
                        };
            }
        }

        public static MessageViewModel CreateSingleView(UserMessage message)
        {
            return new MessageViewModel
                       {
                           DateSent = message.Date, 
                           IsFile = message.IsFile, 
                           Sender = message.Sender.UserName,
                           Reciver = message.Reciever.UserName,
                           Content = message.Content
                       };
        }
    }
}