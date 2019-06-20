using System;

namespace BankingApplication.Domain.Customers
{
    public class CustomerState
    {
        public CustomerState(Guid customerId, string fullName, string address)
        {
            CustomerId = customerId;
            FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
            Address = address ?? throw new ArgumentNullException(nameof(address));
        }

        public Guid CustomerId { get; }
        public string FullName { get; }
        public string Address { get; }
    }
}
