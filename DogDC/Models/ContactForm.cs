using System;
using System.Collections.Generic;

namespace DogDC.Models
{
    public partial class ContactForm
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string Message { get; set; } = null!;
        public DateTime Timestamp { get; set; }
    }
}
