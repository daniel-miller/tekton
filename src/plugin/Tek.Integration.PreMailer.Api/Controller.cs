using Microsoft.AspNetCore.Mvc;

using PreMailerComponent = PreMailer.Net.PreMailer;

namespace Tek.Integration.PreMailer.Api;

[ApiController]
public class Controller(ReleaseSettings releaseSettings, IMonitor monitor) : ControllerBase
{
    private readonly ReleaseSettings _releaseSettings = releaseSettings;
    private readonly IMonitor _monitor = monitor;

    [HttpPost]
    [Route(Endpoints.Execute)]
    public ActionResult<string> Execute()
    {
        try
        {
            using (var reader = new StreamReader(Request.Body))
            {
                string html = reader.ReadToEndAsync().GetAwaiter().GetResult();

                var inline = PreMailerComponent.MoveCssInline(html, true, null, null, false, false).Html;
                
                return Ok(inline);
            }
        }
        catch (Exception ex)
        {
            _monitor.Error(ex.Message);
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet]
    [Route(Endpoints.Error)]
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

    [HttpGet(Endpoints.Status)]
    public IActionResult GetStatus()
    {
        var version = typeof(Controller).Assembly.GetName().Version;

        var status = $"Engine Premailer API version {version} is online. The {_releaseSettings.Environment} environment says hello.";

        return Ok(status);
    }

    [HttpGet(Endpoints.Version)]
    public IActionResult GetVersion()
    {
        var version = typeof(Controller).Assembly.GetName().Version?.ToString() ?? "0.0.0.0";

        return Ok(version);
    }
}