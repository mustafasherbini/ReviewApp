using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReviewApp.DTO;
using ReviewApp.Filters.IActionFilters;
using ReviewApp.Models;
using ReviewApp.Repository;
using System.Collections.Generic;
using System.Linq;

namespace ReviewApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class OwnerController : ControllerBase
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IMapper _mapper;
        private readonly ICountryRepository _countryRepository;

        public OwnerController(IOwnerRepository ownerRepository, IMapper mapper, ICountryRepository countryRepository)
        {
            _ownerRepository = ownerRepository;
            _mapper = mapper;
            _countryRepository = countryRepository; 
        }

        [HttpGet("All")]
        public IActionResult GetOwners()
        {
            var owners = _ownerRepository.GetAll();
            var ownersDTOs = _mapper.Map<List<OwnerDTO>>(owners);
            return Ok(ownersDTOs);
        }


        [HttpGet("{id}")]
        [TypeFilter(typeof(Owner_ValidateOwnerIdFilterAttribute))]

        public IActionResult GetOwner(int id)
        {
       
            var owner = _ownerRepository.GetOwner(id);
            var ownerDTO = _mapper.Map<OwnerDTO>(owner);
            return Ok(ownerDTO);
        }

        [HttpGet("exists/{id}")]
        [TypeFilter(typeof(Owner_ValidateOwnerIdFilterAttribute))]

        public IActionResult CheckOwnerExistence(int id)
        {
            var exists = _ownerRepository.OwnerExist(id);
            return Ok(exists);
        }

        [HttpGet("{id}/product")]
        [TypeFilter(typeof(Owner_ValidateOwnerIdFilterAttribute))]

        public IActionResult GetProductByOwner(int id)
        {
            

            var product = _ownerRepository.GetProductsByOwner(id);
            var productDTOs = _mapper.Map<List<ProductDTO>>(product);
            return Ok(productDTOs);
        }

        [HttpGet("Product/{id}")]
        [TypeFilter(typeof(Product_ValidateProductIdFilterAttribute))]
        public IActionResult GetOwnersOfAProduct(int id)
        {
            var owners = _ownerRepository.GetOwnerOfAProduct(id);
            var ownerDTOs = _mapper.Map<List<OwnerDTO>>(owners);
            return Ok(ownerDTOs);
        }

        [HttpPost]
        public IActionResult CreateOwner([FromBody] OwnerDTO OwnerCreate , [FromQuery] int CountryID)
        {
            if (OwnerCreate == null) { return BadRequest(); }

            var Owners = _ownerRepository.GetAll()
                .Where(c => c.LastName.Trim().ToUpper() == OwnerCreate.LastName.Trim().ToUpper()).
                FirstOrDefault();

            if (Owners != null)
            {
                ModelState.AddModelError("", "Owner alredy exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid) { return BadRequest(); }
           var OwnerMap = _mapper.Map<Owner>(OwnerCreate);
      

            OwnerMap.Country = _countryRepository.GetCountry(CountryID) ;

            if (!_ownerRepository.CreateOwner(OwnerMap))
            {

                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);

            }
            return Ok("Successfully Created");
        }


        [HttpPut("{id}")]

        public IActionResult UpdateOwner(int OwneryId, [FromBody] OwnerDTO upowner)
        {
            if (upowner == null) { return BadRequest(ModelState); }
            if (OwneryId != upowner.Id) return BadRequest(ModelState);
            if (!_countryRepository.CountryExist(upowner.Id)) return NotFound();

            if (!ModelState.IsValid) return BadRequest();
            var ownerMap = _mapper.Map<Owner>(upowner);

            if (!_ownerRepository.UpdateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Something went wrong while Updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [HttpDelete("{id}")]
        [TypeFilter(typeof(Owner_ValidateOwnerIdFilterAttribute))]
        public IActionResult DeleteOwner(int id)
        {

            var OwnerToDelete = _ownerRepository.GetOwner(id);
            _ownerRepository.DeleteOwner(OwnerToDelete);

            return Ok();
        }

    }
}
