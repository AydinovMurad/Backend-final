using Pustak.Models.BaseModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing.Drawing2D;

namespace Pustak.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int DiscountPrice { get; set; }
        public decimal ExTax { get; set; }
        public string ProductCode { get; set; } = null!;
        public int? RewardPoint { get; set; }
        public bool Availability { get; set; }
        public int Count { get; set; }
        public int Rating { get; set; }
        [NotMapped]
        public List<IFormFile>? Files {  get; set; }
        [NotMapped]
        public IFormFile IsMain { get; set; } = null!;
        [NotMapped]
        public IFormFile IsHover { get; set; } = null!;
        public ICollection<ProductImage> ProductImages { get; set; } = null!;
        public int CategoryId { get; set; }
        public bool isNewArrival { get; set; }
        public bool IsMostViewed { get; set; }


        public bool BestSeller { get; set; }
        public bool SpecialOffer { get; set; }
        public bool ChildrenBooks { get; set; }
        public Category? category { get; set; }
    }
}