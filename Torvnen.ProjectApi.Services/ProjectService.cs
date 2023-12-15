using Microsoft.EntityFrameworkCore;
using Torvnen.ProjectApi.Data;
using Torvnen.ProjectApi.Model.Entity;

namespace Torvnen.ProjectApi.Services
{
    public class ProjectService
    {
        private readonly ProjectDbContext dbContext;

        public ProjectService(ProjectDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<Project> GetAllProjects()
        {
            return dbContext.Projects;
        }

        public async Task<Project> GetProjectByIdOrNull(int id)
        {
            return await dbContext.Projects.FirstOrDefaultAsync(project => project.Id == id);
        }
    }
}