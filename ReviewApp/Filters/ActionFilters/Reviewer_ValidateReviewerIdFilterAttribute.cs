﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ReviewApp.Models;
using ReviewApp.Repository;


namespace ReviewApp.Filters.IActionFilters
{
    public class Reviewer_ValidateReviewerIdFilterAttribute : ActionFilterAttribute
    {
        IReviewerRepository _ReviewerRepository;

        public Reviewer_ValidateReviewerIdFilterAttribute(IReviewerRepository ReviewerRepository)
        {
            _ReviewerRepository = ReviewerRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);


            var ID = context.ActionArguments["ID"] as int?;
           
                if (ID <= 0)
                {

                    context.ModelState.AddModelError("ID", "ID is invalid");
                    var problemDeatails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDeatails);

                }
                else if (!_ReviewerRepository.ReviewerExist(ID))
                {
                    context.ModelState.AddModelError("ID", "Reviewer doesn't exist");

                    var problemDeatails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };
                    context.Result = new NotFoundObjectResult(problemDeatails);
                }
            

        }
    }
}