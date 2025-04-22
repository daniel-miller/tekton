namespace Tek.Integration.Scorm.Api;

public class Course
{
    public string Id { get; set; } = null!;
    public string Title { get; set; } = null!;
    public DateTime? Created { get; set; }
    public DateTime? Updated { get; set; }
    public int? Version { get; set; }
    public int? RegistrationCount { get; set; }
    public string? CourseLearningStandard { get; set; } = null!;
}

public class CourseImport
{
    public string CourseId { get; set; } = null!;
    public string Message { get; set; } = null!;

    public bool IsComplete { get; set; }
    public bool IsError { get; set; }
    public bool IsRunning { get; set; }
}

public class Registration
{
    public string Id { get; set; } = null!;
    public int? Instance { get; set; }
    public DateTime? Updated { get; set; }
    public string RegistrationCompletion { get; set; } = null!;
    public string RegistrationSuccess { get; set; } = null!;
    public double? TotalSecondsTracked { get; set; }
    public DateTime? FirstAccessDate { get; set; }
    public DateTime? LastAccessDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string CourseId { get; set; } = null!;
    public string CourseTitle { get; set; } = null!;
    public int? CourseVersion { get; set; }
    public string LearnerId { get; set; } = null!;
    public string LearnerEmail { get; set; } = null!;
    public string LearnerFirstName { get; set; } = null!;
    public string LearnerLastName { get; set; } = null!;
}

public class RegistrationProgress
{
    public DateTime? CompletedDate { get; set; }
    public DateTime? FirstAccessDate { get; set; }
    public DateTime? LastAccessDate { get; set; }
    public string RegistrationCompletion { get; set; } = null!;
    public string RegistrationSuccess { get; set; } = null!;
    public decimal? ScoreScaled { get; set; }
    public int? TotalSecondsTracked { get; set; }
}

public class RegistrationRequest
{
    public Guid RegistrationId { get; set; }
    public string CourseSlug { get; set; } = null!;
    public Guid LearnerId { get; set; }
    public string LearnerEmail { get; set; } = null!;
    public string LearnerFirstName { get; set; } = null!;
    public string LearnerLastName { get; set; } = null!;
}