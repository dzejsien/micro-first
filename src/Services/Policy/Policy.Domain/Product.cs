using System.Collections.Generic;

namespace Policy.Domain
{
    public partial class Product
    {
        public Product()
        {
            PolicyProduct = new HashSet<PolicyProduct>();
        }

        public Product(string code)
        {
            Code = code;
        }

        public long ProductId { get; protected set; }
        public string Code { get; protected set; }

        public ICollection<PolicyProduct> PolicyProduct { get; protected set; }
    }
}
