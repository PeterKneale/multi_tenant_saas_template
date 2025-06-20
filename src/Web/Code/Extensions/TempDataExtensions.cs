﻿namespace Web.Code.Extensions;

public static class TempDataExtensions
{
    public static void Set<T>(this ITempDataDictionary tempData, string key, T value) where T : class
    {
        tempData[key] = JsonConvert.SerializeObject(value);
    }

    public static T? Get<T>(this ITempDataDictionary tempData, string key) where T : class
    {
        tempData.TryGetValue(key, out var value);
        return value == null ? null : JsonConvert.DeserializeObject<T>((string)value);
    }
}