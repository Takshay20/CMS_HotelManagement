using CMS_HotelBooking.Data;
using CMS_HotelBooking.Filters;
using CMS_HotelBooking.Helpers;
using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Implementations;
using CMS_HotelBooking.Repository.Interfaces;
using CMS_HotelBooking.Services;
using CMS_HotelBooking.Services.Background;
using CMS_HotelBooking.Services.Implementations;
using CMS_HotelBooking.Services.Interfaces;
using Dapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IDbConnectionFactory, SqlConnectionFactory>();

builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IHomeWelcomeRepository, HomeWelcomeRepository>();
builder.Services.AddScoped<IHomeWhyChooseUsRepository, HomeWhyChooseUsRepository>();
builder.Services.AddScoped<ISliderRepository, SliderRepository>();
builder.Services.AddScoped<IRoomCategoryRepository, RoomCategoryRepository>();
builder.Services.AddScoped<IAmenityRepository, AmenityRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IFacilityRepository, FacilityRepository>();
builder.Services.AddScoped<IGalleryRepository, GalleryRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IBookingReminderRepository, BookingReminderRepository>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
builder.Services.AddScoped<IContactMessageRepository, ContactMessageRepository>();
builder.Services.AddScoped<IMenuMasterRepository, MenuMasterRepository>();
builder.Services.AddScoped<IFooterRepository, FooterRepository>();
builder.Services.AddScoped<ISocialMediaRepository, SocialMediaRepository>();
builder.Services.AddScoped<ISiteSettingRepository, SiteSettingRepository>();
builder.Services.AddScoped<IAboutRepository, AboutRepository>();
builder.Services.AddScoped<IAboutStoryRepository, AboutStoryRepository>();
builder.Services.AddScoped<IAboutReceptionRepository, AboutReceptionRepository>();
builder.Services.AddScoped<IAboutCounterRepository, AboutCounterRepository>();
builder.Services.AddScoped<IAboutCtaRepository, AboutCtaRepository>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IRestoreRecordsRepository, RestoreRecordsRepository>();

builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IHomeWelcomeService, HomeWelcomeService>();
builder.Services.AddScoped<IHomeWhyChooseUsService, HomeWhyChooseUsService>();
builder.Services.AddScoped<ISliderService, SliderService>();
builder.Services.AddScoped<IRoomCategoryService, RoomCategoryService>();
builder.Services.AddScoped<IAmenityService, AmenityService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IFacilityService, FacilityService>();
builder.Services.AddScoped<IGalleryService, GalleryService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<RazorpaySettings>(builder.Configuration.GetSection("Razorpay"));
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IBookingReminderService, BookingReminderService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

builder.Services.AddHostedService<BookingReminderWorker>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<IContactMessageService, ContactMessageService>();
builder.Services.AddScoped<IMenuMasterService, MenuMasterService>();
builder.Services.AddScoped<IFooterService, FooterService>();
builder.Services.AddScoped<ISocialMediaService, SocialMediaService>();
builder.Services.AddScoped<ISiteSettingService, SiteSettingService>();
builder.Services.AddScoped<IAboutService, AboutService>();
builder.Services.AddScoped<IAboutStoryService, AboutStoryService>();
builder.Services.AddScoped<IAboutReceptionService, AboutReceptionService>();
builder.Services.AddScoped<IAboutCounterService, AboutCounterService>();
builder.Services.AddScoped<IAboutCtaService, AboutCtaService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IRestoreRecordsService, RestoreRecordsService>();
builder.Services.AddScoped<RestoreRestrictionFilter>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/Login";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true;
        options.Cookie.Name = "CMS_HotelBooking.Auth";
    });

builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    try
    {
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var factory = scope.ServiceProvider.GetRequiredService<IDbConnectionFactory>();
        using IDbConnection connection = factory.CreateConnection();

        var adminExists = connection.QueryFirstOrDefault<int>(
            "SELECT COUNT(*) FROM Users WHERE Role = 'Admin'");

        if (adminExists == 0)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@FullName", "Hotel Administrator");
            parameter.Add("@Email", "royalparadisehotels15@gmail.com");
            parameter.Add("@PasswordHash", PasswordHelper.Hash("Admin@123"));
            parameter.Add("@Phone", "9999999999");
            parameter.Add("@Role", "Admin");
            connection.Execute("sp_RegisterUser", parameter, commandType: CommandType.StoredProcedure);
        }
    }
    catch (Exception ex)
    {
        
        Console.WriteLine("Admin seed check skipped: " + ex.Message);
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "Admin",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Dashboard}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
