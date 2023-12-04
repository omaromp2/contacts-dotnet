using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Contact
{
    public int ContactId { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(50)]
    public string Name { get; set; }

    [StringLength(10)]
    public string Phone { get; set; }

    [StringLength(10)]
    public string Fax { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [StringLength(50)]
    public string Email { get; set; }

    [Column(TypeName = "ntext")]
    public string Notes { get; set; }

    public DateTime LastUpdatedDate { get; set; }

    // Assuming you have a User class that represents the user
    // This is a foreign key to the User table
    [Required]
    public int UserId { get; set; }
    // public User User { get; set; }

}
