using Torvnen.ProjectApi.Model.Entity;

namespace Torvnen.ProjectApi.Data
{
    public static class DbContextExtensions
    {
        private static IEnumerable<Customer> customers => new Customer[]
        {
            new ("AllInAll Consulting"),
            new ("Beef and Boys Co."),
            new ("C-Corp Inc."),
            new ("Deidre Construction"),
        };

        private static string[] managers => new string[]
        {
            "Matt",
            "Maybel",
            "Mark",
            "Mandy"
        };

        /// <summary>
        /// Create development / demo data.
        /// To be used with in-memory DB. Does not wipe out previous data.
        /// </summary>
        public static void CreateInitialData(this ProjectDbContext db, int numProjects = 9)
        {
            foreach (var customer in customers)
            {
                db.Customers.Add(customer);
            }
            db.SaveChanges();

            foreach (var number in Enumerable.Range(0, numProjects))
            {
                db.Add(new Project
                {
                    Name = $"Project {number + 1}",
                    Customer = customers.ElementAt(number % customers.Count()),
                    Description = $"Description {number}",
                    Manager = $"Manager {managers[new Random().Next(0, managers.Count() - 1)]}"
                });
            }
            db.SaveChanges();
        }
    }
}