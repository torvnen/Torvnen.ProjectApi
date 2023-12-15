using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using Torvnen.ProjectApi.Data;
using Torvnen.ProjectApi.Model.DTO;
using Torvnen.ProjectApi.Model.Entity;
using Torvnen.ProjectApi.Model.Enum;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddDbContext<ProjectDbContext>(opts => opts.UseInMemoryDatabase("ProjectDbContext"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, ProjectApiJsonContext.Default);
});

var app = builder.Build();

// Seed development data
using (var ctx = app.Services.CreateScope())
{
    var db = ctx.ServiceProvider.GetService<ProjectDbContext>();
    db.CreateInitialData();
}

app.MapGet("/projects", async (ProjectDbContext db) =>
{
    var projects = await db.Projects.ToArrayAsync();
    return projects;
}).WithOpenApi();

app.MapGet("/projecttypes", () =>
{
    // TODO: store in DB instead of enum
    return Enum.GetValues<ProjectType>().Select(pt => new
    {
        id = (int)pt,
        name = pt.ToString()
    });
}).WithOpenApi();

app.MapGet("/project/{id}", async (int id, ProjectDbContext db) =>
{
    return await db.Projects.FindAsync(id) is Project project ?
        Results.Ok(new ProjectDTO(project)) : Results.NotFound();
}).WithOpenApi();

app.MapPost("/project", async (ProjectDTO projectDto, ProjectDbContext db) =>
{
    var customer = await db.FindAsync<Customer>(projectDto.CustomerId);
    if (customer == default(Customer))
    {
        return Results.NotFound("Customer not found.");
    }

    var newProject = new Project().Update(projectDto, customer);
    await db.AddAsync(newProject);
    await db.SaveChangesAsync();

    return Results.Created($"/project/{projectDto.Id}", new ProjectDTO(newProject));
}).WithOpenApi();

app.MapPut("/project/{id}", async (int id, ProjectDTO projectDto, ProjectDbContext db) =>
{
    var existingProject = await db.FindAsync<Project>(id);
    if (existingProject == default(Project))
    {
        return Results.NotFound("Project not found.");
    }
    var customer = await db.FindAsync<Customer>(projectDto.CustomerId);
    if (customer == default(Customer))
    {
        return Results.NotFound("Customer not found.");
    }

    var updatedProject = existingProject.Update(projectDto, customer);
    db.Update(updatedProject);
    await db.SaveChangesAsync();

    return Results.Ok(new ProjectDTO(updatedProject));
}).WithOpenApi();

app.MapDelete("/project/{id}", async (int id, ProjectDbContext db) =>
{
    var existing = await db.FindAsync<Project>(id);
    if (existing == default(Project))
    {
        return Results.NotFound();
    }

    db.Remove(existing);
    await db.SaveChangesAsync();
    return Results.Ok();
}).WithOpenApi();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();

[JsonSourceGenerationOptions(JsonSerializerDefaults.Web, AllowTrailingCommas = true, DefaultBufferSize = 10)]
[JsonSerializable(typeof(Project[]))]
[JsonSerializable(typeof(ProjectType[]))]
[JsonSerializable(typeof(ProjectDTO))]
[JsonSerializable(typeof(Project))]
[JsonSerializable(typeof(Customer))]
internal partial class ProjectApiJsonContext : JsonSerializerContext
{
}