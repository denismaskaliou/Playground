namespace GraphQL.AspNet.Entities;

public class Program
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;
    
    public string CreatedAt { get; set; } = null!;

    public List<Rule> Rules { get; set; } = [];
}