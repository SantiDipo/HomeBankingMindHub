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

        }

    }
}