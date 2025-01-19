using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccommodationService.Model.Entity;

public class Picture
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("url")]
    public string Url { get; set; }

    [Required]
    [ForeignKey("AccommodationId")]
    [Column("accommodation_id")]
    public int AccommodationId { get; set; }

    public Accommodation Accommodation { get; set; }
}