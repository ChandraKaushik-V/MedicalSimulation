# Medical Simulation Platform

A full-stack .NET web application for medical simulation training with interactive 2D surgical procedure simulations.

## Features

- **User Authentication**: Secure login and registration using ASP.NET Identity
- **Multiple Medical Specialties**: 
  - Cardiology
  - Dermatology
  - Orthopedics
  - Neurosurgery
  - General Surgery
- **Interactive 2D Simulations**: Canvas-based surgical procedure practice
- **Progress Tracking**: Track scores, attempts, and completion status
- **User Dashboard**: View statistics and recent activity

## Technology Stack

- **Backend**: ASP.NET Core 8.0 (MVC + Web API)
- **Database**: Entity Framework Core with SQL Server
- **Authentication**: ASP.NET Identity
- **Frontend**: Razor Pages, HTML5 Canvas, CSS3, JavaScript
- **UI**: Custom responsive design with modern aesthetics

## Project Structure

```
MedicalSimulation/
├── MedicalSimulation.Core/          # Domain models and data access
│   ├── Models/                      # Entity models
│   └── Data/                        # DbContext and configurations
└── MedicalSimulation.Web/           # Web application
    ├── Controllers/                 # MVC controllers
    ├── Views/                       # Razor views
    ├── wwwroot/                     # Static files (CSS, JS)
    └── Areas/Identity/              # Authentication pages
```

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- SQL Server or SQL Server LocalDB
- Visual Studio 2022 or VS Code

### Installation

1. Clone the repository
2. Navigate to the project directory
3. Restore NuGet packages:
   ```bash
   dotnet restore
   ```

4. Update the database connection string in `appsettings.json` if needed

5. Apply database migrations:
   ```bash
   cd MedicalSimulation.Web
   dotnet ef database update
   ```

6. Run the application:
   ```bash
   dotnet run
   ```

7. Navigate to `https://localhost:5001` in your browser

## Database

The application uses Entity Framework Core with code-first migrations. The database includes:

- **AspNetUsers**: User accounts (via Identity)
- **Specialties**: Medical specialties
- **Simulations**: Surgical procedure simulations
- **UserProgress**: User attempt tracking and scores

Seed data is automatically populated on first run with 5 specialties and 7 sample simulations.

## Simulation Engine

The 2D simulation engine (`simulation-engine.js`) provides:

- Canvas-based interactive surgical procedures
- Click detection for target areas
- Tool selection and validation
- Real-time scoring
- Step-by-step guidance
- Progress persistence

## Future Enhancements

- 3D simulations using Three.js or Unity WebGL
- Multiplayer/collaborative simulations
- Advanced analytics and reporting
- Mobile app support
- Video tutorials and guided walkthroughs
- Certification and assessment modules

## License

This project is for educational purposes.
