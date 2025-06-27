namespace LoanManagementSystem.entity
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int CreditScore { get; set; }

        public Customer() { }

        public Customer(int customerId, string name, string email, string phone, string address, int creditScore)
        {
            CustomerId = customerId;
            Name = name;
            EmailAddress = email;
            PhoneNumber = phone;
            Address = address;
            CreditScore = creditScore;
        }

        public override string ToString()
        {
            return $"CustomerID: {CustomerId}, Name: {Name}, Email: {EmailAddress}, Phone: {PhoneNumber}, Address: {Address}, Credit Score: {CreditScore}";
        }
    }
}
