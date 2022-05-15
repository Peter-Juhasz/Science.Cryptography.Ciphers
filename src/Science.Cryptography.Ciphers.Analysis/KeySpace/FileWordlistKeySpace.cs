using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace Science.Cryptography.Ciphers.Analysis;

public sealed class FileWordlistKeySpace :
	IKeySpace<string>,
	IAsyncKeySpace<string>
{
	public FileWordlistKeySpace(string filePath)
		: this(filePath, Encoding.UTF8)
	{ }
	public FileWordlistKeySpace(string filePath, Encoding encoding)
	{
		FilePath = filePath;
		Encoding = encoding;
	}

	public string FilePath { get; }
	public Encoding Encoding { get; }

	public IEnumerable<string> GetKeys() => File.ReadLines(FilePath, Encoding);

	public async IAsyncEnumerable<string> GetKeysAsync([EnumeratorCancellation] CancellationToken cancellationToken)
	{
		await using var file = File.OpenRead(FilePath);
		using var reader = new StreamReader(file, Encoding);
		while (await reader.ReadLineAsync() is string line)
		{
			cancellationToken.ThrowIfCancellationRequested();
			yield return line;
		}
	}
}
