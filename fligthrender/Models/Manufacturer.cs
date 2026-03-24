using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace fligthrender.Models;

[Table("manufacturers")]
public partial class Manufacturer
{
    [Key]
    [Column("brand_id")]
    public int BrandId { get; set; }

    [Column("name")]
    [StringLength(50)]
    [Required(ErrorMessage = "Please enter name's manufacturer.")]
    [Unicode(false)]
    public string? Name { get; set; }

    [Column("description")]
    [Required(ErrorMessage = "Please enter description manufacturer.")]
    [Unicode(false)]
    public string? Description { get; set; }

    [Column("address")]
    [StringLength(50)]
    [Required(ErrorMessage = "Please enter address manufacturer.")]
    [Unicode(false)]
    public string? Address { get; set; }

    [InverseProperty("Brand")]
    public virtual ICollection<Plane> Planes { get; set; } = new List<Plane>();
}
