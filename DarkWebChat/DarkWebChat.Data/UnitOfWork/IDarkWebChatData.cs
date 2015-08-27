namespace DarkWebChat.Data.UnitOfWork
{
    using DarkWebChat.Data.Repositories;
    using DarkWebChat.Models;

    using Microsoft.AspNet.Identity;

    public interface IDarkWebChatData
    {
        IRepository<ApplicationUser> Users { get; }

        IRepository<Channel> Channels { get; }

        IRepository<ChannelMessage> ChannelMessages { get; }

        IRepository<UserMessage> UserMessages { get; }

        IRepository<Notification> Notifications { get; }

        IRepository<MessageContent> MessageContents { get; }

        IUserStore<ApplicationUser> UserStore { get; }

        void SaveChanges();
    }
}