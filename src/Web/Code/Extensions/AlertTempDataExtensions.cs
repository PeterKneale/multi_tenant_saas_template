namespace Web.Code.Extensions;

public static class AlertTempDataExtensions
{
    private const string Key = "Alerts";

    public static bool HasAlerts(this ITempDataDictionary dictionary)
    {
        return dictionary.ContainsKey(Key);
    }

    public static void AddAlert(this ITempDataDictionary dictionary, Alert alert)
    {
        if (dictionary.HasAlerts())
        {
            // Append new alert to existing alerts
            var existing = dictionary.GetAlerts();
            var alerts = existing.Append(alert).ToArray();
            dictionary[Key] = JsonConvert.SerializeObject(alerts);
        }
        else
        {
            // Add the first alert
            var alerts = new[] { alert };
            dictionary.Add(Key, JsonConvert.SerializeObject(alerts));
        }
    }

    public static IEnumerable<Alert> GetAlerts(this ITempDataDictionary dictionary)
    {
        if (dictionary.ContainsKey(Key) == false) throw new Exception("No alerts");

        var json = dictionary[Key].ToString();

        return JsonConvert.DeserializeObject<IEnumerable<Alert>>(json);
    }
}