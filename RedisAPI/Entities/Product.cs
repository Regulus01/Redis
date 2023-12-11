namespace RedisAPI.Entities;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    public Product(Guid id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
}