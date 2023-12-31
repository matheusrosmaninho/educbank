using System.ComponentModel.DataAnnotations;

namespace Acme.BookStore;
public class SaveBlobInputDto
{
    public byte[] Content { get; set; }

    [Required]
    public string Name { get; set; }
}