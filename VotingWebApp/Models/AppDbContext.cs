using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VotingWebApp.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Voter> Voters { get; set; }
        public DbSet<Vote> Votes { get; set; }
    }
}
