using System;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Science.Cryptography.Ciphers.Analysis;

public static class KeyFinder
{
    public static async Task<KeyFinderResult<TKey>>? FindBestAsync<TKey>(
        string ciphertext,
        IKeyedCipher<TKey> cipher,
        IKeySpaceSource<TKey> keySpace,
        ISpeculativePlaintextScorer speculativePlaintextScorer,
        Action<KeyFinderResult<TKey>>? onBetterResultFound = null,
        CancellationToken cancellationToken = default
    )
    {
        var degreeOfParallelism = Environment.ProcessorCount;

        var channel = Channel.CreateBounded<TKey>(degreeOfParallelism);
        var writer = channel.Writer;
        var reader = channel.Reader;

        var keyReaderTask = ReadKeysAsync(keySpace, writer, cancellationToken);

        KeyFinderResult<TKey>? best = null;
        object syncRoot = new();
        var workers = Enumerable.Range(0, degreeOfParallelism).Select(_ => TestBestAsync(
            ciphertext,
            cipher,
            speculativePlaintextScorer,
            candidate =>
            {
                if (candidate.Score > best.Score)
                {
                    lock (syncRoot)
                    {
                        if (candidate.Score > best.Score)
                        {
                            best = candidate;
                            onBetterResultFound?.Invoke(candidate);
                        }
                    }
                }
            },
            reader,
            cancellationToken
        ));
        var workerTasks = Task.WhenAll(workers);
        await Task.WhenAll(keyReaderTask, workerTasks);

        return best;
    }

    public static async Task FindAllAboveThresholdAsync<TKey>(
        string ciphertext,
        IKeyedCipher<TKey> cipher,
        IKeySpaceSource<TKey> keySpace,
        ISpeculativePlaintextScorer speculativePlaintextScorer,
        Action<KeyFinderResult<TKey>> onImmediateResultFound,
        double threshold = 0.9,
        CancellationToken cancellationToken = default
    )
    {
        var degreeOfParallelism = Environment.ProcessorCount;

        var channel = Channel.CreateBounded<TKey>(degreeOfParallelism);
        var writer = channel.Writer;
        var reader = channel.Reader;

        var keyReaderTask = ReadKeysAsync(keySpace, writer, cancellationToken);

        var workers = Enumerable.Range(0, degreeOfParallelism).Select(_ => TestAboveThresholdAsync(ciphertext, cipher, speculativePlaintextScorer, onImmediateResultFound, threshold, reader, cancellationToken));
        var workerTasks = Task.WhenAll(workers);
        await Task.WhenAll(keyReaderTask, workerTasks);
    }

    private static async Task ReadKeysAsync<TKey>(IKeySpaceSource<TKey> keySpace, ChannelWriter<TKey> writer, CancellationToken cancellationToken)
    {
        foreach (var key in keySpace.GetKeys())
        {
            await writer.WriteAsync(key, cancellationToken);
        }
        writer.Complete();
    }

    private static async Task TestAboveThresholdAsync<TKey>(
        string ciphertext,
        IKeyedCipher<TKey> cipher,
        ISpeculativePlaintextScorer speculativePlaintextScorer,
        Action<KeyFinderResult<TKey>> onImmediateResultFound,
        double threshold,
        ChannelReader<TKey> reader,
        CancellationToken cancellationToken
    )
    {
        var buffer = new char[ciphertext.Length * 2];

        await foreach (var key in reader.ReadAllAsync(cancellationToken))
        {
            cipher.Decrypt(ciphertext, buffer, key, out var written);

            double rank = speculativePlaintextScorer.Score(buffer.AsSpan(0, written));

            if (rank >= threshold)
            {
                onImmediateResultFound(new KeyFinderResult<TKey>(key, new string(buffer.AsSpan(0, written)), rank));
            }
        }
    }

    private static async Task TestBestAsync<TKey>(
        string ciphertext,
        IKeyedCipher<TKey> cipher,
        ISpeculativePlaintextScorer speculativePlaintextScorer,
        Action<KeyFinderResult<TKey>> onImmediateResultFound,
        ChannelReader<TKey> reader,
        CancellationToken cancellationToken
    )
    {
        var buffer = new char[ciphertext.Length * 2];
        var best = 0D;

        await foreach (var key in reader.ReadAllAsync(cancellationToken))
        {
            cipher.Decrypt(ciphertext, buffer, key, out var written);

            double score = speculativePlaintextScorer.Score(buffer.AsSpan(0, written));

            if (score >= best)
            {
                onImmediateResultFound(new KeyFinderResult<TKey>(key, new string(buffer.AsSpan(0, written)), score));
            }
        }
    }
}
