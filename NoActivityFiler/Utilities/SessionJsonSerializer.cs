using System.Text.Json;

namespace NoActivityFiler.Utilities;

public class SessionJsonSerializer
{
    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web);

    public void Set<T>(ISession session, string key, T value)
    {
        session.SetString(key, JsonSerializer.Serialize(value, Options));
    }

    public T? Get<T>(ISession session, string key)
    {
        var s = session.GetString(key);
        return s == null ? default : JsonSerializer.Deserialize<T>(s, Options);
    }
}

