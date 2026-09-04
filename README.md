<img src="https://capsule-render.vercel.app/api?type=waving&color=0:2E86AB,100:A23B72&height=200&section=header&text=Grade%20Manager&fontSize=42&fontColor=ffffff&animation=fadeIn&fontAlignY=35&desc=Task%2C%20group%2C%20and%20grade%20management%20system&descAlignY=55&descSize=16" width="100%"/>

<div align="center">

![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-WPF-239120?logo=csharp&logoColor=white)
![MVVM](https://img.shields.io/badge/Pattern-MVVM-2E86AB)

</div>

## About the Project

Desktop application developed in **WPF (.NET 9.0)** for the **Software Planning and Development Lab** course, part of the **Bachelor's Degree in Computer Engineering** (UTAD, 2024/2025).

The application allows a professor to manage a class's evaluation process: student registration, organization into work groups, definition of evaluation tasks with their respective weights, grade entry, and consultation of the final gradebook, including graphical visualization of the results.

---

## Features

- **Profile Management** — editing the user's (professor's) name, email, and photo
- **Student Management (CRUD)** — number, name, and email
- **Student Import** from a `.csv` or `.xlsx` file
- **Evaluation Task Management (CRUD)** — title, description, start/end dates, and weight
- **Group Management (CRUD)** — assigning students to work groups
- **Grade Entry** per group (with the possibility of per-student exceptions)
- **Gradebook View** in matrix format, with the final grade calculated per student
- **Histogram** of the class's grade distribution

---

## Technologies Used

| Technology | Description |
|---|---|
| **C#** | Programming language |
| **WPF (.NET 9.0)** | GUI framework |
| **MVVM** | Architectural pattern (Model-View-ViewModel) |
| **XML / JSON** | Local data persistence |
| **Visual Studio 2022** | Development IDE |

---

## Architecture

The application follows the **MVVM** pattern, clearly separating:

- **Model** — domain entities (`User`, `Class`, `CurrentClass`, `Student`, `Group`, `Task`, `GradebookRow`)
- **View** — XAML interfaces
- **ViewModel** — presentation logic and binding between Model and View

Data is persisted in XML/JSON files, stored in the Windows user's personal profile folder.

---

## How to Run

1. Clone the repository
   ```bash
   git clone https://github.com/Tiago095/GestaoTurmas.git
   ```
2. Open the `.sln` solution in **Visual Studio 2022**
3. Restore the NuGet packages (automatic on open)
4. Set the WPF project as the *Startup Project*
5. Run (`F5`)

**Prerequisites:** .NET 9.0 SDK and Visual Studio 2022

<img src="https://capsule-render.vercel.app/api?type=waving&color=0:2E86AB,100:A23B72&height=100&section=footer&animation=fadeIn&reversal=true" width="100%"/>
