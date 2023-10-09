using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ReviewApp.Models;
using ReviewApp.Repository;


namespace ReviewApp.Filters.IActionFilters
{
    public class Category_ValidateCategoryIdFilterAttribute : ActionFilterAttribute
    {
        ICategoryRepository _CategoryRepository;

        public Category_ValidateCategoryIdFilterAttribute(ICategoryRepository CategoryRepository)
        {
            _CategoryRepository = CategoryRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);


            var categoryID = context.ActionArguments["id"] as int?;
            if (categoryID != null)
            {
                if (categoryID <= 0)
                {

                    context.ModelState.AddModelError("CategoryID", "CategoryID is invalid");
                    var problemDeatails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDeatails);

                }
                else if (!_CategoryRepository.DoesCategoryExist(categoryID))
                {
                    context.ModelState.AddModelError("CategoryID", "Category doesn't exist");

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
