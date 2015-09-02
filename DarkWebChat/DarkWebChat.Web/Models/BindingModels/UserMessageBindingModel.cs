namespace DarkWebChat.Web.Models.BindingModels
{
    using System.ComponentModel.DataAnnotations;

    public class UserMessageBindingModel
    {
        [Required]
        public string Content { get; set; }

        [Required]
        public int IsFile { get; set; }
    }
}