using System.Text.Json;
using System.Text.Json.Serialization;
using JsonComparison.Common;
using System.Diagnostics;

Console.WriteLine("╔════════════════════════════════════════════════════╗");
Console.WriteLine("║     SYSTEM.TEXT.JSON DEMONSTRATION                ║");
Console.WriteLine("╚════════════════════════════════════════════════════╝\n");

// ═══════════════════════════════════════════════════════════════
// EXAMPLE 1: Basic Serialization & Deserialization
// ═══════════════════════════════════════════════════════════════
Console.WriteLine("═══ EXAMPLE 1: Basic Operations ═══\n");

var jsonPath = Path.Combine("..", "DemoData", "simple-data.json");
var jsonText = File.ReadAllText(jsonPath);

var stopwatch = Stopwatch.StartNew();
var optionsD = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
    Converters = { new JsonStringEnumConverter() }
};
var data = JsonSerializer.Deserialize<DataWrapper>(jsonText, optionsD);
stopwatch.Stop();

Console.WriteLine($"✓ Deserialized {data!.Persons.Count} persons in {stopwatch.ElapsedMilliseconds}ms");
Console.WriteLine($"✓ Deserialized {data.Products.Count} products\n");

Console.WriteLine("Sample persons:");
foreach (var person in data.Persons.Take(3))
{
    Console.WriteLine($"  • {person}");
}

var options = new JsonSerializerOptions
{
    WriteIndented = true,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};

stopwatch.Restart();
var outputJson = JsonSerializer.Serialize(data, options);
stopwatch.Stop();

var outputPath = "output-systemtext-basic.json";
File.WriteAllText(outputPath, outputJson);
Console.WriteLine($"\n✓ Serialized to '{outputPath}' in {stopwatch.ElapsedMilliseconds}ms");

// ═══════════════════════════════════════════════════════════════
// EXAMPLE 2: Circular References
// ═══════════════════════════════════════════════════════════════
Console.WriteLine("\n═══ EXAMPLE 2: Circular References ═══\n");

var person1 = new Person 
{ 
    Id = 100, 
    Name = "Иван Иванов",
    Email = "ivan@test.com",
    BirthDate = new DateTime(1990, 1, 1),
    IsActive = true
};

var person2 = new Person 
{ 
    Id = 101, 
    Name = "Петър Петров",
    Email = "petar@test.com",
    BirthDate = new DateTime(1985, 5, 15),
    IsActive = true
};

// Create circular reference
person1.Manager = person2;
person2.Manager = person1;

Console.WriteLine("Created circular reference:");
Console.WriteLine($"  {person1.Name} -> Manager: {person1.Manager.Name}");
Console.WriteLine($"  {person2.Name} -> Manager: {person2.Manager.Name}\n");

// Handle circular reference (requires .NET 6+)
var circularOptions = new JsonSerializerOptions
{
    WriteIndented = true,
    ReferenceHandler = ReferenceHandler.IgnoreCycles,
    MaxDepth = 64
};

try
{
    var circularJson = JsonSerializer.Serialize(person1, circularOptions);
    File.WriteAllText("output-systemtext-circular.json", circularJson);
    Console.WriteLine("✓ Circular reference handled successfully!");
    Console.WriteLine("  Strategy: ReferenceHandler.IgnoreCycles (.NET 6+)");
    Console.WriteLine($"  Output saved to 'output-systemtext-circular.json'\n");
}
catch (Exception ex)
{
    Console.WriteLine($"✗ Error: {ex.Message}");
}

// ═══════════════════════════════════════════════════════════════
// EXAMPLE 3: Dynamic Objects (JsonDocument approach)
// ═══════════════════════════════════════════════════════════════
Console.WriteLine("═══ EXAMPLE 3: Dynamic Objects ═══\n");

var dynamicJson = @"{
    ""name"": ""Dynamic Product"",
    ""price"": 99.99,
    ""available"": true,
    ""tags"": [""new"", ""sale""],
    ""customField"": 12345
}";

Console.WriteLine("Parsing dynamic JSON:");
Console.WriteLine(dynamicJson);

// System.Text.Json requires JsonDocument for dynamic access
using (var document = JsonDocument.Parse(dynamicJson))
{
    var root = document.RootElement;
    
    Console.WriteLine("\nAccessing properties via JsonDocument:");
    Console.WriteLine($"  Name: {root.GetProperty("name").GetString()}");
    Console.WriteLine($"  Price: {root.GetProperty("price").GetDouble()}");
    Console.WriteLine($"  Available: {root.GetProperty("available").GetBoolean()}");
    Console.WriteLine($"  Custom Field: {root.GetProperty("customField").GetInt32()}");
    Console.WriteLine($"  First Tag: {root.GetProperty("tags")[0].GetString()}");
}

Console.WriteLine("\n⚠ System.Text.Json requires JsonDocument/JsonNode for dynamic objects");
Console.WriteLine("  (No direct 'dynamic' support like Newtonsoft)");

// ═══════════════════════════════════════════════════════════════
// EXAMPLE 4: Enum Serialization
// ═══════════════════════════════════════════════════════════════
Console.WriteLine("\n═══ EXAMPLE 4: Enum Serialization ═══\n");

var product = new Product
{
    Id = 999,
    Name = "Test Product",
    Price = 49.99m,
    Category = ProductCategory.Electronics,
    CreatedDate = DateTime.Now
};

// Enum as string (requires converter)
var enumOptions = new JsonSerializerOptions
{
    WriteIndented = true,
    Converters = { new JsonStringEnumConverter() }
};

var enumJson = JsonSerializer.Serialize(product, enumOptions);
Console.WriteLine("Enum serialized as STRING (with JsonStringEnumConverter):");
Console.WriteLine(enumJson);

// Enum as number (default)
var enumNumberOptions = new JsonSerializerOptions { WriteIndented = true };
var enumNumberJson = JsonSerializer.Serialize(product, enumNumberOptions);
Console.WriteLine("\nEnum serialized as NUMBER (default):");
Console.WriteLine(enumNumberJson);

File.WriteAllText("output-systemtext-enums.json", enumJson);
Console.WriteLine("\n✓ Enum serialization requires explicit converter for strings");

// ═══════════════════════════════════════════════════════════════
// EXAMPLE 5: Custom Date Formats (Complex)
// ═══════════════════════════════════════════════════════════════
Console.WriteLine("\n═══ EXAMPLE 5: Custom Date Formats ═══\n");

var personWithDate = new Person
{
    Id = 200,
    Name = "Дата Тест",
    Email = "date@test.com",
    BirthDate = new DateTime(1995, 7, 20, 14, 30, 0),
    IsActive = true
};

Console.WriteLine($"Original date: {personWithDate.BirthDate:yyyy-MM-dd HH:mm:ss}\n");

// System.Text.Json uses ISO 8601 by default
var defaultDateOptions = new JsonSerializerOptions 
{ 
    WriteIndented = false 
};
var defaultDateJson = JsonSerializer.Serialize(personWithDate, defaultDateOptions);

// Safe extraction using JsonDocument
using (var doc = JsonDocument.Parse(defaultDateJson))
{
    var birthDateValue = doc.RootElement.GetProperty("BirthDate").GetString();
    Console.WriteLine($"  Default (ISO 8601)      → {birthDateValue}");
}

Console.WriteLine("\n⚠ System.Text.Json limitations:");
Console.WriteLine("  • Always uses ISO 8601 format by default");
Console.WriteLine("  • Custom date formats require writing a custom JsonConverter");
Console.WriteLine("  • Much more complex than Newtonsoft's DateFormatString");
Console.WriteLine("\n  Example formats you CANNOT easily do:");
Console.WriteLine("    ✗ dd.MM.yyyy (Bulgarian style)");
Console.WriteLine("    ✗ MMMM dd, yyyy (US long format)");
Console.WriteLine("    ✗ Custom regional formats");
Console.WriteLine("\n  This is a KEY DIFFERENCE between the two libraries!");

// ═══════════════════════════════════════════════════════════════
// EXAMPLE 6: Null Value Handling
// ═══════════════════════════════════════════════════════════════
Console.WriteLine("\n═══ EXAMPLE 6: Null Value Handling ═══\n");

var personWithNulls = new Person
{
    Id = 300,
    Name = "Нул Тест",
    Email = "null@test.com",
    BirthDate = DateTime.Now,
    IsActive = true,
    Salary = null,
    Address = null
};

// Include nulls
var includeNullsOptions = new JsonSerializerOptions
{
    WriteIndented = true,
    DefaultIgnoreCondition = JsonIgnoreCondition.Never
};

var withNulls = JsonSerializer.Serialize(personWithNulls, includeNullsOptions);
Console.WriteLine("WITH nulls:");
Console.WriteLine(withNulls);

// Ignore nulls
var ignoreNullsOptions = new JsonSerializerOptions
{
    WriteIndented = true,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
};

var withoutNulls = JsonSerializer.Serialize(personWithNulls, ignoreNullsOptions);
Console.WriteLine("\nWITHOUT nulls:");
Console.WriteLine(withoutNulls);

Console.WriteLine("\n✓ Null handling works well with DefaultIgnoreCondition");

// ═══════════════════════════════════════════════════════════════
// EXAMPLE 7: Performance Comparison
// ═══════════════════════════════════════════════════════════════
Console.WriteLine("\n═══ EXAMPLE 7: Performance Test ═══\n");

const int iterations = 1000;
var testPerson = new Person
{
    Id = 1,
    Name = "Test Person",
    Email = "test@example.com",
    BirthDate = DateTime.Now,
    IsActive = true,
    Salary = 5000,
    Skills = new List<string> { "C#", "SQL", "Azure" },
    Address = new Address { Street = "Test St", City = "Sofia", Country = "BG" }
};

var perfOptions = new JsonSerializerOptions { WriteIndented = false };

// Warmup
for (int i = 0; i < 100; i++)
{
    JsonSerializer.Serialize(testPerson, perfOptions);
}

// Actual test
var perfStopwatch = Stopwatch.StartNew();
for (int i = 0; i < iterations; i++)
{
    var json = JsonSerializer.Serialize(testPerson, perfOptions);
    var obj = JsonSerializer.Deserialize<Person>(json);
}
perfStopwatch.Stop();

Console.WriteLine($"Serialization + Deserialization x{iterations}:");
Console.WriteLine($"  Total time: {perfStopwatch.ElapsedMilliseconds}ms");
Console.WriteLine($"  Average: {(double)perfStopwatch.ElapsedMilliseconds / iterations:F3}ms per iteration");
Console.WriteLine($"  Throughput: {iterations * 1000.0 / perfStopwatch.ElapsedMilliseconds:F0} ops/sec");

// ═══════════════════════════════════════════════════════════════
// SUMMARY
// ═══════════════════════════════════════════════════════════════
Console.WriteLine("\n╔════════════════════════════════════════════════════╗");
Console.WriteLine("║     SYSTEM.TEXT.JSON SUMMARY                      ║");
Console.WriteLine("╠════════════════════════════════════════════════════╣");
Console.WriteLine("║ ✓ High performance (2-3x faster)                  ║");
Console.WriteLine("║ ✓ Lower memory usage                              ║");
Console.WriteLine("║ ✓ Built into .NET (no external dependency)        ║");
Console.WriteLine("║ ✓ Modern API design                               ║");
Console.WriteLine("║ ⚠ No direct dynamic object support                ║");
Console.WriteLine("║ ⚠ Custom date formats require converters          ║");
Console.WriteLine("║ ⚠ Less flexible than Newtonsoft                   ║");
Console.WriteLine("╚════════════════════════════════════════════════════╝");

Console.WriteLine("\nPress any key to exit...");
Console.ReadKey();