using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ReviewApp.DTO;
using ReviewApp.Repository;

namespace ReviewApp.Filters.IActionFilters
{
    public class Owner_ValidateCreateOwnerFilterAttribute : ActionFilterAttribute
    {

        IOwnerRepository _OwnerRepository;

        public Owner_ValidateCreateOwnerFilterAttribute(IOwnerRepository OwnerRepository)
        {
            _OwnerRepository = OwnerRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);


            var ownerobject = context.ActionArguments["OwnerCreate"] as OwnerDTO;
            var Owners = _OwnerRepository.GetAll()
               .Where(c => c.LastName.Trim().ToUpper() == ownerobject.LastName.Trim().ToUpper()).
               FirstOrDefault();


            if (Owners != null)
            {
                context.ModelState.AddModelError("owner", "owner alredy exists ");
                var problemDeatails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDeatails);
            }



        }

    }
}
