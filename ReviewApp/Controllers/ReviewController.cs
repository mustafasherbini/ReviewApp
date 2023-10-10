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


        [HttpGet("{ReviewID}")]
        [TypeFilter(typeof(Review_ValidateReviewIdFilterAttribute))]
        public IActionResult GetReview(int ReviewID)
        {
            var review = _reviewRepository.GetReview(ReviewID);
            var reviewDTO = _mapper.Map<ReviewDTO>(review);
            return Ok(reviewDTO);
        }
        


        [HttpGet("exists/{ReviewID}")]
        [TypeFilter(typeof(Review_ValidateReviewIdFilterAttribute))]

        public IActionResult ReviewExist(int ReviewID)
        {
            var exists = _reviewRepository.ReviewExists(ReviewID);
            return Ok(exists);
        }



        [HttpGet("{ProductID}/product")]
        [TypeFilter(typeof(Product_ValidateProductIdFilterAttribute))]

        public IActionResult GetReviewsOfAProduct(int ProductID)
        {
            var ProductReviews = _reviewRepository.GetReviewsOfAProduct(ProductID);

            var reviewsDTO = _mapper.Map<List<ReviewDTO>>(ProductReviews);

            return Ok(reviewsDTO);
        }

        [HttpPost]
        [TypeFilter(typeof(Reviewer_ValidateReviewerIdFilterAttribute))]
        [TypeFilter(typeof(Product_ValidateProductIdFilterAttribute))]

        public IActionResult CreateReview([FromBody] ReviewDTO ReviewCreate, [FromQuery] int ReviewerID, [FromQuery] int ProductID)
        {

            var ReviewMap = _mapper.Map<Review>(ReviewCreate);
            ReviewMap.Reviewer = _reviewerRepository.GetReviewerById(ReviewerID);
            ReviewMap.Product = _productRepository.GetProduct(ProductID);

            if (!_reviewRepository.CreateReview(ReviewMap, ProductID, ReviewerID))
            {

                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);

            }
            return Ok("Successfully Created");

        }


        [HttpPut("{ReviewID}")]
        [TypeFilter(typeof(Review_ValidateReviewIdFilterAttribute))]
        [TypeFilter(typeof(Review_ValidateUpdateReviewFilterAttribute))]

        public IActionResult UpdateReview(int ReviewID, [FromBody] ReviewDTO upReview)
        {

            var ReviewMap = _mapper.Map<Review>(upReview);

            if (!_reviewRepository.UpdateReview(ReviewMap))
            {
                ModelState.AddModelError("", "Something went wrong while Updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }


        [HttpDelete("{ReviewID}")]
        [TypeFilter(typeof(Review_ValidateReviewIdFilterAttribute))]

        public IActionResult DeleteReview(int ReviewID)
        {

            var ReviewToDelete = _reviewRepository.GetReview(ReviewID);
            _reviewRepository.DeleteReview(ReviewToDelete);

            return Ok();
        }
    }
}




