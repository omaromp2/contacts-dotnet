
using System.ComponentModel.DataAnnotations;

public class ContactIndexViewModel
{
    public IEnumerable<Contact> Contacts { get; set; }
    public string UserId { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(50)]
    public string Name { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [StringLength(50)]
    public string Email { get; set; }

}
