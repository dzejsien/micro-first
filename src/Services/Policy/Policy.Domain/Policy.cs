using System;
using System.Collections.Generic;

namespace Policy.Domain
{
    public partial class Policy
    {
        public Policy()
        {
            PolicyProduct = new HashSet<PolicyProduct>();
        }

        public Policy(DateTime dateFrom, DateTime dateTo)
        {
            DateFrom = dateFrom;
            DateTo = dateTo;
            PolicyProduct = new List<PolicyProduct>();
        }

        public long PolicyId { get; protected set; }
        public long InsuredId { get; protected set; }
        public DateTime DateFrom { get; protected set; }
        public DateTime DateTo { get; protected set; }

        public Insured Insured { get; protected set; }
        public ICollection<PolicyProduct> PolicyProduct { get; protected set; }

        public void AttachInsured(Insured insured)
        {
            Insured = insured;
        }

        public void AddProducts(IReadOnlyList<string> productCodes)
        {
            foreach (var code in productCodes)
            {
                var product = new Product(code);
                PolicyProduct.Add(new PolicyProduct(this, product));
            }
        }
    }
}
