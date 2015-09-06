namespace DarkWebChat.Web.Models.BindingModels
{
    using System.ComponentModel.DataAnnotations;

    public class ChannelMessageBindingModel
    {
        [Required]
        public string Text { get; set; }

        public string FileContent { get; set; }
    }
}