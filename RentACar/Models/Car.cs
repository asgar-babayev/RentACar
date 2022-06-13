using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Models
{
    public class Car
    {
        public int Id { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public int Doors { get; set; }
        [Required]
        public int Seats { get; set; }
        [Required]
        public string Luggage { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        [Required]
        public bool Transmission { get; set; }
        [Required]
        public bool AirConditioning { get; set; }
        [Required]
        public double Price { get; set; }
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
    }
}
