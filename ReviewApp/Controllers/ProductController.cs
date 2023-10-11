using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReviewApp.DTO;
using ReviewApp.Filters.IActionFilters;
using ReviewApp.Models;
using ReviewApp.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductController(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetProducts()
        {
            var products = _mapper.Map<List<ProductDTO>>(_productRepository.GetProducts());
            return Ok(products);
        }




        [HttpGet("{ProductID}")]
        [TypeFilter(typeof(Product_ValidateProductIdFilterAttribute))]
        public IActionResult GetProduct(int ProductID)
        {
            var product = _mapper.Map<ProductDTO>(HttpContext.Items["product"]);
            return Ok(product);
        }



        [HttpGet("ByName/{name}")]
        public IActionResult GetProductByName(string name)
        {
            var product = _mapper.Map<ProductDTO>(_productRepository.GetProduct(name));
            if (product == null) return NotFound();
            return Ok(product);
        }





        [HttpGet("{ProductID}/rating")]
        [TypeFilter(typeof(Product_ValidateProductIdFilterAttribute))]
        public IActionResult GetProductRating(int ProductID)
        {
   
            return Ok(_productRepository.GetProductRating(ProductID));
        }




        [HttpPost()]
        [TypeFilter(typeof(Product_ValidateCreateProductFilterAttribute))]
        public IActionResult CreateProduct([FromBody] ProductDTO ProductCreate ,[FromQuery] int ownerID ,[FromQuery] int CategoryID)
        {

            

            var ProductMap = _mapper.Map<Product>(ProductCreate);
           
        
            return Ok(_productRepository.CreateProduct(ownerID, CategoryID, ProductMap));
        }



        [HttpPut("{ProductID}")]
        [TypeFilter(typeof(Product_ValidateProductIdFilterAttribute))]
        [TypeFilter(typeof(Product_ValidateUpdateProductFilterAttribute))]
        public IActionResult UpdateProduct(int ProductID, [FromBody] ProductDTO upProduct)
        { 
            var ProductMap = _mapper.Map<Product>(upProduct);

            if (!_productRepository.UpdateProduct(ProductMap))
            {
                ModelState.AddModelError("", "Something went wrong while Updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }



        [HttpDelete("{ProductID}")]
        [TypeFilter(typeof(Product_ValidateProductIdFilterAttribute))]
        public IActionResult DeleteProduct(int ProductID)
        {
            var ProductToDelete = _productRepository.GetProduct(ProductID);
            _productRepository.DeleteProduct(ProductToDelete);
            return Ok();
        }
    }
}
