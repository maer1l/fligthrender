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
    public string? Model { get; set; }

    [Column("speed")]
    public int? Speed { get; set; }

    [Column("weight")]
    public float? Weight { get; set; }

    [Column("price", TypeName = "money")]
    public decimal? Price { get; set; }

    [Column("year")]
    public int? Year { get; set; }

    [Column("brand_id")]
    public int? BrandId { get; set; }

    [ForeignKey("BrandId")]
    [InverseProperty("Planes")]
    public virtual Manufacturer? Brand { get; set; }
}
