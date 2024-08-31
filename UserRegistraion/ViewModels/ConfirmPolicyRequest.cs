namespace UserRegistraion.ViewModels
{
    public class ConfirmPolicyRequest
    {
        public Guid UserId { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
