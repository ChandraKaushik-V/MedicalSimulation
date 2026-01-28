using MedicalSimulation.Core.Data;
using MedicalSimulation.Core.Services.DTOs;
using MedicalSimulation.Core.Services.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace MedicalSimulation.Core.Services.Implementations;

public class RegistrationService : IRegistrationService
{
    private readonly ApplicationDbContext _context;

    public RegistrationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RegistrationResultDto> RegisterStudentAsync(
        string applicationUserId,
        string firstName,
        string lastName,
        string email,
        string phoneNumber,
        string studentId)
    {
        var result = new RegistrationResultDto();

        try
        {
            var parameters = new[]
            {
                new SqlParameter("@ApplicationUserId", applicationUserId),
                new SqlParameter("@FirstName", firstName),
                new SqlParameter("@LastName", lastName),
                new SqlParameter("@Email", email),
                new SqlParameter("@PhoneNumber", phoneNumber ?? (object)DBNull.Value),
                new SqlParameter("@StudentId", studentId)
            };

            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "sp_RegisterStudent";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);

            await _context.Database.OpenConnectionAsync();

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                result.Success = reader.GetInt32(reader.GetOrdinal("Success")) == 1;
                
                if (result.Success)
                {
                    result.Id = Convert.ToInt32(reader.GetDecimal(reader.GetOrdinal("StudentTableId")));
                }
                else
                {
                    result.ErrorMessage = reader.GetString(reader.GetOrdinal("ErrorMessage"));
                }
            }
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = $"Registration failed: {ex.Message}";
        }
        finally
        {
            await _context.Database.CloseConnectionAsync();
        }

        return result;
    }

    public async Task<RegistrationResultDto> RegisterInstructorAsync(
        string applicationUserId,
        string firstName,
        string lastName,
        string email,
        string phoneNumber,
        string employeeId,
        int specializationId)
    {
        var result = new RegistrationResultDto();

        try
        {
            var parameters = new[]
            {
                new SqlParameter("@ApplicationUserId", applicationUserId),
                new SqlParameter("@FirstName", firstName),
                new SqlParameter("@LastName", lastName),
                new SqlParameter("@Email", email),
                new SqlParameter("@PhoneNumber", phoneNumber ?? (object)DBNull.Value),
                new SqlParameter("@EmployeeId", employeeId),
                new SqlParameter("@SpecializationId", specializationId)
            };

            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "sp_RegisterInstructor";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);

            await _context.Database.OpenConnectionAsync();

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                result.Success = reader.GetInt32(reader.GetOrdinal("Success")) == 1;
                
                if (result.Success)
                {
                    result.Id = Convert.ToInt32(reader.GetDecimal(reader.GetOrdinal("InstructorId")));
                }
                else
                {
                    result.ErrorMessage = reader.GetString(reader.GetOrdinal("ErrorMessage"));
                }
            }
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = $"Registration failed: {ex.Message}";
        }
        finally
        {
            await _context.Database.CloseConnectionAsync();
        }

        return result;
    }
}
