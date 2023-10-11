using Microsoft.EntityFrameworkCore;
using ReviewApp.Data;
using ReviewApp.Models;
using System.Diagnostics.Metrics;

namespace ReviewApp.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly DataContext _Context;
      
        public OwnerRepository(DataContext Context)
        {
            _Context = Context;
        }

        public bool CreateOwner(Owner owner)
        {
            _Context.Add(owner);
            return Save();
        }

        public bool DeleteOwner(Owner owner)
        {
            _Context.Remove(owner);
            return Save();
        }

        public ICollection<Owner> GetAll()
        {
            return _Context.Owners.ToList();
        }

        public Owner GetOwner(int? ownerId)
        {
           
            return _Context.Owners.FirstOrDefault(o => o.Id == ownerId);
        }

      

        public ICollection<Owner> GetOwnerOfAProduct(int ProductID)
        {
            return _Context.ProductOwners.Where(p => p.ProductId == ProductID).Select(o => o.Owner).ToList();


        }

        public ICollection<Product> GetProductsByOwner(int ownerId)
        {
            return _Context.ProductOwners.
                Where(O=> O.OwnerId == ownerId).
                Select(p=>p.Product).ToList();
          

        }

       

        public bool Save()
        {
            var saved = _Context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateOwner(Owner owner)
        {
            _Context.Update(owner);
            return Save();
        }
    }
}
