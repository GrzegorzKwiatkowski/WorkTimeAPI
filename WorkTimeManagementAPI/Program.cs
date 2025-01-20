
var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// Rejestracja EmployeeRepository jako singleton
builder.Services.AddSingleton<EmployeeRepository>();

// Rejestracja TimeEntryRepository jako scoped
builder.Services.AddScoped<TimeEntryRepository>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrEmpty(connectionString))
        throw new InvalidOperationException("Connection string 'DefaultConnection' is missing");

    return new TimeEntryRepository(connectionString);
});
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
// Dodanie kontrolerów
builder.Services.AddControllers();

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

//app.UseMiddleware<ApiKeyMiddleware>();

app.MapControllers();

app.Run();

// Klasa partial dla testów integracyjnych
public partial class Program { }