using System.Collections.Generic;
using LoanManagementSystem.entity;

namespace LoanManagementSystem.dao
{
    public interface ILoanRepository
    {
        bool ApplyLoan(Loan loan);
        double CalculateInterest(int loanId);
        double CalculateInterest(double principal, double rate, int tenure);
        string LoanStatus(int loanId);
        double CalculateEMI(int loanId);
        double CalculateEMI(double principal, double rate, int tenure);
        string LoanRepayment(int loanId, double amount);
        List<Loan> GetAllLoan();
        Loan GetLoanById(int loanId);
    }
}
