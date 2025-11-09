using NoActivityFiler.Models;

namespace NoActivityFiler.Services;

public interface IN8nClient
{
    Task<GenerateResult> GenerateAsync(GenerateRequest request, CancellationToken ct = default);
}

