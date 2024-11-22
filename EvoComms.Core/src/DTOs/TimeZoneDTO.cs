using System.Collections.Generic;

namespace EvoComms.Core.DTOs
{
    public class TimeZoneOption
    {
        public int Offset { get; set; }
        public List<string> Locations { get; set; }
        public string DisplayText => $"UTC{(Offset >= 0 ? "+" : "")}{Offset}: {string.Join(", ", Locations)}";
        public string Value => Offset.ToString();
    }

    public static class TimeZoneDTO
    {
        public static List<TimeZoneOption> GetTimeZones()
        {
            return new List<TimeZoneOption>
            {
                new() { Offset = -12, Locations = new List<string> { "Baker Island", "Howland Island" } },
                new() { Offset = -11, Locations = new List<string> { "American Samoa", "Niue" } },
                new()
                {
                    Offset = -10,
                    Locations = new List<string> { "Hawaii", "Cook Islands", "French Polynesia (part)" }
                },
                new()
                {
                    Offset = -9,
                    Locations = new List<string> { "Alaska", "French Polynesia (part)", "Gambier Islands" }
                },
                new()
                {
                    Offset = -8,
                    Locations = new List<string> { "Pacific Time (US & Canada)", "Baja California" }
                },
                new()
                {
                    Offset = -7,
                    Locations = new List<string> { "Mountain Time (US & Canada)", "Arizona", "Chihuahua" }
                },
                new()
                {
                    Offset = -6,
                    Locations =
                        new List<string> { "Central Time (US & Canada)", "Central America", "Mexico City" }
                },
                new()
                {
                    Offset = -5,
                    Locations = new List<string> { "Eastern Time (US & Canada)", "Bogota", "Lima", "Quito" }
                },
                new()
                {
                    Offset = -4,
                    Locations = new List<string> { "Atlantic Time (Canada)", "Caracas", "La Paz", "Santiago" }
                },
                new()
                {
                    Offset = -3,
                    Locations = new List<string> { "Buenos Aires", "Greenland", "Brasilia", "Montevideo" }
                },
                new() { Offset = -2, Locations = new List<string> { "Fernando de Noronha", "South Georgia" } },
                new() { Offset = -1, Locations = new List<string> { "Azores", "Cape Verde Islands" } },
                new() { Offset = 0, Locations = new List<string> { "London", "Dublin", "Lisbon", "Reykjavik" } },
                new()
                {
                    Offset = 1,
                    Locations = new List<string>
                    {
                        "Berlin",
                        "Paris",
                        "Rome",
                        "Madrid",
                        "Warsaw"
                    }
                },
                new()
                {
                    Offset = 2,
                    Locations = new List<string>
                    {
                        "Cairo",
                        "Athens",
                        "Istanbul",
                        "Jerusalem",
                        "Helsinki"
                    }
                },
                new() { Offset = 3, Locations = new List<string> { "Moscow", "Riyadh", "Baghdad", "Nairobi" } },
                new() { Offset = 4, Locations = new List<string> { "Dubai", "Abu Dhabi", "Muscat", "Baku" } },
                new() { Offset = 5, Locations = new List<string> { "Karachi", "Tashkent", "Yekaterinburg" } },
                new() { Offset = 6, Locations = new List<string> { "Dhaka", "Almaty", "Novosibirsk" } },
                new() { Offset = 7, Locations = new List<string> { "Bangkok", "Jakarta", "Ho Chi Minh City" } },
                new()
                {
                    Offset = 8, Locations = new List<string> { "Beijing", "Hong Kong", "Singapore", "Manila" }
                },
                new() { Offset = 9, Locations = new List<string> { "Tokyo", "Seoul", "Osaka", "Sapporo" } },
                new() { Offset = 10, Locations = new List<string> { "Sydney", "Melbourne", "Brisbane" } },
                new() { Offset = 11, Locations = new List<string> { "Noumea", "Solomon Islands", "Magadan" } },
                new() { Offset = 12, Locations = new List<string> { "Auckland", "Wellington", "Fiji" } },
                new() { Offset = 13, Locations = new List<string> { "Samoa", "Tonga", "Phoenix Islands" } },
                new() { Offset = 14, Locations = new List<string> { "Line Islands", "Kiritimati" } }
            };
        }
    }
}