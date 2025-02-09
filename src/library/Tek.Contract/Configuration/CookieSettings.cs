namespace Tek.Contract
{
    public class CookieSettings
    {
        public string Name { get; set; }
        public string Domain { get; set; }
        public string Path { get; set; }
        public int Lifetime { get; set; }
        public bool Encrypt { get; set; }
    }
}
