using Microsoft.AspNetCore.Mvc;

using Tek.Integration.Google;

namespace Tek.Api.Legacy;

[ApiController]
public class OldTranslationController : ControllerBase
{
    private const string GoogleApiErrorCause = "The payment method for this Google Cloud account may be an expired credit card. Please refer to the documentation for Google API integration in Confluence.";
    private const string GoogleApiErrorEffect = "RESOURCE_EXHAUSTED Quota exceeded for quota metric";

    private readonly ITranslator _translator;
    private readonly IMonitor _monitor;

    public OldTranslationController(ITranslator translator, IMonitor monitor)
    {
        _translator = translator;
        _monitor = monitor;
    }

    [ApiExplorerSettings(GroupName = "Content: Translations")]
    [HttpPost("content/translations/translate")]
    public async Task<ActionResult<List<string>>> TranslateAsync(string from, string to, [FromBody] string[] contents)
    {
        try
        {
            var translations = new List<string>();

            foreach (var content in contents)
                translations.Add(await _translator.TranslateAsync(content, from, to));

            return translations;
        }
        catch (UnsupportedLanguageException ex)
        {
            return BadRequest(ex);
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains(GoogleApiErrorEffect, StringComparison.CurrentCultureIgnoreCase))
                _monitor.Warning(GoogleApiErrorCause);

            _monitor.Error(ex.Message);

            return Problem(ex.Message);
        }
    }
}

