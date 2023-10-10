using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ReviewApp.Models;
using ReviewApp.Repository;


namespace ReviewApp.Filters.IActionFilters
{
    public class Product_ValidateProductIdFilterAttribute : ActionFilterAttribute
    {
        IProductRepository _ProdectRepository;

        public Product_ValidateProductIdFilterAttribute(IProductRepository ProdectRepository)
        {
            _ProdectRepository = ProdectRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);


            var ID = context.ActionArguments["ProductID"] as int?;
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
                else if (!_ProdectRepository.ProductExist(ID))
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
