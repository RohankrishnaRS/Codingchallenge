namespace LoanManagementSystem.entity
{
    public class CarLoan : Loan
    {
        public string CarModel { get; set; }
        public int CarValue { get; set; }

        public CarLoan() { }

        public CarLoan(int loanId, Customer customer, double principal, double interestRate, int loanTerm, string loanStatus,
                       string carModel, int carValue)
            : base(loanId, customer, principal, interestRate, loanTerm, "CarLoan", loanStatus)
        {
            CarModel = carModel;
            CarValue = carValue;
        }

        public override string ToString()
        {
            return base.ToString() + $", CarModel: {CarModel}, CarValue: {CarValue}";
        }
    }
}