using exam_system.Common.Enums;
using exam_system.Domain.Entities.Identity;
using exam_system.Features.Shared.ResultPattern;
using Microsoft.AspNetCore.Identity;

namespace exam_system.Persistence.Context;

public class IdentityDbContextSeed
{

    public async Task SaveAsync(
        UserManager<ApplicationUser> userManager, 
        RoleManager<ApplicationRole> roleManager
        )
    {
        await CreateRoleAfterCheck(roleManager, RoleNames.Student);
        await CreateRoleAfterCheck(roleManager, RoleNames.Admin);

        await CreateUserAfterCheck(userManager, 
            "Yousef Khaled",
            pass:"Yousef@123", 
            "yk086620@gmail.com", 
            "yk086620@gmail.com",
            AccountStatus.Pending,
            confirmed: false);
      }

    private static async Task CreateRoleAfterCheck(RoleManager<ApplicationRole> roleManager, string roleName)
    {
        if(!(await roleManager.RoleExistsAsync(roleName)))
        {
            var result = await roleManager.CreateAsync(new ApplicationRole(roleName));

            if (!(result.Succeeded))
            {
                throw new InvalidOperationException("Role can't be created");
            }
        }
    }

    private static async Task CreateUserAfterCheck(
        UserManager<ApplicationUser> userManager, 
        string fullName, 
        string pass, 
        string email, 
        string userName, 
        AccountStatus accountStatus, 
        bool confirmed)
    {
        if((await userManager.FindByEmailAsync(email)) is null)
        {
            var user = new ApplicationUser
            {
                FullName = fullName,
                Email = email,
                UserName = userName,
                AccountStatus = accountStatus,
                EmailConfirmed = confirmed

            };
            var result = await userManager.CreateAsync(user, pass);

            if (!(result.Succeeded))
            {
                throw new InvalidOperationException("User Can't be created");
            }

            result = await userManager.AddToRoleAsync(user, RoleNames.Admin);

            if (!(result.Succeeded))
            {
                throw new InvalidOperationException("User Can't be created");
            }
        }
    }

}