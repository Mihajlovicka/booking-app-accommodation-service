using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace AccommodationService.Model.Entity;

/**
 * Only accommodation owner are stored in this microservices, so them could be linked to accommodation. *
 */

[Table("users")]
public class User : IdentityUser<int>
{
    [Required]
    [MaxLength(36)]
    [Column("external_id")]
    public Guid ExternalId { get; set; }
    
    [Required]
    [Column("status")]
    public bool Status { get; set; } = true;
}