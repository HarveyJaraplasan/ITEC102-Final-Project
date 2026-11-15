# HARDIS: Hardware Recognition and Detection Identification System

**Group 1 · ITEC 102 – Fundamentals of Programming Final Project**  
Laguna State Polytechnic University – San Pablo City Campus, College of Computer Studies

## Project Description

HARDIS is a Windows console application that scans the computer it runs on and displays information about its processor, memory, storage, and graphics hardware. It groups the results by component, shows a quick system overview, and can save the full scan as a text report on the user's Desktop. The scan does not change hardware or Windows settings.

## Objectives

- Develop a C# console application that retrieves hardware information from a Windows computer.
- Identify the computer's CPU, installed RAM, physical disks, logical drives, and graphics adapters.
- Present detailed results and a short overview so users can find their computer specifications in one place.
- Allow users to save a timestamped report for future reference.
- Apply fundamental C# concepts such as variables, methods, conditionals, loops, and exception handling.

## Group Members

- Harvey O. Jaraplasan
- Dwane C. Garcia
- Aldrin S. Mendoza

## Programming Language and Platform

- **Programming language** — C#
- **Platform** — .NET console application for Windows
- **Windows APIs** — Windows Management Instrumentation (WMI) via `System.Management` for hardware information and Windows performance counters for disk activity

## System Features

| Feature | What HARDIS displays or does |
| --- | --- |
| CPU scan | Shows the processor model, core and logical thread counts, and current and maximum clock speeds. |
| RAM scan | Lists each memory module's capacity and speed, along with its manufacturer and part number when available. |
| Storage scan | Lists physical disks and logical volumes, with total capacity and available space for each ready volume. |
| Disk activity | Takes a one-second sample of overall disk read and write rates when Windows performance counters are available. |
| GPU scan | Lists detected graphics adapters and the video memory reported by Windows. |
| System overview | Summarizes CPU cores, threads, and speed, total RAM and speed, and physical, logical, and free storage. |
| Report export | Saves the overview and detailed scan to a timestamped `.txt` file on the Desktop when requested. |
| Optional sound | Plays console beeps during the scan when enabled at startup. |

## How to Compile and Run

HARDIS requires **Windows** because it uses WMI and Windows performance counters. Download or clone the repository first. If you download a ZIP, extract it before opening the project.

### If the repository includes a Visual Studio project

1. Open the included `.sln` or `.csproj` file in Visual Studio. Install the **.NET desktop development** workload or the required target framework if Visual Studio prompts you.
2. Select **Build → Build Solution** or press **Ctrl + Shift + B** to compile.
3. Press **Ctrl + F5** to run HARDIS without debugging.

### If the repository only includes `src/Program.cs`

Install the **.NET 10 SDK** on Windows. From the repository root, open PowerShell and run the following commands. They create a local console project and copy the source into it without changing the original file.

```powershell
dotnet new console --name Hardis_Proj --output .\Hardis_Proj
Copy-Item .\src\Program.cs .\Hardis_Proj\Program.cs -Force
Set-Location .\Hardis_Proj

dotnet package add System.Management
dotnet package add System.Diagnostics.PerformanceCounter

dotnet build
dotnet run --no-build
```

Run these setup commands once. To launch the project again, open PowerShell in the `Hardis_Proj` folder and run `dotnet run`.

## How to Use HARDIS

1. Type `Y` to enable sound or `N` to run silently, then press **Enter**.
2. Type `Y` and press **Enter** to start the scan, or type `N` to cancel.
3. Wait for HARDIS to scan the CPU, RAM, storage, and GPU. The **System Overview** appears after the detailed results.
4. Type `Y` to save the full report or `N` to skip saving. The file is named `HARDIS System Overview yyyyMMdd_HHmmss.txt` and is saved on the Desktop.
5. Press any key to exit.

If Windows cannot provide live disk activity, HARDIS displays a message and continues with the scan.
