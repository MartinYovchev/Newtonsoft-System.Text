namespace JsonComparison.Common;

public class Person
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public bool IsActive { get; set; }
    public decimal? Salary { get; set; }
    public List<string> Skills { get; set; } = new();
    public Address? Address { get; set; }
    public Person? Manager { get; set; }
    
    public override string ToString()
    {
        return $"[{Id}] {Name} ({Email}) - Active: {IsActive}";
    }
}