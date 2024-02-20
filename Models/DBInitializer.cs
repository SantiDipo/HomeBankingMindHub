namespace HomeBankingMindHub.Models
{
    public class DBInitializer
    {
        public static void Initialize(HomeBankingContext context)
        {
            if (!context.Clients.Any())
            {
                var clients = new Client[]
                {
                    new Client { Email = "vcoronado@gmail.com", FirstName="Victor", 
                        LastName="Coronado", Password="123456"},
                    new Client { Email = "santidipo@gmail.com", FirstName="Santiago",
                        LastName="D Ippolito", Password="456283"},
                    new Client { Email = "jacinfunes@gmail.com", FirstName="Jacinta",
                        LastName="Funes", Password="283946"}
                };

                context.Clients.AddRange(clients);

                //guardamos
                context.SaveChanges();
            }

            if (!context.Account.Any())
            {
                var accountVictor = context.Clients.FirstOrDefault(client => client.Email == "vcoronado@gmail.com");
                var accountSanti = context.Clients.FirstOrDefault(client => client.Email == "santidipo@gmail.com");
                var accountJacin = context.Clients.FirstOrDefault(client => client.Email == "jacinfunes@gmail.com");

                if (accountVictor != null || accountSanti != null || accountJacin != null)
                {
                    var accounts = new Account[]
                    {
                        new Account {ClientId = accountVictor.Id, CreationDate = DateTime.Now, Number = "VIN001", Balance = 10000000 },
                        new Account {ClientId = accountSanti.Id, CreationDate = new DateTime (2021, 6, 5, 10, 00, 0) , Number = "VIN002", Balance = 20000 },
                        new Account {ClientId = accountJacin.Id, CreationDate = new DateTime (2002, 3, 6, 11, 00, 0), Number = "VIN003", Balance = 123 }
                    };
                    foreach (Account account in accounts)
                    {
                        context.Account.Add(account);
                    }
                    context.SaveChanges();

                }
            }

            if (!context.Transactions.Any())

            {
                var account1 = context.Account.FirstOrDefault(c => c.Number == "VIN001");
                var account2 = context.Account.FirstOrDefault(c => c.Number == "VIN002");
                var account3 = context.Account.FirstOrDefault(c => c.Number == "VIN003");

                if (account1 != null)
                {
                    var transactions = new Transaction[]
                    {

                        new Transaction { AccountId= account1.Id, Amount = 10000, DateTimed= DateTime.Now.AddHours(-5), Description = "Transferencia reccibida", Type = TransactionType.CREDIT },

                        new Transaction { AccountId= account1.Id, Amount = -2000, DateTimed= DateTime.Now.AddHours(-6), Description = "Compra en tienda mercado libre", Type = TransactionType.DEBIT },

                        new Transaction { AccountId= account1.Id, Amount = -3000, DateTimed= DateTime.Now.AddHours(-7), Description = "Compra en tienda CompraGamer", Type = TransactionType.DEBIT },

                        new Transaction { AccountId= account2.Id, Amount = 1000000, DateTimed= DateTime.Now.AddHours(5), Description = "Transferencia reccibida", Type = TransactionType.CREDIT },

                        new Transaction { AccountId= account2.Id, Amount = 200000, DateTimed= DateTime.Now.AddHours(8), Description = "Transferencia reccibida", Type = TransactionType.DEBIT },

                        new Transaction { AccountId= account3.Id, Amount = -300000, DateTimed= DateTime.Now.AddHours(-7), Description = "Compra en tienda Falabella", Type = TransactionType.CREDIT },

                    };
                    foreach (Transaction transaction in transactions)
                    {
                        context.Transactions.Add(transaction);
                    }
                    context.SaveChanges();
                }
            }
            if (!context.Loans.Any())
            {
                var loans = new Loan[]
                {
                    new Loan { Name = "Hipotecario", MaxAmount = 500000, Payments = "12,24,36,48,60" },
                    new Loan { Name = "Personal", MaxAmount = 100000, Payments = "6,12,24" },
                    new Loan { Name = "Automotriz", MaxAmount = 300000, Payments = "6,12,24,36" },
                };
                foreach (Loan loan in loans)
                {
                    context.Loans.Add(loan);
                }
                context.SaveChanges();


                var client1 = context.Clients.FirstOrDefault(c => c.Email == "vcoronado@gmail.com");
                var client2 = context.Clients.FirstOrDefault(c => c.Email == "santidipo@gmail.com");
                var client3 = context.Clients.FirstOrDefault(c => c.Email == "jacinfunes@gmail.com");

                if (client1 != null || client2 !=null || client3 != null)
                {
                    var loan1 = context.Loans.FirstOrDefault(l => l.Name == "Hipotecario");
                    if (loan1 != null)
                    {
                        var clientLoan1 = new ClientLoan
                        {
                            Amount = 400000,
                            Clientid = client1.Id,
                            LoanId = loan1.Id,
                            Payment = "60"
                        };
                        var clientLoan2 = new ClientLoan
                        {
                            Amount = 300000,
                            Clientid = client2.Id,
                            LoanId = loan1.Id,
                            Payment = "24"
                        };
                        var clientLoan3 = new ClientLoan
                        {
                            Amount = 500000,
                            Clientid = client3.Id,
                            LoanId = loan1.Id,
                            Payment = "12"
                        };
                        context.ClientLoans.Add(clientLoan1);
                        context.ClientLoans.Add(clientLoan2);
                        context.ClientLoans.Add(clientLoan3);
                    }

                    var loan2 = context.Loans.FirstOrDefault(l => l.Name == "Personal");
                    if (loan2 != null)
                    {
                        var clientLoan1 = new ClientLoan
                        {
                            Amount = 50000,
                            Clientid = client1.Id,
                            LoanId = loan2.Id,
                            Payment = "6"
                        };
                        var clientLoan2 = new ClientLoan
                        {
                            Amount = 40000,
                            Clientid = client2.Id,
                            LoanId = loan2.Id,
                            Payment = "24"
                        };
                        var clientLoan3 = new ClientLoan
                        {
                            Amount = 30000,
                            Clientid = client3.Id,
                            LoanId = loan2.Id,
                            Payment = "12"
                        };
                        context.ClientLoans.Add(clientLoan1);
                        context.ClientLoans.Add(clientLoan2);
                        context.ClientLoans.Add(clientLoan3);
                    }

                    var loan3 = context.Loans.FirstOrDefault(l => l.Name == "Automotriz");
                    if (loan3 != null)
                    {
                        var clientLoan1 = new ClientLoan
                        {
                            Amount = 100000,
                            Clientid = client1.Id,
                            LoanId = loan3.Id,
                            Payment = "36"
                        };
                        var clientLoan2 = new ClientLoan
                        {
                            Amount = 200000,
                            Clientid = client1.Id,
                            LoanId = loan3.Id,
                            Payment = "24"
                        };
                        var clientLoan3 = new ClientLoan
                        {
                            Amount = 300000,
                            Clientid = client1.Id,
                            LoanId = loan3.Id,
                            Payment = "12"
                        };
                        context.ClientLoans.Add(clientLoan1);
                        context.ClientLoans.Add(clientLoan2);
                        context.ClientLoans.Add(clientLoan3);
                    }

                    context.SaveChanges();

                }

            }

        }

    }
}