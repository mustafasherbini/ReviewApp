using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ReviewApp.DTO;
using ReviewApp.Repository;

namespace ReviewApp.Filters.IActionFilters
{
    public class Reviewer_ValidateCreateReviewerFilterAttribute : ActionFilterAttribute
    {

        IReviewerRepository _ReviewerRepository;

        public Reviewer_ValidateCreateReviewerFilterAttribute(IReviewerRepository ReviewerRepository)
        {
            _ReviewerRepository = ReviewerRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);


            var Reviewerobject = context.ActionArguments["ReviewerCreate"] as ReviewerDTO;
            var Reviewer = _ReviewerRepository.GetReviewers()
               .Where(c => c.LastName.Trim().ToUpper() == Reviewerobject.LastName.Trim().ToUpper()).
               FirstOrDefault();

            if (Reviewer != null)
            {

                context.ModelState.AddModelError("Reviewer", "Reviewer alredy exists ");
                var problemDeatails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDeatails);

            }

        }

    }
}
