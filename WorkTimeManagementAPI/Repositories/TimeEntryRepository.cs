    using Dapper;
    using Npgsql;
    using WorkTimeManagementAPI.Models;

    public class TimeEntryRepository
    {
        private readonly string _connectionString;

        public TimeEntryRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<TimeEntry>> GetByEmployeeIdAsync(int employeeId)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QueryAsync<TimeEntry>(
                "SELECT * FROM TimeEntries WHERE EmployeeId = @EmployeeId",
                new { EmployeeId = employeeId });
        }

        public async Task<TimeEntry> GetByIdAsync(int employeeId, int entryId)
        {
        using var connection = new NpgsqlConnection(_connectionString);
        var result = await connection.QuerySingleOrDefaultAsync<TimeEntry>(
            "SELECT * FROM TimeEntries WHERE EmployeeId = @EmployeeId AND Id = @EntryId",
            new { EmployeeId = employeeId, EntryId = entryId });

        if (result == null)
        {
            throw new KeyNotFoundException($"Time entry with ID {entryId} for employee {employeeId} was not found.");
        }

        return result;
    }

        public async Task<int> AddAsync(TimeEntry timeEntry)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.ExecuteAsync(
                "INSERT INTO TimeEntries (EmployeeId, Date, HoursWorked) VALUES (@EmployeeId, @Date, @HoursWorked)",
                timeEntry);
        }

        public async Task<int> UpdateAsync(TimeEntry timeEntry)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.ExecuteAsync(
                "UPDATE TimeEntries SET Date = @Date, HoursWorked = @HoursWorked WHERE Id = @Id AND EmployeeId = @EmployeeId",
                timeEntry);
        }

        public async Task<int> DeleteAsync(int employeeId, int entryId)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.ExecuteAsync(
                "DELETE FROM TimeEntries WHERE EmployeeId = @EmployeeId AND Id = @EntryId",
                new { EmployeeId = employeeId, EntryId = entryId });
        }
    }


