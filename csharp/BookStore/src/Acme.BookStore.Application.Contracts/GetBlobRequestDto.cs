using System.ComponentModel.DataAnnotations;

namespace Acme.BookStore;
public class GetBlobRequestDto
{
    [Required]
    public string Name { get; set; }
}