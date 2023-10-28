using System.ComponentModel.DataAnnotations;

namespace Auth.Api.Models.DTOs;

public sealed class RegisterDto
{
    public required string UserName { get; set; }
    [DataType(DataType.Password)]
    public required string Password { get; set; }
    [DataType(DataType.EmailAddress)]
    public required string Email { get; set; }
}
