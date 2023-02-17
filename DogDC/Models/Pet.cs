using System;
using System.Collections.Generic;

namespace DogDC.Models
{
    public partial class Pet
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Age { get; set; }
        public string Diet { get; set; } = null!;
        public string Breed { get; set; } = null!;
        public int? GroupId { get; set; }
        public int? CustomerId { get; set; }
        public string BlobUrl { get; set; } = null!;

        public virtual Customer? Customer { get; set; }
        public virtual Group? Group { get; set; }
    }
}
