using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ReviewApp.DTO;
using ReviewApp.Repository;

namespace ReviewApp.Filters.IActionFilters
{
    public class Category_ValidateCreateCategoryFilterAttribute : ActionFilterAttribute
    {

        ICategoryRepository _CategoryRepository;

        public Category_ValidateCreateCategoryFilterAttribute(ICategoryRepository CategoryRepository)
        {
            _CategoryRepository = CategoryRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);


               var categoryobject = context.ActionArguments["categoryCreate"] as CategoryDTO ;
               var Category = _CategoryRepository.GetCategoryByName(categoryobject.Name);
            if (Category != null) {
                context.ModelState.AddModelError("Category", "Category alredy exists ");
                var problemDeatails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDeatails);
            }

          

        }

    }
}
