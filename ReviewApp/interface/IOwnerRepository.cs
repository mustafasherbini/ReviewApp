using ReviewApp.Models;

namespace ReviewApp
{
    public interface IOwnerRepository
{
        ICollection<Owner> GetAll();
        Owner GetOwner(int? ownerId);
        ICollection<Owner> GetOwnerOfAProduct(int ProductID);
        ICollection<Product>GetProductsByOwner(int ownerId);
        bool CreateOwner(Owner owner);
        bool UpdateOwner(Owner owner);
        bool DeleteOwner(Owner owner);
        bool Save();
    }

}
