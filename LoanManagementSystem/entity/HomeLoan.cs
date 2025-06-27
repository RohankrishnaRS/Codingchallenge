namespace LoanManagementSystem.entity
{
    public class HomeLoan : Loan
    {
        public string PropertyAddress { get; set; }
        public int PropertyValue { get; set; }

        public HomeLoan() { }

        public HomeLoan(int loanId, Customer customer, double principal, double interestRate, int loanTerm, string loanStatus,
                         string propertyAddress, int propertyValue)
            : base(loanId, customer, principal, interestRate, loanTerm, "HomeLoan", loanStatus)
        {
            PropertyAddress = propertyAddress;
            PropertyValue = propertyValue;
        }

        public override string ToString()
        {
            return base.ToString() + $", PropertyAddress: {PropertyAddress}, PropertyValue: {PropertyValue}";
        }
    }
}