namespace UserRegistraion.ViewModels
{
    public class PinVerificationResponse
    {
        public Guid UserId { get; set; }
        public bool MobileVerified { get; set; }
        public bool EmailVerified { get; set; }
    }
}
