using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VotingWebApp.Models;

namespace VotingWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        
        //Dependency Injection done using constructor.
        public CategoryController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }


        // This method will create category if modelstate is valid.
        [HttpPost]
        [Route("CreateCategory")]
        public async Task<ActionResult<Category>> CreateCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _appDbContext.Categories.Add(category);
                    var categoryId = await _appDbContext.SaveChangesAsync();

                    if (categoryId > 0)
                        return Ok("Category Added Successfully");

                    return NotFound();
                }
                catch(Exception)
                {
                    return BadRequest();
                }
            }

            return BadRequest();
        }
    }
}


/*
Note: As per requirement, I have implemented only adding category. 
As updation and deleteion can not done with category so that methods are not added.
*/
