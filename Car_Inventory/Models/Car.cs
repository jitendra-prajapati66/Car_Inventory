using System.ComponentModel.DataAnnotations;

namespace Car_Inventory.Models
{
    public class Car
    {
        public int Id { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        
        public double Price { get; set; }
  
        public string New { get; set; }
        [Required]
        public bool isNew { get; set; }
        [Required]
        public string UserId { get; set; }
    }
}
