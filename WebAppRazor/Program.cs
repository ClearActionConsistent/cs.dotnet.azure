using WebAppRazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();


builder.Services.AddHttpClient("TodoApi", client => {
    client.BaseAddress = new Uri("http://localhost:5002/api/");
});

builder.Services.AddHttpClient("AuthApi", client => {
    client.BaseAddress = new Uri("http://localhost:5119/api/");
});

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITodoService, TodoService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
