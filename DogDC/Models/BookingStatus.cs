using System;
using System.Collections.Generic;

namespace DogDC.Models
{
    public partial class BookingStatus
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public int? BookingId { get; set; }

        public virtual Booking? Booking { get; set; }
        public virtual Customer? Customer { get; set; }
    }
}
