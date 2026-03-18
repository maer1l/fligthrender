using System.Numerics;
using fligthrender.Models;
using Plane = fligthrender.Models.Plane;

namespace fligthrender.ViewModels
{
    public class SearchPlanesManufacturer
    {
        List<Plane> planes = new List<Plane>();

        List<Manufacturer> manufacturers = new List<Manufacturer>();

        public List<Plane> Planes { get => planes; set => planes = value; }
        public List<Manufacturer> Manufacturers { get => manufacturers; set => manufacturers = value; }

        public Plane PlaneInfo { get => planes[0]; set => planes[0] = value; }
        public Manufacturer ManufacturerInfo { get => manufacturers[0]; set => manufacturers[0] = value; }
    }
}
