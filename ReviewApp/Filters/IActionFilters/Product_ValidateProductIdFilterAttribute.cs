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


            var ProdectID = context.ActionArguments["id"] as int?;
            if (ProdectID != null)
            {
                if (ProdectID <= 0)
                {

                    context.ModelState.AddModelError("ProdectID", "ProdectID is invalid");
                    var problemDeatails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDeatails);

                }
                else if (!_ProdectRepository.ProductExist(ProdectID))
                {
                    context.ModelState.AddModelError("ProdectID", "Prodect doesn't exist");

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
