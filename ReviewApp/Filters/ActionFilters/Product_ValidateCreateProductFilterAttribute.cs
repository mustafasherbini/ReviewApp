using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ReviewApp.DTO;
using ReviewApp.Repository;

namespace ReviewApp.Filters.IActionFilters
{
    public class Product_ValidateCreateProductFilterAttribute : ActionFilterAttribute
    {

        IProductRepository _ProductRepository;

        public Product_ValidateCreateProductFilterAttribute(IProductRepository ProductRepository)
        {
            _ProductRepository = ProductRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);


            var Productobject = context.ActionArguments["ProductCreate"] as ProductDTO;
            var Products = _ProductRepository.GetProduct(Productobject.Name);
            if (Products != null)
            {
              
                context.ModelState.AddModelError("Product", "Product alredy exists ");
                var problemDeatails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDeatails);

            }






        }

    }
}
