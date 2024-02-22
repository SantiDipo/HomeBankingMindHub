﻿using HomeBankingMindHub.Models.Enums;

namespace HomeBankingMindHub.Models
{
    public class Transaction
    {
        public long Id { get; set; }
        public TransactionType Type { get; set; }
        public double Amount {  get; set; }
        public string Description { get; set; }
        public DateTime DateTimed {  get; set; }
        public Account Account { get; set; }
        public long AccountId {  get; set; }

    }
}
