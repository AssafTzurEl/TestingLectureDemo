namespace BankServer.Model
{
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public virtual decimal Balance => _balance;

        public virtual bool IsBlocked => _balance < BlockingThreshold;

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

        private const decimal BlockingThreshold = -5000m;

        private decimal _balance = 0m;
    }
}
