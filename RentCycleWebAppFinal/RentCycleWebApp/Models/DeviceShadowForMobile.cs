namespace RentCycleWebApp.Models
{
    public class UserPaymentForMobile
    {
        public int Id { get; set; }
        public int UserAccountId { get; set; }
        public String? Amount { get; set; }
        public int PaymentModeId { get; set; }
        public String? PaymentDate { get; set; }
        public String? PaymentTime { get; set; }
        public string? TransactionId { get; set; }
        public bool? TransactionStatus { get; set; }
        public string? PaymentModeName { get; set; }

        public virtual PaymentMode? PaymentMode { get; set; }
        public virtual UserAccount? UserAccount { get; set; }
    }

    public class UserPaymentSummaryForMobile
    {
        public List<UserPaymentForMobile>? UserPaymentForMobile { get; set; }
        public String? TotalRideAmount { get; set; }
        public String? TotalPaidAmount { get; set; }
        public String? TotalBalanceAmount { get; set; }
        
    }
}
