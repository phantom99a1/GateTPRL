namespace CommonLib
{
    public class ApplicationError
    {
        public DateTime Time { get; set; }

        public string Level { get; set; } = string.Empty;

        public string ThreadName { get; set; } = string.Empty;

        public string ThreadId { get; set; } = string.Empty;

        public string MethodName { get; set; } = string.Empty;

        public string Line { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;
    }
}
