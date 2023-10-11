using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using ReviewApp.Data;
using ReviewApp.Models;
using ReviewApp.Repository;


namespace ReviewApp.Filters.IActionFilters
{
    public class Reviewer_ValidateReviewerIdFilterAttribute : ActionFilterAttribute
    {
        IReviewerRepository _ReviewerRepository;
        DataContext _dataContext;
        public Reviewer_ValidateReviewerIdFilterAttribute(IReviewerRepository ReviewerRepository, DataContext dataContext)
        {
            _ReviewerRepository = ReviewerRepository;
            _dataContext = dataContext;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);


            var ID = context.ActionArguments["ReviewerID"] as int?;
           
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


                var reviewer = _ReviewerRepository.GetReviewerById(ID);
                if (reviewer == null)
                {
                    context.ModelState.AddModelError("ID", "Reviewer doesn't exist");

                    var problemDeatails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };
                    context.Result = new NotFoundObjectResult(problemDeatails);

                }
                else { context.HttpContext.Items["reviwer"] = reviewer;
                    _dataContext.Entry(reviewer).State = EntityState.Detached;
                }
            }
            

        }
    }
}