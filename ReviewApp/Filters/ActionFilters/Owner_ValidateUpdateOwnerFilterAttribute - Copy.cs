using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ReviewApp.DTO;
using ReviewApp.Models;
using ReviewApp.Repository;


namespace ReviewApp.Filters.IActionFilters
{
    public class Owner_ValidateUpdateOwnerFilterAttribute : ActionFilterAttribute
    {
        IOwnerRepository _OwnerRepository;

        public Owner_ValidateUpdateOwnerFilterAttribute(IOwnerRepository OwnerRepository)
        {
            _OwnerRepository = OwnerRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);


            var OwnerID = context.ActionArguments["OwnerID"] as int?;
            var Ownerobject = context.ActionArguments["upowner"] as OwnerDTO;

            if (Ownerobject == null)
            {
                context.ModelState.AddModelError("Owner", "Owner is invalid");
                var problemDeatails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDeatails);

            }
            else if (OwnerID != Ownerobject.Id)
            {
                context.ModelState.AddModelError("OwnerID", "OwnerID is not the same as the object id");
                var problemDeatails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDeatails);

            }


        }
    }
}
