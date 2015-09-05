namespace DarkWebChat.Web.Models.BindingModels
{
    using System.ComponentModel.DataAnnotations;

    public class UserMessageBindingModel
    {
        [Required]
        public string Content { get; set; }

        public string FileContent { get; set; }
    }
}