using GavinTech.Accounts.Application.Interfaces.Persistence;
using GavinTech.Accounts.Application.TransactionRealisation;
using GavinTech.Accounts.Domain.Entities;
using Moq;
using System.Collections.Generic;

namespace GavinTech.Accounts.UnitTests.Application.TransactionRealisation
{
    public class TransactionRealiserTests
    {
        private List<Account> _accounts = new();
        private List<TransactionTemplate> _templates = new();
        private readonly ITransactionRealiser _patient;

        public TransactionRealiserTests()
        {
            var mockAccounts = new Mock<IRepository<Account>>();
            mockAccounts.Setup(m => m.GetAsync(default, default))
                .ReturnsAsync(() => _accounts);

            var mockTemplates = new Mock<IRepository<TransactionTemplate>>();
            mockTemplates.Setup(m => m.GetAsync(default, default))
                .ReturnsAsync(() => _templates);

            _patient = new TransactionRealiser(mockAccounts.Object, mockTemplates.Object);
        }
    }
}
