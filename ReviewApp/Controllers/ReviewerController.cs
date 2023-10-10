using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReviewApp.DTO;
using ReviewApp.Filters.IActionFilters;
using ReviewApp.Models;
using ReviewApp.Repository;
using System.Collections.Generic;

namespace ReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewerController : ControllerBase
    {
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;

        public ReviewerController(IReviewerRepository reviewerRepository, IMapper mapper)
        {
            _reviewerRepository = reviewerRepository;
            _mapper = mapper;
        }

        [HttpGet("All")]
        public IActionResult GetReviewers()
        {
            var reviewers = _mapper.Map<List<ReviewerDTO>>(_reviewerRepository.GetReviewers());
            return Ok(reviewers);
        }

        [HttpGet("{ReviewerID}")]
        [TypeFilter(typeof(Reviewer_ValidateReviewerIdFilterAttribute))]
        public IActionResult GetReviewer(int ReviewerID)
        {
            var reviewer = _reviewerRepository.GetReviewerById(ReviewerID);
            var reviewerDTO = _mapper.Map<ReviewerDTO>(reviewer);
            return Ok(reviewerDTO);
        }

        [HttpGet("exists/{ReviewerID}")]
        [TypeFilter(typeof(Reviewer_ValidateReviewerIdFilterAttribute))]

        public IActionResult ReviewerExist(int ReviewerID)
        {
            var exists = _reviewerRepository.ReviewerExist(ReviewerID);
            return Ok(exists);
        }

        [HttpGet("{ReviewerID}/reviews")]
        [TypeFilter(typeof(Reviewer_ValidateReviewerIdFilterAttribute))] 

        public IActionResult GetReviewsByReviewer(int ReviewerID)
        {
            var reviews = _reviewerRepository.GetReviewsByReviewer(ReviewerID);

            var reviewsDTO = _mapper.Map<List<ReviewDTO>>(reviews);

            return Ok(reviewsDTO);
        }

        [HttpPost]
        [TypeFilter(typeof(Reviewer_ValidateCreateReviewerFilterAttribute))]

        public IActionResult CreateReviewer([FromBody] ReviewerDTO ReviewerCreate)
        {

            var ReviewerMap = _mapper.Map<Reviewer>(ReviewerCreate);

            return Ok(_reviewerRepository.CreateReviewer(ReviewerMap));
        }


        [HttpPut("ReviewerID")]
        [TypeFilter(typeof(Reviewer_ValidateReviewerIdFilterAttribute))]

        [TypeFilter(typeof(Reviewer_ValidateUpdateReviewerFilterAttribute))]

        public IActionResult UpdateReviewer(int ReviewerID, [FromBody] ReviewerDTO upReviewer)
        {
            if (upReviewer == null) { return BadRequest(ModelState); }
            if (ReviewerID != upReviewer.Id) return BadRequest(ModelState);
            if (!_reviewerRepository.ReviewerExist(upReviewer.Id)) return NotFound();

            if (!ModelState.IsValid) return BadRequest();
            var ReviewerMap = _mapper.Map<Reviewer>(upReviewer);

            if (!_reviewerRepository.UpdateReviewer(ReviewerMap))
            {
                ModelState.AddModelError("", "Something went wrong while Updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [HttpDelete("ReviewerID")]
        [TypeFilter(typeof(Reviewer_ValidateReviewerIdFilterAttribute))]

        public IActionResult DeleteReviewer(int ReviewerID)
        {


            var ReviewToDelete = _reviewerRepository.GetReviewerById(ReviewerID);
            _reviewerRepository.DeleteReviewer(ReviewToDelete);

            return Ok();
        }

    }
}
