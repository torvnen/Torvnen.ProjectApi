namespace Torvnen.ProjectApi.Model.Entity
{
    public class Customer
    {
        public Customer(string name)
        {
            Name = name;
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public IEnumerable<Project>? Projects { get; set; }
    }
}