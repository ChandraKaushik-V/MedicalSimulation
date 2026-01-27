-- Fix Carotid Artery Stenting Simulation Mapping to Cardiology
-- Run this script in SSMS to update the simulation specialty mapping

USE [T];
GO

-- Update Carotid Artery Stenting (ID 3) to map to Cardiology (SpecialtyId 3)
UPDATE Simulations 
SET SpecialtyId = 3 
WHERE Id = 3;
GO

-- Verify the update
SELECT Id, Title, SpecialtyId 
FROM Simulations 
WHERE Id = 3;
GO

-- Show all simulations with their specialties
SELECT 
    s.Id,
    s.Title,
    s.SpecialtyId,
    sp.Name AS SpecialtyName
FROM Simulations s
LEFT JOIN Specialties sp ON s.SpecialtyId = sp.Id
ORDER BY s.Id;
GO
