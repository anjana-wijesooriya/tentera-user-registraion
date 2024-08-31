namespace UserRegistraion.ViewModels
{
    public class BiometricStatusRequest
    {
        public Guid UserId { get; set; }
        public bool HasEnableBiometric { get; set; }
    }
}
