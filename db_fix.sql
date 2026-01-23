-- 1. Drop constraints relying on ValidInstructorEmployeeIds
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE parent_object_id = OBJECT_ID(N'Instructors'))
BEGIN
    DECLARE @sql nvarchar(max) = N'';
    SELECT @sql += N'ALTER TABLE Instructors DROP CONSTRAINT ' + QUOTENAME(name) + N';'
    FROM sys.foreign_keys
    WHERE parent_object_id = OBJECT_ID(N'Instructors')
    AND referenced_object_id = OBJECT_ID(N'ValidInstructorEmployeeIds');
    
    IF @sql IS NOT NULL AND LEN(@sql) > 0 EXEC sp_executesql @sql;
END

-- 2. Drop the ValidInstructorEmployeeIds table (to recreate it clean)
DROP TABLE IF EXISTS ValidInstructorEmployeeIds;

-- 3. Create the table ValidInstructorEmployeeIds
CREATE TABLE ValidInstructorEmployeeIds (
    Id INT PRIMARY KEY IDENTITY(1,1),
    EmployeeId NVARCHAR(50) NOT NULL UNIQUE,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- 4. Seed Data
INSERT INTO ValidInstructorEmployeeIds (EmployeeId, FirstName, LastName, IsActive, CreatedAt)
VALUES 
    ('EMP001', 'John', 'Doe', 1, GETUTCDATE()),
    ('EMP002', 'Jane', 'Smith', 1, GETUTCDATE()),
    ('EMP003', 'Michael', 'Johnson', 1, GETUTCDATE()),
    ('EMP004', 'Sarah', 'Williams', 1, GETUTCDATE()),
    ('EMP005', 'David', 'Brown', 1, GETUTCDATE());

-- 5. Fix Instructors table schema

-- 5a. Drop the old 'EmployeeId' column from Instructors if it exists
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Instructors') AND name = 'EmployeeId')
BEGIN
    -- First drop any default constraints or indexes on this column just in case
    DECLARE @sql2 nvarchar(max) = N'';
    SELECT @sql2 += N'ALTER TABLE Instructors DROP CONSTRAINT ' + QUOTENAME(name) + N';'
    FROM sys.default_constraints
    WHERE parent_object_id = OBJECT_ID(N'Instructors')
    AND parent_column_id = (SELECT column_id FROM sys.columns WHERE object_id = OBJECT_ID(N'Instructors') AND name = 'EmployeeId');
    
    IF @sql2 IS NOT NULL AND LEN(@sql2) > 0 EXEC sp_executesql @sql2;

    ALTER TABLE Instructors DROP COLUMN EmployeeId;
END

-- 5b. Add ValidEmployeeIdId if missing
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Instructors') AND name = 'ValidEmployeeIdId')
BEGIN
    ALTER TABLE Instructors ADD ValidEmployeeIdId INT NOT NULL DEFAULT 1;
END

-- 6. Re-add FK constraint properly (Instructors.ValidEmployeeIdId -> ValidInstructorEmployeeIds.Id)
-- Ensure existing rows have valid ID
UPDATE Instructors SET ValidEmployeeIdId = 1 WHERE ValidEmployeeIdId = 0 OR ValidEmployeeIdId IS NULL;

ALTER TABLE Instructors 
ADD CONSTRAINT FK_Instructors_ValidInstructorEmployeeIds_ValidEmployeeIdId 
FOREIGN KEY (ValidEmployeeIdId) REFERENCES ValidInstructorEmployeeIds(Id)
ON DELETE CASCADE;

PRINT 'Database schema fixed successfully. Obsolete columns removed.';
