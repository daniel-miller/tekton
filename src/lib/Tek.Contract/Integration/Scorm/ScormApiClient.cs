using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Tek.Base;

namespace Tek.Contract.Integration.Scorm
{
    public interface IScormApiClient
    {
        // Registrations
        Task CreateRegistration(RegistrationRequest request);
        Task<int?> GetRegistrationInstance(Guid registrationId);
        Task<string> GetRegistrationLaunchUrl(Guid registrationId, string courseSlug, bool preview, string callbackUrl, string exitUrl);
        Task<RegistrationProgress> GetRegistrationInstanceProgress(Guid registrationId, int? instance);

        // Courses
        Task<Course> GetCourse(string courseSlug);
        Task<Course[]> GetCourses();
        Task<string> GetRegistrationId(string courseSlug, Guid learnerId);

        // Imports
        Task<string> CreateImport(string courseSlug, bool mayCreateNewVersion, string callbackUrl, string uploadedContentType, string contentMetadata, Stream stream);
        Task<CourseImport> GetImportStatus(string importSlug);
    }

    public class ScormApiClient : IScormApiClient
    {
        private readonly IHttpClientFactory _factory;
        private readonly IJsonSerializer _serializer;

        public ScormApiClient(IHttpClientFactory factory, IJsonSerializer serializer)
        {
            _factory = factory;
            _serializer = serializer;
        }

        public async Task CreateRegistration(RegistrationRequest request)
        {
            using (var client = _factory.Create())
            {
                var api = new ApiClient(_factory, _serializer);

                await api.HttpPost("registrations", request);
            }
        }

        public async Task<Course> GetCourse(string courseSlug)
        {
            using (var client = _factory.Create())
            {
                var api = new ApiClient(_factory, _serializer);

                var result = await api.HttpGet<Course>($"courses/{courseSlug}");

                return result.Data;
            }
        }

        public async Task<Course[]> GetCourses()
        {
            using (var client = _factory.Create())
            {
                var api = new ApiClient(_factory, _serializer);

                var result = await api.HttpGet<Course[]>($"courses");

                return result.Data;
            }
        }

        public async Task<int?> GetRegistrationInstance(Guid registrationId)
        {
            using (var client = _factory.Create())
            {
                var api = new ApiClient(_factory, _serializer);

                var result = await api.HttpGet<string>($"registrations/{registrationId}/last-instance");

                var instance = result.Data;

                if (int.TryParse(instance, out var n))
                    return n;

                return null;
            }
        }

        public async Task<string> GetRegistrationLaunchUrl(Guid registrationId, string courseSlug, bool preview, string callbackUrl, string exitUrl)
        {
            using (var client = _factory.Create())
            {
                var api = new ApiClient(_factory, _serializer);

                var result = await api.HttpGet<string>($"registrations/{registrationId}/launch-url?courseSlug={courseSlug}&preview={preview}&callbackUrl={callbackUrl}&exitUrl={exitUrl}");

                return result.Data;
            }
        }

        public async Task<RegistrationProgress> GetRegistrationInstanceProgress(Guid registrationId, int? instance)
        {
            using (var client = _factory.Create())
            {
                var api = new ApiClient(_factory, _serializer);

                var url = $"registrations/{registrationId}/progress";
                if (instance.HasValue)
                    url += $"?instance={instance}";

                var result = await api.HttpGet<RegistrationProgress>(url);

                return result.Data;
            }
        }

        public async Task<string> GetRegistrationId(string courseSlug, Guid learnerId)
        {
            using (var client = _factory.Create())
            {
                var api = new ApiClient(_factory, _serializer);

                var result = await api.HttpGet<string>($"courses/{courseSlug}/learners/{learnerId}/registration");

                return result.Data;
            }
        }

        public async Task<RegistrationList> GetRegistrations(string course, string more = null)
        {
            var list = new RegistrationList();

            using (var client = _factory.Create())
            {
                var api = new ApiClient(_factory, _serializer);

                var url = "registrations";

                if (!string.IsNullOrEmpty(more))
                    url += "?more=" + more;

                else if (!string.IsNullOrEmpty(course))
                    url += "?course=" + course;

                var result = await api.HttpGet<Registration[]>(url);

                if (result.Status == HttpStatusCode.NotFound)
                    return list;

                list.Registrations = result.Data;

                if (result.Headers.Contains("x-scorm-registrations-more"))
                    more = result.Headers.GetValues("x-scorm-registrations-more").FirstOrDefault();

                return list;
            }
        }

        public async Task<string> CreateImport(string courseSlug, bool mayCreateNewVersion, string callbackUrl, string uploadedContentType, string contentMetadata, Stream stream)
        {
            var content = new MultipartFormDataContent
            {
                { new StringContent(courseSlug), "courseSlug" },
                { new StringContent(mayCreateNewVersion.ToString()), "mayCreateNewVersion" },
                { new StringContent(callbackUrl), "callbackUrl" },
                { new StringContent(uploadedContentType), "uploadedContentType" },
                { new StringContent(contentMetadata), "contentMetadata" },
                { new StreamContent(stream), "file", "uploadFile" }
            };

            using (var client = _factory.Create())
            {
                var api = new ApiClient(_factory, _serializer);

                var result = await api.HttpPost<string>("imports", content);

                return result.Data;
            }
        }

        public async Task<CourseImport> GetImportStatus(string importSlug)
        {
            using (var client = _factory.Create())
            {
                var api = new ApiClient(_factory, _serializer);

                var url = $"imports/{importSlug}";

                var result = await api.HttpGet<CourseImport>(url);

                return result.Data;
            }
        }
    }

    public class ScormApplication
    {
        public string AppID { get; set; }
        public string SecretKey { get; set; }
    }

    public class Registration
    {
        public string Id { get; set; }
        public int Instance { get; set; }
        public DateTime? Updated { get; set; }
        public string RegistrationCompletion { get; set; }
        public string RegistrationSuccess { get; set; }
        public int? TotalSecondsTracked { get; set; }
        public DateTime? FirstAccessDate { get; set; }
        public DateTime? LastAccessDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CourseId { get; set; }
        public string CourseTitle { get; set; }
        public int? CourseVersion { get; set; }
        public string LearnerId { get; set; }
        public string LearnerEmail { get; set; }
        public string LearnerFirstName { get; set; }
        public string LearnerLastName { get; set; }
    }

    public class RegistrationList
    {
        public RegistrationList() { }

        public Registration[] Registrations { get; set; }

        public string More { get; set; }
    }

    public class RegistrationRequest
    {
        public RegistrationRequest() { }

        public RegistrationRequest(Guid registrationId, string courseSlug, Guid learnerId, string learnerEmail, string learnerFirstName, string learnerLastName)
        {
            RegistrationId = registrationId;
            CourseSlug = courseSlug;
            LearnerId = learnerId;
            LearnerEmail = learnerEmail;
            LearnerFirstName = learnerFirstName;
            LearnerLastName = learnerLastName;
        }

        public Guid RegistrationId { get; set; }
        public string CourseSlug { get; set; }
        public Guid LearnerId { get; set; }
        public string LearnerEmail { get; set; }
        public string LearnerFirstName { get; set; }
        public string LearnerLastName { get; set; }
    }

    public class RegistrationProgress
    {
        public DateTime? CompletedDate { get; set; }
        public DateTime? FirstAccessDate { get; set; }
        public DateTime? LastAccessDate { get; set; }
        public string RegistrationCompletion { get; set; }
        public string RegistrationSuccess { get; set; }
        public decimal? ScoreScaled { get; set; }
        public int? TotalSecondsTracked { get; set; }
    }

    public class Course
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
        public int? Version { get; set; }
        public int? RegistrationCount { get; set; }
        public string CourseLearningStandard { get; set; }
    }

    public class CourseImport
    {
        public string CourseId { get; set; }
        public string Message { get; set; }

        public bool IsComplete { get; set; }
        public bool IsError { get; set; }
        public bool IsRunning { get; set; }
    }

    public class LaunchRequest
    {
        public Guid RegistrationId { get; set; }
        public string CourseSlug { get; set; }
        public bool Preview { get; set; }
        public string CallbackUrl { get; set; }
        public string ExitUrl { get; set; }
    }
}