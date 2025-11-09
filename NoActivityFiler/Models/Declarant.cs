using System.ComponentModel.DataAnnotations;

namespace NoActivityFiler.Models;

public class Declarant
{
    [Required(ErrorMessage = "Трите имена са задължителни.")]
    [Display(Name = "Декларатор - три имена")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Длъжност е задължително поле.")]
    [Display(Name = "Длъжност")]
    public string Position { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Невалиден e-mail адрес.")]
    [Display(Name = "E-mail")]
    public string? Email { get; set; }

    [Display(Name = "Телефон")]
    public string? Phone { get; set; }
}

