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
        }

        public virtual IDbSet<Message> Messages { get; set; }

        public virtual IDbSet<Notification> Notifications { get; set; }

        public virtual IDbSet<Channel> Channels { get; set; }

        public virtual IDbSet<Attachment> Attachments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>()
                .HasRequired<ApplicationUser>(m => m.Reciever)
                .WithMany(u => u.RecievedMessages)
                .HasForeignKey(m => m.RecieverId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Message>()
                .HasRequired<ApplicationUser>(m => m.Sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(m => m.SenderId)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }

        public static WebChatContext Create()
        {
            return new WebChatContext();
        }
    }
}