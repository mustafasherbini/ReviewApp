using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using ReviewApp.Data;
using ReviewApp.Models;
using ReviewApp.Repository;


namespace ReviewApp.Filters.IActionFilters
{
    public class Country_ValidateCountryIdFilterAttribute : ActionFilterAttribute
    {
        ICountryRepository _CountryRepository;
        DataContext _dataContext;

        public Country_ValidateCountryIdFilterAttribute(DataContext dataContext, ICountryRepository CountryRepository)
        {
            _CountryRepository = CountryRepository;
            _dataContext = dataContext;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);


            var ID = context.ActionArguments["CountryID"] as int?;
            
                if (ID <= 0)
                {

                    context.ModelState.AddModelError("ID", "ID is invalid");
                    var problemDeatails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDeatails);

                }
                else 
                {
                var country = _CountryRepository.GetCountry(ID);
                if (country == null)
                {
                    context.ModelState.AddModelError("ID", "Country doesn't exist");

                    var problemDeatails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };
                    context.Result = new NotFoundObjectResult(problemDeatails);
                }
                else context.HttpContext.Items["country"] = country;
                _dataContext.Entry(country).State = EntityState.Detached;

            }


        }
    }
}
