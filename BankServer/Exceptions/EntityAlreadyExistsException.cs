namespace BankServer.Exceptions
{
    public class EntityAlreadyExistsException : Exception
    {
        public EntityAlreadyExistsException(int id)
            : base($"ID #{id} already exists")
        { }
    }
}
