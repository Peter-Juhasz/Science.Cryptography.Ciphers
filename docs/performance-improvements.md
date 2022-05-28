# Performance improvements in Version 2

The following practices were implemented to reduce heap allocations and speed up performance significantly:
 - Extensive usage of `Span` and `StringSegment` instead of materialization of substructures. The whole `ICipher` concept was refactored to work on Spans instead of Strings.
 - Value-typed enumerators for enumerating collections
 - Vectorization, hardware intrinsics and SIMD operations where possible
 - Reusing buffers instead of creating new ones for each analysis cycle
 - Partitioning options to support parallel analysis better. State can be preserved safely for each thread, so no new instances need to be created to be stateless to avoid locking after all, and no locking is needed to race for resources from a pool either.
 - Specialized implementations for ASCII
 - Flattened access of multi-dimensional arrays for vectorization

## XOR cipher
The old v1 implementation was based on a very simple functional LINQ implementation.

The new version doesn't only include a heap allocation free implementation, but also a fast path for ASCII encoding with vectorization for SIMD processing.

|   |          Method |        Mean |    Error |   StdDev |  Gen 0 | Allocated |
|---|---------------- |------------:|---------:|---------:|-------:|----------:|
|v1 | General_Linq | 1,225.13 ns | 3.401 ns | 3.182 ns | 0.0629 |     400 B |
|**v2**| General_Loop |    90.36 ns | 0.172 ns | 0.153 ns |      - |         - |
|**v2**| Ascii_Vector |    44.27 ns | 0.104 ns | 0.097 ns |      - |         - |
|**v2**| Ascii_Vector_bestcase |    15.49 ns | 0.075 ns | 0.067 ns |      - |         - |

Payload and key length: 64/64, 64/32, 43/32, 64/32 characters

(*bestcase* represents the scenario where the payload is an exact multiple of key size)

Measured speed up: **27x** regular case, **81x** best case

## Atbash cipher
In version 2, a fast path was added for ASCII encoding:

|   |          Method |        Mean |     Error |    StdDev | Allocated |
|---|---------------- |------------:|----------:|----------:|----------:|
|**v2**|    Atbash | 3,974.15 ns | 10.977 ns | 10.268 ns |         - |
|**v2**| Atbash_Ascii |    47.36 ns |  0.256 ns |  0.239 ns |         - |

Payload length: 43 characters

Measured speed up: **84x**

## Relative letter frequencies scorer
The old v1 implementation was based on a very simple functional LINQ implementation. In the new version, memory allocation was greatly reduced:

|      |   Method |     Mean |     Error |    StdDev |  Gen 0 |  Gen 1 | Allocated |
|------|--------- |---------:|----------:|----------:|-------:|-------:|----------:|
|v1    | General_Linq | 3.968 us | 0.0223 us | 0.0209 us | 1.0147 | 0.0076 |   6,368 B |
|**v2**| General_Optimized | 1.464 us | 0.0042 us | 0.0035 us | 0.0210 |      - |     136 B |
|**v2**| General_Ascii | 1.495 us | 0.0056 us | 0.0052 us | 0.0210 |      - |     136 B |

Measured speed up: **2.7x**, memory allocation reduction: **47x**

## N-Gram analysis
The old v1 implementation used allocating enumeration, reading into a StringBuilder, materializing each substring and then finally adding them to a new buffer every time.

In the new version, buffers can be shared, reading uses non-allocating enumeration, instead of materialized Strings, StringSegments are returned. And there is a fast path for ASCII and 2-grams.

|      |     Method |       Mean |    Error |   StdDev |  Gen 0 |  Gen 1 | Allocated |
|------|----------- |-----------:|---------:|---------:|-------:|-------:|----------:|
|v1    | Old | 5,530.7 ns | 27.76 ns | 25.97 ns | 1.7319 | 0.0458 |  10,888 B |
|**v2**| New | 1,124.4 ns |  2.14 ns |  2.01 ns | 0.3719 | 0.0019 |   2,336 B |
|**v2**| New_Ascii_2Grams |   306.5 ns |  1.45 ns |  1.36 ns |      - |      - |         - |

Measured speed up: **18x**, memory allocation reduction from **18 KB to zero** (ascii).

## Appendix

```
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
Intel Core i7-8700K CPU 3.70GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=7.0.100-preview.1.22110.4
  [Host]     : .NET 7.0.0 (7.0.22.7608), X64 RyuJIT
  DefaultJob : .NET 7.0.0 (7.0.22.7608), X64 RyuJIT
```