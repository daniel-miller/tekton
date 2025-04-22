using Microsoft.AspNetCore.Mvc;

namespace Tek.Integration.Scorm.Api;

[ApiController]
public class ScormController(ReleaseSettings releaseSettings, IMonitor monitor) : ControllerBase
{
    private readonly ReleaseSettings _releaseSettings = releaseSettings;
    private readonly IMonitor _monitor = monitor;

    private ScormApi BuildClient()
    {
        var userName = (string)HttpContext.Items["AuthenticatedUserName"]!;
        var password = (string)HttpContext.Items["AuthenticatedPassword"]!;

        return new ScormApi(userName, password);
    }

    [HttpPost("registrations")]
    public IActionResult CreateRegistration(RegistrationRequest request)
    {
        try
        {
            var scorm = BuildClient();
            scorm.CreateRegistration(request.RegistrationId, request.CourseSlug, request.LearnerId, request.LearnerEmail, request.LearnerFirstName, request.LearnerLastName);
            return NoContent();
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("already exists under appid"))
                return NoContent();

            var error = $"An unexpected error occurred trying to create SCORM Cloud registration {request.RegistrationId} for learner {request.LearnerId} ({request.LearnerEmail}). {ex.Message}";
            _monitor.Error(error);
            return Problem(error);
        }
    }

    [HttpGet("registrations")]
    public ActionResult<Registration[]> GetRegistrations([FromQuery] string? course = null, [FromQuery] string? more = null)
    {
        var scorm = BuildClient();
        var result = scorm.GetRegistrations(course, more);
        Response.Headers.Append("X-Engine-Scorm-More", result.More);
        return Ok(result.Registrations);
    }

    [HttpGet("registrations/{registrationId}/last-instance")]
    public ActionResult<int?> GetRegistrationInstance(Guid registrationId)
    {
        var scorm = BuildClient();
        var lastInstance = scorm.GetRegistrationInstance(registrationId);
        return Ok(lastInstance);
    }

    [HttpGet("registrations/{registrationId}/launch-url")]
    public ActionResult<string> GetRegistrationLaunchUrl(Guid registrationId, string courseSlug, bool preview, string callbackUrl, string exitUrl)
    {
        var scorm = BuildClient();
        var link = scorm.GetRegistrationLaunchUrl(registrationId, courseSlug, preview, callbackUrl, exitUrl);
        return Ok(link);
    }

    [HttpGet("registrations/{registrationId}/progress")]
    public ActionResult<RegistrationProgress> GetRegistrationInstanceProgress(Guid registrationId)
    {
        var scorm = BuildClient();
        var instance = scorm.GetRegistrationInstance(registrationId);
        var progress = scorm.GetRegistrationInstanceProgress(registrationId, instance);
        return Ok(progress);
    }

    [HttpGet("courses/{courseSlug}")]
    public ActionResult<Course> GetCourse(string courseSlug)
    {
        var scorm = BuildClient();
        var course = scorm.GetCourse(courseSlug);
        return course != null
            ? Ok(course)
            : NotFound();
    }

    [HttpGet("courses")]
    public ActionResult<Course[]> GetCourses()
    {
        var scorm = BuildClient();
        var courses = scorm.GetCourses();
        return Ok(courses);
    }

    [HttpGet("courses/{courseSlug}/learners/{learnerId}/registration")]
    public ActionResult<string> GetRegistrationId(string courseSlug, Guid learnerId)
    {
        var scorm = BuildClient();
        var registrationId = scorm.GetRegistrationId(courseSlug, learnerId);
        return Ok(registrationId);
    }

    [HttpPost("imports")]
    public async Task<ActionResult<string>> CreateImport(string courseSlug, bool mayCreateNewVersion, string callbackUrl, string uploadedContentType, string contentMetadata)
    {
        using var stream = new MemoryStream();
        await Request.Body.CopyToAsync(stream);
        var scorm = BuildClient();
        var importId = scorm.CreateImport(courseSlug, mayCreateNewVersion, callbackUrl, uploadedContentType, contentMetadata, stream);
        return Ok(importId);
    }

    [HttpGet("imports/{importSlug}")]
    public ActionResult<CourseImport> GetImportStatus(string importSlug)
    {
        var scorm = BuildClient();
        var status = scorm.GetImportStatus(importSlug);
        return Ok(status);
    }

    [HttpGet]
    [Route("error")]
    public ActionResult<string> Error()
    {
        try
        {
            throw new Exception("Error! This is a test exception.");
        }
        catch (Exception ex)
        {
            _monitor.Error(ex.Message);
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("status")]
    public IActionResult GetStatus()
    {
        var version = typeof(ScormController).Assembly.GetName().Version;

        var status = $"Engine SCORM API version {version} is online. The {_releaseSettings.Environment} environment says hello.";

        return Ok(status);
    }

    [HttpGet("version")]
    public IActionResult GetVersion()
    {
        var version = typeof(ScormController).Assembly.GetName().Version?.ToString() ?? "0.0.0.0";

        return Ok(version);
    }
}