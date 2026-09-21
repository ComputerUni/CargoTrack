using CargoTrack.Business;
using CargoTrack.Business.Mappings.CargoMappings;
using CargoTrack.Business.Services.Abouts;
using CargoTrack.Business.Services.AuditLogs;
using CargoTrack.Business.Services.Branches;
using CargoTrack.Business.Services.CargoMovements;
using CargoTrack.Business.Services.CargoPricings;
using CargoTrack.Business.Services.Cargos;
using CargoTrack.Business.Services.Cities;
using CargoTrack.Business.Services.Deliveries;
using CargoTrack.Business.Services.DeliveryExceptions;
using CargoTrack.Business.Services.Employees;
using CargoTrack.Business.Services.TransferCenters;
using CargoTrack.DataAccess.Context;
using CargoTrack.DataAccess.Repositories.Abouts;
using CargoTrack.DataAccess.Repositories.AuditLogs;
using CargoTrack.DataAccess.Repositories.Branches;
using CargoTrack.DataAccess.Repositories.CargoMovements;
using CargoTrack.DataAccess.Repositories.CargoPrices;
using CargoTrack.DataAccess.Repositories.Cargos;
using CargoTrack.DataAccess.Repositories.Cities;
using CargoTrack.DataAccess.Repositories.Deliveries;
using CargoTrack.DataAccess.Repositories.DeliveryExceptions;
using CargoTrack.DataAccess.Repositories.Employees;
using CargoTrack.DataAccess.Repositories.TransferCenters;
using CargoTrack.Entity.Entities;
using CargoTrack.Entity.Entities.Enums;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//IOC Container
builder.Services.AddFluentValidationAutoValidation()
    .AddValidatorsFromAssemblyContaining<BusinessAssembly>();
    //.AddValidatorsFromAssembly(typeof(BusinessAssembly).Assembly);

builder.Services.AddScoped<IAboutRepository, AboutRepository>();
builder.Services.AddScoped<ICargoRepository, CargoRepository>();
builder.Services.AddScoped<IBranchRepository, BranchRepository>();
builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();
builder.Services.AddScoped<ICargoMovementRepository, CargoMovementRepository>();
builder.Services.AddScoped<ICargoPricesRepository, CargoPricesRepository>();
builder.Services.AddScoped<IDeliveryRepository, DeliveryRepository>();
builder.Services.AddScoped<IDeliveryExceptionRepository, DeliveryExceptionRepository>();
builder.Services.AddScoped<ITransferCenterRepository, TransferCenterRepository>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

builder.Services.AddScoped<IAboutService, AboutService>();
builder.Services.AddScoped<IBranchService, BranchService>();
builder.Services.AddScoped<IAuditLogService, AuditLogService>();
builder.Services.AddScoped<ICargoMovementService, CargoMovementService>();
builder.Services.AddScoped<ICargoPricingService, CargoPricingService>();
builder.Services.AddScoped<ICargoService, CargoService>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<IDeliveryService, DeliveryService>();
builder.Services.AddScoped<IDeliveryExceptionService, DeliveryExceptionService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<ITransferCenterService, TransferCenterService>();


builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaulConnection"));
    options.UseLazyLoadingProxies();
});

builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<AppDbContext>();
builder.Services.ConfigureApplicationCookie(config =>
{
    config.LoginPath = "/Login/Index";
    config.LogoutPath = "/Login/Logout";
    config.AccessDeniedPath = "/ErrorPages/AccessDenied";
    config.Cookie.Name = "CargoTrackCookie";
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
          );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    context.Database.EnsureCreated();
    if (!context.Cities.Any())
    {
        var cities = new List<City>
        {
            new City { Id = Guid.NewGuid(), Name = "Ýstanbul" },
            new City { Id = Guid.NewGuid(), Name = "Ankara" },
            new City { Id = Guid.NewGuid(), Name = "Ýzmir" },
            new City { Id = Guid.NewGuid(), Name = "Bursa" },
            new City { Id = Guid.NewGuid(), Name = "Antalya" },
            new City { Id = Guid.NewGuid(), Name = "Adana" },
            new City { Id = Guid.NewGuid(), Name = "Konya" },
            new City { Id = Guid.NewGuid(), Name = "Þanlýurfa" },
            new City { Id = Guid.NewGuid(), Name = "Gaziantep" },
            new City { Id = Guid.NewGuid(), Name = "Kocaeli" },
            new City { Id = Guid.NewGuid(), Name = "Mersin" },
            new City { Id = Guid.NewGuid(), Name = "Diyarbakýr" },
            new City { Id = Guid.NewGuid(), Name = "Hatay" },
            new City { Id = Guid.NewGuid(), Name = "Kayseri" },
            new City { Id = Guid.NewGuid(), Name = "Samsun" },
            new City { Id = Guid.NewGuid(), Name = "Balýkesir" },
            new City { Id = Guid.NewGuid(), Name = "Kahramanmaraþ" },
            new City { Id = Guid.NewGuid(), Name = "Van" },
            new City { Id = Guid.NewGuid(), Name = "Aydýn" },
            new City { Id = Guid.NewGuid(), Name = "Tekirdað" }
        };

        context.Cities.AddRange(cities);
        context.SaveChanges();
    }

    if(!context.Roles.Any())
    {
        var roles = new List<AppRole>
        {
            new AppRole{Name="Admin"},
            new AppRole{Name="Manager"},
            new AppRole{Name="User"},
        };

        context.Roles.AddRange(roles);
        context.SaveChanges();
    }

    //if (!context.Cargos.Any())
    //{
    //    var cargo = new Cargo
    //    {
    //        Id = Guid.NewGuid(),
    //        SenderId = Guid.Parse("9b7f437a-f6ce-41bf-399a-08df085cb218"),
    //        ReceiverId = Guid.Parse("f1b8f752-63fd-44a3-399b-08df085cb218"),
    //        OriginBranchId = Guid.Parse("fc847b38-61a8-40e7-81a8-67cd79941988"),
    //        DestinationBranchId = Guid.Parse("54bfeb0a-75da-4d9c-8cd1-410ad93dbb9a"),
    //        TrackCode = "CT20260908202612345",
    //        ShipmentDate = DateTime.Now,
    //        EstimatedArrivalDate = DateTime.Now.AddDays(2),
    //        Weight = 2.5,
    //        CargoType = CargoType.Standart,
    //        CargoStatus = CargoStatus.DispatchedFromTransferCenter
    //    };
    //    context.Add(cargo);
    //    context.SaveChanges();
    //}
}

CargoMappingConfig.RegisterMappings();

app.Run();


