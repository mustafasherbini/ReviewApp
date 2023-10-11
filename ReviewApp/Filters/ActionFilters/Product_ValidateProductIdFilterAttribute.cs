using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using ReviewApp.Data;
using ReviewApp.Models;
using ReviewApp.Repository;


namespace ReviewApp.Filters.IActionFilters
{
    public class Product_ValidateProductIdFilterAttribute : ActionFilterAttribute
    {
        IProductRepository _ProdectRepository;
        DataContext _dataContext;
        public Product_ValidateProductIdFilterAttribute(IProductRepository ProdectRepository, DataContext dataContext)
        {
            _ProdectRepository = ProdectRepository;
            _dataContext = dataContext;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);


            var ID = context.ActionArguments["ProductID"] as int?;
           
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
                var product = _ProdectRepository.GetProduct(ID);
                if (product == null)
                {
                    context.ModelState.AddModelError("ID", "Product doesn't exist");

                    var problemDeatails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };
                    context.Result = new NotFoundObjectResult(problemDeatails);


                }
                else
                {
                    context.HttpContext.Items["product"] = product;
                    _dataContext.Entry(product).State = EntityState.Detached;


                }
            }
            

        }
    }
}
