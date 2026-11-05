namespace TypesDemo.Models;

// Mutable class — reference semantics, reference equality (default).
public sealed class CustomerClass
{
    public string Name { get; set; } = "";
}

// Small mutable struct — ONLY to demonstrate copying. Prefer readonly structs in real code.
public struct Point
{
    public int X { get; set; }
    public int Y { get; set; }
}

// Positional record: compiler generates constructor, init properties, Equals/GetHashCode, ToString, Deconstruct.
public sealed record Money(decimal Amount, string Currency)
{
    // Validation: runs whenever a Money is created.
    public decimal Amount { get; init; } = Amount >= 0
        ? Amount
        : throw new ArgumentOutOfRangeException(nameof(Amount), "Amount cannot be negative.");

    public Money Add(Money other) =>
        other.Currency == Currency
            ? this with { Amount = Amount + other.Amount }
            : throw new InvalidOperationException($"Cannot add {other.Currency} to {Currency}.");
}

public enum OrderStatus { Pending, Paid, Shipped, Cancelled }

// Entity: has identity (Id) and a lifecycle → class, not record.
public sealed class Order
{
    public required int Id { get; init; }
    public required string Customer { get; init; }
    public OrderStatus Status { get; private set; } = OrderStatus.Pending;

    public void Pay()
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException($"Cannot pay an order that is {Status}.");
        Status = OrderStatus.Paid;
    }

    public string Describe() => Status switch
    {
        OrderStatus.Pending   => "Waiting for payment",
        OrderStatus.Paid      => "Paid, ready to ship",
        OrderStatus.Shipped   => "On its way",
        OrderStatus.Cancelled => "Cancelled",
        _ => throw new ArgumentOutOfRangeException(nameof(Status), Status, "Unknown status")
    };
}
