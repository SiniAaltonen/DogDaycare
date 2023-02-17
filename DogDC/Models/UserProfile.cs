namespace DogDC.Models
{
    public partial class UserProfile
    {

        public string Avatar { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public int? CustomerId { get; set; }

        public virtual Customer? Customer { get; set; }

    }
}
