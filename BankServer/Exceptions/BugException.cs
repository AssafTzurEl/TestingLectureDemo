namespace BankServer.Exceptions
{
    /// <summary>
    /// Should never happen - if it does, write full stack trace and anything else you can to log.
    /// </summary>
    public class BugException : Exception
    {
        public BugException(string info)
            : base(info)
        { }
    }
}
