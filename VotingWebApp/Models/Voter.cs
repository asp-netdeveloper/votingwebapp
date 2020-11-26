using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VotingWebApp.Models
{
    //This class is for Voter Table.
    public class Voter
    {
        [Key]
        public int? VoterId { get; set; }
        public string VoterName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
