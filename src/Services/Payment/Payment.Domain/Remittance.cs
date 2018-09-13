namespace Payment.Domain
{
    public class Remittance
    {
        public Remittance(string accountNumber, decimal value)
        {
            AccountNumber = accountNumber;
            Value = value;
        }

        public string AccountNumber { get; set; }
        public decimal Value { get; set; }
    }
}
