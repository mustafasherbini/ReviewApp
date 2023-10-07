using ReviewApp.Data;
using ReviewApp.Models;

namespace ReviewApp.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly DataContext _context;
        public CountryRepository(DataContext context)
        {
            _context = context;
            
        }
        public bool CountryExist(int? CountryId)
        {
            return _context.Countries.Any(country => country.Id == CountryId);
        }

        public bool DeleteCountry(Country country)
        {
            _context.Remove(country);
            return Save();
        }

        public ICollection<Country> GetAll()
        {
            return _context.Countries.ToList();
        }

        public Country GetCountry(int CountryId)
        {
         return _context.Countries.SingleOrDefault(country => country.Id == CountryId);
        }

        public Country GetCountryByName(string Name)
        {
            return _context.Countries.SingleOrDefault(country => country.Name == Name);
        }

        public Country GetCountryByOwner(int OwnerId)
        {
            return _context.Owners.Where(x => x.Id == OwnerId).Select(c=>c.Country).FirstOrDefault();

        }

        public ICollection<Owner> GetOwnersByCountry(int CountryId)
        {
            return _context.Countries
                .Where(country => country.Id == CountryId)
                .SelectMany(country => country.Owners)
                .ToList();

        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;

        }

        public bool UpdateCountry(Country country)
        {

            _context.Update(country);
            return Save();

        }

        bool ICountryRepository.CreateCountry(Country country)
        {

            _context.Add(country);
            return Save();

        }
    }
}
