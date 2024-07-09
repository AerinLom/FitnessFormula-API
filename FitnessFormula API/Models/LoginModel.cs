using System.ComponentModel.DataAnnotations;

public class LoginModel
{
    [Required(ErrorMessage = "Username is required")] // Specifies that Username property is required
    public string Username { get; set; }

    [Required(ErrorMessage = "Password is required")] // Specifies that Password property is required
    public string Password { get; set; }
}
