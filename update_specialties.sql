-- Update existing specialty to Dermatology
UPDATE Specialties 
SET Name = 'Dermatology', 
    Description = 'Skin and related conditions, including surgical procedures',
    IconClass = 'fa-hand-holding-medical',
    Color = '#10b981',
    DisplayOrder = 1,
    IsActive = 1
WHERE Id = 1;

-- Insert Neurology specialty
IF NOT EXISTS (SELECT 1 FROM Specialties WHERE Id = 2)
BEGIN
    SET IDENTITY_INSERT Specialties ON;
    INSERT INTO Specialties (Id, Name, Description, IconClass, Color, DisplayOrder, IsActive)
    VALUES (2, 'Neurology', 'Brain, spinal cord, and nervous system procedures', 'fa-brain', '#8b5cf6', 2, 1);
    SET IDENTITY_INSERT Specialties OFF;
END
ELSE
BEGIN
    UPDATE Specialties 
    SET Name = 'Neurology', 
        Description = 'Brain, spinal cord, and nervous system procedures',
        IconClass = 'fa-brain',
        Color = '#8b5cf6',
        DisplayOrder = 2,
        IsActive = 1
    WHERE Id = 2;
END

-- Insert Cardiology specialty
IF NOT EXISTS (SELECT 1 FROM Specialties WHERE Id = 3)
BEGIN
    SET IDENTITY_INSERT Specialties ON;
    INSERT INTO Specialties (Id, Name, Description, IconClass, Color, DisplayOrder, IsActive)
    VALUES (3, 'Cardiology', 'Heart and cardiovascular system procedures', 'fa-heart-pulse', '#ef4444', 3, 1);
    SET IDENTITY_INSERT Specialties OFF;
END
ELSE
BEGIN
    UPDATE Specialties 
    SET Name = 'Cardiology', 
        Description = 'Heart and cardiovascular system procedures',
        IconClass = 'fa-heart-pulse',
        Color = '#ef4444',
        DisplayOrder = 3,
        IsActive = 1
    WHERE Id = 3;
END

-- Update simulation specialty mappings
UPDATE Simulations SET SpecialtyId = 1 WHERE Id = 1; -- Subcuticular Running Suture -> Dermatology
UPDATE Simulations SET SpecialtyId = 2 WHERE Id = 2; -- Posterior Neck Craniectomy -> Neurology

PRINT 'Specialties reorganized successfully!';
