namespace UserRegistraion.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public int NIC { get; set; }
        public string Name { get; set; }    
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string PassCode { get; set; } = "";
        public bool HasAcceptedPolicies { get; set; } = false;
        public bool EnableBiometric { get; set; } = false;
        public Otp Otp { get; set; }
    }
}
