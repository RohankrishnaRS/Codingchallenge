namespace LoanManagementSystem.entity
{
    public class Loan
    {
        public int LoanId { get; set; }
        public Customer Customer { get; set; }
        public double PrincipalAmount { get; set; }
        public double InterestRate { get; set; }
        public int LoanTerm { get; set; }
        public string LoanType { get; set; }
        public string LoanStatus { get; set; }

        public Loan() { }

        public Loan(int loanId, Customer customer, double principal, double interestRate, int loanTerm, string loanType, string loanStatus)
        {
            LoanId = loanId;
            Customer = customer;
            PrincipalAmount = principal;
            InterestRate = interestRate;
            LoanTerm = loanTerm;
            LoanType = loanType;
            LoanStatus = loanStatus;
        }

        public override string ToString()
        {
            return $"LoanID: {LoanId}, CustomerID: {Customer?.CustomerId}, Principal: {PrincipalAmount}, InterestRate: {InterestRate}, Term: {LoanTerm}, Type: {LoanType}, Status: {LoanStatus}";
        }
    }
}