using Core.Campaigns;
using Core.Contacts;
using Core.Enum;
using Core.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Seeds
{
    public class Seed
    {

        public static async Task DataSeedTemplate(DataContext context)
        {
            if(!context.Templates.Any())
            {
                var templates = new List<Template>
                {
                    new Template { Id = Guid.NewGuid(), Title = "Customer Greeting", Description = "New Customer Template ", TemplateHtml = "Greetings Customer"},
                    new Template { Id = Guid.NewGuid(),Title = "Al Sharq", Description = "Al Sharq Big Sales and Raffle", TemplateHtml = "Big Sales and Raffle awaits you"}
                };
                context.Templates.AddRange(templates);
            }

            await context.SaveChangesAsync();
        }
        public static async Task SeedData(DataContext context,UserManager<AppUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            //if (context.Contacts.Any())
            //{
            //    var contacts = await context.Contacts.ToListAsync();
            //    context.Contacts.RemoveRange(contacts);
            //}

            if(!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole(RoleEnum.Manager.ToString().ToLower()));
                await roleManager.CreateAsync(new IdentityRole(RoleEnum.Admin.ToString().ToLower()));
                await roleManager.CreateAsync(new IdentityRole(RoleEnum.Staff.ToString().ToLower()));
            }

            if(!userManager.Users.Any())
            {
                var users = new List<AppUser>()
                {
                    new AppUser() { DisplayName = "Bob", JobTitle = "Manager",
                        UserName = "bob", Department="Marketing", Email="bob@testing.com"},
                    new AppUser() { DisplayName = "Designer1", JobTitle = "Graphic Designer",
                        UserName = "des", Department="Marketing", Email="des@testing.com"},
                    new AppUser() { DisplayName = "Staff1", JobTitle="Staff", UserName = "staff", Email="staff@testing.com"}

                };
                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "Pa$$w0rd");
                }
            }
            else
            {
               
                var users = await userManager.Users.ToListAsync();
                foreach(var user in users)
                {
                    if(user.UserName == "bob")
                    {
                        var result = await userManager.RemoveFromRoleAsync(user, RoleEnum.Admin.ToString().ToLower());

                        //await userManager.AddToRoleAsync(user,RoleEnum.Admin.ToString().ToLower());
                        //await userManager.AddToRoleAsync(user, RoleEnum.Manager.ToString().ToLower());
                    }
                    else if(user.UserName == "admin")
                    {
                        var result = await userManager.RemoveFromRoleAsync(user, RoleEnum.Staff.ToString().ToLower());
                    }
                    else
                    {
                        //await userManager.AddToRoleAsync(user,RoleEnum.Staff.ToString().ToLower());
                    }
                }
            }

         


            if (!context.Contacts.Any())
            {
                var contacts = new List<Contact>()
                {
                    new Contact {
                        Title = Core.Enum.Salutation.Mrs,
                        FirstName = "Rima",
                        LastName = "Mousa",
                        Gender = Core.Enum.GenderEnum.Female,
                        Location = "Doha",
                        GroupTag = "Lead",
                        EmailAddress = "rima@gmail.com",
                        MobileNo = "09747777887",
                        PrimaryContact ="09747777887"
                        },
                    
                    new Contact {
                        Title = Core.Enum.Salutation.Mr,
                        FirstName = "Mansour",
                        LastName = "Nabina",
                        Location = "Doha",
                        GroupTag = "Lead",
                        PrimaryContact = "09747777888",
                        EmailAddress= "mansour@gmail.com",
                        Gender = Core.Enum.GenderEnum.Male,
                        MobileNo = "09747777888" }
                };
                context.Contacts.AddRange(contacts);
            }

            //if (context.Campaigns.Any())
            //{
            //    context.Campaigns.RemoveRange(await context.Campaigns.ToListAsync());
            //}

            if (!context.Campaigns.Any())
            {

                var campaigns = new List<Campaign>()
                {
                    new Campaign {
                        Id = new System.Guid(),
                        Title = "Campaign Alsharq",
                        Description = "Alsharq Sale",
                        DateFrom = new DateTime(2022,06,04),
                        DateTo = new DateTime(2022,07,04)
                    },
                     new Campaign {
                        Id = new System.Guid(),
                        Title = "Ramadan Sale Promotion",
                        Description = "Up to 40% promotion",
                        DateFrom = new DateTime(2022,08,04),
                        DateTo = new DateTime(2022,09,04)
                    },
                };
                context.Campaigns.AddRange(campaigns);
            }

            //await context.SaveChangesAsync();


        }
    }
}
