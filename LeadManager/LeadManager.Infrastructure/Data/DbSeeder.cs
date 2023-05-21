using LeadManager.Core.Entities;
using LeadManager.Core.Entities.Source;
using LeadManager.Core.Entities.Supplier;
using LeadManager.Core.Entities.Lead;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManager.Infrastructure.Data
{
    public class DbSeeder
    {


        public static async Task SeedAsync(LeadManagerDbContext dbContext,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {

            dbContext.Database.EnsureCreated();

            if (dbContext.Sources == null)
            {
                await dbContext.Sources.AddRangeAsync(
                   new Source("Source1", "Source1 description") { SourceId = 1 },
                   new Source("Source2", "Source2 description") { SourceId = 2 },
                   new Source("Source3", "Source3 description") { SourceId = 3 }
                );
            }

            if (dbContext.Suppliers == null)
            {
                await dbContext.Suppliers.AddRangeAsync(
                    new Supplier("Supplier1", "Supplier1 description") { SupplierId = 1 },
                    new Supplier("Supplier2", "Supplier2 description") { SupplierId = 2 },
                    new Supplier("Supplier3", "Supplier3 description") { SupplierId = 3 }
                );
            }

            if (dbContext.Leads == null)
            {
                await dbContext.Leads.AddRangeAsync(
                    new Lead(1, 1, "Lead1", "Lead1 with Source1 and Supplier1") { LeadId = 1 },
                    new Lead(2, 2, "Lead2", "Lead2 with Source2 and Supplier2") { LeadId = 2 },
                    new Lead(3, 3, "Lead3", "Lead3 with Source3 and Supplier3") { LeadId = 3 },
                    new Lead(1, 1, "Lead4", "Lead4 with Source1 and Supplier1") { LeadId = 4 },
                    new Lead(1, 1, "Lead5", "Lead5 with Source1 and Supplier1") { LeadId = 5 }
                );
            }

            User user = await userManager.FindByEmailAsync(configuration["AuthSettings:AdminEmail"]);
            if (user == null)
            {
                user = new User()
                {
                    FirstName = "Malith",
                    LastName = "Jayawardana",
                    Email = configuration["AuthSettings:AdminEmail"],
                    UserName = configuration["AuthSettings:AdminEmail"],
                    IsActive= true
                };

                var result = await userManager.CreateAsync(user, configuration["AuthSettings:AdminPW"]);

                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create new user in seeder");
                }

                //Create default user roles
                string[] roleNames = { "Admin", "Manager", "Member" };

                foreach (var roleName in roleNames)
                {
                    var roleExist = await roleManager.RoleExistsAsync(roleName);
                    if (!roleExist)
                    {
                        //create the roles and add them to the database
                        var roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));

                        if (roleResult != IdentityResult.Success)
                        {
                            throw new InvalidOperationException($"Could not create new user role ({roleName}) in seeder");
                        }
                    }
                }

                //Get Admin user account and assign all default user roles
                var addRoleResult = await userManager.AddToRoleAsync(user, "Admin");

                if (addRoleResult != IdentityResult.Success)
                {
                    throw new InvalidOperationException($"Could not add Admin role for user : {user.UserName}");
                }

                //var createdUser = await _userManager.FindByIdAsync(user.Id);
                //var retrievedRoles = await _userManager.GetRolesAsync(createdUser);
            }

           await dbContext.SaveChangesAsync();
        }            
    }  
}

