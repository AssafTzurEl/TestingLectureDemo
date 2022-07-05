using BankServer.Model;
using FluentAssertions;


namespace BankServer.Test
{
    [TestClass]
    public class AccountTests
    {
        [TestMethod]
        public void When_CreateAccount_Expect_ZeroBalance()
        {
            Account sut = new Account();

            //Assert.AreEqual(0, sut.Balance);
            sut.Balance.Should().Be(0);
        }

        [TestMethod]
        [DataRow(0.0)]
        [DataRow(1.0)]
        [DataRow(100.0)]
        [DataRow(1000000000.0)]
        [DataRow(3.1415926)]
        public void When_CreditPositiveAmount_Expect_NewBalance(double amount)
        {
            decimal decimalAmount = (decimal) amount;
            Account sut = new Account();

            sut.Credit(decimalAmount);

            //Assert.AreEqual(decimalAmount, sut.Balance);
            sut.Balance.Should().Be(decimalAmount);
        }

        [TestMethod]
        public void When_CreditNegativeAmount_Expect_OutOfRangeException()
        {
            const decimal amount = -100m;

            Account sut = new Account();

            //Assert.ThrowsException<ArgumentOutOfRangeException>(() => sut.Credit(amount));
            var action = () => sut.Credit(amount);

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        [DataRow(0.0)]
        [DataRow(1.0)]
        [DataRow(100.0)]
        [DataRow(1000000000.0)]
        [DataRow(3.1415926)]
        public void When_ChargePositiveAmount_Expect_NewBalance(double amount)
        {
            decimal decimalAmount = (decimal) amount;
            Account sut = new Account();

            sut.Charge(decimalAmount);

            //Assert.AreEqual(-decimalAmount, sut.Balance);
            sut.Balance.Should().Be(-decimalAmount);
        }


        [TestMethod]
        public void When_ChargeNegativeAmount_Expect_OutOfRangeException()
        {
            const decimal amount = -100m;

            Account sut = new Account();

            //Assert.ThrowsException<ArgumentOutOfRangeException>(() => sut.Charge(amount));
            var action = () => sut.Charge(amount);

            action.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}