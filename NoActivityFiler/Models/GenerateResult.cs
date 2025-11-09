namespace NoActivityFiler.Models;

public class GenerateResult
{
    public required string App11PdfUrl { get; set; }
    public required string ZSchPdfUrl { get; set; }

    public required string App11PdfPath { get; set; }
    public required string ZSchPdfPath { get; set; }

    public required string TempFolderVirtual { get; set; }
    public required string TempFolderPhysical { get; set; }
}

