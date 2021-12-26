using System;
using System.Collections.Generic;

namespace Application.Commons.Extensions
{
    public static class DictionaryExtensions
    {
        public static void AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> valueFactory, Func<TKey, TValue, TValue> updateFactory)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = updateFactory(key, dictionary[key]);
            }
            else
            {
                dictionary.Add(key, valueFactory(key));
            }
        }
    }
}