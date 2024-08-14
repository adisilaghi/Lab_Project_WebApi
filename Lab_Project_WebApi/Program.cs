using System.Reflection;
using Lab_Project_WebApi.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Project_GradeBook_Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<IMarkService, MarkService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen((c =>
{
c.SwaggerDoc("v1", new OpenApiInfo { Title = "Lab_Project_WebApi.DbContext", Version = "v0.1" });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
}));
builder.Services.AddControllers();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
