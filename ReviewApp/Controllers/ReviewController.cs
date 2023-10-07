using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReviewApp.DTO;
using ReviewApp.Filters.IActionFilters;
using ReviewApp.Models;

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
        public ReviewController(IReviewRepository reviewRepository, IMapper mapper, IProductRepository productRepository, IReviewerRepository reviewerRepository)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _productRepository = productRepository;
            _reviewerRepository = reviewerRepository;

        }
        [HttpGet("All")]
        public IActionResult GetReviews()
        {
            var reviews = _mapper.Map<List<ReviewDTO>>(_reviewRepository.GetReviews());
            return Ok(reviews);
        }


        [HttpGet("{id}")]
        [TypeFilter(typeof(Review_ValidateReviewIdFilterAttribute))]
        public IActionResult GetReview(int id)
        {
            var review = _reviewRepository.GetReview(id);
            var reviewDTO = _mapper.Map<ReviewDTO>(review);
            return Ok(reviewDTO);
        }
        


        [HttpGet("exists/{id}")]
        [TypeFilter(typeof(Review_ValidateReviewIdFilterAttribute))]

        public IActionResult ReviewExist(int id)
        {
            var exists = _reviewRepository.ReviewExists(id);
            return Ok(exists);
        }



        [HttpGet("{id}/product")]
        [TypeFilter(typeof(Product_ValidateProductIdFilterAttribute))]

        public IActionResult GetReviewsOfAProduct(int id)
        {
            var ProductReviews = _reviewRepository.GetReviewsOfAProduct(id);

            var reviewsDTO = _mapper.Map<List<ReviewDTO>>(ProductReviews);

            return Ok(reviewsDTO);
        }

        [HttpPost]
        public IActionResult CreateReview([FromBody] ReviewDTO ReviewCreate, [FromQuery] int reviwerID, [FromQuery] int ProductID)
        {
            if (ReviewCreate == null) { return BadRequest(); }


            var ReviewMap = _mapper.Map<Review>(ReviewCreate);
            ReviewMap.Reviewer = _reviewerRepository.GetReviewerById(reviwerID);
            ReviewMap.Product = _productRepository.GetProduct(ProductID);

            if (!_reviewRepository.CreateReview(ReviewMap, ProductID, reviwerID))
            {

                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);

            }
            return Ok("Successfully Created");
        }


        [HttpPut("{id}")]
        public IActionResult UpdateReview(int id, [FromBody] ReviewDTO upReview)
        {
            if (upReview == null) { return BadRequest(ModelState); }
            if (id != upReview.Id) return BadRequest(ModelState);
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


        [HttpDelete("{id}")]
        [TypeFilter(typeof(Review_ValidateReviewIdFilterAttribute))]

        public IActionResult DeleteReview(int id)
        {

            var ReviewToDelete = _reviewRepository.GetReview(id);
            _reviewRepository.DeleteReview(ReviewToDelete);

            return Ok();
        }
    }
}




