using System;
using System.ComponentModel.DataAnnotations;

namespace ParkyAPI.Models.Dtos
{
    public class TrailDto
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        [Required]
        public string Distance { get; set; }
        
        public DifficultType DifficultType { get; set; }

        [Required]
        public int NationalParkId { get; set; }

        public NationalParkDto NationalPark { get; set; }

        public DateTime DateCreated { get; set; }

        [Required]
        public double Elevation { get; set; }
    }
}