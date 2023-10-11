using ReviewApp.Models;

namespace ReviewApp
{
    public interface ICountryRepository
{
        ICollection<Country> GetAll();

         Country GetCountry(int? CountryId);
        Country GetCountryByName(string Name);
        Country GetCountryByOwner( int OwnerId);
         ICollection<Owner>GetOwnersByCountry( int CountryId );
        bool CountryExist(int? CountryId);
        bool CreateCountry(Country country);
        bool UpdateCountry(Country country);
        bool DeleteCountry(Country country);
        bool Save();


    }

}
