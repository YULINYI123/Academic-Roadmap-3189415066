open AcademicRoadmapCore

[<EntryPoint>]
let main _ =
    let roadmap = RoadmapEngine.demoRoadmap
    let gpa = RoadmapEngine.calculateGpa RoadmapEngine.demoGrades
    let events = RoadmapEngine.calendarEvents roadmap
    let recommendations = RoadmapEngine.recommendations roadmap

    printfn "Academic Roadmap F# core model"
    printfn "================================"

    RoadmapEngine.describe roadmap
    |> List.iter (printfn "%s")

    printfn ""
    printfn "GPA: %.2f (%s)" gpa.Gpa gpa.Standing
    printfn "Calendar events: %i" events.Length

    printfn ""
    printfn "Recommendations"
    printfn "---------------"

    recommendations
    |> List.iter (fun item -> printfn "%i. %s - %s" item.Priority item.Title item.Reason)

    0
