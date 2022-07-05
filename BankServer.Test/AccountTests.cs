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

        [TestMethod]
        [DataRow(0.0)]
        [DataRow(10.0)]
        [DataRow(4999.0)]
        [DataRow(4999.9999)]
        [DataRow(5000.0)]
        public void When_BalanceAboveOrEqualsMinus5000_Expect_AccountToNotBeDisabled(double chargeAmount)
        {
            decimal decimalChargeAmount = (decimal) chargeAmount;
            Account sut = new Account();

            sut.Charge(decimalChargeAmount);
            sut.IsBlocked.Should().BeFalse();
        }

        [TestMethod]
        [DataRow(5000.01)]
        [DataRow(5001.0)]
        [DataRow(1000000.0)]
        public void When_BalanceBelowMinus5000_Expect_AccountToBeDisabled(double chargeAmount)
        {
            decimal decimalChargeAmount = (decimal) chargeAmount;
            Account sut = new Account();

            sut.Charge(decimalChargeAmount);
            sut.IsBlocked.Should().BeTrue();
        }

        public void When_BalanceChanges_Expect_AccountToBeDisabledAccordingly()
        {
            Account sut = new Account();

            sut.IsBlocked.Should().BeFalse();
            sut.Charge(4000m); // balance = -4000
            sut.IsBlocked.Should().BeFalse();
            sut.Charge(1000m); // balance = -5000
            sut.IsBlocked.Should().BeFalse();
            sut.Charge(1m); // balance = -5001
            sut.IsBlocked.Should().BeTrue();
            sut.Credit(1); // balance = -5000
            sut.IsBlocked.Should().BeFalse();
            sut.Charge(1m); // balance = -5001
            sut.IsBlocked.Should().BeTrue();
        }
    }
}