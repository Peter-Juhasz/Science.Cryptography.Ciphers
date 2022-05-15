using System.Collections.Generic;

namespace Science.Cryptography.Ciphers.Analysis;

public class IntKeySpace : IKeySpace<int>
{
	public IntKeySpace(int minimum, int maximum)
	{
		Minimum = minimum;
		Maximum = maximum;
	}

	public int Minimum { get; }
	public int Maximum { get; }

	public IEnumerable<int> GetKeys()
	{
		for (int i = Minimum; i <= Maximum; i++)
		{
			yield return i;
		}
	}
}
