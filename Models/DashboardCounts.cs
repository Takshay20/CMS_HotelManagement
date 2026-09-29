namespace CMS_HotelBooking.Models
{
    public class DashboardCounts
    {
        public int TotalRooms { get; set; }
        public int TotalCategories { get; set; }
        public int TotalUsers { get; set; }
        public int TotalBookings { get; set; }
        public int PendingBookings { get; set; }
        public int ApprovedBookings { get; set; }
        public int RejectedBookings { get; set; }
        public int CancelledBookings { get; set; }
        public int UnreadMessages { get; set; }
        public int PendingFeedback { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TodayCheckIns { get; set; }
        public int TodayCheckOuts { get; set; }
    }
}
