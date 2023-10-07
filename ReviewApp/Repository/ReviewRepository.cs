using Microsoft.EntityFrameworkCore;
using ReviewApp.Data;
using ReviewApp.Models;

namespace ReviewApp.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext _Context;
        public ReviewRepository(DataContext Context)
        {
            _Context = Context;

        }

        public bool CreateReview(Review review, int ProductID, int ReviwerID)
        {
          _Context.Add(review);
           return Save();
        }

        public bool DeleteReview(Review review)
        {
            _Context.Remove(review);
            return Save();
        }

        public Review GetReview(int id)
        {
            return _Context.Reviews.Where(R=>R.Id==id).SingleOrDefault();
        }

        public ICollection<Review> GetReviews()
        {
        return _Context.Reviews.ToList();
        }

        public ICollection<Review> GetReviewsOfAProduct(int id)
        {
           
            return _Context.Product.Where(Product => Product.Id==id)
                .SelectMany(R=>R.Reviews).ToList();
        }


        public bool ReviewExists(int? id)
        {
            return _Context.Reviews.Any(R=>R.Id==id);
        }

        public bool Save()
        {
            var saved = _Context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateReview(Review review)
        {
            _Context.Update(review);
            return Save();
        }
    }
}
