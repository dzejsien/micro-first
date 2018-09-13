namespace Policy.Domain
{
    public partial class PolicyProduct
    {
        public PolicyProduct()
        {

        }

        public PolicyProduct(Policy policy, Product product)
        {
            Policy = policy;
            Product = product;
        }

        public long PolicyId { get; protected set; }
        public long ProductId { get; protected set; }

        public Policy Policy { get; protected set; }
        public Product Product { get; protected set; }
    }
}
