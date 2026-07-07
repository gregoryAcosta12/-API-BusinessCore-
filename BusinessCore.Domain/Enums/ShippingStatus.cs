namespace BusinessCore.Domain.Enums
{
    public enum ShippingStatus
    {
        Pending = 0,
        Processing = 1,
        Shipped = 2,
        InTransit = 3,
        OutForDelivery = 4,
        Delivered = 5,
        Failed = 6,
        Returned = 7
    }
}