using System;
using System.ComponentModel.DataAnnotations;

namespace ParkyAPI.Models.Dtos
{
    public class TrailCreateDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public double Distance { get; set; }

        public DifficultType DifficultType { get; set; }

        [Required]
        public int NationalParkId { get; set; }

        [Required]
        public double Elevation { get; set; }
    }
}