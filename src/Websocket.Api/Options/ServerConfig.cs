using System.ComponentModel.DataAnnotations;
using Websocket.Api.Common;

namespace Websocket.Api.Options;

public sealed class ServerConfig
{
    public const string Name = "ServerConfig";

    [Required]
    [EnumDataType(typeof(ServerType))]
    public required ServerType ServerType { get; set; }

    [Required]
    public required string IPAddress { get; set; }

    [Required]
    public required int Port { get; set; }
}
