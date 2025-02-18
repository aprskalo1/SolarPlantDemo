# Solar Power Plant Monitoring API

## Overview

This project is a mid/senior level .NET developer task aimed at developing a robust REST API for monitoring and managing solar power plants. The API provides functionalities for:

- **User Management:**
  - **Register and Authenticate:** Users can create an account, log in, and receive a JWT token for secure API access.
  
- **Solar Power Plant Management (CRUD):**
  - **Attributes:**
    - **Solar Power Plant Name:** (Optional)
    - **Installed Power:** (Mandatory)
    - **Date of Installation:** (Mandatory)
    - **Location:** Longitude and Latitude (Mandatory)
  
- **Production Data Retrieval:**
  - **Data Types:**
    - Real production data
    - Forecasted production data
  - **Data Granularity:**
    - 15 minutes (default)
    - 1 hour (aggregated by summing four 15-minute records)
  - **Time Span:** User-specified period for which timeseries data is returned

## Requirements

- **Framework:** ASP.NET Core 8.0
- **Database:** MSSQL (using a Code-First approach with Entity Framework Core)
- **Authentication:** JWT-based authentication and authorization
- **Logging:** Configurable logging to text files
- **Seed Data:** A function to generate historical production data for testing purposes (solar power plant data and associated timeseries are randomly generated)
- **Forecasting:** Uses the installed power attribute along with weather data from a free online weather API to simulate production forecasting (accuracy is not critical; the goal is to demonstrate data integration)

## Prerequisites

Before starting, ensure you have the following:

- An MSSQL Server instance running.
- .NET SDK (version 8.0) installed on your development machine.

## Setup Instructions

### 1. Adjust Connection Strings

Update the connection strings in the `appsettings.json` file to match your MSSQL Server configuration.

### 2. Create Databases and Tables

Go to the root of the project and  execute the following command:

```bash
dotnet ef database update
```
Now you can start the application and check created enpoints.

## Note

Upon start if database is empty application will seed the database with 3 solar plants and each of them will have mock production data recorded in specific 15 minute interval.

## Logging
Logs will be stored in C:\Logs by default, log directory can be changed in appsettings.json configuration file.

## Technologies Used
- **C#**: Language.
- **Entity Framework Core**: Code-first approach for database management.
- **MSSQL**: For database operations.
- **OpenAPI/OData**: For API definition and documentation.
