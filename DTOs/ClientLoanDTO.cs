﻿namespace HomeBankingMindHub.Models
{
    public class ClientLoanDTO
    {
        public long Id { get; set; }
        public long LoanId { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public int Payments { get; set; }
        public ClientLoanDTO() { }
        public ClientLoanDTO(ClientLoan clientLoan)
        {
            Id = clientLoan.Id;
            LoanId = clientLoan.Loan.Id;
            Name = clientLoan.Loan.Name;
            Amount = clientLoan.Amount;
            Payments =int.Parse(clientLoan.Payment);
        }
        
    }
}
