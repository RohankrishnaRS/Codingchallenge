using System;

namespace LoanManagementSystem.exception
{
    public class InvalidLoanException : Exception
    {
        public InvalidLoanException(string message) : base(message) { }
    }
}
