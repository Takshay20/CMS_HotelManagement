namespace CMS_HotelBooking.Helpers
{
    public static class RefundPolicyHelper
    {
        
        public static (int Percentage, decimal Amount) Calculate(DateTime checkInDate, decimal totalPrice)
        {
            var hoursUntilCheckIn = (checkInDate - DateTime.Now).TotalHours;

            int percentage;
            if (hoursUntilCheckIn >= 48)
                percentage = 100;
            else if (hoursUntilCheckIn >= 24)
                percentage = 50;
            else
                percentage = 0;

            var amount = Math.Round(totalPrice * percentage / 100m, 0);
            return (percentage, amount);
        }

        public static string PolicyText(int percentage) => percentage switch
        {
            100 => "You are eligible for a full refund since you're cancelling 48+ hours before check-in.",
            50 => "As per our policy, you're eligible for a 50% refund since you're cancelling between 24-48 hours before check-in.",
            _ => "As per our policy, no refund is applicable since the cancellation is within 24 hours of check-in."
        };
    }
}
