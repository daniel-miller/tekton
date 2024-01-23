namespace Common.Contract
{
    public class Permission<T>
    {
        public T Operation { get; set; }
        public T Resource { get; set; }
        public T Role { get; set; }
    }
}