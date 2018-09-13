using Core.Messages.Commands.Policies;
using Policy.Domain;
using System.Linq;
using System.Threading.Tasks;

namespace Policy.DataAccess.SqlServer
{
    public class PolicyService : IPolicyService
    {
        private readonly PolicyContext _policyContext;

        public PolicyService(PolicyContext policyContext)
        {
            _policyContext = policyContext;
        }

        public async Task<CreatePolicyResponse> CreatePolicyAsync(CreatePolicyCommand command)
        {
            var policy = new Domain.Policy(command.PolicyDateFrom, command.PolicyDateTo);

            _policyContext.Policy.Add(policy);

            var insured = new Insured(command.InsuredFirstName, command.InsuredLastName, command.InsuredNumber);
            policy.AttachInsured(insured);
            policy.AddProducts(command.ProductsCodes.ToList());

            await _policyContext.SaveChangesAsync();

            return new CreatePolicyResponse { PolicyId = policy.PolicyId };
        }
    }
}
