using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ReviewApp.Models;
using ReviewApp.Repository;


namespace ReviewApp.Filters.IActionFilters
{
    public class Country_ValidateCountryIdFilterAttribute : ActionFilterAttribute
    {
        ICountryRepository _CountryRepository;

        public Country_ValidateCountryIdFilterAttribute(ICountryRepository CountryRepository)
        {
            _CountryRepository = CountryRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);


            var CountryID = context.ActionArguments["id"] as int?;
            if (CountryID != null)
            {
                if (CountryID <= 0)
                {

                    context.ModelState.AddModelError("CountryID", "CountryID is invalid");
                    var problemDeatails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDeatails);

                }
                else if (!_CountryRepository.CountryExist(CountryID))
                {
                    context.ModelState.AddModelError("CountryID", "Country doesn't exist");

                    var problemDeatails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };
                    context.Result = new NotFoundObjectResult(problemDeatails);
                }
            }

        }
    }
}
