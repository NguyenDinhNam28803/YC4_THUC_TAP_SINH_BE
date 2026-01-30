using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Reflection;
using System.Text;
using YC4_THUC_TAP_SINH_BE.Data;
using YC4_THUC_TAP_SINH_BE.Models;
using YC4_THUC_TAP_SINH_BE.Services;

var builder = WebApplication.CreateBuilder(args);

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

// Add services to the container.
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSettings);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAuthInterface, AuthService>();
builder.Services.AddScoped<IUserInterface,UserService>();
builder.Services.AddScoped<IRoleInterface,RoleService>();
builder.Services.AddScoped<IFunctionInterface, FunctionService>();

builder.Services.AddScoped<IJwtService,JwtService>();

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

Console.WriteLine("");
Console.WriteLine("========================================");
Console.WriteLine("   YC4 PERMISSION SYSTEM API");
Console.WriteLine("========================================");
Console.WriteLine("");

try
{
    Console.WriteLine("→ Initializing database and seed data...");
    await SeedData.InitializeAsync(app.Services);
    Console.WriteLine("");
    Console.WriteLine("✓ Database initialization completed!");
    Console.WriteLine("");
}
catch (Exception ex)
{
    Console.WriteLine("");
    Console.WriteLine($"❌ Error during database initialization: {ex.Message}");
    Console.WriteLine("");
}


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

// ==================== STARTUP INFO ====================
Console.WriteLine("========================================");
Console.WriteLine("   SERVER INFORMATION");
Console.WriteLine("========================================");
Console.WriteLine($"Environment: {app.Environment.EnvironmentName}");
Console.WriteLine($"API Base: https://localhost:7192/api or https://localhost:5080/api");
Console.WriteLine("");
Console.WriteLine("========================================");
Console.WriteLine("   SAMPLE ACCOUNTS");
Console.WriteLine("========================================");
Console.WriteLine("Admin Account:");
Console.WriteLine("  Username: admin");
Console.WriteLine("  Password: admin123");
Console.WriteLine("");
Console.WriteLine("Test Registration:");
Console.WriteLine("  POST /api/auth/register");
Console.WriteLine("  POST /api/auth/login");
Console.WriteLine("");
Console.WriteLine("========================================");
Console.WriteLine("   ROLES & PERMISSIONS");
Console.WriteLine("========================================");
Console.WriteLine("Roles:");
Console.WriteLine("  - Admin (tất cả quyền)");
Console.WriteLine("  - Manager (quản lý user + report)");
Console.WriteLine("  - User (xem user + report)");
Console.WriteLine("  - Guest (chỉ xem report)");
Console.WriteLine("");
Console.WriteLine("Functions: 16 permissions");
Console.WriteLine("  - USER_VIEW, USER_CREATE, USER_EDIT, USER_DELETE, USER_EXPORT");
Console.WriteLine("  - ROLE_VIEW, ROLE_CREATE, ROLE_EDIT, ROLE_DELETE");
Console.WriteLine("  - FUNCTION_VIEW, FUNCTION_CREATE, FUNCTION_EDIT, FUNCTION_DELETE");
Console.WriteLine("  - REPORT_VIEW, REPORT_EXPORT");
Console.WriteLine("  - SYSTEM_SETTINGS");
Console.WriteLine("");
Console.WriteLine("========================================");
Console.WriteLine($"🚀 Server started at: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
Console.WriteLine("========================================");
Console.WriteLine("");

app.Run();
