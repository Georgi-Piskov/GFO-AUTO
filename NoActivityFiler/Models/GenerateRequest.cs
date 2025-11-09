using System.ComponentModel.DataAnnotations;

namespace NoActivityFiler.Models;

public class GenerateRequest
{
    [Required]
    public Company Company { get; set; } = new();

    [Required]
    public Declarant Declarant { get; set; } = new();

    [Required]
    public NsiData Nsi { get; set; } = new();

    [Required]
    public ZSchData ZSch { get; set; } = new();

    [Required]
    [Range(2000, 2100, ErrorMessage = "Годината трябва да е между 2000 и 2100.")]
    [Display(Name = "Година")]
    public int Year { get; set; }
}

