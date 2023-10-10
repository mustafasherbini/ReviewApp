using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ReviewApp.Models;
using ReviewApp.Repository;


namespace ReviewApp.Filters.IActionFilters
{
    public class Owner_ValidateOwnerIdFilterAttribute : ActionFilterAttribute
    {
        IOwnerRepository _ownerRepository;

        public Owner_ValidateOwnerIdFilterAttribute(IOwnerRepository ownerRepository)
        {
            _ownerRepository = ownerRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);


            var ID = context.ActionArguments["ID"] as int?;
            if (ID != null)
            {
                if (ID <= 0)
                {

                    context.ModelState.AddModelError("ID", "ID is invalid");
                    var problemDeatails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDeatails);

                }
                else if (!_ownerRepository.OwnerExist(ID))
                {
                    context.ModelState.AddModelError("ID", "Prodect doesn't exist");

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
