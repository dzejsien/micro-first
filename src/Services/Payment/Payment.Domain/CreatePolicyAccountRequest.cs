namespace Payment.Domain
{
    public class CreatePolicyAccountRequest
    {
        public int Charge { get; set; }
        public string Number { get; set; }
        public long PolicyId { get; set; }
    }
}