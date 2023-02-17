using System;
using System.Collections.Generic;

namespace DogDC.Models
{
    public partial class Booking
    {
        public Booking()
        {
            BookingStatuses = new HashSet<BookingStatus>();
        }

        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int? UtilityId { get; set; }
        
        public virtual Utility? Utility { get; set; }
        public virtual ICollection<BookingStatus> BookingStatuses { get; set; }
    }
}
