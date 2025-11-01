```

BenchmarkDotNet v0.15.5, macOS Sequoia 15.3.1 (24D70) [Darwin 24.3.0]
Apple M4 Pro, 1 CPU, 14 logical and 14 physical cores
.NET SDK 9.0.306
  [Host]     : .NET 9.0.10 (9.0.10, 9.0.1025.47515), Arm64 RyuJIT armv8.0-a
  DefaultJob : .NET 9.0.10 (9.0.10, 9.0.1025.47515), Arm64 RyuJIT armv8.0-a


```
| Method                              | Mean      | Error    | StdDev   | Rank | Gen0    | Gen1   | Allocated |
|------------------------------------ |----------:|---------:|---------:|-----:|--------:|-------:|----------:|
| &#39;System.Text.Json: Serialize&#39;       |  51.83 μs | 0.301 μs | 0.281 μs |    1 |  9.3994 |      - |  77.34 KB |
| &#39;Newtonsoft: Serialize&#39;             |  61.20 μs | 0.664 μs | 0.621 μs |    2 | 16.7236 | 2.7466 | 137.36 KB |
| &#39;System.Text.Json: Deserialize&#39;     |  71.14 μs | 0.319 μs | 0.299 μs |    3 |  7.9346 | 1.4648 |   65.8 KB |
| &#39;Newtonsoft: Deserialize&#39;           | 100.85 μs | 0.621 μs | 0.581 μs |    4 |  9.3994 | 1.4648 |  77.42 KB |
| &#39;System.Text.Json: Full Round-Trip&#39; | 147.63 μs | 0.575 μs | 0.537 μs |    5 | 17.0898 | 2.4414 | 143.14 KB |
| &#39;Newtonsoft: Full Round-Trip&#39;       | 163.88 μs | 1.108 μs | 1.036 μs |    6 | 26.1230 | 5.8594 | 214.78 KB |
