using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public static class Extensions
{
    // Expensive. Use in editor only
    public static void SetDictionaryValue<TKey, TValue>(this SerializedDictionary<TKey, TValue> dict, Dictionary<TKey, TValue> dictToSet)
    {
        Type t = typeof(SerializedDictionary<TKey, TValue>);
        FieldInfo fieldInfo = t.GetField("_serializedList", BindingFlags.NonPublic | BindingFlags.Instance);

        List<SerializedKeyValuePair<TKey, TValue>> newSerializedList = new();
        foreach (var item in dictToSet)
            newSerializedList.Add(new(item.Key, item.Value));

        fieldInfo.SetValue(dict, newSerializedList);
    }
}
