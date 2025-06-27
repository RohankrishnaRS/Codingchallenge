using System.Collections.Generic;
using LoanManagementSystem.entity;

namespace LoanManagementSystem.dao
{
    public interface ILoanRepository
    {
        void ApplyLoan(Loan loan);
        decimal CalculateInterest(int loanId);
        decimal CalculateInterest(decimal principal, decimal rate, int term);
        void LoanStatus(int loanId);
        decimal CalculateEMI(int loanId);
        decimal CalculateEMI(decimal principal, decimal rate, int term);
        void LoanRepayment(int loanId, decimal amount);
        List<Loan> GetAllLoan();
        Loan GetLoanById(int loanId);
    }
}
