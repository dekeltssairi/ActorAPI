namespace API.Attributes
{
    public class LogOnErrorAttribute : Attribute
    {
        public string ErrorMessage { get; }

        public LogOnErrorAttribute(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
