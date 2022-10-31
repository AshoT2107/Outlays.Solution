using Outlays.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Outlays.Data.Entities;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    public long ChatId { get; set; }

    public UserStep Step { get; set; }

    [Column(TypeName = "nvarchar(50)")]
    [Required]
    public string? Name { get; set; }

    [Column(TypeName = "nvarchar(50)")]
    public string? Username { get; set; }

    [Column(TypeName = "nvarchar(20)")]
    public string? Phone { get; set; }

    public string? Email { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public int? RoomId { get; set; }
    public bool IsAdmin { get; set; }

    public List<Outlay>? Outlays { get; set; }

    [NotMapped]
    public string? Fullname => Username ?? Name;
}