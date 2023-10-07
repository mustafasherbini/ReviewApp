using ReviewApp.Models;

namespace ReviewApp
{
    public interface IReviewerRepository
{

        ICollection<Reviewer>GetReviewers();
        Reviewer GetReviewerById(int id);
        ICollection<Review>GetReviewsByReviewer(int ReviewerId);
        bool ReviewerExist(int? ReviewerId);

        bool CreateReviewer( Reviewer reviewer);
        bool UpdateReviewer( Reviewer reviewer);
        bool DeleteReviewer(Reviewer reviewer);
        bool Save();


    }
}
