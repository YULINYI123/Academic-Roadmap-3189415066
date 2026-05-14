namespace AcademicRoadmapCore

open System

type DegreeTrack =
    | ComputerScience
    | DataScience
    | BusinessAnalytics
    | LanguageLearning
    | Mathematics
    | CustomTrack of string

type Difficulty =
    | Foundation
    | Intermediate
    | Advanced

type StudyFormat =
    | Reading
    | Practice
    | Project
    | Review
    | Assessment

type StudyTask =
    { Id: int
      Title: string
      Description: string
      EstimatedHours: decimal
      Format: StudyFormat
      Completed: bool
      Resource: string option }

type StudyWeek =
    { WeekNumber: int
      Theme: string
      Objective: string
      Tasks: StudyTask list
      Milestone: string }

type RoadmapRequest =
    { Topic: string
      Track: DegreeTrack
      Difficulty: Difficulty
      StartDate: DateTime
      Weeks: int
      HoursPerWeek: decimal }

type Roadmap =
    { Request: RoadmapRequest
      Weeks: StudyWeek list }

type CourseGrade =
    { Course: string
      Credits: decimal
      Letter: string
      GradePoint: decimal }

type GpaReport =
    { AttemptedCredits: decimal
      WeightedPoints: decimal
      Gpa: decimal
      Standing: string }

type CalendarEvent =
    { Uid: string
      Title: string
      Description: string
      StartDate: DateTime
      EndDate: DateTime }

type ProgressReport =
    { CompletedTasks: int
      TotalTasks: int
      CompletedHours: decimal
      TotalHours: decimal
      CompletionRate: decimal
      CurrentWeek: int option }

type Recommendation =
    { Title: string
      Reason: string
      Priority: int }

module Track =
    let displayName track =
        match track with
        | ComputerScience -> "Computer Science"
        | DataScience -> "Data Science"
        | BusinessAnalytics -> "Business Analytics"
        | LanguageLearning -> "Language Learning"
        | Mathematics -> "Mathematics"
        | CustomTrack value -> value

    let defaultTopics track =
        match track with
        | ComputerScience -> [ "programming fundamentals"; "data structures"; "web development"; "software design" ]
        | DataScience -> [ "statistics"; "python analysis"; "machine learning"; "data visualization" ]
        | BusinessAnalytics -> [ "business metrics"; "dashboard design"; "forecasting"; "decision analysis" ]
        | LanguageLearning -> [ "pronunciation"; "vocabulary"; "grammar"; "conversation" ]
        | Mathematics -> [ "proofs"; "algebra"; "calculus"; "problem solving" ]
        | CustomTrack value -> [ value; "guided practice"; "project work"; "review" ]

module Difficulty =
    let label difficulty =
        match difficulty with
        | Foundation -> "Foundation"
        | Intermediate -> "Intermediate"
        | Advanced -> "Advanced"

    let workloadMultiplier difficulty =
        match difficulty with
        | Foundation -> 0.85m
        | Intermediate -> 1.0m
        | Advanced -> 1.25m

module StudyFormat =
    let label format =
        match format with
        | Reading -> "Reading"
        | Practice -> "Practice"
        | Project -> "Project"
        | Review -> "Review"
        | Assessment -> "Assessment"

module Grade =
    let gradePoint (letter: string) =
        match letter.Trim().ToUpperInvariant() with
        | "A+" -> 4.0m
        | "A" -> 4.0m
        | "A-" -> 3.7m
        | "B+" -> 3.3m
        | "B" -> 3.0m
        | "B-" -> 2.7m
        | "C+" -> 2.3m
        | "C" -> 2.0m
        | "C-" -> 1.7m
        | "D+" -> 1.3m
        | "D" -> 1.0m
        | _ -> 0.0m

module Decimal =
    let round2 (value: decimal) =
        Math.Round(value, 2, MidpointRounding.AwayFromZero)

    let percent (part: decimal) (total: decimal) =
        if total = 0.0m then 0.0m
        else round2 ((part / total) * 100.0m)
