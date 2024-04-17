using Pustak.Models;

namespace Pustak.ViewModels
{
    public class ProductSearchVm
    {
        public string Name { get; set; } = null!;
        public int CategoryId { get; set; }
        public List<Category>? Categories { get; set; }
    }
}
