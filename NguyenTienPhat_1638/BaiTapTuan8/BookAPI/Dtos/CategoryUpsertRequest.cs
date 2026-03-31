using System.ComponentModel.DataAnnotations;

namespace BookAPI.Dtos;

public class CategoryUpsertRequest
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
}
