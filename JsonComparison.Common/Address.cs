namespace JsonComparison.Common;

public class Address
{
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string? PostalCode { get; set; }
    
    public override string ToString()
    {
        return $"{Street}, {City}, {Country}";
    }
}