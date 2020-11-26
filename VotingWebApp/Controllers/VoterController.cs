using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VotingWebApp.Models;

namespace VotingWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoterController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        //Dependency Injection done using constructor.
        public VoterController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        // This method will create voter user and also check if voter's age is > 18 or not.
        // If voter's age is not > 18, then we will display message.
        [HttpPost]
        [Route("CreateVoter")]
        public async Task<ActionResult<Voter>> CreateVoter(Voter voter)
        {
            if (ModelState.IsValid)
            {
                //First we get today's date.
                var today = DateTime.Today;

                //Now calculate age.
                var age = today.Year - voter.DateOfBirth.Year;

                if(age > 18)
                {
                    try
                    {
                        _appDbContext.Voters.Add(voter);
                        var voterId = await _appDbContext.SaveChangesAsync();

                        if (voterId > 0)
                            return Ok("Voter Added Successfully.");

                        return NotFound();
                    }
                    catch (Exception)
                    {
                        return BadRequest();
                    }

                } else
                {
                    return NotFound("Voter's Age must be > 18.");
                }
            }

            return NotFound();
        }

        //This method will delete voter user based on id of that user.
        //means we need to pass user's id in argument and based on that id, this method will delete user.
        [HttpDelete]
        [Route("DeleteVoter/{id}")]
        public async Task<ActionResult<Voter>> DeleteVoter(int? id)
        {
            if(id == null)
            {
                return BadRequest();
            }

            try
            {
                var voter = await _appDbContext.Voters.FindAsync(id);
                if(voter == null)
                {
                    return NotFound("Voter user not found with this given Id, please give correct id.");
                }

                _appDbContext.Voters.Remove(voter);
                await _appDbContext.SaveChangesAsync();

                return Ok("Voter User Deleted Successfully.");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        //Using this method we can change age of Voter user. Here we need to pass DOB and based on that age will be updated.
        //It will also check, if age is > 18 or not, if not then give message.
        [HttpPut]
        [Route("UpdateAge/{id}")]
        public async Task<ActionResult<Voter>> UpdateAge(int id, Voter voter)
        {
            var voterdetail = await _appDbContext.Voters.FindAsync(id);

            if (voterdetail == null)
            {
                return NotFound("Voter user not found with this given Id, please give correct id.");
            }

            voterdetail.DateOfBirth = voter.DateOfBirth;

            try
            {
                //First we get today's date.
                var today = DateTime.Today;

                //Now calculate age.
                var age = today.Year - voter.DateOfBirth.Year;

                if(age > 18)
                {
                    await _appDbContext.SaveChangesAsync();
                    return Ok("Voter Updated Successfully.");
                }
                else
                {
                    return NotFound("Voter's Age must be > 18.");
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                //Here we are checking if voter user exists or not with given Id using this method.
                if (!VoterExists(id))
                {
                    return NotFound("Voter user not found with this given Id, please give correct id.");
                }
                else
                {
                    throw;
                }
            }
        }

        //This method will return either true if voter user found in DB otherwise it will return false.
        private bool VoterExists(int id)
        {
            return _appDbContext.Voters.Any(e => e.VoterId == id);
        }

    }
}
