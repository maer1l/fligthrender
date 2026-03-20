using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace fligthrender.Models;

[Keyless]
[Table("planespictures")]
public partial class Planespicture
{
    [Column("plane_id")]
    public int? PlaneId { get; set; }

    [Column("path")]
    [Unicode(false)]
    public string? Path { get; set; }

    [ForeignKey("PlaneId")]
    public virtual Plane? Plane { get; set; }
}
