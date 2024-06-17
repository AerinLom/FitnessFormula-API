using FitnessFormula_API.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessFormula_API.Models
{
    public class Session
    {
        [Key]
        [MaxLength(255)]
        public string SessionToken { get; set; }

        public int UserId { get; set; }

        [Required]
        public DateTime Expiry { get; set; }

        [ForeignKey("UserId")]
        public UserProfile User { get; set; }
    }
}
