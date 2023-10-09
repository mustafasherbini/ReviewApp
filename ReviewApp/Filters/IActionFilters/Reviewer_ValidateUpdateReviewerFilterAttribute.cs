using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ReviewApp.DTO;


namespace ReviewApp.Filters.IActionFilters
{
    public class Reviewer_ValidateUpdateReviewerFilterAttribute : ActionFilterAttribute
    {
      

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);


            var reviewerID = context.ActionArguments["id"] as int?;
            var reviewerobject = context.ActionArguments["upReviewer"] as ReviewerDTO;

            if (reviewerobject == null)
            {
                context.ModelState.AddModelError("reviewer", "reviewer is invalid");
                var problemDeatails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDeatails);

            }
            else if (reviewerID != reviewerobject.Id)
            {
                context.ModelState.AddModelError("reviewerID", "reviewerID is not the same as the object id");
                var problemDeatails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDeatails);

            }


        }
    }
}
