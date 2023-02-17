using System;
using System.Collections.Generic;

namespace DogDC.Models
{
    public partial class Group
    {
        public Group()
        {
            Pets = new HashSet<Pet>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        public virtual ICollection<Pet> Pets { get; set; }
    }
}
