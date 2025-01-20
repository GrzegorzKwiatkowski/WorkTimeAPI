CREATE TABLE Employees (
    Id SERIAL PRIMARY KEY,
    FirstName VARCHAR(100) NOT NULL,
    LastName VARCHAR(100) NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL
);

CREATE TABLE TimeEntries (
    Id SERIAL PRIMARY KEY,
    EmployeeId INT NOT NULL REFERENCES Employees(Id) ON DELETE CASCADE,
    Date DATE NOT NULL,
    HoursWorked INT NOT NULL CHECK (HoursWorked >= 1 AND HoursWorked <= 24),
    UNIQUE (EmployeeId, Date)
);

INSERT INTO Employees (FirstName, LastName, Email) VALUES
('Jan', 'Kowalski', 'jan.kowalski@example.com');

INSERT INTO TimeEntries (EmployeeId, Date, HoursWorked)
VALUES (1, '2024-12-01', 8);