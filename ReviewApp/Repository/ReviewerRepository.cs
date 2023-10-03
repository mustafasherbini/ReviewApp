using ReviewApp.Data;
using ReviewApp.Models;

namespace ReviewApp.Repository
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly DataContext _context;
        public ReviewerRepository(DataContext context)
        {
            _context   = context;
        }

        public bool CreateReviewer(Reviewer reviewer)
        {

            _context.Add(reviewer);
            return Save();
        }

        public bool DeleteReviewer(Reviewer reviewer)
        {
            _context.Remove(reviewer);
            return Save();
        }

        public Reviewer GetReviewerById(int id)
        {
        return _context.Reviewers.Where(R=>R.Id==id).FirstOrDefault();
        }

        public ICollection<Reviewer> GetReviewers()
        {

        return _context.Reviewers.ToList();
        }


        public ICollection<Review> GetReviewsByReviewer(int ReviewerId)
        {
            return _context.Reviewers
                .Where(R => R.Id == ReviewerId)
                .SelectMany(R => R.Reviews)
                .ToList();
        }


        public bool ReviewerExist(int ReviewerId)
        {
         return _context.Reviewers.Any(R=>R.Id == ReviewerId);   
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateReviewer(Reviewer reviewer)
        {
            _context.Update(reviewer);
            return Save();
        }
    }
}
