using Godot;
using System;
using System.Collections.Generic;

namespace GOSIjnr;

/// <summary>
/// Utility class providing helper methods for various tasks.
/// </summary>
public static class Utils
{
	/// <summary>
	/// Merges the template dictionary into the current dictionary.
	/// For each key in the template, if the current dictionary contains a value,
	/// that value is used; otherwise, the template value is used.
	/// </summary>
	/// <typeparam name="TKey">The type of dictionary keys.</typeparam>
	/// <typeparam name="TValue">The type of dictionary values.</typeparam>
	/// <param name="currentDictionary">The dictionary with current values.</param>
	/// <param name="templateDictionary">The dictionary providing default/template values.</param>
	/// <returns>
	/// A new dictionary containing keys from the template and values from currentDictionary or templateDictionary (if not present in the current dictionary).
	/// </returns>
	public static IDictionary<TKey, TValue> MergeDictionaries<TKey, TValue>(IDictionary<TKey, TValue> currentDictionary, IDictionary<TKey, TValue> templateDictionary)
	{
		Dictionary<TKey, TValue> mergedDictionary = [];

		foreach (var templateEntry in templateDictionary)
		{
			// If the current dictionary has the key, prefer its value; otherwise, use the template value.
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

	/// <summary>
	/// Loads all resources from the specified folder path using the Godot file system.
	/// </summary>
	/// <param name="folderPath">
	/// The folder path (for example, "res://path/to/folder") to search for resources.
	/// </param>
	/// <returns>An array of loaded resources.</returns>
	public static Resource[] GetResources(string folderPath)
	{
		List<Resource> resources = [];

		if (string.IsNullOrEmpty(folderPath)) return resources.ToArray();

		var folderDirectory = folderPath.EndsWith("/", StringComparison.OrdinalIgnoreCase) ? folderPath : folderPath + "/";
		var fileNames = ResourceLoader.ListDirectory(folderDirectory);

		if (fileNames.Length == 0)
		{
			Logger.LogMessage($"No resources found in folder: {folderDirectory}", Logger.LogLevel.Warning);
		}

		foreach (var file in fileNames)
		{
			Resource resource = ResourceLoader.Load(folderDirectory + file);

			if (resource != null) resources.Add(resource);
		}

		return resources.ToArray();
	}

	/// <summary>
	/// Loads all resources of the specified type T from the given folder path.
	/// Only resources that can be successfully cast to T (which must inherit from Godot.Resource) are returned.
	/// </summary>
	/// <typeparam name="T">The type of resource to load (inheriting from Godot.Resource).</typeparam>
	/// <param name="folderPath">
	/// The folder path (for example, "res://path/to/folder") to search for resources.
	/// </param>
	/// <returns>An array of loaded resources of type T.</returns>
	public static T[] GetResources<T>(string folderPath) where T : Resource
	{
		List<T> resources = [];

		if (string.IsNullOrEmpty(folderPath)) return resources.ToArray();

		var folderDirectory = folderPath.Trim().EndsWith("/", StringComparison.OrdinalIgnoreCase) ? folderPath.Trim() : folderPath.Trim() + "/";
		var fileNames = ResourceLoader.ListDirectory(folderDirectory);

		if (fileNames.Length == 0)
		{
			Logger.LogMessage($"No resources found in folder: {folderDirectory}", Logger.LogLevel.Warning);
		}

		foreach (var file in fileNames)
		{
			try
			{
				// Attempt to load the resource of type T.
				T res = ResourceLoader.Load<T>(folderDirectory + file);

				if (res != null) resources.Add(res);
			}
			catch (Exception ex)
			{
				Logger.LogMessage($"Failed to load resource {folderDirectory + file}: {ex.Message}", Logger.LogLevel.Error);
			}
		}

		return resources.ToArray();
	}
}
