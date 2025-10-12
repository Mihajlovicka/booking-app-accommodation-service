using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccommodationService.Model.Entity;

[Table("users")]
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("username")]
    public string Username { get; set; }
    
    [Required]
    [MaxLength(36)]
    [Column("external_id")]
    public Guid ExternalId { get; init; }
    
    [Required]
    [Column("status")]
    public bool Status { get; set; } = true;
}