using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Reflection;
using System.Text;
using YC4_THUC_TAP_SINH_BE.Data;
using YC4_THUC_TAP_SINH_BE.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSettings);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var secretKey = jwtSettings["SecretKey"];
    var key = Encoding.UTF8.GetBytes(secretKey);

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero // Không cho phép sai lệch thời gian
    };

    // Event handlers
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"Authentication failed: {context.Exception.Message}");
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            var username = context.Principal?.Identity?.Name;
            Console.WriteLine($"Token validated for user: {username}");
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthorization(options =>
{
    // Policy cho Roles
    options.AddPolicy("RequireAdminRole", policy =>
        policy.RequireRole("Admin"));

    options.AddPolicy("RequireManagerRole", policy =>
        policy.RequireRole("Manager"));

    options.AddPolicy("RequireAdminOrManager", policy =>
        policy.RequireRole("Admin", "Manager"));

    // Policy cho Permissions
    options.AddPolicy("CanViewUsers", policy =>
        policy.RequireClaim("Permission", "USER_VIEW"));

    options.AddPolicy("CanCreateUsers", policy =>
        policy.RequireClaim("Permission", "USER_CREATE"));

    options.AddPolicy("CanEditUsers", policy =>
        policy.RequireClaim("Permission", "USER_EDIT"));

    options.AddPolicy("CanDeleteUsers", policy =>
        policy.RequireClaim("Permission", "USER_DELETE"));

    options.AddPolicy("CanViewRoles", policy =>
        policy.RequireClaim("Permission", "ROLE_VIEW"));

    options.AddPolicy("CanManageRoles", policy =>
        policy.RequireClaim("Permission", "ROLE_CREATE", "ROLE_EDIT", "ROLE_DELETE"));
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalDev", policy =>
    {
        policy.AllowAnyOrigin() // <-- Allow requests from any origin
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowLocalDev");
app.UseRouting();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
