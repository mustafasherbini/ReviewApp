using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ReviewApp.Models;
using ReviewApp.Repository;


namespace ReviewApp.Filters.IActionFilters
{
    public class Review_ValidateReviewIdFilterAttribute : ActionFilterAttribute
    {
        IReviewRepository _ReviewRepository;

        public Review_ValidateReviewIdFilterAttribute(ReviewRepository ReviewRepository)
        {
            _ReviewRepository = ReviewRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);


            var ReviewID = context.ActionArguments["id"] as int?;
            if (ReviewID != null)
            {
                if (ReviewID <= 0)
                {

                    context.ModelState.AddModelError("ReviewID", "ReviewID is invalid");
                    var problemDeatails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDeatails);

                }
                else if (!_ReviewRepository.ReviewExists(ReviewID))
                {
                    context.ModelState.AddModelError("ReviewID", "Review doesn't exist");

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
