using System.ComponentModel.DataAnnotations;

namespace Auth.Api.Models.DTOs;

public sealed class LoginDto
{
    public required string UserName { get; set; }

    [DataType(DataType.Password)]
    public required string Password { get; set; }
}
