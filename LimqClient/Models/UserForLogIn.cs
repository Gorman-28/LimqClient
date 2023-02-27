using System.ComponentModel.DataAnnotations;

namespace LimqClient.Models;

public class UserForLogIn
{
    [Required(ErrorMessage = "You must write UserName")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "You must write Password")]
    public string Password { get; set; }
}
