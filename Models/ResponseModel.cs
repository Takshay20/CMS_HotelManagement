namespace CMS_HotelBooking.Models
{
    public class ResponseModel
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; }
        public static ResponseModel SuccessResponse(string message, object? data = null)
        {
            return new ResponseModel
            {
                Success = true,
                Message = message,
                Data = data
            };
        }
        public static ResponseModel ErrorResponse(string message)
        {
            return new ResponseModel
            {
                Success = false,
                Message = message
            };
        }
    }
}