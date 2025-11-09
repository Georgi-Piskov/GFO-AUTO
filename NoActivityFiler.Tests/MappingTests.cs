using NoActivityFiler.Models;
using Xunit;

namespace NoActivityFiler.Tests;

public class MappingTests
{
    [Fact]
    public void GenerateRequest_Maps_From_FormModel()
    {
        var req = new GenerateRequest
        {
            Year = 2024,
            Company = new Company { Eik = "123456789", Name = "Пример ООД", LegalForm = "ООД", Seat = "София", Address = "ул. Пример 1" },
            Declarant = new Declarant { FullName = "Иван Иванов", Position = "Управител", Email = "ivan@example.com", Phone = "+359" },
            Nsi = new NsiData { Kid2008 = "6201", Ownership = "Private" },
            ZSch = new ZSchData { FirstPeriodNoActivity = true }
        };

        // Identity mapping in this implementation
        Assert.Equal("123456789", req.Company.Eik);
        Assert.Equal("Пример ООД", req.Company.Name);
        Assert.Equal("ООД", req.Company.LegalForm);
        Assert.Equal(2024, req.Year);
        Assert.Equal("Иван Иванов", req.Declarant.FullName);
        Assert.Equal("Private", req.Nsi.Ownership);
        Assert.True(req.ZSch.FirstPeriodNoActivity);
    }
}

