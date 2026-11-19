using System.ComponentModel.DataAnnotations;
namespace DocControl.Api.Models;

public class UpdateUserRequest
{
    [Required, StringLength(50)]
    public string UserName { get; set; } = "";
    
    [Required, StringLength(100)]
    public string FullName { get; set; } = "";

    [Required, StringLength(50)]
    public string Section { get; set; } = "";

    [Required, RegularExpression("^(EMPLOYEE|ADMIN|DCC|ADMR|AGMR)$", ErrorMessage = "Invalid role.")]
    public string Role { get; set; } = "";

    
}