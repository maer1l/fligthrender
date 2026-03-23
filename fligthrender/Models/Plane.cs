using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace fligthrender.Models;

[Table("planes")]
public partial class Plane
{
    [Key]
    [Column("plane_id")]
    public int PlaneId { get; set; }

    [Column("model")]
    [StringLength(50)]
    [Unicode(false)]
    [Required(ErrorMessage = "Please enter plane's model.")]
    public string? Model { get; set; }

    [Column("speed")]
    [Required(ErrorMessage = "Please enter plane's speed.")]
    public int? Speed { get; set; }

    [Column("weight")]
    [Required(ErrorMessage = "Please enter plane's weight.")]
    public float? Weight { get; set; }

    [Column("price", TypeName = "money")]
    [Required(ErrorMessage = "Please enter plane's price.")]
    public decimal? Price { get; set; }

    [Column("year")]
    [Required(ErrorMessage = "Please enter plane's year of manufactured.")]
    public int? Year { get; set; }

    [Column("brand_id")]
    [Required(ErrorMessage = "Please enter plane's brand.")]
    public int? BrandId { get; set; }

    [ForeignKey("BrandId")]
    [InverseProperty("Planes")]
    public virtual Manufacturer? Brand { get; set; }
}
