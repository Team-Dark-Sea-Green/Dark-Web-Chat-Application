using System.Collections.Generic;
using DarkWebChat.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DarkWebChat.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<WebChatContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(WebChatContext context)
        {
            if (!context.Users.Any())
            {
                UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(context);
                UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(userStore);

                var userA = new ApplicationUser()
                {
                    Email = "richard@mail.com",
                    UserName = "Batman",
                    PasswordHash = new PasswordHasher().HashPassword("Password1!")
                };
                var userB = new ApplicationUser()
                {
                    Email = "william@mail.co.uk",
                    UserName = "Robin",
                    PasswordHash = new PasswordHasher().HashPassword("Password1!")
                };

                context.Users.Add(userA);
                context.Users.Add(userB);
                context.SaveChanges();

                userManager.UpdateSecurityStamp(userA.Id);
                userManager.UpdateSecurityStamp(userB.Id);
                context.SaveChanges();

                var messages = new List<ChannelMessage>()
                {
                    new ChannelMessage()
                    {
                        Content = "Wassup Bats!?",
                        Date = DateTime.Now,
                        Sender = context.Users.FirstOrDefault(u => u.UserName == "Robin"),
                    },
                    new ChannelMessage()
                    {
                        Content = "I'm Batman!",
                        Date = DateTime.Now,
                        Sender = context.Users.FirstOrDefault(u => u.UserName == "Batman"),
                    }
                };

                var channel = new Channel()
                {
                    Name = "Channel-1",
                    Users = context.Users.Where(u => u.UserName == "Batman" || u.UserName == "Robin").ToList(),
                    Messages = messages
                };

                context.Channels.Add(channel);
                context.SaveChanges();
            }
        }
    }
}
