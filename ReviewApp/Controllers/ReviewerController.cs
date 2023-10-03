using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReviewApp.DTO;
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

        [HttpGet("reviewers")]
        public IActionResult GetReviewers()
        {
            var reviewers = _mapper.Map<List<ReviewerDTO>>(_reviewerRepository.GetReviewers());
            return Ok(reviewers);
        }

        [HttpGet("reviewer/{id}")]
        public IActionResult GetReviewer(int id)
        {
            var reviewer = _reviewerRepository.GetReviewerById(id);
            var reviewerDTO = _mapper.Map<ReviewerDTO>(reviewer);
            return Ok(reviewerDTO);
        }

        [HttpGet("reviewer-exists/{id}")]
        public IActionResult ReviewerExist(int id)
        {
            var exists = _reviewerRepository.ReviewerExist(id);
            return Ok(exists);
        }

        [HttpGet("reviews-by-reviewer/{id}")]
        public IActionResult GetReviewsByReviewer(int id)
        {
            var reviews = _reviewerRepository.GetReviewsByReviewer(id);

            var reviewsDTO = _mapper.Map<List<ReviewDTO>>(reviews);

            return Ok(reviewsDTO);
        }

        [HttpPost]
        public IActionResult CreateOwner([FromBody] ReviewerDTO ReviewerCreate)
        {
            if (ReviewerCreate == null) { return BadRequest(); }

            var Reviewer = _reviewerRepository.GetReviewers()
                .Where(c => c.LastName.Trim().ToUpper() == ReviewerCreate.LastName.Trim().ToUpper()).
                FirstOrDefault();

            if (Reviewer != null)
            {
                ModelState.AddModelError("", "Reviewer alredy exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid) { return BadRequest(); }
            var ReviewerMap = _mapper.Map<Reviewer>(ReviewerCreate);



            if (!_reviewerRepository.CreateReviewer(ReviewerMap))
            {

                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);

            }
            return Ok("Successfully Created");
        }


        [HttpPut()]

        public IActionResult UpdateReviewer(int ReviewerId, [FromBody] ReviewerDTO upReviewer)
        {
            if (upReviewer == null) { return BadRequest(ModelState); }
            if (ReviewerId != upReviewer.Id) return BadRequest(ModelState);
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

        [HttpDelete]
        public IActionResult DeleteReview(int id)
        {

            if (!_reviewerRepository.ReviewerExist(id)) return NotFound();

            var ReviewToDelete = _reviewerRepository.GetReviewerById(id);
            _reviewerRepository.DeleteReviewer(ReviewToDelete);

            return Ok();
        }

    }
}
