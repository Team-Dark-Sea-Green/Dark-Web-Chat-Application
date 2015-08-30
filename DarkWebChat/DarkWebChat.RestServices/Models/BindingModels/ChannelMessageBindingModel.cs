namespace DarkWebChat.RestServices.Models.BindingModels
{
    using System.ComponentModel.DataAnnotations;

    public class ChannelMessageBindingModel
    {
        [Required]
        public string Data { get; set; }

        [Required]
        public bool IsFile { get; set; }
    }
}