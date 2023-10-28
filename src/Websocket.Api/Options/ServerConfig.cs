using System.ComponentModel.DataAnnotations;
using Websocket.Api.Common;

namespace Websocket.Api.Options
{
    public class ServerConfig
    {
        public const string Name = "ServerConfig";

        [Required]
        [EnumDataType(typeof(ServerType))]
        public ServerType ServerType { get; set; }

        [Required]
        public string IPAddress { get; set; }
        
        [Required]
        public int Port { get; set; }
    }
}
