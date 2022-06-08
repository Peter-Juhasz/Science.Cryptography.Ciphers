namespace Science.Cryptography.Ciphers.Analysis;

public interface IPartitionedSpeculativePlaintextScorer
{
	ISpeculativePlaintextScorer GetForPartition();
}