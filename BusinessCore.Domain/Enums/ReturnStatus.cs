namespace BusinessCore.Domain.Enums
{
    public enum ReturnStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2,
        InTransit = 3,
        Received = 4,
        Refunded = 5,
        Completed = 6
    }
}