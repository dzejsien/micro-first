namespace Payment.Domain
{
    public class SendRemittanceRequest
    {
        public string AccountNumber { get; set; }
        public int Value { get; set; }
    }
}