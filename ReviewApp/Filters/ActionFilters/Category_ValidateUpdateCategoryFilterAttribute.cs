using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ReviewApp.DTO;
using ReviewApp.Models;
using ReviewApp.Repository;


namespace ReviewApp.Filters.IActionFilters
{
    public class Category_ValidateUpdateCategoryFilterAttribute : ActionFilterAttribute
    {
       

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);


            var CategoryFilterID = context.ActionArguments["CategoryID"] as int?;
            var categoryobject = context.ActionArguments["upcategory"] as CategoryDTO;
                
           if (CategoryFilterID!= categoryobject.Id){
                    context.ModelState.AddModelError("CategoryID", "CategoryID is not the same as the object id");
                    var problemDeatails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDeatails);

           }
            

        }
    }
}
