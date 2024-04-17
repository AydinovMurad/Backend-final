using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pustok.Areas.Admin.Dtos;

public class SliderUpdateDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? ImagePath { get; set; }
    [NotMapped]
    public IFormFile? File { get; set; }
}