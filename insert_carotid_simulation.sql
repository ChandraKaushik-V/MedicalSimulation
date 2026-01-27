-- Insert Missing Carotid Artery Stenting Simulation and Surgery States
-- Database: T
-- Run this in SSMS

USE [T];
GO

PRINT 'Starting to insert missing Carotid Artery Stenting simulation...';
GO

-- Step 1: Check if simulation ID 3 exists
IF EXISTS (SELECT 1 FROM Simulations WHERE Id = 3)
BEGIN
    PRINT 'Simulation ID 3 already exists. Deleting to re-insert...';
    DELETE FROM SurgeryStates WHERE SimulationId = 3;
    DELETE FROM Simulations WHERE Id = 3;
END
GO

-- Step 2: Insert Carotid Artery Stenting Simulation
PRINT 'Inserting Carotid Artery Stenting simulation...';

SET IDENTITY_INSERT Simulations ON;

INSERT INTO Simulations (
    Id, 
    Title, 
    Description, 
    SpecialtyId, 
    Difficulty, 
    EstimatedMinutes, 
    TotalStates, 
    ThumbnailUrl,
    IsActive,
    CreatedAt
)
VALUES (
    3,
    'Carotid Artery Stenting',
    'Learn the essential steps and techniques for performing carotid artery stenting to restore blood flow.',
    3, -- Cardiology
    3, -- Advanced
    10,
    6,
    '/pictures/cartoid.jpg',
    1, -- IsActive = true
    GETUTCDATE()
);

SET IDENTITY_INSERT Simulations OFF;
GO

PRINT 'Simulation inserted successfully.';
GO

-- Step 3: Insert Surgery States for Carotid Artery Stenting
PRINT 'Inserting surgery states for Carotid Artery Stenting...';

SET IDENTITY_INSERT SurgeryStates ON;

-- State 1: Arterial Access Vessel (Pause at 10s)
INSERT INTO SurgeryStates (
    Id, SimulationId, StateNumber, StateName, Description, VideoUrl,
    InteractionType, QuestionText, HotspotDataJson, AnswerOptionsJson,
    CorrectAnswerIndex, LayersJson
)
VALUES (
    13, 3, 1, 'Arterial Access Vessel',
    'Identify the vessel being accessed for arterial entry.',
    '/videos/simulations/Cartoid-artery-stenting.mp4',
    'mcq',
    'Which vessel is being accessed for arterial entry?',
    '{"pauseTime":10}',
    '["Radial artery","Radial vein","Ulnar artery","Cephalic vein"]',
    0,
    '[]'
);

-- State 2: Next Step After Guidewire (Pause at 15s)
INSERT INTO SurgeryStates (
    Id, SimulationId, StateNumber, StateName, Description, VideoUrl,
    InteractionType, QuestionText, HotspotDataJson, AnswerOptionsJson,
    CorrectAnswerIndex, LayersJson
)
VALUES (
    14, 3, 2, 'Next Step After Guidewire',
    'Determine the next step after successful arterial puncture and guidewire placement.',
    '/videos/simulations/Cartoid-artery-stenting.mp4',
    'mcq',
    'After successful arterial puncture and guidewire placement, what is the next step in carotid artery stenting?',
    '{"pauseTime":15}',
    '["Deploy the carotid stent","Insert an introducer sheath over the guidewire","Inflate the balloon to dilate the artery","Close the puncture site"]',
    1,
    '[]'
);

-- State 3: Purpose of Sheath Insertion (Pause at 28s)
INSERT INTO SurgeryStates (
    Id, SimulationId, StateNumber, StateName, Description, VideoUrl,
    InteractionType, QuestionText, HotspotDataJson, AnswerOptionsJson,
    CorrectAnswerIndex, LayersJson
)
VALUES (
    15, 3, 3, 'Purpose of Sheath Insertion',
    'Understand the primary purpose of inserting the sheath.',
    '/videos/simulations/Cartoid-artery-stenting.mp4',
    'mcq',
    'What is the primary purpose of inserting this sheath at this stage of the procedure?',
    '{"pauseTime":28}',
    '["To inject contrast directly into the brain","To maintain stable arterial access for catheter and device passage","To remove arterial plaque","To close the puncture site"]',
    1,
    '[]'
);

-- State 4: Contrast Dye Purpose (Pause at 45s)
INSERT INTO SurgeryStates (
    Id, SimulationId, StateNumber, StateName, Description, VideoUrl,
    InteractionType, QuestionText, HotspotDataJson, AnswerOptionsJson,
    CorrectAnswerIndex, LayersJson
)
VALUES (
    16, 3, 4, 'Contrast Dye Purpose',
    'Identify why contrast dye is injected at this stage.',
    '/videos/simulations/Cartoid-artery-stenting.mp4',
    'mcq',
    'Why is contrast dye injected at this stage of the carotid artery stenting procedure?',
    '{"pauseTime":45}',
    '["To dissolve the plaque","To visualize the carotid artery and identify the stenosis","To widen the artery","To anesthetize the vessel"]',
    1,
    '[]'
);

-- State 5: Stent Deployment Location (Pause at 55s)
INSERT INTO SurgeryStates (
    Id, SimulationId, StateNumber, StateName, Description, VideoUrl,
    InteractionType, QuestionText, HotspotDataJson, AnswerOptionsJson,
    CorrectAnswerIndex, LayersJson
)
VALUES (
    17, 3, 5, 'Stent Deployment Location',
    'Identify the correct location for stent deployment.',
    '/videos/simulations/Cartoid-artery-stenting.mp4',
    'mcq',
    'Where should the stent be deployed during a carotid artery stenting procedure?',
    '{"pauseTime":55}',
    '["In the subclavian artery","In the internal carotid artery at the site of stenosis","In the external carotid artery","In the aortic arch"]',
    1,
    '[]'
);

-- State 6: Purpose of Stent Deployment (Pause at 100s, end at 106s)
INSERT INTO SurgeryStates (
    Id, SimulationId, StateNumber, StateName, Description, VideoUrl,
    InteractionType, QuestionText, HotspotDataJson, AnswerOptionsJson,
    CorrectAnswerIndex, LayersJson
)
VALUES (
    18, 3, 6, 'Purpose of Stent Deployment',
    'Understand the primary purpose of deploying the stent.',
    '/videos/simulations/Cartoid-artery-stenting.mp4',
    'mcq',
    'What is the primary purpose of deploying the stent at this location?',
    '{"pauseTime":100,"endTime":106}',
    '["To dissolve the plaque chemically","To widen the narrowed artery and restore blood flow","To block blood flow to the brain","To remove the artery wall"]',
    1,
    '[]'
);

SET IDENTITY_INSERT SurgeryStates OFF;
GO

PRINT 'All surgery states inserted successfully.';
GO

-- Step 4: Verify the insertions
PRINT '';
PRINT '=== VERIFICATION ===';
PRINT '';

PRINT 'Carotid Artery Stenting Simulation:';
SELECT 
    Id,
    Title,
    SpecialtyId,
    Difficulty,
    EstimatedMinutes,
    TotalStates,
    IsActive
FROM Simulations 
WHERE Id = 3;
GO

PRINT '';
PRINT 'Surgery States for Carotid Artery Stenting:';
SELECT 
    Id,
    StateNumber,
    StateName,
    InteractionType,
    QuestionText
FROM SurgeryStates
WHERE SimulationId = 3
ORDER BY StateNumber;
GO

PRINT '';
PRINT 'All Simulations with Specialties:';
SELECT 
    s.Id,
    s.Title,
    s.SpecialtyId,
    sp.Name AS SpecialtyName,
    s.TotalStates,
    s.IsActive
FROM Simulations s
LEFT JOIN Specialties sp ON s.SpecialtyId = sp.Id
ORDER BY s.Id;
GO

PRINT '';
PRINT '✅ Carotid Artery Stenting simulation and all 6 surgery states inserted successfully!';
PRINT 'The simulation should now appear in the Cardiology section.';
GO
