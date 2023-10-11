using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using ReviewApp.Data;
using ReviewApp.Models;
using ReviewApp.Repository;
using System.Diagnostics.Metrics;


namespace ReviewApp.Filters.IActionFilters
{
    public class Owner_ValidateOwnerIdFilterAttribute : ActionFilterAttribute
    {
        IOwnerRepository _ownerRepository;
        DataContext _dataContext;
        public Owner_ValidateOwnerIdFilterAttribute(IOwnerRepository ownerRepository , DataContext dataContext)
        {
            _ownerRepository = ownerRepository;
            _dataContext = dataContext;
                
        }

      
        public override void OnActionExecuting(ActionExecutingContext context )
        {
            base.OnActionExecuting(context);


            var ID = context.ActionArguments["OwnerID"] as int?;
            
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
                var owner = _ownerRepository.GetOwner(ID);

                if (owner == null)
                {

                    context.ModelState.AddModelError("ID", "Owner doesn't exist");

                    var problemDeatails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };
                    context.Result = new NotFoundObjectResult(problemDeatails);
                }
                else { context.HttpContext.Items["owner"] = owner;

                    _dataContext.Entry(owner).State = EntityState.Detached;
                }

            }
            

        }
    }
}
