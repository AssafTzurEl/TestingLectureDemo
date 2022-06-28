namespace BankServer.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(int id)
            : base($"ID #{id} not found")
        { }
    }
}
