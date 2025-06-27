using System;
using LoanManagementSystem.dao;
using LoanManagementSystem.entity;

namespace LoanManagementSystem.main
{
    public class MainModule
    {
        public static void Run()
        {
            ILoanRepository loanRepo = new LoanRepositoryImpl();
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\n===== Loan Management System =====");
                Console.WriteLine("1. Apply Loan");
                Console.WriteLine("2. Get All Loans");
                Console.WriteLine("3. Get Loan by ID");
                Console.WriteLine("4. Loan Repayment");
                Console.WriteLine("5. Exit");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Applying a loan (stub)...");
                        break;
                    case "2":
                        var loans = loanRepo.GetAllLoan();
                        foreach (var loan in loans)
                            Console.WriteLine(loan);
                        break;
                    case "3":
                        Console.Write("Enter Loan ID: ");
                        int id = int.Parse(Console.ReadLine());
                        try
                        {
                            var loan = loanRepo.GetLoanById(id);
                            Console.WriteLine(loan);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                        break;
                    case "4":
                        Console.WriteLine("Repaying loan (stub)...");
                        break;
                    case "5":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }
        }
    }
}
