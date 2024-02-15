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

        }
    }
}