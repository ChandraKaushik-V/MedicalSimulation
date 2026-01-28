-- Script to Extract Stored Procedure Definitions
-- Run this in SSMS to get all stored procedure definitions

USE [T];
GO

-- Get all stored procedure definitions
SELECT 
    OBJECT_NAME(object_id) AS ProcedureName,
    OBJECT_DEFINITION(object_id) AS Definition
FROM sys.procedures
WHERE name LIKE 'sp_%'
ORDER BY name;
GO

-- Alternative: Get individual procedure definitions
EXEC sp_helptext 'sp_CompleteSimulationAttempt';
GO

EXEC sp_helptext 'sp_GetInstructorDashboardData';
GO

EXEC sp_helptext 'sp_GetStudentDashboardData';
GO

EXEC sp_helptext 'sp_RegisterInstructor';
GO

EXEC sp_helptext 'sp_StartSimulationAttempt';
GO

EXEC sp_helptext 'sp_ValidateSimulationAnswer';
GO
