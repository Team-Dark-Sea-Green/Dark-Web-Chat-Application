namespace DarkWebChat.Data
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Migrations;
    using Models;

    public class WebChatContext : IdentityDbContext<ApplicationUser>
    {
        public WebChatContext()
            : base("name=WebChatContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<WebChatContext, Configuration>());
            //Database.SetInitializer(new DropCreateDatabaseAlways<WebChatContext>());
        }

        public virtual IDbSet<UserMessage> UserMessages { get; set; }

        public virtual IDbSet<ChannelMessage> ChannelMessages { get; set; }

        public virtual IDbSet<Notification> Notifications { get; set; }

        public virtual IDbSet<Channel> Channels { get; set; }

        public virtual IDbSet<MessageContent> MessageContents { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserMessage>()
                .HasRequired<ApplicationUser>(m => m.Sender)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserMessage>()
                .HasRequired<ApplicationUser>(m => m.Reciever)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ChannelMessage>()
                .HasRequired<ApplicationUser>(m => m.Sender)
                .WithMany()
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }

        public static WebChatContext Create()
        {
            return new WebChatContext();
        }
    }
}