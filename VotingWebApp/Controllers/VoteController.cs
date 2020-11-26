using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VotingWebApp.Models;
using System.Diagnostics;

namespace VotingWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        //Dependency Injection done using constructor.
        public VoteController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }


        // Using this method voter user can send vote to partiuclar candidate.
        [HttpPost]
        [Route("SendVote")]
        public async Task<ActionResult<Vote>> SendVote(Vote vote)
        {
            if (ModelState.IsValid)
            {
                /*var voteid = _appDbContext.Votes
                    .Where(v => v.VoterId == vote.VoterId)
                    .Where(v => v.CategoryId == vote.CategoryId)
                    .FirstOrDefault();*/

                /*var voteid = _appDbContext.Votes
                    .Where(v => v.VoterId ==  && v.CategoryId == )
                    .FirstOrDefault();*/

                var voteidnew = _appDbContext.Votes
                    .Where(v => v.VoterId == vote.VoterId && v.CategoryId == vote.CategoryId)
                    .FirstOrDefault();

                /* var voterId = vote.VoterId;
                 var categoryId = vote.CategoryId;

                 var newvoteId = _appDbContext.Votes
                     .FromSqlRaw("SELECT VoteId From Votes WHERE VoterId = '{0}' AND CategoryId = '{1}'", voterId, categoryId)
                     .FirstOrDefault();

                 Debug.WriteLine(newvoteId);*/

                if (voteidnew != null)
                {
                    return NotFound("You have already given vote for this category, please give vote to another category");

                }
                else
                {
                    try
                    {
                        _appDbContext.Votes.Add(vote);
                        var voteId = await _appDbContext.SaveChangesAsync();

                        if (voteId > 0)
                            return Ok("You have successfully given vote. Thanks for that.");

                        return NotFound();
                    }
                    catch (Exception)
                    {
                        return BadRequest();
                    }
                }
            }

            return NotFound();
        }

        // Using this method voter user can send vote to partiuclar candidate.
        [HttpPost]
        [Route("SendVote")]
        public async Task<ActionResult<Vote>> SendVote(Vote vote)
        {
            if (ModelState.IsValid)
            {
                //This is logic to check if voter has already given vote for particular category.

                var voteidnew = _appDbContext.Votes
                    .Where(v => v.VoterId == vote.VoterId && v.CategoryId == vote.CategoryId)
                    .FirstOrDefault();

                if (voteidnew != null)
                {
                    return NotFound("You have already given vote for this category, please give vote to another category");

                } else
                {
                    try
                    {
                        _appDbContext.Votes.Add(vote);
                        var voteId = await _appDbContext.SaveChangesAsync();

                        if (voteId > 0)
                            return Ok("You have successfully given vote. Thanks for that.");

                        return NotFound();
                    }
                    catch (Exception)
                    {
                        return BadRequest();
                    }
                }
            }

            return NotFound();
        }

        //This method is to get number of votes for a candidate.
        //You need to pass candidate Id, and based on that it will return total number of votes.
        [HttpGet]
        [Route("GetVoteCount/{id}")]
        public async Task<ActionResult<Vote>> GetVoteCount(int? id)
        {
            var votecount = await _appDbContext.Votes
                .Where(c => c.CandidateId == id)
                .CountAsync();

            try
            {
                if (votecount == 0)
                    return NotFound("For this candidate there is not any vote.");
                else
                    return Ok("For this candidate total vote is " + votecount);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
