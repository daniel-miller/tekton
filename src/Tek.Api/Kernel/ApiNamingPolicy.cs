namespace Tek.Api
{
    public class ApiNamingPolicy : System.Text.Json.JsonNamingPolicy
    {
        public override string ConvertName(string name) => name;
    }
}
