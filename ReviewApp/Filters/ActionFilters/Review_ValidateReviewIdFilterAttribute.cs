using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using ReviewApp.Data;
using ReviewApp.Models;
using ReviewApp.Repository;


namespace ReviewApp.Filters.IActionFilters
{
    public class Review_ValidateReviewIdFilterAttribute : ActionFilterAttribute
    {
        IReviewRepository _ReviewRepository;
        DataContext _dataContext;



        public Review_ValidateReviewIdFilterAttribute(IReviewRepository reviewRepository , DataContext dataContext)
        {
            _ReviewRepository = reviewRepository;
            _dataContext = dataContext;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);


            var ID = context.ActionArguments["ReviewID"] as int?;
          
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
                var review = _ReviewRepository.GetReview(ID);
                if (review == null)
                {
                    context.ModelState.AddModelError("ID", "Review doesn't exist");

                    var problemDeatails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };
                    context.Result = new NotFoundObjectResult(problemDeatails);

                }
                else { context.HttpContext.Items["review"] = review;
                    _dataContext.Entry(review).State = EntityState.Detached;

                }
            }
            

        }
    }
}
