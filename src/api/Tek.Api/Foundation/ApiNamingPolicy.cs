namespace Tek.Api;

/// <remarks>
/// This naming policy is required to ensure System.Text.Json does not modify the property names
/// of an object when it serializes API response data to JSON format. Otherwise, by default, 
/// property names are converted from PascalCase to camelCase - and this can cause a lot of 
/// problems when it is unexpected.
/// </remarks>
public class ApiNamingPolicy : System.Text.Json.JsonNamingPolicy
{
    public override string ConvertName(string name) => name;
}
