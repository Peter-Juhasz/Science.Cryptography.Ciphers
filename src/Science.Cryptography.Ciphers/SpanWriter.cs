using System;

namespace Science.Cryptography.Ciphers;

public ref struct SpanWriter<T>
{
    public SpanWriter(Span<T> buffer)
    {
        _buffer = buffer;
        _written = 0;
    }

    private readonly Span<T> _buffer;
    public Span<T> Buffer => _buffer;

    private int _written;
    public int Written => _written;

    public void Write(T item)
    {
        _buffer[_written] = item;
        _written++;
    }

    public void Write(ReadOnlySpan<T> span)
    {
        span.CopyTo(_buffer[_written..]);
        _written += span.Length;
    }

    public Span<T> GetSpan(int length)
    {
        var span = _buffer.Slice(_written, length);
        _written += length;
        return span;
    }
}
