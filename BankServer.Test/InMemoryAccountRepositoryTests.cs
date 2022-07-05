using BankServer.Model;
using BankServer.Repositories;
using FluentAssertions;

namespace BankServer.Test
{
    [TestClass]
    public class InMemoryAccountRepositoryTests
    {
        [TestMethod]
        public void When_NoAccountAdded_Expect_GetToReturnZeroItems()
        {
            var sut = new InMemoryAccountRepository();

            sut.GetAll().Should().BeEmpty();
        }

        [TestMethod]
        public void When_AddingAnAccount_Expect_ReturnedValueToMatchAccount()
        {
            const string AccountName = "Assaf";

            var sut = new InMemoryAccountRepository();
            var account = new Account
            {
                Name = AccountName
                // ID = 0 by default
            };

            var actual = sut.Add(account);

            actual.Name.Should().Be(AccountName);
            actual.Id.Should().NotBe(0);
            actual.Balance.Should().Be(0);
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(-1)]
        public void When_AddingAnAccountWithNonZeroId_Expect_ArgumentException(int id)
        {
            var sut = new InMemoryAccountRepository();
            var account = new Account
            {
                Id = id
            };

            var action = () => sut.Add(account);

            action.Should().Throw<ArgumentException>().WithMessage("New account must not have an ID");
        }

        // Many more tests should be added...
    }
}
