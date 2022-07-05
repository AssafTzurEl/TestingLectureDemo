namespace BankServer.Model
{
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public decimal Balance => _balance;

        public decimal Credit(decimal amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount));
            }

            lock (this)
            {
                _balance += amount;

                return _balance;
            }
        }

        public decimal Charge(decimal amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount));
            }

            lock (this)
            {
                _balance -= amount;

                return _balance;
            }
        }

        private decimal _balance = 0m;
    }
}
