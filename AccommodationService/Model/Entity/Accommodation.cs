using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccommodationService.Model.Entity;

public class Accommodation
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(36)]
    [Column("external_id")]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid ExternalId { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("name")]
    public string Name { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("description")]
    public string Description { get; set; }

    [ForeignKey("OwnerId")]
    [Column("owner_id")]
    public int OwnerId { get; set; }
    public User Owner { get; set; }

    [ForeignKey("AddressId")]
    [Column("address_id")]
    public int AddressId { get; set; }
    public Address Address { get; set; }

    [Range(1, int.MaxValue)]
    [Column("min_number_of_guests")]
    public int? MinNumberOfGuests { get; set; }

    [Range(0, int.MaxValue)]
    [Column("max_number_of_guests")]
    public int? MaxNumberOfGuests { get; set; }
    
    [Required]
    [Column]
    public bool AutomaticReservation { get; set; } = false;

    [Required]
    [Column("status")]
    public bool Active { get; set; } = true;
    
    public IList<Equipment> Equipment { get; set; }

    public IList<Picture> Pictures { get; set; } = new List<Picture>();
    
    public PriceType PriceType { get; set; }
}