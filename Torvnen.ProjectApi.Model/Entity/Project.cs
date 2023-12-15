using Torvnen.ProjectApi.Model.DTO;
using Torvnen.ProjectApi.Model.Enum;

namespace Torvnen.ProjectApi.Model.Entity
{
    public class Project
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Manager { get; set; }
        public Customer? Customer { get; set; }
        public ProjectType ProjectType { get; set; }

        public Project Update(ProjectDTO project, Customer customer)
        {
            Name = project.Name;
            Description = project.Description;
            Manager = project.Manager;
            Customer = customer;
            ProjectType = (ProjectType)project.ProjectTypeId;

            return this;
        }
    }
}