using System.Text.Json.Serialization;
using Torvnen.ProjectApi.Model.Entity;

namespace Torvnen.ProjectApi.Model.DTO
{
    // TODO: See if this can be typed as record
    public class ProjectDTO
    {
        [JsonConstructor]
        public ProjectDTO()
        { }

        public ProjectDTO(Project project)
        {
            Id = project.Id;
            Name = project.Name;
            Description = project.Description;
            Manager = project.Manager;
            CustomerId = project.Customer?.Id ?? 0;
            CustomerName = project.Customer?.Name;
            ProjectTypeId = (int)project.ProjectType;
            ProjectTypeName = project.ProjectType.ToString();
        }

        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? Description { get; set; }
        public int Id { get; set; }
        public string? Manager { get; set; }
        public string? Name { get; set; }
        public int ProjectTypeId { get; set; }
        public string? ProjectTypeName { get; set; }
    }
}