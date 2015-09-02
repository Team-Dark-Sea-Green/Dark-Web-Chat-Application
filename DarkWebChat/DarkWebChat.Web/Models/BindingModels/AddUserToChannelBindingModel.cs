namespace DarkWebChat.Web.Models.BindingModels
{
    using System.ComponentModel.DataAnnotations;

    public class AddUserToChannelBindingModel
    {
        [Required]
        public int channelId { get; set; }

        [Required]
        public string userlId { get; set; }
    }
}