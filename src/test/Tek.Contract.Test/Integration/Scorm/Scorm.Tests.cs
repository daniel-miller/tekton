using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Tek.Contract.Integration.Scorm;

namespace Tek.Contract.Test.Integration.Scorm
{
    /// <remarks>
    /// These tests assume an existing Demo application in the InSite realm on the SCORM Cloud 
    /// platform. The Demo must contain one course with Course ID = "demo". To run these tests on
    /// a specific environment, set the BaseUri property to the desired environment. (In v25.2 this
    /// will be improved so all URI values for all environments will be available in the app 
    /// settings configuration file. This intentionally not yet done, to avoid any last-minute
    /// to the Engine API code, which will be needed for improved configuration options here.)
    /// Refer to https://app.cloud.scorm.com/sc/user/AppDetail?appId=K1OVIMEA23
    /// </remarks>
    [Collection("Integration Tests")]
    [Trait("Category", "integration/scorm")]
    public class ScormTests
    {
        private ScormApplication Application { get; set; } = new ScormApplication
        {
            AppID = "K1OVIMEA23",
            SecretKey = "D0EXEZ8z0oqhenKuKXaCyDrrBUl8DgOFCEx7TJqi"
        };

        private readonly ApiClientFixture _fixture;

        public ScormTests(ApiClientFixture fixture)
        {
            _fixture = fixture;
            _fixture.Factory.AddAuthorizationHeader("UserName", Application.AppID);
            _fixture.Factory.AddAuthorizationHeader("Password", Application.SecretKey);
        }

        [Fact]
        public async Task GetCourses()
        {
            var client = new ScormApiClient(_fixture.Factory, _fixture.Serializer);

            var courses = await client.GetCourses();

            Assert.NotNull(courses);
            Assert.NotEmpty(courses);
            Assert.Single(courses);
        }

        [Fact]
        public async Task GetCourse_InSiteDemo()
        {
            var client = new ScormApiClient(_fixture.Factory, _fixture.Serializer);

            var demo = await client.GetCourse("demo");
            
            Assert.NotNull(demo);
            Assert.Equal("demo", demo.Id);
            Assert.Equal("Hazard Identification and Risk Assessment", demo.Title);
            Assert.Equal("SCORM12", demo.CourseLearningStandard);
        }

        [Fact]
        public async Task CreateRegistration_ExistingRegistrationId()
        {
            var course = "demo";
            var learnerId = Guid.Parse("bf6ada46-8158-45fb-bc28-143837e28c6f");
            var registrationId = Guid.Parse("bf6ada46-8158-45fb-bc28-143837e28c6f");

            var registration = new RegistrationRequest
            {
                CourseSlug = course,
                RegistrationId = learnerId,
                LearnerId = learnerId,
                LearnerEmail = "xunit.{learnerId}@mg.shiftiq.com",
                LearnerFirstName = "Unit",
                LearnerLastName = $"Test {learnerId}"
            };

            // If the registration already exists then we expect an HTTP 204 response.

            var client = new ScormApiClient(_fixture.Factory, _fixture.Serializer);

            await client.CreateRegistration(registration);
        }

        /// <remarks>
        /// Please Note! The cost for each new registration is $3.60 when we exceed our quota per
        /// month. Therefore this unit test should not be invoked frequently.
        /// </remarks>
        [Fact(Skip = "Avoid Overage Cost")]
        public async Task CreateRegistration_NewRegistrationId()
        {
            var course = "demo";
            var learnerId = Guid.NewGuid();
            var registrationId = learnerId;

            var registration = new RegistrationRequest
            {
                CourseSlug = course,
                RegistrationId = learnerId,
                LearnerId = learnerId,
                LearnerEmail = "xunit.{learnerId}@mg.shiftiq.com",
                LearnerFirstName = "Unit",
                LearnerLastName = $"Test {learnerId}"
            };

            var client = new ScormApiClient(_fixture.Factory, _fixture.Serializer);

            await client.CreateRegistration(registration);
        }

        [Fact]
        public async Task GetRegistration_ValidRegistrationId()
        {
            var course = "demo";
            var learnerId = Guid.Parse("bf6ada46-8158-45fb-bc28-143837e28c6f");
            var registrationId = Guid.Parse("bf6ada46-8158-45fb-bc28-143837e28c6f");

            var client = new ScormApiClient(_fixture.Factory, _fixture.Serializer);

            var registration = await client.GetRegistrationId(course, learnerId);

            Assert.Equal(registrationId, Guid.Parse(registration));
        }

        [Fact]
        public async Task GetRegistration_ValidRegistrationLastInstance()
        {
            var registrationId = Guid.Parse("bf6ada46-8158-45fb-bc28-143837e28c6f");

            var client = new ScormApiClient(_fixture.Factory, _fixture.Serializer);

            var instance = await client.GetRegistrationInstance(registrationId);

            Assert.Equal(0, instance);
        }

        [Fact]
        public async Task GetRegistration_ValidRegistrationLaunchUrl()
        {
            var registrationId = Guid.Parse("bf6ada46-8158-45fb-bc28-143837e28c6f");

            var launch = new LaunchRequest
            {
                RegistrationId = registrationId,
                CourseSlug = "demo",
                Preview = true,
                CallbackUrl = "https://www.shiftiq.com",
                ExitUrl = "https://www.shiftiq.com"
            };

            var client = new ScormApiClient(_fixture.Factory, _fixture.Serializer);

            var url = await client.GetRegistrationLaunchUrl(registrationId, launch.CourseSlug, launch.Preview, launch.CallbackUrl, launch.ExitUrl);

            // A SCORM Cloud preview launch URL looks like this:
            // https://cloud.scorm.com/api/cloud/course/preview/cc647714-bdf5-4145-80ca-19230828b0b6

            Assert.StartsWith("https://cloud.scorm.com/api/cloud/course/preview/", url);
        }

        [Fact]
        public async Task GetProgress_ValidProgress()
        {
            var registrationId = Guid.Parse("bf6ada46-8158-45fb-bc28-143837e28c6f");

            var client = new ScormApiClient(_fixture.Factory, _fixture.Serializer);

            var progress = await client.GetRegistrationInstanceProgress(registrationId, null);

            Assert.NotNull(progress);

            Assert.StartsWith("UNKNOWN", progress.RegistrationCompletion);
        }
    }
}