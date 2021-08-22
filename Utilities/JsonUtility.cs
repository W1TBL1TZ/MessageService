using System.Text.Json;

namespace MessageService.Utilities
{
    // Just an example of a reusable utility for json manipulation
    // This example is obviously not needed because it is still just one line, but this is 
    // the kind of thing I would extract into a reusable component like a nuget package
    public static class JsonUtility
    {
        public static T Deserialize<T>(this string json)
        {
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
