using CMS_HotelBooking.Services.Interfaces;

namespace CMS_HotelBooking.Services.Background
{
    public class BookingReminderWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<BookingReminderWorker> _logger;

        public BookingReminderWorker(
            IServiceScopeFactory scopeFactory,
            ILogger<BookingReminderWorker> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Booking Reminder Worker started."
            );

            await ProcessRemindersAsync(stoppingToken);

            using var timer = new PeriodicTimer(
                TimeSpan.FromHours(1)
            );

            while (
                await timer.WaitForNextTickAsync(stoppingToken)
            )
            {
                await ProcessRemindersAsync(stoppingToken);
            }
        }

        private async Task ProcessRemindersAsync(
            CancellationToken cancellationToken)
        {
            try
            {
                using var scope =
                    _scopeFactory.CreateScope();

                var reminderService =
                    scope.ServiceProvider
                        .GetRequiredService<IBookingReminderService>();

                await reminderService.ProcessRemindersAsync();

                _logger.LogInformation(
                    "Booking reminder check completed at {Time}",
                    DateTime.Now
                );
            }
            catch (OperationCanceledException)
            {
                
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error while processing booking reminders."
                );
            }
        }
    }
}