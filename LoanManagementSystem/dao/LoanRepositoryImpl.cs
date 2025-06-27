using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using LoanManagementSystem.entity;
using LoanManagementSystem.exception;
using LoanManagementSystem.util;

namespace LoanManagementSystem.dao
{
    public class LoanRepositoryImpl : ILoanRepository
    {
        public bool ApplyLoan(Loan loan)
        {
            using (SqlConnection conn = DBConnUtil.GetConnection())
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    string insertCustomer = @"INSERT INTO Customer (Name, EmailAddress, PhoneNumber, Address, CreditScore)
                                              VALUES (@Name, @Email, @Phone, @Address, @Score);
                                              SELECT SCOPE_IDENTITY();";

                    SqlCommand cmd1 = new SqlCommand(insertCustomer, conn, transaction);
                    cmd1.Parameters.AddWithValue("@Name", loan.Customer.Name);
                    cmd1.Parameters.AddWithValue("@Email", loan.Customer.EmailAddress);
                    cmd1.Parameters.AddWithValue("@Phone", loan.Customer.PhoneNumber);
                    cmd1.Parameters.AddWithValue("@Address", loan.Customer.Address);
                    cmd1.Parameters.AddWithValue("@Score", loan.Customer.CreditScore);

                    int customerId = Convert.ToInt32(cmd1.ExecuteScalar());
                    loan.Customer.CustomerId = customerId;

                    string insertLoan = @"INSERT INTO Loan (CustomerId, PrincipalAmount, InterestRate, LoanTerm, LoanType, LoanStatus)
                                          VALUES (@CustId, @Principal, @Rate, @Term, @Type, 'Pending');
                                          SELECT SCOPE_IDENTITY();";

                    SqlCommand cmd2 = new SqlCommand(insertLoan, conn, transaction);
                    cmd2.Parameters.AddWithValue("@CustId", customerId);
                    cmd2.Parameters.AddWithValue("@Principal", loan.PrincipalAmount);
                    cmd2.Parameters.AddWithValue("@Rate", loan.InterestRate);
                    cmd2.Parameters.AddWithValue("@Term", loan.LoanTerm);
                    cmd2.Parameters.AddWithValue("@Type", loan.LoanType);

                    int loanId = Convert.ToInt32(cmd2.ExecuteScalar());
                    loan.LoanId = loanId;

                    if (loan is HomeLoan homeLoan)
                    {
                        string insertHome = @"INSERT INTO HomeLoan (LoanId, PropertyAddress, PropertyValue)
                                              VALUES (@LoanId, @Address, @Value);";
                        SqlCommand cmd3 = new SqlCommand(insertHome, conn, transaction);
                        cmd3.Parameters.AddWithValue("@LoanId", loanId);
                        cmd3.Parameters.AddWithValue("@Address", homeLoan.PropertyAddress);
                        cmd3.Parameters.AddWithValue("@Value", homeLoan.PropertyValue);
                        cmd3.ExecuteNonQuery();
                    }
                    else if (loan is CarLoan carLoan)
                    {
                        string insertCar = @"INSERT INTO CarLoan (LoanId, CarModel, CarValue)
                                             VALUES (@LoanId, @Model, @Value);";
                        SqlCommand cmd4 = new SqlCommand(insertCar, conn, transaction);
                        cmd4.Parameters.AddWithValue("@LoanId", loanId);
                        cmd4.Parameters.AddWithValue("@Model", carLoan.CarModel);
                        cmd4.Parameters.AddWithValue("@Value", carLoan.CarValue);
                        cmd4.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public double CalculateInterest(int loanId)
        {
            Loan loan = GetLoanById(loanId);
            return CalculateInterest(loan.PrincipalAmount, loan.InterestRate, loan.LoanTerm);
        }

        public double CalculateInterest(double principal, double rate, int tenure)
        {
            return (principal * rate * tenure) / 12;
        }

        public string LoanStatus(int loanId)
        {
            Loan loan = GetLoanById(loanId);
            string status = loan.Customer.CreditScore > 650 ? "Approved" : "Rejected";

            using (SqlConnection conn = DBConnUtil.GetConnection())
            {
                conn.Open();
                string query = "UPDATE Loan SET LoanStatus = @Status WHERE LoanId = @LoanId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@LoanId", loanId);
                cmd.ExecuteNonQuery();
            }
            return status;
        }

        public double CalculateEMI(int loanId)
        {
            Loan loan = GetLoanById(loanId);
            return CalculateEMI(loan.PrincipalAmount, loan.InterestRate, loan.LoanTerm);
        }

        public double CalculateEMI(double principal, double rate, int tenure)
        {
            double r = rate / 12 / 100;
            return (principal * r * Math.Pow(1 + r, tenure)) / (Math.Pow(1 + r, tenure) - 1);
        }

        public string LoanRepayment(int loanId, double amount)
        {
            double emi = CalculateEMI(loanId);
            int noOfEmis = (int)(amount / emi);

            if (noOfEmis < 1)
                return "Payment rejected: amount less than one EMI.";

            return $"Payment accepted: {noOfEmis} EMI(s) covered.";
        }

        public List<Loan> GetAllLoan()
        {
            List<Loan> loans = new List<Loan>();
            using (SqlConnection conn = DBConnUtil.GetConnection())
            {
                conn.Open();
                string query = @"SELECT L.LoanId, L.PrincipalAmount, L.InterestRate, L.LoanTerm, L.LoanType, L.LoanStatus,
                                        C.CustomerId, C.Name, C.EmailAddress, C.PhoneNumber, C.Address, C.CreditScore
                                 FROM Loan L
                                 JOIN Customer C ON L.CustomerId = C.CustomerId";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Customer customer = new Customer
                    {
                        CustomerId = Convert.ToInt32(reader["CustomerId"]),
                        Name = reader["Name"].ToString(),
                        EmailAddress = reader["EmailAddress"].ToString(),
                        PhoneNumber = reader["PhoneNumber"].ToString(),
                        Address = reader["Address"].ToString(),
                        CreditScore = Convert.ToInt32(reader["CreditScore"])
                    };

                    Loan loan = new Loan
                    {
                        LoanId = Convert.ToInt32(reader["LoanId"]),
                        PrincipalAmount = Convert.ToDouble(reader["PrincipalAmount"]),
                        InterestRate = Convert.ToDouble(reader["InterestRate"]),
                        LoanTerm = Convert.ToInt32(reader["LoanTerm"]),
                        LoanType = reader["LoanType"].ToString(),
                        LoanStatus = reader["LoanStatus"].ToString(),
                        Customer = customer
                    };

                    loans.Add(loan);
                }
            }
            return loans;
        }

        public Loan GetLoanById(int loanId)
        {
            using (SqlConnection conn = DBConnUtil.GetConnection())
            {
                conn.Open();
                string query = @"SELECT L.LoanId, L.PrincipalAmount, L.InterestRate, L.LoanTerm, L.LoanType, L.LoanStatus,
                                        C.CustomerId, C.Name, C.EmailAddress, C.PhoneNumber, C.Address, C.CreditScore
                                 FROM Loan L
                                 JOIN Customer C ON L.CustomerId = C.CustomerId
                                 WHERE L.LoanId = @LoanId";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@LoanId", loanId);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    Customer customer = new Customer
                    {
                        CustomerId = Convert.ToInt32(reader["CustomerId"]),
                        Name = reader["Name"].ToString(),
                        EmailAddress = reader["EmailAddress"].ToString(),
                        PhoneNumber = reader["PhoneNumber"].ToString(),
                        Address = reader["Address"].ToString(),
                        CreditScore = Convert.ToInt32(reader["CreditScore"])
                    };

                    Loan loan = new Loan
                    {
                        LoanId = Convert.ToInt32(reader["LoanId"]),
                        PrincipalAmount = Convert.ToDouble(reader["PrincipalAmount"]),
                        InterestRate = Convert.ToDouble(reader["InterestRate"]),
                        LoanTerm = Convert.ToInt32(reader["LoanTerm"]),
                        LoanType = reader["LoanType"].ToString(),
                        LoanStatus = reader["LoanStatus"].ToString(),
                        Customer = customer
                    };

                    return loan;
                }
                else
                {
                    throw new InvalidLoanException("Loan not found.");
                }
            }
        }
    }
}
