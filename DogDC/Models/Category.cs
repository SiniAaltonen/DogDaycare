using System;
using System.Collections.Generic;

namespace DogDC.Models
{
    public partial class Category
    {
        public Category()
        {
            Utilities = new HashSet<Utility>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Utility> Utilities { get; set; }
    }
}
