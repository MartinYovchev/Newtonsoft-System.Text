using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Order;
using Newtonsoft.Json;
using System.Text.Json;
using JsonComparison.Common;

// Run benchmarks
// dotnet run -c Release
BenchmarkRunner.Run<JsonBenchmarks>();

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class JsonBenchmarks
{
    private List<Person> _testData = null!;
    private string _jsonText = null!;
    private JsonSerializerSettings _newtonsoftSettings = null!;
    private JsonSerializerOptions _systemTextOptions = null!;

    [GlobalSetup]
    public void Setup()
    {
        // Generate test data
        _testData = GenerateTestData(100);
        
        // Serialize once for deserialization tests
        _jsonText = JsonConvert.SerializeObject(_testData);
        
        // Setup settings
        _newtonsoftSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.None,
            NullValueHandling = NullValueHandling.Ignore
        };
        
        _systemTextOptions = new JsonSerializerOptions
        {
            WriteIndented = false,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };
    }

    [Benchmark(Description = "Newtonsoft: Serialize")]
    public string NewtonsoftSerialize()
    {
        return JsonConvert.SerializeObject(_testData, _newtonsoftSettings);
    }

    [Benchmark(Description = "System.Text.Json: Serialize")]
    public string SystemTextJsonSerialize()
    {
        return System.Text.Json.JsonSerializer.Serialize(_testData, _systemTextOptions);
    }

    [Benchmark(Description = "Newtonsoft: Deserialize")]
    public List<Person>? NewtonsoftDeserialize()
    {
        return JsonConvert.DeserializeObject<List<Person>>(_jsonText, _newtonsoftSettings);
    }

    [Benchmark(Description = "System.Text.Json: Deserialize")]
    public List<Person>? SystemTextJsonDeserialize()
    {
        return System.Text.Json.JsonSerializer.Deserialize<List<Person>>(_jsonText, _systemTextOptions);
    }

    [Benchmark(Description = "Newtonsoft: Full Round-Trip")]
    public List<Person>? NewtonsoftRoundTrip()
    {
        var json = JsonConvert.SerializeObject(_testData, _newtonsoftSettings);
        return JsonConvert.DeserializeObject<List<Person>>(json, _newtonsoftSettings);
    }

    [Benchmark(Description = "System.Text.Json: Full Round-Trip")]
    public List<Person>? SystemTextJsonRoundTrip()
    {
        var json = System.Text.Json.JsonSerializer.Serialize(_testData, _systemTextOptions);
        return System.Text.Json.JsonSerializer.Deserialize<List<Person>>(json, _systemTextOptions);
    }

    private static List<Person> GenerateTestData(int count)
    {
        var random = new Random(42);
        var persons = new List<Person>();
        
        var skills = new[] { "C#", "Java", "Python", "JavaScript", "SQL", "Docker", "Kubernetes", "Azure", "AWS" };
        var cities = new[] { "София", "Пловдив", "Варна", "Бургас", "Русе" };
        var streets = new[] { "ул. Витоша", "бул. България", "ул. Раковски", "бул. Цариградско шосе" };

        for (int i = 0; i < count; i++)
        {
            var person = new Person
            {
                Id = i + 1,
                Name = $"Person {i + 1}",
                Email = $"person{i + 1}@example.com",
                BirthDate = DateTime.Now.AddYears(-random.Next(20, 60)),
                IsActive = random.Next(100) > 20,
                Salary = random.Next(100) > 10 ? random.Next(2000, 8000) : null,
                Skills = Enumerable.Range(0, random.Next(2, 6))
                    .Select(_ => skills[random.Next(skills.Length)])
                    .Distinct()
                    .ToList(),
                Address = new Address
                {
                    Street = $"{streets[random.Next(streets.Length)]} {random.Next(1, 200)}",
                    City = cities[random.Next(cities.Length)],
                    Country = "България",
                    PostalCode = random.Next(1000, 9999).ToString()
                }
            };
            
            persons.Add(person);
        }

        return persons;
    }
}