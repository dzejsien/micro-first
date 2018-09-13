using System.Collections.Generic;

namespace Policy.Domain
{
    public partial class Insured
    {
        public Insured()
        {
            Policy = new HashSet<Policy>();
        }

        public Insured(string firstName, string lastName, string number)
        {
            FirstName = firstName;
            LastName = lastName;
            Number = number;
        }

        public long InsuredId { get; protected set; }
        public string FirstName { get; protected set; }
        public string LastName { get; protected set; }
        public string Number { get; protected set; }

        public ICollection<Policy> Policy { get; protected set; }
    }
}
