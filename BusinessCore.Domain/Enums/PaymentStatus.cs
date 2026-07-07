namespace BusinessCore.Domain.Enums
{
    public enum PaymentStatus
    {
        Pending = 0,
        Paid = 1,
        Failed = 2,
        Refunded = 3,
        PartiallyRefunded = 4,
        Cancelled = 5,
        Processing = 6
    }
}