using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Science.Cryptography.Ciphers.Analysis.KeySpace;

public static class KeySpaceExtensions
{
	public static IKeySpace<string> AsString(this IKeySpace<char[]> keySpace) =>
		new CharArrayToStringKeySpace(keySpace);

	public static IPartitionedKeySpace<string> AsString(this IPartitionedKeySpace<char[]> keySpace) =>
		new CharArrayToStringPartitionedKeySpace(keySpace);

	public static IAsyncKeySpace<string> AsString(this IAsyncKeySpace<char[]> keySpace) =>
		new CharArrayToStringAsyncKeySpace(keySpace);


	private sealed class CharArrayToStringKeySpace : IKeySpace<string>
	{
		public CharArrayToStringKeySpace(IKeySpace<char[]> keySpace)
		{
			_keySpace = keySpace;
		}

		private readonly IKeySpace<char[]> _keySpace;

		public IEnumerable<string> GetKeys() => _keySpace.GetKeys().Select(k => new string(k));
	}

	private sealed class CharArrayToStringPartitionedKeySpace : IPartitionedKeySpace<string>
	{
		public CharArrayToStringPartitionedKeySpace(IPartitionedKeySpace<char[]> keySpace)
		{
			_keySpace = keySpace;
		}

		private readonly IPartitionedKeySpace<char[]> _keySpace;

		public IEnumerable<IKeySpace<string>> GetPartitions(int? desiredCount = null) => _keySpace.GetPartitions(desiredCount).Select(p => p.AsString());
	}

	private sealed class CharArrayToStringAsyncKeySpace : IAsyncKeySpace<string>
	{
		public CharArrayToStringAsyncKeySpace(IAsyncKeySpace<char[]> keySpace)
		{
			_keySpace = keySpace;
		}

		private readonly IAsyncKeySpace<char[]> _keySpace;

		public IAsyncEnumerable<string> GetKeysAsync(CancellationToken cancellationToken) => _keySpace.GetKeysAsync(cancellationToken).Select(k => new string(k));
	}
}
