namespace AcademicRoadmapCore

open System
open System.Text

module RoadmapEngine =
    let private topicForWeek (request: RoadmapRequest) weekNumber =
        let topics = Track.defaultTopics request.Track
        let index = (weekNumber - 1) % topics.Length
        if String.IsNullOrWhiteSpace request.Topic then topics[index]
        else $"{request.Topic}: {topics[index]}"

    let private hoursForTask (request: RoadmapRequest) weight =
        request.HoursPerWeek * Difficulty.workloadMultiplier request.Difficulty * weight
        |> Decimal.round2

    let private task id title description hours format resource =
        { Id = id
          Title = title
          Description = description
          EstimatedHours = hours
          Format = format
          Completed = false
          Resource = resource }

    let private weekTheme (request: RoadmapRequest) weekNumber =
        match weekNumber with
        | 1 -> $"Map the foundations of {topicForWeek request weekNumber}"
        | 2 -> $"Practice core skills for {topicForWeek request weekNumber}"
        | 3 -> $"Build a small project with {topicForWeek request weekNumber}"
        | 4 -> $"Review, assess, and publish learning evidence"
        | n when n % 4 = 1 -> $"Extend foundations: {topicForWeek request n}"
        | n when n % 4 = 2 -> $"Deep practice: {topicForWeek request n}"
        | n when n % 4 = 3 -> $"Applied project: {topicForWeek request n}"
        | n -> $"Reflection and consolidation: week {n}"

    let private objective (request: RoadmapRequest) weekNumber =
        let difficulty = Difficulty.label request.Difficulty
        $"{difficulty} objective: complete a measurable learning loop for {topicForWeek request weekNumber}."

    let private milestone (_request: RoadmapRequest) weekNumber =
        match weekNumber % 4 with
        | 1 -> "Create a concept map and define success criteria."
        | 2 -> "Finish guided exercises and record mistakes."
        | 3 -> "Complete a small applied artifact."
        | _ -> "Review progress and update the next learning plan."

    let generateWeek (request: RoadmapRequest) weekNumber =
        let baseId = weekNumber * 100
        let topic = topicForWeek request weekNumber
        let resource =
            match request.Track with
            | ComputerScience -> Some "Official documentation and coding exercises"
            | DataScience -> Some "Dataset notebook and statistics reference"
            | BusinessAnalytics -> Some "Dashboard examples and metric dictionary"
            | LanguageLearning -> Some "Vocabulary deck and speaking prompts"
            | Mathematics -> Some "Problem set and proof notes"
            | CustomTrack _ -> Some "Curated learner resources"

        { WeekNumber = weekNumber
          Theme = weekTheme request weekNumber
          Objective = objective request weekNumber
          Milestone = milestone request weekNumber
          Tasks =
            [ task (baseId + 1) $"Read and outline {topic}" "Summarize the key ideas in your own words." (hoursForTask request 0.25m) Reading resource
              task (baseId + 2) $"Practice {topic}" "Complete focused exercises and mark difficult items." (hoursForTask request 0.35m) Practice None
              task (baseId + 3) $"Apply {topic}" "Create a small artifact that proves the concept works." (hoursForTask request 0.30m) Project None
              task (baseId + 4) $"Review {topic}" "Write a short reflection and next-step checklist." (hoursForTask request 0.10m) Review None ] }

    let generateRoadmap (request: RoadmapRequest) =
        let weeks = Math.Max(1, request.Weeks)
        { Request = { request with Weeks = weeks }
          Weeks = [ 1 .. weeks ] |> List.map (generateWeek request) }

    let allTasks (roadmap: Roadmap) =
        roadmap.Weeks |> List.collect _.Tasks

    let totalHours (roadmap: Roadmap) =
        allTasks roadmap
        |> List.sumBy _.EstimatedHours
        |> Decimal.round2

    let completedHours (roadmap: Roadmap) =
        allTasks roadmap
        |> List.filter _.Completed
        |> List.sumBy _.EstimatedHours
        |> Decimal.round2

    let completionRate (roadmap: Roadmap) =
        Decimal.percent (completedHours roadmap) (totalHours roadmap)

    let progress (roadmap: Roadmap) =
        let tasks = allTasks roadmap
        let completed = tasks |> List.filter _.Completed
        let currentWeek =
            roadmap.Weeks
            |> List.tryFind (fun week -> week.Tasks |> List.exists (fun t -> not t.Completed))
            |> Option.map _.WeekNumber

        { CompletedTasks = completed.Length
          TotalTasks = tasks.Length
          CompletedHours = completed |> List.sumBy _.EstimatedHours |> Decimal.round2
          TotalHours = tasks |> List.sumBy _.EstimatedHours |> Decimal.round2
          CompletionRate = completionRate roadmap
          CurrentWeek = currentWeek }

    let markTaskComplete taskId (roadmap: Roadmap) =
        let updateTask task =
            if task.Id = taskId then { task with Completed = true } else task

        let updateWeek week =
            { week with Tasks = week.Tasks |> List.map updateTask }

        { roadmap with Weeks = roadmap.Weeks |> List.map updateWeek }

    let replaceTask taskId newTitle newDescription (roadmap: Roadmap) =
        let updateTask task =
            if task.Id = taskId then
                { task with Title = newTitle; Description = newDescription }
            else task

        { roadmap with
            Weeks =
                roadmap.Weeks
                |> List.map (fun week -> { week with Tasks = week.Tasks |> List.map updateTask }) }

    let removeTask taskId (roadmap: Roadmap) =
        { roadmap with
            Weeks =
                roadmap.Weeks
                |> List.map (fun week -> { week with Tasks = week.Tasks |> List.filter (fun task -> task.Id <> taskId) }) }

    let addTask weekNumber (newTask: StudyTask) (roadmap: Roadmap) =
        { roadmap with
            Weeks =
                roadmap.Weeks
                |> List.map (fun week ->
                    if week.WeekNumber = weekNumber then { week with Tasks = week.Tasks @ [ newTask ] }
                    else week) }

    let calculateGpa (grades: CourseGrade list) =
        let attempted = grades |> List.sumBy _.Credits
        let weighted =
            grades
            |> List.sumBy (fun grade -> grade.Credits * grade.GradePoint)
            |> Decimal.round2

        let gpa =
            if attempted = 0.0m then 0.0m
            else weighted / attempted |> Decimal.round2

        let standing =
            if gpa >= 3.7m then "Excellent"
            elif gpa >= 3.3m then "Strong"
            elif gpa >= 2.7m then "Good"
            elif gpa >= 2.0m then "Needs support"
            else "At risk"

        { AttemptedCredits = attempted
          WeightedPoints = weighted
          Gpa = gpa
          Standing = standing }

    let grade course credits letter =
        { Course = course
          Credits = credits
          Letter = letter
          GradePoint = Grade.gradePoint letter }

    let calendarEvents (roadmap: Roadmap) =
        roadmap.Weeks
        |> List.collect (fun week ->
            let weekStart = roadmap.Request.StartDate.AddDays(float ((week.WeekNumber - 1) * 7))
            week.Tasks
            |> List.mapi (fun index task ->
                let startDate = weekStart.AddDays(float index)
                { Uid = $"{roadmap.Request.Topic}-{task.Id}@academic-roadmap"
                  Title = task.Title
                  Description = task.Description
                  StartDate = startDate
                  EndDate = startDate.AddHours(float task.EstimatedHours) }))

    let private formatDate (date: DateTime) =
        date.ToUniversalTime().ToString("yyyyMMdd'T'HHmmss'Z'")

    let toIcs events =
        let builder = StringBuilder()
        builder.AppendLine("BEGIN:VCALENDAR") |> ignore
        builder.AppendLine("VERSION:2.0") |> ignore
        builder.AppendLine("PRODID:-//Academic Roadmap Generator//FSharp Core//EN") |> ignore

        for event in events do
            builder.AppendLine("BEGIN:VEVENT") |> ignore
            builder.AppendLine($"UID:{event.Uid}") |> ignore
            builder.AppendLine($"SUMMARY:{event.Title}") |> ignore
            builder.AppendLine($"DESCRIPTION:{event.Description}") |> ignore
            builder.AppendLine($"DTSTART:{formatDate event.StartDate}") |> ignore
            builder.AppendLine($"DTEND:{formatDate event.EndDate}") |> ignore
            builder.AppendLine("END:VEVENT") |> ignore

        builder.AppendLine("END:VCALENDAR") |> ignore
        builder.ToString()

    let recommendations roadmap =
        let report = progress roadmap
        let tasks = allTasks roadmap
        let projectTasks = tasks |> List.filter (fun t -> t.Format = Project)
        let reviewTasks = tasks |> List.filter (fun t -> t.Format = Review)

        [ if report.CompletionRate < 50.0m then
              { Title = "Reduce scope"
                Reason = "Less than half of the planned work is complete; focus on the smallest useful milestone."
                Priority = 1 }
          if projectTasks.Length < roadmap.Weeks.Length then
              { Title = "Add more evidence"
                Reason = "Each week should produce at least one visible artifact or mini project."
                Priority = 2 }
          if reviewTasks.IsEmpty then
              { Title = "Schedule reflection"
                Reason = "Reflection tasks help convert practice into reusable knowledge."
                Priority = 3 }
          if totalHours roadmap > roadmap.Request.HoursPerWeek * decimal roadmap.Request.Weeks * 1.2m then
              { Title = "Balance workload"
                Reason = "The estimated workload is higher than the weekly target."
                Priority = 2 } ]
        |> List.sortBy _.Priority

    let describe roadmap =
        let report = progress roadmap
        [ $"Track: {Track.displayName roadmap.Request.Track}"
          $"Topic: {roadmap.Request.Topic}"
          $"Difficulty: {Difficulty.label roadmap.Request.Difficulty}"
          $"Weeks: {roadmap.Weeks.Length}"
          $"Tasks: {report.CompletedTasks}/{report.TotalTasks}"
          $"Hours: {report.CompletedHours}/{report.TotalHours}"
          $"Completion: {report.CompletionRate}%%" ]

    let demoRequest =
        { Topic = "Frontend Engineering"
          Track = ComputerScience
          Difficulty = Intermediate
          StartDate = DateTime(2026, 5, 18, 9, 0, 0)
          Weeks = 4
          HoursPerWeek = 8.0m }

    let demoRoadmap =
        demoRequest
        |> generateRoadmap
        |> markTaskComplete 101
        |> markTaskComplete 102
        |> markTaskComplete 201

    let demoGrades =
        [ grade "Programming" 3.0m "A-"
          grade "Statistics" 3.0m "B+"
          grade "Writing" 2.0m "A" ]
