using Dapper;
using Npgsql;
using WorkTimeManagementAPI.Models;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetEmployeesAsync();
    Task<Employee?> GetEmployeeByIdAsync(int id);
    Task<int> AddEmployeeAsync(Employee employee);
    Task<int> UpdateEmployeeAsync(int id, Employee employee);
    Task<int> DeleteEmployeeAsync(int id);
}
    public class EmployeeRepository(IConfiguration configuration) : IEmployeeRepository
    {
        private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

        public async Task<IEnumerable<Employee>> GetEmployeesAsync()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QueryAsync<Employee>("SELECT * FROM Employees");
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QuerySingleOrDefaultAsync<Employee>(
                "SELECT * FROM Employees WHERE Id = @Id", new { Id = id });
        }

        public async Task<int> AddEmployeeAsync(Employee employee)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.ExecuteAsync(
                "INSERT INTO Employees (FirstName, LastName, Email) VALUES (@FirstName, @LastName, @Email)",
                employee);
        }

        public async Task<int> UpdateEmployeeAsync(int id, Employee employee)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.ExecuteAsync(
                "UPDATE Employees SET FirstName = @FirstName, LastName = @LastName, Email = @Email WHERE Id = @Id",
                new { employee.FirstName, employee.LastName, employee.Email, Id = id });
        }

        public async Task<int> DeleteEmployeeAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.ExecuteAsync("DELETE FROM Employees WHERE Id = @Id", new { Id = id });
        }
    }


