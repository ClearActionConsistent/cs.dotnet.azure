using AspNetCoreIdentityRazor.Data;
using AspNetCoreIdentityRazor.Data.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRazorPages();
builder.Services.AddDbContext<IdentityDBContext>(options => {
    options.UseSqlServer(builder.Configuration.GetSection("SQLDBConnectionStrings")["Default"]);
});

//we can specify other classes to get extra fields of user/role
builder.Services.AddIdentity<CustomUser, IdentityRole>(options => {
    options.Password.RequiredLength = 9;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = true;
})
.AddEntityFrameworkStores<IdentityDBContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options => {
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

//add social logins
builder.Services.AddAuthentication().AddFacebook(options => { 
    options.AppId = builder.Configuration["FacebookAppId"] ?? string.Empty; ;
    options.AppSecret = builder.Configuration["FacebookAppSecret"] ?? string.Empty;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapStaticAssets();
app.MapRazorPages()
  .WithStaticAssets();

app.Run();
