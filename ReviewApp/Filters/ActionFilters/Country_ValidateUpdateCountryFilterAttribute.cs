using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ReviewApp.DTO;
using ReviewApp.Models;
using ReviewApp.Repository;


namespace ReviewApp.Filters.IActionFilters
{
    public class Country_ValidateUpdateCountryFilterAttribute : ActionFilterAttribute
    {
        ICountryRepository _CountryRepository;

        public Country_ValidateUpdateCountryFilterAttribute(ICountryRepository countryRepository)
        {
            _CountryRepository = countryRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);


            var CountryID = context.ActionArguments["id"] as int?;
            var Countryobject = context.ActionArguments["upcountry"] as CountryDTO;

            if (Countryobject == null)
            {
                context.ModelState.AddModelError("Country", "Country is invalid");
                var problemDeatails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDeatails);

            }else if (CountryID != Countryobject.Id)
            {
                context.ModelState.AddModelError("CountryID", "CountryID is not the same as the object id");
                var problemDeatails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDeatails);

            }


        }
    }
}
