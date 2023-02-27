using System.ComponentModel.DataAnnotations;

namespace LimqClient.Models;
public class User
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "You must write UserName")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "You must write Password")]
    public string Password { get; set; }

    [Required(ErrorMessage = "You must write FirstName")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "You must write LastName")]
    public string LastName { get; set; }

#pragma warning disable CA1819 // Properties should not return arrays
    public byte[] Avatar { get; set; } = new byte[0];
#pragma warning restore CA1819 // Properties should not return arrays
    
    public bool Status { get; set; }

}
