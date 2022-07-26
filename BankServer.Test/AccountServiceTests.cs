using BankServer.Model;
using BankServer.Repositories;
using BankServer.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace BankServer.Test
{
    [TestClass]
    public class AccountServiceTests
    {
        /// <summary>
        /// This test is redundant: <see cref="AccountService.Credit"/> is a trivial method
        /// that delegates all functionality to the repository. It is here only as a basic demonstration of
        /// <see href="https://github.com/moq/moq4">Moq</see>'s capabilities.
        /// </summary>
        [TestMethod]
        public void When_CreditAccount_Expect_AccountReturned()
        {
            var accountRepositoryMock = new Mock<IAccountRepository>();
            var testAccount = new Account
            {
                Id = 1,
                Name = "Assaf"
            };

            testAccount.Credit(2m);

            accountRepositoryMock.Setup(repo => repo.Credit(1, 2m)).Returns(testAccount);

            var sut = new AccountService(accountRepositoryMock.Object);

            var actualAccount = sut.Credit(1, 2m);

            actualAccount.Should().BeEquivalentTo(testAccount);
        }

        [TestMethod]
        public void When_DeleteAll_Expect_IReposityDeleteToBeCalledMultipleTimes()
        {
            var accountRepositoryMock = new Mock<IAccountRepository>();
            var testAccounts = new[]
            {
                new Account {Id = 1},
                new Account {Id = 2},
                new Account {Id = 3}
            };
            var testIds = testAccounts.Select(account => account.Id).ToList();

            accountRepositoryMock.Setup(repo => repo.GetAll()).Returns(testAccounts);

            var sut = new AccountService(accountRepositoryMock.Object);

            sut.DeleteAll();
            accountRepositoryMock.Verify(repo => repo.Delete(It.IsIn<int>(testIds)));
        }

        [TestMethod]
        public void When_GetBlockedAccounts_Expect_OnlyBlockedAccountsToBeReturned()
        {
            var mockAccounts = new[]
            {
                Mock.Of<Account>(account => account.IsBlocked == true),
                Mock.Of<Account>(account => account.IsBlocked == false),
                Mock.Of<Account>(account => account.IsBlocked == true)
            };

            var accountRepositoryMock = Mock.Of<IAccountRepository>(repo => repo.GetAll() == mockAccounts);
            var sut = new AccountService(accountRepositoryMock);

            var actualAccounts = sut.GetBlockedAccounts();

            actualAccounts.Should().HaveCount(2);
        }
    }
}
