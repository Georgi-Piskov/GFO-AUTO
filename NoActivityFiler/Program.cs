using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.Extensions.Options;
using NoActivityFiler.Options;
using NoActivityFiler.Services;
using NoActivityFiler.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Options
builder.Services.Configure<N8nOptions>(builder.Configuration.GetSection("N8n"));
builder.Services.PostConfigure<N8nOptions>(opt =>
{
    var envUrl = Environment.GetEnvironmentVariable("N8N_WEBHOOK_URL");
    if (!string.IsNullOrWhiteSpace(envUrl))
    {
        opt.WebhookUrl = envUrl;
    }
});

// Services
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(8);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = false; // require consent
});
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true; // ask for consent for non-essential cookies
    options.MinimumSameSitePolicy = SameSiteMode.Lax;
});

builder.Services.AddSingleton<EmailTemplateService>();
builder.Services.AddSingleton<SessionJsonSerializer>();

builder.Services.AddHttpClient<IN8nClient, N8nClient>((sp, client) =>
{
    var opts = sp.GetRequiredService<IOptions<N8nOptions>>().Value;
    if (string.IsNullOrWhiteSpace(opts.WebhookUrl))
        throw new InvalidOperationException("N8n WebhookUrl is not configured");
    client.BaseAddress = new Uri(opts.WebhookUrl);
});

builder.Services.AddHostedService<TempFolderCleanupService>();

var app = builder.Build();

// Bind to provided PORT for PaaS (Fly.io/Koyeb/etc.)
var portEnv = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrWhiteSpace(portEnv))
{
    app.Urls.Add($"http://0.0.0.0:{portEnv}");
}

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();
app.UseRouting();
app.UseSession();

app.MapRazorPages();

app.Run();
