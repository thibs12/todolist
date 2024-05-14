using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Models;

public class TodoItem
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(100)")]
    public string? Name { get; set; }

    [Required]
    [Column(TypeName = "bit")]
    public bool IsComplete { get; set; }
}
