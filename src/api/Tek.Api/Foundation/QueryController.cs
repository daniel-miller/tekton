using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Tek.Api;

[ApiController]
[ApiExplorerSettings(GroupName = "React: Query")]
public class QueryController : ControllerBase
{
    private readonly Authorizer _authorizer;
    private readonly IClaimConverter _converter;
    private readonly QueryDispatcher _dispatcher;
    private readonly Reflector _reflector;
    private readonly IJsonSerializer _serializer;
    private readonly QueryBuilder _builder;

    public QueryController(QueryTypeCollection queryTypes,
         Authorizer authorizer, IClaimConverter converter, QueryDispatcher dispatcher, IJsonSerializer serializer)
    {
        _authorizer = authorizer;
        _converter = converter;
        _dispatcher = dispatcher;
        _reflector = new Reflector();
        _serializer = serializer;
        _builder = new QueryBuilder(queryTypes, serializer);
    }

    [HttpPost(Endpoints.React.Queries)]
    [Authorize(Endpoints.React.Queries)]
    public async Task<IActionResult> RunQuery([FromQuery] string q, [FromQuery] Filter filter)
    {
        try
        {
            var queryType = _builder.GetQueryType(q);

            ConfirmPermission(queryType);

            var requestBody = await new StreamReader(Request.Body).ReadToEndAsync();

            var resultType = _builder.GetResultType(queryType);

            var queryObject = _builder.BuildQuery(queryType, resultType, requestBody, filter);

            // TODO: Implement monitoring.

            var resultObject = RunQuery(queryObject, resultType);

            if (resultObject == null)
                return NotFound();

            return Ok(resultObject);
        }
        catch (AccessDeniedException ad)
        {
            return Problem(ad.Message, null, 403);
        }
        catch (BadQueryException bq)
        {
            return Problem(bq.Message, null, 400);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, 500);
        }
    }

    private void ConfirmPermission(Type queryType)
    {
        // Confirm the user's authorization token has permission to run this specific query.

        var principal = _converter.ToPrincipal(User.Claims);

        var resourceName = _reflector.GetResourceName(queryType);

        if (IsGranted(resourceName, BasicAccess.Allow, principal.Roles))
            return;

        var roleNames = principal.Roles.Select(x => x.Name);

        var message = "None of the roles assigned to this token are granted access to run this"
            + $" query. Resource = {resourceName}. Roles = {string.Join(", ", roleNames)}.";

        throw new AccessDeniedException(message);
    }

    private bool IsGranted(string resourceName, BasicAccess access, IEnumerable<Model> roles)
    {
        var relativeUrl = new RelativeUrl(resourceName);

        var granted = _authorizer.IsGranted(relativeUrl.Path, roles, access);

        while (!granted && relativeUrl.HasSegments())
        {
            relativeUrl.RemoveLastSegment();

            granted = _authorizer.IsGranted(relativeUrl.Path, roles, access);
        }

        return granted;
    }

    private object? RunQuery(object queryObject, Type resultType)
    {
        var dispatchMethod = typeof(QueryDispatcher).GetMethod(nameof(QueryDispatcher.Dispatch))!;

        var genericDispatchMethod = dispatchMethod.MakeGenericMethod(resultType);

        var resultObject = genericDispatchMethod.Invoke(_dispatcher, [queryObject]);

        return resultObject;
    }
}