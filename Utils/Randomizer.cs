namespace HomeBankingMindHub.Utils
{
    public class Randomizer
    {
        public static string randomAccountNumber()
        {
            string accountNumber = "VIN-" + randomizer(0,99999999);
            return accountNumber;
        }

        public static int randomCvvNumber()
        {           
            return randomizer(0, 999);
        }

        public static string randomCardNumber()
        {
            string cardNumber= "";
            for (int i = 0; i < 3; i++)
            {
                cardNumber += randomizer(1000, 9999) + (i < 2 ? "-" : "");
            }
            return cardNumber;
        }

        public static int randomizer(int min, int max) 
        {
           Random random = new Random();
            return random.Next(min, max);
            
        }

    }
}
