namespace JsonComparison.Common;

public class DataWrapper
{
    public List<Person> Persons { get; set; } = new();
    public List<Product> Products { get; set; } = new();
    public DateTime GeneratedAt { get; set; }
    public string Version { get; set; } = "1.0";
}