using Microsoft.EntityFrameworkCore;
using YC4_THUC_TAP_SINH_BE.Models;

namespace YC4_THUC_TAP_SINH_BE.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Đảm bảo database đã được tạo
            await context.Database.EnsureCreatedAsync();

            // Seed Roles
            await SeedRolesAsync(context);

            // Seed Functions
            await SeedFunctionsAsync(context);

            // Seed Role-Function mapping
            await SeedRoleFunctionsAsync(context);

            // Seed Admin User (optional)
            await SeedAdminUserAsync(context);
        }

        private static async Task SeedRolesAsync(ApplicationDbContext context)
        {
            if (await context.Roles.AnyAsync())
            {
                Console.WriteLine("✓ Roles already exist. Skipping seed.");
                return;
            }

            Console.WriteLine("→ Seeding Roles...");

            var roles = new List<Role>
            {
                new Role
                {
                    RoleName = "Admin",
                    Description = "Quản trị viên hệ thống - Có toàn quyền",
                    CreatedAt = DateTime.UtcNow
                },
                new Role
                {
                    RoleName = "Manager",
                    Description = "Quản lý - Có quyền quản lý nhóm",
                    CreatedAt = DateTime.UtcNow
                },
                new Role
                {
                    RoleName = "User",
                    Description = "Người dùng thông thường",
                    CreatedAt = DateTime.UtcNow
                },
                new Role
                {
                    RoleName = "Guest",
                    Description = "Khách - Chỉ xem",
                    CreatedAt = DateTime.UtcNow
                }
            };

            await context.Roles.AddRangeAsync(roles);
            await context.SaveChangesAsync();

            Console.WriteLine($"✓ Seeded {roles.Count} Roles");
        }

        private static async Task SeedFunctionsAsync(ApplicationDbContext context)
        {
            if (await context.Functions.AnyAsync())
            {
                Console.WriteLine("✓ Functions already exist. Skipping seed.");
                return;
            }

            Console.WriteLine("→ Seeding Functions...");

            var functions = new List<Function>
            {
                // User Management
                new Function
                {
                    FunctionCode = "USER_VIEW",
                    FunctionName = "Xem danh sách User",
                    Description = "Được phép xem danh sách người dùng",
                    CreatedAt = DateTime.UtcNow
                },
                new Function
                {
                    FunctionCode = "USER_CREATE",
                    FunctionName = "Tạo User",
                    Description = "Được phép tạo người dùng mới",
                    CreatedAt = DateTime.UtcNow
                },
                new Function
                {
                    FunctionCode = "USER_EDIT",
                    FunctionName = "Sửa User",
                    Description = "Được phép chỉnh sửa thông tin người dùng",
                    CreatedAt = DateTime.UtcNow
                },
                new Function
                {
                    FunctionCode = "USER_DELETE",
                    FunctionName = "Xóa User",
                    Description = "Được phép xóa người dùng",
                    CreatedAt = DateTime.UtcNow
                },
                new Function
                {
                    FunctionCode = "USER_EXPORT",
                    FunctionName = "Export User",
                    Description = "Được phép xuất dữ liệu người dùng",
                    CreatedAt = DateTime.UtcNow
                },

                // Role Management
                new Function
                {
                    FunctionCode = "ROLE_VIEW",
                    FunctionName = "Xem danh sách Role",
                    Description = "Được phép xem danh sách vai trò",
                    CreatedAt = DateTime.UtcNow
                },
                new Function
                {
                    FunctionCode = "ROLE_CREATE",
                    FunctionName = "Tạo Role",
                    Description = "Được phép tạo vai trò mới",
                    CreatedAt = DateTime.UtcNow
                },
                new Function
                {
                    FunctionCode = "ROLE_EDIT",
                    FunctionName = "Sửa Role",
                    Description = "Được phép chỉnh sửa vai trò",
                    CreatedAt = DateTime.UtcNow
                },
                new Function
                {
                    FunctionCode = "ROLE_DELETE",
                    FunctionName = "Xóa Role",
                    Description = "Được phép xóa vai trò",
                    CreatedAt = DateTime.UtcNow
                },

                // Function Management
                new Function
                {
                    FunctionCode = "FUNCTION_VIEW",
                    FunctionName = "Xem danh sách Function",
                    Description = "Được phép xem danh sách chức năng",
                    CreatedAt = DateTime.UtcNow
                },
                new Function
                {
                    FunctionCode = "FUNCTION_CREATE",
                    FunctionName = "Tạo Function",
                    Description = "Được phép tạo chức năng mới",
                    CreatedAt = DateTime.UtcNow
                },
                new Function
                {
                    FunctionCode = "FUNCTION_EDIT",
                    FunctionName = "Sửa Function",
                    Description = "Được phép chỉnh sửa chức năng",
                    CreatedAt = DateTime.UtcNow
                },
                new Function
                {
                    FunctionCode = "FUNCTION_DELETE",
                    FunctionName = "Xóa Function",
                    Description = "Được phép xóa chức năng",
                    CreatedAt = DateTime.UtcNow
                },

                // Report
                new Function
                {
                    FunctionCode = "REPORT_VIEW",
                    FunctionName = "Xem báo cáo",
                    Description = "Được phép xem báo cáo",
                    CreatedAt = DateTime.UtcNow
                },
                new Function
                {
                    FunctionCode = "REPORT_EXPORT",
                    FunctionName = "Xuất báo cáo",
                    Description = "Được phép xuất báo cáo",
                    CreatedAt = DateTime.UtcNow
                },

                // System Settings
                new Function
                {
                    FunctionCode = "SYSTEM_SETTINGS",
                    FunctionName = "Cài đặt hệ thống",
                    Description = "Được phép cài đặt hệ thống",
                    CreatedAt = DateTime.UtcNow
                }
            };

            await context.Functions.AddRangeAsync(functions);
            await context.SaveChangesAsync();

            Console.WriteLine($"✓ Seeded {functions.Count} Functions");
        }

        private static async Task SeedRoleFunctionsAsync(ApplicationDbContext context)
        {
            if (await context.RoleFunctions.AnyAsync())
            {
                Console.WriteLine("✓ Role-Functions already exist. Skipping seed.");
                return;
            }

            Console.WriteLine("→ Seeding Role-Function mappings...");

            // Lấy tất cả roles và functions
            var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Admin");
            var managerRole = await context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Manager");
            var userRole = await context.Roles.FirstOrDefaultAsync(r => r.RoleName == "User");
            var guestRole = await context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Guest");

            var allFunctions = await context.Functions.ToListAsync();

            var roleFunctions = new List<Role_Function>();

            // ADMIN - Có tất cả quyền
            if (adminRole != null)
            {
                foreach (var function in allFunctions)
                {
                    roleFunctions.Add(new Role_Function
                    {
                        RoleId = adminRole.RoleId,
                        FunctionId = function.FunctionId,
                        AssignedAt = DateTime.UtcNow
                    });
                }
            }

            // MANAGER - Có quyền quản lý User và xem Report
            if (managerRole != null)
            {
                var managerFunctionCodes = new[]
                {
                    "USER_VIEW", "USER_CREATE", "USER_EDIT", "USER_EXPORT",
                    "REPORT_VIEW", "REPORT_EXPORT"
                };

                foreach (var code in managerFunctionCodes)
                {
                    var function = allFunctions.FirstOrDefault(f => f.FunctionCode == code);
                    if (function != null)
                    {
                        roleFunctions.Add(new Role_Function
                        {
                            RoleId = managerRole.RoleId,
                            FunctionId = function.FunctionId,
                            AssignedAt = DateTime.UtcNow
                        });
                    }
                }
            }

            // USER - Chỉ xem User và Report
            if (userRole != null)
            {
                var userFunctionCodes = new[] { "USER_VIEW", "REPORT_VIEW" };

                foreach (var code in userFunctionCodes)
                {
                    var function = allFunctions.FirstOrDefault(f => f.FunctionCode == code);
                    if (function != null)
                    {
                        roleFunctions.Add(new Role_Function
                        {
                            RoleId = userRole.RoleId,
                            FunctionId = function.FunctionId,
                            AssignedAt = DateTime.UtcNow
                        });
                    }
                }
            }

            // GUEST - Chỉ xem Report
            if (guestRole != null)
            {
                var reportViewFunction = allFunctions.FirstOrDefault(f => f.FunctionCode == "REPORT_VIEW");
                if (reportViewFunction != null)
                {
                    roleFunctions.Add(new Role_Function
                    {
                        RoleId = guestRole.RoleId,
                        FunctionId = reportViewFunction.FunctionId,
                        AssignedAt = DateTime.UtcNow
                    });
                }
            }

            await context.RoleFunctions.AddRangeAsync(roleFunctions);
            await context.SaveChangesAsync();

            Console.WriteLine($"✓ Seeded {roleFunctions.Count} Role-Function mappings");
        }

        private static async Task SeedAdminUserAsync(ApplicationDbContext context)
        {
            // Kiểm tra đã có admin chưa
            if (await context.Users.AnyAsync(u => u.Username == "admin"))
            {
                Console.WriteLine("✓ Admin user already exists. Skipping seed.");
                return;
            }

            Console.WriteLine("→ Seeding Admin user...");

            // Hash password "admin123" bằng BCrypt
            var passwordHash = BCrypt.Net.BCrypt.HashPassword("admin123", workFactor: 12);

            var adminUser = new User
            {
                Username = "admin",
                PasswordHash = passwordHash,
                FullName = "Administrator",
                Email = "admin@example.com",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await context.Users.AddAsync(adminUser);
            await context.SaveChangesAsync();

            // Gán role Admin cho user admin
            var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Admin");
            if (adminRole != null)
            {
                var userRole = new User_Role
                {
                    UserId = adminUser.UserId,
                    RoleId = adminRole.RoleId,
                    AssignedAt = DateTime.UtcNow
                };

                await context.UserRoles.AddAsync(userRole);
                await context.SaveChangesAsync();
            }

            Console.WriteLine("✓ Seeded Admin user (username: admin, password: admin123)");
        }
    }
}