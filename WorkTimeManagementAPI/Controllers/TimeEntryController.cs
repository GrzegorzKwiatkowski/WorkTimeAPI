using Microsoft.AspNetCore.Mvc;
using Npgsql;
using WorkTimeManagementAPI.Models;

namespace WorkTimeManagementAPI.Controllers
{
    [ApiController]
    [Route("api/employees/{employeeId}/time-entries")]
    public class TimeEntryController : ControllerBase
    {
        private readonly TimeEntryRepository _timeEntryRepository;

        public TimeEntryController(TimeEntryRepository timeEntryRepository)
        {
            _timeEntryRepository = timeEntryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetTimeEntries(int employeeId)
        {
            var entries = await _timeEntryRepository.GetByEmployeeIdAsync(employeeId);
            return Ok(entries);
        }

        [HttpPost]
        public async Task<IActionResult> AddTimeEntry(int employeeId, [FromBody] TimeEntry timeEntry)
        {
            if (timeEntry.HoursWorked < 1 || timeEntry.HoursWorked > 24)
                return BadRequest("HoursWorked must be between 1 and 24.");

            timeEntry.EmployeeId = employeeId;

            try
            {
                await _timeEntryRepository.AddAsync(timeEntry);
                return CreatedAtAction(nameof(GetTimeEntries), new { employeeId }, timeEntry);
            }
            catch (PostgresException ex) when (ex.SqlState == "23505")
            {
                return Conflict("A time entry for this date already exists.");
            }
        }

        [HttpPut("{entryId}")]
        public async Task<IActionResult> UpdateTimeEntry(int employeeId, int entryId, [FromBody] TimeEntry timeEntry)
        {
            if (timeEntry.HoursWorked < 1 || timeEntry.HoursWorked > 24)
                return BadRequest("HoursWorked must be between 1 and 24.");

            timeEntry.Id = entryId;
            timeEntry.EmployeeId = employeeId;

            var rowsAffected = await _timeEntryRepository.UpdateAsync(timeEntry);
            if (rowsAffected == 0)
                return NotFound("Time entry not found.");

            return NoContent();
        }

        [HttpDelete("{entryId}")]
        public async Task<IActionResult> DeleteTimeEntry(int employeeId, int entryId)
        {
            var rowsAffected = await _timeEntryRepository.DeleteAsync(employeeId, entryId);
            if (rowsAffected == 0)
                return NotFound("Time entry not found.");

            return NoContent();
        }
    }
}

