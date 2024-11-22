# EvoComms - Biometric Terminal Networking

> [!CAUTION]
> This project is currently in beta and should be used with caution.
> The current implementation lacks authentication and should not be used in production environments.

## Overview
EvoComms is a .NET 8.0 Blazor application that runs as a Windows service/process, 
designed to facilitate communication between various biometric clocking machines and time and attendance software packages sold by [Clocking Systems UK](https://www.clockingsystems.co.uk/). 

The application provides a web interface for configuration and management of the integration processes.

## Current State Of Project
This project has not had a sensible amount of time dedicated towards it, as a result a noticeable amount of technical debt exists in the codebase. 

This can be seen in the form of DRY Violations, SOLID Violations, and mind-boggling implementations even hallucinating AI couldn't spit out.

Refactoring will be an ongoing process. A strong effort will also be made to make the codebase beautiful before it's deemed production ready.

## Supported Clocking Software Products
Excluding InTime and EvoTime, the integration is done via outputting a file in CSV Format.
Feel free to use one of the CSV-Based integrations as a stepping stone to integrating it with any unsupported software products.
- EvoTime (Implementation not OS)
- InTime
- TotalTime / BioTime
- InfoTime

## Features
- Real-time communication with biometric clocking devices
- Data transformation and file generation for supported Software Packages
- Web-based configuration interface
- Runs as a Windows service, console app for debugging.
- Supports a variety of clocking machine manufacturers

## Solution Structure
```
EvoComms/
├── EvoComms.Core                    # Core functionality and EF models
├── EvoComms.Devices.HanvonVF       # Hanvon device integration
├── EvoComms.Devices.Timy           # Timy device integration
├── EvoComms.Devices.ZKTeco         # ZKTeco device integration
├── EvoComms.Logging               # Logging functionality
├── EvoComms.Web.App               # Blazor web application
└── EvoComms.Windows.Installer.App # WiX Toolset installer project
```

## Requirements
- Windows 7 or Later
- Supported clocking machine(s) & software
- Network connectivity between the host and clocking machine(s)

## Installation
1. Download the latest release
2. Run the Windows installer package
3. Access the web interface at `http://localhost:17856`
4. Configure your devices and integration settings

## Device Support
The application supports various manufacturer-specific integrations through dedicated device libraries:
- ZKTeco Devices
- Hanvon VF Series
- Timy Clocking Systems

Each device integration is modular and can be enabled/disabled as needed.

## Configuration
The web interface provides configuration options for:
- Device connections and communication settings
- Output file formats and locations
- Integration schedules
- System logging preferences
- Time & Attendance system-specific settings

## Development
### Prerequisites
- Visual Studio 2022 or later (Rider is BIS)
- .NET 8.0 SDK
- WiX Toolset for building the installer

### Building the Solution
1. Clone the repository
2. Open the solution in Visual Studio
3. Restore NuGet packages
4. Build the solution
5. Run the Web.App project for development

> [!WARNING]
> You may need to manually map some of the DLL's included as they have hard paths.

### Project Structure
- **EvoComms.Core**: Contains the core functionality, database context, and entity models
- **EvoComms.Devices.***: Device-specific integration libraries
- **EvoComms.Logging**: Logging specific implementations
- **EvoComms.Web.App**: Blazor web application
- **EvoComms.Windows.Installer.App**: WiX-based installer project

## Contributing
In the unlikely event you want to contribute, please follow the below:
1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request