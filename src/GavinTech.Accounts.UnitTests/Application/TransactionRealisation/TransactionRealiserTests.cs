using FluentAssertions;
using GavinTech.Accounts.Application.Interfaces.Persistence;
using GavinTech.Accounts.Application.TransactionRealisation;
using GavinTech.Accounts.Domain.Entities;
using GavinTech.Accounts.Domain.Primitives;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace GavinTech.Accounts.UnitTests.Application.TransactionRealisation
{
    public class TransactionRealiserTests
    {
        private readonly List<Account> _accounts = new();
        private readonly List<TransactionTemplate> _templates = new();
        private readonly ITransactionRealiser _patient;

        public TransactionRealiserTests()
        {
            var mockAccounts = new Mock<IRepository<Account>>();
            mockAccounts.Setup(m => m.GetAsync(default))
                .ReturnsAsync(_accounts);
            mockAccounts.Setup(m => m.Identify(It.IsAny<Account>()))
                .Returns<Account>(a => a.Name);

            var mockTemplates = new Mock<IRepository<TransactionTemplate>>();
            mockTemplates.Setup(m => m.GetAsync(default))
                .ReturnsAsync(_templates);
            mockTemplates.Setup(m => m.Identify(It.IsAny<TransactionTemplate>()))
                .Returns(string.Empty);

            _patient = new TransactionRealiser(mockAccounts.Object, mockTemplates.Object);
        }

        [Fact]
        public async Task RealiseAsync_YieldsRecurringTransactionsFirst()
        {
            _accounts.Add(new()
            {
                Name = "Root"
            });
            _templates.AddRange(new[] {
                new TransactionTemplate
                {
                    Day = new Day(114),
                    Amount = new Amount(12001),
                    Description = "Non-recurring",
                    Account = _accounts[0]
                },
                new RecurringTransactionTemplate
                {
                    Day = new Day(100),
                    Amount = new Amount(12002),
                    Description = "Fortnightly",
                    Account = _accounts[0],
                    Basis = RecurrenceBasis.Daily,
                    Multiplicand = 14
                }
            });

            var result = await _patient.RealiseAsync(null, new Day(129), "Root", default);

            result.Should().BeEquivalentTo(new[] {
                new Transaction
                {
                    Day = new Day(100),
                    Amount = new Amount(12002),
                    RunningTotal = new Amount(12002),
                    Description = "Fortnightly",
                    AccountId = "Root"
                },
                new Transaction
                {
                    Day = new Day(114),
                    Amount = new Amount(12002),
                    RunningTotal = new Amount(24004),
                    Description = "Fortnightly",
                    AccountId = "Root"
                },
                new Transaction
                {
                    Day = new Day(114),
                    Amount = new Amount(12001),
                    RunningTotal = new Amount(36005),
                    Description = "Non-recurring",
                    AccountId = "Root"
                },
                new Transaction
                {
                    Day = new Day(128),
                    Amount = new Amount(12002),
                    RunningTotal = new Amount(48007),
                    Description = "Fortnightly",
                    AccountId = "Root"
                }
            });
        }

        [Fact]
        public async Task RealiseAsync_IncludesChildrenOrderedByAmountThenName()
        {
            _accounts.AddRange(new[]
            {
                new Account
                {
                    Name = "Child"
                },
                new Account
                {
                    Name = "Root"
                },
                new Account
                {
                    Name = "Parent2"
                },
                new Account
                {
                    Name = "Parent1"
                },
            });
            _accounts[0].Parent = _accounts[3];
            _templates.AddRange(new[] {
                new TransactionTemplate
                {
                    Day = new Day(100),
                    Amount = new Amount(12001),
                    Description = "Child template",
                    Account = _accounts[0]
                },
                new TransactionTemplate
                {
                    Day = new Day(100),
                    Amount = new Amount(12001),
                    Description = "Parent2 template",
                    Account = _accounts[2]
                },
                new TransactionTemplate
                {
                    Day = new Day(100),
                    Amount = new Amount(12000),
                    Description = "Parent1 lesser amount",
                    Account = _accounts[3]
                },
                new TransactionTemplate
                {
                    Day = new Day(100),
                    Amount = new Amount(12001),
                    Description = "Parent1 equal amount",
                    Account = _accounts[3]
                },
                new TransactionTemplate
                {
                    Day = new Day(100),
                    Amount = new Amount(12001),
                    Description = "Root template",
                    Account = _accounts[1]
                }
            });

            var result = await _patient.RealiseAsync(null, new Day(101), "Parent1", default);

            result.Should().BeEquivalentTo(new[] {
                new Transaction
                {
                    Day = new Day(100),
                    Amount = new Amount(12000),
                    RunningTotal = new Amount(12000),
                    Description = "Parent1 lesser amount",
                    AccountId = "Parent1"
                },
                new Transaction
                {
                    Day = new Day(100),
                    Amount = new Amount(12001),
                    RunningTotal = new Amount(24001),
                    Description = "Child template",
                    AccountId = "Child"
                },
                new Transaction
                {
                    Day = new Day(100),
                    Amount = new Amount(12001),
                    RunningTotal = new Amount(36002),
                    Description = "Parent1 equal amount",
                    AccountId = "Parent1"
                }
            });
        }

        [Fact]
        public async Task RealiseAsync_StartsAtStartDate()
        {
            _accounts.Add(new()
            {
                Name = "Root"
            });
            _templates.AddRange(new[] {
                new TransactionTemplate
                {
                    Day = new Day(114),
                    Amount = new Amount(12001),
                    Description = "Non-recurring",
                    Account = _accounts[0]
                },
                new RecurringTransactionTemplate
                {
                    Day = new Day(86),
                    Amount = new Amount(12002),
                    Description = "Fortnightly",
                    Account = _accounts[0],
                    Basis = RecurrenceBasis.Daily,
                    Multiplicand = 14
                }
            });

            var result = await _patient.RealiseAsync(new Day(114), new Day(129), null, default);

            result.Should().BeEquivalentTo(new[] {
                new Transaction
                {
                    Day = new Day(114),
                    Amount = new Amount(12002),
                    RunningTotal = new Amount(12002),
                    Description = "Fortnightly",
                    AccountId = "Root"
                },
                new Transaction
                {
                    Day = new Day(114),
                    Amount = new Amount(12001),
                    RunningTotal = new Amount(24003),
                    Description = "Non-recurring",
                    AccountId = "Root"
                },
                new Transaction
                {
                    Day = new Day(128),
                    Amount = new Amount(12002),
                    RunningTotal = new Amount(36005),
                    Description = "Fortnightly",
                    AccountId = "Root"
                }
            });
        }

        [Fact]
        public async Task RealiseAsync_RespectsHierarchicalClosure()
        {
            _accounts.AddRange(new[] {
                new Account
                {
                    Name = "Root"
                },
                new Account
                {
                    Name = "LateClosingParent",
                    ClosedAfter = new Day("2021-04-15")
                },
                new Account
                {
                    Name = "EarlyClosingChild",
                    ClosedAfter = new Day("2021-03-31")
                }
            });
            _accounts[1].Parent = _accounts[0];
            _accounts[2].Parent = _accounts[1];
            _templates.AddRange(new[] {
                new TransactionTemplate
                {
                    Day = new Day("2021-04-16"),
                    Amount = new Amount(12001),
                    Description = "Invisible",
                    Account = _accounts[1]
                },
                new RecurringTransactionTemplate
                {
                    Day = new Day("2021-03-15"),
                    Amount = new Amount(12002),
                    Description = "LateClosingParent template",
                    Account = _accounts[1],
                    Basis = RecurrenceBasis.Monthly,
                    Multiplicand = 1
                },
                new RecurringTransactionTemplate
                {
                    Day = new Day("2021-03-15"),
                    Amount = new Amount(12002),
                    Description = "EarlyClosingChild template",
                    Account = _accounts[2],
                    Basis = RecurrenceBasis.Monthly,
                    Multiplicand = 1
                }
            });

            var result = await _patient.RealiseAsync(null, new Day("2021-12-31"), null, default);

            result.Should().BeEquivalentTo(new[] {
                new Transaction
                {
                    Day = new Day("2021-03-15"),
                    Amount = new Amount(12002),
                    RunningTotal = new Amount(12002),
                    Description = "EarlyClosingChild template",
                    AccountId = "EarlyClosingChild"
                },
                new Transaction
                {
                    Day = new Day("2021-03-15"),
                    Amount = new Amount(12002),
                    RunningTotal = new Amount(24004),
                    Description = "LateClosingParent template",
                    AccountId = "LateClosingParent"
                },
                new Transaction
                {
                    Day = new Day("2021-04-15"),
                    Amount = new Amount(12002),
                    RunningTotal = new Amount(36006),
                    Description = "LateClosingParent template",
                    AccountId = "LateClosingParent"
                }
            });
        }

        [Fact]
        public async Task RealiseAsync_SkipsTombstones()
        {
            _accounts.AddRange(new[] {
                new Account
                {
                    Name = "Root"
                },
                new Account
                {
                    Name = "Account"
                }
            });
            _accounts[1].Parent = _accounts[0];
            _templates.AddRange(new[] {
                new TransactionTemplate
                {
                    Day = new Day("2021-04-30"),
                    Amount = new Amount(12001),
                    Description = "Non-recurring",
                    Account = _accounts[1]
                },
                new RecurringTransactionTemplate
                {
                    Day = new Day("2021-03-31"),
                    Amount = new Amount(12002),
                    Description = "Monthly",
                    Account = _accounts[1],
                    Basis = RecurrenceBasis.Monthly,
                    Multiplicand = 1,
                    Tombstones = { new Day("2021-04-30") }
                }
            });

            var result = await _patient.RealiseAsync(new Day("2021-03-01"), new Day("2021-06-01"), null, default);

            result.Should().BeEquivalentTo(new[] {
                new Transaction
                {
                    Day = new Day("2021-03-31"),
                    Amount = new Amount(12002),
                    RunningTotal = new Amount(12002),
                    Description = "Monthly",
                    AccountId = "Account"
                },
                new Transaction
                {
                    Day = new Day("2021-04-30"),
                    Amount = new Amount(12001),
                    RunningTotal = new Amount(24003),
                    Description = "Non-recurring",
                    AccountId = "Account"
                },
                new Transaction
                {
                    Day = new Day("2021-05-31"),
                    Amount = new Amount(12002),
                    RunningTotal = new Amount(36005),
                    Description = "Monthly",
                    AccountId = "Account"
                }
            });
        }

        [Fact]
        public async Task RealiseAsync_EndsRecurrenceBeforeUntilExcl()
        {
            _accounts.Add(new()
            {
                Name = "Root"
            });
            _templates.AddRange(new[] {
                new RecurringTransactionTemplate
                {
                    Day = new Day(100),
                    Amount = new Amount(12002),
                    Description = "Fortnightly",
                    Account = _accounts[0],
                    Basis = RecurrenceBasis.Daily,
                    Multiplicand = 14,
                    UntilExcl = new Day(128)
                }
            });

            var result = await _patient.RealiseAsync(null, new Day(129), null, default);

            result.Should().BeEquivalentTo(new[] {
                new Transaction
                {
                    Day = new Day(100),
                    Amount = new Amount(12002),
                    RunningTotal = new Amount(12002),
                    Description = "Fortnightly",
                    AccountId = "Root"
                },
                new Transaction
                {
                    Day = new Day(114),
                    Amount = new Amount(12002),
                    RunningTotal = new Amount(24004),
                    Description = "Fortnightly",
                    AccountId = "Root"
                }
            });
        }

        [Fact]
        public async Task RealiseAsync_UnsticksFromEndOfMonth_GivenRecurrenceFrom28Jan()
        {
            _accounts.AddRange(new[] {
                new Account
                {
                    Name = "Root"
                }
            });
            _templates.AddRange(new[] {
                new RecurringTransactionTemplate
                {
                    Day = new Day("2021-01-28"),
                    Amount = new Amount(12002),
                    Description = "Monthly",
                    Account = _accounts[0],
                    Basis = RecurrenceBasis.Monthly,
                    Multiplicand = 1
                }
            });

            var result = await _patient.RealiseAsync(new Day("2021-01-01"), new Day("2021-03-29"), null, default);

            result.Should().BeEquivalentTo(new[] {
                new Transaction
                {
                    Day = new Day("2021-01-28"),
                    Amount = new Amount(12002),
                    RunningTotal = new Amount(12002),
                    Description = "Monthly",
                    AccountId = "Root"
                },
                new Transaction
                {
                    Day = new Day("2021-02-28"),
                    Amount = new Amount(12002),
                    RunningTotal = new Amount(24004),
                    Description = "Monthly",
                    AccountId = "Root"
                },
                new Transaction
                {
                    Day = new Day("2021-03-28"),
                    Amount = new Amount(12002),
                    RunningTotal = new Amount(36006),
                    Description = "Monthly",
                    AccountId = "Root"
                }
            });
        }

        [Fact]
        public async Task RealiseAsync_SticksToEndOfMonth_GivenRecurrenceFrom28Feb()
        {
            _accounts.AddRange(new[] {
                new Account
                {
                    Name = "Root"
                }
            });
            _templates.AddRange(new[] {
                new RecurringTransactionTemplate
                {
                    Day = new Day("2021-02-28"),
                    Amount = new Amount(12002),
                    Description = "Monthly",
                    Account = _accounts[0],
                    Basis = RecurrenceBasis.Monthly,
                    Multiplicand = 1
                }
            });

            var result = await _patient.RealiseAsync(new Day("2021-02-01"), new Day("2021-05-01"), null, default);

            result.Should().BeEquivalentTo(new[] {
                new Transaction
                {
                    Day = new Day("2021-02-28"),
                    Amount = new Amount(12002),
                    RunningTotal = new Amount(12002),
                    Description = "Monthly",
                    AccountId = "Root"
                },
                new Transaction
                {
                    Day = new Day("2021-03-31"),
                    Amount = new Amount(12002),
                    RunningTotal = new Amount(24004),
                    Description = "Monthly",
                    AccountId = "Root"
                },
                new Transaction
                {
                    Day = new Day("2021-04-30"),
                    Amount = new Amount(12002),
                    RunningTotal = new Amount(36006),
                    Description = "Monthly",
                    AccountId = "Root"
                }
            });
        }
    }
}
