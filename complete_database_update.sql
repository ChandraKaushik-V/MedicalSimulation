-- Complete Database Update Script
-- This script ensures all migrations are applied and fixes the Cardiology simulation mapping
-- Database: T
-- Run this in SSMS

USE [T];
GO

PRINT 'Starting database update...';
GO

-- Step 1: Verify migrations are applied
-- Check if __EFMigrationsHistory table exists
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = '__EFMigrationsHistory')
BEGIN
    PRINT 'ERROR: __EFMigrationsHistory table not found. Please run: dotnet ef database update';
    RETURN;
END
GO

-- Step 2: Check which migrations are applied
PRINT 'Currently applied migrations:';
SELECT MigrationId, ProductVersion FROM __EFMigrationsHistory ORDER BY MigrationId;
GO

-- Step 3: Fix Carotid Artery Stenting simulation mapping to Cardiology
PRINT 'Updating Carotid Artery Stenting simulation to Cardiology specialty...';

UPDATE Simulations 
SET SpecialtyId = 3,
    IsActive = 1
WHERE Id = 3;
GO

-- Step 4: Verify the update
PRINT 'Verification - Carotid Artery Stenting simulation:';
SELECT 
    Id,
    Title,
    SpecialtyId,
    IsActive,
    Difficulty,
    EstimatedMinutes,
    TotalStates
FROM Simulations 
WHERE Id = 3;
GO

-- Step 5: Show all simulations with their specialties
PRINT 'All simulations with specialties:';
SELECT 
    s.Id,
    s.Title,
    s.SpecialtyId,
    sp.Name AS SpecialtyName,
    s.IsActive,
    s.Difficulty,
    s.EstimatedMinutes
FROM Simulations s
LEFT JOIN Specialties sp ON s.SpecialtyId = sp.Id
ORDER BY s.SpecialtyId, s.Id;
GO

-- Step 6: Verify Cardiology specialty exists
PRINT 'Cardiology specialty details:';
SELECT 
    Id,
    Name,
    Description,
    IsActive,
    DisplayOrder
FROM Specialties
WHERE Id = 3;
GO

PRINT 'Database update completed successfully!';
PRINT 'The Carotid Artery Stenting simulation should now appear in the Cardiology section.';
GO
