using System.ComponentModel.DataAnnotations;

namespace Codebridge_Test_REST_API.Domain.Enteties
{
    public class Dog
    {
        [Key]
        public string Name { get; set; }
        public string Color { get; set; }
        public float Tail_Length { get; set; }
        public float Weight { get; set; }
    }
}
