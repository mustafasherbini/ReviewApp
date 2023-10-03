using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReviewApp.DTO;
using ReviewApp.Models;
using ReviewApp.Repository;

namespace ReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {

        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        IProductRepository _productRepository;
        IReviewerRepository _reviewerRepository;
        public ReviewController(IReviewRepository reviewRepository, IMapper mapper , IProductRepository productRepository , IReviewerRepository reviewerRepository)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _productRepository = productRepository;
            _reviewerRepository = reviewerRepository;
           
        }
        [HttpGet("Reviews")]
        public IActionResult GetReviews()
        {
            var reviews = _mapper.Map<List<ReviewDTO>>(_reviewRepository.GetReviews());
            return Ok(reviews);
        }

        [HttpGet("GetReview")]
        public IActionResult GetReview(int ID)
        {
            var review = _reviewRepository.GetReview(ID);
            var reviewDTO = _mapper.Map<ReviewDTO>(review);
            return Ok(reviewDTO);
        }

        [HttpGet("ReviewExist")]
        public IActionResult ReviewExist(int ID)
        {
            var exists = _reviewRepository.ReviewExists(ID);
            return Ok(exists);
        }

        [HttpGet("GetReviewsOfAProduct")]
        public IActionResult GetReviewsOfAPokemon(int ID)
        {
            var ProductReviews = _reviewRepository.GetReviewsOfAProduct(ID);

            if (ProductReviews == null)
            {
                return NotFound(); // Return a 404 Not Found response if the Pokémon is not found.
            }

            var reviewsDTO = _mapper.Map<List<ReviewDTO>>(ProductReviews);

            if (!reviewsDTO.Any())
            {
                return NoContent(); // Return a 204 No Content response if the reviews collection is empty.
            }

            return Ok(reviewsDTO);
        }

        [HttpPost]
        public IActionResult CreateReview([FromBody] ReviewDTO ReviewCreate, [FromQuery] int reviwerID, [FromQuery] int ProductID)
        {
            if (ReviewCreate == null) { return BadRequest(); }

          
            var ReviewMap = _mapper.Map<Review>(ReviewCreate);
            ReviewMap.Reviewer = _reviewerRepository.GetReviewerById(reviwerID);
            ReviewMap.Product = _productRepository.GetProduct(ProductID);
           
            if (!_reviewRepository.CreateReview(ReviewMap,ProductID, reviwerID))
            {

                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);

            }
            return Ok("Successfully Created");
        }


        [HttpPut()]

        public IActionResult UpdateReview(int ReviewId, [FromBody] ReviewDTO upReview)
        {
            if (upReview == null) { return BadRequest(ModelState); }
            if (ReviewId != upReview.Id) return BadRequest(ModelState);
            if (!_reviewRepository.ReviewExists(upReview.Id)) return NotFound();

            if (!ModelState.IsValid) return BadRequest();
            var ReviewMap = _mapper.Map<Review>(upReview);

            if (!_reviewRepository.UpdateReview(ReviewMap))
            {
                ModelState.AddModelError("", "Something went wrong while Updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [HttpDelete]
        public IActionResult DeleteReview(int id)
        {

            if (!_reviewRepository.ReviewExists(id)) return NotFound();

            var ReviewToDelete = _reviewRepository.GetReview(id);
            _reviewRepository.DeleteReview(ReviewToDelete);

            return Ok();
        }
    }
}




