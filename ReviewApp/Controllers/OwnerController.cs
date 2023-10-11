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



        [HttpGet("{OwnerID}")]
        [TypeFilter(typeof(Owner_ValidateOwnerIdFilterAttribute))]
        public IActionResult GetOwner(int OwnerID)
        {
       
            var owner = _ownerRepository.GetOwner(OwnerID);
            var ownerDTO = _mapper.Map<OwnerDTO>(owner);
            return Ok(ownerDTO);
        }



        [HttpGet("{OwnerID}/product")]
        [TypeFilter(typeof(Owner_ValidateOwnerIdFilterAttribute))]
        public IActionResult GetProductByOwner(int OwnerID)
        {
            

            var product = _ownerRepository.GetProductsByOwner(OwnerID);
            var productDTOs = _mapper.Map<List<ProductDTO>>(product);
            return Ok(productDTOs);
        }



        [HttpGet("Product/{ProductID}")]
        [TypeFilter(typeof(Product_ValidateProductIdFilterAttribute))]
        public IActionResult GetOwnersOfAProduct(int ProductID)
        {
            var owners = _ownerRepository.GetOwnerOfAProduct(ProductID);
            var ownerDTOs = _mapper.Map<List<OwnerDTO>>(owners);
            return Ok(ownerDTOs);
        }


        [HttpPost]
        [TypeFilter(typeof(Owner_ValidateCreateOwnerFilterAttribute))]
        [TypeFilter(typeof(Country_ValidateCountryIdFilterAttribute))]

        public IActionResult CreateOwner([FromBody] OwnerDTO OwnerCreate , [FromQuery] int CountryID)
        {

           var OwnerMap = _mapper.Map<Owner>(OwnerCreate);
      
            OwnerMap.Country = _countryRepository.GetCountry(CountryID) ;            
            return Ok(_ownerRepository.CreateOwner(OwnerMap));
        }


        [HttpPut("{OwnerID}")]
        [TypeFilter(typeof(Owner_ValidateOwnerIdFilterAttribute))]
        [TypeFilter(typeof(Owner_ValidateUpdateOwnerFilterAttribute))]
        public IActionResult UpdateOwner(int OwnerID, [FromBody] OwnerDTO upowner)
        {

            if (!ModelState.IsValid) return BadRequest();
            var ownerMap = _mapper.Map<Owner>(upowner);

            if (!_ownerRepository.UpdateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Something went wrong while Updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }


        [HttpDelete("{OwnerID}")]
        [TypeFilter(typeof(Owner_ValidateOwnerIdFilterAttribute))]
        public IActionResult DeleteOwner(int OwnerID)
        {

            var OwnerToDelete = _ownerRepository.GetOwner(OwnerID);
            _ownerRepository.DeleteOwner(OwnerToDelete);

            return Ok();
        }

    }
}
