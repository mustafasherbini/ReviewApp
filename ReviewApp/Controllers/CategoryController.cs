﻿using AutoMapper;
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


        [HttpGet("{CategoryID}")]
        [TypeFilter(typeof(Category_ValidateCategoryIdFilterAttribute))]
        public IActionResult GetCategory(int CategoryID)
        {
            var categoryDTO = _mapper.Map<CategoryDTO>(_categoryRepository.GetCategoryById(CategoryID));
            return Ok(categoryDTO);
        }



        [HttpGet("exists/{CategoryID}")]
        [TypeFilter(typeof(Category_ValidateCategoryIdFilterAttribute))]
        public IActionResult CheckCategoryExistence(int CategoryID)
        {
            var exists = _categoryRepository.DoesCategoryExist(CategoryID);
            return Ok(exists);
        }




        [HttpGet("{CategoryID}/Product")]
        [TypeFilter(typeof(Category_ValidateCategoryIdFilterAttribute))]
        public IActionResult GetProductByCategory(int CategoryID)
        {
            var products = _categoryRepository.GetProductByCategoryId(CategoryID);
            var productDTOs = _mapper.Map<List<ProductDTO>>(products);
            return Ok(productDTOs);
        }




        [HttpPost]
        [TypeFilter(typeof(Category_ValidateCreateCategoryFilterAttribute))]
        public IActionResult CreateCategory([FromBody] CategoryDTO categoryCreate)
        {
            var categotyMap = _mapper.Map<Category>(categoryCreate);
            return Ok(_categoryRepository.CreateCategory(categotyMap));
        }




        [HttpPut(("{CategoryID}"))]

        [TypeFilter(typeof(Category_ValidateCategoryIdFilterAttribute))]
        [TypeFilter(typeof(Category_ValidateUpdateCategoryFilterAttribute))]
        public IActionResult UpdateCategory( int CategoryID , [FromBody]CategoryDTO upcategory )
        {
     
            var categoryMap = _mapper.Map<Category>(upcategory);

            if (!_categoryRepository.UpdateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Something went wrong while Updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }




        [HttpDelete("{CategoryID}")]
        [TypeFilter(typeof(Category_ValidateCategoryIdFilterAttribute))]
        public IActionResult DeleteCategory(int CategoryID)
        {

            var categoryToDelete = _categoryRepository.GetCategoryById(CategoryID);
            _categoryRepository.DeleteCategory(categoryToDelete);  

         return   Ok();
        }
    }
}
