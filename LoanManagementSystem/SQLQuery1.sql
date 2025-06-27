-- Create the database
CREATE DATABASE LoanDB;
GO

-- Use the database
USE LoanDB;
GO

-- Customer Table
CREATE TABLE Customer (
    CustomerId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100),
    EmailAddress NVARCHAR(100),
    PhoneNumber NVARCHAR(20),
    Address NVARCHAR(255),
    CreditScore INT
);
GO

-- Loan Table
CREATE TABLE Loan (
    LoanId INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId INT FOREIGN KEY REFERENCES Customer(CustomerId),
    PrincipalAmount FLOAT,
    InterestRate FLOAT,
    LoanTerm INT,
    LoanType NVARCHAR(50),       -- e.g., 'HomeLoan' or 'CarLoan'
    LoanStatus NVARCHAR(50)      -- e.g., 'Pending', 'Approved'
);
GO

-- HomeLoan Table
CREATE TABLE HomeLoan (
    LoanId INT PRIMARY KEY FOREIGN KEY REFERENCES Loan(LoanId),
    PropertyAddress NVARCHAR(255),
    PropertyValue INT
);
GO

-- CarLoan Table
CREATE TABLE CarLoan (
    LoanId INT PRIMARY KEY FOREIGN KEY REFERENCES Loan(LoanId),
    CarModel NVARCHAR(100),
    CarValue INT
);
GO


-- Insert a sample customer
INSERT INTO Customer (Name, EmailAddress, PhoneNumber, Address, CreditScore)
VALUES ('Ravi Kumar', 'ravi.kumar@example.com', '9876543210', 'Chennai, TN', 700);

-- Insert a loan for that customer (LoanType = HomeLoan)
INSERT INTO Loan (CustomerId, PrincipalAmount, InterestRate, LoanTerm, LoanType, LoanStatus)
VALUES (1, 500000, 8.5, 60, 'HomeLoan', 'Pending');

-- Insert HomeLoan specific details
INSERT INTO HomeLoan (LoanId, PropertyAddress, PropertyValue)
VALUES (1, 'Plot #123, Anna Nagar, Chennai', 800000);

-- Insert another customer with a CarLoan
INSERT INTO Customer (Name, EmailAddress, PhoneNumber, Address, CreditScore)
VALUES ('Priya Sen', 'priya.sen@example.com', '9876512345', 'Bangalore, KA', 640);

-- Insert a loan for Priya (LoanType = CarLoan)
INSERT INTO Loan (CustomerId, PrincipalAmount, InterestRate, LoanTerm, LoanType, LoanStatus)
VALUES (2, 300000, 9.0, 48, 'CarLoan', 'Pending');

-- Insert CarLoan specific details
INSERT INTO CarLoan (LoanId, CarModel, CarValue)
VALUES (2, 'Hyundai Creta', 1200000);

