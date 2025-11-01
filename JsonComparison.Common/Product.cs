namespace JsonComparison.Common;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public ProductCategory Category { get; set; }
    public Dictionary<string, object> Metadata { get; set; } = new();
    public DateTime CreatedDate { get; set; }
    
    public override string ToString()
    {
        return $"[{Id}] {Name} - {Price:C} ({Category})";
    }
}