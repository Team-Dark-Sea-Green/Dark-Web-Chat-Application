using System.ComponentModel.DataAnnotations;

namespace DarkWebChat.RestServices.Models.BindingModels
{
    public class ChannelBindingModel
    {
        [Required]
        [MaxLength(60)]
        [MinLength(1)]
        public string Name { get; set; }
    }
}