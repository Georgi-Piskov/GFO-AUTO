using System.ComponentModel.DataAnnotations;
using NoActivityFiler.Validators;

namespace NoActivityFiler.Models;

public class Company
{
    [Required(ErrorMessage = "ЕИК е задължително поле.")]
    [RegularExpression("^\\d{9}(?:\\d{4})?$", ErrorMessage = "ЕИК трябва да е 9 или 13 цифри.")]
    [Eik]
    public string Eik { get; set; } = string.Empty;

    [Required(ErrorMessage = "Наименование е задължително поле.")]
    [Display(Name = "Наименование на предприятието")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Правна форма е задължително поле.")]
    [Display(Name = "Правна форма")]
    public string LegalForm { get; set; } = string.Empty;

    [Display(Name = "Седалище (град/село)")]
    public string? Seat { get; set; }

    [Display(Name = "Адрес на управление")]
    public string? Address { get; set; }

    [Display(Name = "Област")]
    public string? Region { get; set; }

    [Display(Name = "Община")]
    public string? Municipality { get; set; }
}

