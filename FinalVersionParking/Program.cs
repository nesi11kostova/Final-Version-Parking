using System;
using System.Collections.Generic;
using System.Linq;

namespace Parking_1
{
    public class ParkingSpot
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public string Occupator { get; set; }
        public DateTime? ReservedUntil { get; set; }
    }

    public interface IParkingService
    {
        IEnumerable<ParkingSpot> GetParkingSpots();
        bool ReserveSpot(Reservation reservation);
        bool ReleaseSpot(string spotId, string vehicleNumber);
        bool ReleaseSpotEarlier(string spotId, string vehicleNumber, DateTime releaseDate, DateTime dayToLeave);
    }

    public class ParkingService : IParkingService
    {
        private List<ParkingSpot> _spots = new List<ParkingSpot>
        {
            new ParkingSpot { Id = "A1", Status = "Free" },
            new ParkingSpot { Id = "A2", Status = "Free" },
            new ParkingSpot { Id = "A3", Status = "Free" },
            new ParkingSpot { Id = "A4", Status = "Free" },
            new ParkingSpot { Id = "A5", Status = "Free" },
            new ParkingSpot { Id = "B1", Status = "Free" },
            new ParkingSpot { Id = "B2", Status = "Free" },
            new ParkingSpot { Id = "B3", Status = "Free" },
            new ParkingSpot { Id = "B4", Status = "Free" },
            new ParkingSpot { Id = "B5", Status = "Free" },
            new ParkingSpot { Id = "C1", Status = "Free" },
            new ParkingSpot { Id = "C2", Status = "Free" },
            new ParkingSpot { Id = "C3", Status = "Free" },
            new ParkingSpot { Id = "C4", Status = "Free" },
            new ParkingSpot { Id = "C5", Status = "Free" },
        };

        public IEnumerable<ParkingSpot> GetParkingSpots()
        {
            return _spots;
        }

        public bool ReserveSpot(Reservation reservation)
        {
            var spot = _spots.FirstOrDefault(s => s.Id == reservation.SpotId);

            if (spot != null && spot.Status == "Free")
            {
                spot.Status = "Occupied";
                spot.Occupator = reservation.VehicleNumber;
                spot.ReservedUntil = reservation.DateEnd;
                return true;
            }
            return false;
        }

        public bool ReleaseSpot(string spotId, string vehicleNumber)
        {
            var spot = _spots.FirstOrDefault(s => s.Id == spotId && s.Occupator == vehicleNumber);

            if (spot != null)
            {
                spot.Status = "Free";
                spot.Occupator = null;
                spot.ReservedUntil = null;
                return true;
            }
            return false;
        }

        public bool ReleaseSpotEarlier(string spotId, string vehicleNumber, DateTime releaseDate, DateTime dateToLeave)
        {
            var spot = _spots.FirstOrDefault(s => s.Id == spotId && s.Occupator == vehicleNumber);

            if (spot != null)
            {
                spot.Status = "Free";
                spot.Occupator = null;
                spot.ReservedUntil = null;
                return true;
            }
            return false;
        }
    }

    public class Finance
    {
        public double capital { get; set; }
        public double priceHour { get; set; }
        public double priceHourAdvance { get; set; }
        public double priceHourDayBefore { get; set; }
        public double subscriptionPerMonth { get; set; }
        public double priceDay { get; set; }
        public double priceDay2orMore { get; set; }
        public double priceToBeReturned { get; set; }
    }

    public class Reservation
    {
        public string SpotId { get; set; }
        public string VehicleNumber { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string Type { get; set; }
    }

    public class Release
    {
        public DateTime ReleaseDate { get; set; }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            IParkingService parkingService = new ParkingService();
            bool exit = false;

            Finance finance = new Finance
            {
                capital = 0.0,
                priceHour = 1,
                priceHourAdvance = 1.2,
                priceHourDayBefore = 1,
                subscriptionPerMonth = 168,
                priceDay = 24 * 1,
                priceDay2orMore = 24 * 1.2,
                priceToBeReturned = 0.0
            };

            while (!exit)
            {
                Console.Clear();
                PrintCentered("Parking Management System", ConsoleColor.Cyan);
                PrintCentered("1. View Parking Spots", ConsoleColor.Yellow);
                PrintCentered("2. Reserve a Spot", ConsoleColor.Yellow);
                PrintCentered("3. Release a Spot", ConsoleColor.Yellow);
                PrintCentered("4. Exit", ConsoleColor.Yellow);
                PrintCentered("Select an option: ", ConsoleColor.White, false);

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewParkingSpots(parkingService);
                        break;
                    case "2":
                        ReserveSpot(parkingService, finance);
                        break;
                    case "3":
                        ReleaseSpot(parkingService, finance);
                        break;
                    case "4":
                        exit = true;
                        break;
                    default:
                        PrintCentered("Invalid option. Please try again.", ConsoleColor.Red);
                        break;
                }

                if (!exit)
                {
                    PrintCentered("Press any key to return to the main menu...", ConsoleColor.White);
                    Console.ReadKey();
                }
            }
        }

        public static void ViewParkingSpots(IParkingService parkingService)
        {
            Console.Clear();
            PrintCentered("Parking Spots:", ConsoleColor.Green);
            foreach (var spot in parkingService.GetParkingSpots())
            {
                PrintCentered($"Spot ID: {spot.Id}, Status: {spot.Status}, Reserved By: {spot.Occupator}, Reserved Until: {spot.ReservedUntil}", ConsoleColor.White);
            }
        }

        public static void ReserveSpot(IParkingService parkingService, Finance finance)
        {
            Console.Clear();
            PrintCentered("Reserve a Spot", ConsoleColor.Green);
            PrintCentered("Enter Spot ID: ", ConsoleColor.Yellow, false);
            var spotId = Console.ReadLine();

            if (string.IsNullOrEmpty(spotId))
            {
                PrintCentered("Invalid operation!!!", ConsoleColor.Red);
                return;
            }

            PrintCentered("Enter Vehicle Number: ", ConsoleColor.Yellow, false);
            var vehicleNumber = Console.ReadLine();

            if (string.IsNullOrEmpty(vehicleNumber))
            {
                PrintCentered("Invalid operation!!!", ConsoleColor.Red);
                return;
            }

            PrintCentered("Enter Start Time (yyyy-MM-dd HH:mm): ", ConsoleColor.Yellow, false);
            var startTime = DateTime.Parse(Console.ReadLine());

            PrintCentered("Enter End Time (yyyy-MM-dd HH:mm): ", ConsoleColor.Yellow, false);
            var endTime = DateTime.Parse(Console.ReadLine());

            if (startTime > endTime)
            {
                PrintCentered("Invalid operation!!!", ConsoleColor.Red);
                return;
            }

            TimeSpan daysReserved = endTime - startTime;
            int daysDifference = (int)daysReserved.TotalDays;
            TimeSpan hourReserved = endTime - startTime;
            int hourDifference = (int)hourReserved.TotalHours;

            PrintCentered("Enter Reservation Type (moment/advance/subscription): ", ConsoleColor.Yellow, false);
            var type = Console.ReadLine();

            if (string.IsNullOrEmpty(type))
            {
                PrintCentered("Invalid operation!!!", ConsoleColor.Red);
                return;
            }
            else if (type == "advance")
            {
                if (endTime < startTime.AddDays(1))
                {
                    PrintCentered("You cannot reserve a parking spot for hours. You can only for days using the advance type of reservation!", ConsoleColor.Red);
                    return;
                }

                if (DateTime.Now.AddDays(7) < startTime)
                {
                    PrintCentered("You cannot reserve for this period of time!", ConsoleColor.Red);
                    PrintCentered("Using the advance type you can only reserve parking spot one week before the start time!", ConsoleColor.Red);
                    PrintCentered("You may be interested in our other subscription plans!", ConsoleColor.Red);
                    return;
                }

                if (startTime > endTime)
                {
                    PrintCentered("Invalid operation!!!", ConsoleColor.Red);
                    PrintCentered("Parking spot cannot be reserved for the past!", ConsoleColor.Red);
                    return;
                }

                if (endTime > startTime.AddDays(7))
                {
                    PrintCentered("You cannot reserve for this period of time.", ConsoleColor.Red);
                    PrintCentered("You may be interested in our subscription plan, check it out!", ConsoleColor.Red);
                    return;
                }

                if (startTime >= DateTime.Now.AddDays(2))
                {
                    finance.capital = hourDifference * finance.priceHourAdvance;
                }
                else if (startTime == DateTime.Now)
                {
                    PrintCentered("You cannot make a reservation in advance for today!", ConsoleColor.Red);
                    PrintCentered("You may be interested in our moment plan which will suit your needs!", ConsoleColor.Red);
                    return;
                }
                else if (startTime <= DateTime.Now.AddDays(1) && startTime >= DateTime.Now.AddDays(0))
                {
                    finance.capital = hourDifference * finance.priceHour;
                }
            }
            else if (type == "moment")
            {
                if (endTime < startTime.AddHours(1))
                {
                    PrintCentered("You cannot reserve a parking spot for under an hour!", ConsoleColor.Red);
                    return;
                }

                if (endTime == startTime.AddHours(1))
                {
                    finance.capital = finance.priceHour;
                }
                else if (endTime < startTime.AddHours(24))
                {
                    finance.capital = hourDifference * finance.priceHour;
                }
                else if (endTime == startTime.AddDays(1))
                {
                    finance.capital = 24 * finance.priceHour;
                }

                if (startTime.DayOfWeek == DayOfWeek.Friday)
                {
                    if (endTime > startTime.AddDays(2))
                    {
                        PrintCentered("You cannot reserve for this period of time.", ConsoleColor.Red);
                        PrintCentered("You may be interested in our subscription plan, check it out!", ConsoleColor.Red);
                        return;
                    }

                    if (endTime >= startTime.AddDays(1) && endTime <= startTime.AddDays(2))
                    {
                        finance.capital = hourDifference * finance.priceHour;
                    }
                }

                if (endTime > startTime.AddDays(1))
                {
                    PrintCentered("You cannot reserve for this period of time.", ConsoleColor.Red);
                    PrintCentered("You may be interested in our other plans for reservation.", ConsoleColor.Red);
                    return;
                }
            }
            else if (type == "subscription")
            {
                PrintCentered("There is no refund for the subscription plan!", ConsoleColor.Red);
                if (daysDifference < 30)
                {
                    PrintCentered("You cannot make a subscription for less than a month!", ConsoleColor.Red);
                    PrintCentered("Check out our other reservation plans", ConsoleColor.Red);
                    return;
                }

                if (daysDifference == 30 || daysDifference == 31)
                {
                    finance.capital = finance.subscriptionPerMonth;
                }
                else
                {
                    PrintCentered("You can only make a subscription for a month!", ConsoleColor.Red);
                    return;
                }
            }

            var reservation = new Reservation
            {
                SpotId = spotId,
                VehicleNumber = vehicleNumber,
                DateStart = startTime,
                DateEnd = endTime,
                Type = type
            };

            if (parkingService.ReserveSpot(reservation))
            {
                PrintCentered("Reservation successful.", ConsoleColor.Green);
            }
            else
            {
                PrintCentered("Failed to reserve the spot. It might be already occupied.", ConsoleColor.Red);
            }

            PrintTheFinance(finance);
        }

        public static void ReleaseSpot(IParkingService parkingService, Finance finance)
        {
            Console.Clear();
            PrintCentered("Release a Spot", ConsoleColor.Green);
            PrintCentered("Enter Spot ID: ", ConsoleColor.Yellow, false);
            var spotId = Console.ReadLine();

            PrintCentered("Enter Vehicle Number: ", ConsoleColor.Yellow, false);
            var vehicleNumber = Console.ReadLine();

            PrintCentered("Are you releasing earlier? (yes/no): ", ConsoleColor.Yellow, false);
            string input = Console.ReadLine();

            if (input.Equals("yes", StringComparison.OrdinalIgnoreCase))
            {

                PrintCentered("Enter the date of the release (yyyy-MM-dd HH:mm): ", ConsoleColor.Yellow, false);
                DateTime releaseDate = DateTime.Parse(Console.ReadLine());

                PrintCentered("Enter the date on which you should be leaving (yyyy-MM-dd HH:mm): ", ConsoleColor.Yellow, false);
                DateTime dateToLeave = DateTime.Parse(Console.ReadLine());

                TimeSpan hoursLeft = dateToLeave - releaseDate;
                var hoursLeftToRelease = (int)hoursLeft.TotalHours;

                PrintCentered($"We are giving you back {finance.priceToBeReturned += hoursLeftToRelease * finance.priceHour * 0.7:f2} leva.", ConsoleColor.Green);

                if (parkingService.ReleaseSpotEarlier(spotId, vehicleNumber, releaseDate, dateToLeave))
                {
                    PrintCentered("Spot released successfully.", ConsoleColor.Green);
                }

                else
                {
                    PrintCentered("Failed to release the spot. It might be already free or reserved by another vehicle.", ConsoleColor.Red);
                }
            }
            else if (parkingService.ReleaseSpot(spotId, vehicleNumber))
            {
                PrintCentered("Spot released successfully.", ConsoleColor.Green);
            }
            else
            {
                PrintCentered("Failed to release the spot. It might be already free or reserved by another vehicle.", ConsoleColor.Red);
            }
        }

        public static void PrintTheFinance(Finance finance)
        {
            PrintCentered($"The price of your stay will be {Math.Abs(finance.capital):f2} leva.", ConsoleColor.Green);
        }

        public static void PrintCentered(string message, ConsoleColor color, bool newLine = true)
        {
            // Calculate the position to center the message
            int windowWidth = Console.WindowWidth;
            int messageLength = message.Length;
            int spaces = (windowWidth - messageLength) / 2;

            // Set the console color and print the message centered
            Console.ForegroundColor = color;
            if (newLine)
            {
                Console.WriteLine(new string(' ', spaces) + message);
            }
            else
            {
                Console.Write(new string(' ', spaces) + message);
            }
            Console.ResetColor();
        }
    }
}