using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ReviewApp.DTO;
using ReviewApp.Repository;

namespace ReviewApp.Filters.IActionFilters
{
    public class Country_ValidateCreateCountryFilterAttribute : ActionFilterAttribute
    {

        ICountryRepository _CountryRepository;

        public Country_ValidateCreateCountryFilterAttribute(ICountryRepository CountryRepository)
        {
            _CountryRepository = CountryRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);


            var countryobject = context.ActionArguments["CountryCreate"] as CountryDTO;
            var Country = _CountryRepository.GetCountryByName(countryobject.Name);
            if (Country != null)
            {
                context.ModelState.AddModelError("Country", "Country alredy exists ");
                var problemDeatails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDeatails);
            }



        }

    }
}
