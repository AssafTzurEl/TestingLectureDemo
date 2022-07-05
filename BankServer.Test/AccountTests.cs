using BankServer.Model;

namespace BankServer.Test
{
    [TestClass]
    public class AccountTests
    {
        [TestMethod]
        public void When_CreateAccount_Expect_ZeroBalance()
        {
            Account sut = new Account();

            Assert.AreEqual(0, sut.Balance);
        }

        [TestMethod]
        public void When_CreditPositiveAmount_Expect_NewBalance()
        {
            const decimal amount = 100;

            Account sut = new Account();

            sut.Credit(amount);

            Assert.AreEqual(amount, sut.Balance);
        }

        [TestMethod]
        public void When_CreditNegativeAmount_Expect_OutOfRangeException()
        {
            const decimal amount = -100;

            Account sut = new Account();

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => sut.Credit(amount));
        }

        [TestMethod]
        public void When_ChargePositiveAmount_Expect_NewBalance()
        {
            const decimal amount = 100;

            Account sut = new Account();

            sut.Charge(amount);

            Assert.AreEqual(-amount, sut.Balance);
        }


        [TestMethod]
        public void When_ChargeNegativeAmount_Expect_OutOfRangeException()
        {
            const decimal amount = -100;

            Account sut = new Account();

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => sut.Charge(amount));
        }
    }
}