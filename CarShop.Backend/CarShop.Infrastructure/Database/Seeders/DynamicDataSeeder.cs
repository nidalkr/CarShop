using CarShop.Domain.Entities.Commerce;
using CarShop.Domain.Entities.Appointments;

namespace CarShop.Infrastructure.Database.Seeders;

/// <summary>
/// Dynamic seeder koji se pokrece u runtime-u,
/// obicno pri startu aplikacije (npr. u Program.cs).
/// Koristi se za unos demo/test podataka koji nisu dio migracije.
/// </summary>
public static class DynamicDataSeeder
{
    public static async Task SeedAsync(DatabaseContext context)
    {
        // Osiguraj da baza postoji (bez migracija)
        //await context.Database.EnsureCreatedAsync();

        await SeedUserRolesAsync(context);
        await SeedUsersAsync(context);
        await SeedStatusesAsync(context);
        await SeedCategoriesAsync(context);
        await SeedBrandsAsync(context);
        await SeedCarsAsync(context);
        await SeedInquiriesAsync(context);
        await SeedReviewsAsync(context);

        // Additional seeders for new entities
        await SeedFavouritesAsync(context);
        await SeedCartsAsync(context);
        await SeedOrdersAsync(context);
        await SeedTransactionsAsync(context);
        await SeedDeliveriesAsync(context);
        await SeedTestDrivesAsync(context);
        await SeedServiceAppointmentsAsync(context);
        await SeedServiceRecordsAsync(context);
    }

    private static async Task SeedUserRolesAsync(DatabaseContext context)
    {
        if (await context.UserRoles.AnyAsync())
            return;

        var now = DateTime.UtcNow;
        var roles = new[]
        {
            new UserRoleEntity
            {
                RoleName = "Admin",
                RoleDescription = "Pun pristup administraciji sistema.",
                CreatedAtUtc = now
            },
            new UserRoleEntity
            {
                RoleName = "Manager",
                RoleDescription = "Upravljanje ponudom vozila i korisnicima.",
                CreatedAtUtc = now
            },
            new UserRoleEntity
            {
                RoleName = "Customer",
                RoleDescription = "Krajnji korisnik koji pregledava vozila.",
                CreatedAtUtc = now
            }
        };

        await context.UserRoles.AddRangeAsync(roles);
        await context.SaveChangesAsync();
        Console.WriteLine("? Dynamic seed: user roles added.");
    }

    /// <summary>
    /// Kreira demo korisnike ako ih još nema u bazi.
    /// </summary>
    private static async Task SeedUsersAsync(DatabaseContext context)
    {
        if (await context.Users.AnyAsync())
            return;

        var roles = await context.UserRoles.AsNoTracking().ToDictionaryAsync(x => x.RoleName, x => x.Id);
        if (roles.Count == 0)
            return;

        var hasher = new PasswordHasher<CarShopUserEntity>();
        var now = DateTime.UtcNow;

        var users = new[]
        {
            new CarShopUserEntity
            {
                Username = "admin",
                Email = "admin@carshop.local",
                PasswordHash = hasher.HashPassword(null!, "Admin123!"),
                FirstName = "Admin",
                LastName = "User",
                Phone = "+385911234567",
                Address = "Admin Street 1",
                RoleId = roles["Admin"],
                IsActive = true,
                RegistrationDate = now,
                LastLoginDate = now,
                CreatedAtUtc = now
            },
            new CarShopUserEntity
            {
                Username = "manager",
                Email = "manager@carshop.local",
                PasswordHash = hasher.HashPassword(null!, "User123!"),
                FirstName = "Market",
                LastName = "Manager",
                Phone = "+385981112223",
                Address = "Manager Street 1",
                RoleId = roles["Manager"],
                IsActive = true,
                RegistrationDate = now,
                CreatedAtUtc = now
            },
            new CarShopUserEntity
            {
                Username = "demo",
                Email = "demo@carshop.local",
                PasswordHash = hasher.HashPassword(null!, "Demo123!"),
                FirstName = "Demo",
                LastName = "User",
                Phone = "+385991112233",
                Address = "Demo Street 1",
                RoleId = roles["Customer"],
                IsActive = true,
                RegistrationDate = now,
                CreatedAtUtc = now
            },
            new CarShopUserEntity
            {
                Username = "tester",
                Email = "tester@carshop.local",
                PasswordHash = hasher.HashPassword(null!, "Tester123!"),
                FirstName = "Test",
                LastName = "Account",
                Phone = "+385971234567",
                Address = "Test Street 1",
                RoleId = roles["Customer"],
                IsActive = true,
                RegistrationDate = now,
                CreatedAtUtc = now
            }
        };

        await context.Users.AddRangeAsync(users);
        await context.SaveChangesAsync();
        Console.WriteLine("? Dynamic seed: demo users added.");
    }

    private static async Task SeedStatusesAsync(DatabaseContext context)
    {
        if (await context.Statuses.AnyAsync())
            return;

        var now = DateTime.UtcNow;
        var statuses = new[]
        {
            new StatusEntity
            {
                StatusName = "Available",
                Description = "Vozilo je dostupno za kupnju.",
                CreatedAtUtc = now
            },
            new StatusEntity
            {
                StatusName = "Reserved",
                Description = "Vozilo je rezervirano za kupca.",
                CreatedAtUtc = now
            },
            new StatusEntity
            {
                StatusName = "Sold",
                Description = "Vozilo je prodano.",
                CreatedAtUtc = now
            },
            new StatusEntity
            {
                StatusName = "Pending",
                Description = "Upit je zaprimljen i ceka obradu.",
                CreatedAtUtc = now
            },
            new StatusEntity
            {
                StatusName = "Completed",
                Description = "Upit je kompletiran.",
                CreatedAtUtc = now
            },
            new StatusEntity
            {
                StatusName = "Failed",
                Description = "Upit nije uspio.",
                CreatedAtUtc = now
            },
            new StatusEntity
            {
                StatusName = "Cancelled",
                Description = "Upit je otkazan.",
                CreatedAtUtc = now
            },
            new StatusEntity
            {
                StatusName = "Shipped",
                Description = "Vozilo je poslano.",
                CreatedAtUtc = now
            },
            new StatusEntity
            {
                StatusName = "Delivered",
                Description = "Vozilo je dostavljeno.",
                CreatedAtUtc = now
            },
            new StatusEntity
            {
                StatusName = "OutForDelivery",
                Description = "Vozilo je na dostavi.",
                CreatedAtUtc = now
            },
            new StatusEntity
            {
                StatusName = "Requested",
                Description = "Upit je zaprimljen i ceka obradu.",
                CreatedAtUtc = now
            },
            new StatusEntity
            {
                StatusName = "Confirmed",
                Description = "Upit je odobrne.",
                CreatedAtUtc = now
            },
            new StatusEntity
            {
                StatusName = "Responded",
                Description = "Na upit je odgovoreno kupcu.",
                CreatedAtUtc = now
            }
        };

        await context.Statuses.AddRangeAsync(statuses);
        await context.SaveChangesAsync();
        Console.WriteLine("? Dynamic seed: statuses added.");
    }

    private static async Task SeedCategoriesAsync(DatabaseContext context)
    {
        if (await context.Categories.AnyAsync())
            return;

        var now = DateTime.UtcNow;
        var categories = new[]
        {
            new CategoryEntity { CategoryName = "SUV", CreatedAtUtc = now },
            new CategoryEntity { CategoryName = "Avant", CreatedAtUtc = now },
            new CategoryEntity { CategoryName = "Sedan", CreatedAtUtc = now },
            new CategoryEntity { CategoryName = "Hatchback", CreatedAtUtc = now },
            new CategoryEntity { CategoryName = "Coupe", CreatedAtUtc = now },
            new CategoryEntity { CategoryName = "Truck", CreatedAtUtc = now }
        };
        await context.Categories.AddRangeAsync(categories);
        await context.SaveChangesAsync();
        Console.WriteLine("? Dynamic seed: categories added.");
    }

    private static async Task SeedBrandsAsync(DatabaseContext context)
    {
        if (await context.Brands.AnyAsync())
            return;

        var now = DateTime.UtcNow;
        var brands = new[]
        {
             new BrandEntity { BrandName = "Audi", CreatedAtUtc = now },
            new BrandEntity { BrandName = "Ford", CreatedAtUtc = now },
            new BrandEntity { BrandName = "Mercedes", CreatedAtUtc = now },
            new BrandEntity { BrandName = "BMW", CreatedAtUtc = now }
        };

        await context.Brands.AddRangeAsync(brands);
        await context.SaveChangesAsync();
        Console.WriteLine("? Dynamic seed: brands added.");
    }

    private static async Task SeedCarsAsync(DatabaseContext context)
    {
        if (await context.Cars.AnyAsync())
            return;

        var brands = await context.Brands.AsNoTracking().ToDictionaryAsync(x => x.BrandName, x => x.Id);
        var categories = await context.Categories.AsNoTracking().ToDictionaryAsync(x => x.CategoryName, x => x.Id);
        var statuses = await context.Statuses.AsNoTracking().ToDictionaryAsync(x => x.StatusName, x => x.Id);

        if (brands.Count == 0 || categories.Count == 0 || statuses.Count == 0)
            return;

        var now = DateTime.UtcNow;
        var cars = new List<CarEntity>
        {
            new CarEntity
            {
                BrandId = brands["Mercedes"],
                CategoryId = categories["Avant"],
                CarStatusId = statuses["Available"],

                Condition = "Used",                 
                InventoryLocation = "Mostar",       
                Doors = 5,
                Seats = 5,
                StockNumber = "MER-C-0001",        

                Model = "C 350",
                Vin = "JTM12345678901234",
                ProductionYear = 2017,
                Mileage = 176000,
                Color = "Midnight Blue",
                Transmission = "Automatic",
                FuelType = "Diesel",
                Drivetrain = "AWD",
                Engine = "3.0L V6",
                HorsePower = 256,
                Description = "Pouzdan dizelski karavan sa bogatom opremom.",
                Price = 32990m,
                DiscountedPrice = 31500m,
                DateAdded = now,
                CreatedAtUtc = now
            },
            new CarEntity
            {
                BrandId = brands["BMW"],
                CategoryId = categories["Sedan"],
                CarStatusId = statuses["Reserved"],

                Condition = "Used",
                InventoryLocation = "Mostar",
                Doors = 5,
                Seats = 5,
                StockNumber = "BMW-3-0001",

                Model = "330d",
                Vin = "WBA12345678905678",
                ProductionYear = 2022,
                Mileage = 18000,
                Color = "Mineral Grey",
                Transmission = "Automatic",
                FuelType = "Diesel",
                Drivetrain = "AWD",
                Engine = "3.0L I6",
                HorsePower = 288,
                Description = "Sportska limuzina sa kombinacijom performansi i elektricnog dometa do 60 km.",
                Price = 45990m,
                DateAdded = now,
                CreatedAtUtc = now
            },
            new CarEntity
            {
                BrandId = brands["Ford"],
                CategoryId = categories["SUV"],
                CarStatusId = statuses["Available"],

                Condition = "Used",
                InventoryLocation = "Mostar",
                Doors = 5,
                Seats = 5,
                StockNumber = "FRD-MACH-0001",

                Model = "Mach",
                Vin = "5YJ12345678907890",
                ProductionYear = 2024,
                Mileage = 5000,
                Color = "Red",
                Transmission = "Automatic",
                FuelType = "Electric",
                Drivetrain = "AWD",
                Engine = "Dual Motor",
                HorsePower = 670,
                Description = "Elektricni SUV visokih performansi s dosegom preko 600 km.",
                Price = 89990m,
                DateAdded = now,
                CreatedAtUtc = now
            },
            new CarEntity
            {
                BrandId = brands["Audi"],
                CategoryId = categories["Avant"],
                CarStatusId = statuses["Available"],

                Condition = "Used",
                InventoryLocation = "Mostar",
                Doors = 5,
                Seats = 5,
                StockNumber = "AUD-A6-0001",

                Model = "A6",
                Vin = "VWU12345678903456",
                ProductionYear = 2021,
                Mileage = 22000,
                Color = "Black",
                Transmission = "Automatic",
                FuelType = "Diesel",
                Drivetrain = "Quattro",
                Engine = "3.0L V6",
                HorsePower = 330,
                Description = "Porodicni auto sa 3.0L V6 motorom",
                Price = 68990m,
                DiscountedPrice = 65990m,
                DateAdded = now,
                CreatedAtUtc = now
            },
            new CarEntity
            {
                BrandId = brands["Ford"],
                CategoryId = categories["Hatchback"],
                CarStatusId = statuses["Available"],

                Condition = "Used",
                InventoryLocation = "Mostar",
                Doors = 5,
                Seats = 5,
                StockNumber = "FRD-FOCUS-0001",

                Model = "Focus RS",
                Vin = "WF05XXGCC5GL12345",
                ProductionYear = 2010,
                Mileage = 128000,
                Color = "White",
                Transmission = "Manual",
                FuelType = "Petrol",
                Drivetrain = "FWD",
                Engine = "2,5L",
                HorsePower = 420,
                Description = "Praktican i štedljiv gradski automobil, idealan za svakodnevnu vožnju.",
                Price = 12500m,
                DateAdded = now,
                CreatedAtUtc = now
            }
        };

        
        foreach (var car in cars)
        {
            if (car.BrandId == brands["Mercedes"] && car.Model == "C 350")
            {
                car.AddImage("images/cars/seed/mercedes-c-1.jpg", isPrimary: true);
                car.AddImage("images/cars/seed/mercedes-c-2.jpg");
                car.AddImage("images/cars/seed/mercedes-c-3.jpg");
                car.AddImage("images/cars/seed/mercedes-c-4.jpg");
                car.AddImage("images/cars/seed/mercedes-c-5.jpg");
                car.AddImage("images/cars/seed/mercedes-c-6.jpg");

                car.AddFeature("Burmester Sound System");
                car.AddFeature("AMG Optics");
                car.AddFeature("4MATIC");
            }

            if (car.BrandId == brands["BMW"] && car.Model == "330d")
            {
                car.AddImage("images/cars/seed/bmw-3-1.jpg", isPrimary: true);
                car.AddImage("images/cars/seed/bmw-3-2.jpg");
                car.AddImage("images/cars/seed/bmw-3-3.jpg");
                car.AddImage("images/cars/seed/bmw-3-4.jpg");
                car.AddImage("images/cars/seed/bmw-3-5.jpg");
                car.AddImage("images/cars/seed/bmw-3-6.jpg");

                car.AddFeature("xDrive");
                car.AddFeature("Harman Kardon Sound System");
                car.AddFeature("M Sport Package");
            }

            if (car.BrandId == brands["Ford"] && car.Model == "Mach")
            {
                car.AddImage("images/cars/seed/ford-mach-1.jpg", isPrimary: true);
                car.AddImage("images/cars/seed/ford-mach-2.jpg");
                car.AddImage("images/cars/seed/ford-mach-3.jpg");
                car.AddImage("images/cars/seed/ford-mach-4.jpg");
                car.AddImage("images/cars/seed/ford-mach-5.jpg");
                car.AddImage("images/cars/seed/ford-mach-6.jpg");
            }

            if (car.BrandId == brands["Audi"] && car.Model == "A6")
            {                
                car.AddImage("images/cars/seed/audi-a6-1.jpg", isPrimary: true);                
                car.AddImage("images/cars/seed/audi-a6-2.jpg");
                car.AddImage("images/cars/seed/audi-a6-3.jpg");
                car.AddImage("images/cars/seed/audi-a6-4.jpg");
                car.AddImage("images/cars/seed/audi-a6-5.jpg");
                car.AddImage("images/cars/seed/audi-a6-6.jpg");

                car.AddFeature("S-Line");
                car.AddFeature("Bang & Olufsen Sound System");
                car.AddFeature("Virtual Cockpit");
            }

            if (car.BrandId == brands["Ford"] && car.Model == "Focus RS")
            {
                car.AddImage("images/cars/seed/ford-focus-1.jpg", isPrimary: true);
                car.AddImage("images/cars/seed/ford-focus-2.jpg");
                car.AddImage("images/cars/seed/ford-focus-3.jpg");
                car.AddImage("images/cars/seed/ford-focus-4.jpg");
                car.AddImage("images/cars/seed/ford-focus-5.jpg");
                car.AddImage("images/cars/seed/ford-focus-6.jpg");
            }

        }

        await context.Cars.AddRangeAsync(cars);
        await context.SaveChangesAsync();
        Console.WriteLine("? Dynamic seed: cars added (with images).");
    }

    private static async Task SeedInquiriesAsync(DatabaseContext context)
    {
        if (await context.Inquiries.AnyAsync())
            return;

        var users = await context.Users.AsNoTracking().ToDictionaryAsync(x => x.Username, x => x.Id);
        var cars = await context.Cars.AsNoTracking().ToDictionaryAsync(x => x.Model, x => x.Id);
        var statuses = await context.Statuses.AsNoTracking().ToDictionaryAsync(x => x.StatusName, x => x.Id);

        if (users.Count == 0 || cars.Count == 0 || statuses.Count == 0)
            return;

        var now = DateTime.UtcNow;
        var inquiries = new[]
        {
             new InquiryEntity
            {
                UserId = users["demo"],
                CarId = cars["C 350"],
                Subject = "Dostupnost Mercedes-Benz C 350 4 Matic",
                Message = "Zanima me je li vozilo odmah dostupno i da li je moguce probna vožnja ovaj tjedan?",
                PreferredContactMethod = "Email",
                StatusId = statuses["Pending"],
                CreatedAtUtc = now
            },
            new InquiryEntity
            {
                UserId = users["tester"],
                CarId = cars["330d"],
                Subject = "Leasing opcije",
                Message = "Možete li poslati ponudu leasing financiranja za BMW 330d na 5 godina?",
                PreferredContactMethod = "Phone",
                StatusId = statuses["Responded"],
                RespondedAtUtc = now,
                Response = "Poslali smo ponudu s kamatom 3.2% i mogucnošcu 20% ucešca.",
                CreatedAtUtc = now
            }
        };

        await context.Inquiries.AddRangeAsync(inquiries);
        await context.SaveChangesAsync();
        Console.WriteLine("? Dynamic seed: inquiries added.");
    }

    private static async Task SeedReviewsAsync(DatabaseContext context)
    {
        if (await context.Reviews.AnyAsync())
            return;

        var users = await context.Users.AsNoTracking().ToDictionaryAsync(x => x.Username, x => x.Id);
        var cars = await context.Cars.AsNoTracking().ToDictionaryAsync(x => x.Model, x => x.Id);

        if (users.Count == 0 || cars.Count == 0)
            return;

        var focusModelKey = cars.ContainsKey("Focus RS") ? "Focus RS" : cars.Keys.First();

        var now = DateTime.UtcNow;
        var reviews = new[]
        {
            new ReviewEntity
            {
                UserId = users["demo"],
                CarId = cars[focusModelKey],
                Rating = 5,
                Title = "Nevjerojatne performanse",
                Content = "Focus je tih i izuzetno brza, a autopilot je fantastican na dužim putovanjima.",
                RewievDate = now,
                IsApproved = true,
                CreatedAtUtc = now
            },
            new ReviewEntity
            {
                UserId = users["tester"],
                CarId = cars[focusModelKey],
                Rating = 4,
                Title = "Praktican i štedljiv",
                Content = "Odlican gradski SUV, jedino bi infotainment mogao biti brži.",
                RewievDate = now,
                IsApproved = false,
                CreatedAtUtc = now
            }
        };

        await context.Reviews.AddRangeAsync(reviews);
        await context.SaveChangesAsync();
        Console.WriteLine("? Dynamic seed: reviews added.");
    }

    /// <summary>
    /// Seed demo favourites (user-car relationships).
    /// </summary>
    private static async Task SeedFavouritesAsync(DatabaseContext context)
    {
        if (await context.Favs.AnyAsync())
            return;

        var users = await context.Users.AsNoTracking().ToDictionaryAsync(x => x.Username, x => x.Id);
        var cars = await context.Cars.AsNoTracking().ToDictionaryAsync(x => x.Model, x => x.Id);
        if (users.Count == 0 || cars.Count == 0)
            return;

        var now = DateTime.UtcNow;
        var favourites = new[]
        {
            new FavouriteEntity
            {
                UserId = users.ContainsKey("demo") ? users["demo"] : users.Values.First(),
                CarId = cars.ContainsKey("330d") ? cars["330d"] : cars.Values.First(),
                DateAdded = now,
                Notes = "Razmatram kupnju u skoroj buducnosti."
            },
            new FavouriteEntity
            {
                UserId = users.ContainsKey("tester") ? users["tester"] : users.Values.Last(),
                CarId = cars.ContainsKey("Mach") ? cars["Mach"] : cars.Values.Last(),
                DateAdded = now,
                Notes = "Svida mi se opcija elektricnog pogona."
            }
        };

        await context.Favs.AddRangeAsync(favourites);
        await context.SaveChangesAsync();
        Console.WriteLine("? Dynamic seed: favourites added.");
    }

    /// <summary>
    /// Seed demo carts with subtotal and taxes.
    /// </summary>
    private static async Task SeedCartsAsync(DatabaseContext context)
    {
        if (await context.Carts.AnyAsync())
            return;

        var users = await context.Users.AsNoTracking().ToDictionaryAsync(x => x.Username, x => x.Id);
        var cars = await context.Cars.AsNoTracking().ToDictionaryAsync(x => x.Model, x => x.Id);
        if (users.Count == 0 || cars.Count == 0)
            return;

        var now = DateTime.UtcNow;
        var carts = new[]
        {
            new CartEntity
            {
                UserId = users.ContainsKey("demo") ? users["demo"] : users.Values.First(),
                CarId = cars.ContainsKey("330d") ? cars["330d"] : cars.Values.First(),
                Subtotal = 45990m,
                Tax = 0m,
                CreatedAtUtc = now
            },
            new CartEntity
            {
                UserId = users.ContainsKey("tester") ? users["tester"] : users.Values.Last(),
                CarId = cars.ContainsKey("Mach") ? cars["Mach"] : cars.Values.Last(),
                Subtotal = 89990m,
                Tax = 0m,
                CreatedAtUtc = now
            }
        };

        foreach (var cart in carts)
        {
            cart.RecalculateTotal();
        }

        await context.Carts.AddRangeAsync(carts);
        await context.SaveChangesAsync();
        Console.WriteLine("? Dynamic seed: carts added.");
    }

    /// <summary>
    /// Seed demo orders referencing users, cars and statuses.
    /// </summary>
    private static async Task SeedOrdersAsync(DatabaseContext context)
    {
        if (await context.Orders.AnyAsync())
            return;

        var users = await context.Users.AsNoTracking().ToDictionaryAsync(x => x.Username, x => x.Id);
        var cars = await context.Cars.AsNoTracking().ToDictionaryAsync(x => x.Model, x => x.Id);
        var statuses = await context.Statuses.AsNoTracking().ToDictionaryAsync(x => x.StatusName, x => x.Id);
        if (users.Count == 0 || cars.Count == 0 || statuses.Count == 0)
            return;

        var now = DateTime.UtcNow;
        var orders = new[]
        {
            new OrderEntity
            {
                UserId = users.ContainsKey("demo") ? users["demo"] : users.Values.First(),
                CarId = cars.ContainsKey("C 350") ? cars["C 350"] : cars.Values.First(),
                OrderStatusId = statuses.ContainsKey("Confirmed") ? statuses["Confirmed"] : statuses.Values.First(),
                OrderDate = now.AddDays(-7),
                FinalAmount = 31500m,
                ShippingAddress = "Demo Street 1"
            },
            new OrderEntity
            {
                UserId = users.ContainsKey("tester") ? users["tester"] : users.Values.Last(),
                CarId = cars.ContainsKey("Mach") ? cars["Mach"] : cars.Values.Last(),
                OrderStatusId = statuses.ContainsKey("Pending") ? statuses["Pending"] : statuses.Values.Last(),
                OrderDate = now.AddDays(-3),
                FinalAmount = 89990m,
                ShippingAddress = "Test Street 1"
            }
        };

        await context.Orders.AddRangeAsync(orders);
        await context.SaveChangesAsync();
        Console.WriteLine("? Dynamic seed: orders added.");
    }

    /// <summary>
    /// Seed demo transactions for each order.
    /// </summary>
    private static async Task SeedTransactionsAsync(DatabaseContext context)
    {
        if (await context.Transactions.AnyAsync())
            return;

        var orders = await context.Orders.AsNoTracking().ToListAsync();
        var statuses = await context.Statuses.AsNoTracking().ToDictionaryAsync(x => x.StatusName, x => x.Id);
        if (orders.Count == 0 || statuses.Count == 0)
            return;

        var transactions = new List<TransactionEntity>();

        foreach (var order in orders)
        {
            transactions.Add(new TransactionEntity
            {
                OrderId = order.Id,
                TransactionDate = order.OrderDate.AddDays(1),
                Amount = order.FinalAmount,
                PaymentMethod = "Kartica",
                StatusId = statuses.ContainsKey("Completed") ? statuses["Completed"] : statuses.Values.First(),
                FinancingProvider = null,
                InterestRate = null,
                LoanTermMonths = null
            });
        }

        await context.Transactions.AddRangeAsync(transactions);
        await context.SaveChangesAsync();
        Console.WriteLine("? Dynamic seed: transactions added.");
    }

    /// <summary>
    /// Seed demo deliveries for each order.
    /// </summary>
    private static async Task SeedDeliveriesAsync(DatabaseContext context)
    {
        if (await context.Deliveries.AnyAsync())
            return;

        var orders = await context.Orders.AsNoTracking().ToListAsync();
        var statuses = await context.Statuses.AsNoTracking().ToDictionaryAsync(x => x.StatusName, x => x.Id);
        if (orders.Count == 0 || statuses.Count == 0)
            return;

        var deliveries = new List<DeliveryEntity>();
        var trackingNumber = 100000;

        foreach (var order in orders)
        {
            deliveries.Add(new DeliveryEntity
            {
                OrderId = order.Id,
                StatusId = statuses.ContainsKey("Shipped") ? statuses["Shipped"] : statuses.Values.First(),
                ScheduledDate = order.OrderDate.AddDays(2),
                DeliveredDate = order.OrderDate.AddDays(5),
                Address = order.ShippingAddress,
                TrackingNumber = trackingNumber++
            });
        }

        await context.Deliveries.AddRangeAsync(deliveries);
        await context.SaveChangesAsync();
        Console.WriteLine("? Dynamic seed: deliveries added.");
    }

    /// <summary>
    /// Seed demo test drive appointments.
    /// </summary>
    private static async Task SeedTestDrivesAsync(DatabaseContext context)
    {
        if (await context.Tests.AnyAsync())
            return;

        var users = await context.Users.AsNoTracking().ToDictionaryAsync(x => x.Username, x => x.Id);
        var cars = await context.Cars.AsNoTracking().ToDictionaryAsync(x => x.Model, x => x.Id);
        var statuses = await context.Statuses.AsNoTracking().ToDictionaryAsync(x => x.StatusName, x => x.Id);
        if (users.Count == 0 || cars.Count == 0 || statuses.Count == 0)
            return;

        var now = DateTime.UtcNow;
        var testDrives = new[]
        {
            new TestDriveEntity
            {
                UserId = users.ContainsKey("demo") ? users["demo"] : users.Values.First(),
                CarId = cars.ContainsKey("C 350") ? cars["C 350"] : cars.Values.First(),
                ScheduledDateTime = now.AddDays(1).AddHours(10),
                StatusId = statuses.ContainsKey("Confirmed") ? statuses["Confirmed"] : statuses.Values.First(),
                Notes = "Želim testirati performanse na autocesti."
            },
            new TestDriveEntity
            {
                UserId = users.ContainsKey("tester") ? users["tester"] : users.Values.Last(),
                CarId = cars.ContainsKey("Mach") ? cars["Mach"] : cars.Values.Last(),
                ScheduledDateTime = now.AddDays(2).AddHours(12),
                StatusId = statuses.ContainsKey("Requested") ? statuses["Requested"] : statuses.Values.First(),
                Notes = "Zanima me domet na bateriji."
            }
        };

        await context.Tests.AddRangeAsync(testDrives);
        await context.SaveChangesAsync();
        Console.WriteLine("? Dynamic seed: test drives added.");
    }

    /// <summary>
    /// Seed demo service appointments.
    /// </summary>
    private static async Task SeedServiceAppointmentsAsync(DatabaseContext context)
    {
        if (await context.Services.AnyAsync())
            return;

        var users = await context.Users.AsNoTracking().ToDictionaryAsync(x => x.Username, x => x.Id);
        var cars = await context.Cars.AsNoTracking().ToDictionaryAsync(x => x.Model, x => x.Id);
        var statuses = await context.Statuses.AsNoTracking().ToDictionaryAsync(x => x.StatusName, x => x.Id);
        if (users.Count == 0 || cars.Count == 0 || statuses.Count == 0)
            return;

        var now = DateTime.UtcNow;
        var appointments = new[]
        {
            new ServiceAppointmentEntity
            {
                UserId = users.ContainsKey("demo") ? users["demo"] : users.Values.First(),
                CarId = cars.ContainsKey("C 350") ? cars["C 350"] : cars.Values.First(),
                ServiceType = "Mali servis",
                CustomerNotes = "Provjeriti kocnice i ulje.",
                ScheduledDateTime = now.AddDays(3).AddHours(9),
                StatusId = statuses.ContainsKey("Pending") ? statuses["Pending"] : statuses.Values.First()
            },
            new ServiceAppointmentEntity
            {
                UserId = users.ContainsKey("tester") ? users["tester"] : users.Values.Last(),
                CarId = cars.ContainsKey("Mach") ? cars["Mach"] : cars.Values.Last(),
                ServiceType = "Zamjena guma",
                CustomerNotes = null,
                ScheduledDateTime = now.AddDays(4).AddHours(11),
                StatusId = statuses.ContainsKey("Confirmed") ? statuses["Confirmed"] : statuses.Values.First()
            }
        };

        await context.Services.AddRangeAsync(appointments);
        await context.SaveChangesAsync();
        Console.WriteLine("? Dynamic seed: service appointments added.");
    }

    /// <summary>
    /// Seed demo service records for each appointment.
    /// </summary>
    private static async Task SeedServiceRecordsAsync(DatabaseContext context)
    {
        if (await context.serviceRecords.AnyAsync())
            return;

        var appointments = await context.Services.AsNoTracking().ToListAsync();
        if (appointments.Count == 0)
            return;

        var records = new List<ServiceRecordEntity>();

        foreach (var appointment in appointments)
        {
            records.Add(new ServiceRecordEntity
            {
                AppointmentId = appointment.Id,
                ServiceDate = appointment.ScheduledDateTime,
                MileageAtService = 15000,
                WorkDescription = "Redovni servis: zamjena ulja, filtera i pregled kocnica.",
                PartsUsed = "Motorno ulje, filter ulja",
                LaborCost = 80,
                PartsCost = 30,
                TotalCost = 110
            });
        }

        await context.serviceRecords.AddRangeAsync(records);
        await context.SaveChangesAsync();
        Console.WriteLine("? Dynamic seed: service records added.");
    }
}

