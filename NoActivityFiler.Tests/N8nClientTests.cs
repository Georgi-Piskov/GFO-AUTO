using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.FileProviders;
using Moq;
using NoActivityFiler.Models;
using NoActivityFiler.Services;
using Xunit;

namespace NoActivityFiler.Tests;

public class N8nClientTests
{
    private class HandlerStub : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var payload = new { app11PdfBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes("app11")), zschPdfBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes("zsch")) };
            var json = JsonSerializer.Serialize(payload);
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(json, Encoding.UTF8, "application/json") });
        }
    }

    [Fact]
    public async Task GenerateAsync_HappyPath_SavesFiles()
    {
        var wwwroot = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(wwwroot);

        try
        {
            var env = Mock.Of<IWebHostEnvironment>(e => e.WebRootPath == wwwroot);
            var http = new HttpClient(new HandlerStub()) { BaseAddress = new Uri("https://example.com/") };
            var httpContextAccessor = new HttpContextAccessor { HttpContext = new DefaultHttpContext() };
            var client = new N8nClient(http, env, httpContextAccessor);

            var req = new GenerateRequest
            {
                Year = 2024,
                Company = new Company { Eik = "123456789", Name = "Пример", LegalForm = "ООД" },
                Declarant = new Declarant { FullName = "Иван Иванов", Position = "Управител" },
                Nsi = new NsiData { Ownership = "Private" },
                ZSch = new ZSchData { FirstPeriodNoActivity = true }
            };

            var result = await client.GenerateAsync(req);

            Assert.True(File.Exists(result.App11PdfPath));
            Assert.True(File.Exists(result.ZSchPdfPath));
            Assert.Contains("/tmp/", result.App11PdfUrl);
        }
        finally
        {
            if (Directory.Exists(wwwroot)) Directory.Delete(wwwroot, true);
        }
    }
}

