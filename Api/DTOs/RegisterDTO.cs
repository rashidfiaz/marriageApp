using System;
using System.ComponentModel.DataAnnotations;

namespace Api.DTOs;

public class RegisterDTO
{
    [Required]
    public String Username { get; set; } = string.Empty;
    [Required] public string? Gender { get; set; }
    [Required] public string? KnownAs { get; set; } = string.Empty;
    [Required] public string? DateOfBirth { get; set; }
    [Required] public string? City { get; set; }
    [Required] public string? Country { get; set; }
    [Required]
    [StringLength(8, MinimumLength = 4)]
    public String Password { get; set; } = string.Empty;

}
