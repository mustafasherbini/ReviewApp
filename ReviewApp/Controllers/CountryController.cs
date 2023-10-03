using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReviewApp.Data;
using ReviewApp.DTO;
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

        [HttpGet("countries")]
        public IActionResult GetCountries()
        {
            var countries = _countryRepository.GetAll();
            var countryDTOs = _mapper.Map<List<CountryDTO>>(countries);
            return Ok(countryDTOs);
        }

        [HttpGet("countries/{id}")]
        public IActionResult GetCountry(int id)
        {
            var country = _countryRepository.GetCountry(id);
            var countryDTO = _mapper.Map<CountryDTO>(country);
            return Ok(countryDTO);
        }

        [HttpGet("countries/exists/{id}")]
        public IActionResult CheckCountryExistence(int id)
        {
            var exists = _countryRepository.CountryExist(id);
            return Ok(exists);
        }

        [HttpGet("countries/by-owner/{id}")]
        public IActionResult GetCountryByOwner(int id)
        {
            var country = _countryRepository.GetCountryByOwner(id);
            var countryDTO = _mapper.Map<CountryDTO>(country);
            return Ok(countryDTO);
        }

        [HttpGet("countries/{id}/owners")]
        public IActionResult GetOwnersByCountry(int id)
        {
            var owners = _countryRepository.GetOwnersByCountry(id);
            var ownerDTOs = _mapper.Map<List<OwnerDTO>>(owners);
            return Ok(ownerDTOs);
        }

        [HttpPost]
        public IActionResult CreateCountry([FromBody] CountryDTO CountryCreate)
        {
            if (CountryCreate == null) { return BadRequest(); }

            var Category = _countryRepository.GetCountryByName(CountryCreate.Name);
            if (Category != null)
            {
                ModelState.AddModelError("", "Country alredy exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid) { return BadRequest(); }
            var categotyMap = _mapper.Map<Country>(CountryCreate);
            if (!_countryRepository.CreateCountry(categotyMap))
            {

                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);

            }
            return Ok("Successfully Created");
        }

        [HttpPut()]

        public IActionResult UpdateCountry(int CountryId, [FromBody] CountryDTO upcountry)
        {
            if (upcountry == null) { return BadRequest(ModelState); }
            if (CountryId != upcountry.Id) return BadRequest(ModelState);
            if (!_countryRepository.CountryExist(upcountry.Id)) return NotFound();

            if (!ModelState.IsValid) return BadRequest();
            var countryMap = _mapper.Map<Country>(upcountry);

            if (!_countryRepository.UpdateCountry(countryMap))
            {
                ModelState.AddModelError("", "Something went wrong while Updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [HttpDelete]
        public IActionResult DeleteCountry(int id)
        {

            if (!_countryRepository.CountryExist(id)) return NotFound();

            var CountryToDelete = _countryRepository.GetCountry(id);
            _countryRepository.DeleteCountry(CountryToDelete);

            return Ok();
        }

    }
}
