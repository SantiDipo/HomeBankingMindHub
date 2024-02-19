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

        }

    }
}