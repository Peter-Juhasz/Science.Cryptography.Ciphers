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

Measured speed up: **27x** regular case, **81x** best case, memory allocation reduction from **400 bytes to zero**.

## Atbash cipher
In version 2, a fast path was added for ASCII encoding:

|   |          Method |        Mean |     Error |    StdDev | Allocated |
|---|---------------- |------------:|----------:|----------:|----------:|
|**v2**| General | 3,974.15 ns | 10.977 ns | 10.268 ns |         - |
|**v2**| Ascii |    47.36 ns |  0.256 ns |  0.239 ns |         - |

Payload length: 43 characters

Measured speed up: **84x**

## Shift cipher (Caesar, ROT-13, ...)
In version 2, a fast path was added for shifting ASCII characters:

|      | Method                    | Mean      | Error    | StdDev   | Allocated |
|------|-------------------------- |----------:|---------:|---------:|----------:|
|      | ShiftCipher_General       | 140.20 ns | 0.941 ns | 0.881 ns |         - |
|**v2**| ShiftCipher_Ascii_Avx2128 |  10.81 ns | 0.032 ns | 0.030 ns |         - |

Payload length: 44 characters

Measured speed up: **14x**

## Frequency analysis
The old v1 implementation was based on a very simple, but expensive functional LINQ implementation. In the new version, memory allocation was greatly reduced:

|      |   Method |       Mean |   Error |  StdDev |  Gen 0 |  Gen 1 | Allocated |
|------|--------- |-----------:|--------:|--------:|-------:|-------:|----------:|
|v1    | General | 2,331.2 ns | 9.72 ns | 9.10 ns | 0.7935 | 0.0076 |   4,992 B |
|**v2**| General |   512.9 ns | 2.11 ns | 1.76 ns | 0.2499 |      - |   1,568 B |
|**v2**| Ascii_Optimized |   235.0 ns | 0.72 ns | 0.63 ns |      - |      - |         - |

Measured speed up: **10x**, memory allocation reduction from **5 KB to zero** (ascii).

## Relative letter frequencies scorer
The old v1 implementation was based on a very simple functional LINQ implementation. In the new version, memory allocation was greatly reduced:

|      |   Method |     Mean |     Error |    StdDev |  Gen 0 |  Gen 1 | Allocated |
|------|--------- |---------:|----------:|----------:|-------:|-------:|----------:|
|v1    | General_Linq | 3.968 us | 0.0223 us | 0.0209 us | 1.0147 | 0.0076 |   6,368 B |
|**v2**| General | 1.464 us | 0.0042 us | 0.0035 us | 0.0210 |      - |     136 B |
|**v2**| Ascii_Optimized | 1.495 us | 0.0056 us | 0.0052 us | 0.0210 |      - |     136 B |

Measured speed up: **2.7x**, memory allocation reduction: **47x**

## N-Gram analysis
The old v1 implementation used allocating enumeration, reading into a StringBuilder, materializing each substring and then finally adding them to a new buffer every time.

In the new version, buffers can be shared, reading uses non-allocating enumeration, instead of materialized Strings, StringSegments are returned. And there is a fast path for ASCII and 2-grams.

|      |     Method |       Mean |    Error |   StdDev |  Gen 0 |  Gen 1 | Allocated |
|------|----------- |-----------:|---------:|---------:|-------:|-------:|----------:|
|v1    | General | 5,530.7 ns | 27.76 ns | 25.97 ns | 1.7319 | 0.0458 |  10,888 B |
|**v2**| General | 1,124.4 ns |  2.14 ns |  2.01 ns | 0.3719 | 0.0019 |   2,336 B |
|**v2**| Ascii_2Grams |   306.5 ns |  1.45 ns |  1.36 ns |      - |      - |         - |

Measured speed up: **18x**, memory allocation reduction from **18 KB to zero** (ascii).

## Caesar brute-force

|      |       Method |     Mean |    Error |   StdDev |  Gen 0 | Allocated |
|------|------------- |---------:|---------:|---------:|-------:|----------:|
|v1    | General | 41.41 us | 0.244 us | 0.217 us | 1.2817 |      8 KB |
|**v2**| General | 81.51 us | 0.180 us | 0.150 us | 0.9766 |      6 KB |
|**v2**| General_Optimized | 80.76 us | 0.262 us | 0.245 us | 0.8545 |      5 KB |

Measured speed up: *-2x*, memory allocation reduced to **62.5%**

## Appendix
Performance was measured on the following setup:

```
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
Intel Core i7-8700K CPU 3.70GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=7.0.100-preview.1.22110.4
  [Host]     : .NET 7.0.0 (7.0.22.7608), X64 RyuJIT
  DefaultJob : .NET 7.0.0 (7.0.22.7608), X64 RyuJIT
```