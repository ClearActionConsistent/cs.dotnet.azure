using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using webapi.Authorization;
using webapi.Data;
using webapi.FluentValidators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        //options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

builder.Services.AddValidatorsFromAssemblyContaining<TodoValidator>();

builder.Services.AddDbContext<TodoContext>(opt =>
    opt.UseInMemoryDatabase("TodoList"));

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var secretKey = builder.Configuration.GetValue<string>("SecretKey");
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>//this is default auth scheme, BC it doesn't contains the name(refer to options above. 
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey ?? "")),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidateAudience = false,
        ValidateIssuer = false
    };
});//we can add more authentication scheme here if needed
//when we config multi authentication scheme, when the request come in, authentication middleware
//looks through the configured schemes to figure out which authentication scheme can be used to handle the authentication process
//if authentication middleware can not find appropriated authentication scheme to handle the request, it will return 401

//when the token come in, JWT Bearer will perform the authentication process by extracting & validating the token from the header

//add authorization
builder.Services.AddAuthorization(config => {
    config.AddPolicy("MustBelongToHRDeparment", policy =>
    {
        policy.RequireClaim("Department", "HR");
    });

    config.AddPolicy("MustHRAdmin", policy =>
    {
        policy.RequireClaim("Department", "HR")
        .RequireClaim("Role", "Admin")
        .Requirements.Add(new HRAdminPropationRequirement(3));
    });
});

builder.Services.AddSingleton<IAuthorizationHandler, HRAdminPropationRequirementHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    //app.UseExceptionHandler("/error-development");
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/error");
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
