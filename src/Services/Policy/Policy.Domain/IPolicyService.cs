using Core.Messages.Commands.Policies;
using System.Threading.Tasks;

namespace Policy.Domain
{
    public interface IPolicyService
    {
        Task<CreatePolicyResponse> CreatePolicyAsync(CreatePolicyCommand command);
    }
}
