using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using ReviewApp.Data;
using ReviewApp.Models;
using ReviewApp.Repository;


namespace ReviewApp.Filters.IActionFilters
{
    public class Category_ValidateCategoryIdFilterAttribute : ActionFilterAttribute
    {
        ICategoryRepository _CategoryRepository;
        DataContext _dataContext;

        public Category_ValidateCategoryIdFilterAttribute(ICategoryRepository CategoryRepository)
        {
            _CategoryRepository = CategoryRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
           

            var ID = context.ActionArguments["CategoryID"] as int?;
           
                if (ID <= 0)
                {

                    context.ModelState.AddModelError("CategoryID", "CategoryID is invalid");
                    var problemDeatails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDeatails);

                }
                else 
                {
                var category = _CategoryRepository.GetCategoryById(ID);
                if (category == null)
                {
                    context.ModelState.AddModelError("CategoryID", "Category doesn't exist");

                    var problemDeatails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };
                    context.Result = new NotFoundObjectResult(problemDeatails);

                }else
                {
                    context.HttpContext.Items["category"] = category;
                    _dataContext.Entry(category).State = EntityState.Detached;

                }

            }
            

        }
    }
}
