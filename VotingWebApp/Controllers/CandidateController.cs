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
    public class CandidateController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        //Dependency Injection done using constructor.
        public CandidateController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        // This method will create candidate as well as also assign category to that candidate.
        [HttpPost]
        [Route("CreateCandidate")]
        public async Task<ActionResult<Candidate>> CreateCandidate(Candidate candidate)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _appDbContext.Candidates.Add(candidate);
                    var candidateId = await _appDbContext.SaveChangesAsync();

                    if (candidateId > 0)
                        return Ok("Candidate Added Successfully.");

                    return NotFound();
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }
            
            return NotFound();
        }
    }
}
