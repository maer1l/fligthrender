using fligthrender.Models;
using System.ComponentModel.DataAnnotations;

namespace fligthrender.ViewModels
{
    public class PlaneWithPhoto
    {
        [Required(ErrorMessage = "Вы должны выбрать хотя бы один файл")]
        [Display(Name = "Выбор файлов")]
        public IFormFileCollection files { get; set; }

        public Plane plane { get; set; }

        public List<string> paths { get; set; } = new List<string>();

    }
}
