using ReviewApp.Models;

namespace ReviewApp { 
    public interface IReviewRepository
{
        ICollection<Review>GetReviews();
        Review GetReview(int? id);
        ICollection<Review> GetReviewsOfAProduct(int id);
        bool CreateReview(Review review , int ProductID , int ReviwerID);
        bool UpdateReview(Review review);
        bool DeleteReview(Review review);
        bool Save();

    }

}
