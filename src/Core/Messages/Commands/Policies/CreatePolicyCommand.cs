using Core.Common.EventBus;
using System;
using System.Collections.Generic;

namespace Core.Messages.Commands.Policies
{
    public class CreatePolicyCommand : ICommand
    {
        public string InsuredFirstName { get; set; }
        public string InsuredLastName { get; set; }
        public string InsuredNumber { get; set; }

        public DateTime PolicyDateFrom { get; set; }
        public DateTime PolicyDateTo { get; set; }

        public IList<string> ProductsCodes { get; set; }

    }
}
