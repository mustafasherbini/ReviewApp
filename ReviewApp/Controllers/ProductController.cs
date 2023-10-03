using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReviewApp.DTO;
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

        [HttpGet("Products")]
        public async Task<IActionResult> GetProducts()
        {
            var products = _mapper.Map<List<ProductDTO>>(_productRepository.GetProduct());
            return Ok(products);
        }

        [HttpGet("Product/{id}")]
        public IActionResult GetProduct(int id)
        {
            var product = _mapper.Map<ProductDTO>(_productRepository.GetProduct(id));
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpGet("product")]
        public IActionResult GetProductByName([FromQuery] string name)
        {
            var product = _mapper.Map<ProductDTO>(_productRepository.GetProduct(name));
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpGet("product/{id}/rating")]
        public IActionResult GetProductRating(int id)
        {
            var productExists = _productRepository.ProductExist(id);
            if (!productExists) return NotFound();
            return Ok(_productRepository.GetProductRating(id));
        }

        [HttpGet("product/{id}/exists")]
        public IActionResult ProductExist(int id)
        {
            return Ok(_productRepository.ProductExist(id));
        }

        [HttpPost]
        public IActionResult CreateProduct([FromBody] ProductDTO ProductCreate ,[FromQuery] int ownerID ,[FromQuery] int CategoryID)
        {
            if (ProductCreate == null) { return BadRequest(); }

            var Products = _productRepository.GetProduct(ProductCreate.Name);
            if (Products != null)
            {
                ModelState.AddModelError("", "Product alredy exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid) { return BadRequest(); }
            var ProductMap = _mapper.Map<Product>(ProductCreate);
           
            if (!_productRepository.CreateProduct(ownerID, CategoryID, ProductMap))
            {

                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);

            }
            return Ok("Successfully Created");
        }


        [HttpPut()]

        public IActionResult UpdateProduct(int ProductId, [FromBody] ProductDTO upProduct)
        {
            if (upProduct == null) { return BadRequest(ModelState); }
            if (ProductId != upProduct.Id) return BadRequest(ModelState);
            if (!_productRepository.ProductExist(upProduct.Id)) return NotFound();

            if (!ModelState.IsValid) return BadRequest();
            var ProductMap = _mapper.Map<Product>(upProduct);

            if (!_productRepository.UpdateProduct(ProductMap))
            {
                ModelState.AddModelError("", "Something went wrong while Updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [HttpDelete]
        public IActionResult DeleteProduct(int id)
        {

            if (!_productRepository.ProductExist(id)) return NotFound();

            var ProductToDelete = _productRepository.GetProduct(id);
            _productRepository.DeleteProduct(ProductToDelete);

            return Ok();
        }
    }
}
