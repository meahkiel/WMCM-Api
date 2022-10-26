using Core.Campaigns;
using Core.Channels;
using Core.Contacts;
using Core.Enum;
using Core.Tasks;
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

        public static async Task DataSeedContact(DataContext context)
        {

            //if (context.Contacts.Any())
            //{
            //    var contacts = await context.Contacts.ToListAsync();
            //    context.Contacts.RemoveRange(contacts);
            //}
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
            await context.SaveChangesAsync();
        }

        public static async Task DataSeedChannel(DataContext context)
        {
            if(!context.ChannelSettings.Any())
            {
                context.ChannelSettings.Add(new ChannelSetting
                {
                    Id = Guid.NewGuid(),
                    Title = "Twilio SMS Channel",
                    Description = "SMS Gateway for mobile messaging platform",
                    Type = "twilio",
                    ApiKey = "ACf2d0f5df971acd20300db7622599c714",
                    ApiSecretKey = "ce9ae086ab718a6f57d4591bcfd43850",
                    PhoneNo = "+14143480525"
                });

                context.ChannelSettings.Add(new ChannelSetting
                {
                    Id = Guid.NewGuid(),
                    Title = "Email Channel",
                    Description = "SMTP for sending email to your clients",
                    Type = "email",
                    Host = "smtp.mailtrap.io",
                    Port = 587,
                    UserName = "f7ec880bd97b60",
                    Password = "afb194bcb87e65",
                    Email = "arguimercado@gmail.com"
                });

                context.ChannelSettings.Add(new ChannelSetting
                {
                    Id = Guid.NewGuid(),
                    Title = "Web Campaign Posting",
                    Description = "Website campaign create a post to your Company Website",
                    Type = "website",
                    BaseUrl = "ama-def-rg.azure.net",
                    Header = "application/json"
                });

                await context.SaveChangesAsync();
            }
        }

        public static async Task DataSeedTask(DataContext context)
        {
            if(!context.MarketingTasks.Any())
            {

                var task = new MarketingTask
                {
                    Id = Guid.NewGuid(),
                    Title = "Promotional Campaign Preparation",
                    UserName = "bob"
                };

                task.AddSubTask("Make a concept for graphics", "des", "bob");
                task.AddSubTask("Contact the IT for the preparation of contact list", "staff","bob");
                
                context.MarketingTasks.Add(task);
                
                await context.SaveChangesAsync();
               
            }
        }

        public static async Task DataSeedCampaign(DataContext context)
        {
           
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

            await context.SaveChangesAsync();

        }
        public static async Task SeedRoleData(DataContext context,UserManager<AppUser> userManager,RoleManager<IdentityRole> roleManager)
        {
           

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
                    new AppUser() { DisplayName = "Staff1", JobTitle="Staff", UserName = "staff", Email="staff@testing.com"},
                    new AppUser() { DisplayName = "Staff2", JobTitle="Staff", UserName = "staff2", Email="staff@testing.com"}
                };
                try
                {
                    foreach (var user in users)
                    {
                        await userManager.CreateAsync(user, "Pa$$w0rd");
                    }

                }
                catch(Exception e)
                {

                }
            }
            else
            {
               
                var users = await userManager.Users.ToListAsync();
                foreach(var user in users)
                {
                    if(user.UserName == "bob")
                    {
                        await userManager.AddToRoleAsync(user, RoleEnum.Manager.ToString().ToLower());


                    }
                    else if(user.UserName == "admin")
                    {
                        await userManager.AddToRoleAsync(user, RoleEnum.Admin.ToString().ToLower());
                    }
                    else
                    {
                        await userManager.AddToRoleAsync(user,RoleEnum.Staff.ToString().ToLower());
                    }
                }
            }
        }
    }
}
