using System.ComponentModel.DataAnnotations.Schema;

namespace Pustok.Areas.Admin.Dtos
{
    public class SliderCreateDto
	{
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        [NotMapped]
        public IFormFile File { get; set; } = null!;
    }
}