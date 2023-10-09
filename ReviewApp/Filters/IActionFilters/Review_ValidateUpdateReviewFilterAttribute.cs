using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ReviewApp.DTO;


namespace ReviewApp.Filters.IActionFilters
{
    public class Review_ValidateUpdateReviewFilterAttribute : ActionFilterAttribute
    {
      

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);


            var reviewID = context.ActionArguments["id"] as int?;
            var reviewobject = context.ActionArguments["upReview"] as ReviewDTO;

            if (reviewobject == null)
            {
                context.ModelState.AddModelError("review", "review is invalid");
                var problemDeatails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDeatails);

            }
            else if (reviewID != reviewobject.Id)
            {
                context.ModelState.AddModelError("reviewID", "reviewID is not the same as the object id");
                var problemDeatails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDeatails);

            }


        }
    }
}
