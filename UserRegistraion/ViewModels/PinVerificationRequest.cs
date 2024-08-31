namespace UserRegistraion.ViewModels
{
    public class PinVerificationRequest
    {
        public Guid UserId { get; set; }
        public string PinCode { get; set; }
    }
}
