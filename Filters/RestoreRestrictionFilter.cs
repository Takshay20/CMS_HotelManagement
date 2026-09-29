using CMS_HotelBooking.Repository;
using CMS_HotelBooking.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CMS_HotelBooking.Filters
{
    public class RestoreRestrictionFilter : IAsyncActionFilter
    {
        private readonly IHomeWhyChooseUsRepository _repository;

        public RestoreRestrictionFilter(
            IHomeWhyChooseUsRepository repository)
        {
            _repository = repository;
        }

        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            if (!context.ActionArguments.TryGetValue("id", out var idValue))
            {
                context.Result = new BadRequestObjectResult(
                    new
                    {
                        success = false,
                        message = "Record id is required."
                    });

                return;
            }

            if (!int.TryParse(idValue?.ToString(), out int id))
            {
                context.Result = new BadRequestObjectResult(
                    new
                    {
                        success = false,
                        message = "Invalid record id."
                    });

                return;
            }

            var deletedDate =
                await _repository.GetDeletedDateAsync(id);

            if (!deletedDate.HasValue)
            {
                context.Result = new JsonResult(
                    new
                    {
                        success = false,
                        message = "Deleted date not found."
                    });

                return;
            }

            if (DateTime.Now < deletedDate.Value.AddHours(1))
            {
                context.Result = new JsonResult(
                    new
                    {
                        success = false,
                        message = "This record can be restored only after 1 hour."
                    });

                return;
            }

            await next();
        }
    }
}