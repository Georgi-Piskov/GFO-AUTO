using System.ComponentModel.DataAnnotations;

namespace NoActivityFiler.Models;

public class NsiData
{
    [Display(Name = "КИД-2008 (по желание)")]
    public string? Kid2008 { get; set; }

    [Required(ErrorMessage = "Собственост е задължително поле.")]
    [Display(Name = "Собственост")]
    public string Ownership { get; set; } = string.Empty; // Private, State, Municipal, Mixed
}

