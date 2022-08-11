using Core.Campaigns;
using Core.Contacts;
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
        public static async Task SeedData(DataContext context)
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

            await context.SaveChangesAsync();


        }
    }
}
