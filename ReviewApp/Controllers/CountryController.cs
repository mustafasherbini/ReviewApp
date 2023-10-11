using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReviewApp.Data;
using ReviewApp.DTO;
using ReviewApp.Filters.IActionFilters;
using ReviewApp.Models;
using ReviewApp.Repository;
using System.Collections.Generic;

namespace ReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public CountryController(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }


        [HttpGet("All")]
        public IActionResult GetCountries()
        {
            var countries = _countryRepository.GetAll();
            var countryDTOs = _mapper.Map<List<CountryDTO>>(countries);
            return Ok(countryDTOs);
        }


        [HttpGet("{CountryID}")]
        [TypeFilter(typeof(Country_ValidateCountryIdFilterAttribute))]

        public IActionResult GetCountry(int CountryID)
        {
            var countryDTO = _mapper.Map<CountryDTO>((Country)HttpContext.Items["country"]!);
            return Ok(countryDTO);
        }

     


        [HttpGet("owner/{OwnerID}")]
        [TypeFilter(typeof(Owner_ValidateOwnerIdFilterAttribute))]

        public IActionResult GetCountryByOwner(int OwnerID)
        {
            var country = _countryRepository.GetCountryByOwner(OwnerID);
            var countryDTO = _mapper.Map<CountryDTO>(country);
            return Ok(countryDTO);
        }


        [HttpGet("{CountryID}/owners")]
        [TypeFilter(typeof(Country_ValidateCountryIdFilterAttribute))]
        public IActionResult GetOwnersByCountry(int CountryID)
        {
            var owners = _countryRepository.GetOwnersByCountry(CountryID);
            var ownerDTOs = _mapper.Map<List<OwnerDTO>>(owners);
            return Ok(ownerDTOs);
        }


        [HttpPost]
        [TypeFilter(typeof(Country_ValidateCreateCountryFilterAttribute))]

        public IActionResult CreateCountry([FromBody] CountryDTO CountryCreate)
        {
            var categotyMap = _mapper.Map<Country>(CountryCreate);
          
            return Ok(_countryRepository.CreateCountry(categotyMap));
        }

        [HttpPut("{CountryID}")]

        [TypeFilter(typeof(Country_ValidateCountryIdFilterAttribute))]
        [TypeFilter(typeof(Country_ValidateUpdateCountryFilterAttribute))]

       
        public IActionResult UpdateCountry(int CountryID, [FromBody] CountryDTO upcountry)
        {
     

            var countryMap = _mapper.Map<Country>(upcountry);

            if (!_countryRepository.UpdateCountry(countryMap))
            {
                ModelState.AddModelError("", "Something went wrong while Updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [HttpDelete("{CountryID}")]
        [TypeFilter(typeof(Country_ValidateCountryIdFilterAttribute))]
        public IActionResult DeleteCountry(int CountryID)
        {


            var CountryToDelete = _countryRepository.GetCountry(CountryID);
            _countryRepository.DeleteCountry(CountryToDelete);

            return Ok();
        }

    }
}
