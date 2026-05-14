# Academic Roadmap Generator 🎓

Academic Roadmap Generator is a minimalist, Notion-inspired web application designed to help students and lifelong learners bridge the gap between "wanting to learn" and "knowing how to learn." It generates structured, week-by-week learning paths for any subject and allows users to export these goals directly to their calendars.

## Motivation
Modern learners are often overwhelmed by the sheer volume of information available. The biggest hurdle is often the first step: organizing a logical sequence of study. This tool provides a clear starting point and actionable milestones to ensure consistent progress.

## Features
- **Instant Roadmap Generation**: Get a 4-week structured plan for any topic.
- **Progress Tracking**: Check off tasks as you go. Your progress is saved automatically.
- **Smart Templates**: Specialized paths for Coding, Mathematics, Art, Language, and History.
- **ICS Calendar Export**: One-click export to Google Calendar, Apple Calendar, or Outlook.
- **Premium UI/UX**: Clean, responsive design with glassmorphism and smooth animations.

## Try Live
https://YULINYI123.github.io/Academic-Roadmap-3189415066

## How to Build/Run
1. Clone the repository.
2. Open `index.html` in any modern web browser.
3. No build step required (Pure JavaScript/CSS).

## F# Implementation Notes
This project is deployed as a static, client-only web application, so it does not require a backend server. The repository also contains a separate F#/.NET 8 project under `fsharp-src/` so the academic planning logic is represented as real F# source code.

The F# implementation corresponds to the main features of the web app: roadmap templates, week-by-week study tasks, progress calculation, GPA reporting, recommendation generation, and calendar event export data.

To verify the F# project:
```bash
cd fsharp-src
dotnet build AcademicRoadmapCore.fsproj
dotnet run --project AcademicRoadmapCore.fsproj
```

## Submission Checklist
- **Public code host**: the project is available in the `YULINYI123/Academic-Roadmap-3189415066` GitHub repository.
- **Runnable web app**: the root-level HTML/CSS/JavaScript files provide the live roadmap generator.
- **Live access**: the Try Live link points to the GitHub Pages deployment.
- **Documentation**: this README explains the motivation, feature set, local usage, screenshot, and F# source project.
- **Application screenshot**: `screenshot.png` is included and displayed below.
- **Automatic publishing**: GitHub Actions is configured through `.github/workflows/deploy.yml`.
- **F# requirement**: `fsharp-src/` contains a compile-ready .NET 8 F# project with more than 300 lines of core planning logic.
- **Commit record**: the repository history shows separate commits for interface work, behavior, deployment, documentation, and the F# implementation.

## Screenshots
![Main Interface](screenshot.png)

## License
MIT
