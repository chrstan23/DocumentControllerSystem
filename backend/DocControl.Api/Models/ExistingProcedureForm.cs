using System.ComponentModel.DataAnnotations;

namespace DocControl.Api.Models;

public class ExistingProcedureForm
{
    [Required, StringLength(30)]
    public string DocNo { get; set; } = "";

    [Required, StringLength(200)]
    public string DocTitle { get; set; } = "";
    
    [Required, StringLength(50)]
    public string DocType { get; set; } = "";
    
    [Required, StringLength(50)]
    public string Section { get; set; } = "";
    
    [Required, RegularExpression(@"^\d{2}$", ErrorMessage = "Revision must be 2 digits.")]
    public string Revision { get; set; } = "";

    [Required]
    public DateOnly? EffectivityDate { get; set; }

    [Required, StringLength(100)]
    public string PreparedBy { get; set; } = "";

    [Required]
    public IFormFile? File { get; set; }
    
    
    
}