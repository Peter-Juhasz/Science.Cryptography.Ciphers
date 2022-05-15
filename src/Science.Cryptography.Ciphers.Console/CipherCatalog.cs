using Science.Cryptography.Ciphers.Analysis;
using Science.Cryptography.Ciphers.Specialized;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Reflection;

namespace Science.Cryptography.Ciphers.Console;

internal static class CipherCatalog
{
	public static IReadOnlyCollection<Type> GetTypes() =>
		typeof(ICipher).Assembly.ExportedTypes
			.Union(typeof(ReverseCipher).Assembly.ExportedTypes)
			.Union(typeof(CryptogramSolver).Assembly.ExportedTypes)
		.Where(t => t.GetInterface("ICipher") != null || t.GetInterface("IKeyedCipher`1") != null)
		.Where(t => !t.IsAbstract)
		.OrderBy(t => t.Name)
		.ToList();

	public static ICipher CreateCipher(string name)
	{
		var type = FindCipherByName(name);
		if (type == null)
		{
			throw new InvalidOperationException($"Requested cipher with name '{name}' was not found.");
		}

		return (ICipher)Activator.CreateInstance(type)!;
	}

	public static object CreateKeyedCipher(string name)
	{
		var type = FindCipherByName(name);
		if (type == null)
		{
			throw new InvalidOperationException($"Requested cipher with name '{name}' was not found.");
		}

		return Activator.CreateInstance(type)!;
	}

	public static Type? FindCipherByName(string name)
	{
		return GetTypes().SingleOrDefault(t =>
			t.Name.Equals(name, StringComparison.OrdinalIgnoreCase) ||
			(t.Name.EndsWith("Cipher") && t.Name[..^"Cipher".Length].Equals(name, StringComparison.OrdinalIgnoreCase)) ||
			(t.Name.EndsWith("Code") && t.Name[..^"Code".Length].Equals(name, StringComparison.OrdinalIgnoreCase)) ||
			t.GetCustomAttributes<ExportAttribute>().Any(e => e.ContractName.Equals(name, StringComparison.OrdinalIgnoreCase))
		);
	}
}