using Pustak.Models.BaseModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pustak.Models
{
    public class Slider : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Price { get; set; }
        [NotMapped]
        public ICollection<IFormFile>? Files { get; set; }
        [NotMapped]
        public IFormFile MainFile { get; set; } = null!;
        public ICollection<ProductImage>? ProductImages { get; set; }
        public string ImagePath { get; set; } = null!;
    }
}
