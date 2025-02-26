using System.Collections.Generic;

namespace GOSIjnr;

public static class Utils
{
	// Merges the template dictionary into the current dictionary, preferring current values when keys overlap
	public static IDictionary<TKey, TValue> MergeDictionaries<TKey, TValue>(IDictionary<TKey, TValue> currentDictionary, IDictionary<TKey, TValue> templateDictionary)
	{
		Dictionary<TKey, TValue> mergedDictionary = new Dictionary<TKey, TValue>();

		foreach (var templateEntry in templateDictionary)
		{
			// If the current dictionary contains the key, use its value; otherwise, use the template value
			if (currentDictionary.TryGetValue(templateEntry.Key, out TValue currentValue))
			{
				mergedDictionary[templateEntry.Key] = currentValue;
			}
			else
			{
				mergedDictionary[templateEntry.Key] = templateEntry.Value;
			}
		}

		return mergedDictionary;
	}
}
