using MedicalSimulation.Core.Services.DTOs;

namespace MedicalSimulation.Core.Services.Interfaces;

public interface IRegistrationService
{
    Task<RegistrationResultDto> RegisterStudentAsync(
        string applicationUserId,
        string firstName,
        string lastName,
        string email,
        string phoneNumber,
        string studentId);

    Task<RegistrationResultDto> RegisterInstructorAsync(
        string applicationUserId,
        string firstName,
        string lastName,
        string email,
        string phoneNumber,
        string employeeId,
        int specializationId);
}
