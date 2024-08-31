namespace UserRegistraion.Entities
{
    public class Otp
    {
        public Guid Id { get; set; }
        public string? MobilePin { get; set; }
        public string? EmailPin { get; set; }
        public bool EmailVerified { get; set; }
        public bool MobileVerified { get; set; }
        public Guid UserId { get; set; }
        public virtual required User User { get; set; }
    }
}
