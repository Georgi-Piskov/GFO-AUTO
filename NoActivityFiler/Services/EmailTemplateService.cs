using System.Text;
using NoActivityFiler.Models;

namespace NoActivityFiler.Services;

public class EmailTemplateService
{
    public string BuildNsiEmailBody(GenerateRequest model)
    {
        var sb = new StringBuilder();
        sb.AppendLine("До: Териториално статистическо бюро на НСИ");
        sb.AppendLine();
        sb.AppendLine($"Относно: Приложение №11 – предприятие без дейност за {model.Year} г.");
        sb.AppendLine();
        sb.AppendLine("Уважаеми госпожи и господа,");
        sb.AppendLine($"Представям Приложение №11 за {model.Year} г. за предприятието без дейност.");
        sb.AppendLine();
        sb.AppendLine($"ЕИК: {model.Company.Eik}");
        sb.AppendLine($"Наименование: {model.Company.Name}");
        sb.AppendLine($"Правна форма: {model.Company.LegalForm}");
        if (!string.IsNullOrWhiteSpace(model.Company.Seat)) sb.AppendLine($"Седалище: {model.Company.Seat}");
        if (!string.IsNullOrWhiteSpace(model.Company.Address)) sb.AppendLine($"Адрес: {model.Company.Address}");
        if (!string.IsNullOrWhiteSpace(model.Nsi.Kid2008)) sb.AppendLine($"КИД-2008: {model.Nsi.Kid2008}");
        sb.AppendLine($"Собственост: {model.Nsi.Ownership}");
        sb.AppendLine();
        sb.AppendLine($"Декларатор: {model.Declarant.FullName} – {model.Declarant.Position}");
        if (!string.IsNullOrWhiteSpace(model.Declarant.Phone)) sb.AppendLine($"Телефон: {model.Declarant.Phone}");
        if (!string.IsNullOrWhiteSpace(model.Declarant.Email)) sb.AppendLine($"E-mail: {model.Declarant.Email}");
        sb.AppendLine();
        sb.AppendLine("Прикачвам подписаните PDF документи.");
        sb.AppendLine();
        sb.AppendLine("С уважение,");
        sb.AppendLine(model.Declarant.FullName);
        return sb.ToString();
    }
}

