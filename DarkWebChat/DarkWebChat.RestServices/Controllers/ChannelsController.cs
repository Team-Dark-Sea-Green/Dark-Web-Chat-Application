using System.Linq;
using System.Net;
using DarkWebChat.Models;
using DarkWebChat.RestServices.Models.ViewModels;

namespace DarkWebChat.RestServices.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Security.Authentication.ExtendedProtection;
    using System.Web.Http;

    using DarkWebChat.Data.UnitOfWork;

    using Models.BindingModels;

    [RoutePrefix("api")]
    public class ChannelsController : BaseApiController
    {
        public ChannelsController()
            : base(new DarkWebChatData())
        {
        }

        // GET api/channels
        [Route("channels")]
        public IHttpActionResult GetAllChannels()
        {
            var allChannels = this.Data.Channels.All()
                .OrderBy(c => c.Name)
                .Select(c => new
                {
                    c.Id,
                    c.Name
                });

            return this.Ok(allChannels);
        }

        // GET api/channels/{id}
        [Route("channels/{id}")]
        public IHttpActionResult GetChannel(int id)
        {
            var channel = this.Data.Channels.Find(id);
            if (channel == null)
            {
                return this.NotFound();
            }

            return this.Ok(new ChannelViewModel()
            {
                Id = channel.Id,
                Name = channel.Name
            });
        }

        // POST api/channels
        [Route("channels")]
        public IHttpActionResult CreateChannel(ChannelBindingModel model)
        {
            if (model == null)
            {
                return BadRequest("Empty channel data!");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (this.Data.Channels.All().Any(c => c.Name == model.Name))
            {
                this.Content(HttpStatusCode.Conflict,
                    new {Message = "Duplicated channel name: " + model.Name});
            }

            var channel = new Channel()
            {
                Name = model.Name,
            };
            this.Data.Channels.Add(channel);
            this.Data.SaveChanges();

            return this.Ok(new ChannelViewModel()
            {
                Id = channel.Id,
                Name = channel.Name   
            });
        }

        //PUT api/channels/adduser
        [HttpPut]
        [Route("channels/adduser")]
        public IHttpActionResult AddUserToChannel(AddUserToChannelBindingModel model)
        {
            if (model == null)
            {
                return BadRequest("Invalid data!");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var channel = this.Data.Channels.Find(model.channelId);
            if (channel == null)
            {
                return BadRequest("There is no such channel!");
            }

            var user = this.Data.Users.Find(model.userlId);
            if (user == null)
            {
                return BadRequest("There is no such user!");
            }

            channel.Users.Add(user);
            this.Data.Channels.Update(channel);
            this.Data.SaveChanges();

            return this.Ok(new ChannelViewModel()
            {
                Id = channel.Id,
                Name = channel.Name,
                Users = channel.Users
            });
        }

        // DELETE api/channels/{id}
        [HttpDelete]
        [Route("channels/{id}")]
        public IHttpActionResult DeleteChannel(int id)
        {
            Channel channel = this.Data.Channels.Find(id);
            if (channel == null)
            {
                return BadRequest("There is no such channel!");
            }

            this.Data.Channels.Remove(channel);
            this.Data.SaveChanges();

            return Ok(new
            {
                Message = "Channel " + channel.Name + " deleted!"
            });
        }
    }
}
