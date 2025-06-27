using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanManagementSystem.entity
{
    public abstract class Loan
    {
        [Key]
        public int LoanId { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public decimal PrincipalAmount { get; set; }
        public decimal InterestRate { get; set; }
        public int LoanTerm { get; set; } // in months
        public string LoanType { get; set; }
        public string LoanStatus { get; set; } // Pending, Approved
    }
}
