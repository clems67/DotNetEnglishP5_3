using System.ComponentModel.DataAnnotations;

namespace DotNetEnglishP5_3.ViewModels
{
    public class InventoryViewModel
    {
        [Required]
        [RegularExpression(@"[\d\w]{12}", ErrorMessage = "12 char code")]
        public string VIN { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public string Make { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public string Trim { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime PurchaseDate { get; set; }
        [Required]
        public float PurchasePrice { get; set; }
        public string? Repairs { get; set; }
        public float? RepairCost { get; set; }
        [DataType(DataType.Date)]
        public DateTime? LotDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? SaleDate { get; set; }
        public IFormFile? Picture { get; set; }
    }
}