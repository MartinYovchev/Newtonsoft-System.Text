using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using JsonComparison.Common;
using System.Diagnostics;

Console.WriteLine("╔════════════════════════════════════════════════════╗");
Console.WriteLine("║     NEWTONSOFT.JSON DEMONSTRATION                 ║");
Console.WriteLine("╚════════════════════════════════════════════════════╝\n");

// ═══════════════════════════════════════════════════════════════
// EXAMPLE 1: Basic Serialization & Deserialization
// ═══════════════════════════════════════════════════════════════
Console.WriteLine("═══ EXAMPLE 1: Basic Operations ═══\n");

var jsonPath = Path.Combine("..", "DemoData", "simple-data.json");
var jsonText = File.ReadAllText(jsonPath);

var stopwatch = Stopwatch.StartNew();
var data = JsonConvert.DeserializeObject<DataWrapper>(jsonText);
stopwatch.Stop();

Console.WriteLine($"✓ Deserialized {data!.Persons.Count} persons in {stopwatch.ElapsedMilliseconds}ms");
Console.WriteLine($"✓ Deserialized {data.Products.Count} products\n");

Console.WriteLine("Sample persons:");
foreach (var person in data.Persons.Take(3))
{
    Console.WriteLine($"  • {person}");
}

var settings = new JsonSerializerSettings
{
    Formatting = Formatting.Indented,
    NullValueHandling = NullValueHandling.Ignore,
    DateFormatString = "dd/MM/yyyy HH:mm",
    ContractResolver = new CamelCasePropertyNamesContractResolver()
};

stopwatch.Restart();
var outputJson = JsonConvert.SerializeObject(data, settings);
stopwatch.Stop();

var outputPath = "output-newtonsoft-basic.json";
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

// Handle circular reference
var circularSettings = new JsonSerializerSettings
{
    Formatting = Formatting.Indented,
    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
    MaxDepth = 10
};

try
{
    var circularJson = JsonConvert.SerializeObject(person1, circularSettings);
    File.WriteAllText("output-newtonsoft-circular.json", circularJson);
    Console.WriteLine("✓ Circular reference handled successfully!");
    Console.WriteLine("  Strategy: ReferenceLoopHandling.Ignore");
    Console.WriteLine($"  Output saved to 'output-newtonsoft-circular.json'\n");
}
catch (Exception ex)
{
    Console.WriteLine($"✗ Error: {ex.Message}");
}

// ═══════════════════════════════════════════════════════════════
// EXAMPLE 3: Dynamic Objects
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

// Newtonsoft supports dynamic directly!
dynamic dynObj = JsonConvert.DeserializeObject(dynamicJson)!;

Console.WriteLine("\nAccessing dynamic properties:");
Console.WriteLine($"  Name: {dynObj.name}");
Console.WriteLine($"  Price: {dynObj.price}");
Console.WriteLine($"  Available: {dynObj.available}");
Console.WriteLine($"  Custom Field: {dynObj.customField}");
Console.WriteLine($"  First Tag: {dynObj.tags[0]}");

Console.WriteLine("\n✓ Dynamic objects work seamlessly with Newtonsoft.Json!");

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

// Enum as string
var enumSettings = new JsonSerializerSettings
{
    Formatting = Formatting.Indented,
    Converters = new List<JsonConverter> { new StringEnumConverter() }
};

var enumJson = JsonConvert.SerializeObject(product, enumSettings);
Console.WriteLine("Enum serialized as STRING:");
Console.WriteLine(enumJson);

// Enum as number (default)
var enumNumberJson = JsonConvert.SerializeObject(product, Formatting.Indented);
Console.WriteLine("\nEnum serialized as NUMBER:");
Console.WriteLine(enumNumberJson);

File.WriteAllText("output-newtonsoft-enums.json", enumJson);
Console.WriteLine("\n✓ Enum serialization is very flexible!");

// ═══════════════════════════════════════════════════════════════
// EXAMPLE 5: Custom Date Formats
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

// Different date formats
var dateFormats = new Dictionary<string, string>
{
    { "dd.MM.yyyy", "Bulgarian style" },
    { "yyyy-MM-dd", "ISO 8601" },
    { "dd/MM/yyyy HH:mm:ss", "UK style with time" },
    { "MMMM dd, yyyy", "US long format" }
};

Console.WriteLine("Newtonsoft.Json date formatting examples:");
foreach (var (format, description) in dateFormats)
{
    var dateSettings = new JsonSerializerSettings
    {
        DateFormatString = format,
        Formatting = Formatting.None
    };
    
    var dateJson = JsonConvert.SerializeObject(personWithDate, dateSettings);
    
    // Safe extraction using Newtonsoft's parsing
    var parsedObj = Newtonsoft.Json.Linq.JObject.Parse(dateJson);
    var birthDateValue = parsedObj["BirthDate"]?.ToString() ?? "N/A";
    
    Console.WriteLine($"  {format,-25} → {birthDateValue,-25} ({description})");
}

Console.WriteLine("\n✓ Date formatting is simple and flexible!");
Console.WriteLine("  Just use DateFormatString property - no custom converters needed!");

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
    Salary = null,  // Null value
    Address = null  // Null object
};

// Include nulls
var includeNullsSettings = new JsonSerializerSettings
{
    Formatting = Formatting.Indented,
    NullValueHandling = NullValueHandling.Include
};

var withNulls = JsonConvert.SerializeObject(personWithNulls, includeNullsSettings);
Console.WriteLine("WITH nulls:");
Console.WriteLine(withNulls);

// Ignore nulls
var ignoreNullsSettings = new JsonSerializerSettings
{
    Formatting = Formatting.Indented,
    NullValueHandling = NullValueHandling.Ignore
};

var withoutNulls = JsonConvert.SerializeObject(personWithNulls, ignoreNullsSettings);
Console.WriteLine("\nWITHOUT nulls:");
Console.WriteLine(withoutNulls);

Console.WriteLine("\n✓ Null handling is very straightforward!");

// ═══════════════════════════════════════════════════════════════
// SUMMARY
// ═══════════════════════════════════════════════════════════════
Console.WriteLine("\n╔════════════════════════════════════════════════════╗");
Console.WriteLine("║     NEWTONSOFT.JSON SUMMARY                       ║");
Console.WriteLine("╠════════════════════════════════════════════════════╣");
Console.WriteLine("║ ✓ Rich API with many features                     ║");
Console.WriteLine("║ ✓ Excellent dynamic object support                ║");
Console.WriteLine("║ ✓ Easy circular reference handling                ║");
Console.WriteLine("║ ✓ Flexible date/enum formatting                   ║");
Console.WriteLine("║ ✓ Simple null value handling                      ║");
Console.WriteLine("║ ✓ Mature and well-documented                      ║");
Console.WriteLine("╚════════════════════════════════════════════════════╝");

Console.WriteLine("\nPress any key to exit...");
Console.ReadKey();