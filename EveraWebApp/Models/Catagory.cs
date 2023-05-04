using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace EveraWebApp.Models
{
    public class Catagory
    {
        public int Id { get; set; }
        [Required, MaxLength(20)]
        public string Name { get; set; } = null!;
        public ICollection<Product>? Products { get; set; }
    }
}
