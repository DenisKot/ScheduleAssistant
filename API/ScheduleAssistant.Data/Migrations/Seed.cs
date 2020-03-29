using System;
using System.Linq;
using ScheduleAssistant.Data.EntityFramework;
using ScheduleAssistant.Domain.Windows;

namespace ScheduleAssistant.Data.Migrations
{
    public static class Seed
    {
        public static void SeedData(this AppDbContext context)
        {
            if (!context.Windows.Any())
            {
                context.Windows.Add(
                    new Window {
                        Name = "Сегодня как можно раньше",
                        Description = "Доставка за несколько часов",
                        Price = 99,
                        Type = WindowType.ExpressDelivery,
                        Available = true
                    }
                );

                // ToDo: refactor and use Ukraine offset
                var ukraineOffset = TimeSpan.FromHours(3);

                // Generate windows for the next 30 days
                for(var i = 0; i < 30; i++)
                {
                    var date = new DateTimeOffset(DateTime.Today.ToUniversalTime())
                        .ToOffset(ukraineOffset)
                        .AddDays(i);

                    context.Windows.Add(
                        new Window
                        {
                            Name = "14:00 - 18:00",
                            Description = "Доставка с 14 до 18 часов",
                            Price = 79,
                            Type = WindowType.UsualDelivery,
                            Available = true,
                            Start = date.AddHours(14),
                            Finish = date.AddHours(18)
                        }
                    );

                    context.Windows.Add(
                        new Window
                        {
                            Name = "19:00 - 23:00",
                            Description = "Доставка с 19 до 23 часов",
                            Price = 79,
                            Type = WindowType.UsualDelivery,
                            Available = true,
                            Start = date.AddHours(19),
                            Finish = date.AddHours(23)
                        }
                    );
                }

                context.SaveChanges();
            }
        }
    }
}