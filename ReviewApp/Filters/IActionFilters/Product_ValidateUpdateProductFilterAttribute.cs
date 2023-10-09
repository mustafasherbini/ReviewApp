using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ReviewApp.DTO;
using ReviewApp.Models;
using ReviewApp.Repository;


namespace ReviewApp.Filters.IActionFilters
{
    public class Product_ValidateUpdateProductFilterAttribute : ActionFilterAttribute
    {
        IProductRepository _ProductRepository;

        public Product_ValidateUpdateProductFilterAttribute(IProductRepository productRepository)
        {
            _ProductRepository = productRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);


            var productID = context.ActionArguments["id"] as int?;
            var Productobject = context.ActionArguments["upproduct"] as ProductDTO;

            if (Productobject == null)
            {
                context.ModelState.AddModelError("product", "product is invalid");
                var problemDeatails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDeatails);

            }
            else if (productID != Productobject.Id)
            {
                context.ModelState.AddModelError("productID", "productID is not the same as the object id");
                var problemDeatails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDeatails);

            }


        }
    }
}
