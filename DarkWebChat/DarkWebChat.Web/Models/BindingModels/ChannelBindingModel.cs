namespace DarkWebChat.Web.Models.BindingModels
{
    using System.ComponentModel.DataAnnotations;

    public class ChannelBindingModel
    {
        [Required]
        [MaxLength(60)]
        [MinLength(1)]
        public string Name { get; set; }
    }
}