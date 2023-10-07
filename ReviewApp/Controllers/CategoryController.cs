using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReviewApp.Data;
using ReviewApp.DTO;
using ReviewApp.Filters.IActionFilters;
using ReviewApp.Models;
using ReviewApp.Repository;
using System.Collections.Generic;
using System.Resources;

namespace ReviewApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet("All")]
        public IActionResult GetCategories()
        {
            var categories = _categoryRepository.GetAllCategories();
            var categoryDTOs = _mapper.Map<List<CategoryDTO>>(categories);
            return Ok(categoryDTOs);
        }


        [HttpGet("{id}")]
        [TypeFilter(typeof(Category_CategoryIdFilterAttribute))]
        public IActionResult GetCategory(int id)
        {
            var categoryDTO = _mapper.Map<CategoryDTO>(_categoryRepository.GetCategoryById(id));
            return Ok(categoryDTO);
        }



        [HttpGet("exists/{id}")]
        public IActionResult CheckCategoryExistence(int id)
        {
            var exists = _categoryRepository.DoesCategoryExist(id);
            return Ok(exists);
        }




        [HttpGet("{id}/Product")]
        [TypeFilter(typeof(Category_CategoryIdFilterAttribute))]
        public IActionResult GetProductByCategory(int id)
        {
            var products = _categoryRepository.GetProductByCategoryId(id);
            var productDTOs = _mapper.Map<List<ProductDTO>>(products);
            return Ok(productDTOs);
        }




        [HttpPost]   
        public IActionResult CreateCategory([FromBody] CategoryDTO categoryCreate)
        {
            if (categoryCreate == null) { return  BadRequest(); }

            var Category=_categoryRepository.GetCategoryByName(categoryCreate.Name);
            if (Category != null)
            {
                ModelState.AddModelError("", "Category alredy exists");
                return StatusCode(422, ModelState);
            }
             if(!ModelState.IsValid) { return BadRequest(); }
            var categotyMap = _mapper.Map<Category>(categoryCreate);
            if(!_categoryRepository.CreateCategory(categotyMap)) {

                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500,ModelState);
            
            }
            return Ok("Successfully Created");
        }




        [HttpPut(("{id}"))]
        public IActionResult UpdateCategory( int id , [FromBody]CategoryDTO upcategory )
        {
            if (upcategory == null) { return BadRequest(ModelState); }
            if(id != upcategory.Id)return BadRequest(ModelState);
            if(!_categoryRepository.DoesCategoryExist(upcategory.Id))return NotFound();

            if(!ModelState.IsValid)return BadRequest();
            var categoryMap = _mapper.Map<Category>(upcategory);

            if (!_categoryRepository.UpdateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Something went wrong while Updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }




        [HttpDelete("{id}")]
        [TypeFilter(typeof(Category_CategoryIdFilterAttribute))]
        public IActionResult DeleteCategory(int id)
        {

            var categoryToDelete = _categoryRepository.GetCategoryById(id);
            _categoryRepository.DeleteCategory(categoryToDelete);  

         return   Ok();
        }
    }
}
