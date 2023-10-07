using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReviewApp.Data;
using ReviewApp.DTO;
using ReviewApp.Models;
using System.Collections.Generic;
using System.Resources;

namespace ReviewApp.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet("categories")]
        public IActionResult GetCategories()
        {
            var categories = _categoryRepository.GetAllCategories();
            var categoryDTOs = _mapper.Map<List<CategoryDTO>>(categories);
            return Ok(categoryDTOs);
        }

        [HttpGet("categories/{id}")]
        public IActionResult GetCategory(int id)
        {
            var category = _categoryRepository.GetCategoryById(id);
            if (category == null)
            {
                return NotFound(); // Return a 404 Not Found response if the category is not found.
            }
            var categoryDTO = _mapper.Map<CategoryDTO>(category);
            return Ok(categoryDTO);
        }

        [HttpGet("categories/exists/{id}")]
        public IActionResult CheckCategoryExistence(int id)
        {
            var exists = _categoryRepository.DoesCategoryExist(id);
            return Ok(exists);
        }

        [HttpGet("categories/{id}/Product")]
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

        [HttpPut()]

        public IActionResult UpdateCategory( int cateogryId , [FromBody]CategoryDTO upcategory )
        {
            if (upcategory == null) { return BadRequest(ModelState); }
            if(cateogryId != upcategory.Id)return BadRequest(ModelState);
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

        [HttpDelete]
        public IActionResult DeleteCategory(int id)
        {

        if(!_categoryRepository.DoesCategoryExist(id)) return NotFound();

        var categoryToDelete = _categoryRepository.GetCategoryById(id);
            _categoryRepository.DeleteCategory(categoryToDelete);  

         return   Ok();
        }
    }
}
