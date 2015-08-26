namespace DarkWebChat.ConsoleClient
{
    using System;
    using System.Linq;
    using Data;

    class Program
    {
        static void Main(string[] args)
        {
            var context = new WebChatContext();
            Console.WriteLine(context.UserMessages.Count());
        }
    }
}
