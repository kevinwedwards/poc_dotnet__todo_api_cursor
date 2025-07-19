using Microsoft.OpenApi.Models;
using TodoApi.Services;
using TodoApi.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger/OpenAPI
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Todo API",
        Version = "v1",
        Description = "A simple API for managing todo items",
        Contact = new OpenApiContact
        {
            Name = "Todo API Support",
            Email = "support@todoapi.com"
        }
    });
    
    // Include XML comments for better documentation
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// Configure options
builder.Services.Configure<DataStoreOptions>(
    builder.Configuration.GetSection(DataStoreOptions.SectionName));

// Initialize the data store with configuration
var dataStoreOptions = builder.Configuration.GetSection(DataStoreOptions.SectionName).Get<DataStoreOptions>() ?? new DataStoreOptions();
InMemoryDataStore.Initialize(dataStoreOptions);

// Register services
builder.Services.AddSingleton<InMemoryDataStore>(InMemoryDataStore.Instance);
builder.Services.AddScoped<ITodoService, TodoService>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API V1");
        c.RoutePrefix = string.Empty; // Serve Swagger UI at the app's root
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
