using Codebridge_Test_REST_API.Controllers;
using Codebridge_Test_REST_API.Validation;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Codebridge_Test_REST_API.Models
{
    public class DogModel
    {
        [Required(ErrorMessage = "Enter the name of the dog!")]
        [MinLength(2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter the color of the dog!")]
        public string Color { get; set; }

        [Required]
        [StringToFloat]
        [Range(0, 70, ErrorMessage = "Specify the length of the tail in the range from 0 to 70 cm!")]
        public float Tail_Length { get; set; }

        [Required]
        [Range(1, 150, ErrorMessage = "Specify the weight of the dog in the range from 1 to 150 kg!")]
        public float Weight { get; set; }
    }

}
