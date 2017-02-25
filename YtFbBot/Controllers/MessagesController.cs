using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace YtFbBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));

                Activity reply = activity.CreateReply();

                var client = new HttpClient() { BaseAddress = new Uri("http://api.giphy.com") };
                var result = client.GetStringAsync("/v1/gifs/random?api_key=dc6zaTOxFJmzC&tag=dog").Result;
                var gif = ((dynamic)JObject.Parse(result)).data;
                var gifUrl = gif.image_url;

                reply.Attachments = new List<Attachment>();
                reply.Attachments.Add(new Attachment()
                {
                    ContentUrl = gifUrl,
                    ContentType = "image/gif",
                    Name = "dog.gif"
                });

                await connector.Conversations.ReplyToActivityAsync(reply);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }
        
    }
}