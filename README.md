# Local Authority Districts App

## Overview

**LocalAuthorityDistrictsApp** is a web application that visualizes geographical data of Local Authority Districts in England. It provides an interactive map interface for users to explore, filter, and manage geographical data.

## Features

- Displays all Local Authority Districts on an interactive map upon initial load.
- Allows users to hide all displayed data.
- Enables users to filter and display specific areas by name.
- Built with Clean Architecture principles, ensuring separation of concerns.

## Tech Stack

### Frontend
- **Blazor Server** for interactive UI development.
- **Mapbox** for GIS & Mapping.

### Backend
- **.NET 8 (C#)** for service methods and data processing.
- **GeoJSON Data File** stored in the `Infrastructure/Data` project for district data (instead of a database).

## Project Structure

The project follows a Clean Architecture approach and is structured as follows:

```
LocalAuthorityDistrictsApp
│── LocalAuthorityDistricts.Application         # Business logic & use cases
│── LocalAuthorityDistricts.Domain              # Entities & domain logic
│── LocalAuthorityDistricts.Infrastructure      # Data storage (GeoJSON file)
│   ├── Data
│   │   ├── local-authority-district.geojson    # GeoJSON data file
│── LocalAuthorityDistricts.Presentation.BlazorServer  # Blazor frontend & service methods
│── Tests
│   ├── LocalAuthorityDistricts.Application.Tests
│   ├── LocalAuthorityDistricts.Infrastructure.Tests
│   ├── LocalAuthorityDistricts.Presentation.Tests
```

## Setup Instructions

### Prerequisites
- **.NET 8 SDK**
- **Git**
- **Mapbox API key** (already configured in the project)

### Installation & Running the Project

#### Clone the Repository
```sh
git clone https://github.com/bilalhassan88/LocalAuthorityDistrictsApp.git
cd LocalAuthorityDistrictsApp
```

#### Open the Solution in Visual Studio
- Open `LocalAuthorityDistrictsApp.sln` in **Visual Studio**.
- Ensure that `LocalAuthorityDistricts.Presentation.BlazorServer` is set as the startup project.

#### Run the Application
- Press **F5** in Visual Studio to start debugging.
- The application will launch in your default web browser.

#### Access the Application
Open a browser and navigate to:  
`http://localhost:5013` (or the specified port in Visual Studio).

## Testing

### Backend
Unit tests are implemented using **xUnit**. Run tests with:
```sh
dotnet test
```

### Frontend
Blazor frontend tests use **bUnit**, but only minimal frontend tests have been implemented. Run tests with:
```sh
dotnet test LocalAuthorityDistricts.Presentation.Tests
```

## Development Workflow

### Feature Development
Create a new branch from `main`:
```sh
git checkout -b feature-branch
```

### Commit Changes
```sh
git add .
git commit -m "Implemented feature X"
```

### Push Branch
```sh
git push origin feature-branch
```

### Pull Request
Create a **Pull Request (PR)** to merge into `master`.

## Time Spent
Approximate time spent on this assignment: **24 hours**.

## License
This project is open-source and available under the **MIT License**.

## Contact
For any questions, feel free to reach out via **GitHub Issues** or email.
