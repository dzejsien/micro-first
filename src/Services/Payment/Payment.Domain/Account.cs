using System;

namespace Payment.Domain
{
    public class Account
    {
        public Account(long policyId, decimal charge)
        {
            PolicyId = policyId;
            Number = new Random().Next(100000, 999999).ToString();
            Charge = charge;
            Balance = charge;
        }

        public long PolicyId { get; set; }
        public string Number { get; set; }
        public decimal Charge { get; set; }
        public decimal Balance { get; set; }
    }
}
