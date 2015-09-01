namespace DarkWebChat.RestServices.Models.BindingModels
{
    using System.ComponentModel.DataAnnotations;

    public class ChannelMessageBindingModel
    {
        [Required]
        public string Content { get; set; }

        [Required]
        public int IsFile { get; set; }
    }
}