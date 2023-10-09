using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ReviewApp.DTO;
using ReviewApp.Models;
using ReviewApp.Repository;


namespace ReviewApp.Filters.IActionFilters
{
    public class Category_ValidateUpdateCategoryFilterAttribute : ActionFilterAttribute
    {
        ICategoryRepository _CategoryRepository;

        public Category_ValidateUpdateCategoryFilterAttribute(ICategoryRepository CategoryRepository)
        {
            _CategoryRepository = CategoryRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);


            var categoryID = context.ActionArguments["id"] as int?;
            var categoryobject = context.ActionArguments["upcategory"] as CategoryDTO;
                
            if (categoryobject == null)
            {
                context.ModelState.AddModelError("Category", "Category is invalid");
                var problemDeatails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDeatails);

            }else if (categoryID!= categoryobject.Id){
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
