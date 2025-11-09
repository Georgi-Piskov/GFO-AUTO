using System.ComponentModel.DataAnnotations;

namespace NoActivityFiler.Models;

public class ZSchData
{
    [Display(Name = "Първи период без дейност")]
    public bool FirstPeriodNoActivity { get; set; } = true;
}

