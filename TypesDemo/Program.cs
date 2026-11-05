using TypesDemo.Models;

// ── 1. Class = reference type: two variables, ONE object ─────────────
var c1 = new CustomerClass { Name = "Asha" };
var c2 = c1;                       // copies the REFERENCE
c2.Name = "Ravi";
Console.WriteLine($"1. c1.Name = {c1.Name}");                       // ?

var c3 = new CustomerClass { Name = "Ravi" };
Console.WriteLine($"   c1 == c3 ? {c1 == c3}");                      // ?  same data, different objects

// ── 2. Struct = value type: assignment COPIES the data ───────────────
var p1 = new Point { X = 1, Y = 2 };
var p2 = p1;                       // copies the VALUE
p2.X = 99;
Console.WriteLine($"2. p1.X = {p1.X}, p1.Equals(p2) = {p1.Equals(p2)}");  // ?

// ── 3. Record = reference type with VALUE equality ───────────────────
var m1 = new Money(100m, "INR");
var m2 = new Money(100m, "INR");
Console.WriteLine($"3. m1 == m2 ? {m1 == m2}, same object? {ReferenceEquals(m1, m2)}");  // ?
Console.WriteLine($"   ToString: {m1}");

// ── 4. 'with' = non-destructive mutation (copy + change) ─────────────
var m3 = m1 with { Amount = 250m };
Console.WriteLine($"4. m1 = {m1.Amount}, m3 = {m3.Amount}");

// ── 5. Behaviour on a record + validation in constructor ─────────────
Console.WriteLine($"5. {m1.Add(m3)}");
try { m1.Add(new Money(5m, "USD")); }
catch (InvalidOperationException ex) { Console.WriteLine($"   Error: {ex.Message}"); }

// ── 6. Entity: class with identity, required + init, private setter ──
var order = new Order { Id = 42, Customer = "Asha" };
order.Pay();
// order.Id = 7;           // ❌ init-only: can only be set during construction
// order.Status = OrderStatus.Shipped;  // ❌ private set: must go through a method
Console.WriteLine($"6. Order {order.Id} is {order.Status}: {order.Describe()}");

// ── 7. Pattern matching ──────────────────────────────────────────────
object?[] things = [42, "hello", m1, null, new Point { X = 0, Y = 0 }];
foreach (var thing in things)
    Console.WriteLine($"7. {Classify(thing)}");

static string Classify(object? o) => o switch
{
    null                          => "null",
    int n when n > 10             => $"big int {n}",
    int n                         => $"int {n}",
    string { Length: > 3 } s      => $"long string '{s}'",
    Money { Currency: "INR" } m   => $"rupees {m.Amount:N2}",
    Point { X: 0, Y: 0 }          => "origin",
    _                             => $"something else ({o.GetType().Name})"
};
