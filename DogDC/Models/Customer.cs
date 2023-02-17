using System;
using System.Collections.Generic;

namespace DogDC.Models
{
    public partial class Customer
    {
        public Customer()
        {
            BookingStatuses = new HashSet<BookingStatus>();
            Pets = new HashSet<Pet>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Zipcode { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int? IsAdmin { get; set; }

        public virtual ICollection<BookingStatus> BookingStatuses { get; set; }
        public virtual ICollection<Pet> Pets { get; set; }
    }
}
