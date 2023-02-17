using System;
using System.Collections.Generic;

namespace DogDC.Models
{
    public partial class Utility
    {
        public Utility()
        {
            Bookings = new HashSet<Booking>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Price { get; set; }
        public int? CategoryId { get; set; }

        public virtual Category? Category { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
